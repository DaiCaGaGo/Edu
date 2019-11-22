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
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Form_Push_To_Queue_All
{
    public partial class Main : Form
    {
        #region Variable

        private BackgroundWorker _bgCheckData;
        private readonly BusDbAccess DataAccess = new BusDbAccess();
        private readonly ILog Log = LogManager.GetLogger("LogPushToQueue");
        private readonly Phone Phone = new Phone();
        private readonly System.Windows.Forms.Timer Timer = new System.Windows.Forms.Timer();

        private DataTable tableData = new DataTable();
        private List<PartnerSms> smsToGtel = new List<PartnerSms>();
        private List<PartnerSms> smsToIncom = new List<PartnerSms>();
        private List<PartnerSms> smsToIris = new List<PartnerSms>();
        private List<PartnerSms> smsToNeo = new List<PartnerSms>();
        private List<PartnerSms> smsToVht = new List<PartnerSms>();
        private List<PartnerSms> smsToVivas = new List<PartnerSms>();
        private List<PartnerSms> smsToVmg = new List<PartnerSms>();
        private List<PartnerSms> smsToOther = new List<PartnerSms>();
        private List<PartnerSms> smsToVnpt = new List<PartnerSms>();

        private ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };

        private ObjectCache cache = MemoryCache.Default;
        private CacheItemPolicy policy;

        private string[] partnerCheckLoop;
        #endregion Variable

        public Main()
        {
            InitializeComponent();

            Timer.Tick += _timer_Tick;

            _bgCheckData = new BackgroundWorker();
            _bgCheckData.DoWork += _bgCheckData_DoWork;
            _bgCheckData.RunWorkerCompleted += _bgCheckData_RunWorkerCompleted;

            var log = String.Format("Application started at: {0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            SetListBox(log + Environment.NewLine);
            Log.Info(log);
        }

        private void _bgCheckData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        // ReSharper disable once FunctionComplexityOverflow
        private void _bgCheckData_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                try
                {
                    tableData = DataAccess.GetData_To_SendSms(int.Parse(ConfigurationManager.AppSettings["topSelect"]));

                    if (tableData != null && tableData.Rows.Count > 0)
                    {
                        using (var connection = factory.CreateConnection())
                        {
                            foreach (DataRow row in tableData.Rows)
                            {
                                PartnerSms sms = new PartnerSms
                                {
                                    SmsId = long.Parse(row["smsid"].ToString()),
                                    PartnerId = int.Parse(row["PartnerID"].ToString()),
                                    SenderName = row["SenderName"].ToString(),
                                    Sms = row["Sms"].ToString(),
                                    IsUnicode = bool.Parse(row["isUnicode"].ToString()),
                                    IsFlash = bool.Parse(row["isFlash"].ToString()),
                                    TimePost = DateTime.Parse(row["timepost"].ToString()),
                                    IsHot = bool.Parse(row["isHot"].ToString()),
                                    TimeSend = DateTime.Parse(row["timeSend"].ToString()),
                                    CountMes = int.Parse(row["countmes"].ToString()),
                                    Phone = row["phone"].ToString(),
                                    IsRead = bool.Parse(row["isRead"].ToString()),
                                    IsApproved = bool.Parse(row["isApproved"].ToString()),
                                    Viettel = row["Viettel"].ToString(),
                                    Vnm = row["VNM"].ToString(),
                                    Vms = row["VMS"].ToString(),
                                    Gpc = row["GPC"].ToString(),
                                    Sfone = row["Sfone"].ToString(),
                                    Evn = row["EVN"].ToString(),
                                    Gtel = row["Gtel"].ToString(),
                                    BrdViettel = row["BrdViettel"].ToString(),
                                    BrdVnm = row["BrdVNM"].ToString(),
                                    BrdGpc = row["BrdGPC"].ToString(),
                                    BrdSfone = row["BrdSfone"].ToString(),
                                    BrdEvn = row["BrdEVN"].ToString(),
                                    BrdVms = row["BrdVMS"].ToString(),
                                    BrdGtel = row["BrdGtel"].ToString()
                                };

                                if (row["CustomerSmsID"].ToString().Trim().Length > 0)
                                    sms.CustomerSmsId = long.Parse(row["CustomerSmsID"].ToString());

                                var telCo = Phone.GetTelco(sms.Phone);

                                #region Gtel

                                if ((sms.Viettel.Trim().ToUpper() == "GTEL" && telCo == "Viettel")
                                    || (sms.Gpc.Trim().ToUpper() == "GTEL" && telCo == "GPC")
                                    || (sms.Vms.Trim().ToUpper() == "GTEL" && telCo == "VMS")
                                    || (sms.Vnm.Trim().ToUpper() == "GTEL" && telCo == "VNM")
                                    || (sms.Gtel.Trim().ToUpper() == "GTEL" && telCo == "Gtel"))
                                {
                                    if (PartnerCheckLoopSms("GTEL", sms, telCo, connection))
                                        continue;

                                    smsToGtel.Add(sms);

                                    if (smsToGtel != null && smsToGtel.Count >= int.Parse(ConfigurationManager.AppSettings["limitSmsAddQueue"]))
                                    {
                                        PublishQueue(connection, "gtel_task_queue", smsToGtel);
                                        smsToGtel.Clear();
                                    }

                                    var log =
                                        String.Format(
                                            "Added ==> SmsId: {0}, Brand: {1}, Sms: {2}, Phone: {3}, Count: {4}, At: {5}, Chanel: {6}",
                                            sms.SmsId, sms.SenderName, sms.Sms, sms.Phone, sms.CountMes,
                                            DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), "Gtel");

                                    SetListBox(log + Environment.NewLine);
                                    Log.Info(log);
                                }

                                #endregion Gtel

                                #region Incom

                                else if ((sms.Viettel.Trim().ToUpper() == "INCOM" && telCo == "Viettel")
                                    || (sms.Gpc.Trim().ToUpper() == "INCOM" && telCo == "GPC")
                                    || (sms.Vms.Trim().ToUpper() == "INCOM" && telCo == "VMS")
                                    || (sms.Vnm.Trim().ToUpper() == "INCOM" && telCo == "VNM")
                                    || (sms.Gtel.Trim().ToUpper() == "INCOM" && telCo == "Gtel"))
                                {
                                    if (PartnerCheckLoopSms("INCOM", sms, telCo, connection))
                                        continue;

                                    smsToIncom.Add(sms);

                                    if (smsToIncom != null && smsToIncom.Count >= int.Parse(ConfigurationManager.AppSettings["limitSmsAddQueue"]))
                                    {
                                        PublishQueue(connection, "incom_task_queue", smsToIncom);
                                        smsToIncom.Clear();
                                    }

                                    var log =
                                        String.Format(
                                            "Added ==> SmsId: {0}, Brand: {1}, Sms: {2}, Phone: {3}, Count: {4}, At: {5}, Chanel: {6}",
                                            sms.SmsId, sms.SenderName, sms.Sms, sms.Phone, sms.CountMes,
                                            DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), "Incom");

                                    SetListBox(log + Environment.NewLine);
                                    Log.Info(log);
                                }

                                #endregion Incom

                                #region Iris

                                else if ((sms.Viettel.Trim().ToUpper() == "IRIS" && telCo == "Viettel")
                                    || (sms.Gpc.Trim().ToUpper() == "IRIS" && telCo == "GPC")
                                    || (sms.Vms.Trim().ToUpper() == "IRIS" && telCo == "VMS")
                                    || (sms.Vnm.Trim().ToUpper() == "IRIS" && telCo == "VNN")
                                    || (sms.Gtel.Trim().ToUpper() == "IRIS" && telCo == "Gtel"))
                                {
                                    if (PartnerCheckLoopSms("IRIS", sms, telCo, connection))
                                        continue;

                                    smsToIris.Add(sms);

                                    if (smsToIris != null && smsToIris.Count >= int.Parse(ConfigurationManager.AppSettings["limitSmsAddQueue"]))
                                    {
                                        PublishQueue(connection, "iris_task_queue", smsToIris);
                                        smsToIris.Clear();
                                    }

                                    var log =
                                        String.Format(
                                            "Added ==> SmsId: {0}, Brand: {1}, Sms: {2}, Phone: {3}, Count: {4}, At: {5}, Chanel: {6}",
                                            sms.SmsId, sms.SenderName, sms.Sms, sms.Phone, sms.CountMes,
                                            DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), "Iris");

                                    SetListBox(log + Environment.NewLine);
                                    Log.Info(log);
                                }

                                #endregion Iris

                                #region Neo

                                else if ((sms.Viettel.Trim().ToUpper().Contains("NEO") && !sms.Viettel.Trim().ToUpper().Contains("AMS") && telCo == "Viettel")
                                    || (sms.Gpc.Trim().ToUpper().Contains("NEO") && !sms.Gpc.Trim().ToUpper().Contains("AMS") && telCo == "GPC")
                                    || (sms.Vms.Trim().ToUpper().Contains("NEO") && !sms.Vms.Trim().ToUpper().Contains("AMS") && telCo == "VMS")
                                    || (sms.Vnm.Trim().ToUpper().Contains("NEO") && !sms.Vnm.Trim().ToUpper().Contains("AMS") && telCo == "VNM")
                                    || (sms.Gtel.Trim().ToUpper().Contains("NEO") && !sms.Gtel.Trim().ToUpper().Contains("AMS") && telCo == "Gtel"))
                                {
                                    if (PartnerCheckLoopSms("NEO", sms, telCo, connection))
                                        continue;

                                    smsToNeo.Add(sms);

                                    if (smsToNeo != null && smsToNeo.Count >= int.Parse(ConfigurationManager.AppSettings["limitSmsAddQueue"]))
                                    {
                                        PublishQueue(connection, "neo_task_queue", smsToNeo);
                                        smsToNeo.Clear();
                                    }

                                    var log =
                                        String.Format(
                                            "Added ==> SmsId: {0}, Brand: {1}, Sms: {2}, Phone: {3}, Count: {4}, At: {5}, Chanel: {6}",
                                            sms.SmsId, sms.SenderName, sms.Sms, sms.Phone, sms.CountMes,
                                            DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), "Neo");

                                    SetListBox(log + Environment.NewLine);
                                    Log.Info(log);
                                }

                                #endregion Neo

                                #region Vht

                                else if ((sms.Viettel.Trim().ToUpper() == "VHT" && telCo == "Viettel")
                                    || (sms.Gpc.Trim().ToUpper() == "VHT" && telCo == "GPC")
                                    || (sms.Vms.Trim().ToUpper() == "VHT" && telCo == "VMS")
                                    || (sms.Vnm.Trim().ToUpper() == "VHT" && telCo == "VNM")
                                    || (sms.Gtel.Trim().ToUpper() == "VHT" && telCo == "Gtel"))
                                {
                                    if (PartnerCheckLoopSms("VHT", sms, telCo, connection))
                                        continue;

                                    smsToVht.Add(sms);

                                    if (smsToVht != null && smsToVht.Count >= int.Parse(ConfigurationManager.AppSettings["limitSmsAddQueue"]))
                                    {
                                        PublishQueue(connection, "vht_task_queue", smsToVht);
                                        smsToVht.Clear();
                                    }

                                    var log =
                                        String.Format(
                                            "Added ==> SmsId: {0}, Brand: {1}, Sms: {2}, Phone: {3}, Count: {4}, At: {5}, Chanel: {6}",
                                            sms.SmsId, sms.SenderName, sms.Sms, sms.Phone, sms.CountMes,
                                            DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), "Vht");

                                    SetListBox(log + Environment.NewLine);
                                    Log.Info(log);
                                }

                                #endregion Vht

                                #region Vivas

                                else if ((sms.Viettel.Trim().ToUpper() == "VIVAS" && telCo == "Viettel")
                                    || (sms.Gpc.Trim().ToUpper() == "VIVAS" && telCo == "GPC")
                                    || (sms.Vms.Trim().ToUpper() == "VIVAS" && telCo == "VMS")
                                    || (sms.Vnm.Trim().ToUpper() == "VIVAS" && telCo == "VNM")
                                    || (sms.Gtel.Trim().ToUpper() == "VIVAS" && telCo == "Gtel"))
                                {
                                    if (PartnerCheckLoopSms("VIVAS", sms, telCo, connection))
                                        continue;

                                    smsToVivas.Add(sms);

                                    if (smsToVivas != null && smsToVivas.Count >= int.Parse(ConfigurationManager.AppSettings["limitSmsAddQueue"]))
                                    {
                                        PublishQueue(connection, "vivas_task_queue", smsToVivas);
                                        smsToVivas.Clear();
                                    }

                                    var log =
                                        String.Format(
                                            "Added ==> SmsId: {0}, Brand: {1}, Sms: {2}, Phone: {3}, Count: {4}, At: {5}, Chanel: {6}",
                                            sms.SmsId, sms.SenderName, sms.Sms, sms.Phone, sms.CountMes,
                                            DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), "Vivas");

                                    SetListBox(log + Environment.NewLine);
                                    Log.Info(log);
                                }

                                #endregion Vivas

                                #region Vmg

                                else if ((sms.Viettel.Trim().ToUpper().Contains("VMG") &&
                                          !sms.Viettel.Trim().ToUpper().Contains("AMS") && telCo == "Viettel")
                                         ||
                                         (sms.Gpc.Trim().ToUpper().Contains("VMG") &&
                                          !sms.Gpc.Trim().ToUpper().Contains("AMS") && telCo == "GPC")
                                         ||
                                         (sms.Vms.Trim().ToUpper().Contains("VMG") &&
                                          !sms.Vms.Trim().ToUpper().Contains("AMS") && telCo == "VMS")
                                         ||
                                         (sms.Vnm.Trim().ToUpper().Contains("VMG") &&
                                          !sms.Vnm.Trim().ToUpper().Contains("AMS") && telCo == "VNM")
                                         ||
                                         (sms.Gtel.Trim().ToUpper().Contains("VMG") &&
                                          !sms.Gtel.Trim().ToUpper().Contains("AMS") && telCo == "Gtel"))
                                {
                                    if (PartnerCheckLoopSms("VMG", sms, telCo, connection))
                                        continue;

                                    smsToVmg.Add(sms);

                                    if (smsToVmg != null && smsToVmg.Count >= int.Parse(ConfigurationManager.AppSettings["limitSmsAddQueue"]))
                                    {
                                        PublishQueue(connection, "vmg_task_queue", smsToVmg);
                                        smsToVmg.Clear();
                                    }

                                    var log =
                                        String.Format(
                                            "Added ==> SmsId: {0}, Brand: {1}, Sms: {2}, Phone: {3}, Count: {4}, At: {5}, Chanel: {6}",
                                            sms.SmsId, sms.SenderName, sms.Sms, sms.Phone, sms.CountMes,
                                            DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), "Vmg");

                                    SetListBox(log + Environment.NewLine);
                                    Log.Info(log);
                                }

                                #endregion Vmg

                                #region Vnpt

                                else if ((sms.Viettel.Trim().ToUpper() == "VNPT" && telCo == "Viettel")
                                    || (sms.Gpc.Trim().ToUpper() == "VNPT" && telCo == "GPC")
                                    || (sms.Vms.Trim().ToUpper() == "VNPT" && telCo == "VMS")
                                    || (sms.Vnm.Trim().ToUpper() == "VNPT" && telCo == "VNM")
                                    || (sms.Gtel.Trim().ToUpper() == "VNPT" && telCo == "Gtel"))
                                {
                                    if (PartnerCheckLoopSms("VNPT", sms, telCo, connection))
                                        continue;

                                    smsToVnpt.Add(sms);

                                    if (smsToVnpt != null && smsToVnpt.Count >= int.Parse(ConfigurationManager.AppSettings["limitSmsAddQueue"]))
                                    {
                                        PublishQueue(connection, "vnpt_task_queue", smsToVnpt);
                                        smsToVnpt.Clear();
                                    }

                                    var log =
                                        String.Format(
                                            "Added ==> SmsId: {0}, Brand: {1}, Sms: {2}, Phone: {3}, Count: {4}, At: {5}, Chanel: {6}",
                                            sms.SmsId, sms.SenderName, sms.Sms, sms.Phone, sms.CountMes,
                                            DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), "VNPT");

                                    SetListBox(log + Environment.NewLine);
                                    Log.Info(log);
                                }

                                #endregion Vnpt

                                #region Other

                                else
                                {
                                    smsToOther.Add(sms);

                                    if (smsToOther != null && smsToOther.Count >= int.Parse(ConfigurationManager.AppSettings["limitSmsAddQueue"]))
                                    {
                                        PublishQueue(connection, "other_task_queue", smsToOther);

                                        smsToOther.Clear();
                                    }

                                    var log =
                                        String.Format(
                                            "Added ==> SmsId: {0}, Brand: {1}, Sms: {2}, Phone: {3}, Count: {4}, At: {5}, Chanel: {6}",
                                            sms.SmsId, sms.SenderName, sms.Sms, sms.Phone, sms.CountMes,
                                            DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), "Other");

                                    SetListBox(log + Environment.NewLine);
                                    Log.Info(log);
                                }

                                #endregion Other
                            }

                            if (smsToGtel != null && smsToGtel.Count > 0)
                                PublishQueue(connection, "gtel_task_queue", smsToGtel);

                            if (smsToIncom != null && smsToIncom.Count > 0)
                                PublishQueue(connection, "incom_task_queue", smsToIncom);

                            if (smsToIris != null && smsToIris.Count > 0)
                                PublishQueue(connection, "iris_task_queue", smsToIris);

                            if (smsToNeo != null && smsToNeo.Count > 0)
                                PublishQueue(connection, "neo_task_queue", smsToNeo);

                            if (smsToOther != null && smsToOther.Count > 0)
                                PublishQueue(connection, "other_task_queue", smsToOther);

                            if (smsToVht != null && smsToVht.Count > 0)
                                PublishQueue(connection, "vht_task_queue", smsToVht);

                            if (smsToVivas != null && smsToVivas.Count > 0)
                                PublishQueue(connection, "vivas_task_queue", smsToVivas);

                            if (smsToVmg != null && smsToVmg.Count > 0)
                                PublishQueue(connection, "vmg_task_queue", smsToVmg);

                            if (smsToVnpt != null && smsToVnpt.Count > 0)
                                PublishQueue(connection, "vnpt_task_queue", smsToVnpt);

                            ClearDataTable();
                        }
                    }
                    else
                        break;
                }
                catch (Exception ex)
                {
                    if(tableData != null && tableData.Rows.Count > 0)
                    {
                        DataTable tableStatus = new DataTable();
                        tableStatus.Columns.Add(new DataColumn("sms_id", typeof(long)));

                        foreach (DataRow row in tableData.Rows)
                        {
                            DataRow dr = tableStatus.NewRow();
                            dr["sms_id"] = row["smsid"];
                            tableStatus.Rows.Add(dr);
                        }

                        if (tableStatus != null && tableStatus.Rows.Count > 0)
                        {
                            DataAccess.Rollback_When_Fail(tableStatus);

                            var log = String.Format("Can't add data to queue, rollback: {0} item at: {1}", tableData.Rows.Count, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

                            SetListBox(log + Environment.NewLine);
                            Log.Info(log);
                        }                           
                    }

                    Log.Error("_bgCheckData_DoWork error at: " + DateTime.Now + ", error detail: " + ex.Message);
                }
            }
        }

        private void ClearDataTable()
        {
            smsToGtel.Clear();
            smsToIncom.Clear();
            smsToIris.Clear();
            smsToNeo.Clear();
            smsToOther.Clear();
            smsToVht.Clear();
            smsToVivas.Clear();
            smsToVmg.Clear();
            smsToVnpt.Clear();
        }

        /// <summary>
        /// Partners the check loop SMS.
        /// </summary>
        /// <param name="portName">Name of the port.</param>
        /// <param name="sms">The SMS.</param>
        /// <param name="telCo">The tel co.</param>
        /// <param name="connection">The connection.</param>
        /// <returns></returns>
        private bool PartnerCheckLoopSms(string portName, PartnerSms sms, string telCo, IConnection connection)
        {
            if (partnerCheckLoop != null && partnerCheckLoop.Contains(sms.PartnerId.ToString()))
            {
                var cacheValue = String.Format("[{0}]_[{1}]_[{2}]_[{3}]", sms.PartnerId, sms.SenderName, sms.Sms, sms.Phone);

                if (cache.GetCacheItem(cacheValue) == null)
                {
                    policy = new CacheItemPolicy
                    {
                        AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10)
                    };

                    cache.Add(cacheValue, cacheValue, policy);

                    return false;
                }
                else
                {
                    SmsStatus status = new SmsStatus()
                    {
                        sms_id = sms.SmsId,
                        partner_id = sms.PartnerId,
                        sender_name = sms.SenderName,
                        sms_content = sms.Sms,
                        is_unicode = sms.IsUnicode,
                        is_flash = sms.IsFlash,
                        count_mes = sms.CountMes,
                        port_name = portName,
                        phone = sms.Phone,
                        tel_code = telCo,
                        time_send = sms.TimeSend,
                        time_reveive = DateTime.Now,
                        sent_code = "SMSLoop",
                        customer_sms_id = sms.CustomerSmsId
                    };

                    PublishQueueStatus(connection, "task_queue_success", status);

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Publishes the queue.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="listSms">The list SMS.</param>
        private void PublishQueue(IConnection connection, string queueName, List<PartnerSms> listSms)
        {
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var message = JsonConvert.SerializeObject(listSms);
                var body = Encoding.UTF8.GetBytes(message);

                var properties = channel.CreateBasicProperties();
                properties.SetPersistent(true);

                channel.BasicPublish(exchange: "",
                    routingKey: queueName,
                    basicProperties: properties,
                    body: body);
            }
        }

        /// <summary>
        /// Publishes the queue.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="statusSms">Trạng thái tin nhắn.</param>
        private void PublishQueueStatus(IConnection connection, string queueName, SmsStatus statusSms)
        {
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var message = JsonConvert.SerializeObject(statusSms);
                var body = Encoding.UTF8.GetBytes(message);

                var properties = channel.CreateBasicProperties();
                properties.SetPersistent(true);

                channel.BasicPublish(exchange: "",
                    routingKey: queueName,
                    basicProperties: properties,
                    body: body);
            }
        }

        /// <summary>
        /// Handles the Tick event of the _timer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void _timer_Tick(object sender, EventArgs e)
        {
            if (!_bgCheckData.IsBusy)
                _bgCheckData.RunWorkerAsync();
        }

        /// <summary>
        /// Handles the Click event of the btnClearLog control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnClearLog_Click(object sender, EventArgs e)
        {
            rtbLog.Clear();
        }

        /// <summary>
        /// Handles the Click event of the btnStart control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            partnerCheckLoop = ConfigurationManager.AppSettings["partnerCheckLoop"].Split(',');
            Timer.Interval = int.Parse(ConfigurationManager.AppSettings["timeDelay"]);

            var log = String.Format("Thread started at: {0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            SetListBox(log + Environment.NewLine);
            Log.Info(log);

            Timer.Start();
            btnStop.Enabled = true;
            btnStart.Enabled = false;
        }

        /// <summary>
        /// Handles the Click event of the btnStop control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnStop_Click(object sender, EventArgs e)
        {
            Timer.Stop();

            if (!_bgCheckData.IsBusy)
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

        #region Delegate Set Text Log        
        private delegate void SetListBoxDelegate(string text);

        /// <summary>
        /// Sets the ListBox.
        /// </summary>
        /// <param name="text">The text.</param>
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