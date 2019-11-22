<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="PhieuThuHocSinhDetail.aspx.cs" Inherits="CMS.HocPhi.PhieuThuHocSinhDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rcbKhoi">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbLop" />
                    <telerik:AjaxUpdatedControl ControlID="rcbHocSinh" />
                    <telerik:AjaxUpdatedControl ControlID="rcbDotThu" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbLop">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbHocSinh" />
                    <telerik:AjaxUpdatedControl ControlID="rcbDotThu" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="cboTienAn">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tbTienAn" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbDotThu">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tbGhiChu" />
                    <telerik:AjaxUpdatedControl ControlID="cboTienAn" />
                    <telerik:AjaxUpdatedControl ControlID="tbTienAn" />
                    <telerik:AjaxUpdatedControl ControlID="tbTongTien" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-6">
                <span class="item-title">CHI TIẾT KHOẢN THU</span>
            </div>
            <div class="col-sm-6 text-right">
                <asp:Button ID="btAdd" runat="server" CssClass="btn bt-one" Text="Thêm" OnClick="btAdd_Click" />
                <asp:Button ID="btEdit" runat="server" CssClass="btn bt-one" Text="Sửa" OnClick="btEdit_Click" Visible="false" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="form-horizontal" role="form">
                <div class="row">
                    <div class="col-sm-12">
                        <div class="form-group">
                            <label for="rcbKhoi" class="col-sm-3 control-label">Khối <span style="color: red">(*)</span></label>
                            <div class="col-sm-9">
                                <telerik:RadComboBox ID="rcbKhoi" runat="server" Width="100%" DataSourceID="objKhoi" DataTextField="TEN" DataValueField="MA" AutoPostBack="True" Filter="Contains" OnSelectedIndexChanged="rcbKhoi_SelectedIndexChanged">
                                </telerik:RadComboBox>
                                <asp:ObjectDataSource ID="objKhoi" runat="server" SelectMethod="getKhoiByCapHoc" TypeName="OneEduDataAccess.BO.KhoiBO">
                                    <SelectParameters>
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="rcbLop" class="col-sm-3 control-label">Lớp <span style="color: red">(*)</span></label>
                            <div class="col-sm-9">
                                <telerik:RadComboBox ID="rcbLop" runat="server" Width="100%" DataSourceID="objLop"
                                    DataTextField="TEN" DataValueField="ID" Filter="Contains" OnSelectedIndexChanged="rcbLop_SelectedIndexChanged" AutoPostBack="true">
                                </telerik:RadComboBox>
                                <asp:ObjectDataSource ID="objLop" runat="server" SelectMethod="getLopByKhoiNamHoc" TypeName="OneEduDataAccess.BO.LopBO">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="rcbKhoi" Name="maKhoi" PropertyName="SelectedValue" />
                                        <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                                        <asp:Parameter DefaultValue="" Name="id_all" Type="Int64" />
                                        <asp:Parameter Name="text_all" Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Học sinh <span style="color: red">(*)</span></label>
                            <div class="col-sm-9">
                                <telerik:RadComboBox ID="rcbHocSinh" runat="server" Width="100%" DataSourceID="objHocSinh"
                                    DataTextField="HO_TEN" DataValueField="ID" Filter="Contains">
                                </telerik:RadComboBox>
                                <asp:ObjectDataSource ID="objHocSinh" runat="server" SelectMethod="getHocSinhByTruongKhoiLopTrangThai" TypeName="OneEduDataAccess.BO.HocSinhBO">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="rcbKhoi" Name="id_khoi" PropertyName="SelectedValue" />
                                        <asp:ControlParameter ControlID="rcbLop" Name="id_lop" PropertyName="SelectedValue" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="rcbDotThu" class="col-sm-3 control-label">Đợt thu <span style="color: red">(*)</span></label>
                            <div class="col-sm-9">
                                <telerik:RadComboBox ID="rcbDotThu" runat="server" Width="100%" DataSourceID="objDotThu"
                                    DataTextField="TEN" DataValueField="ID" Filter="Contains" OnSelectedIndexChanged="rcbDotThu_SelectedIndexChanged" AutoPostBack="true">
                                </telerik:RadComboBox>
                                <asp:ObjectDataSource ID="objDotThu" runat="server" SelectMethod="getDotThuLopByTruongLop" TypeName="OneEduDataAccess.BO.HocPhiDotThuBO">
                                    <SelectParameters>
                                       <asp:ControlParameter ControlID="rcbKhoi" Name="id_khoi" PropertyName="SelectedValue" />
                                        <asp:ControlParameter ControlID="rcbLop" Name="id_lop" PropertyName="SelectedValue" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Nội dung <span style="color: red">(*)</span></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbGhiChu" CssClass="form-control" ClientIDMode="Static" placeholder="Nội dung thu" runat="server" Width="100%" TextMode="MultiLine" Rows="3"></asp:TextBox>
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
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
