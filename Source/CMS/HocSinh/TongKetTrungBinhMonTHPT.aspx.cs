using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CMS.HocSinh
{
    public partial class TongKetTrungBinhMonTHPT : AuthenticatePage
    {
        LocalAPI localAPI = new LocalAPI();
        DiemTongKetBO diemTongKetBO = new DiemTongKetBO();
        LopMonBO lopMonBO = new LopMonBO();
        DiemChiTietBO diemChiTietBO = new DiemChiTietBO();
        LogUserBO logUserBO = new LogUserBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            btnTongKet.Visible = is_access(SYS_Type_Access.THEM) || is_access(SYS_Type_Access.SUA);
            if (!IsPostBack)
            {
                objKhoiHoc.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                rcbKhoiHoc.DataBind();
                objLop.SelectParameters.Add("ma_cap_hoc", Sys_This_Cap_Hoc);
                objLop.SelectParameters.Add("idTruong", Sys_This_Truong.ID.ToString());
                objLop.SelectParameters.Add("maNamHoc", Sys_Ma_Nam_hoc.ToString());
                rcbLop.DataBind();
                rcbHocKy.SelectedValue = Sys_Hoc_Ky.ToString();
                if (Sys_Hoc_Ky == 1) rcbHocKy.Enabled = false;
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            short? ma_khoi = localAPI.ConvertStringToShort(rcbKhoiHoc.SelectedValue);
            long? id_lop = localAPI.ConvertStringTolong(rcbLop.SelectedValue);
            if (id_lop != null)
            {
                List<DanhSachMonByLopEntity> lstMonLop = lopMonBO.getMonByLopTruongHocKy(Sys_This_Truong.ID, Convert.ToInt16(Sys_Ma_Nam_hoc), id_lop.Value, Convert.ToInt16(rcbHocKy.SelectedValue));
                RadGrid1.DataSource = diemTongKetBO.viewTongKetDiem(Sys_This_Truong.ID, id_lop.Value, Convert.ToInt16(Sys_Ma_Nam_hoc), Convert.ToInt16(rcbHocKy.SelectedValue), lstMonLop);
                #region "chỉ hiện thị số môn mà lớp này có"
                if (lstMonLop.Count > 0)
                {
                    for (int i = 24; i >= lstMonLop.Count; i--)
                    {
                        RadGrid1.MasterTableView.Columns.FindByUniqueName("MON_" + i + "_Ky1").Display = false;
                        RadGrid1.MasterTableView.Columns.FindByUniqueName("MON_" + i + "_Ky2").Display = false;
                        RadGrid1.MasterTableView.Columns.FindByUniqueName("MON_" + i + "_CN").Display = false;
                    }
                    for (int i = 0; i < lstMonLop.Count; i++)
                    {
                        RadGrid1.MasterTableView.Columns.FindByUniqueName("MON_" + i + "_Ky1").Display = true;
                        RadGrid1.MasterTableView.Columns.FindByUniqueName("MON_" + i + "_Ky2").Display = true;
                        RadGrid1.MasterTableView.Columns.FindByUniqueName("MON_" + i + "_CN").Display = true;
                        RadGrid1.MasterTableView.Columns.FindByUniqueName("TB_KY1").Display = true;
                        RadGrid1.MasterTableView.Columns.FindByUniqueName("TB_KY2").Display = true;
                        RadGrid1.MasterTableView.Columns.FindByUniqueName("TB_CN").Display = true;
                        RadGrid1.MasterTableView.ColumnGroups.FindGroupByName("Mon" + i).HeaderText = lstMonLop[i].TEN.ToString();
                        if (rcbHocKy.SelectedValue == "1")
                        {
                            RadGrid1.MasterTableView.Columns.FindByUniqueName("MON_" + i + "_Ky2").Display = false;
                            RadGrid1.MasterTableView.Columns.FindByUniqueName("MON_" + i + "_CN").Display = false;
                            RadGrid1.MasterTableView.Columns.FindByUniqueName("TB_KY2").Display = false;
                            RadGrid1.MasterTableView.Columns.FindByUniqueName("TB_CN").Display = false;
                        }
                        if (rcbHocKy.SelectedValue == "2")
                        {
                            RadGrid1.MasterTableView.Columns.FindByUniqueName("TB_CN").Display = false;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 24; i++)
                    {
                        RadGrid1.MasterTableView.Columns.FindByUniqueName("MON_" + i + "_Ky1").Display = false;
                        RadGrid1.MasterTableView.Columns.FindByUniqueName("MON_" + i + "_Ky2").Display = false;
                        RadGrid1.MasterTableView.Columns.FindByUniqueName("MON_" + i + "_CN").Display = false;
                        RadGrid1.MasterTableView.Columns.FindByUniqueName("TB_KY1").Display = false;
                        RadGrid1.MasterTableView.Columns.FindByUniqueName("TB_KY2").Display = false;
                        RadGrid1.MasterTableView.Columns.FindByUniqueName("TB_CN").Display = false;
                    }
                }
                #endregion
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                #region get control Textbox
                TextBox tbMON_0_Ky1 = (TextBox)item.FindControl("tbMON_0_Ky1");
                TextBox tbMON_1_Ky1 = (TextBox)item.FindControl("tbMON_1_Ky1");
                TextBox tbMON_2_Ky1 = (TextBox)item.FindControl("tbMON_2_Ky1");
                TextBox tbMON_3_Ky1 = (TextBox)item.FindControl("tbMON_3_Ky1");
                TextBox tbMON_4_Ky1 = (TextBox)item.FindControl("tbMON_4_Ky1");
                TextBox tbMON_5_Ky1 = (TextBox)item.FindControl("tbMON_5_Ky1");
                TextBox tbMON_6_Ky1 = (TextBox)item.FindControl("tbMON_6_Ky1");
                TextBox tbMON_7_Ky1 = (TextBox)item.FindControl("tbMON_7_Ky1");
                TextBox tbMON_8_Ky1 = (TextBox)item.FindControl("tbMON_8_Ky1");
                TextBox tbMON_9_Ky1 = (TextBox)item.FindControl("tbMON_9_Ky1");
                TextBox tbMON_10_Ky1 = (TextBox)item.FindControl("tbMON_10_Ky1");
                TextBox tbMON_11_Ky1 = (TextBox)item.FindControl("tbMON_11_Ky1");
                TextBox tbMON_12_Ky1 = (TextBox)item.FindControl("tbMON_12_Ky1");
                TextBox tbMON_13_Ky1 = (TextBox)item.FindControl("tbMON_13_Ky1");
                TextBox tbMON_14_Ky1 = (TextBox)item.FindControl("tbMON_14_Ky1");
                TextBox tbMON_15_Ky1 = (TextBox)item.FindControl("tbMON_15_Ky1");
                TextBox tbMON_16_Ky1 = (TextBox)item.FindControl("tbMON_16_Ky1");
                TextBox tbMON_17_Ky1 = (TextBox)item.FindControl("tbMON_17_Ky1");
                TextBox tbMON_18_Ky1 = (TextBox)item.FindControl("tbMON_18_Ky1");
                TextBox tbMON_19_Ky1 = (TextBox)item.FindControl("tbMON_19_Ky1");
                TextBox tbMON_20_Ky1 = (TextBox)item.FindControl("tbMON_20_Ky1");
                TextBox tbMON_21_Ky1 = (TextBox)item.FindControl("tbMON_21_Ky1");
                TextBox tbMON_22_Ky1 = (TextBox)item.FindControl("tbMON_22_Ky1");
                TextBox tbMON_23_Ky1 = (TextBox)item.FindControl("tbMON_23_Ky1");
                TextBox tbMON_24_Ky1 = (TextBox)item.FindControl("tbMON_24_Ky1");
                TextBox tbMON_0_Ky2 = (TextBox)item.FindControl("tbMON_0_Ky2");
                TextBox tbMON_1_Ky2 = (TextBox)item.FindControl("tbMON_1_Ky2");
                TextBox tbMON_2_Ky2 = (TextBox)item.FindControl("tbMON_2_Ky2");
                TextBox tbMON_3_Ky2 = (TextBox)item.FindControl("tbMON_3_Ky2");
                TextBox tbMON_4_Ky2 = (TextBox)item.FindControl("tbMON_4_Ky2");
                TextBox tbMON_5_Ky2 = (TextBox)item.FindControl("tbMON_5_Ky2");
                TextBox tbMON_6_Ky2 = (TextBox)item.FindControl("tbMON_6_Ky2");
                TextBox tbMON_7_Ky2 = (TextBox)item.FindControl("tbMON_7_Ky2");
                TextBox tbMON_8_Ky2 = (TextBox)item.FindControl("tbMON_8_Ky2");
                TextBox tbMON_9_Ky2 = (TextBox)item.FindControl("tbMON_9_Ky2");
                TextBox tbMON_10_Ky2 = (TextBox)item.FindControl("tbMON_10_Ky2");
                TextBox tbMON_11_Ky2 = (TextBox)item.FindControl("tbMON_11_Ky2");
                TextBox tbMON_12_Ky2 = (TextBox)item.FindControl("tbMON_12_Ky2");
                TextBox tbMON_13_Ky2 = (TextBox)item.FindControl("tbMON_13_Ky2");
                TextBox tbMON_14_Ky2 = (TextBox)item.FindControl("tbMON_14_Ky2");
                TextBox tbMON_15_Ky2 = (TextBox)item.FindControl("tbMON_15_Ky2");
                TextBox tbMON_16_Ky2 = (TextBox)item.FindControl("tbMON_16_Ky2");
                TextBox tbMON_17_Ky2 = (TextBox)item.FindControl("tbMON_17_Ky2");
                TextBox tbMON_18_Ky2 = (TextBox)item.FindControl("tbMON_18_Ky2");
                TextBox tbMON_19_Ky2 = (TextBox)item.FindControl("tbMON_19_Ky2");
                TextBox tbMON_20_Ky2 = (TextBox)item.FindControl("tbMON_20_Ky2");
                TextBox tbMON_21_Ky2 = (TextBox)item.FindControl("tbMON_21_Ky2");
                TextBox tbMON_22_Ky2 = (TextBox)item.FindControl("tbMON_22_Ky2");
                TextBox tbMON_23_Ky2 = (TextBox)item.FindControl("tbMON_23_Ky2");
                TextBox tbMON_24_Ky2 = (TextBox)item.FindControl("tbMON_24_Ky2");
                TextBox tbMON_0_CN = (TextBox)item.FindControl("tbMON_0_CN");
                TextBox tbMON_1_CN = (TextBox)item.FindControl("tbMON_1_CN");
                TextBox tbMON_2_CN = (TextBox)item.FindControl("tbMON_2_CN");
                TextBox tbMON_3_CN = (TextBox)item.FindControl("tbMON_3_CN");
                TextBox tbMON_4_CN = (TextBox)item.FindControl("tbMON_4_CN");
                TextBox tbMON_5_CN = (TextBox)item.FindControl("tbMON_5_CN");
                TextBox tbMON_6_CN = (TextBox)item.FindControl("tbMON_6_CN");
                TextBox tbMON_7_CN = (TextBox)item.FindControl("tbMON_7_CN");
                TextBox tbMON_8_CN = (TextBox)item.FindControl("tbMON_8_CN");
                TextBox tbMON_9_CN = (TextBox)item.FindControl("tbMON_9_CN");
                TextBox tbMON_10_CN = (TextBox)item.FindControl("tbMON_10_CN");
                TextBox tbMON_11_CN = (TextBox)item.FindControl("tbMON_11_CN");
                TextBox tbMON_12_CN = (TextBox)item.FindControl("tbMON_12_CN");
                TextBox tbMON_13_CN = (TextBox)item.FindControl("tbMON_13_CN");
                TextBox tbMON_14_CN = (TextBox)item.FindControl("tbMON_14_CN");
                TextBox tbMON_15_CN = (TextBox)item.FindControl("tbMON_15_CN");
                TextBox tbMON_16_CN = (TextBox)item.FindControl("tbMON_16_CN");
                TextBox tbMON_17_CN = (TextBox)item.FindControl("tbMON_17_CN");
                TextBox tbMON_18_CN = (TextBox)item.FindControl("tbMON_18_CN");
                TextBox tbMON_19_CN = (TextBox)item.FindControl("tbMON_19_CN");
                TextBox tbMON_20_CN = (TextBox)item.FindControl("tbMON_20_CN");
                TextBox tbMON_21_CN = (TextBox)item.FindControl("tbMON_21_CN");
                TextBox tbMON_22_CN = (TextBox)item.FindControl("tbMON_22_CN");
                TextBox tbMON_23_CN = (TextBox)item.FindControl("tbMON_23_CN");
                TextBox tbMON_24_CN = (TextBox)item.FindControl("tbMON_24_CN");
                TextBox tbTB_KY1 = (TextBox)item.FindControl("tbTB_KY1");
                TextBox tbTB_KY2 = (TextBox)item.FindControl("tbTB_KY2");
                TextBox tbTB_CN = (TextBox)item.FindControl("tbTB_CN");
                #endregion
                #region "get control hidden"
                HiddenField hdMON_0_Ky1 = (HiddenField)item.FindControl("hdMON_0_Ky1");
                HiddenField hdMON_1_Ky1 = (HiddenField)item.FindControl("hdMON_1_Ky1");
                HiddenField hdMON_2_Ky1 = (HiddenField)item.FindControl("hdMON_2_Ky1");
                HiddenField hdMON_3_Ky1 = (HiddenField)item.FindControl("hdMON_3_Ky1");
                HiddenField hdMON_4_Ky1 = (HiddenField)item.FindControl("hdMON_4_Ky1");
                HiddenField hdMON_5_Ky1 = (HiddenField)item.FindControl("hdMON_5_Ky1");
                HiddenField hdMON_6_Ky1 = (HiddenField)item.FindControl("hdMON_6_Ky1");
                HiddenField hdMON_7_Ky1 = (HiddenField)item.FindControl("hdMON_7_Ky1");
                HiddenField hdMON_8_Ky1 = (HiddenField)item.FindControl("hdMON_8_Ky1");
                HiddenField hdMON_9_Ky1 = (HiddenField)item.FindControl("hdMON_9_Ky1");
                HiddenField hdMON_10_Ky1 = (HiddenField)item.FindControl("hdMON_10_Ky1");
                HiddenField hdMON_11_Ky1 = (HiddenField)item.FindControl("hdMON_11_Ky1");
                HiddenField hdMON_12_Ky1 = (HiddenField)item.FindControl("hdMON_12_Ky1");
                HiddenField hdMON_13_Ky1 = (HiddenField)item.FindControl("hdMON_13_Ky1");
                HiddenField hdMON_14_Ky1 = (HiddenField)item.FindControl("hdMON_14_Ky1");
                HiddenField hdMON_15_Ky1 = (HiddenField)item.FindControl("hdMON_15_Ky1");
                HiddenField hdMON_16_Ky1 = (HiddenField)item.FindControl("hdMON_16_Ky1");
                HiddenField hdMON_17_Ky1 = (HiddenField)item.FindControl("hdMON_17_Ky1");
                HiddenField hdMON_18_Ky1 = (HiddenField)item.FindControl("hdMON_18_Ky1");
                HiddenField hdMON_19_Ky1 = (HiddenField)item.FindControl("hdMON_19_Ky1");
                HiddenField hdMON_20_Ky1 = (HiddenField)item.FindControl("hdMON_20_Ky1");
                HiddenField hdMON_21_Ky1 = (HiddenField)item.FindControl("hdMON_21_Ky1");
                HiddenField hdMON_22_Ky1 = (HiddenField)item.FindControl("hdMON_22_Ky1");
                HiddenField hdMON_23_Ky1 = (HiddenField)item.FindControl("hdMON_23_Ky1");
                HiddenField hdMON_24_Ky1 = (HiddenField)item.FindControl("hdMON_24_Ky1");
                HiddenField hdMON_0_Ky2 = (HiddenField)item.FindControl("hdMON_0_Ky2");
                HiddenField hdMON_1_Ky2 = (HiddenField)item.FindControl("hdMON_1_Ky2");
                HiddenField hdMON_2_Ky2 = (HiddenField)item.FindControl("hdMON_2_Ky2");
                HiddenField hdMON_3_Ky2 = (HiddenField)item.FindControl("hdMON_3_Ky2");
                HiddenField hdMON_4_Ky2 = (HiddenField)item.FindControl("hdMON_4_Ky2");
                HiddenField hdMON_5_Ky2 = (HiddenField)item.FindControl("hdMON_5_Ky2");
                HiddenField hdMON_6_Ky2 = (HiddenField)item.FindControl("hdMON_6_Ky2");
                HiddenField hdMON_7_Ky2 = (HiddenField)item.FindControl("hdMON_7_Ky2");
                HiddenField hdMON_8_Ky2 = (HiddenField)item.FindControl("hdMON_8_Ky2");
                HiddenField hdMON_9_Ky2 = (HiddenField)item.FindControl("hdMON_9_Ky2");
                HiddenField hdMON_10_Ky2 = (HiddenField)item.FindControl("hdMON_10_Ky2");
                HiddenField hdMON_11_Ky2 = (HiddenField)item.FindControl("hdMON_11_Ky2");
                HiddenField hdMON_12_Ky2 = (HiddenField)item.FindControl("hdMON_12_Ky2");
                HiddenField hdMON_13_Ky2 = (HiddenField)item.FindControl("hdMON_13_Ky2");
                HiddenField hdMON_14_Ky2 = (HiddenField)item.FindControl("hdMON_14_Ky2");
                HiddenField hdMON_15_Ky2 = (HiddenField)item.FindControl("hdMON_15_Ky2");
                HiddenField hdMON_16_Ky2 = (HiddenField)item.FindControl("hdMON_16_Ky2");
                HiddenField hdMON_17_Ky2 = (HiddenField)item.FindControl("hdMON_17_Ky2");
                HiddenField hdMON_18_Ky2 = (HiddenField)item.FindControl("hdMON_18_Ky2");
                HiddenField hdMON_19_Ky2 = (HiddenField)item.FindControl("hdMON_19_Ky2");
                HiddenField hdMON_20_Ky2 = (HiddenField)item.FindControl("hdMON_20_Ky2");
                HiddenField hdMON_21_Ky2 = (HiddenField)item.FindControl("hdMON_21_Ky2");
                HiddenField hdMON_22_Ky2 = (HiddenField)item.FindControl("hdMON_22_Ky2");
                HiddenField hdMON_23_Ky2 = (HiddenField)item.FindControl("hdMON_23_Ky2");
                HiddenField hdMON_24_Ky2 = (HiddenField)item.FindControl("hdMON_24_Ky2");
                HiddenField hdMON_0_CN = (HiddenField)item.FindControl("hdMON_0_CN");
                HiddenField hdMON_1_CN = (HiddenField)item.FindControl("hdMON_1_CN");
                HiddenField hdMON_2_CN = (HiddenField)item.FindControl("hdMON_2_CN");
                HiddenField hdMON_3_CN = (HiddenField)item.FindControl("hdMON_3_CN");
                HiddenField hdMON_4_CN = (HiddenField)item.FindControl("hdMON_4_CN");
                HiddenField hdMON_5_CN = (HiddenField)item.FindControl("hdMON_5_CN");
                HiddenField hdMON_6_CN = (HiddenField)item.FindControl("hdMON_6_CN");
                HiddenField hdMON_7_CN = (HiddenField)item.FindControl("hdMON_7_CN");
                HiddenField hdMON_8_CN = (HiddenField)item.FindControl("hdMON_8_CN");
                HiddenField hdMON_9_CN = (HiddenField)item.FindControl("hdMON_9_CN");
                HiddenField hdMON_10_CN = (HiddenField)item.FindControl("hdMON_10_CN");
                HiddenField hdMON_11_CN = (HiddenField)item.FindControl("hdMON_11_CN");
                HiddenField hdMON_12_CN = (HiddenField)item.FindControl("hdMON_12_CN");
                HiddenField hdMON_13_CN = (HiddenField)item.FindControl("hdMON_13_CN");
                HiddenField hdMON_14_CN = (HiddenField)item.FindControl("hdMON_14_CN");
                HiddenField hdMON_15_CN = (HiddenField)item.FindControl("hdMON_15_CN");
                HiddenField hdMON_16_CN = (HiddenField)item.FindControl("hdMON_16_CN");
                HiddenField hdMON_17_CN = (HiddenField)item.FindControl("hdMON_17_CN");
                HiddenField hdMON_18_CN = (HiddenField)item.FindControl("hdMON_18_CN");
                HiddenField hdMON_19_CN = (HiddenField)item.FindControl("hdMON_19_CN");
                HiddenField hdMON_20_CN = (HiddenField)item.FindControl("hdMON_20_CN");
                HiddenField hdMON_21_CN = (HiddenField)item.FindControl("hdMON_21_CN");
                HiddenField hdMON_22_CN = (HiddenField)item.FindControl("hdMON_22_CN");
                HiddenField hdMON_23_CN = (HiddenField)item.FindControl("hdMON_23_CN");
                HiddenField hdMON_24_CN = (HiddenField)item.FindControl("hdMON_24_CN");
                HiddenField hdKIEU_MON_0 = (HiddenField)item.FindControl("hdKIEU_MON_0");
                HiddenField hdKIEU_MON_1 = (HiddenField)item.FindControl("hdKIEU_MON_1");
                HiddenField hdKIEU_MON_2 = (HiddenField)item.FindControl("hdKIEU_MON_2");
                HiddenField hdKIEU_MON_3 = (HiddenField)item.FindControl("hdKIEU_MON_3");
                HiddenField hdKIEU_MON_4 = (HiddenField)item.FindControl("hdKIEU_MON_4");
                HiddenField hdKIEU_MON_5 = (HiddenField)item.FindControl("hdKIEU_MON_5");
                HiddenField hdKIEU_MON_6 = (HiddenField)item.FindControl("hdKIEU_MON_6");
                HiddenField hdKIEU_MON_7 = (HiddenField)item.FindControl("hdKIEU_MON_7");
                HiddenField hdKIEU_MON_8 = (HiddenField)item.FindControl("hdKIEU_MON_8");
                HiddenField hdKIEU_MON_9 = (HiddenField)item.FindControl("hdKIEU_MON_9");
                HiddenField hdKIEU_MON_10 = (HiddenField)item.FindControl("hdKIEU_MON_10");
                HiddenField hdKIEU_MON_11 = (HiddenField)item.FindControl("hdKIEU_MON_11");
                HiddenField hdKIEU_MON_12 = (HiddenField)item.FindControl("hdKIEU_MON_12");
                HiddenField hdKIEU_MON_13 = (HiddenField)item.FindControl("hdKIEU_MON_13");
                HiddenField hdKIEU_MON_14 = (HiddenField)item.FindControl("hdKIEU_MON_14");
                HiddenField hdKIEU_MON_15 = (HiddenField)item.FindControl("hdKIEU_MON_15");
                HiddenField hdKIEU_MON_16 = (HiddenField)item.FindControl("hdKIEU_MON_16");
                HiddenField hdKIEU_MON_17 = (HiddenField)item.FindControl("hdKIEU_MON_17");
                HiddenField hdKIEU_MON_18 = (HiddenField)item.FindControl("hdKIEU_MON_18");
                HiddenField hdKIEU_MON_19 = (HiddenField)item.FindControl("hdKIEU_MON_19");
                HiddenField hdKIEU_MON_20 = (HiddenField)item.FindControl("hdKIEU_MON_20");
                HiddenField hdKIEU_MON_21 = (HiddenField)item.FindControl("hdKIEU_MON_21");
                HiddenField hdKIEU_MON_22 = (HiddenField)item.FindControl("hdKIEU_MON_22");
                HiddenField hdKIEU_MON_23 = (HiddenField)item.FindControl("hdKIEU_MON_23");
                HiddenField hdKIEU_MON_24 = (HiddenField)item.FindControl("hdKIEU_MON_24");
                HiddenField hdTB_KY1 = (HiddenField)item.FindControl("hdTB_KY1");
                HiddenField hdTB_KY2 = (HiddenField)item.FindControl("hdTB_KY2");
                HiddenField hdTB_CN = (HiddenField)item.FindControl("hdTB_CN");
                #endregion
                #region "set value control"
                #region "Mon 0"
                if (hdKIEU_MON_0.Value == null || hdKIEU_MON_0.Value == "" || hdKIEU_MON_0.Value == "0")
                {
                    if (hdMON_0_Ky1.Value != "")
                        tbMON_0_Ky1.Text = Math.Round(Convert.ToDecimal(hdMON_0_Ky1.Value), 1).ToString();
                    if (hdMON_0_Ky2.Value != "")
                        tbMON_0_Ky2.Text = Math.Round(Convert.ToDecimal(hdMON_0_Ky2.Value), 1).ToString();
                    if (hdMON_0_CN.Value != "")
                        tbMON_0_CN.Text = Math.Round(Convert.ToDecimal(hdMON_0_CN.Value), 1).ToString();
                }
                else if (hdKIEU_MON_0.Value == "1")
                {
                    if (hdMON_0_Ky1.Value == "0") tbMON_0_Ky1.Text = "CĐ";
                    else if (hdMON_0_Ky1.Value == "1") tbMON_0_Ky1.Text = "Đ";
                    if (hdMON_0_Ky2.Value == "0") tbMON_0_Ky1.Text = "CĐ";
                    else if (hdMON_0_Ky2.Value == "1") tbMON_0_Ky2.Text = "Đ";
                    if (hdMON_0_CN.Value == "0") tbMON_0_CN.Text = "CĐ";
                    else if (hdMON_0_CN.Value == "1") tbMON_0_CN.Text = "Đ";
                }
                #endregion
                #region "Mon 1"
                if (hdKIEU_MON_1.Value == null || hdKIEU_MON_1.Value == "" || hdKIEU_MON_1.Value == "0")
                {
                    if (hdMON_1_Ky1.Value != "")
                        tbMON_1_Ky1.Text = Math.Round(Convert.ToDecimal(hdMON_1_Ky1.Value), 1).ToString();
                    if (hdMON_1_Ky2.Value != "")
                        tbMON_1_Ky2.Text = Math.Round(Convert.ToDecimal(hdMON_1_Ky2.Value), 1).ToString();
                    if (hdMON_1_CN.Value != "")
                        tbMON_1_CN.Text = Math.Round(Convert.ToDecimal(hdMON_1_CN.Value), 1).ToString();
                }
                else if (hdKIEU_MON_1.Value == "1")
                {
                    if (hdMON_1_Ky1.Value == "0") tbMON_1_Ky1.Text = "CĐ";
                    else if (hdMON_1_Ky1.Value == "1") tbMON_1_Ky1.Text = "Đ";
                    if (hdMON_1_Ky2.Value == "0") tbMON_1_Ky1.Text = "CĐ";
                    else if (hdMON_1_Ky2.Value == "1") tbMON_1_Ky2.Text = "Đ";
                    if (hdMON_1_CN.Value == "0") tbMON_1_CN.Text = "CĐ";
                    else if (hdMON_1_CN.Value == "1") tbMON_1_CN.Text = "Đ";
                }
                #endregion
                #region "Mon 2"
                if (hdKIEU_MON_2.Value == null || hdKIEU_MON_2.Value == "" || hdKIEU_MON_2.Value == "0")
                {
                    if (hdMON_2_Ky1.Value != "")
                        tbMON_2_Ky1.Text = Math.Round(Convert.ToDecimal(hdMON_2_Ky1.Value), 1).ToString();
                    if (hdMON_2_Ky2.Value != "")
                        tbMON_2_Ky2.Text = Math.Round(Convert.ToDecimal(hdMON_2_Ky2.Value), 1).ToString();
                    if (hdMON_2_CN.Value != "")
                        tbMON_2_CN.Text = Math.Round(Convert.ToDecimal(hdMON_2_CN.Value), 1).ToString();
                }
                else if (hdKIEU_MON_2.Value == "1")
                {
                    if (hdMON_2_Ky1.Value == "0") tbMON_2_Ky1.Text = "CĐ";
                    else if (hdMON_2_Ky1.Value == "1") tbMON_2_Ky1.Text = "Đ";
                    if (hdMON_2_Ky2.Value == "0") tbMON_2_Ky1.Text = "CĐ";
                    else if (hdMON_2_Ky2.Value == "1") tbMON_2_Ky2.Text = "Đ";
                    if (hdMON_2_CN.Value == "0") tbMON_2_CN.Text = "CĐ";
                    else if (hdMON_2_CN.Value == "1") tbMON_2_CN.Text = "Đ";
                }
                #endregion
                #region "Mon 3"
                if (hdKIEU_MON_3.Value == null || hdKIEU_MON_3.Value == "" || hdKIEU_MON_3.Value == "0")
                {
                    if (hdMON_3_Ky1.Value != "")
                        tbMON_3_Ky1.Text = Math.Round(Convert.ToDecimal(hdMON_3_Ky1.Value), 1).ToString();
                    if (hdMON_3_Ky2.Value != "")
                        tbMON_3_Ky2.Text = Math.Round(Convert.ToDecimal(hdMON_3_Ky2.Value), 1).ToString();
                    if (hdMON_3_CN.Value != "")
                        tbMON_3_CN.Text = Math.Round(Convert.ToDecimal(hdMON_3_CN.Value), 1).ToString();
                }
                else if (hdKIEU_MON_3.Value == "1")
                {
                    if (hdMON_3_Ky1.Value == "0") tbMON_3_Ky1.Text = "CĐ";
                    else if (hdMON_3_Ky1.Value == "1") tbMON_3_Ky1.Text = "Đ";
                    if (hdMON_3_Ky2.Value == "0") tbMON_3_Ky1.Text = "CĐ";
                    else if (hdMON_3_Ky2.Value == "1") tbMON_3_Ky2.Text = "Đ";
                    if (hdMON_3_CN.Value == "0") tbMON_3_CN.Text = "CĐ";
                    else if (hdMON_3_CN.Value == "1") tbMON_3_CN.Text = "Đ";
                }
                #endregion
                #region "Mon 4"
                if (hdKIEU_MON_4.Value == null || hdKIEU_MON_4.Value == "" || hdKIEU_MON_4.Value == "0")
                {
                    if (hdMON_4_Ky1.Value != "")
                        tbMON_4_Ky1.Text = Math.Round(Convert.ToDecimal(hdMON_4_Ky1.Value), 1).ToString();
                    if (hdMON_4_Ky2.Value != "")
                        tbMON_4_Ky2.Text = Math.Round(Convert.ToDecimal(hdMON_4_Ky2.Value), 1).ToString();
                    if (hdMON_4_CN.Value != "")
                        tbMON_4_CN.Text = Math.Round(Convert.ToDecimal(hdMON_4_CN.Value), 1).ToString();
                }
                else if (hdKIEU_MON_4.Value == "1")
                {
                    if (hdMON_4_Ky1.Value == "0") tbMON_4_Ky1.Text = "CĐ";
                    else if (hdMON_4_Ky1.Value == "1") tbMON_4_Ky1.Text = "Đ";
                    if (hdMON_4_Ky2.Value == "0") tbMON_4_Ky1.Text = "CĐ";
                    else if (hdMON_4_Ky2.Value == "1") tbMON_4_Ky2.Text = "Đ";
                    if (hdMON_4_CN.Value == "0") tbMON_4_CN.Text = "CĐ";
                    else if (hdMON_4_CN.Value == "1") tbMON_4_CN.Text = "Đ";
                }
                #endregion
                #region "Mon 5"
                if (hdKIEU_MON_5.Value == null || hdKIEU_MON_5.Value == "" || hdKIEU_MON_5.Value == "0")
                {
                    if (hdMON_5_Ky1.Value != "")
                        tbMON_5_Ky1.Text = Math.Round(Convert.ToDecimal(hdMON_5_Ky1.Value), 1).ToString();
                    if (hdMON_5_Ky2.Value != "")
                        tbMON_5_Ky2.Text = Math.Round(Convert.ToDecimal(hdMON_5_Ky2.Value), 1).ToString();
                    if (hdMON_5_CN.Value != "")
                        tbMON_5_CN.Text = Math.Round(Convert.ToDecimal(hdMON_5_CN.Value), 1).ToString();
                }
                else if (hdKIEU_MON_5.Value == "1")
                {
                    if (hdMON_5_Ky1.Value == "0") tbMON_5_Ky1.Text = "CĐ";
                    else if (hdMON_5_Ky1.Value == "1") tbMON_5_Ky1.Text = "Đ";
                    if (hdMON_5_Ky2.Value == "0") tbMON_5_Ky1.Text = "CĐ";
                    else if (hdMON_5_Ky2.Value == "1") tbMON_5_Ky2.Text = "Đ";
                    if (hdMON_5_CN.Value == "0") tbMON_5_CN.Text = "CĐ";
                    else if (hdMON_5_CN.Value == "1") tbMON_5_CN.Text = "Đ";
                }
                #endregion
                #region "Mon 6"
                if (hdKIEU_MON_6.Value == null || hdKIEU_MON_6.Value == "" || hdKIEU_MON_6.Value == "0")
                {
                    if (hdMON_6_Ky1.Value != "")
                        tbMON_6_Ky1.Text = Math.Round(Convert.ToDecimal(hdMON_6_Ky1.Value), 1).ToString();
                    if (hdMON_6_Ky2.Value != "")
                        tbMON_6_Ky2.Text = Math.Round(Convert.ToDecimal(hdMON_6_Ky2.Value), 1).ToString();
                    if (hdMON_6_CN.Value != "")
                        tbMON_6_CN.Text = Math.Round(Convert.ToDecimal(hdMON_6_CN.Value), 1).ToString();
                }
                else if (hdKIEU_MON_6.Value == "1")
                {
                    if (hdMON_6_Ky1.Value == "0") tbMON_6_Ky1.Text = "CĐ";
                    else if (hdMON_6_Ky1.Value == "1") tbMON_6_Ky1.Text = "Đ";
                    if (hdMON_6_Ky2.Value == "0") tbMON_6_Ky1.Text = "CĐ";
                    else if (hdMON_6_Ky2.Value == "1") tbMON_6_Ky2.Text = "Đ";
                    if (hdMON_6_CN.Value == "0") tbMON_6_CN.Text = "CĐ";
                    else if (hdMON_6_CN.Value == "1") tbMON_6_CN.Text = "Đ";
                }
                #endregion
                #region "Mon 7"
                if (hdKIEU_MON_7.Value == null || hdKIEU_MON_7.Value == "" || hdKIEU_MON_7.Value == "0")
                {
                    if (hdMON_7_Ky1.Value != "")
                        tbMON_7_Ky1.Text = Math.Round(Convert.ToDecimal(hdMON_7_Ky1.Value), 1).ToString();
                    if (hdMON_7_Ky2.Value != "")
                        tbMON_7_Ky2.Text = Math.Round(Convert.ToDecimal(hdMON_7_Ky2.Value), 1).ToString();
                    if (hdMON_7_CN.Value != "")
                        tbMON_7_CN.Text = Math.Round(Convert.ToDecimal(hdMON_7_CN.Value), 1).ToString();
                }
                else if (hdKIEU_MON_7.Value == "1")
                {
                    if (hdMON_7_Ky1.Value == "0") tbMON_7_Ky1.Text = "CĐ";
                    else if (hdMON_7_Ky1.Value == "1") tbMON_7_Ky1.Text = "Đ";
                    if (hdMON_7_Ky2.Value == "0") tbMON_7_Ky1.Text = "CĐ";
                    else if (hdMON_7_Ky2.Value == "1") tbMON_7_Ky2.Text = "Đ";
                    if (hdMON_7_CN.Value == "0") tbMON_7_CN.Text = "CĐ";
                    else if (hdMON_7_CN.Value == "1") tbMON_7_CN.Text = "Đ";
                }
                #endregion
                #region "Mon 8"
                if (hdKIEU_MON_8.Value == null || hdKIEU_MON_8.Value == "" || hdKIEU_MON_8.Value == "0")
                {
                    if (hdMON_8_Ky1.Value != "")
                        tbMON_8_Ky1.Text = Math.Round(Convert.ToDecimal(hdMON_8_Ky1.Value), 1).ToString();
                    if (hdMON_8_Ky2.Value != "")
                        tbMON_8_Ky2.Text = Math.Round(Convert.ToDecimal(hdMON_8_Ky2.Value), 1).ToString();
                    if (hdMON_8_CN.Value != "")
                        tbMON_8_CN.Text = Math.Round(Convert.ToDecimal(hdMON_8_CN.Value), 1).ToString();
                }
                else if (hdKIEU_MON_8.Value == "1")
                {
                    if (hdMON_8_Ky1.Value == "0") tbMON_8_Ky1.Text = "CĐ";
                    else if (hdMON_8_Ky1.Value == "1") tbMON_8_Ky1.Text = "Đ";
                    if (hdMON_8_Ky2.Value == "0") tbMON_8_Ky1.Text = "CĐ";
                    else if (hdMON_8_Ky2.Value == "1") tbMON_8_Ky2.Text = "Đ";
                    if (hdMON_8_CN.Value == "0") tbMON_8_CN.Text = "CĐ";
                    else if (hdMON_8_CN.Value == "1") tbMON_8_CN.Text = "Đ";
                }
                #endregion
                #region "Mon 9"
                if (hdKIEU_MON_9.Value == null || hdKIEU_MON_9.Value == "" || hdKIEU_MON_9.Value == "0")
                {
                    if (hdMON_9_Ky1.Value != "")
                        tbMON_9_Ky1.Text = Math.Round(Convert.ToDecimal(hdMON_9_Ky1.Value), 1).ToString();
                    if (hdMON_9_Ky2.Value != "")
                        tbMON_9_Ky2.Text = Math.Round(Convert.ToDecimal(hdMON_9_Ky2.Value), 1).ToString();
                    if (hdMON_9_CN.Value != "")
                        tbMON_9_CN.Text = Math.Round(Convert.ToDecimal(hdMON_9_CN.Value), 1).ToString();
                }
                else if (hdKIEU_MON_9.Value == "1")
                {
                    if (hdMON_9_Ky1.Value == "0") tbMON_9_Ky1.Text = "CĐ";
                    else if (hdMON_9_Ky1.Value == "1") tbMON_9_Ky1.Text = "Đ";
                    if (hdMON_9_Ky2.Value == "0") tbMON_9_Ky1.Text = "CĐ";
                    else if (hdMON_9_Ky2.Value == "1") tbMON_9_Ky2.Text = "Đ";
                    if (hdMON_9_CN.Value == "0") tbMON_9_CN.Text = "CĐ";
                    else if (hdMON_9_CN.Value == "1") tbMON_9_CN.Text = "Đ";
                }
                #endregion
                #region "Mon 10"
                if (hdKIEU_MON_10.Value == null || hdKIEU_MON_10.Value == "" || hdKIEU_MON_10.Value == "0")
                {
                    if (hdMON_10_Ky1.Value != "")
                        tbMON_10_Ky1.Text = Math.Round(Convert.ToDecimal(hdMON_10_Ky1.Value), 1).ToString();
                    if (hdMON_10_Ky2.Value != "")
                        tbMON_10_Ky2.Text = Math.Round(Convert.ToDecimal(hdMON_10_Ky2.Value), 1).ToString();
                    if (hdMON_10_CN.Value != "")
                        tbMON_10_CN.Text = Math.Round(Convert.ToDecimal(hdMON_10_CN.Value), 1).ToString();
                }
                else if (hdKIEU_MON_10.Value == "1")
                {
                    if (hdMON_10_Ky1.Value == "0") tbMON_10_Ky1.Text = "CĐ";
                    else if (hdMON_10_Ky1.Value == "1") tbMON_10_Ky1.Text = "Đ";
                    if (hdMON_10_Ky2.Value == "0") tbMON_10_Ky1.Text = "CĐ";
                    else if (hdMON_10_Ky2.Value == "1") tbMON_10_Ky2.Text = "Đ";
                    if (hdMON_10_CN.Value == "0") tbMON_10_CN.Text = "CĐ";
                    else if (hdMON_10_CN.Value == "1") tbMON_10_CN.Text = "Đ";
                }
                #endregion
                #region "Mon 11"
                if (hdKIEU_MON_11.Value == null || hdKIEU_MON_11.Value == "" || hdKIEU_MON_11.Value == "0")
                {
                    if (hdMON_11_Ky1.Value != "")
                        tbMON_11_Ky1.Text = Math.Round(Convert.ToDecimal(hdMON_11_Ky1.Value), 1).ToString();
                    if (hdMON_11_Ky2.Value != "")
                        tbMON_11_Ky2.Text = Math.Round(Convert.ToDecimal(hdMON_11_Ky2.Value), 1).ToString();
                    if (hdMON_11_CN.Value != "")
                        tbMON_11_CN.Text = Math.Round(Convert.ToDecimal(hdMON_11_CN.Value), 1).ToString();
                }
                else if (hdKIEU_MON_11.Value == "1")
                {
                    if (hdMON_11_Ky1.Value == "0") tbMON_11_Ky1.Text = "CĐ";
                    else if (hdMON_11_Ky1.Value == "1") tbMON_11_Ky1.Text = "Đ";
                    if (hdMON_11_Ky2.Value == "0") tbMON_11_Ky1.Text = "CĐ";
                    else if (hdMON_11_Ky2.Value == "1") tbMON_11_Ky2.Text = "Đ";
                    if (hdMON_11_CN.Value == "0") tbMON_11_CN.Text = "CĐ";
                    else if (hdMON_11_CN.Value == "1") tbMON_11_CN.Text = "Đ";
                }
                #endregion
                #region "Mon 12"
                if (hdKIEU_MON_12.Value == null || hdKIEU_MON_12.Value == "" || hdKIEU_MON_12.Value == "0")
                {
                    if (hdMON_12_Ky1.Value != "")
                        tbMON_12_Ky1.Text = Math.Round(Convert.ToDecimal(hdMON_12_Ky1.Value), 1).ToString();
                    if (hdMON_12_Ky2.Value != "")
                        tbMON_12_Ky2.Text = Math.Round(Convert.ToDecimal(hdMON_12_Ky2.Value), 1).ToString();
                    if (hdMON_12_CN.Value != "")
                        tbMON_12_CN.Text = Math.Round(Convert.ToDecimal(hdMON_12_CN.Value), 1).ToString();
                }
                else if (hdKIEU_MON_12.Value == "1")
                {
                    if (hdMON_12_Ky1.Value == "0") tbMON_12_Ky1.Text = "CĐ";
                    else if (hdMON_12_Ky1.Value == "1") tbMON_12_Ky1.Text = "Đ";
                    if (hdMON_12_Ky2.Value == "0") tbMON_12_Ky1.Text = "CĐ";
                    else if (hdMON_12_Ky2.Value == "1") tbMON_12_Ky2.Text = "Đ";
                    if (hdMON_12_CN.Value == "0") tbMON_12_CN.Text = "CĐ";
                    else if (hdMON_12_CN.Value == "1") tbMON_12_CN.Text = "Đ";
                }
                #endregion
                #region "Mon 13"
                if (hdKIEU_MON_13.Value == null || hdKIEU_MON_13.Value == "" || hdKIEU_MON_13.Value == "0")
                {
                    if (hdMON_13_Ky1.Value != "")
                        tbMON_13_Ky1.Text = Math.Round(Convert.ToDecimal(hdMON_13_Ky1.Value), 1).ToString();
                    if (hdMON_13_Ky2.Value != "")
                        tbMON_13_Ky2.Text = Math.Round(Convert.ToDecimal(hdMON_13_Ky2.Value), 1).ToString();
                    if (hdMON_13_CN.Value != "")
                        tbMON_13_CN.Text = Math.Round(Convert.ToDecimal(hdMON_13_CN.Value), 1).ToString();
                }
                else if (hdKIEU_MON_13.Value == "1")
                {
                    if (hdMON_13_Ky1.Value == "0") tbMON_13_Ky1.Text = "CĐ";
                    else if (hdMON_13_Ky1.Value == "1") tbMON_13_Ky1.Text = "Đ";
                    if (hdMON_13_Ky2.Value == "0") tbMON_13_Ky1.Text = "CĐ";
                    else if (hdMON_13_Ky2.Value == "1") tbMON_13_Ky2.Text = "Đ";
                    if (hdMON_13_CN.Value == "0") tbMON_13_CN.Text = "CĐ";
                    else if (hdMON_13_CN.Value == "1") tbMON_13_CN.Text = "Đ";
                }
                #endregion
                #region "Mon 14"
                if (hdKIEU_MON_14.Value == null || hdKIEU_MON_14.Value == "" || hdKIEU_MON_14.Value == "0")
                {
                    if (hdMON_14_Ky1.Value != "")
                        tbMON_14_Ky1.Text = Math.Round(Convert.ToDecimal(hdMON_14_Ky1.Value), 1).ToString();
                    if (hdMON_14_Ky2.Value != "")
                        tbMON_14_Ky2.Text = Math.Round(Convert.ToDecimal(hdMON_14_Ky2.Value), 1).ToString();
                    if (hdMON_14_CN.Value != "")
                        tbMON_14_CN.Text = Math.Round(Convert.ToDecimal(hdMON_14_CN.Value), 1).ToString();
                }
                else if (hdKIEU_MON_14.Value == "1")
                {
                    if (hdMON_14_Ky1.Value == "0") tbMON_14_Ky1.Text = "CĐ";
                    else if (hdMON_14_Ky1.Value == "1") tbMON_14_Ky1.Text = "Đ";
                    if (hdMON_14_Ky2.Value == "0") tbMON_14_Ky1.Text = "CĐ";
                    else if (hdMON_14_Ky2.Value == "1") tbMON_14_Ky2.Text = "Đ";
                    if (hdMON_14_CN.Value == "0") tbMON_14_CN.Text = "CĐ";
                    else if (hdMON_14_CN.Value == "1") tbMON_14_CN.Text = "Đ";
                }
                #endregion
                #region "Mon 15"
                if (hdKIEU_MON_15.Value == null || hdKIEU_MON_15.Value == "" || hdKIEU_MON_15.Value == "0")
                {
                    if (hdMON_15_Ky1.Value != "")
                        tbMON_15_Ky1.Text = Math.Round(Convert.ToDecimal(hdMON_15_Ky1.Value), 1).ToString();
                    if (hdMON_15_Ky2.Value != "")
                        tbMON_15_Ky2.Text = Math.Round(Convert.ToDecimal(hdMON_15_Ky2.Value), 1).ToString();
                    if (hdMON_15_CN.Value != "")
                        tbMON_15_CN.Text = Math.Round(Convert.ToDecimal(hdMON_15_CN.Value), 1).ToString();
                }
                else if (hdKIEU_MON_15.Value == "1")
                {
                    if (hdMON_15_Ky1.Value == "0") tbMON_15_Ky1.Text = "CĐ";
                    else if (hdMON_15_Ky1.Value == "1") tbMON_15_Ky1.Text = "Đ";
                    if (hdMON_15_Ky2.Value == "0") tbMON_15_Ky1.Text = "CĐ";
                    else if (hdMON_15_Ky2.Value == "1") tbMON_15_Ky2.Text = "Đ";
                    if (hdMON_15_CN.Value == "0") tbMON_15_CN.Text = "CĐ";
                    else if (hdMON_15_CN.Value == "1") tbMON_15_CN.Text = "Đ";
                }
                #endregion
                #region "Mon 16"
                if (hdKIEU_MON_16.Value == null || hdKIEU_MON_16.Value == "" || hdKIEU_MON_16.Value == "0")
                {
                    if (hdMON_16_Ky1.Value != "")
                        tbMON_16_Ky1.Text = Math.Round(Convert.ToDecimal(hdMON_16_Ky1.Value), 1).ToString();
                    if (hdMON_16_Ky2.Value != "")
                        tbMON_16_Ky2.Text = Math.Round(Convert.ToDecimal(hdMON_16_Ky2.Value), 1).ToString();
                    if (hdMON_16_CN.Value != "")
                        tbMON_16_CN.Text = Math.Round(Convert.ToDecimal(hdMON_16_CN.Value), 1).ToString();
                }
                else if (hdKIEU_MON_16.Value == "1")
                {
                    if (hdMON_16_Ky1.Value == "0") tbMON_16_Ky1.Text = "CĐ";
                    else if (hdMON_16_Ky1.Value == "1") tbMON_16_Ky1.Text = "Đ";
                    if (hdMON_16_Ky2.Value == "0") tbMON_16_Ky1.Text = "CĐ";
                    else if (hdMON_16_Ky2.Value == "1") tbMON_16_Ky2.Text = "Đ";
                    if (hdMON_16_CN.Value == "0") tbMON_16_CN.Text = "CĐ";
                    else if (hdMON_16_CN.Value == "1") tbMON_16_CN.Text = "Đ";
                }
                #endregion
                #region "Mon 17"
                if (hdKIEU_MON_17.Value == null || hdKIEU_MON_17.Value == "" || hdKIEU_MON_17.Value == "0")
                {
                    if (hdMON_17_Ky1.Value != "")
                        tbMON_17_Ky1.Text = Math.Round(Convert.ToDecimal(hdMON_17_Ky1.Value), 1).ToString();
                    if (hdMON_17_Ky2.Value != "")
                        tbMON_17_Ky2.Text = Math.Round(Convert.ToDecimal(hdMON_17_Ky2.Value), 1).ToString();
                    if (hdMON_17_CN.Value != "")
                        tbMON_17_CN.Text = Math.Round(Convert.ToDecimal(hdMON_17_CN.Value), 1).ToString();
                }
                else if (hdKIEU_MON_17.Value == "1")
                {
                    if (hdMON_17_Ky1.Value == "0") tbMON_17_Ky1.Text = "CĐ";
                    else if (hdMON_17_Ky1.Value == "1") tbMON_17_Ky1.Text = "Đ";
                    if (hdMON_17_Ky2.Value == "0") tbMON_17_Ky1.Text = "CĐ";
                    else if (hdMON_17_Ky2.Value == "1") tbMON_17_Ky2.Text = "Đ";
                    if (hdMON_17_CN.Value == "0") tbMON_17_CN.Text = "CĐ";
                    else if (hdMON_17_CN.Value == "1") tbMON_17_CN.Text = "Đ";
                }
                #endregion
                #region "Mon 18"
                if (hdKIEU_MON_18.Value == null || hdKIEU_MON_18.Value == "" || hdKIEU_MON_18.Value == "0")
                {
                    if (hdMON_18_Ky1.Value != "")
                        tbMON_18_Ky1.Text = Math.Round(Convert.ToDecimal(hdMON_18_Ky1.Value), 1).ToString();
                    if (hdMON_18_Ky2.Value != "")
                        tbMON_18_Ky2.Text = Math.Round(Convert.ToDecimal(hdMON_18_Ky2.Value), 1).ToString();
                    if (hdMON_18_CN.Value != "")
                        tbMON_18_CN.Text = Math.Round(Convert.ToDecimal(hdMON_18_CN.Value), 1).ToString();
                }
                else if (hdKIEU_MON_18.Value == "1")
                {
                    if (hdMON_18_Ky1.Value == "0") tbMON_18_Ky1.Text = "CĐ";
                    else if (hdMON_18_Ky1.Value == "1") tbMON_18_Ky1.Text = "Đ";
                    if (hdMON_18_Ky2.Value == "0") tbMON_18_Ky1.Text = "CĐ";
                    else if (hdMON_18_Ky2.Value == "1") tbMON_18_Ky2.Text = "Đ";
                    if (hdMON_18_CN.Value == "0") tbMON_18_CN.Text = "CĐ";
                    else if (hdMON_18_CN.Value == "1") tbMON_18_CN.Text = "Đ";
                }
                #endregion
                #region "Mon 19"
                if (hdKIEU_MON_19.Value == null || hdKIEU_MON_19.Value == "" || hdKIEU_MON_19.Value == "0")
                {
                    if (hdMON_19_Ky1.Value != "")
                        tbMON_19_Ky1.Text = Math.Round(Convert.ToDecimal(hdMON_19_Ky1.Value), 1).ToString();
                    if (hdMON_19_Ky2.Value != "")
                        tbMON_19_Ky2.Text = Math.Round(Convert.ToDecimal(hdMON_19_Ky2.Value), 1).ToString();
                    if (hdMON_19_CN.Value != "")
                        tbMON_19_CN.Text = Math.Round(Convert.ToDecimal(hdMON_19_CN.Value), 1).ToString();
                }
                else if (hdKIEU_MON_19.Value == "1")
                {
                    if (hdMON_19_Ky1.Value == "0") tbMON_19_Ky1.Text = "CĐ";
                    else if (hdMON_19_Ky1.Value == "1") tbMON_19_Ky1.Text = "Đ";
                    if (hdMON_19_Ky2.Value == "0") tbMON_19_Ky1.Text = "CĐ";
                    else if (hdMON_19_Ky2.Value == "1") tbMON_19_Ky2.Text = "Đ";
                    if (hdMON_19_CN.Value == "0") tbMON_19_CN.Text = "CĐ";
                    else if (hdMON_19_CN.Value == "1") tbMON_19_CN.Text = "Đ";
                }
                #endregion
                #region "Mon 20"
                if (hdKIEU_MON_20.Value == null || hdKIEU_MON_20.Value == "" || hdKIEU_MON_20.Value == "0")
                {
                    if (hdMON_20_Ky1.Value != "")
                        tbMON_20_Ky1.Text = Math.Round(Convert.ToDecimal(hdMON_20_Ky1.Value), 1).ToString();
                    if (hdMON_20_Ky2.Value != "")
                        tbMON_20_Ky2.Text = Math.Round(Convert.ToDecimal(hdMON_20_Ky2.Value), 1).ToString();
                    if (hdMON_20_CN.Value != "")
                        tbMON_20_CN.Text = Math.Round(Convert.ToDecimal(hdMON_20_CN.Value), 1).ToString();
                }
                else if (hdKIEU_MON_20.Value == "1")
                {
                    if (hdMON_20_Ky1.Value == "0") tbMON_20_Ky1.Text = "CĐ";
                    else if (hdMON_20_Ky1.Value == "1") tbMON_20_Ky1.Text = "Đ";
                    if (hdMON_20_Ky2.Value == "0") tbMON_20_Ky1.Text = "CĐ";
                    else if (hdMON_20_Ky2.Value == "1") tbMON_20_Ky2.Text = "Đ";
                    if (hdMON_20_CN.Value == "0") tbMON_20_CN.Text = "CĐ";
                    else if (hdMON_20_CN.Value == "1") tbMON_20_CN.Text = "Đ";
                }
                #endregion
                #region "Mon 21"
                if (hdKIEU_MON_21.Value == null || hdKIEU_MON_21.Value == "" || hdKIEU_MON_21.Value == "0")
                {
                    if (hdMON_21_Ky1.Value != "")
                        tbMON_21_Ky1.Text = Math.Round(Convert.ToDecimal(hdMON_21_Ky1.Value), 1).ToString();
                    if (hdMON_21_Ky2.Value != "")
                        tbMON_21_Ky2.Text = Math.Round(Convert.ToDecimal(hdMON_21_Ky2.Value), 1).ToString();
                    if (hdMON_21_CN.Value != "")
                        tbMON_21_CN.Text = Math.Round(Convert.ToDecimal(hdMON_21_CN.Value), 1).ToString();
                }
                else if (hdKIEU_MON_21.Value == "1")
                {
                    if (hdMON_21_Ky1.Value == "0") tbMON_21_Ky1.Text = "CĐ";
                    else if (hdMON_21_Ky1.Value == "1") tbMON_21_Ky1.Text = "Đ";
                    if (hdMON_21_Ky2.Value == "0") tbMON_21_Ky1.Text = "CĐ";
                    else if (hdMON_21_Ky2.Value == "1") tbMON_21_Ky2.Text = "Đ";
                    if (hdMON_21_CN.Value == "0") tbMON_21_CN.Text = "CĐ";
                    else if (hdMON_21_CN.Value == "1") tbMON_21_CN.Text = "Đ";
                }
                #endregion
                #region "Mon 22"
                if (hdKIEU_MON_22.Value == null || hdKIEU_MON_22.Value == "" || hdKIEU_MON_22.Value == "0")
                {
                    if (hdMON_22_Ky1.Value != "")
                        tbMON_22_Ky1.Text = Math.Round(Convert.ToDecimal(hdMON_22_Ky1.Value), 1).ToString();
                    if (hdMON_22_Ky2.Value != "")
                        tbMON_22_Ky2.Text = Math.Round(Convert.ToDecimal(hdMON_22_Ky2.Value), 1).ToString();
                    if (hdMON_22_CN.Value != "")
                        tbMON_22_CN.Text = Math.Round(Convert.ToDecimal(hdMON_22_CN.Value), 1).ToString();
                }
                else if (hdKIEU_MON_22.Value == "1")
                {
                    if (hdMON_22_Ky1.Value == "0") tbMON_22_Ky1.Text = "CĐ";
                    else if (hdMON_22_Ky1.Value == "1") tbMON_22_Ky1.Text = "Đ";
                    if (hdMON_22_Ky2.Value == "0") tbMON_22_Ky1.Text = "CĐ";
                    else if (hdMON_22_Ky2.Value == "1") tbMON_22_Ky2.Text = "Đ";
                    if (hdMON_22_CN.Value == "0") tbMON_22_CN.Text = "CĐ";
                    else if (hdMON_22_CN.Value == "1") tbMON_22_CN.Text = "Đ";
                }
                #endregion
                #region "Mon 23"
                if (hdKIEU_MON_23.Value == null || hdKIEU_MON_23.Value == "" || hdKIEU_MON_23.Value == "0")
                {
                    if (hdMON_23_Ky1.Value != "")
                        tbMON_23_Ky1.Text = Math.Round(Convert.ToDecimal(hdMON_23_Ky1.Value), 1).ToString();
                    if (hdMON_23_Ky2.Value != "")
                        tbMON_23_Ky2.Text = Math.Round(Convert.ToDecimal(hdMON_23_Ky2.Value), 1).ToString();
                    if (hdMON_23_CN.Value != "")
                        tbMON_23_CN.Text = Math.Round(Convert.ToDecimal(hdMON_23_CN.Value), 1).ToString();
                }
                else if (hdKIEU_MON_23.Value == "1")
                {
                    if (hdMON_23_Ky1.Value == "0") tbMON_23_Ky1.Text = "CĐ";
                    else if (hdMON_23_Ky1.Value == "1") tbMON_23_Ky1.Text = "Đ";
                    if (hdMON_23_Ky2.Value == "0") tbMON_23_Ky1.Text = "CĐ";
                    else if (hdMON_23_Ky2.Value == "1") tbMON_23_Ky2.Text = "Đ";
                    if (hdMON_23_CN.Value == "0") tbMON_23_CN.Text = "CĐ";
                    else if (hdMON_23_CN.Value == "1") tbMON_23_CN.Text = "Đ";
                }
                #endregion
                #region "Mon 24"
                if (hdKIEU_MON_24.Value == null || hdKIEU_MON_24.Value == "" || hdKIEU_MON_24.Value == "0")
                {
                    if (hdMON_24_Ky1.Value != "")
                        tbMON_24_Ky1.Text = Math.Round(Convert.ToDecimal(hdMON_24_Ky1.Value), 1).ToString();
                    if (hdMON_24_Ky2.Value != "")
                        tbMON_24_Ky2.Text = Math.Round(Convert.ToDecimal(hdMON_24_Ky2.Value), 1).ToString();
                    if (hdMON_24_CN.Value != "")
                        tbMON_24_CN.Text = Math.Round(Convert.ToDecimal(hdMON_24_CN.Value), 1).ToString();
                }
                else if (hdKIEU_MON_24.Value == "1")
                {
                    if (hdMON_24_Ky1.Value == "0") tbMON_24_Ky1.Text = "CĐ";
                    else if (hdMON_24_Ky1.Value == "1") tbMON_24_Ky1.Text = "Đ";
                    if (hdMON_24_Ky2.Value == "0") tbMON_24_Ky1.Text = "CĐ";
                    else if (hdMON_24_Ky2.Value == "1") tbMON_24_Ky2.Text = "Đ";
                    if (hdMON_24_CN.Value == "0") tbMON_24_CN.Text = "CĐ";
                    else if (hdMON_24_CN.Value == "1") tbMON_24_CN.Text = "Đ";
                }
                #endregion
                #region "TB môn kỳ 1"
                if (hdTB_KY1.Value != null && hdTB_KY1.Value != "")
                {
                    tbTB_KY1.Text = Math.Round(Convert.ToDecimal(hdTB_KY1.Value), 1).ToString();
                }
                #endregion
                #region "TB môn kỳ 2"
                if (hdTB_KY2.Value != null && hdTB_KY2.Value != "")
                {
                    tbTB_KY2.Text = Math.Round(Convert.ToDecimal(hdTB_KY2.Value), 1).ToString();
                }
                #endregion
                #region "TB môn cả năm"
                if (hdTB_CN.Value != null && hdTB_CN.Value != "")
                {
                    tbTB_CN.Text = Math.Round(Convert.ToDecimal(hdTB_CN.Value), 1).ToString();
                }
                #endregion
                #endregion
                #region "set enable"
                tbMON_0_Ky1.Enabled = false;
                tbMON_1_Ky1.Enabled = false;
                tbMON_2_Ky1.Enabled = false;
                tbMON_3_Ky1.Enabled = false;
                tbMON_4_Ky1.Enabled = false;
                tbMON_5_Ky1.Enabled = false;
                tbMON_6_Ky1.Enabled = false;
                tbMON_7_Ky1.Enabled = false;
                tbMON_8_Ky1.Enabled = false;
                tbMON_9_Ky1.Enabled = false;
                tbMON_10_Ky1.Enabled = false;
                tbMON_11_Ky1.Enabled = false;
                tbMON_12_Ky1.Enabled = false;
                tbMON_13_Ky1.Enabled = false;
                tbMON_14_Ky1.Enabled = false;
                tbMON_15_Ky1.Enabled = false;
                tbMON_16_Ky1.Enabled = false;
                tbMON_17_Ky1.Enabled = false;
                tbMON_18_Ky1.Enabled = false;
                tbMON_19_Ky1.Enabled = false;
                tbMON_20_Ky1.Enabled = false;
                tbMON_21_Ky1.Enabled = false;
                tbMON_22_Ky1.Enabled = false;
                tbMON_23_Ky1.Enabled = false;
                tbMON_24_Ky1.Enabled = false;
                tbMON_0_Ky2.Enabled = false;
                tbMON_1_Ky2.Enabled = false;
                tbMON_2_Ky2.Enabled = false;
                tbMON_3_Ky2.Enabled = false;
                tbMON_4_Ky2.Enabled = false;
                tbMON_5_Ky2.Enabled = false;
                tbMON_6_Ky2.Enabled = false;
                tbMON_7_Ky2.Enabled = false;
                tbMON_8_Ky2.Enabled = false;
                tbMON_9_Ky2.Enabled = false;
                tbMON_10_Ky2.Enabled = false;
                tbMON_11_Ky2.Enabled = false;
                tbMON_12_Ky2.Enabled = false;
                tbMON_13_Ky2.Enabled = false;
                tbMON_14_Ky2.Enabled = false;
                tbMON_15_Ky2.Enabled = false;
                tbMON_16_Ky2.Enabled = false;
                tbMON_17_Ky2.Enabled = false;
                tbMON_18_Ky2.Enabled = false;
                tbMON_19_Ky2.Enabled = false;
                tbMON_20_Ky2.Enabled = false;
                tbMON_21_Ky2.Enabled = false;
                tbMON_22_Ky2.Enabled = false;
                tbMON_23_Ky2.Enabled = false;
                tbMON_24_Ky2.Enabled = false;
                tbMON_0_CN.Enabled = false;
                tbMON_1_CN.Enabled = false;
                tbMON_2_CN.Enabled = false;
                tbMON_3_CN.Enabled = false;
                tbMON_4_CN.Enabled = false;
                tbMON_5_CN.Enabled = false;
                tbMON_6_CN.Enabled = false;
                tbMON_7_CN.Enabled = false;
                tbMON_8_CN.Enabled = false;
                tbMON_9_CN.Enabled = false;
                tbMON_10_CN.Enabled = false;
                tbMON_11_CN.Enabled = false;
                tbMON_12_CN.Enabled = false;
                tbMON_13_CN.Enabled = false;
                tbMON_14_CN.Enabled = false;
                tbMON_15_CN.Enabled = false;
                tbMON_16_CN.Enabled = false;
                tbMON_17_CN.Enabled = false;
                tbMON_18_CN.Enabled = false;
                tbMON_19_CN.Enabled = false;
                tbMON_20_CN.Enabled = false;
                tbMON_21_CN.Enabled = false;
                tbMON_22_CN.Enabled = false;
                tbMON_23_CN.Enabled = false;
                tbMON_24_CN.Enabled = false;
                tbTB_KY1.Enabled = false;
                tbTB_KY2.Enabled = false;
                tbTB_CN.Enabled = false;
                #endregion
            }
        }
        protected void rcbKhoiHoc_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbLop.ClearSelection();
            rcbLop.Text = string.Empty;
            rcbLop.DataBind();
            RadGrid1.Rebind();
        }
        protected void rcbLop_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void rcbHocKy_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void btnTongKet_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.THEM) && !is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            int success = 0;
            long? id_lop = localAPI.ConvertStringTolong(rcbLop.SelectedValue);
            short? id_khoi = localAPI.ConvertStringToShort(rcbKhoiHoc.SelectedValue);
            if (id_lop != null && id_khoi != null)
            {
                List<DanhSachMonByLopEntity> lstMonLop = lopMonBO.getMonByLopTruongHocKy(Sys_This_Truong.ID, Convert.ToInt16(Sys_Ma_Nam_hoc), id_lop.Value, Convert.ToInt16(rcbHocKy.SelectedValue));
                if (lstMonLop.Count > 0)
                {
                    string ma_mon_hoc_truong = "";
                    for (int i = 0; i < lstMonLop.Count; i++)
                    {
                        ma_mon_hoc_truong += lstMonLop[i].ID_MON_TRUONG + ",";
                    }
                    ma_mon_hoc_truong = ma_mon_hoc_truong.TrimEnd(',');
                    ma_mon_hoc_truong = "(" + ma_mon_hoc_truong + ")";
                    foreach (GridDataItem item in RadGrid1.Items)
                    {
                        long id_hoc_sinh = Convert.ToInt64(item.GetDataKeyValue("ID").ToString());
                        #region get control Textbox
                        TextBox tbMON_0_Ky1 = (TextBox)item.FindControl("tbMON_0_Ky1");
                        TextBox tbMON_1_Ky1 = (TextBox)item.FindControl("tbMON_1_Ky1");
                        TextBox tbMON_2_Ky1 = (TextBox)item.FindControl("tbMON_2_Ky1");
                        TextBox tbMON_3_Ky1 = (TextBox)item.FindControl("tbMON_3_Ky1");
                        TextBox tbMON_4_Ky1 = (TextBox)item.FindControl("tbMON_4_Ky1");
                        TextBox tbMON_5_Ky1 = (TextBox)item.FindControl("tbMON_5_Ky1");
                        TextBox tbMON_6_Ky1 = (TextBox)item.FindControl("tbMON_6_Ky1");
                        TextBox tbMON_7_Ky1 = (TextBox)item.FindControl("tbMON_7_Ky1");
                        TextBox tbMON_8_Ky1 = (TextBox)item.FindControl("tbMON_8_Ky1");
                        TextBox tbMON_9_Ky1 = (TextBox)item.FindControl("tbMON_9_Ky1");
                        TextBox tbMON_10_Ky1 = (TextBox)item.FindControl("tbMON_10_Ky1");
                        TextBox tbMON_11_Ky1 = (TextBox)item.FindControl("tbMON_11_Ky1");
                        TextBox tbMON_12_Ky1 = (TextBox)item.FindControl("tbMON_12_Ky1");
                        TextBox tbMON_13_Ky1 = (TextBox)item.FindControl("tbMON_13_Ky1");
                        TextBox tbMON_14_Ky1 = (TextBox)item.FindControl("tbMON_14_Ky1");
                        TextBox tbMON_15_Ky1 = (TextBox)item.FindControl("tbMON_15_Ky1");
                        TextBox tbMON_16_Ky1 = (TextBox)item.FindControl("tbMON_16_Ky1");
                        TextBox tbMON_17_Ky1 = (TextBox)item.FindControl("tbMON_17_Ky1");
                        TextBox tbMON_18_Ky1 = (TextBox)item.FindControl("tbMON_18_Ky1");
                        TextBox tbMON_19_Ky1 = (TextBox)item.FindControl("tbMON_19_Ky1");
                        TextBox tbMON_20_Ky1 = (TextBox)item.FindControl("tbMON_20_Ky1");
                        TextBox tbMON_21_Ky1 = (TextBox)item.FindControl("tbMON_21_Ky1");
                        TextBox tbMON_22_Ky1 = (TextBox)item.FindControl("tbMON_22_Ky1");
                        TextBox tbMON_23_Ky1 = (TextBox)item.FindControl("tbMON_23_Ky1");
                        TextBox tbMON_24_Ky1 = (TextBox)item.FindControl("tbMON_24_Ky1");
                        TextBox tbMON_0_Ky2 = (TextBox)item.FindControl("tbMON_0_Ky2");
                        TextBox tbMON_1_Ky2 = (TextBox)item.FindControl("tbMON_1_Ky2");
                        TextBox tbMON_2_Ky2 = (TextBox)item.FindControl("tbMON_2_Ky2");
                        TextBox tbMON_3_Ky2 = (TextBox)item.FindControl("tbMON_3_Ky2");
                        TextBox tbMON_4_Ky2 = (TextBox)item.FindControl("tbMON_4_Ky2");
                        TextBox tbMON_5_Ky2 = (TextBox)item.FindControl("tbMON_5_Ky2");
                        TextBox tbMON_6_Ky2 = (TextBox)item.FindControl("tbMON_6_Ky2");
                        TextBox tbMON_7_Ky2 = (TextBox)item.FindControl("tbMON_7_Ky2");
                        TextBox tbMON_8_Ky2 = (TextBox)item.FindControl("tbMON_8_Ky2");
                        TextBox tbMON_9_Ky2 = (TextBox)item.FindControl("tbMON_9_Ky2");
                        TextBox tbMON_10_Ky2 = (TextBox)item.FindControl("tbMON_10_Ky2");
                        TextBox tbMON_11_Ky2 = (TextBox)item.FindControl("tbMON_11_Ky2");
                        TextBox tbMON_12_Ky2 = (TextBox)item.FindControl("tbMON_12_Ky2");
                        TextBox tbMON_13_Ky2 = (TextBox)item.FindControl("tbMON_13_Ky2");
                        TextBox tbMON_14_Ky2 = (TextBox)item.FindControl("tbMON_14_Ky2");
                        TextBox tbMON_15_Ky2 = (TextBox)item.FindControl("tbMON_15_Ky2");
                        TextBox tbMON_16_Ky2 = (TextBox)item.FindControl("tbMON_16_Ky2");
                        TextBox tbMON_17_Ky2 = (TextBox)item.FindControl("tbMON_17_Ky2");
                        TextBox tbMON_18_Ky2 = (TextBox)item.FindControl("tbMON_18_Ky2");
                        TextBox tbMON_19_Ky2 = (TextBox)item.FindControl("tbMON_19_Ky2");
                        TextBox tbMON_20_Ky2 = (TextBox)item.FindControl("tbMON_20_Ky2");
                        TextBox tbMON_21_Ky2 = (TextBox)item.FindControl("tbMON_21_Ky2");
                        TextBox tbMON_22_Ky2 = (TextBox)item.FindControl("tbMON_22_Ky2");
                        TextBox tbMON_23_Ky2 = (TextBox)item.FindControl("tbMON_23_Ky2");
                        TextBox tbMON_24_Ky2 = (TextBox)item.FindControl("tbMON_24_Ky2");
                        TextBox tbMON_0_CN = (TextBox)item.FindControl("tbMON_0_CN");
                        TextBox tbMON_1_CN = (TextBox)item.FindControl("tbMON_1_CN");
                        TextBox tbMON_2_CN = (TextBox)item.FindControl("tbMON_2_CN");
                        TextBox tbMON_3_CN = (TextBox)item.FindControl("tbMON_3_CN");
                        TextBox tbMON_4_CN = (TextBox)item.FindControl("tbMON_4_CN");
                        TextBox tbMON_5_CN = (TextBox)item.FindControl("tbMON_5_CN");
                        TextBox tbMON_6_CN = (TextBox)item.FindControl("tbMON_6_CN");
                        TextBox tbMON_7_CN = (TextBox)item.FindControl("tbMON_7_CN");
                        TextBox tbMON_8_CN = (TextBox)item.FindControl("tbMON_8_CN");
                        TextBox tbMON_9_CN = (TextBox)item.FindControl("tbMON_9_CN");
                        TextBox tbMON_10_CN = (TextBox)item.FindControl("tbMON_10_CN");
                        TextBox tbMON_11_CN = (TextBox)item.FindControl("tbMON_11_CN");
                        TextBox tbMON_12_CN = (TextBox)item.FindControl("tbMON_12_CN");
                        TextBox tbMON_13_CN = (TextBox)item.FindControl("tbMON_13_CN");
                        TextBox tbMON_14_CN = (TextBox)item.FindControl("tbMON_14_CN");
                        TextBox tbMON_15_CN = (TextBox)item.FindControl("tbMON_15_CN");
                        TextBox tbMON_16_CN = (TextBox)item.FindControl("tbMON_16_CN");
                        TextBox tbMON_17_CN = (TextBox)item.FindControl("tbMON_17_CN");
                        TextBox tbMON_18_CN = (TextBox)item.FindControl("tbMON_18_CN");
                        TextBox tbMON_19_CN = (TextBox)item.FindControl("tbMON_19_CN");
                        TextBox tbMON_20_CN = (TextBox)item.FindControl("tbMON_20_CN");
                        TextBox tbMON_21_CN = (TextBox)item.FindControl("tbMON_21_CN");
                        TextBox tbMON_22_CN = (TextBox)item.FindControl("tbMON_22_CN");
                        TextBox tbMON_23_CN = (TextBox)item.FindControl("tbMON_23_CN");
                        TextBox tbMON_24_CN = (TextBox)item.FindControl("tbMON_24_CN");
                        TextBox tbTB_KY1 = (TextBox)item.FindControl("tbTB_KY1");
                        TextBox tbTB_KY2 = (TextBox)item.FindControl("tbTB_KY2");
                        TextBox tbTB_CN = (TextBox)item.FindControl("tbTB_CN");
                        #endregion
                        #region "get control hidden kieu mon"
                        HiddenField hdKIEU_MON_0 = (HiddenField)item.FindControl("hdKIEU_MON_0");
                        HiddenField hdKIEU_MON_1 = (HiddenField)item.FindControl("hdKIEU_MON_1");
                        HiddenField hdKIEU_MON_2 = (HiddenField)item.FindControl("hdKIEU_MON_2");
                        HiddenField hdKIEU_MON_3 = (HiddenField)item.FindControl("hdKIEU_MON_3");
                        HiddenField hdKIEU_MON_4 = (HiddenField)item.FindControl("hdKIEU_MON_4");
                        HiddenField hdKIEU_MON_5 = (HiddenField)item.FindControl("hdKIEU_MON_5");
                        HiddenField hdKIEU_MON_6 = (HiddenField)item.FindControl("hdKIEU_MON_6");
                        HiddenField hdKIEU_MON_7 = (HiddenField)item.FindControl("hdKIEU_MON_7");
                        HiddenField hdKIEU_MON_8 = (HiddenField)item.FindControl("hdKIEU_MON_8");
                        HiddenField hdKIEU_MON_9 = (HiddenField)item.FindControl("hdKIEU_MON_9");
                        HiddenField hdKIEU_MON_10 = (HiddenField)item.FindControl("hdKIEU_MON_10");
                        HiddenField hdKIEU_MON_11 = (HiddenField)item.FindControl("hdKIEU_MON_11");
                        HiddenField hdKIEU_MON_12 = (HiddenField)item.FindControl("hdKIEU_MON_12");
                        HiddenField hdKIEU_MON_13 = (HiddenField)item.FindControl("hdKIEU_MON_13");
                        HiddenField hdKIEU_MON_14 = (HiddenField)item.FindControl("hdKIEU_MON_14");
                        HiddenField hdKIEU_MON_15 = (HiddenField)item.FindControl("hdKIEU_MON_15");
                        HiddenField hdKIEU_MON_16 = (HiddenField)item.FindControl("hdKIEU_MON_16");
                        HiddenField hdKIEU_MON_17 = (HiddenField)item.FindControl("hdKIEU_MON_17");
                        HiddenField hdKIEU_MON_18 = (HiddenField)item.FindControl("hdKIEU_MON_18");
                        HiddenField hdKIEU_MON_19 = (HiddenField)item.FindControl("hdKIEU_MON_19");
                        HiddenField hdKIEU_MON_20 = (HiddenField)item.FindControl("hdKIEU_MON_20");
                        HiddenField hdKIEU_MON_21 = (HiddenField)item.FindControl("hdKIEU_MON_21");
                        HiddenField hdKIEU_MON_22 = (HiddenField)item.FindControl("hdKIEU_MON_22");
                        HiddenField hdKIEU_MON_23 = (HiddenField)item.FindControl("hdKIEU_MON_23");
                        HiddenField hdKIEU_MON_24 = (HiddenField)item.FindControl("hdKIEU_MON_24");
                        HiddenField hdTB_KY1 = (HiddenField)item.FindControl("hdTB_KY1");
                        HiddenField hdTB_KY2 = (HiddenField)item.FindControl("hdTB_KY2");
                        HiddenField hdTB_CN = (HiddenField)item.FindControl("hdTB_CN");
                        #endregion
                        #region "get control hidden he so"
                        HiddenField hdHE_SO_0 = (HiddenField)item.FindControl("hdHE_SO_0");
                        HiddenField hdHE_SO_1 = (HiddenField)item.FindControl("hdHE_SO_1");
                        HiddenField hdHE_SO_2 = (HiddenField)item.FindControl("hdHE_SO_2");
                        HiddenField hdHE_SO_3 = (HiddenField)item.FindControl("hdHE_SO_3");
                        HiddenField hdHE_SO_4 = (HiddenField)item.FindControl("hdHE_SO_4");
                        HiddenField hdHE_SO_5 = (HiddenField)item.FindControl("hdHE_SO_5");
                        HiddenField hdHE_SO_6 = (HiddenField)item.FindControl("hdHE_SO_6");
                        HiddenField hdHE_SO_7 = (HiddenField)item.FindControl("hdHE_SO_7");
                        HiddenField hdHE_SO_8 = (HiddenField)item.FindControl("hdHE_SO_8");
                        HiddenField hdHE_SO_9 = (HiddenField)item.FindControl("hdHE_SO_9");
                        HiddenField hdHE_SO_10 = (HiddenField)item.FindControl("hdHE_SO_10");
                        HiddenField hdHE_SO_11 = (HiddenField)item.FindControl("hdHE_SO_11");
                        HiddenField hdHE_SO_12 = (HiddenField)item.FindControl("hdHE_SO_12");
                        HiddenField hdHE_SO_13 = (HiddenField)item.FindControl("hdHE_SO_13");
                        HiddenField hdHE_SO_14 = (HiddenField)item.FindControl("hdHE_SO_14");
                        HiddenField hdHE_SO_15 = (HiddenField)item.FindControl("hdHE_SO_15");
                        HiddenField hdHE_SO_16 = (HiddenField)item.FindControl("hdHE_SO_16");
                        HiddenField hdHE_SO_17 = (HiddenField)item.FindControl("hdHE_SO_17");
                        HiddenField hdHE_SO_18 = (HiddenField)item.FindControl("hdHE_SO_18");
                        HiddenField hdHE_SO_19 = (HiddenField)item.FindControl("hdHE_SO_19");
                        HiddenField hdHE_SO_20 = (HiddenField)item.FindControl("hdHE_SO_20");
                        HiddenField hdHE_SO_21 = (HiddenField)item.FindControl("hdHE_SO_21");
                        HiddenField hdHE_SO_22 = (HiddenField)item.FindControl("hdHE_SO_22");
                        HiddenField hdHE_SO_23 = (HiddenField)item.FindControl("hdHE_SO_23");
                        HiddenField hdHE_SO_24 = (HiddenField)item.FindControl("hdHE_SO_24");
                        #endregion
                        #region get control môn chuyên
                        HiddenField hdIS_MON_CHUYEN_0 = (HiddenField)item.FindControl("hdIS_MON_CHUYEN_0");
                        HiddenField hdIS_MON_CHUYEN_1 = (HiddenField)item.FindControl("hdIS_MON_CHUYEN_1");
                        HiddenField hdIS_MON_CHUYEN_2 = (HiddenField)item.FindControl("hdIS_MON_CHUYEN_2");
                        HiddenField hdIS_MON_CHUYEN_3 = (HiddenField)item.FindControl("hdIS_MON_CHUYEN_3");
                        HiddenField hdIS_MON_CHUYEN_4 = (HiddenField)item.FindControl("hdIS_MON_CHUYEN_4");
                        HiddenField hdIS_MON_CHUYEN_5 = (HiddenField)item.FindControl("hdIS_MON_CHUYEN_5");
                        HiddenField hdIS_MON_CHUYEN_6 = (HiddenField)item.FindControl("hdIS_MON_CHUYEN_6");
                        HiddenField hdIS_MON_CHUYEN_7 = (HiddenField)item.FindControl("hdIS_MON_CHUYEN_7");
                        HiddenField hdIS_MON_CHUYEN_8 = (HiddenField)item.FindControl("hdIS_MON_CHUYEN_8");
                        HiddenField hdIS_MON_CHUYEN_9 = (HiddenField)item.FindControl("hdIS_MON_CHUYEN_9");
                        HiddenField hdIS_MON_CHUYEN_10 = (HiddenField)item.FindControl("hdIS_MON_CHUYEN_10");
                        HiddenField hdIS_MON_CHUYEN_11 = (HiddenField)item.FindControl("hdIS_MON_CHUYEN_11");
                        HiddenField hdIS_MON_CHUYEN_12 = (HiddenField)item.FindControl("hdIS_MON_CHUYEN_12");
                        HiddenField hdIS_MON_CHUYEN_13 = (HiddenField)item.FindControl("hdIS_MON_CHUYEN_13");
                        HiddenField hdIS_MON_CHUYEN_14 = (HiddenField)item.FindControl("hdIS_MON_CHUYEN_14");
                        HiddenField hdIS_MON_CHUYEN_15 = (HiddenField)item.FindControl("hdIS_MON_CHUYEN_15");
                        HiddenField hdIS_MON_CHUYEN_16 = (HiddenField)item.FindControl("hdIS_MON_CHUYEN_16");
                        HiddenField hdIS_MON_CHUYEN_17 = (HiddenField)item.FindControl("hdIS_MON_CHUYEN_17");
                        HiddenField hdIS_MON_CHUYEN_18 = (HiddenField)item.FindControl("hdIS_MON_CHUYEN_18");
                        HiddenField hdIS_MON_CHUYEN_19 = (HiddenField)item.FindControl("hdIS_MON_CHUYEN_19");
                        HiddenField hdIS_MON_CHUYEN_20 = (HiddenField)item.FindControl("hdIS_MON_CHUYEN_20");
                        HiddenField hdIS_MON_CHUYEN_21 = (HiddenField)item.FindControl("hdIS_MON_CHUYEN_21");
                        HiddenField hdIS_MON_CHUYEN_22 = (HiddenField)item.FindControl("hdIS_MON_CHUYEN_22");
                        HiddenField hdIS_MON_CHUYEN_23 = (HiddenField)item.FindControl("hdIS_MON_CHUYEN_23");
                        HiddenField hdIS_MON_CHUYEN_24 = (HiddenField)item.FindControl("hdIS_MON_CHUYEN_24");
                        #endregion
                        #region "tinh diem, he so"
                        decimal count_he_so_ky1 = 0;
                        decimal count_he_so_ky2 = 0;
                        decimal diem_tb_1 = 0, diem_tb_2 = 0;
                        #region tính điểm kỳ 1
                        if (rcbHocKy.SelectedValue == "1")
                        {
                            if ((hdKIEU_MON_0.Value == "" || hdKIEU_MON_0.Value == "0") && tbMON_0_Ky1.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_0.Value == "1")
                                {
                                    diem_tb_1 += Convert.ToDecimal(tbMON_0_Ky1.Text) * 2;
                                    count_he_so_ky1 += 2;
                                }
                                else
                                {
                                    diem_tb_1 += hdHE_SO_0.Value != "" ? Convert.ToDecimal(tbMON_0_Ky1.Text) * Convert.ToDecimal(hdHE_SO_0.Value) : Convert.ToDecimal(tbMON_0_Ky1.Text);
                                    count_he_so_ky1 += hdHE_SO_0.Value != "" ? Convert.ToDecimal(hdHE_SO_0.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_1.Value == "" || hdKIEU_MON_1.Value == "0") && tbMON_1_Ky1.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_1.Value == "1")
                                {
                                    diem_tb_1 += Convert.ToDecimal(tbMON_1_Ky1.Text) * 2;
                                    count_he_so_ky1 += 2;
                                }
                                else
                                {
                                    diem_tb_1 += hdHE_SO_1.Value != "" ? Convert.ToDecimal(tbMON_1_Ky1.Text) * Convert.ToDecimal(hdHE_SO_1.Value) : Convert.ToDecimal(tbMON_1_Ky1.Text);
                                    count_he_so_ky1 += hdHE_SO_1.Value != "" ? Convert.ToDecimal(hdHE_SO_1.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_2.Value == "" || hdKIEU_MON_2.Value == "0") && tbMON_2_Ky1.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_2.Value == "1")
                                {
                                    diem_tb_1 += Convert.ToDecimal(tbMON_2_Ky1.Text) * 2;
                                    count_he_so_ky1 += 2;
                                }
                                else
                                {
                                    diem_tb_1 += hdHE_SO_2.Value != "" ? Convert.ToDecimal(tbMON_2_Ky1.Text) * Convert.ToDecimal(hdHE_SO_2.Value) : Convert.ToDecimal(tbMON_2_Ky1.Text);
                                    count_he_so_ky1 += hdHE_SO_2.Value != "" ? Convert.ToDecimal(hdHE_SO_2.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_3.Value == "" || hdKIEU_MON_3.Value == "0") && tbMON_3_Ky1.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_3.Value == "1")
                                {
                                    diem_tb_1 += Convert.ToDecimal(tbMON_3_Ky1.Text) * 2;
                                    count_he_so_ky1 += 2;
                                }
                                else
                                {
                                    diem_tb_1 += hdHE_SO_3.Value != "" ? Convert.ToDecimal(tbMON_3_Ky1.Text) * Convert.ToDecimal(hdHE_SO_3.Value) : Convert.ToDecimal(tbMON_3_Ky1.Text);
                                    count_he_so_ky1 += hdHE_SO_3.Value != "" ? Convert.ToDecimal(hdHE_SO_3.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_4.Value == "" || hdKIEU_MON_4.Value == "0") && tbMON_4_Ky1.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_4.Value == "1")
                                {
                                    diem_tb_1 += Convert.ToDecimal(tbMON_4_Ky1.Text) * 2;
                                    count_he_so_ky1 += 2;
                                }
                                else
                                {
                                    diem_tb_1 += hdHE_SO_4.Value != "" ? Convert.ToDecimal(tbMON_4_Ky1.Text) * Convert.ToDecimal(hdHE_SO_4.Value) : Convert.ToDecimal(tbMON_4_Ky1.Text);
                                    count_he_so_ky1 += hdHE_SO_4.Value != "" ? Convert.ToDecimal(hdHE_SO_4.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_5.Value == "" || hdKIEU_MON_5.Value == "0") && tbMON_5_Ky1.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_5.Value == "1")
                                {
                                    diem_tb_1 += Convert.ToDecimal(tbMON_5_Ky1.Text) * 2;
                                    count_he_so_ky1 += 2;
                                }
                                else
                                {
                                    diem_tb_1 += hdHE_SO_5.Value != "" ? Convert.ToDecimal(tbMON_5_Ky1.Text) * Convert.ToDecimal(hdHE_SO_5.Value) : Convert.ToDecimal(tbMON_5_Ky1.Text);
                                    count_he_so_ky1 += hdHE_SO_5.Value != "" ? Convert.ToDecimal(hdHE_SO_5.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_6.Value == "" || hdKIEU_MON_6.Value == "0") && tbMON_6_Ky1.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_6.Value == "1")
                                {
                                    diem_tb_1 += Convert.ToDecimal(tbMON_6_Ky1.Text) * 2;
                                    count_he_so_ky1 += 2;
                                }
                                else
                                {
                                    diem_tb_1 += hdHE_SO_6.Value != "" ? Convert.ToDecimal(tbMON_6_Ky1.Text) * Convert.ToDecimal(hdHE_SO_6.Value) : Convert.ToDecimal(tbMON_6_Ky1.Text);
                                    count_he_so_ky1 += hdHE_SO_6.Value != "" ? Convert.ToDecimal(hdHE_SO_6.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_7.Value == "" || hdKIEU_MON_7.Value == "0") && tbMON_7_Ky1.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_7.Value == "1")
                                {
                                    diem_tb_1 += Convert.ToDecimal(tbMON_7_Ky1.Text) * 2;
                                    count_he_so_ky1 += 2;
                                }
                                else
                                {
                                    diem_tb_1 += hdHE_SO_7.Value != "" ? Convert.ToDecimal(tbMON_7_Ky1.Text) * Convert.ToDecimal(hdHE_SO_7.Value) : Convert.ToDecimal(tbMON_7_Ky1.Text);
                                    count_he_so_ky1 += hdHE_SO_7.Value != "" ? Convert.ToDecimal(hdHE_SO_7.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_8.Value == "" || hdKIEU_MON_8.Value == "0") && tbMON_8_Ky1.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_8.Value == "1")
                                {
                                    diem_tb_1 += Convert.ToDecimal(tbMON_8_Ky1.Text) * 2;
                                    count_he_so_ky1 += 2;
                                }
                                else
                                {
                                    diem_tb_1 += hdHE_SO_8.Value != "" ? Convert.ToDecimal(tbMON_8_Ky1.Text) * Convert.ToDecimal(hdHE_SO_8.Value) : Convert.ToDecimal(tbMON_8_Ky1.Text);
                                    count_he_so_ky1 += hdHE_SO_8.Value != "" ? Convert.ToDecimal(hdHE_SO_8.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_9.Value == "" || hdKIEU_MON_9.Value == "0") && tbMON_9_Ky1.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_9.Value == "1")
                                {
                                    diem_tb_1 += Convert.ToDecimal(tbMON_9_Ky1.Text) * 2;
                                    count_he_so_ky1 += 2;
                                }
                                else
                                {
                                    diem_tb_1 += hdHE_SO_9.Value != "" ? Convert.ToDecimal(tbMON_9_Ky1.Text) * Convert.ToDecimal(hdHE_SO_9.Value) : Convert.ToDecimal(tbMON_9_Ky1.Text);
                                    count_he_so_ky1 += hdHE_SO_10.Value != "" ? Convert.ToDecimal(hdHE_SO_10.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_10.Value == "" || hdKIEU_MON_10.Value == "0") && tbMON_10_Ky1.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_10.Value == "1")
                                {
                                    diem_tb_1 += Convert.ToDecimal(tbMON_10_Ky1.Text) * 2;
                                    count_he_so_ky1 += 2;
                                }
                                else
                                {
                                    diem_tb_1 += hdHE_SO_10.Value != "" ? Convert.ToDecimal(tbMON_10_Ky1.Text) * Convert.ToDecimal(hdHE_SO_10.Value) : Convert.ToDecimal(tbMON_10_Ky1.Text);
                                    count_he_so_ky1 += hdHE_SO_11.Value != "" ? Convert.ToDecimal(hdHE_SO_11.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_11.Value == "" || hdKIEU_MON_11.Value == "0") && tbMON_11_Ky1.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_11.Value == "1")
                                {
                                    diem_tb_1 += Convert.ToDecimal(tbMON_11_Ky1.Text) * 2;
                                    count_he_so_ky1 += 2;
                                }
                                else
                                {
                                    diem_tb_1 += hdHE_SO_11.Value != "" ? Convert.ToDecimal(tbMON_11_Ky1.Text) * Convert.ToDecimal(hdHE_SO_11.Value) : Convert.ToDecimal(tbMON_11_Ky1.Text);
                                    count_he_so_ky1 += hdHE_SO_12.Value != "" ? Convert.ToDecimal(hdHE_SO_12.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_12.Value == "" || hdKIEU_MON_12.Value == "0") && tbMON_12_Ky1.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_12.Value == "1")
                                {
                                    diem_tb_1 += Convert.ToDecimal(tbMON_12_Ky1.Text) * 2;
                                    count_he_so_ky1 += 2;
                                }
                                else
                                {
                                    diem_tb_1 += hdHE_SO_12.Value != "" ? Convert.ToDecimal(tbMON_12_Ky1.Text) * Convert.ToDecimal(hdHE_SO_12.Value) : Convert.ToDecimal(tbMON_12_Ky1.Text);
                                    count_he_so_ky1 += hdHE_SO_13.Value != "" ? Convert.ToDecimal(hdHE_SO_13.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_13.Value == "" || hdKIEU_MON_13.Value == "0") && tbMON_13_Ky1.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_13.Value == "1")
                                {
                                    diem_tb_1 += Convert.ToDecimal(tbMON_13_Ky1.Text) * 2;
                                    count_he_so_ky1 += 2;
                                }
                                else
                                {
                                    diem_tb_1 += hdHE_SO_13.Value != "" ? Convert.ToDecimal(tbMON_13_Ky1.Text) * Convert.ToDecimal(hdHE_SO_13.Value) : Convert.ToDecimal(tbMON_13_Ky1.Text);
                                    count_he_so_ky1 += hdHE_SO_13.Value != "" ? Convert.ToDecimal(hdHE_SO_13.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_14.Value == "" || hdKIEU_MON_14.Value == "0") && tbMON_14_Ky1.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_14.Value == "1")
                                {
                                    diem_tb_1 += Convert.ToDecimal(tbMON_14_Ky1.Text) * 2;
                                    count_he_so_ky1 += 2;
                                }
                                else
                                {
                                    diem_tb_1 += hdHE_SO_14.Value != "" ? Convert.ToDecimal(tbMON_14_Ky1.Text) * Convert.ToDecimal(hdHE_SO_14.Value) : Convert.ToDecimal(tbMON_14_Ky1.Text);
                                    count_he_so_ky1 += hdHE_SO_14.Value != "" ? Convert.ToDecimal(hdHE_SO_14.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_15.Value == "" || hdKIEU_MON_15.Value == "0") && tbMON_15_Ky1.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_15.Value == "1")
                                {
                                    diem_tb_1 += Convert.ToDecimal(tbMON_15_Ky1.Text) * 2;
                                    count_he_so_ky1 += 2;
                                }
                                else
                                {
                                    diem_tb_1 += hdHE_SO_15.Value != "" ? Convert.ToDecimal(tbMON_15_Ky1.Text) * Convert.ToDecimal(hdHE_SO_15.Value) : Convert.ToDecimal(tbMON_15_Ky1.Text);
                                    count_he_so_ky1 += hdHE_SO_15.Value != "" ? Convert.ToDecimal(hdHE_SO_15.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_16.Value == "" || hdKIEU_MON_16.Value == "0") && tbMON_16_Ky1.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_16.Value == "1")
                                {
                                    diem_tb_1 += Convert.ToDecimal(tbMON_16_Ky1.Text) * 2;
                                    count_he_so_ky1 += 2;
                                }
                                else
                                {
                                    diem_tb_1 += hdHE_SO_16.Value != "" ? Convert.ToDecimal(tbMON_16_Ky1.Text) * Convert.ToDecimal(hdHE_SO_16.Value) : Convert.ToDecimal(tbMON_16_Ky1.Text);
                                    count_he_so_ky1 += hdHE_SO_16.Value != "" ? Convert.ToDecimal(hdHE_SO_16.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_17.Value == "" || hdKIEU_MON_17.Value == "0") && tbMON_17_Ky1.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_17.Value == "1")
                                {
                                    diem_tb_1 += Convert.ToDecimal(tbMON_17_Ky1.Text) * 2;
                                    count_he_so_ky1 += 2;
                                }
                                else
                                {
                                    diem_tb_1 += hdHE_SO_17.Value != "" ? Convert.ToDecimal(tbMON_17_Ky1.Text) * Convert.ToDecimal(hdHE_SO_17.Value) : Convert.ToDecimal(tbMON_17_Ky1.Text);
                                    count_he_so_ky1 += hdHE_SO_17.Value != "" ? Convert.ToDecimal(hdHE_SO_17.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_18.Value == "" || hdKIEU_MON_18.Value == "0") && tbMON_18_Ky1.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_18.Value == "1")
                                {
                                    diem_tb_1 += Convert.ToDecimal(tbMON_18_Ky1.Text) * 2;
                                    count_he_so_ky1 += 2;
                                }
                                else
                                {
                                    diem_tb_1 += hdHE_SO_18.Value != "" ? Convert.ToDecimal(tbMON_18_Ky1.Text) * Convert.ToDecimal(hdHE_SO_18.Value) : Convert.ToDecimal(tbMON_18_Ky1.Text);
                                    count_he_so_ky1 += hdHE_SO_18.Value != "" ? Convert.ToDecimal(hdHE_SO_18.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_19.Value == "" || hdKIEU_MON_19.Value == "0") && tbMON_19_Ky1.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_19.Value == "1")
                                {
                                    diem_tb_1 += Convert.ToDecimal(tbMON_19_Ky1.Text) * 2;
                                    count_he_so_ky1 += 2;
                                }
                                else
                                {
                                    diem_tb_1 += hdHE_SO_19.Value != "" ? Convert.ToDecimal(tbMON_19_Ky1.Text) * Convert.ToDecimal(hdHE_SO_19.Value) : Convert.ToDecimal(tbMON_19_Ky1.Text);
                                    count_he_so_ky1 += hdHE_SO_19.Value != "" ? Convert.ToDecimal(hdHE_SO_19.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_20.Value == "" || hdKIEU_MON_20.Value == "0") && tbMON_20_Ky1.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_20.Value == "1")
                                {
                                    diem_tb_1 += Convert.ToDecimal(tbMON_20_Ky1.Text) * 2;
                                    count_he_so_ky1 += 2;
                                }
                                else
                                {
                                    diem_tb_1 += hdHE_SO_20.Value != "" ? Convert.ToDecimal(tbMON_20_Ky1.Text) * Convert.ToDecimal(hdHE_SO_20.Value) : Convert.ToDecimal(tbMON_20_Ky1.Text);
                                    count_he_so_ky1 += hdHE_SO_20.Value != "" ? Convert.ToDecimal(hdHE_SO_20.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_21.Value == "" || hdKIEU_MON_21.Value == "0") && tbMON_21_Ky1.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_21.Value == "1")
                                {
                                    diem_tb_1 += Convert.ToDecimal(tbMON_21_Ky1.Text) * 2;
                                    count_he_so_ky1 += 2;
                                }
                                else
                                {
                                    diem_tb_1 += hdHE_SO_21.Value != "" ? Convert.ToDecimal(tbMON_21_Ky1.Text) * Convert.ToDecimal(hdHE_SO_21.Value) : Convert.ToDecimal(tbMON_21_Ky1.Text);
                                    count_he_so_ky1 += hdHE_SO_21.Value != "" ? Convert.ToDecimal(hdHE_SO_21.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_22.Value == "" || hdKIEU_MON_22.Value == "0") && tbMON_22_Ky1.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_22.Value == "1")
                                {
                                    diem_tb_1 += Convert.ToDecimal(tbMON_22_Ky1.Text) * 2;
                                    count_he_so_ky1 += 2;
                                }
                                else
                                {
                                    diem_tb_1 += hdHE_SO_22.Value != "" ? Convert.ToDecimal(tbMON_22_Ky1.Text) * Convert.ToDecimal(hdHE_SO_22.Value) : Convert.ToDecimal(tbMON_22_Ky1.Text);
                                    count_he_so_ky1 += hdHE_SO_22.Value != "" ? Convert.ToDecimal(hdHE_SO_22.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_23.Value == "" || hdKIEU_MON_23.Value == "0") && tbMON_23_Ky1.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_23.Value == "1")
                                {
                                    diem_tb_1 += Convert.ToDecimal(tbMON_23_Ky1.Text) * 2;
                                    count_he_so_ky1 += 2;
                                }
                                else
                                {
                                    diem_tb_1 += hdHE_SO_23.Value != "" ? Convert.ToDecimal(tbMON_23_Ky1.Text) * Convert.ToDecimal(hdHE_SO_23.Value) : Convert.ToDecimal(tbMON_23_Ky1.Text);
                                    count_he_so_ky1 += hdHE_SO_23.Value != "" ? Convert.ToDecimal(hdHE_SO_23.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_24.Value == "" || hdKIEU_MON_24.Value == "0") && tbMON_24_Ky1.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_24.Value == "1")
                                {
                                    diem_tb_1 += Convert.ToDecimal(tbMON_24_Ky1.Text) * 2;
                                    count_he_so_ky1 += 2;
                                }
                                else
                                {
                                    diem_tb_1 += hdHE_SO_24.Value != "" ? Convert.ToDecimal(tbMON_24_Ky1.Text) * Convert.ToDecimal(hdHE_SO_24.Value) : Convert.ToDecimal(tbMON_24_Ky1.Text);
                                    count_he_so_ky1 += hdHE_SO_24.Value != "" ? Convert.ToDecimal(hdHE_SO_24.Value) : 1;
                                }
                            }
                        }
                        #endregion
                        #region tính điểm kỳ 2
                        else if (rcbHocKy.SelectedValue == "2" || rcbHocKy.SelectedValue == "3")
                        {
                            if ((hdKIEU_MON_0.Value == "" || hdKIEU_MON_0.Value == "0") && tbMON_0_Ky2.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_0.Value == "1")
                                {
                                    diem_tb_2 += Convert.ToDecimal(tbMON_0_Ky2.Text) * 2;
                                    count_he_so_ky2 += 2;
                                }
                                else
                                {
                                    diem_tb_2 += hdHE_SO_0.Value != "" ? Convert.ToDecimal(tbMON_0_Ky2.Text) * Convert.ToDecimal(hdHE_SO_0.Value) : Convert.ToDecimal(tbMON_0_Ky2.Text);
                                    count_he_so_ky2 += hdHE_SO_0.Value != "" ? Convert.ToDecimal(hdHE_SO_0.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_1.Value == "" || hdKIEU_MON_1.Value == "0") && tbMON_1_Ky2.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_1.Value == "1")
                                {
                                    diem_tb_2 += Convert.ToDecimal(tbMON_1_Ky2.Text) * 2;
                                    count_he_so_ky2 += 2;
                                }
                                else
                                {
                                    diem_tb_2 += hdHE_SO_1.Value != "" ? Convert.ToDecimal(tbMON_1_Ky2.Text) * Convert.ToDecimal(hdHE_SO_1.Value) : Convert.ToDecimal(tbMON_1_Ky2.Text);
                                    count_he_so_ky2 += hdHE_SO_1.Value != "" ? Convert.ToDecimal(hdHE_SO_1.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_2.Value == "" || hdKIEU_MON_2.Value == "0") && tbMON_2_Ky2.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_2.Value == "1")
                                {
                                    diem_tb_2 += Convert.ToDecimal(tbMON_2_Ky2.Text) * 2;
                                    count_he_so_ky2 += 2;
                                }
                                else
                                {
                                    diem_tb_2 += hdHE_SO_2.Value != "" ? Convert.ToDecimal(tbMON_2_Ky2.Text) * Convert.ToDecimal(hdHE_SO_2.Value) : Convert.ToDecimal(tbMON_2_Ky2.Text);
                                    count_he_so_ky2 += hdHE_SO_2.Value != "" ? Convert.ToDecimal(hdHE_SO_2.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_3.Value == "" || hdKIEU_MON_3.Value == "0") && tbMON_3_Ky2.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_3.Value == "1")
                                {
                                    diem_tb_2 += Convert.ToDecimal(tbMON_3_Ky2.Text) * 2;
                                    count_he_so_ky2 += 2;
                                }
                                else
                                {
                                    diem_tb_2 += hdHE_SO_3.Value != "" ? Convert.ToDecimal(tbMON_3_Ky2.Text) * Convert.ToDecimal(hdHE_SO_3.Value) : Convert.ToDecimal(tbMON_3_Ky2.Text);
                                    count_he_so_ky2 += hdHE_SO_3.Value != "" ? Convert.ToDecimal(hdHE_SO_3.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_4.Value == "" || hdKIEU_MON_4.Value == "0") && tbMON_4_Ky2.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_4.Value == "1")
                                {
                                    diem_tb_2 += Convert.ToDecimal(tbMON_4_Ky2.Text) * 2;
                                    count_he_so_ky2 += 2;
                                }
                                else
                                {
                                    diem_tb_2 += hdHE_SO_4.Value != "" ? Convert.ToDecimal(tbMON_4_Ky2.Text) * Convert.ToDecimal(hdHE_SO_4.Value) : Convert.ToDecimal(tbMON_4_Ky2.Text);
                                    count_he_so_ky2 += hdHE_SO_4.Value != "" ? Convert.ToDecimal(hdHE_SO_4.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_5.Value == "" || hdKIEU_MON_5.Value == "0") && tbMON_5_Ky2.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_5.Value == "1")
                                {
                                    diem_tb_2 += Convert.ToDecimal(tbMON_5_Ky2.Text) * 2;
                                    count_he_so_ky2 += 2;
                                }
                                else
                                {
                                    diem_tb_2 += hdHE_SO_5.Value != "" ? Convert.ToDecimal(tbMON_5_Ky2.Text) * Convert.ToDecimal(hdHE_SO_5.Value) : Convert.ToDecimal(tbMON_5_Ky2.Text);
                                    count_he_so_ky2 += hdHE_SO_5.Value != "" ? Convert.ToDecimal(hdHE_SO_5.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_6.Value == "" || hdKIEU_MON_6.Value == "0") && tbMON_6_Ky2.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_6.Value == "1")
                                {
                                    diem_tb_2 += Convert.ToDecimal(tbMON_6_Ky2.Text) * 2;
                                    count_he_so_ky2 += 2;
                                }
                                else
                                {
                                    diem_tb_2 += hdHE_SO_6.Value != "" ? Convert.ToDecimal(tbMON_6_Ky2.Text) * Convert.ToDecimal(hdHE_SO_6.Value) : Convert.ToDecimal(tbMON_6_Ky2.Text);
                                    count_he_so_ky2 += hdHE_SO_6.Value != "" ? Convert.ToDecimal(hdHE_SO_6.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_7.Value == "" || hdKIEU_MON_7.Value == "0") && tbMON_7_Ky2.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_7.Value == "1")
                                {
                                    diem_tb_2 += Convert.ToDecimal(tbMON_7_Ky2.Text) * 2;
                                    count_he_so_ky2 += 2;
                                }
                                else
                                {
                                    diem_tb_2 += hdHE_SO_7.Value != "" ? Convert.ToDecimal(tbMON_7_Ky2.Text) * Convert.ToDecimal(hdHE_SO_7.Value) : Convert.ToDecimal(tbMON_7_Ky2.Text);
                                    count_he_so_ky2 += hdHE_SO_7.Value != "" ? Convert.ToDecimal(hdHE_SO_7.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_8.Value == "" || hdKIEU_MON_8.Value == "0") && tbMON_8_Ky2.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_8.Value == "1")
                                {
                                    diem_tb_2 += Convert.ToDecimal(tbMON_8_Ky2.Text) * 2;
                                    count_he_so_ky2 += 2;
                                }
                                else
                                {
                                    diem_tb_2 += hdHE_SO_8.Value != "" ? Convert.ToDecimal(tbMON_8_Ky2.Text) * Convert.ToDecimal(hdHE_SO_8.Value) : Convert.ToDecimal(tbMON_8_Ky2.Text);
                                    count_he_so_ky2 += hdHE_SO_8.Value != "" ? Convert.ToDecimal(hdHE_SO_8.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_9.Value == "" || hdKIEU_MON_9.Value == "0") && tbMON_9_Ky2.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_9.Value == "1")
                                {
                                    diem_tb_2 += Convert.ToDecimal(tbMON_9_Ky2.Text) * 2;
                                    count_he_so_ky2 += 2;
                                }
                                else
                                {
                                    diem_tb_2 += hdHE_SO_9.Value != "" ? Convert.ToDecimal(tbMON_9_Ky2.Text) * Convert.ToDecimal(hdHE_SO_9.Value) : Convert.ToDecimal(tbMON_9_Ky2.Text);
                                    count_he_so_ky2 += hdHE_SO_9.Value != "" ? Convert.ToDecimal(hdHE_SO_9.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_10.Value == "" || hdKIEU_MON_10.Value == "0") && tbMON_10_Ky2.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_10.Value == "1")
                                {
                                    diem_tb_2 += Convert.ToDecimal(tbMON_10_Ky2.Text) * 2;
                                    count_he_so_ky2 += 2;
                                }
                                else
                                {
                                    diem_tb_2 += hdHE_SO_10.Value != "" ? Convert.ToDecimal(tbMON_10_Ky2.Text) * Convert.ToDecimal(hdHE_SO_10.Value) : Convert.ToDecimal(tbMON_10_Ky2.Text);
                                    count_he_so_ky2 += hdHE_SO_10.Value != "" ? Convert.ToDecimal(hdHE_SO_10.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_11.Value == "" || hdKIEU_MON_11.Value == "0") && tbMON_11_Ky2.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_11.Value == "1")
                                {
                                    diem_tb_2 += Convert.ToDecimal(tbMON_11_Ky2.Text) * 2;
                                    count_he_so_ky2 += 2;
                                }
                                else
                                {
                                    diem_tb_2 += hdHE_SO_11.Value != "" ? Convert.ToDecimal(tbMON_11_Ky2.Text) * Convert.ToDecimal(hdHE_SO_11.Value) : Convert.ToDecimal(tbMON_11_Ky2.Text);
                                    count_he_so_ky2 += hdHE_SO_11.Value != "" ? Convert.ToDecimal(hdHE_SO_11.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_12.Value == "" || hdKIEU_MON_12.Value == "0") && tbMON_12_Ky2.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_12.Value == "1")
                                {
                                    diem_tb_2 += Convert.ToDecimal(tbMON_12_Ky2.Text) * 2;
                                    count_he_so_ky2 += 2;
                                }
                                else
                                {
                                    diem_tb_2 += hdHE_SO_12.Value != "" ? Convert.ToDecimal(tbMON_12_Ky2.Text) * Convert.ToDecimal(hdHE_SO_12.Value) : Convert.ToDecimal(tbMON_12_Ky2.Text);
                                    count_he_so_ky2 += hdHE_SO_12.Value != "" ? Convert.ToDecimal(hdHE_SO_12.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_13.Value == "" || hdKIEU_MON_13.Value == "0") && tbMON_13_Ky2.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_13.Value == "1")
                                {
                                    diem_tb_2 += Convert.ToDecimal(tbMON_13_Ky2.Text) * 2;
                                    count_he_so_ky2 += 2;
                                }
                                else
                                {
                                    diem_tb_2 += hdHE_SO_13.Value != "" ? Convert.ToDecimal(tbMON_13_Ky2.Text) * Convert.ToDecimal(hdHE_SO_13.Value) : Convert.ToDecimal(tbMON_13_Ky2.Text);
                                    count_he_so_ky2 += hdHE_SO_13.Value != "" ? Convert.ToDecimal(hdHE_SO_13.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_14.Value == "" || hdKIEU_MON_14.Value == "0") && tbMON_14_Ky2.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_14.Value == "1")
                                {
                                    diem_tb_2 += Convert.ToDecimal(tbMON_14_Ky2.Text) * 2;
                                    count_he_so_ky2 += 2;
                                }
                                else
                                {
                                    diem_tb_2 += hdHE_SO_14.Value != "" ? Convert.ToDecimal(tbMON_14_Ky2.Text) * Convert.ToDecimal(hdHE_SO_14.Value) : Convert.ToDecimal(tbMON_14_Ky2.Text);
                                    count_he_so_ky2 += hdHE_SO_14.Value != "" ? Convert.ToDecimal(hdHE_SO_14.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_15.Value == "" || hdKIEU_MON_15.Value == "0") && tbMON_15_Ky2.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_15.Value == "1")
                                {
                                    diem_tb_2 += Convert.ToDecimal(tbMON_15_Ky2.Text) * 2;
                                    count_he_so_ky2 += 2;
                                }
                                else
                                {
                                    diem_tb_2 += hdHE_SO_15.Value != "" ? Convert.ToDecimal(tbMON_15_Ky2.Text) * Convert.ToDecimal(hdHE_SO_15.Value) : Convert.ToDecimal(tbMON_15_Ky2.Text);
                                    count_he_so_ky2 += hdHE_SO_15.Value != "" ? Convert.ToDecimal(hdHE_SO_15.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_16.Value == "" || hdKIEU_MON_16.Value == "0") && tbMON_16_Ky2.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_16.Value == "1")
                                {
                                    diem_tb_2 += Convert.ToDecimal(tbMON_16_Ky2.Text) * 2;
                                    count_he_so_ky2 += 2;
                                }
                                else
                                {
                                    diem_tb_2 += hdHE_SO_16.Value != "" ? Convert.ToDecimal(tbMON_16_Ky2.Text) * Convert.ToDecimal(hdHE_SO_16.Value) : Convert.ToDecimal(tbMON_16_Ky2.Text);
                                    count_he_so_ky2 += hdHE_SO_16.Value != "" ? Convert.ToDecimal(hdHE_SO_16.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_17.Value == "" || hdKIEU_MON_17.Value == "0") && tbMON_17_Ky2.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_17.Value == "1")
                                {
                                    diem_tb_2 += Convert.ToDecimal(tbMON_17_Ky2.Text) * 2;
                                    count_he_so_ky2 += 2;
                                }
                                else
                                {
                                    diem_tb_2 += hdHE_SO_17.Value != "" ? Convert.ToDecimal(tbMON_17_Ky2.Text) * Convert.ToDecimal(hdHE_SO_17.Value) : Convert.ToDecimal(tbMON_17_Ky2.Text);
                                    count_he_so_ky2 += hdHE_SO_17.Value != "" ? Convert.ToDecimal(hdHE_SO_17.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_18.Value == "" || hdKIEU_MON_18.Value == "0") && tbMON_18_Ky2.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_18.Value == "1")
                                {
                                    diem_tb_2 += Convert.ToDecimal(tbMON_18_Ky2.Text) * 2;
                                    count_he_so_ky2 += 2;
                                }
                                else
                                {
                                    diem_tb_2 += hdHE_SO_18.Value != "" ? Convert.ToDecimal(tbMON_18_Ky2.Text) * Convert.ToDecimal(hdHE_SO_18.Value) : Convert.ToDecimal(tbMON_18_Ky2.Text);
                                    count_he_so_ky2 += hdHE_SO_18.Value != "" ? Convert.ToDecimal(hdHE_SO_18.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_19.Value == "" || hdKIEU_MON_19.Value == "0") && tbMON_19_Ky2.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_19.Value == "1")
                                {
                                    diem_tb_2 += Convert.ToDecimal(tbMON_19_Ky2.Text) * 2;
                                    count_he_so_ky2 += 2;
                                }
                                else
                                {
                                    diem_tb_2 += hdHE_SO_19.Value != "" ? Convert.ToDecimal(tbMON_19_Ky2.Text) * Convert.ToDecimal(hdHE_SO_19.Value) : Convert.ToDecimal(tbMON_19_Ky2.Text);
                                    count_he_so_ky2 += hdHE_SO_19.Value != "" ? Convert.ToDecimal(hdHE_SO_19.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_20.Value == "" || hdKIEU_MON_20.Value == "0") && tbMON_20_Ky2.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_20.Value == "1")
                                {
                                    diem_tb_2 += Convert.ToDecimal(tbMON_20_Ky2.Text) * 2;
                                    count_he_so_ky2 += 2;
                                }
                                else
                                {
                                    diem_tb_2 += hdHE_SO_20.Value != "" ? Convert.ToDecimal(tbMON_20_Ky2.Text) * Convert.ToDecimal(hdHE_SO_20.Value) : Convert.ToDecimal(tbMON_20_Ky2.Text);
                                    count_he_so_ky2 += hdHE_SO_20.Value != "" ? Convert.ToDecimal(hdHE_SO_20.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_21.Value == "" || hdKIEU_MON_21.Value == "0") && tbMON_21_Ky2.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_21.Value == "1")
                                {
                                    diem_tb_2 += Convert.ToDecimal(tbMON_21_Ky2.Text) * 2;
                                    count_he_so_ky2 += 2;
                                }
                                else
                                {
                                    diem_tb_2 += hdHE_SO_21.Value != "" ? Convert.ToDecimal(tbMON_21_Ky2.Text) * Convert.ToDecimal(hdHE_SO_21.Value) : Convert.ToDecimal(tbMON_21_Ky2.Text);
                                    count_he_so_ky2 += hdHE_SO_21.Value != "" ? Convert.ToDecimal(hdHE_SO_21.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_22.Value == "" || hdKIEU_MON_22.Value == "0") && tbMON_22_Ky2.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_22.Value == "1")
                                {
                                    diem_tb_2 += Convert.ToDecimal(tbMON_22_Ky2.Text) * 2;
                                    count_he_so_ky2 += 2;
                                }
                                else
                                {
                                    diem_tb_2 += hdHE_SO_22.Value != "" ? Convert.ToDecimal(tbMON_22_Ky2.Text) * Convert.ToDecimal(hdHE_SO_22.Value) : Convert.ToDecimal(tbMON_22_Ky2.Text);
                                    count_he_so_ky2 += hdHE_SO_22.Value != "" ? Convert.ToDecimal(hdHE_SO_22.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_23.Value == "" || hdKIEU_MON_23.Value == "0") && tbMON_23_Ky2.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_23.Value == "1")
                                {
                                    diem_tb_2 += Convert.ToDecimal(tbMON_23_Ky2.Text) * 2;
                                    count_he_so_ky2 += 2;
                                }
                                else
                                {
                                    diem_tb_2 += hdHE_SO_23.Value != "" ? Convert.ToDecimal(tbMON_23_Ky2.Text) * Convert.ToDecimal(hdHE_SO_23.Value) : Convert.ToDecimal(tbMON_23_Ky2.Text);
                                    count_he_so_ky2 += hdHE_SO_23.Value != "" ? Convert.ToDecimal(hdHE_SO_23.Value) : 1;
                                }
                            }
                            if ((hdKIEU_MON_24.Value == "" || hdKIEU_MON_24.Value == "0") && tbMON_24_Ky2.Text != "")
                            {
                                if (hdIS_MON_CHUYEN_24.Value == "1")
                                {
                                    diem_tb_2 += Convert.ToDecimal(tbMON_24_Ky2.Text) * 2;
                                    count_he_so_ky2 += 2;
                                }
                                else
                                {
                                    diem_tb_2 += hdHE_SO_24.Value != "" ? Convert.ToDecimal(tbMON_24_Ky2.Text) * Convert.ToDecimal(hdHE_SO_24.Value) : Convert.ToDecimal(tbMON_24_Ky2.Text);
                                    count_he_so_ky2 += hdHE_SO_24.Value != "" ? Convert.ToDecimal(hdHE_SO_24.Value) : 1;
                                }
                            }
                        }
                        #endregion
                        #endregion
                        #region "tính điểm tổng kết"
                        decimal tong_ket_ky1 = 0, tong_ket_ky2 = 0;
                        tong_ket_ky1 = count_he_so_ky1 > 0 ? (diem_tb_1 * Convert.ToDecimal(1.0) / count_he_so_ky1) : 0;
                        tong_ket_ky2 = count_he_so_ky2 > 0 ? (diem_tb_2 * Convert.ToDecimal(1.0) / count_he_so_ky2) : 0;
                        tong_ket_ky1 = Math.Round(tong_ket_ky1, 1, MidpointRounding.AwayFromZero);
                        tong_ket_ky2 = Math.Round(tong_ket_ky2, 1, MidpointRounding.AwayFromZero);
                        List<DiemChiTietEntity> lstDiemChiTietHS = new List<DiemChiTietEntity>();
                        lstDiemChiTietHS = diemChiTietBO.getDiemTBMonByHocSinh(Sys_This_Truong.ID, id_khoi.Value, id_lop.Value, Convert.ToInt16(Sys_Ma_Nam_hoc), Convert.ToInt16(rcbHocKy.SelectedValue), id_hoc_sinh, ma_mon_hoc_truong);
                        DIEM_TONG_KET detail = new DIEM_TONG_KET();
                        detail = diemTongKetBO.getDiemTrungBinhByHocSinh(Sys_This_Truong.ID, Convert.ToInt16(Sys_Ma_Nam_hoc), id_lop.Value, id_hoc_sinh);
                        if (rcbHocKy.SelectedValue == "1")
                        {
                            if (tong_ket_ky1 > 0)
                            {
                                #region xét điều kiện
                                int count_TbYKem = 0, count_Tb = 0, count_Y = 0, count_YKem = 0, count_Kem = 0, count_be2 = 0;
                                int countCD = 0;
                                decimal diem_toan_ky1 = 0, diem_nv_ky1 = 0;
                                int count_chuyen_duoi80 = 0, count_chuyen_duoi65 = 0, count_chuyen_duoi50 = 0;
                                List<decimal> lstDiemChuyen = new List<decimal>();
                                if (lstDiemChiTietHS.Count > 0)
                                {
                                    for (int i = 0; i < lstDiemChiTietHS.Count; i++)
                                    {
                                        DanhSachMonByLopEntity monDetail = lstMonLop.FirstOrDefault(x => x.ID_MON_TRUONG == lstDiemChiTietHS[i].ID_MON_HOC_TRUONG);
                                        if (monDetail != null && lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY1 != null)
                                        {
                                            if ((monDetail.KIEU_MON == null || monDetail.KIEU_MON == 0))
                                            {
                                                if (monDetail.MON_CHUYEN == 1)
                                                    lstDiemChuyen.Add(lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY1.Value);
                                                if (lstDiemChiTietHS[i].ID_MON_HOC != null && lstDiemChiTietHS[i].ID_MON_HOC == 113)
                                                    diem_toan_ky1 = lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY1.Value;
                                                else if (lstDiemChiTietHS[i].ID_MON_HOC != null && lstDiemChiTietHS[i].ID_MON_HOC == 118)
                                                    diem_nv_ky1 = lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY1.Value;

                                                if (lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY1 < Convert.ToDecimal(6.5))
                                                    count_TbYKem++;
                                                if (lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY1 >= Convert.ToDecimal(5.0) &&
                                                    lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY1 < Convert.ToDecimal(6.5))
                                                    count_Tb++;
                                                if (lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY1 >= Convert.ToDecimal(3.5) &&
                                                    lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY1 < Convert.ToDecimal(5.0))
                                                    count_Y++;
                                                if (lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY1 < Convert.ToDecimal(5.0))
                                                    count_YKem++;
                                                if (lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY1 < Convert.ToDecimal(3.5))
                                                    count_Kem++;
                                                if (lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY1 < Convert.ToDecimal(2.0))
                                                    count_be2++;
                                            }
                                            else
                                            {
                                                if (lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY1 != null && lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY1 == 0) countCD++;
                                            }
                                        }
                                    }
                                    for (int i = 0; i < lstDiemChuyen.Count; i++)
                                    {
                                        if (lstDiemChuyen[i] < Convert.ToDecimal(8.0))
                                        {
                                            count_chuyen_duoi80++;
                                            count_chuyen_duoi65++;
                                            count_chuyen_duoi50++;
                                        }
                                        else if (lstDiemChuyen[i] < Convert.ToDecimal(6.5))
                                        {
                                            count_chuyen_duoi65++;
                                            count_chuyen_duoi50++;
                                        }
                                        else if (lstDiemChuyen[i] < Convert.ToDecimal(5.0)) count_chuyen_duoi50++;
                                    }
                                }
                                #endregion
                                #region tính học lực học kỳ 1
                                if (detail != null)
                                {
                                    detail.TB_KY1 = tong_ket_ky1;
                                    if (tong_ket_ky1 >= Convert.ToDecimal(8.0) && countCD == 0 && count_TbYKem == 0 &&
                                        (diem_toan_ky1 >= Convert.ToDecimal(8.0) || diem_nv_ky1 >= Convert.ToDecimal(8.0)) && count_chuyen_duoi80 == 0)
                                    {
                                        detail.MA_HOC_LUC_KY1 = 1;
                                        if (detail.MA_HANH_KIEM_KY1 != null && detail.MA_HANH_KIEM_KY1 == 1)
                                            detail.MA_DANH_HIEU_KY1 = 1;
                                        else if (detail.MA_HANH_KIEM_KY1 != null && detail.MA_HANH_KIEM_KY1 == 2)
                                            detail.MA_DANH_HIEU_KY1 = 2;
                                        else detail.MA_DANH_HIEU_KY1 = null;
                                    }
                                    else if ((tong_ket_ky1 >= Convert.ToDecimal(6.5) && countCD == 0 &&
                                            count_YKem == 0 && count_chuyen_duoi65 == 0 &&
                                            (diem_toan_ky1 >= Convert.ToDecimal(6.5) || diem_nv_ky1 >= Convert.ToDecimal(6.5))) ||
                                        (tong_ket_ky1 >= Convert.ToDecimal(8.0) && countCD == 0 && count_YKem == 0 && count_Tb >= 1))
                                    {
                                        detail.MA_HOC_LUC_KY1 = 2;
                                        if (detail.MA_HANH_KIEM_KY1 != null && (detail.MA_HANH_KIEM_KY1 == 1 || detail.MA_HANH_KIEM_KY1 == 2))
                                            detail.MA_DANH_HIEU_KY1 = 2;
                                        else detail.MA_DANH_HIEU_KY1 = null;
                                    }
                                    else if ((tong_ket_ky1 >= Convert.ToDecimal(5.0) && countCD == 0 && count_chuyen_duoi50 == 0 &&
                                                count_Kem == 0 && (diem_toan_ky1 >= Convert.ToDecimal(5.0) || diem_nv_ky1 >= Convert.ToDecimal(5.0))) ||
                                        (tong_ket_ky1 >= Convert.ToDecimal(6.5) && count_Kem == 0 && (count_Y + countCD >= 1)))
                                    {
                                        detail.MA_HOC_LUC_KY1 = 3;
                                        detail.MA_DANH_HIEU_KY1 = null;
                                    }
                                    else if ((tong_ket_ky1 >= Convert.ToDecimal(3.5) && count_be2 == 0) ||
                                        tong_ket_ky1 >= Convert.ToDecimal(6.5) && count_be2 >= 1)
                                    {
                                        detail.MA_HOC_LUC_KY1 = 4;
                                        detail.MA_DANH_HIEU_KY1 = null;
                                    }
                                    else
                                    {
                                        detail.MA_HOC_LUC_KY1 = 5;
                                        detail.MA_DANH_HIEU_KY1 = null;
                                    }
                                    res = diemTongKetBO.update(detail, Sys_User.ID);
                                    if (res.Res)
                                    {
                                        success++;
                                        logUserBO.insert(Sys_This_Truong.ID, "UPATE", "Cập nhật điểm tổng kết HK1 HS " + detail.ID_HOC_SINH, Sys_User.ID, DateTime.Now);
                                    }
                                }
                                else
                                {
                                    detail = new DIEM_TONG_KET();
                                    detail.ID_HOC_SINH = id_hoc_sinh;
                                    detail.ID_LOP = id_lop.Value;
                                    detail.ID_TRUONG = Sys_This_Truong.ID;
                                    detail.ID_NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
                                    detail.TB_KY1 = tong_ket_ky1;
                                    if (tong_ket_ky1 >= Convert.ToDecimal(8.0) && countCD == 0 && count_TbYKem == 0 &&
                                        (diem_toan_ky1 >= Convert.ToDecimal(8.0) || diem_nv_ky1 >= Convert.ToDecimal(8.0)) && count_chuyen_duoi80 == 0)
                                    {
                                        detail.MA_HOC_LUC_KY1 = 1;
                                        if (detail.MA_HANH_KIEM_KY1 != null && detail.MA_HANH_KIEM_KY1 == 1)
                                            detail.MA_DANH_HIEU_KY1 = 1;
                                        else if (detail.MA_HANH_KIEM_KY1 != null && detail.MA_HANH_KIEM_KY1 == 2)
                                            detail.MA_DANH_HIEU_KY1 = 2;
                                        else detail.MA_DANH_HIEU_KY1 = null;
                                    }
                                    else if ((tong_ket_ky1 >= Convert.ToDecimal(6.5) && countCD == 0 &&
                                            count_YKem == 0 && count_chuyen_duoi65 == 0 &&
                                            (diem_toan_ky1 >= Convert.ToDecimal(6.5) || diem_nv_ky1 >= Convert.ToDecimal(6.5))) ||
                                        (tong_ket_ky1 >= Convert.ToDecimal(8.0) && countCD == 0 && count_YKem == 0 && count_Tb >= 1))
                                    {
                                        detail.MA_HOC_LUC_KY1 = 2;
                                        if (detail.MA_HANH_KIEM_KY1 != null && (detail.MA_HANH_KIEM_KY1 == 1 || detail.MA_HANH_KIEM_KY1 == 2))
                                            detail.MA_DANH_HIEU_KY1 = 2;
                                    }
                                    else if ((tong_ket_ky1 >= Convert.ToDecimal(6.5) && countCD == 0 && count_chuyen_duoi50 == 0 &&
                                                count_Kem == 0 && (diem_toan_ky1 >= Convert.ToDecimal(5.0) || diem_nv_ky1 >= Convert.ToDecimal(5.0))) ||
                                        (tong_ket_ky1 >= Convert.ToDecimal(6.5) && count_Kem == 0 && (count_Y + countCD >= 1)))
                                        detail.MA_HOC_LUC_KY1 = 3;
                                    else if ((tong_ket_ky1 >= Convert.ToDecimal(3.5) && count_be2 == 0) ||
                                        tong_ket_ky1 >= Convert.ToDecimal(6.5) && count_be2 >= 1)
                                        detail.MA_HOC_LUC_KY1 = 4;
                                    else detail.MA_HOC_LUC_KY1 = 5;
                                    res = diemTongKetBO.insert(detail, Sys_User.ID);
                                    if (res.Res)
                                    {
                                        success++;
                                        logUserBO.insert(Sys_This_Truong.ID, "INSERT", "Thêm mới điểm tổng kết HK1 HS " + detail.ID_HOC_SINH, Sys_User.ID, DateTime.Now);
                                    }
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            #region tính TBM học kỳ 2 và cả năm
                            if (tong_ket_ky2 > 0)
                            {
                                if (detail != null)
                                {
                                    detail.TB_KY2 = tong_ket_ky2;
                                    #region xét điều kiện xét học lực kỳ 2
                                    int count_TbYKem_ky2 = 0, count_Tb_ky2 = 0, count_Y_ky2 = 0, count_YKem_ky2 = 0, count_Kem_ky2 = 0, count_be2_ky2 = 0;
                                    int countCD_ky2 = 0;
                                    decimal diem_toan_ky2 = 0, diem_nv_ky2 = 0;
                                    int count_chuyen_duoi80_ky2 = 0, count_chuyen_duoi65_ky2 = 0, count_chuyen_duoi50_ky2 = 0;
                                    List<decimal> lstDiemChuyen_ky2 = new List<decimal>();
                                    if (lstDiemChiTietHS.Count > 0)
                                    {
                                        for (int i = 0; i < lstDiemChiTietHS.Count; i++)
                                        {
                                            DanhSachMonByLopEntity monDetail = lstMonLop.FirstOrDefault(x => x.ID_MON_TRUONG == lstDiemChiTietHS[i].ID_MON_HOC_TRUONG);
                                            if (monDetail != null && lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY2 != null)
                                            {
                                                if ((monDetail.KIEU_MON == null || monDetail.KIEU_MON == 0))
                                                {
                                                    if (monDetail.MON_CHUYEN == 1)
                                                        lstDiemChuyen_ky2.Add(lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY2.Value);
                                                    if (lstDiemChiTietHS[i].ID_MON_HOC != null && lstDiemChiTietHS[i].ID_MON_HOC == 113)
                                                        diem_toan_ky2 = lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY2.Value;
                                                    else if (lstDiemChiTietHS[i].ID_MON_HOC != null && lstDiemChiTietHS[i].ID_MON_HOC == 118)
                                                        diem_nv_ky2 = lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY2.Value;

                                                    if (lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY2 < Convert.ToDecimal(6.5))
                                                        count_TbYKem_ky2++;
                                                    if (lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY2 >= Convert.ToDecimal(5.0) &&
                                                        lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY2 < Convert.ToDecimal(6.5))
                                                        count_Tb_ky2++;
                                                    if (lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY2 >= Convert.ToDecimal(3.5) &&
                                                        lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY2 < Convert.ToDecimal(5.0))
                                                        count_Y_ky2++;
                                                    if (lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY2 < Convert.ToDecimal(5.0))
                                                        count_YKem_ky2++;
                                                    if (lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY2 < Convert.ToDecimal(3.5))
                                                        count_Kem_ky2++;
                                                    if (lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY2 < Convert.ToDecimal(2.0))
                                                        count_be2_ky2++;
                                                }
                                                else
                                                {
                                                    if (lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY2 != null && lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY2 == 0) countCD_ky2++;
                                                }
                                            }
                                        }
                                        for (int i = 0; i < lstDiemChuyen_ky2.Count; i++)
                                        {
                                            if (lstDiemChuyen_ky2[i] < Convert.ToDecimal(8.0))
                                            {
                                                count_chuyen_duoi80_ky2++;
                                                count_chuyen_duoi65_ky2++;
                                                count_chuyen_duoi50_ky2++;
                                            }
                                            else if (lstDiemChuyen_ky2[i] < Convert.ToDecimal(6.5))
                                            {
                                                count_chuyen_duoi65_ky2++;
                                                count_chuyen_duoi50_ky2++;
                                            }
                                            else count_chuyen_duoi50_ky2++;
                                        }
                                    }
                                    #endregion
                                    #region tính học lực học kỳ 2
                                    if (tong_ket_ky2 >= Convert.ToDecimal(8.0) && countCD_ky2 == 0 && count_TbYKem_ky2 == 0 &&
                                            (diem_toan_ky2 >= Convert.ToDecimal(8.0) || diem_nv_ky2 >= Convert.ToDecimal(8.0)) && count_chuyen_duoi80_ky2 == 0)
                                    {
                                        detail.MA_HOC_LUC_KY2 = 1;
                                        if (detail.MA_HANH_KIEM_KY2 != null && detail.MA_HANH_KIEM_KY2 == 1)
                                            detail.MA_DANH_HIEU_KY2 = 1;
                                        else if (detail.MA_HANH_KIEM_KY2 != null && detail.MA_HANH_KIEM_KY2 == 2)
                                            detail.MA_DANH_HIEU_KY2 = 2;
                                        else detail.MA_DANH_HIEU_KY2 = null;
                                    }
                                    else if ((tong_ket_ky2 >= Convert.ToDecimal(6.5) && countCD_ky2 == 0 &&
                                            count_YKem_ky2 == 0 && count_chuyen_duoi65_ky2 == 0 &&
                                            (diem_toan_ky2 >= Convert.ToDecimal(6.5) || diem_nv_ky2 >= Convert.ToDecimal(6.5))) ||
                                        (tong_ket_ky2 >= Convert.ToDecimal(8.0) && countCD_ky2 == 0 && count_YKem_ky2 == 0 && count_Tb_ky2 >= 1))
                                    {
                                        detail.MA_HOC_LUC_KY2 = 2;
                                        if (detail.MA_HANH_KIEM_KY2 != null && (detail.MA_HANH_KIEM_KY2 == 1 || detail.MA_HANH_KIEM_KY2 == 2))
                                            detail.MA_DANH_HIEU_KY2 = 2;
                                        else detail.MA_DANH_HIEU_KY2 = null;
                                    }
                                    else if ((tong_ket_ky2 >= Convert.ToDecimal(6.5) && countCD_ky2 == 0 && count_chuyen_duoi50_ky2 == 0 &&
                                                count_Kem_ky2 == 0 && (diem_toan_ky2 >= Convert.ToDecimal(5.0) || diem_nv_ky2 >= Convert.ToDecimal(5.0))) ||
                                        (tong_ket_ky2 >= Convert.ToDecimal(6.5) && count_Kem_ky2 == 0 && (count_Y_ky2 + countCD_ky2 >= 1)))
                                    {
                                        detail.MA_HOC_LUC_KY2 = 3;
                                        detail.MA_DANH_HIEU_KY2 = null;
                                    }
                                    else if ((tong_ket_ky2 >= Convert.ToDecimal(3.5) && count_be2_ky2 == 0) ||
                                        tong_ket_ky2 >= Convert.ToDecimal(6.5) && count_be2_ky2 >= 1)
                                    {
                                        detail.MA_HOC_LUC_KY2 = 4;
                                        detail.MA_DANH_HIEU_KY2 = null;
                                    }
                                    else
                                    {
                                        detail.MA_HOC_LUC_KY2 = 5;
                                        detail.MA_DANH_HIEU_KY2 = null;
                                    }
                                    #endregion
                                    #region tổng kết cả năm
                                    decimal? diemTBM_ky1 = detail.TB_KY1 != null ? detail.TB_KY1 : null;
                                    decimal diemTBM_cn = diemTBM_ky1 != null ? (tong_ket_ky2 * 2 + diemTBM_ky1.Value) * Convert.ToDecimal(1.0) / 3 : tong_ket_ky2;
                                    diemTBM_cn = Math.Round(diemTBM_cn, 1, MidpointRounding.AwayFromZero);
                                    detail.TB_CN = diemTBM_cn;
                                    if (diemTBM_cn >= Convert.ToDecimal(8.0) && detail.MA_HANH_KIEM_CA_NAM == 1)
                                        detail.MA_DANH_HIEU_CN = 1;
                                    else if (diemTBM_cn >= Convert.ToDecimal(6.5) && (detail.MA_HANH_KIEM_CA_NAM == 1 || detail.MA_HANH_KIEM_CA_NAM == 2))
                                        detail.MA_DANH_HIEU_CN = 2;
                                    else detail.MA_DANH_HIEU_CN = null;
                                    #endregion
                                    res = diemTongKetBO.update(detail, Sys_User.ID);
                                    if (res.Res)
                                    {
                                        success++;
                                        logUserBO.insert(Sys_This_Truong.ID, "UPATE", "Cập nhật điểm tổng kết HK2 HS " + detail.ID_HOC_SINH, Sys_User.ID, DateTime.Now);
                                    }
                                }
                                else
                                {
                                    detail = new DIEM_TONG_KET();
                                    detail.ID_HOC_SINH = id_hoc_sinh;
                                    detail.ID_LOP = id_lop.Value;
                                    detail.ID_TRUONG = Sys_This_Truong.ID;
                                    detail.ID_NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
                                    detail.TB_KY2 = tong_ket_ky2;
                                    detail.TB_CN = tong_ket_ky2;
                                    #region xét điều kiện xét học lực kỳ 2
                                    int count_TbYKem_ky2 = 0, count_Tb_ky2 = 0, count_Y_ky2 = 0, count_YKem_ky2 = 0, count_Kem_ky2 = 0, count_be2_ky2 = 0;
                                    int countCD_ky2 = 0;
                                    decimal diem_toan_ky2 = 0, diem_nv_ky2 = 0;
                                    int count_chuyen_duoi80_ky2 = 0, count_chuyen_duoi65_ky2 = 0, count_chuyen_duoi50_ky2 = 0;
                                    List<decimal> lstDiemChuyen_ky2 = new List<decimal>();
                                    if (lstDiemChiTietHS.Count > 0)
                                    {
                                        for (int i = 0; i < lstDiemChiTietHS.Count; i++)
                                        {
                                            DanhSachMonByLopEntity monDetail = lstMonLop.FirstOrDefault(x => x.ID_MON_TRUONG == lstDiemChiTietHS[i].ID_MON_HOC_TRUONG);
                                            if (monDetail != null && lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY2 != null)
                                            {
                                                if ((monDetail.KIEU_MON == null || monDetail.KIEU_MON == 0))
                                                {
                                                    if (monDetail.MON_CHUYEN == 1)
                                                        lstDiemChuyen_ky2.Add(lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY2.Value);
                                                    if (lstDiemChiTietHS[i].ID_MON_HOC != null && lstDiemChiTietHS[i].ID_MON_HOC == 113)
                                                        diem_toan_ky2 = lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY2.Value;
                                                    else if (lstDiemChiTietHS[i].ID_MON_HOC != null && lstDiemChiTietHS[i].ID_MON_HOC == 118)
                                                        diem_nv_ky2 = lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY2.Value;

                                                    if (lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY2 < Convert.ToDecimal(6.5))
                                                        count_TbYKem_ky2++;
                                                    if (lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY2 >= Convert.ToDecimal(5.0) &&
                                                        lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY2 < Convert.ToDecimal(6.5))
                                                        count_Tb_ky2++;
                                                    if (lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY2 >= Convert.ToDecimal(3.5) &&
                                                        lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY2 < Convert.ToDecimal(5.0))
                                                        count_Y_ky2++;
                                                    if (lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY2 < Convert.ToDecimal(5.0))
                                                        count_YKem_ky2++;
                                                    if (lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY2 < Convert.ToDecimal(3.5))
                                                        count_Kem_ky2++;
                                                    if (lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY2 < Convert.ToDecimal(2.0))
                                                        count_be2_ky2++;
                                                }
                                                else
                                                {
                                                    if (lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY2 != null && lstDiemChiTietHS[i].DIEM_TRUNG_BINH_KY2 == 0) countCD_ky2++;
                                                }
                                            }
                                        }
                                        for (int i = 0; i < lstDiemChuyen_ky2.Count; i++)
                                        {
                                            if (lstDiemChuyen_ky2[i] < Convert.ToDecimal(8.0))
                                            {
                                                count_chuyen_duoi80_ky2++;
                                                count_chuyen_duoi65_ky2++;
                                                count_chuyen_duoi50_ky2++;
                                            }
                                            else if (lstDiemChuyen_ky2[i] < Convert.ToDecimal(6.5))
                                            {
                                                count_chuyen_duoi65_ky2++;
                                                count_chuyen_duoi50_ky2++;
                                            }
                                            else count_chuyen_duoi50_ky2++;
                                        }
                                    }
                                    #endregion
                                    #region tính học lực học kỳ 2
                                    if (tong_ket_ky2 >= Convert.ToDecimal(8.0) && countCD_ky2 == 0 && count_TbYKem_ky2 == 0 &&
                                            (diem_toan_ky2 >= Convert.ToDecimal(8.0) || diem_nv_ky2 >= Convert.ToDecimal(8.0)) && count_chuyen_duoi80_ky2 == 0)
                                        detail.MA_HOC_LUC_KY2 = 1;
                                    else if ((tong_ket_ky2 >= Convert.ToDecimal(6.5) && countCD_ky2 == 0 &&
                                            count_YKem_ky2 == 0 && count_chuyen_duoi65_ky2 == 0 &&
                                            (diem_toan_ky2 >= Convert.ToDecimal(6.5) || diem_nv_ky2 >= Convert.ToDecimal(6.5))) ||
                                        (tong_ket_ky2 >= Convert.ToDecimal(8.0) && countCD_ky2 == 0 && count_YKem_ky2 == 0 && count_Tb_ky2 >= 1))
                                        detail.MA_HOC_LUC_KY2 = 2;
                                    else if ((tong_ket_ky2 >= Convert.ToDecimal(6.5) && countCD_ky2 == 0 && count_chuyen_duoi50_ky2 == 0 &&
                                                count_Kem_ky2 == 0 && (diem_toan_ky2 >= Convert.ToDecimal(5.0) || diem_nv_ky2 >= Convert.ToDecimal(5.0))) ||
                                        (tong_ket_ky2 >= Convert.ToDecimal(6.5) && count_Kem_ky2 == 0 && (count_Y_ky2 + countCD_ky2 >= 1)))
                                        detail.MA_HOC_LUC_KY2 = 3;
                                    else if ((tong_ket_ky2 >= Convert.ToDecimal(3.5) && count_be2_ky2 == 0) ||
                                        tong_ket_ky2 >= Convert.ToDecimal(6.5) && count_be2_ky2 >= 1)
                                        detail.MA_HOC_LUC_KY2 = 4;
                                    else detail.MA_HOC_LUC_KY2 = 5;
                                    #endregion
                                    res = diemTongKetBO.insert(detail, Sys_User.ID);
                                    if (res.Res)
                                    {
                                        success++;
                                        logUserBO.insert(Sys_This_Truong.ID, "INSERT", "Thêm mới điểm tổng kết HK2 HS " + detail.ID_HOC_SINH, Sys_User.ID, DateTime.Now);
                                    }
                                }
                            }
                            #endregion
                        }
                        #endregion
                    }
                }
            }
            string strMsg = "";
            if (success > 0)
                strMsg = "notification('success', 'Cập nhật thành công!');";
            else strMsg = "notification('warning', 'Không có bản ghi nào được lưu!');";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
        }
    }
}