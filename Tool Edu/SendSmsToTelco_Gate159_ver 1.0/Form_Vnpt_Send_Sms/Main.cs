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
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Form_Vnpt_Send_Sms
{
    public partial class Main : Form
    {
        #region Variables

        private BackgroundWorker _bgSendSms;
        private readonly ILog Log = LogManager.GetLogger("LogVnptSendSms");
        private readonly BrandNameWSService _ApiVnpt = new BrandNameWSService();
        private static readonly Encryption Encrypt = new Encryption();
        private readonly Phone Phone = new Phone();
        private Timer _timer = new Timer();

        private ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };

        #endregion Variables

        public Main()
        {
            InitializeComponent();

            _timer.Tick += _timer_Tick; ;

            _bgSendSms = new BackgroundWorker();
            _bgSendSms.DoWork += _bgSendSms_DoWork;
            _bgSendSms.RunWorkerCompleted += _bgSendSms_RunWorkerCompleted;

            var log = String.Format("Application started at: {0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            SetListBox(log + Environment.NewLine);
            Log.Info(log);
        }

        private void _bgSendSms_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private void _bgSendSms_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: "vnpt_task_queue",
                                             durable: true,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null);

                        while (true)
                        {
                            //Tienlm - 20160809 - Lấy dữ liệu từ Rabbitmq
                            var data = channel.BasicGet("vnpt_task_queue", true);
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

                                //Tienlm - 20160809 - Lấy thông tin Port và BrandName chuẩn
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
                                    //Tienlm - 20160809 - Kiểm tra tin nhăn spam cho vccrop
                                    else if (sms.PartnerId == 45 && sms.Sms.ToLower().Contains("vccspam"))
                                        code = "SMS SPAM";
                                    else
                                    {
                                        if (ConfigurationManager.AppSettings["version"] == "running")
                                        {
                                            int result = _ApiVnpt.uploadSMS(ConfigurationManager.AppSettings["username"],
                                                ConfigurationManager.AppSettings["password"],
                                                senderName, sms.Phone, "0", "2", sms.Sms);

                                            code = GetStringResultFromCode(result);
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

                                    #region MyRegion
                                    //channel.QueueDeclare(queue: "task_queue_success",
                                    //    durable: true,
                                    //    exclusive: false,
                                    //    autoDelete: false,
                                    //    arguments: null);

                                    //var smsStatus = JsonConvert.SerializeObject(status);
                                    //var bodyStatus = Encoding.UTF8.GetBytes(smsStatus);

                                    //var properties = channel.CreateBasicProperties();
                                    //properties.SetPersistent(true);

                                    //channel.BasicPublish(exchange: "",
                                    //    routingKey: "task_queue_success",
                                    //    basicProperties: properties,
                                    //    body: bodyStatus);
                                    #endregion
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

                                    #region MyRegion
                                    //channel.QueueDeclare(queue: "task_queue_error",
                                    //            durable: true,
                                    //            exclusive: false,
                                    //            autoDelete: false,
                                    //            arguments: null);

                                    //var smsStatus = JsonConvert.SerializeObject(status);
                                    //var bodyStatus = Encoding.UTF8.GetBytes(smsStatus);

                                    //var properties = channel.CreateBasicProperties();
                                    //properties.SetPersistent(true);

                                    //channel.BasicPublish(exchange: "",
                                    //    routingKey: "task_queue_error",
                                    //    basicProperties: properties,
                                    //    body: bodyStatus);
                                    #endregion
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

        private void _timer_Tick(object sender, EventArgs e)
        {
            if (!_bgSendSms.IsBusy)
                _bgSendSms.RunWorkerAsync();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            _timer.Interval = int.Parse(ConfigurationManager.AppSettings["timeDelay"]);
            _ApiVnpt.Timeout = int.Parse(ConfigurationManager.AppSettings["apiTimeout"]);

            var log = String.Format("Thread started at: {0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            SetListBox(log + Environment.NewLine);
            Log.Info(log);

            _timer.Start();
            btnStop.Enabled = true;
            btnStart.Enabled = false;
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

        private static string GetStringResultFromCode(int result)
        {
            if (result == 0)
                return "DELIVERED";
            return "UNDELIVERED|" + result;
        }

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

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            rtbLog.Clear();
        }
    }
}