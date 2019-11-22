<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterSYS.Master" AutoEventWireup="true" CodeBehind="Truong.aspx.cs" Inherits="CMS.Truong.Truong" %>
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
            <telerik:AjaxSetting AjaxControlID="rcbCapHoc">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbTrangThai">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btDeleteChon">
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
                Height="400px" Title="Chi tiết trường" Behaviors="Reload,Close,Maximize" Modal="true" VisibleStatusbar="false">
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
            function btDeteleClick() {
                if (grid.get_masterTableView().get_selectedItems().length > 0) {
                    if (confirm("Bạn chắc chắn muốn xóa?")) {
                        return true;
                    }
                } else {
                    alert("Mời bạn chọn thông tin xóa.");
                    return false;
                }
            }
        </script>
    </telerik:RadCodeBlock>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">Quản lý trường</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btTimKiem" runat="server" Text="Tìm kiếm" CssClass="btn bt-one" OnClick="btTimKiem_Click"/>
                <input type="button" id="btAddNew" runat="server" class="btn bt-one" value="Thêm mới" onclick="openRadWin('TruongDetail.aspx', 100, 50)" />
                <asp:Button ID="btDeleteChon" runat="server" CssClass="btn bt-one" OnClick="btDeleteChon_Click" Text="Xóa trường" OnClientClick="if(!btDeteleClick()) return false;" />
                <asp:LinkButton runat="server" id="btnImportExcel" href="\Truong\TruongImportExcel.aspx" CssClass="btn bt-one" Font-Size="17px">Import Excel</asp:LinkButton>
                <asp:Button ID="btExport" runat="server" CssClass="btn bt-one" OnClick="btExport_Click" Text="Xuất excel" />
                <input type="button" id="btnGuiTin" runat="server" class="btn bt-one" value="Gửi tin" onclick="openRadWin('GuiTinNhanThuongHieu.aspx', 900, 500, 1)" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="row">
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbCapHoc" runat="server" Width="100%" EmptyMessage="Chọn cấp học" AllowCustomText="true" Filter="Contains" OnSelectedIndexChanged="rcbCapHoc_SelectedIndexChanged" AutoPostBack="true" >
                        <Items>
                            <telerik:RadComboBoxItem Value="" Text="Tất cả" />
                            <telerik:RadComboBoxItem Value="MN" Text="Mầm non" />
                            <telerik:RadComboBoxItem Value="TH" Text="Tiểu học" />
                            <telerik:RadComboBoxItem Value="THCS" Text="THCS" />
                            <telerik:RadComboBoxItem Value="THPT" Text="THPT" />
                            <telerik:RadComboBoxItem Value="GDTX" Text="GDTX" />
                        </Items>
                    </telerik:RadComboBox>
                </div>
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbTrangThai" runat="server" Width="100%" EmptyMessage="Trạng thái trường học" AllowCustomText="true" Filter="Contains" OnSelectedIndexChanged="rcbTrangThai_SelectedIndexChanged" AutoPostBack="true">
                        <Items>
                            <telerik:RadComboBoxItem Value="" Text="Tất cả" />
                            <telerik:RadComboBoxItem Value="1" Text="Hoạt động" />
                            <telerik:RadComboBoxItem Value="0" Text="Dừng hoạt động" />
                        </Items>
                    </telerik:RadComboBox>
                </div>
                <div class="col-sm-3">
                    <telerik:RadTextBox ID="rtbTenTruong" runat="server" EmptyMessage="Nhập tên trường cần tìm kiếm" Width="100%"></telerik:RadTextBox>
                </div>
                <div class="col-sm-3">
                    <telerik:RadTextBox ID="rtbMa" runat="server" EmptyMessage="Nhập mã trường cần tìm kiếm" Width="100%"></telerik:RadTextBox>
                </div>
            </div>
        </div>
        <div class="item-data">
            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" GroupPanelPosition="Top" OnNeedDataSource="RadGrid1_NeedDataSource"
                AutoGenerateColumns="False" PageSize="50" AllowMultiRowSelection="True" OnItemDataBound="RadGrid1_ItemDataBound">
                <ClientSettings>
                    <Selecting AllowRowSelect="true"></Selecting>
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                </ClientSettings>
                <MasterTableView DataKeyNames="ID" ClientDataKeyNames="ID">
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
                        <telerik:GridClientSelectColumn UniqueName="chkChon" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridClientSelectColumn>
                        <telerik:GridTemplateColumn HeaderText="STT" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%# Container.DataSetIndex+1 %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="MA" HeaderStyle-Width="160px" FilterControlAltText="Filter MA column" HeaderText="Mã trường" SortExpression="MA" UniqueName="MA" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TEN" FilterControlAltText="Filter TEN column" HeaderText="Tên trường" SortExpression="TEN" UniqueName="TEN" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="BRAND_NAME_VIETTEL" FilterControlAltText="Filter BRAND_NAME_VIETTEL column" HeaderText="Tên thương hiệu" SortExpression="BRAND_NAME_VIETTEL" UniqueName="BRAND_NAME_VIETTEL" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                        </telerik:GridBoundColumn>
                        <telerik:GridCheckBoxColumn DataField="IS_MN" FilterControlAltText="Filter IS_MN column" HeaderText="Cấp MN" SortExpression="IS_TH" UniqueName="IS_MN" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IS_TH" FilterControlAltText="Filter IS_TH column" HeaderText="Cấp TH" SortExpression="IS_TH" UniqueName="IS_TH" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IS_THCS" FilterControlAltText="Filter IS_THCS column" HeaderText="Cấp THCS" SortExpression="IS_THCS" UniqueName="IS_THCS" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IS_THPT" FilterControlAltText="Filter IS_THPT column" HeaderText="Cấp THPT" SortExpression="IS_THPT" UniqueName="IS_THPT" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IS_GDTX" FilterControlAltText="Filter IS_GDTX column" HeaderText="Cấp GDTX" SortExpression="IS_GDTX" UniqueName="IS_GDTX" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridCheckBoxColumn>
                        <telerik:GridBoundColumn DataField="TRANG_THAI" FilterControlAltText="Filter TRANG_THAI column" HeaderText="Trạng thái" SortExpression="TRANG_THAI" UniqueName="TRANG_THAI" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Sửa" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="SUA">
                            <ItemTemplate>
                                <img src="../img/sua.png" onclick='<%# "openRadWin(\"TruongDetail.aspx?id_hoso=" + Eval("ID") + "\",100,50)" %>' style="cursor: pointer;" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizes="10,20,50,100,200" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
