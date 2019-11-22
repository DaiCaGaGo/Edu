using ClosedXML.Excel;
using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CMS.MonHoc
{
    public partial class MonTruong : AuthenticatePage
    {
        MonHocTruongBO monTruongBO = new MonHocTruongBO();
        MonHocBO monBO = new MonHocBO();
        LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            btAddNew.Visible = is_access(SYS_Type_Access.THEM);
            btDeleteChon.Visible = is_access(SYS_Type_Access.XOA);
            if (!IsPostBack)
            {
                if (Sys_This_Cap_Hoc != SYS_Cap_Hoc.THPT)
                {
                    divMonChuyen.Visible = false;
                }
                if (Sys_This_Cap_Hoc != SYS_Cap_Hoc.GDTX)
                {
                    divLoaiLopGDTX.Visible = false;
                }
                else
                {
                    divLoaiLopGDTX.Visible = true;
                    if (Sys_This_Lop_GDTX != null && rcbLoaiLopGDTX.Items.FindItemByValue(Sys_This_Lop_GDTX.Value.ToString()) != null)
                        rcbLoaiLopGDTX.SelectedValue = Sys_This_Lop_GDTX.Value.ToString();
                }
                if (!IsPostBack)
                {
                    monTruongBO.insertEmptyMonHoc(Sys_This_Truong.ID, Convert.ToInt16(Sys_Ma_Nam_hoc), Sys_This_Cap_Hoc);
                }
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
            short? kieu_mon = localAPI.ConvertStringToShort(rcbKieuMon.SelectedValue);
            int mon_chuyen = 0;
            if (cbMonChuyen.Checked) mon_chuyen = 1;
            short? loai_lop_gdtx = localAPI.ConvertStringToShort(rcbLoaiLopGDTX.SelectedValue);
            if (Sys_This_Cap_Hoc != "GDTX") loai_lop_gdtx = null;
            RadGrid1.DataSource = monTruongBO.getListMonTruong(Sys_This_Truong.ID, Convert.ToInt16(Sys_Ma_Nam_hoc), Sys_This_Cap_Hoc, tbTen.Text.Trim(), kieu_mon, mon_chuyen, loai_lop_gdtx);
            if (Sys_This_Cap_Hoc == "TH")
            {
                for (int i = 6; i <= 12; i++)
                {
                    RadGrid1.Columns.FindByUniqueName("IS_" + i).Visible = false;
                }
                RadGrid1.Columns.FindByUniqueName("IS_MON_CHUYEN").Visible = false;
            }
            else if (Sys_This_Cap_Hoc == "THCS" || (loai_lop_gdtx != null && loai_lop_gdtx == 2))
            {
                for (int i = 1; i <= 12; i++)
                {
                    if (i < 6 || i > 9) RadGrid1.Columns.FindByUniqueName("IS_" + i).Visible = false;
                    else RadGrid1.Columns.FindByUniqueName("IS_" + i).Visible = true;
                }
                RadGrid1.Columns.FindByUniqueName("IS_MON_CHUYEN").Visible = false;
            }
            else if (Sys_This_Cap_Hoc == "THPT" || (loai_lop_gdtx != null && loai_lop_gdtx == 3))
            {
                for (int i = 1; i <= 12; i++)
                {
                    if (i < 10)
                    {
                        RadGrid1.Columns.FindByUniqueName("IS_" + i).Visible = false;
                        RadGrid1.Columns.FindByUniqueName("IS_MON_CHUYEN").Visible = false;
                    }
                    else
                    {
                        RadGrid1.Columns.FindByUniqueName("IS_" + i).Visible = true;
                        RadGrid1.Columns.FindByUniqueName("IS_MON_CHUYEN").Visible = true;
                    }
                }
                
            }
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
            int errorMon = 0;
            foreach (GridDataItem row in RadGrid1.SelectedItems)
            {
                try
                {
                    short id_mon_truong = Convert.ToInt16(row.GetDataKeyValue("ID").ToString());
                    HiddenField hdIdMonHoc = (HiddenField)row.FindControl("hdIdMonHoc");
                    short? id_mon = localAPI.ConvertStringToShort(hdIdMonHoc.Value);

                    string ten = row["TEN"].Text;
                    try
                    {
                        if (id_mon == null || id_mon == 0)
                        {
                            ResultEntity res = monTruongBO.delete(id_mon_truong, Sys_User.ID, true);
                            lst_id += id_mon_truong + ":" + ten + ", ";
                            if (res.Res)
                                success++;
                            else
                                error++;
                        }
                        else
                        {
                            errorMon++;
                        }
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
            if (errorMon > 0)
            {
                strMsg += "notification('error', 'Dữ liệu được bôi đỏ không thể sửa, xóa. Vui lòng kiểm tra lại.');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
        }
        protected void btTimKiem_Click(object sender, EventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                string kieu_mon = item["KIEU_MON"].Text;
                if (kieu_mon != null && kieu_mon != "")
                {
                    if (kieu_mon == "True")
                        item["KIEU_MON_STR"].Text = "Điểm nhận xét";
                    else item["KIEU_MON_STR"].Text = "Điểm số";
                }
                short? id_mon_hoc = localAPI.ConvertStringToShort(item["ID_MON_HOC"].Text);
                if (id_mon_hoc != null && id_mon_hoc != 0)
                {
                    item["chkChon"].Text = "";
                    item.ForeColor = Color.Red;
                    item["SUA"].Text = "";
                }
            }
        }
        protected void rcbKieuMon_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void cbMonChuyen_CheckedChanged(object sender, EventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void rcbLoaiLopGDTX_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
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
            string newName = "Mon_hoc.xlsx";

            List<ExcelHeaderEntity> lstHeader = new List<ExcelHeaderEntity>();
            List<ExcelEntity> lstColumn = new List<ExcelEntity>();
            lstHeader.Add(new ExcelHeaderEntity { name = "STT", colM = 1, rowM = 1, width = 10 });
            #region add column
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TEN"))
            {
                DataColumn col = new DataColumn("TEN");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "TEN", colM = 1, rowM = 1, width = 20 });
                lstColumn.Add(new ExcelEntity { Name = "TEN", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "KIEU_MON"))
            {
                DataColumn col = new DataColumn("KIEU_MON");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Kiểu môn", colM = 1, rowM = 1, width = 20 });
                lstColumn.Add(new ExcelEntity { Name = "KIEU_MON", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "HE_SO"))
            {
                DataColumn col = new DataColumn("HE_SO");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Hệ số", colM = 1, rowM = 1, width = 10 });
                lstColumn.Add(new ExcelEntity { Name = "HE_SO", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_MON_CHUYEN"))
            {
                DataColumn col = new DataColumn("IS_MON_CHUYEN");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Môn chuyên", colM = 1, rowM = 1, width = 15 });
                lstColumn.Add(new ExcelEntity { Name = "IS_MON_CHUYEN", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "check" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_1"))
            {
                DataColumn col = new DataColumn("IS_1");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Khối 1", colM = 1, rowM = 1, width = 15 });
                lstColumn.Add(new ExcelEntity { Name = "IS_1", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "check" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_2"))
            {
                DataColumn col = new DataColumn("IS_2");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Khối 2", colM = 1, rowM = 1, width = 15 });
                lstColumn.Add(new ExcelEntity { Name = "IS_2", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "check" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_3"))
            {
                DataColumn col = new DataColumn("IS_3");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Khối 3", colM = 1, rowM = 1, width = 15 });
                lstColumn.Add(new ExcelEntity { Name = "IS_3", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "check" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_4"))
            {
                DataColumn col = new DataColumn("IS_4");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Khối 4", colM = 1, rowM = 1, width = 15 });
                lstColumn.Add(new ExcelEntity { Name = "IS_4", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "check" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_5"))
            {
                DataColumn col = new DataColumn("IS_5");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Khối 5", colM = 1, rowM = 1, width = 15 });
                lstColumn.Add(new ExcelEntity { Name = "IS_5", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "check" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_6"))
            {
                DataColumn col = new DataColumn("IS_6");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Khối 6", colM = 1, rowM = 1, width = 15 });
                lstColumn.Add(new ExcelEntity { Name = "IS_6", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "check" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_7"))
            {
                DataColumn col = new DataColumn("IS_7");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Khối 7", colM = 1, rowM = 1, width = 15 });
                lstColumn.Add(new ExcelEntity { Name = "IS_7", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "check" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_8"))
            {
                DataColumn col = new DataColumn("IS_8");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Khối 8", colM = 1, rowM = 1, width = 15 });
                lstColumn.Add(new ExcelEntity { Name = "IS_8", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "check" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_9"))
            {
                DataColumn col = new DataColumn("IS_9");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Khối 9", colM = 1, rowM = 1, width = 15 });
                lstColumn.Add(new ExcelEntity { Name = "IS_9", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "check" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_10"))
            {
                DataColumn col = new DataColumn("IS_10");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Khối 10", colM = 1, rowM = 1, width = 15 });
                lstColumn.Add(new ExcelEntity { Name = "IS_10", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "check" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_11"))
            {
                DataColumn col = new DataColumn("IS_11");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Khối 11", colM = 1, rowM = 1, width = 15 });
                lstColumn.Add(new ExcelEntity { Name = "IS_11", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "check" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "IS_12"))
            {
                DataColumn col = new DataColumn("IS_12");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Khối 12", colM = 1, rowM = 1, width = 15 });
                lstColumn.Add(new ExcelEntity { Name = "IS_12", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "check" });
            }
            #endregion
            RadGrid1.AllowPaging = false;
            foreach (GridDataItem item in RadGrid1.Items)
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
            string tieuDe = "DANH SÁCH MÔN HỌC";
            string hocKyNamHoc = "Năm học: " + Sys_Ten_Nam_Hoc;
            listCell.Add(new ExcelHeaderEntity { name = tenSo, colM = 3, rowM = 1, colName = "A", rowIndex = 1, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tenPhong, colM = 3, rowM = 1, colName = "A", rowIndex = 2, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tieuDe, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 3, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            listCell.Add(new ExcelHeaderEntity { name = hocKyNamHoc, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 4, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            string nameFileOutput = newName;
            localAPI.ExportExcelDynamic(serverPath, path, newName, nameFileOutput, 1, listCell, rowHeaderStart, rowStart, colStart, colEnd, dt, lstHeader, lstColumn, true);
        }
    }
}