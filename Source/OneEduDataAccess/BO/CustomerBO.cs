using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class CustomerBO
    {
        #region get
        public List<CUSTOMER> getCustomer(long idTruong, string ten, string sdt, short? ma_gioi_tinh)
        {
            List<CUSTOMER> data = new List<CUSTOMER>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CUSTOMER", "getCustomer", idTruong, ten, sdt, ma_gioi_tinh);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.CUSTOMERs
                               where p.IS_DELETE != true && p.ID_TRUONG == idTruong
                               select p);
                    if (!string.IsNullOrEmpty(ten))
                        tmp = tmp.Where(x => x.HO_TEN.ToLower().Contains(ten.ToLower()));
                    if (ma_gioi_tinh != null)
                        tmp = tmp.Where(x => x.GIOI_TINH == ma_gioi_tinh);
                    if (!string.IsNullOrEmpty(sdt))
                        tmp = tmp.Where(x => x.SDT.Contains(sdt));
                    tmp = tmp.OrderBy(x => x.TEN);
                    data = tmp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<CUSTOMER>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public CUSTOMER getCustomerByID(long id)
        {
            CUSTOMER data = new CUSTOMER();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CUSTOMER", "getCustomerByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.CUSTOMERs where p.ID == id select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as CUSTOMER;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public CUSTOMER checkCustomerByPhone(long id_truong, string phone)
        {
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CUSTOMER", "checkCustomerByPhone", id_truong, phone);
            CUSTOMER detail = new CUSTOMER();
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    detail = (from p in context.CUSTOMERs
                              where p.ID_TRUONG == id_truong && p.SDT == phone select p).FirstOrDefault();
                    QICache.Set(strKeyCache, detail, 300000);
                }
            }
            else
            {
                try
                {
                    detail = QICache.Get(strKeyCache) as CUSTOMER;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return detail;
        }
        public List<CustomerEntity> getCustomerNotExistsTo(long id_truong, short id_to)
        {
            List<CustomerEntity> data = new List<CustomerEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CUSTOMER", "CUSTOMER_TO_CUSTOMER", "getCustomerNotExistsTo", id_truong, id_to);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = @"select ID, TEN, HO_TEN, sdt
                        from customer 
                        where not exists 
                        (select customer_to_customer.id_customer from customer_to_customer where customer_to_customer.id_to=:0
                        and customer_to_customer.id_customer = customer.id and (is_delete = 0 or is_delete is null)) 
                        and id_truong=:1";
                    data = context.Database.SqlQuery<CustomerEntity>(strQuery, id_to, id_truong).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<CustomerEntity>;
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
        public ResultEntity insert(CUSTOMER detail_in, long? nguoi)
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
                    detail_in = context.CUSTOMERs.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CUSTOMER");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity update(CUSTOMER detail_in, long? nguoi)
        {
            CUSTOMER detail = new CUSTOMER();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.CUSTOMERs where p.ID == detail_in.ID select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.TEN = detail_in.TEN;
                        detail.HO_TEN = detail_in.HO_TEN;
                        detail.HO_DEM = detail_in.HO_DEM;
                        detail.ID_TRUONG = detail_in.ID_TRUONG;
                        detail.SDT = detail_in.SDT;
                        detail.EMAIL = detail_in.EMAIL;
                        detail.NGAY_SINH = detail_in.NGAY_SINH;
                        detail.GIOI_TINH = detail_in.GIOI_TINH;
                        detail.ID_NHOM = detail_in.ID_NHOM;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CUSTOMER");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity delete(long id, long? nguoi, bool is_delete = false)
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
                        var sql = @"update CUSTOMER set IS_DELETE = 1, NGUOI_TAO=:0, NGAY_TAO=:1 where ID = :2";
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, id);
                    }
                    else
                    {
                        var sql = @"delete from CUSTOMER_TO_CUSTOMER where ID_CUSTOMER = :0";
                        context.Database.ExecuteSqlCommand(sql, id);
                        sql = @"delete from CUSTOMER where ID = :0";
                        context.Database.ExecuteSqlCommand(sql, id);
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CUSTOMER_TO_CUSTOMER");
                QICache.RemoveByFirstName("CUSTOMER");
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
