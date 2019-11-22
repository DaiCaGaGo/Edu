using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace CMS
{
    public class LoginHelper
    {
        public bool IsLoginSuccess
        {
            get { return HttpContext.Current.User.Identity.IsAuthenticated; }
        }
        public NGUOI_DUNGEntity GetUserLogged
        {
            get
            {
                if (HttpContext.Current.User != null)
                {
                    if (HttpContext.Current.User.GetType() == typeof(MyPrincipal))
                    {
                        var pricipal = (MyPrincipal)HttpContext.Current.User;
                        if (pricipal != null)
                            return pricipal.User;
                    }
                }
                return null;
            }
        }
        public decimal idNguoiDung
        {
            get
            {
                if (GetUserLogged != null)
                {
                    return GetUserLogged.ID;
                }
                return 0;
            }
        }
        public bool IsLoginSuccessSubmit(string userName, string password, out string error,
            out NGUOI_DUNG userLogged)
        {
            error = string.Empty;
            userLogged = null;
            //require username
            if (string.IsNullOrEmpty(userName))
            {
                error = "Yêu cầu nhập tên đăng nhập";
                return false;
            }
            //require password
            if (string.IsNullOrEmpty(password))
            {
                error = "Yêu cầu nhập mật khẩu";
                return false;
            }
            //Get user
            NguoiDungBO ndBO = new NguoiDungBO();
            userLogged = ndBO.getLogin(userName, password);
            if (userLogged == null)
            {
                error = "Tài khoản đăng nhập không đúng";
            }
            return userLogged != null;
        }
        public bool IsLoginSuccessSubmitEmail(string email, out string error,
           out NGUOI_DUNG userLogged)
        {
            error = string.Empty;
            userLogged = null;
            //require username
            if (string.IsNullOrEmpty(email))
            {
                error = "Yêu cầu nhập email";
                return false;
            }
            //Get user
            NguoiDungBO ndBO = new NguoiDungBO();
            userLogged = ndBO.getLoginEmail(email);
            if (userLogged == null)
            {
                error = "Tài khoản đăng nhập không đúng";
                userLogged = null;
            }
            return userLogged != null;
        }
        public void SetLoginSuccess(NGUOI_DUNG userItem)
        {
            SetRememberMe(userItem, 2);
        }
        public void SignOut()
        {
            // Delete the user details from cache.
            HttpContext.Current.Session.Abandon();
            FormsAuthentication.SignOut();
            HttpCookie currentUserCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            HttpContext.Current.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
            currentUserCookie.Expires = DateTime.Now.AddYears(-10);
            currentUserCookie.Value = null;
            HttpContext.Current.Response.SetCookie(currentUserCookie);
            // Clear authentication cookie.
            if (HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                HttpContext.Current.Response.Cookies[FormsAuthentication.FormsCookieName].Expires = DateTime.Now.AddYears(-1);
            }
        }
        private void SetRememberMe(NGUOI_DUNG userItem, int numberDay)
        {
            // Create the authentication ticket with custom user data.
            var serializer = new JavaScriptSerializer();
            string userData = serializer.Serialize(GetNguoi_Dung_No_Proxy(userItem));

            //set login success
            FormsAuthentication.SetAuthCookie(userItem.TEN_DANG_NHAP, true);

            //clear any other tickets that are already in the response
            HttpContext.Current.Response.Cookies.Clear();
            //set the new expiry date - to one day from now
            DateTime expiryDate = DateTime.Now.AddDays(numberDay);
            //create a new forms auth ticket
            var ticket = new FormsAuthenticationTicket(2, userItem.TEN_DANG_NHAP,
                DateTime.Now, expiryDate, true, userData, FormsAuthentication.FormsCookiePath);
            //encrypt the ticket
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            //create a new authentication cookie - and set its expiration date
            var authenticationCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            authenticationCookie.Expires = ticket.Expiration;
            //add the cookie to the response.
            HttpContext.Current.Response.Cookies.Add(authenticationCookie);
        }
        private NGUOI_DUNGEntity GetNguoi_Dung_No_Proxy(NGUOI_DUNG nd)
        {
            return new NGUOI_DUNGEntity
            {
                ID = nd.ID,
                TEN_DANG_NHAP = nd.TEN_DANG_NHAP,
                FACE_BOOK = nd.FACE_BOOK,
                MAT_KHAU = nd.MAT_KHAU,
                TEN_HIEN_THI = nd.TEN_HIEN_THI,
                EMAIL = nd.EMAIL,
                SDT = nd.SDT,
                DIA_CHI = nd.DIA_CHI,
                ANH_DAI_DIEN = nd.ANH_DAI_DIEN,
                GHI_CHU = nd.GHI_CHU,
                TRANG_THAI = nd.TRANG_THAI,
                NGAY_TAO = nd.NGAY_TAO,
                NGUOI_TAO = nd.NGUOI_TAO,
                NGAY_SUA = nd.NGAY_SUA,
                NGUOI_SUA = nd.NGUOI_SUA,
                IS_ROOT = nd.IS_ROOT,
                MA_NGON_NGU = nd.MA_NGON_NGU,
                IS_DELETE = nd.IS_DELETE,
                IS_LOGIN_GMAIL = nd.IS_LOGIN_GMAIL,
                IS_LOGIN_FACEBOOK = nd.IS_LOGIN_FACEBOOK,
                ID_DOI_TAC = nd.ID_DOI_TAC
            };
        }
        public byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA1.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }
        public string GetHashString(string inputString)
        {
            var sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));
            return sb.ToString();
        }
    }
}