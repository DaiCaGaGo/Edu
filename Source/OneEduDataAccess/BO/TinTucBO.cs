using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class TinTucBO
    {
        #region get
        public List<TIN_TUC> getTinTucByTruong(long id_truong, string ma_cap_hoc, short id_nam_hoc, string noi_dung)
        {
            List<TIN_TUC> data = new List<TIN_TUC>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TIN_TUC", "getTinTucByTruong", id_truong, ma_cap_hoc, id_nam_hoc, noi_dung);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.TIN_TUC where p.ID_TRUONG == id_truong &&p.MA_CAP_HOC== ma_cap_hoc && p.ID_NAM_HOC == id_nam_hoc && p.IS_DELETE !=true select p);
                    if (!string.IsNullOrEmpty(noi_dung))
                        tmp = tmp.Where(x => x.TIEU_DE.ToLower().Contains(noi_dung.ToLower()) ||
                                            x.NOI_DUNG_TOM_TAT.ToLower().Contains(noi_dung.ToLower()));
                    tmp = tmp.OrderBy(x => x.THU_TU).ThenByDescending(x => x.NGAY_TAO);
                    data = tmp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<TIN_TUC>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public TIN_TUC getTinTucByID(long id)
        {
            TIN_TUC data = new TIN_TUC();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TIN_TUC", "getTinTucByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.TIN_TUC where p.ID == id select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as TIN_TUC;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public long? getMaxThuTuByTruong(long id_truong, string ma_cap_hoc, short id_nam_hoc)
        {
            long? data = null;
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TIN_TUC", "getMaxThuTuByTruong", id_truong, ma_cap_hoc, id_nam_hoc);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var sql = @"select MAX(THU_TU) as THU_TU from TIN_TUC 
                                    where ID_TRUONG=:0 and MA_CAP_HOC=:1 and ID_NAM_HOC=:2";
                    data = context.Database.SqlQuery<long?>(sql, id_truong, ma_cap_hoc, id_nam_hoc).FirstOrDefault();
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
        public ResultEntity update(TIN_TUC detail_in, long? nguoi)
        {
            TIN_TUC detail = new TIN_TUC();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.TIN_TUC where p.ID == detail_in.ID select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.ID_TRUONG = detail_in.ID_TRUONG;
                        detail.MA_CAP_HOC = detail_in.MA_CAP_HOC;
                        detail.ID_NAM_HOC = detail_in.ID_NAM_HOC;
                        detail.ANH_DAI_DIEN = detail_in.ANH_DAI_DIEN;
                        detail.TIEU_DE = detail_in.TIEU_DE;
                        detail.NOI_DUNG_TOM_TAT = detail_in.NOI_DUNG_TOM_TAT;
                        detail.LINK = detail_in.LINK;
                        detail.NGAY_SU_KIEN = detail_in.NGAY_SU_KIEN;
                        detail.NGAY_HIEU_LUC = detail_in.NGAY_HIEU_LUC;
                        detail.THU_TU = detail_in.THU_TU;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("TIN_TUC");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(TIN_TUC detail_in, long? nguoi)
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
                    detail_in = context.TIN_TUC.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("TIN_TUC");
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
                        var sql = @"update TIN_TUC set IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 where ID=:2";
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, id);
                    }
                    else
                    {
                        var sql = @"delete from TIN_TUC where ID=:0";
                        context.Database.ExecuteSqlCommand(sql, id);
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("TIN_TUC");
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
