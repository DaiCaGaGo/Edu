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

namespace CMS.Lop
{
    public partial class LopDetail : AuthenticatePage
    {
        long? id_lop_req;
        private LopBO lopBO = new LopBO();
        private LocalAPI localAPI = new LocalAPI();
        private LopMonBO lopMonBO = new LopMonBO();
        private KhoiBO khoiBO = new KhoiBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("id_hoso") != null)
            {
                try
                {
                    id_lop_req = Convert.ToInt64(Request.QueryString.Get("id_hoso"));
                }
                catch { }
            }
            if (!IsPostBack)
            {
                objKhoi.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                //objKhoi.SelectParameters.Add("maLoaiLopGDTX", DbType.Int16, Sys_This_Lop_GDTX == null ? "" : Sys_This_Lop_GDTX.ToString());
                rcbKhoiHoc.DataBind();
                objGVCN.SelectParameters.Add("id", Sys_This_Truong.ID.ToString());
                rcbTienTo.DataBind();
                rcbTienTo.SelectedValue = "2";
                if (Sys_This_Cap_Hoc != "GDTX")
                {
                    rcbLoaiLopGDTX.Enabled = false;
                }
                else
                {
                    rcbLoaiLopGDTX.SelectedValue = Sys_This_Lop_GDTX.ToString();
                }

                if (id_lop_req != null)
                {
                    LopEntity detail = new LopEntity();
                    detail = lopBO.getLopById(id_lop_req.Value);
                    if (detail != null)
                    {
                        rcbKhoiHoc.DataBind();
                        if (rcbKhoiHoc.Items.FindItemByValue(detail.ID_KHOI.ToString()) != null)
                            rcbKhoiHoc.SelectedValue = detail.ID_KHOI.ToString();
                        rcbGVCN.DataBind();
                        if (rcbGVCN.Items.FindItemByValue(detail.ID_GVCN.ToString()) != null)
                            rcbGVCN.SelectedValue = detail.ID_GVCN.ToString();
                        if (Sys_This_Truong.IS_GDTX == true)
                        {
                            rcbLoaiLopGDTX.DataBind();
                            rcbLoaiLopGDTX.SelectedValue = Sys_This_Lop_GDTX.ToString();
                        }
                        tbTen.Text = detail.TEN_LOP;
                        tbThuTu.Text = detail.THU_TU.ToString();
                        if (detail.LOAI_CHEN_TIN != null) rcbTienTo.SelectedValue = detail.LOAI_CHEN_TIN.ToString();
                        if (detail.TIEN_TO != null) tbTienTo.Text = detail.TIEN_TO.ToString();
                        if (rcbTienTo.SelectedValue == "3") divTienTo.Visible = true;
                        else divTienTo.Visible = false;
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
                    getMaxThuTu();
                }
            }
        }
        protected void getMaxThuTu()
        {
            int? max_thu_tu = lopBO.getMaxThuTuByTruongKhoiNamHoc(Sys_This_Truong.ID, localAPI.ConvertStringToShort(rcbKhoiHoc.SelectedValue), Convert.ToInt16(Sys_Ma_Nam_hoc));
            if (max_thu_tu != null)
                tbThuTu.Text = Convert.ToString(Convert.ToInt16(max_thu_tu) + 1);
            else tbThuTu.Text = "1";
        }
        protected void btAdd_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.THEM))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            LOP detail = new LOP();
            detail.TEN = tbTen.Text.Trim();
            detail.ID_KHOI = localAPI.ConvertStringToShort(rcbKhoiHoc.SelectedValue).Value;
            detail.ID_GVCN = localAPI.ConvertStringToShort(rcbGVCN.SelectedValue);
            detail.ID_NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
            detail.ID_TRUONG = Sys_This_Truong.ID;
            detail.MA_LOAI_LOP_GDTX = localAPI.ConvertStringToShort(rcbLoaiLopGDTX.SelectedValue);
            detail.THU_TU = localAPI.ConvertStringToint(tbThuTu.Text.Trim());
            detail.LOAI_CHEN_TIN = localAPI.ConvertStringToShort(rcbTienTo.SelectedValue);
            detail.TIEN_TO = tbTienTo.Text.Trim() == "" ? null : tbTienTo.Text.Trim();
            ResultEntity res = lopBO.insert(detail, Sys_User.ID);
            LOP resMaLop = (LOP)res.ResObject;
            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
                reset();
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);

        }
        protected void reset()
        {
            rcbGVCN.ClearSelection();
            tbTen.Text = "";
            rcbLoaiLopGDTX.ClearSelection();
            tbTienTo.Text = "";
            getMaxThuTu();
        }
        protected void btEdit_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            LOP detail = new LOP();
            detail = lopBO.getLopById(id_lop_req.Value);
            if (detail == null) detail = new LOP();
            if (detail.ID_TRUONG != Sys_This_Truong.ID)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện sửa dữ liệu này!');", true);
                return;
            }
            detail.ID_KHOI = localAPI.ConvertStringToShort(rcbKhoiHoc.SelectedValue).Value;
            detail.ID_GVCN = localAPI.ConvertStringToShort(rcbGVCN.SelectedValue);
            detail.TEN = tbTen.Text;
            detail.THU_TU = localAPI.ConvertStringToint(tbThuTu.Text.Trim());
            detail.MA_LOAI_LOP_GDTX = localAPI.ConvertStringToShort(rcbLoaiLopGDTX.SelectedValue);
            detail.ID_TRUONG = Sys_This_Truong.ID;
            detail.ID_NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
            detail.LOAI_CHEN_TIN = localAPI.ConvertStringToShort(rcbTienTo.SelectedValue);
            detail.TIEN_TO = tbTienTo.Text.Trim() == "" ? null : tbTienTo.Text.Trim();
            ResultEntity res = lopBO.update(detail, Sys_User.ID);

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
        protected void rcbLoaiLopGDTX_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbKhoiHoc.ClearSelection();
            rcbKhoiHoc.DataBind();
        }
        protected void rcbTienTo_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (rcbTienTo.SelectedValue == "3") divTienTo.Visible = true;
            else divTienTo.Visible = false;
        }
        protected void rcbKhoiHoc_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            getMaxThuTu();
        }
    }
}