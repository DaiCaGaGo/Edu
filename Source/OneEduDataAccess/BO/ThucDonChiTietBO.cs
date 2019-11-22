using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class ThucDonChiTietBO
    {
        #region get
        public List<THUC_DON_CHI_TIET> getThucDonChiTiet(long id_truong, short id_khoi, long id_thuc_don)
        {
            List<THUC_DON_CHI_TIET> data = new List<THUC_DON_CHI_TIET>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("THUC_DON_CHI_TIET", "getThucDonChiTiet", id_truong, id_khoi, id_thuc_don);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.THUC_DON_CHI_TIET
                            where p.ID_TRUONG == id_truong && p.ID_KHOI == id_khoi && p.ID_THUC_DON == id_thuc_don && p.IS_DELETE != true
                            orderby p.ID_NHOM_THUC_PHAM
                            select p).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<THUC_DON_CHI_TIET>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<ThucDonChiTietEntity> chiTietThucDon(long id_truong, short id_khoi, long id_thuc_don)
        {
            List<ThucDonChiTietEntity> data = new List<ThucDonChiTietEntity>();
            using (oneduEntities context = new oneduEntities())
            {
                var strQuery = string.Format(@"select td.*,tp.ten
                        ,tp.nang_luong_kcal as nang_luong_kcal_old,tp.protid as protid_old,tp.glucid as glucid_old,tp.lipid as lipid_old
                        from thuc_don_chi_tiet td 
                        left join dm_thuc_pham tp on td.id_thuc_pham = tp.id
                        where td.id_truong=:0 and td.id_khoi=:1 and td.id_thuc_don=:2 and not (td.is_delete is not null and td.is_delete=1) 
                        order by td.id_nhom_thuc_pham, tp.ten");
                data = context.Database.SqlQuery<ThucDonChiTietEntity>(strQuery, id_truong, id_khoi, id_thuc_don).ToList();
            }
            return data;
        }
        public THUC_DON_CHI_TIET getThucDonChiTietByID(long id)
        {
            THUC_DON_CHI_TIET data = new THUC_DON_CHI_TIET();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("THUC_DON_CHI_TIET", "getThucDonChiTietByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.THUC_DON_CHI_TIET where p.ID == id && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as THUC_DON_CHI_TIET;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public THUC_DON_CHI_TIET getThucDonChiTietByThucDonTruong(long id_truong, short id_khoi, short id_nhom_thuc_pham, long id_thuc_pham, long id_thuc_don)
        {
            THUC_DON_CHI_TIET data = new THUC_DON_CHI_TIET();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("THUC_DON_CHI_TIET", "getThucDonChiTietByThucDonTruong", id_truong, id_khoi, id_nhom_thuc_pham, id_thuc_pham, id_thuc_don);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.THUC_DON_CHI_TIET where p.ID_TRUONG == id_truong && p.ID_KHOI == id_khoi && p.ID_THUC_DON == id_thuc_don && p.ID_NHOM_THUC_PHAM == id_nhom_thuc_pham && p.ID_THUC_PHAM == id_thuc_pham select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as THUC_DON_CHI_TIET;
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
        public ResultEntity update(THUC_DON_CHI_TIET detail_in, long? nguoi)
        {
            THUC_DON_CHI_TIET detail = new THUC_DON_CHI_TIET();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.THUC_DON_CHI_TIET
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();

                    if (detail != null)
                    {
                        detail.ID_THUC_DON = detail_in.ID_THUC_DON;
                        detail.ID_NHOM_THUC_PHAM = detail_in.ID_NHOM_THUC_PHAM;
                        detail.ID_THUC_PHAM = detail_in.ID_THUC_PHAM;
                        detail.SO_LUONG = detail_in.SO_LUONG;
                        detail.DON_VI_TINH = detail_in.DON_VI_TINH;
                        detail.DON_GIA = detail_in.DON_GIA;
                        detail.TONG_GIA = detail_in.TONG_GIA;
                        detail.NANG_LUONG_KCAL = detail_in.NANG_LUONG_KCAL;
                        detail.PROTID = detail_in.PROTID;
                        detail.GLUCID = detail_in.GLUCID;
                        detail.LIPID = detail_in.LIPID;
                        detail.ID_TRUONG = detail_in.ID_TRUONG;
                        detail.ID_KHOI = detail_in.ID_KHOI;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            var QICache = new DefaultCacheProvider();
            QICache.RemoveByFirstName("THUC_DON_CHI_TIET");
            return res;
        }
        public ResultEntity insert(THUC_DON_CHI_TIET detail_in, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail_in.NGUOI_TAO = nguoi;
                    detail_in.NGAY_TAO = DateTime.Now;
                    detail_in = context.THUC_DON_CHI_TIET.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            var QICache = new DefaultCacheProvider();
            QICache.RemoveByFirstName("THUC_DON_CHI_TIET");
            return res;
        }
        public ResultEntity delete(long id, long? nguoi, bool is_delete = false)
        {
            THUC_DON_CHI_TIET detail = new THUC_DON_CHI_TIET();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            string sql = string.Empty;
            try
            {
                using (var context = new oneduEntities())
                {

                    if (!is_delete)
                    {
                        sql += @"UPDATE THUC_DON_CHI_TIET SET IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 WHERE ID = " + id.ToString();
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now);
                    }
                    else
                    {
                        sql += @"DELETE THUC_DON_CHI_TIET WHERE ID = " + id.ToString();
                        context.Database.ExecuteSqlCommand(sql);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }

            var QICache = new DefaultCacheProvider();
            QICache.RemoveByFirstName("THUC_DON_CHI_TIET");
            return res;
        }
        public ResultEntity deleteThucPhamInThucDon(long id_truong, long id_thuc_don, long id_nhom_thuc_pham, long id_thuc_pham, long? nguoi, bool is_delete = false)
        {
            THUC_DON_CHI_TIET detail = new THUC_DON_CHI_TIET();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            string sql = string.Empty;
            try
            {
                using (var context = new oneduEntities())
                {

                    if (!is_delete)
                    {
                        sql = @"UPDATE THUC_DON_CHI_TIET SET IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 WHERE ID_TRUONG=" + id_truong + " and ID_THUC_DON=" + id_thuc_don + " and ID_NHOM_THUC_PHAM=" + id_nhom_thuc_pham + " and ID_THUC_PHAM=" + id_thuc_pham;
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now);
                    }
                    else
                    {
                        sql = @"DELETE THUC_DON_CHI_TIET WHERE ID_TRUONG=" + id_truong + " and ID_THUC_DON=" + id_thuc_don + " and ID_NHOM_THUC_PHAM=" + id_nhom_thuc_pham + " and ID_THUC_PHAM=" + id_thuc_pham;
                        context.Database.ExecuteSqlCommand(sql);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }

            var QICache = new DefaultCacheProvider();
            QICache.RemoveByFirstName("THUC_DON_CHI_TIET");
            return res;
        }
        public ResultEntity insertOrUpdate(THUC_DON_CHI_TIET detail_in, long? nguoi)
        {
            THUC_DON_CHI_TIET detail = new THUC_DON_CHI_TIET();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.THUC_DON_CHI_TIET
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();

                    if (detail != null)
                    {
                        detail.SO_LUONG = detail_in.SO_LUONG;
                        detail.DON_VI_TINH = detail_in.DON_VI_TINH;
                        detail.DON_GIA = detail_in.DON_GIA;
                        detail.TONG_GIA = detail_in.TONG_GIA;
                        detail.NANG_LUONG_KCAL = detail_in.NANG_LUONG_KCAL;
                        detail.PROTID = detail_in.PROTID;
                        detail.GLUCID = detail_in.GLUCID;
                        detail.LIPID = detail_in.LIPID;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        detail.IS_DELETE = detail_in.IS_DELETE;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                    else
                    {
                        detail_in.ID = context.Database.SqlQuery<long>("SELECT THUC_DON_CHI_TIET_SEQ.nextval FROM SYS.DUAL").FirstOrDefault();
                        detail_in.NGAY_TAO = DateTime.Now;
                        detail_in.NGUOI_TAO = nguoi;
                        detail_in.NGAY_SUA = DateTime.Now;
                        detail_in.NGUOI_SUA = nguoi;
                        detail_in = context.THUC_DON_CHI_TIET.Add(detail_in);
                        context.SaveChanges();
                        res.ResObject = detail_in;
                    }
                }
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            var QICache = new DefaultCacheProvider();
            QICache.RemoveByFirstName("THUC_DON_CHI_TIET");
            return res;
        }
        #endregion
    }
}
