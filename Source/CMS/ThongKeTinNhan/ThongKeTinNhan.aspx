<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterSYS.Master" AutoEventWireup="true" CodeBehind="ThongKeTinNhan.aspx.cs" Inherits="CMS.SMS.ThongKeTinNhan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
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
            <telerik:AjaxSetting AjaxControlID="rcbTinhThanh">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbQuanHuyen" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="rcbTruong" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbQuanHuyen">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbTruong" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbTruong">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbThang">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true" OnClientClose="RadWindowManager1_OnClientClose">
        <Windows>
            <telerik:RadWindow RenderMode="Lightweight" ID="RadWindow1" runat="server" ShowContentDuringLoad="false" Width="400px"
                Height="400px" Title="Thống kê quỹ tin" Behaviors="Reload,Close,Maximize" Modal="true" VisibleStatusbar="false">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadCodeBlock runat="server">
        <script>
            var grid;
            function pageLoad() {
                grid = $find("<%=RadGrid1.ClientID%>");
            }
            function RadWindowManager1_OnClientClose(sender, args) {
                var ajaxManager = $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("RadWindowManager1_Close");
                var arg = args.get_argument();
                if (arg != null) {
                    refreshGrid();
                    notification('success', arg);
                }
            }
            function refreshGrid() {
                var masterTable = grid.get_masterTableView();
                masterTable.rebind();
            }
        </script>
    </telerik:RadCodeBlock>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">THỐNG KÊ TIN NHẮN</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btExport" runat="server" CssClass="btn bt-one" OnClick="btExport_Click" Text="Xuất excel" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="row">
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbTinhThanh" runat="server" Width="100%" MaxHeight="500px" DataSourceID="objTinhThanh" DataTextField="TEN" DataValueField="MA" AutoPostBack="True" EmptyMessage="Chọn tỉnh thành" AllowCustomText="true" Filter="Contains" OnSelectedIndexChanged="rcbTinhThanh_SelectedIndexChanged">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objTinhThanh" runat="server" SelectMethod="getTinhThanh" TypeName="OneEduDataAccess.BO.DmTinhThanhBO">
                        <SelectParameters>
                            <asp:Parameter Name="is_all" DbType="Boolean" DefaultValue="False" />
                            <asp:Parameter Name="id_all" DbType="Int16" DefaultValue="0" />
                            <asp:Parameter Name="text_all" DbType="String" DefaultValue="Chọn tất cả" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbQuanHuyen" runat="server" Width="100%" MaxHeight="500px" DataSourceID="objQuanHuyen" DataTextField="TEN" DataValueField="MA" AutoPostBack="True" EmptyMessage="Chọn quận/huyện" AllowCustomText="true" Filter="Contains" OnSelectedIndexChanged="rcbQuanHuyen_SelectedIndexChanged">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objQuanHuyen" runat="server" SelectMethod="getQuanHuyenByTinh" TypeName="OneEduDataAccess.BO.DmQuanHuyenBO">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="rcbTinhThanh" Name="ma_tinh" PropertyName="SelectedValue" />
                            <asp:Parameter Name="is_all" DbType="Boolean" DefaultValue="False" />
                            <asp:Parameter Name="id_all" DbType="Int16" DefaultValue="0" />
                            <asp:Parameter Name="text_all" DbType="String" DefaultValue="Chọn tất cả" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbTruong" runat="server" Width="100%" MaxHeight="500px" DataSourceID="objTruong" DataTextField="TEN" DataValueField="ID" AutoPostBack="True" EmptyMessage="Chọn trường" AllowCustomText="true" Filter="Contains" OnSelectedIndexChanged="rcbTruong_SelectedIndexChanged">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objTruong" runat="server" SelectMethod="getTruongByTinhHuyen" TypeName="OneEduDataAccess.BO.TruongBO">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="rcbTinhThanh" Name="ma_tinh" PropertyName="SelectedValue" />
                            <asp:ControlParameter ControlID="rcbQuanHuyen" Name="ma_huyen" PropertyName="SelectedValue" />
                            <asp:Parameter Name="is_all" DbType="Boolean" DefaultValue="False" />
                            <asp:Parameter Name="id_all" DbType="Int16" DefaultValue="0" />
                            <asp:Parameter Name="text_all" DbType="String" DefaultValue="Chọn tất cả" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbThang" runat="server" Width="100%" DataTextField="TEN" DataValueField="ID" AutoPostBack="True" EmptyMessage="Chọn Tháng" AllowCustomText="true" Filter="Contains" OnSelectedIndexChanged="rcbThang_SelectedIndexChanged">
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
            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" GroupPanelPosition="Top" OnNeedDataSource="RadGrid1_NeedDataSource" OnItemDataBound="RadGrid1_ItemDataBound"
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
                        <telerik:GridColumnGroup HeaderText="Tin nhắn liên lạc (/tháng)" HeaderStyle-HorizontalAlign="Center" Name="TIN_LL"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Tin nhắn thông báo (/tháng)" HeaderStyle-HorizontalAlign="Center" Name="TIN_TB"></telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="STT" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%# Container.DataSetIndex+1 %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="TEN" FilterControlAltText="Filter TEN column" HeaderText="Tên" SortExpression="TEN" UniqueName="TEN" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="300px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TONG_CAP_LL" ColumnGroupName="TIN_LL" FilterControlAltText="Filter TONG_CAP_LL column" HeaderText="Tổng cấp" SortExpression="TONG_CAP_LL" UniqueName="TONG_CAP_LL" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TONG_THEM_LL" ColumnGroupName="TIN_LL" FilterControlAltText="Filter TONG_THEM_LL column" HeaderText="Tổng thêm" SortExpression="TONG_THEM_LL" UniqueName="TONG_THEM_LL" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TONG_DA_GUI_LL" ColumnGroupName="TIN_LL" FilterControlAltText="Filter TONG_DA_GUI_LL column" HeaderText="Tổng gửi" SortExpression="TONG_DA_GUI_LL" UniqueName="TONG_DA_GUI_LL" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="PHAN_TRAM_LL" ColumnGroupName="TIN_LL" FilterControlAltText="Filter PHAN_TRAM_LL column" HeaderText="Sản lượng sử dụng (%)" SortExpression="PHAN_TRAM_LL" UniqueName="PHAN_TRAM_LL" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:###,##0.##}">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TONG_CAP_TB" ColumnGroupName="TIN_TB" FilterControlAltText="Filter TONG_CAP_TB column" HeaderText="Tổng cấp" SortExpression="TONG_CAP_TB" UniqueName="TONG_CAP_TB" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TONG_THEM_TB" ColumnGroupName="TIN_TB" FilterControlAltText="Filter TONG_THEM_TB column" HeaderText="Tổng thêm" SortExpression="TONG_THEM_TB" UniqueName="TONG_THEM_TB" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TONG_DA_GUI_TB" ColumnGroupName="TIN_TB" FilterControlAltText="Filter TONG_DA_GUI_TB column" HeaderText="Tổng gửi" SortExpression="TONG_DA_GUI_TB" UniqueName="TONG_DA_GUI_TB" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="PHAN_TRAM_TB" ColumnGroupName="TIN_TB" FilterControlAltText="Filter PHAN_TRAM_TB column" HeaderText="Sản lượng sử dụng (%)" SortExpression="PHAN_TRAM_TB" UniqueName="PHAN_TRAM_TB" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:###,##0.##}">
                        </telerik:GridBoundColumn>
                    </Columns>
                    <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
