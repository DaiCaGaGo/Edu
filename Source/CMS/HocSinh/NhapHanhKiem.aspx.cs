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

namespace CMS.HocSinh
{
    public partial class NhapHanhKiem : AuthenticatePage
    {
        private MonHocTruongBO monHocTruongBO = new MonHocTruongBO();
        private LopMonBO lopMonBO = new LopMonBO();
        private DiemChiTietBO diemChiTietBO = new DiemChiTietBO();
        private LocalAPI localAPI = new LocalAPI();
        private DataAccessAPI dataAccessAPI = new DataAccessAPI();
        DiemTongKetBO diemTongKetBO = new DiemTongKetBO();
        LogUserBO logUserBO = new LogUserBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            btSave.Visible = is_access(SYS_Type_Access.SUA);
            if (!IsPostBack)
            {
                objKhoiHoc.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                rcbKhoiHoc.DataBind();
                objLop.SelectParameters.Add("ma_cap_hoc", Sys_This_Cap_Hoc);
                objLop.SelectParameters.Add("idTruong", Sys_This_Truong.ID.ToString());
                objLop.SelectParameters.Add("maNamHoc", Sys_Ma_Nam_hoc.ToString());
                rcbLop.DataBind();
                insertEmpty();
            }
        }
        public void insertEmpty()
        {
            long? id_lop = localAPI.ConvertStringTolong(rcbLop.SelectedValue);
            if (id_lop != null)
                diemTongKetBO.insertEmpty(Sys_This_Truong.ID, Convert.ToInt16(Sys_Ma_Nam_hoc), id_lop.Value);
        }
        protected void rcbKhoiHoc_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbLop.ClearSelection();
            rcbLop.Text = string.Empty;
            rcbLop.DataBind();
            insertEmpty();
            RadGrid1.Rebind();
        }
        protected void rcbLop_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            insertEmpty();
            RadGrid1.Rebind();
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            short? ma_khoi = localAPI.ConvertStringToShort(rcbKhoiHoc.SelectedValue);
            long? id_lop = localAPI.ConvertStringTolong(rcbLop.SelectedValue);
            RadGrid1.DataSource = diemTongKetBO.getDiemTongKetByTruongKhoiLop(Sys_This_Truong.ID, localAPI.ConvertStringToShort(rcbKhoiHoc.SelectedValue), localAPI.ConvertStringTolong(rcbLop.SelectedValue), Convert.ToInt16(Sys_Ma_Nam_hoc));
            if (Sys_Hoc_Ky == 1)
            {
                RadGrid1.MasterTableView.Columns.FindByUniqueName("TB_KY2").Display = false;
                RadGrid1.MasterTableView.Columns.FindByUniqueName("TB_CN").Display = false;
                RadGrid1.MasterTableView.Columns.FindByUniqueName("MA_HOC_LUC_KY2").Display = false;
                RadGrid1.MasterTableView.Columns.FindByUniqueName("MA_HOC_LUC_CA_NAM").Display = false;
                RadGrid1.MasterTableView.Columns.FindByUniqueName("MA_HANH_KIEM_KY2").Display = false;
                RadGrid1.MasterTableView.Columns.FindByUniqueName("MA_HANH_KIEM_CA_NAM").Display = false;
                RadGrid1.MasterTableView.Columns.FindByUniqueName("MA_DANH_HIEU_KY2").Display = false;
                RadGrid1.MasterTableView.Columns.FindByUniqueName("MA_DANH_HIEU_CN").Display = false;
                RadGrid1.MasterTableView.Columns.FindByUniqueName("IS_LEN_LOP").Display = false;
                RadGrid1.MasterTableView.Columns.FindByUniqueName("IS_TOT_NGHIEP").Display = false;
            }
            else
            {
                RadGrid1.MasterTableView.Columns.FindByUniqueName("TB_KY2").Display = true;
                RadGrid1.MasterTableView.Columns.FindByUniqueName("TB_CN").Display = true;
                RadGrid1.MasterTableView.Columns.FindByUniqueName("MA_HOC_LUC_KY2").Display = true;
                RadGrid1.MasterTableView.Columns.FindByUniqueName("MA_HOC_LUC_CA_NAM").Display = true;
                RadGrid1.MasterTableView.Columns.FindByUniqueName("MA_HANH_KIEM_KY2").Display = true;
                RadGrid1.MasterTableView.Columns.FindByUniqueName("MA_HANH_KIEM_CA_NAM").Display = true;
                RadGrid1.MasterTableView.Columns.FindByUniqueName("MA_DANH_HIEU_KY2").Display = true;
                RadGrid1.MasterTableView.Columns.FindByUniqueName("MA_DANH_HIEU_CN").Display = true;
                RadGrid1.MasterTableView.Columns.FindByUniqueName("IS_LEN_LOP").Display = true;
                if (rcbKhoiHoc.SelectedValue == "9" || rcbKhoiHoc.SelectedValue == "12")
                    RadGrid1.MasterTableView.Columns.FindByUniqueName("IS_TOT_NGHIEP").Display = true;
                else
                    RadGrid1.MasterTableView.Columns.FindByUniqueName("IS_TOT_NGHIEP").Display = false;
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
        protected void btSave_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            int success = 0, error = 0;
            foreach (GridDataItem item in RadGrid1.Items)
            {
                long? id_diem_tong_ket = localAPI.ConvertStringTolong(item.GetDataKeyValue("ID").ToString());
                TextBox tbMA_HANH_KIEM_KY1 = (TextBox)item.FindControl("tbMA_HANH_KIEM_KY1");
                TextBox tbMA_HANH_KIEM_KY2 = (TextBox)item.FindControl("tbMA_HANH_KIEM_KY2");
                TextBox tbMA_HANH_KIEM_CA_NAM = (TextBox)item.FindControl("tbMA_HANH_KIEM_CA_NAM");
                HiddenField hdMA_HOC_LUC_KY1 = (HiddenField)item.FindControl("hdMA_HOC_LUC_KY1");
                HiddenField hdMA_HOC_LUC_KY2 = (HiddenField)item.FindControl("hdMA_HOC_LUC_KY2");
                HiddenField hdMA_HOC_LUC_CA_NAM = (HiddenField)item.FindControl("hdMA_HOC_LUC_CA_NAM");
                CheckBox chbLenLop = (CheckBox)item.FindControl("chbLenLop");
                CheckBox chbTotNghiep = (CheckBox)item.FindControl("chbTotNghiep");
                string hanhKiem1 = tbMA_HANH_KIEM_KY1.Text.Trim();
                string hanhKiem2 = tbMA_HANH_KIEM_KY2.Text.Trim();
                string hanhKiemCN = tbMA_HANH_KIEM_CA_NAM.Text.Trim();
                string hoc_luc1 = hdMA_HOC_LUC_KY1.Value;
                string hoc_luc2 = hdMA_HOC_LUC_KY2.Value;
                string hoc_lucCN = hdMA_HOC_LUC_CA_NAM.Value;
                DIEM_TONG_KET detail = new DIEM_TONG_KET();
                if (id_diem_tong_ket != null && id_diem_tong_ket > 0)
                {
                    detail = diemTongKetBO.getDiemTKByID(id_diem_tong_ket.Value);
                    if (detail != null)
                    {
                        if (Sys_Hoc_Ky == 1)
                        {
                            string id_hanh_kiem1 = hanhKiem1 == "T" ? "1" : hanhKiem1 == "K" ? "2" : hanhKiem1 == "TB" ? "3" : hanhKiem1 == "Y" ? "4" : "";
                            detail.MA_HANH_KIEM_KY1 = localAPI.ConvertStringToShort(id_hanh_kiem1);
                            if (id_hanh_kiem1 == "1" && hoc_luc1 == "1")
                                detail.MA_DANH_HIEU_KY1 = 1;
                            else if ((id_hanh_kiem1 == "1" && hoc_luc1 == "2") || (id_hanh_kiem1 == "2" && (hoc_luc1 == "1" || hoc_luc1 == "2")))
                                detail.MA_DANH_HIEU_KY1 = 2;
                            else detail.MA_DANH_HIEU_KY1 = null;
                            res = diemTongKetBO.update(detail, Sys_User.ID);
                            if (res.Res)
                            {
                                success++;
                                logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật hạnh kiểm học kỳ 1 học sinh " + detail.ID_HOC_SINH, Sys_User.ID, DateTime.Now);
                            }
                            else error++;
                        }
                        else
                        {
                            string id_hanh_kiem2 = hanhKiem2 == "T" ? "1" : hanhKiem2 == "K" ? "2" : hanhKiem2 == "TB" ? "3" : hanhKiem2 == "Y" ? "4" : "";
                            string id_hanh_kiemCN = hanhKiemCN == "T" ? "1" : hanhKiemCN == "K" ? "2" : hanhKiemCN == "TB" ? "3" : hanhKiemCN == "Y" ? "4" : "";
                            detail.MA_HANH_KIEM_KY2 = localAPI.ConvertStringToShort(id_hanh_kiem2);
                            detail.MA_HANH_KIEM_CA_NAM = localAPI.ConvertStringToShort(id_hanh_kiemCN);
                            #region "danh hiệu"
                            if (id_hanh_kiem2 == "1" && hoc_luc2 == "1")
                                detail.MA_DANH_HIEU_KY2 = 1;
                            else if ((id_hanh_kiem2 == "1" && hoc_luc2 == "2") || (id_hanh_kiem2 == "2" && (hoc_luc2 == "1" || hoc_luc2 == "2")))
                                detail.MA_DANH_HIEU_KY2 = 2;
                            else detail.MA_DANH_HIEU_KY2 = null;
                            if (id_hanh_kiemCN == "1" && hoc_lucCN == "1")
                                detail.MA_DANH_HIEU_CN = 1;
                            else if ((id_hanh_kiemCN == "1" && hoc_lucCN == "2") || (id_hanh_kiemCN == "2" && (hoc_lucCN == "1" || hoc_lucCN == "2")))
                                detail.MA_DANH_HIEU_CN = 2;
                            else detail.MA_DANH_HIEU_CN = null;
                            #endregion
                            detail.IS_LEN_LOP = chbLenLop.Checked;
                            if (rcbKhoiHoc.SelectedValue == "9" || rcbKhoiHoc.SelectedValue == "12")
                                detail.IS_TOT_NGHIEP = chbTotNghiep.Checked;
                            res = diemTongKetBO.update(detail, Sys_User.ID);
                            if (res.Res)
                            {
                                success++;
                                logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật hạnh kiểm học kỳ 2 học sinh " + detail.ID_HOC_SINH, Sys_User.ID, DateTime.Now);
                            }
                            else error++;
                        }
                    }
                }
            }
            string strMsg = "";
            if (error > 0)
            {
                strMsg = "notification('error', 'Có " + error + " bản ghi chưa được lưu. Liên hệ với quản trị viên');";
            }
            if (success > 0)
            {
                strMsg += " notification('success', 'Có " + success + " bản ghi lưu thành công.');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
        }
        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                TextBox tbTB_KY1 = (TextBox)item.FindControl("tbTB_KY1");
                TextBox tbTB_KY2 = (TextBox)item.FindControl("tbTB_KY2");
                TextBox tbTB_CN = (TextBox)item.FindControl("tbTB_CN");
                TextBox tbMA_HOC_LUC_KY1 = (TextBox)item.FindControl("tbMA_HOC_LUC_KY1");
                TextBox tbMA_HOC_LUC_KY2 = (TextBox)item.FindControl("tbMA_HOC_LUC_KY2");
                TextBox tbMA_HOC_LUC_CA_NAM = (TextBox)item.FindControl("tbMA_HOC_LUC_CA_NAM");
                TextBox tbMA_DANH_HIEU_KY1 = (TextBox)item.FindControl("tbMA_DANH_HIEU_KY1");
                TextBox tbMA_DANH_HIEU_KY2 = (TextBox)item.FindControl("tbMA_DANH_HIEU_KY2");
                TextBox tbMA_DANH_HIEU_CN = (TextBox)item.FindControl("tbMA_DANH_HIEU_CN");
                TextBox tbMA_HANH_KIEM_KY1 = (TextBox)item.FindControl("tbMA_HANH_KIEM_KY1");
                TextBox tbMA_HANH_KIEM_KY2 = (TextBox)item.FindControl("tbMA_HANH_KIEM_KY2");
                TextBox tbMA_HANH_KIEM_CA_NAM = (TextBox)item.FindControl("tbMA_HANH_KIEM_CA_NAM");
                HiddenField hdTB_KY1 = (HiddenField)item.FindControl("hdTB_KY1");
                HiddenField hdTB_KY2 = (HiddenField)item.FindControl("hdTB_KY2");
                HiddenField hdTB_CN = (HiddenField)item.FindControl("hdTB_CN");
                HiddenField hdMA_HOC_LUC_KY1 = (HiddenField)item.FindControl("hdMA_HOC_LUC_KY1");
                HiddenField hdMA_HOC_LUC_KY2 = (HiddenField)item.FindControl("hdMA_HOC_LUC_KY2");
                HiddenField hdMA_HOC_LUC_CA_NAM = (HiddenField)item.FindControl("hdMA_HOC_LUC_CA_NAM");
                HiddenField hdMA_HANH_KIEM_KY1 = (HiddenField)item.FindControl("hdMA_HANH_KIEM_KY1");
                HiddenField hdMA_HANH_KIEM_KY2 = (HiddenField)item.FindControl("hdMA_HANH_KIEM_KY2");
                HiddenField hdMA_HANH_KIEM_CA_NAM = (HiddenField)item.FindControl("hdMA_HANH_KIEM_CA_NAM");
                HiddenField hdMA_DANH_HIEU_KY1 = (HiddenField)item.FindControl("hdMA_DANH_HIEU_KY1");
                HiddenField hdMA_DANH_HIEU_KY2 = (HiddenField)item.FindControl("hdMA_DANH_HIEU_KY2");
                HiddenField hdMA_DANH_HIEU_CN = (HiddenField)item.FindControl("hdMA_DANH_HIEU_CN");
                CheckBox chbLenLop = (CheckBox)item.FindControl("chbLenLop");
                CheckBox chbTotNghiep = (CheckBox)item.FindControl("chbTotNghiep");
                HiddenField hdIS_LEN_LOP = (HiddenField)item.FindControl("hdIS_LEN_LOP");
                HiddenField hdIS_TOT_NGHIEP = (HiddenField)item.FindControl("hdIS_TOT_NGHIEP");

                tbTB_KY1.Enabled = false;
                tbTB_KY2.Enabled = false;
                tbTB_CN.Enabled = false;
                tbMA_HOC_LUC_KY1.Enabled = false;
                tbMA_HOC_LUC_KY2.Enabled = false;
                tbMA_HOC_LUC_CA_NAM.Enabled = false;
                tbMA_DANH_HIEU_KY1.Enabled = false;
                tbMA_DANH_HIEU_KY2.Enabled = false;
                tbMA_DANH_HIEU_CN.Enabled = false;
                if (Sys_Hoc_Ky == 1)
                {
                    tbMA_HANH_KIEM_KY1.Enabled = true;
                    tbMA_HANH_KIEM_KY2.Enabled = false;
                    tbMA_HANH_KIEM_CA_NAM.Enabled = false;
                }
                else if (Sys_Hoc_Ky == 2)
                {
                    tbMA_HANH_KIEM_KY1.Enabled = false;
                    tbMA_HANH_KIEM_KY2.Enabled = true;
                    tbMA_HANH_KIEM_CA_NAM.Enabled = true;
                }
                #region "điểm tổng kết"
                tbTB_KY1.Text = hdTB_KY1.Value != "" ? Math.Round(Convert.ToDecimal(hdTB_KY1.Value), 1).ToString() : "";
                tbTB_KY2.Text = hdTB_KY2.Value != "" ? Math.Round(Convert.ToDecimal(hdTB_KY2.Value), 1).ToString() : "";
                tbTB_CN.Text = hdTB_CN.Value != "" ? Math.Round(Convert.ToDecimal(hdTB_CN.Value), 1).ToString() : "";
                #endregion
                #region "Học lực"
                short? ma_hoc_luc_ky1 = localAPI.ConvertStringToShort(hdMA_HOC_LUC_KY1.Value);
                tbMA_HOC_LUC_KY1.Text = ma_hoc_luc_ky1 == 1 ? "G" : ma_hoc_luc_ky1 == 2 ? "K" : ma_hoc_luc_ky1 == 3 ? "TB" : ma_hoc_luc_ky1 == 4 ? "Y" : ma_hoc_luc_ky1 == 5 ? "KEM" : "";
                short? ma_hoc_luc_ky2 = localAPI.ConvertStringToShort(hdMA_HOC_LUC_KY2.Value);
                tbMA_HOC_LUC_KY2.Text = ma_hoc_luc_ky2 == 1 ? "G" : ma_hoc_luc_ky2 == 2 ? "K" : ma_hoc_luc_ky2 == 3 ? "TB" : ma_hoc_luc_ky2 == 4 ? "Y" : ma_hoc_luc_ky2 == 5 ? "KEM" : "";
                short? ma_hoc_luc_cn = localAPI.ConvertStringToShort(hdMA_HOC_LUC_CA_NAM.Value);
                tbMA_HOC_LUC_CA_NAM.Text = ma_hoc_luc_cn == 1 ? "G" : ma_hoc_luc_cn == 2 ? "K" : ma_hoc_luc_cn == 3 ? "TB" : ma_hoc_luc_cn == 4 ? "Y" : ma_hoc_luc_cn == 5 ? "KEM" : "";
                #endregion
                #region "hạnh kiểm"
                short? ma_hanh_kiem_ky1 = localAPI.ConvertStringToShort(hdMA_HANH_KIEM_KY1.Value);
                tbMA_HANH_KIEM_KY1.Text = ma_hanh_kiem_ky1 == 1 ? "T" : ma_hanh_kiem_ky1 == 2 ? "K" : ma_hanh_kiem_ky1 == 3 ? "TB" : ma_hanh_kiem_ky1 == 4 ? "Y" : "";
                short? ma_hanh_kiem_ky2 = localAPI.ConvertStringToShort(hdMA_HANH_KIEM_KY2.Value);
                tbMA_HANH_KIEM_KY2.Text = ma_hanh_kiem_ky2 == 1 ? "T" : ma_hanh_kiem_ky2 == 2 ? "K" : ma_hanh_kiem_ky2 == 3 ? "TB" : ma_hanh_kiem_ky2 == 4 ? "Y" : "";
                short? ma_hanh_kiem_CN = localAPI.ConvertStringToShort(hdMA_HANH_KIEM_CA_NAM.Value);
                tbMA_HANH_KIEM_CA_NAM.Text = ma_hanh_kiem_CN == 1 ? "T" : ma_hanh_kiem_CN == 2 ? "K" : ma_hanh_kiem_CN == 3 ? "TB" : ma_hanh_kiem_CN == 4 ? "Y" : "";
                #endregion
                #region "danh hiệu"
                short? ma_danh_hieu_ky1 = localAPI.ConvertStringToShort(hdMA_DANH_HIEU_KY1.Value);
                short? ma_danh_hieu_ky2 = localAPI.ConvertStringToShort(hdMA_DANH_HIEU_KY2.Value);
                short? ma_danh_hieu_cn = localAPI.ConvertStringToShort(hdMA_DANH_HIEU_CN.Value);
                tbMA_DANH_HIEU_KY1.Text = ma_danh_hieu_ky1 == 1 ? "G" : ma_danh_hieu_ky1 == 2 ? "TT" : "";
                tbMA_DANH_HIEU_KY2.Text = ma_danh_hieu_ky2 == 1 ? "G" : ma_danh_hieu_ky2 == 2 ? "TT" : "";
                tbMA_DANH_HIEU_CN.Text = ma_danh_hieu_cn == 1 ? "G" : ma_danh_hieu_cn == 2 ? "TT" : "";
                #endregion
                chbLenLop.Checked = hdIS_LEN_LOP.Value != "" && hdIS_LEN_LOP.Value == "1" ? true : false;
                chbTotNghiep.Checked = hdIS_TOT_NGHIEP.Value != "" && hdIS_TOT_NGHIEP.Value == "1" ? true : false;
            }
        }
    }
}