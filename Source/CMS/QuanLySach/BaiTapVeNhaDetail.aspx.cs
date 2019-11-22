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

namespace CMS.QuanLySach
{
    public partial class BaiTapVeNhaDetail : AuthenticatePage
    {
        LocalAPI localAPI = new LocalAPI();
        BaiTapVeNhaBO btvnBO = new BaiTapVeNhaBO();
        BaiTapVeNhaChiTietBO btvnChiTietBO = new BaiTapVeNhaChiTietBO();
        DMSachBO dmSachBO = new DMSachBO();
        LogUserBO logUserBO = new LogUserBO();
        long? id_btvn;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("ID") != null)
            {
                try
                {
                    id_btvn = Convert.ToInt64(Request.QueryString.Get("ID"));
                }
                catch { }
            }
            if (!IsPostBack)
            {
                objKhoi.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                objKhoi.SelectParameters.Add("maLoaiLopGDTX", DbType.Int16, Sys_This_Lop_GDTX == null ? "" : Sys_This_Lop_GDTX.ToString());
                rcbKhoiHoc.DataBind();
                objLop.SelectParameters.Add("ma_cap_hoc", Sys_This_Cap_Hoc);
                objLop.SelectParameters.Add("idTruong", Sys_This_Truong.ID.ToString());
                objLop.SelectParameters.Add("maNamHoc", Sys_Ma_Nam_hoc.ToString());
                rcbLop.DataBind();
                objMonHoc.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                rcbMonHoc.DataBind();
                rcbSach.DataBind();
                rdNgay.SelectedDate = DateTime.Now;
                BAI_TAP_VE_NHA detail = new BAI_TAP_VE_NHA();
                if (id_btvn != null)
                {
                    detail = btvnBO.getBaiTapVeNhaByID(id_btvn.Value);
                    if (detail != null)
                    {
                        rcbKhoiHoc.SelectedValue = detail.ID_KHOI.ToString();
                        rcbLop.SelectedValue = detail.ID_LOP.ToString();
                        tbNoiDung.Text = detail.NOI_DUNG;
                        try
                        {
                            rdNgay.SelectedDate = detail.NGAY_BTVN;
                        }
                        catch { rdNgay.SelectedDate = DateTime.Now; }
                        #region add chi tiết phiếu vào session
                        createSession();
                        List<BAI_TAP_VE_NHA_CHI_TIET> lstDetail = btvnChiTietBO.getChiTietByBaiTapID(Sys_This_Truong.ID, id_btvn.Value);
                        DataTable dt = (DataTable)Session["GiaoBTVN" + Sys_User.ID];
                        if (lstDetail.Count > 0)
                        {
                            for (int i = 0; i < lstDetail.Count; i++)
                            {
                                DataRow drow = dt.NewRow();
                                drow["ID_MON_HOC"] = lstDetail[i].ID_MON_HOC;
                                drow["ID_SACH"] = lstDetail[i].ID_SACH;
                                drow["BAI_SO"] = lstDetail[i].BAI_SO;
                                drow["TRANG_SO"] = lstDetail[i].TRANG_SO;
                                dt.Rows.Add(drow);
                            }
                        }
                        Session["GiaoBTVN" + Sys_User.ID] = dt;
                        #endregion
                        btEdit.Visible = is_access(SYS_Type_Access.SUA);
                        btAdd.Visible = false;
                    }
                    else
                    {
                        btEdit.Visible = false;
                        btAdd.Visible = is_access(SYS_Type_Access.THEM);
                    }
                }
                else
                {
                    btEdit.Visible = false;
                    btAdd.Visible = is_access(SYS_Type_Access.SUA);
                }
            }
            if (id_btvn != null && id_btvn > 0)
            {
                btAdd.Visible = false;
                btEdit.Visible = true;
            }
            else
            {
                btAdd.Visible = true;
                btEdit.Visible = false;
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            List<BAI_TAP_VE_NHA_CHI_TIET> lst = new List<BAI_TAP_VE_NHA_CHI_TIET>();
            if (id_btvn != null && Session["GiaoBTVN" + Sys_User.ID] == null)
            {
                lst = btvnChiTietBO.getChiTietByBaiTapID(Sys_This_Truong.ID, id_btvn.Value);
            }
            if (Session["GiaoBTVN" + Sys_User.ID] == null) createSession();
            DataTable dt = new DataTable();
            dt = (DataTable)Session["GiaoBTVN" + Sys_User.ID];
            if (lst.Count > 0)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    DataRow drow = dt.NewRow();
                    drow["ID_MON_HOC"] = lst[i].ID_MON_HOC;
                    drow["ID_SACH"] = lst[i].ID_SACH;
                    drow["BAI_SO"] = lst[i].BAI_SO;
                    drow["TRANG_SO"] = lst[i].TRANG_SO;
                    dt.Rows.Add(drow);
                }
            }
            Session["GiaoBTVN" + Sys_User.ID] = dt;
            RadGrid1.DataSource = dt;
        }
        protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                long? id_sach = localAPI.ConvertStringTolong(item["ID_SACH"].Text);
                if (id_sach != null)
                {
                    DM_SACH dmSach = dmSachBO.getDMSachByID(id_sach.Value);
                    if (dmSach != null) item["TEN_SACH"].Text = dmSach.TEN;
                    else item["TEN_SACH"].Text = "";
                }
                else item["TEN_SACH"].Text = "";
                short? bai_so = localAPI.ConvertStringToShort(item["BAI_SO"].Text);
                short? trang_so = localAPI.ConvertStringToShort(item["TRANG_SO"].Text);
                item["TEN_BAI"].Text = (bai_so == null ? "" : "Bài số " + bai_so + " ") + (trang_so == null ? "" : "trang " + trang_so);
            }
        }
        protected void btDeleteChon_Click(object sender, EventArgs e)
        {
            int success = 0, error = 0;
            ResultEntity res = new ResultEntity();
            DataTable dt = new DataTable();
            if (Session["GiaoBTVN" + Sys_User.ID] != null) dt = (DataTable)Session["GiaoBTVN" + Sys_User.ID];
            if (id_btvn != null)
            {
                foreach (GridDataItem row in RadGrid1.SelectedItems)
                {
                    short id_mon_hoc = Convert.ToInt16(row["ID_MON_HOC"].Text);
                    long id_sach = Convert.ToInt64(row["ID_SACH"].Text);
                    short bai_so = Convert.ToInt16(row["BAI_SO"].Text);
                    short trang_so = Convert.ToInt16(row["TRANG_SO"].Text);
                    res = btvnChiTietBO.deleteChiTietBySoBai(Sys_This_Truong.ID, id_btvn.Value, id_mon_hoc, id_sach, bai_so, trang_so);
                    if (res.Res)
                    {
                        success++;
                        logUserBO.insert(Sys_This_Truong.ID, "DELETE", "Xóa nội dung bài tập về nhà", Sys_User.ID, DateTime.Now);
                    }
                    else error++;

                    #region xóa trong session
                    int count = dt.Rows.Count;
                    for (int i = 0; i < count; i++)
                    {
                        DataRow drow = dt.Rows[i];
                        if (Convert.ToInt64(drow["ID_SACH"]) == id_sach && Convert.ToInt16(drow["BAI_SO"]) == bai_so && Convert.ToInt16(drow["TRANG_SO"]) == trang_so)
                        {
                            dt.Rows.Remove(drow);
                            break;
                        }
                    }
                    #endregion
                }
            }
            else
            {
                foreach (GridDataItem row in RadGrid1.SelectedItems)
                {
                    short id_mon_hoc = Convert.ToInt16(row["ID_MON_HOC"].Text);
                    long id_sach = Convert.ToInt64(row["ID_SACH"].Text);
                    short bai_so = Convert.ToInt16(row["BAI_SO"].Text);
                    short trang_so = Convert.ToInt16(row["TRANG_SO"].Text);
                    int count = dt.Rows.Count;
                    for (int i = 0; i < count; i++)
                    {
                        DataRow drow = dt.Rows[i];
                        if (Convert.ToInt64(drow["ID_SACH"]) == id_sach && Convert.ToInt16(drow["BAI_SO"]) == bai_so && Convert.ToInt16(drow["TRANG_SO"]) == trang_so)
                        {
                            dt.Rows.Remove(drow);
                            break;
                        }
                    }
                }
            }

            Session["GiaoBTVN" + Sys_User.ID] = dt;
            string strMsg = "";
            if (success > 0)
            {
                strMsg = "notification('success', 'Có " + success + " bản ghi được xóa');";
            }
            if (error > 0) strMsg = "notification('error', 'Có " + error + " bản ghi không được xóa');";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
        }
        protected void btAdd_Click(object sender, EventArgs e)
        {
            #region check value
            if (!is_access(SYS_Type_Access.THEM))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            if (string.IsNullOrEmpty(rcbLop.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn chưa chọn lớp học!');", true);
                return;
            }
            if (string.IsNullOrEmpty(tbNoiDung.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn phải nhập nội dung bài tập về nhà!');", true);
                return;
            }
            #endregion
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            #region Thêm chi tiết bài tập
            //if (RadGrid1.Items.Count > 0)
            //{
                BAI_TAP_VE_NHA detail = new BAI_TAP_VE_NHA();
                detail.ID_TRUONG = Sys_This_Truong.ID;
                detail.MA_CAP_HOC = Sys_This_Cap_Hoc;
                detail.ID_KHOI = Convert.ToInt16(rcbKhoiHoc.SelectedValue);
                detail.ID_NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
                detail.ID_LOP = Convert.ToInt64(rcbLop.SelectedValue);
                detail.NOI_DUNG = tbNoiDung.Text.Trim();
                try
                {
                    detail.NGAY_BTVN = Convert.ToDateTime(rdNgay.SelectedDate);
                }
                catch
                {
                    detail.NGAY_BTVN = DateTime.Now;
                }
                res = btvnBO.insert(detail, Sys_User.ID);
                if (res.Res)
                {
                    BAI_TAP_VE_NHA resBTVN = (BAI_TAP_VE_NHA)res.ResObject;
                    id_btvn = resBTVN.ID;
                    #region insert detail
                    Session["GiaoBTVN" + Sys_User.ID] = null;
                    createSession();
                    DataTable dt = (DataTable)Session["GiaoBTVN" + Sys_User.ID];
                    foreach (GridDataItem row in RadGrid1.Items)
                    {
                        short id_mon_hoc = Convert.ToInt16(row["ID_MON_HOC"].Text);
                        long id_sach = Convert.ToInt64(row["ID_SACH"].Text);
                        short bai_so = Convert.ToInt16(row["BAI_SO"].Text);
                        short trang_so = Convert.ToInt16(row["TRANG_SO"].Text);

                        BAI_TAP_VE_NHA_CHI_TIET btDetail = btvnChiTietBO.getChiTietBySachMonBaiTrang(Sys_This_Truong.ID, id_btvn.Value, id_mon_hoc, id_sach, bai_so, trang_so);
                        if (btDetail != null)
                        {
                            btDetail.ID_MON_HOC = id_mon_hoc;
                            btDetail.ID_SACH = id_sach;
                            btDetail.BAI_SO = bai_so;
                            btDetail.TRANG_SO = trang_so;
                            res = btvnChiTietBO.update(btDetail, Sys_User.ID);
                        }
                        else
                        {
                            btDetail = new BAI_TAP_VE_NHA_CHI_TIET();
                            btDetail.ID_TRUONG = Sys_This_Truong.ID;
                            btDetail.MA_CAP_HOC = Sys_This_Cap_Hoc;
                            btDetail.ID_BTVN = id_btvn.Value;
                            btDetail.ID_MON_HOC = id_mon_hoc;
                            btDetail.ID_SACH = id_sach;
                            btDetail.BAI_SO = bai_so;
                            btDetail.TRANG_SO = trang_so;
                            res = btvnChiTietBO.insert(btDetail, Sys_User.ID);
                        }
                        #region set Session
                        DataRow drow = dt.NewRow();
                        drow["ID_MON_HOC"] = id_mon_hoc;
                        drow["ID_SACH"] = id_sach;
                        drow["BAI_SO"] = bai_so;
                        drow["TRANG_SO"] = trang_so;
                        dt.Rows.Add(drow);
                        #endregion
                    }
                    Session["GiaoBTVN" + Sys_User.ID] = dt;
                    #endregion
                }
            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn vui lòng nhập chi tiết bài tập giao về nhà!');", true);
            //    return;
            //}
            #endregion
            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
                btEdit.Visible = is_access(SYS_Type_Access.SUA);
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
        }
        protected void btEdit_Click(object sender, EventArgs e)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            int success = 0;
            string strMsg = "";
            if (id_btvn != null)
            {
                BAI_TAP_VE_NHA detail = btvnBO.getBaiTapVeNhaByID(id_btvn.Value);
                if (detail != null)
                {
                    detail.ID_KHOI = Convert.ToInt16(rcbKhoiHoc.SelectedValue);
                    detail.ID_LOP = Convert.ToInt64(rcbLop.SelectedValue);
                    detail.ID_NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
                    detail.NOI_DUNG = tbNoiDung.Text.Trim();
                    detail.NGAY_BTVN = Convert.ToDateTime(rdNgay.SelectedDate);

                    #region chi tiết
                    
                    Session["GiaoBTVN" + Sys_User.ID] = null;
                    createSession();
                    DataTable dt = (DataTable)Session["GiaoBTVN" + Sys_User.ID];
                    foreach (GridDataItem row in RadGrid1.Items)
                    {
                        short id_mon_hoc = Convert.ToInt16(row["ID_MON_HOC"].Text);
                        long id_sach = Convert.ToInt64(row["ID_SACH"].Text);
                        short bai_so = Convert.ToInt16(row["BAI_SO"].Text);
                        short trang_so = Convert.ToInt16(row["TRANG_SO"].Text);

                        BAI_TAP_VE_NHA_CHI_TIET btDetail = btvnChiTietBO.getChiTietBySachMonBaiTrang(Sys_This_Truong.ID, id_btvn.Value, id_mon_hoc, id_sach, bai_so, trang_so);
                        if (btDetail != null)
                        {
                            btDetail.ID_MON_HOC = id_mon_hoc;
                            btDetail.ID_SACH = id_sach;
                            btDetail.BAI_SO = bai_so;
                            btDetail.TRANG_SO = trang_so;
                            res = btvnChiTietBO.update(btDetail, Sys_User.ID);
                        }
                        else
                        {
                            btDetail = new BAI_TAP_VE_NHA_CHI_TIET();
                            btDetail.ID_TRUONG = Sys_This_Truong.ID;
                            btDetail.MA_CAP_HOC = Sys_This_Cap_Hoc;
                            btDetail.ID_BTVN = id_btvn.Value;
                            btDetail.ID_MON_HOC = id_mon_hoc;
                            btDetail.ID_SACH = id_sach;
                            btDetail.BAI_SO = bai_so;
                            btDetail.TRANG_SO = trang_so;
                            res = btvnChiTietBO.insert(btDetail, Sys_User.ID);
                        }
                        #region set Session
                        DataRow drow = dt.NewRow();
                        drow["ID_MON_HOC"] = id_mon_hoc;
                        drow["ID_SACH"] = id_sach;
                        drow["BAI_SO"] = bai_so;
                        drow["TRANG_SO"] = trang_so;
                        dt.Rows.Add(drow);
                        #endregion
                    }
                    Session["GiaoBTVN" + Sys_User.ID] = dt;
                    #endregion
                    res = btvnBO.update(detail, Sys_User.ID);
                    if (res.Res)
                    {
                        success++;
                        logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật nội dung bài tập về nhà", Sys_User.ID, DateTime.Now);
                    }
                }
            }
            if (success > 0)
            {
                strMsg = " notification('success', 'Cập nhật thành công.');";
                btAdd.Visible = false;
            }
            else
            {
                strMsg = " notification('error', 'Có lỗi xảy ra');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
        }
        protected void createSession()
        {
            DataTable dt = new DataTable();
            if (Session["GiaoBTVN" + Sys_User.ID] != null) dt = (DataTable)Session["GiaoBTVN" + Sys_User.ID];
            else
            {
                dt.Columns.Add(new DataColumn("ID_MON_HOC"));
                dt.Columns.Add(new DataColumn("ID_SACH"));
                dt.Columns.Add(new DataColumn("BAI_SO"));
                dt.Columns.Add(new DataColumn("TRANG_SO"));
            }
            Session["GiaoBTVN" + Sys_User.ID] = dt;
        }
        protected void rcbKhoiHoc_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbLop.ClearSelection();
            rcbLop.Text = string.Empty;
            rcbLop.DataBind();
            rcbMonHoc.ClearSelection();
            rcbMonHoc.Text = string.Empty;
            rcbMonHoc.DataBind();
            RadGrid1.Rebind();
        }
        protected void rcbLop_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbMonHoc.ClearSelection();
            rcbMonHoc.Text = string.Empty;
            rcbMonHoc.DataBind();
            RadGrid1.Rebind();
        }
        protected void rcbMonHoc_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void btThem_Click(object sender, EventArgs e)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            int success = 0;
            string strMsg = "";
            #region check value"
            if (string.IsNullOrEmpty(rcbMonHoc.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Bạn phải chọn môn học!');", true);
                return;
            }
            if (string.IsNullOrEmpty(rcbSach.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Bạn phải chọn sách!');", true);
                return;
            }
            if (string.IsNullOrEmpty(tbBaiSo.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Bạn phải nhập số tên bài!');", true);
                return;
            }
            if (string.IsNullOrEmpty(tbTrangSo.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Bạn phải nhập số trang!');", true);
                return;
            }
            #endregion
            createSession();
            short id_mon_hoc = Convert.ToInt16(rcbMonHoc.SelectedValue);
            long id_sach = Convert.ToInt64(rcbSach.SelectedValue);
            short bai_so = Convert.ToInt16(tbBaiSo.Text.Trim());
            short trang_so = Convert.ToInt16(tbTrangSo.Text.Trim());
            #region check nếu thực phẩm đã tồn tại thì ko được thêm vào
            DataTable dt = (DataTable)Session["GiaoBTVN" + Sys_User.ID];
            int count_trung = 0;
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["ID_MON_HOC"].ToString() == rcbMonHoc.SelectedValue &&
                        dt.Rows[i]["ID_SACH"].ToString() == rcbSach.SelectedValue &&
                        dt.Rows[i]["BAI_SO"].ToString() == bai_so.ToString() &&
                        dt.Rows[i]["TRANG_SO"].ToString() == trang_so.ToString())
                    {
                        count_trung++;
                        break;
                    }
                }
            }
            #endregion
            if (count_trung == 0)
            {
                DataRow drow = dt.NewRow();
                drow["ID_MON_HOC"] = rcbMonHoc.SelectedValue;
                drow["ID_SACH"] = rcbSach.SelectedValue;
                drow["BAI_SO"] = bai_so;
                drow["TRANG_SO"] = trang_so;
                dt.Rows.Add(drow);
                success++;
                Session["GiaoBTVN" + Sys_User.ID] = dt;
                #region insert vào chi tiết
                if (id_btvn != null)
                {
                    BAI_TAP_VE_NHA_CHI_TIET detail = new BAI_TAP_VE_NHA_CHI_TIET();
                    detail = btvnChiTietBO.getChiTietBySachMonBaiTrang(Sys_This_Truong.ID, id_btvn.Value, Convert.ToInt16(rcbMonHoc.SelectedValue), Convert.ToInt64(rcbSach.SelectedValue), bai_so, trang_so);
                    if (detail == null)
                    {
                        detail = new BAI_TAP_VE_NHA_CHI_TIET();
                        detail.ID_TRUONG = Sys_This_Truong.ID;
                        detail.MA_CAP_HOC = Sys_This_Cap_Hoc;
                        detail.ID_MON_HOC = Convert.ToInt16(rcbMonHoc.SelectedValue);
                        detail.ID_SACH = Convert.ToInt64(rcbSach.SelectedValue);
                        detail.BAI_SO = bai_so;
                        detail.TRANG_SO = trang_so;
                        res = btvnChiTietBO.insert(detail, Sys_User.ID);
                        if (res.Res)
                        {
                            logUserBO.insert(Sys_This_Truong.ID, "INSERT", "Insert nội dung bài tập về nhà", Sys_User.ID, DateTime.Now);
                        }
                    }
                }
                #endregion
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Bài tập này đã tồn tại. Vui lòng kiểm tra lại!');", true);
                return;
            }

            if (success > 0)
            {
                strMsg = " notification('success', 'Cập nhật thành công.');";
            }
            else
            {
                strMsg = " notification('error', 'Có lỗi xảy ra');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
        }
    }
}