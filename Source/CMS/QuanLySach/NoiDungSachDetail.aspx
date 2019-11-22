<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="NoiDungSachDetail.aspx.cs" Inherits="CMS.QuanLySach.NoiDungSachDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rcbKhoiHoc">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbMonHoc"/>
                    <telerik:AjaxUpdatedControl ControlID="rcbSach"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbMonHoc">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbSach"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">tên bài học</span>
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
                            <label class="col-sm-3 control-label">Tên bài <span style="color: red">(*)</span></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbTen" CssClass="form-control" ClientIDMode="Static" placeholder="Tên bài học" runat="server" MaxLength="50"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator1" ControlToValidate="tbTen" ClientValidationFunction="validateMaxchar" ValidateEmptyText="true"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Thông tin bắt buộc, nhập không quá 250 ký tự." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Số trang</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbSoTrangs" CssClass="form-control" ClientIDMode="Static" placeholder="Số các trang của bài học" runat="server" MaxLength="20"></asp:TextBox>
                            </div>
                        </div>
                         <div class="form-group">
                            <label class="col-sm-3 control-label">Khối <span style="color: red">(*)</span></label>
                            <div class="col-sm-9">
                                <telerik:RadComboBox ID="rcbKhoiHoc" runat="server" Width="100%" DataSourceID="objKhoi"
                                    DataTextField="TEN" DataValueField="MA" OnSelectedIndexChanged="rcbKhoiHoc_SelectedIndexChanged" AutoPostBack="True" Filter="Contains">
                                </telerik:RadComboBox>
                                <asp:ObjectDataSource ID="objKhoi" runat="server" SelectMethod="getKhoiByCapHoc" TypeName="OneEduDataAccess.BO.KhoiBO">
                                    <SelectParameters>
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Môn học <span style="color: red">(*)</span></label>
                            <div class="col-sm-9">
                                <telerik:RadComboBox ID="rcbMonHoc" runat="server" Width="100%" DataSourceID="objMonHoc"
                                    DataTextField="TEN" DataValueField="ID" Filter="Contains" OnSelectedIndexChanged="rcbMonHoc_SelectedIndexChanged">
                                </telerik:RadComboBox>
                                <asp:ObjectDataSource ID="objMonHoc" runat="server" SelectMethod="getMonHocByKhoiCapHoc" TypeName="OneEduDataAccess.BO.MonHocBO">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="rcbKhoiHoc" Name="idKhoi" PropertyName="SelectedValue" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Sách <span style="color: red">(*)</span></label>
                            <div class="col-sm-9">
                                <telerik:RadComboBox ID="rcbSach" runat="server" Width="100%" DataSourceID="objSach"
                                    DataTextField="TEN" DataValueField="ID" Filter="Contains">
                                </telerik:RadComboBox>
                                <asp:ObjectDataSource ID="objSach" runat="server" SelectMethod="getListSachByKhoiCapHoc" TypeName="OneEduDataAccess.BO.DMSachBO">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="rcbKhoiHoc" Name="id_Khoi" PropertyName="SelectedValue" />
                                        <asp:ControlParameter ControlID="rcbMonHoc" Name="id_mon_hoc" PropertyName="SelectedValue" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
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
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Ghi chú</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbGhiChu" CssClass="form-control" ClientIDMode="Static" placeholder="Ghi chú" runat="server" MaxLength="500" TextMode="MultiLine" Rows="3"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>