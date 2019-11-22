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

namespace CMS.DinhDuong
{
    public partial class DangKyAn : AuthenticatePage
    {
        DangKyAnBO dangKyAnBO = new DangKyAnBO();
        LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            btnSave.Visible = is_access(SYS_Type_Access.THEM) && is_access(SYS_Type_Access.SUA) && is_access(SYS_Type_Access.XOA);
            if (!IsPostBack)
            {
                objKhoi.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                objKhoi.SelectParameters.Add("maLoaiLopGDTX", DbType.Int16, Sys_This_Lop_GDTX == null ? "" : Sys_This_Lop_GDTX.ToString());
                rcbKhoi.DataBind();
                objLop.SelectParameters.Add("ma_cap_hoc", Sys_This_Cap_Hoc);
                objLop.SelectParameters.Add("idTruong", Sys_This_Truong.ID.ToString());
                objLop.SelectParameters.Add("maNamHoc", Sys_Ma_Nam_hoc.ToString());
                rcbLop.DataBind();
                rcbHocKy.SelectedValue = Sys_Hoc_Ky.ToString();
                objBuaAn.SelectParameters.Add("idTruong", Sys_This_Truong.ID.ToString());
                soHocSinhDangKy();
            }
        }

        protected void btTimKiem_Click(object sender, EventArgs e)
        {
            RadGrid1.Rebind();
        }

        protected void rcbBuaAn_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            short? id_khoi = localAPI.ConvertStringToShort(rcbKhoi.SelectedValue);
            long? id_lop = localAPI.ConvertStringTolong(rcbLop.SelectedValue);
            if (id_khoi !=null && id_lop != null)
            {
                RadGrid1.DataSource = dangKyAnBO.getHocSinhDangKyAnByTruongKhoiAndLop(Sys_This_Truong.ID, id_khoi.Value, id_lop.Value,
                    Convert.ToInt16(Sys_Ma_Nam_hoc), Sys_This_Cap_Hoc,
                    localAPI.ConvertStringToShort(rcbHocKy.SelectedValue).Value,
                    localAPI.ConvertStringTolong(rcbBuaAn.SelectedValue),
                    tbTen.Text.Trim(), localAPI.ConvertStringToShort(rcbTrangThaiDangKy.SelectedValue));
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }

        protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Header)
            {
                Literal ltrHocKy = (Literal)e.Item.FindControl("ltrHocKy");
                if (Sys_Hoc_Ky == 1) ltrHocKy.Text = "ĐK kỳ 1";
                else if (Sys_Hoc_Ky == 2) ltrHocKy.Text = "ĐK kỳ 2";
            }
            else if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                CheckBox chbIS_DK = (CheckBox)e.Item.FindControl("chbIS_DK");
                HiddenField hdIS_DK = (HiddenField)e.Item.FindControl("hdIS_DK");
                int? IS_DK = localAPI.ConvertStringToint(hdIS_DK.Value);
                chbIS_DK.Checked = IS_DK != null && IS_DK == 1 ? true : false;
            }
        }

        protected void rcbKhoi_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbLop.ClearSelection();
            rcbLop.Text = String.Empty;
            rcbLop.DataBind();
            rcbBuaAn.ClearSelection();
            rcbBuaAn.Text = String.Empty;
            rcbBuaAn.DataBind();
            RadGrid1.Rebind();
            soHocSinhDangKy();
        }

        protected void rcbLop_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
            soHocSinhDangKy();
        }

        protected void rcbHocKy_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }

        protected void rcbTrangThaiDangKy_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            int success = 0, error = 0;
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            foreach (GridDataItem item in RadGrid1.Items)
            {
                long id_lop = Convert.ToInt64(item["ID_LOP"].Text);
                long id_hoc_sinh = Convert.ToInt64(item["ID_HOC_SINH"].Text);
                long? id_bua_an = localAPI.ConvertStringTolong(item["ID_BUA_AN"].Text);
                CheckBox chbIS_DK = (CheckBox)item.FindControl("chbIS_DK");
                HiddenField hdIS_DK = (HiddenField)item.FindControl("hdIS_DK");
                bool id_dk = hdIS_DK.Value == null ? false : (hdIS_DK.Value == "1" ? true : hdIS_DK.Value == "0" ? false : false);
                if ((hdIS_DK.Value == null && chbIS_DK.Checked) || (hdIS_DK.Value != null && chbIS_DK.Checked != id_dk))
                {
                    DANG_KY_AN detaiHS = new DANG_KY_AN();
                    long? id = localAPI.ConvertStringTolong(item["ID"].Text);
                    if (id != null)
                    {
                        detaiHS.ID = id.Value;
                        detaiHS.ID_TRUONG = Sys_This_Truong.ID;
                        detaiHS.ID_NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
                        detaiHS.HOC_KY = Convert.ToInt16(Sys_Hoc_Ky);
                        detaiHS.ID_LOP = id_lop;
                        detaiHS.ID_HOC_SINH = id_hoc_sinh;
                        detaiHS.ID_BUA_AN = id_bua_an;
                        if (chbIS_DK.Checked) detaiHS.IS_DELETE = false;
                        else detaiHS.IS_DELETE = true;
                        res = dangKyAnBO.insertOrUpdate(detaiHS, false, Sys_User.ID);
                    }
                    else
                    {
                        detaiHS = new DANG_KY_AN();
                        detaiHS.ID_TRUONG = Sys_This_Truong.ID;
                        detaiHS.ID_NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
                        detaiHS.HOC_KY = Convert.ToInt16(Sys_Hoc_Ky);
                        detaiHS.ID_LOP = id_lop;
                        detaiHS.ID_HOC_SINH = id_hoc_sinh;
                        detaiHS.ID_BUA_AN = id_bua_an;
                        res = dangKyAnBO.insertOrUpdate(detaiHS, true, Sys_User.ID);
                    }
                    if (res.Res)
                        success++;
                    else
                        error++;
                }
            }
            string strMsg = "";
            if (error > 0)
            {
                strMsg = "notification('error', 'Có " + error + " chưa được lưu. Liên hệ với quản trị viên.');";
            }
            if (success > 0)
            {
                strMsg += " notification('success', 'Có " + success + " bản ghi lưu thành công!');";
            }
            else if (error == 0 && success == 0)
                strMsg += "notification('warning', 'Không có bản ghi nào được lưu!');";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
            soHocSinhDangKy();
        }

        protected void RadGrid1_PreRender(object sender, EventArgs e)
        {
            MergeRows(RadGrid1);
        }
        public static void MergeRows(RadGrid RadGrid1)
        {
            for (int i = RadGrid1.Items.Count - 1; i > 0; i--)
            {
                if (RadGrid1.Items[i][RadGrid1.Columns[2]].Text == RadGrid1.Items[i - 1][RadGrid1.Columns[2]].Text)
                {
                    RadGrid1.Items[i - 1][RadGrid1.Columns[5]].RowSpan = RadGrid1.Items[i][RadGrid1.Columns[5]].RowSpan < 2 ? 2 : RadGrid1.Items[i][RadGrid1.Columns[5]].RowSpan + 1;
                    RadGrid1.Items[i][RadGrid1.Columns[5]].Visible = false;

                    RadGrid1.Items[i - 1][RadGrid1.Columns[6]].RowSpan = RadGrid1.Items[i][RadGrid1.Columns[6]].RowSpan < 2 ? 2 : RadGrid1.Items[i][RadGrid1.Columns[6]].RowSpan + 1;
                    RadGrid1.Items[i][RadGrid1.Columns[6]].Visible = false;

                    RadGrid1.Items[i - 1][RadGrid1.Columns[7]].RowSpan = RadGrid1.Items[i][RadGrid1.Columns[7]].RowSpan < 2 ? 2 : RadGrid1.Items[i][RadGrid1.Columns[7]].RowSpan + 1;
                    RadGrid1.Items[i][RadGrid1.Columns[7]].Visible = false;

                    RadGrid1.Items[i - 1][RadGrid1.Columns[8]].RowSpan = RadGrid1.Items[i][RadGrid1.Columns[8]].RowSpan < 2 ? 2 : RadGrid1.Items[i][RadGrid1.Columns[8]].RowSpan + 1;
                    RadGrid1.Items[i][RadGrid1.Columns[8]].Visible = false;
                }
            }
        }
        public void soHocSinhDangKy()
        {
            if (!string.IsNullOrEmpty(rcbLop.SelectedValue))
            {
                long? soHS_dangKy = dangKyAnBO.getSoHocSinhDangKy(Sys_This_Truong.ID, Sys_This_Cap_Hoc, Convert.ToInt16(rcbKhoi.SelectedValue), Convert.ToInt16(Sys_Ma_Nam_hoc), localAPI.ConvertStringTolong(rcbLop.SelectedValue), Convert.ToInt16(Sys_Hoc_Ky));
                if (soHS_dangKy != null) lblSoHSDangKy.Text = "Tổng số học sinh đăng ký ăn của lớp là " + soHS_dangKy;
                else lblSoHSDangKy.Text = "";
            }
        }
    }
}