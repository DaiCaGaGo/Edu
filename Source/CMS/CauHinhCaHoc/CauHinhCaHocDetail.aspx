<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="CauHinhCaHocDetail.aspx.cs" Inherits="CMS.CauHinhCaHoc.CauHinhCaHocDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadCodeBlock runat="server">
        <script type="text/javascript">
            $(function () {
                $('.datepicker').datepicker({
                    format: 'dd/mm/yyyy',
                    todayHighlight: true,
                    autoclose: true
                });
            });

            $(function () {
                $('.ngay').datetimepicker({
                    format: 'HH:mm'
                });
            });
        </script>
        <style>
            .control-label{
                padding-left:0;
            }
        </style>
    </telerik:RadCodeBlock>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">Quản Lý Cấu Hình Ca Học</span>
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
                    <div class="col-sm-4">
                        <div class="form-group">
                            <label for="tbNgayBatDau" class="col-sm-6 control-label">Ngày bắt đầu <span style="color: red">(*)</span></label>
                            <div class="input-group date col-sm-6 datepicker">
                                <telerik:RadDatePicker Culture="vi-VN" RenderMode="Classic" ID="rdNgayBatDau" runat="server" Width="98%" MinDate="1900/1/1" DateInput-EmptyMessage="01/01/2000"
                                    Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="Giá trị không hợp lệ"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="Hôm nay"
                                    DatePopupButton-ToolTip="Chọn ngày"
                                    Calendar-FastNavigationSettings-OkButtonCaption="Chọn"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="Hủy">
                                    <DateInput runat="server" DateFormat="dd/MM/yyyy"></DateInput>
                                </telerik:RadDatePicker>
                                <asp:RequiredFieldValidator ID="validrdNgayBatDau" runat="server" ControlToValidate="rdNgayBatDau"
                                    CssClass="valid-control" ForeColor="Red" ErrorMessage="Thông tin bắt buộc nhập." Display="Dynamic"
                                    SetFocusOnError="true"> </asp:RequiredFieldValidator>
                                <asp:HiddenField ID="hdNgayBatDau" runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-6 control-label">Thời gian tiết 1</label>
                            <div class="input-group date col-sm-6 ngay">
                                <telerik:RadTimePicker ID="rdTiet1" runat="server" Width="98%" TimeView-Interval="45">
                                    <DateInput runat="server" DateFormat="HH:mm"></DateInput>
                                </telerik:RadTimePicker>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-6 control-label">Thời gian tiết 2</label>
                            <div class="input-group date col-sm-6 ngay">
                                <telerik:RadTimePicker ID="rdTiet2" runat="server" Width="98%" TimeView-Interval="45">
                                    <DateInput runat="server" DateFormat="HH:mm"></DateInput>
                                </telerik:RadTimePicker>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-6 control-label">Thời gian tiết 3</label>
                            <div class="input-group date col-sm-6 ngay">
                                <telerik:RadTimePicker ID="rdTiet3" runat="server" Width="98%" TimeView-Interval="45">
                                    <DateInput runat="server" DateFormat="HH:mm"></DateInput>
                                </telerik:RadTimePicker>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-6 control-label">Thời gian tiết 4</label>
                            <div class="input-group date col-sm-6 ngay">
                                <telerik:RadTimePicker ID="rdTiet4" runat="server" Width="98%" TimeView-Interval="45">
                                    <DateInput runat="server" DateFormat="HH:mm"></DateInput>
                                </telerik:RadTimePicker>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-6 control-label">Thời gian tiết 5</label>
                            <div class="input-group date col-sm-6 ngay">
                                <telerik:RadTimePicker ID="rdTiet5" runat="server" Width="98%" TimeView-Interval="45">
                                    <DateInput runat="server" DateFormat="HH:mm"></DateInput>
                                </telerik:RadTimePicker>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="form-group">
                            <label for="tbNgayKetThuc" class="col-sm-6 control-label">Ngày kết thúc <span style="color: red">(*)</span></label>
                            <div class="input-group date col-sm-6 datepicker">
                                <telerik:RadDatePicker Culture="vi-VN" RenderMode="Classic" ID="rdNgayKetThuc" runat="server" Width="98%" MinDate="1900/1/1" DateInput-EmptyMessage="01/01/2001"
                                    Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="Giá trị không hợp lệ"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="Hôm nay"
                                    DatePopupButton-ToolTip="Chọn ngày"
                                    Calendar-FastNavigationSettings-OkButtonCaption="Chọn"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="Hủy">
                                    <DateInput runat="server" DateFormat="dd/MM/yyyy"></DateInput>
                                </telerik:RadDatePicker>
                                <asp:RequiredFieldValidator ID="validrdNgayKetThuc" runat="server" ControlToValidate="rdNgayKetThuc"
                                    CssClass="valid-control" ForeColor="Red" ErrorMessage="Thông tin bắt buộc nhập." Display="Dynamic"
                                    SetFocusOnError="true"> </asp:RequiredFieldValidator>
                                <asp:HiddenField ID="hdNgayKetThuc" runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-6 control-label">Thời gian tiết 6</label>
                            <div class="input-group date col-sm-6 ngay">
                                <telerik:RadTimePicker ID="rdTiet6" runat="server" Width="98%" TimeView-Interval="45">
                                    <DateInput runat="server" DateFormat="HH:mm"></DateInput>
                                </telerik:RadTimePicker>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-6 control-label">Thời gian tiết 7</label>
                            <div class="input-group date col-sm-6 ngay">
                                <telerik:RadTimePicker ID="rdTiet7" runat="server" Width="98%" TimeView-Interval="45">
                                    <DateInput runat="server" DateFormat="HH:mm"></DateInput>
                                </telerik:RadTimePicker>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-6 control-label">Thời gian tiết 8</label>
                            <div class="input-group date col-sm-6 ngay">
                                <telerik:RadTimePicker ID="rdTiet8" runat="server" Width="98%" TimeView-Interval="45">
                                    <DateInput runat="server" DateFormat="HH:mm"></DateInput>
                                </telerik:RadTimePicker>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-6 control-label">Thời gian tiết 9</label>
                            <div class="input-group date col-sm-6 ngay">
                                <telerik:RadTimePicker ID="rdTiet9" runat="server" Width="98%" TimeView-Interval="45">
                                    <DateInput runat="server" DateFormat="HH:mm"></DateInput>
                                </telerik:RadTimePicker>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-6 control-label">Thời gian tiết 10</label>
                            <div class="input-group date col-sm-6 ngay">
                                <telerik:RadTimePicker ID="rdTiet10" runat="server" Width="98%" TimeView-Interval="45">
                                    <DateInput runat="server" DateFormat="HH:mm"></DateInput>
                                </telerik:RadTimePicker>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="form-group">
                            <label for="tbThoiGian" class="col-sm-6 control-label">Tên cấu hình <span style="color: red">(*)</span></label>
                            <div class="input-group date col-sm-6">
                                <asp:TextBox ID="tbTenCauHinh" CssClass="form-control" ClientIDMode="Static" placeholder="Nhập tên cấu hình" runat="server" MaxLength="100" Width="98%"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator3" ControlToValidate="tbTenCauHinh" ClientValidationFunction="validateMaxchar" ValidateEmptyText="true"
                                    runat="server" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Thông tin bắt buộc, nhập không quá 100 ký tự." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-6 control-label">Thời gian tiết 11</label>
                            <div class="input-group date col-sm-6 ngay">
                                <telerik:RadTimePicker ID="rdTiet11" runat="server" Width="98%" TimeView-Interval="45">
                                    <DateInput runat="server" DateFormat="HH:mm"></DateInput>
                                </telerik:RadTimePicker>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-6 control-label">Thời gian tiết 12</label>
                            <div class="input-group date col-sm-6 ngay">
                                <telerik:RadTimePicker ID="rdTiet12" runat="server" Width="98%" TimeView-Interval="45">
                                    <DateInput runat="server" DateFormat="HH:mm"></DateInput>
                                </telerik:RadTimePicker>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-6 control-label">Thời gian tiết 13</label>
                            <div class="input-group date col-sm-6 ngay">
                                <telerik:RadTimePicker ID="rdTiet13" runat="server" Width="98%" TimeView-Interval="45">
                                    <DateInput runat="server" DateFormat="HH:mm"></DateInput>
                                </telerik:RadTimePicker>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-6 control-label">Thời gian tiết 14</label>
                            <div class="input-group date col-sm-6 ngay">
                                <telerik:RadTimePicker ID="rdTiet14" runat="server" Width="98%" TimeView-Interval="45">
                                    <DateInput runat="server" DateFormat="HH:mm"></DateInput>
                                </telerik:RadTimePicker>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-6 control-label">Thời gian tiết 15</label>
                            <div class="input-group date col-sm-6 ngay">
                                <telerik:RadTimePicker ID="rdTiet15" runat="server" Width="98%" TimeView-Interval="45">
                                    <DateInput runat="server" DateFormat="HH:mm"></DateInput>
                                </telerik:RadTimePicker>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
