<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="HocSinh.aspx.cs" Inherits="CMS.HocSinh.HocSinh" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbKhoi">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbLop" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbLop">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbDichVuSMS">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbMienSMS" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbMienSMS">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbTrangThai">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbGioiTinh">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btDeleteChon">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true" OnClientClose="RadWindowManager1_OnClientClose">
        <Windows>
            <telerik:RadWindow RenderMode="Lightweight" ID="RadWindow1" runat="server" ShowContentDuringLoad="false" Width="400px"
                Height="400px" Title="Chi tiết học sinh" Behaviors="Reload,Close,Maximize" Modal="true" VisibleStatusbar="false">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadCodeBlock runat="server">
        <script>
            var grid;
            function pageLoad() {
                grid = $find("<%=RadGrid1.ClientID%>");
            }

            function RadWindowManager1_OnClientClose(sender, args) {
                var ajaxManager = $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("RadWindowManager1_Close");
                var arg = args.get_argument();
                if (arg != null) {
                    refreshGrid();
                    notification('success', arg);
                }
            }
            function refreshGrid() {
                var masterTable = grid.get_masterTableView();
                masterTable.rebind();
            }
            function btDeteleClick() {
                if (grid.get_masterTableView().get_selectedItems().length > 0) {
                    if (confirm("Bạn chắc chắn muốn xóa?")) {
                        return true;
                    }
                } else {
                    alert("Mời bạn chọn thông tin xóa.");
                    return false;
                }
            }
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
            <div class="col-sm-2">
                <span class="item-title">DANH SÁCH HỌC SINH</span>
            </div>
            <div class="col-sm-10 text-right">
                <asp:Button ID="btTimKiem" runat="server" Text="Tìm kiếm" CssClass="btn bt-one" OnClick="btTimKiem_Click" />
                <input type="button" id="btAddNew" runat="server" class="btn bt-one" value="Thêm mới" onclick="openRadWin('HocSinhDetail.aspx', 100, 50)" />
                <asp:LinkButton runat="server" ID="btnImportExcel" href="\HocSinh\HocSinhImportExcel.aspx" CssClass="btn bt-one" Font-Size="17px">Import excel</asp:LinkButton>
                <asp:Button ID="btExport" runat="server" CssClass="btn bt-one" OnClick="btExport_Click" Text="Xuất excel" />
                <asp:Button ID="btnExportAll" runat="server" CssClass="btn bt-one" Text="Xuất excel toàn trường" OnClick="btnExportAll_Click"/>
                <input type="button" id="btChuyenLop" runat="server" class="btn bt-one" value="Chuyển lớp" onclick="openRadWin('ChuyenLop.aspx', 500, 100)" />
                <input type="button" id="btLenLop" runat="server" class="btn bt-one" value="Lên lớp" onclick="openRadWin('LenLop.aspx', 100,50)" />
                <asp:Button ID="btnDangKySMS" runat="server" CssClass="btn bt-one" OnClick="btnDangKySMS_Click" Text="Lưu" OnClientClick="if (confirm('Bạn có chắc chắn muốn cập nhật lại trạng thái đăng ký SMS?')) return true; else return false;" />
                <%--<asp:Button ID="btDeleteChon" runat="server" CssClass="btn bt-one" OnClick="btDeleteChon_Click" Text="Xóa" OnClientClick="if(!btDeteleClick()) return false;" />--%>
                <asp:Button ID="btDeleteByRoot" runat="server" CssClass="btn bt-one" OnClick="btDeleteByRoot_Click" Text="Xóa" OnClientClick="if(!btDeteleClick()) return false;" />
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter">
            <div class="row">
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbKhoi" runat="server" Width="100%" DataSourceID="objKhoi" DataTextField="TEN" DataValueField="MA" AutoPostBack="True" Filter="Contains" OnSelectedIndexChanged="rcbKhoi_SelectedIndexChanged">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objKhoi" runat="server" SelectMethod="getKhoiByCapHoc" TypeName="OneEduDataAccess.BO.KhoiBO">
                        <SelectParameters>
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbLop" runat="server" Width="100%" DataSourceID="objLop"
                        DataTextField="TEN" DataValueField="ID" Filter="Contains" AutoPostBack="true" OnSelectedIndexChanged="rcbLop_SelectedIndexChanged">
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
                <div class="col-sm-3">
                    <telerik:RadTextBox ID="tbTen" runat="server" EmptyMessage="Nhập tên học sinh cần tìm kiếm" Width="100%"></telerik:RadTextBox>
                </div>
                <div class="col-sm-3">
                    <telerik:RadTextBox ID="tbSDT" runat="server" EmptyMessage="Nhập SĐT cần tìm kiếm" Width="100%"></telerik:RadTextBox>
                </div>
            </div>
            <div class="row" style="margin-top: 15px;">
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbDichVuSMS" runat="server" Width="100%" EmptyMessage="Sử dụng dịch vụ SMS" AllowCustomText="true" Filter="Contains" OnSelectedIndexChanged="rcbDichVuSMS_SelectedIndexChanged" AutoPostBack="true">
                        <Items>
                            <telerik:RadComboBoxItem Value="1" Text="Đăng ký kỳ I" />
                            <telerik:RadComboBoxItem Value="2" Text="Đăng ký kỳ II" />
                            <telerik:RadComboBoxItem Value="3" Text="Không sử dụng dịch vụ" />
                        </Items>
                    </telerik:RadComboBox>
                </div>
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbMienSMS" runat="server" Width="100%" AllowCustomText="true" Filter="Contains" EmptyMessage="Đối tượng miễn phí SMS" AutoPostBack="true" OnSelectedIndexChanged="rcbMienSMS_SelectedIndexChanged">
                        <Items>
                            <telerik:RadComboBoxItem Value="1" Text="Miễn phí SMS" />
                            <telerik:RadComboBoxItem Value="2" Text="Tính phí SMS" />
                        </Items>
                    </telerik:RadComboBox>
                </div>
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbTrangThai" runat="server" Width="100%" DataSourceID="objTrangThai" DataTextField="TEN" DataValueField="MA" AutoPostBack="True" EmptyMessage="Trạng thái học sinh" AllowCustomText="true" Filter="Contains" OnSelectedIndexChanged="rcbTrangThai_SelectedIndexChanged">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objTrangThai" runat="server" SelectMethod="getTrangThaiHS" TypeName="OneEduDataAccess.BO.TrangThaiHSBO">
                        <SelectParameters>
                            <asp:Parameter Name="is_all" DbType="Boolean" DefaultValue="False" />
                            <asp:Parameter Name="id_all" DbType="Int16" DefaultValue="0" />
                            <asp:Parameter Name="text_all" DbType="String" DefaultValue="" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
                <div class="col-sm-3">
                    <telerik:RadComboBox ID="rcbGioiTinh" runat="server" Width="100%" DataSourceID="objGioiTinh" DataTextField="TEN" DataValueField="MA" AutoPostBack="True" EmptyMessage="Chọn giới tính" AllowCustomText="true" Filter="Contains" OnSelectedIndexChanged="rcbGioiTinh_SelectedIndexChanged">
                    </telerik:RadComboBox>
                    <asp:ObjectDataSource ID="objGioiTinh" runat="server" SelectMethod="getGioiTinh" TypeName="OneEduDataAccess.BO.GioiTinhBO">
                        <SelectParameters>
                            <asp:Parameter Name="is_all" DbType="Boolean" DefaultValue="False" />
                            <asp:Parameter Name="id_all" DbType="Int16" DefaultValue="0" />
                            <asp:Parameter Name="text_all" DbType="String" DefaultValue="" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
            </div>
        </div>
        <div class="item-data">
            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" GroupPanelPosition="Top" OnNeedDataSource="RadGrid1_NeedDataSource"
                AutoGenerateColumns="False" PageSize="100" AllowMultiRowSelection="True" OnItemDataBound="RadGrid1_ItemDataBound">
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
                        <telerik:GridClientSelectColumn UniqueName="chkChon" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridClientSelectColumn>
                        <telerik:GridTemplateColumn HeaderText="STT" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%# Container.DataSetIndex+1 %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="MA" FilterControlAltText="Filter MA column" HeaderText="Mã HS" SortExpression="MA" UniqueName="MA" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="140px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="HO_TEN" FilterControlAltText="Filter HO_TEN column" HeaderText="Tên HS" SortExpression="HO_TEN" UniqueName="HO_TEN" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Thứ tự" ColumnGroupName="THU_TU" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-CssClass="grid-control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="THU_TU">
                            <ItemTemplate>
                                <asp:TextBox ID="tbTHU_TU" runat="server" Text='<%# Eval("THU_TU") %>' CssClass="form-control" MaxLength="4" TextMode="Number" style="text-align:center"></asp:TextBox>
                                <asp:HiddenField ID="hdTHU_TU" runat="server" Value='<%# Eval("THU_TU") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="ID_LOP" FilterControlAltText="Filter ID_LOP column" HeaderText="Lớp học" SortExpression="ID_LOP" UniqueName="ID_LOP" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TEN_LOP" FilterControlAltText="Filter TEN_LOP column" HeaderText="Lớp học" SortExpression="TEN_LOP" UniqueName="TEN_LOP" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NGAY_SINH" FilterControlAltText="Filter NGAY_SINH column" HeaderText="Ngày sinh" SortExpression="NGAY_SINH" UniqueName="NGAY_SINH" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-Width="100px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MA_GIOI_TINH" FilterControlAltText="Filter MA_GIOI_TINH column" HeaderText="Giới tính" SortExpression="MA_GIOI_TINH" UniqueName="MA_GIOI_TINH" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="90px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SDT_NHAN_TIN" FilterControlAltText="Filter SDT_NHAN_TIN column" HeaderText="SĐT nhận tin" SortExpression="SDT_NHAN_TIN" UniqueName="SDT_NHAN_TIN" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SDT_NHAN_TIN2" FilterControlAltText="Filter SDT_NHAN_TIN2 column" HeaderText="SĐT nhận tin" SortExpression="SDT_NHAN_TIN2" UniqueName="SDT_NHAN_TIN2" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SDT_BM" FilterControlAltText="Filter SDT_BM column" HeaderText="SĐT nhận tin" SortExpression="SDT_BM" UniqueName="SDT_BM" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TRANG_THAI_HOC" FilterControlAltText="Filter TRANG_THAI_HOC column" HeaderText="Trạng thái" SortExpression="TRANG_THAI_HOC" UniqueName="TRANG_THAI_HOC" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TRANG_THAI_STR" FilterControlAltText="Filter TRANG_THAI_STR column" HeaderText="Trạng thái" SortExpression="TRANG_THAI_STR" UniqueName="TRANG_THAI_STR" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NGAY_TAO" FilterControlAltText="Filter NGAY_TAO column" HeaderText="Ngày tạo" SortExpression="NGAY_TAO" UniqueName="NGAY_TAO" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-Width="100px">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Con GV" UniqueName="IS_CON_GV" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chbIS_CON_GV" Enabled="false"/>
                                <asp:HiddenField ID="hdIS_CON_GV" runat="server" Value='<%#Bind("IS_CON_GV") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Gửi cả bố mẹ" UniqueName="IS_GUI_BO_ME" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chbIS_GUI_BO_ME" Enabled="false"/>
                                <asp:HiddenField ID="hdIS_GUI_BO_ME" runat="server" Value='<%#Bind("IS_GUI_BO_ME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="" UniqueName="IS_DK_KY1" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <span>ĐK SMS kỳ 1</span><br />
                                <asp:CheckBox runat="server" ID="ckhAllKy1" onclick="CheckBoxHeaderClick(this,'chb_DK_KY1')" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chbIS_DK_KY1" CssClass="chb_DK_KY1" />
                                <asp:HiddenField ID="hdIS_DK_KY1" runat="server" Value='<%#Bind("IS_DK_KY1") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="" UniqueName="IS_DK_KY2" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <span>ĐK SMS kỳ 2</span><br />
                                <asp:CheckBox runat="server" ID="ckhAllKy2" onclick="CheckBoxHeaderClick(this,'chb_DK_KY2')" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chbIS_DK_KY2" CssClass="chb_DK_KY2" />
                                <asp:HiddenField ID="hdIS_DK_KY2" runat="server" Value='<%#Bind("IS_DK_KY2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Miễn giảm kỳ 1" UniqueName="IS_MIEN_GIAM_KY1" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chbIS_MIEN_GIAM_KY1" Enabled="false"/>
                                <asp:HiddenField ID="hdIS_MIEN_GIAM_KY1" runat="server" Value='<%#Bind("IS_MIEN_GIAM_KY1") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Miễn giảm kỳ 2" UniqueName="IS_MIEN_GIAM_KY2" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chbIS_MIEN_GIAM_KY2" Enabled="false"/>
                                <asp:HiddenField ID="hdIS_MIEN_GIAM_KY2" runat="server" Value='<%#Bind("IS_MIEN_GIAM_KY2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sửa" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="SUA">
                            <ItemTemplate>
                                <img src="../img/sua.png" onclick='<%# "openRadWin(\"HocSinhDetail.aspx?id_hs=" + Eval("ID") + "\",100,50)" %>' style="cursor: pointer;" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--<telerik:GridTemplateColumn HeaderText="In sổ" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="IN_SO">
                            <ItemTemplate>
                                <img src="../img/sua.png" onclick='<%# "openRadWin(\"../InSoHocSinh/InSoTheoHocSinh.aspx?id_hs=" + Eval("ID") + "\",500,200)" %>' style="cursor: pointer;" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                    </Columns>
                    <PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PageSizes="20,50,100,200" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
