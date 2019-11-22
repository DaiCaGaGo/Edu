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

namespace CMS.Menu
{
    public partial class Menu : AuthenticatePage
    {
        public MenuBO bo = new MenuBO();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void RadTreeList1_NeedDataSource(object sender, Telerik.Web.UI.TreeListNeedDataSourceEventArgs e)
        {
            bool is_sys = false;
            if (string.IsNullOrEmpty(rcbCapHoc.SelectedValue)) is_sys = true;
            RadTreeList1.DataSource = bo.getMenu(rcbCapHoc.SelectedValue, false, 0, "", is_sys);
            TreeListColumn editcommandcolumn = RadTreeList1.Columns.FirstOrDefault(x => x.UniqueName == "EditCommandColumn");
            TreeListColumn addcommandcolumn = RadTreeList1.Columns.FirstOrDefault(x => x.UniqueName == "InsertCommandColumn");
            TreeListColumn deletecommandcolumn = RadTreeList1.Columns.FirstOrDefault(x => x.UniqueName == "DeleteCommandColumn");
            if (editcommandcolumn != null)
                editcommandcolumn.Visible = (is_access(SYS_Type_Access.XEM) || is_access(SYS_Type_Access.SUA));
            if (addcommandcolumn != null)
                addcommandcolumn.Visible = is_access(SYS_Type_Access.THEM);
            if (deletecommandcolumn != null)
                deletecommandcolumn.Visible = is_access(SYS_Type_Access.XOA);
        }
        protected void RadTreeList1_UpdateCommand(object sender, Telerik.Web.UI.TreeListCommandEventArgs e)
        {
            try
            {
                var item = e.Item as TreeListEditFormItem;
                var detail = new MENU();
                detail.ID = Convert.ToInt64(item.ParentItem.GetDataKeyValue("ID"));
                detail = bo.getMENUById(detail.ID);
                if (detail != null)
                {
                    detail.TEN = (item.FindControl("tbTen") as TextBox).Text;
                    detail.TEN_EG = (item.FindControl("tbTenEg") as TextBox).Text;
                    detail.URL = (item.FindControl("tbUrrl") as TextBox).Text;
                    detail.ICON_CSS_CLASS = (item.FindControl("tbIconClass") as TextBox).Text;
                    detail.THU_TU = (item.FindControl("tbThuTu") as TextBox).Text;
                    RadAsyncUpload rauAnh = item.FindControl("rauAnh") as RadAsyncUpload;
                    CheckBox cbTrangThai = item.FindControl("cbTrangThai") as CheckBox;
                    CheckBox cbHienThi = item.FindControl("cbHienThi") as CheckBox;
                    foreach (UploadedFile f in rauAnh.UploadedFiles)
                    {
                        Guid strGuid = Guid.NewGuid();
                        string fileName = strGuid.ToString() + f.GetName();
                        string path = Server.MapPath("~/img/IconMenu/" + fileName);
                        f.SaveAs(path);
                        detail.ICON = "~/img/IconMenu/" + fileName;
                    }
                    if (cbTrangThai.Checked)
                        detail.TRANG_THAI = 1;
                    else detail.TRANG_THAI = 0;
                    detail.IS_HIEN_THI = cbHienThi.Checked;
                    detail.MA_CAP_HOC = rcbCapHoc.SelectedValue;
                    bo.update(detail, Sys_User.ID);
                }
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('success', 'Thao tác thành công');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Có lỗi xãy ra!');", true);
            }
        }
        protected void RadTreeList1_InsertCommand(object sender, Telerik.Web.UI.TreeListCommandEventArgs e)
        {
            try
            {
                var detail = new MENU();
                var item = e.Item as TreeListEditFormInsertItem;
                detail.TEN = (item.FindControl("tbTen") as TextBox).Text;
                detail.TEN_EG = (item.FindControl("tbTenEg") as TextBox).Text;
                detail.URL = (item.FindControl("tbUrrl") as TextBox).Text;
                detail.ICON_CSS_CLASS = (item.FindControl("tbIconClass") as TextBox).Text;
                detail.THU_TU = (item.FindControl("tbThuTu") as TextBox).Text;
                RadAsyncUpload rauAnh = item.FindControl("rauAnh") as RadAsyncUpload;
                CheckBox cbTrangThai = item.FindControl("cbTrangThai") as CheckBox;
                CheckBox cbHienThi = item.FindControl("cbHienThi") as CheckBox;
                foreach (UploadedFile f in rauAnh.UploadedFiles)
                {
                    Guid strGuid = Guid.NewGuid();
                    string fileName = strGuid.ToString() + f.GetName();
                    string path = Server.MapPath("~/img/IconMenu/" + fileName);
                    f.SaveAs(path);
                    detail.ICON = "~/img/IconMenu/" + fileName;
                }
                if (cbTrangThai.Checked)
                    detail.TRANG_THAI = 1;
                else detail.TRANG_THAI = 0;
                detail.IS_HIEN_THI = cbHienThi.Checked;
                if (item.ParentItem != null)
                {
                    detail.ID_CHA = Convert.ToInt64(item.ParentItem.GetDataKeyValue("ID"));
                    if (detail.ID_CHA.Value == 0) detail.ID_CHA = null;
                }
                detail.MA_CAP_HOC = rcbCapHoc.SelectedValue;
                if (string.IsNullOrEmpty(rcbCapHoc.SelectedValue))
                    detail.IS_SYS = true;

                if (string.IsNullOrEmpty(detail.TEN) || string.IsNullOrEmpty(detail.TEN_EG))
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Tên menu không được để trống');", true);
                    return;
                }
                bo.insert(detail, Sys_User.ID);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('success', 'Thao tác thành công');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Có lỗi xãy ra!');", true);
            }
        }
        protected void RadTreeList1_DeleteCommand(object sender, Telerik.Web.UI.TreeListCommandEventArgs e)
        {
            try
            {
                var item = e.Item as TreeListDataItem;
                if (item.CanExpand)
                {
                    e.Canceled = true;
                    return;
                }
                long dataKeyValue = Convert.ToInt64(item.GetDataKeyValue("ID").ToString());
                ResultEntity res = bo.delete(dataKeyValue, Sys_User.ID, true);
                if (res.Res)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('success', 'Thao tác thành công');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Có dữ liệu liên quan, không thể xóa!');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Có lỗi xãy ra!');", true);
            }
        }
        protected void RadTreeList1_ItemDataBound(object sender, Telerik.Web.UI.TreeListItemDataBoundEventArgs e)
        {
            if (e.Item is TreeListPagerItem)
            {
                TreeListPagerItem item = e.Item as TreeListPagerItem;
                Label GoToPageLabel = (Label)e.Item.FindControl("GoToPageLabel");
                GoToPageLabel.Text = "Đi đến:";
                Label pageOfLabel = e.Item.FindControl("PageOfLabel") as Label;
                pageOfLabel.Text = "trong " + item.Paging.PageCount.ToString();
                Button GoToPageLinkButton = e.Item.FindControl("GoToPageLinkButton") as Button;
                GoToPageLinkButton.Text = "Đi";
                Button ChangePageSizeLinkButton = e.Item.FindControl("ChangePageSizeLinkButton") as Button;
                ChangePageSizeLinkButton.Text = "Thay đổi";
            }
            if (e.Item is TreeListEditableItem && (e.Item as TreeListEditableItem).IsInEditMode)
            {
                //Load data in tree
            }
        }
        protected void RadTreeList1_EditCommand(object sender, Telerik.Web.UI.TreeListCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                TreeListDataItem treeListItem = (TreeListDataItem)e.Item;
                decimal idValue = decimal.Parse(treeListItem.GetDataKeyValue("ID").ToString());
            }
        }
        protected void rcbCapHoc_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadTreeList1.Rebind();
        }
    }
}