using Inetlab.SMPP;
using Inetlab.SMPP.Common;
using Inetlab.SMPP.PDU;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Authentication;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EduSendSMS_SOUTH
{
    public partial class Service1 : ServiceBase
    {
        #region pro
        public SmppClient client;
        public bool is_Stop = false;
        public bool is_Disconect = false;
        public string cpMa = "SOUTH";
        public List<Thread> threadRun = new List<Thread>();
        public int countThread = 1;
        public string ipserver = OneValueConfig.ipserver();
        public string port = OneValueConfig.port();
        public string user = OneValueConfig.username();
        public string pass = OneValueConfig.password();
        public string systemtype = OneValueConfig.systemtype();
        public string trantype = OneValueConfig.trantype();
        public string urlLog = OneValueConfig.urlLog();
        #endregion
        #region SMPP
        public void createSMPP()
        {
            XuLy.ghilog("SysSMS", "Call createSMPP");
            try
            {
                client = new SmppClient();
                client.Timeout = 4000;
                client.NeedEnquireLink = true;
                client.EnquireInterval = 5;
                client.SendSpeedLimit = 50;
                client.RaiseEventsInMainThread = true;
                client.evConnected += new Inetlab.SMPP.Common.ConnectedEventHandler(client_evConnected);
                client.evDisconnected += new Inetlab.SMPP.Common.DisconnectedEventHandler(client_evDisconnected);
                client.evBindComplete += new Inetlab.SMPP.Common.BindRespEventHandler(client_evBindComplete);
                client.evSubmitComplete += new Inetlab.SMPP.Common.SubmitSmRespEventHandler(client_evSubmitComplete);
                Connect();
            }
            catch (Exception ex)
            {
                XuLy.ghilog("SysSMS", "SYS Error createSMPP " + ex.ToString());
                is_Disconect = true;
                if (!is_Stop)
                {
                    Thread.Sleep(5000);
                    createSMPP();
                }
            }
        }
        void client_evSubmitComplete(object sender, SubmitSmResp data)
        {
            XuLy.ghilog("SysSMS", "Call client_evSubmitComplete");
            try
            {
                string msgLog = string.Format("SubmitSmResp received 2. Status: {0}, Message Id: {1}, Sequence:{2} "
                    , data.Status
                    , data.MessageId
                    , data.Sequence);
                XuLy.ghilog("SysSMS", msgLog);
            }
            catch (ArgumentException x)
            {
                XuLy.ghilog("SysSMS", "SYS Error client_evSubmitComplete");
            }
            catch (Exception ex)
            {
                XuLy.ghilog("SysSMS", "SYS Error client_evSubmitComplete");
            }
        }
        void client_evConnected(object sender, bool bSuccess)
        {
            XuLy.ghilog("SysSMS", "Call client_evConnected");
            try
            {
                if (bSuccess)
                {
                    XuLy.ghilog("SysSMS", "SmppClient connected");
                    XuLy.ghilog("SysSMS", string.Format("Binding SmppClient for SystemId: {0}", user));
                    client.BindAsync(user, pass, ConnectionMode.Transmitter);
                }
                else
                {
                    XuLy.ghilog("SysSMS", "SmppClient not connected");
                    is_Disconect = true;
                    if (!is_Stop)
                    {
                        Thread.Sleep(5000);
                        Connect();
                    }
                }
            }
            catch (ArgumentException ex)
            {
                XuLy.ghilog("SysSMS", "SYS Error client_evConnected " + ex.ToString());
                is_Disconect = true;
                if (!is_Stop)
                {
                    Thread.Sleep(5000);
                    createSMPP();
                }
            }
            catch (Exception ex)
            {
                XuLy.ghilog("SysSMS", "SYS Error client_evConnected " + ex.ToString());
                is_Disconect = true;
                if (!is_Stop)
                {
                    Thread.Sleep(5000);
                    createSMPP();
                }
            }
        }
        void client_evBindComplete(object sender, Inetlab.SMPP.PDU.BindResp data)
        {
            try
            {
                is_Disconect = false;
                switch (data.Status)
                {
                    case CommandStatus.ESME_ROK:
                        XuLy.ghilog("SysSMS", "SmppClient bound");
                        XuLy.ghilog("SysSMS", string.Format("Bind result : system is {0} with status {1}"
                            , data.SystemId
                            , data.Status.ToString()));
                        break;
                    default:

                        XuLy.ghilog("SysSMS", string.Format("Bad status returned during Bind : {0} with status {1}"
                            , data.Command.ToString()
                            , data.Status.ToString()));

                        break;
                }
            }
            catch { }
        }
        void client_evDisconnected(object sender)
        {
            XuLy.ghilog("SysSMS", "SmppClient disconnected");
            if (!is_Stop)
            {
                XuLy.ghilog("SysSMS", "ReConect SmppClient before disconnected");
                is_Disconect = true;
                reConect();
            }
        }
        private void rebind()
        {
            XuLy.ghilog("SysSMS", "Call rebind");
            try
            {
                XuLy.ghilog("SysSMS", string.Format("Binding SmppClient for SystemId: {0}", user));
                client.BindAsync(user, pass, ConnectionMode.Transmitter);

            }
            catch
            {
                XuLy.ghilog("SysSMS", "SYS Error rebind ");
                is_Disconect = true;
                if (!is_Stop)
                {
                    Thread.Sleep(5000);
                    rebind();
                }
            }
        }
        private void Connect()
        {
            XuLy.ghilog("SysSMS", "Call Connect");
            try
            {
                if (client.Status == ConnectionStatus.Closed)
                {
                    client.AddrNpi = Convert.ToByte("1");
                    client.AddrTon = Convert.ToByte("1");
                    client.SystemType = systemtype;
                    client.EnabledSslProtocols = SslProtocols.None;
                    client.ConnectAsync(ipserver, Convert.ToInt32(port));
                }
                else
                {
                    XuLy.ghilog("SysSMS", string.Format("Binding SmppClient for SystemId: {0}", user));
                    client.BindAsync(user, pass, ConnectionMode.Transmitter);
                }
            }
            catch
            {
                XuLy.ghilog("SysSMS", "SYS Error Connect ");
            }
        }
        private void Disconnect()
        {
            XuLy.ghilog("SysSMS", "Call Disconnect");
            try
            {
                if (client.Status == ConnectionStatus.Bound)
                {
                    UnBind();
                }

                if (client.Status == ConnectionStatus.Open)
                {
                    client.Disconnect();
                }
                else
                {
                    if (!is_Stop)
                    {
                        client.evDisconnected -= client_evDisconnected;
                        client.Dispose();
                        createSMPP();
                    }
                }
            }
            catch
            {
                XuLy.ghilog("SysSMS", "SYS Error Disconnect");
                reConect();
            }

        }
        private void UnBind()
        {
            try
            {
                XuLy.ghilog("SysSMS", "Unbinding SmppClient");
                UnBindResp ubtrp = client.UnBind();
                switch (ubtrp.Status)
                {
                    case CommandStatus.ESME_ROK:
                        XuLy.ghilog("SysSMS", "SmppClient unbound");
                        XuLy.ghilog("SysSMS", "UnBind result with status " + ubtrp.Status);
                        break;
                    default:
                        XuLy.ghilog("SysSMS", "Bad status returned during UnBind " + ubtrp.Command + " with status " + ubtrp.Status);
                        break;
                }
            }
            catch (ArgumentException x)
            {
                XuLy.ghilog("SysSMS", "SYS Error UnBind");
            }
            catch (Exception ex)
            {
                XuLy.ghilog("SysSMS", "SYS Error UnBind");
            }
            XuLy.ghilog("SysSMS", "Call UnBind");

        }
        private void reConect()
        {
            XuLy.ghilog("SysSMS", "Call reConect");
            try
            {
                if (!is_Stop)
                {
                    if (client == null)
                    {
                        createSMPP();
                    }
                    else
                    {
                        Disconnect();
                    }
                }
            }
            catch
            {
                XuLy.ghilog("SysSMS", "SYS Error reConect");
                is_Disconect = true;
                if (!is_Stop)
                {
                    Thread.Sleep(5000);
                    createSMPP();
                }
            }
        }
        private string sendMessage(string Braname, string To, string Message, int indexThread)
        {
            try
            {
                IList<SubmitSmResp> resp = client.Submit(SMS.ForSubmit()
                    .ServiceType("")
                    //.MessageInPayload(Message.Length)
                    .Text(Message.Trim())
                    .From(Braname.Trim())
                    .To(To.Trim())
                    .Coding(DataCodings.Default)
                    .DeliveryReceipt()
                    );
                if (resp.Count > 0)
                {
                    XuLy.ghilog("Thread" + indexThread, string.Format(@"{0} {1} {2} {3} {4}",
                                                              Braname,
                                                              To,
                                                              resp[0].Status,
                                                              resp[0].MessageId,
                                                              resp[0].Request));
                    return resp[0].Status.ToString();
                }
            }
            catch (ArgumentException x)
            {
                XuLy.ghilog("Thread" + indexThread, "SYS Error sendMessage " + x.ToString());
            }
            catch (Exception ex)
            {
                XuLy.ghilog("Thread" + indexThread, "SYS Error sendMessage " + ex.ToString());
            }
            return "";
        }
        #endregion
        public Service1()
        {
            InitializeComponent();
            createSMPP();
        }

        protected override void OnStart(string[] args)
        {
            XuLy.ghilog("SysSMS", "Call OnStart");
            try
            {
                is_Stop = false;
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

        protected override void OnStop()
        {
            is_Stop = true;
        }

        public void runSend(int indexThread)
        {
            TinNhanBO tinNhanBO = new TinNhanBO();
            XuLy.ghilog("Thread" + indexThread, "start");
            while (!is_Stop)
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
                            if (!is_Stop)
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
                                            if (!is_Disconect)
                                            {
                                                result = sendMessage(row.BRAND_NAME, XuLy.Add84(row.SDT_NHAN), row.NOI_DUNG_KHONG_DAU, indexThread);
                                                if (!is_Disconect && (result == "SMPPCLIENT_NOCONN" || result == "SMPPCLIENT_UNBOUND"))
                                                {
                                                    res_code = "DELIVERED|" + result;
                                                    is_Disconect = true;
                                                    trang_thai = 2;
                                                    reConect();
                                                }
                                                else if (!is_Disconect && result == "SMPPCLIENT_RCVTIMEOUT")
                                                {
                                                    res_code = "DELIVERED|" + result;
                                                    trang_thai = 2;
                                                    is_Disconect = true;
                                                    rebind();
                                                }
                                                else if (result == "ESME_ROK")
                                                {
                                                    res_code = "DELIVERED";
                                                    trang_thai = 1;
                                                }
                                                else
                                                {
                                                    res_code = "DELIVERED|" + result;
                                                    trang_thai = 3;
                                                }
                                            }
                                            else
                                            {
                                                res_code = "DELIVERED|SMPPCLIENT_NOCONN";
                                                trang_thai = 2;
                                                Thread.Sleep(5000);
                                            }
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
                                    is_Stop = true;
                                    XuLy.ghilog("DB", "Not save " + string.Format("ID:{0} Brandname:{1} SDT:{2} MSG:{3} Res:{4}", row.ID, row.BRAND_NAME, row.SDT_NHAN, row.NOI_DUNG_KHONG_DAU, res.Msg));
                                    if (OneValueConfig.version() == "running")
                                    {
                                        sendMessage(row.BRAND_NAME, "84988094398", "DB error", indexThread);
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
            is_Stop = true;
            Thread.Sleep(10000);
            Disconnect();
        }
    }
}
