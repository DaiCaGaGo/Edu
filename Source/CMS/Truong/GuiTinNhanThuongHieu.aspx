<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="GuiTinNhanThuongHieu.aspx.cs" Inherits="CMS.Truong.GuiTinNhanThuongHieu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>

    <telerik:RadCodeBlock runat="server">
        <script>
        </script>
    </telerik:RadCodeBlock>

    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">Gửi tin nhắn thương hiệu</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btSendSms" runat="server" CssClass="btn bt-one" OnClick="btSendSms_Click" Text="Gửi tin" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="form-horizontal" role="form">
                <div class="row">
                    <div class="col-sm-12">
                        <telerik:RadComboBox ID="rcbTruong" runat="server" Width="100%" DataTextField="TEN" DataValueField="ID" Filter="Contains" AllowCustomText="true" EmptyMessage="Chọn trường học" CheckBoxes="true" CheckedItemsTexts="DisplayAllInInput" EnableCheckAllItemsCheckBox="true" Localization-CheckAllString="Chọn tất cả">
                        </telerik:RadComboBox>
                    </div>
                </div>
                <div class="row" style="margin-top: 15px;">
                    <div class="col-sm-12">
                        <asp:TextBox ID="tbListSDT" ClientIDMode="Static" runat="server" placeholder="Nhập SĐT" CssClass="form-control text-box nd-nx-nl" TextMode="MultiLine" Rows="2"></asp:TextBox>
                    </div>
                </div>
                <div class="row" style="margin-top: 15px;">
                    <div class="col-sm-12">
                        <asp:TextBox ID="tbNoiDung" ClientIDMode="Static" runat="server" placeholder="Nhập nội dung tin nhắn" CssClass="form-control text-box nd-nx-nl" TextMode="MultiLine" Rows="3"></asp:TextBox>
                    </div>
                </div>
                <div class="row" style="margin-top: 15px;">
                    <div class="col-sm-12">
                        <span style="position: absolute; padding-top: 5px"><span style="color: red">Ghi chú:</span> Không nhập quá 10 SĐT, giữa các số ngăn cách bởi dấu <span style="color: red">";"</span></span>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
