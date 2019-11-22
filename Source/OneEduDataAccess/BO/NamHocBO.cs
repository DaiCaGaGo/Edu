using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class NamHocBO
    {
        #region get
        public List<NAM_HOC> getNamHoc(bool is_all = false, short id_all = 0, string text_all = "Chọn tất cả")
        {
            List<NAM_HOC> data = new List<NAM_HOC>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("NAM_HOC", "getNamHoc", is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.NAM_HOC orderby p.THU_TU select p).ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<NAM_HOC>();
                        NAM_HOC item_all = new NAM_HOC();
                        item_all.MA = id_all;
                        item_all.TEN = text_all;
                        data.Insert(0, item_all);
                    }
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<NAM_HOC>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<NAM_HOC> getNamHocNotNamHocID(int id_nam_hoc, bool is_all = false, short id_all = 0, string text_all = "Chọn tất cả")
        {
            List<NAM_HOC> data = new List<NAM_HOC>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("NAM_HOC", "getNamHocNotNamHocID", id_nam_hoc, is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.NAM_HOC where p.MA != id_nam_hoc orderby p.THU_TU select p).ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<NAM_HOC>();
                        NAM_HOC item_all = new NAM_HOC();
                        item_all.MA = id_all;
                        item_all.TEN = text_all;
                        data.Insert(0, item_all);
                    }
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<NAM_HOC>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public NAM_HOC getNamHocByMa(short ma)
        {
            NAM_HOC data = new NAM_HOC();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("NAM_HOC", "getNamHocByMa", ma);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.NAM_HOC where p.MA == ma select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as NAM_HOC;
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
        public ResultEntity update(short ma, short ma_new, string ten, short thu_tu)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    var sql = @"Update NAM_HOC SET MA = :0 ,TEN=:1 ,THU_TU=:2 WHERE MA = :3";
                    context.Database.ExecuteSqlCommand(sql, ma_new, ten, thu_tu, ma);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("NAM_HOC");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(short ma, string ten, short thu_tu)
        {
            NAM_HOC detail = new NAM_HOC();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail.MA = ma;
                    detail.TEN = ten;
                    detail.THU_TU = thu_tu;
                    detail = context.NAM_HOC.Add(detail);
                    context.SaveChanges();
                }
                res.ResObject = detail;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("NAM_HOC");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }

        public ResultEntity delete(short ma)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    var sql = @"delete FROM NAM_HOC where MA = :0";
                    context.Database.ExecuteSqlCommand(sql, ma);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("NAM_HOC");
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
