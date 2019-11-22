<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="NhapDanhGiaDinhKyTH.aspx.cs" Inherits="CMS.KetQuaHocTap.NhapDanhGiaDinhKyTH" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbKhoiHoc">
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
            <telerik:AjaxSetting AjaxControlID="cboNangLuc">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="cboPhamChat">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="cboGVCN">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
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
                            case "DGDKTH":
                                cval = ChangeValidValueDGMH(cell.value);
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
            $(document).on("keyup", ".DGDKTH .text-box", function (event) {
                if (event.which != 37 && event.which != 38 && event.which != 39 && event.which != 40) {
                    ProcessCellValue(this, "DGDKTH");
                } else {
                    JumCell(this, event.which);
                }
            });
            $(document).on("keyup", ".text-box", function (event) {
                if (event.which == 37 || event.which == 38 || event.which == 39 || event.which == 40) {
                    JumCell(this, event.which);
                }
            });
            function btLuuClick() {
                if (confirm("Bạn chắc chắn muốn lưu?")) {
                    return true;
                }
            }
        </script>
    </telerik:RadCodeBlock>
    <div class="item-content">

        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">Đánh giá định kỳ (GVCN nhận xét)</span>
            </div>
            <div class="col-sm-8 text-right">
                <label>
                    <input type="radio" id="chbCot" name="config" value="doc" onchange="chbCot_Change()">Nhảy cột</label>
                <label>
                    <input type="radio" id="chbDong" name="config" value="ngang" onchange="chbDong_Change()">Nhảy dòng</label>
                <asp:Button ID="btSave" runat="server" CssClass="btn bt-one" OnClick="btSave_Click" Text="Lưu" OnClientClick="if(!btLuuClick()) return false;" />
                <asp:Button ID="btExport" runat="server" CssClass="btn bt-one" OnClick="btExport_Click" Text="Xuất excel" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="row note-form">
                <div class="col-sm-12">
                    <label class="content-note-form"><span style="font-weight: bold">Chú ý: </span>Để nhập nhanh gợi ý sẽ xuất hiện khi bạn nhập các ký tự như <span style="color: red">"@,."</span> . Bạn dùng các phím lên, xuống để lựa chọn gợi ý mình mong muốn và <span style="color: red">Enter</span>. Nhấn <span style="color: red">End</span> để xuống dòng.</label><br />
                    <label class="content-note-form">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Nhận xét về năng lực, phẩm chất: <span style="color: red">T</span>: Hoàn thành tốt; <span style="color: red">H</span>: Hoàn thành; <span style="color: red">C</span>: Chưa hoàn thành</label>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-2">
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
                <div class="col-sm-2">
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
                    <telerik:RadComboBox ID="rcbKyDGTH" runat="server" Width="100%" DataSourceID="objKyDGTH"
                        DataTextField="TEN" DataValueField="MA" Filter="Contains" AutoPostBack="true" OnSelectedIndexChanged="rcbKyDGTH_SelectedIndexChanged">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objKyDGTH" runat="server" SelectMethod="getKyDGTHByKy" TypeName="OneEduDataAccess.BO.KyDGTHBO">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                            <asp:Parameter Name="id_all" Type="Int16" />
                            <asp:Parameter Name="text_all" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
                <div class="col-sm-6">
                    <div class="col-sm-4">
                        <div class="one-checkbox">
                            <label>
                                <asp:CheckBox ID="cboNangLuc" runat="server" Text="Năng lực" OnCheckedChanged="cboNangLuc_CheckedChanged" AutoPostBack="true"/>
                            </label>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="one-checkbox">
                            <label>
                                <asp:CheckBox ID="cboPhamChat" runat="server" Text="Phẩm chất" OnCheckedChanged="cboPhamChat_CheckedChanged" AutoPostBack="true"/>
                            </label>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="one-checkbox">
                            <label>
                                <asp:CheckBox ID="cboGVCN" runat="server" Text="GVCN NX" OnCheckedChanged="cboGVCN_CheckedChanged" AutoPostBack="true"/>
                            </label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="item-data">
            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" GroupPanelPosition="Top" OnNeedDataSource="RadGrid1_NeedDataSource"
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
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Năng lực" Name="NANG_LUC" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Phẩm chất" Name="PHAM_CHAT" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="STT" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%# Container.DataSetIndex+1 %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="MA_HS" HeaderStyle-Width="140px" FilterControlAltText="Filter MA_HS column" HeaderText="Mã HS" SortExpression="MA_HS" UniqueName="MA_HS" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TEN_HS" HeaderStyle-Width="200px" FilterControlAltText="Filter TEN_HS column" HeaderText="Tên HS" SortExpression="TEN_HS" UniqueName="TEN_HS" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <%--<telerik:GridBoundColumn DataField="NGAY_SINH" HeaderStyle-Width="100px" FilterControlAltText="Filter NGAY_SINH column" HeaderText="Ngày sinh" SortExpression="NGAY_SINH" UniqueName="NGAY_SINH" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}">
                        </telerik:GridBoundColumn>--%>
                        <%-- nang luc --%>
                        <telerik:GridTemplateColumn HeaderText="TPV, TQ" ColumnGroupName="NANG_LUC" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control DGDKTH" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NL_TPVTQ">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNL_TPVTQ" runat="server" Text='<%# Eval("NL_TPVTQ") %>' CssClass="form-control text-box" Style="text-align: center;" Height="100%"></asp:TextBox>
                                <asp:HiddenField ID="hdNL_TPVTQ" runat="server" Value='<%# Eval("NL_TPVTQ") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HT" ColumnGroupName="NANG_LUC" ItemStyle-CssClass="grid-control DGDKTH" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NL_HT" HeaderStyle-Width="60px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNL_HT" runat="server" Text='<%# Eval("NL_HT") %>' CssClass="form-control text-box" MaxLength="20" Style="text-align: center;" Height="100%"></asp:TextBox>
                                <asp:HiddenField ID="hdNL_HT" runat="server" Value='<%# Eval("NL_HT") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="TGQVĐ" ColumnGroupName="NANG_LUC" ItemStyle-CssClass="grid-control DGDKTH" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NL_TGQVD" HeaderStyle-Width="60px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNL_TGQVD" runat="server" Text='<%# Eval("NL_TGQVD") %>' CssClass="form-control text-box" MaxLength="20" Style="text-align: center;" Height="100%"></asp:TextBox>
                                <asp:HiddenField ID="hdNL_TGQVD" runat="server" Value='<%# Eval("NL_TGQVD") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nhận xét" ColumnGroupName="NANG_LUC" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NL_NX">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNL_NX" runat="server" Text='<%# Eval("NL_NX") %>' CssClass="form-control text-box nd-nx-nl" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                <asp:HiddenField ID="hdNL_NX" runat="server" Value='<%# Eval("NL_NX") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- end nang luc--%>
                        <%-- Pham chat--%>
                        <telerik:GridTemplateColumn HeaderText="CH, CL" ColumnGroupName="PHAM_CHAT" ItemStyle-CssClass="grid-control DGDKTH" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="PC_CHCL" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbPC_CHCL" runat="server" Text='<%# Eval("PC_CHCL") %>' CssClass="form-control text-box" MaxLength="300" Style="text-align: center;" Height="100%">></asp:TextBox>
                                <asp:HiddenField ID="hdPC_CHCL" runat="server" Value='<%# Eval("PC_CHCL") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="TT, TN" ColumnGroupName="PHAM_CHAT" ItemStyle-CssClass="grid-control DGDKTH" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="PC_TTTN" HeaderStyle-Width="60px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbPC_TTTN" runat="server" Text='<%# Eval("PC_TTTN") %>' CssClass="form-control text-box" Style="text-align: center;" MaxLength="300" Height="100%">></asp:TextBox>
                                <asp:HiddenField ID="hdPC_TTTN" runat="server" Value='<%# Eval("PC_TTTN") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="TT, KL" ColumnGroupName="PHAM_CHAT" ItemStyle-CssClass="grid-control DGDKTH" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="PC_TTKL" HeaderStyle-Width="60px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbPC_TTKL" runat="server" Text='<%# Eval("PC_TTKL") %>' CssClass="form-control text-box" Style="text-align: center;" MaxLength="300" Height="100%">></asp:TextBox>
                                <asp:HiddenField ID="hdPC_TTKL" runat="server" Value='<%# Eval("PC_TTKL") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ĐK, YT" ColumnGroupName="PHAM_CHAT" ItemStyle-CssClass="grid-control DGDKTH" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="PC_DKYT" HeaderStyle-Width="60px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbPC_DKYT" runat="server" Text='<%# Eval("PC_DKYT") %>' CssClass="form-control text-box" Style="text-align: center;" MaxLength="20" Height="100%">></asp:TextBox>
                                <asp:HiddenField ID="hdPC_DKYT" runat="server" Value='<%# Eval("PC_DKYT") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nhận xét" ColumnGroupName="PHAM_CHAT" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="PC_NX">
                            <ItemTemplate>
                                <asp:TextBox ID="tbPC_NX" runat="server" Text='<%# Eval("PC_NX") %>' CssClass="form-control text-box nd-nx-pc" MaxLength="300" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                <asp:HiddenField ID="hdPC_NX" runat="server" Value='<%# Eval("PC_NX") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- end pham chat--%>
                        <%-- GVCN--%>
                        <telerik:GridTemplateColumn HeaderText="GVCN nhận xét" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NX_GVCN">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNX_GVCN" runat="server" Text='<%# Eval("NX_GVCN") %>' CssClass="form-control text-box nd-nx-gv" MaxLength="300" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                <asp:HiddenField ID="hdNX_GVCN" runat="server" Value='<%# Eval("NX_GVCN") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End GVCN--%>
                    </Columns>
                    <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
    <script>
        function chbCot_Change() {
            if (document.getElementById('chbCot').checked) {
                Cookies.set('is_jump_col', 1, { expires: 7 });
                is_jump_col = 1;
            } else {
                Cookies.set('is_jump_col', 0, { expires: 7 });
                is_jump_col = 0;
            }
        }
        function chbDong_Change() {
            if (document.getElementById('chbDong').checked) {
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
                document.getElementById('chbDong').checked = true;
                document.getElementById('chbCot').checked = false;
            }
            else {
                is_jump_col = 1;
                document.getElementById('chbCot').checked = true;
                document.getElementById('chbDong').checked = false;
            }
        } catch (ex) { }
        Sys.Application.add_load(function () {
            createAutocomplateMaNX("nd-nx-nl", "../DataMaNX.aspx?ma=2", " ");
            createAutocomplateMaNX("nd-nx-pc", "../DataMaNX.aspx?ma=3", " ");
        });
    </script>
</asp:Content>
