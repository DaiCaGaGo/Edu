using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS.CauHinh
{
    public partial class NamHoc_HocKy : AuthenticatePage
    {
        SYS_Profile sys_Profile = new SYS_Profile();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rcbHocKy.DataBind();
                rcbHocKy.SelectedValue = Sys_Hoc_Ky.ToString();
                rcbNamHoc.DataBind();
                rcbNamHoc.SelectedValue = Sys_Ma_Nam_hoc.ToString();
            }
        }
        protected void btEdit_Click(object sender, EventArgs e)
        {
            sys_Profile.setNamHoc(Convert.ToInt32(rcbNamHoc.SelectedValue));
            sys_Profile.setHocKy(Convert.ToInt32(rcbHocKy.SelectedValue));
            string returnUrl = Request.QueryString["returnUrl"];
            if (string.IsNullOrEmpty(returnUrl))
                Response.Redirect("~/Default.aspx", false);
            else
                Response.Redirect(returnUrl);
            string strMsg = "notification('success', 'Thiết lập thành công');";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
    }
}