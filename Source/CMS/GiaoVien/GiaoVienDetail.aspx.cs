using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS.GiaoVien
{
    public partial class GiaoVienDetail : AuthenticatePage
    {
        long? id_gv;
        GiaoVienBO gvBO = new GiaoVienBO();
        LocalAPI localAPI = new LocalAPI();
        LogUserBO logUserBO = new LogUserBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("id_GiaoVien") != null)
            {
                try
                {
                    id_gv = Convert.ToInt16(Request.QueryString.Get("id_GiaoVien"));
                }
                catch { }
            }
            if (!IsPostBack)
            {
                if (id_gv != null)
                {
                    GIAO_VIEN detail = new GIAO_VIEN();
                    detail = gvBO.getGiaoVienByID(id_gv.Value);
                    if (detail != null)
                    {
                        if (detail.HO_TEN != null)
                            tbTen.Text = detail.HO_TEN.ToString();
                        if (detail.SDT != null) tbSDT.Text = detail.SDT.ToString();
                        try { rdNgaySinh.SelectedDate = detail.NGAY_SINH; }
                        catch { rdNgaySinh.SelectedDate = null; }
                        rcbGioiTinh.SelectedValue = detail.MA_GIOI_TINH.ToString();
                        if (detail.DIA_CHI != null) tbDiaChi.Text = detail.DIA_CHI.ToString();
                        if (detail.EMAIL != null) tbEmail.Text = detail.EMAIL.ToString();
                        if (detail.ID_CHUC_VU != null) rcbChucVu.SelectedValue = detail.ID_CHUC_VU.ToString();
                        if (detail.MA_TRANG_THAI != null) rcbTrangThai.SelectedValue = detail.MA_TRANG_THAI.ToString();
                        if (detail.THU_TU != null) tbThuTu.Text = detail.THU_TU.ToString();
                        btEdit.Visible = is_access(SYS_Type_Access.SUA);
                        btAdd.Visible = false;
                    }
                    else
                    {
                        btEdit.Visible = false;
                        btAdd.Visible = is_access(SYS_Type_Access.THEM);
                    }
                }
                else
                {
                    btEdit.Visible = false;
                    btAdd.Visible = is_access(SYS_Type_Access.THEM);
                    getMaxThuTu();
                }
            }
        }
        protected void getMaxThuTu()
        {
            long? max_thu_tu = gvBO.getMaxThuTuByTruong(Sys_This_Truong.ID);
            if (max_thu_tu != null)
                tbThuTu.Text = Convert.ToString(Convert.ToInt64(max_thu_tu) + 1);
            else tbThuTu.Text = "1";
        }
        protected void btAdd_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.THEM))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            string sdt = localAPI.Add84(tbSDT.Text.Trim());
            GIAO_VIEN checkGiaoVien = gvBO.checkGiaoVienByPhoneAndTruong(Sys_This_Truong.ID, sdt);
            if (sdt != "" && checkGiaoVien != null)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Số điện thoại đã tồn tại, vui lòng kiểm tra lại!');", true);
                return;
            }

            GIAO_VIEN detail = new GIAO_VIEN();
            detail.HO_TEN = tbTen.Text.Trim();
            string ho_dem = "", ten = "";
            localAPI.splitHoTen(tbTen.Text.Trim(), out ho_dem, out ten);
            detail.TEN = ten.Trim();
            detail.HO_DEM = ho_dem.Trim();
            detail.THU_TU = localAPI.ConvertStringToShort(tbThuTu.Text);
            detail.SDT = sdt;
            try { detail.NGAY_SINH = rdNgaySinh.SelectedDate; }
            catch { detail.NGAY_SINH = null; }
            detail.MA_GIOI_TINH = localAPI.ConvertStringToShort(rcbGioiTinh.SelectedValue);
            detail.ID_CHUC_VU = localAPI.ConvertStringToShort(rcbChucVu.SelectedValue);
            detail.MA_TRANG_THAI = localAPI.ConvertStringToShort(rcbTrangThai.SelectedValue);
            detail.DIA_CHI = tbDiaChi.Text.Trim() == "" ? null : tbDiaChi.Text.Trim();
            detail.EMAIL = tbEmail.Text.Trim() == "" ? null : tbEmail.Text.Trim();
            detail.ID_TRUONG = Sys_This_Truong.ID;


            ResultEntity res = gvBO.insert(detail, Sys_User.ID);
            GIAO_VIEN resGiaoVien = (GIAO_VIEN)res.ResObject;
            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
                logUserBO.insert(Sys_This_Truong.ID, "INSERT", "Thêm mới giáo viên " + resGiaoVien.ID, Sys_User.ID, DateTime.Now);
                reset();
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
        protected void reset()
        {
            tbTen.Text = "";
            tbSDT.Text = "";
            rcbTrangThai.ClearSelection();
            rcbGioiTinh.ClearSelection();
            rdNgaySinh.SelectedDate = null;
            tbDiaChi.Text = "";
            tbEmail.Text = "";
            getMaxThuTu();
        }
        protected void btEdit_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            GIAO_VIEN detail = new GIAO_VIEN();
            detail = gvBO.getGiaoVienByID(id_gv.Value);
            if (detail == null) detail = new GIAO_VIEN();
            if (detail.ID_TRUONG != Sys_This_Truong.ID)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện sửa dữ liệu này!');", true);
                return;
            }
            detail.HO_TEN = tbTen.Text.Trim();
            string ho_dem = "", ten = "";
            localAPI.splitHoTen(tbTen.Text.Trim(), out ho_dem, out ten);
            detail.TEN = ten.Trim();
            detail.HO_DEM = ho_dem.Trim();
            detail.THU_TU = localAPI.ConvertStringToShort(tbThuTu.Text);
            detail.SDT = localAPI.Add84(tbSDT.Text.Trim());
            try { detail.NGAY_SINH = rdNgaySinh.SelectedDate; }
            catch { detail.NGAY_SINH = null; }
            detail.MA_GIOI_TINH = localAPI.ConvertStringToShort(rcbGioiTinh.SelectedValue);
            detail.ID_CHUC_VU = localAPI.ConvertStringToShort(rcbChucVu.SelectedValue);
            detail.MA_TRANG_THAI = localAPI.ConvertStringToShort(rcbTrangThai.SelectedValue);
            detail.DIA_CHI = tbDiaChi.Text.Trim() == "" ? null : tbDiaChi.Text.Trim();
            detail.EMAIL = tbEmail.Text.Trim() == "" ? null : tbEmail.Text.Trim();
            detail.ID_TRUONG = Sys_This_Truong.ID;
            ResultEntity res = gvBO.update(id_gv.Value, detail.TEN, detail.SDT, detail.NGAY_SINH, detail.MA_GIOI_TINH, detail.ID_CHUC_VU, detail.MA_TRANG_THAI, detail.DIA_CHI, detail.EMAIL, detail.ID_TRUONG, Sys_User.ID, detail.THU_TU, detail.HO_DEM, detail.HO_TEN);
            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
                logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật giáo viên " + id_gv, Sys_User.ID, DateTime.Now);
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
    }
}