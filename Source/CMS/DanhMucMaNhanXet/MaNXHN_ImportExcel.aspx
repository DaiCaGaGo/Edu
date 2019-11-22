<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="MaNXHN_ImportExcel.aspx.cs" Inherits="CMS.DanhMucMaNhanXet.MaNXHN_ImportExcel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <telerik:RadCodeBlock runat="server">
        <script>
            var grid;
            function pageLoad() {
                grid = $find("<%=RadGrid1.ClientID%>");
            }
            function refreshGrid() {
                var masterTable = grid.get_masterTableView();
                masterTable.rebind();
            }
            function viewResMSG(img, success) {
                var hdresMsg = $(img).closest("td").find("#hdresMsg").first();
                notification(success, $(hdresMsg).val());
            }
        </script>
    </telerik:RadCodeBlock>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">NHẬN XÉT HÀNG NGÀY</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btTaiExcel" runat="server" OnClick="btTaiExcel_Click" Text="Tải file mẫu" CssClass="btn bt-one" />
                <asp:Button ID="btnCapNhat" runat="server" Text="Cập nhật" OnClick="btnCapNhat_Click" CssClass="btn bt-one" OnClientClick="if ( !confirm('Bạn có chắc chắn muốn cập nhật lại dữ liệu?')) return false;" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="row" style="padding: 0 15px 0 15px">
                <table>
                    <tr>
                        <td>
                            <asp:FileUpload ID="FileExcel" AllowMultiple="true" runat="server" CssClass="btn" accept=".xls,.xlsx" />
                        </td>
                        <td>
                            <asp:Button ID="btnUpload" CssClass="btn bt-one" runat="server" OnClick="btnUpload_Click" Text="Tải lên" />
                        </td>
                        <td style="padding-left: 40px">
                            <label class="control-label">Chọn cột để import</label>
                        </td>
                        <td>
                            <div class="col-sm-12">
                                <telerik:RadComboBox ID="rcbColumn" runat="server" Width="300px" EmptyMessage="Chọn cột import" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="FitInInput" AllowCustomText="true" Filter="Contains">
                                    <Localization CheckAllString="--Tất cả--" AllItemsCheckedString="Chọn tất cả" ItemsCheckedString="mục chọn" />
                                </telerik:RadComboBox>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="item-data">
            <telerik:RadGrid ID="RadGrid1" runat="server" GroupPanelPosition="Top" OnNeedDataSource="RadGrid1_NeedDataSource"
                AutoGenerateColumns="False" AllowMultiRowSelection="True" OnItemDataBound="RadGrid1_ItemDataBound">
                <ClientSettings>
                    <Selecting AllowRowSelect="true"></Selecting>
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>
                </ClientSettings>
                <MasterTableView>
                    <NoRecordsTemplate>
                        <div style="padding: 20px 10px;">
                            Không có bản ghi nào!
                        </div>
                    </NoRecordsTemplate>
                    <HeaderStyle CssClass="head-list-grid" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="STT" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%# Container.DataSetIndex+1 %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Trạng thái" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="TRANG_THAI">
                            <ItemTemplate>
                                <img src="../img/error.png" id="iconErr" runat="server" style="cursor: pointer;" onclick="viewResMSG(this,'error')" />
                                <img src="../img/success.png" id="iconSuccess" runat="server" style="cursor: pointer;" onclick="viewResMSG(this,'success')" />
                                <asp:HiddenField ID="hdresMsg" runat="server" Value="Nội dung thông báo" ClientIDMode="Static" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="STT" HeaderStyle-Width="60px" FilterControlAltText="Filter STT column" HeaderText="STT Excel" SortExpression="STT" UniqueName="STT" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbSTT" Text='<%# Eval("STT") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdSTT" Value='<%# Eval("STT") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="MA" FilterControlAltText="Filter MA column" HeaderText="Ký hiệu" SortExpression="MA" UniqueName="MA" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMa" Text='<%# Eval("MA") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdMa" Value='<%# Eval("MA") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="NOI_DUNG" FilterControlAltText="Filter NOI_DUNG column" HeaderText="Nội dung" SortExpression="NOI_DUNG" UniqueName="NOI_DUNG" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNOI_DUNG" Text='<%# Eval("NOI_DUNG") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdNOI_DUNG" Value='<%# Eval("NOI_DUNG") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="THU_TU" FilterControlAltText="Filter THU_TU column" HeaderText="Thứ tự" SortExpression="THU_TU" UniqueName="THU_TU" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbThuTu" Text='<%# Eval("THU_TU") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdThuTu" Value='<%# Eval("THU_TU") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>