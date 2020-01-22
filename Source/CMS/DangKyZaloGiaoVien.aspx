<%@ Page Title="" Language="C#" MasterPageFile="~/LoginMaster.Master" AutoEventWireup="true" CodeBehind="DangKyZaloGiaoVien.aspx.cs" Inherits="CMS.DangKyZaloGiaoVien" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <%--<script src="Scripts/instascan.min.js"></script>--%>
    <script src="Scripts/qr_packed.js"></script>
    <script type="text/javascript">
        function openQRCamera(node) {
            var reader = new FileReader();
            reader.onload = function () {
                node.value = "";
                qrcode.callback = function (res) {
                    if (res instanceof Error) {
                        alert("Mã QR chưa chính xác, vui lòng kiểm tra lại!");
                    } else {
                        node.parentNode.previousElementSibling.value = res;
                    }
                };
                qrcode.decode(reader.result);
            };
            reader.readAsDataURL(node.files[0]);
        }
        function btnXacNhan() {
            if (confirm("Bạn chắc chắn muốn xác nhận con?")) {
                return true;
            }
        }
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

        #ContentPlaceHolder1_divDangKyZalo {
            width: 444px;
            margin-top: 40px;
        }

        #top-login #imLogo {
            width: 444px;
            margin-top: 40px;
        }

        .btn-scan-qr {
            background-color: red;
            margin-left: 10px;
            padding: 3px 6px;
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

    <asp:HiddenField ID="HduserID" ClientIDMode="Static" runat="server" />
    <div class="container-fluid text-center" runat="server" id="divDangKyZalo">
        <h1><span>ĐĂNG KÝ ONEDU TRÊN ZALO</span>
        </h1>
        <div class="row" style="margin-top: 20px">
            <div class="col-sm-12">
                <asp:TextBox ID="tbSDTMap" CssClass="form-control" runat="server" placeholder="Nhập số điện thoại đăng ký" TextMode="Number"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbSDTMap"
                    CssClass="valid-control" ForeColor="Red" ErrorMessage="Thông tin bắt buộc nhập." Display="Dynamic"
                    SetFocusOnError="true" InitialValue=""> </asp:RequiredFieldValidator>
                <asp:CustomValidator ID="CustomValidator2" ControlToValidate="tbSDTMap" ClientValidationFunction="validateDienThoai" ValidateEmptyText="false" runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Dữ liệu không hợp lệ." />
            </div>
        </div>
        <div class="row" style="margin-top: 0px">
            <div class="col-sm-12">
                <asp:TextBox ID="tbSDTNhanSMS" CssClass="form-control" runat="server" placeholder="Nhập SĐT nhận SMS hàng ngày" Style="margin-top: 10px"></asp:TextBox>
                <asp:CustomValidator ID="CustomValidator1" ControlToValidate="tbSDTNhanSMS" ClientValidationFunction="validateDienThoai" ValidateEmptyText="false" runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Dữ liệu không hợp lệ." />
            </div>
        </div>
        <div class="row" style="margin-top: 20px">
            <div class="col-sm-12">
                <asp:Button ID="btnCheck" runat="server" Text="Đăng ký" CssClass="btn bt-one" Width="100%" OnClick="btnCheck_Click" />
            </div>
        </div>
        <div class="row" style="margin-top: 20px">
            <asp:Button ID="btn1" runat="server" Text="" OnClientClick="if(!btnXacNhan()) return false;" OnClick="btn1_Click" Visible="false" />
            <asp:HiddenField ID="hddGV" runat="server" Value="" />
        </div>
        <div class="row" runat="server">
            <div class="col-sm-12">
                <span style="color: red;">
                    <asp:Label runat="server" ID="lblThongBao" Style="font-size: 16px"></asp:Label>
                </span>
            </div>
        </div>
    </div>
    <script src="https://zjs.zdn.vn/zalo/Zalo.Extensions.min.js"></script>
    <script type="text/javascript">
        function getContext() {
            ZaloExtensions.getContext({ "oaId": 3195276698126884179 },
                function success(thread_context) {
                    var obj = new Object();
                    obj = thread_context;
                    var jsonString = JSON.stringify(obj);
                    var parseJson = JSON.parse(jsonString);
                    var userID = parseJson.userId;
                    $('#HduserID').val(userID);
                },
                function error(err) {
                    console.log(err.message);
                });
            //$('#HduserID').val("336483556759542047");
        }
        getContext();
    </script>
</asp:Content>
