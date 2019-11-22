<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="BienLaiThuTienHS.aspx.cs" Inherits="CMS.Report.BienLaiThuTienHS" %>

<%--<%@ Register Assembly="DevExpress.XtraReports.v18.2.Web.WebForms, Version=18.2.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rcbKhoi">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbLop" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbLop">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-3">
                <span class="item-title">CHỌN DANH SÁCH HỌC SINH</span>
            </div>
            <div class="col-sm-9 text-right">
                <asp:Button ID="btTimKiem" runat="server" Text="In" CssClass="btn bt-one" OnClick="btTimKiem_Click" />
            </div>

            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="row">
                <div class="col-sm-2">
                    <telerik:RadComboBox ID="rcbKhoi" runat="server" Width="100%" DataSourceID="objKhoi" DataTextField="TEN" DataValueField="MA" AutoPostBack="True" Filter="Contains" OnSelectedIndexChanged="rcbKhoi_SelectedIndexChanged" EmptyMessage="Chọn khối học" CheckBoxes="true" CheckedItemsTexts="DisplayAllInInput" EnableCheckAllItemsCheckBox="true" Localization-CheckAllString="Chọn tất cả">
                    </telerik:RadComboBox>
                    <asp:HiddenField ID="hdKhoi" runat="server" />
                    <asp:ObjectDataSource ID="objKhoi" runat="server" SelectMethod="getKhoiByCapHoc" TypeName="OneEduDataAccess.BO.KhoiBO">
                        <SelectParameters>
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
                <div class="col-sm-2">
                    <telerik:RadComboBox Width="100%" ID="rcbLop" EmptyMessage="Chọn lớp" runat="server"
                        DataSourceID="objLop" DataTextField="TEN" DataValueField="ID" AllowCustomText="true" Filter="Contains" CheckBoxes="true" CheckedItemsTexts="DisplayAllInInput" EnableCheckAllItemsCheckBox="true" Localization-CheckAllString="Chọn tất cả">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objLop" runat="server" SelectMethod="getLopByLstKhoiNamHoc" TypeName="OneEduDataAccess.BO.LopBO">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="hdKhoi" Name="lstMaKhoi" PropertyName="Value" />
                            <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                            <asp:Parameter DefaultValue="" Name="id_all" Type="Int64" />
                            <asp:Parameter DefaultValue="Tất cả" Name="text_all" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
                <div class="col-sm-2">
                    <telerik:RadNumericTextBox ID="tbSoTien" CssClass="form-control" ClientIDMode="Static" placeholder="Nhập số tiền" runat="server" MinValue="0" MaxLength="9" Width="76%"></telerik:RadNumericTextBox><span> (VNĐ)</span>
                </div>
                <%--<div class="col-sm-1" style="margin-left: -25px; margin-top: 4px;">
                    <span>(VNĐ)</span>
                </div>--%>
                <div class="col-sm-6">
                    <telerik:RadTextBox ID="tbNoiDung" runat="server" EmptyMessage="Nhập nội dung thu tiền" Width="100%"></telerik:RadTextBox>
                </div>
            </div>

        </div>

    </div>
</asp:Content>
