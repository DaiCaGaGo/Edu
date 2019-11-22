 using ClosedXML.Excel;
using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CMS.KetQuaHocTap
{
    public partial class NhapDiem : AuthenticatePage
    {
        private MonHocTruongBO monHocTruongBO = new MonHocTruongBO();
        private LopMonBO lopMonBO = new LopMonBO();
        private DiemChiTietBO diemChiTietBO = new DiemChiTietBO();
        private LocalAPI localAPI = new LocalAPI();
        private DataAccessAPI dataAccessAPI = new DataAccessAPI();
        private KhoaSoBO khoaSoBO = new KhoaSoBO();
        private bool checkKhoa = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            btSave.Visible = is_access(SYS_Type_Access.SUA);
            btExport.Visible = is_access(SYS_Type_Access.EXPORT);
            if (!IsPostBack)
            {
                objKhoiHoc.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                rcbKhoiHoc.DataBind();
                objLop.SelectParameters.Add("ma_cap_hoc", Sys_This_Cap_Hoc);
                objLop.SelectParameters.Add("idTruong", Sys_This_Truong.ID.ToString());
                objLop.SelectParameters.Add("maNamHoc", Sys_Ma_Nam_hoc.ToString());
                rcbLop.DataBind();
                objMonHoc.SelectParameters.Add("id_hocKy", Sys_Hoc_Ky.ToString());
                rcbMonHoc.DataBind();
                insertEmpty();
                checkKhoaSoTheoMon();
            }
        }
        public void checkKhoaSoTheoMon()
        {
            KHOA_SO_THEO_MON kstm = new KHOA_SO_THEO_MON();
            kstm = khoaSoBO.checkKhoaSoTheoMon(Sys_This_Truong.ID, localAPI.ConvertStringTolong(rcbLop.SelectedValue), localAPI.ConvertStringTolong(rcbMonHoc.SelectedValue), Sys_This_Cap_Hoc, Convert.ToInt16(Sys_Ma_Nam_hoc), Convert.ToInt16(Sys_Hoc_Ky), null);
            if (kstm != null && kstm.TRANG_THAI == true)
            {
                checkKhoa = true;
                btSave.Visible = false;
                lblMess.Text = "Môn học này đã khóa sổ";
            }
            else
            {
                btSave.Visible = true;
                lblMess.Text = "";
            }
        }
        public void insertEmpty()
        {
            long? id_lop = localAPI.ConvertStringTolong(rcbLop.SelectedValue);
            long? id_mon_hoc_truong = localAPI.ConvertStringTolong(rcbMonHoc.SelectedValue);
            short? id_mon_hoc = null;
            MON_HOC_TRUONG detail = new MON_HOC_TRUONG();
            if (id_mon_hoc_truong != null)
                detail = monHocTruongBO.getMonTruongByID(id_mon_hoc_truong.Value);
            if (detail != null) id_mon_hoc = detail.ID_MON_HOC;
            short? ma_khoi = localAPI.ConvertStringToShort(rcbKhoiHoc.SelectedValue);
            if (id_lop != null && id_mon_hoc_truong != null && ma_khoi != null)
                diemChiTietBO.insertEmpty(id_lop.Value, id_mon_hoc, Sys_This_Truong.ID, id_mon_hoc_truong.Value, Convert.ToInt16(Sys_Hoc_Ky), Convert.ToInt16(Sys_Ma_Nam_hoc), ma_khoi.Value);
            #region "cập nhật điểm TB từ kỳ 1 sang kỳ 2"
            if (Sys_Hoc_Ky == 2 && id_lop != null)
            {
                diemChiTietBO.updateDiemTBKy1SangKy2(Sys_This_Truong.ID, Convert.ToInt16(Sys_Ma_Nam_hoc), id_lop.Value);
            }
            #endregion
        }
        protected void rcbKhoiHoc_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbLop.ClearSelection();
            rcbLop.Text = string.Empty;
            rcbLop.DataBind();
            rcbMonHoc.ClearSelection();
            rcbMonHoc.Text = string.Empty;
            rcbMonHoc.DataBind();
            insertEmpty();
            checkKhoaSoTheoMon();
            RadGrid1.Rebind();
        }
        protected void rcbLop_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbMonHoc.ClearSelection();
            rcbMonHoc.Text = string.Empty;
            rcbMonHoc.DataBind();
            insertEmpty();
            checkKhoaSoTheoMon();
            RadGrid1.Rebind();
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            short? ma_khoi = localAPI.ConvertStringToShort(rcbKhoiHoc.SelectedValue);
            long? id_lop = localAPI.ConvertStringTolong(rcbLop.SelectedValue);
            long? id_mon_hoc_truong = localAPI.ConvertStringTolong(rcbMonHoc.SelectedValue);
            if (id_mon_hoc_truong == null) id_mon_hoc_truong = 0;
            MON_HOC_TRUONG detailMonHoc = new MON_HOC_TRUONG();
            detailMonHoc = monHocTruongBO.getMonTruongByID(id_mon_hoc_truong.Value);
            LOP_MON detailLopMon = new LOP_MON();
            if (id_lop != null && id_mon_hoc_truong != null)
                detailLopMon = lopMonBO.getLopMonByLopMonHocKy(id_lop.Value, id_mon_hoc_truong.Value, Convert.ToInt16(Sys_Hoc_Ky));
            RadGrid1.DataSource = diemChiTietBO.getDiemChiTietByTruongKhoiLopAndMon(Convert.ToInt16(Sys_Ma_Nam_hoc), Sys_This_Truong.ID, ma_khoi, id_lop, Convert.ToInt16(Sys_Hoc_Ky), id_mon_hoc_truong.Value);
            #region set kieu mon
            string cellClass = "Diem10";
            if ((detailMonHoc != null && (detailMonHoc.KIEU_MON == false || detailMonHoc.KIEU_MON == null)) || detailMonHoc == null)
                cellClass = "Diem10";
            else cellClass = "DiemCD";
            for (int i = 1; i <= 25; i++)
            {
                RadGrid1.MasterTableView.Columns.FindByUniqueName("DIEM" + i + "_VIEW").ItemStyle.CssClass = RadGrid1.MasterTableView.Columns.FindByUniqueName("DIEM" + i + "_VIEW").ItemStyle.CssClass.Replace("Diem10", "").Replace("DiemCD", "").Trim();
                RadGrid1.MasterTableView.Columns.FindByUniqueName("DIEM" + i + "_VIEW").ItemStyle.CssClass += " " + cellClass;
            }
            RadGrid1.MasterTableView.Columns.FindByUniqueName("DIEM_HOC_KY_VIEW").ItemStyle.CssClass = RadGrid1.MasterTableView.Columns.FindByUniqueName("DIEM_HOC_KY_VIEW").ItemStyle.CssClass.Replace("Diem10", "").Replace("DiemCD", "").Trim();
            RadGrid1.MasterTableView.Columns.FindByUniqueName("DIEM_HOC_KY_VIEW").ItemStyle.CssClass += " " + cellClass;
            RadGrid1.MasterTableView.Columns.FindByUniqueName("DIEM_TRUNG_BINH_KY1_VIEW").ItemStyle.CssClass = RadGrid1.MasterTableView.Columns.FindByUniqueName("DIEM_TRUNG_BINH_KY1_VIEW").ItemStyle.CssClass.Replace("Diem10", "").Replace("DiemCD", "").Trim();
            RadGrid1.MasterTableView.Columns.FindByUniqueName("DIEM_TRUNG_BINH_KY1_VIEW").ItemStyle.CssClass += " " + cellClass;
            RadGrid1.MasterTableView.Columns.FindByUniqueName("DIEM_TRUNG_BINH_KY2_VIEW").ItemStyle.CssClass = RadGrid1.MasterTableView.Columns.FindByUniqueName("DIEM_TRUNG_BINH_KY2_VIEW").ItemStyle.CssClass.Replace("Diem10", "").Replace("DiemCD", "").Trim();
            RadGrid1.MasterTableView.Columns.FindByUniqueName("DIEM_TRUNG_BINH_KY2_VIEW").ItemStyle.CssClass += " " + cellClass;
            RadGrid1.MasterTableView.Columns.FindByUniqueName("DIEM_TRUNG_BINH_CN_VIEW").ItemStyle.CssClass = RadGrid1.MasterTableView.Columns.FindByUniqueName("DIEM_TRUNG_BINH_CN_VIEW").ItemStyle.CssClass.Replace("Diem10", "").Replace("DiemCD", "").Trim();
            RadGrid1.MasterTableView.Columns.FindByUniqueName("DIEM_TRUNG_BINH_CN_VIEW").ItemStyle.CssClass += " " + cellClass;
            if (Sys_Hoc_Ky == 1)
            {
                RadGrid1.MasterTableView.Columns.FindByUniqueName("DIEM_TRUNG_BINH_KY2_VIEW").Display = false;
                RadGrid1.MasterTableView.Columns.FindByUniqueName("DIEM_TRUNG_BINH_CN_VIEW").Display = false;
            }
            #endregion
            #region Set view column by lop-mon
            if (detailLopMon != null)
            {
                for (int i = 1; i <= 5; i++)
                {
                    if (detailLopMon.SO_COT_DIEM_MIENG != null)
                        RadGrid1.MasterTableView.Columns.FindByUniqueName("DIEM" + i + "_VIEW").Display = (i <= detailLopMon.SO_COT_DIEM_MIENG.Value);
                    if (detailLopMon.SO_COT_DIEM_15P != null)
                        RadGrid1.MasterTableView.Columns.FindByUniqueName("DIEM" + (5 + i) + "_VIEW").Display = (i <= detailLopMon.SO_COT_DIEM_15P.Value);
                    if (detailLopMon.SO_COT_DIEM_1T_HS1 != null)
                        RadGrid1.MasterTableView.Columns.FindByUniqueName("DIEM" + (10 + i) + "_VIEW").Display = (i <= detailLopMon.SO_COT_DIEM_1T_HS1.Value);
                    if (detailLopMon.SO_COT_DIEM_1T_HS2 != null)
                        RadGrid1.MasterTableView.Columns.FindByUniqueName("DIEM" + (15 + i) + "_VIEW").Display = (i <= detailLopMon.SO_COT_DIEM_1T_HS2.Value);
                    RadGrid1.MasterTableView.Columns.FindByUniqueName("DIEM" + (20 + i) + "_VIEW").Display = false;
                }
            }
            #endregion
            #region ẩn 5 cột điểm
            RadGrid1.MasterTableView.Columns.FindByUniqueName("DIEM21_VIEW").Display = false;
            RadGrid1.MasterTableView.Columns.FindByUniqueName("DIEM22_VIEW").Display = false;
            RadGrid1.MasterTableView.Columns.FindByUniqueName("DIEM23_VIEW").Display = false;
            RadGrid1.MasterTableView.Columns.FindByUniqueName("DIEM24_VIEW").Display = false;
            RadGrid1.MasterTableView.Columns.FindByUniqueName("DIEM25_VIEW").Display = false;
            #endregion
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
        protected void rcbMonHoc_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            insertEmpty();
            checkKhoaSoTheoMon();
            RadGrid1.Rebind();
        }
        protected void btSave_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            if (checkKhoa == true)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Môn học đã bị khóa, bạn không thể sửa điểm!');", true);
                return;
            }
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            short? ma_khoi = localAPI.ConvertStringToShort(rcbKhoiHoc.SelectedValue);
            long? id_lop = localAPI.ConvertStringTolong(rcbLop.SelectedValue);
            long? id_mon_hoc_truong = localAPI.ConvertStringTolong(rcbMonHoc.SelectedValue);
            if (id_mon_hoc_truong == null) id_mon_hoc_truong = 0;
            MON_HOC_TRUONG detailMonHoc = new MON_HOC_TRUONG();
            detailMonHoc = monHocTruongBO.getMonTruongByID(id_mon_hoc_truong.Value);
            if (id_lop != null && id_mon_hoc_truong != null)
            {
                int success = 0; int count_change = 0;
                foreach (GridDataItem item in RadGrid1.Items)
                {
                    var detail = new DIEM_CHI_TIET();
                    long id_diem = localAPI.ConvertStringTolong(item.GetDataKeyValue("ID").ToString()).Value;
                    detail = diemChiTietBO.getDiemChiTietByID(id_diem);
                    if (detail != null && detailMonHoc != null)
                    {
                        #region get control
                        TextBox tbDIEM1 = (TextBox)item.FindControl("tbDIEM1");
                        TextBox tbDIEM2 = (TextBox)item.FindControl("tbDIEM2");
                        TextBox tbDIEM3 = (TextBox)item.FindControl("tbDIEM3");
                        TextBox tbDIEM4 = (TextBox)item.FindControl("tbDIEM4");
                        TextBox tbDIEM5 = (TextBox)item.FindControl("tbDIEM5");
                        TextBox tbDIEM6 = (TextBox)item.FindControl("tbDIEM6");
                        TextBox tbDIEM7 = (TextBox)item.FindControl("tbDIEM7");
                        TextBox tbDIEM8 = (TextBox)item.FindControl("tbDIEM8");
                        TextBox tbDIEM9 = (TextBox)item.FindControl("tbDIEM9");
                        TextBox tbDIEM10 = (TextBox)item.FindControl("tbDIEM10");
                        TextBox tbDIEM11 = (TextBox)item.FindControl("tbDIEM11");
                        TextBox tbDIEM12 = (TextBox)item.FindControl("tbDIEM12");
                        TextBox tbDIEM13 = (TextBox)item.FindControl("tbDIEM13");
                        TextBox tbDIEM14 = (TextBox)item.FindControl("tbDIEM14");
                        TextBox tbDIEM15 = (TextBox)item.FindControl("tbDIEM15");
                        TextBox tbDIEM16 = (TextBox)item.FindControl("tbDIEM16");
                        TextBox tbDIEM17 = (TextBox)item.FindControl("tbDIEM17");
                        TextBox tbDIEM18 = (TextBox)item.FindControl("tbDIEM18");
                        TextBox tbDIEM19 = (TextBox)item.FindControl("tbDIEM19");
                        TextBox tbDIEM20 = (TextBox)item.FindControl("tbDIEM20");
                        TextBox tbDIEM21 = (TextBox)item.FindControl("tbDIEM21");
                        TextBox tbDIEM22 = (TextBox)item.FindControl("tbDIEM22");
                        TextBox tbDIEM23 = (TextBox)item.FindControl("tbDIEM23");
                        TextBox tbDIEM24 = (TextBox)item.FindControl("tbDIEM24");
                        TextBox tbDIEM25 = (TextBox)item.FindControl("tbDIEM25");
                        TextBox tbDIEM_HOC_KY = (TextBox)item.FindControl("tbDIEM_HOC_KY");
                        TextBox tbDIEM_TRUNG_BINH_KY1 = (TextBox)item.FindControl("tbDIEM_TRUNG_BINH_KY1");
                        TextBox tbDIEM_TRUNG_BINH_KY2 = (TextBox)item.FindControl("tbDIEM_TRUNG_BINH_KY2");
                        HiddenField hdDIEM1 = (HiddenField)item.FindControl("hdDIEM1");
                        HiddenField hdDIEM2 = (HiddenField)item.FindControl("hdDIEM2");
                        HiddenField hdDIEM3 = (HiddenField)item.FindControl("hdDIEM3");
                        HiddenField hdDIEM4 = (HiddenField)item.FindControl("hdDIEM4");
                        HiddenField hdDIEM5 = (HiddenField)item.FindControl("hdDIEM5");
                        HiddenField hdDIEM6 = (HiddenField)item.FindControl("hdDIEM6");
                        HiddenField hdDIEM7 = (HiddenField)item.FindControl("hdDIEM7");
                        HiddenField hdDIEM8 = (HiddenField)item.FindControl("hdDIEM8");
                        HiddenField hdDIEM9 = (HiddenField)item.FindControl("hdDIEM9");
                        HiddenField hdDIEM10 = (HiddenField)item.FindControl("hdDIEM10");
                        HiddenField hdDIEM11 = (HiddenField)item.FindControl("hdDIEM11");
                        HiddenField hdDIEM12 = (HiddenField)item.FindControl("hdDIEM12");
                        HiddenField hdDIEM13 = (HiddenField)item.FindControl("hdDIEM13");
                        HiddenField hdDIEM14 = (HiddenField)item.FindControl("hdDIEM14");
                        HiddenField hdDIEM15 = (HiddenField)item.FindControl("hdDIEM15");
                        HiddenField hdDIEM16 = (HiddenField)item.FindControl("hdDIEM16");
                        HiddenField hdDIEM17 = (HiddenField)item.FindControl("hdDIEM17");
                        HiddenField hdDIEM18 = (HiddenField)item.FindControl("hdDIEM18");
                        HiddenField hdDIEM19 = (HiddenField)item.FindControl("hdDIEM19");
                        HiddenField hdDIEM20 = (HiddenField)item.FindControl("hdDIEM20");
                        HiddenField hdDIEM21 = (HiddenField)item.FindControl("hdDIEM21");
                        HiddenField hdDIEM22 = (HiddenField)item.FindControl("hdDIEM22");
                        HiddenField hdDIEM23 = (HiddenField)item.FindControl("hdDIEM23");
                        HiddenField hdDIEM24 = (HiddenField)item.FindControl("hdDIEM24");
                        HiddenField hdDIEM25 = (HiddenField)item.FindControl("hdDIEM25");
                        HiddenField hdDIEM_HOC_KY = (HiddenField)item.FindControl("hdDIEM_HOC_KY");
                        HiddenField hdDIEM_TRUNG_BINH_KY1 = (HiddenField)item.FindControl("hdDIEM_TRUNG_BINH_KY1");
                        HiddenField hdDIEM_TRUNG_BINH_KY2 = (HiddenField)item.FindControl("hdDIEM_TRUNG_BINH_KY2");
                        #endregion
                        #region get value control
                        string DIEM1 = tbDIEM1.Text.Trim();
                        string DIEM2 = tbDIEM2.Text.Trim();
                        string DIEM3 = tbDIEM3.Text.Trim();
                        string DIEM4 = tbDIEM4.Text.Trim();
                        string DIEM5 = tbDIEM5.Text.Trim();
                        string DIEM6 = tbDIEM6.Text.Trim();
                        string DIEM7 = tbDIEM7.Text.Trim();
                        string DIEM8 = tbDIEM8.Text.Trim();
                        string DIEM9 = tbDIEM9.Text.Trim();
                        string DIEM10 = tbDIEM10.Text.Trim();
                        string DIEM11 = tbDIEM11.Text.Trim();
                        string DIEM12 = tbDIEM12.Text.Trim();
                        string DIEM13 = tbDIEM13.Text.Trim();
                        string DIEM14 = tbDIEM14.Text.Trim();
                        string DIEM15 = tbDIEM15.Text.Trim();
                        string DIEM16 = tbDIEM16.Text.Trim();
                        string DIEM17 = tbDIEM17.Text.Trim();
                        string DIEM18 = tbDIEM18.Text.Trim();
                        string DIEM19 = tbDIEM19.Text.Trim();
                        string DIEM20 = tbDIEM20.Text.Trim();
                        string DIEM21 = tbDIEM21.Text.Trim();
                        string DIEM22 = tbDIEM22.Text.Trim();
                        string DIEM23 = tbDIEM23.Text.Trim();
                        string DIEM24 = tbDIEM24.Text.Trim();
                        string DIEM25 = tbDIEM25.Text.Trim();
                        string DIEM_HOC_KY = tbDIEM_HOC_KY.Text.Trim();
                        string DIEM_TRUNG_BINH_KY1 = tbDIEM_TRUNG_BINH_KY1.Text.Trim();
                        string DIEM_TRUNG_BINH_KY2 = tbDIEM_TRUNG_BINH_KY1.Text.Trim();
                        string DIEM1_old = hdDIEM1.Value;
                        string DIEM2_old = hdDIEM2.Value;
                        string DIEM3_old = hdDIEM3.Value;
                        string DIEM4_old = hdDIEM4.Value;
                        string DIEM5_old = hdDIEM5.Value;
                        string DIEM6_old = hdDIEM6.Value;
                        string DIEM7_old = hdDIEM7.Value;
                        string DIEM8_old = hdDIEM8.Value;
                        string DIEM9_old = hdDIEM9.Value;
                        string DIEM10_old = hdDIEM10.Value;
                        string DIEM11_old = hdDIEM11.Value;
                        string DIEM12_old = hdDIEM12.Value;
                        string DIEM13_old = hdDIEM13.Value;
                        string DIEM14_old = hdDIEM14.Value;
                        string DIEM15_old = hdDIEM15.Value;
                        string DIEM16_old = hdDIEM16.Value;
                        string DIEM17_old = hdDIEM17.Value;
                        string DIEM18_old = hdDIEM18.Value;
                        string DIEM19_old = hdDIEM19.Value;
                        string DIEM20_old = hdDIEM20.Value;
                        string DIEM21_old = hdDIEM21.Value;
                        string DIEM22_old = hdDIEM22.Value;
                        string DIEM23_old = hdDIEM23.Value;
                        string DIEM24_old = hdDIEM24.Value;
                        string DIEM25_old = hdDIEM25.Value;
                        string DIEM_HOC_KY_old = hdDIEM_HOC_KY.Value;
                        string DIEM_TRUNG_BINH_KY1_old = hdDIEM_TRUNG_BINH_KY1.Value;
                        string DIEM_TRUNG_BINH_KY2_old = hdDIEM_TRUNG_BINH_KY1.Value;
                        #endregion
                        #region set value detail
                        detail.DIEM1 = (detailMonHoc.KIEU_MON == null || detailMonHoc.KIEU_MON.Value == false) ? localAPI.ConvertStringToDecimal(DIEM1) : dataAccessAPI.ConvertCDToDecimal(DIEM1);
                        detail.DIEM2 = (detailMonHoc.KIEU_MON == null || detailMonHoc.KIEU_MON.Value == false) ? localAPI.ConvertStringToDecimal(DIEM2) : dataAccessAPI.ConvertCDToDecimal(DIEM2);
                        detail.DIEM3 = (detailMonHoc.KIEU_MON == null || detailMonHoc.KIEU_MON.Value == false) ? localAPI.ConvertStringToDecimal(DIEM3) : dataAccessAPI.ConvertCDToDecimal(DIEM3);
                        detail.DIEM4 = (detailMonHoc.KIEU_MON == null || detailMonHoc.KIEU_MON.Value == false) ? localAPI.ConvertStringToDecimal(DIEM4) : dataAccessAPI.ConvertCDToDecimal(DIEM4);
                        detail.DIEM5 = (detailMonHoc.KIEU_MON == null || detailMonHoc.KIEU_MON.Value == false) ? localAPI.ConvertStringToDecimal(DIEM5) : dataAccessAPI.ConvertCDToDecimal(DIEM5);
                        detail.DIEM6 = (detailMonHoc.KIEU_MON == null || detailMonHoc.KIEU_MON.Value == false) ? localAPI.ConvertStringToDecimal(DIEM6) : dataAccessAPI.ConvertCDToDecimal(DIEM6);
                        detail.DIEM7 = (detailMonHoc.KIEU_MON == null || detailMonHoc.KIEU_MON.Value == false) ? localAPI.ConvertStringToDecimal(DIEM7) : dataAccessAPI.ConvertCDToDecimal(DIEM7);
                        detail.DIEM8 = (detailMonHoc.KIEU_MON == null || detailMonHoc.KIEU_MON.Value == false) ? localAPI.ConvertStringToDecimal(DIEM8) : dataAccessAPI.ConvertCDToDecimal(DIEM8);
                        detail.DIEM9 = (detailMonHoc.KIEU_MON == null || detailMonHoc.KIEU_MON.Value == false) ? localAPI.ConvertStringToDecimal(DIEM9) : dataAccessAPI.ConvertCDToDecimal(DIEM9);
                        detail.DIEM10 = (detailMonHoc.KIEU_MON == null || detailMonHoc.KIEU_MON.Value == false) ? localAPI.ConvertStringToDecimal(DIEM10) : dataAccessAPI.ConvertCDToDecimal(DIEM10);
                        detail.DIEM11 = (detailMonHoc.KIEU_MON == null || detailMonHoc.KIEU_MON.Value == false) ? localAPI.ConvertStringToDecimal(DIEM11) : dataAccessAPI.ConvertCDToDecimal(DIEM11);
                        detail.DIEM12 = (detailMonHoc.KIEU_MON == null || detailMonHoc.KIEU_MON.Value == false) ? localAPI.ConvertStringToDecimal(DIEM12) : dataAccessAPI.ConvertCDToDecimal(DIEM12);
                        detail.DIEM13 = (detailMonHoc.KIEU_MON == null || detailMonHoc.KIEU_MON.Value == false) ? localAPI.ConvertStringToDecimal(DIEM13) : dataAccessAPI.ConvertCDToDecimal(DIEM13);
                        detail.DIEM14 = (detailMonHoc.KIEU_MON == null || detailMonHoc.KIEU_MON.Value == false) ? localAPI.ConvertStringToDecimal(DIEM14) : dataAccessAPI.ConvertCDToDecimal(DIEM14);
                        detail.DIEM15 = (detailMonHoc.KIEU_MON == null || detailMonHoc.KIEU_MON.Value == false) ? localAPI.ConvertStringToDecimal(DIEM15) : dataAccessAPI.ConvertCDToDecimal(DIEM15);
                        detail.DIEM16 = (detailMonHoc.KIEU_MON == null || detailMonHoc.KIEU_MON.Value == false) ? localAPI.ConvertStringToDecimal(DIEM16) : dataAccessAPI.ConvertCDToDecimal(DIEM16);
                        detail.DIEM17 = (detailMonHoc.KIEU_MON == null || detailMonHoc.KIEU_MON.Value == false) ? localAPI.ConvertStringToDecimal(DIEM17) : dataAccessAPI.ConvertCDToDecimal(DIEM17);
                        detail.DIEM18 = (detailMonHoc.KIEU_MON == null || detailMonHoc.KIEU_MON.Value == false) ? localAPI.ConvertStringToDecimal(DIEM18) : dataAccessAPI.ConvertCDToDecimal(DIEM18);
                        detail.DIEM19 = (detailMonHoc.KIEU_MON == null || detailMonHoc.KIEU_MON.Value == false) ? localAPI.ConvertStringToDecimal(DIEM19) : dataAccessAPI.ConvertCDToDecimal(DIEM19);
                        detail.DIEM20 = (detailMonHoc.KIEU_MON == null || detailMonHoc.KIEU_MON.Value == false) ? localAPI.ConvertStringToDecimal(DIEM20) : dataAccessAPI.ConvertCDToDecimal(DIEM20);
                        detail.DIEM21 = (detailMonHoc.KIEU_MON == null || detailMonHoc.KIEU_MON.Value == false) ? localAPI.ConvertStringToDecimal(DIEM21) : dataAccessAPI.ConvertCDToDecimal(DIEM21);
                        detail.DIEM22 = (detailMonHoc.KIEU_MON == null || detailMonHoc.KIEU_MON.Value == false) ? localAPI.ConvertStringToDecimal(DIEM22) : dataAccessAPI.ConvertCDToDecimal(DIEM22);
                        detail.DIEM23 = (detailMonHoc.KIEU_MON == null || detailMonHoc.KIEU_MON.Value == false) ? localAPI.ConvertStringToDecimal(DIEM23) : dataAccessAPI.ConvertCDToDecimal(DIEM23);
                        detail.DIEM24 = (detailMonHoc.KIEU_MON == null || detailMonHoc.KIEU_MON.Value == false) ? localAPI.ConvertStringToDecimal(DIEM24) : dataAccessAPI.ConvertCDToDecimal(DIEM24);
                        detail.DIEM25 = (detailMonHoc.KIEU_MON == null || detailMonHoc.KIEU_MON.Value == false) ? localAPI.ConvertStringToDecimal(DIEM25) : dataAccessAPI.ConvertCDToDecimal(DIEM25);
                        detail.DIEM_HOC_KY = (detailMonHoc.KIEU_MON == null || detailMonHoc.KIEU_MON.Value == false) ? localAPI.ConvertStringToDecimal(DIEM_HOC_KY) : dataAccessAPI.ConvertCDToDecimal(DIEM_HOC_KY);
                        //detail.DIEM_TRUNG_BINH_KY1 = (detailMonHoc.KIEU_MON == null || detailMonHoc.KIEU_MON.Value == false) ? localAPI.ConvertStringToDecimal(DIEM_TRUNG_BINH_KY1) : dataAccessAPI.ConvertCDToDecimal(DIEM_TRUNG_BINH_KY1);
                        //detail.DIEM_TRUNG_BINH_KY2 = (detailMonHoc.KIEU_MON == null || detailMonHoc.KIEU_MON.Value == false) ? localAPI.ConvertStringToDecimal(DIEM_TRUNG_BINH_KY2) : dataAccessAPI.ConvertCDToDecimal(DIEM_TRUNG_BINH_KY2);
                        #endregion
                        if (DIEM1 != DIEM1_old || DIEM2 != DIEM2_old || DIEM3 != DIEM3_old || DIEM4 != DIEM4_old || DIEM5 != DIEM5_old
                            || DIEM6 != DIEM6_old || DIEM7 != DIEM7_old || DIEM8 != DIEM8_old || DIEM9 != DIEM9_old || DIEM10 != DIEM10_old
                            || DIEM11 != DIEM11_old || DIEM12 != DIEM12_old || DIEM13 != DIEM13_old || DIEM14 != DIEM14_old || DIEM15 != DIEM15_old
                            || DIEM16 != DIEM16_old || DIEM17 != DIEM17_old || DIEM18 != DIEM18_old || DIEM19 != DIEM19_old || DIEM20 != DIEM20_old
                            || DIEM21 != DIEM21_old || DIEM22 != DIEM22_old || DIEM23 != DIEM23_old || DIEM24 != DIEM24_old || DIEM25 != DIEM25_old
                            || DIEM_HOC_KY != DIEM_HOC_KY_old)
                        {
                            count_change++;
                            res = diemChiTietBO.update(detail, Sys_User);
                            if (res.Res)
                                success++;
                        }
                    }
                }
                string strMsg = "";
                if (count_change - success > 0)
                {
                    strMsg = "notification('error', 'Có " + (count_change - success) + " bản ghi chưa được lưu. Liên hệ với quản trị viên');";
                }
                if (success > 0)
                {
                    strMsg += " notification('success', 'Có " + success + " bản ghi được lưu.');";
                }
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Có lỗi xãy ra');", true);
            }
            RadGrid1.Rebind();
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
            string newName = "Ket_qua_hoc_tap.xlsx";

            List<ExcelHeaderEntity> lstHeader = new List<ExcelHeaderEntity>();
            List<ExcelEntity> lstColumn = new List<ExcelEntity>();
            lstHeader.Add(new ExcelHeaderEntity { name = "STT", colM = 1, rowM = 3, width = 10 });
            List<ExcelHeaderEntity> lstTmpMieng = new List<ExcelHeaderEntity>();
            List<ExcelHeaderEntity> lstTmp15P = new List<ExcelHeaderEntity>();
            List<ExcelHeaderEntity> lstTmp1T = new List<ExcelHeaderEntity>();


            #region add column
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MA_HS"))
            {
                DataColumn col = new DataColumn("MA_HS");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Mã HS", colM = 1, rowM = 3, width = 18 });
                lstColumn.Add(new ExcelEntity { Name = "MA_HS", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TEN_HS"))
            {
                DataColumn col = new DataColumn("TEN_HS");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Họ tên", colM = 1, rowM = 3, width = 30 });
                lstColumn.Add(new ExcelEntity { Name = "TEN_HS", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "NGAY_SINH"))
            {
                DataColumn col = new DataColumn("NGAY_SINH");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Ngày sinh", colM = 1, rowM = 3, width = 15 });
                lstColumn.Add(new ExcelEntity { Name = "NGAY_SINH", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            #region "ĐIỂM HỆ SỐ 1"

            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM1_VIEW"))
                lstTmpMieng.Add(new ExcelHeaderEntity { name = "1", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM2_VIEW"))
                lstTmpMieng.Add(new ExcelHeaderEntity { name = "2", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM3_VIEW"))
                lstTmpMieng.Add(new ExcelHeaderEntity { name = "3", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM4_VIEW"))
                lstTmpMieng.Add(new ExcelHeaderEntity { name = "4", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM5_VIEW"))
                lstTmpMieng.Add(new ExcelHeaderEntity { name = "5", colM = 1, rowM = 1, width = 10 });

            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM6_VIEW"))
                lstTmp15P.Add(new ExcelHeaderEntity { name = "1", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM7_VIEW"))
                lstTmp15P.Add(new ExcelHeaderEntity { name = "2", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM8_VIEW"))
                lstTmp15P.Add(new ExcelHeaderEntity { name = "3", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM9_VIEW"))
                lstTmp15P.Add(new ExcelHeaderEntity { name = "4", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM10_VIEW"))
                lstTmp15P.Add(new ExcelHeaderEntity { name = "5", colM = 1, rowM = 1, width = 10 });

            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM11_VIEW"))
                lstTmp1T.Add(new ExcelHeaderEntity { name = "1", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM12_VIEW"))
                lstTmp1T.Add(new ExcelHeaderEntity { name = "2", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM13_VIEW"))
                lstTmp1T.Add(new ExcelHeaderEntity { name = "3", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM14_VIEW"))
                lstTmp1T.Add(new ExcelHeaderEntity { name = "4", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM15_VIEW"))
                lstTmp1T.Add(new ExcelHeaderEntity { name = "5", colM = 1, rowM = 1, width = 10 });
            int countHS1 = 0;
            int indexHS1 = 0;
            if ((lstTmpMieng != null & lstTmpMieng.Count > 0) || (lstTmp15P != null & lstTmp15P.Count > 0) || (lstTmp1T != null && lstTmp1T.Count > 0))
            {
                if (lstTmpMieng != null) countHS1 = lstTmpMieng.Count;
                if (lstTmp15P != null) countHS1 += lstTmp15P.Count;
                if (lstTmp1T != null) countHS1 += lstTmp1T.Count;
                lstHeader.Add(new ExcelHeaderEntity { name = "Điểm hệ số 1", colM = countHS1, rowM = 1, width = countHS1 * 10 });
                indexHS1 = lstHeader.Count - 1;
            }
            int indexMieng = 0;
            if (lstTmpMieng != null && lstTmpMieng.Count > 0)
            {
                lstHeader.Add(new ExcelHeaderEntity { name = "Miệng", colM = lstTmpMieng.Count, rowM = 1, width = lstTmpMieng.Count * 10, parentIndex = indexHS1 });
                indexMieng = lstHeader.Count - 1;
            }

            #region "add diem mieng"
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM1_VIEW"))
            {
                DataColumn col = new DataColumn("DIEM1_VIEW");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1", colM = 1, rowM = 1, width = 10, parentIndex = indexMieng });
                lstColumn.Add(new ExcelEntity { Name = "DIEM1_VIEW", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM2_VIEW"))
            {
                DataColumn col = new DataColumn("DIEM2_VIEW");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "2", colM = 1, rowM = 1, width = 10, parentIndex = indexMieng });
                lstColumn.Add(new ExcelEntity { Name = "DIEM2_VIEW", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM3_VIEW"))
            {
                DataColumn col = new DataColumn("DIEM3_VIEW");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "3", colM = 1, rowM = 1, width = 10, parentIndex = indexMieng });
                lstColumn.Add(new ExcelEntity { Name = "DIEM3_VIEW", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM4_VIEW"))
            {
                DataColumn col = new DataColumn("DIEM4_VIEW");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "4", colM = 1, rowM = 1, width = 10, parentIndex = indexMieng });
                lstColumn.Add(new ExcelEntity { Name = "DIEM4_VIEW", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM5_VIEW"))
            {
                DataColumn col = new DataColumn("DIEM5_VIEW");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "5", colM = 1, rowM = 1, width = 10, parentIndex = indexMieng });
                lstColumn.Add(new ExcelEntity { Name = "DIEM5_VIEW", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            #endregion
            int index15p = 0;
            if (lstTmp15P != null && lstTmp15P.Count > 0)
            {
                lstHeader.Add(new ExcelHeaderEntity { name = "15 phút", colM = lstTmp15P.Count, rowM = 1, width = lstTmp15P.Count * 10, parentIndex = indexHS1 });
                index15p = lstHeader.Count - 1;
            }
            #region "add diem 15p"
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM6_VIEW"))
            {
                DataColumn col = new DataColumn("DIEM6_VIEW");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1", colM = 1, rowM = 1, width = 10, parentIndex = index15p });
                lstColumn.Add(new ExcelEntity { Name = "DIEM6_VIEW", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM7_VIEW"))
            {
                DataColumn col = new DataColumn("DIEM7_VIEW");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "2", colM = 1, rowM = 1, width = 10, parentIndex = index15p });
                lstColumn.Add(new ExcelEntity { Name = "DIEM7_VIEW", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM8_VIEW"))
            {
                DataColumn col = new DataColumn("DIEM8_VIEW");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "3", colM = 1, rowM = 1, width = 10, parentIndex = index15p });
                lstColumn.Add(new ExcelEntity { Name = "DIEM8_VIEW", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM9_VIEW"))
            {
                DataColumn col = new DataColumn("DIEM9_VIEW");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "4", colM = 1, rowM = 1, width = 10, parentIndex = index15p });
                lstColumn.Add(new ExcelEntity { Name = "DIEM9_VIEW", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM10_VIEW"))
            {
                DataColumn col = new DataColumn("DIEM10_VIEW");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "5", colM = 1, rowM = 1, width = 10, parentIndex = index15p });
                lstColumn.Add(new ExcelEntity { Name = "DIEM10_VIEW", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            #endregion
            int index1T = 0;
            if (lstTmp1T != null && lstTmp1T.Count > 0)
            {
                lstHeader.Add(new ExcelHeaderEntity { name = "1 tiết", colM = lstTmp1T.Count, rowM = 1, width = lstTmp1T.Count * 10, parentIndex = indexHS1 });
                index1T = lstHeader.Count - 1;
            }
            #region "add 1 tiet"
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM11_VIEW"))
            {
                DataColumn col = new DataColumn("DIEM11_VIEW");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1", colM = 1, rowM = 1, width = 10, parentIndex = index1T });
                lstColumn.Add(new ExcelEntity { Name = "DIEM11_VIEW", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM12_VIEW"))
            {
                DataColumn col = new DataColumn("DIEM12_VIEW");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "2", colM = 1, rowM = 1, width = 10, parentIndex = index1T });
                lstColumn.Add(new ExcelEntity { Name = "DIEM12_VIEW", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM13_VIEW"))
            {
                DataColumn col = new DataColumn("DIEM13_VIEW");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "3", colM = 1, rowM = 1, width = 10, parentIndex = index1T });
                lstColumn.Add(new ExcelEntity { Name = "DIEM13_VIEW", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM14_VIEW"))
            {
                DataColumn col = new DataColumn("DIEM14_VIEW");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "4", colM = 1, rowM = 1, width = 10, parentIndex = index1T });
                lstColumn.Add(new ExcelEntity { Name = "DIEM14_VIEW", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM15_VIEW"))
            {
                DataColumn col = new DataColumn("DIEM15_VIEW");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "5", colM = 1, rowM = 1, width = 10, parentIndex = index1T });
                lstColumn.Add(new ExcelEntity { Name = "DIEM15_VIEW", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            #endregion
            #endregion
            #region "ĐIỂM HỆ SỐ 2"
            int indexHS2 = 0;
            lstTmp1T = new List<ExcelHeaderEntity>();
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM16_VIEW"))
                lstTmp1T.Add(new ExcelHeaderEntity { name = "1", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM17_VIEW"))
                lstTmp1T.Add(new ExcelHeaderEntity { name = "2", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM18_VIEW"))
                lstTmp1T.Add(new ExcelHeaderEntity { name = "3", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM19_VIEW"))
                lstTmp1T.Add(new ExcelHeaderEntity { name = "4", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM20_VIEW"))
                lstTmp1T.Add(new ExcelHeaderEntity { name = "5", colM = 1, rowM = 1, width = 10 });
            int index1THS2 = 0;
            if (lstTmp1T != null && lstTmp1T.Count > 0)
            {
                lstHeader.Add(new ExcelHeaderEntity { name = "Điểm hệ số 2", colM = lstTmp1T.Count, rowM = 1, width = lstTmp1T.Count * 10 });
                indexHS2 = lstHeader.Count - 1;
                lstHeader.Add(new ExcelHeaderEntity { name = "1 tiết", colM = lstTmp1T.Count, rowM = 1, width = lstTmp1T.Count * 10, parentIndex = indexHS2 });
                index1THS2 = lstHeader.Count - 1;
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM16_VIEW"))
            {
                DataColumn col = new DataColumn("DIEM16_VIEW");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1", colM = 1, rowM = 1, width = 10, parentIndex = index1THS2 });
                lstColumn.Add(new ExcelEntity { Name = "DIEM16_VIEW", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM17_VIEW"))
            {
                DataColumn col = new DataColumn("DIEM17_VIEW");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "2", colM = 1, rowM = 1, width = 10, parentIndex = index1THS2 });
                lstColumn.Add(new ExcelEntity { Name = "DIEM17_VIEW", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM18_VIEW"))
            {
                DataColumn col = new DataColumn("DIEM18_VIEW");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "3", colM = 1, rowM = 1, width = 10, parentIndex = index1THS2 });
                lstColumn.Add(new ExcelEntity { Name = "DIEM18_VIEW", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM19_VIEW"))
            {
                DataColumn col = new DataColumn("DIEM19_VIEW");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "4", colM = 1, rowM = 1, width = 10, parentIndex = index1THS2 });
                lstColumn.Add(new ExcelEntity { Name = "DIEM19_VIEW", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM20_VIEW"))
            {
                DataColumn col = new DataColumn("DIEM20_VIEW");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "5", colM = 1, rowM = 1, width = 10, parentIndex = index1THS2 });
                lstColumn.Add(new ExcelEntity { Name = "DIEM20_VIEW", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            #endregion
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM_HOC_KY_VIEW"))
            {
                DataColumn col = new DataColumn("DIEM_HOC_KY_VIEW");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Học kỳ", colM = 1, rowM = 3, width = 10 });
                lstColumn.Add(new ExcelEntity { Name = "DIEM_HOC_KY_VIEW", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM_TRUNG_BINH_KY1_VIEW"))
            {
                DataColumn col = new DataColumn("DIEM_TRUNG_BINH_KY1_VIEW");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "TBM kỳ I", colM = 1, rowM = 3, width = 10 });
                lstColumn.Add(new ExcelEntity { Name = "DIEM_TRUNG_BINH_KY1_VIEW", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIEM_TRUNG_BINH_KY2_VIEW"))
            {
                DataColumn col = new DataColumn("DIEM_TRUNG_BINH_KY2_VIEW");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "TBM kỳ II", colM = 1, rowM = 3, width = 10 });
                lstColumn.Add(new ExcelEntity { Name = "DIEM_TRUNG_BINH_KY2_VIEW", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });

            }
            #endregion
            
            foreach (GridDataItem item in RadGrid1.Items)
            {
                TextBox tbDIEM1 = (TextBox)item.FindControl("tbDIEM1");
                TextBox tbDIEM2 = (TextBox)item.FindControl("tbDIEM2");
                TextBox tbDIEM3 = (TextBox)item.FindControl("tbDIEM3");
                TextBox tbDIEM4 = (TextBox)item.FindControl("tbDIEM4");
                TextBox tbDIEM5 = (TextBox)item.FindControl("tbDIEM5");
                TextBox tbDIEM6 = (TextBox)item.FindControl("tbDIEM6");
                TextBox tbDIEM7 = (TextBox)item.FindControl("tbDIEM7");
                TextBox tbDIEM8 = (TextBox)item.FindControl("tbDIEM8");
                TextBox tbDIEM9 = (TextBox)item.FindControl("tbDIEM9");
                TextBox tbDIEM10 = (TextBox)item.FindControl("tbDIEM10");
                TextBox tbDIEM11 = (TextBox)item.FindControl("tbDIEM11");
                TextBox tbDIEM12 = (TextBox)item.FindControl("tbDIEM12");
                TextBox tbDIEM13 = (TextBox)item.FindControl("tbDIEM13");
                TextBox tbDIEM14 = (TextBox)item.FindControl("tbDIEM14");
                TextBox tbDIEM15 = (TextBox)item.FindControl("tbDIEM15");
                TextBox tbDIEM16 = (TextBox)item.FindControl("tbDIEM16");
                TextBox tbDIEM17 = (TextBox)item.FindControl("tbDIEM17");
                TextBox tbDIEM18 = (TextBox)item.FindControl("tbDIEM18");
                TextBox tbDIEM19 = (TextBox)item.FindControl("tbDIEM19");
                TextBox tbDIEM20 = (TextBox)item.FindControl("tbDIEM20");
                TextBox tbDIEM_HOC_KY = (TextBox)item.FindControl("tbDIEM_HOC_KY");
                TextBox tbDIEM_TRUNG_BINH_KY1 = (TextBox)item.FindControl("tbDIEM_TRUNG_BINH_KY1");
                TextBox tbDIEM_TRUNG_BINH_KY2 = (TextBox)item.FindControl("tbDIEM_TRUNG_BINH_KY2");
                DataRow row = dt.NewRow();
                foreach (DataColumn col in dt.Columns)
                {
                    row[col.ColumnName] = item[col.ColumnName].Text.Replace("&nbsp;", " ");
                    if (col.ColumnName == "DIEM1_VIEW") row[col.ColumnName] = tbDIEM1.Text;
                    if (col.ColumnName == "DIEM2_VIEW") row[col.ColumnName] = tbDIEM2.Text;
                    if (col.ColumnName == "DIEM3_VIEW") row[col.ColumnName] = tbDIEM3.Text;
                    if (col.ColumnName == "DIEM4_VIEW") row[col.ColumnName] = tbDIEM4.Text;
                    if (col.ColumnName == "DIEM5_VIEW") row[col.ColumnName] = tbDIEM5.Text;
                    if (col.ColumnName == "DIEM6_VIEW") row[col.ColumnName] = tbDIEM6.Text;
                    if (col.ColumnName == "DIEM7_VIEW") row[col.ColumnName] = tbDIEM7.Text;
                    if (col.ColumnName == "DIEM8_VIEW") row[col.ColumnName] = tbDIEM8.Text;
                    if (col.ColumnName == "DIEM9_VIEW") row[col.ColumnName] = tbDIEM9.Text;
                    if (col.ColumnName == "DIEM10_VIEW") row[col.ColumnName] = tbDIEM10.Text;
                    if (col.ColumnName == "DIEM11_VIEW") row[col.ColumnName] = tbDIEM11.Text;
                    if (col.ColumnName == "DIEM12_VIEW") row[col.ColumnName] = tbDIEM12.Text;
                    if (col.ColumnName == "DIEM13_VIEW") row[col.ColumnName] = tbDIEM13.Text;
                    if (col.ColumnName == "DIEM14_VIEW") row[col.ColumnName] = tbDIEM14.Text;
                    if (col.ColumnName == "DIEM15_VIEW") row[col.ColumnName] = tbDIEM15.Text;
                    if (col.ColumnName == "DIEM16_VIEW") row[col.ColumnName] = tbDIEM16.Text;
                    if (col.ColumnName == "DIEM17_VIEW") row[col.ColumnName] = tbDIEM17.Text;
                    if (col.ColumnName == "DIEM18_VIEW") row[col.ColumnName] = tbDIEM18.Text;
                    if (col.ColumnName == "DIEM19_VIEW") row[col.ColumnName] = tbDIEM19.Text;
                    if (col.ColumnName == "DIEM20_VIEW") row[col.ColumnName] = tbDIEM20.Text;
                    if (col.ColumnName == "DIEM_HOC_KY_VIEW") row[col.ColumnName] = tbDIEM_HOC_KY.Text;
                    if (col.ColumnName == "DIEM_TRUNG_BINH_KY1_VIEW") row[col.ColumnName] = tbDIEM_TRUNG_BINH_KY1.Text;
                    if (col.ColumnName == "DIEM_TRUNG_BINH_KY2_VIEW") row[col.ColumnName] = tbDIEM_TRUNG_BINH_KY2.Text;
                }
                dt.Rows.Add(row);
            }

            int rowHeaderStart = 6;
            int rowStart = 9;
            string colStart = "A";
            string colEnd = localAPI.GetExcelColumnName(lstColumn.Count);
            List<ExcelHeaderEntity> listCell = new List<ExcelHeaderEntity>();
            string tenSo = "";
            string tenPhong = Sys_This_Truong.TEN;
            string tieuDe = "DANH SÁCH TỔNG KẾT ĐIỂM MÔN";
            string hocKyNamHoc = "Năm học: " + Sys_Ten_Nam_Hoc;
            listCell.Add(new ExcelHeaderEntity { name = tenSo, colM = 3, rowM = 1, colName = "A", rowIndex = 1, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tenPhong, colM = 3, rowM = 1, colName = "A", rowIndex = 2, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tieuDe, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 3, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            listCell.Add(new ExcelHeaderEntity { name = hocKyNamHoc, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 4, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            string nameFileOutput = newName;
            localAPI.ExportExcelDynamic(serverPath, path, newName, nameFileOutput, 1, listCell, rowHeaderStart, rowStart, colStart, colEnd, dt, lstHeader, lstColumn, true);
        }
        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                TextBox tbDiem1View = (TextBox)item.FindControl("tbDIEM1");
                TextBox tbDiem2View = (TextBox)item.FindControl("tbDIEM2");
                TextBox tbDiem3View = (TextBox)item.FindControl("tbDIEM3");
                TextBox tbDiem4View = (TextBox)item.FindControl("tbDIEM4");
                TextBox tbDiem5View = (TextBox)item.FindControl("tbDIEM5");
                TextBox tbDiem6View = (TextBox)item.FindControl("tbDIEM6");
                TextBox tbDiem7View = (TextBox)item.FindControl("tbDIEM7");
                TextBox tbDiem8View = (TextBox)item.FindControl("tbDIEM8");
                TextBox tbDiem9View = (TextBox)item.FindControl("tbDIEM9");
                TextBox tbDiem10View = (TextBox)item.FindControl("tbDIEM10");
                TextBox tbDiem11View = (TextBox)item.FindControl("tbDIEM11");
                TextBox tbDiem12View = (TextBox)item.FindControl("tbDIEM12");
                TextBox tbDiem13View = (TextBox)item.FindControl("tbDIEM13");
                TextBox tbDiem14View = (TextBox)item.FindControl("tbDIEM14");
                TextBox tbDiem15View = (TextBox)item.FindControl("tbDIEM15");
                TextBox tbDiem16View = (TextBox)item.FindControl("tbDIEM16");
                TextBox tbDiem17View = (TextBox)item.FindControl("tbDIEM17");
                TextBox tbDiem18View = (TextBox)item.FindControl("tbDIEM18");
                TextBox tbDiem19View = (TextBox)item.FindControl("tbDIEM19");
                TextBox tbDiem20View = (TextBox)item.FindControl("tbDIEM20");
                TextBox tbDiem21View = (TextBox)item.FindControl("tbDIEM21");
                TextBox tbDiem22View = (TextBox)item.FindControl("tbDIEM22");
                TextBox tbDiem23View = (TextBox)item.FindControl("tbDIEM23");
                TextBox tbDiem24View = (TextBox)item.FindControl("tbDIEM24");
                TextBox tbDiem25View = (TextBox)item.FindControl("tbDIEM25");
                TextBox tbDiemHocKy = (TextBox)item.FindControl("tbDIEM_HOC_KY");
                TextBox tbDiemTBKy1 = (TextBox)item.FindControl("tbDIEM_TRUNG_BINH_KY1");
                TextBox tbDiemTBKy2 = (TextBox)item.FindControl("tbDIEM_TRUNG_BINH_KY2");
                TextBox tbDiemTBCN = (TextBox)item.FindControl("tbDIEM_TRUNG_BINH_CN");
                
                tbDiem1View.Enabled = (checkKhoa) ? false : true;
                tbDiem2View.Enabled = (checkKhoa) ? false : true;
                tbDiem3View.Enabled = (checkKhoa) ? false : true;
                tbDiem4View.Enabled = (checkKhoa) ? false : true;
                tbDiem5View.Enabled = (checkKhoa) ? false : true;
                tbDiem6View.Enabled = (checkKhoa) ? false : true;
                tbDiem7View.Enabled = (checkKhoa) ? false : true;
                tbDiem8View.Enabled = (checkKhoa) ? false : true;
                tbDiem9View.Enabled = (checkKhoa) ? false : true;
                tbDiem10View.Enabled = (checkKhoa) ? false : true;
                tbDiem11View.Enabled = (checkKhoa) ? false : true;
                tbDiem12View.Enabled = (checkKhoa) ? false : true;
                tbDiem13View.Enabled = (checkKhoa) ? false : true;
                tbDiem14View.Enabled = (checkKhoa) ? false : true;
                tbDiem15View.Enabled = (checkKhoa) ? false : true;
                tbDiem16View.Enabled = (checkKhoa) ? false : true;
                tbDiem17View.Enabled = (checkKhoa) ? false : true;
                tbDiem18View.Enabled = (checkKhoa) ? false : true;
                tbDiem19View.Enabled = (checkKhoa) ? false : true;
                tbDiem20View.Enabled = (checkKhoa) ? false : true;
                tbDiem21View.Enabled = (checkKhoa) ? false : true;
                tbDiem22View.Enabled = (checkKhoa) ? false : true;
                tbDiem23View.Enabled = (checkKhoa) ? false : true;
                tbDiem24View.Enabled = (checkKhoa) ? false : true;
                tbDiem25View.Enabled = (checkKhoa) ? false : true;
                tbDiemHocKy.Enabled = (checkKhoa) ? false : true;
                tbDiemTBKy1.Enabled = false;
                tbDiemTBKy2.Enabled = false;
                tbDiemTBCN.Enabled = (checkKhoa) ? false : true;
				
				if (Sys_Hoc_Ky == 2)
                {
                    tbDiemTBKy1.Enabled = false;
                }
            }
        }
    }
}