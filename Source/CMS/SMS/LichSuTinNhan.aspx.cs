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
    public partial class LichSuTinNhan : AuthenticatePage
    {
        TinNhanBO tinNhanBO = new TinNhanBO();
        TongHopNXHNBO thBO = new TongHopNXHNBO();
        public LocalAPI localAPI = new LocalAPI();
        short? sms_type;
        LocalAPI local_api = new LocalAPI();
        LogUserBO logUserBO = new LogUserBO();
        NhanXetHangNgayBO nxhnBO = new NhanXetHangNgayBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            if (Sys_User.IS_ROOT == null || (Sys_User.IS_ROOT != null && Sys_User.IS_ROOT == false)) btnGuiLai.Visible = false;
            btExport.Visible = is_access(SYS_Type_Access.EXPORT);
            if (Request.QueryString.Get("sms_type") != null)
            {
                try
                {
                    sms_type = Convert.ToInt16(Request.QueryString.Get("sms_type"));
                }
                catch { }
            }
            if (!IsPostBack)
            {
                rcbLoaiTinNhan.SelectedValue = sms_type == null ? "" : sms_type == 1 ? "1" : "2";
                rcbLoaiTinNhan.DataBind();
                rdTuNgay.SelectedDate = DateTime.Now;
                rdDenNgay.SelectedDate = DateTime.Now;
                objKhoiHoc.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                rcbKhoi.DataBind();
                objLop.SelectParameters.Add("ma_cap_hoc", Sys_This_Cap_Hoc);
                objLop.SelectParameters.Add("idTruong", Sys_This_Truong.ID.ToString());
                objLop.SelectParameters.Add("maNamHoc", Sys_Ma_Nam_hoc.ToString());
                rcbLop.DataBind();
                getTongTin();
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            var date_to = rdTuNgay.SelectedDate.Value.ToString("yyyyMMdd");
            var date_from = rdDenNgay.SelectedDate.Value.ToString("yyyyMMdd");

            RadGrid1.DataSource = tinNhanBO.getTinNhanByKhoiLopNgay(Sys_This_Truong.ID, Sys_This_Cap_Hoc, Convert.ToInt16(Sys_Ma_Nam_hoc), date_to, date_from, localAPI.ConvertStringToShort(rcbTrangThai.SelectedValue), localAPI.ConvertStringToShort(rcbNhaMang.SelectedValue), localAPI.ConvertStringToShort(rcbLoaiTinNhan.SelectedValue), localAPI.chuyenTiengVietKhongDau(tbNoiDung.Text.Trim()), tbSDT.Text.Trim(), localAPI.ConvertStringToShort(rcbKhoi.SelectedValue), localAPI.ConvertStringTolong(rcbLop.SelectedValue), localAPI.ConvertStringToint(rcbSoTinGui.SelectedValue), localAPI.ConvertStringToint(rcbLoaiNguoiNhan.SelectedValue));
            RadGrid1.MasterTableView.Columns.FindByUniqueName("RES_CODE").Visible = (Sys_User.IS_ROOT != true) ? false : true;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
        protected void rdTuNgay_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            checkNgay();
            RadGrid1.Rebind();
            getTongTin();
        }
        protected void rdDenNgay_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            checkNgay();
            RadGrid1.Rebind();
            getTongTin();
        }
        protected void checkNgay()
        {
            if (rdTuNgay.SelectedDate > rdDenNgay.SelectedDate)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Ngày bắt đầu phải nhỏ hơn ngày kết thúc');", true);
                return;
            }
        }
        protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                short? loai_tin = localAPI.ConvertStringToShort(item["LOAI_TIN"].Text);
                item["LOAI_TIN"].Text = loai_tin == null ? "" : loai_tin == 1 ? "Liên lạc cá nhân" : loai_tin == 2 ? "Thông báo chung" : "";
                short? trang_thai = localAPI.ConvertStringToShort(item["TRANG_THAI"].Text);
                item["TRANG_THAI"].Text = trang_thai == null ? "Chờ gửi" : trang_thai == 1 ? "Thành công" : trang_thai == 2 ? "Gửi lỗi" : "Dừng gửi";
                item.ForeColor = trang_thai == null ? Color.Black : trang_thai == 1 ? Color.Blue : trang_thai == 2 ? Color.Green : Color.Red;
                NguoiDungBO nguoiDungBO = new NguoiDungBO();
                long? id_nguoi_gui = localAPI.ConvertStringTolong(item["NGUOI_GUI"].Text);
                if (id_nguoi_gui != null)
                    item["NGUOI_GUI"].Text = nguoiDungBO.getNguoiDung().FirstOrDefault(x => x.ID == id_nguoi_gui.Value).TEN_DANG_NHAP;
            }
        }
        protected void rcbKhoi_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbLop.ClearSelection();
            rcbLop.Text = String.Empty;
            rcbLop.DataBind();
            RadGrid1.Rebind();
            getTongTin();
        }
        protected void rcbLop_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
            getTongTin();
        }
        protected void rcbTrangThai_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
            getTongTin();
        }
        protected void rcbNhaMang_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
            getTongTin();
        }
        protected void rcbLoaiTinNhan_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
            getTongTin();
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
            string newName = "Danh_sach_tin_nhan.xlsx";

            List<ExcelHeaderEntity> lstHeader = new List<ExcelHeaderEntity>();
            List<ExcelEntity> lstColumn = new List<ExcelEntity>();
            lstHeader.Add(new ExcelHeaderEntity { name = "STT", colM = 1, rowM = 1, width = 10 });
            foreach (GridColumn item in RadGrid1.MasterTableView.Columns)
            {
                #region add column
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "BRAND_NAME_VIETTEL") && item.UniqueName == "BRAND_NAME_VIETTEL")
                {
                    DataColumn col = new DataColumn("BRAND_NAME_VIETTEL");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Thương hiệu", colM = 1, rowM = 1, width = 20 });
                    lstColumn.Add(new ExcelEntity { Name = "BRAND_NAME_VIETTEL", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TEN_NGUOI_NHAN") && item.UniqueName == "TEN_NGUOI_NHAN")
                {
                    DataColumn col = new DataColumn("TEN_NGUOI_NHAN");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tên người nhận", colM = 1, rowM = 1, width = 20 });
                    lstColumn.Add(new ExcelEntity { Name = "TEN_NGUOI_NHAN", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TEN_LOP") && item.UniqueName == "TEN_LOP")
                {
                    DataColumn col = new DataColumn("TEN_LOP");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tên lớp", colM = 1, rowM = 1, width = 20 });
                    lstColumn.Add(new ExcelEntity { Name = "TEN_LOP", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "SDT_NHAN") && item.UniqueName == "SDT_NHAN")
                {
                    DataColumn col = new DataColumn("SDT_NHAN");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "SĐT nhận tin", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "SDT_NHAN", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "NOI_DUNG_KHONG_DAU") && item.UniqueName == "NOI_DUNG_KHONG_DAU")
                {
                    DataColumn col = new DataColumn("NOI_DUNG_KHONG_DAU");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Nội dung", colM = 1, rowM = 1, width = 60 });
                    lstColumn.Add(new ExcelEntity { Name = "NOI_DUNG_KHONG_DAU", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "SO_TIN") && item.UniqueName == "SO_TIN")
                {
                    DataColumn col = new DataColumn("SO_TIN");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Số tin", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "SO_TIN", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "LOAI_TIN") && item.UniqueName == "LOAI_TIN")
                {
                    DataColumn col = new DataColumn("LOAI_TIN");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Loại tin", colM = 1, rowM = 1, width = 20 });
                    lstColumn.Add(new ExcelEntity { Name = "LOAI_TIN", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TRANG_THAI") && item.UniqueName == "TRANG_THAI")
                {
                    DataColumn col = new DataColumn("TRANG_THAI");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Trạng thái", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "TRANG_THAI", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "THOI_GIAN_GUI") && item.UniqueName == "THOI_GIAN_GUI")
                {
                    DataColumn col = new DataColumn("THOI_GIAN_GUI");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Thời gian gửi", colM = 1, rowM = 1, width = 25 });
                    lstColumn.Add(new ExcelEntity { Name = "THOI_GIAN_GUI", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "LOAI_NHA_MANG") && item.UniqueName == "LOAI_NHA_MANG")
                {
                    DataColumn col = new DataColumn("LOAI_NHA_MANG");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Nhà mạng", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "LOAI_NHA_MANG", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                #endregion
            }
            RadGrid1.AllowPaging = false;
            RadGrid1.Rebind();
            foreach (GridDataItem item in RadGrid1.Items)
            {
                DataRow row = dt.NewRow();
                HiddenField hdNoiDung = (HiddenField)item.FindControl("hdNoiDung");
                foreach (DataColumn col in dt.Columns)
                {
                    row[col.ColumnName] = item[col.ColumnName].Text.Replace("&nbsp;", " ");
                    if (col.ColumnName == "NOI_DUNG_KHONG_DAU") row[col.ColumnName] = hdNoiDung.Value;
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
            string tieuDe = "DANH SÁCH TIN NHẮN";
            string hocKyNamHoc = rcbLop.SelectedValue == "" ? ("Từ ngày " + rdTuNgay.SelectedDate.Value.ToString("dd/MM/yyyy") + " đến ngày " + rdDenNgay.SelectedDate.Value.ToString("dd/MM/yyyy")) : "Lớp " + (rcbLop.Text + ", từ ngày " + rdTuNgay.SelectedDate.Value + " đến ngày " + rdDenNgay.SelectedDate.Value);
            listCell.Add(new ExcelHeaderEntity { name = tenSo, colM = 3, rowM = 1, colName = "A", rowIndex = 1, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tenPhong, colM = 3, rowM = 1, colName = "A", rowIndex = 2, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tieuDe, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 3, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            listCell.Add(new ExcelHeaderEntity { name = hocKyNamHoc, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 4, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            string nameFileOutput = newName;
            localAPI.ExportExcelDynamic(serverPath, path, newName, nameFileOutput, 1, listCell, rowHeaderStart, rowStart, colStart, colEnd, dt, lstHeader, lstColumn, true);
        }
        protected void btTimKiem_Click(object sender, EventArgs e)
        {
            RadGrid1.Rebind();
            getTongTin();
        }
        protected void rcbSoTinGui_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
            getTongTin();
        }
        protected void btnGuiLai_Click(object sender, EventArgs e)
        {
            #region check quyen
            if (Sys_User.IS_ROOT == null || (Sys_User.IS_ROOT != null && Sys_User.IS_ROOT == false))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện chức năng này.');", true);
                return;
            }
            //if (Convert.ToDateTime(rdTuNgay.SelectedDate).Date < DateTime.Now.Date || Convert.ToDateTime(rdDenNgay.SelectedDate).Date > DateTime.Now.Date)
            //{
            //    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không thể thực hiện thao tác này, chỉ thực hiện trong ngày!');", true);
            //    return;
            //}
            #endregion
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            GridItemCollection lstGrid = new GridItemCollection();
            lstGrid = RadGrid1.SelectedItems;
            int success = 0;
            string strMsgW = "";
            foreach (GridDataItem item in lstGrid)
            {
                long? id_nx = local_api.ConvertStringTolong(item["ID_NHAN_XET_HANG_NGAY"].Text);
                long? id_thnx = local_api.ConvertStringTolong(item["ID_TONG_HOP_NXHN"].Text);
                bool is_send = false;
                if (item["TRANG_THAI"].Text == "1" || item["TRANG_THAI"].Text == "3" || item["TRANG_THAI"].Text == "Thành công" || item["TRANG_THAI"].Text == "Dừng gửi") is_send = true;
                else
                {
                    strMsgW = "Chỉ gửi lại được tin đã gửi hoặc dừng gửi!";
                }
                if (id_thnx != null && is_send)
                {
                    res = thBO.updateTrangThaiGui(id_thnx.Value, Sys_User.ID);
                    if (res.Res)
                    {
                        success++;
                        logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật trạng thái gửi tổng hợp NXHN " + id_thnx, Sys_User.ID, DateTime.Now);
                    }
                }
                else if (id_nx != null && is_send)
                {
                    res = nxhnBO.updateTrangThaiGui(id_nx.Value, Sys_User.ID);
                    if (res.Res)
                    {
                        success++;
                        logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật trạng thái gửi NXHN " + id_nx, Sys_User.ID, DateTime.Now);
                    }
                }
            }
            string strMsg = "";
            if (!string.IsNullOrEmpty(strMsgW))
                strMsgW = "notification('error', '" + strMsgW + "');";
            if (success == 0)
            {
                strMsg = "notification('error', 'Không có bản ghi nào được cập nhật trạng thái.'); ";
            }
            else strMsg = "notification('success', 'Có " + success + " bản ghi được cập nhật trạng thái gửi.'); ";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg + " " + strMsgW, true);
            RadGrid1.Rebind();
        }
        protected void btGuiLaiTin_Click(object sender, EventArgs e)
        {
            #region check quyen
            if (Sys_User.IS_ROOT == null || (Sys_User.IS_ROOT != null && Sys_User.IS_ROOT == false))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện chức năng này.');", true);
                return;
            }
            #endregion
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            int success = 0;
            string strMsgW = "";
            foreach (GridDataItem item in RadGrid1.SelectedItems)
            {
                long? id_tin = local_api.ConvertStringTolong(item.GetDataKeyValue("ID").ToString());
                bool is_send = false;
                if (item["TRANG_THAI"].Text == "1" || item["TRANG_THAI"].Text == "3" || item["TRANG_THAI"].Text == "Thành công" || item["TRANG_THAI"].Text == "Dừng gửi") is_send = true;
                else
                {
                    strMsgW = "Chỉ gửi lại được tin đã gửi hoặc dừng gửi!";
                }
                if (id_tin != null && is_send)
                {
                    res = tinNhanBO.updateGuiLaiTin(id_tin.Value, Sys_This_Truong.ID, Sys_User.ID);
                    if (res.Res)
                    {
                        success++;
                        logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật trạng thái gửi lại tin nhắn " + id_tin, Sys_User.ID, DateTime.Now);
                    }
                }
            }
            string strMsg = "";
            if (!string.IsNullOrEmpty(strMsgW))
                strMsgW = "notification('error', '" + strMsgW + "');";
            if (success == 0)
            {
                strMsg = "notification('error', 'Không có bản ghi nào được cập nhật trạng thái.'); ";
            }
            else strMsg = "notification('success', 'Có " + success + " bản ghi được cập nhật trạng thái gửi.'); ";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg + " " + strMsgW, true);
            RadGrid1.Rebind();
        }
        protected string getTongTin()
        {
            lblTongTin.Text = "";
            long? tong_tin = 0;

            var date_to = rdTuNgay.SelectedDate.Value.ToString("yyyyMMdd");
            var date_from = rdDenNgay.SelectedDate.Value.AddDays(1).ToString("yyyyMMdd");
            tong_tin = tinNhanBO.getTongTinNhanByOtherCondition1(Sys_This_Truong.ID, Convert.ToInt16(Sys_Ma_Nam_hoc), date_to, date_from, localAPI.ConvertStringToShort(rcbTrangThai.SelectedValue), localAPI.ConvertStringToShort(rcbNhaMang.SelectedValue), localAPI.ConvertStringToShort(rcbLoaiTinNhan.SelectedValue), localAPI.chuyenTiengVietKhongDau(tbNoiDung.Text.Trim()), tbSDT.Text.Trim(), localAPI.ConvertStringToShort(rcbKhoi.SelectedValue), localAPI.ConvertStringTolong(rcbLop.SelectedValue), localAPI.ConvertStringToint(rcbSoTinGui.SelectedValue), localAPI.ConvertStringToint(rcbLoaiNguoiNhan.SelectedValue));
            lblTongTin.Text = "Tổng số tin gửi đi: " + tong_tin;
            return lblTongTin.Text;
        }
        protected void rcbLoaiNguoiNhan_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            getTongTin();
            RadGrid1.Rebind();
        }
    }
}