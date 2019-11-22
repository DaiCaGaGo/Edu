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

namespace CMS.CauHinhCaHoc
{
    public partial class PhanLichDay : AuthenticatePage
    {
        CaHocBO caHocBO = new CaHocBO();
        LocalAPI localAPI = new LocalAPI();
        CauHinhCaHocBO chchBO = new CauHinhCaHocBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            btnLuu.Visible = is_access(SYS_Type_Access.SUA);
            btDeleteChon.Visible = is_access(SYS_Type_Access.THEM);
            btExport.Visible = is_access(SYS_Type_Access.EXPORT);
            if (!IsPostBack)
            {
                objKhoiHoc.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                objKhoiHoc.SelectParameters.Add("maLoaiLopGDTX", Sys_This_Lop_GDTX == null ? "" : Sys_This_Lop_GDTX.ToString());
                rcbKhoi.DataBind();
                objLop.SelectParameters.Add("ma_cap_hoc", Sys_This_Cap_Hoc);
                objLop.SelectParameters.Add("idTruong", Sys_This_Truong.ID.ToString());
                objLop.SelectParameters.Add("maNamHoc", Sys_Ma_Nam_hoc.ToString());
                rcbLop.DataBind();
                rcbHocKy.DataBind();
                rcbHocKy.SelectedValue = Sys_Hoc_Ky.ToString();
                
                objMon.SelectParameters.Add("id_hocKy", rcbHocKy.SelectedValue);

                objCaHoc.SelectParameters.Add("idTruong", Sys_This_Truong.ID.ToString());
                rcbCauHinhCaHoc.DataBind();

                //Thêm dữ liệu của trường vào bảng ca học
                insertEmpty();
            }
        }
        public void insertEmpty()
        {
            CAU_HINH_CA_HOC chch = new CAU_HINH_CA_HOC();
            long? id_cau_hinh = chchBO.getIDCauHinhCaHocByTruongHocKy(Sys_This_Truong.ID, localAPI.ConvertStringToShort(rcbHocKy.SelectedValue).Value);
            if (id_cau_hinh != null)
            {
                chch = chchBO.getCauHinhCaHocByID(id_cau_hinh.Value);
                rcbCauHinhCaHoc.SelectedValue = id_cau_hinh.Value.ToString();
            }

            caHocBO.insertFirstData(Sys_This_Truong.ID, Convert.ToInt16(rcbHocKy.SelectedValue), Convert.ToInt16(Sys_Ma_Nam_hoc), chch);
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
            long? id_lop = localAPI.ConvertStringTolong(rcbLop.SelectedValue);
            List<CA_HOC> lst = caHocBO.getCaHocByLopAndHocKy(Sys_This_Truong.ID, id_lop, localAPI.ConvertStringToShort(rcbHocKy.SelectedValue).Value);
            RadGrid1.DataSource = lst;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
        protected void btDeleteChon_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.XOA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            ResultEntity res = caHocBO.updateMonHocNull(localAPI.ConvertStringTolong(rcbLop.SelectedValue).Value, localAPI.ConvertStringToShort(rcbHocKy.SelectedValue).Value);
            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                RadComboBox rcbID_MON_2 = (RadComboBox)e.Item.FindControl("rcbID_MON_2");
                HiddenField hdID_MON_2 = (HiddenField)e.Item.FindControl("hdID_MON_2");
                long? id_ID_MON_2 = null;
                id_ID_MON_2 = localAPI.ConvertStringTolong(hdID_MON_2.Value);

                if (hdID_MON_2 != null && rcbID_MON_2.Items.FindItemByValue(id_ID_MON_2.ToString()) != null)
                {
                    rcbID_MON_2.SelectedValue = id_ID_MON_2.ToString();
                }

                RadComboBox rcbID_MON_3 = (RadComboBox)e.Item.FindControl("rcbID_MON_3");
                HiddenField hdID_MON_3 = (HiddenField)e.Item.FindControl("hdID_MON_3");
                long? id_ID_MON_3 = null;
                id_ID_MON_3 = localAPI.ConvertStringTolong(hdID_MON_3.Value);

                if (hdID_MON_3 != null && rcbID_MON_3.Items.FindItemByValue(id_ID_MON_3.ToString()) != null)
                {
                    rcbID_MON_3.SelectedValue = id_ID_MON_3.ToString();
                }

                RadComboBox rcbID_MON_4 = (RadComboBox)e.Item.FindControl("rcbID_MON_4");
                HiddenField hdID_MON_4 = (HiddenField)e.Item.FindControl("hdID_MON_4");
                long? id_ID_MON_4 = null;
                id_ID_MON_4 = localAPI.ConvertStringTolong(hdID_MON_4.Value);

                if (hdID_MON_4 != null && rcbID_MON_4.Items.FindItemByValue(id_ID_MON_4.ToString()) != null)
                {
                    rcbID_MON_4.SelectedValue = id_ID_MON_4.ToString();
                }

                RadComboBox rcbID_MON_5 = (RadComboBox)e.Item.FindControl("rcbID_MON_5");
                HiddenField hdID_MON_5 = (HiddenField)e.Item.FindControl("hdID_MON_5");
                long? id_ID_MON_5 = null;
                id_ID_MON_5 = localAPI.ConvertStringTolong(hdID_MON_5.Value);

                if (hdID_MON_5 != null && rcbID_MON_5.Items.FindItemByValue(id_ID_MON_5.ToString()) != null)
                {
                    rcbID_MON_5.SelectedValue = id_ID_MON_5.ToString();
                }

                RadComboBox rcbID_MON_6 = (RadComboBox)e.Item.FindControl("rcbID_MON_6");
                HiddenField hdID_MON_6 = (HiddenField)e.Item.FindControl("hdID_MON_6");
                long? id_ID_MON_6 = null;
                id_ID_MON_6 = localAPI.ConvertStringTolong(hdID_MON_6.Value);

                if (hdID_MON_6 != null && rcbID_MON_6.Items.FindItemByValue(id_ID_MON_6.ToString()) != null)
                {
                    rcbID_MON_6.SelectedValue = id_ID_MON_6.ToString();
                }

                RadComboBox rcbID_MON_7 = (RadComboBox)e.Item.FindControl("rcbID_MON_7");
                HiddenField hdID_MON_7 = (HiddenField)e.Item.FindControl("hdID_MON_7");
                long? id_ID_MON_7 = null;
                id_ID_MON_7 = localAPI.ConvertStringTolong(hdID_MON_7.Value);

                if (hdID_MON_7 != null && rcbID_MON_7.Items.FindItemByValue(id_ID_MON_7.ToString()) != null)
                {
                    rcbID_MON_7.SelectedValue = id_ID_MON_7.ToString();
                }

                RadComboBox rcbID_MON_8 = (RadComboBox)e.Item.FindControl("rcbID_MON_8");
                HiddenField hdID_MON_8 = (HiddenField)e.Item.FindControl("hdID_MON_8");
                long? id_ID_MON_8 = null;
                id_ID_MON_8 = localAPI.ConvertStringTolong(hdID_MON_8.Value);

                if (hdID_MON_8 != null && rcbID_MON_8.Items.FindItemByValue(id_ID_MON_8.ToString()) != null)
                {
                    rcbID_MON_8.SelectedValue = id_ID_MON_8.ToString();
                }
            }
        }
        protected void btnDoiLich_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            CA_HOC caHoc = new CA_HOC();
            CAU_HINH_CA_HOC chch = new CAU_HINH_CA_HOC();
            CauHinhCaHocBO chchBO = new CauHinhCaHocBO();
            if (rcbCauHinhCaHoc.SelectedValue == "")
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Vui lòng chọn thời gian học để đổi cấu hình!');", true);
                return;
            }
            chch = chchBO.getCauHinhCaHocByID(Convert.ToInt64(rcbCauHinhCaHoc.SelectedValue));
            ResultEntity res = caHocBO.doiCauHinhCaHoc(Sys_This_Truong.ID, localAPI.ConvertStringToShort(rcbHocKy.SelectedValue).Value, Convert.ToInt16(Sys_Ma_Nam_hoc), chch);

            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', 'Đổi đối hình ca học thành công!');";
            }
            else
            {
                strMsg = "notification('error', 'Ca học này không thể đổi thời gian, vui lòng liên hệ với quản trị viên');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
        protected void btnLuu_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            long? id_lop = localAPI.ConvertStringTolong(rcbLop.SelectedValue);
            List<CA_HOC> lstRes = caHocBO.getCaHocByLopAndHocKy(Sys_This_Truong.ID, id_lop, localAPI.ConvertStringToShort(rcbHocKy.SelectedValue).Value);
            int success = 0, error = 0; string lst_id = string.Empty;
            foreach (GridDataItem item in RadGrid1.Items)
            {
                CA_HOC detail = new CA_HOC();
                long ID = localAPI.ConvertStringTolong(item.GetDataKeyValue("ID").ToString()).Value;
                detail = lstRes.FirstOrDefault(x => x.ID == ID);
                #region get control
                RadComboBox rcbID_MON_2 = (RadComboBox)item.FindControl("rcbID_MON_2");
                HiddenField hdID_MON_2 = (HiddenField)item.FindControl("hdID_MON_2");
                RadComboBox rcbID_MON_3 = (RadComboBox)item.FindControl("rcbID_MON_3");
                HiddenField hdID_MON_3 = (HiddenField)item.FindControl("hdID_MON_3");
                RadComboBox rcbID_MON_4 = (RadComboBox)item.FindControl("rcbID_MON_4");
                HiddenField hdID_MON_4 = (HiddenField)item.FindControl("hdID_MON_4");
                RadComboBox rcbID_MON_5 = (RadComboBox)item.FindControl("rcbID_MON_5");
                HiddenField hdID_MON_5 = (HiddenField)item.FindControl("hdID_MON_5");
                RadComboBox rcbID_MON_6 = (RadComboBox)item.FindControl("rcbID_MON_6");
                HiddenField hdID_MON_6 = (HiddenField)item.FindControl("hdID_MON_6");
                RadComboBox rcbID_MON_7 = (RadComboBox)item.FindControl("rcbID_MON_7");
                HiddenField hdID_MON_7 = (HiddenField)item.FindControl("hdID_MON_7");
                RadComboBox rcbID_MON_8 = (RadComboBox)item.FindControl("rcbID_MON_8");
                HiddenField hdID_MON_8 = (HiddenField)item.FindControl("hdID_MON_8");
                detail.ID_MON_2 = localAPI.ConvertStringTolong(rcbID_MON_2.SelectedValue);
                detail.ID_MON_3 = localAPI.ConvertStringTolong(rcbID_MON_3.SelectedValue);
                detail.ID_MON_4 = localAPI.ConvertStringTolong(rcbID_MON_4.SelectedValue);
                detail.ID_MON_5 = localAPI.ConvertStringTolong(rcbID_MON_5.SelectedValue);
                detail.ID_MON_6 = localAPI.ConvertStringTolong(rcbID_MON_6.SelectedValue);
                detail.ID_MON_7 = localAPI.ConvertStringTolong(rcbID_MON_7.SelectedValue);
                detail.ID_MON_8 = localAPI.ConvertStringTolong(rcbID_MON_8.SelectedValue);
                #endregion
                if (hdID_MON_2.Value != rcbID_MON_2.SelectedValue
                    || hdID_MON_3.Value != rcbID_MON_3.SelectedValue
                    || hdID_MON_4.Value != rcbID_MON_4.SelectedValue
                    || hdID_MON_5.Value != rcbID_MON_5.SelectedValue
                    || hdID_MON_6.Value != rcbID_MON_6.SelectedValue
                    || hdID_MON_7.Value != rcbID_MON_7.SelectedValue
                    || hdID_MON_8.Value != rcbID_MON_8.SelectedValue)
                {
                    try
                    {
                        ResultEntity res = caHocBO.updateMonHoc(detail, Sys_User.ID);
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


            }
            string strMsg = "";
            if (error > 0)
            {
                strMsg = "notification('error', 'Có " + error + " bản ghi chưa được cập nhật. Liên hệ với quản trị viên');";
            }
            if (success > 0)
            {
                strMsg += " notification('success', 'Có " + success + " bản ghi được cập nhật.');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
        }
        protected void rcbKhoi_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbLop.ClearSelection();
            rcbLop.Text = String.Empty;
            rcbLop.DataBind();
            RadGrid1.Rebind();
        }
        protected void rcbLop_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void rcbHocKy_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            insertEmpty();
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
            string newName = "Thoi_khoa_bieu" + (localAPI.ConvertStringTolong(rcbLop.SelectedValue) == null ? "" : "_" + rcbLop.Text) + ".xlsx";

            List<ExcelHeaderEntity> lstHeader = new List<ExcelHeaderEntity>();
            List<ExcelEntity> lstColumn = new List<ExcelEntity>();
            #region add column
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "TIET"))
            {
                DataColumn col = new DataColumn("TIET");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Tiết", colM = 1, rowM = 1, width = 10 });
                lstColumn.Add(new ExcelEntity { Name = "TIET", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "ID_MON_2"))
            {
                DataColumn col = new DataColumn("ID_MON_2");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Thứ 2", colM = 1, rowM = 1, width = 20 });
                lstColumn.Add(new ExcelEntity { Name = "ID_MON_2", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "ID_MON_3"))
            {
                DataColumn col = new DataColumn("ID_MON_3");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Thứ 3", colM = 1, rowM = 1, width = 20 });
                lstColumn.Add(new ExcelEntity { Name = "ID_MON_3", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "ID_MON_4"))
            {
                DataColumn col = new DataColumn("ID_MON_4");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Thứ 4", colM = 1, rowM = 1, width = 20 });
                lstColumn.Add(new ExcelEntity { Name = "ID_MON_4", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "ID_MON_5"))
            {
                DataColumn col = new DataColumn("ID_MON_5");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Thứ 5", colM = 1, rowM = 1, width = 20 });
                lstColumn.Add(new ExcelEntity { Name = "ID_MON_5", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "ID_MON_6"))
            {
                DataColumn col = new DataColumn("ID_MON_6");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Thứ 6", colM = 1, rowM = 1, width = 20 });
                lstColumn.Add(new ExcelEntity { Name = "ID_MON_6", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "ID_MON_7"))
            {
                DataColumn col = new DataColumn("ID_MON_7");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Thứ 7", colM = 1, rowM = 1, width = 20 });
                lstColumn.Add(new ExcelEntity { Name = "ID_MON_7", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "ID_MON_8"))
            {
                DataColumn col = new DataColumn("ID_MON_8");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Chủ nhật", colM = 1, rowM = 1, width = 20 });
                lstColumn.Add(new ExcelEntity { Name = "ID_MON_8", Align = XLAlignmentHorizontalValues.Left, Color = XLColor.Black, Type = "String" });
            }
            #endregion
            RadGrid1.AllowPaging = false;
            foreach (GridDataItem item in RadGrid1.Items)
            {
                RadComboBox rcbID_MON_2 = (RadComboBox)item.FindControl("rcbID_MON_2");
                RadComboBox rcbID_MON_3 = (RadComboBox)item.FindControl("rcbID_MON_3");
                RadComboBox rcbID_MON_4 = (RadComboBox)item.FindControl("rcbID_MON_4");
                RadComboBox rcbID_MON_5 = (RadComboBox)item.FindControl("rcbID_MON_5");
                RadComboBox rcbID_MON_6 = (RadComboBox)item.FindControl("rcbID_MON_6");
                RadComboBox rcbID_MON_7 = (RadComboBox)item.FindControl("rcbID_MON_7");
                RadComboBox rcbID_MON_8 = (RadComboBox)item.FindControl("rcbID_MON_8");
                DataRow row = dt.NewRow();
                foreach (DataColumn col in dt.Columns)
                {
                    row[col.ColumnName] = item[col.ColumnName].Text.Replace("&nbsp;", " ");
                    if (col.ColumnName == "ID_MON_2") row[col.ColumnName] = (rcbID_MON_2.SelectedValue == null || rcbID_MON_2.SelectedValue == "0") ? "" : rcbID_MON_2.Text;
                    if (col.ColumnName == "ID_MON_3") row[col.ColumnName] = (rcbID_MON_3.SelectedValue == null || rcbID_MON_3.SelectedValue == "0") ? "" : rcbID_MON_3.Text;
                    if (col.ColumnName == "ID_MON_4") row[col.ColumnName] = (rcbID_MON_4.SelectedValue == null || rcbID_MON_4.SelectedValue == "0") ? "" : rcbID_MON_4.Text;
                    if (col.ColumnName == "ID_MON_5") row[col.ColumnName] = (rcbID_MON_5.SelectedValue == null || rcbID_MON_5.SelectedValue == "0") ? "" : rcbID_MON_5.Text;
                    if (col.ColumnName == "ID_MON_6") row[col.ColumnName] = (rcbID_MON_6.SelectedValue == null || rcbID_MON_6.SelectedValue == "0") ? "" : rcbID_MON_6.Text;
                    if (col.ColumnName == "ID_MON_7") row[col.ColumnName] = (rcbID_MON_7.SelectedValue == null || rcbID_MON_7.SelectedValue == "0") ? "" : rcbID_MON_7.Text;
                    if (col.ColumnName == "ID_MON_8") row[col.ColumnName] = (rcbID_MON_8.SelectedValue == null || rcbID_MON_8.SelectedValue == "0") ? "" : rcbID_MON_8.Text;
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
            string tieuDe = "THỜI KHÓA BIỂU";
            string hocKyNamHoc = "Lớp " + (localAPI.ConvertStringTolong(rcbLop.SelectedValue) == null ? "" : rcbLop.Text) + ", năm học: " + Sys_Ten_Nam_Hoc + ", " + rcbHocKy.Text.ToLower();
            listCell.Add(new ExcelHeaderEntity { name = tenSo, colM = 3, rowM = 1, colName = "A", rowIndex = 1, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tenPhong, colM = 3, rowM = 1, colName = "A", rowIndex = 2, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tieuDe, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 3, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            listCell.Add(new ExcelHeaderEntity { name = hocKyNamHoc, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 4, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            string nameFileOutput = newName;
            localAPI.ExportExcelDynamic(serverPath, path, newName, nameFileOutput, 1, listCell, rowHeaderStart, rowStart, colStart, colEnd, dt, lstHeader, lstColumn, false);
        }
    }
}