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
    public partial class CustomerDetail : AuthenticatePage
    {
        CustomerBO customerBO = new CustomerBO();
        long? id_customer;
        LocalAPI localAPI = new LocalAPI();
        LogUserBO logUserBO = new LogUserBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("id") != null)
            {
                try
                {
                    id_customer = Convert.ToInt64(Request.QueryString.Get("id"));
                }
                catch { }
            }
            if (!IsPostBack)
            {
                if (id_customer != null)
                {
                    CUSTOMER detail = new CUSTOMER();
                    detail = customerBO.getCustomerByID(id_customer.Value);
                    if (detail != null)
                    {
                        if (detail.HO_TEN != null)
                            tbTen.Text = detail.HO_TEN.ToString();
                        tbSDT.Text = detail.SDT;
                        tbEmail.Text = detail.EMAIL != null ? detail.EMAIL.ToString() : "";
                        try { rdNgaySinh.SelectedDate = detail.NGAY_SINH; }
                        catch { rdNgaySinh.SelectedDate = null; }
                        rcbGioiTinh.SelectedValue = detail.GIOI_TINH != null ? detail.GIOI_TINH.ToString() : "";
                        rcbNhom.SelectedValue = detail.ID_NHOM != null ? detail.ID_NHOM.ToString() : "";
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
            if (string.IsNullOrEmpty(tbSDT.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Số điện thoại bắt buộc nhập!');", true);
                return;
            }
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";

            string sdt = localAPI.Add84(tbSDT.Text.Trim());
            CUSTOMER customer = customerBO.checkCustomerByPhone(Sys_This_Truong.ID, sdt);
            if (customer == null)
            {
                CUSTOMER detail = new CUSTOMER();
                detail.TEN = tbTen.Text;
                detail.HO_TEN = tbTen.Text.Trim();
                string ho_dem = "", ten = "";
                localAPI.splitHoTen(tbTen.Text.Trim(), out ho_dem, out ten);
                detail.TEN = ten.Trim();
                detail.HO_DEM = ho_dem.Trim();
                detail.SDT = sdt;
                try { detail.NGAY_SINH = rdNgaySinh.SelectedDate; }
                catch { detail.NGAY_SINH = null; }
                detail.GIOI_TINH = localAPI.ConvertStringToShort(rcbGioiTinh.SelectedValue);
                detail.ID_NHOM = localAPI.ConvertStringToShort(rcbNhom.SelectedValue);
                detail.EMAIL = tbEmail.Text.Trim() == "" ? null : tbEmail.Text.Trim();
                detail.ID_TRUONG = Sys_This_Truong.ID;
                res = customerBO.insert(detail, Sys_User.ID);
                string strMsg = "";
                if (res.Res)
                {
                    CUSTOMER resCustomer = (CUSTOMER)res.ResObject;
                    logUserBO.insert(Sys_This_Truong.ID, "INSERT", "Thêm mới customer " + resCustomer.ID, Sys_User.ID, DateTime.Now);
                    tbTen.Text = "";
                    tbSDT.Text = "";
                    rcbGioiTinh.ClearSelection();
                    rdNgaySinh.SelectedDate = null;
                    tbEmail.Text = "";
                    tbSDT.Focus();
                    strMsg = "notification('success', '" + res.Msg + "');";
                }
                else
                {
                    strMsg = "notification('error', '" + res.Msg + "');";
                }
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'SĐT này đã tồn tại, vui lòng kiểm tra lại');", true);
                return;
            }
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
            CUSTOMER detail = new CUSTOMER();
            detail = customerBO.getCustomerByID(id_customer.Value);

            if (detail != null)
            {
                if (detail.ID_TRUONG != Sys_This_Truong.ID)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện sửa dữ liệu này!');", true);
                    return;
                }
                detail.HO_TEN = tbTen.Text.Trim();
                string ho_dem = "", ten = "";
                localAPI.splitHoTen(tbTen.Text.Trim(), out ho_dem, out ten);
                detail.TEN = ten.Trim();
                detail.HO_DEM = ho_dem.Trim();
                detail.SDT = localAPI.Add84(tbSDT.Text.Trim());
                try { detail.NGAY_SINH = rdNgaySinh.SelectedDate; }
                catch { detail.NGAY_SINH = null; }
                detail.GIOI_TINH = localAPI.ConvertStringToShort(rcbGioiTinh.SelectedValue);
                detail.ID_NHOM = localAPI.ConvertStringToShort(rcbNhom.SelectedValue);
                detail.EMAIL = tbEmail.Text.Trim() == "" ? null : tbEmail.Text.Trim();
                detail.ID_TRUONG = Sys_This_Truong.ID;
                res = customerBO.update(detail, Sys_User.ID);
            }

            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
                logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật customer " + id_customer.Value, Sys_User.ID, DateTime.Now);
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
    }
}