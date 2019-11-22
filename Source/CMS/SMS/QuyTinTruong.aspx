<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterSYS.Master" AutoEventWireup="true" CodeBehind="QuyTinTruong.aspx.cs" Inherits="CMS.SMS.QuyTinTruong" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            $(document).on("keyup", "#tbTongThemll", function (event) {

            });
            $(document).on("keyup", "#tbTongThemtb", function (event) {

            });
            function btLuuClick() {
                if (confirm("Bạn chắc chắn muốn lưu?")) {
                    return true;
                }
            }
        </script>
    </telerik:RadCodeBlock>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">Quản lý quỹ tin</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btSave" runat="server" CssClass="btn bt-one" OnClick="btSave_Click" Text="Lưu lại" OnClientClick="if(!btLuuClick()) return false;" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="form-horizontal" role="form">
                <div class="row">
                    <div class="col-sm-4">
                        <div class="form-group">
                            <label for="tbMa" class="col-sm-5 control-label">Trường</label>
                            <div class="col-sm-7">
                                <telerik:RadComboBox ID="rcbTruong" runat="server" Width="100%" EmptyMessage="Chọn trường học" DataTextField="TEN" DataValueField="ID" OnSelectedIndexChanged="rcbTruong_SelectedIndexChanged" AutoPostBack="true" AllowCustomText="true" Filter="Contains">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-3 col-sm-offset-5">
                                <asp:CheckBox ID="chbIsActive" Text="ĐK SMS" CssClass="one-checkbox" runat="server" />
                            </div>
                            <div class="col-sm-4">
                                <asp:CheckBox ID="chbIsSanTinNam" Text="San quỹ tin năm" CssClass="one-checkbox" runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="rcbGoi" class="col-sm-5 control-label">Gói</label>
                            <div class="col-sm-7">
                                <telerik:RadComboBox ID="rcbGoi" runat="server" Width="100%" EmptyMessage="Chọn gói tin" DataTextField="TEN" DataValueField="MA" DataSourceID="objMaGoiTin" OnSelectedIndexChanged="rcbGoi_SelectedIndexChanged" AutoPostBack="true" AllowCustomText="true" Filter="Contains">
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
                            <label for="tbSoHSDK" class="col-sm-5 control-label">Số học sinh đăng ký</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbSoHSDK" runat="server" CssClass="form-control" MaxLength="4" TextMode="Number"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbHSMien" class="col-sm-5 control-label">Số học sinh được miễn</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbHSMien" runat="server" CssClass="form-control" MaxLength="4" TextMode="Number"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbPhanTramVuotMuc" class="col-sm-5 control-label">Phần trăm vượt mức</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbPhanTramVuotMuc" runat="server" CssClass="form-control" MaxLength="4" TextMode="Number"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="form-group">
                            <label for="tbThuongHieu" class="col-sm-5 control-label">Thương hiệu Viettel</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbThuongHieu" CssClass="form-control" ClientIDMode="Static" placeholder="Tên thương hiệu Viettel" runat="server" MaxLength="50"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbVina" class="col-sm-5 control-label">Thương hiệu Vina</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbVina" CssClass="form-control" ClientIDMode="Static" placeholder="Tên thương hiệu VinaPhone" runat="server" MaxLength="50"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbMobi" class="col-sm-5 control-label">Thương hiệu Mobi</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbMobi" CssClass="form-control" ClientIDMode="Static" placeholder="Tên thương hiệu MobiFone" runat="server" MaxLength="50"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbGtel" class="col-sm-5 control-label">Thương hiệu GTEL</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbGtel" CssClass="form-control" ClientIDMode="Static" placeholder="Tên thương hiệu GMobile" runat="server" MaxLength="50"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbVNM" class="col-sm-5 control-label">Thương hiệu VNMobile</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbVNM" CssClass="form-control" ClientIDMode="Static" placeholder="Tên thương hiệu VNMobile" runat="server" MaxLength="50"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="rcbThang" class="col-sm-5 control-label">Tháng</label>
                            <div class="col-sm-7">
                                <telerik:RadComboBox ID="rcbThang" runat="server" Width="100%" Filter="Contains" OnSelectedIndexChanged="rcbThang_SelectedIndexChanged" AutoPostBack="true">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="form-group">
                            <label for="tbThuongHieu" class="col-sm-5 control-label">Đối tác Viettel</label>
                            <div class="col-sm-7">
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
                        <div class="form-group">
                            <label for="tbVina" class="col-sm-5 control-label">Đối tác Vina</label>
                            <div class="col-sm-7">
                                <telerik:RadComboBox ID="rcbCPViNa" runat="server" EnableCheckAllItemsCheckBox="true" Width="100%" EmptyMessage="Chọn đối tác" AllowCustomText="true" Filter="Contains" DataSourceID="objDoiTac" DataTextField="TEN" DataValueField="MA">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbMobi" class="col-sm-5 control-label">Đối tác Mobi</label>
                            <div class="col-sm-7">
                                <telerik:RadComboBox ID="rcbCPMobi" runat="server" EnableCheckAllItemsCheckBox="true" Width="100%" EmptyMessage="Chọn đối tác" AllowCustomText="true" Filter="Contains" DataSourceID="objDoiTac" DataTextField="TEN" DataValueField="MA">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbGtel" class="col-sm-5 control-label">Đối tác GTEL</label>
                            <div class="col-sm-7">
                                <telerik:RadComboBox ID="rcbCPGtel" runat="server" EnableCheckAllItemsCheckBox="true" Width="100%" EmptyMessage="Chọn đối tác" AllowCustomText="true" Filter="Contains" DataSourceID="objDoiTac" DataTextField="TEN" DataValueField="MA">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbVNM" class="col-sm-5 control-label">Đối tác VNMobile</label>
                            <div class="col-sm-7">
                                <telerik:RadComboBox ID="rcbCPVNM" runat="server" EnableCheckAllItemsCheckBox="true" Width="100%" EmptyMessage="Chọn đối tác" AllowCustomText="true" Filter="Contains" DataSourceID="objDoiTac" DataTextField="TEN" DataValueField="MA">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">Quỹ tin liên lạc cá nhân (Theo tháng)</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="tbSaveLL" runat="server" CssClass="btn bt-one" OnClick="tbSaveLL_Click" Text="Lưu lại" OnClientClick="if(!btLuuClick()) return false;" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="form-horizontal" role="form">
                <div class="row">
                    <div class="col-sm-4">
                        <div class="form-group">
                            <label for="tbMa" class="col-sm-5 control-label">Số tin học sinh/ tháng</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbSoTinHSll" runat="server" CssClass="form-control" MaxLength="4" TextMode="Number" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbMa" class="col-sm-5 control-label">Tổng cấp</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbTongCapll" runat="server" CssClass="form-control" MaxLength="4" TextMode="Number" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="form-group">
                            <label for="tbMa" class="col-sm-5 control-label">Tổng thêm</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbTongThemll" ClientIDMode="Static" runat="server" CssClass="form-control" MaxLength="4" TextMode="Number"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbMa" class="col-sm-5 control-label">Tổng đã sử dụng</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbTongDaSDll" runat="server" CssClass="form-control" MaxLength="4" TextMode="Number" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">

                        <div class="form-group">
                            <label for="tbMa" class="col-sm-5 control-label">Tổng còn</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbTongConll" ClientIDMode="Static" runat="server" CssClass="form-control" MaxLength="4" TextMode="Number" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">Quỹ tin thông báo (Theo tháng)</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="tbSaveTB" runat="server" CssClass="btn bt-one" OnClick="tbSaveTB_Click" Text="Lưu lại" OnClientClick="if(!btLuuClick()) return false;" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="form-horizontal" role="form">
                <div class="row">
                    <div class="col-sm-4">
                        <div class="form-group">
                            <label for="tbMa" class="col-sm-5 control-label">Số tin học sinh/ tháng</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbSoTinHStb" runat="server" CssClass="form-control" MaxLength="4" TextMode="Number" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbMa" class="col-sm-5 control-label">Tổng cấp</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbTongCaptb" runat="server" CssClass="form-control" MaxLength="4" TextMode="Number" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="form-group">
                            <label for="tbMa" class="col-sm-5 control-label">Tổng thêm</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbTongThemtb" ClientIDMode="Static" runat="server" CssClass="form-control" MaxLength="4" TextMode="Number"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbMa" class="col-sm-5 control-label">Tổng đã sử dụng</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbTongDaSDtb" runat="server" CssClass="form-control" MaxLength="4" TextMode="Number" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">

                        <div class="form-group">
                            <label for="tbMa" class="col-sm-5 control-label">Tổng còn</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbTongContb" ClientIDMode="Static" runat="server" CssClass="form-control" MaxLength="4" TextMode="Number" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
