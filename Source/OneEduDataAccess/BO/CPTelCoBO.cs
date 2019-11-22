using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class CPTelCoBO
    {
        #region Get
        public List<CP_TELCO> getTelco(bool is_all = false, string id_all = "", string text_all = "Chọn tất cả")
        {
            List<CP_TELCO> data = new List<CP_TELCO>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CP_TELCO", "getTelco", is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.CP_TELCO orderby p.TEN select p).ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<CP_TELCO>();
                        CP_TELCO item_all = new CP_TELCO();
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
                    data = QICache.Get(strKeyCache) as List<CP_TELCO>;
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
