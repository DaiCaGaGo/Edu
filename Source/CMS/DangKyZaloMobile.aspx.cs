using CMS.XuLy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ZaloCSharpSDK;

namespace CMS
{
    public partial class DangKyZaloMobile : System.Web.UI.Page
    {
        public LocalAPI LocalAPI = new LocalAPI();
        MapPhuHuynhHocSinhBO mapPhuHuynhHocSinhBO = new MapPhuHuynhHocSinhBO();
        HocSinhBO hocSinhBO = new HocSinhBO();
        TinNhanBO tinNhanBO = new TinNhanBO();
        TruongBO truongBO = new TruongBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                //string str_uid = HduserID.Value;
                //ghilog("getDAtafromCallback", "event=dang_ky, fromuid=" + str_uid);
            }
        }
        #region action
        #endregion
        protected string makeRandomPassword(int length)
        {
            string UpperCase = "QWERTYUIOPASDFGHJKLZXCVBNM";
            string LowerCase = "qwertyuiopasdfghjklzxcvbnm";
            string Digits = "1234567890";
            string allCharacters = UpperCase + LowerCase + Digits;
            Random r = new Random();
            String password = "";
            for (int i = 0; i < length; i++)
            {
                double rand = r.NextDouble();
                if (i == 0)
                {
                    password += UpperCase.ToCharArray()[(int)Math.Floor(rand * UpperCase.Length)];
                }
                else
                {
                    password += allCharacters.ToCharArray()[(int)Math.Floor(rand * allCharacters.Length)];
                }
            }
            return password;
        }
        protected TIN_NHAN addTinNhan(string sdt_nhan_tin, string loai_nha_mang, string noi_dung_gui)
        {
            string brand_name = "ONEDU", cp = "";
            TRUONG truong = truongBO.getTruongById(1);
            if (loai_nha_mang == "Viettel") cp = truong.CP_VIETTEL;
            else if (loai_nha_mang == "GMobile") cp = truong.CP_GTEL;
            else if (loai_nha_mang == "MobiFone") cp = truong.CP_MOBI;
            else if (loai_nha_mang == "VinaPhone") cp = truong.CP_VINA;
            else if (loai_nha_mang == "VietnamMobile") cp = truong.CP_VNM;
            TIN_NHAN tinNhan = new TIN_NHAN();
            tinNhan.ID_TRUONG = 1;//trường onedu
            tinNhan.LOAI_NGUOI_NHAN = 3;//PH học sinh, tin xác nhận OTP
            tinNhan.SDT_NHAN = sdt_nhan_tin;
            tinNhan.LOAI_NHA_MANG = loai_nha_mang;
            tinNhan.BRAND_NAME = brand_name;
            tinNhan.CP = cp;
            tinNhan.NOI_DUNG = noi_dung_gui;
            tinNhan.NOI_DUNG_KHONG_DAU = LocalAPI.chuyenTiengVietKhongDau(noi_dung_gui);
            tinNhan.SO_TIN = 1;
            tinNhan.THOI_GIAN_GUI = DateTime.Now;
            tinNhan.KIEU_GUI = 2;//gửi zalo
            tinNhan.NGAY_TAO = DateTime.Now;
            tinNhan.LOAI_TIN = 3;//nhận OTP xác nhận đk zalo
            tinNhan.NAM_GUI = Convert.ToInt16(DateTime.Now.Year);
            tinNhan.THANG_GUI = Convert.ToInt16(DateTime.Now.Month);
            tinNhan.TUAN_GUI = Convert.ToInt16(LocalAPI.getThisWeek().ToString());
            tinNhan.IS_SMS_ZALO_OTP = true;
            return tinNhan;
        }
        public string GetIPAddress()
        {
            string ipaddress;
            ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (ipaddress == "" || ipaddress == null)
                ipaddress = Request.ServerVariables["REMOTE_ADDR"];
            return ipaddress;
        }
        public void ghilog(string nameFile, string msg)
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
            catch
            {

            }
        }
        protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
        {

        }
        protected void btn1_Click(object sender, EventArgs e)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";

            string sdt_map = tbSDTMap.Text.Trim();
            sdt_map = LocalAPI.Add84(sdt_map);
            string loai_nha_mang = LocalAPI.getLoaiNhaMang(sdt_map);

            if (string.IsNullOrEmpty(loai_nha_mang))
            {
                lblThongBao.Text = "SĐT đăng ký chưa đúng! Vui lòng kiểm tra lại!";
                return;
            }

            string sdt_nhan_tin = tbSDTNhanSMS.Text.Trim();
            sdt_nhan_tin = LocalAPI.Add84(sdt_nhan_tin);
            string nha_mang_nhan_tin = LocalAPI.getLoaiNhaMang(sdt_nhan_tin);

            if (!string.IsNullOrEmpty(sdt_nhan_tin) && string.IsNullOrEmpty(nha_mang_nhan_tin))
            {
                lblThongBao.Text += "Số điện thoại nhận SMS hàng ngày chưa đúng. Vui lòng kiểm tra lại";
                return;
            }
            string ipAddress = GetIPAddress();
            ghilog("logAction", DateTime.Now + ": Phone: " + tbSDTMap.Text + ", event=xacNhanHS, fromuid=" + HduserID.Value + ", id_hoc_sinh=" + hddHS1.Value);
            if (HduserID.Value != "" && HduserID.Value != "0" && !string.IsNullOrEmpty(hddHS1.Value))
            {
                HOC_SINH hocSinh = hocSinhBO.getHocSinhByID(Convert.ToInt64(hddHS1.Value));
                if (hocSinh == null)
                {
                    lblThongBao.Text = "Có lỗi xảy ra! Vui lòng liên hệ với quản trị viên để được giúp đỡ.";
                    return;
                }

                #region "Số đăng ký là số nhận tin nhắn hàng ngày"
                if (string.IsNullOrEmpty(sdt_nhan_tin) || sdt_map == sdt_nhan_tin)
                {
                    MAP_PH_HS hocSinhMap = mapPhuHuynhHocSinhBO.getHocSinhByMaAndPhone(sdt_map, hocSinh.MA);
                    if (hocSinhMap == null)
                    {
                        hocSinhMap = new MAP_PH_HS();
                        hocSinhMap.SDT_MAP = sdt_map;
                        hocSinhMap.MA_HOC_SINH = hocSinh.MA;
                        hocSinhMap.TRANG_THAI = true;
                        hocSinhMap.IP_ADDRESS = ipAddress;
                        hocSinhMap.ZALO_USER_ID = HduserID.Value;
                        res = mapPhuHuynhHocSinhBO.insert(hocSinhMap);
                        if (res.Res)
                        {
                            lblThongBao.Text = "Đăng ký thành công!";
                            btn1.Visible = false;
                            lblThongBao.ForeColor = Color.Blue;
                        }
                        else
                        {
                            lblThongBao.Text = "Có lỗi xảy ra! Vui lòng liên hệ với quản trị viên để được giúp đỡ.";
                            return;
                        }
                    }
                    else
                    {
                        if (hocSinhMap.TRANG_THAI == null || hocSinhMap.TRANG_THAI == false)
                        {
                            hocSinhMap.TRANG_THAI = true;
                            res = mapPhuHuynhHocSinhBO.update(hocSinhMap);
                            if (res.Res)
                            {
                                lblThongBao.Text = "Đăng ký thành công!";
                                btn1.Visible = false;
                                lblThongBao.ForeColor = Color.Blue;
                                return;
                            }
                            else
                            {
                                lblThongBao.Text = "Có lỗi xảy ra! Vui lòng liên hệ với quản trị viên để được giúp đỡ.";
                                return;
                            }
                        }
                        else
                        {
                            lblThongBao.Text = "SĐT này đã đăng ký thành công!";
                            lblThongBao.ForeColor = Color.Blue;
                            return;
                        }
                    }
                }
                #endregion
                #region "Số đăng ký khác số nhận SMS hàng ngày"
                else if (!string.IsNullOrEmpty(nha_mang_nhan_tin) && sdt_map != sdt_nhan_tin)
                {
                    short counter = 0;
                    DateTime? ngay_gui_otp = hocSinh.NGAY_GUI_OTP;
                    string strNgayOTP = ngay_gui_otp != null ? Convert.ToDateTime(ngay_gui_otp).ToString("yyyyMMdd") : "";
                    if (strNgayOTP == DateTime.Now.ToString("yyyyMMdd"))
                        counter = hocSinh.OTP_COUNTER != null ? hocSinh.OTP_COUNTER.Value : (Int16)0;

                    if (counter < 3)
                    {
                        MAP_PH_HS hocSinhMap = mapPhuHuynhHocSinhBO.getHocSinhByMaAndPhone(sdt_map, hocSinh.MA);
                        string ma_xac_nhan = makeRandomPassword(6);
                        if (hocSinhMap == null)
                        {
                            hocSinhMap = new MAP_PH_HS();
                            hocSinhMap.SDT_MAP = sdt_map;
                            hocSinhMap.MA_HOC_SINH = hocSinh.MA;
                            hocSinhMap.TRANG_THAI = false;
                            hocSinhMap.IP_ADDRESS = ipAddress;
                            hocSinhMap.ZALO_USER_ID = HduserID.Value;
                            hocSinhMap.MA_BAO_MAT = ma_xac_nhan;
                            hocSinhMap.NGAY_GUI_MA = DateTime.Now;
                            res = mapPhuHuynhHocSinhBO.insert(hocSinhMap);
                            MAP_PH_HS resMap = (MAP_PH_HS)res.ResObject;
                            if (res.Res)
                            {
                                TIN_NHAN tinNhan = new TIN_NHAN();
                                string noi_dung_gui = "Mã xác nhận đăng ký tài khoản trên Zalo cho SĐT " + sdt_map + " là " + ma_xac_nhan;
                                tinNhan = addTinNhan(sdt_nhan_tin, nha_mang_nhan_tin, noi_dung_gui);

                                res = tinNhanBO.insertTinXacNhanZalo(tinNhan, null);
                                if (res.Res)
                                {
                                    //update số lần gửi mã OTP
                                    hocSinh.NGAY_GUI_OTP = DateTime.Now;
                                    hocSinh.OTP_COUNTER = (Int16)(counter + 1);
                                    hocSinhBO.update(hocSinh, null);
                                }
                                Response.Redirect("DangKyZaloMobileConfirm.aspx?id=" + resMap.ID + "&sdt_map=" + sdt_map + "&ma_hs=" + hocSinh.MA);
                            }
                            else
                            {
                                lblThongBao.Text = "Có lỗi xảy ra! Vui lòng liên hệ với quản trị viên để được giúp đỡ.";
                                return;
                            }
                        }
                        else
                        {
                            if (hocSinhMap.TRANG_THAI == null || hocSinhMap.TRANG_THAI == false)
                            {
                                DateTime current_time = DateTime.Now;
                                DateTime gui_ma_time = DateTime.Now;
                                try
                                {
                                    gui_ma_time = Convert.ToDateTime(hocSinhMap.NGAY_GUI_MA);
                                }
                                catch { }
                                TimeSpan diff = current_time - gui_ma_time;
                                //Mã bảo mật có hiệu lực trong vòng 30p
                                if (diff.Minutes > 30)
                                {
                                    mapPhuHuynhHocSinhBO.updateMaXacNhan(hocSinhMap.ID, ma_xac_nhan);
                                    string noi_dung_gui = "Mã xác nhận đăng ký tài khoản trên Zalo cho SĐT " + sdt_map + " là " + ma_xac_nhan;
                                    TIN_NHAN tinNhan = addTinNhan(sdt_nhan_tin, loai_nha_mang, noi_dung_gui);
                                    res = tinNhanBO.insertTinXacNhanZalo(tinNhan, null);
                                    if (res.Res)
                                    {
                                        //update số lần gửi mã OTP
                                        hocSinh.NGAY_GUI_OTP = DateTime.Now;
                                        hocSinh.OTP_COUNTER = (Int16)(counter + 1);
                                        hocSinhBO.update(hocSinh, null);
                                    }
                                    Response.Redirect("DangKyZaloMobileConfirm.aspx?id=" + hocSinhMap.ID + "&sdt_map=" + sdt_map + "&ma_hs=" + hocSinh.MA + "&ip_address=" + ipAddress + "&userID_zalo=" + HduserID.Value);
                                }
                                else
                                {
                                    Response.Redirect("DangKyZaloMobileConfirm.aspx?id=" + hocSinhMap.ID + "&sdt_map=" + sdt_map + "&ma_hs=" + hocSinh.MA);
                                }
                            }
                            else
                            {
                                lblThongBao.Text = "SĐT này đã đăng ký thành công!";
                                lblThongBao.ForeColor = Color.Blue;
                                return;
                            }
                        }
                    }
                    else
                    {
                        lblThongBao.Text = "Bạn đã đăng ký vượt quá số lần truy cập/ngày. Vui lòng liên hệ với quản trị viên để được giúp đỡ!";
                        lblThongBao.ForeColor = Color.Red;
                        return;
                    }
                }
                #endregion
            }
            else
            {
                lblThongBao.Text = "có lỗi xảy ra. Vui lòng liên hệ với quản trị viên để được giúp đỡ.";
                return;
            }
        }
        protected void btn2_Click(object sender, EventArgs e)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";

            string sdt_map = tbSDTMap.Text.Trim();
            sdt_map = LocalAPI.Add84(sdt_map);
            string loai_nha_mang = LocalAPI.getLoaiNhaMang(sdt_map);

            if (string.IsNullOrEmpty(loai_nha_mang))
            {
                lblThongBao.Text = "SĐT đăng ký chưa đúng! Vui lòng kiểm tra lại!";
                return;
            }

            string sdt_nhan_tin = tbSDTNhanSMS.Text.Trim();
            sdt_nhan_tin = LocalAPI.Add84(sdt_nhan_tin);
            string nha_mang_nhan_tin = LocalAPI.getLoaiNhaMang(sdt_nhan_tin);

            if (!string.IsNullOrEmpty(sdt_nhan_tin) && string.IsNullOrEmpty(nha_mang_nhan_tin))
            {
                lblThongBao.Text += "Số điện thoại nhận SMS hàng ngày chưa đúng. Vui lòng kiểm tra lại";
                return;
            }


            string ipAddress = GetIPAddress();

            if (HduserID.Value != "" && HduserID.Value != "0" && !string.IsNullOrEmpty(hddHS2.Value))
            {
                HOC_SINH hocSinh = hocSinhBO.getHocSinhByID(Convert.ToInt64(hddHS2.Value));
                if (hocSinh == null)
                {
                    lblThongBao.Text = "Có lỗi xảy ra! Vui lòng liên hệ với quản trị viên để được giúp đỡ.";
                    return;
                }

                #region "Số đăng ký là số nhận tin nhắn hàng ngày"
                if (string.IsNullOrEmpty(sdt_nhan_tin))
                {
                    MAP_PH_HS hocSinhMap = mapPhuHuynhHocSinhBO.getHocSinhByMaAndPhone(sdt_map, hocSinh.MA);
                    if (hocSinhMap == null)
                    {
                        hocSinhMap = new MAP_PH_HS();
                        hocSinhMap.SDT_MAP = sdt_map;
                        hocSinhMap.MA_HOC_SINH = hocSinh.MA;
                        hocSinhMap.TRANG_THAI = true;
                        hocSinhMap.IP_ADDRESS = ipAddress;
                        hocSinhMap.ZALO_USER_ID = HduserID.Value;
                        res = mapPhuHuynhHocSinhBO.insert(hocSinhMap);
                        if (res.Res)
                        {
                            lblThongBao.Text = "Đăng ký thành công!";
                            btn2.Visible = false;
                            lblThongBao.ForeColor = Color.Blue;
                        }
                        else
                        {
                            lblThongBao.Text = "Có lỗi xảy ra! Vui lòng liên hệ với quản trị viên để được giúp đỡ.";
                            return;
                        }
                    }
                    else
                    {
                        if (hocSinhMap.TRANG_THAI == null || hocSinhMap.TRANG_THAI == false)
                        {
                            hocSinhMap.TRANG_THAI = true;
                            res = mapPhuHuynhHocSinhBO.update(hocSinhMap);
                            if (res.Res)
                            {
                                lblThongBao.Text = "Đăng ký thành công!";
                                btn2.Visible = false;
                                lblThongBao.ForeColor = Color.Blue;
                                return;
                            }
                            else
                            {
                                lblThongBao.Text = "Có lỗi xảy ra! Vui lòng liên hệ với quản trị viên để được giúp đỡ.";
                                return;
                            }
                        }
                        else
                        {
                            lblThongBao.Text = "SĐT này đã đăng ký thành công!";
                            lblThongBao.ForeColor = Color.Blue;
                            return;
                        }
                    }
                }
                #endregion
                #region "Số đăng ký khác số nhận SMS hàng ngày"
                else if (!string.IsNullOrEmpty(nha_mang_nhan_tin) && sdt_map != sdt_nhan_tin)
                {
                    short counter = 0;
                    DateTime? ngay_gui_otp = hocSinh.NGAY_GUI_OTP;
                    string strNgayOTP = ngay_gui_otp != null ? Convert.ToDateTime(ngay_gui_otp).ToString("yyyyMMdd") : "";
                    if (strNgayOTP == DateTime.Now.ToString("yyyyMMdd"))
                        counter = hocSinh.OTP_COUNTER != null ? hocSinh.OTP_COUNTER.Value : (Int16)0;

                    if (counter < 3)
                    {
                        MAP_PH_HS hocSinhMap = mapPhuHuynhHocSinhBO.getHocSinhByMaAndPhone(sdt_map, hocSinh.MA);
                        string ma_xac_nhan = makeRandomPassword(6);
                        if (hocSinhMap == null)
                        {
                            hocSinhMap = new MAP_PH_HS();
                            hocSinhMap.SDT_MAP = sdt_map;
                            hocSinhMap.MA_HOC_SINH = hocSinh.MA;
                            hocSinhMap.TRANG_THAI = false;
                            hocSinhMap.IP_ADDRESS = ipAddress;
                            hocSinhMap.ZALO_USER_ID = HduserID.Value;
                            hocSinhMap.MA_BAO_MAT = ma_xac_nhan;
                            hocSinhMap.NGAY_GUI_MA = DateTime.Now;
                            res = mapPhuHuynhHocSinhBO.insert(hocSinhMap);
                            MAP_PH_HS resMap = (MAP_PH_HS)res.ResObject;
                            if (res.Res)
                            {
                                TIN_NHAN tinNhan = new TIN_NHAN();
                                string noi_dung_gui = "Mã xác nhận đăng ký tài khoản trên Zalo cho SĐT " + tbSDTMap.Text.Trim() + " là " + ma_xac_nhan;
                                tinNhan = addTinNhan(sdt_nhan_tin, nha_mang_nhan_tin, noi_dung_gui);

                                res = tinNhanBO.insertTinXacNhanZalo(tinNhan, null);
                                if (res.Res)
                                {
                                    //update số lần gửi mã OTP
                                    hocSinh.NGAY_GUI_OTP = DateTime.Now;
                                    hocSinh.OTP_COUNTER = (Int16)(counter + 1);
                                    hocSinhBO.update(hocSinh, null);
                                }
                                Response.Redirect("DangKyZaloMobileConfirm.aspx?id=" + resMap.ID + "&sdt_map=" + sdt_map + "&ma_hs=" + hocSinh.MA);
                            }
                            else
                            {
                                lblThongBao.Text = "Có lỗi xảy ra! Vui lòng liên hệ với quản trị viên để được giúp đỡ.";
                                return;
                            }
                        }
                        else
                        {
                            if (hocSinhMap.TRANG_THAI == null || hocSinhMap.TRANG_THAI == false)
                            {
                                DateTime current_time = DateTime.Now;
                                DateTime gui_ma_time = DateTime.Now;
                                try
                                {
                                    gui_ma_time = Convert.ToDateTime(hocSinhMap.NGAY_GUI_MA);
                                }
                                catch { }
                                TimeSpan diff = current_time - gui_ma_time;
                                //Mã bảo mật có hiệu lực trong vòng 30p
                                if (diff.Minutes > 30)
                                {
                                    mapPhuHuynhHocSinhBO.updateMaXacNhan(hocSinhMap.ID, ma_xac_nhan);
                                    string noi_dung_gui = "Mã xác nhận đăng ký tài khoản trên Zalo cho SĐT " + sdt_map + " là " + ma_xac_nhan;
                                    TIN_NHAN tinNhan = addTinNhan(sdt_nhan_tin, loai_nha_mang, noi_dung_gui);
                                    res = tinNhanBO.insertTinXacNhanZalo(tinNhan, null);
                                    if (res.Res)
                                    {
                                        //update số lần gửi mã OTP
                                        hocSinh.NGAY_GUI_OTP = DateTime.Now;
                                        hocSinh.OTP_COUNTER = (Int16)(counter + 1);
                                        hocSinhBO.update(hocSinh, null);
                                    }
                                    Response.Redirect("DangKyZaloMobileConfirm.aspx?id=" + hocSinhMap.ID + "&sdt_map=" + sdt_map + "&ma_hs=" + hocSinh.MA + "&ip_address=" + ipAddress + "&userID_zalo=" + HduserID.Value);
                                }
                                else
                                {
                                    Response.Redirect("DangKyZaloMobileConfirm.aspx?id=" + hocSinhMap.ID + "&sdt_map=" + sdt_map + "&ma_hs=" + hocSinh.MA);
                                }
                            }
                            else
                            {
                                lblThongBao.Text = "SĐT này đã đăng ký thành công!";
                                lblThongBao.ForeColor = Color.Blue;
                                return;
                            }
                        }
                    }
                    else
                    {
                        lblThongBao.Text = "Bạn đã đăng ký vượt quá số lần truy cập/ngày. Vui lòng liên hệ với quản trị viên để được giúp đỡ!";
                        lblThongBao.ForeColor = Color.Red;
                        return;
                    }
                }
                #endregion
            }
            else
            {
                lblThongBao.Text = "có lỗi xảy ra. Vui lòng liên hệ với quản trị viên để được giúp đỡ.";
                return;
            }
        }
        protected void btnChec1_Click(object sender, EventArgs e)
        {
            int id_nam_hoc = Convert.ToInt16(DateTime.Now.Year);
            int thang = DateTime.Now.Month;
            if (thang >= 1 && thang < 9) id_nam_hoc = id_nam_hoc - 1;
            if (HduserID.Value != "" && HduserID.Value != "0")
            {
                string sdt_map = tbSDTMap.Text.Trim();
                if (sdt_map.IndexOf("+84") == 0)
                    sdt_map = "0" + sdt_map.Substring(1, sdt_map.Length - 1);
                string nha_mang_map = LocalAPI.getLoaiNhaMang(sdt_map);

                string sdt_nhan_tin = tbSDTNhanSMS.Text.Trim();
                if (sdt_nhan_tin.IndexOf("+84") == 0)
                    sdt_nhan_tin = "0" + sdt_nhan_tin.Substring(1, sdt_nhan_tin.Length - 1);
                string nha_mang_nhan_tin = LocalAPI.getLoaiNhaMang(sdt_nhan_tin);

                ZaloOaInfo zaloOaInfo = new ZaloOaInfo(2687558107438251665, "q5BKPirYTV1K4G24t9bS");
                ZaloOaClient oaClient = new ZaloOaClient(zaloOaInfo);
                string sdt_checkZalo = sdt_map;
                if (sdt_checkZalo.IndexOf("0") == 0) sdt_checkZalo = LocalAPI.Add84(sdt_checkZalo);
                JObject profile = oaClient.getProfile(Convert.ToInt64(sdt_checkZalo));
                string userID_check = (string)profile.SelectToken("data.userId");
                if (userID_check == HduserID.Value)
                {
                    if (tbSDTNhanSMS.Visible == true)
                    {
                        if (!string.IsNullOrEmpty(sdt_nhan_tin) && !string.IsNullOrEmpty(sdt_map))
                        {
                            sdt_nhan_tin = LocalAPI.Add84(sdt_nhan_tin);
                            //List<HOC_SINH> lstHocSinh = hocSinhBO.getHocSinhBySDTNhanSMS(sdt_map);
                            List<HOC_SINH> lstHocSinh = hocSinhBO.getHocSinhMapZalo(sdt_map, (Int16)id_nam_hoc);
                            int count = lstHocSinh.Count;
                            if (count == 0)
                            {
                                lblThongBao.Text = "Không có học sinh nào, vui lòng kiểm tra lại số điện thoại nhận tin nhắn SMS hàng ngày!";
                                hddHS1.Value = "";
                                hddHS2.Value = "";
                                return;
                            }
                            else
                            {
                                lblThongBao.Text = "";
                                if (count == 1)
                                {
                                    btn1.Visible = true;
                                    btn2.Visible = false;
                                }
                                else
                                {
                                    btn1.Visible = true;
                                    btn2.Visible = true;
                                }
                                for (int i = 0; i < count; i++)
                                {
                                    if (i == 0)
                                    {
                                        btn1.Text = lstHocSinh[i].HO_TEN;
                                        hddHS1.Value = lstHocSinh[i].ID.ToString();
                                    }
                                    else if (i == 1)
                                    {
                                        btn2.Text = lstHocSinh[i].HO_TEN;
                                        hddHS2.Value = lstHocSinh[i].ID.ToString();
                                    }
                                }
                                lblThongBao.Text = "Bấm chọn vào tên của con để xác nhận!";
                            }
                        }
                        else if (!string.IsNullOrEmpty(nha_mang_map) && string.IsNullOrEmpty(nha_mang_nhan_tin))
                        {
                            sdt_map = LocalAPI.Add84(sdt_map);
                            //List<HOC_SINH> lstHocSinh = hocSinhBO.getHocSinhBySDTNhanSMS(sdt_map);
                            List<HOC_SINH> lstHocSinh = hocSinhBO.getHocSinhMapZalo(sdt_map, (Int16)id_nam_hoc);
                            int count = lstHocSinh.Count;
                            if (count > 0)
                            {
                                tbSDTNhanSMS.Visible = false;
                                tbSDTNhanSMS.Text = "";
                                for (int i = 0; i < count; i++)
                                {
                                    if (i == 0)
                                    {
                                        btn1.Text = lstHocSinh[i].HO_TEN;
                                        hddHS1.Value = lstHocSinh[i].ID.ToString();
                                    }
                                    else if (i == 1)
                                    {
                                        btn2.Text = lstHocSinh[i].HO_TEN;
                                        hddHS2.Value = lstHocSinh[i].ID.ToString();
                                    }
                                }
                                if (count == 1)
                                {
                                    btn1.Visible = true;
                                    btn2.Visible = false;
                                }
                                else
                                {
                                    btn1.Visible = true;
                                    btn2.Visible = true;
                                }
                                lblThongBao.Text = "Bấm chọn vào tên của con để xác nhận!";
                            }
                            else
                            {
                                hddHS1.Value = "";
                                hddHS2.Value = "";
                                btn1.Visible = false;
                                btn2.Visible = false;
                                lblThongBao.Text = "Không tìm thấy học sinh nào. Vui lòng kiểm tra lại số điện thoại nhận tin nhắn SMS hàng ngày!";
                                tbSDTNhanSMS.Visible = true;
                                tbSDTNhanSMS.Text = "";
                                tbSDTNhanSMS.Focus();
                            }
                        }
                        else
                        {
                            lblThongBao.Text = "Số điện thoại nhận tin nhắn SMS hàng ngày chưa hợp lệ, vui lòng kiểm tra lại";
                            return;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(nha_mang_map))
                        {
                            sdt_map = LocalAPI.Add84(sdt_map);
                            //List<HOC_SINH> lstHocSinh = hocSinhBO.getHocSinhBySDTNhanSMS(sdt_map);
                            List<HOC_SINH> lstHocSinh = hocSinhBO.getHocSinhMapZalo(sdt_map, (Int16)id_nam_hoc);
                            int count = lstHocSinh.Count;
                            if (count > 0)
                            {
                                tbSDTNhanSMS.Visible = false;
                                tbSDTNhanSMS.Text = "";
                                for (int i = 0; i < count; i++)
                                {
                                    if (i == 0)
                                    {
                                        btn1.Text = lstHocSinh[i].HO_TEN;
                                        hddHS1.Value = lstHocSinh[i].ID.ToString();
                                    }
                                    else if (i == 1)
                                    {
                                        btn2.Text = lstHocSinh[i].HO_TEN;
                                        hddHS2.Value = lstHocSinh[i].ID.ToString();
                                    }
                                }
                                if (count == 1)
                                {
                                    btn1.Visible = true;
                                    btn2.Visible = false;
                                }
                                else
                                {
                                    btn1.Visible = true;
                                    btn2.Visible = true;
                                }
                                lblThongBao.Text = "Bấm chọn vào tên của con để xác nhận!";
                            }
                            else
                            {
                                hddHS1.Value = "";
                                hddHS2.Value = "";
                                btn1.Visible = false;
                                btn2.Visible = false;
                                lblThongBao.Text = "Không tìm thấy học sinh nào. Vui lòng kiểm tra lại số điện thoại đăng kí!";
                                tbSDTNhanSMS.Visible = true;
                                tbSDTNhanSMS.Text = "";
                                tbSDTNhanSMS.Focus();
                            }
                        }
                        else
                        {
                            lblThongBao.Text = "Số điện thoại đăng kí chưa hợp lệ, vui lòng kiểm tra lại";
                            hddHS1.Value = "";
                            hddHS2.Value = "";
                            btn1.Visible = false;
                            btn2.Visible = false;
                            return;
                        }
                    }
                }
                else
                {
                    lblThongBao.Text = "Số điện thoại đăng kí không đúng. Vui lòng nhập số điện thoại của bạn để đăng kí!";
                    hddHS1.Value = "";
                    hddHS2.Value = "";
                    btn1.Visible = false;
                    btn2.Visible = false;
                    return;
                }

            }
            else
            {
                lblThongBao.Text = "Có lỗi xảy ra. Vui lòng liên hệ với quản trị viên để được giúp đỡ!";
                return;
            }
        }
        protected void btnCheck_Click(object sender, EventArgs e)
        {
            int id_nam_hoc = Convert.ToInt16(DateTime.Now.Year);
            int thang = DateTime.Now.Month;
            if (thang >= 1 && thang < 9) id_nam_hoc = id_nam_hoc - 1;

            if (HduserID.Value != "" && HduserID.Value != "0")
            {
                string sdt_map = tbSDTMap.Text.Trim();
                sdt_map = LocalAPI.Add84(sdt_map);
                string nha_mang_map = LocalAPI.getLoaiNhaMang(sdt_map);

                if (string.IsNullOrEmpty(nha_mang_map))
                {
                    lblThongBao.Text += "Số điện thoại đăng ký chưa đúng. Vui lòng kiểm tra lại";
                    return;
                }

                string sdt_nhan_tin = tbSDTNhanSMS.Text.Trim();
                sdt_nhan_tin = LocalAPI.Add84(sdt_nhan_tin);
                string nha_mang_nhan_tin = LocalAPI.getLoaiNhaMang(sdt_nhan_tin);

                if (!string.IsNullOrEmpty(sdt_nhan_tin) && string.IsNullOrEmpty(nha_mang_nhan_tin))
                {
                    lblThongBao.Text += "Số điện thoại nhận SMS hàng ngày chưa đúng. Vui lòng kiểm tra lại";
                    return;
                }

                ZaloOaInfo zaloOaInfo = new ZaloOaInfo(2687558107438251665, "q5BKPirYTV1K4G24t9bS");
                ZaloOaClient oaClient = new ZaloOaClient(zaloOaInfo);
                JObject profile = oaClient.getProfile(Convert.ToInt64(sdt_map));
                string userID_check = (string)profile.SelectToken("data.userId");

                ghilog("logAction", DateTime.Now + ": Phone: " + tbSDTMap.Text + ", event=checkPhone, fromuid=" + HduserID.Value + ", userID_check=" + userID_check);
                if (userID_check == HduserID.Value)
                {
                    List<MAP_PH_HS> mapList = mapPhuHuynhHocSinhBO.getPhoneMapSuccess(sdt_map, userID_check);
                    string strMaHS = "";
                    if (mapList.Count > 0)
                    {
                        string strHS = "";
                        for (int i = 0; i < mapList.Count; i++)
                        {
                            string maHS = mapList[i].MA_HOC_SINH;
                            HOC_SINH hocSinh = hocSinhBO.getHocSinhByMaAndNamHoc(maHS, (Int16)id_nam_hoc);
                            if (hocSinh != null)
                            {
                                strHS += hocSinh.HO_TEN + ",";
                                strMaHS += "'" + maHS + "',";
                            }
                        }
                        strMaHS = strMaHS.TrimEnd(',');
                        strHS = strHS.TrimEnd(',');
                        lblThongBao.Text = "Bạn đã đăng ký thành công với con là " + strHS + ".";
                    }
                    else lblThongBao.Text = "";
                    
                    if (!string.IsNullOrEmpty(sdt_map) && string.IsNullOrEmpty(sdt_nhan_tin))
                    {
                        #region "Tìm những học sinh khác có đăng ký nhận SMS là SĐT map này
                        List<HocSinhEntity> lstHocSinh = hocSinhBO.getHocSinhByPhoneDangKyZalo(sdt_map, (Int16)id_nam_hoc, strMaHS);
                        if (lstHocSinh.Count > 0)
                        {
                            for (int i = 0; i < lstHocSinh.Count; i++)
                            {
                                if (i == 0)
                                {
                                    btn1.Text = lstHocSinh[i].HO_TEN;
                                    btn1.Visible = true;
                                    btn2.Visible = false;
                                    hddHS1.Value = lstHocSinh[i].ID.ToString();
                                }
                                else if (i == 1)
                                {
                                    btn2.Text = lstHocSinh[i].HO_TEN;
                                    btn2.Visible = true;
                                    hddHS2.Value = lstHocSinh[i].ID.ToString();
                                }
                            }
                            lblThongBao.Text += " Chọn học sinh để đăng ký.";
                        }
                        #endregion
                    }
                    else if (!string.IsNullOrEmpty(nha_mang_nhan_tin))
                    {
                        sdt_nhan_tin = LocalAPI.Add84(sdt_nhan_tin);
                        List<HocSinhEntity> lstHocSinh = hocSinhBO.getHocSinhByPhoneDangKyZalo(sdt_nhan_tin, (Int16)id_nam_hoc, strMaHS);
                        if (lstHocSinh.Count > 0)
                        {
                            for (int i = 0; i < lstHocSinh.Count; i++)
                            {
                                if (i == 0)
                                {
                                    btn1.Text = lstHocSinh[i].HO_TEN;
                                    btn1.Visible = true;
                                    btn2.Visible = false;
                                    hddHS1.Value = lstHocSinh[i].ID.ToString();
                                }
                                else if (i == 1)
                                {
                                    btn2.Text = lstHocSinh[i].HO_TEN;
                                    btn2.Visible = true;
                                    hddHS2.Value = lstHocSinh[i].ID.ToString();
                                }
                            }
                            lblThongBao.Text += " Chọn học sinh để đăng ký.";
                        }
                        else
                        {
                            lblThongBao.Text += " Không tìm thấy học sinh thỏa mãn. Vui lòng liên hệ với quản trị viên để được giúp đỡ!";
                        }
                    }
                }
                else
                {
                    lblThongBao.Text = "Số điện thoại đăng ký phải là số đăng nhập Zalo. Vui lòng kiểm tra lại!";
                    hddHS1.Value = "";
                    hddHS2.Value = "";
                    btn1.Visible = false;
                    btn2.Visible = false;
                    return;
                }

            }
            else
            {
                lblThongBao.Text = "Có lỗi xảy ra. Vui lòng liên hệ với quản trị viên để được giúp đỡ!";
                return;
            }
        }
    }
}