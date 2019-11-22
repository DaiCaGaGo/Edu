using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class MaNXDinhKyBO
    {
        #region get
        public List<MA_NX_DINH_KY> getMaNhanXetDinhKy(long id_truong, string ma, string noi_dung, short? ma_loai_nx)
        {
            List<MA_NX_DINH_KY> data = new List<MA_NX_DINH_KY>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MA_NX_DINH_KY", "getMaNhanXetDinhKy", id_truong, ma, noi_dung, ma_loai_nx);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.MA_NX_DINH_KY where p.ID_TRUONG == id_truong select p);
                    if (!string.IsNullOrEmpty(ma))
                        tmp = tmp.Where(x => x.MA.ToLower().Contains(ma.ToLower()));
                    if (!string.IsNullOrEmpty(noi_dung))
                        tmp = tmp.Where(x => x.NOI_DUNG.ToLower().Contains(noi_dung.ToLower()));
                    if (ma_loai_nx != null)
                        tmp = tmp.Where(x => x.MA_LOAI_NX == ma_loai_nx);
                    tmp = tmp.OrderBy(x => x.THU_TU).ThenBy(x => x.MA);
                    data = tmp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<MA_NX_DINH_KY>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<DataJsonEntity> getMaNhanXetDinhKyToDataJson(long id_truong, short ma_loai_nx)
        {
            List<DataJsonEntity> data = new List<DataJsonEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MA_NX_DINH_KY", "getMaNhanXetDinhKyToDataJson", id_truong, ma_loai_nx);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.MA_NX_DINH_KY
                               where p.ID_TRUONG == id_truong && p.MA_LOAI_NX == ma_loai_nx
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
        public MA_NX_DINH_KY getMaNhanXetByID(long id)
        {
            MA_NX_DINH_KY data = new MA_NX_DINH_KY();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MA_NX_DINH_KY", "getMaNhanXetByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.MA_NX_DINH_KY where p.ID == id select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as MA_NX_DINH_KY;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public MA_NX_DINH_KY getMaNhanXetByMa(long id_truong, string ma)
        {
            MA_NX_DINH_KY data = new MA_NX_DINH_KY();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MA_NX_DINH_KY", "getMaNhanXetByMa", id_truong, ma);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.MA_NX_DINH_KY where p.ID_TRUONG == id_truong && p.MA.ToLower() == ma.ToLower() && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as MA_NX_DINH_KY;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public long? getMaxThuTuByTruong(long id_truong)
        {
            long? data = null;
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MA_NX_DINH_KY", "getMaxThuTuByTruong", id_truong);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var sql = @"select MAX(THU_TU) as THU_TU from MA_NX_DINH_KY 
                                    where ID_TRUONG = :0";
                    data = context.Database.SqlQuery<long?>(sql, id_truong).FirstOrDefault();
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
        public ResultEntity update(MA_NX_DINH_KY detail_in, long? nguoi)
        {
            MA_NX_DINH_KY detail = new MA_NX_DINH_KY();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.MA_NX_DINH_KY
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.MA = detail_in.MA;
                        detail.NOI_DUNG = detail_in.NOI_DUNG;
                        detail.ID_TRUONG = detail_in.ID_TRUONG;
                        detail.THU_TU = detail_in.THU_TU;
                        detail.MA_LOAI_NX = detail_in.MA_LOAI_NX;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("MA_NX_DINH_KY");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(MA_NX_DINH_KY detail_in, long? nguoi)
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
                    detail_in = context.MA_NX_DINH_KY.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("MA_NX_DINH_KY");
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
                        var sql = @"update MA_NX_DINH_KY set IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 where ID = :2";
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, id);
                    }
                    else
                    {
                        var sql = @"delete from MA_NX_DINH_KY where ID = :0";
                        context.Database.ExecuteSqlCommand(sql, id);
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("MA_NX_DINH_KY");
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
