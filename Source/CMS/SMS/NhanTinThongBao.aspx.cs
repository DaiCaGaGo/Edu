using ClosedXML.Excel;
using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using OneEduDataAccess.Static;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Telerik.Web.UI.Map;

namespace CMS.SMS
{
    public partial class NhanTinThongBao : AuthenticatePage
    {
        private HocSinhBO hocSinhBO = new HocSinhBO();
        private LocalAPI localAPI = new LocalAPI();
        private List<TongHopNhanXetHangNgayEntity> lstThEntity = new List<TongHopNhanXetHangNgayEntity>();
        private QuyTinBO quyTinBO = new QuyTinBO();
        private TinNhanBO tinNhanBO = new TinNhanBO();
        TruongBO truongBO = new TruongBO();
        LopBO lopBO = new LopBO();
        GiaoVienBO giaoVienBO = new GiaoVienBO();
        LogUserBO logUserBO = new LogUserBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            btGuiTuyChon.Visible = is_access(SYS_Type_Access.SEND_SMS) && !(Sys_This_Truong.IS_ACTIVE_SMS != true);
            btnSmsTuyBien.Visible = is_access(SYS_Type_Access.SEND_SMS);
            btExport.Visible = is_access(SYS_Type_Access.EXPORT);
            if (!IsPostBack)
            {
                objKhoiHoc.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                rcbKhoi.DataBind();
                objLop.SelectParameters.Add("ma_cap_hoc", Sys_This_Cap_Hoc);
                objLop.SelectParameters.Add("idTruong", Sys_This_Truong.ID.ToString());
                objLop.SelectParameters.Add("maNamHoc", Sys_Ma_Nam_hoc.ToString());
                rcbLop.DataBind();
                viewQuyTin();
            }
        }
        protected void viewQuyTin()
        {
            btGuiTuyChon.Visible = is_access(SYS_Type_Access.SEND_SMS) && !(Sys_This_Truong.IS_ACTIVE_SMS != true);
            short nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc);
            short thang = Convert.ToInt16(DateTime.Now.Month);
            if (thang < 8) nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc + 1);
            if (Sys_This_Truong.ID_DOI_TAC == null || Sys_This_Truong.ID_DOI_TAC == 0)
            {
                QUY_TIN quyTinTheoNam = quyTinBO.getQuyTinByNamHoc(Convert.ToInt16(localAPI.GetYearNow()), Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID);
                bool is_insert_new_quytb = false;
                QUY_TIN quyTinTheoThang = quyTinBO.getQuyTin(nam_hoc, thang, Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID, out is_insert_new_quytb);
                if (quyTinTheoNam != null && quyTinTheoThang != null)
                {
                    double tong_con_thang = quyTinTheoThang.TONG_CON + (quyTinTheoThang.TONG_CAP + quyTinTheoThang.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                    / 100;
                    double tong_con_nam = quyTinTheoNam.TONG_CON + (quyTinTheoNam.TONG_CAP + quyTinTheoNam.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 1 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                        / 100;
                    if (Sys_This_Truong.IS_ACTIVE_SMS != true || tong_con_nam <= 0)
                    {
                        //btGuiTuyChon.Visible = false;
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", " notification('warning', 'Đơn vị đã sử dụng vượt quỹ tin được cấp. Vui lòng liên hệ lại với quản trị viên!');", true);
                        //return;
                    }
                    else if (Sys_This_Truong.IS_ACTIVE_SMS != true || tong_con_thang <= 0)
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", " notification('warning', 'Đơn vị đã sử dụng vượt quỹ tin tháng!');", true);
                    }
                    txtTongQuyTinConLaiTheoNam.Text = "Quỹ tin còn lại theo năm: " + ((quyTinTheoNam == null) ? "0" : quyTinTheoNam.TONG_CON.ToString());
                    //txtTongQuyTinConLaiTheoThang.Text = "Quỹ tháng còn: " + (quyTinTheoThang == null ? "0" : (quyTinTheoThang.TONG_CON > quyTinTheoNam.TONG_CON) ? quyTinTheoNam.TONG_CON.ToString() : quyTinTheoThang.TONG_CON.ToString());
                    txtTongQuyTinConLaiTheoThang.Text = "Quỹ tháng còn: " + (quyTinTheoThang == null ? "0" : quyTinTheoThang.TONG_CON.ToString());
                }
                else
                {
                    btGuiTuyChon.Visible = false;
                    btnSmsTuyBien.Visible = false;
                    txtTongQuyTinConLaiTheoThang.Text = "Đơn vị không được cấp quota";
                    txtTongQuyTinConLaiTheoNam.Visible = false;
                }
            }
            else
            {
                TRUONG detailTruong = new TRUONG();
                long tong_tin_con = 0;
                detailTruong = truongBO.getTruongById(Sys_This_Truong.ID);
                if (detailTruong != null)
                {
                    txtTongQuyTinConLaiTheoNam.Text = "Tổng tin cấp: " + (detailTruong.TONG_TIN_CAP == null ? "0" : detailTruong.TONG_TIN_CAP.ToString());
                    if (detailTruong.TONG_TIN_CAP != null)
                    {
                        tong_tin_con = detailTruong.TONG_TIN_DA_DUNG != null ? (detailTruong.TONG_TIN_CAP.Value - detailTruong.TONG_TIN_DA_DUNG.Value) : detailTruong.TONG_TIN_CAP.Value;
                    }
                    else tong_tin_con = detailTruong.TONG_TIN_DA_DUNG != null ? (-detailTruong.TONG_TIN_DA_DUNG.Value) : 0;
                    txtTongQuyTinConLaiTheoThang.Text = "Tổng tin còn: " + tong_tin_con;
                    if (Sys_This_Truong.IS_ACTIVE_SMS != true)
                    {
                        btGuiTuyChon.Visible = false;
                        btnSmsTuyBien.Visible = false;
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", " notification('warning', 'Đơn vị chưa đăng ký sử dụng SMS nên không thể gửi tin nhắn!');", true);
                    }
                }
                if (tong_tin_con <= 0)
                {
                    btGuiTuyChon.Visible = false;
                    btnSmsTuyBien.Visible = false;
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", " notification('warning', 'Đơn vị đã sử dụng hết quỹ tin được cấp. Vui lòng liên hệ lại với quản trị viên!');", true);
                }
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            List<HocSinhLopEntity> lstHS = new List<HocSinhLopEntity>();

            List<short> lst_ma_khoi = new List<short>();
            foreach (var item in rcbKhoi.CheckedItems)
            {
                lst_ma_khoi.Add(localAPI.ConvertStringToShort(item.Value).Value);
            }
            List<long> lst_id_lop = new List<long>();
            foreach (var item in rcbLop.CheckedItems)
            {
                lst_id_lop.Add(localAPI.ConvertStringTolong(item.Value).Value);
            }

            lstHS = hocSinhBO.getHocSinhGuiThongBaoHangNgayByListKhoiLop(Sys_This_Truong.ID, lst_ma_khoi, lst_id_lop, Convert.ToInt16(Sys_Ma_Nam_hoc), Sys_This_Cap_Hoc, Convert.ToInt16(Sys_Hoc_Ky));
            //lstHS = hocSinhBO.getHocSinhGuiTinThongBao(Sys_This_Truong.ID, lst_ma_khoi, lst_id_lop, Convert.ToInt16(Sys_Ma_Nam_hoc), Sys_This_Cap_Hoc, Convert.ToInt16(Sys_Hoc_Ky));
            RadGrid1.DataSource = lstHS;
            RadGrid1.MasterTableView.Columns.FindByUniqueName("SDT_BM").Display = is_access(SYS_Type_Access.VIEW_INFOR);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "countSMSDuTinhNew();SetGridHeight();", true);
        }
        protected void rcbKhoi_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            string lst_ma_khoi = "";
            foreach (var item in rcbKhoi.CheckedItems)
            {
                if (string.IsNullOrEmpty(lst_ma_khoi))
                    lst_ma_khoi += item.Value;
                else lst_ma_khoi += "," + item.Value;
            }
            hdKhoi.Value = lst_ma_khoi;
            rcbLop.ClearSelection();
            rcbLop.Text = String.Empty;
            rcbLop.DataBind();
            RadGrid1.Rebind();
        }
        protected void rcbLop_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void btGuiTuyChon_Click(object sender, EventArgs e)
        {
            #region "check quyen"
            if (!is_access(SYS_Type_Access.SEND_SMS))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            if (Sys_This_Truong.IS_ACTIVE_SMS != true)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị đang không đăng ký dịch vụ!');", true);
                return;
            }
            DateTime dt = new DateTime();
            if (cbHenGioGuiTin.Checked)
            {
                if (string.IsNullOrEmpty(tbTime.Text))
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Thời gian không được để trống.');", true);
                    return;
                }
                else if (Convert.ToDateTime(tbTime.Text) <= DateTime.Now)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Thời gian hẹn giờ không được nhỏ hơn thời gian hiện tại.');", true);
                    return;
                }
                else
                {
                    dt = Convert.ToDateTime(tbTime.Text);
                }
            }
            #endregion
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            List<TIN_NHAN> lstTinNhan = new List<TIN_NHAN>();
            int tong_tin_gui = 0, count_sms = 0;
            List<long> lstIDLop = new List<long>();

            int countHS = 0, countGVCN = 0, countCBNV = 0, countGuiKem = 0;
            string brandname = "", cp = "";
            if (RadGrid1.SelectedItems != null && RadGrid1.SelectedItems.Count > 0)
            {
                #region Lấy danh sách tin từ grid
                foreach (GridDataItem item in RadGrid1.SelectedItems)
                {
                    long id_lop = Convert.ToInt64(item["ID_LOP"].Text);
                    bool is_gui_bo_me = false;
                    if (item["IS_GUI_BO_ME"].Text == "1") is_gui_bo_me = true;
                    string sdt = item["SDT_NHAN_TIN"].Text;
                    string sdt_k = item["SDT_NHAN_TIN2"].Text;
                    long? id_hoc_sinh = Convert.ToInt64(item.GetDataKeyValue("ID").ToString());

                    TextBox tbNoiDungTB = (TextBox)item.FindControl("tbNoiDungTB");
                    if (RadGrid1.SelectedItems == null || RadGrid1.SelectedItems.Count == 0)
                        tbNoiDungTB.Text = tbNoiDung.Text;

                    short? is_dk1 = localAPI.ConvertStringToShort(item["IS_DK_KY1"].Text);
                    short? is_dk2 = localAPI.ConvertStringToShort(item["IS_DK_KY2"].Text);
                    bool is_sms = false;
                    if (is_dk1 != null && is_dk1 == 1 && Sys_Hoc_Ky == 1) is_sms = true;
                    else if (is_dk2 != null && is_dk2 == 1 && Sys_Hoc_Ky == 2) is_sms = true;

                    string noi_dung = tbNoiDungTB.Text.Trim();
                    string noi_dung_khong_dau = localAPI.chuyenTiengVietKhongDau(noi_dung);
                    if (id_hoc_sinh != null && is_sms && !string.IsNullOrEmpty(noi_dung))
                    {
                        #region Tin 1
                        TIN_NHAN tinDetail = new TIN_NHAN();
                        tinDetail.ID_TRUONG = Sys_This_Truong.ID;
                        tinDetail.ID_NGUOI_NHAN = id_hoc_sinh;
                        tinDetail.LOAI_NGUOI_NHAN = 1;
                        tinDetail.SDT_NHAN = sdt;
                        tinDetail.MA_GOI_TIN = Sys_This_Truong.MA_GOI_TIN;
                        tinDetail.THOI_GIAN_GUI = (cbHenGioGuiTin.Checked && !string.IsNullOrEmpty(tbTime.Text)) ? dt : DateTime.Now;
                        tinDetail.NGUOI_GUI = Sys_User.ID;
                        tinDetail.LOAI_TIN = SYS_Loai_Tin.Tin_Thong_Bao;
                        tinDetail.KIEU_GUI = 1;
                        tinDetail.NAM_GUI = Convert.ToInt16(DateTime.Now.Year);
                        tinDetail.THANG_GUI = Convert.ToInt16(DateTime.Now.Month);
                        tinDetail.TUAN_GUI = Convert.ToInt16(localAPI.getThisWeek().ToString());
                        tinDetail.NOI_DUNG = noi_dung;
                        tinDetail.NOI_DUNG_KHONG_DAU = noi_dung_khong_dau;
                        tinDetail.SO_TIN = localAPI.demSoTin(noi_dung);
                        TIN_NHAN checkExists = new TIN_NHAN();
                        string loaiNhaMang = localAPI.getLoaiNhaMang(sdt);
                        if (!string.IsNullOrEmpty(sdt) && !string.IsNullOrEmpty(loaiNhaMang))
                        {
                            #region check ton tai cung ma hoc sinh, cung so dien thoai, cung noi dung tin nhan
                            if ((cbHenGioGuiTin.Checked && !string.IsNullOrEmpty(tbTime.Text)))
                                checkExists = tinNhanBO.checkExistsHocSinhByMaAndSDTAndNoiDung(tinDetail.ID_NGUOI_NHAN.Value, tinDetail.LOAI_NGUOI_NHAN, sdt, SYS_Loai_Tin.Tin_Thong_Bao, dt, Sys_Time_Send, noi_dung_khong_dau);
                            else
                                checkExists = tinNhanBO.checkExistsHocSinhByMaAndSDTAndNoiDung(tinDetail.ID_NGUOI_NHAN.Value, tinDetail.LOAI_NGUOI_NHAN, sdt, SYS_Loai_Tin.Tin_Thong_Bao, null, Sys_Time_Send, noi_dung_khong_dau);

                            if (checkExists != null) continue;
                            #endregion
                            tinDetail.LOAI_NHA_MANG = loaiNhaMang;
                            brandname = ""; cp = "";
                            getBrandnameAndCp(loaiNhaMang, out brandname, out cp);
                            tinDetail.BRAND_NAME = brandname;
                            tinDetail.CP = cp;
                            if (Sys_This_Truong.ID_DOI_TAC != null && Sys_This_Truong.ID_DOI_TAC > 0)
                                tinDetail.ID_DOI_TAC = Sys_This_Truong.ID_DOI_TAC;
                            else tinDetail.ID_DOI_TAC = null;
                            lstTinNhan.Add(tinDetail);
                            tong_tin_gui += localAPI.demSoTin(noi_dung);
                            countHS++;
                        }
                        #endregion
                        #region Tin 2
                        string loaiNhaMang_k = localAPI.getLoaiNhaMang(sdt_k);
                        if (is_gui_bo_me && !string.IsNullOrEmpty(sdt_k) && !string.IsNullOrEmpty(loaiNhaMang_k))
                        {
                            #region check ton tai cung ma hoc sinh, cung so dien thoai, cung noi dung tin nhan
                            if ((cbHenGioGuiTin.Checked && !string.IsNullOrEmpty(tbTime.Text)))
                                checkExists = tinNhanBO.checkExistsHocSinhByMaAndSDTAndNoiDung(tinDetail.ID_NGUOI_NHAN.Value, tinDetail.LOAI_NGUOI_NHAN, sdt_k, SYS_Loai_Tin.Tin_Thong_Bao, dt, Sys_Time_Send, noi_dung_khong_dau);
                            else
                                checkExists = tinNhanBO.checkExistsHocSinhByMaAndSDTAndNoiDung(tinDetail.ID_NGUOI_NHAN.Value, tinDetail.LOAI_NGUOI_NHAN, sdt_k, SYS_Loai_Tin.Tin_Thong_Bao, null, Sys_Time_Send, noi_dung_khong_dau);

                            if (checkExists != null) continue;
                            #endregion
                            #region Tạo tin 2
                            TIN_NHAN tinDetailK = new TIN_NHAN()
                            {
                                ID_DOI_TAC = tinDetail.ID_DOI_TAC,
                                ID_NGUOI_NHAN = tinDetail.ID_NGUOI_NHAN,
                                ID_NHAN_XET_HANG_NGAY = tinDetail.ID_NHAN_XET_HANG_NGAY,
                                ID_THONG_BAO = tinDetail.ID_THONG_BAO,
                                ID_TONG_HOP_NXHN = tinDetail.ID_TONG_HOP_NXHN,
                                ID_TRUONG = tinDetail.ID_TRUONG,
                                IS_DA_NHAN = tinDetail.IS_DA_NHAN,
                                IS_UNICODE = tinDetail.IS_UNICODE,
                                KIEU_GUI = tinDetail.KIEU_GUI,
                                LOAI_NGUOI_NHAN = tinDetail.LOAI_NGUOI_NHAN,
                                LOAI_NHA_MANG = tinDetail.LOAI_NHA_MANG,
                                MA_GOI_TIN = tinDetail.MA_GOI_TIN,
                                NOI_DUNG = tinDetail.NOI_DUNG,
                                NOI_DUNG_KHONG_DAU = tinDetail.NOI_DUNG_KHONG_DAU,
                                SO_TIN = tinDetail.SO_TIN,
                                THOI_GIAN_GUI = tinDetail.THOI_GIAN_GUI,
                                NGUOI_GUI = tinDetail.NGUOI_GUI,
                                LOAI_TIN = tinDetail.LOAI_TIN,
                                NAM_GUI = tinDetail.NAM_GUI,
                                SEND_NUMBER = tinDetail.SEND_NUMBER,
                                THANG_GUI = tinDetail.THANG_GUI,
                                TUAN_GUI = tinDetail.TUAN_GUI
                            };
                            tinDetailK.SDT_NHAN = sdt_k;
                            tinDetailK.LOAI_NHA_MANG = loaiNhaMang_k;
                            brandname = ""; cp = "";
                            getBrandnameAndCp(loaiNhaMang_k, out brandname, out cp);
                            tinDetail.BRAND_NAME = brandname;
                            tinDetail.CP = cp;
                            #endregion
                            lstTinNhan.Add(tinDetailK);
                            tong_tin_gui += localAPI.demSoTin(noi_dung);
                            countHS++;
                        }
                        #endregion
                        #region "add list lop"
                        if (!lstIDLop.Contains(id_lop))
                        {
                            lstIDLop.Add(id_lop);
                        }
                        #endregion
                    }
                }
                #endregion
                string noi_dung_guiKem = tbNoiDung.Text.Trim();
                string noi_dung_guiKem_en = localAPI.chuyenTiengVietKhongDau(noi_dung_guiKem);
                List<LopEntity> lstLopInTruong = lopBO.getLopGVCNByTruongCapNamHoc(Sys_This_Cap_Hoc, Sys_This_Truong.ID, Convert.ToInt16(Sys_Ma_Nam_hoc));
                if (lstTinNhan.Count > 0 && !string.IsNullOrEmpty(noi_dung_guiKem) && lstLopInTruong != null && lstLopInTruong.Count > 0)
                {
                    #region Gửi GVCN
                    if (cboGuiGVCN.Checked)
                    {
                        for (int i = 0; i < lstIDLop.Count; i++)
                        {
                            LopEntity lop = lstLopInTruong.FirstOrDefault(x => x.ID_LOP == lstIDLop[i]);
                            if (lop != null)
                            {
                                string sdt_gv = lop.SDT_GVCN != null ? lop.SDT_GVCN : "";
                                string loaiNhaMang_gv = localAPI.getLoaiNhaMang(sdt_gv);
                                if (!string.IsNullOrEmpty(sdt_gv) && !string.IsNullOrEmpty(loaiNhaMang_gv))
                                {
                                    TIN_NHAN checkExists = new TIN_NHAN();
                                    checkExists = lstTinNhan.FirstOrDefault(x => x.SDT_NHAN == sdt_gv);
                                    if (checkExists == null)
                                    {
                                        TIN_NHAN tinDetail = new TIN_NHAN();
                                        tinDetail.ID_TRUONG = Sys_This_Truong.ID;
                                        tinDetail.ID_NGUOI_NHAN = lop.ID_GVCN;
                                        tinDetail.LOAI_NGUOI_NHAN = 2;
                                        tinDetail.SDT_NHAN = sdt_gv;
                                        tinDetail.MA_GOI_TIN = Sys_This_Truong.MA_GOI_TIN;
                                        tinDetail.THOI_GIAN_GUI = (cbHenGioGuiTin.Checked && !string.IsNullOrEmpty(tbTime.Text)) ? dt : DateTime.Now;
                                        tinDetail.NGUOI_GUI = Sys_User.ID;
                                        tinDetail.LOAI_TIN = SYS_Loai_Tin.Tin_Thong_Bao;
                                        tinDetail.KIEU_GUI = 1;
                                        tinDetail.NAM_GUI = Convert.ToInt16(DateTime.Now.Year);
                                        tinDetail.THANG_GUI = Convert.ToInt16(DateTime.Now.Month);
                                        tinDetail.TUAN_GUI = Convert.ToInt16(localAPI.getThisWeek().ToString());
                                        tinDetail.NOI_DUNG = noi_dung_guiKem;
                                        tinDetail.NOI_DUNG_KHONG_DAU = noi_dung_guiKem_en;
                                        tinDetail.SO_TIN = localAPI.demSoTin(noi_dung_guiKem_en);
                                        #region check ton tai cung ma hoc sinh, cung so dien thoai, cung noi dung tin nhan
                                        if ((cbHenGioGuiTin.Checked && !string.IsNullOrEmpty(tbTime.Text)))
                                            checkExists = tinNhanBO.checkExistsHocSinhByMaAndSDTAndNoiDung(tinDetail.ID_NGUOI_NHAN.Value, tinDetail.LOAI_NGUOI_NHAN, sdt_gv, SYS_Loai_Tin.Tin_Thong_Bao, dt, Sys_Time_Send, noi_dung_guiKem_en);
                                        else
                                            checkExists = tinNhanBO.checkExistsHocSinhByMaAndSDTAndNoiDung(tinDetail.ID_NGUOI_NHAN.Value, tinDetail.LOAI_NGUOI_NHAN, sdt_gv, SYS_Loai_Tin.Tin_Thong_Bao, null, Sys_Time_Send, noi_dung_guiKem_en);
                                        if (checkExists != null) continue;
                                        #endregion
                                        tinDetail.LOAI_NHA_MANG = loaiNhaMang_gv;
                                        brandname = ""; cp = "";
                                        getBrandnameAndCp(loaiNhaMang_gv, out brandname, out cp);
                                        tinDetail.BRAND_NAME = brandname;
                                        tinDetail.CP = cp;
                                        if (Sys_This_Truong.ID_DOI_TAC != null && Sys_This_Truong.ID_DOI_TAC > 0)
                                            tinDetail.ID_DOI_TAC = Sys_This_Truong.ID_DOI_TAC;
                                        else tinDetail.ID_DOI_TAC = null;
                                        lstTinNhan.Add(tinDetail);
                                        tong_tin_gui += localAPI.demSoTin(noi_dung_guiKem);
                                        countGVCN++;
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    #region Gửi CBNV theo chức vụ
                    List<GIAO_VIEN> lstCBNV = new List<GIAO_VIEN>();
                    if (!string.IsNullOrEmpty(noi_dung_guiKem))
                    {
                        foreach (var item in rcbGuiCBNV.CheckedItems)
                        {
                            lstCBNV = giaoVienBO.getGiaoVienByChucVu(Sys_This_Truong.ID, Convert.ToInt16(item.Value));
                            if (lstCBNV.Count > 0)
                            {
                                for (int i = 0; i < lstCBNV.Count; i++)
                                {
                                    string sdt_CBNV = lstCBNV[i].SDT;
                                    if (!string.IsNullOrEmpty(sdt_CBNV))
                                    {
                                        string loai_nha_mang_cbnv = localAPI.getLoaiNhaMang(sdt_CBNV);
                                        if (!string.IsNullOrEmpty(loai_nha_mang_cbnv))
                                        {
                                            TIN_NHAN checkExists = new TIN_NHAN();
                                            checkExists = lstTinNhan.FirstOrDefault(x => x.SDT_NHAN == sdt_CBNV);
                                            if (checkExists == null)
                                            {
                                                TIN_NHAN tinDetail = new TIN_NHAN();
                                                tinDetail.ID_TRUONG = Sys_This_Truong.ID;
                                                tinDetail.ID_NGUOI_NHAN = lstCBNV[i].ID;
                                                tinDetail.LOAI_NGUOI_NHAN = 2;
                                                tinDetail.SDT_NHAN = sdt_CBNV;
                                                tinDetail.MA_GOI_TIN = Sys_This_Truong.MA_GOI_TIN;
                                                tinDetail.THOI_GIAN_GUI = (cbHenGioGuiTin.Checked && !string.IsNullOrEmpty(tbTime.Text)) ? dt : DateTime.Now;
                                                tinDetail.NGUOI_GUI = Sys_User.ID;
                                                tinDetail.LOAI_TIN = SYS_Loai_Tin.Tin_Thong_Bao;
                                                tinDetail.KIEU_GUI = 1;
                                                tinDetail.NAM_GUI = Convert.ToInt16(DateTime.Now.Year);
                                                tinDetail.THANG_GUI = Convert.ToInt16(DateTime.Now.Month);
                                                tinDetail.TUAN_GUI = Convert.ToInt16(localAPI.getThisWeek().ToString());
                                                tinDetail.NOI_DUNG = noi_dung_guiKem;
                                                tinDetail.NOI_DUNG_KHONG_DAU = noi_dung_guiKem_en;
                                                tinDetail.SO_TIN = localAPI.demSoTin(noi_dung_guiKem);
                                                #region check ton tai cung ma hoc sinh, cung so dien thoai, cung noi dung tin nhan
                                                checkExists = new TIN_NHAN();
                                                if ((cbHenGioGuiTin.Checked && !string.IsNullOrEmpty(tbTime.Text)))
                                                    checkExists = tinNhanBO.checkExistsHocSinhByMaAndSDTAndNoiDung(tinDetail.ID_NGUOI_NHAN.Value, tinDetail.LOAI_NGUOI_NHAN, sdt_CBNV, SYS_Loai_Tin.Tin_Thong_Bao, dt, Sys_Time_Send, noi_dung_guiKem_en);
                                                else
                                                    checkExists = tinNhanBO.checkExistsHocSinhByMaAndSDTAndNoiDung(tinDetail.ID_NGUOI_NHAN.Value, tinDetail.LOAI_NGUOI_NHAN, sdt_CBNV, SYS_Loai_Tin.Tin_Thong_Bao, null, Sys_Time_Send, noi_dung_guiKem_en);
                                                if (checkExists != null) continue;
                                                #endregion
                                                tinDetail.LOAI_NHA_MANG = loai_nha_mang_cbnv;
                                                brandname = ""; cp = "";
                                                getBrandnameAndCp(loai_nha_mang_cbnv, out brandname, out cp);
                                                tinDetail.BRAND_NAME = brandname;
                                                tinDetail.CP = cp;
                                                if (Sys_This_Truong.ID_DOI_TAC != null && Sys_This_Truong.ID_DOI_TAC > 0)
                                                    tinDetail.ID_DOI_TAC = Sys_This_Truong.ID_DOI_TAC;
                                                else tinDetail.ID_DOI_TAC = null;
                                                lstTinNhan.Add(tinDetail);
                                                tong_tin_gui += localAPI.demSoTin(noi_dung_guiKem);
                                                countCBNV++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    #region Gửi kèm
                    List<string> lstsdtGuiKem = new List<string>();
                    if (!string.IsNullOrEmpty(tbListSDT.Text.Trim()) && !string.IsNullOrEmpty(noi_dung_guiKem))
                    {
                        lstsdtGuiKem = tbListSDT.Text.Trim().Split(';').ToList();
                        foreach (var item in lstsdtGuiKem)
                        {
                            string sdtGuiKem = localAPI.Add84(item.Trim());

                            TIN_NHAN checkExists = new TIN_NHAN();
                            checkExists = lstTinNhan.FirstOrDefault(x => x.SDT_NHAN == sdtGuiKem);
                            if (checkExists != null) continue;

                            string loai_nha_mang_gui_kem = localAPI.getLoaiNhaMang(sdtGuiKem);
                            if (!string.IsNullOrEmpty(loai_nha_mang_gui_kem))
                            {
                                TIN_NHAN tinDetail = new TIN_NHAN();
                                tinDetail.ID_TRUONG = Sys_This_Truong.ID;
                                tinDetail.LOAI_NGUOI_NHAN = 2;
                                tinDetail.SDT_NHAN = sdtGuiKem;
                                tinDetail.MA_GOI_TIN = Sys_This_Truong.MA_GOI_TIN;
                                tinDetail.THOI_GIAN_GUI = (cbHenGioGuiTin.Checked && !string.IsNullOrEmpty(tbTime.Text)) ? dt : DateTime.Now;
                                tinDetail.NGUOI_GUI = Sys_User.ID;
                                tinDetail.LOAI_TIN = SYS_Loai_Tin.Tin_Thong_Bao;
                                tinDetail.KIEU_GUI = 1;
                                tinDetail.NAM_GUI = Convert.ToInt16(DateTime.Now.Year);
                                tinDetail.THANG_GUI = Convert.ToInt16(DateTime.Now.Month);
                                tinDetail.TUAN_GUI = Convert.ToInt16(localAPI.getThisWeek().ToString());
                                tinDetail.NOI_DUNG = noi_dung_guiKem;
                                tinDetail.NOI_DUNG_KHONG_DAU = noi_dung_guiKem_en;
                                tinDetail.SO_TIN = localAPI.demSoTin(noi_dung_guiKem);
                                #region check ton tai cung ma hoc sinh, cung so dien thoai, cung noi dung tin nhan
                                checkExists = new TIN_NHAN();
                                if ((cbHenGioGuiTin.Checked && !string.IsNullOrEmpty(tbTime.Text)))
                                    checkExists = tinNhanBO.checkExistsHocSinhByMaAndSDTAndNoiDung(tinDetail.ID_NGUOI_NHAN, tinDetail.LOAI_NGUOI_NHAN, sdtGuiKem, SYS_Loai_Tin.Tin_Thong_Bao, dt, Sys_Time_Send, noi_dung_guiKem_en);
                                else
                                    checkExists = tinNhanBO.checkExistsHocSinhByMaAndSDTAndNoiDung(tinDetail.ID_NGUOI_NHAN, tinDetail.LOAI_NGUOI_NHAN, sdtGuiKem, SYS_Loai_Tin.Tin_Thong_Bao, null, Sys_Time_Send, noi_dung_guiKem_en);
                                if (checkExists != null) continue;
                                #endregion
                                tinDetail.LOAI_NHA_MANG = loai_nha_mang_gui_kem;
                                brandname = ""; cp = "";
                                getBrandnameAndCp(loai_nha_mang_gui_kem, out brandname, out cp);
                                tinDetail.BRAND_NAME = brandname;
                                tinDetail.CP = cp;
                                if (Sys_This_Truong.ID_DOI_TAC != null && Sys_This_Truong.ID_DOI_TAC > 0)
                                    tinDetail.ID_DOI_TAC = Sys_This_Truong.ID_DOI_TAC;
                                else tinDetail.ID_DOI_TAC = null;
                                lstTinNhan.Add(tinDetail);
                                tong_tin_gui += localAPI.demSoTin(noi_dung_guiKem);
                                countGuiKem++;
                            }
                        }
                    }
                    #endregion
                }
                #region save sms
                count_sms = lstTinNhan.Count;
                if (count_sms > 0)
                {
                    short nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc);
                    short thang = Convert.ToInt16(DateTime.Now.Month);
                    if (thang < 8) nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc + 1);
                    if (Sys_This_Truong.ID_DOI_TAC == null || Sys_This_Truong.ID_DOI_TAC == 0)
                    {
                        #region Tính quỹ tin

                        QUY_TIN quyTinTheoNam = quyTinBO.getQuyTinByNamHoc(Convert.ToInt16(localAPI.GetYearNow()), Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID);
                        bool is_insert_new_quytb = false;
                        QUY_TIN quyTinTheoThang = quyTinBO.getQuyTin(nam_hoc, thang, Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID, out is_insert_new_quytb);

                        double tong_con_thang = quyTinTheoThang.TONG_CON + ((quyTinTheoThang.TONG_CAP + quyTinTheoThang.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                    / 100);
                        double tong_con_nam = quyTinTheoNam.TONG_CON + ((quyTinTheoNam.TONG_CAP + quyTinTheoNam.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                            / 100);
                        #endregion
                        if (Sys_This_Truong.IS_SAN_QUY_TIN_NAM == true)
                        {
                            if (tong_tin_gui > tong_con_nam)
                            {
                                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị không còn đủ quota để gửi tin nhắn. Vui lòng liên hệ với quản trị viên!');", true);
                                return;
                            }
                            else
                            {
                                res = tinNhanBO.insertTinNhanThongBao(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, 0, Sys_User.ID);
                                //res = tinNhanBO.guiTinNhan(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, Sys_User.ID, tong_tin_gui, SYS_Loai_Tin.Tin_Thong_Bao);
                                if (res.Res)
                                {
                                    logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi tin thông báo: gửi tùy chọn " + tong_tin_gui + " tin nhắn", Sys_User.ID, DateTime.Now);
                                }
                                if (tong_tin_gui >= tong_con_thang)
                                {
                                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Đơn vị đã sử dụng vượt quá quỹ tin tháng!');", true);
                                }
                            }

                        }
                        else
                        {
                            if (tong_tin_gui > tong_con_thang || tong_tin_gui > tong_con_nam)
                            //if (tong_tin_gui > tong_con_thang)
                            {
                                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị đã sử dụng vượt quá quỹ tin tháng!');", true);
                                return;
                            }
                            else
                            {
                                res = tinNhanBO.insertTinNhanThongBao(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, 0, Sys_User.ID);
                                if (res.Res)
                                {
                                    logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi tin thông báo: gửi tùy chọn " + tong_tin_gui + " tin nhắn", Sys_User.ID, DateTime.Now);
                                }
                            }
                        }
                    }
                    else
                    {
                        TRUONG detailTruong = new TRUONG();
                        long tong_tin_con = 0;
                        detailTruong = truongBO.getTruongById(Sys_This_Truong.ID);
                        if (detailTruong != null)
                        {
                            txtTongQuyTinConLaiTheoNam.Text = "Tổng tin cấp: " + (detailTruong.TONG_TIN_CAP == null ? "0" : detailTruong.TONG_TIN_CAP.ToString());
                            if (detailTruong.TONG_TIN_CAP != null)
                            {
                                tong_tin_con = detailTruong.TONG_TIN_DA_DUNG != null ? (detailTruong.TONG_TIN_CAP.Value - detailTruong.TONG_TIN_DA_DUNG.Value) : detailTruong.TONG_TIN_CAP.Value;
                            }
                            else tong_tin_con = detailTruong.TONG_TIN_DA_DUNG != null ? (-detailTruong.TONG_TIN_DA_DUNG.Value) : 0;
                            if (Sys_This_Truong.IS_ACTIVE_SMS != null && Sys_This_Truong.IS_ACTIVE_SMS == true && tong_tin_con > tong_tin_gui)
                            {
                                res = tinNhanBO.insertTinNhanThongBao(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, 0, Sys_User.ID);
                                if (res.Res)
                                {
                                    logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi tin thông báo: gửi tùy chọn " + tong_tin_gui + " tin nhắn", Sys_User.ID, DateTime.Now);
                                }
                            }
                        }
                    }
                }
                #endregion
                #region Thông báo
                string strMsg = "";
                if (!res.Res && count_sms > 0)
                {
                    strMsg = res.Msg;
                }
                else if (count_sms == 0)
                {
                    strMsg = " notification('warning', 'Không có tin nào được gửi đi.');";
                }
                else
                {
                    strMsg = " notification('success', 'Có " + tong_tin_gui + " số điện thoại được gửi.');";
                }
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
                #endregion
                viewQuyTin();
            }
            else
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Bạn chưa chọn học sinh cần gửi tin');", true);
            RadGrid1.Rebind();
        }
        protected void cbHenGioGuiTin_CheckedChanged(object sender, EventArgs e)
        {
            divTime.Visible = cbHenGioGuiTin.Checked;
        }
        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                TextBox tbNoiDungTB = (TextBox)item.FindControl("tbNoiDungTB");
                #region đỏ: không đk, xanh: Con gv, M: miễn, BM: đk cả bố mẹ
                short? is_bo_me = localAPI.ConvertStringToShort(item["IS_GUI_BO_ME"].Text);
                short? is_dk1 = localAPI.ConvertStringToShort(item["IS_DK_KY1"].Text);
                short? is_dk2 = localAPI.ConvertStringToShort(item["IS_DK_KY2"].Text);
                short? is_mien1 = localAPI.ConvertStringToShort(item["IS_MIEN_GIAM_KY1"].Text);
                short? is_mien2 = localAPI.ConvertStringToShort(item["IS_MIEN_GIAM_KY2"].Text);
                short? is_con_gv = localAPI.ConvertStringToShort(item["IS_CON_GV"].Text);
                if (is_con_gv != null && is_con_gv == 1) item.ForeColor = Color.Blue;

                if (Sys_Hoc_Ky == 1 && is_mien1 == 1) item["HO_TEN"].Text += " (*M)";
                else if (Sys_Hoc_Ky == 2 && is_mien2 == 1) item["HO_TEN"].Text += " (*M)";

                if (Sys_Hoc_Ky == 1 && (is_dk1 == null || is_dk1 == 0))
                {
                    item.ForeColor = Color.Red;
                    tbNoiDungTB.Enabled = false;
                }
                else if (Sys_Hoc_Ky == 2 && (is_dk2 == null || is_dk2 == 0))
                {
                    item.ForeColor = Color.Red;
                    tbNoiDungTB.Enabled = false;
                }

                if (Sys_Hoc_Ky == 1 && is_dk1 != null && is_dk1 == 1 && is_bo_me != null && is_bo_me == 1)
                    item["HO_TEN"].Text += " (*BM)";
                if (Sys_Hoc_Ky == 2 && is_dk2 != null && is_dk2 == 1 && is_bo_me != null && is_bo_me == 1)
                    item["HO_TEN"].Text += " (*BM)";
                #endregion
                #region "SDT"
                string sdt = item["SDT_NHAN_TIN"].Text;
                if (sdt == "&nbsp;") sdt = "";
                string sdt_k = item["SDT_NHAN_TIN2"].Text;
                if (sdt_k == "&nbsp;") sdt_k = "";
                string loai_nha_mang = localAPI.getLoaiNhaMang(sdt);
                string loai_nha_mang_k = localAPI.getLoaiNhaMang(sdt_k);
                if (is_bo_me == 1 && !string.IsNullOrEmpty(sdt_k))
                {
                    item["SDT_BM"].Text = sdt + "; " + sdt_k;
                    item["MANG"].Text = loai_nha_mang + "; " + loai_nha_mang_k;
                }
                else
                {
                    item["SDT_BM"].Text = sdt;
                    item["MANG"].Text = loai_nha_mang;
                }
                #endregion
            }
        }
        protected void btExport_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.EXPORT))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            DataTable dt = new DataTable();
            string serverPath = Server.MapPath("~");
            string path = String.Format("{0}\\ExportTemplates\\FileDynamic.xlsx", Server.MapPath("~"));
            string newName = "Nhan_tin_thong_bao.xlsx";

            List<ExcelHeaderEntity> lstHeader = new List<ExcelHeaderEntity>();
            List<ExcelEntity> lstColumn = new List<ExcelEntity>();
            lstHeader.Add(new ExcelHeaderEntity { name = "STT", colM = 1, rowM = 1, width = 10 });
            foreach (GridColumn item in RadGrid1.MasterTableView.Columns)
            {
                #region add column
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "HO_TEN") && item.UniqueName == "HO_TEN")
                {
                    DataColumn col = new DataColumn("HO_TEN");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tên học sinh", colM = 1, rowM = 1, width = 50 });
                    lstColumn.Add(new ExcelEntity { Name = "HO_TEN", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TEN_LOP") && item.UniqueName == "TEN_LOP")
                {
                    DataColumn col = new DataColumn("TEN_LOP");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tên lớp", colM = 1, rowM = 1, width = 20 });
                    lstColumn.Add(new ExcelEntity { Name = "TEN_LOP", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "SDT_BM") && item.UniqueName == "SDT_BM")
                {
                    DataColumn col = new DataColumn("SDT_BM");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "SĐT nhận tin", colM = 1, rowM = 1, width = 20 });
                    lstColumn.Add(new ExcelEntity { Name = "SDT_BM", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "NOI_DUNG_TB") && item.UniqueName == "NOI_DUNG_TB")
                {
                    DataColumn col = new DataColumn("NOI_DUNG_TB");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Nội dung", colM = 1, rowM = 1, width = 80 });
                    lstColumn.Add(new ExcelEntity { Name = "NOI_DUNG_TB", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                #endregion
            }
            RadGrid1.AllowPaging = false;
            foreach (GridDataItem item in RadGrid1.Items)
            {
                DataRow row = dt.NewRow();
                TextBox tbNoiDungTB = (TextBox)item.FindControl("tbNoiDungTB");
                foreach (DataColumn col in dt.Columns)
                {
                    row[col.ColumnName] = item[col.ColumnName].Text.Replace("&nbsp;", " ");
                    if (col.ColumnName == "NOI_DUNG_TB") row[col.ColumnName] = tbNoiDungTB.Text.Trim(); ;
                }
                dt.Rows.Add(row);
            }
            int rowHeaderStart = 6;
            int rowStart = 7;
            string colStart = "A";
            string colEnd = localAPI.GetExcelColumnName(lstColumn.Count);
            List<ExcelHeaderEntity> listCell = new List<ExcelHeaderEntity>();
            string tenSo = "";
            string tenPhong = Sys_This_Truong.TEN;
            string tieuDe = "TIN NHẮN THÔNG BÁO CHUNG";
            string hocKyNamHoc = "";
            listCell.Add(new ExcelHeaderEntity { name = tenSo, colM = 3, rowM = 1, colName = "A", rowIndex = 1, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tenPhong, colM = 3, rowM = 1, colName = "A", rowIndex = 2, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tieuDe, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 3, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            listCell.Add(new ExcelHeaderEntity { name = hocKyNamHoc, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 4, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            string nameFileOutput = newName;
            localAPI.ExportExcelDynamic(serverPath, path, newName, nameFileOutput, 1, listCell, rowHeaderStart, rowStart, colStart, colEnd, dt, lstHeader, lstColumn, true);
        }
        public string SetHeightForRadgrid()
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "countSMSDuTinhNew();SetGridHeight();", true);
            return null;
        }
        protected void btnGuiAll_Click(object sender, EventArgs e)
        {
            #region "check quyen"
            if (!is_access(SYS_Type_Access.SEND_SMS))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            if (Sys_This_Truong.IS_ACTIVE_SMS != true)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị đang không đăng ký dịch vụ!');", true);
                return;
            }
            DateTime? dt = null;
            bool is_checkHenGio = false;
            string timeSchedule = tbTime.Text;
            if (cbHenGioGuiTin.Checked)
            {
                is_checkHenGio = true;
                if (string.IsNullOrEmpty(timeSchedule))
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Thời gian không được để trống.');", true);
                    return;
                }
                else if (Convert.ToDateTime(timeSchedule) <= DateTime.Now)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Thời gian hẹn giờ không được nhỏ hơn thời gian hiện tại.');", true);
                    return;
                }
                else
                {
                    dt = Convert.ToDateTime(tbTime.Text);
                }
            }
            if (string.IsNullOrEmpty(tbNoiDung.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn phải nhập nội dung tin nhắn thông báo chung!');", true);
                tbNoiDung.Focus();
                return;
            }
            #endregion
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            List<TIN_NHAN> lstTinNhan = new List<TIN_NHAN>();
            int tong_tin_gui = 0, count_sms = 0;
            List<long> lstIDLop = new List<long>();
            int countHS = 0, countGVCN = 0, countCBNV = 0, countGuiKem = 0;

            #region get listHS
            List<HocSinhLopEntity> lstHS = new List<HocSinhLopEntity>();

            List<short> lst_ma_khoi = new List<short>();
            foreach (var item in rcbKhoi.CheckedItems)
            {
                lst_ma_khoi.Add(localAPI.ConvertStringToShort(item.Value).Value);
            }
            List<long> lst_id_lop = new List<long>();
            foreach (var item in rcbLop.CheckedItems)
            {
                lst_id_lop.Add(localAPI.ConvertStringTolong(item.Value).Value);
            }
            lstHS = hocSinhBO.getHocSinhGuiThongBaoHangNgayByListKhoiLop(Sys_This_Truong.ID, lst_ma_khoi, lst_id_lop, Convert.ToInt16(Sys_Ma_Nam_hoc), Sys_This_Cap_Hoc, Convert.ToInt16(Sys_Hoc_Ky));
            //lstHS = hocSinhBO.getHocSinhGuiTinThongBao(Sys_This_Truong.ID, lst_ma_khoi, lst_id_lop, Convert.ToInt16(Sys_Ma_Nam_hoc), Sys_This_Cap_Hoc, Convert.ToInt16(Sys_Hoc_Ky));
            #endregion

            string noi_dung = tbNoiDung.Text.Trim();
            string noi_dung_khong_dau = localAPI.chuyenTiengVietKhongDau(noi_dung);
            if (string.IsNullOrEmpty(noi_dung))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Bạn chưa nhập nội dung tin nhắn!');", true);
                return;
            }
            #region Lấy danh sách tin từ data
            string brandname = "", cp = "";
            short nam_gui = Convert.ToInt16(DateTime.Now.Year);
            short thang_gui = Convert.ToInt16(DateTime.Now.Month);
            short tuan_gui = Convert.ToInt16(localAPI.getThisWeek().ToString());
            int so_tin = localAPI.demSoTin(noi_dung_khong_dau);
            foreach (HocSinhLopEntity item in lstHS)
            {
                long id_lop = item.ID_LOP;
                bool is_gui_bo_me = false;
                if (item.IS_GUI_BO_ME != null && item.IS_GUI_BO_ME == 1) is_gui_bo_me = true;
                string sdt = item.SDT_NHAN_TIN;
                string sdt_k = item.SDT_NHAN_TIN2;
                long? id_hoc_sinh = item.ID;

                short? is_dk1 = item.IS_DK_KY1;
                short? is_dk2 = item.IS_DK_KY2;
                bool is_sms = false;
                if (is_dk1 != null && is_dk1 == 1 && Sys_Hoc_Ky == 1) is_sms = true;
                else if (is_dk2 != null && is_dk2 == 1 && Sys_Hoc_Ky == 2) is_sms = true;
                if (id_hoc_sinh != null && is_sms)
                {
                    #region Tạo tin 1
                    string loai_nha_mang = localAPI.getLoaiNhaMang(sdt);
                    TIN_NHAN tinDetail = new TIN_NHAN();
                    tinDetail.ID_TRUONG = Sys_This_Truong.ID;
                    tinDetail.ID_NGUOI_NHAN = id_hoc_sinh;
                    tinDetail.LOAI_NGUOI_NHAN = 1;
                    tinDetail.SDT_NHAN = sdt;
                    tinDetail.MA_GOI_TIN = Sys_This_Truong.MA_GOI_TIN;
                    tinDetail.THOI_GIAN_GUI = (is_checkHenGio && dt != null) ? dt : DateTime.Now;
                    tinDetail.NGUOI_GUI = Sys_User.ID;
                    tinDetail.LOAI_TIN = SYS_Loai_Tin.Tin_Thong_Bao;
                    tinDetail.KIEU_GUI = 1;
                    tinDetail.NAM_GUI = nam_gui;
                    tinDetail.THANG_GUI = thang_gui;
                    tinDetail.TUAN_GUI = tuan_gui;
                    tinDetail.NOI_DUNG = noi_dung;
                    tinDetail.NOI_DUNG_KHONG_DAU = noi_dung_khong_dau;
                    tinDetail.SO_TIN = so_tin;
                    TIN_NHAN checkExists = new TIN_NHAN();
                    if (!string.IsNullOrEmpty(sdt) && !string.IsNullOrEmpty(loai_nha_mang))
                    {
                        #region check ton tai cung ma hoc sinh, cung so dien thoai, cung noi dung tin nhan
                        //if ((cbHenGioGuiTin.Checked && !string.IsNullOrEmpty(tbTime.Text)))
                        //    checkExists = tinNhanBO.checkExistsHocSinhByMaAndSDTAndNoiDung(tinDetail.ID_NGUOI_NHAN.Value, tinDetail.LOAI_NGUOI_NHAN, sdt, SYS_Loai_Tin.Tin_Thong_Bao, dt, Sys_Time_Send, noi_dung_khong_dau);
                        //else
                        //    checkExists = tinNhanBO.checkExistsHocSinhByMaAndSDTAndNoiDung(tinDetail.ID_NGUOI_NHAN.Value, tinDetail.LOAI_NGUOI_NHAN, sdt, SYS_Loai_Tin.Tin_Thong_Bao, null, Sys_Time_Send, noi_dung_khong_dau);
                        if (is_checkHenGio && dt != null)
                            checkExists = tinNhanBO.checkExistsSms(Sys_This_Truong.ID, nam_gui, thang_gui, null, sdt, dt, Sys_Time_Send, noi_dung_khong_dau);
                        else
                            checkExists = tinNhanBO.checkExistsSms(Sys_This_Truong.ID, nam_gui, thang_gui, null, sdt, null, Sys_Time_Send, noi_dung_khong_dau);

                        if (checkExists != null) continue;
                        #endregion
                        tinDetail.LOAI_NHA_MANG = loai_nha_mang;
                        brandname = ""; cp = "";
                        getBrandnameAndCp(loai_nha_mang, out brandname, out cp);
                        tinDetail.BRAND_NAME = brandname;
                        tinDetail.CP = cp;
                        if (Sys_This_Truong.ID_DOI_TAC != null && Sys_This_Truong.ID_DOI_TAC > 0)
                            tinDetail.ID_DOI_TAC = Sys_This_Truong.ID_DOI_TAC;
                        else tinDetail.ID_DOI_TAC = null;
                        lstTinNhan.Add(tinDetail);
                        tong_tin_gui += so_tin;
                        countHS++;
                    }
                    #endregion
                    #region Tin 2
                    if (is_gui_bo_me)
                    {
                        string loai_nha_mang_k = localAPI.getLoaiNhaMang(sdt_k);
                        if (!string.IsNullOrEmpty(loai_nha_mang_k))
                        {
                            TIN_NHAN tinDetailK = new TIN_NHAN()
                            {
                                ID_DOI_TAC = tinDetail.ID_DOI_TAC,
                                ID_NGUOI_NHAN = tinDetail.ID_NGUOI_NHAN,
                                ID_NHAN_XET_HANG_NGAY = tinDetail.ID_NHAN_XET_HANG_NGAY,
                                ID_THONG_BAO = tinDetail.ID_THONG_BAO,
                                ID_TONG_HOP_NXHN = tinDetail.ID_TONG_HOP_NXHN,
                                ID_TRUONG = tinDetail.ID_TRUONG,
                                IS_DA_NHAN = tinDetail.IS_DA_NHAN,
                                IS_UNICODE = tinDetail.IS_UNICODE,
                                KIEU_GUI = tinDetail.KIEU_GUI,
                                LOAI_NGUOI_NHAN = tinDetail.LOAI_NGUOI_NHAN,
                                LOAI_NHA_MANG = tinDetail.LOAI_NHA_MANG,
                                MA_GOI_TIN = tinDetail.MA_GOI_TIN,
                                NOI_DUNG = tinDetail.NOI_DUNG,
                                NOI_DUNG_KHONG_DAU = tinDetail.NOI_DUNG_KHONG_DAU,
                                SO_TIN = tinDetail.SO_TIN,
                                THOI_GIAN_GUI = tinDetail.THOI_GIAN_GUI,
                                NGUOI_GUI = tinDetail.NGUOI_GUI,
                                LOAI_TIN = tinDetail.LOAI_TIN,
                                NAM_GUI = tinDetail.NAM_GUI,
                                SEND_NUMBER = tinDetail.SEND_NUMBER,
                                THANG_GUI = tinDetail.THANG_GUI,
                                TUAN_GUI = tinDetail.TUAN_GUI
                            };
                            tinDetailK.SDT_NHAN = sdt_k;
                            tinDetailK.LOAI_NHA_MANG = loai_nha_mang_k;
                            brandname = ""; cp = "";
                            getBrandnameAndCp(loai_nha_mang_k, out brandname, out cp);
                            tinDetail.BRAND_NAME = brandname;
                            tinDetail.CP = cp;
                            lstTinNhan.Add(tinDetailK);
                            tong_tin_gui += so_tin;
                            countHS++;
                        }
                    }
                    #endregion

                    #region "add list lop"
                    if (!lstIDLop.Contains(id_lop))
                    {
                        lstIDLop.Add(id_lop);
                    }
                    #endregion
                }
            }
            #endregion
            #region Gửi GVCN

            if (cboGuiGVCN.Checked && lstIDLop.Count > 0)
            {
                List<LopEntity> lstLopInTruong = lopBO.getLopGVCNByTruongCapNamHoc(Sys_This_Cap_Hoc, Sys_This_Truong.ID, Convert.ToInt16(Sys_Ma_Nam_hoc));
                for (int i = 0; i < lstIDLop.Count; i++)
                {
                    LopEntity lop = lstLopInTruong.FirstOrDefault(x => x.ID_LOP == lstIDLop[i]);
                    if (lop != null)
                    {
                        string sdt_gv = lop.SDT_GVCN != null ? lop.SDT_GVCN : "";
                        string loaiNhaMang_gv = localAPI.getLoaiNhaMang(sdt_gv);
                        if (!string.IsNullOrEmpty(loaiNhaMang_gv))
                        {
                            TIN_NHAN checkExists = new TIN_NHAN();
                            checkExists = lstTinNhan.FirstOrDefault(x => x.SDT_NHAN == sdt_gv);
                            if (checkExists == null)
                            {
                                TIN_NHAN tinDetail = new TIN_NHAN();
                                tinDetail.ID_TRUONG = Sys_This_Truong.ID;
                                tinDetail.ID_NGUOI_NHAN = lop.ID_GVCN;
                                tinDetail.LOAI_NGUOI_NHAN = 2;
                                tinDetail.SDT_NHAN = sdt_gv;
                                tinDetail.MA_GOI_TIN = Sys_This_Truong.MA_GOI_TIN;
                                tinDetail.THOI_GIAN_GUI = (is_checkHenGio && dt != null) ? dt : DateTime.Now;
                                tinDetail.NGUOI_GUI = Sys_User.ID;
                                tinDetail.LOAI_TIN = SYS_Loai_Tin.Tin_Thong_Bao;
                                tinDetail.KIEU_GUI = 1;
                                tinDetail.NAM_GUI = nam_gui;
                                tinDetail.THANG_GUI = thang_gui;
                                tinDetail.TUAN_GUI = tuan_gui;
                                tinDetail.NOI_DUNG = noi_dung;
                                tinDetail.NOI_DUNG_KHONG_DAU = noi_dung_khong_dau;
                                tinDetail.SO_TIN = so_tin;
                                #region check ton tai cung ma hoc sinh, cung so dien thoai, cung noi dung tin nhan
                                checkExists = new TIN_NHAN();
                                if (is_checkHenGio && dt != null)
                                    checkExists = tinNhanBO.checkExistsSms(Sys_This_Truong.ID, nam_gui, thang_gui, null, sdt_gv, dt, Sys_Time_Send, noi_dung_khong_dau);
                                else
                                    checkExists = tinNhanBO.checkExistsSms(Sys_This_Truong.ID, nam_gui, thang_gui, null, sdt_gv, null, Sys_Time_Send, noi_dung_khong_dau);
                                if (checkExists != null) continue;
                                #endregion
                                tinDetail.LOAI_NHA_MANG = loaiNhaMang_gv;
                                brandname = ""; cp = "";
                                getBrandnameAndCp(loaiNhaMang_gv, out brandname, out cp);
                                tinDetail.BRAND_NAME = brandname;
                                tinDetail.CP = cp;
                                if (Sys_This_Truong.ID_DOI_TAC != null && Sys_This_Truong.ID_DOI_TAC > 0)
                                    tinDetail.ID_DOI_TAC = Sys_This_Truong.ID_DOI_TAC;
                                else tinDetail.ID_DOI_TAC = null;
                                lstTinNhan.Add(tinDetail);
                                tong_tin_gui += so_tin;
                                countGVCN++;
                            }
                        }
                    }
                }
            }
            #endregion
            #region Gửi CBNV theo chức vụ
            List<GIAO_VIEN> lstCBNV = new List<GIAO_VIEN>();
            foreach (var item in rcbGuiCBNV.CheckedItems)
            {
                lstCBNV = giaoVienBO.getGiaoVienByChucVu(Sys_This_Truong.ID, Convert.ToInt16(item.Value));
                if (lstCBNV.Count > 0)
                {
                    for (int i = 0; i < lstCBNV.Count; i++)
                    {
                        string sdt_CBNV = lstCBNV[i].SDT;
                        if (!string.IsNullOrEmpty(sdt_CBNV))
                        {
                            string loai_nha_mang_cbnv = localAPI.getLoaiNhaMang(sdt_CBNV);
                            if (!string.IsNullOrEmpty(loai_nha_mang_cbnv))
                            {
                                TIN_NHAN checkExists = new TIN_NHAN();
                                checkExists = lstTinNhan.FirstOrDefault(x => x.SDT_NHAN == sdt_CBNV);
                                if (checkExists == null)
                                {
                                    TIN_NHAN tinDetail = new TIN_NHAN();
                                    tinDetail.ID_TRUONG = Sys_This_Truong.ID;
                                    tinDetail.ID_NGUOI_NHAN = lstCBNV[i].ID;
                                    tinDetail.LOAI_NGUOI_NHAN = 2;
                                    tinDetail.SDT_NHAN = sdt_CBNV;
                                    tinDetail.MA_GOI_TIN = Sys_This_Truong.MA_GOI_TIN;
                                    tinDetail.THOI_GIAN_GUI = (is_checkHenGio && dt != null) ? dt : DateTime.Now;
                                    tinDetail.NGUOI_GUI = Sys_User.ID;
                                    tinDetail.LOAI_TIN = SYS_Loai_Tin.Tin_Thong_Bao;
                                    tinDetail.KIEU_GUI = 1;
                                    tinDetail.NAM_GUI = nam_gui;
                                    tinDetail.THANG_GUI = thang_gui;
                                    tinDetail.TUAN_GUI = tuan_gui;
                                    tinDetail.NOI_DUNG = noi_dung;
                                    tinDetail.NOI_DUNG_KHONG_DAU = noi_dung_khong_dau;
                                    tinDetail.SO_TIN = so_tin;
                                    #region check ton tai cung ma hoc sinh, cung so dien thoai, cung noi dung tin nhan
                                    checkExists = new TIN_NHAN();
                                    if (is_checkHenGio && dt != null)
                                        checkExists = tinNhanBO.checkExistsSms(Sys_This_Truong.ID, nam_gui, thang_gui, null, sdt_CBNV, dt, Sys_Time_Send, noi_dung_khong_dau);
                                    else
                                        checkExists = tinNhanBO.checkExistsSms(Sys_This_Truong.ID, nam_gui, thang_gui, null, sdt_CBNV, null, Sys_Time_Send, noi_dung_khong_dau);
                                    if (checkExists != null) continue;
                                    #endregion
                                    tinDetail.LOAI_NHA_MANG = loai_nha_mang_cbnv;
                                    brandname = ""; cp = "";
                                    getBrandnameAndCp(loai_nha_mang_cbnv, out brandname, out cp);
                                    tinDetail.BRAND_NAME = brandname;
                                    tinDetail.CP = cp;
                                    if (Sys_This_Truong.ID_DOI_TAC != null && Sys_This_Truong.ID_DOI_TAC > 0)
                                        tinDetail.ID_DOI_TAC = Sys_This_Truong.ID_DOI_TAC;
                                    else tinDetail.ID_DOI_TAC = null;
                                    lstTinNhan.Add(tinDetail);
                                    tong_tin_gui += so_tin;
                                    countCBNV++;
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            #region Gửi kèm
            List<string> lstsdtGuiKem = new List<string>();
            string strPhoneDinhKem = tbListSDT.Text.Trim();
            if (!string.IsNullOrEmpty(strPhoneDinhKem))
            {
                lstsdtGuiKem = strPhoneDinhKem.Split(';').ToList();
                foreach (var item in lstsdtGuiKem)
                {
                    string sdtGuiKem = item.Trim();
                    string loai_nha_mang_gui_kem = localAPI.getLoaiNhaMang(sdtGuiKem);
                    if (!string.IsNullOrEmpty(loai_nha_mang_gui_kem))
                    {
                        TIN_NHAN checkExists = new TIN_NHAN();
                        checkExists = lstTinNhan.FirstOrDefault(x => x.SDT_NHAN == sdtGuiKem);
                        if (checkExists == null)
                        {
                            TIN_NHAN tinDetail = new TIN_NHAN();
                            tinDetail.ID_TRUONG = Sys_This_Truong.ID;
                            tinDetail.LOAI_NGUOI_NHAN = 2;
                            tinDetail.SDT_NHAN = sdtGuiKem;
                            tinDetail.MA_GOI_TIN = Sys_This_Truong.MA_GOI_TIN;
                            tinDetail.THOI_GIAN_GUI = (is_checkHenGio && dt != null) ? dt : DateTime.Now;
                            tinDetail.NGUOI_GUI = Sys_User.ID;
                            tinDetail.LOAI_TIN = SYS_Loai_Tin.Tin_Thong_Bao;
                            tinDetail.KIEU_GUI = 1;
                            tinDetail.NAM_GUI = nam_gui;
                            tinDetail.THANG_GUI = thang_gui;
                            tinDetail.TUAN_GUI = tuan_gui;
                            tinDetail.NOI_DUNG = noi_dung;
                            tinDetail.NOI_DUNG_KHONG_DAU = noi_dung_khong_dau;
                            tinDetail.SO_TIN = so_tin;
                            #region check ton tai cung ma hoc sinh, cung so dien thoai, cung noi dung tin nhan
                            checkExists = new TIN_NHAN();
                            if (is_checkHenGio && dt != null)
                                checkExists = tinNhanBO.checkExistsSms(Sys_This_Truong.ID, nam_gui, thang_gui, null, sdtGuiKem, dt, Sys_Time_Send, noi_dung_khong_dau);
                            else
                                checkExists = tinNhanBO.checkExistsSms(Sys_This_Truong.ID, nam_gui, thang_gui, null, sdtGuiKem, null, Sys_Time_Send, noi_dung_khong_dau);
                            if (checkExists != null) continue;
                            #endregion
                            tinDetail.LOAI_NHA_MANG = loai_nha_mang_gui_kem;
                            brandname = ""; cp = "";
                            getBrandnameAndCp(loai_nha_mang_gui_kem, out brandname, out cp);
                            tinDetail.BRAND_NAME = brandname;
                            tinDetail.CP = cp;
                            if (Sys_This_Truong.ID_DOI_TAC != null && Sys_This_Truong.ID_DOI_TAC > 0)
                                tinDetail.ID_DOI_TAC = Sys_This_Truong.ID_DOI_TAC;
                            else tinDetail.ID_DOI_TAC = null;
                            lstTinNhan.Add(tinDetail);
                            tong_tin_gui += so_tin;
                            countGuiKem++;
                        }
                    }
                }
            }
            #endregion
            #region save sms
            count_sms = lstTinNhan.Count;
            if (count_sms > 0)
            {
                short nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc);
                short thang = Convert.ToInt16(DateTime.Now.Month);
                if (thang < 8) nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc + 1);
                if (Sys_This_Truong.ID_DOI_TAC == null || Sys_This_Truong.ID_DOI_TAC == 0)
                {
                    #region Tính quỹ tin
                    QUY_TIN quyTinTheoNam = quyTinBO.getQuyTinByNamHoc(Convert.ToInt16(localAPI.GetYearNow()), Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID);
                    bool is_insert_new_quytb = false;
                    QUY_TIN quyTinTheoThang = quyTinBO.getQuyTin(nam_hoc, thang, Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID, out is_insert_new_quytb);

                    double tong_con_thang = quyTinTheoThang.TONG_CON + ((quyTinTheoThang.TONG_CAP + quyTinTheoThang.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                / 100);
                    double tong_con_nam = quyTinTheoNam.TONG_CON + ((quyTinTheoNam.TONG_CAP + quyTinTheoNam.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                        / 100);
                    #endregion
                    if (Sys_This_Truong.IS_SAN_QUY_TIN_NAM == true)
                    {
                        if (tong_tin_gui > tong_con_nam)
                        {
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị không còn đủ quota để gửi tin nhắn. Vui lòng liên hệ với quản trị viên!');", true);
                            return;
                        }
                        else
                        {
                            res = tinNhanBO.insertTinNhanThongBao(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, 0, Sys_User.ID);
                            if (res.Res)
                            {
                                logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi tin thông báo: gửi toàn danh sách " + tong_tin_gui + " tin nhắn", Sys_User.ID, DateTime.Now);
                            }
                            if (tong_tin_gui >= tong_con_thang)
                            {
                                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Đơn vị đã sử dụng vượt quá quỹ tin tháng!');", true);
                            }
                        }

                    }
                    else
                    {
                        if (tong_tin_gui > tong_con_thang || tong_tin_gui > tong_con_nam)
                        //if (tong_tin_gui > tong_con_thang)
                        {
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị đã sử dụng vượt quá quỹ tin tháng. Vui lòng liên hệ với quản trị viên để được giúp đỡ!');", true);
                            return;
                        }
                        else
                        {
                            res = tinNhanBO.insertTinNhanThongBao(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, 0, Sys_User.ID);
                            if (res.Res)
                            {
                                logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi tin thông báo: gửi toàn danh sách " + tong_tin_gui + " tin nhắn", Sys_User.ID, DateTime.Now);
                            }
                        }
                    }
                }
                else
                {
                    TRUONG detailTruong = new TRUONG();
                    long tong_tin_con = 0;
                    detailTruong = truongBO.getTruongById(Sys_This_Truong.ID);
                    if (detailTruong != null)
                    {
                        if (detailTruong.TONG_TIN_CAP != null)
                        {
                            tong_tin_con = detailTruong.TONG_TIN_DA_DUNG != null ? (detailTruong.TONG_TIN_CAP.Value - detailTruong.TONG_TIN_DA_DUNG.Value) : detailTruong.TONG_TIN_CAP.Value;
                        }
                        else tong_tin_con = detailTruong.TONG_TIN_DA_DUNG != null ? (-detailTruong.TONG_TIN_DA_DUNG.Value) : 0;
                        if (Sys_This_Truong.IS_ACTIVE_SMS != null && Sys_This_Truong.IS_ACTIVE_SMS == true && tong_tin_con > tong_tin_gui)
                        {
                            res = tinNhanBO.insertTinNhanThongBao(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, Convert.ToInt16(localAPI.getThisWeek().ToString()), Sys_User.ID);
                            if (res.Res)
                            {
                                logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi tin thông báo: gửi toàn danh sách " + tong_tin_gui + " tin nhắn", Sys_User.ID, DateTime.Now);
                            }
                        }
                    }
                }
            }
            #endregion
            #region Thông báo
            string strMsg = "";
            if (!res.Res && count_sms > 0)
            {
                strMsg = res.Msg;
            }
            else if (count_sms == 0)
            {
                strMsg = " notification('warning', 'Nội dung đã được gửi, không thể gửi lại!');";
            }
            else
            {
                strMsg = " notification('success', 'Có " + tong_tin_gui + " số điện thoại được gửi.');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            #endregion
            viewQuyTin();
            RadGrid1.Rebind();
        }
        protected void btGuiSoDinhKem_Click(object sender, EventArgs e)
        {
            #region "check quyen"
            if (!is_access(SYS_Type_Access.SEND_SMS))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            if (Sys_This_Truong.IS_ACTIVE_SMS != true)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị đang không đăng ký dịch vụ!');", true);
                return;
            }
            DateTime dt = new DateTime();
            if (cbHenGioGuiTin.Checked)
            {
                if (string.IsNullOrEmpty(tbTime.Text))
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Thời gian không được để trống.');", true);
                    return;
                }
                else if (Convert.ToDateTime(tbTime.Text) <= DateTime.Now)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Thời gian hẹn giờ không được nhỏ hơn thời gian hiện tại.');", true);
                    return;
                }
                else
                {
                    dt = Convert.ToDateTime(tbTime.Text);
                }
            }
            if (string.IsNullOrEmpty(tbListSDT.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn phải nhập SĐT nhận tin nhắn!');", true);
                tbListSDT.Focus();
                return;
            }
            if (string.IsNullOrEmpty(tbNoiDung.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn phải nhập nội dung tin nhắn thông báo chung!');", true);
                tbNoiDung.Focus();
                return;
            }
            #endregion
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            #region get List tin nhan
            List<TIN_NHAN> lstTinNhan = new List<TIN_NHAN>();
            List<string> lstsdtGuiKem = new List<string>();
            string noiDungGui = tbNoiDung.Text.Trim();
            string noiDungGui_en = localAPI.chuyenTiengVietKhongDau(noiDungGui);
            int tong_tin_gui = 0;
            int count_trung = 0;
            if (!string.IsNullOrEmpty(tbListSDT.Text.Trim()) && !string.IsNullOrEmpty(noiDungGui))
            {
                lstsdtGuiKem = tbListSDT.Text.Trim().Split(';').ToList();
                foreach (var item in lstsdtGuiKem)
                {
                    string sdtGuiKem = item.Trim();
                    string loai_nha_mang_gui_kem = localAPI.getLoaiNhaMang(sdtGuiKem);
                    if (!string.IsNullOrEmpty(localAPI.getLoaiNhaMang(sdtGuiKem)))
                    {
                        TIN_NHAN checkExists = new TIN_NHAN();
                        checkExists = lstTinNhan.FirstOrDefault(x => x.SDT_NHAN == sdtGuiKem);
                        if (checkExists == null)
                        {
                            TIN_NHAN tinDetail = new TIN_NHAN();
                            tinDetail.ID_TRUONG = Sys_This_Truong.ID;
                            tinDetail.LOAI_NGUOI_NHAN = 2;
                            tinDetail.SDT_NHAN = sdtGuiKem;
                            tinDetail.MA_GOI_TIN = Sys_This_Truong.MA_GOI_TIN;
                            tinDetail.THOI_GIAN_GUI = (cbHenGioGuiTin.Checked && !string.IsNullOrEmpty(tbTime.Text)) ? dt : DateTime.Now;
                            tinDetail.NGUOI_GUI = Sys_User.ID;
                            tinDetail.LOAI_TIN = SYS_Loai_Tin.Tin_Thong_Bao;
                            tinDetail.KIEU_GUI = 1;
                            tinDetail.NAM_GUI = Convert.ToInt16(DateTime.Now.Year);
                            tinDetail.THANG_GUI = Convert.ToInt16(DateTime.Now.Month);
                            tinDetail.TUAN_GUI = Convert.ToInt16(localAPI.getThisWeek().ToString());
                            tinDetail.NOI_DUNG = noiDungGui;
                            tinDetail.NOI_DUNG_KHONG_DAU = noiDungGui_en;
                            tinDetail.SO_TIN = localAPI.demSoTin(noiDungGui);
                            #region check ton tai cung ma hoc sinh, cung so dien thoai, cung noi dung tin nhan
                            checkExists = new TIN_NHAN();
                            if ((cbHenGioGuiTin.Checked && !string.IsNullOrEmpty(tbTime.Text)))
                                checkExists = tinNhanBO.checkExistsHocSinhByMaAndSDTAndNoiDung(tinDetail.ID_NGUOI_NHAN, tinDetail.LOAI_NGUOI_NHAN, sdtGuiKem, SYS_Loai_Tin.Tin_Thong_Bao, dt, Sys_Time_Send, noiDungGui_en);
                            else
                                checkExists = tinNhanBO.checkExistsHocSinhByMaAndSDTAndNoiDung(tinDetail.ID_NGUOI_NHAN, tinDetail.LOAI_NGUOI_NHAN, sdtGuiKem, SYS_Loai_Tin.Tin_Thong_Bao, null, Sys_Time_Send, noiDungGui_en);
                            if (checkExists != null)
                            {
                                count_trung++;
                                continue;
                            }

                            #endregion
                            tinDetail.LOAI_NHA_MANG = loai_nha_mang_gui_kem;
                            string brandname = "", cp = "";
                            getBrandnameAndCp(loai_nha_mang_gui_kem, out brandname, out cp);
                            tinDetail.BRAND_NAME = brandname;
                            tinDetail.CP = cp;
                            if (Sys_This_Truong.ID_DOI_TAC != null && Sys_This_Truong.ID_DOI_TAC > 0)
                                tinDetail.ID_DOI_TAC = Sys_This_Truong.ID_DOI_TAC;
                            else tinDetail.ID_DOI_TAC = null;
                            lstTinNhan.Add(tinDetail);
                            tong_tin_gui += localAPI.demSoTin(noiDungGui);
                        }
                    }
                }
            }
            #endregion
            #region check quỹ và gửi tin nhắn
            if (lstTinNhan.Count > 0)
            {
                short nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc);
                short thang = Convert.ToInt16(DateTime.Now.Month);
                if (thang < 8) nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc + 1);
                if (Sys_This_Truong.ID_DOI_TAC == null || Sys_This_Truong.ID_DOI_TAC == 0)
                {
                    #region Tính quỹ tin
                    QUY_TIN quyTinTheoNam = quyTinBO.getQuyTinByNamHoc(Convert.ToInt16(localAPI.GetYearNow()), Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID);
                    bool is_insert_new_quytb = false;
                    QUY_TIN quyTinTheoThang = quyTinBO.getQuyTin(nam_hoc, thang, Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID, out is_insert_new_quytb);

                    double tong_con_thang = quyTinTheoThang.TONG_CON + ((quyTinTheoThang.TONG_CAP + quyTinTheoThang.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                / 100);
                    double tong_con_nam = quyTinTheoNam.TONG_CON + ((quyTinTheoNam.TONG_CAP + quyTinTheoNam.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                        / 100);
                    #endregion
                    if (Sys_This_Truong.IS_SAN_QUY_TIN_NAM == true)
                    {
                        if (tong_tin_gui > tong_con_nam)
                        {
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị không còn đủ quota để gửi tin nhắn. Vui lòng liên hệ với quản trị viên!');", true);
                            return;
                        }
                        else
                        {
                            res = tinNhanBO.insertTinNhanThongBao(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, 0, Sys_User.ID);
                            if (res.Res)
                            {
                                logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi tin thông báo: SĐT gửi kèm", Sys_User.ID, DateTime.Now);
                            }
                            if (tong_tin_gui >= tong_con_thang)
                            {
                                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Đơn vị đã sử dụng vượt quá quỹ tin tháng!');", true);
                            }
                        }

                    }
                    else
                    {
                        if (tong_tin_gui > tong_con_thang || tong_tin_gui > tong_con_nam)
                        //if (tong_tin_gui > tong_con_thang)
                        {
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị đã sử dụng vượt quá quỹ tin tháng. Vui lòng liên hệ với quản trị viên để được giúp đỡ!');", true);
                            return;
                        }
                        else
                        {
                            res = tinNhanBO.insertTinNhanThongBao(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, 0, Sys_User.ID);
                            if (res.Res)
                            {
                                logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi tin thông báo: SĐT gửi kèm", Sys_User.ID, DateTime.Now);
                            }
                        }
                    }
                }
                else
                {
                    TRUONG detailTruong = new TRUONG();
                    long tong_tin_con = 0;
                    detailTruong = truongBO.getTruongById(Sys_This_Truong.ID);
                    if (detailTruong != null)
                    {
                        if (detailTruong.TONG_TIN_CAP != null)
                        {
                            tong_tin_con = detailTruong.TONG_TIN_DA_DUNG != null ? (detailTruong.TONG_TIN_CAP.Value - detailTruong.TONG_TIN_DA_DUNG.Value) : detailTruong.TONG_TIN_CAP.Value;
                        }
                        else tong_tin_con = detailTruong.TONG_TIN_DA_DUNG != null ? (-detailTruong.TONG_TIN_DA_DUNG.Value) : 0;
                        if (Sys_This_Truong.IS_ACTIVE_SMS != null && Sys_This_Truong.IS_ACTIVE_SMS == true && tong_tin_con > tong_tin_gui)
                        {
                            res = tinNhanBO.insertTinNhanThongBao(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, 0, Sys_User.ID);
                            if (res.Res)
                            {
                                logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi tin thông báo: SĐT gửi kèm", Sys_User.ID, DateTime.Now);
                            }
                        }
                    }
                }
            }
            #endregion
            #region Thông báo
            string strMsg = "";
            if (!res.Res && lstTinNhan.Count > 0)
            {
                strMsg = res.Msg;
            }
            else if (count_trung > 0 && lstTinNhan.Count == 0)
            {
                strMsg = " notification('error', 'Nội dung đã được gửi, không thể gửi lại.');";
            }
            else
            {
                strMsg = " notification('success', 'Có " + tong_tin_gui + " số điện thoại được gửi.');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            #endregion
            viewQuyTin();
        }
        protected void btTimKiem_Click(object sender, EventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void getBrandnameAndCp(string loai_nha_mang, out string brandname, out string cp)
        {
            if (loai_nha_mang == "Viettel")
            {
                brandname = Sys_This_Truong.BRAND_NAME_VIETTEL;
                cp = Sys_This_Truong.CP_VIETTEL;
            }
            else if (loai_nha_mang == "GMobile")
            {
                brandname = Sys_This_Truong.BRAND_NAME_GTEL;
                cp = Sys_This_Truong.CP_GTEL;
            }
            else if (loai_nha_mang == "MobiFone")
            {
                brandname = Sys_This_Truong.BRAND_NAME_MOBI;
                cp = Sys_This_Truong.CP_MOBI;
            }
            else if (loai_nha_mang == "VinaPhone")
            {
                brandname = Sys_This_Truong.BRAND_NAME_VINA;
                cp = Sys_This_Truong.CP_VINA;
            }
            else if (loai_nha_mang == "VietnamMobile")
            {
                brandname = Sys_This_Truong.BRAND_NAME_VNM;
                cp = Sys_This_Truong.CP_VNM;
            }
            else
            {
                brandname = "";
                cp = "";
            }
        }
    }
}