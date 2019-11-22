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

namespace CMS.ThongKeTinNhan
{
    public partial class ThongKeTinNhan_SYS : AuthenticatePage
    {
        private TinNhanBO tinNhanBO = new TinNhanBO();
        private LocalAPI localAPI = new LocalAPI();
        TRUONG truong = new TRUONG();
        TruongBO truongBO = new TruongBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rcbTruong.DataSource = Sys_List_Truong;
                rcbTruong.DataBind();
                loadCapHoc();
                short? loai_lop_GDTX = localAPI.ConvertStringToShort(rcbLoaiLopGDTX.SelectedValue);
                if (rcbCapHoc.SelectedValue != "GDTX") loai_lop_GDTX = null;
                rcbKhoi.DataBind();
                objLop.SelectParameters.Add("maNamHoc", Sys_Ma_Nam_hoc.ToString());
                rcbLop.DataBind();
                rdTuNgay.SelectedDate = DateTime.Now;
                rdDenNgay.SelectedDate = DateTime.Now;
            }
            if (rcbCapHoc.SelectedValue == "GDTX") divLoaiLopGDTX.Visible = true;
            else divLoaiLopGDTX.Visible = false;
        }
        protected void loadCapHoc()
        {
            rcbCapHoc.Items.Clear();
            rcbCapHoc.ClearSelection();
            rcbCapHoc.Text = string.Empty;

            int count = 0;
            if (rcbTruong.SelectedValue != "")
            {
                truong = truongBO.getTruongById(Convert.ToInt64(rcbTruong.SelectedValue));
                if (truong != null)
                {
                    if (truong.IS_MN == true)
                    {
                        rcbCapHoc.Items.Insert(0, new RadComboBoxItem("Mầm non", "MN"));
                        count++;
                    }
                    if (truong.IS_TH == true)
                    {
                        rcbCapHoc.Items.Insert(count, new RadComboBoxItem("Tiểu học", "TH"));
                        count++;
                    }
                    if (truong.IS_THCS == true)
                    {
                        rcbCapHoc.Items.Insert(count, new RadComboBoxItem("Trung học sơ sở", "THCS"));
                        count++;
                    }
                    if (truong.IS_THPT == true)
                    {
                        rcbCapHoc.Items.Insert(count, new RadComboBoxItem("Trung học phổ thông", "THPT"));
                        count++;
                    }
                    if (truong.IS_GDTX == true)
                    {
                        rcbCapHoc.Items.Insert(count, new RadComboBoxItem("Giáo dục thường xuyên", "GDTX"));
                    }
                    rcbCapHoc.DataBind();
                    if (rcbCapHoc.Items.Count == 0)
                    {
                        string strMsg = "notification('errors', 'Trường chưa thiết lập cấp học, vui lòng liên hệ quản trị viên');";
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
                        return;
                    }
                    else if (rcbCapHoc.Items.Count == 1)
                    {
                        rcbCapHoc.SelectedIndex = 0;
                    }
                }
            }
            if (rcbCapHoc.SelectedValue == SYS_Cap_Hoc.GDTX)
            {
                LoaiLopGDTXBO loaiLopGDTXBO = new LoaiLopGDTXBO();
                divLoaiLopGDTX.Visible = true;
                rcbLoaiLopGDTX.DataSource = loaiLopGDTXBO.getLoaiLopGDTX();
                rcbLoaiLopGDTX.DataBind();
            }
            else divLoaiLopGDTX.Visible = false;
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            //RadGrid1.DataSource = tinNhanBO.thongKeTheoKhoiLopNgay(localAPI.ConvertStringTolong(rcbTruong.SelectedValue), localAPI.ConvertStringToShort(rcbKhoi.SelectedValue), localAPI.ConvertStringToShort(rcbLop.SelectedValue), Convert.ToInt16(Sys_Ma_Nam_hoc), rdTuNgay.SelectedDate.Value, rdDenNgay.SelectedDate.Value.AddDays(1));
            var date_to = rdTuNgay.SelectedDate.Value.ToString("yyyyMMdd");
            var date_from = rdDenNgay.SelectedDate.Value.AddDays(1).ToString("yyyyMMdd");
            RadGrid1.DataSource = tinNhanBO.thongKeTheoKhoiLopNgay1(localAPI.ConvertStringTolong(rcbTruong.SelectedValue), localAPI.ConvertStringToShort(rcbKhoi.SelectedValue), localAPI.ConvertStringToShort(rcbLop.SelectedValue), Convert.ToInt16(Sys_Ma_Nam_hoc), date_to, date_from);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
        protected void rcbThang_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
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
        protected void rcbTruong_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            loadCapHoc();
            if (rcbCapHoc.SelectedValue == "GDTX") divLoaiLopGDTX.Visible = true;
            else divLoaiLopGDTX.Visible = false;
            rcbKhoi.ClearSelection();
            rcbKhoi.Text = String.Empty;
            rcbKhoi.DataBind();
            rcbLop.ClearSelection();
            rcbLop.Text = String.Empty;
            rcbLop.DataBind();
            RadGrid1.Rebind();
        }
        protected void rcbCapHoc_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbKhoi.ClearSelection();
            rcbKhoi.Text = String.Empty;
            rcbKhoi.DataBind();
            if (rcbCapHoc.SelectedValue == SYS_Cap_Hoc.GDTX)
            {
                divLoaiLopGDTX.Visible = true;
                LoaiLopGDTXBO loaiLopGDTXBO = new LoaiLopGDTXBO();
                rcbLoaiLopGDTX.DataSource = loaiLopGDTXBO.getLoaiLopGDTX();
                rcbLoaiLopGDTX.DataBind();
            }
            else divLoaiLopGDTX.Visible = false;
        }
        protected void rcbLoaiLopGDTX_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbLop.ClearSelection();
            rcbLop.Text = string.Empty;
            rcbLop.DataBind();
            RadGrid1.Rebind();
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
            string tenPhong = rcbTruong.SelectedValue == "" ? "" : rcbTruong.Text;
            string tieuDe = "THỐNG KÊ TIN NHẮN";
            string hocKyNamHoc = "Năm học: " + Sys_Ten_Nam_Hoc;
            listCell.Add(new ExcelHeaderEntity { name = tenSo, colM = 3, rowM = 1, colName = "A", rowIndex = 1, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tenPhong, colM = 3, rowM = 1, colName = "A", rowIndex = 2, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tieuDe, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 3, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            listCell.Add(new ExcelHeaderEntity { name = hocKyNamHoc, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 4, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            string nameFileOutput = newName;
            localAPI.ExportExcelDynamic(serverPath, path, newName, nameFileOutput, 1, listCell, rowHeaderStart, rowStart, colStart, colEnd, dt, lstHeader, lstColumn, true);
        }
    }
}