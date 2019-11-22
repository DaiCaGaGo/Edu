<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="TruongDetail.aspx.cs" Inherits="CMS.Truong.TruongDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="cbIsGDTX">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbLoaiLopGDTX" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbTinhThanh">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbQuanHuyen" />
                    <telerik:AjaxUpdatedControl ControlID="rcbXaPhuong" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbQuanHuyen">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbXaPhuong" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="chbIsActive">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divSanQuyTinNam" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">Quản lý trường</span>
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
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label for="tbMa" class="col-sm-5 control-label">Mã <span style="color: red">(*)</span></label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbMa" CssClass="form-control" ClientIDMode="Static" placeholder="Mã" runat="server" MaxLength="20"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator1" ControlToValidate="tbMa" ClientValidationFunction="validateMaxchar" ValidateEmptyText="true"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Thông tin bắt buộc, nhập không quá 20 ký tự." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbTen" class="col-sm-5 control-label">Tên <span style="color: red">(*)</span></label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbTen" CssClass="form-control" ClientIDMode="Static" placeholder="Tên" runat="server" MaxLength="50"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator3" ControlToValidate="tbTen" ClientValidationFunction="validateMaxchar" ValidateEmptyText="true"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Thông tin bắt buộc, nhập không quá 50 ký tự." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="rcbCapHoc" class="col-sm-5 control-label">Chọn cấp học <span style="color: red">(*)</span></label>
                            <div class="col-sm-7">
                                <telerik:RadComboBox ID="rcbCapHoc" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="100%" EmptyMessage="Chọn mã cấp học" AllowCustomText="true" Filter="Contains">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="MN" Text="Mầm non" />
                                        <telerik:RadComboBoxItem Value="TH" Text="Tiểu học" />
                                        <telerik:RadComboBoxItem Value="THCS" Text="THCS" />
                                        <telerik:RadComboBoxItem Value="THPT" Text="THPT" />
                                        <telerik:RadComboBoxItem Value="GDTX" Text="GDTX" />
                                    </Items>
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="rfvCap" runat="server" ControlToValidate="rcbCapHoc" CssClass="valid-control" ForeColor="Red" ErrorMessage="Thông tin bắt buộc nhập" Display="Dynamic" SetFocusOnError="true"> </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="rcbTinhThanh" class="col-sm-5 control-label">Tỉnh/Thành phố</label>
                            <div class="col-sm-7">
                                <telerik:RadComboBox ID="rcbTinhThanh" runat="server" Width="100%" DataSourceID="objTinhThanh" DataTextField="TEN" DataValueField="MA" EmptyMessage="Chọn tỉnh" Filter="Contains" CausesValidation="false" OnSelectedIndexChanged="rcbTinhThanh_SelectedIndexChanged" AutoPostBack="true">
                                </telerik:RadComboBox>
                                <asp:ObjectDataSource ID="objTinhThanh" runat="server" SelectMethod="getTinhThanh" TypeName="OneEduDataAccess.BO.DmTinhThanhBO">
                                    <SelectParameters>
                                        <asp:Parameter Name="is_all" DefaultValue="false" DbType="Boolean" />
                                        <asp:Parameter Name="id_all" DefaultValue="0" DbType="Int16" />
                                        <asp:Parameter Name="text_all" DefaultValue="" DbType="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="rcbQuanHuyen" class="col-sm-5 control-label">Quận/huyện</label>
                            <div class="col-sm-7">
                                <telerik:RadComboBox ID="rcbQuanHuyen" runat="server" Width="100%" DataSourceID="objQuanHuyen" DataTextField="TEN" DataValueField="MA" EmptyMessage="Chọn quận/huyện" Filter="Contains" CausesValidation="false" OnSelectedIndexChanged="rcbQuanHuyen_SelectedIndexChanged" AutoPostBack="true">
                                </telerik:RadComboBox>
                                <asp:ObjectDataSource ID="objQuanHuyen" runat="server" SelectMethod="getQuanHuyenByTinh" TypeName="OneEduDataAccess.BO.DmQuanHuyenBO">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="rcbTinhThanh" Name="ma_tinh" PropertyName="SelectedValue" />
                                        <asp:Parameter Name="is_all" DefaultValue="false" DbType="Boolean" />
                                        <asp:Parameter Name="id_all" DefaultValue="0" DbType="Int16" />
                                        <asp:Parameter Name="text_all" DefaultValue="" DbType="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="rcbXaPhuong" class="col-sm-5 control-label">Xã/phường/thị trấn</label>
                            <div class="col-sm-7">
                                <telerik:RadComboBox ID="rcbXaPhuong" runat="server" Width="100%" DataSourceID="objXaPhuong" DataTextField="TEN" DataValueField="MA" EmptyMessage="Chọn xã/phường/thị trấn" Filter="Contains" CausesValidation="false">
                                </telerik:RadComboBox>
                                <asp:ObjectDataSource ID="objXaPhuong" runat="server" SelectMethod="getXaPhuongByTinhHuyen" TypeName="OneEduDataAccess.BO.DmXaPhuongBO">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="rcbTinhThanh" Name="ma_tinh" PropertyName="SelectedValue" DefaultValue="0" />
                                        <asp:ControlParameter ControlID="rcbQuanHuyen" Name="ma_huyen" PropertyName="SelectedValue" DefaultValue="0" />
                                        <asp:Parameter Name="is_all" DefaultValue="false" DbType="Boolean" />
                                        <asp:Parameter Name="id_all" DefaultValue="0" DbType="Int64" />
                                        <asp:Parameter Name="text_all" DefaultValue="" DbType="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbDiaChi" class="col-sm-5 control-label">Địa chỉ</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbDiaChi" CssClass="form-control" ClientIDMode="Static" placeholder="Địa chỉ trường học" runat="server" MaxLength="100"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="tbSDT" class="col-sm-5 control-label">SĐT trường</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbSDT" CssClass="form-control" ClientIDMode="Static" placeholder="Số điện thoại" runat="server" MaxLength="20"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator2" ControlToValidate="tbSDT" ClientValidationFunction="validateDienThoai" ValidateEmptyText="false" runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Dữ liệu không hợp lệ" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbEmail" class="col-sm-5 control-label">Email trường</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbEmail" CssClass="form-control" ClientIDMode="Static" placeholder="Email" runat="server" MaxLength="100"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator7" ControlToValidate="tbEmail" ClientValidationFunction="validateEmails" ValidateEmptyText="false" runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Dữ liệu không hợp lệ" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbHieuTruong" class="col-sm-5 control-label">Hiệu trưởng</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbHieuTruong" CssClass="form-control" ClientIDMode="Static" placeholder="Hiệu trưởng" runat="server" MaxLength="100"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbSDT_HieuTruong" class="col-sm-5 control-label">SĐT hiệu trưởng</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbSDT_HieuTruong" CssClass="form-control" ClientIDMode="Static" placeholder="Số điện thoại hiệu trưởng" runat="server" MaxLength="20"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator4" ControlToValidate="tbSDT_HieuTruong" ClientValidationFunction="validateDienThoai" ValidateEmptyText="false" runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Dữ liệu không hợp lệ" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbEmailHieuTruong" class="col-sm-5 control-label">Email Hiệu trưởng</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbEmailHieuTruong" CssClass="form-control" ClientIDMode="Static" placeholder="Email" runat="server" MaxLength="100"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator6" ControlToValidate="tbEmailHieuTruong" ClientValidationFunction="validateEmails" ValidateEmptyText="false" runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Dữ liệu không hợp lệ" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbSDT_NLH" class="col-sm-5 control-label">SĐT người liên hệ</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbSDT_NLH" CssClass="form-control" ClientIDMode="Static" placeholder="SĐT người liên hệ" runat="server" MaxLength="100"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator5" ControlToValidate="tbSDT_NLH" ClientValidationFunction="validateDienThoai" ValidateEmptyText="false" runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Dữ liệu không hợp lệ" />
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <div class="col-sm-3 col-sm-offset-5">
                                <asp:CheckBox ID="chbIsActive" Text="Đăng ký SMS" CssClass="one-checkbox" runat="server" OnCheckedChanged="chbIsActive_CheckedChanged" AutoPostBack="true"/>
                            </div>
                            <div class="col-sm-4" runat="server" id="divSanQuyTinNam" visible="false">
                                <asp:CheckBox ID="chbIsSanTinNam" Text="San quỹ tin năm" CssClass="one-checkbox" runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-5 control-label">Trạng thái</label>
                            <div class="col-sm-7">
                                <telerik:RadComboBox ID="rcbTrangThai" runat="server" Width="100%" EmptyMessage="Trạng thái trường học" AllowCustomText="true" Filter="Contains">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="1" Text="Hoạt động" Selected="true" />
                                        <telerik:RadComboBoxItem Value="0" Text="Dừng hoạt động" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbThuongHieu" class="col-sm-5 control-label">Thương hiệu Viettel</label>
                            <div class="col-sm-7">
                                <div class="form-group" style="margin-bottom: -15px;">
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="tbThuongHieu" CssClass="form-control" ClientIDMode="Static" placeholder="Tên thương hiệu" runat="server" MaxLength="50" Width="100%"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-6">
                                        <telerik:RadComboBox ID="rcbCPVietTel" runat="server" EnableCheckAllItemsCheckBox="true" Width="100%" EmptyMessage="Chọn đối tác" AllowCustomText="true" Filter="Contains" DataSourceID="objDoiTac" DataTextField="TEN" DataValueField="MA">
                                        </telerik:RadComboBox>
                                        <asp:ObjectDataSource ID="objDoiTac" runat="server" SelectMethod="getTelco" TypeName="OneEduDataAccess.BO.CPTelCoBO">
                                            <SelectParameters>
                                                <asp:Parameter Name="is_all" DefaultValue="false" DbType="Boolean" />
                                                <asp:Parameter Name="id_all" DefaultValue="" DbType="String" />
                                                <asp:Parameter Name="text_all" DefaultValue="" DbType="String" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbVina" class="col-sm-5 control-label">Thương hiệu Vina</label>
                            <div class="col-sm-7">
                                <div class="form-group" style="margin-bottom: -15px;">
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="tbVina" CssClass="form-control" ClientIDMode="Static" placeholder="Tên thương hiệu" runat="server" MaxLength="50"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-6">
                                        <telerik:RadComboBox ID="rcbCPViNa" runat="server" EnableCheckAllItemsCheckBox="true" Width="100%" EmptyMessage="Chọn đối tác" AllowCustomText="true" Filter="Contains" DataSourceID="objDoiTac" DataTextField="TEN" DataValueField="MA">
                                        </telerik:RadComboBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbMobi" class="col-sm-5 control-label">Thương hiệu Mobi</label>
                            <div class="col-sm-7">
                                <div class="form-group" style="margin-bottom: -15px;">
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="tbMobi" CssClass="form-control" ClientIDMode="Static" placeholder="Tên thương hiệu" runat="server" MaxLength="50"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-6">
                                        <telerik:RadComboBox ID="rcbCPMobi" runat="server" EnableCheckAllItemsCheckBox="true" Width="100%" EmptyMessage="Chọn đối tác" AllowCustomText="true" Filter="Contains" DataSourceID="objDoiTac" DataTextField="TEN" DataValueField="MA">
                                        </telerik:RadComboBox>
                                    </div>
                                </div>
                                
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbGtel" class="col-sm-5 control-label">Thương hiệu GTEL</label>
                            <div class="col-sm-7">
                                <div class="form-group" style="margin-bottom: -15px;">
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="tbGtel" CssClass="form-control" ClientIDMode="Static" placeholder="Tên thương hiệu" runat="server" MaxLength="50"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-6">
                                        <telerik:RadComboBox ID="rcbCPGtel" runat="server" EnableCheckAllItemsCheckBox="true" Width="100%" EmptyMessage="Chọn đối tác" AllowCustomText="true" Filter="Contains" DataSourceID="objDoiTac" DataTextField="TEN" DataValueField="MA">
                                        </telerik:RadComboBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbVNM" class="col-sm-5 control-label">Thương hiệu VNMobile</label>
                            <div class="col-sm-7">
                                <div class="form-group" style="margin-bottom:-15px;">
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="tbVNM" CssClass="form-control" ClientIDMode="Static" placeholder="Tên thương hiệu" runat="server" MaxLength="50"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-6">
                                        <telerik:RadComboBox ID="rcbCPVNM" runat="server" EnableCheckAllItemsCheckBox="true" Width="100%" EmptyMessage="Chọn đối tác" AllowCustomText="true" Filter="Contains" DataSourceID="objDoiTac" DataTextField="TEN" DataValueField="MA">
                                        </telerik:RadComboBox>
                                    </div>
                                </div>
                                
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-5 control-label">Mã gói tin</label>
                            <div class="col-sm-7">
                                <telerik:RadComboBox ID="rcbMaGoiTin" runat="server" Width="100%" EmptyMessage="Chọn mã gói tin" DataSourceID="objMaGoiTin"
                                    DataTextField="TEN" DataValueField="MA" AllowCustomText="true" Filter="Contains">
                                </telerik:RadComboBox>
                                <asp:ObjectDataSource ID="objMaGoiTin" runat="server" SelectMethod="getGoiTin" TypeName="OneEduDataAccess.BO.GoiTinBO">
                                    <SelectParameters>
                                        <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                                        <asp:Parameter DefaultValue="" Name="id_all" Type="Int16" />
                                        <asp:Parameter Name="text_all" Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbSoHSDangKy" class="col-sm-5 control-label">Số học sinh đăng ký</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbSoHSDangKy" CssClass="form-control" ClientIDMode="Static" placeholder="Số học sinh đăng ký" runat="server" MaxLength="4" TextMode="Number"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbSoHSDuocMien" class="col-sm-5 control-label">Số học sinh miễn phí</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbSoHSDuocMien" CssClass="form-control" ClientIDMode="Static" placeholder="Số học sinh miễn phí" runat="server" MaxLength="4" TextMode="Number"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbPhanTramVuot" class="col-sm-5 control-label">Phần trăm vượt hạn mức</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbPhanTramVuot" CssClass="form-control" ClientIDMode="Static" placeholder="% vượt hạn mức" runat="server" MaxLength="4" TextMode="Number"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-5 control-label">Đại lý</label>
                            <div class="col-sm-7">
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
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-5 control-label">Tổng tin cấp</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbTongTinCap" CssClass="form-control" ClientIDMode="Static" placeholder="Tổng tin cấp" runat="server" MaxLength="4" TextMode="Number" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-5 control-label">Tổng tin đã sử dụng</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbTongTinSuDung" CssClass="form-control" ClientIDMode="Static" placeholder="Tổng tin đã sử dụng" runat="server" MaxLength="4" TextMode="Number" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
