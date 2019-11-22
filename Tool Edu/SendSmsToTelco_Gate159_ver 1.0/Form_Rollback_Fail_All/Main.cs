using Bus;
using Entity;
using log4net;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Form_Rollback_Fail_All
{
    public partial class Main : Form
    {
        #region Variables

        private BackgroundWorker _bgUpdateData;
        private readonly BusDbAccess DataAccess = new BusDbAccess();
        private readonly ILog Log = LogManager.GetLogger("LogRollbackFail");
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

                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: "task_queue_error",
                                             durable: true,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null);

                        while (true)
                        {
                            var data = channel.BasicGet("task_queue_error", true);
                            if (data == null)
                            {
                                if (tableStatus.Rows.Count > 0)
                                {
                                    DataAccess.Rollback_When_Fail(tableStatus);

                                    var log = String.Format("Rollback {0} item! At: {1}", tableStatus.Rows.Count,
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
                            tableStatus.Rows.Add(dr);

                            if (tableStatus.Rows.Count >= 100)
                            {
                                DataAccess.Rollback_When_Fail(tableStatus);

                                var log = String.Format("Rollback {0} item! At: {1}", tableStatus.Rows.Count,
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