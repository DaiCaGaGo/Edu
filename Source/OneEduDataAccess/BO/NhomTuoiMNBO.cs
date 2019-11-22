using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class NhomTuoiMNBO
    {
        #region get
        public List<NHOM_TUOI_MN> getNhomTuoiMN(bool is_all = false, decimal id_all = 0, string text_all = "Chọn tất cả")
        {
            List<NHOM_TUOI_MN> data = new List<NHOM_TUOI_MN>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("NHOM_TUOI_MN", "getNhomTuoiMN", is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.NHOM_TUOI_MN select p).ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<NHOM_TUOI_MN>();
                        NHOM_TUOI_MN item_all = new NHOM_TUOI_MN();
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
                    data = QICache.Get(strKeyCache) as List<NHOM_TUOI_MN>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public NHOM_TUOI_MN getNhomTuoiMNByMa(decimal ma)
        {
            NHOM_TUOI_MN data = new NHOM_TUOI_MN();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("NHOM_TUOI_MN", "getNhomTuoiMNByMa", ma);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.NHOM_TUOI_MN where p.MA == ma select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as NHOM_TUOI_MN;
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
        public ResultEntity update(decimal ma, decimal ma_new, string ten, short ma_khoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    var sql = @"Update NHOM_TUOI_MN SET MA = :0 ,TEN=:1 ,MA_KHOI=:2 WHERE MA = :3";
                    context.Database.ExecuteSqlCommand(sql, ma_new, ten, ma_khoi, ma);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("NHOM_TUOI_MN");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(decimal ma, string ten, short ma_khoi)
        {
            NHOM_TUOI_MN detail = new NHOM_TUOI_MN();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail.MA = ma;
                    detail.TEN = ten;
                    detail.MA_KHOI = ma_khoi;
                    detail = context.NHOM_TUOI_MN.Add(detail);
                    context.SaveChanges();
                }
                res.ResObject = detail;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("NHOM_TUOI_MN");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }

        public ResultEntity delete(decimal ma)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    var sql = @"delete FROM  NHOM_TUOI_MN where MA = :0";
                    context.Database.ExecuteSqlCommand(sql, ma);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("NHOM_TUOI_MN");
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
