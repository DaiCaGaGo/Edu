<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="LenLop.aspx.cs" Inherits="CMS.HocSinh.LenLop" %>
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
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" />
                </UpdatedControls>
            </telerik:AjaxSetting> 
            <telerik:AjaxSetting AjaxControlID="rcbNamHoc1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbLop1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="rcbLop2" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="rcbNamHoc2" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbNamHoc2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbLop2" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbLop1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbLop2">
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
                <span class="item-title">Xét học sinh lên lớp</span>
            </div>
            <div class="col-sm-8 text-right">
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="form-horizontal" role="form">
                <div class="row">
                    <div class="col-sm-2">
                        <telerik:RadComboBox ID="rcbNamHoc1" runat="server" Width="100%" DataSourceID="objNamHoc" DataValueField="MA" DataTextField="TEN" Filter="Contains" OnSelectedIndexChanged="rcbNamHoc1_SelectedIndexChanged" AutoPostBack="true">
                        </telerik:RadComboBox>
                        <asp:ObjectDataSource ID="objNamHoc" runat="server" SelectMethod="getNamHoc" TypeName="OneEduDataAccess.BO.NamHocBO">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="false" Name="is_all" Type="Boolean" />
                                <asp:Parameter DefaultValue="" Name="id_all" Type="Int16" />
                                <asp:Parameter Name="text_all" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </div>
                    <div class="col-sm-2">
                        <telerik:RadComboBox ID="rcbLop1" runat="server" Width="100%" MaxHeight="500px" DataSourceID="objLop1" DataTextField="TEN" DataValueField="ID" Filter="Contains" OnSelectedIndexChanged="rcbLop1_SelectedIndexChanged" AutoPostBack="true">
                        </telerik:RadComboBox>
                        <asp:ObjectDataSource ID="objLop1" runat="server" SelectMethod="getLopByTruongCapNamHoc" TypeName="OneEduDataAccess.BO.LopBO">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="rcbNamHoc1" Name="id_nam_hoc" PropertyName="SelectedValue" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </div>
                    <div class="col-sm-2">
                        <div class="one-checkbox">
                            <label>
                                <asp:CheckBox ID="cbLenLopAll" runat="server" Text="Chọn tất cả" Checked="true"/>
                            </label>
                        </div>
                    </div>
                    <div class="col-sm-2" style="color:red; font-weight: bold;">Lên lớp</div>
                    <div class="col-sm-2">
                        <telerik:RadComboBox ID="rcbNamHoc2" runat="server" Width="100%" DataSourceID="objNamHoc2" DataValueField="MA" DataTextField="TEN" Filter="Contains" OnSelectedIndexChanged="rcbNamHoc2_SelectedIndexChanged" AutoPostBack="true">
                        </telerik:RadComboBox>
                        <asp:ObjectDataSource ID="objNamHoc2" runat="server" SelectMethod="getNamHocNotNamHocID" TypeName="OneEduDataAccess.BO.NamHocBO">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="rcbNamHoc1" Name="id_nam_hoc" PropertyName="SelectedValue" />
                                <asp:Parameter DefaultValue="false" Name="is_all" Type="Boolean" />
                                <asp:Parameter DefaultValue="" Name="id_all" Type="Int16" />
                                <asp:Parameter Name="text_all" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </div>
                    <div class="col-sm-2">
                        <telerik:RadComboBox ID="rcbLop2" runat="server" Width="100%" MaxHeight="500px" DataSourceID="objLop2" DataTextField="TEN" DataValueField="ID" Filter="Contains" OnSelectedIndexChanged="rcbLop2_SelectedIndexChanged" AutoPostBack="true">
                        </telerik:RadComboBox>
                        <asp:ObjectDataSource ID="objLop2" runat="server" SelectMethod="getLopByTruongCapNamHoc" TypeName="OneEduDataAccess.BO.LopBO">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="rcbNamHoc2" Name="id_nam_hoc" PropertyName="SelectedValue" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </div>
                </div>
            </div>
        </div>
        <div class="row item-data">
            <div class="col-sm-5">
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
                <asp:ImageButton ID="btnChuyenDi" runat="server" ImageUrl="~/img/arrow_right.jpg" style="width:70px;" ToolTip="Lên lớp" OnClick="btnChuyenDi_Click"/><br />
                <asp:ImageButton ID="btnXoa" runat="server"  ImageUrl="~/img/delete.png" style="width:53px;" ToolTip="Xóa học sinh lớp mới"  OnClick="btnXoa_Click"/>
            </div>
            
            <div class="col-sm-5">
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