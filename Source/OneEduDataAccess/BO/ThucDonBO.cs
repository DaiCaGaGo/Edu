using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class ThucDonBO
    {
        #region Get
        public THUC_DON getById(long id)
        {
            THUC_DON data = new THUC_DON();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("THUC_DON", "getById", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.THUC_DON
                            where p.ID == id && p.IS_DELETE != true
                            select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as THUC_DON;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<THUC_DON> getThucDonByTruongKhoiNhomTuoi(long idTruong, short? id_khoi, short? id_nhom_tuoi_mn, string ten = "")
        {
            List<THUC_DON> data = new List<THUC_DON>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("THUC_DON", "getThucDonByTruongKhoiNhomTuoi", idTruong, id_khoi, id_nhom_tuoi_mn, ten);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.THUC_DON where p.IS_DELETE != true && p.ID_TRUONG == idTruong select p);
                    if (id_khoi != null)
                        tmp = tmp.Where(x => x.ID_KHOI == id_khoi);
                    if (id_nhom_tuoi_mn != null)
                        tmp = tmp.Where(x => x.ID_NHOM_TUOI_MN == id_nhom_tuoi_mn);
                    if (!string.IsNullOrEmpty(ten))
                        tmp = tmp.Where(x => x.TEN.Contains(ten));
                    data = tmp.ToList();
                    tmp = tmp.OrderBy(x => x.ID_KHOI).ThenBy(x => x.TEN);
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<THUC_DON>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<THUC_DON> getThucDonByTruongKhoiAndBuaAn(long idTruong, short id_khoi, short? id_nhom_tuoi_mn, long id_bua_an)
        {
            List<THUC_DON> data = new List<THUC_DON>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("THUC_DON", "getThucDonByTruongKhoiAndBuaAn", idTruong, id_khoi, id_nhom_tuoi_mn, id_bua_an);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.THUC_DON where p.ID_TRUONG == idTruong && p.ID_KHOI == id_khoi && p.ID_BUA_AN == id_bua_an && p.IS_DELETE != true select p);
                    if (id_nhom_tuoi_mn != null)
                        tmp = tmp.Where(x => x.ID_NHOM_TUOI_MN == id_nhom_tuoi_mn);
                    data = tmp.ToList();
                    tmp = tmp.OrderBy(x => x.ID_KHOI).ThenBy(x => x.TEN);
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<THUC_DON>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public THUC_DON checkExists(long id_truong, short id_khoi, long id_bua_an, string ten)
        {
            THUC_DON data = new THUC_DON();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("THUC_DON", "checkExists", id_truong, id_khoi, id_bua_an, ten);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.THUC_DON
                            where p.ID_TRUONG == id_truong && p.ID_KHOI == id_khoi && p.ID_BUA_AN == id_bua_an && p.TEN.ToLower() == ten.ToLower() && p.IS_DELETE != true
                            select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as THUC_DON;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        #endregion
        #region Set
        public ResultEntity update(THUC_DON detail_in, long? nguoi)
        {
            THUC_DON detail = new THUC_DON();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.THUC_DON
                              where p.ID == detail_in.ID && p.IS_DELETE != true
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.ID_TRUONG = detail_in.ID_TRUONG;
                        detail.ID_KHOI = detail_in.ID_KHOI;
                        detail.ID_BUA_AN = detail_in.ID_BUA_AN;
                        detail.TEN = detail_in.TEN;
                        detail.SO_HOC_SINH_DK = detail_in.SO_HOC_SINH_DK;
                        detail.ID_NHOM_TUOI_MN = detail_in.ID_NHOM_TUOI_MN;
                        detail.HAN_MUC_GIA = detail_in.HAN_MUC_GIA;
                        detail.TONG_NANG_LUONG_KCAL = detail_in.TONG_NANG_LUONG_KCAL;
                        detail.TONG_PROTID = detail_in.TONG_PROTID;
                        detail.TONG_GLUCID = detail_in.TONG_GLUCID;
                        detail.TONG_LIPID = detail_in.TONG_LIPID;
                        detail.GHI_CHU = detail_in.GHI_CHU;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("THUC_DON");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(THUC_DON detail_in, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail_in.ID = context.Database.SqlQuery<long>("SELECT THUC_DON_SEQ.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                    detail_in.NGAY_TAO = DateTime.Now;
                    detail_in.NGUOI_TAO = nguoi;
                    detail_in.NGAY_SUA = DateTime.Now;
                    detail_in.NGUOI_SUA = nguoi;
                    detail_in = context.THUC_DON.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("THUC_DON");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity delete(long id, long? nguoi, bool is_delete_all, bool is_delete = false)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            string sql = string.Empty;
            try
            {
                using (var context = new oneduEntities())
                {
                    if (is_delete_all)
                    {
                        sql = "DELETE FROM THUC_DON_CHI_TIET WHERE ID_THUC_DON=:0";
                        context.Database.ExecuteSqlCommand(sql, id);
                        sql = "delete from THUC_DON where ID = :0";
                        context.Database.ExecuteSqlCommand(sql, id);
                    }
                    else if (!is_delete)
                    {
                        sql = @"update THUC_DON set IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 where ID = :2";
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, id);
                    }
                    else
                    {
                        sql = "delete from THUC_DON where ID = :0";
                        context.Database.ExecuteSqlCommand(sql, id);
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("THUC_DON_CHI_TIET");
                QICache.RemoveByFirstName("THUC_DON");
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
