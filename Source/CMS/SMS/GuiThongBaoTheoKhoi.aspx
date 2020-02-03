<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="GuiThongBaoTheoKhoi.aspx.cs" Inherits="CMS.SMS.GuiThongBaoTheoKhoi" %>

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
            <telerik:AjaxSetting AjaxControlID="cbHenGioGuiTin">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divTime" UpdatePanelCssClass="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>

    <div class="item-content">
        <div class="row item-header">
            <div class="col-sm-4">
                <span class="item-title">GỬI TIN NHẮN THÔNG BÁO</span>
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btnGuiTuyChon" runat="server" CssClass="btn bt-one" Text="Gửi tùy chọn" OnClientClick="if(!btLuuClick()) return false;" OnClick="btnGuiTuyChon_Click" />
                <asp:Button ID="btnGuiAll" runat="server" CssClass="btn bt-one" Text="Gửi toàn trường" OnClientClick="if(!btLuuClick()) return false;" OnClick="btnGuiAll_Click" />
                <asp:Button ID="btGuiSoDinhKem" runat="server" CssClass="btn bt-one" Text="Gửi SMS đính kèm" OnClientClick="if(!btLuuClick()) return false;" OnClick="btGuiSoDinhKem_Click" />
                <asp:LinkButton runat="server" ID="btnSmsTuyBien" href="\SMS\SMSImportExcel.aspx" CssClass="btn bt-one" Font-Size="17px">Gửi SMS tùy biến</asp:LinkButton>
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="item-filter" style="padding-bottom: 0">
            <div class="form-horizontal" role="form">
                <div class="row note-form">
                    <div class="col-sm-12">
                        <label class="content-note-form"><span style="font-weight: bold">Chú ý:</span> Dòng có màu đỏ: học sinh không đăng ký dịch vụ SMS; xanh: con GV; (*M): miễn phí SMS</label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2">
                        <telerik:RadComboBox ID="rcbKhoi" runat="server" Width="100%" DataSourceID="objKhoiHoc" DataTextField="TEN" DataValueField="MA" Filter="Contains" AllowCustomText="true" EmptyMessage="Chọn khối học" CheckBoxes="true" CheckedItemsTexts="DisplayAllInInput" EnableCheckAllItemsCheckBox="true" Localization-CheckAllString="Chọn tất cả" OnSelectedIndexChanged="rcbKhoi_SelectedIndexChanged" AutoPostBack="true">
                        </telerik:RadComboBox>
                        <asp:HiddenField ID="hdKhoi" runat="server" />
                        <asp:ObjectDataSource ID="objKhoiHoc" runat="server" SelectMethod="getKhoiByCapHocAndMaLoaiGDTX" TypeName="OneEduDataAccess.BO.KhoiBO">
                            <SelectParameters>
                                <asp:Parameter Name="maLoaiLopGDTX" DefaultValue="" />
                                <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                                <asp:Parameter DefaultValue="" Name="id_all" Type="Int16" />
                                <asp:Parameter DefaultValue="Tất cả" Name="text_all" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </div>
                    <div class="col-sm-2">
                        <telerik:RadComboBox Width="100%" ID="rcbLop" EmptyMessage="Chọn lớp" runat="server"
                            DataSourceID="objLop" DataTextField="TEN" DataValueField="ID" AllowCustomText="true" Filter="Contains" CheckBoxes="true" CheckedItemsTexts="DisplayAllInInput" EnableCheckAllItemsCheckBox="true" Localization-CheckAllString="Chọn tất cả">
                        </telerik:RadComboBox>
                        <asp:ObjectDataSource ID="objLop" runat="server" SelectMethod="getLopByLstKhoiNamHoc" TypeName="OneEduDataAccess.BO.LopBO">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hdKhoi" Name="lstMaKhoi" PropertyName="Value" />
                                <asp:Parameter DefaultValue="False" Name="is_all" Type="Boolean" />
                                <asp:Parameter DefaultValue="" Name="id_all" Type="Int64" />
                                <asp:Parameter DefaultValue="Tất cả" Name="text_all" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </div>
                    <div class="one-checkbox col-sm-2">
                    </div>
                    <div class="col-sm-4">
                        <div class="row">
                            <div class="one-checkbox col-md-3 col-sm-5" style="margin-left: -20px;">
                                <label>
                                    <asp:CheckBox ID="cbHenGioGuiTin" runat="server" Text="Hẹn Giờ" OnCheckedChanged="cbHenGioGuiTin_CheckedChanged" AutoPostBack="true" />
                                </label>
                            </div>
                            <div class="col-md-9 col-sm-7">
                                <div runat="server" id="divTime" visible="false">
                                    <asp:TextBox ID="tbTime" runat="server" CssClass="form-control text-box nd-nx-nl" TextMode="DateTimeLocal"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row" style="margin-top: 15px;">
                    <div class="one-checkbox col-sm-12">
                        <label>
                            <asp:CheckBox ID="cboGuiGVCN" runat="server" Text="Gửi GVCN" Checked="true" />
                        </label>
                        &nbsp;&nbsp;&nbsp;
                        <label>
                            <asp:CheckBox ID="cboGuiGV" runat="server" Text="Gửi GV toàn trường" Checked="false" />
                        </label>
                        &nbsp;&nbsp;&nbsp;
                        <label>
                            <asp:CheckBox ID="cboGuiZalo" runat="server" Text="Gửi Zalo" /></label>
                    </div>

                </div>
                <div class="row" style="margin-top: 15px;">
                    <div class="col-sm-6">
                        <asp:TextBox ID="tbNoiDung" ClientIDMode="Static" runat="server" placeholder="Nhập nội dung tin nhắn vào đây" CssClass="form-control text-box nd-nx-nl" TextMode="MultiLine" Rows="3" onkeyup="change(this);" onkeydown="change(this);" onchange="change(this);"></asp:TextBox>
                        <span style="position: absolute; padding-top: 5px">Nội dung tin nhắn (<span id="numberCharConfirm">0</span> ký tự) (<span id="numberSMSConfirm">0</span>/3 tin)</span>
                    </div>
                    <div class="col-sm-4">
                        <asp:TextBox ID="tbListSDT" ClientIDMode="Static" runat="server" placeholder="Nhập SĐT gửi kèm" CssClass="form-control text-box nd-nx-nl" TextMode="MultiLine" Rows="3"></asp:TextBox>
                        <span style="position: absolute; padding-top: 5px">(Không nhập quá 10 SĐT, giữa các số ngăn cách bởi dấu <span style="color: red">;</span>)</span>
                        <asp:HiddenField ID="hdSDT_GuiKem" runat="server" />
                    </div>
                </div>
                <div class="row" style="margin-top: 30px;">
                    <div class="col-sm-12">
                        <span class="progress-description" style="color: red;">
                            <asp:Label runat="server" ID="txtTongQuyTinConLaiTheoNam"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label runat="server" ID="txtTongQuyTinConLaiTheoThang"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                    <%--<span>Số điện thoại được gửi đi:
                                        <label runat="server" id="countPhoneNumber"></label>
                                    </span>--%>
                            <asp:Label runat="server" ID="lbTinSuDung" Font-Size="25px"></asp:Label>
                        </span>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <telerik:RadCodeBlock runat="server">
        <script>
            function btLuuClick() {
                if (confirm("Bạn chắc chắn muốn gửi tin?")) {
                    return true;
                }
            }
            function change(el) {
                //var max_len = 306;
                var max_len = 459;
                if (el.value.length > max_len) {
                    el.value = el.value.substr(0, max_len);
                }
                document.getElementById('numberCharConfirm').innerHTML = el.value.length;
                if (el.value.length > 160) {
                    document.getElementById("numberCharConfirm").style.background = "red";
                    document.getElementById("numberCharConfirm").style.color = "white";
                }
                else {
                    document.getElementById("numberCharConfirm").style.background = "none";
                    document.getElementById("numberCharConfirm").style.color = "black";
                }
                if (el.value.length > 0 && el.value.length <= 160) {
                    document.getElementById('numberSMSConfirm').innerHTML = '1';
                } else if (el.value.length > 160 && el.value.length <= 306) {
                    document.getElementById('numberSMSConfirm').innerHTML = '2';
                } else if (el.value.length > 306 && el.value.length < 460) {
                    document.getElementById('numberSMSConfirm').innerHTML = '3';
                }

                var str;
                if (eval(el))
                    str = eval(el).value;
                else
                    str = el;
                str = str.replace(/á|à|ạ|ả|ã|â|ấ|ầ|ậ|ẩ|ẫ|ă|ắ|ằ|ặ|ẳ|ẵ/g, "a");
                str = str.replace(/Á|À|Ạ|Ả|Ã|Â|Ấ|Ầ|Ậ|Ẩ|Ẫ|Ă|Ắ|Ằ|Ặ|Ẳ|Ẵ/g, "A");
                str = str.replace(/é|è|ẹ|ẻ|ẽ|ê|ế|ề|ệ|ể|ễ/g, "e");
                str = str.replace(/É|È|Ẹ|Ẻ|Ẽ|Ê|Ế|Ề|Ệ|Ể|Ễ/g, "E");
                str = str.replace(/í|ì|ị|ỉ|ĩ/g, "i");
                str = str.replace(/Í|Ì|Ị|Ỉ|Ĩ/g, "I");
                str = str.replace(/ó|ò|ọ|ỏ|õ|ô|ố|ồ|ộ|ổ|ỗ|ơ|ớ|ờ|ợ|ở|ỡ/g, "o");
                str = str.replace(/Ó|Ò|Ọ|Ỏ|Õ|Ô|Ố|Ồ|Ộ|Ổ|Ỗ|Ơ|Ớ|Ờ|Ợ|Ở|Ỡ/g, "O");
                str = str.replace(/ú|ù|ụ|ủ|ũ|ư|ứ|ừ|ự|ử|ữ/g, "u");
                str = str.replace(/Ú|Ù|Ụ|Ủ|Ũ|Ư|Ứ|Ừ|Ự|Ử|Ữ/g, "U");
                str = str.replace(/ý|ỳ|ỵ|ỷ|ỹ/g, "y");
                str = str.replace(/Ý|Ỳ|Ỵ|Ỷ|Ỹ/g, "Y");
                str = str.replace(/đ|₫/g, "d");
                str = str.replace(/Đ/g, "D");

                str = str.replace("–", "-")
                .replace("‘", "'")
                .replace("’", "'")
                .replace("“", "\"")
                .replace("”", "\"")
                    .replace("》", ">");

                eval(el).value = str;
            }
        </script>
        <style>
            #ctl00_ContentPlaceHolder1_ctl00_ContentPlaceHolder1_txtTongQuyTinConLaiPanel {
                display: inline-block !important;
            }
        </style>
    </telerik:RadCodeBlock>
</asp:Content>
