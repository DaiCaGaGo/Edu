<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InSoTheoHocSinh.aspx.cs" Inherits="CMS.InSoHocSinh.InSoTheoHocSinh" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        #quaTrinhHocTap {
            border-collapse: collapse;
        }

            #quaTrinhHocTap .header {
                text-align: center;
                font-weight: bold;
            }

            #quaTrinhHocTap .noidung {
                text-align: center;
            }

            #quaTrinhHocTap tr td {
                border: 1px solid #ccc;
                height: 25px;
            }

        #hoatDongGiaoDuc {
            border-collapse: collapse;
        }

            #hoatDongGiaoDuc .header {
                text-align: center;
                font-weight: bold;
            }

            #hoatDongGiaoDuc .noidung {
                text-align: center;
            }

            #hoatDongGiaoDuc tr td {
                border: 1px solid #ccc;
                height: 25px;
            }

        #NangLuc {
            border-collapse: collapse;
        }

            #NangLuc .header {
                text-align: center;
                font-weight: bold;
            }

            #NangLuc .noidung {
                text-align: left;
            }

            #NangLuc tr td {
                border: 1px solid #ccc;
                height: 25px;
            }

        #PhamChat {
            border-collapse: collapse;
            margin-top: 20px;
        }

            #PhamChat .header {
                text-align: center;
                font-weight: bold;
            }

            #PhamChat .noidung {
                text-align: left;
            }

            #PhamChat tr td {
                border: 1px solid #ccc;
                height: 25px;
            }

        .font-bold {
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="btnConvertPdf" runat="server" CssClass="btn bt-one" OnClick="btnConvertPdf_Click" Text="Xuất sổ" />
            <h1 style="text-align: center">HỌC BẠ</h1>
            <table style="text-align: justify; width:100%">
                <tr>
                    <td style="width:50%"><span>Họ và tên học sinh: </span><asp:Label ID="lblHoTen" runat="server" Text=""></asp:Label></td>
                    <td style="width:50%" colspan="2"><span>Giới tính: </span><asp:Label ID="lblGioiTinh" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td style="width:50%"><span>Ngày, tháng, năm sinh: </span><asp:Label ID="lblNgaySinh" runat="server" Text=""></asp:Label></td>
                    <td style="width:20%"><span>Dân tộc: </span><asp:Label ID="lblDanToc" runat="server" Text=""></asp:Label></td>
                    <td style="width:30%"><span>Quốc tịch: </span><asp:Label ID="lblQuocTich" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td style="width:50%"><span>Nơi sinh: </span><asp:Label ID="lblNoiSinh" runat="server" Text=""></asp:Label></td>
                    <td style="width:50%" colspan="2"><span>Quê quán: </span><asp:Label ID="lblQueQuan" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="3"><span>Nơi ở hiện nay: </span><asp:Label ID="lblNoiOHienTai" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="3"><span>Họ và tên cha: </span><asp:Label ID="lblHoTenCha" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="3"><span>Họ và mẹ: </span><asp:Label ID="lblHoTenMe" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="3"><span>Người giám hộ (nếu có): </span><asp:Label ID="lblNguoiGiamHo" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td></td>
                    <td colspan="2">
                        <table>
                            <tr>
                                <td style="text-align: center">....................................., ngày ....... tháng ......... năm ............</td>
                            </tr>
                            <tr>
                                <td style="text-align: center">HIỆU TRƯỞNG</td>
                            </tr>
                            <tr>
                                <td style="text-align: center">(Ký, ghi rõ họ tên và đóng dấu)</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <h1 style="text-align: center">QUÁ TRÌNH HỌC TẬP</h1>
            <table runat="server" id="quaTrinhHocTap">
                <tr class="header">
                    <td style="width: 20%">Năm học</td>
                    <td style="width: 10%">Lớp</td>
                    <td style="width: 40%">Tên trường</td>
                    <td style="width: 20%">Số đăng bộ</td>
                    <td style="width: 10%">Ngày nhập học/ chuyển đến</td>
                </tr>
                <tr class="noidung">
                    <td>
                        <asp:Label ID="lblNamHoc1" runat="server" Text="20.... - 20...."></asp:Label></td>
                    <td>
                        <asp:Label ID="lblLop1" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lblTruong1" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lblSoDangBo1" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lblNgayNhapHoc1" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr class="noidung">
                    <td>
                        <asp:Label ID="lblNamHoc2" runat="server" Text="20.... - 20...."></asp:Label></td>
                    <td>
                        <asp:Label ID="lblLop2" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lblTruong2" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lblSoDangBo2" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lblNgayNhapHoc2" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr class="noidung">
                    <td>
                        <asp:Label ID="lblNamHoc3" runat="server" Text="20.... - 20...."></asp:Label></td>
                    <td>
                        <asp:Label ID="lblLop3" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lblTruong3" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lblSoDangBo3" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lblNgayNhapHoc3" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr class="noidung">
                    <td>
                        <asp:Label ID="lblNamHoc4" runat="server" Text="20.... - 20...."></asp:Label></td>
                    <td>
                        <asp:Label ID="lblLop4" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lblTruong4" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lblSoDangBo4" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lblNgayNhapHoc4" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr class="noidung">
                    <td>
                        <asp:Label ID="lblNamHoc5" runat="server" Text="20.... - 20...."></asp:Label></td>
                    <td>
                        <asp:Label ID="lblLop5" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lblTruong5" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lblSoDangBo5" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lblNgayNhapHoc5" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr class="noidung">
                    <td>
                        <asp:Label ID="lblNamHoc6" runat="server" Text="20.... - 20...."></asp:Label></td>
                    <td>
                        <asp:Label ID="lblLop6" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lblTruong6" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lblSoDangBo6" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lblNgayNhapHoc6" runat="server" Text=""></asp:Label></td>
                </tr>
            </table>
            <!--Chi tiết theo từng lớp-->
            <table style="width:100%; margin-top:20px;">
                <tr>
                    <td  colspan="2"><span class="font-bold">Họ và tên học sinh: <asp:Label ID="lblHoTen1" runat="server" Text=""></asp:Label></span></td>
                    <td style="width:30%"><span class="font-bold">Lớp: <asp:Label ID="lblTenLop1" runat="server" Text=""></asp:Label></span></td>
                </tr>
                <tr>
                    <td style="width:30%"><span>Chiều cao: </span><asp:Label ID="lblChieuCao" runat="server" Text=""></asp:Label></td>
                    <td style="width:30%"><span>Cân nặng: </span><asp:Label ID="lblCanNang" runat="server" Text=""></asp:Label></td>
                    <td style="width:30%"><span>Sức khỏe: </span><asp:Label ID="lblSucKhoe" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td><span>Số ngày nghỉ: </span><asp:Label ID="lblSoNgayNghi" runat="server" Text=""></asp:Label></td>
                    <td><span>Có phép: </span><asp:Label ID="lblNgayCoPhep" runat="server" Text=""></asp:Label></td>
                    <td><span>Không phép: </span><asp:Label ID="lblNgayKhongPhep" runat="server" Text=""></asp:Label></td>
                </tr>
            </table>
            <p class="font-bold">1. Các môn học và hoạt động giáo dục</p>
            <!-- các môn học và hoạt động giáo dục-->
            <table runat="server" id="hoatDongGiaoDuc">
                <tr class="header">
                    <td style="width: 20%">Môn học và hoạt động giáo dục</td>
                    <td style="width: 60%">Nhận xét</td>
                    <td style="width: 10%">Mức đạt được</td>
                    <td style="width: 10%">Điểm KTĐK</td>
                </tr>
                <tr class="noidung">
                    <td>Tiếng Việt</td>
                    <td>
                        <asp:Label ID="lbl_TiengViet_NhanXet" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lbl_TiengViet_MucDatDuoc" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lbl_TiengViet_DiemKTDK" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr class="noidung">
                    <td>Toán</td>
                    <td>
                        <asp:Label ID="lbl_Toan_NhanXet" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lbl_Toan_MucDatDuoc" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lbl_Toan_DiemKTDK" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr class="noidung">
                    <td>Tự nhiên và xã hội/ Khoa học</td>
                    <td>
                        <asp:Label ID="lbl_TNXH_NhanXet" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lbl_TNXH_MucDatDuoc" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lbl_TNXH_DiemKTDK" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr class="noidung">
                    <td>Lịch sử và Địa lí</td>
                    <td>
                        <asp:Label ID="lbl_LichSu_NhanXet" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lbl_LichSu_MucDatDuoc" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lbl_LichSu_DiemKTDK" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr class="noidung">
                    <td>Ngoại ngữ<br />
                        ..........................</td>
                    <td>
                        <asp:Label ID="lbl_NgoaiNgu_NhanXet" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lbl_NgoaiNgu_MucDatDuoc" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lbl_NgoaiNgu_DiemKTDK" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr class="noidung">
                    <td>Tin học</td>
                    <td>
                        <asp:Label ID="lbl_TinHoc_NhanXet" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lbl_TinHoc_MucDatDuoc" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lbl_TinHoc_DiemKTDK" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr class="noidung">
                    <td>Tiếng dân tộc</td>
                    <td>
                        <asp:Label ID="lbl_TiengDanToc_NhanXet" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lbl_TiengDanToc_MucDatDuoc" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lbl_TiengDanToc_DiemKTDK" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr class="noidung">
                    <td>Đạo đức</td>
                    <td>
                        <asp:Label ID="lbl_DaoDuc_NhanXet" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lbl_DaoDuc_MucDatDuoc" runat="server" Text=""></asp:Label></td>
                    <td rowspan="5"></td>
                </tr>
                <tr class="noidung">
                    <td>Âm nhạc</td>
                    <td>
                        <asp:Label ID="lbl_AmNhac_NhanXet" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lbl_AmNhac_MucDatDuoc" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr class="noidung">
                    <td>Mĩ thuật</td>
                    <td>
                        <asp:Label ID="lbl_MiThuat_NhanXet" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lbl_MiThuat_MucDatDuoc" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr class="noidung">
                    <td>Thủ công/ Kĩ thuật</td>
                    <td>
                        <asp:Label ID="lbl_ThuCong_NhanXet" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lbl_ThuCong_MucDatDuoc" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr class="noidung">
                    <td>Thể dục</td>
                    <td>
                        <asp:Label ID="lbl_TheDuc_NhanXet" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="lbl_TheDuc_MucDatDuoc" runat="server" Text=""></asp:Label></td>
                </tr>
            </table>
            <!--End môn học và hoạt động giáo dục-->
            <table style="width:100%; margin-top:20px;">
                <tr>
                    <td style="width:60%"><span class="font-bold">Trường: </span><asp:Label ID="lblTenTruong" runat="server" Text="...................................................................................................................................."></asp:Label></td>
                    <td style="width:40%"><span class="font-bold">Năm học: </span><asp:Label ID="lblNamHoc" runat="server" Text="...................................."></asp:Label></td>
                </tr>
            </table>
            <p class="font-bold">2. Năng lực, phẩm chất</p>
            <!--Các năng, lực phẩm chất-->
            <table runat="server" id="NangLuc">
                <tr class="header">
                    <td style="width: 20%">Năng lực</td>
                    <td style="width: 10%">Mức đạt được </td>
                    <td style="width: 70%">Nhận xét</td>
                </tr>
                <tr class="noidung">
                    <td>Tự phục vụ, tự quản</td>
                    <td>
                        <asp:Label ID="lblTuPhucVu" runat="server" Text="" Style="text-align: center"></asp:Label></td>
                    <td rowspan="3">
                        <asp:Label ID="lbl_NL_NhanXet" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr class="noidung">
                    <td>Hợp tác</td>
                    <td>
                        <asp:Label ID="lblHopTac" runat="server" Text="" Style="text-align: center"></asp:Label></td>
                </tr>
                <tr class="noidung">
                    <td>Tự học, giải quyết vấn đề</td>
                    <td>
                        <asp:Label ID="lblTuHoc" runat="server" Text="" Style="text-align: center"></asp:Label></td>
                </tr>
            </table>
            <table runat="server" id="PhamChat">
                <tr class="header">
                    <td style="width: 20%">Phẩm chất</td>
                    <td style="width: 10%">Mức đạt được </td>
                    <td style="width: 70%">Nhận xét</td>
                </tr>
                <tr class="noidung">
                    <td>Chăm học, chăm làm</td>
                    <td>
                        <asp:Label ID="lblChamHoc" runat="server" Text="" Style="text-align: center"></asp:Label></td>
                    <td rowspan="4">
                        <asp:Label ID="lbl_PC_NhanXet" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr class="noidung">
                    <td>Tự tin, trách nhiệm</td>
                    <td>
                        <asp:Label ID="lblTuTin" runat="server" Text="" Style="text-align: center"></asp:Label></td>
                </tr>
                <tr class="noidung">
                    <td>Trung thực, kỉ luật</td>
                    <td>
                        <asp:Label ID="lblTrungThuc" runat="server" Text="" Style="text-align: center"></asp:Label></td>
                </tr>
                <tr class="noidung">
                    <td>Đoàn kết, yêu thương</td>
                    <td>
                        <asp:Label ID="lblDoanKet" runat="server" Text="" Style="text-align: center"></asp:Label></td>
                </tr>
            </table>
            <!--End năng lực, phẩm chất-->
            <span class="font-bold">Khen thưởng: </span>
            <asp:Label ID="lblKhenThuong" runat="server" Text=".........................................................................................................................................................................................................................................."></asp:Label>
            <br />
            <span class="font-bold">Hoàn thành chương trình lớp học/ chương trình tiểu học: </span>
            <asp:Label ID="lblHoangThanhChuongTrinh" runat="server" Text="......................................................................................................................."></asp:Label>
            <table style="width:100%">
                <tr>
                    <td style="width: 50%; text-align: center">
                        <table>
                            <tr></tr>
                            <tr>
                                <td>Xác nhận của Hiệu trưởng</td>
                            </tr>
                            <tr>
                                <td>(Ký, ghi rõ họ tên và đóng dấu)</td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 50%; text-align: center">
                        <table>
                            <tr>
                                <td>....................................., ngày ....... tháng ......... năm ............</td>
                            </tr>
                            <tr>
                                <td>Giáo viên chủ nhiệm</td>
                            </tr>
                            <tr>
                                <td>(Ký, ghi rõ họ tên và đóng dấu)</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
