<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LichDayGV.aspx.cs" Inherits="CMS.TraCuu.LichDayGV" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="../css/styleStudentInfo.css">
    <link rel="shortcut icon" href="<%= "http://"+HttpContext.Current.Request.Url.Authority+"/img/logo.ico"  %>" />
</head>
<body>
    <p style="text-align: center"><b>Lịch dạy giáo viên</b></p>
    <div class="cpn-table-response" style="width: 100%; margin: 0px auto;">
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
                <%for (int i = 0; i < lstLichDay.Count; i++)
                    { %>
                <tr>
                    <td><%= lstLichDay[i].TIET%></td>
                    <td><%= lstLichDay[i].THU2== null ? "&nbsp;":lstLichDay[i].THU2.ToString()%></td>
                    <td><%= lstLichDay[i].THU3== null ? "&nbsp;":lstLichDay[i].THU3.ToString()%></td>
                    <td><%= lstLichDay[i].THU4== null ? "&nbsp;":lstLichDay[i].THU4.ToString()%></td>
                    <td><%= lstLichDay[i].THU5== null ? "&nbsp;":lstLichDay[i].THU5.ToString()%></td>
                    <td><%= lstLichDay[i].THU6== null ? "&nbsp;":lstLichDay[i].THU6.ToString()%></td>
                    <td><%= lstLichDay[i].THU7== null ? "&nbsp;":lstLichDay[i].THU7.ToString()%></td>
                    <td><%= lstLichDay[i].THU8== null ? "&nbsp;":lstLichDay[i].THU8.ToString()%></td>
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
