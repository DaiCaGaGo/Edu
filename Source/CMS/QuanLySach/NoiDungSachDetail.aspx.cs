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
    public partial class NoiDungSachDetail : AuthenticatePage
    {
        long? id;
        DMSachNoiDungBO dmSachNoiDungBO = new DMSachNoiDungBO();
        LogUserBO logUserBO = new LogUserBO();
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
                objKhoi.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                objKhoi.SelectParameters.Add("maLoaiLopGDTX", DbType.Int16, Sys_This_Lop_GDTX == null ? "" : Sys_This_Lop_GDTX.ToString());
                rcbKhoiHoc.DataBind();
                objMonHoc.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                rcbMonHoc.DataBind();
                objSach.SelectParameters.Add("ma_cap_hoc", Sys_This_Cap_Hoc);
                objSach.SelectParameters.Add("ten", "");
                rcbSach.DataBind();
                if (id != null)
                {
                    DM_SACH_NOI_DUNG detail = new DM_SACH_NOI_DUNG();
                    detail = dmSachNoiDungBO.getNoiDungSachByID(id.Value);
                    if (detail != null)
                    {
                        tbTen.Text = detail.TEN_BAI_HOC;
                        tbSoTrangs.Text = detail.SO_TRANGS;
                        rcbKhoiHoc.SelectedValue = detail.ID_KHOI.ToString();
                        rcbMonHoc.SelectedValue = detail.ID_MON_HOC.ToString();
                        rcbSach.SelectedValue = detail.ID_SACH.ToString();
                        tbGhiChu.Text = detail.GHI_CHU;
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
                    btAdd.Visible = is_access(SYS_Type_Access.THEM);
                    getMaxThuTu();
                }
            }
        }
        protected void getMaxThuTu()
        {
            if (!string.IsNullOrEmpty(rcbKhoiHoc.SelectedValue) && !string.IsNullOrEmpty(rcbMonHoc.SelectedValue) && !string.IsNullOrEmpty(rcbSach.SelectedValue))
            {

                long? max_thu_tu = dmSachNoiDungBO.getMaxThuTuBySach(Convert.ToInt16(rcbKhoiHoc.SelectedValue), Convert.ToInt16(rcbMonHoc.SelectedValue), Convert.ToInt64(rcbSach.SelectedValue));
                if (max_thu_tu != null)
                    tbThuTu.Text = Convert.ToString(Convert.ToInt16(max_thu_tu) + 1);
                else tbThuTu.Text = "1";
            }
            else tbThuTu.Text = "";
        }
        protected void btAdd_Click(object sender, EventArgs e)
        {
            #region "check du lieu"
            if (!is_access(SYS_Type_Access.THEM))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            if (string.IsNullOrEmpty(rcbMonHoc.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn vui lòng chọn môn học!');", true);
                return;
            }
            if (string.IsNullOrEmpty(rcbSach.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn vui lòng chọn sách!');", true);
                return;
            }
            #endregion
            DM_SACH_NOI_DUNG detail = new DM_SACH_NOI_DUNG();
            detail.TEN_BAI_HOC = tbTen.Text.Trim();
            detail.SO_TRANGS = tbSoTrangs.Text.Trim();
            detail.ID_KHOI = Convert.ToInt16(rcbKhoiHoc.SelectedValue);
            detail.ID_MON_HOC = Convert.ToInt16(rcbMonHoc.SelectedValue);
            detail.ID_SACH = Convert.ToInt64(rcbSach.SelectedValue);
            detail.GHI_CHU = tbGhiChu.Text.Trim();
            detail.THU_TU = localAPI.ConvertStringToShort(tbThuTu.Text.Trim());
            ResultEntity res = dmSachNoiDungBO.insert(detail, Sys_User.ID);
            string strMsg = "";
            if (res.Res)
            {
                tbTen.Text = "";
                tbTen.Focus();
                tbSoTrangs.Text = "";
                tbGhiChu.Text = "";
                getMaxThuTu();
                strMsg = "notification('success', '" + res.Msg + "');";
                DM_SACH_NOI_DUNG resMa = (DM_SACH_NOI_DUNG)res.ResObject;
                logUserBO.insert(null, "INSERT", "Thêm mới tên bài học " + resMa.ID, Sys_User.ID, DateTime.Now);
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
            if (string.IsNullOrEmpty(rcbMonHoc.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn vui lòng chọn môn học!');", true);
                return;
            }
            if (string.IsNullOrEmpty(rcbSach.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn vui lòng chọn sách!');", true);
                return;
            }
            #endregion
            DM_SACH_NOI_DUNG detail = new DM_SACH_NOI_DUNG();
            detail = dmSachNoiDungBO.getNoiDungSachByID(id.Value);
            if (detail == null) detail = new DM_SACH_NOI_DUNG();
            detail.TEN_BAI_HOC = tbTen.Text.Trim();
            detail.SO_TRANGS = tbSoTrangs.Text.Trim();
            detail.ID_KHOI = Convert.ToInt16(rcbKhoiHoc.SelectedValue);
            detail.ID_MON_HOC = Convert.ToInt16(rcbMonHoc.SelectedValue);
            detail.ID_SACH = Convert.ToInt64(rcbSach.SelectedValue);
            detail.GHI_CHU = tbGhiChu.Text.Trim();
            detail.THU_TU = localAPI.ConvertStringToShort(tbThuTu.Text.Trim());
            ResultEntity res = dmSachNoiDungBO.update(detail, Sys_User.ID);
            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
                logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật tên bài học " + id, Sys_User.ID, DateTime.Now);
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
        protected void rcbKhoiHoc_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbMonHoc.ClearSelection();
            rcbMonHoc.Text = string.Empty;
            rcbMonHoc.DataBind();
            rcbSach.ClearSelection();
            rcbSach.Text = string.Empty;
            rcbSach.DataBind();
        }
        protected void rcbMonHoc_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbSach.ClearSelection();
            rcbSach.Text = string.Empty;
            rcbSach.DataBind();
        }
    }
}