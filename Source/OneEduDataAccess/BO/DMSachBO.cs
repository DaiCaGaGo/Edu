using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class DMSachBO
    {
        #region get
        public List<DM_SACH> getListSachByKhoiCapHoc(string ma_cap_hoc, short? id_khoi, short? id_mon_hoc, string ten)
        {
            List<DM_SACH> data = new List<DM_SACH>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_SACH", "getListSachByKhoiCapHoc", ma_cap_hoc, id_khoi, id_mon_hoc, ten);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string sql = "select * from DM_SACH where not (is_delete is not null and IS_DELETE=1)";

                    if (id_khoi != null) sql += " and ID_KHOI=" + id_khoi;
                    else
                    {
                        if (ma_cap_hoc == SYS_Cap_Hoc.MN) sql += " and (ID_KHOI=13 or ID_KHOI=14)";
                        else if (ma_cap_hoc == SYS_Cap_Hoc.TH) sql += " and (ID_KHOI=1 or ID_KHOI=2 or ID_KHOI=3 or ID_KHOI=4 or ID_KHOI=5)";
                        else if (ma_cap_hoc == SYS_Cap_Hoc.THCS) sql += " and (ID_KHOI=6 or ID_KHOI=7 or ID_KHOI=8 or ID_KHOI=9)";
                        else if (ma_cap_hoc == SYS_Cap_Hoc.THPT) sql += " and (ID_KHOI=10 or ID_KHOI=11 or ID_KHOI=12)";
                    }

                    if (id_mon_hoc != null) sql += " and ID_MON_HOC=" + id_mon_hoc;
                    if (!string.IsNullOrEmpty(ten)) sql += string.Format(@" and lower(TEN) like '%{0}%'", ten.ToLower());
                    data = context.Database.SqlQuery<DM_SACH>(sql).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<DM_SACH>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<DM_SACH> getListSachByKhoiMonHoc(short id_khoi, short id_mon_hoc)
        {
            List<DM_SACH> data = new List<DM_SACH>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_SACH", "getListSachByKhoiMonHoc", id_khoi, id_mon_hoc);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DM_SACH where p.ID_KHOI == id_khoi && p.ID_MON_HOC == id_mon_hoc && p.IS_DELETE != true select p).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<DM_SACH>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public DM_SACH getDMSachByID(long id)
        {
            DM_SACH data = new DM_SACH();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_SACH", "getDMSachByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DM_SACH where p.ID == id select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as DM_SACH;
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
        public ResultEntity update(DM_SACH detail_in, long? nguoi)
        {
            DM_SACH detail = new DM_SACH();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.DM_SACH
                              where p.ID == detail_in.ID && p.IS_DELETE != true
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.NAM_HOC = detail_in.NAM_HOC;
                        detail.ID_MON_HOC = detail_in.ID_MON_HOC;
                        detail.ID_KHOI = detail_in.ID_KHOI;
                        detail.HOC_KY = detail_in.HOC_KY;
                        detail.TEN = detail_in.TEN;
                        detail.GHI_CHU = detail_in.GHI_CHU;
                        detail.NGAY_TAO = DateTime.Now;
                        detail.NGUOI_TAO = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DM_SACH");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(DM_SACH detail_in, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail_in.ID = context.Database.SqlQuery<long>("SELECT DM_SACH_SEQ.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                    detail_in.NGAY_TAO = DateTime.Now;
                    detail_in.NGUOI_TAO = nguoi;
                    detail_in = context.DM_SACH.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DM_SACH");
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
                    var sql = @"delete DM_SACH where ID = :0";
                    context.Database.ExecuteSqlCommand(sql, id);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DM_SACH");
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
