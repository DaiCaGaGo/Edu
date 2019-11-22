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
    public partial class ReportBangTheoDoiHS_LanguageLink_TH : AuthenticatePage
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
                int count_HS = lst.Count;
                if (count_HS > 0 && count_HS < 31)
                {
                    for (int i = 1; i <= 31 - count_HS; i++)
                    {
                        BangTheoDoiHocSinhTHEntity detail = new BangTheoDoiHocSinhTHEntity();
                        detail.STT = count_HS + i;
                        detail.GVCN_SDT = lst[0].GVCN_SDT;
                        detail.GVCN_TEN = lst[0].GVCN_TEN;
                        detail.HO_TEN = "";
                        detail.NHAN_XET_HANG_NGAY = "";
                        detail.TEN_LOP = lst[0].TEN_LOP;
                        detail.TEN_TRUONG = lst[0].TEN_TRUONG;
                        lst.Add(detail);
                    }
                }
                string ReportID = "BTDHS_LL_TH";
                Session["DatareportBTDHS_LL_TH" + ReportID] = lst;
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