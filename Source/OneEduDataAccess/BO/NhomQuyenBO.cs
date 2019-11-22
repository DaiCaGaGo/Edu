using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class NhomQuyenBO
    {
        #region get
        public List<NHOM_QUYEN> getNhomQuyen(string ma, string ten, bool is_all = false, string ma_all = "", string text_all = "Chọn tất cả")
        {
            List<NHOM_QUYEN> data = new List<NHOM_QUYEN>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("NHOM_QUYEN", "getNhomQuyen", ma, ten, is_all, ma_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    //data = (from p in context.NHOM_QUYEN where p.IS_DELETE != true orderby p.THU_TU select p).ToList();
                    var temp = (from p in context.NHOM_QUYEN where p.IS_DELETE != true select p);
                    if (!string.IsNullOrEmpty(ma))
                        temp = temp.Where(x => x.MA.ToLower().Contains(ma.ToLower()));
                    if (!string.IsNullOrEmpty(ten))
                        temp = temp.Where(x => x.TEN.ToLower().Contains(ten.ToLower()));
                    temp = temp.OrderBy(x => x.THU_TU);
                    data = temp.ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<NHOM_QUYEN>();
                        NHOM_QUYEN item_all = new NHOM_QUYEN();
                        item_all.MA = ma_all;
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
                    data = QICache.Get(strKeyCache) as List<NHOM_QUYEN>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public NHOM_QUYEN getNhomQuyenByMa(string ma)
        {
            NHOM_QUYEN data = new NHOM_QUYEN();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("NHOM_QUYEN", "getNhomQuyenByMa", ma);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.NHOM_QUYEN where p.MA.ToLower() == ma.ToLower() && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as NHOM_QUYEN;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public NHOM_QUYEN getNhomQuyenByTen(string ten)
        {
            NHOM_QUYEN data = new NHOM_QUYEN();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("NHOM_QUYEN", "getNhomQuyenByTen", ten);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.NHOM_QUYEN where p.TEN.ToLower() == ten.ToLower() && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as NHOM_QUYEN;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public NHOM_QUYEN getNhomQuyenByMaTen(string ma, string ten)
        {
            NHOM_QUYEN data = new NHOM_QUYEN();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("NHOM_QUYEN", "getNhomQuyenByMaTen", ma, ten);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.NHOM_QUYEN where p.MA == ma && p.TEN == ten && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as NHOM_QUYEN;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public short? getMaxThuTu()
        {
            short? data = null;
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("NHOM_QUYEN", "getMaxThuTu");
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var sql = @"select MAX(THU_TU) as THU_TU from NHOM_QUYEN where NOT (IS_DELETE is not null and IS_DELETE =1 )";
                    data = context.Database.SqlQuery<short?>(sql).FirstOrDefault();
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
        public ResultEntity update(string ma, NHOM_QUYEN detail_in, long? nguoi)
        {
            NHOM_QUYEN detail = new NHOM_QUYEN();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    var sql = @"update NHOM_QUYEN set MA=:0,TEN=:1,THU_TU=:2,NGUOI_SUA=:3,NGAY_SUA=:4 where MA=:5";
                    context.Database.ExecuteSqlCommand(sql, detail_in.MA, detail_in.TEN, detail_in.THU_TU, nguoi, DateTime.Now, ma);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("NHOM_QUYEN");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(NHOM_QUYEN detail_in, long? nguoi)
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
                    detail_in = context.NHOM_QUYEN.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("NHOM_QUYEN");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity delete(string ma, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    var sql = @"update NHOM_QUYEN set IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 where MA = :2";
                    context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, ma);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("NHOM_QUYEN");
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
