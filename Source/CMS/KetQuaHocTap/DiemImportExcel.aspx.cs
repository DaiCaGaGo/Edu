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

namespace CMS.KetQuaHocTap
{
    public partial class DiemImportExcel : AuthenticatePage
    {
        private KhoiBO khoiBO = new KhoiBO();
        private LopBO lopBO = new LopBO();
        private HocSinhBO hocSinhBO = new HocSinhBO();
        private LocalAPI localAPI = new LocalAPI();
        MonHocTruongBO monHocTruongBO = new MonHocTruongBO();
        LogUserBO logUserBO = new LogUserBO();
        DiemChiTietBO diemChiTietBO = new DiemChiTietBO();
        LopMonBO lopMonBO = new LopMonBO();
        private List<string> lstTitle = new List<string>();
        public DataAccessAPI dataAccessAPI = new DataAccessAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Sys_This_Truong == null && Sys_This_Cap_Hoc == null)
                checkChonTruong();
            btCapNhat.Visible = is_access(SYS_Type_Access.THEM);
            if (!IsPostBack)
            {
                rcbHocKy.DataBind();
                rcbHocKy.SelectedValue = Sys_Hoc_Ky.ToString();
                rcbLoaiDiem.DataBind();
                rcbLoaiDiem.SelectedValue = "1";
                Session["ExcelNhapDiem" + Sys_User.ID] = null;
                Session["ExcelNhapDiemMessageId" + Sys_User.ID] = null;
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            if (Session["ExcelNhapDiem" + Sys_User.ID] != null)
            {
                dt = (DataTable)Session["ExcelNhapDiem" + Sys_User.ID];
            }

            if (Session["ExcelColumnTitleDiem" + Sys_User.ID] != null)
            {
                lstTitle = (List<string>)Session["ExcelColumnTitleDiem" + Sys_User.ID];
            }

            for (int i = 0; i < 20; i++)
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
            for (int i = count_true; i < 20; i++)
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

                        if (dt.Columns.Count < localAPI.ExcelColumnNameToNumber("U"))
                        {
                            for (int i = dt.Columns.Count - 1; i > localAPI.ExcelColumnNameToNumber("U"); i--)
                                dt.Columns.RemoveAt(i);
                        }
                        Session["ExcelNhapDiem" + Sys_User.ID] = dt;
                        Session["ExcelNhapDiemMessageId" + Sys_User.ID] = message_id;

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

                #region Get tên cột (max = 20 cột)
                int max_cot = myDataTable.Columns.Count;
                if (max_cot > 20) max_cot = 20;
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
                if (lstTitle.Count < 5)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'File dữ liệu chưa đúng. Vui lòng kiểm tra lại!');", true);
                    return null;
                }
                Session["ExcelColumnTitleDiem" + Sys_User.ID] = lstTitle;
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
                    if (lstTitle[1].ToNormalizeLowerRelace() != "Mã HS".ToNormalizeLowerRelace())
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Cột thứ hai phải có tên 'Mã HS'!');", true);
                        return null;
                    }
                    if (lstTitle[2].ToNormalizeLowerRelace() != "Họ và tên".ToNormalizeLowerRelace())
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Cột thứ ba phải có tên 'Họ và tên'!');", true);
                        return null;
                    }
                    if (lstTitle[3].ToNormalizeLowerRelace() != "Lớp".ToNormalizeLowerRelace())
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Cột thứ tứ phải có tên 'Lớp'!');", true);
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
                    if (string.IsNullOrEmpty(row[0].ToString().Trim()) || string.IsNullOrEmpty(row[1].ToString().Trim()) || string.IsNullOrEmpty(row[2].ToString().Trim()) || string.IsNullOrEmpty(row[3].ToString().Trim()))
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
                if (count > 0) ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Có " + count + " dòng dữ liệu chưa thỏa mãn!');", true);
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
                Label lblCOT16 = (Label)item.FindControl("lblCOT16");
                Label lblCOT17 = (Label)item.FindControl("lblCOT17");
                Label lblCOT18 = (Label)item.FindControl("lblCOT18");
                Label lblCOT19 = (Label)item.FindControl("lblCOT19");
                Label lblCOT20 = (Label)item.FindControl("lblCOT20");

                int max_cot = lstTitle.Count;
                if (lstTitle.Count > 0)
                {
                    lblCOT1.Text = lstTitle[0];
                    lblCOT2.Text = lstTitle[1];
                    lblCOT3.Text = lstTitle[2];
                    lblCOT4.Text = lstTitle[3];
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
                    if (lstTitle.Count > 15) lblCOT16.Text = lstTitle[15];
                    if (lstTitle.Count > 16) lblCOT17.Text = lstTitle[16];
                    if (lstTitle.Count > 17) lblCOT18.Text = lstTitle[17];
                    if (lstTitle.Count > 18) lblCOT19.Text = lstTitle[18];
                    if (lstTitle.Count > 19) lblCOT20.Text = lstTitle[19];
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
            string fileMau = "Template-Diem.xls";

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
                if (Session["ExcelNhapDiem" + Sys_User.ID] != null)
                {
                    DataTable dt = (DataTable)Session["ExcelNhapDiem" + Sys_User.ID];
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

            lstTitle = (List<string>)Session["ExcelColumnTitleDiem" + Sys_User.ID];

            if (Session["ExcelNhapDiem" + Sys_User.ID] != null && Session["ExcelNhapDiemMessageId" + Sys_User.ID] != null)
            {
                int countColumn = 4;
                int count_success = 0;

                short? loaiDiem = localAPI.ConvertStringToShort(rcbLoaiDiem.SelectedValue); //1: điểm học kỳ, 2: 1T HS1, 3: 1T HS2, 4: 15P, 5: Miệng
                short? hoc_ky = localAPI.ConvertStringToShort(rcbHocKy.SelectedValue);
                if (loaiDiem == null || hoc_ky == null)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Học kỳ và loại điểm không được để trống.');", true);
                    return;
                }
                foreach (GridColumn item in RadGrid1.MasterTableView.Columns)
                {
                    if (localAPI.checkColumnShowInRadGrid(RadGrid1, "COT5") && item.UniqueName == "COT5") countColumn++;
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
                    else if (localAPI.checkColumnShowInRadGrid(RadGrid1, "COT16") && item.UniqueName == "COT16") countColumn++;
                    else if (localAPI.checkColumnShowInRadGrid(RadGrid1, "COT17") && item.UniqueName == "COT17") countColumn++;
                    else if (localAPI.checkColumnShowInRadGrid(RadGrid1, "COT18") && item.UniqueName == "COT18") countColumn++;
                    else if (localAPI.checkColumnShowInRadGrid(RadGrid1, "COT19") && item.UniqueName == "COT19") countColumn++;
                    else if (localAPI.checkColumnShowInRadGrid(RadGrid1, "COT20") && item.UniqueName == "COT20") countColumn++;
                }
                if (countColumn > 4)
                {
                    List<MON_HOC_TRUONG> listMonHoc = monHocTruongBO.getMonHocByTruongAndMaCapHoc(Sys_This_Truong.ID, Sys_This_Cap_Hoc, (Int16)Sys_Ma_Nam_hoc);

                    foreach (GridDataItem row in RadGrid1.Items)
                    {
                        string error = string.Empty;
                        ResultEntity resEntity = new ResultEntity();
                        string message = "";

                        #region get control
                        TextBox tbLop = (TextBox)row.FindControl("tbCOT4");
                        HiddenField hdresMsg = (HiddenField)row.FindControl("hdresMsg");
                        System.Web.UI.HtmlControls.HtmlImage icError = (System.Web.UI.HtmlControls.HtmlImage)row.FindControl("iconErr");
                        System.Web.UI.HtmlControls.HtmlImage icSuccess = (System.Web.UI.HtmlControls.HtmlImage)row.FindControl("iconSuccess");
                        icError.Visible = false;
                        icSuccess.Visible = false;
                        #endregion

                        TextBox tbMaHS = (TextBox)row.FindControl("tbCOT2");
                        long? id_hs = null;
                        long? id_lop = null;
                        HOC_SINH hs = hocSinhBO.getHocSinhByMaAndNamHoc(tbMaHS.Text.Trim(), (Int16)Sys_Ma_Nam_hoc);
                        if (hs != null)
                        {
                            id_hs = hs.ID;
                            id_lop = hs.ID_LOP;
                            int count_empty = 0;
                            for (int i = 5; i <= countColumn; i++)
                            {
                                bool is_update = true;
                                short? idMonHoc = null;
                                long? idMonTruong = null;
                                string tenMon = lstTitle[i - 1];
                                TextBox tbDiem = (TextBox)row.FindControl("tbCOT" + i);
                                if (!string.IsNullOrEmpty(tenMon))
                                {
                                    MON_HOC_TRUONG monTruong = listMonHoc.Where(x => x.TEN.ToLower() == tenMon.ToLower()).FirstOrDefault();
                                    if (monTruong != null)
                                    {
                                        idMonHoc = monTruong.ID_MON_HOC;
                                        idMonTruong = monTruong.ID;

                                        LOP_MON monLop = lopMonBO.getLopMonByLopMonHocKy(id_lop.Value, idMonTruong.Value, hoc_ky.Value);
                                        if (monLop != null)
                                        {
                                            if (!string.IsNullOrEmpty(tbDiem.Text.Trim()))
                                            {
                                                Decimal? diemTmp = monTruong.KIEU_MON == false ? localAPI.ConvertStringToDecimal(tbDiem.Text.Trim()) : dataAccessAPI.ConvertCDToDecimal(tbDiem.Text.Trim());

                                                DIEM_CHI_TIET diemChiTiet = diemChiTietBO.getDiemMonHocSinh(Sys_This_Truong.ID, (Int16)Sys_Ma_Nam_hoc, id_lop.Value, hoc_ky.Value, idMonTruong.Value, id_hs.Value);
                                                if (diemChiTiet == null)
                                                {
                                                    is_update = false;
                                                    diemChiTiet = new DIEM_CHI_TIET();
                                                }
                                                diemChiTiet.ID_TRUONG = Sys_This_Truong.ID;
                                                diemChiTiet.ID_NAM_HOC = (Int16)Sys_Ma_Nam_hoc;
                                                diemChiTiet.MA_KHOI = hs.ID_KHOI;
                                                diemChiTiet.ID_HOC_SINH = id_hs.Value;
                                                diemChiTiet.ID_LOP = id_lop.Value;
                                                diemChiTiet.HOC_KY = hoc_ky.Value;
                                                diemChiTiet.ID_MON_HOC = idMonHoc;
                                                diemChiTiet.ID_MON_HOC_TRUONG = idMonTruong.Value;

                                                if (loaiDiem == 1) //diem hoc ky
                                                    diemChiTiet.DIEM_HOC_KY = diemTmp;
                                                else if (loaiDiem == 2) // diem 1 tiet HS1
                                                {
                                                    if (diemChiTiet.DIEM11 == null) diemChiTiet.DIEM11 = diemTmp;
                                                    else if (diemChiTiet.DIEM12 == null) diemChiTiet.DIEM12 = diemTmp;
                                                    else if (diemChiTiet.DIEM13 == null) diemChiTiet.DIEM13 = diemTmp;
                                                    else if (diemChiTiet.DIEM14 == null) diemChiTiet.DIEM14 = diemTmp;
                                                    else if (diemChiTiet.DIEM15 == null) diemChiTiet.DIEM15 = diemTmp;
                                                }
                                                else if (loaiDiem == 3) // diem 1 tiet HS2
                                                {
                                                    if (diemChiTiet.DIEM16 == null) diemChiTiet.DIEM16 = diemTmp;
                                                    else if (diemChiTiet.DIEM17 == null) diemChiTiet.DIEM17 = diemTmp;
                                                    else if (diemChiTiet.DIEM18 == null) diemChiTiet.DIEM18 = diemTmp;
                                                    else if (diemChiTiet.DIEM19 == null) diemChiTiet.DIEM19 = diemTmp;
                                                    else if (diemChiTiet.DIEM20 == null) diemChiTiet.DIEM20 = diemTmp;
                                                }
                                                else if (loaiDiem == 4) // diem 15P
                                                {
                                                    if (diemChiTiet.DIEM6 == null) diemChiTiet.DIEM6 = diemTmp;
                                                    else if (diemChiTiet.DIEM7 == null) diemChiTiet.DIEM7 = diemTmp;
                                                    else if (diemChiTiet.DIEM8 == null) diemChiTiet.DIEM8 = diemTmp;
                                                    else if (diemChiTiet.DIEM9 == null) diemChiTiet.DIEM9 = diemTmp;
                                                    else if (diemChiTiet.DIEM10 == null) diemChiTiet.DIEM10 = diemTmp;
                                                }
                                                else if (loaiDiem == 5) //diem mieng
                                                {
                                                    if (diemChiTiet.DIEM1 == null) diemChiTiet.DIEM1 = diemTmp;
                                                    else if (diemChiTiet.DIEM2 == null) diemChiTiet.DIEM2 = diemTmp;
                                                    else if (diemChiTiet.DIEM3 == null) diemChiTiet.DIEM3 = diemTmp;
                                                    else if (diemChiTiet.DIEM4 == null) diemChiTiet.DIEM4 = diemTmp;
                                                    else if (diemChiTiet.DIEM5 == null) diemChiTiet.DIEM5 = diemTmp;
                                                }
                                                ResultEntity result = new ResultEntity();
                                                if (is_update) result = diemChiTietBO.update(diemChiTiet, Sys_User);
                                                else result = diemChiTietBO.insert(diemChiTiet, Sys_User.ID);
                                                if (result.Res) count_success++;
                                                else message += "Lỗi cập nhật " + tenMon + "; ";
                                            }
                                        }
                                        else
                                        {
                                            message += tenMon + " không có trong lớp " + tbLop.Text.Trim() + "; ";
                                            count_empty++;
                                        }
                                    }
                                    else
                                    {
                                        message += tenMon + " không có trong lớp " + tbLop.Text.Trim() + "; ";
                                        count_empty++;
                                    }
                                }
                            }
                            if (count_empty > 0)
                            {
                                resEntity.Res = false;
                                resEntity.Msg = message;
                                hdresMsg.Value = resEntity.Msg;
                                row.ForeColor = Color.Red;
                                icError.Visible = true;
                                continue;
                            }
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Không có dữ liệu cần cập nhật. Vui lòng kiểm tra lại');", true);
                    return;
                }

                if (count_success > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('success', 'Có " + count_success + " điểm được cập nhật');", true);
                }
            }
        }
    }
}