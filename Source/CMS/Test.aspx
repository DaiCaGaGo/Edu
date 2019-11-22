<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="CMS.Test" Async="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width,initial-scale=1,user-scalable=no" />
    <script src="Scripts/jquery-3.0.0.min.js"></script>
    <%--<script src="Scripts/qrcode.js"></script>--%>
    <%--<script src="https://sp.zalo.me/plugins/sdk.js"></script>--%>
</head>
<body>


    <form id="form1" runat="server" class="form-horizontal" role="form">
        <%--<input id="text" type="text" value="http://jindo.dev.naver.com/collie" style="width: 80%" /><br />
        <div id="qrcode" style="width: 100px; height: 100px; margin-top: 15px;"></div>
        <div class="zalo-follow-button" data-oaid="2687558107438251665" data-cover="no" data-article="3" data-width="500" data-height="628"></div>--%>

        <%--<script type="text/javascript">
            var qrcode = new QRCode(document.getElementById("qrcode"), {
                width: 100,
                height: 100
            });

            function makeCode() {
                var elText = document.getElementById("text");

                if (!elText.value) {
                    alert("Input a text");
                    elText.focus();
                    return;
                }

                qrcode.makeCode(elText.value);
            }

            makeCode();

            $("#text").
                on("blur", function () {
                    makeCode();
                }).
                on("keydown", function (e) {
                    if (e.keyCode == 13) {
                        makeCode();
                    }
                });
</script>--%>
        <asp:Label ID="lblText" runat="server" Text="Label"></asp:Label>
    </form>
</body>
</html>
