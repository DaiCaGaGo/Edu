<%@ Page Title="" Language="C#" MasterPageFile="~/LoginMaster.Master" AutoEventWireup="true" CodeBehind="LoginFacebook.aspx.cs" Inherits="CMS.LoginFacebook" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">
       checkURL();
       function checkURL() {
           var url = window.location.href;
           if (url.indexOf("#access_token") > 0)
               window.location = url.replace("#access_token", "access_token?");
       }
    </script>
</asp:Content>
