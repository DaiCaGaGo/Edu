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

namespace CMS.Truong
{
    public partial class TruongImportExcel : AuthenticatePage
    {
        private TruongBO truongBO = new TruongBO();
        private LocalAPI localAPI = new LocalAPI();
        private DmTinhThanhBO dmTinhThanhBO = new DmTinhThanhBO();
        private DmQuanHuyenBO dmQuanHuyenBO = new DmQuanHuyenBO();
        private CPTelCoBO telcoBO = new CPTelCoBO();
        private GoiTinBO goiTinBO = new GoiTinBO();
        List<string> lstValue = new List<string> { "MA", "TEN", "IS_MN", "IS_TH", "IS_THCS", "IS_THPT", "IS_GDTX", "MA_TINH_THANH", "MA_QUAN_HUYEN", "IS_ACTIVE_SMS", "BRAND_NAME_VIETTEL", "CP_VIETTEL", "BRAND_NAME_VINA", "CP_VINA", "BRAND_NAME_MOBI", "CP_MOBI", "BRAND_NAME_GTEL", "CP_GTEL", "BRAND_NAME_VNM", "CP_VNM", "MA_GOI_TIN" };
        List<string> lstText = new List<string> { "Mã trường", "Tên trường", "Mầm non", "Tiểu học", "THCS", "THPT", "GDTX", "Tỉnh/Thành phố", "Quận/Huyện", "Đăng ký SMS", "BrandName Viettel", "Đối tác Viettel", "BrandName Vinaphone", "Đối tác Vinaphone", "BrandName Mobiphone", "Đối tác Mobiphone", "BrandName Gtel", "Đối tác Gtel", "BrandName VietnamMobile", "Đối tác VietnamMobile", "Gói tin" };
        protected void Page_Load(object sender, EventArgs e)
        {
            bt_EXCELtoSQL.Visible = is_access(SYS_Type_Access.THEM);
            btnUpload.Visible = is_access(SYS_Type_Access.THEM);
            if (!IsPostBack)
            {
                Session["ExcelTruong" + Sys_User.ID] = null;
                Session["ExcelTruongMessageId" + Sys_User.ID] = null;
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
            if (Session["ExcelTruong" + Sys_User.ID] != null)
            {
                dt = (DataTable)Session["ExcelTruong" + Sys_User.ID];
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
                if (Session["ExcelTruong" + Sys_User.ID] != null)
                {
                    DataTable dt = (DataTable)Session["ExcelTruong" + Sys_User.ID];
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
                        else if (dt.Columns.Count < localAPI.ExcelColumnNameToNumber("V"))
                        {
                            for (int i = dt.Columns.Count - 1; i > localAPI.ExcelColumnNameToNumber("V"); i--)
                                dt.Columns.RemoveAt(i);
                        }
                        Session["ExcelTruong" + Sys_User.ID] = dt;
                        Session["ExcelTruongMessageId" + Sys_User.ID] = message_id;

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
                // Response.Write(ex.ToString());
                if (ex.ErrorCode == -2147467259)
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn phải đặt tên sheet dữ liệu là DuLieu hoặc định dạng dữ liệu không đúng!');", true);
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
                //RadComboBox rcbCPViettel = (RadComboBox)e.Item.FindControl("rcbCPViettel");
                //RadComboBox rcbCPVina = (RadComboBox)e.Item.FindControl("rcbCPVina");
                //RadComboBox rcbCPMobi = (RadComboBox)e.Item.FindControl("rcbCPMobi");
                //RadComboBox rcbCPGtel = (RadComboBox)e.Item.FindControl("rcbCPGtel");
                //RadComboBox rcbCPVNM = (RadComboBox)e.Item.FindControl("rcbCPVNM");
                CheckBox chkMN = (CheckBox)e.Item.FindControl("cbMamNon");
                HiddenField hdMN = (HiddenField)e.Item.FindControl("hdMamNon");
                CheckBox chkTH = (CheckBox)e.Item.FindControl("cbTH");
                HiddenField hdTH = (HiddenField)e.Item.FindControl("hdTH");
                CheckBox chkTHCS = (CheckBox)e.Item.FindControl("cbTHCS");
                HiddenField hdTHCS = (HiddenField)e.Item.FindControl("hdTHCS");
                CheckBox chkTHPT = (CheckBox)e.Item.FindControl("cbTHPT");
                HiddenField hdTHPT = (HiddenField)e.Item.FindControl("hdTHPT");
                CheckBox chkGDTX = (CheckBox)e.Item.FindControl("cbGDTX");
                HiddenField hdGDTX = (HiddenField)e.Item.FindControl("hdGDTX");
                CheckBox chkDangKySMS = (CheckBox)e.Item.FindControl("cbDangKySMS");
                HiddenField hdDangKySMS = (HiddenField)e.Item.FindControl("hdDangKySMS");
                System.Web.UI.HtmlControls.HtmlImage error1 = (System.Web.UI.HtmlControls.HtmlImage)item.FindControl("iconErr");
                System.Web.UI.HtmlControls.HtmlImage success1 = (System.Web.UI.HtmlControls.HtmlImage)item.FindControl("iconSuccess");
                error1.Visible = false;
                success1.Visible = false;

                #region checkbox
                string strMN = hdMN.Value.Trim();
                string strTH = hdTH.Value.Trim();
                string strTHCS = hdTHCS.Value.Trim();
                string strTHPT = hdTHPT.Value.Trim();
                string strGDTX = hdGDTX.Value.Trim();
                string strDangKySMS = hdDangKySMS.Value.Trim();
                if (strMN == "1") chkMN.Checked = true;
                if (strTH == "1") chkTH.Checked = true;
                if (strTHCS == "1") chkTHCS.Checked = true;
                if (strTHPT == "1") chkTHPT.Checked = true;
                if (strGDTX == "1") chkGDTX.Checked = true;
                if (strDangKySMS == "1") chkDangKySMS.Checked = true;
                #endregion

                //#region Trang thai
                //string strTrangThai = hdTrangThai.Value.ToNormalizeLowerRelace();
                //if (!string.IsNullOrEmpty(strTrangThai))
                //{
                //    List<TRANG_THAI_GV> lstTrangThai = trangThaiGVBO.getTrangThaiGV();
                //    TRANG_THAI_GV trangThaiGV = lstTrangThai.FirstOrDefault(x => x.TEN.ToNormalizeLowerRelace() == strTrangThai);
                //    if (trangThaiGV != null)
                //    {
                //        rcbTrangThaiItem.SelectedValue = trangThaiGV.MA.ToString();
                //    }
                //}
                //#endregion
            }
        }
        protected void bt_EXCELtoSQL_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.THEM))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            if (Session["ExcelTruong" + Sys_User.ID] != null && Session["ExcelTruongMessageId" + Sys_User.ID] != null)
            {
                List<ResultError> lstRes = new List<ResultError>();
                List<ResultEntity> lstResEntity = new List<ResultEntity>();
                ResultError res = new ResultError();
                CommonValidate validate = new CommonValidate();

                DataTable dt = new DataTable();
                dt = (DataTable)Session["ExcelTruong" + Sys_User.ID];
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

                List<DM_TINH_THANH> lstTinh = dmTinhThanhBO.getTinhThanh();
                List<CP_TELCO> lstTelco = telcoBO.getTelco();
                List<GOI_TIN> lstGoiTin = goiTinBO.getGoiTin();
                foreach (GridDataItem row in RadGrid1.Items)
                {
                    ResultEntity resEntity = new ResultEntity();
                    resEntity.Res = true;
                    count_row++;
                    count_total++;
                    object val = new object();
                    string error = string.Empty;
                    TRUONG entity = new TRUONG();

                    TextBox tbStt = (TextBox)(TextBox)row.FindControl("tbSTT");
                    TextBox tbMa = (TextBox)row.FindControl("tbMa");
                    TextBox tbTen = (TextBox)row.FindControl("tbTen");
                    CheckBox cbMamNon = (CheckBox)row.FindControl("cbMamNon");
                    CheckBox cbTH = (CheckBox)row.FindControl("cbTH");
                    CheckBox cbTHCS = (CheckBox)row.FindControl("cbTHCS");
                    CheckBox cbTHPT = (CheckBox)row.FindControl("cbTHPT");
                    CheckBox cbGDTX = (CheckBox)row.FindControl("cbGDTX");
                    CheckBox cbDangKySMS = (CheckBox)row.FindControl("cbDangKySMS");
                    TextBox tbTinhThanh = (TextBox)row.FindControl("tbTinhThanh");
                    TextBox tbQuanHuyen = (TextBox)row.FindControl("tbQuanHuyen");
                    TextBox tbBrViettel = (TextBox)row.FindControl("tbBrViettel");
                    TextBox tbCPViettel = (TextBox)row.FindControl("tbCPViettel");
                    TextBox tbBrVina = (TextBox)row.FindControl("tbBrVina");
                    TextBox tbCPVina = (TextBox)row.FindControl("tbCPVina");
                    TextBox tbBrMobi = (TextBox)row.FindControl("tbBrMobi");
                    TextBox tbCPMobi = (TextBox)row.FindControl("tbCPMobi");
                    TextBox tbBrGtel = (TextBox)row.FindControl("tbBrGtel");
                    TextBox tbCPGtel = (TextBox)row.FindControl("tbCPGtel");
                    TextBox tbBrVNM = (TextBox)row.FindControl("tbBrVNM");
                    TextBox tbCPVNM = (TextBox)row.FindControl("tbCPVNM");
                    TextBox tbGoiTin = (TextBox)row.FindControl("tbGoiTin");
                    HiddenField hdresMsg = (HiddenField)row.FindControl("hdresMsg");
                    System.Web.UI.HtmlControls.HtmlImage icError = (System.Web.UI.HtmlControls.HtmlImage)row.FindControl("iconErr");
                    System.Web.UI.HtmlControls.HtmlImage icSuccess = (System.Web.UI.HtmlControls.HtmlImage)row.FindControl("iconSuccess");
                    icError.Visible = false;
                    icSuccess.Visible = false;

                    long id = Convert.ToInt64(tbStt.Text.Trim());
                    string ma = (tbMa != null) ? tbMa.Text.Trim() : "";
                    string ten = (tbTen != null) ? tbTen.Text.Trim() : "";
                    bool chkMN = (cbMamNon.Checked) ? true : false;
                    bool chkTH = (cbTH.Checked) ? true : false;
                    bool chkTHCS = (cbTHCS.Checked) ? true : false;
                    bool chkTHPT = (cbTHPT.Checked) ? true : false;
                    bool chkGDTX = (cbGDTX.Checked) ? true : false;
                    bool chkDangKySMS = (cbDangKySMS.Checked) ? true : false;
                    string tinh = (tbTinhThanh != null) ? tbTinhThanh.Text.Trim() : "";
                    string quanHuyen = (tbQuanHuyen != null) ? tbQuanHuyen.Text.Trim() : "";
                    string brViettel = (tbBrViettel != null) ? tbBrViettel.Text.Trim() : "";
                    string cpViettel = (tbCPViettel != null) ? tbCPViettel.Text.Trim() : "";
                    string brVina = (tbBrVina != null) ? tbBrVina.Text.Trim() : "";
                    string cpVina = (tbCPVina != null) ? tbCPVina.Text.Trim() : "";
                    string brMobi = (tbBrMobi != null) ? tbBrMobi.Text.Trim() : "";
                    string cpMobi = (tbCPMobi != null) ? tbCPMobi.Text.Trim() : "";
                    string brGtel = (tbBrGtel != null) ? tbBrGtel.Text.Trim() : "";
                    string cpGtel = (tbCPGtel != null) ? tbCPGtel.Text.Trim() : "";
                    string brVNM = (tbBrVNM != null) ? tbBrVNM.Text.Trim() : "";
                    string cpVNM = (tbCPVNM != null) ? tbCPVNM.Text.Trim() : "";
                    string goiTin = (tbGoiTin != null) ? tbGoiTin.Text.Trim() : "";

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

                    // check mã trường
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
                        res.TEN_COT = "Mã trường";
                        lstRes.Add(res);
                        resEntity.Res = false;
                        if (string.IsNullOrEmpty(resEntity.Msg))
                            resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                        else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                    }

                    // check tên trường
                    if (validate.ValString(true, null, ma, out val, out error))
                    {
                        entity.TEN = ten;
                    }
                    else
                    {
                        res.MA_LOI = count_row;
                        res.TEN_LOI = error;
                        res.COT = localAPI.ExcelColumnNameToNumber("C") - 1;
                        res.DONG = count_row;
                        res.TEN_COT = "Tên trường";
                        lstRes.Add(res);
                        resEntity.Res = false;
                        if (string.IsNullOrEmpty(resEntity.Msg))
                            resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                        else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                    }

                    // check cấp học
                    if (!chkMN && !chkTH && !chkTHCS && !chkTHPT && !chkGDTX)
                    {
                        res.MA_LOI = count_row;
                        res.TEN_LOI = " Bắt buộc";
                        res.DONG = count_row;
                        res.TEN_COT = "Cấp học";
                        lstRes.Add(res);
                        resEntity.Res = false;
                        if (string.IsNullOrEmpty(resEntity.Msg))
                            resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                        else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                    }
                    else
                    {
                        entity.IS_MN = (chkMN) ? true : false;
                        entity.IS_TH = (chkTH) ? true : false;
                        entity.IS_THCS = (chkTHCS) ? true : false;
                        entity.IS_THPT = (chkTHPT) ? true : false;
                        entity.IS_GDTX = (chkGDTX) ? true : false;
                    }

                    entity.IS_ACTIVE_SMS = (chkDangKySMS) ? true : false;

                    #region Tỉnh thành
                    short strMaTinh = 0;
                    string strTinh = tinh.ToNormalizeLowerRelace();
                    if (!string.IsNullOrEmpty(strTinh))
                    {
                        if (lstTinh != null && !string.IsNullOrEmpty(strTinh))
                        {
                            DM_TINH_THANH dmTinh = lstTinh.FirstOrDefault(x => x.TEN.ToNormalizeLowerRelace().Contains(strTinh));
                            if (dmTinh != null)
                                entity.MA_TINH_THANH = strMaTinh = dmTinh.MA;
                        }
                    }
                    #endregion

                    #region Quận/Huyện
                    string strQuanHuyen = quanHuyen.ToNormalizeLowerRelace();
                    if (strMaTinh != 0)
                    {
                        List<DM_QUAN_HUYEN> lstQuanHuyen = dmQuanHuyenBO.getQuanHuyenByTinh(strMaTinh);
                        if (lstQuanHuyen != null && !string.IsNullOrEmpty(strQuanHuyen))
                        {
                            DM_QUAN_HUYEN dmQuanHuyen = lstQuanHuyen.FirstOrDefault(x => x.TEN.ToNormalizeLowerRelace().Contains(strQuanHuyen));
                            if (dmQuanHuyen != null)
                                entity.MA_QUAN_HUYEN = dmQuanHuyen.MA;
                        }
                    }
                    #endregion

                    entity.BRAND_NAME_VIETTEL = brViettel;
                    entity.BRAND_NAME_VINA = brVina;
                    entity.BRAND_NAME_MOBI = brMobi;
                    entity.BRAND_NAME_GTEL = brGtel;
                    entity.BRAND_NAME_VNM = brVNM;
                    entity.CP_VIETTEL = cpViettel;
                    entity.CP_VINA = cpVina;
                    entity.CP_MOBI = cpMobi;
                    entity.CP_GTEL = cpGtel;
                    entity.CP_VNM = cpVNM;

                    // gói tin
                    string strGoiTin = tbGoiTin.Text.ToNormalizeLowerRelace();
                    if (!string.IsNullOrEmpty(strGoiTin))
                    {
                        if (lstGoiTin != null)
                        {
                            GOI_TIN dmGoiTin = lstGoiTin.FirstOrDefault(x => x.TEN.ToNormalizeLowerRelace() == strGoiTin);
                            if (dmGoiTin != null)
                                entity.MA_GOI_TIN = dmGoiTin.MA;
                        }
                    }

                    TRUONG truong = truongBO.getTruongByMa(entity.MA);
                    bool is_update = truong != null;
                    if (truong == null) truong = new TRUONG();
                    if (lstColumn.Contains("MA"))
                        truong.MA = entity.MA;
                    if (lstColumn.Contains("TEN"))
                        truong.TEN = entity.TEN;
                    if (lstColumn.Contains("IS_MN") && entity.IS_MN == true)
                        truong.IS_MN = entity.IS_MN;
                    if (lstColumn.Contains("IS_TH") && entity.IS_TH == true)
                        truong.IS_TH = entity.IS_TH;
                    if (lstColumn.Contains("IS_THCS") && entity.IS_THCS == true)
                        truong.IS_THCS = entity.IS_THCS;
                    if (lstColumn.Contains("IS_THPT") && entity.IS_THPT == true)
                        truong.IS_THPT = entity.IS_THPT;
                    if (lstColumn.Contains("IS_GDTX") && entity.IS_GDTX == true)
                        truong.IS_GDTX = entity.IS_GDTX;
                    if (lstColumn.Contains("IS_ACTIVE_SMS") && entity.IS_ACTIVE_SMS == true)
                        truong.IS_ACTIVE_SMS = entity.IS_ACTIVE_SMS;
                    if (lstColumn.Contains("DM_TINH_THANH"))
                        truong.DM_TINH_THANH = entity.DM_TINH_THANH;
                    if (lstColumn.Contains("DM_QUAN_HUYEN"))
                        truong.DM_QUAN_HUYEN = entity.DM_QUAN_HUYEN;
                    if (lstColumn.Contains("BRAND_NAME_VIETTEL"))
                        truong.BRAND_NAME_VIETTEL = entity.BRAND_NAME_VIETTEL;
                    if (lstColumn.Contains("CP_VIETTEL"))
                        truong.CP_VIETTEL = entity.CP_VIETTEL;
                    if (lstColumn.Contains("BRAND_NAME_VINA"))
                        truong.BRAND_NAME_VINA = entity.BRAND_NAME_VINA;
                    if (lstColumn.Contains("CP_VINA"))
                        truong.CP_VINA = entity.CP_VINA;
                    if (lstColumn.Contains("BRAND_NAME_MOBI"))
                        truong.BRAND_NAME_MOBI = entity.BRAND_NAME_MOBI;
                    if (lstColumn.Contains("CP_MOBI"))
                        truong.CP_MOBI = entity.CP_MOBI;
                    if (lstColumn.Contains("BRAND_NAME_GTEL"))
                        truong.BRAND_NAME_GTEL = entity.BRAND_NAME_GTEL;
                    if (lstColumn.Contains("CP_GTEL"))
                        truong.CP_GTEL = entity.CP_GTEL;
                    if (lstColumn.Contains("BRAND_NAME_VNM"))
                        truong.BRAND_NAME_VNM = entity.BRAND_NAME_VNM;
                    if (lstColumn.Contains("CP_VNM"))
                        truong.CP_VNM = entity.CP_VNM;
                    if (lstColumn.Contains("MA_GOI_TIN"))
                        truong.MA_GOI_TIN = entity.MA_GOI_TIN;
                    truong.TRANG_THAI = true;

                    string strMsg = string.Empty;
                    if (resEntity.Res)
                    {
                        if (!is_update)
                        {
                            resEntity = truongBO.insert(truong, Sys_User.ID);
                        }
                        else
                        {
                            resEntity = truongBO.update(truong, Sys_User.ID);
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
                }

                if (res.MA_LOI == 0)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('success', 'Cập nhật thành công " + count_success + "/" + count_total + " bản ghi');", true);
                }
                else
                {
                    if (count_success > 0)
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Cập nhật thành công " + count_success + "/" + count_total + " bản ghi. Vui lòng kiểm tra thông tin các bản ghi chưa cập nhật trong bảng Chi tiết nhập liệu');", true);
                    else
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Cập nhật không thành công. Vui lòng kiểm tra thông tin chưa cập nhật trong bảng Chi tiết nhập liệu');", true);
                }
                //btChiTiet.Visible = true;
                Session["ExcelTruongDes" + Sys_User.ID] = res;
            }
        }
        protected void btTaiExcel_Click(object sender, EventArgs e)
        {
            string fileMau = "Template-Truong.xls";

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