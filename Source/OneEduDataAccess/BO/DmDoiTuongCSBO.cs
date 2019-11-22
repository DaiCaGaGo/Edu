using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class DmDoiTuongCSBO
    {
        #region Get
        public List<DM_DOI_TUONG_CHINH_SACH> getDoiTuongChinhSach(bool is_all = false, short id_all = 0, string text_all = "Chọn tất cả")
        {
            List<DM_DOI_TUONG_CHINH_SACH> data = new List<DM_DOI_TUONG_CHINH_SACH>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_DOI_TUONG_CHINH_SACH", "getDoiTuongChinhSach", is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DM_DOI_TUONG_CHINH_SACH orderby p.THU_TU select p).ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<DM_DOI_TUONG_CHINH_SACH>();
                        DM_DOI_TUONG_CHINH_SACH item_all = new DM_DOI_TUONG_CHINH_SACH();
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
                    data = QICache.Get(strKeyCache) as List<DM_DOI_TUONG_CHINH_SACH>;
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
