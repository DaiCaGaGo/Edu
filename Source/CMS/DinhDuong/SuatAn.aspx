<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="SuatAn.aspx.cs" Inherits="CMS.DinhDuong.SuatAn" %>

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
             <telerik:AjaxSetting AjaxControlID="rcbKhoi">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbBuaAn" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
             <telerik:AjaxSetting AjaxControlID="rcbBuaAn">
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
                Height="400px" Title="Chi tiết suất ăn" Behaviors="Reload,Close,Maximize" Modal="true" VisibleStatusbar="false">
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
                <span class="item-title">danh mục suất ăn hàng ngày</span>
            </div>
            <div class="col-sm-8 text-right">
                <input type="button" id="btAddNew" runat="server" class="btn bt-one" value="Thêm mới" onclick="openRadWin('SuatAnDetail.aspx', 100, 50)" />
                <asp:Button ID="btDeleteChon" runat="server" CssClass="btn bt-one" OnClick="btDeleteChon_Click" Text="Xóa" OnClientClick="if(!btDeteleClick()) return false;" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="row">
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbKhoi" runat="server" Width="100%" DataSourceID="objKhoi"
                        DataTextField="TEN" DataValueField="MA" EmptyMessage="Chọn khối học" AllowCustomText="true" Filter="Contains" OnSelectedIndexChanged="rcbKhoi_SelectedIndexChanged" AutoPostBack="true">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objKhoi" runat="server" SelectMethod="getKhoiByCapHoc" TypeName="OneEduDataAccess.BO.KhoiBO">
                        <SelectParameters>
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbBuaAn" runat="server" Width="100%" DataSourceID="objBuaAn"
                        DataTextField="TEN" DataValueField="ID" EmptyMessage="Chọn bữa ăn" Filter="Contains" OnSelectedIndexChanged="rcbBuaAn_SelectedIndexChanged" AutoPostBack="true">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objBuaAn" runat="server" SelectMethod="getBuaAnByTruongKhoi" TypeName="OneEduDataAccess.BO.DMBuaAnBO">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="rcbKhoi" Name="id_khoi" PropertyName="SelectedValue" />
                            <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                            <asp:Parameter DefaultValue="" Name="id_all" Type="Int64" />
                            <asp:Parameter Name="text_all" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
                <div class="col-sm-3">
                    <telerik:RadDatePicker Culture="vi-VN" RenderMode="Classic" ID="rdNgay" runat="server" Width="100%" MinDate="1900/1/1"
                        Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="Giá trị không hợp lệ"
                        Calendar-FastNavigationSettings-TodayButtonCaption="Hôm nay"
                        DatePopupButton-ToolTip="Chọn ngày"
                        Calendar-FastNavigationSettings-OkButtonCaption="Chọn"
                        Calendar-FastNavigationSettings-CancelButtonCaption="Hủy" OnSelectedDateChanged="rdNgay_SelectedDateChanged" AutoPostBack="true">
                        <DateInput DateFormat="dd/MM/yyyy"></DateInput>
                    </telerik:RadDatePicker>
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
                        <telerik:GridClientSelectColumn UniqueName="chkChon" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridClientSelectColumn>
                        <telerik:GridTemplateColumn HeaderText="STT" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%# Container.DataSetIndex+1 %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="ID_BUA_AN" FilterControlAltText="Filter ID_BUA_AN column" HeaderText="ID_BUA_AN" SortExpression="ID_BUA_AN" UniqueName="ID_BUA_AN" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ID_THUC_DON" FilterControlAltText="Filter ID_THUC_DON column" HeaderText="ID_THUC_DON" SortExpression="ID_THUC_DON" UniqueName="ID_THUC_DON" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ID_KHOI" FilterControlAltText="Filter ID_KHOI column" HeaderText="ID_KHOI" SortExpression="ID_KHOI" UniqueName="ID_KHOI" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>       
                        <telerik:GridBoundColumn DataField="TEN_KHOI" FilterControlAltText="Filter TEN_KHOI column" HeaderText="Khối" SortExpression="TEN_KHOI" UniqueName="TEN_KHOI" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>                 
                        <telerik:GridBoundColumn DataField="TEN_BUA_AN" FilterControlAltText="Filter TEN_BUA_AN column" HeaderText="Tên bữa ăn" SortExpression="TEN_BUA_AN" UniqueName="TEN_BUA_AN" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TEN_THUC_DON" FilterControlAltText="Filter TEN_THUC_DON column" HeaderText="Tên thực đơn" SortExpression="TEN_THUC_DON" UniqueName="TEN_THUC_DON" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SO_HS_DANG_KY" FilterControlAltText="Filter SO_HS_DANG_KY column" HeaderText="Số HS ĐK" SortExpression="SO_HS_DANG_KY" UniqueName="SO_HS_DANG_KY" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TONG_NANG_LUONG_KCAL" ColumnGroupName="TPDD" FilterControlAltText="Filter TONG_NANG_LUONG_KCAL column" HeaderText="Năng lượng" SortExpression="TONG_NANG_LUONG_KCAL" UniqueName="TONG_NANG_LUONG_KCAL" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:###,###}">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TONG_PROTID" ColumnGroupName="TPDD" FilterControlAltText="Filter TONG_PROTID column" HeaderText="Protid" SortExpression="TONG_PROTID" UniqueName="TONG_PROTID" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:###,###}">
                        </telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="TONG_GLUCID" ColumnGroupName="TPDD" FilterControlAltText="Filter TONG_GLUCID column" HeaderText="Glucid" SortExpression="TONG_GLUCID" UniqueName="GLUCID" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:###,###}">
                        </telerik:GridNumericColumn>
                        <telerik:GridBoundColumn DataField="TONG_LIPID" ColumnGroupName="TPDD" FilterControlAltText="Filter TONG_LIPID column" HeaderText="Lipid" SortExpression="TONG_LIPID" UniqueName="TONG_LIPID" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Sửa" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="SUA">
                            <ItemTemplate>
                                <img src="../img/sua.png" onclick='<%# "openRadWin(\"SuatAnDetail.aspx?id=" + Eval("ID") + "&id_truong=" + Eval("ID_TRUONG") + "&id_khoi=" + Eval("ID_KHOI")+ "\",100,50)" %>' style="cursor: pointer;" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
