<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="NguoiDungDonViDetail.aspx.cs" Inherits="CMS.QuanTri.NguoiDungDonViDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rcbNhomQuyen">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbNhomQuyen" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        <Windows>
            <telerik:RadWindow RenderMode="Lightweight" ID="RadWindow1" runat="server" ShowContentDuringLoad="false" Width="400px"
                Height="400px" Title="Chi người dùng" Behaviors="Reload,Close,Maximize" Modal="true" VisibleStatusbar="false">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">QUẢN LÝ NGƯỜI DÙNG</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btAdd" runat="server" CssClass="btn bt-one" OnClick="btAdd_Click" Text="Thêm" />
                <asp:Button ID="btEdit" runat="server" CssClass="btn bt-one" OnClick="btEdit_Click" Text="Cập nhật" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="form-horizontal" role="form">
                <div class="row">
                    <div class="col-sm-4">
                        <div class="form-group">
                            <label for="txtUser" class="col-sm-5 control-label">Tên đăng nhập <span style="color: red">(*)</span></label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtUser" CssClass="form-control" ClientIDMode="Static" placeholder="Tên đăng nhập" runat="server"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator2" ControlToValidate="txtUser" ClientValidationFunction="validateMa" ValidateEmptyText="true"
                                    runat="server" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Thông tin bắt buộc, nhập không quá 100 ký tự." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="txtPass" class="col-sm-5 control-label">Mật khẩu <span style="color: red">(*)</span></label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtPass" CssClass="form-control" ClientIDMode="Static" placeholder="Mật khẩu" runat="server" TextMode="Password"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator3" ControlToValidate="txtPass" ClientValidationFunction="validateMa" ValidateEmptyText="true"
                                    runat="server" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Thông tin bắt buộc, nhập không quá 100 ký tự." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="txtName" class="col-sm-5 control-label">Tên hiển thị</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtName" CssClass="form-control" ClientIDMode="Static" placeholder="Tên hiển thị" runat="server"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator4" ControlToValidate="txtName" ClientValidationFunction="validateMaxchar" ValidateEmptyText="false"
                                    runat="server" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Nhập không quá 100 ký tự." />
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="form-group">
                            <label for="rcbNhomQuyen" class="col-sm-5 control-label">Nhóm quyền <span style="color: red">(*)</span></label>
                            <div class="col-sm-7">
                                <telerik:RadComboBox ID="rcbNhomQuyen" runat="server" Width="100%" DataSourceID="objNhomQuyen"
                                    DataTextField="TEN" DataValueField="MA" Filter="Contains">
                                </telerik:RadComboBox>
                                <asp:ObjectDataSource ID="objNhomQuyen" runat="server" SelectMethod="getNhomQuyen" TypeName="OneEduDataAccess.BO.NhomQuyenBO">
                                    <SelectParameters>
                                        <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                                        <asp:Parameter DefaultValue="" Name="ma_all" Type="String" />
                                        <asp:Parameter Name="text_all" Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="txtSDT" class="col-sm-5 control-label">Số điện thoại <span style="color: red">(*)</span></label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtSDT" CssClass="form-control" ClientIDMode="Static" placeholder="Số điện thoại" runat="server"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator5" ControlToValidate="txtSDT" ClientValidationFunction="validateDienThoai" ValidateEmptyText="true" runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Dữ liệu không hợp lệ." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="txtAddress" class="col-sm-5 control-label">Địa chỉ</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtAddress" CssClass="form-control" ClientIDMode="Static" placeholder="Địa chỉ" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="form-group">
                            <label for="txtMail" class="col-sm-4 control-label">Email</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtMail" CssClass="form-control" ClientIDMode="Static" placeholder="Email" runat="server"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator1" ControlToValidate="txtMail" ClientValidationFunction="validateEmails" ValidateEmptyText="false"
                                    runat="server" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Địa chỉ Email không hợp lệ." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="txtFb" class="col-sm-4 control-label">Facebook</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtFb" CssClass="form-control" ClientIDMode="Static" placeholder="Facebook" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
