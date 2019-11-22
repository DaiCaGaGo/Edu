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

namespace CMS.HocSinh
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
                divLoaiGDTX.Visible = (Sys_This_Cap_Hoc == SYS_Cap_Hoc.GDTX) ? true : false;
                objKhoi.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                rcbKhoi.DataBind();
                objLop.SelectParameters.Add("ma_cap_hoc", Sys_This_Cap_Hoc);
                objLop.SelectParameters.Add("idTruong", Sys_This_Truong.ID.ToString());
                objLop.SelectParameters.Add("maNamHoc", Sys_Ma_Nam_hoc.ToString());
                rcbLop.DataBind();

                if (Sys_Hoc_Ky == 2)
                {
                    cbSMS1.Enabled = false;
                    cbMienPhi1.Enabled = false;
                }
                if (id_hs != null)
                {
                    HOC_SINH detail = new HOC_SINH();
                    detail = hsBO.getHocSinhByID(id_hs.Value);
                    if (detail != null)
                    {
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

                        if (detail.HO_TEN_BO != null) tbTenBo.Text = detail.HO_TEN_BO;
                        if (detail.NAM_SINH_BO != null) tbNamSinhBo.Text = detail.NAM_SINH_BO.ToString();
                        if (detail.SDT_BO != null) tbSDT_Bo.Text = detail.SDT_BO;
                        if (detail.HO_TEN_ME != null) tbTenMe.Text = detail.HO_TEN_ME;
                        if (detail.NAM_SINH_ME != null) tbNamSinhMe.Text = detail.NAM_SINH_ME.ToString();
                        if (detail.SDT_ME != null) tbSDT_Me.Text = detail.SDT_ME;
                        if (detail.HO_TEN_NGUOI_BAO_HO != null) tbTenNBH.Text = detail.HO_TEN_NGUOI_BAO_HO;
                        if (detail.NAM_SINH_NGUOI_BAO_HO != null) tbNamSinhNBH.Text = detail.NAM_SINH_NGUOI_BAO_HO.ToString();
                        if (detail.SDT_NBH != null) tbSDT_NBH.Text = detail.SDT_NBH;
                        if (detail.IS_CON_GV == true) cbConGV.Checked = true;
                        if (detail.IS_DK_KY1 == true) cbSMS1.Checked = true;
                        if (detail.IS_DK_KY2 == true) cbSMS2.Checked = true;
                        if (detail.IS_MIEN_GIAM_KY1 == true) cbMienPhi1.Checked = true;
                        if (detail.IS_MIEN_GIAM_KY2 == true) cbMienPhi2.Checked = true;
                        if (detail.DIA_CHI != null) tbDiaChi.Text = detail.DIA_CHI;
                        if (detail.THU_TU != null) tbThuTu.Text = detail.THU_TU.ToString();
                        if (detail.MA_TINH_THANH != null) rcbTinhThanh.SelectedValue = detail.MA_TINH_THANH.ToString();
                        if (detail.MA_QUAN_HUYEN != null) rcbQuanHuyen.SelectedValue = detail.MA_QUAN_HUYEN.ToString();
                        if (detail.MA_XA_PHUONG != null) rcbXaPhuong.SelectedValue = detail.MA_XA_PHUONG.ToString();
                        if (detail.MA_QUOC_TICH != null) rcbQuocTich.SelectedValue = detail.MA_QUOC_TICH.ToString();
                        if (detail.MA_KHU_VUC != null) rcbKhuVuc.SelectedValue = detail.MA_KHU_VUC.ToString();
                        if (detail.MA_DAN_TOC != null) rcbDanToc.SelectedValue = detail.MA_DAN_TOC.ToString();
                        if (detail.MA_DOI_TUONG_CS != null) rcbDoiTuongCS.SelectedValue = detail.MA_DOI_TUONG_CS.ToString();
                        if (detail.SO_CMND != null) tbSoCMND.Text = detail.SO_CMND;
                        if (detail.NGAY_CAP_CMND != null) rdNgayCapCMND.SelectedDate = detail.NGAY_CAP_CMND;
                        if (detail.NOI_CAP_CMND != null) tbNoiCapCMND.Text = detail.NOI_CAP_CMND;
                        if (detail.NOI_SINH != null) tbNoiSinh.Text = detail.NOI_SINH;
                        if (cbMienPhi1.Checked || cbMienPhi2.Checked) divNhanTinBoMe.Visible = false;
                        else divNhanTinBoMe.Visible = true;
                        if (detail.IS_GUI_BO_ME == true) cboGuiBoMe.Checked = true;
                        divSDTKhac.Visible = cboGuiBoMe.Checked ? true : false;
                        if (detail.SDT_NHAN_TIN2 != null) tbSDT_NhanTinKhac.Text = detail.SDT_NHAN_TIN2;
                        tbChieuCao.Text = detail.CHIEU_CAO != null ? detail.CHIEU_CAO.ToString() : "";
                        tbCanNang.Text = detail.CAN_NANG != null ? detail.CAN_NANG.ToString() : "";
                        imgAnh.ImageUrl = detail.ANH_DAI_DIEN != null ? detail.ANH_DAI_DIEN.ToString() : "";

                        if (detail.IS_HOI_TRUONG_CHPH == true) cbHoiTruong.Checked = true;
                        if (detail.IS_HOI_PHO_CHPH == true) cbHoiPho.Checked = true;

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
            long? max_thu_tu = hsBO.getMaxThuTuByTruongKhoiLopNamHoc(Sys_This_Truong.ID, localAPI.ConvertStringToShort(rcbKhoi.SelectedValue), localAPI.ConvertStringToShort(rcbLop.SelectedValue), Convert.ToInt16(Sys_Ma_Nam_hoc));
            if (max_thu_tu != null)
                tbThuTu.Text = Convert.ToString(Convert.ToInt64(max_thu_tu) + 1);
            else tbThuTu.Text = "1";
        }
        protected void btAdd_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.THEM))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            if (cboGuiBoMe.Checked && tbSDT_NhanTinKhac.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Số điện thoại nhận tin khác không được trống!');", true);
                tbSDT_NhanTinKhac.Focus();
                return;
            }
            string strMsg = "";
            HOC_SINH detail = new HOC_SINH();
            detail.HO_TEN = tbTen.Text.Trim();
            string ho_dem = "", ten = "";
            localAPI.splitHoTen(tbTen.Text.Trim(), out ho_dem, out ten);
            detail.TEN = ten.Trim();
            detail.HO_DEM = ho_dem.Trim();
            detail.THU_TU = localAPI.ConvertStringToShort(tbThuTu.Text);
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
            detail.HO_TEN_BO = tbTenBo.Text.Trim() == "" ? null : tbTenBo.Text.Trim();
            detail.HO_TEN_ME = tbTenMe.Text.Trim() == "" ? null : tbTenMe.Text.Trim();
            detail.HO_TEN_NGUOI_BAO_HO = tbTenNBH.Text.Trim() == "" ? null : tbTenNBH.Text.Trim();
            detail.NAM_SINH_BO = localAPI.ConvertStringToShort(tbNamSinhBo.Text);
            detail.NAM_SINH_ME = localAPI.ConvertStringToShort(tbNamSinhMe.Text);
            detail.NAM_SINH_NGUOI_BAO_HO = localAPI.ConvertStringToShort(tbNamSinhNBH.Text);
            detail.SDT_BO = tbSDT_Bo.Text.Trim() == "" ? null : localAPI.Add84(tbSDT_Bo.Text.Trim());
            detail.SDT_ME = tbSDT_Me.Text.Trim() == "" ? null : localAPI.Add84(tbSDT_Me.Text.Trim());
            detail.SDT_NBH = tbSDT_NBH.Text.Trim() == "" ? null : localAPI.Add84(tbSDT_NBH.Text.Trim());
            detail.IS_CON_GV = cbConGV.Checked ? true : false;
            detail.IS_DK_KY1 = cbSMS1.Checked ? true : false;
            detail.IS_DK_KY2 = cbSMS2.Checked ? true : false;
            detail.IS_MIEN_GIAM_KY1 = cbMienPhi1.Checked ? true : false;
            detail.IS_MIEN_GIAM_KY2 = cbMienPhi2.Checked ? true : false;
            if (cbMienPhi1.Checked || cbMienPhi2.Checked) detail.IS_GUI_BO_ME = false;
            else detail.IS_GUI_BO_ME = cboGuiBoMe.Checked ? true : false;
            if (cbSMS1.Checked == true) detail.NGAY_DK_KY1 = DateTime.Now;
            if (cbSMS2.Checked == true) detail.NGAY_DK_KY2 = DateTime.Now;
            detail.DIA_CHI = tbDiaChi.Text.Trim();
            detail.ID_TRUONG = Sys_This_Truong.ID;
            detail.ID_KHOI = Convert.ToInt16(rcbKhoi.SelectedValue);
            detail.MA_CAP_HOC = Sys_This_Cap_Hoc;
            detail.ID_NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
            detail.MA_KHU_VUC = localAPI.ConvertStringToShort(rcbKhuVuc.SelectedValue);
            detail.MA_TINH_THANH = localAPI.ConvertStringToShort(rcbTinhThanh.SelectedValue);
            detail.MA_QUAN_HUYEN = localAPI.ConvertStringToShort(rcbQuanHuyen.SelectedValue);
            detail.MA_XA_PHUONG = localAPI.ConvertStringToShort(rcbXaPhuong.SelectedValue);
            detail.MA_QUOC_TICH = localAPI.ConvertStringToShort(rcbQuocTich.SelectedValue);
            detail.MA_DAN_TOC = localAPI.ConvertStringToShort(rcbDanToc.SelectedValue);
            detail.MA_DOI_TUONG_CS = localAPI.ConvertStringToShort(rcbDoiTuongCS.SelectedValue);
            detail.SO_CMND = tbSoCMND.Text.Trim() == "" ? null : tbSoCMND.Text.Trim();
            detail.CHIEU_CAO = !string.IsNullOrEmpty(tbChieuCao.Text.Trim()) ? tbChieuCao.Text.Trim() : null;
            detail.CAN_NANG = !string.IsNullOrEmpty(tbCanNang.Text.Trim()) ? tbCanNang.Text.Trim() : null;
            foreach (UploadedFile f in rauAnh.UploadedFiles)
            {
                Guid strGuid = Guid.NewGuid();
                string fileName = strGuid.ToString() + f.GetName();
                string path = Server.MapPath("~/img/AnhDaiDienHS/" + fileName);
                f.SaveAs(path);
                detail.ANH_DAI_DIEN = "~/img/AnhDaiDienHS/" + fileName;
            }

            try
            {
                detail.NGAY_CAP_CMND = rdNgayCapCMND.SelectedDate.Value;
            }
            catch { }
            detail.NOI_CAP_CMND = tbNoiCapCMND.Text.Trim() == "" ? null : tbNoiCapCMND.Text.Trim();
            detail.NOI_SINH = tbNoiSinh.Text.Trim() != "" ? null : tbNoiSinh.Text.Trim();

            detail.IS_HOI_PHO_CHPH = cbHoiPho.Checked ? true : false;
            detail.IS_HOI_TRUONG_CHPH = cbHoiTruong.Checked ? true : false;

            List<string> lstSDT = new List<string>();
            if (!string.IsNullOrEmpty(detail.SDT_NHAN_TIN))
                lstSDT.Add(detail.SDT_NHAN_TIN.Trim());
            HOC_SINH hocSinh = hsBO.checkExistsHocSinh(Convert.ToInt16(Sys_Ma_Nam_hoc), Sys_This_Truong.ID, detail.ID_KHOI, detail.ID_LOP, detail.HO_TEN, lstSDT, detail.NGAY_SINH);
            if (hocSinh == null)
            {
                ResultEntity res = hsBO.insert(detail, Sys_User.ID);
                HOC_SINH resHocSinh = (HOC_SINH)res.ResObject;
                if (res.Res)
                {
                    strMsg = "notification('success', '" + res.Msg + "');";
                    logUserBO.insert(Sys_This_Truong.ID, "INSERT", "Thêm mới học sinh " + resHocSinh.ID, Sys_User.ID, DateTime.Now);
                    reset();
                }
                else
                {
                    strMsg = "notification('error', '" + res.Msg + "');";
                }
            }
            else
            {
                strMsg = "notification('warning', 'Học sinh này đã tồn tại!');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
        protected void btEdit_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
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
            if (detail.ID_TRUONG != Sys_This_Truong.ID)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện sửa dữ liệu này!');", true);
                return;
            }
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
            detail.HO_TEN_BO = tbTenBo.Text.Trim() == "" ? null : tbTenBo.Text.Trim();
            detail.HO_TEN_ME = tbTenMe.Text.Trim() == "" ? null : tbTenMe.Text.Trim();
            detail.HO_TEN_NGUOI_BAO_HO = tbTenNBH.Text.Trim() == "" ? null : tbTenNBH.Text.Trim();
            detail.NAM_SINH_BO = localAPI.ConvertStringToShort(tbNamSinhBo.Text);
            detail.NAM_SINH_ME = localAPI.ConvertStringToShort(tbNamSinhMe.Text);
            detail.NAM_SINH_NGUOI_BAO_HO = localAPI.ConvertStringToShort(tbNamSinhNBH.Text);
            detail.SDT_BO = tbSDT_Bo.Text.Trim() == "" ? null : localAPI.Add84(tbSDT_Bo.Text.Trim());
            detail.SDT_ME = tbSDT_Me.Text.Trim() == "" ? null : localAPI.Add84(tbSDT_Me.Text.Trim());
            detail.SDT_NBH = tbSDT_NBH.Text.Trim() == "" ? null : localAPI.Add84(tbSDT_NBH.Text.Trim());
            detail.IS_CON_GV = cbConGV.Checked;
            detail.IS_DK_KY1 = cbSMS1.Checked;
            detail.IS_DK_KY2 = cbSMS2.Checked;
            detail.IS_MIEN_GIAM_KY1 = cbMienPhi1.Checked;
            detail.IS_MIEN_GIAM_KY2 = cbMienPhi2.Checked;
            if (cbMienPhi1.Checked || cbMienPhi2.Checked) detail.IS_GUI_BO_ME = false;
            else detail.IS_GUI_BO_ME = cboGuiBoMe.Checked;
            detail.DIA_CHI = tbDiaChi.Text.Trim();
            detail.ID_TRUONG = Sys_This_Truong.ID;
            detail.ID_KHOI = Convert.ToInt16(rcbKhoi.SelectedValue);
            detail.MA_CAP_HOC = Sys_This_Cap_Hoc;
            detail.ID_NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
            detail.MA_KHU_VUC = localAPI.ConvertStringToShort(rcbKhuVuc.SelectedValue);
            detail.MA_TINH_THANH = localAPI.ConvertStringToShort(rcbTinhThanh.SelectedValue);
            detail.MA_QUAN_HUYEN = localAPI.ConvertStringToShort(rcbQuanHuyen.SelectedValue);
            detail.MA_XA_PHUONG = localAPI.ConvertStringToShort(rcbXaPhuong.SelectedValue);
            detail.MA_QUOC_TICH = localAPI.ConvertStringToShort(rcbQuocTich.SelectedValue);
            detail.MA_DAN_TOC = localAPI.ConvertStringToShort(rcbDanToc.SelectedValue);
            detail.MA_DOI_TUONG_CS = localAPI.ConvertStringToShort(rcbDoiTuongCS.SelectedValue);
            detail.SO_CMND = tbSoCMND.Text.Trim() == "" ? null : tbSoCMND.Text.Trim(); detail.CHIEU_CAO = !string.IsNullOrEmpty(tbChieuCao.Text.Trim()) ? tbChieuCao.Text.Trim() : null;
            detail.CAN_NANG = !string.IsNullOrEmpty(tbCanNang.Text.Trim()) ? tbCanNang.Text.Trim() : null;
            foreach (UploadedFile f in rauAnh.UploadedFiles)
            {
                Guid strGuid = Guid.NewGuid();
                string fileName = strGuid.ToString() + f.GetName();
                string path = Server.MapPath("~/img/AnhDaiDienHS/" + fileName);
                f.SaveAs(path);
                detail.ANH_DAI_DIEN = "~/img/AnhDaiDienHS/" + fileName;
            }

            try
            {
                detail.NGAY_CAP_CMND = rdNgayCapCMND.SelectedDate.Value;
            }
            catch { }
            detail.NOI_CAP_CMND = tbNoiCapCMND.Text.Trim() == "" ? null : tbNoiCapCMND.Text.Trim();
            detail.NOI_SINH = tbNoiSinh.Text.Trim() != "" ? null : tbNoiSinh.Text.Trim();

            detail.IS_HOI_PHO_CHPH = cbHoiPho.Checked ? true : false;
            detail.IS_HOI_TRUONG_CHPH = cbHoiTruong.Checked ? true : false;

            ResultEntity res = hsBO.update(detail, Sys_User.ID);
            if (res.Res)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
                logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật học sinh " + id_hs.Value, Sys_User.ID, DateTime.Now);
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
        protected void reset()
        {
            tbMa.Text = "";
            tbMa.Focus();
            tbTen.Text = "";
            rdNgaySinh.SelectedDate = null;
            rcbGioiTinh.ClearSelection();
            rcbTrangThai.ClearSelection();
            cbConGV.Checked = false;
            cbSMS1.Checked = false;
            cbSMS2.Checked = false;
            cbMienPhi1.Checked = false;
            cbMienPhi2.Checked = false;
            tbSDT_NhanTin.Text = "";
            rcbQuocTich.ClearSelection();
            rcbDanToc.ClearSelection();
            rcbDoiTuongCS.ClearSelection();
            rcbKhuVuc.ClearSelection();
            rcbTinhThanh.ClearSelection();
            rcbQuanHuyen.ClearSelection();
            rcbXaPhuong.ClearSelection();
            tbNoiSinh.Text = "";
            tbDiaChi.Text = "";
            tbSoCMND.Text = "";
            rdNgayCapCMND.SelectedDate = null;
            tbNoiCapCMND.Text = "";
            tbTenBo.Text = "";
            tbNamSinhBo.Text = "";
            tbSDT_Bo.Text = "";
            tbTenMe.Text = "";
            tbNamSinhMe.Text = "";
            tbSDT_Me.Text = "";
            tbTenNBH.Text = "";
            tbNamSinhNBH.Text = "";
            tbSDT_NBH.Text = "";
            getMaxThuTu();
        }
        protected void rcbKhoi_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbLop.ClearSelection();
            rcbLop.Text = string.Empty;
            rcbLop.DataBind();
            getMaxThuTu();
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
        protected void rcbLoaiLopGDTX_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            //rcbKhoi.DataSource = khoiBO.getKhoiByCapHocGDTX(rcbLoaiLopGDTX.SelectedValue, Sys_This_Cap_Hoc);
            rcbKhoi.ClearSelection();
            rcbKhoi.Text = string.Empty;
            rcbKhoi.DataBind();
            rcbLop.ClearSelection();
            rcbLop.Text = string.Empty;
            rcbLop.DataBind();
        }
        protected void rcbLop_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            getMaxThuTu();
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