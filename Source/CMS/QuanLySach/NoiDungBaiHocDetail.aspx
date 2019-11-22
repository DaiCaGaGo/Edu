<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="NoiDungBaiHocDetail.aspx.cs" Inherits="CMS.QuanLySach.NoiDungBaiHocDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rcbKhoiHoc">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbMonHoc" />
                    <telerik:AjaxUpdatedControl ControlID="rcbSach" />
                    <telerik:AjaxUpdatedControl ControlID="rcbBaiHoc" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbMonHoc">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbSach"/>
                    <telerik:AjaxUpdatedControl ControlID="rcbBaiHoc" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbSach">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbBaiHoc" />
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
                            <label class="col-sm-3 control-label">Khối <span style="color: red">(*)</span></label>
                            <div class="col-sm-9">
                                <telerik:RadComboBox ID="rcbKhoiHoc" runat="server" Width="100%" DataSourceID="objKhoi"
                                    DataTextField="TEN" DataValueField="MA" AutoPostBack="True"
                                    OnSelectedIndexChanged="rcbKhoiHoc_SelectedIndexChanged" Filter="Contains">
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
                                    DataTextField="TEN" DataValueField="ID" AutoPostBack="True" Filter="Contains" OnSelectedIndexChanged="rcbMonHoc_SelectedIndexChanged">
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
                                    DataTextField="TEN" DataValueField="ID" Filter="Contains" OnSelectedIndexChanged="rcbSach_SelectedIndexChanged" AutoPostBack="true">
                                </telerik:RadComboBox>
                                <asp:ObjectDataSource ID="objSach" runat="server" SelectMethod="getListSachByKhoiMonHoc" TypeName="OneEduDataAccess.BO.DMSachBO">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="rcbKhoiHoc" Name="id_Khoi" PropertyName="SelectedValue" />
                                        <asp:ControlParameter ControlID="rcbMonHoc" Name="id_mon_hoc" PropertyName="SelectedValue" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Bài học</label>
                            <div class="col-sm-9">
                                <telerik:RadComboBox ID="rcbBaiHoc" runat="server" Width="100%" DataSourceID="objBaiHoc"
                                    DataTextField="TEN_BAI_HOC" DataValueField="ID" Filter="Contains" EmptyMessage="Chọn bài học" AllowCustomText="true">
                                </telerik:RadComboBox>
                                <asp:ObjectDataSource ID="objBaiHoc" runat="server" SelectMethod="getNoiDungBySach" TypeName="OneEduDataAccess.BO.DMSachNoiDungBO">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="rcbKhoiHoc" Name="id_khoi" PropertyName="SelectedValue" />
                                        <asp:ControlParameter ControlID="rcbMonHoc" Name="id_mon_hoc" PropertyName="SelectedValue" />
                                        <asp:ControlParameter ControlID="rcbSach" Name="id_sach" PropertyName="SelectedValue" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Bài số <span style="color: red">(*)</span></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbBaiSo" CssClass="form-control" ClientIDMode="Static" placeholder="Bài số" runat="server" MaxLength="4" TextMode="Number"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator2" ControlToValidate="tbBaiSo" ClientValidationFunction="validateInt" ValidateEmptyText="false"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Nhập số không quá 4 ký tự." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Trang số <span style="color: red">(*)</span></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbTrangSo" CssClass="form-control" ClientIDMode="Static" placeholder="Trang số" runat="server" MaxLength="4" TextMode="Number"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator1" ControlToValidate="tbTrangSo" ClientValidationFunction="validateInt" ValidateEmptyText="false"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Nhập số không quá 4 ký tự." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Nội dung bài tập</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="tbNoiDung" CssClass="form-control" ClientIDMode="Static" placeholder="Nội dung bài tập" runat="server" MaxLength="500" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator3" ControlToValidate="tbNoiDung" ClientValidationFunction="validateMaxchar" ValidateEmptyText="false"
                                    runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Thông tin bắt buộc, nhập không quá 500 ký tự." />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Ảnh đại diện</label>
                            <div class="col-sm-4">
                                <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" CssClass="async-attachment" ID="rauAnh"
                                    HideFileInput="true"
                                    AllowedFileExtensions=".jpeg,.jpg,.png,.gif" />
                            </div>
                            <div class="col-sm-4">
                                <asp:Image ID="imgAnh" runat="server" Style="width: 150px; height: 150px;" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
