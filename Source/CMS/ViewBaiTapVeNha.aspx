<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewBaiTapVeNha.aspx.cs" Inherits="CMS.ViewBaiTapVeNha" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="css/styleStudentInfo.css">
    <link rel="shortcut icon" href="<%= "http://"+HttpContext.Current.Request.Url.Authority+"/img/logo.ico"  %>" />
</head>
<body>

    <section class="cpn-info">
        <h3><span><asp:Label ID="lblBTVN" runat="server"></asp:Label></span></h3>
    </section>
    <asp:Label ID="lblMes" runat="server" Text="" Style="color: red; margin-top: 15px;"></asp:Label>
    <br />
    <br />
    <div class="cpn-table-response" style="width:100%;margin:0px auto;">
        <table border="1" cellpadding="0" cellspacing="0" dir="ltr" style="min-width:0px;">
            <tbody>
                <tr>
                    <td>Tên sách</td>
                    <td>Bài tập</td>
                    <td>Ảnh</td>
                </tr>
                <%for (int i = 0; i < lstBTVN.Count; i++)
                    { %>
                <tr>
                    <td class="cpn-table-left-content-left"><%= lstBTVN[i].TEN_SACH%></td>
                    <td class="cpn-table-left-content-left"><%= lstBTVN[i].BAI_TAP_CHI_TIET== null ? "&nbsp;":lstBTVN[i].BAI_TAP_CHI_TIET.ToString()%></td>
                    <td class="cpn-table-left-content-center">
                        <img src="<%= lstBTVN[i].ICON.Replace("~/", "")%>" alt="img" class="input-image">
                    </td>
                </tr>
                <%} %>
            </tbody>
        </table>
    </div>
</body>
</html>
