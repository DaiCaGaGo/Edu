using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace CMS
{
    public partial class ZaloCallbackGV : System.Web.UI.Page
    {
        MapZaloGiaoVienBO mapZaloGiaoVienBO = new MapZaloGiaoVienBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string header = HttpContext.Current.Request.Headers.Get("X-ZEvent-Signature");

                string reqBody;
                using (StreamReader reader = new StreamReader(HttpContext.Current.Request.InputStream))
                {
                    reqBody = reader.ReadToEnd();
                }
                ghilog("logGV", DateTime.Now + ": " + reqBody);
                var dataJson = new JavaScriptSerializer().Serialize(reqBody);
                var data = (JObject)JsonConvert.DeserializeObject(reqBody);
                string strAppID = data["app_id"].ToString();
                string fromuid = data["sender"]["id"].ToString();
                string strEvent = data["event_name"].ToString();
                string strMessage = data["message"]["text"].ToString();
                string oaID = data["recipient"]["id"].ToString();
                ghilog("logGV", DateTime.Now + ": " + "strAppID=" + strAppID + ",fromuid=" + fromuid + ",strEvent=" + strEvent + ",strMessage=" + strMessage);

                int id_nam_hoc = Convert.ToInt16(DateTime.Now.Year);
                int hoc_ky = 1;
                int thang = DateTime.Now.Month;
                if (thang >= 1 && thang < 9)
                {
                    id_nam_hoc = id_nam_hoc - 1;
                    hoc_ky = 2;
                }

                if (!string.IsNullOrEmpty(strMessage) && !string.IsNullOrEmpty(fromuid))
                {
                    #region Thao tác menu
                    if (strEvent == "user_send_text")
                    {
                        try
                        {
                            #region Thời khóa biểu lớp
                            if (strMessage == "#tkbLop")
                            {
                                List<ZaloTraCuuEntity> lstLop = mapZaloGiaoVienBO.traCuuThoiKhoaBieuLop(fromuid, (Int16)id_nam_hoc);
                                if (lstLop.Count > 0)
                                {
                                    JObject jObject = new
                                    JObject(
                                        new JProperty("recipient",
                                            new JObject(new JProperty("user_id", fromuid))
                                        ),
                                        new JProperty("message",
                                            new JObject(
                                                new JProperty("text", "Chọn lớp để xem TKB"),
                                                new JProperty("attachment",
                                                    new JObject(
                                                        new JProperty("type", "template"),
                                                        new JProperty("payload", new JObject(
                                                            new JProperty("buttons",
                                                                new JArray(
                                                                    from p in lstLop
                                                                    select new JObject(
                                                                        new JProperty("title", p.title),
                                                                        new JProperty("payload", new JObject(new JProperty("url", p.url))),
                                                                        new JProperty("type", p.type)
                                                                        )
                                                                ))
                                                            ))
                                                    )
                                                )
                                            )
                                    ));
                                    getData1(jObject);
                                }
                                else
                                {
                                    var postData = new
                                    {
                                        recipient = new
                                        {
                                            user_id = fromuid
                                        },
                                        message = new
                                        {
                                            text = "Nếu chưa đăng ký, vui lòng chọn menu 'Đăng ký' để đăng ký!"
                                        }
                                    };
                                    getData(postData);
                                }
                            }
                            #endregion
                            #region Lịch thi lớp
                            else if (strMessage == "#lichthi")
                            {
                                List<ZaloTraCuuEntity> lstLop = mapZaloGiaoVienBO.traLichThiLop(fromuid, (Int16)id_nam_hoc);
                                if (lstLop.Count > 0)
                                {
                                    JObject jObject = new
                                    JObject(
                                        new JProperty("recipient",
                                            new JObject(new JProperty("user_id", fromuid))
                                        ),
                                        new JProperty("message",
                                            new JObject(
                                                new JProperty("text", "Chọn lớp để xem lịch thi"),
                                                new JProperty("attachment",
                                                    new JObject(
                                                        new JProperty("type", "template"),
                                                        new JProperty("payload", new JObject(
                                                            new JProperty("buttons",
                                                                new JArray(
                                                                    from p in lstLop
                                                                    select new JObject(
                                                                        new JProperty("title", p.title),
                                                                        new JProperty("payload", new JObject(new JProperty("url", p.url))),
                                                                        new JProperty("type", p.type)
                                                                        )
                                                                ))
                                                            ))
                                                    )
                                                )
                                            )
                                    ));
                                    getData1(jObject);
                                }
                                else
                                {
                                    var postData = new
                                    {
                                        recipient = new
                                        {
                                            user_id = fromuid
                                        },
                                        message = new
                                        {
                                            text = "Nếu chưa đăng ký, vui lòng chọn menu 'Đăng ký' để đăng ký!"
                                        }
                                    };
                                    getData(postData);
                                }
                            }
                            #endregion
                            #region Xem dặn dò - BTVN
                            else if (strMessage == "#btvn")
                            {
                                List<ZaloTraCuuEntity> lstLop = mapZaloGiaoVienBO.viewBaiTapVeNha(fromuid, (Int16)id_nam_hoc);
                                if (lstLop.Count > 0)
                                {
                                    JObject jObject = new
                                    JObject(
                                        new JProperty("recipient",
                                            new JObject(new JProperty("user_id", fromuid))
                                        ),
                                        new JProperty("message",
                                            new JObject(
                                                new JProperty("text", "Chọn lớp để xem BTVN"),
                                                new JProperty("attachment",
                                                    new JObject(
                                                        new JProperty("type", "template"),
                                                        new JProperty("payload", new JObject(
                                                            new JProperty("buttons",
                                                                new JArray(
                                                                    from p in lstLop
                                                                    select new JObject(
                                                                        new JProperty("title", p.title),
                                                                        new JProperty("payload", new JObject(new JProperty("url", p.url))),
                                                                        new JProperty("type", p.type)
                                                                        )
                                                                ))
                                                            ))
                                                    )
                                                )
                                            )
                                    ));
                                    getData1(jObject);
                                }
                                else
                                {
                                    var postData = new
                                    {
                                        recipient = new
                                        {
                                            user_id = fromuid
                                        },
                                        message = new
                                        {
                                            text = "Nếu chưa đăng ký, vui lòng chọn menu 'Đăng ký' để đăng ký!"
                                        }
                                    };
                                    getData(postData);
                                }
                            }
                            #endregion
                            #region Danh bạ Giáo viên trong trường
                            else if (strMessage == "#gvTruong")
                            {
                                List<ZaloTraCuuEntity> lstLop = mapZaloGiaoVienBO.traCuuDanhBaGiaoVienTruong(fromuid, (Int16)id_nam_hoc);
                                if (lstLop.Count > 0)
                                {
                                    JObject jObject = new
                                    JObject(
                                        new JProperty("recipient",
                                            new JObject(new JProperty("user_id", fromuid))
                                        ),
                                        new JProperty("message",
                                            new JObject(
                                                new JProperty("text", "Xem danh sách giáo viên"),
                                                new JProperty("attachment",
                                                    new JObject(
                                                        new JProperty("type", "template"),
                                                        new JProperty("payload", new JObject(
                                                            new JProperty("buttons",
                                                                new JArray(
                                                                    from p in lstLop
                                                                    select new JObject(
                                                                        new JProperty("title", p.title),
                                                                        new JProperty("payload", new JObject(new JProperty("url", p.url))),
                                                                        new JProperty("type", p.type)
                                                                        )
                                                                ))
                                                            ))
                                                    )
                                                )
                                            )
                                    ));
                                    getData1(jObject);
                                }
                                else
                                {
                                    var postData = new
                                    {
                                        recipient = new
                                        {
                                            user_id = fromuid
                                        },
                                        message = new
                                        {
                                            text = "Nếu chưa đăng ký, vui lòng chọn menu 'Đăng ký' để đăng ký!"
                                        }
                                    };
                                    getData(postData);
                                }
                            }
                            #endregion
                            #region Danh bạ GV bộ môn lớp
                            else if (strMessage == "#gvLopCN")
                            {
                                List<ZaloTraCuuEntity> lstLop = mapZaloGiaoVienBO.traCuuDanhBaGiaoVienBoMon(fromuid, (Int16)id_nam_hoc);
                                if (lstLop.Count > 0)
                                {
                                    JObject jObject = new
                                    JObject(
                                        new JProperty("recipient",
                                            new JObject(new JProperty("user_id", fromuid))
                                        ),
                                        new JProperty("message",
                                            new JObject(
                                                new JProperty("text", "Chọn lớp để xem danh bạ GVBM"),
                                                new JProperty("attachment",
                                                    new JObject(
                                                        new JProperty("type", "template"),
                                                        new JProperty("payload", new JObject(
                                                            new JProperty("buttons",
                                                                new JArray(
                                                                    from p in lstLop
                                                                    select new JObject(
                                                                        new JProperty("title", p.title),
                                                                        new JProperty("payload", new JObject(new JProperty("url", p.url))),
                                                                        new JProperty("type", p.type)
                                                                        )
                                                                ))
                                                            ))
                                                    )
                                                )
                                            )
                                    ));
                                    getData1(jObject);
                                }
                                else
                                {
                                    var postData = new
                                    {
                                        recipient = new
                                        {
                                            user_id = fromuid
                                        },
                                        message = new
                                        {
                                            text = "Nếu chưa đăng ký, vui lòng chọn menu 'Đăng ký' để đăng ký!"
                                        }
                                    };
                                    getData(postData);
                                }
                            }
                            #endregion
                            #region Danh bạ Phụ huynh
                            else if (strMessage == "#phuhuynh")
                            {
                                List<ZaloTraCuuEntity> lstLop = mapZaloGiaoVienBO.traCuuDanhBaChiHoiPhuHuynh(fromuid, (Int16)id_nam_hoc);
                                if (lstLop.Count > 0)
                                {
                                    JObject jObject = new
                                    JObject(
                                        new JProperty("recipient",
                                            new JObject(new JProperty("user_id", fromuid))
                                        ),
                                        new JProperty("message",
                                            new JObject(
                                                new JProperty("text", "Chọn lớp để xem danh bạ PH"),
                                                new JProperty("attachment",
                                                    new JObject(
                                                        new JProperty("type", "template"),
                                                        new JProperty("payload", new JObject(
                                                            new JProperty("buttons",
                                                                new JArray(
                                                                    from p in lstLop
                                                                    select new JObject(
                                                                        new JProperty("title", p.title),
                                                                        new JProperty("payload", new JObject(new JProperty("url", p.url))),
                                                                        new JProperty("type", p.type)
                                                                        )
                                                                ))
                                                            ))
                                                    )
                                                )
                                            )
                                    ));
                                    getData1(jObject);
                                }
                                else
                                {
                                    var postData = new
                                    {
                                        recipient = new
                                        {
                                            user_id = fromuid
                                        },
                                        message = new
                                        {
                                            text = "Nếu chưa đăng ký, vui lòng chọn menu 'Đăng ký' để đăng ký!"
                                        }
                                    };
                                    getData(postData);
                                }
                            }
                            #endregion
                            #region Lịch dạy giáo viên
                            if (strMessage == "#lichday")
                            {
                                List<ZaloTraCuuEntity> lstLop = mapZaloGiaoVienBO.traCuuThoiKhoaBieuGiaoVien(fromuid, (Int16)id_nam_hoc);
                                if (lstLop.Count > 0)
                                {
                                    JObject jObject = new
                                    JObject(
                                        new JProperty("recipient",
                                            new JObject(new JProperty("user_id", fromuid))
                                        ),
                                        new JProperty("message",
                                            new JObject(
                                                new JProperty("text", "Thời khóa biểu giáo viên"),
                                                new JProperty("attachment",
                                                    new JObject(
                                                        new JProperty("type", "template"),
                                                        new JProperty("payload", new JObject(
                                                            new JProperty("buttons",
                                                                new JArray(
                                                                    from p in lstLop
                                                                    select new JObject(
                                                                        new JProperty("title", p.title),
                                                                        new JProperty("payload", new JObject(new JProperty("url", p.url))),
                                                                        new JProperty("type", p.type)
                                                                        )
                                                                ))
                                                            ))
                                                    )
                                                )
                                            )
                                    ));
                                    getData1(jObject);
                                }
                                else
                                {
                                    var postData = new
                                    {
                                        recipient = new
                                        {
                                            user_id = fromuid
                                        },
                                        message = new
                                        {
                                            text = "Nếu chưa đăng ký, vui lòng chọn menu 'Đăng ký' để đăng ký!"
                                        }
                                    };
                                    getData(postData);
                                }
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            ghilog("logGV", ex.ToString());
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ghilog("logGV", ex.ToString());
            }
        }

        protected void getData(object postData)
        {
            HttpWebRequest req = null;
            HttpWebResponse res = null;
            req = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["tokenPH"]);
            req.Method = "POST";
            req.ContentType = "application/json";

            var dataJson = new JavaScriptSerializer().Serialize(postData);
            ghilog("logGV", dataJson);
            byte[] bytes = Encoding.UTF8.GetBytes(dataJson);
            req.ContentLength = bytes.Length;

            Stream dataStream = req.GetRequestStream();
            dataStream.Write(bytes, 0, bytes.Length);
            dataStream.Close();

            res = (HttpWebResponse)req.GetResponse();
            Console.WriteLine(((HttpWebResponse)res).StatusDescription);
            dataStream = res.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            Console.WriteLine(responseFromServer);
            ghilog("logGV", responseFromServer);
            reader.Close();
            dataStream.Close();
            res.Close();
        }
        protected void getData1(JObject postData)
        {
            HttpWebRequest req = null;
            HttpWebResponse res = null;
            req = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["tokenPH"]);
            req.Method = "POST";
            req.ContentType = "application/json";

            //var dataJson = new JavaScriptSerializer().Serialize(postData);
            var dataJson = JsonConvert.SerializeObject(postData);
            ghilog("logGV", dataJson);
            byte[] bytes = Encoding.UTF8.GetBytes(dataJson);
            req.ContentLength = bytes.Length;

            Stream dataStream = req.GetRequestStream();
            dataStream.Write(bytes, 0, bytes.Length);
            dataStream.Close();

            res = (HttpWebResponse)req.GetResponse();
            Console.WriteLine(((HttpWebResponse)res).StatusDescription);
            dataStream = res.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            Console.WriteLine(responseFromServer);
            ghilog("logGV", responseFromServer);
            reader.Close();
            dataStream.Close();
            res.Close();
        }
        public static void ghilog(string nameFile, string msg)
        {
            try
            {
                string path = "";
                DateTime dt = new DateTime();
                dt = DateTime.Now;
                string foldername = dt.ToString();
                path = "D:/LogEduZalo/" + dt.Year.ToString() + dt.Month.ToString() + dt.Day.ToString() + dt.Hour.ToString() + nameFile + ".txt";
                using (StreamWriter sw = new StreamWriter(path, true))
                {
                    string str = msg;
                    sw.WriteLine(str);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}