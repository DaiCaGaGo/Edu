using ClosedXML.Excel;
using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CMS.SMS
{
    public partial class ThongKeTinNhan : AuthenticatePage
    {
        private QuyTinBO quyTinBO = new QuyTinBO();
        private LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rcbThang.SelectedValue = DateTime.Now.Month.ToString();
                rcbThang.DataBind();
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
            List<ThongkeQuyTinEntiy> lst = quyTinBO.thongKeQuyTin(localAPI.ConvertStringToShort(rcbTinhThanh.SelectedValue), localAPI.ConvertStringToShort(rcbQuanHuyen.SelectedValue), localAPI.ConvertStringTolong(rcbTruong.SelectedValue), Sys_Ma_Nam_hoc, localAPI.ConvertStringToint(rcbThang.SelectedValue));
            #region add sum row
            long tong_cap_ll = 0, tong_them_ll = 0, tong_gui_ll = 0;
            long tong_cap_tb = 0, tong_them_tb = 0, tong_gui_tb = 0;
            float phan_tram_ll = 0, phan_tram_tb = 0;
            if (lst.Count > 0)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    tong_cap_ll += lst[i].TONG_CAP_LL;
                    tong_them_ll += lst[i].TONG_THEM_LL;
                    tong_gui_ll += lst[i].TONG_DA_GUI_LL;
                    tong_cap_tb += lst[i].TONG_CAP_TB;
                    tong_them_tb += lst[i].TONG_THEM_TB;
                    tong_gui_tb += lst[i].TONG_DA_GUI_TB;
                }
            }
            if (tong_cap_ll > 0)
                phan_tram_ll = (float)Math.Round(Convert.ToDecimal(tong_gui_ll * 100.0 / tong_cap_ll), 2);
            if (tong_cap_tb > 0)
                phan_tram_tb = (float)Math.Round(Convert.ToDecimal(tong_gui_tb * 100.0 / tong_cap_tb), 2);
            if (lst.Count == 0 || (lst.Count > 0 && lst[0].ID_TRUONG != 0))
            {
                ThongkeQuyTinEntiy detail = new ThongkeQuyTinEntiy();
                detail.ID_TRUONG = 0;
                detail.TONG_CAP_LL = tong_cap_ll;
                detail.TONG_THEM_LL = tong_them_ll;
                detail.TONG_DA_GUI_LL = tong_gui_ll;
                detail.PHAN_TRAM_LL = phan_tram_ll;
                detail.TONG_CAP_TB = tong_cap_tb;
                detail.TONG_THEM_TB = tong_them_tb;
                detail.TONG_DA_GUI_TB = tong_gui_tb;
                detail.PHAN_TRAM_TB = phan_tram_tb;
                lst.Insert(0, detail);
            }
            #endregion
            RadGrid1.DataSource = lst;
            //RadGrid1.DataSource = quyTinBO.thongKeQuyTin(localAPI.ConvertStringToShort(rcbTinhThanh.SelectedValue), localAPI.ConvertStringToShort(rcbQuanHuyen.SelectedValue), localAPI.ConvertStringTolong(rcbTruong.SelectedValue), Sys_Ma_Nam_hoc, localAPI.ConvertStringToint(rcbThang.SelectedValue));
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
        protected void rcbTinhThanh_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbQuanHuyen.ClearSelection();
            rcbQuanHuyen.Text = String.Empty;
            rcbQuanHuyen.DataBind();
            rcbTruong.ClearSelection();
            rcbTruong.Text = String.Empty;
            rcbTruong.DataBind();
            RadGrid1.Rebind();
        }
        protected void rcbQuanHuyen_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbTruong.ClearSelection();
            rcbTruong.Text = String.Empty;
            rcbTruong.DataBind();
            RadGrid1.Rebind();
        }
        protected void rcbTruong_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void rcbThang_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
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
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TEN"))
            {
                DataColumn col = new DataColumn("TEN");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Tên trường", colM = 1, rowM = 2, width = 30 });
                lstColumn.Add(new ExcelEntity { Name = "TEN", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }

            // tin liên lạc
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TONG_CAP_LL"))
                lstTinLienLac.Add(new ExcelHeaderEntity { name = "Tổng cấp", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TONG_THEM_LL"))
                lstTinLienLac.Add(new ExcelHeaderEntity { name = "Tổng thêm", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TONG_DA_GUI_LL"))
                lstTinLienLac.Add(new ExcelHeaderEntity { name = "Tổng gửi", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "PHAN_TRAM_LL"))
                lstTinLienLac.Add(new ExcelHeaderEntity { name = "Sản lượng sử dụng", colM = 1, rowM = 1, width = 10 });

            // tin thông báo
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TONG_CAP_TB"))
                lstTinThongBao.Add(new ExcelHeaderEntity { name = "Tổng cấp", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TONG_THEM_TB"))
                lstTinThongBao.Add(new ExcelHeaderEntity { name = "Tổng thêm", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TONG_DA_GUI_TB"))
                lstTinThongBao.Add(new ExcelHeaderEntity { name = "Tổng gửi", colM = 1, rowM = 1, width = 10 });
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "PHAN_TRAM_TB"))
                lstTinThongBao.Add(new ExcelHeaderEntity { name = "Sản lượng sử dụng", colM = 1, rowM = 1, width = 10 });

            int indexTinLienLac = 0;
            int indexTinThongBao = 0;
            if (lstTinLienLac != null & lstTinLienLac.Count > 0)
            {
                lstHeader.Add(new ExcelHeaderEntity { name = "Tin nhắn liên lạc (/tháng)", colM = lstTinLienLac.Count, rowM = 1, width = lstTinLienLac.Count * 15 });
                indexTinLienLac = lstHeader.Count - 1;
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TONG_CAP_LL"))
            {
                DataColumn col = new DataColumn("TONG_CAP_LL");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Tổng cấp", colM = 1, rowM = 1, width = 15, parentIndex = indexTinLienLac });
                lstColumn.Add(new ExcelEntity { Name = "TONG_CAP_LL", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TONG_THEM_LL"))
            {
                DataColumn col = new DataColumn("TONG_THEM_LL");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Tổng thêm", colM = 1, rowM = 1, width = 15, parentIndex = indexTinLienLac });
                lstColumn.Add(new ExcelEntity { Name = "TONG_THEM_LL", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TONG_DA_GUI_LL"))
            {
                DataColumn col = new DataColumn("TONG_DA_GUI_LL");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Tổng gửi", colM = 1, rowM = 1, width = 15, parentIndex = indexTinLienLac });
                lstColumn.Add(new ExcelEntity { Name = "TONG_DA_GUI_LL", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "PHAN_TRAM_LL"))
            {
                DataColumn col = new DataColumn("PHAN_TRAM_LL");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Sản lượng sử dụng (%)", colM = 1, rowM = 1, width = 25, parentIndex = indexTinLienLac });
                lstColumn.Add(new ExcelEntity { Name = "PHAN_TRAM_LL", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }

            if (lstTinThongBao != null && lstTinThongBao.Count > 0)
            {
                lstHeader.Add(new ExcelHeaderEntity { name = "Tin nhắn thông báo (/tháng)", colM = lstTinThongBao.Count, rowM = 1, width = lstTinThongBao.Count * 15 });
                indexTinThongBao = lstHeader.Count - 1;
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TONG_CAP_TB"))
            {
                DataColumn col = new DataColumn("TONG_CAP_TB");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Tổng cấp", colM = 1, rowM = 1, width = 15, parentIndex = indexTinThongBao });
                lstColumn.Add(new ExcelEntity { Name = "TONG_CAP_TB", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TONG_THEM_TB"))
            {
                DataColumn col = new DataColumn("TONG_THEM_TB");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Tổng thêm", colM = 1, rowM = 1, width = 15, parentIndex = indexTinThongBao });
                lstColumn.Add(new ExcelEntity { Name = "TONG_THEM_TB", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TONG_DA_GUI_TB"))
            {
                DataColumn col = new DataColumn("TONG_DA_GUI_TB");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Tổng gửi", colM = 1, rowM = 1, width = 15, parentIndex = indexTinThongBao });
                lstColumn.Add(new ExcelEntity { Name = "TONG_DA_GUI_TB", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "PHAN_TRAM_TB"))
            {
                DataColumn col = new DataColumn("PHAN_TRAM_TB");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Sản lượng sử dụng (%)", colM = 1, rowM = 1, width = 25, parentIndex = indexTinThongBao });
                lstColumn.Add(new ExcelEntity { Name = "PHAN_TRAM_TB", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
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
            string tenSo = !string.IsNullOrEmpty(rcbTinhThanh.SelectedValue) ? rcbTinhThanh.Text : "";
            string tenPhong = !string.IsNullOrEmpty(rcbQuanHuyen.SelectedValue) ? rcbQuanHuyen.Text : "";
            string tieuDe = "THỐNG KÊ TIN NHẮN";
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
    }
}