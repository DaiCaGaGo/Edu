using Bus;
using Entity;
using log4net;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Form_Update_Success_All
{
    public partial class Main : Form
    {
        #region Variables

        private BackgroundWorker _bgUpdateData;
        private readonly BusDbAccess DataAccess = new BusDbAccess();
        private readonly ILog Log = LogManager.GetLogger("LogUpdateSuccess");
        private readonly Timer _timer = new Timer();

        private ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };

        #endregion Variables

        public Main()
        {
            InitializeComponent();

            _timer.Tick += _timer_Tick;

            _bgUpdateData = new BackgroundWorker();
            _bgUpdateData.DoWork += _bgUpdateData_DoWork;
            _bgUpdateData.RunWorkerCompleted += _bgUpdateData_RunWorkerCompleted;
            _bgUpdateData.WorkerSupportsCancellation = true;

            var log = String.Format("Application started at: {0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            SetListBox(log + Environment.NewLine);
            Log.Info(log);
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            if (!_bgUpdateData.IsBusy)
                _bgUpdateData.RunWorkerAsync();
        }

        private void _bgUpdateData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private void _bgUpdateData_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                DataTable tableStatus = new DataTable();
                tableStatus.Columns.Add(new DataColumn("sms_id", typeof(long)));
                tableStatus.Columns.Add(new DataColumn("partner_id", typeof(int)));
                tableStatus.Columns.Add(new DataColumn("sender_name", typeof(string)));
                tableStatus.Columns.Add(new DataColumn("sms_content", typeof(string)));
                tableStatus.Columns.Add(new DataColumn("is_unicode", typeof(bool)));
                tableStatus.Columns.Add(new DataColumn("is_flash", typeof(bool)));
                tableStatus.Columns.Add(new DataColumn("count_mes", typeof(int)));
                tableStatus.Columns.Add(new DataColumn("port_name", typeof(string)));
                tableStatus.Columns.Add(new DataColumn("phone", typeof(string)));
                tableStatus.Columns.Add(new DataColumn("tel_code", typeof(string)));
                tableStatus.Columns.Add(new DataColumn("time_send", typeof(DateTime)));
                tableStatus.Columns.Add(new DataColumn("time_reveive", typeof(DateTime)));
                tableStatus.Columns.Add(new DataColumn("sent_code", typeof(string)));
                tableStatus.Columns.Add(new DataColumn("customer_sms_id", typeof(long)));

                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: "task_queue_success",
                                             durable: true,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null);

                        while (true)
                        {
                            var data = channel.BasicGet("task_queue_success", true);
                            if (data == null)
                            {
                                if (tableStatus.Rows.Count > 0)
                                {
                                    DataAccess.Update_When_Success(tableStatus);

                                    var log = String.Format("Updated {0} item! At: {1}", tableStatus.Rows.Count,
                                        DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

                                    SetListBox(log + Environment.NewLine);
                                    Log.Info(log);
                                    tableStatus.Clear();
                                }

                                break;
                            }
                            var body = data.Body;
                            var message = Encoding.UTF8.GetString(body);
                            SmsStatus obj = JsonConvert.DeserializeObject<SmsStatus>(message);

                            DataRow dr = tableStatus.NewRow();
                            dr["sms_id"] = obj.sms_id;
                            dr["partner_id"] = obj.partner_id;
                            dr["sender_name"] = obj.sender_name;
                            dr["sms_content"] = obj.sms_content;
                            dr["is_unicode"] = obj.is_unicode;
                            dr["is_flash"] = obj.is_flash;
                            dr["count_mes"] = obj.count_mes;
                            dr["port_name"] = obj.port_name;
                            dr["phone"] = obj.phone;
                            dr["tel_code"] = obj.tel_code;
                            dr["time_send"] = obj.time_send;
                            dr["time_reveive"] = obj.time_reveive;
                            dr["sent_code"] = obj.sent_code;
                            dr["customer_sms_id"] = obj.customer_sms_id;
                            tableStatus.Rows.Add(dr);

                            if (tableStatus.Rows.Count >= 100)
                            {
                                DataAccess.Update_When_Success(tableStatus);

                                var log = String.Format("Updated {0} item! At: {1}", tableStatus.Rows.Count,
                                        DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

                                SetListBox(log + Environment.NewLine);
                                Log.Info(log);
                                tableStatus.Clear();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("_bgUpdateData_DoWork error at: " + DateTime.Now + ", error details: " + ex.Message);
            }
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
            if (!_bgUpdateData.IsBusy)
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
    }
}