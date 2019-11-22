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

namespace CMS.Truong
{
    public partial class TruongDetail : AuthenticatePage
    {
        long? id_truong;
        TruongBO truongBO = new TruongBO();
        LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("id_hoso") != null)
            {
                try
                {
                    id_truong = Convert.ToInt16(Request.QueryString.Get("id_hoso"));
                }
                catch { }
            }
            if (!IsPostBack)
            {
                if (id_truong != null)
                {
                    TRUONG detail = new TRUONG();
                    detail = truongBO.getTruongById(id_truong.Value);
                    if (detail != null)
                    {
                        if (detail.MA != null)
                            tbMa.Text = detail.MA.ToString();
                        tbTen.Text = detail.TEN;
                        tbThuongHieu.Text = detail.BRAND_NAME_VIETTEL;
                        tbVina.Text = detail.BRAND_NAME_VINA;
                        tbGtel.Text = detail.BRAND_NAME_GTEL;
                        tbMobi.Text = detail.BRAND_NAME_MOBI;
                        tbVNM.Text = detail.BRAND_NAME_VNM;
                        rcbCPGtel.SelectedValue = detail.CP_GTEL == null ? "" : detail.CP_GTEL;
                        rcbCPMobi.SelectedValue = detail.CP_MOBI == null ? "" : detail.CP_MOBI;
                        rcbCPVietTel.SelectedValue = detail.CP_VIETTEL == null ? "" : detail.CP_VIETTEL;
                        rcbCPViNa.SelectedValue = detail.CP_VINA == null ? "" : detail.CP_VINA;
                        rcbCPVNM.SelectedValue = detail.CP_VNM == null ? "" : detail.CP_VNM;
                        rcbDaiLy.SelectedValue = detail.ID_DOI_TAC == null ? "" : detail.ID_DOI_TAC.ToString();
                        tbTongTinCap.Text = detail.TONG_TIN_CAP == null ? "" : detail.TONG_TIN_CAP.ToString();
                        tbTongTinSuDung.Text = detail.TONG_TIN_DA_DUNG == null ? "" : detail.TONG_TIN_DA_DUNG.ToString();
                        tbDiaChi.Text = detail.DIA_CHI;
                        tbSDT.Text = detail.DIEN_THOAI;
                        tbEmail.Text = detail.EMAIL;
                        tbHieuTruong.Text = detail.HIEU_TRUONG;
                        tbSDT_HieuTruong.Text = detail.DIEN_THOAI_HT;
                        tbEmailHieuTruong.Text = detail.EMAIL_HT;
                        tbSDT_NLH.Text = detail.DIEN_THOAI_NLH;
                        if (detail.TRANG_THAI) rcbTrangThai.SelectedValue = "1";
                        else rcbTrangThai.SelectedValue = "0";
                        foreach (RadComboBoxItem item in rcbCapHoc.Items)
                        {
                            if (item.Value == "MN") item.Checked = detail.IS_MN == null ? false : detail.IS_MN.Value;
                            if (item.Value == "TH") item.Checked = detail.IS_TH == null ? false : detail.IS_TH.Value;
                            if (item.Value == "THCS") item.Checked = detail.IS_THCS == null ? false : detail.IS_THCS.Value;
                            if (item.Value == "THPT") item.Checked = detail.IS_THPT == null ? false : detail.IS_THPT.Value;
                            if (item.Value == "GDTX") item.Checked = detail.IS_GDTX == null ? false : detail.IS_GDTX.Value;
                        }
                        chbIsActive.Checked = detail.IS_ACTIVE_SMS == null ? false : detail.IS_ACTIVE_SMS.Value;
                        chbIsSanTinNam.Checked = detail.IS_SAN_QUY_TIN_NAM == null ? false : detail.IS_SAN_QUY_TIN_NAM.Value;
                        divSanQuyTinNam.Visible = chbIsActive.Checked;
                        if (detail.MA_GOI_TIN != null) rcbMaGoiTin.SelectedValue = detail.MA_GOI_TIN.ToString();
                        tbSoHSDangKy.Text = detail.SO_HS_DANG_KY.ToString();
                        tbSoHSDuocMien.Text = detail.SO_HS_DUOC_MIEN.ToString();
                        if (detail.PHAN_TRAM_VUOT_HAN_MUC != null) tbPhanTramVuot.Text = detail.PHAN_TRAM_VUOT_HAN_MUC.ToString();

                        if (detail.MA_TINH_THANH != null) rcbTinhThanh.SelectedValue = detail.MA_TINH_THANH.ToString();
                        if (detail.MA_QUAN_HUYEN != null) rcbQuanHuyen.SelectedValue = detail.MA_QUAN_HUYEN.ToString();
                        if (detail.MA_XA_PHUONG != null) rcbXaPhuong.SelectedValue = detail.MA_XA_PHUONG.ToString();
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
            if (!is_access(SYS_Type_Access.THEM))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            if (chbIsActive.Checked)
            {
                if (tbThuongHieu.Text.Trim() == "" || tbVina.Text.Trim() == "" || tbGtel.Text.Trim() == "" || tbMobi.Text.Trim() == "" || tbVNM.Text.Trim() == "" || rcbCPGtel.SelectedValue == "" || rcbCPMobi.SelectedValue == "" || rcbCPVietTel.SelectedValue == "" || rcbCPViNa.SelectedValue == "" || rcbCPVNM.SelectedValue == "")
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Vui lòng nhập đầy đủ tên thương hiệu và các đối tác!');", true);
                    return;
                }
            }
            TRUONG detail = new TRUONG();
            detail.MA = tbMa.Text.Trim();
            detail.TEN = tbTen.Text.Trim();
            detail.BRAND_NAME_VIETTEL = tbThuongHieu.Text.Trim();
            detail.BRAND_NAME_VINA = tbVina.Text.Trim();
            detail.BRAND_NAME_GTEL = tbGtel.Text.Trim();
            detail.BRAND_NAME_MOBI = tbMobi.Text.Trim();
            detail.BRAND_NAME_VNM = tbVNM.Text.Trim();
            detail.CP_GTEL = rcbCPGtel.SelectedValue == "" ? "" : rcbCPGtel.SelectedValue;
            detail.CP_MOBI = rcbCPMobi.SelectedValue == "" ? "" : rcbCPMobi.SelectedValue;
            detail.CP_VIETTEL = rcbCPVietTel.SelectedValue == "" ? "" : rcbCPVietTel.SelectedValue;
            detail.CP_VINA = rcbCPViNa.SelectedValue == "" ? "" : rcbCPViNa.SelectedValue;
            detail.CP_VNM = rcbCPVNM.SelectedValue == "" ? "" : rcbCPVNM.SelectedValue;
            detail.ID_DOI_TAC = localAPI.ConvertStringToShort(rcbDaiLy.SelectedValue);
            detail.DIA_CHI = tbDiaChi.Text.Trim();
            detail.DIEN_THOAI = tbSDT.Text.Trim();
            detail.EMAIL = tbEmail.Text.Trim();
            detail.HIEU_TRUONG = tbHieuTruong.Text.Trim();
            detail.DIEN_THOAI_HT = tbSDT_HieuTruong.Text.Trim();
            detail.EMAIL_HT = tbEmailHieuTruong.Text.Trim();
            detail.DIEN_THOAI_NLH = tbSDT_HieuTruong.Text.Trim();
            foreach (RadComboBoxItem item in rcbCapHoc.Items)
            {
                if (item.Value == "MN") detail.IS_MN = item.Checked ? true : false;
                if (item.Value == "TH") detail.IS_TH = item.Checked ? true : false;
                if (item.Value == "THCS") detail.IS_THCS = item.Checked ? true : false;
                if (item.Value == "THPT") detail.IS_THPT = item.Checked ? true : false;
                if (item.Value == "GDTX") detail.IS_GDTX = item.Checked ? true : false;
            }
            if (rcbTrangThai.SelectedValue == "1") detail.TRANG_THAI = true;
            else detail.TRANG_THAI = false;
            detail.MA_GOI_TIN = localAPI.ConvertStringToShort(rcbMaGoiTin.SelectedValue);
            detail.IS_ACTIVE_SMS = chbIsActive.Checked;
            if (chbIsActive.Checked) detail.IS_SAN_QUY_TIN_NAM = chbIsSanTinNam.Checked;
            else detail.IS_SAN_QUY_TIN_NAM = false;
            detail.SO_HS_DANG_KY = localAPI.ConvertStringToint(tbSoHSDangKy.Text);
            detail.SO_HS_DUOC_MIEN = localAPI.ConvertStringToint(tbSoHSDuocMien.Text);
            detail.PHAN_TRAM_VUOT_HAN_MUC = localAPI.ConvertStringToShort(tbPhanTramVuot.Text);

            detail.MA_TINH_THANH = localAPI.ConvertStringToShort(rcbTinhThanh.SelectedValue);
            detail.MA_QUAN_HUYEN = localAPI.ConvertStringToShort(rcbQuanHuyen.SelectedValue);
            detail.MA_XA_PHUONG = localAPI.ConvertStringToShort(rcbXaPhuong.SelectedValue);
            ResultEntity res = truongBO.insert(detail, Sys_User.ID);
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
        public void reset()
        {
            tbMa.Text = "";
            tbTen.Text = "";
            foreach (RadComboBoxItem item in rcbCapHoc.Items)
            {
                item.Checked = false;
            }
            rcbQuanHuyen.ClearSelection();
            rcbXaPhuong.ClearSelection();
            tbDiaChi.Text = "";
            tbSDT.Text = "";
            tbEmail.Text = "";
            tbHieuTruong.Text = "";
            tbSDT_HieuTruong.Text = "";
            tbEmailHieuTruong.Text = "";
            tbSDT_NLH.Text = "";
            chbIsActive.Checked = false;
            divSanQuyTinNam.Visible = false;
            rcbTrangThai.SelectedValue = "1";
            tbThuongHieu.Text = "";
            tbVina.Text = "";
            tbMobi.Text = "";
            tbGtel.Text = "";
            tbVNM.Text = "";
            tbSoHSDangKy.Text = "";
            tbSoHSDuocMien.Text = "";
            tbPhanTramVuot.Text = "";
            tbMa.Focus();
        }
        protected void btEdit_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            if (chbIsActive.Checked)
            {
                if (tbThuongHieu.Text.Trim() == "" || tbVina.Text.Trim() == "" || tbGtel.Text.Trim() == "" || tbMobi.Text.Trim() == "" || tbVNM.Text.Trim() == "" || rcbCPGtel.SelectedValue == "" || rcbCPMobi.SelectedValue == "" || rcbCPVietTel.SelectedValue == "" || rcbCPViNa.SelectedValue == "" || rcbCPVNM.SelectedValue == "")
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Vui lòng nhập đầy đủ tên thương hiệu và các đối tác!');", true);
                    return;
                }
            }
            TRUONG detail = new TRUONG();
            detail.ID = id_truong.Value;
            detail = truongBO.getTruongById(detail.ID);
            if (detail == null) detail = new TRUONG();
            detail.ID = id_truong.Value;
            detail.MA = tbMa.Text.Trim();
            detail.TEN = tbTen.Text;
            detail.BRAND_NAME_VIETTEL = tbThuongHieu.Text.Trim();
            detail.BRAND_NAME_VINA = tbVina.Text.Trim();
            detail.BRAND_NAME_GTEL = tbGtel.Text.Trim();
            detail.BRAND_NAME_MOBI = tbMobi.Text.Trim();
            detail.BRAND_NAME_VNM = tbVNM.Text.Trim();
            detail.CP_GTEL = rcbCPGtel.SelectedValue == "" ? "" : rcbCPGtel.SelectedValue;
            detail.CP_MOBI = rcbCPMobi.SelectedValue == "" ? "" : rcbCPMobi.SelectedValue;
            detail.CP_VIETTEL = rcbCPVietTel.SelectedValue == "" ? "" : rcbCPVietTel.SelectedValue;
            detail.CP_VINA = rcbCPViNa.SelectedValue == "" ? "" : rcbCPViNa.SelectedValue;
            detail.CP_VNM = rcbCPVNM.SelectedValue == "" ? "" : rcbCPVNM.SelectedValue;
            detail.ID_DOI_TAC = localAPI.ConvertStringToShort(rcbDaiLy.SelectedValue);
            detail.DIA_CHI = tbDiaChi.Text.Trim();
            detail.DIEN_THOAI = tbSDT.Text.Trim();
            detail.EMAIL = tbEmail.Text.Trim();
            detail.HIEU_TRUONG = tbHieuTruong.Text.Trim();
            detail.DIEN_THOAI_HT = tbSDT_HieuTruong.Text.Trim();
            detail.EMAIL_HT = tbEmailHieuTruong.Text.Trim();
            detail.DIEN_THOAI_NLH = tbSDT_HieuTruong.Text.Trim();
            foreach (RadComboBoxItem item in rcbCapHoc.Items)
            {
                if (item.Value == "MN") detail.IS_MN = item.Checked ? true : false;
                if (item.Value == "TH") detail.IS_TH = item.Checked ? true : false;
                if (item.Value == "THCS") detail.IS_THCS = item.Checked ? true : false;
                if (item.Value == "THPT") detail.IS_THPT = item.Checked ? true : false;
                if (item.Value == "GDTX") detail.IS_GDTX = item.Checked ? true : false;
            }
            if (rcbTrangThai.SelectedValue == "1") detail.TRANG_THAI = true;
            else detail.TRANG_THAI = false;
            detail.MA_GOI_TIN = localAPI.ConvertStringToShort(rcbMaGoiTin.SelectedValue);
            detail.IS_ACTIVE_SMS = chbIsActive.Checked;
            if (chbIsActive.Checked) detail.IS_SAN_QUY_TIN_NAM = chbIsSanTinNam.Checked;
            else detail.IS_SAN_QUY_TIN_NAM = false;
            detail.SO_HS_DANG_KY = localAPI.ConvertStringToint(tbSoHSDangKy.Text);
            detail.SO_HS_DUOC_MIEN = localAPI.ConvertStringToint(tbSoHSDuocMien.Text);
            detail.PHAN_TRAM_VUOT_HAN_MUC = localAPI.ConvertStringToShort(tbPhanTramVuot.Text);

            detail.MA_TINH_THANH = localAPI.ConvertStringToShort(rcbTinhThanh.SelectedValue);
            detail.MA_QUAN_HUYEN = localAPI.ConvertStringToShort(rcbQuanHuyen.SelectedValue);
            detail.MA_XA_PHUONG = localAPI.ConvertStringToShort(rcbXaPhuong.SelectedValue);
            ResultEntity res = truongBO.update(detail, Sys_User.ID, true);
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
        protected void rcbTinhThanh_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbQuanHuyen.ClearSelection();
            rcbQuanHuyen.Text = String.Empty;
            rcbQuanHuyen.DataBind();
            rcbXaPhuong.ClearSelection();
            rcbXaPhuong.Text = string.Empty;
            rcbXaPhuong.DataBind();
        }
        protected void rcbQuanHuyen_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbXaPhuong.ClearSelection();
            rcbXaPhuong.Text = string.Empty;
            rcbXaPhuong.DataBind();
        }
        protected void chbIsActive_CheckedChanged(object sender, EventArgs e)
        {
            divSanQuyTinNam.Visible = chbIsActive.Checked;
        }
    }
}