<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="MonTruong.aspx.cs" Inherits="CMS.MonHoc.MonTruong" %>
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
            <telerik:AjaxSetting AjaxControlID="rcbKieuMon">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="cbMonChuyen">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbLoaiLopGDTX">
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
                Height="400px" Title="Chi tiết môn học" Behaviors="Reload,Close,Maximize" Modal="true" VisibleStatusbar="false">
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
                <span class="item-title">QUẢN LÝ MÔN HỌC</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btTimKiem" runat="server" Text="Tìm kiếm" CssClass="btn bt-one" OnClick="btTimKiem_Click"/>
                <input type="button" id="btAddNew" runat="server" class="btn bt-one" value="Thêm mới" onclick="openRadWin('MonTruongDetail.aspx', 880, 600, 1)" />
                <asp:Button ID="btDeleteChon" runat="server" CssClass="btn bt-one" OnClick="btDeleteChon_Click" Text="Xóa môn" OnClientClick="if(!btDeteleClick()) return false;" />
                <asp:Button ID="btExport" runat="server" CssClass="btn bt-one" OnClick="btExport_Click" Text="Xuất excel" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="row">
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbKieuMon" runat="server" Width="100%" EmptyMessage="Chọn kiểu môn" AllowCustomText="true" Filter="Contains" OnSelectedIndexChanged="rcbKieuMon_SelectedIndexChanged" AutoPostBack="true">
                        <Items>
                            <telerik:RadComboBoxItem Value="" Text="Chọn tất cả"/>
                            <telerik:RadComboBoxItem Value="0" Text="Điểm số"/>
                            <telerik:RadComboBoxItem Value="1" Text="Điểm nhận xét"/>
                        </Items>
                    </telerik:RadComboBox>
                </div>
                <div class="col-sm-3" runat="server" id="divLoaiLopGDTX">
                    <telerik:RadComboBox ID="rcbLoaiLopGDTX" runat="server" Width="100%" DataTextField="TEN" DataValueField="MA" AutoPostBack="true" CausesValidation="false" Filter="Contains" OnSelectedIndexChanged="rcbLoaiLopGDTX_SelectedIndexChanged">
                        <Items>
                            <telerik:RadComboBoxItem Value="2" Text="Bổ túc THCS" />
                            <telerik:RadComboBoxItem Value="3" Text="Bổ túc THPT" />
                        </Items>
                    </telerik:RadComboBox>
                </div>
                <div class="col-sm-3" runat="server" id="divMonChuyen">
                    <div class="checkbox">
                        <label>
                            <asp:CheckBox ID="cbMonChuyen" runat="server" Text="Là môn chuyên" OnCheckedChanged="cbMonChuyen_CheckedChanged" AutoPostBack="true"/>
                        </label>
                    </div>
                </div>
                <div class="col-sm-3">
                    <telerik:RadTextBox ID="tbTen" runat="server" EmptyMessage="Nhập tên môn học cần tìm kiếm" Width="100%"></telerik:RadTextBox>
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
                                <asp:HiddenField ID="hdIdMonHoc" runat="server" Value='<%#Bind("ID_MON_HOC") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="TEN" FilterControlAltText="Filter TEN column" HeaderText="Tên" SortExpression="TEN" UniqueName="TEN" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="KIEU_MON" FilterControlAltText="Filter KIEU_MON column" HeaderText="Kiểu môn" SortExpression="KIEU_MON" UniqueName="KIEU_MON" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="KIEU_MON_STR" FilterControlAltText="Filter KIEU_MON_STR column" HeaderText="Kiểu môn" SortExpression="KIEU_MON_STR" UniqueName="KIEU_MON_STR" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="HE_SO" FilterControlAltText="Filter HE_SO column" HeaderText="Hệ số" SortExpression="HE_SO" UniqueName="HE_SO" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="THU_TU" FilterControlAltText="Filter THU_TU column" HeaderText="Thứ tự" SortExpression="THU_TU" UniqueName="THU_TU" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridCheckBoxColumn DataField="IS_MON_CHUYEN" FilterControlAltText="Filter IS_MON_CHUYEN column" HeaderText="Môn chuyên" SortExpression="IS_MON_CHUYEN" UniqueName="IS_MON_CHUYEN" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IS_1" FilterControlAltText="Filter IS_1 column" HeaderText="Khối 1" SortExpression="IS_1" UniqueName="IS_1" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IS_2" FilterControlAltText="Filter IS_2 column" HeaderText="Khối 2" SortExpression="IS_2" UniqueName="IS_2" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IS_3" FilterControlAltText="Filter IS_3 column" HeaderText="Khối 3" SortExpression="IS_3" UniqueName="IS_3" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IS_4" FilterControlAltText="Filter IS_4 column" HeaderText="Khối 4" SortExpression="IS_4" UniqueName="IS_4" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IS_5" FilterControlAltText="Filter IS_5 column" HeaderText="Khối 5" SortExpression="IS_5" UniqueName="IS_5" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IS_6" FilterControlAltText="Filter IS_6 column" HeaderText="Khối 6" SortExpression="IS_6" UniqueName="IS_6" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IS_7" FilterControlAltText="Filter IS_7 column" HeaderText="Khối 7" SortExpression="IS_7" UniqueName="IS_7" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IS_8" FilterControlAltText="Filter IS_8 column" HeaderText="Khối 8" SortExpression="IS_8" UniqueName="IS_8" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IS_9" FilterControlAltText="Filter IS_9 column" HeaderText="Khối 9" SortExpression="IS_9" UniqueName="IS_9" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IS_10" FilterControlAltText="Filter IS_10 column" HeaderText="Khối 10" SortExpression="IS_10" UniqueName="IS_10" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IS_11" FilterControlAltText="Filter IS_11 column" HeaderText="Khối 11" SortExpression="IS_11" UniqueName="IS_11" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IS_12" FilterControlAltText="Filter IS_12 column" HeaderText="Khối 12" SortExpression="IS_12" UniqueName="IS_12" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridCheckBoxColumn>
                        <telerik:GridBoundColumn DataField="ID_MON_HOC" FilterControlAltText="Filter ID_MON_HOC column" HeaderText="ID_MON_HOC" SortExpression="ID_MON_HOC" UniqueName="ID_MON_HOC" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Sửa" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="SUA">
                            <ItemTemplate>
                                <img src="../img/sua.png" onclick='<%# "openRadWin(\"MonTruongDetail.aspx?id_mon_truong=" + Eval("ID") + "&id_mon=" + Eval("ID_MON_HOC") + "\",880, 600, 1)" %>' style="cursor: pointer;" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
