<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="DotThuLopDetail.aspx.cs" Inherits="CMS.HocPhi.DotThuLopDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rcbDotThu">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbLop" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbKhoi">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbLop" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbLop">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
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
        </script>
    </telerik:RadCodeBlock>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">QUẢN LÝ ĐỢT THU LỚP</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btSave" runat="server" CssClass="btn bt-one" OnClick="btSave_Click" Text="Lưu" />
                <asp:Button ID="btDeleteChon" runat="server" CssClass="btn bt-one" OnClick="btDeleteChon_Click" Text="Xóa" OnClientClick="if(!btDeteleClick()) return false;" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="form-horizontal" role="form">
                <div class="row">
                    <div class="col-sm-4">
                        <telerik:RadComboBox ID="rcbDotThu" runat="server" Width="100%" DataSourceID="objDotThu"
                            DataTextField="TEN" DataValueField="ID" Filter="Contains" AutoPostBack="true" OnSelectedIndexChanged="rcbDotThu_SelectedIndexChanged">
                        </telerik:RadComboBox>
                        <asp:ObjectDataSource ID="objDotThu" runat="server" SelectMethod="getAllDotThuByTruong" TypeName="OneEduDataAccess.BO.HocPhiDotThuBO">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                                <asp:Parameter DefaultValue="" Name="id_all" Type="Int64" />
                                <asp:Parameter Name="text_all" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </div>
                    <div class="col-sm-4">
                        <telerik:RadComboBox ID="rcbKhoi" runat="server" Width="100%" DataSourceID="objKhoiHoc" DataTextField="TEN" DataValueField="MA" Filter="Contains" AllowCustomText="true" OnSelectedIndexChanged="rcbKhoi_SelectedIndexChanged" EmptyMessage="Chọn khối học" AutoPostBack="true" CheckBoxes="true" CheckedItemsTexts="DisplayAllInInput" EnableCheckAllItemsCheckBox="true" Localization-CheckAllString="Chọn tất cả">
                        </telerik:RadComboBox>
                        <asp:HiddenField ID="hdKhoi" runat="server" />
                        <asp:ObjectDataSource ID="objKhoiHoc" runat="server" SelectMethod="getKhoiByCapHocAndMaLoaiGDTX" TypeName="OneEduDataAccess.BO.KhoiBO">
                            <SelectParameters>
                                <asp:Parameter Name="maLoaiLopGDTX" DefaultValue="" />
                                <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                                <asp:Parameter DefaultValue="" Name="id_all" Type="Int16" />
                                <asp:Parameter DefaultValue="Toàn trường" Name="text_all" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </div>
                    <div class="col-sm-4">
                        <telerik:RadComboBox Width="100%" ID="rcbLop" EmptyMessage="Chọn lớp" runat="server"
                            DataSourceID="objLop" DataTextField="TEN" DataValueField="ID" AllowCustomText="true" Filter="Contains" CheckBoxes="true" CheckedItemsTexts="DisplayAllInInput" EnableCheckAllItemsCheckBox="true" Localization-CheckAllString="Chọn tất cả">
                        </telerik:RadComboBox>
                        <asp:ObjectDataSource ID="objLop" runat="server" SelectMethod="getLopNotExistDotThuByTruongDotThu" TypeName="OneEduDataAccess.BO.LopBO">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="rcbDotThu" Name="id_dot_thu" PropertyName="SelectedValue" />
                                <asp:ControlParameter ControlID="hdKhoi" Name="lstMaKhoi" PropertyName="Value" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </div>
                </div>
            </div>
        </div>
        <div class="item-data">
            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" GroupPanelPosition="Top" OnNeedDataSource="RadGrid1_NeedDataSource" OnItemDataBound="RadGrid1_ItemDataBound"
                AutoGenerateColumns="False" PageSize="50" AllowMultiRowSelection="True">
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
                        <EditColumn UpdateText="Lưu" CancelText="Hủy" InsertText="Lưu" ButtonType="PushButton"></EditColumn>
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
                        <telerik:GridBoundColumn DataField="ID_LOP" FilterControlAltText="Filter ID_LOP column" HeaderText="ID_LOP" SortExpression="ID_LOP" UniqueName="ID_LOP" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TEN_LOP" HeaderStyle-Width="200px" FilterControlAltText="Filter TEN_LOP column" HeaderText="Lớp" SortExpression="TEN_LOP" UniqueName="TEN_LOP" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TONG_TIEN" HeaderStyle-Width="200px" FilterControlAltText="Filter TONG_TIEN column" HeaderText="Tổng tiền (VNĐ)" SortExpression="TONG_TIEN" UniqueName="TEN_LOP" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                    </Columns>
                    <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
