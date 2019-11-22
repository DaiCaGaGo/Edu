using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CMS.HocSinh
{
    public partial class ChuyenLop : AuthenticatePage
    {
        LocalAPI localAPI = new LocalAPI();
        LopBO lopBO = new LopBO();
        HocSinhBO hocSinhBO = new HocSinhBO();
        LogUserBO logUserBO = new LogUserBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            if (!IsPostBack)
            {
                objTuLop.SelectParameters.Add("ma_cap_hoc", Sys_This_Cap_Hoc);
                objTuLop.SelectParameters.Add("idTruong", Sys_This_Truong.ID.ToString());
                objTuLop.SelectParameters.Add("maNamHoc", Sys_Ma_Nam_hoc.ToString());
                objTuLop.SelectParameters.Add("maKhoi", null);
                rcbTuLop.DataBind();

                objDenLop.SelectParameters.Add("ma_cap_hoc", Sys_This_Cap_Hoc);
                objDenLop.SelectParameters.Add("idTruong", Sys_This_Truong.ID.ToString());
                objDenLop.SelectParameters.Add("maNamHoc", Sys_Ma_Nam_hoc.ToString());
                objDenLop.SelectParameters.Add("maKhoi", null);
                rcbDenLop.DataBind();
            }
        }
        protected void rcbTuLop_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbDenLop.ClearSelection();
            rcbDenLop.Text = String.Empty;
            rcbDenLop.DataBind();
            RadGrid1.Rebind();
            RadGrid2.Rebind();
        }
        protected void rcbDenLop_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid2.Rebind();
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = hocSinhBO.getHocSinhByKhoiLop(Sys_This_Truong.ID, null, localAPI.ConvertStringTolong(rcbTuLop.SelectedValue), Convert.ToInt16(Sys_Ma_Nam_hoc), Sys_This_Cap_Hoc);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
        protected void RadGrid2_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid2.DataSource = hocSinhBO.getHocSinhByKhoiLop(Sys_This_Truong.ID, null, localAPI.ConvertStringTolong(rcbDenLop.SelectedValue), Convert.ToInt16(Sys_Ma_Nam_hoc), Sys_This_Cap_Hoc);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
        protected void btnChuyenDi_Click(object sender, ImageClickEventArgs e)
        {
            int success = 0, error = 0;
            foreach (GridDataItem row in RadGrid1.SelectedItems)
            {
                try
                {
                    long id_hs = Convert.ToInt64(row.GetDataKeyValue("ID").ToString());
                    if (rcbDenLop.SelectedValue != "")
                    {
                        ResultEntity res = hocSinhBO.updateChuyenLop(id_hs, Convert.ToInt64(rcbDenLop.SelectedValue), Sys_User.ID);
                        if (res.Res)
                        {
                            success++;
                            logUserBO.insert(Sys_This_Truong.ID, "CHUYỂN LỚP", "Chuyển học sinh " + id_hs + " từ lớp " + rcbTuLop.SelectedValue + " đến lớp " + rcbDenLop.SelectedValue, Sys_User.ID, DateTime.Now);
                        }
                        else
                            error++;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Bạn vui lòng chọn lớp chuyển đến cho học sinh!');", true);
                        return;
                    }
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Lỗi hệ thống. Bạn vui lòng kiểm tra lại');", true);
                }
            }
            string strMsg = "";
            if (error > 0)
            {
                strMsg = "notification('error', 'Có " + error + " học sinh chưa được chuyển lớp. Liên hệ với quản trị viên');";
            }
            if (success > 0)
            {
                strMsg += " notification('success', 'Có " + success + " học sinh chuyển lớp thành công.');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
            RadGrid2.Rebind();
        }
        protected void btnChuyenDen_Click(object sender, EventArgs e)
        {
            int success = 0, error = 0;
            foreach (GridDataItem row in RadGrid2.SelectedItems)
            {
                try
                {
                    long id_hs = Convert.ToInt64(row.GetDataKeyValue("ID").ToString());
                    if (rcbTuLop.SelectedValue != "")
                    {
                        ResultEntity res = hocSinhBO.updateChuyenLop(id_hs, Convert.ToInt64(rcbTuLop.SelectedValue), Sys_User.ID);
                        if (res.Res)
                        {
                            success++;
                            logUserBO.insert(Sys_This_Truong.ID, "CHUYỂN LỚP", "Chuyển học sinh " + id_hs + " từ lớp " + rcbDenLop.SelectedValue + " đến lớp " + rcbTuLop.SelectedValue, Sys_User.ID, DateTime.Now);
                        }
                        else
                            error++;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Bạn vui lòng chọn lớp chuyển đến cho học sinh!');", true);
                        return;
                    }
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Lỗi hệ thống. Bạn vui lòng kiểm tra lại');", true);
                }
            }
            string strMsg = "";
            if (error > 0)
            {
                strMsg = "notification('error', 'Có " + error + " học sinh chưa được chuyển lớp. Liên hệ với quản trị viên');";
            }
            if (success > 0)
            {
                strMsg += " notification('success', 'Có " + success + " học sinh chuyển lớp thành công.');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
            RadGrid2.Rebind();
        }
    }
}