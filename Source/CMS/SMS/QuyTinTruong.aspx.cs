using CMS.XuLy;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using OneEduDataAccess.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CMS.SMS
{
    public partial class QuyTinTruong : AuthenticatePage
    {
        LocalAPI local_api = new LocalAPI();
        TruongBO truongBO = new TruongBO();
        QuyTinBO quyTinBO = new QuyTinBO();
        GoiTinBO goiTinBO = new GoiTinBO();
        LogUserBO logUserBO = new LogUserBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rcbTruong.DataSource = Sys_List_Truong;
                rcbTruong.DataBind();
                #region set tháng năm học
                int nam_hoc = Sys_Ma_Nam_hoc;
                for (int i = 8; i <= 12; i++)
                    rcbThang.Items.Add(new RadComboBoxItem(i + " - " + nam_hoc, i.ToString()));
                for (int i = 1; i <= 7; i++)
                    rcbThang.Items.Add(new RadComboBoxItem(i + " - " + (nam_hoc + 1), i.ToString()));
                rcbThang.DataBind();
                rcbThang.SelectedValue = DateTime.Now.Month.ToString();
                #endregion
            }
        }

        protected void viewQuyTin()
        {
            short nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc);
            short thang = Convert.ToInt16(rcbThang.SelectedValue);
            long? id_truong = local_api.ConvertStringTolong(rcbTruong.SelectedValue);
            if (thang < 8) nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc + 1);
            if (id_truong != null)
            {
                QUY_TIN qUY_TINLL = new QUY_TIN();
                bool is_insert_new_quyll = false;
                qUY_TINLL = quyTinBO.getQuyTin(nam_hoc, thang, id_truong.Value, SYS_Loai_Tin.Tin_Lien_Lac, Sys_User.ID, out is_insert_new_quyll);

                if (qUY_TINLL != null)
                {
                    tbSoTinHSll.Text = qUY_TINLL.SO_TIN_HS.ToString();
                    tbTongThemll.Text = qUY_TINLL.TONG_THEM.ToString();
                    tbTongConll.Text = qUY_TINLL.TONG_CON.ToString();
                    tbTongCapll.Text = qUY_TINLL.TONG_CAP.ToString();
                    tbTongDaSDll.Text = qUY_TINLL.TONG_DA_SU_DUNG.ToString();
                }
                else
                {
                    tbSoTinHSll.Text = "";
                    tbTongThemll.Text = "";
                    tbTongConll.Text = "";
                    tbTongCapll.Text = "";
                    tbTongDaSDll.Text = "";
                }
                QUY_TIN qUY_TINTB = new QUY_TIN();
                bool is_insert_new_quytb = false;
                qUY_TINTB = quyTinBO.getQuyTin(nam_hoc, thang, id_truong.Value, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID, out is_insert_new_quytb);

                if (qUY_TINTB != null)
                {
                    tbSoTinHStb.Text = qUY_TINTB.SO_TIN_HS.ToString();
                    tbTongThemtb.Text = qUY_TINTB.TONG_THEM.ToString();
                    tbTongContb.Text = qUY_TINTB.TONG_CON.ToString();
                    tbTongCaptb.Text = qUY_TINTB.TONG_CAP.ToString();
                    tbTongDaSDtb.Text = qUY_TINTB.TONG_DA_SU_DUNG.ToString();
                }
                else
                {
                    tbSoTinHStb.Text = "";
                    tbTongThemtb.Text = "";
                    tbTongContb.Text = "";
                    tbTongCaptb.Text = "";
                    tbTongDaSDtb.Text = "";

                }
            }
        }

        protected void btSave_Click(object sender, EventArgs e)
        {
            if (chbIsActive.Checked)
            {
                if (tbThuongHieu.Text.Trim() == "" || tbVina.Text.Trim() == "" || tbGtel.Text.Trim() == "" || tbMobi.Text.Trim() == "" || tbVNM.Text.Trim() == "" || rcbCPGtel.SelectedValue == "" || rcbCPMobi.SelectedValue == "" || rcbCPVietTel.SelectedValue == "" || rcbCPViNa.SelectedValue == "" || rcbCPVNM.SelectedValue == "")
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Vui lòng nhập đầy đủ tên thương hiệu và các đối tác!');", true);
                    return;
                }
            }
            long? id_truong = local_api.ConvertStringTolong(rcbTruong.SelectedValue);
            if (id_truong == null) return;
            TRUONG detailTruong = new TRUONG();
            detailTruong = truongBO.getTruongById(id_truong.Value);
            short nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc);
            short thang = Convert.ToInt16(rcbThang.SelectedValue);
            if (detailTruong != null)
            {
                detailTruong.BRAND_NAME_VIETTEL = tbThuongHieu.Text.Trim();
                detailTruong.BRAND_NAME_VINA = tbVina.Text.Trim();
                detailTruong.BRAND_NAME_GTEL = tbGtel.Text.Trim();
                detailTruong.BRAND_NAME_MOBI = tbMobi.Text.Trim();
                detailTruong.BRAND_NAME_VNM = tbVNM.Text.Trim();
                detailTruong.CP_GTEL = rcbCPGtel.SelectedValue == "" ? "" : rcbCPGtel.SelectedValue;
                detailTruong.CP_MOBI = rcbCPMobi.SelectedValue == "" ? "" : rcbCPMobi.SelectedValue;
                detailTruong.CP_VIETTEL = rcbCPVietTel.SelectedValue == "" ? "" : rcbCPVietTel.SelectedValue;
                detailTruong.CP_VINA = rcbCPViNa.SelectedValue == "" ? "" : rcbCPViNa.SelectedValue;
                detailTruong.CP_VNM = rcbCPVNM.SelectedValue == "" ? "" : rcbCPVNM.SelectedValue;
                detailTruong.MA_GOI_TIN = local_api.ConvertStringToShort(rcbGoi.SelectedValue);
                detailTruong.IS_ACTIVE_SMS = chbIsActive.Checked;
                detailTruong.IS_SAN_QUY_TIN_NAM = chbIsSanTinNam.Checked;
                detailTruong.SO_HS_DANG_KY = local_api.ConvertStringToint(tbSoHSDK.Text);
                detailTruong.SO_HS_DUOC_MIEN = local_api.ConvertStringToint(tbHSMien.Text);
                detailTruong.PHAN_TRAM_VUOT_HAN_MUC = local_api.ConvertStringToShort(tbPhanTramVuotMuc.Text);
                short? themll = local_api.ConvertStringToShort(tbTongThemll.Text);
                short? themtb = local_api.ConvertStringToShort(tbTongThemtb.Text);
                ResultEntity res0 = truongBO.update(detailTruong, Sys_User.ID);
                ResultEntity res1 = quyTinBO.cap_nhat_quy_tin(nam_hoc, thang, id_truong.Value, SYS_Loai_Tin.Tin_Lien_Lac, Sys_User.ID, themll, 0);
                ResultEntity res2 = quyTinBO.cap_nhat_quy_tin(nam_hoc, thang, id_truong.Value, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID, themtb, 0);
                string strMsg = "";
                if (res0.Res && res1.Res && res2.Res)
                {
                    strMsg = "notification('success', 'Thao tác thành công');";
                }
                else
                {
                    strMsg = "notification('error', 'Có lỗi xãy ra');";
                }
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            }
            viewQuyTin();
        }

        protected void rcbTruong_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbGoi.ClearSelection();
            rcbGoi.Text = string.Empty;
            rcbCPGtel.ClearSelection();
            rcbCPGtel.Text = string.Empty;
            rcbCPMobi.ClearSelection();
            rcbCPMobi.Text = string.Empty;
            rcbCPVietTel.ClearSelection();
            rcbCPVietTel.Text = string.Empty;
            rcbCPViNa.ClearSelection();
            rcbCPViNa.Text = string.Empty;
            rcbCPVNM.ClearSelection();
            rcbCPVNM.Text = string.Empty;
            long id_truong = !string.IsNullOrEmpty(rcbTruong.SelectedValue) ? local_api.ConvertStringTolong(rcbTruong.SelectedValue).Value : 0;
            TRUONG detailTruong = new TRUONG();
            detailTruong = truongBO.getTruongById(id_truong);
            if (detailTruong != null)
            {
                chbIsActive.Checked = detailTruong.IS_ACTIVE_SMS == null ? false : detailTruong.IS_ACTIVE_SMS.Value;
                chbIsSanTinNam.Checked = detailTruong.IS_SAN_QUY_TIN_NAM == null ? false : detailTruong.IS_SAN_QUY_TIN_NAM.Value;
                rcbGoi.SelectedValue = detailTruong.MA_GOI_TIN == null ? "" : detailTruong.MA_GOI_TIN.ToString();
                tbSoHSDK.Text = detailTruong.SO_HS_DANG_KY.ToString();
                tbHSMien.Text = detailTruong.SO_HS_DUOC_MIEN.ToString();
                tbPhanTramVuotMuc.Text = detailTruong.PHAN_TRAM_VUOT_HAN_MUC == null ? "0" : detailTruong.PHAN_TRAM_VUOT_HAN_MUC.ToString();
                tbThuongHieu.Text = detailTruong.BRAND_NAME_VIETTEL;
                tbVina.Text = detailTruong.BRAND_NAME_VINA;
                tbGtel.Text = detailTruong.BRAND_NAME_GTEL;
                tbMobi.Text = detailTruong.BRAND_NAME_MOBI;
                tbVNM.Text = detailTruong.BRAND_NAME_VNM;
                rcbCPGtel.SelectedValue = detailTruong.CP_GTEL == null ? "" : detailTruong.CP_GTEL;
                rcbCPMobi.SelectedValue = detailTruong.CP_MOBI == null ? "" : detailTruong.CP_MOBI;
                rcbCPVietTel.SelectedValue = detailTruong.CP_VIETTEL == null ? "" : detailTruong.CP_VIETTEL;
                rcbCPViNa.SelectedValue = detailTruong.CP_VINA == null ? "" : detailTruong.CP_VINA;
                rcbCPVNM.SelectedValue = detailTruong.CP_VNM == null ? "" : detailTruong.CP_VNM;
                viewQuyTin();
            }
        }

        protected void rcbGoi_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            short ma_goi = local_api.ConvertStringToShort(rcbGoi.SelectedValue).Value;
            GOI_TIN gOI_TIN = new GOI_TIN();
            gOI_TIN = goiTinBO.getGoiTinByMa(ma_goi);
            if (gOI_TIN != null)
            {
                long so_hs = local_api.ConvertStringTolong(tbSoHSDK.Text) == null ? 0 : local_api.ConvertStringTolong(tbSoHSDK.Text).Value;

                #region Tin liên lạc
                QUY_TIN qUY_TINLL = new QUY_TIN();
                qUY_TINLL.SO_TIN_HS = (gOI_TIN.SO_TIN_LIEN_LAC_HS == null ? 0 : gOI_TIN.SO_TIN_LIEN_LAC_HS.Value)*4;
                long? themll = local_api.ConvertStringTolong(tbTongThemll.Text);
                qUY_TINLL.TONG_THEM = themll == null ? 0 : themll.Value;
                long? capll = local_api.ConvertStringTolong(tbTongCapll.Text);
                qUY_TINLL.TONG_CAP = capll == null ? 0 : capll.Value;
                long? dasdll = local_api.ConvertStringTolong(tbTongDaSDll.Text);
                qUY_TINLL.TONG_DA_SU_DUNG = dasdll == null ? 0 : dasdll.Value;

                qUY_TINLL.TONG_CAP = qUY_TINLL.SO_TIN_HS * so_hs;
                qUY_TINLL.TONG_CON = qUY_TINLL.TONG_CAP + qUY_TINLL.TONG_THEM - qUY_TINLL.TONG_DA_SU_DUNG;

                tbSoTinHSll.Text = qUY_TINLL.SO_TIN_HS.ToString();
                tbTongThemll.Text = qUY_TINLL.TONG_THEM.ToString();
                tbTongConll.Text = qUY_TINLL.TONG_CON.ToString();
                tbTongCapll.Text = qUY_TINLL.TONG_CAP.ToString();
                tbTongDaSDll.Text = qUY_TINLL.TONG_DA_SU_DUNG.ToString();
                #endregion

                #region Tin thông báo
                QUY_TIN qUY_TINTB = new QUY_TIN();
                qUY_TINTB.SO_TIN_HS = gOI_TIN.SO_TIN_THONG_BAO_HS == null ? 0 : gOI_TIN.SO_TIN_THONG_BAO_HS.Value;
                long? themtb = local_api.ConvertStringTolong(tbTongThemtb.Text);
                qUY_TINTB.TONG_THEM = themtb == null ? 0 : themtb.Value;
                long? captb = local_api.ConvertStringTolong(tbTongCaptb.Text);
                qUY_TINTB.TONG_CAP = captb == null ? 0 : captb.Value;
                long? dasdtb = local_api.ConvertStringTolong(tbTongDaSDtb.Text);
                qUY_TINTB.TONG_DA_SU_DUNG = dasdtb == null ? 0 : dasdtb.Value;

                qUY_TINTB.TONG_CAP = qUY_TINTB.SO_TIN_HS * so_hs;
                qUY_TINTB.TONG_CON = qUY_TINTB.TONG_CAP + qUY_TINTB.TONG_THEM - qUY_TINTB.TONG_DA_SU_DUNG;

                tbSoTinHStb.Text = qUY_TINTB.SO_TIN_HS.ToString();
                tbTongThemtb.Text = qUY_TINTB.TONG_THEM.ToString();
                tbTongContb.Text = qUY_TINTB.TONG_CON.ToString();
                tbTongCaptb.Text = qUY_TINTB.TONG_CAP.ToString();
                tbTongDaSDtb.Text = qUY_TINTB.TONG_DA_SU_DUNG.ToString();
                #endregion

            }
        }

        protected void rcbThang_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            viewQuyTin();
        }

        protected void tbSaveLL_Click(object sender, EventArgs e)
        {
            long? id_truong = local_api.ConvertStringTolong(rcbTruong.SelectedValue);
            if (id_truong == null) return;
            TRUONG detailTruong = new TRUONG();
            detailTruong = truongBO.getTruongById(id_truong.Value);
            //short nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc);
            short thang = Convert.ToInt16(rcbThang.SelectedValue);
            #region lấy năm gửi tin nhắn
            short nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc);
            if (thang >= 1 && thang <= 7) nam_hoc += 1;
            #endregion
            if (detailTruong != null)
            {
                long? themll = local_api.ConvertStringTolong(tbTongThemll.Text);
                ResultEntity res1 = quyTinBO.cap_nhat_quy_tin(nam_hoc, thang, id_truong.Value, SYS_Loai_Tin.Tin_Lien_Lac, Sys_User.ID, themll, 0);
                string strMsg = "";
                if (res1.Res)
                {
                    strMsg = "notification('success', 'Thao tác thành công');";
                    logUserBO.insert(id_truong.Value, "UPDATE", "Lượng tin liên lạc cấp thêm: " + themll, Sys_User.ID, DateTime.Now);
                }
                else
                {
                    strMsg = "notification('error', 'Có lỗi xãy ra');";
                }
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            }
            viewQuyTin();
        }

        protected void tbSaveTB_Click(object sender, EventArgs e)
        {
            long? id_truong = local_api.ConvertStringTolong(rcbTruong.SelectedValue);
            if (id_truong == null)
                return;
            TRUONG detailTruong = new TRUONG();
            detailTruong = truongBO.getTruongById(id_truong.Value);
            //short nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc);
            short thang = Convert.ToInt16(rcbThang.SelectedValue);
            #region lấy năm gửi tin nhắn
            short nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc);
            if (thang >= 1 && thang <= 7) nam_hoc += 1;
            #endregion
            if (detailTruong != null)
            {
                long? themtb = local_api.ConvertStringTolong(tbTongThemtb.Text);
                ResultEntity res2 = quyTinBO.cap_nhat_quy_tin(nam_hoc, thang, id_truong.Value, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID, themtb, 0);
                string strMsg = "";
                if (res2.Res)
                {
                    strMsg = "notification('success', 'Thao tác thành công');";
                    logUserBO.insert(id_truong.Value, "UPDATE", "Lượng tin thông báo cấp thêm: " + themtb, Sys_User.ID, DateTime.Now);
                }
                else
                {
                    strMsg = "notification('error', 'Có lỗi xãy ra');";
                }
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            }
            viewQuyTin();
        }
    }
}