<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="BaiTapVeNhaDetail.aspx.cs" Inherits="CMS.QuanLySach.BaiTapVeNhaDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rcbKhoiHoc">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbLop" />
                    <telerik:AjaxUpdatedControl ControlID="rcbMonHoc" />
                    <telerik:AjaxUpdatedControl ControlID="rcbSach" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbLop">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbMonHoc" />
                    <telerik:AjaxUpdatedControl ControlID="rcbSach" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbMonHoc">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbSach" />
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
            <div class="col-sm-4">
                <span class="item-title">CHI TIẾT giao bài tập</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btAdd" runat="server" CssClass="btn bt-one" Text="Thêm" OnClick="btAdd_Click" />
                <asp:Button ID="btEdit" runat="server" CssClass="btn bt-one" Text="Sửa" OnClick="btEdit_Click" />
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
                                    <label class="col-sm-5 control-label">Khối <span style="color: red">(*)</span></label>
                                    <div class="col-sm-7">
                                        <telerik:RadComboBox ID="rcbKhoiHoc" runat="server" Width="100%" DataSourceID="objKhoi"
                                            DataTextField="TEN" DataValueField="MA" AutoPostBack="True"
                                            OnSelectedIndexChanged="rcbKhoiHoc_SelectedIndexChanged" Filter="Contains">
                                        </telerik:RadComboBox>
                                        <asp:ObjectDataSource ID="objKhoi" runat="server" SelectMethod="getKhoiByCapHoc" TypeName="OneEduDataAccess.BO.KhoiBO">
                                            <SelectParameters>
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label for="rcbLop" class="col-sm-5 control-label">Lớp <span style="color: red">(*)</span></label>
                                    <div class="col-sm-7">
                                        <telerik:RadComboBox Width="100%" ID="rcbLop" runat="server" DataSourceID="objLop" DataTextField="TEN" DataValueField="ID" Filter="Contains" OnSelectedIndexChanged="rcbLop_SelectedIndexChanged" AutoPostBack="true">
                                        </telerik:RadComboBox>
                                        <asp:ObjectDataSource ID="objLop" runat="server" SelectMethod="getLopByKhoiNamHoc" TypeName="OneEduDataAccess.BO.LopBO">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="rcbKhoiHoc" Name="maKhoi" PropertyName="SelectedValue" />
                                                <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                                                <asp:Parameter DefaultValue="" Name="id_all" Type="Int64" />
                                                <asp:Parameter DefaultValue="Tất cả" Name="text_all" Type="String" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label class="col-sm-5 control-label">Ngày giao bài <span style="color: red">(*)</span></label>
                                    <div class="col-sm-7">
                                        <telerik:RadDatePicker Culture="vi-VN" RenderMode="Classic" ID="rdNgay" runat="server" Width="100%" MinDate="1900/1/1"
                                            Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="Giá trị không hợp lệ"
                                            Calendar-FastNavigationSettings-TodayButtonCaption="Hôm nay"
                                            DatePopupButton-ToolTip="Ngày bắt đầu"
                                            Calendar-FastNavigationSettings-OkButtonCaption="Chọn"
                                            Calendar-FastNavigationSettings-CancelButtonCaption="Hủy">
                                            <DateInput DateFormat="dd/MM/yyyy"></DateInput>
                                        </telerik:RadDatePicker>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-10">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Nội dung <span style="color: red">(*)</span></label>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="tbNoiDung" CssClass="form-control" ClientIDMode="Static" placeholder="Nội dung" runat="server" MaxLength="500" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="Label1" runat="server" Text="(Nhập nội dung bài tập giáo viên giao về nhà)"></asp:Label></div>
                        </div>
                    </fieldset>
                </div>
                <div class="row">
                    <fieldset class="collapsible">
                        <legend style="cursor: pointer">Thêm chi tiết (Click để ẩn hiện thông tin)</legend>
                        <div class="content">
                            <div class="row">
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Môn học <span style="color: red">(*)</span></label>
                                        <div class="col-sm-7">
                                            <telerik:RadComboBox ID="rcbMonHoc" runat="server" Width="100%" DataSourceID="objMonHoc"
                                                DataTextField="TEN" DataValueField="ID" AutoPostBack="True" Filter="Contains" OnSelectedIndexChanged="rcbMonHoc_SelectedIndexChanged">
                                            </telerik:RadComboBox>
                                            <asp:ObjectDataSource ID="objMonHoc" runat="server" SelectMethod="getMonHocByKhoiCapHoc" TypeName="OneEduDataAccess.BO.MonHocBO">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="rcbKhoiHoc" Name="idKhoi" PropertyName="SelectedValue" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Tên sách <span style="color: red">(*)</span></label>
                                        <div class="col-sm-7">
                                            <telerik:RadComboBox ID="rcbSach" runat="server" Width="100%" DataSourceID="objSach"
                                                DataTextField="TEN" DataValueField="ID" Filter="Contains">
                                            </telerik:RadComboBox>
                                            <asp:ObjectDataSource ID="objSach" runat="server" SelectMethod="getListSachByKhoiMonHoc" TypeName="OneEduDataAccess.BO.DMSachBO">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="rcbKhoiHoc" Name="id_Khoi" PropertyName="SelectedValue" />
                                                    <asp:ControlParameter ControlID="rcbMonHoc" Name="id_mon_hoc" PropertyName="SelectedValue" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Bài số <span style="color: red">(*)</span></label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="tbBaiSo" CssClass="form-control" ClientIDMode="Static" placeholder="Bài số" runat="server" MaxLength="4" TextMode="Number"></asp:TextBox>
                                            <asp:CustomValidator ID="CustomValidator2" ControlToValidate="tbBaiSo" ClientValidationFunction="validateInt" ValidateEmptyText="false"
                                                runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Nhập số không quá 4 ký tự." />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Trang số <span style="color: red">(*)</span></label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="tbTrangSo" CssClass="form-control" ClientIDMode="Static" placeholder="Trang số" runat="server" MaxLength="4" TextMode="Number"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator1" ControlToValidate="tbTrangSo" ClientValidationFunction="validateInt" ValidateEmptyText="false"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Nhập số không quá 4 ký tự." />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-2">
                                    <asp:Button ID="btThem" runat="server" CssClass="btn bt-infolg" Text="Thêm" OnClick="btThem_Click" />
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
                        <Columns>
                            <telerik:GridClientSelectColumn UniqueName="chkChon" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            </telerik:GridClientSelectColumn>
                            <telerik:GridTemplateColumn HeaderText="STT" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# Container.DataSetIndex+1 %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="ID_MON_HOC" FilterControlAltText="Filter ID_MON_HOC column" HeaderText="ID_MON_HOC" SortExpression="ID_MON_HOC" UniqueName="ID_MON_HOC" Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ID_SACH" FilterControlAltText="Filter ID_SACH column" HeaderText="ID_SACH" SortExpression="ID_SACH" UniqueName="ID_SACH" Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="BAI_SO" FilterControlAltText="Filter BAI_SO column" HeaderText="BAI_SO" SortExpression="BAI_SO" UniqueName="BAI_SO" Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="TRANG_SO" FilterControlAltText="Filter TRANG_SO column" HeaderText="TRANG_SO" SortExpression="TRANG_SO" UniqueName="TRANG_SO" Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="TEN_SACH" FilterControlAltText="Filter TEN_SACH column" HeaderText="Tên sách" SortExpression="TEN_SACH" UniqueName="TEN_SACH">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="TEN_BAI" FilterControlAltText="Filter TEN_BAI column" HeaderText="Tên bài" SortExpression="TEN_BAI" UniqueName="TEN_BAI">
                            </telerik:GridBoundColumn>
                        </Columns>
                        <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </div>
    </div>
</asp:Content>
