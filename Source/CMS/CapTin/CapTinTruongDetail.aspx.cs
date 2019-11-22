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

namespace CMS.CapTin
{
    public partial class CapTinTruongDetail : AuthenticatePage
    {
        long? id;
        CapTinTruongBO capTinTruongBO = new CapTinTruongBO();
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
                objTruong.SelectParameters.Add("id_doi_tac", Sys_This_Truong.ID_DOI_TAC.ToString());
                rcbTruong.DataBind();
                rcbTruong.SelectedValue = Sys_This_Truong.ID.ToString();
                rcbTruong.Enabled = false;
                if (id != null)
                {
                    CAP_TIN_TRUONG detail = new CAP_TIN_TRUONG();
                    detail = capTinTruongBO.getCapTinTruongByID(id.Value);
                    if (detail != null)
                    {
                        rcbTruong.SelectedValue = detail.ID_TRUONG.ToString();
                        tbSoTinCap.Text = detail.SO_TIN_CAP.ToString();
                        btAdd.Visible = false;
                    }
                    else
                    {
                        btAdd.Visible = is_access(SYS_Type_Access.THEM);
                    }
                }
                else
                {
                    btAdd.Visible = is_access(SYS_Type_Access.THEM);
                }
            }
        }
        protected void btAdd_Click(object sender, EventArgs e)
        {
            #region "check du lieu"
            if (!is_access(SYS_Type_Access.THEM))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            #endregion
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            int success = 0;
            CAP_TIN_TRUONG detail = new CAP_TIN_TRUONG();
            detail.ID_DOI_TAC = Sys_This_Truong.ID_DOI_TAC.Value;
            detail.ID_TRUONG = Convert.ToInt64(rcbTruong.SelectedValue);
            detail.SO_TIN_CAP = Convert.ToInt64(tbSoTinCap.Text);
            res = capTinTruongBO.insert(detail, Sys_User.ID);
            if (res.Res)
            {
                success++;
                logUserBO.insert(Sys_This_Truong.ID, "CẤP TIN", "Đối tác " + detail.ID_DOI_TAC+ " cấp tin", Sys_User.ID, DateTime.Now);
            }
            string strMsg = "";
            if (success > 0)
            {
                strMsg = "notification('success', 'Cấp tin thành công!');";
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
    }
}