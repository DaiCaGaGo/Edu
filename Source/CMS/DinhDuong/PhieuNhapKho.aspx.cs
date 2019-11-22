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
    public partial class PhieuNhapKho : AuthenticatePage
    {
        KhoThucPhamBO khoThucPhamBO = new KhoThucPhamBO();
        PhieuNhapKhoBO phieuNhapKhoBO = new PhieuNhapKhoBO();
        PhieuNhapKhoChiTietBO phieuNhapKhoChiTietBO = new PhieuNhapKhoChiTietBO();
        ThucPhamBO thucPhamBO = new ThucPhamBO();
        NhomThucPhamBO nhomThucPhamBO = new NhomThucPhamBO();
        DonViTinhBO donViTinhBO = new DonViTinhBO();
        LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            btAdd.Visible = is_access(SYS_Type_Access.THEM);
            if (!IsPostBack)
            {
                btnAddContinue.Visible = false;
                btEdit.Visible = false;
                objNhanVien.SelectParameters.Add("id_truong", Sys_This_Truong.ID.ToString());
                rcbNguoiNhanHang.DataBind();
                rdNgay.SelectedDate = DateTime.Now;
                sinhMaSoPhieu();
                rcbNhomThucPham.DataBind();
                rcbThucPham.DataBind();
                getDonViTinh(localAPI.ConvertStringToShort(rcbNhomThucPham.SelectedValue), localAPI.ConvertStringTolong(rcbThucPham.SelectedValue));
                Session["PhieuNhapKhoChiTiet" + Sys_User.ID] = null;
                Session["ID_THEM_MOI" + Sys_User.ID] = null;
            }
        }
        protected void sinhMaSoPhieu()
        {
            long? max_thu_tu = phieuNhapKhoBO.getMaxPhieuByTruong(Sys_This_Truong.ID);
            if (max_thu_tu != null)
            {
                if (max_thu_tu < 9) tbMaPhieu.Text = "PN0" + (max_thu_tu + 1);
                else tbMaPhieu.Text = "PN" + Convert.ToString(max_thu_tu + 1);
            }
            else tbMaPhieu.Text = "PN01";
        }
        protected void btAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(rcbNguoiNhanHang.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Bạn phải chọn người nhận hàng!');", true);
                return;
            }
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            int success = 0;
            if (Session["PhieuNhapKhoChiTiet" + Sys_User.ID] == null)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Bạn phải nhập chi tiết phiếu!');", true);
                return;
            }
            else
            {
                DataTable dt = (DataTable)Session["PhieuNhapKhoChiTiet" + Sys_User.ID];
                if (dt.Rows.Count > 0)
                {
                    #region Lưu phiếu nhập kho
                    PHIEU_NHAP_KHO detail = new PHIEU_NHAP_KHO();
                    if (Session["ID_THEM_MOI" + Sys_User.ID] == null)
                    {
                        detail = new PHIEU_NHAP_KHO();
                        detail.ID_TRUONG = Sys_This_Truong.ID;
                        detail.MA_SO_PHIEU = tbMaPhieu.Text;
                        detail.NGAY_NHAP_KHO = Convert.ToDateTime(rdNgay.SelectedDate);
                        detail.ID_NGUOI_NHAN_HANG = Convert.ToInt64(rcbNguoiNhanHang.SelectedValue);
                        detail.TONG_GIA = localAPI.ConvertStringToDecimal(tbTongGia.Text);
                        detail.GHI_CHU = !string.IsNullOrEmpty(tbGhiChu.Text.Trim()) ? tbGhiChu.Text.Trim() : null;
                        long? max_thu_tu = phieuNhapKhoBO.getMaxPhieuByTruong(Sys_This_Truong.ID);
                        detail.THU_TU = max_thu_tu == null ? 1 : (max_thu_tu + 1);
                        res = phieuNhapKhoBO.insert(detail, Sys_User.ID);
                        PHIEU_NHAP_KHO resPhieu = (PHIEU_NHAP_KHO)res.ResObject;
                        Session["ID_THEM_MOI" + Sys_User.ID] = resPhieu.ID;
                    }
                    else
                    {
                        detail = phieuNhapKhoBO.getPhieuNhapKhoByID(Convert.ToInt64(Session["ID_THEM_MOI" + Sys_User.ID].ToString()));
                        detail.NGAY_NHAP_KHO = Convert.ToDateTime(rdNgay.SelectedDate);
                        detail.ID_NGUOI_NHAN_HANG = Convert.ToInt64(rcbNguoiNhanHang.SelectedValue);
                        detail.TONG_GIA = localAPI.ConvertStringToDecimal(tbTongGia.Text);
                        res = phieuNhapKhoBO.update(detail, Sys_User.ID);
                    }
                    #endregion
                    #region Lưu chi tiết phiếu nhập
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        PHIEU_NHAP_KHO_DETAIL checkExists = phieuNhapKhoChiTietBO.getPhieuChiTietByPhieuNhapAndThucPham(Sys_This_Truong.ID, Convert.ToInt64(Session["ID_THEM_MOI" + Sys_User.ID].ToString()), Convert.ToInt16(dt.Rows[i]["ID_NHOM_THUC_PHAM"].ToString()), Convert.ToInt64(dt.Rows[i]["ID_THUC_PHAM"].ToString()));

                        if (checkExists == null)
                        {
                            PHIEU_NHAP_KHO_DETAIL childDetail = new PHIEU_NHAP_KHO_DETAIL();
                            childDetail.ID_PHIEU_NHAP = Convert.ToInt64(Session["ID_THEM_MOI" + Sys_User.ID].ToString());
                            childDetail.ID_TRUONG = Sys_This_Truong.ID;
                            childDetail.ID_NHOM_THUC_PHAM = Convert.ToInt16(dt.Rows[i]["ID_NHOM_THUC_PHAM"].ToString());
                            childDetail.ID_THUC_PHAM = Convert.ToInt64(dt.Rows[i]["ID_THUC_PHAM"].ToString());
                            childDetail.NGAY_NHAP_KHO = Convert.ToDateTime(rdNgay.SelectedDate);
                            childDetail.DON_VI_TINH = dt.Rows[i]["DON_VI_TINH"] != null ? localAPI.ConvertStringToShort(dt.Rows[i]["DON_VI_TINH"].ToString()) : null;
                            childDetail.SO_LUONG = dt.Rows[i]["SO_LUONG"] != null ? localAPI.ConvertStringToDecimal(dt.Rows[i]["SO_LUONG"].ToString()) : null;
                            childDetail.DON_GIA = dt.Rows[i]["DON_GIA"] != null ? localAPI.ConvertStringToDecimal(dt.Rows[i]["DON_GIA"].ToString()) : null;
                            childDetail.TONG_GIA = dt.Rows[i]["TONG_GIA"] != null ? localAPI.ConvertStringToDecimal(dt.Rows[i]["TONG_GIA"].ToString()) : null;
                            res = phieuNhapKhoChiTietBO.insert(childDetail, Sys_User.ID);
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
                PHIEU_NHAP_KHO detail = phieuNhapKhoBO.getPhieuNhapKhoByID(Convert.ToInt64(Session["ID_THEM_MOI" + Sys_User.ID]));
                if (detail != null)
                {
                    detail.ID_TRUONG = Sys_This_Truong.ID;
                    detail.MA_SO_PHIEU = tbMaPhieu.Text;
                    detail.NGAY_NHAP_KHO = Convert.ToDateTime(rdNgay.SelectedDate);
                    detail.ID_NGUOI_NHAN_HANG = Convert.ToInt64(rcbNguoiNhanHang.SelectedValue);
                    detail.TONG_GIA = localAPI.ConvertStringToDecimal(tbTongGia.Text);
                    detail.GHI_CHU = !string.IsNullOrEmpty(tbGhiChu.Text.Trim()) ? tbGhiChu.Text.Trim() : null;
                    res = phieuNhapKhoBO.update(detail, Sys_User.ID);
                }
            }
            if (res.Res)
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('success', 'Cập nhật thành công!');", true);
            else
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Không có bản ghi nào được cập nhật!');", true);

            #region cập nhật
            //if (Session["ID_THEM_MOI" + Sys_User.ID] != null)
            //{
            //    PHIEU_NHAP_KHO detail = phieuNhapKhoBO.getPhieuNhapKhoByID(Convert.ToInt64(Session["ID_THEM_MOI" + Sys_User.ID].ToString()));
            //    if (detail != null)
            //    {
            //        detail.ID_TRUONG = Sys_This_Truong.ID;
            //        detail.MA_SO_PHIEU = tbMaPhieu.Text;
            //        detail.NGAY_NHAP_KHO = Convert.ToDateTime(rdNgay.SelectedDate);
            //        detail.ID_NGUOI_NHAN_HANG = Convert.ToInt64(rcbNguoiNhanHang.SelectedValue);
            //        detail.TONG_GIA = localAPI.ConvertStringToDecimal(tbTongGia.Text);
            //        detail.GHI_CHU = !string.IsNullOrEmpty(tbGhiChu.Text.Trim()) ? tbGhiChu.Text.Trim() : null;
            //        res = phieuNhapKhoBO.update(detail, Sys_User.ID);
            //        DataTable dt = (DataTable)Session["PhieuNhapKhoChiTiet" + Sys_User.ID];
            //        #region Cập nhật dữ liệu
            //        List<PHIEU_NHAP_KHO_DETAIL> lstDetail = phieuNhapKhoChiTietBO.getPhieuNhapKhoChiTiet(Sys_This_Truong.ID, Convert.ToInt64(Session["ID_THEM_MOI" + Sys_User.ID].ToString()));
            //        if (lstDetail.Count == 0)
            //        {
            //            for (int i = 0; i < dt.Rows.Count; i++)
            //            {
            //                PHIEU_NHAP_KHO_DETAIL childDetail = new PHIEU_NHAP_KHO_DETAIL();
            //                childDetail.ID_PHIEU_NHAP = Convert.ToInt64(Session["ID_THEM_MOI" + Sys_User.ID].ToString());
            //                childDetail.ID_TRUONG = Sys_This_Truong.ID;
            //                childDetail.ID_NHOM_THUC_PHAM = Convert.ToInt16(dt.Rows[i]["ID_NHOM_THUC_PHAM"].ToString());
            //                childDetail.ID_THUC_PHAM = Convert.ToInt64(dt.Rows[i]["ID_THUC_PHAM"].ToString());
            //                childDetail.NGAY_NHAP_KHO = Convert.ToDateTime(rdNgay.SelectedDate);
            //                childDetail.DON_VI_TINH = dt.Rows[i]["DON_VI_TINH"] != null ? localAPI.ConvertStringToShort(dt.Rows[i]["DON_VI_TINH"].ToString()) : null;
            //                childDetail.SO_LUONG = dt.Rows[i]["SO_LUONG"] != null ? localAPI.ConvertStringToDecimal(dt.Rows[i]["SO_LUONG"].ToString()) : null;
            //                childDetail.DON_GIA = dt.Rows[i]["DON_GIA"] != null ? localAPI.ConvertStringToDecimal(dt.Rows[i]["DON_GIA"].ToString()) : null;
            //                childDetail.TONG_GIA = dt.Rows[i]["TONG_GIA"] != null ? localAPI.ConvertStringToDecimal(dt.Rows[i]["TONG_GIA"].ToString()) : null;
            //                res = phieuNhapKhoChiTietBO.insert(childDetail, Sys_User.ID);
            //                if (res.Res) success++;
            //            }
            //        }
            //        else
            //        {
            //            for (int i = 0; i < dt.Rows.Count; i++)
            //            {
            //                short id_nhom_thuc_pham = Convert.ToInt16(dt.Rows[i]["ID_NHOM_THUC_PHAM"]);
            //                long id_thuc_pham = Convert.ToInt64(dt.Rows[i]["ID_THUC_PHAM"]);
            //                #region Check nếu thực phẩm ko tồn tại trong list thì insert
            //                if (!lstDetail.Any(x => x.ID_PHIEU_NHAP == Convert.ToInt64(Session["ID_THEM_MOI" + Sys_User.ID].ToString()) && x.ID_NHOM_THUC_PHAM == id_nhom_thuc_pham && x.ID_THUC_PHAM == id_thuc_pham))
            //                {
            //                    PHIEU_NHAP_KHO_DETAIL childDetail = new PHIEU_NHAP_KHO_DETAIL();
            //                    childDetail.ID_PHIEU_NHAP = Convert.ToInt64(Session["ID_THEM_MOI" + Sys_User.ID].ToString());
            //                    childDetail.ID_TRUONG = Sys_This_Truong.ID;
            //                    childDetail.ID_NHOM_THUC_PHAM = id_nhom_thuc_pham;
            //                    childDetail.ID_THUC_PHAM = id_thuc_pham;
            //                    childDetail.NGAY_NHAP_KHO = Convert.ToDateTime(rdNgay.SelectedDate);
            //                    childDetail.DON_VI_TINH = dt.Rows[i]["DON_VI_TINH"] != null ? localAPI.ConvertStringToShort(dt.Rows[i]["DON_VI_TINH"].ToString()) : null;
            //                    childDetail.SO_LUONG = dt.Rows[i]["SO_LUONG"] != null ? localAPI.ConvertStringToDecimal(dt.Rows[i]["SO_LUONG"].ToString()) : null;
            //                    childDetail.DON_GIA = dt.Rows[i]["DON_GIA"] != null ? localAPI.ConvertStringToDecimal(dt.Rows[i]["DON_GIA"].ToString()) : null;
            //                    childDetail.TONG_GIA = dt.Rows[i]["TONG_GIA"] != null ? localAPI.ConvertStringToDecimal(dt.Rows[i]["TONG_GIA"].ToString()) : null;
            //                    res = phieuNhapKhoChiTietBO.insert(childDetail, Sys_User.ID);
            //                    if (res.Res) success++;
            //                }
            //                #endregion
            //                #region item bằng nhau thì cập nhật và loại item này ra khỏi list
            //                else
            //                {
            //                    for (int k = 0; k < lstDetail.Count; k++)
            //                    {
            //                        if (lstDetail[k].ID_THUC_PHAM == id_thuc_pham)
            //                        {
            //                            PHIEU_NHAP_KHO_DETAIL childDetail = phieuNhapKhoChiTietBO.getPhieuChiTietByPhieuNhapAndThucPham(Sys_This_Truong.ID, Convert.ToInt64(Session["ID_THEM_MOI" + Sys_User.ID].ToString()), Convert.ToInt16(dt.Rows[i]["ID_NHOM_THUC_PHAM"].ToString()), id_thuc_pham);
            //                            childDetail.NGAY_NHAP_KHO = Convert.ToDateTime(rdNgay.SelectedDate);
            //                            childDetail.DON_VI_TINH = dt.Rows[i]["DON_VI_TINH"] != null ? localAPI.ConvertStringToShort(dt.Rows[i]["DON_VI_TINH"].ToString()) : null;
            //                            childDetail.SO_LUONG = dt.Rows[i]["SO_LUONG"] != null ? localAPI.ConvertStringToDecimal(dt.Rows[i]["SO_LUONG"].ToString()) : null;
            //                            childDetail.DON_GIA = dt.Rows[i]["DON_GIA"] != null ? localAPI.ConvertStringToDecimal(dt.Rows[i]["DON_GIA"].ToString()) : null;
            //                            childDetail.TONG_GIA = dt.Rows[i]["TONG_GIA"] != null ? localAPI.ConvertStringToDecimal(dt.Rows[i]["TONG_GIA"].ToString()) : null;
            //                            res = phieuNhapKhoChiTietBO.update(childDetail, Sys_User.ID);
            //                            if (res.Res)
            //                            {
            //                                success++;
            //                                lstDetail.RemoveAll(x => x.ID_PHIEU_NHAP == Convert.ToInt64(Session["ID_THEM_MOI" + Sys_User.ID].ToString()) && x.ID_THUC_PHAM == id_thuc_pham);
            //                            }
            //                            break;
            //                        }
            //                    }
            //                }
            //                #endregion
            //            }
            //            #region Xóa dữ liệu còn lại trong list ban đầu
            //            if (lstDetail.Count > 0)
            //            {
            //                for (int i = 0; i < lstDetail.Count; i++)
            //                {
            //                    res = phieuNhapKhoChiTietBO.delete(lstDetail[i].ID, Sys_User.ID, true);
            //                }
            //            }
            //            #endregion
            //        }
            //        #endregion
            //    }
            //    if (success > 0)
            //    {
            //        strMsg = " notification('success', 'Cập nhật thành công.');";
            //    }
            //    else
            //    {
            //        strMsg = " notification('error', 'Có lỗi xảy ra');";
            //    }
            //    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            //    RadGrid1.Rebind();
            //    btAdd.Visible = false;
            //    btnAddContinue.Visible = is_access(SYS_Type_Access.THEM);
            //}
            //else
            //{
            //    strMsg = " notification('warning', 'Không có bản ghi nào được cập nhật');";
            //    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "", true);
            //}
            #endregion
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
            RadGrid1.DataSource = Session["PhieuNhapKhoChiTiet" + Sys_User.ID];
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
            decimal so_luong = Convert.ToDecimal(tbSoLuong.Text.Trim());
            decimal don_gia = Convert.ToDecimal(tbDonGia.Text.Trim());
            decimal tong_gia = so_luong * don_gia;
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
            long id_phieu_nhap = 0;
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
                count_them++;
                Session["PhieuNhapKhoChiTiet" + Sys_User.ID] = dtPhieuChiTiet;
                #region insert phieu chi tiet
                if (Session["ID_THEM_MOI" + Sys_User.ID] != null && Session["ID_THEM_MOI" + Sys_User.ID].ToString() != "")
                {
                    id_phieu_nhap = Convert.ToInt64(Session["ID_THEM_MOI" + Sys_User.ID]);
                    PHIEU_NHAP_KHO_DETAIL phieuDetail = new PHIEU_NHAP_KHO_DETAIL();
                    phieuDetail = phieuNhapKhoChiTietBO.getPhieuChiTietByPhieuNhapAndThucPham(Sys_This_Truong.ID, id_phieu_nhap, Convert.ToInt16(rcbNhomThucPham.SelectedValue), Convert.ToInt64(rcbThucPham.SelectedValue));
                    if (phieuDetail == null)
                    {
                        phieuDetail = new PHIEU_NHAP_KHO_DETAIL();
                        phieuDetail.ID_PHIEU_NHAP = id_phieu_nhap;
                        phieuDetail.ID_TRUONG = Sys_This_Truong.ID;
                        phieuDetail.ID_NHOM_THUC_PHAM = Convert.ToInt16(rcbNhomThucPham.SelectedValue);
                        phieuDetail.ID_THUC_PHAM = Convert.ToInt64(rcbThucPham.SelectedValue);
                        phieuDetail.NGAY_NHAP_KHO = Convert.ToDateTime(rdNgay.SelectedDate);
                        if (hdDonViTinh.Value != "0") phieuDetail.DON_VI_TINH = Convert.ToInt16(hdDonViTinh.Value);
                        phieuDetail.SO_LUONG = so_luong;
                        phieuDetail.DON_GIA = don_gia;
                        phieuDetail.TONG_GIA = tong_gia;
                        res = phieuNhapKhoChiTietBO.insert(phieuDetail, Sys_User.ID);
                    }
                }
                #endregion
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Thực phẩm này đã tồn tại. Vui lòng kiểm tra lại!');", true);
                return;
            }
            decimal gia_phieu = 0;
            DataTable dtNew = (DataTable)Session["PhieuNhapKhoChiTiet" + Sys_User.ID];
            if (dtNew != null && dtNew.Rows.Count > 0)
            {
                for (int i = 0; i < dtNew.Rows.Count; i++)
                {
                    gia_phieu += dtNew.Rows[i]["TONG_GIA"] != null ? Convert.ToDecimal(dtNew.Rows[i]["TONG_GIA"].ToString()) : 0;
                }
            }
            if (id_phieu_nhap > 0)
            {
                phieuNhapKhoBO.updateGiaPhieu(id_phieu_nhap, gia_phieu, Sys_User.ID);
            }
            tbTongGia.Text = gia_phieu > 0 ? gia_phieu.ToString() : "";
            if (res.Res || count_them > 0)
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('success', 'Thêm thành công!');", true);
            else
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Không có bản ghi nào được cập nhật!');", true);
            RadGrid1.Rebind();
        }
        protected void btDeleteChon_Click(object sender, EventArgs e)
        {
            ResultEntity res = new ResultEntity();
            res.Msg = "Cập nhật thành công!";
            int success = 0;
            DataTable dt = new DataTable();
            if (Session["PhieuNhapKhoChiTiet" + Sys_User.ID] != null) dt = (DataTable)Session["PhieuNhapKhoChiTiet" + Sys_User.ID];
            if (Session["ID_THEM_MOI" + Sys_User.ID] != null && Session["ID_THEM_MOI" + Sys_User.ID].ToString() != "")
            {
                long id_phieu_nhap = Convert.ToInt64(Session["ID_THEM_MOI" + Sys_User.ID]);
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
                            res = phieuNhapKhoChiTietBO.deleteByPhieuID(Sys_This_Truong.ID, id_phieu_nhap, id_thuc_pham, so_luong, Sys_User.ID);
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
                    #endregion
                }
            }
            else
            {
                foreach (GridDataItem row in RadGrid1.SelectedItems)
                {
                    short id_nhom_thuc_pham = Convert.ToInt16(row["ID_NHOM_THUC_PHAM"].Text);
                    long id_thuc_pham = Convert.ToInt64(row["ID_THUC_PHAM"].Text);
                    decimal? so_luong = localAPI.ConvertStringToDecimal(row["SO_LUONG"].Text);
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
            decimal gia_phieu = 0;
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    gia_phieu += dt.Rows[i]["TONG_GIA"] != null ? Convert.ToDecimal(dt.Rows[i]["TONG_GIA"].ToString()) : 0;
                }
            }
            #region update tong gia phieu nhap
            if (Session["ID_THEM_MOI" + Sys_User.ID] != null)
            {
                PHIEU_NHAP_KHO detail = phieuNhapKhoBO.getPhieuNhapKhoByID(Convert.ToInt64(Session["ID_THEM_MOI" + Sys_User.ID]));
                if (detail != null)
                {
                    detail.TONG_GIA = gia_phieu;
                    phieuNhapKhoBO.update(detail, Sys_User.ID);
                }
            }
            #endregion
            tbTongGia.Text = gia_phieu > 0 ? gia_phieu.ToString() : "";
            if (res.Res || success > 0)
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('success', 'Cập nhật thành công!');", true);
            else
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Không có bản ghi nào được cập nhật!');", true);
            RadGrid1.Rebind();
        }
        protected void btnAddContinue_Click(object sender, EventArgs e)
        {
            DataTable dtNew = (DataTable)Session["PhieuNhapKhoChiTiet" + Sys_User.ID];
            if (dtNew != null && dtNew.Rows.Count > 0)
            {
                int count = dtNew.Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    DataRow drow = dtNew.Rows[0];
                    dtNew.Rows.Remove(drow);
                }
            }
            Session["PhieuNhapKhoChiTiet" + Sys_User.ID] = dtNew;
            Session["ID_THEM_MOI" + Sys_User.ID] = null;
            tbSoLuong.Text = "";
            tbDonGia.Text = "";
            sinhMaSoPhieu();
            tbTongGia.Text = "";
            RadGrid1.Rebind();
            btEdit.Visible = false;
            btAdd.Visible = is_access(SYS_Type_Access.THEM);
        }
    }
}