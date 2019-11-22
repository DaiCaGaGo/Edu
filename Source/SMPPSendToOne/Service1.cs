using Inetlab.SMPP;
using Inetlab.SMPP.Common;
using Inetlab.SMPP.PDU;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Security.Authentication;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SMPPSendToOne
{
    public partial class Service1 : ServiceBase
    {
        #region pro
        public Uti uti = new Uti();
        public SmppClient client;
        public bool is_Disconect = false;
        public AppConfig _config = new AppConfig();
        public bool _stopRequested = false;
        public string strConnectSQL = "";
        public string _dbstatus;
        public string smppsendername = "";
        public MailSendError mailSendError;
        public int countThread = 1;
        #endregion
        #region SMPP
        public void createSMPP()
        {

            uti.ghilog("Info", "Call createSMPP");
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
                is_Disconect = true;
                uti.ghilog("Error", "SYS Error createSMPP " + ex.ToString());
            }
        }
        void client_evSubmitComplete(object sender, SubmitSmResp data)
        {
            uti.ghilog("Info", "Call client_evSubmitComplete");
            try
            {
                string msgLog = string.Format("SubmitSmResp received 2. Status: {0}, Message Id: {1}, Sequence:{2} "
                    , data.Status
                    , data.MessageId
                    , data.Sequence);
                uti.ghilog("Info", msgLog);
            }
            catch (ArgumentException x)
            {
                uti.ghilog("Info", "SYS Error client_evSubmitComplete");
            }
            catch (Exception ex)
            {
                uti.ghilog("Error", "SYS Error client_evSubmitComplete");
            }
        }
        void client_evConnected(object sender, bool bSuccess)
        {
            uti.ghilog("Info", "Call client_evConnected");
            try
            {
                if (bSuccess)
                {
                    uti.ghilog("Info", "SmppClient connected");
                    uti.ghilog("Info", string.Format("Binding SmppClient for SystemId: {0}", _config.SMPPUser));
                    client.BindAsync(_config.SMPPUser, _config.SMPPPass, ConnectionMode.Transmitter);
                }
                else
                {
                    uti.ghilog("Info", "SmppClient not connected");
                    is_Disconect = true;
                    if (!_stopRequested)
                    {
                        Thread.Sleep(5000);
                        Connect();
                    }
                }
            }
            catch (ArgumentException ex)
            {
                uti.ghilog("Error", "SYS Error client_evConnected " + ex.ToString());
                is_Disconect = true;
                if (!_stopRequested)
                {
                    Thread.Sleep(5000);
                    createSMPP();
                }
            }
            catch (Exception ex)
            {
                uti.ghilog("Error", "SYS Error client_evConnected " + ex.ToString());
                is_Disconect = true;
                if (!_stopRequested)
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
                        uti.ghilog("Info", "SmppClient bound");
                        uti.ghilog("Info", string.Format("Bind result : system is {0} with status {1}"
                            , data.SystemId
                            , data.Status.ToString()));
                        break;
                    default:

                        uti.ghilog("Info", string.Format("Bad status returned during Bind : {0} with status {1}"
                            , data.Command.ToString()
                            , data.Status.ToString()));

                        break;
                }
            }
            catch { }
        }
        void client_evDisconnected(object sender)
        {
            uti.ghilog("Info", "SmppClient disconnected");
            if (!_stopRequested)
            {
                uti.ghilog("Info", "ReConect SmppClient before disconnected");
                is_Disconect = true;
                reConect();
            }
        }
        private void rebind()
        {
            uti.ghilog("Info", "Call rebind");
            try
            {
                uti.ghilog("Info", string.Format("Binding SmppClient for SystemId: {0}", _config.SMPPUser));
                client.BindAsync(_config.SMPPUser, _config.SMPPPass, ConnectionMode.Transmitter);

            }
            catch
            {
                uti.ghilog("Error", "SYS Error rebind ");
                is_Disconect = true;
                if (!_stopRequested)
                {
                    Thread.Sleep(5000);
                    rebind();
                }
            }
        }
        private void Connect()
        {
            uti.ghilog("Info", "Call Connect");
            try
            {
                if (client.Status == ConnectionStatus.Closed)
                {
                    client.AddrNpi = Convert.ToByte("0");
                    client.AddrTon = Convert.ToByte("0");
                    //client.AddressRange = "ONESMSCLI1";
                    client.SystemType = _config.SMPPSystemtype;
                    client.EnabledSslProtocols = SslProtocols.None;
                    client.ConnectAsync(_config.SMPPServer, Convert.ToInt32(_config.SMPPPort));
                }
                else
                {
                    uti.ghilog("Info", string.Format("Binding SmppClient for SystemId: {0}", _config.SMPPUser));
                    client.BindAsync(_config.SMPPUser, _config.SMPPPass, ConnectionMode.Transmitter);
                }
            }
            catch
            {
                uti.ghilog("Error", "SYS Error Connect ");
            }
        }
        private void Disconnect()
        {
            uti.ghilog("Info", "Call Disconnect");
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
                    if (!_stopRequested)
                    {
                        client.evDisconnected -= client_evDisconnected;
                        client.Dispose();
                        createSMPP();
                    }
                }
            }
            catch
            {
                uti.ghilog("Error", "SYS Error Disconnect");
                reConect();
            }

        }
        private void UnBind()
        {
            try
            {
                uti.ghilog("Info", "Unbinding SmppClient");
                UnBindResp ubtrp = client.UnBind();
                switch (ubtrp.Status)
                {
                    case CommandStatus.ESME_ROK:
                        uti.ghilog("Info", "SmppClient unbound");
                        uti.ghilog("Info", "UnBind result with status " + ubtrp.Status);
                        break;
                    default:
                        uti.ghilog("Info", "Bad status returned during UnBind " + ubtrp.Command + " with status " + ubtrp.Status);
                        break;
                }
            }
            catch (ArgumentException x)
            {
                uti.ghilog("Error", "SYS Error UnBind");
            }
            catch (Exception ex)
            {
                uti.ghilog("Error", "SYS Error UnBind");
            }
            uti.ghilog("Info", "Call UnBind");

        }
        private void reConect()
        {
            uti.ghilog("Info", "Call reConect");
            try
            {
                if (!_stopRequested)
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
                uti.ghilog("Error", "SYS Error reConect");
                is_Disconect = true;
                if (!_stopRequested)
                {
                    Thread.Sleep(5000);
                    createSMPP();
                }
            }
        }
        private string sendMessage(string Braname, string To, string Message, out string MessageId)
        {
            MessageId = "0";
            try
            {
                IList<SubmitSmResp> resp = client.Submit(SMS.ForSubmit()
                    .ServiceType("")
                    .MessageInPayload(Message.Length)
                    .Text(Message.Trim())
                    .From(Braname.Trim())
                    .To(To.Trim())
                    .Coding(DataCodings.Default)
                    .DeliveryReceipt()
                    );
                if (resp.Count > 0)
                {
                    MessageId = resp[0].MessageId;
                    uti.ghilog("Info", string.Format(@"{0} {1} {2} {3} {4} {5}",
                                                              Braname,
                                                              To,
                                                              resp[0].Status,
                                                              resp[0].MessageId,
                                                              resp[0].Request,
                                                              Message));
                    return resp[0].Status.ToString();
                }
            }
            catch (ArgumentException x)
            {
                uti.ghilog("Error", "SYS Error sendMessage " + x.ToString());
            }
            catch (Exception ex)
            {
                uti.ghilog("Error", "SYS Error sendMessage " + ex.ToString());
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
            uti.ghilog("Info", "Call OnStart");
            try
            {
                _stopRequested = false;
                is_Disconect = false;
                smppsendername = _config.SMPPSendername;
                mailSendError = new MailSendError();
                #region Edit db
                strConnectSQL = @"Server=" + _config.DBInstanceName + ";database=" + _config.DbSource + "; UID=" + _config.DbUser + "; PWD=" + _config.DbPass + "; Connect Timeout=100; Min pool size=2; Max pool size=150";
                SqlConnection conn = new SqlConnection() { ConnectionString = strConnectSQL };
                try
                {
                    conn.Open();
                    SqlCommand com = new SqlCommand() { Connection = conn, CommandTimeout = 200 };
                    com.Parameters.Clear();
                    com.CommandText = @"begin try 
	                                        EXEC  ('alter table MT_QUEUE add count_retry int')
                                        end try

                                        begin catch
                                           print 'exists count_retry in MT_QUEUE'
                                        end catch 
                                        begin try 
	                                        EXEC  ('alter table MT_sent add count_retry int')
                                        end try

                                        begin catch
                                             print 'exists count_retry in MT_sent'
                                        end catch 
                                        ";
                    com.ExecuteNonQuery();
                    com.Parameters.Clear();
                }
                catch
                {
                }
                finally
                {
                    try
                    {
                        conn.Dispose();
                    }
                    catch { }
                }
                #endregion
                Thread.Sleep(10000);
                for (int i = 0; i < countThread; i++)
                {
                    int a = i;
                    Thread th = new Thread(() => run(a));
                    th.Start();
                }
            }
            catch (ArgumentException x)
            {
                uti.ghilog("Error", "SYS Error OnStart");
            }
            catch (Exception ex)
            {
                uti.ghilog("Error", "SYS Error OnStart");
            }
        }
        public void run(int indexThread)
        {
            uti.ghilog("Thread" + indexThread, "Start");
            while (!_stopRequested)
            {

                uti.ghilog("Thread" + indexThread, "start turn");
                try
                {
                    strConnectSQL = @"Server=" + _config.DBInstanceName + ";database=" + _config.DbSource + "; UID=" + _config.DbUser + "; PWD=" + _config.DbPass + "; Connect Timeout=100; Min pool size=2; Max pool size=150";
                    SqlConnection conn = new SqlConnection() { ConnectionString = strConnectSQL };
                    conn.Open();
                    SqlCommand com = new SqlCommand() { Connection = conn, CommandTimeout = 200 };
                    SqlDataAdapter da = new SqlDataAdapter();
                    DataTable tbl = new DataTable();
                    try
                    {
                        #region get data
                        tbl.Clear();
                        try
                        {
                            com.CommandText = "select top(500) id,info,user_id,send_time,count_retry from MT_QUEUE where id%" + countThread + "=" + indexThread;
                            da.SelectCommand = com;
                            da.Fill(tbl);
                        }
                        catch (Exception ex0)
                        {
                            uti.ghilog("DB", string.Format("Error in read sms in NH database. Mess: {0}", ex0));
                            mailSendError.OnSendError(string.Format("Error in read sms in NH database. Mess: {0}", ex0));
                        }
                        #endregion
                        uti.ghilog("Thread" + indexThread, "Get " + (tbl == null ? "0" : tbl.Rows.Count.ToString()) + " rows");
                        if (tbl != null && tbl.Rows.Count > 0)
                        {
                            foreach (DataRow row in tbl.Rows)
                            {
                                string id, info, user_id, telco, msg_type, status, sendtime;
                                id = ""; info = ""; user_id = ""; telco = ""; msg_type = ""; status = "";
                                int count_retry = 0;
                                byte cmes;
                                try
                                {
                                    #region get data item
                                    id = row["id"].ToString();
                                    info = row["info"].ToString();
                                    try
                                    {
                                        count_retry = Convert.ToInt32(row["count_retry"].ToString());
                                    }
                                    catch { count_retry = 0; }
                                    info = (info.Length > 459) ? info.Substring(0, 459) : info;
                                    user_id = "84" + uti.FilterPhone(row["user_id"].ToString());
                                    telco = uti.GetTelco(user_id);
                                    cmes = uti.CountMess(info, false);
                                    sendtime = row["send_time"].ToString();
                                    msg_type = "0";
                                    status = "";
                                    #endregion
                                    try
                                    {
                                        #region send smpp
                                        if (!is_Disconect)
                                        {
                                            string MessageId = "";
                                            status = sendMessage(smppsendername, user_id, info, out MessageId);
                                            count_retry++;
                                            #region check trạng thái
                                            if (status == "ESME_ROK")// trạng thái thành công smpp
                                            {
                                                msg_type = "1";//sent
                                                status = "Successfull_" + MessageId;
                                            }
                                            else
                                            {
                                                status = status + "_" + MessageId;
                                                msg_type = "0";//sent fail
                                                uti.ghilog("Thread" + indexThread, string.Format("Send message fail: id: {0} user_id: {1} info: {2} Status: {3}", id, user_id, info, status));
                                            }
                                            #endregion
                                            #region Lưu db
                                            try
                                            {
                                                long msgid = 0;
                                                try
                                                {
                                                    msgid = Convert.ToInt64(MessageId);
                                                }
                                                catch { }
                                                #region Thành công hoặc retry > 5
                                                if (msg_type == "1" || count_retry >= 5)
                                                {
                                                    com.CommandText = "SET IDENTITY_INSERT MT_SENT ON insert into MT_SENT (id,User_Id,Info,telco,Total_Msg,send_time,status,messageId,count_retry) values (@id,@User_Id,@Info,@telco,@Total_Msg,@send_time,@status,@messageid,@count_retry) delete from mt_queue where id=@id SET IDENTITY_INSERT MT_SENT OFF";
                                                    com.Parameters.Clear();
                                                    com.Parameters.AddWithValue("id", id);
                                                    com.Parameters.AddWithValue("User_Id", user_id);
                                                    com.Parameters.AddWithValue("Info", info);
                                                    com.Parameters.AddWithValue("telco", telco);
                                                    com.Parameters.AddWithValue("Total_Msg", cmes);
                                                    com.Parameters.AddWithValue("send_time", Convert.ToDateTime(sendtime));
                                                    com.Parameters.AddWithValue("status", status);
                                                    com.Parameters.AddWithValue("messageid", msgid);
                                                    com.Parameters.AddWithValue("count_retry", count_retry);
                                                    com.ExecuteNonQuery();
                                                    com.Parameters.Clear();
                                                }
                                                #endregion
                                                #region Lỗi cho call lại
                                                else
                                                {
                                                    com.CommandText = "update mt_queue set count_retry=@count_retry where id=@id";
                                                    com.Parameters.Clear();
                                                    com.Parameters.AddWithValue("count_retry", count_retry);
                                                    com.Parameters.AddWithValue("id", id);
                                                    com.ExecuteNonQuery();
                                                    com.Parameters.Clear();
                                                }
                                                #endregion
                                                uti.ghilog("Thread" + indexThread, string.Format("Sent:{7}|ID:{0}|{1}|{2}|{3}|SoBanTin:{4}|{5}|{6}|OnesmsStatus:{8}", id, "", user_id, telco, cmes, msg_type, info, msg_type, status));
                                            }
                                            catch (Exception ex2)
                                            {
                                                uti.ghilog("DB", string.Format("Error in process insert into mt_sent, delete from mt_queue:\nid {0}\nuser_id: {1}\nerror: {2}\nplease check database.", id, user_id, ex2));
                                                mailSendError.OnSendError(string.Format("Error in process insert into mt_sent, delete from mt_queue:\nid {0}\nuser_id: {1}\nerror: {2}\nplease check database.", id, user_id, ex2));
                                            }
                                            #endregion
                                            #region Re connect smpp
                                            if (!is_Disconect && (status == "SMPPCLIENT_NOCONN" || status == "SMPPCLIENT_UNBOUND"))
                                            {
                                                is_Disconect = true;
                                                reConect();
                                            }
                                            else if (!is_Disconect && (status == "SMPPCLIENT_RCVTIMEOUT"))
                                            {
                                                is_Disconect = true;
                                                rebind();
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            status = "SMPPCLIENT_NOCONN";
                                            uti.ghilog("Thread" + indexThread, string.Format("wait for connection"));
                                            Thread.Sleep(3000);
                                        }
                                        #endregion
                                    }
                                    catch (Exception ex1)
                                    {
                                        uti.ghilog("Thread" + indexThread, string.Format("Error in process send message: \nId {0} user_id: {1}\nError message :{2}", id, user_id, ex1.Message));
                                        mailSendError.OnSendError(string.Format("Error in process send message.\n-Id: {0} user_id: {1}\n-Error message:\n{2}", id, user_id, ex1.Message));
                                    }
                                }
                                catch (Exception exrrr)
                                {
                                    mailSendError.OnSendError(string.Format("Error in sms ID:{0},service_id:{1},info:{2},user_id:{3},\nerror: {4}", id, "", info, user_id, exrrr.Message));
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                    finally
                    {
                        try
                        {
                            conn.Dispose();
                            com.Dispose();
                            da.Dispose();
                            tbl.Dispose();
                        }
                        catch { }
                    }
                }
                catch (Exception ex)
                {
                    uti.ghilog("DB", ex.ToString());
                }
                uti.ghilog("Thread" + indexThread, "Stop turn");
                Thread.Sleep(2000);
            }
            uti.ghilog("Thread" + indexThread, "Stop");
        }
        protected override void OnStop()
        {
            _stopRequested = true;
            try
            {
                uti.ghilog("Info", "OneSMS Send Messages Service is stopping...");
                Thread.Sleep(30000);
                Disconnect();
            }
            catch { }
        }
    }
}
