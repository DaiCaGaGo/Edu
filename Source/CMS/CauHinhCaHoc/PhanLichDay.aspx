<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="PhanLichDay.aspx.cs" Inherits="CMS.CauHinhCaHoc.PhanLichDay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbKhoi">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbLop" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbLop">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbHocKy">
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
                Height="400px" Title="Chi tiết lịch dạy" Behaviors="Reload,Close,Maximize" Modal="true" VisibleStatusbar="false">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadCodeBlock runat="server">
        <script>
            var grid;
            function pageload() {
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
        </script>
        <style>
            div #btnDoiLich{
                padding: 3px 12px !important;
            }
        </style>
    </telerik:RadCodeBlock>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">Thời khóa biểu lớp</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btnLuu" runat="server" Text="Lưu" CssClass="btn bt-one" OnClick="btnLuu_Click" />
                <asp:Button ID="btDeleteChon" runat="server" CssClass="btn bt-one" OnClick="btDeleteChon_Click" Text="Xóa tất cả" OnClientClick="if (!confirm('Bạn có muốn xóa thông tin dữ liệu này không?')) return false;" />
                <asp:LinkButton runat="server" ID="btnImportExcel" href="\CauHinhCaHoc\ThoiKhoaBieuImportExcel.aspx" CssClass="btn bt-one" Font-Size="17px">Import excel</asp:LinkButton>
                <asp:Button ID="btExport" runat="server" CssClass="btn bt-one" OnClick="btExport_Click" Text="Xuất excel" />
                <asp:Button ID="btCopy" runat="server" CssClass="btn bt-one" Text="Copy kỳ I sang kỳ II" OnClientClick="if (confirm('Bạn chắc chắn muốn copy?')) return true; else return false;" OnClick="btCopy_Click" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="form-horizontal" role="form">
                <div class="row">
                    <div class="col-sm-4">

                        <telerik:RadComboBox ID="rcbKhoi" runat="server" Width="100%" DataSourceID="objKhoiHoc" DataTextField="TEN" DataValueField="MA" AutoPostBack="True" Filter="Contains" OnSelectedIndexChanged="rcbKhoi_SelectedIndexChanged">
                        </telerik:RadComboBox>
                        <asp:ObjectDataSource ID="objKhoiHoc" runat="server" SelectMethod="getKhoiByCapHoc" TypeName="OneEduDataAccess.BO.KhoiBO">
                            <SelectParameters>
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </div>
                    <div class="col-sm-4">
                        <telerik:RadComboBox ID="rcbLop" runat="server" Width="100%" DataSourceID="objLop"
                            DataTextField="TEN" DataValueField="ID" Filter="Contains" AutoPostBack="true" OnSelectedIndexChanged="rcbLop_SelectedIndexChanged">
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
                    <div class="col-sm-4">
                        <telerik:RadComboBox ID="rcbHocKy" runat="server" Width="100%" EmptyMessage="Chọn học kỳ" AllowCustomText="true" Filter="Contains" AutoPostBack="true" OnSelectedIndexChanged="rcbHocKy_SelectedIndexChanged">
                            <Items>
                                <telerik:RadComboBoxItem Value="1" Text="Học kỳ 1" />
                                <telerik:RadComboBoxItem Value="2" Text="Học kỳ 2" />
                            </Items>
                        </telerik:RadComboBox>
                    </div>
                </div>
                <div class="row" style="margin-top: 5px">
                    <div class="col-sm-4">
                        <telerik:RadComboBox ID="rcbCauHinhCaHoc" runat="server" Width="100%" DataSourceID="objCaHoc" DataTextField="MUA" DataValueField="ID" EmptyMessage="Chọn thời gian học" AllowCustomText="true" Filter="Contains">
                        </telerik:RadComboBox>
                        <asp:ObjectDataSource ID="objCaHoc" runat="server" SelectMethod="getCauHinhCaHocByTruong" TypeName="OneEduDataAccess.BO.CauHinhCaHocBO">
                            <SelectParameters>
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </div>
                    <div class="col-sm-4">
                        <asp:Button ID="btnDoiLich" runat="server" Text="Đổi cấu hình" ClientIDMode="Static" CssClass="btn bt-one" OnClick="btnDoiLich_Click" />
                    </div>
                </div>
            </div>
        </div>
        <div class="item-data">
            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" GroupPanelPosition="Top" OnNeedDataSource="RadGrid1_NeedDataSource"
                AutoGenerateColumns="False" PageSize="50" AllowMultiRowSelection="True" OnItemDataBound="RadGrid1_ItemDataBound">
                <ClientSettings>
                    <Selecting AllowRowSelect="true"></Selecting>
                    <%--<Scrolling AllowScroll="true" UseStaticHeaders="true" />--%>
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
                        <telerik:GridBoundColumn DataField="Tiet" HeaderStyle-Width="50px" FilterControlAltText="Filter TIET column" HeaderText="Tiết" SortExpression="TIET" UniqueName="TIET" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Thứ 2" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="ID_MON_2">
                            <ItemTemplate>
                                <telerik:RadComboBox runat="server" ID="rcbID_MON_2" DataTextField="text" EmptyMessage="--" DataValueField="value" DataSourceID="objMon" Width="100%" MaxHeight="250px" AllowCustomText="true" Filter="Contains">
                                </telerik:RadComboBox>
                                <asp:HiddenField ID="hdID_MON_2" runat="server" Value='<%#Bind("ID_MON_2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Thứ 3" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="ID_MON_3">
                            <ItemTemplate>
                                <telerik:RadComboBox runat="server" ID="rcbID_MON_3" DataTextField="text" EmptyMessage="--" DataValueField="value" DataSourceID="objMon" Width="100%" MaxHeight="250px" AllowCustomText="true" Filter="Contains">
                                </telerik:RadComboBox>
                                <asp:HiddenField ID="hdID_MON_3" runat="server" Value='<%#Bind("ID_MON_3") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Thứ 4" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="ID_MON_4">
                            <ItemTemplate>
                                <telerik:RadComboBox runat="server" ID="rcbID_MON_4" DataTextField="text" EmptyMessage="--" DataValueField="value" DataSourceID="objMon" Width="100%" MaxHeight="250px" AllowCustomText="true" Filter="Contains">
                                </telerik:RadComboBox>
                                <asp:HiddenField ID="hdID_MON_4" runat="server" Value='<%#Bind("ID_MON_4") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Thứ 5" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="ID_MON_5">
                            <ItemTemplate>
                                <telerik:RadComboBox runat="server" ID="rcbID_MON_5" DataTextField="text" EmptyMessage="--" DataValueField="value" DataSourceID="objMon" Width="100%" MaxHeight="250px" AllowCustomText="true" Filter="Contains">
                                </telerik:RadComboBox>
                                <asp:HiddenField ID="hdID_MON_5" runat="server" Value='<%#Bind("ID_MON_5") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Thứ 6" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="ID_MON_6">
                            <ItemTemplate>
                                <telerik:RadComboBox runat="server" ID="rcbID_MON_6" DataTextField="text" EmptyMessage="--" DataValueField="value" DataSourceID="objMon" Width="100%" MaxHeight="250px" AllowCustomText="true" Filter="Contains">
                                </telerik:RadComboBox>
                                <asp:HiddenField ID="hdID_MON_6" runat="server" Value='<%#Bind("ID_MON_6") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Thứ 7" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="ID_MON_7">
                            <ItemTemplate>
                                <telerik:RadComboBox runat="server" ID="rcbID_MON_7" DataTextField="text" EmptyMessage="--" DataValueField="value" DataSourceID="objMon" Width="100%" MaxHeight="250px" AllowCustomText="true" Filter="Contains">
                                </telerik:RadComboBox>
                                <asp:HiddenField ID="hdID_MON_7" runat="server" Value='<%#Bind("ID_MON_7") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Chủ nhật" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="ID_MON_8">
                            <ItemTemplate>
                                <telerik:RadComboBox runat="server" ID="rcbID_MON_8" DataTextField="text" EmptyMessage="--" DataValueField="value" DataSourceID="objMon" Width="100%" MaxHeight="250px" AllowCustomText="true" Filter="Contains">
                                </telerik:RadComboBox>
                                <asp:HiddenField ID="hdID_MON_8" runat="server" Value='<%#Bind("ID_MON_8") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                </MasterTableView>
            </telerik:RadGrid>
            <asp:ObjectDataSource ID="objMon" runat="server" SelectMethod="getMonTruongByLopHocKyToCombo" TypeName="OneEduDataAccess.BO.LopMonBO">
                <SelectParameters>
                    <asp:ControlParameter ControlID="rcbLop" Name="id_lop" PropertyName="SelectedValue" />
                    <asp:Parameter Name="is_all" DbType="Boolean" DefaultValue="True" />
                    <asp:Parameter Name="id_all" DbType="Int64" DefaultValue="0" />
                    <asp:Parameter Name="text_all" DbType="String" DefaultValue="--" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>
</asp:Content>
