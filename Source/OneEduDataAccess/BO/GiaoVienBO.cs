using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class GiaoVienBO
    {
        #region get
        #region get giao vien theo ten
        public GIAO_VIEN getGiaoVienByTen(long id_truong, string ten)
        {
            GIAO_VIEN data = new GIAO_VIEN();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("GIAO_VIEN", "getGiaoVienByTen", id_truong, ten);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.GIAO_VIEN
                            where p.HO_TEN == ten && p.ID_TRUONG == id_truong && p.IS_DELETE != true
                            select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as GIAO_VIEN;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        #endregion

        public List<GIAO_VIEN> getGiaoVien(long idTruong, string ten, bool is_all = false, long id_all = 0, string text_all = "Chọn tất cả")
        {
            List<GIAO_VIEN> data = new List<GIAO_VIEN>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("GIAO_VIEN", "getGiaoVien", idTruong, ten, is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.GIAO_VIEN where p.IS_DELETE != true && p.ID_TRUONG == idTruong select p);
                    if (!string.IsNullOrEmpty(ten))
                        tmp = tmp.Where(x => x.TEN.ToLower().Contains(ten.ToLower()));
                    tmp = tmp.OrderBy(x => x.TEN);
                    data = tmp.ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<GIAO_VIEN>();
                        GIAO_VIEN item_all = new GIAO_VIEN();
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
                    data = QICache.Get(strKeyCache) as List<GIAO_VIEN>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<LOP> checkExistsGiaoVienLop(long id_truong, long id_giao_vien)
        {
            List<LOP> data = new List<LOP>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("LOP", "checkExistsGiaoVienLop", id_truong, id_giao_vien);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.LOPs where p.ID_TRUONG == id_truong && p.ID_GVCN == id_giao_vien && p.IS_DELETE != true select p).ToList();
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
        public List<GIAO_VIEN> getGiaoVienByTruongTenSDT(long idTruong, string ten, string sdt, short? ma_trang_thai, short? ma_gioi_tinh)
        {
            List<GIAO_VIEN> data = new List<GIAO_VIEN>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("GIAO_VIEN", "getGiaoVienByTruongTenSDT", idTruong, ten, sdt, ma_trang_thai, ma_gioi_tinh);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.GIAO_VIEN
                               where p.IS_DELETE != true && p.ID_TRUONG == idTruong
                               select p);
                    if (!string.IsNullOrEmpty(ten))
                        tmp = tmp.Where(x => x.HO_TEN.ToLower().Contains(ten.ToLower()));
                    if (ma_trang_thai != null)
                        tmp = tmp.Where(x => x.MA_TRANG_THAI == ma_trang_thai);
                    if (ma_gioi_tinh != null)
                        tmp = tmp.Where(x => x.MA_GIOI_TINH == ma_gioi_tinh);
                    if (!string.IsNullOrEmpty(sdt))
                        tmp = tmp.Where(x => x.SDT.Contains(sdt));
                    tmp = tmp.OrderBy(x => x.TEN).ThenBy(x => x.THU_TU);
                    data = tmp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<GIAO_VIEN>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public GIAO_VIEN getGiaoVienByID(long id)
        {
            GIAO_VIEN data = new GIAO_VIEN();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("GIAO_VIEN", "getGiaoVienByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.GIAO_VIEN where p.ID == id select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as GIAO_VIEN;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<GIAO_VIEN> getGiaoVienByTruong(string id, bool is_all = false, long id_all = 0, string text_all = "Chọn tất cả")
        {
            List<GIAO_VIEN> data = new List<GIAO_VIEN>();
            long idTruong = 0;
            if (!string.IsNullOrEmpty(id)) idTruong = Convert.ToInt64(id);
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("GIAO_VIEN", "getGiaoVienByTruong", idTruong, is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.GIAO_VIEN where p.ID_TRUONG == idTruong && p.MA_TRANG_THAI == 1 && p.IS_DELETE != true orderby p.ID_CHUC_VU select p).ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<GIAO_VIEN>();
                        GIAO_VIEN item_all = new GIAO_VIEN();
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
                    data = QICache.Get(strKeyCache) as List<GIAO_VIEN>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<GIAO_VIEN> getGiaoVienByTruongTrangThai(long id_truong, bool is_all = false, long id_all = 0, string text_all = "Chọn tất cả")
        {
            List<GIAO_VIEN> data = new List<GIAO_VIEN>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("GIAO_VIEN", "getGiaoVienByTruongTrangThai", id_truong, is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.GIAO_VIEN where p.ID_TRUONG == id_truong && p.MA_TRANG_THAI == 1 && p.IS_DELETE != true select p).ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<GIAO_VIEN>();
                        GIAO_VIEN item_all = new GIAO_VIEN();
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
                    data = QICache.Get(strKeyCache) as List<GIAO_VIEN>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public GIAO_VIEN checkGiaoVienByPhoneAndTruong(long id_truong, string phone)
        {
            var QICache = new DefaultCacheProvider();
            string strSession = QICache.BuildCachedKey("GIAO_VIEN", "checkGiaoVienByPhoneAndTruong", id_truong, phone);
            GIAO_VIEN detail = new GIAO_VIEN();
            if (!QICache.IsSet(strSession))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    detail = (from p in context.GIAO_VIEN
                              where p.ID_TRUONG == id_truong && p.SDT == phone && p.IS_DELETE != true
                              select p).FirstOrDefault();
                    QICache.Set(strSession, detail, 300000);
                }
            }
            else
            {
                try
                {
                    detail = QICache.Get(strSession) as GIAO_VIEN;
                }
                catch
                {
                    QICache.Invalidate(strSession);
                }
            }
            return detail;
        }
        public long? getMaxThuTuByTruong(long id_truong)
        {
            long? data = null;
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("GIAO_VIEN", "getMaxThuTuByTruong", id_truong);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var sql = @"select MAX(THU_TU) as THU_TU from GIAO_VIEN 
                                    where ID_TRUONG = :0";
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
        public long? getTongSoGiaoVienByTruong(long id_truong)
        {
            long? data = null;
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("GIAO_VIEN", "getTongSoGiaoVienByTruong", id_truong);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var sql = @"select count(id) from giao_vien
                        where not (IS_DELETE is not null and IS_DELETE = 1) 
                        and MA_TRANG_THAI = 1 and id_truong=:0";
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
        public List<GIAO_VIEN> getNhanVienByTruong(long id_truong)
        {
            List<GIAO_VIEN> data = new List<GIAO_VIEN>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("GIAO_VIEN", "getNhanVienByTruong", id_truong);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.GIAO_VIEN where p.ID_TRUONG == id_truong && p.ID_CHUC_VU == 4 && p.MA_TRANG_THAI == 1 && p.IS_DELETE != true select p).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<GIAO_VIEN>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<GiaoVienEntity> getGiaoVienBoMonByLop(long id_lop, short hoc_ky)
        {
            List<GiaoVienEntity> data = new List<GiaoVienEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("LOP_MON", "MON_HOC_TRUONG", "GIAO_VIEN", "getGiaoVienBoMonByLop", id_lop, hoc_ky);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    try
                    {
                        string strQuery = string.Format(@"select gv.sdt,mt.ten || ' (' || gv.ho_ten || ')' as ho_ten
                        from lop_mon lm
                        join MON_HOC_TRUONG mt on mt.id=lm.id_mon_truong
                        join giao_vien gv on gv.id=lm.id_giao_vien
                        where lm.ID_LOP=:0 and lm.hoc_ky=:1");
                        data = context.Database.SqlQuery<GiaoVienEntity>(strQuery, id_lop, hoc_ky).ToList();

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
                    data = QICache.Get(strKeyCache) as List<GiaoVienEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        #endregion
        #region  to giao viên
        public List<ToGiaoVienEntity> getGiaoVienGuiSMSThongBaoByListToGiaoVien(long id_truong, List<short> lst_ma_khoi)
        {
            DataAccessAPI dataAccessAPI = new DataAccessAPI();
            List<ToGiaoVienEntity> data = new List<ToGiaoVienEntity>();
            var QICache = new DefaultCacheProvider();
            string str_lst_ma_khoi = "";
            str_lst_ma_khoi = dataAccessAPI.ConvertListToString<short>(lst_ma_khoi, ",");
            string strKeyCache = QICache.BuildCachedKey("GIAO_VIEN", "TO_GIAO_VIEN_GV", "getGiaoVienGuiSMSThongBaoByListToGiaoVien", id_truong, str_lst_ma_khoi);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    try
                    {
                        string strQuery = string.Format(@"select gv.ID
                                                          ,gv.HO_TEN
                                                          ,gv.SDT
                                                           ,tgv.ID_GIAO_VIEN
                                                          from GIAO_VIEN gv
                                                          inner join TO_GIAO_VIEN_GV tgv on gv.ID = tgv.ID_GIAO_VIEN 
                                            where 1=1 ");
                        if (lst_ma_khoi != null && lst_ma_khoi.Count == 1)
                            strQuery += string.Format(@" and tgv.ID_TO ={0}", lst_ma_khoi[0]);
                        else if (lst_ma_khoi != null && lst_ma_khoi.Count > 1)
                        {
                            strQuery += string.Format(" and not ( tgv.ID_TO !={0}", lst_ma_khoi[0]);
                            for (int i = 1; i < lst_ma_khoi.Count; i++)
                            {
                                strQuery += string.Format(" and tgv.ID_TO !={0}", lst_ma_khoi[i]);
                            }
                            strQuery += " );";
                        }
                        data = context.Database.SqlQuery<ToGiaoVienEntity>(strQuery, DateTime.Now, id_truong).ToList();
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
                    data = QICache.Get(strKeyCache) as List<ToGiaoVienEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<TO_GIAO_VIEN_GV> checkExsitsGiaoVienInToGiaoVien(long id_truong, long id_giao_vien)
        {
            List<TO_GIAO_VIEN_GV> data = new List<TO_GIAO_VIEN_GV>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TO_GIAO_VIEN_GV", "checkExsitsGiaoVienInToGiaoVien", id_truong, id_giao_vien);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.TO_GIAO_VIEN_GV
                            where p.ID_TRUONG == id_truong && p.ID_GIAO_VIEN == id_giao_vien && p.IS_DELETE != true
                            select p).ToList();

                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<TO_GIAO_VIEN_GV>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<GIAO_VIEN> getGiaoVienByChucVu(long id_truong, short id_chuc_vu)
        {
            List<GIAO_VIEN> data = new List<GIAO_VIEN>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("GIAO_VIEN", "getGiaoVienByChucVu", id_truong, id_chuc_vu);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.GIAO_VIEN where p.IS_DELETE != true && p.ID_TRUONG == id_truong && p.ID_CHUC_VU == id_chuc_vu select p);
                    tmp = tmp.OrderBy(x => x.TEN);
                    data = tmp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<GIAO_VIEN>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<GIAO_VIEN> getGiaoVienByTruongSDT(long id_truong, string sdt)
        {
            List<GIAO_VIEN> data = new List<GIAO_VIEN>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("GIAO_VIEN", "getGiaoVienByTruongSDT", id_truong, sdt);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.GIAO_VIEN where p.ID_TRUONG == id_truong && p.SDT == sdt && p.IS_DELETE != true orderby p.NGAY_TAO descending select p).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<GIAO_VIEN>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }

        public List<GIAO_VIEN> getGiaoVienInTruong(long id_truong)
        {
            List<GIAO_VIEN> data = new List<GIAO_VIEN>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("GIAO_VIEN", "getGiaoVienInTruong", id_truong);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.GIAO_VIEN where p.ID_TRUONG == id_truong && p.IS_DELETE != true orderby p.NGAY_TAO descending select p).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<GIAO_VIEN>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<GIAO_VIEN> getGiaoVienByPhone(string sdt)
        {
            List<GIAO_VIEN> data = new List<GIAO_VIEN>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("GIAO_VIEN", "getGiaoVienByPhone", sdt);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.GIAO_VIEN where p.SDT == sdt && p.IS_DELETE != true orderby p.NGAY_TAO descending select p).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<GIAO_VIEN>;
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
        public ResultEntity update(long id, string ten, string sdt, DateTime? ngay_sinh, short? gioi_tinh, short? id_chuc_vu, short? ma_trang_thai, string dia_chi, string email, long id_truong, long? nguoi, short? thu_tu, string ho_dem, string ho_ten)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    var sql = @"Update GIAO_VIEN SET TEN=:0, SDT=:1, NGAY_SINH=:2, MA_GIOI_TINH=:3, DIA_CHI=:4, EMAIL=:5, ID_TRUONG=:6, NGAY_SUA=:7, NGUOI_SUA=:8, MA_TRANG_THAI=:9, THU_TU=:10, HO_DEM=:11, HO_TEN=:12, ID_CHUC_VU=:13 WHERE ID=:14";
                    context.Database.ExecuteSqlCommand(sql, ten, sdt, ngay_sinh, gioi_tinh, dia_chi, email, id_truong, DateTime.Now, nguoi, ma_trang_thai, thu_tu, ho_dem, ho_ten, id_chuc_vu, id);
                    context.SaveChanges();
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("GIAO_VIEN");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity update(GIAO_VIEN detail_in, long? nguoi)
        {
            GIAO_VIEN detail = new GIAO_VIEN();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.GIAO_VIEN where p.ID == detail_in.ID select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.TEN = detail_in.TEN;
                        detail.HO_DEM = detail_in.HO_DEM;
                        detail.HO_TEN = detail_in.HO_TEN;
                        detail.SDT = detail_in.SDT;
                        detail.NGAY_SINH = detail_in.NGAY_SINH;
                        detail.MA_GIOI_TINH = detail_in.MA_GIOI_TINH;
                        detail.DIA_CHI = detail_in.DIA_CHI;
                        detail.EMAIL = detail_in.EMAIL;
                        detail.ID_TRUONG = detail_in.ID_TRUONG;
                        detail.MA_TRANG_THAI = detail_in.MA_TRANG_THAI;
                        detail.THU_TU = detail_in.THU_TU;
                        detail.ID_CHUC_VU = detail_in.ID_CHUC_VU;
                        detail.ZALO_CODE = detail_in.ZALO_CODE;
                        detail.NGAY_GUI_OTP = detail_in.NGAY_GUI_OTP;
                        detail.OTP_COUNTER = detail_in.OTP_COUNTER;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("GIAO_VIEN");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(GIAO_VIEN detail_in, long? nguoi)
        {
            GIAO_VIEN detail = new GIAO_VIEN();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail_in.ID = context.Database.SqlQuery<long>("SELECT GIAO_VIEN_SEQ1.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                    detail_in.NGAY_TAO = DateTime.Now;
                    detail_in.NGUOI_TAO = nguoi;
                    detail_in = context.GIAO_VIEN.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("GIAO_VIEN");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }

        public ResultEntity delete(long id, long? nguoi, bool is_delete_all, bool is_delete = false)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    if (is_delete_all)
                    {
                        var sql = @"update LOP set ID_GVCN = NULL,NGUOI_SUA=:0,NGAY_SUA=:1 where ID_GVCN = :2";
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, id);
                        sql = @"update LOP_MON set ID_GIAO_VIEN = NULL,NGUOI_SUA=:0,NGAY_SUA=:1 where ID_GIAO_VIEN = :2";
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, id);
                        sql = @"delete TO_GIAO_VIEN_GV where ID_GIAO_VIEN = :0";
                        context.Database.ExecuteSqlCommand(sql, id);
                        sql = @"delete GIAO_VIEN where ID = :0";
                        context.Database.ExecuteSqlCommand(sql, id);
                    }
                    else if (!is_delete)
                    {
                        var sql = @"update GIAO_VIEN set IS_DELETE = 1,NGUOI_SUA=:0,NGAY_SUA=:1 where ID = :2";
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, id);
                    }
                    else
                    {
                        var sql = @"Delete GIAO_VIEN where ID = :0";
                        context.Database.ExecuteSqlCommand(sql, id);
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("GIAO_VIEN");
                if (is_delete_all)
                {
                    QICache.RemoveByFirstName("LOP");
                    QICache.RemoveByFirstName("LOP_MON");
                    QICache.RemoveByFirstName("TO_GIAO_VIEN_GV");
                }
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
