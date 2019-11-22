using CMS.XuLy;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS
{
    public partial class Login : System.Web.UI.Page
    {
        protected string googleplus_client_id = "724611141278-ko3c5ijpo3k75te78ilv40p5k4cuc3lk.apps.googleusercontent.com";    // Replace this with your Client ID
        protected string googleplus_client_secret = "Xfr-TFedyyeMr44Ju2hkhk94"; 
        protected string googleplus_redirect_url = "http://localhost:51793/loginEmail.aspx"; // Replace this with your Redirect URL; Your Redirect URL from your developer.google application should match this URL.
        public LoginHelper LoginHelper = new LoginHelper();
        public LocalAPI LocalAPI = new LocalAPI();
        SYS_Cookies SYS_Cookies = new SYS_Cookies();

        private string returnUrl = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            returnUrl = Request.QueryString["returnUrl"];
        }

        protected void btDangNhap_Click(object sender, EventArgs e)
        {
            try
            {
                string strError="";
                NGUOI_DUNG userLogged=new NGUOI_DUNG();
                if (LoginHelper.IsLoginSuccessSubmit(tbUserName.Text.Trim(), tbMatKhau.Text.Trim(), out strError, out userLogged))
                {
                    LoginHelper.SetLoginSuccess(userLogged);
                    if (string.IsNullOrEmpty(returnUrl))
                        Response.Redirect("~/Default.aspx", false);
                    else
                        Response.Redirect(returnUrl);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Tài khoản đăng nhập hoặc mật khẩu chưa đúng!');", true);
                    tbUserName.Focus();
                }
            }
            catch (Exception ex)
            {
            }
        }
        protected void btGoogle_Click(object sender, EventArgs e)
        {
            var Googleurl = "https://accounts.google.com/o/oauth2/auth?response_type=code&redirect_uri=" + googleplus_redirect_url + "&scope=https://www.googleapis.com/auth/userinfo.email%20https://www.googleapis.com/auth/userinfo.profile&client_id=" + googleplus_client_id;
            Session["loginWith"] = "google";
            Response.Redirect(Googleurl);
        }
        protected void btFacebook_Click(object sender, EventArgs e)
        {

        }
    }
}