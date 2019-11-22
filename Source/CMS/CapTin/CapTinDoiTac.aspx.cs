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

namespace CMS.CapTin
{
    public partial class CapTinDoiTac : AuthenticatePage
    {
        LocalAPI localAPI = new LocalAPI();
        CapTinDoiTacBO capTindoiTacBO = new CapTinDoiTacBO();
        DoiTacBO doiTacBO = new DoiTacBO();
        NguoiDungBO nguoiDungBO = new NguoiDungBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            btAddNew.Visible = is_access(SYS_Type_Access.THEM);
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
            RadGrid1.DataSource = capTindoiTacBO.getDoiTacDuocCapTin(localAPI.ConvertStringToShort(rcbDaiLy.SelectedValue));
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
            string newName = "Cap_tin_doi_tac.xlsx";

            List<ExcelHeaderEntity> lstHeader = new List<ExcelHeaderEntity>();
            List<ExcelEntity> lstColumn = new List<ExcelEntity>();
            lstHeader.Add(new ExcelHeaderEntity { name = "STT", colM = 1, rowM = 1, width = 10 });
            #region add column
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TEN"))
            {
                DataColumn col = new DataColumn("TEN");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "TEN", colM = 1, rowM = 1, width = 10 });
                lstColumn.Add(new ExcelEntity { Name = "TEN", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "SO_TIN_CAP"))
            {
                DataColumn col = new DataColumn("SO_TIN_CAP");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Số tin cấp", colM = 1, rowM = 1, width = 100 });
                lstColumn.Add(new ExcelEntity { Name = "SO_TIN_CAP", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "NGAY_CAP"))
            {
                DataColumn col = new DataColumn("NGAY_CAP");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Ngày cấp", colM = 1, rowM = 1, width = 100 });
                lstColumn.Add(new ExcelEntity { Name = "NGAY_CAP", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            #endregion
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
            string tieuDe = "Danh cách cấp tin đại lý";
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
                short? id_doi_tac = localAPI.ConvertStringToShort(item["ID_DOI_TAC"].Text);
                if (id_doi_tac != null)
                    item["TEN"].Text = doiTacBO.getDoiTac().FirstOrDefault(x => x.ID == id_doi_tac.Value).TEN;
                long? id_nguoi_cap = localAPI.ConvertStringTolong(item["NGUOI_CAP"].Text);

                if (id_nguoi_cap != null)
                    item["NGUOI_CAP"].Text = nguoiDungBO.getNguoiDungByID(id_nguoi_cap.Value).TEN_DANG_NHAP;
            }
        }
        protected void rcbDaiLy_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
    }
}