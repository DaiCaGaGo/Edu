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

namespace CMS.DanhMucMaNhanXet
{
    public partial class MaNXHN_ImportExcel : AuthenticatePage
    {
        MaNhanXetBO nxhnBO = new MaNhanXetBO();
        private LocalAPI localAPI = new LocalAPI();
        LogUserBO logUserBO = new LogUserBO();

        List<string> lstValue = new List<string> { "MA", "NOI_DUNG", "THU_TU" };
        List<string> lstText = new List<string> { "Ký hiệu", "Nội dung", "Thứ tự" };
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            btnCapNhat.Visible = is_access(SYS_Type_Access.THEM);
            btnUpload.Visible = is_access(SYS_Type_Access.THEM);
            if (!IsPostBack)
            {
                Session["ExceNXHN" + Sys_User.ID] = null;
                Session["ExcelNXHNMessageId" + Sys_User.ID] = null;

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
            if (Session["ExceNXHN" + Sys_User.ID] != null)
            {
                dt = (DataTable)Session["ExceNXHN" + Sys_User.ID];
            }

            if (dt != null && dt.Columns.Count > 0)
                RadGrid1.DataSource = dt;
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
                        else if (dt.Columns.Count < localAPI.ExcelColumnNameToNumber("E"))
                        {
                            for (int i = dt.Columns.Count - 1; i > localAPI.ExcelColumnNameToNumber("E"); i--)
                                dt.Columns.RemoveAt(i);
                        }
                        Session["ExceNXHN" + Sys_User.ID] = dt;
                        Session["ExcelNXHNMessageId" + Sys_User.ID] = message_id;

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
                            if (string.IsNullOrEmpty(row[1].ToString().Trim()))
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
                System.Web.UI.HtmlControls.HtmlImage error1 = (System.Web.UI.HtmlControls.HtmlImage)item.FindControl("iconErr");
                System.Web.UI.HtmlControls.HtmlImage success1 = (System.Web.UI.HtmlControls.HtmlImage)item.FindControl("iconSuccess");
                error1.Visible = false;
                success1.Visible = false;
            }
        }
        protected void btTaiExcel_Click(object sender, EventArgs e)
        {
            string fileMau = "Template-NhanXetHangNgay.xls";
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
                if (Session["ExceNXHN" + Sys_User.ID] != null)
                {
                    DataTable dt = (DataTable)Session["ExceNXHN" + Sys_User.ID];
                    if (dt.Rows.Count > 0)
                    {
                        RadGrid1.Rebind();
                        btnUpload.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Write(ex.ToString());
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Không có dữ liệu hoặc tệp sai cấu trúc!');", true);
            }
        }
        protected void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.THEM))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            if (Session["ExceNXHN" + Sys_User.ID] != null && Session["ExcelNXHNMessageId" + Sys_User.ID] != null)
            {
                List<ResultError> lstRes = new List<ResultError>();
                List<ResultEntity> lstResEntity = new List<ResultEntity>();
                ResultError res = new ResultError();
                CommonValidate validate = new CommonValidate();

                DataTable dt = new DataTable();
                dt = (DataTable)Session["ExceNXHN" + Sys_User.ID];
                int count_success = 0;
                int count_error = 0;
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
                    MA_NXHN entity = new MA_NXHN();
                    TextBox tbStt = (TextBox)(TextBox)row.FindControl("tbSTT");
                    TextBox tbMa = (TextBox)row.FindControl("tbMa");
                    TextBox tbNOI_DUNG = (TextBox)row.FindControl("tbNOI_DUNG");
                    TextBox tbThuTu = (TextBox)row.FindControl("tbThuTu");
                    HiddenField hdresMsg = (HiddenField)row.FindControl("hdresMsg");
                    System.Web.UI.HtmlControls.HtmlImage icError = (System.Web.UI.HtmlControls.HtmlImage)row.FindControl("iconErr");
                    System.Web.UI.HtmlControls.HtmlImage icSuccess = (System.Web.UI.HtmlControls.HtmlImage)row.FindControl("iconSuccess");
                    icError.Visible = false;
                    icSuccess.Visible = false;
                    long id = Convert.ToInt64(tbStt.Text.Trim());
                    string ma = !string.IsNullOrEmpty(tbMa.Text.Trim()) ? tbMa.Text.Trim() : "";
                    string noi_dung = (tbNOI_DUNG != null) ? tbNOI_DUNG.Text.Trim() : "";
                    string ThuTu = (tbThuTu != null) ? tbThuTu.Text.Trim() : "";
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
                        goto endxuly;

                    }
                    #endregion
                    #region Check Mã
                    if (validate.ValString(true, null, ma, out val, out error))
                    {
                        entity.MA = ma;
                    }
                    else
                    {
                        res.MA_LOI = count_row;
                        res.TEN_LOI = error;
                        res.COT = localAPI.ExcelColumnNameToNumber("B") - 1;
                        res.DONG = count_row;
                        res.TEN_COT = "Ký hiệu";
                        lstRes.Add(res);
                        resEntity.Res = false;
                        if (string.IsNullOrEmpty(resEntity.Msg))
                            resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                        else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                        goto endxuly;
                    }
                    #endregion
                    #region check Nội dung
                    if (validate.ValString(true, null, noi_dung, out val, out error))
                    {
                        entity.NOI_DUNG = noi_dung;
                    }
                    else
                    {
                        res.MA_LOI = count_row;
                        res.TEN_LOI = error;
                        res.COT = localAPI.ExcelColumnNameToNumber("C") - 1;
                        res.DONG = count_row;
                        res.TEN_COT = "Tên lớp";
                        lstRes.Add(res);
                        resEntity.Res = false;
                        if (string.IsNullOrEmpty(resEntity.Msg))
                            resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                        else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                        goto endxuly;
                    }
                    #endregion
                    #region "check thu tu"
                    if (validate.ValString(false, null, ThuTu, out val, out error))
                    {
                        entity.THU_TU = localAPI.ConvertStringToShort(ThuTu);
                    }
                    else
                    {
                        res.MA_LOI = count_row;
                        res.TEN_LOI = error;
                        res.COT = localAPI.ExcelColumnNameToNumber("D") - 1;
                        res.DONG = count_row;
                        res.TEN_COT = "Thứ tự";
                        lstRes.Add(res);
                        resEntity.Res = false;
                        if (string.IsNullOrEmpty(resEntity.Msg))
                            resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                        else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                    }
                    #endregion
                    #region check exists and udpate
                    MA_NXHN maNhanXet = nxhnBO.getMaNhanXetByMa(Sys_This_Truong.ID, ma);
                    bool is_update = maNhanXet != null;
                    if (maNhanXet == null) maNhanXet = new MA_NXHN();
                    maNhanXet.ID_TRUONG = Sys_This_Truong.ID;
                    maNhanXet.MA_CAP_HOC = Sys_This_Cap_Hoc;
                    if (lstColumn.Contains("MA"))
                        maNhanXet.MA = entity.MA.Trim();
                    if (lstColumn.Contains("NOI_DUNG"))
                        maNhanXet.NOI_DUNG = entity.NOI_DUNG.Trim();
                    if (lstColumn.Contains("THU_TU"))
                        maNhanXet.THU_TU = entity.THU_TU;
                    string strMsg = string.Empty;
                    if (resEntity.Res)
                    {
                        ResultEntity resEntity1 = new ResultEntity();
                        resEntity1.Res = true;
                        if (!is_update)
                        {
                            resEntity1 = nxhnBO.insert(maNhanXet, Sys_User.ID);
                            if (resEntity1.Res)
                            {
                                MA_NXHN resNXHN = (MA_NXHN)resEntity1.ResObject;
                                logUserBO.insert(Sys_This_Truong.ID, "IMPORT", "Thêm mới mã NXHN " + resNXHN.ID, Sys_User.ID, DateTime.Now);
                            }

                        }
                        else
                        {
                            resEntity1 = nxhnBO.update(maNhanXet, Sys_User.ID);
                            if (resEntity1.Res)
                                logUserBO.insert(Sys_This_Truong.ID, "IMPORT", "Cập nhật mã NXHN " + maNhanXet.ID, Sys_User.ID, DateTime.Now);
                        }
                        if (resEntity1.Res) count_success++;
                        else count_error++;
                    }
                    endxuly:
                    #endregion
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
                if (res.MA_LOI == 0 && count_error == 0)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('success', 'Cập nhật thành công!');", true);
                }
                else if (res.MA_LOI == 0 && count_error > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Chưa có bản ghi nào được cập nhật. Vui lòng liên hệ với quản trị viên để được hỗ trợ!');", true);
                }
                if (count_success > 0)
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Cập nhật " + count_success + "/" + count_total + " bản ghi. Vui lòng kiểm tra thông tin các bản ghi chưa cập nhật trong bảng Chi tiết nhập liệu');", true);
                else
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Cập nhật không thành công. Vui lòng kiểm tra thông tin chưa cập nhật trong bảng Chi tiết nhập liệu');", true);
            }
        }
    }
}