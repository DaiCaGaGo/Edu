<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterSYS.Master" AutoEventWireup="true" CodeBehind="ThongKeTinNhanTheoCTV.aspx.cs" Inherits="CMS.ThongKeTinNhan.ThongKeTinNhanTheoCTV" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rcbNam">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadHtmlChart1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbThang">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadHtmlChart1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-6">
                <span class="item-title">THỐNG KÊ TIN NHẮN THEO CTV</span>
            </div>
            <div class="col-sm-6 text-right">
                <asp:Button ID="btExport" runat="server" CssClass="btn bt-one" OnClick="btExport_Click" Text="Xuất excel" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="row">
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbNam" runat="server" Width="100%" Filter="Contains" OnSelectedIndexChanged="rcbNam_SelectedIndexChanged" AutoPostBack="true">
                        <Items>
                            <telerik:RadComboBoxItem Value="2018" Text="2018" />
                            <telerik:RadComboBoxItem Value="2019" Text="2019" />
                            <telerik:RadComboBoxItem Value="2020" Text="2020" />
                            <telerik:RadComboBoxItem Value="2021" Text="2021" />
                            <telerik:RadComboBoxItem Value="2022" Text="2022" />
                            <telerik:RadComboBoxItem Value="2023" Text="2023" />
                            <telerik:RadComboBoxItem Value="2024" Text="2024" />
                            <telerik:RadComboBoxItem Value="2025" Text="2025" />
                            <telerik:RadComboBoxItem Value="2026" Text="2026" />
                            <telerik:RadComboBoxItem Value="2027" Text="2027" />
                            <telerik:RadComboBoxItem Value="2028" Text="2028" />
                            <telerik:RadComboBoxItem Value="2029" Text="2029" />
                        </Items>
                    </telerik:RadComboBox>
                </div>
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbThang" runat="server" Width="100%" Filter="Contains" OnSelectedIndexChanged="rcbThang_SelectedIndexChanged" AutoPostBack="true">
                        <Items>
                            <telerik:RadComboBoxItem Value="1" Text="Tháng 1" />
                            <telerik:RadComboBoxItem Value="2" Text="Tháng 2" />
                            <telerik:RadComboBoxItem Value="3" Text="Tháng 3" />
                            <telerik:RadComboBoxItem Value="4" Text="Tháng 4" />
                            <telerik:RadComboBoxItem Value="5" Text="Tháng 5" />
                            <telerik:RadComboBoxItem Value="6" Text="Tháng 6" />
                            <telerik:RadComboBoxItem Value="7" Text="Tháng 7" />
                            <telerik:RadComboBoxItem Value="8" Text="Tháng 8" />
                            <telerik:RadComboBoxItem Value="9" Text="Tháng 9" />
                            <telerik:RadComboBoxItem Value="10" Text="Tháng 10" />
                            <telerik:RadComboBoxItem Value="11" Text="Tháng 11" />
                            <telerik:RadComboBoxItem Value="12" Text="Tháng 12" />
                        </Items>
                    </telerik:RadComboBox>
                </div>
            </div>
        </div>
        <div class="item-data">
            <div class="row">
                <div class="col-sm-5">
                    <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" GroupPanelPosition="Top" OnNeedDataSource="RadGrid1_NeedDataSource"
                        AutoGenerateColumns="False" PageSize="100" AllowMultiRowSelection="True">
                        <ClientSettings>
                            <Selecting AllowRowSelect="true"></Selecting>
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                        </ClientSettings>
                        <MasterTableView DataKeyNames="NGUOI_GUI" ClientDataKeyNames="NGUOI_GUI" AllowMultiColumnSorting="true" AllowSorting="true">
                            <NoRecordsTemplate>
                                <div style="padding: 20px 10px;">
                                    Không có bản ghi nào!
                                </div>
                            </NoRecordsTemplate>
                            <CommandItemSettings AddNewRecordText="Thêm mới bản ghi" CancelChangesText="Hủy"
                                RefreshText="Làm mới" SaveChangesText="Lưu thay đổi" ShowAddNewRecordButton="true" />
                            <EditFormSettings>
                                <PopUpSettings />
                                <EditColumn UpdateText="Lưu" CancelText="Hủy" InsertText="Thêm" ButtonType="PushButton"></EditColumn>
                            </EditFormSettings>
                            <HeaderStyle CssClass="head-list-grid" />
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="STT" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%# Container.DataSetIndex+1 %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="NGUOI_GUI" FilterControlAltText="Filter NGUOI_GUI column" HeaderText="NGUOI_GUI" SortExpression="NGUOI_GUI" UniqueName="NGUOI_GUI" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TEN_DANG_NHAP" FilterControlAltText="Filter TEN_DANG_NHAP column" HeaderText="Tài khoản" SortExpression="TEN_DANG_NHAP" UniqueName="TEN_DANG_NHAP" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="300px">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="SO_TIN" ColumnGroupName="SO_TIN" FilterControlAltText="Filter SO_TIN column" HeaderText="Số tin" SortExpression="SO_TIN" UniqueName="SO_TIN" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="SO_NGAY_GUI" ColumnGroupName="SO_NGAY_GUI" FilterControlAltText="Filter SO_NGAY_GUI column" HeaderText="Số ngày gửi" SortExpression="SO_NGAY_GUI" UniqueName="SO_NGAY_GUI" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TRUNG_BINH" ColumnGroupName="TRUNG_BINH" FilterControlAltText="Filter TRUNG_BINH column" HeaderText="Trung bình (tin/ngày)" SortExpression="TRUNG_BINH" UniqueName="TRUNG_BINH" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                </telerik:GridBoundColumn>
                            </Columns>
                            <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
                <div class="col-sm-7">
                    <telerik:RadHtmlChart runat="server" ID="RadHtmlChart1" Height="400px" Width="100%">
                        <PlotArea>
                            <Series>
                                <telerik:ColumnSeries DataFieldY="SO_TIN" Name="Tổng tin">
                                    <TooltipsAppearance Color="White" />
                                </telerik:ColumnSeries>
                                <telerik:ColumnSeries DataFieldY="TRUNG_BINH" Name="Trung bình (tin/ngày)">
                                    <TooltipsAppearance Color="White" />
                                </telerik:ColumnSeries>
                            </Series>
                            <XAxis DataLabelsField="TEN_DANG_NHAP">
                                <TitleAppearance Text="Tài khoản">
                                    <TextStyle Margin="20" />
                                </TitleAppearance>
                                <MajorGridLines Visible="false" />
                                <MinorGridLines Visible="false" />
                            </XAxis>
                            <YAxis>
                                <TitleAppearance Text="Số tin">
                                    <TextStyle Margin="20" />
                                </TitleAppearance>
                                <MinorGridLines Visible="false" />
                            </YAxis>
                        </PlotArea>
                        <ChartTitle Text="Biểu đồ thống kê tin nhắn theo tài khoản">
                        </ChartTitle>
                    </telerik:RadHtmlChart>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
