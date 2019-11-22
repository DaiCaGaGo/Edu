using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class PhieuNhapKhoBO
    {
        #region GET
        public long? getMaxPhieuByTruong(long id_truong)
        {
            long? data = null;
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("PHIEU_NHAP_KHO", "getMaxPhieuByTruong", id_truong);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var sql = @"select MAX(THU_TU) as MA_SO from PHIEU_NHAP_KHO 
                                    where ID_TRUONG=:0";
                    data = context.Database.SqlQuery<long?>(sql, id_truong).FirstOrDefault();
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
        public PHIEU_NHAP_KHO getPhieuNhapKhoByID(long id)
        {
            PHIEU_NHAP_KHO data = new PHIEU_NHAP_KHO();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("PHIEU_NHAP_KHO", "getPhieuNhapKhoByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.PHIEU_NHAP_KHO where p.ID == id && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as PHIEU_NHAP_KHO;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<PHIEU_NHAP_KHO> getPhieuNhapKhoByTime(long id_truong, DateTime? tu_ngay, DateTime? den_ngay, long? nguoi_nhan_hang)
        {
            List<PHIEU_NHAP_KHO> data = new List<PHIEU_NHAP_KHO>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("PHIEU_NHAP_KHO", "getPhieuNhapKhoByTime", id_truong, tu_ngay, den_ngay, nguoi_nhan_hang);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    try
                    {
                        var sql = "";
                        if (tu_ngay != null)
                            sql = string.Format(@" and NGAY_NHAP_KHO >= TO_DATE('{0}', 'DD/MM/YYYY HH:MI:SS AM')", tu_ngay);
                        if (den_ngay != null)
                            sql += string.Format(@" and NGAY_NHAP_KHO < TO_DATE('{0}', 'DD/MM/YYYY HH:MI:SS AM')", den_ngay);
                        string strQuery = string.Format(@"select * from PHIEU_NHAP_KHO where ID_TRUONG=:0 and not (is_delete is not null and is_delete=1) {0}",
                            nguoi_nhan_hang != null ? (" and ID_NGUOI_NHAN_HANG=" + nguoi_nhan_hang) : "");
                        strQuery += sql;
                        strQuery += " order by NGAY_NHAP_KHO desc";
                        data = context.Database.SqlQuery<PHIEU_NHAP_KHO>(strQuery, id_truong).ToList();
                        QICache.Set(strKeyCache, data, 300000);
                    }
                    catch (Exception ex)
                    { }
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<PHIEU_NHAP_KHO>;
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
        public ResultEntity update(PHIEU_NHAP_KHO detail_in, long? nguoi)
        {
            PHIEU_NHAP_KHO detail = new PHIEU_NHAP_KHO();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.PHIEU_NHAP_KHO
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();

                    if (detail != null)
                    {
                        detail.MA_SO_PHIEU = detail_in.MA_SO_PHIEU;
                        detail.ID_TRUONG = detail_in.ID_TRUONG;
                        detail.NGAY_NHAP_KHO = detail_in.NGAY_NHAP_KHO;
                        detail.ID_NGUOI_NHAN_HANG = detail_in.ID_NGUOI_NHAN_HANG;
                        detail.TONG_GIA = detail_in.TONG_GIA;
                        detail.GHI_CHU = detail_in.GHI_CHU;
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
            QICache.RemoveByFirstName("PHIEU_NHAP_KHO");
            return res;
        }
        public ResultEntity updateGiaPhieu(long id_phieu_nhap_kho, decimal gia_phieu, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            string sql = string.Empty;
            try
            {
                using (var context = new oneduEntities())
                {
                    sql = @"UPDATE PHIEU_NHAP_KHO SET TONG_GIA=:0, NGAY_SUA=:1, NGUOI_SUA=:2 WHERE ID=" + id_phieu_nhap_kho;
                    int resKQ = context.Database.ExecuteSqlCommand(sql, gia_phieu, DateTime.Now, nguoi);
                }
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }

            var QICache = new DefaultCacheProvider();
            QICache.RemoveByFirstName("PHIEU_NHAP_KHO");
            return res;
        }
        public ResultEntity insert(PHIEU_NHAP_KHO detail_in, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    long newID = context.Database.SqlQuery<long>("SELECT PHIEU_NHAP_KHO_SEQ.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                    detail_in.ID = newID;
                    detail_in.NGAY_TAO = DateTime.Now;
                    detail_in.NGUOI_TAO = nguoi;
                    detail_in = context.PHIEU_NHAP_KHO.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("PHIEU_NHAP_KHO");
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
                        sql = @"DELETE PHIEU_NHAP_KHO_DETAIL WHERE ID_PHIEU_NHAP = " + id.ToString();
                        context.Database.ExecuteSqlCommand(sql);
                        sql = @"DELETE PHIEU_NHAP_KHO WHERE ID = " + id.ToString();
                        int resKQ = context.Database.ExecuteSqlCommand(sql);
                    }
                    else
                    {
                        sql = @"UPDATE PHIEU_NHAP_KHO SET IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 WHERE ID=" + id.ToString();
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
            QICache.RemoveByFirstName("PHIEU_NHAP_KHO_DETAIL");
            QICache.RemoveByFirstName("PHIEU_NHAP_KHO");
            return res;
        }
        #endregion
    }
}
