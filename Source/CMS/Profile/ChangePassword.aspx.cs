using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS.Profile
{
    public partial class ChangePassword : AuthenticatePage
    {
        private NguoiDungBO nguoiDungBO = new NguoiDungBO();
        private LoginHelper loginHelper = new LoginHelper();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btSave_Click(object sender, EventArgs e)
        {
            NGUOI_DUNG ndDetail = new NGUOI_DUNG();
            ndDetail = nguoiDungBO.getNguoiDungByID(Sys_User.ID);

            string pass_old = tbMatKhau.Text;
            string pass_new = tbMatKhauMoi.Text;
            string pass_confirm = tbMatKhauMoiNhapLai.Text;

            if (ndDetail.MAT_KHAU != pass_old)
            {
                string script = "notification('error','Mật khẩu cũ chưa đúng');";
                ScriptManager.RegisterStartupScript(this, GetType(), "Alert", script, true);
                return;
            }

            if (!string.IsNullOrEmpty(pass_new) && !string.IsNullOrEmpty(pass_confirm))
            {

                if (pass_new.Length < 6)
                {
                    string script = "notification('error','Mật khẩu phải có độ dài lớn hơn 6');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "Alert", script, true);
                    return;
                }

                if (pass_new != pass_confirm)
                {
                    string script = "notification('error','Mật khẩu xác nhận chưa trùng khớp');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "Alert", script, true);
                    return;
                }

                ndDetail.MAT_KHAU = tbMatKhauMoi.Text.Trim();
                ResultEntity res = nguoiDungBO.update(ndDetail, Sys_User.ID);
                if (res.Res)
                {
                    ndDetail = (NGUOI_DUNG)res.ResObject;
                    loginHelper.SetLoginSuccess(ndDetail);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('success', 'Đổi mật khẩu thành công');setTimeout(function(){ document.location.href='" + "http://" + HttpContext.Current.Request.Url.Authority + "/Logout.aspx'; }, 1000);", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Có lỗi xãy ra!');", true);
                }
            }
            else
            {
                string script = "notification('error','Bạn phải nhập đầy đủ thông tin');";
                ScriptManager.RegisterStartupScript(this, GetType(), "Alert", script, true);
                return;
            }
        }
    }
}