<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="GiaoVien.aspx.cs" Inherits="CMS.GiaoVien.GiaoVien" %>

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
            <telerik:AjaxSetting AjaxControlID="rcbTrangThai">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbGioiTinh">
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
                Height="400px" Title="Chi tiết giáo viên" Behaviors="Reload,Close,Maximize" Modal="true" VisibleStatusbar="false">
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
                <span class="item-title">DANH SÁCH GIÁO VIÊN</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btTimKiem" runat="server" Text="Tìm kiếm" CssClass="btn bt-one" OnClick="btTimKiem_Click" />
                <input type="button" id="btAddNew" runat="server" class="btn bt-one" value="Thêm mới" onclick="openRadWin('GiaoVienDetail.aspx', 860, 620, 1)" />
                <asp:Button ID="btDeleteChon" runat="server" CssClass="btn bt-one" OnClick="btDeleteChon_Click" Text="Xóa GV" OnClientClick="if(!btDeteleClick()) return false;" />
                <asp:Button ID="btDeleteByRoot" runat="server" CssClass="btn bt-one" OnClick="btDeleteByRoot_Click" Text="Xóa chi tiết GV" OnClientClick="if(!btDeteleClick()) return false;" />
                <asp:LinkButton runat="server" ID="btnImportExcel" href="\GiaoVien\GiaoVienImportExcel.aspx" CssClass="btn bt-one" Font-Size="17px">Import Excel</asp:LinkButton>
                <asp:Button ID="btExport" runat="server" CssClass="btn bt-one" OnClick="btExport_Click" Text="Xuất excel" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="row">
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbTrangThai" runat="server" Width="100%" DataSourceID="objTrangThai" DataTextField="TEN" DataValueField="MA" AutoPostBack="True" EmptyMessage="Trạng thái giáo viên" AllowCustomText="true" Filter="Contains" OnSelectedIndexChanged="rcbTrangThai_SelectedIndexChanged">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objTrangThai" runat="server" SelectMethod="getTrangThaiGV" TypeName="OneEduDataAccess.BO.TrangThaiGVBO">
                        <SelectParameters>
                            <asp:Parameter Name="is_all" DbType="Boolean" DefaultValue="False" />
                            <asp:Parameter Name="id_all" DbType="Int16" DefaultValue="0" />
                            <asp:Parameter Name="text_all" DbType="String" DefaultValue="" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbGioiTinh" runat="server" Width="100%" DataSourceID="objGioiTinh" DataTextField="TEN" DataValueField="MA" AutoPostBack="True" EmptyMessage="Chọn giới tính" AllowCustomText="true" Filter="Contains" OnSelectedIndexChanged="rcbGioiTinh_SelectedIndexChanged">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objGioiTinh" runat="server" SelectMethod="getGioiTinh" TypeName="OneEduDataAccess.BO.GioiTinhBO">
                        <SelectParameters>
                            <asp:Parameter Name="is_all" DbType="Boolean" DefaultValue="False" />
                            <asp:Parameter Name="id_all" DbType="Int16" DefaultValue="0" />
                            <asp:Parameter Name="text_all" DbType="String" DefaultValue="" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
                <div class="col-sm-3">
                    <telerik:RadTextBox ID="tbTenGV" runat="server" EmptyMessage="Nhập tên GV cần tìm kiếm" Width="100%"></telerik:RadTextBox>
                </div>
                <div class="col-sm-3">
                    <telerik:RadTextBox ID="tbSDT" runat="server" EmptyMessage="Nhập SĐT cần tìm kiếm" Width="100%"></telerik:RadTextBox>
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
                        <telerik:GridBoundColumn DataField="HO_TEN" FilterControlAltText="Filter HO_TEN column" HeaderText="Tên GV" SortExpression="HO_TEN" UniqueName="HO_TEN" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NGAY_SINH" FilterControlAltText="Filter NGAY_SINH column" HeaderText="Ngày sinh" SortExpression="NGAY_SINH" UniqueName="NGAY_SINH"  HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MA_GIOI_TINH" FilterControlAltText="Filter MA_GIOI_TINH column" HeaderText="Giới tính" SortExpression="MA_GIOI_TINH" UniqueName="MA_GIOI_TINH" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="GIOI_TINH_STR" FilterControlAltText="Filter GIOI_TINH_STR column" HeaderText="Giới tính" SortExpression="GIOI_TINH_STR" UniqueName="GIOI_TINH_STR" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ID_CHUC_VU" FilterControlAltText="Filter ID_CHUC_VU column" HeaderText="chức vụ hid" SortExpression="ID_CHUC_VU" UniqueName="ID_CHUC_VU" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CHUC_VU_STR" FilterControlAltText="Filter CHUC_VU_STR column" HeaderText="Chức vụ" SortExpression="CHUC_VU_STR" UniqueName="CHUC_VU_STR" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MA_TRANG_THAI" FilterControlAltText="Filter MA_TRANG_THAI column" HeaderText="Trạng thái" SortExpression="MA_TRANG_THAI" UniqueName="MA_TRANG_THAI" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TRANG_THAI_STR" FilterControlAltText="Filter TRANG_THAI_STR column" HeaderText="Trạng thái" SortExpression="TRANG_THAI_STR" UniqueName="TRANG_THAI_STR" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SDT" FilterControlAltText="Filter SDT column" HeaderText="Số điện thoại" SortExpression="SDT" UniqueName="SDT" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DIA_CHI" FilterControlAltText="Filter DIA_CHI column" HeaderText="Địa chỉ" SortExpression="DIA_CHI" UniqueName="DIA_CHI" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Sửa" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="SUA">
                            <ItemTemplate>
                                <img src="../img/sua.png" onclick='<%# "openRadWin(\"GiaoVienDetail.aspx?id_GiaoVien=" + Eval("ID") + "\",860, 620, 1)" %>' style="cursor: pointer;" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
