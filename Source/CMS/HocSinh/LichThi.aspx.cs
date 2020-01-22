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
    public partial class LichThi : AuthenticatePage
    {
        private LichThiBO lichThiBO = new LichThiBO();
        private HocSinhBO hsBO = new HocSinhBO();
        LocalAPI local_api = new LocalAPI();
        LogUserBO logUserBO = new LogUserBO();

        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            btSave.Visible = is_access(SYS_Type_Access.SUA);
            if (!IsPostBack)
            {
                objKhoiHoc.SelectParameters.Add("maLoaiLopGDTX", Sys_This_Lop_GDTX == null ? "" : Sys_This_Lop_GDTX.ToString());
                objKhoiHoc.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                rcbKhoi.DataBind();
                objLop.SelectParameters.Add("ma_cap_hoc", Sys_This_Cap_Hoc);
                objLop.SelectParameters.Add("idTruong", Sys_This_Truong.ID.ToString());
                objLop.SelectParameters.Add("maNamHoc", Sys_Ma_Nam_hoc.ToString());
                rcbLop.DataBind();
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = lichThiBO.getLichThiByLop(Sys_This_Truong.ID, Convert.ToInt16(rcbKhoi.SelectedValue), Convert.ToInt16(Sys_Ma_Nam_hoc), Convert.ToInt16(Sys_Hoc_Ky), Convert.ToInt64(rcbLop.SelectedValue));
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
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
            short? ma_khoi = local_api.ConvertStringToShort(rcbKhoi.SelectedValue);
            long? id_lop = local_api.ConvertStringTolong(rcbLop.SelectedValue);

            if (ma_khoi == null || id_lop == null) return;

            LICH_THI detail = new LICH_THI();
            int success = 0;
            foreach (GridDataItem item in RadGrid1.Items)
            {
                detail = new LICH_THI();
                long? id_mon_truong = local_api.ConvertStringTolong(item["ID_MON_TRUONG"].Text);
                if (id_mon_truong != null)
                {
                    detail = lichThiBO.checkLichThiByMon(Sys_This_Truong.ID, ma_khoi.Value, id_lop.Value, (Int16)Sys_Hoc_Ky, id_mon_truong.Value);
                }

                #region get control
                RadDatePicker rd15p = (RadDatePicker)item.FindControl("rd15p");
                RadDatePicker rd1T = (RadDatePicker)item.FindControl("rd1T");
                RadDatePicker rdGK = (RadDatePicker)item.FindControl("rdGK");
                RadDatePicker rdHK = (RadDatePicker)item.FindControl("rdHK");
                RadTimePicker radTime15p = (RadTimePicker)item.FindControl("radTime15p");
                RadTimePicker radTime1T = (RadTimePicker)item.FindControl("radTime1T");
                RadTimePicker radTimeGK = (RadTimePicker)item.FindControl("radTimeGK");
                RadTimePicker radTimeHK = (RadTimePicker)item.FindControl("radTimeHK");

                DateTime? time15P = null;
                DateTime? time1T = null;
                DateTime? timeGK = null;
                DateTime? timeHK = null;

                string hour_15p = "";
                string hour_1t = "";
                string hour_GK = "";
                string hour_HK = "";
                var timer_15p = "";
                var timer_1t = "";
                var timer_GK = "";
                var timer_HK = "";

                if (radTime15p.SelectedTime != null) timer_15p = radTime15p.SelectedTime.Value.ToString();
                if (radTime1T.SelectedTime != null) timer_1t = radTime1T.SelectedTime.Value.ToString();
                if (radTimeGK.SelectedTime != null) timer_GK = radTimeGK.SelectedTime.Value.ToString();

                try
                {
                    hour_15p = rd15p.SelectedDate.Value.ToString("yyyy/MM/dd");
                    time15P = Convert.ToDateTime(hour_15p + " " + timer_15p);
                }
                catch { }

                try
                {
                    hour_1t = rd1T.SelectedDate.Value.ToString("yyyy/MM/dd");
                    time1T = Convert.ToDateTime(hour_1t + " " + timer_1t);
                }
                catch { }


                try
                {
                    hour_GK = rdGK.SelectedDate.Value.ToString("yyyy/MM/dd");
                    timeGK = Convert.ToDateTime(hour_GK + " " + timer_GK);
                }
                catch { }

                try
                {
                    hour_HK = rdHK.SelectedDate.Value.ToString("yyyy/MM/dd");
                    timeHK = Convert.ToDateTime(hour_HK + " " + timer_HK);
                }
                catch { }
                #endregion

                if (detail == null)
                {
                    detail = new LICH_THI();
                    detail.ID_TRUONG = Sys_This_Truong.ID;
                    detail.ID_KHOI = ma_khoi.Value;
                    detail.ID_NAM_HOC = (Int16)Sys_Ma_Nam_hoc;
                    detail.HOC_KY = (Int16)Sys_Hoc_Ky;
                    detail.ID_LOP = id_lop.Value;
                    detail.ID_MON_TRUONG = id_mon_truong.Value;
                    detail.TIME_1T = time1T;
                    detail.TIME_GK = timeGK;
                    detail.TIME_HK = timeHK;
                    detail.TIME_15P = time15P;
                    //detail.THOI_GIAN_LAM_BAI
                    res = lichThiBO.insert(detail, Sys_User.ID);
                }
                else
                {
                    detail.TIME_15P = time15P;
                    detail.TIME_1T = time1T;
                    detail.TIME_GK = timeGK;
                    detail.TIME_HK = timeHK;
                    res = lichThiBO.update(detail, Sys_User.ID);
                }
            }
            string strMsg = "";
            if (success > 0)
            {
                strMsg += " notification('success', 'Có " + success + " bản ghi được lưu.');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
        }
        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                RadTimePicker radTime1T = (RadTimePicker)item.FindControl("radTime1T");
                RadTimePicker radTimeGK = (RadTimePicker)item.FindControl("radTimeGK");
                RadTimePicker radTimeHK = (RadTimePicker)item.FindControl("radTimeHK");
                RadTimePicker radTime15P = (RadTimePicker)item.FindControl("radTime15P");

                if (radTime15P.SelectedTime != null)
                {
                    if (radTime15P.SelectedTime.Value.ToString() == "00:00:00") radTime15P.SelectedTime = null;
                }
                else radTime15P.SelectedTime = null;

                if (radTime1T.SelectedTime != null)
                {
                    if (radTime1T.SelectedTime.Value.ToString() == "00:00:00") radTime1T.SelectedTime = null;
                }
                else radTime1T.SelectedTime = null;

                if (radTimeGK.SelectedTime != null)
                {
                    if (radTimeGK.SelectedTime.Value.ToString() == "00:00:00") radTimeGK.SelectedTime = null;
                }
                else radTimeGK.SelectedTime = null;

                if (radTimeHK.SelectedTime != null)
                {
                    if (radTimeHK.SelectedTime.Value.ToString() == "00:00:00") radTimeHK.SelectedTime = null;
                }
                else radTimeHK.SelectedTime = null;
            }
        }
    }
}