<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="SuatAnDetail.aspx.cs" Inherits="CMS.DinhDuong.SuatAnDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="rcbKhoi" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="rcbBuaAn" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="rcbThucDon" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbKhoi">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbBuaAn" />
                    <telerik:AjaxUpdatedControl ControlID="tbSoHocSinhDK" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbBuaAn">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="hdfSoTien1HS" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="tbGia" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="rcbThucDon" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="tbSoHocSinhDK" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rdNgay">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tbSoHocSinhDK" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
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
                    <telerik:AjaxUpdatedControl ControlID="tbTongNangLuong" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="tbProtid" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="tbGlucid" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="tbLipid" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbThucDon">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="tbSoHocSinhDK" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="hdfSoTien1HS" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="tbGia" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="tbSoHocSinhDK">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="tbGia" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true" OnClientClose="RadWindowManager1_OnClientClose">
        <Windows>
            <telerik:RadWindow RenderMode="Lightweight" ID="RadWindow1" runat="server" ShowContentDuringLoad="false" Width="400px"
                Height="400px" Title="Chi tiết suất ăn" Behaviors="Reload,Close,Maximize" Modal="true" VisibleStatusbar="false">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
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
            function text_changed(strText) {
                var soLuongHS = strText.value;
                var gia1HS = document.getElementById('<%= hdfSoTien1HS.ClientID %>');
                var tongGia = document.getElementById('<%= tbGia.ClientID %>');
                if (gia1HS.value != "" && soLuongHS != "") {
                    tongGia.value = soLuongHS * gia1HS.value;
                }
            }
            function changeSoLuong() {
                grid = $find("<%=RadGrid1.ClientID%>");
                var count = grid.get_masterTableView().get_dataItems().length - 2;
                var out_nang_luong = 0, out_protid = 0, out_lipid = 0, out_glucid = 0;
                for (var i = 0; i < grid.get_masterTableView().get_dataItems().length - 2; i++) {
                    var item = grid.get_masterTableView().get_dataItems()[i];
                    var soLuong = $(item.get_cell("SO_LUONG")).find('.so-luong').val();

                    var nangLuong_new = $(item.get_cell("NANG_LUONG_KCAL")).find('.nang-luong');
                    var protid_new = $(item.get_cell("PROTID")).find('.protid');
                    var glucid_new = $(item.get_cell("GLUCID")).find('.glucid');
                    var lipid_new = $(item.get_cell("LIPID")).find('.lipid');

                    var nangLuong_old = $(item.get_cell("NANG_LUONG_KCAL_OLD")).find('.nang-luong-old').val();
                    var protid_old = $(item.get_cell("PROTID_OLD")).find('.protid-old').val();
                    var glucid_old = $(item.get_cell("GLUCID_OLD")).find('.glucid-old').val();
                    var lipid_old = $(item.get_cell("LIPID_OLD")).find('.lipid-old').val();

                    var tong_nang_luong = 0, tong_protid = 0, tong_glucid = 0, tong_lipid = 0;
                    tong_nang_luong = soLuong * nangLuong_old;
                    tong_protid = soLuong * protid_old;
                    tong_glucid = soLuong * glucid_old;
                    tong_lipid = soLuong * lipid_old;
                    //alert($(nangLuong_new).val(tong_nang_luong));
                    if (tong_nang_luong > 0) $(nangLuong_new).val(tong_nang_luong.toFixed(1)); else $(nangLuong_new).val('');
                    if (tong_protid > 0) $(protid_new).val(tong_protid.toFixed(1)); else $(protid_new).val('');
                    if (tong_glucid > 0) $(glucid_new).val(tong_glucid.toFixed(1)); else $(glucid_new).val('');
                    if (tong_lipid > 0) $(lipid_new).val(tong_lipid.toFixed(1)); else $(lipid_new).val('');

                    out_nang_luong += tong_nang_luong;
                    out_protid += tong_protid;
                    out_lipid += tong_lipid;
                    out_glucid += tong_glucid;

                }
                //tính năng lượng tổng
                var itemSum = grid.get_masterTableView().get_dataItems()[count];
                var sum_nang_luong = $(itemSum.get_cell("NANG_LUONG_KCAL")).find('.nang-luong');
                var sum_protid = $(itemSum.get_cell("PROTID")).find('.protid');
                var sum_glucid = $(itemSum.get_cell("GLUCID")).find('.glucid');
                var sum_lipid = $(itemSum.get_cell("LIPID")).find('.lipid');

                if (out_nang_luong > 0) $(sum_nang_luong).val(out_nang_luong.toFixed(1)); else $(sum_nang_luong).val('');
                if (out_protid > 0) $(sum_protid).val(out_protid.toFixed(1)); else $(sum_protid).val('');
                if (out_lipid > 0) $(sum_lipid).val(out_lipid.toFixed(1)); else $(sum_lipid).val('');
                if (out_glucid > 0) $(sum_glucid).val(out_glucid.toFixed(1)); else $(sum_glucid).val('');

                //nếu thành phần dinh dưỡng chưa đạt chuẩn thì có màu đỏ

                var hdNangLuongTu = document.getElementById('<%= hdNangLuongTu.ClientID %>');
                var hdNangLuongDen = document.getElementById('<%= hdNangLuongDen.ClientID %>');
                var hdProtidTu = document.getElementById('<%= hdProtidTu.ClientID %>');
                var hdProtidDen = document.getElementById('<%= hdProtidDen.ClientID %>');
                var hdGlucidTu = document.getElementById('<%= hdGlucidTu.ClientID %>');
                var hdGlucidDen = document.getElementById('<%= hdGlucidDen.ClientID %>');
                var hdLipidTu = document.getElementById('<%= hdLipidTu.ClientID %>');
                var hdLipidDen = document.getElementById('<%= hdLipidDen.ClientID %>');

                if (out_nang_luong < hdNangLuongTu.value || out_nang_luong > hdNangLuongDen.value)
                    $(sum_nang_luong).css('color', 'red');
                else $(sum_nang_luong).css('color', 'black');

                if (out_protid < hdProtidTu.value || out_protid > hdProtidDen.value)
                    $(sum_protid).css('color', 'red');
                else $(sum_protid).css('color', 'black');

                if (out_lipid < hdLipidTu.value || out_lipid > hdLipidDen.value)
                    $(sum_lipid).css('color', 'red');
                else $(sum_lipid).css('color', 'black');

                if (out_glucid < hdGlucidTu.value || out_glucid > hdGlucidDen.value)
                    $(sum_glucid).css('color', 'red');
                else $(sum_glucid).css('color', 'black');


            }
        </script>
    </telerik:RadCodeBlock>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">CHI TIẾT SUẤT ĂN</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btAdd" runat="server" CssClass="btn bt-one" Text="Thêm" OnClick="btAdd_Click" />
                <asp:Button ID="btEdit" runat="server" CssClass="btn bt-one" Text="Sửa" OnClick="btEdit_Click" />
                <input type="button" id="tbnThemThucPham" runat="server" class="btn bt-one" value="Thêm thực phẩm" onclick="openRadWin('SuatAn_ThemThucPham.aspx', 100, 50)" />
                <asp:Button ID="btDeleteChon" runat="server" CssClass="btn bt-one" OnClick="btDeleteChon_Click" Text="Xóa thực phẩm" OnClientClick="if(!btDeteleClick()) return false;" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="form-horizontal" role="form">
                <div class="row">
                    <div class="col-sm-4">
                        <div class="form-group">
                            <label for="rcbKhoi" class="col-sm-5 control-label">Khối <span style="color: red">(*)</span></label>
                            <div class="col-sm-7">
                                <telerik:RadComboBox ID="rcbKhoi" runat="server" Width="100%" DataTextField="TEN" DataValueField="MA" Filter="Contains" CausesValidation="false" DataSourceID="objKhoi" OnSelectedIndexChanged="rcbKhoi_SelectedIndexChanged" AutoPostBack="True">
                                </telerik:RadComboBox>
                                <asp:ObjectDataSource ID="objKhoi" runat="server" SelectMethod="getKhoiByCapHoc" TypeName="OneEduDataAccess.BO.KhoiBO">
                                    <SelectParameters>
                                        <asp:Parameter Name="maLoaiLopGDTX" DefaultValue="" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="rcbKhoi"
                                    CssClass="valid-control" ForeColor="Red" ErrorMessage="Thông tin bắt buộc nhập." Display="Dynamic"
                                    SetFocusOnError="true"> </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="rcbBuaAn" class="col-sm-5 control-label">Bữa ăn <span style="color: red">(*)</span></label>
                            <div class="col-sm-7">
                                <telerik:RadComboBox ID="rcbBuaAn" runat="server" Width="100%" DataSourceID="objBuaAn"
                                    DataTextField="TEN" DataValueField="ID" Filter="Contains" OnSelectedIndexChanged="rcbBuaAn_SelectedIndexChanged" AutoPostBack="true">
                                </telerik:RadComboBox>
                                <asp:ObjectDataSource ID="objBuaAn" runat="server" SelectMethod="getBuaAnByTruongKhoi" TypeName="OneEduDataAccess.BO.DMBuaAnBO">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="rcbKhoi" Name="id_khoi" PropertyName="SelectedValue" />
                                        <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                                        <asp:Parameter DefaultValue="" Name="id_all" Type="Int64" />
                                        <asp:Parameter Name="text_all" Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="rcbBuaAn"
                                    CssClass="valid-control" ForeColor="Red" ErrorMessage="Thông tin bắt buộc nhập." Display="Dynamic"
                                    SetFocusOnError="true"> </asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="form-group">
                            <label for="rcbThucDon" class="col-sm-5 control-label">Thực đơn</label>
                            <div class="col-sm-7">
                                <telerik:RadComboBox ID="rcbThucDon" runat="server" Width="100%" DataSourceID="objThucDon"
                                    DataTextField="TEN" DataValueField="ID" EmptyMessage="Chọn thực đơn" AllowCustomText="true" Filter="Contains" OnSelectedIndexChanged="rcbThucDon_SelectedIndexChanged" AutoPostBack="true">
                                </telerik:RadComboBox>
                                <asp:ObjectDataSource ID="objThucDon" runat="server" SelectMethod="getThucDonByTruongKhoiAndBuaAn" TypeName="OneEduDataAccess.BO.ThucDonBO">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="rcbKhoi" Name="id_khoi" PropertyName="SelectedValue" />
                                        <asp:Parameter Name="id_nhom_tuoi_mn" DefaultValue="" />
                                        <asp:ControlParameter ControlID="rcbBuaAn" Name="id_bua_an" PropertyName="SelectedValue" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbSoHocSinhDK" class="col-sm-5 control-label">Số HS đăng ký</label>
                            <%--<div class="col-sm-7">
                                <asp:TextBox ID="tbSoHocSinhDK" CssClass="form-control" ClientIDMode="Static" placeholder="Số học sinh đăng ký" runat="server" MaxLength="4" TextMode="Number" onchange="text_changed(this)"></asp:TextBox>
                            </div>--%>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbSoHocSinhDK" CssClass="form-control" ClientIDMode="Static" placeholder="Số học sinh đăng ký" runat="server" MaxLength="4" TextMode="Number" OnTextChanged="tbSoHocSinhDK_TextChanged" AutoPostBack="true"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="form-group">
                            <label for="rdNgay" class="col-sm-5 control-label">Ngày ăn</label>
                            <div class="col-sm-7">
                                <telerik:RadDatePicker Culture="vi-VN" RenderMode="Classic" ID="rdNgay" runat="server" Width="100%" MinDate="1900/1/1"
                                    Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="Giá trị không hợp lệ"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="Hôm nay"
                                    DatePopupButton-ToolTip="Chọn ngày"
                                    Calendar-FastNavigationSettings-OkButtonCaption="Chọn"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="Hủy" OnSelectedDateChanged="rdNgay_SelectedDateChanged" AutoPostBack="true">
                                    <DateInput DateFormat="dd/MM/yyyy"></DateInput>
                                </telerik:RadDatePicker>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbGia" class="col-sm-5 control-label">Giá tiền (VNĐ)</label>
                            <div class="col-sm-7">
                                <telerik:RadNumericTextBox ID="tbGia" CssClass="form-control" ClientIDMode="Static" placeholder="Giá" runat="server" MinValue="0" MaxValue="1000000000" MaxLength="10" Width="100%"></telerik:RadNumericTextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="item-data">
                <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" GroupPanelPosition="Top" OnNeedDataSource="RadGrid1_NeedDataSource" OnItemDataBound="RadGrid1_ItemDataBound" OnPreRender="RadGrid1_PreRender"
                    AutoGenerateColumns="False" PageSize="100" AllowMultiRowSelection="True">
                    <ClientSettings>
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <MasterTableView>
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
                            <telerik:GridColumnGroup HeaderText="Thành phần dinh dưỡng (Kcal)" Name="TPDD" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        </ColumnGroups>
                        <Columns>
                            <telerik:GridClientSelectColumn UniqueName="chkChon" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            </telerik:GridClientSelectColumn>
                            <telerik:GridTemplateColumn HeaderText="STT" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" UniqueName="STT">
                                <ItemTemplate>
                                    <%# Container.DataSetIndex+1 %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="ID_SUAT_AN" FilterControlAltText="Filter ID_SUAT_AN column" HeaderText="ID_SUAT_AN" SortExpression="ID_SUAT_AN" UniqueName="ID_SUAT_AN" HeaderStyle-HorizontalAlign="Center" Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ID_NHOM_THUC_PHAM" FilterControlAltText="Filter ID_NHOM_THUC_PHAM column" HeaderText="ID_NHOM_THUC_PHAM" SortExpression="ID_NHOM_THUC_PHAM" UniqueName="ID_NHOM_THUC_PHAM" HeaderStyle-HorizontalAlign="Center" Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ID_THUC_PHAM" FilterControlAltText="Filter ID_THUC_PHAM column" HeaderText="ID_THUC_PHAM" SortExpression="ID_THUC_PHAM" UniqueName="ID_THUC_PHAM" HeaderStyle-HorizontalAlign="Center" Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="IS_NANG_LUONG_CHUAN" FilterControlAltText="Filter IS_NANG_LUONG_CHUAN column" HeaderText="IS_NANG_LUONG_CHUAN" SortExpression="IS_NANG_LUONG_CHUAN" UniqueName="IS_NANG_LUONG_CHUAN" HeaderStyle-HorizontalAlign="Center" Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="Năng lượng" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NANG_LUONG_KCAL_OLD" Display="false">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbNANG_LUONG_KCAL_OLD" runat="server" Text='<%# Eval("NANG_LUONG_KCAL_OLD") %>' CssClass="form-control text-box nang-luong-old" Enabled="false"></asp:TextBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Protid" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="PROTID_OLD" Display="false">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbPROTID_OLD" runat="server" Text='<%# Eval("PROTID_OLD") %>' CssClass="form-control text-box protid-old" Enabled="false"></asp:TextBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Glucid" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="GLUCID_OLD" Display="false">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbGLUCID_OLD" runat="server" Text='<%# Eval("GLUCID_OLD") %>' CssClass="form-control text-box glucid-old" Enabled="false"></asp:TextBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Lipid" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="LIPID_OLD" Display="false">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbLIPID_OLD" runat="server" Text='<%# Eval("LIPID_OLD") %>' CssClass="form-control text-box lipid-old" Enabled="false"></asp:TextBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="DON_VI_TINH" FilterControlAltText="Filter DON_VI_TINH column" HeaderText="Đơn vị tính" SortExpression="DON_VI_TINH" UniqueName="DON_VI_TINH" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="TEN_NHOM_THUC_PHAM" FilterControlAltText="Filter TEN_NHOM_THUC_PHAM column" HeaderText="Tên nhóm" SortExpression="TEN_NHOM_THUC_PHAM" UniqueName="TEN_NHOM_THUC_PHAM" HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="TEN_THUC_PHAM" FilterControlAltText="Filter TEN_THUC_PHAM column" HeaderText="Tên thực phẩm" SortExpression="TEN_THUC_PHAM" UniqueName="TEN_THUC_PHAM" HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="DON_VI" FilterControlAltText="Filter DON_VI column" HeaderText="Đơn vị tính" SortExpression="DON_VI" UniqueName="DON_VI" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="Số lượng" HeaderStyle-Width="100px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="SO_LUONG">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbSO_LUONG" runat="server" Text='<%# Eval("SO_LUONG") %>' CssClass="form-control text-box so-luong" onchange="changeSoLuong()" Style="text-align: center;"></asp:TextBox>
                                    <asp:HiddenField ID="hdSO_LUONG" runat="server" Value='<%# Eval("SO_LUONG") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Năng lượng" ColumnGroupName="TPDD" HeaderStyle-Width="150px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NANG_LUONG_KCAL">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbNANG_LUONG_KCAL" ColumnGroupName="TPDD" runat="server" Text='<%# Eval("NANG_LUONG_KCAL") %>' CssClass="form-control text-box nang-luong" Style="text-align: center"></asp:TextBox>
                                    <asp:HiddenField ID="hdNANG_LUONG_KCAL" runat="server" Value='<%# Eval("NANG_LUONG_KCAL") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Protid" ColumnGroupName="TPDD" HeaderStyle-Width="150px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="PROTID">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbPROTID" runat="server" Text='<%# Eval("PROTID") %>' CssClass="form-control text-box protid" Style="text-align: center"></asp:TextBox>
                                    <asp:HiddenField ID="hdPROTID" runat="server" Value='<%# Eval("PROTID") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Glucid" ColumnGroupName="TPDD" HeaderStyle-Width="150px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="GLUCID">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbGLUCID" runat="server" Text='<%# Eval("GLUCID") %>' CssClass="form-control text-box glucid" Style="text-align: center"></asp:TextBox>
                                    <asp:HiddenField ID="hdGLUCID" runat="server" Value='<%# Eval("GLUCID") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Lipid" ColumnGroupName="TPDD" HeaderStyle-Width="150px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="LIPID">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbLIPID" runat="server" Text='<%# Eval("LIPID") %>' CssClass="form-control text-box lipid" Style="text-align: center"></asp:TextBox>
                                    <asp:HiddenField ID="hdLIPID" runat="server" Value='<%# Eval("LIPID") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="IS_NANG_LUONG_SUAT_AN" FilterControlAltText="Filter IS_NANG_LUONG_SUAT_AN column" HeaderText="IS_NANG_LUONG_SUAT_AN" SortExpression="IS_NANG_LUONG_SUAT_AN" UniqueName="IS_NANG_LUONG_SUAT_AN" HeaderStyle-HorizontalAlign="Center" Display="false">
                            </telerik:GridBoundColumn>
                        </Columns>
                        <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdfSoTien1HS" runat="server" Value="" />
    <asp:HiddenField ID="hdNangLuongTu" runat="server" Value="0" />
    <asp:HiddenField ID="hdNangLuongDen" runat="server" Value="0" />
    <asp:HiddenField ID="hdProtidTu" runat="server" Value="0" />
    <asp:HiddenField ID="hdProtidDen" runat="server" Value="0" />
    <asp:HiddenField ID="hdGlucidTu" runat="server" Value="0" />
    <asp:HiddenField ID="hdGlucidDen" runat="server" Value="0" />
    <asp:HiddenField ID="hdLipidTu" runat="server" Value="0" />
    <asp:HiddenField ID="hdLipidDen" runat="server" Value="0" />
</asp:Content>
