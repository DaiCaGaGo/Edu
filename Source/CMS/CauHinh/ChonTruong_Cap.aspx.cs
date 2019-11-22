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

namespace CMS.CauHinh
{
    public partial class ChonTruong_Cap : AuthenticatePage
    {
        TruongBO truongBO = new TruongBO();
        TRUONG truong = new TRUONG();
        SYS_Profile sys_profile = new SYS_Profile();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Sys_This_Cap_Hoc == "GDTX") divLoaiLopGDTX.Visible = true;
            else divLoaiLopGDTX.Visible = false;
            if (!IsPostBack)
            {

                rcbTruong.DataSource = Sys_List_Truong;
                rcbTruong.DataBind();
                if (Sys_This_Truong != null) rcbTruong.SelectedValue = Sys_This_Truong.ID.ToString();
                loadCapHoc();
                if (Sys_This_Truong != null) rcbCapHoc.SelectedValue = Sys_This_Cap_Hoc;
                LoaiLopGDTXBO loaiLopGDTXBO = new LoaiLopGDTXBO();
                rcbLoaiLopGDTX.Visible = true;
                rcbGDTX.Visible = true;
                rcbLoaiLopGDTX.DataSource = loaiLopGDTXBO.getLoaiLopGDTX();
                rcbLoaiLopGDTX.DataBind();
                if (rcbCapHoc.SelectedValue == SYS_Cap_Hoc.GDTX)
                {
                    divLoaiLopGDTX.Visible = true;
                    if (Sys_This_Lop_GDTX != null) rcbLoaiLopGDTX.SelectedValue = Sys_This_Lop_GDTX.ToString();
                }
                else divLoaiLopGDTX.Visible = false;
            }
        }
        protected void rcbTruong_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            loadCapHoc();
            if (rcbCapHoc.SelectedValue == "GDTX") divLoaiLopGDTX.Visible = true;
            else divLoaiLopGDTX.Visible = false;
        }
        protected void rcbCapHoc_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (rcbCapHoc.SelectedValue == SYS_Cap_Hoc.GDTX)
            {
                divLoaiLopGDTX.Visible = true;
            }
            else divLoaiLopGDTX.Visible = false;
        }
        protected void loadCapHoc()
        {
            rcbCapHoc.Items.Clear();
            rcbCapHoc.ClearSelection();
            rcbCapHoc.Text = string.Empty;

            int count = 0;
            if (rcbTruong.SelectedValue != "")
            {
                truong = truongBO.getTruongById(Convert.ToInt64(rcbTruong.SelectedValue));
                if (truong != null)
                {
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
                        rcbCapHoc.Items.Insert(count, new RadComboBoxItem("Trung học cơ sở", "THCS"));
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
                }
            }
        }
        protected void btEdit_Click(object sender, EventArgs e)
        {
            if (rcbTruong.SelectedValue == null || rcbTruong.SelectedValue == "")
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn chưa chọn trường');", true);
                return;
            }
            if (rcbCapHoc.SelectedValue == null || rcbCapHoc.SelectedValue == "")
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn chưa chọn cấp học');", true);
                return;
            }
            if (rcbCapHoc.SelectedValue == SYS_Cap_Hoc.GDTX && (rcbLoaiLopGDTX.SelectedValue == null || rcbLoaiLopGDTX.SelectedValue == ""))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn chưa chọn loại lớp giáo dục thường xuyên');", true);
                divLoaiLopGDTX.Visible = true;
                return;
            }
            truong = new TRUONG();
            truong = truongBO.getTruongById(Convert.ToInt64(rcbTruong.SelectedValue));
            sys_profile.setThisTruong(truong);
            sys_profile.setCapHoc(rcbCapHoc.SelectedValue);
            if (rcbCapHoc.SelectedValue == SYS_Cap_Hoc.GDTX)
                sys_profile.setLoaiLopGDTX(rcbLoaiLopGDTX.SelectedValue);
            else sys_profile.setLoaiLopGDTX("");
            string returnUrl = Request.QueryString["returnUrl"];
            if (string.IsNullOrEmpty(returnUrl))
                Response.Redirect("~/Default.aspx", false);
            else
                Response.Redirect(returnUrl);
        }
    }
}