<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterSYS.Master" AutoEventWireup="true" CodeBehind="TinNhanDatLich.aspx.cs" Inherits="CMS.LichSuTinNhan.TinNhanDatLich" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbTruong">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblTongTin" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <telerik:RadCodeBlock runat="server">
        <script>
            var grid;
            function pageload() {
                grid = $find("<%=RadGrid1.ClientID%>");
            }
            function btnXoaTuyChonClick() {
                grid = $find("<%=RadGrid1.ClientID%>");
                if (grid.get_masterTableView().get_selectedItems().length > 0) {
                    if (confirm("Bạn có chắc chắn muốn dừng gửi?")) {
                        return true;
                    }
                } else {
                    alert("Mời bạn chọn tin nhắn để dừng gửi.");
                    return false;
                }
            }

            function btConfirmDelete() {
                if (confirm("Bạn chắc chắn muốn xóa tin nhắn?")) {
                    return true;
                }
            }
        </script>
    </telerik:RadCodeBlock>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">LỊCH SỬ TIN NHẮN</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btTimKiem" runat="server" Text="Tìm kiếm" CssClass="btn bt-one" OnClick="btTimKiem_Click" />
                <asp:Button ID="btXoaTuyChon" runat="server" CssClass="btn bt-one" OnClick="btXoaTuyChon_Click" Text="Xóa tùy chọn" OnClientClick="if(!btnXoaTuyChonClick()) return false;" />
                <asp:Button ID="btnXoa" runat="server" CssClass="btn bt-one" OnClick="btnXoa_Click" Text="Xóa tất cả" OnClientClick="if(!btConfirmDelete()) return false;" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="form-horizontal" role="form">
                <div class="row">
                    <div class="col-sm-3">
                        <telerik:RadComboBox ID="rcbTruong" runat="server" Width="100%" DataTextField="TEN" DataValueField="ID" Filter="Contains" OnSelectedIndexChanged="rcbTruong_SelectedIndexChanged" AutoPostBack="true">
                        </telerik:RadComboBox>
                    </div>
                    <div class="col-sm-6">
                        <telerik:RadTextBox ID="tbNoiDung" runat="server" EmptyMessage="Nhập nội dung tin nhắn cần tìm kiếm" Width="100%"></telerik:RadTextBox>
                    </div>
                    <div class="col-sm-3">
                        <span style="color: red;">
                            <asp:Label runat="server" ID="lblTongTin"></asp:Label>
                        </span>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="item-data">
        <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" GroupPanelPosition="Top" OnNeedDataSource="RadGrid1_NeedDataSource" OnItemDataBound="RadGrid1_ItemDataBound" AutoGenerateColumns="False" PageSize="100" AllowMultiRowSelection="True">
            <ClientSettings>
                <Selecting AllowRowSelect="true"></Selecting>
                <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>
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
                    <telerik:GridBoundColumn DataField="ID" FilterControlAltText="Filter ID column" HeaderText="ID" SortExpression="ID" UniqueName="ID" HeaderStyle-HorizontalAlign="Center" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="BRAND_NAME" FilterControlAltText="Filter BRAND_NAME column" HeaderText="Tên thương hiệu" SortExpression="BRAND_NAME" UniqueName="BRAND_NAME" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="GUI_BO_ME" FilterControlAltText="Filter GUI_BO_ME column" HeaderText="GUI_BO_ME" SortExpression="GUI_BO_ME" UniqueName="GUI_BO_ME" HeaderStyle-HorizontalAlign="Center" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="SDT_NHAN" FilterControlAltText="Filter SDT_NHAN column" HeaderText="SĐT nhận tin" SortExpression="SDT_NHAN" UniqueName="SDT_NHAN" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn HeaderText="Nội dung" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" UniqueName="NOI_DUNG_KHONG_DAU">
                        <ItemTemplate>
                            <%#localAPI.catChuoi(Eval("NOI_DUNG_KHONG_DAU").ToString(), 100) %>

                            <image src="../img/read_more.png" data-toggle="modal" data-target='<%# "#modal"+ Container.DataSetIndex+1 %>' style='<%#"cursor: pointer; display:" +(Eval("NOI_DUNG_KHONG_DAU").ToString().Length>100?"unset": "none")  %>'></image>

                            <div id='<%# "modal"+ Container.DataSetIndex+1 %>' class="modal fade bd-example-modal-sm" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
                                <div class="modal-dialog modal-sm">
                                    <div class="modal-content">
                                        <%#  Eval("NOI_DUNG_KHONG_DAU") %>
                                    </div>
                                </div>
                            </div>
                            <asp:HiddenField ID="hdNoiDung" runat="server" Value='<%# Eval("NOI_DUNG_KHONG_DAU") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="SO_TIN" FilterControlAltText="Filter SO_TIN column" HeaderText="Số tin" SortExpression="SO_TIN" UniqueName="SO_TIN" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="LOAI_TIN" FilterControlAltText="Filter LOAI_TIN column" HeaderText="Loại tin" SortExpression="LOAI_TIN" UniqueName="LOAI_TIN" HeaderStyle-HorizontalAlign="Center" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="TRANG_THAI" FilterControlAltText="Filter TRANG_THAI column" HeaderText="Trạng thái" SortExpression="TRANG_THAI" UniqueName="TRANG_THAI" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="THOI_GIAN_GUI" FilterControlAltText="Filter THOI_GIAN_GUI column" HeaderText="Thời gian gửi" SortExpression="THOI_GIAN_GUI" UniqueName="THOI_GIAN_GUI" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="SEND_NUMBER" FilterControlAltText="Filter SEND_NUMBER column" HeaderText="Số lần gửi lại" SortExpression="SEND_NUMBER" UniqueName="SEND_NUMBER" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="NGUOI_GUI" FilterControlAltText="Filter NGUOI_GUI column" HeaderText="Người gửi" SortExpression="NGUOI_GUI" UniqueName="NGUOI_GUI" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="LOAI_NHA_MANG" FilterControlAltText="Filter LOAI_NHA_MANG column" HeaderText="Nhà mạng" SortExpression="LOAI_NHA_MANG" UniqueName="LOAI_NHA_MANG" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px">
                    </telerik:GridBoundColumn>
                </Columns>
                <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
            </MasterTableView>
        </telerik:RadGrid>
    </div>
</asp:Content>