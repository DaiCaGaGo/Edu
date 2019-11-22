using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CMS.ChuyenCan
{
    public partial class NhapChuyenCan : AuthenticatePage
    {
        private ChuyenCanBO ccBO = new ChuyenCanBO();
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
                #region set tháng năm học
               int nam_hoc = Sys_Ma_Nam_hoc;
                for(int i=8;i<=12;i++)
                    rcbThang.Items.Add( new RadComboBoxItem(i + " - " + nam_hoc, i.ToString()));
                for (int i = 1; i <= 6; i++)
                    rcbThang.Items.Add(new RadComboBoxItem(i + " - " + (nam_hoc+1), i.ToString()));
                rcbThang.DataBind();
                rcbThang.SelectedValue = DateTime.Now.Month.ToString();
                tuanDatabin();
                rcbTuan.SelectedValue = local_api.getThisWeek().ToString();
                #endregion
                ccBO.insertFirstData(local_api.ConvertStringTolong(rcbLop.SelectedValue), Convert.ToInt16(Sys_Ma_Nam_hoc));
            }
        }
        protected void tuanDatabin()
        {
            rcbTuan.Items.Clear();
            int nam_hoc = Sys_Ma_Nam_hoc;
            short thang = Convert.ToInt16(rcbThang.SelectedValue);
            if (thang < 8) nam_hoc = Sys_Ma_Nam_hoc + 1;
            int soTuanTrongThang = local_api.getSoTuanTrongThang(thang, nam_hoc);
            for (int i = 1; i <= soTuanTrongThang; i++)
            {
                rcbTuan.Items.Add(new RadComboBoxItem("Tuần " + i, i.ToString()));
            }
            rcbTuan.DataBind();
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {

            int nam_hoc = Sys_Ma_Nam_hoc;
            short thang = Convert.ToInt16(rcbThang.SelectedValue);
            if (thang < 8) nam_hoc = Sys_Ma_Nam_hoc + 1;
            RadGrid1.DataSource = ccBO.getDiemChuyenCanByTruongLopNamThang(Sys_This_Truong.ID, local_api.ConvertStringToShort(rcbKhoi.SelectedValue), local_api.ConvertStringTolong(rcbLop.SelectedValue), Convert.ToInt16(Sys_Ma_Nam_hoc), Convert.ToInt16(rcbThang.SelectedValue));
            setViewColumnGrid(nam_hoc, thang, Convert.ToInt16(rcbTuan.SelectedValue));
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
        protected void setViewColumnGrid(int nam, short thang, short tuan)
        {
            DateTime ngayDauThang = new DateTime(nam, thang, 1);
            DateTime ngayCuoiThang = ngayDauThang.AddMonths(1).AddDays(-1);
            int sttThuDauThang = (int)ngayDauThang.DayOfWeek;
            int ngay_dau_tuan = 1;
            int ngay_cuoi_tuan = 1 + 7 - sttThuDauThang;
            int soTuanTrongThang = local_api.getSoTuanTrongThang(thang, nam);
            if (tuan != 1)
            {
                ngay_dau_tuan = (tuan - 1) * 7 - (sttThuDauThang-1) + 1;
                ngay_cuoi_tuan = ngay_dau_tuan + 6;
                if (ngay_cuoi_tuan > ngayCuoiThang.Day) ngay_cuoi_tuan = ngayCuoiThang.Day;
            }

            for (int i = 1; i <= 31; i++)
            {
                if (i < ngay_dau_tuan || i > ngay_cuoi_tuan)
                {
                    RadGrid1.MasterTableView.Columns.FindByUniqueName("NGAY" + i).Display = false;
                }
                else
                {
                    DateTime ngay_hien_tai = new DateTime(nam, thang, i);
                    string ten_ngay_ht = ngay_hien_tai.DayOfWeek.ToString();
                    RadGrid1.MasterTableView.Columns.FindByUniqueName("NGAY" + i).ColumnGroupName = "Tuan" + tuan;
                    RadGrid1.MasterTableView.Columns.FindByUniqueName("NGAY" + i).HeaderText = local_api.convertNgayTrongTuan(ngay_hien_tai) + " (" + i + ")";
                    RadGrid1.MasterTableView.Columns.FindByUniqueName("NGAY" + i).Display = true;
                }
            }
        }
        protected void rcbKhoi_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbLop.ClearSelection();
            rcbLop.Text = String.Empty;
            rcbLop.DataBind();
            ccBO.insertFirstData(local_api.ConvertStringTolong(rcbLop.SelectedValue), Convert.ToInt16(Sys_Ma_Nam_hoc));
            RadGrid1.Rebind();
        }
        protected void rcbLop_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ccBO.insertFirstData(local_api.ConvertStringTolong(rcbLop.SelectedValue), Convert.ToInt16(Sys_Ma_Nam_hoc));
            RadGrid1.Rebind();
        }
        protected void rcbThang_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            tuanDatabin();
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

            CHUYEN_CAN detail = new CHUYEN_CAN();
            int success = 0; int count_change = 0;
            foreach (GridDataItem item in RadGrid1.Items)
            {
                detail = new CHUYEN_CAN();
                long id_chuyen_can = local_api.ConvertStringTolong(item.GetDataKeyValue("ID").ToString()).Value;
                detail = ccBO.getChuyenCanByID(id_chuyen_can);
                if (detail != null)
                {
                    #region get control
                    TextBox tbNgay1 = (TextBox)item.FindControl("tbNgay1");
                    TextBox tbNgay2 = (TextBox)item.FindControl("tbNgay2");
                    TextBox tbNgay3 = (TextBox)item.FindControl("tbNgay3");
                    TextBox tbNgay4 = (TextBox)item.FindControl("tbNgay4");
                    TextBox tbNgay5 = (TextBox)item.FindControl("tbNgay5");
                    TextBox tbNgay6 = (TextBox)item.FindControl("tbNgay6");
                    TextBox tbNgay7 = (TextBox)item.FindControl("tbNgay7");
                    TextBox tbNgay8 = (TextBox)item.FindControl("tbNgay8");
                    TextBox tbNgay9 = (TextBox)item.FindControl("tbNgay9");
                    TextBox tbNgay10 = (TextBox)item.FindControl("tbNgay10");
                    TextBox tbNgay11 = (TextBox)item.FindControl("tbNgay11");
                    TextBox tbNgay12 = (TextBox)item.FindControl("tbNgay12");
                    TextBox tbNgay13 = (TextBox)item.FindControl("tbNgay13");
                    TextBox tbNgay14 = (TextBox)item.FindControl("tbNgay14");
                    TextBox tbNgay15 = (TextBox)item.FindControl("tbNgay15");
                    TextBox tbNgay16 = (TextBox)item.FindControl("tbNgay16");
                    TextBox tbNgay17 = (TextBox)item.FindControl("tbNgay17");
                    TextBox tbNgay18 = (TextBox)item.FindControl("tbNgay18");
                    TextBox tbNgay19 = (TextBox)item.FindControl("tbNgay19");
                    TextBox tbNgay20 = (TextBox)item.FindControl("tbNgay20");
                    TextBox tbNgay21 = (TextBox)item.FindControl("tbNgay21");
                    TextBox tbNgay22 = (TextBox)item.FindControl("tbNgay22");
                    TextBox tbNgay23 = (TextBox)item.FindControl("tbNgay23");
                    TextBox tbNgay24 = (TextBox)item.FindControl("tbNgay24");
                    TextBox tbNgay25 = (TextBox)item.FindControl("tbNgay25");
                    TextBox tbNgay26 = (TextBox)item.FindControl("tbNgay26");
                    TextBox tbNgay27 = (TextBox)item.FindControl("tbNgay27");
                    TextBox tbNgay28 = (TextBox)item.FindControl("tbNgay28");
                    TextBox tbNgay29 = (TextBox)item.FindControl("tbNgay29");
                    TextBox tbNgay30 = (TextBox)item.FindControl("tbNgay30");
                    TextBox tbNgay31 = (TextBox)item.FindControl("tbNgay31");
                    TextBox tbTongPhep = (TextBox)item.FindControl("tbTongPhep");
                    TextBox tbTongKhongPhep = (TextBox)item.FindControl("tbTongKhongPhep");
                    HiddenField hdNgay1 = (HiddenField)item.FindControl("hdNgay1");
                    HiddenField hdNgay2 = (HiddenField)item.FindControl("hdNgay2");
                    HiddenField hdNgay3 = (HiddenField)item.FindControl("hdNgay3");
                    HiddenField hdNgay4 = (HiddenField)item.FindControl("hdNgay4");
                    HiddenField hdNgay5 = (HiddenField)item.FindControl("hdNgay5");
                    HiddenField hdNgay6 = (HiddenField)item.FindControl("hdNgay6");
                    HiddenField hdNgay7 = (HiddenField)item.FindControl("hdNgay7");
                    HiddenField hdNgay8 = (HiddenField)item.FindControl("hdNgay8");
                    HiddenField hdNgay9 = (HiddenField)item.FindControl("hdNgay9");
                    HiddenField hdNgay10 = (HiddenField)item.FindControl("hdNgay10");
                    HiddenField hdNgay11 = (HiddenField)item.FindControl("hdNgay11");
                    HiddenField hdNgay12 = (HiddenField)item.FindControl("hdNgay12");
                    HiddenField hdNgay13 = (HiddenField)item.FindControl("hdNgay13");
                    HiddenField hdNgay14 = (HiddenField)item.FindControl("hdNgay14");
                    HiddenField hdNgay15 = (HiddenField)item.FindControl("hdNgay15");
                    HiddenField hdNgay16 = (HiddenField)item.FindControl("hdNgay16");
                    HiddenField hdNgay17 = (HiddenField)item.FindControl("hdNgay17");
                    HiddenField hdNgay18 = (HiddenField)item.FindControl("hdNgay18");
                    HiddenField hdNgay19 = (HiddenField)item.FindControl("hdNgay19");
                    HiddenField hdNgay20 = (HiddenField)item.FindControl("hdNgay20");
                    HiddenField hdNgay21 = (HiddenField)item.FindControl("hdNgay21");
                    HiddenField hdNgay22 = (HiddenField)item.FindControl("hdNgay22");
                    HiddenField hdNgay23 = (HiddenField)item.FindControl("hdNgay23");
                    HiddenField hdNgay24 = (HiddenField)item.FindControl("hdNgay24");
                    HiddenField hdNgay25 = (HiddenField)item.FindControl("hdNgay25");
                    HiddenField hdNgay26 = (HiddenField)item.FindControl("hdNgay26");
                    HiddenField hdNgay27 = (HiddenField)item.FindControl("hdNgay27");
                    HiddenField hdNgay28 = (HiddenField)item.FindControl("hdNgay28");
                    HiddenField hdNgay29 = (HiddenField)item.FindControl("hdNgay29");
                    HiddenField hdNgay30 = (HiddenField)item.FindControl("hdNgay30");
                    HiddenField hdNgay31 = (HiddenField)item.FindControl("hdNgay31");
                    HiddenField hdTongPhep = (HiddenField)item.FindControl("hdTongPhep");
                    HiddenField hdTongKhongPhep = (HiddenField)item.FindControl("hdTongKhongPhep");
                    #endregion
                    #region get value control
                    string ngay1 = tbNgay1.Text.Trim();
                    string ngay2 = tbNgay2.Text.Trim();
                    string ngay3 = tbNgay3.Text.Trim();
                    string ngay4 = tbNgay4.Text.Trim();
                    string ngay5 = tbNgay5.Text.Trim();
                    string ngay6 = tbNgay6.Text.Trim();
                    string ngay7 = tbNgay7.Text.Trim();
                    string ngay8 = tbNgay8.Text.Trim();
                    string ngay9 = tbNgay9.Text.Trim();
                    string ngay10 = tbNgay10.Text.Trim();
                    string ngay11 = tbNgay11.Text.Trim();
                    string ngay12 = tbNgay12.Text.Trim();
                    string ngay13 = tbNgay13.Text.Trim();
                    string ngay14 = tbNgay14.Text.Trim();
                    string ngay15 = tbNgay15.Text.Trim();
                    string ngay16 = tbNgay16.Text.Trim();
                    string ngay17 = tbNgay17.Text.Trim();
                    string ngay18 = tbNgay18.Text.Trim();
                    string ngay19 = tbNgay19.Text.Trim();
                    string ngay20 = tbNgay20.Text.Trim();
                    string ngay21 = tbNgay21.Text.Trim();
                    string ngay22 = tbNgay22.Text.Trim();
                    string ngay23 = tbNgay23.Text.Trim();
                    string ngay24 = tbNgay24.Text.Trim();
                    string ngay25 = tbNgay25.Text.Trim();
                    string ngay26 = tbNgay26.Text.Trim();
                    string ngay27 = tbNgay27.Text.Trim();
                    string ngay28 = tbNgay28.Text.Trim();
                    string ngay29 = tbNgay29.Text.Trim();
                    string ngay30 = tbNgay30.Text.Trim();
                    string ngay31 = tbNgay31.Text.Trim();
                    string ngay1_old = hdNgay1.Value;
                    string ngay2_old = hdNgay2.Value;
                    string ngay3_old = hdNgay3.Value;
                    string ngay4_old = hdNgay4.Value;
                    string ngay5_old = hdNgay5.Value;
                    string ngay6_old = hdNgay6.Value;
                    string ngay7_old = hdNgay7.Value;
                    string ngay8_old = hdNgay8.Value;
                    string ngay9_old = hdNgay9.Value;
                    string ngay10_old = hdNgay10.Value;
                    string ngay11_old = hdNgay11.Value;
                    string ngay12_old = hdNgay12.Value;
                    string ngay13_old = hdNgay13.Value;
                    string ngay14_old = hdNgay14.Value;
                    string ngay15_old = hdNgay15.Value;
                    string ngay16_old = hdNgay16.Value;
                    string ngay17_old = hdNgay17.Value;
                    string ngay18_old = hdNgay18.Value;
                    string ngay19_old = hdNgay19.Value;
                    string ngay20_old = hdNgay20.Value;
                    string ngay21_old = hdNgay21.Value;
                    string ngay22_old = hdNgay22.Value;
                    string ngay23_old = hdNgay23.Value;
                    string ngay24_old = hdNgay24.Value;
                    string ngay25_old = hdNgay25.Value;
                    string ngay26_old = hdNgay26.Value;
                    string ngay27_old = hdNgay27.Value;
                    string ngay28_old = hdNgay28.Value;
                    string ngay29_old = hdNgay29.Value;
                    string ngay30_old = hdNgay30.Value;
                    string ngay31_old = hdNgay31.Value;
                    #endregion
                    #region set value detail
                    detail.NGAY1 = ngay1;
                    detail.NGAY2 = ngay2;
                    detail.NGAY3 = ngay3;
                    detail.NGAY4 = ngay4;
                    detail.NGAY5 = ngay5;
                    detail.NGAY6 = ngay6;
                    detail.NGAY7 = ngay7;
                    detail.NGAY8 = ngay8;
                    detail.NGAY9 = ngay9;
                    detail.NGAY10 = ngay10;
                    detail.NGAY11 = ngay11;
                    detail.NGAY12 = ngay12;
                    detail.NGAY13 = ngay13;
                    detail.NGAY14 = ngay14;
                    detail.NGAY15 = ngay15;
                    detail.NGAY16 = ngay16;
                    detail.NGAY17 = ngay17;
                    detail.NGAY18 = ngay18;
                    detail.NGAY19 = ngay19;
                    detail.NGAY20 = ngay20;
                    detail.NGAY21 = ngay21;
                    detail.NGAY22 = ngay22;
                    detail.NGAY23 = ngay23;
                    detail.NGAY24 = ngay24;
                    detail.NGAY25 = ngay25;
                    detail.NGAY26 = ngay26;
                    detail.NGAY27 = ngay27;
                    detail.NGAY28 = ngay28;
                    detail.NGAY29 = ngay29;
                    detail.NGAY30 = ngay30;
                    detail.NGAY31 = ngay31;
                    #endregion
                    if (ngay1 != ngay1_old || ngay2 != ngay2_old || ngay3 != ngay3_old || ngay4 != ngay4_old || ngay5 != ngay5_old || ngay6 != ngay6_old || ngay7 != ngay7_old || ngay8 != ngay8_old || ngay9 != ngay9_old || ngay10 != ngay10_old || ngay11 != ngay11_old || ngay12 != ngay12_old || ngay13 != ngay13_old || ngay14 != ngay14_old || ngay15 != ngay15_old || ngay16 != ngay16_old || ngay17 != ngay17_old || ngay18 != ngay18_old || ngay19 != ngay19_old || ngay20 != ngay20_old || ngay21 != ngay21_old || ngay22 != ngay22_old || ngay23 != ngay23_old || ngay24 != ngay24_old || ngay25 != ngay25_old || ngay26 != ngay26_old || ngay27 != ngay27_old || ngay28 != ngay28_old || ngay29 != ngay29_old || ngay30 != ngay30_old || ngay31 != ngay31_old)
                    {
                        count_change++;
                        res = ccBO.update(detail, Sys_User.ID);
                        if (res.Res)
                        {
                            success++;
                            logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật chuyên cần " + detail.ID_HOC_SINH, Sys_User.ID, DateTime.Now);
                        }
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
            RadGrid1.Rebind();
        }
        protected void rcbTuan_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
    }
}