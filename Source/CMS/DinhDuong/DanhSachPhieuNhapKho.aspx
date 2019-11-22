<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="DanhSachPhieuNhapKho.aspx.cs" Inherits="CMS.DinhDuong.DanhSachPhieuNhapKho" %>

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
            <telerik:AjaxSetting AjaxControlID="rcbNguoiNhanHang">
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
                Height="400px" Title="Chi tiết phiếu nhập" Behaviors="Reload,Close,Maximize" Modal="true" VisibleStatusbar="false">
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
                    if (confirm("Chi tiết phiếu sẽ bị xóa, bạn có chắc chắn muốn xóa?")) {
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
                <span class="item-title">Danh sách phiếu nhập kho</span>
            </div>
            <div class="col-sm-8 text-right">
                <input type="button" id="btnNhapKho" runat="server" class="btn bt-one" value="Thêm phiếu nhập" onclick="openRadWin('PhieuNhapKho.aspx', 100, 50)" />
                <asp:Button ID="btExport" runat="server" CssClass="btn bt-one" OnClick="btExport_Click" Text="Xuất excel" />
                <asp:Button ID="btDeleteChon" runat="server" CssClass="btn bt-one" OnClick="btDeleteChon_Click" Text="Xóa" OnClientClick="if(!btDeteleClick()) return false;" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="row">
                <div class="col-sm-6">
                    <telerik:RadDatePicker Culture="vi-VN" RenderMode="Classic" ID="rdTuNgay" runat="server" Width="48%"
                        Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="Giá trị không hợp lệ"
                        Calendar-FastNavigationSettings-TodayButtonCaption="Hôm nay"
                        DatePopupButton-ToolTip="Chọn ngày"
                        Calendar-FastNavigationSettings-OkButtonCaption="Chọn"
                        Calendar-FastNavigationSettings-CancelButtonCaption="Hủy" OnSelectedDateChanged="rdTuNgay_SelectedDateChanged" AutoPostBack="true">
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
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbNguoiNhanHang" runat="server" Width="100%" DataSourceID="objNhanVien" EmptyMessage="Chọn người nhận hàng" DataTextField="HO_TEN" DataValueField="ID" AllowCustomText="true" Filter="Contains" OnSelectedIndexChanged="rcbNguoiNhanHang_SelectedIndexChanged" AutoPostBack="true">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objNhanVien" runat="server" SelectMethod="getNhanVienByTruong" TypeName="OneEduDataAccess.BO.GiaoVienBO">
                        <SelectParameters>
                        </SelectParameters>
                    </asp:ObjectDataSource>
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
                        <telerik:GridBoundColumn DataField="MA_SO_PHIEU" FilterControlAltText="Filter MA_SO_PHIEU column" HeaderText="Mã phiếu" SortExpression="MA_SO_PHIEU" UniqueName="MA_SO_PHIEU" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NGAY_NHAP_KHO" FilterControlAltText="Filter NGAY_NHAP_KHO column" HeaderText="Ngày nhập" SortExpression="NGAY_NHAP_KHO" UniqueName="NGAY_NHAP_KHO" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ID_NGUOI_NHAN_HANG" FilterControlAltText="Filter ID_NGUOI_NHAN_HANG column" HeaderText="ID_NGUOI_NHAN_HANG" SortExpression="ID_NGUOI_NHAN_HANG" UniqueName="ID_NGUOI_NHAN_HANG" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NGUOI_NHAN_HANG" FilterControlAltText="Filter NGUOI_NHAN_HANG column" HeaderText="Người nhận hàng" SortExpression="NGUOI_NHAN_HANG" UniqueName="NGUOI_NHAN_HANG" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TONG_GIA" FilterControlAltText="Filter TONG_GIA column" HeaderText="Tổng tiền (VNĐ)" SortExpression="TONG_GIA" UniqueName="TONG_GIA" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="250px">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Sửa" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="SUA">
                            <ItemTemplate>
                                <img src="../img/sua.png" onclick='<%# "openRadWin(\"ChiTietPhieuNhapKho.aspx?id_phieu_nhap=" + Eval("ID") + "\",100, 50)" %>' style="cursor: pointer;" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
