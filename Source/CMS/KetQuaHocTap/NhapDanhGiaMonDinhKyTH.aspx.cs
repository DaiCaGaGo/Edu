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
    public partial class NhapDanhGiaMonDinhKyTH : AuthenticatePage
    {
        LocalAPI localAPI = new LocalAPI();
        DanhGiaDinhKyMonTHBO danhGiaDinhKyMonTHBO = new DanhGiaDinhKyMonTHBO();
        MonHocTruongBO monHocTruongBO = new MonHocTruongBO();
        private KhoaSoBO khoaSoBO = new KhoaSoBO();
        private bool checkKhoa = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            btSave.Visible = is_access(SYS_Type_Access.THEM) || is_access(SYS_Type_Access.SUA);
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
                objKyDGTH.SelectParameters.Add("id_hocKy", Sys_Hoc_Ky.ToString());
                rcbKyDGTH.DataBind();
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
            short? ma_khoi = localAPI.ConvertStringToShort(rcbKhoiHoc.SelectedValue);
            short? id_kydg = localAPI.ConvertStringToShort(rcbKyDGTH.SelectedValue);
            if (id_lop != null && id_mon_hoc_truong != null && ma_khoi != null && id_kydg != null)
                danhGiaDinhKyMonTHBO.insertEmpty(Convert.ToInt16(Sys_Ma_Nam_hoc), Sys_This_Truong.ID, ma_khoi.Value, id_lop.Value, id_kydg.Value, id_mon_hoc_truong.Value, Sys_User.ID);
        }
        protected void rcbKyDGTH_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            insertEmpty();
            checkKhoaSoTheoMon();
            RadGrid1.Rebind();
        }
        protected void rcbMonHoc_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
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
        protected void btSave_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.THEM) && !is_access(SYS_Type_Access.SUA))
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
            long? id_lop = localAPI.ConvertStringTolong(rcbLop.SelectedValue);
            long? id_mon_hoc_truong = localAPI.ConvertStringTolong(rcbMonHoc.SelectedValue);
            short? ma_khoi = localAPI.ConvertStringToShort(rcbKhoiHoc.SelectedValue);
            short? id_kydg = localAPI.ConvertStringToShort(rcbKyDGTH.SelectedValue);
            MON_HOC_TRUONG detailMonHoc = new MON_HOC_TRUONG();
            detailMonHoc = monHocTruongBO.getMonTruongByID(id_mon_hoc_truong.Value);
            if (id_lop != null && id_mon_hoc_truong != null)
            {
                int success = 0; int count_change = 0;
                foreach (GridDataItem item in RadGrid1.Items)
                {
                    var detail = new DANH_GIA_DINH_KY_MON_TH();
                    long id_Detail = localAPI.ConvertStringTolong(item.GetDataKeyValue("ID").ToString()).Value;
                    detail = danhGiaDinhKyMonTHBO.getDanhGiaDinhKyMonTHByID(id_Detail);
                    if (detail != null && detailMonHoc != null)
                    {
                        #region get control
                        //TextBox tbMA_NX = (TextBox)item.FindControl("tbMA_NX");
                        TextBox tbNOI_DUNG_NX = (TextBox)item.FindControl("tbNOI_DUNG_NX");
                        TextBox tbKTDK = (TextBox)item.FindControl("tbKTDK");
                        TextBox tbMUC = (TextBox)item.FindControl("tbMUC");

                        //HiddenField hdMA_NX = (HiddenField)item.FindControl("hdMA_NX");
                        HiddenField hdNOI_DUNG_NX = (HiddenField)item.FindControl("hdNOI_DUNG_NX");
                        HiddenField hdKTDK = (HiddenField)item.FindControl("hdKTDK");
                        HiddenField hdMUC = (HiddenField)item.FindControl("hdMUC");
                        #endregion
                        #region get value control
                        //string MA_NX = tbMA_NX.Text.Trim();
                        string NOI_DUNG_NX = tbNOI_DUNG_NX.Text.Trim();
                        string MUC = tbMUC.Text.Trim();
                        string KTDK = tbKTDK.Text.Trim();
                        //string MA_NX_old = hdMA_NX.Value;
                        string NOI_DUNG_NX_old = hdNOI_DUNG_NX.Value;
                        string KTDK_old = hdKTDK.Value;
                        string MUC_old = hdMUC.Value;
                        #endregion
                        #region set value detail
                        //detail.MA_NX = MA_NX;
                        detail.NOI_DUNG_NX = NOI_DUNG_NX;
                        detail.KTDK = localAPI.ConvertStringToShort(KTDK);
                        detail.MUC = MUC;

                        #endregion
                        if (
                            //MA_NX != MA_NX_old ||
                            NOI_DUNG_NX != NOI_DUNG_NX_old || KTDK != KTDK_old || MUC != MUC_old)
                        {
                            count_change++;
                            res = danhGiaDinhKyMonTHBO.update(detail, Sys_User);
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
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            long? id_lop = localAPI.ConvertStringTolong(rcbLop.SelectedValue);
            long? id_mon_hoc_truong = localAPI.ConvertStringTolong(rcbMonHoc.SelectedValue);
            short? ma_khoi = localAPI.ConvertStringToShort(rcbKhoiHoc.SelectedValue);
            short? id_kydg = localAPI.ConvertStringToShort(rcbKyDGTH.SelectedValue);
            if (id_mon_hoc_truong == null) id_mon_hoc_truong = 0;
            MON_HOC_TRUONG detailMonHoc = new MON_HOC_TRUONG();
            detailMonHoc = monHocTruongBO.getMonTruongByID(id_mon_hoc_truong.Value);
            RadGrid1.DataSource = danhGiaDinhKyMonTHBO.getDanhGiaDinhKyMonTHByTruongKhoiLopMonAndGiaiDoan(Convert.ToInt16(Sys_Ma_Nam_hoc), Sys_This_Truong.ID, ma_khoi, id_lop, id_mon_hoc_truong, id_kydg.Value);
            if (((detailMonHoc != null && (detailMonHoc.KIEU_MON == false || detailMonHoc.KIEU_MON == null)) || detailMonHoc == null) && (id_kydg != null && ((id_kydg == 2 || id_kydg == 4) || (ma_khoi != null && (ma_khoi == 4|| ma_khoi == 5)))))
                RadGrid1.MasterTableView.Columns.FindByUniqueName("KTDK").Display = true;
            else RadGrid1.MasterTableView.Columns.FindByUniqueName("KTDK").Display = false;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
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
            string newName = "Danh_gia_dinh_ky_mon.xlsx";

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
                    lstHeader.Add(new ExcelHeaderEntity { name = "Mã HS", colM = 1, rowM = 1, width = 18 });
                    lstColumn.Add(new ExcelEntity { Name = "MA_HS", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TEN_HS") && item.UniqueName == "TEN_HS")
                {
                    DataColumn col = new DataColumn("TEN_HS");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Họ tên", colM = 1, rowM = 1, width = 30 });
                    lstColumn.Add(new ExcelEntity { Name = "TEN_HS", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "NGAY_SINH") && item.UniqueName == "NGAY_SINH")
                {
                    DataColumn col = new DataColumn("NGAY_SINH");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Ngày sinh", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "NGAY_SINH", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "NOI_DUNG_NX") && item.UniqueName == "NOI_DUNG_NX")
                {
                    DataColumn col = new DataColumn("NOI_DUNG_NX");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Nội dung", colM = 1, rowM = 1, width = 60 });
                    lstColumn.Add(new ExcelEntity { Name = "NOI_DUNG_NX", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "KTDK") && item.UniqueName == "KTDK")
                {
                    DataColumn col = new DataColumn("KTDK");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Kiểm tra định kỳ", colM = 1, rowM = 1, width = 20 });
                    lstColumn.Add(new ExcelEntity { Name = "KTDK", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MUC") && item.UniqueName == "MUC")
                {
                    DataColumn col = new DataColumn("MUC");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Mức đạt được", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "MUC", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                #endregion
            }
            RadGrid1.AllowPaging = false;
            foreach (GridDataItem item in RadGrid1.Items)
            {
                DataRow row = dt.NewRow();
                TextBox tbNoiDung = (TextBox)item.FindControl("tbNOI_DUNG_NX");
                TextBox tbKTDK = (TextBox)item.FindControl("tbKTDK");
                TextBox tbMUC = (TextBox)item.FindControl("tbMUC");
                foreach (DataColumn col in dt.Columns)
                {
                    row[col.ColumnName] = item[col.ColumnName].Text.Replace("&nbsp;", " ");
                    if (col.ColumnName == "NOI_DUNG_NX") row[col.ColumnName] = tbNoiDung.Text.Trim().TrimEnd(',');
                    if (col.ColumnName == "KTDK") row[col.ColumnName] = tbKTDK.Text.Trim().TrimEnd(',');
                    if (col.ColumnName == "MUC") row[col.ColumnName] = tbMUC.Text.Trim().TrimEnd(',');
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
            string tieuDe = "ĐÁNH GIÁ ĐỊNH KỲ MÔN";
            string hocKyNamHoc = "Lớp " + (rcbLop.Text == "" ? "" : rcbLop.Text) + ", môn " + (localAPI.ConvertStringTolong(rcbMonHoc.SelectedValue) == null ? "" : rcbMonHoc.Text) + ", " + rcbKyDGTH.Text;
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
                TextBox tbNoiDungNX = (TextBox)item.FindControl("tbNOI_DUNG_NX");
                TextBox tbKTDK = (TextBox)item.FindControl("tbKTDK");
                TextBox tbMucDatDuoc = (TextBox)item.FindControl("tbMUC");

                if (checkKhoa)
                {
                    btSave.Visible = false;
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Môn học này đã khóa sổ!');", true);
                }
                else
                {
                    btSave.Visible = true;
                }
                tbNoiDungNX.Enabled = (checkKhoa) ? false : true;
                tbKTDK.Enabled = (checkKhoa) ? false : true;
                tbMucDatDuoc.Enabled = (checkKhoa) ? false : true;
            }
        }
    }
}