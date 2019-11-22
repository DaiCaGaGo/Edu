using CMS.XuLy;
using OneEduDataAccess.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CMS.MamNon_DanhMuc
{
    public partial class ThucPham : AuthenticatePage
    {
        ThucPhamBO thucPhamBO = new ThucPhamBO();
        NhomThucPhamBO nhomThucPhamBO = new NhomThucPhamBO();
        DonViTinhBO donViTinhBO = new DonViTinhBO();
        LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {

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
            RadGrid1.DataSource = thucPhamBO.getThucPham(localAPI.ConvertStringToShort(rcbNhomThucPham.SelectedValue), tbTen.Text.Trim());
        }
        protected void rcbNhomThucPham_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void btTimKiem_Click(object sender, EventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void btDeleteChon_Click(object sender, EventArgs e)
        {
            int success = 0, error = 0; string lst_id = string.Empty;
            foreach (GridDataItem row in RadGrid1.SelectedItems)
            {
                try
                {
                    short id = Convert.ToInt16(row.GetDataKeyValue("ID").ToString());
                    string ten = row["TEN"].Text;
                    try
                    {
                        ResultEntity res = thucPhamBO.delete(id, Sys_User.ID, true);
                        lst_id += id + ":" + ten + ", ";
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
                short? id_nhom = localAPI.ConvertStringToShort(item["ID_NHOM_THUC_PHAM"].Text);
                if (id_nhom != null)
                    item["TEN_NHOM"].Text = nhomThucPhamBO.getNhomThucPham("").FirstOrDefault(x => x.ID == id_nhom.Value).TEN;
                short? don_vi_tinh = localAPI.ConvertStringToShort(item["DON_VI_TINH"].Text);
                if (don_vi_tinh != null)
                    item["DON_VI_TINH"].Text = donViTinhBO.getDonViTinh().FirstOrDefault(x => x.ID == don_vi_tinh.Value).TEN;
            }
        }
    }
}