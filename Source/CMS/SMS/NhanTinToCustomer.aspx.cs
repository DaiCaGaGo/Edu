using ClosedXML.Excel;
using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using OneEduDataAccess.Static;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CMS.SMS
{
    public partial class NhanTinToCustomer : AuthenticatePage
    {
        TruongBO truongBO = new TruongBO();
        QuyTinBO quyTinBO = new QuyTinBO();
        TinNhanBO tinNhanBO = new TinNhanBO();
        CustomerBO customerBO = new CustomerBO();
        CustomerToCustomerBO customerToCustomer = new CustomerToCustomerBO();
        LocalAPI localAPI = new LocalAPI();
        LogUserBO logUserBO = new LogUserBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            btGuiTuyChon.Visible = is_access(SYS_Type_Access.SEND_SMS) && !(Sys_This_Truong.IS_ACTIVE_SMS != true);
            btnGuiAll.Visible = is_access(SYS_Type_Access.SEND_SMS) && !(Sys_This_Truong.IS_ACTIVE_SMS != true);
            btExport.Visible = is_access(SYS_Type_Access.EXPORT);
            if (!IsPostBack)
            {
                objTo.SelectParameters.Add("id_truong", Sys_This_Truong.ID.ToString());
                rcbTo.DataBind();
                viewQuyTin();
            }
        }
        protected void viewQuyTin()
        {
            btGuiTuyChon.Visible = is_access(SYS_Type_Access.SEND_SMS) && !(Sys_This_Truong.IS_ACTIVE_SMS != true);
            btnGuiAll.Visible = is_access(SYS_Type_Access.SEND_SMS) && !(Sys_This_Truong.IS_ACTIVE_SMS != true);
            short nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc);
            short thang = Convert.ToInt16(DateTime.Now.Month);
            if (thang < 8) nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc + 1);
            if (Sys_This_Truong.ID_DOI_TAC == null || Sys_This_Truong.ID_DOI_TAC == 0)
            {
                QUY_TIN quyTinTheoNam = quyTinBO.getQuyTinByNamHoc(Convert.ToInt16(localAPI.GetYearNow()), Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID);
                bool is_insert_new_quytb = false;
                QUY_TIN quyTinTheoThang = quyTinBO.getQuyTin(nam_hoc, thang, Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID, out is_insert_new_quytb);
                if (Sys_This_Truong.IS_ACTIVE_SMS != null && Sys_This_Truong.IS_ACTIVE_SMS == true)
                {
                    if (quyTinTheoNam != null && quyTinTheoThang != null)
                    {
                        double tong_con_thang = quyTinTheoThang.TONG_CON + (quyTinTheoThang.TONG_CAP + quyTinTheoThang.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                        / 100;
                        double tong_con_nam = quyTinTheoNam.TONG_CON + (quyTinTheoNam.TONG_CAP + quyTinTheoNam.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                            / 100;
                        if (Sys_This_Truong.IS_ACTIVE_SMS != true || tong_con_nam <= 0)
                        {
                            //btGuiTuyChon.Visible = false;
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", " notification('warning', 'Đơn vị đã sử dụng vượt quỹ tin được cấp. Vui lòng liên hệ lại với quản trị viên!');", true);
                            //return;
                        }
                        else if (Sys_This_Truong.IS_ACTIVE_SMS != true || tong_con_thang <= 0)
                        {
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", " notification('warning', 'Đơn vị đã sử dụng vượt quỹ tin tháng!');", true);
                        }

                        txtTongQuyTinConLaiTheoNam.Text = "Quỹ tin còn lại theo năm: " + ((quyTinTheoNam == null) ? "0" : quyTinTheoNam.TONG_CON.ToString());
                        //if (quyTinTheoThang == null)
                        //{
                        //    txtTongQuyTinConLaiTheoThang.Text = "Quỹ tin còn lại theo tháng: 0";

                        //}
                        //else if (quyTinTheoThang.TONG_CON > quyTinTheoNam.TONG_CON)
                        //{
                        //    txtTongQuyTinConLaiTheoThang.Text = "Quỹ tin còn lại theo tháng: " + quyTinTheoNam.TONG_CON.ToString();
                        //}
                        //else
                        //{
                        //    txtTongQuyTinConLaiTheoThang.Text = "Quỹ tin còn lại theo tháng: " + quyTinTheoThang.TONG_CON.ToString();
                        //}
                        txtTongQuyTinConLaiTheoThang.Text = "Quỹ tháng còn: " + (quyTinTheoThang == null ? "0" : quyTinTheoThang.TONG_CON.ToString());
                    }
                    else
                    {
                        txtTongQuyTinConLaiTheoNam.Visible = false;
                        txtTongQuyTinConLaiTheoThang.Text = "Đơn vị không được cấp quota";
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", " notification('warning', 'Đơn vị chưa được cấp quota. Vui lòng liên hệ lại với quản trị viên!');", true);
                    txtTongQuyTinConLaiTheoNam.Text = "";
                    txtTongQuyTinConLaiTheoThang.Text = "";
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
                        btGuiTuyChon.Visible = false;
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", " notification('warning', 'Đơn vị chưa đăng ký sử dụng SMS nên không thể gửi tin nhắn!');", true);
                    }
                }
                if (tong_tin_con <= 0)
                {
                    btGuiTuyChon.Visible = false;
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", " notification('warning', 'Đơn vị đã sử dụng hết quỹ tin được cấp. Vui lòng liên hệ lại với quản trị viên!');", true);
                }
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            List<short> lst_ma_to = new List<short>();
            foreach (var item in rcbTo.CheckedItems)
            {
                lst_ma_to.Add(localAPI.ConvertStringToShort(item.Value).Value);
            }
            RadGrid1.DataSource = customerToCustomer.getCustomerByTo(Sys_This_Truong.ID, lst_ma_to);
            RadGrid1.MasterTableView.Columns.FindByUniqueName("SDT").Display = is_access(SYS_Type_Access.VIEW_INFOR);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "countSMSDuTinhNew();SetGridHeight();", true);
        }
        protected void cbHenGioGuiTin_CheckedChanged(object sender, EventArgs e)
        {
            divTime.Visible = (cbHenGioGuiTin.Checked) ? true : false;
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
            string newName = "Nhan_tin_thong_bao.xlsx";

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
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tên giáo viên", colM = 1, rowM = 1, width = 50 });
                    lstColumn.Add(new ExcelEntity { Name = "HO_TEN", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "SDT") && item.UniqueName == "SDT")
                {
                    DataColumn col = new DataColumn("SDT");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "SĐT nhận tin", colM = 1, rowM = 1, width = 20 });
                    lstColumn.Add(new ExcelEntity { Name = "SDT", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "NOI_DUNG_TB") && item.UniqueName == "NOI_DUNG_TB")
                {
                    DataColumn col = new DataColumn("NOI_DUNG_TB");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Nội dung", colM = 1, rowM = 1, width = 80 });
                    lstColumn.Add(new ExcelEntity { Name = "NOI_DUNG_TB", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                #endregion
            }
            RadGrid1.AllowPaging = false;
            foreach (GridDataItem item in RadGrid1.Items)
            {
                DataRow row = dt.NewRow();
                TextBox tbNoiDungTB = (TextBox)item.FindControl("tbNoiDungTB");
                foreach (DataColumn col in dt.Columns)
                {
                    row[col.ColumnName] = item[col.ColumnName].Text.Replace("&nbsp;", " ");
                    if (col.ColumnName == "NOI_DUNG_TB") row[col.ColumnName] = tbNoiDungTB.Text.Trim(); ;
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
            string tieuDe = "TIN NHẮN THÔNG BÁO";
            string hocKyNamHoc = "";
            listCell.Add(new ExcelHeaderEntity { name = tenSo, colM = 3, rowM = 1, colName = "A", rowIndex = 1, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tenPhong, colM = 3, rowM = 1, colName = "A", rowIndex = 2, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tieuDe, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 3, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            listCell.Add(new ExcelHeaderEntity { name = hocKyNamHoc, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 4, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            string nameFileOutput = newName;
            localAPI.ExportExcelDynamic(serverPath, path, newName, nameFileOutput, 1, listCell, rowHeaderStart, rowStart, colStart, colEnd, dt, lstHeader, lstColumn, true);
        }
        public string SetHeightForRadgrid()
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "countSMSDuTinhNew();SetGridHeight();", true);
            return null;
        }
        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                TextBox tbNoiDungTB = (TextBox)item.FindControl("tbNoiDungTB");
                string sdt = item["SDT"].Text;
                if (sdt == "&nbsp;") sdt = "";
                if (string.IsNullOrEmpty(sdt))
                {
                    item.ForeColor = Color.Red;
                    tbNoiDungTB.Enabled = false;
                }
            }
        }
        protected void btGuiTuyChon_Click(object sender, EventArgs e)
        {
            #region "check quyen"
            if (!is_access(SYS_Type_Access.SEND_SMS))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            if (Sys_This_Truong.IS_ACTIVE_SMS != true)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị đang không đăng ký dịch vụ!');", true);
                return;
            }
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
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            List<TIN_NHAN> lstTinNhan = new List<TIN_NHAN>();
            int tong_tin_gui = 0;

            TIN_NHAN checkExists = new TIN_NHAN();
            short nam_gui = Convert.ToInt16(DateTime.Now.Year);
            short thang_gui = Convert.ToInt16(DateTime.Now.Month);
            short tuan_gui = Convert.ToInt16(localAPI.getThisWeek().ToString());
            string brandname = "", cp = "";

            #region get list tin nhan
            if (RadGrid1.SelectedItems != null && RadGrid1.SelectedItems.Count > 0)
            {
                foreach (GridDataItem item in RadGrid1.SelectedItems)
                {
                    long? id_customer = Convert.ToInt64(item.GetDataKeyValue("ID").ToString());
                    string sdt = item["SDT"].Text;
                    if (sdt == "&nbsp;") sdt = "";
                    string loaiNhaMang = localAPI.getLoaiNhaMang(sdt);

                    TextBox tbNoiDungTB = (TextBox)item.FindControl("tbNoiDungTB");
                    string noi_dung = tbNoiDungTB.Text.Trim();
                    string noi_dung_en = localAPI.chuyenTiengVietKhongDau(noi_dung);
                    int so_tin = 0;
                    if (!string.IsNullOrEmpty(noi_dung_en)) so_tin = localAPI.demSoTin(noi_dung_en);

                    checkExists = tinNhanBO.checkExistsSms(Sys_This_Truong.ID, nam_gui, thang_gui, null, sdt, (is_checkHenGio && dt != null) ? dt : null, Sys_Time_Send, noi_dung_en);
                    if (!string.IsNullOrEmpty(loaiNhaMang) && so_tin > 0 & so_tin < 0 && checkExists == null)
                    {
                        TIN_NHAN tinDetail = new TIN_NHAN();
                        tinDetail.ID_TRUONG = Sys_This_Truong.ID;
                        tinDetail.ID_NGUOI_NHAN = id_customer;
                        tinDetail.LOAI_NGUOI_NHAN = 3;
                        tinDetail.SDT_NHAN = sdt;
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
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Bạn chưa chọn người nhận tin!');", true);
                return;
            }
            #endregion
            #region check quy tin
            if (lstTinNhan.Count > 0)
            {
                short nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc);
                short thang = Convert.ToInt16(DateTime.Now.Month);
                if (thang < 8) nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc + 1);
                if (Sys_This_Truong.ID_DOI_TAC == null || Sys_This_Truong.ID_DOI_TAC == 0)
                {
                    #region Tính quỹ tin
                    QUY_TIN quyTinTheoNam = quyTinBO.getQuyTinByNamHoc(Convert.ToInt16(localAPI.GetYearNow()), Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID);
                    bool is_insert_new_quytb = false;
                    QUY_TIN quyTinTheoThang = quyTinBO.getQuyTin(nam_hoc, thang, Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID, out is_insert_new_quytb);
                    double tong_con_thang = quyTinTheoThang.TONG_CON + ((quyTinTheoThang.TONG_CAP + quyTinTheoThang.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                / 100);
                    double tong_con_nam = quyTinTheoNam.TONG_CON + ((quyTinTheoNam.TONG_CAP + quyTinTheoNam.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                        / 100);
                    #endregion
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
                                logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi tin nhắn customer (" + tong_tin_gui + " tin)", Sys_User.ID, DateTime.Now);
                            if (tong_tin_gui >= tong_con_thang)
                            {
                                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Đơn vị đã sử dụng vượt quá quỹ tin tháng!');", true);
                            }
                        }

                    }
                    else
                    {
                        //if (tong_tin_gui > tong_con_thang || tong_tin_gui > tong_con_nam)
                        if (tong_tin_gui > tong_con_thang)
                        {
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị đã sử dụng vượt quá quỹ tin tháng!');", true);
                            return;
                        }
                        else
                        {
                            res = tinNhanBO.insertTinNhanThongBao(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, 0, Sys_User.ID);
                            if (res.Res)
                                logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi tin nhắn customer (" + tong_tin_gui + " tin)", Sys_User.ID, DateTime.Now);
                        }
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
                            res = tinNhanBO.insertTinNhanThongBao(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, 0, Sys_User.ID);
                            if (res.Res)
                                logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi tin nhắn customer (" + tong_tin_gui + " tin)", Sys_User.ID, DateTime.Now);
                        }
                    }
                }
            }
            #endregion
            #region Thông báo
            string strMsg = "";
            if (res.Res && tong_tin_gui > 0)
                strMsg = " notification('success', 'Có " + tong_tin_gui + " tin nhắn được gửi.');";
            else
                strMsg = " notification('warning', 'Không có tin nào được gửi đi!');";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            #endregion
            RadGrid1.Rebind();
            viewQuyTin();
        }
        protected void btnGuiAll_Click(object sender, EventArgs e)
        {
            #region "check quyen"
            if (!is_access(SYS_Type_Access.SEND_SMS))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            if (Sys_This_Truong.IS_ACTIVE_SMS != true)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị đang không đăng ký dịch vụ!');", true);
                return;
            }
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

            string noi_dung = tbNoiDung.Text.Trim();
            if (string.IsNullOrEmpty(noi_dung))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn phải nhập nội dung tin nhắn thông báo chung!');", true);
                tbNoiDung.Focus();
                return;
            }
            string noi_dung_en = localAPI.chuyenTiengVietKhongDau(noi_dung);
            int so_tin = localAPI.demSoTin(noi_dung_en);
            #endregion
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            List<TIN_NHAN> lstTinNhan = new List<TIN_NHAN>();
            int tong_tin_gui = 0;

            #region get list tin nhan
            List<DSGiaoVienTheoToEntity> lstCustomInTo = new List<DSGiaoVienTheoToEntity>();
            List<short> lst_ma_to = new List<short>();
            foreach (var item in rcbTo.CheckedItems)
            {
                lst_ma_to.Add(localAPI.ConvertStringToShort(item.Value).Value);
            }
            lstCustomInTo = customerToCustomer.getCustomerByTo(Sys_This_Truong.ID, lst_ma_to);

            TIN_NHAN checkExists = new TIN_NHAN();
            TIN_NHAN checkSms = new TIN_NHAN();
            short nam_gui = Convert.ToInt16(DateTime.Now.Year);
            short thang_gui = Convert.ToInt16(DateTime.Now.Month);
            short tuan_gui = Convert.ToInt16(localAPI.getThisWeek().ToString());
            string brandname = "", cp = "";

            foreach (DSGiaoVienTheoToEntity item in lstCustomInTo)
            {
                long id_customer = item.ID;
                string sdt = item.SDT;
                string loaiNhaMang = localAPI.getLoaiNhaMang(sdt);

                checkSms = tinNhanBO.checkExistsSms(Sys_This_Truong.ID, nam_gui, thang_gui, null, sdt, (is_checkHenGio && dt != null) ? dt : null, Sys_Time_Send, noi_dung_en);
                checkExists = lstTinNhan.FirstOrDefault(x => x.SDT_NHAN == sdt);
                if (!string.IsNullOrEmpty(loaiNhaMang) && checkExists == null && checkSms == null)
                {
                    TIN_NHAN tinDetail = new TIN_NHAN();
                    tinDetail.ID_TRUONG = Sys_This_Truong.ID;
                    tinDetail.ID_NGUOI_NHAN = id_customer;
                    tinDetail.LOAI_NGUOI_NHAN = 3;
                    tinDetail.SDT_NHAN = sdt;
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
                }
            }
            #endregion
            #region check quy tin
            if (lstTinNhan.Count > 0)
            {
                short nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc);
                short thang = Convert.ToInt16(DateTime.Now.Month);
                if (thang < 8) nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc + 1);
                if (Sys_This_Truong.ID_DOI_TAC == null || Sys_This_Truong.ID_DOI_TAC == 0)
                {
                    #region Tính quỹ tin
                    QUY_TIN quyTinTheoNam = quyTinBO.getQuyTinByNamHoc(Convert.ToInt16(localAPI.GetYearNow()), Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID);
                    bool is_insert_new_quytb = false;
                    QUY_TIN quyTinTheoThang = quyTinBO.getQuyTin(nam_hoc, thang, Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Thong_Bao, Sys_User.ID, out is_insert_new_quytb);

                    double tong_con_thang = quyTinTheoThang.TONG_CON + ((quyTinTheoThang.TONG_CAP + quyTinTheoThang.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                / 100);
                    double tong_con_nam = quyTinTheoNam.TONG_CON + ((quyTinTheoNam.TONG_CAP + quyTinTheoNam.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                        / 100);
                    #endregion
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
                                logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi tin nhắn customer (all " + tong_tin_gui + " tin)", Sys_User.ID, DateTime.Now);
                            if (tong_tin_gui >= tong_con_thang)
                            {
                                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Đơn vị đã sử dụng vượt quá quỹ tin tháng!');", true);
                            }
                        }

                    }
                    else
                    {
                        //if (tong_tin_gui > tong_con_thang || tong_tin_gui > tong_con_nam)
                        if (tong_tin_gui > tong_con_thang)
                        {
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Đơn vị đã sử dụng vượt quá quỹ tin tháng!');", true);
                            return;
                        }
                        else
                        {
                            res = tinNhanBO.insertTinNhanThongBao(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, 0, Sys_User.ID);
                            if (res.Res)
                                logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi tin nhắn customer (all " + tong_tin_gui + " tin)", Sys_User.ID, DateTime.Now);
                        }
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
                            res = tinNhanBO.insertTinNhanThongBao(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, 0, Sys_User.ID);
                            if (res.Res)
                                logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi tin nhắn customer (all " + tong_tin_gui + " tin)", Sys_User.ID, DateTime.Now);
                        }
                    }
                }
            }
            #endregion
            #region Thông báo
            string strMsg = "";
            if (res.Res && tong_tin_gui > 0)
                strMsg = " notification('success', 'Có " + tong_tin_gui + " tin nhắn được gửi.');";
            else
                strMsg = " notification('warning', 'Không có tin nào được gửi đi!');";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            #endregion
            RadGrid1.Rebind();
            viewQuyTin();
        }
        protected void rcbTo_ItemChecked(object sender, RadComboBoxItemEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void rcbTo_CheckAllCheck(object sender, RadComboBoxCheckAllCheckEventArgs e)
        {
            RadGrid1.Rebind();
        }
    }
}