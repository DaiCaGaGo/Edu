<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="CustomerInTo.aspx.cs" Inherits="CMS.Customer.CustomerInTo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rcbTo">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbCustomer"/>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbCustomer" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbCustomer" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btDeleteChon">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="rcbCustomer" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
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
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">QUẢN LÝ CUSTOMER THEO TỔ</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btSave" runat="server" CssClass="btn bt-one" OnClick="btSave_Click" Text="Lưu" />
                <asp:Button ID="btTaiExcel" runat="server" OnClick="btTaiExcel_Click" Text="Tải file mẫu" CssClass="btn bt-one" />
                <asp:Button ID="btDeleteChon" runat="server" CssClass="btn bt-one" OnClick="btDeleteChon_Click" Text="Xóa" OnClientClick="if(!btDeteleClick()) return false;" />
                <asp:Button ID="btExport" runat="server" CssClass="btn bt-one" OnClick="btExport_Click" Text="Xuất excel" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="form-horizontal" role="form">
                <div class="row">
                    <div class="col-sm-3">
                        <telerik:RadComboBox ID="rcbTo" runat="server" Width="100%" DataSourceID="objTo"
                            DataTextField="TEN" DataValueField="ID" AutoPostBack="true" OnSelectedIndexChanged="rcbTo_SelectedIndexChanged" Filter="Contains" Enabled="true">
                        </telerik:RadComboBox>
                        <asp:ObjectDataSource ID="objTo" runat="server" SelectMethod="getToCustomer" TypeName="OneEduDataAccess.BO.CustomerToBO">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="" Name="ten" Type="String" />
                                <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                                <asp:Parameter DefaultValue="0" Name="id_all" Type="Int16" />
                                <asp:Parameter Name="text_all" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </div>
                    <div class="col-sm-3">
                        <telerik:RadComboBox ID="rcbCustomer" runat="server" Width="100%" DataSourceID="objCustomer"
                            DataTextField="HO_TEN" DataValueField="ID" EmptyMessage="Chọn customer" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" AllowCustomText="true" Filter="Contains" Localization-CheckAllString="Chọn tất cả">
                        </telerik:RadComboBox>
                        <asp:ObjectDataSource ID="objCustomer" runat="server" SelectMethod="getCustomerNotExistsTo" TypeName="OneEduDataAccess.BO.CustomerBO">
                            <SelectParameters>
                                <asp:ControlParameter Name="id_to" PropertyName="SelectedValue" ControlID="rcbTo" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </div>
                    <div class="col-sm-6">
                        <table>
                            <tr>
                                <td>
                                    <asp:FileUpload ID="FileExcel" AllowMultiple="true" runat="server" CssClass="btn" accept=".xls,.xlsx" />
                                </td>
                                <td>
                                    <asp:Button ID="btnUpload" CssClass="btn bt-infolg" runat="server" OnClick="btnUpload_Click" Text="Tải lên" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <div class="item-data">
            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" GroupPanelPosition="Top" OnNeedDataSource="RadGrid1_NeedDataSource"
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
                        <telerik:GridBoundColumn DataField="HO_TEN" FilterControlAltText="Filter TEN column" HeaderText="Tên giáo viên" SortExpression="HO_TEN" UniqueName="HO_TEN" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SDT" HeaderStyle-Width="200px" FilterControlAltText="Filter SDT column" HeaderText="Số điện thoại" SortExpression="SDT" UniqueName="SDT" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                    </Columns>
                    <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
