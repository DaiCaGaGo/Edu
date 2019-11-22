<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="HocSinhImportExcel.aspx.cs" Inherits="CMS.HocSinh.HocSinhImportExcel" %>

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
                        <telerik:GridTemplateColumn DataField="SDT_NHAN_TIN" HeaderStyle-Width="170px" FilterControlAltText="Filter SDT_NHAN_TIN column" HeaderText="SĐT nhắn tin" SortExpression="SDT_NHAN_TIN" UniqueName="SDT_NHAN_TIN" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbSDTNhanTin" Text='<%# Eval("SDT_NHAN_TIN") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdSDTNhanTin" Value='<%# Eval("SDT_NHAN_TIN") %>' runat="server"></asp:HiddenField>
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
                        <telerik:GridTemplateColumn UniqueName="MA_GIOI_TINH" DataField="MA_GIOI_TINH" HeaderText="Giới tính" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbGioiTinh" Text='<%# Eval("MA_GIOI_TINH") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdGioiTinh" Value='<%# Eval("MA_GIOI_TINH") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="TRANG_THAI_HOC" DataField="TRANG_THAI_HOC" HeaderText="Trạng thái học" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="170px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbTrangThaiHoc" Text='<%# Eval("TRANG_THAI_HOC") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdTrangThaiHoc" Value='<%# Eval("TRANG_THAI_HOC") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                            <HeaderStyle Width="170px" />
                            <ItemStyle Width="170px" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="THU_TU" HeaderStyle-Width="100px" FilterControlAltText="Filter THU_TU column" HeaderText="Thứ tự" SortExpression="THU_TU" UniqueName="THU_TU" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbThuTu" Text='<%# Eval("THU_TU") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdThuTu" Value='<%# Eval("THU_TU") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="IS_CON_GV" HeaderStyle-Width="100px" FilterControlAltText="Filter IS_CON_GV column" HeaderText="Con giáo viên" SortExpression="IS_CON_GV" UniqueName="IS_CON_GV" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbConGV" runat="server" />
                                <asp:HiddenField ID="hdConGV" Value='<%# Eval("IS_CON_GV") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="IS_DK_KY1" HeaderStyle-Width="100px" FilterControlAltText="Filter IS_DK_KY1 column" HeaderText="Đăng ký SMS kỳ 1" SortExpression="IS_DK_KY1" UniqueName="IS_DK_KY1" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbSMS1" runat="server" />
                                <asp:HiddenField ID="hdSMS1" Value='<%# Eval("IS_DK_KY1") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="IS_DK_KY2" HeaderStyle-Width="100px" FilterControlAltText="Filter IS_DK_KY2 column" HeaderText="Đăng ký SMS kỳ 2" SortExpression="IS_DK_KY2" UniqueName="IS_DK_KY2" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbSMS2" runat="server" />
                                <asp:HiddenField ID="hdSMS2" Value='<%# Eval("IS_DK_KY2") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="IS_MIEN_GIAM_KY1" HeaderStyle-Width="100px" FilterControlAltText="Filter IS_MIEN_GIAM_KY1 column" HeaderText="Miễn giảm kỳ 1" SortExpression="IS_MIEN_GIAM_KY1" UniqueName="IS_MIEN_GIAM_KY1" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbMienPhi1" runat="server" />
                                <asp:HiddenField ID="hdMienPhi1" Value='<%# Eval("IS_MIEN_GIAM_KY1") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="IS_MIEN_GIAM_KY2" HeaderStyle-Width="100px" FilterControlAltText="Filter IS_MIEN_GIAM_KY2 column" HeaderText="Miễn giảm kỳ 2" SortExpression="IS_MIEN_GIAM_KY2" UniqueName="IS_MIEN_GIAM_KY2" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbMienPhi2" runat="server" />
                                <asp:HiddenField ID="hdMienPhi2" Value='<%# Eval("IS_MIEN_GIAM_KY2") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="MA_QUOC_TICH" HeaderStyle-Width="100px" FilterControlAltText="Filter MA_QUOC_TICH column" HeaderText="Quốc tịch" SortExpression="MA_QUOC_TICH" UniqueName="MA_QUOC_TICH" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbQuocTich" Text='<%# Eval("MA_QUOC_TICH") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdQuocTich" Value='<%# Eval("MA_QUOC_TICH") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="MA_DAN_TOC" HeaderStyle-Width="100px" FilterControlAltText="Filter MA_DAN_TOC column" HeaderText="Dân tộc" SortExpression="MA_DAN_TOC" UniqueName="MA_DAN_TOC" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDanToc" Text='<%# Eval("MA_DAN_TOC") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdDanToc" Value='<%# Eval("MA_DAN_TOC") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="MA_DOI_TUONG_CS" HeaderStyle-Width="150px" FilterControlAltText="Filter MA_DOI_TUONG_CS column" HeaderText="Đối tượng chính sách" SortExpression="MA_DOI_TUONG_CS" UniqueName="MA_DOI_TUONG_CS" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDoiTuong" Text='<%# Eval("MA_DOI_TUONG_CS") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdDoiTuong" Value='<%# Eval("MA_DOI_TUONG_CS") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="MA_KHU_VUC" HeaderStyle-Width="150px" FilterControlAltText="Filter MA_KHU_VUC column" HeaderText="Khu vực" SortExpression="MA_KHU_VUC" UniqueName="MA_KHU_VUC" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbKhuVuc" Text='<%# Eval("MA_KHU_VUC") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdKhuVuc" Value='<%# Eval("MA_KHU_VUC") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="MA_TINH_THANH" HeaderStyle-Width="150px" FilterControlAltText="Filter MA_TINH_THANH column" HeaderText="Tỉnh thành" SortExpression="MA_TINH_THANH" UniqueName="MA_TINH_THANH" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbTinh" Text='<%# Eval("MA_TINH_THANH") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdTinh" Value='<%# Eval("MA_TINH_THANH") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="MA_QUAN_HUYEN" HeaderStyle-Width="150px" FilterControlAltText="Filter MA_QUAN_HUYEN column" HeaderText="Quận/Huyện" SortExpression="MA_QUAN_HUYEN" UniqueName="MA_QUAN_HUYEN" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbQuanHuyen" Text='<%# Eval("MA_QUAN_HUYEN") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdQuanHuyen" Value='<%# Eval("MA_QUAN_HUYEN") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="MA_XA_PHUONG" HeaderStyle-Width="150px" FilterControlAltText="Filter MA_XA_PHUONG column" HeaderText="Đối tượng chính sách" SortExpression="MA_XA_PHUONG" UniqueName="MA_XA_PHUONG" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbXaPhuong" Text='<%# Eval("MA_XA_PHUONG") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdXaPhuong" Value='<%# Eval("MA_XA_PHUONG") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="NOI_SINH" HeaderStyle-Width="250px" FilterControlAltText="Filter NOI_SINH column" HeaderText="Nơi sinh" SortExpression="NOI_SINH" UniqueName="NOI_SINH" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNoiSinh" Text='<%# Eval("NOI_SINH") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdNoiSinh" Value='<%# Eval("NOI_SINH") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="DIA_CHI" HeaderStyle-Width="350px" FilterControlAltText="Filter DIA_CHI column" HeaderText="Địa chỉ thường trú" SortExpression="DIA_CHI" UniqueName="DIA_CHI" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDiaChi" Text='<%# Eval("DIA_CHI") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdDiaChi" Value='<%# Eval("DIA_CHI") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="SO_CMND" HeaderStyle-Width="200px" FilterControlAltText="Filter SO_CMND column" HeaderText="Số CMND" SortExpression="SO_CMND" UniqueName="SO_CMND" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbSoCMND" Text='<%# Eval("SO_CMND") %>' runat="server" CssClass="form-control" TextMode="Number" MaxLength="12"></asp:TextBox>
                                <asp:HiddenField ID="hdSoCMND" Value='<%# Eval("SO_CMND") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="NOI_CAP_CMND" HeaderStyle-Width="150px" FilterControlAltText="Filter NOI_CAP_CMND column" HeaderText="Nơi cấp" SortExpression="NOI_CAP_CMND" UniqueName="NOI_CAP_CMND" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNoiCapCMND" Text='<%# Eval("NOI_CAP_CMND") %>' runat="server" CssClass="form-control">
                                </asp:TextBox>
                                <asp:HiddenField ID="hdNoiCapCMND" Value='<%# Eval("NOI_CAP_CMND") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="NGAY_CAP_CMND" HeaderStyle-Width="200px" FilterControlAltText="Filter NGAY_CAP_CMND column" HeaderText="Ngày cấp" SortExpression="NGAY_CAP_CMND" UniqueName="NGAY_CAP_CMND" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNgayCapCMND" Text='<%# Eval("NGAY_CAP_CMND") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdNgayCap" Value='<%# Eval("NGAY_CAP_CMND") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="HO_TEN_BO" HeaderStyle-Width="200px" FilterControlAltText="Filter HO_TEN_BO column" HeaderText="Họ tên bố" SortExpression="HO_TEN_BO" UniqueName="HO_TEN_BO" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbHoTenBo" Text='<%# Eval("HO_TEN_BO") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdHoTenBo" Value='<%# Eval("HO_TEN_BO") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="NAM_SINH_BO" HeaderStyle-Width="100px" FilterControlAltText="Filter NAM_SINH_BO column" HeaderText="Năm sinh bố" SortExpression="NAM_SINH_BO" UniqueName="NAM_SINH_BO" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNamSinhBo" Text='<%# Eval("NAM_SINH_BO") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdNamSinhBo" Value='<%# Eval("NAM_SINH_BO") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="SDT_BO" HeaderStyle-Width="200px" FilterControlAltText="Filter SDT_BO column" HeaderText="SĐT bố" SortExpression="SDT_BO" UniqueName="SDT_BO" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbSDTBo" Text='<%# Eval("SDT_BO") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdSDTBo" Value='<%# Eval("SDT_BO") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="HO_TEN_ME" HeaderStyle-Width="200px" FilterControlAltText="Filter HO_TEN_ME column" HeaderText="Họ tên mẹ" SortExpression="HO_TEN_ME" UniqueName="HO_TEN_ME" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbHoTenMe" Text='<%# Eval("HO_TEN_ME") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdHoTenMe" Value='<%# Eval("HO_TEN_ME") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="NAM_SINH_ME" HeaderStyle-Width="100px" FilterControlAltText="Filter NAM_SINH_ME column" HeaderText="Năm sinh mẹ" SortExpression="NAM_SINH_ME" UniqueName="NAM_SINH_ME" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNamSinhMe" Text='<%# Eval("NAM_SINH_ME") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdNamSinhMe" Value='<%# Eval("NAM_SINH_ME") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="SDT_ME" HeaderStyle-Width="200px" FilterControlAltText="Filter SDT_ME column" HeaderText="SĐT mẹ" SortExpression="SDT_ME" UniqueName="SDT_ME" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbSDTMe" Text='<%# Eval("SDT_ME") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdSDTMe" Value='<%# Eval("SDT_ME") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="HO_TEN_NGUOI_BAO_HO" HeaderStyle-Width="200px" FilterControlAltText="Filter HO_TEN_NGUOI_BAO_HO column" HeaderText="Họ tên người bảo hộ" SortExpression="HO_TEN_NGUOI_BAO_HO" UniqueName="HO_TEN_NGUOI_BAO_HO" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbHoTenNBH" Text='<%# Eval("HO_TEN_NGUOI_BAO_HO") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdHoTenNBH" Value='<%# Eval("HO_TEN_NGUOI_BAO_HO") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="NAM_SINH_NGUOI_BAO_HO" HeaderStyle-Width="130px" FilterControlAltText="Filter NAM_SINH_NGUOI_BAO_HO column" HeaderText="Năm sinh người bảo hộ" SortExpression="NAM_SINH_NGUOI_BAO_HO" UniqueName="NAM_SINH_NGUOI_BAO_HO" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNamSinhNBH" Text='<%# Eval("NAM_SINH_NGUOI_BAO_HO") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdNamSinhNBH" Value='<%# Eval("NAM_SINH_NGUOI_BAO_HO") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="SDT_NBH" HeaderStyle-Width="200px" FilterControlAltText="Filter SDT_NBH column" HeaderText="SĐT người bảo hộ" SortExpression="SDT_NBH" UniqueName="SDT_NBH" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbSDTNBH" Text='<%# Eval("SDT_NBH") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField ID="hdSDTNBH" Value='<%# Eval("SDT_NBH") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <%--<PagerStyle AlwaysVisible="True" Mode="NextPrevAndNumeric" PageSizeLabelText="Số bản ghi/trang" PagerTextFormat="{4} Dòng {2} đến {3}/{5}" />--%>
                </MasterTableView>
            </telerik:RadGrid>
            <asp:ObjectDataSource ID="objTrangThaiHoc" runat="server" SelectMethod="getTrangThaiHS" TypeName="OneEduDataAccess.BO.TrangThaiHSBO">
                <SelectParameters>
                    <asp:Parameter Name="is_all" DefaultValue="false" DbType="Boolean" />
                    <asp:Parameter Name="id_all" DefaultValue="0" DbType="Int16" />
                    <asp:Parameter Name="text_all" DefaultValue="" DbType="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:ObjectDataSource ID="objQuocTich" runat="server" SelectMethod="getQuocTich" TypeName="OneEduDataAccess.BO.DmQuocTichBO">
                <SelectParameters>
                    <asp:Parameter Name="is_all" DefaultValue="false" DbType="Boolean" />
                    <asp:Parameter Name="id_all" DefaultValue="0" DbType="Int16" />
                    <asp:Parameter Name="text_all" DefaultValue="" DbType="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:ObjectDataSource ID="objDanToc" runat="server" SelectMethod="getDanToc" TypeName="OneEduDataAccess.BO.DmDanTocBO">
                <SelectParameters>
                    <asp:Parameter Name="is_all" DefaultValue="false" DbType="Boolean" />
                    <asp:Parameter Name="id_all" DefaultValue="0" DbType="Int16" />
                    <asp:Parameter Name="text_all" DefaultValue="" DbType="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:ObjectDataSource ID="objDoiTuong" runat="server" SelectMethod="getDoiTuongChinhSach" TypeName="OneEduDataAccess.BO.DmDoiTuongCSBO">
                <SelectParameters>
                    <asp:Parameter Name="is_all" DefaultValue="false" DbType="Boolean" />
                    <asp:Parameter Name="id_all" DefaultValue="0" DbType="Int16" />
                    <asp:Parameter Name="text_all" DefaultValue="" DbType="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:ObjectDataSource ID="objKhuVuc" runat="server" SelectMethod="getKhuVuc" TypeName="OneEduDataAccess.BO.DmKhuVucBO">
                <SelectParameters>
                    <asp:Parameter Name="is_all" DefaultValue="false" DbType="Boolean" />
                    <asp:Parameter Name="id_all" DefaultValue="0" DbType="Int16" />
                    <asp:Parameter Name="text_all" DefaultValue="" DbType="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:ObjectDataSource ID="objTinh" runat="server" SelectMethod="getTinhThanh" TypeName="OneEduDataAccess.BO.DmTinhThanhBO">
                <SelectParameters>
                    <asp:Parameter Name="is_all" DefaultValue="false" DbType="Boolean" />
                    <asp:Parameter Name="id_all" DefaultValue="0" DbType="Int16" />
                    <asp:Parameter Name="text_all" DefaultValue="" DbType="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>
</asp:Content>
