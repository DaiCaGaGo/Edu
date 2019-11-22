<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="GiaoVienDetail.aspx.cs" Inherits="CMS.GiaoVien.GiaoVienDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">CHI TIẾT GIÁO VIÊN</span>
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
                            <label for="tbSDT" class="col-sm-3 control-label">Số điện thoại</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbSDT" CssClass="form-control" ClientIDMode="Static" placeholder="Nhập số điện thoại" runat="server" MaxLength="20"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator6" ControlToValidate="tbSDT" ClientValidationFunction="validateDienThoai" ValidateEmptyText="False" runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Dữ liệu không hợp lệ." />
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-3 control-label">Chức vụ</label>
                            <div class="col-sm-9">
                                <telerik:RadComboBox ID="rcbChucVu" runat="server" Width="300px" DataSourceID="objectChucVu" DataTextField="TEN" DataValueField="ID" Filter="Contains">
                                </telerik:RadComboBox>
                                <asp:ObjectDataSource ID="objectChucVu" runat="server" SelectMethod="getChucVu" TypeName="OneEduDataAccess.BO.DMChucVuBO">
                                    <SelectParameters>
                                        <asp:Parameter Name="is_all" DbType="Boolean" DefaultValue="False" />
                                        <asp:Parameter Name="id_all" DbType="Int16" DefaultValue="0" />
                                        <asp:Parameter Name="text_all" DbType="String" DefaultValue="" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Trạng thái</label>
                            <div class="col-sm-9">
                                <telerik:RadComboBox ID="rcbTrangThai" runat="server" Width="300px" DataSourceID="objTrangThai" DataTextField="TEN" DataValueField="MA" Filter="Contains">
                                </telerik:RadComboBox>
                                <asp:ObjectDataSource ID="objTrangThai" runat="server" SelectMethod="getTrangThaiGV" TypeName="OneEduDataAccess.BO.TrangThaiGVBO">
                                    <SelectParameters>
                                        <asp:Parameter Name="is_all" DbType="Boolean" DefaultValue="False" />
                                        <asp:Parameter Name="id_all" DbType="Int16" DefaultValue="0" />
                                        <asp:Parameter Name="text_all" DbType="String" DefaultValue="" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="rcbGioiTinh" class="col-sm-3 control-label">Giới tính</label>
                            <div class="col-sm-9">
                                <telerik:RadComboBox ID="rcbGioiTinh" runat="server" Width="300px" Filter="Contains">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="1" Text="Nam" />
                                        <telerik:RadComboBoxItem Value="2" Text="Nữ" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="rdNgaySinh" class="col-sm-3 control-label">Ngày sinh</label>
                            <div class="col-sm-9">
                                <%--                            <telerik:RadDatePicker ID="rdNgaySinh" runat="server" Width="300px"></telerik:RadDatePicker>--%>
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
                            <label for="tbThuTu" class="col-sm-3 control-label">Thứ tự</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbThuTu" CssClass="form-control" ClientIDMode="Static" placeholder="Thứ tự" runat="server" MaxLength="4" TextMode="Number"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbDiaChi" class="col-sm-3 control-label">Địa chỉ</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbDiaChi" CssClass="form-control" ClientIDMode="Static" placeholder="Nhập địa chỉ" runat="server" MaxLength="100"></asp:TextBox>
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
