using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Drawing;

namespace CMS.HocSinh
{
    public partial class HocSinhImportExcel : AuthenticatePage
    {
        private KhoiBO khoiBO = new KhoiBO();
        private LopBO lopBO = new LopBO();
        private HocSinhBO hocSinhBO = new HocSinhBO();
        private LocalAPI localAPI = new LocalAPI();
        private GioiTinhBO gioiTinhBO = new GioiTinhBO();
        private TrangThaiHSBO trangThaiHSBO = new TrangThaiHSBO();
        private DmQuocTichBO dmQuocTichBO = new DmQuocTichBO();
        private DmDanTocBO dmDanTocBO = new DmDanTocBO();
        private DmDoiTuongCSBO dmDoiTuongCSBO = new DmDoiTuongCSBO();
        private DmKhuVucBO dmKhuVucBO = new DmKhuVucBO();
        private DmTinhThanhBO dmTinhThanhBO = new DmTinhThanhBO();
        private DmQuanHuyenBO dmQuanHuyenBO = new DmQuanHuyenBO();
        private DmXaPhuongBO dmXaPhuongBO = new DmXaPhuongBO();
        LogUserBO logUserBO = new LogUserBO();
        List<string> lstValue = new List<string> { "HO_TEN", "SDT_NHAN_TIN", "ID_KHOI", "ID_LOP", "NGAY_SINH", "MA_GIOI_TINH", "TRANG_THAI_HOC", "THU_TU", "IS_CON_GV", "IS_DK_KY1", "IS_DK_KY2", "IS_MIEN_GIAM_KY1", "IS_MIEN_GIAM_KY2", "MA_QUOC_TICH", "MA_DAN_TOC", "MA_DOI_TUONG_CS", "MA_KHU_VUC", "MA_TINH_THANH", "MA_QUAN_HUYEN", "MA_XA_PHUONG", "NOI_SINH", "DIA_CHI", "SO_CMND", "NOI_CAP_CMND", "NGAY_CAP_CMND", "HO_TEN_BO", "NAM_SINH_BO", "SDT_BO", "HO_TEN_ME", "NAM_SINH_ME", "SDT_ME", "HO_TEN_NGUOI_BAO_HO", "NAM_SINH_NGUOI_BAO_HO", "SDT_NBH" };
        List<string> lstText = new List<string> { "Họ và tên", "SĐT nhắn tin", "Khối", "Lớp", "Ngày sinh", "Giới tính", "Trạng thái học", "Thứ tự", "Con giáo viên", "Đăng ký SMS kỳ 1", "Đăng ký SMS kỳ 2", "Miễn phí SMS kỳ 1", "Miễn phí SMS kỳ 2", "Quốc tịch", "Dân tộc", "Đối tượng chính sách", "Khu vực", "Tỉnh/Thành phố", "Quận/Huyện", "Xã/Phường", "Nơi sinh", "Địa chỉ thường trú", "Số CMND", "Nơi cấp", "Ngày cấp", "Họ tên bố", "Năm sinh bố", "SĐT bố", "Họ tên mẹ", "Năm sinh mẹ", "SĐT mẹ", "Họ tên người bảo hộ", "Năm sinh người bảo hộ", "SĐT người bảo hộ" };
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            bt_EXCELtoSQL.Visible = is_access(SYS_Type_Access.THEM);
            if (!IsPostBack)
            {
                // combobox chon cot import
                rcbColumn.Items.Clear();
                for (int i = 0; i < lstValue.Count; i++)
                {
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Value = lstValue[i];
                    item.Text = lstText[i];
                    item.Checked = true;
                    rcbColumn.Items.Add(item);
                }
                Session["ExcelHocSinh" + Sys_User.ID] = null;
                Session["ExcelHocSinhMessageId" + Sys_User.ID] = null;
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            if (Session["ExcelHocSinh" + Sys_User.ID] != null)
            {
                dt = (DataTable)Session["ExcelHocSinh" + Sys_User.ID];
            }

            if (dt != null && dt.Columns.Count > 0)
                RadGrid1.DataSource = dt;

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
        protected void bt_importSQL_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.THEM))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            try
            {
                UploadFile();
                if (Session["ExcelHocSinh" + Sys_User.ID] != null)
                {
                    DataTable dt = (DataTable)Session["ExcelHocSinh" + Sys_User.ID];
                    if (dt.Rows.Count > 0)
                    {
                        RadGrid1.Rebind();
                        bt_EXCELtoSQL.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Write(ex.ToString());
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Không có dữ liệu hoặc tệp sai cấu trúc!');", true);
            }
        }
        protected void UploadFile()
        {
            try
            {
                if (FileExcel.HasFile)
                {
                    string FileName = Path.GetFileName(FileExcel.PostedFile.FileName);
                    string Extension = Path.GetExtension(FileExcel.PostedFile.FileName);
                    string FolderPath = "/Tmps/";
                    if ((Extension == ".xls") || (Extension == ".xlsx"))
                    {
                        string message_id = DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds.ToString();
                        string FilePath = Server.MapPath(FolderPath + message_id + FileName);
                        FileExcel.SaveAs(FilePath);
                        DataTable dt = new DataTable();
                        dt = GetDataTableFromExcel(FilePath, "DuLieu", "*", true);
                        if (dt == null || dt.Columns.Count < lstValue.Count + 2)
                        {
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'File chưa đúng mẫu, hoặc dữ liệu trống. Vui lòng kiểm tra lại!');", true);
                        }
                        else if (dt.Columns.Count < lstValue.Count + 2)
                        {
                            for (int i = dt.Columns.Count - 1; i > lstValue.Count + 2; i--)
                                dt.Columns.RemoveAt(i);
                        }
                        Session["ExcelHocSinh" + Sys_User.ID] = dt;
                        Session["ExcelHocSinhMessageId" + Sys_User.ID] = message_id;

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Chỉ cho phép import dữ liệu từ file Excel (*.xls, *.xlsx)');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn chưa lựa chọn file excel!');", true);
                }
            }
            catch (Exception ms)
            {
                Response.Write(ms.ToString());
                //ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Có lỗi xảy ra!');", true);
            }
        }
        public DataTable GetDataTableFromExcel(string filename, string sheet, string danhsachcot, bool multiRow)
        {
            try
            {
                DataTable myDataTable = new DataTable();
                string strConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + @";Extended Properties=""Excel 12.0 Xml;HDR=YES""";
                OleDbConnection connExcel = new OleDbConnection(strConn);
                OleDbCommand cmdExcel = new OleDbCommand();
                OleDbDataAdapter oda = new OleDbDataAdapter();
                cmdExcel.Connection = connExcel;

                //Get the name of First Sheet
                connExcel.Open();
                DataTable dtExcelSchema;
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                connExcel.Close();

                //Read Data from First Sheet
                connExcel.Open();
                cmdExcel.CommandText = "SELECT * From [" + sheet + "$]";
                oda.SelectCommand = cmdExcel;
                oda.Fill(myDataTable);
                connExcel.Close();
                DataTable dt = new DataTable();
                #region Change Name column
                #region Set data theo khối và đợt dữ liệu
                myDataTable.Columns[0].ColumnName = "STT";
                myDataTable.Columns[1].ColumnName = "MA";
                if (myDataTable.Columns.Count < lstValue.Count + 2)
                {
                    return null;
                }
                for (int i = 0; i <= lstValue.Count; i++)
                {
                    try
                    {
                        if (myDataTable.Columns[i + 2].ColumnName.ToNormalizeLowerRelace() == lstText[i].ToNormalizeLowerRelace())
                            myDataTable.Columns[i + 2].ColumnName = lstValue[i];
                        else
                            return null;
                    }
                    catch { }
                }
                #endregion
                #endregion
                #region Valid excel
                if (myDataTable.Rows.Count != 0)
                {
                    dt = myDataTable.Copy();
                    int i = 0;
                    if (multiRow == true)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            Boolean kt = true;
                            if (string.IsNullOrEmpty(row[2].ToString().Trim()))
                            {
                                kt = false;
                            }

                            // chuyen doi du lieu

                            if (!kt)
                            {
                                myDataTable.Rows.RemoveAt(i);
                            }
                            else
                                i++;
                        }
                    }
                }
                if (myDataTable.Rows.Count > 500)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Không upload file vượt quá 500 bản ghi!');", true);
                    return null;
                }
                #endregion
                return myDataTable;
            }
            catch (OleDbException ex)
            {
                //Response.Write(ex.ToString());
                if (ex.ErrorCode == -2147467259)
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn phải đặt tên sheet dữ liệu là Sheet1 hoặc định dạng dữ liệu không đúng!');", true);
                else ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Có lỗi xảy ra!');", true);

            }
            catch (Exception ex)
            {
                //Response.Write(ex.ToString());
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Có lỗi xảy ra!');", true);
            }
            return new DataTable();
        }
        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                TextBox tbNgaySinh = (TextBox)e.Item.FindControl("tbNgaySinh");
                HiddenField hdNgaySinh = (HiddenField)e.Item.FindControl("hdNgaySinh");
                CheckBox chkConGV = (CheckBox)e.Item.FindControl("cbConGV");
                HiddenField hdConGV = (HiddenField)e.Item.FindControl("hdConGV");
                CheckBox chkSMS1 = (CheckBox)e.Item.FindControl("cbSMS1");
                HiddenField hdSMS1 = (HiddenField)e.Item.FindControl("hdSMS1");
                CheckBox chkSMS2 = (CheckBox)e.Item.FindControl("cbSMS2");
                HiddenField hdSMS2 = (HiddenField)e.Item.FindControl("hdSMS2");
                CheckBox chkMienPhi1 = (CheckBox)e.Item.FindControl("cbMienPhi1");
                HiddenField hdMienPhi1 = (HiddenField)e.Item.FindControl("hdMienPhi1");
                CheckBox chkMienPhi2 = (CheckBox)e.Item.FindControl("cbMienPhi2");
                HiddenField hdMienPhi2 = (HiddenField)e.Item.FindControl("hdMienPhi2");
                System.Web.UI.HtmlControls.HtmlImage error1 = (System.Web.UI.HtmlControls.HtmlImage)item.FindControl("iconErr");
                System.Web.UI.HtmlControls.HtmlImage success1 = (System.Web.UI.HtmlControls.HtmlImage)item.FindControl("iconSuccess");
                error1.Visible = false;
                success1.Visible = false;

                #region checkbox
                string strConGV = hdConGV.Value.Trim();
                string strSMS1 = hdSMS1.Value.Trim();
                string strSMS2 = hdSMS2.Value.Trim();
                string strMienPhi1 = hdMienPhi1.Value.Trim();
                string strMienPhi2 = hdMienPhi2.Value.Trim();
                if (strConGV == "1") chkConGV.Checked = true;
                if (strSMS1 == "1") chkSMS1.Checked = true;
                if (strSMS2 == "1") chkSMS2.Checked = true;
                if (strMienPhi1 == "1") chkMienPhi1.Checked = true;
                if (strMienPhi2 == "1") chkMienPhi2.Checked = true;
                #endregion
                #region Ngày sinh
                if (!string.IsNullOrEmpty(tbNgaySinh.Text))
                {
                    try
                    {
                        tbNgaySinh.Text = Convert.ToDateTime(hdNgaySinh.Value).ToString("dd/MM/yyyy");
                    }
                    catch { }
                }
                #endregion
            }
        }
        protected void bt_EXCELtoSQL_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.THEM))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            if (Session["ExcelHocSinh" + Sys_User.ID] != null && Session["ExcelHocSinhMessageId" + Sys_User.ID] != null)
            {
                List<ResultError> lstRes = new List<ResultError>();
                List<ResultEntity> lstResEntity = new List<ResultEntity>();
                ResultError res = new ResultError();
                CommonValidate validate = new CommonValidate();

                DataTable dt = new DataTable();
                dt = (DataTable)Session["ExcelHocSinh" + Sys_User.ID];
                int count_success = 0;
                int count_total = 0;
                int count_row = 0;
                List<string> lstColumn = new List<string>();
                var collection = rcbColumn.CheckedItems;
                if (collection.Count != 0)
                {
                    for (int i = 0; i < collection.Count; i++)
                    {
                        lstColumn.Add(collection[i].Value);
                    }
                }

                List<GIOI_TINH> lstGioiTinh = gioiTinhBO.getGioiTinh();
                List<KHOI> lstKhoi = khoiBO.getKhoiByCapHoc(Sys_This_Cap_Hoc);
                List<DM_QUOC_TICH> lstQuocTich = dmQuocTichBO.getQuocTich();
                List<DM_DAN_TOC> lstDanToc = dmDanTocBO.getDanToc();
                List<TRANG_THAI_HS> lstTrangThai = trangThaiHSBO.getTrangThaiHS();
                List<DM_DOI_TUONG_CHINH_SACH> lstDoiTuong = dmDoiTuongCSBO.getDoiTuongChinhSach();
                List<DM_KHU_VUC> lstKhuVuc = dmKhuVucBO.getKhuVuc();
                List<DM_TINH_THANH> lstTinh = dmTinhThanhBO.getTinhThanh();
                foreach (GridDataItem row in RadGrid1.Items)
                {
                    ResultEntity resEntity = new ResultEntity();
                    resEntity.Res = true;
                    count_row++;
                    count_total++;
                    object val = new object();
                    string out_sdt = string.Empty;
                    string error = string.Empty;
                    HOC_SINH entity = new HOC_SINH();

                    #region get control
                    TextBox tbStt = (TextBox)row.FindControl("tbSTT");
                    TextBox tbMA = (TextBox)row.FindControl("tbMA");
                    TextBox tbHoTen = (TextBox)row.FindControl("tbHoTen");
                    TextBox tbKhoi = (TextBox)row.FindControl("tbIdKhoi");
                    TextBox tbLop = (TextBox)row.FindControl("tbIdLop");
                    TextBox tbNgaySinh = (TextBox)row.FindControl("tbNgaySinh");
                    CheckBox chkConGV = (CheckBox)row.FindControl("cbConGV");
                    CheckBox chkSMS1 = (CheckBox)row.FindControl("cbSMS1");
                    CheckBox chkSMS2 = (CheckBox)row.FindControl("cbSMS2");
                    CheckBox chkMienPhi1 = (CheckBox)row.FindControl("cbMienPhi1");
                    CheckBox chkMienPhi2 = (CheckBox)row.FindControl("cbMienPhi2");
                    TextBox tbSDTNhanTin = (TextBox)row.FindControl("tbSDTNhanTin");
                    TextBox tbGioiTinh = (TextBox)row.FindControl("tbGioiTinh");
                    TextBox tbTrangThai = (TextBox)row.FindControl("tbTrangThaiHoc");
                    TextBox tbThuTu = (TextBox)row.FindControl("tbThuTu");
                    TextBox tbQuocTich = (TextBox)row.FindControl("tbQuocTich");
                    TextBox tbDanToc = (TextBox)row.FindControl("tbDanToc");
                    TextBox tbDoiTuong = (TextBox)row.FindControl("tbDoiTuong");
                    TextBox tbKhuVuc = (TextBox)row.FindControl("tbKhuVuc");
                    TextBox tbTinh = (TextBox)row.FindControl("tbTinh");
                    TextBox tbQuanHuyen = (TextBox)row.FindControl("tbQuanHuyen");
                    TextBox tbXaPhuong = (TextBox)row.FindControl("tbXaPhuong");
                    TextBox tbNoiSinh = (TextBox)row.FindControl("tbNoiSinh");
                    TextBox tbDiaChi = (TextBox)row.FindControl("tbDiaChi");
                    TextBox tbSoCMND = (TextBox)row.FindControl("tbSoCMND");
                    TextBox tbNoiCapCMND = (TextBox)row.FindControl("tbNoiCapCMND");
                    TextBox tbNgayCapCMND = (TextBox)row.FindControl("tbNgayCapCMND");
                    TextBox tbHoTenBo = (TextBox)row.FindControl("tbHoTenBo");
                    TextBox tbNamSinhBo = (TextBox)row.FindControl("tbNamSinhBo");
                    TextBox tbSDTBo = (TextBox)row.FindControl("tbSDTBo");
                    TextBox tbHoTenMe = (TextBox)row.FindControl("tbHoTenMe");
                    TextBox tbNamSinhMe = (TextBox)row.FindControl("tbNamSinhMe");
                    TextBox tbSDTMe = (TextBox)row.FindControl("tbSDTMe");
                    TextBox tbHoTenNBH = (TextBox)row.FindControl("tbHoTenNBH");
                    TextBox tbNamSinhNBH = (TextBox)row.FindControl("tbNamSinhNBH");
                    TextBox tbSDTNBH = (TextBox)row.FindControl("tbSDTNBH");
                    HiddenField hdresMsg = (HiddenField)row.FindControl("hdresMsg");
                    System.Web.UI.HtmlControls.HtmlImage icError = (System.Web.UI.HtmlControls.HtmlImage)row.FindControl("iconErr");
                    System.Web.UI.HtmlControls.HtmlImage icSuccess = (System.Web.UI.HtmlControls.HtmlImage)row.FindControl("iconSuccess");
                    icError.Visible = false;
                    icSuccess.Visible = false;
                    #endregion

                    #region set value
                    short hdGioiTinh = -1;
                    string id = tbStt.Text.Trim();
                    string ma = tbMA.Text.Trim();
                    string hoTen = (tbHoTen != null) ? tbHoTen.Text.Trim() : "";
                    #region Giới tính
                    string strGioiTinh = tbGioiTinh.Text.ToNormalizeLowerRelace();
                    if (lstGioiTinh != null)
                    {
                        GIOI_TINH gt = lstGioiTinh.FirstOrDefault(x => x.TEN.ToNormalizeLowerRelace() == strGioiTinh);
                        if (gt != null)
                        {
                            hdGioiTinh = gt.MA;
                        }
                    }
                    #endregion
                    #region Khối
                    short idKhoi = 0;
                    string strKhoi = tbKhoi.Text.ToNormalizeLowerRelace();
                    if (!string.IsNullOrEmpty(strKhoi) && lstKhoi != null)
                    {
                        KHOI khoi = lstKhoi.FirstOrDefault(x => x.TEN.ToNormalizeLowerRelace().Contains(strKhoi));
                        if (khoi != null)
                        {
                            idKhoi = khoi.MA;
                        }
                    }
                    #endregion
                    #region Lớp
                    long idLop = 0;
                    string strLop = tbLop.Text.ToNormalizeLowerRelace();
                    List<LopEntity> lstLop = lopBO.getLopByTruongCapHocAndKhoi(Sys_This_Cap_Hoc, Sys_This_Truong.ID, idKhoi, Convert.ToInt16(Sys_Ma_Nam_hoc), null);
                    if (!string.IsNullOrEmpty(strLop) && lstLop != null)
                    {
                        LOP lop = lstLop.FirstOrDefault(x => x.TEN.ToNormalizeLowerRelace().Contains(strLop));
                        if (lop != null)
                        {
                            idLop = lop.ID;
                        }
                    }
                    #endregion
                    #region Quốc tịch
                    short hdQuocTich = 0;
                    string strQuocTich = tbQuocTich.Text.ToNormalizeLowerRelace();
                    if (lstQuocTich != null)
                    {
                        DM_QUOC_TICH dmQuocTich = lstQuocTich.FirstOrDefault(x => x.TEN.ToNormalizeLowerRelace().Contains(strQuocTich));
                        if (dmQuocTich != null)
                            hdQuocTich = dmQuocTich.MA;
                        else
                            hdQuocTich = 0;
                    }
                    else
                        hdQuocTich = 0;
                    #endregion
                    #region Dân tộc
                    short hdDanToc = 0;
                    string strDanToc = tbDanToc.Text.ToNormalizeLowerRelace();
                    if (lstDanToc != null)
                    {
                        DM_DAN_TOC dmDanToc = lstDanToc.FirstOrDefault(x => x.TEN.ToNormalizeLowerRelace().Contains(strDanToc));
                        if (dmDanToc != null)
                            hdDanToc = dmDanToc.MA;
                        else
                            hdDanToc = 0;
                    }
                    else
                        hdDanToc = 0;
                    #endregion
                    #region Trạng thái học
                    short hdTrangThai = 0;
                    string strTrangThai = tbTrangThai.Text.ToNormalizeLowerRelace();
                    if (lstTrangThai != null)
                    {
                        TRANG_THAI_HS trangThaiHS = lstTrangThai.FirstOrDefault(x => x.TEN.ToNormalizeLowerRelace() == strTrangThai);
                        if (trangThaiHS != null)
                        {
                            hdTrangThai = trangThaiHS.MA;
                        }
                        else
                            hdTrangThai = 0;
                    }
                    else
                        hdTrangThai = 0;
                    #endregion
                    #region Đối tượng chính sách
                    short hdDoiTuong = 0;
                    string strDoiTuong = tbDoiTuong.Text.ToNormalizeLowerRelace();
                    if (lstDoiTuong != null && !string.IsNullOrEmpty(strDoiTuong))
                    {
                        DM_DOI_TUONG_CHINH_SACH dmDoiTuong = lstDoiTuong.FirstOrDefault(x => x.TEN.ToNormalizeLowerRelace().Contains(strDoiTuong));
                        if (dmDoiTuong != null)
                            hdDoiTuong = dmDoiTuong.MA;
                        else
                            hdDoiTuong = 0;
                    }
                    else
                        hdDoiTuong = 0;
                    #endregion
                    #region Khu vực	
                    short hdKhuVuc = 0;
                    string strKhuVuc = tbKhuVuc.Text.ToNormalizeLowerRelace();
                    if (lstKhuVuc != null && !string.IsNullOrEmpty(strKhuVuc))
                    {
                        DM_KHU_VUC dmKhuVuc = lstKhuVuc.FirstOrDefault(x => x.TEN.ToNormalizeLowerRelace().Contains(strKhuVuc));
                        if (dmKhuVuc != null)
                            hdKhuVuc = dmKhuVuc.MA;
                        else
                            hdKhuVuc = 0;
                    }
                    else
                        hdKhuVuc = 0;
                    #endregion
                    #region Tỉnh thành
                    short strMaTinh = 0;
                    short hdTinh = 0;
                    string strTinh = tbTinh.Text.ToNormalizeLowerRelace();
                    if (!string.IsNullOrEmpty(strTinh))
                    {
                        if (lstTinh != null && !string.IsNullOrEmpty(strTinh))
                        {
                            DM_TINH_THANH dmTinh = lstTinh.FirstOrDefault(x => x.TEN.ToNormalizeLowerRelace().Contains(strTinh));
                            if (dmTinh != null)
                                hdTinh = strMaTinh = dmTinh.MA;
                            else
                                hdTinh = 0;
                        }
                        else
                            hdTinh = 0;
                    }
                    else
                        hdTinh = 0;
                    #endregion
                    #region Quận/Huyện
                    short strMaQuan = 0;
                    short hdQuanHuyen = 0;
                    string strQuanHuyen = tbQuanHuyen.Text.ToNormalizeLowerRelace();
                    if (strMaTinh != 0)
                    {
                        List<DM_QUAN_HUYEN> lstQuanHuyen = dmQuanHuyenBO.getQuanHuyenByTinh(strMaTinh);
                        if (lstQuanHuyen != null && !string.IsNullOrEmpty(strQuanHuyen))
                        {
                            DM_QUAN_HUYEN dmQuanHuyen = lstQuanHuyen.FirstOrDefault(x => x.TEN.ToNormalizeLowerRelace().Contains(strQuanHuyen));
                            if (dmQuanHuyen != null)
                                hdQuanHuyen = strMaQuan = dmQuanHuyen.MA;
                            else
                                hdQuanHuyen = 0;
                        }
                        else
                            hdQuanHuyen = 0;
                    }
                    else
                        hdQuanHuyen = 0;
                    #endregion
                    #region Xã/Phường
                    long hdXaPhuong = 0;
                    string strXaPhuong = tbXaPhuong.Text.ToNormalizeLowerRelace();
                    if (strMaTinh != 0 && strMaQuan != 0)
                    {
                        List<DM_XA_PHUONG> lstXaPhuong = dmXaPhuongBO.getXaPhuongByTinhHuyen(strMaTinh, strMaQuan);
                        if (lstXaPhuong != null && !string.IsNullOrEmpty(strXaPhuong))
                        {
                            DM_XA_PHUONG dmXaPhuong = lstXaPhuong.FirstOrDefault(x => x.TEN.ToNormalizeLowerRelace().Contains(strXaPhuong));
                            if (dmXaPhuong != null)
                            {
                                hdXaPhuong = dmXaPhuong.MA;
                            }
                            else
                            {
                                hdXaPhuong = 0;
                            }
                        }
                        else
                        {
                            hdXaPhuong = 0;
                        }
                    }
                    else
                        hdXaPhuong = 0;
                    #endregion
                    #region Ngày cấp CMND
                    string strNgayCap = tbNgayCapCMND.Text.Trim();
                    if (!string.IsNullOrEmpty(strNgayCap)) tbNgayCapCMND.Text = localAPI.ConvertDDMMYYToDateTime(strNgayCap).ToString();
                    #endregion
                    string ngaySinh = (tbNgaySinh != null) ? tbNgaySinh.Text : "";
                    bool bConGV = chkConGV.Checked;
                    bool bSMS1 = chkSMS1.Checked;
                    bool bSMS2 = chkSMS2.Checked;
                    bool bMienPhi1 = chkMienPhi1.Checked;
                    bool bMienPhi2 = chkMienPhi2.Checked;
                    string strSDTNhanTin = (tbSDTNhanTin != null) ? tbSDTNhanTin.Text.Trim() : "";
                    string strThuTu = (tbThuTu != null) ? tbThuTu.Text.Trim() : "";
                    string strNoiSinh = (tbNoiSinh != null) ? tbNoiSinh.Text.Trim() : "";
                    string strDiaChi = (tbDiaChi != null) ? tbDiaChi.Text.Trim() : "";
                    string strSoCMND = (tbSoCMND != null) ? tbSoCMND.Text.Trim() : "";
                    string strNoiCapCMND = (tbNoiCapCMND != null) ? tbNoiCapCMND.Text.Trim() : "";
                    string strNgayCapCMND = (tbNgayCapCMND != null) ? tbNgayCapCMND.Text.Trim() : "";
                    string strHoTenBo = (tbHoTenBo != null) ? tbHoTenBo.Text.Trim() : "";
                    string strNamSinhBo = (tbNamSinhBo != null) ? tbNamSinhBo.Text.Trim() : "";
                    string strSDTBo = (tbSDTBo != null) ? tbSDTBo.Text.Trim() : "";
                    string strHoTenMe = (tbHoTenMe != null) ? tbHoTenMe.Text.Trim() : "";
                    string strNamSinhMe = (tbNamSinhMe != null) ? tbNamSinhMe.Text.Trim() : "";
                    string strSDTMe = (tbSDTMe != null) ? tbSDTMe.Text.Trim() : "";
                    string strHoTenNBH = (tbHoTenNBH != null) ? tbHoTenNBH.Text.Trim() : "";
                    string strNamSinhNBH = (tbNamSinhNBH != null) ? tbNamSinhNBH.Text.Trim() : "";
                    string strSDTNBH = (tbSDTNBH != null) ? tbSDTNBH.Text.Trim() : "";
                    #region check stt
                    if (validate.ValInt(true, null, null, id, out val, out error))
                    {
                        entity.ID = Convert.ToInt64(id);
                        //entity.THU_TU = localAPI.ConvertStringToShort(id);
                    }
                    else
                    {
                        res.MA_LOI = count_row;
                        res.TEN_LOI = error;
                        res.COT = localAPI.ExcelColumnNameToNumber("A") - 1;
                        res.DONG = count_row;
                        res.TEN_COT = "STT";
                        lstRes.Add(res);
                        resEntity.Res = false;
                        if (string.IsNullOrEmpty(resEntity.Msg))
                            resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                        else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                    }
                    #endregion
                    entity.MA = ma;
                    #region check khoi
                    if (validate.ValInt(true, null, null, idKhoi.ToString(), out val, out error) && idKhoi > 0)
                    {
                        entity.ID_KHOI = Convert.ToInt16(idKhoi);
                    }
                    else
                    {
                        res.MA_LOI = count_row;
                        res.TEN_LOI = error;
                        if (idKhoi == 0)
                            res.TEN_LOI = " chưa tồn tại trong hệ thống, vui lòng kiểm tra lại!";
                        res.COT = localAPI.ExcelColumnNameToNumber("C") - 1;
                        res.DONG = count_row;
                        res.TEN_COT = "Khối";
                        lstRes.Add(res);
                        resEntity.Res = false;
                        if (string.IsNullOrEmpty(resEntity.Msg))
                            resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                        else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                    }
                    #endregion
                    #region check lop
                    if (validate.ValInt(true, null, null, idLop.ToString(), out val, out error) && idLop > 0)
                    {
                        entity.ID_LOP = Convert.ToInt64(idLop);
                    }
                    else
                    {
                        res.MA_LOI = count_row;
                        res.TEN_LOI = error;
                        if (idLop == 0)
                            res.TEN_LOI = " chưa tồn tại trong hệ thống, vui lòng kiểm tra lại!";
                        res.COT = localAPI.ExcelColumnNameToNumber("D") - 1;
                        res.DONG = count_row;
                        res.TEN_COT = "Lớp";
                        lstRes.Add(res);
                        resEntity.Res = false;
                        if (string.IsNullOrEmpty(resEntity.Msg))
                            resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                        else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                    }
                    #endregion
                    #region check giới tính
                    if (validate.ValString(true, null, strGioiTinh, out val, out error))
                    {
                        entity.MA_GIOI_TINH = hdGioiTinh;
                    }
                    else
                    {
                        res.MA_LOI = count_row;
                        res.TEN_LOI = error;
                        res.COT = localAPI.ExcelColumnNameToNumber("G") - 1;
                        res.DONG = count_row;
                        res.TEN_COT = "Giới tính";
                        lstRes.Add(res);
                        resEntity.Res = false;
                        if (string.IsNullOrEmpty(resEntity.Msg))
                            resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                        else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                    }
                    #endregion
                    if (!string.IsNullOrEmpty(ngaySinh)) entity.NGAY_SINH = localAPI.ConvertDDMMYYToDateTime(ngaySinh);
                    if (!string.IsNullOrEmpty(strTrangThai)) entity.TRANG_THAI_HOC = hdTrangThai;
                    entity.IS_CON_GV = bConGV;
                    entity.IS_DK_KY1 = bSMS1;
                    entity.IS_DK_KY2 = bSMS2;
                    entity.IS_MIEN_GIAM_KY1 = bMienPhi1;
                    entity.IS_MIEN_GIAM_KY1 = bMienPhi2;

                    #region check so dien thoai nhan tin
                    if (string.IsNullOrEmpty(strSDTNhanTin))
                        entity.SDT_NHAN_TIN = " ";
                    else
                    {
                        if (validate.ValidateSDT(false, strSDTNhanTin, out out_sdt, out error))
                        {
                            entity.SDT_NHAN_TIN = out_sdt;
                        }
                        else
                        {
                            res.MA_LOI = count_row;
                            res.TEN_LOI = error;
                            res.COT = localAPI.ExcelColumnNameToNumber("M") - 1;
                            res.DONG = count_row;
                            res.TEN_COT = "Số điện thoại nhắn tin";
                            lstRes.Add(res);
                            resEntity.Res = false;
                            if (string.IsNullOrEmpty(resEntity.Msg))
                                resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                            else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                        }
                    }
                    #endregion 
                    #region check ho ten
                    if (validate.ValString(true, null, hoTen, out val, out error))
                    {
                        entity.HO_TEN = hoTen;
                    }
                    else
                    {
                        res.MA_LOI = count_row;
                        res.TEN_LOI = error;
                        res.COT = localAPI.ExcelColumnNameToNumber("B") - 1;
                        res.DONG = count_row;
                        res.TEN_COT = "Họ tên";
                        lstRes.Add(res);
                        resEntity.Res = false;
                        if (string.IsNullOrEmpty(resEntity.Msg))
                            resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                        else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                    }
                    #endregion
                    if (!string.IsNullOrEmpty(strThuTu)) entity.THU_TU = Convert.ToInt16(strThuTu);
                    if (hdQuocTich > 0) entity.MA_QUOC_TICH = hdQuocTich;
                    if (hdDanToc > 0) entity.MA_DAN_TOC = hdDanToc;
                    if (hdDoiTuong > 0) entity.MA_DOI_TUONG_CS = hdDoiTuong;
                    if (hdKhuVuc > 0) entity.MA_KHU_VUC = hdKhuVuc;
                    if (hdTinh > 0) entity.MA_TINH_THANH = hdTinh;
                    if (hdQuanHuyen > 0) entity.MA_QUAN_HUYEN = hdQuanHuyen;
                    if (hdXaPhuong > 0) entity.MA_XA_PHUONG = hdXaPhuong;
                    if (!string.IsNullOrEmpty(strNoiSinh)) entity.NOI_SINH = strNoiSinh;
                    if (!string.IsNullOrEmpty(strDiaChi)) entity.DIA_CHI = strDiaChi;
                    if (!string.IsNullOrEmpty(strSoCMND)) entity.SO_CMND = strSoCMND;
                    if (!string.IsNullOrEmpty(strNoiCapCMND)) entity.NOI_CAP_CMND = strNoiCapCMND;
                    if (!string.IsNullOrEmpty(strNgayCapCMND)) entity.NGAY_CAP_CMND = Convert.ToDateTime(strNgayCapCMND);
                    if (!string.IsNullOrEmpty(strHoTenBo)) entity.HO_TEN_BO = strHoTenBo;
                    if (!string.IsNullOrEmpty(strNamSinhBo)) entity.NAM_SINH_BO = Convert.ToInt16(strNamSinhBo);
                    if (!string.IsNullOrEmpty(strSDTBo)) entity.SDT_BO = strSDTBo;
                    if (!string.IsNullOrEmpty(strHoTenMe)) entity.HO_TEN_ME = strHoTenMe;
                    if (!string.IsNullOrEmpty(strNamSinhMe)) entity.NAM_SINH_ME = Convert.ToInt16(strNamSinhMe);
                    if (!string.IsNullOrEmpty(strSDTMe)) entity.SDT_ME = strSDTMe;
                    if (!string.IsNullOrEmpty(strHoTenNBH)) entity.HO_TEN_NGUOI_BAO_HO = strHoTenNBH;
                    if (!string.IsNullOrEmpty(strNamSinhNBH)) entity.NAM_SINH_NGUOI_BAO_HO = Convert.ToInt16(strNamSinhNBH);
                    if (!string.IsNullOrEmpty(strSDTNBH)) entity.SDT_NBH = strSDTNBH;
                    #endregion
                    #region Set Entity
                    List<string> lstSDT = new List<string>();
                    if (!string.IsNullOrEmpty(entity.SDT_NHAN_TIN))
                        lstSDT.Add(entity.SDT_NHAN_TIN.Trim());
                    if (!string.IsNullOrEmpty(entity.SDT_NHAN_TIN2))
                        lstSDT.Add(entity.SDT_NHAN_TIN2.Trim());
                    if (!string.IsNullOrEmpty(entity.SDT_BO))
                        lstSDT.Add(entity.SDT_BO.Trim());
                    if (!string.IsNullOrEmpty(entity.SDT_ME))
                        lstSDT.Add(entity.SDT_ME.Trim());
                    if (!string.IsNullOrEmpty(entity.SDT_NBH))
                        lstSDT.Add(entity.SDT_NBH.Trim());
                    HOC_SINH hocSinh = hocSinhBO.checkExistsHocSinh(Convert.ToInt16(Sys_Ma_Nam_hoc), Sys_This_Truong.ID, entity.ID_KHOI, entity.ID_LOP, entity.HO_TEN, lstSDT, entity.NGAY_SINH, entity.MA);
                    bool is_update = hocSinh != null;
                    if (hocSinh == null)
                    {
                        hocSinh = new HOC_SINH();
                        hocSinh.ID_TRUONG = Sys_This_Truong.ID;
                        if (lstColumn.Contains("ID_KHOI"))
                            hocSinh.ID_KHOI = entity.ID_KHOI;
                        if (lstColumn.Contains("ID_LOP"))
                            hocSinh.ID_LOP = entity.ID_LOP;
                        if (lstColumn.Contains("HO_TEN"))
                        {
                            string ho_dem = "", ten = "";
                            hocSinh.HO_TEN = entity.HO_TEN;
                            localAPI.splitHoTen(hocSinh.HO_TEN, out ho_dem, out ten);
                            hocSinh.TEN = ten.Trim();
                            hocSinh.HO_DEM = ho_dem.Trim();
                        }
                    }
                    if (lstColumn.Contains("NGAY_SINH"))
                        hocSinh.NGAY_SINH = entity.NGAY_SINH;
                    if (lstColumn.Contains("SDT_NHAN_TIN"))
                        hocSinh.SDT_NHAN_TIN = entity.SDT_NHAN_TIN;
                    if (lstColumn.Contains("MA_GIOI_TINH"))
                        hocSinh.MA_GIOI_TINH = entity.MA_GIOI_TINH;
                    if (lstColumn.Contains("TRANG_THAI_HOC"))
                        hocSinh.TRANG_THAI_HOC = entity.TRANG_THAI_HOC;
                    if (lstColumn.Contains("IS_CON_GV"))
                        hocSinh.IS_CON_GV = entity.IS_CON_GV;
                    if (lstColumn.Contains("IS_DK_KY1"))
                        hocSinh.IS_DK_KY1 = entity.IS_DK_KY1;
                    if (lstColumn.Contains("IS_DK_KY2"))
                        hocSinh.IS_DK_KY2 = entity.IS_DK_KY2;
                    if (lstColumn.Contains("IS_MIEN_GIAM_KY1"))
                        hocSinh.IS_MIEN_GIAM_KY1 = entity.IS_MIEN_GIAM_KY1;
                    if (lstColumn.Contains("IS_MIEN_GIAM_KY2"))
                        hocSinh.IS_MIEN_GIAM_KY2 = entity.IS_MIEN_GIAM_KY2;
                    if (lstColumn.Contains("THU_TU"))
                        hocSinh.THU_TU = entity.THU_TU;
                    if (lstColumn.Contains("MA_QUOC_TICH"))
                        hocSinh.MA_QUOC_TICH = entity.MA_QUOC_TICH;
                    if (lstColumn.Contains("MA_DAN_TOC"))
                        hocSinh.MA_DAN_TOC = entity.MA_DAN_TOC;
                    if (lstColumn.Contains("MA_DOI_TUONG_CS"))
                        hocSinh.MA_DOI_TUONG_CS = entity.MA_DOI_TUONG_CS;
                    if (lstColumn.Contains("MA_KHU_VUC"))
                        hocSinh.MA_KHU_VUC = entity.MA_KHU_VUC;
                    if (lstColumn.Contains("MA_TINH_THANH"))
                        hocSinh.MA_TINH_THANH = entity.MA_TINH_THANH;
                    if (lstColumn.Contains("MA_QUAN_HUYEN"))
                        hocSinh.MA_QUAN_HUYEN = entity.MA_QUAN_HUYEN;
                    if (lstColumn.Contains("MA_XA_PHUONG"))
                        hocSinh.MA_XA_PHUONG = entity.MA_XA_PHUONG;
                    if (lstColumn.Contains("NOI_SINH"))
                        hocSinh.NOI_SINH = entity.NOI_SINH;
                    if (lstColumn.Contains("DIA_CHI"))
                        hocSinh.DIA_CHI = entity.DIA_CHI;
                    if (lstColumn.Contains("SO_CMND"))
                        hocSinh.SO_CMND = entity.SO_CMND;
                    if (lstColumn.Contains("NOI_CAP_CMND"))
                        hocSinh.NOI_CAP_CMND = entity.NOI_CAP_CMND;
                    if (lstColumn.Contains("NGAY_CAP_CMND"))
                        hocSinh.NGAY_CAP_CMND = entity.NGAY_CAP_CMND;
                    if (lstColumn.Contains("HO_TEN_BO"))
                        hocSinh.HO_TEN_BO = entity.HO_TEN_BO;
                    if (lstColumn.Contains("NAM_SINH_BO"))
                        hocSinh.NAM_SINH_BO = entity.NAM_SINH_BO;
                    if (lstColumn.Contains("SDT_BO"))
                        hocSinh.SDT_BO = entity.SDT_BO;
                    if (lstColumn.Contains("HO_TEN_ME"))
                        hocSinh.HO_TEN_ME = entity.HO_TEN_ME;
                    if (lstColumn.Contains("NAM_SINH_ME"))
                        hocSinh.NAM_SINH_ME = entity.NAM_SINH_ME;
                    if (lstColumn.Contains("SDT_ME"))
                        hocSinh.SDT_ME = entity.SDT_ME;
                    if (lstColumn.Contains("HO_TEN_NGUOI_BAO_HO"))
                        hocSinh.HO_TEN_NGUOI_BAO_HO = entity.HO_TEN_NGUOI_BAO_HO;
                    if (lstColumn.Contains("NAM_SINH_NGUOI_BAO_HO"))
                        hocSinh.NAM_SINH_NGUOI_BAO_HO = entity.NAM_SINH_NGUOI_BAO_HO;
                    if (lstColumn.Contains("SDT_NBH"))
                        hocSinh.SDT_NBH = entity.SDT_NBH;
                    hocSinh.IS_DELETE = false;
                    //if(lstColumn.Contains("STT"))
                    //    hocSinh.THU_TU = entity.THU_TU;
                    hocSinh.ID_NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
                    hocSinh.MA_CAP_HOC = Sys_This_Cap_Hoc;
                    #endregion
                    string strMsg = string.Empty;
                    if (resEntity.Res)
                    {
                        if (!is_update)
                        {
                            resEntity = hocSinhBO.insert(hocSinh, Sys_User.ID);
                            HOC_SINH resHocSinh = (HOC_SINH)resEntity.ResObject;
                            if (resEntity.Res)
                                logUserBO.insert(Sys_This_Truong.ID, "IMPORT", "Thêm mới học sinh " + resHocSinh.ID, Sys_User.ID, DateTime.Now);
                        }
                        else
                        {
                            resEntity = hocSinhBO.update(hocSinh, Sys_User.ID);
                            if (resEntity.Res)
                                logUserBO.insert(Sys_This_Truong.ID, "IMPORT", "Cập nhật học sinh " + hocSinh.ID, Sys_User.ID, DateTime.Now);
                        }
                        if (resEntity.Res)
                            count_success++;

                    }
                    hdresMsg.Value = resEntity.Msg;
                    if (!resEntity.Res)
                    {
                        row.ForeColor = Color.Red;
                        icError.Visible = true;
                    }
                    else
                    {
                        icSuccess.Visible = true;
                    }
                    lstResEntity.Add(resEntity);
                }

                if (res.MA_LOI == 0)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('success', 'Cập nhật thành công " + count_success + " bản ghi!');", true);
                }
                else
                {
                    if (count_success > 0)
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Cập nhật " + count_success + "/" + count_total + " bản ghi. Vui lòng kiểm tra thông tin các bản ghi chưa cập nhật trong bảng Chi tiết nhập liệu');", true);
                    else
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Cập nhật không thành công. Vui lòng kiểm tra thông tin chưa cập nhật trong bảng Chi tiết nhập liệu');", true);
                }
            }
        }
        protected void btTaiExcel_Click(object sender, EventArgs e)
        {
            string fileMau = "Template-HocSinh.xls";

            string serverPath = Server.MapPath("~");
            string path = String.Format("{0}Tmps\\{1}", Server.MapPath("~"), fileMau);
            string newName = fileMau;

            System.IO.FileInfo file = new System.IO.FileInfo(path);
            string Outgoingfile = fileMau;
            if (file.Exists)
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + Outgoingfile);
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.WriteFile(file.FullName);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Lỗi tải file');", true);
                return;
            }
        }

    }
}