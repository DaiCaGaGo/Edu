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
    public partial class GuiTinHangNgay : AuthenticatePage
    {
        TongHopNXHNBO thBO = new TongHopNXHNBO();
        LocalAPI localAPI = new LocalAPI();
        DiemChiTietBO diemBO = new DiemChiTietBO();
        LopBO lopBO = new LopBO();
        GiaoVienBO giaoVienBO = new GiaoVienBO();
        HocSinhBO hsBO = new HocSinhBO();
        QuyTinBO quyTinBO = new QuyTinBO();
        TruongBO truongBO = new TruongBO();
        LogUserBO logUserBO = new LogUserBO();
        TinNhanBO tinNhanBO = new TinNhanBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            if (Sys_User.IS_ROOT == null || (Sys_User.IS_ROOT != null && Sys_User.IS_ROOT == false)) btnGuiLai.Visible = false;
            btnTongHop.Visible = is_access(SYS_Type_Access.SEND_SMS);
            btnGui.Visible = (is_access(SYS_Type_Access.SEND_SMS) && (Sys_This_Truong.IS_ACTIVE_SMS != null && Sys_This_Truong.IS_ACTIVE_SMS == true));
            btExport.Visible = is_access(SYS_Type_Access.EXPORT);
            if (!IsPostBack)
            {
                objKhoiHoc.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                rcbKhoi.DataBind();
                objLop.SelectParameters.Add("ma_cap_hoc", Sys_This_Cap_Hoc);
                objLop.SelectParameters.Add("idTruong", Sys_This_Truong.ID.ToString());
                objLop.SelectParameters.Add("maNamHoc", Sys_Ma_Nam_hoc.ToString());
                rcbLop.DataBind();
                if (rcbLop.SelectedValue != "") getGVCNLop(Convert.ToInt64(rcbLop.SelectedValue));
                rdNgay.SelectedDate = DateTime.Now;
                Session["TongHop"] = null;
                lblTongTinSuDung.Text = "";
                viewQuyTinCon();
            }
        }
        protected void getGVCNLop(long id_lop)
        {
            LOP lop = lopBO.getLopById(id_lop);
            if (lop != null)
            {
                if (lop.ID_GVCN != null)
                {
                    long id_giao_vien = lop.ID_GVCN.Value;
                    hdID_GVCN.Value = lop.ID_GVCN.Value.ToString();
                    GIAO_VIEN giaoVien = giaoVienBO.getGiaoVienByID(id_giao_vien);
                    if (giaoVien != null)
                    {
                        lblSDT_GV.Text = "(" + giaoVien.SDT + ")";
                        //cboGuiGVCN.Enabled = true;
                        cboGuiGVCN.Checked = true;
                    }
                }
                else
                {
                    lblSDT_GV.Text = "";
                    hdID_GVCN.Value = "";
                    //cboGuiGVCN.Enabled = false;
                }
            }
        }
        protected void viewQuyTinCon()
        {
            btnGui.Visible = (is_access(SYS_Type_Access.SEND_SMS) && !(Sys_This_Truong.IS_ACTIVE_SMS != true));
            short nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc);
            short thang = Convert.ToInt16(DateTime.Now.Month);
            if (thang < 8) nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc + 1);
            if (Sys_This_Truong.ID_DOI_TAC == null || Sys_This_Truong.ID_DOI_TAC == 0)
            {
                QUY_TIN quyTinTheoNam = quyTinBO.getQuyTinByNamHoc(Convert.ToInt16(localAPI.GetYearNow()), Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Lien_Lac, Sys_User.ID);
                bool is_insert_new_quytb = false;
                QUY_TIN quyTinTheoThang = quyTinBO.getQuyTin(nam_hoc, thang, Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Lien_Lac, Sys_User.ID, out is_insert_new_quytb);
                if (quyTinTheoNam != null && quyTinTheoThang != null)
                {
                    double tong_con_thang = quyTinTheoThang.TONG_CON + (quyTinTheoThang.TONG_CAP + quyTinTheoThang.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                    / 100;
                    double tong_con_nam = quyTinTheoNam.TONG_CON + (quyTinTheoNam.TONG_CAP + quyTinTheoNam.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                        / 100;
                    if (Sys_This_Truong.IS_ACTIVE_SMS != true || tong_con_nam <= 0)
                    {
                        btnGui.Visible = false;
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", " notification('warning', 'Đơn vị đã sử dụng vượt quỹ tin được cấp. Vui lòng liên hệ lại với quản trị viên!');", true);
                        return;
                    }
                    else if (Sys_This_Truong.IS_ACTIVE_SMS != true || tong_con_thang <= 0)
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", " notification('warning', 'Đơn vị đã sử dụng vượt quỹ tin tháng!');", true);
                    }
                    lblTongTinConNam.Text = "Quỹ năm còn: " + (quyTinTheoNam == null ? "0" : quyTinTheoNam.TONG_CON.ToString());
                    lblTongTinConThang.Text = "Quỹ tháng còn: " + (quyTinTheoThang == null ? "0" : (quyTinTheoThang.TONG_CON > quyTinTheoNam.TONG_CON) ? quyTinTheoNam.TONG_CON.ToString() : quyTinTheoThang.TONG_CON.ToString());
                }
                else
                {
                    btnGui.Visible = false;
                    lblTongTinConNam.Text = "Đơn vị không được cấp quota";
                    lblTongTinConThang.Visible = false;
                }
            }
            else
            {
                TRUONG detailTruong = new TRUONG();
                long tong_tin_con = 0;
                detailTruong = truongBO.getTruongById(Sys_This_Truong.ID);
                if (detailTruong != null)
                {
                    lblTongTinConNam.Text = "Tổng tin cấp: " + (detailTruong.TONG_TIN_CAP == null ? "0" : detailTruong.TONG_TIN_CAP.ToString());
                    if (detailTruong.TONG_TIN_CAP != null)
                    {
                        tong_tin_con = detailTruong.TONG_TIN_DA_DUNG != null ? (detailTruong.TONG_TIN_CAP.Value - detailTruong.TONG_TIN_DA_DUNG.Value) : detailTruong.TONG_TIN_CAP.Value;
                    }
                    else tong_tin_con = detailTruong.TONG_TIN_DA_DUNG != null ? (-detailTruong.TONG_TIN_DA_DUNG.Value) : 0;
                    lblTongTinConThang.Text = "Tổng tin còn: " + tong_tin_con;
                    if (Sys_This_Truong.IS_ACTIVE_SMS != true)
                    {
                        btnGui.Visible = false;
                        btnTongHop.Visible = false;
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", " notification('warning', 'Đơn vị chưa đăng ký sử dụng SMS nên không thể gửi tin nhắn!');", true);
                    }
                }
                if (tong_tin_con <= 0)
                {
                    btnGui.Visible = false;
                    btnTongHop.Visible = false;
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", " notification('warning', 'Đơn vị đã sử dụng hết quỹ tin được cấp. Vui lòng liên hệ lại với quản trị viên!');", true);
                }
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = thBO.getTongHopNXHNByTruongKhoiLopNgay(Sys_This_Truong.ID, localAPI.ConvertStringToShort(rcbKhoi.SelectedValue), localAPI.ConvertStringTolong(rcbLop.SelectedValue), rdNgay.SelectedDate, Convert.ToInt16(Sys_Ma_Nam_hoc), Convert.ToInt16(Sys_Hoc_Ky));
            RadGrid1.MasterTableView.Columns.FindByUniqueName("SDT_BM").Display = is_access(SYS_Type_Access.VIEW_INFOR);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
        protected void rcbKhoi_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbLop.ClearSelection();
            rcbLop.Text = String.Empty;
            rcbLop.DataBind();
            if (rcbLop.SelectedValue != "") getGVCNLop(Convert.ToInt64(rcbLop.SelectedValue));
            RadGrid1.Rebind();
        }
        protected void rcbLop_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (rcbLop.SelectedValue != "") getGVCNLop(Convert.ToInt64(rcbLop.SelectedValue));
            RadGrid1.Rebind();
        }
        protected void rdNgay_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void btnTongHop_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.SUA) || !is_access(SYS_Type_Access.THEM))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            if (Convert.ToDateTime(rdNgay.SelectedDate).Date < DateTime.Now.Date || Convert.ToDateTime(rdNgay.SelectedDate).Date > DateTime.Now.Date)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không thể thực hiện thao tác này!');", true);
                return;
            }
            short? ma_khoi = localAPI.ConvertStringToShort(rcbKhoi.SelectedValue);
            long? id_lop = localAPI.ConvertStringTolong(rcbLop.SelectedValue);
            NhanXetHangNgayBO nxhnBO = new NhanXetHangNgayBO();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            int success = 0;
            List<DiemChiTietEntity> lstDieminLop = diemBO.getDiemChiTietByTruongKhoiLopHocSinhNgay(Convert.ToInt16(Sys_Ma_Nam_hoc), Sys_This_Truong.ID, localAPI.ConvertStringToShort(rcbKhoi.SelectedValue), localAPI.ConvertStringTolong(rcbLop.SelectedValue), Convert.ToInt16(Sys_Hoc_Ky), rdNgay.SelectedDate.Value, null);

            GridItemCollection lstGrid = new GridItemCollection();
            if (RadGrid1.SelectedItems != null && RadGrid1.SelectedItems.Count > 0) lstGrid = RadGrid1.SelectedItems;
            else lstGrid = RadGrid1.Items;

            List<NhanXetHangNgayEntity> lstNXHN = nxhnBO.getNXHNByTruongKhoiLopNgay(Sys_This_Truong.ID, localAPI.ConvertStringToShort(rcbKhoi.SelectedValue), localAPI.ConvertStringTolong(rcbLop.SelectedValue), DateTime.Now, Convert.ToInt16(Sys_Ma_Nam_hoc), Convert.ToInt16(Sys_Hoc_Ky));
            foreach (GridDataItem item in lstGrid)
            {
                long? id_nx_th = localAPI.ConvertStringTolong(item["ID"].Text);
                long id_hs = Convert.ToInt64(item.GetDataKeyValue("ID_HS").ToString());
                long? id_mon_truong = localAPI.ConvertStringTolong(item["ID_MON_HOC_TRUONG"].Text);
                TextBox tbNoiDung = (TextBox)item.FindControl("tbNoiDung");
                NhanXetHangNgayEntity detailNhanXet = lstNXHN.FirstOrDefault(x => x.ID_HS == id_hs);
                #region GetDiem
                string str_Diem = "";
                if (Sys_This_Cap_Hoc != "MN")
                {
                    List<DiemChiTietEntity> lstDiem = lstDieminLop.Where(x => x.ID_HOC_SINH == id_hs).ToList();
                    if (lstDiem.Count > 0)
                    {
                        for (int i = 0; i < lstDiem.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(lstDiem[i].DIEMMIENG) || !string.IsNullOrEmpty(lstDiem[i].DIEM15P) || !string.IsNullOrEmpty(lstDiem[i].DIEM1T_HS1) || !string.IsNullOrEmpty(lstDiem[i].DIEM1T_HS2) || !string.IsNullOrEmpty(lstDiem[i].DIEMHOCKY))
                            {
                                str_Diem += lstDiem[i].TEN_MON_TRUONG + "(";

                                string strDes = "";
                                string addThem = "";
                                strDes += string.IsNullOrEmpty(lstDiem[i].DIEMMIENG) ? "" : "M:" + lstDiem[i].DIEMMIENG.TrimEnd(',');

                                addThem = (string.IsNullOrEmpty(strDes) ? "" : ",");
                                strDes = strDes
                                        + (string.IsNullOrEmpty(lstDiem[i].DIEM15P) ? "" : addThem + "15P:" + lstDiem[i].DIEM15P.TrimEnd(','));

                                string str1T = (string.IsNullOrEmpty(lstDiem[i].DIEM1T_HS1) ? "" : lstDiem[i].DIEM1T_HS1.TrimEnd(','))
                                    + (!string.IsNullOrEmpty(lstDiem[i].DIEM1T_HS1) && !string.IsNullOrEmpty(lstDiem[i].DIEM1T_HS2) ? "," : "")
                                   + (string.IsNullOrEmpty(lstDiem[i].DIEM1T_HS2) ? "" : lstDiem[i].DIEM1T_HS2.TrimEnd(','));

                                addThem = (string.IsNullOrEmpty(strDes) ? "" : ",");
                                strDes = strDes
                                    + (string.IsNullOrEmpty(str1T) ? "" : addThem + "1T:" + str1T);
                                strDes += (string.IsNullOrEmpty(lstDiem[i].DIEMHOCKY) ? "" : "HK:" + lstDiem[i].DIEMHOCKY.TrimEnd(','));

                                str_Diem += strDes;
                                str_Diem += ") ";
                            }
                        }
                    }
                }
                #endregion
                #region Set content
                string tien_to = !string.IsNullOrEmpty(detailNhanXet.TIEN_TO) ? detailNhanXet.TIEN_TO : "";
                string noi_dung = "";

                if (detailNhanXet != null && !string.IsNullOrEmpty(detailNhanXet.NOI_DUNG_NX))
                {
                    if (Sys_This_Truong.ID != 209)
                        noi_dung = (string.IsNullOrEmpty(tien_to) ? "" : (tien_to + ": ")) +
                            (string.IsNullOrEmpty(str_Diem) ? "" : (str_Diem + " ")) +
                            detailNhanXet.NOI_DUNG_NX;
                    else noi_dung = string.IsNullOrEmpty(str_Diem) ? "" : (str_Diem + " " + detailNhanXet.NOI_DUNG_NX);
                }
                else
                {
                    if (Sys_This_Truong.ID != 209)
                    {
                        if (!string.IsNullOrEmpty(str_Diem))
                            noi_dung = (string.IsNullOrEmpty(tien_to) ? "" : tien_to + ": ") + str_Diem;
                    }
                    else noi_dung = !string.IsNullOrEmpty(str_Diem) ? str_Diem : "";
                }
                noi_dung = noi_dung.Trim();
                #endregion
                #region Save data
                tbNoiDung.Text = noi_dung;
                if (id_nx_th != null)
                {
                    TONG_HOP_NHAN_XET_HANG_NGAY detail = new TONG_HOP_NHAN_XET_HANG_NGAY();
                    detail = thBO.getTongHopNhanXetHangNgayByID(id_nx_th.Value);
                    if (detailNhanXet != null) detail.ID_NHAN_XET_HN = detailNhanXet.ID;
                    detail.ID_TRUONG = Sys_This_Truong.ID;
                    detail.ID_LOP = id_lop;
                    detail.NOI_DUNG_NX = tbNoiDung.Text.Trim();
                    detail.NGAY_TONG_HOP = rdNgay.SelectedDate.Value;
                    if (detail.IS_SEND != 1)
                    {
                        res = thBO.update(detail, Sys_User.ID);
                        if (res.Res) success++;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(noi_dung))
                    {
                        TONG_HOP_NHAN_XET_HANG_NGAY detail = new TONG_HOP_NHAN_XET_HANG_NGAY();
                        detail.ID_HOC_SINH = id_hs;
                        if (detailNhanXet != null) detail.ID_NHAN_XET_HN = detailNhanXet.ID;
                        detail.ID_TRUONG = Sys_This_Truong.ID;
                        detail.ID_LOP = id_lop;
                        detail.NOI_DUNG_NX = tbNoiDung.Text.Trim();
                        detail.NGAY_TONG_HOP = rdNgay.SelectedDate.Value;
                        res = thBO.insert(detail, Sys_User.ID);
                        if (res.Res) success++;
                    }
                }
                #endregion
            }
            string strMsg = "";
            if (success > 0)
            {
                strMsg += " notification('success', 'Có " + success + " bản ghi được lưu.');";
                logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Tổng hợp NXHN HS lớp " + id_lop, Sys_User.ID, DateTime.Now);
            }
            else strMsg = "notification('warning', 'Không có bản ghi nào được lưu.');";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
        }
        protected void btnGui_Click(object sender, EventArgs e)
        {
            #region Valid quyen
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
            if (Convert.ToDateTime(rdNgay.SelectedDate).Date < DateTime.Now.Date || Convert.ToDateTime(rdNgay.SelectedDate).Date > DateTime.Now.Date)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không thể thực hiện thao tác này!');", true);
                return;
            }
            if (cboGuiGVCN.Checked && string.IsNullOrEmpty(tbNoiDung.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn chưa nhập nội dung gửi GVCN!');", true);
                tbNoiDung.Focus();
                return;
            }
            if (RadGrid1.SelectedItems.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Chọn học sinh cần gửi tin');", true);
                return;
            }
            #endregion
            int count_sms = 0;
            int tong_tin_gui = 0;
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";

            string brandname = "", cp = "";
            short nam_gui = Convert.ToInt16(DateTime.Now.Year);
            short thang_gui = Convert.ToInt16(DateTime.Now.Month);
            short tuan_gui = Convert.ToInt16(localAPI.getThisWeek().ToString());

            List<TIN_NHAN> lstTinNhan = new List<TIN_NHAN>();
            TIN_NHAN checkExists = new TIN_NHAN();
            #region "gửi GVCN"
            if (cboGuiGVCN.Checked)
            {
                string sdt_gv = lblSDT_GV.Text.Trim();
                sdt_gv = sdt_gv.TrimStart('(').TrimEnd(')');
                string loai_nha_mang_gv = localAPI.getLoaiNhaMang(sdt_gv);

                string noi_dung_gv = tbNoiDung.Text.Trim();
                string noi_dung_gv_en = localAPI.chuyenTiengVietKhongDau(noi_dung_gv);
                int so_tin = 0;
                if (!string.IsNullOrEmpty(noi_dung_gv_en)) so_tin = localAPI.demSoTin(noi_dung_gv_en);

                checkExists = tinNhanBO.checkExistsSms(Sys_This_Truong.ID, nam_gui, thang_gui, null, sdt_gv, null, Sys_Time_Send, noi_dung_gv_en);

                long id_gvcn = 0;
                List<GIAO_VIEN> gvDetail = giaoVienBO.getGiaoVienByTruongSDT(Sys_This_Truong.ID, sdt_gv);
                if (gvDetail.Count > 0 && !string.IsNullOrEmpty(loai_nha_mang_gv) && so_tin > 0 && so_tin < 3 && checkExists == null)
                {
                    id_gvcn = gvDetail[0].ID;
                    TIN_NHAN tinDetail = new TIN_NHAN();
                    tinDetail.ID_TRUONG = Sys_This_Truong.ID;
                    tinDetail.ID_NGUOI_NHAN = id_gvcn;
                    tinDetail.LOAI_NGUOI_NHAN = 2;
                    tinDetail.SDT_NHAN = sdt_gv;
                    tinDetail.MA_GOI_TIN = Sys_This_Truong.MA_GOI_TIN;
                    tinDetail.THOI_GIAN_GUI = DateTime.Now;
                    tinDetail.NGUOI_GUI = Sys_User.ID;
                    tinDetail.LOAI_TIN = 1;
                    tinDetail.NAM_GUI = nam_gui;
                    tinDetail.THANG_GUI = thang_gui;
                    tinDetail.TUAN_GUI = tuan_gui;
                    tinDetail.NOI_DUNG = noi_dung_gv;
                    tinDetail.NOI_DUNG_KHONG_DAU = noi_dung_gv_en;
                    tinDetail.SO_TIN = so_tin;
                    tinDetail.NGAY_TAO = DateTime.Now;
                    tinDetail.NGUOI_TAO = Sys_User.ID;
                    tinDetail.LOAI_NHA_MANG = loai_nha_mang_gv;
                    brandname = ""; cp = "";
                    localAPI.getBrandnameAndCp(Sys_This_Truong, loai_nha_mang_gv, out brandname, out cp);
                    tinDetail.BRAND_NAME = brandname;
                    tinDetail.CP = cp;
                    if (Sys_This_Truong.ID_DOI_TAC != null && Sys_This_Truong.ID_DOI_TAC > 0)
                        tinDetail.ID_DOI_TAC = Sys_This_Truong.ID_DOI_TAC;
                    else tinDetail.ID_DOI_TAC = null;
                    lstTinNhan.Add(tinDetail);
                    tong_tin_gui += so_tin;
                }
            }
            #endregion
            #region "gửi tin học sinh"
            foreach (GridDataItem item in RadGrid1.SelectedItems)
            {
                #region get control
                long? id_nx_th = localAPI.ConvertStringTolong(item["ID"].Text);
                bool is_gui_bo_me = false;
                if (item["IS_GUI_BO_ME"].Text == "1") is_gui_bo_me = true;
                string sdt = item["SDT"].Text;
                string sdt_k = item["SDT_KHAC"].Text;
                long id_hoc_sinh = Convert.ToInt64(item.GetDataKeyValue("ID_HS").ToString());
                bool is_send = false;
                if (item["IS_SEND"].Text == "1") is_send = true;
                long? id_nx_hn = localAPI.ConvertStringTolong(item["ID_NHAN_XET_HN"].Text);
                TextBox tbNoiDung = (TextBox)item.FindControl("tbNoiDung");
                #region "check dang ky sms"
                short? is_dk1 = localAPI.ConvertStringToShort(item["IS_DK_KY1"].Text);
                short? is_dk2 = localAPI.ConvertStringToShort(item["IS_DK_KY2"].Text);
                bool is_sms = false;
                if (is_dk1 != null && is_dk1 == 1 && Sys_Hoc_Ky == 1) is_sms = true;
                else if (is_dk2 != null && is_dk2 == 1 && Sys_Hoc_Ky == 2) is_sms = true;
                #endregion
                #endregion
                #region get data

                string noi_dung = tbNoiDung.Text.Trim();
                string noi_dung_en = localAPI.chuyenTiengVietKhongDau(noi_dung);
                int so_tin = 0;
                if (!string.IsNullOrEmpty(noi_dung_en)) so_tin = localAPI.demSoTin(noi_dung_en);

                string loai_nha_mang = localAPI.getLoaiNhaMang(sdt);
                string loai_nha_mang_k = localAPI.getLoaiNhaMang(sdt_k);

                if (so_tin > 0 && so_tin < 3 && id_nx_th != null && is_send != true && is_sms && !string.IsNullOrEmpty(loai_nha_mang))
                {
                    #region Gui tin 1
                    TIN_NHAN tinDetail = new TIN_NHAN();
                    tinDetail.ID_TRUONG = Sys_This_Truong.ID;
                    tinDetail.ID_NGUOI_NHAN = id_hoc_sinh;
                    tinDetail.LOAI_NGUOI_NHAN = 1;
                    tinDetail.SDT_NHAN = sdt;
                    tinDetail.MA_GOI_TIN = Sys_This_Truong.MA_GOI_TIN;
                    tinDetail.THOI_GIAN_GUI = DateTime.Now;
                    tinDetail.NGUOI_GUI = Sys_User.ID;
                    tinDetail.ID_NHAN_XET_HANG_NGAY = id_nx_hn;
                    tinDetail.ID_TONG_HOP_NXHN = id_nx_th;
                    tinDetail.LOAI_TIN = 1;
                    tinDetail.NAM_GUI = nam_gui;
                    tinDetail.THANG_GUI = thang_gui;
                    tinDetail.TUAN_GUI = tuan_gui;
                    tinDetail.NOI_DUNG = noi_dung;
                    tinDetail.NOI_DUNG_KHONG_DAU = noi_dung_en;
                    tinDetail.SO_TIN = so_tin;
                    tinDetail.NGAY_TAO = DateTime.Now;
                    tinDetail.NGUOI_TAO = Sys_User.ID;
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

                    #region Gui tin 2
                    if (is_gui_bo_me && !string.IsNullOrEmpty(loai_nha_mang_k))
                    {
                        brandname = ""; cp = "";
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
                #endregion
            }
            #endregion
            #region save sms
            count_sms = lstTinNhan.Count;
            if (count_sms > 0)
            {
                #region "check quy tin nhan"
                short nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc);
                short thang = Convert.ToInt16(DateTime.Now.Month);
                if (thang < 8) nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc + 1);
                if (Sys_This_Truong.ID_DOI_TAC == null || Sys_This_Truong.ID_DOI_TAC == 0)
                {
                    QUY_TIN quyTinTheoNam = quyTinBO.getQuyTinByNamHoc(Convert.ToInt16(localAPI.GetYearNow()), Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Lien_Lac, Sys_User.ID);
                    bool is_insert_new_quytb = false;
                    QUY_TIN quyTinTheoThang = quyTinBO.getQuyTin(nam_hoc, thang, Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Lien_Lac, Sys_User.ID, out is_insert_new_quytb);
                    if (quyTinTheoNam != null && quyTinTheoThang != null)
                    {
                        double tong_con_thang = quyTinTheoThang.TONG_CON + ((quyTinTheoThang.TONG_CAP + quyTinTheoThang.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                    / 100);
                        double tong_con_nam = quyTinTheoNam.TONG_CON + ((quyTinTheoNam.TONG_CAP + quyTinTheoNam.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                            / 100);
                        if (Sys_This_Truong.IS_SAN_QUY_TIN_NAM != null && Sys_This_Truong.IS_SAN_QUY_TIN_NAM.Value)
                        {
                            if (tong_tin_gui > tong_con_nam)
                            {
                                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị không còn đủ quota để gửi tin nhắn. Vui lòng liên hệ với quản trị viên!');", true);
                                return;
                            }
                            else
                            {
                                res = tinNhanBO.insertTinNhanTongHopHangNgay(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, Sys_User.ID);
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
                                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị không còn đủ quota để gửi tin nhắn. Vui lòng liên hệ với quản trị viên!');", true);
                                return;
                            }
                            else
                            {
                                res = tinNhanBO.insertTinNhanTongHopHangNgay(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, Sys_User.ID);
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
                            res = tinNhanBO.insertTinNhanThongBao(lstTinNhan, Sys_This_Truong.ID, Convert.ToInt16(DateTime.Now.Year), Convert.ToInt16(DateTime.Now.Month), Convert.ToInt16(localAPI.getThisWeek().ToString()), Sys_User.ID);
                        }
                    }
                }
                #endregion
            }
            #endregion

            string strMsg = "";
            if (!res.Res && count_sms > 0)
            {
                strMsg = res.Msg;
            }
            else if (count_sms == 0)
            {
                strMsg = " notification('error', 'Không có tin nào được gửi đi.');";
            }
            else
            {
                strMsg = " notification('success', 'Có " + tong_tin_gui + " tin nhắn được gửi.');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);

            RadGrid1.Rebind();
            lblTongTinSuDung.Text = "Số tin vừa gửi: <b>" + tong_tin_gui + "</b>";
            viewQuyTinCon();
        }
        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                #region image trạng thái
                bool is_send = false;
                if (item["IS_SEND"].Text == "1") is_send = true;
                System.Web.UI.HtmlControls.HtmlImage image_chua_gui = (System.Web.UI.HtmlControls.HtmlImage)item.FindControl("chuaGui");
                System.Web.UI.HtmlControls.HtmlImage image_da_gui = (System.Web.UI.HtmlControls.HtmlImage)item.FindControl("daGui");
                if (is_send)
                {
                    image_chua_gui.Visible = false;
                    image_da_gui.Visible = true;
                }
                else
                {
                    image_chua_gui.Visible = true;
                    image_da_gui.Visible = false;
                }
                #endregion
                #region đỏ: không đk, xanh: Con gv, M: miễn, BM: đk cả bố mẹ
                short? is_bo_me = localAPI.ConvertStringToShort(item["IS_GUI_BO_ME"].Text);
                short? is_dk1 = localAPI.ConvertStringToShort(item["IS_DK_KY1"].Text);
                short? is_dk2 = localAPI.ConvertStringToShort(item["IS_DK_KY2"].Text);
                short? is_mien1 = localAPI.ConvertStringToShort(item["IS_MIEN_GIAM_KY1"].Text);
                short? is_mien2 = localAPI.ConvertStringToShort(item["IS_MIEN_GIAM_KY2"].Text);
                short? is_con_gv = localAPI.ConvertStringToShort(item["IS_CON_GV"].Text);
                if (is_con_gv != null && is_con_gv == 1) item.ForeColor = Color.Blue;
                if (Sys_Hoc_Ky == 1 && is_mien1 == 1) item["TEN_HS"].Text += " (*M)";
                else if (Sys_Hoc_Ky == 2 && is_mien2 == 1) item["TEN_HS"].Text += " (*M)";
                if (Sys_Hoc_Ky == 1 && (is_dk1 == null || is_dk1 == 0))
                {
                    item.ForeColor = Color.Red;
                }
                else if (Sys_Hoc_Ky == 2 && (is_dk2 == null || is_dk2 == 0))
                {
                    item.ForeColor = Color.Red;
                }
                if (Sys_Hoc_Ky == 1 && is_dk1 != null && is_dk1 == 1 && is_bo_me != null && is_bo_me == 1)
                    item["TEN_HS"].Text += " (*BM)";
                else if (Sys_Hoc_Ky == 2 && is_dk2 != null && is_dk2 == 1 && is_bo_me != null && is_bo_me == 1)
                    item["TEN_HS"].Text += " (*BM)";
                #endregion
                #region "SĐT"
                string sdt_k = item["SDT_KHAC"].Text;
                if (sdt_k == "&nbsp;") sdt_k = "";
                bool is_gui_bo_me = false;
                if (item["IS_GUI_BO_ME"].Text == "1") is_gui_bo_me = true;
                if (is_gui_bo_me && !string.IsNullOrEmpty(sdt_k))
                {
                    item["SDT_BM"].Text = item["SDT"].Text + "; " + sdt_k;
                }
                else item["SDT_BM"].Text = item["SDT"].Text;
                #endregion
                #region bôi đỏ số bản tin > 1
                TextBox tbNoiDung_nx = (TextBox)item.FindControl("tbNoiDung");
                string noi_dung_nx = tbNoiDung_nx.Text;
                int so_ky_tu = !string.IsNullOrEmpty(noi_dung_nx) ? noi_dung_nx.Length : 0;
                if (so_ky_tu > 160)
                {
                    item["CountLength"].ForeColor = Color.Red;
                }
                else item["CountLength"].ForeColor = Color.Black;
                #endregion
            }
        }
        protected void btnGuiLai_Click(object sender, EventArgs e)
        {
            #region check quyen
            if (Sys_User.IS_ROOT == null || (Sys_User.IS_ROOT != null && Sys_User.IS_ROOT == false))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện chức năng này.');", true);
                return;
            }
            if (Convert.ToDateTime(rdNgay.SelectedDate).Date < DateTime.Now.Date || Convert.ToDateTime(rdNgay.SelectedDate).Date > DateTime.Now.Date)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không thể thực hiện thao tác này!');", true);
                return;
            }
            #endregion
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            int success = 0;
            foreach (GridDataItem item in RadGrid1.SelectedItems)
            {
                long? id_nx = localAPI.ConvertStringTolong(item["ID"].Text);
                bool is_send = false;
                if (item["IS_SEND"].Text == "1") is_send = true;
                if (id_nx != null && is_send)
                {
                    res = thBO.updateTrangThaiGui(id_nx.Value, Sys_User.ID);
                    if (res.Res)
                    {
                        success++;
                        logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật trạng thái gửi tổng hợp NXHN " + id_nx, Sys_User.ID, DateTime.Now);
                    }
                }
            }
            string strMsg = "";
            if (success == 0)
            {
                strMsg = "notification('error', 'Không có bản ghi nào được cập nhật trạng thái.');";
            }
            else strMsg = "notification('success', 'Có " + success + " bản ghi được cập nhật trạng thái gửi.');";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
            lblTongTinSuDung.Text = "";
            viewQuyTinCon();
        }
        protected void btExport_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.EXPORT))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            DataTable dt = new DataTable();
            string serverPath = Server.MapPath("~");
            string path = String.Format("{0}\\ExportTemplates\\FileDynamic.xlsx", Server.MapPath("~"));
            string newName = "Tong_hop_tin_nhan_hang_ngay.xlsx";

            List<ExcelHeaderEntity> lstHeader = new List<ExcelHeaderEntity>();
            List<ExcelEntity> lstColumn = new List<ExcelEntity>();
            lstHeader.Add(new ExcelHeaderEntity { name = "STT", colM = 1, rowM = 1, width = 10 });
            foreach (GridColumn item in RadGrid1.MasterTableView.Columns)
            {
                #region add column
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MA_HS") && item.UniqueName == "MA_HS")
                {
                    DataColumn col = new DataColumn("MA_HS");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Mã HS", colM = 1, rowM = 1, width = 20 });
                    lstColumn.Add(new ExcelEntity { Name = "MA_HS", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TEN_HS") && item.UniqueName == "TEN_HS")
                {
                    DataColumn col = new DataColumn("TEN_HS");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tên học sinh", colM = 1, rowM = 1, width = 50 });
                    lstColumn.Add(new ExcelEntity { Name = "TEN_HS", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "SDT_BM") && item.UniqueName == "SDT_BM")
                {
                    DataColumn col = new DataColumn("SDT_BM");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "SĐT nhận tin", colM = 1, rowM = 1, width = 20 });
                    lstColumn.Add(new ExcelEntity { Name = "SDT_BM", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "NOI_DUNG_NX") && item.UniqueName == "NOI_DUNG_NX")
                {
                    DataColumn col = new DataColumn("NOI_DUNG_NX");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Nội dung nhận xét", colM = 1, rowM = 1, width = 100 });
                    lstColumn.Add(new ExcelEntity { Name = "NOI_DUNG_NX", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                #endregion
            }
            RadGrid1.AllowPaging = false;
            foreach (GridDataItem item in RadGrid1.Items)
            {
                DataRow row = dt.NewRow();
                TextBox tbNoiDung = (TextBox)item.FindControl("tbNoiDung");
                foreach (DataColumn col in dt.Columns)
                {
                    row[col.ColumnName] = item[col.ColumnName].Text.Replace("&nbsp;", " ");
                    if (col.ColumnName == "NOI_DUNG_NX") row[col.ColumnName] = tbNoiDung.Text.Trim(); ;
                }
                dt.Rows.Add(row);
            }
            int rowHeaderStart = 6;
            int rowStart = 7;
            string colStart = "A";
            string colEnd = localAPI.GetExcelColumnName(lstColumn.Count);
            List<ExcelHeaderEntity> listCell = new List<ExcelHeaderEntity>();
            string tenSo = "";
            string tenPhong = Sys_This_Truong.TEN;
            string tieuDe = "TIN NHẮN THÔNG BÁO CHUNG";
            string hocKyNamHoc = rcbLop.SelectedValue == "" ? "" : "Lớp " + rcbLop.Text;
            listCell.Add(new ExcelHeaderEntity { name = tenSo, colM = 3, rowM = 1, colName = "A", rowIndex = 1, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tenPhong, colM = 3, rowM = 1, colName = "A", rowIndex = 2, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tieuDe, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 3, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            listCell.Add(new ExcelHeaderEntity { name = hocKyNamHoc, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 4, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            string nameFileOutput = newName;
            localAPI.ExportExcelDynamic(serverPath, path, newName, nameFileOutput, 1, listCell, rowHeaderStart, rowStart, colStart, colEnd, dt, lstHeader, lstColumn, true);
        }
    }
}