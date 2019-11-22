<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="CustomerDetail.aspx.cs" Inherits="CMS.Customer.CustomerDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">Chi tiết tổ giáo viên</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btAdd" runat="server" CssClass="btn bt-one" OnClick="btAdd_Click" Text="Thêm" />
                <asp:Button ID="btEdit" runat="server" CssClass="btn bt-one" OnClick="btEdit_Click" Text="Sửa" />
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
                                <asp:TextBox ID="tbTen" CssClass="form-control" ClientIDMode="Static" placeholder="Tên" runat="server" MaxLength="100"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator3" ControlToValidate="tbTen" ClientValidationFunction="validateMaxchar" ValidateEmptyText="true"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Thông tin bắt buộc, nhập không quá 100 ký tự." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbSDT" class="col-sm-3 control-label">Số điện thoại<span style="color: red">(*)</span></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbSDT" CssClass="form-control" ClientIDMode="Static" placeholder="Nhập số điện thoại" runat="server" MaxLength="20"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator6" ControlToValidate="tbSDT" ClientValidationFunction="validateDienThoai" ValidateEmptyText="false" runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Dữ liệu không hợp lệ." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="rcbNhom" class="col-sm-3 control-label">Thuộc nhóm</label>
                            <div class="col-sm-9">
                                <telerik:RadComboBox ID="rcbNhom" runat="server" Width="300px" Filter="Contains">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="" Text="Chọn" />
                                        <telerik:RadComboBoxItem Value="1" Text="Học sinh" />
                                        <telerik:RadComboBoxItem Value="2" Text="Giáo viên" />
                                        <telerik:RadComboBoxItem Value="3" Text="Khác" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="rcbGioiTinh" class="col-sm-3 control-label">Giới tính</label>
                            <div class="col-sm-9">
                                <telerik:RadComboBox ID="rcbGioiTinh" runat="server" Width="300px" Filter="Contains">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="" Text="Chọn" />
                                        <telerik:RadComboBoxItem Value="1" Text="Nam" />
                                        <telerik:RadComboBoxItem Value="2" Text="Nữ" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="rdNgaySinh" class="col-sm-3 control-label">Ngày sinh</label>
                            <div class="col-sm-9">
                                <telerik:RadDatePicker Culture="vi-VN" RenderMode="Classic" ID="rdNgaySinh" runat="server" Width="300px" MinDate="1900/1/1"
                                    Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="Giá trị không hợp lệ"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="Hôm nay"
                                    DatePopupButton-ToolTip="Chọn ngày"
                                    Calendar-FastNavigationSettings-OkButtonCaption="Chọn"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="Hủy">
                                    <DateInput DateFormat="dd/MM/yyyy" ClientEvents-OnError="validRadDatePicker" runat="server"></DateInput>
                                </telerik:RadDatePicker>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbEmail" class="col-sm-3 control-label">Email</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbEmail" CssClass="form-control" ClientIDMode="Static" placeholder="Nhập email" runat="server" MaxLength="20"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator1" ControlToValidate="tbEmail" ClientValidationFunction="validateEmails" ValidateEmptyText="false"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Dữ liệu không hợp lệ." />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>