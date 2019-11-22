<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterSYS.Master" AutoEventWireup="true" CodeBehind="ManageCache.aspx.cs" Inherits="CMS.Manage.ManageCache" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">Quản lý cache</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btXoa" CssClass="btn btn-default qi-bt" runat="server" Text="Xóa" OnClick="btXoa_Click" />
                <asp:Button ID="btXoaAll" CssClass="btn btn-default qi-bt" runat="server" Text="Xóa hết" OnClick="btXoaAll_Click" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="row">
            </div>
        </div>
        <div class="item-data">
            <telerik:RadListBox ID="rlCache" Width="100%" CheckBoxes="true" Sort="Ascending" runat="server"></telerik:RadListBox>
        </div>
    </div>
</asp:Content>
