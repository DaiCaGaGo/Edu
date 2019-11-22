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

namespace CMS.DinhDuong
{
    public partial class ThucPhamDetail : AuthenticatePage
    {
        long? id;
        ThucPhamBO thucPhamBO = new ThucPhamBO();
        LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("id") != null)
            {
                try
                {
                    id = Convert.ToInt64(Request.QueryString.Get("id"));
                }
                catch { }
            }
            if (!IsPostBack)
            {
                if (id != null)
                {
                    DM_THUC_PHAM detail = new DM_THUC_PHAM();
                    detail = thucPhamBO.getThucPhamByID(id.Value);
                    if (detail != null)
                    {
                        rcbNhomThucPham.SelectedValue = detail.ID_NHOM_THUC_PHAM.ToString();
                        tbTen.Text = detail.TEN;
                        tbTen_en.Text = detail.TEN_EN != null ? detail.TEN_EN : "";
                        if (detail.THU_TU != null) tbThuTu.Text = detail.THU_TU.ToString();
                        rcbDonViTinh.SelectedValue = detail.DON_VI_TINH != null ? detail.DON_VI_TINH.ToString() : "";
                        tbThaiBo.Text = detail.PHAN_TRAM_THAI_BO != null ? detail.PHAN_TRAM_THAI_BO.ToString() : "";
                        tbNangLuong.Text = detail.NANG_LUONG_KCAL != null ? detail.NANG_LUONG_KCAL.ToString() : "";
                        tbProtid.Text = detail.PROTID != null ? detail.PROTID.ToString() : "";
                        tbGlucid.Text = detail.GLUCID != null ? detail.GLUCID.ToString() : "";
                        tbLipid.Text = detail.LIPID != null ? detail.LIPID.ToString() : "";

                        tbProtid_weigh.Text = detail.PROTID_WEIGH == null ? "" : detail.PROTID_WEIGH.ToString();
                        tbGlucid_weigh.Text = detail.GLUCID_WEIGH == null ? "" : detail.GLUCID_WEIGH.ToString();
                        tbLipid_weigh.Text = detail.LIPID_WEIGH == null ? "" : detail.LIPID_WEIGH.ToString();

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
                    btAdd.Visible = is_access(SYS_Type_Access.SUA);
                    getMaxThuTu();
                }
            }
        }
        protected void getMaxThuTu()
        {
            long? max_thu_tu = thucPhamBO.getMaxThuTu();
            if (max_thu_tu != null)
                tbThuTu.Text = Convert.ToString(Convert.ToInt16(max_thu_tu) + 1);
            else tbThuTu.Text = "1";
        }
        protected void btAdd_Click(object sender, EventArgs e)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            #region "check du lieu"
            if (!is_access(SYS_Type_Access.THEM))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            if (rcbNhomThucPham.SelectedValue=="")
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn chưa chọn nhóm thực phẩm, vui lòng kiểm tra lại!');", true);
                return;
            }
            #endregion
            DM_THUC_PHAM checkExist = thucPhamBO.checkExistThucPham(Convert.ToInt16(rcbNhomThucPham.SelectedValue), tbTen.Text.Trim());
            if (checkExist == null)
            {
                DM_THUC_PHAM detail = new DM_THUC_PHAM();
                detail.ID_NHOM_THUC_PHAM = Convert.ToInt16(rcbNhomThucPham.SelectedValue);
                detail.TEN = tbTen.Text.Trim();
                detail.TEN_EN = tbTen_en.Text.Trim();
                detail.DON_VI_TINH = localAPI.ConvertStringToShort(rcbDonViTinh.SelectedValue);
                detail.PHAN_TRAM_THAI_BO = localAPI.ConvertStringToDecimal(tbThaiBo.Text.Trim());
                detail.NANG_LUONG_KCAL = localAPI.ConvertStringToDecimal(tbNangLuong.Text.Trim());
                detail.PROTID = localAPI.ConvertStringToDecimal(tbProtid.Text.Trim());
                detail.GLUCID = localAPI.ConvertStringToDecimal(tbGlucid.Text.Trim());
                detail.LIPID = localAPI.ConvertStringToDecimal(tbLipid.Text.Trim());
                detail.THU_TU = localAPI.ConvertStringToShort(tbThuTu.Text.Trim());
                detail.PROTID_WEIGH = localAPI.ConvertStringToDecimal(tbProtid_weigh.Text.Trim());
                detail.GLUCID_WEIGH = localAPI.ConvertStringToDecimal(tbGlucid_weigh.Text.Trim());
                detail.LIPID_WEIGH = localAPI.ConvertStringToDecimal(tbLipid_weigh.Text.Trim());

                res = thucPhamBO.insert(detail, Sys_User.ID);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Tên thực phẩm này đã tồn tại. Vui lòng kiểm tra lại!');", true);
                tbTen.Focus();
                return;
            }
            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
                tbTen.Text = "";
                tbTen.Focus();
                tbTen_en.Text = "";
                tbThaiBo.Text = "";
                tbNangLuong.Text = "";
                tbProtid.Text = "";
                tbGlucid.Text = "";
                tbLipid.Text = "";
                tbProtid_weigh.Text = "";
                tbLipid_weigh.Text = "";
                tbGlucid_weigh.Text = "";
                getMaxThuTu();
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
            if (string.IsNullOrEmpty(rcbNhomThucPham.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn chưa chọn nhóm thực phẩm, vui lòng kiểm tra lại!');", true);
                return;
            }
            #endregion
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            DM_THUC_PHAM detail = new DM_THUC_PHAM();
            detail = thucPhamBO.getThucPhamByID(id.Value);
            DM_THUC_PHAM checkExist = thucPhamBO.checkExistThucPham(Convert.ToInt16(rcbNhomThucPham.SelectedValue), tbTen.Text.Trim());
            if (checkExist != null && checkExist.ID != id.Value)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Tên thực phẩm này đã tồn tại. Vui lòng kiểm tra lại!');", true);
                tbTen.Focus();
                return;
            }
            else
            {
                if (detail != null)
                {
                    detail.ID_NHOM_THUC_PHAM = Convert.ToInt16(rcbNhomThucPham.SelectedValue);
                    detail.TEN = tbTen.Text.Trim();
                    detail.TEN_EN = tbTen_en.Text.Trim();
                    detail.DON_VI_TINH = localAPI.ConvertStringToShort(rcbDonViTinh.SelectedValue);
                    detail.PHAN_TRAM_THAI_BO = localAPI.ConvertStringToDecimal(tbThaiBo.Text.Trim());
                    detail.NANG_LUONG_KCAL = localAPI.ConvertStringToDecimal(tbNangLuong.Text.Trim());
                    detail.THU_TU = localAPI.ConvertStringToShort(tbThuTu.Text.Trim());

                    if (tbProtid.Text.Trim() != "" && tbProtid.Text.Trim() != "0")
                        detail.PROTID = Convert.ToDecimal(tbProtid.Text.Trim());
                    else detail.PROTID = null;
                    if (tbLipid.Text.Trim() != "" && tbLipid.Text.Trim() != "0")
                        detail.LIPID = Convert.ToDecimal(tbLipid.Text.Trim());
                    else detail.LIPID = null;
                    if (tbGlucid.Text.Trim() != "" && tbGlucid.Text.Trim() != "0")
                        detail.GLUCID = Convert.ToDecimal(tbGlucid.Text.Trim());
                    else detail.GLUCID = null;

                    if (tbProtid_weigh.Text.Trim() != "" && tbProtid_weigh.Text.Trim() != "0")
                        detail.PROTID_WEIGH = Convert.ToDecimal(tbProtid_weigh.Text.Trim());
                    else detail.PROTID_WEIGH = null;
                    if (tbLipid_weigh.Text.Trim() != "" && tbLipid_weigh.Text.Trim() != "0")
                        detail.LIPID_WEIGH = Convert.ToDecimal(tbLipid_weigh.Text.Trim());
                    else detail.LIPID_WEIGH = null;
                    if (tbGlucid_weigh.Text.Trim() != "" && tbGlucid_weigh.Text.Trim() != "0")
                        detail.GLUCID_WEIGH = Convert.ToDecimal(tbGlucid_weigh.Text.Trim());
                    else detail.GLUCID_WEIGH = null;
                    res = thucPhamBO.update(detail, Sys_User.ID);
                }
            }
            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
    }
}