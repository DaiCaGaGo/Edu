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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CMS.Customer
{
    public partial class CustomerImportExcel : AuthenticatePage
    {
        private LocalAPI localAPI = new LocalAPI();
        private GioiTinhBO gioiTinhBO = new GioiTinhBO();
        CustomerBO customerBO = new CustomerBO();
        LogUserBO logUserBO = new LogUserBO();
        List<string> lstValue = new List<string> { "HO_TEN", "SDT", "ID_NHOM", "NGAY_SINH", "GIOI_TINH", "EMAIL" };
        List<string> lstText = new List<string> { "Họ tên", "Số điện thoại", "Thuộc nhóm", "Ngày sinh", "Giới tính", "Email" };
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            btnCapNhat.Visible = is_access(SYS_Type_Access.THEM);
            btnUpload.Visible = is_access(SYS_Type_Access.THEM);
            if (!IsPostBack)
            {
                Session["ExcelCustomer" + Sys_User.ID] = null;
                Session["ExcelCustomerMessageId" + Sys_User.ID] = null;
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
            if (Session["ExcelCustomer" + Sys_User.ID] != null)
            {
                dt = (DataTable)Session["ExcelCustomer" + Sys_User.ID];
            }

            if (dt != null && dt.Columns.Count > 0)
                RadGrid1.DataSource = dt;
        }
        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                RadComboBox rcbNhom = (RadComboBox)e.Item.FindControl("rcbNhom");
                HiddenField hdNhom = (HiddenField)e.Item.FindControl("hdNhom");
                RadComboBox rcbGioiTinh = (RadComboBox)e.Item.FindControl("rcbGioiTinh");
                HiddenField hdGioiTinh = (HiddenField)e.Item.FindControl("hdGioiTinh");
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
                #region Gioi tinh
                string strGioiTinh = hdGioiTinh.Value.ToNormalizeLowerRelace();
                if (!string.IsNullOrEmpty(strGioiTinh))
                {
                    List<GIOI_TINH> lstGioiTinh = gioiTinhBO.getGioiTinh();
                    GIOI_TINH gt = lstGioiTinh.FirstOrDefault(x => x.TEN.ToNormalizeLowerRelace() == strGioiTinh);
                    if (gt != null)
                    {
                        rcbGioiTinh.SelectedValue = gt.MA.ToString();
                    }
                }
                #endregion
                #region Nhóm
                string strNhom = hdNhom.Value.ToLower();
                if (!string.IsNullOrEmpty(strNhom))
                {
                    if (strNhom == "học sinh") rcbNhom.SelectedValue = "1";
                    else if (strNhom == "giáo viên") rcbNhom.SelectedValue = "2";
                    else if (strNhom == "khác") rcbNhom.SelectedValue = "3";
                }
                #endregion
            }
        }
        protected void btTaiExcel_Click(object sender, EventArgs e)
        {
            string fileMau = "Template-Customer.xls";

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
        protected void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.THEM))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            if (Session["ExcelCustomer" + Sys_User.ID] != null && Session["ExcelCustomerMessageId" + Sys_User.ID] != null)
            {
                List<ResultError> lstRes = new List<ResultError>();
                List<ResultEntity> lstResEntity = new List<ResultEntity>();
                ResultError res = new ResultError();
                CommonValidate validate = new CommonValidate();

                DataTable dt = new DataTable();
                dt = (DataTable)Session["ExcelCustomer" + Sys_User.ID];
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
                    string out_sdt = string.Empty;
                    string error = string.Empty;
                    CUSTOMER entity = new CUSTOMER();

                    TextBox tbStt = (TextBox)(TextBox)row.FindControl("tbSTT");
                    TextBox tbHoTen = (TextBox)row.FindControl("tbHoTen");
                    TextBox tbSDT = (TextBox)row.FindControl("tbSDT");
                    RadDatePicker rdNgaySinh = (RadDatePicker)row.FindControl("rdNgaySinh");
                    RadComboBox rcbGioiTinh = (RadComboBox)row.FindControl("rcbGioiTinh");
                    RadComboBox rcbNhom = (RadComboBox)row.FindControl("rcbNhom");
                    TextBox tbEmail = (TextBox)row.FindControl("tbEmail");
                    HiddenField hdresMsg = (HiddenField)row.FindControl("hdresMsg");
                    System.Web.UI.HtmlControls.HtmlImage icError = (System.Web.UI.HtmlControls.HtmlImage)row.FindControl("iconErr");
                    System.Web.UI.HtmlControls.HtmlImage icSuccess = (System.Web.UI.HtmlControls.HtmlImage)row.FindControl("iconSuccess");
                    icError.Visible = false;
                    icSuccess.Visible = false;

                    long? id = localAPI.ConvertStringTolong(tbStt.Text.Trim());
                    string hoTen = (tbHoTen != null) ? tbHoTen.Text.Trim() : "";
                    string sdt = (tbSDT != null) ? tbSDT.Text.Trim() : "";
                    string mail = (tbEmail != null) ? tbEmail.Text.Trim() : "";
                    short? gioi_tinh = localAPI.ConvertStringToShort(rcbGioiTinh.SelectedValue);


                    #region check STT
                    if (validate.ValInt(true, null, null, id.ToString(), out val, out error) && id != null)
                    {
                        entity.ID = id.Value;
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
                    #region ngày sinh, giới tính, nhóm
                    if (rdNgaySinh.SelectedDate != null)
                    {
                        try
                        {
                            entity.NGAY_SINH = Convert.ToDateTime(rdNgaySinh.SelectedDate);
                        }
                        catch { entity.NGAY_SINH = null; }
                    }
                    else
                    {
                        entity.NGAY_SINH = null;
                    }
                    if (gioi_tinh != null) entity.GIOI_TINH = gioi_tinh.Value;
                    entity.ID_NHOM = localAPI.ConvertStringToShort(rcbNhom.SelectedValue);
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
                    #region insert data to database
                    // check so dien thoai chua ton tai trong he thong thi insert 
                    CUSTOMER customer = customerBO.checkCustomerByPhone(Sys_This_Truong.ID, entity.SDT);
                    bool is_update = customer != null;
                    if (customer == null) customer = new CUSTOMER();
                    customer.SDT = entity.SDT;
                    customer.ID_TRUONG = Sys_This_Truong.ID;
                    if (lstColumn.Contains("HO_TEN"))
                    {
                        string ho_dem = "", ten = "";
                        customer.HO_TEN = entity.HO_TEN;
                        localAPI.splitHoTen(customer.HO_TEN, out ho_dem, out ten);
                        customer.TEN = ten.Trim();
                        customer.HO_DEM = ho_dem.Trim();
                    }
                    if (lstColumn.Contains("ID_NHOM"))
                        customer.ID_NHOM = entity.ID_NHOM;
                    if (lstColumn.Contains("NGAY_SINH"))
                        customer.NGAY_SINH = entity.NGAY_SINH;
                    if (lstColumn.Contains("GIOI_TINH"))
                        customer.GIOI_TINH = entity.GIOI_TINH != null ? entity.GIOI_TINH : null;
                    if (lstColumn.Contains("EMAIL"))
                        customer.EMAIL = entity.EMAIL;

                    string strMsg = string.Empty;
                    if (resEntity.Res)
                    {
                        if (!is_update)
                        {
                            resEntity = customerBO.insert(customer, Sys_User.ID);
                            if (resEntity.Res)
                            {
                                CUSTOMER resCUS = (CUSTOMER)resEntity.ResObject;
                                logUserBO.insert(Sys_This_Truong.ID, "INSERT", "Thêm mới customer " +resCUS.ID, Sys_User.ID, DateTime.Now);
                            }
                        }
                        else
                        {
                            resEntity = customerBO.update(customer, Sys_User.ID);
                            if (resEntity.Res)
                                logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật customer " + customer.ID, Sys_User.ID, DateTime.Now);
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
                Session["ExcelCustomerDes" + Sys_User.ID] = res;
            }
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.THEM))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            try
            {
                UploadFile();
                if (Session["ExcelCustomer" + Sys_User.ID] != null)
                {
                    DataTable dt = (DataTable)Session["ExcelCustomer" + Sys_User.ID];
                    if (dt.Rows.Count > 0)
                    {
                        RadGrid1.Rebind();
                        btnCapNhat.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
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
                        Session["ExcelCustomer" + Sys_User.ID] = dt;
                        Session["ExcelCustomerMessageId" + Sys_User.ID] = message_id;

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
                            //xóa dòng nếu không có dữ liệu
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
                if (ex.ErrorCode == -2147467259)
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn phải đặt tên sheet dữ liệu là Sheet1 hoặc định dạng dữ liệu không đúng!');", true);
                else ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Có lỗi xảy ra!');", true);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Có lỗi xảy ra!');", true);
            }
            return new DataTable();
        }
    }
}