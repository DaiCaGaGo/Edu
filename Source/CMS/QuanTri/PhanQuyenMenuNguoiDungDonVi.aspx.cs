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
using Telerik.Web.UI;

namespace CMS.QuanTri
{
    public partial class PhanQuyenMenuNguoiDungDonVi : AuthenticatePage
    {
        long id_user_pq;
        public NguoiDungMenuBO ndBO = new NguoiDungMenuBO();
        private LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("id_user") != null)
            {
                try
                {
                    id_user_pq = Convert.ToInt64(Request.QueryString.Get("id_user"));
                }
                catch { }
            }
            btEdit.Visible = is_access(SYS_Type_Access.THEM) || is_access(SYS_Type_Access.SUA);
            if (!IsPostBack)
            {
                rcbCapHoc.SelectedValue = Sys_This_Cap_Hoc;
                RadTreeList1.ExpandAllItems();
            }
        }
        protected void btEdit_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.EXPORT) && !is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            int success = 0, error = 0;
            foreach (TreeListDataItem item in RadTreeList1.Items)
            {
                var lm = new NguoiDungMenuEntity();

                CheckBox cbXem = (CheckBox)item.FindControl("cbXem");
                CheckBox cbThem = (CheckBox)item.FindControl("cbThem");
                CheckBox cbSua = (CheckBox)item.FindControl("cbSua");
                CheckBox cbXoa = (CheckBox)item.FindControl("cbXoa");
                CheckBox cbGuiSMS = (CheckBox)item.FindControl("cbGuiSMS");
                CheckBox cbXemChiTiet = (CheckBox)item.FindControl("cbXemChiTiet");
                CheckBox cbXuatExcel = (CheckBox)item.FindControl("cbXuatExcel");

                HiddenField hdIdNguoiDungMenu = (HiddenField)item.FindControl("hdIdNguoiDungMenu");
                HiddenField hdXem = (HiddenField)item.FindControl("hdXem");
                HiddenField hdThem = (HiddenField)item.FindControl("hdThem");
                HiddenField hdSua = (HiddenField)item.FindControl("hdSua");
                HiddenField hdXoa = (HiddenField)item.FindControl("hdXoa");
                HiddenField hdGuiSMS = (HiddenField)item.FindControl("hdGuiSMS");
                HiddenField hdXemChiTiet = (HiddenField)item.FindControl("hdXemChiTiet");
                HiddenField hdXuatExcel = (HiddenField)item.FindControl("hdXuatExcel");
                lm.ID = localAPI.ConvertStringTolong(item.GetDataKeyValue("ID").ToString()).Value;
                lm.ID_NGUOI_DUNG_MENU = localAPI.ConvertStringTolong(hdIdNguoiDungMenu.Value);
                lm.IS_XEM = cbXem.Checked ? 1 : 0;
                lm.IS_THEM = cbThem.Checked ? 1 : 0;
                lm.IS_SUA = cbSua.Checked ? 1 : 0;
                lm.IS_XOA = cbXoa.Checked ? 1 : 0;
                lm.IS_SEND_SMS = cbGuiSMS.Checked ? 1 : 0;
                lm.IS_VIEW_INFOR = cbXemChiTiet.Checked ? 1 : 0;
                lm.IS_EXPORT = cbXuatExcel.Checked ? 1 : 0;

                int? IS_XEM_old = localAPI.ConvertStringToint(hdXem.Value); IS_XEM_old = IS_XEM_old == null ? 0 : IS_XEM_old;
                int? IS_THEM_old = localAPI.ConvertStringToint(hdThem.Value); IS_THEM_old = IS_THEM_old == null ? 0 : IS_THEM_old;
                int? IS_SUA_old = localAPI.ConvertStringToint(hdSua.Value); IS_SUA_old = IS_SUA_old == null ? 0 : IS_SUA_old;
                int? IS_XOA_old = localAPI.ConvertStringToint(hdXoa.Value); IS_XOA_old = IS_XOA_old == null ? 0 : IS_XOA_old;
                int? IS_SEND_SMS_old = localAPI.ConvertStringToint(hdGuiSMS.Value); IS_SEND_SMS_old = IS_SEND_SMS_old == null ? 0 : IS_SEND_SMS_old;
                int? IS_VIEW_INFOR_old = localAPI.ConvertStringToint(hdXemChiTiet.Value); IS_VIEW_INFOR_old = IS_VIEW_INFOR_old == null ? 0 : IS_VIEW_INFOR_old;
                int? IS_EXPORT_old = localAPI.ConvertStringToint(hdXuatExcel.Value); IS_EXPORT_old = IS_EXPORT_old == null ? 0 : IS_EXPORT_old;
                if (IS_XEM_old != lm.IS_XEM || IS_THEM_old != lm.IS_THEM || IS_SUA_old != lm.IS_SUA || IS_XOA_old != lm.IS_XOA || IS_SEND_SMS_old != lm.IS_SEND_SMS || IS_VIEW_INFOR_old != lm.IS_VIEW_INFOR || IS_EXPORT_old != lm.IS_EXPORT)
                {
                    ResultEntity res = new ResultEntity();
                    if (lm.ID_NGUOI_DUNG_MENU != null && lm.ID_NGUOI_DUNG_MENU > 0)
                    {
                        var detail = new NGUOI_DUNG_MENU();
                        detail.ID = lm.ID_NGUOI_DUNG_MENU.Value;
                        if (cbXem.Checked || cbThem.Checked || cbSua.Checked || cbXoa.Checked || cbXemChiTiet.Checked || cbXuatExcel.Checked)
                            detail.IS_XEM = true;
                        detail.IS_THEM = cbThem.Checked;
                        detail.IS_SUA = cbSua.Checked;
                        detail.IS_XOA = cbXoa.Checked;
                        detail.IS_SEND_SMS = cbGuiSMS.Checked;
                        detail.IS_VIEW_INFOR = cbXemChiTiet.Checked;
                        detail.IS_EXPORT = cbXuatExcel.Checked;
                        res = ndBO.update(detail, Sys_User.ID);
                    }
                    else
                    {
                        var detail = new NGUOI_DUNG_MENU();
                        detail.ID_MENU = lm.ID;
                        detail.ID_TRUONG = Sys_This_Truong.ID;
                        detail.ID_NGUOI_DUNG = id_user_pq;
                        if (cbXem.Checked || cbThem.Checked || cbSua.Checked || cbXoa.Checked || cbXemChiTiet.Checked || cbXuatExcel.Checked)
                            detail.IS_XEM = true;
                        detail.IS_THEM = cbThem.Checked;
                        detail.IS_SUA = cbSua.Checked;
                        detail.IS_XOA = cbXoa.Checked;
                        detail.IS_SEND_SMS = cbGuiSMS.Checked;
                        detail.IS_VIEW_INFOR = cbXemChiTiet.Checked;
                        detail.IS_EXPORT = cbXuatExcel.Checked;
                        res = ndBO.insert(detail, Sys_User.ID);
                    }
                    if (res.Res)
                        success++;
                    else
                        error++;
                }

            }
            string strMsg = "";
            if (error > 0)
            {
                strMsg = "notification('error', 'Có " + error + " bản ghi lỗi. Liên hệ với quản trị viên');";
            }
            if (success > 0)
            {
                //strMsg += " notification('success', 'Có " + success + " bản ghi lưu thành công.');";
                strMsg += " notification('success', 'Cấp quyền thành công.');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadTreeList1.Rebind();
            RadTreeList1.ExpandAllItems();

        }
        protected void rcbCapHoc_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadTreeList1.Rebind();
            RadTreeList1.ExpandAllItems();

        }
        protected void RadTreeList1_NeedDataSource(object sender, Telerik.Web.UI.TreeListNeedDataSourceEventArgs e)
        {
            RadTreeList1.DataSource = ndBO.getQuyenByNguoiDungTruongCapHoc(id_user_pq, Sys_This_Truong.ID, rcbCapHoc.SelectedValue);
        }
        protected void RadTreeList1_ItemCreated(object sender, TreeListItemCreatedEventArgs e)
        {
            if (e.Item is TreeListDataItem)
            {
                var dataItem = (TreeListDataItem)e.Item;
                var expandCell = dataItem.Cells[dataItem.HierarchyIndex.NestedLevel];
                if (expandCell.Controls.Count > 0)
                {
                    expandCell.Controls[0].Visible = false;
                }
            }
        }
    }
}