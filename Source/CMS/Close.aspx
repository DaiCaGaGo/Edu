<%@ Page Title="" Language="C#" MasterPageFile="~/LoginMaster.Master" AutoEventWireup="true" CodeBehind="Close.aspx.cs" Inherits="CMS.Close" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">

         window.close();
         if (window.opener && !window.opener.closed) {
             window.opener.location.reload();
         }

    </script>
</asp:Content>
