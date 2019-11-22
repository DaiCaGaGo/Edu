<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="DangKyAn.aspx.cs" Inherits="CMS.DinhDuong.DangKyAn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btTimKiem">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbKhoi">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbBuaAn" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="rcbLop" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="lblSoHSDangKy" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbLop">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblSoHSDangKy" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbHocKy">
                <UpdatedControls>
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
            <%--<telerik:AjaxSetting AjaxControlID="btnSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblSoHSDangKy"/>
                </UpdatedControls>
            </telerik:AjaxSetting>--%>
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
            function CheckBoxHeaderClick(cb, cssLst) {
                $('.' + cssLst + ' input').each(function () {
                    if ($(this).attr('disabled') != "disabled") {
                        $(this).prop('checked', cb.checked);
                    }
                });
            }
        </script>
    </telerik:RadCodeBlock>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">Danh sách học sinh đăng ký ăn</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btTimKiem" runat="server" Text="Tìm kiếm" CssClass="btn bt-one" OnClick="btTimKiem_Click" />
                <asp:Button ID="btnSave" runat="server" CssClass="btn bt-one" Text="Lưu" OnClientClick="if (confirm('Bạn có chắc chắn muốn cập nhật lại?')) return true; else return false;" OnClick="btnSave_Click" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="row">
                <div class="col-sm-4">
                    <telerik:RadComboBox ID="rcbKhoi" runat="server" Width="100%" DataSourceID="objKhoi" DataTextField="TEN" DataValueField="MA" AutoPostBack="True" Filter="Contains" OnSelectedIndexChanged="rcbKhoi_SelectedIndexChanged">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objKhoi" runat="server" SelectMethod="getKhoiByCapHoc" TypeName="OneEduDataAccess.BO.KhoiBO">
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
                    <telerik:RadComboBox ID="rcbHocKy" runat="server" Width="100%" Filter="Contains" OnSelectedIndexChanged="rcbHocKy_SelectedIndexChanged" AutoPostBack="true">
                        <Items>
                            <telerik:RadComboBoxItem Value="1" Text="Học kỳ I" Selected="true" />
                            <telerik:RadComboBoxItem Value="2" Text="Học kỳ II" />
                        </Items>
                    </telerik:RadComboBox>
                </div>
            </div>
            <div class="row" style="margin-top: 15px;">
                <div class="col-sm-4">
                    <telerik:RadComboBox ID="rcbBuaAn" runat="server" Width="100%" DataSourceID="objBuaAn" EmptyMessage="Chọn bữa ăn" AllowCustomText="true"
                        DataTextField="TEN" DataValueField="ID" Filter="Contains" OnSelectedIndexChanged="rcbBuaAn_SelectedIndexChanged" AutoPostBack="true">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objBuaAn" runat="server" SelectMethod="getDM_BUA_ANByTruongKhoiAndNhomTuoi" TypeName="OneEduDataAccess.BO.DMBuaAnBO">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="rcbKhoi" Name="id_khoi" PropertyName="SelectedValue" />
                            <asp:Parameter DefaultValue="" Name="id_nhom_tuoi_mn" Type="Int16" />
                            <asp:Parameter DefaultValue=" " Name="ten" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
                <div class="col-sm-4">
                    <telerik:RadComboBox ID="rcbTrangThaiDangKy" runat="server" Width="100%" EmptyMessage="Chọn trạng thái đăng ký" AllowCustomText="true" Filter="Contains" OnSelectedIndexChanged="rcbTrangThaiDangKy_SelectedIndexChanged" AutoPostBack="true">
                        <Items>
                            <telerik:RadComboBoxItem Value="1" Text="Đã đăng ký" />
                            <telerik:RadComboBoxItem Value="0" Text="Chưa đăng ký" />
                        </Items>
                    </telerik:RadComboBox>
                </div>
                <div class="col-sm-4">
                    <telerik:RadTextBox ID="tbTen" runat="server" EmptyMessage="Nhập tên học sinh cần tìm kiếm" Width="100%"></telerik:RadTextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-12">
                    <asp:Label ID="lblSoHSDangKy" runat="server" Text="" ForeColor="Red"></asp:Label>
                </div>
            </div>
        </div>
        <div class="item-data">
            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" GroupPanelPosition="Top" OnNeedDataSource="RadGrid1_NeedDataSource"
                AutoGenerateColumns="False" PageSize="100" AllowMultiRowSelection="True" OnItemDataBound="RadGrid1_ItemDataBound" OnPreRender="RadGrid1_PreRender">
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
                        <telerik:GridTemplateColumn HeaderText="STT" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%# Container.DataSetIndex+1 %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="ID" FilterControlAltText="Filter ID column" HeaderText="ID" SortExpression="ID" UniqueName="ID" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ID_HOC_SINH" FilterControlAltText="Filter ID_HOC_SINH column" HeaderText="Mã HS" SortExpression="ID_HOC_SINH" UniqueName="ID_HOC_SINH" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ID_LOP" FilterControlAltText="Filter ID_LOP column" HeaderText="Lớp học" SortExpression="ID_LOP" UniqueName="ID_LOP" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ID_BUA_AN" FilterControlAltText="Filter ID_BUA_AN column" HeaderText="ID_BUA_AN" SortExpression="ID_BUA_AN" UniqueName="ID_BUA_AN" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MA_HOC_SINH" FilterControlAltText="Filter MA_HOC_SINH column" HeaderText="Mã HS" SortExpression="MA_HOC_SINH" UniqueName="MA_HOC_SINH" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="140px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="HO_TEN" FilterControlAltText="Filter HO_TEN column" HeaderText="Tên HS" SortExpression="HO_TEN" UniqueName="HO_TEN" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TEN_LOP" FilterControlAltText="Filter TEN_LOP column" HeaderText="Lớp học" SortExpression="TEN_LOP" UniqueName="TEN_LOP" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NGAY_SINH" FilterControlAltText="Filter NGAY_SINH column" HeaderText="Ngày sinh" SortExpression="NGAY_SINH" UniqueName="NGAY_SINH" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-Width="100px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TEN_BUA_AN" FilterControlAltText="Filter TEN_BUA_AN column" HeaderText="Tên bữa ăn" SortExpression="TEN_BUA_AN" UniqueName="TEN_BUA_AN" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Đăng ký ăn" UniqueName="IS_DK" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <span><asp:Literal ID="ltrHocKy" runat="server"></asp:Literal></span><br />
                                <asp:CheckBox runat="server" ID="ckhAllIS_DK" onclick="CheckBoxHeaderClick(this,'chb_IS_DK')" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chbIS_DK" CssClass="chb_IS_DK" />
                                <asp:HiddenField ID="hdIS_DK" runat="server" Value='<%#Bind("IS_DK") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PageSizes="20,50,100,200" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
