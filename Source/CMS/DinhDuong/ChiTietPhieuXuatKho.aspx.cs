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
    public partial class ChiTietPhieuXuatKho : AuthenticatePage
    {
        PhieuXuatKhoBO phieuXuatKhoBO = new PhieuXuatKhoBO();
        PhieuXuatKhoChiTietBO phieuXuatKhoChiTietBO = new PhieuXuatKhoChiTietBO();
        ThucPhamBO thucPhamBO = new ThucPhamBO();
        NhomThucPhamBO nhomThucPhamBO = new NhomThucPhamBO();
        DonViTinhBO donViTinhBO = new DonViTinhBO();
        KhoThucPhamBO khoThucPhamBO = new KhoThucPhamBO();
        LocalAPI localAPI = new LocalAPI();
        long? id_phieu_xuat;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("id_phieu_xuat") != null)
            {
                try
                {
                    id_phieu_xuat = Convert.ToInt64(Request.QueryString.Get("id_phieu_xuat"));
                }
                catch { }
            }
            if (!IsPostBack)
            {
                objNhanVien.SelectParameters.Add("id_truong", Sys_This_Truong.ID.ToString());
                rcbNguoiXuatHang.DataBind();
                rdNgay.SelectedDate = DateTime.Now;
                rcbNhomThucPham.DataBind();
                rcbThucPham.DataBind();
                getDonViTinhVaLuongConTrongKho(localAPI.ConvertStringToShort(rcbNhomThucPham.SelectedValue), localAPI.ConvertStringTolong(rcbThucPham.SelectedValue));
                PHIEU_XUAT_KHO detail = new PHIEU_XUAT_KHO();
                if (id_phieu_xuat != null)
                {
                    detail = phieuXuatKhoBO.getPhieuXuatKhoByID(id_phieu_xuat.Value);
                    if (detail != null)
                    {
                        tbMaPhieu.Text = detail.MA_SO_PHIEU;
                        try
                        {
                            rdNgay.SelectedDate = detail.NGAY_XUAT_KHO;
                        }
                        catch { }
                        rcbNguoiXuatHang.SelectedValue = detail.ID_NGUOI_XUAT_HANG.ToString();
                        tbGhiChu.Text = detail.GHI_CHU != null ? detail.GHI_CHU : "";
                        #region add chi tiết phiếu vào session
                        createSession();
                        List<PHIEU_XUAT_KHO_DETAIL> lstDetail = phieuXuatKhoChiTietBO.getPhieuXuatKhoChiTiet(Sys_This_Truong.ID, id_phieu_xuat.Value);
                        DataTable dt = (DataTable)Session["PhieuXuatKhoChiTiet" + Sys_User.ID];
                        if (lstDetail.Count > 0)
                        {
                            for (int i = 0; i < lstDetail.Count; i++)
                            {
                                DataRow drow = dt.NewRow();
                                drow["ID_NHOM_THUC_PHAM"] = lstDetail[i].ID_NHOM_THUC_PHAM;
                                drow["ID_THUC_PHAM"] = lstDetail[i].ID_THUC_PHAM;
                                drow["DON_VI_TINH"] = lstDetail[i].DON_VI_TINH;
                                drow["SO_LUONG"] = lstDetail[i].SO_LUONG;
                                drow["IS_BO"] = lstDetail[i].IS_BO;
                                dt.Rows.Add(drow);
                            }
                        }
                        Session["PhieuXuatKhoChiTiet" + Sys_User.ID] = dt;
                        #endregion
                        #region Chỉ được sửa, xóa phiếu trong ngày
                        string strNgayNhap = detail.NGAY_XUAT_KHO.ToString("dd/MM/yyyy");
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
            if (id_phieu_xuat != null)
            {
                if (Session["PhieuXuatKhoChiTiet" + Sys_User.ID] == null) createSession();
                RadGrid1.DataSource = Session["PhieuXuatKhoChiTiet" + Sys_User.ID];
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
                    item["SO_LUONG"].Text += " " + donViTinhBO.getDonViTinh().FirstOrDefault(x => x.ID == don_vi_tinh.Value).TEN;
            }
        }
        protected void rcbNhomThucPham_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbThucPham.ClearSelection();
            rcbThucPham.Text = string.Empty;
            rcbThucPham.DataBind();
            getDonViTinhVaLuongConTrongKho(localAPI.ConvertStringToShort(rcbNhomThucPham.SelectedValue), localAPI.ConvertStringTolong(rcbThucPham.SelectedValue));
        }
        protected void rcbThucPham_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            getDonViTinhVaLuongConTrongKho(localAPI.ConvertStringToShort(rcbNhomThucPham.SelectedValue), localAPI.ConvertStringTolong(rcbThucPham.SelectedValue));
        }
        protected void btDeleteChon_Click(object sender, EventArgs e)
        {
            int success = 0;
            ResultEntity res = new ResultEntity();
            DataTable dt = new DataTable();
            if (Session["PhieuXuatKhoChiTiet" + Sys_User.ID] != null) dt = (DataTable)Session["PhieuXuatKhoChiTiet" + Sys_User.ID];
            if (id_phieu_xuat != null)
            {
                foreach (GridDataItem row in RadGrid1.SelectedItems)
                {
                    short id_nhom_thuc_pham = Convert.ToInt16(row["ID_NHOM_THUC_PHAM"].Text);
                    long id_thuc_pham = Convert.ToInt64(row["ID_THUC_PHAM"].Text);
                    decimal? so_luong = localAPI.ConvertStringToDecimal(row["SO_LUONG"].Text);
                    res = phieuXuatKhoChiTietBO.deleteByPhieuID(Sys_This_Truong.ID, id_phieu_xuat.Value, id_thuc_pham, so_luong, Sys_User.ID);
                    if (res.Res)
                    {
                        success++;
                        getDonViTinhVaLuongConTrongKho(localAPI.ConvertStringToShort(rcbNhomThucPham.SelectedValue), localAPI.ConvertStringTolong(rcbThucPham.SelectedValue));
                    }
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
            }
            else
            {
                foreach (GridDataItem row in RadGrid1.SelectedItems)
                {
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
            Session["PhieuXuatKhoChiTiet" + Sys_User.ID] = dt;
            if (success > 0)
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('success', 'Xóa nhật thành công!');", true);
            else
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Không có bản ghi nào được xóa!');", true);
            RadGrid1.Rebind();
        }
        protected void btnThemThucPham_Click(object sender, EventArgs e)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            int success = 0;
            string strMsg = "";
            #region check value và kiểm tra lượng còn lại trong kho
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
            getDonViTinhVaLuongConTrongKho(localAPI.ConvertStringToShort(rcbNhomThucPham.SelectedValue), localAPI.ConvertStringTolong(rcbThucPham.SelectedValue));
            createSession();
            decimal so_luong = Convert.ToDecimal(tbSoLuong.Text.Trim());
            if (so_luong > Convert.ToDecimal(hdSoLuongCon.Value))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Số lượng xuất ra không được lớn hơn số lượng còn trong kho!');", true);
                tbSoLuong.Focus();
                return;
            }
            #endregion
            #region check nếu thực phẩm đã tồn tại thì ko được thêm vào
            DataTable dtPhieuChiTiet = (DataTable)Session["PhieuXuatKhoChiTiet" + Sys_User.ID];
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
                drow["IS_BO"] = cbIS_BO.Checked;
                dtPhieuChiTiet.Rows.Add(drow);
                success++;
                Session["PhieuXuatKhoChiTiet" + Sys_User.ID] = dtPhieuChiTiet;
                #region insert vào chi tiết phiếu
                if (id_phieu_xuat != null)
                {
                    PHIEU_XUAT_KHO_DETAIL phieuDetail = new PHIEU_XUAT_KHO_DETAIL();
                    phieuDetail = phieuXuatKhoChiTietBO.getPhieuChiTietByPhieuXuatAndThucPham(Sys_This_Truong.ID, id_phieu_xuat.Value, Convert.ToInt16(rcbNhomThucPham.SelectedValue), Convert.ToInt64(rcbThucPham.SelectedValue));
                    if (phieuDetail == null)
                    {
                        phieuDetail = new PHIEU_XUAT_KHO_DETAIL();
                        phieuDetail.ID_PHIEU_XUAT = id_phieu_xuat.Value;
                        phieuDetail.ID_TRUONG = Sys_This_Truong.ID;
                        phieuDetail.ID_NHOM_THUC_PHAM = Convert.ToInt16(rcbNhomThucPham.SelectedValue);
                        phieuDetail.ID_THUC_PHAM = Convert.ToInt64(rcbThucPham.SelectedValue);
                        phieuDetail.NGAY_XUAT_KHO = Convert.ToDateTime(rdNgay.SelectedDate);
                        if (hdDonViTinh.Value != "0") phieuDetail.DON_VI_TINH = Convert.ToInt16(hdDonViTinh.Value);
                        phieuDetail.IS_BO = cbIS_BO.Checked;
                        phieuDetail.SO_LUONG = so_luong;
                        res = phieuXuatKhoChiTietBO.insert(phieuDetail, Sys_User.ID);
                    }
                }
                #endregion
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Thực phẩm này đã tồn tại. Vui lòng kiểm tra lại!');", true);
                return;
            }
            if (success > 0 || res.Res)
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
        protected void btEdit_Click(object sender, EventArgs e)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            int success = 0;
            string strMsg = "";
            if (id_phieu_xuat != null)
            {
                PHIEU_XUAT_KHO detail = phieuXuatKhoBO.getPhieuXuatKhoByID(id_phieu_xuat.Value);
                if (detail != null)
                {
                    detail.ID_TRUONG = Sys_This_Truong.ID;
                    detail.MA_SO_PHIEU = tbMaPhieu.Text;
                    detail.NGAY_XUAT_KHO = Convert.ToDateTime(rdNgay.SelectedDate);
                    detail.ID_NGUOI_XUAT_HANG = Convert.ToInt64(rcbNguoiXuatHang.SelectedValue);
                    detail.GHI_CHU = !string.IsNullOrEmpty(tbGhiChu.Text.Trim()) ? tbGhiChu.Text.Trim() : null;
                    res = phieuXuatKhoBO.update(detail, Sys_User.ID);
                    if (res.Res) success++;
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
            if (Session["PhieuXuatKhoChiTiet" + Sys_User.ID] != null) dtPhieuChiTiet = (DataTable)Session["PhieuXuatKhoChiTiet" + Sys_User.ID];
            else
            {
                dtPhieuChiTiet.Columns.Add(new DataColumn("ID_NHOM_THUC_PHAM"));
                dtPhieuChiTiet.Columns.Add(new DataColumn("ID_THUC_PHAM"));
                dtPhieuChiTiet.Columns.Add(new DataColumn("DON_VI_TINH"));
                dtPhieuChiTiet.Columns.Add(new DataColumn("SO_LUONG"));
                dtPhieuChiTiet.Columns.Add(new DataColumn("IS_BO"));
            }
            Session["PhieuXuatKhoChiTiet" + Sys_User.ID] = dtPhieuChiTiet;
        }
        protected void getDonViTinhVaLuongConTrongKho(short? id_nhom_thuc_pham, long? id_thuc_pham)
        {
            short? don_vi_tinh = thucPhamBO.getDonViTinhByThucPham(id_nhom_thuc_pham, id_thuc_pham);
            if (don_vi_tinh != null)
            {
                lblDonViTinh.Text = "(" + donViTinhBO.getDonViTinh().FirstOrDefault(x => x.ID == don_vi_tinh).TEN + ")";
                hdDonViTinh.Value = don_vi_tinh.Value.ToString();
            }
            else lblDonViTinh.Text = "";
            if (id_nhom_thuc_pham != null && id_thuc_pham != null)
            {
                KHO_THUC_PHAM khoDetail = khoThucPhamBO.getThucPhamTrongKhoByNhomAndThucPham(Sys_This_Truong.ID, id_nhom_thuc_pham.Value, id_thuc_pham.Value);
                if (khoDetail != null)
                {
                    hdSoLuongCon.Value = khoDetail.SO_LUONG != null ? khoDetail.SO_LUONG.ToString() : "0";
                    if (khoDetail.SO_LUONG != null)
                        lbSoLuongCon.Text = "(lượng còn trong kho: " + khoDetail.SO_LUONG.Value + donViTinhBO.getDonViTinh().FirstOrDefault(x => x.ID == don_vi_tinh).TEN + ").";
                    else lbSoLuongCon.Text = "";
                }
                else
                {
                    hdSoLuongCon.Value = "0";
                    lbSoLuongCon.Text = "";
                }
            }
            else
            {
                hdSoLuongCon.Value = "0";
                lbSoLuongCon.Text = "";
            }
        }
    }
}