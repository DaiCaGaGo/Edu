using CMS.XuLy;
using OneEduDataAccess.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CMS.DinhDuong
{
    public partial class KhoThucPham : AuthenticatePage
    {
        KhoThucPhamBO khoThucPhamBO = new KhoThucPhamBO();
        DonViTinhBO donViTinhBO = new DonViTinhBO();
        LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
        }
        protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
        {
            if (e.Argument == "RadWindowManager1_Close")
            {
                Session["PhieuNhapKhoChiTiet" + Sys_User.ID] = null;
                Session["PhieuXuatKhoChiTiet" + Sys_User.ID] = null;
                RadGrid1.Rebind();
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = khoThucPhamBO.getKhoThucPham(Sys_This_Truong.ID, localAPI.ConvertStringToShort(rcbNhomThucPham.SelectedValue), tbTen.Text.Trim());
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
            int success = 0, error = 0;
            foreach (GridDataItem row in RadGrid1.SelectedItems)
            {
                try
                {
                    long id = Convert.ToInt64(row.GetDataKeyValue("ID").ToString());
                    try
                    {
                        ResultEntity res = khoThucPhamBO.delete(id, Sys_User.ID, true);
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
                short? don_vi_tinh = localAPI.ConvertStringToShort(item["DON_VI_TINH"].Text);
                decimal? so_luong = localAPI.ConvertStringToDecimal(item["SO_LUONG"].Text);
                item["LUONG_TRONG_KHO"].Text = (so_luong != null ? so_luong.ToString() : "") + " " + (don_vi_tinh != null ? donViTinhBO.getDonViTinh().FirstOrDefault(x => x.ID == don_vi_tinh.Value).TEN : "");
            }
        }
    }
}