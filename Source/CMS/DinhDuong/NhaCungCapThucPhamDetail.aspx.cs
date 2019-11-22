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

namespace CMS.DinhDuong
{
    public partial class NhaCungCapThucPhamDetail : AuthenticatePage
    {
        long? id;
        NhaCungCapThucPhamBO nhaCungCapBO = new NhaCungCapThucPhamBO();
        LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("id") != null)
            {
                try
                {
                    id = Convert.ToInt16(Request.QueryString.Get("id"));
                }
                catch { }
            }
            if (!IsPostBack)
            {
                if (id != null)
                {
                    DM_NHA_CUNG_CAP_THUC_PHAM detail = new DM_NHA_CUNG_CAP_THUC_PHAM();
                    detail = nhaCungCapBO.getNhaCungCapByID(id.Value);
                    if (detail != null)
                    {
                        tbTen.Text = detail.TEN;
                        tbSDT.Text = detail.SDT != null ? detail.SDT : "";
                        tbDiaChi.Text = detail.DIA_CHI != null ? detail.DIA_CHI : "";
                        tbEmail.Text = detail.EMAIL != null ? detail.EMAIL : "";
                        btEdit.Visible = is_access(SYS_Type_Access.SUA);
                        btAdd.Visible = false;
                    }
                    else
                    {
                        btEdit.Visible = false;
                        btAdd.Visible = is_access(SYS_Type_Access.THEM);
                    }
                }
                else
                {
                    btEdit.Visible = false;
                    btAdd.Visible = is_access(SYS_Type_Access.SUA);
                }
            }
        }
        protected void btAdd_Click(object sender, EventArgs e)
        {
            #region "check du lieu"
            if (!is_access(SYS_Type_Access.THEM))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            #endregion
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            DM_NHA_CUNG_CAP_THUC_PHAM checkExist = nhaCungCapBO.checkExist(Sys_This_Truong.ID, tbTen.Text.Trim());
            if (checkExist == null)
            {
                DM_NHA_CUNG_CAP_THUC_PHAM detail = new DM_NHA_CUNG_CAP_THUC_PHAM();
                detail.TEN = tbTen.Text.Trim();
                detail.ID_TRUONG = Sys_This_Truong.ID;
                if (!string.IsNullOrEmpty(tbDiaChi.Text.Trim())) detail.DIA_CHI = tbDiaChi.Text.Trim();
                if (!string.IsNullOrEmpty(tbSDT.Text.Trim())) detail.SDT = tbSDT.Text.Trim();
                if (!string.IsNullOrEmpty(tbEmail.Text.Trim())) detail.EMAIL = tbEmail.Text.Trim();
                res = nhaCungCapBO.insert(detail, Sys_User.ID);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Tên nhà cung cấp này đã tồn tại. Vui lòng kiểm tra lại!');", true);
                tbTen.Focus();
                return;
            }
            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
                tbTen.Text = "";
                tbTen.Focus();
                tbDiaChi.Text = "";
                tbSDT.Text = "";
                tbEmail.Text = "";
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
        protected void btEdit_Click(object sender, EventArgs e)
        {
            #region "check du lieu"
            if (!is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            #endregion
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            DM_NHA_CUNG_CAP_THUC_PHAM detail = nhaCungCapBO.getNhaCungCapByID(id.Value);
            DM_NHA_CUNG_CAP_THUC_PHAM checkExist = nhaCungCapBO.checkExist(Sys_This_Truong.ID, tbTen.Text.Trim());
            if (checkExist != null && checkExist.ID != id.Value)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Tên nhà cung cấp này đã tồn tại. Vui lòng kiểm tra lại!');", true);
                tbTen.Focus();
                return;
            }
            else
            {
                if (detail != null)
                {
                    detail.TEN = tbTen.Text.Trim();
                    detail.ID_TRUONG = Sys_This_Truong.ID;
                    if (!string.IsNullOrEmpty(tbDiaChi.Text.Trim())) detail.DIA_CHI = tbDiaChi.Text.Trim();
                    if (!string.IsNullOrEmpty(tbSDT.Text.Trim())) detail.SDT = tbSDT.Text.Trim();
                    if (!string.IsNullOrEmpty(tbEmail.Text.Trim())) detail.EMAIL = tbEmail.Text.Trim();
                    res = nhaCungCapBO.update(detail, Sys_User.ID);
                }
            }
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
    }
}