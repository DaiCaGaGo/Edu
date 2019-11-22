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

namespace CMS.Truong
{
    public partial class GuiTinNhanThuongHieu : AuthenticatePage
    {
        private LocalAPI localAPI = new LocalAPI();
        TruongBO truongBO = new TruongBO();
        private QuyTinBO quyTinBO = new QuyTinBO();
        private TinNhanBO tinNhanBO = new TinNhanBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rcbTruong.DataSource = Sys_List_Truong;
                rcbTruong.DataBind();
            }
        }
        protected void btSendSms_Click(object sender, EventArgs e)
        {
            List<long> lst_ma_truong = new List<long>();
            foreach (var item in rcbTruong.CheckedItems)
            {
                if (item != null)
                    lst_ma_truong.Add(Convert.ToInt64(item.Value));
            }

            if (lst_ma_truong.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn phải chọn trường để gửi tin!');", true);
                return;
            }

            string noiDungGui = tbNoiDung.Text.Trim();
            string noiDungGui_en = localAPI.chuyenTiengVietKhongDau(noiDungGui);
            if (string.IsNullOrEmpty(tbNoiDung.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn phải nhập nội dung tin nhắn!');", true);
                tbNoiDung.Focus();
                return;
            }

            List<string> listSDT = new List<string>();
            if (!string.IsNullOrEmpty(tbListSDT.Text.Trim())) listSDT = tbListSDT.Text.Trim().Split(';').ToList();
            if (listSDT.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn phải nhập SĐT nhận tin!');", true);
                tbListSDT.Focus();
                return;
            }

            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            int tong = 0;
            foreach (var truongID in lst_ma_truong)
            {
                int tong_tin_gui = 0;
                List<TIN_NHAN> lstTinNhan = new List<TIN_NHAN>();
                long id_truong = truongID;
                TRUONG truong = truongBO.getTruongById(id_truong);
                if (truong != null)
                {
                    if (truong.IS_ACTIVE_SMS != true)
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Trường học chưa được active dịch vụ gửi tin nhắn!');", true);
                        continue;
                    }

                    #region get List tin nhan
                    foreach (var item in listSDT)
                    {
                        string sdt = item.Trim();
                        sdt = localAPI.Add84(sdt);
                        string loai_nha_mang = localAPI.getLoaiNhaMang(sdt);
                        if (!string.IsNullOrEmpty(localAPI.getLoaiNhaMang(sdt)))
                        {
                            TIN_NHAN checkExists = new TIN_NHAN();
                            checkExists = lstTinNhan.FirstOrDefault(x => x.SDT_NHAN == sdt);
                            if (checkExists == null)
                            {
                                TIN_NHAN tinDetail = new TIN_NHAN();
                                tinDetail.ID_TRUONG = id_truong;
                                tinDetail.LOAI_NGUOI_NHAN = 2;
                                tinDetail.SDT_NHAN = sdt;
                                tinDetail.MA_GOI_TIN = truong.MA_GOI_TIN;
                                tinDetail.THOI_GIAN_GUI = DateTime.Now;
                                tinDetail.NGUOI_GUI = Sys_User.ID;
                                tinDetail.LOAI_TIN = 2;
                                tinDetail.KIEU_GUI = 2;
                                tinDetail.NAM_GUI = Convert.ToInt16(DateTime.Now.Year);
                                tinDetail.THANG_GUI = Convert.ToInt16(DateTime.Now.Month);
                                tinDetail.TUAN_GUI = Convert.ToInt16(localAPI.getThisWeek().ToString());
                                tinDetail.NOI_DUNG = noiDungGui;
                                tinDetail.NOI_DUNG_KHONG_DAU = noiDungGui_en;
                                tinDetail.SO_TIN = localAPI.demSoTin(noiDungGui);

                                tinDetail.LOAI_NHA_MANG = loai_nha_mang;
                                if (loai_nha_mang == "Viettel")
                                {
                                    tinDetail.BRAND_NAME = truong.BRAND_NAME_VIETTEL;
                                    tinDetail.CP = truong.CP_VIETTEL;
                                }
                                else if (loai_nha_mang == "GMobile")
                                {
                                    tinDetail.BRAND_NAME = truong.BRAND_NAME_GTEL;
                                    tinDetail.CP = truong.CP_GTEL;
                                }
                                else if (loai_nha_mang == "MobiFone")
                                {
                                    tinDetail.BRAND_NAME = truong.BRAND_NAME_MOBI;
                                    tinDetail.CP = truong.CP_MOBI;
                                }
                                else if (loai_nha_mang == "VinaPhone")
                                {
                                    tinDetail.BRAND_NAME = truong.BRAND_NAME_VINA;
                                    tinDetail.CP = truong.CP_VINA;
                                }
                                else if (loai_nha_mang == "VietnamMobile")
                                {
                                    tinDetail.BRAND_NAME = truong.BRAND_NAME_VNM;
                                    tinDetail.CP = truong.CP_VNM;
                                }
                                if (truong.ID_DOI_TAC != null && truong.ID_DOI_TAC > 0)
                                    tinDetail.ID_DOI_TAC = truong.ID_DOI_TAC;
                                else tinDetail.ID_DOI_TAC = null;
                                lstTinNhan.Add(tinDetail);
                                tong_tin_gui += localAPI.demSoTin(noiDungGui);
                                tong += tong_tin_gui;
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
                        if (truong.ID_DOI_TAC == null || truong.ID_DOI_TAC == 0)
                        {
                            #region Tính quỹ tin
                            QUY_TIN quyTinTheoNam = quyTinBO.getQuyTinByNamHoc(Convert.ToInt16(localAPI.GetYearNow()), truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID);
                            bool is_insert_new_quytb = false;
                            QUY_TIN quyTinTheoThang = quyTinBO.getQuyTin(nam_hoc, thang, truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID, out is_insert_new_quytb);

                            if (quyTinTheoNam != null && quyTinTheoThang != null)
                            {
                                double tong_con_thang = quyTinTheoThang.TONG_CON + ((quyTinTheoThang.TONG_CAP + quyTinTheoThang.TONG_THEM) * 1.0 * (truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                        / 100);
                                double tong_con_nam = quyTinTheoNam.TONG_CON + ((quyTinTheoNam.TONG_CAP + quyTinTheoNam.TONG_THEM) * 1.0 * (truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                                    / 100);
                                #endregion
                                if (truong.IS_SAN_QUY_TIN_NAM == true)
                                {
                                    if (tong_tin_gui > tong_con_nam)
                                    {
                                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị không còn đủ quota để gửi tin nhắn. Vui lòng liên hệ với quản trị viên!');", true);
                                        return;
                                    }
                                    else
                                    {
                                        res = tinNhanBO.insertTinNhanThongBao(lstTinNhan, truong.ID, nam_hoc, thang, Convert.ToInt16(localAPI.getThisWeek().ToString()), Sys_User.ID);
                                        if (tong_tin_gui >= tong_con_thang)
                                        {
                                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Đơn vị đã sử dụng vượt quá quỹ tin tháng!');", true);
                                        }
                                    }

                                }
                                else
                                {
                                    if (tong_tin_gui > tong_con_thang || tong_tin_gui > tong_con_nam)
                                    {
                                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị đã sử dụng vượt quá quỹ tin tháng. Vui lòng liên hệ với quản trị viên để được giúp đỡ!');", true);
                                        return;
                                    }
                                    else
                                    {
                                        res = tinNhanBO.insertTinNhanThongBao(lstTinNhan, truong.ID, nam_hoc, thang, Convert.ToInt16(localAPI.getThisWeek().ToString()), Sys_User.ID);
                                    }
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị chưa được cấp tin nhắn!');", true);
                                return;
                            }
                        }
                        else
                        {
                            TRUONG detailTruong = new TRUONG();
                            long tong_tin_con = 0;
                            detailTruong = truongBO.getTruongById(truong.ID);
                            if (detailTruong != null)
                            {
                                if (detailTruong.TONG_TIN_CAP != null)
                                {
                                    tong_tin_con = detailTruong.TONG_TIN_DA_DUNG != null ? (detailTruong.TONG_TIN_CAP.Value - detailTruong.TONG_TIN_DA_DUNG.Value) : detailTruong.TONG_TIN_CAP.Value;
                                }
                                else tong_tin_con = detailTruong.TONG_TIN_DA_DUNG != null ? (-detailTruong.TONG_TIN_DA_DUNG.Value) : 0;
                                if (truong.IS_ACTIVE_SMS != null && truong.IS_ACTIVE_SMS == true && tong_tin_con > tong_tin_gui)
                                {
                                    res = tinNhanBO.insertTinNhanThongBao(lstTinNhan, truong.ID, nam_hoc, thang, Convert.ToInt16(localAPI.getThisWeek().ToString()), Sys_User.ID);
                                }
                            }
                        }
                    }
                    #endregion

                }
            }

            #region Thông báo
            string strMsg = "";
            if (!res.Res && tong > 0)
            {
                strMsg = res.Msg;
            }
            else
            {
                strMsg = " notification('success', 'Có " + tong + " số tin nhắn được gửi.');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            #endregion
        }
    }
}