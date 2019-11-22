<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="LopImportExcel.aspx.cs" Inherits="CMS.Lop.LopImportExcel" %>

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
                <span class="item-title">Nhập lớp từ Excel</span>
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
                        <telerik:GridTemplateColumn UniqueName="ID_KHOI" DataField="ID_KHOI" HeaderText="Tên khối" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <telerik:RadComboBox runat="server" ID="rcbKhoiHoc" DataTextField="TEN" EmptyMessage="Chọn khối" DataValueField="MA" DataSourceID="objKhoi" Width="100%" AllowCustomText="true" Filter="Contains">
                                </telerik:RadComboBox>
                                <asp:HiddenField ID="hdKhoiHoc" Value='<%# Eval("ID_KHOI") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                            <HeaderStyle Width="250px" />
                            <ItemStyle Width="250px" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="TEN" FilterControlAltText="Filter TEN column" HeaderText="Tên lớp" SortExpression="TEN" UniqueName="TEN" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbTen" Text='<%# Eval("TEN") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdTen" Value='<%# Eval("TEN") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn DataField="ID_GVCN" HeaderStyle-Width="130px" FilterControlAltText="Filter ID_GVCN column" HeaderText="Giáo viên CN" SortExpression="ID_GVCN" UniqueName="SDT" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbGVCN" Text='<%# Eval("ID_GVCN") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdGVCN" Value='<%# Eval("ID_GVCN") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="THU_TU" FilterControlAltText="Filter THU_TU column" HeaderText="Thứ tự" SortExpression="THU_TU" UniqueName="THU_TU" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbThuTu" Text='<%# Eval("THU_TU") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdThuTu" Value='<%# Eval("THU_TU") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn DataField="MA_LOAI_LOP_GDTX" FilterControlAltText="Filter MA_LOAI_LOP_GDTX column" HeaderText="Mã loại lóp GDTX" SortExpression="MA_LOAI_LOP_GDTX" UniqueName="EMAIL" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMA_LOAI_LOP_GDTX" Text='<%# Eval("MA_LOAI_LOP_GDTX") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdMA_LOAI_LOP_GDTX" Value='<%# Eval("MA_LOAI_LOP_GDTX") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="LOAI_CHEN_TIN" DataField="LOAI_CHEN_TIN" HeaderText="Loại chèn tin" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <telerik:RadComboBox runat="server" ID="rcbLoaiChenTin" EmptyMessage="Chọn tiền tố" Width="100%" AllowCustomText="true" Filter="Contains">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="1" Text="Họ tên" />
                                        <telerik:RadComboBoxItem Value="2" Text="Tên" />
                                        <telerik:RadComboBoxItem Value="3" Text="Tiền tố" />
                                    </Items>
                                </telerik:RadComboBox>
                                <asp:HiddenField ID="hdLOAI_CHEN_TIN" Value='<%# Eval("LOAI_CHEN_TIN") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn DataField="TIEN_TO" FilterControlAltText="Filter TIEN_TO column" HeaderText="Tiền tố" SortExpression="TIEN_TO" UniqueName="TIEN_TO" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbTienTo" Text='<%# Eval("TIEN_TO") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdTienTo" Value='<%# Eval("TIEN_TO") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
        <asp:ObjectDataSource ID="objKhoi" runat="server" SelectMethod="getKhoiByCapHoc" TypeName="OneEduDataAccess.BO.KhoiBO">
            <SelectParameters>
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
