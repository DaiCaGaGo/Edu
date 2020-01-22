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

namespace CMS.HocSinh
{
    public partial class LichThiImportExcel : AuthenticatePage
    {
        private KhoiBO khoiBO = new KhoiBO();
        private LopBO lopBO = new LopBO();
        private HocSinhBO hocSinhBO = new HocSinhBO();
        LichThiBO lichThiBO = new LichThiBO();
        private LocalAPI localAPI = new LocalAPI();
        MonHocTruongBO monHocTruongBO = new MonHocTruongBO();
        LogUserBO logUserBO = new LogUserBO();
        List<string> lstValue = new List<string> { "ID_KHOI", "ID_LOP", "MON_THI", "LOAI_KIEM_TRA", "THOI_GIAN_LAM_BAI", "THOI_GIAN_THI" };
        List<string> lstText = new List<string> { "Khối", "Lớp", "Môn thi", "Loại kiểm tra", "Giời gian làm bài (phút)", "Thời gian thi" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Sys_This_Truong == null && Sys_This_Cap_Hoc == null)
                checkChonTruong();
            btCapNhat.Visible = is_access(SYS_Type_Access.THEM);
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
                Session["ExcelLichThi" + Sys_User.ID] = null;
                Session["ExcelLichThiMessageId" + Sys_User.ID] = null;
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            if (Session["ExcelLichThi" + Sys_User.ID] != null)
            {
                dt = (DataTable)Session["ExcelLichThi" + Sys_User.ID];
            }

            if (dt != null && dt.Columns.Count > 0)
                RadGrid1.DataSource = dt;

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
                        if (dt == null || dt.Columns.Count < lstValue.Count)
                        {
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'File chưa đúng mẫu, hoặc dữ liệu trống. Vui lòng kiểm tra lại!');", true);
                        }
                        Session["ExcelLichThi" + Sys_User.ID] = dt;
                        Session["ExcelLichThiMessageId" + Sys_User.ID] = message_id;

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

                #region Add cột thứ tự
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

                #region Xóa dữ liệu trống
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

                if (myDataTable.Rows.Count > 500)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Không upload file vượt quá 500 bản ghi!');", true);
                    return null;
                }
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
                //TextBox tbTHOI_GIAN_THI = (TextBox)e.Item.FindControl("tbTHOI_GIAN_THI");
                //HiddenField hdTHOI_GIAN_THI = (HiddenField)e.Item.FindControl("hdTHOI_GIAN_THI");
                System.Web.UI.HtmlControls.HtmlImage error1 = (System.Web.UI.HtmlControls.HtmlImage)item.FindControl("iconErr");
                System.Web.UI.HtmlControls.HtmlImage success1 = (System.Web.UI.HtmlControls.HtmlImage)item.FindControl("iconSuccess");
                error1.Visible = false;
                success1.Visible = false;

                #region Thời gian thi
                //if (!string.IsNullOrEmpty(tbTHOI_GIAN_THI.Text))
                //{
                //    try
                //    {
                //        tbTHOI_GIAN_THI.Text = Convert.ToDateTime(hdTHOI_GIAN_THI.Value).ToString("dd/MM/yyyy HH:mm");
                //    }
                //    catch { }
                //}
                #endregion
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
                if (Session["ExcelLichThi" + Sys_User.ID] != null)
                {
                    DataTable dt = (DataTable)Session["ExcelLichThi" + Sys_User.ID];
                    if (dt.Rows.Count > 0)
                    {
                        RadGrid1.Rebind();
                        btCapNhat.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Write(ex.ToString());
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
            if (Session["ExcelLichThi" + Sys_User.ID] != null && Session["ExcelLichThiMessageId" + Sys_User.ID] != null)
            {
                ResultError res = new ResultError();
                CommonValidate validate = new CommonValidate();

                DataTable dt = new DataTable();
                dt = (DataTable)Session["ExcelLichThi" + Sys_User.ID];
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

                List<KHOI> lstKhoi = khoiBO.getKhoiByCapHoc(Sys_This_Cap_Hoc);
                List<LOP> lstLop = lopBO.getLopByTruongCapNamHoc(Sys_This_Cap_Hoc, Sys_This_Truong.ID, (Int16)Sys_Ma_Nam_hoc);
                List<MON_HOC_TRUONG> listMonTruong = monHocTruongBO.getMonHocByTruongAndMaCapHoc(Sys_This_Truong.ID, Sys_This_Cap_Hoc, (Int16)Sys_Ma_Nam_hoc);
                foreach (GridDataItem row in RadGrid1.Items)
                {
                    ResultEntity resEntity = new ResultEntity();
                    resEntity.Res = true;
                    count_row++;
                    count_total++;
                    object val = new object();
                    string out_sdt = string.Empty;
                    string error = string.Empty;

                    #region get control
                    TextBox tbStt = (TextBox)row.FindControl("tbSTT");
                    TextBox tbKhoi = (TextBox)row.FindControl("tbIdKhoi");
                    TextBox tbLop = (TextBox)row.FindControl("tbIdLop");
                    TextBox tbMON_THI = (TextBox)row.FindControl("tbMON_THI");
                    TextBox tbLOAI_KIEM_TRA = (TextBox)row.FindControl("tbLOAI_KIEM_TRA");
                    TextBox tbTHOI_GIAN_LAM_BAI = (TextBox)row.FindControl("tbTHOI_GIAN_LAM_BAI");
                    TextBox tbTHOI_GIAN_THI = (TextBox)row.FindControl("tbTHOI_GIAN_THI");
                    HiddenField hdresMsg = (HiddenField)row.FindControl("hdresMsg");
                    System.Web.UI.HtmlControls.HtmlImage icError = (System.Web.UI.HtmlControls.HtmlImage)row.FindControl("iconErr");
                    System.Web.UI.HtmlControls.HtmlImage icSuccess = (System.Web.UI.HtmlControls.HtmlImage)row.FindControl("iconSuccess");
                    icError.Visible = false;
                    icSuccess.Visible = false;
                    #endregion

                    #region check stt
                    string id = tbStt.Text.Trim();
                    if (!validate.ValInt(true, null, null, id, out val, out error))
                    {
                        res = getError(count_row, error, "A", "STT");
                        resEntity.Res = false;
                        resEntity.Msg = "STT không được để trống.";
                        hdresMsg.Value = resEntity.Msg;
                        row.ForeColor = Color.Red;
                        icError.Visible = true;
                        continue;
                    }
                    #endregion

                    #region set môn học
                    long? id_mon_truong = 0;
                    string monHoc = tbMON_THI.Text.Trim();
                    if (!string.IsNullOrEmpty(monHoc))
                    {
                        MON_HOC_TRUONG monTruong = listMonTruong.Where(x => x.TEN.ToLower() == monHoc.ToLower()).FirstOrDefault();
                        if (monTruong == null)
                        {
                            res = getError(count_row, error, "D", "Môn thi");
                            resEntity.Res = false;
                            resEntity.Msg = "Môn thi chưa đúng.";
                            hdresMsg.Value = resEntity.Msg;
                            row.ForeColor = Color.Red;
                            icError.Visible = true;
                            continue;
                        }
                        else
                        {
                            id_mon_truong = monTruong.ID;
                        }
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
                    #endregion

                    #region set thời gian thi
                    DateTime? thoi_gian_thi = null;
                    try
                    {
                        thoi_gian_thi = Convert.ToDateTime(tbTHOI_GIAN_THI.Text.Trim());
                    }
                    catch
                    {
                        res = getError(count_row, error, "G", "Thời gian thi");
                        resEntity.Res = false;
                        resEntity.Msg = "Bạn phải nhập đúng thời gian thi.";
                        hdresMsg.Value = resEntity.Msg;
                        row.ForeColor = Color.Red;
                        icError.Visible = true;
                        continue;
                    }
                    #endregion

                    #region loại kiểm tra
                    string loai_kiem_tra = tbLOAI_KIEM_TRA.Text.Trim();
                    long type = 0;
                    if (loai_kiem_tra.ToLower() == "15 phút") type = 1;
                    else if (loai_kiem_tra.ToLower() == "1 tiết") type = 2;
                    else if (loai_kiem_tra.ToLower() == "giữa kỳ") type = 3;
                    else if (loai_kiem_tra.ToLower() == "học kỳ") type = 4;
                    if (type == 0)
                    {
                        res = getError(count_row, error, "E", "Loại kiểm tra");
                        resEntity.Res = false;
                        resEntity.Msg = "Loại kiểm tra không được để trống";
                        hdresMsg.Value = resEntity.Msg;
                        row.ForeColor = Color.Red;
                        icError.Visible = true;
                        continue;
                    }
                    #endregion

                    short? thoi_gian_lam_bai = localAPI.ConvertStringToShort(tbTHOI_GIAN_LAM_BAI.Text.Trim());

                    #region Khối
                    string strKhoi = tbKhoi.Text.Trim();
                    string strLop = tbLop.Text.Trim();

                    string[] arrKhoi = !string.IsNullOrEmpty(strKhoi) ? strKhoi.Split(',') : null;
                    string[] arrLop = !string.IsNullOrEmpty(strLop) ? strLop.Split(',') : null;

                    if (arrKhoi != null || arrKhoi.Length == 0)
                    {
                        if (arrLop == null || arrLop.Length == 0)//tất cả khối
                        {
                            for (int i = 0; i < arrKhoi.Length; i++)
                            {
                                short? id_khoi = localAPI.ConvertStringToShort(arrKhoi[i]);
                                if (id_khoi != null)
                                {
                                    List<LOP> lopInKhoi = lstLop.Where(x => x.ID_KHOI == id_khoi.Value).ToList();
                                    if (lopInKhoi.Count > 0)
                                    {
                                        for (int j = 0; j < lopInKhoi.Count; j++)
                                        {
                                            LICH_THI detail = lichThiBO.checkLichThiByMon(Sys_This_Truong.ID, id_khoi.Value, lopInKhoi[j].ID, (Int16)Sys_Hoc_Ky, id_mon_truong.Value);
                                            bool is_update = true;
                                            if (detail == null)
                                            {
                                                detail = new LICH_THI();
                                                is_update = false;
                                            }
                                            detail.ID_TRUONG = Sys_This_Truong.ID;
                                            detail.ID_KHOI = id_khoi.Value;
                                            detail.ID_NAM_HOC = (Int16)Sys_Ma_Nam_hoc;
                                            detail.HOC_KY = (Int16)Sys_Hoc_Ky;
                                            detail.ID_LOP = lopInKhoi[j].ID;
                                            detail.ID_MON_TRUONG = id_mon_truong;
                                            if (type == 1) detail.TIME_15P = thoi_gian_thi;
                                            else if (type == 2) detail.TIME_1T = thoi_gian_thi;
                                            else if (type == 3) detail.TIME_GK = thoi_gian_thi;
                                            else if (type == 4)
                                            {
                                                detail.TIME_HK = thoi_gian_thi;
                                                detail.THOI_GIAN_LAM_BAI = thoi_gian_lam_bai;
                                            }

                                            if (!is_update)
                                            {
                                                resEntity = lichThiBO.insert(detail, Sys_User.ID);
                                            }
                                            else
                                            {
                                                resEntity = lichThiBO.update(detail, Sys_User.ID);
                                            }
                                            if (resEntity.Res)
                                                count_success++;
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
                                        }
                                    }
                                }
                            }
                        }
                        else //set từng lớp trong khối
                        {
                            for (int i = 0; i < arrKhoi.Length; i++)
                            {
                                short? id_khoi = localAPI.ConvertStringToShort(arrKhoi[i]);
                                if (id_khoi != null)
                                {
                                    string[] arrLop_temp = arrLop;
                                    for (int j = 0; j < arrLop_temp.Length; j++)
                                    {
                                        LOP lop = lstLop.Where(x => x.ID_KHOI == id_khoi.Value && x.TEN == arrLop_temp[j] && (x.IS_DELETE == null || x.IS_DELETE == false)).FirstOrDefault();
                                        if (lop == null) continue;

                                        LICH_THI detail = lichThiBO.checkLichThiByMon(Sys_This_Truong.ID, id_khoi.Value, lop.ID, (Int16)Sys_Hoc_Ky, id_mon_truong.Value);
                                        bool is_update = true;
                                        if (detail == null)
                                        {
                                            detail = new LICH_THI();
                                            is_update = false;
                                        }
                                        detail.ID_TRUONG = Sys_This_Truong.ID;
                                        detail.ID_KHOI = id_khoi.Value;
                                        detail.ID_NAM_HOC = (Int16)Sys_Ma_Nam_hoc;
                                        detail.HOC_KY = (Int16)Sys_Hoc_Ky;
                                        detail.ID_LOP = lop.ID;
                                        detail.ID_MON_TRUONG = id_mon_truong;
                                        if (type == 1) detail.TIME_15P = thoi_gian_thi;
                                        else if (type == 2) detail.TIME_1T = thoi_gian_thi;
                                        else if (type == 3) detail.TIME_GK = thoi_gian_thi;
                                        else if (type == 4)
                                        {
                                            detail.TIME_HK = thoi_gian_thi;
                                            detail.THOI_GIAN_LAM_BAI = thoi_gian_lam_bai;
                                        }

                                        if (!is_update)
                                        {
                                            resEntity = lichThiBO.insert(detail, Sys_User.ID);
                                        }
                                        else
                                        {
                                            resEntity = lichThiBO.update(detail, Sys_User.ID);
                                        }
                                        if (resEntity.Res) count_success++;
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
                                        arrLop = arrLop.Where(x => x != arrLop_temp[j]).ToArray();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        res = getError(count_row, error, "B", "Khối");
                        resEntity.Res = false;
                        resEntity.Msg = "Khối học không được để trống";
                        hdresMsg.Value = resEntity.Msg;
                        row.ForeColor = Color.Red;
                        icError.Visible = true;
                        continue;
                    }
                    #endregion
                }

                if (res.MA_LOI == 0 && count_success > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('success', 'Cập nhật thành công " + count_success + " bản ghi!');", true);
                }
                else if (res.MA_LOI == 0 && count_success == 0)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Không có bản ghi nào được cập nhật.');", true);
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