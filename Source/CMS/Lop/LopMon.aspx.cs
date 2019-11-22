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

namespace CMS.Lop
{
    public partial class LopMon : AuthenticatePage
    {
        private LopMonBO lopMonBO = new LopMonBO();
        long? id_lop;
        short? ma_khoi;
        private LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("id_lop") != null)
            {
                try
                {
                    id_lop = Convert.ToInt64(Request.QueryString.Get("id_lop"));
                }
                catch { }
            }
            if (Request.QueryString.Get("ma_khoi") != null)
            {
                try
                {
                    ma_khoi = Convert.ToInt16(Request.QueryString.Get("ma_khoi"));
                }
                catch { }
            }
            btEdit.Visible = is_access(SYS_Type_Access.SUA);
            btCopy.Visible = is_access(SYS_Type_Access.SUA);
            if (!IsPostBack)
            {
                objGVCN.SelectParameters.Add("id_truong", Sys_This_Truong.ID.ToString());
                rcbHocKy.DataBind();
                rcbHocKy.SelectedValue = Sys_Hoc_Ky.ToString();
            }
        }
        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                long idMonTruong = localAPI.ConvertStringTolong(item.GetDataKeyValue("ID").ToString()).Value;
                RadComboBox rcbGiaoVienItem = (RadComboBox)e.Item.FindControl("rcbGiaoVien");
                CheckBox ckIsCheckItem = (CheckBox)e.Item.FindControl("ckIsCheck");
                CheckBox ckIS_MON_CHUYEN = (CheckBox)e.Item.FindControl("ckIS_MON_CHUYEN");
                HiddenField hdGiaoVien = (HiddenField)e.Item.FindControl("hdGiaoVien");
                HiddenField hdIsMonByLop = (HiddenField)e.Item.FindControl("hdIsCheck");
                HiddenField hdIS_MON_CHUYEN = (HiddenField)e.Item.FindControl("hdIS_MON_CHUYEN");
                long? id_gv = null;
                int? isCheck = null;
                id_gv = localAPI.ConvertStringTolong(hdGiaoVien.Value);
                isCheck = localAPI.ConvertStringToint(hdIsMonByLop.Value);
                int? is_mon_chuyen = localAPI.ConvertStringToint(hdIS_MON_CHUYEN.Value);
                rcbGiaoVienItem.DataBind();
                if (id_gv != null && rcbGiaoVienItem.Items.FindItemByValue(id_gv.ToString()) != null)
                {
                    rcbGiaoVienItem.SelectedValue = id_gv.ToString();
                }
                ckIsCheckItem.Checked = isCheck == null ? false : isCheck == 1;
                ckIS_MON_CHUYEN.Checked = is_mon_chuyen != null && is_mon_chuyen == 1 ? true : false;
                HiddenField hdIdMonHoc = (HiddenField)item.FindControl("hdIdMonHoc");
                HiddenField hdIdLop = (HiddenField)item.FindControl("hdIdLop");

                RadNumericTextBox txtSoCotMieng = (RadNumericTextBox)e.Item.FindControl("txtSoCotMieng");
                RadNumericTextBox txtSoCot15P = (RadNumericTextBox)e.Item.FindControl("txtSoCot15P");
                RadNumericTextBox txtSoCot1T_HS1 = (RadNumericTextBox)e.Item.FindControl("txtSoCot1T_HS1");
                RadNumericTextBox txtSoCot1T_HS2 = (RadNumericTextBox)e.Item.FindControl("txtSoCot1T_HS2");
                HiddenField hdIDLopMon = (HiddenField)item.FindControl("hdIDLopMon");
                if (hdIDLopMon.Value == "" || hdIDLopMon.Value == null)
                {
                    txtSoCotMieng.Text = "5";
                    txtSoCot15P.Text = "5";
                    txtSoCot1T_HS1.Text = "5";
                    txtSoCot1T_HS2.Text = "5";
                }
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = lopMonBO.getMonXetChoLopByKhoiLopHocKy(ma_khoi.ToString(), Sys_This_Truong.ID, id_lop, Sys_Ma_Nam_hoc, Convert.ToInt16(rcbHocKy.SelectedValue), Sys_This_Cap_Hoc);
            if (Sys_This_Cap_Hoc == "THPT")
                RadGrid1.MasterTableView.Columns.FindByUniqueName("IS_MON_CHUYEN").Display = true;
            else RadGrid1.MasterTableView.Columns.FindByUniqueName("IS_MON_CHUYEN").Display = false;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
        protected void rcbHocKy_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void btCopy_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            ResultEntity res = lopMonBO.copyMonLopKy1SangKy2(id_lop, Sys_User.ID);
            res = lopMonBO.copyMonLopKy1SangKy2_capNhatTrangThai(id_lop);
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
            RadGrid1.Rebind();
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
            foreach (GridDataItem item in RadGrid1.Items)
            {
                var lm = new LopMonEntity();
                long idMonTruong = localAPI.ConvertStringTolong(item.GetDataKeyValue("ID").ToString()).Value;
                RadComboBox rcbGiaoVienItem = (RadComboBox)item.FindControl("rcbGiaoVien");
                CheckBox ckIsCheckItem = (CheckBox)item.FindControl("ckIsCheck");
                CheckBox ckIS_MON_CHUYEN = (CheckBox)item.FindControl("ckIS_MON_CHUYEN");
                lm.ID_GIAO_VIEN = localAPI.ConvertStringTolong(rcbGiaoVienItem.SelectedValue);
                lm.IS_CHECK = ckIsCheckItem.Checked ? 1 : 0;

                HiddenField hdGiaoVien = (HiddenField)item.FindControl("hdGiaoVien");
                HiddenField hdIsMonByLop = (HiddenField)item.FindControl("hdIsCheck");
                HiddenField hdIdMonHoc = (HiddenField)item.FindControl("hdIdMonHoc");
                long? giaoVien_old = localAPI.ConvertStringTolong(hdGiaoVien.Value);
                int? isCheck_old = null;
                isCheck_old = localAPI.ConvertStringToint(hdIsMonByLop.Value);

                LOP_MON detailLopMon = new LOP_MON();
                RadNumericTextBox txtSoCotMieng = (RadNumericTextBox)item.FindControl("txtSoCotMieng");
                RadNumericTextBox txtSoCot15P = (RadNumericTextBox)item.FindControl("txtSoCot15P");
                RadNumericTextBox txtSoCot1T_HS1 = (RadNumericTextBox)item.FindControl("txtSoCot1T_HS1");
                RadNumericTextBox txtSoCot1T_HS2 = (RadNumericTextBox)item.FindControl("txtSoCot1T_HS2");

                detailLopMon = lopMonBO.getLopMonByLopMonHocKy(id_lop.Value, idMonTruong, Convert.ToInt16(rcbHocKy.SelectedValue));
                short mieng_n = 0, p15_n = 0, hs1_n = 0, hs2_n = 0;
                if (txtSoCotMieng.Text != "") mieng_n = Convert.ToInt16(txtSoCotMieng.Text);
                if (txtSoCot15P.Text != "") p15_n = Convert.ToInt16(txtSoCot15P.Text);
                if (txtSoCot1T_HS1.Text != "") hs1_n = Convert.ToInt16(txtSoCot1T_HS1.Text);
                if (txtSoCot1T_HS2.Text != "") hs2_n = Convert.ToInt16(txtSoCot1T_HS2.Text);

                if (isCheck_old != lm.IS_CHECK || giaoVien_old != lm.ID_GIAO_VIEN || lm.SO_COT_DIEM_MIENG != mieng_n || lm.SO_COT_DIEM_15P != p15_n || lm.SO_COT_DIEM_1T_HS1 != hs1_n || lm.SO_COT_DIEM_1T_HS2 != hs2_n)
                {
                    if (detailLopMon != null)
                    {
                        detailLopMon.ID_GIAO_VIEN = lm.ID_GIAO_VIEN;
                        detailLopMon.TRANG_THAI = ckIsCheckItem.Checked;
                        detailLopMon.IS_MON_CHUYEN = ckIS_MON_CHUYEN.Checked;
                        detailLopMon.IS_DELETE = null;
                        if (mieng_n != 0) detailLopMon.SO_COT_DIEM_MIENG = mieng_n; else detailLopMon.SO_COT_DIEM_MIENG = null;
                        if (p15_n != 0) detailLopMon.SO_COT_DIEM_15P = p15_n; else detailLopMon.SO_COT_DIEM_15P = null;
                        if (hs1_n != 0) detailLopMon.SO_COT_DIEM_1T_HS1 = hs1_n; else detailLopMon.SO_COT_DIEM_1T_HS1 = null;
                        if (hs2_n != 0) detailLopMon.SO_COT_DIEM_1T_HS2 = hs2_n; else detailLopMon.SO_COT_DIEM_1T_HS2 = null;
                        res = lopMonBO.update(detailLopMon, Sys_User.ID);
                    }
                    else if (ckIsCheckItem.Checked)
                    {
                        detailLopMon = new LOP_MON();
                        detailLopMon.ID_LOP = id_lop.Value;
                        if (hdIdMonHoc.Value != null && hdIdMonHoc.Value != "") detailLopMon.ID_MON = Convert.ToInt16(hdIdMonHoc.Value);
                        detailLopMon.ID_MON_TRUONG = Convert.ToInt16(item.GetDataKeyValue("ID").ToString());
                        detailLopMon.HOC_KY = localAPI.ConvertStringToShort(rcbHocKy.SelectedValue).Value;
                        detailLopMon.TRANG_THAI = ckIsCheckItem.Checked;
                        detailLopMon.IS_MON_CHUYEN = ckIS_MON_CHUYEN.Checked;
                        detailLopMon.IS_DELETE = null;
                        detailLopMon.ID_GIAO_VIEN = lm.ID_GIAO_VIEN;
                        if (mieng_n != 0) detailLopMon.SO_COT_DIEM_MIENG = mieng_n; else detailLopMon.SO_COT_DIEM_MIENG = null;
                        if (p15_n != 0) detailLopMon.SO_COT_DIEM_15P = p15_n; else detailLopMon.SO_COT_DIEM_15P = null;
                        if (hs1_n != 0) detailLopMon.SO_COT_DIEM_1T_HS1 = hs1_n; else detailLopMon.SO_COT_DIEM_1T_HS1 = null;
                        if (hs2_n != 0) detailLopMon.SO_COT_DIEM_1T_HS2 = hs2_n; else detailLopMon.SO_COT_DIEM_1T_HS2 = null;
                        res = lopMonBO.insert(detailLopMon, Sys_User.ID);
                    }
                }
            }
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
    }
}