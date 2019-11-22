using CMS.XuLy;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnDangKy_Click(object sender, EventArgs e)
        {
            NguoiDungBO nd_bo = new NguoiDungBO();
            NGUOI_DUNG nd = new NGUOI_DUNG();
            LocalAPI local_api = new LocalAPI();
            nd = nd_bo.checkNguoiDungByUser(txtUser.Text.Trim());
            if (nd != null)
            {
                lblMess.Text = "Tên người dùng này đã tồn tại, vui lòng chọn tên khác!";
                return;
            }
            nd = nd_bo.checkNguoiDungByPhone(txtSDT.Text.Trim());
            if (nd != null)
            {
                lblMess.Text = "Số điện thoại đã tồn tại, vui lòng kiểm tra lại!";
                return;
            }
            if (ddlVaiTro.SelectedValue == "0")
            {
                lblMess.Text = "Vui lòng chọn vai trò người dùng!";
                return;
            }
            nd = new NGUOI_DUNG();
            nd.TEN_HIEN_THI = txtHoTen.Text.Trim();
            nd.TEN_DANG_NHAP = txtUser.Text.Trim();
            nd.MAT_KHAU = local_api.encryption(txtPass.Text.Trim());
            nd.SDT = txtSDT.Text.Trim();
            if (txtEmail.Text.Trim() != "") nd.EMAIL = txtEmail.Text.Trim();
            nd.IS_DELETE = false;
            nd_bo.insert(nd, 0);
        }
    }
}