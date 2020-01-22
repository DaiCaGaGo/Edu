<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LichThiLop.aspx.cs" Inherits="CMS.LichThiLop" %>

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
            <asp:Label ID="lblLop" runat="server"></asp:Label></span></h3>
    </section>
    <br />
    <div class="cpn-table-response" style="width: 100%; margin: 0px auto;">
        <table border="1" cellpadding="0" cellspacing="0" dir="ltr" style="min-width: 0px;">
            <tbody>
                <tr>
                    <td style="text-align: center">Môn học</td>
                    <td style="width: 21%; text-align: center">15 phút</td>
                    <td style="width: 21%; text-align: center">1 tiết</td>
                    <td style="width: 21%; text-align: center">Giữa kỳ</td>
                    <td style="width: 21%; text-align: center">Học kỳ</td>
                </tr>
                <%for (int i = 0; i < listLichThi.Count; i++)
                    { %>
                <tr>
                    <td><%= listLichThi[i].TEN%></td>
                    <td style="text-align: center"><%= listLichThi[i].TIME_15P== null ? "&nbsp;": (Convert.ToDateTime(listLichThi[i].TIME_15P).ToString("HH:mm") != "00:00" ? Convert.ToDateTime(listLichThi[i].TIME_15P).ToString("dd/MM/yyyy HH:mm") : Convert.ToDateTime(listLichThi[i].TIME_15P).ToString("HH:mm"))%></td>
                    <td style="text-align: center"><%= listLichThi[i].TIME_1T== null ? "&nbsp;": (Convert.ToDateTime(listLichThi[i].TIME_1T).ToString("HH:mm") != "00:00" ? Convert.ToDateTime(listLichThi[i].TIME_1T).ToString("dd/MM/yyyy HH:mm") : Convert.ToDateTime(listLichThi[i].TIME_1T).ToString("HH:mm"))%></td>
                    <td style="text-align: center"><%= listLichThi[i].TIME_GK== null ? "&nbsp;": (Convert.ToDateTime(listLichThi[i].TIME_GK).ToString("HH:mm") != "00:00" ? Convert.ToDateTime(listLichThi[i].TIME_GK).ToString("dd/MM/yyyy HH:mm") : Convert.ToDateTime(listLichThi[i].TIME_GK).ToString("dd/MM/yyyy"))%></td>
                    <td style="text-align: center"><%= listLichThi[i].TIME_HK== null ? "&nbsp;": (Convert.ToDateTime(listLichThi[i].TIME_HK).ToString("HH:mm") != "00:00" ? Convert.ToDateTime(listLichThi[i].TIME_HK).ToString("dd/MM/yyyy HH:mm") : Convert.ToDateTime(listLichThi[i].TIME_HK).ToString("dd/MM/yyyy"))%></td>
                </tr>
                <%} %>
            </tbody>
        </table>
    </div>
    <footer class="main-footer">
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
