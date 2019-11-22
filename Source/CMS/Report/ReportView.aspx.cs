
using DevExpress.XtraReports.UI;
using OneEduDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS.Report
{
    public partial class ReportView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            XtraReport report = null;
            string reportID = Request.QueryString["ma"];
            switch (reportID)
            {
                case "BLTT":
                    report = new XtraReport1();
                    List<BienLaiThuTienHSEntity> lstBLTT = (List<BienLaiThuTienHSEntity>)Session["Datareport" + reportID];
                    report.DataSource = lstBLTT;
                    break;
                case "BTDHS1":
                    report = new BangTheoDoiHS_TH();
                    List<BangTheoDoiHocSinhTHEntity> lstBTDHS1 = (List<BangTheoDoiHocSinhTHEntity>)Session["DatareportBTDHS1" + reportID];
                    report.DataSource = lstBTDHS1;
                    break;
                case "BTDHS_THCS":
                    report = new BangTheoDoiHS_THCS();
                    List<BangTheoDoiHocSinhTHEntity> lstBTDHS_THCS = (List<BangTheoDoiHocSinhTHEntity>)Session["DatareportBTDHS_THCS" + reportID];
                    report.DataSource = lstBTDHS_THCS;
                    break;
                case "BTDHS_THCS_mau2":
                    report = new BangTheoDoiHS_THCS_Mau2();
                    List<BangTheoDoiHocSinhTHEntity> lstBTDHS_THCS_mau2 = (List<BangTheoDoiHocSinhTHEntity>)Session["DatareportBTDHS_THCS_mau2" + reportID];
                    report.DataSource = lstBTDHS_THCS_mau2;
                    break;
                case "BTDHS_THCS_NguyenTruongTo":
                    report = new BangTheoDoiHS_NguyenTruongTo();
                    List<BangTheoDoiHocSinhTHEntity> lstBTDHS_THCS_NTT = (List<BangTheoDoiHocSinhTHEntity>)Session["DatareportBTDHS_THCS_NguyenTruongTo" + reportID];
                    report.DataSource = lstBTDHS_THCS_NTT;
                    break;
                case "BTDHS_THCS_NguyenPhongSac":
                    report = new BangTheoDoiHS_NguyenPhongSac();
                    List<BangTheoDoiHocSinhTHEntity> lstBTDHS_THCS_NPS = (List<BangTheoDoiHocSinhTHEntity>)Session["DatareportBTDHS_THCS_NguyenPhongSac" + reportID];
                    report.DataSource = lstBTDHS_THCS_NPS;
                    break;
                case "BTDHS_THCS_HaiBaTrung":
                    report = new BangTheoDoiHS_HaiBaTrung();
                    List<BangTheoDoiHocSinhTHEntity> lstBTDHS_THCS_HaiBaTrung = (List<BangTheoDoiHocSinhTHEntity>)Session["DatareportBTDHS_THCS_HaiBaTrung" + reportID];
                    report.DataSource = lstBTDHS_THCS_HaiBaTrung;
                    break;
                case "BTDHS_LL":
                    report = new BangTheoDoiLanguageLink();
                    List<BangTheoDoiHocSinhTHEntity> lstBTDHS_LL = (List<BangTheoDoiHocSinhTHEntity>)Session["DatareportBTDHS_LL" + reportID];
                    report.DataSource = lstBTDHS_LL;
                    break;
                case "BTDHS_LL_TH":
                    report = new BangTheoDoiLanguageLink_TH();
                    List<BangTheoDoiHocSinhTHEntity> lstBTDHS_LL_TH = (List<BangTheoDoiHocSinhTHEntity>)Session["DatareportBTDHS_LL_TH" + reportID];
                    report.DataSource = lstBTDHS_LL_TH;
                    break;
                case "DangKyZalo":
                    report = new ReportDangKyOneduTrenZalo();
                    List<HocSinhEntity> lst = (List<HocSinhEntity>)Session["PhieuHuongDanDangKyZalo" + reportID];
                    report.DataSource = lst;
                    break;
                case "PhieuThuHocPhiHS":
                    report = new PhieuThuTongTienHocSinhTheoLop();
                    List<BienLaiThuTienHSEntity> lstHocPhiHS = (List<BienLaiThuTienHSEntity>)Session["ReportPhieuThuHocPhiHS" + reportID];
                    report.DataSource = lstHocPhiHS;
                    break;

                case "BTDHS1_he":
                    report = new BangTheoDoiHS_TH_he();
                    List<BangTheoDoiHocSinhTHEntity> lstBTDHS1_he = (List<BangTheoDoiHocSinhTHEntity>)Session["DatareportBTDHS1_he" + reportID];
                    report.DataSource = lstBTDHS1_he;
                    break;

                case "PhieuLienLacTH":
                    report = new PhieuLienLac_TH();
                    List<PhieuDanhGiaDinhKy_TH> lstLienLac = (List<PhieuDanhGiaDinhKy_TH>)Session["DatareportPhieuLienLacTH" + reportID];
                    report.DataSource = lstLienLac;
                    break;
                case "PhieuLienLacTH1":
                    report = new PhieuLienLac_TH1();
                    List<PhieuDanhGiaDinhKy_TH> lstLienLac1 = (List<PhieuDanhGiaDinhKy_TH>)Session["DatareportPhieuLienLacTH1" + reportID];
                    report.DataSource = lstLienLac1;
                    break;
                case "PhieuDoiSoatHS":
                    report = new PhieuDoiSoatHS();
                    List<BangTheoDoiHocSinhTHEntity> phieuDoiSoatHS = (List<BangTheoDoiHocSinhTHEntity>)Session["DatareportPhieuDoiSoatHS" + reportID];
                    report.DataSource = phieuDoiSoatHS;
                    break;
                case "BTDHSTH_NghiaTan":
                    report = new BangTheoDoiHS_TH_NghiaTan();
                    List<BangTheoDoiHocSinhTHEntity> lstHSNghiaTan = (List<BangTheoDoiHocSinhTHEntity>)Session["DatareportBTDHSTH_NghiaTan" + reportID];
                    report.DataSource = lstHSNghiaTan;
                    break;
                case "BTDHS_TH_NoPhone":
                    report = new BangTheoDoiHS_TH_NoPhone();
                    List<BangTheoDoiHocSinhTHEntity> listNoPhone = (List<BangTheoDoiHocSinhTHEntity>)Session["DatareportBTDHS_TH_NoPhone" + reportID];
                    report.DataSource = listNoPhone;
                    break;
                case "BTDHS_THCS_NoPhone":
                    report = new BangTheoDoiHS_THCS_NoPhone();
                    List<BangTheoDoiHocSinhTHEntity> list_THCS_NoPhone = (List<BangTheoDoiHocSinhTHEntity>)Session["DatareportBTDHS_THCS_NoPhone" + reportID];
                    report.DataSource = list_THCS_NoPhone;
                    break;
            }
            documentViewer.OpenReport(report);

        }
    }
}