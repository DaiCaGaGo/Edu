<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="KhoThucPham.aspx.cs" Inherits="CMS.DinhDuong.KhoThucPham" %>
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
                Height="400px" Title="Chi tiết thực phẩm" Behaviors="Reload,Close,Maximize" Modal="true" VisibleStatusbar="false">
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
                <span class="item-title">danh mục thực phẩm</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btTimKiem" runat="server" Text="Tìm kiếm" CssClass="btn bt-one" OnClick="btTimKiem_Click" />
                <input type="button" id="btnNhapKho" runat="server" class="btn bt-one" value="Nhập kho" onclick="openRadWin('PhieuNhapKho.aspx', 100, 50)" />
                <input type="button" id="btnXuatKho" runat="server" class="btn bt-one" value="Xuất kho" onclick="openRadWin('PhieuXuatKho.aspx', 100, 50)" />
                <asp:Button ID="btDeleteChon" runat="server" CssClass="btn bt-one" OnClick="btDeleteChon_Click" Text="Xóa" OnClientClick="if(!btDeteleClick()) return false;" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="row">
                <div class="col-sm-4">
                    <telerik:RadComboBox ID="rcbNhomThucPham" runat="server" Width="100%" DataSourceID="objNhomThucPham"
                        DataTextField="TEN" DataValueField="ID" EmptyMessage="Chọn nhóm thực phẩm" AllowCustomText="true" Filter="Contains" OnSelectedIndexChanged="rcbNhomThucPham_SelectedIndexChanged" AutoPostBack="true">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objNhomThucPham" runat="server" SelectMethod="getNhomThucPham" TypeName="OneEduDataAccess.BO.NhomThucPhamBO">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="" Name="ten" Type="String" />
                            <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                            <asp:Parameter DefaultValue="" Name="id_all" Type="Int16" />
                            <asp:Parameter Name="text_all" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
                <div class="col-sm-4">
                    <telerik:RadTextBox ID="tbTen" runat="server" EmptyMessage="Nhập tên thực phẩm cần tìm kiếm" Width="100%"></telerik:RadTextBox>
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
                         <telerik:GridBoundColumn DataField="ID_NHOM_THUC_PHAM" FilterControlAltText="Filter ID_NHOM_THUC_PHAM column" HeaderText="Tên nhóm" SortExpression="ID_NHOM_THUC_PHAM" UniqueName="ID_NHOM_THUC_PHAM" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TEN_NHOM_THUC_PHAM" FilterControlAltText="Filter TEN_NHOM_THUC_PHAM column" HeaderText="Tên nhóm thực phẩm" SortExpression="TEN_NHOM_THUC_PHAM" UniqueName="TEN_NHOM_THUC_PHAM" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TEN_THUC_PHAM" FilterControlAltText="Filter TEN_THUC_PHAM column" HeaderText="Tên thực phẩm" SortExpression="TEN_THUC_PHAM" UniqueName="TEN_THUC_PHAM" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="40%">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DON_VI_TINH" FilterControlAltText="Filter DON_VI_TINH column" HeaderText="Đơn vị tính" SortExpression="DON_VI_TINH" UniqueName="DON_VI_TINH" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SO_LUONG" FilterControlAltText="Filter SO_LUONG column" HeaderText="Số lượng" SortExpression="SO_LUONG" UniqueName="SO_LUONG" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="LUONG_TRONG_KHO" FilterControlAltText="Filter LUONG_TRONG_KHO column" HeaderText="Số lượng" SortExpression="LUONG_TRONG_KHO" UniqueName="LUONG_TRONG_KHO" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                        </telerik:GridBoundColumn>
                    </Columns>
                    <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>