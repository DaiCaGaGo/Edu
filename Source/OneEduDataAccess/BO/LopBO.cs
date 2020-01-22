using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class LopBO
    {
        #region get
        #region get lop theo ten
        public LOP getLopByTen(long id_truong, short id_nam_hoc, short ma_khoi, string ten)
        {
            LOP data = new LOP();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("LOP", "getLopByTen", id_truong, id_nam_hoc, ma_khoi, ten);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.LOPs
                            where p.TEN == ten && p.ID_TRUONG == id_truong && p.ID_KHOI == ma_khoi && p.ID_NAM_HOC == id_nam_hoc && p.IS_DELETE != true
                            select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as LOP;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public long? getIdLopByTen(long id_truong, short id_nam_hoc, string ten)
        {
            long? data = null;
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("LOP", "getIdLopByTen", id_truong, id_nam_hoc, ten);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var sql = @"select id from lop 
                        where id_truong=:0 and ID_NAM_HOC=:1 and ten=:2
                        and not (IS_DELETE is not null and IS_DELETE = 1)";
                    data = context.Database.SqlQuery<long?>(sql, id_truong, id_nam_hoc, ten).FirstOrDefault();
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
        #region get tat ca cac LOP
        public List<LOP> getAllLop(bool is_all = false, short id_all = 0, string text_all = "Chọn tất cả")
        {
            List<LOP> data = new List<LOP>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("LOP", "getAllLop", is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.LOPs where p.IS_DELETE != true select p).ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<LOP>();
                        LOP item_all = new LOP();
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
                    data = QICache.Get(strKeyCache) as List<LOP>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        #endregion
        #region get tat ca cac LOP theo truong
        public List<LopEntity> getLopByTruongCapHocAndKhoi(string ma_cap_hoc, long idTruong, short? maKhoi, short ma_nam_hoc, string txtSearch, short? maLoaiLopGDTX = null)
        {
            List<LopEntity> data = new List<LopEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("LOP", "KHOI", "getLopByTruongCapHocAndKhoi", ma_cap_hoc, idTruong, maKhoi, ma_nam_hoc, txtSearch, maLoaiLopGDTX);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from lop in context.LOPs
                               join khoi in context.KHOIs on lop.ID_KHOI equals khoi.MA
                               where lop.IS_DELETE != true && lop.ID_TRUONG == idTruong && lop.ID_NAM_HOC == ma_nam_hoc
                               select new LopEntity()
                               {
                                   ID = lop.ID,
                                   ID_GVCN = lop.ID_GVCN,
                                   ID_KHOI = lop.ID_KHOI,
                                   TEN_KHOI = khoi.TEN,
                                   ID_NAM_HOC = lop.ID_NAM_HOC,
                                   ID_TRUONG = lop.ID_TRUONG,
                                   IS_DELETE = lop.IS_DELETE,
                                   MA_LOAI_LOP_GDTX = lop.MA_LOAI_LOP_GDTX,
                                   TEN = lop.TEN,
                                   THU_TU = lop.THU_TU,
                                   NGAY_TAO = lop.NGAY_TAO,
                                   NGUOI_TAO = lop.NGUOI_TAO,
                                   NGUOI_SUA = lop.NGUOI_SUA,
                                   NGAY_SUA = lop.NGAY_SUA
                               });
                    if (maKhoi != null)
                        tmp = tmp.Where(lop => lop.ID_KHOI == maKhoi);
                    else
                    {
                        if (ma_cap_hoc == SYS_Cap_Hoc.MN) tmp = tmp.Where(x => x.ID_KHOI > 12);
                        if (ma_cap_hoc == SYS_Cap_Hoc.TH) tmp = tmp.Where(x => x.ID_KHOI >= 1 && x.ID_KHOI <= 5);
                        if (ma_cap_hoc == SYS_Cap_Hoc.THCS) tmp = tmp.Where(x => x.ID_KHOI >= 6 && x.ID_KHOI <= 9);
                        if (ma_cap_hoc == SYS_Cap_Hoc.THPT) tmp = tmp.Where(x => x.ID_KHOI >= 10 && x.ID_KHOI <= 12);
                        if (ma_cap_hoc == SYS_Cap_Hoc.GDTX)
                        {
                            if (maLoaiLopGDTX == 1) tmp = tmp.Where(x => x.ID_KHOI >= 1 && x.ID_KHOI <= 5);
                            if (maLoaiLopGDTX == 2) tmp.Where(x => x.ID_KHOI >= 6 && x.ID_KHOI <= 9);
                            if (maLoaiLopGDTX == 3) tmp = tmp.Where(x => x.ID_KHOI >= 10 && x.ID_KHOI <= 12);
                        }
                    }
                    if (!string.IsNullOrEmpty(txtSearch))
                        tmp = tmp.Where(lop => lop.TEN.ToLower().Contains(txtSearch.ToLower()));
                    tmp = tmp.OrderBy(x => x.ID_KHOI).ThenBy(x => x.THU_TU).ThenBy(x => x.TEN);
                    data = tmp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<LopEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<LopEntity> getLopByTruongCapHocAndKhoiLop(string ma_cap_hoc, long idTruong, short? maKhoi, long? id_lop, short? maLoaiLopGDTX = null)
        {
            List<LopEntity> data = new List<LopEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("LOP", "KHOI", "getLopByTruongCapHocAndKhoiLop", ma_cap_hoc, idTruong, maKhoi, id_lop, maLoaiLopGDTX);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from lop in context.LOPs
                               join khoi in context.KHOIs on lop.ID_KHOI equals khoi.MA
                               where lop.IS_DELETE != true && lop.ID_TRUONG == idTruong
                               select new LopEntity()
                               {
                                   ID = lop.ID,
                                   ID_GVCN = lop.ID_GVCN,
                                   ID_KHOI = lop.ID_KHOI,
                                   TEN_KHOI = khoi.TEN,
                                   ID_NAM_HOC = lop.ID_NAM_HOC,
                                   ID_TRUONG = lop.ID_TRUONG,
                                   IS_DELETE = lop.IS_DELETE,
                                   MA_LOAI_LOP_GDTX = lop.MA_LOAI_LOP_GDTX,
                                   TEN = lop.TEN,
                                   THU_TU = lop.THU_TU,
                                   NGAY_TAO = lop.NGAY_TAO,
                                   NGUOI_TAO = lop.NGUOI_TAO,
                                   NGUOI_SUA = lop.NGUOI_SUA,
                                   NGAY_SUA = lop.NGAY_SUA
                               });
                    if (maKhoi != null)
                        tmp = tmp.Where(lop => lop.ID_KHOI == maKhoi);
                    else
                    {
                        if (ma_cap_hoc == SYS_Cap_Hoc.MN) tmp = tmp.Where(x => x.ID_KHOI > 12);
                        if (ma_cap_hoc == SYS_Cap_Hoc.TH) tmp = tmp.Where(x => x.ID_KHOI >= 1 && x.ID_KHOI <= 5);
                        if (ma_cap_hoc == SYS_Cap_Hoc.THCS) tmp = tmp.Where(x => x.ID_KHOI >= 6 && x.ID_KHOI <= 9);
                        if (ma_cap_hoc == SYS_Cap_Hoc.THPT) tmp = tmp.Where(x => x.ID_KHOI >= 10 && x.ID_KHOI <= 12);
                        if (ma_cap_hoc == SYS_Cap_Hoc.GDTX)
                        {
                            if (maLoaiLopGDTX == 1) tmp = tmp.Where(x => x.ID_KHOI >= 1 && x.ID_KHOI <= 5);
                            if (maLoaiLopGDTX == 2) tmp.Where(x => x.ID_KHOI >= 6 && x.ID_KHOI <= 9);
                            if (maLoaiLopGDTX == 3) tmp = tmp.Where(x => x.ID_KHOI >= 10 && x.ID_KHOI <= 12);
                        }
                    }
                    if (id_lop != null) tmp = tmp.Where(x => x.ID == id_lop);
                    tmp = tmp.OrderBy(x => x.ID_KHOI).ThenBy(x => x.THU_TU).ThenBy(x => x.TEN);
                    data = tmp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<LopEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        #endregion
        #region get Lop theo id
        public LopEntity getLopById(long id)
        {
            LopEntity data = new LopEntity();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("LOP", "KHOI", "LOAI_LOP_GDTX", "GIAO_VIEN", "getLopById", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from lop in context.LOPs
                            join khoi in context.KHOIs on lop.ID_KHOI equals khoi.MA
                            join gdtx in context.LOAI_LOP_GDTX on lop.MA_LOAI_LOP_GDTX equals gdtx.MA into lop_gdtx
                            join gvcn in context.GIAO_VIEN on lop.ID_GVCN equals gvcn.ID into lop_gvcn
                            from gdtx in lop_gdtx.DefaultIfEmpty()
                            from gvcn in lop_gvcn.DefaultIfEmpty()
                            where lop.ID == id && lop.IS_DELETE != true
                            select new LopEntity()
                            {
                                ID = lop.ID,
                                ID_LOP = lop.ID,
                                ID_GVCN = lop.ID_GVCN,
                                ID_KHOI = lop.ID_KHOI,
                                TEN_KHOI = khoi.TEN,
                                MA_LOAI_LOP_GDTX = gdtx.MA,
                                LOAI_LOP_GDTX = gdtx.TEN,
                                TEN_LOP = lop.TEN,
                                THU_TU = lop.THU_TU,
                                TEN_GVCN = gvcn.TEN,
                                LOAI_CHEN_TIN = lop.LOAI_CHEN_TIN,
                                TIEN_TO = lop.TIEN_TO,
                                TEN = lop.TEN,
                                ID_TRUONG = lop.ID_TRUONG,
                                ID_NAM_HOC = lop.ID_NAM_HOC,
                                IS_DELETE = lop.IS_DELETE,
                                NGAY_TAO = lop.NGAY_TAO,
                                NGUOI_TAO = lop.NGUOI_TAO,
                                NGAY_SUA = lop.NGAY_SUA,
                                NGUOI_SUA = lop.NGUOI_SUA
                            }).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as LopEntity;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        #endregion
        #region get Lop theo khoi
        public List<LOP> getLopByLstKhoiNamHoc(string ma_cap_hoc, long idTruong, string lstMaKhoi, short? maNamHoc, bool is_all = false, short id_all = 0, string text_all = "Chọn tất cả")
        {
            List<LOP> data = new List<LOP>();
            var QICache = new DefaultCacheProvider();
            List<string> lststrKhoi = new List<string>();
            if (!string.IsNullOrEmpty(lstMaKhoi))
                lststrKhoi = lstMaKhoi.Split(',').ToList();
            List<short> lstKhoi = new List<short>();
            foreach (var item in lststrKhoi)
            {
                try
                {
                    lstKhoi.Add(Convert.ToInt16(item));
                }
                catch
                {
                }
            }
            string strKeyCache = QICache.BuildCachedKey("LOP", "getLopByLstKhoiNamHoc", idTruong, lstMaKhoi, maNamHoc, is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.LOPs where p.ID_TRUONG == idTruong && lstKhoi.Contains(p.ID_KHOI) && p.ID_NAM_HOC == maNamHoc && p.IS_DELETE != true select p);
                    if (ma_cap_hoc == SYS_Cap_Hoc.TH)
                        tmp = tmp.Where(x => x.ID_KHOI >= 1 && x.ID_KHOI <= 5);
                    else if (ma_cap_hoc == SYS_Cap_Hoc.THCS)
                        tmp = tmp.Where(x => x.ID_KHOI >= 6 && x.ID_KHOI <= 9);
                    else if (ma_cap_hoc == SYS_Cap_Hoc.THPT)
                        tmp = tmp.Where(x => x.ID_KHOI >= 10 && x.ID_KHOI <= 12);
                    else if (ma_cap_hoc == SYS_Cap_Hoc.MN)
                        tmp = tmp.Where(x => x.ID_KHOI > 12);
                    data = tmp.OrderBy(x => x.ID_KHOI).ThenBy(x => x.THU_TU).ThenBy(x => x.TEN).ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<LOP>();
                        LOP item_all = new LOP();
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
                    data = QICache.Get(strKeyCache) as List<LOP>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<LOP> getLopByKhoiNamHoc(string ma_cap_hoc, long idTruong, short? maKhoi, short? maNamHoc, bool is_all = false, short id_all = 0, string text_all = "Chọn tất cả")
        {
            List<LOP> data = new List<LOP>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("LOP", "getLopByKhoiNamHoc", ma_cap_hoc, idTruong, maKhoi, maNamHoc, is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.LOPs
                               where p.ID_TRUONG == idTruong && p.ID_NAM_HOC == maNamHoc && p.IS_DELETE != true
                               select p);
                    if (ma_cap_hoc == SYS_Cap_Hoc.TH)
                        tmp = tmp.Where(x => x.ID_KHOI >= 1 && x.ID_KHOI <= 5);
                    else if (ma_cap_hoc == SYS_Cap_Hoc.THCS)
                        tmp = tmp.Where(x => x.ID_KHOI >= 6 && x.ID_KHOI <= 9);
                    else if (ma_cap_hoc == SYS_Cap_Hoc.THPT)
                        tmp = tmp.Where(x => x.ID_KHOI >= 10 && x.ID_KHOI <= 12);
                    else if (ma_cap_hoc == SYS_Cap_Hoc.MN)
                        tmp = tmp.Where(x => x.ID_KHOI > 12);
                    if (maKhoi != null && maKhoi != 0)
                        tmp = tmp.Where(x => x.ID_KHOI == maKhoi.Value);
                    data = tmp.OrderBy(x => x.ID_KHOI).ThenBy(x => x.THU_TU).ThenBy(x => x.TEN).ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<LOP>();
                        LOP item_all = new LOP();
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
                    data = QICache.Get(strKeyCache) as List<LOP>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<LOP> getLopByLopKhoiNamHocNotIdLop(string ma_cap_hoc, long idTruong, short? maKhoi, short? maNamHoc, long? id_lop, bool is_all = false, short id_all = 0, string text_all = "Chọn tất cả")
        {
            List<LOP> data = new List<LOP>();
            data = getLopByKhoiNamHoc(ma_cap_hoc, idTruong, maKhoi, maNamHoc).Where(x => x.ID != id_lop).ToList();
            if (is_all)
            {
                if (data == null) data = new List<LOP>();
                LOP item_all = new LOP();
                item_all.ID = id_all;
                item_all.TEN = text_all;
                data.Insert(0, item_all);
            }
            return data;
        }
        #endregion
        public List<LOP> getLopByTruongCapNamHoc(string ma_cap_hoc, long idTruong, short id_nam_hoc)
        {
            List<LOP> data = new List<LOP>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("LOP", "getLopByTruongCapNamHoc", ma_cap_hoc, idTruong, id_nam_hoc);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.LOPs where p.ID_TRUONG == idTruong && p.ID_NAM_HOC == id_nam_hoc && p.IS_DELETE != true select p);
                    if (ma_cap_hoc == SYS_Cap_Hoc.TH)
                        tmp = tmp.Where(x => x.ID_KHOI >= 1 && x.ID_KHOI <= 5);
                    else if (ma_cap_hoc == SYS_Cap_Hoc.THCS)
                        tmp = tmp.Where(x => x.ID_KHOI >= 6 && x.ID_KHOI <= 9);
                    else if (ma_cap_hoc == SYS_Cap_Hoc.THPT)
                        tmp = tmp.Where(x => x.ID_KHOI >= 10 && x.ID_KHOI <= 12);
                    else if (ma_cap_hoc == SYS_Cap_Hoc.MN)
                        tmp = tmp.Where(x => x.ID_KHOI > 12);
                    tmp = tmp.OrderBy(x => x.ID_KHOI).ThenBy(x => x.THU_TU).ThenBy(x => x.TEN);
                    data = tmp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<LOP>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public long? getTongSoLopTheoTruongMaCapNamHoc(long id_truong, string ma_cap_hoc, short nam_hoc)
        {
            long? data = null;
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("LOP", "getTongSoLopTheoTruongMaCapNamHoc", id_truong, ma_cap_hoc, nam_hoc);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var sql = @"select count(id) from lop 
                        where not (IS_DELETE is not null and IS_DELETE = 1)
                        and id_truong=:0 and ID_NAM_HOC=:1";
                    List<object> parameter = new List<object>();
                    parameter.Add(id_truong);
                    parameter.Add(nam_hoc);
                    if (ma_cap_hoc == SYS_Cap_Hoc.TH)
                        sql += " and (ID_KHOI>=1 and ID_KHOI<=5)";
                    else if (ma_cap_hoc == SYS_Cap_Hoc.THCS)
                        sql += " and (ID_KHOI>=6 and ID_KHOI<=9)";
                    else if (ma_cap_hoc == SYS_Cap_Hoc.THPT)
                        sql += " and (ID_KHOI>=10 and ID_KHOI<=12)";
                    else if (ma_cap_hoc == SYS_Cap_Hoc.MN)
                        sql += " and (ID_KHOI>12)";
                    data = context.Database.SqlQuery<long?>(sql, parameter.ToArray()).FirstOrDefault();
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
        public int? getMaxThuTuByTruongKhoiNamHoc(long id_truong, short? ma_khoi, short id_nam_hoc)
        {
            int? data = null;
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("LOP", "getMaxThuTuByTruongKhoiNamHoc", id_truong, ma_khoi, id_nam_hoc);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var sql = @"select MAX(THU_TU) as THU_TU from LOP 
                        where ID_TRUONG = :0 and ID_NAM_HOC=:1";
                    if (ma_khoi != null)
                        sql += @" and ID_KHOI=" + ma_khoi;
                    data = context.Database.SqlQuery<int?>(sql, id_truong, id_nam_hoc).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as int?;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<LopMonEntity> getLopNotExistDotThuByTruongDotThu(long id_truong, string ma_cap_hoc, short id_nam_hoc, long? id_dot_thu, string lstMaKhoi)
        {
            List<LopMonEntity> data = new List<LopMonEntity>();
            var QICache = new DefaultCacheProvider();
            List<string> lststrKhoi = new List<string>();
            if (!string.IsNullOrEmpty(lstMaKhoi))
                lststrKhoi = lstMaKhoi.Split(',').ToList();
            List<short> lstKhoi = new List<short>();
            foreach (var item in lststrKhoi)
            {
                try
                {
                    lstKhoi.Add(Convert.ToInt16(item));
                }
                catch { }
            }
            string strKeyCache = QICache.BuildCachedKey("LOP", "HOC_PHI_DOT_THU_LOP", "getLopNotExistDotThuByTruongDotThu", id_truong, ma_cap_hoc, id_nam_hoc, id_dot_thu, lstMaKhoi);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    try
                    {
                        string strQuery = string.Format(@"select a.ID,a.TEN from lop a where 
                            a.id_truong=:0 and a.id_nam_hoc=:1
                            and not (a.is_delete is not null and a.is_delete=1)");
                        #region Khối
                        if (lstKhoi != null && lstKhoi.Count == 1)
                            strQuery += string.Format(@" and a.ID_KHOI ={0}", lstKhoi[0]);
                        else if (lstKhoi != null && lstKhoi.Count > 1)
                        {
                            strQuery += string.Format(" and not ( a.ID_KHOI !={0}", lstKhoi[0]);
                            for (int i = 1; i < lstKhoi.Count; i++)
                            {
                                strQuery += string.Format(" and a.ID_KHOI !={0}", lstKhoi[i]);
                            }
                            strQuery += " )";
                        }
                        else if (lstKhoi == null || lstKhoi.Count == 0)
                        {
                            if (ma_cap_hoc == SYS_Cap_Hoc.MN)
                                strQuery += " and not (a.ID_KHOI != 13 and a.ID_KHOI != 14)";
                            else if (ma_cap_hoc == SYS_Cap_Hoc.TH)
                                strQuery += " and not (a.ID_KHOI != 1 and a.ID_KHOI != 2 and a.ID_KHOI != 3 and a.ID_KHOI != 4 and a.ID_KHOI != 5)";
                            else if (ma_cap_hoc == SYS_Cap_Hoc.THCS)
                                strQuery += " and not (a.ID_KHOI != 6 and a.ID_KHOI != 7 and a.ID_KHOI != 8 and a.ID_KHOI != 9)";
                            else if (ma_cap_hoc == SYS_Cap_Hoc.THPT)
                                strQuery += " and not (a.ID_KHOI != 10 and a.ID_KHOI != 11 and a.ID_KHOI != 12";
                        }
                        #endregion
                        if (id_dot_thu != null)
                            strQuery += string.Format(@" and not exists (select id from HOC_PHI_DOT_THU_LOP b where a.id=b.ID_LOP and b.ID_DOT_THU={0} and not (b.is_delete is not null and b.is_delete=1))", id_dot_thu);
                        else
                            strQuery += " and not exists (select id from HOC_PHI_DOT_THU_LOP b where a.id=b.ID_LOP and not (b.is_delete is not null and b.is_delete=1))";
                        strQuery += @" order by a.id_khoi,a.thu_tu";
                        data = context.Database.SqlQuery<LopMonEntity>(strQuery, id_truong, id_nam_hoc).ToList();
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
                    data = QICache.Get(strKeyCache) as List<LopMonEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public LOP getLopByGVCN(short id_nam_hoc, long id_gvcn)
        {
            LOP data = new LOP();
            using (oneduEntities context = new oneduEntities())
            {
                data = (from p in context.LOPs
                        where p.ID_NAM_HOC == id_nam_hoc && p.ID_GVCN == id_gvcn && p.IS_DELETE != true
                        select p).FirstOrDefault();
            }
            return data;
        }
        public List<LopEntity> getLopGVCNByTruongCapNamHoc(string ma_cap_hoc, long idTruong, short id_nam_hoc)
        {
            List<LopEntity> data = new List<LopEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("LOP", "getLopGVCNByTruongCapNamHoc", ma_cap_hoc, idTruong, id_nam_hoc);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string sql = string.Format(@"select l.id as id_lop, l.ten as TEN_LOP, l.id_khoi, l.id_truong, gv.sdt as SDT_GVCN, gv.ten as TEN_GVCN, l.id_gvcn from lop l
                        left join giao_vien gv on l.id_gvcn = gv.id
                        where l.id_truong = {0} and l.id_nam_hoc = {1} and not(l.is_delete is not null and l.is_delete = 1)", idTruong, id_nam_hoc);
                    if (ma_cap_hoc == SYS_Cap_Hoc.TH)
                        sql += " and l.id_khoi >=1 and l.id_khoi <=5";
                    else if (ma_cap_hoc == SYS_Cap_Hoc.THCS)
                        sql += " and l.id_khoi >=6 and l.id_khoi <=9";
                    else if (ma_cap_hoc == SYS_Cap_Hoc.THPT)
                        sql += " and l.id_khoi >=10 and l.id_khoi <=12";
                    else if (ma_cap_hoc == SYS_Cap_Hoc.MN)
                        sql += " and l.id_khoi > 12";
                    data = context.Database.SqlQuery<LopEntity>(sql).ToList();

                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<LopEntity>;
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
        public ResultEntity update(LOP detail_in, long? nguoi)
        {
            LOP detail = new LOP();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.LOPs
                              where p.ID == detail_in.ID && p.IS_DELETE != true
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.TEN = detail_in.TEN;
                        detail.ID_KHOI = detail_in.ID_KHOI;
                        detail.ID_GVCN = detail_in.ID_GVCN;
                        detail.ID_NAM_HOC = detail_in.ID_NAM_HOC;
                        detail.ID_TRUONG = detail_in.ID_TRUONG;
                        detail.MA_LOAI_LOP_GDTX = detail_in.MA_LOAI_LOP_GDTX;
                        detail.THU_TU = detail_in.THU_TU;
                        detail.LOAI_CHEN_TIN = detail_in.LOAI_CHEN_TIN;
                        detail.TIEN_TO = detail_in.TIEN_TO;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("LOP");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(LOP detail_in, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail_in.ID = context.Database.SqlQuery<long>("SELECT LOP_SEQ2.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                    detail_in.NGAY_TAO = DateTime.Now;
                    detail_in.NGUOI_TAO = nguoi;
                    detail_in.NGAY_SUA = DateTime.Now;
                    detail_in.NGUOI_SUA = nguoi;
                    detail_in = context.LOPs.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("LOP");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity delete(long ma, long? nguoi, bool is_delete_all, bool is_delete = false)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            string sql = string.Empty;
            var QICache = new DefaultCacheProvider();
            try
            {
                using (var context = new oneduEntities())
                {
                    if (is_delete_all)
                    {
                        sql = @"delete CA_HOC where ID_LOP = :0";
                        context.Database.ExecuteSqlCommand(sql, ma);
                        sql = @"delete CHUYEN_CAN where ID_LOP = :0";
                        context.Database.ExecuteSqlCommand(sql, ma);
                        sql = @"delete DIEM_CHI_TIET where ID_LOP = :0";
                        context.Database.ExecuteSqlCommand(sql, ma);
                        sql = @"delete DIEM_TONG_KET where ID_LOP = :0";
                        context.Database.ExecuteSqlCommand(sql, ma);

                        sql = @"delete NHAN_XET_HANG_NGAY where ID_LOP = :0";
                        context.Database.ExecuteSqlCommand(sql, ma);
                        sql = @"delete HOC_SINH where ID_LOP = :0";
                        context.Database.ExecuteSqlCommand(sql, ma);

                        sql = @"delete LOP_MON where ID_LOP = :0";
                        context.Database.ExecuteSqlCommand(sql, ma);
                        sql = "delete from LOP where ID = :0";
                        context.Database.ExecuteSqlCommand(sql, ma);

                        QICache.RemoveByFirstName("CA_HOC");
                        QICache.RemoveByFirstName("CHUYEN_CAN");
                        QICache.RemoveByFirstName("DIEM_CHI_TIET");
                        QICache.RemoveByFirstName("DIEM_TONG_KET");
                        QICache.RemoveByFirstName("HOC_SINH");
                        QICache.RemoveByFirstName("NHAN_XET_HANG_NGAY");
                        QICache.RemoveByFirstName("LOP_MON");
                    }
                    else if (!is_delete)
                    {
                        sql = @"update LOP set  IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 where ID = :2";
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, ma);
                    }
                    else
                    {
                        sql = "delete from LOP where ID = :0";
                        context.Database.ExecuteSqlCommand(sql, ma);
                    }
                }
                QICache.RemoveByFirstName("LOP");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }

        #region tạo lớp năm học mới từ các lớp từ năm học cũ
        public ResultEntity TaoLopNamMoiTheoNamCu(long id_truong, int ma_nam_hoc, string ma_cap_hoc, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    string sql = string.Format(@"
                            insert into lop (ten,
                                           id_khoi,
                                           id_gvcn,
                                           id_nam_hoc,
                                           id_truong,
                                           ma_loai_lop_gdtx,
                                           thu_tu,
                                           loai_chen_tin,
                                           tien_to,
                                           ngay_tao,
                                           nguoi_tao)
                                          select ten,
                                                 id_khoi,
                                                 id_gvcn,
                                                 :0,
                                                 id_truong,
                                                 ma_loai_lop_gdtx,
                                                 thu_tu,
                                                 loai_chen_tin,
                                                 tien_to,
                                                 sysdate,
                                                 :1
                                            from lop a
                                           where a.id_truong = :2
                                             and a.id_nam_hoc = :3");
                    if (ma_cap_hoc == SYS_Cap_Hoc.MN)
                        sql += " and (a.id_khoi=13 or a.id_khoi=14)";
                    else if (ma_cap_hoc == SYS_Cap_Hoc.TH)
                        sql += " and (a.id_khoi >= 1 and a.id_khoi <= 5)";
                    else if (ma_cap_hoc == SYS_Cap_Hoc.THCS)
                        sql += " and (a.id_khoi >= 6 and a.id_khoi <= 9)";
                    else if (ma_cap_hoc == SYS_Cap_Hoc.THPT)
                        sql += " and (a.id_khoi >= 10 and a.id_khoi <= 12)";
                    sql += string.Format(@"and not (a.is_delete is not null and a.is_delete = 1)
                                             and not exists (select *
                                                    from lop b
                                                   where b.id_truong = :4
                                                     and b.id_nam_hoc = :5
                                                     and b.id_khoi = a.id_khoi
                                                     and b.ten = a.ten)");
                    res.ResObject = context.Database.ExecuteSqlCommand(sql, ma_nam_hoc, nguoi, id_truong, ma_nam_hoc - 1, id_truong, ma_nam_hoc);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("LOP");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        #endregion
        #endregion
    }
}
