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

namespace CMS.DinhDuong
{
    public partial class DMBuAnDetail : AuthenticatePage
    {
        long? id_dm_bua_an_req = null;
        DMBuaAnBO dMBuaAnBO = new DMBuaAnBO();
        NangLuongTrongNgayBO nangLuongNgayBO = new NangLuongTrongNgayBO();
        private LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            if (Request.QueryString.Get("id_hoso") != null)
            {
                try
                {
                    id_dm_bua_an_req = Convert.ToInt64(Request.QueryString.Get("id_hoso"));
                }
                catch { }
            }
            if (!IsPostBack)
            {
                objKhoi.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                objKhoi.SelectParameters.Add("maLoaiLopGDTX", DbType.Int16, Sys_This_Lop_GDTX == null ? "" : Sys_This_Lop_GDTX.ToString());
                rcbKhoiHoc.DataBind();

                if (id_dm_bua_an_req != null)
                {
                    DM_BUA_AN detail = new DM_BUA_AN();
                    detail = dMBuaAnBO.getById(id_dm_bua_an_req.Value);
                    if (detail != null)
                    {
                        rcbKhoiHoc.DataBind();
                        if (rcbKhoiHoc.Items.FindItemByValue(detail.ID_KHOI.ToString()) != null)
                            rcbKhoiHoc.SelectedValue = detail.ID_KHOI.ToString();
                        tbTen.Text = detail.TEN;
                        tbThuTu.Text = detail.THU_TU == null ? "" : detail.THU_TU.ToString();
                        rntKcalTu.Text = detail.NANG_LUONG_KCAL_TU == null ? "" : detail.NANG_LUONG_KCAL_TU.ToString();
                        rntKcalDen.Text = detail.NANG_LUONG_KCAL_DEN == null ? "" : detail.NANG_LUONG_KCAL_DEN.ToString();
                        rntProtitTu.Text = detail.PROTID_TU == null ? "" : detail.PROTID_TU.ToString();
                        rntProtitDen.Text = detail.PROTID_DEN == null ? "" : detail.PROTID_DEN.ToString();
                        rntLipitTu.Text = detail.LIPID_TU == null ? "" : detail.LIPID_TU.ToString();
                        rntLipitDen.Text = detail.LIPID_DEN == null ? "" : detail.LIPID_DEN.ToString();
                        rntGluxitTu.Text = detail.GLUCID_TU == null ? "" : detail.GLUCID_TU.ToString();
                        rntGluxitDen.Text = detail.GLUCID_DEN == null ? "" : detail.GLUCID_DEN.ToString();
                        tbGia.Text = detail.GIA_TIEN == null ? "" : detail.GIA_TIEN.ToString();
                        tbGhiChu.Text = detail.GHI_CHU == null ? "" : detail.GHI_CHU.ToString();

                        rntNangLuong_Kcal_tu.Text = detail.NANG_LUONG_TU_KCAL == null ? "" : detail.NANG_LUONG_TU_KCAL.ToString();
                        rntNangLuong_Kcal_den.Text = detail.NANG_LUONG_DEN_KCAL == null ? "" : detail.NANG_LUONG_DEN_KCAL.ToString();
                        rntProtid_Kcal_tu.Text = detail.PROTID_TU_KCAL == null ? "" : detail.PROTID_TU_KCAL.ToString();
                        rntProtid_Kcal_den.Text = detail.PROTID_DEN_KCAL == null ? "" : detail.PROTID_DEN_KCAL.ToString();
                        rntLipid_Kcal_tu.Text = detail.LIPID_TU_KCAL == null ? "" : detail.LIPID_DEN_KCAL.ToString();
                        rntLipid_Kcal_den.Text = detail.LIPID_DEN_KCAL == null ? "" : detail.LIPID_DEN_KCAL.ToString();
                        rntGlucid_Kcal_tu.Text = detail.GLUCID_TU_KCAL == null ? "" : detail.GLUCID_TU_KCAL.ToString();
                        rntGlucid_Kcal_den.Text = detail.GLUCID_DEN_KCAL == null ? "" : detail.GLUCID_DEN_KCAL.ToString();

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
                    // getMaxThuTu();
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
            string strMsg = "";
            DM_BUA_AN detail = new DM_BUA_AN();
            detail.TEN = tbTen.Text.Trim();
            detail.ID_KHOI = localAPI.ConvertStringToShort(rcbKhoiHoc.SelectedValue).Value;
            detail.GHI_CHU = tbGhiChu.Text.Trim();
            detail.GIA_TIEN = localAPI.ConvertStringToDecimal(tbGia.Text.Trim());
            detail.GLUCID_DEN = localAPI.ConvertStringToDecimal(rntGluxitDen.Text.Trim());
            detail.TEN = tbTen.Text.Trim();
            detail.GLUCID_TU = localAPI.ConvertStringToDecimal(rntGluxitTu.Text.Trim());
            if (string.IsNullOrEmpty(rcbKhoiHoc.SelectedValue))
            {
                strMsg = "notification('error', 'Khối học không được để trống');";
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
                return;
            }
            decimal? nang_luong_tu = localAPI.ConvertStringToDecimal(rntKcalTu.Text.Trim());
            decimal? nang_luong_den = localAPI.ConvertStringToDecimal(rntKcalDen.Text.Trim());
            decimal? protid_tu = localAPI.ConvertStringToDecimal(rntProtitTu.Text.Trim());
            decimal? protid_den = localAPI.ConvertStringToDecimal(rntProtitDen.Text.Trim());
            decimal? lipid_tu = localAPI.ConvertStringToDecimal(rntLipitTu.Text.Trim());
            decimal? lipid_den = localAPI.ConvertStringToDecimal(rntLipitDen.Text.Trim());
            decimal? glucid_tu = localAPI.ConvertStringToDecimal(rntGluxitTu.Text.Trim());
            decimal? glucid_den = localAPI.ConvertStringToDecimal(rntGluxitDen.Text.Trim());

            detail.ID_KHOI = localAPI.ConvertStringToShort(rcbKhoiHoc.SelectedValue).Value;
            detail.ID_NHOM_TUOI_MN = null;
            detail.ID_TRUONG = Sys_This_Truong.ID;
            detail.IS_DELETE = null;
            detail.LIPID_DEN = lipid_den;
            detail.LIPID_TU = lipid_tu;
            detail.NANG_LUONG_KCAL_TU = nang_luong_tu;
            detail.NANG_LUONG_KCAL_DEN = nang_luong_den;
            detail.PROTID_DEN = protid_den;
            detail.PROTID_TU = protid_tu;
            detail.THU_TU = localAPI.ConvertStringTolong(tbThuTu.Text.Trim());
            #region get năng lượng trong ngày theo khối học
            NANG_LUONG_TRONG_NGAY nangLuong = nangLuongNgayBO.getNangLuongNgayByKhoi(Convert.ToInt16(rcbKhoiHoc.SelectedValue));
            if (nangLuong != null)
            {
                if (nangLuong.NANG_LUONG_KCAL_TU != null && nang_luong_tu != null)
                    detail.NANG_LUONG_TU_KCAL = Math.Round(Convert.ToDecimal(nang_luong_tu * nangLuong.NANG_LUONG_KCAL_TU / 100), 0);
                if (nangLuong.NANG_LUONG_KCAL_DEN != null && nang_luong_den != null)
                    detail.NANG_LUONG_DEN_KCAL = Math.Round(Convert.ToDecimal(nang_luong_den * nangLuong.NANG_LUONG_KCAL_DEN / 100), 0);
                if (nangLuong.PROTID_TU != null && protid_tu != null)
                    detail.PROTID_TU_KCAL = Math.Round(Convert.ToDecimal(protid_tu * nangLuong.PROTID_TU / 100), 0);
                if (nangLuong.PROTID_DEN != null && protid_den != null)
                    detail.PROTID_DEN_KCAL = Math.Round(Convert.ToDecimal(protid_den * nangLuong.PROTID_DEN / 100), 0);
                if (nangLuong.LIPID_TU != null && lipid_tu != null)
                    detail.LIPID_TU_KCAL = Math.Round(Convert.ToDecimal(lipid_tu * nangLuong.LIPID_TU / 100), 0);
                if (nangLuong.LIPID_DEN != null && lipid_den != null)
                    detail.LIPID_DEN_KCAL = Math.Round(Convert.ToDecimal(lipid_den * nangLuong.LIPID_DEN / 100), 0);
                if (nangLuong.GLUCID_TU != null && glucid_tu != null)
                    detail.GLUCID_TU_KCAL = Math.Round(Convert.ToDecimal(glucid_tu * nangLuong.GLUCID_TU / 100), 0);
                if (nangLuong.GLUCID_DEN != null && glucid_den != null)
                    detail.GLUCID_DEN_KCAL = Math.Round(Convert.ToDecimal(glucid_den * nangLuong.GLUCID_DEN / 100), 0);
            }
            #endregion
            ResultEntity res = dMBuaAnBO.insert(detail, Sys_User.ID);
            DM_BUA_AN resMaLop = (DM_BUA_AN)res.ResObject;
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

        protected void btEdit_Click(object sender, EventArgs e)
        {
            string strMsg = "";
            if (!is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            DM_BUA_AN detail = new DM_BUA_AN();
            detail.ID = id_dm_bua_an_req.Value;
            detail = dMBuaAnBO.getById(detail.ID);
            if (detail == null)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Không có bửa ăn này!');", true);
                return;
            }
            if (detail.ID_TRUONG != Sys_This_Truong.ID)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện sửa dữ liệu này!');", true);
                return;
            }
            detail.TEN = tbTen.Text.Trim();
            detail.ID_KHOI = localAPI.ConvertStringToShort(rcbKhoiHoc.SelectedValue).Value;
            detail.GHI_CHU = tbGhiChu.Text.Trim();
            detail.GIA_TIEN = localAPI.ConvertStringToDecimal(tbGia.Text.Trim());
            detail.GLUCID_DEN = localAPI.ConvertStringToDecimal(rntGluxitDen.Text.Trim());
            detail.TEN = tbTen.Text.Trim();
            detail.GLUCID_TU = localAPI.ConvertStringToDecimal(rntGluxitTu.Text.Trim());
            if (string.IsNullOrEmpty(rcbKhoiHoc.SelectedValue))
            {
                strMsg = "notification('error', 'Khối học không được để trống');";
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
                return;
            }
            decimal? nang_luong_tu = localAPI.ConvertStringToDecimal(rntKcalTu.Text.Trim());
            decimal? nang_luong_den = localAPI.ConvertStringToDecimal(rntKcalDen.Text.Trim());
            decimal? protid_tu = localAPI.ConvertStringToDecimal(rntProtitTu.Text.Trim());
            decimal? protid_den = localAPI.ConvertStringToDecimal(rntProtitDen.Text.Trim());
            decimal? lipid_tu = localAPI.ConvertStringToDecimal(rntLipitTu.Text.Trim());
            decimal? lipid_den = localAPI.ConvertStringToDecimal(rntLipitDen.Text.Trim());
            decimal? glucid_tu = localAPI.ConvertStringToDecimal(rntGluxitTu.Text.Trim());
            decimal? glucid_den = localAPI.ConvertStringToDecimal(rntGluxitDen.Text.Trim());

            detail.ID_KHOI = localAPI.ConvertStringToShort(rcbKhoiHoc.SelectedValue).Value;
            detail.ID_NHOM_TUOI_MN = null;
            detail.ID_TRUONG = Sys_This_Truong.ID;
            detail.IS_DELETE = null;
            detail.LIPID_DEN = lipid_den;
            detail.LIPID_TU = lipid_tu;
            detail.NANG_LUONG_KCAL_TU = nang_luong_tu;
            detail.NANG_LUONG_KCAL_DEN = nang_luong_den;
            detail.PROTID_DEN = protid_den;
            detail.PROTID_TU = protid_tu;
            detail.THU_TU = localAPI.ConvertStringTolong(tbThuTu.Text.Trim());
            #region get năng lượng trong ngày theo khối học
            NANG_LUONG_TRONG_NGAY nangLuong = nangLuongNgayBO.getNangLuongNgayByKhoi(Convert.ToInt16(rcbKhoiHoc.SelectedValue));
            if (nangLuong != null)
            {
                if (nangLuong.NANG_LUONG_KCAL_TU != null && nang_luong_tu != null)
                    detail.NANG_LUONG_TU_KCAL = Math.Round(Convert.ToDecimal(nang_luong_tu * nangLuong.NANG_LUONG_KCAL_TU / 100), 0);
                if (nangLuong.NANG_LUONG_KCAL_DEN != null && nang_luong_den != null)
                    detail.NANG_LUONG_DEN_KCAL = Math.Round(Convert.ToDecimal(nang_luong_den * nangLuong.NANG_LUONG_KCAL_DEN / 100), 0);
                if (nangLuong.PROTID_TU != null && protid_tu != null)
                    detail.PROTID_TU_KCAL = Math.Round(Convert.ToDecimal(protid_tu * nangLuong.PROTID_TU / 100), 0);
                if (nangLuong.PROTID_DEN != null && protid_den != null)
                    detail.PROTID_DEN_KCAL = Math.Round(Convert.ToDecimal(protid_den * nangLuong.PROTID_DEN / 100), 0);
                if (nangLuong.LIPID_TU != null && lipid_tu != null)
                    detail.LIPID_TU_KCAL = Math.Round(Convert.ToDecimal(lipid_tu * nangLuong.LIPID_TU / 100), 0);
                if (nangLuong.LIPID_DEN != null && lipid_den != null)
                    detail.LIPID_DEN_KCAL = Math.Round(Convert.ToDecimal(lipid_den * nangLuong.LIPID_DEN / 100), 0);
                if (nangLuong.GLUCID_TU != null && glucid_tu != null)
                    detail.GLUCID_TU_KCAL = Math.Round(Convert.ToDecimal(glucid_tu * nangLuong.GLUCID_TU / 100), 0);
                if (nangLuong.GLUCID_DEN != null && glucid_den != null)
                    detail.GLUCID_DEN_KCAL = Math.Round(Convert.ToDecimal(glucid_den * nangLuong.GLUCID_DEN / 100), 0);
            }
            #endregion
            ResultEntity res = dMBuaAnBO.update(detail, Sys_User.ID);
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
        protected void reset()
        {
            tbTen.Text = "";
            tbThuTu.Text = "";
            tbGhiChu.Text = "";
            tbGia.Text = "";
            rntKcalTu.Text = "";
            rntKcalDen.Text = "";
            rntProtitTu.Text = "";
            rntProtitDen.Text = "";
            rntLipitTu.Text = "";
            rntLipitDen.Text = "";
            rntGluxitTu.Text = "";
            rntGluxitDen.Text = "";
        }
    }
}