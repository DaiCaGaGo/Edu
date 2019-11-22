using Common;
using log4net;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Entity;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Form_Vivas_Send_Sms
{
    public partial class Main : Form
    {
        #region Variables

        private static BackgroundWorker _bgSendSms;
        private static readonly ILog Log = LogManager.GetLogger("LogVivasSendSms");
        private static readonly Phone Phone = new Phone();
        private static readonly Encryption Encrypt = new Encryption();
        private static string _cookies = "";
        private static Common.WebRequest _rq;
        private Timer _timer = new Timer();

        private ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };

        #endregion Variables

        public Main()
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

        /// <summary>
        /// Handles the RunWorkerCompleted event of the _bgSendSms control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        private void _bgSendSms_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //
        }

        /// <summary>
        /// Handles the DoWork event of the _bgSendSms control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        private void _bgSendSms_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {                
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: "vivas_task_queue",
                                             durable: true,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null);

                        while (true)
                        {
                            var data = channel.BasicGet("vivas_task_queue", true);
                            if (data == null)
                                break;
                            var body = data.Body;
                            var message = Encoding.UTF8.GetString(body);
                            List<PartnerSms> listSms = JsonConvert.DeserializeObject<List<PartnerSms>>(message);

                            foreach (var sms in listSms)
                            {
                                var portName = "";
                                string code = "UNDELIVERED";
                                var telCo = Phone.GetTelco(sms.Phone);
                                var senderName = sms.SenderName;

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
                                            string resms =
                                        sms.Sms.Replace(Environment.NewLine, " ")
                                            .Replace("\n", "")
                                            .Replace("\r", "")
                                            .Replace("·", ".");

                                            resms = StringTool.RemoveSign4VietnameseString(resms);

                                            string alsms =
                                                resms.Replace("&", "&amp;")
                                                    .Replace("<", "&lt;")
                                                    .Replace(">", "&gt;")
                                                    .Replace("'", "&apos;");

                                            alsms = WebUtility.HtmlEncode(alsms);

                                            const string method = "POST";

                                            string requestid = ConfigurationManager.AppSettings["headid"] + sms.SmsId;
                                            string sendtime = DateTime.Now.ToString("yyyyMMddHHmmss");
                                            string inputchecksum =
                                                String.Format(
                                                    "username={0}&password={1}&brandname={2}&sendtime={3}&msgid={4}&msg={5}&msisdn={6}&sharekey={7}",
                                                    ConfigurationManager.AppSettings["username"],
                                                    Encrypt.GetShaHash(SHA1.Create(), ConfigurationManager.AppSettings["password"]),
                                                    senderName, sendtime, requestid, resms,
                                                    sms.Phone, ConfigurationManager.AppSettings["sharekey"]);
                                            using (MD5 md5 = MD5.Create())
                                            {
                                                inputchecksum = Encrypt.GetMd5Hash(md5, inputchecksum);
                                            }

                                            var dataSendSms = String.Format(
                                                "<RQST><REQID>{0}</REQID><BRANDNAME>{1}</BRANDNAME><TEXTMSG>{2}</TEXTMSG><SENDTIME>{3}</SENDTIME><TYPE>{4}</TYPE><DESTINATION><MSGID>{0}</MSGID><MSISDN>{5}</MSISDN><CHECKSUM>{6}</CHECKSUM></DESTINATION></RQST>",
                                                requestid, senderName.Replace("&", "&amp;"), alsms, sendtime, "1", sms.Phone,
                                                inputchecksum);
                                            _rq = new Common.WebRequest(ConfigurationManager.AppSettings["urlsend"], method, dataSendSms, _cookies);
                                            string respone = _rq.GetResponse();

                                            code = GetStringResultFromCode(respone);

                                            if (code == "20") // request bị từ chối vì chưa đăng nhập hoặc mất session.
                                            {
                                                var dataLogIn = String.Format(
                                                    "<RQST><USERNAME>{0}</USERNAME><PASSWORD>{1}</PASSWORD></RQST>",
                                                    ConfigurationManager.AppSettings["username"],
                                                    Encrypt.GetShaHash(SHA1.Create(), ConfigurationManager.AppSettings["password"]));
                                                _rq = new Common.WebRequest(ConfigurationManager.AppSettings["urllogin"], method, dataLogIn);
                                                string responeLogIn = _rq.GetResponseCookie();
                                                string statusLogIn = GetStringResultFromCode(responeLogIn);
                                                if (statusLogIn == "0")
                                                {
                                                    _cookies = _rq._header;
                                                }

                                                dataSendSms =
                                                String.Format(
                                                    "<RQST><REQID>{0}</REQID><BRANDNAME>{1}</BRANDNAME><TEXTMSG>{2}</TEXTMSG><SENDTIME>{3}</SENDTIME><TYPE>{4}</TYPE><DESTINATION><MSGID>{0}</MSGID><MSISDN>{5}</MSISDN><CHECKSUM>{6}</CHECKSUM></DESTINATION></RQST>",
                                                    requestid, senderName.Replace("&", "&amp;"), alsms, sendtime, "1", sms.Phone,
                                                    inputchecksum);
                                                _rq = new Common.WebRequest(ConfigurationManager.AppSettings["urlsend"], method, dataSendSms, _cookies);
                                                string responeReTry = _rq.GetResponse();

                                                code = GetStringResultFromCode(responeReTry);
                                            }

                                            if (code == "0")
                                                code = "DELIVERED";
                                            else
                                                code = code + "|UNDELIVERED";
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

                                    channel.QueueDeclare(queue: "task_queue_success",
                                                durable: true,
                                                exclusive: false,
                                                autoDelete: false,
                                                arguments: null);

                                    var smsStatus = JsonConvert.SerializeObject(status);
                                    var bodyStatus = Encoding.UTF8.GetBytes(smsStatus);

                                    var properties = channel.CreateBasicProperties();
                                    properties.SetPersistent(true);

                                    channel.BasicPublish(exchange: "",
                                        routingKey: "task_queue_success",
                                        basicProperties: properties,
                                        body: bodyStatus);

                                    var log = String.Format(@"Send to: {0}, smsId: {1}, sender: {2}, timesend: {3}, code: {4}, timelog: {5}",
                                    status.phone, status.sms_id, status.sender_name, status.time_send, status.sent_code, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

                                    SetListBox(log + Environment.NewLine);
                                    Log.Info(log);
                                }
                                catch (Exception)
                                {
                                    code = "TIME OUT";

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

                                    channel.QueueDeclare(queue: "task_queue_error",
                                                durable: true,
                                                exclusive: false,
                                                autoDelete: false,
                                                arguments: null);

                                    var smsStatus = JsonConvert.SerializeObject(status);
                                    var bodyStatus = Encoding.UTF8.GetBytes(smsStatus);

                                    var properties = channel.CreateBasicProperties();
                                    properties.SetPersistent(true);

                                    channel.BasicPublish(exchange: "",
                                        routingKey: "task_queue_error",
                                        basicProperties: properties,
                                        body: bodyStatus);

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

        private void _timer_Tick(object sender, EventArgs e)
        {
            if (!_bgSendSms.IsBusy)
                _bgSendSms.RunWorkerAsync();
        }

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

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (!_bgSendSms.IsBusy)
            {
                var logAppStop = String.Format("Application stoped at: {0}!",
                    DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                SetListBox(logAppStop + Environment.NewLine);
                Log.Info(logAppStop);

                _timer.Stop();
                btnStop.Enabled = false;
                btnStart.Enabled = true;
            }
            else
                SetListBox("Thread is running! Please try again." + Environment.NewLine);
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            rtbLog.Clear();
        }

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

        #region GetCodeReturnIncom

        /// <summary>
        /// Gets the string result from code.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        private static string GetStringResultFromCode(string xml)
        {
            if (xml.Contains("<STATUS>"))
            {
                int start = xml.IndexOf("<STATUS>", StringComparison.Ordinal) + 8;
                int end = xml.IndexOf("</STATUS>", StringComparison.Ordinal);
                int len = end - start;

                return xml.Substring(start, len);
            }

            return "-100";
        }

        #endregion GetCodeReturnIncom
    }
}