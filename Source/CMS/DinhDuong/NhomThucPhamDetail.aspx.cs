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

namespace CMS.MamNon_DanhMuc
{
    public partial class NhomThucPhamDetail : AuthenticatePage
    {
        short? id;
        NhomThucPhamBO nhomThucPhamBO = new NhomThucPhamBO();
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
                    DM_NHOM_THUC_PHAM detail = new DM_NHOM_THUC_PHAM();
                    detail = nhomThucPhamBO.getNhomThucPhamByID(id.Value);
                    if (detail != null)
                    {
                        tbTen.Text = detail.TEN;
                        if (detail.THU_TU != null) tbThuTu.Text = detail.THU_TU.ToString();
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
                    getMaxThuTu();
                }
            }
        }
        protected void getMaxThuTu()
        {
            long? max_thu_tu = nhomThucPhamBO.getMaxThuTu();
            if (max_thu_tu != null)
                tbThuTu.Text = Convert.ToString(Convert.ToInt16(max_thu_tu) + 1);
            else tbThuTu.Text = "1";
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
            DM_NHOM_THUC_PHAM checkExist = nhomThucPhamBO.checkExistNhomThucPham(tbTen.Text.Trim());
            if (checkExist == null)
            {
                DM_NHOM_THUC_PHAM detail = new DM_NHOM_THUC_PHAM();
                detail.TEN = tbTen.Text.Trim();
                detail.THU_TU = localAPI.ConvertStringToShort(tbThuTu.Text.Trim());
                res = nhomThucPhamBO.insert(detail, Sys_User.ID);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Tên nhóm thực phẩm này đã tồn tại. Vui lòng kiểm tra lại!');", true);
                tbTen.Focus();
                return;
            }
            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
                tbTen.Text = "";
                tbTen.Focus();
                getMaxThuTu();
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
            DM_NHOM_THUC_PHAM detail = new DM_NHOM_THUC_PHAM();
            detail = nhomThucPhamBO.getNhomThucPhamByID(id.Value);
            DM_NHOM_THUC_PHAM checkExist = nhomThucPhamBO.checkExistNhomThucPham(tbTen.Text.Trim());
            if (checkExist != null && checkExist.ID != id.Value)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Tên nhóm thực phẩm này đã tồn tại. Vui lòng kiểm tra lại!');", true);
                tbTen.Focus();
                return;
            }
            else
            {
                if (detail != null)
                {
                    detail.TEN = tbTen.Text.Trim();
                    detail.THU_TU = localAPI.ConvertStringToShort(tbThuTu.Text.Trim());
                    res = nhomThucPhamBO.update(detail, Sys_User.ID);
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