using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class DmDanTocBO
    {
        #region Get
        public List<DM_DAN_TOC> getDanToc(bool is_all = false, short id_all = 0, string text_all = "Chọn tất cả")
        {
            List<DM_DAN_TOC> data = new List<DM_DAN_TOC>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_DAN_TOC", "getDanToc", is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DM_DAN_TOC orderby p.THU_TU select p).ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<DM_DAN_TOC>();
                        DM_DAN_TOC item_all = new DM_DAN_TOC();
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
                    data = QICache.Get(strKeyCache) as List<DM_DAN_TOC>;
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
