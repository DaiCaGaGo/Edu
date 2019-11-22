using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using System.Security.Principal;
using System.Web.Script.Serialization;
using OneEduDataAccess.Model;
using OneEduDataAccess;

namespace CMS
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            //GlobalConfiguration.Configure(WebApiConfig.Register);
        }
        private void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            
           // DevExpress.XtraReports.Web.ReportDesigner.DefaultReportDesignerContainer.RegisterDataSourceWizardConfigFileConnectionStringsProvider();

            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                // Get the forms authentication ticket.
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                var identity = new GenericIdentity(authTicket.Name, "Forms");
                var principal = new MyPrincipal(identity);

                // Get the custom user data encrypted in the ticket.
                string userData = ((FormsIdentity)(Context.User.Identity)).Ticket.UserData;

                // Deserialize the json data and set it on the custom principal.
                var serializer = new JavaScriptSerializer();
                principal.User = serializer.Deserialize<NGUOI_DUNGEntity>(userData);

                // Set the context user.
                Context.User = principal;
            }
        }
    }
}