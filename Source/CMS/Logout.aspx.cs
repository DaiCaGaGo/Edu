using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS
{
    public partial class Logout : System.Web.UI.Page
    {
        public LoginHelper LoginHelper = new LoginHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            LoginHelper.SignOut();
            Session.Abandon();
            Response.Cookies.Clear();
            var sys_profile = new SYS_Profile();
            sys_profile.Clear_Profile();
            Response.Redirect("~/Login.aspx");
        }
    }
}