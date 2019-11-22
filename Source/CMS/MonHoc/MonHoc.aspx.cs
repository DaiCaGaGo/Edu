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

namespace CMS.MonHoc
{
    public partial class MonHoc : AuthenticatePage
    {
        MonHocBO monBO = new MonHocBO();
        LocalAPI localAPI = new LocalAPI();
        LogUserBO logUserBO = new LogUserBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rcbKhoi.DataBind();
            }
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
            short idKhoi = 0;
            if (rcbKhoi.SelectedValue != "") idKhoi = Convert.ToInt16(rcbKhoi.SelectedValue);

            RadGrid1.DataSource = monBO.getMonHocByKhoiTen(rcbCapHoc.SelectedValue, idKhoi, tbTen.Text.Trim());
            if (rcbCapHoc.SelectedValue == "TH")
            {
                for (int i = 1; i <= 12; i++)
                {
                    if (i >= 6)
                        RadGrid1.Columns.FindByUniqueName("IS_" + i).Display = false;
                    else RadGrid1.Columns.FindByUniqueName("IS_" + i).Display = true;
                }
            }
            else if (rcbCapHoc.SelectedValue == "THCS")
            {
                for (int i = 1; i <= 12; i++)
                {
                    if (i < 6 || i > 9) RadGrid1.Columns.FindByUniqueName("IS_" + i).Display = false;
                    else RadGrid1.Columns.FindByUniqueName("IS_" + i).Display = true;
                }
            }
            else if (rcbCapHoc.SelectedValue == "THPT")
            {
                for (int i = 1; i <= 12; i++)
                {
                    if (i < 10)
                        RadGrid1.Columns.FindByUniqueName("IS_" + i).Display = false;
                    else RadGrid1.Columns.FindByUniqueName("IS_" + i).Display = true;
                }
            }
            RadGrid1.MasterTableView.Columns.FindByUniqueName("SUA").Display = (is_access(SYS_Type_Access.XEM) || is_access(SYS_Type_Access.SUA));
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
                    short id_mon = Convert.ToInt16(row.GetDataKeyValue("ID").ToString());
                    string ten = row["TEN"].Text;
                    try
                    {
                        ResultEntity res = monBO.delete(id_mon, Sys_User.ID);
                        lst_id += id_mon + ":" + ten + ", ";
                        if (res.Res)
                        {
                            success++;
                            logUserBO.insert(Sys_This_Truong.ID, "DELETE", "Xóa môn học " + ten + " (" + id_mon + ")", Sys_User.ID, DateTime.Now);
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
        protected void btTimKiem_Click(object sender, EventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void rcbKhoi_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void rcbCapHoc_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbKhoi.ClearSelection();
            rcbKhoi.Text = string.Empty;
            rcbKhoi.DataBind();
            RadGrid1.Rebind();
        }
        protected void btExport_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string serverPath = Server.MapPath("~");
            string path = String.Format("{0}\\ExportTemplates\\FileDynamic.xlsx", Server.MapPath("~"));
            string newName = "Danh_sach_mon_hoc.xlsx";

            List<ExcelHeaderEntity> lstHeader = new List<ExcelHeaderEntity>();
            List<ExcelEntity> lstColumn = new List<ExcelEntity>();
            lstHeader.Add(new ExcelHeaderEntity { name = "STT", colM = 1, rowM = 1, width = 10 });
            foreach (GridColumn item in RadGrid1.MasterTableView.Columns)
            {
                #region add column
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TEN") && item.UniqueName == "TEN")
                {
                    DataColumn col = new DataColumn("TEN");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tên", colM = 1, rowM = 1, width = 30 });
                    lstColumn.Add(new ExcelEntity { Name = "TEN", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "KIEU_MON_STR") && item.UniqueName == "KIEU_MON_STR")
                {
                    DataColumn col = new DataColumn("KIEU_MON_STR");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Kiểu môn", colM = 1, rowM = 1, width = 30 });
                    lstColumn.Add(new ExcelEntity { Name = "KIEU_MON_STR", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "HE_SO") && item.UniqueName == "HE_SO")
                {
                    DataColumn col = new DataColumn("HE_SO");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Hệ số", colM = 1, rowM = 1, width = 12 });
                    lstColumn.Add(new ExcelEntity { Name = "HE_SO", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "THU_TU") && item.UniqueName == "THU_TU")
                {
                    DataColumn col = new DataColumn("THU_TU");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Thứ tự", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "THU_TU", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_1") && item.UniqueName == "IS_1")
                {
                    DataColumn col = new DataColumn("IS_1");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Khối 1", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "IS_1", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "check" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_2") && item.UniqueName == "IS_2")
                {
                    DataColumn col = new DataColumn("IS_2");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Khối 2", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "IS_2", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "check" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_3") && item.UniqueName == "IS_3")
                {
                    DataColumn col = new DataColumn("IS_3");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Khối 3", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "IS_3", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "check" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_4") && item.UniqueName == "IS_4")
                {
                    DataColumn col = new DataColumn("IS_4");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Khối 4", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "IS_4", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "check" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_5") && item.UniqueName == "IS_5")
                {
                    DataColumn col = new DataColumn("IS_5");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Khối 5", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "IS_5", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "check" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_6") && item.UniqueName == "IS_6")
                {
                    DataColumn col = new DataColumn("IS_6");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Khối 6", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "IS_6", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "check" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_7") && item.UniqueName == "IS_7")
                {
                    DataColumn col = new DataColumn("IS_7");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Khối 7", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "IS_7", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "check" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_8") && item.UniqueName == "IS_8")
                {
                    DataColumn col = new DataColumn("IS_8");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Khối 8", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "IS_8", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "check" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_9") && item.UniqueName == "IS_9")
                {
                    DataColumn col = new DataColumn("IS_9");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Khối 9", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "IS_9", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "check" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_10") && item.UniqueName == "IS_10")
                {
                    DataColumn col = new DataColumn("IS_10");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Khối 10", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "IS_10", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "check" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_11") && item.UniqueName == "IS_11")
                {
                    DataColumn col = new DataColumn("IS_11");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Khối 10", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "IS_11", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "check" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_12") && item.UniqueName == "IS_12")
                {
                    DataColumn col = new DataColumn("IS_12");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Khối 12", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "IS_12", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "check" });
                }
                #endregion
            }
            RadGrid1.AllowPaging = false;
            GridItemCollection lstGrid = new GridItemCollection();
            if (RadGrid1.SelectedItems != null && RadGrid1.SelectedItems.Count > 0)
                lstGrid = RadGrid1.SelectedItems;
            else lstGrid = RadGrid1.Items;
            foreach( GridDataItem item in lstGrid)
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
            string tieuDe = "DANH SÁCH MÔN HỌC";
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