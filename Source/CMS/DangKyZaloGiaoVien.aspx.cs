using CMS.XuLy;
using Newtonsoft.Json.Linq;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZaloCSharpSDK;

namespace CMS
{
    public partial class DangKyZaloGiaoVien : System.Web.UI.Page
    {
        public LocalAPI LocalAPI = new LocalAPI();
        MapZaloGiaoVienBO mapZaloGiaoVienBO = new MapZaloGiaoVienBO();
        GiaoVienBO giaoVienBO = new GiaoVienBO();
        TruongBO truongBO = new TruongBO();
        TinNhanBO tinNhanBO = new TinNhanBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //string str_uid = HduserID.Value;
                //ghilog("logGV", DateTime.Now + ":event=dang_ky, fromuid=" + str_uid);
            }
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
        protected void btnCheck_Click(object sender, EventArgs e)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";

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

                ZaloOaInfo zaloOaInfo = new ZaloOaInfo(3195276698126884179, "fWK4kNcFQaMsdWKDQ6BN");
                ZaloOaClient oaClient = new ZaloOaClient(zaloOaInfo);
                JObject profile = oaClient.getProfile(Convert.ToInt64(sdt_map));
                string userID_check = (string)profile.SelectToken("data.userId");
                ghilog("logGV", DateTime.Now + ":event=dang_ky, sdt_map=" + sdt_map + ", fromuid=" + HduserID.Value + ", userID_check=" + userID_check);
                if (userID_check == HduserID.Value)
                {
                    if ((!string.IsNullOrEmpty(sdt_map) && string.IsNullOrEmpty(sdt_nhan_tin)) || (sdt_map == sdt_nhan_tin)) // sdt_map
                    {
                        MAP_ZALO_GV checkExistsGV = mapZaloGiaoVienBO.getDataByPhone(sdt_map);
                        if (checkExistsGV == null)
                        {
                            List<GIAO_VIEN> giaoVien = giaoVienBO.getGiaoVienByPhone(sdt_map);
                            if (giaoVien.Count == 0)
                            {
                                lblThongBao.Text = "SĐT đăng ký không thỏa mãn. Kiểm tra lại SĐT đăng ký hoặc nhập SĐT nhận SMS hàng ngày để đăng ký!";
                                return;
                            }
                            else
                            {
                                string ipAddress = GetIPAddress();
                                MAP_ZALO_GV mapInsert = new MAP_ZALO_GV();
                                mapInsert.SDT_GV = sdt_map;
                                mapInsert.ZALO_USER_ID = userID_check;
                                mapInsert.IP_ADDRESS = ipAddress;
                                mapInsert.ID_VAI_TRO = 1;
                                mapInsert.SDT_NHAN_SMS = sdt_map;
                                mapInsert.TRANG_THAI = true;
                                res = mapZaloGiaoVienBO.insert(mapInsert);
                                if (res.Res)
                                {
                                    lblThongBao.Text = "SĐT đã được đăng ký. Mời quay lại để xem thông tin!";
                                    return;
                                }
                                else
                                {
                                    lblThongBao.Text = "Có lỗi xảy ra. Vui lòng liên hệ với quản trị viên để được giúp đỡ!";
                                    return;
                                }
                            }
                        }
                        else
                        {
                            lblThongBao.Text = "SĐT đã được đăng ký. Mời quay lại để xem thông tin!";
                            return;
                        }
                    }
                    else//sdt_nhan_sms
                    {
                        List<GIAO_VIEN> giaoVien = giaoVienBO.getGiaoVienByPhone(sdt_nhan_tin);
                        if (giaoVien.Count > 0)
                        {
                            btn1.Text = giaoVien[0].HO_TEN;
                            btn1.Visible = true;
                            hddGV.Value = giaoVien[0].ID.ToString();
                            lblThongBao.Text = "Mời chọn giáo viên cần đăng ký!";
                        }
                        else
                        {
                            lblThongBao.Text = "SĐT nhận SMS hàng ngày chưa đúng. Vui lòng kiểm tra lại!";
                            return;
                        }
                    }
                }
                else
                {
                    lblThongBao.Text = "Số điện thoại đăng ký phải là số đăng nhập Zalo. Vui lòng kiểm tra lại!";
                    return;
                }

            }
            else
            {
                lblThongBao.Text = "Có lỗi xảy ra. Vui lòng liên hệ với quản trị viên để được giúp đỡ!";
                return;
            }
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

            if (HduserID.Value != "" && HduserID.Value != "0" && !string.IsNullOrEmpty(hddGV.Value))
            {
                GIAO_VIEN giaoVien = giaoVienBO.getGiaoVienByID(Convert.ToInt64(hddGV.Value));
                if (giaoVien == null)
                {
                    lblThongBao.Text = "Có lỗi xảy ra. Vui lòng liên hệ với quản trị viên để được giúp đỡ!";
                    return;
                }

                if (!string.IsNullOrEmpty(nha_mang_nhan_tin) && sdt_map != sdt_nhan_tin)
                {
                    short counter = 0;
                    DateTime? ngay_gui_otp = giaoVien.NGAY_GUI_OTP;
                    string strNgayOTP = ngay_gui_otp != null ? Convert.ToDateTime(ngay_gui_otp).ToString("yyyyMMdd") : "";
                    if (strNgayOTP == DateTime.Now.ToString("yyyyMMdd"))
                        counter = giaoVien.OTP_COUNTER != null ? giaoVien.OTP_COUNTER.Value : (Int16)0;

                    if (counter < 3)
                    {
                        MAP_ZALO_GV gvMap = mapZaloGiaoVienBO.getGiaoVienByPhone(sdt_map, sdt_nhan_tin);
                        string ma_xac_nhan = makeRandomPassword(6);
                        if (gvMap == null)
                        {
                            gvMap = new MAP_ZALO_GV();
                            gvMap.SDT_GV = sdt_map;
                            gvMap.SDT_NHAN_SMS = giaoVien.SDT;
                            gvMap.TRANG_THAI = false;
                            gvMap.IP_ADDRESS = ipAddress;
                            gvMap.ZALO_USER_ID = HduserID.Value;
                            gvMap.MA_XAC_NHAN = ma_xac_nhan;
                            gvMap.NGAY_TRUY_CAP = DateTime.Now;
                            gvMap.ID_VAI_TRO = 1;
                            res = mapZaloGiaoVienBO.insert(gvMap);
                            MAP_ZALO_GV resMap = (MAP_ZALO_GV)res.ResObject;
                            if (res.Res)
                            {
                                TIN_NHAN tinNhan = new TIN_NHAN();
                                string noi_dung_gui = "Mã xác nhận đăng ký tài khoản trên Zalo cho SĐT " + sdt_map + " là " + ma_xac_nhan;
                                tinNhan = addTinNhan(sdt_nhan_tin, nha_mang_nhan_tin, noi_dung_gui);

                                res = tinNhanBO.insertTinXacNhanZalo(tinNhan, null);
                                if (res.Res)
                                {
                                    //update số lần gửi mã OTP
                                    giaoVien.NGAY_GUI_OTP = DateTime.Now;
                                    giaoVien.OTP_COUNTER = (Int16)(counter + 1);
                                    giaoVienBO.update(giaoVien, null);
                                }
                                Response.Redirect("DangKyZaloGiaoVienConfirm.aspx?id=" + resMap.ID + "&sdt_map=" + sdt_map + "&id_gv=" + giaoVien.ID);
                            }
                            else
                            {
                                lblThongBao.Text = "Có lỗi xảy ra! Vui lòng liên hệ với quản trị viên để được giúp đỡ.";
                                return;
                            }
                        }
                        else
                        {
                            if (gvMap.TRANG_THAI == null || gvMap.TRANG_THAI == false)
                            {
                                DateTime current_time = DateTime.Now;
                                DateTime gui_ma_time = DateTime.Now;
                                try
                                {
                                    gui_ma_time = Convert.ToDateTime(gvMap.NGAY_TRUY_CAP);
                                }
                                catch { }
                                TimeSpan diff = current_time - gui_ma_time;
                                //Mã bảo mật có hiệu lực trong vòng 30p
                                if (diff.Minutes > 30)
                                {
                                    mapZaloGiaoVienBO.updateMaXacNhan(gvMap.ID, ma_xac_nhan);
                                    string noi_dung_gui = "Mã xác nhận đăng ký tài khoản trên Zalo cho SĐT " + sdt_map + " là " + ma_xac_nhan;
                                    TIN_NHAN tinNhan = addTinNhan(sdt_nhan_tin, loai_nha_mang, noi_dung_gui);
                                    res = tinNhanBO.insertTinXacNhanZalo(tinNhan, null);
                                    if (res.Res)
                                    {
                                        //update số lần gửi mã OTP
                                        giaoVien.NGAY_GUI_OTP = DateTime.Now;
                                        giaoVien.OTP_COUNTER = (Int16)(counter + 1);
                                        giaoVienBO.update(giaoVien, null);
                                    }
                                    Response.Redirect("DangKyZaloGiaoVienConfirm.aspx?id=" + gvMap.ID + "&sdt_map=" + sdt_map + "&id_gv=" + giaoVien.ID + "&ip_address=" + ipAddress + "&userID_zalo=" + HduserID.Value);
                                }
                                else
                                {
                                    Response.Redirect("DangKyZaloGiaoVienConfirm.aspx?id=" + gvMap.ID + "&sdt_map=" + sdt_map + "&id_gv=" + giaoVien.ID);
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
            }
            else
            {
                lblThongBao.Text = "có lỗi xảy ra. Vui lòng liên hệ với quản trị viên để được giúp đỡ.";
                return;
            }
        }
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
    }
}