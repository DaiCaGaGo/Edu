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

namespace CMS.Report
{
    public partial class PhieuThuTienHocPhiHocSinh : AuthenticatePage
    {
        HocSinhBO hsBO = new HocSinhBO();
        HocPhiPhieuThuHocSinhBO phieuThuHocSinhBO = new HocPhiPhieuThuHocSinhBO();
        LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            btTimKiem.Visible = is_access(SYS_Type_Access.THEM);
            btTimKiem.Visible = is_access(SYS_Type_Access.XEM);
            if (!IsPostBack)
            {
                objKhoi.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                objKhoi.SelectParameters.Add("maLoaiLopGDTX", DbType.Int16, Sys_This_Lop_GDTX == null ? "" : Sys_This_Lop_GDTX.ToString());
                rcbKhoi.DataBind();
                objLop.SelectParameters.Add("ma_cap_hoc", Sys_This_Cap_Hoc);
                objLop.SelectParameters.Add("idTruong", Sys_This_Truong.ID.ToString());
                objLop.SelectParameters.Add("maNamHoc", Sys_Ma_Nam_hoc.ToString());
                rcbLop.DataBind();
                objDotThu.SelectParameters.Add("id_truong", Sys_This_Truong.ID.ToString());
                objDotThu.SelectParameters.Add("id_nam_hoc", Sys_Ma_Nam_hoc.ToString());
                rcbDotThu.DataBind();
            }
        }
        protected void btTimKiem_Click(object sender, EventArgs e)
        {
            #region check value
            if (!is_access(SYS_Type_Access.XEM))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Bạn không có quyền truy cập chức năng này!');", true);
                return;
            }
            if (string.IsNullOrEmpty(rcbLop.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Bạn chưa chọn lớp học!');", true);
                return;
            }
            if (string.IsNullOrEmpty(rcbDotThu.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Bạn chưa chọn đợt thu!');", true);
                return;
            }
            #endregion
            List<BienLaiThuTienHSEntity> lst = phieuThuHocSinhBO.getPhieuThuHocSinhByLopAndDotThu(Sys_This_Truong.ID, Convert.ToInt16(Sys_Ma_Nam_hoc), Convert.ToInt16(Sys_Hoc_Ky), Convert.ToInt16(rcbKhoi.SelectedValue), Convert.ToInt64(rcbLop.SelectedValue), Convert.ToInt64(rcbDotThu.SelectedValue));
            string ReportID = "PhieuThuHocPhiHS";
            Session["ReportPhieuThuHocPhiHS" + ReportID] = lst;
            Response.Redirect("~/Report/ReportView.aspx?ma=" + ReportID, true);


        }
        protected void rcbKhoi_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbLop.ClearSelection();
            rcbLop.Text = String.Empty;
            rcbLop.DataBind();
            rcbDotThu.ClearSelection();
            rcbDotThu.Text = string.Empty;
            rcbDotThu.DataBind();
        }
        protected void rcbLop_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbDotThu.ClearSelection();
            rcbDotThu.Text = string.Empty;
            rcbDotThu.DataBind();
        }
    }
}