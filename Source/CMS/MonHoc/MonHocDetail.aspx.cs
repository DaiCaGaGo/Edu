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

namespace CMS.MonHoc
{
    public partial class MonHocDetail : AuthenticatePage
    {
        short? id_mon;
        MonHocBO monBO = new MonHocBO();
        LocalAPI localAPI = new LocalAPI();
        LogUserBO logUserBO = new LogUserBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("id_mon") != null)
            {
                try
                {
                    id_mon = Convert.ToInt16(Request.QueryString.Get("id_mon"));
                }
                catch { }
            }
            if (!IsPostBack)
            {
                rcbKhoi.DataBind();
                if (id_mon != null)
                {
                    MON_HOC detail = new MON_HOC();
                    detail = monBO.getMonHocByID(id_mon.Value);
                    if (detail != null)
                    {
                        tbTen.Text = detail.TEN;
                        if (detail.THU_TU != null)
                            tbThuTu.Text = detail.THU_TU.ToString();
                        if (detail.KIEU_MON == true) rcbKieuMon.SelectedValue = "1";
                        else rcbKieuMon.SelectedValue = "0";
                        if (detail.HE_SO != null) tbHeSo.Text = detail.HE_SO.ToString();
                        short? maLoaiLopGDTX = (rcbCapHoc.SelectedValue == "GDTX") ? Sys_This_Lop_GDTX : null;
                        rcbCapHoc.SelectedValue = detail.MA_CAP_HOC;
                        rcbKhoi.DataBind();
                        foreach (RadComboBoxItem item in rcbKhoi.Items)
                        {
                            if (rcbCapHoc.SelectedValue == "TH")
                            {
                                if (item.Value == "1") item.Checked = detail.IS_1 == null ? false : detail.IS_1.Value;
                                if (item.Value == "2") item.Checked = detail.IS_2 == null ? false : detail.IS_2.Value;
                                if (item.Value == "3") item.Checked = detail.IS_3 == null ? false : detail.IS_3.Value;
                                if (item.Value == "4") item.Checked = detail.IS_4 == null ? false : detail.IS_4.Value;
                                if (item.Value == "5") item.Checked = detail.IS_5 == null ? false : detail.IS_5.Value;
                            }
                            else if (rcbCapHoc.SelectedValue == "THCS")
                            {
                                if (item.Value == "6") item.Checked = detail.IS_6 == null ? false : detail.IS_6.Value;
                                if (item.Value == "7") item.Checked = detail.IS_7 == null ? false : detail.IS_7.Value;
                                if (item.Value == "8") item.Checked = detail.IS_8 == null ? false : detail.IS_8.Value;
                                if (item.Value == "9") item.Checked = detail.IS_9 == null ? false : detail.IS_9.Value;
                            }
                            else if (rcbCapHoc.SelectedValue == "THPT")
                            {
                                if (item.Value == "10") item.Checked = detail.IS_10 == null ? false : detail.IS_10.Value;
                                if (item.Value == "11") item.Checked = detail.IS_11 == null ? false : detail.IS_11.Value;
                                if (item.Value == "12") item.Checked = detail.IS_12 == null ? false : detail.IS_12.Value;
                            }
                            else if (rcbCapHoc.SelectedValue == "GDTX")
                            {
                                if (item.Value == "1") item.Checked = detail.IS_1 == null ? false : detail.IS_1.Value;
                                if (item.Value == "2") item.Checked = detail.IS_2 == null ? false : detail.IS_2.Value;
                                if (item.Value == "3") item.Checked = detail.IS_3 == null ? false : detail.IS_3.Value;
                                if (item.Value == "4") item.Checked = detail.IS_4 == null ? false : detail.IS_4.Value;
                                if (item.Value == "5") item.Checked = detail.IS_5 == null ? false : detail.IS_5.Value;
                                if (item.Value == "6") item.Checked = detail.IS_6 == null ? false : detail.IS_6.Value;
                                if (item.Value == "7") item.Checked = detail.IS_7 == null ? false : detail.IS_7.Value;
                                if (item.Value == "8") item.Checked = detail.IS_8 == null ? false : detail.IS_8.Value;
                                if (item.Value == "9") item.Checked = detail.IS_9 == null ? false : detail.IS_9.Value;
                                if (item.Value == "10") item.Checked = detail.IS_10 == null ? false : detail.IS_10.Value;
                                if (item.Value == "11") item.Checked = detail.IS_11 == null ? false : detail.IS_11.Value;
                                if (item.Value == "12") item.Checked = detail.IS_12 == null ? false : detail.IS_12.Value;
                            }
                        }

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
            if (rcbKieuMon.SelectedValue == "0" && tbHeSo.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Bạn phải nhập hệ số cho môn học này!');", true);
                return;
            }
            MON_HOC detail = new MON_HOC();
            detail.TEN = tbTen.Text.Trim();
            detail.KIEU_MON = rcbKieuMon.SelectedValue == "0" ? false : true;
            detail.THU_TU = localAPI.ConvertStringToShort(tbThuTu.Text);
            detail.IS_DELETE = false;
            foreach (RadComboBoxItem item in rcbKhoi.Items)
            {
                if (rcbCapHoc.SelectedValue == "TH")
                {
                    if (item.Value == "1") detail.IS_1 = item.Checked ? true : false;
                    if (item.Value == "2") detail.IS_2 = item.Checked ? true : false;
                    if (item.Value == "3") detail.IS_3 = item.Checked ? true : false;
                    if (item.Value == "4") detail.IS_4 = item.Checked ? true : false;
                    if (item.Value == "5") detail.IS_5 = item.Checked ? true : false;
                }
                else if (rcbCapHoc.SelectedValue == "THCS")
                {
                    if (item.Value == "6") detail.IS_6 = item.Checked ? true : false;
                    if (item.Value == "7") detail.IS_7 = item.Checked ? true : false;
                    if (item.Value == "8") detail.IS_8 = item.Checked ? true : false;
                    if (item.Value == "9") detail.IS_9 = item.Checked ? true : false;
                }
                else if (rcbCapHoc.SelectedValue == "THPT")
                {
                    if (item.Value == "10") detail.IS_10 = item.Checked ? true : false;
                    if (item.Value == "11") detail.IS_11 = item.Checked ? true : false;
                    if (item.Value == "12") detail.IS_12 = item.Checked ? true : false;
                }
                else if (rcbCapHoc.SelectedValue == "GDTX")
                {
                    if (item.Value == "1") detail.IS_1 = item.Checked ? true : false;
                    if (item.Value == "2") detail.IS_2 = item.Checked ? true : false;
                    if (item.Value == "3") detail.IS_3 = item.Checked ? true : false;
                    if (item.Value == "4") detail.IS_4 = item.Checked ? true : false;
                    if (item.Value == "5") detail.IS_5 = item.Checked ? true : false;
                    if (item.Value == "6") detail.IS_6 = item.Checked ? true : false;
                    if (item.Value == "7") detail.IS_7 = item.Checked ? true : false;
                    if (item.Value == "8") detail.IS_8 = item.Checked ? true : false;
                    if (item.Value == "9") detail.IS_9 = item.Checked ? true : false;
                    if (item.Value == "10") detail.IS_10 = item.Checked ? true : false;
                    if (item.Value == "11") detail.IS_11 = item.Checked ? true : false;
                    if (item.Value == "12") detail.IS_12 = item.Checked ? true : false;
                }
            }
            detail.MA_CAP_HOC = rcbCapHoc.SelectedValue;
            detail.HE_SO = localAPI.ConvertStringToShort(tbHeSo.Text);
            ResultEntity res = monBO.insert(detail, Sys_User.ID);
            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
                MON_HOC resMonHoc = (MON_HOC)res.ResObject;
                logUserBO.insert(Sys_This_Truong.ID, "INSERT", "Thêm mới môn học " + resMonHoc.ID, Sys_User.ID, DateTime.Now);
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
        protected void btEdit_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            MON_HOC detail = new MON_HOC();
            detail.ID = id_mon.Value;
            detail = monBO.getMonHocByID(detail.ID);
            if (detail == null) detail = new MON_HOC();
            detail.TEN = tbTen.Text.Trim();
            detail.KIEU_MON = rcbKieuMon.SelectedValue == "0" ? false : true;
            detail.THU_TU = localAPI.ConvertStringToShort(tbThuTu.Text);
            foreach (RadComboBoxItem item in rcbKhoi.Items)
            {
                if (rcbCapHoc.SelectedValue == "TH")
                {
                    if (item.Value == "1") detail.IS_1 = item.Checked ? true : false;
                    if (item.Value == "2") detail.IS_2 = item.Checked ? true : false;
                    if (item.Value == "3") detail.IS_3 = item.Checked ? true : false;
                    if (item.Value == "4") detail.IS_4 = item.Checked ? true : false;
                    if (item.Value == "5") detail.IS_5 = item.Checked ? true : false;
                }
                else if (rcbCapHoc.SelectedValue == "THCS")
                {
                    if (item.Value == "6") detail.IS_6 = item.Checked ? true : false;
                    if (item.Value == "7") detail.IS_7 = item.Checked ? true : false;
                    if (item.Value == "8") detail.IS_8 = item.Checked ? true : false;
                    if (item.Value == "9") detail.IS_9 = item.Checked ? true : false;
                }
                else if (rcbCapHoc.SelectedValue == "THPT")
                {
                    if (item.Value == "10") detail.IS_10 = item.Checked ? true : false;
                    if (item.Value == "11") detail.IS_11 = item.Checked ? true : false;
                    if (item.Value == "12") detail.IS_12 = item.Checked ? true : false;
                }
                else if (rcbCapHoc.SelectedValue == "GDTX")
                {
                    if (item.Value == "1") detail.IS_1 = item.Checked ? true : false;
                    if (item.Value == "2") detail.IS_2 = item.Checked ? true : false;
                    if (item.Value == "3") detail.IS_3 = item.Checked ? true : false;
                    if (item.Value == "4") detail.IS_4 = item.Checked ? true : false;
                    if (item.Value == "5") detail.IS_5 = item.Checked ? true : false;
                    if (item.Value == "6") detail.IS_6 = item.Checked ? true : false;
                    if (item.Value == "7") detail.IS_7 = item.Checked ? true : false;
                    if (item.Value == "8") detail.IS_8 = item.Checked ? true : false;
                    if (item.Value == "9") detail.IS_9 = item.Checked ? true : false;
                    if (item.Value == "10") detail.IS_10 = item.Checked ? true : false;
                    if (item.Value == "11") detail.IS_11 = item.Checked ? true : false;
                    if (item.Value == "12") detail.IS_12 = item.Checked ? true : false;
                }
            }
            detail.MA_CAP_HOC = rcbCapHoc.SelectedValue;
            detail.HE_SO = localAPI.ConvertStringToShort(tbHeSo.Text);
            ResultEntity res = monBO.update(detail, Sys_User.ID);
            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
                logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật môn học " + detail.ID, Sys_User.ID, DateTime.Now);
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
        protected void rcbCapHoc_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbKhoi.ClearCheckedItems();
            rcbKhoi.Text = string.Empty;
        }
    }
}