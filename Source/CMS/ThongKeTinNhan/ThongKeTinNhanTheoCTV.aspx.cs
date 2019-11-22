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

namespace CMS.ThongKeTinNhan
{
    public partial class ThongKeTinNhanTheoCTV : AuthenticatePage
    {
        private TinNhanBO tinNhanBO = new TinNhanBO();
        private LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                rcbNam.DataBind();
                rcbNam.SelectedValue = DateTime.Now.Year.ToString();
                rcbThang.DataBind();
                rcbThang.SelectedValue = DateTime.Now.Month.ToString();
            }
        }

        protected void btExport_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string serverPath = Server.MapPath("~");
            string path = String.Format("{0}\\ExportTemplates\\FileDynamic.xlsx", Server.MapPath("~"));
            string newName = "Thong_ke_tin_nhan.xlsx";

            List<ExcelHeaderEntity> lstHeader = new List<ExcelHeaderEntity>();
            List<ExcelEntity> lstColumn = new List<ExcelEntity>();
            lstHeader.Add(new ExcelHeaderEntity { name = "STT", colM = 1, rowM = 2, width = 10 });
            List<ExcelHeaderEntity> lstTinLienLac = new List<ExcelHeaderEntity>();
            List<ExcelHeaderEntity> lstTinThongBao = new List<ExcelHeaderEntity>();

            #region add column
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TEN_DANG_NHAP"))
            {
                DataColumn col = new DataColumn("TEN_DANG_NHAP");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Tài khoản", colM = 1, rowM = 2, width = 40 });
                lstColumn.Add(new ExcelEntity { Name = "TEN_DANG_NHAP", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "SO_TIN"))
            {
                DataColumn col = new DataColumn("SO_TIN");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Số tin", colM = 1, rowM = 2, width = 30 });
                lstColumn.Add(new ExcelEntity { Name = "SO_TIN", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "SO_NGAY_GUI"))
            {
                DataColumn col = new DataColumn("SO_NGAY_GUI");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Số ngày gửi", colM = 1, rowM = 2, width = 30 });
                lstColumn.Add(new ExcelEntity { Name = "SO_NGAY_GUI", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TRUNG_BINH"))
            {
                DataColumn col = new DataColumn("TRUNG_BINH");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Số tin trung bình", colM = 1, rowM = 2, width = 30 });
                lstColumn.Add(new ExcelEntity { Name = "TRUNG_BINH", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
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
            int rowStart = 8;
            string colStart = "A";
            string colEnd = localAPI.GetExcelColumnName(lstColumn.Count);
            List<ExcelHeaderEntity> listCell = new List<ExcelHeaderEntity>();
            string tenSo = "";
            string tieuDe = "THỐNG KÊ TIN NHẮN";
            string hocKyNamHoc = "Tháng: " + rcbThang.SelectedValue + " năm " + rcbNam.SelectedValue;
            listCell.Add(new ExcelHeaderEntity { name = tenSo, colM = 3, rowM = 1, colName = "A", rowIndex = 1, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tieuDe, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 3, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            listCell.Add(new ExcelHeaderEntity { name = hocKyNamHoc, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 4, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            string nameFileOutput = newName;
            localAPI.ExportExcelDynamic(serverPath, path, newName, nameFileOutput, 1, listCell, rowHeaderStart, rowStart, colStart, colEnd, dt, lstHeader, lstColumn, true);
        }

        protected void rcbNam_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }

        protected void rcbThang_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            int thang = 0, nam = 0;
            thang = localAPI.ConvertStringToint(rcbThang.SelectedValue).Value;
            nam = localAPI.ConvertStringToint(rcbNam.SelectedValue).Value;
            List<ThongKeTinNhanTheoCongTacVienEntity> lstData = new List<ThongKeTinNhanTheoCongTacVienEntity>();
            lstData = tinNhanBO.thongKeTheoCTV1(nam, thang);
            RadGrid1.DataSource = lstData;
            RadHtmlChart1.DataSource = lstData;
            RadHtmlChart1.ChartTitle.Text = "Biểu đồ thống kê tin nhắn theo tài khoản tháng "+thang.ToString()+ " năm "+nam.ToString();
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
    }
}