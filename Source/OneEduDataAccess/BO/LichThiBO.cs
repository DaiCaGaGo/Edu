using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class LichThiBO
    {
        #region get
        public List<LichThiEntity> getLichThiByLop(long id_truong, short id_khoi, short id_nam_hoc, short hoc_ky, long id_lop)
        {
            List<LichThiEntity> data = new List<LichThiEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("LICH_THI", "MON_HOC_TRUONG", "getLichThiByLop", id_truong, id_khoi, id_nam_hoc, hoc_ky, id_lop);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var sql = string.Format(@"select a.id_mon_truong, a.ten, lt.time_1t, lt.time_gk, lt.time_hk, lt.time_15p, lt.id,
                        lt.id_truong, lt.id_khoi, lt.id_nam_hoc, lt.id_lop, lt.hoc_ky, lt.ngay_tao, lt.ngay_sua, lt.nguoi_sua, lt.nguoi_tao
                        from (
                        select lm.id_mon_truong, mt.ten from lop_mon lm 
                        join mon_hoc_truong mt on lm.id_mon_truong = mt.id
                        where lm.id_lop={0} and lm.hoc_ky={1} and (lm.is_delete is null or lm.is_delete = 0)
                        order by mt.thu_tu) a
                        left join lich_thi lt on a.id_mon_truong = lt.id_mon_truong 
                        and lt.id_truong={2} and lt.id_khoi={3} and lt.id_nam_hoc={4} and lt.id_lop={0} and lt.hoc_ky={1}", id_lop, hoc_ky, id_truong, id_khoi, id_nam_hoc);
                    data = context.Database.SqlQuery<LichThiEntity>(sql).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<LichThiEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public LICH_THI getLichThiByID(long id)
        {
            LICH_THI data = new LICH_THI();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("LICH_THI", "getLichThiByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.LICH_THI where p.ID == id select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as LICH_THI;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }

        public LICH_THI checkLichThiByMon(long id_truong, short id_khoi, long id_lop, short hoc_ky, long id_mon_truong)
        {
            LICH_THI data = new LICH_THI();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("LICH_THI", "checkLichThiByMon", id_truong, id_khoi, id_lop, hoc_ky, id_mon_truong);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.LICH_THI where p.ID_TRUONG == id_truong && p.ID_KHOI == id_khoi && p.ID_LOP == id_lop && p.HOC_KY == hoc_ky && p.ID_MON_TRUONG == id_mon_truong select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as LICH_THI;
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
        #region update
        public ResultEntity update(LICH_THI detail_in, long? nguoi)
        {
            LICH_THI detail = new LICH_THI();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.LICH_THI
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();

                    if (detail != null)
                    {
                        detail.ID_TRUONG = detail_in.ID_TRUONG;
                        detail.ID_KHOI = detail_in.ID_KHOI;
                        detail.ID_NAM_HOC = detail_in.ID_NAM_HOC;
                        detail.HOC_KY = detail_in.HOC_KY;
                        detail.ID_LOP = detail_in.ID_LOP;
                        detail.ID_MON_TRUONG = detail_in.ID_MON_TRUONG;
                        detail.TIME_15P = detail_in.TIME_15P;
                        detail.TIME_1T = detail_in.TIME_1T;
                        detail.TIME_GK = detail_in.TIME_GK;
                        detail.TIME_HK = detail_in.TIME_HK;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        context.SaveChanges();
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("LICH_THI");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        #endregion
        #region insert
        public ResultEntity insert(LICH_THI detail_in, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail_in.NGAY_TAO = DateTime.Now;
                    detail_in.NGUOI_TAO = nguoi;
                    context.LICH_THI.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("LICH_THI");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        #endregion
        #region delete
        public ResultEntity delete(long id, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    var sql = @"delete from LICH_THI where ID = :0";
                    context.Database.ExecuteSqlCommand(sql, id);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("LICH_THI");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }

            return res;
        }
        #endregion
        #endregion
    }
}
