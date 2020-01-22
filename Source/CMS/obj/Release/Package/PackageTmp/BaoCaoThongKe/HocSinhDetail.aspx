<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="HocSinhDetail.aspx.cs" Inherits="CMS.BaoCaoThongKe.HocSinhDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rcbKhoi">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbLop" />
                    <telerik:AjaxUpdatedControl ControlID="tbThuTu" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbLop">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tbThuTu" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbLoaiLopGDTX">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbKhoi" />
                    <telerik:AjaxUpdatedControl ControlID="rcbLop" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="cbMienPhi1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divNhanTinBoMe" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="cbMienPhi2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divNhanTinBoMe" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="cboGuiBoMe">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divSDTKhac" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <telerik:RadCodeBlock runat="server">
        <style>
            fieldset .content {
                display: none;
            }
        </style>
        <script>
            $(function () {
                $('legend').click(function () {
                    $(this).parent().find('.content').slideToggle("slow");
                });
            });
        </script>
    </telerik:RadCodeBlock>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">CHI TIẾT HỌC SINH</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btEdit" runat="server" CssClass="btn bt-one" OnClick="btEdit_Click" Text="Sửa" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="form-horizontal" role="form">
                <div class="row">
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label for="tbMa" class="col-sm-5 control-label">Mã</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbMa" CssClass="form-control" ClientIDMode="Static" placeholder="Mã" runat="server" MaxLength="100" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbTen" class="col-sm-5 control-label">Tên <span style="color: red">(*)</span></label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbTen" CssClass="form-control" ClientIDMode="Static" placeholder="Tên" runat="server" MaxLength="100"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator3" ControlToValidate="tbTen" ClientValidationFunction="validateMaxchar" ValidateEmptyText="true"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Thông tin bắt buộc, nhập không quá 100 ký tự." />
                            </div>
                        </div>
                        <div class="form-group" runat="server" id="divLoaiGDTX">
                            <label for="tbTen" class="col-sm-5 control-label">Loại lớp GDTX <span style="color: red">(*)</span></label>
                            <div class="col-sm-7">
                                <telerik:RadComboBox ID="rcbLoaiLopGDTX" runat="server" Width="100%" AutoPostBack="True"
                                    OnSelectedIndexChanged="rcbLoaiLopGDTX_SelectedIndexChanged" CausesValidation="false" Filter="Contains">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="2" Text="Bổ túc THCS" />
                                        <telerik:RadComboBoxItem Value="3" Text="Bổ túc THPT" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="rcbKhoi" class="col-sm-5 control-label">Khối <span style="color: red">(*)</span></label>
                            <div class="col-sm-7">
                                <telerik:RadComboBox ID="rcbKhoi" runat="server" Width="100%" DataTextField="TEN" DataValueField="MA" AutoPostBack="True" Filter="Contains" CausesValidation="false" DataSourceID="objKhoi" OnSelectedIndexChanged="rcbKhoi_SelectedIndexChanged">
                                </telerik:RadComboBox>
                                <asp:ObjectDataSource ID="objKhoi" runat="server" SelectMethod="getKhoiByCapHoc" TypeName="OneEduDataAccess.BO.KhoiBO">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="rcbLoaiLopGDTX" Name="maLoaiLopGDTX" PropertyName="SelectedValue" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="rcbLop" class="col-sm-5 control-label">Lớp <span style="color: red">(*)</span></label>
                            <div class="col-sm-7">
                                <telerik:RadComboBox ID="rcbLop" runat="server" Width="100%" DataSourceID="objLop" DataTextField="TEN" DataValueField="ID" Filter="Contains"  CausesValidation="false">
                                </telerik:RadComboBox>
                                <asp:ObjectDataSource ID="objLop" runat="server" SelectMethod="getLopByKhoiNamHoc" TypeName="OneEduDataAccess.BO.LopBO">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="rcbKhoi" Name="maKhoi" PropertyName="SelectedValue" />
                                        <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                                        <asp:Parameter DefaultValue="" Name="id_all" Type="Int64" />
                                        <asp:Parameter Name="text_all" Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="rcbLop"
                                    CssClass="valid-control" ForeColor="Red" ErrorMessage="Thông tin bắt buộc nhập." Display="Dynamic"
                                    SetFocusOnError="true"> </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbSDT_NhanTin" class="col-sm-5 control-label">SĐT nhận tin</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbSDT_NhanTin" CssClass="form-control" ClientIDMode="Static" placeholder="Nhập SĐT nhận tin" runat="server" MaxLength="20"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator2" ControlToValidate="tbSDT_NhanTin" ClientValidationFunction="validateDienThoai" ValidateEmptyText="false" runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Dữ liệu không hợp lệ." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="rdNgaySinh" class="col-sm-5 control-label">Ngày sinh <%--<span style="color: red">(*)</span>--%></label>
                            <div class="col-sm-7">
                                <telerik:RadDatePicker Culture="vi-VN" RenderMode="Classic" ID="rdNgaySinh" runat="server" Width="100%" MinDate="1900/1/1"
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
                            <label for="rcbGioiTinh" class="col-sm-5 control-label">Giới tính</label>
                            <div class="col-sm-7">
                                <telerik:RadComboBox ID="rcbGioiTinh" runat="server" Width="100%" Filter="Contains">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="1" Text="Nam" />
                                        <telerik:RadComboBoxItem Value="2" Text="Nữ" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbThuTu" class="col-sm-5 control-label">Thứ tự</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbThuTu" CssClass="form-control" ClientIDMode="Static" placeholder="Thứ tự" runat="server" MaxLength="4" TextMode="Number"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-5 control-label">Trạng thái</label>
                            <div class="col-sm-7">
                                <telerik:RadComboBox ID="rcbTrangThai" runat="server" Width="100%" DataSourceID="objTrangThai" DataTextField="TEN" DataValueField="MA" Filter="Contains">
                                </telerik:RadComboBox>
                                <asp:ObjectDataSource ID="objTrangThai" runat="server" SelectMethod="getTrangThaiHS" TypeName="OneEduDataAccess.BO.TrangThaiHSBO">
                                    <SelectParameters>
                                        <asp:Parameter Name="is_all" DbType="Boolean" DefaultValue="False" />
                                        <asp:Parameter Name="id_all" DbType="Int16" DefaultValue="0" />
                                        <asp:Parameter Name="text_all" DbType="String" DefaultValue="" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-5 control-label">Con giáo viên</label>
                            <div class="col-sm-7">
                                <div class="one-checkbox">
                                    <label>
                                        <asp:CheckBox ID="cbConGV" runat="server" Text="Là con giáo viên" />
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-5 control-label">Chi hội phụ huynh</label>
                            <div class="col-sm-3">
                                <div class="one-checkbox">
                                    <label>
                                        <asp:CheckBox ID="cbHoiTruong" runat="server" Text="Hội trưởng"/>
                                    </label>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="one-checkbox">
                                    <label>
                                        <asp:CheckBox ID="cbHoiPho" runat="server" Text="Hội phó"/>
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-5 control-label">Đăng ký SMS</label>
                            <div class="col-sm-3">
                                <div class="one-checkbox">
                                    <label>
                                        <asp:CheckBox ID="cbSMS1" runat="server" Text="Kỳ I" />
                                    </label>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="one-checkbox">
                                    <label>
                                        <asp:CheckBox ID="cbSMS2" runat="server" Text="Kỳ II" />
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-5 control-label">Miễn phí SMS</label>
                            <div class="col-sm-3">
                                <div class="one-checkbox">
                                    <label>
                                        <asp:CheckBox ID="cbMienPhi1" runat="server" Text="Kỳ I" OnCheckedChanged="cbMienPhi1_CheckedChanged" AutoPostBack="true" />
                                    </label>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="one-checkbox">
                                    <label>
                                        <asp:CheckBox ID="cbMienPhi2" runat="server" Text="Kỳ II" OnCheckedChanged="cbMienPhi2_CheckedChanged" AutoPostBack="true" />
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div class="form-group" runat="server" id="divNhanTinBoMe">
                            <label class="col-sm-5 control-label">Nhắn tin cả bố và mẹ</label>
                            <div class="col-sm-7">
                                <div class="one-checkbox">
                                    <label>
                                        <asp:CheckBox ID="cboGuiBoMe" runat="server" Text="Có" OnCheckedChanged="cboGuiBoMe_CheckedChanged" AutoPostBack="true" />
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div class="form-group" runat="server" id="divSDTKhac" visible="false">
                            <label for="tbSDT_NhanTin" class="col-sm-5 control-label">SĐT nhận tin khác <span style="color: red">(*)</span></label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbSDT_NhanTinKhac" CssClass="form-control" ClientIDMode="Static" placeholder="Nhập SĐT nhận tin" runat="server" MaxLength="20"></asp:TextBox>
                                <asp:CustomValidator ID="cusValidSDTKhac" ControlToValidate="tbSDT_NhanTinKhac" ClientValidationFunction="validateDienThoai" ValidateEmptyText="false" runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Dữ liệu không hợp lệ." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-5 control-label">Chiều cao</label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="tbChieuCao" CssClass="form-control" ClientIDMode="Static" placeholder="Chiều cao" runat="server" MaxLength="20"></asp:TextBox>
                            </div>
                            <div class="col-sm-2" style="margin-left:-20px; margin-top:10px;">(cm)</div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-5 control-label">Cân nặng</label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="tbCanNang" CssClass="form-control" ClientIDMode="Static" placeholder="Cân nặng" runat="server" MaxLength="20"></asp:TextBox>
                            </div>
                            <div class="col-sm-2" style="margin-left:-20px; margin-top:10px;">(kg)</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>