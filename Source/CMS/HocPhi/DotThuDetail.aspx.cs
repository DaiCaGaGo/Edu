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

namespace CMS.HocPhi
{
    public partial class DotThuDetail : AuthenticatePage
    {
        long? id;
        //HP_KhoanThuBO khoanThuBO = new HP_KhoanThuBO();
        HocPhiDotThuBO dotThuBO = new HocPhiDotThuBO();
        LocalAPI localAPI = new LocalAPI();
        LogUserBO logUserBO = new LogUserBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("id") != null)
            {
                try
                {
                    id = Convert.ToInt16(Request.QueryString.Get("id"));
                }
                catch { }
            }
            if (!IsPostBack)
            {
                if (id != null)
                {
                    HOC_PHI_DOT_THU detail = new HOC_PHI_DOT_THU();
                    detail = dotThuBO.getDotThuByID(id.Value);
                    if (detail != null)
                    {
                        tbTen.Text = detail.TEN;
                        tbNoiDung.Text = detail.GHI_CHU;
                        if (detail.IS_TIEN_AN == true)
                        {
                            cboTienAn.Checked = true;
                            tbTienAn.Enabled = true;
                            tbTienAn.Text = detail.SO_TIEN_AN != null ? detail.SO_TIEN_AN.ToString() : "";
                        }
                        else
                        {
                            cboTienAn.Checked = false;
                            tbTienAn.Enabled = false;
                        }
                        tbTongTien.Text = detail.TONG_TIEN != null ? detail.TONG_TIEN.ToString() : "";
                        short? loai_khoan_thu = detail.ID_DOT_THU;
                        if (loai_khoan_thu != null)
                        {
                            rcbLoaiDotThu.SelectedValue = loai_khoan_thu.ToString();
                            if (loai_khoan_thu == 2)
                            {
                                divHocKy.Visible = true;
                                divThang.Visible = false;
                                if (detail.HOC_KY != null) rcbHocKy.SelectedValue = detail.HOC_KY.ToString();
                            }
                            else if (loai_khoan_thu == 3)
                            {
                                divHocKy.Visible = false;
                                divThang.Visible = true;
                                if (detail.THANG != null) rcbThang.SelectedValue = detail.THANG.ToString();
                            }
                            else
                            {
                                divHocKy.Visible = false;
                                divThang.Visible = false;
                            }
                        }
                        if (detail.THOI_GIAN_BAT_DAU != null)
                            rdTuNgay.SelectedDate = detail.THOI_GIAN_BAT_DAU;
                        if (detail.THOI_GIAN_KET_THUC != null)
                            rdDenNgay.SelectedDate = detail.THOI_GIAN_KET_THUC;
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
            long? max_thu_tu = dotThuBO.getMaxThuTuByTruong(Sys_This_Truong.ID, Sys_Ma_Nam_hoc);
            if (max_thu_tu != null)
                tbThuTu.Text = Convert.ToString(max_thu_tu + 1);
            else tbThuTu.Text = "1";
        }
        protected void btAdd_Click(object sender, EventArgs e)
        {
            #region "check du lieu"
            if (!is_access(SYS_Type_Access.THEM))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            if (string.IsNullOrEmpty(tbTen.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Tên đợt thu không được để trống!');", true);
                tbTen.Focus();
                return;
            }
            if (string.IsNullOrEmpty(tbNoiDung.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Nội dung thu không được để trống!');", true);
                tbTen.Focus();
                return;
            }
            #endregion
            HOC_PHI_DOT_THU detail = new HOC_PHI_DOT_THU();
            detail.ID_TRUONG = Sys_This_Truong.ID;
            detail.ID_NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
            detail.TEN = tbTen.Text.Trim();
            detail.GHI_CHU = tbNoiDung.Text.Trim();
            if (cboTienAn.Checked)
            {
                detail.IS_TIEN_AN = true;
                detail.SO_TIEN_AN = localAPI.ConvertStringTolong(tbTienAn.Text.Trim());
            }
            detail.TONG_TIEN = localAPI.ConvertStringTolong(tbTongTien.Text.Trim());
            try
            {
                detail.THOI_GIAN_BAT_DAU = rdTuNgay.SelectedDate.Value;
            }
            catch { }
            try
            {
                detail.THOI_GIAN_KET_THUC = rdDenNgay.SelectedDate.Value;
            }
            catch { }
            short? loai_khoan_thu = localAPI.ConvertStringToShort(rcbLoaiDotThu.SelectedValue);
            detail.ID_DOT_THU = loai_khoan_thu;
            if (loai_khoan_thu == 2) detail.HOC_KY = localAPI.ConvertStringToShort(rcbHocKy.SelectedValue);
            else if (loai_khoan_thu == 3) detail.THANG = localAPI.ConvertStringToShort(rcbThang.SelectedValue);
            detail.THU_TU = localAPI.ConvertStringTolong(tbThuTu.Text.Trim());
            ResultEntity res = dotThuBO.insert(detail, Sys_User.ID);
            string strMsg = "";
            if (res.Res)
            {
                tbTen.Text = "";
                tbTen.Focus();
                tbNoiDung.Text = "";
                rcbLoaiDotThu.ClearSelection();
                rcbHocKy.ClearSelection();
                rcbThang.ClearSelection();
                getMaxThuTu();
                strMsg = "notification('success', '" + res.Msg + "');";
                HOC_PHI_DOT_THU resMa = (HOC_PHI_DOT_THU)res.ResObject;
                logUserBO.insert(Sys_This_Truong.ID, "INSERT", "Thêm mới khoản thu " + resMa.ID, Sys_User.ID, DateTime.Now);
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
        protected void btEdit_Click(object sender, EventArgs e)
        {
            #region "check du lieu"
            if (!is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            if (string.IsNullOrEmpty(tbTen.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Tên đợt thu không được để trống!');", true);
                tbTen.Focus();
                return;
            }
            #endregion
            HOC_PHI_DOT_THU detail = new HOC_PHI_DOT_THU();
            detail.ID = id.Value;
            detail = dotThuBO.getDotThuByID(detail.ID);
            if (detail == null) detail = new HOC_PHI_DOT_THU();
            if (detail.ID_TRUONG != Sys_This_Truong.ID)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện sửa dữ liệu này!');", true);
                return;
            }
            detail.ID_TRUONG = Sys_This_Truong.ID;
            detail.ID_NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
            detail.TEN = tbTen.Text.Trim();
            detail.GHI_CHU = tbNoiDung.Text.Trim();
            if (cboTienAn.Checked)
            {
                detail.IS_TIEN_AN = true;
                detail.SO_TIEN_AN = localAPI.ConvertStringTolong(tbTienAn.Text.Trim());
            }
            else
            {
                detail.IS_TIEN_AN = false;
                detail.SO_TIEN_AN = null;
            }
            detail.TONG_TIEN = localAPI.ConvertStringTolong(tbTongTien.Text.Trim());
            try
            {
                detail.THOI_GIAN_BAT_DAU = rdTuNgay.SelectedDate.Value;
            }
            catch { }
            try
            {
                detail.THOI_GIAN_KET_THUC = rdDenNgay.SelectedDate.Value;
            }
            catch { }
            short? loai_khoan_thu = localAPI.ConvertStringToShort(rcbLoaiDotThu.SelectedValue);
            detail.ID_DOT_THU = loai_khoan_thu;
            if (loai_khoan_thu == 2) detail.HOC_KY = localAPI.ConvertStringToShort(rcbHocKy.SelectedValue);
            else if (loai_khoan_thu == 3) detail.THANG = localAPI.ConvertStringToShort(rcbThang.SelectedValue);
            detail.THU_TU = localAPI.ConvertStringTolong(tbThuTu.Text.Trim());
            ResultEntity res = dotThuBO.update(detail, Sys_User.ID);
            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
                logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật đợt thu " + id, Sys_User.ID, DateTime.Now);
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
        protected void rcbLoaiDotThu_SelectedIndexChanged1(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (rcbLoaiDotThu.SelectedValue == "1")
            {
                divHocKy.Visible = false;
                divThang.Visible = false;
            }
            else if (rcbLoaiDotThu.SelectedValue == "2")
            {
                divHocKy.Visible = true;
                divThang.Visible = false;
            }
            else if (rcbLoaiDotThu.SelectedValue == "3")
            {
                divHocKy.Visible = false;
                divThang.Visible = true;
            }
        }

        protected void cboTienAn_CheckedChanged(object sender, EventArgs e)
        {
            if (cboTienAn.Checked) tbTienAn.Enabled = true;
            else
            {
                tbTienAn.Enabled = false;
                tbTienAn.Text = "";
            }
        }
    }
}