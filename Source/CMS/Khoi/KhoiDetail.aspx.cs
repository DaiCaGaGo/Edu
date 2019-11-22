using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;

namespace CMS.Khoi
{
    public partial class KhoiDetail : AuthenticatePage
    {
        short? ma_khoi;
        KhoiBO khoiBO = new KhoiBO();
        LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("id_hoso") != null)
            {
                try
                {
                    ma_khoi = Convert.ToInt16(Request.QueryString.Get("id_hoso"));
                }
                catch { }
            }
            if (!IsPostBack)
            {
                if (ma_khoi != null)
                {
                    KHOI detail = new KHOI();
                    detail = khoiBO.getKhoiByMa(ma_khoi.Value);
                    if (detail != null)
                    {
                        tbMa.Text = detail.MA.ToString();
                        tbTen.Text = detail.TEN;
                        if (detail.THU_TU != null)
                            tbThuTu.Text = detail.THU_TU.ToString();
                        cbIsMN.Checked = detail.IS_MN == null ? false : detail.IS_MN.Value;
                        cbIsTH.Checked = detail.IS_TH == null ? false : detail.IS_TH.Value;
                        cbIsTHCS.Checked = detail.IS_THCS == null ? false : detail.IS_THCS.Value;
                        cbIsTHPT.Checked = detail.IS_THPT == null ? false : detail.IS_THPT.Value;
                        cbIsGDTX.Checked = detail.IS_GDTX == null ? false : detail.IS_GDTX.Value;
                        if (cbIsGDTX.Checked)
                        {
                            rcbLoaiLopGDTX.Enabled = true;
                            rcbLoaiLopGDTX.DataBind();
                            if (detail.MA_LOAI_LOP_GDTX != null && rcbLoaiLopGDTX.Items.FindItemByValue(detail.MA_LOAI_LOP_GDTX.Value.ToString()) != null)
                            {
                                rcbLoaiLopGDTX.SelectedValue = detail.MA_LOAI_LOP_GDTX.Value.ToString();
                            }
                            else
                            {
                                rcbLoaiLopGDTX.ClearSelection();
                                rcbLoaiLopGDTX.Text = string.Empty;
                            }
                        }
                        else
                        {
                            rcbLoaiLopGDTX.Enabled = false;
                            rcbLoaiLopGDTX.ClearSelection();
                            rcbLoaiLopGDTX.Text = string.Empty;
                        }
                        btEdit.Visible = is_access(SYS_Type_Access.SUA);
                        btAdd.Visible = false;
                    }
                    else
                    {
                        rcbLoaiLopGDTX.Enabled = false;
                        rcbLoaiLopGDTX.ClearSelection();
                        rcbLoaiLopGDTX.Text = string.Empty;
                        btEdit.Visible = false;
                        btAdd.Visible = is_access(SYS_Type_Access.THEM);
                    }
                }
                else
                {
                    rcbLoaiLopGDTX.Enabled = false;
                    rcbLoaiLopGDTX.ClearSelection();
                    rcbLoaiLopGDTX.Text = string.Empty;
                    btEdit.Visible = false;
                    btAdd.Visible = is_access(SYS_Type_Access.THEM);
                }
            }
        }

        protected void btAdd_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.THEM))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            KHOI detail = new KHOI();
            detail.MA = localAPI.ConvertStringToShort(tbMa.Text.Trim()).Value;
            detail.TEN = tbTen.Text;
            detail.THU_TU = localAPI.ConvertStringToint(tbThuTu.Text.Trim());
            detail.IS_MN = cbIsMN.Checked;
            detail.IS_TH = cbIsTH.Checked;
            detail.IS_THCS = cbIsTHCS.Checked;
            detail.IS_THPT = cbIsTHPT.Checked;
            detail.IS_GDTX = cbIsGDTX.Checked;
            detail.MA_LOAI_LOP_GDTX = localAPI.ConvertStringToShort(rcbLoaiLopGDTX.SelectedValue);
            ResultEntity res = khoiBO.insert(detail.MA, detail.TEN, detail.MA_LOAI_LOP_GDTX, detail.THU_TU, detail.IS_MN.Value, detail.IS_TH.Value, detail.IS_THCS.Value, detail.IS_THPT.Value, detail.IS_GDTX.Value, Sys_User.ID);
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

        protected void btEdit_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            KHOI detail = new KHOI();
            detail.MA = localAPI.ConvertStringToShort(tbMa.Text.Trim()).Value;
            detail = khoiBO.getKhoiByMa(detail.MA);
            if (detail == null) detail = new KHOI();
            detail.MA = localAPI.ConvertStringToShort(tbMa.Text.Trim()).Value;
            detail.TEN = tbTen.Text;
            detail.THU_TU = localAPI.ConvertStringToint(tbThuTu.Text.Trim());
            detail.IS_MN = cbIsMN.Checked;
            detail.IS_TH = cbIsTH.Checked;
            detail.IS_THCS = cbIsTHCS.Checked;
            detail.IS_THPT = cbIsTHPT.Checked;
            detail.IS_GDTX = cbIsGDTX.Checked;
            detail.MA_LOAI_LOP_GDTX = localAPI.ConvertStringToShort(rcbLoaiLopGDTX.SelectedValue);
            ResultEntity res = khoiBO.update(detail.MA, detail.TEN, detail.MA_LOAI_LOP_GDTX, detail.THU_TU, detail.IS_MN.Value, detail.IS_TH.Value, detail.IS_THCS.Value, detail.IS_THPT.Value, detail.IS_GDTX.Value, Sys_User.ID);
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

        protected void cbIsGDTX_CheckedChanged(object sender, EventArgs e)
        {
            rcbLoaiLopGDTX.Enabled = cbIsGDTX.Checked;
            rcbLoaiLopGDTX.DataBind();
            rcbLoaiLopGDTX.ClearSelection();
            rcbLoaiLopGDTX.Text = string.Empty;
        }
    }
}