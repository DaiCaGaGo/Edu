using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace CMS
{
    public class AuthenticatePage : Page
    {
        public NGUOI_DUNGEntity Sys_User;
        public NGUOI_DUNG_MENU Sys_User_Menu;
        public MENU Sys_This_Menu;
        public TRUONG Sys_This_Truong;
        public int Sys_Ma_Nam_hoc;
        public int Sys_Hoc_Ky;
        public string Sys_Ten_Nam_Hoc;
        public string Sys_This_Cap_Hoc;
        public short? Sys_This_Lop_GDTX;
        public short Sys_Time_Send = 30;
        public List<TRUONG> Sys_List_Truong;
        public LoginHelper LoginHelper = new LoginHelper();
        public SYS_Profile SYS_Profile = new SYS_Profile();
        public List<string> lstLinkNotCheck = new List<string>() { "default.aspx", "manage/manage.aspx", "cauhinh/chontruong_cap.aspx", "cauhinh/namhoc_hocky.aspx", "logout.aspx", "datamanxhn.aspx", "datamanx.aspx", "profile/changepassword.aspx" };
        protected override void OnInit(EventArgs e)
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
                #region get list truong access
                TruongBO truongBO = new TruongBO();
                Sys_List_Truong = new List<TRUONG>();
                Sys_List_Truong = truongBO.getTruongAccessByNguoiDung(Sys_User);
                if (Sys_User.IS_ROOT != true && (Sys_List_Truong == null || Sys_List_Truong.Count == 0))
                {
                    SetAccessDenied("Bạn chưa được phân quyền đơn vị nào");
                    return;
                }
                #endregion
                #region get truong đang xử lý
                Sys_This_Truong = new TRUONG();
                Sys_This_Truong = SYS_Profile.getThisTruong();
                if (path.Replace("~/CauHinh/", "").ToLower() != "chontruong_cap.aspx" && Sys_User.IS_ROOT != true && Sys_This_Truong == null && Sys_List_Truong != null && Sys_List_Truong.Count > 0)
                {
                    SetChonTruongRequired(path);
                    return;
                }
                #endregion
                #region get cấp đang xử lý
                Sys_This_Cap_Hoc = SYS_Profile.getCapHoc();
                if (path.Replace("~/CauHinh/", "").ToLower() != "chontruong_cap.aspx" && Sys_User.IS_ROOT != true && (string.IsNullOrEmpty(Sys_This_Cap_Hoc) || !SYS_Cap_Hoc.listData.Contains(Sys_This_Cap_Hoc)))
                {
                    SetChonTruongRequired(path);
                    return;
                }
                #endregion
                #region get detail menu
                MenuBO menubo = new MenuBO();
                Sys_This_Menu = new MENU();
                Sys_This_Menu = menubo.getMENUByUrl(path, Sys_This_Cap_Hoc);
                #endregion
                #region get detail nguoi dung menu
                int count_menu = 0;
                if (Sys_This_Menu != null && Sys_This_Truong != null)
                {
                    NguoiDungMenuBO nguoiDungMenuBO = new NguoiDungMenuBO();
                    Sys_User_Menu = new NGUOI_DUNG_MENU();
                    List<NGUOI_DUNG_MENU> lstUserMenu = new List<NGUOI_DUNG_MENU>();
                    lstUserMenu = nguoiDungMenuBO.getByNguoiDungTruong(Sys_User.ID, Sys_This_Truong.ID);
                    count_menu = lstUserMenu.Where(x => x.ID_MENU == Sys_This_Menu.ID).ToList().Count;
                    Sys_User_Menu = lstUserMenu.FirstOrDefault(x => x.ID_MENU == Sys_This_Menu.ID);
                }
                #endregion
                #region get loại lớp GDTX
                if (Sys_This_Cap_Hoc == SYS_Cap_Hoc.GDTX)
                {
                    Sys_This_Lop_GDTX = Convert.ToInt16(SYS_Profile.getLoaiLopGDTX());
                    if (path.Replace("~/CauHinh/", "").ToLower() != "chontruong_cap.aspx" && Sys_User.IS_ROOT != true && Sys_This_Lop_GDTX == 0)
                    {
                        SetChonTruongRequired(path);
                        return;
                    }
                }
                #endregion
                #region check access link
                string url_check = path.ToLower().Trim().Replace("~/", "");
                if (!is_access(SYS_Type_Access.XEM) && !lstLinkNotCheck.Contains(url_check))
                {
                    SetAccessDenied("Bạn chưa được phân quyền xem chức năng này " + count_menu.ToString()
                        + (Sys_This_Menu == null ? " không thấy menu" : " menu:" + Sys_This_Menu.ID.ToString())
                        + (Sys_User_Menu == null ? " không thấy user" : " is_xem:" + Sys_User_Menu.IS_XEM.ToString()));
                    return;
                }
                #endregion
            }

            base.OnInit(e);
        }
        public bool is_access(int typeAccess)
        {
            if (typeAccess == SYS_Type_Access.XEM)
            {
                if (Sys_User == null || (Sys_User.IS_ROOT != true && (Sys_User_Menu == null || Sys_User_Menu.IS_XEM != true)))
                    return false;
            }
            if (typeAccess == SYS_Type_Access.THEM)
            {
                if (Sys_User == null || (Sys_User.IS_ROOT != true && (Sys_User_Menu == null || Sys_User_Menu.IS_THEM != true)))
                    return false;
            }
            if (typeAccess == SYS_Type_Access.SUA)
            {
                if (Sys_User == null || (Sys_User.IS_ROOT != true && (Sys_User_Menu == null || Sys_User_Menu.IS_SUA != true)))
                    return false;
            }
            if (typeAccess == SYS_Type_Access.XOA)
            {
                if (Sys_User == null || (Sys_User.IS_ROOT != true && (Sys_User_Menu == null || Sys_User_Menu.IS_XOA != true)))
                    return false;
            }
            if (typeAccess == SYS_Type_Access.SEND_SMS)
            {
                if (Sys_User == null || (Sys_User.IS_ROOT != true && (Sys_User_Menu == null || Sys_User_Menu.IS_SEND_SMS != true)))
                    return false;
            }
            if (typeAccess == SYS_Type_Access.VIEW_INFOR)
            {
                if (Sys_User == null || (Sys_User.IS_ROOT != true && (Sys_User_Menu == null || Sys_User_Menu.IS_VIEW_INFOR != true)))
                    return false;
            }
            if (typeAccess == SYS_Type_Access.EXPORT)
            {
                if (Sys_User == null || (Sys_User.IS_ROOT != true && (Sys_User_Menu == null || Sys_User_Menu.IS_EXPORT != true)))
                    return false;
            }
            return true;
        }
        private void Page_Error(object sender, EventArgs e)
        {
            //handle page error
            //ErrorHandleHelper.Page_Error(sender, e);
        }
        private void SetLoginRequired(string path)
        {
            Response.Redirect("~/Login.aspx?returnUrl=" + path + Request.Url.Query);
        }
        public void checkChonTruong()
        {
            string path = Request.AppRelativeCurrentExecutionFilePath.Split('?')[0];
            if (path.Replace("~/CauHinh/", "").ToLower() != "chontruong_cap.aspx" && (Sys_This_Truong == null || string.IsNullOrEmpty(Sys_This_Cap_Hoc)))
            {
                Response.Redirect("~/CauHinh/ChonTruong_Cap.aspx?returnUrl=" + path + Request.Url.Query);
            }
        }
        private void SetChonTruongRequired(string path)
        {
            Response.Redirect("~/CauHinh/ChonTruong_Cap.aspx?returnUrl=" + path + Request.Url.Query);
        }
        private void SetAccessDenied(string msg)
        {
            SYS_Profile.setThongBao(msg);
            Response.Redirect("~/AccessDenied.aspx");
        }
        public void jsAlertAndRedirect(System.Web.UI.Page instance, string Message, string url)
        {
            instance.Response.Write(@"<script language='javascript'>alert('" + Message + "');document.location.href='" + url + "'; </script>");
        }
    }
}