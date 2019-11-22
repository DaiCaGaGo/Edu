using HiQPdf;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS.InSoHocSinh
{
    public partial class InSoTheoHocSinh : AuthenticatePage
    {
        long? id_hs;
        HocSinhBO hsBO = new HocSinhBO();
        DmDanTocBO danTocBO = new DmDanTocBO();
        DmQuocTichBO quocTichBO = new DmQuocTichBO();
        DmXaPhuongBO xaPhuongBO = new DmXaPhuongBO();
        DmQuanHuyenBO quanHuyenBO = new DmQuanHuyenBO();
        DmTinhThanhBO tinhThanhBO = new DmTinhThanhBO();
        NamHocBO namHocBO = new NamHocBO();
        TruongBO truongBO = new TruongBO();
        LopBO lopBO = new LopBO();
        ChuyenCanBO chuyenCanBO = new ChuyenCanBO();
        DanhGiaDinhKyMonTHBO danhGiaDinhKyTHBO = new DanhGiaDinhKyMonTHBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("id_hs") != null)
            {
                try
                {
                    id_hs = Convert.ToInt16(Request.QueryString.Get("id_hs"));
                }
                catch { }
                if (!IsPostBack)
                {
                    if (id_hs != null)
                    {
                        HOC_SINH detail = new HOC_SINH();
                        detail = hsBO.getHocSinhByID(id_hs.Value);
                        if (detail != null)
                        {
                            #region "học bạ"
                            lblHoTen.Text = detail.HO_TEN != null ? detail.HO_TEN : "";
                            lblGioiTinh.Text = detail.MA_GIOI_TINH == 1 ? "Nam" : detail.MA_GIOI_TINH == 2 ? "Nữ" : "";
                            lblNgaySinh.Text = detail.NGAY_SINH != null ? Convert.ToDateTime(detail.NGAY_SINH).ToString("dd/MM/yyyy") : "";
                            DM_DAN_TOC danToc = new DM_DAN_TOC();
                            if (detail.MA_DAN_TOC != null)
                                danToc = danTocBO.getDanToc().FirstOrDefault(x => x.MA == detail.MA_DAN_TOC);
                            lblDanToc.Text = danToc != null ? danToc.TEN : "";
                            DM_QUOC_TICH quocTich = new DM_QUOC_TICH();
                            if (detail.MA_QUOC_TICH != null)
                                quocTich = quocTichBO.getQuocTich().FirstOrDefault(x => x.MA == detail.MA_QUOC_TICH);
                            lblQuocTich.Text = quocTich != null ? quocTich.TEN : "";
                            lblNoiSinh.Text = detail.NOI_SINH != null ? detail.NOI_SINH : "";
                            DM_TINH_THANH tinhThanh = new DM_TINH_THANH();
                            if (detail.MA_TINH_THANH != null)
                            {
                                string str_tinh = "", str_huyen = "", str_xa = "";
                                tinhThanh = tinhThanhBO.getTinhThanh().FirstOrDefault(x => x.MA == detail.MA_TINH_THANH);
                                str_tinh = tinhThanh != null ? tinhThanh.TEN : "";
                                lblQueQuan.Text = str_tinh;
                                DM_QUAN_HUYEN quanHuyen = new DM_QUAN_HUYEN();
                                if (detail.MA_QUAN_HUYEN != null)
                                {
                                    quanHuyen = quanHuyenBO.getQuanHuyen().FirstOrDefault(x => x.MA_TINH == detail.MA_TINH_THANH && x.MA == detail.MA_QUAN_HUYEN);
                                    str_huyen = quanHuyen != null ? quanHuyen.TEN : "";
                                    DM_XA_PHUONG xaPhuong = new DM_XA_PHUONG();
                                    if (detail.MA_XA_PHUONG != null)
                                        xaPhuong = xaPhuongBO.getXaPhuong().FirstOrDefault(x => x.MA == detail.MA_XA_PHUONG && x.MA_TINH == detail.MA_TINH_THANH && x.MA_HUYEN == detail.MA_QUAN_HUYEN);
                                    str_xa = xaPhuong != null ? xaPhuong.TEN : "";
                                    lblNoiOHienTai.Text = !string.IsNullOrEmpty(str_xa) ? (str_xa + ", " + str_huyen + ", " + str_tinh) : (!string.IsNullOrEmpty(str_huyen) ? (str_huyen + ", " + str_tinh) : (!string.IsNullOrEmpty(str_tinh) ? str_tinh : ""));
                                }
                            }
                            lblHoTenCha.Text = detail.HO_TEN_BO != null ? detail.HO_TEN_BO : "";
                            lblHoTenMe.Text = detail.HO_TEN_ME != null ? detail.HO_TEN_ME : "";
                            lblNguoiGiamHo.Text = detail.HO_TEN_NGUOI_BAO_HO != null ? detail.HO_TEN_NGUOI_BAO_HO : "";
                            #endregion
                            #region Quá trình học tập
                            NAM_HOC namHoc = namHocBO.getNamHoc().FirstOrDefault(x => x.MA == Sys_Ma_Nam_hoc);
                            lblNamHoc1.Text = namHoc.TEN;
                            lblLop1.Text = lopBO.getAllLop().FirstOrDefault(x => x.ID == detail.ID_LOP && x.ID_TRUONG == detail.ID_TRUONG && x.ID_NAM_HOC == detail.ID_NAM_HOC).TEN;
                            lblTruong1.Text = truongBO.getTruongById(detail.ID_TRUONG).TEN;
                            #endregion
                            #region Số ngày nghỉ phép
                            int so_buoi_nghi = 0, so_buoi_p = 0, so_buoi_kp = 0;
                            ChuyenCanEntity chuyenCan = new ChuyenCanEntity();
                            chuyenCan = chuyenCanBO.getTongSoBuoiNghiTrongNam(Convert.ToInt16(Sys_Ma_Nam_hoc), Sys_This_Truong.ID, detail.ID_LOP, detail.ID);
                            if (chuyenCan != null)
                            {
                                so_buoi_p = chuyenCan.NGHI_P != null ? chuyenCan.NGHI_P.Value : 0;
                                so_buoi_kp = chuyenCan.NGHI_KP != null ? chuyenCan.NGHI_KP.Value : 0;
                                so_buoi_nghi = so_buoi_p + so_buoi_kp;
                            }
                            lblSoNgayNghi.Text = so_buoi_nghi.ToString();
                            lblNgayCoPhep.Text = so_buoi_nghi > 0 ? so_buoi_p.ToString() : "";
                            lblNgayKhongPhep.Text = so_buoi_nghi > 0 ? so_buoi_kp.ToString() : "";
                            #endregion
                            #region chi tiết điểm
                            lblHoTen1.Text = detail.HO_TEN;
                            lblTenLop1.Text = lopBO.getAllLop().FirstOrDefault(x => x.ID == detail.ID_LOP && x.ID_TRUONG == detail.ID_TRUONG && x.ID_NAM_HOC == detail.ID_NAM_HOC).TEN;
                            lblTenTruong.Text = Sys_This_Truong.TEN;
                            lblNamHoc.Text = namHocBO.getNamHoc().FirstOrDefault(x => x.MA == Sys_Ma_Nam_hoc).TEN;
                            List<DanhGiaDinhKyMonTHEntity> lstDanhGiaMonHoc = danhGiaDinhKyTHBO.getDiemTongKetNamTheoHocSinh(Convert.ToInt16(Sys_Ma_Nam_hoc), Sys_This_Truong.ID, detail.ID_LOP, id_hs.Value);
                            #endregion
                        }
                    }
                }
            }
        }
        protected void btnConvertPdf_Click(object sender, EventArgs e)
        {
            // create the HTML to PDF converter
            HtmlToPdf htmlToPdfConverter = new HtmlToPdf();

            // set the PDF standard used by the document
            //htmlToPdfConverter.Document.PdfStandard = PdfStandard.Pdf;

            // set PDF page margins
            //htmlToPdfConverter.Document.Margins = new PdfMargins(5);

            // convert HTML to PDF
            byte[] pdfBuffer = null;

            string url = "http://" + HttpContext.Current.Request.Url.Authority + "/InSoHocSinh/InSoTheoHocSinh.aspx?id_hs=" + id_hs;

            pdfBuffer = htmlToPdfConverter.ConvertUrlToMemory(url);

            // inform the browser about the binary data format
            HttpContext.Current.Response.AddHeader("Content-Type", "application/pdf");

            // let the browser know how to open the PDF document, attachment or inline, and the file name
            HttpContext.Current.Response.AddHeader("Content-Disposition", String.Format("{0}; filename=HocBaHocSinh.pdf; size={1}", "attachment", pdfBuffer.Length.ToString()));

            // write the PDF buffer to HTTP response
            HttpContext.Current.Response.BinaryWrite(pdfBuffer);

            // call End() method of HTTP response to stop ASP.NET page processing
            HttpContext.Current.Response.End();
        }
    }
}