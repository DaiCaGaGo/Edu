<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="ChotSoTheoMon.aspx.cs" Inherits="CMS.ChotSo.ChotSoTheoMon" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbKhoiHoc">
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
            <telerik:AjaxSetting AjaxControlID="rcbKyDGTH">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <telerik:RadCodeBlock runat="server">
        <script>
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
                <span class="item-title">CHỐT SỔ THEO MÔN</span>
            </div>
            <div class="col-sm-8 text-right">
                <table style="float:right">
                    <tr>
                        <td style="padding-right:5px">
                            <asp:Button ID="btnMoKhoa" runat="server" CssClass="btn bt-one" Text="Mở khóa toàn trường" OnClick="btnMoKhoa_Click" OnClientClick="if(confirm('Bạn có chắc chắn muốn mở khóa?')) return true; else return false;"/>
                        </td>
                        <td style="padding-right:5px">
                            <asp:Button ID="btnKhoaToanTruong" runat="server" CssClass="btn bt-one" Text="Khóa môn toàn trường" OnClick="btnKhoaToanTruong_Click" OnClientClick="if(confirm('Bạn có chắc chắn muốn khóa tất cả các môn?')) return true; else return false;" />
                        </td>
                        <td>
                            <asp:Button ID="btEdit" runat="server" CssClass="btn bt-one" Text="Lưu" OnClick="btEdit_Click" OnClientClick="if(confirm('Bạn có chắc chắn muốn lưu?')) return true; else return false;"/>
                        </td>
                    </tr>
                </table>
                <%--
                --%>
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="form-horizontal" role="form">
                <div class="row">
                    <div class="col-sm-3">
                        <telerik:RadComboBox ID="rcbKhoiHoc" runat="server" Width="100%" DataSourceID="objKhoi"
                            DataTextField="TEN" DataValueField="MA" AutoPostBack="True"
                            OnSelectedIndexChanged="rcbKhoiHoc_SelectedIndexChanged" Filter="Contains">
                        </telerik:RadComboBox>
                        <asp:ObjectDataSource ID="objKhoi" runat="server" SelectMethod="getKhoiByCapHoc" TypeName="OneEduDataAccess.BO.KhoiBO">
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
                                <asp:ControlParameter ControlID="rcbKhoiHoc" Name="maKhoi" PropertyName="SelectedValue" />
                                <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                                <asp:Parameter DefaultValue="" Name="id_all" Type="Int64" />
                                <asp:Parameter Name="text_all" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </div>
                    <div class="col-sm-3" runat="server" id="giaiDoan" visible="true">
                        <telerik:RadComboBox ID="rcbKyDGTH" runat="server" Width="100%" DataSourceID="objKyDGTH"
                            DataTextField="TEN" DataValueField="MA" Filter="Contains" AutoPostBack="true" OnSelectedIndexChanged="rcbKyDGTH_SelectedIndexChanged">
                        </telerik:RadComboBox>
                        <asp:ObjectDataSource ID="objKyDGTH" runat="server" SelectMethod="getKyDGTHByKy" TypeName="OneEduDataAccess.BO.KyDGTHBO">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                                <asp:Parameter Name="id_all" Type="Int16" />
                                <asp:Parameter Name="text_all" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </div>
                    <%--<div class="col-sm-3 one-checkbox">
                        <label>
                            <asp:RadioButton ID="rbtKhoaMon" ClientIDMode="Static" runat="server" Text="Khóa sổ theo môn" GroupName="is_khoa" Checked="true" />
                            &nbsp;&nbsp;
                                    <asp:RadioButton ID="rbtKhoaTruong" ClientIDMode="Static" runat="server" Text="Khóa sổ toàn trường" GroupName="is_khoa" />
                        </label>
                    </div>--%>
                </div>
            </div>
        </div>
        <div class="item-data">
            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" GroupPanelPosition="Top" OnNeedDataSource="RadGrid1_NeedDataSource"
                AutoGenerateColumns="False" PageSize="50" AllowMultiRowSelection="True" OnItemDataBound="RadGrid1_ItemDataBound">
                <ClientSettings>
                    <Selecting AllowRowSelect="true"></Selecting>
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                </ClientSettings>
                <MasterTableView DataKeyNames="ID_MON" ClientDataKeyNames="ID_MON">
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
                        <telerik:GridBoundColumn DataField="TEN" FilterControlAltText="Filter TEN column" HeaderText="Tên môn" SortExpression="TEN" UniqueName="TEN" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="TRANG_THAI" UniqueName="TRANG_THAI" DataField="TRANG_THAI" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <span>Khóa sổ</span><br />
                                <asp:CheckBox runat="server" ID="ckhAll" onclick="CheckBoxHeaderClick(this,'chbTRANG_THAI')" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="ckIsCheck" CssClass="chbTRANG_THAI" />
                                <asp:HiddenField ID="hdIsCheck" runat="server" Value='<%#Bind("TRANG_THAI") %>' />
                                <asp:HiddenField ID="hdID_MON" runat="server" Value='<%#Bind("ID_MON") %>' />
                                <asp:HiddenField ID="hdID" runat="server" Value='<%#Bind("ID") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="NGAY_KHOA" HeaderStyle-Width="200px" FilterControlAltText="Filter NGAY_KHOA column" HeaderText="Ngày khóa" SortExpression="NGAY_KHOA" UniqueName="NGAY_KHOA" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <telerik:RadDatePicker Culture="vi-VN" RenderMode="Classic" ID="rdNgayKhoa" runat="server" Width="100%" MinDate="1900/1/1"
                                    Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="Giá trị không hợp lệ"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="Hôm nay"
                                    DatePopupButton-ToolTip="Chọn ngày"
                                    Calendar-FastNavigationSettings-OkButtonCaption="Chọn"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="Hủy">
                                    <DateInput runat="server" DateFormat="dd/MM/yyyy"></DateInput>
                                </telerik:RadDatePicker>
                                <asp:HiddenField ID="hdNgayKhoa" Value='<%# Eval("NGAY_KHOA") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="NGUOI_KHOA" HeaderStyle-Width="150px" FilterControlAltText="Filter NGUOI_KHOA column" HeaderText="Người khóa" SortExpression="NGUOI_KHOA" UniqueName="NGUOI_KHOA" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                    </Columns>
                    <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>

</asp:Content>
