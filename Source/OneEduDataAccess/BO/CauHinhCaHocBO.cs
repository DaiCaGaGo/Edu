using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class CauHinhCaHocBO
    {
        #region Get
        public List<CAU_HINH_CA_HOC> getCauHinhCaHoc()
        {
            List<CAU_HINH_CA_HOC> data = new List<CAU_HINH_CA_HOC>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CAU_HINH_CA_HOC", "getCauHinhCaHoc");
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.CAU_HINH_CA_HOC where p.IS_DELETE != true select p).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<CAU_HINH_CA_HOC>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<CAU_HINH_CA_HOC> getCauHinhCaHocByTruong(long idTruong)
        {
            List<CAU_HINH_CA_HOC> data = new List<CAU_HINH_CA_HOC>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CAU_HINH_CA_HOC", "getCauHinhCaHocByTruong", idTruong);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.CAU_HINH_CA_HOC where p.IS_DELETE != true && p.ID_TRUONG == idTruong select p).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<CAU_HINH_CA_HOC>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public CauHinhCaHocEntity getMinMaxDateCauHinhCaHoc(long idTruong)
        {
            CauHinhCaHocEntity data = new CauHinhCaHocEntity();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CAU_HINH_CA_HOC", "getMinMaxDateCauHinhCaHoc", idTruong);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = @"select 
                                            MIN(NGAY_BAT_DAU) AS NGAY_BAT_DAU,
                                            MAX(NGAY_KET_THUC) AS NGAY_KET_THUC
                                        from CAU_HINH_CA_HOC c
                                        where ID_TRUONG = :0 and not exists (select * from CAU_HINH_CA_HOC  c1 where IS_DELETE=1 and c.ID=c1.ID)";

                    data = context.Database.SqlQuery<CauHinhCaHocEntity>(strQuery, idTruong).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as CauHinhCaHocEntity;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public CAU_HINH_CA_HOC getCauHinhCaHocByID(long id)
        {
            CAU_HINH_CA_HOC data = new CAU_HINH_CA_HOC();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CAU_HINH_CA_HOC", "getCauHinhCaHocByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.CAU_HINH_CA_HOC where p.IS_DELETE != true && p.ID == id select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as CAU_HINH_CA_HOC;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public long? getIDCauHinhCaHocByTruongHocKy(long id_truong, short id_hoc_ky)
        {
            long? data = null;
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CA_HOC", "LOP", "getIDCauHinhCaHocByTruong", id_truong, id_hoc_ky);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var sql = @"select MAX(ID_CAU_HINH_CA_HOC) as ID_CAU_HINH_CA_HOC from CA_HOC c
                                    join Lop l on c.ID_LOP = l.ID
                                    where l.ID_TRUONG = :0 and c.ID_HOC_KY = :1";
                    data = context.Database.SqlQuery<long?>(sql, id_truong, id_hoc_ky).FirstOrDefault();
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
        public ResultEntity update(CAU_HINH_CA_HOC detail_in, long? nguoi)
        {
            CAU_HINH_CA_HOC detail = new CAU_HINH_CA_HOC();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.CAU_HINH_CA_HOC
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.NGAY_BAT_DAU = detail_in.NGAY_BAT_DAU;
                        detail.NGAY_KET_THUC = detail_in.NGAY_KET_THUC;
                        detail.MUA = detail_in.MUA;
                        detail.THOI_GIAN_TIET1 = detail_in.THOI_GIAN_TIET1;
                        detail.THOI_GIAN_TIET2 = detail_in.THOI_GIAN_TIET2;
                        detail.THOI_GIAN_TIET3 = detail_in.THOI_GIAN_TIET3;
                        detail.THOI_GIAN_TIET4 = detail_in.THOI_GIAN_TIET4;
                        detail.THOI_GIAN_TIET5 = detail_in.THOI_GIAN_TIET5;
                        detail.THOI_GIAN_TIET6 = detail_in.THOI_GIAN_TIET6;
                        detail.THOI_GIAN_TIET7 = detail_in.THOI_GIAN_TIET7;
                        detail.THOI_GIAN_TIET8 = detail_in.THOI_GIAN_TIET8;
                        detail.THOI_GIAN_TIET9 = detail_in.THOI_GIAN_TIET9;
                        detail.THOI_GIAN_TIET10 = detail_in.THOI_GIAN_TIET10;
                        detail.THOI_GIAN_TIET11 = detail_in.THOI_GIAN_TIET11;
                        detail.THOI_GIAN_TIET12 = detail_in.THOI_GIAN_TIET12;
                        detail.THOI_GIAN_TIET13 = detail_in.THOI_GIAN_TIET13;
                        detail.THOI_GIAN_TIET14 = detail_in.THOI_GIAN_TIET14;
                        detail.THOI_GIAN_TIET15 = detail_in.THOI_GIAN_TIET15;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        detail.IS_DELETE = detail_in.IS_DELETE;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CAU_HINH_CA_HOC");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
                //res.Msg = ex.ToString();
            }
            return res;
        }
        public ResultEntity insert(CAU_HINH_CA_HOC detail_in, long? nguoi)
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
                    detail_in = context.CAU_HINH_CA_HOC.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CAU_HINH_CA_HOC");
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
                    var sql = @"update CAU_HINH_CA_HOC set IS_DELETE = 1,NGUOI_SUA=:0,NGAY_SUA=:1 where ID = :2";
                    context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, id);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CAU_HINH_CA_HOC");
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
