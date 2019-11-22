using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OneEduDataAccess;
using OneEduDataAccess.Model;
using OneEduDataAccess.BO;

namespace ChuyenMangGiuSo
{
    public partial class Service1 : ServiceBase
    {
        public bool is_stop = true;
        long id = 1;
        long idstep = 0;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            XuLy.ghilog("LogRun", "Call OnStart");
            try
            {
                try
                {
                    id = Convert.ToInt64(OneValueConfig.urlId());
                }
                catch { }
                is_stop = false;
                Thread threadRun = (new Thread(() => run()));
                threadRun.Start();
            }
            catch (ArgumentException x)
            {
                XuLy.ghilog("LogRun", "SYS Error OnStart");
            }
            catch (Exception ex)
            {
                XuLy.ghilog("LogRun", "SYS Error OnStart");
            }
        }
        public void run()
        {
            while (!is_stop)
            {
                try
                {
                    XuLy.ghilog("LogRun", "Start turn "+ id);
                    #region request
                    // Create a request for the URL.   
                    var request = WebRequest.CreateHttp("http://vntelecom.vnta.gov.vn:10246/vnta/file/download?id=" + id);
                    //request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    request.Headers.Add("Accept-Encoding", "gzip, deflate");
                    request.Headers.Add("Accept-Language", "en,vi-VN;q=0.8,vi;q=0.5,en-US;q=0.3");
                    //request.Headers.Add("Connection", "keep-alive");
                    request.Headers.Add("Cookie", string.Format(@"f5_cspm=1234; JSESSIONID={0}; TS017dff08={1}; TS0165a601={2}"
                                                , OneValueConfig.JSESSIONID()
                                                , OneValueConfig.TS017dff08()
                                                , OneValueConfig.TS0165a601()));
                    //request.Headers.Add("Host", "vntelecom.vnta.gov.vn:10246");
                    request.Headers.Add("Upgrade-Insecure-Requests", "1");
                    request.Timeout = 3000;
                    //request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:65.0) Gecko/20100101 Firefox/65.0");
                    // Get the response.  
                    WebResponse response = request.GetResponse();
                    // Display the status.  
                    Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                    // Get the stream containing content returned by the server.  
                    Stream dataStream = response.GetResponseStream();
                    // Open the stream using a StreamReader for easy access.  
                    StreamReader reader = new StreamReader(dataStream);
                    #endregion
                    #region read data
                    using (StreamReader sr = new StreamReader(dataStream))
                    {
                        while (!sr.EndOfStream)
                        {
                            try
                            {
                                string strLineValue = sr.ReadLine();
                                XuLy.ghilog("LogRun",id+" : "+ strLineValue);
                                if (strLineValue.Contains("MOBILE,"))
                                {
                                    idstep = 0;
                                    //201811191446470200009093,MOBILE,PORT,84962898999,02,02,04,04,2018-11-20 06:46:05
                                    List<string> lstValue = strLineValue.Split(',').ToList();
                                    if (lstValue.Count > 5 && lstValue[2] != null && lstValue[3].Contains("84"))
                                    {
                                        string sdt = lstValue[3].Trim();
                                        long indexSDT = removeHeadPhone(sdt);
                                        string ma_nha_mang = lstValue[4].Trim();
                                        string nha_mang = "";
                                        switch (ma_nha_mang)
                                        {
                                            case "01":
                                                nha_mang = "MobiFone";
                                                break;
                                            case "02":
                                                nha_mang = "VinaPhone";
                                                break;
                                            case "04":
                                                nha_mang = "Viettel";
                                                break;
                                            case "05":
                                                nha_mang = "VietnamMobile";
                                                break;
                                            case "07":
                                                nha_mang = "GMobile";
                                                break;
                                            case "08":
                                                nha_mang = "DongDuongTelecom";
                                                break;
                                            default:
                                                nha_mang = "";
                                                break;
                                        }
                                        if (!string.IsNullOrEmpty(nha_mang))
                                        {
                                            CHUYEN_MANG_GIU_SO detail = new CHUYEN_MANG_GIU_SO();
                                            detail.SDT = indexSDT;
                                            detail.ID_PROCESS = id;
                                            detail.LOAI_NHA_MANG = nha_mang;
                                            ChuyenMangGiuSoBO chuyenMangGiuSoBO = new ChuyenMangGiuSoBO();
                                            chuyenMangGiuSoBO.update(detail, null);
                                        }
                                        else
                                        {
                                            XuLy.ghilog("LogRun", "Mã nhà mạng không đúng " + ma_nha_mang);
                                        }
                                    }
                                    else
                                    {
                                        XuLy.ghilog("LogRun", "Giá trị không hợp lệ");
                                    }
                                }
                                else if (strLineValue.Contains("Count of Telephone Numbers"))
                                {
                                    idstep = 0;
                                }
                                else
                                {
                                    if (idstep >= 1000)
                                    {

                                        id = id - idstep-3;
                                        idstep = 0;
                                    }
                                    else
                                    {
                                        idstep = idstep + 1;
                                    }
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                XuLy.ghilog("LogRun", ex.ToString());
                                Thread.Sleep(10);
                            }
                        }
                    }
                    // Read the content.  
                    //string responseFromServer = reader.ReadToEnd();
                    // Display the content.  
                    // Clean up the streams and the response.  
                    reader.Close();
                    response.Close();
                    #endregion
                    id++;
                }
                catch (Exception ex)
                {
                    if (idstep >= 1000)
                    {

                        id = id - idstep-2;
                        idstep = 0;
                    }
                    else
                    {
                        idstep = idstep + 1;
                        id++;
                    }
                    XuLy.ghilog("LogRun", ex.ToString());
                }
                XuLy.ghilog("LogRun", "Stop turn");
                Thread.Sleep(1000);
            }
        }
        public long removeHeadPhone(string phone)
        {
            long index_sdt = 0;
            try
            {
                if (!string.IsNullOrEmpty(phone))
                {
                    if (phone.IndexOf("84") == 0)
                        index_sdt = Convert.ToInt64(phone.Substring(2, phone.Length - 2));
                    else if (phone.IndexOf("0") == 0)
                        index_sdt = Convert.ToInt64(phone.Substring(1, phone.Length - 1));
                    else index_sdt = Convert.ToInt64(phone);
                }
            }
            catch { }
            return index_sdt;
        }
        protected override void OnStop()
        {
            is_stop = true;
        }
    }
}
