using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS.TraCuu
{
    public partial class HuongDanZalo : System.Web.UI.Page
    {
        public string user_type;
        public string fileLocation = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("user_type") != null)
            {
                try
                {
                    user_type = Convert.ToString(Request.QueryString.Get("user_type"));
                }
                catch (Exception ex) { }
            }
            if (user_type == "GV")
            {

            }
            else if (user_type == "PH")
            {
                fileLocation = "http://" + HttpContext.Current.Request.Url.Authority + "/Tmps/HuongDanDangKyZalo.pdf";
            }
        }
    }
}