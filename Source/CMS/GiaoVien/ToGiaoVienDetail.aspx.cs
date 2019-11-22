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

namespace CMS.GiaoVien
{
    public partial class ToGiaoVienDetail : AuthenticatePage
    {
        DmToGiaoVienBO tgvBO = new DmToGiaoVienBO();
        long? ma_tgv;
        LocalAPI localAPI = new LocalAPI();
        LogUserBO logUserBO = new LogUserBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("ma_tgv") != null)
            {
                try
                {
                    ma_tgv = Convert.ToInt16(Request.QueryString.Get("ma_tgv"));
                }
                catch { }
            }
            if (!IsPostBack)
            {
                if (ma_tgv != null)
                {
                    TO_GIAO_VIEN detail = new TO_GIAO_VIEN();
                    detail = tgvBO.getToGiaoVienByID(ma_tgv.Value);
                    if (detail != null)
                    {
                        if (detail.TEN != null)
                            tbTen.Text = detail.TEN.ToString();
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
        protected void btAdd_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.THEM))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            TO_GIAO_VIEN detail = new TO_GIAO_VIEN();
            detail.TEN = tbTen.Text;
            detail.THU_TU = localAPI.ConvertStringToShort(tbThuTu.Text);
            detail.ID_TRUONG = Sys_This_Truong.ID;
            if (detail.TEN != null)
            {
                res = tgvBO.insert(detail, Sys_User.ID);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Nhập tên tổ giáo viên!');", true);
            }
            string strMsg = "";
            if (res.Res)
            {
                TO_GIAO_VIEN resTo = (TO_GIAO_VIEN)res.ResObject;
                tbTen.Text = "";
                logUserBO.insert(Sys_This_Truong.ID, "INSERT", "Thêm mới tổ gv " + resTo.ID, Sys_User.ID, DateTime.Now);
                getMaxThuTu();
                strMsg = "notification('success', '" + res.Msg + "');";
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
        protected void getMaxThuTu()
        {
            short? max_thu_tu = tgvBO.getMaxThuTuByTruong(Sys_This_Truong.ID);
            if (max_thu_tu != null)
                tbThuTu.Text = Convert.ToString(Convert.ToInt16(max_thu_tu) + 1);
            else tbThuTu.Text = "1";
        }
        protected void btEdit_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            TO_GIAO_VIEN detail = new TO_GIAO_VIEN();
            detail = tgvBO.getToGiaoVienByID(ma_tgv.Value);

            if (detail != null)
            {
                if (detail.ID_TRUONG != Sys_This_Truong.ID)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện sửa dữ liệu này!');", true);
                    return;
                }
                detail.TEN = tbTen.Text.Trim();
                detail.THU_TU = localAPI.ConvertStringToShort(tbThuTu.Text);
                res = tgvBO.update(detail, Sys_User.ID, ma_tgv.Value);
            }

            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
                logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật tổ gv " + ma_tgv, Sys_User.ID, DateTime.Now);
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
    }
}