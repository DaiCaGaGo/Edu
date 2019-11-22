using Bus;
using Common;
using Entity;
using log4net;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Form_Vht_Send_Sms
{
    public partial class Main : Form
    {
        #region Variables

        private static BackgroundWorker _bgSendSms;
        private static readonly BusDbAccess DataAccess = new BusDbAccess();
        private static readonly sms VhtApi = new sms();
        private static readonly ILog Log = LogManager.GetLogger("LogVhtSendSms");
        private static readonly Phone Phone = new Phone();
        private static readonly TemplateSms Tmp = new TemplateSms();
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
                        channel.QueueDeclare(queue: "vht_task_queue",
                                             durable: true,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null);

                        while (true)
                        {
                            var data = channel.BasicGet("vht_task_queue", true);
                            if (data == null)
                                break;
                            var body = data.Body;
                            var message = Encoding.UTF8.GetString(body);
                            List<PartnerSms> listSms = JsonConvert.DeserializeObject<List<PartnerSms>>(message);

                            //Send to smsc
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
                                    int result;
                                    bool isResult;

                                    if (String.IsNullOrEmpty(telCo))
                                        code = "PHONE INVALID";
                                    else if (sms.PartnerId == 45 && sms.Sms.ToLower().Contains("vccspam"))
                                        code = "SMS SPAM";
                                    else
                                    {
                                        if (ConfigurationManager.AppSettings["version"] == "running")
                                        {
                                            if (telCo == "GPC")
                                            {
                                                DataTable tableTemplate = DataAccess.GetTemplateTelCo(senderName);
                                                string[] arrsms = Tmp.CheckTemplate(sms.Sms, tableTemplate.Select(), true);

                                                if (arrsms != null && arrsms[0] == "True") //đúng template gửi
                                                {
                                                    VhtApi.sendSmsReport(ConfigurationManager.AppSettings["password"],
                                                        ConfigurationManager.AppSettings["username"], sms.Phone,
                                                        senderName, sms.Sms, ConfigurationManager.AppSettings["headid"] + sms.SmsId,
                                                        out result, out isResult);

                                                    code = GetStringResultFromCode(result);
                                                }
                                                else //sai template báo lỗi.
                                                {
                                                    code = "TemplateFail";
                                                }
                                            }
                                            else
                                            {
                                                VhtApi.sendSmsReport(ConfigurationManager.AppSettings["password"],
                                                    ConfigurationManager.AppSettings["username"], sms.Phone,
                                                    senderName, sms.Sms, ConfigurationManager.AppSettings["headid"] + sms.SmsId,
                                                    out result, out isResult);

                                                code = GetStringResultFromCode(result);
                                            }
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
            VhtApi.Timeout = int.Parse(ConfigurationManager.AppSettings["apiTimeout"]);
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

        #region GetCodeReturnVht

        /// <summary>
        /// Gets the string result from code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        private static string GetStringResultFromCode(int code)
        {
            switch (code)
            {
                case -1:
                    return "ApiInvalid";

                case -3:
                    return "OutOfQuota";

                case -4:
                    return "NotEnough";

                case -5:
                    return "RecipientInvalid";

                case -6:
                    return "ErrorMessage";

                case -7:
                    return "ErrorBrandName";

                case -8:
                    return "IpInvalid";

                case -9:
                    return "BrandnameNotRegister";

                case -10:
                    return "RecipientNotReceiverMessage";

                case -11:
                    return "UserPostpaid";

                case -12:
                    return "BrandNameHasExisted";

                case -13:
                    return "IpExisted";

                case -14:
                    return "OutOfLengthMessage";

                case -15:
                    return "NotSupportTelco";

                case -16:
                    return "XMLFormatFail";

                case -17:
                    return "AuthenticationFail";

                case -20:
                    return "DuplicateBrandname";

                case -21:
                    return "Timeout";

                case 105:
                    return "TemplateDoesNotExisted";

                default:
                    return "DELIVERED";
            }
        }

        #endregion GetCodeReturnVht

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
    }
}