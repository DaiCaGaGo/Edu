<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterSYS.Master" AutoEventWireup="true" CodeBehind="ThongKeTinNhanTheoThuongHieu.aspx.cs" Inherits="CMS.ThongKeTinNhan.ThongKeTinNhanTheoThuongHieu" %>
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
            <telerik:AjaxSetting AjaxControlID="rcbThuongHieu">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting> 
            <telerik:AjaxSetting AjaxControlID="rcbTelCo">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbTelCo" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbCP">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbCP" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
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
                <span class="item-title">THỐNG KÊ TIN NHẮN THEO ĐỐI TÁC</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btExport" runat="server" CssClass="btn bt-one" OnClick="btExport_Click" Text="Xuất excel" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="row">
                <div class="col-sm-2">
                    <telerik:RadComboBox ID="rcbThuongHieu" runat="server" Width="100%" DataSourceID="objThuongHieu" DataTextField="BRAND_NAME" DataValueField="BRAND_NAME" Filter="Contains" OnSelectedIndexChanged="rcbThuongHieu_SelectedIndexChanged" EmptyMessage="Chọn thương hiệu" AllowCustomText="true" AutoPostBack="true">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objThuongHieu" runat="server" SelectMethod="getThuongHieu" TypeName="OneEduDataAccess.BO.ThuongHieuBO">
                        <SelectParameters>
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
                <div class="col-sm-2">
                    <telerik:RadComboBox ID="rcbTelCo" runat="server" Width="100%" Filter="Contains" OnSelectedIndexChanged="rcbTelCo_SelectedIndexChanged" EmptyMessage="Chọn nhà mạng" AllowCustomText="true" AutoPostBack="true">
                        <Items>
                            <telerik:RadComboBoxItem Value="Viettel" Text="Viettel" />
                            <telerik:RadComboBoxItem Value="MobiFone" Text="MobiFone" />
                            <telerik:RadComboBoxItem Value="VinaPhone" Text="VinaPhone" />
                            <telerik:RadComboBoxItem Value="VietnamMobile" Text="VietnamMobile" />
                            <telerik:RadComboBoxItem Value="GMobile" Text="GMobile" />
                        </Items>
                    </telerik:RadComboBox>

                </div>
                <div class="col-sm-2">
                    <telerik:RadComboBox ID="rcbCP" runat="server" Width="100%" DataSourceID="objDoiTac" DataTextField="TEN" DataValueField="MA" EmptyMessage="Chọn đối tác" AllowCustomText="true" Filter="Contains" OnSelectedIndexChanged="rcbCP_SelectedIndexChanged" AutoPostBack="true">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objDoiTac" runat="server" SelectMethod="getTelco" TypeName="OneEduDataAccess.BO.CPTelCoBO">
                        <SelectParameters>
                            <asp:Parameter Name="is_all" DefaultValue="false" DbType="Boolean" />
                            <asp:Parameter Name="id_all" DefaultValue="" DbType="String" />
                            <asp:Parameter Name="text_all" DefaultValue="" DbType="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
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
            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" GroupPanelPosition="Top" OnNeedDataSource="RadGrid1_NeedDataSource" OnItemDataBound="RadGrid1_ItemDataBound"
                AutoGenerateColumns="False" PageSize="100" AllowMultiRowSelection="True" OnPreRender="RadGrid1_PreRender">
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