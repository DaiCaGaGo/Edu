using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class KhoThucPhamBO
    {
        #region get
        public List<KhoThucPhamEntity> getKhoThucPham(long id_truong, short? id_nhom_thuc_pham, string ten_thuc_pham)
        {
            List<KhoThucPhamEntity> data = new List<KhoThucPhamEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("KHO_THUC_PHAM", "DM_NHOM_THUC_PHAM", "DM_THUC_PHAM", "getKhoThucPham", id_truong, id_nhom_thuc_pham, ten_thuc_pham);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var strQuery = string.Format(@"select k.*, n.ten as ten_nhom_thuc_pham, t.ten as ten_thuc_pham from kho_thuc_pham k 
join dm_nhom_thuc_pham n on n.id=k.id_nhom_thuc_pham
join dm_thuc_pham t on t.id_nhom_thuc_pham=k.id_nhom_thuc_pham and t.id=k.id_thuc_pham
where k.id_truong=:0 and not (k.is_delete is not null and k.is_delete=1)");
                    if (id_nhom_thuc_pham != null)
                        strQuery += @" and k.id_nhom_thuc_pham=" + id_nhom_thuc_pham;
                    if (!string.IsNullOrEmpty(ten_thuc_pham))
                        strQuery += string.Format(@" and lower(t.ten) like '%{0}%'", ten_thuc_pham.ToNormalizeLower());
                    strQuery += " order by n.thu_tu,n.ten,t.thu_tu,t.ten";
                    data = context.Database.SqlQuery<KhoThucPhamEntity>(strQuery, id_truong).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<KhoThucPhamEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<KhoThucPhamEntity> getThucPhamTrongKhoByNhom(short id_nhom)
        {
            List<KhoThucPhamEntity> data = new List<KhoThucPhamEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("KHO_THUC_PHAM", "DM_THUC_PHAM", "getThucPhamTrongKhoByNhom", id_nhom);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var strQuery = string.Format(@"select k.id_nhom_thuc_pham, k.id_thuc_pham, tp.ten as TEN_THUC_PHAM 
                        from kho_thuc_pham k 
                        left join dm_thuc_pham tp on k.id_nhom_thuc_pham = tp.id_nhom_thuc_pham and k.id_thuc_pham=tp.id
                        where not (k.is_delete is not null and k.is_delete=1) and k.id_nhom_thuc_pham=:0");
                    data = context.Database.SqlQuery<KhoThucPhamEntity>(strQuery, id_nhom).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<KhoThucPhamEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public KHO_THUC_PHAM getThucPhamTrongKhoByNhomAndThucPham(long id_truong, short id_nhom_thuc_pham, long id_thuc_pham)
        {
            KHO_THUC_PHAM data = new KHO_THUC_PHAM();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("KHO_THUC_PHAM", "getThucPhamTrongKhoByNhomAndThucPham", id_truong, id_nhom_thuc_pham, id_thuc_pham);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.KHO_THUC_PHAM where p.ID_TRUONG == id_truong && p.ID_NHOM_THUC_PHAM == id_nhom_thuc_pham && p.ID_THUC_PHAM == id_thuc_pham && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as KHO_THUC_PHAM;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public KHO_THUC_PHAM getKhoThucPhamByThucPham(long id_truong, long id_thuc_pham)
        {
            KHO_THUC_PHAM data = new KHO_THUC_PHAM();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("KHO_THUC_PHAM", "getKhoThucPhamByThucPham", id_truong, id_thuc_pham);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.KHO_THUC_PHAM where p.ID_TRUONG == id_truong && p.ID_THUC_PHAM == id_thuc_pham && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as KHO_THUC_PHAM;
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
        public ResultEntity update(KHO_THUC_PHAM detail_in, long? nguoi)
        {
            KHO_THUC_PHAM detail = new KHO_THUC_PHAM();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.KHO_THUC_PHAM
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();

                    if (detail != null)
                    {
                        detail.ID_NHOM_THUC_PHAM = detail_in.ID_NHOM_THUC_PHAM;
                        detail.ID_THUC_PHAM = detail_in.ID_THUC_PHAM;
                        detail.DON_VI_TINH = detail_in.DON_VI_TINH;
                        detail.SO_LUONG = detail_in.SO_LUONG;
                        detail.TONG_GIA = detail_in.TONG_GIA;
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
            QICache.RemoveByFirstName("KHO_THUC_PHAM");
            return res;
        }
        public ResultEntity insert(KHO_THUC_PHAM detail_in, long? nguoi)
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
                    detail_in = context.KHO_THUC_PHAM.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("KHO_THUC_PHAM");
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
            string sql = string.Empty;
            try
            {
                using (var context = new oneduEntities())
                {

                    if (is_delete)
                    {
                        sql = @"DELETE KHO_THUC_PHAM WHERE ID=" + id.ToString();
                        int resKQ = context.Database.ExecuteSqlCommand(sql);
                    }
                    else
                    {
                        sql = @"UPDATE KHO_THUC_PHAM SET IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 WHERE ID=" + id.ToString();
                        int resKQ = context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }

            var QICache = new DefaultCacheProvider();
            QICache.RemoveByFirstName("KHO_THUC_PHAM");
            return res;
        }
        public ResultEntity cap_nhat_kho_thuc_pham(PHIEU_NHAP_KHO_DETAIL phieu_detail, decimal? luong_them_moi, decimal? luong_bot, long? nguoi)
        {
            KHO_THUC_PHAM detail = new KHO_THUC_PHAM();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.KHO_THUC_PHAM
                              where p.ID_TRUONG == phieu_detail.ID_TRUONG && p.ID_NHOM_THUC_PHAM == phieu_detail.ID_NHOM_THUC_PHAM && p.ID_THUC_PHAM == phieu_detail.ID_THUC_PHAM && p.IS_DELETE != true
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        if (luong_them_moi != null)
                            detail.SO_LUONG = detail.SO_LUONG != null ? (detail.SO_LUONG + luong_them_moi) : luong_them_moi;
                        else if (luong_bot != null)
                            detail.SO_LUONG = detail.SO_LUONG != null ? (detail.SO_LUONG - luong_bot) : ((-1) * luong_bot);
                        detail.DON_GIA = phieu_detail.DON_GIA;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                    else
                    {
                        detail = new KHO_THUC_PHAM();
                        long newID = context.Database.SqlQuery<long>("SELECT KHO_THUC_PHAM_SEQ.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                        detail.ID_TRUONG = phieu_detail.ID_TRUONG;
                        detail.ID_NHOM_THUC_PHAM = phieu_detail.ID_NHOM_THUC_PHAM;
                        detail.ID_THUC_PHAM = phieu_detail.ID_THUC_PHAM;
                        detail.DON_VI_TINH = phieu_detail.DON_VI_TINH;
                        detail.SO_LUONG = phieu_detail.SO_LUONG;
                        detail.DON_GIA = phieu_detail.DON_GIA;
                        detail.NGAY_TAO = DateTime.Now;
                        detail.NGUOI_TAO = nguoi;
                        detail = context.KHO_THUC_PHAM.Add(detail);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            var QICache = new DefaultCacheProvider();
            QICache.RemoveByFirstName("KHO_THUC_PHAM");
            return res;
        }
        public ResultEntity xuat_kho_thuc_pham(PHIEU_XUAT_KHO_DETAIL phieu_detail, decimal? luong_them_moi, decimal? luong_bot, long? nguoi)
        {
            KHO_THUC_PHAM detail = new KHO_THUC_PHAM();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.KHO_THUC_PHAM
                              where p.ID_TRUONG == phieu_detail.ID_TRUONG && p.ID_NHOM_THUC_PHAM == phieu_detail.ID_NHOM_THUC_PHAM && p.ID_THUC_PHAM == phieu_detail.ID_THUC_PHAM && p.IS_DELETE != true
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        if (luong_them_moi != null) detail.SO_LUONG = detail.SO_LUONG + luong_them_moi;
                        else if (luong_bot != null) detail.SO_LUONG = detail.SO_LUONG - luong_bot;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                    else
                    {
                        detail = new KHO_THUC_PHAM();
                        long newID = context.Database.SqlQuery<long>("SELECT KHO_THUC_PHAM_SEQ.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                        detail.ID_TRUONG = phieu_detail.ID_TRUONG;
                        detail.ID_NHOM_THUC_PHAM = phieu_detail.ID_NHOM_THUC_PHAM;
                        detail.ID_THUC_PHAM = phieu_detail.ID_THUC_PHAM;
                        detail.DON_VI_TINH = phieu_detail.DON_VI_TINH;
                        detail.SO_LUONG = phieu_detail.SO_LUONG;
                        detail.NGAY_TAO = DateTime.Now;
                        detail.NGUOI_TAO = nguoi;
                        detail = context.KHO_THUC_PHAM.Add(detail);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            var QICache = new DefaultCacheProvider();
            QICache.RemoveByFirstName("KHO_THUC_PHAM");
            return res;
        }
        #endregion
    }
}
