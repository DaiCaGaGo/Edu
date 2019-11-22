using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class BaiTapVeNhaChiTietBO
    {
        KhoThucPhamBO khoThucPhamBO = new KhoThucPhamBO();
        PhieuNhapKhoBO phieuNhapKhoBO = new PhieuNhapKhoBO();
        #region GET
        public List<BAI_TAP_VE_NHA_CHI_TIET> getChiTietByBaiTapID(long id_truong, long id_btvn)
        {
            List<BAI_TAP_VE_NHA_CHI_TIET> data = new List<BAI_TAP_VE_NHA_CHI_TIET>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("BAI_TAP_VE_NHA_CHI_TIET", "getChiTietByBaiTapID", id_truong, id_btvn);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var temp = (from p in context.BAI_TAP_VE_NHA_CHI_TIET where p.ID_TRUONG == id_truong && p.ID_BTVN == id_btvn && p.IS_DELETE != true select p);
                    temp = temp.OrderBy(p => p.ID_MON_HOC).ThenBy(p => p.BAI_SO).ThenByDescending(p => p.TRANG_SO);
                    data = temp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<BAI_TAP_VE_NHA_CHI_TIET>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public BAI_TAP_VE_NHA_CHI_TIET getChiTietBySachMonBaiTrang(long id_truong, long id_btvn, short id_mon_hoc, long id_sach, short bai_so, short trang_so)
        {
            BAI_TAP_VE_NHA_CHI_TIET data = new BAI_TAP_VE_NHA_CHI_TIET();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("BAI_TAP_VE_NHA_CHI_TIET", "getChiTietBySachMonBaiTrang", id_truong, id_btvn, id_mon_hoc, id_sach, bai_so, trang_so);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.BAI_TAP_VE_NHA_CHI_TIET where p.ID_TRUONG == id_truong && p.ID_BTVN == id_btvn && p.ID_MON_HOC == id_mon_hoc && p.ID_SACH == id_sach && p.BAI_SO == bai_so && p.TRANG_SO == trang_so && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as BAI_TAP_VE_NHA_CHI_TIET;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public BAI_TAP_VE_NHA_CHI_TIET getChiTietBaiTapVeNhaByID(long id)
        {
            BAI_TAP_VE_NHA_CHI_TIET data = new BAI_TAP_VE_NHA_CHI_TIET();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("BAI_TAP_VE_NHA_CHI_TIET", "getChiTietBaiTapVeNhaByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.BAI_TAP_VE_NHA_CHI_TIET where p.ID == id && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as BAI_TAP_VE_NHA_CHI_TIET;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<ViewBaiTapVeNhaEntity> getBaiTapVeNhaByHocSinh(long id_truong, short id_nam_hoc, long id_lop, string ngay_btvn)
        {
            List<ViewBaiTapVeNhaEntity> data = new List<ViewBaiTapVeNhaEntity>();
            using (oneduEntities context = new oneduEntities())
            {
                //var tmp = string.Format(@"select ct.ID_MON_HOC, ct.ID_SACH, mon.TEN as TEN_MON, s.TEN as TEN_SACH, ct.BAI_SO, ct.TRANG_SO, sach.ICON, btvn.NOI_DUNG ,
                //    case when ct.bai_so is not null and ct.trang_so is not null then N'Bài số ' || ct.bai_so || N' trang số ' || ct.trang_so 
                //    when ct.bai_so is not null and ct.trang_so is null then N'Bài số ' || ct.bai_so
                //    when ct.bai_so is null and ct.trang_so is not null then N'Trang số ' || ct.trang_so
                //    else N'' end as BAI_TAP_CHI_TIET
                //    from BAI_TAP_VE_NHA_CHI_TIET ct 
                //    left join BAI_TAP_VE_NHA btvn on ct.ID_BTVN = btvn.id
                //    left join DM_SACH_CHI_TIET sach on sach.ID_MON=ct.ID_MON_HOC and sach.ID_SACH=ct.ID_SACH and sach.TRANG_SO=ct.TRANG_SO and sach.BAI_SO=ct.BAI_SO
                //    join mon_hoc mon on mon.id=ct.ID_MON_HOC
                //    join dm_sach s on s.id = ct.ID_SACH
                //    where btvn.ID_TRUONG=:0 and btvn.ID_NAM_HOC=:1 and btvn.ID_LOP=:2
                //    and to_char(btvn.NGAY_BTVN, 'ddMMyyyy') = '{0}' order by ct.id_mon_hoc, ct.bai_so, ct.Trang_so", ngay_btvn);
                var tmp = string.Format(@"select ct.ID_MON_HOC, ct.ID_SACH, mon.TEN as TEN_MON, s.TEN as TEN_SACH, ct.BAI_SO, ct.TRANG_SO, sach.ICON, btvn.NOI_DUNG ,
                    case when ct.bai_so is not null and ct.trang_so is not null then N'Bài số ' || ct.bai_so || N' trang số ' || ct.trang_so 
                    when ct.bai_so is not null and ct.trang_so is null then N'Bài số ' || ct.bai_so
                    when ct.bai_so is null and ct.trang_so is not null then N'Trang số ' || ct.trang_so
                    else N'' end as BAI_TAP_CHI_TIET
                    from BAI_TAP_VE_NHA_CHI_TIET ct 
                    left join BAI_TAP_VE_NHA btvn on ct.ID_BTVN = btvn.id
                    left join DM_SACH_CHI_TIET sach on sach.ID_MON=ct.ID_MON_HOC and sach.ID_SACH=ct.ID_SACH and sach.TRANG_SO=ct.TRANG_SO and sach.BAI_SO=ct.BAI_SO
                    join mon_hoc mon on mon.id=ct.ID_MON_HOC
                    join dm_sach s on s.id = ct.ID_SACH
                    where btvn.ID_TRUONG=:0 and btvn.ID_NAM_HOC=:1 and btvn.ID_LOP=:2
                    and to_char(btvn.NGAY_BTVN, 'ddMMyyyy') = '{0}' order by ct.id_mon_hoc, ct.bai_so, ct.Trang_so", ngay_btvn);
                data = context.Database.SqlQuery<ViewBaiTapVeNhaEntity>(tmp, id_truong, id_nam_hoc, id_lop).ToList();
            }
            return data;
        }

        public List<BAI_TAP_VE_NHA> getBaiTapVeNhaChung(long id_truong, short id_nam_hoc, long id_lop, string ngay_btvn)
        {
            List<BAI_TAP_VE_NHA> data = new List<BAI_TAP_VE_NHA>();
            using (oneduEntities context = new oneduEntities())
            {
                var tmp = string.Format(@"select * from BAI_TAP_VE_NHA btvn
                    where btvn.ID_TRUONG=:0 and btvn.ID_NAM_HOC=:1 and btvn.ID_LOP=:2
                    and to_char(btvn.NGAY_BTVN, 'ddMMyyyy') = '{0}'", ngay_btvn);
                data = context.Database.SqlQuery<BAI_TAP_VE_NHA>(tmp, id_truong, id_nam_hoc, id_lop).ToList();
            }
            return data;
        }
        #endregion
        #region SET
        public ResultEntity update(BAI_TAP_VE_NHA_CHI_TIET detail_in, long? nguoi)
        {
            BAI_TAP_VE_NHA_CHI_TIET detail = new BAI_TAP_VE_NHA_CHI_TIET();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.BAI_TAP_VE_NHA_CHI_TIET
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();

                    if (detail != null)
                    {
                        detail.ID_TRUONG = detail_in.ID_TRUONG;
                        detail.MA_CAP_HOC = detail_in.MA_CAP_HOC;
                        detail.ID_BTVN = detail_in.ID_BTVN;
                        detail.ID_MON_HOC = detail_in.ID_MON_HOC;
                        detail.ID_SACH = detail_in.ID_SACH;
                        detail.BAI_SO = detail_in.BAI_SO;
                        detail.TRANG_SO = detail_in.TRANG_SO;
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
            QICache.RemoveByFirstName("BAI_TAP_VE_NHA_CHI_TIET");
            return res;
        }
        public ResultEntity insert(BAI_TAP_VE_NHA_CHI_TIET detail_in, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    long newID = context.Database.SqlQuery<long>("SELECT BAI_TAP_VE_NHA_CHI_TIET_SEQ.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                    detail_in.ID = newID;
                    detail_in.NGAY_TAO = DateTime.Now;
                    detail_in.NGUOI_TAO = nguoi;
                    detail_in = context.BAI_TAP_VE_NHA_CHI_TIET.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("BAI_TAP_VE_NHA_CHI_TIET");
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
            string sql = string.Empty;
            try
            {
                using (var context = new oneduEntities())
                {
                    sql = @"DELETE BAI_TAP_VE_NHA_CHI_TIET WHERE ID = " + id.ToString();
                    context.Database.ExecuteSqlCommand(sql);
                }
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }

            var QICache = new DefaultCacheProvider();
            QICache.RemoveByFirstName("BAI_TAP_VE_NHA_CHI_TIET");
            return res;
        }
        public ResultEntity deleteChiTietBySoBai(long id_truong, long id_btvn, short id_mon_hoc, long id_sach, short bai_so, short trang_so)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            string sql = string.Empty;
            try
            {
                using (var context = new oneduEntities())
                {
                    sql = @"DELETE BAI_TAP_VE_NHA_CHI_TIET WHERE ID_TRUONG=" + id_truong + " and ID_BTVN=" + id_btvn + " and ID_MON_HOC=" + id_mon_hoc + " and ID_SACH=" + id_sach + " and BAI_SO=" + bai_so + " and TRANG_SO=" + trang_so;
                    context.Database.ExecuteSqlCommand(sql);
                }
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }

            var QICache = new DefaultCacheProvider();
            QICache.RemoveByFirstName("BAI_TAP_VE_NHA_CHI_TIET");
            return res;
        }
        #endregion
    }
}
