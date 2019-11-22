using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class KhoaSoBO
    {
        #region get
        #region "get khoa so theo truong"
        public List<KHOA_SO_THEO_TRUONG> khoaSoTheoTruong(long id_truong, string ma_cap_hoc, short nam_hoc, short? giai_doan, short? hoc_ky, bool trang_thai)
        {
            List<KHOA_SO_THEO_TRUONG> data = new List<KHOA_SO_THEO_TRUONG>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("KHOA_SO_THEO_TRUONG", "khoaSoTheoTruong", id_truong, ma_cap_hoc, nam_hoc, giai_doan, hoc_ky, trang_thai);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.KHOA_SO_THEO_TRUONG where p.ID_TRUONG == id_truong && p.NAM_HOC == nam_hoc select p);
                    if (ma_cap_hoc == SYS_Cap_Hoc.TH)
                        tmp = tmp.Where(x => x.GIAI_DOAN == giai_doan);
                    if (ma_cap_hoc == SYS_Cap_Hoc.THCS || ma_cap_hoc == SYS_Cap_Hoc.THPT)
                        tmp = tmp.Where(x => x.HOC_KY == hoc_ky);
                    data = tmp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<KHOA_SO_THEO_TRUONG>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public KHOA_SO_THEO_TRUONG getKhoaSoTheoTruongCapHocKy(long id_truong, string ma_cap_hoc, short nam_hoc, short hoc_ky, short? giai_doan)
        {
            KHOA_SO_THEO_TRUONG data = new KHOA_SO_THEO_TRUONG();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("KHOA_SO_THEO_TRUONG", "getKhoaSoTheoTruongCapHocKy", id_truong, ma_cap_hoc, nam_hoc, hoc_ky, giai_doan);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.KHOA_SO_THEO_TRUONG where p.ID_TRUONG == id_truong && p.MA_CAP_HOC == ma_cap_hoc && p.NAM_HOC == nam_hoc && p.HOC_KY == hoc_ky select p);
                    if (ma_cap_hoc == SYS_Cap_Hoc.TH && giai_doan != null)
                        tmp = tmp.Where(x => x.GIAI_DOAN == giai_doan);
                    data = tmp.FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as KHOA_SO_THEO_TRUONG;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        #endregion
        public List<KHOA_SO_THEO_LOP> khoaSoTheoLop(long id_truong, long id_lop, string ma_cap_hoc, short nam_hoc, short? giai_doan, short? hoc_ky, bool trang_thai)
        {
            List<KHOA_SO_THEO_LOP> data = new List<KHOA_SO_THEO_LOP>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("KHOA_SO_THEO_LOP", "khoaSoTheoLop", id_truong, id_lop, ma_cap_hoc, nam_hoc, giai_doan, hoc_ky, trang_thai);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.KHOA_SO_THEO_LOP where p.ID_TRUONG == id_truong && p.ID_LOP == id_lop && p.NAM_HOC == nam_hoc select p);
                    if (ma_cap_hoc == SYS_Cap_Hoc.TH)
                        tmp = tmp.Where(x => x.GIAI_DOAN == giai_doan);
                    if (ma_cap_hoc == SYS_Cap_Hoc.THCS || ma_cap_hoc == SYS_Cap_Hoc.THPT)
                        tmp = tmp.Where(x => x.HOC_KY == hoc_ky);
                    data = tmp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<KHOA_SO_THEO_LOP>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        #region "get khoa so theo mon"
        public List<KhoaSoTheoMonEntity> khoaSoTheoMon(long id_truong, long? id_lop, string ma_cap_hoc, short nam_hoc, short? giai_doan, short? hoc_ky)
        {
            List<KhoaSoTheoMonEntity> data = new List<KhoaSoTheoMonEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("LOP_MON", "MON_HOC_TRUONG", "KHOA_SO_THEO_MON", "khoaSoTheoMon", id_truong, id_lop, ma_cap_hoc, nam_hoc, giai_doan, hoc_ky);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strSelect = string.Format(@"select mt.id_truong, lm.id_lop, lm.id_mon_truong as id_mon, mt.ten, lm.hoc_ky , km.trang_thai
                        ,km.id, km.ngay_khoa, km.nguoi_khoa
                        from lop_mon lm
                        join mon_hoc_truong mt on lm.id_mon_truong=mt.id
                        left join khoa_so_theo_mon km on lm.id_lop=km.id_lop and lm.id_mon_truong=km.id_mon 
                        and km.id_truong=mt.id_truong and km.NAM_HOC=mt.ID_NAM_HOC and km.MA_CAP_HOC=mt.MA_CAP_HOC {0}
                        where lm.trang_thai=1 and mt.id_truong=:0 and lm.id_lop=:1 and mt.ma_cap_hoc=:2 and mt.id_nam_hoc=:3 and lm.hoc_ky=:4 order by mt.thu_tu", (ma_cap_hoc == SYS_Cap_Hoc.TH && giai_doan != null)? " and km.giai_doan=" + giai_doan : "");
                    data = context.Database.SqlQuery<KhoaSoTheoMonEntity>(strSelect, id_truong, id_lop, ma_cap_hoc, nam_hoc, hoc_ky).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<KhoaSoTheoMonEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public KHOA_SO_THEO_MON checkKhoaSoTheoMon(long id_truong, long? id_lop, long? id_mon, string ma_cap_hoc, short nam_hoc, short? hoc_ky, short? giai_doan)
        {
            KHOA_SO_THEO_MON data = new KHOA_SO_THEO_MON();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("KHOA_SO_THEO_MON", "checkKhoaSoTheoMon", id_truong, id_lop, id_mon, ma_cap_hoc, nam_hoc, hoc_ky, giai_doan);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.KHOA_SO_THEO_MON where p.ID_TRUONG == id_truong && p.MA_CAP_HOC == ma_cap_hoc && p.ID_LOP == id_lop && p.ID_MON==id_mon && p.NAM_HOC == nam_hoc && p.HOC_KY==hoc_ky select p);
                    if (ma_cap_hoc == SYS_Cap_Hoc.TH)
                        tmp = tmp.Where(x => x.GIAI_DOAN == giai_doan);
                    data = tmp.FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as KHOA_SO_THEO_MON;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<KHOA_SO_THEO_MON> getListMonDaKhoaByTruong(long id_truong, string ma_cap_hoc, short nam_hoc, short hoc_ky, short? giai_doan)
        {
            List<KHOA_SO_THEO_MON> data = new List<KHOA_SO_THEO_MON>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("KHOA_SO_THEO_MON", "getListMonDaKhoaByTruong", id_truong, ma_cap_hoc, nam_hoc, hoc_ky, giai_doan);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.KHOA_SO_THEO_MON where p.ID_TRUONG == id_truong && p.MA_CAP_HOC==ma_cap_hoc && p.NAM_HOC == nam_hoc && p.HOC_KY == hoc_ky && !(p.TRANG_THAI == null || p.TRANG_THAI == false) select p);
                    if (ma_cap_hoc == SYS_Cap_Hoc.TH)
                        tmp = tmp.Where(x => x.GIAI_DOAN == giai_doan);
                    data = tmp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<KHOA_SO_THEO_MON>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<KHOA_SO_THEO_MON> getKhoaSoMonByTruong(long id_truong, string ma_cap_hoc, short nam_hoc, short hoc_ky, short? giai_doan)
        {
            List<KHOA_SO_THEO_MON> data = new List<KHOA_SO_THEO_MON>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("KHOA_SO_THEO_MON", "getKhoaSoMonByTruong", id_truong, ma_cap_hoc, nam_hoc, hoc_ky, giai_doan);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.KHOA_SO_THEO_MON where p.ID_TRUONG == id_truong && p.MA_CAP_HOC == ma_cap_hoc && p.NAM_HOC == nam_hoc && p.HOC_KY == hoc_ky select p);
                    if (ma_cap_hoc == SYS_Cap_Hoc.TH)
                        tmp = tmp.Where(x => x.GIAI_DOAN == giai_doan);
                    data = tmp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<KHOA_SO_THEO_MON>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public KHOA_SO_THEO_MON getKhoaSoMonByID(long id)
        {
            KHOA_SO_THEO_MON data = new KHOA_SO_THEO_MON();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("KHOA_SO_THEO_MON", "getKhoaSoMonByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.KHOA_SO_THEO_MON where p.ID == id select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as KHOA_SO_THEO_MON;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        #endregion
        #endregion
        #region set
        #region "khoa so theo truong"
        public ResultEntity updateByTruong(KHOA_SO_THEO_TRUONG detail_in, long? nguoi)
        {
            KHOA_SO_THEO_TRUONG detail = new KHOA_SO_THEO_TRUONG();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.KHOA_SO_THEO_TRUONG where p.ID == detail_in.ID select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.ID_TRUONG = detail_in.ID_TRUONG;
                        detail.MA_CAP_HOC = detail_in.MA_CAP_HOC;
                        detail.NGAY_KHOA = detail_in.NGAY_KHOA;
                        detail.NAM_HOC = detail_in.NAM_HOC;
                        detail.GIAI_DOAN = detail_in.GIAI_DOAN;
                        detail.TRANG_THAI = detail_in.TRANG_THAI;
                        detail.HOC_KY = detail_in.HOC_KY;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("KHOA_SO_THEO_TRUONG");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insertKhoaSoTheoTruong(KHOA_SO_THEO_TRUONG detail_in, long? nguoi, long id_truong, string ma_cap_hoc, short nam_hoc, short hoc_ky, short? giai_doan)
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
                    detail_in = context.KHOA_SO_THEO_TRUONG.Add(detail_in);
                    context.SaveChanges();
                    #region "update khoa so mon"
                    List<KHOA_SO_THEO_MON> lstKhoaMon = new List<KHOA_SO_THEO_MON>();
                    lstKhoaMon = getKhoaSoMonByTruong(id_truong, ma_cap_hoc, nam_hoc, hoc_ky, giai_doan);
                    if (lstKhoaMon.Count > 0)
                    {
                        for (int i = 0; i < lstKhoaMon.Count; i++)
                        {
                            var updateKhoaMon = @"update KHOA_SO_THEO_MON set TRANG_THAI=1, NGAY_KHOA=:0, NGUOI_KHOA=:1, NGAY_SUA=:2, NGUOI_SUA=:3 where ID=:4";
                            context.Database.ExecuteSqlCommand(updateKhoaMon, DateTime.Now, nguoi, DateTime.Now, nguoi, Convert.ToInt64(lstKhoaMon[i].ID));
                        }
                    }
                    #endregion
                    #region "insert khoa so mon"
                    var insertKhoaMon = "";
                    if (ma_cap_hoc == "TH")
                    {
                        insertKhoaMon = @"insert into khoa_so_theo_mon (id_truong, id_lop, id_mon, ngay_khoa, trang_thai
                        ,nam_hoc, hoc_ky, giai_doan, ma_cap_hoc, ngay_tao, nguoi_tao) 
                        select id_truong, id_lop, id_mon, ngay_khoa, trang_thai, nam_hoc
                        ,hoc_ky, giai_doan, ma_cap_hoc, ngay_tao, nguoi_tao from 
                        (select l.id_truong, lm.id_lop, lm.id_mon_truong as id_mon, :0 as ngay_khoa, 1 as trang_thai
                        ,l.id_nam_hoc as nam_hoc, lm.hoc_ky as hoc_ky, :1 as giai_doan, :2 as ngay_tao, :3 as nguoi_tao
                        ,case when l.MA_LOAI_LOP_GDTX is null and (l.id_khoi = 1 or l.id_khoi = 2 or l.id_khoi = 3 or l.id_khoi = 4 or l.id_khoi = 5) then 'TH'
                        when l.MA_LOAI_LOP_GDTX is null and (l.id_khoi = 6 or l.id_khoi = 7 or l.id_khoi = 8 or l.id_khoi = 9) then 'THCS' 
                        when l.MA_LOAI_LOP_GDTX is null and (l.id_khoi = 10 or l.id_khoi = 11 or l.id_khoi = 12) then 'THPT'
                        else 'GDTX' end as ma_cap_hoc
                        from lop_mon lm
                        left join lop l on l.id = lm.id_lop
                        where l.id_truong=:4 and l.id_nam_hoc=:5 and lm.hoc_ky=:6) tmp
                        where not exists (select * from khoa_so_theo_mon km where km.id_truong=tmp.id_truong 
                        and km.ma_cap_hoc=tmp.ma_cap_hoc and km.id_lop=tmp.id_lop and km.ID_MON = tmp.id_mon
                        and km.NAM_HOC = tmp.nam_hoc and km.HOC_KY=tmp.hoc_ky and km.giai_doan=tmp.giai_doan) and tmp.ma_cap_hoc=:7";
                        context.Database.ExecuteSqlCommand(insertKhoaMon, DateTime.Now, giai_doan, DateTime.Now, nguoi, id_truong, nam_hoc, hoc_ky, ma_cap_hoc);
                    }
                    else
                    {
                        insertKhoaMon = @"insert into khoa_so_theo_mon (id_truong, id_lop, id_mon, ngay_khoa, trang_thai
                        ,nam_hoc, hoc_ky, ma_cap_hoc, ngay_tao, nguoi_tao) 
                        select id_truong, id_lop, id_mon, ngay_khoa, trang_thai, nam_hoc
                        ,hoc_ky, giai_doan, ma_cap_hoc, ngay_tao, nguoi_tao from 
                        (select l.id_truong, lm.id_lop, lm.id_mon_truong as id_mon, :0 as ngay_khoa, 1 as trang_thai
                        ,l.id_nam_hoc as nam_hoc, lm.hoc_ky as hoc_ky, :1 as ngay_tao, :2 as nguoi_tao
                        ,case when l.MA_LOAI_LOP_GDTX is null and (l.id_khoi = 1 or l.id_khoi = 2 or l.id_khoi = 3 or l.id_khoi = 4 or l.id_khoi = 5) then 'TH'
                        when l.MA_LOAI_LOP_GDTX is null and (l.id_khoi = 6 or l.id_khoi = 7 or l.id_khoi = 8 or l.id_khoi = 9) then 'THCS' 
                        when l.MA_LOAI_LOP_GDTX is null and (l.id_khoi = 10 or l.id_khoi = 11 or l.id_khoi = 12) then 'THPT'
                        else 'GDTX' end as ma_cap_hoc
                        from lop_mon lm
                        left join lop l on l.id = lm.id_lop
                        where l.id_truong=:3 and l.id_nam_hoc=:4 and lm.hoc_ky=:5) tmp
                        where not exists (select * from khoa_so_theo_mon km where km.id_truong=tmp.id_truong 
                        and km.ma_cap_hoc=tmp.ma_cap_hoc and km.id_lop=tmp.id_lop and km.ID_MON = tmp.id_mon
                        and km.NAM_HOC = tmp.nam_hoc and km.HOC_KY=tmp.hoc_ky) and tmp.ma_cap_hoc=:6";
                        context.Database.ExecuteSqlCommand(insertKhoaMon, DateTime.Now, DateTime.Now, nguoi, id_truong, nam_hoc, hoc_ky, ma_cap_hoc);
                    }
                    #endregion
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("KHOA_SO_THEO_TRUONG");
                QICache.RemoveByFirstName("KHOA_SO_THEO_MON");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity updateTrangThaiKhoaTruong(long id, long? nguoi, long id_truong, string ma_cap_hoc, short nam_hoc, short hoc_ky, short? giai_doan)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    var sql = @"update KHOA_SO_THEO_TRUONG set TRANG_THAI=1, NGAY_KHOA=:0, NGUOI_KHOA=:1, NGAY_SUA=:2, NGUOI_SUA=:3 where ID=:4";
                    context.Database.ExecuteSqlCommand(sql, DateTime.Now, nguoi, DateTime.Now, nguoi, id);
                    #region "update khoa so mon"
                    List<KHOA_SO_THEO_MON> lstKhoaMon = new List<KHOA_SO_THEO_MON>();
                    lstKhoaMon = getKhoaSoMonByTruong(id_truong, ma_cap_hoc, nam_hoc, hoc_ky, giai_doan);
                    if (lstKhoaMon.Count > 0)
                    {
                        for (int i = 0; i < lstKhoaMon.Count; i++)
                        {
                            var updateKhoaMon = @"update KHOA_SO_THEO_MON set TRANG_THAI=1, NGAY_KHOA=:0, NGUOI_KHOA=:1, NGAY_SUA=:2, NGUOI_SUA=:3 where ID=:4";
                            context.Database.ExecuteSqlCommand(updateKhoaMon, DateTime.Now, nguoi, DateTime.Now, nguoi, Convert.ToInt64(lstKhoaMon[i].ID));
                        }
                    }
                    #endregion
                    #region "insert khoa so mon"
                    var insertKhoaMon = "";
                    if (ma_cap_hoc == "TH")
                    {
                        insertKhoaMon = @"insert into khoa_so_theo_mon (id_truong, id_lop, id_mon, ngay_khoa, trang_thai
                        ,nam_hoc, hoc_ky, giai_doan, ma_cap_hoc, ngay_tao, nguoi_tao) 
                        select id_truong, id_lop, id_mon, ngay_khoa, trang_thai, nam_hoc
                        ,hoc_ky, giai_doan, ma_cap_hoc, ngay_tao, nguoi_tao from 
                        (select l.id_truong, lm.id_lop, lm.id_mon_truong as id_mon, :0 as ngay_khoa, 1 as trang_thai
                        ,l.id_nam_hoc as nam_hoc, lm.hoc_ky as hoc_ky, :1 as giai_doan, :2 as ngay_tao, :3 as nguoi_tao
                        ,case when l.MA_LOAI_LOP_GDTX is null and (l.id_khoi = 1 or l.id_khoi = 2 or l.id_khoi = 3 or l.id_khoi = 4 or l.id_khoi = 5) then 'TH'
                        when l.MA_LOAI_LOP_GDTX is null and (l.id_khoi = 6 or l.id_khoi = 7 or l.id_khoi = 8 or l.id_khoi = 9) then 'THCS' 
                        when l.MA_LOAI_LOP_GDTX is null and (l.id_khoi = 10 or l.id_khoi = 11 or l.id_khoi = 12) then 'THPT'
                        else 'GDTX' end as ma_cap_hoc
                        from lop_mon lm
                        left join lop l on l.id = lm.id_lop
                        where l.id_truong=:4 and l.id_nam_hoc=:5 and lm.hoc_ky=:6) tmp
                        where not exists (select * from khoa_so_theo_mon km where km.id_truong=tmp.id_truong 
                        and km.ma_cap_hoc=tmp.ma_cap_hoc and km.id_lop=tmp.id_lop and km.ID_MON = tmp.id_mon
                        and km.NAM_HOC = tmp.nam_hoc and km.HOC_KY=tmp.hoc_ky and km.giai_doan=tmp.giai_doan) and tmp.ma_cap_hoc=:7";
                        context.Database.ExecuteSqlCommand(insertKhoaMon, DateTime.Now, giai_doan, DateTime.Now, nguoi, id_truong, nam_hoc, hoc_ky, ma_cap_hoc);
                    }
                    else
                    {
                        insertKhoaMon = @"insert into khoa_so_theo_mon (id_truong, id_lop, id_mon, ngay_khoa, trang_thai
                        ,nam_hoc, hoc_ky, ma_cap_hoc, ngay_tao, nguoi_tao) 
                        select id_truong, id_lop, id_mon, ngay_khoa, trang_thai, nam_hoc
                        ,hoc_ky, ma_cap_hoc, ngay_tao, nguoi_tao from 
                        (select l.id_truong, lm.id_lop, lm.id_mon_truong as id_mon, :0 as ngay_khoa, 1 as trang_thai
                        ,l.id_nam_hoc as nam_hoc, lm.hoc_ky as hoc_ky, :1 as ngay_tao, :2 as nguoi_tao
                        ,case when l.MA_LOAI_LOP_GDTX is null and (l.id_khoi = 1 or l.id_khoi = 2 or l.id_khoi = 3 or l.id_khoi = 4 or l.id_khoi = 5) then 'TH'
                        when l.MA_LOAI_LOP_GDTX is null and (l.id_khoi = 6 or l.id_khoi = 7 or l.id_khoi = 8 or l.id_khoi = 9) then 'THCS' 
                        when l.MA_LOAI_LOP_GDTX is null and (l.id_khoi = 10 or l.id_khoi = 11 or l.id_khoi = 12) then 'THPT'
                        else 'GDTX' end as ma_cap_hoc
                        from lop_mon lm
                        left join lop l on l.id = lm.id_lop
                        where l.id_truong=:3 and l.id_nam_hoc=:4 and lm.hoc_ky=:5) tmp
                        where not exists (select * from khoa_so_theo_mon km where km.id_truong=tmp.id_truong 
                        and km.ma_cap_hoc=tmp.ma_cap_hoc and km.id_lop=tmp.id_lop and km.ID_MON = tmp.id_mon
                        and km.NAM_HOC = tmp.nam_hoc and km.HOC_KY=tmp.hoc_ky) and tmp.ma_cap_hoc=:6";
                        context.Database.ExecuteSqlCommand(insertKhoaMon, DateTime.Now, DateTime.Now, nguoi, id_truong, nam_hoc, hoc_ky, ma_cap_hoc);
                    }
                    #endregion
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("KHOA_SO_THEO_TRUONG");
                QICache.RemoveByFirstName("KHOA_SO_THEO_MON");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity updateTrangThaiMoKhoaTruong(long id, long? nguoi, long id_truong, string ma_cap_hoc, short nam_hoc, short hoc_ky, short? giai_doan)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    var sql = @"update KHOA_SO_THEO_TRUONG set TRANG_THAI=0, NGAY_KHOA=:0, NGUOI_KHOA=:1, NGAY_SUA=:2, NGUOI_SUA=:3 where ID=:4";
                    context.Database.ExecuteSqlCommand(sql, DateTime.Now, nguoi, DateTime.Now, nguoi, id);
                    #region "update mở khóa sổ theo môn"
                    List<KHOA_SO_THEO_MON> lstKhoaMon = new List<KHOA_SO_THEO_MON>();
                    lstKhoaMon = getListMonDaKhoaByTruong(id_truong, ma_cap_hoc, nam_hoc, hoc_ky, giai_doan); 
                    if (lstKhoaMon.Count > 0)
                    {
                        for (int i = 0; i < lstKhoaMon.Count; i++)
                        {
                            var updateKhoaMon = @"update KHOA_SO_THEO_MON set TRANG_THAI=0, NGAY_KHOA=:0, NGUOI_KHOA=:1, NGAY_SUA=:2, NGUOI_SUA=:3 where ID=:4";
                            context.Database.ExecuteSqlCommand(updateKhoaMon, DateTime.Now, nguoi, DateTime.Now, nguoi, lstKhoaMon[i].ID);
                        }
                    }
                    #endregion
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("KHOA_SO_THEO_TRUONG");
                QICache.RemoveByFirstName("KHOA_SO_THEO_MON");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        #endregion
        public ResultEntity updateByLop(KHOA_SO_THEO_LOP detail_in, long? nguoi)
        {
            KHOA_SO_THEO_LOP detail = new KHOA_SO_THEO_LOP();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.KHOA_SO_THEO_LOP where p.ID == detail_in.ID select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.ID_TRUONG = detail_in.ID_TRUONG;
                        detail.ID_LOP = detail_in.ID_LOP;
                        detail.MA_CAP_HOC = detail_in.MA_CAP_HOC;
                        detail.NGAY_KHOA = detail_in.NGAY_KHOA;
                        detail.NAM_HOC = detail_in.NAM_HOC;
                        detail.GIAI_DOAN = detail_in.GIAI_DOAN;
                        detail.TRANG_THAI = detail_in.TRANG_THAI;
                        detail.HOC_KY = detail_in.HOC_KY;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("KHOA_SO_THEO_LOP");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity updateByMon(KHOA_SO_THEO_MON detail_in, long? nguoi)
        {
            KHOA_SO_THEO_MON detail = new KHOA_SO_THEO_MON();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.KHOA_SO_THEO_MON where p.ID == detail_in.ID select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.ID_TRUONG = detail_in.ID_TRUONG;
                        detail.ID_LOP = detail_in.ID_LOP;
                        detail.ID_MON = detail_in.ID_MON;
                        detail.MA_CAP_HOC = detail_in.MA_CAP_HOC;
                        detail.NGAY_KHOA = detail_in.NGAY_KHOA;
                        detail.NAM_HOC = detail_in.NAM_HOC;
                        detail.GIAI_DOAN = detail_in.GIAI_DOAN;
                        detail.TRANG_THAI = detail_in.TRANG_THAI;
                        detail.HOC_KY = detail_in.HOC_KY;
                        detail.NGUOI_KHOA = detail_in.NGUOI_KHOA;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("KHOA_SO_THEO_MON");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insertByLop(KHOA_SO_THEO_LOP detail_in, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail_in = context.KHOA_SO_THEO_LOP.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("KHOA_SO_THEO_LOP");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insertByMon(KHOA_SO_THEO_MON detail_in, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail_in.ID = context.Database.SqlQuery<long>("SELECT KHOA_SO_THEO_MON_SEQ.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                    detail_in.NGAY_TAO = DateTime.Now;
                    detail_in.NGUOI_TAO = nguoi;
                    detail_in = context.KHOA_SO_THEO_MON.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("KHOA_SO_THEO_MON");
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
                        var sql = @"update KHOA_SO_THEO_MON set IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 where ID = :2";
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, ma);
                    }
                    else
                    {
                        var sql = @"delete from KHOA_SO_THEO_MON where ID = :0";
                        context.Database.ExecuteSqlCommand(sql, ma);
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("KHOA_SO_THEO_MON");
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
