<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewThongBao.aspx.cs" Inherits="CMS.TinTuc.ViewThongBao" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title runat="server" id="title"></title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel='stylesheet' href='https://fonts.googleapis.com/css?family=Raleway%3A400%2C500%2C600%2C700%2C300%2C100%2C800%2C900%7COpen+Sans%3A400%2C300%2C300italic%2C400italic%2C600%2C600italic%2C700%2C700italic&#038;subset=latin%2Clatin-ext&#038;ver=1.4.3' type='text/css' media='all' />
    <link rel="stylesheet" href="/Assets/templates/plugins/bootstrap-4.3.1/css/bootstrap.min.css" />

    <link rel="stylesheet" href="/Assets/templates/plugins/wow/css/libs/animate.css">
    <link rel="stylesheet" type="text/css" href="/Assets/templates/css/main-style.css" />
    <link rel="stylesheet" type="text/css" href="/Assets/templates/css/global-style.css" />
    <link rel="stylesheet" type="text/css" href="/Assets/templates/css/menu-style.css" />
    <link rel="stylesheet" type="text/css" href="/Assets/templates/css/response.css" />
    <link href="https://fonts.googleapis.com/css?family=Roboto" rel="stylesheet">
    <link rel="stylesheet" type="text/css" href="/Assets/templates/plugins/font-awesome-4.6.3/css/font-awesome.css" />

    <script type="text/javascript" src="/Assets/templates/plugins/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="/Assets/templates/plugins/bootstrap-4.3.1/js/bootstrap.min.js"></script>
    <script src="/Assets/templates/js/function.js"></script>
</head>
<body>
    <section id="wrap" class="">
        <header class="main-header clearfix">

            <section class="head-foot">
                <div class="container">
                    <div class="box-head-foot">
                        <div class="mini-box date-time">
                            <span class="show-date">
                                <img src="/img/logo1.jpg" alt="Alternate Text" style="max-width: 80%" />
                            </span>
                        </div>
                        <div class="mini-box scroll-left">
                            <marquee direction="left" class="">THÔNG BÁO CHUNG CỦA TRƯỜNG: <label runat="server" id="schoolName"></label> </marquee>
                        </div>
                    </div>
                </div>
            </section>
        </header>
        <!-- content is King -->
        <article class="main-content">
            <div class="container">
                <section class="row main-bound">
                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col- main-left page-one">

                        <%--<h1 class="title-main" runat="server" id="title_h1" style="color: red;"></h1>
                        <section class="single-page content-page">
                            <asp:Literal Text="" runat="server" ID="content" />
                            <div class="box-list-images">
                                <%
                                    if (listImage != null && listImage.Count() > 0)
                                    {
                                        foreach (var itemImage in listImage)
                                        {
                                            if (itemImage == null || String.IsNullOrEmpty(itemImage))
                                            {
                                                continue;
                                            }
                                %>
                                <div class="image">
                                    <div class="thumbnail">
                                        <img src="<%= "http://" + HttpContext.Current.Request.Url.Authority + "" + itemImage.ToString() %>" />
                                        <input name="list_images_node[]" type="hidden" value="<%= itemImage.ToString() %>" />
                                    </div>
                                </div>
                            <%
                                    }
                                }
                            %>
                            </div>
                        </section>--%>
                        <% foreach (var notify in listThongBao)
                            {%>
                        <h1 class="title-main" id="title_h1" style="color: red;"><%= notify.NOI_DUNG %></h1>
                        <section class="single-page content-page">
                            <asp:Literal Text="" runat="server" ID="content" />
                            <div class="box-list-images">
                                <%
                                    if (notify != null && notify.LIST_IMAGE.Count() > 0)
                                    {
                                        foreach (var itemImage in notify.LIST_IMAGE)
                                        {
                                            if (itemImage == null || String.IsNullOrEmpty(itemImage))
                                            {
                                                continue;
                                            }
                                %>
                                <div class="image">
                                    <div class="thumbnail">
                                        <img src="<%= "http://" + HttpContext.Current.Request.Url.Authority + "" + itemImage.ToString() %>" />
                                        <input name="list_images_node[]" type="hidden" value="<%= itemImage.ToString() %>" />
                                    </div>
                                </div>
                                <%
                                        }
                                    }
                                %>
                            </div>
                        </section>

                        <%} %>
                    </div>
                </section>
            </div>
        </article>
        <footer class="main-footer">
            <div>
                <p><span style="font-size: 20px;"><b>CÔNG TY CPTM CÔNG NGHỆ THÔNG TIN DI ĐỘNG ONE-SMS</b></span></p>
                <p>
                    Địa chỉ: Coneck Building, 6/61 Phạm Tuấn Tài, Hà Nội<br>
                    Điện thoại: 0901 707 069-Nhánh 2&nbsp;- Kinh Doanh: 0901 707 069-Nhánh 1
                </p>
            </div>
        </footer>
    </section>
</body>
</html>
