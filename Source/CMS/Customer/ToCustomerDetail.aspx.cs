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

namespace CMS.Customer
{
    public partial class ToCustomerDetail : AuthenticatePage
    {
        CustomerToBO customerToBO = new CustomerToBO();
        short? id_to;
        LocalAPI localAPI = new LocalAPI();
        LogUserBO logUserBO = new LogUserBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("id") != null)
            {
                try
                {
                    id_to = Convert.ToInt16(Request.QueryString.Get("id"));
                }
                catch { }
            }
            if (!IsPostBack)
            {
                if (id_to != null)
                {
                    CUSTOMER_TO detail = new CUSTOMER_TO();
                    detail = customerToBO.getToCustomerByID(id_to.Value);
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
            CUSTOMER_TO detail = new CUSTOMER_TO();
            detail.TEN = tbTen.Text;
            detail.THU_TU = localAPI.ConvertStringToShort(tbThuTu.Text);
            detail.ID_TRUONG = Sys_This_Truong.ID;
            if (detail.TEN != null)
            {
                res = customerToBO.insert(detail, Sys_User.ID);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Nhập tên tổ giáo viên!');", true);
            }
            string strMsg = "";
            if (res.Res)
            {
                CUSTOMER_TO resCUS = (CUSTOMER_TO)res.ResObject;
                logUserBO.insert(Sys_This_Truong.ID, "INSERT", "Thêm tổ customer " + resCUS.ID, Sys_User.ID, DateTime.Now);
                tbTen.Text = "";
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
            short? max_thu_tu = customerToBO.getMaxThuTuByTruong(Sys_This_Truong.ID);
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
            CUSTOMER_TO detail = new CUSTOMER_TO();
            detail = customerToBO.getToCustomerByID(id_to.Value);

            if (detail != null)
            {
                if (detail.ID_TRUONG != Sys_This_Truong.ID)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện sửa dữ liệu này!');", true);
                    return;
                }
                detail.TEN = tbTen.Text.Trim();
                detail.THU_TU = localAPI.ConvertStringToShort(tbThuTu.Text);
                res = customerToBO.update(detail, Sys_User.ID, id_to.Value);
            }

            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
                logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật tổ customer " + id_to, Sys_User.ID, DateTime.Now);
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
    }
}