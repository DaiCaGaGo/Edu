<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="CMS.Profile.ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        <Windows>
            <telerik:RadWindow RenderMode="Lightweight" ID="RadWindow1" runat="server" ShowContentDuringLoad="false" Width="400px"
                Height="400px" Title="Thông tin cá nhân" Behaviors="Reload,Close,Maximize" Modal="true" VisibleStatusbar="false">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadCodeBlock runat="server">
        <script>
            function btClick() {
                if (confirm("Bạn chắc chắn muốn cập nhật thông tin?")) {
                    return true;
                }
            }
        </script>
    </telerik:RadCodeBlock>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">Hồ sơ cá nhân</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btSave" runat="server" CssClass="btn bt-one" OnClick="btSave_Click" Text="Lưu" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="form-horizontal" role="form">
                <div class="row">
                    <div class="col-sm-6 col-sm-offset-3">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Nhập mật khẩu <span style="color: red">(*)</span></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbMatKhau" CssClass="form-control" ClientIDMode="Static" TextMode="Password" placeholder="Nhập mật khẩu" runat="server"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator2" ControlToValidate="tbMatKhau" ClientValidationFunction="validateMaxchar"
                                    ValidateEmptyText="true" runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic"
                                    ErrorMessage="Thông tin bắt buộc nhập và không nhập quá 50 ký tự." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">
                                Mật khẩu mới 
                            <span style="color: red">(*)</span>
                            </label>
                            <div class="col-sm-9">
                                <asp:TextBox MaxLength="50" ID="tbMatKhauMoi" runat="server" CssClass="form-control" placeholder="Mật khẩu mới" TextMode="Password"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator1" ControlToValidate="tbMatKhauMoi" ClientValidationFunction="validateMaxchar"
                                    ValidateEmptyText="true" runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic"
                                    ErrorMessage="Thông tin bắt buộc nhập và không nhập quá 50 ký tự." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">
                                Nhập lại mật khẩu mới
                            <span style="color: red">(*)</span>
                            </label>
                            <div class="col-sm-9">
                                <asp:TextBox MaxLength="50" ID="tbMatKhauMoiNhapLai" runat="server" CssClass="form-control" placeholder="Nhập lại mật khẩu mới" TextMode="Password"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator3" ControlToValidate="tbMatKhauMoiNhapLai" ClientValidationFunction="validateMaxchar"
                                    ValidateEmptyText="true" runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic"
                                    ErrorMessage="Thông tin bắt buộc nhập và không nhập quá 50 ký tự." />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
