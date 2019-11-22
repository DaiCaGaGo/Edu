<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="DMBuaAn.aspx.cs" Inherits="CMS.DinhDuong.DMBuaAn" %>

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
            <telerik:AjaxSetting AjaxControlID="btTimKiem">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbKhoi">
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
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true" OnClientClose="RadWindowManager1_OnClientClose">
        <Windows>
            <telerik:RadWindow RenderMode="Lightweight" ID="RadWindow1" runat="server" ShowContentDuringLoad="false" Width="400px"
                Height="400px" Title="Chi tiết bữa ăn" Behaviors="Reload,Close,Maximize" Modal="true" VisibleStatusbar="false">
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
                <span class="item-title">Danh sách bữa ăn</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btTimKiem" runat="server" CssClass="btn bt-one" OnClick="btTimKiem_Click" Text="Tìm kiếm" />
                <input type="button" id="btAddNew" runat="server" class="btn bt-one" value="Thêm mới" onclick="openRadWin('DMBuAnDetail.aspx', 100, 50)" />
                <asp:Button ID="btDeleteChon" runat="server" CssClass="btn bt-one" OnClick="btDeleteChon_Click" Text="Xóa bữa ăn" OnClientClick="if(!btDeteleClick()) return false;" />
                <asp:Button ID="btExport" runat="server" CssClass="btn bt-one" OnClick="btExport_Click" Text="Xuất excel" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="row">
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbKhoi" runat="server" Width="100%" DataSourceID="objKhoi"
                        DataTextField="TEN" DataValueField="MA" AutoPostBack="True" EmptyMessage="Chọn khối học"
                        OnSelectedIndexChanged="rcbKhoi_SelectedIndexChanged" AllowCustomText="true" Filter="Contains">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objKhoi" runat="server" SelectMethod="getKhoiByCapHoc" TypeName="OneEduDataAccess.BO.KhoiBO">
                        <SelectParameters>
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
                <div class="col-sm-3">
                    <telerik:RadTextBox ID="txtBuaAn" EmptyMessage="Nhập tên bữa ăn" Width="100%" runat="server"></telerik:RadTextBox>
                </div>
            </div>
        </div>
        <div class="item-data">
            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" GroupPanelPosition="Top" OnNeedDataSource="RadGrid1_NeedDataSource"
                AutoGenerateColumns="False" PageSize="50" AllowMultiRowSelection="True" OnItemDataBound="RadGrid1_ItemDataBound">
                <ClientSettings>
                    <Selecting AllowRowSelect="true"></Selecting>
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>
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
                        <telerik:GridColumnGroup HeaderText="Năng lượng (Kcal)" HeaderStyle-HorizontalAlign="Center" Name="NL"></telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridClientSelectColumn UniqueName="chkChon" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridClientSelectColumn>
                        <telerik:GridTemplateColumn HeaderText="STT" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%# Container.DataSetIndex+1 %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="TEN" FilterControlAltText="Filter TEN column" HeaderText="Tên bữa ăn" SortExpression="TEN" UniqueName="TEN" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ID_KHOI" FilterControlAltText="Filter ID_KHOI column" HeaderText="Thuộc khối học" SortExpression="ID_KHOI" UniqueName="ID_KHOI" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TEN_KHOI" FilterControlAltText="Filter TEN_KHOI column" HeaderText="Thuộc khối học" SortExpression="TEN_KHOI" UniqueName="TEN_KHOI" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="THU_TU" HeaderStyle-Width="100px" FilterControlAltText="Filter THU_TU column" HeaderText="Thứ tự" SortExpression="THU_TU" UniqueName="THU_TU" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>

                        <%--<telerik:GridBoundColumn DataField="NANG_LUONG_KCAL_TU" HeaderText="NANG_LUONG_KCAL_TU" SortExpression="NANG_LUONG_KCAL_TU" UniqueName="NANG_LUONG_KCAL_TU" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NANG_LUONG_KCAL_DEN" HeaderText="NANG_LUONG_KCAL_DEN" SortExpression="NANG_LUONG_KCAL_DEN" UniqueName="NANG_LUONG_KCAL_DEN" Display="false">
                        </telerik:GridBoundColumn>                       
                        <telerik:GridBoundColumn DataField="PROTID_TU" HeaderText="PROTID_TU" SortExpression="PROTID_TU" UniqueName="PROTID_TU" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="PROTID_DEN" HeaderText="PROTID_DEN" SortExpression="PROTID_DEN" UniqueName="PROTID_DEN" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="LIPID_TU" HeaderText="LIPID_TU" SortExpression="LIPID_TU" UniqueName="LIPID_TU" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="LIPID_DEN" HeaderText="LIPID_DEN" SortExpression="LIPID_DEN" UniqueName="LIPID_DEN" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="GLUCID_TU" HeaderText="GLUCID_TU" SortExpression="GLUCID_TU" UniqueName="GLUCID_TU" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="GLUCID_DEN" HeaderText="GLUCID_DEN" SortExpression="GLUCID_DEN" UniqueName="GLUCID_DEN" Display="false">
                        </telerik:GridBoundColumn>--%>

                        <telerik:GridBoundColumn DataField="NANG_LUONG_TU_KCAL" HeaderText="NANG_LUONG_TU_KCAL" SortExpression="NANG_LUONG_TU_KCAL" UniqueName="NANG_LUONG_TU_KCAL" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NANG_LUONG_DEN_KCAL" HeaderText="NANG_LUONG_DEN_KCAL" SortExpression="NANG_LUONG_DEN_KCAL" UniqueName="NANG_LUONG_DEN_KCAL" Display="false">
                        </telerik:GridBoundColumn>                       
                        <telerik:GridBoundColumn DataField="PROTID_TU_KCAL" HeaderText="PROTID_TU_KCAL" SortExpression="PROTID_TU_KCAL" UniqueName="PROTID_TU_KCAL" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="PROTID_DEN_KCAL" HeaderText="PROTID_DEN_KCAL" SortExpression="PROTID_DEN_KCAL" UniqueName="PROTID_DEN_KCAL" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="LIPID_TU_KCAL" HeaderText="LIPID_TU_KCAL" SortExpression="LIPID_TU_KCAL" UniqueName="LIPID_TU_KCAL" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="LIPID_DEN_KCAL" HeaderText="LIPID_DEN_KCAL" SortExpression="LIPID_DEN_KCAL" UniqueName="LIPID_DEN_KCAL" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="GLUCID_TU_KCAL" HeaderText="GLUCID_TU_KCAL" SortExpression="GLUCID_TU_KCAL" UniqueName="GLUCID_TU_KCAL" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="GLUCID_DEN_KCAL" HeaderText="GLUCID_DEN_KCAL" SortExpression="GLUCID_DEN_KCAL" UniqueName="GLUCID_DEN_KCAL" Display="false">
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn HeaderStyle-Width="120px" FilterControlAltText="Filter KCAL column" HeaderText="Tổng NL (Kcal)" SortExpression="KCAL" UniqueName="KCAL" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-Width="120px" FilterControlAltText="Filter PROTID column" HeaderText="PROTID" SortExpression="PROTID" UniqueName="PROTID" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="NL">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-Width="120px" FilterControlAltText="Filter LIPID column" HeaderText="LIPID" SortExpression="LIPID" UniqueName="LIPID" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="NL">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-Width="120px" FilterControlAltText="Filter GLUCID column" HeaderText="GLUCID" SortExpression="GLUCID" UniqueName="GLUCID" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="NL">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Sửa" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="SUA">
                            <ItemTemplate>
                                <img src="../img/sua.png" onclick='<%# "openRadWin(\"DMBuAnDetail.aspx?id_hoso=" + Eval("ID") + "\",100, 50)" %>' style="cursor: pointer;" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
