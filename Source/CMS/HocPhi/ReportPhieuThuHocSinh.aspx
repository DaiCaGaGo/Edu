<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="ReportPhieuThuHocSinh.aspx.cs" Inherits="CMS.HocPhi.ReportPhieuThuHocSinh" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rcbKhoi">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbLop" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
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
                <span class="item-title">Nhập học sinh từ Excel</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btTaiExcel" runat="server" OnClick="btTaiExcel_Click" Text="Tải file mẫu" CssClass="btn bt-one" />
                <asp:Button ID="bt_EXCELtoSQL" runat="server" Text="Cập nhật" OnClick="bt_EXCELtoSQL_Click" CssClass="btn bt-one" OnClientClick="if ( !confirm('Bạn có chắc chắn muốn cập nhật lại dữ liệu?')) return false;" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="row">
                <div class="col-sm-2">
                    <telerik:RadComboBox ID="rcbKhoi" runat="server" Width="100%" DataSourceID="objKhoi" DataTextField="TEN" DataValueField="MA" AutoPostBack="True" Filter="Contains" OnSelectedIndexChanged="rcbKhoi_SelectedIndexChanged">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objKhoi" runat="server" SelectMethod="getKhoiByCapHoc" TypeName="OneEduDataAccess.BO.KhoiBO">
                        <SelectParameters>
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
                <div class="col-sm-2">
                    <telerik:RadComboBox ID="rcbLop" runat="server" Width="100%" DataSourceID="objLop"
                        DataTextField="TEN" DataValueField="ID" Filter="Contains">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objLop" runat="server" SelectMethod="getLopByKhoiNamHoc" TypeName="OneEduDataAccess.BO.LopBO">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="rcbKhoi" Name="maKhoi" PropertyName="SelectedValue" />
                            <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                            <asp:Parameter DefaultValue="" Name="id_all" Type="Int64" />
                            <asp:Parameter Name="text_all" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
                <div class="col-sm-2">
                    <div class="form-group">
                        <label for="rcbDotThu" class="col-sm-5 control-label">Đợt thu</label>
                        <div class="col-sm-7">
                            <telerik:RadComboBox ID="rcbDotThu" runat="server" Width="100%" Filter="Contains" OnSelectedIndexChanged="rcbDotThu_SelectedIndexChanged" AutoPostBack="true">
                                <Items>
                                    <telerik:RadComboBoxItem Value="1" Text="Đầu năm" />
                                    <telerik:RadComboBoxItem Value="2" Text="Theo kỳ" />
                                    <telerik:RadComboBoxItem Value="3" Text="Theo tháng" />
                                </Items>
                            </telerik:RadComboBox>
                        </div>
                    </div>
                </div>
                <div class="col-sm-2">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Học kỳ</label>
                        <div class="col-sm-7">
                            <telerik:RadComboBox ID="rcbHocKy" runat="server" Width="100%" Filter="Contains" Enabled="false">
                                <Items>
                                    <telerik:RadComboBoxItem Value="1" Text="HK 1" />
                                    <telerik:RadComboBoxItem Value="2" Text="HK 2" />
                                </Items>
                            </telerik:RadComboBox>
                        </div>
                    </div>
                </div>
                <div class="col-sm-2">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Tháng</label>
                        <div class="col-sm-7">
                            <telerik:RadComboBox ID="rcbThang" runat="server" Width="100%" Filter="Contains" Enabled="false">
                                <Items>
                                    <telerik:RadComboBoxItem Value="1" Text="Tháng 1" />
                                    <telerik:RadComboBoxItem Value="2" Text="Tháng 2" />
                                    <telerik:RadComboBoxItem Value="3" Text="Tháng 3" />
                                    <telerik:RadComboBoxItem Value="4" Text="Tháng 4" />
                                    <telerik:RadComboBoxItem Value="5" Text="Tháng 5" />
                                    <telerik:RadComboBoxItem Value="6" Text="Tháng 6" />
                                    <telerik:RadComboBoxItem Value="7" Text="Tháng 7" />
                                    <telerik:RadComboBoxItem Value="8" Text="Tháng 8" />
                                    <telerik:RadComboBoxItem Value="9" Text="Tháng 9" />
                                    <telerik:RadComboBoxItem Value="10" Text="Tháng 10" />
                                    <telerik:RadComboBoxItem Value="11" Text="Tháng 11" />
                                    <telerik:RadComboBoxItem Value="12" Text="Tháng 12" />
                                </Items>
                            </telerik:RadComboBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
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
                AutoGenerateColumns="False" AllowMultiRowSelection="True" AllowPaging="false">
                <ClientSettings>
                    <Selecting AllowRowSelect="true"></Selecting>
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
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
                        <telerik:GridTemplateColumn HeaderText="Trạng thái" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="TRANG_THAI">
                            <ItemTemplate>
                                <img src="../img/error.png" id="iconErr" runat="server" style="cursor: pointer;" onclick="viewResMSG(this,'error')" />
                                <img src="../img/success.png" id="iconSuccess" runat="server" style="cursor: pointer;" onclick="viewResMSG(this,'success')" />
                                <asp:HiddenField ID="hdresMsg" runat="server" Value="Nội dung thông báo" ClientIDMode="Static" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="STT" HeaderStyle-Width="70px" FilterControlAltText="Filter STT column" HeaderText="STT Excel" SortExpression="STT" UniqueName="STT" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbSTT" Text='<%# Eval("STT") %>' runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                <asp:HiddenField ID="hdSTT" Value='<%# Eval("STT") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="MA" HeaderStyle-Width="70px" FilterControlAltText="Filter MA column" HeaderText="Mã" SortExpression="MA" UniqueName="MA" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMA" Text='<%# Eval("MA") %>' runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                <asp:HiddenField ID="hdMA" Value='<%# Eval("MA") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="HO_TEN" FilterControlAltText="Filter HO_TEN column" HeaderStyle-Width="200px" HeaderText="Họ tên" SortExpression="HO_TEN" UniqueName="HO_TEN" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbHoTen" Text='<%# Eval("HO_TEN") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdHoTen" Value='<%# Eval("HO_TEN") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="ID_KHOI" HeaderStyle-Width="80px" FilterControlAltText="Filter ID_KHOI column" HeaderText="Khối" SortExpression="ID_KHOI" UniqueName="ID_KHOI" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbIdKhoi" Text='<%# Eval("ID_KHOI") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdIdKhoi" Value='<%# Eval("ID_KHOI") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="ID_LOP" HeaderStyle-Width="80px" FilterControlAltText="Filter ID_LOP column" HeaderText="Lớp" SortExpression="ID_LOP" UniqueName="ID_LOP" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbIdLop" Text='<%# Eval("ID_LOP") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdIdLop" Value='<%# Eval("ID_LOP") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="NGAY_SINH" HeaderStyle-Width="150px" FilterControlAltText="Filter NGAY_SINH column" HeaderText="Ngày sinh" SortExpression="NGAY_SINH" UniqueName="NGAY_SINH" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNgaySinh" Text='<%# Eval("NGAY_SINH") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdNgaysinh" Value='<%# Eval("NGAY_SINH") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="NOI_DUNG" HeaderStyle-Width="200px" FilterControlAltText="Filter NOI_DUNG column" HeaderText="Năm sinh người bảo hộ" SortExpression="Nội dung" UniqueName="NOI_DUNG" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNoiDung" Text='<%# Eval("NOI_DUNG") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdNoiDung" Value='<%# Eval("NOI_DUNG") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="SO_TIEN" HeaderStyle-Width="100px" FilterControlAltText="Filter SO_TIEN column" HeaderText="Số tiền (NVĐ)" SortExpression="SO_TIEN" UniqueName="SO_TIEN" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbSoTien" Text='<%# Eval("SO_TIEN") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdSoTien" Value='<%# Eval("SO_TIEN") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
