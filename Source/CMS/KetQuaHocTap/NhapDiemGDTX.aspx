<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="NhapDiemGDTX.aspx.cs" Inherits="CMS.KetQuaHocTap.NhapDiemGDTX" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" >
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbLoaiLopGDTX">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbKhoiHoc" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="rcbLop" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="rcbMonHoc" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="btSave" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="lblMess" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
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
                <span class="item-title">Nhập điểm</span>
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
            <div class="row">
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbLoaiLopGDTX" runat="server" Width="100%" AutoPostBack="True"
                        OnSelectedIndexChanged="rcbLoaiLopGDTX_SelectedIndexChanged" Filter="Contains">
                        <Items>
                            <telerik:RadComboBoxItem Value="2" Text="Bổ túc THCS" />
                            <telerik:RadComboBoxItem Value="3" Text="Bổ túc THPT" />
                        </Items>
                    </telerik:RadComboBox>
                </div>
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbKhoiHoc" runat="server" Width="100%" DataSourceID="objKhoiHoc"
                        DataTextField="TEN" DataValueField="MA" AutoPostBack="True"
                        OnSelectedIndexChanged="rcbKhoiHoc_SelectedIndexChanged" Filter="Contains">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objKhoiHoc" runat="server" SelectMethod="getKhoiByCapHoc" TypeName="OneEduDataAccess.BO.KhoiBO">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="rcbLoaiLopGDTX" Name="maLoaiLopGDTX" PropertyName="SelectedValue" />
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
                    <asp:Label ID="lblMess" runat="server" Text="" style="color:red"></asp:Label>
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
                        <telerik:GridColumnGroup HeaderText="Điểm hệ số 1" HeaderStyle-HorizontalAlign="Center" Name="HS1"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Miệng" HeaderStyle-HorizontalAlign="Center" Name="MIENG_HS1" ParentGroupName="HS1"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="15 phút" HeaderStyle-HorizontalAlign="Center" Name="DIEM_15P" ParentGroupName="HS1"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="1 tiết" HeaderStyle-HorizontalAlign="Center" Name="DIEM_1T_HS1" ParentGroupName="HS1"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Điểm hệ số 2" HeaderStyle-HorizontalAlign="Center" Name="HS2"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="1 tiết" HeaderStyle-HorizontalAlign="Center" Name="DIEM_1T_HS2" ParentGroupName="HS2"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Thực hành" HeaderStyle-HorizontalAlign="Center" Name="TH_HS2" ParentGroupName="HS2"></telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="STT" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%# Container.DataSetIndex+1 %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="MA_HS" HeaderStyle-Width="120px" FilterControlAltText="Filter MA_HS column" HeaderText="Mã HS" SortExpression="MA_HS" UniqueName="MA_HS" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TEN_HS" HeaderStyle-Width="200px" FilterControlAltText="Filter TEN_HS column" HeaderText="Tên HS" SortExpression="TEN_HS" UniqueName="TEN_HS" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NGAY_SINH" HeaderStyle-Width="100px" FilterControlAltText="Filter NGAY_SINH column" HeaderText="Ngày sinh" SortExpression="NGAY_SINH" UniqueName="NGAY_SINH" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}">
                        </telerik:GridBoundColumn>
                        <%--Hệ số 1--%>
                        <%-- Miệng --%>
                        <telerik:GridTemplateColumn HeaderText="1" ColumnGroupName="MIENG_HS1" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="DIEM1_VIEW">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDIEM1" runat="server" Text='<%# Eval("DIEM1_VIEW") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdDIEM1" runat="server" Value='<%# Eval("DIEM1_VIEW") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="2" ColumnGroupName="MIENG_HS1" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="DIEM2_VIEW">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDIEM2" runat="server" Text='<%# Eval("DIEM2_VIEW") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdDIEM2" runat="server" Value='<%# Eval("DIEM2_VIEW") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="3" ColumnGroupName="MIENG_HS1" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="DIEM3_VIEW">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDIEM3" runat="server" Text='<%# Eval("DIEM3_VIEW") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdDIEM3" runat="server" Value='<%# Eval("DIEM3_VIEW") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="4" ColumnGroupName="MIENG_HS1" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="DIEM4_VIEW">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDIEM4" runat="server" Text='<%# Eval("DIEM4_VIEW") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdDIEM4" runat="server" Value='<%# Eval("DIEM4_VIEW") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="5" ColumnGroupName="MIENG_HS1" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="DIEM5_VIEW">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDIEM5" runat="server" Text='<%# Eval("DIEM5_VIEW") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdDIEM5" runat="server" Value='<%# Eval("DIEM5_VIEW") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Miệng--%>
                        <%-- Viết --%>
                        <telerik:GridTemplateColumn HeaderText="1" ColumnGroupName="DIEM_15P" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="DIEM6_VIEW">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDIEM6" runat="server" Text='<%# Eval("DIEM6_VIEW") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdDIEM6" runat="server" Value='<%# Eval("DIEM6_VIEW") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="2" ColumnGroupName="DIEM_15P" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="DIEM7_VIEW">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDIEM7" runat="server" Text='<%# Eval("DIEM7_VIEW") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdDIEM7" runat="server" Value='<%# Eval("DIEM7_VIEW") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="3" ColumnGroupName="DIEM_15P" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="DIEM8_VIEW">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDIEM8" runat="server" Text='<%# Eval("DIEM8_VIEW") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdDIEM8" runat="server" Value='<%# Eval("DIEM8_VIEW") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="4" ColumnGroupName="DIEM_15P" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="DIEM9_VIEW">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDIEM9" runat="server" Text='<%# Eval("DIEM9_VIEW") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdDIEM9" runat="server" Value='<%# Eval("DIEM9_VIEW") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="5" ColumnGroupName="DIEM_15P" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="DIEM10_VIEW">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDIEM10" runat="server" Text='<%# Eval("DIEM10_VIEW") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdDIEM10" runat="server" Value='<%# Eval("DIEM10_VIEW") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Viết--%>
                        <%-- Thực hành --%>
                        <telerik:GridTemplateColumn HeaderText="1" ColumnGroupName="DIEM_1T_HS1" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="DIEM11_VIEW">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDIEM11" runat="server" Text='<%# Eval("DIEM11_VIEW") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdDIEM11" runat="server" Value='<%# Eval("DIEM11_VIEW") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="2" ColumnGroupName="DIEM_1T_HS1" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="DIEM12_VIEW">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDIEM12" runat="server" Text='<%# Eval("DIEM12_VIEW") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdDIEM12" runat="server" Value='<%# Eval("DIEM12_VIEW") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="3" ColumnGroupName="DIEM_1T_HS1" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="DIEM13_VIEW">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDIEM13" runat="server" Text='<%# Eval("DIEM13_VIEW") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdDIEM13" runat="server" Value='<%# Eval("DIEM13_VIEW") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="4" ColumnGroupName="DIEM_1T_HS1" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="DIEM14_VIEW">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDIEM14" runat="server" Text='<%# Eval("DIEM14_VIEW") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdDIEM14" runat="server" Value='<%# Eval("DIEM14_VIEW") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="5" ColumnGroupName="DIEM_1T_HS1" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="DIEM15_VIEW">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDIEM15" runat="server" Text='<%# Eval("DIEM15_VIEW") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdDIEM15" runat="server" Value='<%# Eval("DIEM15_VIEW") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Thực hành--%>
                        <%--Hệ số 1--%>
                        <%-- Viết --%>
                        <telerik:GridTemplateColumn HeaderText="1" ColumnGroupName="DIEM_1T_HS2" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="DIEM16_VIEW">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDIEM16" runat="server" Text='<%# Eval("DIEM16_VIEW") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdDIEM16" runat="server" Value='<%# Eval("DIEM16_VIEW") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="2" ColumnGroupName="DIEM_1T_HS2" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="DIEM17_VIEW">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDIEM17" runat="server" Text='<%# Eval("DIEM17_VIEW") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdDIEM17" runat="server" Value='<%# Eval("DIEM17_VIEW") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="3" ColumnGroupName="DIEM_1T_HS2" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="DIEM18_VIEW">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDIEM18" runat="server" Text='<%# Eval("DIEM18_VIEW") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdDIEM18" runat="server" Value='<%# Eval("DIEM18_VIEW") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="4" ColumnGroupName="DIEM_1T_HS2" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="DIEM19_VIEW">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDIEM19" runat="server" Text='<%# Eval("DIEM19_VIEW") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdDIEM19" runat="server" Value='<%# Eval("DIEM19_VIEW") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="5" ColumnGroupName="DIEM_1T_HS2" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="DIEM20_VIEW">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDIEM20" runat="server" Text='<%# Eval("DIEM20_VIEW") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdDIEM20" runat="server" Value='<%# Eval("DIEM20_VIEW") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Viết--%>
                        <%-- Thực hành --%>
                        <telerik:GridTemplateColumn HeaderText="1" ColumnGroupName="TH_HS2" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="DIEM21_VIEW">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDIEM21" runat="server" Text='<%# Eval("DIEM21_VIEW") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdDIEM21" runat="server" Value='<%# Eval("DIEM21_VIEW") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="2" ColumnGroupName="TH_HS2" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="DIEM22_VIEW">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDIEM22" runat="server" Text='<%# Eval("DIEM22_VIEW") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdDIEM22" runat="server" Value='<%# Eval("DIEM22_VIEW") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="3" ColumnGroupName="TH_HS2" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="DIEM23_VIEW">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDIEM23" runat="server" Text='<%# Eval("DIEM23_VIEW") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdDIEM23" runat="server" Value='<%# Eval("DIEM23_VIEW") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="4" ColumnGroupName="TH_HS2" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="DIEM24_VIEW">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDIEM24" runat="server" Text='<%# Eval("DIEM24_VIEW") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdDIEM24" runat="server" Value='<%# Eval("DIEM24_VIEW") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="5" ColumnGroupName="TH_HS2" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="DIEM25_VIEW">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDIEM25" runat="server" Text='<%# Eval("DIEM25_VIEW") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdDIEM25" runat="server" Value='<%# Eval("DIEM25_VIEW") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Thực hành--%>
                        <%-- Điểm học kỳ --%>
                        <telerik:GridTemplateColumn HeaderText="HK" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="DIEM_HOC_KY_VIEW">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDIEM_HOC_KY" runat="server" Text='<%# Eval("DIEM_HOC_KY_VIEW") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdDIEM_HOC_KY" runat="server" Value='<%# Eval("DIEM_HOC_KY_VIEW") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Điểm học kỳ--%>
                        <%-- TB môn kỳ 1 --%>
                        <telerik:GridTemplateColumn HeaderText="TBMK1" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="DIEM_TRUNG_BINH_KY1_VIEW">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDIEM_TRUNG_BINH_KY1" runat="server" Text='<%# Eval("DIEM_TRUNG_BINH_KY1_VIEW") %>' CssClass="form-control text-box" Enabled="false"></asp:TextBox>
                                <asp:HiddenField ID="hdDIEM_TRUNG_BINH_KY1" runat="server" Value='<%# Eval("DIEM_TRUNG_BINH_KY1_VIEW") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End TB môn kỳ 1--%>
                        <%-- TB môn kỳ 2 --%>
                        <telerik:GridTemplateColumn HeaderText="TBMK2" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="DIEM_TRUNG_BINH_KY2_VIEW">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDIEM_TRUNG_BINH_KY2" runat="server" Text='<%# Eval("DIEM_TRUNG_BINH_KY2_VIEW") %>' CssClass="form-control text-box" Enabled="false"></asp:TextBox>
                                <asp:HiddenField ID="hdDIEM_TRUNG_BINH_KY2" runat="server" Value='<%# Eval("DIEM_TRUNG_BINH_KY2_VIEW") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End TB môn kỳ 1--%>
                        <%-- TB môn CN--%>
                        <telerik:GridTemplateColumn HeaderText="TBMCN" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-CssClass="grid-control" Display="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="DIEM_TRUNG_BINH_CN_VIEW">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDIEM_TRUNG_BINH_CN" runat="server" Text='<%# Eval("DIEM_TRUNG_BINH_CN_VIEW") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdDIEM_TRUNG_BINH_CN" runat="server" Value='<%# Eval("DIEM_TRUNG_BINH_CN_VIEW") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End TB môn CN--%>
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
