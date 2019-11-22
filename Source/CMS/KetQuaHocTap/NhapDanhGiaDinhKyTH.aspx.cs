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

namespace CMS.KetQuaHocTap
{
    public partial class NhapDanhGiaDinhKyTH : AuthenticatePage
    {
        LocalAPI localAPI = new LocalAPI();
        DanhGiaDinhKyTHBO dgdkTHBO = new DanhGiaDinhKyTHBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            btSave.Visible = is_access(SYS_Type_Access.THEM) || is_access(SYS_Type_Access.SUA);
            btExport.Visible = is_access(SYS_Type_Access.EXPORT);
            if (!IsPostBack)
            {
                objKhoiHoc.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                rcbKhoiHoc.DataBind();
                objLop.SelectParameters.Add("ma_cap_hoc", Sys_This_Cap_Hoc);
                objLop.SelectParameters.Add("idTruong", Sys_This_Truong.ID.ToString());
                objLop.SelectParameters.Add("maNamHoc", Sys_Ma_Nam_hoc.ToString());
                rcbLop.DataBind();
                objKyDGTH.SelectParameters.Add("id_hocKy", Sys_Hoc_Ky.ToString());
                rcbKyDGTH.DataBind();
                insertEmpty();
                cboNangLuc.Checked = true;
                RadGrid1.MasterTableView.Columns.FindByUniqueName("PC_CHCL").Display = false;
                RadGrid1.MasterTableView.Columns.FindByUniqueName("PC_TTTN").Display = false;
                RadGrid1.MasterTableView.Columns.FindByUniqueName("PC_TTKL").Display = false;
                RadGrid1.MasterTableView.Columns.FindByUniqueName("PC_DKYT").Display = false;
                RadGrid1.MasterTableView.Columns.FindByUniqueName("PC_NX").Display = false;
                RadGrid1.MasterTableView.Columns.FindByUniqueName("NX_GVCN").Display = false;
            }
        }
        public void insertEmpty()
        {
            long? id_lop = localAPI.ConvertStringTolong(rcbLop.SelectedValue);
            short? ma_khoi = localAPI.ConvertStringToShort(rcbKhoiHoc.SelectedValue);
            short? id_kydg = localAPI.ConvertStringToShort(rcbKyDGTH.SelectedValue);
            if (id_lop != null && ma_khoi != null && id_kydg != null)
                dgdkTHBO.insertEmpty(Convert.ToInt16(Sys_Ma_Nam_hoc), Sys_This_Truong.ID, ma_khoi.Value, id_lop.Value, id_kydg.Value, Sys_User.ID);
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            long? id_lop = localAPI.ConvertStringTolong(rcbLop.SelectedValue);
            short? ma_khoi = localAPI.ConvertStringToShort(rcbKhoiHoc.SelectedValue);
            short? id_kydg = localAPI.ConvertStringToShort(rcbKyDGTH.SelectedValue);
            RadGrid1.DataSource = dgdkTHBO.getDanhGiaDinhKyTHByTruongKhoiLopAndGiaiDoan(Convert.ToInt16(Sys_Ma_Nam_hoc), Sys_This_Truong.ID, ma_khoi, id_lop, id_kydg.Value);
        }
        protected void rcbKyDGTH_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            insertEmpty();
            RadGrid1.Rebind();
        }
        protected void rcbLop_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            insertEmpty();
            RadGrid1.Rebind();
        }
        protected void rcbKhoiHoc_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbLop.ClearSelection();
            rcbLop.Text = string.Empty;
            rcbLop.DataBind();
            insertEmpty();
            RadGrid1.Rebind();
        }
        protected void btSave_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.THEM) && !is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            long? id_lop = localAPI.ConvertStringTolong(rcbLop.SelectedValue);
            short? ma_khoi = localAPI.ConvertStringToShort(rcbKhoiHoc.SelectedValue);
            short? id_kydg = localAPI.ConvertStringToShort(rcbKyDGTH.SelectedValue);
            if (id_lop != null)
            {
                int success = 0; int count_change = 0;
                foreach (GridDataItem item in RadGrid1.Items)
                {
                    var detail = new DANH_GIA_DINH_KY_TH();
                    long id_Detail = localAPI.ConvertStringTolong(item.GetDataKeyValue("ID").ToString()).Value;
                    detail = dgdkTHBO.getDanhGiaDinhKyTHByID(id_Detail);
                    if (detail != null)
                    {
                        #region get control
                        TextBox tbNL_NX = (TextBox)item.FindControl("tbNL_NX");
                        TextBox tbNL_TPVTQ = (TextBox)item.FindControl("tbNL_TPVTQ");
                        TextBox tbNL_HT = (TextBox)item.FindControl("tbNL_HT");
                        TextBox tbNL_TGQVD = (TextBox)item.FindControl("tbNL_TGQVD");
                        TextBox tbPC_NX = (TextBox)item.FindControl("tbPC_NX");
                        TextBox tbPC_CHCL = (TextBox)item.FindControl("tbPC_CHCL");
                        TextBox tbPC_TTTN = (TextBox)item.FindControl("tbPC_TTTN");
                        TextBox tbPC_TTKL = (TextBox)item.FindControl("tbPC_TTKL");
                        TextBox tbPC_DKYT = (TextBox)item.FindControl("tbPC_DKYT");
                        TextBox tbNX_GVCN = (TextBox)item.FindControl("tbNX_GVCN");
                        HiddenField hdNL_NX = (HiddenField)item.FindControl("hdNL_NX");
                        HiddenField hdNL_TPVTQ = (HiddenField)item.FindControl("hdNL_TPVTQ");
                        HiddenField hdNL_HT = (HiddenField)item.FindControl("hdNL_HT");
                        HiddenField hdNL_TGQVD = (HiddenField)item.FindControl("hdNL_TGQVD");
                        HiddenField hdPC_NX = (HiddenField)item.FindControl("hdPC_NX");
                        HiddenField hdPC_CHCL = (HiddenField)item.FindControl("hdPC_CHCL");
                        HiddenField hdPC_TTTN = (HiddenField)item.FindControl("hdPC_TTTN");
                        HiddenField hdPC_TTKL = (HiddenField)item.FindControl("hdPC_TTKL");
                        HiddenField hdPC_DKYT = (HiddenField)item.FindControl("hdPC_DKYT");
                        HiddenField hdNX_GVCN = (HiddenField)item.FindControl("hdNX_GVCN");
                        #endregion
                        #region get value control
                        string NL_NX = tbNL_NX.Text.Trim();
                        string NL_TPVTQ = tbNL_TPVTQ.Text.Trim();
                        string NL_HT = tbNL_HT.Text.Trim();
                        string NL_TGQVD = tbNL_TGQVD.Text.Trim();
                        string PC_NX = tbPC_NX.Text.Trim();
                        string PC_CHCL = tbPC_CHCL.Text.Trim();
                        string PC_TTTN = tbPC_TTTN.Text.Trim();
                        string PC_TTKL = tbPC_TTKL.Text.Trim();
                        string PC_DKYT = tbPC_DKYT.Text.Trim();
                        string NX_GVCN = tbNX_GVCN.Text.Trim();
                        string NL_NX_old = hdNL_NX.Value;
                        string NL_TPVTQ_old = hdNL_TPVTQ.Value;
                        string NL_HT_old = hdNL_HT.Value;
                        string NL_TGQVD_old = hdNL_TGQVD.Value;
                        string PC_NX_old = hdPC_NX.Value;
                        string PC_CHCL_old = hdPC_CHCL.Value;
                        string PC_TTTN_old = hdPC_TTTN.Value;
                        string PC_TTKL_old = hdPC_TTKL.Value;
                        string PC_DKYT_old = hdPC_DKYT.Value;
                        string NX_GVCN_old = hdNX_GVCN.Value;
                        #endregion
                        #region set value detail
                        detail.NL_HT = NL_HT;
                        detail.NL_NX = NL_NX;
                        detail.NL_TGQVD = NL_TGQVD;
                        detail.NL_TPVTQ = NL_TPVTQ;
                        detail.PC_CHCL = PC_CHCL;
                        detail.PC_DKYT = PC_DKYT;
                        detail.PC_NX = PC_NX;
                        detail.PC_TTKL = PC_TTKL;
                        detail.PC_TTTN = PC_TTTN;
                        detail.NX_GVCN = NX_GVCN;
                        #endregion
                        if (NL_NX != NL_NX_old || NL_TPVTQ != NL_TPVTQ_old || NL_HT != NL_HT_old || NL_TGQVD != NL_TGQVD_old || PC_NX != PC_NX_old || PC_CHCL != PC_CHCL_old || PC_TTTN != PC_TTTN_old || PC_TTKL != PC_TTKL_old || PC_DKYT != PC_DKYT_old || NX_GVCN != NX_GVCN_old)
                        {
                            count_change++;
                            res = dgdkTHBO.update(detail, Sys_User);
                            if (res.Res)
                                success++;
                        }
                    }
                }
                string strMsg = "";
                if (count_change - success > 0)
                {
                    strMsg = "notification('error', 'Có " + (count_change - success) + " bản ghi chưa được lưu. Liên hệ với quản trị viên');";
                }
                if (success > 0)
                {
                    strMsg += " notification('success', 'Có " + success + " bản ghi được lưu.');";
                }
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Có lỗi xãy ra');", true);
            }
            RadGrid1.Rebind();
        }
        public void setView()
        {
            RadGrid1.MasterTableView.Columns.FindByUniqueName("NL_TPVTQ").Display = cboNangLuc.Checked;
            RadGrid1.MasterTableView.Columns.FindByUniqueName("NL_HT").Display = cboNangLuc.Checked;
            RadGrid1.MasterTableView.Columns.FindByUniqueName("NL_TGQVD").Display = cboNangLuc.Checked;
            RadGrid1.MasterTableView.Columns.FindByUniqueName("NL_NX").Display = cboNangLuc.Checked;
            RadGrid1.MasterTableView.Columns.FindByUniqueName("PC_CHCL").Display = cboPhamChat.Checked;
            RadGrid1.MasterTableView.Columns.FindByUniqueName("PC_TTTN").Display = cboPhamChat.Checked;
            RadGrid1.MasterTableView.Columns.FindByUniqueName("PC_TTKL").Display = cboPhamChat.Checked;
            RadGrid1.MasterTableView.Columns.FindByUniqueName("PC_DKYT").Display = cboPhamChat.Checked;
            RadGrid1.MasterTableView.Columns.FindByUniqueName("PC_NX").Display = cboPhamChat.Checked;
            RadGrid1.MasterTableView.Columns.FindByUniqueName("NX_GVCN").Display = cboGVCN.Checked;
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
            string newName = "Danh_gia_dinh_ky_TH.xlsx";

            List<ExcelHeaderEntity> lstHeader = new List<ExcelHeaderEntity>();
            List<ExcelEntity> lstColumn = new List<ExcelEntity>();
            lstHeader.Add(new ExcelHeaderEntity { name = "STT", colM = 1, rowM = 2, width = 10 });
            #region add column
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "MA_HS"))
            {
                DataColumn col = new DataColumn("MA_HS");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Mã HS", colM = 1, rowM = 2, width = 18 });
                lstColumn.Add(new ExcelEntity { Name = "MA_HS", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TEN_HS"))
            {
                DataColumn col = new DataColumn("TEN_HS");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Họ tên", colM = 1, rowM = 2, width = 30 });
                lstColumn.Add(new ExcelEntity { Name = "TEN_HS", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            #region "Năng lực"
            int indexNL = 0;
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "NL_TPVTQ") || localAPI.checkColumnShowInRadGrid(RadGrid1, "NL_HT") || localAPI.checkColumnShowInRadGrid(RadGrid1, "NL_TGQVD") || localAPI.checkColumnShowInRadGrid(RadGrid1, "NL_NX"))
            {
                lstHeader.Add(new ExcelHeaderEntity { name = "Năng lực", colM = 4, rowM = 1, width = 90 });
                indexNL = lstHeader.Count - 1;
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "NL_TPVTQ"))
                {
                    DataColumn col = new DataColumn("NL_TPVTQ");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "TPV, TQ", colM = 1, rowM = 1, width = 10, parentIndex = indexNL });
                    lstColumn.Add(new ExcelEntity { Name = "NL_TPVTQ", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "NL_HT"))
                {
                    DataColumn col = new DataColumn("NL_HT");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "HT", colM = 1, rowM = 1, width = 10, parentIndex = indexNL });
                    lstColumn.Add(new ExcelEntity { Name = "NL_HT", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "NL_TGQVD"))
                {
                    DataColumn col = new DataColumn("NL_TGQVD");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "TGQVD", colM = 1, rowM = 1, width = 10, parentIndex = indexNL });
                    lstColumn.Add(new ExcelEntity { Name = "NL_TGQVD", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "NL_NX"))
                {
                    DataColumn col = new DataColumn("NL_NX");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Nhận xét", colM = 1, rowM = 1, width = 60, parentIndex = indexNL });
                    lstColumn.Add(new ExcelEntity { Name = "NL_NX", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
            }
            #endregion
            #region "Phẩm chất"
            int indexPC = 0;
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "PC_CHCL") || localAPI.checkColumnShowInRadGrid(RadGrid1, "PC_TTTN") || localAPI.checkColumnShowInRadGrid(RadGrid1, "PC_TTKL") || localAPI.checkColumnShowInRadGrid(RadGrid1, "PC_DKYT") || localAPI.checkColumnShowInRadGrid(RadGrid1, "PC_NX"))
            {
                lstHeader.Add(new ExcelHeaderEntity { name = "Phẩm chất", colM = 5, rowM = 1, width = 100 });
                indexPC = lstHeader.Count - 1;
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "PC_CHCL"))
                {
                    DataColumn col = new DataColumn("PC_CHCL");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "CHCL", colM = 1, rowM = 1, width = 10, parentIndex = indexPC });
                    lstColumn.Add(new ExcelEntity { Name = "PC_CHCL", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "PC_TTTN"))
                {
                    DataColumn col = new DataColumn("PC_TTTN");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "TTTN", colM = 1, rowM = 1, width = 10, parentIndex = indexPC });
                    lstColumn.Add(new ExcelEntity { Name = "PC_TTTN", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "PC_TTKL"))
                {
                    DataColumn col = new DataColumn("PC_TTKL");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "TTKL", colM = 1, rowM = 1, width = 10, parentIndex = indexPC });
                    lstColumn.Add(new ExcelEntity { Name = "PC_TTKL", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "PC_DKYT"))
                {
                    DataColumn col = new DataColumn("PC_DKYT");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "DKYT", colM = 1, rowM = 1, width = 10, parentIndex = indexPC });
                    lstColumn.Add(new ExcelEntity { Name = "PC_DKYT", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "PC_NX"))
                {
                    DataColumn col = new DataColumn("PC_NX");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Nhận xét", colM = 1, rowM = 1, width = 60, parentIndex = indexPC });
                    lstColumn.Add(new ExcelEntity { Name = "PC_NX", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
            }
            #endregion
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "NX_GVCN"))
            {
                DataColumn col = new DataColumn("NX_GVCN");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "GVCN nhận xét", colM = 1, rowM = 2, width = 60 });
                lstColumn.Add(new ExcelEntity { Name = "NX_GVCN", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            #endregion
            RadGrid1.AllowPaging = false;
            foreach (GridDataItem item in RadGrid1.Items)
            {
                DataRow row = dt.NewRow();
                TextBox tbNL_NX = (TextBox)item.FindControl("tbNL_NX");
                TextBox tbNL_TPVTQ = (TextBox)item.FindControl("tbNL_TPVTQ");
                TextBox tbNL_HT = (TextBox)item.FindControl("tbNL_HT");
                TextBox tbNL_TGQVD = (TextBox)item.FindControl("tbNL_TGQVD");
                TextBox tbPC_NX = (TextBox)item.FindControl("tbPC_NX");
                TextBox tbPC_CHCL = (TextBox)item.FindControl("tbPC_CHCL");
                TextBox tbPC_TTTN = (TextBox)item.FindControl("tbPC_TTTN");
                TextBox tbPC_TTKL = (TextBox)item.FindControl("tbPC_TTKL");
                TextBox tbPC_DKYT = (TextBox)item.FindControl("tbPC_DKYT");
                TextBox tbNX_GVCN = (TextBox)item.FindControl("tbNX_GVCN");
                foreach (DataColumn col in dt.Columns)
                {
                    row[col.ColumnName] = item[col.ColumnName].Text.Replace("&nbsp;", " ");
                    if (col.ColumnName == "NL_NX") row[col.ColumnName] = tbNL_NX.Text.Trim().TrimEnd(',');
                    if (col.ColumnName == "NL_TPVTQ") row[col.ColumnName] = tbNL_TPVTQ.Text.Trim().TrimEnd(',');
                    if (col.ColumnName == "NL_HT") row[col.ColumnName] = tbNL_HT.Text.Trim().TrimEnd(',');
                    if (col.ColumnName == "NL_TGQVD") row[col.ColumnName] = tbNL_TGQVD.Text.Trim().TrimEnd(',');
                    if (col.ColumnName == "PC_NX") row[col.ColumnName] = tbPC_NX.Text.Trim().TrimEnd(',');
                    if (col.ColumnName == "PC_CHCL") row[col.ColumnName] = tbPC_CHCL.Text.Trim().TrimEnd(',');
                    if (col.ColumnName == "PC_TTTN") row[col.ColumnName] = tbPC_TTTN.Text.Trim().TrimEnd(',');
                    if (col.ColumnName == "PC_TTKL") row[col.ColumnName] = tbPC_TTKL.Text.Trim().TrimEnd(',');
                    if (col.ColumnName == "PC_DKYT") row[col.ColumnName] = tbPC_DKYT.Text.Trim().TrimEnd(',');
                    if (col.ColumnName == "NX_GVCN") row[col.ColumnName] = tbNX_GVCN.Text.Trim().TrimEnd(',');
                }
                dt.Rows.Add(row);
            }
            int rowHeaderStart = 6;
            int rowStart = 8;
            string colStart = "A";
            string colEnd = localAPI.GetExcelColumnName(lstColumn.Count);
            List<ExcelHeaderEntity> listCell = new List<ExcelHeaderEntity>();
            string tenSo = "";
            string tenPhong = Sys_This_Truong.TEN;
            string tieuDe = "ĐÁNH GIÁ ĐỊNH KỲ";
            string hocKyNamHoc = "Lớp " + (rcbLop.Text == "" ? "" : rcbLop.Text) + ", " + rcbKyDGTH.Text;
            listCell.Add(new ExcelHeaderEntity { name = tenSo, colM = 3, rowM = 1, colName = "A", rowIndex = 1, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tenPhong, colM = 3, rowM = 1, colName = "A", rowIndex = 2, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tieuDe, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 3, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            listCell.Add(new ExcelHeaderEntity { name = hocKyNamHoc, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 4, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            string nameFileOutput = newName;
            localAPI.ExportExcelDynamic(serverPath, path, newName, nameFileOutput, 1, listCell, rowHeaderStart, rowStart, colStart, colEnd, dt, lstHeader, lstColumn, true);
        }
        protected void cboNangLuc_CheckedChanged(object sender, EventArgs e)
        {
            setView();
        }
        protected void cboPhamChat_CheckedChanged(object sender, EventArgs e)
        {
            setView();
        }
        protected void cboGVCN_CheckedChanged(object sender, EventArgs e)
        {
            setView();
        }
    }
}