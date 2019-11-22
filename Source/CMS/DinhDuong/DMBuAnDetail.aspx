<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="DMBuAnDetail.aspx.cs" Inherits="CMS.DinhDuong.DMBuAnDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <telerik:RadCodeBlock runat="server">
    </telerik:RadCodeBlock>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">CHI TIẾT BỮA ĂN</span>
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
                        <div class="row">
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label class="col-sm-6 control-label">Tên khối <span style="color: red">(*)</span></label>
                                    <div class="col-sm-6">
                                        <telerik:RadComboBox ID="rcbKhoiHoc" runat="server" Width="100%" DataSourceID="objKhoi"
                                            DataTextField="TEN" DataValueField="MA" EmptyMessage="Chọn khối học" AllowCustomText="true" Filter="Contains" CausesValidation="false">
                                        </telerik:RadComboBox>
                                        <asp:ObjectDataSource ID="objKhoi" runat="server" SelectMethod="getKhoiByCapHoc" TypeName="OneEduDataAccess.BO.KhoiBO">
                                            <SelectParameters>
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                        <asp:RequiredFieldValidator ID="rfvKhoi" runat="server" ControlToValidate="rcbKhoiHoc" CssClass="valid-control" ForeColor="Red" ErrorMessage="Thông tin bắt buộc nhập" Display="Dynamic" SetFocusOnError="true"> </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="tbTen" class="col-sm-6 control-label">Tên bữa ăn <span style="color: red">(*)</span></label>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="tbTen" CssClass="form-control" ClientIDMode="Static" placeholder="Nhập tên bữa ăn" runat="server" MaxLength="150"></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidator3" ControlToValidate="tbTen" ClientValidationFunction="validateMaxchar" ValidateEmptyText="true"
                                            runat="server" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Thông tin bắt buộc, nhập không quá 50 ký tự." />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="tbThuTu" class="col-sm-6 control-label">Thứ tự</label>
                                    <div class="col-sm-6">
                                        <telerik:RadNumericTextBox ID="tbThuTu" CssClass="form-control" ClientIDMode="Static" NumberFormat-DecimalDigits="0" placeholder="Thứ tự" runat="server" MinValue="0" MaxValue="1000" MaxLength="4" Width="100%"></telerik:RadNumericTextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="tbGia" class="col-sm-6 control-label">Giá cho 1 học sinh <span style="color: red">(*)</span></label>
                                    <div class="col-sm-6">
                                        <telerik:RadNumericTextBox ID="tbGia" CssClass="form-control" ClientIDMode="Static" placeholder="Giá" runat="server" MinValue="0" MaxValue="100000" MaxLength="6" Width="100%"></telerik:RadNumericTextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="tbGia" CssClass="valid-control" ForeColor="Red" ErrorMessage="Thông tin bắt buộc nhập" Display="Dynamic" SetFocusOnError="true"> </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                + Năng lượng phân phối cho các bữa ăn (% năng lượng/ngày):
                                <br />
                                &nbsp;&nbsp;- <span style="color: red">Bữa ăn buổi trưa</span> cung cấp từ <span style="color: red">30% đến 35%</span> năng lượng cả ngày.
                                <br />
                                &nbsp;&nbsp;- <span style="color: red">Bữa ăn buổi chiều</span> cung cấp từ <span style="color: red">25% đến 30%</span> năng lượng cả ngày.
                                <br />
                                &nbsp;&nbsp;- <span style="color: red">Bữa phụ</span> cung cấp khoảng <span style="color: red">5% đến 10%</span> năng lượng cả ngày.<br />
                                + Tỉ lệ các chất cung cấp năng lượng được khuyến nghị theo cơ cấu:
                                <br />
                                &nbsp;&nbsp;- <span style="color: red">Chất đạm (Protit)</span> cung cấp khoảng <span style="color: red">13% - 20%</span> năng lượng khẩu phần.
                                <br />
                                &nbsp;&nbsp;- <span style="color: red">Chất béo (Lipit)</span> cung cấp khoảng <span style="color: red">30% - 35%</span> năng lượng khẩu phần.
                                <br />
                                &nbsp;&nbsp;- <span style="color: red">Chất bột (Gluxit)</span> cung cấp khoảng <span style="color: red">47% - 50%</span> năng lượng khẩu phần. 
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label for="rntKcalTu" class="col-sm-6 control-label">Kcal <span style="color: red">(*)</span></label>
                                    <div class="col-sm-6">
                                        <telerik:RadNumericTextBox ID="rntKcalTu" CssClass="form-control" Width="35%" ClientIDMode="Static" placeholder="Kcal" runat="server" MinValue="0" MaxValue="100" MaxLength="6"></telerik:RadNumericTextBox>
                                        &nbsp;đến &nbsp;
                                        <telerik:RadNumericTextBox ID="rntKcalDen" CssClass="form-control" Width="35%" ClientIDMode="Static" placeholder="Kcal" runat="server" MinValue="0" MaxValue="100" MaxLength="6"></telerik:RadNumericTextBox>
                                        (%)
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="rntKcalTu" CssClass="valid-control" ForeColor="Red" ErrorMessage="Thông tin bắt buộc nhập" Display="Dynamic" SetFocusOnError="true"> </asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="rntKcalDen" CssClass="valid-control" ForeColor="Red" ErrorMessage="Thông tin bắt buộc nhập" Display="Dynamic" SetFocusOnError="true"> </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="rntProtitTu" class="col-sm-6 control-label">Protit <span style="color: red">(*)</span></label>
                                    <div class="col-sm-6">
                                        <telerik:RadNumericTextBox ID="rntProtitTu" CssClass="form-control" Width="35%" Value="13" ClientIDMode="Static" placeholder="Protit" runat="server" MinValue="0" MaxValue="100" MaxLength="6"></telerik:RadNumericTextBox>
                                        &nbsp;đến &nbsp;
                                        <telerik:RadNumericTextBox ID="rntProtitDen" CssClass="form-control" Width="35%" Value="20" ClientIDMode="Static" placeholder="Protit" runat="server" MinValue="0" MaxValue="100" MaxLength="6"></telerik:RadNumericTextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="rntProtitTu" CssClass="valid-control" ForeColor="Red" ErrorMessage="Thông tin bắt buộc nhập" Display="Dynamic" SetFocusOnError="true"> </asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="rntProtitDen" CssClass="valid-control" ForeColor="Red" ErrorMessage="Thông tin bắt buộc nhập" Display="Dynamic" SetFocusOnError="true"> </asp:RequiredFieldValidator>
                                        (%)
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="rntLipitTu" class="col-sm-6 control-label">Lipit <span style="color: red">(*)</span></label>
                                    <div class="col-sm-6">
                                        <telerik:RadNumericTextBox ID="rntLipitTu" CssClass="form-control" Width="35%" Value="30" ClientIDMode="Static" placeholder="Lipit" runat="server" MinValue="0" MaxValue="100" MaxLength="6"></telerik:RadNumericTextBox>
                                        &nbsp;đến &nbsp;
                                        <telerik:RadNumericTextBox ID="rntLipitDen" CssClass="form-control" Width="35%" Value="40" ClientIDMode="Static" placeholder="Lipit" runat="server" MinValue="0" MaxValue="100" MaxLength="6"></telerik:RadNumericTextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="rntLipitTu" CssClass="valid-control" ForeColor="Red" ErrorMessage="Thông tin bắt buộc nhập" Display="Dynamic" SetFocusOnError="true"> </asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="rntLipitDen" CssClass="valid-control" ForeColor="Red" ErrorMessage="Thông tin bắt buộc nhập" Display="Dynamic" SetFocusOnError="true"> </asp:RequiredFieldValidator>
                                        (%)
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="rntGluxitTu" class="col-sm-6 control-label">Gluxit <span style="color: red">(*)</span></label>
                                    <div class="col-sm-6">
                                        <telerik:RadNumericTextBox ID="rntGluxitTu" CssClass="form-control" Width="35%" Value="45" ClientIDMode="Static" placeholder="Gluxit" runat="server" MinValue="0" MaxValue="100" MaxLength="6"></telerik:RadNumericTextBox>
                                        &nbsp;đến &nbsp;
                                        <telerik:RadNumericTextBox ID="rntGluxitDen" CssClass="form-control" Width="35%" Value="50" ClientIDMode="Static" placeholder="Gluxit" runat="server" MinValue="0" MaxValue="100" MaxLength="6"></telerik:RadNumericTextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="rntGluxitTu" CssClass="valid-control" ForeColor="Red" ErrorMessage="Thông tin bắt buộc nhập" Display="Dynamic" SetFocusOnError="true"> </asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="rntGluxitDen" CssClass="valid-control" ForeColor="Red" ErrorMessage="Thông tin bắt buộc nhập" Display="Dynamic" SetFocusOnError="true"> </asp:RequiredFieldValidator>
                                        (%)
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label class="col-sm-4 control-label" style="text-align:left">Năng lượng tương đương</label>
                                    <div class="col-sm-7">
                                        <telerik:RadNumericTextBox ID="rntNangLuong_Kcal_tu" CssClass="form-control" Width="35%" ClientIDMode="Static" placeholder="Kcal" runat="server" MinValue="0" MaxValue="100" MaxLength="6" Enabled="false"></telerik:RadNumericTextBox>
                                        &nbsp;đến &nbsp;
                                        <telerik:RadNumericTextBox ID="rntNangLuong_Kcal_den" CssClass="form-control" Width="35%" ClientIDMode="Static" placeholder="Kcal" runat="server" MinValue="0" MaxValue="100" MaxLength="6" Enabled="false"></telerik:RadNumericTextBox>
                                        (Kcal)
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-4 control-label" style="text-align:left">Lượng Protid tương đương</label>
                                    <div class="col-sm-7">
                                        <telerik:RadNumericTextBox ID="rntProtid_Kcal_tu" CssClass="form-control" Width="35%" ClientIDMode="Static" placeholder="Kcal" runat="server" MinValue="0" MaxValue="100" MaxLength="6" Enabled="false"></telerik:RadNumericTextBox>
                                        &nbsp;đến &nbsp;
                                        <telerik:RadNumericTextBox ID="rntProtid_Kcal_den" CssClass="form-control" Width="35%" ClientIDMode="Static" placeholder="Kcal" runat="server" MinValue="0" MaxValue="100" MaxLength="6" Enabled="false"></telerik:RadNumericTextBox>
                                        (Kcal)
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-4 control-label" style="text-align:left">Lượng Lipid tương đương</label>
                                    <div class="col-sm-7">
                                        <telerik:RadNumericTextBox ID="rntLipid_Kcal_tu" CssClass="form-control" Width="35%" ClientIDMode="Static" placeholder="Kcal" runat="server" MinValue="0" MaxValue="100" MaxLength="6" Enabled="false"></telerik:RadNumericTextBox>
                                        &nbsp;đến &nbsp;
                                        <telerik:RadNumericTextBox ID="rntLipid_Kcal_den" CssClass="form-control" Width="35%" ClientIDMode="Static" placeholder="Kcal" runat="server" MinValue="0" MaxValue="100" MaxLength="6" Enabled="false"></telerik:RadNumericTextBox>
                                        (Kcal)
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-4 control-label" style="text-align:left">Lượng Glucid tương đương</label>
                                    <div class="col-sm-7">
                                        <telerik:RadNumericTextBox ID="rntGlucid_Kcal_tu" CssClass="form-control" Width="35%" ClientIDMode="Static" placeholder="Kcal" runat="server" MinValue="0" MaxValue="100" MaxLength="6" Enabled="false"></telerik:RadNumericTextBox>
                                        &nbsp;đến &nbsp;
                                        <telerik:RadNumericTextBox ID="rntGlucid_Kcal_den" CssClass="form-control" Width="35%" ClientIDMode="Static" placeholder="Kcal" runat="server" MinValue="0" MaxValue="100" MaxLength="6" Enabled="false"></telerik:RadNumericTextBox>
                                        (Kcal)
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbGhiChu" class="col-sm-3 control-label">Ghi chú</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbGhiChu" CssClass="form-control" ClientIDMode="Static" placeholder="Ghi chú" runat="server" TextMode="MultiLine" Rows="3" Width="100%"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
