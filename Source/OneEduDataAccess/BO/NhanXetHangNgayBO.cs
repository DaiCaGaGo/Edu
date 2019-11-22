using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class NhanXetHangNgayBO
    {
        #region get
        public List<NHAN_XET_HANG_NGAY> getNhanXetHangNgay(long id_truong, short? ma_khoi, long? id_lop, long id_hoc_sinh)
        {
            List<NHAN_XET_HANG_NGAY> data = new List<NHAN_XET_HANG_NGAY>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("NHAN_XET_HANG_NGAY", "getNhanXetHangNgay", id_truong, ma_khoi, id_lop, id_hoc_sinh);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.NHAN_XET_HANG_NGAY where p.ID_TRUONG == id_truong select p);
                    if (ma_khoi != null)
                        tmp = tmp.Where(x => x.MA_KHOI == ma_khoi);
                    if (id_lop != null)
                        tmp = tmp.Where(x => x.ID_LOP == id_lop);
                    tmp = tmp.Where(x => x.ID_HOC_SINH == id_hoc_sinh);
                    tmp = tmp.OrderByDescending(x => x.NGAY_NX);
                    data = tmp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<NHAN_XET_HANG_NGAY>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<NhanXetHangNgayEntity> getNXHNByTruongKhoiLopNgay(long id_truong, short? ma_khoi, long? id_lop, DateTime? ngay, short id_nam_hoc, short ma_hoc_ky)
        {
            List<NhanXetHangNgayEntity> data = new List<NhanXetHangNgayEntity>();
            var QICache = new DefaultCacheProvider();
            int nam_gui = ngay != null ? ngay.Value.Year : DateTime.Now.Year;
            int thang_gui = ngay != null ? ngay.Value.Month : DateTime.Now.Year;
            int ngay_gui = ngay != null ? ngay.Value.Day : DateTime.Now.Day;
            string strKeyCache = QICache.BuildCachedKey("HOC_SINH", "NHAN_XET_HANG_NGAY", "getNXHNByTruongKhoiLopNgay", id_truong, ma_khoi, id_lop, nam_gui, thang_gui, ngay_gui, id_nam_hoc, ma_hoc_ky);
            if (!QICache.IsSet(strKeyCache))
            {
                LOP detailLop = new LOP();
                LopBO lopBO = new LopBO();
                if (id_lop != null)
                    detailLop = lopBO.getLopById(id_lop.Value);
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = string.Format(@"select hs.ID as ID_HS, hs.MA as MA_HS,hs.HO_TEN as TEN_HS
                        ,hs.TEN as TEN_LAST,hs.NGAY_SINH,hs.IS_GUI_BO_ME, hs.IS_DK_KY1, hs.IS_DK_KY2
                        ,hs.IS_MIEN_GIAM_KY1, hs.IS_MIEN_GIAM_KY2, hs.IS_CON_GV,hs.ID_KHOI as MA_KHOI, hs.ID_LOP as ID_LOP
                        ,hs.SDT_NHAN_TIN as SDT, hs.SDT_NHAN_TIN2 as SDT_KHAC, nxhn.*
                        ,{0} as TIEN_TO
                        from hoc_sinh hs 
                        left join nhan_xet_hang_ngay nxhn on hs.id = nxhn.id_hoc_sinh {1}
                        where not ( hs.is_delete is not null and hs.is_delete = 1)
                        and hs.id_truong = :0 and hs.id_khoi= :1
                        and hs.id_lop = :2 and hs.ID_NAM_HOC = :3 {2} {3}
                        order by nvl(hs.THU_TU, 0),NLSSORT(hs.ten,'NLS_SORT=vietnamese'),NLSSORT(hs.ho_dem,'NLS_SORT=vietnamese')",
                        detailLop != null && detailLop.LOAI_CHEN_TIN != null ? (detailLop.LOAI_CHEN_TIN == 1 ? "hs.HO_TEN" : 
                            (detailLop.LOAI_CHEN_TIN == 2 ? "hs.TEN" : "'" + detailLop.TIEN_TO + "'")) : "''",
                        " and extract(year from NGAY_NX)=" + nam_gui + " and extract(month from NGAY_NX)=" + thang_gui + 
                            " and extract(day from NGAY_NX)=" + ngay_gui,
                        ma_hoc_ky == 1 ? " and hs.TRANG_THAI_HOC in (1,2,3,8,9,10)" : " and hs.TRANG_THAI_HOC in (1,2,3,6,7,10)",
                        ma_hoc_ky == 1 ? " and ((hs.is_dk_ky1 is not null and hs.is_dk_ky1 = 1) or (hs.is_mien_giam_ky1 is not null and hs.is_mien_giam_ky1 = 1))" : " and ((hs.is_dk_ky2 is not null and hs.is_dk_ky2 = 1) or (hs.is_mien_giam_ky2 is not null and hs.is_mien_giam_ky2 = 1))");
                    data = context.Database.SqlQuery<NhanXetHangNgayEntity>(strQuery, id_truong, ma_khoi, id_lop, id_nam_hoc).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<NhanXetHangNgayEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public NhanXetHangNgayEntity getNXHNByHocSinhNgay(long id_hoc_sinh, DateTime? ngay)
        {
            NhanXetHangNgayEntity data = new NhanXetHangNgayEntity();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("NHAN_XET_HANG_NGAY", "getNXHNByHocSinhNgay", id_hoc_sinh, ngay);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = @"select id, id_hoc_sinh, noi_dung_nx, ngay_nx, is_send 
                        from nhan_xet_hang_ngay 
                        where not (is_send is not null and is_send = 1)
                        and id_hoc_sinh = :0
                        and trunc(NGAY_NX) = trunc(:1)";
                    data = context.Database.SqlQuery<NhanXetHangNgayEntity>(strQuery, id_hoc_sinh, ngay).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as NhanXetHangNgayEntity;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public NHAN_XET_HANG_NGAY getNhanXetHangNgayByID(long id)
        {
            NHAN_XET_HANG_NGAY data = new NHAN_XET_HANG_NGAY();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("NHAN_XET_HANG_NGAY", "getNhanXetHangNgayByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.NHAN_XET_HANG_NGAY where p.ID == id select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as NHAN_XET_HANG_NGAY;
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
        public ResultEntity update(NHAN_XET_HANG_NGAY detail_in, long? nguoi)
        {
            NHAN_XET_HANG_NGAY detail = new NHAN_XET_HANG_NGAY();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.NHAN_XET_HANG_NGAY
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.NOI_DUNG_NX = detail_in.NOI_DUNG_NX;
                        detail.LIST_MA_NX = detail_in.LIST_MA_NX;
                        detail.IS_TONG_HOP = detail_in.IS_TONG_HOP;
                        detail.IS_SEND = detail_in.IS_SEND;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("NHAN_XET_HANG_NGAY");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(NHAN_XET_HANG_NGAY detail_in, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail_in.ID = context.Database.SqlQuery<long>("SELECT NHAN_XET_HANG_NGAY_SEQ.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                    detail_in.NGAY_TAO = DateTime.Now;
                    detail_in.NGUOI_TAO = nguoi;
                    detail_in.NGAY_SUA = DateTime.Now;
                    detail_in.NGUOI_SUA = nguoi;
                    detail_in = context.NHAN_XET_HANG_NGAY.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("NHAN_XET_HANG_NGAY");
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
                    var sql = @"delete from NHAN_XET_HANG_NGAY where ID = :0";
                    context.Database.ExecuteSqlCommand(sql, id);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("NHAN_XET_HANG_NGAY");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity UpdateChuyenLop(long id_hoc_sinh, long id_lop, short ma_khoi, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    string sql = string.Format(@"Update NHAN_XET_HANG_NGAY set ID_LOP=:0, MA_KHOI=:1, NGUOI_SUA=:2, NGAY_SUA=:3 where ID_HOC_SINH=:4");
                    context.Database.ExecuteSqlCommand(sql, id_lop, ma_khoi, nguoi, DateTime.Now, id_hoc_sinh);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("NHAN_XET_HANG_NGAY");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity updateTrangThaiGui(long id, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    var sql = @"update NHAN_XET_HANG_NGAY set IS_SEND = 0 where ID = :0";
                    context.Database.ExecuteSqlCommand(sql, id);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("NHAN_XET_HANG_NGAY");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }

        public ResultEntity updateTrangThaiGuiTongHop(long id_nxhn,  long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    var sql = @"update NHAN_XET_HANG_NGAY set IS_SEND = 0 where ID = :0";
                    context.Database.ExecuteSqlCommand(sql, id_nxhn);

                    sql = @"update TONG_HOP_NHAN_XET_HANG_NGAY set IS_SEND = 0 where ID_NHAN_XET_HN = :0";
                    context.Database.ExecuteSqlCommand(sql, id_nxhn);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("NHAN_XET_HANG_NGAY");
                QICache.RemoveByFirstName("TONG_HOP_NHAN_XET_HANG_NGAY");
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
