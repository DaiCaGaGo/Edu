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

namespace CMS.QuanTri
{
    public partial class NguoiDungImportExcel : AuthenticatePage
    {
        private NguoiDungBO nguoiDungBO = new NguoiDungBO();
        private NguoiDungTruongBO nguoiDungTruongBO = new NguoiDungTruongBO();
        private LocalAPI localAPI = new LocalAPI();
        List<string> lstValue = new List<string> { "TEN_DANG_NHAP", "TEN_HIEN_THI", "EMAIL", "SDT", "DIA_CHI", "GHI_CHU", "QUYEN", "FACE_BOOK" };
        List<string> lstText = new List<string> { "Tên đăng nhập", "Tên hiển thị", "Email", "Số điện thoại", "Địa chỉ", "Ghi chú", "Quyền", "Facebook" };
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            if (!IsPostBack)
            {
                // combobox vai tro
                objQuyen.SelectParameters.Add("ma", "");
                objQuyen.SelectParameters.Add("ten", "");

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
                Session["ExcelNguoiDung" + Sys_User.ID] = null;
                Session["ExcelNguoiDungMessageId" + Sys_User.ID] = null;
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            if (Session["ExcelNguoiDung" + Sys_User.ID] != null)
            {
                dt = (DataTable)Session["ExcelNguoiDung" + Sys_User.ID];
            }

            if (dt != null && dt.Columns.Count > 0)
                RadGrid1.DataSource = dt;
        }
        protected void bt_importSQL_Click(object sender, EventArgs e)
        {
            try
            {
                UploadFile();
                if (Session["ExcelNguoiDung" + Sys_User.ID] != null)
                {
                    DataTable dt = (DataTable)Session["ExcelNguoiDung" + Sys_User.ID];
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
                        else if (dt.Columns.Count < localAPI.ExcelColumnNameToNumber("H"))
                        {
                            for (int i = dt.Columns.Count - 1; i > localAPI.ExcelColumnNameToNumber("H"); i--)
                                dt.Columns.RemoveAt(i);
                        }
                        Session["ExcelNguoiDung" + Sys_User.ID] = dt;
                        Session["ExcelNguoiDungMessageId" + Sys_User.ID] = message_id;

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
                            if (string.IsNullOrEmpty(row[1].ToString().Trim()) ||
                                string.IsNullOrEmpty(row[4].ToString().Trim()) ||
                                string.IsNullOrEmpty(row[7].ToString().Trim()))
                            {
                                kt = false;
                            }

                            // xoa dong du lieu
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
                //Response.Write(ex.ToString());
                if (ex.ErrorCode == -2147467259)
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn phải đặt tên sheet dữ liệu là Sheet1 hoặc định dạng dữ liệu không đúng!');", true);
                else ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Có lỗi xảy ra!');", true);

            }
            catch (Exception ex)
            {
                //  Response.Write(ex.ToString());
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Có lỗi xảy ra!');", true);
            }
            return new DataTable();
        }
        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                RadComboBox rcbQuyenItem = (RadComboBox)e.Item.FindControl("rcbQuyen");
                HiddenField hdQuyen = (HiddenField)e.Item.FindControl("hdQuyen");
                System.Web.UI.HtmlControls.HtmlImage error1 = (System.Web.UI.HtmlControls.HtmlImage)item.FindControl("iconErr");
                System.Web.UI.HtmlControls.HtmlImage success1 = (System.Web.UI.HtmlControls.HtmlImage)item.FindControl("iconSuccess");
                error1.Visible = false;
                success1.Visible = false;

                rcbQuyenItem.DataBind();
                if (!string.IsNullOrEmpty(hdQuyen.Value) && rcbQuyenItem.Items.FindItemByValue(hdQuyen.Value) != null)
                {
                    rcbQuyenItem.SelectedValue = hdQuyen.Value;
                }
            }
        }
        protected void bt_EXCELtoSQL_Click(object sender, EventArgs e)
        {
            if (Session["ExcelNguoiDung" + Sys_User.ID] != null && Session["ExcelNguoiDungMessageId" + Sys_User.ID] != null)
            {
                List<ResultError> lstRes = new List<ResultError>();
                List<ResultEntity> lstResEntity = new List<ResultEntity>();
                ResultError res = new ResultError();
                CommonValidate validate = new CommonValidate();

                DataTable dt = new DataTable();
                dt = (DataTable)Session["ExcelNguoiDung" + Sys_User.ID];
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

                foreach (GridDataItem row in RadGrid1.Items)
                {
                    ResultEntity resEntity = new ResultEntity();
                    resEntity.Res = true;
                    count_row++;
                    count_total++;
                    object val = new object();
                    string error = string.Empty;
                    NGUOI_DUNGEntity entity = new NGUOI_DUNGEntity();

                    TextBox tbStt = (TextBox)(TextBox)row.FindControl("tbSTT");
                    TextBox tbDisplayName = (TextBox)row.FindControl("tbTenHienThi");
                    TextBox tbEmail = (TextBox)row.FindControl("tbEmail");
                    TextBox tbPhone = (TextBox)row.FindControl("tbSDT");
                    TextBox tbAddress = (TextBox)row.FindControl("tbDiaChi");
                    TextBox tbNote = (TextBox)row.FindControl("tbGhiChu");
                    RadComboBox rcbRoles = (RadComboBox)row.FindControl("rcbQuyen");
                    TextBox tbFb = (TextBox)row.FindControl("tbFacebook");
                    HiddenField hdresMsg = (HiddenField)row.FindControl("hdresMsg");
                    System.Web.UI.HtmlControls.HtmlImage icError = (System.Web.UI.HtmlControls.HtmlImage)row.FindControl("iconErr");
                    System.Web.UI.HtmlControls.HtmlImage icSuccess = (System.Web.UI.HtmlControls.HtmlImage)row.FindControl("iconSuccess");
                    icError.Visible = false;
                    icSuccess.Visible = false;

                    long id = Convert.ToInt64(tbStt.Text.Trim());
                    string displayName = tbDisplayName.Text.Trim();
                    string mail = tbEmail.Text.Trim();
                    string phone = tbPhone.Text.Trim();
                    string address = tbAddress.Text.Trim();
                    string note = tbNote.Text.Trim();
                    string roles = rcbRoles.SelectedValue;
                    string fb = tbFb.Text.Trim();

                    // check stt
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

                    // check ten hien thi
                    if (validate.ValString(false, null, displayName, out val, out error))
                    {
                        entity.TEN_HIEN_THI = displayName;
                    }
                    else
                    {
                        res.MA_LOI = count_row;
                        res.TEN_LOI = error;
                        res.COT = localAPI.ExcelColumnNameToNumber("D") - 1;
                        res.DONG = count_row;
                        res.TEN_COT = "Tên hiển thị";
                        lstRes.Add(res);
                        resEntity.Res = false;
                        if (string.IsNullOrEmpty(resEntity.Msg))
                            resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                        else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                    }

                    // check email
                    if (validate.ValidateEmail(false, mail, out val, out error))
                    {
                        entity.EMAIL = mail;
                    }
                    else
                    {
                        res.MA_LOI = count_row;
                        res.TEN_LOI = error;
                        res.COT = localAPI.ExcelColumnNameToNumber("E") - 1;
                        res.DONG = count_row;
                        res.TEN_COT = "Email";
                        lstRes.Add(res);
                        resEntity.Res = false;
                        if (string.IsNullOrEmpty(resEntity.Msg))
                            resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                        else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                    }

                    // check so dien thoai
                    if (validate.ValInt(true, 10, 11, phone, out val, out error))
                    {
                        entity.SDT = phone;
                    }
                    else
                    {
                        res.MA_LOI = count_row;
                        res.TEN_LOI = error;
                        res.COT = localAPI.ExcelColumnNameToNumber("F") - 1;
                        res.DONG = count_row;
                        res.TEN_COT = "Số điện thoại";
                        lstRes.Add(res);
                        resEntity.Res = false;
                        if (string.IsNullOrEmpty(resEntity.Msg))
                            resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                        else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                    }

                    // check dia chi
                    if (validate.ValString(false, null, address, out val, out error))
                    {
                        entity.DIA_CHI = address;
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

                    // check ghi chu
                    if (validate.ValString(false, null, note, out val, out error))
                    {
                        entity.GHI_CHU = note;
                    }
                    else
                    {
                        res.MA_LOI = count_row;
                        res.TEN_LOI = error;
                        res.COT = localAPI.ExcelColumnNameToNumber("G") - 1;
                        res.DONG = count_row;
                        res.TEN_COT = "Ghi chú";
                        lstRes.Add(res);
                        resEntity.Res = false;
                        if (string.IsNullOrEmpty(resEntity.Msg))
                            resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                        else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                    }

                    // check quyen
                    if (validate.ValString(true, null, roles, out val, out error))
                    {
                        entity.QUYEN = roles;
                    }
                    else
                    {
                        res.MA_LOI = count_row;
                        res.TEN_LOI = error;
                        res.COT = localAPI.ExcelColumnNameToNumber("G") - 1;
                        res.DONG = count_row;
                        res.TEN_COT = "Quyền";
                        lstRes.Add(res);
                        resEntity.Res = false;
                        if (string.IsNullOrEmpty(resEntity.Msg))
                            resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                        else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                    }

                    // check facebook
                    if (validate.ValString(false, null, fb, out val, out error))
                    {
                        entity.FACE_BOOK = fb;
                    }
                    else
                    {
                        res.MA_LOI = count_row;
                        res.TEN_LOI = error;
                        res.COT = localAPI.ExcelColumnNameToNumber("G") - 1;
                        res.DONG = count_row;
                        res.TEN_COT = "Facebook";
                        lstRes.Add(res);
                        resEntity.Res = false;
                        if (string.IsNullOrEmpty(resEntity.Msg))
                            resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                        else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                    }

                    // insert data to database
                    // check so dien thoai chua ton tai trong he thong thi insert 
                    NGUOI_DUNG nguoiDung = nguoiDungBO.checkNguoiDungByPhone(phone);
                    bool is_update = nguoiDung != null;
                    if (nguoiDung == null)
                    {
                        nguoiDung = new NGUOI_DUNG();
                        nguoiDung.TEN_DANG_NHAP = entity.SDT;
                        nguoiDung.SDT = entity.SDT;
                        nguoiDung.MAT_KHAU = GeneratePassword(15);
                        if (lstColumn.Contains("TEN_HIEN_THI"))
                            nguoiDung.TEN_HIEN_THI = entity.TEN_HIEN_THI;
                        if (lstColumn.Contains("EMAIL"))
                            nguoiDung.EMAIL = entity.EMAIL;
                        if (lstColumn.Contains("DIA_CHI"))
                            nguoiDung.DIA_CHI = entity.DIA_CHI;
                        if (lstColumn.Contains("GHI_CHU"))
                            nguoiDung.GHI_CHU = entity.GHI_CHU;
                        if (lstColumn.Contains("MA_NHOM_QUYEN"))
                            nguoiDung.MA_NHOM_QUYEN = entity.QUYEN;
                        if (lstColumn.Contains("FACE_BOOK"))
                            nguoiDung.FACE_BOOK = entity.FACE_BOOK;
                    }
                    nguoiDung.TRANG_THAI = true;

                    string strMsg = string.Empty;
                    if (resEntity.Res)
                    {
                        if (!is_update)
                        {
                            resEntity = nguoiDungBO.insert(nguoiDung, Sys_User.ID);
                            nguoiDung = (NGUOI_DUNG)resEntity.ResObject;
                        }

                        #region assign truong
                        NGUOI_DUNG_TRUONG ndTruong = new NGUOI_DUNG_TRUONG();
                        if (is_update)
                            ndTruong = nguoiDungTruongBO.getNguoiDungTruongByIDNguoiDungAndTruong(Sys_This_Truong.ID, nguoiDung.ID);
                        if (ndTruong == null || ndTruong.ID == 0)
                        {
                            ndTruong = new NGUOI_DUNG_TRUONG();
                            ndTruong.ID_NGUOI_DUNG = nguoiDung.ID;
                            ndTruong.ID_TRUONG = Sys_This_Truong.ID;
                            ndTruong.TRANG_THAI = 1;

                            resEntity = nguoiDungTruongBO.insertOrUpdate(ndTruong, Sys_User.ID, entity.QUYEN);
                            count_success++;
                        }
                        else if (ndTruong != null && (ndTruong.TRANG_THAI == null || ndTruong.TRANG_THAI == 0))
                        {
                            ndTruong.TRANG_THAI = 1;
                            resEntity = nguoiDungTruongBO.insertOrUpdate(ndTruong, Sys_User.ID, entity.QUYEN);
                            count_success++;
                        }
                        else
                        {
                            resEntity.Res = false;
                            resEntity.Msg = "Tài khoản này đã tồn tại!";
                        }
                        #endregion
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

                if (res.MA_LOI == 0 && count_success == lstResEntity.Count)
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('success', 'Cập nhật thành công!');", true);
                else if (count_success > 0)
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warn', 'Cập nhật " + count_success + "/" + count_total + " bản ghi. Vui lòng kiểm tra thông tin các bản ghi chưa cập nhật trong bảng Chi tiết nhập liệu');", true);
                else
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Cập nhật không thành công. Vui lòng kiểm tra thông tin chưa cập nhật trong bảng Chi tiết nhập liệu');", true);
                //btChiTiet.Visible = true;
                Session["ExcelNguoiDungDes" + Sys_User.ID] = res;
            }
        }
        protected void btTaiExcel_Click(object sender, EventArgs e)
        {
            string fileMau = "Template-NguoiDung.xls";

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
        public static string GeneratePassword(int passLength)
        {
            var chars = "abcdefghijklmnopqrstuvwxyz@#$&ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, passLength)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }
    }
}