<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="ThemThucPhamThucDon.aspx.cs" Inherits="CMS.DinhDuong.ThemThucPhamThucDon" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbNhomThucPham">
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
            grid = $find("<%= RadGrid1.ClientID %>");
            function pageload() {
                grid = $find("<%= RadGrid1.ClientID %>");
            }
            function refreshGrid() {
                var masterTable = grid.get_masterTableView();
                masterTable.rebind();
            }
            function changeSoLuong() {
                grid = $find("<%=RadGrid1.ClientID%>");

                for (var i = 0; i < grid.get_masterTableView().get_dataItems().length; i++) {
                    var item = grid.get_masterTableView().get_dataItems()[i];
                    var soLuong = $(item.get_cell("SO_LUONG")).find('.so-luong').val();

                    var nangLuong_new = $(item.get_cell("NANG_LUONG_KCAL")).find('.nang-luong');
                    var protid_new = $(item.get_cell("PROTID")).find('.protid');
                    var glucid_new = $(item.get_cell("GLUCID")).find('.glucid');
                    var lipid_new = $(item.get_cell("LIPID")).find('.lipid');

                    var nangLuong_old = $(item.get_cell("NANG_LUONG_KCAL_OLD")).find('.nang-luong-old').val();
                    var protid_old = $(item.get_cell("PROTID_OLD")).find('.protid-old').val();
                    var glucid_old = $(item.get_cell("GLUCID_OLD")).find('.glucid-old').val();
                    var lipid_old = $(item.get_cell("LIPID_OLD")).find('.lipid-old').val();

                    var tong_nang_luong = 0;
                    var tong_protid = 0, tong_glucid = 0, tong_lipid = 0;
                    tong_nang_luong = soLuong * nangLuong_old;
                    tong_protid = soLuong * protid_old;
                    tong_glucid = soLuong * glucid_old;
                    tong_lipid = soLuong * lipid_old;

                    if (tong_nang_luong > 0) $(nangLuong_new).val(tong_nang_luong); else $(nangLuong_new).val('');
                    if (tong_protid > 0) $(protid_new).val(tong_protid); else $(protid_new).val('');
                    if (tong_glucid > 0) $(glucid_new).val(tong_glucid); else $(glucid_new).val('');
                    if (tong_lipid > 0) $(lipid_new).val(tong_lipid); else $(lipid_new).val('');
                }
            }
            function CheckBoxHeaderClick(cb, cssLst) {
                $('.' + cssLst + ' input').each(function () {
                    if ($(this).attr('disabled') != "disabled") {
                        $(this).prop('checked', cb.checked);
                    }
                });
            }
        </script>
    </telerik:RadCodeBlock>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">danh mục thực phẩm</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btTimKiem" runat="server" Text="Tìm kiếm" CssClass="btn bt-one" OnClick="btTimKiem_Click"/>
                <asp:Button ID="btnChon" runat="server" CssClass="btn bt-one" OnClick="btnChon_Click" Text="Cập nhật" OnClientClick="if (confirm('Bạn có chắc chắn muốn lưu?')) return true; else return false;" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="row">
                <div class="col-sm-4">
                    <telerik:RadComboBox ID="rcbNhomThucPham" runat="server" Width="100%" DataSourceID="objNhomThucPham"
                        DataTextField="TEN" DataValueField="ID" EmptyMessage="Chọn nhóm thực phẩm" AllowCustomText="true" Filter="Contains" OnSelectedIndexChanged="rcbNhomThucPham_SelectedIndexChanged" AutoPostBack="true">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objNhomThucPham" runat="server" SelectMethod="getNhomThucPham" TypeName="OneEduDataAccess.BO.NhomThucPhamBO">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="" Name="ten" Type="String" />
                            <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                            <asp:Parameter DefaultValue="" Name="id_all" Type="Int16" />
                            <asp:Parameter Name="text_all" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
                <div class="col-sm-8">
                    <telerik:RadTextBox ID="tbTen" runat="server" EmptyMessage="Nhập tên thực phẩm cần tìm kiếm" Width="100%"></telerik:RadTextBox>
                </div>
            </div>
        </div>
        <div class="item-data">
            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" GroupPanelPosition="Top" OnNeedDataSource="RadGrid1_NeedDataSource" OnItemDataBound="RadGrid1_ItemDataBound"
                AutoGenerateColumns="False" PageSize="100" AllowMultiRowSelection="True">
                <ClientSettings>
                    <Selecting AllowRowSelect="true"></Selecting>
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
                        <telerik:GridColumnGroup HeaderText="Thành phần dinh dưỡng (Kcal)" HeaderStyle-HorizontalAlign="Center" Name="TPDD"></telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Chọn" UniqueName="IS_CHON" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <span>Chọn</span><br />
                                <asp:CheckBox runat="server" ID="ckhAllIS_CHON" onclick="CheckBoxHeaderClick(this,'chb_IS_CHON')" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chbIS_CHON" CssClass="chb_IS_CHON" />
                                <asp:HiddenField ID="hdIS_CHON" runat="server" Value='<%#Bind("IS_CHON") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="STT" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%# Container.DataSetIndex+1 %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="ID_NHOM_THUC_PHAM" FilterControlAltText="Filter ID_NHOM_THUC_PHAM column" HeaderText="Tên nhóm" SortExpression="ID_NHOM_THUC_PHAM" UniqueName="ID_NHOM_THUC_PHAM" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ID_THUC_PHAM" FilterControlAltText="Filter ID_THUC_PHAM column" HeaderText="ID_THUC_PHAM" SortExpression="ID_THUC_PHAM" UniqueName="ID_THUC_PHAM" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ID_KHOI" FilterControlAltText="Filter ID_KHOI column" HeaderText="Tên nhóm" SortExpression="ID_KHOI" UniqueName="ID_KHOI" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TEN_NHOM" FilterControlAltText="Filter TEN_NHOM column" HeaderText="Tên nhóm thực phẩm" SortExpression="TEN_NHOM" UniqueName="TEN_NHOM" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="400px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TEN" FilterControlAltText="Filter TEN column" HeaderText="Tên thực phẩm" SortExpression="TEN" UniqueName="TEN" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DON_VI_TINH" FilterControlAltText="Filter DON_VI_TINH column" HeaderText="Đơn vị" SortExpression="DON_VI_TINH" UniqueName="DON_VI_TINH" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DON_VI" FilterControlAltText="Filter DON_VI column" HeaderText="Đơn vị" SortExpression="DON_VI" UniqueName="DON_VI" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Năng lượng" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NANG_LUONG_KCAL_OLD" Display="false">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNANG_LUONG_KCAL_OLD" runat="server" Text='<%# Eval("NANG_LUONG_KCAL") %>' CssClass="form-control text-box nang-luong-old" Enabled="false"></asp:TextBox>
                                <asp:HiddenField ID="hdNANG_LUONG_KCAL_NEW" runat="server" Value="" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Protid" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="PROTID_OLD" Display="false">
                            <ItemTemplate>
                                <asp:TextBox ID="tbPROTID_OLD" runat="server" Text='<%# Eval("PROTID") %>' CssClass="form-control text-box protid-old" Enabled="false"></asp:TextBox>
                                <asp:HiddenField ID="hdPROTID_NEW" runat="server" Value="" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Glucid" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="GLUCID_OLD" Display="false">
                            <ItemTemplate>
                                <asp:TextBox ID="tbGLUCID_OLD" runat="server" Text='<%# Eval("GLUCID") %>' CssClass="form-control text-box glucid-old" Enabled="false"></asp:TextBox>
                                <asp:HiddenField ID="hdGLUCID_NEW" runat="server" Value="" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Lipid" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="LIPID_OLD" Display="false">
                            <ItemTemplate>
                                <asp:TextBox ID="tbLIPID_OLD" runat="server" Text='<%# Eval("LIPID") %>' CssClass="form-control text-box lipid-old" Enabled="false"></asp:TextBox>
                                <asp:HiddenField ID="hdLIPID_NEW" runat="server" Value="" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Số lượng" HeaderStyle-Width="100px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="SO_LUONG">
                            <ItemTemplate>
                                <asp:TextBox ID="tbSO_LUONG" runat="server" Text='<%# Eval("SO_LUONG") %>' CssClass="form-control text-box so-luong" onchange="changeSoLuong()" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdSO_LUONG" runat="server" Value='<%# Eval("SO_LUONG") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Năng lượng" ColumnGroupName="TPDD" HeaderStyle-Width="100px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="NANG_LUONG_KCAL">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNANG_LUONG_KCAL" ColumnGroupName="TPDD" runat="server" Text='<%# Eval("NANG_LUONG_KCAL_NEW") %>' CssClass="form-control text-box nang-luong" Style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdNANG_LUONG_KCAL" runat="server" Value='<%# Eval("NANG_LUONG_KCAL") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Protid" ColumnGroupName="TPDD" HeaderStyle-Width="100px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="PROTID">
                            <ItemTemplate>
                                <asp:TextBox ID="tbPROTID" runat="server" Text='<%# Eval("PROTID_NEW") %>' CssClass="form-control text-box protid" Style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdPROTID" runat="server" Value='<%# Eval("PROTID") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Glucid" ColumnGroupName="TPDD" HeaderStyle-Width="100px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="GLUCID">
                            <ItemTemplate>
                                <asp:TextBox ID="tbGLUCID" runat="server" Text='<%# Eval("GLUCID_NEW") %>' CssClass="form-control text-box glucid" Style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdGLUCID" runat="server" Value='<%# Eval("GLUCID") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Lipid" ColumnGroupName="TPDD" HeaderStyle-Width="100px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="LIPID">
                            <ItemTemplate>
                                <asp:TextBox ID="tbLIPID" runat="server" Text='<%# Eval("LIPID_NEW") %>' CssClass="form-control text-box lipid" Style="text-align: center"></asp:TextBox>
                                <asp:HiddenField ID="hdLIPID" runat="server" Value='<%# Eval("LIPID") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="NANG_LUONG_KCAL" ColumnGroupName="TPDD" FilterControlAltText="Filter NANG_LUONG_KCAL column" HeaderText="Năng lượng" SortExpression="NANG_LUONG_KCAL_TP" UniqueName="NANG_LUONG_KCAL_TP" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="PROTID" ColumnGroupName="TPDD" FilterControlAltText="Filter PROTID column" HeaderText="Protid" SortExpression="PROTID_TP" UniqueName="PROTID_TP" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="GLUCID" ColumnGroupName="TPDD" FilterControlAltText="Filter GLUCID_TP column" HeaderText="Glucid" SortExpression="GLUCID_TP" UniqueName="GLUCID_TP" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="LIPID" ColumnGroupName="TPDD" FilterControlAltText="Filter LIPID column" HeaderText="Lipid" SortExpression="LIPID_TP" UniqueName="LIPID_TP" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                    </Columns>
                    <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
