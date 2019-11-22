using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CMS.BaoCaoThongKe
{
    public partial class TimKiemHocSinh : AuthenticatePage
    {
        HocSinhBO hsBO = new HocSinhBO();
        LopBO lopBO = new LopBO();
        LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rcbTruong.DataSource = Sys_List_Truong;
                rcbTruong.DataBind();
            }
        }
        protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
        {
            if (e.Argument == "RadWindowManager1_Close")
            {
                RadGrid1.Rebind();
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            List<HocSinhEntity> lst = new List<HocSinhEntity>();
            string phone = tbSDT.Text.Trim();
            if (!string.IsNullOrEmpty(phone))
            {
                lst = hsBO.getHocSinhByTruongAndPhone(localAPI.ConvertStringTolong(rcbTruong.SelectedValue), Convert.ToInt16(Sys_Ma_Nam_hoc), tbSDT.Text.Trim());
            }
            RadGrid1.DataSource = lst;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
        protected void btTimKiem_Click(object sender, EventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                //GridDataItem item = e.Item as GridDataItem;
                //if (!string.IsNullOrEmpty(item["SDT_NHAN_TIN2"].Text) && item["SDT_NHAN_TIN2"].Text != "&nbsp;")
                //    item["SDT_BM"].Text = item["SDT_NHAN_TIN"].Text + "; " + item["SDT_NHAN_TIN2"].Text;
                //else item["SDT_BM"].Text = item["SDT_NHAN_TIN"].Text;
            }
        }

        protected void rcbTruong_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
    }
}