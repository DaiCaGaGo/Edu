<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QRCodeScan.aspx.cs" Inherits="CMS.QRCodeScan" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <script src="Scripts/jquery-3.0.0.min.js"></script>
    <script src="Scripts/instascan.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <video id="preview" style="max-width: 500px; width: 100%"></video>

            <script type="text/javascript">

                let scanner = new Instascan.Scanner({ video: document.getElementById('preview') });

                scanner.addListener('scan', function (content) {
                    scanner.stop();
                    closeAndPassData(content);

                });

                Instascan.Camera.getCameras().then(function (cameras) {

                    if (cameras.length > 0) {

                        scanner.start(cameras[0]);

                    } else {

                        console.error('No cameras found.');

                    }

                }).catch(function (e) {

                    console.error(e);

                });
                function closeAndPassData(data) {
                    var wnd = GetRadWindow();
                    if (wnd) {
                        wnd.close(data);
                    }
                }
                function GetRadWindow() {
                    var oWindow = null;
                    if (window.radWindow) oWindow = window.radWindow;
                    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                    return oWindow;
                }
            </script>
        </div>
    </form>
</body>
</html>
