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

namespace CMS.HocPhi
{
    public partial class PhieuThuHocSinh : AuthenticatePage
    {
        LocalAPI localAPI = new LocalAPI();
        LogUserBO logUserBO = new LogUserBO();
        HocSinhBO hocSinhBO = new HocSinhBO();
        HocPhiDotThuBO dotThuBO = new HocPhiDotThuBO();
        HocPhiPhieuThuHocSinhBO phieuThuHocSinhBO = new HocPhiPhieuThuHocSinhBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            btAddNew.Visible = is_access(SYS_Type_Access.THEM);
            btDeleteChon.Visible = is_access(SYS_Type_Access.XOA);
            btExport.Visible = is_access(SYS_Type_Access.EXPORT);
            if (!IsPostBack)
            {
                objKhoi.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                objKhoi.SelectParameters.Add("maLoaiLopGDTX", DbType.Int16, Sys_This_Lop_GDTX == null ? "" : Sys_This_Lop_GDTX.ToString());
                rcbKhoi.DataBind();
                objLop.SelectParameters.Add("ma_cap_hoc", Sys_This_Cap_Hoc);
                objLop.SelectParameters.Add("idTruong", Sys_This_Truong.ID.ToString());
                objLop.SelectParameters.Add("maNamHoc", Sys_Ma_Nam_hoc.ToString());
                rcbLop.DataBind();
                objDotThu.SelectParameters.Add("id_truong", Sys_This_Truong.ID.ToString());
                objDotThu.SelectParameters.Add("id_nam_hoc", Sys_Ma_Nam_hoc.ToString());
                rcbDotThu.DataBind();
            }
        }
        protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
        {
            if (e.Argument == "RadWindowManager1_Close")
            {
                RadGrid1.Rebind();
                Session["KhoanThuTheoHocSinh" + Sys_User.ID] = null;
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = phieuThuHocSinhBO.getPhieuThuByLopAndDotThu(Sys_This_Truong.ID, Convert.ToInt16(Sys_Ma_Nam_hoc), localAPI.ConvertStringToShort(rcbKhoi.SelectedValue), localAPI.ConvertStringTolong(rcbLop.SelectedValue), localAPI.ConvertStringTolong(rcbDotThu.SelectedValue), tbGhiChu.Text.Trim());
            if (is_access(SYS_Type_Access.XEM) || is_access(SYS_Type_Access.SUA))
                RadGrid1.MasterTableView.Columns.FindByUniqueName("SUA").Visible = true;
            else RadGrid1.MasterTableView.Columns.FindByUniqueName("SUA").Visible = false;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
        protected void rcbDotThu_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void rcbKhoi_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbLop.ClearSelection();
            rcbLop.Text = string.Empty;
            rcbLop.DataBind();
            RadGrid1.Rebind();
        }
        protected void rcbLop_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
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
                    long id = Convert.ToInt16(row.GetDataKeyValue("ID").ToString());
                    string ten = row["TEN_HOC_SINH"].Text;
                    try
                    {
                        ResultEntity res = phieuThuHocSinhBO.delete(id, Sys_User.ID, true);
                        lst_id += id + ":" + ten + ", ";
                        if (res.Res)
                        {
                            success++;
                            logUserBO.insert(Sys_This_Truong.ID, "DELETE", "Xóa phiếu thu học sinh " + ten + " (id_phieu=" + id + ")", Sys_User.ID, DateTime.Now);
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
            string newName = "DanhSachPhieuThu.xlsx";

            List<ExcelHeaderEntity> lstHeader = new List<ExcelHeaderEntity>();
            List<ExcelEntity> lstColumn = new List<ExcelEntity>();
            lstHeader.Add(new ExcelHeaderEntity { name = "STT", colM = 1, rowM = 1, width = 10 });
            #region add column
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TEN_HOC_SINH"))
            {
                DataColumn col = new DataColumn("TEN_HOC_SINH");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Họ tên", colM = 1, rowM = 1, width = 80 });
                lstColumn.Add(new ExcelEntity { Name = "TEN_HOC_SINH", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "GHI_CHU"))
            {
                DataColumn col = new DataColumn("GHI_CHU");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Nội dung thu", colM = 1, rowM = 1, width = 30 });
                lstColumn.Add(new ExcelEntity { Name = "GHI_CHU", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_TIEN_AN"))
            {
                DataColumn col = new DataColumn("IS_TIEN_AN");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Đóng tiền ăn", colM = 1, rowM = 1, width = 30 });
                lstColumn.Add(new ExcelEntity { Name = "IS_TIEN_AN", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "SO_TIEN_AN"))
            {
                DataColumn col = new DataColumn("SO_TIEN_AN");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Tiền ăn (VNĐ)", colM = 1, rowM = 1, width = 30 });
                lstColumn.Add(new ExcelEntity { Name = "SO_TIEN_AN", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TONG_TIEN"))
            {
                DataColumn col = new DataColumn("TONG_TIEN");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Số tiền (VNĐ)", colM = 1, rowM = 1, width = 30 });
                lstColumn.Add(new ExcelEntity { Name = "TONG_TIEN", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            #endregion
            RadGrid1.AllowPaging = false;
            var lstGrid = RadGrid1.SelectedItems;
            if (lstGrid == null || lstGrid.Count == 0) lstGrid = RadGrid1.Items;
            foreach (GridDataItem item in lstGrid)
            {
                DataRow row = dt.NewRow();
                foreach (DataColumn col in dt.Columns)
                {
                    row[col.ColumnName] = item[col.ColumnName].Text.Replace("&nbsp;", " ");
                    if (col.ColumnName.IndexOf("IS_") == 0)
                    {
                        CheckBox check = (CheckBox)item[col.ColumnName].Controls[0];
                        row[col.ColumnName] = check.Checked;
                    }
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
            string tieuDe = "Danh sách phiếu thu" + (rcbLop.SelectedValue != "" ? " lớp " + rcbLop.Text : "");
            string hocKyNamHoc = "Năm học: " + Sys_Ten_Nam_Hoc;
            listCell.Add(new ExcelHeaderEntity { name = tenSo, colM = 3, rowM = 1, colName = "A", rowIndex = 1, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tenPhong, colM = 3, rowM = 1, colName = "A", rowIndex = 2, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tieuDe, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 3, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            listCell.Add(new ExcelHeaderEntity { name = hocKyNamHoc, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 4, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            string nameFileOutput = newName;
            localAPI.ExportExcelDynamic(serverPath, path, newName, nameFileOutput, 1, listCell, rowHeaderStart, rowStart, colStart, colEnd, dt, lstHeader, lstColumn, true);
        }
        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                long id_hoc_sinh = Convert.ToInt64(item["ID_HOC_SINH"].Text);
                HOC_SINH detail = hocSinhBO.getHocSinhByID(id_hoc_sinh);
                if (detail != null)
                    item["TEN_HOC_SINH"].Text = detail.HO_TEN;
                long id_dot_thu = Convert.ToInt64(item["ID_DOT_THU"].Text);
                item["TEN_DOT_THU"].Text = dotThuBO.getAllDotThuByTruong(Sys_This_Truong.ID, Convert.ToInt16(Sys_Ma_Nam_hoc)).ToList().Where(x => x.ID == id_dot_thu).FirstOrDefault().TEN;
            }
        }
    }
}