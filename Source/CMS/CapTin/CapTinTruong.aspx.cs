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

namespace CMS.CapTin
{
    public partial class CapTinTruong : AuthenticatePage
    {
        LocalAPI localAPI = new LocalAPI();
        CapTinTruongBO capTinTruongBO = new CapTinTruongBO();
        DoiTacBO doiTacBO = new DoiTacBO();
        TruongBO truongBO = new TruongBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                checkChonTruong();
                objTruong.SelectParameters.Add("id_doi_tac", Sys_This_Truong.ID_DOI_TAC.ToString());
                rcbTruong.DataBind();
                rcbTruong.SelectedValue = Sys_This_Truong.ID.ToString();
                rcbTruong.Enabled = false;
                tongTinCapTruong();
            }
        }
        protected void tongTinCapTruong()
        {
            DOI_TAC doiTac = new DOI_TAC();
            if (Sys_This_Truong.ID_DOI_TAC != null)
            {
                doiTac = doiTacBO.getDoiTacByID(Sys_This_Truong.ID_DOI_TAC.Value);
                long tong_dai_ly = 0;
                if (doiTac != null && doiTac.TONG_TIN_CAP != null)
                    tong_dai_ly = doiTac.TONG_TIN_CAP.Value;
                long tong_tin_da_cap = 0;
                List<TruongEntity> lstTruong = truongBO.getTruongByDoiTac(Sys_This_Truong.ID_DOI_TAC.Value);
                if (lstTruong != null && lstTruong.Count > 0)
                {
                    for (int i = 0; i < lstTruong.Count; i++)
                    {
                        tong_tin_da_cap += lstTruong[i].TONG_TIN_CAP != null ? lstTruong[i].TONG_TIN_CAP.Value : 0;
                    }
                }
                lblTongTinCapTruong.Text = "Lượng tin còn được cấp: " + (tong_dai_ly - tong_tin_da_cap);
            }
        }
        protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
        {
            if (e.Argument == "RadWindowManager1_Close")
            {
                RadGrid1.Rebind();
                tongTinCapTruong();
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            short? id_doi_tac = Sys_This_Truong.ID_DOI_TAC;
            if (id_doi_tac != null)
            {
                RadGrid1.DataSource = capTinTruongBO.getCapTinTruong(id_doi_tac.Value, localAPI.ConvertStringToShort(rcbTruong.SelectedValue));
                btAddNew.Visible = is_access(SYS_Type_Access.THEM);
                btExport.Visible = is_access(SYS_Type_Access.EXPORT);
            }
            else
            {
                btAddNew.Visible = false;
                btExport.Visible = false;
            }
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
            string newName = "Cap_tin_truong.xlsx";

            List<ExcelHeaderEntity> lstHeader = new List<ExcelHeaderEntity>();
            List<ExcelEntity> lstColumn = new List<ExcelEntity>();
            lstHeader.Add(new ExcelHeaderEntity { name = "STT", colM = 1, rowM = 1, width = 10 });
            #region add column
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TEN_TRUONG"))
            {
                DataColumn col = new DataColumn("TEN_TRUONG");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Tên trường", colM = 1, rowM = 1, width = 10 });
                lstColumn.Add(new ExcelEntity { Name = "TEN_TRUONG", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
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
            string tieuDe = "Danh cách cấp tin trường";
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
                short? id_truong = localAPI.ConvertStringToShort(item["ID_TRUONG"].Text);
                TruongBO truongBO = new TruongBO();
                if (id_truong != null)
                    item["TEN_TRUONG"].Text = truongBO.getTruong("", "", "", null).FirstOrDefault(x => x.ID == id_truong.Value).TEN;
            }
        }
        protected void rcbTruong_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
    }
}