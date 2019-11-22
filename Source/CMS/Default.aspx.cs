using CMS.BieuDo.Entities;
using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using OneEduDataAccess.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS
{
    public partial class Default : AuthenticatePage
    {
        HocSinhBO hsBO = new HocSinhBO();
        LopBO lopBO = new LopBO();
        GiaoVienBO gvBO = new GiaoVienBO();
        QuyTinBO quyTinBO = new QuyTinBO();
        LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            loadBieuDoTinThongBao();
            loadBieuDoHStheoKhoiHoc();
            loadBieuDoHStheonamhoc();
            if (!IsPostBack)
            {
                short nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc);
                short thang = Convert.ToInt16(DateTime.Now.Month);
                if (thang < 8) nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc + 1);
                #region "số học sinh"
                long? tong_so_hs = hsBO.getTongSoHocSinhByTruongCapHocNamHoc(Sys_This_Truong.ID, Sys_This_Cap_Hoc, Convert.ToInt16(Sys_Ma_Nam_hoc), Convert.ToInt16(Sys_Hoc_Ky));
                long? tong_so_hs_dk = hsBO.getTongSoHocSinhDangKySMSByTruongCapHocNamHoc(Sys_This_Truong.ID, Sys_This_Cap_Hoc, Convert.ToInt16(Sys_Ma_Nam_hoc), Convert.ToInt16(Sys_Hoc_Ky));
                if (tong_so_hs != null && tong_so_hs > 0)
                {
                    if (tong_so_hs_dk != null)
                    {
                        lbSoHocSinh.Text = tong_so_hs_dk + "/" + tong_so_hs;
                        double so_phan_tram = (tong_so_hs_dk.Value * 0.1 / tong_so_hs.Value) * 1000;
                        so_phan_tram = Math.Round(so_phan_tram, 2);
                        lbDiscriptionSoHS.Text = so_phan_tram + "% HS đăng ký SMS trên tổng số HS toàn trường";
                        proSoHS.Style.Value = "Width:" + so_phan_tram + "%";
                    }
                }
                #endregion
                #region "số lớp"
                long? so_lop = lopBO.getTongSoLopTheoTruongMaCapNamHoc(Sys_This_Truong.ID, Sys_This_Cap_Hoc, Convert.ToInt16(Sys_Ma_Nam_hoc));
                if (so_lop != null)
                    lbSoLop.Text = so_lop.Value.ToString();
                #endregion
                #region "số giáo viên"
                long? so_gv = gvBO.getTongSoGiaoVienByTruong(Sys_This_Truong.ID);
                if (so_gv != null)
                    lbSoGiaoVien.Text = so_gv.Value.ToString();
                #endregion
                #region "view quỹ tin liên lạc theo tuần"
                QUY_TIN quyTin = new QUY_TIN();
                bool is_insert_new_quytb = false;
                quyTin = quyTinBO.getQuyTin(nam_hoc, thang, Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Lien_Lac, Sys_User.ID, out is_insert_new_quytb);
                QUY_TIN quyTinNam = quyTinBO.getQuyTinByNamHoc(Convert.ToInt16(localAPI.GetYearNow()), Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Lien_Lac, Sys_User.ID);
                long tong_tin_ll = 0;
                long tong_da_su_dung = 0;
                if (quyTin != null && quyTinNam != null)
                {
                    tong_tin_ll = quyTin.TONG_CAP + quyTin.TONG_THEM;
                    tong_da_su_dung = quyTin.TONG_DA_SU_DUNG;
                    ltrLL.Text = "Quỹ tháng: <b><i>" + tong_da_su_dung + "/" + tong_tin_ll + "</i></b>. Quỹ năm: <b><i>" + quyTinNam.TONG_DA_SU_DUNG + "/" + (quyTinNam.TONG_CAP + quyTinNam.TONG_THEM) + "</i></b>";
                    double phan_tram_ll = 0;
                    if (tong_tin_ll > 0) phan_tram_ll = tong_da_su_dung * 1000 * 0.1 / tong_tin_ll;
                    phan_tram_ll = Math.Round(phan_tram_ll, 2);
                    lbDisQuyTinLL.Text = phan_tram_ll + "% tin đã sử dụng trên tổng tin cấp theo tháng";
                    proQuyTinLL.Style.Value = "Width:" + phan_tram_ll + "%";
                }
                #endregion
                #region "view quỹ tin thông báo theo tháng"
                quyTin = quyTinBO.getQuyTin(nam_hoc, thang, Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID, out is_insert_new_quytb);
                quyTinNam = quyTinBO.getQuyTinByNamHoc(Convert.ToInt16(localAPI.GetYearNow()), Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID);
                if (quyTin != null)
                {
                    double phan_tram_tb = 0;
                    if (quyTin.TONG_CAP > 0) phan_tram_tb = quyTin.TONG_DA_SU_DUNG * 0.1 * 1000 / (quyTin.TONG_CAP + quyTin.TONG_THEM);
                    phan_tram_tb = Math.Round(phan_tram_tb, 2);
                    ltrTB.Text = "Quỹ tháng: <b><i>" + quyTin.TONG_DA_SU_DUNG + "/" + (quyTin.TONG_CAP + quyTin.TONG_THEM) + "</i></b>. Quỹ năm: <b><i>" + quyTinNam.TONG_DA_SU_DUNG + "/" + (quyTinNam.TONG_CAP + quyTinNam.TONG_THEM) + "</i></b>";
                    lbDisQuyTinTB.Text = phan_tram_tb + "% tin đã sử dụng trên tổng tin cấp theo tháng";
                    proQuyTinTB.Style.Value = "Width:" + phan_tram_tb + "%";
                }
                #endregion
            }
        }
        public void loadBieuDoTinThongBao()
        {
            List<BIEU_DO_Entity> lstItemBieuDo = new List<BIEU_DO_Entity>();
            try
            {
                #region "biểu đồ quỹ tin thông báo theo tháng"
                //QUY_TIN quyTin = new QUY_TIN();
                //bool is_insert_new_quytb = false;
                //quyTin = quyTinBO.getQuyTin(Convert.ToInt16(localAPI.GetYearNow()), Convert.ToInt16(DateTime.Now.Month), Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID, out is_insert_new_quytb);
                //QUY_TIN quyTinNam = quyTinBO.getQuyTinByNamHoc(Convert.ToInt16(localAPI.GetYearNow()), Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID);
                //if (quyTinNam != null)
                //{
                //    lstItemBieuDo.Add(new BIEU_DO_Entity("Tin đã sử dụng", quyTinNam.TONG_DA_SU_DUNG, true, ""));
                //    lstItemBieuDo.Add(new BIEU_DO_Entity("Tổng tin/Năm", (quyTinNam.TONG_CAP + quyTin.TONG_THEM) - quyTinNam.TONG_DA_SU_DUNG, true, ""));
                //double phan_tram_tb = 0;
                //if (quyTin.TONG_CAP > 0) phan_tram_tb = quyTin.TONG_DA_SU_DUNG * 0.1 * 1000 / (quyTin.TONG_CAP + quyTin.TONG_THEM);
                //phan_tram_tb = Math.Round(phan_tram_tb, 2);
                //lstItemBieuDo.Add(new BIEU_DO_Entity("Tin đã sử dụng", phan_tram_tb, true,""));
                //lstItemBieuDo.Add(new BIEU_DO_Entity("Tin chưa sử dụng", 100 - phan_tram_tb, true, ""));
                // }
                #endregion
                TinNhanBO tinnhanBO = new TinNhanBO();
                List<KhoiHocSinhEntity> lstTinNhan = tinnhanBO.thongkeSoLuongTinNhanGuiTrongCacThang(Sys_This_Truong.ID, Sys_Ma_Nam_hoc);
                foreach (var item in lstTinNhan)
                {
                    lstItemBieuDo.Add(new BIEU_DO_Entity("", item.SOLUONG_HS, true, item.ID_NAM_HOC.ToString()));
                }
                if (lstItemBieuDo.Count <= 1)
                {
                    int CurrenMonth = (DateTime.Now.Month) + 1;
                    if(CurrenMonth <=12)
                    lstItemBieuDo.Add(new BIEU_DO_Entity("", 0, true, CurrenMonth.ToString()));
                }
            }
            catch { }
            RadHtmlChart1.DataSource = lstItemBieuDo;
            RadHtmlChart1.DataBind();
        }
        public void loadBieuDoHStheoKhoiHoc()
        {
            List<BIEU_DO_Entity> lstItemBieuDo = new List<BIEU_DO_Entity>();
            try
            {
                KhoiBO khoiBO = new KhoiBO();
                List<KhoiHocSinhEntity> lstKhoi = khoiBO.thongkeSoLuongHocSinhByKhoiLop(Sys_This_Truong.ID, Sys_This_Cap_Hoc, Convert.ToInt16(Sys_Ma_Nam_hoc));
                foreach (var item in lstKhoi)
                {
                    lstItemBieuDo.Add(new BIEU_DO_Entity("", item.SOLUONG_HS, true, item.TEN));
                }
            }
            catch { }
            RadHtmlChart2.DataSource = lstItemBieuDo;
            RadHtmlChart2.DataBind();
            // RadHtmlChart2.ChartTitle.Text = "Số Học Sinh trong khối học";
        }
        public void loadBieuDoHStheonamhoc()
        {
            List<BIEU_DO_Entity> lstItemBieuDo3 = new List<BIEU_DO_Entity>();
            try
            {
                KhoiBO khoiBO = new KhoiBO();
                List<KhoiHocSinhEntity> lstKhoi = khoiBO.thongkeSoLuongHocSinhByNamHoc(Sys_This_Truong.ID, Sys_Ma_Nam_hoc);
                foreach (var item in lstKhoi)
                {
                    lstItemBieuDo3.Add(new BIEU_DO_Entity("", item.SOLUONG_HS, true, item.ID_NAM_HOC.ToString()));
                }
            }
            catch { }
            RadHtmlChart3.DataSource = lstItemBieuDo3;
            RadHtmlChart3.DataBind();
            //  RadHtmlChart3.ChartTitle.Text = "Biểu đồ thống kê số HS theo từng năm";
        }

    }
}