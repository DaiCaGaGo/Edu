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
    public partial class ReportBangTheoDoiHS_NguyenPhongSac : AuthenticatePage
    {
        private HocSinhBO hsBO = new HocSinhBO();
        LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {

            checkChonTruong();
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
            }
        }
        protected void btTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                List<BangTheoDoiHocSinhTHEntity> lst = hsBO.getBangTheoDoiHocSinh(Sys_This_Truong.ID, Convert.ToInt16(Sys_Ma_Nam_hoc), Convert.ToInt16(Sys_Hoc_Ky), localAPI.ConvertStringToShort(rcbKhoi.SelectedValue), localAPI.ConvertStringToShort(rcbLop.SelectedValue), Sys_This_Cap_Hoc, Sys_This_Truong.TEN);
                string ReportID = "BTDHS_THCS_NguyenPhongSac";
                Session["DatareportBTDHS_THCS_NguyenPhongSac" + ReportID] = lst;
                Response.Redirect("~/Report/ReportView.aspx?ma=" + ReportID, true);
            }
            catch (Exception ex) { }
        }
        protected void rcbKhoi_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbLop.ClearSelection();
            rcbLop.Text = String.Empty;
            rcbLop.DataBind();
        }
    }
}