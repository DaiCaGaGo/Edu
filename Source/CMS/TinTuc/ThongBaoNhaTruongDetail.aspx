<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="ThongBaoNhaTruongDetail.aspx.cs" Inherits="CMS.TinTuc.ThongBaoNhaTruongDetail" %>

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
            function removeNodeParent(e) {
                console.log(e.parentNode.remove());
                //var parent = e.parent.hide();
            }
        </script>
    </telerik:RadCodeBlock>
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
                        <div class="form-group" runat="server">
                            <label class="col-sm-2 control-label">Nội dung <span style="color: red">(*)</span></label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="tbNoiDung" CssClass="form-control" ClientIDMode="Static" placeholder="Nội dung" runat="server" MaxLength="500" TextMode="MultiLine" Rows="3"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group" runat="server">
                            <label class="col-sm-2 control-label">Loại thông báo <span style="color: red">(*)</span></label>
                            <div class="col-sm-10">
                                <telerik:RadComboBox ID="rcbLoaiTB" runat="server" Width="100%" EmptyMessage="Chọn loại thông báo" AllowCustomText="true" Filter="Contains">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="1" Text="Thông báo chung" Selected="true" />
                                        <telerik:RadComboBoxItem Value="2" Text="Giờ vào lớp, đưa đón con" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label">Ảnh đại diện</label>
                            <div class="col-sm-3">
                                <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" CssClass="async-attachment" ID="rauAnh"
                                    HideFileInput="true" AllowedFileExtensions=".jpeg,.jpg,.png,.gif" MultipleFileSelection="Automatic" />
                            </div>

                            <div class="box-list-images">
                                <%
                                    if (listImage != null && listImage.Count() > 0)
                                    {
                                        foreach (var itemImage in listImage)
                                        {
                                            if (itemImage == null || String.IsNullOrEmpty(itemImage))
                                            {
                                                continue;
                                            }
                                %>
                                <div class="image">
                                    <div class="thumbnail">
                                        <img src="<%= "http://" + HttpContext.Current.Request.Url.Authority + "" + itemImage.ToString() %>" />
                                        <input name="list_images_node[]" type="hidden" value="<%= itemImage.ToString() %>" />
                                    </div>
                                    <lable class="remove-node" onclick="removeNodeParent(this);"><i class="fa fa-times-circle"></i></lable>
                                </div>
                                <%
                                        }
                                    }
                                %>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
