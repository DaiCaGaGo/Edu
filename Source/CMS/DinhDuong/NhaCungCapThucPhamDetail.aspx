<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="NhaCungCapThucPhamDetail.aspx.cs" Inherits="CMS.DinhDuong.NhaCungCapThucPhamDetail" %>
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
            <div class="col-sm-6">
                <span class="item-title">CHI TIẾT NHÀ CUNG CẤP THỰC PHẨM</span>
            </div>
            <div class="col-sm-6 text-right">
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
                            <label for="tbTen" class="col-sm-3 control-label">Tên <span style="color: red">(*)</span></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbTen" CssClass="form-control" ClientIDMode="Static" placeholder="Tên nhà cung cấp" runat="server" MaxLength="250"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator1" ControlToValidate="tbTen" ClientValidationFunction="validateMaxchar" ValidateEmptyText="true"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Thông tin bắt buộc, nhập không quá 250 ký tự." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbDiaChi" class="col-sm-3 control-label">SĐT</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbSDT" CssClass="form-control" ClientIDMode="Static" placeholder="Số điện thoại" runat="server" MaxLength="20" ></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator3" ControlToValidate="tbDiaChi" ClientValidationFunction="validateMaxchar"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Thông tin bắt buộc, nhập không quá 20 ký tự." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbDiaChi" class="col-sm-3 control-label">Địa chỉ</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbDiaChi" CssClass="form-control" ClientIDMode="Static" placeholder="Địa chỉ" runat="server" MaxLength="250" ></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator2" ControlToValidate="tbDiaChi" ClientValidationFunction="validateMaxchar"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Thông tin bắt buộc, nhập không quá 250 ký tự." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbEmail" class="col-sm-3 control-label">Email</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbEmail" CssClass="form-control" ClientIDMode="Static" placeholder="Email" runat="server" MaxLength="100" ></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator4" ControlToValidate="tbEmail" ClientValidationFunction="validateMaxchar"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Thông tin bắt buộc, nhập không quá 100 ký tự." />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>