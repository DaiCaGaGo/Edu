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

namespace CMS.NhanXetHangNgay
{
    public partial class NhanXetHangNgayHS : AuthenticatePage
    {
        NhanXetHangNgayBO nxhnBO = new NhanXetHangNgayBO();
        LopBO lopBO = new LopBO();
        GiaoVienBO giaoVienBO = new GiaoVienBO();
        QuyTinBO quyTinBO = new QuyTinBO();
        HocSinhBO hsBO = new HocSinhBO();
        TinNhanBO tinNhanBO = new TinNhanBO();
        public LocalAPI localAPI = new LocalAPI();
        TruongBO truongBO = new TruongBO();
        LogUserBO logUserBO = new LogUserBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            btSave.Visible = is_access(SYS_Type_Access.SUA);
            btnGuiLai.Visible = Sys_User.IS_ROOT == true ? true : false;
            btnGui.Visible = (is_access(SYS_Type_Access.SEND_SMS) && (Sys_This_Truong.IS_ACTIVE_SMS != null && Sys_This_Truong.IS_ACTIVE_SMS == true));
            btExport.Visible = is_access(SYS_Type_Access.EXPORT);
            if (!IsPostBack)
            {
                objKhoiHoc.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                rcbKhoi.DataBind();
                objLop.SelectParameters.Add("ma_cap_hoc", Sys_This_Cap_Hoc);
                objLop.SelectParameters.Add("idTruong", Sys_This_Truong.ID.ToString());
                objLop.SelectParameters.Add("maNamHoc", Sys_Ma_Nam_hoc.ToString());
                rcbLop.DataBind();
                if (rcbLop.SelectedValue != "") getGVCNLop(Convert.ToInt64(rcbLop.SelectedValue));
                rdNgay.SelectedDate = DateTime.Now;
                viewQuyTinCon();
            }
        }
        protected void getGVCNLop(long id_lop)
        {
            LOP lop = lopBO.getLopById(id_lop);
            if (lop != null)
            {
                if (lop.ID_GVCN != null)
                {
                    long id_giao_vien = lop.ID_GVCN.Value;
                    hdID_GVCN.Value = lop.ID_GVCN.Value.ToString();
                    GIAO_VIEN giaoVien = giaoVienBO.getGiaoVienByID(id_giao_vien);
                    if (giaoVien != null)
                    {
                        lblSDT_GV.Text = "(" + giaoVien.SDT + ")";
                        cboGuiGVCN.Enabled = true;
                    }
                }
                else
                {
                    lblSDT_GV.Text = "";
                    hdID_GVCN.Value = "";
                    cboGuiGVCN.Enabled = false;
                }
            }
        }
        protected void viewQuyTinCon()
        {
            btnGui.Visible = (is_access(SYS_Type_Access.SEND_SMS) && !(Sys_This_Truong.IS_ACTIVE_SMS != true));
            short nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc);
            short thang = Convert.ToInt16(DateTime.Now.Month);
            if (thang < 8) nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc + 1);
            if (Sys_This_Truong.ID_DOI_TAC == null || Sys_This_Truong.ID_DOI_TAC == 0)
            {
                QUY_TIN quyTinTheoNam = quyTinBO.getQuyTinByNamHoc(Convert.ToInt16(localAPI.GetYearNow()), Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Lien_Lac, Sys_User.ID);
                bool is_insert_new_quytb = false;
                QUY_TIN quyTinTheoThang = quyTinBO.getQuyTin(nam_hoc, thang, Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Lien_Lac, Sys_User.ID, out is_insert_new_quytb);
                if (quyTinTheoNam != null && quyTinTheoThang != null)
                {
                    double tong_con_thang = quyTinTheoThang.TONG_CON + (quyTinTheoThang.TONG_CAP + quyTinTheoThang.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                    / 100;
                    double tong_con_nam = quyTinTheoNam.TONG_CON + (quyTinTheoNam.TONG_CAP + quyTinTheoNam.TONG_THEM) * 1.0 * (Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC == null ? 0 : Sys_This_Truong.PHAN_TRAM_VUOT_HAN_MUC.Value)
                        / 100;
                    if (Sys_This_Truong.IS_ACTIVE_SMS != true || tong_con_nam <= 0)
                    {
                        btnGui.Visible = false;
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", " notification('warning', 'Đơn vị đã sử dụng vượt quỹ tin được cấp. Vui lòng liên hệ lại với quản trị viên!');", true);
                        return;
                    }
                    else if (Sys_This_Truong.IS_ACTIVE_SMS != true || tong_con_thang <= 0)
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", " notification('warning', 'Đơn vị đã sử dụng vượt quỹ tin tháng!');", true);
                    }
                    lblTongTinConNam.Text = "Quỹ năm còn: " + (quyTinTheoNam == null ? "0" : quyTinTheoNam.TONG_CON.ToString());
                    lblTongTinConThang.Text = "Quỹ tháng còn: " + (quyTinTheoThang == null ? "0" : (quyTinTheoThang.TONG_CON > quyTinTheoNam.TONG_CON) ? quyTinTheoNam.TONG_CON.ToString() : quyTinTheoThang.TONG_CON.ToString());
                }
                else
                {
                    btnGui.Visible = false;
                    lblTongTinConNam.Text = "Đơn vị không được cấp quota";
                    lblTongTinConThang.Visible = false;
                }
            }
            else
            {
                TRUONG detailTruong = new TRUONG();
                long tong_tin_con = 0;
                detailTruong = truongBO.getTruongById(Sys_This_Truong.ID);
                if (detailTruong != null)
                {
                    lblTongTinConNam.Text = "Tổng tin cấp: " + (detailTruong.TONG_TIN_CAP == null ? "0" : detailTruong.TONG_TIN_CAP.ToString());
                    if (detailTruong.TONG_TIN_CAP != null)
                    {
                        tong_tin_con = detailTruong.TONG_TIN_DA_DUNG != null ? (detailTruong.TONG_TIN_CAP.Value - detailTruong.TONG_TIN_DA_DUNG.Value) : detailTruong.TONG_TIN_CAP.Value;
                    }
                    else tong_tin_con = detailTruong.TONG_TIN_DA_DUNG != null ? (-detailTruong.TONG_TIN_DA_DUNG.Value) : 0;
                    lblTongTinConThang.Text = "Tổng tin còn: " + tong_tin_con;
                    if (Sys_This_Truong.IS_ACTIVE_SMS != true)
                    {
                        btnGui.Visible = false;
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", " notification('warning', 'Đơn vị chưa đăng ký sử dụng SMS nên không thể gửi tin nhắn!');", true);
                    }
                }
                if (tong_tin_con <= 0)
                {
                    btnGui.Visible = false;
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", " notification('warning', 'Đơn vị đã sử dụng hết quỹ tin được cấp. Vui lòng liên hệ lại với quản trị viên!');", true);
                }
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            long? id_lop = localAPI.ConvertStringTolong(rcbLop.SelectedValue);
            RadGrid1.DataSource = nxhnBO.getNXHNByTruongKhoiLopNgay(Sys_This_Truong.ID, localAPI.ConvertStringToShort(rcbKhoi.SelectedValue), id_lop, rdNgay.SelectedDate, Convert.ToInt16(Sys_Ma_Nam_hoc), Convert.ToInt16(Sys_Hoc_Ky));
            short? id_tien_to = 0;
            if (id_lop != null) id_tien_to = lopBO.getAllLop().FirstOrDefault(x => x.ID == id_lop.Value).LOAI_CHEN_TIN;
            if (id_tien_to == 1 || id_tien_to == 2) RadGrid1.Columns.FindByUniqueName("TIEN_TO").Display = true;
            else if (id_tien_to == 3 && !string.IsNullOrEmpty(lopBO.getAllLop().FirstOrDefault(x => x.ID == id_lop).TIEN_TO))
                RadGrid1.Columns.FindByUniqueName("TIEN_TO").Display = true;
            else RadGrid1.Columns.FindByUniqueName("TIEN_TO").Display = false;
            RadGrid1.Columns.FindByUniqueName("SDT_BM").Display = is_access(SYS_Type_Access.VIEW_INFOR);


            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
        protected void rcbKhoi_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbLop.ClearSelection();
            rcbLop.Text = String.Empty;
            rcbLop.DataBind();
            if (rcbLop.SelectedValue != "") getGVCNLop(Convert.ToInt64(rcbLop.SelectedValue));
            tbNoiDungChen.Text = "";
            RadGrid1.Rebind();
        }
        protected void rcbLop_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (rcbLop.SelectedValue != "") getGVCNLop(Convert.ToInt64(rcbLop.SelectedValue));
            tbNoiDungChen.Text = "";
            RadGrid1.Rebind();
        }
        protected void btSave_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            if (Convert.ToDateTime(rdNgay.SelectedDate).Date < DateTime.Now.Date || Convert.ToDateTime(rdNgay.SelectedDate).Date > DateTime.Now.Date)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không thể thực hiện thao tác này!');", true);
                return;
            }
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            short? ma_khoi = localAPI.ConvertStringToShort(rcbKhoi.SelectedValue);
            long? id_lop = localAPI.ConvertStringTolong(rcbLop.SelectedValue);
            NHAN_XET_HANG_NGAY detail = new NHAN_XET_HANG_NGAY();
            int success = 0; int count_change = 0; int insert = 0;
            GridItemCollection lstGrid = new GridItemCollection();
            if (RadGrid1.SelectedItems != null && RadGrid1.SelectedItems.Count > 0)
                lstGrid = RadGrid1.SelectedItems;
            else lstGrid = RadGrid1.Items;
            foreach (GridDataItem item in lstGrid)
            {
                long? id_nx = localAPI.ConvertStringTolong(item["ID"].Text);
                long id_hs = Convert.ToInt64(item.GetDataKeyValue("ID_HS").ToString());
                TextBox tbNoiDung = (TextBox)item.FindControl("tbNoiDung");
                HiddenField hdNoiDung = (HiddenField)item.FindControl("hdNoiDung");
                string str_noi_dung = tbNoiDung.Text.Trim();
                str_noi_dung = str_noi_dung.TrimEnd(',');
                string str_noi_dung_old = hdNoiDung.Value;
                if (id_nx != null)
                {
                    detail = nxhnBO.getNhanXetHangNgayByID(id_nx.Value);
                    detail.NOI_DUNG_NX = str_noi_dung;
                    if (str_noi_dung != str_noi_dung_old)
                    {
                        count_change++;
                        res = nxhnBO.update(detail, Sys_User.ID);
                        if (res.Res)
                        {
                            success++;
                            logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật NXHN HS " + detail.ID_HOC_SINH + ": " + str_noi_dung, Sys_User.ID, DateTime.Now);
                        }
                    }
                }
                else
                {
                    detail = new NHAN_XET_HANG_NGAY();
                    detail.NOI_DUNG_NX = str_noi_dung;
                    detail.ID_HOC_SINH = id_hs;
                    detail.ID_TRUONG = Sys_This_Truong.ID;
                    detail.ID_NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
                    detail.MA_KHOI = Convert.ToInt16(rcbKhoi.SelectedValue);
                    detail.ID_LOP = Convert.ToInt64(rcbLop.SelectedValue);
                    try
                    {
                        detail.NGAY_NX = rdNgay.SelectedDate.Value;
                    }
                    catch
                    {
                        detail.NGAY_NX = DateTime.Now;
                    }
                    if (str_noi_dung != "")
                    {
                        insert++;
                        res = nxhnBO.insert(detail, Sys_User.ID);
                        if (res.Res)
                        {
                            success++;
                            logUserBO.insert(Sys_This_Truong.ID, "INSERT", "Thêm mới NXHN HS " + detail.ID_HOC_SINH + ": " + str_noi_dung, Sys_User.ID, DateTime.Now);
                        }
                    }
                }

            }
            string strMsg = "";
            if (count_change + insert - success > 0)
            {
                strMsg = "notification('error', 'Có " + (count_change - success) + " bản ghi chưa được lưu. Liên hệ với quản trị viên');";
            }
            if (success > 0)
            {
                strMsg += " notification('success', 'Có " + success + " bản ghi được lưu.');";
            }

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
        }
        protected void rdNgay_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                TextBox tbNoiDung = (TextBox)item.FindControl("tbNoiDung");
                #region image trạng thái
                bool is_send = false;
                if (item["IS_SEND"].Text == "1") is_send = true;
                System.Web.UI.HtmlControls.HtmlImage image_chua_gui = (System.Web.UI.HtmlControls.HtmlImage)item.FindControl("chuaGui");
                System.Web.UI.HtmlControls.HtmlImage image_da_gui = (System.Web.UI.HtmlControls.HtmlImage)item.FindControl("daGui");
                if (is_send)
                {
                    image_chua_gui.Visible = false;
                    image_da_gui.Visible = true;
                    tbNoiDung.Enabled = false;
                }
                else
                {
                    image_chua_gui.Visible = true;
                    image_da_gui.Visible = false;
                    tbNoiDung.Enabled = true;
                }
                #endregion
                #region đỏ: không đk, xanh: Con gv, M: miễn, BM: đk cả bố mẹ
                short? is_bo_me = localAPI.ConvertStringToShort(item["IS_GUI_BO_ME"].Text);
                short? is_dk1 = localAPI.ConvertStringToShort(item["IS_DK_KY1"].Text);
                short? is_dk2 = localAPI.ConvertStringToShort(item["IS_DK_KY2"].Text);
                short? is_mien1 = localAPI.ConvertStringToShort(item["IS_MIEN_GIAM_KY1"].Text);
                short? is_mien2 = localAPI.ConvertStringToShort(item["IS_MIEN_GIAM_KY2"].Text);
                short? is_con_gv = localAPI.ConvertStringToShort(item["IS_CON_GV"].Text);
                if (is_con_gv != null && is_con_gv == 1) item.ForeColor = Color.Blue;
                if (Sys_Hoc_Ky == 1 && is_mien1 == 1) item["TEN_HS"].Text += " (*M)";
                else if (Sys_Hoc_Ky == 2 && is_mien2 == 1) item["TEN_HS"].Text += " (*M)";
                if (Sys_Hoc_Ky == 1 && (is_dk1 == null || is_dk1 == 0))
                {
                    item.ForeColor = Color.Red;
                    tbNoiDung.Enabled = false;
                }
                else if (Sys_Hoc_Ky == 2 && (is_dk2 == null || is_dk2 == 0))
                {
                    item.ForeColor = Color.Red;
                    tbNoiDung.Enabled = false;
                }
                if (Sys_Hoc_Ky == 1 && is_dk1 != null && is_dk1 == 1 && is_bo_me != null && is_bo_me == 1)
                    item["TEN_HS"].Text += " (*BM)";
                else if (Sys_Hoc_Ky == 2 && is_dk2 != null && is_dk2 == 1 && is_bo_me != null && is_bo_me == 1)
                    item["TEN_HS"].Text += " (*BM)";
                #endregion
                #region "SĐT"
                string sdt = item["SDT"].Text;
                if (sdt == "&nbsp;") sdt = "";
                string sdt_k = item["SDT_KHAC"].Text;
                if (sdt_k == "&nbsp;") sdt_k = "";
                bool is_gui_bo_me = false;
                if (item["IS_GUI_BO_ME"].Text == "1") is_gui_bo_me = true;
                string loai_nha_mang = localAPI.getLoaiNhaMang(sdt);
                string loai_nha_mang_k = localAPI.getLoaiNhaMang(sdt_k);
                if (is_gui_bo_me && !string.IsNullOrEmpty(sdt_k))
                {
                    item["SDT_BM"].Text = sdt + "; " + sdt_k;
                    item["MANG"].Text = loai_nha_mang + "; " + loai_nha_mang_k;
                }
                else
                {
                    item["SDT_BM"].Text = sdt;
                    item["MANG"].Text = loai_nha_mang;
                }
                #endregion
                #region bôi đỏ số bản tin > 1
                string tien_to = item["TIEN_TO"].Text;
                string noi_dung_nx = tbNoiDung.Text;
                int so_ky_tu = (tien_to != "&nbsp;" ? tien_to.Length : 0) + (!string.IsNullOrEmpty(noi_dung_nx) ? noi_dung_nx.Length : 0);
                if (so_ky_tu > 160)
                {
                    item["CountLength"].ForeColor = Color.Red;
                }
                else item["CountLength"].ForeColor = Color.Black;
                #endregion
                DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                if (rdNgay.SelectedDate != null && rdNgay.SelectedDate.Value < dt)
                {
                    tbNoiDung.Enabled = false;
                }
            }
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
            string newName = "Nhan_xet_hang_ngay.xlsx";

            List<ExcelHeaderEntity> lstHeader = new List<ExcelHeaderEntity>();
            List<ExcelEntity> lstColumn = new List<ExcelEntity>();
            lstHeader.Add(new ExcelHeaderEntity { name = "STT", colM = 1, rowM = 1, width = 10 });
            foreach (GridColumn item in RadGrid1.MasterTableView.Columns)
            {
                #region add column
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MA_HS") && item.UniqueName == "MA_HS")
                {
                    DataColumn col = new DataColumn("MA_HS");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Mã HS", colM = 1, rowM = 1, width = 18 });
                    lstColumn.Add(new ExcelEntity { Name = "MA_HS", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TEN_HS") && item.UniqueName == "TEN_HS")
                {
                    DataColumn col = new DataColumn("TEN_HS");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Họ tên", colM = 1, rowM = 1, width = 30 });
                    lstColumn.Add(new ExcelEntity { Name = "TEN_HS", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "SDT_BM") && item.UniqueName == "SDT_BM")
                {
                    DataColumn col = new DataColumn("SDT_BM");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "SĐT nhận tin", colM = 1, rowM = 1, width = 20 });
                    lstColumn.Add(new ExcelEntity { Name = "SDT_BM", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "NOI_DUNG_NX") && item.UniqueName == "NOI_DUNG_NX")
                {
                    DataColumn col = new DataColumn("NOI_DUNG_NX");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Nội dung", colM = 1, rowM = 1, width = 100 });
                    lstColumn.Add(new ExcelEntity { Name = "NOI_DUNG_NX", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                #endregion
            }
            RadGrid1.AllowPaging = false;
            var lstGrid = RadGrid1.SelectedItems;
            if (lstGrid == null || lstGrid.Count == 0)
                lstGrid = RadGrid1.Items;
            foreach (GridDataItem item in lstGrid)
            {
                DataRow row = dt.NewRow();
                TextBox tbNoiDung = (TextBox)item.FindControl("tbNoiDung");
                foreach (DataColumn col in dt.Columns)
                {
                    row[col.ColumnName] = item[col.ColumnName].Text.Replace("&nbsp;", " ");
                    if (col.ColumnName == "NOI_DUNG_NX") row[col.ColumnName] = tbNoiDung.Text.Trim().TrimEnd(',');
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
            string tieuDe = "NHẬN XÉT HÀNG NGÀY";
            string hocKyNamHoc = "Lớp " + (rcbLop.Text == "" ? "" : rcbLop.Text) + ", ngày " + rdNgay.SelectedDate.Value.ToString("dd/MM/yyyy");
            listCell.Add(new ExcelHeaderEntity { name = tenSo, colM = 3, rowM = 1, colName = "A", rowIndex = 1, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tenPhong, colM = 3, rowM = 1, colName = "A", rowIndex = 2, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tieuDe, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 3, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            listCell.Add(new ExcelHeaderEntity { name = hocKyNamHoc, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 4, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            string nameFileOutput = newName;
            localAPI.ExportExcelDynamic(serverPath, path, newName, nameFileOutput, 1, listCell, rowHeaderStart, rowStart, colStart, colEnd, dt, lstHeader, lstColumn, true);
        }
        protected void btnGuiLai_Click(object sender, EventArgs e)
        {
            #region check quyen
            if (Sys_User.IS_ROOT == null || (Sys_User.IS_ROOT != null && Sys_User.IS_ROOT == false))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện chức năng này.');", true);
                return;
            }
            if (Convert.ToDateTime(rdNgay.SelectedDate).Date < DateTime.Now.Date || Convert.ToDateTime(rdNgay.SelectedDate).Date > DateTime.Now.Date)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không thể thực hiện thao tác này!');", true);
                return;
            }
            #endregion
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            int success = 0;
            foreach (GridDataItem item in RadGrid1.SelectedItems)
            {
                long? id_nx = localAPI.ConvertStringTolong(item["ID"].Text);
                bool is_send = false;
                if (item["IS_SEND"].Text == "1") is_send = true;
                if (id_nx != null && is_send)
                {
                    res = nxhnBO.updateTrangThaiGui(id_nx.Value, Sys_User.ID);
                    if (res.Res)
                    {
                        success++;
                        logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật trạng thái gửi NXHN " + id_nx, Sys_User.ID, DateTime.Now);
                    }
                }
            }
            string strMsg = "";
            if (success == 0)
            {
                strMsg = "notification('error', 'Không có bản ghi nào được cập nhật trạng thái.');";
            }
            else strMsg = "notification('success', 'Có " + success + " bản ghi được cập nhật trạng thái gửi.');";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
            lblTongTinSuDung.Text = "";
            viewQuyTinCon();
        }
        protected void cbHenGioGuiTin_CheckedChanged(object sender, EventArgs e)
        {
            divTime.Visible = cbHenGioGuiTin.Checked;
        }
        protected void btnGui_Click(object sender, EventArgs e)
        {
            #region "check condition"
            if (!is_access(SYS_Type_Access.SEND_SMS))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            if (Convert.ToDateTime(rdNgay.SelectedDate).Date < DateTime.Now.Date || Convert.ToDateTime(rdNgay.SelectedDate).Date > DateTime.Now.Date)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không thể thực hiện thao tác này!');", true);
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
            short? ma_khoi = localAPI.ConvertStringToShort(rcbKhoi.SelectedValue);
            long? id_lop = localAPI.ConvertStringTolong(rcbLop.SelectedValue);
            NHAN_XET_HANG_NGAY nxhn = new NHAN_XET_HANG_NGAY();

            GridItemCollection lstGrid = new GridItemCollection();
            if (RadGrid1.SelectedItems != null && RadGrid1.SelectedItems.Count > 0)
                lstGrid = RadGrid1.SelectedItems;
            else lstGrid = RadGrid1.Items;

            string brandname = "", cp = "";
            short nam_gui = Convert.ToInt16(DateTime.Now.Year);
            short thang_gui = Convert.ToInt16(DateTime.Now.Month);
            short tuan_gui = Convert.ToInt16(localAPI.getThisWeek().ToString());
            int tong_tin_gui = 0;
            string listNXHN_ID = "";

            List<TIN_NHAN> lstTinNhan = new List<TIN_NHAN>();
            #region lấy list tin nhan từ grid
            foreach (GridDataItem item in lstGrid)
            {
                long? id_nx = localAPI.ConvertStringTolong(item["ID"].Text);
                long id_hs = Convert.ToInt64(item.GetDataKeyValue("ID_HS").ToString());

                bool is_gui_bo_me = false;
                if (item["IS_GUI_BO_ME"].Text == "1") is_gui_bo_me = true;
                bool is_send = false;
                if (item["IS_SEND"].Text == "1") is_send = true;

                #region "check dang ky sms"
                short? is_dk1 = localAPI.ConvertStringToShort(item["IS_DK_KY1"].Text);
                short? is_dk2 = localAPI.ConvertStringToShort(item["IS_DK_KY2"].Text);
                bool is_sms = false;
                if (is_dk1 != null && is_dk1 == 1 && Sys_Hoc_Ky == 1) is_sms = true;
                else if (is_dk2 != null && is_dk2 == 1 && Sys_Hoc_Ky == 2) is_sms = true;
                #endregion

                string tien_to = item["TIEN_TO"].Text;
                if (tien_to == "&nbsp;") tien_to = "";

                TextBox tbNoiDung = (TextBox)item.FindControl("tbNoiDung");
                string noi_dung_nx = tbNoiDung.Text.Trim();
                noi_dung_nx = noi_dung_nx.TrimEnd(',');

                string sdt = item["SDT"].Text;
                string telco = localAPI.getLoaiNhaMang(sdt);

                #region update nhan xet hang ngay
                if (id_nx != null && is_send != true)
                {
                    nxhn = nxhnBO.getNhanXetHangNgayByID(id_nx.Value);
                    nxhn.NOI_DUNG_NX = noi_dung_nx;
                    res = nxhnBO.update(nxhn, Sys_User.ID);
                }
                #endregion
                #region them moi NXHN
                else if (id_nx == null && !string.IsNullOrEmpty(noi_dung_nx))
                {
                    nxhn = new NHAN_XET_HANG_NGAY();
                    nxhn.NOI_DUNG_NX = localAPI.chuyenTiengVietKhongDau(noi_dung_nx);
                    nxhn.ID_HOC_SINH = id_hs;
                    nxhn.ID_TRUONG = Sys_This_Truong.ID;
                    nxhn.ID_NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
                    nxhn.MA_KHOI = ma_khoi.Value;
                    nxhn.ID_LOP = id_lop.Value;
                    try
                    {
                        nxhn.NGAY_NX = rdNgay.SelectedDate.Value;
                    }
                    catch
                    {
                        nxhn.NGAY_NX = DateTime.Now;
                    }
                    res = nxhnBO.insert(nxhn, Sys_User.ID);
                    nxhn = (NHAN_XET_HANG_NGAY)res.ResObject;
                    id_nx = nxhn.ID;
                }
                #endregion

                #region add list tin nhan
                if (id_nx != null && !string.IsNullOrEmpty(noi_dung_nx) && !string.IsNullOrEmpty(telco) && is_sms && is_send != true)
                {
                    string noi_dung_check = localAPI.chuyenTiengVietKhongDau(noi_dung_nx);

                    listNXHN_ID += id_nx + ",";
                    if (Sys_This_Truong.ID != 209)
                        noi_dung_nx = !string.IsNullOrEmpty(tien_to) ? (tien_to + " " + noi_dung_nx) : noi_dung_nx;
                    string noi_dung_en = localAPI.chuyenTiengVietKhongDau(noi_dung_nx);
                    int so_tin = localAPI.demSoTin(noi_dung_en);

                    TIN_NHAN checkExists = new TIN_NHAN();
                    checkExists = tinNhanBO.checkExistsSms(Sys_This_Truong.ID, nam_gui, thang_gui, null, sdt, (is_checkHenGio && dt != null) ? dt : null, Sys_Time_Send, noi_dung_check);
                    if (checkExists == null)
                    {
                        #region tin 1
                        TIN_NHAN tinDetail = new TIN_NHAN();
                        tinDetail.ID_TRUONG = Sys_This_Truong.ID;
                        tinDetail.ID_NGUOI_NHAN = id_hs;
                        tinDetail.LOAI_NGUOI_NHAN = 1;
                        tinDetail.SDT_NHAN = sdt;
                        tinDetail.MA_GOI_TIN = Sys_This_Truong.MA_GOI_TIN;
                        tinDetail.THOI_GIAN_GUI = (is_checkHenGio && dt != null) ? dt : DateTime.Now;
                        tinDetail.NGUOI_GUI = Sys_User.ID;
                        tinDetail.ID_NHAN_XET_HANG_NGAY = id_nx;
                        tinDetail.LOAI_TIN = SYS_Loai_Tin.Tin_Lien_Lac;
                        tinDetail.NAM_GUI = nam_gui;
                        tinDetail.THANG_GUI = thang_gui;
                        tinDetail.TUAN_GUI = tuan_gui;
                        tinDetail.NOI_DUNG = noi_dung_nx;
                        tinDetail.NOI_DUNG_KHONG_DAU = noi_dung_en;
                        tinDetail.SO_TIN = so_tin;
                        tinDetail.LOAI_NHA_MANG = telco;
                        brandname = ""; cp = "";
                        localAPI.getBrandnameAndCp(Sys_This_Truong, telco, out brandname, out cp);
                        tinDetail.BRAND_NAME = brandname;
                        tinDetail.CP = cp;
                        if (Sys_This_Truong.ID_DOI_TAC != null && Sys_This_Truong.ID_DOI_TAC > 0)
                            tinDetail.ID_DOI_TAC = Sys_This_Truong.ID_DOI_TAC;
                        else tinDetail.ID_DOI_TAC = null;
                        lstTinNhan.Add(tinDetail);
                        tong_tin_gui += so_tin;
                        #endregion
                        #region tin 2
                        if (is_gui_bo_me)
                        {
                            string sdt_k = item["SDT_KHAC"].Text;
                            string telco_k = localAPI.getLoaiNhaMang(sdt_k);
                            if (!string.IsNullOrEmpty(telco_k))
                            {
                                TIN_NHAN tinDetailK = new TIN_NHAN()
                                {
                                    ID_DOI_TAC = tinDetail.ID_DOI_TAC,
                                    ID_NGUOI_NHAN = tinDetail.ID_NGUOI_NHAN,
                                    ID_NHAN_XET_HANG_NGAY = tinDetail.ID_NHAN_XET_HANG_NGAY,
                                    ID_THONG_BAO = tinDetail.ID_THONG_BAO,
                                    ID_TONG_HOP_NXHN = tinDetail.ID_TONG_HOP_NXHN,
                                    ID_TRUONG = tinDetail.ID_TRUONG,
                                    IS_DA_NHAN = tinDetail.IS_DA_NHAN,
                                    IS_UNICODE = tinDetail.IS_UNICODE,
                                    KIEU_GUI = tinDetail.KIEU_GUI,
                                    LOAI_NGUOI_NHAN = tinDetail.LOAI_NGUOI_NHAN,
                                    LOAI_NHA_MANG = tinDetail.LOAI_NHA_MANG,
                                    MA_GOI_TIN = tinDetail.MA_GOI_TIN,
                                    NOI_DUNG = tinDetail.NOI_DUNG,
                                    NOI_DUNG_KHONG_DAU = tinDetail.NOI_DUNG_KHONG_DAU,
                                    SO_TIN = tinDetail.SO_TIN,
                                    THOI_GIAN_GUI = tinDetail.THOI_GIAN_GUI,
                                    NGUOI_GUI = tinDetail.NGUOI_GUI,
                                    LOAI_TIN = tinDetail.LOAI_TIN,
                                    NAM_GUI = tinDetail.NAM_GUI,
                                    SEND_NUMBER = tinDetail.SEND_NUMBER,
                                    THANG_GUI = tinDetail.THANG_GUI,
                                    TUAN_GUI = tinDetail.TUAN_GUI
                                };
                                tinDetailK.SDT_NHAN = sdt_k;
                                tinDetailK.LOAI_NHA_MANG = telco_k;
                                brandname = ""; cp = "";
                                localAPI.getBrandnameAndCp(Sys_This_Truong, telco_k, out brandname, out cp);
                                tinDetailK.BRAND_NAME = brandname;
                                tinDetailK.CP = cp;
                                lstTinNhan.Add(tinDetailK);
                                tong_tin_gui += so_tin;
                            }
                        }
                        #endregion
                    }
                }
                #endregion
            }
            if (!string.IsNullOrEmpty(listNXHN_ID))
            {
                listNXHN_ID = listNXHN_ID.TrimEnd(',');
                listNXHN_ID = "(" + listNXHN_ID + ")";
            }
            #endregion

            #region gui GVCN
            string sdt_gv = lblSDT_GV.Text.Trim();
            sdt_gv = sdt_gv.TrimStart('(').TrimEnd(')');
            sdt_gv = localAPI.Add84(sdt_gv);
            string telco_gv = localAPI.getLoaiNhaMang(sdt_gv);
            string noi_dung_gv = tbNoiDungChen.Text.Trim();
            string noi_dung_gv_end = localAPI.chuyenTiengVietKhongDau(noi_dung_gv);
            int so_tin_gv = localAPI.demSoTin(noi_dung_gv_end);


            if (cboGuiGVCN.Checked && !string.IsNullOrEmpty(sdt_gv) && !string.IsNullOrEmpty(noi_dung_gv_end) && lstTinNhan.Count > 0)
            {
                TIN_NHAN checkSmsGV = new TIN_NHAN();
                checkSmsGV = tinNhanBO.checkExistsSms(Sys_This_Truong.ID, nam_gui, thang_gui, null, sdt_gv, (is_checkHenGio && dt != null) ? dt : null, Sys_Time_Send, noi_dung_gv_end);
                LOP lop = lopBO.getLopById(id_lop.Value);
                if (checkSmsGV == null && lop != null)
                {
                    TIN_NHAN tinNhanDetail = new TIN_NHAN();
                    tinNhanDetail.ID_TRUONG = Sys_This_Truong.ID;
                    tinNhanDetail.ID_NGUOI_NHAN = lop.ID_GVCN;
                    tinNhanDetail.LOAI_NGUOI_NHAN = 2;
                    tinNhanDetail.SDT_NHAN = sdt_gv;
                    tinNhanDetail.MA_GOI_TIN = Sys_This_Truong.MA_GOI_TIN;
                    tinNhanDetail.THOI_GIAN_GUI = (is_checkHenGio && dt != null) ? dt : DateTime.Now;
                    tinNhanDetail.NGUOI_GUI = Sys_User.ID;
                    tinNhanDetail.LOAI_TIN = SYS_Loai_Tin.Tin_Lien_Lac;
                    tinNhanDetail.NAM_GUI = nam_gui;
                    tinNhanDetail.THANG_GUI = thang_gui;
                    tinNhanDetail.TUAN_GUI = tuan_gui;
                    tinNhanDetail.NOI_DUNG = noi_dung_gv;
                    tinNhanDetail.NOI_DUNG_KHONG_DAU = noi_dung_gv_end;
                    tinNhanDetail.SO_TIN = so_tin_gv;
                    tinNhanDetail.LOAI_NHA_MANG = telco_gv;
                    brandname = ""; cp = "";
                    localAPI.getBrandnameAndCp(Sys_This_Truong, telco_gv, out brandname, out cp);
                    tinNhanDetail.BRAND_NAME = brandname;
                    tinNhanDetail.CP = cp;
                    lstTinNhan.Add(tinNhanDetail);
                    tong_tin_gui += so_tin_gv;
                }
            }
            #endregion

            #region save sms
            if (lstTinNhan.Count > 0)
            {
                short nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc);
                short thang = Convert.ToInt16(DateTime.Now.Month);
                if (thang < 8) nam_hoc = Convert.ToInt16(Sys_Ma_Nam_hoc + 1);
                if (Sys_This_Truong.ID_DOI_TAC == null || Sys_This_Truong.ID_DOI_TAC == 0)
                {
                    #region Tính quỹ tin
                    QUY_TIN quyTinTheoNam = quyTinBO.getQuyTinByNamHoc(Convert.ToInt16(localAPI.GetYearNow()), Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Lien_Lac, Sys_User.ID);
                    bool is_insert_new_quytb = false;
                    QUY_TIN quyTinTheoThang = quyTinBO.getQuyTin(nam_hoc, thang, Sys_This_Truong.ID, SYS_Loai_Tin.Tin_Lien_Lac, Sys_User.ID, out is_insert_new_quytb);
                    if (quyTinTheoNam != null && quyTinTheoThang != null)
                    {
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
                                res = tinNhanBO.insertTinNXHN(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, Sys_User.ID, tong_tin_gui, listNXHN_ID);
                                if (res.Res)
                                {
                                    logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi nhận xét hàng ngày: gửi toàn danh sách " + tong_tin_gui + " tin nhắn", Sys_User.ID, DateTime.Now);
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
                                res = tinNhanBO.insertTinNXHN(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, Sys_User.ID, tong_tin_gui, listNXHN_ID);
                                if (res.Res)
                                {
                                    logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi nhận xét hàng ngày: gửi toàn danh sách " + tong_tin_gui + " tin nhắn", Sys_User.ID, DateTime.Now);
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
                            res = tinNhanBO.insertTinNXHN(lstTinNhan, Sys_This_Truong.ID, nam_hoc, thang, Sys_User.ID, tong_tin_gui, listNXHN_ID);
                            if (res.Res)
                            {
                                logUserBO.insert(Sys_This_Truong.ID, "SMS", "Gửi nhận xét hàng ngày: gửi toàn danh sách " + tong_tin_gui + " tin nhắn", Sys_User.ID, DateTime.Now);
                            }
                        }
                    }
                }
            }
            #endregion

            string strMsg = "";
            if (res.Res && tong_tin_gui > 0)
                strMsg = " notification('success', 'Có " + tong_tin_gui + " tin nhắn được gửi.');";
            else
                strMsg = " notification('warning', 'Không có tin nào được gửi đi!');";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
            lblTongTinSuDung.Text = "Số tin vừa gửi: <b>" + tong_tin_gui + "</b>";
            viewQuyTinCon();
        }
    }
}