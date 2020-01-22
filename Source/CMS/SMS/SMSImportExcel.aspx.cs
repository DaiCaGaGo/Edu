using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using OneEduDataAccess.Static;
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

namespace CMS.SMS
{
    public partial class SMSImportExcel : AuthenticatePage
    {
        private HocSinhBO hocSinhBO = new HocSinhBO();
        private LopBO lopBO = new LopBO();
        private GiaoVienBO giaoVienBO = new GiaoVienBO();
        private QuyTinBO quyTinBO = new QuyTinBO();
        private TinNhanBO tinNhanBO = new TinNhanBO();
        private LocalAPI localAPI = new LocalAPI();
        private List<string> lstTitle = new List<string>();
        TruongBO truongBO = new TruongBO();
        LogUserBO logUserBO = new LogUserBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            bt_EXCELtoSQL.Visible = is_access(SYS_Type_Access.SEND_SMS);
            btnSendAll.Visible = is_access(SYS_Type_Access.SEND_SMS);
            if (!IsPostBack)
            {
                Session["ExcelSMS" + Sys_User.ID] = null;
                Session["ExcelSMSMessageId" + Sys_User.ID] = null;
                Session["ExcelColumnTitle" + Sys_User.ID] = null;
                viewQuyTin();
            }
        }
        protected void viewQuyTin()
        {
            short nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc);
            short thang = Convert.ToInt16(DateTime.Now.Month);
            if (thang < 8) nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc + 1);
            if (Sys_This_Truong.ID_DOI_TAC == null || Sys_This_Truong.ID_DOI_TAC == 0)
            {
                QUY_TIN quyTinTheoNam = quyTinBO.getQuyTinByNamHoc(Convert.ToInt16(localAPI.GetYearNow()), Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID);
                bool is_insert_new_quytb = false;
                QUY_TIN quyTinTheoThang = quyTinBO.getQuyTin(nam_hoc, thang, Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID, out is_insert_new_quytb);
                if (quyTinTheoNam != null && quyTinTheoThang != null)
                {
                    double tong_con_thang = quyTinTheoThang.TONG_CON + (quyTinTheoThang.TONG_CAP + quyTinTheoThang.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                    / 100;
                    double tong_con_nam = quyTinTheoNam.TONG_CON + (quyTinTheoNam.TONG_CAP + quyTinTheoNam.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                        / 100;
                    if (Sys_This_Truong.IS_ACTIVE_SMS != true || tong_con_nam <= 0)
                    {
                        //bt_EXCELtoSQL.Visible = false;
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", " notification('warning', 'Đơn vị đã sử dụng vượt quỹ tin được cấp. Vui lòng liên hệ lại với quản trị viên!');", true);
                        //return;
                    }
                    else if (Sys_This_Truong.IS_ACTIVE_SMS != true || tong_con_thang <= 0)
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", " notification('warning', 'Đơn vị đã sử dụng vượt quỹ tin tháng!');", true);
                    }
                    txtTongQuyTinConLaiTheoNam.Text = "Quỹ tin còn lại theo năm: " + ((quyTinTheoNam == null) ? "0" : quyTinTheoNam.TONG_CON.ToString());
                    //txtTongQuyTinConLaiTheoThang.Text = "Quỹ tháng còn: " + (quyTinTheoThang == null ? "0" : (quyTinTheoThang.TONG_CON > quyTinTheoNam.TONG_CON) ? quyTinTheoNam.TONG_CON.ToString() : quyTinTheoThang.TONG_CON.ToString());
                    txtTongQuyTinConLaiTheoThang.Text = "Quỹ tháng còn: " + (quyTinTheoThang == null ? "0" : quyTinTheoThang.TONG_CON.ToString());
                }
                else
                {
                    bt_EXCELtoSQL.Visible = false;
                    btnSendAll.Visible = false;
                    txtTongQuyTinConLaiTheoThang.Visible = false;
                    txtTongQuyTinConLaiTheoNam.Text = "Đơn vị không được cấp quota";
                }
            }
            else
            {
                TRUONG detailTruong = new TRUONG();
                long tong_tin_con = 0;
                detailTruong = truongBO.getTruongById(Sys_This_Truong.ID);
                if (detailTruong != null)
                {
                    txtTongQuyTinConLaiTheoNam.Text = "Tổng tin cấp: " + (detailTruong.TONG_TIN_CAP == null ? "0" : detailTruong.TONG_TIN_CAP.ToString());
                    if (detailTruong.TONG_TIN_CAP != null)
                    {
                        tong_tin_con = detailTruong.TONG_TIN_DA_DUNG != null ? (detailTruong.TONG_TIN_CAP.Value - detailTruong.TONG_TIN_DA_DUNG.Value) : detailTruong.TONG_TIN_CAP.Value;
                    }
                    else tong_tin_con = detailTruong.TONG_TIN_DA_DUNG != null ? (-detailTruong.TONG_TIN_DA_DUNG.Value) : 0;
                    txtTongQuyTinConLaiTheoThang.Text = "Tổng tin còn: " + tong_tin_con;
                    if (Sys_This_Truong.IS_ACTIVE_SMS != true)
                    {
                        bt_EXCELtoSQL.Visible = false;
                        btnSendAll.Visible = false;
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", " notification('warning', 'Đơn vị chưa đăng ký sử dụng SMS nên không thể gửi tin nhắn!');", true);
                    }
                }
                if (tong_tin_con <= 0)
                {
                    bt_EXCELtoSQL.Visible = false;
                    btnSendAll.Visible = false;
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", " notification('warning', 'Đơn vị đã sử dụng hết quỹ tin được cấp. Vui lòng liên hệ lại với quản trị viên!');", true);
                }
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            if (Session["ExcelSMS" + Sys_User.ID] != null)
            {
                dt = (DataTable)Session["ExcelSMS" + Sys_User.ID];
            }
            if (Session["ExcelColumnTitle" + Sys_User.ID] != null)
            {
                lstTitle = (List<string>)Session["ExcelColumnTitle" + Sys_User.ID];
            }
            for (int i = 0; i < 15; i++)
            {
                if (i < lstTitle.Count)
                    RadGrid1.MasterTableView.Columns.FindByUniqueName("COT_" + (i + 1)).Display = true;
                else
                    RadGrid1.MasterTableView.Columns.FindByUniqueName("COT_" + (i + 1)).Display = false;
            }

            #region add thêm cột
            int count_true = lstTitle.Count;
            DataTable dt1 = dt;
            List<string> lstTitle1 = lstTitle;
            for (int i = count_true; i < 15; i++)
            {
                lstTitle1.Add("COT_" + (i + 1));
                dt1.Columns.Add(new DataColumn("COT_" + (i + 1)));
            }
            #endregion

            if (dt != null && dt.Columns.Count > 0)
                RadGrid1.DataSource = dt1;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "SetGridHeight();", true);
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
                if (Session["ExcelSMS" + Sys_User.ID] != null)
                {
                    DataTable dt = (DataTable)Session["ExcelSMS" + Sys_User.ID];
                    if (dt.Rows.Count > 0)
                    {
                        RadGrid1.Rebind();
                        bt_EXCELtoSQL.Visible = true;
                        btnSendAll.Visible = true;
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
                        if (dt.Columns.Count < localAPI.ExcelColumnNameToNumber("T"))
                        {
                            for (int i = dt.Columns.Count - 1; i > localAPI.ExcelColumnNameToNumber("T"); i--)
                                dt.Columns.RemoveAt(i);
                        }
                        Session["ExcelSMS" + Sys_User.ID] = dt;
                        Session["ExcelSMSMessageId" + Sys_User.ID] = message_id;

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
                int max_cot = myDataTable.Columns.Count;
                if (max_cot > 15) max_cot = 15;
                lstTitle = new List<string>();
                for (int i = 0; i < max_cot; i++)
                {
                    try
                    {
                        lstTitle.Add(myDataTable.Columns[i].ColumnName);
                        myDataTable.Columns[i].ColumnName = "COT_" + (i + 1);
                    }
                    catch { }
                }
                Session["ExcelColumnTitle" + Sys_User.ID] = lstTitle;
                #endregion
                #endregion
                #region Valid excel
                if (myDataTable.Rows.Count != 0)
                {
                    dt = myDataTable.Copy();
                    int indexhoten = -1;
                    //int indexngaysinh = -1;
                    int indexlop = -1;
                    int indexsdt = -1;
                    for (int i = 0; i < lstTitle.Count; i++)
                    {
                        if (lstTitle[i].ToNormalizeLowerRelace() == "Số điện thoại".ToNormalizeLowerRelace())
                        {
                            indexsdt = i;
                        }
                        if (lstTitle[i].ToNormalizeLowerRelace() == "Họ và Tên".ToNormalizeLowerRelace())
                        {
                            indexhoten = i;
                        }
                        //if (lstTitle[i].ToNormalizeLowerRelace() == "Ngày Sinh".ToNormalizeLowerRelace())
                        //{
                        //    indexngaysinh = i;
                        //}
                        if (lstTitle[i].ToNormalizeLowerRelace() == "Lớp".ToNormalizeLowerRelace())
                        {
                            indexlop = i;
                        }
                    }
                    if (cbIsHocSinh.Checked && (indexhoten < 0 || indexlop < 0))
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Phải có các cột dữ liệu Họ và Tên, Lớp!');", true);
                        bt_EXCELtoSQL.Visible = false;
                        btnSendAll.Visible = false;
                        return myDataTable;
                    }
                    else if (!cbIsHocSinh.Checked && indexsdt == -1)
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn phải có cột số điện thoại!');", true);
                        bt_EXCELtoSQL.Visible = false;
                        btnSendAll.Visible = false;
                        return myDataTable;
                    }
                    else
                    {
                        bt_EXCELtoSQL.Visible = true;
                        btnSendAll.Visible = false;
                        int i = 0;
                        int count = 0;
                        foreach (DataRow row in dt.Rows)
                        {
                            Boolean kt = true;
                            if (string.IsNullOrEmpty(row[indexsdt].ToString().Trim()))
                            {
                                kt = false;
                            }
                            if (!kt)
                            {
                                myDataTable.Rows.RemoveAt(i);
                                count++;
                            }
                            else
                                i++;
                        }
                        if (count > 0) ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Có " + count + " số điện thoại đang để trống!');", true);
                    }
                }
                #endregion
                #region Đổi đầu số điện thoại
                if (myDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < myDataTable.Rows.Count; i++)
                    {
                        string sdt = "";
                        if (myDataTable.Rows[i]["COT_4"] != null) sdt = myDataTable.Rows[i]["COT_4"].ToString();
                        if (!string.IsNullOrEmpty(sdt))
                        {
                            sdt = localAPI.Add84(myDataTable.Rows[i]["COT_4"].ToString());
                            myDataTable.Rows[i]["COT_4"] = sdt;
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
        protected void bt_EXCELtoSQL_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.SEND_SMS))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            List<ResultError> lstRes = new List<ResultError>();
            List<ResultEntity> lstResEntity = new List<ResultEntity>();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            CommonValidate validate = new CommonValidate();
            lstTitle = (List<string>)Session["ExcelColumnTitle" + Sys_User.ID];

            #region hẹn giờ
            DateTime? dt = null;
            bool is_checkHenGio = false;
            if (cbHenGioGuiTin.Checked)
            {
                is_checkHenGio = true;
                try
                {
                    dt = Convert.ToDateTime(tbTime.Text);
                }
                catch (Exception ex) { dt = null; }
                if (dt == null)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Thời gian không được để trống.');", true);
                    return;
                }
                else if (dt <= DateTime.Now)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Thời gian hẹn giờ không được nhỏ hơn thời gian hiện tại.');", true);
                    return;
                }
            }
            #endregion

            int tong_tin_gui = 0;
            string sdt = string.Empty;

            List<LOP> lstLopInTruong = new List<LOP>();
            lstLopInTruong = lopBO.getLopByTruongCapNamHoc(Sys_This_Cap_Hoc, Sys_This_Truong.ID, Convert.ToInt16(Sys_Ma_Nam_hoc));
            List<HOC_SINH> lstHocSinhInTruong = hocSinhBO.getHocSinhByKhoiLop(Sys_This_Truong.ID, null, null, Convert.ToInt16(Sys_Ma_Nam_hoc), Sys_This_Cap_Hoc);
            List<TIN_NHAN> lstTinNhan = new List<TIN_NHAN>();

            short nam_gui = Convert.ToInt16(DateTime.Now.Year);
            short thang_gui = Convert.ToInt16(DateTime.Now.Month);
            short tuan_gui = Convert.ToInt16(localAPI.getThisWeek().ToString());
            string brandname = "", cp = "";

            TIN_NHAN checkExists = new TIN_NHAN();
            TIN_NHAN checkSms = new TIN_NHAN();
            foreach (GridDataItem item in RadGrid1.SelectedItems)
            {
                ResultEntity resEntity = new ResultEntity();
                resEntity.Res = true;
                bool checkStatus = false;
                bool checkPhone = false;
                HiddenField hdresMsg = (HiddenField)item.FindControl("hdresMsg");
                System.Web.UI.HtmlControls.HtmlImage icError = (System.Web.UI.HtmlControls.HtmlImage)item.FindControl("iconErr");
                System.Web.UI.HtmlControls.HtmlImage icSuccess = (System.Web.UI.HtmlControls.HtmlImage)item.FindControl("iconSuccess");
                icError.Visible = false;
                icSuccess.Visible = false;

                LOP lopDetail = new LOP();
                HOC_SINH detailHS = new HOC_SINH();
                if (cbIsHocSinh.Checked)
                {
                    int indexhoten = -1;
                    int indexngaysinh = -1;
                    int indexlop = -1;
                    int indexsdt = -1;
                    for (int i = 0; i < lstTitle.Count; i++)
                    {
                        if (lstTitle[i].ToNormalizeLowerRelace() == "Họ và Tên".ToNormalizeLowerRelace())
                        {
                            indexhoten = i;
                        }
                        if (lstTitle[i].ToNormalizeLowerRelace() == "Ngày Sinh".ToNormalizeLowerRelace())
                        {
                            indexngaysinh = i;
                        }
                        if (lstTitle[i].ToNormalizeLowerRelace() == "Lớp".ToNormalizeLowerRelace())
                        {
                            indexlop = i;
                        }
                        if (lstTitle[i].ToNormalizeLowerRelace() == "Số điện thoại".ToNormalizeLowerRelace())
                        {
                            indexsdt = i;
                        }
                    }
                    if (indexhoten < 0 || indexngaysinh < 0 || indexlop < 0)
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Phải có các cột dữ liệu Họ và Tên, Ngày Sinh, Lớp!');", true);
                        return;
                    }

                    Label lblTenLop = (Label)item.FindControl("lblCOT_" + (indexlop + 1));
                    Label lblHoTen = (Label)item.FindControl("lblCOT_" + (indexhoten + 1));
                    Label lblSDT = (Label)item.FindControl("lblCOT_" + (indexsdt + 1));
                    Label lblNgaySinh = (Label)item.FindControl("lblCOT_" + (indexngaysinh + 1));
                    string ten_lop = lblTenLop.Text.Trim();
                    string ho_ten = lblHoTen.Text.Trim();
                    sdt = lblSDT.Text.Trim();
                    string str_ngay_sinh = lblNgaySinh.Text.Trim();

                    DateTime? dtNgaySinh = localAPI.ConvertDDMMYYToDateTime(str_ngay_sinh);
                    lopDetail = lstLopInTruong.FirstOrDefault(x => x.TEN.ToNormalizeLowerRelace() == ten_lop.ToNormalizeLowerRelace());
                    if (lopDetail == null)
                    {
                        resEntity.Msg = "Không tồn tại lớp trong trường.";
                        goto endxuly;
                    }
                    detailHS = lstHocSinhInTruong.FirstOrDefault(x => x.HO_TEN.ToNormalizeLowerRelace() == ho_ten.ToNormalizeLowerRelace()
                    && x.ID_LOP == lopDetail.ID && x.NGAY_SINH == dtNgaySinh);
                    if (detailHS == null)
                    {
                        resEntity.Msg = "Không tồn tại học sinh trong lớp.";
                        goto endxuly;
                    }
                }
                else
                {
                    int indexhoten = -1;
                    int indexsdt = -1;
                    for (int i = 0; i < lstTitle.Count; i++)
                    {
                        if (lstTitle[i].ToNormalizeLowerRelace() == "Họ và Tên".ToNormalizeLowerRelace())
                        {
                            indexhoten = i;
                        }
                        if (lstTitle[i].ToNormalizeLowerRelace() == "Số điện thoại".ToNormalizeLowerRelace())
                        {
                            indexsdt = i;
                        }
                    }
                    if (indexsdt < 0)
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Phải có các cột dữ liệu Số điện thoại!');", true);
                        return;
                    }
                    string ten_lop = string.Empty;
                    Label lblHoTen = (Label)item.FindControl("lblCOT_" + (indexhoten + 1));
                    Label lblSDT = (Label)item.FindControl("lblCOT_" + (indexsdt + 1));
                    string ho_ten = lblHoTen.Text.Trim();
                    sdt = lblSDT.Text.Trim();
                    string str_ngay_sinh = string.Empty;
                    DateTime? dtNgaySinh = localAPI.ConvertDDMMYYToDateTime(str_ngay_sinh);
                }

                sdt = localAPI.Add84(sdt);
                TextBox tbNoiDung = (TextBox)item.FindControl("tbNoiDungTongHop");

                string noi_dung = tbNoiDung.Text.Trim();
                string noi_dung_en = localAPI.chuyenTiengVietKhongDau(noi_dung);
                int so_tin = localAPI.demSoTin(noi_dung_en);
                if (string.IsNullOrEmpty(noi_dung_en))
                {
                    resEntity.Msg = "Nội dung không được để trống";
                    checkStatus = false;
                    goto endxuly;
                }

                #region get data
                string sdt_nhan_tin = (detailHS != null && detailHS.ID > 0) ? detailHS.SDT_NHAN_TIN : sdt;
                string loaiNhaMang = localAPI.getLoaiNhaMang(sdt_nhan_tin);

                if (!string.IsNullOrEmpty(loaiNhaMang) && !string.IsNullOrEmpty(noi_dung_en))
                {
                    checkPhone = true;
                    long? id_nguoi_nhan = (detailHS != null && detailHS.ID > 0) ? detailHS.ID : (long?)null;
                    short loai_nguoi_nhan = (detailHS != null && detailHS.ID > 0) ? Convert.ToInt16(1) : Convert.ToInt16(3);

                    checkExists = tinNhanBO.checkExistsSms(Sys_This_Truong.ID, nam_gui, thang_gui, null, sdt, (is_checkHenGio && dt != null) ? dt : null, Sys_Time_Send, noi_dung_en);
                    checkSms = lstTinNhan.FirstOrDefault(x => x.SDT_NHAN == sdt && x.NOI_DUNG_KHONG_DAU == noi_dung_en);

                    if (checkExists == null && checkSms == null)
                    {
                        checkStatus = true;
                        TIN_NHAN tinDetail = new TIN_NHAN();
                        tinDetail.ID_TRUONG = Sys_This_Truong.ID;
                        tinDetail.ID_NGUOI_NHAN = id_nguoi_nhan;
                        tinDetail.LOAI_NGUOI_NHAN = loai_nguoi_nhan;
                        tinDetail.SDT_NHAN = sdt_nhan_tin;
                        tinDetail.MA_GOI_TIN = Sys_This_Truong.MA_GOI_TIN;
                        tinDetail.THOI_GIAN_GUI = (is_checkHenGio && dt != null) ? dt : DateTime.Now;
                        tinDetail.NGUOI_GUI = Sys_User.ID;
                        tinDetail.LOAI_TIN = SYS_Loai_Tin.Tin_Thong_Bao;
                        tinDetail.KIEU_GUI = 1;
                        tinDetail.NAM_GUI = nam_gui;
                        tinDetail.THANG_GUI = thang_gui;
                        tinDetail.TUAN_GUI = tuan_gui;
                        tinDetail.NOI_DUNG = noi_dung;
                        tinDetail.NOI_DUNG_KHONG_DAU = noi_dung_en;
                        tinDetail.SO_TIN = so_tin;
                        tinDetail.LOAI_NHA_MANG = loaiNhaMang;

                        brandname = ""; cp = "";
                        localAPI.getBrandnameAndCp(Sys_This_Truong, loaiNhaMang, out brandname, out cp);
                        tinDetail.BRAND_NAME = brandname;
                        tinDetail.CP = cp;
                        lstTinNhan.Add(tinDetail);
                        tong_tin_gui += so_tin;
                        resEntity.Msg = "Gửi tin thành công";
                    }
                    else
                    {
                        resEntity.Msg = "Nội dung đã được gửi, không thể gửi lại.";
                    }
                }
                else
                {
                    resEntity.Msg = "Vui lòng kiểm tra lại SĐT hoặc nội dung gửi!";
                }
                #endregion

                if (!checkPhone && !string.IsNullOrEmpty(resEntity.Msg))
                    resEntity.Msg = "Số điện thoại không đúng!";
                endxuly:
                {
                    hdresMsg.Value = resEntity.Msg;
                    if (checkStatus)
                    {
                        icSuccess.Visible = true;
                    }
                    else
                    {
                        item.ForeColor = Color.Red;
                        icError.Visible = true;
                    }
                }
            }
            #region save sms
            if (lstTinNhan.Count > 0)
            {
                short nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc);
                short thang = Convert.ToInt16(DateTime.Now.Month);
                if (thang < 8) nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc + 1);
                if (Sys_This_Truong.ID_DOI_TAC == null || Sys_This_Truong.ID_DOI_TAC == 0)
                {
                    QUY_TIN quyTinTheoNam = quyTinBO.getQuyTinByNamHoc(Convert.ToInt16(localAPI.GetYearNow()), Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID);
                    bool is_insert_new_quytb = false;
                    QUY_TIN quyTinTheoThang = quyTinBO.getQuyTin(nam_hoc, thang, Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID, out is_insert_new_quytb);
                    if (quyTinTheoNam != null && quyTinTheoThang != null)
                    {
                        double tong_con_thang = quyTinTheoThang.TONG_CON + ((quyTinTheoThang.TONG_CAP + quyTinTheoThang.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                    / 100);
                        double tong_con_nam = quyTinTheoNam.TONG_CON + ((quyTinTheoNam.TONG_CAP + quyTinTheoNam.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                            / 100);

                        if (Sys_This_Truong.IS_SAN_QUY_TIN_NAM == true)
                        {
                            if (tong_tin_gui > tong_con_nam)
                            {
                                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị không còn đủ quota để gửi tin nhắn. Vui lòng liên hệ với quản trị viên!');", true);
                                return;
                            }
                            else
                            {
                                res = tinNhanBO.insertTinNhanThongBao(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, 0, Sys_User.ID);
                                if (res.Res)
                                {
                                    logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi tin tùy biến: " + tong_tin_gui + " tin nhắn", Sys_User.ID, DateTime.Now);
                                }
                                if (tong_tin_gui >= tong_con_thang)
                                {
                                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Đơn vị đã sử dụng vượt quá quỹ tin tháng!');", true);
                                }
                            }

                        }
                        else
                        {
                            if (tong_tin_gui > tong_con_thang || tong_tin_gui > tong_con_nam)
                            {
                                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị đã sử dụng vượt quá quỹ tin tháng. Vui lòng liên hệ với quản trị viên để được giúp đỡ!');", true);
                                return;
                            }
                            else
                            {
                                res = tinNhanBO.insertTinNhanThongBao(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, 0, Sys_User.ID);
                                if (res.Res)
                                {
                                    logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi tin tùy biến: " + tong_tin_gui + " tin nhắn", Sys_User.ID, DateTime.Now);
                                }
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị không được cấp quota!');", true);
                        return;
                    }
                }
                else
                {
                    TRUONG detailTruong = new TRUONG();
                    long tong_tin_con = 0;
                    detailTruong = truongBO.getTruongById(Sys_This_Truong.ID);
                    if (detailTruong != null)
                    {
                        if (detailTruong.TONG_TIN_CAP != null)
                        {
                            tong_tin_con = detailTruong.TONG_TIN_DA_DUNG != null ? (detailTruong.TONG_TIN_CAP.Value - detailTruong.TONG_TIN_DA_DUNG.Value) : detailTruong.TONG_TIN_CAP.Value;
                        }
                        else tong_tin_con = detailTruong.TONG_TIN_DA_DUNG != null ? (-detailTruong.TONG_TIN_DA_DUNG.Value) : 0;
                        if (Sys_This_Truong.IS_ACTIVE_SMS != null && Sys_This_Truong.IS_ACTIVE_SMS == true && tong_tin_con > tong_tin_gui)
                        {
                            res = tinNhanBO.insertTinNhanThongBao(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, Convert.ToInt16(localAPI.getThisWeek().ToString()), Sys_User.ID);
                            if (res.Res)
                            {
                                logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi tin tùy biến: " + tong_tin_gui + " tin nhắn", Sys_User.ID, DateTime.Now);
                            }
                        }
                    }
                }
            }
            #endregion

            string strMsg = "";
            if (!res.Res && lstTinNhan.Count > 0)
            {
                strMsg = "notification('error', 'Có lỗi khi lưu dữ liệu. Liên hệ với quản trị viên.');";
            }
            else
            {
                strMsg = " notification('success', 'Có " + tong_tin_gui + " tin nhắn được gửi.');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            Session["ExcelSMSDes" + Sys_User.ID] = res;
            viewQuyTin();

        }
        protected void btTaiExcel_Click(object sender, EventArgs e)
        {
            string fileMau = "Template-SMSTuyBien.xls";

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
        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridHeaderItem)
            {
                GridHeaderItem item = (GridHeaderItem)e.Item;
                Label lblCOT_1 = (Label)item.FindControl("lblCOT_1");
                Label lblCOT_2 = (Label)item.FindControl("lblCOT_2");
                Label lblCOT_3 = (Label)item.FindControl("lblCOT_3");
                Label lblCOT_4 = (Label)item.FindControl("lblCOT_4");
                Label lblCOT_5 = (Label)item.FindControl("lblCOT_5");
                Label lblCOT_6 = (Label)item.FindControl("lblCOT_6");
                Label lblCOT_7 = (Label)item.FindControl("lblCOT_7");
                Label lblCOT_8 = (Label)item.FindControl("lblCOT_8");
                Label lblCOT_9 = (Label)item.FindControl("lblCOT_9");
                Label lblCOT_10 = (Label)item.FindControl("lblCOT_10");
                Label lblCOT_11 = (Label)item.FindControl("lblCOT_11");
                Label lblCOT_12 = (Label)item.FindControl("lblCOT_12");
                Label lblCOT_13 = (Label)item.FindControl("lblCOT_13");
                Label lblCOT_14 = (Label)item.FindControl("lblCOT_14");
                Label lblCOT_15 = (Label)item.FindControl("lblCOT_15");
                int max_cot = lstTitle.Count;
                if (lstTitle.Count > 0)
                {
                    lblCOT_1.Text = lstTitle[0] + " (1)";
                    lblCOT_2.Text = lstTitle[1] + " (2)";
                    lblCOT_3.Text = lstTitle[2] + " (3)";
                    lblCOT_4.Text = lstTitle[3] + " (4)";
                    lblCOT_5.Text = lstTitle[4] + " (5)";
                    lblCOT_6.Text = lstTitle[5] + " (6)";
                    if (lstTitle.Count > 6) lblCOT_7.Text = lstTitle[6] + " (7)";
                    if (lstTitle.Count > 7) lblCOT_8.Text = lstTitle[7] + " (8)";
                    if (lstTitle.Count > 8) lblCOT_9.Text = lstTitle[8] + " (9)";
                    if (lstTitle.Count > 9) lblCOT_10.Text = lstTitle[9] + " (10)";
                    if (lstTitle.Count > 10) lblCOT_11.Text = lstTitle[10] + " (11)";
                    if (lstTitle.Count > 11) lblCOT_12.Text = lstTitle[11] + " (12)";
                    if (lstTitle.Count > 12) lblCOT_13.Text = lstTitle[12] + " (13)";
                    if (lstTitle.Count > 13) lblCOT_14.Text = lstTitle[13] + " (14)";
                    if (lstTitle.Count > 14) lblCOT_15.Text = lstTitle[14] + " (15)";
                }

            }
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                System.Web.UI.HtmlControls.HtmlImage error1 = (System.Web.UI.HtmlControls.HtmlImage)item.FindControl("iconErr");
                System.Web.UI.HtmlControls.HtmlImage success1 = (System.Web.UI.HtmlControls.HtmlImage)item.FindControl("iconSuccess");
                error1.Visible = false;
                success1.Visible = false;
                try
                {
                    Label lblCOT_3 = (Label)item.FindControl("lblCOT_3");
                    string str_ngay_sinh = Convert.ToDateTime(lblCOT_3.Text).ToString("dd/MM/yyyy");
                    lblCOT_3.Text = str_ngay_sinh;
                }
                catch { }
            }
        }
        protected void cbHenGioGuiTin_CheckedChanged(object sender, EventArgs e)
        {
            tbTime.Visible = (cbHenGioGuiTin.Checked) ? true : false;
        }
        protected void btnSendAll_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.SEND_SMS))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            List<ResultError> lstRes = new List<ResultError>();
            List<ResultEntity> lstResEntity = new List<ResultEntity>();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            CommonValidate validate = new CommonValidate();
            lstTitle = (List<string>)Session["ExcelColumnTitle" + Sys_User.ID];

            #region hẹn giờ
            DateTime? dt = null;
            bool is_checkHenGio = false;
            if (cbHenGioGuiTin.Checked)
            {
                is_checkHenGio = true;
                try
                {
                    dt = Convert.ToDateTime(tbTime.Text);
                }
                catch (Exception ex) { dt = null; }
                if (dt == null)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Thời gian không được để trống.');", true);
                    return;
                }
                else if (dt <= DateTime.Now)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Thời gian hẹn giờ không được nhỏ hơn thời gian hiện tại.');", true);
                    return;
                }
            }
            #endregion

            int tong_tin_gui = 0;
            string sdt = string.Empty;
            List<LOP> lstLopInTruong = new List<LOP>();
            lstLopInTruong = lopBO.getLopByTruongCapNamHoc(Sys_This_Cap_Hoc, Sys_This_Truong.ID, Convert.ToInt16(Sys_Ma_Nam_hoc));
            List<HOC_SINH> lstHocSinhInTruong = hocSinhBO.getHocSinhByKhoiLop(Sys_This_Truong.ID, null, null, Convert.ToInt16(Sys_Ma_Nam_hoc), Sys_This_Cap_Hoc);
            List<TIN_NHAN> lstTinNhan = new List<TIN_NHAN>();

            short nam_gui = Convert.ToInt16(DateTime.Now.Year);
            short thang_gui = Convert.ToInt16(DateTime.Now.Month);
            short tuan_gui = Convert.ToInt16(localAPI.getThisWeek().ToString());
            string brandname = "", cp = "";

            TIN_NHAN checkExists = new TIN_NHAN();
            TIN_NHAN checkSms = new TIN_NHAN();
            foreach (GridDataItem item in RadGrid1.Items)
            {
                ResultEntity resEntity = new ResultEntity();
                resEntity.Res = true;
                bool checkStatus = false;
                bool checkPhone = false;
                HiddenField hdresMsg = (HiddenField)item.FindControl("hdresMsg");
                System.Web.UI.HtmlControls.HtmlImage icError = (System.Web.UI.HtmlControls.HtmlImage)item.FindControl("iconErr");
                System.Web.UI.HtmlControls.HtmlImage icSuccess = (System.Web.UI.HtmlControls.HtmlImage)item.FindControl("iconSuccess");
                icError.Visible = false;
                icSuccess.Visible = false;

                LOP lopDetail = new LOP();
                HOC_SINH detailHS = new HOC_SINH();
                if (cbIsHocSinh.Checked)
                {
                    int indexhoten = -1;
                    int indexngaysinh = -1;
                    int indexlop = -1;
                    int indexsdt = -1;
                    for (int i = 0; i < lstTitle.Count; i++)
                    {
                        if (lstTitle[i].ToNormalizeLowerRelace() == "Họ và Tên".ToNormalizeLowerRelace())
                        {
                            indexhoten = i;
                        }
                        if (lstTitle[i].ToNormalizeLowerRelace() == "Ngày Sinh".ToNormalizeLowerRelace())
                        {
                            indexngaysinh = i;
                        }
                        if (lstTitle[i].ToNormalizeLowerRelace() == "Lớp".ToNormalizeLowerRelace())
                        {
                            indexlop = i;
                        }
                        if (lstTitle[i].ToNormalizeLowerRelace() == "Số điện thoại".ToNormalizeLowerRelace())
                        {
                            indexsdt = i;
                        }
                    }
                    if (indexhoten < 0 || indexngaysinh < 0 || indexlop < 0)
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Phải có các cột dữ liệu Họ và Tên, Ngày Sinh, Lớp!');", true);
                        return;
                    }

                    Label lblTenLop = (Label)item.FindControl("lblCOT_" + (indexlop + 1));
                    Label lblHoTen = (Label)item.FindControl("lblCOT_" + (indexhoten + 1));
                    Label lblSDT = (Label)item.FindControl("lblCOT_" + (indexsdt + 1));
                    Label lblNgaySinh = (Label)item.FindControl("lblCOT_" + (indexngaysinh + 1));
                    string ten_lop = lblTenLop.Text.Trim();
                    string ho_ten = lblHoTen.Text.Trim();
                    sdt = lblSDT.Text.Trim();
                    string str_ngay_sinh = lblNgaySinh.Text.Trim();

                    DateTime? dtNgaySinh = localAPI.ConvertDDMMYYToDateTime(str_ngay_sinh);
                    lopDetail = lstLopInTruong.FirstOrDefault(x => x.TEN.ToNormalizeLowerRelace() == ten_lop.ToNormalizeLowerRelace());
                    if (lopDetail == null)
                    {
                        resEntity.Msg = "Không tồn tại lớp trong trường.";
                        goto endxuly;
                    }
                    detailHS = lstHocSinhInTruong.FirstOrDefault(x => x.HO_TEN.ToNormalizeLowerRelace() == ho_ten.ToNormalizeLowerRelace()
                    && x.ID_LOP == lopDetail.ID && x.NGAY_SINH == dtNgaySinh);
                    if (detailHS == null)
                    {
                        resEntity.Msg = "Không tồn tại học sinh trong lớp.";
                        goto endxuly;
                    }
                }
                else
                {
                    int indexhoten = -1;
                    int indexsdt = -1;
                    for (int i = 0; i < lstTitle.Count; i++)
                    {
                        if (lstTitle[i].ToNormalizeLowerRelace() == "Họ và Tên".ToNormalizeLowerRelace())
                        {
                            indexhoten = i;
                        }
                        if (lstTitle[i].ToNormalizeLowerRelace() == "Số điện thoại".ToNormalizeLowerRelace())
                        {
                            indexsdt = i;
                        }
                    }
                    if (indexsdt < 0)
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Phải có các cột dữ liệu Số điện thoại!');", true);
                        return;
                    }
                    string ten_lop = string.Empty;
                    Label lblHoTen = (Label)item.FindControl("lblCOT_" + (indexhoten + 1));
                    Label lblSDT = (Label)item.FindControl("lblCOT_" + (indexsdt + 1));
                    string ho_ten = lblHoTen.Text.Trim();
                    sdt = lblSDT.Text.Trim();
                    string str_ngay_sinh = string.Empty;
                    DateTime? dtNgaySinh = localAPI.ConvertDDMMYYToDateTime(str_ngay_sinh);
                }

                sdt = localAPI.Add84(sdt);
                TextBox tbNoiDung = (TextBox)item.FindControl("tbNoiDungTongHop");

                string noi_dung = tbNoiDung.Text.Trim();
                string noi_dung_en = localAPI.chuyenTiengVietKhongDau(noi_dung);
                int so_tin = localAPI.demSoTin(noi_dung_en);
                if (string.IsNullOrEmpty(noi_dung_en))
                {
                    resEntity.Msg = "Nội dung không được để trống";
                    checkStatus = false;
                    goto endxuly;
                }

                #region get data
                string sdt_nhan_tin = (detailHS != null && detailHS.ID > 0) ? detailHS.SDT_NHAN_TIN : sdt;
                string loaiNhaMang = localAPI.getLoaiNhaMang(sdt_nhan_tin);

                if (!string.IsNullOrEmpty(loaiNhaMang) && !string.IsNullOrEmpty(noi_dung_en))
                {
                    checkPhone = true;
                    long? id_nguoi_nhan = (detailHS != null && detailHS.ID > 0) ? detailHS.ID : (long?)null;
                    short loai_nguoi_nhan = (detailHS != null && detailHS.ID > 0) ? Convert.ToInt16(1) : Convert.ToInt16(3);

                    checkExists = tinNhanBO.checkExistsSms(Sys_This_Truong.ID, nam_gui, thang_gui, null, sdt, (is_checkHenGio && dt != null) ? dt : null, Sys_Time_Send, noi_dung_en);
                    checkSms = lstTinNhan.FirstOrDefault(x => x.SDT_NHAN == sdt && x.NOI_DUNG_KHONG_DAU == noi_dung_en);

                    if (checkExists == null && checkSms == null)
                    {
                        checkStatus = true;
                        TIN_NHAN tinDetail = new TIN_NHAN();
                        tinDetail.ID_TRUONG = Sys_This_Truong.ID;
                        tinDetail.ID_NGUOI_NHAN = id_nguoi_nhan;
                        tinDetail.LOAI_NGUOI_NHAN = loai_nguoi_nhan;
                        tinDetail.SDT_NHAN = sdt_nhan_tin;
                        tinDetail.MA_GOI_TIN = Sys_This_Truong.MA_GOI_TIN;
                        tinDetail.THOI_GIAN_GUI = (is_checkHenGio && dt != null) ? dt : DateTime.Now;
                        tinDetail.NGUOI_GUI = Sys_User.ID;
                        tinDetail.LOAI_TIN = SYS_Loai_Tin.Tin_Thong_Bao;
                        tinDetail.KIEU_GUI = 1;
                        tinDetail.NAM_GUI = nam_gui;
                        tinDetail.THANG_GUI = thang_gui;
                        tinDetail.TUAN_GUI = tuan_gui;
                        tinDetail.NOI_DUNG = noi_dung;
                        tinDetail.NOI_DUNG_KHONG_DAU = noi_dung_en;
                        tinDetail.SO_TIN = so_tin;
                        tinDetail.LOAI_NHA_MANG = loaiNhaMang;

                        brandname = ""; cp = "";
                        localAPI.getBrandnameAndCp(Sys_This_Truong, loaiNhaMang, out brandname, out cp);
                        tinDetail.BRAND_NAME = brandname;
                        tinDetail.CP = cp;
                        if (Sys_This_Truong.ID_DOI_TAC != null && Sys_This_Truong.ID_DOI_TAC > 0)
                            tinDetail.ID_DOI_TAC = Sys_This_Truong.ID_DOI_TAC;
                        else tinDetail.ID_DOI_TAC = null;
                        lstTinNhan.Add(tinDetail);
                        tong_tin_gui += so_tin;
                        resEntity.Msg = "Gửi tin thành công";
                    }
                    else
                    {
                        resEntity.Msg = "Nội dung đã được gửi, không thể gửi lại.";
                    }
                }
                else
                {
                    resEntity.Msg = "Vui lòng kiểm tra lại SĐT hoặc nội dung gửi!";
                }
                #endregion

                if (!checkPhone && !string.IsNullOrEmpty(resEntity.Msg))
                    resEntity.Msg = "Số điện thoại không đúng!";
                endxuly:
                {
                    hdresMsg.Value = resEntity.Msg;
                    if (checkStatus)
                    {
                        icSuccess.Visible = true;
                    }
                    else
                    {
                        item.ForeColor = Color.Red;
                        icError.Visible = true;
                    }
                }
            }
            #region save sms
            if (lstTinNhan.Count > 0)
            {
                short nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc);
                short thang = Convert.ToInt16(DateTime.Now.Month);
                if (thang < 8) nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc + 1);
                if (Sys_This_Truong.ID_DOI_TAC == null || Sys_This_Truong.ID_DOI_TAC == 0)
                {
                    QUY_TIN quyTinTheoNam = quyTinBO.getQuyTinByNamHoc(Convert.ToInt16(localAPI.GetYearNow()), Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID);
                    bool is_insert_new_quytb = false;
                    QUY_TIN quyTinTheoThang = quyTinBO.getQuyTin(nam_hoc, thang, Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID, out is_insert_new_quytb);
                    if (quyTinTheoNam != null && quyTinTheoThang != null)
                    {
                        double tong_con_thang = quyTinTheoThang.TONG_CON + ((quyTinTheoThang.TONG_CAP + quyTinTheoThang.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                    / 100);
                        double tong_con_nam = quyTinTheoNam.TONG_CON + ((quyTinTheoNam.TONG_CAP + quyTinTheoNam.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                            / 100);

                        if (Sys_This_Truong.IS_SAN_QUY_TIN_NAM == true)
                        {
                            if (tong_tin_gui > tong_con_nam)
                            {
                                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị không còn đủ quota để gửi tin nhắn. Vui lòng liên hệ với quản trị viên!');", true);
                                return;
                            }
                            else
                            {
                                res = tinNhanBO.insertTinNhanThongBao(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, 0, Sys_User.ID);
                                if (res.Res)
                                {
                                    logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi tin tùy biến: " + tong_tin_gui + " tin nhắn", Sys_User.ID, DateTime.Now);
                                }
                                if (tong_tin_gui >= tong_con_thang)
                                {
                                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Đơn vị đã sử dụng vượt quá quỹ tin tháng!');", true);
                                }
                            }

                        }
                        else
                        {
                            if (tong_tin_gui > tong_con_thang || tong_tin_gui > tong_con_nam)
                            {
                                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị đã sử dụng vượt quá quỹ tin tháng. Vui lòng liên hệ với quản trị viên để được giúp đỡ!');", true);
                                return;
                            }
                            else
                            {
                                res = tinNhanBO.insertTinNhanThongBao(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, 0, Sys_User.ID);
                                if (res.Res)
                                {
                                    logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi tin tùy biến: " + tong_tin_gui + " tin nhắn", Sys_User.ID, DateTime.Now);
                                }
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị không được cấp quota!');", true);
                        return;
                    }
                }
                else
                {
                    TRUONG detailTruong = new TRUONG();
                    long tong_tin_con = 0;
                    detailTruong = truongBO.getTruongById(Sys_This_Truong.ID);
                    if (detailTruong != null)
                    {
                        if (detailTruong.TONG_TIN_CAP != null)
                        {
                            tong_tin_con = detailTruong.TONG_TIN_DA_DUNG != null ? (detailTruong.TONG_TIN_CAP.Value - detailTruong.TONG_TIN_DA_DUNG.Value) : detailTruong.TONG_TIN_CAP.Value;
                        }
                        else tong_tin_con = detailTruong.TONG_TIN_DA_DUNG != null ? (-detailTruong.TONG_TIN_DA_DUNG.Value) : 0;
                        if (Sys_This_Truong.IS_ACTIVE_SMS != null && Sys_This_Truong.IS_ACTIVE_SMS == true && tong_tin_con > tong_tin_gui)
                        {
                            res = tinNhanBO.insertTinNhanThongBao(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, Convert.ToInt16(localAPI.getThisWeek().ToString()), Sys_User.ID);
                            if (res.Res)
                            {
                                logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi tin tùy biến: " + tong_tin_gui + " tin nhắn", Sys_User.ID, DateTime.Now);
                            }
                        }
                    }
                }
            }
            #endregion

            string strMsg = "";
            if (!res.Res && lstTinNhan.Count > 0)
            {
                strMsg = "notification('error', 'Có lỗi khi lưu dữ liệu. Liên hệ với quản trị viên.');";
            }
            else
            {
                strMsg = " notification('success', 'Có " + tong_tin_gui + " tin nhắn được gửi.');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            Session["ExcelSMSDes" + Sys_User.ID] = res;
            viewQuyTin();
        }
    }
}