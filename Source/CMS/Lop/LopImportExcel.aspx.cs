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

namespace CMS.Lop
{
    public partial class LopImportExcel : AuthenticatePage
    {
        private LopBO lopBO = new LopBO();
        private KhoiBO khoiBO = new KhoiBO();
        private LocalAPI localAPI = new LocalAPI();

        List<string> lstValue = new List<string> { "ID_KHOI", "ID_GVCN", "TEN", "THU_TU", "MA_LOAI_LOP_GDTX", "LOAI_CHEN_TIN", "TIEN_TO" };
        List<string> lstText = new List<string> { "Tên khối học", "GVCN", "Tên lớp", "Thứ tự", "Mã loại lớp GDTX", "Loại tiền tố tin nhắn", "Tiền tố tin nhắn" };
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            bt_EXCELtoSQL.Visible = is_access(SYS_Type_Access.THEM);
            btnUpload.Visible = is_access(SYS_Type_Access.THEM);
            if (!IsPostBack)
            {
                objKhoi.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                objKhoi.SelectParameters.Add("maLoaiLopGDTX", DbType.Int16, Sys_This_Lop_GDTX == null ? "" : Sys_This_Lop_GDTX.ToString());
                Session["ExcelLop" + Sys_User.ID] = null;
                Session["ExcelLopMessageId" + Sys_User.ID] = null;
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
            if (Session["ExcelLop" + Sys_User.ID] != null)
            {
                dt = (DataTable)Session["ExcelLop" + Sys_User.ID];
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
                if (Session["ExcelLop" + Sys_User.ID] != null)
                {
                    DataTable dt = (DataTable)Session["ExcelLop" + Sys_User.ID];
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
                        Session["ExcelLop" + Sys_User.ID] = dt;
                        Session["ExcelLopMessageId" + Sys_User.ID] = message_id;

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
                            if (string.IsNullOrEmpty(row[1].ToString().Trim())
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
                RadComboBox rcbKhoiHocItem = (RadComboBox)e.Item.FindControl("rcbKhoiHoc");

                HiddenField hdKhoiHoc = (HiddenField)e.Item.FindControl("hdKhoiHoc");
                RadComboBox rcbLoaiChenTin = (RadComboBox)e.Item.FindControl("rcbLoaiChenTin");
                HiddenField hdLOAI_CHEN_TIN = (HiddenField)e.Item.FindControl("hdLOAI_CHEN_TIN");
                System.Web.UI.HtmlControls.HtmlImage error1 = (System.Web.UI.HtmlControls.HtmlImage)item.FindControl("iconErr");
                System.Web.UI.HtmlControls.HtmlImage success1 = (System.Web.UI.HtmlControls.HtmlImage)item.FindControl("iconSuccess");
                error1.Visible = false;
                success1.Visible = false;
                #region khối học
                string strKhoiHoc = hdKhoiHoc.Value.ToNormalizeLowerRelace();
                if (!string.IsNullOrEmpty(strKhoiHoc))
                {
                    List<KHOI> lstkhoihoc = khoiBO.getKhoi(Sys_This_Cap_Hoc);
                    KHOI khoihoc = lstkhoihoc.FirstOrDefault(x => x.TEN.ToNormalizeLowerRelace() == strKhoiHoc);
                    if (khoihoc != null)
                    {
                        rcbKhoiHocItem.SelectedValue = khoihoc.MA.ToString();
                    }
                }
                #endregion
                #region loại chèn tin
                string strLoaiChenTin = hdLOAI_CHEN_TIN.Value.Trim();
                if (!string.IsNullOrEmpty(strLoaiChenTin))
                {
                    switch (strLoaiChenTin)
                    {
                        case "Tên":
                            rcbLoaiChenTin.SelectedValue = "2";
                            break;
                        case "Họ tên":
                            rcbLoaiChenTin.SelectedValue = "1";
                            break;
                        case "Tiền tố":
                            rcbLoaiChenTin.SelectedValue = "3";
                            break;
                        default:
                            rcbLoaiChenTin.SelectedValue = "2";
                            break;
                    }
                    //if (strLoaiChenTin == "Tên")
                    //    rcbLoaiChenTin.SelectedValue = "2";
                    //else { }
                    //if (strLoaiChenTin == "Họ tên")
                    //    rcbLoaiChenTin.SelectedValue = "1";
                    //else { }
                    //if (strLoaiChenTin == "Tiền tố")
                    //    rcbLoaiChenTin.SelectedValue = "3";
                    //else { }
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
            if (Session["ExcelLop" + Sys_User.ID] != null && Session["ExcelLopMessageId" + Sys_User.ID] != null)
            {
                List<ResultError> lstRes = new List<ResultError>();
                List<ResultEntity> lstResEntity = new List<ResultEntity>();
                ResultError res = new ResultError();
                CommonValidate validate = new CommonValidate();

                DataTable dt = new DataTable();
                dt = (DataTable)Session["ExcelLop" + Sys_User.ID];
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
                    LOP entity = new LOP();
                    TextBox tbStt = (TextBox)(TextBox)row.FindControl("tbSTT");
                    RadComboBox rcbKhoiHoc = (RadComboBox)row.FindControl("rcbKhoiHoc");
                    TextBox tbTen = (TextBox)row.FindControl("tbTen");
                    TextBox tbMA_LOAI_LOP_GDTX = (TextBox)row.FindControl("tbMA_LOAI_LOP_GDTX");
                    TextBox tbGVCN = (TextBox)row.FindControl("tbGVCN");
                    TextBox tbThuTu = (TextBox)row.FindControl("tbThuTu");
                    TextBox tbTienTo = (TextBox)row.FindControl("tbTienTo");
                    RadComboBox rcbLoaiChenTin = (RadComboBox)row.FindControl("rcbLoaiChenTin");
                    HiddenField hdresMsg = (HiddenField)row.FindControl("hdresMsg");
                    System.Web.UI.HtmlControls.HtmlImage icError = (System.Web.UI.HtmlControls.HtmlImage)row.FindControl("iconErr");
                    System.Web.UI.HtmlControls.HtmlImage icSuccess = (System.Web.UI.HtmlControls.HtmlImage)row.FindControl("iconSuccess");
                    icError.Visible = false;
                    icSuccess.Visible = false;
                    long id = Convert.ToInt64(tbStt.Text.Trim());
                    string KhoiHoc = rcbKhoiHoc.SelectedValue;
                    string TenLop = (tbTen != null) ? tbTen.Text.Trim() : "";
                    string tenGVCN = (tbGVCN != null) ? tbGVCN.Text.Trim() : "";
                    string ThuTu = (tbThuTu != null) ? tbThuTu.Text.Trim() : "";
                    string LoaiChenTin = rcbLoaiChenTin.SelectedValue;
                    string ma_LOAI_LOP_GDTX = (tbMA_LOAI_LOP_GDTX != null) ? tbMA_LOAI_LOP_GDTX.Text.Trim() : "";
                    string tien_to = tbTienTo != null ? tbTienTo.Text.Trim() : "";
                    #region "check STT"
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
                    #region "check Ten lop"
                    if (validate.ValString(true, null, TenLop, out val, out error))
                    {
                        entity.TEN = TenLop;
                    }
                    else
                    {
                        res.MA_LOI = count_row;
                        res.TEN_LOI = error;
                        res.COT = localAPI.ExcelColumnNameToNumber("D") - 1;
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
                    #region "check khoi hoc"
                    if (validate.ValInt(true, null, null, KhoiHoc.ToString(), out val, out error))
                    {
                        entity.ID_KHOI = localAPI.ConvertStringToShort(rcbKhoiHoc.SelectedValue).Value;
                    }
                    else
                    {
                        res.MA_LOI = count_row;
                        res.TEN_LOI = error;
                        res.COT = localAPI.ExcelColumnNameToNumber("B") - 1;
                        res.DONG = count_row;
                        res.TEN_COT = "Khối học";
                        lstRes.Add(res);
                        resEntity.Res = false;
                        if (string.IsNullOrEmpty(resEntity.Msg))
                            resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                        else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                        goto endxuly;
                    }
                    #endregion
                    #region "check giáo viên"
                    if (validate.ValInt(false, null, null, tenGVCN.ToString(), out val, out error))
                    {
                        try
                        {
                            GiaoVienBO giaoVienBO = new GiaoVienBO();
                            GIAO_VIEN GiaoVien = giaoVienBO.getGiaoVienByTen(Sys_This_Truong.ID, tenGVCN);
                            if (GiaoVien != null)
                                entity.ID_GVCN = GiaoVien.ID;
                            else entity.ID_GVCN = null;
                        }
                        catch { entity.ID_GVCN = null; }
                    }
                    else
                    {
                        res.MA_LOI = count_row;
                        res.TEN_LOI = error;
                        res.COT = localAPI.ExcelColumnNameToNumber("C") - 1;
                        res.DONG = count_row;
                        res.TEN_COT = "Giáo viên chủ nhiệm";
                        lstRes.Add(res);
                        resEntity.Res = false;
                        if (string.IsNullOrEmpty(resEntity.Msg))
                            resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                        else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
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
                        res.COT = localAPI.ExcelColumnNameToNumber("I") - 1;
                        res.DONG = count_row;
                        res.TEN_COT = "Thứ tự";
                        lstRes.Add(res);
                        resEntity.Res = false;
                        if (string.IsNullOrEmpty(resEntity.Msg))
                            resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                        else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                    }
                    #endregion
                    #region "check loại chèn tin"
                    if (validate.ValInt(false, null, null, LoaiChenTin.ToString(), out val, out error))
                    {
                        try
                        {
                            if (LoaiChenTin == "1") entity.LOAI_CHEN_TIN = 1;
                            else if (LoaiChenTin == "3") entity.LOAI_CHEN_TIN = 3;
                            else entity.LOAI_CHEN_TIN = 2;
                        }
                        catch { entity.LOAI_CHEN_TIN = null; }
                    }
                    else
                    {
                        res.MA_LOI = count_row;
                        res.TEN_LOI = error;
                        res.COT = localAPI.ExcelColumnNameToNumber("G") - 1;
                        res.DONG = count_row;
                        res.TEN_COT = "Loại chèn tin";
                        lstRes.Add(res);
                        resEntity.Res = false;
                        if (string.IsNullOrEmpty(resEntity.Msg))
                            resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                        else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                    }
                    #endregion
                    LopBO lopBO = new LopBO();
                    LOP lop = lopBO.getLopByTen(Sys_This_Truong.ID, Convert.ToInt16(Sys_Ma_Nam_hoc), Convert.ToInt16(KhoiHoc), TenLop);
                    bool is_update = lop != null;
                    if (lop == null) lop = new LOP();
                    lop.ID_NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
                    lop.ID_TRUONG = Sys_This_Truong.ID;
                    if (lstColumn.Contains("TEN"))
                    {
                        lop.TEN = entity.TEN.Trim();
                    }
                    if (lstColumn.Contains("ID_KHOI"))
                        lop.ID_KHOI = entity.ID_KHOI;
                    if (lstColumn.Contains("ID_GVCN"))
                        lop.ID_GVCN = entity.ID_GVCN;
                    if (lstColumn.Contains("THU_TU"))
                        lop.THU_TU = entity.THU_TU;
                    if (lstColumn.Contains("MA_LOAI_LOP_GDTX"))
                        lop.MA_LOAI_LOP_GDTX = entity.MA_LOAI_LOP_GDTX;
                    if (lstColumn.Contains("LOAI_CHEN_TIN"))
                        lop.LOAI_CHEN_TIN = entity.LOAI_CHEN_TIN;
                    if (lstColumn.Contains("TIEN_TO"))
                        lop.TIEN_TO = entity.LOAI_CHEN_TIN == 3 ? tien_to : "";
                    string strMsg = string.Empty;
                    if (resEntity.Res)
                    {
                        ResultEntity resEntity1 = new ResultEntity();
                        resEntity1.Res = true;
                        if (!is_update)
                            resEntity1 = lopBO.insert(lop, Sys_User.ID);
                        else
                            resEntity1 = lopBO.update(lop, Sys_User.ID);
                        if (resEntity1.Res) count_success++;
                        else count_error++;
                    }
                    endxuly:
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
                //btChiTiet.Visible = true;
                Session["ExcelLopDes" + Sys_User.ID] = res;
            }
        }
        protected void btTaiExcel_Click(object sender, EventArgs e)
        {
            string fileMau = "Template-LopHoc.xls";
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