using ClosedXML.Excel;
using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CMS.Lop
{
    public partial class Lop : AuthenticatePage
    {
        private LopBO lopBO = new LopBO();
        private LopMonBO lopMonBO = new LopMonBO();
        private LocalAPI localAPI = new LocalAPI();
        private GiaoVienBO gvBO = new GiaoVienBO();
        private KhoiBO khoiBO = new KhoiBO();
        private CaHocBO caHocBO = new CaHocBO();
        private ChuyenCanBO chuyenCanBO = new ChuyenCanBO();
        private DiemChiTietBO diemChiTietBO = new DiemChiTietBO();
        private DiemTongKetBO diemTongKetBO = new DiemTongKetBO();
        private HocSinhBO hocSinhBO = new HocSinhBO();
        LogUserBO logUserBO = new LogUserBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            btAddNew.Visible = is_access(SYS_Type_Access.THEM);
            btXetMacDinhMonToanTruong.Visible = is_access(SYS_Type_Access.THEM) || is_access(SYS_Type_Access.SUA);
            btLopNamMoi.Visible = Sys_User.IS_ROOT != true ? false : true;
            btDeleteChon.Visible = is_access(SYS_Type_Access.XOA);
            btExport.Visible = is_access(SYS_Type_Access.EXPORT);
            btDeleteByRoot.Visible = (Sys_User.IS_ROOT != true) ? false : true;
            if (!IsPostBack)
            {
                objKhoi.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                objKhoi.SelectParameters.Add("maLoaiLopGDTX", DbType.Int16, Sys_This_Lop_GDTX == null ? "" : Sys_This_Lop_GDTX.ToString());
                rcbKhoiHoc.DataBind();
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
            short? maKhoiHoc = localAPI.ConvertStringToShort(rcbKhoiHoc.SelectedValue);
            string txtSearch = txtLop.Text.Trim();
            short? maLoaiLopGDTX = (Sys_This_Cap_Hoc == "GDTX") ? Sys_This_Lop_GDTX : null;
            RadGrid1.DataSource = lopBO.getLopByTruongCapHocAndKhoi(Sys_This_Cap_Hoc, Sys_This_Truong.ID, maKhoiHoc, Convert.ToInt16(Sys_Ma_Nam_hoc), txtSearch, maLoaiLopGDTX);
            if (is_access(SYS_Type_Access.XEM) || is_access(SYS_Type_Access.SUA))
                RadGrid1.MasterTableView.Columns.FindByUniqueName("SUA").Visible = true;
            else RadGrid1.MasterTableView.Columns.FindByUniqueName("SUA").Visible = false;
            if (Sys_This_Cap_Hoc == "MN")
                RadGrid1.MasterTableView.Columns.FindByUniqueName("XET_MON").Visible = false;
            else RadGrid1.MasterTableView.Columns.FindByUniqueName("XET_MON").Visible = true;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
        protected void btDeleteChon_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.XOA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            int success = 0, error = 0; string lst_id = string.Empty;
            foreach (GridDataItem row in RadGrid1.SelectedItems)
            {
                try
                {
                    string maKhoiHoc = rcbKhoiHoc.SelectedValue;
                    long maLop = Convert.ToInt64(row.GetDataKeyValue("ID").ToString());
                    string ten = row["TEN"].Text;
                    try
                    {
                        List<CA_HOC> checkExistsLopCaHoc = caHocBO.getCaHocByLop(maLop);
                        if (checkExistsLopCaHoc != null && checkExistsLopCaHoc.Count > 0)
                        {
                            error++;
                            continue;
                        }
                        List<CHUYEN_CAN> checkExistsLopChuyenCan = chuyenCanBO.getChuyenCanByLop(maLop);
                        if (checkExistsLopChuyenCan != null && checkExistsLopChuyenCan.Count > 0)
                        {
                            error++;
                            continue;
                        }
                        List<DIEM_CHI_TIET> checkExistsLopDiemChiTiet = diemChiTietBO.getDiemChiTietByLop(maLop);
                        if (checkExistsLopDiemChiTiet != null && checkExistsLopDiemChiTiet.Count > 0)
                        {
                            error++;
                            continue;
                        }
                        List<DIEM_TONG_KET> checkExistsLopDiemTongKet = diemTongKetBO.getDiemTongKetByLop(maLop);
                        if (checkExistsLopDiemTongKet != null && checkExistsLopDiemTongKet.Count > 0)
                        {
                            error++;
                            continue;
                        }
                        List<HOC_SINH> checkExistsLopHocSinh = hocSinhBO.getHocSinhByLop(maLop);
                        if (checkExistsLopHocSinh != null && checkExistsLopHocSinh.Count > 0)
                        {
                            error++;
                            continue;
                        }
                        List<LOP_MON> checkExistsLopLopMon = lopMonBO.getLopMonByLop(maLop);
                        if (checkExistsLopLopMon != null && checkExistsLopLopMon.Count > 0)
                        {
                            error++;
                            continue;
                        }

                        ResultEntity res = lopBO.delete(maLop, Sys_User.ID, false, false);
                        lst_id += maLop + ":" + ten + ", ";
                        if (res.Res)
                        {
                            success++;
                            logUserBO.insert(Sys_This_Truong.ID, "DELETE", "Xóa lớp " + ten + " (" + maLop + ")", Sys_User.ID, DateTime.Now);
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
            RadGrid1.Rebind();
        }
        protected void btDeleteByRoot_Click(object sender, EventArgs e)
        {
            if (Sys_User.IS_ROOT != true)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            int success = 0, error = 0; string lst_id = string.Empty;
            foreach (GridDataItem row in RadGrid1.SelectedItems)
            {
                try
                {
                    string maKhoiHoc = rcbKhoiHoc.SelectedValue;
                    short maLop = Convert.ToInt16(row.GetDataKeyValue("ID").ToString());
                    string ten = row["TEN"].Text;
                    try
                    {
                        ResultEntity res = lopBO.delete(maLop, Sys_User.ID, true, true);
                        lst_id += maLop + ":" + ten + ", ";
                        if (res.Res)
                        {
                            success++;
                            logUserBO.insert(Sys_This_Truong.ID, "DELETE", "Xóa lớp " + ten + " (" + maLop + ") bởi admin", Sys_User.ID, DateTime.Now);
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
            RadGrid1.Rebind();
        }
        protected void rcbKhoiHoc_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void btTimKiem_Click(object sender, EventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            List<GIAO_VIEN> lstGV = gvBO.getGiaoVien(Sys_This_Truong.ID, "");
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                long? id_gv = localAPI.ConvertStringTolong(item["ID_GVCN"].Text);
                if (id_gv != null && lstGV.Count > 0)
                {
                    try
                    {
                        item["TEN_GVCN"].Text = lstGV.FirstOrDefault(x => x.ID == id_gv.Value).HO_TEN;
                    }
                    catch { }
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
            string newName = "Danh_sach_lop_hoc.xlsx";

            List<ExcelHeaderEntity> lstHeader = new List<ExcelHeaderEntity>();
            List<ExcelEntity> lstColumn = new List<ExcelEntity>();
            lstHeader.Add(new ExcelHeaderEntity { name = "STT", colM = 1, rowM = 1, width = 10 });
            foreach (GridColumn item in RadGrid1.MasterTableView.Columns)
            {
                #region add column
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TEN") && item.UniqueName == "TEN")
                {
                    DataColumn col = new DataColumn("TEN");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tên lớp", colM = 1, rowM = 1, width = 30 });
                    lstColumn.Add(new ExcelEntity { Name = "TEN", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TEN_KHOI") && item.UniqueName == "TEN_KHOI")
                {
                    DataColumn col = new DataColumn("TEN_KHOI");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Khối học", colM = 1, rowM = 1, width = 30 });
                    lstColumn.Add(new ExcelEntity { Name = "TEN_KHOI", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TEN_GVCN") && item.UniqueName == "TEN_GVCN")
                {
                    DataColumn col = new DataColumn("TEN_GVCN");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Giáo viên chủ nhiệm", colM = 1, rowM = 1, width = 30 });
                    lstColumn.Add(new ExcelEntity { Name = "TEN_GVCN", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                #endregion
            }
            RadGrid1.AllowPaging = false;
            GridItemCollection lstGrid = new GridItemCollection();
            if (RadGrid1.SelectedItems != null && RadGrid1.SelectedItems.Count > 0) lstGrid = RadGrid1.SelectedItems;
            else lstGrid = RadGrid1.Items;
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
            string tieuDe = "DANH SÁCH LỚP HỌC";
            string hocKyNamHoc = "Năm học: " + Sys_Ten_Nam_Hoc;
            listCell.Add(new ExcelHeaderEntity { name = tenSo, colM = 3, rowM = 1, colName = "A", rowIndex = 1, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tenPhong, colM = 3, rowM = 1, colName = "A", rowIndex = 2, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tieuDe, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 3, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            listCell.Add(new ExcelHeaderEntity { name = hocKyNamHoc, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 4, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            string nameFileOutput = newName;
            localAPI.ExportExcelDynamic(serverPath, path, newName, nameFileOutput, 1, listCell, rowHeaderStart, rowStart, colStart, colEnd, dt, lstHeader, lstColumn, true);
        }
        protected void btXetMacDinhMonToanTruong_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.THEM) && !is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            ResultEntity res = lopMonBO.XetMacDinhMonChoLopToanTruong(Sys_This_Truong.ID, Sys_Ma_Nam_hoc, Sys_This_Cap_Hoc, Sys_User.ID);
            string strMsg = "";
            if (!res.Res)
            {
                strMsg = "notification('error', 'Có Lỗi xảy ra');";
            }
            else
            {
                strMsg += " notification('success', 'Thực hiện thành công');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
        protected void btLopNamMoi_Click(object sender, EventArgs e)
        {
            if (Sys_User.IS_ROOT != true)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            ResultEntity res = lopBO.TaoLopNamMoiTheoNamCu(Sys_This_Truong.ID, Sys_Ma_Nam_hoc, Sys_This_Cap_Hoc, Sys_User.ID);
            string strMsg = "";
            if (!res.Res)
            {
                strMsg = "notification('error', 'Có Lỗi xảy ra');";
            }
            else
            {
                strMsg += " notification('success', 'Thực hiện thành công');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
        }
        protected void btXoaHocSinh_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.XOA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            int success = 0, error = 0;
            foreach (GridDataItem row in RadGrid1.SelectedItems)
            {
                long maLop = Convert.ToInt64(row.GetDataKeyValue("ID").ToString());
                List<HOC_SINH> lstHocSinhInLop = hocSinhBO.getHocSinhByLop(maLop);
                for (int i = 0; i < lstHocSinhInLop.Count; i++)
                {
                    long id_hoc_sinh = lstHocSinhInLop[i].ID;
                    ResultEntity res = hocSinhBO.delete(id_hoc_sinh, Sys_User.ID, true, true);
                    if (res.Res) success++;
                    else error++;
                }
            }

            string strMsg = "";
            if (success > 0 && error == 0)
            {
                strMsg += " notification('success', 'Có " + success + " học sinh được xóa.');";
            }
            else if (success > 0 && error > 0 ) strMsg = "notification('warning', 'Thành công: " + success + " bản ghi, lỗi: " + error + "bản ghi');";
            else if (error > 0 && success == 0) strMsg = "notification('error', 'Không có bản ghi nào được xóa');";
            else if (success == 0 && error == 0) strMsg = "notification('warning', 'Không có bản ghi nào được xóa');";

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
        }
    }
}