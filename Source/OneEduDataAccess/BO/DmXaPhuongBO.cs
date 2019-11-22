using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class DmXaPhuongBO
    {
        #region Get
        public List<DM_XA_PHUONG> getXaPhuong(bool is_all = false, long id_all = 0, string text_all = "Chọn tất cả")
        {
            List<DM_XA_PHUONG> data = new List<DM_XA_PHUONG>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_XA_PHUONG", "getXaPhuong", is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DM_XA_PHUONG orderby p.THU_TU select p).ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<DM_XA_PHUONG>();
                        DM_XA_PHUONG item_all = new DM_XA_PHUONG();
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
                    data = QICache.Get(strKeyCache) as List<DM_XA_PHUONG>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<DM_XA_PHUONG> getXaPhuongByTinhHuyen(short? ma_tinh, short? ma_huyen, bool is_all = false, long id_all = 0, string text_all = "Chọn tất cả")
        {
            List<DM_XA_PHUONG> data = new List<DM_XA_PHUONG>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_XA_PHUONG", "getXaPhuongByTinhHuyen", ma_tinh, ma_huyen, is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.DM_XA_PHUONG where p.MA_TINH==ma_tinh && p.MA_HUYEN==ma_huyen orderby p.THU_TU select p);
                    data = tmp.ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<DM_XA_PHUONG>();
                        DM_XA_PHUONG item_all = new DM_XA_PHUONG();
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
                    data = QICache.Get(strKeyCache) as List<DM_XA_PHUONG>;
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
