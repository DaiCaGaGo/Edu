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

namespace CMS.HocPhi
{
    public partial class PhieuThuHocSinhDetail : AuthenticatePage
    {
        LocalAPI localAPI = new LocalAPI();
        HocPhiDotThuBO dotThuBO = new HocPhiDotThuBO();
        HocPhiPhieuThuHocSinhBO phieuThuHocSinhBO = new HocPhiPhieuThuHocSinhBO();
        long? id;
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
            if (id != null && id > 0)
            {
                btAdd.Visible = false;
                btEdit.Visible = is_access(SYS_Type_Access.SUA);
            }
            else
            {
                btAdd.Visible = is_access(SYS_Type_Access.THEM);
                btEdit.Visible = false;
            }
            if (!IsPostBack)
            {
                objKhoi.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                objKhoi.SelectParameters.Add("maLoaiLopGDTX", DbType.Int16, Sys_This_Lop_GDTX == null ? "" : Sys_This_Lop_GDTX.ToString());
                rcbKhoi.DataBind();
                objLop.SelectParameters.Add("ma_cap_hoc", Sys_This_Cap_Hoc);
                objLop.SelectParameters.Add("idTruong", Sys_This_Truong.ID.ToString());
                objLop.SelectParameters.Add("maNamHoc", Sys_Ma_Nam_hoc.ToString());
                rcbLop.DataBind();
                objHocSinh.SelectParameters.Add("id_truong", Sys_This_Truong.ID.ToString());
                objHocSinh.SelectParameters.Add("ma_cap_hoc", Sys_This_Cap_Hoc);
                objHocSinh.SelectParameters.Add("id_nam_hoc", Sys_Ma_Nam_hoc.ToString());
                objHocSinh.SelectParameters.Add("hoc_ky", Sys_Hoc_Ky.ToString());
                rcbHocSinh.DataBind();
                objDotThu.SelectParameters.Add("id_truong", Sys_This_Truong.ID.ToString());
                objDotThu.SelectParameters.Add("id_nam_hoc", Sys_Ma_Nam_hoc.ToString());
                rcbDotThu.DataBind();
                #region Load detail
                if (id != null)
                {
                    Session["ID_THEM_MOI" + Sys_User.ID] = id;
                    HOC_PHI_PHIEU_THU_HOC_SINH detail = phieuThuHocSinhBO.getPhieuThuHocSinhByID(id.Value);
                    if (detail != null)
                    {
                        rcbKhoi.SelectedValue = detail.ID_KHOI.ToString();
                        rcbLop.SelectedValue = detail.ID_LOP.ToString();
                        rcbHocSinh.SelectedValue = detail.ID_HOC_SINH.ToString();
                        rcbDotThu.SelectedValue = detail.ID_DOT_THU.ToString();
                        if (detail.IS_TIEN_AN != null && detail.IS_TIEN_AN == true)
                        {
                            cboTienAn.Checked = true;
                            tbTienAn.Text = detail.SO_TIEN_AN != null ? detail.SO_TIEN_AN.ToString() : "";
                        }
                        else
                        {
                            cboTienAn.Checked = false;
                            tbTienAn.Text = "";
                        }
                        tbTongTien.Text = detail.TONG_TIEN != null ? detail.TONG_TIEN.ToString() : "";
                        tbGhiChu.Text = detail.GHI_CHU != null ? detail.GHI_CHU.ToString() : "";
                    }
                }
                else
                {
                    Session["ID_THEM_MOI" + Sys_User.ID] = null;
                    getNoiDungByDotThu(localAPI.ConvertStringTolong(rcbDotThu.SelectedValue));
                }
                #endregion
            }
        }
        protected void btAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(rcbLop.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Bạn chưa chọn lớp học!');", true);
                return;
            }
            if (string.IsNullOrEmpty(rcbHocSinh.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Bạn chưa chọn học sinh!');", true);
                return;
            }
            if (string.IsNullOrEmpty(rcbDotThu.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Bạn chưa chọn đợt thu!');", true);
                return;
            }
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            if (Session["ID_THEM_MOI" + Sys_User.ID] == null)
            {
                HOC_PHI_PHIEU_THU_HOC_SINH detail = new HOC_PHI_PHIEU_THU_HOC_SINH();
                detail.ID_DOT_THU = Convert.ToInt64(rcbDotThu.SelectedValue);
                detail.ID_TRUONG = Sys_This_Truong.ID;
                detail.ID_NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
                detail.ID_KHOI = Convert.ToInt16(rcbKhoi.SelectedValue);
                detail.ID_LOP = Convert.ToInt64(rcbLop.SelectedValue);
                detail.ID_HOC_SINH = Convert.ToInt64(rcbHocSinh.SelectedValue);
                detail.GHI_CHU = tbGhiChu.Text.Trim();
                detail.IS_TIEN_AN = cboTienAn.Checked;
                if (cboTienAn.Checked)
                {
                    detail.IS_TIEN_AN = true;
                    if (!string.IsNullOrEmpty(tbTienAn.Text.Trim())) detail.SO_TIEN_AN = Convert.ToInt64(tbTienAn.Text.Trim());
                }
                if (!string.IsNullOrEmpty(tbTongTien.Text.Trim())) detail.TONG_TIEN = Convert.ToInt64(tbTongTien.Text.Trim());
                detail.GHI_CHU = !string.IsNullOrEmpty(tbGhiChu.Text.Trim()) ? tbGhiChu.Text.Trim() : null;
                #region check exists
                HOC_PHI_PHIEU_THU_HOC_SINH lstCheckExists = phieuThuHocSinhBO.checkExistsPhieuThu(Sys_This_Truong.ID, Convert.ToInt16(rcbKhoi.SelectedValue), Convert.ToInt16(rcbLop.SelectedValue), Convert.ToInt64(rcbDotThu.SelectedValue), Convert.ToInt64(rcbHocSinh.SelectedValue));
                if (lstCheckExists == null)
                {
                    res = phieuThuHocSinhBO.insert(detail, Sys_User.ID);
                    if (res.Res)
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('success', 'Cập nhật thành công!');", true);
                        Session["ID_THEM_MOI" + Sys_User.ID] = null;
                    }
                    else
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Không có bản ghi nào được lưu.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Đợt thu của học sinh đã tồn tại, vui lòng kiểm tra lại!');", true);
                    return;
                }
                #endregion
            }
            else
            {
                long id_phieu_thu_hs = Convert.ToInt64(Session["ID_THEM_MOI" + Sys_User.ID].ToString());
                HOC_PHI_PHIEU_THU_HOC_SINH detail = phieuThuHocSinhBO.getPhieuThuHocSinhByID(id_phieu_thu_hs);
                if (detail != null)
                {
                    detail.ID_DOT_THU = Convert.ToInt64(rcbDotThu.SelectedValue);
                    detail.ID_TRUONG = Sys_This_Truong.ID;
                    detail.ID_NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
                    detail.ID_KHOI = Convert.ToInt16(rcbKhoi.SelectedValue);
                    detail.ID_LOP = Convert.ToInt64(rcbLop.SelectedValue);
                    detail.ID_HOC_SINH = Convert.ToInt64(rcbHocSinh.SelectedValue);
                    detail.GHI_CHU = tbGhiChu.Text.Trim();
                    detail.IS_TIEN_AN = cboTienAn.Checked;
                    if (cboTienAn.Checked)
                    {
                        detail.IS_TIEN_AN = true;
                        if (!string.IsNullOrEmpty(tbTienAn.Text.Trim())) detail.SO_TIEN_AN = Convert.ToInt64(tbTienAn.Text.Trim());
                    }
                    if (!string.IsNullOrEmpty(tbTongTien.Text.Trim())) detail.TONG_TIEN = Convert.ToInt64(tbTongTien.Text.Trim());
                    detail.GHI_CHU = !string.IsNullOrEmpty(tbGhiChu.Text.Trim()) ? tbGhiChu.Text.Trim() : null;
                    res = phieuThuHocSinhBO.update(detail, Sys_User.ID);
                    if (res.Res)
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('success', 'Cập nhật thành công!');", true);
                    else
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Không có bản ghi nào được lưu.');", true);
                }

            }
        }
        protected void btEdit_Click(object sender, EventArgs e)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            if (Session["ID_THEM_MOI" + Sys_User.ID] != null)
            {
                HOC_PHI_PHIEU_THU_HOC_SINH detail = phieuThuHocSinhBO.getPhieuThuHocSinhByID(Convert.ToInt64(Session["ID_THEM_MOI" + Sys_User.ID]));
                if (detail != null)
                {
                    detail.ID_DOT_THU = Convert.ToInt64(rcbDotThu.SelectedValue);
                    detail.ID_TRUONG = Sys_This_Truong.ID;
                    detail.ID_NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
                    detail.ID_KHOI = Convert.ToInt16(rcbKhoi.SelectedValue);
                    detail.ID_LOP = Convert.ToInt64(rcbLop.SelectedValue);
                    detail.ID_HOC_SINH = Convert.ToInt64(rcbHocSinh.SelectedValue);
                    detail.GHI_CHU = tbGhiChu.Text.Trim();
                    detail.IS_TIEN_AN = cboTienAn.Checked;
                    if (cboTienAn.Checked)
                    {
                        detail.IS_TIEN_AN = true;
                        if (!string.IsNullOrEmpty(tbTienAn.Text.Trim())) detail.SO_TIEN_AN = Convert.ToInt64(tbTienAn.Text.Trim());
                    }
                    if (!string.IsNullOrEmpty(tbTongTien.Text.Trim())) detail.TONG_TIEN = Convert.ToInt64(tbTongTien.Text.Trim());
                    detail.GHI_CHU = !string.IsNullOrEmpty(tbGhiChu.Text.Trim()) ? tbGhiChu.Text.Trim() : null;
                    res = phieuThuHocSinhBO.update(detail, Sys_User.ID);
                }
            }
            if (res.Res)
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('success', 'Cập nhật thành công!');", true);
            else
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Không có bản ghi nào được cập nhật!');", true);
        }
        protected void rcbKhoi_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbLop.ClearSelection();
            rcbLop.Text = string.Empty;
            rcbLop.DataBind();
            rcbHocSinh.ClearSelection();
            rcbHocSinh.Text = string.Empty;
            rcbHocSinh.DataBind();
            rcbDotThu.ClearSelection();
            rcbDotThu.Text = string.Empty;
            rcbDotThu.DataBind();
        }
        protected void rcbLop_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbHocSinh.ClearSelection();
            rcbHocSinh.Text = string.Empty;
            rcbHocSinh.DataBind();
            rcbDotThu.ClearSelection();
            rcbDotThu.Text = string.Empty;
            rcbDotThu.DataBind();
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
        protected void rcbDotThu_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            getNoiDungByDotThu(localAPI.ConvertStringTolong(rcbDotThu.SelectedValue));
        }
        protected void getNoiDungByDotThu(long? id_dot_thu)
        {
            if (id_dot_thu != null)
            {
                HOC_PHI_DOT_THU detail = dotThuBO.getDotThuByID(id_dot_thu.Value);
                if (detail != null)
                {
                    tbGhiChu.Text = detail.GHI_CHU != null ? detail.GHI_CHU : "";
                    if (detail.IS_TIEN_AN != null && detail.IS_TIEN_AN == true)
                    {
                        cboTienAn.Checked = true;
                        tbTienAn.Text = detail.SO_TIEN_AN != null ? detail.SO_TIEN_AN.ToString() : "";
                    }
                    else
                    {
                        cboTienAn.Checked = false;
                        tbTienAn.Text = "";
                    }
                    tbTongTien.Text = detail.TONG_TIEN != null ? detail.TONG_TIEN.ToString() : "";
                }
            }
        }
    }
}