<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ThoiKhoaBieuLop.aspx.cs" Inherits="CMS.ThoiKhoaBieuLop" %>

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
        <h3><span><asp:Label ID="lblLop" runat="server"></asp:Label></span></h3>
    </section>
    <br />
    <div class="cpn-table-response" style="width:100%;margin:0px auto;">
        <table border="1" cellpadding="0" cellspacing="0" dir="ltr" style="min-width:0px;">
            <tbody>
                <tr>
                    <td>Tiết</td>
                    <td>Thứ 2</td>
                    <td>Thứ 3</td>
                    <td>Thứ 4</td>
                    <td>Thứ 5</td>
                    <td>Thứ 6</td>
                    <td>Thứ 7</td>
                    <td>Chủ nhật</td>
                </tr>
                <%for (int i = 0; i < listTKB.Count; i++)
                    { %>
                <tr>
                    <td><%= listTKB[i].TIET%></td>
                    <td><%= listTKB[i].TEN_MON_2== null ? "&nbsp;":listTKB[i].TEN_MON_2.ToString()%></td>
                    <td><%= listTKB[i].TEN_MON_3== null ? "&nbsp;":listTKB[i].TEN_MON_3.ToString()%></td>
                    <td><%= listTKB[i].TEN_MON_4== null ? "&nbsp;":listTKB[i].TEN_MON_4.ToString()%></td>
                    <td><%= listTKB[i].TEN_MON_5== null ? "&nbsp;":listTKB[i].TEN_MON_5.ToString()%></td>
                    <td><%= listTKB[i].TEN_MON_6== null ? "&nbsp;":listTKB[i].TEN_MON_6.ToString()%></td>
                    <td><%= listTKB[i].TEN_MON_7== null ? "&nbsp;":listTKB[i].TEN_MON_7.ToString()%></td>
                    <td><%= listTKB[i].TEN_MON_8== null ? "&nbsp;":listTKB[i].TEN_MON_8.ToString()%></td>
                </tr>
                <%} %>
            </tbody>
        </table>
    </div>
</body>
</html>
