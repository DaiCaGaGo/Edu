<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="DotThuLop.aspx.cs" Inherits="CMS.HocPhi.DotThuLop" %>

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
                    <telerik:AjaxUpdatedControl ControlID="rcbLop" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbLop">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbDotThu">
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
                Height="400px" Title="Chi tiết mã nhận xét" Behaviors="Reload,Close,Maximize" Modal="true" VisibleStatusbar="false">
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
                    if (confirm("Bạn có chắc chắn muốn xóa?")) {
                        return true;
                    }
                } else {
                    alert("Bạn chưa chọn bản ghi nào để xóa.");
                    return false;
                }
            }
            function btTaoPhieuThuHS() {
                if (grid.get_masterTableView().get_selectedItems().length > 0) {
                    if (confirm("Bạn có chắc chắn muốn tạo phiếu thu cho lớp")) {
                        return true;
                    }
                } else {
                    alert("Bạn chưa chọn bản ghi nào để xóa.");
                    return false;
                }
            }
        </script>
    </telerik:RadCodeBlock>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">KHOẢN THU THEO LỚP</span>
            </div>
            <div class="col-sm-8 text-right">
                <input type="button" id="btAddNew" runat="server" class="btn bt-one" value="Thêm mới" onclick="openRadWin('DotThuLopDetail.aspx', 860, 550, 1)" />
                <asp:Button ID="btDeleteChon" runat="server" CssClass="btn bt-one" OnClick="btDeleteChon_Click" Text="Xóa" OnClientClick="if(!btDeteleClick()) return false;" />
                <asp:Button ID="btExport" runat="server" CssClass="btn bt-one" OnClick="btExport_Click" Text="Xuất excel" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="row">
                <div class="col-sm-2">
                    <telerik:RadComboBox ID="rcbKhoi" runat="server" Width="100%" DataSourceID="objKhoi" DataTextField="TEN" DataValueField="MA" EmptyMessage="Chọn khối" AllowCustomText="true" Filter="Contains" OnSelectedIndexChanged="rcbKhoi_SelectedIndexChanged" AutoPostBack="True">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objKhoi" runat="server" SelectMethod="getKhoiByCapHoc" TypeName="OneEduDataAccess.BO.KhoiBO">
                        <SelectParameters>
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
                <div class="col-sm-2">
                    <telerik:RadComboBox ID="rcbLop" runat="server" Width="100%" DataSourceID="objLop"
                        DataTextField="TEN" DataValueField="ID" EmptyMessage="Chọn lớp" AllowCustomText="true" Filter="Contains" AutoPostBack="true" OnSelectedIndexChanged="rcbLop_SelectedIndexChanged">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objLop" runat="server" SelectMethod="getLopByKhoiNamHoc" TypeName="OneEduDataAccess.BO.LopBO">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="rcbKhoi" Name="maKhoi" PropertyName="SelectedValue" />
                            <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                            <asp:Parameter DefaultValue="" Name="id_all" Type="Int64" />
                            <asp:Parameter Name="text_all" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
                <div class="col-sm-2">
                    <telerik:RadComboBox ID="rcbDotThu" runat="server" Width="100%" DataSourceID="objDotThu"
                        DataTextField="TEN" DataValueField="ID" EmptyMessage="Chọn đợt thu" AllowCustomText="true" Filter="Contains" AutoPostBack="true" OnSelectedIndexChanged="rcbDotThu_SelectedIndexChanged">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objDotThu" runat="server" SelectMethod="getAllDotThuByTruong" TypeName="OneEduDataAccess.BO.HocPhiDotThuBO">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                            <asp:Parameter DefaultValue="" Name="id_all" Type="Int64" />
                            <asp:Parameter Name="text_all" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
            </div>
        </div>
        <div class="item-data">
            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" GroupPanelPosition="Top" OnNeedDataSource="RadGrid1_NeedDataSource"
                AutoGenerateColumns="False" PageSize="100" AllowMultiRowSelection="True" OnItemDataBound="RadGrid1_ItemDataBound" OnItemCommand="RadGrid1_ItemCommand">
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
                    <Columns>
                        <telerik:GridClientSelectColumn UniqueName="chkChon" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridClientSelectColumn>
                        <telerik:GridTemplateColumn HeaderText="STT" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%# Container.DataSetIndex+1 %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="ID_KHOI" FilterControlAltText="Filter ID_KHOI column" HeaderText="ID_KHOI" SortExpression="ID_KHOI" UniqueName="ID_KHOI" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ID_LOP" FilterControlAltText="Filter ID_LOP column" HeaderText="ID_LOP" SortExpression="ID_LOP" UniqueName="ID_LOP" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TEN_LOP" FilterControlAltText="Filter TEN_LOP column" HeaderText="Tên lớp" SortExpression="TEN_LOP" UniqueName="TEN_LOP" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ID_DOT_THU" FilterControlAltText="Filter ID_DOT_THU column" HeaderText="ID_DOT_THU" SortExpression="ID_DOT_THU" UniqueName="ID_DOT_THU" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TEN_DOT_THU" FilterControlAltText="Filter TEN_DOT_THU column" HeaderText="Đợt thu" SortExpression="TEN_DOT_THU" UniqueName="TEN_DOT_THU" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="250px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TONG_TIEN" FilterControlAltText="Filter TONG_TIEN column" HeaderText="Tổng tiền (VNĐ)" SortExpression="TONG_TIEN" UniqueName="TONG_TIEN" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px" DataFormatString="{0:#,0}">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Tạo phiếu thu HS" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%--<asp:Button ID="btnSave" runat="server" CommandName="TaoPHieuThuHS" OnClientClick="if(!btDeteleClick()) return false;" CommandArgument='<%# Eval("ID") %>' Text="Tạo" />--%>
                                <asp:ImageButton runat="server" ImageUrl="~/img/sua.png" CommandName="TaoPHieuThuHS" CommandArgument='<%# Eval("ID") %>' OnClientClick="if (confirm('Bạn có chắc chắn muốn tạo phiếu thu cho lớp?')) { return true; } return false;"/>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center"/>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sửa" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="SUA">
                            <ItemTemplate>
                                <img src="../img/sua.png" onclick='<%# "openRadWin(\"DotThuLopDetail.aspx?id=" + Eval("ID") + "\", 860, 550, 1)" %>' style="cursor: pointer;" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
