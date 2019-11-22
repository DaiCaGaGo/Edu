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

namespace CMS.NhanXetHangNgay
{
    public partial class MaNXHNDetail : AuthenticatePage
    {
        long? id;
        MaNhanXetBO maBO = new MaNhanXetBO();
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
                if (id != null)
                {
                    MA_NXHN detail = new MA_NXHN();
                    detail = maBO.getMaNhanXetByID(id.Value);
                    if (detail != null)
                    {
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
            long? max_thu_tu = maBO.getMaxThuTuByTruong(Sys_This_Truong.ID, Sys_This_Cap_Hoc);
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
            MA_NXHN checkMa = maBO.getMaNhanXetByMa(Sys_This_Truong.ID, tbMa.Text.Trim());
            if (checkMa != null)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Mã nhận xét đã tồn tại, vui lòng kiểm tra lại!');", true);
                tbMa.Focus();
                return;
            }
            #endregion
            MA_NXHN detail = new MA_NXHN();
            detail.MA = tbMa.Text.Trim();
            detail.NOI_DUNG = tbNoiDung.Text.Trim();
            detail.ID_TRUONG = Sys_This_Truong.ID;
            detail.THU_TU = localAPI.ConvertStringToShort(tbThuTu.Text.Trim());
            detail.MA_CAP_HOC = Sys_This_Cap_Hoc;
            ResultEntity res = maBO.insert(detail, Sys_User.ID);
            string strMsg = "";
            if (res.Res)
            {
                tbMa.Text = "";
                tbMa.Focus();
                tbNoiDung.Text = "";
                getMaxThuTu();
                strMsg = "notification('success', '" + res.Msg + "');";
                MA_NXHN resMa = (MA_NXHN)res.ResObject;
                logUserBO.insert(Sys_This_Truong.ID, "INSERT", "Thêm mới mã NXHN " + resMa.ID, Sys_User.ID, DateTime.Now);
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

            MA_NXHN checkMa = maBO.getMaNhanXetByMa(Sys_This_Truong.ID, tbMa.Text.Trim());
            if (checkMa != null && checkMa.ID != id)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Mã nhận xét đã tồn tại, vui lòng kiểm tra lại!');", true);
                tbMa.Focus();
                return;
            }
            #endregion
            MA_NXHN detail = new MA_NXHN();
            detail.ID = id.Value;
            detail = maBO.getMaNhanXetByID(detail.ID);
            if (detail == null) detail = new MA_NXHN();
            if (detail.ID_TRUONG != Sys_This_Truong.ID)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện sửa dữ liệu này!');", true);
                return;
            }
            detail.MA = tbMa.Text.Trim();
            detail.NOI_DUNG = tbNoiDung.Text.Trim();
            detail.THU_TU = localAPI.ConvertStringToShort(tbThuTu.Text.Trim());
            detail.MA_CAP_HOC = Sys_This_Cap_Hoc;
            ResultEntity res = maBO.update(detail, Sys_User.ID);
            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
                logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật mã NXHN " + id, Sys_User.ID, DateTime.Now);
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
    }
}