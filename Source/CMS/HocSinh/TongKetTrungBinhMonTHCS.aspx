<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="TongKetTrungBinhMonTHCS.aspx.cs" Inherits="CMS.HocSinh.TongKetTrungBinhMonTHCS" %>
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
                    <telerik:AjaxUpdatedControl ControlID="btSave" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="lblMess" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbLop">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="btSave" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="lblMess" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbHocKy">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="btnTongKet" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnTongKet">
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
            function ProcessCellValue(cell) {
                try {
                    if (cell != null) {
                        var cval;
                        console.log(cell.value);
                        cval = ChangeValidValueHanhKiem(cell.value);
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
            $(document).on("keyup", ".hanh-kiem", function (event) {
                if (event.which != 37 && event.which != 38 && event.which != 39 && event.which != 40) {
                    ProcessCellValue(this);
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
                <span class="item-title">Tổng kết điểm</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btnTongKet" runat="server" CssClass="btn bt-one" Text="Tổng kết" OnClientClick="if(!btLuuClick()) return false;" OnClick="btnTongKet_Click"/>
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
                <div class="col-sm-3">
                     <telerik:RadComboBox ID="rcbHocKy" runat="server" Width="100%" DataSourceID="objHocKy"
                        DataTextField="TEN" DataValueField="MA" Filter="Contains" OnSelectedIndexChanged="rcbHocKy_SelectedIndexChanged" AutoPostBack="true">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objHocKy" runat="server" SelectMethod="getHocKy" TypeName="OneEduDataAccess.BO.HocKyBO">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                            <asp:Parameter DefaultValue="" Name="id_all" Type="Int16" />
                            <asp:Parameter Name="text_all" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
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
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="" HeaderStyle-HorizontalAlign="Center" Name="Mon0"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" HeaderStyle-HorizontalAlign="Center" Name="Mon1"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" HeaderStyle-HorizontalAlign="Center" Name="Mon2"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" HeaderStyle-HorizontalAlign="Center" Name="Mon3"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" HeaderStyle-HorizontalAlign="Center" Name="Mon4"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" HeaderStyle-HorizontalAlign="Center" Name="Mon5"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" HeaderStyle-HorizontalAlign="Center" Name="Mon6"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" HeaderStyle-HorizontalAlign="Center" Name="Mon7"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" HeaderStyle-HorizontalAlign="Center" Name="Mon8"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" HeaderStyle-HorizontalAlign="Center" Name="Mon9"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" HeaderStyle-HorizontalAlign="Center" Name="Mon10"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" HeaderStyle-HorizontalAlign="Center" Name="Mon11"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" HeaderStyle-HorizontalAlign="Center" Name="Mon12"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" HeaderStyle-HorizontalAlign="Center" Name="Mon13"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" HeaderStyle-HorizontalAlign="Center" Name="Mon14"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" HeaderStyle-HorizontalAlign="Center" Name="Mon15"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" HeaderStyle-HorizontalAlign="Center" Name="Mon16"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" HeaderStyle-HorizontalAlign="Center" Name="Mon17"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" HeaderStyle-HorizontalAlign="Center" Name="Mon18"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" HeaderStyle-HorizontalAlign="Center" Name="Mon19"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" HeaderStyle-HorizontalAlign="Center" Name="Mon20"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" HeaderStyle-HorizontalAlign="Center" Name="Mon21"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" HeaderStyle-HorizontalAlign="Center" Name="Mon22"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" HeaderStyle-HorizontalAlign="Center" Name="Mon23"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" HeaderStyle-HorizontalAlign="Center" Name="Mon24"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Tổng kết" HeaderStyle-HorizontalAlign="Center" Name="TongKet"></telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="STT" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%# Container.DataSetIndex+1 %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="MA" HeaderStyle-Width="130px" FilterControlAltText="Filter MA column" HeaderText="Mã HS" SortExpression="MA" UniqueName="MA" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="HO_TEN" FilterControlAltText="Filter HO_TEN column" HeaderText="Tên HS" SortExpression="HO_TEN" UniqueName="HO_TEN" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="rgHeader head-list-grid headerGrid" ItemStyle-CssClass="grid-control headerGrid" HeaderStyle-Width="200px">
                        </telerik:GridBoundColumn>
                        <%--Mon 0--%>
                        <telerik:GridTemplateColumn HeaderText="HK1" ColumnGroupName="Mon0" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_0_Ky1">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_0_Ky1" runat="server" Text='<%# Eval("MON_0_Ky1") %>' CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_0_Ky1" runat="server" Value='<%# Eval("MON_0_Ky1") %>' />
                                <asp:HiddenField ID="hdKIEU_MON_0" runat="server" Value='<%# Eval("KIEU_MON_0") %>' />
                                <asp:HiddenField ID="hdHE_SO_0" runat="server" Value='<%# Eval("HE_SO_0") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HK2" ColumnGroupName="Mon0" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_0_Ky2">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_0_Ky2" runat="server" Text='<%# Eval("MON_0_Ky2") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_0_Ky2" runat="server" Value='<%# Eval("MON_0_Ky2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CN" ColumnGroupName="Mon0" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_0_CN">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_0_CN" runat="server" Text='<%# Eval("MON_0_CN") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_0_CN" runat="server" Value='<%# Eval("MON_0_CN") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--Mon 0--%>
                        <%-- Mon 1--%>
                        <telerik:GridTemplateColumn HeaderText="HK1" ColumnGroupName="Mon1" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_1_Ky1">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_1_Ky1" runat="server" Text='<%# Eval("MON_1_Ky1") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_1_Ky1" runat="server" Value='<%# Eval("MON_1_Ky1") %>' />
                                <asp:HiddenField ID="hdKIEU_MON_1" runat="server" Value='<%# Eval("KIEU_MON_1") %>' />
                                <asp:HiddenField ID="hdHE_SO_1" runat="server" Value='<%# Eval("HE_SO_1") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HK2" ColumnGroupName="Mon1" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_1_Ky2">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_1_Ky2" runat="server" Text='<%# Eval("MON_1_Ky2") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_1_Ky2" runat="server" Value='<%# Eval("MON_1_Ky2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CN" ColumnGroupName="Mon1" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_1_CN">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_1_CN" runat="server" Text='<%# Eval("MON_1_CN") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_1_CN" runat="server" Value='<%# Eval("MON_1_CN") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Mon 1--%>
                        <%--Mon 2--%>
                        <telerik:GridTemplateColumn HeaderText="HK1" ColumnGroupName="Mon2" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_2_Ky1">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_2_Ky1" runat="server" Text='<%# Eval("MON_2_Ky1") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_2_Ky1" runat="server" Value='<%# Eval("MON_2_Ky1") %>' />
                                <asp:HiddenField ID="hdKIEU_MON_2" runat="server" Value='<%# Eval("KIEU_MON_2") %>' />
                                <asp:HiddenField ID="hdHE_SO_2" runat="server" Value='<%# Eval("HE_SO_2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HK2" ColumnGroupName="Mon2" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_2_Ky2">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_2_Ky2" runat="server" Text='<%# Eval("MON_2_Ky2") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_2_Ky2" runat="server" Value='<%# Eval("MON_2_Ky2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CN" ColumnGroupName="Mon2" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_2_CN">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_2_CN" runat="server" Text='<%# Eval("MON_2_CN") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_2_CN" runat="server" Value='<%# Eval("MON_2_CN") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--Mon 2--%>
                        <%--Mon 3--%>
                        <telerik:GridTemplateColumn HeaderText="HK1" ColumnGroupName="Mon3" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_3_Ky1">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_3_Ky1" runat="server" Text='<%# Eval("MON_3_Ky1") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_3_Ky1" runat="server" Value='<%# Eval("MON_3_Ky1") %>' />
                                <asp:HiddenField ID="hdKIEU_MON_3" runat="server" Value='<%# Eval("KIEU_MON_3") %>' />
                                <asp:HiddenField ID="hdHE_SO_3" runat="server" Value='<%# Eval("HE_SO_3") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HK2" ColumnGroupName="Mon3" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_3_Ky2">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_3_Ky2" runat="server" Text='<%# Eval("MON_3_Ky2") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_3_Ky2" runat="server" Value='<%# Eval("MON_3_Ky2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CN" ColumnGroupName="Mon3" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_3_CN">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_3_CN" runat="server" Text='<%# Eval("MON_3_CN") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_3_CN" runat="server" Value='<%# Eval("MON_3_CN") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End Mon 3--%>
                        <%--Mon 4--%>
                        <telerik:GridTemplateColumn HeaderText="HK1" ColumnGroupName="Mon4" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_4_Ky1">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_4_Ky1" runat="server" Text='<%# Eval("MON_4_Ky1") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_4_Ky1" runat="server" Value='<%# Eval("MON_4_Ky1") %>' />
                                <asp:HiddenField ID="hdKIEU_MON_4" runat="server" Value='<%# Eval("KIEU_MON_4") %>' />
                                <asp:HiddenField ID="hdHE_SO_4" runat="server" Value='<%# Eval("HE_SO_4") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HK2" ColumnGroupName="Mon4" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_4_Ky2">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_4_Ky2" runat="server" Text='<%# Eval("MON_4_Ky2") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_4_Ky2" runat="server" Value='<%# Eval("MON_4_Ky2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CN" ColumnGroupName="Mon4" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_4_CN">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_4_CN" runat="server" Text='<%# Eval("MON_4_CN") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_4_CN" runat="server" Value='<%# Eval("MON_4_CN") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End Mon 4--%>
                        <%--Mon 5--%>
                        <telerik:GridTemplateColumn HeaderText="HK1" ColumnGroupName="Mon5" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_5_Ky1">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_5_Ky1" runat="server" Text='<%# Eval("MON_5_Ky1") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_5_Ky1" runat="server" Value='<%# Eval("MON_5_Ky1") %>' />
                                <asp:HiddenField ID="hdKIEU_MON_5" runat="server" Value='<%# Eval("KIEU_MON_5") %>' />
                                <asp:HiddenField ID="hdHE_SO_5" runat="server" Value='<%# Eval("HE_SO_5") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HK2" ColumnGroupName="Mon5" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_5_Ky2">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_5_Ky2" runat="server" Text='<%# Eval("MON_5_Ky2") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_5_Ky2" runat="server" Value='<%# Eval("MON_5_Ky2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CN" ColumnGroupName="Mon5" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_5_CN">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_5_CN" runat="server" Text='<%# Eval("MON_5_CN") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_5_CN" runat="server" Value='<%# Eval("MON_5_CN") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End Mon 5--%>
                        <%--Mon 6--%>
                        <telerik:GridTemplateColumn HeaderText="HK1" ColumnGroupName="Mon6" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_6_Ky1">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_6_Ky1" runat="server" Text='<%# Eval("MON_6_Ky1") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_6_Ky1" runat="server" Value='<%# Eval("MON_6_Ky1") %>' />
                                <asp:HiddenField ID="hdKIEU_MON_6" runat="server" Value='<%# Eval("KIEU_MON_6") %>' />
                                <asp:HiddenField ID="hdHE_SO_6" runat="server" Value='<%# Eval("HE_SO_6") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HK2" ColumnGroupName="Mon6" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_6_Ky2">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_6_Ky2" runat="server" Text='<%# Eval("MON_6_Ky2") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_6_Ky2" runat="server" Value='<%# Eval("MON_6_Ky2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CN" ColumnGroupName="Mon6" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_6_CN">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_6_CN" runat="server" Text='<%# Eval("MON_6_CN") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_6_CN" runat="server" Value='<%# Eval("MON_6_CN") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End Mon 6--%>
                        <%--Mon 7--%>
                        <telerik:GridTemplateColumn HeaderText="HK1" ColumnGroupName="Mon7" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_7_Ky1">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_7_Ky1" runat="server" Text='<%# Eval("MON_7_Ky1") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_7_Ky1" runat="server" Value='<%# Eval("MON_7_Ky1") %>' />
                                <asp:HiddenField ID="hdKIEU_MON_7" runat="server" Value='<%# Eval("KIEU_MON_7") %>' />
                                <asp:HiddenField ID="hdHE_SO_7" runat="server" Value='<%# Eval("HE_SO_7") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HK2" ColumnGroupName="Mon7" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_7_Ky2">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_7_Ky2" runat="server" Text='<%# Eval("MON_7_Ky2") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_7_Ky2" runat="server" Value='<%# Eval("MON_7_Ky2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CN" ColumnGroupName="Mon7" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_7_CN">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_7_CN" runat="server" Text='<%# Eval("MON_7_CN") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_7_CN" runat="server" Value='<%# Eval("MON_7_CN") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End Mon 7--%>
                        <%--Mon 8--%>
                        <telerik:GridTemplateColumn HeaderText="HK1" ColumnGroupName="Mon8" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_8_Ky1">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_8_Ky1" runat="server" Text='<%# Eval("MON_8_Ky1") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_8_Ky1" runat="server" Value='<%# Eval("MON_8_Ky1") %>' />
                                <asp:HiddenField ID="hdKIEU_MON_8" runat="server" Value='<%# Eval("KIEU_MON_8") %>' />
                                <asp:HiddenField ID="hdHE_SO_8" runat="server" Value='<%# Eval("HE_SO_8") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HK2" ColumnGroupName="Mon8" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_8_Ky2">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_8_Ky2" runat="server" Text='<%# Eval("MON_8_Ky2") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_8_Ky2" runat="server" Value='<%# Eval("MON_8_Ky2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CN" ColumnGroupName="Mon8" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_8_CN">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_8_CN" runat="server" Text='<%# Eval("MON_8_CN") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_8_CN" runat="server" Value='<%# Eval("MON_8_CN") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End Mon 8--%>
                        <%--Mon 9--%>
                        <telerik:GridTemplateColumn HeaderText="HK1" ColumnGroupName="Mon9" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_9_Ky1">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_9_Ky1" runat="server" Text='<%# Eval("MON_9_Ky1") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_9_Ky1" runat="server" Value='<%# Eval("MON_9_Ky1") %>' />
                                <asp:HiddenField ID="hdKIEU_MON_9" runat="server" Value='<%# Eval("KIEU_MON_9") %>' />
                                <asp:HiddenField ID="hdHE_SO_9" runat="server" Value='<%# Eval("HE_SO_9") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HK2" ColumnGroupName="Mon9" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_9_Ky2">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_9_Ky2" runat="server" Text='<%# Eval("MON_9_Ky2") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_9_Ky2" runat="server" Value='<%# Eval("MON_9_Ky2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CN" ColumnGroupName="Mon9" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_9_CN">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_9_CN" runat="server" Text='<%# Eval("MON_9_CN") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_9_CN" runat="server" Value='<%# Eval("MON_9_CN") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End Mon 9--%>
                       <%--Mon 10--%>
                        <telerik:GridTemplateColumn HeaderText="HK1" ColumnGroupName="Mon10" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_10_Ky1">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_10_Ky1" runat="server" Text='<%# Eval("MON_10_Ky1") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_10_Ky1" runat="server" Value='<%# Eval("MON_10_Ky1") %>' />
                                <asp:HiddenField ID="hdKIEU_MON_10" runat="server" Value='<%# Eval("KIEU_MON_10") %>' />
                                <asp:HiddenField ID="hdHE_SO_10" runat="server" Value='<%# Eval("HE_SO_10") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HK2" ColumnGroupName="Mon10" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_10_Ky2">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_10_Ky2" runat="server" Text='<%# Eval("MON_10_Ky2") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_10_Ky2" runat="server" Value='<%# Eval("MON_10_Ky2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CN" ColumnGroupName="Mon10" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_10_CN">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_10_CN" runat="server" Text='<%# Eval("MON_10_CN") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_10_CN" runat="server" Value='<%# Eval("MON_10_CN") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End Mon 10--%>
                        <%--Mon 11--%>
                        <telerik:GridTemplateColumn HeaderText="HK1" ColumnGroupName="Mon11" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_11_Ky1">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_11_Ky1" runat="server" Text='<%# Eval("MON_11_Ky1") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_11_Ky1" runat="server" Value='<%# Eval("MON_11_Ky1") %>' />
                                <asp:HiddenField ID="hdKIEU_MON_11" runat="server" Value='<%# Eval("KIEU_MON_11") %>' />
                                <asp:HiddenField ID="hdHE_SO_11" runat="server" Value='<%# Eval("HE_SO_11") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HK2" ColumnGroupName="Mon11" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_11_Ky2">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_11_Ky2" runat="server" Text='<%# Eval("MON_11_Ky2") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_11_Ky2" runat="server" Value='<%# Eval("MON_11_Ky2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CN" ColumnGroupName="Mon11" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_11_CN">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_11_CN" runat="server" Text='<%# Eval("MON_11_CN") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_11_CN" runat="server" Value='<%# Eval("MON_11_CN") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End Mon 11--%>
                        <%--Mon 12--%>
                        <telerik:GridTemplateColumn HeaderText="HK1" ColumnGroupName="Mon12" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_12_Ky1">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_12_Ky1" runat="server" Text='<%# Eval("MON_12_Ky1") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_12_Ky1" runat="server" Value='<%# Eval("MON_12_Ky1") %>' />
                                <asp:HiddenField ID="hdKIEU_MON_12" runat="server" Value='<%# Eval("KIEU_MON_12") %>' />
                                <asp:HiddenField ID="hdHE_SO_12" runat="server" Value='<%# Eval("HE_SO_12") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HK2" ColumnGroupName="Mon12" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_12_Ky2">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_12_Ky2" runat="server" Text='<%# Eval("MON_12_Ky2") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_12_Ky2" runat="server" Value='<%# Eval("MON_12_Ky2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CN" ColumnGroupName="Mon12" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_12_CN">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_12_CN" runat="server" Text='<%# Eval("MON_12_CN") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_12_CN" runat="server" Value='<%# Eval("MON_12_CN") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End Mon 12--%>
                        <%--Mon 13--%>
                        <telerik:GridTemplateColumn HeaderText="HK1" ColumnGroupName="Mon13" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_13_Ky1">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_13_Ky1" runat="server" Text='<%# Eval("MON_13_Ky1") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_13_Ky1" runat="server" Value='<%# Eval("MON_13_Ky1") %>' />
                                <asp:HiddenField ID="hdKIEU_MON_13" runat="server" Value='<%# Eval("KIEU_MON_13") %>' />
                                <asp:HiddenField ID="hdHE_SO_13" runat="server" Value='<%# Eval("HE_SO_13") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HK2" ColumnGroupName="Mon13" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_13_Ky2">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_13_Ky2" runat="server" Text='<%# Eval("MON_13_Ky2") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_13_Ky2" runat="server" Value='<%# Eval("MON_13_Ky2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CN" ColumnGroupName="Mon13" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_13_CN">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_13_CN" runat="server" Text='<%# Eval("MON_13_CN") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_13_CN" runat="server" Value='<%# Eval("MON_13_CN") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--Mon 13--%>
                        <%--Mon 14--%>
                        <telerik:GridTemplateColumn HeaderText="HK1" ColumnGroupName="Mon14" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_14_Ky1">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_14_Ky1" runat="server" Text='<%# Eval("MON_14_Ky1") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_14_Ky1" runat="server" Value='<%# Eval("MON_14_Ky1") %>' />
                                <asp:HiddenField ID="hdKIEU_MON_14" runat="server" Value='<%# Eval("KIEU_MON_14") %>' />
                                <asp:HiddenField ID="hdHE_SO_14" runat="server" Value='<%# Eval("HE_SO_14") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HK2" ColumnGroupName="Mon14" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_14_Ky2">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_14_Ky2" runat="server" Text='<%# Eval("MON_14_Ky2") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_14_Ky2" runat="server" Value='<%# Eval("MON_14_Ky2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CN" ColumnGroupName="Mon14" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_14_CN">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_14_CN" runat="server" Text='<%# Eval("MON_14_CN") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_14_CN" runat="server" Value='<%# Eval("MON_14_CN") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End Mon 4--%>
                        <%--Mon 15--%>
                        <telerik:GridTemplateColumn HeaderText="HK1" ColumnGroupName="Mon15" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_15_Ky1">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_15_Ky1" runat="server" Text='<%# Eval("MON_15_Ky1") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_15_Ky1" runat="server" Value='<%# Eval("MON_15_Ky1") %>' />
                                <asp:HiddenField ID="hdKIEU_MON_15" runat="server" Value='<%# Eval("KIEU_MON_15") %>' />
                                <asp:HiddenField ID="hdHE_SO_15" runat="server" Value='<%# Eval("HE_SO_15") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HK2" ColumnGroupName="Mon15" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_15_Ky2">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_15_Ky2" runat="server" Text='<%# Eval("MON_15_Ky2") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_15_Ky2" runat="server" Value='<%# Eval("MON_15_Ky2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CN" ColumnGroupName="Mon15" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_15_CN">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_15_CN" runat="server" Text='<%# Eval("MON_15_CN") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_15_CN" runat="server" Value='<%# Eval("MON_15_CN") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End Mon 5--%>
                        <%--Mon 16--%>
                        <telerik:GridTemplateColumn HeaderText="HK1" ColumnGroupName="Mon16" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_16_Ky1">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_16_Ky1" runat="server" Text='<%# Eval("MON_16_Ky1") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_16_Ky1" runat="server" Value='<%# Eval("MON_16_Ky1") %>' />
                                <asp:HiddenField ID="hdKIEU_MON_16" runat="server" Value='<%# Eval("KIEU_MON_16") %>' />
                                <asp:HiddenField ID="hdHE_SO_16" runat="server" Value='<%# Eval("HE_SO_16") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HK2" ColumnGroupName="Mon16" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_16_Ky2">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_16_Ky2" runat="server" Text='<%# Eval("MON_16_Ky2") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_16_Ky2" runat="server" Value='<%# Eval("MON_16_Ky2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CN" ColumnGroupName="Mon16" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_16_CN">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_16_CN" runat="server" Text='<%# Eval("MON_16_CN") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_16_CN" runat="server" Value='<%# Eval("MON_16_CN") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End Mon 16--%>
                        <%--Mon 17--%>
                        <telerik:GridTemplateColumn HeaderText="HK1" ColumnGroupName="Mon17" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_17_Ky1">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_17_Ky1" runat="server" Text='<%# Eval("MON_17_Ky1") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_17_Ky1" runat="server" Value='<%# Eval("MON_17_Ky1") %>' />
                                <asp:HiddenField ID="hdKIEU_MON_17" runat="server" Value='<%# Eval("KIEU_MON_17") %>' />
                                <asp:HiddenField ID="hdHE_SO_17" runat="server" Value='<%# Eval("HE_SO_17") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HK2" ColumnGroupName="Mon17" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_17_Ky2">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_17_Ky2" runat="server" Text='<%# Eval("MON_17_Ky2") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_17_Ky2" runat="server" Value='<%# Eval("MON_17_Ky2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CN" ColumnGroupName="Mon17" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_17_CN">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_17_CN" runat="server" Text='<%# Eval("MON_17_CN") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_17_CN" runat="server" Value='<%# Eval("MON_17_CN") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End Mon 17--%>
                        <%--Mon 18--%>
                        <telerik:GridTemplateColumn HeaderText="HK1" ColumnGroupName="Mon18" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_18_Ky1">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_18_Ky1" runat="server" Text='<%# Eval("MON_18_Ky1") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_18_Ky1" runat="server" Value='<%# Eval("MON_18_Ky1") %>' />
                                <asp:HiddenField ID="hdKIEU_MON_18" runat="server" Value='<%# Eval("KIEU_MON_18") %>' />
                                <asp:HiddenField ID="hdHE_SO_18" runat="server" Value='<%# Eval("HE_SO_18") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HK2" ColumnGroupName="Mon18" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_18_Ky2">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_18_Ky2" runat="server" Text='<%# Eval("MON_18_Ky2") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_18_Ky2" runat="server" Value='<%# Eval("MON_18_Ky2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CN" ColumnGroupName="Mon18" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_18_CN">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_18_CN" runat="server" Text='<%# Eval("MON_18_CN") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_18_CN" runat="server" Value='<%# Eval("MON_18_CN") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End Mon 18--%>
                        <%--Mon 19--%>
                        <telerik:GridTemplateColumn HeaderText="HK1" ColumnGroupName="Mon19" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_19_Ky1">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_19_Ky1" runat="server" Text='<%# Eval("MON_19_Ky1") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_19_Ky1" runat="server" Value='<%# Eval("MON_19_Ky1") %>' />
                                <asp:HiddenField ID="hdKIEU_MON_19" runat="server" Value='<%# Eval("KIEU_MON_19") %>' />
                                <asp:HiddenField ID="hdHE_SO_19" runat="server" Value='<%# Eval("HE_SO_19") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HK2" ColumnGroupName="Mon19" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_19_Ky2">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_19_Ky2" runat="server" Text='<%# Eval("MON_19_Ky2") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_19_Ky2" runat="server" Value='<%# Eval("MON_19_Ky2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CN" ColumnGroupName="Mon19" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_19_CN">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_19_CN" runat="server" Text='<%# Eval("MON_19_CN") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_19_CN" runat="server" Value='<%# Eval("MON_19_CN") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End Mon 19--%>
                       <%--Mon 20--%>
                        <telerik:GridTemplateColumn HeaderText="HK1" ColumnGroupName="Mon20" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_20_Ky1">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_20_Ky1" runat="server" Text='<%# Eval("MON_20_Ky1") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_20_Ky1" runat="server" Value='<%# Eval("MON_20_Ky1") %>' />
                                <asp:HiddenField ID="hdKIEU_MON_20" runat="server" Value='<%# Eval("KIEU_MON_20") %>' />
                                <asp:HiddenField ID="hdHE_SO_20" runat="server" Value='<%# Eval("HE_SO_20") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HK2" ColumnGroupName="Mon20" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_20_Ky2">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_20_Ky2" runat="server" Text='<%# Eval("MON_20_Ky2") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_20_Ky2" runat="server" Value='<%# Eval("MON_20_Ky2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CN" ColumnGroupName="Mon20" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_20_CN">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_20_CN" runat="server" Text='<%# Eval("MON_20_CN") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_20_CN" runat="server" Value='<%# Eval("MON_20_CN") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End Mon 20--%>
                        <%-- Mon 21--%>
                        <telerik:GridTemplateColumn HeaderText="HK1" ColumnGroupName="Mon21" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_21_Ky1">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_21_Ky1" runat="server" Text='<%# Eval("MON_21_Ky1") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_21_Ky1" runat="server" Value='<%# Eval("MON_21_Ky1") %>' />
                                <asp:HiddenField ID="hdKIEU_MON_21" runat="server" Value='<%# Eval("KIEU_MON_21") %>' />
                                <asp:HiddenField ID="hdHE_SO_21" runat="server" Value='<%# Eval("HE_SO_21") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HK2" ColumnGroupName="Mon21" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_21_Ky2">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_21_Ky2" runat="server" Text='<%# Eval("MON_21_Ky2") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_21_Ky2" runat="server" Value='<%# Eval("MON_21_Ky2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CN" ColumnGroupName="Mon21" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_21_CN">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_21_CN" runat="server" Text='<%# Eval("MON_21_CN") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_21_CN" runat="server" Value='<%# Eval("MON_21_CN") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Mon 21--%>
                        <%--Mon 22--%>
                        <telerik:GridTemplateColumn HeaderText="HK1" ColumnGroupName="Mon22" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_22_Ky1">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_22_Ky1" runat="server" Text='<%# Eval("MON_22_Ky1") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_22_Ky1" runat="server" Value='<%# Eval("MON_22_Ky1") %>' />
                                <asp:HiddenField ID="hdKIEU_MON_22" runat="server" Value='<%# Eval("KIEU_MON_22") %>' />
                                <asp:HiddenField ID="hdHE_SO_22" runat="server" Value='<%# Eval("HE_SO_22") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HK2" ColumnGroupName="Mon22" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_22_Ky2">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_22_Ky2" runat="server" Text='<%# Eval("MON_22_Ky2") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_22_Ky2" runat="server" Value='<%# Eval("MON_22_Ky2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CN" ColumnGroupName="Mon22" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_22_CN">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_22_CN" runat="server" Text='<%# Eval("MON_22_CN") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_22_CN" runat="server" Value='<%# Eval("MON_22_CN") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--Mon 22--%>
                        <%--Mon 23--%>
                        <telerik:GridTemplateColumn HeaderText="HK1" ColumnGroupName="Mon23" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_23_Ky1">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_23_Ky1" runat="server" Text='<%# Eval("MON_23_Ky1") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_23_Ky1" runat="server" Value='<%# Eval("MON_23_Ky1") %>' />
                                <asp:HiddenField ID="hdKIEU_MON_23" runat="server" Value='<%# Eval("KIEU_MON_23") %>' />
                                <asp:HiddenField ID="hdHE_SO_23" runat="server" Value='<%# Eval("HE_SO_23") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HK2" ColumnGroupName="Mon23" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_23_Ky2">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_23_Ky2" runat="server" Text='<%# Eval("MON_23_Ky2") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_23_Ky2" runat="server" Value='<%# Eval("MON_23_Ky2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CN" ColumnGroupName="Mon23" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_23_CN">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_23_CN" runat="server" Text='<%# Eval("MON_23_CN") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_23_CN" runat="server" Value='<%# Eval("MON_23_CN") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End Mon 23--%>
                        <%--Mon 24--%>
                        <telerik:GridTemplateColumn HeaderText="HK1" ColumnGroupName="Mon24" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_24_Ky1">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_24_Ky1" runat="server" Text='<%# Eval("MON_24_Ky1") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_24_Ky1" runat="server" Value='<%# Eval("MON_24_Ky1") %>' />
                                <asp:HiddenField ID="hdKIEU_MON_24" runat="server" Value='<%# Eval("KIEU_MON_24") %>' />
                                <asp:HiddenField ID="hdHE_SO_24" runat="server" Value='<%# Eval("HE_SO_24") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HK2" ColumnGroupName="Mon24" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_24_Ky2">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_24_Ky2" runat="server" Text='<%# Eval("MON_24_Ky2") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_24_Ky2" runat="server" Value='<%# Eval("MON_24_Ky2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CN" ColumnGroupName="Mon24" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MON_24_CN">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMON_24_CN" runat="server" Text='<%# Eval("MON_24_CN") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdMON_24_CN" runat="server" Value='<%# Eval("MON_24_CN") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End Mon 24--%>
                        <%--Tổng kết--%>
                        <telerik:GridTemplateColumn HeaderText="HK1" ColumnGroupName="TongKet" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="TB_KY1">
                            <ItemTemplate>
                                <asp:TextBox ID="tbTB_KY1" runat="server" Text='<%# Eval("TB_KY1") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdTB_KY1" runat="server" Value='<%# Eval("TB_KY1") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HK2" ColumnGroupName="TongKet" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="TB_KY2">
                            <ItemTemplate>
                                <asp:TextBox ID="tbTB_KY2" runat="server" Text='<%# Eval("TB_KY2") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdTB_KY2" runat="server" Value='<%# Eval("TB_KY2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CN" ColumnGroupName="TongKet" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="TB_CN">
                            <ItemTemplate>
                                <asp:TextBox ID="tbTB_CN" runat="server" Text='<%# Eval("TB_CN") %>'  CssClass="form-control text-box" style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdTB_CN" runat="server" Value='<%# Eval("TB_CN") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End Tổng kết--%>
                    </Columns>
                    <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>