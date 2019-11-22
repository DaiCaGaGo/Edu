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

namespace CMS
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {

        public LoginHelper LoginHelper = new LoginHelper();
        public LocalAPI localAPI = new LocalAPI();
        public NGUOI_DUNGEntity Sys_User;
        public SYS_Profile SYS_Profile = new SYS_Profile();
        public TRUONG Sys_This_Truong;
        public int Sys_Ma_Nam_hoc;
        public int Sys_Hoc_Ky;
        public string Sys_Ten_Nam_Hoc;
        public string Sys_This_Cap_Hoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            string path = Request.AppRelativeCurrentExecutionFilePath.Split('?')[0];
            if (!LoginHelper.IsLoginSuccess)
            {
                LoginHelper.SignOut();
                SetLoginRequired(path);
                return;
            }
            else
            {
                Sys_User = LoginHelper.GetUserLogged;
                if (Sys_User == null || (Sys_User.IS_ROOT != true && string.IsNullOrEmpty(Sys_User.TEN_DANG_NHAP)))
                {
                    LoginHelper.SignOut();
                    SetLoginRequired(path);
                    return;
                }
                Sys_Ma_Nam_hoc = SYS_Profile.getMaNamHoc();
                Sys_Ten_Nam_Hoc = SYS_Profile.getTenNamHoc();
                Sys_Hoc_Ky = SYS_Profile.getHocKy();
                
                #region get truong đang xử lý
                Sys_This_Truong = new TRUONG();
                Sys_This_Truong = SYS_Profile.getThisTruong();
                #endregion
                #region get cấp đang xử lý
                Sys_This_Cap_Hoc = SYS_Profile.getCapHoc();
                #endregion
               
            }
        }

        private void SetLoginRequired(string path)
        {
            Response.Redirect("~/Login.aspx?returnUrl=" + path + Request.Url.Query);
        }
    }
}