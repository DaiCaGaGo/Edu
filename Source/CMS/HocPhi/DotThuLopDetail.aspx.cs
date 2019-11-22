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

namespace CMS.HocPhi
{
    public partial class DotThuLopDetail : AuthenticatePage
    {
        long? id_dot_thu_lop;
        HocPhiDotThuBO dotThuBO = new HocPhiDotThuBO();
        HocPhiDotThuLopBO dotThuLopBO = new HocPhiDotThuLopBO();
        LopBO lopBO = new LopBO();
        LocalAPI localAPI = new LocalAPI();
        LogUserBO logUserBO = new LogUserBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("id") != null)
            {
                try
                {
                    id_dot_thu_lop = Convert.ToInt16(Request.QueryString.Get("id"));
                }
                catch { }
            }
            if (!IsPostBack)
            {
                objDotThu.SelectParameters.Add("id_truong", Sys_This_Truong.ID.ToString());
                objDotThu.SelectParameters.Add("id_nam_hoc", Sys_Ma_Nam_hoc.ToString());
                rcbDotThu.DataBind();
                objKhoiHoc.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                rcbKhoi.DataBind();
                objLop.SelectParameters.Add("id_truong", Sys_This_Truong.ID.ToString());
                objLop.SelectParameters.Add("ma_cap_hoc", Sys_This_Cap_Hoc);
                objLop.SelectParameters.Add("id_nam_hoc", Sys_Ma_Nam_hoc.ToString());
                rcbLop.DataBind();
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            List<short> lst_ma_khoi = new List<short>();
            foreach (var item in rcbKhoi.CheckedItems)
            {
                lst_ma_khoi.Add(localAPI.ConvertStringToShort(item.Value).Value);
            }
            RadGrid1.DataSource = dotThuLopBO.getDotThuLopByDotThuAndKhoi(Sys_This_Truong.ID, Convert.ToInt16(Sys_Ma_Nam_hoc), localAPI.ConvertStringToShort(rcbDotThu.SelectedValue), lst_ma_khoi);
        }
        protected void rcbKhoi_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            string lst_ma_khoi = "";
            foreach (var item in rcbKhoi.CheckedItems)
            {
                if (string.IsNullOrEmpty(lst_ma_khoi))
                    lst_ma_khoi += item.Value;
                else lst_ma_khoi += "," + item.Value;
            }
            hdKhoi.Value = lst_ma_khoi;
            rcbLop.ClearSelection();
            rcbLop.Text = String.Empty;
            rcbLop.DataBind();
            RadGrid1.Rebind();
        }
        protected void rcbDotThu_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            string lst_ma_khoi = "";
            foreach (var item in rcbKhoi.CheckedItems)
            {
                if (string.IsNullOrEmpty(lst_ma_khoi))
                    lst_ma_khoi += item.Value;
                else lst_ma_khoi += "," + item.Value;
            }
            hdKhoi.Value = lst_ma_khoi;
            rcbLop.ClearSelection();
            rcbLop.Text = String.Empty;
            rcbLop.DataBind();
            RadGrid1.Rebind();
        }
        protected void btSave_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            if (string.IsNullOrEmpty(rcbDotThu.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Bạn chưa chọn đợt thu nào!');", true);
                return;
            }
            string strMsg = "";
            int countCheck = 0;
            HOC_PHI_DOT_THU dotThu = dotThuBO.getDotThuByID(Convert.ToInt64(rcbDotThu.SelectedValue));
            if (dotThu != null)
            {
                foreach (RadComboBoxItem item in rcbLop.Items)
                {
                    if (item.Checked)
                    {
                        countCheck++;
                        HOC_PHI_DOT_THU_LOP detail = new HOC_PHI_DOT_THU_LOP();
                        detail.ID_DOT_THU = Convert.ToInt64(rcbDotThu.SelectedValue);
                        detail.ID_LOP = Convert.ToInt64(item.Value);
                        detail.ID_KHOI = lopBO.getLopByTruongCapNamHoc(Sys_This_Cap_Hoc, Sys_This_Truong.ID, Convert.ToInt16(Sys_Ma_Nam_hoc)).ToList().Where(x => x.ID == Convert.ToInt64(item.Value)).FirstOrDefault().ID_KHOI;
                        detail.ID_TRUONG = Sys_This_Truong.ID;
                        detail.ID_NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
                        detail.TONG_TIEN = dotThu.TONG_TIEN;
                        detail.IS_TIEN_AN = dotThu.IS_TIEN_AN;
                        detail.SO_TIEN_AN = dotThu.SO_TIEN_AN;
                        detail.GHI_CHU = dotThu.GHI_CHU;
                        ResultEntity res = dotThuLopBO.insert(detail, Sys_User.ID);

                        if (res.Res)
                        {
                            strMsg = "notification('success', '" + res.Msg + "');";
                            logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật đợt thu lớp " + item.Value + " vào đợt " + rcbDotThu.SelectedValue, Sys_User.ID, DateTime.Now);
                        }
                        else
                        {
                            strMsg = "notification('error', '" + res.Msg + "');";
                            break;
                        }
                    }
                }
            }
            if (countCheck == 0)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Chưa có lớp nào được chọn!');", true);
                return;
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            rcbLop.DataBind();
            RadGrid1.Rebind();
        }
        protected void btDeleteChon_Click(object sender, EventArgs e)
        {
            int success = 0, error = 0;
            foreach (GridDataItem row in RadGrid1.SelectedItems)
            {
                try
                {
                    string lst_id = string.Empty;
                    short id_dot_thu_lop = Convert.ToInt16(row.GetDataKeyValue("ID").ToString());
                    string id_lop = row["ID_LOP"].Text;
                    try
                    {
                        ResultEntity res = dotThuLopBO.delete(id_dot_thu_lop, Sys_User.ID);
                        lst_id = id_lop;
                        if (res.Res)
                        {
                            success++;
                            logUserBO.insert(Sys_This_Truong.ID, "DELETE", "Xóa lớp " + id_lop + " khỏi đợt thu " + id_dot_thu_lop, Sys_User.ID, DateTime.Now);
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
            rcbLop.DataBind();
            RadGrid1.Rebind();
        }
        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                long? id_lop = localAPI.ConvertStringTolong(item["ID_LOP"].Text);
                if (id_lop != null)
                    item["TEN_LOP"].Text = lopBO.getLopById(id_lop.Value).TEN;
            }
        }
    }
}