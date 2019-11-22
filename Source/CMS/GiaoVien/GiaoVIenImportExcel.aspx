<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="GiaoVienImportExcel.aspx.cs" Inherits="CMS.GiaoVien.GiaoVienImportExcel" %>

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
                <span class="item-title">Nhập giáo viên từ Excel</span>
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
                        <telerik:GridTemplateColumn DataField="HO_TEN" FilterControlAltText="Filter HO_TEN column" HeaderText="Họ tên" SortExpression="HO_TEN" UniqueName="HO_TEN" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbHoTen" Text='<%# Eval("HO_TEN") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdHoTen" Value='<%# Eval("HO_TEN") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="SDT" HeaderStyle-Width="130px" FilterControlAltText="Filter SDT column" HeaderText="Số điện thoại" SortExpression="SDT" UniqueName="SDT" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbSDT" Text='<%# Eval("SDT") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdSDT" Value='<%# Eval("SDT") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="NGAY_SINH" HeaderStyle-Width="200px" FilterControlAltText="Filter NGAY_SINH column" HeaderText="Ngày sinh" SortExpression="NGAY_SINH" UniqueName="NGAY_SINH" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <telerik:RadDatePicker Culture="vi-VN" RenderMode="Classic" ID="rdNgaySinh" runat="server" Width="100%" MinDate="1900/1/1"
                                    Calendar-FastNavigationSettings-DateIsOutOfRangeMessage="Giá trị không hợp lệ"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="Hôm nay"
                                    DatePopupButton-ToolTip="Chọn ngày"
                                    Calendar-FastNavigationSettings-OkButtonCaption="Chọn"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="Hủy">
                                    <DateInput runat="server" DateFormat="dd/MM/yyyy"></DateInput>
                                </telerik:RadDatePicker>
                                <asp:HiddenField ID="hdNgaysinh" Value='<%# Eval("NGAY_SINH") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="MA_GIOI_TINH" DataField="MA_GIOI_TINH" HeaderText="Giới tính" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbGioiTinh" Text='<%# Eval("MA_GIOI_TINH") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdGioiTinh" Value='<%# Eval("MA_GIOI_TINH") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="EMAIL" FilterControlAltText="Filter EMAIL column" HeaderText="Email" SortExpression="EMAIL" UniqueName="EMAIL" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbEmail" Text='<%# Eval("EMAIL") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdEmail" Value='<%# Eval("EMAIL") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="DIA_CHI" HeaderStyle-Width="200px" FilterControlAltText="Filter DIA_CHI column" HeaderText="Địa chỉ" SortExpression="DIA_CHI" UniqueName="DIA_CHI" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDiaChi" Text='<%# Eval("DIA_CHI") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdDiaChi" Value='<%# Eval("DIA_CHI") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="ID_CHUC_VU" DataField="ID_CHUC_VU" HeaderText="Chức vụ" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbChucVu" Text='<%# Eval("ID_CHUC_VU") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdChucVu" Value='<%# Eval("ID_CHUC_VU") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="MA_TRANG_THAI" DataField="MA_TRANG_THAI" HeaderText="Trạng thái" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbTrangThai" Text='<%# Eval("MA_TRANG_THAI") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdTrangThai" Value='<%# Eval("MA_TRANG_THAI") %>' runat="server"></asp:HiddenField>
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
