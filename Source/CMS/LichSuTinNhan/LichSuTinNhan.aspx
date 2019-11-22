<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterSYS.Master" AutoEventWireup="true" CodeBehind="LichSuTinNhan.aspx.cs" Inherits="CMS.LichSuTinNhan.LichSuTinNhan" %>

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
                    <telerik:AjaxUpdatedControl ControlID="rcbCapHoc" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="rcbKhoi" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="rcbLop" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="lblTongTin" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbCapHoc">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbKhoi" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="rcbLop" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="lblTongTin" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbLoaiLopGDTX">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbKhoi" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="rcbLop" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="lblTongTin" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbKhoi">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbLop" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="lblTongTin" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbLop">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblTongTin" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rdTuNgay">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblTongTin" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rdDenNgay">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblTongTin" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbTrangThai">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblTongTin" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbNhaMang">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblTongTin" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbLoaiTinNhan">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblTongTin" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbSoTinGui">
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
        </script>
    </telerik:RadCodeBlock>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">LỊCH SỬ TIN NHẮN</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btTimKiem" runat="server" Text="Tìm kiếm" CssClass="btn bt-one" OnClick="btTimKiem_Click" />
                <asp:Button ID="btGuiLaiTin" runat="server" CssClass="btn bt-one" Text="Gửi lại" OnClientClick="if (confirm('Bạn có chắc chắn muốn gửi lại tin?')) return true; else return false;" OnClick="btGuiLaiTin_Click" />
                <asp:Button ID="btExport" runat="server" CssClass="btn bt-one" OnClick="btExport_Click" Text="Xuất excel" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="form-horizontal" role="form">
                <div class="row">
                    <div class="col-sm-3">
                        <telerik:RadComboBox ID="rcbTruong" runat="server" Width="100%" DataTextField="TEN" DataValueField="ID" Filter="Contains" OnSelectedIndexChanged="rcbTruong_SelectedIndexChanged" AutoPostBack="true" EmptyMessage="Chọn trường học" AllowCustomText="true">
                        </telerik:RadComboBox>
                    </div>
                    <div class="col-sm-3">
                        <telerik:RadComboBox ID="rcbCapHoc" runat="server" Width="100%" Filter="Contains" OnSelectedIndexChanged="rcbCapHoc_SelectedIndexChanged" AutoPostBack="true" EmptyMessage="Chọn cấp học" AllowCustomText="true">
                        </telerik:RadComboBox>
                    </div>
                    <div class="col-sm-3">
                        <telerik:RadDatePicker Culture="vi-VN" RenderMode="Classic" ID="rdTuNgay" runat="server" Width="45%" MinDate="1900/1/1"
                            Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="Giá trị không hợp lệ"
                            Calendar-FastNavigationSettings-TodayButtonCaption="Hôm nay"
                            DatePopupButton-ToolTip="Ngày bắt đầu"
                            Calendar-FastNavigationSettings-OkButtonCaption="Chọn"
                            Calendar-FastNavigationSettings-CancelButtonCaption="Hủy"
                            OnSelectedDateChanged="rdTuNgay_SelectedDateChanged" AutoPostBack="true">
                            <DateInput DateFormat="dd/MM/yyyy"></DateInput>
                        </telerik:RadDatePicker>
                        &nbsp;---&nbsp;
                        <telerik:RadDatePicker Culture="vi-VN" RenderMode="Classic" ID="rdDenNgay" runat="server" Width="45%" MinDate="1900/1/1"
                            Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="Giá trị không hợp lệ"
                            Calendar-FastNavigationSettings-TodayButtonCaption="Hôm nay"
                            DatePopupButton-ToolTip="Ngày kết thúc"
                            Calendar-FastNavigationSettings-OkButtonCaption="Chọn"
                            Calendar-FastNavigationSettings-CancelButtonCaption="Hủy" OnSelectedDateChanged="rdDenNgay_SelectedDateChanged" AutoPostBack="true">
                            <DateInput DateFormat="dd/MM/yyyy"></DateInput>
                        </telerik:RadDatePicker>
                    </div>
                    <div class="col-sm-3">
                        <telerik:RadComboBox ID="rcbSoTinGui" runat="server" Width="100%" EmptyMessage="Chọn số tin trên một lần gửi" AllowCustomText="true" Filter="Contains" OnSelectedIndexChanged="rcbSoTinGui_SelectedIndexChanged" AutoPostBack="true">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="Tất cả" />
                                <telerik:RadComboBoxItem Value="1" Text="1" />
                                <telerik:RadComboBoxItem Value="2" Text="2" />
                            </Items>
                        </telerik:RadComboBox>
                    </div>
                </div>
                <div class="row" style="margin-top: 15px;">
                    <%--<div class="col-sm-3" runat="server" id="divLoaiLopGDTX">
                        <telerik:RadComboBox ID="rcbLoaiLopGDTX" runat="server" Width="100%" DataTextField="TEN" DataValueField="MA" Filter="Contains" OnSelectedIndexChanged="rcbLoaiLopGDTX_SelectedIndexChanged" AutoPostBack="true" EmptyMessage="Chọn loại lớp GDTX">
                        </telerik:RadComboBox>
                    </div>--%>
                    <div class="col-sm-3">
                        <telerik:RadComboBox ID="rcbKhoi" runat="server" Width="100%" DataSourceID="objKhoiHoc" DataTextField="TEN" DataValueField="MA" Filter="Contains" AllowCustomText="true" OnSelectedIndexChanged="rcbKhoi_SelectedIndexChanged" EmptyMessage="Chọn khối học" AutoPostBack="true">
                        </telerik:RadComboBox>
                        <asp:ObjectDataSource ID="objKhoiHoc" runat="server" SelectMethod="getKhoiByCapHocAndMaLoaiGDTX" TypeName="OneEduDataAccess.BO.KhoiBO">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="rcbCapHoc" Name="cap_hoc" PropertyName="SelectedValue" />
                                <asp:Parameter Name="maLoaiLopGDTX" DefaultValue="" />
                                <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                                <asp:Parameter DefaultValue="" Name="id_all" Type="Int64" />
                                <asp:Parameter DefaultValue="Toàn trường" Name="text_all" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </div>
                    <div class="col-sm-3">
                        <telerik:RadComboBox Width="100%" ID="rcbLop" EmptyMessage="Chọn lớp" runat="server"
                            DataSourceID="objLop" DataTextField="TEN" DataValueField="ID" AllowCustomText="true" Filter="Contains"
                            OnSelectedIndexChanged="rcbLop_SelectedIndexChanged" AutoPostBack="true">
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
                    <div class="col-sm-3">
                        <telerik:RadComboBox ID="rcbLoaiNguoiNhan" runat="server" Width="100%" EmptyMessage="Chọn loại người nhận" AllowCustomText="true" Filter="Contains" OnSelectedIndexChanged="rcbLoaiNguoiNhan_SelectedIndexChanged" AutoPostBack="true">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="Tất cả" />
                                <telerik:RadComboBoxItem Value="1" Text="Học sinh" />
                                <telerik:RadComboBoxItem Value="2" Text="Giáo viên" />
                                <telerik:RadComboBoxItem Value="3" Text="Khác" />
                            </Items>
                        </telerik:RadComboBox>
                    </div>
                    <div class="col-sm-3">
                        <telerik:RadComboBox ID="rcbLoaiTinNhan" runat="server" Width="100%" Filter="Contains" AutoPostBack="true" AllowCustomText="true" OnSelectedIndexChanged="rcbLoaiTinNhan_SelectedIndexChanged" EmptyMessage="Chọn loại tin">
                            <Items>
                                <telerik:RadComboBoxItem Value="1" Text="Liên lạc cá nhân" />
                                <telerik:RadComboBoxItem Value="2" Text="Gửi tin thông báo" />
                            </Items>
                        </telerik:RadComboBox>
                    </div>
                </div>
                <div class="row" style="margin-top: 15px;">
                    <div class="col-sm-3">
                        <telerik:RadComboBox ID="rcbNhaMang" runat="server" Width="100%" Filter="Contains" AllowCustomText="true" OnSelectedIndexChanged="rcbNhaMang_SelectedIndexChanged" EmptyMessage="Chọn nhà mạng" AutoPostBack="true">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="Tất cả" />
                                <telerik:RadComboBoxItem Value="1" Text="Viettel" />
                                <telerik:RadComboBoxItem Value="2" Text="MobiFone" />
                                <telerik:RadComboBoxItem Value="3" Text="VinaPhone" />
                                <telerik:RadComboBoxItem Value="4" Text="VietnamMobile" />
                                <telerik:RadComboBoxItem Value="5" Text="Gmobile" />
                            </Items>
                        </telerik:RadComboBox>
                    </div>
                    <div class="col-sm-3">
                        <telerik:RadComboBox ID="rcbTrangThai" runat="server" Width="100%" Filter="Contains" AllowCustomText="true" EmptyMessage="Chọn trạng thái" OnSelectedIndexChanged="rcbTrangThai_SelectedIndexChanged" AutoPostBack="true">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="Tất cả" />
                                <telerik:RadComboBoxItem Value="0" Text="Chờ gửi" />
                                <telerik:RadComboBoxItem Value="1" Text="Thành công" />
                                <telerik:RadComboBoxItem Value="2" Text="Gửi lỗi" />
                                <telerik:RadComboBoxItem Value="3" Text="Dừng gửi" />
                            </Items>
                        </telerik:RadComboBox>
                    </div>
                    <div class="col-sm-3">
                        <telerik:RadTextBox ID="tbSDT" runat="server" EmptyMessage="Nhập số điện thoại tìm kiếm" Width="100%"></telerik:RadTextBox>
                    </div>
                    <div class="col-sm-3">
                        <telerik:RadTextBox ID="tbNoiDung" runat="server" EmptyMessage="Nhập nội dung cần tìm kiếm" Width="100%"></telerik:RadTextBox>
                    </div>
                </div>
                <div class="row" style="margin-top: 15px;">
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
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false"></Selecting>
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
                    <telerik:GridBoundColumn DataField="TEN_NGUOI_NHAN" FilterControlAltText="Filter TEN_NGUOI_NHAN column" HeaderText="Tên người nhận" SortExpression="TEN_NGUOI_NHAN" UniqueName="TEN_NGUOI_NHAN" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="160px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="TEN_LOP" FilterControlAltText="Filter TEN_LOP column" HeaderText="Tên lớp" SortExpression="TEN_LOP" UniqueName="TEN_LOP" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="SDT_NHAN" FilterControlAltText="Filter SDT_NHAN column" HeaderText="SĐT nhận tin" SortExpression="SDT_NHAN" UniqueName="SDT_NHAN" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn HeaderText="Nội dung" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" UniqueName="NOI_DUNG_KHONG_DAU" HeaderStyle-Width="300px">
                        <ItemTemplate>
                            <%--<%#localAPI.catChuoi(Eval("NOI_DUNG_KHONG_DAU").ToString(), 100) %>

                            <image src="../img/read_more.png" data-toggle="modal" data-target='<%# "#modal"+ Container.DataSetIndex+1 %>' style='<%#"cursor: pointer;display:" +(Eval("NOI_DUNG_KHONG_DAU").ToString().Length>100?"unset": "none")  %>'></image>

                            <div id='<%# "modal"+ Container.DataSetIndex+1 %>' class="modal fade bd-example-modal-sm" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
                                <div class="modal-dialog modal-sm">
                                    <div class="modal-content">
                                        <%#  Eval("NOI_DUNG_KHONG_DAU") %>
                                    </div>
                                </div>
                            </div>--%>
                            <%#Eval("NOI_DUNG_KHONG_DAU").ToString()%>
                            <asp:HiddenField ID="hdNoiDung" runat="server" Value='<%# Eval("NOI_DUNG_KHONG_DAU") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="SO_TIN" FilterControlAltText="Filter SO_TIN column" HeaderText="Số tin" SortExpression="SO_TIN" UniqueName="SO_TIN" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="LOAI_TIN" FilterControlAltText="Filter LOAI_TIN column" HeaderText="Loại tin" SortExpression="LOAI_TIN" UniqueName="LOAI_TIN" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="140px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="TRANG_THAI" FilterControlAltText="Filter TRANG_THAI column" HeaderText="Trạng thái" SortExpression="TRANG_THAI" UniqueName="TRANG_THAI" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="THOI_GIAN_GUI" FilterControlAltText="Filter THOI_GIAN_GUI column" HeaderText="Thời gian gửi" SortExpression="THOI_GIAN_GUI" UniqueName="THOI_GIAN_GUI" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="NGUOI_GUI" FilterControlAltText="Filter NGUOI_GUI column" HeaderText="Người gửi" SortExpression="NGUOI_GUI" UniqueName="NGUOI_GUI" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="LOAI_NHA_MANG" FilterControlAltText="Filter LOAI_NHA_MANG column" HeaderText="Nhà mạng" SortExpression="LOAI_NHA_MANG" UniqueName="LOAI_NHA_MANG" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CP" FilterControlAltText="Filter CP column" HeaderText="Kênh" SortExpression="CP" UniqueName="CP" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RES_CODE" FilterControlAltText="Filter RES_CODE column" HeaderText="Mã code" SortExpression="RES_CODE" UniqueName="RES_CODE" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="180px">
                    </telerik:GridBoundColumn>
                </Columns>
                <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
            </MasterTableView>
        </telerik:RadGrid>
    </div>
</asp:Content>
