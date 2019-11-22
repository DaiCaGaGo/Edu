using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class HocPhiDotThuLopBO
    {
        #region get
        public List<HOC_PHI_DOT_THU_LOP> getAllDotThuLop(long id_truong, string ma_cap_hoc, short id_nam_hoc, short? id_khoi, long? id_lop, long? id_dot_thu)
        {
            List<HOC_PHI_DOT_THU_LOP> data = new List<HOC_PHI_DOT_THU_LOP>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_PHI_DOT_THU_LOP", "getAllDotThuLop", id_truong, ma_cap_hoc, id_nam_hoc, id_khoi, id_lop, id_dot_thu);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.HOC_PHI_DOT_THU_LOP where p.ID_TRUONG == id_truong && p.ID_NAM_HOC == id_nam_hoc && p.IS_DELETE != true select p);
                    if (id_khoi != null)
                        tmp = tmp.Where(x => x.ID_KHOI == id_khoi);
                    else
                    {
                        if (ma_cap_hoc == SYS_Cap_Hoc.MN)
                            tmp = tmp.Where(x => x.ID_KHOI == 13 || x.ID_KHOI == 14);
                        else if (ma_cap_hoc == SYS_Cap_Hoc.TH)
                            tmp = tmp.Where(x => x.ID_KHOI == 1 || x.ID_KHOI == 2 || x.ID_KHOI == 3 || x.ID_KHOI == 4 || x.ID_KHOI == 5);
                        else if (ma_cap_hoc == SYS_Cap_Hoc.THCS)
                            tmp = tmp.Where(x => x.ID_KHOI == 6 || x.ID_KHOI == 7 || x.ID_KHOI == 8 || x.ID_KHOI == 9);
                        else if (ma_cap_hoc == SYS_Cap_Hoc.THPT)
                            tmp = tmp.Where(x => x.ID_KHOI == 10 || x.ID_KHOI == 11 || x.ID_KHOI == 12);
                    }
                    if (id_lop != null)
                        tmp = tmp.Where(x => x.ID_LOP == id_lop);
                    if (id_dot_thu != null)
                        tmp = tmp.Where(x => x.ID_DOT_THU == id_dot_thu);
                    data = tmp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<HOC_PHI_DOT_THU_LOP>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public HOC_PHI_DOT_THU_LOP getDotThuLopByID(long id)
        {
            HOC_PHI_DOT_THU_LOP data = new HOC_PHI_DOT_THU_LOP();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_PHI_DOT_THU_LOP", "getDotThuLopByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.HOC_PHI_DOT_THU_LOP where p.ID == id && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as HOC_PHI_DOT_THU_LOP;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<HocPhiDotThuLopEntity> getDotThuLopByDotThuAndKhoi(long id_truong, short id_nam_hoc, long? id_dot_thu, List<short> lstKhoi)
        {
            List<HocPhiDotThuLopEntity> data = new List<HocPhiDotThuLopEntity>();
            var QICache = new DefaultCacheProvider();
            DataAccessAPI dataAccessAPI = new DataAccessAPI();
            string str_lst_ma_khoi = "";
            str_lst_ma_khoi = dataAccessAPI.ConvertListToString<short>(lstKhoi, ",");
            string strKeyCache = QICache.BuildCachedKey("HOC_PHI_DOT_THU_LOP", "getDotThuLopByDotThuAndKhoi", id_truong, id_nam_hoc, id_dot_thu, str_lst_ma_khoi);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var strQuery = string.Format(@"select * from HOC_PHI_DOT_THU_LOP where ID_TRUONG={0} and ID_NAM_HOC={1}", id_truong, id_nam_hoc);
                    if (id_dot_thu != null)
                        strQuery += @" and ID_DOT_THU=" + id_dot_thu;
                    if (lstKhoi != null && lstKhoi.Count == 1)
                        strQuery += string.Format(@" and ID_KHOI ={0}", lstKhoi[0]);
                    else if (lstKhoi != null && lstKhoi.Count > 1)
                    {
                        strQuery += string.Format(" and not ( ID_KHOI !={0}", lstKhoi[0]);
                        for (int i = 1; i < lstKhoi.Count; i++)
                        {
                            strQuery += string.Format(" and ID_KHOI !={0}", lstKhoi[i]);
                        }
                        strQuery += " )";
                    }
                    strQuery += " order by ID_KHOI,ID_LOP";
                    data = context.Database.SqlQuery<HocPhiDotThuLopEntity>(strQuery).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<HocPhiDotThuLopEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        #endregion
        #region set
        public ResultEntity update(HOC_PHI_DOT_THU_LOP detail_in, long? nguoi)
        {
            HOC_PHI_DOT_THU_LOP detail = new HOC_PHI_DOT_THU_LOP();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.HOC_PHI_DOT_THU_LOP
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.ID_DOT_THU = detail_in.ID_DOT_THU;
                        detail.ID_LOP = detail_in.ID_LOP;
                        detail.ID_TRUONG = detail_in.ID_TRUONG;
                        detail.ID_NAM_HOC = detail_in.ID_NAM_HOC;
                        detail.ID_KHOI = detail_in.ID_KHOI;
                        detail.GHI_CHU = detail_in.GHI_CHU;
                        detail.IS_TIEN_AN = detail_in.IS_TIEN_AN;
                        detail.SO_TIEN_AN = detail_in.SO_TIEN_AN;
                        detail.TONG_TIEN = detail_in.TONG_TIEN;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("HOC_PHI_DOT_THU_LOP");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(HOC_PHI_DOT_THU_LOP detail_in, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    long newID = context.Database.SqlQuery<long>("SELECT HOC_PHI_DOT_THU_LOP_SEQ.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                    detail_in.ID = newID;
                    detail_in.NGAY_TAO = DateTime.Now;
                    detail_in.NGUOI_TAO = nguoi;
                    detail_in = context.HOC_PHI_DOT_THU_LOP.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("HOC_PHI_DOT_THU_LOP");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity delete(long id, long? nguoi, bool is_delete = false)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    if (!is_delete)
                    {
                        var sql = @"update HOC_PHI_DOT_THU_LOP set IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 where ID = :2";
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, id);
                    }
                    else
                    {
                        var sql = @"delete from HOC_PHI_DOT_THU_LOP where ID = :0";
                        context.Database.ExecuteSqlCommand(sql, id);
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("HOC_PHI_DOT_THU_LOP");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        #endregion
    }
}
