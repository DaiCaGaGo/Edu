using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

/// <summary>
/// Summary description for ReportDangKyOneduTrenZalo
/// </summary>
public class ReportDangKyOneduTrenZalo : DevExpress.XtraReports.UI.XtraReport
{
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private XRTable xrTable1;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell1;
    private XRPictureBox xrPictureBox1;
    private XRTableCell xrTableCell2;
    private XRLabel xrLabel2;
    private XRLabel xrLabel1;
    private XRLine xrLine1;
    private XRLabel xrLabel3;
    private XRTable xrTable2;
    private XRTableRow xrTableRow2;
    private XRTableCell xrTableCell3;
    private XRTableCell xrTableCell4;
    private XRTable xrTable3;
    private XRTableRow xrTableRow3;
    private XRTableCell xrTableCell5;
    private XRTableCell xrTableCell6;
    private XRTableCell xrTableCell7;
    private XRTableCell xrTableCell8;
    private XRPictureBox xrPictureBox2;
    private XRTableCell xrTableCell9;
    private XRTableCell xrTableCell10;
    private XRPictureBox xrPictureBox3;
    private XRTableCell xrTableCell11;
    private XRPictureBox xrPictureBox4;
    private XRTable xrTable4;
    private XRTableRow xrTableRow4;
    private XRTableCell xrTableCell12;
    private XRPictureBox xrPictureBox5;
    private XRTableCell xrTableCell14;
    private XRTable xrTable5;
    private XRTableRow xrTableRow5;
    private XRTableCell xrTableCell13;
    private XRTableCell xrTableCell15;
    private XRPictureBox xrPictureBox6;
    private XRTableRow xrTableRow6;
    private XRTableCell xrTableCell16;
    private XRTableCell xrTableCell17;
    private XRPictureBox xrPictureBox7;
    private XRTableRow xrTableRow9;
    private XRTableCell xrTableCell22;
    private XRTableCell xrTableCell23;
    private XRTableRow xrTableRow8;
    private XRTableCell xrTableCell20;
    private XRTableCell xrTableCell21;
    private XRPictureBox xrPictureBox9;
    private XRTableRow xrTableRow7;
    private XRTableCell xrTableCell18;
    private XRTableCell xrTableCell19;
    private XRPictureBox xrPictureBox10;
    private XRTable xrTable6;
    private XRTableRow xrTableRow10;
    private XRTableCell xrTableCell24;
    private XRPictureBox xrPictureBox11;
    private XRTableCell xrTableCell25;
    private XRPageBreak xrPageBreak1;
    private XRTable xrTable7;
    private XRTableRow xrTableRow11;
    private XRTableCell xrTableCell26;
    private XRLabel xrLabel4;
    private XRLabel xrLabel5;
    private XRTable xrTable8;
    private XRTableRow xrTableRow12;
    private XRTableCell xrTableCell27;
    private XRPictureBox xrPictureBox12;
    private XRTable xrTable9;
    private XRTableRow xrTableRow15;
    private XRTableCell xrTableCell30;
    private XRLabel xrLabel6;
    private XRLabel xrLabel7;
    private XRTable xrTable10;
    private XRTableRow xrTableRow16;
    private XRTableCell xrTableCell31;
    private XRPictureBox xrPictureBox15;
    private XRLabel xrLabel8;
    private XRTableCell xrTableCell32;
    private XRLabel xrLabel9;
    private XRPictureBox xrPictureBox16;
    private XRTableRow xrTableRow13;
    private XRTableCell xrTableCell28;
    private XRPictureBox xrPictureBox13;
    private XRTableRow xrTableRow14;
    private XRTableCell xrTableCell29;
    private XRPictureBox xrPictureBox14;
    private XRTable xrTable11;
    private XRTableRow xrTableRow17;
    private XRTableCell xrTableCell33;
    private XRLabel xrLabel16;
    private XRLabel xrLabel14;
    private XRLabel xrLabel13;
    private XRLabel xrLabel10;
    private XRPictureBox xrPictureBox17;
    private XRLabel xrLabel12;
    private XRTableCell xrTableCell34;
    private XRLabel xrLabel21;
    private XRLabel xrLabel20;
    private XRLabel xrLabel19;
    private XRLabel xrLabel18;
    private XRLabel xrLabel17;
    private XRLabel xrLabel11;
    private XRTable xrTable12;
    private XRTableRow xrTableRow18;
    private XRTableCell xrTableCell35;
    private XRLabel xrLabel22;
    private XRLabel xrLabel23;
    private XRTableCell xrTableCell36;
    private XRPictureBox xrPictureBox20;
    private XRLabel xrLabel24;
    private XRLine xrLine3;
    private XRLine xrLine2;
    private XRLabel xrLabel26;
    private XRLine xrLine4;
    private XRLabel xrLabel25;
    private XRLabel xrLabel27;
    private XRLine xrLine5;
    private XRLabel xrLabel28;
    private XRLine xrLine6;
    private XRLine xrLine7;
    private XRLabel xrLabel29;
    private XRLine xrLine8;
    private XRLabel xrLabel30;
    private XRLine xrLine9;
    private XRLabel xrLabel31;
    private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource1;
    private XRLine xrLine10;
    private XRLabel xrLabel32;
    private XRPictureBox xrPictureBox8;
    private XRBarCode xrBarCode2;
    private XRPageBreak xrPageBreak2;
    private XRTable xrTable13;
    private XRTableRow xrTableRow19;
    private XRTableCell xrTableCell37;
    private XRLabel xrLabel34;
    private XRLabel xrLabel33;
    private XRPictureBox xrPictureBox18;
    private XRPictureBox xrPictureBox19;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public ReportDangKyOneduTrenZalo()
    {
        InitializeComponent();
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportDangKyOneduTrenZalo));
            DevExpress.XtraPrinting.BarCode.QRCodeGenerator qrCodeGenerator1 = new DevExpress.XtraPrinting.BarCode.QRCodeGenerator();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable9 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow15 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell30 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable6 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow10 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrPictureBox11 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrPictureBox5 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrPictureBox2 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrPictureBox3 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrPictureBox4 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xrTable5 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel26 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine4 = new DevExpress.XtraReports.UI.XRLine();
            this.xrPictureBox6 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel25 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine3 = new DevExpress.XtraReports.UI.XRLine();
            this.xrPictureBox7 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrTableRow9 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLine10 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel32 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox8 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrTableRow8 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel27 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine5 = new DevExpress.XtraReports.UI.XRLine();
            this.xrPictureBox9 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel28 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine6 = new DevExpress.XtraReports.UI.XRLine();
            this.xrPictureBox10 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrPageBreak1 = new DevExpress.XtraReports.UI.XRPageBreak();
            this.xrTable7 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow11 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable8 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow12 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLine7 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel29 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox12 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrTableRow13 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLine8 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel30 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox13 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrTableRow14 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell29 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLine9 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel31 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox14 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrTable10 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow16 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell31 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrPictureBox15 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableCell32 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox16 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrTable11 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow17 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell33 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable13 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow19 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell37 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrBarCode2 = new DevExpress.XtraReports.UI.XRBarCode();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox17 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableCell34 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel34 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel33 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox18 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrPictureBox19 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrTable12 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow18 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell35 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableCell36 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox20 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.xrPageBreak2 = new DevExpress.XtraReports.UI.XRPageBreak();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.xrTable9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable9,
            this.xrTable6,
            this.xrTable4,
            this.xrTable2,
            this.xrTable3,
            this.xrLabel3,
            this.xrTable1,
            this.xrTable5,
            this.xrPageBreak1,
            this.xrTable7,
            this.xrTable8,
            this.xrTable10,
            this.xrTable11,
            this.xrTable12,
            this.xrPageBreak2});
            this.Detail.HeightF = 2198.395F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTable9
            // 
            this.xrTable9.BackColor = System.Drawing.Color.Silver;
            this.xrTable9.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable9.LocationFloat = new DevExpress.Utils.PointFloat(0F, 1403.507F);
            this.xrTable9.Name = "xrTable9";
            this.xrTable9.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow15});
            this.xrTable9.SizeF = new System.Drawing.SizeF(383.4309F, 149.7919F);
            this.xrTable9.StylePriority.UseBackColor = false;
            this.xrTable9.StylePriority.UseFont = false;
            this.xrTable9.StylePriority.UseTextAlignment = false;
            this.xrTable9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            // 
            // xrTableRow15
            // 
            this.xrTableRow15.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell30});
            this.xrTableRow15.Name = "xrTableRow15";
            this.xrTableRow15.Weight = 1D;
            // 
            // xrTableCell30
            // 
            this.xrTableCell30.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel6,
            this.xrLabel7});
            this.xrTableCell30.Multiline = true;
            this.xrTableCell30.Name = "xrTableCell30";
            this.xrTableCell30.Weight = 1D;
            // 
            // xrLabel6
            // 
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(0F, 33.00006F);
            this.xrLabel6.Multiline = true;
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(365.8103F, 111.1665F);
            this.xrLabel6.Text = "     - Để đăng ký, người dùng chọn mục \"Khác\" -> \"Đăng ký\" ở bên dưới phải góc mà" +
    "n hình (hình H9).\r\n     - Trang đăng ký sẽ được hiển thị như hình H10:\r\n";
            // 
            // xrLabel7
            // 
            this.xrLabel7.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(2.379401F, 9.999974F);
            this.xrLabel7.Multiline = true;
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(363.4308F, 23.00008F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.Text = "ĐĂNG KÝ\r\n";
            // 
            // xrTable6
            // 
            this.xrTable6.BackColor = System.Drawing.Color.Transparent;
            this.xrTable6.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable6.LocationFloat = new DevExpress.Utils.PointFloat(0F, 1049.48F);
            this.xrTable6.Name = "xrTable6";
            this.xrTable6.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow10});
            this.xrTable6.SizeF = new System.Drawing.SizeF(774.0001F, 25F);
            this.xrTable6.StylePriority.UseBackColor = false;
            this.xrTable6.StylePriority.UseFont = false;
            this.xrTable6.StylePriority.UseTextAlignment = false;
            this.xrTable6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow10
            // 
            this.xrTableRow10.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell24,
            this.xrTableCell25});
            this.xrTableRow10.Name = "xrTableRow10";
            this.xrTableRow10.Weight = 1D;
            // 
            // xrTableCell24
            // 
            this.xrTableCell24.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox11});
            this.xrTableCell24.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell24.Name = "xrTableCell24";
            this.xrTableCell24.StylePriority.UseFont = false;
            this.xrTableCell24.Weight = 0.013958004545763916D;
            // 
            // xrPictureBox11
            // 
            this.xrPictureBox11.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox11.Image")));
            this.xrPictureBox11.LocationFloat = new DevExpress.Utils.PointFloat(2.37941F, 0F);
            this.xrPictureBox11.Name = "xrPictureBox11";
            this.xrPictureBox11.SizeF = new System.Drawing.SizeF(35.41666F, 23F);
            this.xrPictureBox11.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrTableCell25
            // 
            this.xrTableCell25.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell25.Name = "xrTableCell25";
            this.xrTableCell25.StylePriority.UseFont = false;
            this.xrTableCell25.Text = "BƯỚC 2: ĐĂNG KÝ";
            this.xrTableCell25.Weight = 0.21012133565439251D;
            // 
            // xrTable4
            // 
            this.xrTable4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 283.3333F);
            this.xrTable4.Name = "xrTable4";
            this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow4});
            this.xrTable4.SizeF = new System.Drawing.SizeF(774.0001F, 25F);
            this.xrTable4.StylePriority.UseFont = false;
            this.xrTable4.StylePriority.UseTextAlignment = false;
            this.xrTable4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell12,
            this.xrTableCell14});
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 1D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox5});
            this.xrTableCell12.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.StylePriority.UseFont = false;
            this.xrTableCell12.Weight = 0.013958004545763916D;
            // 
            // xrPictureBox5
            // 
            this.xrPictureBox5.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox5.Image")));
            this.xrPictureBox5.LocationFloat = new DevExpress.Utils.PointFloat(2.37941F, 0F);
            this.xrPictureBox5.Name = "xrPictureBox5";
            this.xrPictureBox5.SizeF = new System.Drawing.SizeF(35.41666F, 23F);
            this.xrPictureBox5.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.Text = "BƯỚC 1: QUÉT MÃ QR QUAN TÂM ONEDU TRÊN ZALO";
            this.xrTableCell14.Weight = 0.21012133565439251D;
            // 
            // xrTable2
            // 
            this.xrTable2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 210.4167F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(774.0001F, 25F);
            this.xrTable2.StylePriority.UseFont = false;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell3,
            this.xrTableCell4,
            this.xrTableCell9});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox2});
            this.xrTableCell3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.StylePriority.UseFont = false;
            this.xrTableCell3.Weight = 0.013957982458043529D;
            // 
            // xrPictureBox2
            // 
            this.xrPictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox2.Image")));
            this.xrPictureBox2.LocationFloat = new DevExpress.Utils.PointFloat(2.37941F, 0F);
            this.xrPictureBox2.Name = "xrPictureBox2";
            this.xrPictureBox2.SizeF = new System.Drawing.SizeF(35.41666F, 23F);
            this.xrPictureBox2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.StylePriority.UseFont = false;
            this.xrTableCell4.Text = "Trường:";
            this.xrTableCell4.Weight = 0.019449840886555065D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "TEN_TRUONG")});
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.Weight = 0.1906715168555578D;
            // 
            // xrTable3
            // 
            this.xrTable3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 247.9167F);
            this.xrTable3.Name = "xrTable3";
            this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow3});
            this.xrTable3.SizeF = new System.Drawing.SizeF(774.0001F, 25F);
            this.xrTable3.StylePriority.UseFont = false;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell10,
            this.xrTableCell5,
            this.xrTableCell6,
            this.xrTableCell11,
            this.xrTableCell7,
            this.xrTableCell8});
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 1D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox3});
            this.xrTableCell10.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.StylePriority.UseFont = false;
            this.xrTableCell10.Weight = 0.085450267759978771D;
            // 
            // xrPictureBox3
            // 
            this.xrPictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox3.Image")));
            this.xrPictureBox3.LocationFloat = new DevExpress.Utils.PointFloat(2.37941F, 0F);
            this.xrPictureBox3.Name = "xrPictureBox3";
            this.xrPictureBox3.SizeF = new System.Drawing.SizeF(35.41667F, 23F);
            this.xrPictureBox3.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.StylePriority.UseFont = false;
            this.xrTableCell5.Text = "Họ và tên:";
            this.xrTableCell5.Weight = 0.1432172473050973D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "HO_TEN")});
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.Weight = 0.69044308535812682D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox4});
            this.xrTableCell11.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.StylePriority.UseFont = false;
            this.xrTableCell11.Weight = 0.080994738780203024D;
            // 
            // xrPictureBox4
            // 
            this.xrPictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox4.Image")));
            this.xrPictureBox4.LocationFloat = new DevExpress.Utils.PointFloat(9.999974F, 0F);
            this.xrPictureBox4.Name = "xrPictureBox4";
            this.xrPictureBox4.SizeF = new System.Drawing.SizeF(32.6695F, 30.0026F);
            this.xrPictureBox4.Sizing = DevExpress.XtraPrinting.ImageSizeMode.AutoSize;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.StylePriority.UseFont = false;
            this.xrTableCell7.Text = "Lớp:";
            this.xrTableCell7.Weight = 0.0625329612901247D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.Text = "[TEN_LOP]";
            this.xrTableCell8.Weight = 0.30916715676396833D;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Font = new System.Drawing.Font("Times New Roman", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(100.7414F, 165.0626F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(599.6006F, 33.41667F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "HƯỚNG DẪN TRA CỨU ONEDU TRÊN ZALO";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTable1
            // 
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(2.37941F, 57.91667F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(754.0001F, 83.33334F);
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell2});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox1});
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Weight = 0.36140685909515324D;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox1.Image")));
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(131.3654F, 83.33334F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1,
            this.xrLabel2,
            this.xrLine1});
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Weight = 1.7129664605082031D;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Times New Roman", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(599.6006F, 33.41667F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "CÔNG TY CPTM CÔNG NGHỆ THÔNG TIN DI ĐỘNG";
            // 
            // xrLabel2
            // 
            this.xrLabel2.Font = new System.Drawing.Font("Times New Roman", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 45.91667F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(599.6006F, 37.41667F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.Text = "Phần mềm Sổ liên lạc điện tử Onedu";
            // 
            // xrLine1
            // 
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 33.41667F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(522.9167F, 2.083332F);
            // 
            // xrTable5
            // 
            this.xrTable5.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable5.LocationFloat = new DevExpress.Utils.PointFloat(0F, 308.3333F);
            this.xrTable5.Name = "xrTable5";
            this.xrTable5.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow5,
            this.xrTableRow6,
            this.xrTableRow9,
            this.xrTableRow8,
            this.xrTableRow7});
            this.xrTable5.SizeF = new System.Drawing.SizeF(771.6205F, 703.1249F);
            this.xrTable5.StylePriority.UseFont = false;
            // 
            // xrTableRow5
            // 
            this.xrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell13,
            this.xrTableCell15});
            this.xrTableRow5.Name = "xrTableRow5";
            this.xrTableRow5.Weight = 1D;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.StylePriority.UseTextAlignment = false;
            this.xrTableCell13.Text = "Sau khi đăng nhập vào Zalo, người dùng chọn biểu tượng ngoài cùng bên góc phải ph" +
    "ía dưới màn hình theo mũi tên chỉ như hình H1:";
            this.xrTableCell13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            this.xrTableCell13.Weight = 1D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel26,
            this.xrLine4,
            this.xrPictureBox6});
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.Weight = 1D;
            // 
            // xrLabel26
            // 
            this.xrLabel26.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel26.LocationFloat = new DevExpress.Utils.PointFloat(12.37949F, 76.12511F);
            this.xrLabel26.Name = "xrLabel26";
            this.xrLabel26.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel26.SizeF = new System.Drawing.SizeF(50.00003F, 31.66669F);
            this.xrLabel26.StylePriority.UseFont = false;
            this.xrLabel26.StylePriority.UseTextAlignment = false;
            this.xrLabel26.Text = "H1";
            this.xrLabel26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLine4
            // 
            this.xrLine4.LocationFloat = new DevExpress.Utils.PointFloat(12.37939F, 61.4584F);
            this.xrLine4.Name = "xrLine4";
            this.xrLine4.SizeF = new System.Drawing.SizeF(50.00003F, 14.66662F);
            // 
            // xrPictureBox6
            // 
            this.xrPictureBox6.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox6.Image")));
            this.xrPictureBox6.LocationFloat = new DevExpress.Utils.PointFloat(73.54158F, 10.00001F);
            this.xrPictureBox6.Name = "xrPictureBox6";
            this.xrPictureBox6.SizeF = new System.Drawing.SizeF(302.2687F, 120.625F);
            this.xrPictureBox6.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrTableRow6
            // 
            this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell16,
            this.xrTableCell17});
            this.xrTableRow6.Name = "xrTableRow6";
            this.xrTableRow6.Weight = 1D;
            // 
            // xrTableCell16
            // 
            this.xrTableCell16.Name = "xrTableCell16";
            this.xrTableCell16.StylePriority.UseTextAlignment = false;
            this.xrTableCell16.Text = "Bên góc phải phía trên màn hình, người dùng chọn nút quét mã QR code theo hình mũ" +
    "i tên chỉ H2:";
            this.xrTableCell16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            this.xrTableCell16.Weight = 1D;
            // 
            // xrTableCell17
            // 
            this.xrTableCell17.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel25,
            this.xrLine3,
            this.xrPictureBox7});
            this.xrTableCell17.Name = "xrTableCell17";
            this.xrTableCell17.Weight = 1D;
            // 
            // xrLabel25
            // 
            this.xrLabel25.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel25.LocationFloat = new DevExpress.Utils.PointFloat(12.37949F, 79.25011F);
            this.xrLabel25.Name = "xrLabel25";
            this.xrLabel25.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel25.SizeF = new System.Drawing.SizeF(50.00003F, 31.66669F);
            this.xrLabel25.StylePriority.UseFont = false;
            this.xrLabel25.StylePriority.UseTextAlignment = false;
            this.xrLabel25.Text = "H2";
            this.xrLabel25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLine3
            // 
            this.xrLine3.LocationFloat = new DevExpress.Utils.PointFloat(12.37939F, 64.58346F);
            this.xrLine3.Name = "xrLine3";
            this.xrLine3.SizeF = new System.Drawing.SizeF(50.00003F, 14.66662F);
            // 
            // xrPictureBox7
            // 
            this.xrPictureBox7.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox7.Image")));
            this.xrPictureBox7.LocationFloat = new DevExpress.Utils.PointFloat(73.54155F, 10.00001F);
            this.xrPictureBox7.Name = "xrPictureBox7";
            this.xrPictureBox7.SizeF = new System.Drawing.SizeF(302.2687F, 120.625F);
            this.xrPictureBox7.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrTableRow9
            // 
            this.xrTableRow9.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell22,
            this.xrTableCell23});
            this.xrTableRow9.Name = "xrTableRow9";
            this.xrTableRow9.Weight = 1D;
            // 
            // xrTableCell22
            // 
            this.xrTableCell22.Name = "xrTableCell22";
            this.xrTableCell22.StylePriority.UseTextAlignment = false;
            this.xrTableCell22.Text = "Di chuyển camera vào hình H3 bên dưới, quét mã QR code để quan tâm ONEDU trên Zal" +
    "o";
            this.xrTableCell22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            this.xrTableCell22.Weight = 1.3441339067553746D;
            // 
            // xrTableCell23
            // 
            this.xrTableCell23.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine10,
            this.xrLabel32,
            this.xrPictureBox8});
            this.xrTableCell23.Name = "xrTableCell23";
            this.xrTableCell23.Weight = 0.65586609324462541D;
            // 
            // xrLine10
            // 
            this.xrLine10.LocationFloat = new DevExpress.Utils.PointFloat(20.55117F, 52.0834F);
            this.xrLine10.Name = "xrLine10";
            this.xrLine10.SizeF = new System.Drawing.SizeF(50.00003F, 14.66662F);
            // 
            // xrLabel32
            // 
            this.xrLabel32.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel32.LocationFloat = new DevExpress.Utils.PointFloat(20.5513F, 66.75002F);
            this.xrLabel32.Name = "xrLabel32";
            this.xrLabel32.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel32.SizeF = new System.Drawing.SizeF(50.00003F, 31.66669F);
            this.xrLabel32.StylePriority.UseFont = false;
            this.xrLabel32.StylePriority.UseTextAlignment = false;
            this.xrLabel32.Text = "H3";
            this.xrLabel32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrPictureBox8
            // 
            this.xrPictureBox8.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox8.Image")));
            this.xrPictureBox8.LocationFloat = new DevExpress.Utils.PointFloat(81.76136F, 9.999974F);
            this.xrPictureBox8.Name = "xrPictureBox8";
            this.xrPictureBox8.SizeF = new System.Drawing.SizeF(163.658F, 120.625F);
            this.xrPictureBox8.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrTableRow8
            // 
            this.xrTableRow8.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell20,
            this.xrTableCell21});
            this.xrTableRow8.Name = "xrTableRow8";
            this.xrTableRow8.Weight = 1D;
            // 
            // xrTableCell20
            // 
            this.xrTableCell20.Name = "xrTableCell20";
            this.xrTableCell20.StylePriority.UseTextAlignment = false;
            this.xrTableCell20.Text = "Sau khi quét thành công, màn hình quan tâm sẽ hiển thị như H4, nhấn chọn \"Quan tâ" +
    "m\"";
            this.xrTableCell20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            this.xrTableCell20.Weight = 1D;
            // 
            // xrTableCell21
            // 
            this.xrTableCell21.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel27,
            this.xrLine5,
            this.xrPictureBox9});
            this.xrTableCell21.Name = "xrTableCell21";
            this.xrTableCell21.Weight = 1D;
            // 
            // xrLabel27
            // 
            this.xrLabel27.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel27.LocationFloat = new DevExpress.Utils.PointFloat(12.37949F, 77.16662F);
            this.xrLabel27.Name = "xrLabel27";
            this.xrLabel27.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel27.SizeF = new System.Drawing.SizeF(50.00003F, 31.66669F);
            this.xrLabel27.StylePriority.UseFont = false;
            this.xrLabel27.StylePriority.UseTextAlignment = false;
            this.xrLabel27.Text = "H4";
            this.xrLabel27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLine5
            // 
            this.xrLine5.LocationFloat = new DevExpress.Utils.PointFloat(12.37939F, 62.5F);
            this.xrLine5.Name = "xrLine5";
            this.xrLine5.SizeF = new System.Drawing.SizeF(50.00003F, 14.66662F);
            // 
            // xrPictureBox9
            // 
            this.xrPictureBox9.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox9.Image")));
            this.xrPictureBox9.LocationFloat = new DevExpress.Utils.PointFloat(73.5417F, 9.999847F);
            this.xrPictureBox9.Name = "xrPictureBox9";
            this.xrPictureBox9.SizeF = new System.Drawing.SizeF(302.2686F, 120.6251F);
            this.xrPictureBox9.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrTableRow7
            // 
            this.xrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell18,
            this.xrTableCell19});
            this.xrTableRow7.Name = "xrTableRow7";
            this.xrTableRow7.Weight = 1D;
            // 
            // xrTableCell18
            // 
            this.xrTableCell18.Name = "xrTableCell18";
            this.xrTableCell18.StylePriority.UseTextAlignment = false;
            this.xrTableCell18.Text = "Sau khi quan tâm thành công, hệ thống đưa ra thông báo thành công như H5:";
            this.xrTableCell18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            this.xrTableCell18.Weight = 1D;
            // 
            // xrTableCell19
            // 
            this.xrTableCell19.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel28,
            this.xrLine6,
            this.xrPictureBox10});
            this.xrTableCell19.Name = "xrTableCell19";
            this.xrTableCell19.Weight = 1D;
            // 
            // xrLabel28
            // 
            this.xrLabel28.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel28.LocationFloat = new DevExpress.Utils.PointFloat(12.37946F, 75.08335F);
            this.xrLabel28.Name = "xrLabel28";
            this.xrLabel28.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel28.SizeF = new System.Drawing.SizeF(50F, 31.66669F);
            this.xrLabel28.StylePriority.UseFont = false;
            this.xrLabel28.StylePriority.UseTextAlignment = false;
            this.xrLabel28.Text = "H5";
            this.xrLabel28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLine6
            // 
            this.xrLine6.LocationFloat = new DevExpress.Utils.PointFloat(12.37949F, 60.41673F);
            this.xrLine6.Name = "xrLine6";
            this.xrLine6.SizeF = new System.Drawing.SizeF(50.00003F, 14.66662F);
            // 
            // xrPictureBox10
            // 
            this.xrPictureBox10.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox10.Image")));
            this.xrPictureBox10.LocationFloat = new DevExpress.Utils.PointFloat(73.5417F, 9.999974F);
            this.xrPictureBox10.Name = "xrPictureBox10";
            this.xrPictureBox10.SizeF = new System.Drawing.SizeF(302.2685F, 120.625F);
            this.xrPictureBox10.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrPageBreak1
            // 
            this.xrPageBreak1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 1031.292F);
            this.xrPageBreak1.Name = "xrPageBreak1";
            // 
            // xrTable7
            // 
            this.xrTable7.BackColor = System.Drawing.Color.Transparent;
            this.xrTable7.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable7.LocationFloat = new DevExpress.Utils.PointFloat(0F, 1075.05F);
            this.xrTable7.Name = "xrTable7";
            this.xrTable7.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow11});
            this.xrTable7.SizeF = new System.Drawing.SizeF(383.4309F, 327.9164F);
            this.xrTable7.StylePriority.UseBackColor = false;
            this.xrTable7.StylePriority.UseFont = false;
            this.xrTable7.StylePriority.UseTextAlignment = false;
            this.xrTable7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            // 
            // xrTableRow11
            // 
            this.xrTableRow11.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell26});
            this.xrTableRow11.Name = "xrTableRow11";
            this.xrTableRow11.Weight = 1D;
            // 
            // xrTableCell26
            // 
            this.xrTableCell26.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel4,
            this.xrLabel5});
            this.xrTableCell26.Multiline = true;
            this.xrTableCell26.Name = "xrTableCell26";
            this.xrTableCell26.Weight = 1D;
            // 
            // xrLabel4
            // 
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 104.2502F);
            this.xrLabel4.Multiline = true;
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(365.8103F, 124.7082F);
            this.xrLabel4.Text = "      - Người dùng chọn vào biểu tượng bên dưới góc trái màn hình (hình H6)\r\n    " +
    "  - Chọn Offical Account (hình H7)\r\n      - Tìm kiếm đến Onedu (hình H8), chọn đ" +
    "ể đăng ký và xem thông tin học sinh\r\n";
            // 
            // xrLabel5
            // 
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(0F, 65.00053F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(363.4308F, 34.30494F);
            this.xrLabel5.Text = "TÌM KIẾM ONEDU TRÊN ZALO";
            // 
            // xrTable8
            // 
            this.xrTable8.BackColor = System.Drawing.Color.Transparent;
            this.xrTable8.LocationFloat = new DevExpress.Utils.PointFloat(385.8102F, 1075.05F);
            this.xrTable8.Name = "xrTable8";
            this.xrTable8.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow12,
            this.xrTableRow13,
            this.xrTableRow14});
            this.xrTable8.SizeF = new System.Drawing.SizeF(385.8103F, 327.9164F);
            this.xrTable8.StylePriority.UseBackColor = false;
            this.xrTable8.StylePriority.UseFont = false;
            // 
            // xrTableRow12
            // 
            this.xrTableRow12.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell27});
            this.xrTableRow12.Name = "xrTableRow12";
            this.xrTableRow12.Weight = 1D;
            // 
            // xrTableCell27
            // 
            this.xrTableCell27.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine7,
            this.xrLabel29,
            this.xrPictureBox12});
            this.xrTableCell27.Name = "xrTableCell27";
            this.xrTableCell27.Weight = 1D;
            // 
            // xrLine7
            // 
            this.xrLine7.LocationFloat = new DevExpress.Utils.PointFloat(12.37958F, 41.6673F);
            this.xrLine7.Name = "xrLine7";
            this.xrLine7.SizeF = new System.Drawing.SizeF(50.00003F, 14.66662F);
            // 
            // xrLabel29
            // 
            this.xrLabel29.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel29.LocationFloat = new DevExpress.Utils.PointFloat(12.37958F, 56.33405F);
            this.xrLabel29.Name = "xrLabel29";
            this.xrLabel29.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel29.SizeF = new System.Drawing.SizeF(50F, 31.66669F);
            this.xrLabel29.StylePriority.UseFont = false;
            this.xrLabel29.StylePriority.UseTextAlignment = false;
            this.xrLabel29.Text = "H6";
            this.xrLabel29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrPictureBox12
            // 
            this.xrPictureBox12.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox12.Image")));
            this.xrPictureBox12.LocationFloat = new DevExpress.Utils.PointFloat(73.54173F, 0F);
            this.xrPictureBox12.Name = "xrPictureBox12";
            this.xrPictureBox12.SizeF = new System.Drawing.SizeF(297.0276F, 99.30547F);
            this.xrPictureBox12.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrTableRow13
            // 
            this.xrTableRow13.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell28});
            this.xrTableRow13.Name = "xrTableRow13";
            this.xrTableRow13.Weight = 1D;
            // 
            // xrTableCell28
            // 
            this.xrTableCell28.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine8,
            this.xrLabel30,
            this.xrPictureBox13});
            this.xrTableCell28.Name = "xrTableCell28";
            this.xrTableCell28.Weight = 1D;
            // 
            // xrLine8
            // 
            this.xrLine8.LocationFloat = new DevExpress.Utils.PointFloat(12.37946F, 41.58376F);
            this.xrLine8.Name = "xrLine8";
            this.xrLine8.SizeF = new System.Drawing.SizeF(50.00003F, 14.66662F);
            // 
            // xrLabel30
            // 
            this.xrLabel30.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel30.LocationFloat = new DevExpress.Utils.PointFloat(12.37955F, 56.25038F);
            this.xrLabel30.Name = "xrLabel30";
            this.xrLabel30.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel30.SizeF = new System.Drawing.SizeF(50F, 31.66669F);
            this.xrLabel30.StylePriority.UseFont = false;
            this.xrLabel30.StylePriority.UseTextAlignment = false;
            this.xrLabel30.Text = "H7";
            this.xrLabel30.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrPictureBox13
            // 
            this.xrPictureBox13.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox13.Image")));
            this.xrPictureBox13.LocationFloat = new DevExpress.Utils.PointFloat(71.31204F, 0F);
            this.xrPictureBox13.Name = "xrPictureBox13";
            this.xrPictureBox13.SizeF = new System.Drawing.SizeF(304.4982F, 99.30547F);
            this.xrPictureBox13.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrTableRow14
            // 
            this.xrTableRow14.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell29});
            this.xrTableRow14.Name = "xrTableRow14";
            this.xrTableRow14.Weight = 1D;
            // 
            // xrTableCell29
            // 
            this.xrTableCell29.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine9,
            this.xrLabel31,
            this.xrPictureBox14});
            this.xrTableCell29.Name = "xrTableCell29";
            this.xrTableCell29.Weight = 1D;
            // 
            // xrLine9
            // 
            this.xrLine9.LocationFloat = new DevExpress.Utils.PointFloat(12.37958F, 42.70884F);
            this.xrLine9.Name = "xrLine9";
            this.xrLine9.SizeF = new System.Drawing.SizeF(50.00003F, 14.66662F);
            // 
            // xrLabel31
            // 
            this.xrLabel31.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel31.LocationFloat = new DevExpress.Utils.PointFloat(12.37955F, 57.37546F);
            this.xrLabel31.Name = "xrLabel31";
            this.xrLabel31.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel31.SizeF = new System.Drawing.SizeF(50F, 31.66669F);
            this.xrLabel31.StylePriority.UseFont = false;
            this.xrLabel31.StylePriority.UseTextAlignment = false;
            this.xrLabel31.Text = "H8";
            this.xrLabel31.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrPictureBox14
            // 
            this.xrPictureBox14.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox14.Image")));
            this.xrPictureBox14.LocationFloat = new DevExpress.Utils.PointFloat(71.31204F, 0F);
            this.xrPictureBox14.Name = "xrPictureBox14";
            this.xrPictureBox14.SizeF = new System.Drawing.SizeF(304.4983F, 99.30547F);
            this.xrPictureBox14.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrTable10
            // 
            this.xrTable10.BackColor = System.Drawing.Color.Silver;
            this.xrTable10.LocationFloat = new DevExpress.Utils.PointFloat(383.4308F, 1403.507F);
            this.xrTable10.Name = "xrTable10";
            this.xrTable10.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow16});
            this.xrTable10.SizeF = new System.Drawing.SizeF(385.8103F, 149.792F);
            this.xrTable10.StylePriority.UseBackColor = false;
            // 
            // xrTableRow16
            // 
            this.xrTableRow16.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell31,
            this.xrTableCell32});
            this.xrTableRow16.Name = "xrTableRow16";
            this.xrTableRow16.Weight = 1D;
            // 
            // xrTableCell31
            // 
            this.xrTableCell31.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox15,
            this.xrLabel8});
            this.xrTableCell31.Name = "xrTableCell31";
            this.xrTableCell31.Weight = 1D;
            // 
            // xrPictureBox15
            // 
            this.xrPictureBox15.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox15.Image")));
            this.xrPictureBox15.LocationFloat = new DevExpress.Utils.PointFloat(0F, 6.166967F);
            this.xrPictureBox15.Name = "xrPictureBox15";
            this.xrPictureBox15.SizeF = new System.Drawing.SizeF(182.9053F, 120.625F);
            this.xrPictureBox15.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrLabel8
            // 
            this.xrLabel8.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(73.54158F, 126.792F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(31.25003F, 22.99995F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.Text = "H9";
            // 
            // xrTableCell32
            // 
            this.xrTableCell32.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel9,
            this.xrPictureBox16});
            this.xrTableCell32.Name = "xrTableCell32";
            this.xrTableCell32.Weight = 1D;
            // 
            // xrLabel9
            // 
            this.xrLabel9.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(75.49292F, 126.792F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(46.13368F, 22.99995F);
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.Text = "H10";
            // 
            // xrPictureBox16
            // 
            this.xrPictureBox16.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox16.Image")));
            this.xrPictureBox16.LocationFloat = new DevExpress.Utils.PointFloat(2.379735F, 6.166712F);
            this.xrPictureBox16.Name = "xrPictureBox16";
            this.xrPictureBox16.SizeF = new System.Drawing.SizeF(182.9051F, 120.6253F);
            this.xrPictureBox16.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrTable11
            // 
            this.xrTable11.BackColor = System.Drawing.Color.Silver;
            this.xrTable11.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable11.LocationFloat = new DevExpress.Utils.PointFloat(0F, 1553.299F);
            this.xrTable11.Name = "xrTable11";
            this.xrTable11.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow17});
            this.xrTable11.SizeF = new System.Drawing.SizeF(769.2411F, 367.7091F);
            this.xrTable11.StylePriority.UseBackColor = false;
            this.xrTable11.StylePriority.UseFont = false;
            // 
            // xrTableRow17
            // 
            this.xrTableRow17.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell33,
            this.xrTableCell34});
            this.xrTableRow17.Name = "xrTableRow17";
            this.xrTableRow17.Weight = 1D;
            // 
            // xrTableCell33
            // 
            this.xrTableCell33.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel11,
            this.xrTable13,
            this.xrLabel16,
            this.xrLabel14,
            this.xrLabel13,
            this.xrLabel10,
            this.xrPictureBox17,
            this.xrLabel12});
            this.xrTableCell33.Multiline = true;
            this.xrTableCell33.Name = "xrTableCell33";
            this.xrTableCell33.Weight = 1D;
            // 
            // xrLabel11
            // 
            this.xrLabel11.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(254.8104F, 345.5419F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(37.50006F, 22.99995F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.Text = "H12";
            // 
            // xrTable13
            // 
            this.xrTable13.BackColor = System.Drawing.Color.White;
            this.xrTable13.LocationFloat = new DevExpress.Utils.PointFloat(173.458F, 178.5421F);
            this.xrTable13.Name = "xrTable13";
            this.xrTable13.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow19});
            this.xrTable13.SizeF = new System.Drawing.SizeF(201.1854F, 166.9998F);
            this.xrTable13.StylePriority.UseBackColor = false;
            // 
            // xrTableRow19
            // 
            this.xrTableRow19.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell37});
            this.xrTableRow19.Name = "xrTableRow19";
            this.xrTableRow19.Weight = 1D;
            // 
            // xrTableCell37
            // 
            this.xrTableCell37.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrBarCode2});
            this.xrTableCell37.Name = "xrTableCell37";
            this.xrTableCell37.Weight = 1D;
            // 
            // xrBarCode2
            // 
            this.xrBarCode2.Alignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrBarCode2.AnchorHorizontal = DevExpress.XtraReports.UI.HorizontalAnchorStyles.Left;
            this.xrBarCode2.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Top;
            this.xrBarCode2.AutoModule = true;
            this.xrBarCode2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "MA")});
            this.xrBarCode2.Font = new System.Drawing.Font("Times New Roman", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrBarCode2.ForeColor = System.Drawing.Color.Black;
            this.xrBarCode2.LocationFloat = new DevExpress.Utils.PointFloat(4.653422F, 9.999974F);
            this.xrBarCode2.LockedInUserDesigner = true;
            this.xrBarCode2.Module = 1F;
            this.xrBarCode2.Name = "xrBarCode2";
            this.xrBarCode2.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrBarCode2.SizeF = new System.Drawing.SizeF(187.7218F, 147F);
            this.xrBarCode2.StylePriority.UseForeColor = false;
            this.xrBarCode2.StylePriority.UseTextAlignment = false;
            qrCodeGenerator1.Version = DevExpress.XtraPrinting.BarCode.QRCodeVersion.Version3;
            this.xrBarCode2.Symbology = qrCodeGenerator1;
            this.xrBarCode2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel16
            // 
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(0F, 188.5421F);
            this.xrLabel16.Multiline = true;
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(162.2226F, 63.70799F);
            this.xrLabel16.StylePriority.UseTextAlignment = false;
            this.xrLabel16.Text = "    - Nhấn \"Đăng ký\" (4) để đăng ký.";
            this.xrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            // 
            // xrLabel14
            // 
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(0F, 105.6262F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(162.2226F, 82.91575F);
            this.xrLabel14.StylePriority.UseTextAlignment = false;
            this.xrLabel14.Text = "     - Nhập Mã Học sinh bên dưới hình H12 vào mục (2)";
            this.xrLabel14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            // 
            // xrLabel13
            // 
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(0F, 23.00059F);
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(164.602F, 82.62546F);
            this.xrLabel13.StylePriority.UseTextAlignment = false;
            this.xrLabel13.Text = "      - Người dùng nhập Số điện thoại của mình vào mục (1) ";
            this.xrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            // 
            // xrLabel10
            // 
            this.xrLabel10.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(254.8104F, 155.5421F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(37.50006F, 22.99995F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.Text = "H11";
            // 
            // xrPictureBox17
            // 
            this.xrPictureBox17.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox17.Image")));
            this.xrPictureBox17.LocationFloat = new DevExpress.Utils.PointFloat(173.4351F, 0.0006357829F);
            this.xrPictureBox17.Name = "xrPictureBox17";
            this.xrPictureBox17.SizeF = new System.Drawing.SizeF(201.2083F, 155.5415F);
            this.xrPictureBox17.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrLabel12
            // 
            this.xrLabel12.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(2.379401F, 0.0006357829F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(162.2226F, 22.99995F);
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.Text = "Cách 1: Nhập mã HS";
            // 
            // xrTableCell34
            // 
            this.xrTableCell34.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel34,
            this.xrLabel33,
            this.xrLabel21,
            this.xrLabel20,
            this.xrLabel19,
            this.xrLabel18,
            this.xrLabel17,
            this.xrPictureBox18,
            this.xrPictureBox19});
            this.xrTableCell34.Name = "xrTableCell34";
            this.xrTableCell34.Weight = 1D;
            // 
            // xrLabel34
            // 
            this.xrLabel34.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel34.LocationFloat = new DevExpress.Utils.PointFloat(275.8421F, 345.5419F);
            this.xrLabel34.Name = "xrLabel34";
            this.xrLabel34.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel34.SizeF = new System.Drawing.SizeF(37.50006F, 22.99995F);
            this.xrLabel34.StylePriority.UseFont = false;
            this.xrLabel34.Text = "H14";
            // 
            // xrLabel33
            // 
            this.xrLabel33.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel33.LocationFloat = new DevExpress.Utils.PointFloat(275.842F, 155.5421F);
            this.xrLabel33.Name = "xrLabel33";
            this.xrLabel33.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel33.SizeF = new System.Drawing.SizeF(37.50006F, 22.99995F);
            this.xrLabel33.StylePriority.UseFont = false;
            this.xrLabel33.Text = "H13";
            // 
            // xrLabel21
            // 
            this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(0F, 302.459F);
            this.xrLabel21.Name = "xrLabel21";
            this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel21.SizeF = new System.Drawing.SizeF(181.7157F, 53.20828F);
            this.xrLabel21.StylePriority.UseTextAlignment = false;
            this.xrLabel21.Text = "   - Nhấn \"Đăng ký\" (4) để đăng ký.";
            this.xrLabel21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            // 
            // xrLabel20
            // 
            this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(0F, 200.0005F);
            this.xrLabel20.Name = "xrLabel20";
            this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel20.SizeF = new System.Drawing.SizeF(179.3358F, 102.4585F);
            this.xrLabel20.StylePriority.UseTextAlignment = false;
            this.xrLabel20.Text = "   - Chọn nút tích bên góc phải phía trên màn hình sau khi chụp như hình H14. Khi" +
    " đó mã HS sẽ được hiển thị ở mục (2). ";
            this.xrLabel20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            // 
            // xrLabel19
            // 
            this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(0.3230413F, 94.16796F);
            this.xrLabel19.Name = "xrLabel19";
            this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel19.SizeF = new System.Drawing.SizeF(179.3358F, 105.8325F);
            this.xrLabel19.StylePriority.UseTextAlignment = false;
            this.xrLabel19.Text = "   - Bấm chọn \"QR Code\" (3). Chọn \"Máy ảnh\" như hình H13 để chụp ảnh mã QR trong " +
    "hình H12.";
            this.xrLabel19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopJustify;
            // 
            // xrLabel18
            // 
            this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(0F, 23.00059F);
            this.xrLabel18.Name = "xrLabel18";
            this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel18.SizeF = new System.Drawing.SizeF(181.7155F, 71.16737F);
            this.xrLabel18.StylePriority.UseTextAlignment = false;
            this.xrLabel18.Text = "   - Người dùng nhập Số điện thoại của mình vào mục (1) ";
            this.xrLabel18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            // 
            // xrLabel17
            // 
            this.xrLabel17.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel17.SizeF = new System.Drawing.SizeF(181.7154F, 22.99995F);
            this.xrLabel17.StylePriority.UseFont = false;
            this.xrLabel17.Text = "Cách 2: Quét mã QR";
            // 
            // xrPictureBox18
            // 
            this.xrPictureBox18.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox18.Image")));
            this.xrPictureBox18.LocationFloat = new DevExpress.Utils.PointFloat(194.0952F, 0F);
            this.xrPictureBox18.Name = "xrPictureBox18";
            this.xrPictureBox18.SizeF = new System.Drawing.SizeF(180.5254F, 155.5411F);
            this.xrPictureBox18.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrPictureBox19
            // 
            this.xrPictureBox19.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox19.Image")));
            this.xrPictureBox19.LocationFloat = new DevExpress.Utils.PointFloat(194.0952F, 178.5421F);
            this.xrPictureBox19.Name = "xrPictureBox19";
            this.xrPictureBox19.SizeF = new System.Drawing.SizeF(182.9051F, 167F);
            this.xrPictureBox19.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrTable12
            // 
            this.xrTable12.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable12.LocationFloat = new DevExpress.Utils.PointFloat(0F, 1921.841F);
            this.xrTable12.Name = "xrTable12";
            this.xrTable12.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow18});
            this.xrTable12.SizeF = new System.Drawing.SizeF(771.6206F, 144.792F);
            this.xrTable12.StylePriority.UseFont = false;
            // 
            // xrTableRow18
            // 
            this.xrTableRow18.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell35,
            this.xrTableCell36});
            this.xrTableRow18.Name = "xrTableRow18";
            this.xrTableRow18.Weight = 1D;
            // 
            // xrTableCell35
            // 
            this.xrTableCell35.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel22,
            this.xrLabel23});
            this.xrTableCell35.Name = "xrTableCell35";
            this.xrTableCell35.Weight = 1D;
            // 
            // xrLabel22
            // 
            this.xrLabel22.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel22.Name = "xrLabel22";
            this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel22.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel22.StylePriority.UseFont = false;
            this.xrLabel22.Text = "XÁC NHẬN";
            // 
            // xrLabel23
            // 
            this.xrLabel23.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(2.379401F, 23.00034F);
            this.xrLabel23.Multiline = true;
            this.xrLabel23.Name = "xrLabel23";
            this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel23.SizeF = new System.Drawing.SizeF(381.0515F, 120.1257F);
            this.xrLabel23.StylePriority.UseFont = false;
            this.xrLabel23.StylePriority.UseTextAlignment = false;
            this.xrLabel23.Text = resources.GetString("xrLabel23.Text");
            this.xrLabel23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            // 
            // xrTableCell36
            // 
            this.xrTableCell36.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel24,
            this.xrPictureBox20,
            this.xrLine2});
            this.xrTableCell36.Name = "xrTableCell36";
            this.xrTableCell36.Weight = 1D;
            // 
            // xrLabel24
            // 
            this.xrLabel24.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(7.620493F, 82.33324F);
            this.xrLabel24.Name = "xrLabel24";
            this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel24.SizeF = new System.Drawing.SizeF(50.0001F, 23.00021F);
            this.xrLabel24.StylePriority.UseFont = false;
            this.xrLabel24.StylePriority.UseTextAlignment = false;
            this.xrLabel24.Text = "H13";
            this.xrLabel24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrPictureBox20
            // 
            this.xrPictureBox20.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox20.Image")));
            this.xrPictureBox20.LocationFloat = new DevExpress.Utils.PointFloat(71.16219F, 22.99995F);
            this.xrPictureBox20.Name = "xrPictureBox20";
            this.xrPictureBox20.SizeF = new System.Drawing.SizeF(304.6481F, 106.3333F);
            this.xrPictureBox20.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrLine2
            // 
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(7.620525F, 67.66663F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(50.00003F, 14.66662F);
            // 
            // xrPageBreak2
            // 
            this.xrPageBreak2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 2105.25F);
            this.xrPageBreak2.Name = "xrPageBreak2";
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 18F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 100F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataSource = typeof(OneEduDataAccess.HocSinhEntity);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // ReportDangKyOneduTrenZalo
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource1});
            this.DataSource = this.objectDataSource1;
            this.Margins = new System.Drawing.Printing.Margins(26, 27, 18, 100);
            this.PageHeight = 1169;
            this.PageWidth = 827;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.Version = "17.1";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}
