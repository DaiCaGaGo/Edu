using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class NangLuongTrongNgayBO
    {
        public NANG_LUONG_TRONG_NGAY getNangLuongNgayByKhoi(short id_khoi)
        {
            NANG_LUONG_TRONG_NGAY data = new NANG_LUONG_TRONG_NGAY();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("NANG_LUONG_TRONG_NGAY", "getNangLuongNgayByKhoi", id_khoi);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.NANG_LUONG_TRONG_NGAY where p.ID_KHOI == id_khoi select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as NANG_LUONG_TRONG_NGAY;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
    }
}
