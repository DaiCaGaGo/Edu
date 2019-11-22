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

namespace CMS.ChotSo
{
    public partial class ChotSoTheoMon : AuthenticatePage
    {
        private KhoaSoBO khoaSoBO = new KhoaSoBO();
        private LocalAPI localAPI = new LocalAPI();
        private NguoiDungBO nguoiDungBO = new NguoiDungBO();
        LogUserBO logUserBO = new LogUserBO();

        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            btEdit.Visible = is_access(SYS_Type_Access.THEM) || is_access(SYS_Type_Access.SUA);
            btnKhoaToanTruong.Visible = is_access(SYS_Type_Access.THEM) || is_access(SYS_Type_Access.SUA);
            btnMoKhoa.Visible = is_access(SYS_Type_Access.THEM) || is_access(SYS_Type_Access.SUA);
            giaiDoan.Visible = (Sys_This_Cap_Hoc == SYS_Cap_Hoc.TH) ? true : false;
            if (!IsPostBack)
            {
                objKhoi.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                objKhoi.SelectParameters.Add("maLoaiLopGDTX", Sys_This_Lop_GDTX == null ? "" : Sys_This_Lop_GDTX.ToString());
                rcbKhoiHoc.DataBind();
                objLop.SelectParameters.Add("ma_cap_hoc", Sys_This_Cap_Hoc);
                objLop.SelectParameters.Add("idTruong", Sys_This_Truong.ID.ToString());
                objLop.SelectParameters.Add("maNamHoc", Sys_Ma_Nam_hoc.ToString());
                rcbLop.DataBind();
                objKyDGTH.SelectParameters.Add("id_hocKy", Sys_Hoc_Ky.ToString());
                rcbKyDGTH.DataBind();
                checkKhoaToanTruong();
            }
        }
        protected void checkKhoaToanTruong()
        {
            KHOA_SO_THEO_TRUONG khoaTruong = new KHOA_SO_THEO_TRUONG();
            khoaTruong = khoaSoBO.getKhoaSoTheoTruongCapHocKy(Sys_This_Truong.ID, Sys_This_Cap_Hoc, Convert.ToInt16(Sys_Ma_Nam_hoc), Convert.ToInt16(Sys_Hoc_Ky), localAPI.ConvertStringToShort(rcbKyDGTH.SelectedValue));
            if (khoaTruong != null && !(khoaTruong.TRANG_THAI == null || khoaTruong.TRANG_THAI == false))
                btnMoKhoa.Visible = true;
            else btnMoKhoa.Visible = false;
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = khoaSoBO.khoaSoTheoMon(Sys_This_Truong.ID, localAPI.ConvertStringTolong(rcbLop.SelectedValue), Sys_This_Cap_Hoc, Convert.ToInt16(Sys_Ma_Nam_hoc), localAPI.ConvertStringToShort(rcbKyDGTH.SelectedValue), Convert.ToInt16(Sys_Hoc_Ky));
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                HiddenField hdNgayKhoa = (HiddenField)e.Item.FindControl("hdNgayKhoa");
                RadDatePicker ngayKhoa = (RadDatePicker)e.Item.FindControl("rdNgayKhoa");
                CheckBox ckIsCheck = (CheckBox)e.Item.FindControl("ckIsCheck");
                HiddenField hdIsCheck = (HiddenField)e.Item.FindControl("hdIsCheck");
                ckIsCheck.Checked = (hdIsCheck.Value == "" || hdIsCheck.Value == null || hdIsCheck.Value == "0") ? false : true;
                long? nguoi_khoa = localAPI.ConvertStringTolong(item["NGUOI_KHOA"].Text);
                item["NGUOI_KHOA"].Text = (ckIsCheck.Checked && nguoi_khoa != null) ? nguoiDungBO.getNguoiDung().FirstOrDefault(x => x.ID == nguoi_khoa).TEN_DANG_NHAP : "";
                if (ckIsCheck.Checked && hdNgayKhoa.Value != "")
                ngayKhoa.SelectedDate = Convert.ToDateTime(hdNgayKhoa.Value);
            }
        }
        protected void rcbKhoiHoc_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbLop.ClearSelection();
            rcbLop.Text = string.Empty;
            rcbLop.DataBind();
            checkKhoaToanTruong();
            RadGrid1.Rebind();
        }
        protected void rcbLop_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            checkKhoaToanTruong();
            RadGrid1.Rebind();
        }
        protected void rcbKyDGTH_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            checkKhoaToanTruong();
            RadGrid1.Rebind();
        }
        protected void btEdit_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.THEM) && !is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            string strMsg = "";
            int success = 0, error = 0;
            short? giai_doan = localAPI.ConvertStringToShort(rcbKyDGTH.SelectedValue);
            #region "khoa so theo mon"
            foreach (GridDataItem item in RadGrid1.Items)
            {
                #region "get control"
                //long? id_khoa_mon = localAPI.ConvertStringTolong(item.GetDataKeyValue("ID").ToString());
                HiddenField hdID_MON = (HiddenField)item.FindControl("hdID_MON");
                HiddenField hdID = (HiddenField)item.FindControl("hdID");
                RadDatePicker rdNgayKhoa = (RadDatePicker)item.FindControl("rdNgayKhoa");
                CheckBox ckIsCheckItem = (CheckBox)item.FindControl("ckIsCheck");
                HiddenField hdIsCheck = (HiddenField)item.FindControl("hdIsCheck");
                HiddenField hdNgayKhoa = (HiddenField)item.FindControl("hdNgayKhoa");

                DateTime? hdNgayKhoa_old = DateTime.Now;
                if (!string.IsNullOrEmpty(hdNgayKhoa.Value))
                    hdNgayKhoa_old = Convert.ToDateTime(hdNgayKhoa.Value);
                int? isCheck_old = localAPI.ConvertStringToint(hdIsCheck.Value);
                #endregion
                KHOA_SO_THEO_MON kstm = new KHOA_SO_THEO_MON();
                var ks = new KhoaSoTheoMonEntity();
                ks.NGAY_KHOA = rdNgayKhoa.SelectedDate;
                ks.TRANG_THAI = ckIsCheckItem.Checked ? 1 : 0;
                #region "insert"
                if (hdID.Value != null && hdID.Value != "")
                {
                    kstm = khoaSoBO.getKhoaSoMonByID(Convert.ToInt64(hdID.Value));
                    if (isCheck_old != ks.TRANG_THAI || (ks.NGAY_KHOA != null && hdNgayKhoa_old.Value.ToString("dd/MM/yyyy") != ks.NGAY_KHOA.Value.ToString("dd/MM/yyyy")))
                    {
                        if (ckIsCheckItem.Checked && rdNgayKhoa.SelectedDate < DateTime.Now.Date)
                        {
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Ngày khóa sổ không được nhỏ hơn ngày hiện tại!');", true);
                            return;
                        }
                        if (kstm != null)
                        {
                            kstm.TRANG_THAI = ks.TRANG_THAI == 1 ? true : false;
                            kstm.NGAY_KHOA = (ks.TRANG_THAI == 1) ? (ks.NGAY_KHOA == null ? DateTime.Now : ks.NGAY_KHOA) : null;
                            if (ks.TRANG_THAI == 1)
                                kstm.NGUOI_KHOA = Sys_User.ID;
                            else kstm.NGUOI_KHOA = null;
                            res = khoaSoBO.updateByMon(kstm, Sys_User.ID);
                            if (res.Res)
                            {
                                success++;
                                logUserBO.insert(Sys_This_Truong.ID, "CHỐT SỔ", "Cập nhật khóa sổ môn " + kstm.ID_MON + " lớp " + kstm.ID_LOP, Sys_User.ID, DateTime.Now);
                            }
                            else error++;
                        }
                    }
                }
                #endregion
                #region "update"
                else if (ckIsCheckItem.Checked)
                {
                    kstm = new KHOA_SO_THEO_MON();
                    kstm.ID_TRUONG = Sys_This_Truong.ID;
                    kstm.ID_LOP = localAPI.ConvertStringTolong(rcbLop.SelectedValue).Value;
                    kstm.ID_MON = Convert.ToInt64(hdID_MON.Value);
                    kstm.NGAY_KHOA = (ks.NGAY_KHOA == null) ? DateTime.Now : ks.NGAY_KHOA;
                    kstm.TRANG_THAI = ks.TRANG_THAI == 1 ? true : false;
                    kstm.NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
                    kstm.HOC_KY = Convert.ToInt16(Sys_Hoc_Ky);
                    kstm.MA_CAP_HOC = Sys_This_Cap_Hoc;
                    kstm.NGUOI_KHOA = Sys_User.ID;
                    if (Sys_This_Cap_Hoc == SYS_Cap_Hoc.TH)
                        kstm.GIAI_DOAN = localAPI.ConvertStringToShort(rcbKyDGTH.SelectedValue);
                    res = khoaSoBO.insertByMon(kstm, Sys_User.ID);
                    if (res.Res)
                    {
                        success++;
                        logUserBO.insert(Sys_This_Truong.ID, "CHỐT SỔ", "Cập nhật khóa sổ môn " + kstm.ID_MON + " lớp " + kstm.ID_LOP, Sys_User.ID, DateTime.Now);
                    }
                    else error++;
                }
                #endregion
            }
            if (success == 0) strMsg = "notification('warning', 'Không có môn nào cập nhật thành công!');";
            else strMsg = "notification('success', 'Có " + success + " môn cập nhật thành công!');";
            if (error > 0) strMsg = "notification('error', 'Có " + error + " môn không thể cập nhật!');";
            #endregion
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
        }
        protected void btnMoKhoa_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.THEM) && !is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            string strMsg = "";
            KHOA_SO_THEO_TRUONG khoaTruong = new KHOA_SO_THEO_TRUONG();
            khoaTruong = khoaSoBO.getKhoaSoTheoTruongCapHocKy(Sys_This_Truong.ID, Sys_This_Cap_Hoc, Convert.ToInt16(Sys_Ma_Nam_hoc), Convert.ToInt16(Sys_Hoc_Ky), localAPI.ConvertStringToShort(rcbKyDGTH.SelectedValue));
            if (khoaTruong != null && !(khoaTruong.TRANG_THAI == null || khoaTruong.TRANG_THAI == false))
            {
                res = khoaSoBO.updateTrangThaiMoKhoaTruong(khoaTruong.ID, Sys_User.ID, Sys_This_Truong.ID, Sys_This_Cap_Hoc, Convert.ToInt16(Sys_Ma_Nam_hoc), Convert.ToInt16(Sys_Hoc_Ky), localAPI.ConvertStringToShort(rcbKyDGTH.SelectedValue));
                if (res.Res)
                {
                    strMsg = "notification('success', 'Mở khóa thành công!');";
                    logUserBO.insert(Sys_This_Truong.ID, "CHỐT SỔ", "Mở khóa sổ toàn trường", Sys_User.ID, DateTime.Now);
                }
                else
                {
                    strMsg = "notification('error', 'Bạn không thể mở khóa. Vui lòng liên hệ với quản trị viên!');";
                }
            }
            else
            {
                strMsg = "notification('warning', 'Không có bản ghi nào được cập nhật!');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            btnMoKhoa.Visible = false;
            RadGrid1.Rebind();
        }
        protected void btnKhoaToanTruong_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.THEM) && !is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            string strMsg = "";

            bool is_khoa_Truong = false;
            KHOA_SO_THEO_TRUONG khoaTruong = new KHOA_SO_THEO_TRUONG();
            khoaTruong = khoaSoBO.getKhoaSoTheoTruongCapHocKy(Sys_This_Truong.ID, Sys_This_Cap_Hoc, Convert.ToInt16(Sys_Ma_Nam_hoc), Convert.ToInt16(Sys_Hoc_Ky), localAPI.ConvertStringToShort(rcbKyDGTH.SelectedValue));
            #region "them moi truong khoa so"
            if (khoaTruong == null)
            {
                khoaTruong = new KHOA_SO_THEO_TRUONG();
                khoaTruong.ID_TRUONG = Sys_This_Truong.ID;
                khoaTruong.NGAY_KHOA = DateTime.Now;
                khoaTruong.NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
                if (Sys_This_Cap_Hoc == SYS_Cap_Hoc.TH) khoaTruong.GIAI_DOAN = Convert.ToInt16(rcbKyDGTH.SelectedValue);
                khoaTruong.TRANG_THAI = true;
                khoaTruong.HOC_KY = Convert.ToInt16(Sys_Hoc_Ky);
                khoaTruong.NGUOI_KHOA = Sys_User.ID;
                khoaTruong.MA_CAP_HOC = Sys_This_Cap_Hoc;
                res = khoaSoBO.insertKhoaSoTheoTruong(khoaTruong, Sys_User.ID, Sys_This_Truong.ID, Sys_This_Cap_Hoc, Convert.ToInt16(Sys_Ma_Nam_hoc), Convert.ToInt16(Sys_Hoc_Ky), localAPI.ConvertStringToShort(rcbKyDGTH.SelectedValue));
                if (res.Res)
                {
                    is_khoa_Truong = true;
                    logUserBO.insert(Sys_This_Truong.ID, "CHỐT SỔ", "Khóa sổ toàn trường", Sys_User.ID, DateTime.Now);
                }
            }
            #endregion
            #region "update khoa so truong"
            else
            {
                res = khoaSoBO.updateTrangThaiKhoaTruong(khoaTruong.ID, Sys_User.ID, Sys_This_Truong.ID, Sys_This_Cap_Hoc, Convert.ToInt16(Sys_Ma_Nam_hoc), Convert.ToInt16(Sys_Hoc_Ky), localAPI.ConvertStringToShort(rcbKyDGTH.SelectedValue));
                if (res.Res)
                {
                    is_khoa_Truong = true;
                    logUserBO.insert(Sys_This_Truong.ID, "CHỐT SỔ", "Khóa sổ toàn trường", Sys_User.ID, DateTime.Now);
                }
            }
            #endregion
            if (is_khoa_Truong)
            {
                strMsg = "notification('success', 'Khóa sổ toàn trường thành công');";
            }
            else strMsg = "notification('error', 'Dữ liệu chưa được khóa. Vui lòng liên hệ với quản trị viên!');";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            btnMoKhoa.Visible = true;
            RadGrid1.Rebind();
        }
    }
}