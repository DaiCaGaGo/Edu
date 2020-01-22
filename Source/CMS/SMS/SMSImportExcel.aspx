<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="SMSImportExcel.aspx.cs" Inherits="CMS.SMS.SMSImportExcel" %>

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
            <telerik:AjaxSetting AjaxControlID="cbHenGioGuiTin">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tbTime" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <telerik:RadCodeBlock runat="server">
        <script>
            var grid;
            function pageLoad() {
                grid = $find("<%=RadGrid1.ClientID%>");
            }
            function refreshGrid() {
                var masterTable = grid.get_masterTableView();
                masterTable.rebind();
            }
            function viewResMSG(img, success) {
                var hdresMsg = $(img).closest("td").find("#hdresMsg").first();
                notification(success, $(hdresMsg).val());
            }
            function change(el) {

            }
            function countSMSByText(value) {
                var count = value.length;
                if (count > 0 && count <= 160) {
                    return 1;
                } else if (count > 160 && count < 307) {
                    return 2;
                }
                return 0;
            }
            var cell;
            function countSMSDuTinh() {
                grid = $find("<%=RadGrid1.ClientID%>");
                var soTinSuDung = 0;
                var countHeSo = 0;
                var total = 1;
                var countCheck = (grid == null && grid.get_masterTableView().get_selectedItems() == null) ? 0 : grid.get_masterTableView().get_selectedItems().length;
                if (countCheck > 0) {
                    for (var i = 0; i < countCheck; i++) {
                        var item = grid.get_masterTableView().get_selectedItems()[i];
                        var cellHeSo = 1;
                        var cellNOI_DUNG_TB = $(item.get_cell("NOI_DUNG_TONG_HOP"));
                        var cellNoiDung = (cellNOI_DUNG_TB).find('.nd-th').val();
                        soTinSuDung += cellHeSo * countSMSByText(cellNoiDung);
                    }
                } else {
                    for (var i = 0; i < grid.get_masterTableView().get_dataItems().length; i++) {
                        var item = grid.get_masterTableView().get_dataItems()[i];
                        var cellHeSo = 1;
                        var cellNOI_DUNG_TB = $(item.get_cell("NOI_DUNG_TONG_HOP"));
                        var cellNoiDung = (cellNOI_DUNG_TB).find('.nd-th').val();
                        soTinSuDung += cellHeSo * countSMSByText(cellNoiDung);
                    }
                }
                document.getElementById('numberConfirm').innerHTML = soTinSuDung;
            }

            function btCopyClick(sender, args) {
                grid = $find("<%=RadGrid1.ClientID%>");
                var max_len = 306;
                var ndCongThuc = $('#tbNoiDung').val();
                var countCheck = (grid == null && grid.get_masterTableView().get_selectedItems() == null) ? 0 : grid.get_masterTableView().get_selectedItems().length;
                if (countCheck > 0) {
                    for (var i = 0; i < countCheck; i++) {
                        var item = grid.get_masterTableView().get_selectedItems()[i];
                        var ndth = ndCongThuc;

                        //set control
                        var label1 = $telerik.findElement(item.get_element(), "lblCOT_1");
                        var label2 = $telerik.findElement(item.get_element(), "lblCOT_2");
                        var label3 = $telerik.findElement(item.get_element(), "lblCOT_3");
                        var label4 = $telerik.findElement(item.get_element(), "lblCOT_4");
                        var label5 = $telerik.findElement(item.get_element(), "lblCOT_5");
                        var label6 = $telerik.findElement(item.get_element(), "lblCOT_6");
                        var label7 = $telerik.findElement(item.get_element(), "lblCOT_7");
                        var label8 = $telerik.findElement(item.get_element(), "lblCOT_8");
                        var label9 = $telerik.findElement(item.get_element(), "lblCOT_9");
                        var label10 = $telerik.findElement(item.get_element(), "lblCOT_10");
                        var label11 = $telerik.findElement(item.get_element(), "lblCOT_11");
                        var label12 = $telerik.findElement(item.get_element(), "lblCOT_12");
                        var label13 = $telerik.findElement(item.get_element(), "lblCOT_13");
                        var label14 = $telerik.findElement(item.get_element(), "lblCOT_14");
                        var label15 = $telerik.findElement(item.get_element(), "lblCOT_15");

                        ndth = ndth.replace(/@Cot/g, '@cot').replace(/@COT/g, '@cot');
                        ndth = ndth.replace(/@cot1@/g, label1.innerHTML);
                        ndth = ndth.replace(/@cot2@/g, label2.innerHTML);
                        ndth = ndth.replace(/@cot3@/g, label3.innerHTML);
                        ndth = ndth.replace(/@cot4@/g, label4.innerHTML);
                        ndth = ndth.replace(/@cot5@/g, label5.innerHTML);
                        if (label6.innerHTML == "&nbsp;")
                            ndth = ndth.replace(/@cot6@/g, "");
                        else ndth = ndth.replace(/@cot6@/g, label6.innerHTML);
                        ndth = ndth.replace(/@cot7@/g, label7.innerHTML);
                        ndth = ndth.replace(/@cot8@/g, label8.innerHTML);
                        ndth = ndth.replace(/@cot9@/g, label9.innerHTML);
                        ndth = ndth.replace(/@cot10@/g, label10.innerHTML);
                        ndth = ndth.replace(/@cot11@/g, label11.innerHTML);
                        ndth = ndth.replace(/@cot12@/g, label12.innerHTML);
                        ndth = ndth.replace(/@cot13@/g, label13.innerHTML);
                        ndth = ndth.replace(/@cot14@/g, label14.innerHTML);
                        ndth = ndth.replace(/@cot15@/g, label15.innerHTML);
                        //ndth = ndth.replace(/(\r\n|\n|\r)/g,"");
                        ndth = ndth.replace(/      /g, " ");
                        ndth = ndth.replace(/    /g, " ");
                        ndth = ndth.replace(/   /g, " ");
                        ndth = ndth.replace(/  /g, " ");

                        var tbndtb = $(item.get_cell("NOI_DUNG_TONG_HOP")).find('.nd-th');
                        $(tbndtb).val(ndth);
                        if ($(tbndtb).val().length > max_len) {
                            $(tbndtb).val() = $(tbndtb).val().substr(0, max_len);
                        }
                        var nextTd = $(tbndtb).closest('td').next().find('.view-length').first();
                        if ($(tbndtb).val().length == 0) $(nextTd).html('');
                        else $(nextTd).html($(tbndtb).val().length);
                    }
                } else {
                    for (var i = 0; i < grid.get_masterTableView().get_dataItems().length; i++) {
                        var item = grid.get_masterTableView().get_dataItems()[i];
                        var ndth = ndCongThuc;

                        //set control
                        var label1 = $telerik.findElement(item.get_element(), "lblCOT_1");
                        var label2 = $telerik.findElement(item.get_element(), "lblCOT_2");
                        var label3 = $telerik.findElement(item.get_element(), "lblCOT_3");
                        var label4 = $telerik.findElement(item.get_element(), "lblCOT_4");
                        var label5 = $telerik.findElement(item.get_element(), "lblCOT_5");
                        var label6 = $telerik.findElement(item.get_element(), "lblCOT_6");
                        var label7 = $telerik.findElement(item.get_element(), "lblCOT_7");
                        var label8 = $telerik.findElement(item.get_element(), "lblCOT_8");
                        var label9 = $telerik.findElement(item.get_element(), "lblCOT_9");
                        var label10 = $telerik.findElement(item.get_element(), "lblCOT_10");
                        var label11 = $telerik.findElement(item.get_element(), "lblCOT_11");
                        var label12 = $telerik.findElement(item.get_element(), "lblCOT_12");
                        var label13 = $telerik.findElement(item.get_element(), "lblCOT_13");
                        var label14 = $telerik.findElement(item.get_element(), "lblCOT_14");
                        var label15 = $telerik.findElement(item.get_element(), "lblCOT_15");

                        //replace value
                        ndth = ndth.replace(/@Cot/g, '@cot').replace(/@COT/g, '@cot');
                        ndth = ndth.replace(/@cot1@/g, label1.innerHTML);
                        ndth = ndth.replace(/@cot2@/g, label2.innerHTML);
                        ndth = ndth.replace(/@cot3@/g, label3.innerHTML);
                        ndth = ndth.replace(/@cot4@/g, label4.innerHTML);
                        ndth = ndth.replace(/@cot5@/g, label5.innerHTML);
                        if (label6.innerHTML == "&nbsp;")
                            ndth = ndth.replace(/@cot6@/g, "");
                        else ndth = ndth.replace(/@cot6@/g, label6.innerHTML);
                        ndth = ndth.replace(/@cot7@/g, label7.innerHTML);
                        ndth = ndth.replace(/@cot8@/g, label8.innerHTML);
                        ndth = ndth.replace(/@cot9@/g, label9.innerHTML);
                        ndth = ndth.replace(/@cot10@/g, label10.innerHTML);
                        ndth = ndth.replace(/@cot11@/g, label11.innerHTML);
                        ndth = ndth.replace(/@cot12@/g, label12.innerHTML);
                        ndth = ndth.replace(/@cot13@/g, label13.innerHTML);
                        ndth = ndth.replace(/@cot14@/g, label14.innerHTML);
                        ndth = ndth.replace(/@cot15@/g, label15.innerHTML);
                        //ndth = ndth.replace(/(\r\n|\n|\r)/g,"");
                        ndth = ndth.replace(/      /g, " ");
                        ndth = ndth.replace(/    /g, " ");
                        ndth = ndth.replace(/   /g, " ");
                        ndth = ndth.replace(/  /g, " ");

                        var tbndtb = $(item.get_cell("NOI_DUNG_TONG_HOP")).find('.nd-th');
                        $(tbndtb).val(ndth);
                        if ($(tbndtb).val().length > max_len) {
                            $(tbndtb).val() = $(tbndtb).val().substr(0, max_len);
                        }
                        var nextTd = $(tbndtb).closest('td').next().find('.view-length').first();
                        if ($(tbndtb).val().length == 0) $(nextTd).html('');
                        else $(nextTd).html($(tbndtb).val().length);
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
            $(document).on("keyup", ".nd-th", function (event) {
                var max_len = 306;
                if ($(this).val().length > max_len) {
                    $(this).val() = $(this).val().substr(0, max_len);
                }
                var nextTd = $(this).closest('td').next().find('.view-length').first();
                if ($(this).val().length == 0) $(nextTd).html('');
                else $(nextTd).html($(this).val().length);
            });
            
            function getCot(i) {

                var textNoiDung = document.getElementById("tbNoiDung")
                textNoiDung.focus()
                var ss = textNoiDung.selectionStart
                var se = textNoiDung.selectionEnd

                var temp = " @cot" + i +"@ ";
                textNoiDung.value = textNoiDung.value.substring(0, ss) + temp + textNoiDung.value.substring(se, textNoiDung.value.length)
                textNoiDung.setSelectionRange(ss + temp.length, ss + temp.length)

                <%--grid = $find("<%=RadGrid1.ClientID%>");
                var ndChung = $('#tbNoiDung').val();
                var strNoiDung = ndChung;
                strNoiDung = strNoiDung.trim();
                strNoiDung += " @cot" + i + "@ ";
                document.getElementById("tbNoiDung").value = strNoiDung;
                document.getElementById("tbNoiDung").focus();--%>
            }

            function guiTuyChonClick() {
                grid = $find("<%=RadGrid1.ClientID%>");
                if (grid.get_masterTableView().get_selectedItems().length > 0) {
                    if (confirm("Bạn có chắc chắn muốn gửi tin?")) {
                        return true;
                    }
                } else {
                    alert("Mời bạn chọn học sinh cần gửi tin.");
                    return false;
                }
            }
        </script>
        <style>
            .img-con-arrow {
                padding: 0 10px;
            }
        </style>
    </telerik:RadCodeBlock>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">GỬI TIN NHẮN TÙY BIẾN</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btTaiExcel" runat="server" OnClick="btTaiExcel_Click" Text="Tải file mẫu" CssClass="btn bt-one" />
                <asp:Button ID="bt_EXCELtoSQL" runat="server" Text="Gửi tùy chọn" OnClick="bt_EXCELtoSQL_Click" CssClass="btn bt-one" OnClientClick="if(!guiTuyChonClick()) return false;" />
                <asp:Button ID="btnSendAll" runat="server" Text="Gửi tất cả" OnClick="btnSendAll_Click" CssClass="btn bt-one" OnClientClick="if ( !confirm('Bạn có chắc chắn muốn gửi tin?')) return false;" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="row note-form">
                <div class="col-sm-12">
                    <label class="content-note-form">
                        <span style="font-weight: bold">Hướng dẫn:</span> Nhập nội dung tin nhắn vào phần tổng hợp tin nhắn có dạng <span style="color: red">"@cot1@ @cot2@ ..."</span> hoặc <span style="color: red">"nội dung chèn @cot1@ nội dung chèn @cot2@ nội dung chèn ..."</span>
                        (Trong đó: cột 1 là cột ngay sau cột trạng thái) 
                    </label>
                </div>
            </div>
            <div class="row" style="padding: 0 15px 0 15px">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 460px">
                            <div class="col-sm-12 one-checkbox">
                                <asp:CheckBox ID="cbIsHocSinh" runat="server" Text="Gửi cho học sinh" />
                            </div>
                            <div>
                                <asp:FileUpload ID="FileExcel" AllowMultiple="true" runat="server" CssClass="btn" accept=".xls,.xlsx" />
                                <asp:Button ID="btnUpload" CssClass="btn bt-one" runat="server" OnClick="bt_importSQL_Click" Text="Tải lên" />
                            </div>

                        </td>
                        <td style="width: 150px"></td>
                        <td>
                            <asp:TextBox ID="tbNoiDung" ClientIDMode="Static" runat="server" placeholder="Nhập nội dung tin nhắn hoặc tạo tin nhắn vào đây" CssClass="form-control text-box nd-nx-nl" TextMode="MultiLine" Rows="3" MaxLength="306" onkeyup="change(this);" onkeydown="change(this);"></asp:TextBox>

                        </td>
                        <td style="width: 400px; padding-left: 20px">
                            <table>
                                <tr>
                                    <td class="one-checkbox">
                                        <asp:CheckBox ID="cbHenGioGuiTin" runat="server" Text="Hẹn Giờ" OnCheckedChanged="cbHenGioGuiTin_CheckedChanged" AutoPostBack="true" />
                                    </td>
                                    <td style="padding-left: 20px;">
                                        <asp:TextBox ID="tbTime" Visible="false" runat="server" CssClass="nd-nx-nl" TextMode="DateTimeLocal"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <input type="button" id="btCopy" class="btn bt-one" value="Tổng hợp" onclick="btCopyClick()" />

                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <div class="col-sm-12">
                                <asp:Label runat="server" ID="txtTongQuyTinConLaiTheoNam" Font-Size="20px" ForeColor="Red"></asp:Label>
                                <asp:Label runat="server" ID="txtTongQuyTinConLaiTheoThang" Font-Size="20px" ForeColor="Red" Style="padding-left: 50px;"></asp:Label>
                                <span style="font-size: 20px; padding-left: 50px; color: red;">Dự tính số tin sẽ sử dụng: <span id="numberConfirm" style="font-size: 20px; color: red;">0</span></span>
                                <asp:Label runat="server" ID="lbTinSuDung" Font-Size="20px"></asp:Label>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="item-data">
            <asp:HiddenField ID="hdTongHeSo" runat="server" ClientIDMode="Static" />
            <telerik:RadGrid ID="RadGrid1" runat="server" GroupPanelPosition="Top" OnNeedDataSource="RadGrid1_NeedDataSource" OnItemDataBound="RadGrid1_ItemDataBound"
                AutoGenerateColumns="false" AllowMultiRowSelection="True">
                <ClientSettings>
                    <Selecting AllowRowSelect="true"></Selecting>
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <ClientEvents OnRowSelected="RowSelected" OnRowDeselected="RowDeselected" />
                </ClientSettings>
                <MasterTableView>
                    <NoRecordsTemplate>
                        <div style="padding: 20px 10px;">
                            Không có bản ghi nào!
                        </div>
                    </NoRecordsTemplate>
                    <HeaderStyle CssClass="head-list-grid" />
                    <Columns>
                        <telerik:GridClientSelectColumn UniqueName="chkChon" ItemStyle-CssClass="chbChon" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridClientSelectColumn>
                        <telerik:GridTemplateColumn HeaderText="Trạng thái" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="TRANG_THAI">
                            <ItemTemplate>
                                <img src="../img/error.png" id="iconErr" runat="server" style="cursor: pointer;" onclick="viewResMSG(this,'error')" />
                                <img src="../img/success.png" id="iconSuccess" runat="server" style="cursor: pointer;" onclick="viewResMSG(this,'success')" />
                                <asp:HiddenField ID="hdresMsg" runat="server" Value="Nội dung thông báo" ClientIDMode="Static" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="COT_1" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="COT_1">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT_1" onclick="getCot(1)" Text="acb"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCOT_1" runat="server" Text='<%# Eval("COT_1")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="COT_2" HeaderStyle-Width="160px" HeaderStyle-HorizontalAlign="Center" UniqueName="COT_2">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT_2" onclick="getCot(2)" Text="acb"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCOT_2" runat="server" Text='<%# Eval("COT_2")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="COT_3" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="COT_3">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT_3" onclick="getCot(3)" Text="acb"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%--<span><%# Eval("COT_3").ToString() %></span>--%>
                                <asp:Label ID="lblCOT_3" runat="server" Text='<%# Eval("COT_3")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="COT_4" HeaderStyle-HorizontalAlign="Center" UniqueName="COT_4">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT_4" onclick="getCot(4)" Text="acb"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCOT_4" runat="server" Text='<%# Eval("COT_4")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="COT_5" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="COT_5">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT_5" onclick="getCot(5)" Text="acb"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCOT_5" runat="server" Text='<%# Eval("COT_5")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="COT_6" HeaderStyle-HorizontalAlign="Center" UniqueName="COT_6">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT_6" onclick="getCot(6)" Text="acb"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCOT_6" runat="server" Text='<%# Eval("COT_6")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="COT_7" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="COT_7">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT_7" onclick="getCot(7)" Text="acb"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCOT_7" runat="server" Text='<%# Eval("COT_7")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="COT_8" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="COT_8">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT_8" onclick="getCot(8)" Text="acb"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCOT_8" runat="server" Text='<%# Eval("COT_8")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="COT_9" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="COT_9">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT_9" onclick="getCot(9)" Text="acb"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCOT_9" runat="server" Text='<%# Eval("COT_9")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="COT_10" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="COT_10">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT_10" onclick="getCot(10)" Text="acb"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCOT_10" runat="server" Text='<%# Eval("COT_10")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="COT_11" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="COT_11">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT_11" onclick="getCot(11)" Text="acb"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCOT_11" runat="server" Text='<%# Eval("COT_11")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="COT_12" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="COT_12">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT_12" onclick="getCot(12)" Text="acb"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCOT_12" runat="server" Text='<%# Eval("COT_12")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="COT_13" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="COT_13">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT_13" onclick="getCot(13)" Text="acb"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCOT_13" runat="server" Text='<%# Eval("COT_13")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="COT_14" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="COT_14">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT_14" onclick="getCot(14)" Text="acb"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCOT_14" runat="server" Text='<%# Eval("COT_14")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="COT_15" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="COT_15">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT_15" onclick="getCot(15)" Text="acb"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCOT_15" runat="server" Text='<%# Eval("COT_15")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="NOI_DUNG_TONG_HOP" HeaderStyle-Width="200px" FilterControlAltText="Filter NOI_DUNG_TONG_HOP column" HeaderText="Nội dung tổng hợp" SortExpression="NOI_DUNG_TONG_HOP" UniqueName="NOI_DUNG_TONG_HOP" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNoiDungTongHop" runat="server" CssClass="form-control nd-th" TextMode="MultiLine" Rows="2" ClientIDMode="Static" MaxLength="306" onkeyup="changeNoiDungRow(this);" onkeydown="changeNoiDungRow(this);" onchange="changeNoiDungRow(this);"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Đếm ký tự" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="CountLength" ItemStyle-CssClass="grid-control" HeaderStyle-Width="60px">
                            <ItemTemplate>
                                <span class="view-length">0
                                </span>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
