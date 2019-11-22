using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class GVLopMonBO
    {
        #region get
        public List<GV_LOP_MON> getGVLopMon()
        {
            List<GV_LOP_MON> data = new List<GV_LOP_MON>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("GV_LOP_MON", "getGVLopMon");
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.GV_LOP_MON where p.IS_DELETE != true select p).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<GV_LOP_MON>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public GV_LOP_MON getGVLopMonByID(long id)
        {
            GV_LOP_MON data = new GV_LOP_MON();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("GV_LOP_MON", "getGVLopMonByID", id);

            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.GV_LOP_MON where p.ID == id select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as GV_LOP_MON;
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
        public ResultEntity update(long id,short? trang_thai,long? nguoi)
        {
            GV_LOP_MON detail = new GV_LOP_MON();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.GV_LOP_MON
                              where p.ID == id
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.TRANG_THAI = trang_thai;
                        detail.NGUOI_SUA = nguoi;
                        detail.NGAY_SUA = DateTime.Now;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("GV_LOP_MON");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(long id_gv, long id_lop, short id_mon, short id_hocky,  long? nguoi, bool is_delete)
        {
            GV_LOP_MON detail = new GV_LOP_MON();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail.ID_GIAO_VIEN = id_gv;
                    detail.ID_LOP = id_lop;
                    detail.ID_MON = id_mon;
                    detail.NGAY_TAO = DateTime.Now;
                    detail.NGUOI_TAO = nguoi;
                    detail.IS_DELETE = is_delete;
                    detail = context.GV_LOP_MON.Add(detail);
                    context.SaveChanges();
                }
                res.ResObject = detail;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("GV_LOP_MON");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity delete(long id, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    var sql = @"update GV_LOP_MON set IS_DELETE = 1,NGUOI_SUA=:0,NGAY_SUA=:1 where ID = :2";
                    context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, id);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("GV_LOP_MON");
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
