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

namespace CMS.QuanTri
{
    public partial class NguoiDungDonViDetail : AuthenticatePage
    {
        long? id_NguoiDung_req = null;
        private NguoiDungBO nguoiDungBO = new NguoiDungBO();
        NguoiDungTruongBO userTruongBO = new NguoiDungTruongBO();
        private LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("id_hoso") != null)
            {
                try
                {
                    id_NguoiDung_req = Convert.ToInt64(Request.QueryString.Get("id_hoso"));
                }
                catch { }
            }
            if (!IsPostBack)
            {
                objNhomQuyen.SelectParameters.Add("ma", "");
                objNhomQuyen.SelectParameters.Add("ten", "");
                rcbNhomQuyen.DataBind();
                if (id_NguoiDung_req != null)
                {
                    NGUOI_DUNG detail = new NGUOI_DUNG();
                    detail = nguoiDungBO.getNguoiDungByID(id_NguoiDung_req.Value);
                    if (detail != null)
                    {
                        txtUser.Text = detail.TEN_DANG_NHAP;
                        txtPass.Attributes["value"] = detail.MAT_KHAU;
                        txtName.Text = detail.TEN_HIEN_THI;
                        txtMail.Text = detail.EMAIL;
                        txtSDT.Text = detail.SDT;
                        txtAddress.Text = detail.DIA_CHI;
                        txtFb.Text = detail.FACE_BOOK;
                        rcbNhomQuyen.DataBind();
                        if (rcbNhomQuyen.Items.FindItemByValue(detail.MA_NHOM_QUYEN) != null)
                            rcbNhomQuyen.SelectedValue = detail.MA_NHOM_QUYEN.ToString();

                        txtUser.Enabled = false;
                        btEdit.Visible = is_access(SYS_Type_Access.SUA);
                        btAdd.Visible = false;
                    }
                    else
                    {
                        txtUser.Enabled = true;
                        btEdit.Visible = false;
                        btAdd.Visible = is_access(SYS_Type_Access.THEM);
                    }
                }
                else
                {
                    txtUser.Enabled = true;
                    btEdit.Visible = false;
                    btAdd.Visible = is_access(SYS_Type_Access.THEM);
                }
            }
        }
        protected void btAdd_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.THEM))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            NGUOI_DUNG detail = new NGUOI_DUNG();
            detail.TEN_DANG_NHAP = txtUser.Text;
            detail.MAT_KHAU = txtPass.Text;
            detail.TEN_HIEN_THI = txtName.Text;
            detail.EMAIL = txtMail.Text;
            detail.SDT = txtSDT.Text;
            detail.DIA_CHI = txtAddress.Text;
            detail.TRANG_THAI = true;
            detail.MA_NHOM_QUYEN = rcbNhomQuyen.SelectedValue;
            detail.FACE_BOOK = txtFb.Text;

            // check nguoi dung theo so dien thoai
            NGUOI_DUNG nd = nguoiDungBO.checkNguoiDungByPhone(txtSDT.Text.Trim());
            if (nd != null)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Số điện thoại đã tồn tại, vui lòng kiểm tra lại!');", true);
                return;
            }
            ResultEntity res = nguoiDungBO.insert(detail, Sys_User.ID);
            #region "Cập nhật người dùng trường"
            if (res.Res)
            {
                detail = (NGUOI_DUNG)res.ResObject;
                NGUOI_DUNG_TRUONG detailNguoiDungTruong = new NGUOI_DUNG_TRUONG();
                detailNguoiDungTruong.ID_NGUOI_DUNG = detail.ID;
                detailNguoiDungTruong.ID_TRUONG = Sys_This_Truong.ID;
                detailNguoiDungTruong.TRANG_THAI = 1;
                res = userTruongBO.insert(detailNguoiDungTruong, Sys_User.ID, rcbNhomQuyen.SelectedValue);
            }
                
            #endregion
            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
        protected void btEdit_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            string strMsg = "";
            if (id_NguoiDung_req != null && id_NguoiDung_req > 0)
            {
                NGUOI_DUNG detail = new NGUOI_DUNG();
                detail.ID = id_NguoiDung_req.Value;
                detail.TEN_DANG_NHAP = txtUser.Text;
                detail.MAT_KHAU = txtPass.Text;
                detail.TEN_HIEN_THI = txtName.Text;
                detail.EMAIL = txtMail.Text;
                detail.SDT = txtSDT.Text;
                detail.DIA_CHI = txtAddress.Text;
                detail.MA_NHOM_QUYEN = rcbNhomQuyen.SelectedValue;
                detail.FACE_BOOK = txtFb.Text;
                detail.TRANG_THAI = true;
                ResultEntity res = nguoiDungBO.update(detail, Sys_User.ID);
                if (res.Res)
                {
                    strMsg = "notification('success', '" + res.Msg + "');";
                }
                else
                {
                    strMsg = "notification('error', '" + res.Msg + "');";
                }

            }
            else
            {
                strMsg = "notification('error', 'Có lỗi xảy ra');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
    }
}