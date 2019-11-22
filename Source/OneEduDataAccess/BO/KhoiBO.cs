using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class KhoiBO
    {
        #region get
        public List<KHOI> getKhoi(string cap_hoc, bool is_all = false, short id_all = 0, string text_all = "Chọn tất cả")
        {
            List<KHOI> data = new List<KHOI>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("KHOI", "getKhoi", cap_hoc, is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.KHOIs where p.IS_DELETE != true select p);
                    if (cap_hoc == SYS_Cap_Hoc.MN)
                        tmp = tmp.Where(x => x.IS_MN == true);
                    if (cap_hoc == SYS_Cap_Hoc.TH)
                        tmp = tmp.Where(x => x.IS_TH == true);
                    if (cap_hoc == SYS_Cap_Hoc.THCS)
                        tmp = tmp.Where(x => x.IS_THCS == true);
                    if (cap_hoc == SYS_Cap_Hoc.THPT)
                        tmp = tmp.Where(x => x.IS_THPT == true);
                    if (cap_hoc == SYS_Cap_Hoc.GDTX)
                        tmp = tmp.Where(x => x.IS_GDTX == true);
                    tmp = tmp.OrderBy(x => x.THU_TU);
                    data = tmp.ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<KHOI>();
                        KHOI item_all = new KHOI();
                        item_all.MA = id_all;
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
                    data = QICache.Get(strKeyCache) as List<KHOI>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<KHOI> getKhoiByCapHocAndMaLoaiGDTX(string cap_hoc, short? maLoaiLopGDTX = null, bool is_all = false, short id_all = 0, string text_all = "Chọn tất cả")
        {
            List<KHOI> data = new List<KHOI>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("KHOI", "getKhoiByCapHocAndMaLoaiGDTX", cap_hoc, maLoaiLopGDTX, is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.KHOIs where p.IS_DELETE != true select p);
                    if (cap_hoc == SYS_Cap_Hoc.MN)
                        tmp = tmp.Where(x => x.IS_MN == true);
                    if (cap_hoc == SYS_Cap_Hoc.TH)
                        tmp = tmp.Where(x => x.IS_TH == true);
                    if (cap_hoc == SYS_Cap_Hoc.THCS)
                        tmp = tmp.Where(x => x.IS_THCS == true);
                    if (cap_hoc == SYS_Cap_Hoc.THPT)
                        tmp = tmp.Where(x => x.IS_THPT == true);
                    if (cap_hoc == SYS_Cap_Hoc.GDTX)
                    {
                        tmp = tmp.Where(x => x.IS_GDTX == true);
                        if (maLoaiLopGDTX != null)
                            tmp = tmp.Where(x => x.MA_LOAI_LOP_GDTX == maLoaiLopGDTX);
                    }
                    tmp = tmp.OrderBy(x => x.THU_TU);
                    data = tmp.ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<KHOI>();
                        KHOI item_all = new KHOI();
                        item_all.MA = id_all;
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
                    data = QICache.Get(strKeyCache) as List<KHOI>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public KHOI getKhoiByMa(short ma)
        {
            KHOI data = new KHOI();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("KHOI", "getKhoiByMa", ma);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.KHOIs where p.MA == ma select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as KHOI;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<KHOI> getKhoiByCapHoc(string cap_hoc, short? maLoaiLopGDTX = null)
        {
            List<KHOI> data = new List<KHOI>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("KHOI", "getKhoiByCapHoc", cap_hoc, maLoaiLopGDTX);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.KHOIs
                               where p.IS_DELETE != true
                               select p);
                    if (cap_hoc == SYS_Cap_Hoc.MN)
                        tmp = tmp.Where(x => x.IS_MN == true);
                    if (cap_hoc == SYS_Cap_Hoc.TH)
                        tmp = tmp.Where(x => x.IS_TH == true);
                    if (cap_hoc == SYS_Cap_Hoc.THCS)
                        tmp = tmp.Where(x => x.IS_THCS == true);
                    if (cap_hoc == SYS_Cap_Hoc.THPT)
                        tmp = tmp.Where(x => x.IS_THPT == true);
                    if (cap_hoc == SYS_Cap_Hoc.GDTX)
                    {
                        tmp = tmp.Where(x => x.IS_GDTX == true);
                        if (maLoaiLopGDTX != null)
                            tmp = tmp.Where(x => x.MA_LOAI_LOP_GDTX == maLoaiLopGDTX);
                    }
                    data = tmp.OrderBy(x => x.THU_TU).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<KHOI>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }

        public List<KhoiHocSinhEntity> thongkeSoLuongHocSinhByKhoiLop(long id_truong, string ma_cap_hoc, short id_nam_hoc)
        {
            List<KhoiHocSinhEntity> data = new List<KhoiHocSinhEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("KHOI", "HOC_SINH", "thongkeSoLuongHocSinhByKhoiLop", id_truong, ma_cap_hoc, id_nam_hoc);
            string where_cap = "";
            if (ma_cap_hoc == SYS_Cap_Hoc.MN)
                where_cap = " and KHOI.IS_MN=1";
            else if (ma_cap_hoc == SYS_Cap_Hoc.TH)
                where_cap = " and KHOI.IS_TH=1";
            else if (ma_cap_hoc == SYS_Cap_Hoc.THCS)
                where_cap = " and KHOI.IS_THCS=1";
            else if (ma_cap_hoc == SYS_Cap_Hoc.THPT)
                where_cap = " and KHOI.IS_THPT=1";
            else if (ma_cap_hoc == SYS_Cap_Hoc.GDTX)
                where_cap = " and KHOI.IS_GDTX=1";
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    try
                    {
                        string strQuery = string.Format(@"select KHOI.MA,KHOI.TEN,
                               sum(case when HOC_SINH.ID is not null then 1 else 0 end) SOLUONG_HS 
                               from KHOI
                               left join HOC_SINH on ID_KHOI=KHOI.MA and ID_TRUONG= :0 and ID_NAM_HOC=:1
                               and (HOC_SINH.IS_DELETE is null or HOC_SINH.IS_DELETE=0)
                               where  1=1 {0}
                               group by KHOI.MA,KHOI.TEN,KHOI.THU_TU
                               order by KHOI.THU_TU", where_cap
                                );
                        data = context.Database.SqlQuery<KhoiHocSinhEntity>(strQuery, id_truong, id_nam_hoc).ToList();
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
                    data = QICache.Get(strKeyCache) as List<KhoiHocSinhEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }

        public List<KhoiHocSinhEntity> thongkeSoLuongHocSinhByNamHoc(long id_truong, int Ma_Nam_hoc)
        {
            List<KhoiHocSinhEntity> data = new List<KhoiHocSinhEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_SINH", "thongkeSoLuongHocSinhByNamHoc", id_truong, Ma_Nam_hoc);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    try
                    {
                        string strQuery = string.Format(@"
                        SELECT (select count(ID) from HOC_SINH  where ID_TRUONG = :0 AND ID_NAM_HOC=:1 -4 AND (IS_DELETE is null or IS_DELETE=0)) AS SOLUONG_HS ,:1 -4 AS ID_NAM_HOC from dual
                        UNION ALL 
                        SELECT (select count(ID) from HOC_SINH  where ID_TRUONG = :0 AND ID_NAM_HOC=:1 -3 AND (IS_DELETE is null or IS_DELETE=0)) AS SOLUONG_HS ,:1 -3 AS ID_NAM_HOC from dual
                        UNION ALL 
                        SELECT (select count(ID) from HOC_SINH  where ID_TRUONG = :0 AND ID_NAM_HOC=:1 -2 AND (IS_DELETE is null or IS_DELETE=0)) AS SOLUONG_HS,:1 -2 AS ID_NAM_HOC from dual
                        UNION ALL 
                        SELECT (select count(ID) from HOC_SINH  where ID_TRUONG = :0 AND ID_NAM_HOC=:1 -1 AND (IS_DELETE is null or IS_DELETE=0)) AS SOLUONG_HS,:1 -1 AS ID_NAM_HOC from dual
                        UNION ALL 
                        SELECT (select count(ID) from HOC_SINH  where ID_TRUONG = :0 AND ID_NAM_HOC=:1 AND (IS_DELETE is null or IS_DELETE=0)) AS SOLUONG_HS ,:1  AS ID_NAM_HOC from dual");
                        data = context.Database.SqlQuery<KhoiHocSinhEntity>(strQuery, id_truong, Ma_Nam_hoc).ToList();
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
                    data = QICache.Get(strKeyCache) as List<KhoiHocSinhEntity>;
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
        public ResultEntity update(short ma, string ten, short? ma_loai_lop_gdtx, int? thu_tu, bool is_mn, bool is_th, bool is_thcs, bool is_thpt, bool is_gdtx, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    var sql = @"Update KHOI SET TEN = :0, MA_LOAI_LOP_GDTX = :1, THU_TU = :2, IS_MN = :3, IS_TH=:4, IS_THCS=:5, IS_THPT=:6, IS_GDTX=:7, NGAY_SUA=:8, NGUOI_SUA=:9 WHERE MA=:10";
                    context.Database.ExecuteSqlCommand(sql, ten, ma_loai_lop_gdtx, thu_tu, is_mn ? 1 : 0, is_th ? 1 : 0, is_thcs ? 1 : 0, is_thpt ? 1 : 0, is_gdtx ? 1 : 0, DateTime.Now, nguoi, ma);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("KHOI");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
                //res.Msg = ex.ToString();
            }
            return res;
        }
        public ResultEntity insert(short ma, string ten, short? ma_loai_lop_gdtx, int? thu_tu, bool is_mn, bool is_th, bool is_thcs, bool is_thpt, bool is_gdtx, long? nguoi)
        {
            KHOI detail = new KHOI();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail.MA = ma;
                    detail.TEN = ten;
                    detail.MA_LOAI_LOP_GDTX = ma_loai_lop_gdtx;
                    detail.THU_TU = thu_tu;
                    detail.IS_MN = is_mn;
                    detail.IS_TH = is_th;
                    detail.IS_THCS = is_thcs;
                    detail.IS_THPT = is_thpt;
                    detail.IS_GDTX = is_gdtx;
                    detail.NGAY_TAO = DateTime.Now;
                    detail.NGUOI_TAO = nguoi;
                    detail = context.KHOIs.Add(detail);
                    context.SaveChanges();
                }
                res.ResObject = detail;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("KHOI");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity delete(short ma, long? nguoi, bool is_delete = false)
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
                        var sql = @"update KHOI set  IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 where MA = :2";
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, ma);
                    }
                    else
                    {
                        var sql = @"delete from KHOI where MA = :0";
                        context.Database.ExecuteSqlCommand(sql, ma);
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("KHOI");
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
