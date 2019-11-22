<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CMS.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/AdminLTE.min.css" rel="stylesheet" />
    <link href="CSS/ionicons.min.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row" style="margin-top: 15px">
        <div class="col-sm-4">
            <a href='<%="http://"+HttpContext.Current.Request.Url.Authority+ "/SMS/LichSuTinNhan.aspx?sms_type=1" %>'>
                <div class="info-box bg-light-blue">
                    <span class="info-box-icon"><i class="ion-ios-email-outline" style="font-size: 80px; margin-top: 5px;"></i></span>
                    <div class="info-box-content">
                        <span class="info-box-text">Quỹ tin liên lạc</span>
                        <span class="info-box-number"><span class="progress-description">
                            <asp:Literal ID="ltrLL" runat="server" Text=""></asp:Literal></span></span>
                        <div class="progress">
                            <div class="progress-bar" id="proQuyTinLL" runat="server"></div>
                        </div>
                        <span class="progress-description">
                            <asp:Label ID="lbDisQuyTinLL" runat="server" class="progress-description"></asp:Label>
                        </span>
                    </div>
                    <!-- /.info-box-content -->
                </div>
            </a>
        </div>
        <div class="col-sm-4">
            <a href='<%="http://"+HttpContext.Current.Request.Url.Authority+ "/SMS/LichSuTinNhan.aspx?sms_type=2" %>'>
                <div class="info-box bg-aqua">
                    <span class="info-box-icon"><i class="ion-ios-chatbubble-outline" style="font-size: 80px; margin-top: 5px;"></i></span>
                    <div class="info-box-content">
                        <span class="info-box-text">Quỹ tin thông báo</span>
                        <span class="info-box-number"><span class="progress-description">
                            <asp:Literal ID="ltrTB" runat="server" Text=""></asp:Literal></span></span>
                        <div class="progress">
                            <div class="progress-bar" id="proQuyTinTB" runat="server"></div>
                        </div>
                        <span class="progress-description">
                            <asp:Label ID="lbDisQuyTinTB" runat="server" class="progress-description"></asp:Label>
                        </span>
                    </div>
                    <!-- /.info-box-content -->
                </div>
            </a>
        </div>
        <div class="col-sm-4">
            <a href='<%="http://"+HttpContext.Current.Request.Url.Authority+ "/HocSinh/HocSinh.aspx" %>'>
                <div class="info-box bg-green">
                    <span class="info-box-icon"><i class="ion ion-ios-heart-outline" style="font-size: 80px; margin-top: 5px;"></i></span>

                    <div class="info-box-content">
                        <span class="info-box-text">Số học sinh</span>
                        <span class="info-box-number">
                            <asp:Label ID="lbSoHocSinh" runat="server" Text="" class="progress-description"></asp:Label></span>

                        <div class="progress">
                            <div class="progress-bar" style="width: 100%" runat="server" id="proSoHS"></div>
                        </div>
                        <span class="progress-description">
                            <asp:Label ID="lbDiscriptionSoHS" runat="server" class="progress-description"></asp:Label></span>
                    </div>
                    <!-- /.info-box-content -->
                </div>
            </a>
        </div>
        <div class="col-sm-4">
            <a href='<%="http://"+HttpContext.Current.Request.Url.Authority+ "/Lop/Lop.aspx" %>'>
                <div class="info-box bg-yellow">
                    <span class="info-box-icon"><i class="ion ion-ios-pricetag-outline" style="font-size: 80px; margin-top: 5px;"></i></span>

                    <div class="info-box-content">
                        <span class="info-box-text">Số lớp</span>
                        <span class="info-box-number">
                            <asp:Label ID="lbSoLop" runat="server" Text="" CssClass="info-box-text"></asp:Label></span>

                        <div class="progress">
                            <div class="progress-bar" style="width: 100%"></div>
                        </div>
                        <span class="progress-description"></span>
                    </div>
                    <!-- /.info-box-content -->
                </div>
            </a>
        </div>
        <div class="col-sm-4">
            <a href='<%="http://"+HttpContext.Current.Request.Url.Authority+ "/GiaoVien/GiaoVien.aspx" %>'>
                <div class="info-box bg-red">
                    <span class="info-box-icon"><i class="ion ion-ios-people-outline" style="font-size: 80px; margin-top: 5px;"></i></span>

                    <div class="info-box-content">
                        <span class="info-box-text">Số giáo viên, nhân viên</span>
                        <span class="info-box-number">
                            <asp:Label ID="lbSoGiaoVien" runat="server" Text="" CssClass="info-box-text"></asp:Label></span>

                        <div class="progress">
                            <div class="progress-bar" style="width: 100%"></div>
                        </div>
                        <span class="progress-description"></span>
                    </div>
                    <!-- /.info-box-content -->
                </div>
            </a>
        </div>
    </div>

    <div class="row" style="margin: 15px">
        <div class="col-sm-4">
            <telerik:RadHtmlChart runat="server" Height="300px" Width="100%" ID="RadHtmlChart2">
                <PlotArea>
                    <Series>
                        <telerik:ColumnSeries DataFieldY="Value" Name="">
                            <TooltipsAppearance Color="White" />
                        </telerik:ColumnSeries>
                    </Series>
                    <XAxis DataLabelsField="Year">
                        <LabelsAppearance RotationAngle="55">
                        </LabelsAppearance>
                        <TitleAppearance Text="Số học sinh có trong khối">
                        </TitleAppearance>
                    </XAxis>
                    <YAxis>
                        <TitleAppearance Text="Số lượng">
                        </TitleAppearance>
                    </YAxis>
                </PlotArea>
                <Legend>
                    <Appearance Visible="false">
                    </Appearance>
                </Legend>
                <ChartTitle Text="">
                </ChartTitle>
            </telerik:RadHtmlChart>
        </div>
        <%--<div class="col-md-4">
            <telerik:RadHtmlChart runat="server" ID="RadHtmlChart1" Height="300px" Width="100%" CssClass="fb-sized">
                <PlotArea>
                    <Series>
                        <telerik:PieSeries DataFieldY="Value" NameField="Name">
                            <LabelsAppearance DataFormatString="{0}">
                            </LabelsAppearance>
                            <TooltipsAppearance Color="White" DataFormatString="{0} tin"></TooltipsAppearance>
                        </telerik:PieSeries>
                        <telerik:ColumnSeries DataFieldY="Value" Name="">
                            <TooltipsAppearance Color="White" />
                        </telerik:ColumnSeries>
                    </Series>
                    <XAxis>
                          <TitleAppearance Text="Quỹ tin thông báo theo tháng"></TitleAppearance>
                    </XAxis>
                </PlotArea>
            </telerik:RadHtmlChart>
        </div>--%>
        <div class="col-md-4">
            <telerik:RadHtmlChart runat="server" ID="RadHtmlChart1" Height="300px" Width="100%" Skin="Silk">
                <PlotArea>
                    <Series>
                        <telerik:AreaSeries Name="Số tin đã dùng theo tháng" DataFieldY="Value">
                            <Appearance>
                                <FillStyle BackgroundColor="Red"></FillStyle>
                            </Appearance>
                            <LabelsAppearance Position="Above"></LabelsAppearance>
                            <LineAppearance Width="1"></LineAppearance>
                            <MarkersAppearance MarkersType="Circle" BackgroundColor="White" Size="5" BorderColor="Blue"
                                BorderWidth="2"></MarkersAppearance>
                            <TooltipsAppearance Color="White"></TooltipsAppearance>
                        </telerik:AreaSeries>
                    </Series>
                    <Appearance>
                        <FillStyle BackgroundColor="Transparent"></FillStyle>
                    </Appearance>
                    <XAxis AxisCrossingValue="0" Color="black" MajorTickType="Outside" MinorTickType="Outside" Reversed="false" DataLabelsField="Year">
                        <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1">
                        </LabelsAppearance>
                    </XAxis>
                    <YAxis AxisCrossingValue="0" Color="black" MajorTickSize="4" MajorTickType="Outside"
                        MinorTickType="None" MinValue="0" Reversed="false"
                        Step="5000">
                        <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1">
                        </LabelsAppearance>
                        <TitleAppearance RotationAngle="0" Position="Center" Text="Số lượng"></TitleAppearance>
                    </YAxis>
                </PlotArea>
                <Appearance>
                    <FillStyle BackgroundColor="Transparent"></FillStyle>
                </Appearance>
                <Legend>
                    <Appearance BackgroundColor="Transparent" Position="Bottom">
                    </Appearance>
                </Legend>
            </telerik:RadHtmlChart>
        </div>
        <div class="col-md-4">
            <telerik:RadHtmlChart runat="server" ID="RadHtmlChart3" Width="100%" Height="300px">
                <PlotArea>
                    <Series>
                        <telerik:BarSeries DataFieldY="Value" Name="Drinks">
                            <TooltipsAppearance Visible="false"></TooltipsAppearance>
                        </telerik:BarSeries>
                    </Series>
                    <XAxis DataLabelsField="Year">
                        <MinorGridLines Visible="false"></MinorGridLines>
                        <MajorGridLines Visible="false"></MajorGridLines>
                    </XAxis>
                    <YAxis>
                        <TitleAppearance Text="Tổng số học sinh theo từng năm"></TitleAppearance>
                        <MinorGridLines Visible="false"></MinorGridLines>
                    </YAxis>
                </PlotArea>
                <Legend>
                    <Appearance Visible="false"></Appearance>
                </Legend>
                <%-- <ChartTitle Text="Units In Stock">
                </ChartTitle>--%>
            </telerik:RadHtmlChart>
        </div>

    </div>

</asp:Content>
