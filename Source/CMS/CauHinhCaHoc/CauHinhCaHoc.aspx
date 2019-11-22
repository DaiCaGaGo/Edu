<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="CauHinhCaHoc.aspx.cs" Inherits="CMS.CauHinhCaHoc.CauHinhCaHoc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
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
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true" OnClientClose="RadWindowManager1_OnClientClose">
        <Windows>
            <telerik:RadWindow RenderMode="Lightweight" ID="RadWindow1" runat="server" ShowContentDuringLoad="false" Width="400px"
                Height="400px" Title="Chi tiết khối" Behaviors="Reload,Close,Maximize" Modal="true" VisibleStatusbar="false">
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
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">Cấu Hình Ca Học</span>
            </div>
            <div class="col-sm-8 text-right">
                <input type="button" id="btAddNew" runat="server" class="btn bt-one" value="Thêm mới" onclick="openRadWin('CauHinhCaHocDetail.aspx', 990, 470, 1)" />
                <asp:Button ID="btExport" runat="server" CssClass="btn bt-one" OnClick="btExport_Click" Text="Xuất excel" />
                <asp:Button ID="btDeleteChon" runat="server" CssClass="btn bt-one" OnClick="btDeleteChon_Click" Text="Xóa" OnClientClick="if(!btDeteleClick()) return false;" />
            </div>
            <div style="clear: both"></div>
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
                        <telerik:GridBoundColumn DataField="NGAY_BAT_DAU" FilterControlAltText="Filter NGAY_BAT_DAU column" HeaderText="Ngày bắt đầu" SortExpression="NGAY_BAT_DAU" UniqueName="NGAY_BAT_DAU" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NGAY_KET_THUC" FilterControlAltText="Filter NGAY_KET_THUC column" HeaderText="Ngày kết thúc" SortExpression="NGAY_KET_THUC" UniqueName="NGAY_KET_THUC" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="THOI_GIAN_TIET1" FilterControlAltText="Filter THOI_GIAN_TIET1 column" HeaderText="Tiết 1" SortExpression="THOI_GIAN_TIET1" UniqueName="THOI_GIAN_TIET1" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="THOI_GIAN_TIET2" FilterControlAltText="Filter THOI_GIAN_TIET2 column" HeaderText="Tiết 2" SortExpression="THOI_GIAN_TIET2" UniqueName="THOI_GIAN_TIET2" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="THOI_GIAN_TIET3" FilterControlAltText="Filter THOI_GIAN_TIET3 column" HeaderText="Tiết 3" SortExpression="THOI_GIAN_TIET3" UniqueName="THOI_GIAN_TIET3" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="THOI_GIAN_TIET4" FilterControlAltText="Filter THOI_GIAN_TIET4 column" HeaderText="Tiết 4" SortExpression="THOI_GIAN_TIET4" UniqueName="THOI_GIAN_TIET4" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="THOI_GIAN_TIET5" FilterControlAltText="Filter THOI_GIAN_TIET5 column" HeaderText="Tiết 5" SortExpression="THOI_GIAN_TIET5" UniqueName="THOI_GIAN_TIET5" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="THOI_GIAN_TIET6" FilterControlAltText="Filter THOI_GIAN_TIET6 column" HeaderText="Tiết 6" SortExpression="THOI_GIAN_TIET6" UniqueName="THOI_GIAN_TIET6" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="THOI_GIAN_TIET7" FilterControlAltText="Filter THOI_GIAN_TIET7 column" HeaderText="Tiết 7" SortExpression="THOI_GIAN_TIET7" UniqueName="THOI_GIAN_TIET7" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="THOI_GIAN_TIET8" FilterControlAltText="Filter THOI_GIAN_TIET8 column" HeaderText="Tiết 8" SortExpression="THOI_GIAN_TIET8" UniqueName="THOI_GIAN_TIET8" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="THOI_GIAN_TIET9" FilterControlAltText="Filter THOI_GIAN_TIET9 column" HeaderText="Tiết 9" SortExpression="THOI_GIAN_TIET9" UniqueName="THOI_GIAN_TIET9" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="THOI_GIAN_TIET10" FilterControlAltText="Filter THOI_GIAN_TIET10 column" HeaderText="Tiết 10" SortExpression="THOI_GIAN_TIET10" UniqueName="THOI_GIAN_TIET10" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="THOI_GIAN_TIET11" FilterControlAltText="Filter THOI_GIAN_TIET11 column" HeaderText="Tiết 11" SortExpression="THOI_GIAN_TIET11" UniqueName="THOI_GIAN_TIET11" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="THOI_GIAN_TIET12" FilterControlAltText="Filter THOI_GIAN_TIET12 column" HeaderText="Tiết 12" SortExpression="THOI_GIAN_TIET12" UniqueName="THOI_GIAN_TIET12" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="THOI_GIAN_TIET13" FilterControlAltText="Filter THOI_GIAN_TIET13 column" HeaderText="Tiết 13" SortExpression="THOI_GIAN_TIET13" UniqueName="THOI_GIAN_TIET13" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="THOI_GIAN_TIET14" FilterControlAltText="Filter THOI_GIAN_TIET14 column" HeaderText="Tiết 14" SortExpression="THOI_GIAN_TIET14" UniqueName="THOI_GIAN_TIET14" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="THOI_GIAN_TIET15" FilterControlAltText="Filter THOI_GIAN_TIET15 column" HeaderText="Tiết 15" SortExpression="THOI_GIAN_TIET15" UniqueName="THOI_GIAN_TIET15" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MUA" FilterControlAltText="Filter MUA column" HeaderText="Cấu hình" SortExpression="MUA" ItemStyle-Width="140px" UniqueName="MUA" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Sửa" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="SUA">
                            <ItemTemplate>
                                <img src="../img/sua.png" onclick='<%# "openRadWin(\"CauHinhCaHocDetail.aspx?id_hoso=" + Eval("ID") + "\",990, 470, 1)" %>' style="cursor: pointer;" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
