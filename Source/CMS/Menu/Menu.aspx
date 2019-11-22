<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterSYS.Master" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="CMS.Menu.Menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadTreeList1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadTreeList1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbCapHoc">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadTreeList1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">Quản lý menu</span>
            </div>
            <div class="col-sm-8 text-right">
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="row">
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbCapHoc" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="rcbCapHoc_SelectedIndexChanged">
                        <Items>
                            <telerik:RadComboBoxItem Value="" Text="Hệ thống" />
                            <telerik:RadComboBoxItem Value="MN" Text="Mầm non" />
                            <telerik:RadComboBoxItem Value="TH" Text="Tiểu học" />
                            <telerik:RadComboBoxItem Value="THCS" Text="THCS" />
                            <telerik:RadComboBoxItem Value="THPT" Text="THPT" />
                            <telerik:RadComboBoxItem Value="GDTX" Text="GDTX" />
                        </Items>
                    </telerik:RadComboBox>
                </div>
            </div>
        </div>
        <div class="item-data">
            <telerik:RadTreeList runat="server" ID="RadTreeList1" Width="100%"
                AllowPaging="True" PageSize="50" DataKeyNames="ID" ParentDataKeyNames="ID_CHA"
                OnNeedDataSource="RadTreeList1_NeedDataSource" OnUpdateCommand="RadTreeList1_UpdateCommand" OnInsertCommand="RadTreeList1_InsertCommand"
                OnDeleteCommand="RadTreeList1_DeleteCommand" AutoGenerateColumns="false" NoRecordsText="Chưa có dữ liệu..." OnItemDataBound="RadTreeList1_ItemDataBound" OnEditCommand="RadTreeList1_EditCommand">
                <HeaderStyle CssClass="head-list-grid" />
                <Columns>
                    <telerik:TreeListEditCommandColumn UniqueName="InsertCommandColumn" ButtonType="ImageButton" ShowEditButton="false" HeaderStyle-Width="45px" ItemStyle-HorizontalAlign="Center"></telerik:TreeListEditCommandColumn>
                    <telerik:TreeListButtonColumn CommandName="Edit" Text="Edit" UniqueName="EditCommandColumn" ButtonType="ImageButton" HeaderStyle-Width="45px" ItemStyle-HorizontalAlign="Center"></telerik:TreeListButtonColumn>
                    <telerik:TreeListButtonColumn UniqueName="DeleteCommandColumn" Text="Delete" CommandName="Delete" ButtonType="ImageButton" ConfirmText="Bạn thật sự muốn xóa bản ghi này?" HeaderStyle-Width="45px"></telerik:TreeListButtonColumn>
                    <telerik:TreeListBoundColumn DataField="TEN" HeaderText="Tiêu đề" HeaderStyle-HorizontalAlign="Center" DataType="System.String" UniqueName="TEN"></telerik:TreeListBoundColumn>
                    <telerik:TreeListBoundColumn DataField="TEN_EG" HeaderText="Tiêu đề tiếng Anh" HeaderStyle-HorizontalAlign="Center" DataType="System.String" UniqueName="TEN_EG"></telerik:TreeListBoundColumn>
                    <telerik:TreeListBoundColumn DataField="URL" HeaderText="Đường dẫn" HeaderStyle-HorizontalAlign="Center" DataType="System.String" UniqueName="URL"></telerik:TreeListBoundColumn>
                    <telerik:TreeListBoundColumn DataField="THU_TU" HeaderText="Thứ tự" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="75px" UniqueName="THU_TU"></telerik:TreeListBoundColumn>
                    <telerik:TreeListBoundColumn DataField="ICON_CSS_CLASS" HeaderText="Css class icon" HeaderStyle-HorizontalAlign="Center" DataType="System.String" UniqueName="ICON_CSS_CLASS"></telerik:TreeListBoundColumn>
                    <telerik:TreeListTemplateColumn DataField="ICON" UniqueName="ICON" HeaderText="Ảnh" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="110px" ItemStyle-CssClass="main-menu">
                        <ItemTemplate>
                            <asp:Image ID="imgAnh" runat="server" ImageUrl='<%# Eval("ICON")==DBNull.Value?"": Eval("ICON") %>' Width="100px" Visible='<%# Eval("ICON")!=DBNull.Value && Eval("ICON")!=null && !string.IsNullOrEmpty(Eval("ICON").ToString()) %>' />
                        </ItemTemplate>
                    </telerik:TreeListTemplateColumn>
                    <telerik:TreeListTemplateColumn HeaderText="Hiệu lực" UniqueName="TRANG_THAI" DataField="TRANG_THAI" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:CheckBox ID="cbTrangThai" runat="server" Checked='<%# Eval("TRANG_THAI") == DBNull.Value ? false : Convert.ToInt32(Eval("TRANG_THAI"))==1 %>' Enabled="false" />
                        </ItemTemplate>
                    </telerik:TreeListTemplateColumn>
                    <telerik:TreeListCheckBoxColumn DataField="IS_HIEN_THI" HeaderText="Hiển thị" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" UniqueName="IS_HIEN_THI" ItemStyle-HorizontalAlign="Center"></telerik:TreeListCheckBoxColumn>
                </Columns>
                <EditFormSettings EditFormType="Template">
                    <FormTemplate>
                        <div class="item-data">
                            <div class="container">
                                <div class="form-group row">
                                    <label class="col-sm-2 col-form-label">
                                        Tiêu đề<span style="color: red">(*)</span>
                                    </label>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="tbTen" runat="server" CssClass="form-control" placeholder="Tiêu đề" Text='<%# Eval("TEN") %>' MaxLength="50"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <label class="col-sm-2 col-form-label">
                                        Tiêu đề tiếng anh <span style="color: red">(*)</span>
                                    </label>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="tbTenEg" runat="server" CssClass="form-control" placeholder="Tiêu đề tiếng Anh" Text='<%# Eval("TEN_EG") %>' MaxLength="50"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <label class="col-sm-2 col-form-label">
                                        Đường dẫn
                                    </label>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="tbUrrl" runat="server" CssClass="form-control" placeholder="Đường dẫn" Text='<%# Eval("URL") %>' MaxLength="50"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <label class="col-sm-2 col-form-label">
                                        Thứ tự
                                    </label>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="tbThuTu" runat="server" CssClass="form-control" placeholder="Thứ tự" Text='<%# Eval("THU_TU") %>' MaxLength="20"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <label class="col-sm-2 col-form-label">
                                        Css class icon
                                    </label>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="tbIconClass" runat="server" CssClass="form-control" placeholder="Css class icon" Text='<%# Eval("ICON_CSS_CLASS") %>' MaxLength="50"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <label class="col-sm-2 col-form-label">
                                        Ảnh
                                    </label>
                                    <div class="col-sm-10">
                                        <asp:Image ID="imgAnh" runat="server" ImageUrl='<%# Eval("ICON")==DBNull.Value?"": Eval("ICON") %>' Width="100px" Visible='<%# Eval("ICON")!=DBNull.Value && Eval("ICON")!=null && !string.IsNullOrEmpty(Eval("ICON").ToString()) %>' />
                                        <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" CssClass="async-attachment" ID="rauAnh"
                                            HideFileInput="true"
                                            AllowedFileExtensions=".jpeg,.jpg,.png" />
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <label class="col-sm-2 col-form-label">
                                        Hiệu lực
                                    </label>
                                    <div class="col-sm-10">
                                        <asp:CheckBox ID="cbTrangThai" runat="server" Checked='<%# Eval("TRANG_THAI") == DBNull.Value ||  Eval("TRANG_THAI") ==null ? false :  Eval("TRANG_THAI").ToString()=="1" %>' />
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <label class="col-sm-2 col-form-label">
                                        Hiển thị
                                    </label>
                                    <div class="col-sm-10">
                                        <asp:CheckBox ID="cbHienThi" runat="server" Checked='<%# Eval("IS_HIEN_THI") == DBNull.Value ||  Eval("IS_HIEN_THI") ==null ? false :  Eval("IS_HIEN_THI") %>' />
                                    </div>
                                </div>
                                <div class="form-group row text-center">
                                    <telerik:RadButton ID="btnUpdate" Text='<%# (Container is TreeListEditFormInsertItem) ? "Thêm" : "Sửa" %>'
                                        runat="server" CommandName='<%# (Container is TreeListEditFormInsertItem) ? "PerformInsert" : "Update" %>'
                                        Icon-PrimaryIconCssClass="rbOk">
                                    </telerik:RadButton>
                                    &nbsp;
                                    <telerik:RadButton ID="btnCancel" Text="Hủy" runat="server" CausesValidation="False"
                                        CommandName="Cancel" Icon-PrimaryIconCssClass="rbCancel" CssClass="qi-bt">
                                    </telerik:RadButton>
                                </div>
                            </div>
                        </div>
                    </FormTemplate>
                </EditFormSettings>
                <PagerStyle Mode="Advanced" />
            </telerik:RadTreeList>
        </div>
    </div>
</asp:Content>
