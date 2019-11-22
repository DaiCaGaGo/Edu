<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="GoiTinDetail.aspx.cs" Inherits="CMS.GoiTin.GoiTinDetail" %>
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
            <div class="col-sm-4">
                <span class="item-title">CHI TIẾT GÓI TIN</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btAdd" runat="server" CssClass="btn bt-one" Text="Thêm" OnClick="btAdd_Click" />
                <asp:Button ID="btEdit" runat="server" CssClass="btn bt-one" Text="Sửa" OnClick="btEdit_Click" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="form-horizontal" role="form">
                <div class="row">
                    <div class="col-sm-12">
                        <div class="form-group">
                            <label for="tbTen" class="col-sm-4 control-label-left">Tên <span style="color: red">(*)</span></label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="tbTen" CssClass="form-control" ClientIDMode="Static" placeholder="Tên" runat="server" MaxLength="50"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator1" ControlToValidate="tbTen" ClientValidationFunction="validateMaxchar" ValidateEmptyText="true"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Thông tin bắt buộc, nhập không quá 50 ký tự." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbSoTinLienLac" class="col-sm-4 control-label-left">Số tin liên lạc (/tuần) <span style="color: red">(*)</span></label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="tbSoTinLienLac" CssClass="form-control" ClientIDMode="Static" placeholder="Số tin liên lạc" runat="server" MaxLength="200"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator3" ControlToValidate="tbSoTinLienLac" ClientValidationFunction="validateInt" ValidateEmptyText="true"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Dữ liệu không hợp lệ." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbSoTinThongBao" class="col-sm-4 control-label-left">Số tin thông báo (/tháng) <span style="color: red">(*)</span></label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="tbSoTinThongBao" CssClass="form-control" ClientIDMode="Static" placeholder="Số tin thông báo" runat="server" MaxLength="200"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator4" ControlToValidate="tbSoTinThongBao" ClientValidationFunction="validateInt" ValidateEmptyText="true"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Dữ liệu không hợp lệ." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbSoTinHe" class="col-sm-4 control-label-left">Số tin hè</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="tbSoTinHe" CssClass="form-control" ClientIDMode="Static" placeholder="Số tin hè" runat="server" MaxLength="200"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator5" ControlToValidate="tbSoTinHe" ClientValidationFunction="validateInt" ValidateEmptyText="false"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Dữ liệu không hợp lệ." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbThuTu" class="col-sm-4 control-label-left">Thứ tự</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="tbThuTu" CssClass="form-control" ClientIDMode="Static" placeholder="Thứ tự" runat="server" MaxLength="4" TextMode="Number"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator2" ControlToValidate="tbThuTu" ClientValidationFunction="validateInt" ValidateEmptyText="false"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Nhập số không quá 4 ký tự." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbGhiChu" class="col-sm-4 control-label-left">Ghi chú</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="tbGhiChu" CssClass="form-control" ClientIDMode="Static" placeholder="Ghi chú" runat="server" MaxLength="100"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
