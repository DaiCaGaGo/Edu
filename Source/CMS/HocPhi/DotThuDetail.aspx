<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="DotThuDetail.aspx.cs" Inherits="CMS.HocPhi.DotThuDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rcbLoaiDotThu">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divHocKy" />
                    <telerik:AjaxUpdatedControl ControlID="divThang" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="cboTienAn">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tbTienAn" />
                </UpdatedControls>
            </telerik:AjaxSetting>
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
                            <label for="tbTen" class="col-sm-3 control-label">Tên đợt thu <span style="color: red">(*)</span></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbTen" CssClass="form-control" ClientIDMode="Static" placeholder="Tên đợt thu" runat="server" MaxLength="50" CausesValidation="false"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator1" ControlToValidate="tbTen" ClientValidationFunction="validateMaxchar" ValidateEmptyText="false"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Thông tin bắt buộc, nhập không quá 250 ký tự." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbNoiDung" class="col-sm-3 control-label">Nội dung <span style="color: red">(*)</span></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbNoiDung" CssClass="form-control" ClientIDMode="Static" placeholder="Nội dung thu" runat="server" MaxLength="50" CausesValidation="false"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator3" ControlToValidate="tbTen" ClientValidationFunction="validateMaxchar" ValidateEmptyText="false"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Thông tin bắt buộc, nhập không quá 500 ký tự." />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-9 col-sm-offset-3">
                                <div class="col-sm-3" style="margin-left: -15px;">
                                    <div class="one-checkbox">
                                        <label>
                                            <asp:CheckBox ID="cboTienAn" runat="server" Text="Tiền ăn" OnCheckedChanged="cboTienAn_CheckedChanged" AutoPostBack="true" />
                                        </label>
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <telerik:RadNumericTextBox ID="tbTienAn" CssClass="form-control" ClientIDMode="Static" placeholder="Tiền ăn" runat="server" MinValue="0" MaxValue="1000000000" MaxLength="10" Width="100%" Enabled="false"></telerik:RadNumericTextBox>
                                </div>
                                <div class="col-sm-2" style="padding-top: 8px;">
                                    <asp:Label ID="Label1" runat="server" Text="" Style="margin-left: -20px;">(VNĐ)</asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Tổng tiền</label>
                            <div class="col-sm-5">
                                <telerik:RadNumericTextBox ID="tbTongTien" CssClass="form-control" ClientIDMode="Static" placeholder="Tổng tiền" runat="server" MinValue="0" MaxValue="1000000000" MaxLength="10" Width="100%"></telerik:RadNumericTextBox>
                            </div>
                            <div class="col-sm-2" style="padding-top: 8px;">
                                <asp:Label ID="Label2" runat="server" Text="" Style="margin-left: -20px;">(VNĐ)</asp:Label>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="rcbLoaiDotThu" class="col-sm-3 control-label">Đợt thu</label>
                            <div class="col-sm-9">
                                <telerik:RadComboBox ID="rcbLoaiDotThu" runat="server" Width="100%" Filter="Contains" EmptyMessage="Chọn loại đợt thu" AllowCustomText="true" OnSelectedIndexChanged="rcbLoaiDotThu_SelectedIndexChanged1" AutoPostBack="true">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="" Text="Chọn đợt thu" />
                                        <telerik:RadComboBoxItem Value="1" Text="Theo năm" />
                                        <telerik:RadComboBoxItem Value="2" Text="Theo kỳ" />
                                        <telerik:RadComboBoxItem Value="3" Text="Theo tháng" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                        <div class="form-group" runat="server" id="divHocKy" visible="false">
                            <label for="rcbHocKy" class="col-sm-3 control-label">Học kỳ</label>
                            <div class="col-sm-9">
                                <telerik:RadComboBox ID="rcbHocKy" runat="server" Width="100%" Filter="Contains">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="1" Text="HK 1" />
                                        <telerik:RadComboBoxItem Value="2" Text="HK 2" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                        <div class="form-group" runat="server" id="divThang" visible="false">
                            <label for="rcbThang" class="col-sm-3 control-label">Tháng</label>
                            <div class="col-sm-9">
                                <telerik:RadComboBox ID="rcbThang" runat="server" Width="100%" Filter="Contains">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="1" Text="Tháng 1" />
                                        <telerik:RadComboBoxItem Value="2" Text="Tháng 2" />
                                        <telerik:RadComboBoxItem Value="3" Text="Tháng 3" />
                                        <telerik:RadComboBoxItem Value="4" Text="Tháng 4" />
                                        <telerik:RadComboBoxItem Value="5" Text="Tháng 5" />
                                        <telerik:RadComboBoxItem Value="6" Text="Tháng 6" />
                                        <telerik:RadComboBoxItem Value="7" Text="Tháng 7" />
                                        <telerik:RadComboBoxItem Value="8" Text="Tháng 8" />
                                        <telerik:RadComboBoxItem Value="9" Text="Tháng 9" />
                                        <telerik:RadComboBoxItem Value="10" Text="Tháng 10" />
                                        <telerik:RadComboBoxItem Value="11" Text="Tháng 11" />
                                        <telerik:RadComboBoxItem Value="12" Text="Tháng 12" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Thời gian</label>
                            <div class="col-sm-9">
                                <telerik:RadDatePicker Culture="vi-VN" RenderMode="Classic" ID="rdTuNgay" runat="server" Width="30%" MinDate="1900/1/1"
                                    Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="Giá trị không hợp lệ"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="Hôm nay"
                                    DatePopupButton-ToolTip="Ngày bắt đầu"
                                    Calendar-FastNavigationSettings-OkButtonCaption="Chọn"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="Hủy">
                                    <DateInput DateFormat="dd/MM/yyyy"></DateInput>
                                </telerik:RadDatePicker>
                                &nbsp;&nbsp;--&nbsp;&nbsp;
                                <telerik:RadDatePicker Culture="vi-VN" RenderMode="Classic" ID="rdDenNgay" runat="server" Width="30%" MinDate="1900/1/1"
                                    Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="Giá trị không hợp lệ"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="Hôm nay"
                                    DatePopupButton-ToolTip="Ngày kết thúc"
                                    Calendar-FastNavigationSettings-OkButtonCaption="Chọn"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="Hủy">
                                    <DateInput DateFormat="dd/MM/yyyy"></DateInput>
                                </telerik:RadDatePicker>
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
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
