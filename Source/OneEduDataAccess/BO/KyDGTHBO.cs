using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class KyDGTHBO
    {
        #region get
        public List<KY_DG_TH> getKyDGTHByKy(short id_hocKy, bool is_all = false, short id_all = 0, string text_all = "Chọn tất cả")
        {
            List<KY_DG_TH> data = new List<KY_DG_TH>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("KY_DG_TH", "getKyDGTHByKy", id_hocKy, is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.KY_DG_TH
                            where p.ID_HOC_KY == id_hocKy
                            orderby p.THU_TU
                            select p).ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<KY_DG_TH>();
                        KY_DG_TH item_all = new KY_DG_TH();
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
                    data = QICache.Get(strKeyCache) as List<KY_DG_TH>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public KY_DG_TH getKyDGTHByMa(decimal ma)
        {
            KY_DG_TH data = new KY_DG_TH();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("KY_DG_TH", "getKyDGTHByMa", ma);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.KY_DG_TH where p.MA == ma select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as KY_DG_TH;
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
