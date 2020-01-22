﻿<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="DiemImportExcel.aspx.cs" Inherits="CMS.KetQuaHocTap.DiemImportExcel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
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
                <span class="item-title">Nhập lịch thi từ Excel</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btTaiExcel" runat="server" OnClick="btTaiExcel_Click" Text="Tải file mẫu" CssClass="btn bt-one" />
                <asp:Button ID="btCapNhat" runat="server" Text="Cập nhật" OnClick="btCapNhat_Click" CssClass="btn bt-one" OnClientClick="if ( !confirm('Bạn có chắc chắn muốn cập nhật lại dữ liệu?')) return false;" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="row">
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbHocKy" runat="server" Width="100%" EmptyMessage="Chọn học kỳ" AllowCustomText="true" Filter="Contains">
                        <Items>
                            <telerik:RadComboBoxItem Value="1" Text="Học kỳ I" Selected="true" />
                            <telerik:RadComboBoxItem Value="2" Text="Học kỳ II" />
                        </Items>
                    </telerik:RadComboBox>
                </div>
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbLoaiDiem" runat="server" Width="100%" EmptyMessage="Chọn loại điểm" AllowCustomText="true" Filter="Contains">
                        <Items>
                            <telerik:RadComboBoxItem Value="1" Text="Điểm học kỳ" Selected="true" />
                            <telerik:RadComboBoxItem Value="2" Text="Điểm 1T hệ số 1" />
                            <telerik:RadComboBoxItem Value="3" Text="Điểm 1T hệ số 2" />
                            <telerik:RadComboBoxItem Value="4" Text="Điểm 15P" />
                            <telerik:RadComboBoxItem Value="5" Text="Điểm miệng" />
                        </Items>
                    </telerik:RadComboBox>
                </div>
                <div class="col-sm-6">
                    <div class="row">
                        <div class="col-sm-6">
                            <asp:FileUpload ID="FileExcel" AllowMultiple="true" runat="server" CssClass="btn" accept=".xls,.xlsx" />
                        </div>
                        <div class="col-sm-6">
                            <asp:Button ID="btnUpload" CssClass="btn bt-one" runat="server" OnClick="btnUpload_Click" Text="Tải lên" />
                        </div>
                    </div>
                </div>
            </div>

        </div>
        <div class="item-data">
            <telerik:RadGrid ID="RadGrid1" runat="server" GroupPanelPosition="Top" OnNeedDataSource="RadGrid1_NeedDataSource"
                AutoGenerateColumns="False" AllowMultiRowSelection="True" OnItemDataBound="RadGrid1_ItemDataBound" AllowPaging="false">
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
                        <telerik:GridTemplateColumn HeaderText="Trạng thái" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="TRANG_THAI">
                            <ItemTemplate>
                                <img src="../img/error.png" id="iconErr" runat="server" style="cursor: pointer;" onclick="viewResMSG(this,'error')" />
                                <img src="../img/success.png" id="iconSuccess" runat="server" style="cursor: pointer;" onclick="viewResMSG(this,'success')" />
                                <asp:HiddenField ID="hdresMsg" runat="server" Value="Nội dung thông báo" ClientIDMode="Static" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- cột STT--%>
                        <telerik:GridTemplateColumn DataField="COT1" HeaderStyle-Width="50px" FilterControlAltText="Filter COT1 column" HeaderText="COT1" SortExpression="COT1" UniqueName="COT1" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT1" Text=""></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbCOT1" Text='<%# Eval("COT1") %>' runat="server" CssClass="form-control"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--end STT--%>
                        <%-- Mã HS--%>
                        <telerik:GridTemplateColumn DataField="COT2" HeaderStyle-Width="120px" FilterControlAltText="Filter COT2 column" HeaderText="COT2" SortExpression="COT2" UniqueName="COT2" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT2" Text=""></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbCOT2" Text='<%# Eval("COT2") %>' runat="server" CssClass="form-control"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End Mã HS--%>
                        <%--Họ và tên--%>
                        <telerik:GridTemplateColumn DataField="COT3" HeaderStyle-Width="150px" FilterControlAltText="Filter COT3 column" HeaderText="COT3" SortExpression="COT3" UniqueName="COT3" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT3" Text=""></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbCOT3" Text='<%# Eval("COT3") %>' runat="server" CssClass="form-control"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End Họ và tên--%>
                        <%--Lớp học--%>
                        <telerik:GridTemplateColumn DataField="COT4" FilterControlAltText="Filter COT4 column" HeaderText="COT4" SortExpression="COT4" UniqueName="COT4" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT4" Text=""></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbCOT4" Text='<%# Eval("COT4") %>' runat="server" CssClass="form-control"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End Lớp học--%>
                        <%--Các môn học học--%>
                        <telerik:GridTemplateColumn DataField="COT5" FilterControlAltText="Filter COT5 column" HeaderText="COT5" SortExpression="COT5" UniqueName="COT5" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT5" Text=""></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbCOT5" Text='<%# Eval("COT5") %>' runat="server" CssClass="form-control"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="COT6" FilterControlAltText="Filter COT6 column" HeaderText="COT6" SortExpression="COT6" UniqueName="COT6" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT6" Text=""></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbCOT6" Text='<%# Eval("COT6") %>' runat="server" CssClass="form-control"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="COT7" FilterControlAltText="Filter COT7 column" HeaderText="COT7" SortExpression="COT7" UniqueName="COT7" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT7" Text=""></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbCOT7" Text='<%# Eval("COT7") %>' runat="server" CssClass="form-control"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="COT8" FilterControlAltText="Filter COT8 column" HeaderText="COT8" SortExpression="COT8" UniqueName="COT8" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT8" Text=""></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbCOT8" Text='<%# Eval("COT8") %>' runat="server" CssClass="form-control"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="COT9" FilterControlAltText="Filter COT9 column" HeaderText="COT9" SortExpression="COT9" UniqueName="COT9" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT9" Text=""></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbCOT9" Text='<%# Eval("COT9") %>' runat="server" CssClass="form-control"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="COT10" FilterControlAltText="Filter COT10 column" HeaderText="COT10" SortExpression="COT10" UniqueName="COT10" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT10" Text=""></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbCOT10" Text='<%# Eval("COT10") %>' runat="server" CssClass="form-control"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="COT11" FilterControlAltText="Filter COT11 column" HeaderText="COT11" SortExpression="COT11" UniqueName="COT11" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT11" Text=""></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbCOT11" Text='<%# Eval("COT11") %>' runat="server" CssClass="form-control"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="COT12" FilterControlAltText="Filter COT12 column" HeaderText="COT12" SortExpression="COT12" UniqueName="COT12" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT12" Text=""></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbCOT12" Text='<%# Eval("COT12") %>' runat="server" CssClass="form-control"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="COT13" FilterControlAltText="Filter COT13 column" HeaderText="COT13" SortExpression="COT13" UniqueName="COT13" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT13" Text=""></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbCOT13" Text='<%# Eval("COT13") %>' runat="server" CssClass="form-control"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="COT14" FilterControlAltText="Filter COT14 column" HeaderText="COT14" SortExpression="COT14" UniqueName="COT14" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT14" Text=""></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbCOT14" Text='<%# Eval("COT14") %>' runat="server" CssClass="form-control"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="COT15" FilterControlAltText="Filter COT15 column" HeaderText="COT15" SortExpression="COT15" UniqueName="COT15" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT15" Text=""></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbCOT15" Text='<%# Eval("COT15") %>' runat="server" CssClass="form-control"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="COT16" FilterControlAltText="Filter COT16 column" HeaderText="COT16" SortExpression="COT16" UniqueName="COT16" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT16" Text=""></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbCOT16" Text='<%# Eval("COT16") %>' runat="server" CssClass="form-control"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="COT17" FilterControlAltText="Filter COT17 column" HeaderText="COT17" SortExpression="COT17" UniqueName="COT17" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT17" Text=""></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbCOT17" Text='<%# Eval("COT17") %>' runat="server" CssClass="form-control"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="COT18" FilterControlAltText="Filter COT18 column" HeaderText="COT18" SortExpression="COT18" UniqueName="COT18" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT18" Text=""></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbCOT18" Text='<%# Eval("COT18") %>' runat="server" CssClass="form-control"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="COT19" FilterControlAltText="Filter COT19 column" HeaderText="COT19" SortExpression="COT19" UniqueName="COT19" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT19" Text=""></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbCOT19" Text='<%# Eval("COT19") %>' runat="server" CssClass="form-control"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="COT20" FilterControlAltText="Filter COT20 column" HeaderText="COT20" SortExpression="COT20" UniqueName="COT20" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lblCOT20" Text=""></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbCOT20" Text='<%# Eval("COT20") %>' runat="server" CssClass="form-control"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--End Các môn học học--%>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>