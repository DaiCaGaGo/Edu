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

namespace CMS.HocPhi
{
    public partial class ReportPhieuThuHocSinh : AuthenticatePage
    {
        private KhoiBO khoiBO = new KhoiBO();
        private LopBO lopBO = new LopBO();
        private HocSinhBO hocSinhBO = new HocSinhBO();
        private LocalAPI localAPI = new LocalAPI();
        LogUserBO logUserBO = new LogUserBO();
        List<string> lstValue = new List<string> { "MA", "HO_TEN", "ID_KHOI", "ID_LOP", "NGAY_SINH", "NOI_DUNG", "SO_TIEN" };
        List<string> lstText = new List<string> { "Mã HS", "Họ và tên", "Khối", "Lớp", "Ngày sinh", "Nội dung thu tiền", "Số tiền thu (VNĐ)" };
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            bt_EXCELtoSQL.Visible = is_access(SYS_Type_Access.THEM);
            if (!IsPostBack)
            {
                objKhoi.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                objKhoi.SelectParameters.Add("maLoaiLopGDTX", DbType.Int16, Sys_This_Lop_GDTX == null ? "" : Sys_This_Lop_GDTX.ToString());
                rcbKhoi.DataBind();
                objLop.SelectParameters.Add("ma_cap_hoc", Sys_This_Cap_Hoc);
                objLop.SelectParameters.Add("idTruong", Sys_This_Truong.ID.ToString());
                objLop.SelectParameters.Add("maNamHoc", Sys_Ma_Nam_hoc.ToString());
                rcbLop.DataBind(); rcbColumn.Items.Clear();
                for (int i = 0; i < lstValue.Count; i++)
                {
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Value = lstValue[i];
                    item.Text = lstText[i];
                    item.Checked = true;
                    rcbColumn.Items.Add(item);
                }
                Session["ExcelPhieuThuHocSinh" + Sys_User.ID] = null;
                Session["ExcelPhieuThuHocSinhMessageId" + Sys_User.ID] = null;
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            if (Session["ExcelPhieuThuHocSinh" + Sys_User.ID] != null)
            {
                dt = (DataTable)Session["ExcelPhieuThuHocSinh" + Sys_User.ID];
            }

            if (dt != null && dt.Columns.Count > 0)
                RadGrid1.DataSource = dt;

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
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
                if (Session["ExcelPhieuThuHocSinh" + Sys_User.ID] != null)
                {
                    DataTable dt = (DataTable)Session["ExcelPhieuThuHocSinh" + Sys_User.ID];
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
                        if (dt == null || dt.Columns.Count < lstValue.Count + 2)
                        {
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'File chưa đúng mẫu, hoặc dữ liệu trống. Vui lòng kiểm tra lại!');", true);
                        }
                        else if (dt.Columns.Count < lstValue.Count + 2)
                        {
                            for (int i = dt.Columns.Count - 1; i > lstValue.Count + 2; i--)
                                dt.Columns.RemoveAt(i);
                        }
                        Session["ExcelPhieuThuHocSinh" + Sys_User.ID] = dt;
                        Session["ExcelPhieuThuHocSinhMessageId" + Sys_User.ID] = message_id;

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
                myDataTable.Columns[1].ColumnName = "MA";
                if (myDataTable.Columns.Count < lstValue.Count + 2)
                {
                    return null;
                }
                for (int i = 0; i <= lstValue.Count; i++)
                {
                    try
                    {
                        if (myDataTable.Columns[i + 2].ColumnName.ToNormalizeLowerRelace() == lstText[i].ToNormalizeLowerRelace())
                            myDataTable.Columns[i + 2].ColumnName = lstValue[i];
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
                            if (string.IsNullOrEmpty(row[2].ToString().Trim()))
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
                if (myDataTable.Rows.Count > 500)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Không upload file vượt quá 500 bản ghi!');", true);
                    return null;
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
                //Response.Write(ex.ToString());
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Có lỗi xảy ra!');", true);
            }
            return new DataTable();
        }
        protected void bt_EXCELtoSQL_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.THEM))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            if (Session["ExcelPhieuThuHocSinh" + Sys_User.ID] != null && Session["ExcelPhieuThuHocSinhMessageId" + Sys_User.ID] != null)
            {
                List<ResultError> lstRes = new List<ResultError>();
                List<ResultEntity> lstResEntity = new List<ResultEntity>();
                ResultError res = new ResultError();
                CommonValidate validate = new CommonValidate();

                DataTable dt = new DataTable();
                dt = (DataTable)Session["ExcelPhieuThuHocSinh" + Sys_User.ID];
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

                    #region get control
                    TextBox tbStt = (TextBox)row.FindControl("tbSTT");
                    TextBox tbMA = (TextBox)row.FindControl("tbMA");
                    TextBox tbHoTen = (TextBox)row.FindControl("tbHoTen");
                    TextBox tbKhoi = (TextBox)row.FindControl("tbIdKhoi");
                    TextBox tbLop = (TextBox)row.FindControl("tbIdLop");

                    TextBox tbNoiDung = (TextBox)row.FindControl("tbNoiDung");
                    TextBox tbNgaySinh = (TextBox)row.FindControl("tbNgaySinh");

                    HiddenField hdresMsg = (HiddenField)row.FindControl("hdresMsg");
                    System.Web.UI.HtmlControls.HtmlImage icError = (System.Web.UI.HtmlControls.HtmlImage)row.FindControl("iconErr");
                    System.Web.UI.HtmlControls.HtmlImage icSuccess = (System.Web.UI.HtmlControls.HtmlImage)row.FindControl("iconSuccess");
                    icError.Visible = false;
                    icSuccess.Visible = false;
                    #endregion

                    #region set value
                    string stt = tbStt.Text.Trim();
                    string maHS = tbMA.Text.Trim();
                    string strNoiDung = (tbNoiDung != null) ? tbNoiDung.Text.Trim() : "";
                    string ngaySinh = (tbNgaySinh != null) ? tbNgaySinh.Text : "";
                    string hoTen = (tbHoTen != null) ? tbHoTen.Text.Trim() : "";
                    #region Khối
                    short idKhoi = 0;
                    string strKhoi = tbKhoi.Text.ToNormalizeLowerRelace();
                    List<KHOI> lstKhoi = khoiBO.getKhoiByCapHoc(Sys_This_Cap_Hoc);
                    if (!string.IsNullOrEmpty(strKhoi) && lstKhoi != null)
                    {
                        KHOI khoi = lstKhoi.FirstOrDefault(x => x.TEN.ToNormalizeLowerRelace().Contains(strKhoi));
                        if (khoi != null)
                        {
                            idKhoi = khoi.MA;
                        }
                    }
                    #endregion
                    #region Lớp
                    long idLop = 0;
                    string strLop = tbLop.Text.ToNormalizeLowerRelace();
                    List<LopEntity> lstLop = lopBO.getLopByTruongCapHocAndKhoi(Sys_This_Cap_Hoc, Sys_This_Truong.ID, idKhoi, Convert.ToInt16(Sys_Ma_Nam_hoc), null);
                    if (!string.IsNullOrEmpty(strLop) && lstLop != null)
                    {
                        LOP lop = lstLop.FirstOrDefault(x => x.TEN.ToNormalizeLowerRelace().Contains(strLop));
                        if (lop != null)
                        {
                            idLop = lop.ID;
                        }
                    }
                    #endregion

                    #region đk bắt buộc
                    HP_PhieuThuHocSinhEntity entity = new HP_PhieuThuHocSinhEntity();
                    #region check STT excel
                    if (validate.ValInt(true, null, null, stt, out val, out error))
                    {
                        entity.STT = Convert.ToInt64(stt);
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
                    #region check mã
                    long? id_hoc_sinh = hocSinhBO.getHocSinhByKhoiLop(Sys_This_Truong.ID, entity.ID_KHOI, entity.ID_LOP, Convert.ToInt16(Sys_Ma_Nam_hoc), Sys_This_Cap_Hoc).Where(x => x.MA == entity.MA).FirstOrDefault().ID;
                    if (id_hoc_sinh != null && id_hoc_sinh > 0)
                    {
                        if (validate.ValString(true, 250, maHS, out val, out error))
                        {
                            entity.MA = maHS;
                        }
                        else
                        {
                            res.MA_LOI = count_row;
                            res.TEN_LOI = error;
                            res.COT = localAPI.ExcelColumnNameToNumber("B") - 1;
                            res.DONG = count_row;
                            res.TEN_COT = "MA";
                            lstRes.Add(res);
                            resEntity.Res = false;
                            if (!string.IsNullOrEmpty(resEntity.Msg))
                                resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                            else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                        }
                    }
                    else
                    {
                        res.MA_LOI = count_row;
                        res.TEN_LOI = "Mã HS không tồn tại";
                        res.COT = localAPI.ExcelColumnNameToNumber("B") - 1;
                        res.DONG = count_row;
                        res.TEN_COT = "MA";
                        lstRes.Add(res);
                        resEntity.Res = false;
                        if (!string.IsNullOrEmpty(resEntity.Msg))
                            resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                        else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                    }
                    #endregion
                    #region check khoi
                    if (validate.ValInt(true, null, null, idKhoi.ToString(), out val, out error) && idKhoi > 0)
                    {
                        entity.ID_KHOI = Convert.ToInt16(idKhoi);
                    }
                    else
                    {
                        res.MA_LOI = count_row;
                        res.TEN_LOI = error;
                        if (idKhoi == 0)
                            res.TEN_LOI = " chưa tồn tại trong hệ thống, vui lòng kiểm tra lại!";
                        res.COT = localAPI.ExcelColumnNameToNumber("D") - 1;
                        res.DONG = count_row;
                        res.TEN_COT = "Khối";
                        lstRes.Add(res);
                        resEntity.Res = false;
                        if (string.IsNullOrEmpty(resEntity.Msg))
                            resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                        else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                    }
                    #endregion
                    #region check lop
                    if (validate.ValInt(true, null, null, idLop.ToString(), out val, out error) && idLop > 0)
                    {
                        entity.ID_LOP = Convert.ToInt64(idLop);
                    }
                    else
                    {
                        res.MA_LOI = count_row;
                        res.TEN_LOI = error;
                        if (idLop == 0)
                            res.TEN_LOI = " chưa tồn tại trong hệ thống, vui lòng kiểm tra lại!";
                        res.COT = localAPI.ExcelColumnNameToNumber("E") - 1;
                        res.DONG = count_row;
                        res.TEN_COT = "Lớp";
                        lstRes.Add(res);
                        resEntity.Res = false;
                        if (string.IsNullOrEmpty(resEntity.Msg))
                            resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                        else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                    }
                    #endregion
                    #endregion
                    #region check Nội dung
                    if (string.IsNullOrEmpty(strNoiDung))
                        entity.NOI_DUNG = " ";
                    else
                    {
                        if (validate.ValidateSDT(false, strNoiDung, out out_sdt, out error))
                        {
                            entity.NOI_DUNG = out_sdt;
                        }
                        else
                        {
                            res.MA_LOI = count_row;
                            res.TEN_LOI = error;
                            res.COT = localAPI.ExcelColumnNameToNumber("G") - 1;
                            res.DONG = count_row;
                            res.TEN_COT = "Nội dung";
                            lstRes.Add(res);
                            resEntity.Res = false;
                            if (string.IsNullOrEmpty(resEntity.Msg))
                                resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                            else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                        }
                    }
                    #endregion 
                    #region check ho ten
                    if (validate.ValString(true, null, hoTen, out val, out error))
                    {
                        entity.HO_TEN = hoTen;
                    }
                    else
                    {
                        res.MA_LOI = count_row;
                        res.TEN_LOI = error;
                        res.COT = localAPI.ExcelColumnNameToNumber("C") - 1;
                        res.DONG = count_row;
                        res.TEN_COT = "Họ và tên";
                        lstRes.Add(res);
                        resEntity.Res = false;
                        if (string.IsNullOrEmpty(resEntity.Msg))
                            resEntity.Msg += res.TEN_COT + " " + res.TEN_LOI;
                        else resEntity.Msg += ", " + res.TEN_COT + " " + res.TEN_LOI;
                    }
                    #endregion
                    if (!string.IsNullOrEmpty(ngaySinh)) entity.NGAY_SINH = localAPI.ConvertDDMMYYToDateTime(ngaySinh);
                    #endregion
                    #region Set Entity
                    //List<string> lstSDT = new List<string>();
                    //if (!string.IsNullOrEmpty(entity.SDT_NHAN_TIN))
                    //    lstSDT.Add(entity.SDT_NHAN_TIN.Trim());
                    //if (!string.IsNullOrEmpty(entity.SDT_NHAN_TIN2))
                    //    lstSDT.Add(entity.SDT_NHAN_TIN2.Trim());
                    //if (!string.IsNullOrEmpty(entity.SDT_BO))
                    //    lstSDT.Add(entity.SDT_BO.Trim());
                    //if (!string.IsNullOrEmpty(entity.SDT_ME))
                    //    lstSDT.Add(entity.SDT_ME.Trim());
                    //if (!string.IsNullOrEmpty(entity.SDT_NBH))
                    //    lstSDT.Add(entity.SDT_NBH.Trim());
                    //HOC_SINH hocSinh = hocSinhBO.checkExistsHocSinh(Convert.ToInt16(Sys_Ma_Nam_hoc), Sys_This_Truong.ID, entity.ID_KHOI, entity.ID_LOP, entity.HO_TEN, lstSDT, entity.NGAY_SINH, entity.MA);
                    //bool is_update = hocSinh != null;
                    //if (hocSinh == null)
                    //{
                    //    hocSinh = new HOC_SINH();
                    //    hocSinh.ID_TRUONG = Sys_This_Truong.ID;
                    //    if (lstColumn.Contains("ID_KHOI"))
                    //        hocSinh.ID_KHOI = entity.ID_KHOI;
                    //    if (lstColumn.Contains("ID_LOP"))
                    //        hocSinh.ID_LOP = entity.ID_LOP;
                    //    if (lstColumn.Contains("HO_TEN"))
                    //    {
                    //        string ho_dem = "", ten = "";
                    //        hocSinh.HO_TEN = entity.HO_TEN;
                    //        localAPI.splitHoTen(hocSinh.HO_TEN, out ho_dem, out ten);
                    //        hocSinh.TEN = ten.Trim();
                    //        hocSinh.HO_DEM = ho_dem.Trim();
                    //    }
                    //}
                    //if (lstColumn.Contains("NGAY_SINH"))
                    //    hocSinh.IS_DELETE = false;
                    //hocSinh.ID_NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
                    //hocSinh.MA_CAP_HOC = Sys_This_Cap_Hoc;
                    #endregion
                    string strMsg = string.Empty;
                    //if (resEntity.Res)
                    //{
                    //    if (!is_update)
                    //    {
                    //        resEntity = hocSinhBO.insert(hocSinh, Sys_User.ID);
                    //        HOC_SINH resHocSinh = (HOC_SINH)resEntity.ResObject;
                    //        if (resEntity.Res)
                    //            logUserBO.insert(Sys_This_Truong.ID, "IMPORT", "Thêm mới học sinh " + resHocSinh.ID, Sys_User.ID, DateTime.Now);
                    //    }
                    //    else
                    //    {
                    //        resEntity = hocSinhBO.update(hocSinh, Sys_User.ID);
                    //        if (resEntity.Res)
                    //            logUserBO.insert(Sys_This_Truong.ID, "IMPORT", "Cập nhật học sinh " + hocSinh.ID, Sys_User.ID, DateTime.Now);
                    //    }
                    //    if (resEntity.Res)
                    //        count_success++;

                    //}
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
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('success', 'Cập nhật thành công " + count_success + " bản ghi!');", true);
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
        protected void btTaiExcel_Click(object sender, EventArgs e)
        {
            string fileMau = "Template-ThuPhiHocSinh.xls";

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
        protected void rcbKhoi_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbLop.ClearSelection();
            rcbLop.Text = string.Empty;
            rcbLop.DataBind();
        }
        protected void rcbDotThu_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            short? id_dot_thu = localAPI.ConvertStringToShort(rcbDotThu.SelectedValue);
            if (id_dot_thu == 2)
            {
                rcbHocKy.Enabled = true;
                rcbThang.Enabled = false;
                rcbThang.ClearSelection();
                rcbThang.Text = string.Empty;
            }
            else if (id_dot_thu == 3)
            {
                rcbHocKy.Enabled = false;
                rcbThang.Enabled = true;
                rcbHocKy.ClearSelection();
                rcbHocKy.Text = string.Empty;
            }
            else
            {
                rcbHocKy.Enabled = false;
                rcbThang.Enabled = false;
                rcbHocKy.ClearSelection();
                rcbThang.ClearSelection();
                rcbHocKy.Text = string.Empty;
                rcbThang.Text = string.Empty;
            }
            RadGrid1.Rebind();
        }
    }
}