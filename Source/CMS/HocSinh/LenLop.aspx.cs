using CMS.XuLy;
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
    public partial class LenLop : AuthenticatePage
    {
        LocalAPI localAPI = new LocalAPI();
        LopBO lopBO = new LopBO();
        HocSinhBO hocSinhBO = new HocSinhBO();
        LogUserBO logUserBO = new LogUserBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            if (!IsPostBack)
            {
                rcbNamHoc1.DataBind();
                //rcbNamHoc1.SelectedValue = Sys_Ma_Nam_hoc.ToString();
                rcbNamHoc1.SelectedValue = (Sys_Ma_Nam_hoc - 1).ToString();
                objLop1.SelectParameters.Add("ma_cap_hoc", Sys_This_Cap_Hoc);
                objLop1.SelectParameters.Add("idTruong", Sys_This_Truong.ID.ToString());
                rcbLop1.DataBind();

                rcbNamHoc2.DataBind();
                //rcbNamHoc2.SelectedValue = (Sys_Ma_Nam_hoc + 1).ToString();
                rcbNamHoc2.SelectedValue = Sys_Ma_Nam_hoc.ToString();
                objLop2.SelectParameters.Add("ma_cap_hoc", Sys_This_Cap_Hoc);
                objLop2.SelectParameters.Add("idTruong", Sys_This_Truong.ID.ToString());
                rcbLop2.DataBind();
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            short? id_nam_hoc = localAPI.ConvertStringToShort(rcbNamHoc1.SelectedValue);
            long? id_lop = localAPI.ConvertStringTolong(rcbLop1.SelectedValue);
            if (id_nam_hoc != null && id_lop != null)
                RadGrid1.DataSource = hocSinhBO.getHocSinhByTruongLopNamHoc(Sys_This_Truong.ID, null, id_lop.Value, id_nam_hoc.Value);
            else RadGrid1.DataSource = "";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
        protected void RadGrid2_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            short? id_nam_hoc = localAPI.ConvertStringToShort(rcbNamHoc2.SelectedValue);
            long? id_lop = localAPI.ConvertStringTolong(rcbLop2.SelectedValue);
            if (id_nam_hoc != null && id_lop != null)
                RadGrid2.DataSource = hocSinhBO.getHocSinhByTruongLopNamHoc(Sys_This_Truong.ID, null, id_lop.Value, id_nam_hoc.Value);
            else RadGrid2.DataSource = "";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
        protected void rcbNamHoc1_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbLop1.ClearSelection();
            rcbLop1.Text = String.Empty;
            rcbLop1.DataBind();

            rcbNamHoc2.ClearSelection();
            rcbNamHoc2.Text = String.Empty;
            rcbNamHoc2.DataBind();

            rcbLop2.ClearSelection();
            rcbLop2.Text = String.Empty;
            rcbLop2.DataBind();

            RadGrid1.Rebind();
            RadGrid2.Rebind();
        }
        protected void rcbNamHoc2_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbLop2.ClearSelection();
            rcbLop2.Text = String.Empty;
            rcbLop2.DataBind();
            RadGrid2.Rebind();
        }
        protected void rcbLop1_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void rcbLop2_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid2.Rebind();
        }
        protected void btnChuyenDi_Click(object sender, ImageClickEventArgs e)
        {
            if (Sys_User.IS_ROOT != true)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            int success = 0, error = 0;
            int id_nam_cu = Convert.ToInt16(rcbNamHoc1.SelectedValue);
            int id_nam_moi = Convert.ToInt16(rcbNamHoc2.SelectedValue);
            if (id_nam_moi < id_nam_cu)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Năm học mới phải lớn hơn năm học cũ!');", true);
                return;
            }
            long? id_lop_cu = localAPI.ConvertStringTolong(rcbLop1.SelectedValue);
            long? id_lop_moi = localAPI.ConvertStringTolong(rcbLop2.SelectedValue);
            if (id_lop_cu != null && id_lop_moi != null)
            {
                LOP lopMoi = lopBO.getLopById(id_lop_moi.Value);
                short idKhoiMoi = lopMoi.ID_KHOI;
                if (cbLenLopAll.Checked)
                {
                    ResultEntity res = hocSinhBO.ChuyenHocSinhLenLopMoi(Sys_This_Truong.ID, Sys_This_Cap_Hoc, id_nam_cu, id_nam_moi, id_lop_cu.Value, id_lop_moi.Value, idKhoiMoi, Sys_User.ID);
                    if (res.Res)
                    {
                        success++;
                        logUserBO.insert(Sys_This_Truong.ID, "LÊN LỚP", "Chuyển học sinh từ lớp " + id_lop_cu.Value + " lên lớp " + id_lop_moi.Value, Sys_User.ID, DateTime.Now);
                    }
                    else
                        error++;
                }
                else
                {
                    List<string> lstMaHS = new List<string>();
                    List<HOC_SINH> lstExist = hocSinhBO.getHocSinhByTruongLopNamHoc(Sys_This_Truong.ID, null, id_lop_moi.Value, Convert.ToInt16(id_nam_moi));
                    for (int i = 0; i < lstExist.Count; i++)
                    {
                        lstMaHS.Add(lstExist[i].MA);
                    }

                    List<HOC_SINH> lstHocSinh = new List<HOC_SINH>();
                    foreach (GridDataItem row in RadGrid1.SelectedItems)
                    {
                        long id_hs = Convert.ToInt64(row.GetDataKeyValue("ID").ToString());
                        HOC_SINH detail = hocSinhBO.getHocSinhByID(id_hs);
                        if (detail != null)
                        {
                            if (lstMaHS.Contains(detail.MA)) continue;
                            HOC_SINH hsInsert = new HOC_SINH();
                            hsInsert.ID_TRUONG = Sys_This_Truong.ID;
                            hsInsert.ID_KHOI = idKhoiMoi;
                            hsInsert.ID_LOP = id_lop_moi.Value;
                            hsInsert.ID_NAM_HOC = Convert.ToInt16(rcbNamHoc2.SelectedValue);
                            hsInsert.MA_CAP_HOC = Sys_This_Cap_Hoc;
                            hsInsert.MA = detail.MA;
                            hsInsert.TEN = detail.TEN;
                            hsInsert.HO_TEN = detail.HO_TEN;
                            hsInsert.HO_DEM = detail.HO_DEM;
                            hsInsert.NGAY_SINH = detail.NGAY_SINH;
                            hsInsert.MA_GIOI_TINH = detail.MA_GIOI_TINH;
                            hsInsert.IS_GUI_BO_ME = detail.IS_GUI_BO_ME;
                            hsInsert.SDT_NHAN_TIN = detail.SDT_NHAN_TIN;
                            hsInsert.SDT_NHAN_TIN2 = detail.SDT_NHAN_TIN2;
                            hsInsert.TRANG_THAI_HOC = detail.TRANG_THAI_HOC;
                            hsInsert.IS_DK_KY1 = detail.IS_DK_KY1;
                            hsInsert.IS_DK_KY2 = detail.IS_DK_KY2;
                            hsInsert.IS_CON_GV = detail.IS_CON_GV;
                            hsInsert.THU_TU = detail.THU_TU;
                            hsInsert.DIA_CHI = detail.DIA_CHI;
                            lstHocSinh.Add(hsInsert);
                        }
                    }
                    if (lstHocSinh.Count > 0)
                    {
                        ResultEntity res = hocSinhBO.ChuyenLenLopTheoHocSinh(lstHocSinh, Sys_User.ID);
                        if (res.Res)
                        {
                            success++;
                            logUserBO.insert(Sys_This_Truong.ID, "LÊN LỚP", "Chuyển học sinh từ lớp " + id_lop_cu.Value + " lên lớp " + id_lop_moi.Value, Sys_User.ID, DateTime.Now);
                        }
                        else
                            error++;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Không có học sinh nào thỏa mãn!');", true);
                        return;
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Lớp học không được để trống!');", true);
                return;
            }

            string strMsg = "";
            if (error > 0)
            {
                strMsg = "notification('error', 'Có lỗi xảy ra');";
            }
            if (success > 0)
            {
                strMsg += " notification('success', 'Cập nhật thành công');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
            RadGrid2.Rebind();
        }
        protected void btnXoa_Click(object sender, ImageClickEventArgs e)
        {
            if (Sys_User.IS_ROOT != true)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            int success = 0, error = 0;
            DiemChiTietBO diemChiTietBO = new DiemChiTietBO();
            DiemTongKetBO diemTongKetBO = new DiemTongKetBO();
            foreach (GridDataItem row in RadGrid2.SelectedItems)
            {
                long id_hs = Convert.ToInt64(row.GetDataKeyValue("ID").ToString());
                HOC_SINH detail = hocSinhBO.getHocSinhByID(id_hs);
                if (detail != null)
                {
                    List<DIEM_CHI_TIET> checkExistsDiemChiTietHocSinh = diemChiTietBO.getDiemChiTietByTruongKhoiLopAndHocSinhAndCapAndHocKy(Convert.ToInt16(Sys_Ma_Nam_hoc), Sys_This_Truong.ID, detail.ID_KHOI, detail.ID_LOP, Convert.ToInt16(Sys_Hoc_Ky), id_hs, Sys_This_Cap_Hoc);
                    if (checkExistsDiemChiTietHocSinh != null && checkExistsDiemChiTietHocSinh.Count > 0)
                    {
                        error++;
                        continue;
                    }
                    DIEM_TONG_KET checkExistsDiemTongKetHocSinh = diemTongKetBO.getDiemTrungBinhByHocSinh(Sys_This_Truong.ID, Convert.ToInt16(Sys_Ma_Nam_hoc), detail.ID_LOP, id_hs);
                    if (checkExistsDiemTongKetHocSinh != null && checkExistsDiemTongKetHocSinh.ID > 0)
                    {
                        error++;
                        continue;
                    }
                    ResultEntity res = hocSinhBO.delete(id_hs, Sys_User.ID, false, false);
                    if (res.Res)
                    {
                        success++;
                        logUserBO.insert(Sys_This_Truong.ID, "DELETE", "Xóa học sinh " + id_hs + " lớp " + detail.ID_LOP, Sys_User.ID, DateTime.Now);
                    }
                    else
                        error++;
                }
            }
            if (RadGrid2.SelectedItems.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Không có học sinh nào được chọn!');", true);
                return;
            }
            string strMsg = "";
            if (error > 0)
            {
                strMsg = "notification('error', 'Có lỗi xảy ra');";
            }
            if (success > 0)
            {
                strMsg += " notification('success', 'Cập nhật thành công');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid2.Rebind();
        }
    }
}