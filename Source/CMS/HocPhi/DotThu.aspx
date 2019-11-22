<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="DotThu.aspx.cs" Inherits="CMS.HocPhi.DotThu" %>
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
            <telerik:AjaxSetting AjaxControlID="rcbLoaiKhoanThu">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbHocKy"/>
                    <telerik:AjaxUpdatedControl ControlID="rcbThang"/>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbHocKy">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbThang">
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
                Height="400px" Title="Chi tiết mã nhận xét" Behaviors="Reload,Close,Maximize" Modal="true" VisibleStatusbar="false">
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
                    alert("Bạn chưa chọn bản ghi nào để xóa.");
                    return false;
                }
            }
        </script>
    </telerik:RadCodeBlock>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">QUẢN LÝ KHOẢN THU</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btTimKiem" runat="server" Text="Tìm kiếm" CssClass="btn bt-one" OnClick="btTimKiem_Click" />
                <input type="button" id="btAddNew" runat="server" class="btn bt-one" value="Thêm mới" onclick="openRadWin('DotThuDetail.aspx', 860, 550, 1)" />
                <input type="button" id="Button1" runat="server" class="btn bt-one" value="Tạo đợt thu lớp" onclick="openRadWin('DotThuLopDetail.aspx', 860, 550, 1)" />
                <asp:Button ID="btDeleteChon" runat="server" CssClass="btn bt-one" OnClick="btDeleteChon_Click" Text="Xóa" OnClientClick="if(!btDeteleClick()) return false;" />
                <asp:Button ID="btExport" runat="server" CssClass="btn bt-one" OnClick="btExport_Click" Text="Xuất excel" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="row">
                <div class="col-sm-2">
                    <telerik:RadComboBox ID="rcbLoaiKhoanThu" runat="server" Width="100%" EmptyMessage="Chọn loại thu" AllowCustomText="true" Filter="Contains" OnSelectedIndexChanged="rcbLoaiKhoanThu_SelectedIndexChanged" AutoPostBack="true">
                        <Items>
                            <telerik:RadComboBoxItem Value="" Text="Tất cả" />
                            <telerik:RadComboBoxItem Value="1" Text="Theo năm" />
                            <telerik:RadComboBoxItem Value="2" Text="Theo kỳ" />
                            <telerik:RadComboBoxItem Value="3" Text="Theo tháng" />
                        </Items>
                    </telerik:RadComboBox>
                </div>
                <div class="col-sm-2">
                    <telerik:RadComboBox ID="rcbHocKy" runat="server" Width="100%" EmptyMessage="Chọn học kỳ" AllowCustomText="true" Filter="Contains" OnSelectedIndexChanged="rcbHocKy_SelectedIndexChanged" AutoPostBack="true" Enabled="false">
                        <Items>
                            <telerik:RadComboBoxItem Value="" Text="Tất cả" />
                            <telerik:RadComboBoxItem Value="1" Text="HK 1" />
                            <telerik:RadComboBoxItem Value="2" Text="HK 2" />
                        </Items>
                    </telerik:RadComboBox>
                </div>
                <div class="col-sm-2">
                    <telerik:RadComboBox ID="rcbThang" runat="server" Width="100%" EmptyMessage="Chọn tháng" AllowCustomText="true" Filter="Contains" AutoPostBack="True" OnSelectedIndexChanged="rcbThang_SelectedIndexChanged" Enabled="false">
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
                <div class="col-sm-6">
                    <telerik:RadTextBox ID="tbTen" runat="server" EmptyMessage="Nhập đợt thu cần tìm kiếm" Width="100%"></telerik:RadTextBox>
                </div>
            </div>
        </div>
        <div class="item-data">
            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" GroupPanelPosition="Top" OnNeedDataSource="RadGrid1_NeedDataSource"
                AutoGenerateColumns="False" PageSize="100" AllowMultiRowSelection="True" OnItemDataBound="RadGrid1_ItemDataBound">
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
                        <telerik:GridBoundColumn DataField="TEN" FilterControlAltText="Filter TEN column" HeaderText="Tên đợt thu" SortExpression="TEN" UniqueName="TEN" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ID_DOT_THU" FilterControlAltText="Filter ID_DOT_THU column" HeaderText="ID_DOT_THU" SortExpression="ID_DOT_THU" UniqueName="ID_DOT_THU" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="HOC_KY" FilterControlAltText="Filter HOC_KY column" HeaderText="HOC_KY" SortExpression="HOC_KY" UniqueName="HOC_KY" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="THANG" FilterControlAltText="Filter THANG column" HeaderText="THANG" SortExpression="THANG" UniqueName="THANG" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DOT_THU" FilterControlAltText="Filter DOT_THU column" HeaderText="Đợt thu" SortExpression="DOT_THU" UniqueName="DOT_THU" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="THOI_GIAN_BAT_DAU" FilterControlAltText="Filter THOI_GIAN_BAT_DAU column" HeaderText="THOI_GIAN_BAT_DAU" SortExpression="THOI_GIAN_BAT_DAU" UniqueName="THOI_GIAN_BAT_DAU" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="THOI_GIAN_KET_THUC" FilterControlAltText="Filter THOI_GIAN_KET_THUC column" HeaderText="THOI_GIAN_KET_THUC" SortExpression="THOI_GIAN_KET_THUC" UniqueName="THOI_GIAN_KET_THUC" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="THOI_GIAN" FilterControlAltText="Filter THOI_GIAN column" HeaderText="Thời gian thu" SortExpression="THOI_GIAN" UniqueName="THOI_GIAN" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="250px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="GHI_CHU" FilterControlAltText="Filter GHI_CHU column" HeaderText="Nội dung thu" SortExpression="GHI_CHU" UniqueName="GHI_CHU" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="250px">
                        </telerik:GridBoundColumn>
                        <telerik:GridCheckBoxColumn DataField="IS_TIEN_AN" FilterControlAltText="Filter IS_TIEN_AN column" HeaderText="Tiền ăn" SortExpression="IS_TIEN_AN" UniqueName="IS_TIEN_AN" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridCheckBoxColumn>
                        <telerik:GridBoundColumn DataField="TONG_TIEN" FilterControlAltText="Filter TONG_TIEN column" HeaderText="Tổng tiền" SortExpression="TONG_TIEN" UniqueName="TONG_TIEN" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px" DataFormatString="{0:#,0}">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="THU_TU" FilterControlAltText="Filter THU_TU column" HeaderText="Thứ tự" SortExpression="THU_TU" UniqueName="THU_TU" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Sửa" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="SUA">
                            <ItemTemplate>
                                <img src="../img/sua.png" onclick='<%# "openRadWin(\"DotThuDetail.aspx?id=" + Eval("ID") + "\", 860, 550, 1)" %>' style="cursor: pointer;" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>