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
    public partial class PhieuXuatKho : AuthenticatePage
    {
        PhieuXuatKhoBO phieuXuatKhoBO = new PhieuXuatKhoBO();
        PhieuXuatKhoChiTietBO phieuXuatKhoChiTietBO = new PhieuXuatKhoChiTietBO();
        ThucPhamBO thucPhamBO = new ThucPhamBO();
        NhomThucPhamBO nhomThucPhamBO = new NhomThucPhamBO();
        DonViTinhBO donViTinhBO = new DonViTinhBO();
        KhoThucPhamBO khoThucPhamBO = new KhoThucPhamBO();
        LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            btAdd.Visible = is_access(SYS_Type_Access.THEM);
            if (!IsPostBack)
            {
                btnAddContinue.Visible = false;
                btEdit.Visible = false;
                objNhanVien.SelectParameters.Add("id_truong", Sys_This_Truong.ID.ToString());
                rcbNguoiXuatHang.DataBind();
                rdNgay.SelectedDate = DateTime.Now;
                sinhMaSoPhieu();
                rcbNhomThucPham.DataBind();
                rcbThucPham.DataBind();
                getDonViTinhVaLuongConTrongKho(localAPI.ConvertStringToShort(rcbNhomThucPham.SelectedValue), localAPI.ConvertStringTolong(rcbThucPham.SelectedValue));
                Session["PhieuXuatKhoChiTiet" + Sys_User.ID] = null;
                Session["ID_THEM_MOI_PHIEU_XUAT" + Sys_User.ID] = null;
            }
        }
        protected void sinhMaSoPhieu()
        {
            long? max_thu_tu = phieuXuatKhoBO.getMaxPhieuByTruong(Sys_This_Truong.ID);
            if (max_thu_tu != null)
            {
                if (max_thu_tu < 9) tbMaPhieu.Text = "PX0" + (max_thu_tu + 1);
                else tbMaPhieu.Text = "PX" + Convert.ToString(max_thu_tu + 1);
            }
            else tbMaPhieu.Text = "PX01";
        }
        protected void btAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(rcbNguoiXuatHang.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Bạn phải chọn người nhận hàng!');", true);
                return;
            }
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            int success = 0;
            if (Session["PhieuXuatKhoChiTiet" + Sys_User.ID] == null)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Bạn phải nhập chi tiết phiếu!');", true);
                return;
            }
            else
            {
                DataTable dt = (DataTable)Session["PhieuXuatKhoChiTiet" + Sys_User.ID];
                if (dt.Rows.Count > 0)
                {
                    #region Lưu phiếu nhập kho
                    PHIEU_XUAT_KHO detail = new PHIEU_XUAT_KHO();
                    if (Session["ID_THEM_MOI_PHIEU_XUAT" + Sys_User.ID] == null)
                    {
                        detail = new PHIEU_XUAT_KHO();
                        detail.ID_TRUONG = Sys_This_Truong.ID;
                        detail.MA_SO_PHIEU = tbMaPhieu.Text;
                        detail.NGAY_XUAT_KHO = Convert.ToDateTime(rdNgay.SelectedDate);
                        detail.ID_NGUOI_XUAT_HANG = Convert.ToInt64(rcbNguoiXuatHang.SelectedValue);
                        detail.GHI_CHU = !string.IsNullOrEmpty(tbGhiChu.Text.Trim()) ? tbGhiChu.Text.Trim() : null;
                        long? max_thu_tu = phieuXuatKhoBO.getMaxPhieuByTruong(Sys_This_Truong.ID);
                        detail.THU_TU = max_thu_tu == null ? 1 : (max_thu_tu + 1);
                        res = phieuXuatKhoBO.insert(detail, Sys_User.ID);
                        PHIEU_XUAT_KHO resPhieu = (PHIEU_XUAT_KHO)res.ResObject;
                        Session["ID_THEM_MOI_PHIEU_XUAT" + Sys_User.ID] = resPhieu.ID;
                    }
                    else
                    {
                        detail = phieuXuatKhoBO.getPhieuXuatKhoByID(Convert.ToInt64(Session["ID_THEM_MOI_PHIEU_XUAT" + Sys_User.ID].ToString()));
                        detail.NGAY_XUAT_KHO = Convert.ToDateTime(rdNgay.SelectedDate);
                        detail.ID_NGUOI_XUAT_HANG = Convert.ToInt64(rcbNguoiXuatHang.SelectedValue);
                        res = phieuXuatKhoBO.update(detail, Sys_User.ID);
                    }
                    #endregion
                    #region Lưu chi tiết phiếu nhập
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        PHIEU_XUAT_KHO_DETAIL checkExists = phieuXuatKhoChiTietBO.getPhieuChiTietByPhieuXuatAndThucPham(Sys_This_Truong.ID, Convert.ToInt64(Session["ID_THEM_MOI_PHIEU_XUAT" + Sys_User.ID].ToString()), Convert.ToInt16(dt.Rows[i]["ID_NHOM_THUC_PHAM"].ToString()), Convert.ToInt64(dt.Rows[i]["ID_THUC_PHAM"].ToString()));

                        if (checkExists == null)
                        {
                            PHIEU_XUAT_KHO_DETAIL childDetail = new PHIEU_XUAT_KHO_DETAIL();
                            childDetail.ID_PHIEU_XUAT = Convert.ToInt64(Session["ID_THEM_MOI_PHIEU_XUAT" + Sys_User.ID].ToString());
                            childDetail.ID_TRUONG = Sys_This_Truong.ID;
                            childDetail.ID_NHOM_THUC_PHAM = Convert.ToInt16(dt.Rows[i]["ID_NHOM_THUC_PHAM"].ToString());
                            childDetail.ID_THUC_PHAM = Convert.ToInt64(dt.Rows[i]["ID_THUC_PHAM"].ToString());
                            childDetail.NGAY_XUAT_KHO = Convert.ToDateTime(rdNgay.SelectedDate);
                            childDetail.DON_VI_TINH = dt.Rows[i]["DON_VI_TINH"] != null ? localAPI.ConvertStringToShort(dt.Rows[i]["DON_VI_TINH"].ToString()) : null;
                            childDetail.SO_LUONG = dt.Rows[i]["SO_LUONG"] != null ? localAPI.ConvertStringToDecimal(dt.Rows[i]["SO_LUONG"].ToString()) : null;
                            childDetail.IS_BO = Convert.ToBoolean(dt.Rows[i]["IS_BO"]);
                            res = phieuXuatKhoChiTietBO.insert(childDetail, Sys_User.ID);
                            if (res.Res) success++;
                        }
                    }
                    #endregion
                }
            }
            string strMsg = "";
            if (success > 0)
            {
                strMsg += " notification('success', 'Có " + success + " chi tiết được lưu.');";
            }
            else
            {
                strMsg += " notification('warning', 'Không có chi tiết được lưu.');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
            btEdit.Visible = is_access(SYS_Type_Access.SUA);
            btnAddContinue.Visible = is_access(SYS_Type_Access.THEM);
            btAdd.Visible = false;
        }
        protected void btEdit_Click(object sender, EventArgs e)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";

            if (Session["ID_THEM_MOI" + Sys_User.ID] != null)
            {
                PHIEU_XUAT_KHO detail = phieuXuatKhoBO.getPhieuXuatKhoByID(Convert.ToInt64(Session["ID_THEM_MOI_PHIEU_XUAT" + Sys_User.ID].ToString()));
                if (detail != null)
                {
                    detail.ID_TRUONG = Sys_This_Truong.ID;
                    detail.MA_SO_PHIEU = tbMaPhieu.Text;
                    detail.NGAY_XUAT_KHO = Convert.ToDateTime(rdNgay.SelectedDate);
                    detail.ID_NGUOI_XUAT_HANG = Convert.ToInt64(rcbNguoiXuatHang.SelectedValue);
                    detail.GHI_CHU = !string.IsNullOrEmpty(tbGhiChu.Text.Trim()) ? tbGhiChu.Text.Trim() : null;
                    res = phieuXuatKhoBO.update(detail, Sys_User.ID);
                }
            }
            if (res.Res)
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('success', 'Cập nhật thành công!');", true);
            else
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Không có bản ghi nào được cập nhật!');", true);
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
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = Session["PhieuXuatKhoChiTiet" + Sys_User.ID];
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
        protected void btnThemThucPham_Click(object sender, EventArgs e)
        {
            ResultEntity res = new ResultEntity();
            res.Msg = "Thành công";
            int count_them = 0;
            #region check value và số lượng còn trong kho
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
            decimal so_luong = Convert.ToDecimal(tbSoLuong.Text.Trim());
            if (so_luong > Convert.ToDecimal(hdSoLuongCon.Value))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Số lượng xuất ra không được lớn hơn số lượng còn trong kho!');", true);
                tbSoLuong.Focus();
                return;
            }
            #endregion
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
            #region check nếu thực phẩm đã tồn tại thì ko được thêm vào
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
            long id_phieu_xuat = 0;
            if (count_trung == 0)
            {
                DataRow drow = dtPhieuChiTiet.NewRow();
                drow["ID_NHOM_THUC_PHAM"] = rcbNhomThucPham.SelectedValue;
                drow["ID_THUC_PHAM"] = rcbThucPham.SelectedValue;
                drow["DON_VI_TINH"] = hdDonViTinh.Value != "0" ? hdDonViTinh.Value : null;
                drow["SO_LUONG"] = so_luong;
                drow["IS_BO"] = cbIS_BO.Checked;
                dtPhieuChiTiet.Rows.Add(drow);
                count_them++;
                Session["PhieuXuatKhoChiTiet" + Sys_User.ID] = dtPhieuChiTiet;
                #region insert phieu chi tiet
                if (Session["ID_THEM_MOI_PHIEU_XUAT" + Sys_User.ID] != null && Session["ID_THEM_MOI_PHIEU_XUAT" + Sys_User.ID].ToString() != "")
                {
                    id_phieu_xuat = Convert.ToInt64(Session["ID_THEM_MOI_PHIEU_XUAT" + Sys_User.ID]);
                    PHIEU_XUAT_KHO_DETAIL phieuDetail = new PHIEU_XUAT_KHO_DETAIL();
                    phieuDetail = phieuXuatKhoChiTietBO.getPhieuChiTietByPhieuXuatAndThucPham(Sys_This_Truong.ID, id_phieu_xuat, Convert.ToInt16(rcbNhomThucPham.SelectedValue), Convert.ToInt64(rcbThucPham.SelectedValue));
                    if (phieuDetail == null)
                    {
                        phieuDetail = new PHIEU_XUAT_KHO_DETAIL();
                        phieuDetail.ID_PHIEU_XUAT = id_phieu_xuat;
                        phieuDetail.ID_TRUONG = Sys_This_Truong.ID;
                        phieuDetail.ID_NHOM_THUC_PHAM = Convert.ToInt16(rcbNhomThucPham.SelectedValue);
                        phieuDetail.ID_THUC_PHAM = Convert.ToInt64(rcbThucPham.SelectedValue);
                        phieuDetail.NGAY_XUAT_KHO = Convert.ToDateTime(rdNgay.SelectedDate);
                        if (hdDonViTinh.Value != "0") phieuDetail.DON_VI_TINH = Convert.ToInt16(hdDonViTinh.Value);
                        phieuDetail.SO_LUONG = so_luong;
                        phieuDetail.IS_BO = cbIS_BO.Checked;
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
            if (res.Res || count_them > 0)
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('success', 'Thêm thành công!');", true);
            else
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('erro r', 'Không có bản ghi nào được cập nhật!');", true);
            RadGrid1.Rebind();
        }
        protected void btDeleteChon_Click(object sender, EventArgs e)
        {
            ResultEntity res = new ResultEntity();
            res.Msg = "Cập nhật thành công!";
            int success = 0;
            DataTable dt = new DataTable();
            if (Session["PhieuXuatKhoChiTiet" + Sys_User.ID] != null) dt = (DataTable)Session["PhieuXuatKhoChiTiet" + Sys_User.ID];
            if (Session["ID_THEM_MOI_PHIEU_XUAT" + Sys_User.ID] != null && Session["ID_THEM_MOI_PHIEU_XUAT" + Sys_User.ID].ToString() != "")
            {
                long id_phieu_xuat = Convert.ToInt64(Session["ID_THEM_MOI_PHIEU_XUAT" + Sys_User.ID]);
                foreach (GridDataItem row in RadGrid1.SelectedItems)
                {
                    short id_nhom_thuc_pham = Convert.ToInt16(row["ID_NHOM_THUC_PHAM"].Text);
                    long id_thuc_pham = Convert.ToInt64(row["ID_THUC_PHAM"].Text);
                    decimal? so_luong = localAPI.ConvertStringToDecimal(row["SO_LUONG"].Text);
                    res = phieuXuatKhoChiTietBO.deleteByPhieuID(Sys_This_Truong.ID, id_phieu_xuat, id_thuc_pham, so_luong, Sys_User.ID);
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
        protected void btnAddContinue_Click(object sender, EventArgs e)
        {
            DataTable dtNew = (DataTable)Session["PhieuXuatKhoChiTiet" + Sys_User.ID];
            if (dtNew != null && dtNew.Rows.Count > 0)
            {
                int count = dtNew.Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    DataRow drow = dtNew.Rows[0];
                    dtNew.Rows.Remove(drow);
                }
            }
            Session["PhieuXuatKhoChiTiet" + Sys_User.ID] = dtNew;
            Session["ID_THEM_MOI_PHIEU_XUAT" + Sys_User.ID] = null;
            tbSoLuong.Text = "";
            sinhMaSoPhieu();
            cbIS_BO.Checked = false;
            RadGrid1.Rebind();
            btEdit.Visible = false;
            btAdd.Visible = is_access(SYS_Type_Access.THEM);
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