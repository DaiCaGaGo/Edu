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

namespace CMS.CauHinhCaHoc
{
    public partial class ThoiKhoaBieuImportExcel : AuthenticatePage
    {
        private KhoiBO khoiBO = new KhoiBO();
        private LopBO lopBO = new LopBO();
        private HocSinhBO hocSinhBO = new HocSinhBO();
        LichThiBO lichThiBO = new LichThiBO();
        private LocalAPI localAPI = new LocalAPI();
        MonHocTruongBO monHocTruongBO = new MonHocTruongBO();
        LogUserBO logUserBO = new LogUserBO();
        CaHocBO caHocBO = new CaHocBO();
        private List<string> lstTitle = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Sys_This_Truong == null && Sys_This_Cap_Hoc == null)
                checkChonTruong();
            btCapNhat.Visible = is_access(SYS_Type_Access.THEM);
            if (!IsPostBack)
            {
                Session["ExcelThoiKhoaBieuLop" + Sys_User.ID] = null;
                Session["ExcelThoiKhoaBieuLopMessageId" + Sys_User.ID] = null;
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            if (Session["ExcelThoiKhoaBieuLop" + Sys_User.ID] != null)
            {
                dt = (DataTable)Session["ExcelThoiKhoaBieuLop" + Sys_User.ID];
            }

            if (Session["ExcelColumnTitleTKBLop" + Sys_User.ID] != null)
            {
                lstTitle = (List<string>)Session["ExcelColumnTitleTKBLop" + Sys_User.ID];
            }

            for (int i = 0; i < 15; i++)
            {
                if (i < lstTitle.Count)
                    RadGrid1.MasterTableView.Columns.FindByUniqueName("COT" + (i + 1)).Display = true;
                else
                    RadGrid1.MasterTableView.Columns.FindByUniqueName("COT" + (i + 1)).Display = false;
            }

            #region add thêm cột
            int count_true = lstTitle.Count;
            DataTable dt1 = dt;
            List<string> lstTitle1 = lstTitle;
            for (int i = count_true; i < 15; i++)
            {
                lstTitle1.Add("COT" + (i + 1));
                dt1.Columns.Add(new DataColumn("COT" + (i + 1)));
            }
            #endregion

            if (dt != null && dt.Columns.Count > 0)
                RadGrid1.DataSource = dt1;

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
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

                        if (dt.Columns.Count < localAPI.ExcelColumnNameToNumber("T"))
                        {
                            for (int i = dt.Columns.Count - 1; i > localAPI.ExcelColumnNameToNumber("T"); i--)
                                dt.Columns.RemoveAt(i);
                        }
                        Session["ExcelThoiKhoaBieuLop" + Sys_User.ID] = dt;
                        Session["ExcelThoiKhoaBieuLopMessageId" + Sys_User.ID] = message_id;

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

                #region Get tên cột (max = 15 cột)
                int max_cot = myDataTable.Columns.Count;
                if (max_cot > 15) max_cot = 15;
                lstTitle = new List<string>();
                for (int i = 0; i < max_cot; i++)
                {
                    try
                    {
                        lstTitle.Add(myDataTable.Columns[i].ColumnName);
                        myDataTable.Columns[i].ColumnName = "COT" + (i + 1);
                    }
                    catch { }
                }
                if (lstTitle.Count < 3)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'File dữ liệu chưa đúng. Vui lòng kiểm tra lại!');", true);
                    return null;
                }
                Session["ExcelColumnTitleTKBLop" + Sys_User.ID] = lstTitle;
                #endregion

                #region Valid excel
                dt = myDataTable.Copy();
                if (myDataTable.Rows.Count != 0)
                {
                    if (lstTitle[0].ToNormalizeLowerRelace() != "STT".ToNormalizeLowerRelace())
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Cột đầu tiên phải là cột 'STT'!');", true);
                        return null;
                    }
                    if (lstTitle[1].ToNormalizeLowerRelace() != "Thứ".ToNormalizeLowerRelace())
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Cột đầu tiên phải có tên 'Thứ'!');", true);
                        return null;
                    }
                    if (lstTitle[2].ToNormalizeLowerRelace() != "Tiết".ToNormalizeLowerRelace())
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Cột thứ hai phải có tên 'Tiết'!');", true);
                        return null;
                    }
                }
                #endregion

                #region xóa dữ liệu trống
                int k = 0;
                int count = 0;
                foreach (DataRow row in dt.Rows)
                {
                    Boolean kt = true;
                    if (string.IsNullOrEmpty(row[0].ToString().Trim()) || string.IsNullOrEmpty(row[2].ToString().Trim()))
                    {
                        kt = false;
                    }
                    if (!kt)
                    {
                        myDataTable.Rows.RemoveAt(k);
                        count++;
                    }
                    else
                        k++;
                }
                if (count > 0) ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Có " + count + " tiết học đang để trống!');", true);
                #endregion

                if (myDataTable.Rows.Count > 500)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Không upload file vượt quá 500 bản ghi!');", true);
                    return null;
                }
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
        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridHeaderItem)
            {
                GridHeaderItem item = (GridHeaderItem)e.Item;
                Label lblCOT1 = (Label)item.FindControl("lblCOT1");
                Label lblCOT2 = (Label)item.FindControl("lblCOT2");
                Label lblCOT3 = (Label)item.FindControl("lblCOT3");
                Label lblCOT4 = (Label)item.FindControl("lblCOT4");
                Label lblCOT5 = (Label)item.FindControl("lblCOT5");
                Label lblCOT6 = (Label)item.FindControl("lblCOT6");
                Label lblCOT7 = (Label)item.FindControl("lblCOT7");
                Label lblCOT8 = (Label)item.FindControl("lblCOT8");
                Label lblCOT9 = (Label)item.FindControl("lblCOT9");
                Label lblCOT10 = (Label)item.FindControl("lblCOT10");
                Label lblCOT11 = (Label)item.FindControl("lblCOT11");
                Label lblCOT12 = (Label)item.FindControl("lblCOT12");
                Label lblCOT13 = (Label)item.FindControl("lblCOT13");
                Label lblCOT14 = (Label)item.FindControl("lblCOT14");
                Label lblCOT15 = (Label)item.FindControl("lblCOT15");

                int max_cot = lstTitle.Count;
                if (lstTitle.Count > 0)
                {
                    lblCOT1.Text = lstTitle[0];
                    lblCOT2.Text = lstTitle[1];
                    lblCOT3.Text = lstTitle[2];
                    if (lstTitle.Count > 3) lblCOT4.Text = lstTitle[3];
                    if (lstTitle.Count > 4) lblCOT5.Text = lstTitle[4];
                    if (lstTitle.Count > 5) lblCOT6.Text = lstTitle[5];
                    if (lstTitle.Count > 6) lblCOT7.Text = lstTitle[6];
                    if (lstTitle.Count > 7) lblCOT8.Text = lstTitle[7];
                    if (lstTitle.Count > 8) lblCOT9.Text = lstTitle[8];
                    if (lstTitle.Count > 9) lblCOT10.Text = lstTitle[9];
                    if (lstTitle.Count > 10) lblCOT11.Text = lstTitle[10];
                    if (lstTitle.Count > 11) lblCOT12.Text = lstTitle[11];
                    if (lstTitle.Count > 12) lblCOT13.Text = lstTitle[12];
                    if (lstTitle.Count > 13) lblCOT14.Text = lstTitle[13];
                    if (lstTitle.Count > 14) lblCOT15.Text = lstTitle[14];
                }
            }
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
            string fileMau = "Template-ThoiKhoaBieu.xls";

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
                if (Session["ExcelThoiKhoaBieuLop" + Sys_User.ID] != null)
                {
                    DataTable dt = (DataTable)Session["ExcelThoiKhoaBieuLop" + Sys_User.ID];
                    if (dt.Rows.Count > 0)
                    {
                        RadGrid1.Rebind();
                        btCapNhat.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Không có dữ liệu hoặc tệp sai cấu trúc!');", true);
            }
        }
        protected void btCapNhat_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.THEM))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }

            ResultError res = new ResultError();
            CommonValidate validate = new CommonValidate();

            lstTitle = (List<string>)Session["ExcelColumnTitleTKBLop" + Sys_User.ID];

            if (Session["ExcelThoiKhoaBieuLop" + Sys_User.ID] != null && Session["ExcelThoiKhoaBieuLopMessageId" + Sys_User.ID] != null)
            {
                int countColumn = 3;
                foreach (GridColumn item in RadGrid1.MasterTableView.Columns)
                {
                    if (localAPI.checkColumnShowInRadGrid(RadGrid1, "COT4") && item.UniqueName == "COT4")
                        countColumn++;
                    else if (localAPI.checkColumnShowInRadGrid(RadGrid1, "COT5") && item.UniqueName == "COT5") countColumn++;
                    else if (localAPI.checkColumnShowInRadGrid(RadGrid1, "COT6") && item.UniqueName == "COT6") countColumn++;
                    else if (localAPI.checkColumnShowInRadGrid(RadGrid1, "COT7") && item.UniqueName == "COT7") countColumn++;
                    else if (localAPI.checkColumnShowInRadGrid(RadGrid1, "COT8") && item.UniqueName == "COT8") countColumn++;
                    else if (localAPI.checkColumnShowInRadGrid(RadGrid1, "COT9") && item.UniqueName == "COT9") countColumn++;
                    else if (localAPI.checkColumnShowInRadGrid(RadGrid1, "COT10") && item.UniqueName == "COT10") countColumn++;
                    else if (localAPI.checkColumnShowInRadGrid(RadGrid1, "COT11") && item.UniqueName == "COT11") countColumn++;
                    else if (localAPI.checkColumnShowInRadGrid(RadGrid1, "COT12") && item.UniqueName == "COT12") countColumn++;
                    else if (localAPI.checkColumnShowInRadGrid(RadGrid1, "COT13") && item.UniqueName == "COT13") countColumn++;
                    else if (localAPI.checkColumnShowInRadGrid(RadGrid1, "COT14") && item.UniqueName == "COT14") countColumn++;
                    else if (localAPI.checkColumnShowInRadGrid(RadGrid1, "COT15") && item.UniqueName == "COT15") countColumn++;
                }

                if (countColumn > 3)
                {
                    int class_success = 0;
                    for (int i = 4; i <= countColumn; i++)
                    {
                        int count_row = 0;
                        string error = string.Empty;

                        long? id_lop = null;
                        string tenLop = lstTitle[i - 1];
                        if (!string.IsNullOrEmpty(tenLop))
                        {
                            id_lop = lopBO.getIdLopByTen(Sys_This_Truong.ID, (Int16)Sys_Ma_Nam_hoc, tenLop);
                            if (id_lop != null)
                            {
                                int count_success = 0;
                                List<CA_HOC> listLichHocLop = caHocBO.getCaHocByLopAndHocKy(Sys_This_Truong.ID, id_lop, (Int16)Sys_Hoc_Ky);
                                List<MON_HOC_TRUONG> listMonHoc = monHocTruongBO.getMonHocByTruongAndMaCapHoc(Sys_This_Truong.ID, Sys_This_Cap_Hoc, (Int16)Sys_Ma_Nam_hoc);
                                foreach (GridDataItem row in RadGrid1.Items)
                                {
                                    ResultEntity resEntity = new ResultEntity();
                                    resEntity.Res = true;
                                    count_row++;
                                    #region get control
                                    TextBox tbThu = (TextBox)row.FindControl("tbCOT2");
                                    TextBox tbTiet = (TextBox)row.FindControl("tbCOT3");
                                    TextBox tbMonHoc = (TextBox)row.FindControl("tbCOT" + i);
                                    HiddenField hdresMsg = (HiddenField)row.FindControl("hdresMsg");
                                    System.Web.UI.HtmlControls.HtmlImage icError = (System.Web.UI.HtmlControls.HtmlImage)row.FindControl("iconErr");
                                    System.Web.UI.HtmlControls.HtmlImage icSuccess = (System.Web.UI.HtmlControls.HtmlImage)row.FindControl("iconSuccess");
                                    icError.Visible = false;
                                    icSuccess.Visible = false;
                                    #endregion

                                    string thu = tbThu.Text.Trim();
                                    short? tiet = localAPI.ConvertStringToShort(tbTiet.Text.Trim());
                                    string monHoc = tbMonHoc.Text.Trim();

                                    if (tiet != null && !string.IsNullOrEmpty(thu) && !string.IsNullOrEmpty(monHoc))
                                    {
                                        MON_HOC_TRUONG monTruong = listMonHoc.Where(x => x.TEN.ToLower() == monHoc.ToLower()).FirstOrDefault();
                                        CA_HOC caHoc = listLichHocLop.Where(x => x.TIET == tiet).FirstOrDefault();
                                        Boolean is_update = true;
                                        if (caHoc == null)
                                        {
                                            is_update = false;
                                            caHoc = new CA_HOC();
                                            caHoc.TIET = tiet;
                                            caHoc.ID_TRUONG = Sys_This_Truong.ID;
                                            caHoc.ID_LOP = id_lop.Value;
                                            caHoc.ID_HOC_KY = (Int16)Sys_Hoc_Ky;
                                            caHoc.MA_NAM_HOC = (Int16)Sys_Ma_Nam_hoc;
                                        }
                                        if (monTruong != null)
                                        {
                                            string strThu = thu.ToLower();
                                            if (strThu == "thứ 2" || strThu == "2")
                                            {
                                                caHoc.ID_MON_2 = monTruong.ID;
                                            }
                                            else if (strThu == "thứ 3" || strThu == "3")
                                            {
                                                caHoc.ID_MON_3 = monTruong.ID;
                                            }
                                            else if (strThu == "thứ 4" || strThu == "4")
                                            {
                                                caHoc.ID_MON_4 = monTruong.ID;
                                            }
                                            else if (strThu == "thứ 5" || strThu == "5")
                                            {
                                                caHoc.ID_MON_5 = monTruong.ID;
                                            }
                                            else if (strThu == "thứ 6" || strThu == "6")
                                            {
                                                caHoc.ID_MON_6 = monTruong.ID;
                                            }
                                            else if (strThu == "thứ 7" || strThu == "7")
                                            {
                                                caHoc.ID_MON_7 = monTruong.ID;
                                            }
                                            else if (strThu == "chủ nhật" || strThu == "8")
                                            {
                                                caHoc.ID_MON_8 = monTruong.ID;
                                            }
                                            ResultEntity result = new ResultEntity();
                                            if (is_update)
                                                caHocBO.update(caHoc, Sys_User.ID);
                                            else caHocBO.insert(caHoc, Sys_User.ID);
                                            if (result.Res) count_success++;
                                        }
                                        else
                                        {
                                            res = getError(count_row, error, "D", "Môn thi");
                                            resEntity.Res = false;
                                            resEntity.Msg = "Môn thi không được để trống";
                                            hdresMsg.Value = resEntity.Msg;
                                            row.ForeColor = Color.Red;
                                            icError.Visible = true;
                                            continue;
                                        }
                                    }
                                }
                                if (count_success > 0) class_success++;
                            }
                        }
                    }
                    if (class_success > 0)
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('success', 'Có " + class_success + " lớp được cập nhật lịch học.');", true);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Không có lớp cần cập nhật lịch học. Vui lòng kiểm tra lại');", true);
                    return;
                }
            }
        }
        protected ResultError getError(int count_row, string error, string column, string columnName)
        {
            ResultError resultError = new ResultError();
            resultError.MA_LOI = count_row;
            resultError.TEN_LOI = error;
            resultError.COT = localAPI.ExcelColumnNameToNumber(column) - 1;
            resultError.DONG = count_row;
            resultError.TEN_COT = columnName;
            return resultError;
        }
    }
}