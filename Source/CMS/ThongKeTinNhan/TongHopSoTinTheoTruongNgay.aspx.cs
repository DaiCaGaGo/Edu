using ClosedXML.Excel;
using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CMS.ThongKeTinNhan
{
    public partial class TongHopSoTinTheoTruongNgay : AuthenticatePage
    {
        TinNhanBO tinNhanBO = new TinNhanBO();
        LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rcbTruong.DataSource = Sys_List_Truong;
                rcbTruong.DataBind();
                rdTuNgay.SelectedDate = DateTime.Now;
                rdDenNgay.SelectedDate = DateTime.Now;
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
            var date_to = rdTuNgay.SelectedDate.Value.ToString("yyyyMMdd");
            var date_from = rdDenNgay.SelectedDate.Value.ToString("yyyyMMdd");
            List<TongHopSoTinTheoTruongNgayEntity> lst = tinNhanBO.tongTinDaGuiTheoTruongAndNgay(localAPI.ConvertStringTolong(rcbTruong.SelectedValue), date_to, date_from);
            #region add sum row
            long? tong_tin = 0, thanh_cong = 0, gui_loi = 0, dung_gui = 0;
            if (lst.Count > 0)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    tong_tin += lst[i].TONG_TIN;
                    thanh_cong += lst[i].TIN_THANH_CONG;
                    gui_loi += lst[i].TIN_GUI_LOI;
                    dung_gui += lst[i].TIN_DUNG_GUI;
                }
            }
            if (lst.Count == 0 || (lst.Count > 0 && lst[0].ID_TRUONG != 0))
            {
                TongHopSoTinTheoTruongNgayEntity detail = new TongHopSoTinTheoTruongNgayEntity();
                detail.ID_TRUONG = 0;
                detail.TONG_TIN = tong_tin;
                detail.TIN_THANH_CONG = thanh_cong;
                detail.TIN_GUI_LOI = gui_loi;
                detail.TIN_DUNG_GUI = dung_gui;
                lst.Insert(0, detail);
            }
            #endregion
            RadGrid1.DataSource = lst;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
        protected void rcbTruong_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void rdTuNgay_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            checkNgay();
            RadGrid1.Rebind();
        }
        protected void rdDenNgay_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            checkNgay();
            RadGrid1.Rebind();
        }
        protected void btExport_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string serverPath = Server.MapPath("~");
            string path = String.Format("{0}\\ExportTemplates\\FileDynamic.xlsx", Server.MapPath("~"));
            string newName = "TongTinDaGui.xlsx";

            List<ExcelHeaderEntity> lstHeader = new List<ExcelHeaderEntity>();
            List<ExcelEntity> lstColumn = new List<ExcelEntity>();
            lstHeader.Add(new ExcelHeaderEntity { name = "STT", colM = 1, rowM = 1, width = 10 });
            #region add column
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TEN"))
            {
                DataColumn col = new DataColumn("TEN");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Tên trường", colM = 1, rowM = 1, width = 60 });
                lstColumn.Add(new ExcelEntity { Name = "TEN", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TONG_TIN"))
            {
                DataColumn col = new DataColumn("TONG_TIN");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Tổng tin đã gửi", colM = 1, rowM = 1, width = 30 });
                lstColumn.Add(new ExcelEntity { Name = "TONG_TIN", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
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
            string tenPhong = "";
            string tieuDe = "THỐNG KÊ TIN NHẮN " + (
                rdTuNgay.SelectedDate == rdDenNgay.SelectedDate ? ("NGÀY " + rdTuNgay.SelectedDate.Value.ToString("dd/MM/yyyy")) :
                ("(" + rdTuNgay.SelectedDate.Value.ToString("dd/MM/yyyy") + " - " + rdDenNgay.SelectedDate.Value.ToString("dd/MM/yyyy") + ")"));
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
                long id_truong = Convert.ToInt64(item.GetDataKeyValue("ID_TRUONG").ToString());
                if (id_truong == 0)
                {
                    item["TEN"].Text = "Tổng";
                    item.Font.Bold = true;
                    item.ForeColor = Color.Red;
                }
            }
        }
        protected void checkNgay()
        {
            if (rdTuNgay.SelectedDate > rdDenNgay.SelectedDate)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Ngày bắt đầu phải nhỏ hơn ngày kết thúc');", true);
                return;
            }
        }
    }
}