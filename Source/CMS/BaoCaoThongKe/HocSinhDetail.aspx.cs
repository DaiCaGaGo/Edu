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

namespace CMS.BaoCaoThongKe
{
    public partial class HocSinhDetail : AuthenticatePage
    {
        public LocalAPI localAPI = new LocalAPI();
        HocSinhBO hsBO = new HocSinhBO();
        KhoiBO khoiBO = new KhoiBO();
        LogUserBO logUserBO = new LogUserBO();
        long? id_hs;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("id_hs") != null)
            {
                try
                {
                    id_hs = Convert.ToInt64(Request.QueryString.Get("id_hs"));
                }
                catch (Exception ex) { }
            }
            if (!IsPostBack)
            {
                if (id_hs != null)
                {
                    HOC_SINH detail = new HOC_SINH();
                    detail = hsBO.getHocSinhByID(id_hs.Value);
                    if (detail != null)
                    {
                        divLoaiGDTX.Visible = (detail.MA_CAP_HOC == SYS_Cap_Hoc.GDTX) ? true : false;
                        objKhoi.SelectParameters.Add("cap_hoc", detail.MA_CAP_HOC);
                        rcbKhoi.DataBind();
                        objLop.SelectParameters.Add("ma_cap_hoc", detail.MA_CAP_HOC);
                        objLop.SelectParameters.Add("idTruong", detail.ID_TRUONG.ToString());
                        objLop.SelectParameters.Add("maNamHoc", Sys_Ma_Nam_hoc.ToString());
                        rcbLop.DataBind();

                        tbMa.Text = detail.MA;
                        if (detail.TEN != null)
                            tbTen.Text = detail.HO_TEN.ToString();
                        rcbLop.SelectedValue = detail.ID_LOP.ToString();
                        rcbKhoi.SelectedValue = detail.ID_KHOI.ToString();
                        if (detail.NGAY_SINH != null)
                            rdNgaySinh.SelectedDate = detail.NGAY_SINH;
                        rcbGioiTinh.SelectedValue = detail.MA_GIOI_TINH.ToString();
                        if (detail.TRANG_THAI_HOC != null) rcbTrangThai.SelectedValue = detail.TRANG_THAI_HOC.ToString();
                        if (detail.SDT_NHAN_TIN != null) tbSDT_NhanTin.Text = detail.SDT_NHAN_TIN.Trim();

                        if (detail.IS_CON_GV == true) cbConGV.Checked = true;
                        if (detail.IS_DK_KY1 == true) cbSMS1.Checked = true;
                        if (detail.IS_DK_KY2 == true) cbSMS2.Checked = true;
                        if (detail.IS_MIEN_GIAM_KY1 == true) cbMienPhi1.Checked = true;
                        if (detail.IS_MIEN_GIAM_KY2 == true) cbMienPhi2.Checked = true;
                        if (detail.THU_TU != null) tbThuTu.Text = detail.THU_TU.ToString();
                        if (cbMienPhi1.Checked || cbMienPhi2.Checked) divNhanTinBoMe.Visible = false;
                        else divNhanTinBoMe.Visible = true;
                        if (detail.IS_GUI_BO_ME == true) cboGuiBoMe.Checked = true;
                        divSDTKhac.Visible = cboGuiBoMe.Checked ? true : false;
                        if (detail.SDT_NHAN_TIN2 != null) tbSDT_NhanTinKhac.Text = detail.SDT_NHAN_TIN2;
                        tbChieuCao.Text = detail.CHIEU_CAO != null ? detail.CHIEU_CAO.ToString() : "";
                        tbCanNang.Text = detail.CAN_NANG != null ? detail.CAN_NANG.ToString() : "";

                        if (detail.IS_HOI_TRUONG_CHPH == true) cbHoiTruong.Checked = true;
                        if (detail.IS_HOI_PHO_CHPH == true) cbHoiPho.Checked = true;
                    }
                }
                if (Sys_Hoc_Ky == 2)
                {
                    cbSMS1.Enabled = false;
                    cbMienPhi1.Enabled = false;
                }
            }
        }
        protected void btEdit_Click(object sender, EventArgs e)
        {
            if (cboGuiBoMe.Checked && tbSDT_NhanTinKhac.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Số điện thoại nhận tin khác không được trống!');", true);
                tbSDT_NhanTinKhac.Focus();
                return;
            }
            string strMsg = "";
            HOC_SINH detail = new HOC_SINH();
            detail = hsBO.getHocSinhByID(id_hs.Value);
            if (detail == null) detail = new HOC_SINH();
            detail.HO_TEN = tbTen.Text.Trim();
            string ho_dem = "", ten = "";
            localAPI.splitHoTen(tbTen.Text.Trim(), out ho_dem, out ten);
            detail.TEN = ten.Trim();
            detail.HO_DEM = ho_dem.Trim();
            if (tbThuTu.Text.Trim() != "") detail.THU_TU = localAPI.ConvertStringToShort(tbThuTu.Text);
            try
            {
                detail.NGAY_SINH = rdNgaySinh.SelectedDate.Value;
            }
            catch { }
            if (rcbGioiTinh.SelectedValue != "") detail.MA_GIOI_TINH = Convert.ToInt16(rcbGioiTinh.SelectedValue);
            detail.ID_LOP = Convert.ToInt64(rcbLop.SelectedValue);
            detail.TRANG_THAI_HOC = localAPI.ConvertStringToShort(rcbTrangThai.SelectedValue);
            detail.SDT_NHAN_TIN = tbSDT_NhanTin.Text.Trim() == "" ? " " : localAPI.Add84(tbSDT_NhanTin.Text.Trim());
            detail.SDT_NHAN_TIN2 = tbSDT_NhanTinKhac.Text.Trim() == "" ? null : localAPI.Add84(tbSDT_NhanTinKhac.Text.Trim());
            detail.IS_CON_GV = cbConGV.Checked;
            detail.IS_DK_KY1 = cbSMS1.Checked;
            detail.IS_DK_KY2 = cbSMS2.Checked;
            detail.IS_MIEN_GIAM_KY1 = cbMienPhi1.Checked;
            detail.IS_MIEN_GIAM_KY2 = cbMienPhi2.Checked;
            if (cbMienPhi1.Checked || cbMienPhi2.Checked) detail.IS_GUI_BO_ME = false;
            else detail.IS_GUI_BO_ME = cboGuiBoMe.Checked;
            detail.ID_KHOI = Convert.ToInt16(rcbKhoi.SelectedValue);
            detail.ID_NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
            detail.CAN_NANG = !string.IsNullOrEmpty(tbCanNang.Text.Trim()) ? tbCanNang.Text.Trim() : null;
            
            detail.IS_HOI_PHO_CHPH = cbHoiPho.Checked ? true : false;
            detail.IS_HOI_TRUONG_CHPH = cbHoiTruong.Checked ? true : false;

            ResultEntity res = hsBO.update(detail, Sys_User.ID);
            if (res.Res)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
                logUserBO.insert(detail.ID_TRUONG, "UPDATE", "Cập nhật học sinh " + id_hs.Value, Sys_User.ID, DateTime.Now);
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
        protected void rcbKhoi_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbLop.ClearSelection();
            rcbLop.Text = string.Empty;
            rcbLop.DataBind();
        }
        
        protected void rcbLoaiLopGDTX_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbKhoi.ClearSelection();
            rcbKhoi.Text = string.Empty;
            rcbKhoi.DataBind();
            rcbLop.ClearSelection();
            rcbLop.Text = string.Empty;
            rcbLop.DataBind();
        }
        protected void cboGuiBoMe_CheckedChanged(object sender, EventArgs e)
        {
            divSDTKhac.Visible = cboGuiBoMe.Checked ? true : false;
        }
        protected void cbMienPhi1_CheckedChanged(object sender, EventArgs e)
        {
            if (cbMienPhi1.Checked)
            {
                divNhanTinBoMe.Visible = false;
            }
            else
            {
                if (cbMienPhi2.Checked) divNhanTinBoMe.Visible = false;
                else divNhanTinBoMe.Visible = true;
            }
        }
        protected void cbMienPhi2_CheckedChanged(object sender, EventArgs e)
        {
            if (cbMienPhi2.Checked)
            {
                divNhanTinBoMe.Visible = false;
            }
            else
            {
                if (cbMienPhi1.Checked) divNhanTinBoMe.Visible = false;
                else divNhanTinBoMe.Visible = true;
            }
        }
    }
}