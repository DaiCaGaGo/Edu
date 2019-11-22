using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class DmQuanHuyenBO
    {
        #region Get
        public List<DM_QUAN_HUYEN> getQuanHuyen(bool is_all = false, short id_all = 0, string text_all = "Chọn tất cả")
        {
            List<DM_QUAN_HUYEN> data = new List<DM_QUAN_HUYEN>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_QUAN_HUYEN", "getQuanHuyen", is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DM_QUAN_HUYEN orderby p.THU_TU select p).ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<DM_QUAN_HUYEN>();
                        DM_QUAN_HUYEN item_all = new DM_QUAN_HUYEN();
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
                    data = QICache.Get(strKeyCache) as List<DM_QUAN_HUYEN>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<DM_QUAN_HUYEN> getQuanHuyenByTinh(short? ma_tinh, bool is_all = false, short id_all = 0, string text_all = "Chọn tất cả")
        {
            List<DM_QUAN_HUYEN> data = new List<DM_QUAN_HUYEN>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_QUAN_HUYEN", "getQuanHuyenByTinh", ma_tinh, is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.DM_QUAN_HUYEN where p.MA_TINH==ma_tinh orderby p.THU_TU,p.TEN select p);
                    data = tmp.ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<DM_QUAN_HUYEN>();
                        DM_QUAN_HUYEN item_all = new DM_QUAN_HUYEN();
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
                    data = QICache.Get(strKeyCache) as List<DM_QUAN_HUYEN>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        #endregion
    }
}
