using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CMS.GiaoVien
{
    public partial class GiaoVienImportExcel : AuthenticatePage
    {
        private GiaoVienBO giaoVienBO = new GiaoVienBO();
        private LocalAPI localAPI = new LocalAPI();
        private GioiTinhBO gioiTinhBO = new GioiTinhBO();
        private TrangThaiGVBO trangThaiGVBO = new TrangThaiGVBO();
        DMChucVuBO chucvuBO = new DMChucVuBO();
        LogUserBO logUserBO = new LogUserBO();
        List<string> lstValue = new List<string> { "HO_TEN", "SDT", "NGAY_SINH", "MA_GIOI_TINH", "EMAIL", "DIA_CHI", "ID_CHUC_VU", "MA_TRANG_THAI", "THU_TU" };
        List<string> lstText = new List<string> { "Họ tên", "Số điện thoại", "Ngày sinh", "Giới tính", "Email", "Địa chỉ", "Chức vụ", "Trạng thái", "Thứ tự" };
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            bt_EXCELtoSQL.Visible = is_access(SYS_Type_Access.THEM);
            btnUpload.Visible = is_access(SYS_Type_Access.THEM);
            if (!IsPostBack)
            {
                Session["ExcelGiaoVien" + Sys_User.ID] = null;
                Session["ExcelGiaoVienMessageId" + Sys_User.ID] = null;
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
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            if (Session["ExcelGiaoVien" + Sys_User.ID] != null)
            {
                dt = (DataTable)Session["ExcelGiaoVien" + Sys_User.ID];
            }

            if (dt != null && dt.Columns.Count > 0)
                RadGrid1.DataSource = dt;
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
                if (Session["ExcelGiaoVien" + Sys_User.ID] != null)
                {
                    DataTable dt = (DataTable)Session["ExcelGiaoVien" + Sys_User.ID];
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
                        if (dt == null || dt.Columns.Count < lstValue.Count + 1)
                        {
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'File chưa đúng mẫu, hoặc dữ liệu trống. Vui lòng kiểm tra lại!');", true);
                        }
                        else if (dt.Columns.Count < localAPI.ExcelColumnNameToNumber("I"))
                        {
                            for (int i = dt.Columns.Count - 1; i > localAPI.ExcelColumnNameToNumber("I"); i--)
                                dt.Columns.RemoveAt(i);
                        }
                        Session["ExcelGiaoVien" + Sys_User.ID] = dt;
                        Session["ExcelGiaoVienMessageId" + Sys_User.ID] = message_id;

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
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Có lỗi xảy ra!');", true);
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
                if (myDataTable.Columns.Count < lstValue.Count + 1)
                {
                    return null;
                }
                for (int i = 0; i <= lstValue.Count; i++)
                {
                    try
                    {
                        if (myDataTable.Columns[i + 1].ColumnName.ToNormalizeLowerRelace() == lstText[i].ToNormalizeLowerRelace())
                            myDataTable.Columns[i + 1].ColumnName = lstValue[i];
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
                            if (string.IsNullOrEmpty(row[1].ToString().Trim()) || string.IsNullOrEmpty(row[2].ToString().Trim()))
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
                #endregion
                return myDataTable;
            }
            catch (OleDbException ex)
            {
                // Response.Write(ex.ToString());
                if (ex.ErrorCode == -2147467259)
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn phải đặt tên sheet dữ liệu là Sheet1 hoặc định dạng dữ liệu không đúng!');", true);
                else ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Có lỗi xảy ra!');", true);

            }
            catch (Exception ex)
            {
                // Response.Write(ex.ToString());
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Có lỗi xảy ra!');", true);
            }
            return new DataTable();
        }
        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                RadDatePicker rdNgaySinh = (RadDatePicker)e.Item.FindControl("rdNgaySinh");
                HiddenField hdNgaySinh = (HiddenField)e.Item.FindControl("hdNgaySinh");
                System.Web.UI.HtmlControls.HtmlImage error1 = (System.Web.UI.HtmlControls.HtmlImage)item.FindControl("iconErr");
                System.Web.UI.HtmlControls.HtmlImage success1 = (System.Web.UI.HtmlControls.HtmlImage)item.FindControl("iconSuccess");
                error1.Visible = false;
                success1.Visible = false;
                if (!string.IsNullOrEmpty(hdNgaySinh.Value))
                {
                    rdNgaySinh.SelectedDate = Convert.ToDateTime(hdNgaySinh.Value);
                }
            }
        }
        protected void bt_EXCELtoSQL_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.THEM))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            if (Session["ExcelGiaoVien" + Sys_User.ID] != null && Session["ExcelGiaoVienMessageId" + Sys_User.ID] != null)
            {
                List<ResultError> lstRes = new List<ResultError>();
                List<ResultEntity> lstResEntity = new List<ResultEntity>();
                ResultError res = new ResultError();
                CommonValidate validate = new CommonValidate();

                DataTable dt = new DataTable();
                dt = (DataTable)Session["ExcelGiaoVien" + Sys_User.ID];
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
                List<DM_CHUC_VU> lstChucVu = chucvuBO.getChucVu();
                List<TRANG_THAI_GV> lstTrangThai = trangThaiGVBO.getTrangThaiGV();

                foreach (GridDataItem row in RadGrid1.Items)
                {
                    ResultEntity resEntity = new ResultEntity();
                    resEntity.Res = true;
                    count_row++;
                    count_total++;
                    object val = new object();
                    string out_sdt = string.Empty;
                    string error = string.Empty;
                    GIAO_VIEN entity = new GIAO_VIEN();

                    TextBox tbStt = (TextBox)(TextBox)row.FindControl("tbSTT");
                    TextBox tbHoTen = (TextBox)row.FindControl("tbHoTen");
                    TextBox tbSDT = (TextBox)row.FindControl("tbSDT");
                    TextBox tbNgaySinh = (TextBox)row.FindControl("tbNgaySinh");
                    TextBox tbGioiTinh = (TextBox)row.FindControl("tbGioiTinh");
                    TextBox tbEmail = (TextBox)row.FindControl("tbEmail");
                    TextBox tbDiaChi = (TextBox)row.FindControl("tbDiaChi");
                    TextBox tbTrangThai = (TextBox)row.FindControl("tbTrangThai");
                    TextBox tbChucVu = (TextBox)row.FindControl("tbChucVu");
                    TextBox tbThuTu = (TextBox)row.FindControl("tbThuTu");
                    HiddenField hdresMsg = (HiddenField)row.FindControl("hdresMsg");
                    System.Web.UI.HtmlControls.HtmlImage icError = (System.Web.UI.HtmlControls.HtmlImage)row.FindControl("iconErr");
                    System.Web.UI.HtmlControls.HtmlImage icSuccess = (System.Web.UI.HtmlControls.HtmlImage)row.FindControl("iconSuccess");
                    icError.Visible = false;
                    icSuccess.Visible = false;

                    #region set value
                    long id = Convert.ToInt64(tbStt.Text.Trim());
                    string hoTen = (tbHoTen != null) ? tbHoTen.Text.Trim() : "";
                    string sdt = (tbSDT != null) ? tbSDT.Text.Trim() : "";
                    string mail = (tbEmail != null) ? tbEmail.Text.Trim() : "";
                    string ngaySinh = (tbNgaySinh != null) ? tbNgaySinh.Text.Trim() : "";
                    string diachi = (tbDiaChi != null) ? tbDiaChi.Text.Trim() : "";
                    string thutu = (tbThuTu != null) ? tbThuTu.Text.Trim() : "";
                    #region Giới tính
                    short hdGioiTinh = -1;
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
                    #region Chức vụ
                    short hdChucVu = 0;
                    string strChucVu = tbChucVu.Text.ToNormalizeLowerRelace();
                    if (lstChucVu != null)
                    {
                        DM_CHUC_VU chucVu = lstChucVu.FirstOrDefault(x => x.TEN.ToNormalizeLowerRelace() == strChucVu);
                        if (chucVu != null) hdChucVu = chucVu.ID;
                    }
                    #endregion
                    #region Trạng thái
                    short hdTrangThai = 0;
                    string strTrangThai = tbTrangThai.Text.ToNormalizeLowerRelace();
                    if (lstTrangThai != null)
                    {
                        TRANG_THAI_GV trangThai = lstTrangThai.FirstOrDefault(x => x.TEN.ToNormalizeLowerRelace() == strTrangThai);
                        if (trangThai != null) hdTrangThai = trangThai.MA;
                    }
                    #endregion
                    #endregion

                    #region check value
                    #region check STT
                    if (validate.ValInt(true, null, null, id.ToString(), out val, out error))
                    {
                        entity.ID = id;
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
                    #region check họ tên
                    if (validate.ValString(false, null, hoTen, out val, out error))
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
                    #region check SĐT
                    if (validate.ValidateSDT(true, sdt, out out_sdt, out error))
                    {
                        entity.SDT = out_sdt;
                    }
                    else
                    {
                        res.MA_LOI = count_row;
                        res.TEN_LOI = error;
                        res.COT = localAPI.ExcelColumnNameToNumber("C") - 1;
                        res.DONG = count_row;
                        res.TEN_COT = "Số điện thoại";
                        lstRes.Add(res);
                        resEntity.Res = false;
                        if (string.IsNullOrEmpty(resEntity.Msg))
                            resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                        else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                    }
                    #endregion
                    if (!string.IsNullOrEmpty(ngaySinh)) entity.NGAY_SINH = Convert.ToDateTime(ngaySinh);
                    #region check Giới tính
                    if (validate.ValString(true, null, strGioiTinh, out val, out error))
                    {
                        entity.MA_GIOI_TINH = hdGioiTinh;
                    }
                    else
                    {
                        res.MA_LOI = count_row;
                        res.TEN_LOI = error;
                        res.COT = localAPI.ExcelColumnNameToNumber("E") - 1;
                        res.DONG = count_row;
                        res.TEN_COT = "Giới tính";
                        lstRes.Add(res);
                        resEntity.Res = false;
                        if (string.IsNullOrEmpty(resEntity.Msg))
                            resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                        else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                    }
                    #endregion
                    #region check Email
                    if (validate.ValidateEmail(false, mail, out val, out error))
                    {
                        entity.EMAIL = mail;
                    }
                    else
                    {
                        res.MA_LOI = count_row;
                        res.TEN_LOI = error;
                        res.COT = localAPI.ExcelColumnNameToNumber("F") - 1;
                        res.DONG = count_row;
                        res.TEN_COT = "Email";
                        lstRes.Add(res);
                        resEntity.Res = false;
                        if (string.IsNullOrEmpty(resEntity.Msg))
                            resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                        else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                    }
                    #endregion
                    #region check Địa chỉ
                    if (validate.ValString(false, null, diachi, out val, out error))
                    {
                        entity.DIA_CHI = diachi;
                    }
                    else
                    {
                        res.MA_LOI = count_row;
                        res.TEN_LOI = error;
                        res.COT = localAPI.ExcelColumnNameToNumber("G") - 1;
                        res.DONG = count_row;
                        res.TEN_COT = "Địa chỉ";
                        lstRes.Add(res);
                        resEntity.Res = false;
                        if (string.IsNullOrEmpty(resEntity.Msg))
                            resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                        else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                    }
                    #endregion
                    #region Check Chức vụ
                    if (validate.ValString(true, null, strChucVu, out val, out error))
                    {
                        entity.ID_CHUC_VU = hdChucVu;
                    }
                    else
                    {
                        res.MA_LOI = count_row;
                        res.TEN_LOI = error;
                        res.COT = localAPI.ExcelColumnNameToNumber("H") - 1;
                        res.DONG = count_row;
                        res.TEN_COT = "Chức vụ";
                        lstRes.Add(res);
                        resEntity.Res = false;
                        if (string.IsNullOrEmpty(resEntity.Msg))
                            resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                        else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                    }
                    #endregion
                    #region Check trạng thái
                    if (validate.ValString(true, null, strTrangThai, out val, out error))
                    {
                        entity.MA_TRANG_THAI = hdTrangThai;
                    }
                    else
                    {
                        res.MA_LOI = count_row;
                        res.TEN_LOI = error;
                        res.COT = localAPI.ExcelColumnNameToNumber("I") - 1;
                        res.DONG = count_row;
                        res.TEN_COT = "Trạng thái";
                        lstRes.Add(res);
                        resEntity.Res = false;
                        if (string.IsNullOrEmpty(resEntity.Msg))
                            resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                        else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                    }
                    #endregion
                    #region Check Thứ tự
                    if (validate.ValString(false, null, thutu, out val, out error))
                    {
                        entity.THU_TU = localAPI.ConvertStringToShort(thutu);
                    }
                    else
                    {
                        res.MA_LOI = count_row;
                        res.TEN_LOI = error;
                        res.COT = localAPI.ExcelColumnNameToNumber("J") - 1;
                        res.DONG = count_row;
                        res.TEN_COT = "Thứ tự";
                        lstRes.Add(res);
                        resEntity.Res = false;
                        if (string.IsNullOrEmpty(resEntity.Msg))
                            resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                        else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                    }
                    #endregion
                    #endregion
                    #region insert data to database
                    // check so dien thoai chua ton tai trong he thong thi insert 
                    GIAO_VIEN giaoVien = giaoVienBO.checkGiaoVienByPhoneAndTruong(Sys_This_Truong.ID, entity.SDT);
                    bool is_update = giaoVien != null;
                    if (giaoVien == null) giaoVien = new GIAO_VIEN();
                    giaoVien.SDT = entity.SDT;
                    giaoVien.ID_TRUONG = Sys_This_Truong.ID;
                    giaoVien.MA_TRANG_THAI = entity.MA_TRANG_THAI;
                    giaoVien.ID_CHUC_VU = entity.ID_CHUC_VU;
                    if (lstColumn.Contains("HO_TEN"))
                    {
                        string ho_dem = "", ten = "";
                        giaoVien.HO_TEN = entity.HO_TEN;
                        localAPI.splitHoTen(giaoVien.HO_TEN, out ho_dem, out ten);
                        giaoVien.TEN = ten.Trim();
                        giaoVien.HO_DEM = ho_dem.Trim();
                    }
                    if (lstColumn.Contains("NGAY_SINH"))
                        giaoVien.NGAY_SINH = entity.NGAY_SINH;
                    if (lstColumn.Contains("MA_GIOI_TINH"))
                        giaoVien.MA_GIOI_TINH = entity.MA_GIOI_TINH;
                    if (lstColumn.Contains("EMAIL"))
                        giaoVien.EMAIL = entity.EMAIL;
                    if (lstColumn.Contains("DIA_CHI"))
                        giaoVien.DIA_CHI = entity.DIA_CHI;
                    if (lstColumn.Contains("THU_TU"))
                        giaoVien.THU_TU = entity.THU_TU;

                    string strMsg = string.Empty;
                    if (resEntity.Res)
                    {
                        if (!is_update)
                        {
                            resEntity = giaoVienBO.insert(giaoVien, Sys_User.ID);
                            GIAO_VIEN resGiaoVien = (GIAO_VIEN)resEntity.ResObject;
                            if (resEntity.Res)
                                logUserBO.insert(Sys_This_Truong.ID, "IMPORT", "Thêm mới gv " + resGiaoVien.ID, Sys_User.ID, DateTime.Now);
                        }
                        else
                        {
                            resEntity = giaoVienBO.update(giaoVien.ID, giaoVien.TEN, giaoVien.SDT, giaoVien.NGAY_SINH, giaoVien.MA_GIOI_TINH, giaoVien.ID_CHUC_VU, giaoVien.MA_TRANG_THAI, giaoVien.DIA_CHI, giaoVien.EMAIL, giaoVien.ID_TRUONG, Sys_User.ID, giaoVien.THU_TU, giaoVien.HO_DEM, giaoVien.HO_TEN);
                            if (resEntity.Res)
                                logUserBO.insert(Sys_This_Truong.ID, "IMPORT", "Cập nhật gv " + giaoVien.ID, Sys_User.ID, DateTime.Now);
                        }
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
                    #endregion
                }

                if (res.MA_LOI == 0)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('success', 'Cập nhật thành công!');", true);
                }
                else
                {
                    if (count_success > 0)
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Cập nhật " + count_success + "/" + count_total + " bản ghi. Vui lòng kiểm tra thông tin các bản ghi chưa cập nhật trong bảng Chi tiết nhập liệu');", true);
                    else
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Cập nhật không thành công. Vui lòng kiểm tra thông tin chưa cập nhật trong bảng Chi tiết nhập liệu');", true);
                }
                Session["ExcelGiaoVienDes" + Sys_User.ID] = res;
            }
        }
        protected void btTaiExcel_Click(object sender, EventArgs e)
        {
            string fileMau = "Template-GiaoVien.xls";

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