<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentInfor.aspx.cs" Inherits="CMS.StudentInfor" %>

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
        <div>
            <h3><span>Họ và tên: <span style="font-weight: bold">
                <asp:Label ID="lbHoTen" runat="server"></asp:Label></span></span></h3>
            <h4><span>Lớp: <span style="font-weight: bold">
                <asp:Label ID="lbLop" runat="server"></asp:Label></span></span></h4>
        </div>
        <div class="image-book">
            <img src="img/book.png" width="200px">
        </div>
    </section>
    <h4>Điểm chi tiết môn học</h4>
    <div class="cpn-table" style="display: flex;">
        <div class="cp-table-left">
            <table border="1" cellpadding="0" cellspacing="0" dir="ltr">
                <tbody>
                    <tr>
                        <td style="border-bottom-color: #1ac6ff;">&nbsp</td>
                        <td style="border-bottom-color: #1ac6ff;">&nbsp</td>
                    </tr>
                    <tr>
                        <td style="border-bottom-color: #1ac6ff;">STT</td>
                        <td style="border-bottom-color: #1ac6ff;"><span>Môn học</span></td>
                    </tr>
                    <tr>
                        <td style="">&nbsp</td>
                        <td style="">&nbsp</td>
                    </tr>
                    <%for (int i = 0; i < lstDiemChiTiet.Count; i++)
                        { %>
                    <tr>
                        <td><%=(i+1) %></td>
                        <td class="zoom" style="text-align: left"><%= lstDiemChiTiet[i].TEN_MON_HOC%></td>
                    </tr>
                    <%} %>
                </tbody>
            </table>
        </div>
        <div class="cp-table-right" style="width: 100%;">
            <table border="1" cellpadding="0" cellspacing="0" dir="ltr">
                <tbody>
                    <tr>
                        <td colspan="15" rowspan="1">Điểm hệ số 1</td>
                        <td colspan="5" rowspan="1">Điểm hệ số 2</td>
                    </tr>
                    <tr>
                        <td colspan="5" rowspan="1">Miệng</td>
                        <td colspan="5" rowspan="1">15 phút</td>
                        <td colspan="5" rowspan="1">1 tiết</td>
                        <td colspan="5" rowspan="1">1 tiết</td>
                    </tr>
                    <tr>
                        <td>1</td>
                        <td>2</td>
                        <td>3</td>
                        <td>4</td>
                        <td>5</td>
                        <td>1</td>
                        <td>2</td>
                        <td>3</td>
                        <td>4</td>
                        <td>5</td>
                        <td>1</td>
                        <td>2</td>
                        <td>3</td>
                        <td>4</td>
                        <td>5</td>
                        <td>1</td>
                        <td>2</td>
                        <td>3</td>
                        <td>4</td>
                        <td>5</td>
                    </tr>
                    <!-- show diem -->
                    <%for (int i = 0; i < lstDiemChiTiet.Count; i++)
                        { %>
                    <tr>
                        <td><%=lstDiemChiTiet[i].DIEM1 ==null? "&nbsp;":lstDiemChiTiet[i].DIEM1.ToString()%></td>
                        <td><%=lstDiemChiTiet[i].DIEM2 ==null? "&nbsp;":lstDiemChiTiet[i].DIEM2.ToString()%></td>
                        <td><%=lstDiemChiTiet[i].DIEM3 ==null? "&nbsp;":lstDiemChiTiet[i].DIEM3.ToString()%></td>
                        <td><%=lstDiemChiTiet[i].DIEM4 ==null? "&nbsp;":lstDiemChiTiet[i].DIEM4.ToString()%></td>
                        <td><%=lstDiemChiTiet[i].DIEM5 ==null? "&nbsp;":lstDiemChiTiet[i].DIEM5.ToString()%></td>
                        <td><%=lstDiemChiTiet[i].DIEM6 ==null? "&nbsp;":lstDiemChiTiet[i].DIEM6.ToString()%></td>
                        <td><%=lstDiemChiTiet[i].DIEM7 ==null? "&nbsp;":lstDiemChiTiet[i].DIEM7.ToString()%></td>
                        <td><%=lstDiemChiTiet[i].DIEM8 ==null? "&nbsp;":lstDiemChiTiet[i].DIEM8.ToString()%></td>
                        <td><%=lstDiemChiTiet[i].DIEM9 ==null? "&nbsp;":lstDiemChiTiet[i].DIEM9.ToString()%></td>
                        <td><%=lstDiemChiTiet[i].DIEM10 ==null? "&nbsp;":lstDiemChiTiet[i].DIEM10.ToString()%></td>
                        <td><%=lstDiemChiTiet[i].DIEM11 ==null? "&nbsp;":lstDiemChiTiet[i].DIEM11.ToString()%></td>
                        <td><%=lstDiemChiTiet[i].DIEM12 ==null? "&nbsp;":lstDiemChiTiet[i].DIEM12.ToString()%></td>
                        <td><%=lstDiemChiTiet[i].DIEM13 ==null? "&nbsp;":lstDiemChiTiet[i].DIEM13.ToString()%></td>
                        <td><%=lstDiemChiTiet[i].DIEM14 ==null? "&nbsp;":lstDiemChiTiet[i].DIEM14.ToString()%></td>
                        <td><%=lstDiemChiTiet[i].DIEM15 ==null? "&nbsp;":lstDiemChiTiet[i].DIEM15.ToString()%></td>
                        <td><%=lstDiemChiTiet[i].DIEM16 ==null? "&nbsp;":lstDiemChiTiet[i].DIEM16.ToString()%></td>
                        <td><%=lstDiemChiTiet[i].DIEM17 ==null? "&nbsp;":lstDiemChiTiet[i].DIEM17.ToString()%></td>
                        <td><%=lstDiemChiTiet[i].DIEM18 ==null? "&nbsp;":lstDiemChiTiet[i].DIEM18.ToString()%></td>
                        <td><%=lstDiemChiTiet[i].DIEM19 ==null? "&nbsp;":lstDiemChiTiet[i].DIEM19.ToString()%></td>
                        <td><%=lstDiemChiTiet[i].DIEM20 ==null? "&nbsp;":lstDiemChiTiet[i].DIEM20.ToString()%></td>
                    </tr>
                    <%} %>
                </tbody>
            </table>
        </div>
    </div>
    <br />
    <hr />
    <br />
    <h4>Điểm tổng kết</h4>
    <div class="cpn-table-response" style="width:100%;margin:0px auto;">
        <table border="1" cellpadding="0" cellspacing="0" dir="ltr" style="min-width:0px;">
            <tbody>
                <tr>
                    <td>Môn học</td>
                    <td>HK1</td>
                    <td>HK2</td>
                    <td>CN</td>
                </tr>
                <%for (int i = 0; i < lstDiemChiTiet.Count; i++)
                    { %>
                <tr>
                    <td><%= lstDiemChiTiet[i].TEN_MON_HOC%></td>
                    <td><%= lstDiemChiTiet[i].DIEM_TRUNG_BINH_KY1== null ? "&nbsp;":lstDiemChiTiet[i].DIEM_TRUNG_BINH_KY1.ToString()%></td>
                    <td><%= lstDiemChiTiet[i].DIEM_TRUNG_BINH_KY2== null ? "&nbsp;":lstDiemChiTiet[i].DIEM_TRUNG_BINH_KY2.ToString()%></td>
                    <td><%= lstDiemChiTiet[i].DIEM_TRUNG_BINH_CN== null ? "&nbsp;":lstDiemChiTiet[i].DIEM_TRUNG_BINH_CN.ToString()%></td>
                </tr>
                <%} %>
            </tbody>
        </table>
    </div>
</body>
</html>
