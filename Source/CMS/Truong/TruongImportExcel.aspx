<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterSYS.Master" AutoEventWireup="true" CodeBehind="TruongImportExcel.aspx.cs" Inherits="CMS.Truong.TruongImportExcel" %>

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
                        <telerik:GridTemplateColumn DataField="MA" FilterControlAltText="Filter MA column" HeaderText="Mã trường" SortExpression="MA" UniqueName="MA" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMa" Text='<%# Eval("MA") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdMa" Value='<%# Eval("MA") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="TEN" FilterControlAltText="Filter TEN column" HeaderText="Tên trường" SortExpression="TEN" UniqueName="TEN" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="300px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbTen" Text='<%# Eval("TEN") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdTen" Value='<%# Eval("TEN") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="IS_MN" HeaderStyle-Width="100px" FilterControlAltText="Filter IS_MN column" HeaderText="Mầm non" SortExpression="IS_MN" UniqueName="IS_MN" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbMamNon" runat="server" />
                                <asp:HiddenField ID="hdMamNon" Value='<%# Eval("IS_MN") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="IS_TH" HeaderStyle-Width="100px" FilterControlAltText="Filter IS_TH column" HeaderText="Tiểu học" SortExpression="IS_TH" UniqueName="IS_TH" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbTH" runat="server" />
                                <asp:HiddenField ID="hdTH" Value='<%# Eval("IS_TH") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="IS_THCS" HeaderStyle-Width="100px" FilterControlAltText="Filter IS_THCS column" HeaderText="THCS" SortExpression="IS_THCS" UniqueName="IS_THCS" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbTHCS" runat="server" />
                                <asp:HiddenField ID="hdTHCS" Value='<%# Eval("IS_THCS") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="IS_THPT" HeaderStyle-Width="100px" FilterControlAltText="Filter IS_THPT column" HeaderText="THPT" SortExpression="IS_THPT" UniqueName="IS_THPT" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbTHPT" runat="server" />
                                <asp:HiddenField ID="hdTHPT" Value='<%# Eval("IS_THPT") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="IS_GDTX" HeaderStyle-Width="100px" FilterControlAltText="Filter IS_GDTX column" HeaderText="GDTX" SortExpression="IS_GDTX" UniqueName="IS_GDTX" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbGDTX" runat="server" />
                                <asp:HiddenField ID="hdGDTX" Value='<%# Eval("IS_GDTX") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="MA_TINH_THANH" DataField="MA_TINH_THANH" HeaderText="Tỉnh/Thành phố" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbTinhThanh" Text='<%# Eval("MA_TINH_THANH") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdTinhThanh" runat="server" Value='<%# Eval("MA_TINH_THANH") %>' />
                            </ItemTemplate>
                            <HeaderStyle Width="250px" />
                            <ItemStyle Width="250px" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="MA_QUAN_HUYEN" DataField="MA_QUAN_HUYEN" HeaderText="Quận/Huyện" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbQuanHuyen" Text='<%# Eval("MA_QUAN_HUYEN") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdQuanHuyen" runat="server" Value='<%# Eval("MA_QUAN_HUYEN") %>' />
                            </ItemTemplate>
                            <HeaderStyle Width="250px" />
                            <ItemStyle Width="250px" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="IS_ACTIVE_SMS" HeaderStyle-Width="100px" FilterControlAltText="Filter IS_ACTIVE_SMS column" HeaderText="Đăng ký SMS" SortExpression="IS_ACTIVE_SMS" UniqueName="IS_ACTIVE_SMS" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbDangKySMS" runat="server" />
                                <asp:HiddenField ID="hdDangKySMS" Value='<%# Eval("IS_ACTIVE_SMS") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="BRAND_NAME_VIETTEL" FilterControlAltText="Filter BRAND_NAME_VIETTEL column" HeaderText="BrandName Viettel" SortExpression="BRAND_NAME_VIETTEL" UniqueName="BRAND_NAME_VIETTEL" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbBrViettel" Text='<%# Eval("BRAND_NAME_VIETTEL") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdBrViettel" Value='<%# Eval("BRAND_NAME_VIETTEL") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="CP_VIETTEL" DataField="CP_VIETTEL" HeaderText="Đối tác Viettel" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%--<telerik:RadComboBox runat="server" ID="rcbCPViettel" DataTextField="TEN" EmptyMessage="Chọn đối tác" DataValueField="MA" DataSourceID="objTelco" Width="100%" AllowCustomText="true" Filter="Contains">
                                </telerik:RadComboBox>--%>
                                <asp:TextBox ID="tbCPViettel" Text='<%# Eval("CP_VIETTEL") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdCPViettel" runat="server" Value='<%#Bind("CP_VIETTEL") %>' />
                            </ItemTemplate>
                            <HeaderStyle Width="200px" />
                            <ItemStyle Width="200px" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="BRAND_NAME_VINA" FilterControlAltText="Filter BRAND_NAME_VINA column" HeaderText="BrandName VinaPhone" SortExpression="BRAND_NAME_VINA" UniqueName="BRAND_NAME_VINA" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbBrVina" Text='<%# Eval("BRAND_NAME_VINA") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdBrVina" Value='<%# Eval("BRAND_NAME_VINA") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="CP_VINA" DataField="CP_VINA" HeaderText="Đối tác Vinaphone" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%--<telerik:RadComboBox runat="server" ID="rcbCPVina" DataTextField="TEN" EmptyMessage="Chọn đối tác" DataValueField="MA" DataSourceID="objTelco" Width="100%" AllowCustomText="true" Filter="Contains">
                                </telerik:RadComboBox>--%>
                                <asp:TextBox ID="tbCPVina" Text='<%# Eval("CP_VINA") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdCPVina" runat="server" Value='<%# Eval("CP_VINA") %>' />
                            </ItemTemplate>
                            <HeaderStyle Width="200px" />
                            <ItemStyle Width="200px" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="BRAND_NAME_MOBI" FilterControlAltText="Filter BRAND_NAME_MOBI column" HeaderText="BrandName Mobiphone" SortExpression="BRAND_NAME_MOBI" UniqueName="BRAND_NAME_MOBI" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbBrMobi" Text='<%# Eval("BRAND_NAME_MOBI") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdBrMobi" Value='<%# Eval("BRAND_NAME_MOBI") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="CP_MOBI" DataField="CP_MOBI" HeaderText="Đối tác Mobiphone" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%--<telerik:RadComboBox runat="server" ID="rcbCPMobi" DataTextField="TEN" EmptyMessage="Chọn đối tác" DataValueField="MA" DataSourceID="objTelco" Width="100%" AllowCustomText="true" Filter="Contains">
                                </telerik:RadComboBox>--%>
                                <asp:TextBox ID="tbCPMobi" Text='<%# Eval("CP_MOBI") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdCPMobi" runat="server" Value='<%# Eval("CP_MOBI") %>' />
                            </ItemTemplate>
                            <HeaderStyle Width="200px" />
                            <ItemStyle Width="200px" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="BRAND_NAME_GTEL" FilterControlAltText="Filter BRAND_NAME_GTEL column" HeaderText="BrandName Gtel" SortExpression="BRAND_NAME_GTEL" UniqueName="BRAND_NAME_GTEL" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbBrGtel" Text='<%# Eval("BRAND_NAME_GTEL") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdBrGtel" Value='<%# Eval("BRAND_NAME_GTEL") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="CP_GTEL" DataField="CP_GTEL" HeaderText="Đối tác Gtel" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%--<telerik:RadComboBox runat="server" ID="rcbCPGtel" DataTextField="TEN" EmptyMessage="Chọn đối tác" DataValueField="MA" DataSourceID="objTelco" Width="100%" AllowCustomText="true" Filter="Contains">
                                </telerik:RadComboBox>--%>
                                <asp:TextBox ID="tbCPGtel" Text='<%# Eval("CP_GTEL") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdCPGtel" runat="server" Value='<%# Eval("CP_GTEL") %>' />
                            </ItemTemplate>
                            <HeaderStyle Width="200px" />
                            <ItemStyle Width="200px" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="BRAND_NAME_VNM" FilterControlAltText="Filter BRAND_NAME_VNM column" HeaderText="BrandName VietnamMobile" SortExpression="BRAND_NAME_VNM" UniqueName="BRAND_NAME_VNM" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbBrVNM" Text='<%# Eval("BRAND_NAME_VNM") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdBrVNM" Value='<%# Eval("BRAND_NAME_VNM") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="CP_VNM" DataField="CP_VNM" HeaderText="Đối tác VietnamMobile" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%--<telerik:RadComboBox runat="server" ID="rcbCPVNM" DataTextField="CP_VNM" EmptyMessage="Chọn đối tác" DataValueField="MA" DataSourceID="objTelco" Width="100%" AllowCustomText="true" Filter="Contains">
                                </telerik:RadComboBox>--%>
                                <asp:TextBox ID="tbCPVNM" Text='<%# Eval("CP_VNM") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdCPVNM" runat="server" Value='<%# Eval("CP_VNM") %>' />
                            </ItemTemplate>
                            <HeaderStyle Width="200px" />
                            <ItemStyle Width="200px" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="MA_GOI_TIN" FilterControlAltText="Filter MA_GOI_TIN column" HeaderText="Gói tin" SortExpression="MA_GOI_TIN" UniqueName="MA_GOI_TIN" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbGoiTin" Text='<%# Eval("MA_GOI_TIN") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdGoiTin" Value='<%# Eval("MA_GOI_TIN") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            <%--<asp:ObjectDataSource ID="objTelco" runat="server" SelectMethod="getTelco" TypeName="OneEduDataAccess.BO.CPTelCoBO">
                <SelectParameters>
                    <asp:Parameter Name="is_all" DefaultValue="false" DbType="Boolean" />
                    <asp:Parameter Name="id_all" DefaultValue="0" DbType="Int16" />
                    <asp:Parameter Name="text_all" DefaultValue="" DbType="String" />
                </SelectParameters>
            </asp:ObjectDataSource>--%>
        </div>
    </div>
</asp:Content>
