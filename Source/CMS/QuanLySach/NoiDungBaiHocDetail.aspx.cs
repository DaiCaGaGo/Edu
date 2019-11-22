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

namespace CMS.QuanLySach
{
    public partial class NoiDungBaiHocDetail : AuthenticatePage
    {
        long? id;
        DMSachChiTietBO dmSachChiTietBO = new DMSachChiTietBO();
        LocalAPI localAPI = new LocalAPI();
        LogUserBO logUserBO = new LogUserBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("id") != null)
            {
                try
                {
                    id = Convert.ToInt16(Request.QueryString.Get("id"));
                }
                catch { }
            }
            if (!IsPostBack)
            {
                objKhoi.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                objKhoi.SelectParameters.Add("maLoaiLopGDTX", DbType.Int16, Sys_This_Lop_GDTX == null ? "" : Sys_This_Lop_GDTX.ToString());
                rcbKhoiHoc.DataBind();
                objMonHoc.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                rcbMonHoc.DataBind();
                rcbSach.DataBind();
                rcbBaiHoc.DataBind();
                if (id != null)
                {
                    DM_SACH_CHI_TIET detail = new DM_SACH_CHI_TIET();
                    detail = dmSachChiTietBO.getChiTietSachByID(id.Value);
                    if (detail != null)
                    {
                        rcbKhoiHoc.SelectedValue = detail.ID_KHOI.ToString();
                        rcbMonHoc.SelectedValue = detail.ID_MON.ToString();
                        rcbSach.SelectedValue = detail.ID_SACH.ToString();
                        rcbBaiHoc.SelectedValue = detail.ID_TEN_BAI_HOC.ToString();
                        if (detail.BAI_SO != null) tbBaiSo.Text = detail.BAI_SO.ToString();
                        if (detail.TRANG_SO != null) tbTrangSo.Text = detail.TRANG_SO.ToString();
                        imgAnh.ImageUrl = detail.ICON != null ? detail.ICON.ToString() : "";
                        tbNoiDung.Text = detail.NOI_DUNG != null ? detail.NOI_DUNG.ToString() : "";
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
            #region "check du lieu"
            if (!is_access(SYS_Type_Access.THEM))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            if (string.IsNullOrEmpty(rcbMonHoc.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Môn học không được để trống!');", true);
                return;
            }
            if (string.IsNullOrEmpty(rcbSach.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Tên sách không được để trống!');", true);
                return;
            }
            if (string.IsNullOrEmpty(tbBaiSo.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Số tên bài tập không được để trống!');", true);
                return;
            }
            if (string.IsNullOrEmpty(tbTrangSo.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Số trang bài tập không được để trống!');", true);
                return;
            }
            //if (string.IsNullOrEmpty(rcbBaiHoc.SelectedValue))
            //{
            //    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Tên bài học không được để trống!');", true);
            //    return;
            //}
            #endregion
            DM_SACH_CHI_TIET detail = new DM_SACH_CHI_TIET();
            detail.ID_KHOI = Convert.ToInt16(rcbKhoiHoc.SelectedValue);
            detail.ID_MON = Convert.ToInt16(rcbMonHoc.SelectedValue);
            detail.ID_SACH = Convert.ToInt64(rcbSach.SelectedValue);
            detail.ID_TEN_BAI_HOC = localAPI.ConvertStringTolong(rcbBaiHoc.SelectedValue);
            detail.BAI_SO = localAPI.ConvertStringToShort(tbBaiSo.Text.Trim());
            detail.TRANG_SO = localAPI.ConvertStringToShort(tbTrangSo.Text.Trim());
            detail.NOI_DUNG = tbNoiDung.Text.Trim();
            foreach (UploadedFile f in rauAnh.UploadedFiles)
            {
                Guid strGuid = Guid.NewGuid();
                string fileName = strGuid.ToString() + f.GetName();
                string path = Server.MapPath("~/img/AnhBaiTrongSach/" + fileName);
                f.SaveAs(path);
                detail.ICON = "~/img/AnhBaiTrongSach/" + fileName;
            }
            ResultEntity res = dmSachChiTietBO.insert(detail, Sys_User.ID);
            string strMsg = "";
            if (res.Res)
            {
                tbBaiSo.Text = "";
                tbTrangSo.Text = "";
                tbNoiDung.Text = "";
                strMsg = "notification('success', '" + res.Msg + "');";
                DM_SACH_CHI_TIET resMa = (DM_SACH_CHI_TIET)res.ResObject;
                logUserBO.insert(Sys_This_Truong.ID, "INSERT", "Thêm mới bài tập trong sách " + resMa.ID, Sys_User.ID, DateTime.Now);
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
        protected void btEdit_Click(object sender, EventArgs e)
        {
            #region "check du lieu"
            if (!is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            if (string.IsNullOrEmpty(rcbMonHoc.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Môn học không được để trống!');", true);
                return;
            }
            if (string.IsNullOrEmpty(rcbSach.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Tên sách không được để trống!');", true);
                return;
            }
            if (string.IsNullOrEmpty(tbBaiSo.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Số tên bài tập không được để trống!');", true);
                return;
            }
            if (string.IsNullOrEmpty(tbTrangSo.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Số trang bài tập không được để trống!');", true);
                return;
            }
            #endregion
            DM_SACH_CHI_TIET detail = new DM_SACH_CHI_TIET();
            detail = dmSachChiTietBO.getChiTietSachByID(id.Value);
            if (detail == null) detail = new DM_SACH_CHI_TIET();
            detail.ID_KHOI = Convert.ToInt16(rcbKhoiHoc.SelectedValue);
            detail.ID_MON = Convert.ToInt16(rcbMonHoc.SelectedValue);
            detail.ID_SACH = Convert.ToInt64(rcbSach.SelectedValue);
            detail.ID_TEN_BAI_HOC = localAPI.ConvertStringTolong(rcbBaiHoc.SelectedValue);
            detail.BAI_SO = localAPI.ConvertStringToShort(tbBaiSo.Text.Trim());
            detail.TRANG_SO = localAPI.ConvertStringToShort(tbTrangSo.Text.Trim());
            detail.NOI_DUNG = tbNoiDung.Text.Trim();
            foreach (UploadedFile f in rauAnh.UploadedFiles)
            {
                Guid strGuid = Guid.NewGuid();
                string fileName = strGuid.ToString() + f.GetName();
                string path = Server.MapPath("~/img/AnhBaiTrongSach/" + fileName);
                f.SaveAs(path);
                detail.ICON = "~/img/AnhBaiTrongSach/" + fileName;
            }
            ResultEntity res = dmSachChiTietBO.update(detail, Sys_User.ID);
            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
                logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật bài tập trong sách " + id, Sys_User.ID, DateTime.Now);
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
        protected void rcbKhoiHoc_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbMonHoc.ClearSelection();
            rcbMonHoc.Text = string.Empty;
            rcbMonHoc.DataBind();
            rcbSach.ClearSelection();
            rcbSach.Text = string.Empty;
            rcbSach.ClearSelection();
            rcbBaiHoc.ClearSelection();
            rcbBaiHoc.Text = string.Empty;
            rcbBaiHoc.DataBind();
        }
        protected void rcbMonHoc_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbSach.ClearSelection();
            rcbSach.Text = string.Empty;
            rcbSach.ClearSelection();
            rcbBaiHoc.ClearSelection();
            rcbBaiHoc.Text = string.Empty;
            rcbBaiHoc.DataBind();
        }
        protected void rcbSach_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbBaiHoc.ClearSelection();
            rcbBaiHoc.Text = string.Empty;
            rcbBaiHoc.DataBind();
        }
    }
}