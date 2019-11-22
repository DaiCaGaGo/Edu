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
    public partial class ThemThucPhamThucDon : AuthenticatePage
    {
        long? id_thuc_don;
        short? id_khoi;
        ThucPhamBO thucPhamBO = new ThucPhamBO();
        NhomThucPhamBO nhomThucPhamBO = new NhomThucPhamBO();
        DonViTinhBO donViTinhBO = new DonViTinhBO();
        ThucDonChiTietBO thucDonChiTietBO = new ThucDonChiTietBO();
        ThucDonBO thucDonBO = new ThucDonBO();
        LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["id_thuc_don" + Sys_User.ID] != null) id_thuc_don = localAPI.ConvertStringTolong(Session["id_thuc_don" + Sys_User.ID].ToString());
            if (Session["id_khoi" + Sys_User.ID] != null) id_khoi = localAPI.ConvertStringToShort(Session["id_khoi" + Sys_User.ID].ToString());
            if (!IsPostBack)
            {
                if (id_thuc_don != null) Session["TableThucPhamThucDon" + Sys_User.ID] = null;
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            id_khoi = 0; id_thuc_don = 0;
            if (Session["id_khoi" + Sys_User.ID] != null) id_khoi = localAPI.ConvertStringToShort(Session["id_khoi" + Sys_User.ID].ToString());
            if (Session["id_thuc_don" + Sys_User.ID] != null) id_thuc_don = localAPI.ConvertStringToShort(Session["id_thuc_don" + Sys_User.ID].ToString());
            RadGrid1.DataSource = thucPhamBO.getThucPhamInThucDon(Sys_This_Truong.ID, id_khoi, id_thuc_don.Value, localAPI.ConvertStringToShort(rcbNhomThucPham.SelectedValue), tbTen.Text.Trim());
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
            List<ThucDonChiTietEntity> lst = new List<ThucDonChiTietEntity>();
            if (id_thuc_don == null)
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
                        ThucDonChiTietEntity thucDon = new ThucDonChiTietEntity();
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
                    THUC_DON_CHI_TIET detail = new THUC_DON_CHI_TIET();
                    detail = thucDonChiTietBO.getThucDonChiTietByThucDonTruong(Sys_This_Truong.ID, id_khoi.Value, id_nhom_tp, id_tp, id_thuc_don.Value);
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
                            ThucDonChiTietEntity thucDon = new ThucDonChiTietEntity();
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
                        res = thucDonChiTietBO.insertOrUpdate(detail, Sys_User.ID);
                        if (res.Res) success++; else error++;
                    }
                    else if (detail == null && chbIS_CHON.Checked)
                    {
                        detail = new THUC_DON_CHI_TIET();
                        detail.ID_TRUONG = Sys_This_Truong.ID;
                        detail.ID_KHOI = id_khoi.Value;
                        detail.ID_THUC_DON = id_thuc_don.Value;
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
                        res = thucDonChiTietBO.insertOrUpdate(detail, Sys_User.ID);
                        if (res.Res) success++; else error++;

                        #region add session
                        ThucDonChiTietEntity thucDon = new ThucDonChiTietEntity();
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
                THUC_DON detaiThucDon = new THUC_DON();
                detaiThucDon = thucDonBO.getById(id_thuc_don.Value);
                if (detaiThucDon != null)
                {
                    detaiThucDon.TONG_NANG_LUONG_KCAL = tong_nang_luong;
                    detaiThucDon.TONG_PROTID = tong_protid;
                    detaiThucDon.TONG_GLUCID = tong_glucid;
                    detaiThucDon.TONG_LIPID = tong_lipid;
                    thucDonBO.update(detaiThucDon, Sys_User.ID);
                }
            }
            Session["TableThucPhamThucDon" + Sys_User.ID] = lst;
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
                if (Session["TableThucPhamThucDon" + Sys_User.ID] != null)
                {
                    List<ThucDonChiTietEntity> lstThucDon = (List<ThucDonChiTietEntity>)Session["TableThucPhamThucDon" + Sys_User.ID];
                    for (int i = 0; i < lstThucDon.Count; i++)
                    {
                        if (lstThucDon[i].ID_THUC_PHAM == id_thuc_pham)
                        {
                            chbIS_CHON.Checked = true;
                            tbSO_LUONG.Text = lstThucDon[i].SO_LUONG != null ? lstThucDon[i].SO_LUONG.ToString() : "";
                            tbNANG_LUONG_KCAL.Text = lstThucDon[i].NANG_LUONG_KCAL != null ? lstThucDon[i].NANG_LUONG_KCAL.ToString() : "";
                            tbPROTID.Text = lstThucDon[i].PROTID != null ? lstThucDon[i].PROTID.ToString() : "";
                            tbGLUCID.Text = lstThucDon[i].GLUCID != null ? lstThucDon[i].GLUCID.ToString() : "";
                            tbLIPID.Text = lstThucDon[i].LIPID != null ? lstThucDon[i].LIPID.ToString() : "";
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
        protected void btTimKiem_Click(object sender, EventArgs e)
        {
            RadGrid1.Rebind();
        }
    }
}