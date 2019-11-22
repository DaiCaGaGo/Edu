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

namespace CMS.CauHinhCaHoc
{
    public partial class CauHinhCaHoc : AuthenticatePage
    {
        private CauHinhCaHocBO cauHinhCaHocBO = new CauHinhCaHocBO();
        LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
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
            RadGrid1.DataSource = cauHinhCaHocBO.getCauHinhCaHocByTruong(Convert.ToInt64(Sys_This_Truong.ID));
            if (is_access(SYS_Type_Access.XEM) || is_access(SYS_Type_Access.SUA))
                RadGrid1.MasterTableView.Columns.FindByUniqueName("SUA").Visible = true;
            else RadGrid1.MasterTableView.Columns.FindByUniqueName("SUA").Visible = false;
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
                    string ten = row["NGAY_BAT_DAU"].Text;
                    try
                    {
                        ResultEntity res = cauHinhCaHocBO.delete(ma, Sys_User.ID);
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
        protected void btExport_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string serverPath = Server.MapPath("~");
            string path = String.Format("{0}\\ExportTemplates\\FileDynamic.xlsx", Server.MapPath("~"));
            string newName = "Danh_sach_ca_hoc.xlsx";

            List<ExcelHeaderEntity> lstHeader = new List<ExcelHeaderEntity>();
            List<ExcelEntity> lstColumn = new List<ExcelEntity>();
            lstHeader.Add(new ExcelHeaderEntity { name = "STT", colM = 1, rowM = 1, width = 10 });
            foreach (GridColumn item in RadGrid1.MasterTableView.Columns)
            {
                #region add column
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "NGAY_BAT_DAU") && item.UniqueName == "NGAY_BAT_DAU")
                {
                    DataColumn col = new DataColumn("NGAY_BAT_DAU");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Ngày bắt đầu", colM = 1, rowM = 1, width = 30 });
                    lstColumn.Add(new ExcelEntity { Name = "NGAY_BAT_DAU", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "NGAY_KET_THUC") && item.UniqueName == "NGAY_KET_THUC")
                {
                    DataColumn col = new DataColumn("NGAY_KET_THUC");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Ngày kết thúc", colM = 1, rowM = 1, width = 30 });
                    lstColumn.Add(new ExcelEntity { Name = "NGAY_KET_THUC", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "THOI_GIAN_TIET1") && item.UniqueName == "THOI_GIAN_TIET1")
                {
                    DataColumn col = new DataColumn("THOI_GIAN_TIET1");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tiết 1", colM = 1, rowM = 1, width = 12 });
                    lstColumn.Add(new ExcelEntity { Name = "THOI_GIAN_TIET1", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "THOI_GIAN_TIET2") && item.UniqueName == "THOI_GIAN_TIET2")
                {
                    DataColumn col = new DataColumn("THOI_GIAN_TIET2");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tiết 2", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "THOI_GIAN_TIET2", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "THOI_GIAN_TIET3") && item.UniqueName == "THOI_GIAN_TIET3")
                {
                    DataColumn col = new DataColumn("THOI_GIAN_TIET3");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tiết 3", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "THOI_GIAN_TIET3", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "THOI_GIAN_TIET4") && item.UniqueName == "THOI_GIAN_TIET4")
                {
                    DataColumn col = new DataColumn("THOI_GIAN_TIET4");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tiết 4", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "THOI_GIAN_TIET4", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "THOI_GIAN_TIET5") && item.UniqueName == "THOI_GIAN_TIET5")
                {
                    DataColumn col = new DataColumn("THOI_GIAN_TIET5");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tiết 5", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "THOI_GIAN_TIET5", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "THOI_GIAN_TIET6") && item.UniqueName == "THOI_GIAN_TIET6")
                {
                    DataColumn col = new DataColumn("THOI_GIAN_TIET6");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tiết 6", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "THOI_GIAN_TIET6", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "check" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "THOI_GIAN_TIET7") && item.UniqueName == "THOI_GIAN_TIET7")
                {
                    DataColumn col = new DataColumn("THOI_GIAN_TIET7");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tiết 7", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "THOI_GIAN_TIET7", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "THOI_GIAN_TIET8") && item.UniqueName == "THOI_GIAN_TIET8")
                {
                    DataColumn col = new DataColumn("THOI_GIAN_TIET8");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tiết 8", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "THOI_GIAN_TIET8", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "THOI_GIAN_TIET9") && item.UniqueName == "THOI_GIAN_TIET9")
                {
                    DataColumn col = new DataColumn("THOI_GIAN_TIET9");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tiết 9", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "THOI_GIAN_TIET9", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "THOI_GIAN_TIET10") && item.UniqueName == "THOI_GIAN_TIET10")
                {
                    DataColumn col = new DataColumn("THOI_GIAN_TIET10");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tiết 10", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "THOI_GIAN_TIET10", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "THOI_GIAN_TIET11") && item.UniqueName == "THOI_GIAN_TIET11")
                {
                    DataColumn col = new DataColumn("THOI_GIAN_TIET11");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tiết 11", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "THOI_GIAN_TIET11", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "THOI_GIAN_TIET12") && item.UniqueName == "THOI_GIAN_TIET12")
                {
                    DataColumn col = new DataColumn("THOI_GIAN_TIET12");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tiết 12", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "THOI_GIAN_TIET12", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "THOI_GIAN_TIET13") && item.UniqueName == "THOI_GIAN_TIET13")
                {
                    DataColumn col = new DataColumn("THOI_GIAN_TIET13");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tiết 13", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "THOI_GIAN_TIET13", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "THOI_GIAN_TIET14") && item.UniqueName == "THOI_GIAN_TIET14")
                {
                    DataColumn col = new DataColumn("THOI_GIAN_TIET14");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tiết 14", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "THOI_GIAN_TIET14", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "THOI_GIAN_TIET15") && item.UniqueName == "THOI_GIAN_TIET15")
                {
                    DataColumn col = new DataColumn("THOI_GIAN_TIET15");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tiết 15", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "THOI_GIAN_TIET15", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
                }
                
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MUA") && item.UniqueName == "MUA")
                {
                    DataColumn col = new DataColumn("MUA");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tên cấu hình", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "MUA", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
                }
                #endregion
            }
            RadGrid1.AllowPaging = false;
            foreach (GridDataItem item in RadGrid1.Items)
            {
                DataRow row = dt.NewRow();
                foreach (DataColumn col in dt.Columns)
                {
                    row[col.ColumnName] = item[col.ColumnName].Text.Replace("&nbsp;", " ");
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
            string tieuDe = "DANH SÁCH CA HỌC";
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