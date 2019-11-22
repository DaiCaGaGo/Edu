<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="HoSo.aspx.cs" Inherits="CMS.Profile.HoSo" %>

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
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label for="tbTenHienThi" class="col-sm-3 control-label">Tên hiển thị</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbTenHienThi" CssClass="form-control" ClientIDMode="Static" placeholder="Nhập tên hiển thị" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbEmail" class="col-sm-3 control-label">Email</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbEmail" CssClass="form-control" ClientIDMode="Static" placeholder="Nhập Email" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="checkbox col-sm-9 col-sm-offset-3">
                                <label>
                                    <asp:CheckBox ID="cbLoginGmail" runat="server" Text="Đăng nhập bằng Gmail" />
                                </label>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Facebook</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbFacebook" CssClass="form-control" ClientIDMode="Static" placeholder="Địa chỉ facebook" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="checkbox col-sm-9 col-sm-offset-3">
                                <label>
                                    <asp:CheckBox ID="cbLoginFb" runat="server" Text="Đăng nhập bằng Facebook" />
                                </label>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Nhập mật khẩu <span style="color: red">(*)</span></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbMatKhau" CssClass="form-control" ClientIDMode="Static" TextMode="Password" placeholder="Nhập mật khẩu" runat="server"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator2" ControlToValidate="tbMatKhau" ClientValidationFunction="validateMaxchar"
                                    ValidateEmptyText="true" runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic"
                                    ErrorMessage="Thông tin bắt buộc nhập và không nhập quá 50 ký tự." />
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Số điện thoại <span style="color: red">(*)</span></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbSDT" CssClass="form-control" ClientIDMode="Static" placeholder="Nhập số điện thoại" runat="server"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator7" ControlToValidate="tbSDT" ClientValidationFunction="validateDienThoai"
                                    ValidateEmptyText="true" runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic"
                                    ErrorMessage="Thông tin số điện thoại đang để trống hoặc sai định dạng." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Địa chỉ</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbDiaChi" CssClass="form-control" ClientIDMode="Static" placeholder="Nhập địa chỉ" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbGhiChu" class="col-sm-3 control-label">Ghi chú</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbGhiChu" CssClass="form-control" ClientIDMode="Static" placeholder="Nhập Ghi chú" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbAnhDaiDien" class="col-sm-3 control-label">Ảnh đại diện</label>
                            <div class="col-sm-9">
                                <asp:Image ID="imgAnh" runat="server" Width="100px" Height="100px" Visible="false" />
                                <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" CssClass="async-attachment" ID="rauAnh"
                                    HideFileInput="true" AllowedFileExtensions=".jpeg,.jpg,.png" MaxFileInputsCount="1" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
