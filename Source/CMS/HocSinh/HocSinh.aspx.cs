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

namespace CMS.HocSinh
{
    public partial class HocSinh : AuthenticatePage
    {
        private HocSinhBO hsBO = new HocSinhBO();
        private GioiTinhBO gioiTinhBO = new GioiTinhBO();
        private TrangThaiHSBO trangThaiBO = new TrangThaiHSBO();
        private LopBO lopBO = new LopBO();
        private KhoiBO khoiBO = new KhoiBO();
        private DiemChiTietBO diemChiTietBO = new DiemChiTietBO();
        private DiemTongKetBO diemTongKetBO = new DiemTongKetBO();
        LocalAPI localAPI = new LocalAPI();
        LogUserBO logUserBO = new LogUserBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            btAddNew.Visible = is_access(SYS_Type_Access.THEM);
            btnImportExcel.Visible = is_access(SYS_Type_Access.THEM);
            //btDeleteChon.Visible = is_access(SYS_Type_Access.XOA);
            btChuyenLop.Visible = is_access(SYS_Type_Access.SUA);
            btExport.Visible = is_access(SYS_Type_Access.EXPORT);
            //btDeleteByRoot.Visible = (Sys_User.IS_ROOT != true) ? false : true;
            btDeleteByRoot.Visible = is_access(SYS_Type_Access.XOA);
            if (!IsPostBack)
            {
                objKhoi.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                objKhoi.SelectParameters.Add("maLoaiLopGDTX", DbType.Int16, Sys_This_Lop_GDTX == null ? "" : Sys_This_Lop_GDTX.ToString());
                rcbKhoi.DataBind();
                objLop.SelectParameters.Add("ma_cap_hoc", Sys_This_Cap_Hoc);
                objLop.SelectParameters.Add("idTruong", Sys_This_Truong.ID.ToString());
                objLop.SelectParameters.Add("maNamHoc", Sys_Ma_Nam_hoc.ToString());
                rcbLop.DataBind();
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
            RadGrid1.DataSource = hsBO.getHocSinhByOther(Sys_This_Truong.ID, Convert.ToInt16(Sys_Ma_Nam_hoc), localAPI.ConvertStringToShort(rcbKhoi.SelectedValue), localAPI.ConvertStringTolong(rcbLop.SelectedValue), tbTen.Text.Trim(), localAPI.ConvertStringToShort(rcbDichVuSMS.SelectedValue), localAPI.ConvertStringToShort(rcbMienSMS.SelectedValue), localAPI.ConvertStringToShort(rcbTrangThai.SelectedValue), localAPI.ConvertStringToShort(rcbGioiTinh.SelectedValue), Sys_This_Cap_Hoc, tbSDT.Text.Trim());
            RadGrid1.MasterTableView.Columns.FindByUniqueName("SUA").Visible = (is_access(SYS_Type_Access.XEM) || is_access(SYS_Type_Access.SUA));
            RadGrid1.MasterTableView.Columns.FindByUniqueName("NGAY_TAO").Visible = (Sys_User.IS_ROOT != true) ? false : true;
            RadGrid1.MasterTableView.Columns.FindByUniqueName("SDT_BM").Visible = is_access(SYS_Type_Access.VIEW_INFOR);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
        protected void btDeleteChon_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.XOA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            int success = 0, error = 0; string lst_id = string.Empty;
            foreach (GridDataItem row in RadGrid1.SelectedItems)
            {
                try
                {
                    long id_hs = Convert.ToInt64(row.GetDataKeyValue("ID").ToString());
                    string ten = row["HO_TEN"].Text;
                    long id_lop = Convert.ToInt64(row["ID_LOP"].Text);
                    try
                    {
                        List<DIEM_CHI_TIET> checkExistsDiemChiTietHocSinh = diemChiTietBO.getDiemChiTietByTruongKhoiLopAndHocSinhAndCapAndHocKy(Convert.ToInt16(Sys_Ma_Nam_hoc), Sys_This_Truong.ID, Convert.ToInt16(rcbKhoi.SelectedValue), Convert.ToInt16(rcbLop.SelectedValue), Convert.ToInt16(Sys_Hoc_Ky), id_hs, Sys_This_Cap_Hoc);
                        if (checkExistsDiemChiTietHocSinh != null && checkExistsDiemChiTietHocSinh.Count > 0)
                        {
                            error++;
                            continue;
                        }
                        DIEM_TONG_KET checkExistsDiemTongKetHocSinh = diemTongKetBO.getDiemTrungBinhByHocSinh(Sys_This_Truong.ID, Convert.ToInt16(Sys_Ma_Nam_hoc), Convert.ToInt16(rcbLop.SelectedValue), id_hs);
                        if (checkExistsDiemTongKetHocSinh != null && checkExistsDiemTongKetHocSinh.ID > 0)
                        {
                            error++;
                            continue;
                        }

                        ResultEntity res = hsBO.delete(id_hs, Sys_User.ID, false, false);
                        lst_id += id_hs + ":" + ten + ", ";
                        if (res.Res)
                        {
                            success++;
                            logUserBO.insert(Sys_This_Truong.ID, "DELETE", "Xóa học sinh " + ten + " (" + id_hs + ") lớp " + id_lop, Sys_User.ID, DateTime.Now);
                        }
                        else
                            error++;
                    }
                    catch
                    {
                        error++;
                    }
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Lỗi hệ thống. Bạn vui lòng kiểm tra lại');", true);
                }
            }
            string strMsg = "";
            if (error > 0)
            {
                strMsg = "notification('error', 'Có dữ liệu liên quan không thể xóa. Vui lòng liên hệ với quản trị viên để được giúp đỡ!');";
            }
            if (success > 0)
            {
                strMsg += " notification('success', 'Có " + success + " bản ghi được xóa.');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
        }
        protected void btDeleteByRoot_Click(object sender, EventArgs e)
        {
            //if (Sys_User.IS_ROOT != true)
            //{
            //    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
            //    return;
            //}
            int success = 0, error = 0; string lst_id = string.Empty;
            foreach (GridDataItem row in RadGrid1.SelectedItems)
            {
                try
                {
                    long id_hs = Convert.ToInt64(row.GetDataKeyValue("ID").ToString());
                    string ten = row["HO_TEN"].Text;
                    long id_lop = Convert.ToInt64(row["ID_LOP"].Text);
                    try
                    {
                        ResultEntity res = hsBO.delete(id_hs, Sys_User.ID, true, true);
                        lst_id += id_hs + ":" + ten + ", ";
                        if (res.Res)
                        {
                            success++;
                            logUserBO.insert(Sys_This_Truong.ID, "DELETE", "Xóa học sinh " + ten + " (" + id_hs + ") lớp " + id_lop, Sys_User.ID, DateTime.Now);
                        }
                        else
                            error++;
                    }
                    catch
                    {
                        error++;
                    }
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Lỗi hệ thống. Bạn vui lòng kiểm tra lại');", true);
                }
            }
            string strMsg = "";
            if (error > 0)
            {
                strMsg = "notification('error', 'Có " + error + " bản ghi chưa được xóa. Liên hệ với quản trị viên');";
            }
            if (success > 0)
            {
                strMsg += " notification('success', 'Có " + success + " bản ghi được xóa.');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
        }
        protected void btTimKiem_Click(object sender, EventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void rcbDichVuSMS_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (rcbDichVuSMS.SelectedValue == "3") rcbMienSMS.Enabled = false;
            else rcbMienSMS.Enabled = true;
            RadGrid1.Rebind();
        }
        protected void rcbKhoi_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbLop.ClearSelection();
            rcbLop.Text = String.Empty;
            rcbLop.DataBind();
            RadGrid1.Rebind();
        }
        protected void rcbLop_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void rcbMienSMS_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void rcbTrangThai_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void rcbGioiTinh_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                short? ma_gioi_tinh = localAPI.ConvertStringToShort(item["MA_GIOI_TINH"].Text);
                if (ma_gioi_tinh != null)
                    item["MA_GIOI_TINH"].Text = gioiTinhBO.getGioiTinh().FirstOrDefault(x => x.MA == ma_gioi_tinh.Value).TEN;
                long? id_lop = localAPI.ConvertStringTolong(item["ID_LOP"].Text);
                if (id_lop != null)
                    item["TEN_LOP"].Text = lopBO.getAllLop().FirstOrDefault(x => x.ID == id_lop.Value).TEN;
                short? ma_trang_thai = localAPI.ConvertStringToShort(item["TRANG_THAI_HOC"].Text);
                if (ma_trang_thai != null)
                    item["TRANG_THAI_STR"].Text = trangThaiBO.getTrangThaiHS().FirstOrDefault(x => x.MA == ma_trang_thai.Value).TEN;
                if (!string.IsNullOrEmpty(item["SDT_NHAN_TIN2"].Text) && item["SDT_NHAN_TIN2"].Text != "&nbsp;")
                    item["SDT_BM"].Text = item["SDT_NHAN_TIN"].Text + "; " + item["SDT_NHAN_TIN2"].Text;
                else item["SDT_BM"].Text = item["SDT_NHAN_TIN"].Text;
                CheckBox chbIS_CON_GV = (CheckBox)e.Item.FindControl("chbIS_CON_GV");
                HiddenField hdIS_CON_GV = (HiddenField)e.Item.FindControl("hdIS_CON_GV");
                int? is_con_gv = localAPI.ConvertStringToint(hdIS_CON_GV.Value);
                chbIS_CON_GV.Checked = is_con_gv != null && is_con_gv == 1 ? true : false;
                CheckBox chbIS_GUI_BO_ME = (CheckBox)e.Item.FindControl("chbIS_GUI_BO_ME");
                HiddenField hdIS_GUI_BO_ME = (HiddenField)e.Item.FindControl("hdIS_GUI_BO_ME");
                int? is_gui_bo_me = localAPI.ConvertStringToint(hdIS_GUI_BO_ME.Value);
                chbIS_GUI_BO_ME.Checked = is_gui_bo_me != null && is_gui_bo_me == 1 ? true : false;
                CheckBox chbIS_DK_KY1 = (CheckBox)e.Item.FindControl("chbIS_DK_KY1");
                HiddenField hdIS_DK_KY1 = (HiddenField)e.Item.FindControl("hdIS_DK_KY1");
                int? is_dk_ky1 = localAPI.ConvertStringToint(hdIS_DK_KY1.Value);
                chbIS_DK_KY1.Checked = is_dk_ky1 != null && is_dk_ky1 == 1 ? true : false;
                CheckBox chbIS_DK_KY2 = (CheckBox)e.Item.FindControl("chbIS_DK_KY2");
                HiddenField hdIS_DK_KY2 = (HiddenField)e.Item.FindControl("hdIS_DK_KY2");
                int? is_dk_ky2 = localAPI.ConvertStringToint(hdIS_DK_KY2.Value);
                chbIS_DK_KY2.Checked = is_dk_ky2 != null && is_dk_ky2 == 1 ? true : false;
                CheckBox chbIS_MIEN_GIAM_KY1 = (CheckBox)e.Item.FindControl("chbIS_MIEN_GIAM_KY1");
                HiddenField hdIS_MIEN_GIAM_KY1 = (HiddenField)e.Item.FindControl("hdIS_MIEN_GIAM_KY1");
                int? is_mien_ky1 = localAPI.ConvertStringToint(hdIS_MIEN_GIAM_KY1.Value);
                chbIS_MIEN_GIAM_KY1.Checked = is_mien_ky1 != null && is_mien_ky1 == 1 ? true : false;
                CheckBox chbIS_MIEN_GIAM_KY2 = (CheckBox)e.Item.FindControl("chbIS_MIEN_GIAM_KY2");
                HiddenField hdIS_MIEN_GIAM_KY2 = (HiddenField)e.Item.FindControl("hdIS_MIEN_GIAM_KY2");
                int? is_mien_ky2 = localAPI.ConvertStringToint(hdIS_MIEN_GIAM_KY2.Value);
                chbIS_MIEN_GIAM_KY2.Checked = is_mien_ky2 != null && is_mien_ky2 == 1 ? true : false;
            }
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
            string newName = "Danh_sach_hoc_sinh.xlsx";

            List<ExcelHeaderEntity> lstHeader = new List<ExcelHeaderEntity>();
            List<ExcelEntity> lstColumn = new List<ExcelEntity>();
            lstHeader.Add(new ExcelHeaderEntity { name = "STT", colM = 1, rowM = 1, width = 10 });
            foreach (GridColumn item in RadGrid1.MasterTableView.Columns)
            {
                #region add column
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MA") && item.UniqueName == "MA")
                {
                    DataColumn col = new DataColumn("MA");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Mã HS", colM = 1, rowM = 1, width = 18 });
                    lstColumn.Add(new ExcelEntity { Name = "MA", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "HO_TEN") && item.UniqueName == "HO_TEN")
                {
                    DataColumn col = new DataColumn("HO_TEN");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Họ tên", colM = 1, rowM = 1, width = 30 });
                    lstColumn.Add(new ExcelEntity { Name = "HO_TEN", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TEN_LOP") && item.UniqueName == "TEN_LOP")
                {
                    DataColumn col = new DataColumn("TEN_LOP");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Lớp", colM = 1, rowM = 1, width = 12 });
                    lstColumn.Add(new ExcelEntity { Name = "TEN_LOP", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "NGAY_SINH") && item.UniqueName == "NGAY_SINH")
                {
                    DataColumn col = new DataColumn("NGAY_SINH");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Ngày sinh", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "NGAY_SINH", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MA_GIOI_TINH") && item.UniqueName == "MA_GIOI_TINH")
                {
                    DataColumn col = new DataColumn("MA_GIOI_TINH");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Giới tính", colM = 1, rowM = 1, width = 10 });
                    lstColumn.Add(new ExcelEntity { Name = "MA_GIOI_TINH", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "SDT_BM") && item.UniqueName == "SDT_BM")
                {
                    DataColumn col = new DataColumn("SDT_BM");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "SĐT nhận tin", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "SDT_BM", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TRANG_THAI_STR") && item.UniqueName == "TRANG_THAI_STR")
                {
                    DataColumn col = new DataColumn("TRANG_THAI_STR");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Trạng thái", colM = 1, rowM = 1, width = 20 });
                    lstColumn.Add(new ExcelEntity { Name = "TRANG_THAI_STR", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "NGAY_TAO") && item.UniqueName == "NGAY_TAO")
                {
                    DataColumn col = new DataColumn("NGAY_TAO");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Ngày tạo", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "NGAY_TAO", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_CON_GV") && item.UniqueName == "IS_CON_GV")
                {
                    DataColumn col = new DataColumn("IS_CON_GV");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Con GV", colM = 1, rowM = 1, width = 10 });
                    lstColumn.Add(new ExcelEntity { Name = "IS_CON_GV", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "CoKhong" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_GUI_BO_ME") && item.UniqueName == "IS_GUI_BO_ME")
                {
                    DataColumn col = new DataColumn("IS_GUI_BO_ME");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Gửi cả bố mẹ", colM = 1, rowM = 1, width = 10 });
                    lstColumn.Add(new ExcelEntity { Name = "IS_GUI_BO_ME", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "CoKhong" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_DK_KY1") && item.UniqueName == "IS_DK_KY1")
                {
                    DataColumn col = new DataColumn("IS_DK_KY1");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "ĐK SMS kỳ I", colM = 1, rowM = 1, width = 10 });
                    lstColumn.Add(new ExcelEntity { Name = "IS_DK_KY1", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "CoKhong" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_DK_KY2") && item.UniqueName == "IS_DK_KY2")
                {
                    DataColumn col = new DataColumn("IS_DK_KY2");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "ĐK SMS kỳ II", colM = 1, rowM = 1, width = 10 });
                    lstColumn.Add(new ExcelEntity { Name = "IS_DK_KY2", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "CoKhong" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_MIEN_GIAM_KY1") && item.UniqueName == "IS_MIEN_GIAM_KY1")
                {
                    DataColumn col = new DataColumn("IS_MIEN_GIAM_KY1");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Miễn giảm kỳ I", colM = 1, rowM = 1, width = 10 });
                    lstColumn.Add(new ExcelEntity { Name = "IS_MIEN_GIAM_KY1", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "CoKhong" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_MIEN_GIAM_KY2") && item.UniqueName == "IS_MIEN_GIAM_KY2")
                {
                    DataColumn col = new DataColumn("IS_MIEN_GIAM_KY2");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Miễn giảm kỳ II", colM = 1, rowM = 1, width = 10 });
                    lstColumn.Add(new ExcelEntity { Name = "IS_MIEN_GIAM_KY2", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "CoKhong" });
                }
                #endregion
            }
            RadGrid1.AllowPaging = false;
            if (RadGrid1.SelectedItems == null || RadGrid1.SelectedItems.Count == 0)
            {
                foreach (GridDataItem item in RadGrid1.Items)
                {
                    DataRow row = dt.NewRow();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row[col.ColumnName] = item[col.ColumnName].Text.Replace("&nbsp;", " ");
                        if (col.ColumnName == "IS_DK_KY1" || col.ColumnName == "IS_DK_KY2" || col.ColumnName == "IS_CON_GV" ||
                            col.ColumnName == "IS_GUI_BO_ME" || col.ColumnName == "IS_MIEN_GIAM_KY1" || col.ColumnName == "IS_MIEN_GIAM_KY2")
                        {
                            string nameControl = col.ColumnName == "IS_DK_KY1" ? "chbIS_DK_KY1" :
                                col.ColumnName == "IS_DK_KY2" ? "chbIS_DK_KY2" :
                                col.ColumnName == "IS_MIEN_GIAM_KY1" ? "chbIS_MIEN_GIAM_KY1" :
                                col.ColumnName == "IS_MIEN_GIAM_KY2" ? "chbIS_MIEN_GIAM_KY2" :
                                col.ColumnName == "IS_CON_GV" ? "chbIS_CON_GV" : "chbIS_GUI_BO_ME";
                            CheckBox check = (CheckBox)item.FindControl(nameControl);
                            row[col.ColumnName] = check.Checked;
                        }
                    }
                    dt.Rows.Add(row);
                }
            }
            else
            {
                foreach (GridDataItem item in RadGrid1.SelectedItems)
                {
                    DataRow row = dt.NewRow();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row[col.ColumnName] = item[col.ColumnName].Text.Replace("&nbsp;", " ");
                        if (col.ColumnName == "IS_DK_KY1" || col.ColumnName == "IS_DK_KY2" || col.ColumnName == "IS_CON_GV" ||
                            col.ColumnName == "IS_GUI_BO_ME" || col.ColumnName == "IS_MIEN_GIAM_KY1" || col.ColumnName == "IS_MIEN_GIAM_KY2")
                        {
                            string nameControl = col.ColumnName == "IS_DK_KY1" ? "chbIS_DK_KY1" :
                                col.ColumnName == "IS_DK_KY2" ? "chbIS_DK_KY2" :
                                col.ColumnName == "IS_MIEN_GIAM_KY1" ? "chbIS_MIEN_GIAM_KY1" :
                                col.ColumnName == "IS_MIEN_GIAM_KY2" ? "chbIS_MIEN_GIAM_KY2" :
                                col.ColumnName == "IS_CON_GV" ? "chbIS_CON_GV" : "chbIS_GUI_BO_ME";
                            CheckBox check = (CheckBox)item.FindControl(nameControl);
                            row[col.ColumnName] = check.Checked;
                        }
                    }
                    dt.Rows.Add(row);
                }
            }
            int rowHeaderStart = 6;
            int rowStart = 7;
            string colStart = "A";
            string colEnd = localAPI.GetExcelColumnName(lstColumn.Count);
            List<ExcelHeaderEntity> listCell = new List<ExcelHeaderEntity>();
            string tenSo = "";
            string tenPhong = Sys_This_Truong.TEN;
            string tieuDe = "DANH SÁCH HỌC SINH";
            string hocKyNamHoc = "Năm học: " + Sys_Ten_Nam_Hoc;
            listCell.Add(new ExcelHeaderEntity { name = tenSo, colM = 3, rowM = 1, colName = "A", rowIndex = 1, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tenPhong, colM = 3, rowM = 1, colName = "A", rowIndex = 2, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tieuDe, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 3, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            listCell.Add(new ExcelHeaderEntity { name = hocKyNamHoc, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 4, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            string nameFileOutput = newName;
            localAPI.ExportExcelDynamic(serverPath, path, newName, nameFileOutput, 1, listCell, rowHeaderStart, rowStart, colStart, colEnd, dt, lstHeader, lstColumn, true);
        }
        protected void btnDangKySMS_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            int success = 0, error = 0;
            foreach (GridDataItem item in RadGrid1.Items)
            {
                CheckBox chbIS_DK_KY1 = (CheckBox)item.FindControl("chbIS_DK_KY1");
                CheckBox chbIS_DK_KY2 = (CheckBox)item.FindControl("chbIS_DK_KY2");
                HiddenField hdIS_DK_KY1 = (HiddenField)item.FindControl("hdIS_DK_KY1");
                HiddenField hdIS_DK_KY2 = (HiddenField)item.FindControl("hdIS_DK_KY2");
                TextBox tbTHU_TU = (TextBox)item.FindControl("tbTHU_TU");
                HiddenField hdTHU_TU = (HiddenField)item.FindControl("hdTHU_TU");
                bool is_dk_ky1 = hdIS_DK_KY1.Value == null ? false : (hdIS_DK_KY1.Value == "1" ? true : hdIS_DK_KY1.Value == "0" ? false : false);
                bool is_dk_ky2 = hdIS_DK_KY2.Value == null ? false : (hdIS_DK_KY2.Value == "1" ? true : hdIS_DK_KY2.Value == "0" ? false : false);
                if ((hdIS_DK_KY1.Value == null && chbIS_DK_KY1.Checked) ||
                    (hdIS_DK_KY1.Value != null && chbIS_DK_KY1.Checked != is_dk_ky1) ||
                    (hdIS_DK_KY2.Value == null && chbIS_DK_KY2.Checked) ||
                    (hdIS_DK_KY2.Value != null && chbIS_DK_KY2.Checked != is_dk_ky2)
                    || tbTHU_TU.Text != hdTHU_TU.Value)
                {
                    HOC_SINH detaiHS = new HOC_SINH();
                    long id_hs = Convert.ToInt64(item.GetDataKeyValue("ID").ToString());
                    detaiHS.ID = id_hs;
                    detaiHS.IS_DK_KY1 = chbIS_DK_KY1.Checked;
                    detaiHS.IS_DK_KY2 = chbIS_DK_KY2.Checked;
                    detaiHS.THU_TU = localAPI.ConvertStringToShort(tbTHU_TU.Text);
                    ResultEntity res = hsBO.updateDangKySMS(detaiHS, Sys_User.ID);
                    if (res.Res)
                        success++;
                    else
                        error++;
                }
            }
            string strMsg = "";
            if (error > 0)
            {
                strMsg = "notification('error', 'Có " + error + " chưa được lưu. Liên hệ với quản trị viên.');";
            }
            if (success > 0)
            {
                strMsg += " notification('success', 'Có " + success + " bản ghi lưu thành công!');";
            }
            else if (error == 0 && success == 0)
                strMsg += "notification('warning', 'Không có bản ghi nào được lưu!');";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
        }
        protected void btnExportAll_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.EXPORT))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            DataTable dt = new DataTable();
            string serverPath = Server.MapPath("~");
            string path = String.Format("{0}\\ExportTemplates\\FileDynamic.xlsx", Server.MapPath("~"));
            string newName = "Danh_sach_hoc_sinh_toan_truong.xlsx";

            List<ExcelHeaderEntity> lstHeader = new List<ExcelHeaderEntity>();
            List<ExcelEntity> lstColumn = new List<ExcelEntity>();
            #region Add Header
            lstHeader.Add(new ExcelHeaderEntity { name = "STT", colM = 1, rowM = 1, width = 10 });

            lstHeader.Add(new ExcelHeaderEntity { name = "Mã HS", colM = 1, rowM = 1, width = 18 });
            lstColumn.Add(new ExcelEntity { Name = "MA", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });

            lstHeader.Add(new ExcelHeaderEntity { name = "Họ tên", colM = 1, rowM = 1, width = 30 });
            lstColumn.Add(new ExcelEntity { Name = "HO_TEN", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });

            lstHeader.Add(new ExcelHeaderEntity { name = "Khối", colM = 1, rowM = 1, width = 12 });
            lstColumn.Add(new ExcelEntity { Name = "TEN_KHOI", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });

            lstHeader.Add(new ExcelHeaderEntity { name = "Lớp", colM = 1, rowM = 1, width = 12 });
            lstColumn.Add(new ExcelEntity { Name = "TEN_LOP", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });

            lstHeader.Add(new ExcelHeaderEntity { name = "Ngày sinh", colM = 1, rowM = 1, width = 15 });
            lstColumn.Add(new ExcelEntity { Name = "NGAY_SINH", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });

            lstHeader.Add(new ExcelHeaderEntity { name = "Giới tính", colM = 1, rowM = 1, width = 10 });
            lstColumn.Add(new ExcelEntity { Name = "TEN_GIOI_TINH", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });

            lstHeader.Add(new ExcelHeaderEntity { name = "SĐT nhận tin", colM = 1, rowM = 1, width = 15 });
            lstColumn.Add(new ExcelEntity { Name = "STR_SDT_NHAN_TIN", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });

            lstHeader.Add(new ExcelHeaderEntity { name = "Trạng thái", colM = 1, rowM = 1, width = 20 });
            lstColumn.Add(new ExcelEntity { Name = "TRANG_THAI_HS", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });

            lstHeader.Add(new ExcelHeaderEntity { name = "Ngày tạo", colM = 1, rowM = 1, width = 15 });
            lstColumn.Add(new ExcelEntity { Name = "NGAY_TAO", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });

            lstHeader.Add(new ExcelHeaderEntity { name = "Con GV", colM = 1, rowM = 1, width = 10 });
            lstColumn.Add(new ExcelEntity { Name = "STR_CON_GV", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });

            lstHeader.Add(new ExcelHeaderEntity { name = "ĐK SMS kỳ 1", colM = 1, rowM = 1, width = 10 });
            lstColumn.Add(new ExcelEntity { Name = "STR_DK_KY1", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });

            lstHeader.Add(new ExcelHeaderEntity { name = "ĐK SMS kỳ 2", colM = 1, rowM = 1, width = 10 });
            lstColumn.Add(new ExcelEntity { Name = "STR_DK_KY2", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });

            lstHeader.Add(new ExcelHeaderEntity { name = "Miễn SMS kỳ 1", colM = 1, rowM = 1, width = 10 });
            lstColumn.Add(new ExcelEntity { Name = "STR_MIEN_KY1", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });

            lstHeader.Add(new ExcelHeaderEntity { name = "Miễn SMS kỳ 2", colM = 1, rowM = 1, width = 10 });
            lstColumn.Add(new ExcelEntity { Name = "STR_MIEN_KY1", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            #endregion
            #region Add column
            List<HocSinhEntity> lstHocSinh = hsBO.getHocSinhByTruongCapNamHoc(Sys_This_Truong.ID, Sys_This_Cap_Hoc, Convert.ToInt16(Sys_Ma_Nam_hoc), Convert.ToInt16(Sys_Hoc_Ky));
            dt.Columns.Add(new DataColumn("MA"));
            dt.Columns.Add(new DataColumn("HO_TEN"));
            dt.Columns.Add(new DataColumn("TEN_KHOI"));
            dt.Columns.Add(new DataColumn("TEN_LOP"));
            dt.Columns.Add(new DataColumn("NGAY_SINH"));
            dt.Columns.Add(new DataColumn("TEN_GIOI_TINH"));
            dt.Columns.Add(new DataColumn("STR_SDT_NHAN_TIN"));
            dt.Columns.Add(new DataColumn("TRANG_THAI_HS"));
            dt.Columns.Add(new DataColumn("NGAY_TAO"));
            dt.Columns.Add(new DataColumn("STR_CON_GV"));
            dt.Columns.Add(new DataColumn("STR_GUI_BO_ME"));
            dt.Columns.Add(new DataColumn("STR_DK_KY1"));
            dt.Columns.Add(new DataColumn("STR_DK_KY2"));
            dt.Columns.Add(new DataColumn("STR_MIEN_KY1"));
            dt.Columns.Add(new DataColumn("STR_MIEN_KY2"));
            if (lstHocSinh.Count > 0)
            {
                for (int i = 0; i < lstHocSinh.Count; i++)
                {
                    DataRow row = dt.NewRow();
                    row["MA"] = lstHocSinh[i].MA;
                    row["HO_TEN"] = lstHocSinh[i].HO_TEN;
                    row["TEN_KHOI"] = lstHocSinh[i].TEN_KHOI;
                    row["TEN_LOP"] = lstHocSinh[i].TEN_LOP;
                    row["NGAY_SINH"] = lstHocSinh[i].STR_NGAY_SINH;
                    row["TEN_GIOI_TINH"] = lstHocSinh[i].TEN_GIOI_TINH;
                    row["STR_SDT_NHAN_TIN"] = lstHocSinh[i].STR_SDT_NHAN_TIN;
                    row["TRANG_THAI_HS"] = lstHocSinh[i].TRANG_THAI_HS;
                    row["NGAY_TAO"] = lstHocSinh[i].NGAY_TAO != null ? lstHocSinh[i].NGAY_TAO.Value.ToString("dd/MM/yyyy") : "";
                    row["STR_CON_GV"] = lstHocSinh[i].STR_CON_GV;
                    row["STR_GUI_BO_ME"] = lstHocSinh[i].STR_GUI_BO_ME;
                    row["STR_DK_KY1"] = lstHocSinh[i].STR_DK_KY1;
                    row["STR_DK_KY2"] = lstHocSinh[i].STR_DK_KY2;
                    row["STR_MIEN_KY1"] = lstHocSinh[i].STR_MIEN_KY1;
                    row["STR_MIEN_KY2"] = lstHocSinh[i].STR_MIEN_KY2;
                    dt.Rows.Add(row);
                }
            }
            #endregion
            int rowHeaderStart = 6;
            int rowStart = 7;
            string colStart = "A";
            string colEnd = localAPI.GetExcelColumnName(lstColumn.Count);
            List<ExcelHeaderEntity> listCell = new List<ExcelHeaderEntity>();
            string tenSo = "";
            string tenPhong = Sys_This_Truong.TEN;
            string tieuDe = "DANH SÁCH HỌC SINH";
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