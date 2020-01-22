using DevExpress.Office.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZaloCSharpSDK;

namespace CMS
{
    public partial class zalocallback : System.Web.UI.Page
    {
        MapPhuHuynhHocSinhBO mapPhuHuynhHocSinhBO = new MapPhuHuynhHocSinhBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ghilog("CallbackZalo", Request.Url.ToString());
                string strEvent = Request.QueryString.Get("event");
                string str_message = Request.QueryString.Get("message");
                string fromuid = Request.QueryString.Get("fromuid");
                string oaid = Request.QueryString.Get("oaid");
                string appid = Request.QueryString.Get("appid");
                ghilog("getDAtafromCallback", "event=" + strEvent + ", message=" + str_message + ", fromuid=" + fromuid);

                int id_nam_hoc = Convert.ToInt16(DateTime.Now.Year);
                int hoc_ky = 1;
                int thang = DateTime.Now.Month;
                if (thang >= 1 && thang < 9)
                {
                    id_nam_hoc = id_nam_hoc - 1;
                    hoc_ky = 2;
                }

                if (!string.IsNullOrEmpty(strEvent) && !string.IsNullOrEmpty(fromuid))
                {
                    #region Thao tác menu
                    if (strEvent == "sendmsg")
                    {
                        try
                        {
                            #region Xem điểm
                            if (str_message == "#diem")
                            {
                                HocSinhBO hocSinhBO = new HocSinhBO();
                                List<ZaloTraCuuEntity> lstHocSinhButton = mapPhuHuynhHocSinhBO.traCuuKetQuaHocTap(fromuid, (Int16)id_nam_hoc);
                                if (lstHocSinhButton.Count > 0)
                                {
                                    JObject jObject = new
                                    JObject(
                                        new JProperty("recipient",
                                            new JObject(new JProperty("user_id", fromuid))
                                        ),
                                        new JProperty("message",
                                            new JObject(
                                                new JProperty("text", "Chọn HS để xem KQ"),
                                                new JProperty("attachment",
                                                    new JObject(
                                                        new JProperty("type", "template"),
                                                        new JProperty("payload", new JObject(
                                                            new JProperty("buttons",
                                                                new JArray(
                                                                    from p in lstHocSinhButton
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
                                            text = "Bạn chưa đăng ký. Vui lòng chọn menu 'Đăng ký' để đăng ký xem thông tin của con!"
                                        }
                                    };
                                    getData(postData);
                                }
                            }
                            #endregion
                            #region "danh bạ GV"
                            else if (str_message == "#danhbagv")
                            {
                                List<ZaloTraCuuEntity> lstLop = mapPhuHuynhHocSinhBO.traCuuDanhBaGiaoVien(fromuid, (Int16)id_nam_hoc);
                                if (lstLop.Count > 0)
                                {
                                    JObject jObject = new
                                    JObject(
                                        new JProperty("recipient",
                                            new JObject(new JProperty("user_id", fromuid))
                                        ),
                                        new JProperty("message",
                                            new JObject(
                                                new JProperty("text", "Chọn lớp để tra danh bạ Giáo viên"),
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
                                            text = "Bạn chưa đăng ký. Vui lòng chọn menu 'Đăng ký' để đăng ký xem thông tin của con!"
                                        }
                                    };
                                    getData(postData);
                                }
                            }
                            #endregion
                            #region danh bạ Chi hội PH
                            else if (str_message == "#danhbaph")
                            {
                                List<ZaloTraCuuEntity> lstLop = mapPhuHuynhHocSinhBO.traCuuDanhBaChiHoiPhuHuynh(fromuid, (Int16)id_nam_hoc);
                                if (lstLop.Count > 0)
                                {
                                    JObject jObject = new
                                    JObject(
                                        new JProperty("recipient",
                                            new JObject(new JProperty("user_id", fromuid))
                                        ),
                                        new JProperty("message",
                                            new JObject(
                                                new JProperty("text", "Chọn lớp để tra danh bạ Chi hội PH"),
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
                                            text = "Bạn chưa đăng ký. Vui lòng chọn menu 'Đăng ký' để đăng ký xem thông tin của con!"
                                        }
                                    };
                                    getData(postData);
                                }
                            }
                            #endregion
                            #region Thời khóa biểu lớp
                            else if (str_message == "#tkb")
                            {
                                List<ZaloTraCuuEntity> lstLop = mapPhuHuynhHocSinhBO.traCuuThoiKhoaBieuLop(fromuid, (Int16)id_nam_hoc);
                                if (lstLop.Count > 0)
                                {
                                    JObject jObject = new
                                    JObject(
                                        new JProperty("recipient",
                                            new JObject(new JProperty("user_id", fromuid))
                                        ),
                                        new JProperty("message",
                                            new JObject(
                                                new JProperty("text", "Chọn lớp để xem TKB của con"),
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
                                            text = "Bạn chưa đăng ký. Vui lòng chọn menu 'Đăng ký' để đăng ký xem thông tin của con!"
                                        }
                                    };
                                    getData(postData);
                                }
                            }
                            #endregion
                            #region Tin tức, sự kiện theo trường
                            else if (str_message == "#tintuc")
                            {
                                List<ZaloTraCuuEntity> lstLop = mapPhuHuynhHocSinhBO.traCuuTinTucTheoUser(fromuid, (Int16)id_nam_hoc);
                                if (lstLop.Count > 0)
                                {
                                    JObject jObject = new
                                    JObject(
                                        new JProperty("recipient",
                                            new JObject(new JProperty("user_id", fromuid))
                                        ),
                                        new JProperty("message",
                                            new JObject(
                                                new JProperty("attachment",
                                                    new JObject(
                                                        new JProperty("type", "template"),
                                                        new JProperty("payload", new JObject(
                                                            new JProperty("template_type", "list"),
                                                            new JProperty("elements",
                                                                new JArray(
                                                                    from p in lstLop
                                                                    select new JObject(
                                                                        new JProperty("title", p.title),
                                                                        new JProperty("subtitle", p.subtitle),
                                                                        new JProperty("image_url", p.image_url),
                                                                        new JProperty("default_action",
                                                                            new JObject(new JProperty("type", p.type),
                                                                                        new JProperty("url", p.url))
                                                                        )
                                                                    )
                                                                )
                                                            )
                                                        )
                                                    )
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
                                            text = "Chưa có dữ liệu!"
                                        }
                                    };
                                    getData(postData);
                                }
                            }
                            #endregion
                            #region Bài tập về nhà
                            else if (str_message == "#btvn")
                            {
                                List<ZaloTraCuuEntity> lstLop = mapPhuHuynhHocSinhBO.viewBaiTapVeNha(fromuid, (Int16)id_nam_hoc);
                                if (lstLop.Count > 0)
                                {
                                    JObject jObject = new
                                    JObject(
                                        new JProperty("recipient",
                                            new JObject(new JProperty("user_id", fromuid))
                                        ),
                                        new JProperty("message",
                                            new JObject(
                                                new JProperty("text", "Chọn lớp để xem bài tập của con"),
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
                                            text = "Bạn chưa đăng ký. Vui lòng chọn menu 'Đăng ký' để đăng ký xem thông tin của con!"
                                        }
                                    };
                                    getData(postData);
                                }
                            }
                            #endregion
                            #region Lich thi
                            else if (str_message == "#lichthi")
                            {

                                List<ZaloTraCuuEntity> lstLop = mapPhuHuynhHocSinhBO.traLichThiLop(fromuid, (Int16)id_nam_hoc);
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
                                            text = "Bạn chưa đăng ký. Vui lòng chọn menu 'Đăng ký' để đăng ký xem thông tin của con!"
                                        }
                                    };
                                    getData(postData);
                                }
                            }
                            #endregion
                            #region Thông báo chung
                            else if (str_message == "#thongbao")
                            {

                                List<ZaloTraCuuEntity> lstLop = mapPhuHuynhHocSinhBO.traThongBaoTruong(fromuid, (Int16)id_nam_hoc, 1);
                                if (lstLop.Count > 0)
                                {
                                    JObject jObject = new
                                    JObject(
                                        new JProperty("recipient",
                                            new JObject(new JProperty("user_id", fromuid))
                                        ),
                                        new JProperty("message",
                                            new JObject(
                                                new JProperty("text", "Chọn trường để xem thông báo"),
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
                                            text = "Bạn chưa đăng ký. Vui lòng chọn menu 'Đăng ký' để đăng ký xem thông tin của con!"
                                        }
                                    };
                                    getData(postData);
                                }
                            }
                            #endregion
                            #region Giờ vào lớp, đưa đón học sinh
                            else if (str_message == "#giogiac")
                            {

                                List<ZaloTraCuuEntity> lstLop = mapPhuHuynhHocSinhBO.traThongBaoTruong(fromuid, (Int16)id_nam_hoc, 2);
                                if (lstLop.Count > 0)
                                {
                                    JObject jObject = new
                                    JObject(
                                        new JProperty("recipient",
                                            new JObject(new JProperty("user_id", fromuid))
                                        ),
                                        new JProperty("message",
                                            new JObject(
                                                new JProperty("text", "Chọn trường để xem giờ đưa đón con"),
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
                                            text = "Bạn chưa đăng ký. Vui lòng chọn menu 'Đăng ký' để đăng ký xem thông tin của con!"
                                        }
                                    };
                                    getData(postData);
                                }
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            ghilog("CallbackZalo", ex.ToString());
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ghilog("CallbackZalo", ex.ToString());
            }
        }
        protected void getData(object postData)
        {
            HttpWebRequest req = null;
            HttpWebResponse res = null;
            req = (HttpWebRequest)WebRequest.Create("https://openapi.zalo.me/v2.0/oa/message?access_token=UHaCG824G55s7Gz2Z8H0KtiT3aA4cWKfSc4nPekPBpCRGW5qijy8A2G_FI2HsWDaDHaeR8FYC2iF9XXcaVSG17DpFapax2eV1ZTYJuRdLXa08KbwjUzFFafqSd3kjtXWOsPH4l-MLtPlQde6pf0pJ4zGF2tKg3W-QL1yVU2wP6mOHrK1jQbQTY0p4cM6o0qd0I05JvZlC0GD9JLiihG0FnGmAa-Mm64v3YPxHFJgS1bhDc9mb_CfPmu53J6RnY4vB1qCTuxT7m8CHL9mpwby9OMUH5G");
            req.Method = "POST";
            req.ContentType = "application/json";

            var dataJson = new JavaScriptSerializer().Serialize(postData);
            ghilog("getDAtafromCallback", dataJson);
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
            ghilog("getDAtafromCallback", responseFromServer);
            reader.Close();
            dataStream.Close();
            res.Close();
        }
        protected void getData1(JObject postData)
        {
            HttpWebRequest req = null;
            HttpWebResponse res = null;
            req = (HttpWebRequest)WebRequest.Create("https://openapi.zalo.me/v2.0/oa/message?access_token=UHaCG824G55s7Gz2Z8H0KtiT3aA4cWKfSc4nPekPBpCRGW5qijy8A2G_FI2HsWDaDHaeR8FYC2iF9XXcaVSG17DpFapax2eV1ZTYJuRdLXa08KbwjUzFFafqSd3kjtXWOsPH4l-MLtPlQde6pf0pJ4zGF2tKg3W-QL1yVU2wP6mOHrK1jQbQTY0p4cM6o0qd0I05JvZlC0GD9JLiihG0FnGmAa-Mm64v3YPxHFJgS1bhDc9mb_CfPmu53J6RnY4vB1qCTuxT7m8CHL9mpwby9OMUH5G");
            req.Method = "POST";
            req.ContentType = "application/json";

            //var dataJson = new JavaScriptSerializer().Serialize(postData);
            var dataJson = JsonConvert.SerializeObject(postData);
            ghilog("getDAtafromCallback", dataJson);
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
            ghilog("getDAtafromCallback", responseFromServer);
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