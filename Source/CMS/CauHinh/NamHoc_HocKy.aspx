<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="NamHoc_HocKy.aspx.cs" Inherits="CMS.CauHinh.NamHoc_HocKy" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">Cập nhật năm học, học kỳ</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btEdit" runat="server" CssClass="btn bt-one" OnClick="btEdit_Click" Text="Lưu" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="form-horizontal" role="form">
                <div class="row">
                    <div class="col-sm-4">
                        <div class="form-group">
                            <label for="rcbNamHoc" class="col-sm-3 control-label">Năm học</label>
                            <div class="col-sm-7">
                                <telerik:RadComboBox ID="rcbNamHoc" runat="server" Width="100%" EmptyMessage="Chọn năm học" DataSourceID="objNamHoc" DataValueField="MA" DataTextField="TEN" AllowCustomText="true" Filter="Contains">
                                </telerik:RadComboBox>
                                <asp:ObjectDataSource ID="objNamHoc" runat="server" SelectMethod="getNamHoc" TypeName="OneEduDataAccess.BO.NamHocBO">
                                    <SelectParameters>
                                        <asp:Parameter DefaultValue="false" Name="is_all" Type="Boolean" />
                                        <asp:Parameter DefaultValue="" Name="id_all" Type="Int16" />
                                        <asp:Parameter Name="text_all" Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="form-group">
                            <label for="rcbHocKy" class="col-sm-3 control-label">Học kỳ</label>
                            <div class="col-sm-7">
                                <telerik:RadComboBox ID="rcbHocKy" runat="server" Width="100%" EmptyMessage="Chọn học kỳ" AllowCustomText="true" Filter="Contains">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="1" Text="Học kỳ I" Selected="true" />
                                        <telerik:RadComboBoxItem Value="2" Text="Học kỳ II" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
