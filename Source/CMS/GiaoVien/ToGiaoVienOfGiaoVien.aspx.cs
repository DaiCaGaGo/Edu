using ClosedXML.Excel;
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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CMS.GiaoVien
{
    public partial class ToGiaoVienOfGiaoVien : AuthenticatePage
    {
        LocalAPI localAPI = new LocalAPI();
        GiaoVienBO gvBO = new GiaoVienBO();
        DmToGiaoVienBO togiaovienBO = new DmToGiaoVienBO();
        CommonValidate validate = new CommonValidate();
        LogUserBO logUserBO = new LogUserBO();
        long? add_gv;
        List<string> lstValue = new List<string> { "HO_TEN", "SDT" };
        List<string> lstText = new List<string> { "Họ tên", "Số điện thoại" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("add_gv") != null)
            {
                try
                {
                    add_gv = Convert.ToInt16(Request.QueryString.Get("add_gv"));
                }
                catch { }
            }
            if (!IsPostBack)
            {
                if (add_gv != null)
                {
                    objToGiaoVien.SelectParameters.Add("id_truong", Sys_This_Truong.ID.ToString());
                    rcbToGiaoVien.SelectedValue = Convert.ToString(add_gv.Value);
                    rcbToGiaoVien.DataBind();
                    objGiaoVien.SelectParameters.Add("id_truong", Sys_This_Truong.ID.ToString());
                    rcbGiaoVien.DataBind();
                }
                else
                {
                }
            }
        }
        protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
        {
            if (e.Argument == "RadWindowManager1_Close")
            {
                RadGrid1.Rebind();
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = togiaovienBO.getGiaoVienExistsToGiaoVien_GiaoVien(localAPI.ConvertStringTolong(rcbToGiaoVien.SelectedValue));
        }
        protected void btSave_Click(object sender, EventArgs e)
        {
            string strMsg = "";
            int countCheck = 0;
            foreach (RadComboBoxItem item in rcbGiaoVien.Items)
            {
                if (item.Checked)
                {
                    countCheck++;
                    TO_GIAO_VIEN_GV detailTO_GIAO_VIEN_GV = new TO_GIAO_VIEN_GV();
                    detailTO_GIAO_VIEN_GV.ID_TO = Convert.ToInt64(rcbToGiaoVien.SelectedValue);
                    detailTO_GIAO_VIEN_GV.ID_GIAO_VIEN = Convert.ToInt64(item.Value);
                    detailTO_GIAO_VIEN_GV.ID_TRUONG = Sys_This_Truong.ID;
                    ResultEntity res = togiaovienBO.insertOrUpdate(detailTO_GIAO_VIEN_GV, Sys_User.ID);

                    if (res.Res)
                    {
                        strMsg = "notification('success', '" + res.Msg + "');";
                        logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật gv " + item.Value + " vào tổ " + rcbToGiaoVien.SelectedValue, Sys_User.ID, DateTime.Now);
                    }
                    else
                    {
                        strMsg = "notification('error', '" + res.Msg + "');";
                        break;
                    }
                }
            }
            if (countCheck == 0)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Chưa có giáo viên nào được chọn!');", true);
                return;
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            rcbGiaoVien.DataBind();
            RadGrid1.Rebind();
        }
        protected void btDeleteChon_Click(object sender, EventArgs e)
        {
            int success = 0, error = 0; string lst_id = string.Empty;

            foreach (GridDataItem row in RadGrid1.SelectedItems)
            {
                try
                {
                    short idGiaoVien = Convert.ToInt16(row.GetDataKeyValue("ID").ToString());
                    string ten = row["HO_TEN"].Text;
                    try
                    {
                        ResultEntity res = togiaovienBO.deleteGiaoVien_in_ToGV(Sys_User.ID, Convert.ToInt64(rcbToGiaoVien.SelectedValue), idGiaoVien, true);
                        lst_id += idGiaoVien + ":" + ten + ", ";
                        if (res.Res)
                        {
                            success++;
                            logUserBO.insert(Sys_This_Truong.ID, "DELETE", "Xóa gv " + idGiaoVien + " khỏi tổ " + rcbToGiaoVien.SelectedValue, Sys_User.ID, DateTime.Now);
                        }
                        else
                            error++;
                    }
                    catch
                    {
                        error++;
                    }
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Lỗi hệ thống. Bạn vui lòng kiểm tra lại');", true);
                }
            }
            string strMsg = "";
            if (error > 0)
            {
                strMsg = "notification('error', 'Có " + error + " bản ghi chưa được xóa. Liên hệ với quản trị viên');";
            }
            if (success > 0)
            {
                strMsg += " notification('success', 'Có " + success + " bản ghi được xóa.');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            rcbGiaoVien.DataBind();
            RadGrid1.Rebind();
        }
        protected void rcbToGiaoVien_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void btTaiExcel_Click(object sender, EventArgs e)
        {
            string fileMau = "Template-GiaoVien-ToGV.xls";

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
                int success = 0, error = 0;
                if (Session["ExcelGiaoVienToGV" + Sys_User.ID] != null)
                {
                    DataTable dt = (DataTable)Session["ExcelGiaoVienToGV" + Sys_User.ID];
                    if (dt.Rows.Count > 0)
                    {
                        string sdt = "";
                        long id_to_gv = 0;
                        if (rcbToGiaoVien.SelectedValue != "") id_to_gv = Convert.ToInt64(rcbToGiaoVien.SelectedValue);
                        List<GIAO_VIEN> gvInTruong = gvBO.getGiaoVienInTruong(Sys_This_Truong.ID);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sdt = dt.Rows[i]["SDT"].ToString();
                            #region check SĐT
                            string out_sdt = string.Empty;
                            string out_error = string.Empty;
                            if (validate.ValidateSDT(false, sdt, out out_sdt, out out_error))
                            {
                                sdt = out_sdt;
                            }
                            #endregion
                            //string ho_ten = "";
                            //ho_ten = dt.Rows[i]["HO_TEN"].ToString();
                            #region thêm gv vào tổ nếu gv này đã có trong ds GV
                            List<GIAO_VIEN> lstGV = gvInTruong.Where(x => x.SDT == sdt).ToList();
                            int count = 0;

                            if (lstGV.Count > 0)
                            {
                                for (int k = 0; k < lstGV.Count; k++)
                                {
                                    if (count > 1) break;
                                    if (lstGV[k].SDT == sdt)
                                    {
                                        GiaoVienInToEntity checkExist = togiaovienBO.checkExistSdtGiaoVienInTo(Sys_This_Truong.ID, id_to_gv, sdt);
                                        if (checkExist == null)
                                        {
                                            TO_GIAO_VIEN_GV detailTO_GIAO_VIEN_GV = new TO_GIAO_VIEN_GV();
                                            detailTO_GIAO_VIEN_GV.ID_TO = id_to_gv;
                                            detailTO_GIAO_VIEN_GV.ID_GIAO_VIEN = Convert.ToInt64(lstGV[k].ID);
                                            detailTO_GIAO_VIEN_GV.ID_TRUONG = Sys_This_Truong.ID;
                                            ResultEntity res = togiaovienBO.insertOrUpdate(detailTO_GIAO_VIEN_GV, Sys_User.ID);
                                            if (res.Res)
                                            {
                                                success++;
                                                count++;
                                                logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật gv " + lstGV[k].ID + " vào tổ " + rcbToGiaoVien.SelectedValue, Sys_User.ID, DateTime.Now);
                                            }
                                            else error++;
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    string strMsg = "";
                    if (error > 0)
                    {
                        strMsg = "notification('error', 'Có " + error + " bản ghi bị lỗi, vui lòng kiểm tra lại');";
                    }
                    if (success > 0)
                    {
                        strMsg = "notification('success', 'Có " + success + " bản ghi được lưu');";
                    }
                    else
                    {
                        strMsg = "notification('warning', 'Không có bản ghi nào được lưu');";
                    }
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
                    RadGrid1.Rebind();
                }
            }
            catch (Exception ex)
            {
                //Response.Write(ex.ToString());
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Không có dữ liệu hoặc tệp sai cấu trúc!');", true);
            }
            Session["ExcelGiaoVienToGV" + Sys_User.ID] = null;
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
                        Session["ExcelGiaoVienToGV" + Sys_User.ID] = dt;
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
                    catch (Exception ex) { }
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
                            if (string.IsNullOrEmpty(row[2].ToString().Trim())
                                )
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
        protected void btExport_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.EXPORT))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            DataTable dt = new DataTable();
            string serverPath = Server.MapPath("~");
            string path = String.Format("{0}\\ExportTemplates\\FileDynamic.xlsx", Server.MapPath("~"));
            string newName = "Danh_sach_giao_vien_to.xlsx";

            List<ExcelHeaderEntity> lstHeader = new List<ExcelHeaderEntity>();
            List<ExcelEntity> lstColumn = new List<ExcelEntity>();
            lstHeader.Add(new ExcelHeaderEntity { name = "STT", colM = 1, rowM = 1, width = 10 });
            foreach (GridColumn item in RadGrid1.MasterTableView.Columns)
            {
                #region add column
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "HO_TEN") && item.UniqueName == "HO_TEN")
                {
                    DataColumn col = new DataColumn("HO_TEN");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Họ tên", colM = 1, rowM = 1, width = 60 });
                    lstColumn.Add(new ExcelEntity { Name = "HO_TEN", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "SDT") && item.UniqueName == "SDT")
                {
                    DataColumn col = new DataColumn("SDT");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Số điện thoại", colM = 1, rowM = 1, width = 30 });
                    lstColumn.Add(new ExcelEntity { Name = "SDT", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                #endregion
            }
            RadGrid1.AllowPaging = false;
            foreach (GridDataItem item in RadGrid1.Items)
            {
                DataRow row = dt.NewRow();
                foreach (DataColumn col in dt.Columns)
                {
                    row[col.ColumnName] = item[col.ColumnName].Text.Replace("&nbsp;", " ");
                }
                dt.Rows.Add(row);
            }
            int rowHeaderStart = 6;
            int rowStart = 7;
            string colStart = "A";
            string colEnd = localAPI.GetExcelColumnName(lstColumn.Count);
            List<ExcelHeaderEntity> listCell = new List<ExcelHeaderEntity>();
            string tenSo = "";
            string tenPhong = Sys_This_Truong.TEN;
            string tieuDe = "DANH SÁCH GIÁO VIÊN TỔ " + rcbToGiaoVien.Text.Trim().ToUpper();
            string hocKyNamHoc = "Năm học: " + Sys_Ten_Nam_Hoc;
            listCell.Add(new ExcelHeaderEntity { name = tenSo, colM = 3, rowM = 1, colName = "A", rowIndex = 1, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tenPhong, colM = 3, rowM = 1, colName = "A", rowIndex = 2, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tieuDe, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 3, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            listCell.Add(new ExcelHeaderEntity { name = hocKyNamHoc, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 4, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            string nameFileOutput = newName;
            localAPI.ExportExcelDynamic(serverPath, path, newName, nameFileOutput, 1, listCell, rowHeaderStart, rowStart, colStart, colEnd, dt, lstHeader, lstColumn, true);
        }
    }
}