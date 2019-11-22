using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class MaNhanXetBO
    {
        #region get
        public List<MA_NXHN> getMaNhanXet(long id_truong, string ma, string noi_dung, string ma_cap_hoc)
        {
            List<MA_NXHN> data = new List<MA_NXHN>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MA_NXHN", "getMaNhanXet", id_truong, ma, noi_dung, ma_cap_hoc);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.MA_NXHN where p.ID_TRUONG == id_truong && p.MA_CAP_HOC == ma_cap_hoc select p);
                    if (!string.IsNullOrEmpty(ma))
                        tmp = tmp.Where(x => x.MA.ToLower().Contains(ma.ToLower()));
                    if (!string.IsNullOrEmpty(noi_dung))
                        tmp = tmp.Where(x => x.NOI_DUNG.ToLower().Contains(noi_dung.ToLower()));
                    tmp = tmp.OrderBy(x => x.THU_TU).ThenBy(x => x.MA);
                    data = tmp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<MA_NXHN>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public MA_NXHN getMaNhanXetByID(long id)
        {
            MA_NXHN data = new MA_NXHN();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MA_NXHN", "getMaNhanXetByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.MA_NXHN where p.ID == id select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as MA_NXHN;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public MA_NXHN getMaNhanXetByMa(long id_truong, string ma)
        {
            MA_NXHN data = new MA_NXHN();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MA_NXHN", "getMaNhanXetByMa", id_truong, ma);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.MA_NXHN where p.ID_TRUONG == id_truong && p.MA.ToLower() == ma.ToLower() select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as MA_NXHN;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public long? getMaxThuTuByTruong(long id_truong, string ma_cap_hoc)
        {
            long? data = null;
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MA_NXHN", "getMaxThuTuByTruong", id_truong, ma_cap_hoc);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var sql = @"select MAX(THU_TU) as THU_TU from MA_NXHN 
                                    where ID_TRUONG = :0 and ma_cap_hoc=:1";
                    data = context.Database.SqlQuery<long?>(sql, id_truong, ma_cap_hoc).FirstOrDefault();
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
        public List<DataJsonEntity> getMaNhanXetToDataJson(long id_truong)
        {
            List<DataJsonEntity> data = new List<DataJsonEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MA_NXHN", "getMaNhanXetToDataJson", id_truong);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.MA_NXHN
                               where p.ID_TRUONG == id_truong
                               orderby p.THU_TU, p.MA
                               select new DataJsonEntity
                               {
                                   name = p.MA + "/" + p.NOI_DUNG
                               }
                               );
                    data = tmp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<DataJsonEntity>;
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
        public ResultEntity update(MA_NXHN detail_in, long? nguoi)
        {
            MA_NXHN detail = new MA_NXHN();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.MA_NXHN
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.MA = detail_in.MA;
                        detail.NOI_DUNG = detail_in.NOI_DUNG;
                        detail.ID_TRUONG = detail_in.ID_TRUONG;
                        detail.THU_TU = detail_in.THU_TU;
                        detail.MA_CAP_HOC = detail_in.MA_CAP_HOC;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("MA_NXHN");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(MA_NXHN detail_in, long? nguoi)
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
                    detail_in = context.MA_NXHN.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("MA_NXHN");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insertNXHNBySys(long id_truong, string ma_cap_hoc, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    string sql = string.Format(@"insert into MA_NXHN (ID_TRUONG, MA, NOI_DUNG, THU_TU, NGUOI_TAO, NGAY_TAO, MA_CAP_HOC)
                                                select :0, MA, NOI_DUNG, THU_TU, :1 AS NGUOI_TAO, (SELECT SYSDATE FROM DUAL) as NGAY_TAO, :2
                                                from DM_MA_NX where not exists(select id from MA_NXHN where DM_MA_NX.MA = MA_NXHN.MA and MA_NXHN.id_truong=:0 and  MA_NXHN.MA_CAP_HOC = :2 and not (MA_NXHN.iS_delete is not null and MA_NXHN.is_delete = 1)) and DM_MA_NX.Ma_Cap_Hoc=:2");
                    context.Database.ExecuteSqlCommand(sql, id_truong, nguoi, ma_cap_hoc);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("MA_NXHN");
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
                        var sql = @"update MA_NXHN set IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 where ID = :2";
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, id);
                    }
                    else
                    {
                        var sql = @"delete from MA_NXHN where ID = :0";
                        context.Database.ExecuteSqlCommand(sql, id);
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("MA_NXHN");
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
