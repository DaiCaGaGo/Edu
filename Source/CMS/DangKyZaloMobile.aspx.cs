using CMS.XuLy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
                string str_uid = HduserID.Value;
                ghilog("getDAtafromCallback", "event=dang_ky, fromuid=" + str_uid);
            }
        }
        #region action
        protected void btDangKy_Click(object sender, EventArgs e)
        {
            //string sdt_map = tbSDTMap.Text.Trim();
            //string ma_hs = tbMaHS.Text.Trim();
            //#region check null
            //if (string.IsNullOrEmpty(sdt_map))
            //{
            //    lblThongBao.Text = "Bạn phải nhập SĐT!";
            //    return;
            //}
            //if (string.IsNullOrEmpty(ma_hs))
            //{
            //    lblThongBao.Text = "Bạn phải nhập mã học sinh!";
            //    return;
            //}
            //if (ma_hs.Length != 12 || ma_hs.Substring(0, 2) != "HS")
            //{
            //    lblThongBao.Text = "Mã học sinh gồm 12 ký tự và bắt đầu bằng chữ 'HS'!";
            //    return;
            //}
            //#endregion

            //#region check số lần map trong ngày (max=3)
            //if (sdt_map.IndexOf("+84") == 0)
            //{
            //    sdt_map = "0" + sdt_map.Substring(3, sdt_map.Length - 3);
            //}
            //else if (sdt_map.IndexOf("84") == 0)
            //{
            //    sdt_map = sdt_map.Substring(2, sdt_map.Length - 2);
            //}
            //string ipAddress = GetIPAddress();
            //string loai_nha_mang_sdt_map = LocalAPI.getLoaiNhaMang(sdt_map);
            //if (string.IsNullOrEmpty(loai_nha_mang_sdt_map))
            //{
            //    lblThongBao.Text = "Số điện thoại đăng ký chưa đúng, vui lòng kiểm tra lại!";
            //    return;
            //}
            //if (HduserID.Value == null || HduserID.Value == "")
            //{
            //    lblThongBao.Text = "Có lỗi xảy ra, bạn không thể đăng ký, vui lòng liên hệ với quản trị viên!";
            //    return;
            //}
            //else
            //{
            //    long? count = mapPhuHuynhHocSinhBO.getCountIPMapTrongNgay(HduserID.Value, DateTime.Now.ToString("dd/MM/yyyy"));
            //    if (count != null && count <= 3)
            //    {
            //        List<MAP_PH_HS> lstDetail = mapPhuHuynhHocSinhBO.getDuLieuMap(sdt_map, ma_hs);
            //        HOC_SINH hocSinh = hocSinhBO.getHocSinhByMa(ma_hs);
            //        if (hocSinh != null)
            //        {
            //            string sdt_nhan_tin = hocSinh.SDT_NHAN_TIN;
            //            string loai_nha_mang = LocalAPI.getLoaiNhaMang(sdt_nhan_tin);
            //            if (sdt_nhan_tin.IndexOf("84") == 0)
            //            {
            //                sdt_nhan_tin = "0" + sdt_nhan_tin.Substring(2, sdt_nhan_tin.Length - 2);
            //            }
            //            #region SĐT chưa map lần nào
            //            if (lstDetail.Count == 0)
            //            {
            //                if (sdt_nhan_tin == tbSDTMap.Text.Trim())
            //                {
            //                    ResultEntity res = mapPhuHuynhHocSinhBO.insertFirst(2, sdt_map, ma_hs, "", ipAddress, HduserID.Value);
            //                    if (res.Res)
            //                    {
            //                        lblThongBao.Text = "SĐT đã đăng ký thành công!";
            //                        lblThongBao.ForeColor = Color.Green;
            //                    }
            //                    else
            //                    {
            //                        lblThongBao.Text = "Có lỗi xảy ra!";
            //                        return;
            //                    }
            //                }
            //                else
            //                {
            //                    string ma_xac_nhan = makeRandomPassword(6);
            //                    //string ma_xac_nhan = "1234";
            //                    MAP_PH_HS detail = new MAP_PH_HS();
            //                    detail.SDT_MAP = sdt_map;
            //                    detail.MA_HOC_SINH = ma_hs;
            //                    detail.MA_BAO_MAT = ma_xac_nhan;
            //                    detail.TRANG_THAI = false;
            //                    detail.NGAY_GUI_MA = DateTime.Now;
            //                    detail.IP_ADDRESS = ipAddress;
            //                    detail.ZALO_USER_ID = HduserID.Value;
            //                    ResultEntity res = mapPhuHuynhHocSinhBO.insert(detail);
            //                    if (res.Res)
            //                    {
            //                        MAP_PH_HS resMap = (MAP_PH_HS)res.ResObject;
            //                        #region gửi tin nhắn xác nhận 
            //                        TIN_NHAN tinNhan = new TIN_NHAN();
            //                        string noi_dung_gui = "Mã xác nhận đăng ký tài khoản trên Zalo cho SĐT " + tbSDTMap.Text.Trim() + " là " + ma_xac_nhan;
            //                        tinNhan = addTinNhan(sdt_nhan_tin, loai_nha_mang, noi_dung_gui);
            //                        tinNhanBO.insertTinXacNhanZalo(tinNhan, null);
            //                        #endregion
            //                        Response.Redirect("DangKyZaloMobileConfirm.aspx?id=" + resMap.ID + "&sdt_map=" + sdt_map + "&ma_hs=" + ma_hs + "&zalo_id=" + HduserID.Value);
            //                    }
            //                    else
            //                    {
            //                        lblThongBao.Text = "SĐT chưa được đăng ký, vui lòng liên hệ với quản trị viên để được giúp đỡ!";
            //                        return;
            //                    }
            //                }
            //            }
            //            #endregion
            //            #region SĐT đã map
            //            else
            //            {
            //                MAP_PH_HS detail = lstDetail.Where(x => x.TRANG_THAI == true).FirstOrDefault();
            //                if (detail != null)
            //                {
            //                    lblThongBao.Text = "SĐT này đã được duyệt!";
            //                    lblThongBao.ForeColor = Color.Green;
            //                    return;
            //                }
            //                else
            //                {
            //                    #region check mã bảo mật còn hiệu lực ko, ko thì update mã bảo mật mới
            //                    DateTime current_time = DateTime.Now;
            //                    DateTime gui_ma_time = DateTime.Now;
            //                    try
            //                    {
            //                        gui_ma_time = Convert.ToDateTime(lstDetail[0].NGAY_GUI_MA);
            //                    }
            //                    catch { }
            //                    TimeSpan diff = current_time - gui_ma_time;
            //                    if (diff.Minutes > 30)
            //                    {
            //                        #region sinh mã xác nhận mới và gửi cho SĐT đăng ký SMS
            //                        string ma_xac_nhan = makeRandomPassword(6);
            //                        //string ma_xac_nhan = "1234";
            //                        mapPhuHuynhHocSinhBO.updateMaXacNhan(lstDetail[0].ID, ma_xac_nhan);
            //                        #region insert tin nhắn
            //                        ResultEntity res = new ResultEntity();
            //                        TIN_NHAN tinNhan = new TIN_NHAN();
            //                        string noi_dung_gui = "Mã xác nhận đăng ký tài khoản trên Zalo cho SĐT " + tbSDTMap.Text.Trim() + " là " + ma_xac_nhan;
            //                        tinNhan = addTinNhan(sdt_nhan_tin, loai_nha_mang, noi_dung_gui);
            //                        res = tinNhanBO.insertTinXacNhanZalo(tinNhan, null);
            //                        #endregion
            //                        Response.Redirect("DangKyZaloMobileConfirm.aspx?id=" + lstDetail[0].ID + "&sdt_map=" + sdt_map + "&ma_hs=" + ma_hs + "&ip_address=" + ipAddress + "&userID_zalo=" + HduserID.Value);
            //                        #endregion
            //                    }
            //                    else
            //                    {
            //                        //Sử dụng mã bảo mật cũ
            //                        Response.Redirect("DangKyZaloMobileConfirm.aspx?id=" + lstDetail[0].ID + "&sdt_map=" + sdt_map + "&ma_hs=" + ma_hs);
            //                    }
            //                    #endregion
            //                }
            //            }
            //            #endregion
            //        }
            //        else
            //        {
            //            MAP_PH_HS detail = new MAP_PH_HS();
            //            detail.SDT_MAP = sdt_map;
            //            detail.MA_HOC_SINH = ma_hs;
            //            detail.TRANG_THAI = false;
            //            detail.IP_ADDRESS = ipAddress;
            //            detail.ZALO_USER_ID = HduserID.Value;
            //            ResultEntity res = mapPhuHuynhHocSinhBO.insert(detail);
            //            lblThongBao.Text = "Mã học sinh này không tồn tại. Vui lòng kiểm tra lại!";
            //            return;
            //        }
            //    }
            //    else
            //    {
            //        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('success', 'Bạn đã dùng hết lượt truy cập /ngày!');", true);
            //        lblThongBao.Text = "Bạn đã dùng hết lượt truy cập /ngày!";
            //        return;
            //    }
            //}
            //#endregion
        }
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
            if (loai_nha_mang == "Viettel") cp = "VNPT";
            else if (loai_nha_mang == "GMobile") cp = "SOUTH";
            else if (loai_nha_mang == "MobiFone") cp = "NEO";
            else if (loai_nha_mang == "VinaPhone") cp = "MOBI_BANK";
            else if (loai_nha_mang == "VietnamMobile") cp = "SOUTH";
            TIN_NHAN tinNhan = new TIN_NHAN();
            tinNhan.ID_TRUONG = 1;//trường onedu
            tinNhan.LOAI_NGUOI_NHAN = 1;//PH học sinh
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
            string sdt_map = tbSDTMap.Text.Trim();
            if (sdt_map.IndexOf("+84") == 0)
            {
                sdt_map = "0" + sdt_map.Substring(3, sdt_map.Length - 3);
            }
            string loai_nha_mang = LocalAPI.getLoaiNhaMang(sdt_map);
            string ipAddress = GetIPAddress();
            if (HduserID.Value != "" && HduserID.Value != "0")
            {
                if (!string.IsNullOrEmpty(loai_nha_mang))
                {
                    if (hddHS1.Value != "")
                    {
                        //SĐT đăNG Ký TRùng Số NHận SMS Hàng NGàY
                        HOC_SINH hocSinh = hocSinhBO.getHocSinhByID(Convert.ToInt64(hddHS1.Value));
                        if (hocSinh != null)
                        {
                            if (string.IsNullOrEmpty(tbSDTNhanSMS.Text.Trim()))
                            {
                                List<MAP_PH_HS> lstDetail = mapPhuHuynhHocSinhBO.getDuLieuMap(sdt_map, hocSinh.MA);
                                if (lstDetail.Count == 0)
                                {
                                    MAP_PH_HS detail = new MAP_PH_HS();
                                    detail.SDT_MAP = sdt_map;
                                    detail.MA_HOC_SINH = hocSinh.MA;
                                    detail.TRANG_THAI = true;
                                    detail.IP_ADDRESS = ipAddress;
                                    detail.ZALO_USER_ID = HduserID.Value;
                                    ResultEntity res = mapPhuHuynhHocSinhBO.insert(detail);
                                    if (res.Res)
                                    {
                                        lblThongBao.Text = "Đăng ký thành công!";
                                        lblThongBao.ForeColor = Color.Blue;
                                    }
                                    else lblThongBao.Text = "error!";
                                }
                                else
                                {
                                    ResultEntity res = new ResultEntity();
                                    for (int i = 0; i < lstDetail.Count; i++)
                                    {
                                        if (sdt_map == hocSinh.SDT_NHAN_TIN || LocalAPI.Add84(sdt_map) == hocSinh.SDT_NHAN_TIN)
                                        {
                                            MAP_PH_HS detail = lstDetail[i];
                                            if (detail.TRANG_THAI == null || detail.TRANG_THAI == false)
                                            {
                                                res = mapPhuHuynhHocSinhBO.updateTrangThaiDuyet(detail.ID);
                                            }
                                        }
                                    }
                                    lblThongBao.Text = "Đăng ký thành công!";
                                    lblThongBao.ForeColor = Color.Blue;
                                }
                            }
                            else
                            {
                                //sdt đăng ký khác với số nhận sms hàng ngày
                                string sdt_nhan_tin = tbSDTNhanSMS.Text.Trim();
                                string loai_nha_mang_nhan_tin = LocalAPI.getLoaiNhaMang(sdt_nhan_tin);
                                if (!string.IsNullOrEmpty(loai_nha_mang_nhan_tin))
                                {
                                    List<MAP_PH_HS> lstDetail = mapPhuHuynhHocSinhBO.getDuLieuMap(sdt_map, hocSinh.MA);
                                    string ma_xac_nhan = makeRandomPassword(6);
                                    if (lstDetail.Count == 0)
                                    {
                                        MAP_PH_HS detail = new MAP_PH_HS();
                                        detail.SDT_MAP = sdt_map;
                                        detail.MA_HOC_SINH = hocSinh.MA;
                                        detail.TRANG_THAI = false;
                                        detail.IP_ADDRESS = ipAddress;
                                        detail.ZALO_USER_ID = HduserID.Value;
                                        detail.MA_BAO_MAT = ma_xac_nhan;
                                        detail.NGAY_GUI_MA = DateTime.Now;
                                        ResultEntity res = mapPhuHuynhHocSinhBO.insert(detail);
                                        if (res.Res)
                                        {
                                            MAP_PH_HS resMap = (MAP_PH_HS)res.ResObject;
                                            #region insert tin nhắn
                                            TIN_NHAN tinNhan = new TIN_NHAN();
                                            string noi_dung_gui = "Mã xác nhận đăng ký tài khoản trên Zalo cho SĐT " + tbSDTMap.Text.Trim() + " là " + ma_xac_nhan;
                                            tinNhan = addTinNhan(sdt_nhan_tin, loai_nha_mang_nhan_tin, noi_dung_gui);
                                            int? countCheck = tinNhanBO.checkTinNhanXacNhanDangKyZalo(sdt_nhan_tin, DateTime.Now.ToString("dd/MM/yyyy"));
                                            if (countCheck == null || (countCheck != null && countCheck < 3))
                                            {
                                                res = tinNhanBO.insertTinXacNhanZalo(tinNhan, null);
                                                Response.Redirect("DangKyZaloMobileConfirm.aspx?id=" + resMap.ID + "&sdt_map=" + sdt_map + "&ma_hs=" + hocSinh.MA);
                                            }
                                            else
                                            {
                                                lblThongBao.Text = "Bạn đã đăng ký vượt quá số lần truy cập/ngày!";
                                                lblThongBao.ForeColor = Color.Red;
                                                return;
                                            }
                                            #endregion
                                        }
                                        else lblThongBao.Text = "error!";
                                    }
                                    else
                                    {
                                        sdt_map = LocalAPI.Add84(sdt_map);
                                        ResultEntity res = new ResultEntity();

                                        int count_sucess = 0;
                                        for (int i = 0; i < lstDetail.Count; i++)
                                        {
                                            if (lstDetail[i].TRANG_THAI == true) count_sucess++;
                                        }

                                        if (count_sucess == 0)
                                        {
                                            DateTime current_time = DateTime.Now;
                                            DateTime gui_ma_time = DateTime.Now;
                                            try
                                            {
                                                gui_ma_time = Convert.ToDateTime(lstDetail[0].NGAY_GUI_MA);
                                            }
                                            catch { }
                                            TimeSpan diff = current_time - gui_ma_time;
                                            if (diff.Minutes > 30)
                                            {
                                                #region sinh mã xác nhận mới và gửi cho SĐT đăng ký SMS
                                                mapPhuHuynhHocSinhBO.updateMaXacNhan(lstDetail[0].ID, ma_xac_nhan);
                                                string noi_dung_gui = "Mã xác nhận đăng ký tài khoản trên Zalo cho SĐT " + tbSDTMap.Text.Trim() + " là " + ma_xac_nhan;
                                                TIN_NHAN tinNhan = addTinNhan(sdt_nhan_tin, loai_nha_mang, noi_dung_gui);
                                                int? countCheck = tinNhanBO.checkTinNhanXacNhanDangKyZalo(sdt_nhan_tin, DateTime.Now.ToString("dd/MM/yyyy"));
                                                if (countCheck == null || (countCheck != null && countCheck < 3))
                                                {
                                                    res = tinNhanBO.insertTinXacNhanZalo(tinNhan, null);
                                                    Response.Redirect("DangKyZaloMobileConfirm.aspx?id=" + lstDetail[0].ID + "&sdt_map=" + sdt_map + "&ma_hs=" + hocSinh.MA + "&ip_address=" + ipAddress + "&userID_zalo=" + HduserID.Value);
                                                }
                                                else
                                                {
                                                    lblThongBao.Text = "Bạn đã đăng ký vượt quá số lần truy cập/ngày!";
                                                    lblThongBao.ForeColor = Color.Red;
                                                    return;
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                //Sử dụng mã bảo mật cũ
                                                Response.Redirect("DangKyZaloMobileConfirm.aspx?id=" + lstDetail[0].ID + "&sdt_map=" + sdt_map + "&ma_hs=" + hocSinh.MA);
                                            }
                                        }
                                        else
                                        {
                                            lblThongBao.Text = "SĐT đã đăng ký thành công!";
                                            lblThongBao.ForeColor = Color.Blue;
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            lblThongBao.Text = "Có lỗi xảy ra!";
                            return;
                        }
                    }
                    else
                    {
                        //error
                    }
                }
                else
                {
                    lblThongBao.Text = "SĐT chưa đúng, vui lòng kiểm tra lại!";
                    return;
                }
            }
            else
            {
                lblThongBao.Text = "có lỗi xảy ra!";
                return;
            }
        }
        protected void btn2_Click(object sender, EventArgs e)
        {
            string sdt_map = tbSDTMap.Text.Trim();
            if (sdt_map.IndexOf("+84") == 0)
            {
                sdt_map = "0" + sdt_map.Substring(3, sdt_map.Length - 3);
            }
            string loai_nha_mang = LocalAPI.getLoaiNhaMang(sdt_map);
            string ipAddress = GetIPAddress();
            if (HduserID.Value != "" && HduserID.Value != "0")
            {
                if (!string.IsNullOrEmpty(loai_nha_mang))
                {
                    if (hddHS2.Value != "")
                    {
                        //SĐT đăNG Ký TRùng Số NHận SMS Hàng NGàY
                        HOC_SINH hocSinh = hocSinhBO.getHocSinhByID(Convert.ToInt64(hddHS2.Value));
                        if (hocSinh != null)
                        {
                            if (string.IsNullOrEmpty(tbSDTNhanSMS.Text.Trim()))
                            {
                                //sdt đăng ký trùng với số nhận sms hàng ngày
                                List<MAP_PH_HS> lstDetail = mapPhuHuynhHocSinhBO.getDuLieuMap(sdt_map, hocSinh.MA);
                                if (lstDetail.Count == 0)
                                {
                                    MAP_PH_HS detail = new MAP_PH_HS();
                                    detail.SDT_MAP = sdt_map;
                                    detail.MA_HOC_SINH = hocSinh.MA;
                                    detail.TRANG_THAI = true;
                                    detail.IP_ADDRESS = ipAddress;
                                    detail.ZALO_USER_ID = HduserID.Value;
                                    ResultEntity res = mapPhuHuynhHocSinhBO.insert(detail);
                                    if (res.Res)
                                    {
                                        lblThongBao.Text = "Đăng ký thành công!";
                                        lblThongBao.ForeColor = Color.Blue;
                                    }
                                    else lblThongBao.Text = "error!";
                                }
                                else
                                {
                                    //sdt_map = LocalAPI.Add84(sdt_map);
                                    ResultEntity res = new ResultEntity();
                                    for (int i = 0; i < lstDetail.Count; i++)
                                    {
                                        if (sdt_map == hocSinh.SDT_NHAN_TIN || LocalAPI.Add84(sdt_map) == hocSinh.SDT_NHAN_TIN)
                                        {
                                            MAP_PH_HS detail = lstDetail[i];
                                            if (detail.TRANG_THAI == null || detail.TRANG_THAI == false)
                                            {
                                                res = mapPhuHuynhHocSinhBO.updateTrangThaiDuyet(detail.ID);
                                            }
                                        }
                                    }
                                    lblThongBao.Text = "Đăng ký thành công!";
                                    lblThongBao.ForeColor = Color.Blue;
                                }
                            }
                            else
                            {
                                //sdt đăng ký khác với số nhận sms hàng ngày
                                string sdt_nhan_tin = tbSDTNhanSMS.Text.Trim();
                                string loai_nha_mang_nhan_tin = LocalAPI.getLoaiNhaMang(sdt_nhan_tin);
                                if (!string.IsNullOrEmpty(loai_nha_mang_nhan_tin))
                                {
                                    List<MAP_PH_HS> lstDetail = mapPhuHuynhHocSinhBO.getDuLieuMap(sdt_map, hocSinh.MA);
                                    string ma_xac_nhan = makeRandomPassword(6);
                                    if (lstDetail.Count == 0)
                                    {
                                        MAP_PH_HS detail = new MAP_PH_HS();
                                        detail.SDT_MAP = sdt_map;
                                        detail.MA_HOC_SINH = hocSinh.MA;
                                        detail.TRANG_THAI = false;
                                        detail.IP_ADDRESS = ipAddress;
                                        detail.ZALO_USER_ID = HduserID.Value;
                                        detail.MA_BAO_MAT = ma_xac_nhan;
                                        detail.NGAY_GUI_MA = DateTime.Now;
                                        ResultEntity res = mapPhuHuynhHocSinhBO.insert(detail);
                                        if (res.Res)
                                        {
                                            MAP_PH_HS resMap = (MAP_PH_HS)res.ResObject;
                                            #region insert tin nhắn
                                            string noi_dung_gui = "Mã xác nhận đăng ký tài khoản trên Zalo cho SĐT " + tbSDTMap.Text.Trim() + " là " + ma_xac_nhan;
                                            TIN_NHAN tinNhan = addTinNhan(sdt_nhan_tin, loai_nha_mang_nhan_tin, noi_dung_gui);
                                            int? countCheck = tinNhanBO.checkTinNhanXacNhanDangKyZalo(sdt_nhan_tin, DateTime.Now.ToString("dd/MM/yyyy"));
                                            if (countCheck == null || (countCheck != null && countCheck < 3))
                                            {
                                                res = tinNhanBO.insertTinXacNhanZalo(tinNhan, null);
                                                Response.Redirect("DangKyZaloMobileConfirm.aspx?id=" + resMap.ID + "&sdt_map=" + sdt_map + "&ma_hs=" + hocSinh.MA);
                                            }
                                            else
                                            {
                                                lblThongBao.Text = "Bạn đã đăng ký vượt quá số lần truy cập/ngày!";
                                                lblThongBao.ForeColor = Color.Red;
                                                return;
                                            }
                                            #endregion
                                        }
                                        else lblThongBao.Text = "error!";
                                    }
                                    else
                                    {
                                        sdt_map = LocalAPI.Add84(sdt_map);
                                        ResultEntity res = new ResultEntity();

                                        int count_sucess = 0;
                                        for (int i = 0; i < lstDetail.Count; i++)
                                        {
                                            if (lstDetail[i].TRANG_THAI == true) count_sucess++;
                                        }

                                        if (count_sucess == 0)
                                        {
                                            //Chưa có bản ghi nào thành công
                                            DateTime current_time = DateTime.Now;
                                            DateTime gui_ma_time = DateTime.Now;
                                            try
                                            {
                                                gui_ma_time = Convert.ToDateTime(lstDetail[0].NGAY_GUI_MA);
                                            }
                                            catch { }
                                            TimeSpan diff = current_time - gui_ma_time;
                                            if (diff.Minutes > 30)
                                            {
                                                #region sinh mã xác nhận mới và gửi cho SĐT đăng ký SMS
                                                mapPhuHuynhHocSinhBO.updateMaXacNhan(lstDetail[0].ID, ma_xac_nhan);
                                                TIN_NHAN tinNhan = new TIN_NHAN();
                                                string noi_dung_gui = "Mã xác nhận đăng ký tài khoản trên Zalo cho SĐT " + tbSDTMap.Text.Trim() + " là " + ma_xac_nhan;
                                                tinNhan = addTinNhan(sdt_nhan_tin, loai_nha_mang, noi_dung_gui);
                                                int? countCheck = tinNhanBO.checkTinNhanXacNhanDangKyZalo(sdt_nhan_tin, DateTime.Now.ToString("dd/MM/yyyy"));
                                                if (countCheck == null || (countCheck != null && countCheck < 3))
                                                {
                                                    res = tinNhanBO.insertTinXacNhanZalo(tinNhan, null);
                                                    Response.Redirect("DangKyZaloMobileConfirm.aspx?id=" + lstDetail[0].ID + "&sdt_map=" + sdt_map + "&ma_hs=" + hocSinh.MA + "&ip_address=" + ipAddress + "&userID_zalo=" + HduserID.Value);
                                                }
                                                else
                                                {
                                                    lblThongBao.Text = "Bạn đã đăng ký vượt quá số lần truy cập/ngày!";
                                                    lblThongBao.ForeColor = Color.Red;
                                                    return;
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                //Sử dụng mã bảo mật cũ
                                                Response.Redirect("DangKyZaloMobileConfirm.aspx?id=" + lstDetail[0].ID + "&sdt_map=" + sdt_map + "&ma_hs=" + hocSinh.MA);
                                            }
                                        }
                                        else
                                        {
                                            lblThongBao.Text = "SĐT đã đăng ký thành công!";
                                            lblThongBao.ForeColor = Color.Blue;
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            lblThongBao.Text = "Có lỗi xảy ra!";
                            return;
                        }
                    }
                    else
                    {
                        //error
                    }
                }
                else
                {
                    lblThongBao.Text = "SĐT chưa đúng, vui lòng kiểm tra lại!";
                    return;
                }
            }
            else
            {
                lblThongBao.Text = "có lỗi xảy ra!";
                return;
            }
        }
        protected void btnCheck_Click(object sender, EventArgs e)
        {
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
                            List<HOC_SINH> lstHocSinh = hocSinhBO.getHocSinhBySDTNhanSMS(sdt_nhan_tin);
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
                            List<HOC_SINH> lstHocSinh = hocSinhBO.getHocSinhBySDTNhanSMS(sdt_map);
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
                            List<HOC_SINH> lstHocSinh = hocSinhBO.getHocSinhBySDTNhanSMS(sdt_map);
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
    }
}