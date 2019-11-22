<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="MonHocDetail.aspx.cs" Inherits="CMS.MonHoc.MonHocDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rcbLoaiLopGDTX">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="cblKhoiHoc"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
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
        <div class="item-filter">
            <div class="form-horizontal" role="form">
                <div class="row">
                    <div class="col-sm-12">
                        <div class="form-group">
                            <label for="tbTen" class="col-sm-3 control-label">Tên <span style="color: red">(*)</span></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbTen" CssClass="form-control" ClientIDMode="Static" placeholder="Tên môn học" runat="server" MaxLength="50"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator3" ControlToValidate="tbTen" ClientValidationFunction="validateMaxchar" ValidateEmptyText="true"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Thông tin bắt buộc, nhập không quá 50 ký tự." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="rcbKieuMon" class="col-sm-3 control-label">Kiểu môn <span style="color: red">(*)</span></label>
                            <div class="col-sm-9">
                                <telerik:RadComboBox ID="rcbKieuMon" runat="server" EmptyMessage="Chọn kiểu môn" Width="100%" AllowCustomText="true" Filter="Contains">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="0" Text="Điểm số" />
                                        <telerik:RadComboBoxItem Value="1" Text="Điểm nhận xét" />
                                    </Items>
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="rfvKieuMon" runat="server" ControlToValidate="rcbKieuMon" CssClass="valid-control" ForeColor="Red" ErrorMessage="Thông tin bắt buộc nhập" Display="Dynamic" SetFocusOnError="true"> </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Chọn cấp học <span style="color: red">(*)</span></label>
                            <div class="col-sm-9">
                                <telerik:RadComboBox ID="rcbCapHoc" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="rcbCapHoc_SelectedIndexChanged" CausesValidation="false">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="TH" Text="Tiểu học" />
                                        <telerik:RadComboBoxItem Value="THCS" Text="THCS" />
                                        <telerik:RadComboBoxItem Value="THPT" Text="THPT" />
                                        <telerik:RadComboBoxItem Value="GDTX" Text="GDTX" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Chọn khối học <span style="color: red">(*)</span></label>
                            <div class="col-sm-9">
                                <telerik:RadComboBox ID="rcbKhoi" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="100%" EmptyMessage="Chọn khối" AllowCustomText="true" Filter="Contains" DataSourceID="objKhoiHoc" DataTextField="TEN" DataValueField="MA">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="rfvCap" runat="server" ControlToValidate="rcbKhoi" CssClass="valid-control" ForeColor="Red" ErrorMessage="Thông tin bắt buộc nhập" Display="Dynamic" SetFocusOnError="true"> </asp:RequiredFieldValidator>
                                <asp:ObjectDataSource ID="objKhoiHoc" runat="server" SelectMethod="getKhoi" TypeName="OneEduDataAccess.BO.KhoiBO">
                                    <SelectParameters>
                                        <asp:ControlParameter Name="cap_hoc" ControlID="rcbCapHoc" PropertyName="SelectedValue" />
                                        <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                                        <asp:Parameter DefaultValue="" Name="id_all" Type="Int16" />
                                        <asp:Parameter DefaultValue="Tất cả" Name="text_all" Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbHeSo" class="col-sm-3 control-label">Hệ số</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbHeSo" CssClass="form-control" ClientIDMode="Static" placeholder="Hệ số" runat="server" MaxLength="4" TextMode="Number"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator4" ControlToValidate="tbHeSo" ClientValidationFunction="validateInt" ValidateEmptyText="false"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Dữ liệu không hợp lệ" />
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
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
