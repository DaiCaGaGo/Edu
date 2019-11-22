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
using Telerik.Web.UI;

namespace CMS.QuanTri
{
    public partial class QuanLyMaNXHNDetail : AuthenticatePage
    {
        long? id;
        DmMaNhanXetBO maBO = new DmMaNhanXetBO();
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
            rcbCapHoc.Items.Insert(0, new RadComboBoxItem("Tiểu học", "TH"));
            rcbCapHoc.Items.Insert(1, new RadComboBoxItem("Trung học cơ sở", "THCS"));
            rcbCapHoc.Items.Insert(2, new RadComboBoxItem("Trung học phổ thông", "THPT"));
            rcbCapHoc.DataBind();
            if (!IsPostBack)
            {
                if (id != null)
                {
                    DM_MA_NX detail = new DM_MA_NX();
                    detail = maBO.getMaNhanXetByID(id.Value);
                    if (detail != null)
                    {
                        rcbCapHoc.SelectedValue = detail.MA_CAP_HOC != null ? detail.MA_CAP_HOC : "";
                        tbMa.Text = detail.MA;
                        tbNoiDung.Text = detail.NOI_DUNG;
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
            string ma_cap_hoc = "";
            if (rcbCapHoc.SelectedValue != null) ma_cap_hoc = rcbCapHoc.SelectedValue;
            if (ma_cap_hoc != "")
            {
                long? max_thu_tu = maBO.getMaxThuTu(ma_cap_hoc);
                if (max_thu_tu != null)
                    tbThuTu.Text = Convert.ToString(Convert.ToInt16(max_thu_tu) + 1);
                else tbThuTu.Text = "1";
            }
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
            DM_MA_NX checkMa = maBO.getMaNhanXetByMa(rcbCapHoc.SelectedValue, tbMa.Text.Trim());
            if (checkMa != null)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Mã nhận xét đã tồn tại, vui lòng kiểm tra lại!');", true);
                tbMa.Focus();
                return;
            }
            #endregion
            DM_MA_NX detail = new DM_MA_NX();
            detail.MA = tbMa.Text.Trim();
            detail.NOI_DUNG = tbNoiDung.Text.Trim();
            detail.THU_TU = localAPI.ConvertStringToShort(tbThuTu.Text.Trim());
            detail.MA_CAP_HOC = rcbCapHoc.SelectedValue;
            ResultEntity res = maBO.insert(detail, Sys_User.ID);
            string strMsg = "";
            if (res.Res)
            {
                tbMa.Text = "";
                tbMa.Focus();
                tbNoiDung.Text = "";
                getMaxThuTu();
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
            #region "check du lieu"
            if (!is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            DM_MA_NX checkMa = maBO.getMaNhanXetByMa(rcbCapHoc.SelectedValue, tbMa.Text.Trim());
            if (checkMa != null && checkMa.ID != id)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Mã nhận xét đã tồn tại, vui lòng kiểm tra lại!');", true);
                tbMa.Focus();
                return;
            }
            #endregion
            DM_MA_NX detail = new DM_MA_NX();
            detail = maBO.getMaNhanXetByID(id.Value);
            if (detail == null) detail = new DM_MA_NX();
            detail.MA = tbMa.Text.Trim();
            detail.NOI_DUNG = tbNoiDung.Text.Trim();
            detail.THU_TU = localAPI.ConvertStringToShort(tbThuTu.Text.Trim());
            detail.MA_CAP_HOC = rcbCapHoc.SelectedValue;
            ResultEntity res = maBO.update(detail, Sys_User.ID);
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
        protected void rcbCapHoc_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            getMaxThuTu();
        }
    }
}