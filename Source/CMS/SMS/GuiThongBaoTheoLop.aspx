<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="GuiThongBaoTheoLop.aspx.cs" Inherits="CMS.SMS.GuiThongBaoTheoLop" %>

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
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbLop">
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
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <telerik:RadCodeBlock runat="server">
        <script>
            var grid;
            function btGuiTuyChonClick() {
                grid = $find("<%=RadGrid1.ClientID%>");
                if (grid.get_masterTableView().get_selectedItems().length > 0) {
                    if (confirm("Bạn có chắc chắn muốn gửi tin?")) {
                        return true;
                    }
                } else {
                    alert("Bạn phải chọn học sinh cần gửi tin!");
                    return false;
                }
            }
        </script>
    </telerik:RadCodeBlock>

    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">GỬI TIN NHẮN THÔNG BÁO</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btGuiTuyChon" runat="server" CssClass="btn bt-one" Text="Gửi SMS chọn lọc" OnClientClick="if(!btGuiTuyChonClick()) return false;" OnClick="btGuiTuyChon_Click" />
                <asp:Button ID="btnGuiAll" runat="server" CssClass="btn bt-one" Text="Gửi toàn danh sách" OnClientClick="if(!btLuuClick()) return false;" OnClick="btnGuiAll_Click" />
                <asp:Button ID="btGuiSoDinhKem" runat="server" CssClass="btn bt-one" Text="Gửi SMS đính kèm" OnClientClick="if(!btLuuClick()) return false;" OnClick="btGuiSoDinhKem_Click" />
                <asp:LinkButton runat="server" ID="btnSmsTuyBien" href="\SMS\SMSImportExcel.aspx" CssClass="btn bt-one" Font-Size="17px">Gửi SMS tùy biến</asp:LinkButton>
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter" style="padding-bottom: 0">
            <div class="form-horizontal" role="form">
                <div class="row note-form">
                    <div class="col-sm-12">
                        <label class="content-note-form"><span style="font-weight: bold">Chú ý:</span> Dòng có màu đỏ: học sinh không đăng ký dịch vụ SMS; xanh: con GV; (*BM): đăng ký gửi cả bố mẹ; (*M): miễn phí SMS</label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2">
                        <telerik:RadComboBox ID="rcbKhoi" runat="server" Width="100%" DataSourceID="objKhoiHoc" DataTextField="TEN" DataValueField="MA" Filter="Contains" OnSelectedIndexChanged="rcbKhoi_SelectedIndexChanged" AutoPostBack="true">
                        </telerik:RadComboBox>
                        <asp:ObjectDataSource ID="objKhoiHoc" runat="server" SelectMethod="getKhoiByCapHoc" TypeName="OneEduDataAccess.BO.KhoiBO">
                            <SelectParameters>
                                <asp:Parameter Name="maLoaiLopGDTX" DefaultValue="" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </div>
                    <div class="col-sm-2">
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
                    <div class="one-checkbox col-sm-4">
                        <label>
                            <asp:CheckBox ID="cboGuiGVCN" runat="server" Text="Gửi GVCN" Checked="true" />
                        </label>&nbsp;&nbsp;&nbsp;
                        <label>
                            <asp:CheckBox ID="cboGuiZalo" runat="server" Text="Gửi Zalo" /></label>
                    </div>
                    <div class="col-sm-4">
                        <div class="row">
                            <div class="one-checkbox col-md-3 col-sm-5">
                                <label>
                                    <asp:CheckBox ID="cbHenGioGuiTin" runat="server" Text="Hẹn Giờ" OnCheckedChanged="cbHenGioGuiTin_CheckedChanged" AutoPostBack="true" />
                                </label>
                            </div>
                            <div class="col-md-9 col-sm-7" runat="server" id="divTime" visible="false">
                                <asp:TextBox ID="tbTime" runat="server" CssClass="form-control text-box nd-nx-nl" TextMode="DateTimeLocal"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row" style="margin-top: 15px;">
                    <div class="col-sm-6">
                        <asp:TextBox ID="tbNoiDung" ClientIDMode="Static" runat="server" placeholder="Nhập nội dung tin nhắn vào đây" CssClass="form-control text-box nd-nx-nl" TextMode="MultiLine" Rows="3" onkeyup="change(this);" onkeydown="change(this);" onchange="change(this);"></asp:TextBox>
                        <span style="position: absolute; padding-top: 5px">Nội dung tin nhắn (<span id="numberCharConfirm">0</span> ký tự) (<span id="numberSMSConfirm">0</span>/3 tin)</span>
                    </div>
                    <div class="col-sm-2">
                        <div class="row">
                            <div class="one-checkbox col-sm-12">
                                <label>
                                    <asp:CheckBox ID="rcbChenThem" ClientIDMode="Static" runat="server" Text="Chèn thêm" />
                                </label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-6">
                                <input type="button" id="btCopy" class="btn bt-infolg" value="Copy" onclick="btCopyClick()" />
                            </div>
                            <div class="col-sm-6">
                                <input type="button" id="btnRemove" class="btn bt-infolg" value="Xóa" onclick="btnRemoveClick()" />
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <asp:TextBox ID="tbListSDT" ClientIDMode="Static" runat="server" placeholder="Nhập SĐT gửi kèm" CssClass="form-control text-box nd-nx-nl" TextMode="MultiLine" Rows="3"></asp:TextBox>
                        <span style="position: absolute; padding-top: 5px">(Không nhập quá 10 SĐT, giữa các số ngăn cách bởi dấu <span style="color: red">;</span>)</span>
                        <asp:HiddenField ID="hdSDT_GuiKem" runat="server" />
                    </div>
                </div>
                <div class="row" style="margin-top: 15px;">
                    <div class="col-sm-12">
                        <div class="form-group">
                            <div class="col-sm-12" style="margin: 5px 0 -10px 0">
                                <span class="progress-description" style="color: red;">
                                    <asp:Label runat="server" ID="txtTongQuyTinConLaiTheoNam"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label runat="server" ID="txtTongQuyTinConLaiTheoThang"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                    <span>Dự tính số tin sẽ sử dụng: <span id="numberConfirm">0</span></span>
                                    <asp:Label runat="server" ID="lbTinSuDung" Font-Size="25px"></asp:Label>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="item-data">
            <%--<asp:HiddenField ID="hdTongHeSo" runat="server" ClientIDMode="Static" />--%>
            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="true" GroupPanelPosition="Top" OnNeedDataSource="RadGrid1_NeedDataSource" OnItemDataBound="RadGrid1_ItemDataBound"
                AutoGenerateColumns="False" PageSize="500" AllowMultiRowSelection="True">
                <ClientSettings>
                    <Selecting AllowRowSelect="true"></Selecting>
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>
                    <ClientEvents OnRowSelected="RowSelected" OnRowDeselected="RowDeselected" />
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
                        <telerik:GridClientSelectColumn UniqueName="chkChon" ItemStyle-CssClass="chbChon" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridClientSelectColumn>
                        <telerik:GridTemplateColumn HeaderText="STT" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="STT">
                            <ItemTemplate>
                                <%# Container.DataSetIndex+1 %>
                                <%--<asp:HiddenField ID="hdHeSo" Value='<%# Eval("HE_SO") %>' ClientIDMode="Static" runat="server" />--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--<telerik:GridBoundColumn DataField="HE_SO" FilterControlAltText="Filter HE_SO column" HeaderText="HE_SO" SortExpression="HE_SO" UniqueName="HE_SO" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>--%>
                        <telerik:GridBoundColumn DataField="HO_TEN" FilterControlAltText="Filter HO_TEN column" HeaderText="Tên học sinh" SortExpression="HO_TEN" UniqueName="HO_TEN" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TEN" FilterControlAltText="Filter TEN column" HeaderText="TEN" SortExpression="TEN" UniqueName="TEN" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <%--<telerik:GridBoundColumn DataField="NGAY_SINH" FilterControlAltText="Filter NGAY_SINH column" HeaderText="Ngày sinh" SortExpression="NGAY_SINH" UniqueName="NGAY_SINH" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-Width="100px">
                        </telerik:GridBoundColumn>--%>
                        <telerik:GridBoundColumn DataField="ID_LOP" FilterControlAltText="Filter ID_LOP column" HeaderText="Mã Lớp" SortExpression="ID_LOP" UniqueName="ID_LOP" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TEN_LOP" FilterControlAltText="Filter TEN_LOP column" HeaderText="Lớp" SortExpression="TEN_LOP" UniqueName="TEN_LOP" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="70px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SDT_NHAN_TIN" FilterControlAltText="Filter SDT_NHAN_TIN column" HeaderText="Số điện thoại" SortExpression="SDT_NHAN_TIN" UniqueName="SDT_NHAN_TIN" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SDT_NHAN_TIN2" FilterControlAltText="Filter SDT_NHAN_TIN2 column" HeaderText="SDT_NHAN_TIN2" SortExpression="SDT_NHAN_TIN2" UniqueName="SDT_NHAN_TIN2" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SDT_BM" FilterControlAltText="Filter SDT_BM column" HeaderText="SĐT nhận tin" SortExpression="SDT_BM" UniqueName="SDT_BM" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="140px">
                        </telerik:GridBoundColumn>
                        <%-- <telerik:GridBoundColumn DataField="MANG" FilterControlAltText="Filter MANG column" HeaderText="Mạng" SortExpression="MANG" UniqueName="MANG" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="140px">
                        </telerik:GridBoundColumn>--%>
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
                        <telerik:GridTemplateColumn HeaderText="Nội dung thông báo" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NOI_DUNG_TB" ItemStyle-CssClass="grid-control">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNoiDungTB" runat="server" CssClass="form-control text-box nd-nx-nl" TextMode="MultiLine" Rows="2" ClientIDMode="Static" onkeyup="changeNoiDungRow(this);" onkeydown="changeNoiDungRow(this);" onchange="changeNoiDungRow(this);" MaxLength="459"></asp:TextBox>
                                <asp:HiddenField ID="hdNoiDungTB" runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Đếm ký tự" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="CountLength" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <span class="view-length">0</span>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="SO_TIN_TRONG_NGAY" FilterControlAltText="Filter SO_TIN_TRONG_NGAY column" HeaderText="Số tin đã dùng trong ngày" SortExpression="SO_TIN_TRONG_NGAY" UniqueName="SO_TIN_TRONG_NGAY" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="140px">
                        </telerik:GridBoundColumn>
                    </Columns>
                    <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>

    <telerik:RadCodeBlock runat="server">
        <script>
            var grid;
            function pageload() {
                grid = $find("<%=RadGrid1.ClientID%>");
            }
            function btLuuClick() {
                if (confirm("Bạn chắc chắn muốn gửi tin?")) {
                    return true;
                }
            }
            function change(el) {
                var max_len = 459;
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
                } else if (el.value.length > 160 && el.value.length <= 306) {
                    document.getElementById('numberSMSConfirm').innerHTML = '2';
                } else if (el.value.length > 306 && el.value.length < 460) {
                    document.getElementById('numberSMSConfirm').innerHTML = '3';
                }

                var str;
                if (eval(el))
                    str = eval(el).value;
                else
                    str = el;
                str = str.replace(/á|à|ạ|ả|ã|â|ấ|ầ|ậ|ẩ|ẫ|ă|ắ|ằ|ặ|ẳ|ẵ/g, "a");
                str = str.replace(/Á|À|Ạ|Ả|Ã|Â|Ấ|Ầ|Ậ|Ẩ|Ẫ|Ă|Ắ|Ằ|Ặ|Ẳ|Ẵ/g, "A");
                str = str.replace(/é|è|ẹ|ẻ|ẽ|ê|ế|ề|ệ|ể|ễ/g, "e");
                str = str.replace(/É|È|Ẹ|Ẻ|Ẽ|Ê|Ế|Ề|Ệ|Ể|Ễ/g, "E");
                str = str.replace(/í|ì|ị|ỉ|ĩ/g, "i");
                str = str.replace(/Í|Ì|Ị|Ỉ|Ĩ/g, "I");
                str = str.replace(/ó|ò|ọ|ỏ|õ|ô|ố|ồ|ộ|ổ|ỗ|ơ|ớ|ờ|ợ|ở|ỡ/g, "o");
                str = str.replace(/Ó|Ò|Ọ|Ỏ|Õ|Ô|Ố|Ồ|Ộ|Ổ|Ỗ|Ơ|Ớ|Ờ|Ợ|Ở|Ỡ/g, "O");
                str = str.replace(/ú|ù|ụ|ủ|ũ|ư|ứ|ừ|ự|ử|ữ/g, "u");
                str = str.replace(/Ú|Ù|Ụ|Ủ|Ũ|Ư|Ứ|Ừ|Ự|Ử|Ữ/g, "U");
                str = str.replace(/ý|ỳ|ỵ|ỷ|ỹ/g, "y");
                str = str.replace(/Ý|Ỳ|Ỵ|Ỷ|Ỹ/g, "Y");
                str = str.replace(/đ|₫/g, "d");
                str = str.replace(/Đ/g, "D");

                str = str.replace("–", "-")
                .replace("‘", "'")
                .replace("’", "'")
                .replace("“", "\"")
                .replace("”", "\"")
                    .replace("》", ">");

                eval(el).value = str;

                countSMSDuTinh();
            }
            function countSMSByText(value) {
                var count = value.length;
                if (count > 0 && count <= 160) {
                    return 1;
                } else if (count > 160 && count <= 306) {
                    return 2;
                } else if (count > 306 && count < 460) return 3;
                return 0;
            }
            var cell;
            function countSMSDuTinh() {
                grid = $find("<%=RadGrid1.ClientID%>");
                var soTinSuDung = 0;
                var countCheck = (grid == null && grid.get_masterTableView().get_selectedItems() == null) ? 0 : grid.get_masterTableView().get_selectedItems().length;
                if (countCheck == 0) {
                    for (var i = 0; i < grid.get_masterTableView().get_dataItems().length; i++) {
                        var item = grid.get_masterTableView().get_dataItems()[i];
                        var cellHeSo = $(item.get_cell("HE_SO")).text();
                        var cellNOI_DUNG_TB = $(item.get_cell("NOI_DUNG_TB"));
                        var cellNoiDung = (cellNOI_DUNG_TB).find('.nd-nx-nl').val();
                        soTinSuDung += cellHeSo * countSMSByText(cellNoiDung);
                    }
                } else {
                    for (var i = 0; i < countCheck; i++) {
                        var item = grid.get_masterTableView().get_selectedItems()[i];
                        var cellHeSo = $(item.get_cell("HE_SO")).text();
                        var cellNOI_DUNG_TB = $(item.get_cell("NOI_DUNG_TB"));
                        var cellNoiDung = (cellNOI_DUNG_TB).find('.nd-nx-nl').val();
                        soTinSuDung += cellHeSo * countSMSByText(cellNoiDung);
                    }
                }
                document.getElementById('numberConfirm').innerHTML = soTinSuDung;
            }
            function countSMSDuTinhNew() {

                document.getElementById('numberConfirm').innerHTML = "0";
            }
            function btnRemoveClick() {
                grid = $find("<%=RadGrid1.ClientID%>");
                var noiDungChen = $('#tbNoiDung').val();
                var countCheck = (grid == null && grid.get_masterTableView().get_selectedItems() == null) ? 0 : grid.get_masterTableView().get_selectedItems().length;
                if (countCheck > 0) {
                    for (var i = 0; i < countCheck; i++) {
                        var item = grid.get_masterTableView().get_selectedItems()[i];
                        var noiDungNX = $(item.get_cell("NOI_DUNG_TB")).find('.nd-nx-nl');
                        var noiDung_new = $(noiDungNX).val().replace(noiDungChen, '').trim();
                        $(noiDungNX).val(noiDung_new);

                        var nextTd = $(noiDungNX).closest('td').next().find('.view-length').first();
                        $(nextTd).html($(noiDungNX).val().length);

                        if ($(noiDungNX).val().length > 160) {
                            $(nextTd).css('color', 'white');
                            $(nextTd).css('background', 'red');
                        }
                        else {
                            $(nextTd).css('background', 'none');
                            $(nextTd).css('color', 'black');
                        }
                    }
                }
                else {
                    for (var i = 0; i < grid.get_masterTableView().get_dataItems().length; i++) {
                        var item = grid.get_masterTableView().get_dataItems()[i];
                        var noiDungNX = $(item.get_cell("NOI_DUNG_TB")).find('.nd-nx-nl');
                        var noiDung_new = $(noiDungNX).val().replace(noiDungChen, '').trim();
                        console.log(noiDung_new);
                        $(noiDungNX).val(noiDung_new);

                        var nextTd = $(noiDungNX).closest('td').next().find('.view-length').first();
                        $(nextTd).html($(noiDungNX).val().length);

                        if ($(noiDungNX).val().length > 160) {
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
            function btCopyClick(sender, args) {
                grid = $find("<%=RadGrid1.ClientID%>");
                var max_len = 459;
                var ndth = $('#tbNoiDung').val();
                var countCheck = (grid == null && grid.get_masterTableView().get_selectedItems() == null) ? 0 : grid.get_masterTableView().get_selectedItems().length;
                var chenThem = $('#rcbChenThem:checked').length > 0;
                if (countCheck > 0) {
                    for (var i = 0; i < countCheck; i++) {
                        var item = grid.get_masterTableView().get_selectedItems()[i];

                        var tbndtb = $(item.get_cell("NOI_DUNG_TB")).find('.nd-nx-nl');
                        var isDisabled = $(tbndtb).prop('disabled');
                        if (!isDisabled) {
                            if (chenThem) $(tbndtb).val($(tbndtb).val() + ' ' + ndth);
                            else $(tbndtb).val(ndth);

                            if ($(tbndtb).val().length > max_len) {
                                $(tbndtb).val($(tbndtb).val().substr(0, max_len));
                            }
                            var nextTd = $(tbndtb).closest('td').next().find('.view-length').first();
                            if ($(tbndtb).val().length == 0) $(nextTd).html('');
                            else $(nextTd).html($(tbndtb).val().length);

                            //báo đỏ nếu vượt quá 160 ký tự
                            if ($(tbndtb).val().length > 160) {
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
                        var tbndtb = $(item.get_cell("NOI_DUNG_TB")).find('.nd-nx-nl');
                        var isDisabled = $(tbndtb).prop('disabled');
                        if (!isDisabled) {
                            if (chenThem) $(tbndtb).val($(tbndtb).val() + ' ' + ndth);
                            else $(tbndtb).val(ndth);

                            if ($(tbndtb).val().length > max_len) {
                                $(tbndtb).val($(tbndtb).val().substr(0, max_len));
                            }
                            var nextTd = $(tbndtb).closest('td').next().find('.view-length').first();
                            if ($(tbndtb).val().length == 0) $(nextTd).html('');
                            else $(nextTd).html($(tbndtb).val().length);

                            if ($(tbndtb).val().length > 160) {
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
                countSMSDuTinh();
            }
            function RowSelected(sender, args) {
                countSMSDuTinh();
            }
            function RowDeselected(sender, args) {
                countSMSDuTinh();
            }
            function changeNoiDungRow(sender, args) {
                countSMSDuTinh();
            }
            $(document).on("keyup", ".nd-nx-nl", function (event) {
                var max_len = 459;
                if ($(this).val().length > max_len) {
                    $(this).val($(this).val().substr(0, max_len));
                    notification('warning', 'Nội dung nhận xét không nhập quá 459 ký tự');
                }
                var nextTd = $(this).closest('td').next().find('.view-length').first();
                if ($(this).val().length == 0) $(nextTd).html('');
                else $(nextTd).html($(this).val().length);
                if ($(this).val().length > 160) {
                    $(nextTd).css('color', 'white');
                    $(nextTd).css('background', 'red');
                }
                else {
                    $(nextTd).css('background', 'none');
                    $(nextTd).css('color', 'black');
                }
                
                var str = $(this).val();
                str = str.replace(/á|à|ạ|ả|ã|â|ấ|ầ|ậ|ẩ|ẫ|ă|ắ|ằ|ặ|ẳ|ẵ/g, "a");
                str = str.replace(/Á|À|Ạ|Ả|Ã|Â|Ấ|Ầ|Ậ|Ẩ|Ẫ|Ă|Ắ|Ằ|Ặ|Ẳ|Ẵ/g, "A");
                str = str.replace(/é|è|ẹ|ẻ|ẽ|ê|ế|ề|ệ|ể|ễ/g, "e");
                str = str.replace(/É|È|Ẹ|Ẻ|Ẽ|Ê|Ế|Ề|Ệ|Ể|Ễ/g, "E");
                str = str.replace(/í|ì|ị|ỉ|ĩ/g, "i");
                str = str.replace(/Í|Ì|Ị|Ỉ|Ĩ/g, "I");
                str = str.replace(/ó|ò|ọ|ỏ|õ|ô|ố|ồ|ộ|ổ|ỗ|ơ|ớ|ờ|ợ|ở|ỡ/g, "o");
                str = str.replace(/Ó|Ò|Ọ|Ỏ|Õ|Ô|Ố|Ồ|Ộ|Ổ|Ỗ|Ơ|Ớ|Ờ|Ợ|Ở|Ỡ/g, "O");
                str = str.replace(/ú|ù|ụ|ủ|ũ|ư|ứ|ừ|ự|ử|ữ/g, "u");
                str = str.replace(/Ú|Ù|Ụ|Ủ|Ũ|Ư|Ứ|Ừ|Ự|Ử|Ữ/g, "U");
                str = str.replace(/ý|ỳ|ỵ|ỷ|ỹ/g, "y");
                str = str.replace(/Ý|Ỳ|Ỵ|Ỷ|Ỹ/g, "Y");
                str = str.replace(/đ|₫/g, "d");
                str = str.replace(/Đ/g, "D");

                str = str.replace("–", "-")
                .replace("‘", "'")
                .replace("’", "'")
                .replace("“", "\"")
                .replace("”", "\"")
                    .replace("》", ">");

                $(this).val(str);
            });
            $(document).on("keydown", ".nd-nx-nl", function (event) {
                var code = event.keyCode || event.which;
                if (code == 9) {
                    JumCellDivInTD(this, 9);
                    return false;
                }
            });
            function OnClientDropDownClosed1(sender, eventArgs) {
                <%=SetHeightForRadgrid()%>;
            }
        </script>
        <style>
            #ctl00_ContentPlaceHolder1_ctl00_ContentPlaceHolder1_txtTongQuyTinConLaiPanel {
                display: inline-block !important;
            }
        </style>
    </telerik:RadCodeBlock>
</asp:Content>
