<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="NhapTongHop.aspx.cs" Inherits="CMS.HocSinh.NhapTongHop" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            var grid;
            function pageLoad() {
                try {
                    grid = $find("<%=RadGrid1.ClientID%>");
                    setBorderRowGrid(grid, 5);
                }
                catch (ex) { }
            }
            function ProcessCellValue(cell, type) {
                try {
                    if (cell != null) {
                        var cval;
                        switch (type) {
                            case "Diem10":
                                cval = ChangeValidValue0_10(cell.value);
                                break;
                            case "DiemCD":
                                cval = ChangeValidValueD_CD(cell.value);
                                break;
                            default: cval = cell.value;
                                break;

                        }
                        if (cval != cell.value || NeedJump == true) { cell.value = cval; NeedJump = true; }
                        if (cval == '') NeedJump = false;
                        var cellIndex = $(cell).parent().index();
                        if (NeedJump) {
                            if (is_jump_col == "1" || is_jump_col == 1) {
                                nextRow(cell);
                            }
                            else {
                                nextCol(cell, cellIndex);
                            }
                        }
                    }
                    NeedJump = false;
                }
                catch (ex) { }
            }

            $(document).on("keyup", ".Diem10 .text-box", function (event) {
                if (event.which != 37 && event.which != 38 && event.which != 39 && event.which != 40) {
                    ProcessCellValue(this, "Diem10");
                } else {
                    JumCell(this, event.which);
                }
            });
            $(document).on("keyup", ".DiemCD .text-box", function (event) {
                if (event.which != 37 && event.which != 38 && event.which != 39 && event.which != 40) {
                    ProcessCellValue(this, "DiemCD");
                } else {
                    JumCell(this, event.which);
                }
            });
            $(document).on("keyup", ".nd-nxhn", function (event) {
                var max_len = 306;
                if ($(this).val().length > max_len) {
                    $(this).val($(this).val().substr(0, max_len));
                    notification('warning', 'Nội dung nhận xét không nhập quá 306 ký tự');
                }
                var nextTd = $(this).closest('td').next().find('.view-length').first();
                if ($(this).val().length == 0) $(nextTd).html('');
                else $(nextTd).html($(this).val().length);

                if ($(this).val().length > 160) {
                    $(nextTd).css('background', 'red');
                    $(nextTd).css('color', 'white');
                }
                else {
                    $(nextTd).css('background', 'none');
                    $(nextTd).css('color', 'black');
                }
            });
            function btnChenClick(sender, args) {
                grid = $find("<%=RadGrid1.ClientID%>");
                var max_len = 306;
                var noiDungChen = $('#tbNoiDungChen').val();
                var countCheck = (grid == null && grid.get_masterTableView().get_selectedItems() == null) ? 0 : grid.get_masterTableView().get_selectedItems().length;
                var chenTruoc = $('#rbtChenTruoc').is(':checked');
                var chenSau = $('#rbtChenSau').is(':checked');

                for (var i = 0; i < grid.get_masterTableView().get_dataItems().length; i++) {
                    var item = grid.get_masterTableView().get_dataItems()[i];
                    var noiDungNX = $(item.get_cell("NOI_DUNG_NX")).find('.nd-nxhn');
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

                        if ($(noiDungNX).val().length > max_len) {
                            $(noiDungNX).val() = $(noiDungNX).val().substr(0, max_len);
                        }
                        var nextTd = $(noiDungNX).closest('td').next().find('.view-length').first();
                        if ($(noiDungNX).val().length == 0) $(nextTd).html('');
                        else $(nextTd).html($(noiDungNX).val().length);

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
            function btnXoaClick(sender, args) {
                grid = $find("<%=RadGrid1.ClientID%>");
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
            function btLuuClick() {
                if (confirm("Bạn chắc chắn muốn lưu?")) {
                    return true;
                }
            }


            function btGuiTin() {
                if (confirm("Bạn chắc chắn muốn gửi tin?")) {
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
            <div class="col-sm-5">
                <span class="item-title">Nhập điểm và nhận xét hằng ngày</span>
            </div>
            <div class="col-sm-7 text-right">
                <label>
                    <input type="radio" id="chbDong" name="config" value="ngang" onchange="chbDong_Change()">Nhảy dòng</label>
                <label>
                    <input type="radio" id="chbCot" name="config" value="doc" onchange="chbCot_Change()">Nhảy cột</label>
                <asp:Button ID="btSave" runat="server" CssClass="btn bt-one" OnClick="btSave_Click" Text="Lưu" OnClientClick="if(!btLuuClick()) return false;" />
                <asp:Button ID="btnGuiTin" runat="server" CssClass="btn bt-one" OnClick="btnGuiTin_Click" Text="Gửi tin" OnClientClick="if(!btGuiTin()) return false;" />
                <asp:Button ID="btnGuiLai" runat="server" CssClass="btn bt-one" Text="Cập nhật trạng thái gửi lại" OnClientClick="if(!btGuiLaiClick()) return false;" OnClick="btnGuiLai_Click" />
                <asp:Button ID="btExport" runat="server" CssClass="btn bt-one" OnClick="btExport_Click" Text="Xuất excel" />
                <input type="button" id="btnXoa" class="btn bt-one" value="Xóa nội dung" onclick="btnXoaClick()" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="row">
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbKhoiHoc" runat="server" Width="100%" DataSourceID="objKhoiHoc"
                        DataTextField="TEN" DataValueField="MA" AutoPostBack="True"
                        OnSelectedIndexChanged="rcbKhoiHoc_SelectedIndexChanged" Filter="Contains">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objKhoiHoc" runat="server" SelectMethod="getKhoiByCapHoc" TypeName="OneEduDataAccess.BO.KhoiBO">
                        <SelectParameters>
                            <asp:Parameter Name="maLoaiLopGDTX" DefaultValue="" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbLop" runat="server" Width="100%" DataSourceID="objLop"
                        DataTextField="TEN" DataValueField="ID" Filter="Contains" AutoPostBack="true" OnSelectedIndexChanged="rcbLop_SelectedIndexChanged">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objLop" runat="server" SelectMethod="getLopByKhoiNamHoc" TypeName="OneEduDataAccess.BO.LopBO">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="rcbKhoiHoc" Name="maKhoi" PropertyName="SelectedValue" />
                            <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                            <asp:Parameter DefaultValue="" Name="id_all" Type="Int64" />
                            <asp:Parameter Name="text_all" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
                <div class="col-sm-2">
                    <button type="button" class="btn btn-infolg" data-toggle="modal" data-target="#myModal">Cấu hình môn học</button>

                    <!-- Modal -->
                    <div id="myModal" class="modal fade" role="dialog">
                        <div class="modal-dialog">

                            <!-- Modal content-->
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                    <h4 class="modal-title">Cấu hình môn học</h4>
                                </div>
                                <div class="modal-body">
                                    <asp:Repeater ID="rpMonHoc" runat="server" DataSourceID="objMonHoc" OnItemDataBound="rpMonHoc_ItemDataBound">
                                        <HeaderTemplate>
                                            <table class="table table-bordered">
                                                <tr>
                                                    <th style="text-align: center">Môn</th>
                                                    <th style="text-align: center">Miệng</th>
                                                    <th style="text-align: center">15P</th>
                                                    <th style="text-align: center" id="th1THS1" runat="server">1T HS1</th>
                                                    <th style="text-align: center">1T HS2</th>
                                                    <th style="text-align: center">Học kỳ</th>
                                                </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbName" runat="server" Text='<%# Eval("TEN") %>'></asp:Label>
                                                    <asp:HiddenField ID="hdIDMonTruong" runat="server" Value='<%# Eval("ID") %>' />
                                                    <asp:HiddenField ID="hdKieuMon" runat="server" Value='<%# Eval("KIEU_MON") %>' />
                                                </td>
                                                <td style="text-align: center">
                                                    <asp:CheckBox ID="cbM" runat="server" Text="Có" />
                                                </td>
                                                <td style="text-align: center">
                                                    <asp:CheckBox ID="cb15P" runat="server" Text="Có" />
                                                </td>
                                                <td style="text-align: center" id="td1THS1" runat="server">
                                                    <asp:CheckBox ID="cb1THS1" runat="server" Text="Có" />
                                                </td>
                                                <td style="text-align: center">
                                                    <asp:CheckBox ID="cb1THS2" runat="server" Text="Có" />
                                                </td>
                                                <td style="text-align: center">
                                                    <asp:CheckBox ID="cbHocKy" runat="server" Text="Có" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                    <asp:ObjectDataSource ID="objMonHoc" runat="server" SelectMethod="getMonTruongCauHinhByLopHocKy" TypeName="OneEduDataAccess.BO.LopMonBO">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="rcbLop" Name="id_lop" PropertyName="SelectedValue" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button ID="btConfig" runat="server" CssClass="btn bt-one" OnClick="btConfig_Click" Text="Lưu thay đổi" />
                                    <button type="button" class="btn btn-default" data-dismiss="modal">Đóng</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <span class="progress-description" style="color: red;">
                        <asp:Literal ID="lblTongTinConNam" runat="server" Text=""></asp:Literal></span>&nbsp;&nbsp;
                    <span class="progress-description" style="color: red;">
                        <asp:Literal ID="lblTongTinConThang" runat="server" Text=""></asp:Literal></span>&nbsp;&nbsp;
                    <span class="progress-description" style="color: red;">
                        <asp:Literal ID="lblTongTinSuDung" runat="server" Text=""></asp:Literal></span>
                </div>
            </div>
            <div class="row" style="margin-top: 20px;">
                <div class="col-sm-6">
                    <asp:TextBox ID="tbNoiDungChen" ClientIDMode="Static" runat="server" placeholder="Nhập nội dung" CssClass="form-control text-box ndc" TextMode="MultiLine" Rows="2"></asp:TextBox>
                </div>
                <div class="col-sm-2">
                        <div class="form-group">
                            <div class="one-checkbox col-sm-12">
                                <table style="margin-left: -15px;">
                                    <tr>
                                        <td>
                                            <label>
                                                <asp:CheckBox ID="cboGuiZalo" runat="server" Text="Gửi Zalo" /></label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>
                                                <asp:CheckBox ID="cboGuiGVCN" runat="server" Text="Gửi GVCN" Checked="true" /></label>
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
                            </div>

                        </div>
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
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Môn 1" HeaderStyle-HorizontalAlign="Center" Name="M1"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Môn 2" HeaderStyle-HorizontalAlign="Center" Name="M2"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Môn 3" HeaderStyle-HorizontalAlign="Center" Name="M3"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Môn 4" HeaderStyle-HorizontalAlign="Center" Name="M4"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Môn 5" HeaderStyle-HorizontalAlign="Center" Name="M5"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Môn 6" HeaderStyle-HorizontalAlign="Center" Name="M6"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Môn 7" HeaderStyle-HorizontalAlign="Center" Name="M7"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Môn 8" HeaderStyle-HorizontalAlign="Center" Name="M8"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Môn 9" HeaderStyle-HorizontalAlign="Center" Name="M9"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Môn 10" HeaderStyle-HorizontalAlign="Center" Name="M10"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Môn 11" HeaderStyle-HorizontalAlign="Center" Name="M11"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Môn 12" HeaderStyle-HorizontalAlign="Center" Name="M12"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Môn 13" HeaderStyle-HorizontalAlign="Center" Name="M13"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Môn 14" HeaderStyle-HorizontalAlign="Center" Name="M14"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Môn 15" HeaderStyle-HorizontalAlign="Center" Name="M15"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Môn 16" HeaderStyle-HorizontalAlign="Center" Name="M16"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Môn 17" HeaderStyle-HorizontalAlign="Center" Name="M17"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Môn 18" HeaderStyle-HorizontalAlign="Center" Name="M18"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Môn 19" HeaderStyle-HorizontalAlign="Center" Name="M19"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Môn 20" HeaderStyle-HorizontalAlign="Center" Name="M20"></telerik:GridColumnGroup>
                    </ColumnGroups>
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
                        <telerik:GridBoundColumn DataField="MA_HS" HeaderStyle-Width="140px" FilterControlAltText="Filter MA_HS column" HeaderText="Mã HS" SortExpression="MA_HS" UniqueName="MA_HS" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TEN_HS" HeaderStyle-Width="200px" FilterControlAltText="Filter TEN_HS column" HeaderText="Tên HS" SortExpression="TEN_HS" UniqueName="TEN_HS" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <%--<telerik:GridBoundColumn DataField="NGAY_SINH" HeaderStyle-Width="100px" FilterControlAltText="Filter NGAY_SINH column" HeaderText="Ngày sinh" SortExpression="NGAY_SINH" UniqueName="NGAY_SINH" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}">
                        </telerik:GridBoundColumn>--%>
                        <telerik:GridBoundColumn DataField="IS_GUI_BO_ME" FilterControlAltText="Filter IS_GUI_BO_ME column" HeaderText="IS_GUI_BO_ME" SortExpression="IS_GUI_BO_ME" UniqueName="IS_GUI_BO_ME" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TIEN_TO" FilterControlAltText="Filter TIEN_TO column" HeaderText="TIEN_TO" SortExpression="TIEN_TO" UniqueName="TIEN_TO" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="IS_SEND" FilterControlAltText="Filter IS_SEND column" HeaderText="IS_SEND" SortExpression="IS_SEND" UniqueName="IS_SEND" HeaderStyle-HorizontalAlign="Center" Display="false">
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
                        <telerik:GridBoundColumn DataField="SDT_BM" FilterControlAltText="Filter SDT_BM column" HeaderText="SĐT nhận tin" SortExpression="SDT_BM" UniqueName="SDT_BM" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px">
                        </telerik:GridBoundColumn>
                        <%--Môn 1--%>
                        <%-- Miệng --%>
                        <telerik:GridTemplateColumn HeaderText="M" ColumnGroupName="M1" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MM1">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMM1" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Miệng --%>
                        <%-- 15p --%>
                        <telerik:GridTemplateColumn HeaderText="15p" ColumnGroupName="M1" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="15pM1">
                            <ItemTemplate>
                                <asp:TextBox ID="tb15pM1" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 15p --%>
                        <%-- 1 tiết hs1 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS1" ColumnGroupName="M1" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS1M1">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS1M1" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs1 --%>
                        <%-- 1 tiết hs2 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS2" ColumnGroupName="M1" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS2M1">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS2M1" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs2 --%>
                        <%-- Học kỳ --%>
                        <telerik:GridTemplateColumn HeaderText="HK" ColumnGroupName="M1" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="HKM1">
                            <ItemTemplate>
                                <asp:TextBox ID="tbHKM1" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End học kỳ --%>
                        <%-- End Môn 1 --%>

                        <%--Môn 2--%>
                        <%-- Miệng --%>
                        <telerik:GridTemplateColumn HeaderText="M" ColumnGroupName="M2" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MM2">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMM2" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Miệng --%>
                        <%-- 15p --%>
                        <telerik:GridTemplateColumn HeaderText="15p" ColumnGroupName="M2" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="15pM2">
                            <ItemTemplate>
                                <asp:TextBox ID="tb15pM2" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 15p --%>
                        <%-- 1 tiết hs1 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS1" ColumnGroupName="M2" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS1M2">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS1M2" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs1 --%>
                        <%-- 1 tiết hs2 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS2" ColumnGroupName="M2" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS2M2">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS2M2" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs2 --%>
                        <%-- Học kỳ --%>
                        <telerik:GridTemplateColumn HeaderText="HK" ColumnGroupName="M2" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="HKM2">
                            <ItemTemplate>
                                <asp:TextBox ID="tbHKM2" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End học kỳ --%>
                        <%-- End Môn 2 --%>

                        <%--Môn 3--%>
                        <%-- Miệng --%>
                        <telerik:GridTemplateColumn HeaderText="M" ColumnGroupName="M3" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MM3">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMM3" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Miệng --%>
                        <%-- 15p --%>
                        <telerik:GridTemplateColumn HeaderText="15p" ColumnGroupName="M3" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="15pM3">
                            <ItemTemplate>
                                <asp:TextBox ID="tb15pM3" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 15p --%>
                        <%-- 1 tiết hs1 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS1" ColumnGroupName="M3" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS1M3">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS1M3" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs1 --%>
                        <%-- 1 tiết hs2 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS2" ColumnGroupName="M3" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS2M3">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS2M3" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs2 --%>
                        <%-- Học kỳ --%>
                        <telerik:GridTemplateColumn HeaderText="HK" ColumnGroupName="M3" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="HKM3">
                            <ItemTemplate>
                                <asp:TextBox ID="tbHKM3" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End học kỳ --%>
                        <%-- End Môn 3 --%>

                        <%--Môn 4--%>
                        <%-- Miệng --%>
                        <telerik:GridTemplateColumn HeaderText="M" ColumnGroupName="M4" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MM4">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMM4" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Miệng --%>
                        <%-- 15p --%>
                        <telerik:GridTemplateColumn HeaderText="15p" ColumnGroupName="M4" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="15pM4">
                            <ItemTemplate>
                                <asp:TextBox ID="tb15pM4" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 15p --%>
                        <%-- 1 tiết hs1 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS1" ColumnGroupName="M4" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS1M4">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS1M4" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs1 --%>
                        <%-- 1 tiết hs2 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS2" ColumnGroupName="M4" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS2M4">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS2M4" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs2 --%>
                        <%-- Học kỳ --%>
                        <telerik:GridTemplateColumn HeaderText="HK" ColumnGroupName="M4" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="HKM4">
                            <ItemTemplate>
                                <asp:TextBox ID="tbHKM4" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End học kỳ --%>
                        <%-- End Môn 4 --%>

                        <%--Môn 5--%>
                        <%-- Miệng --%>
                        <telerik:GridTemplateColumn HeaderText="M" ColumnGroupName="M5" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MM5">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMM5" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Miệng --%>
                        <%-- 15p --%>
                        <telerik:GridTemplateColumn HeaderText="15p" ColumnGroupName="M5" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="15pM5">
                            <ItemTemplate>
                                <asp:TextBox ID="tb15pM5" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 15p --%>
                        <%-- 1 tiết hs1 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS1" ColumnGroupName="M5" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS1M5">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS1M5" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs1 --%>
                        <%-- 1 tiết hs2 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS2" ColumnGroupName="M5" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS2M5">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS2M5" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs2 --%>
                        <%-- Học kỳ --%>
                        <telerik:GridTemplateColumn HeaderText="HK" ColumnGroupName="M5" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="HKM5">
                            <ItemTemplate>
                                <asp:TextBox ID="tbHKM5" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End học kỳ --%>
                        <%-- End Môn 5 --%>

                        <%--Môn 6--%>
                        <%-- Miệng --%>
                        <telerik:GridTemplateColumn HeaderText="M" ColumnGroupName="M6" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MM6">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMM6" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Miệng --%>
                        <%-- 15p --%>
                        <telerik:GridTemplateColumn HeaderText="15p" ColumnGroupName="M6" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="15pM6">
                            <ItemTemplate>
                                <asp:TextBox ID="tb15pM6" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 15p --%>
                        <%-- 1 tiết hs1 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS1" ColumnGroupName="M6" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS1M6">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS1M6" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs1 --%>
                        <%-- 1 tiết hs2 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS2" ColumnGroupName="M6" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS2M6">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS2M6" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs2 --%>
                        <%-- Học kỳ --%>
                        <telerik:GridTemplateColumn HeaderText="HK" ColumnGroupName="M6" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="HKM6">
                            <ItemTemplate>
                                <asp:TextBox ID="tbHKM6" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End học kỳ --%>
                        <%-- End Môn 6 --%>

                        <%--Môn 7--%>
                        <%-- Miệng --%>
                        <telerik:GridTemplateColumn HeaderText="M" ColumnGroupName="M7" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MM7">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMM7" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Miệng --%>
                        <%-- 15p --%>
                        <telerik:GridTemplateColumn HeaderText="15p" ColumnGroupName="M7" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="15pM7">
                            <ItemTemplate>
                                <asp:TextBox ID="tb15pM7" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 15p --%>
                        <%-- 1 tiết hs1 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS1" ColumnGroupName="M7" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS1M7">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS1M7" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs1 --%>
                        <%-- 1 tiết hs2 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS2" ColumnGroupName="M7" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS2M7">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS2M7" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs2 --%>
                        <%-- Học kỳ --%>
                        <telerik:GridTemplateColumn HeaderText="HK" ColumnGroupName="M7" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="HKM7">
                            <ItemTemplate>
                                <asp:TextBox ID="tbHKM7" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End học kỳ --%>
                        <%-- End Môn 7 --%>

                        <%--Môn 8--%>
                        <%-- Miệng --%>
                        <telerik:GridTemplateColumn HeaderText="M" ColumnGroupName="M8" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MM8">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMM8" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Miệng --%>
                        <%-- 15p --%>
                        <telerik:GridTemplateColumn HeaderText="15p" ColumnGroupName="M8" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="15pM8">
                            <ItemTemplate>
                                <asp:TextBox ID="tb15pM8" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 15p --%>
                        <%-- 1 tiết hs1 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS1" ColumnGroupName="M8" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS1M8">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS1M8" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs1 --%>
                        <%-- 1 tiết hs2 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS2" ColumnGroupName="M8" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS2M8">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS2M8" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs2 --%>
                        <%-- Học kỳ --%>
                        <telerik:GridTemplateColumn HeaderText="HK" ColumnGroupName="M8" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="HKM8">
                            <ItemTemplate>
                                <asp:TextBox ID="tbHKM8" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End học kỳ --%>
                        <%-- End Môn 8 --%>

                        <%--Môn 9--%>
                        <%-- Miệng --%>
                        <telerik:GridTemplateColumn HeaderText="M" ColumnGroupName="M9" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MM9">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMM9" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Miệng --%>
                        <%-- 15p --%>
                        <telerik:GridTemplateColumn HeaderText="15p" ColumnGroupName="M9" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="15pM9">
                            <ItemTemplate>
                                <asp:TextBox ID="tb15pM9" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 15p --%>
                        <%-- 1 tiết hs1 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS1" ColumnGroupName="M9" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS1M9">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS1M9" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs1 --%>
                        <%-- 1 tiết hs2 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS2" ColumnGroupName="M9" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS2M9">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS2M9" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs2 --%>
                        <%-- Học kỳ --%>
                        <telerik:GridTemplateColumn HeaderText="HK" ColumnGroupName="M9" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="HKM9">
                            <ItemTemplate>
                                <asp:TextBox ID="tbHKM9" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End học kỳ --%>
                        <%-- End Môn 9 --%>

                        <%--M10--%>
                        <%-- Miệng --%>
                        <telerik:GridTemplateColumn HeaderText="M" ColumnGroupName="M10" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MM10">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMM10" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Miệng --%>
                        <%-- 15p --%>
                        <telerik:GridTemplateColumn HeaderText="15p" ColumnGroupName="M10" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="15pM10">
                            <ItemTemplate>
                                <asp:TextBox ID="tb15pM10" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 15p --%>
                        <%-- 1 tiết hs1 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS1" ColumnGroupName="M10" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS1M10">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS1M10" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs1 --%>
                        <%-- 1 tiết hs2 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS2" ColumnGroupName="M10" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS2M10">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS2M10" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs2 --%>
                        <%-- Học kỳ --%>
                        <telerik:GridTemplateColumn HeaderText="HK" ColumnGroupName="M10" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="HKM10">
                            <ItemTemplate>
                                <asp:TextBox ID="tbHKM10" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End học kỳ --%>
                        <%-- End M10 --%>

                        <%--M11--%>
                        <%-- Miệng --%>
                        <telerik:GridTemplateColumn HeaderText="M" ColumnGroupName="M11" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MM11">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMM11" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Miệng --%>
                        <%-- 15p --%>
                        <telerik:GridTemplateColumn HeaderText="15p" ColumnGroupName="M11" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="15pM11">
                            <ItemTemplate>
                                <asp:TextBox ID="tb15pM11" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 15p --%>
                        <%-- 1 tiết hs1 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS1" ColumnGroupName="M11" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS1M11">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS1M11" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs1 --%>
                        <%-- 1 tiết hs2 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS2" ColumnGroupName="M11" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS2M11">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS2M11" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs2 --%>
                        <%-- Học kỳ --%>
                        <telerik:GridTemplateColumn HeaderText="HK" ColumnGroupName="M11" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="HKM11">
                            <ItemTemplate>
                                <asp:TextBox ID="tbHKM11" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End học kỳ --%>
                        <%-- End M11 --%>

                        <%--M12--%>
                        <%-- Miệng --%>
                        <telerik:GridTemplateColumn HeaderText="M" ColumnGroupName="M12" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MM12">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMM12" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Miệng --%>
                        <%-- 15p --%>
                        <telerik:GridTemplateColumn HeaderText="15p" ColumnGroupName="M12" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="15pM12">
                            <ItemTemplate>
                                <asp:TextBox ID="tb15pM12" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 15p --%>
                        <%-- 1 tiết hs1 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS1" ColumnGroupName="M12" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS1M12">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS1M12" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs1 --%>
                        <%-- 1 tiết hs2 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS2" ColumnGroupName="M12" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS2M12">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS2M12" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs2 --%>
                        <%-- Học kỳ --%>
                        <telerik:GridTemplateColumn HeaderText="HK" ColumnGroupName="M12" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="HKM12">
                            <ItemTemplate>
                                <asp:TextBox ID="tbHKM12" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End học kỳ --%>
                        <%-- End M12 --%>

                        <%--M13--%>
                        <%-- Miệng --%>
                        <telerik:GridTemplateColumn HeaderText="M" ColumnGroupName="M13" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MM13">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMM13" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Miệng --%>
                        <%-- 15p --%>
                        <telerik:GridTemplateColumn HeaderText="15p" ColumnGroupName="M13" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="15pM13">
                            <ItemTemplate>
                                <asp:TextBox ID="tb15pM13" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 15p --%>
                        <%-- 1 tiết hs1 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS1" ColumnGroupName="M13" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS1M13">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS1M13" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs1 --%>
                        <%-- 1 tiết hs2 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS2" ColumnGroupName="M13" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS2M13">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS2M13" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs2 --%>
                        <%-- Học kỳ --%>
                        <telerik:GridTemplateColumn HeaderText="HK" ColumnGroupName="M13" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="HKM13">
                            <ItemTemplate>
                                <asp:TextBox ID="tbHKM13" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End học kỳ --%>
                        <%-- End M13 --%>

                        <%--M14--%>
                        <%-- Miệng --%>
                        <telerik:GridTemplateColumn HeaderText="M" ColumnGroupName="M14" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MM14">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMM14" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Miệng --%>
                        <%-- 15p --%>
                        <telerik:GridTemplateColumn HeaderText="15p" ColumnGroupName="M14" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="15pM14">
                            <ItemTemplate>
                                <asp:TextBox ID="tb15pM14" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 15p --%>
                        <%-- 1 tiết hs1 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS1" ColumnGroupName="M14" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS1M14">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS1M14" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs1 --%>
                        <%-- 1 tiết hs2 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS2" ColumnGroupName="M14" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS2M14">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS2M14" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs2 --%>
                        <%-- Học kỳ --%>
                        <telerik:GridTemplateColumn HeaderText="HK" ColumnGroupName="M14" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="HKM14">
                            <ItemTemplate>
                                <asp:TextBox ID="tbHKM14" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End học kỳ --%>
                        <%-- End M14 --%>

                        <%--M15--%>
                        <%-- Miệng --%>
                        <telerik:GridTemplateColumn HeaderText="M" ColumnGroupName="M15" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MM15">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMM15" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Miệng --%>
                        <%-- 15p --%>
                        <telerik:GridTemplateColumn HeaderText="15p" ColumnGroupName="M15" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="15pM15">
                            <ItemTemplate>
                                <asp:TextBox ID="tb15pM15" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 15p --%>
                        <%-- 1 tiết hs1 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS1" ColumnGroupName="M15" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS1M15">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS1M15" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs1 --%>
                        <%-- 1 tiết hs2 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS2" ColumnGroupName="M15" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS2M15">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS2M15" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs2 --%>
                        <%-- Học kỳ --%>
                        <telerik:GridTemplateColumn HeaderText="HK" ColumnGroupName="M15" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="HKM15">
                            <ItemTemplate>
                                <asp:TextBox ID="tbHKM15" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End học kỳ --%>
                        <%-- End M15 --%>

                        <%--M16--%>
                        <%-- Miệng --%>
                        <telerik:GridTemplateColumn HeaderText="M" ColumnGroupName="M16" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MM16">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMM16" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Miệng --%>
                        <%-- 15p --%>
                        <telerik:GridTemplateColumn HeaderText="15p" ColumnGroupName="M16" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="15pM16">
                            <ItemTemplate>
                                <asp:TextBox ID="tb15pM16" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 15p --%>
                        <%-- 1 tiết hs1 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS1" ColumnGroupName="M16" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS1M16">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS1M16" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs1 --%>
                        <%-- 1 tiết hs2 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS2" ColumnGroupName="M16" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS2M16">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS2M16" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs2 --%>
                        <%-- Học kỳ --%>
                        <telerik:GridTemplateColumn HeaderText="HK" ColumnGroupName="M16" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="HKM16">
                            <ItemTemplate>
                                <asp:TextBox ID="tbHKM16" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End học kỳ --%>
                        <%-- End M16 --%>

                        <%--M17--%>
                        <%-- Miệng --%>
                        <telerik:GridTemplateColumn HeaderText="M" ColumnGroupName="M17" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MM17">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMM17" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Miệng --%>
                        <%-- 15p --%>
                        <telerik:GridTemplateColumn HeaderText="15p" ColumnGroupName="M17" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="15pM17">
                            <ItemTemplate>
                                <asp:TextBox ID="tb15pM17" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 15p --%>
                        <%-- 1 tiết hs1 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS1" ColumnGroupName="M17" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS1M17">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS1M17" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs1 --%>
                        <%-- 1 tiết hs2 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS2" ColumnGroupName="M17" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS2M17">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS2M17" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs2 --%>
                        <%-- Học kỳ --%>
                        <telerik:GridTemplateColumn HeaderText="HK" ColumnGroupName="M17" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="HKM17">
                            <ItemTemplate>
                                <asp:TextBox ID="tbHKM17" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End học kỳ --%>
                        <%-- End M17 --%>

                        <%--M18--%>
                        <%-- Miệng --%>
                        <telerik:GridTemplateColumn HeaderText="M" ColumnGroupName="M18" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MM18">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMM18" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Miệng --%>
                        <%-- 15p --%>
                        <telerik:GridTemplateColumn HeaderText="15p" ColumnGroupName="M18" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="15pM18">
                            <ItemTemplate>
                                <asp:TextBox ID="tb15pM18" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 15p --%>
                        <%-- 1 tiết hs1 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS1" ColumnGroupName="M18" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS1M18">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS1M18" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs1 --%>
                        <%-- 1 tiết hs2 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS2" ColumnGroupName="M18" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS2M18">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS2M18" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs2 --%>
                        <%-- Học kỳ --%>
                        <telerik:GridTemplateColumn HeaderText="HK" ColumnGroupName="M18" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="HKM18">
                            <ItemTemplate>
                                <asp:TextBox ID="tbHKM18" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End học kỳ --%>
                        <%-- End M18 --%>

                        <%--M19--%>
                        <%-- Miệng --%>
                        <telerik:GridTemplateColumn HeaderText="M" ColumnGroupName="M19" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MM19">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMM19" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Miệng --%>
                        <%-- 15p --%>
                        <telerik:GridTemplateColumn HeaderText="15p" ColumnGroupName="M19" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="15pM19">
                            <ItemTemplate>
                                <asp:TextBox ID="tb15pM19" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 15p --%>
                        <%-- 1 tiết hs1 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS1" ColumnGroupName="M19" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS1M19">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS1M19" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs1 --%>
                        <%-- 1 tiết hs2 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS2" ColumnGroupName="M19" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS2M19">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS2M19" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs2 --%>
                        <%-- Học kỳ --%>
                        <telerik:GridTemplateColumn HeaderText="HK" ColumnGroupName="M19" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="HKM19">
                            <ItemTemplate>
                                <asp:TextBox ID="tbHKM19" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End học kỳ --%>
                        <%-- End M19 --%>

                        <%--M20--%>
                        <%-- Miệng --%>
                        <telerik:GridTemplateColumn HeaderText="M" ColumnGroupName="M20" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MM20">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMM20" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Miệng --%>
                        <%-- 15p --%>
                        <telerik:GridTemplateColumn HeaderText="15p" ColumnGroupName="M20" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="15pM20">
                            <ItemTemplate>
                                <asp:TextBox ID="tb15pM20" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 15p --%>
                        <%-- 1 tiết hs1 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS1" ColumnGroupName="M20" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS1M20">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS1M20" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs1 --%>
                        <%-- 1 tiết hs2 --%>
                        <telerik:GridTemplateColumn HeaderText="1T-HS2" ColumnGroupName="M20" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="1THS2M20">
                            <ItemTemplate>
                                <asp:TextBox ID="tb1THS2M20" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End 1 tiết hs2 --%>
                        <%-- Học kỳ --%>
                        <telerik:GridTemplateColumn HeaderText="HK" ColumnGroupName="M20" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="HKM20">
                            <ItemTemplate>
                                <asp:TextBox ID="tbHKM20" runat="server" CssClass="form-control text-box" Height="100%"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End học kỳ --%>
                        <%-- End M20 --%>
                        <telerik:GridTemplateColumn HeaderText="Nội dung nhận xét" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NOI_DUNG_NX" ItemStyle-CssClass="grid-control grid-col-nd">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNoiDung" runat="server" Text='<%# Eval("NOI_DUNG_NX") %>' CssClass="form-control text-box nd-nxhn" MaxLength="306" TextMode="MultiLine" Rows="2"></asp:TextBox>
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
    <script>
        Sys.Application.add_load(function () {
            createAutocomplateMaNX("nd-nxhn", "../DataMaNXHN.aspx");
        });
        function chbDong_Change() {
            if (document.getElementById('chbDong').checked) {
                Cookies.set('is_jump_col', 1, { expires: 7 });
                is_jump_col = 1;
            } else {
                Cookies.set('is_jump_col', 0, { expires: 7 });
                is_jump_col = 0;
            }
        }
        function chbCot_Change() {
            if (document.getElementById('chbCot').checked) {
                Cookies.set('is_jump_col', 0, { expires: 7 });
                is_jump_col = 0;
            } else {
                Cookies.set('is_jump_col', 1, { expires: 7 });
                is_jump_col = 1;
            }
        }
        try {
            if (Cookies.get('is_jump_col') != 1 && Cookies.get('is_jump_col') != "1") {
                is_jump_col = 0;
                document.getElementById('chbCot').checked = true;
                document.getElementById('chbDong').checked = false;
            }
            else {
                is_jump_col = 1;
                document.getElementById('chbDong').checked = true;
                document.getElementById('chbCot').checked = false;
            }
        } catch (ex) { }
    </script>
</asp:Content>
