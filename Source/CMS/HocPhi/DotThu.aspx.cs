using ClosedXML.Excel;
using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CMS.HocPhi
{
    public partial class DotThu : AuthenticatePage
    {
        LocalAPI localAPI = new LocalAPI();
        LogUserBO logUserBO = new LogUserBO();
        HocPhiDotThuBO dotThuBO = new HocPhiDotThuBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            btAddNew.Visible = is_access(SYS_Type_Access.THEM);
            btDeleteChon.Visible = is_access(SYS_Type_Access.XOA);
            btExport.Visible = is_access(SYS_Type_Access.EXPORT);
            if (!IsPostBack)
            {
                rcbHocKy.Enabled = false;
                rcbThang.Enabled = false;
            }
        }
        protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
        {
            if (e.Argument == "RadWindowManager1_Close")
            {
                RadGrid1.Rebind();
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = dotThuBO.getDotThuByOther(Sys_This_Truong.ID, Convert.ToInt16(Sys_Ma_Nam_hoc), localAPI.ConvertStringToShort(rcbLoaiKhoanThu.SelectedValue), localAPI.ConvertStringToShort(rcbHocKy.SelectedValue), localAPI.ConvertStringToShort(rcbThang.SelectedValue), tbTen.Text.Trim());
            if (is_access(SYS_Type_Access.XEM) || is_access(SYS_Type_Access.SUA))
                RadGrid1.MasterTableView.Columns.FindByUniqueName("SUA").Visible = true;
            else RadGrid1.MasterTableView.Columns.FindByUniqueName("SUA").Visible = false;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
        protected void rcbLoaiKhoanThu_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            short? loai_khoan_thu = localAPI.ConvertStringToShort(rcbLoaiKhoanThu.SelectedValue);
            if (loai_khoan_thu == 2)
            {
                rcbHocKy.Enabled = true;
                rcbThang.Enabled = false;
                rcbThang.ClearSelection();
                rcbThang.Text = string.Empty;
            }
            else if (loai_khoan_thu == 3)
            {
                rcbHocKy.Enabled = false;
                rcbThang.Enabled = true;
                rcbHocKy.ClearSelection();
                rcbHocKy.Text = string.Empty;
            }
            else
            {
                rcbHocKy.Enabled = false;
                rcbThang.Enabled = false;
                rcbHocKy.ClearSelection();
                rcbThang.ClearSelection();
                rcbHocKy.Text = string.Empty;
                rcbThang.Text = string.Empty;
            }
            RadGrid1.Rebind();
        }
        protected void rcbHocKy_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void rcbThang_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void btTimKiem_Click(object sender, EventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void btDeleteChon_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.XOA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            int success = 0, error = 0; string lst_id = string.Empty;
            foreach (GridDataItem row in RadGrid1.SelectedItems)
            {
                try
                {
                    long id = Convert.ToInt16(row.GetDataKeyValue("ID").ToString());
                    string ten = row["TEN"].Text;
                    try
                    {
                        ResultEntity res = dotThuBO.delete(id, Sys_User.ID, true);
                        lst_id += id + ":" + ten + ", ";
                        if (res.Res)
                        {
                            success++;
                            logUserBO.insert(Sys_This_Truong.ID, "DELETE", "Xóa đợt thu " + ten + " (" + id + ")", Sys_User.ID, DateTime.Now);
                        }
                        else
                            error++;
                    }
                    catch
                    {
                        error++;
                    }
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Lỗi hệ thống. Bạn vui lòng kiểm tra lại');", true);
                }
            }
            string strMsg = "";
            if (error > 0)
            {
                strMsg = "notification('error', 'Có " + error + " bản ghi chưa được xóa. Liên hệ với quản trị viên');";
            }
            if (success > 0)
            {
                strMsg += " notification('success', 'Có " + success + " bản ghi được xóa.');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
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
            string newName = "Dot_thu.xlsx";

            List<ExcelHeaderEntity> lstHeader = new List<ExcelHeaderEntity>();
            List<ExcelEntity> lstColumn = new List<ExcelEntity>();
            lstHeader.Add(new ExcelHeaderEntity { name = "STT", colM = 1, rowM = 1, width = 10 });
            #region add column
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TEN"))
            {
                DataColumn col = new DataColumn("TEN");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Tên đợt thu", colM = 1, rowM = 1, width = 60 });
                lstColumn.Add(new ExcelEntity { Name = "TEN", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DOT_THU"))
            {
                DataColumn col = new DataColumn("DOT_THU");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Đợt thu", colM = 1, rowM = 1, width = 40 });
                lstColumn.Add(new ExcelEntity { Name = "DOT_THU", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "THOI_GIAN"))
            {
                DataColumn col = new DataColumn("THOI_GIAN");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Thời gian thu", colM = 1, rowM = 1, width = 40 });
                lstColumn.Add(new ExcelEntity { Name = "THOI_GIAN", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "GHI_CHU"))
            {
                DataColumn col = new DataColumn("GHI_CHU");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Nội dung thu", colM = 1, rowM = 1, width = 80 });
                lstColumn.Add(new ExcelEntity { Name = "GHI_CHU", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_TIEN_AN"))
            {
                DataColumn col = new DataColumn("IS_TIEN_AN");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Thu tiền ăn", colM = 1, rowM = 1, width = 15 });
                lstColumn.Add(new ExcelEntity { Name = "IS_TIEN_AN", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "check" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TONG_TIEN"))
            {
                DataColumn col = new DataColumn("TONG_TIEN");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Tổng tiền (VNĐ)", colM = 1, rowM = 1, width = 15 });
                lstColumn.Add(new ExcelEntity { Name = "TONG_TIEN", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "THU_TU"))
            {
                DataColumn col = new DataColumn("THU_TU");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Thứ tự", colM = 1, rowM = 1, width = 10 });
                lstColumn.Add(new ExcelEntity { Name = "THU_TU", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            #endregion
            RadGrid1.AllowPaging = false;
            var lstGrid = RadGrid1.SelectedItems;
            if (lstGrid == null || lstGrid.Count == 0) lstGrid = RadGrid1.Items;
            foreach (GridDataItem item in lstGrid)
            {
                DataRow row = dt.NewRow();
                foreach (DataColumn col in dt.Columns)
                {
                    row[col.ColumnName] = item[col.ColumnName].Text.Replace("&nbsp;", " ");
                    if (col.ColumnName.IndexOf("IS_") == 0)
                    {
                        CheckBox check = (CheckBox)item[col.ColumnName].Controls[0];
                        row[col.ColumnName] = check.Checked;
                    }
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
            string tieuDe = "Danh mục đợt thu";
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
                #region đợt thu
                short? id_dot_thu = localAPI.ConvertStringToShort(item["ID_DOT_THU"].Text);
                if (id_dot_thu == 1) item["DOT_THU"].Text = "Theo năm";
                else if (id_dot_thu == 2)
                {
                    short? hoc_ky = localAPI.ConvertStringToShort(item["HOC_KY"].Text);
                    if (hoc_ky == 1) item["DOT_THU"].Text = "Học kỳ 1";
                    else if (hoc_ky == 2) item["DOT_THU"].Text = "Học kỳ 2";
                }
                else if (id_dot_thu == 3)
                {
                    short? thang = localAPI.ConvertStringToShort(item["THANG"].Text);
                    item["DOT_THU"].Text = thang != null ? "Tháng " + thang : "";
                }
                #endregion
                #region thời gian thu
                string thoi_gian_bat_dau = "", thoi_gian_ket_thuc = "";
                if (item["THOI_GIAN_BAT_DAU"].Text != "&nbsp;")
                    thoi_gian_bat_dau = Convert.ToDateTime(item["THOI_GIAN_BAT_DAU"].Text).ToString("dd/MM/yyyy");
                if (item["THOI_GIAN_KET_THUC"].Text != "&nbsp;")
                    thoi_gian_ket_thuc = Convert.ToDateTime(item["THOI_GIAN_KET_THUC"].Text).ToString("dd/MM/yyyy");
                if (thoi_gian_bat_dau != "" || thoi_gian_ket_thuc != "")
                    item["THOI_GIAN"].Text = thoi_gian_bat_dau + " - " + thoi_gian_ket_thuc;
                #endregion
            }
        }
    }
}