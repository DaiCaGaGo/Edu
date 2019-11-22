<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="DoiTacDetail.aspx.cs" Inherits="CMS.DoiTac.DoiTacDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="item-content">
        <div class="row item-header">
            <div class="col-xs-6">
                <span class="item-title">Chi tiết đối tác</span>
            </div>
            <div class="col-xs-6 text-right">
                <asp:Button ID="btAdd" runat="server" CssClass="btn bt-one" OnClick="btAdd_Click" Text="Thêm" />
                <asp:Button ID="btEdit" runat="server" CssClass="btn bt-one" OnClick="btEdit_Click" Text="Sửa" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="form-horizontal" role="form">
                <div class="row">
                    <div class="col-xs-12">
                        <div class="form-group">
                            <label for="tbTen" class="col-xs-3 control-label">Tên đối tác <span style="color: red">(*)</span></label>
                            <div class="col-xs-9">
                                <asp:TextBox ID="tbTen" CssClass="form-control" ClientIDMode="Static" placeholder="Tên đối tác" runat="server" MaxLength="200"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator3" ControlToValidate="tbTen" ClientValidationFunction="validateMaxchar" ValidateEmptyText="true"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Nhập tên đối tác" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbSoDienThoai" class="col-xs-3 control-label">Số điện thoại</label>
                            <div class="col-xs-9">
                                <asp:TextBox ID="tbSoDienThoai" CssClass="form-control" ClientIDMode="Static" placeholder="Số điện thoại" runat="server" MaxLength="20"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator4" ControlToValidate="tbSoDienThoai" ClientValidationFunction="validateDienThoai" ValidateEmptyText="False" runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Dữ liệu không hợp lệ." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbDiaChi" class="col-xs-3 control-label">Địa chỉ</label>
                            <div class="col-xs-9">
                                <asp:TextBox ID="tbDiaChi" CssClass="form-control" ClientIDMode="Static" placeholder="Địa chỉ" runat="server" MaxLength="4000"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator1" ControlToValidate="tbDiaChi" runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
