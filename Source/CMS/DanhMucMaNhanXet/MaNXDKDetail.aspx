<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="MaNXDKDetail.aspx.cs" Inherits="CMS.NhanXetHangNgay.MaNXDKDetail" %>

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
                <span class="item-title">CHI TIẾT MÃ NHẬN XÉT</span>
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
                            <label for="rcbLoaiNX" class="col-sm-4 control-label">Loại nhận xét</label>
                            <div class="col-sm-8">
                                <telerik:RadComboBox ID="rcbLoaiNX" runat="server" Width="100%" Filter="Contains">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="1" Text="Môn học hoặc hoạt động giáo dục" />
                                        <telerik:RadComboBoxItem Value="2" Text="Năng lực" />
                                        <telerik:RadComboBoxItem Value="3" Text="Phẩm chất" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbMa" class="col-sm-4 control-label">Mã <span style="color: red">(*)</span></label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="tbMa" CssClass="form-control" ClientIDMode="Static" placeholder="Mã" runat="server" MaxLength="50"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator1" ControlToValidate="tbMa" ClientValidationFunction="validateMaxchar" ValidateEmptyText="true"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Thông tin bắt buộc, nhập không quá 50 ký tự." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbTen" class="col-sm-4 control-label">Nội dung <span style="color: red">(*)</span></label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="tbNoiDung" CssClass="form-control" ClientIDMode="Static" placeholder="Nội dung" runat="server" MaxLength="200"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator3" ControlToValidate="tbNoiDung" ClientValidationFunction="validateMaxchar" ValidateEmptyText="true"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Thông tin bắt buộc, nhập không quá 200 ký tự." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbThuTu" class="col-sm-4 control-label">Thứ tự</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="tbThuTu" CssClass="form-control" ClientIDMode="Static" placeholder="Thứ tự" runat="server" MaxLength="4" TextMode="Number"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator2" ControlToValidate="tbThuTu" ClientValidationFunction="validateInt" ValidateEmptyText="false"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Nhập số không quá 4 ký tự." />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
