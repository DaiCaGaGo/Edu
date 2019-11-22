<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="NhapHanhKiem.aspx.cs" Inherits="CMS.HocSinh.NhapHanhKiem" %>
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

            function CheckBoxHeaderClick(cb, cssLst) {
                $('.' + cssLst + ' input').each(function () {
                    if ($(this).attr('disabled') != "disabled") {
                        $(this).prop('checked', cb.checked);
                    }
                });
            }
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
                <span class="item-title">Nhập hạnh kiểm</span>
            </div>
            <div class="col-sm-8">
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
                        <%--<td>
                            <asp:Button ID="btExport" runat="server" CssClass="btn bt-one" OnClick="btExport_Click" Text="Xuất excel" />
                        </td>--%>
                    </tr>
                </table>
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
                        <telerik:GridColumnGroup HeaderText="Điểm tổng kết" HeaderStyle-HorizontalAlign="Center" Name="DKT"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Học lực" HeaderStyle-HorizontalAlign="Center" Name="HL"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Hạnh kiểm" HeaderStyle-HorizontalAlign="Center" Name="HK"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Danh hiệu" HeaderStyle-HorizontalAlign="Center" Name="DH"></telerik:GridColumnGroup>
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
                        <%--Tổng kết điểm--%>
                        <telerik:GridTemplateColumn HeaderText="HK1" ColumnGroupName="DKT" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="TB_KY1">
                            <ItemTemplate>
                                <asp:TextBox ID="tbTB_KY1" runat="server" Text='<%# Eval("TB_KY1") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdTB_KY1" runat="server" Value='<%# Eval("TB_KY1") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HK2" ColumnGroupName="DKT" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="TB_KY2">
                            <ItemTemplate>
                                <asp:TextBox ID="tbTB_KY2" runat="server" Text='<%# Eval("TB_KY2") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdTB_KY2" runat="server" Value='<%# Eval("TB_KY2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CN" ColumnGroupName="DKT" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="TB_CN">
                            <ItemTemplate>
                                <asp:TextBox ID="tbTB_CN" runat="server" Text='<%# Eval("TB_CN") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdTB_CN" runat="server" Value='<%# Eval("TB_CN") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Tổng kết điểm--%>
                        <%--Học lực--%>
                        <telerik:GridTemplateColumn HeaderText="HK1" ColumnGroupName="HL" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MA_HOC_LUC_KY1">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMA_HOC_LUC_KY1" runat="server" Text='<%# Eval("MA_HOC_LUC_KY1") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdMA_HOC_LUC_KY1" runat="server" Value='<%# Eval("MA_HOC_LUC_KY1") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HK2" ColumnGroupName="HL" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MA_HOC_LUC_KY2">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMA_HOC_LUC_KY2" runat="server" Text='<%# Eval("MA_HOC_LUC_KY2") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdMA_HOC_LUC_KY2" runat="server" Value='<%# Eval("MA_HOC_LUC_KY2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CN" ColumnGroupName="HL" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MA_HOC_LUC_CA_NAM">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMA_HOC_LUC_CA_NAM" runat="server" Text='<%# Eval("MA_HOC_LUC_CA_NAM") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdMA_HOC_LUC_CA_NAM" runat="server" Value='<%# Eval("MA_HOC_LUC_CA_NAM") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Học lực--%>
                        <%-- Hạnh kiểm --%>
                        <telerik:GridTemplateColumn HeaderText="HK1" ColumnGroupName="HK" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MA_HANH_KIEM_KY1">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMA_HANH_KIEM_KY1" runat="server" Text='<%# Eval("MA_HANH_KIEM_KY1") %>' CssClass="form-control text-box hanh-kiem"></asp:TextBox>
                                <asp:HiddenField ID="hdMA_HANH_KIEM_KY1" runat="server" Value='<%# Eval("MA_HANH_KIEM_KY1") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HK2" ColumnGroupName="HK" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MA_HANH_KIEM_KY2">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMA_HANH_KIEM_KY2" runat="server" Text='<%# Eval("MA_HANH_KIEM_KY2") %>' CssClass="form-control text-box hanh-kiem"></asp:TextBox>
                                <asp:HiddenField ID="hdMA_HANH_KIEM_KY2" runat="server" Value='<%# Eval("MA_HANH_KIEM_KY2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CN" ColumnGroupName="HK" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MA_HANH_KIEM_CA_NAM">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMA_HANH_KIEM_CA_NAM" runat="server" Text='<%# Eval("MA_HANH_KIEM_CA_NAM") %>' CssClass="form-control text-box hanh-kiem"></asp:TextBox>
                                <asp:HiddenField ID="hdMA_HANH_KIEM_CA_NAM" runat="server" Value='<%# Eval("MA_HANH_KIEM_CA_NAM") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Hạnh kiểm--%>
                        <%-- Danh hiệu --%>
                        <telerik:GridTemplateColumn HeaderText="HK1" ColumnGroupName="DH" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MA_DANH_HIEU_KY1">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMA_DANH_HIEU_KY1" runat="server" Text='<%# Eval("MA_DANH_HIEU_KY1") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdMA_DANH_HIEU_KY1" runat="server" Value='<%# Eval("MA_DANH_HIEU_KY1") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HK2" ColumnGroupName="DH" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MA_DANH_HIEU_KY2">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMA_DANH_HIEU_KY2" runat="server" Text='<%# Eval("MA_DANH_HIEU_KY2") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdMA_DANH_HIEU_KY2" runat="server" Value='<%# Eval("MA_DANH_HIEU_KY2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CN" ColumnGroupName="DH" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="MA_DANH_HIEU_CN">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMA_DANH_HIEU_CN" runat="server" Text='<%# Eval("MA_DANH_HIEU_CN") %>' CssClass="form-control text-box"></asp:TextBox>
                                <asp:HiddenField ID="hdMA_DANH_HIEU_CN" runat="server" Value='<%# Eval("MA_DANH_HIEU_CN") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- End Danh hiệu--%>
                        <%-- Lên lớp --%>
                        <telerik:GridTemplateColumn HeaderText="IS_LEN_LOP" UniqueName="IS_LEN_LOP" DataField="IS_LEN_LOP" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <span>Lên lớp</span><br />
                                <asp:CheckBox runat="server" ID="chbLenLopAll" onclick="CheckBoxHeaderClick(this,'chbIS_LEN_LOP')" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chbLenLop" CssClass="chbIS_LEN_LOP" />
                                <asp:HiddenField ID="hdIS_LEN_LOP" runat="server" Value='<%#Bind("IS_LEN_LOP") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End lên lớp--%>
                        <%-- Tốt nghiệp --%>
                        <telerik:GridTemplateColumn HeaderText="IS_TOT_NGHIEP" UniqueName="IS_TOT_NGHIEP" DataField="IS_TOT_NGHIEP" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <span>Tốt nghiệp</span><br />
                                <asp:CheckBox runat="server" ID="chbTotNghiepAll" onclick="CheckBoxHeaderClick(this,'chbIS_TOT_NGHIEP')" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chbTotNghiep" CssClass="chbIS_TOT_NGHIEP" />
                                <asp:HiddenField ID="hdIS_TOT_NGHIEP" runat="server" Value='<%#Bind("IS_TOT_NGHIEP") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End tốt nghiệp--%>
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
