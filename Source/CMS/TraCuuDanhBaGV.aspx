<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TraCuuDanhBaGV.aspx.cs" Inherits="CMS.TraCuuDanhBaGV" %>

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
        <h3><span>
            <asp:Label ID="lblDanhBaGV" runat="server"></asp:Label></span></h3>
    </section>
    <asp:Literal ID="ltrGoiGVCN" runat="server"></asp:Literal>
    <p><b>Giáo viên bộ môn</b></p>
    <div class="cpn-table-response" style="width: 100%; margin: 0px auto;">
        <table border="1" cellpadding="0" cellspacing="0" dir="ltr" style="min-width: 0px;">
            <tbody>
                <tr>
                    <td>STT</td>
                    <td>Tên giáo viên</td>
                    <td>Số điện thoại</td>
                    <td>Gọi điện</td>
                </tr>
                <%for (int i = 0; i < lstGVBM.Count; i++)
                    { %>
                <tr>
                    <td style="vertical-align: middle; font-weight: normal;"><%= i+1 %></td>
                    <td class="cpn-table-left-content-left"><%= lstGVBM[i].HO_TEN%></td>
                    <td class="cpn-table-left-content-left"><%= lstGVBM[i].SDT== null ? "&nbsp;":lstGVBM[i].SDT.ToString()%></td>
                    <td class="cpn-table-left-content-center"><a href="tel: <%= lstGVBM[i].SDT %>" onclick="_gaq.push(['_trackEvent', 'Contact', 'Call Now Button', 'Phone']);" id="callnowbutton" style="" class="btn-callphone">&nbsp;</a></td>
                </tr>
                <%} %>
            </tbody>
        </table>
    </div>
    <footer class="main-footer" style="position: fixed; bottom: 0px; width: 100%; background: #fff;">
        <div>
            <p><span style="font-size: 20px;"><b>CÔNG TY CPTM CÔNG NGHỆ THÔNG TIN DI ĐỘNG ONE-SMS</b></span></p>
            <p>
                Địa chỉ: Coneck Building, 6/61 Phạm Tuấn Tài, Hà Nội<br>
                Điện thoại: 0901 707 069-Nhánh 2&nbsp;- Kinh Doanh: 0901 707 069-Nhánh 1
            </p>
        </div>
    </footer>
</body>
</html>
