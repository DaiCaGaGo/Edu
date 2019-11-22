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
    public partial class ChamAn : AuthenticatePage
    {
        private HocSinhBO hsBO = new HocSinhBO();
        ChamAnBO chamAnBO = new ChamAnBO();
        DMBuaAnBO buaAnBO = new DMBuaAnBO();
        DangKyAnBO dangKyAnBO = new DangKyAnBO();
        LocalAPI localAPI = new LocalAPI();
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
                rdNgay.SelectedDate = DateTime.Now;
                insertFirstData(Convert.ToDateTime(rdNgay.SelectedDate));
            }
        }
        public void insertFirstData(DateTime ngay)
        {
            List<DM_BUA_AN> lstBuaAn = buaAnBO.getBuaAnByTruongKhoi(Sys_This_Truong.ID, localAPI.ConvertStringToShort(rcbKhoi.SelectedValue));
            if (lstBuaAn.Count > 0 && rcbLop.SelectedValue != "" && ngay != null)
            {
                short thang = Convert.ToInt16(ngay.Month);
                chamAnBO.insertFirstData(Sys_This_Truong.ID, Convert.ToInt16(rcbKhoi.SelectedValue), Convert.ToInt16(Sys_Ma_Nam_hoc), Convert.ToInt64(rcbLop.SelectedValue), thang, Convert.ToInt16(Sys_Hoc_Ky), lstBuaAn);
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DateTime ngayDK = DateTime.Now;
            try
            {
                ngayDK = Convert.ToDateTime(rdNgay.SelectedDate);
            }
            catch { }
            int thang = Convert.ToInt16(ngayDK.Month);
            int ngay = Convert.ToInt16(ngayDK.Day);
            List<DM_BUA_AN> lstBuaAn = buaAnBO.getBuaAnByTruongKhoi(Sys_This_Truong.ID, localAPI.ConvertStringToShort(rcbKhoi.SelectedValue));
            if (lstBuaAn.Count > 0 && rcbLop.SelectedValue != "")
            {
                RadGrid1.DataSource = chamAnBO.getChamAnByTruongLopBuaAnNgay(Sys_This_Truong.ID, Convert.ToInt16(rcbKhoi.SelectedValue), Convert.ToInt16(Sys_Ma_Nam_hoc), Convert.ToInt64(rcbLop.SelectedValue), thang, ngay, Sys_Hoc_Ky, lstBuaAn);

                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("ID"));
                dt.Columns.Add(new DataColumn("TEN"));
                for (int i = 0; i < lstBuaAn.Count; i++)
                {
                    DataRow drow = dt.NewRow();
                    drow["ID"] = lstBuaAn[i].ID;
                    drow["TEN"] = lstBuaAn[i].TEN;
                    dt.Rows.Add(drow);
                }
                Session["DanhMucBuaAn" + Sys_User.ID] = dt;
                int countBuaAn = lstBuaAn.Count;
                for (int i = 0; i < 10; i++)
                {
                    if (i < countBuaAn) RadGrid1.MasterTableView.Columns.FindByUniqueName("BUA" + i).Display = true;
                    else RadGrid1.Columns.FindByUniqueName("BUA" + i).Display = false;
                }
            }

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
        protected void rcbKhoi_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbLop.ClearSelection();
            rcbLop.Text = String.Empty;
            rcbLop.DataBind();
            insertFirstData(Convert.ToDateTime(rdNgay.SelectedDate));
            RadGrid1.Rebind();
        }
        protected void rcbLop_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            insertFirstData(Convert.ToDateTime(rdNgay.SelectedDate));
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
            short? ma_khoi = localAPI.ConvertStringToShort(rcbKhoi.SelectedValue);
            long? id_lop = localAPI.ConvertStringTolong(rcbLop.SelectedValue);
            int ngay = Convert.ToDateTime(rdNgay.SelectedDate).Day;
            int thang = Convert.ToDateTime(rdNgay.SelectedDate).Month;
            int success = 0;
            if (id_lop != null)
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["DanhMucBuaAn" + Sys_User.ID];
                int count = 0;
                if (dt != null && dt.Rows.Count > 0) count = dt.Rows.Count;
                foreach (GridDataItem item in RadGrid1.Items)
                {
                    long id_hoc_sinh = Convert.ToInt64(item["ID_HOC_SINH"].Text);
                    #region get control
                    CheckBox chbBUA0 = (CheckBox)item.FindControl("chbBUA0");
                    CheckBox chbBUA1 = (CheckBox)item.FindControl("chbBUA1");
                    CheckBox chbBUA2 = (CheckBox)item.FindControl("chbBUA2");
                    CheckBox chbBUA3 = (CheckBox)item.FindControl("chbBUA3");
                    CheckBox chbBUA4 = (CheckBox)item.FindControl("chbBUA4");
                    #endregion
                    #region Bữa ăn 0
                    if (count > 0)
                    {
                        CHAM_AN detail0 = new CHAM_AN();
                        detail0 = chamAnBO.getChamAnBy_Truong_Lop_BuaAn_HocSinh_Thang_HocKy(Sys_This_Truong.ID, Convert.ToInt16(rcbKhoi.SelectedValue), Convert.ToInt16(Sys_Ma_Nam_hoc), id_lop.Value, id_hoc_sinh, thang, Sys_Hoc_Ky, Convert.ToInt64(dt.Rows[0]["ID"]));
                        if (detail0 != null)
                        {
                            if (ngay == 1) detail0.NGAY1 = chbBUA0.Checked;
                            else if (ngay == 2) detail0.NGAY2 = chbBUA0.Checked;
                            else if (ngay == 3) detail0.NGAY3 = chbBUA0.Checked;
                            else if (ngay == 4) detail0.NGAY4 = chbBUA0.Checked;
                            else if (ngay == 5) detail0.NGAY5 = chbBUA0.Checked;
                            else if (ngay == 6) detail0.NGAY6 = chbBUA0.Checked;
                            else if (ngay == 7) detail0.NGAY7 = chbBUA0.Checked;
                            else if (ngay == 8) detail0.NGAY8 = chbBUA0.Checked;
                            else if (ngay == 9) detail0.NGAY9 = chbBUA0.Checked;
                            else if (ngay == 10) detail0.NGAY10 = chbBUA0.Checked;
                            else if (ngay == 11) detail0.NGAY11 = chbBUA0.Checked;
                            else if (ngay == 12) detail0.NGAY12 = chbBUA0.Checked;
                            else if (ngay == 13) detail0.NGAY13 = chbBUA0.Checked;
                            else if (ngay == 14) detail0.NGAY14 = chbBUA0.Checked;
                            else if (ngay == 15) detail0.NGAY15 = chbBUA0.Checked;
                            else if (ngay == 16) detail0.NGAY16 = chbBUA0.Checked;
                            else if (ngay == 17) detail0.NGAY17 = chbBUA0.Checked;
                            else if (ngay == 18) detail0.NGAY18 = chbBUA0.Checked;
                            else if (ngay == 19) detail0.NGAY19 = chbBUA0.Checked;
                            else if (ngay == 20) detail0.NGAY20 = chbBUA0.Checked;
                            else if (ngay == 21) detail0.NGAY21 = chbBUA0.Checked;
                            else if (ngay == 22) detail0.NGAY22 = chbBUA0.Checked;
                            else if (ngay == 23) detail0.NGAY23 = chbBUA0.Checked;
                            else if (ngay == 24) detail0.NGAY24 = chbBUA0.Checked;
                            else if (ngay == 25) detail0.NGAY25 = chbBUA0.Checked;
                            else if (ngay == 26) detail0.NGAY26 = chbBUA0.Checked;
                            else if (ngay == 27) detail0.NGAY27 = chbBUA0.Checked;
                            else if (ngay == 28) detail0.NGAY28 = chbBUA0.Checked;
                            else if (ngay == 29) detail0.NGAY29 = chbBUA0.Checked;
                            else if (ngay == 30) detail0.NGAY30 = chbBUA0.Checked;
                            else if (ngay == 31) detail0.NGAY31 = chbBUA0.Checked;
                            res = chamAnBO.update(detail0, Sys_User.ID);
                            if (res.Res)
                                success++;
                        }
                    }
                    #endregion
                    #region Bữa ăn 1
                    if (count > 1)
                    {
                        CHAM_AN detail1 = new CHAM_AN();
                        detail1 = chamAnBO.getChamAnBy_Truong_Lop_BuaAn_HocSinh_Thang_HocKy(Sys_This_Truong.ID, Convert.ToInt16(rcbKhoi.SelectedValue), Convert.ToInt16(Sys_Ma_Nam_hoc), id_lop.Value, id_hoc_sinh, thang, Sys_Hoc_Ky, Convert.ToInt64(dt.Rows[1]["ID"]));
                        if (detail1 != null)
                        {
                            if (ngay == 1) detail1.NGAY1 = chbBUA1.Checked;
                            else if (ngay == 2) detail1.NGAY2 = chbBUA1.Checked;
                            else if (ngay == 3) detail1.NGAY3 = chbBUA1.Checked;
                            else if (ngay == 4) detail1.NGAY4 = chbBUA1.Checked;
                            else if (ngay == 5) detail1.NGAY5 = chbBUA1.Checked;
                            else if (ngay == 6) detail1.NGAY6 = chbBUA1.Checked;
                            else if (ngay == 7) detail1.NGAY7 = chbBUA1.Checked;
                            else if (ngay == 8) detail1.NGAY8 = chbBUA1.Checked;
                            else if (ngay == 9) detail1.NGAY9 = chbBUA1.Checked;
                            else if (ngay == 10) detail1.NGAY10 = chbBUA1.Checked;
                            else if (ngay == 11) detail1.NGAY11 = chbBUA1.Checked;
                            else if (ngay == 12) detail1.NGAY12 = chbBUA1.Checked;
                            else if (ngay == 13) detail1.NGAY13 = chbBUA1.Checked;
                            else if (ngay == 14) detail1.NGAY14 = chbBUA1.Checked;
                            else if (ngay == 15) detail1.NGAY15 = chbBUA1.Checked;
                            else if (ngay == 16) detail1.NGAY16 = chbBUA1.Checked;
                            else if (ngay == 17) detail1.NGAY17 = chbBUA1.Checked;
                            else if (ngay == 18) detail1.NGAY18 = chbBUA1.Checked;
                            else if (ngay == 19) detail1.NGAY19 = chbBUA1.Checked;
                            else if (ngay == 20) detail1.NGAY20 = chbBUA1.Checked;
                            else if (ngay == 21) detail1.NGAY21 = chbBUA1.Checked;
                            else if (ngay == 22) detail1.NGAY22 = chbBUA1.Checked;
                            else if (ngay == 23) detail1.NGAY23 = chbBUA1.Checked;
                            else if (ngay == 24) detail1.NGAY24 = chbBUA1.Checked;
                            else if (ngay == 25) detail1.NGAY25 = chbBUA1.Checked;
                            else if (ngay == 26) detail1.NGAY26 = chbBUA1.Checked;
                            else if (ngay == 27) detail1.NGAY27 = chbBUA1.Checked;
                            else if (ngay == 28) detail1.NGAY28 = chbBUA1.Checked;
                            else if (ngay == 29) detail1.NGAY29 = chbBUA1.Checked;
                            else if (ngay == 30) detail1.NGAY30 = chbBUA1.Checked;
                            else if (ngay == 31) detail1.NGAY31 = chbBUA1.Checked;
                            res = chamAnBO.update(detail1, Sys_User.ID);
                            if (res.Res)
                                success++;
                        }
                    }
                    #endregion
                    #region Bữa ăn 2
                    if (count > 2)
                    {
                        CHAM_AN detail2 = new CHAM_AN();
                        detail2 = chamAnBO.getChamAnBy_Truong_Lop_BuaAn_HocSinh_Thang_HocKy(Sys_This_Truong.ID, Convert.ToInt16(rcbKhoi.SelectedValue), Convert.ToInt16(Sys_Ma_Nam_hoc), id_lop.Value, id_hoc_sinh, thang, Sys_Hoc_Ky, Convert.ToInt64(dt.Rows[2]["ID"]));
                        if (detail2 != null)
                        {
                            if (ngay == 1) detail2.NGAY1 = chbBUA0.Checked;
                            else if (ngay == 2) detail2.NGAY2 = chbBUA2.Checked;
                            else if (ngay == 3) detail2.NGAY3 = chbBUA2.Checked;
                            else if (ngay == 4) detail2.NGAY4 = chbBUA2.Checked;
                            else if (ngay == 5) detail2.NGAY5 = chbBUA2.Checked;
                            else if (ngay == 6) detail2.NGAY6 = chbBUA2.Checked;
                            else if (ngay == 7) detail2.NGAY7 = chbBUA2.Checked;
                            else if (ngay == 8) detail2.NGAY8 = chbBUA2.Checked;
                            else if (ngay == 9) detail2.NGAY9 = chbBUA2.Checked;
                            else if (ngay == 10) detail2.NGAY10 = chbBUA2.Checked;
                            else if (ngay == 11) detail2.NGAY11 = chbBUA2.Checked;
                            else if (ngay == 12) detail2.NGAY12 = chbBUA2.Checked;
                            else if (ngay == 13) detail2.NGAY13 = chbBUA2.Checked;
                            else if (ngay == 14) detail2.NGAY14 = chbBUA2.Checked;
                            else if (ngay == 15) detail2.NGAY15 = chbBUA2.Checked;
                            else if (ngay == 16) detail2.NGAY16 = chbBUA2.Checked;
                            else if (ngay == 17) detail2.NGAY17 = chbBUA2.Checked;
                            else if (ngay == 18) detail2.NGAY18 = chbBUA2.Checked;
                            else if (ngay == 19) detail2.NGAY19 = chbBUA2.Checked;
                            else if (ngay == 20) detail2.NGAY20 = chbBUA2.Checked;
                            else if (ngay == 21) detail2.NGAY21 = chbBUA2.Checked;
                            else if (ngay == 22) detail2.NGAY22 = chbBUA2.Checked;
                            else if (ngay == 23) detail2.NGAY23 = chbBUA2.Checked;
                            else if (ngay == 24) detail2.NGAY24 = chbBUA2.Checked;
                            else if (ngay == 25) detail2.NGAY25 = chbBUA2.Checked;
                            else if (ngay == 26) detail2.NGAY26 = chbBUA2.Checked;
                            else if (ngay == 27) detail2.NGAY27 = chbBUA2.Checked;
                            else if (ngay == 28) detail2.NGAY28 = chbBUA2.Checked;
                            else if (ngay == 29) detail2.NGAY29 = chbBUA2.Checked;
                            else if (ngay == 30) detail2.NGAY30 = chbBUA2.Checked;
                            else if (ngay == 31) detail2.NGAY31 = chbBUA2.Checked;
                            res = chamAnBO.update(detail2, Sys_User.ID);
                            if (res.Res)
                                success++;
                        }
                    }
                    #endregion
                    #region bữa ăn 3
                    if (count > 3)
                    {
                        CHAM_AN detail3 = new CHAM_AN();
                        detail3 = chamAnBO.getChamAnBy_Truong_Lop_BuaAn_HocSinh_Thang_HocKy(Sys_This_Truong.ID, Convert.ToInt16(rcbKhoi.SelectedValue), Convert.ToInt16(Sys_Ma_Nam_hoc), id_lop.Value, id_hoc_sinh, thang, Sys_Hoc_Ky, Convert.ToInt64(dt.Rows[3]["ID"]));
                        if (detail3 != null)
                        {
                            if (ngay == 1) detail3.NGAY1 = chbBUA3.Checked;
                            else if (ngay == 2) detail3.NGAY2 = chbBUA3.Checked;
                            else if (ngay == 3) detail3.NGAY3 = chbBUA3.Checked;
                            else if (ngay == 4) detail3.NGAY4 = chbBUA3.Checked;
                            else if (ngay == 5) detail3.NGAY5 = chbBUA3.Checked;
                            else if (ngay == 6) detail3.NGAY6 = chbBUA3.Checked;
                            else if (ngay == 7) detail3.NGAY7 = chbBUA3.Checked;
                            else if (ngay == 8) detail3.NGAY8 = chbBUA3.Checked;
                            else if (ngay == 9) detail3.NGAY9 = chbBUA3.Checked;
                            else if (ngay == 10) detail3.NGAY10 = chbBUA3.Checked;
                            else if (ngay == 11) detail3.NGAY11 = chbBUA3.Checked;
                            else if (ngay == 12) detail3.NGAY12 = chbBUA3.Checked;
                            else if (ngay == 13) detail3.NGAY13 = chbBUA3.Checked;
                            else if (ngay == 14) detail3.NGAY14 = chbBUA3.Checked;
                            else if (ngay == 15) detail3.NGAY15 = chbBUA3.Checked;
                            else if (ngay == 16) detail3.NGAY16 = chbBUA3.Checked;
                            else if (ngay == 17) detail3.NGAY17 = chbBUA3.Checked;
                            else if (ngay == 18) detail3.NGAY18 = chbBUA3.Checked;
                            else if (ngay == 19) detail3.NGAY19 = chbBUA3.Checked;
                            else if (ngay == 20) detail3.NGAY20 = chbBUA3.Checked;
                            else if (ngay == 21) detail3.NGAY21 = chbBUA3.Checked;
                            else if (ngay == 22) detail3.NGAY22 = chbBUA3.Checked;
                            else if (ngay == 23) detail3.NGAY23 = chbBUA3.Checked;
                            else if (ngay == 24) detail3.NGAY24 = chbBUA3.Checked;
                            else if (ngay == 25) detail3.NGAY25 = chbBUA3.Checked;
                            else if (ngay == 26) detail3.NGAY26 = chbBUA3.Checked;
                            else if (ngay == 27) detail3.NGAY27 = chbBUA3.Checked;
                            else if (ngay == 28) detail3.NGAY28 = chbBUA3.Checked;
                            else if (ngay == 29) detail3.NGAY29 = chbBUA3.Checked;
                            else if (ngay == 30) detail3.NGAY30 = chbBUA3.Checked;
                            else if (ngay == 31) detail3.NGAY31 = chbBUA3.Checked;
                            res = chamAnBO.update(detail3, Sys_User.ID);
                            if (res.Res)
                                success++;
                        }
                    }
                    #endregion
                    #region bữa ăn 4
                    if (count > 4)
                    {
                        CHAM_AN detail4 = new CHAM_AN();
                        detail4 = chamAnBO.getChamAnBy_Truong_Lop_BuaAn_HocSinh_Thang_HocKy(Sys_This_Truong.ID, Convert.ToInt16(rcbKhoi.SelectedValue), Convert.ToInt16(Sys_Ma_Nam_hoc), id_lop.Value, id_hoc_sinh, thang, Sys_Hoc_Ky, Convert.ToInt64(dt.Rows[4]["ID"]));
                        if (detail4 != null)
                        {
                            if (ngay == 1) detail4.NGAY1 = chbBUA4.Checked;
                            else if (ngay == 2) detail4.NGAY2 = chbBUA4.Checked;
                            else if (ngay == 3) detail4.NGAY3 = chbBUA4.Checked;
                            else if (ngay == 4) detail4.NGAY4 = chbBUA4.Checked;
                            else if (ngay == 5) detail4.NGAY5 = chbBUA4.Checked;
                            else if (ngay == 6) detail4.NGAY6 = chbBUA4.Checked;
                            else if (ngay == 7) detail4.NGAY7 = chbBUA4.Checked;
                            else if (ngay == 8) detail4.NGAY8 = chbBUA4.Checked;
                            else if (ngay == 9) detail4.NGAY9 = chbBUA4.Checked;
                            else if (ngay == 10) detail4.NGAY10 = chbBUA4.Checked;
                            else if (ngay == 11) detail4.NGAY11 = chbBUA4.Checked;
                            else if (ngay == 12) detail4.NGAY12 = chbBUA4.Checked;
                            else if (ngay == 13) detail4.NGAY13 = chbBUA4.Checked;
                            else if (ngay == 14) detail4.NGAY14 = chbBUA4.Checked;
                            else if (ngay == 15) detail4.NGAY15 = chbBUA4.Checked;
                            else if (ngay == 16) detail4.NGAY16 = chbBUA4.Checked;
                            else if (ngay == 17) detail4.NGAY17 = chbBUA4.Checked;
                            else if (ngay == 18) detail4.NGAY18 = chbBUA4.Checked;
                            else if (ngay == 19) detail4.NGAY19 = chbBUA4.Checked;
                            else if (ngay == 20) detail4.NGAY20 = chbBUA4.Checked;
                            else if (ngay == 21) detail4.NGAY21 = chbBUA4.Checked;
                            else if (ngay == 22) detail4.NGAY22 = chbBUA4.Checked;
                            else if (ngay == 23) detail4.NGAY23 = chbBUA4.Checked;
                            else if (ngay == 24) detail4.NGAY24 = chbBUA4.Checked;
                            else if (ngay == 25) detail4.NGAY25 = chbBUA4.Checked;
                            else if (ngay == 26) detail4.NGAY26 = chbBUA4.Checked;
                            else if (ngay == 27) detail4.NGAY27 = chbBUA4.Checked;
                            else if (ngay == 28) detail4.NGAY28 = chbBUA4.Checked;
                            else if (ngay == 29) detail4.NGAY29 = chbBUA4.Checked;
                            else if (ngay == 30) detail4.NGAY30 = chbBUA4.Checked;
                            else if (ngay == 31) detail4.NGAY31 = chbBUA4.Checked;
                            res = chamAnBO.update(detail4, Sys_User.ID);
                            if (res.Res)
                                success++;
                        }
                    }
                    #endregion
                    #region Bữa ăn 5
                    if (count > 5)
                    {
                        CHAM_AN detail0 = new CHAM_AN();
                        detail0 = chamAnBO.getChamAnBy_Truong_Lop_BuaAn_HocSinh_Thang_HocKy(Sys_This_Truong.ID, Convert.ToInt16(rcbKhoi.SelectedValue), Convert.ToInt16(Sys_Ma_Nam_hoc), id_lop.Value, id_hoc_sinh, thang, Sys_Hoc_Ky, Convert.ToInt64(dt.Rows[5]["ID"]));
                        if (detail0 != null)
                        {
                            if (ngay == 1) detail0.NGAY1 = chbBUA0.Checked;
                            else if (ngay == 2) detail0.NGAY2 = chbBUA0.Checked;
                            else if (ngay == 3) detail0.NGAY3 = chbBUA0.Checked;
                            else if (ngay == 4) detail0.NGAY4 = chbBUA0.Checked;
                            else if (ngay == 5) detail0.NGAY5 = chbBUA0.Checked;
                            else if (ngay == 6) detail0.NGAY6 = chbBUA0.Checked;
                            else if (ngay == 7) detail0.NGAY7 = chbBUA0.Checked;
                            else if (ngay == 8) detail0.NGAY8 = chbBUA0.Checked;
                            else if (ngay == 9) detail0.NGAY9 = chbBUA0.Checked;
                            else if (ngay == 10) detail0.NGAY10 = chbBUA0.Checked;
                            else if (ngay == 11) detail0.NGAY11 = chbBUA0.Checked;
                            else if (ngay == 12) detail0.NGAY12 = chbBUA0.Checked;
                            else if (ngay == 13) detail0.NGAY13 = chbBUA0.Checked;
                            else if (ngay == 14) detail0.NGAY14 = chbBUA0.Checked;
                            else if (ngay == 15) detail0.NGAY15 = chbBUA0.Checked;
                            else if (ngay == 16) detail0.NGAY16 = chbBUA0.Checked;
                            else if (ngay == 17) detail0.NGAY17 = chbBUA0.Checked;
                            else if (ngay == 18) detail0.NGAY18 = chbBUA0.Checked;
                            else if (ngay == 19) detail0.NGAY19 = chbBUA0.Checked;
                            else if (ngay == 20) detail0.NGAY20 = chbBUA0.Checked;
                            else if (ngay == 21) detail0.NGAY21 = chbBUA0.Checked;
                            else if (ngay == 22) detail0.NGAY22 = chbBUA0.Checked;
                            else if (ngay == 23) detail0.NGAY23 = chbBUA0.Checked;
                            else if (ngay == 24) detail0.NGAY24 = chbBUA0.Checked;
                            else if (ngay == 25) detail0.NGAY25 = chbBUA0.Checked;
                            else if (ngay == 26) detail0.NGAY26 = chbBUA0.Checked;
                            else if (ngay == 27) detail0.NGAY27 = chbBUA0.Checked;
                            else if (ngay == 28) detail0.NGAY28 = chbBUA0.Checked;
                            else if (ngay == 29) detail0.NGAY29 = chbBUA0.Checked;
                            else if (ngay == 30) detail0.NGAY30 = chbBUA0.Checked;
                            else if (ngay == 31) detail0.NGAY31 = chbBUA0.Checked;
                            res = chamAnBO.update(detail0, Sys_User.ID);
                            if (res.Res)
                                success++;
                        }
                    }
                    #endregion
                    #region Bữa ăn 6
                    if (count > 6)
                    {
                        CHAM_AN detail0 = new CHAM_AN();
                        detail0 = chamAnBO.getChamAnBy_Truong_Lop_BuaAn_HocSinh_Thang_HocKy(Sys_This_Truong.ID, Convert.ToInt16(rcbKhoi.SelectedValue), Convert.ToInt16(Sys_Ma_Nam_hoc), id_lop.Value, id_hoc_sinh, thang, Sys_Hoc_Ky, Convert.ToInt64(dt.Rows[6]["ID"]));
                        if (detail0 != null)
                        {
                            if (ngay == 1) detail0.NGAY1 = chbBUA0.Checked;
                            else if (ngay == 2) detail0.NGAY2 = chbBUA0.Checked;
                            else if (ngay == 3) detail0.NGAY3 = chbBUA0.Checked;
                            else if (ngay == 4) detail0.NGAY4 = chbBUA0.Checked;
                            else if (ngay == 5) detail0.NGAY5 = chbBUA0.Checked;
                            else if (ngay == 6) detail0.NGAY6 = chbBUA0.Checked;
                            else if (ngay == 7) detail0.NGAY7 = chbBUA0.Checked;
                            else if (ngay == 8) detail0.NGAY8 = chbBUA0.Checked;
                            else if (ngay == 9) detail0.NGAY9 = chbBUA0.Checked;
                            else if (ngay == 10) detail0.NGAY10 = chbBUA0.Checked;
                            else if (ngay == 11) detail0.NGAY11 = chbBUA0.Checked;
                            else if (ngay == 12) detail0.NGAY12 = chbBUA0.Checked;
                            else if (ngay == 13) detail0.NGAY13 = chbBUA0.Checked;
                            else if (ngay == 14) detail0.NGAY14 = chbBUA0.Checked;
                            else if (ngay == 15) detail0.NGAY15 = chbBUA0.Checked;
                            else if (ngay == 16) detail0.NGAY16 = chbBUA0.Checked;
                            else if (ngay == 17) detail0.NGAY17 = chbBUA0.Checked;
                            else if (ngay == 18) detail0.NGAY18 = chbBUA0.Checked;
                            else if (ngay == 19) detail0.NGAY19 = chbBUA0.Checked;
                            else if (ngay == 20) detail0.NGAY20 = chbBUA0.Checked;
                            else if (ngay == 21) detail0.NGAY21 = chbBUA0.Checked;
                            else if (ngay == 22) detail0.NGAY22 = chbBUA0.Checked;
                            else if (ngay == 23) detail0.NGAY23 = chbBUA0.Checked;
                            else if (ngay == 24) detail0.NGAY24 = chbBUA0.Checked;
                            else if (ngay == 25) detail0.NGAY25 = chbBUA0.Checked;
                            else if (ngay == 26) detail0.NGAY26 = chbBUA0.Checked;
                            else if (ngay == 27) detail0.NGAY27 = chbBUA0.Checked;
                            else if (ngay == 28) detail0.NGAY28 = chbBUA0.Checked;
                            else if (ngay == 29) detail0.NGAY29 = chbBUA0.Checked;
                            else if (ngay == 30) detail0.NGAY30 = chbBUA0.Checked;
                            else if (ngay == 31) detail0.NGAY31 = chbBUA0.Checked;
                            res = chamAnBO.update(detail0, Sys_User.ID);
                            if (res.Res)
                                success++;
                        }
                    }
                    #endregion
                    #region Bữa ăn 7
                    if (count > 7)
                    {
                        CHAM_AN detail0 = new CHAM_AN();
                        detail0 = chamAnBO.getChamAnBy_Truong_Lop_BuaAn_HocSinh_Thang_HocKy(Sys_This_Truong.ID, Convert.ToInt16(rcbKhoi.SelectedValue), Convert.ToInt16(Sys_Ma_Nam_hoc), id_lop.Value, id_hoc_sinh, thang, Sys_Hoc_Ky, Convert.ToInt64(dt.Rows[7]["ID"]));
                        if (detail0 != null)
                        {
                            if (ngay == 1) detail0.NGAY1 = chbBUA0.Checked;
                            else if (ngay == 2) detail0.NGAY2 = chbBUA0.Checked;
                            else if (ngay == 3) detail0.NGAY3 = chbBUA0.Checked;
                            else if (ngay == 4) detail0.NGAY4 = chbBUA0.Checked;
                            else if (ngay == 5) detail0.NGAY5 = chbBUA0.Checked;
                            else if (ngay == 6) detail0.NGAY6 = chbBUA0.Checked;
                            else if (ngay == 7) detail0.NGAY7 = chbBUA0.Checked;
                            else if (ngay == 8) detail0.NGAY8 = chbBUA0.Checked;
                            else if (ngay == 9) detail0.NGAY9 = chbBUA0.Checked;
                            else if (ngay == 10) detail0.NGAY10 = chbBUA0.Checked;
                            else if (ngay == 11) detail0.NGAY11 = chbBUA0.Checked;
                            else if (ngay == 12) detail0.NGAY12 = chbBUA0.Checked;
                            else if (ngay == 13) detail0.NGAY13 = chbBUA0.Checked;
                            else if (ngay == 14) detail0.NGAY14 = chbBUA0.Checked;
                            else if (ngay == 15) detail0.NGAY15 = chbBUA0.Checked;
                            else if (ngay == 16) detail0.NGAY16 = chbBUA0.Checked;
                            else if (ngay == 17) detail0.NGAY17 = chbBUA0.Checked;
                            else if (ngay == 18) detail0.NGAY18 = chbBUA0.Checked;
                            else if (ngay == 19) detail0.NGAY19 = chbBUA0.Checked;
                            else if (ngay == 20) detail0.NGAY20 = chbBUA0.Checked;
                            else if (ngay == 21) detail0.NGAY21 = chbBUA0.Checked;
                            else if (ngay == 22) detail0.NGAY22 = chbBUA0.Checked;
                            else if (ngay == 23) detail0.NGAY23 = chbBUA0.Checked;
                            else if (ngay == 24) detail0.NGAY24 = chbBUA0.Checked;
                            else if (ngay == 25) detail0.NGAY25 = chbBUA0.Checked;
                            else if (ngay == 26) detail0.NGAY26 = chbBUA0.Checked;
                            else if (ngay == 27) detail0.NGAY27 = chbBUA0.Checked;
                            else if (ngay == 28) detail0.NGAY28 = chbBUA0.Checked;
                            else if (ngay == 29) detail0.NGAY29 = chbBUA0.Checked;
                            else if (ngay == 30) detail0.NGAY30 = chbBUA0.Checked;
                            else if (ngay == 31) detail0.NGAY31 = chbBUA0.Checked;
                            res = chamAnBO.update(detail0, Sys_User.ID);
                            if (res.Res)
                                success++;
                        }
                    }
                    #endregion
                    #region Bữa ăn 8
                    if (count > 8)
                    {
                        CHAM_AN detail0 = new CHAM_AN();
                        detail0 = chamAnBO.getChamAnBy_Truong_Lop_BuaAn_HocSinh_Thang_HocKy(Sys_This_Truong.ID, Convert.ToInt16(rcbKhoi.SelectedValue), Convert.ToInt16(Sys_Ma_Nam_hoc), id_lop.Value, id_hoc_sinh, thang, Sys_Hoc_Ky, Convert.ToInt64(dt.Rows[8]["ID"]));
                        if (detail0 != null)
                        {
                            if (ngay == 1) detail0.NGAY1 = chbBUA0.Checked;
                            else if (ngay == 2) detail0.NGAY2 = chbBUA0.Checked;
                            else if (ngay == 3) detail0.NGAY3 = chbBUA0.Checked;
                            else if (ngay == 4) detail0.NGAY4 = chbBUA0.Checked;
                            else if (ngay == 5) detail0.NGAY5 = chbBUA0.Checked;
                            else if (ngay == 6) detail0.NGAY6 = chbBUA0.Checked;
                            else if (ngay == 7) detail0.NGAY7 = chbBUA0.Checked;
                            else if (ngay == 8) detail0.NGAY8 = chbBUA0.Checked;
                            else if (ngay == 9) detail0.NGAY9 = chbBUA0.Checked;
                            else if (ngay == 10) detail0.NGAY10 = chbBUA0.Checked;
                            else if (ngay == 11) detail0.NGAY11 = chbBUA0.Checked;
                            else if (ngay == 12) detail0.NGAY12 = chbBUA0.Checked;
                            else if (ngay == 13) detail0.NGAY13 = chbBUA0.Checked;
                            else if (ngay == 14) detail0.NGAY14 = chbBUA0.Checked;
                            else if (ngay == 15) detail0.NGAY15 = chbBUA0.Checked;
                            else if (ngay == 16) detail0.NGAY16 = chbBUA0.Checked;
                            else if (ngay == 17) detail0.NGAY17 = chbBUA0.Checked;
                            else if (ngay == 18) detail0.NGAY18 = chbBUA0.Checked;
                            else if (ngay == 19) detail0.NGAY19 = chbBUA0.Checked;
                            else if (ngay == 20) detail0.NGAY20 = chbBUA0.Checked;
                            else if (ngay == 21) detail0.NGAY21 = chbBUA0.Checked;
                            else if (ngay == 22) detail0.NGAY22 = chbBUA0.Checked;
                            else if (ngay == 23) detail0.NGAY23 = chbBUA0.Checked;
                            else if (ngay == 24) detail0.NGAY24 = chbBUA0.Checked;
                            else if (ngay == 25) detail0.NGAY25 = chbBUA0.Checked;
                            else if (ngay == 26) detail0.NGAY26 = chbBUA0.Checked;
                            else if (ngay == 27) detail0.NGAY27 = chbBUA0.Checked;
                            else if (ngay == 28) detail0.NGAY28 = chbBUA0.Checked;
                            else if (ngay == 29) detail0.NGAY29 = chbBUA0.Checked;
                            else if (ngay == 30) detail0.NGAY30 = chbBUA0.Checked;
                            else if (ngay == 31) detail0.NGAY31 = chbBUA0.Checked;
                            res = chamAnBO.update(detail0, Sys_User.ID);
                            if (res.Res)
                                success++;
                        }
                    }
                    #endregion
                    #region Bữa ăn 9
                    if (count > 9)
                    {
                        CHAM_AN detail0 = new CHAM_AN();
                        detail0 = chamAnBO.getChamAnBy_Truong_Lop_BuaAn_HocSinh_Thang_HocKy(Sys_This_Truong.ID, Convert.ToInt16(rcbKhoi.SelectedValue), Convert.ToInt16(Sys_Ma_Nam_hoc), id_lop.Value, id_hoc_sinh, thang, Sys_Hoc_Ky, Convert.ToInt64(dt.Rows[9]["ID"]));
                        if (detail0 != null)
                        {
                            if (ngay == 1) detail0.NGAY1 = chbBUA0.Checked;
                            else if (ngay == 2) detail0.NGAY2 = chbBUA0.Checked;
                            else if (ngay == 3) detail0.NGAY3 = chbBUA0.Checked;
                            else if (ngay == 4) detail0.NGAY4 = chbBUA0.Checked;
                            else if (ngay == 5) detail0.NGAY5 = chbBUA0.Checked;
                            else if (ngay == 6) detail0.NGAY6 = chbBUA0.Checked;
                            else if (ngay == 7) detail0.NGAY7 = chbBUA0.Checked;
                            else if (ngay == 8) detail0.NGAY8 = chbBUA0.Checked;
                            else if (ngay == 9) detail0.NGAY9 = chbBUA0.Checked;
                            else if (ngay == 10) detail0.NGAY10 = chbBUA0.Checked;
                            else if (ngay == 11) detail0.NGAY11 = chbBUA0.Checked;
                            else if (ngay == 12) detail0.NGAY12 = chbBUA0.Checked;
                            else if (ngay == 13) detail0.NGAY13 = chbBUA0.Checked;
                            else if (ngay == 14) detail0.NGAY14 = chbBUA0.Checked;
                            else if (ngay == 15) detail0.NGAY15 = chbBUA0.Checked;
                            else if (ngay == 16) detail0.NGAY16 = chbBUA0.Checked;
                            else if (ngay == 17) detail0.NGAY17 = chbBUA0.Checked;
                            else if (ngay == 18) detail0.NGAY18 = chbBUA0.Checked;
                            else if (ngay == 19) detail0.NGAY19 = chbBUA0.Checked;
                            else if (ngay == 20) detail0.NGAY20 = chbBUA0.Checked;
                            else if (ngay == 21) detail0.NGAY21 = chbBUA0.Checked;
                            else if (ngay == 22) detail0.NGAY22 = chbBUA0.Checked;
                            else if (ngay == 23) detail0.NGAY23 = chbBUA0.Checked;
                            else if (ngay == 24) detail0.NGAY24 = chbBUA0.Checked;
                            else if (ngay == 25) detail0.NGAY25 = chbBUA0.Checked;
                            else if (ngay == 26) detail0.NGAY26 = chbBUA0.Checked;
                            else if (ngay == 27) detail0.NGAY27 = chbBUA0.Checked;
                            else if (ngay == 28) detail0.NGAY28 = chbBUA0.Checked;
                            else if (ngay == 29) detail0.NGAY29 = chbBUA0.Checked;
                            else if (ngay == 30) detail0.NGAY30 = chbBUA0.Checked;
                            else if (ngay == 31) detail0.NGAY31 = chbBUA0.Checked;
                            res = chamAnBO.update(detail0, Sys_User.ID);
                            if (res.Res)
                                success++;
                        }
                    }
                    #endregion
                }
            }
            string strMsg = "";
            if (success > 0)
            {
                strMsg += " notification('success', 'Cập nhật thành công!');";
            }
            else strMsg += " notification('warning', 'Không có bản ghi nào được lưu!');";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
        }
        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Header)
            {
                int ngay = Convert.ToDateTime(rdNgay.SelectedDate).Day;
                DataTable dt = new DataTable();
                if (Session["DanhMucBuaAn" + Sys_User.ID] != null) dt = (DataTable)Session["DanhMucBuaAn" + Sys_User.ID];
                int count = dt.Rows.Count;
                #region get control
                Literal ltrBua0 = (Literal)e.Item.FindControl("ltrBua0");
                Literal ltrBua1 = (Literal)e.Item.FindControl("ltrBua1");
                Literal ltrBua2 = (Literal)e.Item.FindControl("ltrBua2");
                Literal ltrBua3 = (Literal)e.Item.FindControl("ltrBua3");
                Literal ltrBua4 = (Literal)e.Item.FindControl("ltrBua4");
                Literal ltrBua5 = (Literal)e.Item.FindControl("ltrBua5");
                Literal ltrBua6 = (Literal)e.Item.FindControl("ltrBua6");
                Literal ltrBua7 = (Literal)e.Item.FindControl("ltrBua7");
                Literal ltrBua8 = (Literal)e.Item.FindControl("ltrBua8");
                Literal ltrBua9 = (Literal)e.Item.FindControl("ltrBua9");
                #endregion
                if (count > 0) ltrBua0.Text = dt.Rows[0]["TEN"].ToString();
                if (count > 1) ltrBua1.Text = dt.Rows[1]["TEN"].ToString();
                if (count > 2) ltrBua2.Text = dt.Rows[2]["TEN"].ToString();
                if (count > 3) ltrBua3.Text = dt.Rows[3]["TEN"].ToString();
                if (count > 4) ltrBua4.Text = dt.Rows[4]["TEN"].ToString();
                if (count > 5) ltrBua4.Text = dt.Rows[5]["TEN"].ToString();
                if (count > 6) ltrBua4.Text = dt.Rows[6]["TEN"].ToString();
                if (count > 7) ltrBua4.Text = dt.Rows[7]["TEN"].ToString();
                if (count > 8) ltrBua4.Text = dt.Rows[8]["TEN"].ToString();
                if (count > 9) ltrBua4.Text = dt.Rows[9]["TEN"].ToString();
            }
            else if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                #region get control
                CheckBox chbBUA0 = (CheckBox)item.FindControl("chbBUA0");
                CheckBox chbBUA1 = (CheckBox)item.FindControl("chbBUA1");
                CheckBox chbBUA2 = (CheckBox)item.FindControl("chbBUA2");
                CheckBox chbBUA3 = (CheckBox)item.FindControl("chbBUA3");
                CheckBox chbBUA4 = (CheckBox)item.FindControl("chbBUA4");
                CheckBox chbBUA5 = (CheckBox)item.FindControl("chbBUA5");
                CheckBox chbBUA6 = (CheckBox)item.FindControl("chbBUA6");
                CheckBox chbBUA7 = (CheckBox)item.FindControl("chbBUA7");
                CheckBox chbBUA8 = (CheckBox)item.FindControl("chbBUA8");
                CheckBox chbBUA9 = (CheckBox)item.FindControl("chbBUA9");

                HiddenField hdIS_BUA_AN_0 = (HiddenField)item.FindControl("hdIS_BUA_AN_0");
                HiddenField hdIS_BUA_AN_1 = (HiddenField)item.FindControl("hdIS_BUA_AN_1");
                HiddenField hdIS_BUA_AN_2 = (HiddenField)item.FindControl("hdIS_BUA_AN_2");
                HiddenField hdIS_BUA_AN_3 = (HiddenField)item.FindControl("hdIS_BUA_AN_3");
                HiddenField hdIS_BUA_AN_4 = (HiddenField)item.FindControl("hdIS_BUA_AN_4");
                HiddenField hdIS_BUA_AN_5 = (HiddenField)item.FindControl("hdIS_BUA_AN_5");
                HiddenField hdIS_BUA_AN_6 = (HiddenField)item.FindControl("hdIS_BUA_AN_6");
                HiddenField hdIS_BUA_AN_7 = (HiddenField)item.FindControl("hdIS_BUA_AN_7");
                HiddenField hdIS_BUA_AN_8 = (HiddenField)item.FindControl("hdIS_BUA_AN_8");
                HiddenField hdIS_BUA_AN_9 = (HiddenField)item.FindControl("hdIS_BUA_AN_9");

                HiddenField hdBUA_AN_0 = (HiddenField)item.FindControl("hdBUA_AN_0");
                HiddenField hdBUA_AN_1 = (HiddenField)item.FindControl("hdBUA_AN_1");
                HiddenField hdBUA_AN_2 = (HiddenField)item.FindControl("hdBUA_AN_2");
                HiddenField hdBUA_AN_3 = (HiddenField)item.FindControl("hdBUA_AN_3");
                HiddenField hdBUA_AN_4 = (HiddenField)item.FindControl("hdBUA_AN_4");
                HiddenField hdBUA_AN_5 = (HiddenField)item.FindControl("hdBUA_AN_5");
                HiddenField hdBUA_AN_6 = (HiddenField)item.FindControl("hdBUA_AN_6");
                HiddenField hdBUA_AN_7 = (HiddenField)item.FindControl("hdBUA_AN_7");
                HiddenField hdBUA_AN_8 = (HiddenField)item.FindControl("hdBUA_AN_8");
                HiddenField hdBUA_AN_9 = (HiddenField)item.FindControl("hdBUA_AN_9");
                #endregion

                chbBUA0.Enabled = hdIS_BUA_AN_0.Value == "1" ? true : false;
                chbBUA1.Enabled = hdIS_BUA_AN_1.Value == "1" ? true : false;
                chbBUA2.Enabled = hdIS_BUA_AN_2.Value == "1" ? true : false;
                chbBUA3.Enabled = hdIS_BUA_AN_3.Value == "1" ? true : false;
                chbBUA4.Enabled = hdIS_BUA_AN_4.Value == "1" ? true : false;
                chbBUA5.Enabled = hdIS_BUA_AN_5.Value == "1" ? true : false;
                chbBUA6.Enabled = hdIS_BUA_AN_6.Value == "1" ? true : false;
                chbBUA7.Enabled = hdIS_BUA_AN_7.Value == "1" ? true : false;
                chbBUA8.Enabled = hdIS_BUA_AN_8.Value == "1" ? true : false;
                chbBUA9.Enabled = hdIS_BUA_AN_9.Value == "1" ? true : false;

                chbBUA0.Checked = hdBUA_AN_0.Value == "1" ? true : false;
                chbBUA1.Checked = hdBUA_AN_1.Value == "1" ? true : false;
                chbBUA2.Checked = hdBUA_AN_2.Value == "1" ? true : false;
                chbBUA3.Checked = hdBUA_AN_3.Value == "1" ? true : false;
                chbBUA4.Checked = hdBUA_AN_4.Value == "1" ? true : false;
                chbBUA5.Checked = hdBUA_AN_5.Value == "1" ? true : false;
                chbBUA6.Checked = hdBUA_AN_6.Value == "1" ? true : false;
                chbBUA7.Checked = hdBUA_AN_7.Value == "1" ? true : false;
                chbBUA8.Checked = hdBUA_AN_8.Value == "1" ? true : false;
                chbBUA9.Checked = hdBUA_AN_9.Value == "1" ? true : false;
            }
        }
        protected void rdNgay_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            DateTime dtNgay = Convert.ToDateTime(rdNgay.SelectedDate);
            insertFirstData(dtNgay);
            if (dtNgay.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy")) btSave.Enabled = true;
            else btSave.Enabled = false;
            RadGrid1.Rebind();
        }
    }
}