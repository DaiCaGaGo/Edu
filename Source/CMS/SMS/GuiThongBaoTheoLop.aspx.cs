using ClosedXML.Excel;
using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using OneEduDataAccess.Static;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CMS.SMS
{
    public partial class GuiThongBaoTheoLop : AuthenticatePage
    {
        private HocSinhBO hocSinhBO = new HocSinhBO();
        private LocalAPI localAPI = new LocalAPI();
        private QuyTinBO quyTinBO = new QuyTinBO();
        private TinNhanBO tinNhanBO = new TinNhanBO();
        TruongBO truongBO = new TruongBO();
        LopBO lopBO = new LopBO();
        LogUserBO logUserBO = new LogUserBO();
        BaiTapVeNhaBO btvnBO = new BaiTapVeNhaBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            btGuiTuyChon.Visible = is_access(SYS_Type_Access.SEND_SMS) && !(Sys_This_Truong.IS_ACTIVE_SMS != true);
            btnSmsTuyBien.Visible = is_access(SYS_Type_Access.SEND_SMS);
            if (!IsPostBack)
            {
                objKhoiHoc.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                rcbKhoi.DataBind();
                objLop.SelectParameters.Add("ma_cap_hoc", Sys_This_Cap_Hoc);
                objLop.SelectParameters.Add("idTruong", Sys_This_Truong.ID.ToString());
                objLop.SelectParameters.Add("maNamHoc", Sys_Ma_Nam_hoc.ToString());
                rcbLop.DataBind();
                viewQuyTin();
            }
        }
        protected void viewQuyTin()
        {
            btGuiTuyChon.Visible = is_access(SYS_Type_Access.SEND_SMS) && !(Sys_This_Truong.IS_ACTIVE_SMS != true);
            short nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc);
            short thang = Convert.ToInt16(DateTime.Now.Month);
            if (thang < 8) nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc + 1);
            if (Sys_This_Truong.ID_DOI_TAC == null || Sys_This_Truong.ID_DOI_TAC == 0)
            {
                QUY_TIN quyTinTheoNam = quyTinBO.getQuyTinByNamHoc(Convert.ToInt16(localAPI.GetYearNow()), Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID);
                bool is_insert_new_quytb = false;
                QUY_TIN quyTinTheoThang = quyTinBO.getQuyTin(nam_hoc, thang, Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID, out is_insert_new_quytb);
                if (quyTinTheoNam != null && quyTinTheoThang != null)
                {
                    double tong_con_thang = quyTinTheoThang.TONG_CON + (quyTinTheoThang.TONG_CAP + quyTinTheoThang.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                    / 100;
                    double tong_con_nam = quyTinTheoNam.TONG_CON + (quyTinTheoNam.TONG_CAP + quyTinTheoNam.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 1 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                        / 100;
                    if (Sys_This_Truong.IS_ACTIVE_SMS != true || tong_con_nam <= 0)
                    {
                        //btGuiTuyChon.Visible = false;
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", " notification('warning', 'Đơn vị đã sử dụng vượt quỹ tin được cấp. Vui lòng liên hệ lại với quản trị viên!');", true);
                        //return;
                    }
                    else if (Sys_This_Truong.IS_ACTIVE_SMS != true || tong_con_thang <= 0)
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", " notification('warning', 'Đơn vị đã sử dụng vượt quỹ tin tháng!');", true);
                    }
                    txtTongQuyTinConLaiTheoNam.Text = "Quỹ tin còn lại theo năm: " + ((quyTinTheoNam == null) ? "0" : quyTinTheoNam.TONG_CON.ToString());
                    //txtTongQuyTinConLaiTheoThang.Text = "Quỹ tháng còn: " + (quyTinTheoThang == null ? "0" : (quyTinTheoThang.TONG_CON > quyTinTheoNam.TONG_CON) ? quyTinTheoNam.TONG_CON.ToString() : quyTinTheoThang.TONG_CON.ToString());
                    txtTongQuyTinConLaiTheoThang.Text = "Quỹ tháng còn: " + (quyTinTheoThang == null ? "0" : quyTinTheoThang.TONG_CON.ToString());
                }
                else
                {
                    btGuiTuyChon.Visible = false;
                    btnSmsTuyBien.Visible = false;
                    txtTongQuyTinConLaiTheoThang.Text = "Đơn vị không được cấp quota";
                    txtTongQuyTinConLaiTheoNam.Visible = false;
                }
            }
            else
            {
                TRUONG detailTruong = new TRUONG();
                long tong_tin_con = 0;
                detailTruong = truongBO.getTruongById(Sys_This_Truong.ID);
                if (detailTruong != null)
                {
                    txtTongQuyTinConLaiTheoNam.Text = "Tổng tin cấp: " + (detailTruong.TONG_TIN_CAP == null ? "0" : detailTruong.TONG_TIN_CAP.ToString());
                    if (detailTruong.TONG_TIN_CAP != null)
                    {
                        tong_tin_con = detailTruong.TONG_TIN_DA_DUNG != null ? (detailTruong.TONG_TIN_CAP.Value - detailTruong.TONG_TIN_DA_DUNG.Value) : detailTruong.TONG_TIN_CAP.Value;
                    }
                    else tong_tin_con = detailTruong.TONG_TIN_DA_DUNG != null ? (-detailTruong.TONG_TIN_DA_DUNG.Value) : 0;
                    txtTongQuyTinConLaiTheoThang.Text = "Tổng tin còn: " + tong_tin_con;
                    if (Sys_This_Truong.IS_ACTIVE_SMS != true)
                    {
                        btGuiTuyChon.Visible = false;
                        btnSmsTuyBien.Visible = false;
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", " notification('warning', 'Đơn vị chưa đăng ký sử dụng SMS nên không thể gửi tin nhắn!');", true);
                    }
                }
                if (tong_tin_con <= 0)
                {
                    btGuiTuyChon.Visible = false;
                    btnSmsTuyBien.Visible = false;
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", " notification('warning', 'Đơn vị đã sử dụng hết quỹ tin được cấp. Vui lòng liên hệ lại với quản trị viên!');", true);
                }
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            List<HocSinhLopEntity> lstHS = new List<HocSinhLopEntity>();

            long? id_lop = localAPI.ConvertStringTolong(rcbLop.SelectedValue);
            if (id_lop != null)
                lstHS = hocSinhBO.getHocSinhGuiTinThongBao(Sys_This_Truong.ID, Sys_This_Cap_Hoc, Convert.ToInt16(Sys_Ma_Nam_hoc), Convert.ToInt16(rcbKhoi.SelectedValue), id_lop.Value, Convert.ToInt16(Sys_Hoc_Ky));
            RadGrid1.DataSource = lstHS;
            RadGrid1.MasterTableView.Columns.FindByUniqueName("SDT_BM").Display = is_access(SYS_Type_Access.VIEW_INFOR);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "countSMSDuTinhNew();SetGridHeight();", true);
        }
        protected void rcbKhoi_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbLop.ClearSelection();
            rcbLop.Text = String.Empty;
            rcbLop.DataBind();
            RadGrid1.Rebind();
        }
        protected void rcbLop_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void btGuiTuyChon_Click(object sender, EventArgs e)
        {
            #region "check quyen"
            if (!is_access(SYS_Type_Access.SEND_SMS))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            if (Sys_This_Truong.IS_ACTIVE_SMS != true)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị đang không đăng ký dịch vụ!');", true);
                return;
            }

            long? id_lop = localAPI.ConvertStringTolong(rcbLop.SelectedValue);
            if (id_lop == null)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Chọn lớp cần gửi tin.');", true);
                return;
            }

            DateTime? dt = null;
            bool is_checkHenGio = false;
            if (cbHenGioGuiTin.Checked)
            {
                is_checkHenGio = true;
                try
                {
                    dt = Convert.ToDateTime(tbTime.Text);
                }
                catch (Exception ex) { dt = null; }
                if (dt == null)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Thời gian không được để trống.');", true);
                    return;
                }
                else if (dt <= DateTime.Now)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Thời gian hẹn giờ không được nhỏ hơn thời gian hiện tại.');", true);
                    return;
                }
            }

            #endregion
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            List<TIN_NHAN> lstTinNhan = new List<TIN_NHAN>();
            int tong_tin_gui = 0;
            string brandname = "", cp = "";

            short nam_gui = Convert.ToInt16(DateTime.Now.Year);
            short thang_gui = Convert.ToInt16(DateTime.Now.Month);
            short tuan_gui = Convert.ToInt16(localAPI.getThisWeek().ToString());
            #region Lấy danh sách tin từ grid

            TIN_NHAN checkExists = new TIN_NHAN();
            TIN_NHAN checkSms = new TIN_NHAN();
            foreach (GridDataItem item in RadGrid1.SelectedItems)
            {
                bool is_gui_bo_me = false;
                if (item["IS_GUI_BO_ME"].Text == "1") is_gui_bo_me = true;
                string sdt = item["SDT_NHAN_TIN"].Text;
                string sdt_k = item["SDT_NHAN_TIN2"].Text;

                short? is_dk1 = localAPI.ConvertStringToShort(item["IS_DK_KY1"].Text);
                short? is_dk2 = localAPI.ConvertStringToShort(item["IS_DK_KY2"].Text);
                bool is_sms = false;
                if (is_dk1 != null && is_dk1 == 1 && Sys_Hoc_Ky == 1) is_sms = true;
                else if (is_dk2 != null && is_dk2 == 1 && Sys_Hoc_Ky == 2) is_sms = true;

                string loai_nha_mang = localAPI.getLoaiNhaMang(sdt);
                string loai_nha_mang_k = localAPI.getLoaiNhaMang(sdt_k);
                long id_hoc_sinh = Convert.ToInt64(item.GetDataKeyValue("ID").ToString());

                TextBox tbNoiDungTB = (TextBox)item.FindControl("tbNoiDungTB");
                string noi_dung = tbNoiDungTB.Text.Trim();
                string noi_dung_khong_dau = localAPI.chuyenTiengVietKhongDau(noi_dung);
                int so_tin = localAPI.demSoTin(noi_dung_khong_dau);

                checkExists = tinNhanBO.checkExistsSms(Sys_This_Truong.ID, nam_gui, thang_gui, null, sdt, (is_checkHenGio && dt != null) ? dt : null, Sys_Time_Send, noi_dung_khong_dau);
                checkSms = lstTinNhan.FirstOrDefault(x => x.SDT_NHAN == sdt && x.NOI_DUNG_KHONG_DAU == noi_dung_khong_dau);

                if (is_sms && !string.IsNullOrEmpty(noi_dung) && !string.IsNullOrEmpty(loai_nha_mang) && checkExists == null && checkSms == null)
                {
                    #region Tin 1
                    TIN_NHAN tinDetail = new TIN_NHAN();
                    tinDetail.ID_TRUONG = Sys_This_Truong.ID;
                    tinDetail.ID_NGUOI_NHAN = id_hoc_sinh;
                    tinDetail.LOAI_NGUOI_NHAN = 1;
                    tinDetail.SDT_NHAN = sdt;
                    tinDetail.MA_GOI_TIN = Sys_This_Truong.MA_GOI_TIN;
                    tinDetail.THOI_GIAN_GUI = (is_checkHenGio && dt != null) ? dt : DateTime.Now;
                    tinDetail.NGUOI_GUI = Sys_User.ID;
                    tinDetail.LOAI_TIN = SYS_Loai_Tin.Tin_Thong_Bao;
                    tinDetail.KIEU_GUI = 1;
                    tinDetail.NAM_GUI = nam_gui;
                    tinDetail.THANG_GUI = thang_gui;
                    tinDetail.TUAN_GUI = tuan_gui;
                    tinDetail.NOI_DUNG = noi_dung;
                    tinDetail.NOI_DUNG_KHONG_DAU = noi_dung_khong_dau;
                    tinDetail.SO_TIN = so_tin;
                    tinDetail.LOAI_NHA_MANG = loai_nha_mang;
                    brandname = ""; cp = "";
                    localAPI.getBrandnameAndCp(Sys_This_Truong, loai_nha_mang, out brandname, out cp);
                    tinDetail.BRAND_NAME = brandname;
                    tinDetail.CP = cp;
                    if (Sys_This_Truong.ID_DOI_TAC != null && Sys_This_Truong.ID_DOI_TAC > 0)
                        tinDetail.ID_DOI_TAC = Sys_This_Truong.ID_DOI_TAC;
                    else tinDetail.ID_DOI_TAC = null;
                    lstTinNhan.Add(tinDetail);
                    tong_tin_gui += so_tin;
                    #endregion

                    #region Tin 2
                    if (is_gui_bo_me && !string.IsNullOrEmpty(loai_nha_mang_k))
                    {
                        TIN_NHAN tinDetailK = new TIN_NHAN()
                        {
                            ID_DOI_TAC = tinDetail.ID_DOI_TAC,
                            ID_NGUOI_NHAN = tinDetail.ID_NGUOI_NHAN,
                            ID_NHAN_XET_HANG_NGAY = tinDetail.ID_NHAN_XET_HANG_NGAY,
                            ID_THONG_BAO = tinDetail.ID_THONG_BAO,
                            ID_TONG_HOP_NXHN = tinDetail.ID_TONG_HOP_NXHN,
                            ID_TRUONG = tinDetail.ID_TRUONG,
                            IS_DA_NHAN = tinDetail.IS_DA_NHAN,
                            IS_UNICODE = tinDetail.IS_UNICODE,
                            KIEU_GUI = tinDetail.KIEU_GUI,
                            LOAI_NGUOI_NHAN = tinDetail.LOAI_NGUOI_NHAN,
                            LOAI_NHA_MANG = tinDetail.LOAI_NHA_MANG,
                            MA_GOI_TIN = tinDetail.MA_GOI_TIN,
                            NOI_DUNG = tinDetail.NOI_DUNG,
                            NOI_DUNG_KHONG_DAU = tinDetail.NOI_DUNG_KHONG_DAU,
                            SO_TIN = tinDetail.SO_TIN,
                            THOI_GIAN_GUI = tinDetail.THOI_GIAN_GUI,
                            NGUOI_GUI = tinDetail.NGUOI_GUI,
                            LOAI_TIN = tinDetail.LOAI_TIN,
                            NAM_GUI = tinDetail.NAM_GUI,
                            SEND_NUMBER = tinDetail.SEND_NUMBER,
                            THANG_GUI = tinDetail.THANG_GUI,
                            TUAN_GUI = tinDetail.TUAN_GUI
                        };
                        tinDetailK.SDT_NHAN = sdt_k;
                        tinDetailK.LOAI_NHA_MANG = loai_nha_mang_k;
                        brandname = ""; cp = "";
                        localAPI.getBrandnameAndCp(Sys_This_Truong, loai_nha_mang_k, out brandname, out cp);
                        tinDetailK.BRAND_NAME = brandname;
                        tinDetailK.CP = cp;

                        lstTinNhan.Add(tinDetailK);
                        tong_tin_gui += so_tin;
                    }
                    #endregion
                }
            }
            #endregion

            string noi_dung_guiKem = tbNoiDung.Text.Trim();
            string noi_dung_guiKem_en = localAPI.chuyenTiengVietKhongDau(noi_dung_guiKem);

            if (!string.IsNullOrEmpty(noi_dung_guiKem_en) && lstTinNhan.Count > 0)
            {
                int so_tin = localAPI.demSoTin(noi_dung_guiKem_en);
                #region Gửi GVCN
                List<LopEntity> lstLopInTruong = lopBO.getLopGVCNByTruongCapNamHoc(Sys_This_Cap_Hoc, Sys_This_Truong.ID, Convert.ToInt16(Sys_Ma_Nam_hoc));
                if (cboGuiGVCN.Checked && lstLopInTruong != null && lstLopInTruong.Count > 0)
                {
                    LopEntity lop = lstLopInTruong.FirstOrDefault(x => x.ID_LOP == id_lop.Value);
                    if (lop != null)
                    {
                        string sdt_gv = lop.SDT_GVCN != null ? lop.SDT_GVCN : "";
                        string loaiNhaMang_gv = localAPI.getLoaiNhaMang(sdt_gv);

                        checkSms = tinNhanBO.checkExistsSms(Sys_This_Truong.ID, nam_gui, thang_gui, null, sdt_gv, (is_checkHenGio && dt != null) ? dt : null, Sys_Time_Send, noi_dung_guiKem_en);
                        checkExists = lstTinNhan.FirstOrDefault(x => x.SDT_NHAN == sdt_gv);

                        if (!string.IsNullOrEmpty(loaiNhaMang_gv) && checkSms == null && checkExists == null)
                        {
                            TIN_NHAN tinDetail = new TIN_NHAN();
                            tinDetail.ID_TRUONG = Sys_This_Truong.ID;
                            tinDetail.ID_NGUOI_NHAN = lop.ID_GVCN;
                            tinDetail.LOAI_NGUOI_NHAN = 2;
                            tinDetail.SDT_NHAN = sdt_gv;
                            tinDetail.MA_GOI_TIN = Sys_This_Truong.MA_GOI_TIN;
                            tinDetail.THOI_GIAN_GUI = (is_checkHenGio && dt != null) ? dt : DateTime.Now;
                            tinDetail.NGUOI_GUI = Sys_User.ID;
                            tinDetail.LOAI_TIN = SYS_Loai_Tin.Tin_Thong_Bao;
                            tinDetail.KIEU_GUI = 1;
                            tinDetail.NAM_GUI = nam_gui;
                            tinDetail.THANG_GUI = thang_gui;
                            tinDetail.TUAN_GUI = tuan_gui;
                            tinDetail.NOI_DUNG = noi_dung_guiKem;
                            tinDetail.NOI_DUNG_KHONG_DAU = noi_dung_guiKem_en;
                            tinDetail.SO_TIN = so_tin;

                            tinDetail.LOAI_NHA_MANG = loaiNhaMang_gv;
                            brandname = ""; cp = "";
                            localAPI.getBrandnameAndCp(Sys_This_Truong, loaiNhaMang_gv, out brandname, out cp);
                            tinDetail.BRAND_NAME = brandname;
                            tinDetail.CP = cp;
                            if (Sys_This_Truong.ID_DOI_TAC != null && Sys_This_Truong.ID_DOI_TAC > 0)
                                tinDetail.ID_DOI_TAC = Sys_This_Truong.ID_DOI_TAC;
                            else tinDetail.ID_DOI_TAC = null;
                            lstTinNhan.Add(tinDetail);
                            tong_tin_gui += so_tin;
                        }
                    }
                }
                #endregion
                #region Gửi kèm
                List<string> lstsdtGuiKem = new List<string>();
                if (!string.IsNullOrEmpty(tbListSDT.Text.Trim()))
                {
                    lstsdtGuiKem = tbListSDT.Text.Trim().Split(';').ToList();
                    foreach (var item in lstsdtGuiKem)
                    {
                        string sdtGuiKem = localAPI.Add84(item.Trim());
                        string loai_nha_mang_gui_kem = localAPI.getLoaiNhaMang(sdtGuiKem);

                        checkExists = lstTinNhan.FirstOrDefault(x => x.SDT_NHAN == sdtGuiKem);
                        checkSms = tinNhanBO.checkExistsSms(Sys_This_Truong.ID, nam_gui, thang_gui, null, sdtGuiKem, (is_checkHenGio && dt != null) ? dt : null, Sys_Time_Send, noi_dung_guiKem_en);

                        if (!string.IsNullOrEmpty(loai_nha_mang_gui_kem) && checkExists == null && checkSms == null)
                        {
                            TIN_NHAN tinDetail = new TIN_NHAN();
                            tinDetail.ID_TRUONG = Sys_This_Truong.ID;
                            tinDetail.LOAI_NGUOI_NHAN = 2;
                            tinDetail.SDT_NHAN = sdtGuiKem;
                            tinDetail.MA_GOI_TIN = Sys_This_Truong.MA_GOI_TIN;
                            tinDetail.THOI_GIAN_GUI = (is_checkHenGio && dt != null) ? dt : DateTime.Now;
                            tinDetail.NGUOI_GUI = Sys_User.ID;
                            tinDetail.LOAI_TIN = SYS_Loai_Tin.Tin_Thong_Bao;
                            tinDetail.KIEU_GUI = 1;
                            tinDetail.NAM_GUI = nam_gui;
                            tinDetail.THANG_GUI = thang_gui;
                            tinDetail.TUAN_GUI = tuan_gui;
                            tinDetail.NOI_DUNG = noi_dung_guiKem;
                            tinDetail.NOI_DUNG_KHONG_DAU = noi_dung_guiKem_en;
                            tinDetail.SO_TIN = so_tin;
                            tinDetail.LOAI_NHA_MANG = loai_nha_mang_gui_kem;
                            brandname = ""; cp = "";
                            localAPI.getBrandnameAndCp(Sys_This_Truong, loai_nha_mang_gui_kem, out brandname, out cp);
                            tinDetail.BRAND_NAME = brandname;
                            tinDetail.CP = cp;
                            if (Sys_This_Truong.ID_DOI_TAC != null && Sys_This_Truong.ID_DOI_TAC > 0)
                                tinDetail.ID_DOI_TAC = Sys_This_Truong.ID_DOI_TAC;
                            else tinDetail.ID_DOI_TAC = null;
                            lstTinNhan.Add(tinDetail);
                            tong_tin_gui += so_tin;
                        }
                    }
                }
                #endregion
            }

            #region save sms
            if (lstTinNhan.Count > 0)
            {
                short nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc);
                short thang = Convert.ToInt16(DateTime.Now.Month);
                if (thang < 8) nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc + 1);
                if (Sys_This_Truong.ID_DOI_TAC == null || Sys_This_Truong.ID_DOI_TAC == 0)
                {
                    #region Tính quỹ tin

                    QUY_TIN quyTinTheoNam = quyTinBO.getQuyTinByNamHoc(Convert.ToInt16(localAPI.GetYearNow()), Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID);
                    bool is_insert_new_quytb = false;
                    QUY_TIN quyTinTheoThang = quyTinBO.getQuyTin(nam_hoc, thang, Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID, out is_insert_new_quytb);

                    double tong_con_thang = quyTinTheoThang.TONG_CON + ((quyTinTheoThang.TONG_CAP + quyTinTheoThang.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                / 100);
                    double tong_con_nam = quyTinTheoNam.TONG_CON + ((quyTinTheoNam.TONG_CAP + quyTinTheoNam.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                        / 100);
                    #endregion
                    if (Sys_This_Truong.IS_SAN_QUY_TIN_NAM == true)
                    {
                        if (tong_tin_gui > tong_con_nam)
                        {
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị không còn đủ quota để gửi tin nhắn. Vui lòng liên hệ với quản trị viên!');", true);
                            return;
                        }
                        else
                        {
                            res = tinNhanBO.insertTinNhanThongBao(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, 0, Sys_User.ID);
                            if (res.Res)
                            {
                                logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi tin thông báo: gửi tùy chọn " + tong_tin_gui + " tin nhắn", Sys_User.ID, DateTime.Now);
                            }
                            if (tong_tin_gui >= tong_con_thang)
                            {
                                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Đơn vị đã sử dụng vượt quá quỹ tin tháng!');", true);
                            }
                        }

                    }
                    else
                    {
                        if (tong_tin_gui > tong_con_thang || tong_tin_gui > tong_con_nam)
                        {
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị đã sử dụng vượt quá quỹ tin tháng!');", true);
                            return;
                        }
                        else
                        {
                            res = tinNhanBO.insertTinNhanThongBao(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, 0, Sys_User.ID);
                            if (res.Res)
                            {
                                logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi tin thông báo: gửi tùy chọn " + tong_tin_gui + " tin nhắn", Sys_User.ID, DateTime.Now);
                            }
                        }
                    }
                }
                else
                {
                    TRUONG detailTruong = new TRUONG();
                    long tong_tin_con = 0;
                    detailTruong = truongBO.getTruongById(Sys_This_Truong.ID);
                    if (detailTruong != null)
                    {
                        txtTongQuyTinConLaiTheoNam.Text = "Tổng tin cấp: " + (detailTruong.TONG_TIN_CAP == null ? "0" : detailTruong.TONG_TIN_CAP.ToString());
                        if (detailTruong.TONG_TIN_CAP != null)
                        {
                            tong_tin_con = detailTruong.TONG_TIN_DA_DUNG != null ? (detailTruong.TONG_TIN_CAP.Value - detailTruong.TONG_TIN_DA_DUNG.Value) : detailTruong.TONG_TIN_CAP.Value;
                        }
                        else tong_tin_con = detailTruong.TONG_TIN_DA_DUNG != null ? (-detailTruong.TONG_TIN_DA_DUNG.Value) : 0;
                        if (Sys_This_Truong.IS_ACTIVE_SMS != null && Sys_This_Truong.IS_ACTIVE_SMS == true && tong_tin_con > tong_tin_gui)
                        {
                            res = tinNhanBO.insertTinNhanThongBao(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, 0, Sys_User.ID);
                            if (res.Res)
                            {
                                logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi tin thông báo: gửi tùy chọn " + tong_tin_gui + " tin nhắn", Sys_User.ID, DateTime.Now);
                            }
                        }
                    }
                }
            }
            #endregion

            #region lưu thông báo hiển thị dặn dò trong Zalo
            if (cboGuiZalo.Checked && lstTinNhan.Count > 0 && !string.IsNullOrEmpty(noi_dung_guiKem))
            {
                BAI_TAP_VE_NHA btvn = new BAI_TAP_VE_NHA();
                btvn = btvnBO.getBaiTapVeNhaByNgay(Sys_This_Truong.ID, (Int16)Sys_Ma_Nam_hoc, id_lop.Value, DateTime.Now.ToString("yyyyMMdd"));
                if (btvn == null)
                {
                    btvn = new BAI_TAP_VE_NHA();
                    btvn.ID_TRUONG = Sys_This_Truong.ID;
                    btvn.MA_CAP_HOC = Sys_This_Cap_Hoc;
                    btvn.ID_KHOI = Convert.ToInt16(rcbKhoi.SelectedValue);
                    btvn.ID_NAM_HOC = (Int16)Sys_Ma_Nam_hoc;
                    btvn.ID_LOP = Convert.ToInt64(rcbLop.SelectedValue);
                    btvn.NGAY_BTVN = DateTime.Now;
                    btvn.NOI_DUNG = noi_dung_guiKem;
                    btvnBO.insert(btvn, Sys_User.ID);
                }
                else
                {
                    btvn.NOI_DUNG = noi_dung_guiKem;
                    btvnBO.update(btvn, Sys_User.ID);
                }
            }
            #endregion

            #region Thông báo
            string strMsg = "";
            if (res.Res && tong_tin_gui > 0)
                strMsg = " notification('success', 'Có " + tong_tin_gui + " tin nhắn được gửi.');";
            else
                strMsg = " notification('warning', 'Không có tin nào được gửi đi!');";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            #endregion
            viewQuyTin();
            RadGrid1.Rebind();
        }
        protected void cbHenGioGuiTin_CheckedChanged(object sender, EventArgs e)
        {
            divTime.Visible = cbHenGioGuiTin.Checked;
        }
        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                TextBox tbNoiDungTB = (TextBox)item.FindControl("tbNoiDungTB");
                #region đỏ: không đk, xanh: Con gv, M: miễn, BM: đk cả bố mẹ
                short? is_bo_me = localAPI.ConvertStringToShort(item["IS_GUI_BO_ME"].Text);
                short? is_dk1 = localAPI.ConvertStringToShort(item["IS_DK_KY1"].Text);
                short? is_dk2 = localAPI.ConvertStringToShort(item["IS_DK_KY2"].Text);
                short? is_mien1 = localAPI.ConvertStringToShort(item["IS_MIEN_GIAM_KY1"].Text);
                short? is_mien2 = localAPI.ConvertStringToShort(item["IS_MIEN_GIAM_KY2"].Text);
                short? is_con_gv = localAPI.ConvertStringToShort(item["IS_CON_GV"].Text);
                if (is_con_gv != null && is_con_gv == 1) item.ForeColor = Color.Blue;

                if (Sys_Hoc_Ky == 1 && is_mien1 == 1) item["HO_TEN"].Text += " (*M)";
                else if (Sys_Hoc_Ky == 2 && is_mien2 == 1) item["HO_TEN"].Text += " (*M)";

                if (Sys_Hoc_Ky == 1 && (is_dk1 == null || is_dk1 == 0))
                {
                    item.ForeColor = Color.Red;
                    tbNoiDungTB.Enabled = false;
                }
                else if (Sys_Hoc_Ky == 2 && (is_dk2 == null || is_dk2 == 0))
                {
                    item.ForeColor = Color.Red;
                    tbNoiDungTB.Enabled = false;
                }

                //if (Sys_Hoc_Ky == 1 && is_dk1 != null && is_dk1 == 1 && is_bo_me != null && is_bo_me == 1)
                //    item["HO_TEN"].Text += " (*BM)";
                //if (Sys_Hoc_Ky == 2 && is_dk2 != null && is_dk2 == 1 && is_bo_me != null && is_bo_me == 1)
                //    item["HO_TEN"].Text += " (*BM)";
                #endregion
                #region "SDT"
                string sdt = item["SDT_NHAN_TIN"].Text;
                if (sdt == "&nbsp;") sdt = "";
                string sdt_k = item["SDT_NHAN_TIN2"].Text;
                if (sdt_k == "&nbsp;") sdt_k = "";
                if (is_bo_me == 1 && !string.IsNullOrEmpty(sdt_k))
                {
                    item["SDT_BM"].Text = sdt + "; " + sdt_k;
                }
                else
                {
                    item["SDT_BM"].Text = sdt;
                }
                #endregion
            }
        }
        public string SetHeightForRadgrid()
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "countSMSDuTinhNew();SetGridHeight();", true);
            return null;
        }
        protected void btnGuiAll_Click(object sender, EventArgs e)
        {
            #region "check quyen"
            if (!is_access(SYS_Type_Access.SEND_SMS))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            if (Sys_This_Truong.IS_ACTIVE_SMS != true)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị đang không đăng ký dịch vụ!');", true);
                return;
            }
            DateTime? dt = null;
            bool is_checkHenGio = false;
            if (cbHenGioGuiTin.Checked)
            {
                is_checkHenGio = true;
                try
                {
                    dt = Convert.ToDateTime(tbTime.Text);
                }
                catch (Exception ex) { dt = null; }
                if (dt == null)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Thời gian không được để trống.');", true);
                    return;
                }
                else if (dt <= DateTime.Now)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Thời gian hẹn giờ không được nhỏ hơn thời gian hiện tại.');", true);
                    return;
                }
            }

            string noi_dung = tbNoiDung.Text.Trim();
            if (string.IsNullOrEmpty(noi_dung))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn phải nhập nội dung tin nhắn thông báo chung!');", true);
                tbNoiDung.Focus();
                return;
            }
            #endregion

            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            List<TIN_NHAN> lstTinNhan = new List<TIN_NHAN>();
            int tong_tin_gui = 0;

            string noi_dung_khong_dau = localAPI.chuyenTiengVietKhongDau(noi_dung);

            #region Lấy danh sách tin từ data
            string brandname = "", cp = "";
            short nam_gui = Convert.ToInt16(DateTime.Now.Year);
            short thang_gui = Convert.ToInt16(DateTime.Now.Month);
            short tuan_gui = Convert.ToInt16(localAPI.getThisWeek().ToString());
            int so_tin = localAPI.demSoTin(noi_dung_khong_dau);

            TIN_NHAN checkExists = new TIN_NHAN();
            TIN_NHAN checkSms = new TIN_NHAN();
            foreach (GridDataItem item in RadGrid1.Items)
            {
                bool is_gui_bo_me = false;
                if (item["IS_GUI_BO_ME"].Text == "1") is_gui_bo_me = true;
                string sdt = item["SDT_NHAN_TIN"].Text;
                string sdt_k = item["SDT_NHAN_TIN2"].Text;
                long id_hoc_sinh = Convert.ToInt64(item.GetDataKeyValue("ID").ToString());

                short? is_dk1 = localAPI.ConvertStringToShort(item["IS_DK_KY1"].Text);
                short? is_dk2 = localAPI.ConvertStringToShort(item["IS_DK_KY2"].Text);
                bool is_sms = false;
                if (is_dk1 != null && is_dk1 == 1 && Sys_Hoc_Ky == 1) is_sms = true;
                else if (is_dk2 != null && is_dk2 == 1 && Sys_Hoc_Ky == 2) is_sms = true;

                string loai_nha_mang = localAPI.getLoaiNhaMang(sdt);

                checkExists = tinNhanBO.checkExistsSms(Sys_This_Truong.ID, nam_gui, thang_gui, null, sdt, (is_checkHenGio && dt != null) ? dt : null, Sys_Time_Send, noi_dung_khong_dau);
                checkSms = lstTinNhan.FirstOrDefault(x => x.SDT_NHAN == sdt && x.NOI_DUNG_KHONG_DAU == noi_dung_khong_dau);

                if (is_sms && !string.IsNullOrEmpty(loai_nha_mang) && checkExists == null && checkSms == null)
                {
                    #region Tạo tin 1

                    TIN_NHAN tinDetail = new TIN_NHAN();
                    tinDetail.ID_TRUONG = Sys_This_Truong.ID;
                    tinDetail.ID_NGUOI_NHAN = id_hoc_sinh;
                    tinDetail.LOAI_NGUOI_NHAN = 1;
                    tinDetail.SDT_NHAN = sdt;
                    tinDetail.MA_GOI_TIN = Sys_This_Truong.MA_GOI_TIN;
                    tinDetail.THOI_GIAN_GUI = (is_checkHenGio && dt != null) ? dt : DateTime.Now;
                    tinDetail.NGUOI_GUI = Sys_User.ID;
                    tinDetail.LOAI_TIN = SYS_Loai_Tin.Tin_Thong_Bao;
                    tinDetail.KIEU_GUI = 1;
                    tinDetail.NAM_GUI = nam_gui;
                    tinDetail.THANG_GUI = thang_gui;
                    tinDetail.TUAN_GUI = tuan_gui;
                    tinDetail.NOI_DUNG = noi_dung;
                    tinDetail.NOI_DUNG_KHONG_DAU = noi_dung_khong_dau;
                    tinDetail.SO_TIN = so_tin;
                    tinDetail.LOAI_NHA_MANG = loai_nha_mang;
                    brandname = ""; cp = "";
                    localAPI.getBrandnameAndCp(Sys_This_Truong, loai_nha_mang, out brandname, out cp);
                    tinDetail.BRAND_NAME = brandname;
                    tinDetail.CP = cp;
                    if (Sys_This_Truong.ID_DOI_TAC != null && Sys_This_Truong.ID_DOI_TAC > 0)
                        tinDetail.ID_DOI_TAC = Sys_This_Truong.ID_DOI_TAC;
                    else tinDetail.ID_DOI_TAC = null;
                    lstTinNhan.Add(tinDetail);
                    tong_tin_gui += so_tin;
                    #endregion

                    #region Tin 2
                    if (is_gui_bo_me)
                    {
                        string loai_nha_mang_k = localAPI.getLoaiNhaMang(sdt_k);
                        if (!string.IsNullOrEmpty(loai_nha_mang_k))
                        {
                            TIN_NHAN tinDetailK = new TIN_NHAN()
                            {
                                ID_DOI_TAC = tinDetail.ID_DOI_TAC,
                                ID_NGUOI_NHAN = tinDetail.ID_NGUOI_NHAN,
                                ID_NHAN_XET_HANG_NGAY = tinDetail.ID_NHAN_XET_HANG_NGAY,
                                ID_THONG_BAO = tinDetail.ID_THONG_BAO,
                                ID_TONG_HOP_NXHN = tinDetail.ID_TONG_HOP_NXHN,
                                ID_TRUONG = tinDetail.ID_TRUONG,
                                IS_DA_NHAN = tinDetail.IS_DA_NHAN,
                                IS_UNICODE = tinDetail.IS_UNICODE,
                                KIEU_GUI = tinDetail.KIEU_GUI,
                                LOAI_NGUOI_NHAN = tinDetail.LOAI_NGUOI_NHAN,
                                LOAI_NHA_MANG = tinDetail.LOAI_NHA_MANG,
                                MA_GOI_TIN = tinDetail.MA_GOI_TIN,
                                NOI_DUNG = tinDetail.NOI_DUNG,
                                NOI_DUNG_KHONG_DAU = tinDetail.NOI_DUNG_KHONG_DAU,
                                SO_TIN = tinDetail.SO_TIN,
                                THOI_GIAN_GUI = tinDetail.THOI_GIAN_GUI,
                                NGUOI_GUI = tinDetail.NGUOI_GUI,
                                LOAI_TIN = tinDetail.LOAI_TIN,
                                NAM_GUI = tinDetail.NAM_GUI,
                                SEND_NUMBER = tinDetail.SEND_NUMBER,
                                THANG_GUI = tinDetail.THANG_GUI,
                                TUAN_GUI = tinDetail.TUAN_GUI
                            };
                            tinDetailK.SDT_NHAN = sdt_k;
                            tinDetailK.LOAI_NHA_MANG = loai_nha_mang_k;
                            brandname = ""; cp = "";
                            localAPI.getBrandnameAndCp(Sys_This_Truong, loai_nha_mang_k, out brandname, out cp);
                            tinDetailK.BRAND_NAME = brandname;
                            tinDetailK.CP = cp;
                            lstTinNhan.Add(tinDetailK);
                            tong_tin_gui += so_tin;
                        }
                    }
                    #endregion
                }
            }
            #endregion
            #region Gửi GVCN
            long id_lop = Convert.ToInt64(rcbLop.SelectedValue);
            if (cboGuiGVCN.Checked && lstTinNhan.Count > 0)
            {
                List<LopEntity> lstLopInTruong = lopBO.getLopGVCNByTruongCapNamHoc(Sys_This_Cap_Hoc, Sys_This_Truong.ID, Convert.ToInt16(Sys_Ma_Nam_hoc));
                LopEntity lop = lstLopInTruong.FirstOrDefault(x => x.ID_LOP == id_lop);
                if (lop != null)
                {
                    string sdt_gv = lop.SDT_GVCN != null ? lop.SDT_GVCN : "";
                    string loaiNhaMang_gv = localAPI.getLoaiNhaMang(sdt_gv);
                    if (!string.IsNullOrEmpty(loaiNhaMang_gv))
                    {
                        checkExists = lstTinNhan.FirstOrDefault(x => x.SDT_NHAN == sdt_gv);
                        checkSms = tinNhanBO.checkExistsSms(Sys_This_Truong.ID, nam_gui, thang_gui, null, sdt_gv, (is_checkHenGio && dt != null) ? dt : null, Sys_Time_Send, noi_dung_khong_dau);

                        if (checkExists == null && checkSms == null)
                        {
                            TIN_NHAN tinDetail = new TIN_NHAN();
                            tinDetail.ID_TRUONG = Sys_This_Truong.ID;
                            tinDetail.ID_NGUOI_NHAN = lop.ID_GVCN;
                            tinDetail.LOAI_NGUOI_NHAN = 2;
                            tinDetail.SDT_NHAN = sdt_gv;
                            tinDetail.MA_GOI_TIN = Sys_This_Truong.MA_GOI_TIN;
                            tinDetail.THOI_GIAN_GUI = (is_checkHenGio && dt != null) ? dt : DateTime.Now;
                            tinDetail.NGUOI_GUI = Sys_User.ID;
                            tinDetail.LOAI_TIN = SYS_Loai_Tin.Tin_Thong_Bao;
                            tinDetail.KIEU_GUI = 1;
                            tinDetail.NAM_GUI = nam_gui;
                            tinDetail.THANG_GUI = thang_gui;
                            tinDetail.TUAN_GUI = tuan_gui;
                            tinDetail.NOI_DUNG = noi_dung;
                            tinDetail.NOI_DUNG_KHONG_DAU = noi_dung_khong_dau;
                            tinDetail.SO_TIN = so_tin;
                            tinDetail.LOAI_NHA_MANG = loaiNhaMang_gv;
                            brandname = ""; cp = "";
                            localAPI.getBrandnameAndCp(Sys_This_Truong, loaiNhaMang_gv, out brandname, out cp);
                            tinDetail.BRAND_NAME = brandname;
                            tinDetail.CP = cp;
                            if (Sys_This_Truong.ID_DOI_TAC != null && Sys_This_Truong.ID_DOI_TAC > 0)
                                tinDetail.ID_DOI_TAC = Sys_This_Truong.ID_DOI_TAC;
                            else tinDetail.ID_DOI_TAC = null;

                            lstTinNhan.Add(tinDetail);
                            tong_tin_gui += so_tin;
                        }
                    }
                }
            }
            #endregion
            #region Gửi kèm
            List<string> lstsdtGuiKem = new List<string>();
            string strPhoneDinhKem = tbListSDT.Text.Trim();
            if (!string.IsNullOrEmpty(strPhoneDinhKem) && lstTinNhan.Count > 0)
            {
                lstsdtGuiKem = strPhoneDinhKem.Split(';').ToList();
                foreach (var item in lstsdtGuiKem)
                {
                    string sdtGuiKem = item.Trim();
                    sdtGuiKem = localAPI.Add84(sdtGuiKem);
                    string loai_nha_mang_gui_kem = localAPI.getLoaiNhaMang(sdtGuiKem);
                    if (!string.IsNullOrEmpty(loai_nha_mang_gui_kem))
                    {
                        checkExists = lstTinNhan.FirstOrDefault(x => x.SDT_NHAN == sdtGuiKem);
                        checkSms = tinNhanBO.checkExistsSms(Sys_This_Truong.ID, nam_gui, thang_gui, null, sdtGuiKem, (is_checkHenGio && dt != null) ? dt : null, Sys_Time_Send, noi_dung_khong_dau);

                        if (checkExists == null && checkSms == null)
                        {
                            TIN_NHAN tinDetail = new TIN_NHAN();
                            tinDetail.ID_TRUONG = Sys_This_Truong.ID;
                            tinDetail.LOAI_NGUOI_NHAN = 2;
                            tinDetail.SDT_NHAN = sdtGuiKem;
                            tinDetail.MA_GOI_TIN = Sys_This_Truong.MA_GOI_TIN;
                            tinDetail.THOI_GIAN_GUI = (is_checkHenGio && dt != null) ? dt : DateTime.Now;
                            tinDetail.NGUOI_GUI = Sys_User.ID;
                            tinDetail.LOAI_TIN = SYS_Loai_Tin.Tin_Thong_Bao;
                            tinDetail.KIEU_GUI = 1;
                            tinDetail.NAM_GUI = nam_gui;
                            tinDetail.THANG_GUI = thang_gui;
                            tinDetail.TUAN_GUI = tuan_gui;
                            tinDetail.NOI_DUNG = noi_dung;
                            tinDetail.NOI_DUNG_KHONG_DAU = noi_dung_khong_dau;
                            tinDetail.SO_TIN = so_tin;
                            tinDetail.LOAI_NHA_MANG = loai_nha_mang_gui_kem;
                            brandname = ""; cp = "";
                            localAPI.getBrandnameAndCp(Sys_This_Truong, loai_nha_mang_gui_kem, out brandname, out cp);
                            tinDetail.BRAND_NAME = brandname;
                            tinDetail.CP = cp;
                            if (Sys_This_Truong.ID_DOI_TAC != null && Sys_This_Truong.ID_DOI_TAC > 0)
                                tinDetail.ID_DOI_TAC = Sys_This_Truong.ID_DOI_TAC;
                            else tinDetail.ID_DOI_TAC = null;
                            lstTinNhan.Add(tinDetail);
                            tong_tin_gui += so_tin;
                        }
                    }
                }
            }
            #endregion
            #region save sms
            if (lstTinNhan.Count > 0)
            {
                short nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc);
                short thang = Convert.ToInt16(DateTime.Now.Month);
                if (thang < 8) nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc + 1);
                if (Sys_This_Truong.ID_DOI_TAC == null || Sys_This_Truong.ID_DOI_TAC == 0)
                {
                    QUY_TIN quyTinTheoNam = quyTinBO.getQuyTinByNamHoc(Convert.ToInt16(localAPI.GetYearNow()), Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID);
                    bool is_insert_new_quytb = false;
                    QUY_TIN quyTinTheoThang = quyTinBO.getQuyTin(nam_hoc, thang, Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID, out is_insert_new_quytb);
                    if (quyTinTheoNam != null && quyTinTheoThang != null)
                    {
                        double tong_con_thang = quyTinTheoThang.TONG_CON + ((quyTinTheoThang.TONG_CAP + quyTinTheoThang.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                    / 100);
                        double tong_con_nam = quyTinTheoNam.TONG_CON + ((quyTinTheoNam.TONG_CAP + quyTinTheoNam.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                            / 100);

                        if (Sys_This_Truong.IS_SAN_QUY_TIN_NAM == true)
                        {
                            if (tong_tin_gui > tong_con_nam)
                            {
                                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị không còn đủ quota để gửi tin nhắn. Vui lòng liên hệ với quản trị viên!');", true);
                                return;
                            }
                            else
                            {
                                res = tinNhanBO.insertTinNhanThongBao(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, 0, Sys_User.ID);
                                if (res.Res)
                                {
                                    logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi tin thông báo: gửi toàn danh sách " + tong_tin_gui + " tin nhắn", Sys_User.ID, DateTime.Now);
                                }
                                if (tong_tin_gui >= tong_con_thang)
                                {
                                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Đơn vị đã sử dụng vượt quá quỹ tin tháng!');", true);
                                }
                            }

                        }
                        else
                        {
                            if (tong_tin_gui > tong_con_thang || tong_tin_gui > tong_con_nam)
                            {
                                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị đã sử dụng vượt quá quỹ tin tháng. Vui lòng liên hệ với quản trị viên để được giúp đỡ!');", true);
                                return;
                            }
                            else
                            {
                                res = tinNhanBO.insertTinNhanThongBao(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, 0, Sys_User.ID);
                                if (res.Res)
                                {
                                    logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi tin thông báo: gửi toàn danh sách " + tong_tin_gui + " tin nhắn", Sys_User.ID, DateTime.Now);
                                }
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị không được cấp quota!');", true);
                        return;
                    }
                }
                else
                {
                    TRUONG detailTruong = new TRUONG();
                    long tong_tin_con = 0;
                    detailTruong = truongBO.getTruongById(Sys_This_Truong.ID);
                    if (detailTruong != null)
                    {
                        if (detailTruong.TONG_TIN_CAP != null)
                        {
                            tong_tin_con = detailTruong.TONG_TIN_DA_DUNG != null ? (detailTruong.TONG_TIN_CAP.Value - detailTruong.TONG_TIN_DA_DUNG.Value) : detailTruong.TONG_TIN_CAP.Value;
                        }
                        else tong_tin_con = detailTruong.TONG_TIN_DA_DUNG != null ? (-detailTruong.TONG_TIN_DA_DUNG.Value) : 0;
                        if (Sys_This_Truong.IS_ACTIVE_SMS != null && Sys_This_Truong.IS_ACTIVE_SMS == true && tong_tin_con > tong_tin_gui)
                        {
                            res = tinNhanBO.insertTinNhanThongBao(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, Convert.ToInt16(localAPI.getThisWeek().ToString()), Sys_User.ID);
                            if (res.Res)
                            {
                                logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi tin thông báo: gửi toàn danh sách " + tong_tin_gui + " tin nhắn", Sys_User.ID, DateTime.Now);
                            }
                        }
                    }
                }
            }
            #endregion

            #region lưu thông báo hiển thị dặn dò trong Zalo
            if (cboGuiZalo.Checked && lstTinNhan.Count > 0 && !string.IsNullOrEmpty(noi_dung))
            {
                BAI_TAP_VE_NHA btvn = new BAI_TAP_VE_NHA();
                btvn = btvnBO.getBaiTapVeNhaByNgay(Sys_This_Truong.ID, (Int16)Sys_Ma_Nam_hoc, id_lop, DateTime.Now.ToString("yyyyMMdd"));
                if (btvn == null)
                {
                    btvn = new BAI_TAP_VE_NHA();
                    btvn.ID_TRUONG = Sys_This_Truong.ID;
                    btvn.MA_CAP_HOC = Sys_This_Cap_Hoc;
                    btvn.ID_KHOI = Convert.ToInt16(rcbKhoi.SelectedValue);
                    btvn.ID_NAM_HOC = (Int16)Sys_Ma_Nam_hoc;
                    btvn.ID_LOP = id_lop;
                    btvn.NGAY_BTVN = DateTime.Now;
                    btvn.NOI_DUNG = noi_dung;
                    btvnBO.insert(btvn, Sys_User.ID);
                }
                else
                {
                    btvn.NOI_DUNG = noi_dung;
                    btvnBO.update(btvn, Sys_User.ID);
                }
            }
            #endregion

            #region Thông báo
            string strMsg = "";

            if (res.Res && tong_tin_gui > 0)
                strMsg = " notification('success', 'Có " + tong_tin_gui + " tin nhắn được gửi.');";
            else
                strMsg = " notification('warning', 'Không có tin nào được gửi đi!');";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            #endregion
            viewQuyTin();
            RadGrid1.Rebind();
        }
        protected void btGuiSoDinhKem_Click(object sender, EventArgs e)
        {
            #region "check quyen"
            if (!is_access(SYS_Type_Access.SEND_SMS))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            if (Sys_This_Truong.IS_ACTIVE_SMS != true)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị đang không đăng ký dịch vụ!');", true);
                return;
            }
            DateTime? dt = null;
            bool is_checkHenGio = false;
            if (cbHenGioGuiTin.Checked)
            {
                is_checkHenGio = true;
                try
                {
                    dt = Convert.ToDateTime(tbTime.Text);
                }
                catch (Exception ex) { dt = null; }
                if (dt == null)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Thời gian không được để trống.');", true);
                    return;
                }
                else if (dt <= DateTime.Now)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Thời gian hẹn giờ không được nhỏ hơn thời gian hiện tại.');", true);
                    return;
                }
            }
            if (string.IsNullOrEmpty(tbListSDT.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn phải nhập SĐT nhận tin nhắn!');", true);
                tbListSDT.Focus();
                return;
            }

            string noiDungGui = tbNoiDung.Text.Trim();
            if (string.IsNullOrEmpty(noiDungGui))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn phải nhập nội dung tin nhắn thông báo chung!');", true);
                tbNoiDung.Focus();
                return;
            }
            #endregion
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            #region get List tin nhan
            List<TIN_NHAN> lstTinNhan = new List<TIN_NHAN>();
            List<string> lstsdtGuiKem = new List<string>();
            string noiDungGui_en = localAPI.chuyenTiengVietKhongDau(noiDungGui);
            int tong_tin_gui = 0;

            lstsdtGuiKem = tbListSDT.Text.Trim().Split(';').ToList();
            short nam_gui = Convert.ToInt16(DateTime.Now.Year);
            short thang_gui = Convert.ToInt16(DateTime.Now.Month);
            short tuan_gui = Convert.ToInt16(localAPI.getThisWeek().ToString());
            int so_tin = localAPI.demSoTin(noiDungGui_en);

            TIN_NHAN checkExists = new TIN_NHAN();
            TIN_NHAN checkSms = new TIN_NHAN();
            string brandname = "", cp = "";
            foreach (var item in lstsdtGuiKem)
            {
                string sdtGuiKem = item.Trim();
                sdtGuiKem = localAPI.Add84(sdtGuiKem);
                string loai_nha_mang_gui_kem = localAPI.getLoaiNhaMang(sdtGuiKem);
                if (!string.IsNullOrEmpty(localAPI.getLoaiNhaMang(sdtGuiKem)))
                {
                    checkExists = lstTinNhan.FirstOrDefault(x => x.SDT_NHAN == sdtGuiKem);
                    checkSms = tinNhanBO.checkExistsSms(Sys_This_Truong.ID, nam_gui, thang_gui, null, sdtGuiKem, (is_checkHenGio && dt != null) ? dt : null, Sys_Time_Send, noiDungGui_en);

                    if (checkExists == null)
                    {
                        TIN_NHAN tinDetail = new TIN_NHAN();
                        tinDetail.ID_TRUONG = Sys_This_Truong.ID;
                        tinDetail.LOAI_NGUOI_NHAN = 2;
                        tinDetail.SDT_NHAN = sdtGuiKem;
                        tinDetail.MA_GOI_TIN = Sys_This_Truong.MA_GOI_TIN;
                        tinDetail.THOI_GIAN_GUI = (is_checkHenGio && dt != null) ? dt : DateTime.Now;
                        tinDetail.NGUOI_GUI = Sys_User.ID;
                        tinDetail.LOAI_TIN = SYS_Loai_Tin.Tin_Thong_Bao;
                        tinDetail.KIEU_GUI = 1;
                        tinDetail.NAM_GUI = nam_gui;
                        tinDetail.THANG_GUI = thang_gui;
                        tinDetail.TUAN_GUI = tuan_gui;
                        tinDetail.NOI_DUNG = noiDungGui;
                        tinDetail.NOI_DUNG_KHONG_DAU = noiDungGui_en;
                        tinDetail.SO_TIN = so_tin;
                        tinDetail.LOAI_NHA_MANG = loai_nha_mang_gui_kem;
                        brandname = ""; cp = "";
                        localAPI.getBrandnameAndCp(Sys_This_Truong, loai_nha_mang_gui_kem, out brandname, out cp);
                        tinDetail.BRAND_NAME = brandname;
                        tinDetail.CP = cp;
                        if (Sys_This_Truong.ID_DOI_TAC != null && Sys_This_Truong.ID_DOI_TAC > 0)
                            tinDetail.ID_DOI_TAC = Sys_This_Truong.ID_DOI_TAC;
                        else tinDetail.ID_DOI_TAC = null;
                        lstTinNhan.Add(tinDetail);
                        tong_tin_gui += so_tin;
                    }
                }
            }
            #endregion
            #region check quỹ và gửi tin nhắn
            if (lstTinNhan.Count > 0)
            {
                short nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc);
                short thang = Convert.ToInt16(DateTime.Now.Month);
                if (thang < 8) nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc + 1);
                if (Sys_This_Truong.ID_DOI_TAC == null || Sys_This_Truong.ID_DOI_TAC == 0)
                {
                    #region Tính quỹ tin
                    QUY_TIN quyTinTheoNam = quyTinBO.getQuyTinByNamHoc(Convert.ToInt16(localAPI.GetYearNow()), Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID);
                    bool is_insert_new_quytb = false;
                    QUY_TIN quyTinTheoThang = quyTinBO.getQuyTin(nam_hoc, thang, Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID, out is_insert_new_quytb);

                    double tong_con_thang = quyTinTheoThang.TONG_CON + ((quyTinTheoThang.TONG_CAP + quyTinTheoThang.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                / 100);
                    double tong_con_nam = quyTinTheoNam.TONG_CON + ((quyTinTheoNam.TONG_CAP + quyTinTheoNam.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                        / 100);
                    #endregion
                    if (Sys_This_Truong.IS_SAN_QUY_TIN_NAM == true)
                    {
                        if (tong_tin_gui > tong_con_nam)
                        {
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị không còn đủ quota để gửi tin nhắn. Vui lòng liên hệ với quản trị viên!');", true);
                            return;
                        }
                        else
                        {
                            res = tinNhanBO.insertTinNhanThongBao(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, 0, Sys_User.ID);
                            if (res.Res)
                            {
                                logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi tin thông báo đính kèm SĐT", Sys_User.ID, DateTime.Now);
                            }
                            if (tong_tin_gui >= tong_con_thang)
                            {
                                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Đơn vị đã sử dụng vượt quá quỹ tin tháng!');", true);
                            }
                        }

                    }
                    else
                    {
                        if (tong_tin_gui > tong_con_thang || tong_tin_gui > tong_con_nam)
                        //if (tong_tin_gui > tong_con_thang)
                        {
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị đã sử dụng vượt quá quỹ tin tháng. Vui lòng liên hệ với quản trị viên để được giúp đỡ!');", true);
                            return;
                        }
                        else
                        {
                            res = tinNhanBO.insertTinNhanThongBao(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, 0, Sys_User.ID);
                            if (res.Res)
                            {
                                logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi tin thông báo đính kèm SĐT ", Sys_User.ID, DateTime.Now);
                            }
                        }
                    }
                }
                else
                {
                    TRUONG detailTruong = new TRUONG();
                    long tong_tin_con = 0;
                    detailTruong = truongBO.getTruongById(Sys_This_Truong.ID);
                    if (detailTruong != null)
                    {
                        if (detailTruong.TONG_TIN_CAP != null)
                        {
                            tong_tin_con = detailTruong.TONG_TIN_DA_DUNG != null ? (detailTruong.TONG_TIN_CAP.Value - detailTruong.TONG_TIN_DA_DUNG.Value) : detailTruong.TONG_TIN_CAP.Value;
                        }
                        else tong_tin_con = detailTruong.TONG_TIN_DA_DUNG != null ? (-detailTruong.TONG_TIN_DA_DUNG.Value) : 0;
                        if (Sys_This_Truong.IS_ACTIVE_SMS != null && Sys_This_Truong.IS_ACTIVE_SMS == true && tong_tin_con > tong_tin_gui)
                        {
                            res = tinNhanBO.insertTinNhanThongBao(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, 0, Sys_User.ID);
                            if (res.Res)
                            {
                                logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi tin thông báo đính kèm SĐT ", Sys_User.ID, DateTime.Now);
                            }
                        }
                    }
                }
            }
            #endregion
            #region Thông báo
            string strMsg = "";
            if (res.Res && tong_tin_gui > 0)
                strMsg = " notification('success', 'Có " + tong_tin_gui + " tin nhắn được gửi.');";
            else
                strMsg = " notification('warning', 'Không có tin nào được gửi đi!');";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            #endregion
            viewQuyTin();
        }
    }
}