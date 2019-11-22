using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class DmMaNhanXetBO
    {
        #region get
        public List<DM_MA_NX> getMaNhanXet(string ma_cap_hoc, string ma, string noi_dung)
        {
            List<DM_MA_NX> data = new List<DM_MA_NX>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_MA_NX", "getMaNhanXet", ma_cap_hoc, ma, noi_dung);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.DM_MA_NX select p);
                    if (!string.IsNullOrEmpty(ma_cap_hoc))
                        tmp = tmp.Where(x => x.MA_CAP_HOC == ma_cap_hoc);
                    if (!string.IsNullOrEmpty(ma))
                        tmp = tmp.Where(x => x.MA.ToLower().Contains(ma.ToLower()));
                    if (!string.IsNullOrEmpty(noi_dung))
                        tmp = tmp.Where(x => x.NOI_DUNG.ToLower().Contains(noi_dung.ToLower()));
                    tmp = tmp.OrderBy(x => x.MA_CAP_HOC).ThenBy(x => x.THU_TU).ThenBy(x => x.MA);
                    data = tmp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<DM_MA_NX>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public DM_MA_NX getMaNhanXetByID(long id)
        {
            DM_MA_NX data = new DM_MA_NX();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_MA_NX", "getMaNhanXetByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DM_MA_NX where p.ID == id select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as DM_MA_NX;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public DM_MA_NX getMaNhanXetByMa(string ma, string ma_cap_hoc)
        {
            DM_MA_NX data = new DM_MA_NX();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_MA_NX", "getMaNhanXetByMa", ma, ma_cap_hoc);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DM_MA_NX where p.MA_CAP_HOC == ma_cap_hoc && p.MA.ToLower() == ma.ToLower() select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as DM_MA_NX;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public long? getMaxThuTu(string ma_cap_hoc)
        {
            long? data = null;
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_MA_NX", "getMaxThuTu", ma_cap_hoc);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var sql = @"select MAX(THU_TU) as THU_TU from DM_MA_NX where ma_cap_hoc='" + ma_cap_hoc + "'";
                    data = context.Database.SqlQuery<long?>(sql).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as long?;
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
        public ResultEntity update(DM_MA_NX detail_in, long? nguoi)
        {
            DM_MA_NX detail = new DM_MA_NX();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.DM_MA_NX
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.MA = detail_in.MA;
                        detail.NOI_DUNG = detail_in.NOI_DUNG;
                        detail.THU_TU = detail_in.THU_TU;
                        detail.MA_CAP_HOC = detail_in.MA_CAP_HOC;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DM_MA_NX");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(DM_MA_NX detail_in, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail_in.ID = context.Database.SqlQuery<long>("SELECT DM_MA_NX_SEQ.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                    detail_in = context.DM_MA_NX.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DM_MA_NX");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity delete(long id, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    var sql = @"delete from DM_MA_NX where ID = :0";
                    context.Database.ExecuteSqlCommand(sql, id);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DM_MA_NX");
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
