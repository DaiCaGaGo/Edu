using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class DmKhuVucBO
    {
        #region Get
        public List<DM_KHU_VUC> getKhuVuc(bool is_all = false, short id_all = 0, string text_all = "Chọn tất cả")
        {
            List<DM_KHU_VUC> data = new List<DM_KHU_VUC>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_KHU_VUC", "getKhuVuc", is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DM_KHU_VUC orderby p.THU_TU select p).ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<DM_KHU_VUC>();
                        DM_KHU_VUC item_all = new DM_KHU_VUC();
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
                    data = QICache.Get(strKeyCache) as List<DM_KHU_VUC>;
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
