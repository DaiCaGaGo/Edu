<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="NhanTinDiemTongHop.aspx.cs" Inherits="CMS.SMS.NhanTinDiemTongHop" %>

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
            <telerik:AjaxSetting AjaxControlID="rcbKhoi">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbLop" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="lblSDT_GV" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbLop">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblSDT_GV" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="cboGuiGVCN">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tbNoiDung" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
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

            $(document).on("keyup", ".nd-nxhn", function (event) {

                var prevTd = $(this).closest('td').prev();
                var nextTd = $(this).closest('td').next().find('.view-length').first();
                if ($(this).val().length == 0) $(nextTd).html('');
                else {
                    var strprevTd = $(prevTd).html();
                    if (strprevTd == "&nbsp;") strprevTd = "";
                    //Khong cho phep nhap qua ky tu
                    var max_len = 306 - (strprevTd.length + (strprevTd.length > 0 ? 1 : 0));
                    if ($(this).val().length > max_len) {
                        $(this).val($(this).val().substr(0, max_len));
                        notification('warning', 'Nội dung nhận xét không nhập quá 306 ký tự');
                    }
                    //End Khong cho phep nhap qua ky tu
                    $(nextTd).html($(this).val().length);
                    var countchar = $(this).val().length;
                    if (countchar > 160) {
                        $(nextTd).css('background', 'red');
                        $(nextTd).css('color', 'white');
                    }
                    else {
                        $(nextTd).css('background', 'none');
                        $(nextTd).css('color', 'black');
                    }
                }
            });
            $(document).on("keydown", ".nd-nxhn", function (event) {
                var code = event.keyCode || event.which;
                if (code == 9) {
                    JumCellDivInTD(this, 9);
                    return false;
                }
            });
        </script>
    </telerik:RadCodeBlock>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-md-4 col-sm-6 col-xs-12">
                <span class="item-title">Tổng hợp điểm theo ngày</span>
            </div>
            <div class="col-md-8 col-sm-6 col-xs-12 text-right">
                <asp:Button runat="server" ID="btnTongHop" CssClass="btn bt-one" OnClick="btnTongHop_Click" Text="Tổng hợp" OnClientClick="if (confirm('Bạn có chắc chắn muốn tổng hợp lại nội dung nhận xét hàng ngày?')) return true; else return false;" />
                <asp:Button runat="server" ID="btnGui" CssClass="btn bt-one" Text="Gửi tin" OnClick="btnGui_Click" OnClientClick="if (confirm('Bạn có chắc chắn muốn gửi tin?')) return true; else return false;" />
                <asp:Button ID="btnGuiLai" runat="server" CssClass="btn bt-one" Text="Cập nhật trạng thái gửi lại" OnClientClick="if (confirm('Bạn có chắc chắn muốn cập nhật lại trạng thái cho phép gửi lại tin nhắn?')) return true; else return false;" OnClick="btnGuiLai_Click" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="form-horizontal" role="form">
                <div class="row note-form">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <label class="content-note-form"><span style="font-weight: bold">Chú ý:</span> Dòng có màu đỏ: học sinh không đăng ký dịch vụ SMS; xanh: con GV; (*BM): đăng ký gửi cả bố mẹ; (*M): miễn phí SMS</label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-3">
                        <telerik:RadComboBox ID="rcbKhoi" runat="server" Width="100%" DataSourceID="objKhoiHoc" DataTextField="TEN" DataValueField="MA" Filter="Contains" OnSelectedIndexChanged="rcbKhoi_SelectedIndexChanged" AutoPostBack="true">
                        </telerik:RadComboBox>
                        <asp:ObjectDataSource ID="objKhoiHoc" runat="server" SelectMethod="getKhoiByCapHoc" TypeName="OneEduDataAccess.BO.KhoiBO">
                            <SelectParameters>
                                <asp:Parameter Name="maLoaiLopGDTX" DefaultValue="" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </div>
                    <div class="col-sm-3">
                        <telerik:RadComboBox ID="rcbLop" runat="server" Width="100%" DataSourceID="objLop"
                            DataTextField="TEN" DataValueField="ID" Filter="Contains" OnSelectedIndexChanged="rcbLop_SelectedIndexChanged" AutoPostBack="true">
                        </telerik:RadComboBox>
                        <asp:ObjectDataSource ID="objLop" runat="server" SelectMethod="getLopByKhoiNamHoc" TypeName="OneEduDataAccess.BO.LopBO">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="rcbKhoi" Name="maKhoi" PropertyName="SelectedValue" />
                                <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                                <asp:Parameter DefaultValue="" Name="id_all" Type="Int64" />
                                <asp:Parameter Name="text_all" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </div>
                    <div class="col-sm-3">
                        <telerik:RadDatePicker Culture="vi-VN" RenderMode="Classic" ID="rdNgay" runat="server" Width="100%" MinDate="1900/1/1"
                            Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="Giá trị không hợp lệ"
                            Calendar-FastNavigationSettings-TodayButtonCaption="Hôm nay"
                            DatePopupButton-ToolTip="Chọn ngày"
                            Calendar-FastNavigationSettings-OkButtonCaption="Chọn"
                            Calendar-FastNavigationSettings-CancelButtonCaption="Hủy">
                            <DateInput DateFormat="dd/MM/yyyy"></DateInput>
                        </telerik:RadDatePicker>
                    </div>
                    <div class="col-sm-3">
                        <telerik:RadDatePicker Culture="vi-VN" RenderMode="Classic" ID="rdDenNgay" runat="server" Width="100%" MinDate="1900/1/1"
                            Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="Giá trị không hợp lệ"
                            Calendar-FastNavigationSettings-TodayButtonCaption="Hôm nay"
                            DatePopupButton-ToolTip="Chọn ngày"
                            Calendar-FastNavigationSettings-OkButtonCaption="Chọn"
                            Calendar-FastNavigationSettings-CancelButtonCaption="Hủy">
                            <DateInput DateFormat="dd/MM/yyyy"></DateInput>
                        </telerik:RadDatePicker>
                    </div>
                </div>
                <div class="row" style="margin-top: 15px;">
                    <div class="col-sm-3">
                        <div class="one-checkbox">
                            <table>
                                <tr>
                                    <td>
                                        <label>
                                            <asp:CheckBox ID="cboGuiGVCN" runat="server" Text="Gửi GVCN" /></label>
                                        <asp:HiddenField ID="hdID_GVCN" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblSDT_GV"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="col-sm-9">
                        <asp:TextBox ID="tbNoiDung" ClientIDMode="Static" runat="server" placeholder="Nhập nội dung gửi GVCN" CssClass="form-control text-box"></asp:TextBox>
                    </div>
                </div>
                <div class="row" style="margin-top: 15px;">
                    <span>
                        <span class="progress-description" style="color: red;">
                            <asp:Literal ID="lblTongTinConNam" runat="server" Text=""></asp:Literal></span>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <span class="progress-description" style="color: red;">
                            <asp:Literal ID="lblTongTinConThang" runat="server" Text=""></asp:Literal></span>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <span class="progress-description" style="color: red;">
                            <asp:Literal ID="lblTongTinSuDung" runat="server" Text=""></asp:Literal></span>
                    </span>
                </div>
            </div>
        </div>
        <div class="item-data">
            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" GroupPanelPosition="Top" OnNeedDataSource="RadGrid1_NeedDataSource" OnItemDataBound="RadGrid1_ItemDataBound"
                AutoGenerateColumns="False" PageSize="100" AllowMultiRowSelection="True">
                <ClientSettings>
                    <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true"></Selecting>
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>
                </ClientSettings>
                <MasterTableView DataKeyNames="ID_HS" ClientDataKeyNames="ID_HS">
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
                        <telerik:GridBoundColumn DataField="MA_KHOI" FilterControlAltText="Filter MA_KHOI column" HeaderText="MA_KHOI" SortExpression="MA_KHOI" UniqueName="MA_KHOI" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ID_LOP" FilterControlAltText="Filter ID_LOP column" HeaderText="ID_LOP" SortExpression="ID_LOP" UniqueName="ID_LOP" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="IS_GUI_BO_ME" FilterControlAltText="Filter IS_GUI_BO_ME column" HeaderText="IS_GUI_BO_ME" SortExpression="IS_GUI_BO_ME" UniqueName="IS_GUI_BO_ME" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="IS_DK_KY1" FilterControlAltText="Filter IS_DK_KY1 column" HeaderText="IS_DK_KY1" SortExpression="IS_DK_KY1" UniqueName="IS_DK_KY1" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="IS_DK_KY2" FilterControlAltText="Filter ID column" HeaderText="IS_DK_KY2" SortExpression="IS_DK_KY2" UniqueName="IS_DK_KY2" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="IS_MIEN_GIAM_KY1" FilterControlAltText="Filter IS_MIEN_GIAM_KY1 column" HeaderText="IS_MIEN_GIAM_KY1" SortExpression="IS_MIEN_GIAM_KY1" UniqueName="IS_MIEN_GIAM_KY1" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="IS_MIEN_GIAM_KY2" FilterControlAltText="Filter IS_MIEN_GIAM_KY2 column" HeaderText="IS_MIEN_GIAM_KY2" SortExpression="IS_MIEN_GIAM_KY2" UniqueName="IS_MIEN_GIAM_KY2" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="IS_CON_GV" FilterControlAltText="Filter IS_CON_GV column" HeaderText="IS_CON_GV" SortExpression="IS_CON_GV" UniqueName="IS_CON_GV" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SDT" FilterControlAltText="Filter SDT column" HeaderText="SDT" SortExpression="SDT" UniqueName="SDT" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SDT_KHAC" FilterControlAltText="Filter SDT_KHAC column" HeaderText="SDT_KHAC" SortExpression="SDT_KHAC" UniqueName="SDT_KHAC" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ID_NHAN_XET_HN" FilterControlAltText="Filter ID_NHAN_XET_HN column" HeaderText="ID_NHAN_XET_HN" SortExpression="ID_NHAN_XET_HN" UniqueName="ID_NHAN_XET_HN" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TEN_LAST" FilterControlAltText="Filter TEN_LAST column" HeaderText="TEN_LAST" SortExpression="TEN_LAST" UniqueName="TEN_LAST" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="IS_SEND" FilterControlAltText="Filter IS_SEND column" HeaderText="IS_SEND" SortExpression="IS_SEND" UniqueName="IS_SEND" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ID_MON_HOC_TRUONG" FilterControlAltText="Filter ID_MON_HOC_TRUONG column" HeaderText="ID_MON_HOC_TRUONG" SortExpression="ID_MON_HOC_TRUONG" UniqueName="ID_MON_HOC_TRUONG" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MA_HS" FilterControlAltText="Filter MA_HS column" HeaderText="Mã HS" SortExpression="MA_HS" UniqueName="MA_HS" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="140px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TEN_HS" FilterControlAltText="Filter TEN_HS column" HeaderText="Họ tên" SortExpression="TEN_HS" UniqueName="TEN_HS" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="250px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SDT_BM" FilterControlAltText="Filter SDT_BM column" HeaderText="SĐT nhận tin" SortExpression="SDT_BM" UniqueName="SDT_BM" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Nội dung nhận xét" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NOI_DUNG_NX" ItemStyle-CssClass="grid-control">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNoiDung" runat="server" Text='<%# Eval("NOI_DUNG_NX") %>' CssClass="form-control text-box nd-nxhn" Width="100%" Height="100%"></asp:TextBox>
                                <asp:HiddenField ID="hdNoiDung" runat="server" Value='<%# Eval("NOI_DUNG_NX") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Đếm ký tự" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="CountLength" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <span class="view-length"><%# Eval("NOI_DUNG_NX")==null?"":Eval("NOI_DUNG_NX").ToString().Length==0?"" : Eval("NOI_DUNG_NX").ToString().Length.ToString() %></span>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Trạng thái" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="TRANG_THAI_GUI">
                            <ItemTemplate>
                                <img src="../img/cho_gui.jpg" id="chuaGui" runat="server" style="cursor: pointer; width: 24px;" title="Chưa gửi" />
                                <img src="../img/success.png" id="daGui" runat="server" style="cursor: pointer; width: 24px;" title="Đã gửi" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
