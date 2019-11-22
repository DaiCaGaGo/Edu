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

namespace CMS.DoiTac
{
    public partial class DoiTacDetail : AuthenticatePage
    {
        DoiTacBO doitacBO = new DoiTacBO();
        short? iddoitac;
        LocalAPI localAPI = new LocalAPI();
        LogUserBO logUserBO = new LogUserBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("iddoitac") != null)
            {
                try
                {
                    iddoitac = Convert.ToInt16(Request.QueryString.Get("iddoitac"));
                }
                catch { }
            }
            if (!IsPostBack)
            {
                if (iddoitac != null)
                {
                    DOI_TAC detail = new DOI_TAC();
                    detail = doitacBO.getDoiTacByID(iddoitac.Value);
                    if (detail != null)
                    {
                        if (detail.TEN != null)
                            tbTen.Text = detail.TEN.ToString();
                        tbSoDienThoai.Text = detail.SDT != null ? detail.SDT : "";
                        tbDiaChi.Text = detail.DIA_CHI != null ? detail.DIA_CHI.ToString() : "";
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
            DOI_TAC detail = new DOI_TAC();
            detail.TEN = tbTen.Text;
            detail.SDT = tbSoDienThoai.Text;
            detail.DIA_CHI = tbDiaChi.Text;
            if (detail.TEN != null)
            {
                res = doitacBO.insert(detail.TEN, detail.DIA_CHI, detail.SDT, Sys_User.ID, false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Nhập tên tổ giáo viên!');", true);
            }
            string strMsg = "";
            if (res.Res)
            {
                tbTen.Text = "";
                tbSoDienThoai.Text = "";
                tbDiaChi.Text = "";
                strMsg = "notification('success', '" + res.Msg + "');";
                DOI_TAC resDoiTac = (DOI_TAC)res.ResObject;
                logUserBO.insert(Sys_This_Truong.ID, "INSERT", "Thêm mới đối tác " + resDoiTac.ID, Sys_User.ID, DateTime.Now);
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
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
            DOI_TAC detail = new DOI_TAC();
            detail.TEN = tbTen.Text;
            detail.SDT = tbSoDienThoai.Text;
            detail.DIA_CHI = tbDiaChi.Text;
            res = doitacBO.update(iddoitac.Value, detail.TEN, detail.DIA_CHI, detail.SDT, Sys_User.ID, false);
            if (detail != null)
            {
                detail.TEN = tbTen.Text.Trim();
            }
            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
                logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật đối tác " + iddoitac, Sys_User.ID, DateTime.Now);
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
    }
}