<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="NhapDanhGiaMonDinhKyTH.aspx.cs" Inherits="CMS.KetQuaHocTap.NhapDanhGiaMonDinhKyTH" %>

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
                    <telerik:AjaxUpdatedControl ControlID="rcbMonHoc" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="btSave" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="lblMess" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbLop">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbMonHoc" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="btSave" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="lblMess" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbMonHoc">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="btSave" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="lblMess" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbKyDGTH">
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
                            case "DiemChan10":
                                cval = DiemChan10(cell.value);
                                break;
                            case "DGMH":
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
            $(document).on("keyup", ".DiemChan10 .text-box", function (event) {
                var code = event.keyCode;
                if (event.which != 37 && event.which != 38 && event.which != 39 && event.which != 40 && event.which != 35) {
                    ProcessCellValue(this, "DiemChan10");
                } else {
                    JumCell(this, event.which);
                }
            });
            $(document).on("keyup", ".DGMH .text-box", function (event) {
                if (event.which != 37 && event.which != 38 && event.which != 39 && event.which != 40 && event.which != 35) {
                    ProcessCellValue(this, "DGMH");
                } else {
                    JumCell(this, event.which);
                }
            });
            $(document).on("keyup", ".NhanXet .text-box", function (event) {
                if (event.which == 37 || event.which == 39 || event.which == 35) {
                    JumCellDivInTD(this, event.which);
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
                <span class="item-title">Nhập đánh giá định kỳ môn học</span>
            </div>
            <div class="col-sm-8 text-right">
                <table style="float:right">
                    <tr>
                        <td style="padding-right:5px">
                            <label style="margin-bottom:unset !important">
                                <input type="radio" id="chbCot" name="config" value="doc" onchange="chbCot_Change()">Nhảy cột</label>
                            <label style="margin-bottom:unset !important">
                                <input type="radio" id="chbDong" name="config" value="ngang" onchange="chbDong_Change()">Nhảy dòng</label>
                        </td>
                        <td style="padding-right:5px">
                            <asp:Button ID="btSave" runat="server" CssClass="btn bt-one" OnClick="btSave_Click" Text="Lưu" OnClientClick="if(!btLuuClick()) return false;" />
                        </td>
                        <td>
                            <asp:Button ID="btExport" runat="server" CssClass="btn bt-one" OnClick="btExport_Click" Text="Xuất excel" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="row note-form">
                <div class="col-sm-12">
                    <label class="content-note-form"><span style="font-weight: bold">Chú ý:</span> Để nhập nhanh gợi ý sẽ xuất hiện khi bạn nhập các ký tự như <span style="color: red">"@,."</span> . Bạn dùng các phím lên, xuống để lựa chọn gợi ý mình mong muốn và <span style="color: red">Enter</span>. Nhấn <span style="color: red">End</span> để xuống dòng.</label>
                </div>
            </div>
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
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbMonHoc" runat="server" Width="100%" DataSourceID="objMonHoc"
                        DataTextField="TEN" DataValueField="ID" Filter="Contains" AutoPostBack="true" OnSelectedIndexChanged="rcbMonHoc_SelectedIndexChanged">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objMonHoc" runat="server" SelectMethod="getMonTruongByLopHocKy" TypeName="OneEduDataAccess.BO.LopMonBO">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="rcbLop" Name="id_lop" PropertyName="SelectedValue" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
                <div class="col-sm-3">
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
            </div>
            <div class="row">
                <div class="col-sm-3">
                    <asp:Label ID="lblMess" runat="server" Text="" style="color:red"></asp:Label>
                </div>
            </div>
        </div>
        <div class="item-data">
            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" GroupPanelPosition="Top" OnNeedDataSource="RadGrid1_NeedDataSource"
                AutoGenerateColumns="False" PageSize="100" AllowMultiRowSelection="True" OnItemDataBound="RadGrid1_ItemDataBound">
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
                        <telerik:GridTemplateColumn HeaderText="STT" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%# Container.DataSetIndex+1 %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="MA_HS" HeaderStyle-Width="140px" FilterControlAltText="Filter MA_HS column" HeaderText="Mã HS" SortExpression="MA_HS" UniqueName="MA_HS" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TEN_HS" HeaderStyle-Width="200px" FilterControlAltText="Filter TEN_HS column" HeaderText="Tên HS" SortExpression="TEN_HS" UniqueName="TEN_HS" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NGAY_SINH" HeaderStyle-Width="100px" FilterControlAltText="Filter NGAY_SINH column" HeaderText="Ngày sinh" SortExpression="NGAY_SINH" UniqueName="NGAY_SINH" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Nội dung nhận xét" ColumnGroupName="NOI_DUNG_NX" ItemStyle-CssClass="grid-control NhanXet" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NOI_DUNG_NX">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNOI_DUNG_NX" runat="server" Text='<%# Eval("NOI_DUNG_NX") %>' CssClass="form-control text-box nd-nx-mon" MaxLength="300"></asp:TextBox>
                                <asp:HiddenField ID="hdNOI_DUNG_NX" runat="server" Value='<%# Eval("NOI_DUNG_NX") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="KTĐK" ColumnGroupName="KTDK" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-CssClass="grid-control DiemChan10" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="KTDK">
                            <ItemTemplate>
                                <asp:TextBox ID="tbKTDK" runat="server" Text='<%# Eval("KTDK") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdKTDK" runat="server" Value='<%# Eval("KTDK") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Mức đạt được" ColumnGroupName="MUC" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-CssClass="grid-control DGMH" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MUC">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMUC" runat="server" Text='<%# Eval("MUC") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdMUC" runat="server" Value='<%# Eval("MUC") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Nhập dữ liệu--%>
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
            createAutocomplateMaNX("nd-nx-mon", "../DataMaNX.aspx?ma=1");
        });
    </script>
</asp:Content>
