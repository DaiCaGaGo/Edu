using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS
{
    public partial class AccessDenied : System.Web.UI.Page
    {
        public SYS_Profile sys_profile = new SYS_Profile();
        protected void Page_Load(object sender, EventArgs e)
        {
            lbThongBao.Text = sys_profile.getThongBao();
        }
    }
}