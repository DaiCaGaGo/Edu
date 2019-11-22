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

namespace CMS.DinhDuong
{
    public partial class DanhSachPhieuXuatKho : AuthenticatePage
    {
        KhoThucPhamBO khoThucPhamBO = new KhoThucPhamBO();
        PhieuXuatKhoBO phieuXuatKhoBO = new PhieuXuatKhoBO();
        PhieuXuatKhoChiTietBO phieuXuatKhoChiTietBO = new PhieuXuatKhoChiTietBO();
        GiaoVienBO giaoVienBO = new GiaoVienBO();
        LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            if (!IsPostBack)
            {
                rdTuNgay.SelectedDate = DateTime.Now;
                rdDenNgay.SelectedDate = DateTime.Now;
                objNhanVien.SelectParameters.Add("id_truong", Sys_This_Truong.ID.ToString());
                rcbNguoiXuatHang.DataBind();
            }
            btnXuat.Visible = is_access(SYS_Type_Access.THEM);
            btDeleteChon.Visible = is_access(SYS_Type_Access.XOA);
            btExport.Visible = is_access(SYS_Type_Access.EXPORT);
        }
        protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
        {
            if (e.Argument == "RadWindowManager1_Close")
            {
                Session["PhieuXuatKhoChiTiet" + Sys_User.ID] = null;
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
            RadGrid1.DataSource = phieuXuatKhoBO.getPhieuXuatKhoByTime(Sys_This_Truong.ID, tu_ngay, den_ngay, localAPI.ConvertStringTolong(rcbNguoiXuatHang.SelectedValue));
        }
        protected void rcbNguoiXuatHang_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
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
            int success = 0, error = 0;
            string strMsg = "";
            string str_ma_phieu_khong_xoa = "";
            foreach (GridDataItem row in RadGrid1.SelectedItems)
            {
                try
                {
                    long id = Convert.ToInt64(row.GetDataKeyValue("ID").ToString());
                    PHIEU_XUAT_KHO detail = new PHIEU_XUAT_KHO();
                    detail = phieuXuatKhoBO.getPhieuXuatKhoByID(id);
                    string strNgayXuat = detail.NGAY_XUAT_KHO.ToString("dd/MM/yyyy");
                    string strNgayHienTai = DateTime.Now.ToString("dd/MM/yyyy");
                    if (Convert.ToDateTime(strNgayXuat) == Convert.ToDateTime(strNgayHienTai))
                    {
                        #region Cộng lại kho thực phẩm
                        List<PHIEU_XUAT_KHO_DETAIL> lstXuat = phieuXuatKhoChiTietBO.getPhieuXuatKhoChiTiet(Sys_This_Truong.ID, id);
                        if (lstXuat.Count > 0)
                        {
                            for (int i = 0; i < lstXuat.Count; i++)
                            {
                                short id_nhom_thuc_pham = lstXuat[i].ID_NHOM_THUC_PHAM;
                                long id_thuc_pham = lstXuat[i].ID_THUC_PHAM;
                                decimal? so_luong = lstXuat[i].SO_LUONG;
                                KHO_THUC_PHAM khoTP = khoThucPhamBO.getThucPhamTrongKhoByNhomAndThucPham(Sys_This_Truong.ID, id_nhom_thuc_pham, id_thuc_pham);
                                if (khoTP != null)
                                {
                                    khoTP.SO_LUONG = khoTP.SO_LUONG + so_luong;
                                    khoThucPhamBO.update(khoTP, Sys_User.ID);
                                }
                            }
                        }
                        #endregion
                        ResultEntity res = phieuXuatKhoBO.delete(id, Sys_User.ID, true);
                        if (res.Res)
                            success++;
                        else
                            error++;
                    }
                    else
                    {
                        str_ma_phieu_khong_xoa += phieuXuatKhoBO.getPhieuXuatKhoByID(id).MA_SO_PHIEU + ", ";
                    }
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Lỗi hệ thống. Bạn vui lòng kiểm tra lại');", true);
                }
            }
            if (error > 0)
            {
                strMsg = "notification('error', 'Có " + error + " bản ghi chưa được xóa. Liên hệ với quản trị viên');";
            }
            if (success > 0)
            {
                strMsg += " notification('success', 'Có " + success + " bản ghi được xóa.');";
            }
            if (str_ma_phieu_khong_xoa != "")
            {
                str_ma_phieu_khong_xoa = str_ma_phieu_khong_xoa.TrimEnd(' ').TrimEnd(',');
                strMsg += "notification('warning', 'Phiếu " + str_ma_phieu_khong_xoa + " không thể xóa do phiếu chỉ được xóa trong ngày!');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
        }
        protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                decimal? id_nguoi_nhan_hang = localAPI.ConvertStringToDecimal(item["ID_NGUOI_XUAT_HANG"].Text);
                item["NGUOI_XUAT_HANG"].Text = giaoVienBO.getGiaoVienByTruong(Sys_This_Truong.ID.ToString()).FirstOrDefault(x => x.ID == id_nguoi_nhan_hang).HO_TEN;
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
            string newName = "Danh_sach_phieu_xuat_kho.xlsx";

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
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "NGAY_XUAT_KHO") && item.UniqueName == "NGAY_XUAT_KHO")
                {
                    DataColumn col = new DataColumn("NGAY_XUAT_KHO");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Ngày xuất", colM = 1, rowM = 1, width = 15 });
                    lstColumn.Add(new ExcelEntity { Name = "NGAY_XUAT_KHO", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "NGUOI_XUAT_HANG") && item.UniqueName == "NGUOI_XUAT_HANG")
                {
                    DataColumn col = new DataColumn("NGUOI_XUAT_HANG");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Người xuất hàng", colM = 1, rowM = 1, width = 30 });
                    lstColumn.Add(new ExcelEntity { Name = "NGUOI_XUAT_HANG", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
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
            string tieuDe = "DANH SÁCH PHIẾU XUẤT KHO";
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