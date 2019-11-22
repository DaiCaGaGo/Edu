using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CMS.Profile
{
    public partial class HoSo : AuthenticatePage
    {
        private NguoiDungBO nguoiDungBO = new NguoiDungBO();
        private LoginHelper loginHelper = new LoginHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            if (!IsPostBack)
            {
                tbTenHienThi.Text = Sys_User.TEN_HIEN_THI;
                tbEmail.Text = Sys_User.EMAIL;
                tbSDT.Text = Sys_User.SDT;
                tbDiaChi.Text = Sys_User.DIA_CHI;
                if (!string.IsNullOrEmpty(Sys_User.ANH_DAI_DIEN))
                {
                    imgAnh.ImageUrl = Sys_User.ANH_DAI_DIEN;
                    imgAnh.Visible = true;
                }
                tbFacebook.Text = Sys_User.FACE_BOOK;
                tbGhiChu.Text = Sys_User.GHI_CHU;
                cbLoginGmail.Checked = (Sys_User.IS_LOGIN_GMAIL == true) ? true : false;
                cbLoginFb.Checked = (Sys_User.IS_LOGIN_FACEBOOK == true) ? true : false;
            }
        }
        protected void btSave_Click(object sender, EventArgs e)
        {
            NGUOI_DUNG detail = new NGUOI_DUNG();
            detail = nguoiDungBO.getNguoiDungByID(Sys_User.ID);
            if(tbMatKhau.Text.Trim()!=detail.MAT_KHAU)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Mật khẩu không đúng!');", true);
                tbMatKhau.Focus();
                return;
            }
            detail.ID = Sys_User.ID;
            detail.TEN_HIEN_THI = tbTenHienThi.Text;
            detail.EMAIL = tbEmail.Text;
            detail.SDT = tbSDT.Text;
            detail.DIA_CHI = tbDiaChi.Text;
            foreach (UploadedFile f in rauAnh.UploadedFiles)
            {
                Guid strGuid = Guid.NewGuid();
                string fileName = strGuid.ToString() + f.GetName();
                string path = Server.MapPath("~/img/AnhNguoiDung/" + fileName);
                f.SaveAs(path);
                detail.ANH_DAI_DIEN = "~/img/AnhNguoiDung/" + fileName;
            }
            detail.FACE_BOOK = tbFacebook.Text;
            detail.GHI_CHU = tbGhiChu.Text;
            detail.IS_LOGIN_GMAIL = cbLoginGmail.Checked;
            detail.IS_LOGIN_FACEBOOK = cbLoginFb.Checked;
            ResultEntity res= nguoiDungBO.update(detail,Sys_User.ID);
            if (res.Res)
            {
                detail=(NGUOI_DUNG)res.ResObject;
                loginHelper.SetLoginSuccess(detail);
                Sys_User = loginHelper.GetUserLogged;// Lấy lại từ profile
                if (!string.IsNullOrEmpty(detail.ANH_DAI_DIEN))
                {
                    imgAnh.ImageUrl = detail.ANH_DAI_DIEN;
                    imgAnh.Visible = true;
                }
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('success', 'Thao tác thành công');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Có lỗi xãy ra!');", true);
            }
        }
    }
}