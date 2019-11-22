<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="NguoiDungTruong.aspx.cs" Inherits="CMS.QuanTri.NguoiDungTruong" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" EnableAJAX="true" DefaultLoadingPanelID="RadAjaxLoadingPanel1" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rcbUser">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbTruong" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbTruong">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbTruong" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbTruong" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btDeleteChon">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="rcbTruong" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true" OnClientClose="RadWindowManager1_OnClientClose">
        <Windows>
            <telerik:RadWindow RenderMode="Lightweight" ID="RadWindow1" runat="server" ShowContentDuringLoad="false" Width="400px"
                Height="400px" Title="Chi người dùng" Behaviors="Reload,Close,Maximize" Modal="true" VisibleStatusbar="false">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadCodeBlock runat="server">
        <script>
            var grid;
            function pageLoad() {
                grid = $find("<%=RadGrid1.ClientID%>");
            }
            function RadWindowManager1_OnClientClose(sender, args) {
                var ajaxManager = $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("RadWindowManager1_Close");
                var arg = args.get_argument();
                if (arg != null) {
                    refreshGrid();
                    notification('success', arg);
                }
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
                <span class="item-title">QUẢN LÝ NGƯỜI DÙNG THEO TRƯỜNG</span>
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
                        <div class="form-group">
                            <label for="txtUser" class="col-sm-5 control-label">Tên đăng nhập</label>
                            <div class="col-sm-7">
                                <telerik:RadComboBox ID="rcbUser" runat="server" Width="100%" DataSourceID="objUser"
                                    DataTextField="TEN_DANG_NHAP" DataValueField="ID" EmptyMessage="Chọn tài khoản" AutoPostBack="true" OnSelectedIndexChanged="rcbUser_SelectedIndexChanged" AllowCustomText="true" Filter="Contains" Enabled="false">
                                </telerik:RadComboBox>
                                <asp:ObjectDataSource ID="objUser" runat="server" SelectMethod="getAllNguoiDung" TypeName="OneEduDataAccess.BO.NguoiDungBO">
                                    <SelectParameters>
                                        <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                                        <asp:Parameter DefaultValue="" Name="id_all" Type="Int16" />
                                        <asp:Parameter Name="text_all" Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4 col-sm-offset-1">
                        <div class="form-group">
                            <label class="col-sm-5 control-label">Tên trường</label>
                            <div class="col-sm-7">
                                <telerik:RadComboBox ID="rcbTruong" runat="server" Width="100%" DataSourceID="objTruong"
                                    DataTextField="TEN" DataValueField="ID" EmptyMessage="Chọn trường" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" AllowCustomText="true" Filter="Contains" Localization-CheckAllString="Chọn tất cả">
                                </telerik:RadComboBox>
                                <asp:ObjectDataSource ID="objTruong" runat="server" SelectMethod="getTruongNotExistsNguoiDungTruong" TypeName="OneEduDataAccess.BO.TruongBO">
                                    <SelectParameters>
                                        <asp:ControlParameter Name="userID" PropertyName="SelectedValue" ControlID="rcbUser" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <asp:HiddenField runat="server" ID="hdfTruong" Value="0" />
                            </div>
                        </div>
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
                        <telerik:GridBoundColumn DataField="TEN" HeaderStyle-Width="400px" FilterControlAltText="Filter TEN column" HeaderText="Tên trường" SortExpression="TEN" UniqueName="TEN" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Quyền Menu" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="QUYEN_MENU">
                            <ItemTemplate>
                                <img src="../img/sua.png" onclick='<%# "openRadWin(\"NguoiDungMenu.aspx?idTruong=" + Eval("ID") + "&idNguoiDung=" + Request.QueryString.Get("id_hoso") + "\",100,50)" %>' style="cursor: pointer;" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
