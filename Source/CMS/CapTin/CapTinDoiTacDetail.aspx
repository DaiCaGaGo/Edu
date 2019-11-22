<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="CapTinDoiTacDetail.aspx.cs" Inherits="CMS.CapTin.CapTinDoiTacDetail" %>

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
    <div class="item-content">
        <div class="row item-header">
            <div class="col-xs-6">
                <span class="item-title">CHI TIẾT CẤP TIN ĐẠI LÝ</span>
            </div>
            <div class="col-xs-6 text-right">
                <asp:Button ID="btAdd" runat="server" CssClass="btn bt-one" Text="Thêm" OnClick="btAdd_Click" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="form-horizontal" role="form">
                <div class="row">
                    <div class="col-xs-12">
                        <div class="form-group">
                            <label class="col-xs-3 control-label">Tên đại lý <span style="color: red">(*)</span></label>
                            <div class="col-xs-9">
                                <telerik:RadComboBox ID="rcbDaiLy" runat="server" Width="100%" EmptyMessage="Chọn đại lý" DataSourceID="objDaiLy"
                                    DataTextField="TEN" DataValueField="ID" AllowCustomText="true" Filter="Contains">
                                </telerik:RadComboBox>
                                <asp:ObjectDataSource ID="objDaiLy" runat="server" SelectMethod="getDoiTac" TypeName="OneEduDataAccess.BO.DoiTacBO">
                                    <SelectParameters>
                                        <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                                        <asp:Parameter DefaultValue="" Name="id_all" Type="Int16" />
                                        <asp:Parameter Name="text_all" Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="rcbDaiLy"
                                    CssClass="valid-control" ForeColor="Red" ErrorMessage="Thông tin bắt buộc nhập." Display="Dynamic"
                                    SetFocusOnError="true"> </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-xs-3 control-label">Số tin cấp <span style="color: red">(*)</span></label>
                            <div class="col-xs-9">
                                <asp:TextBox ID="tbSoTinCap" CssClass="form-control" ClientIDMode="Static" placeholder="Số tin cấp" runat="server" MaxLength="4" TextMode="Number"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator2" ControlToValidate="tbSoTinCap" ClientValidationFunction="validateNumber" ValidateEmptyText="true" runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Dữ liệu không hợp lệ." />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
