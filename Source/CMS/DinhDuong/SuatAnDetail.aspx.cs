using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CMS.DinhDuong
{
    public partial class SuatAnDetail : AuthenticatePage
    {
        long? id_suat_an; short? id_khoi;
        ThucPhamBO thucPhamBO = new ThucPhamBO();
        NhomThucPhamBO nhomThucPhamBO = new NhomThucPhamBO();
        DonViTinhBO donViTinhBO = new DonViTinhBO();
        ThucDonChiTietBO thucDonChiTietBO = new ThucDonChiTietBO();
        ThucDonBO thucDonBO = new ThucDonBO();
        DMBuaAnBO buaAnBO = new DMBuaAnBO();
        SuatAnBO suatAnBO = new SuatAnBO();
        SuatAnChiTietBO suatAnChiTietBO = new SuatAnChiTietBO();
        ChamAnBO chamAnBO = new ChamAnBO();
        LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)

        {
            if (Request.QueryString.Get("id") != null)
            {
                try
                {
                    id_suat_an = Convert.ToInt64(Request.QueryString.Get("id"));
                    id_khoi = Convert.ToInt16(Request.QueryString.Get("id_khoi"));
                }
                catch { }
            }
            Session["KhoiID" + Sys_User.ID] = id_khoi;
            Session["id_suat_an" + Sys_User.ID] = id_suat_an;
            if (!IsPostBack)
            {
                Session["TableThucPhamSuatAn" + Sys_User.ID] = null;
                objKhoi.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                rcbKhoi.DataBind();
                objBuaAn.SelectParameters.Add("id_truong", Sys_This_Truong.ID.ToString());
                rcbBuaAn.DataBind();
                objThucDon.SelectParameters.Add("idTruong", Sys_This_Truong.ID.ToString());
                rcbThucDon.DataBind();
                rdNgay.SelectedDate = DateTime.Now;
                if (id_suat_an != null)
                {
                    SUAT_AN detail = new SUAT_AN();
                    detail = suatAnBO.getSuatAnByID(id_suat_an.Value);
                    if (detail != null)
                    {
                        rcbKhoi.SelectedValue = detail.ID_KHOI.ToString();
                        rcbBuaAn.ClearSelection();
                        rcbBuaAn.Text = String.Empty;
                        rcbBuaAn.DataBind();
                        rcbBuaAn.SelectedValue = detail.ID_BUA_AN.ToString();
                        rcbThucDon.SelectedValue = detail.ID_THUC_DON.ToString();
                        tbSoHocSinhDK.Text = detail.SO_HS_DANG_KY != null ? detail.SO_HS_DANG_KY.ToString() : "";
                        tbGia.Text = detail.HAN_MUC_GIA != null ? detail.HAN_MUC_GIA.ToString() : "";
                        try
                        {
                            rdNgay.SelectedDate = detail.NGAY_AN;
                        }
                        catch { rdNgay.SelectedDate = DateTime.Now; }
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
                    if (rcbKhoi.SelectedValue != "" && rcbBuaAn.SelectedValue != "")
                        getSoDangKyTrongNgay(Convert.ToDateTime(rdNgay.SelectedDate), Convert.ToInt16(rcbKhoi.SelectedValue), Convert.ToInt64(rcbBuaAn.SelectedValue));
                    btEdit.Visible = false;
                    btAdd.Visible = is_access(SYS_Type_Access.SUA);
                }
                Session["ThucDonID" + Sys_User.ID] = rcbThucDon.SelectedValue;
                Session["KhoiID" + Sys_User.ID] = rcbKhoi.SelectedValue;
                Session["BuaAnID" + Sys_User.ID] = rcbBuaAn.SelectedValue;

                getGiaTien(localAPI.ConvertStringTolong(rcbBuaAn.SelectedValue), localAPI.ConvertStringToDecimal(tbSoHocSinhDK.Text.Trim()));
            }
            #region get value hiddenfield
            long? so_hs_dk = localAPI.ConvertStringTolong(tbSoHocSinhDK.Text.Trim());
            long? id_bua_an = localAPI.ConvertStringTolong(rcbBuaAn.SelectedValue);
            if (id_bua_an != null) getValueHidden(so_hs_dk, id_bua_an.Value);
            #endregion

        }
        protected void getSoDangKyTrongNgay(DateTime ngay, short id_khoi, long id_bua_an)
        {
            long? cham_an = chamAnBO.getSoHocSinhByNgayAndBuaAn(Sys_This_Truong.ID, id_khoi, id_bua_an, Sys_Ma_Nam_hoc, ngay.Month, ngay.Day);
            if (cham_an != null) tbSoHocSinhDK.Text = cham_an.Value.ToString();
            else tbSoHocSinhDK.Text = "";
        }
        protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
        {
            if (e.Argument == "RadWindowManager1_Close")
            {
                RadGrid1.Rebind();
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            List<SuatAnChiTietEntity> lst = new List<SuatAnChiTietEntity>();
            if (id_suat_an != null && id_khoi != null && Session["ChonThucDon" + Sys_User.ID] == null)
            {
                Session["TableThucPhamSuatAn" + Sys_User.ID] = suatAnChiTietBO.getSuatAnChiTiet(Sys_This_Truong.ID, id_khoi.Value, id_suat_an.Value);
            }
            #region Add dòng tính tổng
            lst = (List<SuatAnChiTietEntity>)Session["TableThucPhamSuatAn" + Sys_User.ID];
            if (lst != null && lst.Count > 0)
            {
                decimal nang_luong = 0, protid = 0, lipid = 0, glucid = 0;
                for (int i = 0; i < lst.Count; i++)
                {
                    nang_luong += lst[i].NANG_LUONG_KCAL != null ? Convert.ToDecimal(lst[i].NANG_LUONG_KCAL.ToString()) : 0;
                    protid += lst[i].PROTID != null ? Convert.ToDecimal(lst[i].PROTID.ToString()) : 0;
                    lipid += lst[i].LIPID != null ? Convert.ToDecimal(lst[i].LIPID.ToString()) : 0;
                    glucid += lst[i].GLUCID != null ? Convert.ToDecimal(lst[i].GLUCID.ToString()) : 0;
                }
                SuatAnChiTietEntity detail = new SuatAnChiTietEntity();
                detail.NANG_LUONG_KCAL = nang_luong;
                detail.PROTID = protid;
                detail.LIPID = lipid;
                detail.GLUCID = glucid;
                detail.IS_NANG_LUONG_SUAT_AN = 1;
                lst.Add(detail);
                #region Add dòng tính năng lượng chuẩn
                DM_BUA_AN buaAn = new DM_BUA_AN();
                long? so_hs_dk = localAPI.ConvertStringTolong(tbSoHocSinhDK.Text.Trim());
                long? id_bua_an = localAPI.ConvertStringTolong(rcbBuaAn.SelectedValue);
                if (id_bua_an != null) buaAn = buaAnBO.getById(id_bua_an.Value);
                SuatAnChiTietEntity detail1 = new SuatAnChiTietEntity();
                if (buaAn != null && so_hs_dk != null)
                {
                    detail1.NANG_LUONG_KCAL = buaAn.NANG_LUONG_TU_KCAL;
                    detail1.NANG_LUONG_KCAL_OLD = buaAn.NANG_LUONG_DEN_KCAL;
                    detail1.PROTID = buaAn.PROTID_TU_KCAL;
                    detail1.PROTID_OLD = buaAn.PROTID_DEN_KCAL;
                    detail1.LIPID = buaAn.LIPID_TU_KCAL;
                    detail1.LIPID_OLD = buaAn.LIPID_DEN_KCAL;
                    detail1.GLUCID = buaAn.GLUCID_TU_KCAL;
                    detail1.GLUCID_OLD = buaAn.GLUCID_DEN_KCAL;
                    detail1.IS_NANG_LUONG_CHUAN = 1;
                    lst.Add(detail1);
                }
                else
                {
                    detail1.NANG_LUONG_KCAL = 0;
                    detail1.NANG_LUONG_KCAL_OLD = 0;
                    detail1.PROTID = 0;
                    detail1.PROTID_OLD = 0;
                    detail1.LIPID = 0;
                    detail1.LIPID_OLD = 0;
                    detail1.GLUCID = 0;
                    detail1.GLUCID_OLD = 0;
                    detail1.IS_NANG_LUONG_CHUAN = 1;
                    lst.Add(detail1);
                }
                #endregion
                Session["TableThucPhamSuatAn" + Sys_User.ID] = lst;
            }
            #endregion
            RadGrid1.DataSource = (List<SuatAnChiTietEntity>)Session["TableThucPhamSuatAn" + Sys_User.ID];
            Session["ChonThucDon" + Sys_User.ID] = null;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
        protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            long? so_hs_dk = localAPI.ConvertStringTolong(tbSoHocSinhDK.Text.Trim());
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                TextBox tbNANG_LUONG_KCAL = (TextBox)item.FindControl("tbNANG_LUONG_KCAL");
                TextBox tbPROTID = (TextBox)item.FindControl("tbPROTID");
                TextBox tbGLUCID = (TextBox)item.FindControl("tbGLUCID");
                TextBox tbLIPID = (TextBox)item.FindControl("tbLIPID");
                TextBox tbNANG_LUONG_KCAL_OLD = (TextBox)item.FindControl("tbNANG_LUONG_KCAL_OLD");
                TextBox tbPROTID_OLD = (TextBox)item.FindControl("tbPROTID_OLD");
                TextBox tbGLUCID_OLD = (TextBox)item.FindControl("tbGLUCID_OLD");
                TextBox tbLIPID_OLD = (TextBox)item.FindControl("tbLIPID_OLD");
                #region năng lượng chuẩn
                int? is_nang_luong_chuan = localAPI.ConvertStringToint(item["IS_NANG_LUONG_CHUAN"].Text);
                if (is_nang_luong_chuan != null && is_nang_luong_chuan == 1)
                {

                    decimal? nang_luong_tu = localAPI.ConvertStringToDecimal(tbNANG_LUONG_KCAL.Text) * so_hs_dk;
                    decimal? nang_luong_den = localAPI.ConvertStringToDecimal(tbNANG_LUONG_KCAL_OLD.Text) * so_hs_dk;
                    decimal? protid_tu = localAPI.ConvertStringToDecimal(tbPROTID.Text) * so_hs_dk;
                    decimal? protid_den = localAPI.ConvertStringToDecimal(tbPROTID_OLD.Text) * so_hs_dk;
                    decimal? glucid_tu = localAPI.ConvertStringToDecimal(tbGLUCID.Text) * so_hs_dk;
                    decimal? glucid_den = localAPI.ConvertStringToDecimal(tbGLUCID_OLD.Text) * so_hs_dk;
                    decimal? lipid_tu = localAPI.ConvertStringToDecimal(tbLIPID.Text) * so_hs_dk;
                    decimal? lipid_den = localAPI.ConvertStringToDecimal(tbLIPID_OLD.Text) * so_hs_dk;

                    tbNANG_LUONG_KCAL.Text = (nang_luong_tu != null ? nang_luong_tu.Value.ToString() : "") + " - " + (nang_luong_den != null ? nang_luong_den.ToString() : "");
                    tbPROTID.Text = (protid_tu != null ? protid_tu.Value.ToString() : "") + " - " + (protid_den != null ? protid_den.Value.ToString() : "");
                    tbGLUCID.Text = (glucid_tu != null ? glucid_tu.Value.ToString() : "") + " - " + (glucid_den != null ? glucid_den.Value.ToString() : "");
                    tbLIPID.Text = (lipid_tu != null ? lipid_tu.Value.ToString() : "") + " - " + (lipid_den != null ? lipid_den.Value.ToString() : "");
                    item["chkChon"].Text = "";
                    hdNangLuongTu.Value = nang_luong_tu != null ? nang_luong_tu.ToString() : "0";
                    hdNangLuongDen.Value = nang_luong_den != null ? nang_luong_den.ToString() : "0";
                    hdProtidTu.Value = protid_tu != null ? protid_tu.ToString() : "0";
                    hdProtidDen.Value = protid_den != null ? protid_den.ToString() : "0";
                    hdGlucidTu.Value = glucid_tu != null ? glucid_tu.ToString() : "0";
                    hdGlucidDen.Value = glucid_den != null ? glucid_den.ToString() : "0";
                    hdLipidTu.Value = lipid_tu != null ? lipid_tu.ToString() : "0";
                    hdLipidDen.Value = lipid_den != null ? lipid_den.ToString() : "0";
                }
                #endregion
                #region năng lượng thực đơn

                int? is_nang_luong_thuc_don = localAPI.ConvertStringToint(item["IS_NANG_LUONG_SUAT_AN"].Text);
                if (is_nang_luong_thuc_don != null && is_nang_luong_thuc_don == 1)
                {
                    List<SuatAnChiTietEntity> lst = (List<SuatAnChiTietEntity>)Session["TableThucPhamSuatAn" + Sys_User.ID];
                    int i = lst.Count;
                    if (i > 0)
                    {
                        decimal? nang_luong_tu = lst[i - 1].NANG_LUONG_KCAL != null ? (localAPI.ConvertStringToDecimal(lst[i - 1].NANG_LUONG_KCAL.ToString()) * so_hs_dk) : null;
                        decimal? nang_luong_den = lst[i - 1].NANG_LUONG_KCAL_OLD != null ? (localAPI.ConvertStringToDecimal(lst[i - 1].NANG_LUONG_KCAL_OLD.ToString()) * so_hs_dk) : null;
                        decimal? protid_tu = lst[i - 1].PROTID != null ? (localAPI.ConvertStringToDecimal(lst[i - 1].PROTID.ToString()) * so_hs_dk) : null;
                        decimal? protid_den = lst[i - 1].PROTID_OLD != null ? (localAPI.ConvertStringToDecimal(lst[i - 1].PROTID_OLD.ToString()) * so_hs_dk) : null;
                        decimal? glucid_tu = lst[i - 1].GLUCID != null ? (localAPI.ConvertStringToDecimal(lst[i - 1].GLUCID.ToString()) * so_hs_dk) : null;
                        decimal? glucid_den = lst[i - 1].GLUCID_OLD != null ? (localAPI.ConvertStringToDecimal(lst[i - 1].GLUCID_OLD.ToString()) * so_hs_dk) : null;
                        decimal? lipid_tu = lst[i - 1].LIPID != null ? (localAPI.ConvertStringToDecimal(lst[i - 1].LIPID.ToString()) * so_hs_dk) : null;
                        decimal? lipid_den = lst[i - 1].LIPID_OLD != null ? (localAPI.ConvertStringToDecimal(lst[i - 1].LIPID_OLD.ToString()) * so_hs_dk) : null;

                        decimal? nang_luong = localAPI.ConvertStringToDecimal(tbNANG_LUONG_KCAL.Text.Trim());
                        decimal? protid = localAPI.ConvertStringToDecimal(tbPROTID.Text.Trim());
                        decimal? glucid = localAPI.ConvertStringToDecimal(tbGLUCID.Text.Trim());
                        decimal? lipid = localAPI.ConvertStringToDecimal(tbLIPID.Text.Trim());
                        if (nang_luong < nang_luong_tu || nang_luong > nang_luong_den)
                            tbNANG_LUONG_KCAL.ForeColor = Color.Red;
                        if (protid < protid_tu || protid > protid_den)
                            tbPROTID.ForeColor = Color.Red;
                        if (glucid < glucid_tu || glucid > glucid_den)
                            tbGLUCID.ForeColor = Color.Red;
                        if (lipid < lipid_tu || lipid > lipid_den)
                            tbLIPID.ForeColor = Color.Red;
                    }
                }
                #endregion
                short? id_nhom_tp = localAPI.ConvertStringToShort(item["ID_NHOM_THUC_PHAM"].Text);
                if (id_nhom_tp != null && id_nhom_tp > 0)
                    item["TEN_NHOM_THUC_PHAM"].Text = nhomThucPhamBO.getNhomThucPham("").FirstOrDefault(x => x.ID == id_nhom_tp).TEN;
                long? id_thuc_pham = localAPI.ConvertStringTolong(item["ID_THUC_PHAM"].Text);
                if (id_thuc_pham != null && id_thuc_pham > 0)
                    item["TEN_THUC_PHAM"].Text = thucPhamBO.getThucPham(id_nhom_tp, "").FirstOrDefault(x => x.ID == id_thuc_pham.Value).TEN;
                short? don_vi_tinh = localAPI.ConvertStringToShort(item["DON_VI_TINH"].Text);
                if (don_vi_tinh != null && don_vi_tinh > 0)
                    item["DON_VI"].Text = donViTinhBO.getDonViTinh().FirstOrDefault(x => x.ID == don_vi_tinh).TEN;
                if (id_nhom_tp == 0 && id_thuc_pham == 0)
                {
                    item["chkChon"].Text = "";
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
            SUAT_AN checkExists = suatAnBO.checkExists(Sys_This_Truong.ID, Convert.ToInt16(rcbKhoi.SelectedValue), Convert.ToInt64(rcbBuaAn.SelectedValue), Convert.ToDateTime(rdNgay.SelectedDate));
            if (checkExists != null)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Thực đơn này đã tồn tại. Vui lòng kiểm tra lại!');", true);
                return;
            }
            #endregion
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            #region Thêm thực phẩm cho thực đơn
            List<SuatAnChiTietEntity> lstNew = new List<SuatAnChiTietEntity>();
            if (RadGrid1.Items.Count > 0)
            {
                SUAT_AN detail = new SUAT_AN();
                detail.ID_TRUONG = Sys_This_Truong.ID;
                detail.ID_KHOI = Convert.ToInt16(rcbKhoi.SelectedValue);
                detail.ID_NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
                detail.ID_BUA_AN = Convert.ToInt64(rcbBuaAn.SelectedValue);
                detail.ID_THUC_DON = localAPI.ConvertStringTolong(rcbThucDon.SelectedValue);
                detail.NGAY_AN = Convert.ToDateTime(rdNgay.SelectedDate);
                detail.SO_HS_DANG_KY = localAPI.ConvertStringTolong(tbSoHocSinhDK.Text.Trim());
                detail.HAN_MUC_GIA = localAPI.ConvertStringToDecimal(tbGia.Text.Trim());
                res = suatAnBO.insert(detail, Sys_User.ID);
                if (res.Res)
                {
                    SUAT_AN resSuatAn = (SUAT_AN)res.ResObject;
                    id_suat_an = resSuatAn.ID;
                    Session["id_suat_an" + Sys_User.ID] = resSuatAn.ID;
                    Session["KhoiID" + Sys_User.ID] = Convert.ToInt16(rcbKhoi.SelectedValue);
                    #region insert detail
                    decimal? tong_nang_luong = 0, tong_protid = 0, tong_lipid = 0, tong_glucid = 0;
                    int so_dong_cap_nhat = 0;
                    foreach (GridDataItem row in RadGrid1.Items)
                    {
                        so_dong_cap_nhat++;
                        if (so_dong_cap_nhat > RadGrid1.Items.Count - 2) break;
                        short id_nhom_tp = Convert.ToInt16(row["ID_NHOM_THUC_PHAM"].Text);
                        long id_tp = Convert.ToInt64(row["ID_THUC_PHAM"].Text);
                        short? don_vi_tinh = localAPI.ConvertStringToShort(row["DON_VI_TINH"].Text);
                        #region get control
                        TextBox tbSO_LUONG = (TextBox)row.FindControl("tbSO_LUONG");
                        TextBox tbNANG_LUONG_KCAL = (TextBox)row.FindControl("tbNANG_LUONG_KCAL");
                        TextBox tbPROTID = (TextBox)row.FindControl("tbPROTID");
                        TextBox tbGLUCID = (TextBox)row.FindControl("tbGLUCID");
                        TextBox tbLIPID = (TextBox)row.FindControl("tbLIPID");

                        TextBox tbNANG_LUONG_KCAL_OLD = (TextBox)row.FindControl("tbNANG_LUONG_KCAL_OLD");
                        TextBox tbPROTID_OLD = (TextBox)row.FindControl("tbPROTID_OLD");
                        TextBox tbGLUCID_OLD = (TextBox)row.FindControl("tbGLUCID_OLD");
                        TextBox tbLIPID_OLD = (TextBox)row.FindControl("tbLIPID_OLD");

                        string so_luong = tbSO_LUONG.Text.Trim();
                        string nang_luong = tbNANG_LUONG_KCAL.Text.Trim();
                        string protid = tbPROTID.Text.Trim();
                        string glucid = tbGLUCID.Text.Trim();
                        string lipid = tbLIPID.Text.Trim();

                        string nang_luong_old = tbNANG_LUONG_KCAL_OLD.Text.Trim();
                        string protid_old = tbPROTID_OLD.Text.Trim();
                        string glucid_old = tbGLUCID_OLD.Text.Trim();
                        string lipid_old = tbLIPID_OLD.Text.Trim();
                        #endregion
                        tong_nang_luong += localAPI.ConvertStringToDecimal(nang_luong);
                        tong_protid += localAPI.ConvertStringToDecimal(protid);
                        tong_lipid += localAPI.ConvertStringToDecimal(lipid);
                        tong_glucid += localAPI.ConvertStringToDecimal(glucid);
                        SUAT_AN_CHI_TIET tdDetail = suatAnChiTietBO.getSuatAnChiTietByTruongAndSuatAn(Sys_This_Truong.ID, Convert.ToInt16(rcbKhoi.SelectedValue), id_nhom_tp, id_tp, resSuatAn.ID);
                        if (tdDetail != null)
                        {
                            tdDetail.ID_NHOM_THUC_PHAM = id_nhom_tp;
                            tdDetail.ID_THUC_PHAM = id_tp;
                            tdDetail.DON_VI_TINH = don_vi_tinh;
                            tdDetail.SO_LUONG = localAPI.ConvertStringToDecimal(so_luong);
                            tdDetail.NANG_LUONG_KCAL = localAPI.ConvertStringToDecimal(nang_luong);
                            tdDetail.PROTID = localAPI.ConvertStringToDecimal(protid);
                            tdDetail.LIPID = localAPI.ConvertStringToDecimal(lipid);
                            tdDetail.GLUCID = localAPI.ConvertStringToDecimal(glucid);
                            res = suatAnChiTietBO.update(tdDetail, Sys_User.ID);
                        }
                        else
                        {
                            tdDetail = new SUAT_AN_CHI_TIET();
                            tdDetail.ID_TRUONG = Sys_This_Truong.ID;
                            tdDetail.ID_KHOI = Convert.ToInt16(rcbKhoi.SelectedValue);
                            tdDetail.ID_SUAT_AN = resSuatAn.ID;
                            tdDetail.ID_NHOM_THUC_PHAM = id_nhom_tp;
                            tdDetail.ID_THUC_PHAM = id_tp;
                            tdDetail.DON_VI_TINH = don_vi_tinh;
                            tdDetail.SO_LUONG = localAPI.ConvertStringToDecimal(so_luong);
                            tdDetail.NANG_LUONG_KCAL = localAPI.ConvertStringToDecimal(nang_luong);
                            tdDetail.PROTID = localAPI.ConvertStringToDecimal(protid);
                            tdDetail.LIPID = localAPI.ConvertStringToDecimal(lipid);
                            tdDetail.GLUCID = localAPI.ConvertStringToDecimal(glucid);
                            res = suatAnChiTietBO.insert(tdDetail, Sys_User.ID);
                        }
                        #region set Session
                        addThucDonInList(id_suat_an.Value, id_nhom_tp, id_tp, don_vi_tinh, localAPI.ConvertStringToDecimal(so_luong),
                            localAPI.ConvertStringToDecimal(nang_luong), localAPI.ConvertStringToDecimal(protid),
                            localAPI.ConvertStringToDecimal(lipid), localAPI.ConvertStringToDecimal(glucid),
                            localAPI.ConvertStringToDecimal(nang_luong_old), localAPI.ConvertStringToDecimal(protid_old), localAPI.ConvertStringToDecimal(lipid_old), localAPI.ConvertStringToDecimal(glucid_old), lstNew);
                        #endregion
                    }
                    #endregion
                    #region update nang luong suat an
                    if (tong_nang_luong > 0 || tong_protid > 0 || tong_lipid > 0 || tong_glucid > 0)
                    {
                        resSuatAn.TONG_NANG_LUONG_KCAL = tong_nang_luong;
                        resSuatAn.TONG_PROTID = tong_protid;
                        resSuatAn.TONG_GLUCID = tong_glucid;
                        resSuatAn.TONG_LIPID = tong_lipid;
                        suatAnBO.update(resSuatAn, Sys_User.ID);
                    }
                    #endregion
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn vui lòng thêm thực phẩm cho thực đơn!');", true);
                return;
            }
            #endregion
            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
                btEdit.Visible = is_access(SYS_Type_Access.SUA);
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            Session["TableThucPhamSuatAn" + Sys_User.ID] = lstNew;
            RadGrid1.Rebind();
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
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            int success = 0, error = 0;
            List<SuatAnChiTietEntity> lstNew = new List<SuatAnChiTietEntity>();
            SUAT_AN detail = suatAnBO.getSuatAnByID(id_suat_an.Value);
            if (detail != null)
            {
                detail.ID_KHOI = Convert.ToInt16(rcbKhoi.SelectedValue);
                detail.ID_NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
                detail.ID_BUA_AN = Convert.ToInt64(rcbBuaAn.SelectedValue);
                detail.ID_THUC_DON = localAPI.ConvertStringTolong(rcbThucDon.SelectedValue);
                detail.NGAY_AN = Convert.ToDateTime(rdNgay.SelectedDate);
                detail.SO_HS_DANG_KY = localAPI.ConvertStringTolong(tbSoHocSinhDK.Text.Trim());
                detail.HAN_MUC_GIA = localAPI.ConvertStringToDecimal(tbGia.Text.Trim());
                #region update thực đơn chi tiết
                decimal? tong_nang_luong = 0, tong_protid = 0, tong_lipid = 0, tong_glucid = 0;
                int so_dong_cap_nhat = 0;
                foreach (GridDataItem row in RadGrid1.Items)
                {
                    so_dong_cap_nhat++;
                    if (so_dong_cap_nhat > RadGrid1.Items.Count - 2) break;
                    short id_nhom_tp = Convert.ToInt16(row["ID_NHOM_THUC_PHAM"].Text);
                    long id_tp = Convert.ToInt64(row["ID_THUC_PHAM"].Text);
                    short? don_vi_tinh = localAPI.ConvertStringToShort(row["DON_VI_TINH"].Text);
                    #region get control
                    TextBox tbSO_LUONG = (TextBox)row.FindControl("tbSO_LUONG");
                    TextBox tbNANG_LUONG_KCAL = (TextBox)row.FindControl("tbNANG_LUONG_KCAL");
                    TextBox tbPROTID = (TextBox)row.FindControl("tbPROTID");
                    TextBox tbGLUCID = (TextBox)row.FindControl("tbGLUCID");
                    TextBox tbLIPID = (TextBox)row.FindControl("tbLIPID");

                    TextBox tbNANG_LUONG_KCAL_OLD = (TextBox)row.FindControl("tbNANG_LUONG_KCAL_OLD");
                    TextBox tbPROTID_OLD = (TextBox)row.FindControl("tbPROTID_OLD");
                    TextBox tbGLUCID_OLD = (TextBox)row.FindControl("tbGLUCID_OLD");
                    TextBox tbLIPID_OLD = (TextBox)row.FindControl("tbLIPID_OLD");

                    string so_luong = tbSO_LUONG.Text.Trim();
                    string nang_luong = tbNANG_LUONG_KCAL.Text.Trim();
                    string protid = tbPROTID.Text.Trim();
                    string glucid = tbGLUCID.Text.Trim();
                    string lipid = tbLIPID.Text.Trim();

                    string nang_luong_old = tbNANG_LUONG_KCAL_OLD.Text.Trim();
                    string protid_old = tbPROTID_OLD.Text.Trim();
                    string glucid_old = tbGLUCID_OLD.Text.Trim();
                    string lipid_old = tbLIPID_OLD.Text.Trim();
                    #endregion

                    tong_nang_luong += !string.IsNullOrEmpty(nang_luong) ? localAPI.ConvertStringToDecimal(nang_luong) : 0;
                    tong_protid += !string.IsNullOrEmpty(protid) ? localAPI.ConvertStringToDecimal(protid) : 0;
                    tong_lipid += !string.IsNullOrEmpty(lipid) ? localAPI.ConvertStringToDecimal(lipid) : 0;
                    tong_glucid += !string.IsNullOrEmpty(glucid) ? localAPI.ConvertStringToDecimal(glucid) : 0;
                    SUAT_AN_CHI_TIET tdDetail = suatAnChiTietBO.getSuatAnChiTietByTruongAndSuatAn(Sys_This_Truong.ID, Convert.ToInt16(rcbKhoi.SelectedValue), id_nhom_tp, id_tp, id_suat_an.Value);
                    if (tdDetail != null)
                    {
                        tdDetail.ID_NHOM_THUC_PHAM = id_nhom_tp;
                        tdDetail.ID_THUC_PHAM = id_tp;
                        tdDetail.SO_LUONG = localAPI.ConvertStringToDecimal(so_luong);
                        tdDetail.NANG_LUONG_KCAL = localAPI.ConvertStringToDecimal(nang_luong);
                        tdDetail.PROTID = localAPI.ConvertStringToDecimal(protid);
                        tdDetail.LIPID = localAPI.ConvertStringToDecimal(lipid);
                        tdDetail.GLUCID = localAPI.ConvertStringToDecimal(glucid);
                        tdDetail.DON_VI_TINH = don_vi_tinh;
                        res = suatAnChiTietBO.update(tdDetail, Sys_User.ID);
                        if (res.Res) success++; else error++;
                    }
                    else
                    {
                        tdDetail = new SUAT_AN_CHI_TIET();
                        tdDetail.ID_TRUONG = Sys_This_Truong.ID;
                        tdDetail.ID_KHOI = Convert.ToInt16(rcbKhoi.SelectedValue);
                        tdDetail.ID_SUAT_AN = id_suat_an.Value;
                        tdDetail.ID_NHOM_THUC_PHAM = id_nhom_tp;
                        tdDetail.ID_THUC_PHAM = id_tp;
                        tdDetail.SO_LUONG = localAPI.ConvertStringToDecimal(so_luong);
                        tdDetail.NANG_LUONG_KCAL = localAPI.ConvertStringToDecimal(nang_luong);
                        tdDetail.PROTID = localAPI.ConvertStringToDecimal(protid);
                        tdDetail.LIPID = localAPI.ConvertStringToDecimal(lipid);
                        tdDetail.GLUCID = localAPI.ConvertStringToDecimal(glucid);
                        tdDetail.DON_VI_TINH = don_vi_tinh;
                        res = suatAnChiTietBO.insert(tdDetail, Sys_User.ID);
                        if (res.Res) success++; else error++;
                    }
                    #region set Session
                    addThucDonInList(id_suat_an.Value, id_nhom_tp, id_tp, don_vi_tinh, localAPI.ConvertStringToDecimal(so_luong),
                        localAPI.ConvertStringToDecimal(nang_luong), localAPI.ConvertStringToDecimal(protid),
                        localAPI.ConvertStringToDecimal(lipid), localAPI.ConvertStringToDecimal(glucid),
                        localAPI.ConvertStringToDecimal(nang_luong_old), localAPI.ConvertStringToDecimal(protid_old), localAPI.ConvertStringToDecimal(lipid_old), localAPI.ConvertStringToDecimal(glucid_old), lstNew);
                    #endregion
                }
                #endregion
                #region Tính tổng năng lượng
                if (tong_nang_luong > 0 || tong_protid > 0 || tong_lipid > 0 || tong_glucid > 0)
                {
                    detail.TONG_NANG_LUONG_KCAL = tong_nang_luong;
                    detail.TONG_PROTID = tong_protid;
                    detail.TONG_GLUCID = tong_glucid;
                    detail.TONG_LIPID = tong_lipid;
                }
                #endregion
                res = suatAnBO.update(detail, Sys_User.ID);
                if (res.Res) success++; else error++;
            }
            string strMsg = "";
            if (success > 0)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
            }
            if (error > 0)
            {
                strMsg = "notification('error', 'Có " + error + " bản ghi không được cập nhật');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            Session["TableThucPhamThucDon" + Sys_User.ID] = lstNew;
            RadGrid1.Rebind();
        }
        protected void rcbKhoi_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbBuaAn.ClearSelection();
            rcbBuaAn.Text = String.Empty;
            rcbBuaAn.DataBind();
            if (rcbKhoi.SelectedValue != "" && rcbBuaAn.SelectedValue != "") getSoDangKyTrongNgay(Convert.ToDateTime(rdNgay.SelectedDate), Convert.ToInt16(rcbKhoi.SelectedValue), Convert.ToInt64(rcbBuaAn.SelectedValue));
            RadGrid1.Rebind();
        }
        protected void rcbBuaAn_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbThucDon.ClearSelection();
            rcbThucDon.Text = string.Empty;
            rcbThucDon.DataBind();
            decimal? so_hs_dk = localAPI.ConvertStringToDecimal(tbSoHocSinhDK.Text.Trim());
            getGiaTien(localAPI.ConvertStringTolong(rcbBuaAn.SelectedValue), so_hs_dk);
            getNangLuong(so_hs_dk);
            if (rcbKhoi.SelectedValue != "" && rcbBuaAn.SelectedValue != "") getSoDangKyTrongNgay(Convert.ToDateTime(rdNgay.SelectedDate), Convert.ToInt16(rcbKhoi.SelectedValue), Convert.ToInt64(rcbBuaAn.SelectedValue));
            RadGrid1.Rebind();
        }
        protected void btDeleteChon_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.XOA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            int success = 0, error = 0;
            if (id_suat_an != null)
            {
                SUAT_AN detaiTD = suatAnBO.getSuatAnByID(id_suat_an.Value);
                foreach (GridDataItem row in RadGrid1.SelectedItems)
                {
                    short id_nhom_tp = Convert.ToInt16(row["ID_NHOM_THUC_PHAM"].Text);
                    long id_tp = Convert.ToInt64(row["ID_THUC_PHAM"].Text);
                    decimal? nang_luong = 0, protid = 0, glucid = 0, lipid = 0;
                    nang_luong = localAPI.ConvertStringToDecimal(row["NANG_LUONG_KCAL"].Text);
                    protid = localAPI.ConvertStringToDecimal(row["PROTID"].Text);
                    glucid = localAPI.ConvertStringToDecimal(row["GLUCID"].Text);
                    lipid = localAPI.ConvertStringToDecimal(row["LIPID"].Text);
                    if (id_nhom_tp > 0 && id_tp > 0)
                    {
                        res = suatAnChiTietBO.deleteThucPhamInSuatAn(Sys_This_Truong.ID, id_suat_an.Value, id_nhom_tp, id_tp, Sys_User.ID, true);
                        #region update thanh phan dinh duong
                        detaiTD.TONG_NANG_LUONG_KCAL -= nang_luong;
                        detaiTD.TONG_PROTID -= protid;
                        detaiTD.TONG_GLUCID -= glucid;
                        detaiTD.TONG_LIPID -= lipid;
                        suatAnBO.update(detaiTD, Sys_User.ID);
                        #endregion
                        if (res.Res) success++;
                        else error++;
                    }
                }
            }
            else
            {
                List<SuatAnChiTietEntity> lst = (List<SuatAnChiTietEntity>)Session["TableThucPhamSuatAn" + Sys_User.ID];
                if (lst != null && lst.Count > 0)
                {
                    foreach (GridDataItem row in RadGrid1.SelectedItems)
                    {
                        long id_thuc_pham = Convert.ToInt64(row["ID_THUC_PHAM"].Text);
                        for (int i = 0; i < lst.Count; i++)
                        {
                            SuatAnChiTietEntity detail = lst[i];
                            if (Convert.ToInt64(detail.ID_THUC_PHAM) == id_thuc_pham && i < lst.Count - 1)
                            {
                                lst.Remove(detail);
                                success++;
                                break;
                            }
                        }
                    }
                    if (lst.Count >= 2)
                    {
                        lst.Remove(lst[lst.Count - 2]);
                        lst.Remove(lst[lst.Count - 1]);
                    }
                    else if (lst.Count == 1 && lst[0].ID_THUC_PHAM == 0) lst.Remove(lst[0]);
                    Session["TableThucPhamSuatAn" + Sys_User.ID] = lst;
                }
            }
            string strMsg = "";
            if (success > 0)
            {
                strMsg = "notification('success', 'Có " + success + " bản ghi đã bị xóa');";
            }
            else
            {
                strMsg = "notification('warning', 'Không có bản ghi nào được xóa');";
            }
            if (error > 0)
            {
                strMsg = "notification('error', 'Có lỗi xảy ra, vui lòng liên hệ với quản trị viên!');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
        }
        protected void rcbThucDon_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            Session["ChonThucDon" + Sys_User.ID] = null;
            Session["TableThucPhamSuatAn" + Sys_User.ID] = null;
            if (!string.IsNullOrEmpty(rcbKhoi.SelectedValue) && !string.IsNullOrEmpty(rcbThucDon.SelectedValue))
            {
                List<SuatAnChiTietEntity> lstSuatAn = new List<SuatAnChiTietEntity>();
                List<THUC_DON_CHI_TIET> lstThucDon = thucDonChiTietBO.getThucDonChiTiet(Sys_This_Truong.ID, Convert.ToInt16(rcbKhoi.SelectedValue), Convert.ToInt64(rcbThucDon.SelectedValue));
                if (lstThucDon.Count > 0)
                {
                    Session["ChonThucDon" + Sys_User.ID] = 1;
                    for (int i = 0; i < lstThucDon.Count; i++)
                    {
                        SuatAnChiTietEntity detail = new SuatAnChiTietEntity();
                        detail.ID_NHOM_THUC_PHAM = lstThucDon[i].ID_NHOM_THUC_PHAM;
                        detail.ID_THUC_PHAM = lstThucDon[i].ID_THUC_PHAM;
                        detail.SO_LUONG = lstThucDon[i].SO_LUONG;
                        detail.NANG_LUONG_KCAL = lstThucDon[i].NANG_LUONG_KCAL;
                        detail.PROTID = lstThucDon[i].PROTID;
                        detail.GLUCID = lstThucDon[i].GLUCID;
                        detail.LIPID = lstThucDon[i].LIPID;
                        DM_THUC_PHAM thucPham = thucPhamBO.getThucPhamByNhomAndID(lstThucDon[i].ID_NHOM_THUC_PHAM, lstThucDon[i].ID_THUC_PHAM);
                        if (thucPham != null)
                        {
                            detail.NANG_LUONG_KCAL_OLD = thucPham.NANG_LUONG_KCAL;
                            detail.PROTID_OLD = thucPham.PROTID;
                            detail.GLUCID_OLD = thucPham.GLUCID;
                            detail.LIPID_OLD = thucPham.LIPID;
                        }
                        lstSuatAn.Add(detail);
                    }
                    Session["TableThucPhamSuatAn" + Sys_User.ID] = lstSuatAn;
                }

                Session["ThucDonID" + Sys_User.ID] = rcbThucDon.SelectedValue;
                Session["KhoiID" + Sys_User.ID] = rcbKhoi.SelectedValue;
                Session["BuaAnID" + Sys_User.ID] = rcbBuaAn.SelectedValue;
            }
            RadGrid1.Rebind();
        }
        protected void tbSoHocSinhDK_TextChanged(object sender, EventArgs e)
        {
            decimal? so_hs_dk = localAPI.ConvertStringToDecimal(tbSoHocSinhDK.Text.Trim());
            getGiaTien(localAPI.ConvertStringTolong(rcbBuaAn.SelectedValue), so_hs_dk);
            getNangLuong(so_hs_dk);
            RadGrid1.Rebind();
        }
        protected void getGiaTien(long? id_bua_an, decimal? so_hoc_sinh_dk)
        {
            if (id_bua_an != null && so_hoc_sinh_dk != null)
            {
                DM_BUA_AN detai = buaAnBO.getBuaAnByTruongKhoi(Sys_This_Truong.ID, localAPI.ConvertStringToShort(rcbKhoi.SelectedValue)).FirstOrDefault(x => x.ID == id_bua_an.Value);
                if (detai != null) hdfSoTien1HS.Value = detai.GIA_TIEN != null ? detai.GIA_TIEN.Value.ToString() : "";
                tbGia.Text = so_hoc_sinh_dk > 0 && hdfSoTien1HS.Value != "" ? (so_hoc_sinh_dk * (Convert.ToDecimal(hdfSoTien1HS.Value))).ToString() : "";
            }
            else tbGia.Text = "";
        }
        protected void getNangLuong(decimal? so_hs_dk)
        {
            int count = 0;
            List<SuatAnChiTietEntity> lst = (List<SuatAnChiTietEntity>)Session["TableThucPhamSuatAn" + Sys_User.ID];
            if (lst != null && lst.Count > 0)
            {
                int so_dong = lst.Count;
                for (int i = 0; i < so_dong; i++)
                {
                    lst.Remove(lst[0]);
                }
            }
            Session["TableThucPhamSuatAn" + Sys_User.ID] = lst;
            foreach (GridDataItem item in RadGrid1.Items)
            {
                TextBox tbSO_LUONG = (TextBox)item.FindControl("tbSO_LUONG");
                TextBox tbNANG_LUONG_KCAL = (TextBox)item.FindControl("tbNANG_LUONG_KCAL");
                TextBox tbPROTID = (TextBox)item.FindControl("tbPROTID");
                TextBox tbGLUCID = (TextBox)item.FindControl("tbGLUCID");
                TextBox tbLIPID = (TextBox)item.FindControl("tbLIPID");
                TextBox tbNANG_LUONG_KCAL_OLD = (TextBox)item.FindControl("tbNANG_LUONG_KCAL_OLD");
                TextBox tbPROTID_OLD = (TextBox)item.FindControl("tbPROTID_OLD");
                TextBox tbGLUCID_OLD = (TextBox)item.FindControl("tbGLUCID_OLD");
                TextBox tbLIPID_OLD = (TextBox)item.FindControl("tbLIPID_OLD");

                string so_luong = tbSO_LUONG.Text.Trim();
                string nang_luong = tbNANG_LUONG_KCAL.Text.Trim();
                string protid = tbPROTID.Text.Trim();
                string glucid = tbGLUCID.Text.Trim();
                string lipid = tbLIPID.Text.Trim();

                string nang_luong_old = tbNANG_LUONG_KCAL_OLD.Text.Trim();
                string protid_old = tbPROTID_OLD.Text.Trim();
                string glucid_old = tbGLUCID_OLD.Text.Trim();
                string lipid_old = tbLIPID_OLD.Text.Trim();

                short id_nhom_tp = Convert.ToInt16(item["ID_NHOM_THUC_PHAM"].Text);
                long id_tp = Convert.ToInt64(item["ID_THUC_PHAM"].Text);
                short? don_vi_tinh = localAPI.ConvertStringToShort(item["DON_VI_TINH"].Text);
                count++;
                if (count == RadGrid1.Items.Count - 1)
                {
                    decimal? nang_luong_tu = localAPI.ConvertStringToDecimal(tbNANG_LUONG_KCAL.Text) * so_hs_dk;
                    decimal? nang_luong_den = localAPI.ConvertStringToDecimal(tbNANG_LUONG_KCAL_OLD.Text) * so_hs_dk;
                    decimal? protid_tu = localAPI.ConvertStringToDecimal(tbPROTID.Text) * so_hs_dk;
                    decimal? protid_den = localAPI.ConvertStringToDecimal(tbPROTID_OLD.Text) * so_hs_dk;
                    decimal? glucid_tu = localAPI.ConvertStringToDecimal(tbGLUCID.Text) * so_hs_dk;
                    decimal? glucid_den = localAPI.ConvertStringToDecimal(tbGLUCID_OLD.Text) * so_hs_dk;
                    decimal? lipid_tu = localAPI.ConvertStringToDecimal(tbLIPID.Text) * so_hs_dk;
                    decimal? lipid_den = localAPI.ConvertStringToDecimal(tbLIPID_OLD.Text) * so_hs_dk;

                    tbNANG_LUONG_KCAL.Text = (nang_luong_tu != null ? nang_luong_tu.Value.ToString() : "") + " - " + (nang_luong_den != null ? nang_luong_den.ToString() : "");
                    tbPROTID.Text = (protid_tu != null ? protid_tu.Value.ToString() : "") + " - " + (protid_den != null ? protid_den.Value.ToString() : "");
                    tbGLUCID.Text = (glucid_tu != null ? glucid_tu.Value.ToString() : "") + " - " + (glucid_den != null ? glucid_den.Value.ToString() : "");
                    tbLIPID.Text = (lipid_tu != null ? lipid_tu.Value.ToString() : "") + " - " + (lipid_den != null ? lipid_den.Value.ToString() : "");
                    break;
                }
                #region reset Session
                SuatAnChiTietEntity tdEntity = new SuatAnChiTietEntity();
                if (id_suat_an != null) tdEntity.ID_SUAT_AN = id_suat_an.Value;
                else tdEntity.ID_SUAT_AN = 0;
                tdEntity.ID_NHOM_THUC_PHAM = id_nhom_tp;
                tdEntity.ID_THUC_PHAM = id_tp;
                tdEntity.ID_TRUONG = Sys_This_Truong.ID;
                tdEntity.ID_KHOI = Convert.ToInt16(rcbKhoi.SelectedValue);
                tdEntity.DON_VI_TINH = don_vi_tinh;
                tdEntity.SO_LUONG = localAPI.ConvertStringToDecimal(so_luong);
                tdEntity.NANG_LUONG_KCAL = localAPI.ConvertStringToDecimal(nang_luong);
                tdEntity.PROTID = localAPI.ConvertStringToDecimal(protid);
                tdEntity.LIPID = localAPI.ConvertStringToDecimal(lipid);
                tdEntity.GLUCID = localAPI.ConvertStringToDecimal(glucid);
                tdEntity.NANG_LUONG_KCAL_OLD = localAPI.ConvertStringToDecimal(nang_luong_old);
                tdEntity.PROTID_OLD = localAPI.ConvertStringToDecimal(protid_old);
                tdEntity.LIPID_OLD = localAPI.ConvertStringToDecimal(lipid_old);
                tdEntity.GLUCID_OLD = localAPI.ConvertStringToDecimal(glucid_old);
                lst.Add(tdEntity);
                #endregion
            }
            Session["TableThucPhamSuatAn" + Sys_User.ID] = lst;
        }
        protected void addThucDonInList(long suat_an, short id_nhom_tp, long id_tp, short? don_vi_tinh, decimal? so_luong, decimal? nang_luong, decimal? protid, decimal? lipid, decimal? glucid, decimal? nang_luong_old, decimal? protid_old, decimal? lipid_old, decimal? glucid_old, List<SuatAnChiTietEntity> lst)
        {
            SuatAnChiTietEntity tdEntity = new SuatAnChiTietEntity();
            tdEntity.ID_SUAT_AN = suat_an;
            tdEntity.ID_NHOM_THUC_PHAM = id_nhom_tp;
            tdEntity.ID_THUC_PHAM = id_tp;
            tdEntity.ID_TRUONG = Sys_This_Truong.ID;
            tdEntity.ID_KHOI = Convert.ToInt16(rcbKhoi.SelectedValue);
            tdEntity.DON_VI_TINH = don_vi_tinh;
            tdEntity.SO_LUONG = so_luong;
            tdEntity.NANG_LUONG_KCAL = nang_luong;
            tdEntity.PROTID = protid;
            tdEntity.LIPID = lipid;
            tdEntity.GLUCID = glucid;
            tdEntity.NANG_LUONG_KCAL_OLD = nang_luong_old;
            tdEntity.PROTID_OLD = protid_old;
            tdEntity.LIPID_OLD = lipid_old;
            tdEntity.GLUCID_OLD = glucid_old;
            lst.Add(tdEntity);
        }
        protected void RadGrid1_PreRender(object sender, EventArgs e)
        {
            int row_tong_nang_luong = 0;
            if (RadGrid1.Items.Count > 2)
            {
                row_tong_nang_luong = RadGrid1.Items.Count - 2;
                for (int i = row_tong_nang_luong; i < RadGrid1.Items.Count; i++)
                {
                    //RadGrid1.Items[i]["TEN_NHOM_THUC_PHAM"].ColumnSpan = 4;
                    RadGrid1.Items[i]["TEN_NHOM_THUC_PHAM"].Text = "Tổng";
                    RadGrid1.Items[i]["TEN_NHOM_THUC_PHAM"].Font.Bold = true;
                    RadGrid1.Items[i]["TEN_NHOM_THUC_PHAM"].ForeColor = Color.Red;
                    //RadGrid1.Items[i]["TEN_THUC_PHAM"].Visible = false;
                    //RadGrid1.Items[i]["DON_VI"].Visible = false;
                    //RadGrid1.Items[i]["SO_LUONG"].Visible = false;
                }
                RadGrid1.Items[RadGrid1.Items.Count - 1]["TEN_NHOM_THUC_PHAM"].ColumnSpan = 4;
                RadGrid1.Items[RadGrid1.Items.Count - 1]["TEN_THUC_PHAM"].Visible = false;
                RadGrid1.Items[RadGrid1.Items.Count - 1]["DON_VI"].Visible = false;
                RadGrid1.Items[RadGrid1.Items.Count - 1]["SO_LUONG"].Visible = false;
                RadGrid1.Items[RadGrid1.Items.Count - 1]["TEN_NHOM_THUC_PHAM"].Text = "Năng lượng chuẩn";
            }
        }
        protected void rdNgay_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            if (rcbKhoi.SelectedValue != "" && rcbBuaAn.SelectedValue != "")
                getSoDangKyTrongNgay(Convert.ToDateTime(rdNgay.SelectedDate), Convert.ToInt16(rcbKhoi.SelectedValue), Convert.ToInt64(rcbBuaAn.SelectedValue));
            #region get value hiddenfield
            long? so_hs_dk = localAPI.ConvertStringTolong(tbSoHocSinhDK.Text.Trim());
            long? id_bua_an = localAPI.ConvertStringTolong(rcbBuaAn.SelectedValue);
            if (id_bua_an != null) getValueHidden(so_hs_dk, id_bua_an.Value);
            #endregion
        }
        protected void getValueHidden(long? so_hs_dk, long id_bua_an)
        {
            DM_BUA_AN buaAn = new DM_BUA_AN();
            if (rcbBuaAn.SelectedValue != "") buaAn = buaAnBO.getById(id_bua_an);
            if (buaAn != null && so_hs_dk != null)
            {
                hdNangLuongTu.Value = buaAn.NANG_LUONG_TU_KCAL != null ? (buaAn.NANG_LUONG_TU_KCAL * so_hs_dk).ToString() : "0";
                hdNangLuongDen.Value = buaAn.NANG_LUONG_DEN_KCAL != null ? (buaAn.NANG_LUONG_DEN_KCAL * so_hs_dk).ToString() : "0";
                hdProtidTu.Value = buaAn.PROTID_TU_KCAL != null ? (buaAn.PROTID_TU_KCAL * so_hs_dk).ToString() : "0";
                hdProtidDen.Value = buaAn.PROTID_DEN_KCAL != null ? (buaAn.PROTID_DEN_KCAL * so_hs_dk).ToString() : "0";
                hdGlucidTu.Value = buaAn.GLUCID_TU_KCAL != null ? (buaAn.GLUCID_TU_KCAL * so_hs_dk).ToString() : "0";
                hdGlucidDen.Value = buaAn.GLUCID_DEN_KCAL != null ? (buaAn.GLUCID_DEN_KCAL * so_hs_dk).ToString() : "0";
                hdLipidTu.Value = buaAn.LIPID_TU_KCAL != null ? (buaAn.LIPID_TU_KCAL * so_hs_dk).ToString() : "0";
                hdLipidDen.Value = buaAn.LIPID_DEN_KCAL != null ? (buaAn.LIPID_DEN_KCAL * so_hs_dk).ToString() : "0";
            }
        }
    }
}