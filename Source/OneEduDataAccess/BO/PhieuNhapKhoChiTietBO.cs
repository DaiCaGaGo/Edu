using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class PhieuNhapKhoChiTietBO
    {
        KhoThucPhamBO khoThucPhamBO = new KhoThucPhamBO();
        PhieuNhapKhoBO phieuNhapKhoBO = new PhieuNhapKhoBO();
        #region GET
        public List<PHIEU_NHAP_KHO_DETAIL> getPhieuNhapKhoChiTiet(long id_truong, long id_phieu_nhap)
        {
            List<PHIEU_NHAP_KHO_DETAIL> data = new List<PHIEU_NHAP_KHO_DETAIL>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("PHIEU_NHAP_KHO_DETAIL", "getPhieuNhapKhoChiTiet", id_truong, id_phieu_nhap);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.PHIEU_NHAP_KHO_DETAIL where p.ID_TRUONG == id_truong && p.ID_PHIEU_NHAP == id_phieu_nhap && p.IS_DELETE != true select p).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<PHIEU_NHAP_KHO_DETAIL>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public PHIEU_NHAP_KHO_DETAIL getPhieuChiTietByPhieuNhapAndThucPham(long id_truong, long id_phieu_nhap, short id_nhom_thuc_pham, long id_thuc_pham)
        {
            PHIEU_NHAP_KHO_DETAIL data = new PHIEU_NHAP_KHO_DETAIL();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("PHIEU_NHAP_KHO_DETAIL", "getPhieuChiTietByPhieuNhapAndThucPham", id_truong, id_phieu_nhap, id_nhom_thuc_pham, id_thuc_pham);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.PHIEU_NHAP_KHO_DETAIL where p.ID_TRUONG == id_truong && p.ID_PHIEU_NHAP == id_phieu_nhap && p.ID_NHOM_THUC_PHAM == id_nhom_thuc_pham && p.ID_THUC_PHAM == id_thuc_pham && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as PHIEU_NHAP_KHO_DETAIL;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public PHIEU_NHAP_KHO_DETAIL getPhieuNhapKhoChiTietByID(long id)
        {
            PHIEU_NHAP_KHO_DETAIL data = new PHIEU_NHAP_KHO_DETAIL();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("PHIEU_NHAP_KHO_DETAIL", "getPhieuNhapKhoChiTietByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.PHIEU_NHAP_KHO_DETAIL where p.ID == id && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as PHIEU_NHAP_KHO_DETAIL;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        #endregion
        #region SET
        public ResultEntity update(PHIEU_NHAP_KHO_DETAIL detail_in, long? nguoi)
        {
            PHIEU_NHAP_KHO_DETAIL detail = new PHIEU_NHAP_KHO_DETAIL();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.PHIEU_NHAP_KHO_DETAIL
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();

                    if (detail != null)
                    {
                        decimal? so_luong_old = detail.SO_LUONG;
                        detail.ID_PHIEU_NHAP = detail_in.ID_PHIEU_NHAP;
                        detail.ID_TRUONG = detail_in.ID_TRUONG;
                        detail.NGAY_NHAP_KHO = detail_in.NGAY_NHAP_KHO;
                        detail.DON_VI_TINH = detail_in.DON_VI_TINH;
                        detail.SO_LUONG = detail_in.SO_LUONG;
                        detail.DON_GIA = detail_in.DON_GIA;
                        detail.TONG_GIA = detail_in.TONG_GIA;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                        #region update thực phẩm trong kho
                        decimal? so_luong_new = detail_in.SO_LUONG;
                        decimal? luong_them_hoac_bot = so_luong_old - so_luong_new;
                        ResultEntity res1 = khoThucPhamBO.cap_nhat_kho_thuc_pham(detail_in, null, luong_them_hoac_bot, nguoi);
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            var QICache = new DefaultCacheProvider();
            QICache.RemoveByFirstName("PHIEU_NHAP_KHO_DETAIL");
            return res;
        }
        public ResultEntity insert(PHIEU_NHAP_KHO_DETAIL detail_in, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    long newID = context.Database.SqlQuery<long>("SELECT PHIEU_NHAP_KHO_DETAIL_SEQ.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                    detail_in.ID = newID;
                    detail_in.NGAY_TAO = DateTime.Now;
                    detail_in.NGUOI_TAO = nguoi;
                    detail_in = context.PHIEU_NHAP_KHO_DETAIL.Add(detail_in);
                    context.SaveChanges();
                    #region Thêm thực phẩm vào kho
                    ResultEntity res2 = khoThucPhamBO.cap_nhat_kho_thuc_pham(detail_in, detail_in.SO_LUONG, null, nguoi);
                    #endregion
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("PHIEU_NHAP_KHO_DETAIL");
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
                        sql = @"DELETE PHIEU_NHAP_KHO_DETAIL WHERE ID = " + id.ToString();
                        context.Database.ExecuteSqlCommand(sql);
                    }
                    else
                    {
                        sql = @"UPDATE PHIEU_NHAP_KHO_DETAIL SET IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 WHERE ID=" + id.ToString();
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }

            var QICache = new DefaultCacheProvider();
            QICache.RemoveByFirstName("PHIEU_NHAP_KHO_DETAIL");
            return res;
        }
        public ResultEntity deleteByPhieuID(long id_truong, long id_phieu_nhap, long id_thuc_pham,decimal? so_luong, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            string sql = string.Empty;
            try
            {
                using (var context = new oneduEntities())
                {
                    sql = @"DELETE PHIEU_NHAP_KHO_DETAIL WHERE ID_TRUONG=" + id_truong + " and ID_PHIEU_NHAP=" + id_phieu_nhap + " and ID_THUC_PHAM=" + id_thuc_pham;
                    context.Database.ExecuteSqlCommand(sql);
                    #region Trừ thực phẩm trong kho
                    KHO_THUC_PHAM khoThucPham = new KHO_THUC_PHAM();
                    khoThucPham = khoThucPhamBO.getKhoThucPhamByThucPham(id_truong, id_thuc_pham);
                    if (khoThucPham != null)
                    {
                        khoThucPham.SO_LUONG = khoThucPham.SO_LUONG - so_luong;
                        ResultEntity res1 = khoThucPhamBO.update(khoThucPham, nguoi);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }

            var QICache = new DefaultCacheProvider();
            QICache.RemoveByFirstName("PHIEU_NHAP_KHO_DETAIL");
            return res;
        }
        #endregion
    }
}
