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

namespace CMS.NhanXetHangNgay
{
    public partial class MaNXDKDetail : AuthenticatePage
    {
        long? id;
        MaNXDinhKyBO maBO = new MaNXDinhKyBO();
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
                    MA_NX_DINH_KY detail = new MA_NX_DINH_KY();
                    detail = maBO.getMaNhanXetByID(id.Value);
                    if (detail != null)
                    {
                        tbMa.Text = detail.MA;
                        tbNoiDung.Text = detail.NOI_DUNG;
                        if (detail.THU_TU != null) tbThuTu.Text = detail.THU_TU.ToString();
                        rcbLoaiNX.SelectedValue = detail.MA_LOAI_NX.ToString();
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
            long? max_thu_tu = maBO.getMaxThuTuByTruong(Sys_This_Truong.ID);
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
            MA_NX_DINH_KY checkMa = maBO.getMaNhanXetByMa(Sys_This_Truong.ID, tbMa.Text.Trim());
            if (checkMa != null)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Mã nhận xét đã tồn tại, vui lòng kiểm tra lại!');", true);
                tbMa.Focus();
                return;
            }
            #endregion
            MA_NX_DINH_KY detail = new MA_NX_DINH_KY();
            detail.MA = tbMa.Text.Trim();
            detail.NOI_DUNG = tbNoiDung.Text.Trim();
            detail.ID_TRUONG = Sys_This_Truong.ID;
            detail.THU_TU = localAPI.ConvertStringToShort(tbThuTu.Text.Trim());
            detail.MA_LOAI_NX = Convert.ToInt16(rcbLoaiNX.SelectedValue);
            ResultEntity res = maBO.insert(detail, Sys_User.ID);
            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
                reset();
                MA_NX_DINH_KY resMa = (MA_NX_DINH_KY)res.ResObject;
                logUserBO.insert(Sys_This_Truong.ID, "INSERT", "Thêm mới mã NXĐK " + resMa.ID, Sys_User.ID, DateTime.Now);
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
        protected void reset()
        {
            tbMa.Text = "";
            tbMa.Focus();
            tbNoiDung.Text = "";
            rcbLoaiNX.ClearSelection();
            getMaxThuTu();
        }
        protected void btEdit_Click(object sender, EventArgs e)
        {
            #region "check du lieu"
            if (!is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            MA_NX_DINH_KY checkMa = maBO.getMaNhanXetByMa(Sys_This_Truong.ID, tbMa.Text.Trim());
            if (checkMa != null && checkMa.ID != id)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Mã nhận xét đã tồn tại, vui lòng kiểm tra lại!');", true);
                tbMa.Focus();
                return;
            }
            #endregion
            MA_NX_DINH_KY detail = new MA_NX_DINH_KY();
            detail.ID = id.Value;
            detail = maBO.getMaNhanXetByID(detail.ID);
            if (detail == null) detail = new MA_NX_DINH_KY();
            if (detail.ID_TRUONG != Sys_This_Truong.ID)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện sửa dữ liệu này!');", true);
                return;
            }
            detail.MA = tbMa.Text.Trim();
            detail.NOI_DUNG = tbNoiDung.Text.Trim();
            detail.THU_TU = localAPI.ConvertStringToShort(tbThuTu.Text.Trim());
            detail.MA_LOAI_NX = Convert.ToInt16(rcbLoaiNX.SelectedValue);
            ResultEntity res = maBO.update(detail, Sys_User.ID);
            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
                logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật mã NXĐK " + id, Sys_User.ID, DateTime.Now);
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
    }
}