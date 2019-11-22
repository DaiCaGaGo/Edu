using ClosedXML.Excel;
using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CMS.HocSinh
{
    public partial class NhapTongHop : AuthenticatePage
    {
        public MonHocTruongBO monHocTruongBO = new MonHocTruongBO();
        public LopMonBO lopMonBO = new LopMonBO();
        public DiemChiTietBO diemChiTietBO = new DiemChiTietBO();
        public NhanXetHangNgayBO nxhnBO = new NhanXetHangNgayBO();
        public LocalAPI localAPI = new LocalAPI();
        LogUserBO logUserBO = new LogUserBO();
        TongHopNXHNBO thBO = new TongHopNXHNBO();
        TruongBO truongBO = new TruongBO();
        QuyTinBO quyTinBO = new QuyTinBO();
        public DataAccessAPI dataAccessAPI = new DataAccessAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            btSave.Visible = is_access(SYS_Type_Access.SUA);
            btnGuiLai.Visible = Sys_User.IS_ROOT == true ? true : false;
            btnGuiTin.Visible = is_access(SYS_Type_Access.SUA);
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
                rpMonHoc.DataBind();
                lblTongTinSuDung.Text = "";
                viewQuyTinCon();
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = nxhnBO.getNXHNByTruongKhoiLopNgay(Sys_This_Truong.ID, localAPI.ConvertStringToShort(rcbKhoiHoc.SelectedValue), localAPI.ConvertStringTolong(rcbLop.SelectedValue), DateTime.Now, Convert.ToInt16(Sys_Ma_Nam_hoc), Convert.ToInt16(Sys_Hoc_Ky));
            RadGrid1.MasterTableView.Columns.FindByUniqueName("SDT_BM").Visible = is_access(SYS_Type_Access.VIEW_INFOR);
            viewColumn();
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
        public void insertEmpty(long id_mon_hoc_truong)
        {
            long? id_lop = localAPI.ConvertStringTolong(rcbLop.SelectedValue);
            short? id_mon_hoc = null;
            MON_HOC_TRUONG detail = new MON_HOC_TRUONG();
            detail = monHocTruongBO.getMonTruongByID(id_mon_hoc_truong);
            if (detail != null) id_mon_hoc = detail.ID_MON_HOC;
            short? ma_khoi = localAPI.ConvertStringToShort(rcbKhoiHoc.SelectedValue);
            if (id_lop != null && detail != null && ma_khoi != null)
                diemChiTietBO.insertEmpty(id_lop.Value, id_mon_hoc, Sys_This_Truong.ID, id_mon_hoc_truong, Convert.ToInt16(Sys_Hoc_Ky), Convert.ToInt16(Sys_Ma_Nam_hoc), ma_khoi.Value);
        }
        private void viewColumn()
        {
            int i = 1;
            foreach (RepeaterItem item in rpMonHoc.Items)
            {
                HiddenField hdIDMonTruong = (HiddenField)item.FindControl("hdIDMonTruong");
                long? id_mon_hoc_truong = localAPI.ConvertStringTolong(hdIDMonTruong.Value);
                HiddenField hdKieuMon = (HiddenField)item.FindControl("hdKieuMon");
                Label lbName = (Label)item.FindControl("lbName");
                CheckBox cbM = (CheckBox)item.FindControl("cbM");
                CheckBox cb15P = (CheckBox)item.FindControl("cb15P");
                CheckBox cb1THS1 = (CheckBox)item.FindControl("cb1THS1");
                CheckBox cb1THS2 = (CheckBox)item.FindControl("cb1THS2");
                CheckBox cbHocKy = (CheckBox)item.FindControl("cbHocKy");
                string cellClass = "Diem10";
                if (hdKieuMon.Value.ToLower() == "false" || string.IsNullOrEmpty(hdKieuMon.Value) || hdKieuMon.Value == "0")
                    cellClass = "Diem10";
                else cellClass = "DiemCD";

                if (id_mon_hoc_truong != null && (cbM.Checked || cb15P.Checked || cb1THS1.Checked || cb1THS2.Checked) || cbHocKy.Checked)
                {
                    insertEmpty(id_mon_hoc_truong.Value);
                }

                if (i <= 20)
                {
                    RadGrid1.MasterTableView.ColumnGroups.FindGroupByName("M" + i).HeaderText = lbName.Text;
                    RadGrid1.MasterTableView.Columns.FindByUniqueName("MM" + i).Display = cbM.Checked;
                    RadGrid1.MasterTableView.Columns.FindByUniqueName("15pM" + i).Display = cb15P.Checked;
                    RadGrid1.MasterTableView.Columns.FindByUniqueName("1THS1M" + i).Display = cb1THS1.Checked;
                    RadGrid1.MasterTableView.Columns.FindByUniqueName("1THS2M" + i).Display = cb1THS2.Checked;
                    RadGrid1.MasterTableView.Columns.FindByUniqueName("HKM" + i).Display = cbHocKy.Checked;

                    RadGrid1.MasterTableView.Columns.FindByUniqueName("MM" + i).ItemStyle.CssClass = RadGrid1.MasterTableView.Columns.FindByUniqueName("MM" + i).ItemStyle.CssClass.Replace("Diem10", "").Replace("DiemCD", "").Trim();
                    RadGrid1.MasterTableView.Columns.FindByUniqueName("MM" + i).ItemStyle.CssClass += " " + cellClass;

                    RadGrid1.MasterTableView.Columns.FindByUniqueName("15pM" + i).ItemStyle.CssClass = RadGrid1.MasterTableView.Columns.FindByUniqueName("15pM" + i).ItemStyle.CssClass.Replace("Diem10", "").Replace("DiemCD", "").Trim();
                    RadGrid1.MasterTableView.Columns.FindByUniqueName("15pM" + i).ItemStyle.CssClass += " " + cellClass;

                    RadGrid1.MasterTableView.Columns.FindByUniqueName("1THS1M" + i).ItemStyle.CssClass = RadGrid1.MasterTableView.Columns.FindByUniqueName("1THS1M" + i).ItemStyle.CssClass.Replace("Diem10", "").Replace("DiemCD", "").Trim();
                    RadGrid1.MasterTableView.Columns.FindByUniqueName("1THS1M" + i).ItemStyle.CssClass += " " + cellClass;

                    RadGrid1.MasterTableView.Columns.FindByUniqueName("1THS2M" + i).ItemStyle.CssClass = RadGrid1.MasterTableView.Columns.FindByUniqueName("1THS2M" + i).ItemStyle.CssClass.Replace("Diem10", "").Replace("DiemCD", "").Trim();
                    RadGrid1.MasterTableView.Columns.FindByUniqueName("1THS2M" + i).ItemStyle.CssClass += " " + cellClass;

                    RadGrid1.MasterTableView.Columns.FindByUniqueName("HKM" + i).ItemStyle.CssClass = RadGrid1.MasterTableView.Columns.FindByUniqueName("HKM" + i).ItemStyle.CssClass.Replace("Diem10", "").Replace("DiemCD", "").Trim();
                    RadGrid1.MasterTableView.Columns.FindByUniqueName("HKM" + i).ItemStyle.CssClass += " " + cellClass;
                    i++;
                }
            }
            for (int j = i; j <= 20; j++)
            {
                RadGrid1.MasterTableView.Columns.FindByUniqueName("MM" + j).Display = false;
                RadGrid1.MasterTableView.Columns.FindByUniqueName("15pM" + j).Display = false;
                RadGrid1.MasterTableView.Columns.FindByUniqueName("1THS1M" + j).Display = false;
                RadGrid1.MasterTableView.Columns.FindByUniqueName("1THS2M" + j).Display = false;
                RadGrid1.MasterTableView.Columns.FindByUniqueName("1THS2M" + j).Display = false;
                RadGrid1.MasterTableView.Columns.FindByUniqueName("HKM" + j).Display = false;
            }
        }
        protected void rcbKhoiHoc_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbLop.ClearSelection();
            rcbLop.Text = string.Empty;
            rcbLop.DataBind();
            rpMonHoc.DataBind();
            RadGrid1.Rebind();
        }
        protected void rcbLop_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rpMonHoc.DataBind();
            RadGrid1.Rebind();
        }
        protected void btSave_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }

            short? ma_khoi = localAPI.ConvertStringToShort(rcbKhoiHoc.SelectedValue);
            long? id_lop = localAPI.ConvertStringTolong(rcbLop.SelectedValue);
            NHAN_XET_HANG_NGAY detail = new NHAN_XET_HANG_NGAY();
            int success = 0;
            List<DIEM_CHI_TIET> lstDiemInLop = new List<DIEM_CHI_TIET>();
            lstDiemInLop = diemChiTietBO.getDiemChiTietByTruongKhoiLopAndMonAndCapAndHocKy(Convert.ToInt16(Sys_Ma_Nam_hoc), Sys_This_Truong.ID, ma_khoi, id_lop, Convert.ToInt16(Sys_Hoc_Ky), null, Sys_This_Cap_Hoc);

            GridItemCollection lstGrid = new GridItemCollection();
            if (RadGrid1.SelectedItems != null && RadGrid1.SelectedItems.Count > 0)
                lstGrid = RadGrid1.SelectedItems;
            else lstGrid = RadGrid1.Items;

            foreach (GridDataItem item in lstGrid)
            {
                ResultEntity res = new ResultEntity();
                res.Res = false;
                res.Msg = "Không có thay đổi";
                bool is_update_nx = false;
                long id_hs = Convert.ToInt64(item.GetDataKeyValue("ID_HS").ToString());
                #region Nhận xét

                long? id_nx = localAPI.ConvertStringTolong(item["ID"].Text);
                TextBox tbNoiDung = (TextBox)item.FindControl("tbNoiDung");
                HiddenField hdNoiDung = (HiddenField)item.FindControl("hdNoiDung");
                string str_noi_dung = tbNoiDung.Text.Trim();
                str_noi_dung = str_noi_dung.TrimEnd(',');
                string str_noi_dung_old = hdNoiDung.Value;
                if (id_nx != null)
                {
                    detail = nxhnBO.getNhanXetHangNgayByID(id_nx.Value);
                    detail.NOI_DUNG_NX = str_noi_dung;
                    if (str_noi_dung != str_noi_dung_old)
                    {
                        res = nxhnBO.update(detail, Sys_User.ID);
                        if (res.Res)
                        {
                            logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật NXHN HS " + detail.ID_HOC_SINH + ": " + str_noi_dung, Sys_User.ID, DateTime.Now);
                        }
                        is_update_nx = true;
                    }
                }
                else
                {
                    detail = new NHAN_XET_HANG_NGAY();
                    detail.NOI_DUNG_NX = str_noi_dung;
                    detail.ID_HOC_SINH = id_hs;
                    detail.ID_TRUONG = Sys_This_Truong.ID;
                    detail.ID_NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
                    detail.MA_KHOI = ma_khoi.Value;
                    detail.ID_LOP = id_lop.Value;
                    detail.NGAY_NX = DateTime.Now;
                    if (str_noi_dung != "")
                    {
                        res = nxhnBO.insert(detail, Sys_User.ID);
                        if (res.Res)
                        {
                            logUserBO.insert(Sys_This_Truong.ID, "INSERT", "Thêm NXHN HS " + detail.ID_HOC_SINH + ": " + str_noi_dung, Sys_User.ID, DateTime.Now);
                        }
                        is_update_nx = true;
                    }
                }
                #endregion
                #region Điểm
                #region Get control
                List<TextBox> lsttbM = new List<TextBox>();
                List<TextBox> lsttb15p = new List<TextBox>();
                List<TextBox> lsttb1THS1 = new List<TextBox>();
                List<TextBox> lsttb1THS2 = new List<TextBox>();
                List<TextBox> lsttbHK = new List<TextBox>();
                lsttbM.Add((TextBox)item.FindControl("tbMM1"));
                lsttbM.Add((TextBox)item.FindControl("tbMM2"));
                lsttbM.Add((TextBox)item.FindControl("tbMM3"));
                lsttbM.Add((TextBox)item.FindControl("tbMM4"));
                lsttbM.Add((TextBox)item.FindControl("tbMM5"));
                lsttbM.Add((TextBox)item.FindControl("tbMM6"));
                lsttbM.Add((TextBox)item.FindControl("tbMM7"));
                lsttbM.Add((TextBox)item.FindControl("tbMM8"));
                lsttbM.Add((TextBox)item.FindControl("tbMM9"));
                lsttbM.Add((TextBox)item.FindControl("tbMM10"));
                lsttbM.Add((TextBox)item.FindControl("tbMM11"));
                lsttbM.Add((TextBox)item.FindControl("tbMM12"));
                lsttbM.Add((TextBox)item.FindControl("tbMM13"));
                lsttbM.Add((TextBox)item.FindControl("tbMM14"));
                lsttbM.Add((TextBox)item.FindControl("tbMM15"));
                lsttbM.Add((TextBox)item.FindControl("tbMM16"));
                lsttbM.Add((TextBox)item.FindControl("tbMM17"));
                lsttbM.Add((TextBox)item.FindControl("tbMM18"));
                lsttbM.Add((TextBox)item.FindControl("tbMM19"));
                lsttbM.Add((TextBox)item.FindControl("tbMM20"));

                lsttb15p.Add((TextBox)item.FindControl("tb15pM1"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM2"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM3"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM4"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM5"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM6"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM7"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM8"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM9"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM10"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM11"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM12"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM13"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM14"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM15"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM16"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM17"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM18"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM19"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM20"));

                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M1"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M2"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M3"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M4"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M5"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M6"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M7"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M8"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M9"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M10"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M11"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M12"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M13"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M14"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M15"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M16"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M17"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M18"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M19"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M20"));

                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M1"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M2"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M3"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M4"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M5"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M6"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M7"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M8"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M9"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M10"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M11"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M12"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M13"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M14"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M15"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M16"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M17"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M18"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M19"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M20"));

                lsttbHK.Add((TextBox)item.FindControl("tbHKM1"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM2"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM3"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM4"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM5"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM6"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM7"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM8"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM9"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM10"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM11"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM12"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM13"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM14"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM15"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM16"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM17"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM18"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM19"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM20"));
                #endregion
                int i = 0;
                if ((res.Res && is_update_nx) || is_update_nx == false)
                {
                    foreach (RepeaterItem itemMon in rpMonHoc.Items)
                    {
                        #region Lặp môn
                        HiddenField hdIDMonTruong = (HiddenField)itemMon.FindControl("hdIDMonTruong");
                        long? id_mon_hoc_truong = localAPI.ConvertStringTolong(hdIDMonTruong.Value);
                        HiddenField hdKieuMon = (HiddenField)itemMon.FindControl("hdKieuMon");
                        bool is_tinh_diem = (hdKieuMon.Value.ToLower() == "false" || string.IsNullOrEmpty(hdKieuMon.Value) || hdKieuMon.Value == "0");
                        Label lbName = (Label)itemMon.FindControl("lbName");
                        CheckBox cbM = (CheckBox)itemMon.FindControl("cbM");
                        CheckBox cb15P = (CheckBox)itemMon.FindControl("cb15P");
                        CheckBox cb1THS1 = (CheckBox)itemMon.FindControl("cb1THS1");
                        CheckBox cb1THS2 = (CheckBox)itemMon.FindControl("cb1THS2");
                        CheckBox cbHocKy = (CheckBox)itemMon.FindControl("cbHocKy");
                        if ((cbM.Checked || cb15P.Checked || cb1THS1.Checked || cb1THS2.Checked || cbHocKy.Checked) && id_mon_hoc_truong != null)
                        {
                            if (!string.IsNullOrEmpty(lsttbM[i].Text) || !string.IsNullOrEmpty(lsttb15p[i].Text) || !string.IsNullOrEmpty(lsttb1THS1[i].Text) || !string.IsNullOrEmpty(lsttb1THS2[i].Text) || !string.IsNullOrEmpty(lsttbHK[i].Text))
                            {
                                #region Get detail điểm
                                DIEM_CHI_TIET diemDetail = new DIEM_CHI_TIET();
                                diemDetail = lstDiemInLop.FirstOrDefault(x => x.ID_HOC_SINH == id_hs && x.ID_MON_HOC_TRUONG == id_mon_hoc_truong.Value);
                                #endregion
                                if (diemDetail != null)
                                {
                                    #region Điểm M
                                    if (!string.IsNullOrEmpty(lsttbM[i].Text.Trim()))
                                    {
                                        Decimal? diemTmp = is_tinh_diem ? localAPI.ConvertStringToDecimal(lsttbM[i].Text.Trim()) : dataAccessAPI.ConvertCDToDecimal(lsttbM[i].Text.Trim());
                                        if (diemDetail.DIEM1 == null) diemDetail.DIEM1 = diemTmp;
                                        else if (diemDetail.DIEM2 == null) diemDetail.DIEM2 = diemTmp;
                                        else if (diemDetail.DIEM3 == null) diemDetail.DIEM3 = diemTmp;
                                        else if (diemDetail.DIEM4 == null) diemDetail.DIEM4 = diemTmp;
                                        else if (diemDetail.DIEM5 == null) diemDetail.DIEM5 = diemTmp;
                                    }
                                    #endregion
                                    #region Điểm 15p
                                    if (!string.IsNullOrEmpty(lsttb15p[i].Text.Trim()))
                                    {
                                        Decimal? diemTmp = is_tinh_diem ? localAPI.ConvertStringToDecimal(lsttb15p[i].Text.Trim()) : dataAccessAPI.ConvertCDToDecimal(lsttb15p[i].Text.Trim());
                                        if (diemDetail.DIEM6 == null) diemDetail.DIEM6 = diemTmp;
                                        else if (diemDetail.DIEM7 == null) diemDetail.DIEM7 = diemTmp;
                                        else if (diemDetail.DIEM8 == null) diemDetail.DIEM8 = diemTmp;
                                        else if (diemDetail.DIEM9 == null) diemDetail.DIEM9 = diemTmp;
                                        else if (diemDetail.DIEM10 == null) diemDetail.DIEM10 = diemTmp;
                                    }
                                    #endregion
                                    #region Điểm 1THS1
                                    if (!string.IsNullOrEmpty(lsttb1THS1[i].Text.Trim()))
                                    {
                                        Decimal? diemTmp = is_tinh_diem ? localAPI.ConvertStringToDecimal(lsttb1THS1[i].Text.Trim()) : dataAccessAPI.ConvertCDToDecimal(lsttb1THS1[i].Text.Trim());
                                        if (diemDetail.DIEM11 == null) diemDetail.DIEM11 = diemTmp;
                                        else if (diemDetail.DIEM12 == null) diemDetail.DIEM12 = diemTmp;
                                        else if (diemDetail.DIEM13 == null) diemDetail.DIEM13 = diemTmp;
                                        else if (diemDetail.DIEM14 == null) diemDetail.DIEM14 = diemTmp;
                                        else if (diemDetail.DIEM15 == null) diemDetail.DIEM15 = diemTmp;
                                    }
                                    #endregion
                                    #region Điểm 1THS2
                                    if (!string.IsNullOrEmpty(lsttb1THS2[i].Text.Trim()))
                                    {
                                        Decimal? diemTmp = is_tinh_diem ? localAPI.ConvertStringToDecimal(lsttb1THS2[i].Text.Trim()) : dataAccessAPI.ConvertCDToDecimal(lsttb1THS2[i].Text.Trim());
                                        if (diemDetail.DIEM16 == null) diemDetail.DIEM16 = diemTmp;
                                        else if (diemDetail.DIEM17 == null) diemDetail.DIEM17 = diemTmp;
                                        else if (diemDetail.DIEM18 == null) diemDetail.DIEM18 = diemTmp;
                                        else if (diemDetail.DIEM19 == null) diemDetail.DIEM19 = diemTmp;
                                        else if (diemDetail.DIEM20 == null) diemDetail.DIEM20 = diemTmp;
                                    }
                                    #endregion
                                    #region Điểm học kỳ
                                    if (!string.IsNullOrEmpty(lsttbHK[i].Text.Trim()))
                                    {
                                        Decimal? diemTmp = is_tinh_diem ? localAPI.ConvertStringToDecimal(lsttbHK[i].Text.Trim()) : dataAccessAPI.ConvertCDToDecimal(lsttbHK[i].Text.Trim());
                                        diemDetail.DIEM_HOC_KY = diemTmp;
                                    }
                                    #endregion
                                    //Lưu điểm
                                    res = diemChiTietBO.update(diemDetail, Sys_User);
                                    if (res.Res)
                                    {
                                        logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật điểm môn " + diemDetail.ID_MON_HOC + " HS " + detail.ID_HOC_SINH, Sys_User.ID, DateTime.Now);
                                    }
                                }
                            }
                        }
                        i++;
                        #endregion
                    }
                }
                #endregion
                if (res.Res) success++;
            }
            string strMsg = "";
            if (success == 0)
            {
                strMsg = "notification('error', 'Không có học sinh nào lưu thành công');";
            }
            if (success > 0)
            {
                strMsg += " notification('success', 'Có " + success + " học sinh lưu thành công.');";
            }

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
        }
        protected void btConfig_Click(object sender, EventArgs e)
        {
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
            string newName = "Nhap_diem_va_nhan_xet_hang_ngay.xlsx";

            List<ExcelHeaderEntity> lstHeader = new List<ExcelHeaderEntity>();
            List<ExcelEntity> lstColumn = new List<ExcelEntity>();
            lstHeader.Add(new ExcelHeaderEntity { name = "STT", colM = 1, rowM = 2, width = 10 });

            #region add column
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MA_HS"))
            {
                DataColumn col = new DataColumn("MA_HS");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Mã HS", colM = 1, rowM = 2, width = 18 });
                lstColumn.Add(new ExcelEntity { Name = "MA_HS", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TEN_HS"))
            {
                DataColumn col = new DataColumn("TEN_HS");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Họ tên", colM = 1, rowM = 2, width = 30 });
                lstColumn.Add(new ExcelEntity { Name = "TEN_HS", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "NGAY_SINH"))
            {
                DataColumn col = new DataColumn("NGAY_SINH");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Ngày sinh", colM = 1, rowM = 2, width = 15 });
                lstColumn.Add(new ExcelEntity { Name = "NGAY_SINH", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            #region "Mon 1"
            List<ExcelHeaderEntity> lstMon1 = new List<ExcelHeaderEntity>();
            int indexMon1 = 0;
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM1"))
                lstMon1.Add(new ExcelHeaderEntity { name = "1", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM1"))
                lstMon1.Add(new ExcelHeaderEntity { name = "2", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M1"))
                lstMon1.Add(new ExcelHeaderEntity { name = "3", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M1"))
                lstMon1.Add(new ExcelHeaderEntity { name = "4", colM = 1, rowM = 1, width = 10 });
            if (lstMon1 != null && lstMon1.Count > 0)
            {
                lstHeader.Add(new ExcelHeaderEntity { name = RadGrid1.MasterTableView.ColumnGroups.FindGroupByName("M1").HeaderText, colM = lstMon1.Count, rowM = 1, width = lstMon1.Count * 10 });
                indexMon1 = lstHeader.Count - 1;
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM1"))
            {
                DataColumn col = new DataColumn("MM1");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "M", colM = 1, rowM = 1, width = 10, parentIndex = indexMon1 });
                lstColumn.Add(new ExcelEntity { Name = "MM1", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM1"))
            {
                DataColumn col = new DataColumn("15pM1");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "15P", colM = 1, rowM = 1, width = 10, parentIndex = indexMon1 });
                lstColumn.Add(new ExcelEntity { Name = "15pM1", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M1"))
            {
                DataColumn col = new DataColumn("1THS1M1");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS1", colM = 1, rowM = 1, width = 10, parentIndex = indexMon1 });
                lstColumn.Add(new ExcelEntity { Name = "1THS1M1", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M1"))
            {
                DataColumn col = new DataColumn("1THS2M1");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS2", colM = 1, rowM = 1, width = 10, parentIndex = indexMon1 });
                lstColumn.Add(new ExcelEntity { Name = "1THS2M1", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            #endregion
            #region "Mon 2"
            List<ExcelHeaderEntity> lstMon2 = new List<ExcelHeaderEntity>();
            int indexMon2 = 0;
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM2"))
                lstMon2.Add(new ExcelHeaderEntity { name = "1", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM2"))
                lstMon2.Add(new ExcelHeaderEntity { name = "2", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M2"))
                lstMon2.Add(new ExcelHeaderEntity { name = "3", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M2"))
                lstMon2.Add(new ExcelHeaderEntity { name = "4", colM = 1, rowM = 1, width = 10 });
            if (lstMon2 != null && lstMon2.Count > 0)
            {
                lstHeader.Add(new ExcelHeaderEntity { name = RadGrid1.MasterTableView.ColumnGroups.FindGroupByName("M2").HeaderText, colM = lstMon2.Count, rowM = 1, width = lstMon2.Count * 10 });
                indexMon2 = lstHeader.Count - 1;
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM2"))
            {
                DataColumn col = new DataColumn("MM2");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "M", colM = 1, rowM = 1, width = 10, parentIndex = indexMon2 });
                lstColumn.Add(new ExcelEntity { Name = "MM2", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM2"))
            {
                DataColumn col = new DataColumn("15pM2");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "15P", colM = 1, rowM = 1, width = 10, parentIndex = indexMon2 });
                lstColumn.Add(new ExcelEntity { Name = "15pM2", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M2"))
            {
                DataColumn col = new DataColumn("1THS1M2");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS1", colM = 1, rowM = 1, width = 10, parentIndex = indexMon2 });
                lstColumn.Add(new ExcelEntity { Name = "1THS1M2", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M2"))
            {
                DataColumn col = new DataColumn("1THS2M2");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS2", colM = 1, rowM = 1, width = 10, parentIndex = indexMon2 });
                lstColumn.Add(new ExcelEntity { Name = "1THS2M2", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            #endregion
            #region "Mon 3"
            List<ExcelHeaderEntity> lstMon3 = new List<ExcelHeaderEntity>();
            int indexMon3 = 0;
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM3"))
                lstMon3.Add(new ExcelHeaderEntity { name = "1", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM3"))
                lstMon3.Add(new ExcelHeaderEntity { name = "2", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M3"))
                lstMon3.Add(new ExcelHeaderEntity { name = "3", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M3"))
                lstMon3.Add(new ExcelHeaderEntity { name = "4", colM = 1, rowM = 1, width = 10 });
            if (lstMon3 != null && lstMon3.Count > 0)
            {
                lstHeader.Add(new ExcelHeaderEntity { name = RadGrid1.MasterTableView.ColumnGroups.FindGroupByName("M3").HeaderText, colM = lstMon3.Count, rowM = 1, width = lstMon3.Count * 10 });
                indexMon3 = lstHeader.Count - 1;
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM3"))
            {
                DataColumn col = new DataColumn("MM3");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "M", colM = 1, rowM = 1, width = 10, parentIndex = indexMon3 });
                lstColumn.Add(new ExcelEntity { Name = "MM3", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM3"))
            {
                DataColumn col = new DataColumn("15pM3");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "15P", colM = 1, rowM = 1, width = 10, parentIndex = indexMon3 });
                lstColumn.Add(new ExcelEntity { Name = "15pM3", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M3"))
            {
                DataColumn col = new DataColumn("1THS1M3");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS1", colM = 1, rowM = 1, width = 10, parentIndex = indexMon3 });
                lstColumn.Add(new ExcelEntity { Name = "1THS1M3", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M3"))
            {
                DataColumn col = new DataColumn("1THS2M3");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS2", colM = 1, rowM = 1, width = 10, parentIndex = indexMon3 });
                lstColumn.Add(new ExcelEntity { Name = "1THS2M3", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            #endregion
            #region "Mon 4"
            List<ExcelHeaderEntity> lstMon4 = new List<ExcelHeaderEntity>();
            int indexMon4 = 0;
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM4"))
                lstMon4.Add(new ExcelHeaderEntity { name = "1", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM4"))
                lstMon4.Add(new ExcelHeaderEntity { name = "2", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M4"))
                lstMon4.Add(new ExcelHeaderEntity { name = "3", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M4"))
                lstMon4.Add(new ExcelHeaderEntity { name = "4", colM = 1, rowM = 1, width = 10 });
            if (lstMon4 != null && lstMon4.Count > 0)
            {
                lstHeader.Add(new ExcelHeaderEntity { name = RadGrid1.MasterTableView.ColumnGroups.FindGroupByName("M4").HeaderText, colM = lstMon4.Count, rowM = 1, width = lstMon4.Count * 10 });
                indexMon4 = lstHeader.Count - 1;
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM4"))
            {
                DataColumn col = new DataColumn("MM4");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "M", colM = 1, rowM = 1, width = 10, parentIndex = indexMon4 });
                lstColumn.Add(new ExcelEntity { Name = "MM4", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM4"))
            {
                DataColumn col = new DataColumn("15pM4");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "15P", colM = 1, rowM = 1, width = 10, parentIndex = indexMon4 });
                lstColumn.Add(new ExcelEntity { Name = "15pM4", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M4"))
            {
                DataColumn col = new DataColumn("1THS1M4");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS1", colM = 1, rowM = 1, width = 10, parentIndex = indexMon4 });
                lstColumn.Add(new ExcelEntity { Name = "1THS1M4", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M4"))
            {
                DataColumn col = new DataColumn("1THS2M4");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS2", colM = 1, rowM = 1, width = 10, parentIndex = indexMon4 });
                lstColumn.Add(new ExcelEntity { Name = "1THS2M4", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            #endregion
            #region "Mon 5"
            List<ExcelHeaderEntity> lstMon5 = new List<ExcelHeaderEntity>();
            int indexMon5 = 0;
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM5"))
                lstMon5.Add(new ExcelHeaderEntity { name = "1", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM5"))
                lstMon5.Add(new ExcelHeaderEntity { name = "2", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M5"))
                lstMon5.Add(new ExcelHeaderEntity { name = "3", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M5"))
                lstMon5.Add(new ExcelHeaderEntity { name = "4", colM = 1, rowM = 1, width = 10 });
            if (lstMon5 != null && lstMon5.Count > 0)
            {
                lstHeader.Add(new ExcelHeaderEntity { name = RadGrid1.MasterTableView.ColumnGroups.FindGroupByName("M5").HeaderText, colM = lstMon5.Count, rowM = 1, width = lstMon5.Count * 10 });
                indexMon5 = lstHeader.Count - 1;
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM5"))
            {
                DataColumn col = new DataColumn("MM5");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "M", colM = 1, rowM = 1, width = 10, parentIndex = indexMon5 });
                lstColumn.Add(new ExcelEntity { Name = "MM5", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM5"))
            {
                DataColumn col = new DataColumn("15pM5");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "15P", colM = 1, rowM = 1, width = 10, parentIndex = indexMon5 });
                lstColumn.Add(new ExcelEntity { Name = "15pM5", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M5"))
            {
                DataColumn col = new DataColumn("1THS1M5");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS1", colM = 1, rowM = 1, width = 10, parentIndex = indexMon5 });
                lstColumn.Add(new ExcelEntity { Name = "1THS1M5", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M5"))
            {
                DataColumn col = new DataColumn("1THS2M5");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS2", colM = 1, rowM = 1, width = 10, parentIndex = indexMon5 });
                lstColumn.Add(new ExcelEntity { Name = "1THS2M5", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            #endregion
            #region "Mon 6"
            List<ExcelHeaderEntity> lstMon6 = new List<ExcelHeaderEntity>();
            int indexMon6 = 0;
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM6"))
                lstMon6.Add(new ExcelHeaderEntity { name = "1", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM6"))
                lstMon6.Add(new ExcelHeaderEntity { name = "2", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M6"))
                lstMon6.Add(new ExcelHeaderEntity { name = "3", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M6"))
                lstMon6.Add(new ExcelHeaderEntity { name = "4", colM = 1, rowM = 1, width = 10 });
            if (lstMon6 != null && lstMon6.Count > 0)
            {
                lstHeader.Add(new ExcelHeaderEntity { name = RadGrid1.MasterTableView.ColumnGroups.FindGroupByName("M6").HeaderText, colM = lstMon6.Count, rowM = 1, width = lstMon6.Count * 10 });
                indexMon6 = lstHeader.Count - 1;
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM6"))
            {
                DataColumn col = new DataColumn("MM6");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "M", colM = 1, rowM = 1, width = 10, parentIndex = indexMon6 });
                lstColumn.Add(new ExcelEntity { Name = "MM6", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM6"))
            {
                DataColumn col = new DataColumn("15pM6");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "15P", colM = 1, rowM = 1, width = 10, parentIndex = indexMon6 });
                lstColumn.Add(new ExcelEntity { Name = "15pM6", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M6"))
            {
                DataColumn col = new DataColumn("1THS1M6");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS1", colM = 1, rowM = 1, width = 10, parentIndex = indexMon6 });
                lstColumn.Add(new ExcelEntity { Name = "1THS1M6", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M6"))
            {
                DataColumn col = new DataColumn("1THS2M6");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS2", colM = 1, rowM = 1, width = 10, parentIndex = indexMon6 });
                lstColumn.Add(new ExcelEntity { Name = "1THS2M6", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            #endregion
            #region "Mon 7"
            List<ExcelHeaderEntity> lstMon7 = new List<ExcelHeaderEntity>();
            int indexMon7 = 0;
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM7"))
                lstMon7.Add(new ExcelHeaderEntity { name = "1", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM7"))
                lstMon7.Add(new ExcelHeaderEntity { name = "2", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M7"))
                lstMon7.Add(new ExcelHeaderEntity { name = "3", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M7"))
                lstMon7.Add(new ExcelHeaderEntity { name = "4", colM = 1, rowM = 1, width = 10 });
            if (lstMon7 != null && lstMon7.Count > 0)
            {
                lstHeader.Add(new ExcelHeaderEntity { name = RadGrid1.MasterTableView.ColumnGroups.FindGroupByName("M7").HeaderText, colM = lstMon7.Count, rowM = 1, width = lstMon7.Count * 10 });
                indexMon7 = lstHeader.Count - 1;
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM7"))
            {
                DataColumn col = new DataColumn("MM7");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "M", colM = 1, rowM = 1, width = 10, parentIndex = indexMon7 });
                lstColumn.Add(new ExcelEntity { Name = "MM7", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM7"))
            {
                DataColumn col = new DataColumn("15pM7");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "15P", colM = 1, rowM = 1, width = 10, parentIndex = indexMon7 });
                lstColumn.Add(new ExcelEntity { Name = "15pM7", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M7"))
            {
                DataColumn col = new DataColumn("1THS1M7");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS1", colM = 1, rowM = 1, width = 10, parentIndex = indexMon7 });
                lstColumn.Add(new ExcelEntity { Name = "1THS1M7", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M7"))
            {
                DataColumn col = new DataColumn("1THS2M7");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS2", colM = 1, rowM = 1, width = 10, parentIndex = indexMon7 });
                lstColumn.Add(new ExcelEntity { Name = "1THS2M7", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            #endregion
            #region "Mon 8"
            List<ExcelHeaderEntity> lstMon8 = new List<ExcelHeaderEntity>();
            int indexMon8 = 0;
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM8"))
                lstMon8.Add(new ExcelHeaderEntity { name = "1", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM8"))
                lstMon8.Add(new ExcelHeaderEntity { name = "2", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M8"))
                lstMon8.Add(new ExcelHeaderEntity { name = "3", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M8"))
                lstMon8.Add(new ExcelHeaderEntity { name = "4", colM = 1, rowM = 1, width = 10 });
            if (lstMon8 != null && lstMon8.Count > 0)
            {
                lstHeader.Add(new ExcelHeaderEntity { name = RadGrid1.MasterTableView.ColumnGroups.FindGroupByName("M8").HeaderText, colM = lstMon8.Count, rowM = 1, width = lstMon8.Count * 10 });
                indexMon8 = lstHeader.Count - 1;
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM8"))
            {
                DataColumn col = new DataColumn("MM8");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "M", colM = 1, rowM = 1, width = 10, parentIndex = indexMon8 });
                lstColumn.Add(new ExcelEntity { Name = "MM8", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM8"))
            {
                DataColumn col = new DataColumn("15pM8");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "15P", colM = 1, rowM = 1, width = 10, parentIndex = indexMon8 });
                lstColumn.Add(new ExcelEntity { Name = "15pM8", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M8"))
            {
                DataColumn col = new DataColumn("1THS1M8");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS1", colM = 1, rowM = 1, width = 10, parentIndex = indexMon8 });
                lstColumn.Add(new ExcelEntity { Name = "1THS1M8", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M8"))
            {
                DataColumn col = new DataColumn("1THS2M8");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS2", colM = 1, rowM = 1, width = 10, parentIndex = indexMon8 });
                lstColumn.Add(new ExcelEntity { Name = "1THS2M8", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            #endregion
            #region "Mon 9"
            List<ExcelHeaderEntity> lstMon9 = new List<ExcelHeaderEntity>();
            int indexMon9 = 0;
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM9"))
                lstMon9.Add(new ExcelHeaderEntity { name = "1", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM9"))
                lstMon9.Add(new ExcelHeaderEntity { name = "2", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M9"))
                lstMon9.Add(new ExcelHeaderEntity { name = "3", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M9"))
                lstMon9.Add(new ExcelHeaderEntity { name = "4", colM = 1, rowM = 1, width = 10 });
            if (lstMon9 != null && lstMon9.Count > 0)
            {
                lstHeader.Add(new ExcelHeaderEntity { name = RadGrid1.MasterTableView.ColumnGroups.FindGroupByName("M9").HeaderText, colM = lstMon9.Count, rowM = 1, width = lstMon9.Count * 10 });
                indexMon9 = lstHeader.Count - 1;
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM9"))
            {
                DataColumn col = new DataColumn("MM9");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "M", colM = 1, rowM = 1, width = 10, parentIndex = indexMon9 });
                lstColumn.Add(new ExcelEntity { Name = "MM9", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM9"))
            {
                DataColumn col = new DataColumn("15pM9");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "15P", colM = 1, rowM = 1, width = 10, parentIndex = indexMon9 });
                lstColumn.Add(new ExcelEntity { Name = "15pM9", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M9"))
            {
                DataColumn col = new DataColumn("1THS1M9");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS1", colM = 1, rowM = 1, width = 10, parentIndex = indexMon9 });
                lstColumn.Add(new ExcelEntity { Name = "1THS1M9", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M9"))
            {
                DataColumn col = new DataColumn("1THS2M9");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS2", colM = 1, rowM = 1, width = 10, parentIndex = indexMon9 });
                lstColumn.Add(new ExcelEntity { Name = "1THS2M9", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            #endregion
            #region "Mon 10"
            List<ExcelHeaderEntity> lstMon10 = new List<ExcelHeaderEntity>();
            int indexMon10 = 0;
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM10"))
                lstMon10.Add(new ExcelHeaderEntity { name = "1", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM10"))
                lstMon10.Add(new ExcelHeaderEntity { name = "2", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M10"))
                lstMon10.Add(new ExcelHeaderEntity { name = "3", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M10"))
                lstMon10.Add(new ExcelHeaderEntity { name = "4", colM = 1, rowM = 1, width = 10 });
            if (lstMon10 != null && lstMon10.Count > 0)
            {
                lstHeader.Add(new ExcelHeaderEntity { name = RadGrid1.MasterTableView.ColumnGroups.FindGroupByName("M10").HeaderText, colM = lstMon10.Count, rowM = 1, width = lstMon10.Count * 10 });
                indexMon10 = lstHeader.Count - 1;
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM10"))
            {
                DataColumn col = new DataColumn("MM10");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "M", colM = 1, rowM = 1, width = 10, parentIndex = indexMon10 });
                lstColumn.Add(new ExcelEntity { Name = "MM10", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM10"))
            {
                DataColumn col = new DataColumn("15pM10");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "15P", colM = 1, rowM = 1, width = 10, parentIndex = indexMon10 });
                lstColumn.Add(new ExcelEntity { Name = "15pM10", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M10"))
            {
                DataColumn col = new DataColumn("1THS1M10");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS1", colM = 1, rowM = 1, width = 10, parentIndex = indexMon10 });
                lstColumn.Add(new ExcelEntity { Name = "1THS1M10", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M10"))
            {
                DataColumn col = new DataColumn("1THS2M10");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS2", colM = 1, rowM = 1, width = 10, parentIndex = indexMon10 });
                lstColumn.Add(new ExcelEntity { Name = "1THS2M10", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            #endregion
            #region "Mon 11"
            List<ExcelHeaderEntity> lstMon11 = new List<ExcelHeaderEntity>();
            int indexMon11 = 0;
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM11"))
                lstMon11.Add(new ExcelHeaderEntity { name = "1", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM11"))
                lstMon11.Add(new ExcelHeaderEntity { name = "2", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M11"))
                lstMon11.Add(new ExcelHeaderEntity { name = "3", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M11"))
                lstMon11.Add(new ExcelHeaderEntity { name = "4", colM = 1, rowM = 1, width = 10 });
            if (lstMon11 != null && lstMon11.Count > 0)
            {
                lstHeader.Add(new ExcelHeaderEntity { name = RadGrid1.MasterTableView.ColumnGroups.FindGroupByName("M11").HeaderText, colM = lstMon11.Count, rowM = 1, width = lstMon11.Count * 10 });
                indexMon11 = lstHeader.Count - 1;
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM11"))
            {
                DataColumn col = new DataColumn("MM11");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "M", colM = 1, rowM = 1, width = 10, parentIndex = indexMon11 });
                lstColumn.Add(new ExcelEntity { Name = "MM11", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM11"))
            {
                DataColumn col = new DataColumn("15pM11");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "15P", colM = 1, rowM = 1, width = 10, parentIndex = indexMon11 });
                lstColumn.Add(new ExcelEntity { Name = "15pM11", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M11"))
            {
                DataColumn col = new DataColumn("1THS1M11");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS1", colM = 1, rowM = 1, width = 10, parentIndex = indexMon11 });
                lstColumn.Add(new ExcelEntity { Name = "1THS1M11", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M11"))
            {
                DataColumn col = new DataColumn("1THS2M11");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS2", colM = 1, rowM = 1, width = 10, parentIndex = indexMon11 });
                lstColumn.Add(new ExcelEntity { Name = "1THS2M11", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            #endregion
            #region "Mon 12"
            List<ExcelHeaderEntity> lstMon12 = new List<ExcelHeaderEntity>();
            int indexMon12 = 0;
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM12"))
                lstMon12.Add(new ExcelHeaderEntity { name = "1", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM12"))
                lstMon12.Add(new ExcelHeaderEntity { name = "2", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M12"))
                lstMon12.Add(new ExcelHeaderEntity { name = "3", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M12"))
                lstMon12.Add(new ExcelHeaderEntity { name = "4", colM = 1, rowM = 1, width = 10 });
            if (lstMon12 != null && lstMon12.Count > 0)
            {
                lstHeader.Add(new ExcelHeaderEntity { name = RadGrid1.MasterTableView.ColumnGroups.FindGroupByName("M12").HeaderText, colM = lstMon12.Count, rowM = 1, width = lstMon12.Count * 10 });
                indexMon12 = lstHeader.Count - 1;
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM12"))
            {
                DataColumn col = new DataColumn("MM12");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "M", colM = 1, rowM = 1, width = 10, parentIndex = indexMon12 });
                lstColumn.Add(new ExcelEntity { Name = "MM12", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM12"))
            {
                DataColumn col = new DataColumn("15pM12");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "15P", colM = 1, rowM = 1, width = 10, parentIndex = indexMon12 });
                lstColumn.Add(new ExcelEntity { Name = "15pM12", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M12"))
            {
                DataColumn col = new DataColumn("1THS1M12");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS1", colM = 1, rowM = 1, width = 10, parentIndex = indexMon12 });
                lstColumn.Add(new ExcelEntity { Name = "1THS1M12", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M12"))
            {
                DataColumn col = new DataColumn("1THS2M12");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS2", colM = 1, rowM = 1, width = 10, parentIndex = indexMon12 });
                lstColumn.Add(new ExcelEntity { Name = "1THS2M12", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            #endregion
            #region "Mon 13"
            List<ExcelHeaderEntity> lstMon13 = new List<ExcelHeaderEntity>();
            int indexMon13 = 0;
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM13"))
                lstMon13.Add(new ExcelHeaderEntity { name = "1", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM13"))
                lstMon13.Add(new ExcelHeaderEntity { name = "2", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M13"))
                lstMon13.Add(new ExcelHeaderEntity { name = "3", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M13"))
                lstMon13.Add(new ExcelHeaderEntity { name = "4", colM = 1, rowM = 1, width = 10 });
            if (lstMon13 != null && lstMon13.Count > 0)
            {
                lstHeader.Add(new ExcelHeaderEntity { name = RadGrid1.MasterTableView.ColumnGroups.FindGroupByName("M13").HeaderText, colM = lstMon13.Count, rowM = 1, width = lstMon13.Count * 10 });
                indexMon13 = lstHeader.Count - 1;
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM13"))
            {
                DataColumn col = new DataColumn("MM13");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "M", colM = 1, rowM = 1, width = 10, parentIndex = indexMon13 });
                lstColumn.Add(new ExcelEntity { Name = "MM13", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM13"))
            {
                DataColumn col = new DataColumn("15pM13");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "15P", colM = 1, rowM = 1, width = 10, parentIndex = indexMon13 });
                lstColumn.Add(new ExcelEntity { Name = "15pM13", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M13"))
            {
                DataColumn col = new DataColumn("1THS1M13");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS1", colM = 1, rowM = 1, width = 10, parentIndex = indexMon13 });
                lstColumn.Add(new ExcelEntity { Name = "1THS1M13", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M13"))
            {
                DataColumn col = new DataColumn("1THS2M13");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS2", colM = 1, rowM = 1, width = 10, parentIndex = indexMon13 });
                lstColumn.Add(new ExcelEntity { Name = "1THS2M13", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            #endregion
            #region "Mon 14"
            List<ExcelHeaderEntity> lstMon14 = new List<ExcelHeaderEntity>();
            int indexMon14 = 0;
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM14"))
                lstMon14.Add(new ExcelHeaderEntity { name = "1", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM14"))
                lstMon14.Add(new ExcelHeaderEntity { name = "2", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M14"))
                lstMon14.Add(new ExcelHeaderEntity { name = "3", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M14"))
                lstMon14.Add(new ExcelHeaderEntity { name = "4", colM = 1, rowM = 1, width = 10 });
            if (lstMon14 != null && lstMon14.Count > 0)
            {
                lstHeader.Add(new ExcelHeaderEntity { name = RadGrid1.MasterTableView.ColumnGroups.FindGroupByName("M14").HeaderText, colM = lstMon14.Count, rowM = 1, width = lstMon14.Count * 10 });
                indexMon14 = lstHeader.Count - 1;
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM14"))
            {
                DataColumn col = new DataColumn("MM14");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "M", colM = 1, rowM = 1, width = 10, parentIndex = indexMon14 });
                lstColumn.Add(new ExcelEntity { Name = "MM14", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM14"))
            {
                DataColumn col = new DataColumn("15pM14");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "15P", colM = 1, rowM = 1, width = 10, parentIndex = indexMon14 });
                lstColumn.Add(new ExcelEntity { Name = "15pM14", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M14"))
            {
                DataColumn col = new DataColumn("1THS1M14");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS1", colM = 1, rowM = 1, width = 10, parentIndex = indexMon14 });
                lstColumn.Add(new ExcelEntity { Name = "1THS1M14", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M14"))
            {
                DataColumn col = new DataColumn("1THS2M14");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS2", colM = 1, rowM = 1, width = 10, parentIndex = indexMon14 });
                lstColumn.Add(new ExcelEntity { Name = "1THS2M14", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            #endregion
            #region "Mon 15"
            List<ExcelHeaderEntity> lstMon15 = new List<ExcelHeaderEntity>();
            int indexMon15 = 0;
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM15"))
                lstMon15.Add(new ExcelHeaderEntity { name = "1", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM15"))
                lstMon15.Add(new ExcelHeaderEntity { name = "2", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M15"))
                lstMon15.Add(new ExcelHeaderEntity { name = "3", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M15"))
                lstMon15.Add(new ExcelHeaderEntity { name = "4", colM = 1, rowM = 1, width = 10 });
            if (lstMon15 != null && lstMon15.Count > 0)
            {
                lstHeader.Add(new ExcelHeaderEntity { name = RadGrid1.MasterTableView.ColumnGroups.FindGroupByName("M15").HeaderText, colM = lstMon15.Count, rowM = 1, width = lstMon15.Count * 10 });
                indexMon15 = lstHeader.Count - 1;
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM15"))
            {
                DataColumn col = new DataColumn("MM15");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "M", colM = 1, rowM = 1, width = 10, parentIndex = indexMon15 });
                lstColumn.Add(new ExcelEntity { Name = "MM15", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM15"))
            {
                DataColumn col = new DataColumn("15pM15");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "15P", colM = 1, rowM = 1, width = 10, parentIndex = indexMon15 });
                lstColumn.Add(new ExcelEntity { Name = "15pM15", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M15"))
            {
                DataColumn col = new DataColumn("1THS1M15");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS1", colM = 1, rowM = 1, width = 10, parentIndex = indexMon15 });
                lstColumn.Add(new ExcelEntity { Name = "1THS1M15", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M15"))
            {
                DataColumn col = new DataColumn("1THS2M15");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS2", colM = 1, rowM = 1, width = 10, parentIndex = indexMon15 });
                lstColumn.Add(new ExcelEntity { Name = "1THS2M15", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            #endregion
            #region "Mon 16"
            List<ExcelHeaderEntity> lstMon16 = new List<ExcelHeaderEntity>();
            int indexMon16 = 0;
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM16"))
                lstMon16.Add(new ExcelHeaderEntity { name = "1", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM16"))
                lstMon16.Add(new ExcelHeaderEntity { name = "2", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M16"))
                lstMon16.Add(new ExcelHeaderEntity { name = "3", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M16"))
                lstMon16.Add(new ExcelHeaderEntity { name = "4", colM = 1, rowM = 1, width = 10 });
            if (lstMon16 != null && lstMon16.Count > 0)
            {
                lstHeader.Add(new ExcelHeaderEntity { name = RadGrid1.MasterTableView.ColumnGroups.FindGroupByName("M16").HeaderText, colM = lstMon16.Count, rowM = 1, width = lstMon16.Count * 10 });
                indexMon16 = lstHeader.Count - 1;
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM16"))
            {
                DataColumn col = new DataColumn("MM16");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "M", colM = 1, rowM = 1, width = 10, parentIndex = indexMon16 });
                lstColumn.Add(new ExcelEntity { Name = "MM16", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM16"))
            {
                DataColumn col = new DataColumn("15pM16");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "15P", colM = 1, rowM = 1, width = 10, parentIndex = indexMon16 });
                lstColumn.Add(new ExcelEntity { Name = "15pM16", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M16"))
            {
                DataColumn col = new DataColumn("1THS1M16");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS1", colM = 1, rowM = 1, width = 10, parentIndex = indexMon16 });
                lstColumn.Add(new ExcelEntity { Name = "1THS1M16", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M16"))
            {
                DataColumn col = new DataColumn("1THS2M16");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS2", colM = 1, rowM = 1, width = 10, parentIndex = indexMon16 });
                lstColumn.Add(new ExcelEntity { Name = "1THS2M16", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            #endregion
            #region "Mon 17"
            List<ExcelHeaderEntity> lstMon17 = new List<ExcelHeaderEntity>();
            int indexMon17 = 0;
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM17"))
                lstMon17.Add(new ExcelHeaderEntity { name = "1", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM17"))
                lstMon17.Add(new ExcelHeaderEntity { name = "2", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M17"))
                lstMon17.Add(new ExcelHeaderEntity { name = "3", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M17"))
                lstMon17.Add(new ExcelHeaderEntity { name = "4", colM = 1, rowM = 1, width = 10 });
            if (lstMon17 != null && lstMon17.Count > 0)
            {
                lstHeader.Add(new ExcelHeaderEntity { name = RadGrid1.MasterTableView.ColumnGroups.FindGroupByName("M17").HeaderText, colM = lstMon17.Count, rowM = 1, width = lstMon17.Count * 10 });
                indexMon17 = lstHeader.Count - 1;
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM17"))
            {
                DataColumn col = new DataColumn("MM17");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "M", colM = 1, rowM = 1, width = 10, parentIndex = indexMon17 });
                lstColumn.Add(new ExcelEntity { Name = "MM17", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM17"))
            {
                DataColumn col = new DataColumn("15pM17");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "15P", colM = 1, rowM = 1, width = 10, parentIndex = indexMon17 });
                lstColumn.Add(new ExcelEntity { Name = "15pM17", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M17"))
            {
                DataColumn col = new DataColumn("1THS1M17");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS1", colM = 1, rowM = 1, width = 10, parentIndex = indexMon17 });
                lstColumn.Add(new ExcelEntity { Name = "1THS1M17", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M17"))
            {
                DataColumn col = new DataColumn("1THS2M17");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS2", colM = 1, rowM = 1, width = 10, parentIndex = indexMon17 });
                lstColumn.Add(new ExcelEntity { Name = "1THS2M17", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            #endregion
            #region "Mon 18"
            List<ExcelHeaderEntity> lstMon18 = new List<ExcelHeaderEntity>();
            int indexMon18 = 0;
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM18"))
                lstMon18.Add(new ExcelHeaderEntity { name = "1", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM18"))
                lstMon18.Add(new ExcelHeaderEntity { name = "2", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M18"))
                lstMon18.Add(new ExcelHeaderEntity { name = "3", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M18"))
                lstMon18.Add(new ExcelHeaderEntity { name = "4", colM = 1, rowM = 1, width = 10 });
            if (lstMon18 != null && lstMon18.Count > 0)
            {
                lstHeader.Add(new ExcelHeaderEntity { name = RadGrid1.MasterTableView.ColumnGroups.FindGroupByName("M18").HeaderText, colM = lstMon18.Count, rowM = 1, width = lstMon18.Count * 10 });
                indexMon18 = lstHeader.Count - 1;
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM18"))
            {
                DataColumn col = new DataColumn("MM18");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "M", colM = 1, rowM = 1, width = 10, parentIndex = indexMon18 });
                lstColumn.Add(new ExcelEntity { Name = "MM18", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM18"))
            {
                DataColumn col = new DataColumn("15pM18");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "15P", colM = 1, rowM = 1, width = 10, parentIndex = indexMon18 });
                lstColumn.Add(new ExcelEntity { Name = "15pM18", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M18"))
            {
                DataColumn col = new DataColumn("1THS1M18");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS1", colM = 1, rowM = 1, width = 10, parentIndex = indexMon18 });
                lstColumn.Add(new ExcelEntity { Name = "1THS1M18", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M18"))
            {
                DataColumn col = new DataColumn("1THS2M18");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS2", colM = 1, rowM = 1, width = 10, parentIndex = indexMon18 });
                lstColumn.Add(new ExcelEntity { Name = "1THS2M18", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            #endregion
            #region "Mon 19"
            List<ExcelHeaderEntity> lstMon19 = new List<ExcelHeaderEntity>();
            int indexMon19 = 0;
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM19"))
                lstMon19.Add(new ExcelHeaderEntity { name = "1", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM19"))
                lstMon19.Add(new ExcelHeaderEntity { name = "2", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M19"))
                lstMon19.Add(new ExcelHeaderEntity { name = "3", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M19"))
                lstMon19.Add(new ExcelHeaderEntity { name = "4", colM = 1, rowM = 1, width = 10 });
            if (lstMon19 != null && lstMon19.Count > 0)
            {
                lstHeader.Add(new ExcelHeaderEntity { name = RadGrid1.MasterTableView.ColumnGroups.FindGroupByName("M19").HeaderText, colM = lstMon19.Count, rowM = 1, width = lstMon19.Count * 10 });
                indexMon19 = lstHeader.Count - 1;
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM19"))
            {
                DataColumn col = new DataColumn("MM19");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "M", colM = 1, rowM = 1, width = 10, parentIndex = indexMon19 });
                lstColumn.Add(new ExcelEntity { Name = "MM19", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM19"))
            {
                DataColumn col = new DataColumn("15pM19");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "15P", colM = 1, rowM = 1, width = 10, parentIndex = indexMon19 });
                lstColumn.Add(new ExcelEntity { Name = "15pM19", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M19"))
            {
                DataColumn col = new DataColumn("1THS1M19");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS1", colM = 1, rowM = 1, width = 10, parentIndex = indexMon19 });
                lstColumn.Add(new ExcelEntity { Name = "1THS1M19", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M19"))
            {
                DataColumn col = new DataColumn("1THS2M19");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS2", colM = 1, rowM = 1, width = 10, parentIndex = indexMon19 });
                lstColumn.Add(new ExcelEntity { Name = "1THS2M19", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            #endregion
            #region "Mon 20"
            List<ExcelHeaderEntity> lstMon20 = new List<ExcelHeaderEntity>();
            int indexMon20 = 0;
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM20"))
                lstMon20.Add(new ExcelHeaderEntity { name = "1", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM20"))
                lstMon20.Add(new ExcelHeaderEntity { name = "2", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M20"))
                lstMon20.Add(new ExcelHeaderEntity { name = "3", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M20"))
                lstMon20.Add(new ExcelHeaderEntity { name = "4", colM = 1, rowM = 1, width = 10 });
            if (lstMon20 != null && lstMon20.Count > 0)
            {
                lstHeader.Add(new ExcelHeaderEntity { name = RadGrid1.MasterTableView.ColumnGroups.FindGroupByName("M20").HeaderText, colM = lstMon20.Count, rowM = 1, width = lstMon20.Count * 10 });
                indexMon20 = lstHeader.Count - 1;
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MM20"))
            {
                DataColumn col = new DataColumn("MM20");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "M", colM = 1, rowM = 1, width = 10, parentIndex = indexMon20 });
                lstColumn.Add(new ExcelEntity { Name = "MM20", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "15pM20"))
            {
                DataColumn col = new DataColumn("15pM20");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "15P", colM = 1, rowM = 1, width = 10, parentIndex = indexMon20 });
                lstColumn.Add(new ExcelEntity { Name = "15pM20", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS1M20"))
            {
                DataColumn col = new DataColumn("1THS1M20");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS1", colM = 1, rowM = 1, width = 10, parentIndex = indexMon20 });
                lstColumn.Add(new ExcelEntity { Name = "1THS1M20", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "1THS2M20"))
            {
                DataColumn col = new DataColumn("1THS2M20");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "1T-HS2", colM = 1, rowM = 1, width = 10, parentIndex = indexMon20 });
                lstColumn.Add(new ExcelEntity { Name = "1THS2M20", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            #endregion
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "NOI_DUNG_NX"))
            {
                DataColumn col = new DataColumn("NOI_DUNG_NX");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Nội dung nhận xét", colM = 1, rowM = 2, width = 60 });
                lstColumn.Add(new ExcelEntity { Name = "NOI_DUNG_NX", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            #endregion
            foreach (GridDataItem item in RadGrid1.Items)
            {
                #region "get control"
                #region "điểm miệng"
                TextBox tbMM1 = (TextBox)item.FindControl("tbMM1");
                TextBox tbMM2 = (TextBox)item.FindControl("tbMM2");
                TextBox tbMM3 = (TextBox)item.FindControl("tbMM3");
                TextBox tbMM4 = (TextBox)item.FindControl("tbMM4");
                TextBox tbMM5 = (TextBox)item.FindControl("tbMM5");
                TextBox tbMM6 = (TextBox)item.FindControl("tbMM6");
                TextBox tbMM7 = (TextBox)item.FindControl("tbMM7");
                TextBox tbMM8 = (TextBox)item.FindControl("tbMM8");
                TextBox tbMM9 = (TextBox)item.FindControl("tbMM9");
                TextBox tbMM10 = (TextBox)item.FindControl("tbMM10");
                TextBox tbMM11 = (TextBox)item.FindControl("tbMM11");
                TextBox tbMM12 = (TextBox)item.FindControl("tbMM12");
                TextBox tbMM13 = (TextBox)item.FindControl("tbMM13");
                TextBox tbMM14 = (TextBox)item.FindControl("tbMM14");
                TextBox tbMM15 = (TextBox)item.FindControl("tbMM15");
                TextBox tbMM16 = (TextBox)item.FindControl("tbMM16");
                TextBox tbMM17 = (TextBox)item.FindControl("tbMM17");
                TextBox tbMM18 = (TextBox)item.FindControl("tbMM18");
                TextBox tbMM19 = (TextBox)item.FindControl("tbMM19");
                TextBox tbMM20 = (TextBox)item.FindControl("tbMM20");
                #endregion
                #region "điểm 15p"
                TextBox tb15pM1 = (TextBox)item.FindControl("tb15pM1");
                TextBox tb15pM2 = (TextBox)item.FindControl("tb15pM2");
                TextBox tb15pM3 = (TextBox)item.FindControl("tb15pM3");
                TextBox tb15pM4 = (TextBox)item.FindControl("tb15pM4");
                TextBox tb15pM5 = (TextBox)item.FindControl("tb15pM5");
                TextBox tb15pM6 = (TextBox)item.FindControl("tb15pM6");
                TextBox tb15pM7 = (TextBox)item.FindControl("tb15pM7");
                TextBox tb15pM8 = (TextBox)item.FindControl("tb15pM8");
                TextBox tb15pM9 = (TextBox)item.FindControl("tb15pM9");
                TextBox tb15pM10 = (TextBox)item.FindControl("tb15pM10");
                TextBox tb15pM11 = (TextBox)item.FindControl("tb15pM11");
                TextBox tb15pM12 = (TextBox)item.FindControl("tb15pM12");
                TextBox tb15pM13 = (TextBox)item.FindControl("tb15pM13");
                TextBox tb15pM14 = (TextBox)item.FindControl("tb15pM14");
                TextBox tb15pM15 = (TextBox)item.FindControl("tb15pM15");
                TextBox tb15pM16 = (TextBox)item.FindControl("tb15pM16");
                TextBox tb15pM17 = (TextBox)item.FindControl("tb15pM17");
                TextBox tb15pM18 = (TextBox)item.FindControl("tb15pM18");
                TextBox tb15pM19 = (TextBox)item.FindControl("tb15pM19");
                TextBox tb15pM20 = (TextBox)item.FindControl("tb15pM20");
                #endregion
                #region "điểm 1T hệ số 1"
                TextBox tb1THS1M1 = (TextBox)item.FindControl("tb1THS1M1");
                TextBox tb1THS1M2 = (TextBox)item.FindControl("tb1THS1M2");
                TextBox tb1THS1M3 = (TextBox)item.FindControl("tb1THS1M3");
                TextBox tb1THS1M4 = (TextBox)item.FindControl("tb1THS1M4");
                TextBox tb1THS1M5 = (TextBox)item.FindControl("tb1THS1M5");
                TextBox tb1THS1M6 = (TextBox)item.FindControl("tb1THS1M6");
                TextBox tb1THS1M7 = (TextBox)item.FindControl("tb1THS1M7");
                TextBox tb1THS1M8 = (TextBox)item.FindControl("tb1THS1M8");
                TextBox tb1THS1M9 = (TextBox)item.FindControl("tb1THS1M9");
                TextBox tb1THS1M10 = (TextBox)item.FindControl("tb1THS1M10");
                TextBox tb1THS1M11 = (TextBox)item.FindControl("tb1THS1M11");
                TextBox tb1THS1M12 = (TextBox)item.FindControl("tb1THS1M12");
                TextBox tb1THS1M13 = (TextBox)item.FindControl("tb1THS1M13");
                TextBox tb1THS1M14 = (TextBox)item.FindControl("tb1THS1M14");
                TextBox tb1THS1M15 = (TextBox)item.FindControl("tb1THS1M15");
                TextBox tb1THS1M16 = (TextBox)item.FindControl("tb1THS1M16");
                TextBox tb1THS1M17 = (TextBox)item.FindControl("tb1THS1M17");
                TextBox tb1THS1M18 = (TextBox)item.FindControl("tb1THS1M18");
                TextBox tb1THS1M19 = (TextBox)item.FindControl("tb1THS1M19");
                TextBox tb1THS1M20 = (TextBox)item.FindControl("tb1THS1M20");
                #endregion
                #region "điểm 1T hệ số 2"
                TextBox tb1THS2M1 = (TextBox)item.FindControl("tb1THS2M1");
                TextBox tb1THS2M2 = (TextBox)item.FindControl("tb1THS2M2");
                TextBox tb1THS2M3 = (TextBox)item.FindControl("tb1THS2M3");
                TextBox tb1THS2M4 = (TextBox)item.FindControl("tb1THS2M4");
                TextBox tb1THS2M5 = (TextBox)item.FindControl("tb1THS2M5");
                TextBox tb1THS2M6 = (TextBox)item.FindControl("tb1THS2M6");
                TextBox tb1THS2M7 = (TextBox)item.FindControl("tb1THS2M7");
                TextBox tb1THS2M8 = (TextBox)item.FindControl("tb1THS2M8");
                TextBox tb1THS2M9 = (TextBox)item.FindControl("tb1THS2M9");
                TextBox tb1THS2M10 = (TextBox)item.FindControl("tb1THS2M10");
                TextBox tb1THS2M11 = (TextBox)item.FindControl("tb1THS2M11");
                TextBox tb1THS2M12 = (TextBox)item.FindControl("tb1THS2M12");
                TextBox tb1THS2M13 = (TextBox)item.FindControl("tb1THS2M13");
                TextBox tb1THS2M14 = (TextBox)item.FindControl("tb1THS2M14");
                TextBox tb1THS2M15 = (TextBox)item.FindControl("tb1THS2M15");
                TextBox tb1THS2M16 = (TextBox)item.FindControl("tb1THS2M16");
                TextBox tb1THS2M17 = (TextBox)item.FindControl("tb1THS2M17");
                TextBox tb1THS2M18 = (TextBox)item.FindControl("tb1THS2M18");
                TextBox tb1THS2M19 = (TextBox)item.FindControl("tb1THS2M19");
                TextBox tb1THS2M20 = (TextBox)item.FindControl("tb1THS2M20");
                #endregion
                TextBox tbNoiDung = (TextBox)item.FindControl("tbNoiDung");
                #endregion
                DataRow row = dt.NewRow();
                foreach (DataColumn col in dt.Columns)
                {
                    row[col.ColumnName] = item[col.ColumnName].Text.Replace("&nbsp;", " ");
                    #region "điểm miệng"
                    if (col.ColumnName == "MM1") row[col.ColumnName] = tbMM1.Text;
                    if (col.ColumnName == "MM2") row[col.ColumnName] = tbMM2.Text;
                    if (col.ColumnName == "MM3") row[col.ColumnName] = tbMM3.Text;
                    if (col.ColumnName == "MM4") row[col.ColumnName] = tbMM4.Text;
                    if (col.ColumnName == "MM5") row[col.ColumnName] = tbMM5.Text;
                    if (col.ColumnName == "MM6") row[col.ColumnName] = tbMM6.Text;
                    if (col.ColumnName == "MM7") row[col.ColumnName] = tbMM7.Text;
                    if (col.ColumnName == "MM8") row[col.ColumnName] = tbMM8.Text;
                    if (col.ColumnName == "MM9") row[col.ColumnName] = tbMM9.Text;
                    if (col.ColumnName == "MM10") row[col.ColumnName] = tbMM10.Text;
                    if (col.ColumnName == "MM11") row[col.ColumnName] = tbMM11.Text;
                    if (col.ColumnName == "MM12") row[col.ColumnName] = tbMM12.Text;
                    if (col.ColumnName == "MM13") row[col.ColumnName] = tbMM13.Text;
                    if (col.ColumnName == "MM14") row[col.ColumnName] = tbMM14.Text;
                    if (col.ColumnName == "MM15") row[col.ColumnName] = tbMM15.Text;
                    if (col.ColumnName == "MM16") row[col.ColumnName] = tbMM16.Text;
                    if (col.ColumnName == "MM17") row[col.ColumnName] = tbMM17.Text;
                    if (col.ColumnName == "MM18") row[col.ColumnName] = tbMM18.Text;
                    if (col.ColumnName == "MM19") row[col.ColumnName] = tbMM19.Text;
                    if (col.ColumnName == "MM20") row[col.ColumnName] = tbMM20.Text;
                    #endregion
                    #region "điểm 15p"
                    if (col.ColumnName == "15pM1") row[col.ColumnName] = tb15pM1.Text;
                    if (col.ColumnName == "15pM2") row[col.ColumnName] = tb15pM2.Text;
                    if (col.ColumnName == "15pM3") row[col.ColumnName] = tb15pM3.Text;
                    if (col.ColumnName == "15pM4") row[col.ColumnName] = tb15pM4.Text;
                    if (col.ColumnName == "15pM5") row[col.ColumnName] = tb15pM5.Text;
                    if (col.ColumnName == "15pM6") row[col.ColumnName] = tb15pM6.Text;
                    if (col.ColumnName == "15pM7") row[col.ColumnName] = tb15pM7.Text;
                    if (col.ColumnName == "15pM8") row[col.ColumnName] = tb15pM8.Text;
                    if (col.ColumnName == "15pM9") row[col.ColumnName] = tb15pM9.Text;
                    if (col.ColumnName == "15pM10") row[col.ColumnName] = tb15pM10.Text;
                    if (col.ColumnName == "15pM11") row[col.ColumnName] = tb15pM11.Text;
                    if (col.ColumnName == "15pM12") row[col.ColumnName] = tb15pM12.Text;
                    if (col.ColumnName == "15pM13") row[col.ColumnName] = tb15pM13.Text;
                    if (col.ColumnName == "15pM14") row[col.ColumnName] = tb15pM14.Text;
                    if (col.ColumnName == "15pM15") row[col.ColumnName] = tb15pM15.Text;
                    if (col.ColumnName == "15pM16") row[col.ColumnName] = tb15pM16.Text;
                    if (col.ColumnName == "15pM17") row[col.ColumnName] = tb15pM17.Text;
                    if (col.ColumnName == "15pM18") row[col.ColumnName] = tb15pM18.Text;
                    if (col.ColumnName == "15pM19") row[col.ColumnName] = tb15pM19.Text;
                    if (col.ColumnName == "15pM20") row[col.ColumnName] = tb15pM20.Text;
                    #endregion
                    #region "điểm 1T hệ số 1"
                    if (col.ColumnName == "1THS1M1") row[col.ColumnName] = tb1THS1M1.Text;
                    if (col.ColumnName == "1THS1M2") row[col.ColumnName] = tb1THS1M2.Text;
                    if (col.ColumnName == "1THS1M3") row[col.ColumnName] = tb1THS1M3.Text;
                    if (col.ColumnName == "1THS1M4") row[col.ColumnName] = tb1THS1M4.Text;
                    if (col.ColumnName == "1THS1M5") row[col.ColumnName] = tb1THS1M5.Text;
                    if (col.ColumnName == "1THS1M6") row[col.ColumnName] = tb1THS1M6.Text;
                    if (col.ColumnName == "1THS1M7") row[col.ColumnName] = tb1THS1M7.Text;
                    if (col.ColumnName == "1THS1M8") row[col.ColumnName] = tb1THS1M8.Text;
                    if (col.ColumnName == "1THS1M9") row[col.ColumnName] = tb1THS1M9.Text;
                    if (col.ColumnName == "1THS1M10") row[col.ColumnName] = tb1THS1M10.Text;
                    if (col.ColumnName == "1THS1M11") row[col.ColumnName] = tb1THS1M11.Text;
                    if (col.ColumnName == "1THS1M12") row[col.ColumnName] = tb1THS1M12.Text;
                    if (col.ColumnName == "1THS1M13") row[col.ColumnName] = tb1THS1M13.Text;
                    if (col.ColumnName == "1THS1M14") row[col.ColumnName] = tb1THS1M14.Text;
                    if (col.ColumnName == "1THS1M15") row[col.ColumnName] = tb1THS1M15.Text;
                    if (col.ColumnName == "1THS1M16") row[col.ColumnName] = tb1THS1M16.Text;
                    if (col.ColumnName == "1THS1M17") row[col.ColumnName] = tb1THS1M17.Text;
                    if (col.ColumnName == "1THS1M18") row[col.ColumnName] = tb1THS1M18.Text;
                    if (col.ColumnName == "1THS1M19") row[col.ColumnName] = tb1THS1M19.Text;
                    if (col.ColumnName == "1THS1M20") row[col.ColumnName] = tb1THS1M20.Text;
                    #endregion
                    #region "điểm 1T hệ số 2"
                    if (col.ColumnName == "1THS2M1") row[col.ColumnName] = tb1THS2M1.Text;
                    if (col.ColumnName == "1THS2M2") row[col.ColumnName] = tb1THS2M2.Text;
                    if (col.ColumnName == "1THS2M3") row[col.ColumnName] = tb1THS2M3.Text;
                    if (col.ColumnName == "1THS2M4") row[col.ColumnName] = tb1THS2M4.Text;
                    if (col.ColumnName == "1THS2M5") row[col.ColumnName] = tb1THS2M5.Text;
                    if (col.ColumnName == "1THS2M6") row[col.ColumnName] = tb1THS2M6.Text;
                    if (col.ColumnName == "1THS2M7") row[col.ColumnName] = tb1THS2M7.Text;
                    if (col.ColumnName == "1THS2M8") row[col.ColumnName] = tb1THS2M8.Text;
                    if (col.ColumnName == "1THS2M9") row[col.ColumnName] = tb1THS2M9.Text;
                    if (col.ColumnName == "1THS2M10") row[col.ColumnName] = tb1THS2M10.Text;
                    if (col.ColumnName == "1THS2M11") row[col.ColumnName] = tb1THS2M11.Text;
                    if (col.ColumnName == "1THS2M12") row[col.ColumnName] = tb1THS2M12.Text;
                    if (col.ColumnName == "1THS2M13") row[col.ColumnName] = tb1THS2M13.Text;
                    if (col.ColumnName == "1THS2M14") row[col.ColumnName] = tb1THS2M14.Text;
                    if (col.ColumnName == "1THS2M15") row[col.ColumnName] = tb1THS2M15.Text;
                    if (col.ColumnName == "1THS2M16") row[col.ColumnName] = tb1THS2M16.Text;
                    if (col.ColumnName == "1THS2M17") row[col.ColumnName] = tb1THS2M17.Text;
                    if (col.ColumnName == "1THS2M18") row[col.ColumnName] = tb1THS2M18.Text;
                    if (col.ColumnName == "1THS2M19") row[col.ColumnName] = tb1THS2M19.Text;
                    if (col.ColumnName == "1THS2M20") row[col.ColumnName] = tb1THS2M20.Text;
                    #endregion
                    if (col.ColumnName == "NOI_DUNG_NX") row[col.ColumnName] = tbNoiDung.Text;
                }
                dt.Rows.Add(row);
            }

            int rowHeaderStart = 6;
            int rowStart = 8;
            string colStart = "A";
            string colEnd = localAPI.GetExcelColumnName(lstColumn.Count);
            List<ExcelHeaderEntity> listCell = new List<ExcelHeaderEntity>();
            string tenSo = "";
            string tenPhong = Sys_This_Truong.TEN;
            string tieuDe = "BẢNG TỔNG HỢP ĐIỂM VÀ NHẬN XÉT HÀNG NGÀY";
            string hocKyNamHoc = "Lớp " + rcbLop.Text + ", năm học " + Sys_Ten_Nam_Hoc;
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
                TextBox tbNoiDung = (TextBox)item.FindControl("tbNoiDung");
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
                if (Sys_Hoc_Ky == 1 && (is_dk1 == null || is_dk1 == 0) && (is_mien1 == null || is_mien1 == 0))
                {
                    item.ForeColor = Color.Red;
                    tbNoiDung.Enabled = false;
                }
                else if (Sys_Hoc_Ky == 2 && (is_dk2 == null || is_dk2 == 0) && (is_mien2 == null || is_mien2 == 0))
                {
                    item.ForeColor = Color.Red;
                    tbNoiDung.Enabled = false;
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
                #region image trạng thái
                bool is_send = false;
                if (item["IS_SEND"].Text == "1") is_send = true;
                System.Web.UI.HtmlControls.HtmlImage image_chua_gui = (System.Web.UI.HtmlControls.HtmlImage)item.FindControl("chuaGui");
                System.Web.UI.HtmlControls.HtmlImage image_da_gui = (System.Web.UI.HtmlControls.HtmlImage)item.FindControl("daGui");
                if (is_send)
                {
                    image_chua_gui.Visible = false;
                    image_da_gui.Visible = true;
                    tbNoiDung.Enabled = false;
                }
                else
                {
                    image_chua_gui.Visible = true;
                    image_da_gui.Visible = false;
                    tbNoiDung.Enabled = true;
                }
                #endregion
            }
        }
        protected void btnGuiTin_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.SEND_SMS))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }

            short? ma_khoi = localAPI.ConvertStringToShort(rcbKhoiHoc.SelectedValue);
            long? id_lop = localAPI.ConvertStringTolong(rcbLop.SelectedValue);

            if (ma_khoi == null || id_lop == null) return;

            NHAN_XET_HANG_NGAY detail = new NHAN_XET_HANG_NGAY();
            List<DIEM_CHI_TIET> lstDiemInLop = new List<DIEM_CHI_TIET>();
            lstDiemInLop = diemChiTietBO.getDiemChiTietByTruongKhoiLopAndMonAndCapAndHocKy(Convert.ToInt16(Sys_Ma_Nam_hoc), Sys_This_Truong.ID, ma_khoi, id_lop, Convert.ToInt16(Sys_Hoc_Ky), null, Sys_This_Cap_Hoc);


            List<TONG_HOP_NHAN_XET_HANG_NGAY> listTongHop = thBO.getListTongHopByLop(Sys_This_Truong.ID, id_lop.Value, DateTime.Now);
            List<TIN_NHAN> lstTinNhan = new List<TIN_NHAN>();
            int countSms = 0;

            GridItemCollection lstGrid = new GridItemCollection();
            if (RadGrid1.SelectedItems != null && RadGrid1.SelectedItems.Count > 0)
                lstGrid = RadGrid1.SelectedItems;
            else lstGrid = RadGrid1.Items;

            short nam_gui = Convert.ToInt16(DateTime.Now.Year);
            short thang_gui = Convert.ToInt16(DateTime.Now.Month);
            short tuan_gui = Convert.ToInt16(localAPI.getThisWeek().ToString());

            TinNhanBO tinNhanBO = new TinNhanBO();
            TIN_NHAN checkExists = new TIN_NHAN();
            string brandname = "", cp = "";
            foreach (GridDataItem item in lstGrid)
            {
                ResultEntity res = new ResultEntity();
                res.Res = false;
                res.Msg = "Không có thay đổi";
                long id_hs = Convert.ToInt64(item.GetDataKeyValue("ID_HS").ToString());
                string sdt = item["SDT"].Text;
                string loai_nha_mang = localAPI.getLoaiNhaMang(sdt);

                #region Điểm
                #region Get control
                List<TextBox> lsttbM = new List<TextBox>();
                List<TextBox> lsttb15p = new List<TextBox>();
                List<TextBox> lsttb1THS1 = new List<TextBox>();
                List<TextBox> lsttb1THS2 = new List<TextBox>();
                List<TextBox> lsttbHK = new List<TextBox>();
                lsttbM.Add((TextBox)item.FindControl("tbMM1"));
                lsttbM.Add((TextBox)item.FindControl("tbMM2"));
                lsttbM.Add((TextBox)item.FindControl("tbMM3"));
                lsttbM.Add((TextBox)item.FindControl("tbMM4"));
                lsttbM.Add((TextBox)item.FindControl("tbMM5"));
                lsttbM.Add((TextBox)item.FindControl("tbMM6"));
                lsttbM.Add((TextBox)item.FindControl("tbMM7"));
                lsttbM.Add((TextBox)item.FindControl("tbMM8"));
                lsttbM.Add((TextBox)item.FindControl("tbMM9"));
                lsttbM.Add((TextBox)item.FindControl("tbMM10"));
                lsttbM.Add((TextBox)item.FindControl("tbMM11"));
                lsttbM.Add((TextBox)item.FindControl("tbMM12"));
                lsttbM.Add((TextBox)item.FindControl("tbMM13"));
                lsttbM.Add((TextBox)item.FindControl("tbMM14"));
                lsttbM.Add((TextBox)item.FindControl("tbMM15"));
                lsttbM.Add((TextBox)item.FindControl("tbMM16"));
                lsttbM.Add((TextBox)item.FindControl("tbMM17"));
                lsttbM.Add((TextBox)item.FindControl("tbMM18"));
                lsttbM.Add((TextBox)item.FindControl("tbMM19"));
                lsttbM.Add((TextBox)item.FindControl("tbMM20"));

                lsttb15p.Add((TextBox)item.FindControl("tb15pM1"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM2"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM3"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM4"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM5"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM6"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM7"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM8"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM9"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM10"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM11"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM12"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM13"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM14"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM15"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM16"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM17"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM18"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM19"));
                lsttb15p.Add((TextBox)item.FindControl("tb15pM20"));

                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M1"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M2"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M3"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M4"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M5"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M6"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M7"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M8"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M9"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M10"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M11"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M12"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M13"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M14"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M15"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M16"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M17"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M18"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M19"));
                lsttb1THS1.Add((TextBox)item.FindControl("tb1THS1M20"));

                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M1"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M2"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M3"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M4"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M5"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M6"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M7"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M8"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M9"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M10"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M11"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M12"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M13"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M14"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M15"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M16"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M17"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M18"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M19"));
                lsttb1THS2.Add((TextBox)item.FindControl("tb1THS2M20"));

                lsttbHK.Add((TextBox)item.FindControl("tbHKM1"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM2"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM3"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM4"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM5"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM6"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM7"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM8"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM9"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM10"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM11"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM12"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM13"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM14"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM15"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM16"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM17"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM18"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM19"));
                lsttbHK.Add((TextBox)item.FindControl("tbHKM20"));
                #endregion

                int i = 0;
                foreach (RepeaterItem itemMon in rpMonHoc.Items)
                {
                    #region Lặp môn
                    HiddenField hdIDMonTruong = (HiddenField)itemMon.FindControl("hdIDMonTruong");
                    long? id_mon_hoc_truong = localAPI.ConvertStringTolong(hdIDMonTruong.Value);
                    HiddenField hdKieuMon = (HiddenField)itemMon.FindControl("hdKieuMon");
                    bool is_tinh_diem = (hdKieuMon.Value.ToLower() == "false" || string.IsNullOrEmpty(hdKieuMon.Value) || hdKieuMon.Value == "0");
                    Label lbName = (Label)itemMon.FindControl("lbName");
                    CheckBox cbM = (CheckBox)itemMon.FindControl("cbM");
                    CheckBox cb15P = (CheckBox)itemMon.FindControl("cb15P");
                    CheckBox cb1THS1 = (CheckBox)itemMon.FindControl("cb1THS1");
                    CheckBox cb1THS2 = (CheckBox)itemMon.FindControl("cb1THS2");
                    CheckBox cbHocKy = (CheckBox)itemMon.FindControl("cbHocKy");

                    if ((cbM.Checked || cb15P.Checked || cb1THS1.Checked || cb1THS2.Checked || cbHocKy.Checked) && id_mon_hoc_truong != null)
                    {
                        if (!string.IsNullOrEmpty(lsttbM[i].Text) || !string.IsNullOrEmpty(lsttb15p[i].Text) || !string.IsNullOrEmpty(lsttb1THS1[i].Text) || !string.IsNullOrEmpty(lsttb1THS2[i].Text) || !string.IsNullOrEmpty(lsttbHK[i].Text))
                        {
                            #region Get detail điểm
                            DIEM_CHI_TIET diemDetail = new DIEM_CHI_TIET();
                            diemDetail = lstDiemInLop.FirstOrDefault(x => x.ID_HOC_SINH == id_hs && x.ID_MON_HOC_TRUONG == id_mon_hoc_truong.Value);
                            #endregion
                            if (diemDetail != null)
                            {
                                #region Điểm M
                                if (!string.IsNullOrEmpty(lsttbM[i].Text.Trim()))
                                {
                                    Decimal? diemTmp = is_tinh_diem ? localAPI.ConvertStringToDecimal(lsttbM[i].Text.Trim()) : dataAccessAPI.ConvertCDToDecimal(lsttbM[i].Text.Trim());
                                    if (diemDetail.DIEM1 == null) diemDetail.DIEM1 = diemTmp;
                                    else if (diemDetail.DIEM2 == null) diemDetail.DIEM2 = diemTmp;
                                    else if (diemDetail.DIEM3 == null) diemDetail.DIEM3 = diemTmp;
                                    else if (diemDetail.DIEM4 == null) diemDetail.DIEM4 = diemTmp;
                                    else if (diemDetail.DIEM5 == null) diemDetail.DIEM5 = diemTmp;
                                }
                                #endregion
                                #region Điểm 15p
                                if (!string.IsNullOrEmpty(lsttb15p[i].Text.Trim()))
                                {
                                    Decimal? diemTmp = is_tinh_diem ? localAPI.ConvertStringToDecimal(lsttb15p[i].Text.Trim()) : dataAccessAPI.ConvertCDToDecimal(lsttb15p[i].Text.Trim());
                                    if (diemDetail.DIEM6 == null) diemDetail.DIEM6 = diemTmp;
                                    else if (diemDetail.DIEM7 == null) diemDetail.DIEM7 = diemTmp;
                                    else if (diemDetail.DIEM8 == null) diemDetail.DIEM8 = diemTmp;
                                    else if (diemDetail.DIEM9 == null) diemDetail.DIEM9 = diemTmp;
                                    else if (diemDetail.DIEM10 == null) diemDetail.DIEM10 = diemTmp;
                                }
                                #endregion
                                #region Điểm 1THS1
                                if (!string.IsNullOrEmpty(lsttb1THS1[i].Text.Trim()))
                                {
                                    Decimal? diemTmp = is_tinh_diem ? localAPI.ConvertStringToDecimal(lsttb1THS1[i].Text.Trim()) : dataAccessAPI.ConvertCDToDecimal(lsttb1THS1[i].Text.Trim());
                                    if (diemDetail.DIEM11 == null) diemDetail.DIEM11 = diemTmp;
                                    else if (diemDetail.DIEM12 == null) diemDetail.DIEM12 = diemTmp;
                                    else if (diemDetail.DIEM13 == null) diemDetail.DIEM13 = diemTmp;
                                    else if (diemDetail.DIEM14 == null) diemDetail.DIEM14 = diemTmp;
                                    else if (diemDetail.DIEM15 == null) diemDetail.DIEM15 = diemTmp;
                                }
                                #endregion
                                #region Điểm 1THS2
                                if (!string.IsNullOrEmpty(lsttb1THS2[i].Text.Trim()))
                                {
                                    Decimal? diemTmp = is_tinh_diem ? localAPI.ConvertStringToDecimal(lsttb1THS2[i].Text.Trim()) : dataAccessAPI.ConvertCDToDecimal(lsttb1THS2[i].Text.Trim());
                                    if (diemDetail.DIEM16 == null) diemDetail.DIEM16 = diemTmp;
                                    else if (diemDetail.DIEM17 == null) diemDetail.DIEM17 = diemTmp;
                                    else if (diemDetail.DIEM18 == null) diemDetail.DIEM18 = diemTmp;
                                    else if (diemDetail.DIEM19 == null) diemDetail.DIEM19 = diemTmp;
                                    else if (diemDetail.DIEM20 == null) diemDetail.DIEM20 = diemTmp;
                                }
                                #endregion
                                #region Điểm học kỳ
                                if (!string.IsNullOrEmpty(lsttbHK[i].Text.Trim()))
                                {
                                    Decimal? diemTmp = is_tinh_diem ? localAPI.ConvertStringToDecimal(lsttbHK[i].Text.Trim()) : dataAccessAPI.ConvertCDToDecimal(lsttbHK[i].Text.Trim());
                                    diemDetail.DIEM_HOC_KY = diemTmp;
                                }
                                #endregion
                                res = diemChiTietBO.update(diemDetail, Sys_User);
                            }
                        }
                    }
                    i++;
                    #endregion
                }

                string str_Diem = "";
                List<DiemChiTietEntity> lstDiem = diemChiTietBO.getDiemGuiTinHangNgay(Convert.ToInt16(Sys_Ma_Nam_hoc), Sys_This_Truong.ID, ma_khoi.Value, id_lop.Value, Convert.ToInt16(Sys_Hoc_Ky), DateTime.Now, id_hs);
                if (lstDiem.Count > 0)
                {
                    for (int k = 0; k < lstDiem.Count; k++)
                    {
                        if (!string.IsNullOrEmpty(lstDiem[k].DIEMMIENG) || !string.IsNullOrEmpty(lstDiem[k].DIEM15P) || !string.IsNullOrEmpty(lstDiem[k].DIEM1T_HS1) || !string.IsNullOrEmpty(lstDiem[k].DIEM1T_HS2) || !string.IsNullOrEmpty(lstDiem[k].DIEMHOCKY))
                        {
                            str_Diem += lstDiem[k].TEN_MON_TRUONG + "(";

                            string strDes = "";
                            string addThem = "";
                            strDes += string.IsNullOrEmpty(lstDiem[k].DIEMMIENG) ? "" : "M:" + lstDiem[k].DIEMMIENG.TrimEnd(',');

                            addThem = (string.IsNullOrEmpty(strDes) ? "" : ",");
                            strDes = strDes
                                    + (string.IsNullOrEmpty(lstDiem[k].DIEM15P) ? "" : addThem + "15P:" + lstDiem[k].DIEM15P.TrimEnd(','));

                            string str1T = (string.IsNullOrEmpty(lstDiem[k].DIEM1T_HS1) ? "" : lstDiem[k].DIEM1T_HS1.TrimEnd(','))
                                + (!string.IsNullOrEmpty(lstDiem[k].DIEM1T_HS1) && !string.IsNullOrEmpty(lstDiem[k].DIEM1T_HS2) ? "," : "")
                               + (string.IsNullOrEmpty(lstDiem[k].DIEM1T_HS2) ? "" : lstDiem[k].DIEM1T_HS2.TrimEnd(','));

                            addThem = (string.IsNullOrEmpty(strDes) ? "" : ",");
                            strDes = strDes
                                + (string.IsNullOrEmpty(str1T) ? "" : addThem + "1T:" + str1T);
                            strDes += (string.IsNullOrEmpty(lstDiem[k].DIEMHOCKY) ? "" : "HK:" + lstDiem[k].DIEMHOCKY.TrimEnd(','));

                            str_Diem += strDes;
                            str_Diem += ") ";
                        }
                    }
                }

                #endregion

                long? id_nx = localAPI.ConvertStringTolong(item["ID"].Text);
                TextBox tbNoiDung = (TextBox)item.FindControl("tbNoiDung");
                HiddenField hdNoiDung = (HiddenField)item.FindControl("hdNoiDung");
                string str_noi_dung = tbNoiDung.Text.Trim();
                str_noi_dung = str_noi_dung.TrimEnd(',');
                string str_noi_dung_old = hdNoiDung.Value;

                bool checkSend = false;
                if (item["IS_SEND"].Text == "1") checkSend = true;
                if (!checkSend)
                {

                    #region Nhận xét hàng ngày
                    if (id_nx != null)
                    {
                        detail = nxhnBO.getNhanXetHangNgayByID(id_nx.Value);
                        detail.NOI_DUNG_NX = str_noi_dung;
                        if (!string.IsNullOrEmpty(str_noi_dung))
                        {
                            detail.IS_SEND = true;
                            res = nxhnBO.update(detail, Sys_User.ID);
                        }
                        else res = nxhnBO.update(detail, Sys_User.ID);
                    }
                    else if (id_nx == null && !string.IsNullOrEmpty(str_noi_dung))
                    {
                        detail = new NHAN_XET_HANG_NGAY();
                        detail.NOI_DUNG_NX = str_noi_dung;
                        detail.ID_HOC_SINH = id_hs;
                        detail.ID_TRUONG = Sys_This_Truong.ID;
                        detail.ID_NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
                        detail.MA_KHOI = ma_khoi.Value;
                        detail.ID_LOP = id_lop.Value;
                        detail.NGAY_NX = DateTime.Now;
                        detail.IS_SEND = true;
                        if (str_noi_dung != "")
                        {
                            res = nxhnBO.insert(detail, Sys_User.ID);
                            NHAN_XET_HANG_NGAY resNXHN = (NHAN_XET_HANG_NGAY)res.ResObject;
                            if (res.Res)
                            {
                                id_nx = resNXHN.ID;
                            }
                        }
                    }
                    #endregion

                    #region lấy điểm và nhận xét hàng ngày để gửi tin
                    String noi_dung_gui = !string.IsNullOrEmpty(str_noi_dung) ? (str_Diem + localAPI.chuyenTiengVietKhongDau(str_noi_dung)) : str_Diem;

                    #region tong hop tin nhan
                    long id_th_nxhn = 0;
                    TONG_HOP_NHAN_XET_HANG_NGAY tongHopNXHN = listTongHop.Where(x => x.ID_HOC_SINH == id_hs).FirstOrDefault();
                    if (!string.IsNullOrEmpty(noi_dung_gui))
                    {
                        if (tongHopNXHN == null)
                        {
                            #region tong hop NXHN
                            TONG_HOP_NHAN_XET_HANG_NGAY thNew = new TONG_HOP_NHAN_XET_HANG_NGAY();
                            thNew.ID_HOC_SINH = id_hs;
                            thNew.ID_NHAN_XET_HN = id_nx;
                            thNew.ID_TRUONG = Sys_This_Truong.ID;
                            thNew.ID_LOP = id_lop;
                            thNew.NOI_DUNG_NX = noi_dung_gui;
                            thNew.NGAY_TONG_HOP = DateTime.Now;
                            thNew.IS_SEND = 1;
                            res = thBO.insert(thNew, Sys_User.ID);
                            TONG_HOP_NHAN_XET_HANG_NGAY resTongHop = (TONG_HOP_NHAN_XET_HANG_NGAY)res.ResObject;
                            id_th_nxhn = resTongHop.ID;
                            #endregion
                        }
                        else if (tongHopNXHN != null && tongHopNXHN.IS_SEND != 1)
                        {
                            id_th_nxhn = tongHopNXHN.ID;
                            tongHopNXHN.NOI_DUNG_NX = noi_dung_gui;
                            tongHopNXHN.IS_SEND = 1;
                            res = thBO.update(tongHopNXHN, Sys_User.ID);
                        }

                        string tien_to = item["TIEN_TO"].Text;
                        #region "check dang ky sms"
                        short? is_dk1 = localAPI.ConvertStringToShort(item["IS_DK_KY1"].Text);
                        short? is_dk2 = localAPI.ConvertStringToShort(item["IS_DK_KY2"].Text);
                        bool is_sms = false;
                        if (is_dk1 != null && is_dk1 == 1 && Sys_Hoc_Ky == 1) is_sms = true;
                        else if (is_dk2 != null && is_dk2 == 1 && Sys_Hoc_Ky == 2) is_sms = true;
                        #endregion

                        noi_dung_gui = !string.IsNullOrEmpty(tien_to) ? (tien_to + " " + noi_dung_gui) : noi_dung_gui;
                        string noi_dung_khong_dau = localAPI.chuyenTiengVietKhongDau(noi_dung_gui);
                        
                        checkExists = tinNhanBO.checkExistsSms(Sys_This_Truong.ID, nam_gui, thang_gui, null, sdt, null, Sys_Time_Send, noi_dung_khong_dau);
                        if (checkExists == null && !string.IsNullOrEmpty(loai_nha_mang) && is_sms)
                        {
                            int so_tin = localAPI.demSoTin(noi_dung_khong_dau);
                            TIN_NHAN sms = new TIN_NHAN();
                            sms.ID_TRUONG = Sys_This_Truong.ID;
                            sms.ID_NGUOI_NHAN = id_hs;
                            sms.LOAI_NGUOI_NHAN = 1;
                            sms.SDT_NHAN = sdt;
                            sms.MA_GOI_TIN = Sys_This_Truong.MA_GOI_TIN;
                            sms.THOI_GIAN_GUI = DateTime.Now;
                            sms.NGUOI_GUI = Sys_User.ID;
                            sms.ID_NHAN_XET_HANG_NGAY = id_nx;
                            sms.ID_TONG_HOP_NXHN = id_th_nxhn;
                            sms.LOAI_TIN = 1;
                            sms.NAM_GUI = nam_gui;
                            sms.THANG_GUI = thang_gui;
                            sms.TUAN_GUI = tuan_gui;
                            sms.NOI_DUNG = noi_dung_gui;
                            sms.NOI_DUNG_KHONG_DAU = noi_dung_khong_dau;
                            sms.SO_TIN = so_tin;
                            sms.NGAY_TAO = DateTime.Now;
                            sms.NGUOI_TAO = Sys_User.ID;
                            sms.LOAI_NHA_MANG = loai_nha_mang;
                            brandname = ""; cp = "";
                            localAPI.getBrandnameAndCp(Sys_This_Truong, loai_nha_mang, out brandname, out cp);
                            sms.BRAND_NAME = brandname;
                            sms.CP = cp;
                            if (Sys_This_Truong.ID_DOI_TAC != null && Sys_This_Truong.ID_DOI_TAC > 0)
                                sms.ID_DOI_TAC = Sys_This_Truong.ID_DOI_TAC;
                            else sms.ID_DOI_TAC = null;
                            lstTinNhan.Add(sms);
                            countSms += so_tin;
                        }
                        #endregion
                    }

                    #endregion

                }
            }

            #region "check gửi GVCN"

            if (cboGuiGVCN.Checked)
            {
                GiaoVienBO giaoVienBO = new GiaoVienBO();
                LopBO lopBO = new LopBO();
                LOP lop = lopBO.getLopById(id_lop.Value);
                if (lop != null)
                {
                    if (lop.ID_GVCN != null)
                    {
                        long id_giao_vien = lop.ID_GVCN.Value;
                        GIAO_VIEN giaoVien = giaoVienBO.getGiaoVienByID(id_giao_vien);

                        string sdt_gv = giaoVien != null ? giaoVien.SDT : "";
                        string loai_nha_mang_gv = localAPI.getLoaiNhaMang(sdt_gv);
                        string noi_dung_gui_gv = localAPI.chuyenTiengVietKhongDau(tbNoiDungChen.Text.Trim());
                        int so_tin = localAPI.demSoTin(noi_dung_gui_gv);
                        
                        checkExists = tinNhanBO.checkExistsSms(Sys_This_Truong.ID, nam_gui, thang_gui, null, sdt_gv, null, Sys_Time_Send, noi_dung_gui_gv);

                        if (checkExists == null && !string.IsNullOrEmpty(noi_dung_gui_gv) && !string.IsNullOrEmpty(loai_nha_mang_gv) && so_tin > 0)
                        {
                            TIN_NHAN sms = new TIN_NHAN();
                            sms.ID_TRUONG = Sys_This_Truong.ID;
                            sms.ID_NGUOI_NHAN = id_giao_vien;
                            sms.LOAI_NGUOI_NHAN = 2;
                            sms.SDT_NHAN = sdt_gv;
                            sms.MA_GOI_TIN = Sys_This_Truong.MA_GOI_TIN;
                            sms.THOI_GIAN_GUI = DateTime.Now;
                            sms.NGUOI_GUI = Sys_User.ID;
                            sms.LOAI_TIN = 1;
                            sms.NAM_GUI = nam_gui;
                            sms.THANG_GUI = thang_gui;
                            sms.TUAN_GUI = tuan_gui;
                            sms.NOI_DUNG = noi_dung_gui_gv;
                            sms.NOI_DUNG_KHONG_DAU = noi_dung_gui_gv;
                            sms.SO_TIN = so_tin;
                            sms.NGAY_TAO = DateTime.Now;
                            sms.NGUOI_TAO = Sys_User.ID;
                            sms.LOAI_NHA_MANG = loai_nha_mang_gv;
                            brandname = ""; cp = "";
                            localAPI.getBrandnameAndCp(Sys_This_Truong, loai_nha_mang_gv, out brandname, out cp);
                            sms.BRAND_NAME = brandname;
                            sms.CP = cp;
                            if (Sys_This_Truong.ID_DOI_TAC != null && Sys_This_Truong.ID_DOI_TAC > 0)
                                sms.ID_DOI_TAC = Sys_This_Truong.ID_DOI_TAC;
                            else sms.ID_DOI_TAC = null;
                            lstTinNhan.Add(sms);
                            countSms += so_tin;
                        }
                    }
                }
            }
            
            #endregion

            #region save sms
            ResultEntity result = new ResultEntity();
            result.Res = true;
            if (lstTinNhan.Count > 0)
            {
                #region "check quy tin nhan"
                if (Sys_This_Truong.ID_DOI_TAC == null || Sys_This_Truong.ID_DOI_TAC == 0)
                {
                    QUY_TIN quyTinTheoNam = quyTinBO.getQuyTinByNamHoc(Convert.ToInt16(localAPI.GetYearNow()), Sys_This_Truong.ID, 1, Sys_User.ID);
                    bool is_insert_new_quytb = false;
                    QUY_TIN quyTinTheoThang = quyTinBO.getQuyTin(Convert.ToInt16(DateTime.Now.Year), Convert.ToInt16(DateTime.Now.Month), Sys_This_Truong.ID, 1, Sys_User.ID, out is_insert_new_quytb);
                    if (quyTinTheoNam != null && quyTinTheoThang != null)
                    {
                        double tong_con_thang = quyTinTheoThang.TONG_CON + ((quyTinTheoThang.TONG_CAP + quyTinTheoThang.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                    / 100);
                        double tong_con_nam = quyTinTheoNam.TONG_CON + ((quyTinTheoNam.TONG_CAP + quyTinTheoNam.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                            / 100);
                        if (Sys_This_Truong.IS_SAN_QUY_TIN_NAM != null && Sys_This_Truong.IS_SAN_QUY_TIN_NAM.Value)
                        {
                            if (countSms > tong_con_nam)
                            {
                                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị không còn đủ quota để gửi tin nhắn. Vui lòng liên hệ với quản trị viên!');", true);
                                return;
                            }
                            else
                            {
                                result = tinNhanBO.insertTinNhanThongBao(lstTinNhan, Sys_This_Truong.ID, Convert.ToInt16(DateTime.Now.Year), Convert.ToInt16(DateTime.Now.Month), 0, Sys_User.ID);
                                if (countSms >= tong_con_thang)
                                {
                                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Đơn vị đã sử dụng vượt quá quỹ tin tháng!');", true);
                                }
                            }
                        }
                        else
                        {
                            if (countSms > tong_con_thang || countSms > tong_con_nam)
                            {
                                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị không còn đủ quota để gửi tin nhắn. Vui lòng liên hệ với quản trị viên!');", true);
                                return;
                            }
                            else
                            {
                                result = tinNhanBO.insertTinNhanThongBao(lstTinNhan, Sys_This_Truong.ID, Convert.ToInt16(DateTime.Now.Year), Convert.ToInt16(DateTime.Now.Month), 0, Sys_User.ID);
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
                        if (Sys_This_Truong.IS_ACTIVE_SMS != null && Sys_This_Truong.IS_ACTIVE_SMS == true && tong_tin_con > countSms)
                        {
                            result = tinNhanBO.insertTinNhanThongBao(lstTinNhan, Sys_This_Truong.ID, Convert.ToInt16(DateTime.Now.Year), Convert.ToInt16(DateTime.Now.Month), 0, Sys_User.ID);
                        }
                    }
                }
                #endregion
            }
            #endregion

            string strMsg = "";
            if (!result.Res && countSms > 0)
            {
                strMsg = result.Msg;
            }
            else if (countSms == 0)
            {
                strMsg = " notification('warning', 'Không có tin nào được gửi đi.');";
            }
            else
            {
                strMsg = " notification('success', 'Có " + countSms + " tin nhắn được gửi.');";
                logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi tin nhắn tổng hợp lớp " + id_lop, Sys_User.ID, DateTime.Now);
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);

            RadGrid1.Rebind();
            lblTongTinSuDung.Text = "Số tin vừa gửi: <b>" + countSms + "</b>";
            viewQuyTinCon();
        }
        protected void btnGuiLai_Click(object sender, EventArgs e)
        {
            #region check quyen
            if (Sys_User.IS_ROOT == null || (Sys_User.IS_ROOT != null && Sys_User.IS_ROOT == false))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện chức năng này.');", true);
                return;
            }
            #endregion
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            int success = 0;
            long? id_lop = localAPI.ConvertStringTolong(rcbLop.SelectedValue);

            foreach (GridDataItem item in RadGrid1.SelectedItems)
            {
                long? id_nx = localAPI.ConvertStringTolong(item["ID"].Text);
                bool is_send = false;
                if (item["IS_SEND"].Text == "1") is_send = true;
                if (id_nx != null && is_send)
                {
                    res = nxhnBO.updateTrangThaiGuiTongHop(id_nx.Value, Sys_User.ID);
                    if (res.Res)
                    {
                        success++;
                        logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật trạng thái gửi lại tổng hợp NXHN lớp " + id_lop, Sys_User.ID, DateTime.Now);
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
        protected void viewQuyTinCon()
        {
            btnGuiTin.Visible = (is_access(SYS_Type_Access.SEND_SMS) && !(Sys_This_Truong.IS_ACTIVE_SMS != true));
            short nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc);
            short thang = Convert.ToInt16(DateTime.Now.Month);
            if (thang < 8) nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc + 1);
            if (Sys_This_Truong.ID_DOI_TAC == null || Sys_This_Truong.ID_DOI_TAC == 0)
            {
                QUY_TIN quyTinTheoNam = quyTinBO.getQuyTinByNamHoc(Convert.ToInt16(localAPI.GetYearNow()), Sys_This_Truong.ID, 1, Sys_User.ID);
                bool is_insert_new_quytb = false;
                QUY_TIN quyTinTheoThang = quyTinBO.getQuyTin(nam_hoc, thang, Sys_This_Truong.ID, 1, Sys_User.ID, out is_insert_new_quytb);
                if (quyTinTheoNam != null && quyTinTheoThang != null)
                {
                    double tong_con_thang = quyTinTheoThang.TONG_CON + (quyTinTheoThang.TONG_CAP + quyTinTheoThang.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                    / 100;
                    double tong_con_nam = quyTinTheoNam.TONG_CON + (quyTinTheoNam.TONG_CAP + quyTinTheoNam.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                        / 100;
                    if (Sys_This_Truong.IS_ACTIVE_SMS != true || tong_con_nam <= 0)
                    {
                        btnGuiTin.Visible = false;
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
                    btnGuiTin.Visible = false;
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
                        btnGuiTin.Visible = false;
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", " notification('warning', 'Đơn vị chưa đăng ký sử dụng SMS nên không thể gửi tin nhắn!');", true);
                    }
                }
                if (tong_tin_con <= 0)
                {
                    btnGuiTin.Visible = false;
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", " notification('warning', 'Đơn vị đã sử dụng hết quỹ tin được cấp. Vui lòng liên hệ lại với quản trị viên!');", true);
                }
            }
        }
        protected void rpMonHoc_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (Sys_This_Truong.ID == 197)
            {
                if (e.Item.ItemType == ListItemType.Header)
                {
                    HtmlTableCell thTierFacility = (HtmlTableCell)e.Item.FindControl("th1THS1");

                    if (thTierFacility != null) { thTierFacility.Visible = false; }
                }
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    HtmlTableCell tdTableCell = (HtmlTableCell)e.Item.FindControl("td1THS1");
                    if (tdTableCell != null) tdTableCell.Visible = false;
                }
            }
        }
    }
}