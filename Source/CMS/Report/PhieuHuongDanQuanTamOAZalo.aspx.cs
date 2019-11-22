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

namespace CMS.Report
{
    public partial class PhieuHuongDanQuanTamOAZalo : AuthenticatePage
    {
        private HocSinhBO hsBO = new HocSinhBO();
        TruongBO truongBO = new TruongBO();
        LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            btTimKiem.Visible = is_access(SYS_Type_Access.THEM);
            btTimKiem.Visible = is_access(SYS_Type_Access.XEM);
            if (!IsPostBack)
            {
                rcbTruong.DataSource = Sys_List_Truong;
                rcbTruong.DataBind();
                loadCapHoc();
                objKhoi.SelectParameters.Add("maLoaiLopGDTX", DbType.Int16, Sys_This_Lop_GDTX == null ? "" : Sys_This_Lop_GDTX.ToString());
                rcbKhoi.DataBind();
                objLop.SelectParameters.Add("maNamHoc", Sys_Ma_Nam_hoc.ToString());
                rcbLop.DataBind();
            }
        }
        protected void loadCapHoc()
        {
            rcbCapHoc.Items.Clear();
            rcbCapHoc.ClearSelection();
            rcbCapHoc.Text = string.Empty;

            int count = 0;
            if (rcbTruong.SelectedValue != "")
            {
                TRUONG truong = new TRUONG();
                truong = truongBO.getTruongById(Convert.ToInt64(rcbTruong.SelectedValue));
                if (truong != null)
                {
                    if (truong.IS_MN == true)
                    {
                        rcbCapHoc.Items.Insert(0, new RadComboBoxItem("Mầm non", "MN"));
                        count++;
                    }
                    if (truong.IS_TH == true)
                    {
                        rcbCapHoc.Items.Insert(count, new RadComboBoxItem("Tiểu học", "TH"));
                        count++;
                    }
                    if (truong.IS_THCS == true)
                    {
                        rcbCapHoc.Items.Insert(count, new RadComboBoxItem("Trung học sơ sở", "THCS"));
                        count++;
                    }
                    if (truong.IS_THPT == true)
                    {
                        rcbCapHoc.Items.Insert(count, new RadComboBoxItem("Trung học phổ thông", "THPT"));
                        count++;
                    }
                    if (truong.IS_GDTX == true)
                    {
                        rcbCapHoc.Items.Insert(count, new RadComboBoxItem("Giáo dục thường xuyên", "GDTX"));
                    }
                    rcbCapHoc.DataBind();
                    if (rcbCapHoc.Items.Count == 0)
                    {
                        string strMsg = "notification('errors', 'Trường chưa thiết lập cấp học, vui lòng liên hệ quản trị viên');";
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
                        return;
                    }
                    else if (rcbCapHoc.Items.Count == 1)
                    {
                        rcbCapHoc.SelectedIndex = 0;
                    }
                }
            }
        }
        protected void rcbTruong_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {

            loadCapHoc();
            rcbKhoi.ClearSelection();
            rcbKhoi.Text = String.Empty;
            rcbKhoi.DataBind();
            rcbLop.ClearSelection();
            rcbLop.Text = String.Empty;
            rcbLop.DataBind();
        }
        protected void rcbCapHoc_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbKhoi.ClearSelection();
            rcbKhoi.Text = String.Empty;
            rcbKhoi.DataBind();
        }
        protected void rcbKhoi_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbLop.ClearSelection();
            rcbLop.Text = String.Empty;
            rcbLop.DataBind();
        }
        protected void btTimKiem_Click(object sender, EventArgs e)
        {
            List<long> lst_id_lop = new List<long>();
            foreach (var item in rcbLop.CheckedItems)
            {
                lst_id_lop.Add(localAPI.ConvertStringTolong(item.Value).Value);
            }
            List<HocSinhEntity> lst = hsBO.getListHocSinhByListLop(Convert.ToInt64(rcbTruong.SelectedValue), localAPI.ConvertStringToShort(rcbKhoi.SelectedValue), lst_id_lop, Convert.ToInt16(Sys_Ma_Nam_hoc), rcbCapHoc.SelectedValue, Sys_Hoc_Ky);
            string ReportID = "DangKyZalo";
            Session["PhieuHuongDanDangKyZalo" + ReportID] = lst;
            Response.Redirect("~/Report/ReportView.aspx?ma=" + ReportID, true);

        }
    }
}