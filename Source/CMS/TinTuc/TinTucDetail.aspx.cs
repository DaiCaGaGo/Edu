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

namespace CMS.TinTuc
{
    public partial class TinTucDetail : AuthenticatePage
    {
        long? id;
        TinTucBO tinTucBO = new TinTucBO();
        LocalAPI localAPI = new LocalAPI();
        LogUserBO logUserBO = new LogUserBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            cbTaoBai_CheckedChanged(sender, e);
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
                if (id != null)
                {
                    TIN_TUC detail = new TIN_TUC();
                    detail = tinTucBO.getTinTucByID(id.Value);
                    if (detail != null)
                    {
                        tbTieuDe.Text = detail.TIEU_DE;
                        tbTomtat.Text = detail.NOI_DUNG_TOM_TAT;
                        tbNoiDung.Text = detail.NOI_DUNG != null ? detail.NOI_DUNG : "";
                        tbLink.Text = detail.LINK;
                        imgAnh.ImageUrl = detail.ANH_DAI_DIEN != null ? detail.ANH_DAI_DIEN.ToString() : "";
                        if (detail.NGAY_HIEU_LUC != null) rdNgayHieuLuc.SelectedDate = detail.NGAY_HIEU_LUC;
                        else rdNgayHieuLuc.SelectedDate = null;
                        if (detail.THU_TU != null) tbThuTu.Text = detail.THU_TU.ToString();
                        btEdit.Visible = is_access(SYS_Type_Access.SUA);
                        btAdd.Visible = false;

                        if (!string.IsNullOrEmpty(detail.NOI_DUNG) && detail.LINK.Contains("https://demo.1sms.vn/TinTuc/ViewPost.aspx"))
                        {
                            cbTaoBai.Checked = true;
                            cbTaoBai_CheckedChanged(sender, e);
                        }
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
                    getMaxThuTu();
                }
            }
        }
        protected void getMaxThuTu()
        {
            long? max_thu_tu = tinTucBO.getMaxThuTuByTruong(Sys_This_Truong.ID, Sys_This_Cap_Hoc, Convert.ToInt16(Sys_Ma_Nam_hoc));
            if (max_thu_tu != null)
                tbThuTu.Text = Convert.ToString(Convert.ToInt16(max_thu_tu) + 1);
            else tbThuTu.Text = "1";
        }
        protected void btAdd_Click(object sender, EventArgs e)
        {
            #region "check du lieu"
            if (!is_access(SYS_Type_Access.THEM))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            #endregion
            TIN_TUC detail = new TIN_TUC();
            detail.ID_TRUONG = Sys_This_Truong.ID;
            detail.MA_CAP_HOC = Sys_This_Cap_Hoc;
            detail.ID_NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
            detail.TIEU_DE = tbTieuDe.Text.Trim();
            detail.NOI_DUNG_TOM_TAT = tbTomtat.Text.Trim();
            detail.NOI_DUNG = Server.HtmlDecode(tbNoiDung.Text.Trim());
            detail.LINK = tbLink.Text.Trim();
            if (cbTaoBai.Checked)
            {
                if (string.IsNullOrEmpty(tbNoiDung.Text.Trim()))
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Nội dung tin bài không được để trống!');", true);
                    return;
                }
                detail.NOI_DUNG = Server.HtmlDecode(tbNoiDung.Text.Trim());
                detail.LINK = "https://demo.1sms.vn/TinTuc/ViewPost.aspx";
            }
            else
            {
                if (string.IsNullOrEmpty(tbLink.Text.Trim()))
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Link tin bài không được để trống!');", true);
                    return;
                }
                detail.NOI_DUNG = null;
                detail.LINK = tbLink.Text.Trim();
            }
            foreach (UploadedFile f in rauAnh.UploadedFiles)
            {
                Guid strGuid = Guid.NewGuid();
                string fileName = strGuid.ToString() + f.GetName();
                string path = Server.MapPath("~/img/AnhTinTuc/" + fileName);
                f.SaveAs(path);
                detail.ANH_DAI_DIEN = "~/img/AnhTinTuc/" + fileName;
            }
            try
            {
                detail.NGAY_HIEU_LUC = rdNgayHieuLuc.SelectedDate.Value;
            }
            catch { }
            detail.THU_TU = localAPI.ConvertStringToShort(tbThuTu.Text.Trim());
            ResultEntity res = tinTucBO.insert(detail, Sys_User.ID);
            string strMsg = "";
            if (res.Res)
            {
                tbTieuDe.Text = "";
                tbTieuDe.Focus();
                tbNoiDung.Text = "";
                tbLink.Text = "";
                getMaxThuTu();
                strMsg = "notification('success', '" + res.Msg + "');";
                TIN_TUC resMa = (TIN_TUC)res.ResObject;
                logUserBO.insert(Sys_This_Truong.ID, "INSERT", "Thêm mới tin tức " + resMa.ID, Sys_User.ID, DateTime.Now);
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
            #endregion
            TIN_TUC detail = new TIN_TUC();
            detail = tinTucBO.getTinTucByID(id.Value);
            if (detail == null) detail = new TIN_TUC();
            if (detail.ID_TRUONG != Sys_This_Truong.ID)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện sửa dữ liệu này!');", true);
                return;
            }
            detail.ID_TRUONG = Sys_This_Truong.ID;
            detail.MA_CAP_HOC = Sys_This_Cap_Hoc;
            detail.ID_NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
            detail.TIEU_DE = tbTieuDe.Text.Trim();
            detail.NOI_DUNG_TOM_TAT = tbTomtat.Text.Trim();
            //detail.NOI_DUNG = Server.HtmlDecode(tbNoiDung.Text.Trim());
            //detail.LINK = tbLink.Text.Trim();
            if (cbTaoBai.Checked)
            {
                if (string.IsNullOrEmpty(tbNoiDung.Text.Trim()))
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Nội dung tin bài không được để trống!');", true);
                    return;
                }
                detail.NOI_DUNG = Server.HtmlDecode(tbNoiDung.Text.Trim());
                detail.LINK = "https://demo.1sms.vn/TinTuc/ViewPost.aspx?id=" + id.Value;
            }
            else
            {
                if (string.IsNullOrEmpty(tbLink.Text.Trim()))
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Link tin bài không được để trống!');", true);
                    return;
                }
                detail.NOI_DUNG = null;
                detail.LINK = tbLink.Text.Trim();
            }
            foreach (UploadedFile f in rauAnh.UploadedFiles)
            {
                Guid strGuid = Guid.NewGuid();
                string fileName = strGuid.ToString() + f.GetName();
                string path = Server.MapPath("~/img/AnhTinTuc/" + fileName);
                f.SaveAs(path);
                detail.ANH_DAI_DIEN = "~/img/AnhTinTuc/" + fileName;
            }
            try
            {
                detail.NGAY_HIEU_LUC = rdNgayHieuLuc.SelectedDate.Value;
            }
            catch { }
            detail.THU_TU = localAPI.ConvertStringToShort(tbThuTu.Text.Trim());
            ResultEntity res = tinTucBO.update(detail, Sys_User.ID);
            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
                logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật tin tức " + id, Sys_User.ID, DateTime.Now);
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
        protected void cbTaoBai_CheckedChanged(object sender, EventArgs e)
        {
            if (cbTaoBai.Checked)
            {
                divNoiDung.Visible = true;
                divLink.Visible = false;
            }
            else
            {
                divNoiDung.Visible = false;
                divLink.Visible = true;
            }
        }
    }
}