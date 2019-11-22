<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="PhieuHuongDanQuanTamOAZalo.aspx.cs" Inherits="CMS.Report.PhieuHuongDanQuanTamOAZalo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rcbTruong">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbCapHoc" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="rcbKhoi" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="rcbLop" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbCapHoc">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbKhoi"  UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="rcbLop" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbKhoi">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbLop" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
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
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbTruong" runat="server" Width="100%" DataTextField="TEN" DataValueField="ID" Filter="Contains" OnSelectedIndexChanged="rcbTruong_SelectedIndexChanged" AutoPostBack="true">
                    </telerik:RadComboBox>
                </div>
                <div class="col-sm-2">
                    <telerik:RadComboBox ID="rcbCapHoc" runat="server" Width="100%" Filter="Contains" OnSelectedIndexChanged="rcbCapHoc_SelectedIndexChanged" AutoPostBack="true">
                    </telerik:RadComboBox>
                </div>
                <div class="col-sm-2">
                    <telerik:RadComboBox ID="rcbKhoi" runat="server" Width="100%" DataSourceID="objKhoi" DataTextField="TEN" DataValueField="MA" Filter="Contains" OnSelectedIndexChanged="rcbKhoi_SelectedIndexChanged" AutoPostBack="true">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objKhoi" runat="server" SelectMethod="getKhoiByCapHoc" TypeName="OneEduDataAccess.BO.KhoiBO">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="rcbCapHoc" Name="cap_hoc" PropertyName="SelectedValue"/>
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
                <div class="col-sm-2">
                    <telerik:RadComboBox Width="100%" ID="rcbLop" EmptyMessage="Chọn lớp" runat="server"
                            DataSourceID="objLop" DataTextField="TEN" DataValueField="ID" AllowCustomText="true" Filter="Contains" CheckBoxes="true" CheckedItemsTexts="DisplayAllInInput" EnableCheckAllItemsCheckBox="true" Localization-CheckAllString="Chọn tất cả">
                        </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objLop" runat="server" SelectMethod="getLopByKhoiNamHoc" TypeName="OneEduDataAccess.BO.LopBO">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="rcbTruong" Name="idTruong" PropertyName="SelectedValue" />
                            <asp:ControlParameter ControlID="rcbCapHoc" Name="ma_cap_hoc" PropertyName="SelectedValue" />
                            <asp:ControlParameter ControlID="rcbKhoi" Name="maKhoi" PropertyName="SelectedValue" />
                            <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                            <asp:Parameter DefaultValue="" Name="id_all" Type="Int64" />
                            <asp:Parameter Name="text_all" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
            </div>

        </div>

    </div>
</asp:Content>