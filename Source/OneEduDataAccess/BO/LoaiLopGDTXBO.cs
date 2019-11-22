using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class LoaiLopGDTXBO
    {
        #region get
        public List<LOAI_LOP_GDTX> getLoaiLopGDTX(bool is_all = false, short id_all = 0, string text_all = "Chọn tất cả")
        {
            List<LOAI_LOP_GDTX> data = new List<LOAI_LOP_GDTX>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("LOAI_LOP_GDTX", "getLoaiLopGDTX", is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.LOAI_LOP_GDTX where p.IS_DELETE != true select p).ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<LOAI_LOP_GDTX>();
                        LOAI_LOP_GDTX item_all = new LOAI_LOP_GDTX();
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
                    data = QICache.Get(strKeyCache) as List<LOAI_LOP_GDTX>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public LOAI_LOP_GDTX getLoaiLopGDTXByMa(short ma)
        {
            LOAI_LOP_GDTX data = new LOAI_LOP_GDTX();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("LOAI_LOP_GDTX", "getLoaiLopGDTXByMa", ma);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.LOAI_LOP_GDTX where p.MA == ma && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as LOAI_LOP_GDTX;
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
        public ResultEntity update(short ma, short ma_new, string ten, long? nguoi, bool is_delete)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    var sql = @"Update LOAI_LOP_GDTX SET MA = :0, TEN = :1, NGAY_SUA=:2, NGUOI_SUA=:3, IS_DELETE=:4 WHERE MA = :5";
                    context.Database.ExecuteSqlCommand(sql, ma_new, ten, DateTime.Now, nguoi, is_delete ? 1 : 0, ma);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("LOAI_LOP_GDTX");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(string ten, long nguoi, bool is_delete)
        {
            LOAI_LOP_GDTX detail = new LOAI_LOP_GDTX();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail.TEN = ten;
                    detail.NGAY_TAO = DateTime.Now;
                    detail.NGUOI_TAO = nguoi;
                    detail.IS_DELETE = is_delete;
                    detail = context.LOAI_LOP_GDTX.Add(detail);
                    context.SaveChanges();
                }
                res.ResObject = detail;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("LOAI_LOP_GDTX");
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
