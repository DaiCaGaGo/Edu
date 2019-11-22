using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class HocPhiDotThuBO
    {
        #region get
        public List<HOC_PHI_DOT_THU> getAllDotThuByTruong(long id_truong, short id_nam_hoc, bool is_all = false, long id_all = 0, string text_all = "Chọn tất cả")
        {
            List<HOC_PHI_DOT_THU> data = new List<HOC_PHI_DOT_THU>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_PHI_DOT_THU", "getAllDotThuByTruong", id_truong, id_nam_hoc, is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.HOC_PHI_DOT_THU where p.ID_TRUONG == id_truong && p.ID_NAM_HOC == id_nam_hoc && p.IS_DELETE != true select p).ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<HOC_PHI_DOT_THU>();
                        HOC_PHI_DOT_THU item_all = new HOC_PHI_DOT_THU();
                        item_all.ID = id_all;
                        item_all.TEN = text_all;
                        data.Insert(0, item_all);
                    }
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<HOC_PHI_DOT_THU>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<HocPhiDotThuTheoLopEntity> getDotThuLopByTruongLop(long id_truong, short id_nam_hoc, short id_khoi, long? id_lop)
        {
            List<HocPhiDotThuTheoLopEntity> data = new List<HocPhiDotThuTheoLopEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_PHI_DOT_THU_LOP", "HOC_PHI_DOT_THU", "getDotThuLopByTruongLop", id_truong, id_nam_hoc, id_khoi, id_lop);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = @"select dt.id,dt.ten from HOC_PHI_DOT_THU_LOP dtl 
                        join HOC_PHI_DOT_THU dt on dtl.id_truong = dt.id_truong and dtl.id_dot_thu = dt.id
                        where dtl.id_truong=:0 and dtl.id_nam_hoc=:1 and dtl.id_khoi=:2";
                    if (id_lop != null)
                        tmp += " and dtl.id_lop=" + id_lop;
                    data = context.Database.SqlQuery<HocPhiDotThuTheoLopEntity>(tmp, id_truong, id_nam_hoc, id_khoi).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<HocPhiDotThuTheoLopEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<HOC_PHI_DOT_THU> getDotThuByOther(long id_truong, short id_nam_hoc, short? loai_khoan_thu, short? hoc_ky, short? thang, string noi_dung)
        {
            List<HOC_PHI_DOT_THU> data = new List<HOC_PHI_DOT_THU>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_PHI_DOT_THU", "getDotThuByOther", id_truong, id_nam_hoc, loai_khoan_thu, hoc_ky, thang, noi_dung);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.HOC_PHI_DOT_THU where p.ID_TRUONG == id_truong && p.ID_NAM_HOC == id_nam_hoc select p);
                    if (loai_khoan_thu != null)
                    {
                        tmp = tmp.Where(x => x.ID_DOT_THU == loai_khoan_thu);
                        if (hoc_ky != null) tmp = tmp.Where(x => x.HOC_KY == hoc_ky);
                        if (thang != null) tmp = tmp.Where(x => x.THANG == thang);
                    }
                    if (!string.IsNullOrEmpty(noi_dung))
                        tmp = tmp.Where(x => x.TEN.ToLower().Contains(noi_dung.ToLower()) || x.GHI_CHU.ToLower().Contains(noi_dung.ToLower()));
                    tmp = tmp.OrderBy(x => x.THU_TU).ThenBy(x => x.TEN);
                    data = tmp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<HOC_PHI_DOT_THU>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public HOC_PHI_DOT_THU getDotThuByID(long id)
        {
            HOC_PHI_DOT_THU data = new HOC_PHI_DOT_THU();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_PHI_DOT_THU", "getDotThuByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.HOC_PHI_DOT_THU where p.ID == id && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as HOC_PHI_DOT_THU;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public long? getMaxThuTuByTruong(long id_truong, int? id_nam_hoc)
        {
            long? data = null;
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_PHI_DOT_THU", "getMaxThuTuByTruong", id_truong, id_nam_hoc);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var sql = @"select MAX(THU_TU) as THU_TU from HOC_PHI_DOT_THU 
                                    where ID_TRUONG = :0 and ID_NAM_HOC=:1";
                    data = context.Database.SqlQuery<long?>(sql, id_truong, id_nam_hoc).FirstOrDefault();
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
        public ResultEntity update(HOC_PHI_DOT_THU detail_in, long? nguoi)
        {
            HOC_PHI_DOT_THU detail = new HOC_PHI_DOT_THU();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.HOC_PHI_DOT_THU
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.ID_TRUONG = detail_in.ID_TRUONG;
                        detail.ID_NAM_HOC = detail_in.ID_NAM_HOC;
                        detail.TEN = detail_in.TEN;
                        detail.GHI_CHU = detail_in.GHI_CHU;
                        detail.IS_TIEN_AN = detail_in.IS_TIEN_AN;
                        detail.TONG_TIEN = detail_in.TONG_TIEN;
                        detail.THOI_GIAN_BAT_DAU = detail_in.THOI_GIAN_BAT_DAU;
                        detail.THOI_GIAN_KET_THUC = detail_in.THOI_GIAN_KET_THUC;
                        detail.ID_DOT_THU = detail_in.ID_DOT_THU;
                        detail.HOC_KY = detail_in.HOC_KY;
                        detail.THANG = detail_in.THANG;
                        detail.SO_TIEN_AN = detail_in.SO_TIEN_AN;
                        detail.THU_TU = detail_in.THU_TU;
                        detail.ID_TRUONG = detail_in.ID_TRUONG;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("HOC_PHI_DOT_THU");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(HOC_PHI_DOT_THU detail_in, long? nguoi)
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
                    detail_in = context.HOC_PHI_DOT_THU.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("HOC_PHI_DOT_THU");
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
                        var sql = @"update HOC_PHI_DOT_THU set IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 where ID = :2";
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, id);
                    }
                    else
                    {
                        var sql = @"delete from HOC_PHI_DOT_THU where ID = :0";
                        context.Database.ExecuteSqlCommand(sql, id);
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("HOC_PHI_DOT_THU");
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
