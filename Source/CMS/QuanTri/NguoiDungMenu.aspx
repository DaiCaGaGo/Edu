<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="NguoiDungMenu.aspx.cs" Inherits="CMS.QuanTri.NguoiDungMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager2" runat="server" EnableAJAX="true" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rcbCapHoc">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadTreeList1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btEdit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadTreeList1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadTreeList1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadTreeList1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
    <telerik:RadCodeBlock runat="server">
        <script type="text/javascript"> 
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
            <div class="col-sm-6">
                <span class="item-title">Phân quyền người dùng menu</span>
            </div>
            <div class="col-sm-6 text-right">
                <asp:Button ID="btEdit" runat="server" CssClass="btn bt-one" OnClick="btEdit_Click" Text="Lưu" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="form-horizontal" role="form">
                <div class="row">
                    <div class="col-sm-12">
                        <div class="form-group">
                            <label class="col-sm-2 control-label">Cấp học</label>
                            <div class="col-sm-2">
                                <telerik:RadComboBox ID="rcbCapHoc" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="rcbCapHoc_SelectedIndexChanged">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12">
                        <div class="form-group">
                            <div class="col-sm-12">
                                <telerik:RadTreeList runat="server" ID="RadTreeList1" RenderMode="Lightweight" Width="100%" OnItemCreated="RadTreeList1_ItemCreated"
                                    AllowPaging="True" PageSize="50" DataKeyNames="ID" ParentDataKeyNames="ID_CHA" OnItemCommand="RadTreeList1_ItemCommand"
                                    OnNeedDataSource="RadTreeList1_NeedDataSource" AutoGenerateColumns="false" NoRecordsText="Chưa có dữ liệu...">

                                    <HeaderStyle CssClass="head-list-grid" />
                                    <Columns>
                                        <telerik:TreeListBoundColumn DataField="TEN" HeaderText="Tiêu đề" HeaderStyle-HorizontalAlign="Center" DataType="System.String" UniqueName="TEN"></telerik:TreeListBoundColumn>
                                        <telerik:TreeListTemplateColumn HeaderText="Quyền xem" UniqueName="IS_XEM" DataField="IS_XEM" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <span>Xem</span><br />
                                                <asp:CheckBox runat="server" ID="ckhAllXem" onclick="CheckBoxHeaderClick(this,'chbIS_XEM')" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbXem" runat="server" CssClass="chbIS_XEM" Checked='<%# Eval("IS_XEM") == DBNull.Value ? false : Convert.ToInt32(Eval("IS_XEM"))==1 %>' />
                                                <asp:HiddenField ID="hdIdNguoiDungMenu" runat="server" Value='<%#Bind("ID_NGUOI_DUNG_MENU") %>' />
                                                <asp:HiddenField ID="hdXem" runat="server" Value='<%#Bind("IS_XEM") %>' />
                                            </ItemTemplate>
                                        </telerik:TreeListTemplateColumn>
                                        <telerik:TreeListTemplateColumn HeaderText="Quyền thêm" UniqueName="IS_THEM" DataField="IS_THEM" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <span>Thêm</span><br />
                                                <asp:CheckBox runat="server" ID="ckhAllThem" onclick="CheckBoxHeaderClick(this,'chbIS_THEM')" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbThem" runat="server" CssClass="chbIS_THEM" Checked='<%# Eval("IS_THEM") == DBNull.Value ? false : Convert.ToInt32(Eval("IS_THEM"))==1 %>' />
                                                <asp:HiddenField ID="hdThem" runat="server" Value='<%#Bind("IS_THEM") %>' />
                                            </ItemTemplate>
                                        </telerik:TreeListTemplateColumn>
                                        <telerik:TreeListTemplateColumn HeaderText="Quyền sửa" UniqueName="IS_SUA" DataField="IS_SUA" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <span>Sửa</span><br />
                                                <asp:CheckBox runat="server" ID="ckhAllSua" onclick="CheckBoxHeaderClick(this,'chbIS_SUA')" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbSua" runat="server" CssClass="chbIS_SUA" Checked='<%# Eval("IS_SUA") == DBNull.Value ? false : Convert.ToInt32(Eval("IS_SUA"))==1 %>' />
                                                <asp:HiddenField ID="hdSua" runat="server" Value='<%#Bind("IS_SUA") %>' />
                                            </ItemTemplate>
                                        </telerik:TreeListTemplateColumn>
                                        <telerik:TreeListTemplateColumn HeaderText="Quyền xóa" UniqueName="IS_XOA" DataField="IS_XOA" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <span>Xóa</span><br />
                                                <asp:CheckBox runat="server" ID="ckhAllXoa" onclick="CheckBoxHeaderClick(this,'chbIS_XOA')" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbXoa" runat="server" CssClass="chbIS_XOA" Checked='<%# Eval("IS_XOA") == DBNull.Value ? false : Convert.ToInt32(Eval("IS_XOA"))==1 %>' />
                                                <asp:HiddenField ID="hdXoa" runat="server" Value='<%#Bind("IS_XOA") %>' />
                                            </ItemTemplate>
                                        </telerik:TreeListTemplateColumn>
                                        <telerik:TreeListTemplateColumn HeaderText="Quyền Gửi SMS" UniqueName="IS_SEND_SMS" DataField="IS_SEND_SMS" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <span>Gửi SMS</span><br />
                                                <asp:CheckBox runat="server" ID="ckhAllGuiSMS" onclick="CheckBoxHeaderClick(this,'chbIS_SEND_SMS')" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbGuiSMS" runat="server" CssClass="chbIS_SEND_SMS" Checked='<%# Eval("IS_SEND_SMS") == DBNull.Value ? false : Convert.ToInt32(Eval("IS_SEND_SMS"))==1 %>' />
                                                <asp:HiddenField ID="hdGuiSMS" runat="server" Value='<%#Bind("IS_SEND_SMS") %>' />
                                            </ItemTemplate>
                                        </telerik:TreeListTemplateColumn>
                                        <telerik:TreeListTemplateColumn HeaderText="Quyền xem chi tiết" UniqueName="IS_VIEW_INFOR" DataField="IS_VIEW_INFOR" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <span>Xem chi tiết</span><br />
                                                <asp:CheckBox runat="server" ID="ckhAllXemChiTiet" onclick="CheckBoxHeaderClick(this,'chbIS_VIEW_INFOR')" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbXemChiTiet" runat="server" CssClass="chbIS_VIEW_INFOR" Checked='<%# Eval("IS_VIEW_INFOR") == DBNull.Value ? false : Convert.ToInt32(Eval("IS_VIEW_INFOR"))==1 %>' />
                                                <asp:HiddenField ID="hdXemChiTiet" runat="server" Value='<%#Bind("IS_VIEW_INFOR") %>' />
                                            </ItemTemplate>
                                        </telerik:TreeListTemplateColumn>
                                        <telerik:TreeListTemplateColumn HeaderText="Quyền xuất Excel" UniqueName="IS_EXPORT" DataField="IS_EXPORT" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <span>Xuất excel</span><br />
                                                <asp:CheckBox runat="server" ID="ckhAllXuatExcel" onclick="CheckBoxHeaderClick(this,'chbIS_EXPORT')" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbXuatExcel" runat="server" CssClass="chbIS_EXPORT" Checked='<%# Eval("IS_EXPORT") == DBNull.Value ? false : Convert.ToInt32(Eval("IS_EXPORT"))==1 %>' />
                                                <asp:HiddenField ID="hdXuatExcel" runat="server" Value='<%#Bind("IS_EXPORT") %>' />
                                            </ItemTemplate>
                                        </telerik:TreeListTemplateColumn>


                                        <telerik:TreeListButtonColumn HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" HeaderText="Áp dụng nhiều trường"
                                            CommandName="CopyRole" ConfirmText="Bạn muốn áp dụng với tất cả các trường học của tài khoản này không?"
                                            Text="" ButtonType="ImageButton" UniqueName="CopyRole" ImageUrl="../img/RoleUser.png">
                                        </telerik:TreeListButtonColumn>


                                    </Columns>
                                </telerik:RadTreeList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
