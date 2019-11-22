<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterSYS.Master" AutoEventWireup="true" CodeBehind="DoiSoatTinTheoNgay.aspx.cs" Inherits="CMS.ThongKeTinNhan.DoiSoatTinTheoNgay" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbTruong">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting> 
            <telerik:AjaxSetting AjaxControlID="rdTuNgay">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rdDenNgay">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">ĐỐI SOÁT TIN NHẮN THEO NGÀY</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btExport" runat="server" CssClass="btn bt-one" OnClick="btExport_Click" Text="Xuất excel" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="row">
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbTruong" runat="server" Width="100%" DataTextField="TEN" DataValueField="ID" Filter="Contains" EmptyMessage="Chọn trường" AllowCustomText="true" OnSelectedIndexChanged="rcbTruong_SelectedIndexChanged" AutoPostBack="true">
                        </telerik:RadComboBox>
                </div>
                <div class="col-sm-6">
                    <telerik:RadDatePicker Culture="vi-VN" RenderMode="Classic" ID="rdTuNgay" runat="server" Width="48%" MinDate="1900/1/1"
                        Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="Giá trị không hợp lệ"
                        Calendar-FastNavigationSettings-TodayButtonCaption="Hôm nay"
                        DatePopupButton-ToolTip="Ngày bắt đầu"
                        Calendar-FastNavigationSettings-OkButtonCaption="Chọn"
                        Calendar-FastNavigationSettings-CancelButtonCaption="Hủy"
                        OnSelectedDateChanged="rdTuNgay_SelectedDateChanged" AutoPostBack="true">
                        <DateInput DateFormat="dd/MM/yyyy"></DateInput>
                    </telerik:RadDatePicker>
                    &nbsp;--&nbsp;
                    <telerik:RadDatePicker Culture="vi-VN" RenderMode="Classic" ID="rdDenNgay" runat="server" Width="48%" MinDate="1900/1/1"
                        Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="Giá trị không hợp lệ"
                        Calendar-FastNavigationSettings-TodayButtonCaption="Hôm nay"
                        DatePopupButton-ToolTip="Ngày kết thúc"
                        Calendar-FastNavigationSettings-OkButtonCaption="Chọn"
                        Calendar-FastNavigationSettings-CancelButtonCaption="Hủy" OnSelectedDateChanged="rdDenNgay_SelectedDateChanged" AutoPostBack="true">
                        <DateInput DateFormat="dd/MM/yyyy"></DateInput>
                    </telerik:RadDatePicker>
                </div>
            </div>
        </div>
        <div class="item-data">
            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" GroupPanelPosition="Top" OnNeedDataSource="RadGrid1_NeedDataSource" AutoGenerateColumns="False" PageSize="100" AllowMultiRowSelection="True" OnPreRender="RadGrid1_PreRender">
                <ClientSettings>
                    <Selecting AllowRowSelect="true"></Selecting>
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                </ClientSettings>
                <MasterTableView>
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
                        <telerik:GridBoundColumn DataField="ID_TRUONG" FilterControlAltText="Filter ID_TRUONG column" HeaderText="ID_TRUONG" SortExpression="ID_TRUONG" UniqueName="ID_TRUONG" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TEN_TRUONG" FilterControlAltText="Filter TEN_TRUONG column" HeaderText="Trường" SortExpression="TEN_TRUONG" UniqueName="TEN_TRUONG" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="BRAND_NAME" FilterControlAltText="Filter BRAND_NAME column" HeaderText="Thương hiệu" SortExpression="BRAND_NAME" UniqueName="BRAND_NAME" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CP" FilterControlAltText="Filter CP column" HeaderText="Đối tác" SortExpression="CP" UniqueName="CP" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="LOAI_NHA_MANG" FilterControlAltText="Filter LOAI_NHA_MANG column" HeaderText="Loại nhà mạng" SortExpression="LOAI_NHA_MANG" UniqueName="LOAI_NHA_MANG" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="300px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SO_TIN" FilterControlAltText="Filter SO_TIN column" HeaderText="Số tin" SortExpression="SO_TIN" UniqueName="SO_TIN" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                    </Columns>
                    <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>