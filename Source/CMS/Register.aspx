<%@ Page Title="" Language="C#" MasterPageFile="~/LoginMaster.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="CMS.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .lable{
            text-align: left !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid text-center" style="width: 444px; margin-top: 40px">
        <h1><span>ĐĂNG KÝ</span></h1>
        <div class="row" style="margin-top: 20px">
            <div class="col-sm-12">
                <div class="row">
                    <div class="col-sm-4 lable"><label class="control-label">Họ và tên:</label></div>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtHoTen" CssClass="form-control" runat="server" placeholder="Nhập họ và tên"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtHoTen"
                    CssClass="valid-control" ForeColor="Red" ErrorMessage="Bạn phải nhập họ tên." Display="Dynamic"
                    SetFocusOnError="true" InitialValue=""> </asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <div class="row">
                    <div class="col-sm-4 lable"><label class="control-label">Tên đăng nhập:</label></div>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtUser" CssClass="form-control" runat="server" placeholder="Nhập tên đăng nhập"></asp:TextBox>
                         <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtUser"
                    CssClass="valid-control" ForeColor="Red" ErrorMessage="Bạn phải nhập họ tên." Display="Dynamic"
                    SetFocusOnError="true" InitialValue=""> </asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <div class="row">
                    <div class="col-sm-4 lable"><label class="control-label">Mật khẩu:</label></div>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtPass" TextMode="Password" CssClass="form-control" runat="server" placeholder="Nhập mật khẩu"></asp:TextBox>
                         <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtPass"
                    CssClass="valid-control" ForeColor="Red" ErrorMessage="Bạn phải nhập họ tên." Display="Dynamic"
                    SetFocusOnError="true" InitialValue=""> </asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <div class="row">
                    <div class="col-sm-4 lable"><label class="control-label">Số điện thoại:</label></div>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtSDT" CssClass="form-control" runat="server" placeholder="Nhập số điện thoại"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtSDT"
                    CssClass="valid-control" ForeColor="Red" ErrorMessage="Bạn phải nhập số điện thoại." Display="Dynamic"
                    SetFocusOnError="true" InitialValue=""> </asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <div class="row">
                    <div class="col-sm-4 lable"><label class="control-label">Email:</label></div>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server" placeholder="Nhập email"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        
        <div class="row">
            <div class="col-sm-12">
                <div class="row">
                    <div class="col-sm-4 lable"><label class="control-label">Chọn vai trò:</label></div>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddlVaiTro" runat="server" CssClass="form-control">
                            <asp:ListItem Value="0">Chọn vai trò</asp:ListItem>
                            <asp:ListItem Value="1">Quản lý trường</asp:ListItem>
                            <asp:ListItem Value="2">Giáo viên</asp:ListItem>
                            <asp:ListItem Value="3">Cộng tác viên</asp:ListItem>
                            <asp:ListItem Value="4">Phụ huynh</asp:ListItem>
                        </asp:DropDownList>
                        <%--<select class="form-control" id="ddlVaiTro">
                          <option value="0">Chọn vai trò</option>
                          <option value="1">Quản lý trường</option>
                          <option value="2">Giáo viên</option>
                          <option value="3">Cộng tác viên</option>
                          <option value="4">Phụ huynh</option>
                        </select>--%>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <asp:Label ID="lblMess" runat="server" Text="" Style="color: red;"></asp:Label>
        </div>
        <div class="row" style="margin-top: 20px">
            <div class="col-sm-12">
                <asp:Button ID="btnDangKy" runat="server" Text="Đăng ký" CssClass="btn bt-one" Width="100%" OnClick="btnDangKy_Click" />
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <label>Bạn đã có tài khoản? <a class="color-one" href="login.aspx">Đăng nhập</a></label>
            </div>
        </div>
    </div>
</asp:Content>
