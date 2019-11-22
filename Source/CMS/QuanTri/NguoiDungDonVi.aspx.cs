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

namespace CMS.QuanTri
{
    public partial class NguoiDungDonVi : AuthenticatePage
    {
        private NguoiDungBO nguoiDungBO = new NguoiDungBO();
        private LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            btAddNew.Visible = is_access(SYS_Type_Access.THEM);
            btDeleteChon.Visible = is_access(SYS_Type_Access.XOA);
            btnImportExcel.Visible = is_access(SYS_Type_Access.THEM);
            btExport.Visible = is_access(SYS_Type_Access.EXPORT);
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
            RadGrid1.DataSource = nguoiDungBO.getNguoiDungByTruong(Sys_This_Truong.ID, txtUser.Text.Trim());
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
        protected void btDeleteChon_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.XOA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            int success = 0, error = 0;
            foreach (GridDataItem row in RadGrid1.SelectedItems)
            {
                short maNguoiDung = Convert.ToInt16(row.GetDataKeyValue("ID").ToString());
                ResultEntity res = nguoiDungBO.delete(maNguoiDung, Sys_User.ID);
                if (res.Res)
                    success++;
                else
                    error++;
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
        protected void rcbKhoiHoc_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void btTimKiem_Click(object sender, EventArgs e)
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
            string newName = "Danh_sach_nguoi_dung.xlsx";

            List<ExcelHeaderEntity> lstHeader = new List<ExcelHeaderEntity>();
            List<ExcelEntity> lstColumn = new List<ExcelEntity>();
            lstHeader.Add(new ExcelHeaderEntity { name = "STT", colM = 1, rowM = 1, width = 10 });
            foreach (GridColumn item in RadGrid1.MasterTableView.Columns)
            {
                #region add column
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TEN_DANG_NHAP") && item.UniqueName == "TEN_DANG_NHAP")
                {
                    DataColumn col = new DataColumn("TEN_DANG_NHAP");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tên đăng nhập", colM = 1, rowM = 1, width = 30 });
                    lstColumn.Add(new ExcelEntity { Name = "TEN_DANG_NHAP", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TEN_HIEN_THI") && item.UniqueName == "TEN_HIEN_THI")
                {
                    DataColumn col = new DataColumn("TEN_HIEN_THI");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tên hiển thị", colM = 1, rowM = 1, width = 30 });
                    lstColumn.Add(new ExcelEntity { Name = "TEN_HIEN_THI", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "SDT") && item.UniqueName == "SDT")
                {
                    DataColumn col = new DataColumn("SDT");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Số điện thoại", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "SDT", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "QUYEN") && item.UniqueName == "QUYEN")
                {
                    DataColumn col = new DataColumn("QUYEN");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Nhóm quyền", colM = 1, rowM = 1, width = 20 });
                    lstColumn.Add(new ExcelEntity { Name = "QUYEN", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "DIA_CHI") && item.UniqueName == "DIA_CHI")
                {
                    DataColumn col = new DataColumn("DIA_CHI");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Địa chỉ", colM = 1, rowM = 1, width = 60 });
                    lstColumn.Add(new ExcelEntity { Name = "DIA_CHI", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "FACE_BOOK") && item.UniqueName == "FACE_BOOK")
                {
                    DataColumn col = new DataColumn("FACE_BOOK");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Facebook", colM = 1, rowM = 1, width = 60 });
                    lstColumn.Add(new ExcelEntity { Name = "FACE_BOOK", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "EMAIL") && item.UniqueName == "EMAIL")
                {
                    DataColumn col = new DataColumn("EMAIL");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Email", colM = 1, rowM = 1, width = 60 });
                    lstColumn.Add(new ExcelEntity { Name = "EMAIL", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                #endregion
            }
            RadGrid1.AllowPaging = false;
            if (RadGrid1.SelectedItems == null || RadGrid1.SelectedItems.Count == 0)
            {
                foreach (GridDataItem item in RadGrid1.Items)
                {
                    DataRow row = dt.NewRow();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row[col.ColumnName] = item[col.ColumnName].Text.Replace("&nbsp;", " ");
                    }
                    dt.Rows.Add(row);
                }
            }
            else
            {
                foreach (GridDataItem item in RadGrid1.SelectedItems)
                {
                    DataRow row = dt.NewRow();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row[col.ColumnName] = item[col.ColumnName].Text.Replace("&nbsp;", " ");
                    }
                    dt.Rows.Add(row);
                }
            }
            int rowHeaderStart = 6;
            int rowStart = 7;
            string colStart = "A";
            string colEnd = localAPI.GetExcelColumnName(lstColumn.Count);
            List<ExcelHeaderEntity> listCell = new List<ExcelHeaderEntity>();
            string tenSo = "";
            string tenPhong = Sys_This_Truong.TEN;
            string tieuDe = "DANH SÁCH NGƯỜI DÙNG";
            string hocKyNamHoc = "";
            listCell.Add(new ExcelHeaderEntity { name = tenSo, colM = 3, rowM = 1, colName = "A", rowIndex = 1, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tenPhong, colM = 3, rowM = 1, colName = "A", rowIndex = 2, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tieuDe, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 3, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            listCell.Add(new ExcelHeaderEntity { name = hocKyNamHoc, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 4, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            string nameFileOutput = newName;
            localAPI.ExportExcelDynamic(serverPath, path, newName, nameFileOutput, 1, listCell, rowHeaderStart, rowStart, colStart, colEnd, dt, lstHeader, lstColumn, true);
        }
    }
}