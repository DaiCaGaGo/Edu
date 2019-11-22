using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class DMSachNoiDungBO
    {
        #region get
        public List<DM_SACH_NOI_DUNG> getNoiDungSach(short? id_khoi, short? id_mon_hoc, long? id_sach, string ten)
        {
            List<DM_SACH_NOI_DUNG> data = new List<DM_SACH_NOI_DUNG>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_SACH_NOI_DUNG", "getNoiDungSach", id_khoi, id_mon_hoc, id_sach, ten);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var temp = (from p in context.DM_SACH_NOI_DUNG where p.IS_DELETE != true select p);
                    if (id_khoi != null) temp = temp.Where(p => p.ID_KHOI == id_khoi);
                    if (id_mon_hoc != null) temp = temp.Where(p => p.ID_MON_HOC == id_mon_hoc);
                    if (id_sach != null) temp = temp.Where(p => p.ID_SACH == id_sach);
                    if (!string.IsNullOrEmpty(ten)) temp = temp.Where(p => p.TEN_BAI_HOC.ToLower().Contains(ten.ToLower()));
                    temp = temp.OrderBy(p => p.ID_SACH).ThenBy(p => p.THU_TU);
                    data = temp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<DM_SACH_NOI_DUNG>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<DM_SACH_NOI_DUNG> getNoiDungBySach(short id_khoi, short id_mon_hoc, long id_sach)
        {
            List<DM_SACH_NOI_DUNG> data = new List<DM_SACH_NOI_DUNG>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_SACH_NOI_DUNG", "getNoiDungBySach", id_khoi, id_mon_hoc, id_sach);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var temp = (from p in context.DM_SACH_NOI_DUNG where p.ID_KHOI == id_khoi && p.ID_MON_HOC== id_mon_hoc && p.ID_SACH == id_sach && p.IS_DELETE != true select p);
                    temp = temp.OrderBy(p => p.ID_SACH).ThenBy(p => p.THU_TU);
                    data = temp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<DM_SACH_NOI_DUNG>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public DM_SACH_NOI_DUNG getNoiDungSachByID(long id)
        {
            DM_SACH_NOI_DUNG data = new DM_SACH_NOI_DUNG();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_SACH_NOI_DUNG", "getNoiDungSachByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DM_SACH_NOI_DUNG where p.ID == id select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as DM_SACH_NOI_DUNG;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public long? getMaxThuTuBySach(short id_khoi, short id_mon_hoc, long id_sach)
        {
            long? data = null;
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_SACH_NOI_DUNG", "getMaxThuTuBySach", id_khoi, id_mon_hoc, id_sach);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var sql = @"select MAX(THU_TU) as THU_TU from DM_SACH_NOI_DUNG 
                                    where ID_KHOI=:0 and ID_MON_HOC=:1 and ID_SACH=:2";
                    data = context.Database.SqlQuery<long?>(sql, id_khoi, id_mon_hoc, id_sach).FirstOrDefault();
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
        public ResultEntity update(DM_SACH_NOI_DUNG detail_in, long? nguoi)
        {
            DM_SACH_NOI_DUNG detail = new DM_SACH_NOI_DUNG();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.DM_SACH_NOI_DUNG
                              where p.ID == detail_in.ID && p.IS_DELETE != true
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.ID_SACH = detail_in.ID_SACH;
                        detail.ID_MON_HOC = detail_in.ID_MON_HOC;
                        detail.ID_KHOI = detail_in.ID_KHOI;
                        detail.TEN_BAI_HOC = detail_in.TEN_BAI_HOC;
                        detail.SO_TRANGS = detail_in.SO_TRANGS;
                        detail.GHI_CHU = detail_in.GHI_CHU;
                        detail.THU_TU = detail_in.THU_TU;
                        detail.NGAY_TAO = DateTime.Now;
                        detail.NGUOI_TAO = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DM_SACH_NOI_DUNG");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(DM_SACH_NOI_DUNG detail_in, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail_in.ID = context.Database.SqlQuery<long>("SELECT DM_SACH_NOI_DUNG_SEQ.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                    detail_in.NGAY_TAO = DateTime.Now;
                    detail_in.NGUOI_TAO = nguoi;
                    detail_in = context.DM_SACH_NOI_DUNG.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DM_SACH_NOI_DUNG");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity delete(long id)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    var sql = @"delete DM_SACH_NOI_DUNG where ID = :0";
                    context.Database.ExecuteSqlCommand(sql, id);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DM_SACH_NOI_DUNG");
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
