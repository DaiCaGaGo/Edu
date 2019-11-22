<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="NhanXetHangNgayHS.aspx.cs" Inherits="CMS.NhanXetHangNgay.NhanXetHangNgayHS" %>

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
                    <telerik:AjaxUpdatedControl ControlID="cboGuiGVCN" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="tbNoiDungChen" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbLop">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblSDT_GV" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="cboGuiGVCN" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="tbNoiDungChen" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rdNgay">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="cbHenGioGuiTin">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divTime" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btSave">
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
            grid = $find("<%= RadGrid1.ClientID %>");
            function pageload() {
                grid = $find("<%= RadGrid1.ClientID %>");
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
                    $(nextTd).html($(this).val().length + strprevTd.length + (strprevTd.length > 0 ? 1 : 0));
                    var countchar = $(this).val().length + strprevTd.length + (strprevTd.length > 0 ? 1 : 0);
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
            function btnRemoveClick() {
                grid = $find("<%=RadGrid1.ClientID%>");
                var noiDungChen = $('#tbNoiDungChen').val();
                var countCheck = (grid == null && grid.get_masterTableView().get_selectedItems() == null) ? 0 : grid.get_masterTableView().get_selectedItems().length;
                if (countCheck > 0) {
                    for (var i = 0; i < countCheck; i++) {
                        var item = grid.get_masterTableView().get_selectedItems()[i];
                        var noiDungNX = $(item.get_cell("NOI_DUNG_NX")).find('.nd-nxhn');
                        var tienTo = $(item.get_cell("TIEN_TO")).html();
                        if (tienTo == "&nbsp;") tienTo = "";
                        else tienTo = tienTo + " ";

                        var isDisabled = $(noiDungNX).prop('disabled');
                        if (!isDisabled) {
                            var noiDung_new = '';
                            if (noiDungChen != '') noiDung_new = $(noiDungNX).val().replace(noiDungChen, '').trim();
                            $(noiDungNX).val(noiDung_new);

                            var nextTd = $(noiDungNX).closest('td').next().find('.view-length').first();
                            $(nextTd).html($(noiDungNX).val().length + tienTo.length);

                            //báo đỏ nếu vượt quá 160 ký tự
                            if ($(noiDungNX).val().length + tienTo.length > 160) {
                                $(nextTd).css('color', 'white');
                                $(nextTd).css('background', 'red');
                            }
                            else {
                                $(nextTd).css('background', 'none');
                                $(nextTd).css('color', 'black');
                            }
                        }
                    }
                }
                else {
                    for (var i = 0; i < grid.get_masterTableView().get_dataItems().length; i++) {
                        var item = grid.get_masterTableView().get_dataItems()[i];
                        var noiDungNX = $(item.get_cell("NOI_DUNG_NX")).find('.nd-nxhn');

                        var isDisabled = $(noiDungNX).prop('disabled');
                        if (!isDisabled) {
                            var noiDung_new = '';
                            if (noiDungChen != '') noiDung_new = $(noiDungNX).val().replace(noiDungChen, '').trim();
                            $(noiDungNX).val(noiDung_new);

                            var tienTo = $(item.get_cell("TIEN_TO")).html();
                            if (tienTo == "&nbsp;") tienTo = "";
                            else tienTo = tienTo + " ";

                            var nextTd = $(noiDungNX).closest('td').next().find('.view-length').first();
                            $(nextTd).html($(noiDungNX).val().length + tienTo.length);

                            //báo đỏ nếu vượt quá 160 ký tự
                            if ($(noiDungNX).val().length + tienTo.length > 160) {
                                $(nextTd).css('color', 'white');
                                $(nextTd).css('background', 'red');
                            }
                            else {
                                $(nextTd).css('background', 'none');
                                $(nextTd).css('color', 'black');
                            }
                        }
                    }
                }
            }
            function btnChenClick(sender, args) {
                grid = $find("<%=RadGrid1.ClientID%>");
                var max_len = 306;
                var noiDungChen = $('#tbNoiDungChen').val();
                if (noiDungChen == "") {
                    alert("Vui lòng nhập nội dung cần chèn!");
                    return;
                }
                var countCheck = (grid == null && grid.get_masterTableView().get_selectedItems() == null) ? 0 : grid.get_masterTableView().get_selectedItems().length;
                var chenTruoc = $('#rbtChenTruoc').is(':checked');
                var chenSau = $('#rbtChenSau').is(':checked');
                if (countCheck > 0) {
                    for (var i = 0; i < countCheck; i++) {
                        var item = grid.get_masterTableView().get_selectedItems()[i];
                        var noiDungNX = $(item.get_cell("NOI_DUNG_NX")).find('.nd-nxhn');
                        var tienTo = $(item.get_cell("TIEN_TO")).html();
                        if (tienTo == "&nbsp;") tienTo = "";
                        else tienTo = tienTo + " ";
                        var isDisabled = $(noiDungNX).prop('disabled');
                        if (!isDisabled) {
                            if (chenTruoc) {
                                if ($(noiDungNX).val().length > 0) $(noiDungNX).val(noiDungChen + " " + $(noiDungNX).val());
                                else $(noiDungNX).val(noiDungChen);
                            }
                            else if (chenSau) {
                                if ($(noiDungNX).val().length > 0) $(noiDungNX).val($(noiDungNX).val() + " " + noiDungChen);
                                else $(noiDungNX).val(noiDungChen);
                            }

                            //if ($(noiDungNX).val().length > max_len) {
                            //    // cắt chỉ lấy đến 306 ký tự
                            //    $(noiDungNX).val($(noiDungNX).val().substr(0, max_len));
                            //}
                            var nextTd = $(noiDungNX).closest('td').next().find('.view-length').first();
                            if ($(noiDungNX).val().length == 0) $(nextTd).html('');
                            else $(nextTd).html($(noiDungNX).val().length + tienTo.length);

                            if ($(noiDungNX).val().length + tienTo.length > max_len) {
                                // cắt chỉ lấy đến 306 ký tự
                                $(noiDungNX).val($(noiDungNX).val().substr(0, max_len - tienTo.length));
                                $(nextTd).html(max_len);
                            }

                            //báo đỏ nếu vượt quá 160 ký tự
                            if ($(noiDungNX).val().length + tienTo.length > 160) {
                                $(nextTd).css('color', 'white');
                                $(nextTd).css('background', 'red');
                            }
                            else {
                                $(nextTd).css('background', 'none');
                                $(nextTd).css('color', 'black');
                            }
                        }
                    }
                } else {
                    for (var i = 0; i < grid.get_masterTableView().get_dataItems().length; i++) {
                        var item = grid.get_masterTableView().get_dataItems()[i];
                        var noiDungNX = $(item.get_cell("NOI_DUNG_NX")).find('.nd-nxhn');
                        var tienTo = $(item.get_cell("TIEN_TO")).html();
                        if (tienTo == "&nbsp;") tienTo = "";
                        else tienTo = tienTo + " ";
                        var isDisabled = $(noiDungNX).prop('disabled');
                        if (!isDisabled) {
                            if (chenTruoc) {
                                if ($(noiDungNX).val().length > 0) $(noiDungNX).val(noiDungChen + " " + $(noiDungNX).val());
                                else $(noiDungNX).val(noiDungChen);
                            }
                            else if (chenSau) {
                                if ($(noiDungNX).val().length > 0) $(noiDungNX).val($(noiDungNX).val() + " " + noiDungChen);
                                else $(noiDungNX).val(noiDungChen);
                            }

                            var nextTd = $(noiDungNX).closest('td').next().find('.view-length').first();
                            if ($(noiDungNX).val().length == 0) $(nextTd).html('');
                            else $(nextTd).html($(noiDungNX).val().length + tienTo.length);

                            if ($(noiDungNX).val().length + tienTo.length > max_len) {
                                $(noiDungNX).val($(noiDungNX).val().substr(0, max_len - tienTo.length));
                                $(nextTd).html(max_len);
                            }

                            //báo đỏ nếu vượt quá 160 ký tự
                            if ($(noiDungNX).val().length + tienTo.length > 160) {
                                $(nextTd).css('color', 'white');
                                $(nextTd).css('background', 'red');
                            }
                            else {
                                $(nextTd).css('background', 'none');
                                $(nextTd).css('color', 'black');
                            }
                        }
                    }
                }
            }
            function btnXoaClick(sender, args) {
                grid = $find("<%=RadGrid1.ClientID%>");
                var countCheck = (grid == null && grid.get_masterTableView().get_selectedItems() == null) ? 0 : grid.get_masterTableView().get_selectedItems().length;
                if (countCheck > 0) {
                    for (var i = 0; i < countCheck; i++) {
                        var item = grid.get_masterTableView().get_selectedItems()[i];
                        var isSend = $(item.get_cell("IS_SEND")).html();
                        if (isSend != 1) {
                            var noiDungNX = $(item.get_cell("NOI_DUNG_NX")).find('.nd-nxhn');
                            $(noiDungNX).val('');

                            var nextTd = $(noiDungNX).closest('td').next().find('.view-length').first();
                            $(nextTd).html('');
                        }
                    }
                }
                else {
                    for (var i = 0; i < grid.get_masterTableView().get_dataItems().length; i++) {
                        var item = grid.get_masterTableView().get_dataItems()[i];
                        var isSend = $(item.get_cell("IS_SEND")).html();
                        if (isSend != 1) {
                            var noiDungNX = $(item.get_cell("NOI_DUNG_NX")).find('.nd-nxhn');
                            $(noiDungNX).val('');

                            var nextTd = $(noiDungNX).closest('td').next().find('.view-length').first();
                            $(nextTd).html('');
                        }
                    }
                }
            }
            function change(el) {
                var max_len = 306;
                if (el.value.length > max_len) {
                    el.value = el.value.substr(0, max_len);
                }
                document.getElementById('numberCharConfirm').innerHTML = el.value.length;
                if (el.value.length > 160) {
                    document.getElementById("numberCharConfirm").style.background = "red";
                    document.getElementById("numberCharConfirm").style.color = "white";
                }
                else {
                    document.getElementById("numberCharConfirm").style.background = "none";
                    document.getElementById("numberCharConfirm").style.color = "black";
                }
                if (el.value.length > 0 && el.value.length <= 160) {
                    document.getElementById('numberSMSConfirm').innerHTML = '1';
                } else if (el.value.length > 160 && el.value.length < 307) {
                    document.getElementById('numberSMSConfirm').innerHTML = '2';
                }
            }
            function btLuuClick() {
                if (confirm("Bạn chắc chắn muốn lưu?")) {
                    return true;
                }
            }
            function btGuiLaiClick() {
                grid = $find("<%=RadGrid1.ClientID%>");
                if (grid.get_masterTableView().get_selectedItems().length > 0) {
                    if (confirm("Bạn có chắc chắn muốn cập nhật trạng thái gửi lại tin nhắn?")) {
                        return true;
                    }
                } else {
                    alert("Mời bạn chọn học sinh cần cập nhật lại trạng thái gửi tin.");
                    return false;
                }
            }
        </script>
    </telerik:RadCodeBlock>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">NHẬP THEO DÕI HÀNG NGÀY</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btSave" runat="server" CssClass="btn bt-one" OnClick="btSave_Click" Text="Lưu" OnClientClick="if(!btLuuClick()) return false;" />
                <asp:Button runat="server" ID="btnGui" CssClass="btn bt-one" Text="Gửi tin" OnClientClick="if (confirm('Bạn có chắc chắn muốn gửi tin?')) return true; else return false;" OnClick="btnGui_Click" />
                <asp:Button ID="btnGuiLai" runat="server" CssClass="btn bt-one" OnClick="btnGuiLai_Click" Text="Cập nhật trạng thái gửi lại" OnClientClick="if(!btGuiLaiClick()) return false;" />
                <asp:Button ID="btExport" runat="server" CssClass="btn bt-one" OnClick="btExport_Click" Text="Xuất excel" />
                <input type="button" id="btnXoa" class="btn bt-one" value="Xóa nội dung" onclick="btnXoaClick()" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="form-horizontal" role="form">
                <div class="row note-form">
                    <div class="col-sm-12">
                        <label class="content-note-form"><span style="font-weight: bold">Chú ý:</span> Để nhập nhanh gợi ý sẽ xuất hiện khi bạn nhập các ký tự như <span style="color: red">"@,."</span> . Bạn dùng các phím lên, xuống để lựa chọn gợi ý mình mong muốn và <span style="color: red">Enter</span>. Nhấn <span style="color: red">Tab</span> để xuống dòng.</label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2">
                        <div class="form-group">
                            <div class="col-sm-12">
                                <telerik:RadComboBox ID="rcbKhoi" runat="server" Width="100%" DataSourceID="objKhoiHoc" DataTextField="TEN" DataValueField="MA" Filter="Contains" OnSelectedIndexChanged="rcbKhoi_SelectedIndexChanged" AutoPostBack="true">
                                </telerik:RadComboBox>
                                <asp:ObjectDataSource ID="objKhoiHoc" runat="server" SelectMethod="getKhoiByCapHoc" TypeName="OneEduDataAccess.BO.KhoiBO">
                                    <SelectParameters>
                                        <asp:Parameter Name="maLoaiLopGDTX" DefaultValue="" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-2">
                        <div class="form-group">
                            <div class="col-sm-12">
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
                        </div>
                    </div>
                    <div class="col-sm-2">
                        <div class="form-group">
                            <div class="col-sm-12">
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
                    </div>
                    <div class="col-sm-4">
                        <div class="form-group">
                            <div class="one-checkbox col-sm-3">
                                <label>
                                    <asp:CheckBox ID="cbHenGioGuiTin" runat="server" Text="Hẹn Giờ" OnCheckedChanged="cbHenGioGuiTin_CheckedChanged" AutoPostBack="true" />
                                </label>
                            </div>
                            <div class="col-sm-9" id="divTime" runat="server" visible="false">
                                <asp:TextBox ID="tbTime" runat="server" CssClass="form-control text-box nd-nx-nl" TextMode="DateTimeLocal"></asp:TextBox>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <div class="form-group">
                            <div class="col-sm-12">
                                <asp:TextBox ID="tbNoiDungChen" ClientIDMode="Static" runat="server" placeholder="Nhập nội dung" CssClass="form-control text-box ndc" TextMode="MultiLine" Rows="2" onkeyup="change(this);" onkeydown="change(this);" onchange="change(this);"></asp:TextBox>
                                <span style="">Nội dung tin nhắn (<span id="numberCharConfirm">0</span> ký tự) (<span id="numberSMSConfirm">0</span>/2 tin)</span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-2">
                        <div class="form-group">
                            <div class="one-checkbox col-sm-12">
                                <table>
                                    <tr>
                                        <td>
                                            <label>
                                                <asp:CheckBox ID="cboGuiGVCN" runat="server" Text="Gửi GVCN" Enabled="false" Checked="true" /></label>
                                            <asp:HiddenField ID="hdID_GVCN" runat="server" />
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblSDT_GV"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="form-group">
                            <div class="one-checkbox col-sm-12">
                                <label>
                                    <asp:RadioButton ID="rbtChenTruoc" ClientIDMode="Static" runat="server" Text="Chèn trước" GroupName="is_chen" Checked="true" />
                                    &nbsp;&nbsp;
                                    <asp:RadioButton ID="rbtChenSau" ClientIDMode="Static" runat="server" Text="Chèn sau" GroupName="is_chen" />
                                </label>
                                &nbsp;&nbsp;
                                <input type="button" id="btnChen" class="btn bt-infolg" value="Chèn" onclick="btnChenClick()" />
                                <input type="button" id="btnRemove" class="btn bt-infolg" value="Xóa ND chọn" onclick="btnRemoveClick()" />
                            </div>

                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-sm-6">
                        <div class="form-group" style="margin-bottom: 0">
                            <div class="col-sm-12">
                                <span class="progress-description" style="color: red;">
                                    <asp:Literal ID="lblTongTinConNam" runat="server" Text=""></asp:Literal>
                                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:Literal ID="lblTongTinConThang" runat="server" Text=""></asp:Literal>
                                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:Literal ID="lblTongTinSuDung" runat="server" Text=""></asp:Literal>
                                </span>
                            </div>
                        </div>
                    </div>

                </div>

            </div>
        </div>
        <div class="item-data">
            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" GroupPanelPosition="Top" OnNeedDataSource="RadGrid1_NeedDataSource"
                AutoGenerateColumns="False" PageSize="100" AllowMultiRowSelection="True" OnItemDataBound="RadGrid1_ItemDataBound">
                <ClientSettings>
                    <Selecting AllowRowSelect="true"></Selecting>
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
                        <telerik:GridBoundColumn DataField="IS_SEND" FilterControlAltText="Filter IS_SEND column" HeaderText="IS_SEND" SortExpression="IS_SEND" UniqueName="IS_SEND" HeaderStyle-HorizontalAlign="Center" Display="false">
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
                        <telerik:GridBoundColumn DataField="MA_HS" FilterControlAltText="Filter MA_HS column" HeaderText="Mã HS" SortExpression="MA_HS" UniqueName="MA_HS" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="140px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TEN_LAST" FilterControlAltText="Filter TEN_LAST column" HeaderText="TEN_LAST" SortExpression="TEN_LAST" UniqueName="TEN_LAST" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TEN_HS" FilterControlAltText="Filter TEN_HS column" HeaderText="Họ tên" SortExpression="TEN_HS" UniqueName="TEN_HS" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="220px">
                        </telerik:GridBoundColumn>
                        <%--<telerik:GridBoundColumn DataField="NGAY_SINH" FilterControlAltText="Filter NGAY_SINH column" HeaderText="Ngày sinh" SortExpression="NGAY_SINH" UniqueName="NGAY_SINH" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-Width="100px">
                        </telerik:GridBoundColumn>--%>
                        <telerik:GridBoundColumn DataField="SDT_BM" FilterControlAltText="Filter SDT_BM column" HeaderText="SĐT nhận tin" SortExpression="SDT_BM" UniqueName="SDT_BM" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px">
                        </telerik:GridBoundColumn>
                        <%--<telerik:GridTemplateColumn HeaderText="Mạng" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="TELCO" ItemStyle-CssClass="grid-control" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <%# localAPI.getLoaiNhaMang(Eval("SDT")==null ?"":Eval("SDT").ToString()) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridBoundColumn DataField="MANG" FilterControlAltText="Filter MANG column" HeaderText="Mạng" SortExpression="MANG" UniqueName="MANG" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TIEN_TO" FilterControlAltText="Filter TIEN_TO column" HeaderText="Tiền tố" SortExpression="TIEN_TO" UniqueName="TIEN_TO" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Nội dung nhận xét" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NOI_DUNG_NX" ItemStyle-CssClass="grid-control">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNoiDung" runat="server" Text='<%# Eval("NOI_DUNG_NX") %>' CssClass="form-control text-box nd-nxhn" MaxLength="306" TextMode="MultiLine" Rows="2" Width="100%" Height="100%"></asp:TextBox>
                                <asp:HiddenField ID="hdNoiDung" runat="server" Value='<%# Eval("NOI_DUNG_NX") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Đếm ký tự" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="CountLength" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <span class="view-length" id="lblCountChar"><%# Eval("NOI_DUNG_NX")==null?"":Eval("NOI_DUNG_NX").ToString().Length==0?"" : (Eval("NOI_DUNG_NX").ToString().Length +
                                   (Eval("TIEN_TO")==null?0: Eval("TIEN_TO").ToString().Length) +
                                   (Eval("TIEN_TO")==null || Eval("TIEN_TO").ToString().Length==0?0:1)).ToString() %></span>
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
    <script>
        Sys.Application.add_load(function () {
            createAutocomplateMaNX("nd-nxhn", "../DataMaNXHN.aspx");
        });
    </script>
</asp:Content>
