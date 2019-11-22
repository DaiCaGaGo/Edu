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

namespace MonitorServiceSMPP
{
    public partial class Service1 : ServiceBase
    {
        bool is_stop = false;
        List<Thread> lstTh = new List<Thread>();
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            XuLy.ghilog("SysSMS", "Call OnStart");
            try
            {
                Thread th1 = new Thread(() => run()); th1.Start();
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
        public void run()
        {
            try
            {
                while (!is_stop)
                {
                    XuLy.ghilog("SysSMS", "Start turn");
                    List<string> lstServiceName = OneValueConfig.getservername().Split(',').ToList();
                    foreach (string serviceName in lstServiceName)
                    {
                        if (!string.IsNullOrEmpty(serviceName))
                        {
                            try
                            {
                                ServiceController sc = new ServiceController(serviceName);
                                switch (sc.Status)
                                {
                                    case ServiceControllerStatus.Running:
                                        XuLy.ghilog("SysSMS", serviceName + " Running");
                                        DateTime dtnow = new DateTime();
                                        List<int> lstH = new List<int>() { 3, 6, 9, 12, 15, 18, 21, 0 };
                                        dtnow = DateTime.Now;
                                        if ((lstH.Contains(dtnow.Hour) || serviceName== "OneduSendMOBI") && dtnow.Minute <= 5)
                                        {
                                            try
                                            {
                                                XuLy.ghilog("SysSMS", "Kill " + serviceName);
                                                sc.Stop();
                                            }
                                            catch (InvalidOperationException)
                                            {
                                                XuLy.ghilog("SysSMS", serviceName + " Could not start the Alerter service.");
                                            }
                                        }
                                        break;
                                    case ServiceControllerStatus.Stopped:
                                        XuLy.ghilog("SysSMS", serviceName + " Stopped");
                                        try
                                        {
                                            sc.Start();
                                            sc.WaitForStatus(ServiceControllerStatus.Running);
                                        }
                                        catch (InvalidOperationException)
                                        {
                                            XuLy.ghilog("SysSMS", serviceName + " Could not start the Alerter service.");
                                        }
                                        break;
                                    case ServiceControllerStatus.Paused:
                                        XuLy.ghilog("SysSMS", serviceName + " Paused");
                                        break;
                                    case ServiceControllerStatus.StopPending:
                                        XuLy.ghilog("SysSMS", serviceName + " Stopping");
                                        break;
                                    case ServiceControllerStatus.StartPending:
                                        XuLy.ghilog("SysSMS", serviceName + " Starting");
                                        break;
                                    default:
                                        XuLy.ghilog("SysSMS", serviceName + " Changing");
                                        break;
                                }
                            }
                            catch
                            {
                                XuLy.ghilog("SysSMS", serviceName + " not found");
                            }
                        }
                    }
                    XuLy.ghilog("SysSMS", "Stop turn");
                    Thread.Sleep(300000);
                }
            }
            catch (Exception ex)
            {
                XuLy.ghilog("SysSMS", ex.ToString());
            }
        }
        protected override void OnStop()
        {
            is_stop = true;
        }
    }
}
