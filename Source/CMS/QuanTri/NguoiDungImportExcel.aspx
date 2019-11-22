<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="NguoiDungImportExcel.aspx.cs" Inherits="CMS.QuanTri.NguoiDungImportExcel" %>

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
                <span class="item-title">Nhập người dùng từ Excel</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btTaiExcel" runat="server" OnClick="btTaiExcel_Click" Text="Tải file mẫu" CssClass="btn bt-one" />
                <asp:Button ID="bt_EXCELtoSQL" runat="server" Text="Cập nhật" OnClick="bt_EXCELtoSQL_Click" CssClass="btn bt-one" OnClientClick="if ( !confirm('Bạn có chắc chắn muốn cập nhật lại dữ liệu?')) return false;" />
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
                            <asp:Button ID="btnUpload" CssClass="btn bt-one" runat="server" OnClick="bt_importSQL_Click" Text="Tải lên" />
                        </td>
                        <td style="padding-left: 130px">
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
                        <telerik:GridTemplateColumn DataField="TEN_HIEN_THI" FilterControlAltText="Filter TEN_HIEN_THI column" HeaderText="Tên hiển thị" SortExpression="TEN_HIEN_THI" UniqueName="TEN_HIEN_THI" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbTenHienThi" Text='<%# Eval("TEN_HIEN_THI") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdTenHienThi" Value='<%# Eval("TEN_HIEN_THI") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="EMAIL" FilterControlAltText="Filter EMAIL column" HeaderText="Email" SortExpression="EMAIL" UniqueName="EMAIL" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbEmail" Text='<%# Eval("EMAIL") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdEmail" Value='<%# Eval("EMAIL") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="SDT" HeaderStyle-Width="200px" FilterControlAltText="Filter SDT column" HeaderText="Số điện thoại" SortExpression="SDT" UniqueName="SDT" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbSDT" Text='<%# Eval("SDT") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdSDT" Value='<%# Eval("SDT") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="DIA_CHI" HeaderStyle-Width="200px" FilterControlAltText="Filter DIA_CHI column" HeaderText="Địa chỉ" SortExpression="DIA_CHI" UniqueName="DIA_CHI" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDiaChi" Text='<%# Eval("DIA_CHI") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdDiaChi" Value='<%# Eval("DIA_CHI") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="GHI_CHU" FilterControlAltText="Filter GHI_CHU column" HeaderText="Ghi chú" SortExpression="GHI_CHU" UniqueName="GHI_CHU" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbGhiChu" Text='<%# Eval("GHI_CHU") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdGhiChu" Value='<%# Eval("GHI_CHU") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="QUYEN" DataField="QUYEN" HeaderText="Quyền" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <telerik:RadComboBox runat="server" ID="rcbQuyen" EmptyMessage="Chọn vai trò" Width="100%" AllowCustomText="true" DataSourceID="objQuyen" DataTextField="TEN" DataValueField="MA" Filter="Contains">
                                </telerik:RadComboBox>
                                <asp:HiddenField ID="hdQuyen" Value='<%# Eval("QUYEN") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="FACE_BOOK" FilterControlAltText="Filter FACE_BOOK column" HeaderText="Facebook" SortExpression="FACE_BOOK" UniqueName="FACE_BOOK" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbFacebook" Text='<%# Eval("FACE_BOOK") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdFacebook" Value='<%# Eval("FACE_BOOK") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            <asp:ObjectDataSource ID="objQuyen" runat="server" SelectMethod="getNhomQuyen" TypeName="OneEduDataAccess.BO.NhomQuyenBO">
                <SelectParameters>
                    <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                    <asp:Parameter DefaultValue="" Name="ma_all" Type="Int16" />
                    <asp:Parameter Name="text_all" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>
</asp:Content>
