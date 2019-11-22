using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class CustomerToBO
    {
        #region get
        public List<CUSTOMER_TO> getToCustomer(long id_truong, string ten, bool is_all = false, short id_all = 0, string text_all = "Chọn tất cả")
        {
            List<CUSTOMER_TO> data = new List<CUSTOMER_TO>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CUSTOMER_TO", "getToCustomer", id_truong, ten, is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var temp = (from p in context.CUSTOMER_TO where p.ID_TRUONG == id_truong && p.IS_DELETE != true select p);
                    if (!string.IsNullOrEmpty(ten))
                        temp = temp.Where(x => x.TEN.ToLower().Contains(ten.ToLower()));
                    temp = temp.OrderBy(x => x.THU_TU);
                    data = temp.ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<CUSTOMER_TO>();
                        CUSTOMER_TO item_all = new CUSTOMER_TO();
                        item_all.ID = id_all;
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
                    data = QICache.Get(strKeyCache) as List<CUSTOMER_TO>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public CUSTOMER_TO getToCustomerByID(long id)
        {
            CUSTOMER_TO data = new CUSTOMER_TO();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CUSTOMER_TO", "getToCustomerByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.CUSTOMER_TO where p.ID == id select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as CUSTOMER_TO;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public short? getMaxThuTuByTruong(long id_truong)
        {
            short? data = null;
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CUSTOMER_TO", "getMaxThuTuByTruong", id_truong);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var sql = @"select MAX(THU_TU) as THU_TU from CUSTOMER_TO where NOT (IS_DELETE is not null and IS_DELETE =1 ) and id_truong=:0";
                    data = context.Database.SqlQuery<short?>(sql, id_truong).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as short?;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        #endregion
        #region set
        public ResultEntity delete(short id, long? nguoi, bool is_delete = false)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    if (!is_delete)
                    {
                        var sql = @"update CUSTOMER_TO set IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 where ID = :2";
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, id);
                    }
                    else
                    {
                        var sql = @"delete CUSTOMER_TO where ID = :0";
                        context.Database.ExecuteSqlCommand(sql, id);
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CUSTOMER_TO");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(CUSTOMER_TO detail_in, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail_in.NGAY_TAO = DateTime.Now;
                    detail_in.NGUOI_TAO = nguoi;
                    detail_in = context.CUSTOMER_TO.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CUSTOMER_TO");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity update(CUSTOMER_TO detail_in, long? USerID, long? ID)
        {
            CUSTOMER_TO detail = new CUSTOMER_TO();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    var sql = @"update CUSTOMER_TO set TEN=:0,THU_TU=:1,NGUOI_SUA=:2,NGAY_SUA=:3 where ID=:4";
                    context.Database.ExecuteSqlCommand(sql, detail_in.TEN, detail_in.THU_TU, USerID, DateTime.Now, ID);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CUSTOMER_TO");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        #endregion
    }
}
