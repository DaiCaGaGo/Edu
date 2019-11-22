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

namespace CMS.QuanLySach
{
    public partial class DanhMucSachDetail : AuthenticatePage
    {
        long? id;
        DMSachBO dmSachBO = new DMSachBO();
        LocalAPI localAPI = new LocalAPI();
        LogUserBO logUserBO = new LogUserBO();
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
                objKhoi.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                objKhoi.SelectParameters.Add("maLoaiLopGDTX", DbType.Int16, Sys_This_Lop_GDTX == null ? "" : Sys_This_Lop_GDTX.ToString());
                rcbKhoiHoc.DataBind();
                objMonHoc.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                rcbMonHoc.DataBind();
                //tbNamHoc.Text = "2018 - 2019";
                if (id != null)
                {
                    DM_SACH detail = new DM_SACH();
                    detail = dmSachBO.getDMSachByID(id.Value);
                    if (detail != null)
                    {
                        tbTen.Text = detail.TEN;
                        rcbKhoiHoc.SelectedValue = detail.ID_KHOI.ToString();
                        rcbMonHoc.SelectedValue = detail.ID_MON_HOC.ToString();
                        tbGhiChu.Text = detail.GHI_CHU;
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
                    btAdd.Visible = is_access(SYS_Type_Access.THEM);
                }
            }
        }
        protected void rcbKhoiHoc_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbMonHoc.ClearSelection();
            rcbMonHoc.Text = string.Empty;
            rcbMonHoc.DataBind();
        }
        protected void btAdd_Click(object sender, EventArgs e)
        {
            #region "check du lieu"
            if (!is_access(SYS_Type_Access.THEM))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            if (string.IsNullOrEmpty(rcbKhoiHoc.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn vui lòng chọn khối học!');", true);
                return;
            }
            if (string.IsNullOrEmpty(rcbMonHoc.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn vui lòng chọn môn học!');", true);
                return;
            }
            #endregion
            DM_SACH detail = new DM_SACH();
            detail.TEN = tbTen.Text.Trim();
            detail.ID_KHOI = Convert.ToInt16(rcbKhoiHoc.SelectedValue);
            detail.ID_MON_HOC = Convert.ToInt16(rcbMonHoc.SelectedValue);
            detail.GHI_CHU = tbGhiChu.Text.Trim();
            ResultEntity res = dmSachBO.insert(detail, Sys_User.ID);
            string strMsg = "";
            if (res.Res)
            {
                tbTen.Text = "";
                tbTen.Focus();
                tbGhiChu.Text = "";
                strMsg = "notification('success', '" + res.Msg + "');";
                DM_SACH resMa = (DM_SACH)res.ResObject;
                logUserBO.insert(null, "INSERT", "Thêm mới sách " + resMa.ID, Sys_User.ID, DateTime.Now);
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
            if (string.IsNullOrEmpty(rcbKhoiHoc.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn vui lòng chọn khối học!');", true);
                return;
            }
            if (string.IsNullOrEmpty(rcbMonHoc.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn vui lòng chọn môn học!');", true);
                return;
            }
            #endregion
            DM_SACH detail = new DM_SACH();
            detail = dmSachBO.getDMSachByID(id.Value);
            if (detail == null) detail = new DM_SACH();
            detail.TEN = tbTen.Text.Trim();
            detail.ID_KHOI = Convert.ToInt16(rcbKhoiHoc.SelectedValue);
            detail.ID_MON_HOC = Convert.ToInt16(rcbMonHoc.SelectedValue);
            detail.GHI_CHU = tbGhiChu.Text.Trim();
            ResultEntity res = dmSachBO.update(detail, Sys_User.ID);
            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
                logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật sách " + id, Sys_User.ID, DateTime.Now);
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
    }
}