<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="KhoiDetail.aspx.cs" Inherits="CMS.Khoi.KhoiDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="cbIsGDTX">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbLoaiLopGDTX" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">chi tiết khối</span>
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
                            <label for="tbMa" class="col-sm-3 control-label">Mã <span style="color: red">(*)</span></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbMa" CssClass="form-control" ClientIDMode="Static" placeholder="Mã" runat="server" MaxLength="4"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator1" ControlToValidate="tbMa" ClientValidationFunction="validateInt" ValidateEmptyText="true"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Nhập số không quá 4 ký tự." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbTen" class="col-sm-3 control-label">Tên <span style="color: red">(*)</span></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbTen" CssClass="form-control" ClientIDMode="Static" placeholder="Tên" runat="server" MaxLength="50"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator3" ControlToValidate="tbTen" ClientValidationFunction="validateMaxchar" ValidateEmptyText="true"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Thông tin bắt buộc, nhập không quá 50 ký tự." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbThuTu" class="col-sm-3 control-label">Thứ tự</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbThuTu" CssClass="form-control" ClientIDMode="Static" placeholder="Thứ tự" runat="server" MaxLength="4" TextMode="Number"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator2" ControlToValidate="tbThuTu" ClientValidationFunction="validateInt" ValidateEmptyText="false"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Nhập số không quá 4 ký tự." />
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-3 control-label">Cấp Mầm non</label>
                            <div class="col-sm-9">
                                <div class="one-checkbox">
                                    <label>
                                        <asp:CheckBox ID="cbIsMN" runat="server" Text="Có" />
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Cấp Tiểu học</label>
                            <div class="col-sm-9">
                                <div class="one-checkbox">
                                    <label>
                                        <asp:CheckBox ID="cbIsTH" runat="server" Text="Có" />
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Cấp THCS</label>
                            <div class="col-sm-9">
                                <div class="one-checkbox">
                                    <label>
                                        <asp:CheckBox ID="cbIsTHCS" runat="server" Text="Có" />
                                    </label>
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-3 control-label">Cấp THPT</label>
                            <div class="col-sm-9">
                                <div class="one-checkbox">
                                    <label>
                                        <asp:CheckBox ID="cbIsTHPT" runat="server" Text="Có" />
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Cấp GDTX</label>
                            <div class="col-sm-9">
                                <div class="one-checkbox">
                                    <label>
                                        <asp:CheckBox ID="cbIsGDTX" runat="server" Text="Có" AutoPostBack="true" OnCheckedChanged="cbIsGDTX_CheckedChanged" />
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Mã loại lớp GDTX</label>
                            <div class="col-sm-9">
                                <telerik:RadComboBox ID="rcbLoaiLopGDTX" runat="server" Width="100%" EmptyMessage="Chọn loại lớp"
                                    DataTextField="TEN" DataValueField="MA" Enabled="false" AllowCustomText="true" Filter="Contains">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="2" Text="Bổ túc THCS" />
                                        <telerik:RadComboBoxItem Value="3" Text="Bổ túc THPT" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
