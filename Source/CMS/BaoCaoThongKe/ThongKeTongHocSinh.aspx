<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterSYS.Master" AutoEventWireup="true" CodeBehind="ThongKeTongHocSinh.aspx.cs" Inherits="CMS.BaoCaoThongKe.ThongKeTongHocSinh" %>
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
            <telerik:AjaxSetting AjaxControlID="rcbTruong">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">THỐNG KÊ SỐ LƯỢNG HỌC SINH THEO TRƯỜNG</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btExport" runat="server" CssClass="btn bt-one" OnClick="btExport_Click" Text="Xuất excel" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="row">
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbTruong" runat="server" Width="100%" DataTextField="TEN" DataValueField="ID" Filter="Contains" OnSelectedIndexChanged="rcbTruong_SelectedIndexChanged" AutoPostBack="true" EmptyMessage="Chọn trường" AllowCustomText="true">
                    </telerik:RadComboBox>
                </div>
            </div>
        </div>
        <div class="item-data">
            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" GroupPanelPosition="Top" OnNeedDataSource="RadGrid1_NeedDataSource" OnItemDataBound="RadGrid1_ItemDataBound"
                AutoGenerateColumns="False" PageSize="100" AllowMultiRowSelection="True">
                <ClientSettings>
                    <Selecting AllowRowSelect="true"></Selecting>
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
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
                        <telerik:GridTemplateColumn HeaderText="STT" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%# Container.DataSetIndex+1 %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="ID_TRUONG" FilterControlAltText="Filter ID_TRUONG column" HeaderText="ID_TRUONG" SortExpression="ID_TRUONG" UniqueName="ID_TRUONG" HeaderStyle-HorizontalAlign="Center" Visible="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TEN_TRUONG" FilterControlAltText="Filter TEN_TRUONG column" HeaderText="Tên trường" SortExpression="TEN_TRUONG" UniqueName="TEN_TRUONG" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <%--<telerik:GridBoundColumn DataField="TEN_KHOI" FilterControlAltText="Filter TEN_KHOI column" HeaderText="Khối" SortExpression="TEN_KHOI" UniqueName="TEN_KHOI" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TEN_LOP" FilterControlAltText="Filter TEN_LOP column" HeaderText="Lớp" SortExpression="TEN_LOP" UniqueName="TEN_LOP" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="300px">
                        </telerik:GridBoundColumn>--%>
                        <telerik:GridBoundColumn DataField="SO_HOC_SINH" FilterControlAltText="Filter SO_HOC_SINH column" HeaderText="Số học sinh" SortExpression="SO_HOC_SINH" UniqueName="SO_HOC_SINH" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                    </Columns>
                    <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>