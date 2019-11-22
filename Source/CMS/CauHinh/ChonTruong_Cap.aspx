<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="ChonTruong_Cap.aspx.cs" Inherits="CMS.CauHinh.ChonTruong_Cap" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rcbTruong">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbCapHoc"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbCapHoc">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divLoaiLopGDTX"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">Thiết lập trường, cấp học</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btEdit" runat="server" CssClass="btn bt-one" OnClick="btEdit_Click" Text="Xem" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="form-horizontal" role="form">
                <div class="row">
                    <div class="col-sm-4">
                        <div class="form-group">
                            <label for="rcbTruong" class="col-sm-5 control-label">Trường học</label>
                            <div class="col-sm-7">
                                <telerik:RadComboBox ID="rcbTruong" runat="server" Width="100%" DataTextField="TEN" DataValueField="ID" OnSelectedIndexChanged="rcbTruong_SelectedIndexChanged" AutoPostBack="true" Filter="Contains">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="form-group">
                            <label for="rcbHocKy" class="col-sm-5 control-label" style="padding-right: 0px;">Cấp học</label>
                            <div class="col-sm-7">
                                <telerik:RadComboBox ID="rcbCapHoc" runat="server" Width="100%" OnSelectedIndexChanged="rcbCapHoc_SelectedIndexChanged" AutoPostBack="true" Filter="Contains">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="form-group" runat="server" id="divLoaiLopGDTX">
                            <label id="rcbGDTX" runat="server" for="rcbGDTX" class="col-sm-5 control-label" style="padding-right: 0px;">Loại lớp GDTX</label>
                            <div class="col-sm-7">
                                <telerik:RadComboBox ID="rcbLoaiLopGDTX" runat="server" Width="100%" DataTextField="TEN" DataValueField="MA" Filter="Contains">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
