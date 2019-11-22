using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class ThuongHieuBO
    {
        public List<ThuongHieuEntity> getThuongHieu()
        {
            List<ThuongHieuEntity> data = new List<ThuongHieuEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TRUONG", "getThuongHieu");
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    try
                    {
                        string strQuery = string.Format(@"
                        select distinct brand_name from
(
select brand_name_gtel as brand_name from truong where not(truong.is_delete is not null and truong.is_delete=1) and truong.is_active_sms=1
union 
select brand_name_mobi as brand_name from truong where not(truong.is_delete is not null and truong.is_delete=1) and truong.is_active_sms=1
union 
select brand_name_viettel as brand_name from truong where not(truong.is_delete is not null and truong.is_delete=1) and truong.is_active_sms=1
union 
select brand_name_vina as brand_name from truong where not(truong.is_delete is not null and truong.is_delete=1) and truong.is_active_sms=1
union 
select brand_name_vnm as brand_name from truong where not(truong.is_delete is not null and truong.is_delete=1) and truong.is_active_sms=1)");
                        data = context.Database.SqlQuery<ThuongHieuEntity>(strQuery).ToList();
                        // QICache.Set(strKeyCache, data, 300000);
                    }
                    catch (Exception ex)
                    { }
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<ThuongHieuEntity>;
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
