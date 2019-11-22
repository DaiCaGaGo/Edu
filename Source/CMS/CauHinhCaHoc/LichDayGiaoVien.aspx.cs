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
    public partial class LichDayGiaoVien : AuthenticatePage
    {
        CaHocBO caHocBO = new CaHocBO();
        LocalAPI localAPI = new LocalAPI();
        LopMonBO lopMonBO = new LopMonBO();
        LopBO lopBO = new LopBO();
        MonHocTruongBO monTruongBO = new MonHocTruongBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            btExport.Visible = is_access(SYS_Type_Access.EXPORT);
            if (!IsPostBack)
            {
                objGVCN.SelectParameters.Add("id", Sys_This_Truong.ID.ToString());
                rcbGiaoVien.DataBind();
                rcbHocKy.DataBind();
                rcbHocKy.SelectedValue = Sys_Hoc_Ky.ToString();
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            List<CaHocEntity> lstCaHoc = caHocBO.getCaHocByGiaoVienTruong(Sys_This_Truong.ID, Convert.ToInt16(Sys_Ma_Nam_hoc), Convert.ToInt16(rcbHocKy.SelectedValue), localAPI.ConvertStringTolong(rcbGiaoVien.SelectedValue));
            List<LichDayEntity> lstLichDay = new List<LichDayEntity>();
            List<LopMonTruongEntity> lstLopMon = lopMonBO.getLopMonByGiaoVienNamHoc(Sys_This_Truong.ID, Convert.ToInt16(Sys_Ma_Nam_hoc), Convert.ToInt16(rcbHocKy.SelectedValue), localAPI.ConvertStringTolong(rcbGiaoVien.SelectedValue));
            LopMonTruongEntity lopMon2 = new LopMonTruongEntity();
            LopMonTruongEntity lopMon3 = new LopMonTruongEntity();
            LopMonTruongEntity lopMon4 = new LopMonTruongEntity();
            LopMonTruongEntity lopMon5 = new LopMonTruongEntity();
            LopMonTruongEntity lopMon6 = new LopMonTruongEntity();
            LopMonTruongEntity lopMon7 = new LopMonTruongEntity();
            LopMonTruongEntity lopMon8 = new LopMonTruongEntity();
            #region "tiet 1"
            List<CaHocEntity> lstTiet1 = lstCaHoc.Where(x => x.TIET == 1).ToList();
            if (lstCaHoc != null && lstCaHoc.Count > 0)
            {
                LichDayEntity lichDay = new LichDayEntity();
                lichDay.TIET = 1;
                for (int i = 0; i < lstTiet1.Count; i++)
                {
                    lopMon2 = lstLopMon.Where(x => x.ID_LOP == lstTiet1[i].ID_LOP && x.ID_MON_TRUONG == lstTiet1[i].ID_MON_2 && x.HOC_KY == lstTiet1[i].ID_HOC_KY).FirstOrDefault();
                    lopMon3 = lstLopMon.Where(x => x.ID_LOP == lstTiet1[i].ID_LOP && x.ID_MON_TRUONG == lstTiet1[i].ID_MON_3 && x.HOC_KY == lstTiet1[i].ID_HOC_KY).FirstOrDefault();
                    lopMon4 = lstLopMon.Where(x => x.ID_LOP == lstTiet1[i].ID_LOP && x.ID_MON_TRUONG == lstTiet1[i].ID_MON_4 && x.HOC_KY == lstTiet1[i].ID_HOC_KY).FirstOrDefault();
                    lopMon5 = lstLopMon.Where(x => x.ID_LOP == lstTiet1[i].ID_LOP && x.ID_MON_TRUONG == lstTiet1[i].ID_MON_5 && x.HOC_KY == lstTiet1[i].ID_HOC_KY).FirstOrDefault();
                    lopMon6 = lstLopMon.Where(x => x.ID_LOP == lstTiet1[i].ID_LOP && x.ID_MON_TRUONG == lstTiet1[i].ID_MON_6 && x.HOC_KY == lstTiet1[i].ID_HOC_KY).FirstOrDefault();
                    lopMon7 = lstLopMon.Where(x => x.ID_LOP == lstTiet1[i].ID_LOP && x.ID_MON_TRUONG == lstTiet1[i].ID_MON_7 && x.HOC_KY == lstTiet1[i].ID_HOC_KY).FirstOrDefault();
                    lopMon8 = lstLopMon.Where(x => x.ID_LOP == lstTiet1[i].ID_LOP && x.ID_MON_TRUONG == lstTiet1[i].ID_MON_8 && x.HOC_KY == lstTiet1[i].ID_HOC_KY).FirstOrDefault();
                    if (lopMon2 != null) lichDay.THU2 += lopMon2.TEN_LOP + " - " + lopMon2.TEN_MON_TRUONG + ", ";
                    if (lopMon3 != null) lichDay.THU3 += lopMon3.TEN_LOP + " - " + lopMon3.TEN_MON_TRUONG + ", ";
                    if (lopMon4 != null) lichDay.THU4 += lopMon4.TEN_LOP + " - " + lopMon4.TEN_MON_TRUONG + ", ";
                    if (lopMon5 != null) lichDay.THU5 += lopMon5.TEN_LOP + " - " + lopMon5.TEN_MON_TRUONG + ", ";
                    if (lopMon6 != null) lichDay.THU6 += lopMon6.TEN_LOP + " - " + lopMon6.TEN_MON_TRUONG + ", ";
                    if (lopMon7 != null) lichDay.THU7 += lopMon7.TEN_LOP + " - " + lopMon7.TEN_MON_TRUONG + ", ";
                    if (lopMon8 != null) lichDay.THU8 += lopMon8.TEN_LOP + " - " + lopMon8.TEN_MON_TRUONG + ", ";
                }
                lichDay.THU2 = lichDay.THU2 == null ? "" : lichDay.THU2.Trim().TrimEnd(',');
                lichDay.THU3 = lichDay.THU3 == null ? "" : lichDay.THU3.Trim().TrimEnd(',');
                lichDay.THU4 = lichDay.THU4 == null ? "" : lichDay.THU4.Trim().TrimEnd(',');
                lichDay.THU5 = lichDay.THU5 == null ? "" : lichDay.THU5.Trim().TrimEnd(',');
                lichDay.THU6 = lichDay.THU6 == null ? "" : lichDay.THU6.Trim().TrimEnd(',');
                lichDay.THU7 = lichDay.THU7 == null ? "" : lichDay.THU7.Trim().TrimEnd(',');
                lichDay.THU8 = lichDay.THU8 == null ? "" : lichDay.THU8.Trim().TrimEnd(',');
                lstLichDay.Add(lichDay);
            }
            #endregion
            #region "tiet 2"
            List<CaHocEntity> lstTiet2 = lstCaHoc.Where(x => x.TIET == 2).ToList();
            if (lstCaHoc != null && lstCaHoc.Count > 0)
            {
                LichDayEntity lichDay = new LichDayEntity();
                lichDay.TIET = 2;
                for (int i = 0; i < lstTiet2.Count; i++)
                {
                    lopMon2 = lstLopMon.Where(x => x.ID_LOP == lstTiet2[i].ID_LOP && x.ID_MON_TRUONG == lstTiet2[i].ID_MON_2 && x.HOC_KY == lstTiet2[i].ID_HOC_KY).FirstOrDefault();
                    lopMon3 = lstLopMon.Where(x => x.ID_LOP == lstTiet2[i].ID_LOP && x.ID_MON_TRUONG == lstTiet2[i].ID_MON_3 && x.HOC_KY == lstTiet2[i].ID_HOC_KY).FirstOrDefault();
                    lopMon4 = lstLopMon.Where(x => x.ID_LOP == lstTiet2[i].ID_LOP && x.ID_MON_TRUONG == lstTiet2[i].ID_MON_4 && x.HOC_KY == lstTiet2[i].ID_HOC_KY).FirstOrDefault();
                    lopMon5 = lstLopMon.Where(x => x.ID_LOP == lstTiet2[i].ID_LOP && x.ID_MON_TRUONG == lstTiet2[i].ID_MON_5 && x.HOC_KY == lstTiet2[i].ID_HOC_KY).FirstOrDefault();
                    lopMon6 = lstLopMon.Where(x => x.ID_LOP == lstTiet2[i].ID_LOP && x.ID_MON_TRUONG == lstTiet2[i].ID_MON_6 && x.HOC_KY == lstTiet2[i].ID_HOC_KY).FirstOrDefault();
                    lopMon7 = lstLopMon.Where(x => x.ID_LOP == lstTiet2[i].ID_LOP && x.ID_MON_TRUONG == lstTiet2[i].ID_MON_7 && x.HOC_KY == lstTiet2[i].ID_HOC_KY).FirstOrDefault();
                    lopMon8 = lstLopMon.Where(x => x.ID_LOP == lstTiet2[i].ID_LOP && x.ID_MON_TRUONG == lstTiet2[i].ID_MON_8 && x.HOC_KY == lstTiet2[i].ID_HOC_KY).FirstOrDefault();
                    if (lopMon2 != null) lichDay.THU2 += lopMon2.TEN_LOP + " - " + lopMon2.TEN_MON_TRUONG + ", ";
                    if (lopMon3 != null) lichDay.THU3 += lopMon3.TEN_LOP + " - " + lopMon3.TEN_MON_TRUONG + ", ";
                    if (lopMon4 != null) lichDay.THU4 += lopMon4.TEN_LOP + " - " + lopMon4.TEN_MON_TRUONG + ", ";
                    if (lopMon5 != null) lichDay.THU5 += lopMon5.TEN_LOP + " - " + lopMon5.TEN_MON_TRUONG + ", ";
                    if (lopMon6 != null) lichDay.THU6 += lopMon6.TEN_LOP + " - " + lopMon6.TEN_MON_TRUONG + ", ";
                    if (lopMon7 != null) lichDay.THU7 += lopMon7.TEN_LOP + " - " + lopMon7.TEN_MON_TRUONG + ", ";
                    if (lopMon8 != null) lichDay.THU8 += lopMon8.TEN_LOP + " - " + lopMon8.TEN_MON_TRUONG + ", ";
                }
                lichDay.THU2 = lichDay.THU2 == null ? "" : lichDay.THU2.Trim().TrimEnd(',');
                lichDay.THU3 = lichDay.THU3 == null ? "" : lichDay.THU3.Trim().TrimEnd(',');
                lichDay.THU4 = lichDay.THU4 == null ? "" : lichDay.THU4.Trim().TrimEnd(',');
                lichDay.THU5 = lichDay.THU5 == null ? "" : lichDay.THU5.Trim().TrimEnd(',');
                lichDay.THU6 = lichDay.THU6 == null ? "" : lichDay.THU6.Trim().TrimEnd(',');
                lichDay.THU7 = lichDay.THU7 == null ? "" : lichDay.THU7.Trim().TrimEnd(',');
                lichDay.THU8 = lichDay.THU8 == null ? "" : lichDay.THU8.Trim().TrimEnd(',');
                lstLichDay.Add(lichDay);
            }
            #endregion
            #region "tiet 3"
            List<CaHocEntity> lstTiet3 = lstCaHoc.Where(x => x.TIET == 3).ToList();
            if (lstCaHoc != null && lstCaHoc.Count > 0)
            {
                LichDayEntity lichDay = new LichDayEntity();
                lichDay.TIET = 3;
                for (int i = 0; i < lstTiet3.Count; i++)
                {
                    lopMon2 = lstLopMon.Where(x => x.ID_LOP == lstTiet3[i].ID_LOP && x.ID_MON_TRUONG == lstTiet3[i].ID_MON_2 && x.HOC_KY == lstTiet3[i].ID_HOC_KY).FirstOrDefault();
                    lopMon3 = lstLopMon.Where(x => x.ID_LOP == lstTiet3[i].ID_LOP && x.ID_MON_TRUONG == lstTiet3[i].ID_MON_3 && x.HOC_KY == lstTiet3[i].ID_HOC_KY).FirstOrDefault();
                    lopMon4 = lstLopMon.Where(x => x.ID_LOP == lstTiet3[i].ID_LOP && x.ID_MON_TRUONG == lstTiet3[i].ID_MON_4 && x.HOC_KY == lstTiet3[i].ID_HOC_KY).FirstOrDefault();
                    lopMon5 = lstLopMon.Where(x => x.ID_LOP == lstTiet3[i].ID_LOP && x.ID_MON_TRUONG == lstTiet3[i].ID_MON_5 && x.HOC_KY == lstTiet3[i].ID_HOC_KY).FirstOrDefault();
                    lopMon6 = lstLopMon.Where(x => x.ID_LOP == lstTiet3[i].ID_LOP && x.ID_MON_TRUONG == lstTiet3[i].ID_MON_6 && x.HOC_KY == lstTiet3[i].ID_HOC_KY).FirstOrDefault();
                    lopMon7 = lstLopMon.Where(x => x.ID_LOP == lstTiet3[i].ID_LOP && x.ID_MON_TRUONG == lstTiet3[i].ID_MON_7 && x.HOC_KY == lstTiet3[i].ID_HOC_KY).FirstOrDefault();
                    lopMon8 = lstLopMon.Where(x => x.ID_LOP == lstTiet3[i].ID_LOP && x.ID_MON_TRUONG == lstTiet3[i].ID_MON_8 && x.HOC_KY == lstTiet3[i].ID_HOC_KY).FirstOrDefault();
                    if (lopMon2 != null) lichDay.THU2 += lopMon2.TEN_LOP + " - " + lopMon2.TEN_MON_TRUONG + ", ";
                    if (lopMon3 != null) lichDay.THU3 += lopMon3.TEN_LOP + " - " + lopMon3.TEN_MON_TRUONG + ", ";
                    if (lopMon4 != null) lichDay.THU4 += lopMon4.TEN_LOP + " - " + lopMon4.TEN_MON_TRUONG + ", ";
                    if (lopMon5 != null) lichDay.THU5 += lopMon5.TEN_LOP + " - " + lopMon5.TEN_MON_TRUONG + ", ";
                    if (lopMon6 != null) lichDay.THU6 += lopMon6.TEN_LOP + " - " + lopMon6.TEN_MON_TRUONG + ", ";
                    if (lopMon7 != null) lichDay.THU7 += lopMon7.TEN_LOP + " - " + lopMon7.TEN_MON_TRUONG + ", ";
                    if (lopMon8 != null) lichDay.THU8 += lopMon8.TEN_LOP + " - " + lopMon8.TEN_MON_TRUONG + ", ";
                }
                lichDay.THU2 = lichDay.THU2 == null ? "" : lichDay.THU2.Trim().TrimEnd(',');
                lichDay.THU3 = lichDay.THU3 == null ? "" : lichDay.THU3.Trim().TrimEnd(',');
                lichDay.THU4 = lichDay.THU4 == null ? "" : lichDay.THU4.Trim().TrimEnd(',');
                lichDay.THU5 = lichDay.THU5 == null ? "" : lichDay.THU5.Trim().TrimEnd(',');
                lichDay.THU6 = lichDay.THU6 == null ? "" : lichDay.THU6.Trim().TrimEnd(',');
                lichDay.THU7 = lichDay.THU7 == null ? "" : lichDay.THU7.Trim().TrimEnd(',');
                lichDay.THU8 = lichDay.THU8 == null ? "" : lichDay.THU8.Trim().TrimEnd(',');
                lstLichDay.Add(lichDay);
            }
            #endregion
            #region "tiet 4"
            List<CaHocEntity> lstTiet4 = lstCaHoc.Where(x => x.TIET == 4).ToList();
            if (lstCaHoc != null && lstCaHoc.Count > 0)
            {
                LichDayEntity lichDay = new LichDayEntity();
                lichDay.TIET = 4;
                for (int i = 0; i < lstTiet4.Count; i++)
                {
                    lopMon2 = lstLopMon.Where(x => x.ID_LOP == lstTiet4[i].ID_LOP && x.ID_MON_TRUONG == lstTiet4[i].ID_MON_2 && x.HOC_KY == lstTiet4[i].ID_HOC_KY).FirstOrDefault();
                    lopMon3 = lstLopMon.Where(x => x.ID_LOP == lstTiet4[i].ID_LOP && x.ID_MON_TRUONG == lstTiet4[i].ID_MON_3 && x.HOC_KY == lstTiet4[i].ID_HOC_KY).FirstOrDefault();
                    lopMon4 = lstLopMon.Where(x => x.ID_LOP == lstTiet4[i].ID_LOP && x.ID_MON_TRUONG == lstTiet4[i].ID_MON_4 && x.HOC_KY == lstTiet4[i].ID_HOC_KY).FirstOrDefault();
                    lopMon5 = lstLopMon.Where(x => x.ID_LOP == lstTiet4[i].ID_LOP && x.ID_MON_TRUONG == lstTiet4[i].ID_MON_5 && x.HOC_KY == lstTiet4[i].ID_HOC_KY).FirstOrDefault();
                    lopMon6 = lstLopMon.Where(x => x.ID_LOP == lstTiet4[i].ID_LOP && x.ID_MON_TRUONG == lstTiet4[i].ID_MON_6 && x.HOC_KY == lstTiet4[i].ID_HOC_KY).FirstOrDefault();
                    lopMon7 = lstLopMon.Where(x => x.ID_LOP == lstTiet4[i].ID_LOP && x.ID_MON_TRUONG == lstTiet4[i].ID_MON_7 && x.HOC_KY == lstTiet4[i].ID_HOC_KY).FirstOrDefault();
                    lopMon8 = lstLopMon.Where(x => x.ID_LOP == lstTiet4[i].ID_LOP && x.ID_MON_TRUONG == lstTiet4[i].ID_MON_8 && x.HOC_KY == lstTiet4[i].ID_HOC_KY).FirstOrDefault();
                    if (lopMon2 != null) lichDay.THU2 += lopMon2.TEN_LOP + " - " + lopMon2.TEN_MON_TRUONG + ", ";
                    if (lopMon3 != null) lichDay.THU3 += lopMon3.TEN_LOP + " - " + lopMon3.TEN_MON_TRUONG + ", ";
                    if (lopMon4 != null) lichDay.THU4 += lopMon4.TEN_LOP + " - " + lopMon4.TEN_MON_TRUONG + ", ";
                    if (lopMon5 != null) lichDay.THU5 += lopMon5.TEN_LOP + " - " + lopMon5.TEN_MON_TRUONG + ", ";
                    if (lopMon6 != null) lichDay.THU6 += lopMon6.TEN_LOP + " - " + lopMon6.TEN_MON_TRUONG + ", ";
                    if (lopMon7 != null) lichDay.THU7 += lopMon7.TEN_LOP + " - " + lopMon7.TEN_MON_TRUONG + ", ";
                    if (lopMon8 != null) lichDay.THU8 += lopMon8.TEN_LOP + " - " + lopMon8.TEN_MON_TRUONG + ", ";
                }
                lichDay.THU2 = lichDay.THU2 == null ? "" : lichDay.THU2.Trim().TrimEnd(',');
                lichDay.THU3 = lichDay.THU3 == null ? "" : lichDay.THU3.Trim().TrimEnd(',');
                lichDay.THU4 = lichDay.THU4 == null ? "" : lichDay.THU4.Trim().TrimEnd(',');
                lichDay.THU5 = lichDay.THU5 == null ? "" : lichDay.THU5.Trim().TrimEnd(',');
                lichDay.THU6 = lichDay.THU6 == null ? "" : lichDay.THU6.Trim().TrimEnd(',');
                lichDay.THU7 = lichDay.THU7 == null ? "" : lichDay.THU7.Trim().TrimEnd(',');
                lichDay.THU8 = lichDay.THU8 == null ? "" : lichDay.THU8.Trim().TrimEnd(',');
                lstLichDay.Add(lichDay);
            }
            #endregion
            #region "tiet 5"
            List<CaHocEntity> lstTiet5 = lstCaHoc.Where(x => x.TIET == 5).ToList();
            if (lstCaHoc != null && lstCaHoc.Count > 0)
            {
                LichDayEntity lichDay = new LichDayEntity();
                lichDay.TIET = 5;
                for (int i = 0; i < lstTiet5.Count; i++)
                {
                    lopMon2 = lstLopMon.Where(x => x.ID_LOP == lstTiet5[i].ID_LOP && x.ID_MON_TRUONG == lstTiet5[i].ID_MON_2 && x.HOC_KY == lstTiet5[i].ID_HOC_KY).FirstOrDefault();
                    lopMon3 = lstLopMon.Where(x => x.ID_LOP == lstTiet5[i].ID_LOP && x.ID_MON_TRUONG == lstTiet5[i].ID_MON_3 && x.HOC_KY == lstTiet5[i].ID_HOC_KY).FirstOrDefault();
                    lopMon4 = lstLopMon.Where(x => x.ID_LOP == lstTiet5[i].ID_LOP && x.ID_MON_TRUONG == lstTiet5[i].ID_MON_4 && x.HOC_KY == lstTiet5[i].ID_HOC_KY).FirstOrDefault();
                    lopMon5 = lstLopMon.Where(x => x.ID_LOP == lstTiet5[i].ID_LOP && x.ID_MON_TRUONG == lstTiet5[i].ID_MON_5 && x.HOC_KY == lstTiet5[i].ID_HOC_KY).FirstOrDefault();
                    lopMon6 = lstLopMon.Where(x => x.ID_LOP == lstTiet5[i].ID_LOP && x.ID_MON_TRUONG == lstTiet5[i].ID_MON_6 && x.HOC_KY == lstTiet5[i].ID_HOC_KY).FirstOrDefault();
                    lopMon7 = lstLopMon.Where(x => x.ID_LOP == lstTiet5[i].ID_LOP && x.ID_MON_TRUONG == lstTiet5[i].ID_MON_7 && x.HOC_KY == lstTiet5[i].ID_HOC_KY).FirstOrDefault();
                    lopMon8 = lstLopMon.Where(x => x.ID_LOP == lstTiet5[i].ID_LOP && x.ID_MON_TRUONG == lstTiet5[i].ID_MON_8 && x.HOC_KY == lstTiet5[i].ID_HOC_KY).FirstOrDefault();
                    if (lopMon2 != null) lichDay.THU2 += lopMon2.TEN_LOP + " - " + lopMon2.TEN_MON_TRUONG + ", ";
                    if (lopMon3 != null) lichDay.THU3 += lopMon3.TEN_LOP + " - " + lopMon3.TEN_MON_TRUONG + ", ";
                    if (lopMon4 != null) lichDay.THU4 += lopMon4.TEN_LOP + " - " + lopMon4.TEN_MON_TRUONG + ", ";
                    if (lopMon5 != null) lichDay.THU5 += lopMon5.TEN_LOP + " - " + lopMon5.TEN_MON_TRUONG + ", ";
                    if (lopMon6 != null) lichDay.THU6 += lopMon6.TEN_LOP + " - " + lopMon6.TEN_MON_TRUONG + ", ";
                    if (lopMon7 != null) lichDay.THU7 += lopMon7.TEN_LOP + " - " + lopMon7.TEN_MON_TRUONG + ", ";
                    if (lopMon8 != null) lichDay.THU8 += lopMon8.TEN_LOP + " - " + lopMon8.TEN_MON_TRUONG + ", ";
                }
                lichDay.THU2 = lichDay.THU2 == null ? "" : lichDay.THU2.Trim().TrimEnd(',');
                lichDay.THU3 = lichDay.THU3 == null ? "" : lichDay.THU3.Trim().TrimEnd(',');
                lichDay.THU4 = lichDay.THU4 == null ? "" : lichDay.THU4.Trim().TrimEnd(',');
                lichDay.THU5 = lichDay.THU5 == null ? "" : lichDay.THU5.Trim().TrimEnd(',');
                lichDay.THU6 = lichDay.THU6 == null ? "" : lichDay.THU6.Trim().TrimEnd(',');
                lichDay.THU7 = lichDay.THU7 == null ? "" : lichDay.THU7.Trim().TrimEnd(',');
                lichDay.THU8 = lichDay.THU8 == null ? "" : lichDay.THU8.Trim().TrimEnd(',');
                lstLichDay.Add(lichDay);
            }
            #endregion
            #region "tiet 6"
            List<CaHocEntity> lstTiet6 = lstCaHoc.Where(x => x.TIET == 6).ToList();
            if (lstCaHoc != null && lstCaHoc.Count > 0)
            {
                LichDayEntity lichDay = new LichDayEntity();
                lichDay.TIET = 6;
                for (int i = 0; i < lstTiet6.Count; i++)
                {
                    lopMon2 = lstLopMon.Where(x => x.ID_LOP == lstTiet6[i].ID_LOP && x.ID_MON_TRUONG == lstTiet6[i].ID_MON_2 && x.HOC_KY == lstTiet6[i].ID_HOC_KY).FirstOrDefault();
                    lopMon3 = lstLopMon.Where(x => x.ID_LOP == lstTiet6[i].ID_LOP && x.ID_MON_TRUONG == lstTiet6[i].ID_MON_3 && x.HOC_KY == lstTiet6[i].ID_HOC_KY).FirstOrDefault();
                    lopMon4 = lstLopMon.Where(x => x.ID_LOP == lstTiet6[i].ID_LOP && x.ID_MON_TRUONG == lstTiet6[i].ID_MON_4 && x.HOC_KY == lstTiet6[i].ID_HOC_KY).FirstOrDefault();
                    lopMon5 = lstLopMon.Where(x => x.ID_LOP == lstTiet6[i].ID_LOP && x.ID_MON_TRUONG == lstTiet6[i].ID_MON_5 && x.HOC_KY == lstTiet6[i].ID_HOC_KY).FirstOrDefault();
                    lopMon6 = lstLopMon.Where(x => x.ID_LOP == lstTiet6[i].ID_LOP && x.ID_MON_TRUONG == lstTiet6[i].ID_MON_6 && x.HOC_KY == lstTiet6[i].ID_HOC_KY).FirstOrDefault();
                    lopMon7 = lstLopMon.Where(x => x.ID_LOP == lstTiet6[i].ID_LOP && x.ID_MON_TRUONG == lstTiet6[i].ID_MON_7 && x.HOC_KY == lstTiet6[i].ID_HOC_KY).FirstOrDefault();
                    lopMon8 = lstLopMon.Where(x => x.ID_LOP == lstTiet6[i].ID_LOP && x.ID_MON_TRUONG == lstTiet6[i].ID_MON_8 && x.HOC_KY == lstTiet6[i].ID_HOC_KY).FirstOrDefault();
                    if (lopMon2 != null) lichDay.THU2 += lopMon2.TEN_LOP + " - " + lopMon2.TEN_MON_TRUONG + ", ";
                    if (lopMon3 != null) lichDay.THU3 += lopMon3.TEN_LOP + " - " + lopMon3.TEN_MON_TRUONG + ", ";
                    if (lopMon4 != null) lichDay.THU4 += lopMon4.TEN_LOP + " - " + lopMon4.TEN_MON_TRUONG + ", ";
                    if (lopMon5 != null) lichDay.THU5 += lopMon5.TEN_LOP + " - " + lopMon5.TEN_MON_TRUONG + ", ";
                    if (lopMon6 != null) lichDay.THU6 += lopMon6.TEN_LOP + " - " + lopMon6.TEN_MON_TRUONG + ", ";
                    if (lopMon7 != null) lichDay.THU7 += lopMon7.TEN_LOP + " - " + lopMon7.TEN_MON_TRUONG + ", ";
                    if (lopMon8 != null) lichDay.THU8 += lopMon8.TEN_LOP + " - " + lopMon8.TEN_MON_TRUONG + ", ";
                }
                lichDay.THU2 = lichDay.THU2 == null ? "" : lichDay.THU2.Trim().TrimEnd(',');
                lichDay.THU3 = lichDay.THU3 == null ? "" : lichDay.THU3.Trim().TrimEnd(',');
                lichDay.THU4 = lichDay.THU4 == null ? "" : lichDay.THU4.Trim().TrimEnd(',');
                lichDay.THU5 = lichDay.THU5 == null ? "" : lichDay.THU5.Trim().TrimEnd(',');
                lichDay.THU6 = lichDay.THU6 == null ? "" : lichDay.THU6.Trim().TrimEnd(',');
                lichDay.THU7 = lichDay.THU7 == null ? "" : lichDay.THU7.Trim().TrimEnd(',');
                lichDay.THU8 = lichDay.THU8 == null ? "" : lichDay.THU8.Trim().TrimEnd(',');
                lstLichDay.Add(lichDay);
            }
            #endregion
            #region "tiet 7"
            List<CaHocEntity> lstTiet7 = lstCaHoc.Where(x => x.TIET == 7).ToList();
            if (lstCaHoc != null && lstCaHoc.Count > 0)
            {
                LichDayEntity lichDay = new LichDayEntity();
                lichDay.TIET = 7;
                for (int i = 0; i < lstTiet7.Count; i++)
                {
                    lopMon2 = lstLopMon.Where(x => x.ID_LOP == lstTiet7[i].ID_LOP && x.ID_MON_TRUONG == lstTiet7[i].ID_MON_2 && x.HOC_KY == lstTiet7[i].ID_HOC_KY).FirstOrDefault();
                    lopMon3 = lstLopMon.Where(x => x.ID_LOP == lstTiet7[i].ID_LOP && x.ID_MON_TRUONG == lstTiet7[i].ID_MON_3 && x.HOC_KY == lstTiet7[i].ID_HOC_KY).FirstOrDefault();
                    lopMon4 = lstLopMon.Where(x => x.ID_LOP == lstTiet7[i].ID_LOP && x.ID_MON_TRUONG == lstTiet7[i].ID_MON_4 && x.HOC_KY == lstTiet7[i].ID_HOC_KY).FirstOrDefault();
                    lopMon5 = lstLopMon.Where(x => x.ID_LOP == lstTiet7[i].ID_LOP && x.ID_MON_TRUONG == lstTiet7[i].ID_MON_5 && x.HOC_KY == lstTiet7[i].ID_HOC_KY).FirstOrDefault();
                    lopMon6 = lstLopMon.Where(x => x.ID_LOP == lstTiet7[i].ID_LOP && x.ID_MON_TRUONG == lstTiet7[i].ID_MON_6 && x.HOC_KY == lstTiet7[i].ID_HOC_KY).FirstOrDefault();
                    lopMon7 = lstLopMon.Where(x => x.ID_LOP == lstTiet7[i].ID_LOP && x.ID_MON_TRUONG == lstTiet7[i].ID_MON_7 && x.HOC_KY == lstTiet7[i].ID_HOC_KY).FirstOrDefault();
                    lopMon8 = lstLopMon.Where(x => x.ID_LOP == lstTiet7[i].ID_LOP && x.ID_MON_TRUONG == lstTiet7[i].ID_MON_8 && x.HOC_KY == lstTiet7[i].ID_HOC_KY).FirstOrDefault();
                    if (lopMon2 != null) lichDay.THU2 += lopMon2.TEN_LOP + " - " + lopMon2.TEN_MON_TRUONG + ", ";
                    if (lopMon3 != null) lichDay.THU3 += lopMon3.TEN_LOP + " - " + lopMon3.TEN_MON_TRUONG + ", ";
                    if (lopMon4 != null) lichDay.THU4 += lopMon4.TEN_LOP + " - " + lopMon4.TEN_MON_TRUONG + ", ";
                    if (lopMon5 != null) lichDay.THU5 += lopMon5.TEN_LOP + " - " + lopMon5.TEN_MON_TRUONG + ", ";
                    if (lopMon6 != null) lichDay.THU6 += lopMon6.TEN_LOP + " - " + lopMon6.TEN_MON_TRUONG + ", ";
                    if (lopMon7 != null) lichDay.THU7 += lopMon7.TEN_LOP + " - " + lopMon7.TEN_MON_TRUONG + ", ";
                    if (lopMon8 != null) lichDay.THU8 += lopMon8.TEN_LOP + " - " + lopMon8.TEN_MON_TRUONG + ", ";
                }
                lichDay.THU2 = lichDay.THU2 == null ? "" : lichDay.THU2.Trim().TrimEnd(',');
                lichDay.THU3 = lichDay.THU3 == null ? "" : lichDay.THU3.Trim().TrimEnd(',');
                lichDay.THU4 = lichDay.THU4 == null ? "" : lichDay.THU4.Trim().TrimEnd(',');
                lichDay.THU5 = lichDay.THU5 == null ? "" : lichDay.THU5.Trim().TrimEnd(',');
                lichDay.THU6 = lichDay.THU6 == null ? "" : lichDay.THU6.Trim().TrimEnd(',');
                lichDay.THU7 = lichDay.THU7 == null ? "" : lichDay.THU7.Trim().TrimEnd(',');
                lichDay.THU8 = lichDay.THU8 == null ? "" : lichDay.THU8.Trim().TrimEnd(',');
                lstLichDay.Add(lichDay);
            }
            #endregion
            #region "tiet 8"
            List<CaHocEntity> lstTiet8 = lstCaHoc.Where(x => x.TIET == 8).ToList();
            if (lstCaHoc != null && lstCaHoc.Count > 0)
            {
                LichDayEntity lichDay = new LichDayEntity();
                lichDay.TIET = 8;
                for (int i = 0; i < lstTiet8.Count; i++)
                {
                    lopMon2 = lstLopMon.Where(x => x.ID_LOP == lstTiet8[i].ID_LOP && x.ID_MON_TRUONG == lstTiet8[i].ID_MON_2 && x.HOC_KY == lstTiet8[i].ID_HOC_KY).FirstOrDefault();
                    lopMon3 = lstLopMon.Where(x => x.ID_LOP == lstTiet8[i].ID_LOP && x.ID_MON_TRUONG == lstTiet8[i].ID_MON_3 && x.HOC_KY == lstTiet8[i].ID_HOC_KY).FirstOrDefault();
                    lopMon4 = lstLopMon.Where(x => x.ID_LOP == lstTiet8[i].ID_LOP && x.ID_MON_TRUONG == lstTiet8[i].ID_MON_4 && x.HOC_KY == lstTiet8[i].ID_HOC_KY).FirstOrDefault();
                    lopMon5 = lstLopMon.Where(x => x.ID_LOP == lstTiet8[i].ID_LOP && x.ID_MON_TRUONG == lstTiet8[i].ID_MON_5 && x.HOC_KY == lstTiet8[i].ID_HOC_KY).FirstOrDefault();
                    lopMon6 = lstLopMon.Where(x => x.ID_LOP == lstTiet8[i].ID_LOP && x.ID_MON_TRUONG == lstTiet8[i].ID_MON_6 && x.HOC_KY == lstTiet8[i].ID_HOC_KY).FirstOrDefault();
                    lopMon7 = lstLopMon.Where(x => x.ID_LOP == lstTiet8[i].ID_LOP && x.ID_MON_TRUONG == lstTiet8[i].ID_MON_7 && x.HOC_KY == lstTiet8[i].ID_HOC_KY).FirstOrDefault();
                    lopMon8 = lstLopMon.Where(x => x.ID_LOP == lstTiet8[i].ID_LOP && x.ID_MON_TRUONG == lstTiet8[i].ID_MON_8 && x.HOC_KY == lstTiet8[i].ID_HOC_KY).FirstOrDefault();
                    if (lopMon2 != null) lichDay.THU2 += lopMon2.TEN_LOP + " - " + lopMon2.TEN_MON_TRUONG + ", ";
                    if (lopMon3 != null) lichDay.THU3 += lopMon3.TEN_LOP + " - " + lopMon3.TEN_MON_TRUONG + ", ";
                    if (lopMon4 != null) lichDay.THU4 += lopMon4.TEN_LOP + " - " + lopMon4.TEN_MON_TRUONG + ", ";
                    if (lopMon5 != null) lichDay.THU5 += lopMon5.TEN_LOP + " - " + lopMon5.TEN_MON_TRUONG + ", ";
                    if (lopMon6 != null) lichDay.THU6 += lopMon6.TEN_LOP + " - " + lopMon6.TEN_MON_TRUONG + ", ";
                    if (lopMon7 != null) lichDay.THU7 += lopMon7.TEN_LOP + " - " + lopMon7.TEN_MON_TRUONG + ", ";
                    if (lopMon8 != null) lichDay.THU8 += lopMon8.TEN_LOP + " - " + lopMon8.TEN_MON_TRUONG + ", ";
                }
                lichDay.THU2 = lichDay.THU2 == null ? "" : lichDay.THU2.Trim().TrimEnd(',');
                lichDay.THU3 = lichDay.THU3 == null ? "" : lichDay.THU3.Trim().TrimEnd(',');
                lichDay.THU4 = lichDay.THU4 == null ? "" : lichDay.THU4.Trim().TrimEnd(',');
                lichDay.THU5 = lichDay.THU5 == null ? "" : lichDay.THU5.Trim().TrimEnd(',');
                lichDay.THU6 = lichDay.THU6 == null ? "" : lichDay.THU6.Trim().TrimEnd(',');
                lichDay.THU7 = lichDay.THU7 == null ? "" : lichDay.THU7.Trim().TrimEnd(',');
                lichDay.THU8 = lichDay.THU8 == null ? "" : lichDay.THU8.Trim().TrimEnd(',');
                lstLichDay.Add(lichDay);
            }
            #endregion
            #region "tiet 9"
            List<CaHocEntity> lstTiet9 = lstCaHoc.Where(x => x.TIET == 9).ToList();
            if (lstCaHoc != null && lstCaHoc.Count > 0)
            {
                LichDayEntity lichDay = new LichDayEntity();
                lichDay.TIET = 9;
                for (int i = 0; i < lstTiet9.Count; i++)
                {
                    lopMon2 = lstLopMon.Where(x => x.ID_LOP == lstTiet9[i].ID_LOP && x.ID_MON_TRUONG == lstTiet9[i].ID_MON_2 && x.HOC_KY == lstTiet9[i].ID_HOC_KY).FirstOrDefault();
                    lopMon3 = lstLopMon.Where(x => x.ID_LOP == lstTiet9[i].ID_LOP && x.ID_MON_TRUONG == lstTiet9[i].ID_MON_3 && x.HOC_KY == lstTiet9[i].ID_HOC_KY).FirstOrDefault();
                    lopMon4 = lstLopMon.Where(x => x.ID_LOP == lstTiet9[i].ID_LOP && x.ID_MON_TRUONG == lstTiet9[i].ID_MON_4 && x.HOC_KY == lstTiet9[i].ID_HOC_KY).FirstOrDefault();
                    lopMon5 = lstLopMon.Where(x => x.ID_LOP == lstTiet9[i].ID_LOP && x.ID_MON_TRUONG == lstTiet9[i].ID_MON_5 && x.HOC_KY == lstTiet9[i].ID_HOC_KY).FirstOrDefault();
                    lopMon6 = lstLopMon.Where(x => x.ID_LOP == lstTiet9[i].ID_LOP && x.ID_MON_TRUONG == lstTiet9[i].ID_MON_6 && x.HOC_KY == lstTiet9[i].ID_HOC_KY).FirstOrDefault();
                    lopMon7 = lstLopMon.Where(x => x.ID_LOP == lstTiet9[i].ID_LOP && x.ID_MON_TRUONG == lstTiet9[i].ID_MON_7 && x.HOC_KY == lstTiet9[i].ID_HOC_KY).FirstOrDefault();
                    lopMon8 = lstLopMon.Where(x => x.ID_LOP == lstTiet9[i].ID_LOP && x.ID_MON_TRUONG == lstTiet9[i].ID_MON_8 && x.HOC_KY == lstTiet9[i].ID_HOC_KY).FirstOrDefault();
                    if (lopMon2 != null) lichDay.THU2 += lopMon2.TEN_LOP + " - " + lopMon2.TEN_MON_TRUONG + ", ";
                    if (lopMon3 != null) lichDay.THU3 += lopMon3.TEN_LOP + " - " + lopMon3.TEN_MON_TRUONG + ", ";
                    if (lopMon4 != null) lichDay.THU4 += lopMon4.TEN_LOP + " - " + lopMon4.TEN_MON_TRUONG + ", ";
                    if (lopMon5 != null) lichDay.THU5 += lopMon5.TEN_LOP + " - " + lopMon5.TEN_MON_TRUONG + ", ";
                    if (lopMon6 != null) lichDay.THU6 += lopMon6.TEN_LOP + " - " + lopMon6.TEN_MON_TRUONG + ", ";
                    if (lopMon7 != null) lichDay.THU7 += lopMon7.TEN_LOP + " - " + lopMon7.TEN_MON_TRUONG + ", ";
                    if (lopMon8 != null) lichDay.THU8 += lopMon8.TEN_LOP + " - " + lopMon8.TEN_MON_TRUONG + ", ";
                }
                lichDay.THU2 = lichDay.THU2 == null ? "" : lichDay.THU2.Trim().TrimEnd(',');
                lichDay.THU3 = lichDay.THU3 == null ? "" : lichDay.THU3.Trim().TrimEnd(',');
                lichDay.THU4 = lichDay.THU4 == null ? "" : lichDay.THU4.Trim().TrimEnd(',');
                lichDay.THU5 = lichDay.THU5 == null ? "" : lichDay.THU5.Trim().TrimEnd(',');
                lichDay.THU6 = lichDay.THU6 == null ? "" : lichDay.THU6.Trim().TrimEnd(',');
                lichDay.THU7 = lichDay.THU7 == null ? "" : lichDay.THU7.Trim().TrimEnd(',');
                lichDay.THU8 = lichDay.THU8 == null ? "" : lichDay.THU8.Trim().TrimEnd(',');
                lstLichDay.Add(lichDay);
            }
            #endregion
            #region "tiet 10"
            List<CaHocEntity> lstTiet10 = lstCaHoc.Where(x => x.TIET == 10).ToList();
            if (lstCaHoc != null && lstCaHoc.Count > 0)
            {
                LichDayEntity lichDay = new LichDayEntity();
                lichDay.TIET = 10;
                for (int i = 0; i < lstTiet10.Count; i++)
                {
                    lopMon2 = lstLopMon.Where(x => x.ID_LOP == lstTiet10[i].ID_LOP && x.ID_MON_TRUONG == lstTiet10[i].ID_MON_2 && x.HOC_KY == lstTiet10[i].ID_HOC_KY).FirstOrDefault();
                    lopMon3 = lstLopMon.Where(x => x.ID_LOP == lstTiet10[i].ID_LOP && x.ID_MON_TRUONG == lstTiet10[i].ID_MON_3 && x.HOC_KY == lstTiet10[i].ID_HOC_KY).FirstOrDefault();
                    lopMon4 = lstLopMon.Where(x => x.ID_LOP == lstTiet10[i].ID_LOP && x.ID_MON_TRUONG == lstTiet10[i].ID_MON_4 && x.HOC_KY == lstTiet10[i].ID_HOC_KY).FirstOrDefault();
                    lopMon5 = lstLopMon.Where(x => x.ID_LOP == lstTiet10[i].ID_LOP && x.ID_MON_TRUONG == lstTiet10[i].ID_MON_5 && x.HOC_KY == lstTiet10[i].ID_HOC_KY).FirstOrDefault();
                    lopMon6 = lstLopMon.Where(x => x.ID_LOP == lstTiet10[i].ID_LOP && x.ID_MON_TRUONG == lstTiet10[i].ID_MON_6 && x.HOC_KY == lstTiet10[i].ID_HOC_KY).FirstOrDefault();
                    lopMon7 = lstLopMon.Where(x => x.ID_LOP == lstTiet10[i].ID_LOP && x.ID_MON_TRUONG == lstTiet10[i].ID_MON_7 && x.HOC_KY == lstTiet10[i].ID_HOC_KY).FirstOrDefault();
                    lopMon8 = lstLopMon.Where(x => x.ID_LOP == lstTiet10[i].ID_LOP && x.ID_MON_TRUONG == lstTiet10[i].ID_MON_8 && x.HOC_KY == lstTiet10[i].ID_HOC_KY).FirstOrDefault();
                    if (lopMon2 != null) lichDay.THU2 += lopMon2.TEN_LOP + " - " + lopMon2.TEN_MON_TRUONG + ", ";
                    if (lopMon3 != null) lichDay.THU3 += lopMon3.TEN_LOP + " - " + lopMon3.TEN_MON_TRUONG + ", ";
                    if (lopMon4 != null) lichDay.THU4 += lopMon4.TEN_LOP + " - " + lopMon4.TEN_MON_TRUONG + ", ";
                    if (lopMon5 != null) lichDay.THU5 += lopMon5.TEN_LOP + " - " + lopMon5.TEN_MON_TRUONG + ", ";
                    if (lopMon6 != null) lichDay.THU6 += lopMon6.TEN_LOP + " - " + lopMon6.TEN_MON_TRUONG + ", ";
                    if (lopMon7 != null) lichDay.THU7 += lopMon7.TEN_LOP + " - " + lopMon7.TEN_MON_TRUONG + ", ";
                    if (lopMon8 != null) lichDay.THU8 += lopMon8.TEN_LOP + " - " + lopMon8.TEN_MON_TRUONG + ", ";
                }
                lichDay.THU2 = lichDay.THU2 == null ? "" : lichDay.THU2.Trim().TrimEnd(',');
                lichDay.THU3 = lichDay.THU3 == null ? "" : lichDay.THU3.Trim().TrimEnd(',');
                lichDay.THU4 = lichDay.THU4 == null ? "" : lichDay.THU4.Trim().TrimEnd(',');
                lichDay.THU5 = lichDay.THU5 == null ? "" : lichDay.THU5.Trim().TrimEnd(',');
                lichDay.THU6 = lichDay.THU6 == null ? "" : lichDay.THU6.Trim().TrimEnd(',');
                lichDay.THU7 = lichDay.THU7 == null ? "" : lichDay.THU7.Trim().TrimEnd(',');
                lichDay.THU8 = lichDay.THU8 == null ? "" : lichDay.THU8.Trim().TrimEnd(',');
                lstLichDay.Add(lichDay);
            }
            #endregion
            RadGrid1.DataSource = lstLichDay;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
        protected void rcbHocKy_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }
        protected void rcbGiaoVien_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
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
            string newName = "Lich_day_hoc.xlsx";

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
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "THU2"))
            {
                DataColumn col = new DataColumn("THU2");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Thứ 2", colM = 1, rowM = 1, width = 20 });
                lstColumn.Add(new ExcelEntity { Name = "THU2", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "THU3"))
            {
                DataColumn col = new DataColumn("THU3");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Thứ 3", colM = 1, rowM = 1, width = 20 });
                lstColumn.Add(new ExcelEntity { Name = "THU3", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "THU4"))
            {
                DataColumn col = new DataColumn("THU4");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Thứ 4", colM = 1, rowM = 1, width = 20 });
                lstColumn.Add(new ExcelEntity { Name = "THU4", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "THU5"))
            {
                DataColumn col = new DataColumn("THU5");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Thứ 5", colM = 1, rowM = 1, width = 20 });
                lstColumn.Add(new ExcelEntity { Name = "THU5", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "THU6"))
            {
                DataColumn col = new DataColumn("THU6");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Thứ 6", colM = 1, rowM = 1, width = 20 });
                lstColumn.Add(new ExcelEntity { Name = "THU6", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "THU7"))
            {
                DataColumn col = new DataColumn("THU7");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Thứ 7", colM = 1, rowM = 1, width = 20 });
                lstColumn.Add(new ExcelEntity { Name = "THU7", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            if (localAPI.checkColumnShowInRadGrid(RadGrid1, "THU8"))
            {
                DataColumn col = new DataColumn("THU8");
                dt.Columns.Add(col);
                lstHeader.Add(new ExcelHeaderEntity { name = "Chủ nhật", colM = 1, rowM = 1, width = 20 });
                lstColumn.Add(new ExcelEntity { Name = "THU8", Align = XLAlignmentHorizontalValues.Center, Color = XLColor.Black, Type = "String" });
            }
            #endregion
            RadGrid1.AllowPaging = false;
            foreach (GridDataItem item in RadGrid1.Items)
            {
                DataRow row = dt.NewRow();
                foreach (DataColumn col in dt.Columns)
                {
                    row[col.ColumnName] = item[col.ColumnName].Text.Replace("&nbsp;", " ");
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
            string hocKyNamHoc = "Năm học: " + Sys_Ten_Nam_Hoc + ", " + rcbHocKy.Text.ToLower();
            listCell.Add(new ExcelHeaderEntity { name = tenSo, colM = 3, rowM = 1, colName = "A", rowIndex = 1, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tenPhong, colM = 3, rowM = 1, colName = "A", rowIndex = 2, Align = XLAlignmentHorizontalValues.Left });
            listCell.Add(new ExcelHeaderEntity { name = tieuDe, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 3, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            listCell.Add(new ExcelHeaderEntity { name = hocKyNamHoc, colM = lstColumn.Count + 1, rowM = 1, colName = "A", rowIndex = 4, fontSize = 14, Align = XLAlignmentHorizontalValues.Center });
            string nameFileOutput = newName;
            localAPI.ExportExcelDynamic(serverPath, path, newName, nameFileOutput, 1, listCell, rowHeaderStart, rowStart, colStart, colEnd, dt, lstHeader, lstColumn, false);
        }
    }
}