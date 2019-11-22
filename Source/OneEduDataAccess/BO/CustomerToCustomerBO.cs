using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class CustomerToCustomerBO
    {
        #region get
        public List<CUSTOMER_TO_CUSTOMER> getListCustomerInTo(long id_truong, short id_to)
        {
            List<CUSTOMER_TO_CUSTOMER> data = new List<CUSTOMER_TO_CUSTOMER>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CUSTOMER_TO_CUSTOMER", "getListCustomerInTo", id_truong, id_to);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.CUSTOMER_TO_CUSTOMER
                            where p.ID_TRUONG == id_truong && p.ID_TO == id_to && p.IS_DELETE != true
                            select p).ToList();

                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<CUSTOMER_TO_CUSTOMER>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<CustomerEntity> getCustomerExistsTo(long id_truong, short? id_to)
        {
            List<CustomerEntity> data = new List<CustomerEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CUSTOMER", "CUSTOMER_TO_CUSTOMER", "getCustomerExistsTo", id_truong, id_to);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = @"SELECT ID, TEN, HO_TEN, SDT FROM CUSTOMER WHERE ID_TRUONG=:0 AND EXISTS 
                    (SELECT CUSTOMER_TO_CUSTOMER.ID_CUSTOMER FROM CUSTOMER_TO_CUSTOMER WHERE CUSTOMER_TO_CUSTOMER.ID_TO=:1 
                    AND CUSTOMER_TO_CUSTOMER.ID_CUSTOMER = CUSTOMER.ID AND (IS_DELETE = 0 OR IS_DELETE IS NULL))";
                    data = context.Database.SqlQuery<CustomerEntity>(strQuery, id_truong, id_to).ToList();
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
        public CUSTOMER_TO_CUSTOMER checkCustomerInTo(long id_truong, short id_to, long id_customer)
        {
            CUSTOMER_TO_CUSTOMER data = new CUSTOMER_TO_CUSTOMER();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CUSTOMER_TO_CUSTOMER", "checkCustomerInTo", id_truong, id_to, id_customer);
            try
            {
                if (!QICache.IsSet(strKeyCache))
                {
                    using (oneduEntities context = new oneduEntities())
                    {
                        data = (from p in context.CUSTOMER_TO_CUSTOMER
                                where p.ID_TRUONG == id_truong && p.ID_TO == id_to && p.ID_CUSTOMER == id_customer && p.IS_DELETE != true
                                select p).FirstOrDefault();
                        QICache.Set(strKeyCache, data, 300000);
                    }
                }
                else
                {
                    try
                    {
                        data = QICache.Get(strKeyCache) as CUSTOMER_TO_CUSTOMER;
                    }
                    catch
                    {
                        QICache.Invalidate(strKeyCache);
                    }
                }
            }
            catch (Exception ex) { }
            return data;
        }
        public List<DSGiaoVienTheoToEntity> getCustomerByTo(long id_truong, List<short> lst_ma_to)
        {
            DataAccessAPI dataAccessAPI = new DataAccessAPI();
            List<DSGiaoVienTheoToEntity> data = new List<DSGiaoVienTheoToEntity>();
            string str_lst_ma_to = dataAccessAPI.ConvertListToString<short>(lst_ma_to, ",");
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CUSTOMER", "CUSTOMER_TO_CUSTOMER", "TIN_NHAN", "getCustomerByTo", id_truong, str_lst_ma_to);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = "";
                    string strTemp = "";
                    if (lst_ma_to != null && lst_ma_to.Count > 0)
                    {
                        if (lst_ma_to.Count == 1)
                        {
                            strTemp = string.Format(@"select * from customer_to_customer ct where c.ID=ct.id_customer and ct.ID_TO={0}", lst_ma_to[0]);
                        }
                        else if (lst_ma_to.Count > 1)
                        {
                            strTemp = string.Format(@"select tgv.* from customer_to_customer ct where c.ID=ct.id_customer and not (ct.ID_TO !={0}", lst_ma_to[0]);
                            for (int i = 1; i < lst_ma_to.Count; i++)
                            {
                                strTemp += " and ct.ID_TO != " + lst_ma_to[i];
                            }
                            strTemp += ")";
                        }
                        strQuery = @"select c.id, c.ten, c.ho_ten, c.sdt,case when sum(tn.so_tin)>0 then sum(tn.so_tin) else 0 end as SO_TIN_TRONG_NGAY
                            from customer c
                            left join tin_nhan tn on c.id = tn.id_nguoi_nhan and c.id_truong = tn.id_truong 
                            and tn.loai_nguoi_nhan = 2 and tn.loai_tin = 2 and TRUNC(tn.ngay_tao)=TRUNC(:0)
                            where exists (" + strTemp;
                        strQuery += string.Format(@") and c.id_truong = {0} group by c.id, c.ten, c.ho_ten, c.sdt
                            order by c.ten", id_truong);
                        data = context.Database.SqlQuery<DSGiaoVienTheoToEntity>(strQuery, DateTime.Now).ToList();
                    }
                    else
                    {
                        #region "hiển thị ds giáo viên trong trường"
                        strQuery = @"select c.id, c.ho_ten, c.ten, c.sdt
                            ,case when sum(tn.so_tin)>0 then sum(tn.so_tin) else 0 end as SO_TIN_TRONG_NGAY
                            from customer c
                            left join tin_nhan tn on c.id=tn.id_nguoi_nhan and c.id_truong=tn.id_truong 
                            and tn.loai_nguoi_nhan=2 and tn.loai_tin=2 and TRUNC(tn.ngay_tao)=TRUNC(:0)
                            where c.id_truong=:1 group by c.id, c.ho_ten, c.ten, c.sdt order by c.ho_ten";
                        data = context.Database.SqlQuery<DSGiaoVienTheoToEntity>(strQuery, DateTime.Now, id_truong).ToList();
                        #endregion
                    }
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<DSGiaoVienTheoToEntity>;
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
        public ResultEntity insertOrUpdate(CUSTOMER_TO_CUSTOMER detail_in, long? nguoi)
        {
            CUSTOMER_TO_CUSTOMER detail = new CUSTOMER_TO_CUSTOMER();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.CUSTOMER_TO_CUSTOMER
                              where p.ID_TO == detail_in.ID_TO && p.ID_TRUONG == detail_in.ID_TRUONG && p.ID_CUSTOMER == detail_in.ID_CUSTOMER
                              select p).FirstOrDefault();

                    if (detail == null)
                    {
                        detail_in.ID = context.Database.SqlQuery<long>("SELECT CUSTOMER_TO_CUSTOMER_SEQ.nextval FROM SYS.DUAL").FirstOrDefault();
                        detail_in.NGAY_TAO = DateTime.Now;
                        detail_in.NGUOI_TAO = nguoi;
                        detail_in.NGAY_SUA = DateTime.Now;
                        detail_in.NGUOI_SUA = nguoi;
                        detail_in = context.CUSTOMER_TO_CUSTOMER.Add(detail_in);
                        context.SaveChanges();
                        res.ResObject = detail_in;
                    }
                    else
                    {
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CUSTOMER_TO_CUSTOMER");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity delete(long id_truong, short id_to, long id_customer, long? nguoi, bool is_delete = false)
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
                        var sql = @"update CUSTOMER_TO_CUSTOMER set IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 where ID_TRUONG=:2 and ID_TO=:3 and ID_CUSTOMER=:4";
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, id_truong, id_to, id_customer);
                    }
                    else
                    {
                        var sql = @"delete CUSTOMER_TO_CUSTOMER where ID_TRUONG=:0 and ID_TO=:1 and ID_CUSTOMER=:2";
                        context.Database.ExecuteSqlCommand(sql, id_truong, id_to, id_customer);
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CUSTOMER_TO_CUSTOMER");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(CUSTOMER_TO_CUSTOMER detail_in, long? nguoi)
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
                    detail_in = context.CUSTOMER_TO_CUSTOMER.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CUSTOMER_TO_CUSTOMER");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity update(CUSTOMER_TO_CUSTOMER detail_in, long? nguoi)
        {
            CUSTOMER_TO_CUSTOMER detail = new CUSTOMER_TO_CUSTOMER();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.CUSTOMER_TO_CUSTOMER where p.ID == detail_in.ID select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.ID_TRUONG = detail_in.ID_TRUONG;
                        detail.ID_TO = detail_in.ID_TO;
                        detail.ID_CUSTOMER = detail_in.ID_CUSTOMER;
                        detail.IS_DELETE = detail_in.IS_DELETE;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CUSTOMER_TO_CUSTOMER");
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
