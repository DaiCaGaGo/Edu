<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TraCuuDanhBaPhuHuynh.aspx.cs" Inherits="CMS.TraCuuDanhBaPhuHuynh" %>

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
        <h3><span><asp:Label ID="lblDanhBaPH" runat="server"></asp:Label></span></h3>
    </section>
    <div class="cpn-table-response" style="width:100%;margin:0px auto;">
        <table border="1" cellpadding="0" cellspacing="0" dir="ltr" style="min-width:0px;">
            <tbody>
                <tr>
                    <td>STT</td>
                    <td>Tên phụ huynh</td>
                    <td>Số điện thoại</td>
                    <td>Gọi điện</td>
                </tr>
                <%for (int i = 0; i < lstDanhBaPhuHuynh.Count; i++)
                    { %>
                <tr>
                    <td style="vertical-align: middle; font-weight: normal;"><%= i+1 %></td>
                    <td class="cpn-table-left-content-left"><%= lstDanhBaPhuHuynh[i].TEN_PHU_HUYNH%></td>
                    <td class="cpn-table-left-content-left"><%= lstDanhBaPhuHuynh[i].SDT_NHAN_TIN== null ? "&nbsp;":lstDanhBaPhuHuynh[i].SDT_NHAN_TIN.ToString()%></td>
                    <td class="cpn-table-left-content-center"><a href="tel: <%= lstDanhBaPhuHuynh[i].SDT_NHAN_TIN %>" onclick="_gaq.push(['_trackEvent', 'Contact', 'Call Now Button', 'Phone']);" id="callnowbutton" style="" class="btn-callphone">&nbsp;</a></td>
                </tr>
                <%} %>
            </tbody>
        </table>
    </div>
</body>
</html>
