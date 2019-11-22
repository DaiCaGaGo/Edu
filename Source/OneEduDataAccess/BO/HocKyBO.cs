using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class HocKyBO
    {
        #region Get
        public List<HOC_KY> getHocKy(bool is_all = false, short id_all = 0, string text_all = "Chọn tất cả")
        {
            List<HOC_KY> data = new List<HOC_KY>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_KY", "getHocKy", is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.HOC_KY orderby p.THU_TU select p).ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<HOC_KY>();
                        HOC_KY item_all = new HOC_KY();
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
                    data = QICache.Get(strKeyCache) as List<HOC_KY>;
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
