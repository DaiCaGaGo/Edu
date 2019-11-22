using ClosedXML.Excel;
using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CMS.DinhDuong
{
    public partial class DanhSachPhieuNhapKho : AuthenticatePage
    {
        PhieuNhapKhoBO phieuNhapKhoBO = new PhieuNhapKhoBO();
        PhieuNhapKhoChiTietBO phieuNhapKhoChiTietBO = new PhieuNhapKhoChiTietBO();
        GiaoVienBO giaoVienBO = new GiaoVienBO();
        KhoThucPhamBO khoThucPhamBO = new KhoThucPhamBO();
        LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            if (!IsPostBack)
            {
                rdTuNgay.SelectedDate = DateTime.Now;
                rdDenNgay.SelectedDate = DateTime.Now;
                objNhanVien.SelectParameters.Add("id_truong", Sys_This_Truong.ID.ToString());
                rcbNguoiNhanHang.DataBind();
            }
            btnNhapKho.Visible = is_access(SYS_Type_Access.THEM);
            btDeleteChon.Visible = is_access(SYS_Type_Access.XOA);
            btExport.Visible = is_access(SYS_Type_Access.EXPORT);
        }
        protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
        {
            if (e.Argument == "RadWindowManager1_Close")
            {
                Session["PhieuNhapKhoChiTiet" + Sys_User.ID] = null;
                RadGrid1.Rebind();
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DateTime? tu_ngay = null, den_ngay = null;
            try
            {
                tu_ngay = rdTuNgay.SelectedDate;
            }
            catch
            {
                tu_ngay = null;
            }
            try
            {
                den_ngay = rdDenNgay.SelectedDate;
            }
            catch { den_ngay = null; }
            if (den_ngay != null) den_ngay = den_ngay.Value.AddDays(1);
            RadGrid1.DataSource = phieuNhapKhoBO.getPhieuNhapKhoByTime(Sys_This_Truong.ID, tu_ngay, den_ngay, localAPI.ConvertStringTolong(rcbNguoiNhanHang.SelectedValue));
        }
        protected void rcbNguoiNhanHang_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void rdTuNgay_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void rdDenNgay_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void btDeleteChon_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.XOA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            int success = 0, error = 0;
            string strMsg = "";
            string str_ma_phieu_khong_xoa = "";
            ResultEntity res = new ResultEntity();
            foreach (GridDataItem row in RadGrid1.SelectedItems)
            {
                try
                {
                    long id = Convert.ToInt64(row.GetDataKeyValue("ID").ToString());
                    int count = 0;
                    PHIEU_NHAP_KHO phieuNhap = phieuNhapKhoBO.getPhieuNhapKhoByID(id);
                    if (phieuNhap != null)
                    {
                        string strNgayNhap = phieuNhap.NGAY_NHAP_KHO.ToString("dd/MM/yyyy");
                        string strNgayHienTai = DateTime.Now.ToString("dd/MM/yyyy");
                        if (Convert.ToDateTime(strNgayNhap) == Convert.ToDateTime(strNgayHienTai))
                        {
                            #region check thực phẩm còn trong kho
                            List<PHIEU_NHAP_KHO_DETAIL> lstPhieuDetail = phieuNhapKhoChiTietBO.getPhieuNhapKhoChiTiet(Sys_This_Truong.ID, id);
                            if (lstPhieuDetail.Count > 0)
                            {
                                for (int i = 0; i < lstPhieuDetail.Count; i++)
                                {
                                    short id_nhom_thuc_pham = lstPhieuDetail[i].ID_NHOM_THUC_PHAM;
                                    long id_thuc_pham = lstPhieuDetail[i].ID_THUC_PHAM;
                                    decimal? so_luong = lstPhieuDetail[i].SO_LUONG;
                                    KHO_THUC_PHAM khoTP = khoThucPhamBO.getThucPhamTrongKhoByNhomAndThucPham(Sys_This_Truong.ID, id_nhom_thuc_pham, id_thuc_pham);
                                    if (khoTP != null)
                                    {
                                        if (khoTP.SO_LUONG < lstPhieuDetail[i].SO_LUONG)
                                        {
                                            count++;
                                            if (count == 1) str_ma_phieu_khong_xoa += phieuNhapKhoBO.getPhieuNhapKhoByID(id).MA_SO_PHIEU + ", ";
                                        }
                                    }
                                }
                            }
                            #endregion
                            #region xóa phiếu nhập và trừ trong kho
                            if (count == 0)
                            {
                                if (lstPhieuDetail.Count > 0)
                                {
                                    for (int i = 0; i < lstPhieuDetail.Count; i++)
                                    {
                                        short id_nhom_thuc_pham = lstPhieuDetail[i].ID_NHOM_THUC_PHAM;
                                        long id_thuc_pham = lstPhieuDetail[i].ID_THUC_PHAM;
                                        decimal? so_luong = lstPhieuDetail[i].SO_LUONG;
                                        KHO_THUC_PHAM khoTP = khoThucPhamBO.getThucPhamTrongKhoByNhomAndThucPham(Sys_This_Truong.ID, id_nhom_thuc_pham, id_thuc_pham);
                                        if (khoTP != null)
                                        {
                                            #region Trừ đi lượng thực phẩm trong kho
                                            khoTP.SO_LUONG = khoTP.SO_LUONG - so_luong;
                                            khoThucPhamBO.update(khoTP, Sys_User.ID);
                                            #endregion
                                        }
                                    }
                                    res = phieuNhapKhoBO.delete(id, Sys_User.ID, true);
                                    if (res.Res) success++;
                                    else error++;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            if (count == 0) str_ma_phieu_khong_xoa += phieuNhapKhoBO.getPhieuNhapKhoByID(id).MA_SO_PHIEU + ", ";
                        }
                    }

                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Lỗi hệ thống. Bạn vui lòng kiểm tra lại');", true);
                }
            }
            if (str_ma_phieu_khong_xoa != "")
            {
                str_ma_phieu_khong_xoa = str_ma_phieu_khong_xoa.TrimEnd(' ').TrimEnd(',');
                strMsg = "notification('warning', 'Phiếu " + str_ma_phieu_khong_xoa + " không thể xóa.Vui lòng kiểm tra lại!');";
                //có thể do thực phẩm trong phiếu đã được xuất kho, hoặc do chỉ được xóa phiếu trong ngày
            }
            if (error > 0)
            {
                strMsg += "notification('error', 'Có " + error + " bản ghi chưa được xóa. Liên hệ với quản trị viên');";
            }
            if (success > 0)
            {
                strMsg += " notification('success', 'Có " + success + " bản ghi được xóa.');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
        }
        protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                decimal? id_nguoi_nhan_hang = localAPI.ConvertStringToDecimal(item["ID_NGUOI_NHAN_HANG"].Text);
                item["NGUOI_NHAN_HANG"].Text = giaoVienBO.getGiaoVienByTruong(Sys_This_Truong.ID.ToString()).FirstOrDefault(x => x.ID == id_nguoi_nhan_hang).HO_TEN;
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
            string newName = "Danh_sach_phieu_nhap_kho.xlsx";

            List<ExcelHeaderEntity> lstHeader = new List<ExcelHeaderEntity>();
            List<ExcelEntity> lstColumn = new List<ExcelEntity>();
            lstHeader.Add(new ExcelHeaderEntity { name = "STT", colM = 1, rowM = 1, width = 10 });
            foreach (GridColumn item in RadGrid1.MasterTableView.Columns)
            {
                #region add column
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MA_SO_PHIEU") && item.UniqueName == "MA_SO_PHIEU")
                {
                    DataColumn col = new DataColumn("MA_SO_PHIEU");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Mã phiếu", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "MA_SO_PHIEU", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "NGAY_NHAP_KHO") && item.UniqueName == "NGAY_NHAP_KHO")
                {
                    DataColumn col = new DataColumn("NGAY_NHAP_KHO");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Ngày nhập", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "NGAY_NHAP_KHO", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "NGUOI_NHAN_HANG") && item.UniqueName == "NGUOI_NHAN_HANG")
                {
                    DataColumn col = new DataColumn("NGUOI_NHAN_HANG");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Người nhận hàng", colM = 1, rowM = 1, width = 30 });
                    lstColumn.Add(new ExcelEntity { Name = "NGUOI_NHAN_HANG", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TONG_GIA") && item.UniqueName == "TONG_GIA")
                {
                    DataColumn col = new DataColumn("TONG_GIA");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tổng tiền", colM = 1, rowM = 1, width = 30 });
                    lstColumn.Add(new ExcelEntity { Name = "TONG_GIA", Align = XLAlignmentHorizontalValues.Right, Color = XLColor.Black, Type = "String" });
                }
                #endregion
            }
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
            string tieuDe = "DANH SÁCH PHIẾU NHẬP KHO";
            string hocKyNamHoc = "";
            listCell.Add(new ExcelHeaderEntity { name = tenSo, colM = 3, rowM = 1, colName = "A", rowIndex = 1, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tenPhong, colM = 3, rowM = 1, colName = "A", rowIndex = 2, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tieuDe, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 3, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            listCell.Add(new ExcelHeaderEntity { name = hocKyNamHoc, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 4, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            string nameFileOutput = newName;
            localAPI.ExportExcelDynamic(serverPath, path, newName, nameFileOutput, 1, listCell, rowHeaderStart, rowStart, colStart, colEnd, dt, lstHeader, lstColumn, true);
        }
    }
}