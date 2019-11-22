<%@ Page Title="" Language="C#" MasterPageFile="~/LoginMaster.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CMS.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        var delete_cookie = function (name) {
            document.cookie = name + '=;expires=Thu, 01 Jan 1970 00:00:01 GMT;';
        };
        delete_cookie('MyCookieOnedu');

    </script>
    <style>
        ::-webkit-input-placeholder {
            text-align: center;
        }

        :-moz-placeholder { /* Firefox 18- */
            text-align: center;
        }

        ::-moz-placeholder { /* Firefox 19+ */
            text-align: center;
        }

        :-ms-input-placeholder {
            text-align: center;
        }

        .btn {
            flex: 1 1 auto;
            transition: 0.5s;
            background-size: 200% auto;
            color: white;
            box-shadow: 0 0 20px #eee;
        }

            .btn:hover {
                background-position: right center;
                background-image: linear-gradient(to right, #587fd2 0%, #0059ce 51%, #01579b 100%);
            }

        .bt-one-google:hover {
            background-position: right center;
            background-image: linear-gradient(to right, #c12800 0%, #c1401e 51%, #9b2101 100%);
        }

        #top-login #imLogo {
            width: 444px;
            margin-top: 40px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid text-center" style="width: 444px; margin-top: 80px">
        <h1><span>ĐĂNG NHẬP SỔ LIÊN LẠC ĐIỆN TỬ EDU SMS</span></h1>
        <div class="row" style="margin-top: 20px">
            <div class="col-sm-12">
                <asp:TextBox ID="tbUserName" CssClass="form-control" runat="server" placeholder="Nhập tên tài khoản đăng nhập"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbUserName"
                    CssClass="valid-control" ForeColor="Red" ErrorMessage="Thông tin bắt buộc nhập." Display="Dynamic"
                    SetFocusOnError="true" InitialValue=""> </asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <asp:TextBox ID="tbMatKhau" CssClass="form-control" runat="server" placeholder="Nhập mật khẩu đăng nhập" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbMatKhau"
                    CssClass="valid-control" ForeColor="Red" ErrorMessage="Thông tin bắt buộc nhập." Display="Dynamic"
                    SetFocusOnError="true" InitialValue=""> </asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <asp:Button ID="btDangNhap" runat="server" Text="Đăng nhập" CssClass="btn bt-one" Width="100%" OnClick="btDangNhap_Click" />
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <label><a class="color-one" href="#">Quên mật khẩu?</a></label>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <asp:Button ID="btGoogle" runat="server" Text="Đăng nhập bằng Google" CssClass="btn bt-one-google" Width="100%" CausesValidation="false" OnClick="btGoogle_Click" />
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <asp:Button ID="btFacebook" runat="server" Text="Đăng nhập bằng Facebook" CssClass="btn bt-one" Width="100%" CausesValidation="false" OnClick="btFacebook_Click" />
            </div>
        </div>
    </div>
</asp:Content>
