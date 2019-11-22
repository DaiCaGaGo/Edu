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
    public partial class NguoiDungMenu : AuthenticatePage
    {
        long id_Truong_req;
        long id_NguoiDung_req;
        public NguoiDungMenuBO ndBO = new NguoiDungMenuBO();
        private LocalAPI localAPI = new LocalAPI();
        private TruongBO truongBO = new TruongBO();
        private TRUONG truong = new TRUONG();
        LogUserBO logUserBO = new LogUserBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("idTruong") != null)
            {
                try
                {
                    id_Truong_req = Convert.ToInt64(Request.QueryString.Get("idTruong"));
                }
                catch { }
            }
            if (Request.QueryString.Get("idNguoiDung") != null)
            {
                try
                {
                    id_NguoiDung_req = Convert.ToInt64(Request.QueryString.Get("idNguoiDung"));
                }
                catch { }
            }
            if (!IsPostBack)
            {
                //objNguoiDung.SelectParameters.Add("id_truong", Sys_This_Truong.ID.ToString());
                //rcbNguoiDung.DataBind();
                if (id_Truong_req > 0)
                    truong = truongBO.getTruongById(id_Truong_req);
                int count = 0;
                if (truong.IS_MN == true)
                {
                    rcbCapHoc.Items.Insert(0, new RadComboBoxItem("Mầm non", "MN"));
                    count++;
                    if (Sys_This_Cap_Hoc == "MN") rcbCapHoc.SelectedValue = Sys_This_Cap_Hoc;
                }
                if (truong.IS_TH == true)
                {
                    rcbCapHoc.Items.Insert(count, new RadComboBoxItem("Tiểu học", "TH"));
                    count++;
                    if (Sys_This_Cap_Hoc == "TH") rcbCapHoc.SelectedValue = Sys_This_Cap_Hoc;
                }
                if (truong.IS_THCS == true)
                {
                    rcbCapHoc.Items.Insert(count, new RadComboBoxItem("Trung học sơ sở", "THCS"));
                    count++;
                    if (Sys_This_Cap_Hoc == "THCS") rcbCapHoc.SelectedValue = Sys_This_Cap_Hoc;
                }
                if (truong.IS_THPT == true)
                {
                    rcbCapHoc.Items.Insert(count, new RadComboBoxItem("Trung học phổ thông", "THPT"));
                    count++;
                    if (Sys_This_Cap_Hoc == "THPT") rcbCapHoc.SelectedValue = Sys_This_Cap_Hoc;
                }
                if (truong.IS_GDTX == true)
                {
                    rcbCapHoc.Items.Insert(count, new RadComboBoxItem("Giáo dục thường xuyên", "GDTX"));
                    if (Sys_This_Cap_Hoc == "GDTX") rcbCapHoc.SelectedValue = Sys_This_Cap_Hoc;
                }
                rcbCapHoc.DataBind();
                if (rcbCapHoc.Items.Count == 0)
                {
                    string strMsg = "notification('errors', 'Trường chưa thiết lập cấp học, vui lòng liên hệ quản trị viên');";
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
                    return;
                }
                RadTreeList1.ExpandAllItems();
            }
        }
        protected void btEdit_Click(object sender, EventArgs e)
        {
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
                        detail.ID_TRUONG = id_Truong_req;
                        detail.ID_NGUOI_DUNG = id_NguoiDung_req;
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
            RadTreeList1.DataSource = ndBO.getQuyenByNguoiDungTruongCapHoc(id_NguoiDung_req, id_Truong_req, rcbCapHoc.SelectedValue);
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
        protected void RadTreeList1_ItemCommand(object sender, TreeListCommandEventArgs e)
        {
            int success = 0, error = 0;
            if (e.CommandName == "CopyRole")
            {
                if (e.Item is TreeListDataItem)
                {
                    var lm = new NhomQuyenMenuEntity();
                    TreeListDataItem item = e.Item as TreeListDataItem;
                    long IdMenu = localAPI.ConvertStringTolong(item.GetDataKeyValue("ID").ToString()).Value;
                    ResultEntity res = new ResultEntity();
                       res = ndBO.SetRoleToManySchools(IdMenu, id_Truong_req, id_NguoiDung_req,  Sys_User.ID);//với tài khoản này: áp dụng cho tất cả các trường còn lại
                    if (res.Res)
                    {
                        success++;
                        logUserBO.insert(null, "UPDATE", "Áp dụng chức năng menu " + IdMenu + " cho nhiều trường với tài khoản " + id_NguoiDung_req, Sys_User.ID, DateTime.Now);
                    }
                    else
                        error++;
                    string strMsg = "";
                    if (error > 0)
                    {
                        strMsg = "notification('error', 'Có " + error + " bản ghi lỗi. Liên hệ với quản trị viên');";
                    }
                    if (success > 0)
                    {
                        strMsg += " notification('success', 'Cấp quyền thành công.');";
                    }
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
                }
            }
        }


    }
}