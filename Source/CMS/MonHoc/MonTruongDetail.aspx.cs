using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS.MonHoc
{
    public partial class MonTruongDetail : AuthenticatePage
    {
        long? id_mon_truong;
        short? id_mon_hoc;
        MonHocTruongBO monTruongBO = new MonHocTruongBO();
        MonHocBO monBO = new MonHocBO();
        LocalAPI local_api = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("id_mon_truong") != null)
            {
                try
                {
                    id_mon_truong = Convert.ToInt16(Request.QueryString.Get("id_mon_truong"));
                }
                catch { }
            }
            if (Request.QueryString.Get("id_mon") != null)
            {
                try
                {
                    id_mon_hoc = local_api.ConvertStringToShort(Request.QueryString.Get("id_mon"));
                }
                catch { }
            }
            if (id_mon_hoc != null)
            {
                tbTen.Enabled = false;
                rcbKieuMon.Enabled = false;
                tbThuTu.Enabled = false;
                cblKhoiHoc.Enabled = false;
                tbHeSo.Enabled = false;
            }
            if (!IsPostBack)
            {
                if (Sys_This_Cap_Hoc != "GDTX")
                {
                    divGDTX.Visible = false;
                }
                else
                {
                    divGDTX.Visible = true;
                    rcbLoaiLopGDTX.SelectedValue = Sys_This_Lop_GDTX.ToString();
                }

                objKhoiHoc.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                cblKhoiHoc.DataBind();
                if (Sys_This_Cap_Hoc == "TH" || Sys_This_Cap_Hoc == "THCS" || (Sys_This_Lop_GDTX != null && Sys_This_Lop_GDTX == 2))
                {
                    divMonChuyen.Visible = false;
                }
                if (id_mon_truong != null)
                {
                    MON_HOC_TRUONG detail = new MON_HOC_TRUONG();
                    detail = monTruongBO.getMonTruongByID(id_mon_truong.Value);
                    if (detail != null)
                    {
                        tbTen.Text = detail.TEN;
                        if (detail.THU_TU != null)
                            tbThuTu.Text = detail.THU_TU.ToString();
                        if (detail.KIEU_MON == true) rcbKieuMon.SelectedValue = "1";
                        else rcbKieuMon.SelectedValue = "0";
                        if (detail.IS_MON_CHUYEN == true) cbMonChuyen.Checked = true;
                        if (detail.HE_SO != null) tbHeSo.Text = detail.HE_SO.ToString();
                        foreach (ListItem item in cblKhoiHoc.Items)
                        {
                            if (Sys_This_Cap_Hoc == "TH")
                            {
                                if (detail.IS_1 == true && item.Value == "1")
                                    item.Selected = true;
                                else if (detail.IS_2 == true && item.Value == "2")
                                    item.Selected = true;
                                else if (detail.IS_3 == true && item.Value == "3")
                                    item.Selected = true;
                                else if (detail.IS_4 == true && item.Value == "4")
                                    item.Selected = true;
                                else if (detail.IS_5 == true && item.Value == "5")
                                    item.Selected = true;
                            }
                            else if (Sys_This_Cap_Hoc == "THCS" || (Sys_This_Lop_GDTX != null && Sys_This_Lop_GDTX == 2))
                            {
                                if (detail.IS_6 == true && item.Value == "6")
                                    item.Selected = true;
                                else if (detail.IS_7 == true && item.Value == "7")
                                    item.Selected = true;
                                else if (detail.IS_8 == true && item.Value == "8")
                                    item.Selected = true;
                                else if (detail.IS_9 == true && item.Value == "9")
                                    item.Selected = true;
                            }
                            else if (Sys_This_Cap_Hoc == "THPT" || (Sys_This_Lop_GDTX != null && Sys_This_Lop_GDTX == 3))
                            {
                                if (detail.IS_10 == true && item.Value == "10")
                                    item.Selected = true;
                                else if (detail.IS_11 == true && item.Value == "11")
                                    item.Selected = true;
                                else if (detail.IS_12 == true && item.Value == "12")
                                    item.Selected = true;
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
                    foreach (ListItem item in cblKhoiHoc.Items)
                    {
                        item.Selected = true;
                    }
                    btEdit.Visible = false;
                    btAdd.Visible = is_access(SYS_Type_Access.THEM);
                    short? max_thu_tu = monTruongBO.getMaxThuTuByTruongNamHoc(Sys_This_Truong.ID, Convert.ToInt16(Sys_Ma_Nam_hoc), Sys_This_Cap_Hoc);
                    if (max_thu_tu != null)
                        tbThuTu.Text = Convert.ToString(Convert.ToInt16(max_thu_tu) + 1);
                }
            }
        }
        private static bool CheckKyTuDacBiet(string Char)
        {
            bool Kitu_dacbiet = false;
            for (int i = 0; i < Char.Length; i++)
            {
                if ((Char[i] >= 32 && Char[i] <= 47)
                   || (Char[i] >= 58 && Char[i] <= 64)
                   || (Char[i] >= 91 && Char[i] <= 96)
                   || (Char[i] >= 123 && Char[i] <= 126))
                    Kitu_dacbiet = true;
            }
            return Regex.IsMatch(Char, @"[A-Z]")
                   && Regex.IsMatch(Char, @"[0-9]")
                   && Kitu_dacbiet;
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
            short? loai_lop_GDTX = local_api.ConvertStringToShort(rcbLoaiLopGDTX.SelectedValue);
            if (Sys_This_Cap_Hoc != "GDTX") loai_lop_GDTX = null;
            MON_HOC_TRUONG detail = new MON_HOC_TRUONG();
            detail.TEN = tbTen.Text.Trim();
            detail.KIEU_MON = rcbKieuMon.SelectedValue == "0" ? false : true;
            detail.THU_TU = local_api.ConvertStringToShort(tbThuTu.Text);
            detail.IS_DELETE = false;
            bool IsSelectedKhoiHoc = false;
            foreach (ListItem item in cblKhoiHoc.Items)
            {
                
                if (Sys_This_Cap_Hoc == "TH")
                {
                    if (item.Selected)
                    {
                        if (item.Value == "1") detail.IS_1 = true;
                        if (item.Value == "2") detail.IS_2 = true;
                        if (item.Value == "3") detail.IS_3 = true;
                        if (item.Value == "4") detail.IS_4 = true;
                        if (item.Value == "5") detail.IS_5 = true;
                        IsSelectedKhoiHoc = true;
                    }
                }
                else if (Sys_This_Cap_Hoc == "THCS" || (loai_lop_GDTX != null && loai_lop_GDTX == 2))
                {
                    if (item.Selected)
                    {
                        if (item.Value == "6") detail.IS_6 = true;
                        if (item.Value == "7") detail.IS_7 = true;
                        if (item.Value == "8") detail.IS_8 = true;
                        if (item.Value == "9") detail.IS_9 = true;
                        IsSelectedKhoiHoc = true;
                    }
                }
                else if (Sys_This_Cap_Hoc == "THPT" || (loai_lop_GDTX != null && loai_lop_GDTX == 3))
                {
                    if (item.Selected)
                    {
                        if (item.Value == "10") detail.IS_10 = true;
                        if (item.Value == "11") detail.IS_11 = true;
                        if (item.Value == "12") detail.IS_12 = true;
                        IsSelectedKhoiHoc = true;
                    }
                }
            }
            if (IsSelectedKhoiHoc == false)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Chưa chọn khối học!');", true);
                return;
            }
            detail.MA_CAP_HOC = Sys_This_Cap_Hoc;
            detail.HE_SO = local_api.ConvertStringToShort(tbHeSo.Text);
            detail.ID_NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
            detail.ID_TRUONG = Sys_This_Truong.ID;
            short? loai_lop_gdtx = local_api.ConvertStringToShort(rcbLoaiLopGDTX.SelectedValue);
            if (Sys_This_Cap_Hoc == "THPT") detail.IS_MON_CHUYEN = cbMonChuyen.Checked ? true : false;

            ResultEntity res = monTruongBO.insert(detail, Sys_User.ID);
            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
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
            if (rcbKieuMon.SelectedValue == "0" && tbHeSo.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Bạn phải nhập hệ số cho môn học này!');", true);
                return;
            }
            short? loai_lop_GDTX = local_api.ConvertStringToShort(rcbLoaiLopGDTX.SelectedValue);
            if (Sys_This_Cap_Hoc != "GDTX") loai_lop_GDTX = null;
            MON_HOC_TRUONG detail = new MON_HOC_TRUONG();
            detail = monTruongBO.getMonTruongByID(id_mon_truong.Value);
            if (detail == null) detail = new MON_HOC_TRUONG();
            if (detail.ID_TRUONG != Sys_This_Truong.ID)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện sửa dữ liệu này!');", true);
                return;
            }
            detail.TEN = tbTen.Text.Trim();
            detail.KIEU_MON = rcbKieuMon.SelectedValue == "0" ? false : true;
            detail.THU_TU = local_api.ConvertStringToShort(tbThuTu.Text);
            foreach (ListItem item in cblKhoiHoc.Items)
            {
                if (Sys_This_Cap_Hoc == "TH")
                {
                    if (item.Selected)
                    {
                        if (item.Value == "1") detail.IS_1 = true;
                        else if (item.Value == "2") detail.IS_2 = true;
                        else if (item.Value == "3") detail.IS_3 = true;
                        else if (item.Value == "4") detail.IS_4 = true;
                        else if (item.Value == "5") detail.IS_5 = true;
                    }
                    else
                    {
                        if (item.Value == "1") detail.IS_1 = false;
                        else if (item.Value == "2") detail.IS_2 = false;
                        else if (item.Value == "3") detail.IS_3 = false;
                        else if (item.Value == "4") detail.IS_4 = false;
                        else if (item.Value == "5") detail.IS_5 = false;
                    }
                }
                else if (Sys_This_Cap_Hoc == "THCS" || (loai_lop_GDTX != null && loai_lop_GDTX == 2))
                {
                    if (item.Selected)
                    {
                        if (item.Value == "6") detail.IS_6 = true;
                        else if (item.Value == "7") detail.IS_7 = true;
                        else if (item.Value == "8") detail.IS_8 = true;
                        else if (item.Value == "9") detail.IS_9 = true;
                    }
                    else
                    {
                        if (item.Value == "6") detail.IS_6 = false;
                        else if (item.Value == "7") detail.IS_7 = false;
                        else if (item.Value == "8") detail.IS_8 = false;
                        else if (item.Value == "9") detail.IS_9 = false;
                    }
                }
                else if (Sys_This_Cap_Hoc == "THPT" || (loai_lop_GDTX != null && loai_lop_GDTX == 3))
                {
                    if (item.Selected)
                    {
                        if (item.Value == "10") detail.IS_10 = true;
                        else if (item.Value == "11") detail.IS_11 = true;
                        else if (item.Value == "12") detail.IS_12 = true;
                    }
                    else
                    {
                        if (item.Value == "10") detail.IS_10 = false;
                        else if (item.Value == "11") detail.IS_11 = false;
                        else if (item.Value == "12") detail.IS_12 = false;
                    }
                }
            }
            detail.MA_CAP_HOC = Sys_This_Cap_Hoc;
            detail.HE_SO = local_api.ConvertStringToShort(tbHeSo.Text);
            detail.IS_MON_CHUYEN = cbMonChuyen.Checked ? true : false;
            detail.ID_TRUONG = Sys_This_Truong.ID;
            detail.ID_NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
            ResultEntity res = monTruongBO.update(detail, Sys_User.ID);
            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
        protected void rcbLoaiLopGDTX_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            cblKhoiHoc.ClearSelection();
            cblKhoiHoc.DataBind();
        }
    }
}