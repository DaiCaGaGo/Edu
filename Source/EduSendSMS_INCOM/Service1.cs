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

namespace EduSendSMS_INCOM
{
    public partial class Service1 : ServiceBase
    {
        public bool is_stop = true;
        public string cpMa = "INCOM";
        public List<Thread> threadRun = new List<Thread>();
        public int countThread = 1;
        private readonly Service_SendSMS2 IncomApi = new Service_SendSMS2();
        public Service1()
        {
            InitializeComponent();
            IncomApi.Timeout = int.Parse(OneValueConfig.apiTimeout());
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
                                        int result = 0;
                                        try
                                        {
                                            result = IncomApi.SendSMS(OneValueConfig.username(),
                                                 OneValueConfig.password(), XuLy.Add84(row.SDT_NHAN), row.NOI_DUNG_KHONG_DAU, row.BRAND_NAME,
                                                 row.BRAND_NAME, row.ID.ToString(), "0", "1", "1", "0", "0");
                                            res_code = GetStringResultFromCode(result);
                                            if (result == 0 || result == 399 || result == 304) trang_thai = 1;
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
                                    if (OneValueConfig.version() == "running")
                                    {
                                        IncomApi.SendSMS(OneValueConfig.username(),
                                                 OneValueConfig.password(), "84988094398", "DB error", row.BRAND_NAME,
                                                 row.BRAND_NAME, row.ID.ToString(), "0", "1", "1", "0", "0");
                                    }
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

        private static string GetStringResultFromCode(int code)
        {
            switch (code)
            {
                case 0:
                    return "DELIVERED";

                case 509:
                    return "TemplateFail";

                case 399:
                    return "MTLoop";

                case 398:
                    return "PartnerNotfound";

                case 397:
                    return "ProviderNotfound";

                case 396:
                    return "SessionNotfound";

                case 395:
                    return "IpNotRegisted";

                case 394:
                    return "PartnerNotfoundUser";

                case 393:
                    return "UserPassFail";

                case 392:
                    return "TelcoNotfound";

                case 359:
                    return "SessionNotExistOrInactive";

                case 360:
                    return "PhoneInBlaclist";

                case 357:
                    return "ServiceNotfoundOrInactive";

                case 356:
                    return "ZezoServiceId";

                case 253:
                    return "InsertConcentratorFail";

                case 304:
                    return "SmsLoop";

                default:
                    return code.ToString();
            }
        }
    }
}
