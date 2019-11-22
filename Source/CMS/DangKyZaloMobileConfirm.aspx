<%@ Page Title="" Language="C#" MasterPageFile="~/LoginMaster.Master" AutoEventWireup="true" CodeBehind="DangKyZaloMobileConfirm.aspx.cs" Inherits="CMS.DangKyZaloMobileConfirm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <style>
        #ContentPlaceHolder1_divDangKyZalo {
            width: 444px;
            margin-top: 70px;
        }

        #top-login #imLogo {
            width: 444px;
            margin-top: 40px;
        }

       @media screen and (min-width: 320px) and (max-width: 480px) {
            body {
                width: 100%;
            }

            #top-login #imLogo {
                width: 100%;
            }

            #ContentPlaceHolder1_divDangKyZalo {
                width: 100%;
            }

            .footer {
                position: unset;
                margin-top: 210px;
            }

                .footer h1 {
                    font-size: 10pt;
                }
        }

        @media screen and (min-width: 600px) and (max-width: 768px) {
            body {
                width: 100%;
            }
        }

        @media only screen (max-width: 320px) {
            body {
                width: 100%;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid text-center" runat="server" id="divDangKyZalo">
        <h1><span>XÁC NHẬN ĐĂNG KÝ ZALO</span></h1>
        <div class="row" style="margin-top: 20px">
            <div class="col-sm-12">
                <asp:TextBox ID="tbMaXacNhan" CssClass="form-control" runat="server" placeholder="Nhập mã xác nhận"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbMaXacNhan"
                    CssClass="valid-control" ForeColor="Red" ErrorMessage="Thông tin bắt buộc nhập." Display="Dynamic"
                    SetFocusOnError="true" InitialValue=""> </asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row" runat="server">
            <div class="col-sm-12">
                <asp:Button ID="btXacNhan" runat="server" Text="Xác nhận" CssClass="btn bt-one" Width="100%" OnClick="btXacNhan_Click" />
            </div>
        </div>
        <div class="row" runat="server">
            <div class="col-sm-12">
                <span style="color: red;">
                    <asp:Label runat="server" ID="lblThongBao" style="font-size:20px;"></asp:Label>
                </span>
            </div>
        </div>
    </div>
</asp:Content>
