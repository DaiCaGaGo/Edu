using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Net.Http;
using System.IO;
using System.Text;
using OneEduDataAccess.Model;

namespace CMS
{
    public partial class LoginEmail : AuthenticatePage
    {
        protected string googleplus_client_id = "724611141278-ko3c5ijpo3k75te78ilv40p5k4cuc3lk.apps.googleusercontent.com";    // Replace this with your Client ID
        protected string googleplus_client_secret = "Xfr-TFedyyeMr44Ju2hkhk94";                                                // Replace this with your Client Secret
        protected string googleplus_redirect_url = "http://localhost:51793/loginEmail.aspx"; // Replace this with your Redirect URL; Your Redirect URL from your developer.google application should match this URL.
        protected string Parameters;
        private string returnUrl = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            returnUrl = Request.QueryString["returnUrl"];

            if ((Session.Contents.Count > 0) && (Session["loginWith"] != null) && (Session["loginWith"].ToString() == "google"))
            {
                try
                {
                    var url = Request.Url.Query;
                    if (url != "")
                    {
                        string queryString = url.ToString();
                        char[] delimiterChars = { '=' };
                        string[] words = queryString.Split(delimiterChars);
                        string code = words[1];

                        if (code != null)
                        {
                            //get the access token 
                            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://accounts.google.com/o/oauth2/token");
                            webRequest.Method = "POST";
                            Parameters = "code=" + code + "&client_id=" + googleplus_client_id + "&client_secret=" + googleplus_client_secret + "&redirect_uri=" + googleplus_redirect_url + "&grant_type=authorization_code";
                            byte[] byteArray = Encoding.UTF8.GetBytes(Parameters);
                            webRequest.ContentType = "application/x-www-form-urlencoded";
                            webRequest.ContentLength = byteArray.Length;
                            Stream postStream = webRequest.GetRequestStream();
                            // Add the post data to the web request
                            postStream.Write(byteArray, 0, byteArray.Length);
                            postStream.Close();

                            WebResponse response = webRequest.GetResponse();
                            postStream = response.GetResponseStream();
                            StreamReader reader = new StreamReader(postStream);
                            string responseFromServer = reader.ReadToEnd();

                            GooglePlusAccessToken serStatus = JsonConvert.DeserializeObject<GooglePlusAccessToken>(responseFromServer);

                            if (serStatus != null)
                            {
                                string accessToken = string.Empty;
                                accessToken = serStatus.access_token;

                                if (!string.IsNullOrEmpty(accessToken))
                                {
                                    // This is where you want to add the code if login is successful.
                                    getgoogleplususerdataSer(accessToken);
                                }
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    //throw new Exception(ex.Message, ex);
                    Response.Redirect("default.aspx");
                }
            }
        }

        public class GooglePlusAccessToken
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
            public string id_token { get; set; }
            public string refresh_token { get; set; }
        }

        private async void getgoogleplususerdataSer(string access_token)
        {
            try
            {
                HttpClient client = new HttpClient();
                var urlProfile = "https://www.googleapis.com/oauth2/v1/userinfo?access_token=" + access_token;

                client.CancelPendingRequests();
                HttpResponseMessage output = await client.GetAsync(urlProfile);

                if (output.IsSuccessStatusCode)
                {
                    string outputData = await output.Content.ReadAsStringAsync();
                    GoogleUserOutputData serStatus = JsonConvert.DeserializeObject<GoogleUserOutputData>(outputData);

                    if (serStatus != null)
                    {
                        string strError;
                        NGUOI_DUNG userLogged;
                        if (LoginHelper.IsLoginSuccessSubmitEmail(serStatus.email, out strError, out userLogged))
                        {
                            LoginHelper.SetLoginSuccess(userLogged);
                            if (string.IsNullOrEmpty(returnUrl))
                                Response.Redirect("~/Default.aspx", false);
                            else
                                Response.Redirect(returnUrl);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Tài khoản đăng nhập chưa đúng!');", true);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //catching the exception
            }
        }

        public class GoogleUserOutputData
        {
            public string id { get; set; }
            public string name { get; set; }
            public string given_name { get; set; }
            public string email { get; set; }
            public string picture { get; set; }
        }
    }
}