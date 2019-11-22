using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EduSendSMS_VIETTEL_HN_API
{
    public partial class Service1 : ServiceBase
    {
        public bool is_stop = true;
        public string cpMa = "VIETTEL_HN";
        public List<Thread> threadRun = new List<Thread>();
        public int countThread = 1;
        private readonly vn.tinnhanthuonghieu.ams.CcApi vtApi = new vn.tinnhanthuonghieu.ams.CcApi();
        public Service1()
        {
            InitializeComponent();
            vtApi.Timeout = int.Parse(OneValueConfig.apiTimeout());
        }

        protected override void OnStart(string[] args)
        {
            XuLy.ghilog("SysSMS", "Call OnStart");
            try
            {
                is_stop = false;
                for (int i = 0; i < countThread; i++)
                {
                    int indexThread = i;
                    threadRun.Add(new Thread(() => runSend(indexThread)));
                }
                for (int i = 0; i < threadRun.Count; i++)
                {
                    threadRun[i].Start();
                }
            }
            catch (ArgumentException x)
            {
                XuLy.ghilog("SysSMS", "SYS Error OnStart");
            }
            catch (Exception ex)
            {
                XuLy.ghilog("SysSMS", "SYS Error OnStart");
            }
        }

        public void runSend(int indexThread)
        {
            TinNhanBO tinNhanBO = new TinNhanBO();
            XuLy.ghilog("Thread" + indexThread, "start");
            while (!is_stop)
            {
                XuLy.ghilog("Thread" + indexThread, "start turn");
                try
                {
                    List<TIN_NHAN> lstData = new List<TIN_NHAN>();
                    lstData = tinNhanBO.getTinNhanChoGui(cpMa, countThread, indexThread);

                    if (lstData != null && lstData.Count > 0)
                    {
                        long count_smsbrand_thbevandan = 0, count_smsbrand_thcsntto = 0, count_smsbrand_thnghiatan = 0;
                        if (lstData.Count(x => x.BRAND_NAME == "TH-BEVANDAN") > 0)
                        {
                            count_smsbrand_thbevandan = tinNhanBO.thongKeTongTinGuiQuaCP("TH-BEVANDAN", cpMa);
                            XuLy.ghilog("TH-BEVANDAN" , "Da gui "+ count_smsbrand_thbevandan.ToString());
                        }
                        if (lstData.Count(x => x.BRAND_NAME == "THCS NTTo") > 0)
                        {
                            count_smsbrand_thcsntto = tinNhanBO.thongKeTongTinGuiQuaCP("THCS NTTo", cpMa);
                            XuLy.ghilog("THCS NTTo", "Da gui " + count_smsbrand_thcsntto.ToString());
                        }
                        if (lstData.Count(x => x.BRAND_NAME == "TH-NGHIATAN") > 0)
                        {
                            count_smsbrand_thnghiatan = tinNhanBO.thongKeTongTinGuiQuaCP("TH-NGHIATAN", cpMa);
                            XuLy.ghilog("TH-NGHIATAN", "Da gui " + count_smsbrand_thnghiatan.ToString());
                        }
                        bool isHetQuy_thbevandan = false, isHetQuy_thcsntto = false, isHetQuy_thnghiatan = false;
                        long id_truong_thbevandan = 0, id_truong_thcsntto = 0, id_truong_thnghiatan = 0;
                        XuLy.ghilog("Thread" + indexThread, string.Format(@"Lay duoc {0} ban ghi", lstData.Count));
                        #region Vòng lặp
                        foreach (var row in lstData)
                        {
                            if (!is_stop)
                            {
                                string res_code = "";
                                short? trang_thai = null;
                                if (OneValueConfig.version() == "running")
                                {
                                    if (!string.IsNullOrEmpty(row.BRAND_NAME.Trim()) && !string.IsNullOrEmpty(row.NOI_DUNG_KHONG_DAU.Trim()) && !string.IsNullOrEmpty(row.SDT_NHAN.Trim()))
                                    {
                                        string userName = "", pass = "", cpcode = "";
                                        switch (row.BRAND_NAME)
                                        {
                                            case "TH-BEVANDAN":
                                                count_smsbrand_thbevandan = count_smsbrand_thbevandan + row.SO_TIN.Value;
                                                userName = "smsbrand_thbevandan"; pass = "THBEVANDAN@123"; cpcode = "THBEVANDAN";
                                                if (count_smsbrand_thbevandan > 20000)
                                                {
                                                    id_truong_thbevandan = row.ID_TRUONG;
                                                    isHetQuy_thbevandan = true;
                                                    trang_thai = 3;
                                                    res_code = "UNDELIVERED|HET QUY " + row.BRAND_NAME;
                                                }
                                                break;
                                            case "THCS NTTo":
                                                count_smsbrand_thcsntto = count_smsbrand_thcsntto + row.SO_TIN.Value;
                                                userName = "smsbrand_thcsntto"; pass = "THCSNTTO@123"; cpcode = "THCSNTTO";
                                                if (count_smsbrand_thcsntto > 20000)
                                                {
                                                    id_truong_thcsntto = row.ID_TRUONG;
                                                    isHetQuy_thcsntto = true;
                                                    trang_thai = 3;
                                                    res_code = "UNDELIVERED|HET QUY " + row.BRAND_NAME;
                                                }
                                                break;
                                            case "TH-NGHIATAN":
                                                count_smsbrand_thnghiatan = count_smsbrand_thnghiatan + row.SO_TIN.Value;
                                                userName = "smsbrand_thnghiatan"; pass = "THNGHIATAN@123"; cpcode = "THNGHIATAN";
                                                if (count_smsbrand_thnghiatan > 20000)
                                                {
                                                    id_truong_thnghiatan = row.ID_TRUONG;
                                                    isHetQuy_thnghiatan = true;
                                                    trang_thai = 3;
                                                    res_code = "UNDELIVERED|HET QUY " + row.BRAND_NAME;
                                                }
                                                break;

                                        }
                                        if (!string.IsNullOrEmpty(userName) && trang_thai == null)
                                        {

                                            vn.tinnhanthuonghieu.ams.result result = new vn.tinnhanthuonghieu.ams.result();
                                            try
                                            {
                                                result = vtApi.wsCpMt(userName, pass, cpcode, row.ID.ToString(), XuLy.Add84(row.SDT_NHAN), XuLy.Add84(row.SDT_NHAN), row.BRAND_NAME, "bulksms", row.NOI_DUNG_KHONG_DAU, "0");
                                                res_code = GetStringResultFromCode(result);
                                                if (res_code == "DELIVERED") trang_thai = 1;
                                                else trang_thai = 3;
                                            }
                                            catch
                                            {
                                                trang_thai = 2;
                                                res_code = "DELIVERED|TIME OUT";
                                            }
                                        }
                                        else
                                        {
                                            trang_thai = 3;
                                            res_code = "DELIVERED|CHUA CO TAI KHOAN";
                                        }
                                    }
                                    else
                                    {
                                        res_code = "UNDELIVERED|NOT AVAILABLE";
                                        trang_thai = 3;
                                    }
                                }
                                else
                                {
                                    res_code = "DELIVERED|TEST";
                                    trang_thai = 1;
                                }
                                #region Save
                                ResultEntity res = tinNhanBO.updateTrangThaiTinNhan(row.ID, row.ID_TRUONG, trang_thai.Value, res_code, null, true);
                                if (!res.Res)
                                {
                                    is_stop = true;
                                    XuLy.ghilog("DB", "Not save " + string.Format("ID:{0} Brandname:{1} SDT:{2} MSG:{3} Res:{4}", row.ID, row.BRAND_NAME, row.SDT_NHAN, row.NOI_DUNG_KHONG_DAU, res.Msg));
                                }
                                else
                                {
                                    XuLy.ghilog("Thread" + indexThread, string.Format("Send at: {0} ID:{1} Brandname:{2} SDT:{3} MSG:{4} Res:{5}", DateTime.Now.ToString("yyyyMMddhhmmss"), row.ID, row.BRAND_NAME, row.SDT_NHAN, row.NOI_DUNG_KHONG_DAU, res_code));
                                }
                                #endregion
                            }
                        }
                        #endregion
                        #region Đổi cp
                        if (isHetQuy_thbevandan && id_truong_thbevandan > 0)
                        {
                            tinNhanBO.doiCPViettel(id_truong_thbevandan, cpMa, "VNPT", "TH-BEVANDAN");
                        }
                        if (isHetQuy_thcsntto && id_truong_thcsntto > 0)
                        {
                            tinNhanBO.doiCPViettel(id_truong_thcsntto, cpMa, "VNPT", "THCS NTTo");
                        }
                        if (isHetQuy_thnghiatan && id_truong_thnghiatan > 0)
                        {
                            tinNhanBO.doiCPViettel(id_truong_thnghiatan, cpMa, "VNPT", "TH-NGHIATAN");
                        }
                        #endregion
                    }
                    else
                    {
                        XuLy.ghilog("Thread" + indexThread, "Lay duoc 0 ban ghi");
                        Thread.Sleep(2000);
                    }
                }
                catch (Exception ex)
                {
                    XuLy.ghilog("Error", ex.ToString());
                }
                XuLy.ghilog("Thread" + indexThread, "stop turn");
                Thread.Sleep(2000);
            }
            XuLy.ghilog("Thread" + indexThread, "stop");
        }

        protected override void OnStop()
        {
            is_stop = true;
        }

        private string GetStringResultFromCode(vn.tinnhanthuonghieu.ams.result result)
        {

            if (result.result1 == 1)
                return "DELIVERED";
            else if (result.message.Contains("DUPLICATE MESSAGE"))
                return "DELIVERED";
            return "UNDELIVERED|" + result.message;
        }
    }
}
