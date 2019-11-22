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

namespace CMS.BaoCaoThongKe
{
    public partial class ThongKeHocSinhTheoTruong : AuthenticatePage
    {
        private LocalAPI localAPI = new LocalAPI();
        TruongBO truongBO = new TruongBO();
        HocSinhBO hocSinhBO = new HocSinhBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rcbTruong.DataSource = Sys_List_Truong;
                rcbTruong.DataBind();
                loadCapHoc();
            }
        }
        protected void loadCapHoc()
        {
            rcbCapHoc.Items.Clear();
            rcbCapHoc.ClearSelection();
            rcbCapHoc.Text = string.Empty;

            int count = 0;
            if (rcbTruong.SelectedValue != "")
            {
                TRUONG truong = truongBO.getTruongById(Convert.ToInt64(rcbTruong.SelectedValue));
                if (truong != null)
                {
                    if (truong.IS_MN == true)
                    {
                        rcbCapHoc.Items.Insert(0, new RadComboBoxItem("Mầm non", "MN"));
                        count++;
                        if (Sys_This_Cap_Hoc == "MN") rcbCapHoc.SelectedValue = Sys_This_Cap_Hoc;
                    }
                    if (truong.IS_TH == true)
                    {
                        rcbCapHoc.Items.Insert(count, new RadComboBoxItem("Tiểu học", "TH"));
                        count++;
                        if (Sys_This_Cap_Hoc == "TH") rcbCapHoc.SelectedValue = Sys_This_Cap_Hoc;
                    }
                    if (truong.IS_THCS == true)
                    {
                        rcbCapHoc.Items.Insert(count, new RadComboBoxItem("Trung học cơ sở", "THCS"));
                        count++;
                        if (Sys_This_Cap_Hoc == "THCS") rcbCapHoc.SelectedValue = Sys_This_Cap_Hoc;
                    }
                    if (truong.IS_THPT == true)
                    {
                        rcbCapHoc.Items.Insert(count, new RadComboBoxItem("Trung học phổ thông", "THPT"));
                        count++;
                        if (Sys_This_Cap_Hoc == "THPT") rcbCapHoc.SelectedValue = Sys_This_Cap_Hoc;
                    }
                    if (truong.IS_GDTX == true)
                    {
                        rcbCapHoc.Items.Insert(count, new RadComboBoxItem("Giáo dục thường xuyên", "GDTX"));
                        if (Sys_This_Cap_Hoc == "GDTX") rcbCapHoc.SelectedValue = Sys_This_Cap_Hoc;
                    }
                    rcbCapHoc.DataBind();
                    if (rcbCapHoc.Items.Count == 0)
                    {
                        string strMsg = "notification('errors', 'Trường chưa thiết lập cấp học, vui lòng liên hệ quản trị viên');";
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
                        return;
                    }
                }
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            List<ThongKeHocSinhTheoTruongEntity> lst = hocSinhBO.thongKeTongHocSinhTheoTruong(localAPI.ConvertStringTolong(rcbTruong.SelectedValue), rcbCapHoc.SelectedValue, Convert.ToInt16(Sys_Ma_Nam_hoc), Convert.ToInt16(Sys_Hoc_Ky));
            long tong_so_hs = 0;
            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i].TEN_LOP == null || lst[i].TEN_LOP == "") tong_so_hs += lst[i].SO_HOC_SINH;
            }
            ThongKeHocSinhTheoTruongEntity detail = new ThongKeHocSinhTheoTruongEntity();
            detail.TEN_TRUONG = "Tổng";
            detail.SO_HOC_SINH = tong_so_hs;
            lst.Insert(0, detail);

            RadGrid1.DataSource = lst;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
        protected void rcbTruong_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            loadCapHoc();
            RadGrid1.Rebind();
        }
        protected void rcbCapHoc_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void btExport_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string serverPath = Server.MapPath("~");
            string path = String.Format("{0}\\ExportTemplates\\FileDynamic.xlsx", Server.MapPath("~"));
            string newName = "Thong_ke_hoc_sinh_theo_khoi.xlsx";

            List<ExcelHeaderEntity> lstHeader = new List<ExcelHeaderEntity>();
            List<ExcelEntity> lstColumn = new List<ExcelEntity>();
            lstHeader.Add(new ExcelHeaderEntity { name = "STT", colM = 1, rowM = 1, width = 10 });
            List<ExcelHeaderEntity> lstTinLienLac = new List<ExcelHeaderEntity>();
            List<ExcelHeaderEntity> lstTinThongBao = new List<ExcelHeaderEntity>();
            foreach (GridColumn item in RadGrid1.MasterTableView.Columns)
            {
                #region add column
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TEN_TRUONG") && item.UniqueName == "TEN_TRUONG")
                {
                    DataColumn col = new DataColumn("TEN_TRUONG");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tên trường", colM = 1, rowM = 1, width = 50 });
                    lstColumn.Add(new ExcelEntity { Name = "TEN_TRUONG", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TEN_KHOI") && item.UniqueName == "TEN_KHOI")
                {
                    DataColumn col = new DataColumn("TEN_KHOI");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tên khối", colM = 1, rowM = 1, width = 50 });
                    lstColumn.Add(new ExcelEntity { Name = "TEN_KHOI", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TEN_LOP") && item.UniqueName == "TEN_LOP")
                {
                    DataColumn col = new DataColumn("TEN_LOP");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tên lớp", colM = 1, rowM = 1, width = 30 });
                    lstColumn.Add(new ExcelEntity { Name = "TEN_LOP", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "SO_HOC_SINH") && item.UniqueName == "SO_HOC_SINH")
                {
                    DataColumn col = new DataColumn("SO_HOC_SINH");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Số học sinh", colM = 1, rowM = 1, width = 20 });
                    lstColumn.Add(new ExcelEntity { Name = "SO_HOC_SINH", Align = XLAlignmentHorizontalValues.Right, Color = XLColor.Black, Type = "String" });
                }
                #endregion
            }

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
            int rowStart = 7;
            string colStart = "A";
            string colEnd = localAPI.GetExcelColumnName(lstColumn.Count);
            List<ExcelHeaderEntity> listCell = new List<ExcelHeaderEntity>();
            string tenSo = "";
            string tenPhong = "";
            string tieuDe = "THỐNG KÊ TỔNG HỌC SINH THEO KHỐI";
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
                string ten_truong = item["TEN_TRUONG"].Text;
                string khoi = item["TEN_KHOI"].Text;
                string lop = item["TEN_LOP"].Text;
                if (!(ten_truong == "Tổng" && khoi == "&nbsp;" && lop == "&nbsp;"))
                {
                    if (lop.ToLower().Contains("tổng"))
                        item.Font.Bold = true;
                    if (khoi.ToLower().Contains("tổng"))
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
            for (int i = RadGrid1.Items.Count - 1; i > 1; i--)
            {
                if (RadGrid1.Items[i][RadGrid1.Columns[3]].Text == RadGrid1.Items[i - 1][RadGrid1.Columns[3]].Text)
                {
                    RadGrid1.Items[i - 1][RadGrid1.Columns[3]].RowSpan = RadGrid1.Items[i][RadGrid1.Columns[3]].RowSpan < 2 ? 2 : RadGrid1.Items[i][RadGrid1.Columns[3]].RowSpan + 1;
                    RadGrid1.Items[i][RadGrid1.Columns[3]].Visible = false;
                }
                if (RadGrid1.Items[i][RadGrid1.Columns[2]].Text == RadGrid1.Items[i - 1][RadGrid1.Columns[2]].Text)
                {
                    RadGrid1.Items[i - 1][RadGrid1.Columns[2]].RowSpan = RadGrid1.Items[i][RadGrid1.Columns[2]].RowSpan < 2 ? 2 : RadGrid1.Items[i][RadGrid1.Columns[2]].RowSpan + 1;
                    RadGrid1.Items[i][RadGrid1.Columns[2]].Visible = false;
                }
            }
            RadGrid1.Items[0]["TEN_TRUONG"].ColumnSpan = 3;
            RadGrid1.Items[0]["TEN_TRUONG"].ForeColor = Color.Red;
            RadGrid1.Items[0]["TEN_KHOI"].Visible = false;
            RadGrid1.Items[0]["TEN_LOP"].Visible = false;
        }
    }
}