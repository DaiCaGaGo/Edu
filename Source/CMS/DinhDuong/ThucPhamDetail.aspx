<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="ThucPhamDetail.aspx.cs" Inherits="CMS.DinhDuong.ThucPhamDetail" %>

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
        <script>
            function changeNangLuong() {
                var tbProtid_weigh = document.getElementById('<%= tbProtid_weigh.ClientID %>');
                var tbLipid_weigh = document.getElementById('<%= tbLipid_weigh.ClientID %>');
                var tbGlucid_weigh = document.getElementById('<%= tbGlucid_weigh.ClientID %>');

                var tbProtid = document.getElementById('<%= tbProtid.ClientID %>');
                var tbLipid = document.getElementById('<%= tbLipid.ClientID %>');
                var tbGlucid = document.getElementById('<%= tbGlucid.ClientID %>');

                if (tbProtid_weigh != "") tbProtid.value = tbProtid_weigh.value * 4; else tbProtid.value = "";
                if (tbLipid_weigh != "") tbLipid.value = tbLipid_weigh.value * 9; else tbLipid.value = "";
                if (tbGlucid_weigh != "") tbGlucid.value = tbGlucid_weigh.value * 4; else tbGlucid.value = "";
            }
        </script>
    </telerik:RadCodeBlock>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-6">
                <span class="item-title">CHI TIẾT NHÓM THỰC PHẨM</span>
            </div>
            <div class="col-sm-6 text-right">
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
                            <label for="tbTen" class="col-sm-4 control-label">Nhóm thực phẩm <span style="color: red">(*)</span></label>
                            <div class="col-sm-8">
                                <telerik:RadComboBox ID="rcbNhomThucPham" runat="server" Width="100%" DataSourceID="objNhomThucPham"
                                    DataTextField="TEN" DataValueField="ID" Filter="Contains">
                                </telerik:RadComboBox>
                                <asp:ObjectDataSource ID="objNhomThucPham" runat="server" SelectMethod="getNhomThucPham" TypeName="OneEduDataAccess.BO.NhomThucPhamBO">
                                    <SelectParameters>
                                        <asp:Parameter DefaultValue="" Name="ten" Type="String" />
                                        <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                                        <asp:Parameter DefaultValue="" Name="id_all" Type="Int16" />
                                        <asp:Parameter Name="text_all" Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <asp:RequiredFieldValidator ID="rfvNhomTP" runat="server" ControlToValidate="rcbNhomThucPham" CssClass="valid-control" ForeColor="Red" ErrorMessage="Thông tin bắt buộc nhập" Display="Dynamic" SetFocusOnError="true"> </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbTen" class="col-sm-4 control-label">Tên <span style="color: red">(*)</span></label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="tbTen" CssClass="form-control" ClientIDMode="Static" placeholder="Tên thực phẩm" runat="server" MaxLength="50"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator1" ControlToValidate="tbTen" ClientValidationFunction="validateMaxchar" ValidateEmptyText="true"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Thông tin bắt buộc, nhập không quá 100 ký tự." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbTen_en" class="col-sm-4 control-label">Tên tiếng Anh</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="tbTen_en" CssClass="form-control" ClientIDMode="Static" placeholder="Tên tiếng anh" runat="server" MaxLength="50"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator8" ControlToValidate="tbTen_en" ClientValidationFunction="validateMaxchar" ValidateEmptyText="false"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Thông tin bắt buộc, nhập không quá 100 ký tự."/>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbTen" class="col-sm-4 control-label">Đơn vị tính <span style="color: red">(*)</span></label>
                            <div class="col-sm-8">
                                <telerik:RadComboBox ID="rcbDonViTinh" runat="server" Width="35%" DataSourceID="objDonViTinh"
                                    DataTextField="TEN" DataValueField="ID" Filter="Contains">
                                </telerik:RadComboBox>
                                <asp:ObjectDataSource ID="objDonViTinh" runat="server" SelectMethod="getDonViTinh" TypeName="OneEduDataAccess.BO.DonViTinhBO">
                                    <SelectParameters>
                                        <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                                        <asp:Parameter DefaultValue="" Name="id_all" Type="Int16" />
                                        <asp:Parameter Name="text_all" Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <asp:RequiredFieldValidator ID="rfvDonViTinh" runat="server" ControlToValidate="rcbDonViTinh" CssClass="valid-control" ForeColor="Red" ErrorMessage="Thông tin bắt buộc nhập" Display="Dynamic" SetFocusOnError="true"> </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbThaiBo" class="col-sm-4 control-label">Phần trăm thải bỏ</label>
                            <div class="col-sm-8">
                                <telerik:RadNumericTextBox ID="tbThaiBo" CssClass="form-control" ClientIDMode="Static" placeholder="Phần trăm thải bỏ (%)" runat="server" MinValue="0" MaxValue="100000" MaxLength="6" Width="35%"></telerik:RadNumericTextBox> (%)
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbThaiBo" CssClass="valid-control" ForeColor="Red" ErrorMessage="Dữ liệu nhập chưa đúng" Display="Dynamic" SetFocusOnError="true"> </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbNangLuong" class="col-sm-4 control-label">Năng lượng <span style="color: red">(*)</span></label>
                            <div class="col-sm-8">
                                <telerik:RadNumericTextBox ID="tbNangLuong" CssClass="form-control" ClientIDMode="Static" placeholder="Năng lượng" runat="server" MinValue="0" MaxValue="100000" MaxLength="6" Width="35%"></telerik:RadNumericTextBox> (Kcal)
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="tbNangLuong" CssClass="valid-control" ForeColor="Red" ErrorMessage="Dữ liệu nhập chưa đúng" Display="Dynamic" SetFocusOnError="true"> </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbProtid" class="col-sm-4 control-label">Chất đạm (Protid)</label>
                            <div class="col-sm-8">
                                <telerik:RadNumericTextBox ID="tbProtid_weigh" CssClass="form-control" ClientIDMode="Static" placeholder="Protid" runat="server" MinValue="0" MaxValue="100000" MaxLength="6" Width="35%" onchange="changeNangLuong()"></telerik:RadNumericTextBox>
                                 (g) &nbsp;= &nbsp; 
                                <telerik:RadNumericTextBox ID="tbProtid" CssClass="form-control" ClientIDMode="Static" placeholder="Protid" runat="server" MinValue="0" MaxValue="100000" MaxLength="6" Width="35%"></telerik:RadNumericTextBox> (Kcal)
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbLipid" class="col-sm-4 control-label">Chất béo (Lipid)</label>
                            <div class="col-sm-8">
                                <telerik:RadNumericTextBox ID="tbLipid_weigh" CssClass="form-control" ClientIDMode="Static" placeholder="Lipid" runat="server" MinValue="0" MaxValue="100000" MaxLength="6" Width="35%" onchange="changeNangLuong()"></telerik:RadNumericTextBox>
                                 (g) &nbsp;= &nbsp; 
                                <telerik:RadNumericTextBox ID="tbLipid" CssClass="form-control" ClientIDMode="Static" placeholder="Lipid" runat="server" MinValue="0" MaxValue="100000" MaxLength="6" Width="35%"></telerik:RadNumericTextBox> (Kcal)
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbGlucid" class="col-sm-4 control-label">Tinh bột (Glucid)</label>
                            <div class="col-sm-8">
                                <telerik:RadNumericTextBox ID="tbGlucid_weigh" CssClass="form-control" ClientIDMode="Static" placeholder="Glucid" runat="server" MinValue="0" MaxValue="100000" MaxLength="6" Width="35%" onchange="changeNangLuong()"></telerik:RadNumericTextBox>
                                 (g) &nbsp;= &nbsp; 
                                <telerik:RadNumericTextBox ID="tbGlucid" CssClass="form-control" ClientIDMode="Static" placeholder="Glucid" runat="server" MinValue="0" MaxValue="100000" MaxLength="6" Width="35%"></telerik:RadNumericTextBox> (Kcal)
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tbThuTu" class="col-sm-4 control-label">Thứ tự</label>
                            <div class="col-sm-8">
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
