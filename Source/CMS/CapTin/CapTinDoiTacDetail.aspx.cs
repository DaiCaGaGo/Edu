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

namespace CMS.CapTin
{
    public partial class CapTinDoiTacDetail : AuthenticatePage
    {
        long? id;
        CapTinDoiTacBO capTinDoiTacBO = new CapTinDoiTacBO();
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
                    CAP_TIN_DOI_TAC detail = new CAP_TIN_DOI_TAC();
                    detail = capTinDoiTacBO.getCapTinDoiTacByID(id.Value);
                    if (detail != null)
                    {
                        rcbDaiLy.SelectedValue = detail.ID_DOI_TAC.ToString();
                        tbSoTinCap.Text = detail.SO_TIN_CAP.ToString();
                        btAdd.Visible = false;
                    }
                    else
                    {
                        btAdd.Visible = is_access(SYS_Type_Access.THEM);
                    }
                }
                else
                {
                    btAdd.Visible = is_access(SYS_Type_Access.THEM);
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
            int success = 0;
            CAP_TIN_DOI_TAC detail = new CAP_TIN_DOI_TAC();
            detail.ID_DOI_TAC = Convert.ToInt16(rcbDaiLy.SelectedValue);
            detail.SO_TIN_CAP = Convert.ToInt64(tbSoTinCap.Text);
            #region "chec quỹ tin đối tác"
            DoiTacBO doiTacBO = new DoiTacBO(); 
            DOI_TAC doiTac = new DOI_TAC();
            doiTac = doiTacBO.getDoiTacByID(Convert.ToInt16(rcbDaiLy.SelectedValue));
            TruongBO truongBO = new TruongBO();
            List<TruongEntity> lstTruong = new List<TruongEntity>();
            lstTruong = truongBO.getTruongByDoiTac(Convert.ToInt16(rcbDaiLy.SelectedValue));
            long tong_tin_da_cap = 0;
            if (lstTruong.Count > 0)
            {
                for (int i = 0; i < lstTruong.Count; i++)
                {
                    tong_tin_da_cap += lstTruong[i].TONG_TIN_CAP != null ? lstTruong[i].TONG_TIN_CAP.Value : 0;
                }
            }
            if (doiTac.TONG_TIN_CAP + Convert.ToInt64(tbSoTinCap.Text) < tong_tin_da_cap)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Số tin cấp chưa hợp lệ. Vui lòng kiểm tra lại!');", true);
                return;
            }
            else
            {
                res = capTinDoiTacBO.insert(detail, Sys_User.ID);
                if (res.Res)
                {
                    success++;
                    logUserBO.insert(null, "CẤP TIN", "Cấp tin đại lý " + detail.ID_DOI_TAC, Sys_User.ID, DateTime.Now);
                }
            }
            #endregion

            string strMsg = "";
            if (success > 0)
            {
                strMsg = "notification('success', 'Cấp tin thành công!');";
            }
            else
            {
                strMsg = "notification('error', 'Bạn không thể cấp tin. Vui lòng liên hệ với quản trị viên!');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
    }
}