<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MainMenu.ascx.cs" Inherits="CMS.Control.MainMenu" %>
<telerik:RadMenu ID="radMenu" runat="server" DataFieldID="ID" DataFieldParentID="ID_CHA" DataNavigateUrlField="URL" CssClass="main-menu"
    DataTextField="TEN" DataValueField="ID" Skin="Bootstrap" Width="100%" ClickToOpen="true" OnItemDataBound="RadMenu1_ItemDataBound">
    <ExpandAnimation Type="InOutBack" Duration="3" />
    <DataBindings>
        <telerik:RadMenuItemBinding ImageUrlField="ICON" />
    </DataBindings>
</telerik:RadMenu>
