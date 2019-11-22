using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CMS.QuanTri
{
    public partial class NguoiDungDetail : AuthenticatePage
    {
        public long? id_NguoiDung_req = null;
        private NguoiDungBO nguoiDungBO = new NguoiDungBO();
        private LocalAPI localAPI = new LocalAPI();
        LogUserBO logUserBO = new LogUserBO();
        NguoiDungTruongBO nguoiDungTruongBO = new NguoiDungTruongBO();
        NguoiDungMenuBO nguoiDungMenuBO = new NguoiDungMenuBO();
        TruongBO truongBO = new TruongBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("id_hoso") != null)
            {
                try
                {
                    id_NguoiDung_req = Convert.ToInt64(Request.QueryString.Get("id_hoso"));
                    if (IsPostBack)
                        btNguoiDungTruong.Attributes.Add("onclick", "");
                }
                catch { }
            }
            if (Sys_User.IS_ROOT != true) btNguoiDungTruong.Visible = false;
            else btNguoiDungTruong.Visible = true;
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
                        //txtPass.Visible = false;
                        chbLaDaiLy.Checked = detail.ID_DOI_TAC != null && detail.ID_DOI_TAC > 0 ? true : false;
                        if (chbLaDaiLy.Checked)
                        {
                            divDaiLy.Visible = true;
                            rcbDaiLy.SelectedValue = detail.ID_DOI_TAC != null ? detail.ID_DOI_TAC.ToString() : "";
                        }
                        else divDaiLy.Visible = false;
                        btEdit.Visible = true;
                        btAdd.Visible = false;
                        btNguoiDungTruong.Visible = Sys_User.IS_ROOT == true ? true : false;
                    }
                    else
                    {
                        txtUser.Enabled = true;
                        //txtPass.Visible = true;
                        btEdit.Visible = false;
                        btAdd.Visible = true;
                        btNguoiDungTruong.Visible = false;
                    }
                }
                else
                {
                    txtUser.Enabled = true;
                    //txtPass.Visible = true;
                    btEdit.Visible = false;
                    btAdd.Visible = true;
                    btNguoiDungTruong.Visible = false;
                }
            }
        }
        protected void btAdd_Click(object sender, EventArgs e)
        {
            #region "check du lieu"
            if (txtSDT.Text.Trim() != "" && txtSDT.Text.Trim().Length > 20)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Số điện thoại không hợp lệ');", true);
                txtSDT.Focus();
                return;
            }
            if (txtSDT.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Số điện thoại không được để trống!');", true);
                txtSDT.Focus();
                return;
            }
            NGUOI_DUNG nd = nguoiDungBO.checkNguoiDungByPhone(txtSDT.Text.Trim());
            if (nd != null)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Số điện thoại đã tồn tại, vui lòng kiểm tra lại!');", true);
                txtSDT.Focus();
                return;
            }
            NGUOI_DUNG checkUser = nguoiDungBO.checkNguoiDungByUser(txtUser.Text.Trim());
            if (checkUser != null)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Tên đăng nhập này đã tồn tại. Vui lòng kiểm tra lại!');", true);
                txtUser.Focus();
                return;
            }
            if (txtMail.Text.Trim() != "")
            {
                NGUOI_DUNG checkEmail = nguoiDungBO.checkNguoiDungByEmail(txtMail.Text.Trim());
                if (checkEmail != null)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Email này đã tồn tại. Vui lòng kiểm tra lại!');", true);
                    txtMail.Focus();
                    return;
                }
            }
            if (chbLaDaiLy.Checked && rcbDaiLy.SelectedValue == "")
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn vui lòng chọn tên đại lý!');", true);
                rcbDaiLy.Focus();
                return;
            }
            #endregion
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
            if (chbLaDaiLy.Checked) detail.ID_DOI_TAC = localAPI.ConvertStringToShort(rcbDaiLy.SelectedValue);
            else detail.ID_DOI_TAC = null;
            ResultEntity res = nguoiDungBO.insert(detail, Sys_User.ID);
            string strMsg = "";
            if (res.Res)
            {
                NGUOI_DUNG resUSER = (NGUOI_DUNG)res.ResObject;
                logUserBO.insert(null, "INSERT", "Thêm mới người dùng " + resUSER.ID, Sys_User.ID, DateTime.Now);
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
            #region check du lieu
            if (txtSDT.Text.Trim() != "" && txtSDT.Text.Trim().Length > 20)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Số điện thoại không hợp lệ');", true);
                txtSDT.Focus();
                return;
            }
            if (txtSDT.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Số điện thoại không được để trống!');", true);
                txtSDT.Focus();
                return;
            }
            if (chbLaDaiLy.Checked && rcbDaiLy.SelectedValue == "")
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn vui lòng chọn tên đại lý!');", true);
                rcbDaiLy.Focus();
                return;
            }
            #endregion
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            string strMsg = "";
            if (id_NguoiDung_req != null && id_NguoiDung_req > 0)
            {
                NGUOI_DUNG detail = new NGUOI_DUNG();
                detail = nguoiDungBO.getNguoiDungByID(id_NguoiDung_req.Value);
                if (detail != null)
                {
                    string ma_nhom_quyen_old = detail.MA_NHOM_QUYEN;
                    detail.TEN_DANG_NHAP = txtUser.Text;
                    detail.MAT_KHAU = txtPass.Text;
                    detail.TEN_HIEN_THI = txtName.Text;
                    detail.EMAIL = txtMail.Text;
                    detail.SDT = txtSDT.Text;
                    detail.DIA_CHI = txtAddress.Text;
                    detail.MA_NHOM_QUYEN = rcbNhomQuyen.SelectedValue;
                    detail.FACE_BOOK = txtFb.Text;
                    detail.TRANG_THAI = true;
                    if (chbLaDaiLy.Checked) detail.ID_DOI_TAC = localAPI.ConvertStringToShort(rcbDaiLy.SelectedValue);
                    else detail.ID_DOI_TAC = null;
                    #region check exists
                    if (txtUser.Text.Trim() != "")
                    {
                        NGUOI_DUNG checkUser = nguoiDungBO.checkNguoiDungByUser(txtUser.Text.Trim());
                        if (checkUser != null && id_NguoiDung_req != checkUser.ID)
                        {
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Tên đăng nhập này đã tồn tại. Vui lòng kiểm tra lại!');", true);
                            return;
                        }
                    }
                    if (txtSDT.Text.Trim() != "")
                    {
                        NGUOI_DUNG checkSDT = nguoiDungBO.checkNguoiDungByPhone(txtSDT.Text.Trim());
                        if (checkSDT != null && id_NguoiDung_req != checkSDT.ID)
                        {
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Số điện thoại này đã tồn tại. Vui lòng kiểm tra lại!');", true);
                            txtSDT.Focus();
                            return;
                        }
                    }
                    if (txtMail.Text.Trim() != "")
                    {
                        NGUOI_DUNG checkEmail = nguoiDungBO.checkNguoiDungByEmail(txtMail.Text.Trim());
                        if (checkEmail != null && id_NguoiDung_req != checkEmail.ID)
                        {
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Email này đã tồn tại. Vui lòng kiểm tra lại!');", true);
                            txtMail.Focus();
                            return;
                        }
                    }
                    #endregion
                    res = nguoiDungBO.update(detail, Sys_User.ID);
                    #region cập nhật lại người dùng menu theo nhóm quyền
                    if (res.Res && ma_nhom_quyen_old != rcbNhomQuyen.SelectedValue)
                    {
                        List<NGUOI_DUNG_TRUONG> lstTruongByUser = nguoiDungTruongBO.getListTruongByNguoiDung(id_NguoiDung_req.Value);
                        if (lstTruongByUser.Count > 0)
                        {
                            for (int i = 0; i < lstTruongByUser.Count; i++)
                            {
                                long id_truong = lstTruongByUser[i].ID_TRUONG;
                                TRUONG truong = truongBO.getTruongById(id_truong);
                                if (truong != null)
                                {
                                    if (truong.IS_MN == true)
                                    {
                                        nguoiDungMenuBO.updateNguoiDungMenuKhiUpdateNhomQuyen(id_NguoiDung_req.Value, id_truong, "MN", ma_nhom_quyen_old, rcbNhomQuyen.SelectedValue, Sys_User.ID);
                                    }
                                    if (truong.IS_TH == true)
                                    {
                                        nguoiDungMenuBO.updateNguoiDungMenuKhiUpdateNhomQuyen(id_NguoiDung_req.Value, id_truong, "TH", ma_nhom_quyen_old, rcbNhomQuyen.SelectedValue, Sys_User.ID);
                                    }
                                    if (truong.IS_THCS == true)
                                    {
                                        nguoiDungMenuBO.updateNguoiDungMenuKhiUpdateNhomQuyen(id_NguoiDung_req.Value, id_truong, "THCS", ma_nhom_quyen_old, rcbNhomQuyen.SelectedValue, Sys_User.ID);
                                    }
                                    if (truong.IS_THPT == true)
                                    {
                                        nguoiDungMenuBO.updateNguoiDungMenuKhiUpdateNhomQuyen(id_NguoiDung_req.Value, id_truong, "THPT", ma_nhom_quyen_old, rcbNhomQuyen.SelectedValue, Sys_User.ID);
                                    }
                                }
                                logUserBO.insert(null, "UPDATE", "Cập nhật menu người dùng từ nhóm quyền " + ma_nhom_quyen_old + " sang nhóm quyền " + rcbNhomQuyen.SelectedValue, Sys_User.ID, DateTime.Now);
                            }
                        }
                    }
                    #endregion
                }
                if (res.Res)
                {
                    strMsg = "notification('success', '" + res.Msg + "');";
                    logUserBO.insert(null, "UPDATE", "Cập nhật người dùng " + id_NguoiDung_req, Sys_User.ID, DateTime.Now);
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
        protected void chbLaDaiLy_CheckedChanged(object sender, EventArgs e)
        {
            divDaiLy.Visible = chbLaDaiLy.Checked;
        }
    }
}