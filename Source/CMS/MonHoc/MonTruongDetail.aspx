<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="MonTruongDetail.aspx.cs" Inherits="CMS.MonHoc.MonTruongDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">CHI TIẾT MÔN HỌC</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btAdd" runat="server" CssClass="btn bt-one" OnClick="btAdd_Click" Text="Thêm" />
                <asp:Button ID="btEdit" runat="server" CssClass="btn bt-one" OnClick="btEdit_Click" Text="Sửa" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter" id="divAdd">
            <div class="form-horizontal" role="form">
                <div class="row">
                    <div class="col-sm-12">
                        <div class="form-group">
                            <label for="tbTen" class="col-sm-3 control-label">Tên <span style="color: red">(*)</span></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbTen" CssClass="form-control" ClientIDMode="Static" placeholder="Tên môn học" runat="server" MaxLength="50"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator" ControlToValidate="tbTen" ClientValidationFunction="validateMaxchar" ValidateEmptyText="true"
                            runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Thông tin bắt buộc, nhập không quá 50 ký tự." />

                            </div>
                        </div>
                        <div class="form-group">
                            <label for="rcbKieuMon" class="col-sm-3 control-label">Kiểu môn <span style="color: red">(*)</span></label>
                            <div class="col-sm-9">
                                <telerik:RadComboBox ID="rcbKieuMon" runat="server" Width="100%" Filter="Contains">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="0" Text="Điểm số" />
                                        <telerik:RadComboBoxItem Value="1" Text="Điểm nhận xét" />
                                    </Items>
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="rfvKieuMon" runat="server" ControlToValidate="rcbKieuMon" CssClass="valid-control" ForeColor="Red" ErrorMessage="Thông tin bắt buộc nhập" Display="Dynamic" SetFocusOnError="true"> </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group" runat="server" id="divGDTX">
                            <label class="col-sm-3 control-label">Mã loại lớp GDTX</label>
                            <div class="col-sm-9">
                                <telerik:RadComboBox ID="rcbLoaiLopGDTX" runat="server" Width="100%" DataTextField="TEN" DataValueField="MA" AutoPostBack="true" CausesValidation="false" OnSelectedIndexChanged="rcbLoaiLopGDTX_SelectedIndexChanged" Filter="Contains">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="2" Text="Bổ túc THCS" />
                                        <telerik:RadComboBoxItem Value="3" Text="Bổ túc THPT" />
                                    </Items>
                                </telerik:RadComboBox>

                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Chọn khối học <span style="color: red">(*)</span></label>
                            <div class="col-sm-9">
                                <div class="checkbox">
                                    <label>

                                        <asp:CheckBoxList ID="cblKhoiHoc" runat="server" DataSourceID="objKhoiHoc" DataTextField="TEN" DataValueField="MA">
                                        </asp:CheckBoxList>
                                        <asp:CustomValidator ID="CustomValidator2" ErrorMessage ="Thông tin bắt buộc, chọn khối cho môn học." ForeColor="Red" runat="server" />
                                        <asp:ObjectDataSource ID="objKhoiHoc" runat="server" SelectMethod="getKhoiByCapHoc" TypeName="OneEduDataAccess.BO.KhoiBO">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="rcbLoaiLopGDTX" Name="maLoaiLopGDTX" PropertyName="SelectedValue" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbThuTu" class="col-sm-3 control-label">Thứ tự</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbThuTu" CssClass="form-control" ClientIDMode="Static" placeholder="Thứ tự" runat="server" MaxLength="4" TextMode="Number"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator1" ControlToValidate="tbThuTu" ClientValidationFunction="validateInt" ValidateEmptyText="true"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Nhập số không quá 4 ký tự." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbHeSo" class="col-sm-3 control-label">Hệ số</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbHeSo" CssClass="form-control" ClientIDMode="Static" placeholder="Hệ số" runat="server" MaxLength="4" TextMode="Number"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator4" ControlToValidate="tbHeSo" ClientValidationFunction="validateInt" ValidateEmptyText="false"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Dữ liệu không hợp lệ." />
                            </div>
                        </div>
                        <div runat="server" class="form-group" id="divMonChuyen">
                            <label class="col-sm-3 control-label">Là môn chuyên</label>
                            <div class="col-sm-9">
                                <div class="one-checkbox">
                                    <label>
                                        <asp:CheckBox ID="cbMonChuyen" runat="server" Text="Là môn chuyên" />
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
