using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class GioiTinhBO
    {
        #region Get
        public List<GIOI_TINH> getGioiTinh(bool is_all = false, short id_all = 0, string text_all = "Chọn tất cả")
        {
            List<GIOI_TINH> data = new List<GIOI_TINH>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("GIOI_TINH", "getGioiTinh", is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.GIOI_TINH orderby p.THU_TU select p).ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<GIOI_TINH>();
                        GIOI_TINH item_all = new GIOI_TINH();
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
                    data = QICache.Get(strKeyCache) as List<GIOI_TINH>;
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
