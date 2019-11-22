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

namespace CMS.Truong
{
    public partial class Truong : AuthenticatePage
    {
        private TruongBO truongBO = new TruongBO();
        LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            btAddNew.Visible = is_access(SYS_Type_Access.THEM);
            btDeleteChon.Visible = is_access(SYS_Type_Access.XOA);
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
            bool? trang_thai = null;
            if (rcbTrangThai.SelectedValue == "1") trang_thai = true;
            else if (rcbTrangThai.SelectedValue == "0") trang_thai = false;
            RadGrid1.DataSource = truongBO.getTruong(rcbCapHoc.SelectedValue, rtbTenTruong.Text.Trim(), rtbMa.Text.Trim(), trang_thai);
            RadGrid1.MasterTableView.Columns.FindByUniqueName("SUA").Visible = (is_access(SYS_Type_Access.XEM) || is_access(SYS_Type_Access.SUA));
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
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
                    short ma = Convert.ToInt16(row.GetDataKeyValue("ID").ToString());
                    string ten = row["TEN"].Text;
                    try
                    {
                        ResultEntity res = truongBO.delete(ma, Sys_User.ID, true);
                        lst_id += ma + ":" + ten + ", ";
                        if (res.Res)
                            success++;
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
        protected void btTimKiem_Click(object sender, EventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void rcbCapHoc_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void rcbTrangThai_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void btExport_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string serverPath = Server.MapPath("~");
            string path = String.Format("{0}\\ExportTemplates\\FileDynamic.xlsx", Server.MapPath("~"));
            string newName = "Danh_sach_truong_hoc.xlsx";

            List<ExcelHeaderEntity> lstHeader = new List<ExcelHeaderEntity>();
            List<ExcelEntity> lstColumn = new List<ExcelEntity>();
            lstHeader.Add(new ExcelHeaderEntity { name = "STT", colM = 1, rowM = 1, width = 10 });
            foreach (GridColumn item in RadGrid1.MasterTableView.Columns)
            {
                #region add column
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MA") && item.UniqueName == "MA")
                {
                    DataColumn col = new DataColumn("MA");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Mã trường", colM = 1, rowM = 1, width = 20});
                    lstColumn.Add(new ExcelEntity { Name = "MA", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TEN") && item.UniqueName == "TEN")
                {
                    DataColumn col = new DataColumn("TEN");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tên trường", colM = 1, rowM = 1, width = 60 });
                    lstColumn.Add(new ExcelEntity { Name = "TEN", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "BRAND_NAME_VIETTEL") && item.UniqueName == "BRAND_NAME_VIETTEL")
                {
                    DataColumn col = new DataColumn("BRAND_NAME_VIETTEL");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tên thương hiệu", colM = 1, rowM = 1, width = 30 });
                    lstColumn.Add(new ExcelEntity { Name = "BRAND_NAME_VIETTEL", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_MN") && item.UniqueName == "IS_MN")
                {
                    DataColumn col = new DataColumn("IS_MN");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Cấp MN", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "IS_MN", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "check" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_TH") && item.UniqueName == "IS_TH")
                {
                    DataColumn col = new DataColumn("IS_TH");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Cấp TH", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "IS_TH", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "check" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_THCS") && item.UniqueName == "IS_THCS")
                {
                    DataColumn col = new DataColumn("IS_THCS");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Cấp THCS", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "IS_THCS", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "check" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_THPT") && item.UniqueName == "IS_THPT")
                {
                    DataColumn col = new DataColumn("IS_THPT");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Cấp THPT", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "IS_THPT", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "check" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_GDTX") && item.UniqueName == "IS_GDTX")
                {
                    DataColumn col = new DataColumn("IS_GDTX");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Cấp GDTX", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "IS_GDTX", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "check" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TRANG_THAI") && item.UniqueName == "TRANG_THAI")
                {
                    DataColumn col = new DataColumn("TRANG_THAI");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Trạng thái", colM = 1, rowM = 1, width = 20 });
                    lstColumn.Add(new ExcelEntity { Name = "TRANG_THAI", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "string" });
                }
                #endregion
            }
            RadGrid1.AllowPaging = false;
            GridItemCollection lstGrid = new GridItemCollection();
            if (RadGrid1.SelectedItems != null && RadGrid1.SelectedItems.Count > 0)
                lstGrid = RadGrid1.SelectedItems;
            else lstGrid = RadGrid1.Items;
            foreach(GridDataItem item in lstGrid)
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
            string tenPhong = "";
            string tieuDe = "DANH SÁCH TRƯỜNG HỌC";
            string hocKyNamHoc = "";
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
                string ma_trang_thai = item["TRANG_THAI"].Text;
                if (ma_trang_thai == "&nbsp;") ma_trang_thai = "";
                if (ma_trang_thai != "")
                    item["TRANG_THAI"].Text = ma_trang_thai == "True" ? "Hoạt động" : ma_trang_thai == "False" ? "Dừng hoạt động" : "";
            }
        }
    }
}