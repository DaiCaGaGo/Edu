<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HuongDanDangKyZalo.aspx.cs" Inherits="CMS.Report.HuongDanDangKyZalo" %>
<%@ Register Assembly="DevExpress.XtraReports.v17.1.Web, Version=17.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title> 
</head>
<body>
    <form id="form1" runat="server">
        <%--<div class="row" runat="server">
             <div class="col-sm-9 text-right">
                <asp:Button ID="btIn" runat="server" Text="In" CssClass="btn bt-one" OnClick="btIn_Click" />
            </div>
        </div>--%>
        <dx:ASPxWebDocumentViewer ID="documentViewer" runat="server"></dx:ASPxWebDocumentViewer>
    </form>
</body>
</html>
