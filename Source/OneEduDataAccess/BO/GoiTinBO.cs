using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class GoiTinBO
    {
        #region get
        public List<GOI_TIN> getGoiTin(bool is_all = false, short id_all = 0, string text_all = "Chọn tất cả")
        {
            List<GOI_TIN> data = new List<GOI_TIN>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("GOI_TIN", "getGoiTin", is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.GOI_TIN where p.IS_DELETE != true orderby p.THU_TU select p).ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<GOI_TIN>();
                        GOI_TIN item_all = new GOI_TIN();
                        item_all.MA = id_all;
                        item_all.GHI_CHU = text_all;
                        data.Insert(0, item_all);
                    }
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<GOI_TIN>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public GOI_TIN getGoiTinByMa(short ma)
        {
            GOI_TIN data = new GOI_TIN();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("GOI_TIN", "getGoiTinByMa", ma);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.GOI_TIN where p.MA == ma select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as GOI_TIN;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public GOI_TIN getGoiTinByTen(string ten)
        {
            GOI_TIN data = new GOI_TIN();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("GOI_TIN", "getGoiTinByTen", ten);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.GOI_TIN where p.TEN == ten && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as GOI_TIN;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<GOI_TIN> getListGoiTinByTen(string ten, bool is_all = false, short id_all = 0, string text_all = "Chọn tất cả")
        {
            List<GOI_TIN> data = new List<GOI_TIN>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("GOI_TIN", "getGoiTinByTen", ten, is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.GOI_TIN where p.IS_DELETE != true select p);
                    if (!string.IsNullOrEmpty(ten))
                    {
                        tmp = tmp.Where(x => x.TEN.ToLower().Contains(ten.ToLower()));
                    }
                    tmp = tmp.OrderBy(x => x.THU_TU);
                    data = tmp.ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<GOI_TIN>();
                        GOI_TIN item_all = new GOI_TIN();
                        item_all.MA = id_all;
                        item_all.GHI_CHU = text_all;
                        data.Insert(0, item_all);
                    }
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<GOI_TIN>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public long? getMaxThuTu()
        {
            long? data = null;
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("GOI_TIN", "getMaxThuTu");
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var sql = @"select MAX(THU_TU) as THU_TU from GOI_TIN";
                    data = context.Database.SqlQuery<long?>(sql).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as long?;
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
        public ResultEntity update(GOI_TIN detail_in, long? nguoi)
        {
            GOI_TIN detail = new GOI_TIN();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.GOI_TIN where p.MA == detail_in.MA select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.TEN = detail_in.TEN;
                        detail.GHI_CHU = detail_in.GHI_CHU;
                        detail.SO_TIN_LIEN_LAC_HS = detail_in.SO_TIN_LIEN_LAC_HS;
                        detail.SO_TIN_THONG_BAO_HS = detail_in.SO_TIN_THONG_BAO_HS;
                        detail.SO_TIN_HE_HS = detail_in.SO_TIN_HE_HS;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("GOI_TIN");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(GOI_TIN detail_in, long? nguoi)
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
                    detail_in = context.GOI_TIN.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("GOI_TIN");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity delete(short ma, long? nguoi, bool is_delete = false)
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
                        var sql = @"update GOI_TIN set IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 where MA = :2";
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, ma);
                    }
                    else
                    {
                        var sql = @"delete from GOI_TIN where MA = :0";
                        context.Database.ExecuteSqlCommand(sql, ma);
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("GOI_TIN");
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
