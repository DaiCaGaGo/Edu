using ClosedXML.Excel;
using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CMS.ThongKeTinNhan
{
    public partial class ThongKeTinNhanTheoThuongHieu : AuthenticatePage
    {
        private TinNhanBO tinNhanBO = new TinNhanBO();
        private LocalAPI localAPI = new LocalAPI();
        TRUONG truong = new TRUONG();
        TruongBO truongBO = new TruongBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rdTuNgay.SelectedDate = DateTime.Now;
                rdDenNgay.SelectedDate = DateTime.Now;
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            var date_to = rdTuNgay.SelectedDate.Value.ToString("yyyyMMdd");
            var date_from = rdDenNgay.SelectedDate.Value.ToString("yyyyMMdd");
            List<ThongKeTinNhanTheoThuongHieuEntity> lst = tinNhanBO.thongKeTheoThuongHieu(rcbThuongHieu.SelectedValue, rcbTelCo.SelectedValue, rcbCP.SelectedValue, date_to, date_from);

            RadGrid1.DataSource = lst;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
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
        protected void checkNgay()
        {
            if (rdTuNgay.SelectedDate > rdDenNgay.SelectedDate)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Ngày bắt đầu phải nhỏ hơn ngày kết thúc');", true);
                return;
            }
        }
        protected void rcbLop_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void rcbTruong_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void btExport_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string serverPath = Server.MapPath("~");
            string path = String.Format("{0}\\ExportTemplates\\FileDynamic.xlsx", Server.MapPath("~"));
            string newName = "Thong_ke_tin_nhan_theo_thuong_hieu.xlsx";

            List<ExcelHeaderEntity> lstHeader = new List<ExcelHeaderEntity>();
            List<ExcelEntity> lstColumn = new List<ExcelEntity>();
            lstHeader.Add(new ExcelHeaderEntity { name = "STT", colM = 1, rowM = 2, width = 10 });
            List<ExcelHeaderEntity> lstTinLienLac = new List<ExcelHeaderEntity>();
            List<ExcelHeaderEntity> lstTinThongBao = new List<ExcelHeaderEntity>();

            #region add column
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "BRAND_NAME"))
            {
                DataColumn col = new DataColumn("BRAND_NAME");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Thương hiệu", colM = 1, rowM = 2, width = 30 });
                lstColumn.Add(new ExcelEntity { Name = "BRAND_NAME", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "CP"))
            {
                DataColumn col = new DataColumn("CP");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Đối tác", colM = 1, rowM = 2, width = 20 });
                lstColumn.Add(new ExcelEntity { Name = "CP", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "LOAI_NHA_MANG"))
            {
                DataColumn col = new DataColumn("LOAI_NHA_MANG");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Loại nhà mạng", colM = 1, rowM = 2, width = 40 });
                lstColumn.Add(new ExcelEntity { Name = "LOAI_NHA_MANG", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "SO_TIN"))
            {
                DataColumn col = new DataColumn("SO_TIN");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Số tin", colM = 1, rowM = 2, width = 20 });
                lstColumn.Add(new ExcelEntity { Name = "SO_TIN", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            #endregion

            RadGrid1.AllowPaging = false;
            RadGrid1.Rebind();
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
            string tenPhong = rcbThuongHieu.SelectedValue;
            string tieuDe = "THỐNG KÊ TIN NHẮN";
            string hocKyNamHoc = "Từ ngày: " + rdTuNgay.SelectedDate.Value.ToString("dd/MM/yyyy") + " - " + rdDenNgay.SelectedDate.Value.ToString("dd/MM/yyyy");
            listCell.Add(new ExcelHeaderEntity { name = tenSo, colM = 3, rowM = 1, colName = "A", rowIndex = 1, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tenPhong, colM = 3, rowM = 1, colName = "A", rowIndex = 2, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tieuDe, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 3, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            listCell.Add(new ExcelHeaderEntity { name = hocKyNamHoc, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 4, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            string nameFileOutput = newName;
            localAPI.ExportExcelDynamic(serverPath, path, newName, nameFileOutput, 1, listCell, rowHeaderStart, rowStart, colStart, colEnd, dt, lstHeader, lstColumn, true);
        }
        protected void rcbTelCo_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void rcbCP_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                string brand_name = item["BRAND_NAME"].Text;
                string doi_tac = item["CP"].Text;
                string loai_nha_mang = item["LOAI_NHA_MANG"].Text;
                if (!(brand_name == "Tổng" && doi_tac == "&nbsp;" && loai_nha_mang == "&nbsp;"))
                {
                    if (loai_nha_mang.ToLower().Contains("tổng"))
                        item.Font.Bold = true;
                    if (doi_tac.ToLower().Contains("tổng"))
                    {
                        item.Font.Bold = true;
                        item.ForeColor = Color.Red;
                    }
                }
                else
                {
                    item.Font.Bold = true;
                    item.ForeColor = Color.Red;
                }
            }
        }
        protected void RadGrid1_PreRender(object sender, EventArgs e)
        {
            MergeRows(RadGrid1);
        }
        public static void MergeRows(RadGrid RadGrid1)
        {
            string cp = "", telco = "";
            for (int i = RadGrid1.Items.Count - 1; i > 0; i--)
            {
                if (RadGrid1.Items[i][RadGrid1.Columns[1]].Text == RadGrid1.Items[i - 1][RadGrid1.Columns[1]].Text)
                {
                    RadGrid1.Items[i - 1][RadGrid1.Columns[1]].RowSpan = RadGrid1.Items[i][RadGrid1.Columns[1]].RowSpan < 2 ? 2 : RadGrid1.Items[i][RadGrid1.Columns[1]].RowSpan + 1;
                    RadGrid1.Items[i][RadGrid1.Columns[1]].Visible = false;
                }
                cp = RadGrid1.Items[i][RadGrid1.Columns[2]].Text;
                telco = RadGrid1.Items[i][RadGrid1.Columns[3]].Text;
                if (cp == "&nbsp;" && telco == "&nbsp;")
                {
                    RadGrid1.Items[i][RadGrid1.Columns[1]].ColumnSpan = 3;
                    RadGrid1.Items[i].ForeColor = Color.Red;
                    RadGrid1.Items[i][RadGrid1.Columns[2]].Visible = false;
                    RadGrid1.Items[i][RadGrid1.Columns[3]].Visible = false;
                }
            }
        }
        protected void rcbThuongHieu_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
    }
}