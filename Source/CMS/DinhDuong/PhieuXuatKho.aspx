<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="PhieuXuatKho.aspx.cs" Inherits="CMS.DinhDuong.PhieuXuatKho" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rcbNhomThucPham">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbThucPham" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="lblDonViTinh" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="hdSoLuongCon" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="lbSoLuongCon" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbThucPham">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblDonViTinh" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="hdSoLuongCon" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="lbSoLuongCon" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnThemThucPham">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btDeleteChon">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btEdit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <telerik:RadCodeBlock runat="server">
        <style>
            fieldset {
                border: 1px solid #ccc !important;
                padding: 0 1.4em 1.4em 1.4em !important;
                margin: 0 0 1.5em 0 !important;
                -webkit-box-shadow: 0px 0px 0px 0px #000;
                box-shadow: 0px 0px 0px 0px #000;
            }

                fieldset .content {
                    /*display: none;*/
                }

            legend {
                font-size: 1.2em !important;
                font-weight: bold !important;
                text-align: left !important;
                width: auto;
                padding: 0 10px;
                border-bottom: none;
            }
        </style>
        <script>
            var grid;
            function pageLoad() {
                grid = $find("<%=RadGrid1.ClientID%>");
            }
            function refreshGrid() {
                var masterTable = grid.get_masterTableView();
                masterTable.rebind();
            }
            function btDeteleClick() {
                if (grid.get_masterTableView().get_selectedItems().length > 0) {
                    if (confirm("Bạn chắc chắn muốn xóa?")) {
                        return true;
                    }
                } else {
                    alert("Mời bạn chọn thông tin xóa.");
                    return false;
                }
            }
            $(function () {
                $('legend').click(function () {
                    $(this).parent().find('.content').slideToggle("slow");
                });
            });
        </script>
    </telerik:RadCodeBlock>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-6">
                <span class="item-title">CHI TIẾT PHIẾU XUẤT KHO</span>
            </div>
            <div class="col-sm-6 text-right">
                <asp:Button ID="btAdd" runat="server" CssClass="btn bt-one" Text="Thêm" OnClick="btAdd_Click" />
                <asp:Button ID="btnAddContinue" runat="server" CssClass="btn bt-one" Text="Thêm tiếp" OnClick="btnAddContinue_Click" />
                <asp:Button ID="btEdit" runat="server" CssClass="btn bt-one" Text="Sửa" OnClick="btEdit_Click" Visible="false" />
                <asp:Button ID="btDeleteChon" runat="server" CssClass="btn bt-one" Text="Xóa chi tiết" OnClientClick="if(!btDeteleClick()) return false;" OnClick="btDeleteChon_Click" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="form-horizontal" role="form">
                <div class="row">
                    <fieldset>
                        <legend>Thông tin chung:</legend>
                        <div class="row">
                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label for="tbTen" class="col-sm-6 control-label">Mã phiếu</label>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="tbMaPhieu" CssClass="form-control" ClientIDMode="Static" placeholder="Mã phiếu" runat="server" MaxLength="50" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label for="rcbNguoiXuatHang" class="col-sm-6 control-label">Người xuất hàng <span style="color: red">(*)</span></label>
                                    <div class="col-sm-6">
                                        <telerik:RadComboBox ID="rcbNguoiXuatHang" runat="server" Width="100%" DataSourceID="objNhanVien" EmptyMessage="Chọn nhân viên" DataTextField="HO_TEN" DataValueField="ID" AllowCustomText="true" Filter="Contains">
                                        </telerik:RadComboBox>
                                        <asp:ObjectDataSource ID="objNhanVien" runat="server" SelectMethod="getNhanVienByTruong" TypeName="OneEduDataAccess.BO.GiaoVienBO">
                                            <SelectParameters>
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label for="rdNgay" class="col-sm-5 control-label">Ngày xuất kho</label>
                                    <div class="col-sm-7">
                                        <telerik:RadDateTimePicker RenderMode="Classic" ID="rdNgay" Width="100%" runat="server" MinDate="1999/1/1">
                                        </telerik:RadDateTimePicker>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Ghi chú</label>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="tbGhiChu" CssClass="form-control" ClientIDMode="Static" placeholder="Ghi chú" runat="server" Width="100%"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                </div>
                <div class="row">
                    <fieldset class="collapsible">
                        <legend style="cursor: pointer">Thêm thực phẩm (Click để ẩn hiện thông tin)</legend>
                        <div class="content">
                            <div class="row">
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Nhóm thực phẩm</label>
                                        <div class="col-sm-7">
                                            <telerik:RadComboBox ID="rcbNhomThucPham" runat="server" Width="100%" DataSourceID="objNhomThucPham"
                                                DataTextField="TEN" DataValueField="ID" Filter="Contains" OnSelectedIndexChanged="rcbNhomThucPham_SelectedIndexChanged" AutoPostBack="true">
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
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Thực phẩm</label>
                                        <div class="col-sm-7">
                                            <telerik:RadComboBox ID="rcbThucPham" runat="server" Width="100%" DataSourceID="objThucPham"
                                                DataTextField="TEN_THUC_PHAM" DataValueField="ID_THUC_PHAM" Filter="Contains" OnSelectedIndexChanged="rcbThucPham_SelectedIndexChanged" AutoPostBack="true">
                                            </telerik:RadComboBox>
                                            <asp:ObjectDataSource ID="objThucPham" runat="server" SelectMethod="getThucPhamTrongKhoByNhom" TypeName="OneEduDataAccess.BO.KhoThucPhamBO">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="rcbNhomThucPham" Name="id_nhom" PropertyName="SelectedValue" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <asp:Label ID="lbSoLuongCon" runat="server" Text="" Style="margin-left: -20px; line-height: 45px;"></asp:Label>
                                    <asp:HiddenField ID="hdSoLuongCon" runat="server" Value="0" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Số lượng</label>
                                        <div class="col-sm-5">
                                            <telerik:RadNumericTextBox ID="tbSoLuong" CssClass="form-control" ClientIDMode="Static" placeholder="Số lượng" runat="server" MinValue="0" MaxValue="1000000000" MaxLength="10" Width="100%"></telerik:RadNumericTextBox>
                                        </div>
                                        <div class="col-sm-2" style="padding-top: 8px;">
                                            <asp:Label ID="lblDonViTinh" runat="server" Text="" Style="margin-left: -20px;"></asp:Label>
                                            <asp:HiddenField ID="hdDonViTinh" runat="server" Value="0" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <div class="col-sm-5">
                                            <div class="one-checkbox" style="text-align: right;">
                                                <label>
                                                    <asp:CheckBox ID="cbIS_BO" runat="server" Text="Xuất bỏ" />
                                                </label>
                                            </div>
                                        </div>
                                        <div class="col-sm-3" runat="server">
                                            <asp:Button ID="btnThemThucPham" runat="server" CssClass="btn bt-infolg" Text="Thêm" OnClick="btnThemThucPham_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                </div>
            </div>
            <div class="item-data">
                <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" GroupPanelPosition="Top" OnNeedDataSource="RadGrid1_NeedDataSource" OnItemDataBound="RadGrid1_ItemDataBound"
                    AutoGenerateColumns="False" PageSize="100" AllowMultiRowSelection="True">
                    <ClientSettings>
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <MasterTableView>
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
                            <telerik:GridColumnGroup HeaderText="Thành phần dinh dưỡng (Kcal)" Name="TPDD" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        </ColumnGroups>
                        <Columns>
                            <telerik:GridClientSelectColumn UniqueName="chkChon" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            </telerik:GridClientSelectColumn>
                            <telerik:GridTemplateColumn HeaderText="STT" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# Container.DataSetIndex+1 %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="ID_NHOM_THUC_PHAM" FilterControlAltText="Filter ID_NHOM_THUC_PHAM column" HeaderText="Tên nhóm" SortExpression="ID_NHOM_THUC_PHAM" UniqueName="ID_NHOM_THUC_PHAM" HeaderStyle-HorizontalAlign="Center" Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ID_THUC_PHAM" FilterControlAltText="Filter ID_THUC_PHAM column" HeaderText="Tên nhóm" SortExpression="ID_THUC_PHAM" UniqueName="ID_THUC_PHAM" HeaderStyle-HorizontalAlign="Center" Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="TEN_THUC_PHAM" FilterControlAltText="Filter TEN_THUC_PHAM column" HeaderText="Tên thực phẩm" SortExpression="TEN_THUC_PHAM" UniqueName="TEN_THUC_PHAM" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="250px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="TEN_NHOM_THUC_PHAM" FilterControlAltText="Filter TEN_NHOM_THUC_PHAM column" HeaderText="Tên nhóm thực phẩm" SortExpression="TEN_NHOM_THUC_PHAM" UniqueName="TEN_NHOM_THUC_PHAM" HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="DON_VI_TINH" FilterControlAltText="Filter DON_VI_TINH column" HeaderText="Đơn vị tính" SortExpression="DON_VI_TINH" UniqueName="DON_VI_TINH" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="SO_LUONG" FilterControlAltText="Filter SO_LUONG column" HeaderText="Số lượng" SortExpression="SO_LUONG" UniqueName="SO_LUONG" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="200px">
                            </telerik:GridBoundColumn>
                            <telerik:GridCheckBoxColumn DataField="IS_BO" FilterControlAltText="Filter IS_BO column" HeaderText="Xuất bỏ" SortExpression="IS_BO" UniqueName="IS_BO" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                            </telerik:GridCheckBoxColumn>
                        </Columns>
                        <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </div>
    </div>
</asp:Content>
