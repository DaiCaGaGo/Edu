<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="LopDetail.aspx.cs" Inherits="CMS.Lop.LopDetail" %>

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
            <telerik:AjaxSetting AjaxControlID="rcbLoaiLopGDTX">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbKhoiHoc" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbTienTo">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divTienTo" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbKhoiHoc">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tbThuTu" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <telerik:RadCodeBlock runat="server">
        <script>
            function CheckBoxHeaderClick(cb, cssLst) {
                $('.' + cssLst + ' input').each(function () {
                    if ($(this).attr('disabled') != "disabled") {
                        $(this).prop('checked', cb.checked);
                    }
                });
            }
        </script>

    </telerik:RadCodeBlock>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">CHI TIẾT LỚP HỌC</span>
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
                            <label class="col-sm-3 control-label">Tên khối <span style="color: red">(*)</span></label>
                            <div class="col-sm-9">
                                <telerik:RadComboBox ID="rcbKhoiHoc" runat="server" Width="100%" DataSourceID="objKhoi"
                                    DataTextField="TEN" DataValueField="MA" EmptyMessage="Chọn khối học" AllowCustomText="true" Filter="Contains" CausesValidation="false" OnSelectedIndexChanged="rcbKhoiHoc_SelectedIndexChanged" AutoPostBack="true">
                                </telerik:RadComboBox>
                                <asp:ObjectDataSource ID="objKhoi" runat="server" SelectMethod="getKhoiByCapHoc" TypeName="OneEduDataAccess.BO.KhoiBO">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="rcbLoaiLopGDTX" Name="maLoaiLopGDTX" PropertyName="SelectedValue" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <asp:RequiredFieldValidator ID="rfvKhoi" runat="server" ControlToValidate="rcbKhoiHoc" CssClass="valid-control" ForeColor="Red" ErrorMessage="Thông tin bắt buộc nhập" Display="Dynamic" SetFocusOnError="true"> </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbTen" class="col-sm-3 control-label">Tên lớp <span style="color: red">(*)</span></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbTen" CssClass="form-control" ClientIDMode="Static" placeholder="Nhập tên lớp" runat="server" MaxLength="50"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator3" ControlToValidate="tbTen" ClientValidationFunction="validateMaxchar" ValidateEmptyText="true"
                                    runat="server" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Thông tin bắt buộc, nhập không quá 50 ký tự." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">GVCN</label>
                            <div class="col-sm-9">
                                <telerik:RadComboBox ID="rcbGVCN" runat="server" Width="100%" DataSourceID="objGVCN" EmptyMessage="Chọn giáo viên" DataTextField="HO_TEN" DataValueField="ID" AllowCustomText="true" Filter="Contains">
                                </telerik:RadComboBox>
                                <asp:ObjectDataSource ID="objGVCN" runat="server" SelectMethod="getGiaoVienByTruong" TypeName="OneEduDataAccess.BO.GiaoVienBO">
                                    <SelectParameters>
                                        <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                                        <asp:Parameter DefaultValue="" Name="id_all" Type="Int16" />
                                        <asp:Parameter Name="text_all" Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbThuTu" class="col-sm-3 control-label">Thứ tự</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbThuTu" CssClass="form-control" ClientIDMode="Static" placeholder="Thứ tự" runat="server" MaxLength="4" TextMode="Number"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator1" ControlToValidate="tbThuTu" ClientValidationFunction="validateInt" ValidateEmptyText="false"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Nhập số không quá 4 ký tự." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Mã loại lớp GDTX</label>
                            <div class="col-sm-9">
                                <telerik:RadComboBox ID="rcbLoaiLopGDTX" runat="server" Width="100%" EmptyMessage="Chọn loại lớp" DataTextField="TEN" AutoPostBack="true" OnSelectedIndexChanged="rcbLoaiLopGDTX_SelectedIndexChanged" DataValueField="MA" CausesValidation="false" AllowCustomText="true" Filter="Contains">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="2" Text="Bổ túc THCS" />
                                        <telerik:RadComboBoxItem Value="3" Text="Bổ túc THPT" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Loại tiền tố tin nhắn</label>
                            <div class="col-sm-9">
                                <telerik:RadComboBox ID="rcbTienTo" runat="server" Width="100%" EmptyMessage="Chọn loại tiền tố" CausesValidation="false" AllowCustomText="true" Filter="Contains" OnSelectedIndexChanged="rcbTienTo_SelectedIndexChanged" AutoPostBack="true">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="1" Text="Họ tên" />
                                        <telerik:RadComboBoxItem Value="2" Text="Tên" />
                                        <telerik:RadComboBoxItem Value="3" Text="Tiền tố" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                        <div class="form-group" runat="server" id="divTienTo" visible="false">
                            <label for="tbTienTo" class="col-sm-3 control-label">Tiền tố</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbTienTo" CssClass="form-control" ClientIDMode="Static" placeholder="Tiền tố" runat="server" MaxLength="100"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
