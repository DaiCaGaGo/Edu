<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="ChuyenLop.aspx.cs" Inherits="CMS.HocSinh.ChuyenLop" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbTuLop">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbDenLop" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbDenLop">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">

        </Script>
    </telerik:RadCodeBlock>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">Chuyển lớp</span>
            </div>
            <div class="col-sm-8 text-right">
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="row item-data">
            <div class="col-sm-5">
                <div class="row" style="margin-bottom: 15px">
                    <div class="col-sm-12">
                        <label for="tbTen" class="col-sm-5 control-label">Từ lớp</label>
                        <div class="col-sm-7">
                            <telerik:RadComboBox ID="rcbTuLop" runat="server" Width="100%" MaxHeight="500px" DataSourceID="objTuLop" DataTextField="TEN" DataValueField="ID" AutoPostBack="True" Filter="Contains" OnSelectedIndexChanged="rcbTuLop_SelectedIndexChanged">
                            </telerik:RadComboBox>
                            <asp:ObjectDataSource ID="objTuLop" runat="server" SelectMethod="getLopByKhoiNamHoc" TypeName="OneEduDataAccess.BO.LopBO">
                                <SelectParameters>
                                    <%--<asp:ControlParameter ControlID="rcbKhoi" Name="maKhoi" PropertyName="SelectedValue" />--%>
                                    <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                                    <asp:Parameter DefaultValue="" Name="id_all" Type="Int64" />
                                    <asp:Parameter Name="text_all" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </div>
                    </div>
                </div>
                <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" GroupPanelPosition="Top" OnNeedDataSource="RadGrid1_NeedDataSource"
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
                            <telerik:GridClientSelectColumn UniqueName="chkChon" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            </telerik:GridClientSelectColumn>
                            <telerik:GridTemplateColumn HeaderText="STT" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# Container.DataSetIndex+1 %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="HO_TEN" FilterControlAltText="Filter HO_TEN column" HeaderText="Họ tên" SortExpression="HO_TEN" UniqueName="HO_TEN" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="NGAY_SINH" FilterControlAltText="Filter NGAY_SINH column" HeaderText="Ngày sinh" SortExpression="NGAY_SINH" UniqueName="NGAY_SINH" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-Width="100px">
                            </telerik:GridBoundColumn>
                        </Columns>
                        <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
            <div class="col-sm-2" style="margin-top: 150px; text-align: center">
                <asp:ImageButton ID="btnChuyenDi" runat="server" ImageUrl="~/img/arrow_right.jpg" style="width:70px;" OnClick="btnChuyenDi_Click"/><br />
                <asp:ImageButton ID="btnChuyenDen" runat="server"  ImageUrl="~/img/arrow_left.jpg" style="width:53px;" OnClick="btnChuyenDen_Click"/>
            </div>           
            <div class="col-sm-5">
                <div class="row" style="margin-bottom: 15px">
                    <div class="col-sm-12">
                        <label for="tbTen" class="col-sm-5 control-label">Đến lớp</label>
                        <div class="col-sm-7">
                            <telerik:RadComboBox ID="rcbDenLop" runat="server" Width="100%" MaxHeight="500px" DataSourceID="objDenLop" DataTextField="TEN" DataValueField="ID" Filter="Contains" OnSelectedIndexChanged="rcbDenLop_SelectedIndexChanged" AutoPostBack="true">
                            </telerik:RadComboBox>
                            <asp:ObjectDataSource ID="objDenLop" runat="server" SelectMethod="getLopByLopKhoiNamHocNotIdLop" TypeName="OneEduDataAccess.BO.LopBO">
                                <SelectParameters>
                                    <%--<asp:ControlParameter ControlID="rcbKhoi" Name="maKhoi" PropertyName="SelectedValue" />--%>
                                    <asp:ControlParameter ControlID="rcbTuLop" Name="id_lop" PropertyName="SelectedValue" />
                                    <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                                    <asp:Parameter DefaultValue="" Name="id_all" Type="Int64" />
                                    <asp:Parameter Name="text_all" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </div>
                    </div>
                </div>
                <telerik:RadGrid ID="RadGrid2" runat="server" AllowPaging="True" GroupPanelPosition="Top" OnNeedDataSource="RadGrid2_NeedDataSource"
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
                            <telerik:GridClientSelectColumn UniqueName="chkChon" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            </telerik:GridClientSelectColumn>
                            <telerik:GridTemplateColumn HeaderText="STT" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# Container.DataSetIndex+1 %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="HO_TEN" FilterControlAltText="Filter HO_TEN column" HeaderText="Họ tên" SortExpression="HO_TEN" UniqueName="HO_TEN" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="NGAY_SINH" FilterControlAltText="Filter NGAY_SINH column" HeaderText="Ngày sinh" SortExpression="NGAY_SINH" UniqueName="NGAY_SINH" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-Width="100px">
                            </telerik:GridBoundColumn>
                        </Columns>
                        <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </div>
    </div>
</asp:Content>
