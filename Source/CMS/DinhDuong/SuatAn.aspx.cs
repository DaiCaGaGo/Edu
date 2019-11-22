using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CMS.DinhDuong
{
    public partial class SuatAn : AuthenticatePage
    {
        SuatAnBO suatAnBO = new SuatAnBO();
        KhoiBO khoiBO = new KhoiBO();
        DMBuaAnBO buaAnBO = new DMBuaAnBO();
        ThucDonBO thucDonBO = new ThucDonBO();
        LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            btAddNew.Visible = is_access(SYS_Type_Access.THEM);
            btDeleteChon.Visible = is_access(SYS_Type_Access.XOA);
            if (!IsPostBack)
            {
                objKhoi.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                objKhoi.SelectParameters.Add("maLoaiLopGDTX", DbType.Int16, Sys_This_Lop_GDTX == null ? "" : Sys_This_Lop_GDTX.ToString());
                rcbKhoi.DataBind();
                objBuaAn.SelectParameters.Add("id_truong", Sys_This_Truong.ID.ToString());
                rcbBuaAn.DataBind();
                rdNgay.SelectedDate = DateTime.Now;
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
            RadGrid1.DataSource = suatAnBO.getSuatAn(Sys_This_Truong.ID, localAPI.ConvertStringToShort(rcbKhoi.SelectedValue), Convert.ToInt16(Sys_Ma_Nam_hoc), null, localAPI.ConvertStringTolong(rcbBuaAn.SelectedValue), null, Convert.ToDateTime(rdNgay.SelectedDate));
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
        protected void btDeleteChon_Click(object sender, EventArgs e)
        {
            int success = 0, error = 0;
            foreach (GridDataItem row in RadGrid1.SelectedItems)
            {
                try
                {
                    short id = Convert.ToInt16(row.GetDataKeyValue("ID").ToString());
                    try
                    {
                        ResultEntity res = suatAnBO.delete(id, Sys_User.ID, true);
                        if (res.Res)
                            success++;
                        else
                            error++;
                    }
                    catch
                    {
                        error++;
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
                strMsg = "notification('error', 'Có " + error + " bản ghi chưa được xóa. Liên hệ với quản trị viên');";
            }
            if (success > 0)
            {
                strMsg += " notification('success', 'Có " + success + " bản ghi được xóa.');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
        }
        protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                short? id_khoi = localAPI.ConvertStringToShort(item["ID_KHOI"].Text);
                if (id_khoi != null)
                    item["TEN_KHOI"].Text = khoiBO.getKhoi(Sys_This_Cap_Hoc).FirstOrDefault(x => x.MA == id_khoi).TEN;
                long? id_bua_an = localAPI.ConvertStringTolong(item["ID_BUA_AN"].Text);
                if (id_bua_an != null)
                    item["TEN_BUA_AN"].Text = buaAnBO.getBuaAnByTruongKhoi(Sys_This_Truong.ID, id_khoi).FirstOrDefault(x => x.ID == id_bua_an).TEN;
                long? id_thuc_don = localAPI.ConvertStringTolong(item["ID_THUC_DON"].Text);
                if (id_thuc_don != null)
                    item["TEN_THUC_DON"].Text = thucDonBO.getThucDonByTruongKhoiNhomTuoi(Sys_This_Truong.ID, id_khoi, null).FirstOrDefault(x => x.ID == id_thuc_don).TEN;
            }
        }
        protected void rcbKhoi_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbBuaAn.ClearSelection();
            rcbBuaAn.Text = string.Empty;
            rcbBuaAn.DataBind();
            RadGrid1.Rebind();
        }
        protected void rdNgay_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void rcbBuaAn_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
    }
}