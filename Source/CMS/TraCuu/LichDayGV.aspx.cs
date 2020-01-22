using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS.TraCuu
{
    public partial class LichDayGV : System.Web.UI.Page
    {
        CaHocBO caHocBO = new CaHocBO();
        GiaoVienBO giaoVienBO = new GiaoVienBO();
        LopMonBO lopMonBO = new LopMonBO();
        long? id_giao_vien;
        public List<LichDayEntity> lstLichDay = new List<LichDayEntity>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("id_giao_vien") != null)
            {
                try
                {
                    id_giao_vien = Convert.ToInt64(Request.QueryString.Get("id_giao_vien"));
                }
                catch (Exception ex) { }
            }
            if (!IsPostBack)
            {
                if (id_giao_vien != null)
                {
                    GIAO_VIEN giaoVien = giaoVienBO.getGiaoVienByID(id_giao_vien.Value);
                    if (giaoVien != null)
                    {
                        int id_nam_hoc = Convert.ToInt16(DateTime.Now.Year);
                        int hoc_ky = 1;
                        int thang = DateTime.Now.Month;
                        if (thang >= 1 && thang < 8)
                        {
                            id_nam_hoc = id_nam_hoc - 1;
                            hoc_ky = 2;
                        }
                        List<CaHocEntity> lstCaHoc = caHocBO.getCaHocByGiaoVienTruong(giaoVien.ID_TRUONG, Convert.ToInt16(id_nam_hoc), Convert.ToInt16(hoc_ky), id_giao_vien);
                        List<LopMonTruongEntity> lstLopMon = lopMonBO.getLopMonByGiaoVienNamHoc(giaoVien.ID_TRUONG, Convert.ToInt16(id_nam_hoc), Convert.ToInt16(hoc_ky), id_giao_vien);
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
                    }
                }
            }
        }
    }
}