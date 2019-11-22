<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="NhapChuyenCan.aspx.cs" Inherits="CMS.ChuyenCan.NhapChuyenCan" %>

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
            <telerik:AjaxSetting AjaxControlID="rcbThang">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbTuan" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbTuan">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
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
            function ProcessCellValue(cell) {
                try {
                    if (cell != null) {
                        var cval;
                        cval = ChangeValidValueP_K(cell.value);
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
            $(document).on("keyup", ".text-box", function (event) {
                if (event.which != 37 && event.which != 38 && event.which != 39 && event.which != 40) {
                    ProcessCellValue(this);
                } else {
                    JumCell(this, event.which);
                }
            });
            function btLuuClick() {
                if (confirm("Bạn chắc chắn muốn lưu ?")) {
                    return true;
                }
            }
        </script>
    </telerik:RadCodeBlock>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">THEO DÕI ĐIỂM CHUYÊN CẦN</span>
            </div>
            <div class="col-sm-8 text-right">
                <label>
                    <input type="radio" id="chbCot" name="config" value="doc" onchange="chbCot_Change()">Nhảy cột</label>
                <label>
                    <input type="radio" id="chbDong" name="config" value="ngang" onchange="chbDong_Change()">Nhảy dòng</label>
                <asp:Button ID="btSave" runat="server" CssClass="btn bt-one" OnClick="btSave_Click" Text="Lưu" OnClientClick="if(!btLuuClick()) return false;" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="form-horizontal" role="form">

                <div class="row note-form">
                    <div class="col-sm-12">
                        <label class="content-note-form"><span style="font-weight: bold">Chú ý:</span><span style="color: red"> Nghỉ học có phép: P; nghỉ học không phép: K</span></label>
                    </div>
                </div>
                <div class="row" style="margin-top: 5px">
                    <div class="col-sm-3">

                        <telerik:RadComboBox ID="rcbKhoi" runat="server" Width="100%" DataSourceID="objKhoiHoc" DataTextField="TEN" DataValueField="MA" AutoPostBack="True" Filter="Contains" OnSelectedIndexChanged="rcbKhoi_SelectedIndexChanged">
                        </telerik:RadComboBox>
                        <asp:ObjectDataSource ID="objKhoiHoc" runat="server" SelectMethod="getKhoiByCapHoc" TypeName="OneEduDataAccess.BO.KhoiBO">
                            <SelectParameters>
                            </SelectParameters>
                        </asp:ObjectDataSource>

                    </div>
                    <div class="col-sm-3">

                        <telerik:RadComboBox ID="rcbLop" runat="server" Width="100%" DataSourceID="objLop"
                            DataTextField="TEN" DataValueField="ID" Filter="Contains" AutoPostBack="true" OnSelectedIndexChanged="rcbLop_SelectedIndexChanged">
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

                        <telerik:RadComboBox ID="rcbThang" runat="server" Width="100%" Filter="Contains" OnSelectedIndexChanged="rcbThang_SelectedIndexChanged" AutoPostBack="true">
                        </telerik:RadComboBox>

                    </div>
                    <div class="col-sm-3">

                        <telerik:RadComboBox ID="rcbTuan" runat="server" Width="100%" Filter="Contains" OnSelectedIndexChanged="rcbTuan_SelectedIndexChanged" AutoPostBack="true">
                        </telerik:RadComboBox>

                    </div>
                </div>
            </div>
        </div>
        <div class="item-data">
            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" GroupPanelPosition="Top" OnNeedDataSource="RadGrid1_NeedDataSource"
                AutoGenerateColumns="False" PageSize="50" AllowMultiRowSelection="True">
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
                        <telerik:GridColumnGroup HeaderText="Tuần 1" HeaderStyle-HorizontalAlign="Center" Name="Tuan1"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Tuần 2" HeaderStyle-HorizontalAlign="Center" Name="Tuan2"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Tuần 3" HeaderStyle-HorizontalAlign="Center" Name="Tuan3"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Tuần 4" HeaderStyle-HorizontalAlign="Center" Name="Tuan4"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Tuần 5" HeaderStyle-HorizontalAlign="Center" Name="Tuan5"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Tuần 6" HeaderStyle-HorizontalAlign="Center" Name="Tuan6"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Số ngày nghỉ trong tháng" HeaderStyle-HorizontalAlign="Center" Name="SO_NGAY_NGHI"></telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="STT" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%# Container.DataSetIndex+1 %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="TEN_HS" FilterControlAltText="Filter TEN_HS column" HeaderText="Họ tên" SortExpression="TEN_HS" UniqueName="TEN_HS" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NGAY_SINH" FilterControlAltText="Filter NGAY_SINH column" HeaderText="Ngày sinh" SortExpression="NGAY_SINH" UniqueName="NGAY_SINH" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-Width="150px">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="1" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NGAY1" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNgay1" runat="server" Text='<%# Eval("NGAY1") %>' CssClass="form-control text-box" MaxLength="1" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdNgay1" runat="server" Value='<%# Eval("NGAY1") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="2" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NGAY2" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNgay2" runat="server" Text='<%# Eval("NGAY2") %>' CssClass="form-control text-box" MaxLength="1" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdNgay2" runat="server" Value='<%# Eval("NGAY2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="3" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NGAY3" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNgay3" runat="server" Text='<%# Eval("NGAY3") %>' CssClass="form-control text-box" MaxLength="1" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdNgay3" runat="server" Value='<%# Eval("NGAY3") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="4" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NGAY4" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNgay4" runat="server" Text='<%# Eval("NGAY4") %>' CssClass="form-control text-box" MaxLength="1" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdNgay4" runat="server" Value='<%# Eval("NGAY4") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="5" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NGAY5" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNgay5" runat="server" Text='<%# Eval("NGAY5") %>' CssClass="form-control text-box" MaxLength="1" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdNgay5" runat="server" Value='<%# Eval("NGAY5") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="6" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NGAY6" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNgay6" runat="server" Text='<%# Eval("NGAY6") %>' CssClass="form-control text-box" MaxLength="1" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdNgay6" runat="server" Value='<%# Eval("NGAY6") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="7" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NGAY7" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNgay7" runat="server" Text='<%# Eval("NGAY7") %>' CssClass="form-control text-box" MaxLength="1" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdNgay7" runat="server" Value='<%# Eval("NGAY7") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="8" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NGAY8" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNgay8" runat="server" Text='<%# Eval("NGAY8") %>' CssClass="form-control text-box" MaxLength="1" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdNgay8" runat="server" Value='<%# Eval("NGAY8") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="9" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NGAY9" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNgay9" runat="server" Text='<%# Eval("NGAY9") %>' CssClass="form-control text-box" MaxLength="1" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdNgay9" runat="server" Value='<%# Eval("NGAY9") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="10" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NGAY10" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNgay10" runat="server" Text='<%# Eval("NGAY10") %>' CssClass="form-control text-box" MaxLength="1" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdNgay10" runat="server" Value='<%# Eval("NGAY10") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="11" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NGAY11" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNgay11" runat="server" Text='<%# Eval("NGAY11") %>' CssClass="form-control text-box" MaxLength="1" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdNgay11" runat="server" Value='<%# Eval("NGAY11") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="12" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NGAY12" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNgay12" runat="server" Text='<%# Eval("NGAY12") %>' CssClass="form-control text-box" MaxLength="1" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdNgay12" runat="server" Value='<%# Eval("NGAY12") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="13" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NGAY13" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNgay13" runat="server" Text='<%# Eval("NGAY13") %>' CssClass="form-control text-box" MaxLength="1" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdNgay13" runat="server" Value='<%# Eval("NGAY13") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="14" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NGAY14" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNgay14" runat="server" Text='<%# Eval("NGAY14") %>' CssClass="form-control text-box" MaxLength="1" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdNgay14" runat="server" Value='<%# Eval("NGAY14") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="15" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NGAY15" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNgay15" runat="server" Text='<%# Eval("NGAY15") %>' CssClass="form-control text-box" MaxLength="1" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdNgay15" runat="server" Value='<%# Eval("NGAY15") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="16" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NGAY16" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNgay16" runat="server" Text='<%# Eval("NGAY16") %>' CssClass="form-control text-box" MaxLength="1" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdNgay16" runat="server" Value='<%# Eval("NGAY16") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="17" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NGAY17" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNgay17" runat="server" Text='<%# Eval("NGAY17") %>' CssClass="form-control text-box" MaxLength="1" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdNgay17" runat="server" Value='<%# Eval("NGAY17") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="18" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NGAY18" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNgay18" runat="server" Text='<%# Eval("NGAY18") %>' CssClass="form-control text-box" MaxLength="1" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdNgay18" runat="server" Value='<%# Eval("NGAY18") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="19" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NGAY19" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNgay19" runat="server" Text='<%# Eval("NGAY19") %>' CssClass="form-control text-box" MaxLength="1" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdNgay19" runat="server" Value='<%# Eval("NGAY19") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="20" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NGAY20" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNgay20" runat="server" Text='<%# Eval("NGAY20") %>' CssClass="form-control text-box" MaxLength="1" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdNgay20" runat="server" Value='<%# Eval("NGAY20") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="21" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NGAY21" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNgay21" runat="server" Text='<%# Eval("NGAY21") %>' CssClass="form-control text-box" MaxLength="1" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdNgay21" runat="server" Value='<%# Eval("NGAY21") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="22" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NGAY22" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNgay22" runat="server" Text='<%# Eval("NGAY22") %>' CssClass="form-control text-box" MaxLength="1" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdNgay22" runat="server" Value='<%# Eval("NGAY22") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="23" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NGAY23" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNGAY23" runat="server" Text='<%# Eval("NGAY23") %>' CssClass="form-control text-box" MaxLength="1" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdNgay23" runat="server" Value='<%# Eval("NGAY23") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="24" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NGAY24" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNgay24" runat="server" Text='<%# Eval("NGAY24") %>' CssClass="form-control text-box" MaxLength="1" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdNgay24" runat="server" Value='<%# Eval("NGAY24") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="25" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NGAY25" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNgay25" runat="server" Text='<%# Eval("NGAY25") %>' CssClass="form-control text-box" MaxLength="1" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdNgay25" runat="server" Value='<%# Eval("NGAY25") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="26" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NGAY26" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNgay26" runat="server" Text='<%# Eval("NGAY26") %>' CssClass="form-control text-box" MaxLength="1" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdNgay26" runat="server" Value='<%# Eval("NGAY26") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="27" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NGAY27" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNgay27" runat="server" Text='<%# Eval("NGAY27") %>' CssClass="form-control text-box" MaxLength="1" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdNgay27" runat="server" Value='<%# Eval("NGAY27") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="28" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NGAY28" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNgay28" runat="server" Text='<%# Eval("NGAY28") %>' CssClass="form-control text-box" MaxLength="1" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdNgay28" runat="server" Value='<%# Eval("NGAY28") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="29" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NGAY29" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNgay29" runat="server" Text='<%# Eval("NGAY29") %>' CssClass="form-control text-box" MaxLength="1" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdNgay29" runat="server" Value='<%# Eval("NGAY29") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="30" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NGAY30" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNgay30" runat="server" Text='<%# Eval("NGAY30") %>' CssClass="form-control text-box" MaxLength="1" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdNgay30" runat="server" Value='<%# Eval("NGAY30") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="31" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NGAY31" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNgay31" runat="server" Text='<%# Eval("NGAY31") %>' CssClass="form-control text-box" MaxLength="1" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdNgay31" runat="server" Value='<%# Eval("NGAY31") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="P" ColumnGroupName="SO_NGAY_NGHI" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="TONG_PHEP" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbTongPhep" runat="server" Text='<%# Eval("TONG_PHEP") %>' CssClass="form-control text-box" Enabled="false" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdTongPhep" runat="server" Value='<%# Eval("TONG_PHEP") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="K" ColumnGroupName="SO_NGAY_NGHI" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="TONG_KHONG_PHEP" ItemStyle-CssClass="grid-control" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbTongKhongPhep" runat="server" Text='<%# Eval("TONG_KHONG_PHEP") %>' CssClass="form-control text-box" Enabled="false" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdTongKhongPhep" runat="server" Value='<%# Eval("TONG_KHONG_PHEP") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
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
    </script>
</asp:Content>
