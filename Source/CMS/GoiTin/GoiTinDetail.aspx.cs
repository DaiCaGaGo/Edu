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

namespace CMS.GoiTin
{
    public partial class GoiTinDetail : AuthenticatePage
    {
        short? ma;
        GoiTinBO goiTinBO = new GoiTinBO();
        LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("ma") != null)
            {
                try
                {
                    ma = Convert.ToInt16(Request.QueryString.Get("ma"));
                }
                catch { }
            }
            if (!IsPostBack)
            {
                if (ma != null)
                {
                    GOI_TIN detail = new GOI_TIN();
                    detail = goiTinBO.getGoiTinByMa(ma.Value);
                    if (detail != null)
                    {
                        tbTen.Text = detail.TEN;
                        tbSoTinLienLac.Text = detail.SO_TIN_LIEN_LAC_HS == null ? "" : detail.SO_TIN_LIEN_LAC_HS.ToString();
                        tbSoTinThongBao.Text = detail.SO_TIN_THONG_BAO_HS == null ? "" : detail.SO_TIN_THONG_BAO_HS.ToString();
                        tbSoTinHe.Text = detail.SO_TIN_HE_HS == null ? "" : detail.SO_TIN_HE_HS.ToString();
                        tbThuTu.Text = detail.THU_TU == null ? "" : detail.THU_TU.ToString();
                        tbGhiChu.Text = detail.GHI_CHU == null ? "" : detail.GHI_CHU;
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
            long? max_thu_tu = goiTinBO.getMaxThuTu();
            if (max_thu_tu != null)
                tbThuTu.Text = Convert.ToString(Convert.ToInt16(max_thu_tu) + 1);
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
            GOI_TIN checkTen = goiTinBO.getGoiTinByTen(tbTen.Text.Trim());
            if (checkTen != null)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Tên gói tin này đã tồn tại. Vui lòng kiểm tra lại!');", true);
                tbTen.Focus();
                return;
            }
            #endregion
            GOI_TIN detail = new GOI_TIN();
            detail.TEN = tbTen.Text.Trim();
            detail.GHI_CHU = tbGhiChu.Text.Trim();
            detail.SO_TIN_HE_HS = localAPI.ConvertStringTolong(tbSoTinHe.Text.Trim());
            detail.SO_TIN_LIEN_LAC_HS = localAPI.ConvertStringTolong(tbSoTinLienLac.Text.Trim());
            detail.SO_TIN_THONG_BAO_HS = localAPI.ConvertStringTolong(tbSoTinThongBao.Text.Trim());
            detail.THU_TU = localAPI.ConvertStringToShort(tbThuTu.Text.Trim());
            ResultEntity res = goiTinBO.insert(detail, Sys_User.ID);
            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
                tbTen.Text = "";
                tbTen.Focus();
                tbGhiChu.Text = "";
                tbSoTinHe.Text = "";
                tbSoTinLienLac.Text = "";
                tbSoTinThongBao.Text = "";
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
            GOI_TIN checkTen = goiTinBO.getGoiTinByTen(tbTen.Text.Trim());
            if (checkTen != null && checkTen.MA != ma)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Tên gói tin này đã tồn tại. Vui lòng kiểm tra lại!');", true);
                tbTen.Focus();
                return;
            }
            #endregion
            GOI_TIN detail = new GOI_TIN();
            detail = goiTinBO.getGoiTinByMa(ma.Value);
            if (detail == null) detail = new GOI_TIN();
            detail.TEN = tbTen.Text.Trim();
            if (tbGhiChu.Text.Trim() != "") detail.GHI_CHU = tbGhiChu.Text.Trim();
            detail.SO_TIN_HE_HS = localAPI.ConvertStringTolong(tbSoTinHe.Text.Trim());
            detail.SO_TIN_LIEN_LAC_HS = localAPI.ConvertStringTolong(tbSoTinLienLac.Text.Trim());
            detail.SO_TIN_THONG_BAO_HS = localAPI.ConvertStringTolong(tbSoTinThongBao.Text.Trim());
            detail.THU_TU = localAPI.ConvertStringToShort(tbThuTu.Text.Trim());
            ResultEntity res = goiTinBO.update(detail, Sys_User.ID);
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