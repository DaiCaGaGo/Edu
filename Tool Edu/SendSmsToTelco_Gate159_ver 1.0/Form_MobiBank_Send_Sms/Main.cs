using Common;
using Entity;
using log4net;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Threading;
using System.Web.Script.Serialization;
using System.Security;
namespace Form_MobiBank_Send_Sms
{
    public partial class MobiBankSendSms : Form
    {
        #region Variables

        private BackgroundWorker _bgSendSms;
        private readonly ILog Log = LogManager.GetLogger("LogMobiBankSendSms");
        private readonly Phone Phone = new Phone();
        private System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer();
        List<string> listsendsms = new List<string>();
        private ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };

        #endregion Variables

        public MobiBankSendSms()
        {
            InitializeComponent();

            _timer.Tick += _timer_Tick;

            _bgSendSms = new BackgroundWorker();
            _bgSendSms.DoWork += _bgSendSms_DoWork;
            _bgSendSms.RunWorkerCompleted += _bgSendSms_RunWorkerCompleted;
           
            var log = String.Format("Application started at: {0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            SetListBox(log + Environment.NewLine);
            Log.Info(log);
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            if (!_bgSendSms.IsBusy)
                _bgSendSms.RunWorkerAsync();
        }

        private void _bgSendSms_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //
        }

        #region _bgSendSms_DoWork

        private void _bgSendSms_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {                
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: "mobibank_task_queue",
                                             durable: true,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null);
                       
                        while (true)
                        {
                            var data = channel.BasicGet("mobibank_task_queue", true);
                            if (data == null)
                                break;
                            var body = data.Body;
                            var message = Encoding.UTF8.GetString(body);
                            List<PartnerSms> listSms = JsonConvert.DeserializeObject<List<PartnerSms>>(message);

                            foreach (var sms in listSms)
                            {
                                var senderName = sms.SenderName;
                                var portName = "";
                                string code = "UNDELIVERED";
                                var telCo = Phone.GetTelco(sms.Phone);
                                if (listsendsms.IndexOf(sms.SmsId.ToString()) > 0)
                                    continue;
                               
                                switch (telCo)
                                {
                                    case "Viettel":
                                        if (!String.IsNullOrEmpty(sms.BrdViettel))
                                            senderName = sms.BrdViettel;
                                        portName = sms.Viettel;
                                        break;

                                    case "EVN":
                                        if (!String.IsNullOrEmpty(sms.BrdViettel))
                                            senderName = sms.BrdViettel;
                                        portName = sms.Evn;
                                        break;

                                    case "VNM":
                                        if (!String.IsNullOrEmpty(sms.BrdVnm))
                                            senderName = sms.BrdVnm;
                                        portName = sms.Vnm;
                                        break;

                                    case "GPC":
                                        if (!String.IsNullOrEmpty(sms.BrdGpc))
                                            senderName = sms.BrdGpc;
                                        portName = sms.Gpc;
                                        break;

                                    case "Gtel":
                                        if (!String.IsNullOrEmpty(sms.BrdGtel))
                                            senderName = sms.BrdGtel;
                                        portName = sms.Gtel;
                                        break;

                                    case "VMS":
                                        if (!String.IsNullOrEmpty(sms.BrdVms))
                                            senderName = sms.BrdVms;
                                        portName = sms.Vms;
                                        break;

                                    case "Sfone":
                                        if (!String.IsNullOrEmpty(sms.BrdSfone))
                                            senderName = sms.BrdSfone;
                                        portName = sms.Sfone;
                                        break;
                                }

                                try
                                {
                                    if (String.IsNullOrEmpty(telCo))
                                        code = "PHONE INVALID";
                                    else if (sms.PartnerId == 45 && sms.Sms.ToLower().Contains("vccspam"))
                                        code = "SMS SPAM";
                                    else
                                    {
                                        if (ConfigurationManager.AppSettings["version"] == "running")
                                        {
                                            //int result = MobiBankApi.SendSMS(ConfigurationManager.AppSettings["username"],
                                            //ConfigurationManager.AppSettings["password"], sms.Phone, sms.Sms, senderName,
                                            //senderName, "0", "0", "1", "1", "0", "0");

                                            //code = GetStringResultFromCode(result);

                                            string url = ConfigurationManager.AppSettings["urlRequestMobibank"];

                                            string pass = ConfigurationManager.AppSettings["passwordMobibank"];
                                            string statusCode = "", responseMess = "";
                                            GetResponseFromUrl(sms.SmsId.ToString(), senderName, sms.Phone, sms.Sms,
                                                ConfigurationManager.AppSettings["usernameMobibank"],
                                                pass,
                                                url, ref statusCode, ref responseMess);
                                            code = responseMess;
                                            var jss = new JavaScriptSerializer();
                                            var table = jss.Deserialize<dynamic>(responseMess);


                                            code = table["message"].ToString().Replace("Success", "DELIVERED");
                                            SetListBox("SMSID "+ sms.SmsId.ToString()+":"+ code + Environment.NewLine);
                                         
                                            listsendsms.Add(sms.SmsId.ToString());
                                           // sms.
                                        }
                                        else
                                            code = "DELIVERED|TEST";
                                    }

                                    SmsStatus status = new SmsStatus()
                                    {
                                        sms_id = sms.SmsId,
                                        partner_id = sms.PartnerId,
                                        sender_name = senderName,
                                        sms_content = sms.Sms,
                                        is_unicode = sms.IsUnicode,
                                        is_flash = sms.IsFlash,
                                        count_mes = sms.CountMes,
                                        port_name = portName,
                                        phone = sms.Phone,
                                        tel_code = telCo,
                                        time_send = sms.TimeSend,
                                        time_reveive = DateTime.Now,
                                        sent_code = code,
                                        customer_sms_id = sms.CustomerSmsId
                                    };

                                    PublishQueue(channel, "task_queue_success", status);

                                    var log =
                                        String.Format(
                                            @"Send to: {0}, smsId: {1}, sender: {2}, timesend: {3}, code: {4}, timelog: {5}",
                                            status.phone, status.sms_id, status.sender_name, status.time_send,
                                            status.sent_code, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

                                    SetListBox(log + Environment.NewLine);
                                    Log.Info(log);
                                }
                                catch (Exception ex)
                                {
                                    SetListBox(ex.ToString() + Environment.NewLine);
                                    SmsStatus status = new SmsStatus()
                                    {
                                        sms_id = sms.SmsId,
                                        partner_id = sms.PartnerId,
                                        sender_name = senderName,
                                        sms_content = sms.Sms,
                                        is_unicode = sms.IsUnicode,
                                        is_flash = sms.IsFlash,
                                        count_mes = sms.CountMes,
                                        port_name = portName,
                                        phone = sms.Phone,
                                        tel_code = telCo,
                                        time_send = sms.TimeSend,
                                        time_reveive = DateTime.Now,
                                        sent_code = ex.Message,
                                        customer_sms_id = sms.CustomerSmsId
                                    };

                                    PublishQueue(channel, "task_queue_error", status);

                                    var log = String.Format(@"Send to: {0}, smsId: {1}, sender: {2}, timesend: {3}, code: {4}, timelog: {5}",
                                    status.phone, status.sms_id, status.sender_name, status.time_send, status.sent_code, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

                                    SetListBox(log + Environment.NewLine);
                                    Log.Info(log);
                                }
                            }
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                var log = String.Format(@"_bgSendSms_DoWork error at: {0}, error: {1}",
                    DateTime.Now.ToString("dd/MM/yyyy HH:ss:mm"), ex.Message);

                SetListBox(log + Environment.NewLine);
                Log.Error(log);
            }
        }

        #endregion _bgSendSms_DoWork
        private void GetResponseFromUrl(string rqid, string brandname, string msisdn, string message,
           string authuser, string authpass, string url, ref string statusCode, ref string responseMess)
        {
            try
            {

                // Create a request for the URL. 		
                System.Net.WebRequest request = System.Net.WebRequest.Create(url);
                request.ContentType = "application/json;charset=UTF-8";
                request.Method = "POST";
                request.Credentials = CredentialCache.DefaultCredentials;
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {

                    Dictionary<string, string> values = new Dictionary<string, string>();
                    values.Add("tranId", rqid);
                    values.Add("brandName", brandname);
                    values.Add("phone", msisdn);
                    values.Add("mess", (message));
                    values.Add("user", authuser);
                    values.Add("pass", authpass);
                    values.Add("dataEncode", "0");
                    values.Add("sendTime", "");

                    string json = JsonConvert.SerializeObject(values);
                    streamWriter.Write(json);
                    streamWriter.Close();
                }
                string result = "";
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    // Get the response stream
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    responseMess = reader.ReadToEnd();
                    SetListBox("MSGID " + rqid + ":" + responseMess + Environment.NewLine);
                    //  XmlDocument doc = new XmlDocument();
                    // doc.LoadXml(result);
                    //XmlNodeList n = doc.GetElementsByTagName("code")[0].InnerText;
                    // responseMess = doc.GetElementsByTagName("message")[0].InnerText.Replace("Success", "DELIVERED");
                    response.Close();
                    reader.Close();
                }

                //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //// Display the status.
                //statusCode = response.StatusDescription;
                //// Get the stream containing content returned by the server.
                //Stream dataStream = response.GetResponseStream();
                //// Open the stream using a StreamReader for easy access.
                //StreamReader reader = new StreamReader(dataStream);
                //// Read the content.
                //responseMess = reader.ReadToEnd();

                //// Cleanup the streams and the response.
                //reader.Close();
                //dataStream.Close();
                //response.Close();
                /*string result = "";
                StringBuilder datas = new StringBuilder();
                datas.Append("<message>");
                datas.Append("<user>").Append(authuser).Append("</user>");
                datas.Append("<pass>").Append(authpass).Append("</pass>");
                datas.Append("<tranId>").Append(rqid).Append("</tranId>");
                datas.Append("<brandName>").Append(brandname).Append("</brandName>");
                datas.Append("<phone>").Append(msisdn).Append("</phone>");
                datas.Append("<mess>").Append(SecurityElement.Escape(message)).Append("</mess>");
                datas.Append("</message>");
                
                byte[] buffer = Encoding.ASCII.GetBytes(datas.ToString());
                HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                request.Method = "POST";
                request.Timeout = 15000;
                request.ContentType = "application/xml";
                request.ContentLength = buffer.Length;
                Stream PostData = request.GetRequestStream();
                PostData.Write(buffer, 0, buffer.Length);
                PostData.Close();
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    // Get the response stream
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    result = reader.ReadToEnd();
                    SetListBox("MSGID " + rqid+":"+result + Environment.NewLine);
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(result);
                    //XmlNodeList n = doc.GetElementsByTagName("code")[0].InnerText;
                    responseMess = doc.GetElementsByTagName("desc")[0].InnerText.Replace("Success", "DELIVERED");
                    response.Close();
                    reader.Close();
                }
                System.Threading.Thread.Sleep(50);
                */
            }
            catch (Exception)
            {
                responseMess = "";
            }
        }

        /// <summary>
        /// Publishes the queue.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="status">The status.</param>
        private void PublishQueue(IModel channel, string queueName, SmsStatus status)
        {
            channel.QueueDeclare(queue: queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

            var smsStatus = JsonConvert.SerializeObject(status);
            var bodyStatus = Encoding.UTF8.GetBytes(smsStatus);

            var properties = channel.CreateBasicProperties();
            properties.SetPersistent(true);

            channel.BasicPublish(exchange: "",
                routingKey: queueName,
                basicProperties: properties,
                body: bodyStatus);
        }

        #region btnStart_Click

        /// <summary>
        /// Handles the Click event of the btnStart control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            _timer.Interval = int.Parse(ConfigurationManager.AppSettings["timeDelay"]);
         
            var log = String.Format("Thread started at: {0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            SetListBox(log + Environment.NewLine);
            Log.Info(log);

            _timer.Start();
            btnStop.Enabled = true;
            btnStart.Enabled = false;
        }

        #endregion btnStart_Click

        #region btnStop_Click

        /// <summary>
        /// Handles the Click event of the btnStop control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnStop_Click(object sender, EventArgs e)
        {
            _timer.Stop();

            if (!_bgSendSms.IsBusy)
            {
                var logAppStop = String.Format("Application stoped at: {0}!",
                    DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                SetListBox(logAppStop + Environment.NewLine);
                Log.Info(logAppStop);
                
                btnStop.Enabled = false;
                btnStart.Enabled = true;
            }
            else
                SetListBox("Thread is running! Please try again." + Environment.NewLine);
        }

        #endregion btnStop_Click

        #region btnClearLog_Click

        /// <summary>
        /// Handles the Click event of the btnClearLog control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnClearLog_Click(object sender, EventArgs e)
        {
            rtbLog.Clear();
        }

        #endregion btnClearLog_Click

        #region Delegate Set Text Log

        private delegate void SetListBoxDelegate(string text);

        private void SetListBox(string text)
        {
            if (!InvokeRequired)
            {
                if (rtbLog.Lines.Count() > 100)
                    rtbLog.Clear();
                rtbLog.AppendText(text);
                rtbLog.ScrollToCaret();
            }
            else
                Invoke(new SetListBoxDelegate(SetListBox), text);
        }

        #endregion Delegate Set Text Log

        #region GetCodeReturnMobiBank

        /// <summary>
        /// Gets the string result from code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        private static string GetStringResultFromCode(int code)
        {
            switch (code)
            {
                case 1:
                    return "DELIVERED";
               
                default:
                    return code.ToString();
            }
        }

        #endregion GetCodeReturnMobiBank
    }
}