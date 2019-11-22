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

namespace CMS.DinhDuong
{
    public partial class SuatAn_ThemThucPham : AuthenticatePage
    {
        long? id_suat_an;
        short? id_khoi;
        ThucPhamBO thucPhamBO = new ThucPhamBO();
        NhomThucPhamBO nhomThucPhamBO = new NhomThucPhamBO();
        DonViTinhBO donViTinhBO = new DonViTinhBO();
        SuatAnChiTietBO suatAnChiTietBO = new SuatAnChiTietBO();
        SuatAnBO suatAnBO = new SuatAnBO();
        LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["id_suat_an" + Sys_User.ID] != null)
                id_suat_an = localAPI.ConvertStringTolong(Session["id_suat_an" + Sys_User.ID].ToString());
            if (Session["KhoiID" + Sys_User.ID] != null)
                id_khoi = localAPI.ConvertStringToShort(Session["KhoiID" + Sys_User.ID].ToString());
            if (!IsPostBack)
            {
                objKhoi.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                rcbKhoi.DataBind();
                objBuaAn.SelectParameters.Add("id_truong", Sys_This_Truong.ID.ToString());
                rcbBuaAn.DataBind();
                objThucDon.SelectParameters.Add("idTruong", Sys_This_Truong.ID.ToString());
                rcbThucDon.DataBind();
                if (id_suat_an != null)
                {
                    Session["TableThucPhamSuatAn" + Sys_User.ID] = null;
                    Session["TableThanhPhanDinhDuongSuatAn" + Sys_User.ID] = null;
                }
            }
            if ((Session["ThucDonID" + Sys_User.ID] != null && Session["ThucDonID" + Sys_User.ID].ToString() != "") &&
                (Session["KhoiID" + Sys_User.ID] != null && Session["KhoiID" + Sys_User.ID].ToString() != "") &&
                (Session["BuaAnID" + Sys_User.ID] != null && Session["BuaAnID" + Sys_User.ID].ToString() != ""))
            {
                try
                {
                    rcbKhoi.SelectedValue = Session["KhoiID" + Sys_User.ID].ToString();
                    rcbBuaAn.SelectedValue = Session["BuaAnID" + Sys_User.ID].ToString();
                    rcbThucDon.SelectedValue = Session["ThucDonID" + Sys_User.ID].ToString();
                }
                catch { }
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            id_khoi = 0; id_suat_an = 0;
            if (Session["KhoiID" + Sys_User.ID] != null)
                id_khoi = localAPI.ConvertStringToShort(Session["KhoiID" + Sys_User.ID].ToString());
            if (Session["id_suat_an" + Sys_User.ID] != null)
                id_suat_an = localAPI.ConvertStringToShort(Session["id_suat_an" + Sys_User.ID].ToString());
            RadGrid1.DataSource = suatAnChiTietBO.getThucPhamInSuatAn(Sys_This_Truong.ID, id_khoi, id_suat_an.Value, localAPI.ConvertStringToShort(rcbNhomThucPham.SelectedValue), localAPI.ConvertStringTolong(rcbThucDon.SelectedValue), tbTen.Text.Trim());
        }
        protected void btnChon_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            int success = 0, error = 0;
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            List<SuatAnChiTietEntity> lst = new List<SuatAnChiTietEntity>();
            if (id_suat_an == null)
            {
                foreach (GridDataItem item in RadGrid1.Items)
                {
                    CheckBox chbIS_CHON = (CheckBox)item.FindControl("chbIS_CHON");
                    if (chbIS_CHON.Checked)
                    {
                        short id_nhom_tp = Convert.ToInt16(item["ID_NHOM_THUC_PHAM"].Text);
                        long id_tp = Convert.ToInt64(item.GetDataKeyValue("ID").ToString());
                        short? don_vi_tinh = localAPI.ConvertStringToShort(item["DON_VI_TINH"].Text);
                        TextBox tbSO_LUONG = (TextBox)item.FindControl("tbSO_LUONG");
                        TextBox tbPROTID = (TextBox)item.FindControl("tbPROTID");
                        TextBox tbGLUCID = (TextBox)item.FindControl("tbGLUCID");
                        TextBox tbLIPID = (TextBox)item.FindControl("tbLIPID");
                        TextBox tbNANG_LUONG_KCAL = (TextBox)item.FindControl("tbNANG_LUONG_KCAL");

                        decimal? NANG_LUONG_KCAL_TP = localAPI.ConvertStringToDecimal(item["NANG_LUONG_KCAL_TP"].Text);
                        decimal? PROTID_TP = localAPI.ConvertStringToDecimal(item["PROTID_TP"].Text);
                        decimal? GLUCID_TP = localAPI.ConvertStringToDecimal(item["GLUCID_TP"].Text);
                        decimal? LIPID_TP = localAPI.ConvertStringToDecimal(item["LIPID_TP"].Text);
                        DM_THUC_PHAM thucPham = thucPhamBO.getThucPhamByNhomAndID(id_nhom_tp, id_tp);
                        success++;
                        #region add session
                        SuatAnChiTietEntity thucDon = new SuatAnChiTietEntity();
                        thucDon.ID_NHOM_THUC_PHAM = id_nhom_tp;
                        thucDon.ID_THUC_PHAM = id_tp;
                        thucDon.NANG_LUONG_KCAL = localAPI.ConvertStringToDecimal(tbNANG_LUONG_KCAL.Text.Trim());
                        thucDon.PROTID = localAPI.ConvertStringToDecimal(tbPROTID.Text.Trim());
                        thucDon.GLUCID = localAPI.ConvertStringToDecimal(tbGLUCID.Text.Trim());
                        thucDon.LIPID = localAPI.ConvertStringToDecimal(tbLIPID.Text.Trim());
                        thucDon.SO_LUONG = localAPI.ConvertStringToDecimal(tbSO_LUONG.Text.Trim());
                        thucDon.DON_VI_TINH = don_vi_tinh;
                        thucDon.NANG_LUONG_KCAL_OLD = NANG_LUONG_KCAL_TP;
                        thucDon.PROTID_OLD = PROTID_TP;
                        thucDon.GLUCID_OLD = GLUCID_TP;
                        thucDon.LIPID_OLD = LIPID_TP;
                        lst.Add(thucDon);
                        #endregion
                    }
                }
            }
            else
            {
                decimal? tong_nang_luong = 0, tong_protid = 0, tong_glucid = 0, tong_lipid = 0;
                foreach (GridDataItem item in RadGrid1.Items)
                {
                    short id_nhom_tp = Convert.ToInt16(item["ID_NHOM_THUC_PHAM"].Text);
                    long id_tp = Convert.ToInt64(item.GetDataKeyValue("ID").ToString());
                    short? don_vi_tinh = localAPI.ConvertStringToShort(item["DON_VI_TINH"].Text);
                    TextBox tbPROTID = (TextBox)item.FindControl("tbPROTID");
                    TextBox tbGLUCID = (TextBox)item.FindControl("tbGLUCID");
                    TextBox tbLIPID = (TextBox)item.FindControl("tbLIPID");
                    TextBox tbNANG_LUONG_KCAL = (TextBox)item.FindControl("tbNANG_LUONG_KCAL");
                    TextBox tbSO_LUONG = (TextBox)item.FindControl("tbSO_LUONG");

                    decimal? NANG_LUONG_KCAL_TP = localAPI.ConvertStringToDecimal(item["NANG_LUONG_KCAL_TP"].Text);
                    decimal? PROTID_TP = localAPI.ConvertStringToDecimal(item["PROTID_TP"].Text);
                    decimal? GLUCID_TP = localAPI.ConvertStringToDecimal(item["GLUCID_TP"].Text);
                    decimal? LIPID_TP = localAPI.ConvertStringToDecimal(item["LIPID_TP"].Text);

                    CheckBox chbIS_CHON = (CheckBox)item.FindControl("chbIS_CHON");
                    SUAT_AN_CHI_TIET detail = new SUAT_AN_CHI_TIET();
                    detail = suatAnChiTietBO.getSuatAnChiTietByTruongAndSuatAn(Sys_This_Truong.ID, id_khoi.Value, id_nhom_tp, id_tp, id_suat_an.Value);
                    if (detail != null)
                    {
                        if (chbIS_CHON.Checked)
                        {
                            detail.SO_LUONG = localAPI.ConvertStringToDecimal(tbSO_LUONG.Text.Trim());
                            detail.DON_VI_TINH = don_vi_tinh;
                            detail.NANG_LUONG_KCAL = localAPI.ConvertStringToDecimal(tbNANG_LUONG_KCAL.Text.Trim());
                            detail.PROTID = localAPI.ConvertStringToDecimal(tbPROTID.Text.Trim());
                            detail.GLUCID = localAPI.ConvertStringToDecimal(tbGLUCID.Text.Trim());
                            detail.LIPID = localAPI.ConvertStringToDecimal(tbLIPID.Text.Trim());
                            detail.IS_DELETE = false;
                            tong_nang_luong += localAPI.ConvertStringToDecimal(tbNANG_LUONG_KCAL.Text.Trim());
                            tong_protid += localAPI.ConvertStringToDecimal(tbPROTID.Text.Trim());
                            tong_glucid += localAPI.ConvertStringToDecimal(tbGLUCID.Text.Trim());
                            tong_lipid += localAPI.ConvertStringToDecimal(tbLIPID.Text.Trim());

                            #region add session
                            SuatAnChiTietEntity thucDon = new SuatAnChiTietEntity();
                            thucDon.ID_NHOM_THUC_PHAM = id_nhom_tp;
                            thucDon.ID_THUC_PHAM = id_tp;
                            thucDon.NANG_LUONG_KCAL = localAPI.ConvertStringToDecimal(tbNANG_LUONG_KCAL.Text.Trim());
                            thucDon.PROTID = localAPI.ConvertStringToDecimal(tbPROTID.Text.Trim());
                            thucDon.GLUCID = localAPI.ConvertStringToDecimal(tbGLUCID.Text.Trim());
                            thucDon.LIPID = localAPI.ConvertStringToDecimal(tbLIPID.Text.Trim());
                            thucDon.SO_LUONG = localAPI.ConvertStringToDecimal(tbSO_LUONG.Text.Trim());
                            thucDon.DON_VI_TINH = don_vi_tinh;
                            thucDon.NANG_LUONG_KCAL_OLD = NANG_LUONG_KCAL_TP;
                            thucDon.PROTID_OLD = PROTID_TP;
                            thucDon.GLUCID_OLD = GLUCID_TP;
                            thucDon.LIPID_OLD = LIPID_TP;
                            lst.Add(thucDon);
                            #endregion
                        }
                        else
                        {
                            detail.IS_DELETE = true;
                            detail.SO_LUONG = null;
                            detail.DON_VI_TINH = null;
                            detail.NANG_LUONG_KCAL = null;
                            detail.PROTID = null;
                            detail.GLUCID = null;
                            detail.LIPID = null;
                        }
                        res = suatAnChiTietBO.insertOrUpdate(detail, Sys_User.ID);
                        if (res.Res) success++; else error++;
                    }
                    else if (detail == null && chbIS_CHON.Checked)
                    {
                        detail = new SUAT_AN_CHI_TIET();
                        detail.ID_TRUONG = Sys_This_Truong.ID;
                        detail.ID_KHOI = id_khoi.Value;
                        detail.ID_SUAT_AN = id_suat_an.Value;
                        detail.ID_NHOM_THUC_PHAM = id_nhom_tp;
                        detail.ID_THUC_PHAM = id_tp;
                        detail.SO_LUONG = localAPI.ConvertStringToDecimal(tbSO_LUONG.Text.Trim());
                        detail.DON_VI_TINH = don_vi_tinh;
                        detail.NANG_LUONG_KCAL = localAPI.ConvertStringToDecimal(tbNANG_LUONG_KCAL.Text.Trim());
                        detail.PROTID = localAPI.ConvertStringToDecimal(tbPROTID.Text.Trim());
                        detail.GLUCID = localAPI.ConvertStringToDecimal(tbGLUCID.Text.Trim());
                        detail.LIPID = localAPI.ConvertStringToDecimal(tbLIPID.Text.Trim());
                        tong_nang_luong += localAPI.ConvertStringToDecimal(tbNANG_LUONG_KCAL.Text.Trim());
                        tong_protid += localAPI.ConvertStringToDecimal(tbPROTID.Text.Trim());
                        tong_glucid += localAPI.ConvertStringToDecimal(tbGLUCID.Text.Trim());
                        tong_lipid += localAPI.ConvertStringToDecimal(tbLIPID.Text.Trim());
                        res = suatAnChiTietBO.insertOrUpdate(detail, Sys_User.ID);
                        if (res.Res) success++; else error++;

                        #region add session
                        SuatAnChiTietEntity thucDon = new SuatAnChiTietEntity();
                        thucDon.ID_NHOM_THUC_PHAM = id_nhom_tp;
                        thucDon.ID_THUC_PHAM = id_tp;
                        thucDon.NANG_LUONG_KCAL = localAPI.ConvertStringToDecimal(tbNANG_LUONG_KCAL.Text.Trim());
                        thucDon.PROTID = localAPI.ConvertStringToDecimal(tbPROTID.Text.Trim());
                        thucDon.GLUCID = localAPI.ConvertStringToDecimal(tbGLUCID.Text.Trim());
                        thucDon.LIPID = localAPI.ConvertStringToDecimal(tbLIPID.Text.Trim());
                        thucDon.SO_LUONG = localAPI.ConvertStringToDecimal(tbSO_LUONG.Text.Trim());
                        thucDon.DON_VI_TINH = don_vi_tinh;
                        thucDon.NANG_LUONG_KCAL_OLD = NANG_LUONG_KCAL_TP;
                        thucDon.PROTID_OLD = PROTID_TP;
                        thucDon.GLUCID_OLD = GLUCID_TP;
                        thucDon.LIPID_OLD = LIPID_TP;
                        lst.Add(thucDon);
                        #endregion
                    }
                }
                SUAT_AN detaiSuatAn = new SUAT_AN();
                detaiSuatAn = suatAnBO.getSuatAnByID(id_suat_an.Value);
                if (detaiSuatAn != null)
                {
                    detaiSuatAn.TONG_NANG_LUONG_KCAL = tong_nang_luong;
                    detaiSuatAn.TONG_PROTID = tong_protid;
                    detaiSuatAn.TONG_GLUCID = tong_glucid;
                    detaiSuatAn.TONG_LIPID = tong_lipid;
                    suatAnBO.update(detaiSuatAn, Sys_User.ID);
                }
            }
            Session["TableThucPhamSuatAn" + Sys_User.ID] = lst;
            string strMsg = "";
            if (success > 0)
            {
                strMsg = "notification('success', 'Có " + success + " bản ghi lưu thành công!');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
        }
        protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                short? id_nhom = localAPI.ConvertStringToShort(item["ID_NHOM_THUC_PHAM"].Text);
                if (id_nhom != null)
                    item["TEN_NHOM"].Text = nhomThucPhamBO.getNhomThucPham("").FirstOrDefault(x => x.ID == id_nhom.Value).TEN;
                short? don_vi_tinh = localAPI.ConvertStringToShort(item["DON_VI_TINH"].Text);
                if (don_vi_tinh != null)
                    item["DON_VI"].Text = donViTinhBO.getDonViTinh().FirstOrDefault(x => x.ID == don_vi_tinh.Value).TEN;
                CheckBox chbIS_CHON = (CheckBox)e.Item.FindControl("chbIS_CHON");
                HiddenField hdIS_CHON = (HiddenField)e.Item.FindControl("hdIS_CHON");
                int? IS_CHON = localAPI.ConvertStringToint(hdIS_CHON.Value);
                chbIS_CHON.Checked = IS_CHON != null && IS_CHON == 1 ? true : false;

                #region "Load theo session"
                long id_thuc_pham = Convert.ToInt64(item.GetDataKeyValue("ID").ToString());
                TextBox tbSO_LUONG = (TextBox)item.FindControl("tbSO_LUONG");
                TextBox tbNANG_LUONG_KCAL = (TextBox)item.FindControl("tbNANG_LUONG_KCAL");
                TextBox tbPROTID = (TextBox)item.FindControl("tbPROTID");
                TextBox tbGLUCID = (TextBox)item.FindControl("tbGLUCID");
                TextBox tbLIPID = (TextBox)item.FindControl("tbLIPID");
                if (Session["TableThucPhamSuatAn" + Sys_User.ID] != null)
                {
                    List<SuatAnChiTietEntity> lstSuatAn = (List<SuatAnChiTietEntity>)Session["TableThucPhamSuatAn" + Sys_User.ID];
                    for (int i = 0; i < lstSuatAn.Count; i++)
                    {
                        if (lstSuatAn[i].ID_THUC_PHAM == id_thuc_pham)
                        {
                            chbIS_CHON.Checked = true;
                            tbSO_LUONG.Text = lstSuatAn[i].SO_LUONG != null ? lstSuatAn[i].SO_LUONG.ToString() : "";
                            tbNANG_LUONG_KCAL.Text = lstSuatAn[i].NANG_LUONG_KCAL != null ? lstSuatAn[i].NANG_LUONG_KCAL.ToString() : "";
                            tbPROTID.Text = lstSuatAn[i].PROTID != null ? lstSuatAn[i].PROTID.ToString() : "";
                            tbGLUCID.Text = lstSuatAn[i].GLUCID != null ? lstSuatAn[i].GLUCID.ToString() : "";
                            tbLIPID.Text = lstSuatAn[i].LIPID != null ? lstSuatAn[i].LIPID.ToString() : "";
                        }
                    }
                }
                #endregion
            }
        }
        protected void rcbNhomThucPham_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void rcbKhoi_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbBuaAn.ClearSelection();
            rcbBuaAn.Text = String.Empty;
            rcbBuaAn.DataBind();
            rcbThucDon.ClearSelection();
            rcbThucDon.Text = string.Empty;
            rcbThucDon.DataBind();
        }
        protected void rcbBuaAn_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbThucDon.ClearSelection();
            rcbThucDon.Text = string.Empty;
            rcbThucDon.DataBind();
            long? id_thuc_don = localAPI.ConvertStringTolong(rcbThucDon.SelectedValue);
            RadGrid1.Rebind();
        }
        protected void rcbThucDon_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            long? id_thuc_don = localAPI.ConvertStringTolong(rcbThucDon.SelectedValue);
            RadGrid1.Rebind();
        }
        protected void btTimKiem_Click(object sender, EventArgs e)
        {
            RadGrid1.Rebind();
        }
    }
}