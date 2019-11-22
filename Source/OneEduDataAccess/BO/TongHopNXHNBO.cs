using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class TongHopNXHNBO
    {
        #region get
        public List<TongHopNhanXetHangNgayEntity> getTongHopNXHNByTruongKhoiLopNgay(long id_truong, short? ma_khoi, long? id_lop, DateTime? ngay, short id_nam_hoc, short ma_hoc_ky)
        {
            List<TongHopNhanXetHangNgayEntity> data = new List<TongHopNhanXetHangNgayEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TONG_HOP_NHAN_XET_HANG_NGAY", "HOC_SINH", "GIOI_TINH", "getTongHopNXHNByTruongKhoiLopNgay", id_truong, ma_khoi, id_lop, ngay, id_nam_hoc, ma_hoc_ky);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {

                    string strQuery = string.Format(@"select hs.ID as ID_HS, hs.MA as MA_HS,hs.HO_TEN as TEN_HS
                        ,hs.TEN as TEN_LAST,hs.MA_GIOI_TINH,g.TEN as TEN_GIOI_TINH,hs.NGAY_SINH, hs.IS_DK_KY1, hs.IS_DK_KY2
                        ,hs.IS_MIEN_GIAM_KY1, hs.IS_MIEN_GIAM_KY2, hs.IS_CON_GV
                        ,hs.ID_KHOI as ma_khoi, hs.ID_LOP as id_lop
                        ,hs.IS_GUI_BO_ME as IS_GUI_BO_ME,hs.SDT_NHAN_TIN as sdt
                        ,hs.SDT_NHAN_TIN2 as sdt_khac,nxhn.*
                        from hoc_sinh hs 
                        left join tong_hop_nhan_xet_hang_ngay nxhn on hs.id = nxhn.id_hoc_sinh
                                 and TRUNC(NGAY_TONG_HOP) = :0
                        join GIOI_TINH g on hs.MA_GIOI_TINH=g.MA
                        where not ( hs.is_delete is not null and hs.is_delete = 1)
                        AND hs.id_truong = :1 and hs.id_khoi = :2 
                        and hs.id_nam_hoc = :3 and hs.id_lop = :4 {0} {1}
                        order by nvl(hs.THU_TU, 0),NLSSORT(hs.ten,'NLS_SORT=vietnamese'),NLSSORT(hs.ho_dem,'NLS_SORT=vietnamese')", 
                        ma_hoc_ky == 1 ? " and hs.TRANG_THAI_HOC in(1,2,3,8,9,10)" : " and hs.TRANG_THAI_HOC in (1,2,3,6,7,10)",
                        ma_hoc_ky == 1 ? " and ((hs.is_dk_ky1 is not null and hs.is_dk_ky1 = 1) or (hs.is_mien_giam_ky1 is not null and hs.is_mien_giam_ky1 = 1))" : " and ((hs.is_dk_ky2 is not null and hs.is_dk_ky2 = 1) or (hs.is_mien_giam_ky2 is not null and hs.is_mien_giam_ky2 = 1))"
                        );
                    data = context.Database.SqlQuery<TongHopNhanXetHangNgayEntity>(strQuery, ngay, id_truong, ma_khoi, id_nam_hoc, id_lop).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<TongHopNhanXetHangNgayEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public TONG_HOP_NHAN_XET_HANG_NGAY getTongHopNhanXetHangNgayByID(long id)
        {
            TONG_HOP_NHAN_XET_HANG_NGAY data = new TONG_HOP_NHAN_XET_HANG_NGAY();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TONG_HOP_NHAN_XET_HANG_NGAY", "getTongHopNhanXetHangNgayByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.TONG_HOP_NHAN_XET_HANG_NGAY where p.ID == id select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as TONG_HOP_NHAN_XET_HANG_NGAY;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public TONG_HOP_NHAN_XET_HANG_NGAY getTongHopNhanXetHangNgayByHocSinhAndNgay(long id_hoc_sinh, DateTime ngay_nhan_xet)
        {
            TONG_HOP_NHAN_XET_HANG_NGAY data = new TONG_HOP_NHAN_XET_HANG_NGAY();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TONG_HOP_NHAN_XET_HANG_NGAY", "getTongHopNhanXetHangNgayByHocSinhAndNgay", id_hoc_sinh, ngay_nhan_xet);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = string.Format(@"SELECT * FROM TONG_HOP_NHAN_XET_HANG_NGAY WHERE ID_HOC_SINH=:0 AND TRUNC(NGAY_TONG_HOP)=TRUNC(:1)");
                    data = context.Database.SqlQuery<TONG_HOP_NHAN_XET_HANG_NGAY>(strQuery, id_hoc_sinh, ngay_nhan_xet).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as TONG_HOP_NHAN_XET_HANG_NGAY;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<TONG_HOP_NHAN_XET_HANG_NGAY> getListTongHopByLop(long id_truong, long id_lop, DateTime ngay)
        {
            List<TONG_HOP_NHAN_XET_HANG_NGAY> data = new List<TONG_HOP_NHAN_XET_HANG_NGAY>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TONG_HOP_NHAN_XET_HANG_NGAY", "getListTongHopByLop", id_truong, id_lop, ngay);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    int nam_gui = ngay.Year;
                    int thang_gui = ngay.Month;
                    int ngay_gui = ngay.Day;
                    //string strQuery = string.Format(@"select * from TONG_HOP_NHAN_XET_HANG_NGAY 
                    //    where id_truong = :0 and id_lop = :1 and TRUNC(NGAY_TONG_HOP) = :2");
                    string strQuery = string.Format(@"select * from TONG_HOP_NHAN_XET_HANG_NGAY 
                        where id_truong = :0 and id_lop = :1 
                        and extract(year from NGAY_TONG_HOP) = :2
                        and extract(month from NGAY_TONG_HOP) = :3
                        and EXTRACT(day from NGAY_TONG_HOP) = :4");
                    data = context.Database.SqlQuery<TONG_HOP_NHAN_XET_HANG_NGAY>(strQuery, id_truong, id_lop, nam_gui, thang_gui, ngay_gui).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<TONG_HOP_NHAN_XET_HANG_NGAY>;
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
        public ResultEntity update(TONG_HOP_NHAN_XET_HANG_NGAY detail_in, long? nguoi)
        {
            TONG_HOP_NHAN_XET_HANG_NGAY detail = new TONG_HOP_NHAN_XET_HANG_NGAY();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.TONG_HOP_NHAN_XET_HANG_NGAY
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.ID_TRUONG = detail_in.ID_TRUONG;
                        detail.ID_LOP = detail_in.ID_LOP;
                        detail.NOI_DUNG_NX = detail_in.NOI_DUNG_NX;
                        detail.NGAY_TONG_HOP = detail_in.NGAY_TONG_HOP;
                        detail.IS_SEND = detail_in.IS_SEND;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("TONG_HOP_NHAN_XET_HANG_NGAY");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(TONG_HOP_NHAN_XET_HANG_NGAY detail_in, long? nguoi)
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
                    detail_in.NGAY_SUA = DateTime.Now;
                    detail_in.NGUOI_SUA = nguoi;
                    detail_in = context.TONG_HOP_NHAN_XET_HANG_NGAY.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("TONG_HOP_NHAN_XET_HANG_NGAY");
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
                    var sql = @"delete from TONG_HOP_NHAN_XET_HANG_NGAY where ID = :0";
                    context.Database.ExecuteSqlCommand(sql, id);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("TONG_HOP_NHAN_XET_HANG_NGAY");
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
                    string strQuery = string.Format(@"update TONG_HOP_NHAN_XET_HANG_NGAY set IS_SEND = 0 where ID=:0");
                    context.Database.ExecuteSqlCommand(strQuery, id);
                    TONG_HOP_NHAN_XET_HANG_NGAY tongHopNXHN = new TONG_HOP_NHAN_XET_HANG_NGAY();
                    tongHopNXHN = getTongHopNhanXetHangNgayByID(id);
                    long? id_nxhn = tongHopNXHN.ID_NHAN_XET_HN;
                    if (id_nxhn != null && id_nxhn > 0)
                    {
                        var sql = @"update NHAN_XET_HANG_NGAY set IS_SEND = 0 where ID=:0";
                        context.Database.ExecuteSqlCommand(sql, id_nxhn.Value);
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("TONG_HOP_NHAN_XET_HANG_NGAY");
                QICache.RemoveByFirstName("NHAN_XET_HANG_NGAY");
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
