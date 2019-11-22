using ClosedXML.Excel;
using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CMS.DinhDuong
{
    public partial class ThucDon : AuthenticatePage
    {
        ThucDonBO thucDonBO = new ThucDonBO();
        DMBuaAnBO buaAnBO = new DMBuaAnBO();
        KhoiBO khoiBO = new KhoiBO();
        LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            if (!IsPostBack)
            {
                objKhoi.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                objKhoi.SelectParameters.Add("maLoaiLopGDTX", DbType.Int16, Sys_This_Lop_GDTX == null ? "" : Sys_This_Lop_GDTX.ToString());
                rcbKhoiHoc.DataBind();
                objBuaAn.SelectParameters.Add("id_truong", Sys_This_Truong.ID.ToString());
                rcbBuaAn.DataBind();
            }
        }
        protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
        {
            if (e.Argument == "RadWindowManager1_Close")
            {
                Session["TableThucPhamThucDon" + Sys_User.ID] = null;
                List<ThucDonChiTietEntity> lst = (List<ThucDonChiTietEntity>)Session["TableThucPhamThucDon" + Sys_User.ID];
                RadGrid1.Rebind();
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = thucDonBO.getThucDonByTruongKhoiNhomTuoi(Sys_This_Truong.ID, localAPI.ConvertStringToShort(rcbKhoiHoc.SelectedValue), null, tbTen.Text.Trim());
        }
        protected void btTimKiem_Click(object sender, EventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void btDeleteChon_Click(object sender, EventArgs e)
        {
            int success = 0, error = 0; string lst_id = string.Empty;
            foreach (GridDataItem row in RadGrid1.SelectedItems)
            {
                try
                {
                    short id = Convert.ToInt16(row.GetDataKeyValue("ID").ToString());
                    string ten = row["TEN"].Text;
                    try
                    {
                        ResultEntity res = thucDonBO.delete(id, Sys_User.ID, true);
                        lst_id += id + ":" + ten + ", ";
                        if (res.Res)
                            success++;
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
        protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                short? id_khoi = localAPI.ConvertStringToShort(item["ID_KHOI"].Text);
                if (id_khoi != null)
                    item["TEN_KHOI"].Text = khoiBO.getKhoiByCapHoc(Sys_This_Cap_Hoc).FirstOrDefault(x => x.MA == id_khoi).TEN;
                short? id_bua_an = localAPI.ConvertStringToShort(item["ID_BUA_AN"].Text);
                if (id_bua_an != null)
                    item["TEN_BUA_AN"].Text = buaAnBO.getBuaAnByTruongKhoi(Sys_This_Truong.ID, id_khoi).FirstOrDefault(x => x.ID == id_bua_an.Value).TEN;
            }
        }
        protected void rcbKhoiHoc_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbBuaAn.ClearSelection();
            rcbBuaAn.Text = String.Empty;
            rcbBuaAn.DataBind();
            RadGrid1.Rebind();
        }
        protected void rcbBuaAn_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
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
            string newName = "Danh_sach_thực đơn.xlsx";

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
                    lstHeader.Add(new ExcelHeaderEntity { name = "Tên thực đơn", colM = 1, rowM = 1, width = 60 });
                    lstColumn.Add(new ExcelEntity { Name = "TEN", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TEN_BUA_AN") && item.UniqueName == "TEN_BUA_AN")
                {
                    DataColumn col = new DataColumn("TEN_BUA_AN");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Bữa ăn", colM = 1, rowM = 1, width = 60 });
                    lstColumn.Add(new ExcelEntity { Name = "TEN_BUA_AN", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TEN_KHOI") && item.UniqueName == "TEN_KHOI")
                {
                    DataColumn col = new DataColumn("TEN_KHOI");
                    dt.Columns.Add(col);
                    lstHeader.Add(new ExcelHeaderEntity { name = "Khối", colM = 1, rowM = 1, width = 30 });
                    lstColumn.Add(new ExcelEntity { Name = "TEN_KHOI", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
                }
                #endregion
            }
            RadGrid1.AllowPaging = false;
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
            string tieuDe = "DANH SÁCH THỰC ĐƠN";
            string hocKyNamHoc = "";
            listCell.Add(new ExcelHeaderEntity { name = tenSo, colM = 3, rowM = 1, colName = "A", rowIndex = 1, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tenPhong, colM = 3, rowM = 1, colName = "A", rowIndex = 2, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tieuDe, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 3, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            listCell.Add(new ExcelHeaderEntity { name = hocKyNamHoc, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 4, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            string nameFileOutput = newName;
            localAPI.ExportExcelDynamic(serverPath, path, newName, nameFileOutput, 1, listCell, rowHeaderStart, rowStart, colStart, colEnd, dt, lstHeader, lstColumn, true);
        }
    }
}