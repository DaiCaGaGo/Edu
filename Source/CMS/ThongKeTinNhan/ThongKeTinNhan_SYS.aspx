<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterSYS.Master" AutoEventWireup="true" CodeBehind="ThongKeTinNhan_SYS.aspx.cs" Inherits="CMS.ThongKeTinNhan.ThongKeTinNhan_SYS" %>

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
                    <telerik:AjaxUpdatedControl ControlID="rcbCapHoc" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="divLoaiLopGDTX" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="rcbKhoi" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="rcbLop" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbCapHoc">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divLoaiLopGDTX" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="rcbKhoi"  UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="rcbLop" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbLoaiLopGDTX">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbKhoi" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="rcbLop" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbKhoi">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbLop" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbLop">
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
                <span class="item-title">THỐNG KÊ TIN NHẮN THEO LỚP</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btExport" runat="server" CssClass="btn bt-one" OnClick="btExport_Click" Text="Xuất excel" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="row">
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbTruong" runat="server" Width="100%" DataTextField="TEN" DataValueField="ID" Filter="Contains" OnSelectedIndexChanged="rcbTruong_SelectedIndexChanged" AutoPostBack="true">
                    </telerik:RadComboBox>
                </div>
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbCapHoc" runat="server" Width="100%" Filter="Contains" OnSelectedIndexChanged="rcbCapHoc_SelectedIndexChanged" AutoPostBack="true">
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
            <div class="row" style="margin-top: 20px;">
                <div class="col-sm-3" runat="server" id="divLoaiLopGDTX">
                    <telerik:RadComboBox ID="rcbLoaiLopGDTX" runat="server" Width="100%" DataTextField="TEN" DataValueField="MA" Filter="Contains" OnSelectedIndexChanged="rcbLoaiLopGDTX_SelectedIndexChanged" AutoPostBack="true">
                    </telerik:RadComboBox>
                </div>
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbKhoi" runat="server" Width="100%" DataSourceID="objKhoi" DataTextField="TEN" DataValueField="MA" EmptyMessage="Chọn khối" Filter="Contains" AllowCustomText="true" OnSelectedIndexChanged="rcbKhoi_SelectedIndexChanged" AutoPostBack="true">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objKhoi" runat="server" SelectMethod="getKhoiByCapHoc" TypeName="OneEduDataAccess.BO.KhoiBO">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="rcbCapHoc" Name="cap_hoc" PropertyName="SelectedValue"/>
                            <asp:ControlParameter ControlID="rcbLoaiLopGDTX" Name="maLoaiLopGDTX" PropertyName="SelectedValue"/>
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbLop" runat="server" Width="100%" DataSourceID="objLop"
                        DataTextField="TEN" DataValueField="ID" EmptyMessage="Chọn lớp" Filter="Contains" AllowCustomText="true" OnSelectedIndexChanged="rcbLop_SelectedIndexChanged" AutoPostBack="true">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objLop" runat="server" SelectMethod="getLopByKhoiNamHoc" TypeName="OneEduDataAccess.BO.LopBO">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="rcbTruong" Name="idTruong" PropertyName="SelectedValue" />
                            <asp:ControlParameter ControlID="rcbCapHoc" Name="ma_cap_hoc" PropertyName="SelectedValue" />
                            <asp:ControlParameter ControlID="rcbKhoi" Name="maKhoi" PropertyName="SelectedValue" />
                            <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                            <asp:Parameter DefaultValue="" Name="id_all" Type="Int64" />
                            <asp:Parameter Name="text_all" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
            </div>
        </div>
        <div class="item-data">
            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" GroupPanelPosition="Top" OnNeedDataSource="RadGrid1_NeedDataSource"
                AutoGenerateColumns="False" PageSize="100" AllowMultiRowSelection="True">
                <ClientSettings>
                    <Selecting AllowRowSelect="true"></Selecting>
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                </ClientSettings>
                <MasterTableView DataKeyNames="ID_TRUONG" ClientDataKeyNames="ID_TRUONG">
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
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Tin nhắn liên lạc" HeaderStyle-HorizontalAlign="Center" Name="TIN_LL"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Tin nhắn thông báo" HeaderStyle-HorizontalAlign="Center" Name="TIN_TB"></telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="STT" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%# Container.DataSetIndex+1 %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="ID_TRUONG" FilterControlAltText="Filter ID_TRUONG column" HeaderText="ID_TRUONG" SortExpression="ID_TRUONG" UniqueName="ID_TRUONG" HeaderStyle-HorizontalAlign="Center" Visible="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TEN_LOP" FilterControlAltText="Filter TEN_LOP column" HeaderText="Tên lớp" SortExpression="TEN_LOP" UniqueName="TEN_LOP" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="300px">
                        </telerik:GridBoundColumn>
                        <%-- tin liên lạc--%>
                        <telerik:GridBoundColumn DataField="TONG_TIN_LL" ColumnGroupName="TIN_LL" FilterControlAltText="Filter TONG_TIN_LL column" HeaderText="Tổng số" SortExpression="TONG_TIN_LL" UniqueName="TONG_TIN_LL" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TIN_LL_TC" ColumnGroupName="TIN_LL" FilterControlAltText="Filter TIN_LL_TC column" HeaderText="Thành công" SortExpression="TIN_LL_TC" UniqueName="TIN_LL_TC" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TIN_LL_CG" ColumnGroupName="TIN_LL" FilterControlAltText="Filter TIN_LL_CG column" HeaderText="Chờ gửi" SortExpression="TIN_LL_CG" UniqueName="TIN_LL_CG" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TIN_LL_GL" ColumnGroupName="TIN_LL" FilterControlAltText="Filter TIN_LL_GL column" HeaderText="Gửi lại" SortExpression="TIN_LL_GL" UniqueName="TIN_LL_GL" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TIN_LL_DG" ColumnGroupName="TIN_LL" FilterControlAltText="Filter TIN_LL_DG column" HeaderText="Dừng gửi" SortExpression="TIN_LL_DG" UniqueName="TIN_LL_DG" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <%-- End tin liên lạc--%>
                        <%-- Tin thông báo--%>
                        <telerik:GridBoundColumn DataField="TONG_TIN_TB" ColumnGroupName="TIN_TB" FilterControlAltText="Filter TONG_TIN_TB column" HeaderText="Tổng số" SortExpression="TONG_TIN_TB" UniqueName="TONG_TIN_TB" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TIN_TB_TC" ColumnGroupName="TIN_TB" FilterControlAltText="Filter TIN_TB_TC column" HeaderText="Thành công" SortExpression="TIN_TB_TC" UniqueName="TIN_TB_TC" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TIN_TB_CG" ColumnGroupName="TIN_TB" FilterControlAltText="Filter TIN_TB_CG column" HeaderText="Chờ gửi" SortExpression="TIN_TB_CG" UniqueName="TIN_TB_CG" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TIN_TB_GL" ColumnGroupName="TIN_TB" FilterControlAltText="Filter TIN_TB_GL column" HeaderText="Gửi lại" SortExpression="TIN_TB_GL" UniqueName="TIN_TB_GL" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TIN_TB_DG" ColumnGroupName="TIN_TB" FilterControlAltText="Filter TIN_TB_DG column" HeaderText="Dừng gửi" SortExpression="TIN_TB_DG" UniqueName="TIN_TB_DG" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <%-- End tin thông báo--%>
                    </Columns>
                    <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
