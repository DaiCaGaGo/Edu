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
    public partial class ThongKeTinNhanLop : AuthenticatePage
    {
        private TinNhanBO tinNhanBO = new TinNhanBO();
        private LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            btExport.Visible = is_access(SYS_Type_Access.EXPORT);
            if (!IsPostBack)
            {
                objKhoi.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                objKhoi.SelectParameters.Add("maLoaiLopGDTX", DbType.Int16, Sys_This_Lop_GDTX == null ? "" : Sys_This_Lop_GDTX.ToString());
                rcbKhoi.DataBind();
                objLop.SelectParameters.Add("ma_cap_hoc", Sys_This_Cap_Hoc);
                objLop.SelectParameters.Add("idTruong", Sys_This_Truong.ID.ToString());
                objLop.SelectParameters.Add("maNamHoc", Sys_Ma_Nam_hoc.ToString());
                rcbLop.DataBind();
                rdTuNgay.SelectedDate = DateTime.Now;
                rdDenNgay.SelectedDate = DateTime.Now;
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            var date_to = rdTuNgay.SelectedDate.Value.ToString("yyyyMMdd");
            var date_from = rdDenNgay.SelectedDate.Value.AddDays(1).ToString("yyyyMMdd");
            RadGrid1.DataSource = tinNhanBO.thongKeTheoKhoiLopNgay1(Sys_This_Truong.ID, localAPI.ConvertStringToShort(rcbKhoi.SelectedValue), localAPI.ConvertStringToShort(rcbLop.SelectedValue), Convert.ToInt16(Sys_Ma_Nam_hoc), date_to, date_from);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
        protected void rcbThang_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
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
            string newName = "Thong_ke_tin_nhan.xlsx";

            List<ExcelHeaderEntity> lstHeader = new List<ExcelHeaderEntity>();
            List<ExcelEntity> lstColumn = new List<ExcelEntity>();
            lstHeader.Add(new ExcelHeaderEntity { name = "STT", colM = 1, rowM = 2, width = 10 });
            List<ExcelHeaderEntity> lstTinLienLac = new List<ExcelHeaderEntity>();
            List<ExcelHeaderEntity> lstTinThongBao = new List<ExcelHeaderEntity>();

            #region add column
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TEN_LOP"))
            {
                DataColumn col = new DataColumn("TEN_LOP");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Tên lớp", colM = 1, rowM = 2, width = 20 });
                lstColumn.Add(new ExcelEntity { Name = "TEN_LOP", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }

            // tin liên lạc
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TONG_TIN_LL"))
                lstTinLienLac.Add(new ExcelHeaderEntity { name = "TONG_TIN_LL", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TIN_LL_TC"))
                lstTinLienLac.Add(new ExcelHeaderEntity { name = "TIN_LL_TC", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TIN_LL_CG"))
                lstTinLienLac.Add(new ExcelHeaderEntity { name = "TIN_LL_CG", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TIN_LL_GL"))
                lstTinLienLac.Add(new ExcelHeaderEntity { name = "TIN_LL_GL", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TIN_LL_DG"))
                lstTinLienLac.Add(new ExcelHeaderEntity { name = "TIN_LL_DG", colM = 1, rowM = 1, width = 10 });

            // tin thông báo
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TONG_TIN_TB"))
                lstTinThongBao.Add(new ExcelHeaderEntity { name = "TONG_TIN_TB", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TIN_TB_TC"))
                lstTinThongBao.Add(new ExcelHeaderEntity { name = "TIN_TB_TC", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TIN_TB_CG"))
                lstTinThongBao.Add(new ExcelHeaderEntity { name = "TIN_TB_CG", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TIN_TB_GL"))
                lstTinThongBao.Add(new ExcelHeaderEntity { name = "TIN_TB_GL", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TIN_TB_DG"))
                lstTinThongBao.Add(new ExcelHeaderEntity { name = "TIN_TB_DG", colM = 1, rowM = 1, width = 10 });
            int indexTinLienLac = 0;
            int indexTinThongBao = 0;
            #region "add liên lạc"
            if (lstTinLienLac != null & lstTinLienLac.Count > 0)
            {
                lstHeader.Add(new ExcelHeaderEntity { name = "Tin nhắn liên lạc", colM = lstTinLienLac.Count, rowM = 1, width = lstTinLienLac.Count * 15 });
                indexTinLienLac = lstHeader.Count - 1;
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TONG_TIN_LL"))
            {
                DataColumn col = new DataColumn("TONG_TIN_LL");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Tổng số", colM = 1, rowM = 1, width = 15, parentIndex = indexTinLienLac });
                lstColumn.Add(new ExcelEntity { Name = "TONG_TIN_LL", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TIN_LL_TC"))
            {
                DataColumn col = new DataColumn("TIN_LL_TC");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Thành công", colM = 1, rowM = 1, width = 15, parentIndex = indexTinLienLac });
                lstColumn.Add(new ExcelEntity { Name = "TIN_LL_TC", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TIN_LL_CG"))
            {
                DataColumn col = new DataColumn("TIN_LL_CG");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Chờ gửi", colM = 1, rowM = 1, width = 15, parentIndex = indexTinLienLac });
                lstColumn.Add(new ExcelEntity { Name = "TIN_LL_CG", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TIN_LL_GL"))
            {
                DataColumn col = new DataColumn("TIN_LL_GL");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Gửi lại", colM = 1, rowM = 1, width = 15, parentIndex = indexTinLienLac });
                lstColumn.Add(new ExcelEntity { Name = "TIN_LL_GL", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TIN_LL_DG"))
            {
                DataColumn col = new DataColumn("TIN_LL_DG");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Dừng lại", colM = 1, rowM = 1, width = 15, parentIndex = indexTinLienLac });
                lstColumn.Add(new ExcelEntity { Name = "TIN_LL_DG", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            #endregion
            #region "add thông báo"
            if (lstTinThongBao != null && lstTinThongBao.Count > 0)
            {
                lstHeader.Add(new ExcelHeaderEntity { name = "Tin nhắn thông báo", colM = lstTinThongBao.Count, rowM = 1, width = lstTinThongBao.Count * 15 });
                indexTinThongBao = lstHeader.Count - 1;
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TONG_TIN_TB"))
            {
                DataColumn col = new DataColumn("TONG_TIN_TB");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Tổng số", colM = 1, rowM = 1, width = 15, parentIndex = indexTinThongBao });
                lstColumn.Add(new ExcelEntity { Name = "TONG_TIN_TB", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TIN_TB_TC"))
            {
                DataColumn col = new DataColumn("TIN_TB_TC");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Thành công", colM = 1, rowM = 1, width = 15, parentIndex = indexTinThongBao });
                lstColumn.Add(new ExcelEntity { Name = "TIN_TB_TC", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TIN_TB_CG"))
            {
                DataColumn col = new DataColumn("TIN_TB_CG");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Chờ gửi", colM = 1, rowM = 1, width = 15, parentIndex = indexTinThongBao });
                lstColumn.Add(new ExcelEntity { Name = "TIN_TB_CG", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TIN_TB_GL"))
            {
                DataColumn col = new DataColumn("TIN_TB_GL");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Gửi lại", colM = 1, rowM = 1, width = 15, parentIndex = indexTinThongBao });
                lstColumn.Add(new ExcelEntity { Name = "TIN_TB_GL", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TIN_TB_DG"))
            {
                DataColumn col = new DataColumn("TIN_TB_DG");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Dừng gửi", colM = 1, rowM = 1, width = 15, parentIndex = indexTinThongBao });
                lstColumn.Add(new ExcelEntity { Name = "TIN_TB_DG", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            #endregion
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
            string tenPhong = Sys_This_Truong.TEN;
            string tieuDe = "THỐNG KÊ TIN NHẮN";
            string hocKyNamHoc = "Từ ngày " + rdTuNgay.SelectedDate.Value.ToString("dd/MM/yyyy") + " đến ngày " + rdDenNgay.SelectedDate.Value.ToString("dd/MM/yyyy");
            listCell.Add(new ExcelHeaderEntity { name = tenSo, colM = 3, rowM = 1, colName = "A", rowIndex = 1, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tenPhong, colM = 3, rowM = 1, colName = "A", rowIndex = 2, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tieuDe, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 3, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            listCell.Add(new ExcelHeaderEntity { name = hocKyNamHoc, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 4, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            string nameFileOutput = newName;
            localAPI.ExportExcelDynamic(serverPath, path, newName, nameFileOutput, 1, listCell, rowHeaderStart, rowStart, colStart, colEnd, dt, lstHeader, lstColumn, true);
        }
        protected void rcbKhoi_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbLop.ClearSelection();
            rcbLop.Text = String.Empty;
            rcbLop.DataBind();
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

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                long? TONG_TIN_LL = localAPI.ConvertStringToShort(item["TONG_TIN_LL"].Text);
                long? TIN_LL_TC = localAPI.ConvertStringToShort(item["TIN_LL_TC"].Text);
                long? TIN_LL_CG = localAPI.ConvertStringToShort(item["TIN_LL_CG"].Text);
                long? TIN_LL_GL = localAPI.ConvertStringToShort(item["TIN_LL_GL"].Text);
                long? TIN_LL_DG = localAPI.ConvertStringToShort(item["TIN_LL_DG"].Text);

                long? TONG_TIN_TB = localAPI.ConvertStringToShort(item["TONG_TIN_TB"].Text);
                long? TIN_TB_TC = localAPI.ConvertStringToShort(item["TIN_TB_TC"].Text);
                long? TIN_TB_CG = localAPI.ConvertStringToShort(item["TIN_TB_CG"].Text);
                long? TIN_TB_GL = localAPI.ConvertStringToShort(item["TIN_TB_GL"].Text);
                long? TIN_TB_DG = localAPI.ConvertStringToShort(item["TIN_TB_DG"].Text);

                if (!(TONG_TIN_LL > 0)) item["TONG_TIN_LL"].Text = "";
                if (!(TIN_LL_TC > 0)) item["TIN_LL_TC"].Text = "";
                if (!(TIN_LL_CG > 0)) item["TIN_LL_CG"].Text = "";
                if (!(TIN_LL_GL > 0)) item["TIN_LL_GL"].Text = "";
                if (!(TIN_LL_DG > 0)) item["TIN_LL_DG"].Text = "";

                if (!(TONG_TIN_TB > 0)) item["TONG_TIN_TB"].Text = "";
                if (!(TIN_TB_TC > 0)) item["TIN_TB_TC"].Text = "";
                if (!(TIN_TB_CG > 0)) item["TIN_TB_CG"].Text = "";
                if (!(TIN_TB_GL > 0)) item["TIN_TB_GL"].Text = "";
                if (!(TIN_TB_DG > 0)) item["TIN_TB_DG"].Text = "";
            }
        }
    }
}