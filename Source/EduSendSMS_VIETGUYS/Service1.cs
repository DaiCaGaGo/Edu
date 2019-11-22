using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EduSendSMS_VIETGUYS
{
    public partial class Service1 : ServiceBase
    {
        public bool is_stop = true;
        public string cpMa = "VIETGUYS";
        public List<Thread> threadRun = new List<Thread>();
        public int countThread = 1;
        private readonly vetguys.smsPortTypeClient vietguysApi = new vetguys.smsPortTypeClient();
        public Service1()
        {
            InitializeComponent();
            // vietguysApi.time = int.Parse(OneValueConfig.apiTimeout());
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
            List<int> kqretry = new List<int>()
            {
                -2,-3,-8,-13
            };
            while (!is_stop)
            {
                XuLy.ghilog("Thread" + indexThread, "start turn");
                try
                {
                    List<TIN_NHAN> lstData = new List<TIN_NHAN>();
                    lstData = tinNhanBO.getTinNhanChoGui(cpMa, countThread, indexThread);
                    if (lstData != null && lstData.Count > 0)
                    {
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
                                        string result = "";
                                        try
                                        {
                                            result = sendSMSGet(row.BRAND_NAME, XuLy.Add84(row.SDT_NHAN), row.NOI_DUNG_KHONG_DAU.Trim());
                                            int kq = 0;
                                            res_code = GetStringResultFromCode(result, out kq);
                                            if (kq > 0 || kq == -16) trang_thai = 1;
                                            else if (kqretry.Contains(kq) && (row.SEND_NUMBER==null || row.SEND_NUMBER<5))
                                                trang_thai = 2;
                                            else trang_thai = 3;// Dừng gửi
                                        }
                                        catch
                                        {
                                            trang_thai = 2;
                                            res_code = "UNDELIVERED|TIME OUT";
                                        }
                                    }
                                    else
                                    {
                                        res_code = "DELIVERED|NOT AVAILABLE";
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

        private string GetStringResultFromCode(string result,out int kq)
        {
            try
            {
                kq = Convert.ToInt32(result.Trim());
                if (kq > 0)
                    return "DELIVERED|"+ result;
                else return  "UNDELIVERED|"+ result ;
            }
            catch
            {
                kq = 1;
                return "DELIVERED|" + result;
            }
        }
        private string sendSMSGet(string brandName,string sdt,string sms)
        {
            try
            {
                var webRequest = WebRequest.Create(string.Format(@"https://cloudsms.vietguys.biz:4438/api/index.php?u=onesms&pwd=9avrh&from={0}&phone={1}&sms={2}&bid=1&type=&json=", brandName,sdt,sms));

                using (var response = webRequest.GetResponse())
                using (var content = response.GetResponseStream())
                using (var reader = new StreamReader(content))
                {
                    var strContent = reader.ReadToEnd();
                    return strContent;
                }
            }
            catch
            {
                return "-86";
            }
        }
    }
}
