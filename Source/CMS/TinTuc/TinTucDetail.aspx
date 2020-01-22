<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="TinTucDetail.aspx.cs" Inherits="CMS.TinTuc.TinTucDetail" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="cbTaoBai">
                <UpdatedControls>
                    <%--<telerik:AjaxUpdatedControl ControlID="divNoiDung" />--%>
                    <%--<telerik:AjaxUpdatedControl ControlID="divLink" />--%>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <telerik:RadCodeBlock runat="server"></telerik:RadCodeBlock>
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
                            <label class="col-sm-2 control-label">Tiêu đề <span style="color: red">(*)</span></label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="tbTieuDe" CssClass="form-control" ClientIDMode="Static" placeholder="Tiêu đề" runat="server" MaxLength="50"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator1" ControlToValidate="tbTieuDe" ClientValidationFunction="validateMaxchar" ValidateEmptyText="true"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Thông tin bắt buộc, nhập không quá 250 ký tự." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label">Nội dung tóm tắt <span style="color: red">(*)</span></label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="tbTomtat" CssClass="form-control" ClientIDMode="Static" placeholder="Nội dung tóm tắt" runat="server" MaxLength="500" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator5" ControlToValidate="tbTomtat" ClientValidationFunction="validateMaxchar" ValidateEmptyText="true"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Thông tin bắt buộc, nhập không quá 200 ký tự." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label">Ảnh đại diện</label>
                            <div class="col-sm-3">
                                <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" CssClass="async-attachment" ID="rauAnh"
                                    HideFileInput="true"
                                    AllowedFileExtensions=".jpeg,.jpg,.png,.gif" />
                            </div>
                            <div class="col-sm-4">
                                <asp:Image ID="imgAnh" runat="server" Style="width: 150px; height: 150px;" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label">Tạo bài viết</label>
                            <div class="col-sm-7">
                                <div class="one-checkbox">
                                    <label>
                                        <asp:CheckBox ID="cbTaoBai" runat="server" OnCheckedChanged="cbTaoBai_CheckedChanged" AutoPostBack="true"/>
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div class="form-group" runat="server" id="divNoiDung">
                            <label class="col-sm-2 control-label">Nội dung <span style="color: red">(*)</span></label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="tbNoiDung" CssClass="form-control" ClientIDMode="Static" placeholder="Nội dung" runat="server" MaxLength="500" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                <%--<asp:CustomValidator ID="CustomValidator3" ControlToValidate="tbNoiDung" ClientValidationFunction="validateMaxchar" ValidateEmptyText="true"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Thông tin bắt buộc, nhập không quá 200 ký tự." />--%>
                            </div>
                        </div>
                        <div class="form-group" runat="server" id="divLink">
                            <label class="col-sm-2 control-label">Link <span style="color: red">(*)</span></label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="tbLink" CssClass="form-control" ClientIDMode="Static" placeholder="Link liên kết" runat="server" MaxLength="250"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator4" ControlToValidate="tbLink" ClientValidationFunction="validateMaxchar" ValidateEmptyText="true"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Thông tin bắt buộc, nhập không quá 250 ký tự." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label">Ngày hiệu lực</label>
                            <div class="col-sm-3">
                                <telerik:RadDatePicker Culture="vi-VN" RenderMode="Classic" ID="rdNgayHieuLuc" runat="server" Width="100%" MinDate="1900/1/1"
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
                            <label for="tbThuTu" class="col-sm-2 control-label">Thứ tự</label>
                            <div class="col-sm-3">
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

    <script>
        var baseUrlCustomer = '<%= "http://"+HttpContext.Current.Request.Url.Authority %>/';
        var editor = CKEDITOR.replace('tbNoiDung', {
            customConfig: '<%= "http://"+HttpContext.Current.Request.Url.Authority %>/Assets/ckeditor/config.js'
        });
    </script>
</asp:Content>
