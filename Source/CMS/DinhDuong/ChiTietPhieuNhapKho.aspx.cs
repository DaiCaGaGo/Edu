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
    public partial class ChiTietPhieuNhapKho : AuthenticatePage
    {
        PhieuNhapKhoBO phieuNhapKhoBO = new PhieuNhapKhoBO();
        PhieuNhapKhoChiTietBO phieuNhapKhoChiTietBO = new PhieuNhapKhoChiTietBO();
        ThucPhamBO thucPhamBO = new ThucPhamBO();
        NhomThucPhamBO nhomThucPhamBO = new NhomThucPhamBO();
        KhoThucPhamBO khoThucPhamBO = new KhoThucPhamBO();
        DonViTinhBO donViTinhBO = new DonViTinhBO();
        LocalAPI localAPI = new LocalAPI();
        long? id_phieu_nhap;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("id_phieu_nhap") != null)
            {
                try
                {
                    id_phieu_nhap = Convert.ToInt64(Request.QueryString.Get("id_phieu_nhap"));
                }
                catch { }
            }
            if (!IsPostBack)
            {
                objNhanVien.SelectParameters.Add("id_truong", Sys_This_Truong.ID.ToString());
                rcbNguoiNhanHang.DataBind();
                rdNgay.SelectedDate = DateTime.Now;
                rcbNhomThucPham.DataBind();
                rcbThucPham.DataBind();
                getDonViTinh(localAPI.ConvertStringToShort(rcbNhomThucPham.SelectedValue), localAPI.ConvertStringTolong(rcbThucPham.SelectedValue));
                PHIEU_NHAP_KHO detail = new PHIEU_NHAP_KHO();
                if (id_phieu_nhap != null)
                {
                    detail = phieuNhapKhoBO.getPhieuNhapKhoByID(id_phieu_nhap.Value);
                    if (detail != null)
                    {
                        tbMaPhieu.Text = detail.MA_SO_PHIEU;
                        try
                        {
                            rdNgay.SelectedDate = detail.NGAY_NHAP_KHO;
                        }
                        catch { }
                        rcbNguoiNhanHang.SelectedValue = detail.ID_NGUOI_NHAN_HANG.ToString();
                        tbTongGia.Text = detail.TONG_GIA != null ? detail.TONG_GIA.ToString() : "";
                        tbGhiChu.Text = detail.GHI_CHU != null ? detail.GHI_CHU : "";
                        #region add chi tiết phiếu vào session
                        createSession();
                        List<PHIEU_NHAP_KHO_DETAIL> lstDetail = phieuNhapKhoChiTietBO.getPhieuNhapKhoChiTiet(Sys_This_Truong.ID, id_phieu_nhap.Value);
                        DataTable dt = (DataTable)Session["PhieuNhapKhoChiTiet" + Sys_User.ID];
                        if (lstDetail.Count > 0)
                        {
                            for (int i = 0; i < lstDetail.Count; i++)
                            {
                                DataRow drow = dt.NewRow();
                                drow["ID_NHOM_THUC_PHAM"] = lstDetail[i].ID_NHOM_THUC_PHAM;
                                drow["ID_THUC_PHAM"] = lstDetail[i].ID_THUC_PHAM;
                                drow["DON_VI_TINH"] = lstDetail[i].DON_VI_TINH;
                                drow["SO_LUONG"] = lstDetail[i].SO_LUONG;
                                drow["DON_GIA"] = lstDetail[i].DON_GIA;
                                drow["TONG_GIA"] = lstDetail[i].TONG_GIA;
                                dt.Rows.Add(drow);
                            }
                        }
                        Session["PhieuNhapKhoChiTiet" + Sys_User.ID] = dt;
                        #endregion
                        #region Chỉ được sửa, xóa phiếu trong ngày
                        string strNgayNhap = detail.NGAY_NHAP_KHO.ToString("dd/MM/yyyy");
                        string strNgayHienTai = DateTime.Now.ToString("dd/MM/yyyy");
                        if (strNgayNhap != strNgayHienTai)
                        {
                            btDeleteChon.Enabled = false;
                            btEdit.Enabled = false;
                            btnThemThucPham.Enabled = false;
                        }
                        else
                        {
                            btDeleteChon.Enabled = is_access(SYS_Type_Access.XOA);
                            btEdit.Enabled = is_access(SYS_Type_Access.SUA);
                            btnThemThucPham.Enabled = is_access(SYS_Type_Access.SUA);
                        }
                        #endregion
                    }
                }
            }
        }
        protected void getDonViTinh(short? id_nhom_thuc_pham, long? id_thuc_pham)
        {
            short? don_vi_tinh = thucPhamBO.getDonViTinhByThucPham(id_nhom_thuc_pham, id_thuc_pham);
            if (don_vi_tinh != null)
            {
                lblDonViTinh.Text = "(" + donViTinhBO.getDonViTinh().FirstOrDefault(x => x.ID == don_vi_tinh).TEN + ")";
                hdDonViTinh.Value = don_vi_tinh.Value.ToString();
            }
            else lblDonViTinh.Text = "";
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (id_phieu_nhap != null)
            {
                if (Session["PhieuNhapKhoChiTiet" + Sys_User.ID] == null) createSession();
                DataTable dt = (DataTable)Session["PhieuNhapKhoChiTiet" + Sys_User.ID];
                RadGrid1.DataSource = Session["PhieuNhapKhoChiTiet" + Sys_User.ID];
            }
        }
        protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                short? id_nhom = localAPI.ConvertStringToShort(item["ID_NHOM_THUC_PHAM"].Text);
                if (id_nhom != null)
                    item["TEN_NHOM_THUC_PHAM"].Text = nhomThucPhamBO.getNhomThucPham("").FirstOrDefault(x => x.ID == id_nhom.Value).TEN;
                long? id_thuc_pham = localAPI.ConvertStringTolong(item["ID_THUC_PHAM"].Text);
                if (id_thuc_pham != null)
                    item["TEN_THUC_PHAM"].Text = thucPhamBO.getThucPham(id_nhom, "").First(x => x.ID == id_thuc_pham).TEN;
                short? don_vi_tinh = localAPI.ConvertStringToShort(item["DON_VI_TINH"].Text);
                if (don_vi_tinh != null)
                    item["DON_VI"].Text = donViTinhBO.getDonViTinh().FirstOrDefault(x => x.ID == don_vi_tinh.Value).TEN;
                //item["DON_VI_TINH"].Text = donViTinhBO.getDonViTinh().FirstOrDefault(x => x.ID == don_vi_tinh.Value).TEN;
            }
        }
        protected void rcbNhomThucPham_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbThucPham.ClearSelection();
            rcbThucPham.Text = string.Empty;
            rcbThucPham.DataBind();
            getDonViTinh(localAPI.ConvertStringToShort(rcbNhomThucPham.SelectedValue), localAPI.ConvertStringTolong(rcbThucPham.SelectedValue));
        }
        protected void rcbThucPham_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            getDonViTinh(localAPI.ConvertStringToShort(rcbNhomThucPham.SelectedValue), localAPI.ConvertStringTolong(rcbThucPham.SelectedValue));
        }
        protected void btDeleteChon_Click(object sender, EventArgs e)
        {
            int success = 0;
            int count_khong_xoa = 0;
            ResultEntity res = new ResultEntity();
            DataTable dt = new DataTable();
            if (Session["PhieuNhapKhoChiTiet" + Sys_User.ID] != null) dt = (DataTable)Session["PhieuNhapKhoChiTiet" + Sys_User.ID];
            if (id_phieu_nhap != null)
            {
                foreach (GridDataItem row in RadGrid1.SelectedItems)
                {
                    short id_nhom_thuc_pham = Convert.ToInt16(row["ID_NHOM_THUC_PHAM"].Text);
                    long id_thuc_pham = Convert.ToInt64(row["ID_THUC_PHAM"].Text);
                    decimal? so_luong = localAPI.ConvertStringToDecimal(row["SO_LUONG"].Text);
                    #region check kho thực phẩm, nếu số lượng lớn hơn còn lại trong kho thì không thể xóa
                    KHO_THUC_PHAM kho_thuc_pham = khoThucPhamBO.getThucPhamTrongKhoByNhomAndThucPham(Sys_This_Truong.ID, id_nhom_thuc_pham, id_thuc_pham);
                    if (kho_thuc_pham != null)
                    {
                        if (kho_thuc_pham.SO_LUONG >= so_luong)
                        {
                            res = phieuNhapKhoChiTietBO.deleteByPhieuID(Sys_This_Truong.ID, id_phieu_nhap.Value, id_thuc_pham, so_luong, Sys_User.ID);
                            if (res.Res) success++;
                            #region xóa trong session
                            int count = dt.Rows.Count;
                            for (int i = 0; i < count; i++)
                            {
                                DataRow drow = dt.Rows[i];
                                if (Convert.ToInt64(drow["ID_THUC_PHAM"]) == id_thuc_pham)
                                {
                                    dt.Rows.Remove(drow);
                                    break;
                                }
                            }
                            #endregion
                        }
                        else count_khong_xoa++;
                    }
                    #endregion
                }
            }
            else
            {
                foreach (GridDataItem row in RadGrid1.SelectedItems)
                {
                    short id_nhom_thuc_pham = Convert.ToInt16(row["ID_NHOM_THUC_PHAM"].Text);
                    long id_thuc_pham = Convert.ToInt64(row["ID_THUC_PHAM"].Text);
                    int count = dt.Rows.Count;
                    for (int i = 0; i < count; i++)
                    {
                        DataRow drow = dt.Rows[i];
                        if (Convert.ToInt64(drow["ID_THUC_PHAM"]) == id_thuc_pham)
                        {
                            dt.Rows.Remove(drow);
                            success++;
                            break;
                        }
                    }
                }
            }

            Session["PhieuNhapKhoChiTiet" + Sys_User.ID] = dt;
            #region Load giá tiền và cập nhật lại phiếu nhập kho
            decimal gia_phieu = 0;
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    gia_phieu += dt.Rows[i]["TONG_GIA"] != null ? Convert.ToDecimal(dt.Rows[i]["TONG_GIA"].ToString()) : 0;
                }
            }
            tbTongGia.Text = gia_phieu > 0 ? gia_phieu.ToString() : "";
            if (id_phieu_nhap != null)
            {
                PHIEU_NHAP_KHO detail = phieuNhapKhoBO.getPhieuNhapKhoByID(id_phieu_nhap.Value);
                if (detail != null)
                {
                    detail.TONG_GIA = gia_phieu;
                    phieuNhapKhoBO.update(detail, Sys_User.ID);
                }
            }
            #endregion
            string strMsg = "";
            if (success > 0)
            {
                strMsg = "notification('success', 'Có " + success + " bản ghi được xóa');";
            }
            if (count_khong_xoa > 0)
            {
                strMsg += "notification('warning', 'Dữ liệu không thể xóa, do số lượng phiếu nhập lớn hơn lượng còn lại trong kho.');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
        }
        protected void btnThemThucPham_Click(object sender, EventArgs e)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            int success = 0;
            string strMsg = "";
            #region check value"
            if (string.IsNullOrEmpty(rcbNhomThucPham.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Bạn phải chọn nhóm thực phẩm!');", true);
                return;
            }
            if (string.IsNullOrEmpty(rcbThucPham.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Bạn phải chọn thực phẩm!');", true);
                return;
            }
            if (string.IsNullOrEmpty(tbSoLuong.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Số lượng nhập kho không được để trống!');", true);
                tbSoLuong.Focus();
                return;
            }
            if (string.IsNullOrEmpty(tbDonGia.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Giá tiền không được để trống!');", true);
                tbDonGia.Focus();
                return;
            }
            #endregion
            getDonViTinh(localAPI.ConvertStringToShort(rcbNhomThucPham.SelectedValue), localAPI.ConvertStringTolong(rcbThucPham.SelectedValue));
            createSession();
            decimal so_luong = Convert.ToDecimal(tbSoLuong.Text.Trim());
            decimal don_gia = Convert.ToDecimal(tbDonGia.Text.Trim());
            decimal tong_gia = so_luong * don_gia;
            decimal gia_phieu = 0;
            #region check nếu thực phẩm đã tồn tại thì ko được thêm vào
            DataTable dtPhieuChiTiet = (DataTable)Session["PhieuNhapKhoChiTiet" + Sys_User.ID];
            int count_trung = 0;
            if (dtPhieuChiTiet != null && dtPhieuChiTiet.Rows.Count > 0)
            {
                for (int i = 0; i < dtPhieuChiTiet.Rows.Count; i++)
                {
                    if (dtPhieuChiTiet.Rows[i]["ID_THUC_PHAM"].ToString() == rcbThucPham.SelectedValue)
                    {
                        count_trung++;
                        break;
                    }
                }
            }
            #endregion
            if (count_trung == 0)
            {
                DataRow drow = dtPhieuChiTiet.NewRow();
                drow["ID_NHOM_THUC_PHAM"] = rcbNhomThucPham.SelectedValue;
                drow["ID_THUC_PHAM"] = rcbThucPham.SelectedValue;
                drow["DON_VI_TINH"] = hdDonViTinh.Value != "0" ? hdDonViTinh.Value : null;
                drow["SO_LUONG"] = so_luong;
                drow["DON_GIA"] = don_gia;
                drow["TONG_GIA"] = tong_gia;
                dtPhieuChiTiet.Rows.Add(drow);
                Session["PhieuNhapKhoChiTiet" + Sys_User.ID] = dtPhieuChiTiet;
                #region insert vào chi tiết phiếu
                if (id_phieu_nhap != null)
                {
                    PHIEU_NHAP_KHO_DETAIL phieuDetail = new PHIEU_NHAP_KHO_DETAIL();
                    phieuDetail = phieuNhapKhoChiTietBO.getPhieuChiTietByPhieuNhapAndThucPham(Sys_This_Truong.ID, id_phieu_nhap.Value, Convert.ToInt16(rcbNhomThucPham.SelectedValue), Convert.ToInt64(rcbThucPham.SelectedValue));
                    if (phieuDetail == null)
                    {
                        phieuDetail = new PHIEU_NHAP_KHO_DETAIL();
                        phieuDetail.ID_PHIEU_NHAP = id_phieu_nhap.Value;
                        phieuDetail.ID_TRUONG = Sys_This_Truong.ID;
                        phieuDetail.ID_NHOM_THUC_PHAM = Convert.ToInt16(rcbNhomThucPham.SelectedValue);
                        phieuDetail.ID_THUC_PHAM = Convert.ToInt64(rcbThucPham.SelectedValue);
                        phieuDetail.NGAY_NHAP_KHO = Convert.ToDateTime(rdNgay.SelectedDate);
                        if (hdDonViTinh.Value != "0") phieuDetail.DON_VI_TINH = Convert.ToInt16(hdDonViTinh.Value);
                        phieuDetail.SO_LUONG = so_luong;
                        phieuDetail.DON_GIA = don_gia;
                        phieuDetail.TONG_GIA = tong_gia;
                        res = phieuNhapKhoChiTietBO.insert(phieuDetail, Sys_User.ID);
                        if (res.Res) success++;
                    }
                    #region Cập nhật giá phiếu
                    DataTable dtNew = (DataTable)Session["PhieuNhapKhoChiTiet" + Sys_User.ID];
                    if (dtNew != null && dtNew.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtNew.Rows.Count; i++)
                        {
                            gia_phieu += dtNew.Rows[i]["TONG_GIA"] != null ? Convert.ToDecimal(dtNew.Rows[i]["TONG_GIA"].ToString()) : 0;
                        }
                    }
                    phieuNhapKhoBO.updateGiaPhieu(id_phieu_nhap.Value, gia_phieu, Sys_User.ID);
                    #endregion
                }
                #endregion
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Thực phẩm này đã tồn tại. Vui lòng kiểm tra lại!');", true);
                return;
            }
            
            if (success > 0)
            {
                strMsg = " notification('success', 'Cập nhật thành công.');";
            }
            else
            {
                strMsg = " notification('error', 'Có lỗi xảy ra');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            tbTongGia.Text = gia_phieu > 0 ? gia_phieu.ToString() : "";
            RadGrid1.Rebind();
        }
        protected void btEdit_Click(object sender, EventArgs e)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            int success = 0;
            string strMsg = "";
            if (id_phieu_nhap != null)
            {
                PHIEU_NHAP_KHO detail = phieuNhapKhoBO.getPhieuNhapKhoByID(id_phieu_nhap.Value);
                if (detail != null)
                {
                    detail.ID_TRUONG = Sys_This_Truong.ID;
                    detail.MA_SO_PHIEU = tbMaPhieu.Text;
                    detail.NGAY_NHAP_KHO = Convert.ToDateTime(rdNgay.SelectedDate);
                    detail.ID_NGUOI_NHAN_HANG = Convert.ToInt64(rcbNguoiNhanHang.SelectedValue);
                    detail.TONG_GIA = localAPI.ConvertStringToDecimal(tbTongGia.Text);
                    detail.GHI_CHU = !string.IsNullOrEmpty(tbGhiChu.Text.Trim()) ? tbGhiChu.Text.Trim() : null;
                    res = phieuNhapKhoBO.update(detail, Sys_User.ID);
                    if (res.Res) success++;
                    //DataTable dt = (DataTable)Session["PhieuNhapKhoChiTiet" + Sys_User.ID];
                }
            }
            if (success > 0)
            {
                strMsg = " notification('success', 'Cập nhật thành công.');";
            }
            else
            {
                strMsg = " notification('error', 'Có lỗi xảy ra');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
        }
        protected void createSession()
        {
            DataTable dtPhieuChiTiet = new DataTable();
            if (Session["PhieuNhapKhoChiTiet" + Sys_User.ID] != null) dtPhieuChiTiet = (DataTable)Session["PhieuNhapKhoChiTiet" + Sys_User.ID];
            else
            {
                dtPhieuChiTiet.Columns.Add(new DataColumn("ID_NHOM_THUC_PHAM"));
                dtPhieuChiTiet.Columns.Add(new DataColumn("ID_THUC_PHAM"));
                dtPhieuChiTiet.Columns.Add(new DataColumn("DON_VI_TINH"));
                dtPhieuChiTiet.Columns.Add(new DataColumn("SO_LUONG"));
                dtPhieuChiTiet.Columns.Add(new DataColumn("DON_GIA"));
                dtPhieuChiTiet.Columns.Add(new DataColumn("TONG_GIA"));
            }
            Session["PhieuNhapKhoChiTiet" + Sys_User.ID] = dtPhieuChiTiet;
        }
    }
}