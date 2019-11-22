<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="ChamAn.aspx.cs" Inherits="CMS.DinhDuong.ChamAn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
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
            <telerik:AjaxSetting AjaxControlID="rdNgay">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="btSave" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <telerik:RadCodeBlock runat="server">
        <script>
            var grid;
            function pageload() {
                grid = $find("<%=RadGrid1.ClientID%>");
            }
            function ProcessCellValue(cell) {
                try {
                    if (cell != null) {
                        var cval;
                        cval = ChangeValidValueP_K(cell.value);
                        if (cval != cell.value || NeedJump == true) { cell.value = cval; NeedJump = true; }
                        if (cval == '') NeedJump = false;
                        var cellIndex = $(cell).parent().index();
                        if (NeedJump) {
                            if (is_jump_col == "1" || is_jump_col == 1) {
                                nextRow(cell);
                            }
                            else {
                                nextCol(cell, cellIndex);
                            }
                        }
                    }
                    NeedJump = false;
                }
                catch (ex) { }
            }
            $(document).on("keyup", ".text-box", function (event) {
                if (event.which != 37 && event.which != 38 && event.which != 39 && event.which != 40) {
                    ProcessCellValue(this);
                } else {
                    JumCell(this, event.which);
                }
            });
            function btLuuClick() {
                if (confirm("Bạn chắc chắn muốn lưu dữ liệu?")) {
                    return true;
                }
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
                <span class="item-title">CHẤM ĂN HÀNG NGÀY</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btSave" runat="server" CssClass="btn bt-one" OnClick="btSave_Click" Text="Lưu" OnClientClick="if(!btLuuClick()) return false;" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="form-horizontal" role="form">
                <div class="row" style="margin-top: 5px">
                    <div class="col-sm-3">

                        <telerik:RadComboBox ID="rcbKhoi" runat="server" Width="100%" DataSourceID="objKhoiHoc" DataTextField="TEN" DataValueField="MA" AutoPostBack="True" Filter="Contains" OnSelectedIndexChanged="rcbKhoi_SelectedIndexChanged">
                        </telerik:RadComboBox>
                        <asp:ObjectDataSource ID="objKhoiHoc" runat="server" SelectMethod="getKhoiByCapHoc" TypeName="OneEduDataAccess.BO.KhoiBO">
                            <SelectParameters>
                            </SelectParameters>
                        </asp:ObjectDataSource>

                    </div>
                    <div class="col-sm-3">
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
        </div>
        <div class="item-data">
            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" GroupPanelPosition="Top" OnNeedDataSource="RadGrid1_NeedDataSource" OnItemDataBound="RadGrid1_ItemDataBound"
                AutoGenerateColumns="False" PageSize="50" AllowMultiRowSelection="True">
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
                        <telerik:GridBoundColumn DataField="MA_HS" FilterControlAltText="Filter MA_HS column" HeaderText="Mã HS" SortExpression="MA_HS" UniqueName="MA_HS" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TEN_HS" FilterControlAltText="Filter TEN_HS column" HeaderText="Họ tên" SortExpression="TEN_HS" UniqueName="TEN_HS" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ID_HOC_SINH" FilterControlAltText="Filter ID_HOC_SINH column" HeaderText="ID_HOC_SINH" SortExpression="ID_HOC_SINH" UniqueName="ID_HOC_SINH" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NGAY_SINH" FilterControlAltText="Filter NGAY_SINH column" HeaderText="Ngày sinh" SortExpression="NGAY_SINH" UniqueName="NGAY_SINH" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-Width="150px">
                        </telerik:GridBoundColumn>
                        <%--Bữa 0--%>
                        <telerik:GridTemplateColumn HeaderText="0" UniqueName="BUA0" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <span>
                                    <asp:Literal ID="ltrBua0" runat="server"></asp:Literal></span><br />
                                <asp:CheckBox runat="server" ID="ckhAll_BUA0" onclick="CheckBoxHeaderClick(this,'chb_BUA0')" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chbBUA0" CssClass="chb_BUA0" />
                                <asp:HiddenField ID="hdIS_BUA_AN_0" runat="server" Value='<%#Bind("IS_BUA_AN_0") %>' />
                                <asp:HiddenField ID="hdBUA_AN_0" runat="server" Value='<%#Bind("BUA_AN_0") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End bữa 0--%>
                        <%--Bữa 1--%>
                        <telerik:GridTemplateColumn HeaderText="1" UniqueName="BUA1" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <span>
                                    <asp:Literal ID="ltrBua1" runat="server"></asp:Literal></span><br />
                                <asp:CheckBox runat="server" ID="ckhAll_BUA1" onclick="CheckBoxHeaderClick(this,'chb_BUA1')" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chbBUA1" CssClass="chb_BUA1" />
                                <asp:HiddenField ID="hdIS_BUA_AN_1" runat="server" Value='<%#Bind("IS_BUA_AN_1") %>' />
                                <asp:HiddenField ID="hdBUA_AN_1" runat="server" Value='<%#Bind("BUA_AN_1") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End bữa 1--%>
                        <%--Bữa 2--%>
                        <telerik:GridTemplateColumn HeaderText="2" UniqueName="BUA2" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <span>
                                    <asp:Literal ID="ltrBua2" runat="server"></asp:Literal></span><br />
                                <asp:CheckBox runat="server" ID="ckhAll_BUA2" onclick="CheckBoxHeaderClick(this,'chb_BUA2')" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chbBUA2" CssClass="chb_BUA2" />
                                <asp:HiddenField ID="hdIS_BUA_AN_2" runat="server" Value='<%#Bind("IS_BUA_AN_2") %>' />
                                <asp:HiddenField ID="hdBUA_AN_2" runat="server" Value='<%#Bind("BUA_AN_2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End bữa 2--%>
                        <%--Bữa 3--%>
                        <telerik:GridTemplateColumn HeaderText="3" UniqueName="BUA3" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <span>
                                    <asp:Literal ID="ltrBua3" runat="server"></asp:Literal></span><br />
                                <asp:CheckBox runat="server" ID="ckhAll_BUA3" onclick="CheckBoxHeaderClick(this,'chb_BUA3')" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chbBUA3" CssClass="chb_BUA3" />
                                <asp:HiddenField ID="hdIS_BUA_AN_3" runat="server" Value='<%#Bind("IS_BUA_AN_3") %>' />
                                <asp:HiddenField ID="hdBUA_AN_3" runat="server" Value='<%#Bind("BUA_AN_3") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End bữa 3--%>
                        <%--Bữa 4--%>
                        <telerik:GridTemplateColumn HeaderText="4" UniqueName="BUA4" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <span>
                                    <asp:Literal ID="ltrBua4" runat="server"></asp:Literal></span><br />
                                <asp:CheckBox runat="server" ID="ckhAll_BUA4" onclick="CheckBoxHeaderClick(this,'chb_BUA4')" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chbBUA4" CssClass="chb_BUA4" />
                                <asp:HiddenField ID="hdIS_BUA_AN_4" runat="server" Value='<%#Bind("IS_BUA_AN_4") %>' />
                                <asp:HiddenField ID="hdBUA_AN_4" runat="server" Value='<%#Bind("BUA_AN_4") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End bữa 4--%>
                        <%--Bữa 5--%>
                        <telerik:GridTemplateColumn HeaderText="5" UniqueName="BUA5" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <span>
                                    <asp:Literal ID="ltrBua5" runat="server"></asp:Literal></span><br />
                                <asp:CheckBox runat="server" ID="ckhAll_BUA5" onclick="CheckBoxHeaderClick(this,'chb_BUA5')" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chbBUA5" CssClass="chb_BUA5" />
                                <asp:HiddenField ID="hdIS_BUA_AN_5" runat="server" Value='<%#Bind("IS_BUA_AN_5") %>' />
                                <asp:HiddenField ID="hdBUA_AN_5" runat="server" Value='<%#Bind("BUA_AN_5") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End bữa 5--%>
                        <%--Bữa 6--%>
                        <telerik:GridTemplateColumn HeaderText="6" UniqueName="BUA6" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <span>
                                    <asp:Literal ID="ltrBua6" runat="server"></asp:Literal></span><br />
                                <asp:CheckBox runat="server" ID="ckhAll_BUA6" onclick="CheckBoxHeaderClick(this,'chb_BUA6')" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chbBUA6" CssClass="chb_BUA6" />
                                <asp:HiddenField ID="hdIS_BUA_AN_6" runat="server" Value='<%#Bind("IS_BUA_AN_6") %>' />
                                <asp:HiddenField ID="hdBUA_AN_6" runat="server" Value='<%#Bind("BUA_AN_6") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End bữa 6--%>
                        <%--Bữa 7--%>
                        <telerik:GridTemplateColumn HeaderText="7" UniqueName="BUA7" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <span>
                                    <asp:Literal ID="ltrBua7" runat="server"></asp:Literal></span><br />
                                <asp:CheckBox runat="server" ID="ckhAll_BUA7" onclick="CheckBoxHeaderClick(this,'chb_BUA7')" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chbBUA7" CssClass="chb_BUA7" />
                                <asp:HiddenField ID="hdIS_BUA_AN_7" runat="server" Value='<%#Bind("IS_BUA_AN_7") %>' />
                                <asp:HiddenField ID="hdBUA_AN_7" runat="server" Value='<%#Bind("BUA_AN_7") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End bữa 7--%>
                        <%--Bữa 8--%>
                        <telerik:GridTemplateColumn HeaderText="8" UniqueName="BUA8" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <span>
                                    <asp:Literal ID="ltrBua8" runat="server"></asp:Literal></span><br />
                                <asp:CheckBox runat="server" ID="ckhAll_BUA8" onclick="CheckBoxHeaderClick(this,'chb_BUA8')" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chbBUA8" CssClass="chb_BUA8" />
                                <asp:HiddenField ID="hdIS_BUA_AN_8" runat="server" Value='<%#Bind("IS_BUA_AN_8") %>' />
                                <asp:HiddenField ID="hdBUA_AN_8" runat="server" Value='<%#Bind("BUA_AN_8") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End bữa 8--%>
                        <%--Bữa 9--%>
                        <telerik:GridTemplateColumn HeaderText="9" UniqueName="BUA9" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <span>
                                    <asp:Literal ID="ltrBua9" runat="server"></asp:Literal></span><br />
                                <asp:CheckBox runat="server" ID="ckhAll_BUA9" onclick="CheckBoxHeaderClick(this,'chb_BUA9')" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chbBUA9" CssClass="chb_BUA9" />
                                <asp:HiddenField ID="hdIS_BUA_AN_9" runat="server" Value='<%#Bind("IS_BUA_AN_9") %>' />
                                <asp:HiddenField ID="hdBUA_AN_9" runat="server" Value='<%#Bind("BUA_AN_9") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End bữa 9--%>
                    </Columns>
                    <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
