<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPopup.Master" AutoEventWireup="true" CodeBehind="LopMon.aspx.cs" Inherits="CMS.Lop.LopMon" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rcbHocKy">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <telerik:RadCodeBlock runat="server">
        <script>
            function CheckBoxHeaderClick(cb, cssLst) {
                $('.' + cssLst + ' input').each(function () {
                    if ($(this).attr('disabled') != "disabled") {
                        $(this).prop('checked', cb.checked);
                    }
                });
            }
        </script>

    </telerik:RadCodeBlock>
    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">DANH SÁCH MÔN HỌC THEO LỚP</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btCopy" runat="server" CssClass="btn bt-one" Text="Copy kỳ I sang kỳ II" OnClientClick="if (confirm('Bạn chắc chắn muốn copy?')) return true; else return false;" OnClick="btCopy_Click" />
                <asp:Button ID="btEdit" runat="server" CssClass="btn bt-one" Text="Lưu" OnClick="btEdit_Click" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="form-horizontal" role="form">
                <div class="row">
                    <div class="col-sm-4">
                        <telerik:RadComboBox ID="rcbHocKy" runat="server" Width="100%" Filter="Contains" OnSelectedIndexChanged="rcbHocKy_SelectedIndexChanged" AutoPostBack="true">
                            <Items>
                                <telerik:RadComboBoxItem Value="1" Text="Học kỳ I" Selected="true" />
                                <telerik:RadComboBoxItem Value="2" Text="Học kỳ II" />
                            </Items>
                        </telerik:RadComboBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="item-data">
            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" GroupPanelPosition="Top" OnNeedDataSource="RadGrid1_NeedDataSource"
                AutoGenerateColumns="False" PageSize="50" AllowMultiRowSelection="True" OnItemDataBound="RadGrid1_ItemDataBound">
                <ClientSettings>
                    <Selecting AllowRowSelect="true"></Selecting>
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                </ClientSettings>
                <MasterTableView DataKeyNames="ID" ClientDataKeyNames="ID">
                    <NoRecordsTemplate>
                        <div style="padding: 20px 10px;">
                            Không có bản ghi nào!
                        </div>
                    </NoRecordsTemplate>
                    <CommandItemSettings AddNewRecordText="Thêm mới bản ghi" CancelChangesText="Hủy"
                        RefreshText="Làm mới" SaveChangesText="Lưu thay đổi" ShowAddNewRecordButton="true" />
                    <EditFormSettings>
                        <PopUpSettings />
                        <EditColumn UpdateText="Lưu" CancelText="Hủy" InsertText="Thêm" ButtonType="PushButton"></EditColumn>
                    </EditFormSettings>
                    <HeaderStyle CssClass="head-list-grid" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="STT" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%# Container.DataSetIndex+1 %>
                                <asp:HiddenField ID="hdIdMonHoc" runat="server" Value='<%#Bind("ID_MON_HOC") %>' />
                                <asp:HiddenField ID="hdIdLop" runat="server" Value='<%#Bind("ID_LOP") %>' />
                                <asp:HiddenField ID="hdIDLopMon" runat="server" Value='<%#Bind("ID_LOP_MON") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="TEN" FilterControlAltText="Filter TEN column" HeaderText="Tên môn" SortExpression="TEN" UniqueName="TEN" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="IS_CHECK" UniqueName="IS_CHECK" DataField="IS_CHECK" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <span>Xét môn học</span><br />
                                <asp:CheckBox runat="server" ID="ckhAll" onclick="CheckBoxHeaderClick(this,'chbIS_CHECK')" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="ckIsCheck" CssClass="chbIS_CHECK" />
                                <asp:HiddenField ID="hdIsCheck" runat="server" Value='<%#Bind("IS_CHECK") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="IS_MON_CHUYEN" UniqueName="IS_MON_CHUYEN" DataField="IS_MON_CHUYEN" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <span>Là môn chuyên</span><br />
                                <asp:CheckBox runat="server" ID="ckhMonChuyenAll" onclick="CheckBoxHeaderClick(this,'chbIS_MON_CHUYEN')" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="ckIS_MON_CHUYEN" CssClass="chbIS_MON_CHUYEN" />
                                <asp:HiddenField ID="hdIS_MON_CHUYEN" runat="server" Value='<%#Bind("IS_MON_CHUYEN") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="ID_GIAO_VIEN" DataField="ID_GIAO_VIEN" HeaderText="Giáo viên bộ môn" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <telerik:RadComboBox runat="server" ID="rcbGiaoVien" DataTextField="HO_TEN" EmptyMessage="Chọn giáo viên" DataValueField="ID" DataSourceID="objGVCN" Width="100%" AllowCustomText="true" Filter="Contains">
                                </telerik:RadComboBox>
                                <asp:HiddenField ID="hdGiaoVien" runat="server" Value='<%#Bind("ID_GIAO_VIEN") %>' />
                            </ItemTemplate>
                            <HeaderStyle Width="250px" />
                            <ItemStyle Width="250px" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="SO_COT_DIEM_MIENG" DataField="SO_COT_DIEM_MIENG" HeaderText="Số cột điểm miệng" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <telerik:RadNumericTextBox ID="txtSoCotMieng" runat="server" Width="100%" Text='<%#Eval("SO_COT_DIEM_MIENG") %>' MaxValue="5" MinValue="1" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>
                                <asp:CustomValidator ID="CustomValidator1" ControlToValidate="txtSoCotMieng" ClientValidationFunction="validateInt" ValidateEmptyText="true" runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Dữ liệu không hợp lệ" />
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtSoCotMieng" ValueToCompare="5" Operator="LessThanEqual" ForeColor="Red" ErrorMessage="Giá trị không vượt quá 5" Display="Dynamic"></asp:CompareValidator>
                            </ItemTemplate>
                            <HeaderStyle Width="120px" />
                            <ItemStyle Width="120px" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="SO_COT_DIEM_15P" DataField="SO_COT_DIEM_15P" HeaderText="Số cột điểm 15P" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <telerik:RadNumericTextBox ID="txtSoCot15P" runat="server" Width="100%" Text='<%#Eval("SO_COT_DIEM_15P") %>' MaxValue="5" MinValue="1" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>
                                <asp:CustomValidator ID="CustomValidator2" ControlToValidate="txtSoCot15P" ClientValidationFunction="validateInt" ValidateEmptyText="true" runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Dữ liệu không hợp lệ." />
                                <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="txtSoCot15P" ValueToCompare="5" Operator="LessThanEqual" ForeColor="Red" ErrorMessage="Giá trị không vượt quá 5" Display="Dynamic"></asp:CompareValidator>
                            </ItemTemplate>
                            <HeaderStyle Width="120px" />
                            <ItemStyle Width="120px" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="SO_COT_DIEM_1T_HS1" DataField="SO_COT_DIEM_1T_HS1" HeaderText="Số cột điểm 1 tiết hệ số 1" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <telerik:RadNumericTextBox ID="txtSoCot1T_HS1" runat="server" Width="100%" Text='<%#Eval("SO_COT_DIEM_1T_HS1") %>' MaxValue="5" MinValue="1" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>
                                <asp:CustomValidator ID="CustomValidator4" ControlToValidate="txtSoCot1T_HS1" ClientValidationFunction="validateInt" ValidateEmptyText="true" runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Dữ liệu không hợp lệ." />
                                <asp:CompareValidator ID="CompareValidator4" runat="server" ControlToValidate="txtSoCot1T_HS1" ValueToCompare="5" Operator="LessThanEqual" ForeColor="Red" ErrorMessage="Giá trị không vượt quá 5" Display="Dynamic"></asp:CompareValidator>
                            </ItemTemplate>
                            <HeaderStyle Width="120px" />
                            <ItemStyle Width="120px" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="SO_COT_DIEM_1T_HS2" DataField="SO_COT_DIEM_1T_HS2" HeaderText="Số cột điểm 1 tiết hệ số 2" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <telerik:RadNumericTextBox ID="txtSoCot1T_HS2" runat="server" Width="100%" Text='<%#Eval("SO_COT_DIEM_1T_HS2") %>' MaxValue="5" MinValue="1" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>
                                <asp:CustomValidator ID="CustomValidator3" ControlToValidate="txtSoCot1T_HS2" ClientValidationFunction="validateInt" ValidateEmptyText="true" runat="server" SetFocusOnError="true" CssClass="valid-control" ForeColor="Red" Display="Dynamic" ErrorMessage="Dữ liệu không hợp lệ." />
                                <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="txtSoCot1T_HS2" ValueToCompare="5" Operator="LessThanEqual" ForeColor="Red" ErrorMessage="Giá trị không vượt quá 5" Display="Dynamic"></asp:CompareValidator>
                            </ItemTemplate>
                            <HeaderStyle Width="120px" />
                            <ItemStyle Width="120px" />
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                </MasterTableView>
            </telerik:RadGrid>
            <asp:ObjectDataSource ID="objGVCN" runat="server" SelectMethod="getGiaoVienByTruongTrangThai" TypeName="OneEduDataAccess.BO.GiaoVienBO">
                <SelectParameters>
                    <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                    <asp:Parameter DefaultValue="" Name="id_all" Type="Int16" />
                    <asp:Parameter Name="text_all" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>

</asp:Content>
