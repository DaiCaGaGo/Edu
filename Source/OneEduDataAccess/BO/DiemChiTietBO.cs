using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class DiemChiTietBO
    {
        #region get
        public List<DIEM_CHI_TIET> getDiemChiTiet()
        {
            List<DIEM_CHI_TIET> data = new List<DIEM_CHI_TIET>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DIEM_CHI_TIET", "getDiemChiTiet");
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DIEM_CHI_TIET where p.IS_DELETE != true select p).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<DIEM_CHI_TIET>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<DIEM_CHI_TIET> getDiemChiTietByLop(long id_lop)
        {
            List<DIEM_CHI_TIET> data = new List<DIEM_CHI_TIET>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DIEM_CHI_TIET", "getDiemChiTietByLop", id_lop);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DIEM_CHI_TIET where p.IS_DELETE != true && p.ID_LOP == id_lop select p).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<DIEM_CHI_TIET>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<DIEM_CHI_TIET> getDiemChiTietByTruongKhoiLopAndMonAndCapAndHocKy(short id_nam_hoc, long id_truong, short? ma_khoi, long? id_lop, short hoc_ky, long? id_mon_truong, string cap_hoc)
        {
            List<DIEM_CHI_TIET> data = new List<DIEM_CHI_TIET>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DIEM_CHI_TIET", "getDiemChiTietByTruongKhoiLopAndMonAndCapAndHocKy"
                , id_nam_hoc, id_truong, ma_khoi, id_lop, hoc_ky, id_mon_truong, cap_hoc);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.DIEM_CHI_TIET where p.IS_DELETE != true && p.ID_NAM_HOC == id_nam_hoc && p.ID_TRUONG == id_truong && p.HOC_KY == hoc_ky select p);
                    if (ma_khoi != null)
                    {
                        tmp = tmp.Where(x => x.MA_KHOI == ma_khoi);
                    }
                    if (id_lop != null)
                    {
                        tmp = tmp.Where(x => x.ID_LOP == id_lop);
                    }
                    if (id_mon_truong != null)
                    {
                        tmp = tmp.Where(x => x.ID_MON_HOC_TRUONG == id_mon_truong);
                    }
                    if (cap_hoc == SYS_Cap_Hoc.TH)
                        tmp = tmp.Where(x => x.MA_KHOI >= 1 && x.MA_KHOI <= 5);
                    else if (cap_hoc == SYS_Cap_Hoc.THCS)
                        tmp = tmp.Where(x => x.MA_KHOI >= 6 && x.MA_KHOI <= 9);
                    else if (cap_hoc == SYS_Cap_Hoc.THPT)
                        tmp = tmp.Where(x => x.MA_KHOI >= 10 && x.MA_KHOI <= 12);
                    else if (cap_hoc == SYS_Cap_Hoc.MN)
                        tmp = tmp.Where(x => x.MA_KHOI > 12);
                    else if (cap_hoc == SYS_Cap_Hoc.GDTX)
                        tmp = tmp.Where(x => x.MA_KHOI <= 12);
                    data = tmp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<DIEM_CHI_TIET>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<DIEM_CHI_TIET> getDiemChiTietByTruongKhoiLopAndHocSinhAndCapAndHocKy(short id_nam_hoc, long id_truong, short ma_khoi, long id_lop, short hoc_ky, long id_hoc_sinh, string cap_hoc)
        {
            List<DIEM_CHI_TIET> data = new List<DIEM_CHI_TIET>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DIEM_CHI_TIET", "getDiemChiTietByTruongKhoiLopAndHocSinhAndCapAndHocKy", id_nam_hoc, id_truong, ma_khoi, id_lop, hoc_ky, id_hoc_sinh, cap_hoc);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DIEM_CHI_TIET where p.IS_DELETE != true && p.ID_NAM_HOC == id_nam_hoc && p.ID_TRUONG == id_truong && p.MA_KHOI == ma_khoi && p.ID_LOP == id_lop && p.HOC_KY == hoc_ky && p.ID_HOC_SINH == id_hoc_sinh select p).ToList();

                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<DIEM_CHI_TIET>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<DiemChiTietEntity> getDiemTBMonByHocSinh(long id_truong, short ma_khoi, long id_lop, short id_nam_hoc, short hoc_ky, long id_hoc_sinh, string lstMaMonHoc)
        {
            List<DiemChiTietEntity> data = new List<DiemChiTietEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DIEM_CHI_TIET", "getDiemTBMonByHocSinh", id_truong, ma_khoi, id_lop, id_nam_hoc, hoc_ky, id_hoc_sinh, lstMaMonHoc);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var strQuery = string.Format(@"select dct.* from diem_chi_tiet dct
                            where dct.ID_TRUONG=:0 and dct.MA_KHOI=:1 and dct.id_lop = :2 and dct.ID_NAM_HOC=:3 and dct.hoc_ky=:4 
                            and dct.id_hoc_sinh=:5 and dct.ID_MON_HOC_TRUONG in {0}", lstMaMonHoc);
                    data = context.Database.SqlQuery<DiemChiTietEntity>(strQuery, id_truong, ma_khoi, id_lop, id_nam_hoc, hoc_ky, id_hoc_sinh).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<DiemChiTietEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<DiemChiTietEntity> getDiemChiTietByTruongKhoiLopAndMon(short id_nam_hoc, long id_truong, short? ma_khoi, long? id_lop, short hoc_ky, long id_mon_truong)
        {
            DataAccessAPI dataAccessAPI = new DataAccessAPI();
            List<DiemChiTietEntity> data = new List<DiemChiTietEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DIEM_CHI_TIET", "HOC_SINH", "GIOI_TINH", "getDiemChiTietByTruongKhoiLopAndMon", id_nam_hoc, id_truong, ma_khoi, id_lop, hoc_ky, id_mon_truong);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = @"select d.*,h.MA as MA_HS,h.HO_TEN as TEN_HS,h.MA_GIOI_TINH ,g.TEN as TEN_GIOI_TINH,h.NGAY_SINH 
                                        from DIEM_CHI_TIET d
                                        join HOC_SINH h on h.ID_TRUONG=d.ID_TRUONG and d.ID_HOC_SINH =h.ID and d.ID_LOP=h.ID_LOP
                                        join GIOI_TINH g on h.MA_GIOI_TINH=g.MA
                                        where not ( d.IS_DELETE is not null and d.IS_DELETE =1 ) and not ( h.IS_DELETE is not null and h.IS_DELETE =1 ) and d.ID_NAM_HOC=:0 and d.ID_TRUONG=:1 and d.HOC_KY=:2 and d.ID_MON_HOC_TRUONG=:3
                                        ";
                    List<object> parameterList = new List<object>();
                    parameterList.Add(id_nam_hoc);
                    parameterList.Add(id_truong);
                    parameterList.Add(hoc_ky);
                    parameterList.Add(id_mon_truong);
                    if (ma_khoi != null)
                    {
                        strQuery += " and h.ID_KHOI=:" + parameterList.Count;
                        parameterList.Add(ma_khoi);
                    }
                    if (id_lop != null)
                    {
                        strQuery += " and d.ID_LOP=:" + parameterList.Count;
                        parameterList.Add(id_lop);
                    }
                    strQuery += " order by nvl(h.THU_TU, 0),NLSSORT(h.ten,'NLS_SORT=vietnamese'),NLSSORT(h.HO_DEM,'NLS_SORT=vietnamese')";
                    data = context.Database.SqlQuery<DiemChiTietEntity>(strQuery, parameterList.ToArray()).ToList();
                    foreach (var item in data)
                    {
                        item.DIEM1_VIEW = dataAccessAPI.ConvertDiemToString(item.DIEM1, id_mon_truong);
                        item.DIEM2_VIEW = dataAccessAPI.ConvertDiemToString(item.DIEM2, id_mon_truong);
                        item.DIEM3_VIEW = dataAccessAPI.ConvertDiemToString(item.DIEM3, id_mon_truong);
                        item.DIEM4_VIEW = dataAccessAPI.ConvertDiemToString(item.DIEM4, id_mon_truong);
                        item.DIEM5_VIEW = dataAccessAPI.ConvertDiemToString(item.DIEM5, id_mon_truong);
                        item.DIEM6_VIEW = dataAccessAPI.ConvertDiemToString(item.DIEM6, id_mon_truong);
                        item.DIEM7_VIEW = dataAccessAPI.ConvertDiemToString(item.DIEM7, id_mon_truong);
                        item.DIEM8_VIEW = dataAccessAPI.ConvertDiemToString(item.DIEM8, id_mon_truong);
                        item.DIEM9_VIEW = dataAccessAPI.ConvertDiemToString(item.DIEM9, id_mon_truong);
                        item.DIEM10_VIEW = dataAccessAPI.ConvertDiemToString(item.DIEM10, id_mon_truong);
                        item.DIEM11_VIEW = dataAccessAPI.ConvertDiemToString(item.DIEM11, id_mon_truong);
                        item.DIEM12_VIEW = dataAccessAPI.ConvertDiemToString(item.DIEM12, id_mon_truong);
                        item.DIEM13_VIEW = dataAccessAPI.ConvertDiemToString(item.DIEM13, id_mon_truong);
                        item.DIEM14_VIEW = dataAccessAPI.ConvertDiemToString(item.DIEM14, id_mon_truong);
                        item.DIEM15_VIEW = dataAccessAPI.ConvertDiemToString(item.DIEM15, id_mon_truong);
                        item.DIEM16_VIEW = dataAccessAPI.ConvertDiemToString(item.DIEM16, id_mon_truong);
                        item.DIEM17_VIEW = dataAccessAPI.ConvertDiemToString(item.DIEM17, id_mon_truong);
                        item.DIEM18_VIEW = dataAccessAPI.ConvertDiemToString(item.DIEM18, id_mon_truong);
                        item.DIEM19_VIEW = dataAccessAPI.ConvertDiemToString(item.DIEM19, id_mon_truong);
                        item.DIEM20_VIEW = dataAccessAPI.ConvertDiemToString(item.DIEM20, id_mon_truong);
                        item.DIEM21_VIEW = dataAccessAPI.ConvertDiemToString(item.DIEM21, id_mon_truong);
                        item.DIEM22_VIEW = dataAccessAPI.ConvertDiemToString(item.DIEM22, id_mon_truong);
                        item.DIEM23_VIEW = dataAccessAPI.ConvertDiemToString(item.DIEM23, id_mon_truong);
                        item.DIEM24_VIEW = dataAccessAPI.ConvertDiemToString(item.DIEM24, id_mon_truong);
                        item.DIEM25_VIEW = dataAccessAPI.ConvertDiemToString(item.DIEM25, id_mon_truong);
                        item.DIEM_HOC_KY_VIEW = dataAccessAPI.ConvertDiemToString(item.DIEM_HOC_KY, id_mon_truong);
                        item.DIEM_TRUNG_BINH_KY1_VIEW = dataAccessAPI.ConvertDiemToString(item.DIEM_TRUNG_BINH_KY1, id_mon_truong);
                        item.DIEM_TRUNG_BINH_KY2_VIEW = dataAccessAPI.ConvertDiemToString(item.DIEM_TRUNG_BINH_KY2, id_mon_truong);
                        item.DIEM_TRUNG_BINH_CN_VIEW = dataAccessAPI.ConvertDiemToString(item.DIEM_TRUNG_BINH_CN, id_mon_truong);
                        item.DIEM_TRUNG_BINH_CN_TTL_VIEW = dataAccessAPI.ConvertDiemToString(item.DIEM_TRUNG_BINH_CN_TTL, id_mon_truong);
                    }
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<DiemChiTietEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<DiemChiTietEntity> getDiemChiTietByTruongKhoiLopHocSinhNgay(short id_nam_hoc, long id_truong, short? ma_khoi, long? id_lop, short hoc_ky, DateTime ngay, long? id_hoc_sinh)
        {
            List<DiemChiTietEntity> data = new List<DiemChiTietEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DIEM_CHI_TIET", "HOC_SINH", "MON_HOC_TRUONG", "getDiemChiTietByTruongKhoiLopHocSinhNgay", id_nam_hoc, id_truong, ma_khoi, id_lop, hoc_ky, ngay, id_hoc_sinh);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {

                    string strQuery = string.Format(@"select d.*, m.Ten as TEN_MON_TRUONG
,case when m.kieu_mon = 0 then 
 (case when diem1 is not null and TRUNC(ngay_diem1) = TRUNC({0}) then rtrim(to_char(diem1, 'FM90D99'), to_char(0, 'D')) || ','  else '' end
||case when diem2 is not null and TRUNC(ngay_diem2) = TRUNC({0}) then rtrim(to_char(diem2, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem3 is not null and TRUNC(ngay_diem3) = TRUNC({0}) then rtrim(to_char(diem3, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem4 is not null and TRUNC(ngay_diem4) = TRUNC({0}) then rtrim(to_char(diem4, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem5 is not null and TRUNC(ngay_diem5) = TRUNC({0}) then rtrim(to_char(diem5, 'FM90D99'), to_char(0, 'D')) ||','  else '' end) 
else
(case when diem1 is not null and diem1 = 1 and TRUNC(ngay_diem1) = TRUNC({0}) then 'Đ' || ','  
      WHEN diem1 is not null and diem1 = 0 and TRUNC(ngay_diem1) = TRUNC({0}) then 'CĐ' || ',' else '' end
||case when diem2 is not null and diem2 = 1 and TRUNC(ngay_diem2) = TRUNC({0}) then 'Đ' ||','
      WHEN diem2 is not null and diem2 = 0 and TRUNC(ngay_diem2) = TRUNC({0}) then 'CĐ' || ',' else '' end
||case when diem3 is not null and diem3 = 1 and TRUNC(ngay_diem3) = TRUNC({0}) then 'Đ' ||','
      WHEN diem3 is not null and diem3 = 0 and TRUNC(ngay_diem3) = TRUNC({0}) then 'CĐ' || ',' else '' end
||case when diem4 is not null and diem4 = 1 and TRUNC(ngay_diem4) = TRUNC({0}) then 'Đ' ||',' 
      WHEN diem4 is not null and diem4 = 0 and TRUNC(ngay_diem4) = TRUNC({0}) then 'CĐ' || ',' else '' end
||case when diem5 is not null and diem5 = 1 and TRUNC(ngay_diem5) = TRUNC({0}) then 'Đ' ||','  
      WHEN diem5 is not null and diem5 = 0 and TRUNC(ngay_diem5) = TRUNC({0}) then 'CĐ' || ',' else '' end
) end as diemMieng
,case when m.kieu_mon = 0 then
(case when diem6 is not null and TRUNC(ngay_diem6) = TRUNC({0}) then rtrim(to_char(diem6, 'FM90D99'), to_char(0, 'D')) || ','  else '' end
||case when diem7 is not null and TRUNC(ngay_diem7) = TRUNC({0}) then rtrim(to_char(diem7, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem8 is not null and TRUNC(ngay_diem8) = TRUNC({0}) then rtrim(to_char(diem8, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem9 is not null and TRUNC(ngay_diem9) = TRUNC({0}) then rtrim(to_char(diem9, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem10 is not null and TRUNC(ngay_diem10) = TRUNC({0}) then rtrim(to_char(diem10, 'FM90D99'), to_char(0, 'D')) ||','  else '' end)
else 
 (case when diem6 is not null and diem6 = 1 and TRUNC(ngay_diem6) = TRUNC({0}) then 'Đ' ||',' WHEN diem6 is not null and diem6 = 0 and TRUNC(ngay_diem6) = TRUNC({0}) then 'CĐ' || ',' else '' end
||case when diem7 is not null and diem7 = 1 and TRUNC(ngay_diem7) = TRUNC({0}) then 'Đ' ||',' WHEN diem7 is not null and diem7 = 0 and TRUNC(ngay_diem7) = TRUNC({0}) then 'CĐ' || ',' else '' end
||case when diem8 is not null and diem8 = 1 and TRUNC(ngay_diem8) = TRUNC({0}) then 'Đ' ||',' WHEN diem8 is not null and diem8 = 0 and TRUNC(ngay_diem8) = TRUNC({0}) then 'CĐ' || ',' else '' end
||case when diem9 is not null and diem9 = 1 and TRUNC(ngay_diem9) = TRUNC({0}) then 'Đ' ||',' WHEN diem9 is not null and diem9 = 0 and TRUNC(ngay_diem9) = TRUNC({0}) then 'CĐ' || ',' else '' end
||case when diem10 is not null and diem10 = 1  and TRUNC(ngay_diem10) = TRUNC({0})then 'Đ' ||',' WHEN diem10 is not null and diem10 = 0 and TRUNC(ngay_diem10) = TRUNC({0}) then 'CĐ' || ',' else '' end
) end as diem15P
,case when m.kieu_mon = 0 then 
 (case when diem11 is not null and TRUNC(ngay_diem11) = TRUNC({0}) then rtrim(to_char(diem11, 'FM90D99'), to_char(0, 'D')) || ','  else '' end
||case when diem12 is not null and TRUNC(ngay_diem12) = TRUNC({0}) then rtrim(to_char(diem12, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem13 is not null and TRUNC(ngay_diem13) = TRUNC({0}) then rtrim(to_char(diem13, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem14 is not null and TRUNC(ngay_diem14) = TRUNC({0}) then rtrim(to_char(diem14, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem15 is not null and TRUNC(ngay_diem15) = TRUNC({0}) then rtrim(to_char(diem15, 'FM90D99'), to_char(0, 'D')) ||','  else '' end)
else
(case when diem11 is not null and diem11 = 1 and TRUNC(ngay_diem11) = TRUNC({0}) then 'Đ' ||',' WHEN diem11 is not null and diem11 = 0 and TRUNC(ngay_diem11) = TRUNC({0}) then 'CĐ' || ',' else '' end
||case when diem12 is not null and diem12 = 1 and TRUNC(ngay_diem12) = TRUNC({0}) then 'Đ' ||',' WHEN diem12 is not null and diem12 = 0 and TRUNC(ngay_diem12) = TRUNC({0}) then 'CĐ' || ',' else '' end
||case when diem13 is not null and diem13 = 1 and TRUNC(ngay_diem13) = TRUNC({0}) then 'Đ' ||',' WHEN diem13 is not null and diem13 = 0 and TRUNC(ngay_diem13) = TRUNC({0}) then 'CĐ' || ',' else '' end
||case when diem14 is not null and diem14 = 1 and TRUNC(ngay_diem14) = TRUNC({0}) then 'Đ' ||',' WHEN diem14 is not null and diem14 = 0 and TRUNC(ngay_diem14) = TRUNC({0}) then 'CĐ' || ',' else '' end
||case when diem15 is not null and diem15 = 1 and TRUNC(ngay_diem15) = TRUNC({0}) then 'Đ' ||',' WHEN diem15 is not null and diem15 = 0 and TRUNC(ngay_diem15) = TRUNC({0}) then 'CĐ' || ',' else '' end
) end as diem1T_HS1
,case when m.kieu_mon = 0 then 
 (case when diem16 is not null and TRUNC(ngay_diem16) = TRUNC({0}) then rtrim(to_char(diem16, 'FM90D99'), to_char(0, 'D')) || ','  else '' end
||case when diem17 is not null and TRUNC(ngay_diem17) = TRUNC({0}) then rtrim(to_char(diem17, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem18 is not null and TRUNC(ngay_diem18) = TRUNC({0}) then rtrim(to_char(diem18, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem19 is not null and TRUNC(ngay_diem19) = TRUNC({0}) then rtrim(to_char(diem19, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem20 is not null and TRUNC(ngay_diem20) = TRUNC({0}) then rtrim(to_char(diem20, 'FM90D99'), to_char(0, 'D')) ||','  else '' end)
else
 (case when diem16 is not null and diem16 = 1 and TRUNC(ngay_diem16) = TRUNC({0}) then 'Đ' ||',' WHEN diem16 is not null and diem16 = 0 and TRUNC(ngay_diem16) = TRUNC({0}) then 'CĐ' || ',' else '' end
||case when diem17 is not null and diem17 = 1 and TRUNC(ngay_diem17) = TRUNC({0}) then 'Đ' ||',' WHEN diem17 is not null and diem17 = 0 and TRUNC(ngay_diem17) = TRUNC({0}) then 'CĐ' || ',' else '' end
||case when diem18 is not null and diem18 = 1 and TRUNC(ngay_diem18) = TRUNC({0}) then 'Đ' ||',' WHEN diem18 is not null and diem18 = 0 and TRUNC(ngay_diem18) = TRUNC({0}) then 'CĐ' || ',' else '' end
||case when diem19 is not null and diem19 = 1 and TRUNC(ngay_diem19) = TRUNC({0}) then 'Đ' ||',' WHEN diem19 is not null and diem19 = 0 and TRUNC(ngay_diem19) = TRUNC({0}) then 'CĐ' || ',' else '' end
||case when diem20 is not null and diem20 = 1 and TRUNC(ngay_diem20) = TRUNC({0}) then 'Đ' ||',' WHEN diem20 is not null and diem20 = 0 and TRUNC(ngay_diem20) = TRUNC({0}) then 'CĐ' || ',' else '' end
) end as diem1T_HS2
,case when m.kieu_mon = 0 then 
    case when DIEM_HOC_KY is not null and TRUNC(NGAY_DIEM_HOC_KY) = TRUNC({0}) then rtrim(to_char(DIEM_HOC_KY, 'FM90D99'), to_char(0, 'D')) || ','  else '' end
else
    case when DIEM_HOC_KY is not null and DIEM_HOC_KY = 1 and TRUNC(NGAY_DIEM_HOC_KY) = TRUNC( {0}) then 'Đ' ||',' WHEN DIEM_HOC_KY is not null and DIEM_HOC_KY = 0 and TRUNC(NGAY_DIEM_HOC_KY) = TRUNC({0}) then 'CĐ' || ',' else '' end
end as diemHocKy
                    from diem_chi_tiet d 
                    left join hoc_sinh h on d.id_hoc_sinh = h.id
                    left join mon_hoc_truong m on d.id_mon_hoc_truong = m.id
                    where not ( d.IS_DELETE is not null and d.IS_DELETE =1 ) 
                    and not ( h.IS_DELETE is not null and h.IS_DELETE =1 ) 
                    and d.ID_NAM_HOC=:0 and d.ID_TRUONG=:1 and d.hoc_ky=:2
                        and (TRUNC(ngay_diem1) = TRUNC({0})
                        or TRUNC(ngay_diem2) = TRUNC({0})
                        or TRUNC(ngay_diem3) = TRUNC({0})
                        or TRUNC(ngay_diem4) = TRUNC({0})
                        or TRUNC(ngay_diem5) = TRUNC({0})
                        or TRUNC(ngay_diem6) = TRUNC({0})
                        or TRUNC(ngay_diem7) = TRUNC({0})
                        or TRUNC(ngay_diem8) = TRUNC({0})
                        or TRUNC(ngay_diem9) = TRUNC({0})
                        or TRUNC(ngay_diem10) = TRUNC({0})
                        or TRUNC(ngay_diem11) = TRUNC({0})
                        or TRUNC(ngay_diem12) = TRUNC({0})
                        or TRUNC(ngay_diem13) = TRUNC({0})
                        or TRUNC(ngay_diem14) = TRUNC({0})
                        or TRUNC(ngay_diem15) = TRUNC({0})
                        or TRUNC(ngay_diem16) = TRUNC({0})
                        or TRUNC(ngay_diem17) = TRUNC({0})
                        or TRUNC(ngay_diem18) = TRUNC({0})
                        or TRUNC(ngay_diem19) = TRUNC({0})
                        or TRUNC(ngay_diem20) = TRUNC({0})
                        or TRUNC(ngay_diem21) = TRUNC({0})
                        or TRUNC(ngay_diem22) = TRUNC({0})
                        or TRUNC(ngay_diem23) = TRUNC({0})
                        or TRUNC(ngay_diem24) = TRUNC({0})
                        or TRUNC(ngay_diem25) = TRUNC({0})
                        or TRUNC(NGAY_DIEM_HOC_KY) = TRUNC({0})
                        )", "to_date('" + ngay.ToString("dd/MM/yyyy") + "', 'dd/mm/yyyy')");
                    List<object> lstObjectParameter = new List<object>();
                    lstObjectParameter.Add(id_nam_hoc);
                    lstObjectParameter.Add(id_truong);
                    lstObjectParameter.Add(hoc_ky);
                    if (ma_khoi != null)
                    {
                        strQuery += " and h.id_khoi=:" + lstObjectParameter.Count;
                        lstObjectParameter.Add(ma_khoi.Value);
                    }
                    if (id_lop != null)
                    {
                        strQuery += " and d.id_lop=:" + lstObjectParameter.Count;
                        lstObjectParameter.Add(id_lop.Value);
                    }
                    if (id_hoc_sinh != null)
                    {
                        strQuery += " and d.ID_HOC_SINH=:" + lstObjectParameter.Count;
                        lstObjectParameter.Add(id_hoc_sinh.Value);
                    }
                    strQuery += " order by h.THU_TU,h.TEN,h.HO_DEM";
                    data = context.Database.SqlQuery<DiemChiTietEntity>(strQuery, lstObjectParameter.ToArray()).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<DiemChiTietEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        
        public DIEM_CHI_TIET getDiemChiTietByID(long id)
        {
            DIEM_CHI_TIET data = new DIEM_CHI_TIET();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DIEM_CHI_TIET", "getDiemChiTietByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DIEM_CHI_TIET where p.ID == id select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as DIEM_CHI_TIET;
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
        public ResultEntity update(DIEM_CHI_TIET detail_in, NGUOI_DUNGEntity nguoi)
        {
            DIEM_CHI_TIET detail = new DIEM_CHI_TIET();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.DIEM_CHI_TIET
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        #region DIEM1
                        if (detail.DIEM1 != detail_in.DIEM1)
                        {
                            detail.NGAY_DIEM1 = DateTime.Now;
                            detail.LOG_DIEM1 += (string.IsNullOrEmpty(detail.LOG_DIEM1) ? "" : ",") + "{" + nguoi.TEN_DANG_NHAP + ":" + detail_in.DIEM1 + "}";
                        }
                        detail.DIEM1 = detail_in.DIEM1;
                        #endregion

                        #region DIEM2
                        if (detail.DIEM2 != detail_in.DIEM2)
                        {
                            detail.NGAY_DIEM2 = DateTime.Now;
                            detail.LOG_DIEM2 += (string.IsNullOrEmpty(detail.LOG_DIEM2) ? "" : ",") + "{" + nguoi.TEN_DANG_NHAP + ":" + detail_in.DIEM2 + "}";
                        }
                        detail.DIEM2 = detail_in.DIEM2;
                        #endregion

                        #region DIEM3
                        if (detail.DIEM3 != detail_in.DIEM3)
                        {
                            detail.NGAY_DIEM3 = DateTime.Now;
                            detail.LOG_DIEM3 += (string.IsNullOrEmpty(detail.LOG_DIEM3) ? "" : ",") + "{" + nguoi.TEN_DANG_NHAP + ":" + detail_in.DIEM3 + "}";
                        }
                        detail.DIEM3 = detail_in.DIEM3;
                        #endregion

                        #region DIEM4
                        if (detail.DIEM4 != detail_in.DIEM4)
                        {
                            detail.NGAY_DIEM4 = DateTime.Now;
                            detail.LOG_DIEM4 += (string.IsNullOrEmpty(detail.LOG_DIEM4) ? "" : ",") + "{" + nguoi.TEN_DANG_NHAP + ":" + detail_in.DIEM4 + "}";
                        }
                        detail.DIEM4 = detail_in.DIEM4;
                        #endregion

                        #region DIEM5
                        if (detail.DIEM5 != detail_in.DIEM5)
                        {
                            detail.NGAY_DIEM5 = DateTime.Now;
                            detail.LOG_DIEM5 += (string.IsNullOrEmpty(detail.LOG_DIEM5) ? "" : ",") + "{" + nguoi.TEN_DANG_NHAP + ":" + detail_in.DIEM5 + "}";
                        }
                        detail.DIEM5 = detail_in.DIEM5;
                        #endregion

                        #region DIEM6
                        if (detail.DIEM6 != detail_in.DIEM6)
                        {
                            detail.NGAY_DIEM6 = DateTime.Now;
                            detail.LOG_DIEM6 += (string.IsNullOrEmpty(detail.LOG_DIEM6) ? "" : ",") + "{" + nguoi.TEN_DANG_NHAP + ":" + detail_in.DIEM6 + "}";
                        }
                        detail.DIEM6 = detail_in.DIEM6;
                        #endregion

                        #region DIEM7
                        if (detail.DIEM7 != detail_in.DIEM7)
                        {
                            detail.NGAY_DIEM7 = DateTime.Now;
                            detail.LOG_DIEM7 += (string.IsNullOrEmpty(detail.LOG_DIEM7) ? "" : ",") + "{" + nguoi.TEN_DANG_NHAP + ":" + detail_in.DIEM7 + "}";
                        }
                        detail.DIEM7 = detail_in.DIEM7;
                        #endregion

                        #region DIEM8
                        if (detail.DIEM8 != detail_in.DIEM8)
                        {
                            detail.NGAY_DIEM8 = DateTime.Now;
                            detail.LOG_DIEM8 += (string.IsNullOrEmpty(detail.LOG_DIEM8) ? "" : ",") + "{" + nguoi.TEN_DANG_NHAP + ":" + detail_in.DIEM8 + "}";
                        }
                        detail.DIEM8 = detail_in.DIEM8;
                        #endregion

                        #region DIEM9
                        if (detail.DIEM9 != detail_in.DIEM9)
                        {
                            detail.NGAY_DIEM9 = DateTime.Now;
                            detail.LOG_DIEM9 += (string.IsNullOrEmpty(detail.LOG_DIEM9) ? "" : ",") + "{" + nguoi.TEN_DANG_NHAP + ":" + detail_in.DIEM9 + "}";
                        }
                        detail.DIEM9 = detail_in.DIEM9;
                        #endregion

                        #region DIEM10
                        if (detail.DIEM10 != detail_in.DIEM10)
                        {
                            detail.NGAY_DIEM10 = DateTime.Now;
                            detail.LOG_DIEM10 += (string.IsNullOrEmpty(detail.LOG_DIEM10) ? "" : ",") + "{" + nguoi.TEN_DANG_NHAP + ":" + detail_in.DIEM10 + "}";
                        }
                        detail.DIEM10 = detail_in.DIEM10;
                        #endregion

                        #region DIEM11
                        if (detail.DIEM11 != detail_in.DIEM11)
                        {
                            detail.NGAY_DIEM11 = DateTime.Now;
                            detail.LOG_DIEM11 += (string.IsNullOrEmpty(detail.LOG_DIEM11) ? "" : ",") + "{" + nguoi.TEN_DANG_NHAP + ":" + detail_in.DIEM11 + "}";
                        }
                        detail.DIEM11 = detail_in.DIEM11;
                        #endregion

                        #region DIEM12
                        if (detail.DIEM12 != detail_in.DIEM12)
                        {
                            detail.NGAY_DIEM12 = DateTime.Now;
                            detail.LOG_DIEM12 += (string.IsNullOrEmpty(detail.LOG_DIEM12) ? "" : ",") + "{" + nguoi.TEN_DANG_NHAP + ":" + detail_in.DIEM12 + "}";
                        }
                        detail.DIEM12 = detail_in.DIEM12;
                        #endregion

                        #region DIEM13
                        if (detail.DIEM13 != detail_in.DIEM13)
                        {
                            detail.NGAY_DIEM13 = DateTime.Now;
                            detail.LOG_DIEM13 += (string.IsNullOrEmpty(detail.LOG_DIEM13) ? "" : ",") + "{" + nguoi.TEN_DANG_NHAP + ":" + detail_in.DIEM13 + "}";
                        }
                        detail.DIEM13 = detail_in.DIEM13;
                        #endregion

                        #region DIEM14
                        if (detail.DIEM14 != detail_in.DIEM14)
                        {
                            detail.NGAY_DIEM14 = DateTime.Now;
                            detail.LOG_DIEM14 += (string.IsNullOrEmpty(detail.LOG_DIEM14) ? "" : ",") + "{" + nguoi.TEN_DANG_NHAP + ":" + detail_in.DIEM14 + "}";
                        }
                        detail.DIEM14 = detail_in.DIEM14;
                        #endregion

                        #region DIEM15
                        if (detail.DIEM15 != detail_in.DIEM15)
                        {
                            detail.NGAY_DIEM15 = DateTime.Now;
                            detail.LOG_DIEM15 += (string.IsNullOrEmpty(detail.LOG_DIEM15) ? "" : ",") + "{" + nguoi.TEN_DANG_NHAP + ":" + detail_in.DIEM15 + "}";
                        }
                        detail.DIEM15 = detail_in.DIEM15;
                        #endregion

                        #region DIEM16
                        if (detail.DIEM16 != detail_in.DIEM16)
                        {
                            detail.NGAY_DIEM16 = DateTime.Now;
                            detail.LOG_DIEM16 += (string.IsNullOrEmpty(detail.LOG_DIEM16) ? "" : ",") + "{" + nguoi.TEN_DANG_NHAP + ":" + detail_in.DIEM16 + "}";
                        }
                        detail.DIEM16 = detail_in.DIEM16;
                        #endregion

                        #region DIEM17
                        if (detail.DIEM17 != detail_in.DIEM17)
                        {
                            detail.NGAY_DIEM17 = DateTime.Now;
                            detail.LOG_DIEM17 += (string.IsNullOrEmpty(detail.LOG_DIEM17) ? "" : ",") + "{" + nguoi.TEN_DANG_NHAP + ":" + detail_in.DIEM17 + "}";
                        }
                        detail.DIEM17 = detail_in.DIEM17;
                        #endregion

                        #region DIEM18
                        if (detail.DIEM18 != detail_in.DIEM18)
                        {
                            detail.NGAY_DIEM18 = DateTime.Now;
                            detail.LOG_DIEM18 += (string.IsNullOrEmpty(detail.LOG_DIEM18) ? "" : ",") + "{" + nguoi.TEN_DANG_NHAP + ":" + detail_in.DIEM18 + "}";
                        }
                        detail.DIEM18 = detail_in.DIEM18;
                        #endregion

                        #region DIEM19
                        if (detail.DIEM19 != detail_in.DIEM19)
                        {
                            detail.NGAY_DIEM19 = DateTime.Now;
                            detail.LOG_DIEM19 += (string.IsNullOrEmpty(detail.LOG_DIEM19) ? "" : ",") + "{" + nguoi.TEN_DANG_NHAP + ":" + detail_in.DIEM19 + "}";
                        }
                        detail.DIEM19 = detail_in.DIEM19;
                        #endregion

                        #region DIEM20
                        if (detail.DIEM20 != detail_in.DIEM20)
                        {
                            detail.NGAY_DIEM20 = DateTime.Now;
                            detail.LOG_DIEM20 += (string.IsNullOrEmpty(detail.LOG_DIEM20) ? "" : ",") + "{" + nguoi.TEN_DANG_NHAP + ":" + detail_in.DIEM20 + "}";
                        }
                        detail.DIEM20 = detail_in.DIEM20;
                        #endregion

                        #region DIEM21
                        if (detail.DIEM21 != detail_in.DIEM21)
                        {
                            detail.NGAY_DIEM21 = DateTime.Now;
                            detail.LOG_DIEM21 += (string.IsNullOrEmpty(detail.LOG_DIEM21) ? "" : ",") + "{" + nguoi.TEN_DANG_NHAP + ":" + detail_in.DIEM21 + "}";
                        }
                        detail.DIEM21 = detail_in.DIEM21;
                        #endregion

                        #region DIEM22
                        if (detail.DIEM22 != detail_in.DIEM22)
                        {
                            detail.NGAY_DIEM22 = DateTime.Now;
                            detail.LOG_DIEM22 += (string.IsNullOrEmpty(detail.LOG_DIEM22) ? "" : ",") + "{" + nguoi.TEN_DANG_NHAP + ":" + detail_in.DIEM22 + "}";
                        }
                        detail.DIEM22 = detail_in.DIEM22;
                        #endregion

                        #region DIEM23
                        if (detail.DIEM23 != detail_in.DIEM23)
                        {
                            detail.NGAY_DIEM23 = DateTime.Now;
                            detail.LOG_DIEM23 += (string.IsNullOrEmpty(detail.LOG_DIEM23) ? "" : ",") + "{" + nguoi.TEN_DANG_NHAP + ":" + detail_in.DIEM23 + "}";
                        }
                        detail.DIEM23 = detail_in.DIEM23;
                        #endregion

                        #region DIEM24
                        if (detail.DIEM24 != detail_in.DIEM24)
                        {
                            detail.NGAY_DIEM24 = DateTime.Now;
                            detail.LOG_DIEM24 += (string.IsNullOrEmpty(detail.LOG_DIEM24) ? "" : ",") + "{" + nguoi.TEN_DANG_NHAP + ":" + detail_in.DIEM24 + "}";
                        }
                        detail.DIEM24 = detail_in.DIEM24;
                        #endregion

                        #region DIEM25
                        if (detail.DIEM25 != detail_in.DIEM25)
                        {
                            detail.NGAY_DIEM25 = DateTime.Now;
                            detail.LOG_DIEM25 += (string.IsNullOrEmpty(detail.LOG_DIEM25) ? "" : ",") + "{" + nguoi.TEN_DANG_NHAP + ":" + detail_in.DIEM25 + "}";
                        }
                        detail.DIEM25 = detail_in.DIEM25;
                        #endregion

                        #region DIEM_HOC_KY
                        if (detail.DIEM_HOC_KY != detail_in.DIEM_HOC_KY)
                        {
                            detail.NGAY_DIEM_HOC_KY = DateTime.Now;
                            detail.LOG_DIEM_HOC_KY += (string.IsNullOrEmpty(detail.LOG_DIEM_HOC_KY) ? "" : ",") + "{" + nguoi.TEN_DANG_NHAP + ":" + detail_in.DIEM_HOC_KY + "}";
                        }
                        detail.DIEM_HOC_KY = detail_in.DIEM_HOC_KY;
                        #endregion

                        #region Update điểm học kỳ 1 ở học kỳ 2
                        if (detail.HOC_KY == 1 && detail.DIEM_TRUNG_BINH_KY1 != detail_in.DIEM_TRUNG_BINH_KY1)
                        {
                            DIEM_CHI_TIET detailK2 = new DIEM_CHI_TIET();
                            detailK2 = (from p in context.DIEM_CHI_TIET
                                        where p.ID_HOC_SINH == detail_in.ID_HOC_SINH && p.ID_MON_HOC_TRUONG == detail.ID_MON_HOC_TRUONG && p.ID_LOP == detail.ID_LOP && p.HOC_KY == 2
                                        select p).FirstOrDefault();
                            if (detailK2 != null)
                            {
                                detailK2.DIEM_TRUNG_BINH_KY1 = detail_in.DIEM_TRUNG_BINH_KY1;
                            }
                        }
                        #endregion

                        if (detail.HOC_KY == 1)
                            detail.DIEM_TRUNG_BINH_KY1 = detail_in.DIEM_TRUNG_BINH_KY1;
                        else
                        {
                            detail.DIEM_TRUNG_BINH_KY2 = detail_in.DIEM_TRUNG_BINH_KY2;
                            detail.DIEM_TRUNG_BINH_CN = detail_in.DIEM_TRUNG_BINH_CN;
                            detail.DIEM_TRUNG_BINH_CN_TTL = detail_in.DIEM_TRUNG_BINH_CN_TTL;
                        }
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi.ID;
                        detail.IS_DELETE = detail_in.IS_DELETE;
                        context.SaveChanges();
                        res.ResObject = detail;
                        #region "update điểm TB học kỳ (nếu đã có điểm thi học kỳ)"
                        MON_HOC_TRUONG detailMonHoc = new MON_HOC_TRUONG();
                        MonHocTruongBO monHocTruongBO = new MonHocTruongBO();
                        detailMonHoc = monHocTruongBO.getMonTruongByID(detail.ID_MON_HOC_TRUONG);
                        detail = (from p in context.DIEM_CHI_TIET
                                  where p.ID == detail_in.ID
                                  select p).FirstOrDefault();
                        double diem_tb_hoc_ky = 0;
                        if (detail.DIEM_HOC_KY != null)
                        {
                            #region "điểm số"
                            if (detail.DIEM_HOC_KY > 0 && detailMonHoc != null && (detailMonHoc.KIEU_MON == null || detailMonHoc.KIEU_MON.Value == false))
                            {
                                int countHS1 = 0, countHS2 = 0;
                                if (detail.DIEM1 != null) countHS1++;
                                if (detail.DIEM2 != null) countHS1++;
                                if (detail.DIEM3 != null) countHS1++;
                                if (detail.DIEM4 != null) countHS1++;
                                if (detail.DIEM5 != null) countHS1++;
                                if (detail.DIEM6 != null) countHS1++;
                                if (detail.DIEM7 != null) countHS1++;
                                if (detail.DIEM8 != null) countHS1++;
                                if (detail.DIEM9 != null) countHS1++;
                                if (detail.DIEM10 != null) countHS1++;
                                if (detail.DIEM11 != null) countHS1++;
                                if (detail.DIEM12 != null) countHS1++;
                                if (detail.DIEM13 != null) countHS1++;
                                if (detail.DIEM14 != null) countHS1++;
                                if (detail.DIEM15 != null) countHS1++;
                                if (detail.DIEM16 != null) countHS2++;
                                if (detail.DIEM17 != null) countHS2++;
                                if (detail.DIEM18 != null) countHS2++;
                                if (detail.DIEM19 != null) countHS2++;
                                if (detail.DIEM20 != null) countHS2++;
                                diem_tb_hoc_ky = (((detail.DIEM1 != null && detail.DIEM1 > 0 ? Convert.ToDouble(detail.DIEM1.Value) : 0) +
                                    (detail.DIEM2 != null ? Convert.ToDouble(detail.DIEM2.Value) : 0) +
                                    (detail.DIEM3 != null ? Convert.ToDouble(detail.DIEM3.Value) : 0) +
                                    (detail.DIEM4 != null ? Convert.ToDouble(detail.DIEM4.Value) : 0) +
                                    (detail.DIEM5 != null ? Convert.ToDouble(detail.DIEM5.Value) : 0) +
                                    (detail.DIEM6 != null ? Convert.ToDouble(detail.DIEM6.Value) : 0) +
                                    (detail.DIEM7 != null ? Convert.ToDouble(detail.DIEM7.Value) : 0) +
                                    (detail.DIEM8 != null ? Convert.ToDouble(detail.DIEM8.Value) : 0) +
                                    (detail.DIEM9 != null ? Convert.ToDouble(detail.DIEM9.Value) : 0) +
                                    (detail.DIEM10 != null ? Convert.ToDouble(detail.DIEM10.Value) : 0) +
                                    (detail.DIEM11 != null ? Convert.ToDouble(detail.DIEM11.Value) : 0) +
                                    (detail.DIEM12 != null ? Convert.ToDouble(detail.DIEM12.Value) : 0) +
                                    (detail.DIEM13 != null ? Convert.ToDouble(detail.DIEM13.Value) : 0) +
                                    (detail.DIEM14 != null ? Convert.ToDouble(detail.DIEM14.Value) : 0) +
                                    (detail.DIEM15 != null ? Convert.ToDouble(detail.DIEM15.Value) : 0)) + (
                                    (detail.DIEM16 != null ? Convert.ToDouble(detail.DIEM16.Value) : 0) +
                                    (detail.DIEM17 != null ? Convert.ToDouble(detail.DIEM17.Value) : 0) +
                                    (detail.DIEM18 != null ? Convert.ToDouble(detail.DIEM18.Value) : 0) +
                                    (detail.DIEM19 != null ? Convert.ToDouble(detail.DIEM19.Value) : 0) +
                                    (detail.DIEM20 != null ? Convert.ToDouble(detail.DIEM20.Value) : 0)) * 2 +
                                    Convert.ToDouble(detail.DIEM_HOC_KY.Value) * 3);
                                diem_tb_hoc_ky = diem_tb_hoc_ky * 1.0 / (countHS1 + countHS2 * 2 + 3);
                                diem_tb_hoc_ky = Math.Round((diem_tb_hoc_ky), 1, MidpointRounding.AwayFromZero);
                                if (detail.HOC_KY == 1 && diem_tb_hoc_ky > 0)
                                {
                                    detail.DIEM_TRUNG_BINH_KY1 = Convert.ToDecimal(diem_tb_hoc_ky);
                                    detail.DIEM_TRUNG_BINH_CN = Convert.ToDecimal(diem_tb_hoc_ky);
                                    context.SaveChanges();
                                    res.ResObject = detail;
                                }
                                else if (detail.HOC_KY == 2 && diem_tb_hoc_ky > 0)
                                {
                                    detail.DIEM_TRUNG_BINH_KY2 = Convert.ToDecimal(diem_tb_hoc_ky);
                                    double diem_tb_ca_nam = 0;
                                    diem_tb_ca_nam = (detail.DIEM_TRUNG_BINH_KY1 != null && detail.DIEM_TRUNG_BINH_KY1.Value > 0) ?
                                        ((Convert.ToDouble(detail.DIEM_TRUNG_BINH_KY1.Value) + diem_tb_hoc_ky * 2) * 1.0 / 3) :
                                        diem_tb_hoc_ky;
                                    detail.DIEM_TRUNG_BINH_CN = Convert.ToDecimal(diem_tb_ca_nam);
                                    context.SaveChanges();
                                    res.ResObject = detail;
                                }
                            }
                            #endregion
                            #region "điểm nhận xét"
                            else
                            {
                                int countD = 0, countCD = 0;
                                if (detail.DIEM1 != null) if (detail.DIEM1 == 0) countCD++; else if (detail.DIEM1 == 1) countD++;
                                if (detail.DIEM2 != null) if (detail.DIEM2 == 0) countCD++; else if (detail.DIEM2 == 1) countD++;
                                if (detail.DIEM3 != null) if (detail.DIEM3 == 0) countCD++; else if (detail.DIEM3 == 1) countD++;
                                if (detail.DIEM4 != null) if (detail.DIEM4 == 0) countCD++; else if (detail.DIEM4 == 1) countD++;
                                if (detail.DIEM5 != null) if (detail.DIEM5 == 0) countCD++; else if (detail.DIEM5 == 1) countD++;
                                if (detail.DIEM6 != null) if (detail.DIEM6 == 0) countCD++; else if (detail.DIEM6 == 1) countD++;
                                if (detail.DIEM7 != null) if (detail.DIEM7 == 0) countCD++; else if (detail.DIEM7 == 1) countD++;
                                if (detail.DIEM8 != null) if (detail.DIEM8 == 0) countCD++; else if (detail.DIEM8 == 1) countD++;
                                if (detail.DIEM9 != null) if (detail.DIEM9 == 0) countCD++; else if (detail.DIEM9 == 1) countD++;
                                if (detail.DIEM10 != null) if (detail.DIEM10 == 0) countCD++; else if (detail.DIEM10 == 1) countD++;
                                if (detail.DIEM11 != null) if (detail.DIEM11 == 0) countCD++; else if (detail.DIEM11 == 1) countD++;
                                if (detail.DIEM12 != null) if (detail.DIEM12 == 0) countCD++; else if (detail.DIEM12 == 1) countD++;
                                if (detail.DIEM13 != null) if (detail.DIEM13 == 0) countCD++; else if (detail.DIEM13 == 1) countD++;
                                if (detail.DIEM14 != null) if (detail.DIEM14 == 0) countCD++; else if (detail.DIEM14 == 1) countD++;
                                if (detail.DIEM15 != null) if (detail.DIEM15 == 0) countCD++; else if (detail.DIEM15 == 1) countD++;
                                if (detail.DIEM16 != null) if (detail.DIEM16 == 0) countCD++; else if (detail.DIEM16 == 1) countD++;
                                if (detail.DIEM17 != null) if (detail.DIEM17 == 0) countCD++; else if (detail.DIEM17 == 1) countD++;
                                if (detail.DIEM18 != null) if (detail.DIEM18 == 0) countCD++; else if (detail.DIEM18 == 1) countD++;
                                if (detail.DIEM19 != null) if (detail.DIEM19 == 0) countCD++; else if (detail.DIEM19 == 1) countD++;
                                if (detail.DIEM20 != null) if (detail.DIEM20 == 0) countCD++; else if (detail.DIEM20 == 1) countD++;
                                if (detail.HOC_KY == 1)
                                {
                                    if (detail.DIEM_HOC_KY != null && detail.DIEM_HOC_KY == 1 && countD >= (countD + countCD) * 2 * 1.0 / 3)
                                    {
                                        detail.DIEM_TRUNG_BINH_KY1 = 1;
                                        detail.DIEM_TRUNG_BINH_CN = 1;
                                    }
                                    else
                                    {
                                        detail.DIEM_TRUNG_BINH_KY1 = 0;
                                        detail.DIEM_TRUNG_BINH_CN = 0;
                                    }
                                    context.SaveChanges();
                                    res.ResObject = detail;
                                }
                                else if (detail.HOC_KY == 2)
                                {
                                    if (detail.DIEM_HOC_KY != null && detail.DIEM_HOC_KY == 1 && countD >= (countD + countCD) * 2 * 1.0 / 3)
                                        diem_tb_hoc_ky = 1;
                                    else diem_tb_hoc_ky = 0;
                                    detail.DIEM_TRUNG_BINH_KY2 = Convert.ToDecimal(diem_tb_hoc_ky);
                                    if (diem_tb_hoc_ky == 1) detail.DIEM_TRUNG_BINH_CN = 1;
                                    else detail.DIEM_TRUNG_BINH_CN = 0;
                                    context.SaveChanges();
                                    res.ResObject = detail;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            if (detail.HOC_KY == 1) detail.DIEM_TRUNG_BINH_KY1 = null;
                            else
                            {
                                detail.DIEM_TRUNG_BINH_KY2 = null;
                                detail.DIEM_TRUNG_BINH_CN = null;
                                context.SaveChanges();
                                res.ResObject = detail;
                            }
                        }

                        #endregion
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DIEM_CHI_TIET");
            }
            catch (DbEntityValidationException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
                //res.Msg = ex.ToString();
            }
            return res;
        }
        public ResultEntity insert(DIEM_CHI_TIET detail_in, long? nguoi)
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
                    detail_in = context.DIEM_CHI_TIET.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DIEM_CHI_TIET");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insertEmpty(long id_lop, short? id_mon_hoc, long id_truong, long id_mon_hoc_truong, short hoc_ky, short id_nam_hoc, short ma_khoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    string strQuery = string.Format(@"insert into DIEM_CHI_TIET(ID_HOC_SINH,ID_LOP,HOC_KY,ID_MON_HOC,ID_TRUONG,ID_MON_HOC_TRUONG,ID_NAM_HOC,MA_KHOI,DIEM_TRUNG_BINH_KY1)
                                                      select HOC_SINH.ID,:0 as ID_LOP,:1 as HOC_KY,:2 as ID_MON_HOC,:3 as ID_TRUONG,:4 as ID_MON_HOC_TRUONG,:5 as ID_NAM_HOC,:6 as MA_KHOI,dct.DIEM_TRUNG_BINH_KY1
                                                      from HOC_SINH 
                                                      left join DIEM_CHI_TIET dct on HOC_SINH.ID_LOP=dct.ID_LOP and HOC_SINH.ID=dct.ID_HOC_SINH and dct.HOC_KY=1 and dct.ID_MON_HOC_TRUONG=:4 and dct.ID_NAM_HOC=:5
                                                      and not ( dct.IS_DELETE is not null and dct.IS_DELETE =1 )
                                                      where HOC_SINH.ID_LOP=:0  and not ( HOC_SINH.IS_DELETE is not null and HOC_SINH.IS_DELETE =1 )
                                                      and not exists (select * from DIEM_CHI_TIET d 
                                                                      where d.ID_HOC_SINH=HOC_SINH.ID and d.HOC_KY=:1 and d.ID_LOP=:0 and d.ID_MON_HOC_TRUONG=:4 and d.ID_NAM_HOC=:5)");
                    context.Database.ExecuteSqlCommand(strQuery, id_lop, hoc_ky, id_mon_hoc == null ? (object)DBNull.Value : id_mon_hoc.Value, id_truong, id_mon_hoc_truong, id_nam_hoc, ma_khoi);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DIEM_CHI_TIET");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity updateDiemTBKy1SangKy2(long id_truong, short id_nam_hoc, long id_lop)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    string strQuery = string.Format(@"update diem_chi_tiet a
                    set diem_trung_binh_ky1 = (select b.diem_trung_binh_ky1 
                    from diem_chi_tiet b 
                    where a.ID_HOC_SINH=b.ID_HOC_SINH and a.ID_MON_HOC_TRUONG=b.ID_MON_HOC_TRUONG
                    and a.ID_TRUONG=b.ID_TRUONG and a.ID_LOP=b.ID_LOP and a.ID_NAM_HOC = b.ID_NAM_HOC and b.hoc_ky=1)
                    where a.id_truong=:0 and a.id_nam_hoc=:1 and a.ID_LOP=:2 and a.hoc_ky=2");
                    context.Database.ExecuteSqlCommand(strQuery, id_truong, id_nam_hoc, id_lop);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DIEM_CHI_TIET");
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
                        var sql = @"update DIEM_CHI_TIET set IS_DELETE = 1,NGUOI_SUA=:0,NGAY_SUA=:1 where ID = :2";
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, id);
                    }
                    else
                    {
                        var sql = @"delete DIEM_CHI_TIET where ID = :0";
                        context.Database.ExecuteSqlCommand(sql, id);
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DIEM_CHI_TIET");
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
                    string sql = string.Format(@"Update DIEM_CHI_TIET set ID_LOP=:0, MA_KHOI=:1, NGUOI_SUA=:2, NGAY_SUA=:3 where ID_HOC_SINH=:4");
                    context.Database.ExecuteSqlCommand(sql, id_lop, ma_khoi, nguoi, DateTime.Now, id_hoc_sinh);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DIEM_CHI_TIET");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        #endregion

        public List<DiemChiTietEntity> getDiemGuiTinHangNgay(short id_nam_hoc, long id_truong, short ma_khoi, long id_lop, short hoc_ky, DateTime ngay, long id_hoc_sinh)
        {
            List<DiemChiTietEntity> data = new List<DiemChiTietEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DIEM_CHI_TIET", "HOC_SINH", "MON_HOC_TRUONG", "getDiemChiTietByTruongKhoiLopHocSinhNgay", id_nam_hoc, id_truong, ma_khoi, id_lop, hoc_ky, ngay, id_hoc_sinh);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = string.Format(@"select d.*, m.Ten as TEN_MON_TRUONG
,case when m.kieu_mon = 0 then 
 (case when diem1 is not null and TRUNC(ngay_diem1) = TRUNC({0}) then rtrim(to_char(diem1, 'FM90D99'), to_char(0, 'D')) || ','  else '' end
||case when diem2 is not null and TRUNC(ngay_diem2) = TRUNC({0}) then rtrim(to_char(diem2, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem3 is not null and TRUNC(ngay_diem3) = TRUNC({0}) then rtrim(to_char(diem3, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem4 is not null and TRUNC(ngay_diem4) = TRUNC({0}) then rtrim(to_char(diem4, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem5 is not null and TRUNC(ngay_diem5) = TRUNC({0}) then rtrim(to_char(diem5, 'FM90D99'), to_char(0, 'D')) ||','  else '' end) 
else
(case when diem1 is not null and diem1 = 1 and TRUNC(ngay_diem1) = TRUNC({0}) then 'Đ' || ','  
      WHEN diem1 is not null and diem1 = 0 and TRUNC(ngay_diem1) = TRUNC({0}) then 'CĐ' || ',' else '' end
||case when diem2 is not null and diem2 = 1 and TRUNC(ngay_diem2) = TRUNC({0}) then 'Đ' ||','
      WHEN diem2 is not null and diem2 = 0 and TRUNC(ngay_diem2) = TRUNC({0}) then 'CĐ' || ',' else '' end
||case when diem3 is not null and diem3 = 1 and TRUNC(ngay_diem3) = TRUNC({0}) then 'Đ' ||','
      WHEN diem3 is not null and diem3 = 0 and TRUNC(ngay_diem3) = TRUNC({0}) then 'CĐ' || ',' else '' end
||case when diem4 is not null and diem4 = 1 and TRUNC(ngay_diem4) = TRUNC({0}) then 'Đ' ||',' 
      WHEN diem4 is not null and diem4 = 0 and TRUNC(ngay_diem4) = TRUNC({0}) then 'CĐ' || ',' else '' end
||case when diem5 is not null and diem5 = 1 and TRUNC(ngay_diem5) = TRUNC({0}) then 'Đ' ||','  
      WHEN diem5 is not null and diem5 = 0 and TRUNC(ngay_diem5) = TRUNC({0}) then 'CĐ' || ',' else '' end
) end as diemMieng
,case when m.kieu_mon = 0 then
(case when diem6 is not null and TRUNC(ngay_diem6) = TRUNC({0}) then rtrim(to_char(diem6, 'FM90D99'), to_char(0, 'D')) || ','  else '' end
||case when diem7 is not null and TRUNC(ngay_diem7) = TRUNC({0}) then rtrim(to_char(diem7, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem8 is not null and TRUNC(ngay_diem8) = TRUNC({0}) then rtrim(to_char(diem8, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem9 is not null and TRUNC(ngay_diem9) = TRUNC({0}) then rtrim(to_char(diem9, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem10 is not null and TRUNC(ngay_diem10) = TRUNC({0}) then rtrim(to_char(diem10, 'FM90D99'), to_char(0, 'D')) ||','  else '' end)
else 
 (case when diem6 is not null and diem6 = 1 and TRUNC(ngay_diem6) = TRUNC({0}) then 'Đ' ||',' WHEN diem6 is not null and diem6 = 0 and TRUNC(ngay_diem6) = TRUNC({0}) then 'CĐ' || ',' else '' end
||case when diem7 is not null and diem7 = 1 and TRUNC(ngay_diem7) = TRUNC({0}) then 'Đ' ||',' WHEN diem7 is not null and diem7 = 0 and TRUNC(ngay_diem7) = TRUNC({0}) then 'CĐ' || ',' else '' end
||case when diem8 is not null and diem8 = 1 and TRUNC(ngay_diem8) = TRUNC({0}) then 'Đ' ||',' WHEN diem8 is not null and diem8 = 0 and TRUNC(ngay_diem8) = TRUNC({0}) then 'CĐ' || ',' else '' end
||case when diem9 is not null and diem9 = 1 and TRUNC(ngay_diem9) = TRUNC({0}) then 'Đ' ||',' WHEN diem9 is not null and diem9 = 0 and TRUNC(ngay_diem9) = TRUNC({0}) then 'CĐ' || ',' else '' end
||case when diem10 is not null and diem10 = 1  and TRUNC(ngay_diem10) = TRUNC({0})then 'Đ' ||',' WHEN diem10 is not null and diem10 = 0 and TRUNC(ngay_diem10) = TRUNC({0}) then 'CĐ' || ',' else '' end
) end as diem15P
,case when m.kieu_mon = 0 then 
 (case when diem11 is not null and TRUNC(ngay_diem11) = TRUNC({0}) then rtrim(to_char(diem11, 'FM90D99'), to_char(0, 'D')) || ','  else '' end
||case when diem12 is not null and TRUNC(ngay_diem12) = TRUNC({0}) then rtrim(to_char(diem12, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem13 is not null and TRUNC(ngay_diem13) = TRUNC({0}) then rtrim(to_char(diem13, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem14 is not null and TRUNC(ngay_diem14) = TRUNC({0}) then rtrim(to_char(diem14, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem15 is not null and TRUNC(ngay_diem15) = TRUNC({0}) then rtrim(to_char(diem15, 'FM90D99'), to_char(0, 'D')) ||','  else '' end)
else
(case when diem11 is not null and diem11 = 1 and TRUNC(ngay_diem11) = TRUNC({0}) then 'Đ' ||',' WHEN diem11 is not null and diem11 = 0 and TRUNC(ngay_diem11) = TRUNC({0}) then 'CĐ' || ',' else '' end
||case when diem12 is not null and diem12 = 1 and TRUNC(ngay_diem12) = TRUNC({0}) then 'Đ' ||',' WHEN diem12 is not null and diem12 = 0 and TRUNC(ngay_diem12) = TRUNC({0}) then 'CĐ' || ',' else '' end
||case when diem13 is not null and diem13 = 1 and TRUNC(ngay_diem13) = TRUNC({0}) then 'Đ' ||',' WHEN diem13 is not null and diem13 = 0 and TRUNC(ngay_diem13) = TRUNC({0}) then 'CĐ' || ',' else '' end
||case when diem14 is not null and diem14 = 1 and TRUNC(ngay_diem14) = TRUNC({0}) then 'Đ' ||',' WHEN diem14 is not null and diem14 = 0 and TRUNC(ngay_diem14) = TRUNC({0}) then 'CĐ' || ',' else '' end
||case when diem15 is not null and diem15 = 1 and TRUNC(ngay_diem15) = TRUNC({0}) then 'Đ' ||',' WHEN diem15 is not null and diem15 = 0 and TRUNC(ngay_diem15) = TRUNC({0}) then 'CĐ' || ',' else '' end
) end as diem1T_HS1
,case when m.kieu_mon = 0 then 
 (case when diem16 is not null and TRUNC(ngay_diem16) = TRUNC({0}) then rtrim(to_char(diem16, 'FM90D99'), to_char(0, 'D')) || ','  else '' end
||case when diem17 is not null and TRUNC(ngay_diem17) = TRUNC({0}) then rtrim(to_char(diem17, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem18 is not null and TRUNC(ngay_diem18) = TRUNC({0}) then rtrim(to_char(diem18, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem19 is not null and TRUNC(ngay_diem19) = TRUNC({0}) then rtrim(to_char(diem19, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem20 is not null and TRUNC(ngay_diem20) = TRUNC({0}) then rtrim(to_char(diem20, 'FM90D99'), to_char(0, 'D')) ||','  else '' end)
else
 (case when diem16 is not null and diem16 = 1 and TRUNC(ngay_diem16) = TRUNC({0}) then 'Đ' ||',' WHEN diem16 is not null and diem16 = 0 and TRUNC(ngay_diem16) = TRUNC({0}) then 'CĐ' || ',' else '' end
||case when diem17 is not null and diem17 = 1 and TRUNC(ngay_diem17) = TRUNC({0}) then 'Đ' ||',' WHEN diem17 is not null and diem17 = 0 and TRUNC(ngay_diem17) = TRUNC({0}) then 'CĐ' || ',' else '' end
||case when diem18 is not null and diem18 = 1 and TRUNC(ngay_diem18) = TRUNC({0}) then 'Đ' ||',' WHEN diem18 is not null and diem18 = 0 and TRUNC(ngay_diem18) = TRUNC({0}) then 'CĐ' || ',' else '' end
||case when diem19 is not null and diem19 = 1 and TRUNC(ngay_diem19) = TRUNC({0}) then 'Đ' ||',' WHEN diem19 is not null and diem19 = 0 and TRUNC(ngay_diem19) = TRUNC({0}) then 'CĐ' || ',' else '' end
||case when diem20 is not null and diem20 = 1 and TRUNC(ngay_diem20) = TRUNC({0}) then 'Đ' ||',' WHEN diem20 is not null and diem20 = 0 and TRUNC(ngay_diem20) = TRUNC({0}) then 'CĐ' || ',' else '' end
) end as diem1T_HS2
,case when m.kieu_mon = 0 then 
    case when DIEM_HOC_KY is not null and TRUNC(NGAY_DIEM_HOC_KY) = TRUNC({0}) then rtrim(to_char(DIEM_HOC_KY, 'FM90D99'), to_char(0, 'D')) || ','  else '' end
else
    case when DIEM_HOC_KY is not null and DIEM_HOC_KY = 1 and TRUNC(NGAY_DIEM_HOC_KY) = TRUNC( {0}) then 'Đ' ||',' WHEN DIEM_HOC_KY is not null and DIEM_HOC_KY = 0 and TRUNC(NGAY_DIEM_HOC_KY) = TRUNC({0}) then 'CĐ' || ',' else '' end
end as diemHocKy
                    from diem_chi_tiet d 
                    left join mon_hoc_truong m on d.id_mon_hoc_truong = m.id
                    where d.ID_NAM_HOC=:0 and d.ID_TRUONG=:1 and d.hoc_ky=:2
                        and d.ma_khoi=:3 and d.id_lop=:4 and d.ID_HOC_SINH=:5
                        and not (d.IS_DELETE is not null and d.IS_DELETE =1) 
                        and (TRUNC(ngay_diem1) = TRUNC({0})
                        or TRUNC(ngay_diem2) = TRUNC({0})
                        or TRUNC(ngay_diem3) = TRUNC({0})
                        or TRUNC(ngay_diem4) = TRUNC({0})
                        or TRUNC(ngay_diem5) = TRUNC({0})
                        or TRUNC(ngay_diem6) = TRUNC({0})
                        or TRUNC(ngay_diem7) = TRUNC({0})
                        or TRUNC(ngay_diem8) = TRUNC({0})
                        or TRUNC(ngay_diem9) = TRUNC({0})
                        or TRUNC(ngay_diem10) = TRUNC({0})
                        or TRUNC(ngay_diem11) = TRUNC({0})
                        or TRUNC(ngay_diem12) = TRUNC({0})
                        or TRUNC(ngay_diem13) = TRUNC({0})
                        or TRUNC(ngay_diem14) = TRUNC({0})
                        or TRUNC(ngay_diem15) = TRUNC({0})
                        or TRUNC(ngay_diem16) = TRUNC({0})
                        or TRUNC(ngay_diem17) = TRUNC({0})
                        or TRUNC(ngay_diem18) = TRUNC({0})
                        or TRUNC(ngay_diem19) = TRUNC({0})
                        or TRUNC(ngay_diem20) = TRUNC({0})
                        or TRUNC(ngay_diem21) = TRUNC({0})
                        or TRUNC(ngay_diem22) = TRUNC({0})
                        or TRUNC(ngay_diem23) = TRUNC({0})
                        or TRUNC(ngay_diem24) = TRUNC({0})
                        or TRUNC(ngay_diem25) = TRUNC({0})
                        or TRUNC(NGAY_DIEM_HOC_KY) = TRUNC({0})
                        )", "to_date('" + ngay.ToString("dd/MM/yyyy") + "', 'dd/mm/yyyy')");
                    List<object> lstObjectParameter = new List<object>();
                    lstObjectParameter.Add(id_nam_hoc);
                    lstObjectParameter.Add(id_truong);
                    lstObjectParameter.Add(hoc_ky);
                    lstObjectParameter.Add(ma_khoi);
                    lstObjectParameter.Add(id_lop);
                    lstObjectParameter.Add(id_hoc_sinh);
                    
                    data = context.Database.SqlQuery<DiemChiTietEntity>(strQuery, lstObjectParameter.ToArray()).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<DiemChiTietEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }

        #region Tổng hợp điểm từ ngày A đến ngày B
        public List<TongHopNhanXetHangNgayEntity> getNhanXetTheoNgay(long id_truong, short? ma_khoi, long? id_lop, DateTime ngay, short id_nam_hoc, short ma_hoc_ky)
        {
            List<TongHopNhanXetHangNgayEntity> data = new List<TongHopNhanXetHangNgayEntity>();
            var QICache = new DefaultCacheProvider();
            string ngayTongHop = ngay.ToString("yyyyMMdd");
            string strKeyCache = QICache.BuildCachedKey("TONG_HOP_NHAN_XET_HANG_NGAY", "HOC_SINH", "GIOI_TINH", "getNhanXetTheoNgay", id_truong, ma_khoi, id_lop, ngayTongHop, id_nam_hoc, ma_hoc_ky);
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
                                 and to_char(NGAY_TONG_HOP, 'YYYYMMDD') = :0
                        join GIOI_TINH g on hs.MA_GIOI_TINH=g.MA
                        where not ( hs.is_delete is not null and hs.is_delete = 1)
                        AND hs.id_truong = :1 and hs.id_khoi = :2 
                        and hs.id_nam_hoc = :3 and hs.id_lop = :4 {0} {1}
                        order by nvl(hs.THU_TU, 0),NLSSORT(hs.ten,'NLS_SORT=vietnamese'),NLSSORT(hs.ho_dem,'NLS_SORT=vietnamese')",
                        ma_hoc_ky == 1 ? " and hs.TRANG_THAI_HOC in(1,2,3,8,9,10)" : " and hs.TRANG_THAI_HOC in (1,2,3,6,7,10)",
                        ma_hoc_ky == 1 ? " and ((hs.is_dk_ky1 is not null and hs.is_dk_ky1 = 1) or (hs.is_mien_giam_ky1 is not null and hs.is_mien_giam_ky1 = 1))" : " and ((hs.is_dk_ky2 is not null and hs.is_dk_ky2 = 1) or (hs.is_mien_giam_ky2 is not null and hs.is_mien_giam_ky2 = 1))"
                        );
                    data = context.Database.SqlQuery<TongHopNhanXetHangNgayEntity>(strQuery, ngayTongHop, id_truong, ma_khoi, id_nam_hoc, id_lop).ToList();
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

        public List<DiemChiTietEntity> getDiemChiTietTheoNgay(short id_nam_hoc, long id_truong, short? ma_khoi, long? id_lop, short hoc_ky, DateTime tu_ngay, DateTime den_ngay, long? id_hoc_sinh)
        {
            List<DiemChiTietEntity> data = new List<DiemChiTietEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DIEM_CHI_TIET", "HOC_SINH", "MON_HOC_TRUONG", "getDiemChiTietTheoNgay", id_nam_hoc, id_truong, ma_khoi, id_lop, hoc_ky, tu_ngay.ToString("yyyyMMdd"), den_ngay.ToString("yyyyMMdd"), id_hoc_sinh);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {

                    string strQuery = string.Format(@"select d.*, m.Ten as TEN_MON_TRUONG
,case when m.kieu_mon = 0 then 
 (case when diem1 is not null and TO_CHAR(ngay_diem1, 'YYYYMMDD') >= {0} and TO_CHAR(ngay_diem1, 'YYYYMMDD') <= {1} then rtrim(to_char(diem1, 'FM90D99'), to_char(0, 'D')) || ','  else '' end
||case when diem2 is not null and TO_CHAR(ngay_diem2, 'YYYYMMDD') >= {0} and TO_CHAR(ngay_diem2, 'YYYYMMDD') <= {1}  then rtrim(to_char(diem2, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem3 is not null and TO_CHAR(ngay_diem3, 'YYYYMMDD') >= {0} and TO_CHAR(ngay_diem3, 'YYYYMMDD') <= {1}  then rtrim(to_char(diem3, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem4 is not null and TO_CHAR(ngay_diem4, 'YYYYMMDD') >= {0} and TO_CHAR(ngay_diem4, 'YYYYMMDD') <= {1}  then rtrim(to_char(diem4, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem5 is not null and TO_CHAR(ngay_diem5, 'YYYYMMDD') >= {0} and TO_CHAR(ngay_diem5, 'YYYYMMDD') <= {1}  then rtrim(to_char(diem5, 'FM90D99'), to_char(0, 'D')) ||','  else '' end) 
else
(case when diem1 is not null and diem1 = 1 and to_char(ngay_diem1, 'YYYYMMDD') >= {0} and to_char(ngay_diem1, 'YYYYMMDD') <= {1} then 'Đ' || ',' 
    WHEN diem1 is not null and diem1 = 0 and to_char(ngay_diem1, 'YYYYMMDD') >= {0} and to_char(ngay_diem1, 'YYYYMMDD') <= {1} then 'CĐ' || ',' 
    else '' end
||case when diem2 is not null and diem2 = 1 and to_char(ngay_diem2, 'YYYYMMDD') >= {0} and to_char(ngay_diem2, 'YYYYMMDD') <= {1} then 'Đ' ||',' WHEN diem2 is not null and diem2 = 0 and to_char(ngay_diem2, 'YYYYMMDD') >= {0} and to_char(ngay_diem2, 'YYYYMMDD') <= {1} then 'CĐ' || ',' else '' end
||case when diem3 is not null and diem3 = 1 and to_char(ngay_diem3, 'YYYYMMDD') >= {0} and to_char(ngay_diem3, 'YYYYMMDD') <= {1} then 'Đ' ||',' WHEN diem3 is not null and diem3 = 0 and to_char(ngay_diem3, 'YYYYMMDD') >= {0} and to_char(ngay_diem3, 'YYYYMMDD') <= {1} then 'CĐ' || ',' else '' end
||case when diem4 is not null and diem4 = 1 and to_char(ngay_diem4, 'YYYYMMDD') >= {0} and to_char(ngay_diem4, 'YYYYMMDD') <= {1} then 'Đ' ||',' WHEN diem4 is not null and diem4 = 0 and to_char(ngay_diem4, 'YYYYMMDD') >= {0} and to_char(ngay_diem4, 'YYYYMMDD') <= {1} then 'CĐ' || ',' else '' end
||case when diem5 is not null and diem5 = 1 and to_char(ngay_diem5, 'YYYYMMDD') >= {0} and to_char(ngay_diem5, 'YYYYMMDD') <= {1} then 'Đ' ||',' WHEN diem5 is not null and diem5 = 0 and to_char(ngay_diem5, 'YYYYMMDD') >= {0} and to_char(ngay_diem5, 'YYYYMMDD') <= {1} then 'CĐ' || ',' else '' end
) end as diemMieng         
,case when m.kieu_mon = 0 then
 (case when diem6 is not null and to_char(ngay_diem6, 'YYYYMMDD') >= {0} and to_char(ngay_diem6, 'YYYYMMDD') <= {1} then rtrim(to_char(diem6, 'FM90D99'), to_char(0, 'D')) || ','  else '' end
||case when diem7 is not null and to_char(ngay_diem7, 'YYYYMMDD') >= {0} and to_char(ngay_diem7, 'YYYYMMDD') <= {1} then rtrim(to_char(diem7, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem8 is not null and to_char(ngay_diem8, 'YYYYMMDD') >= {0} and to_char(ngay_diem8, 'YYYYMMDD') <= {1} then rtrim(to_char(diem8, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem9 is not null and to_char(ngay_diem9, 'YYYYMMDD') >= {0} and to_char(ngay_diem9, 'YYYYMMDD') <= {1} then rtrim(to_char(diem9, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem10 is not null and to_char(ngay_diem10, 'YYYYMMDD') >= {0} and to_char(ngay_diem10, 'YYYYMMDD') <= {1} then rtrim(to_char(diem10, 'FM90D99'), to_char(0, 'D')) ||','  else '' end)
else 
 (case when diem6 is not null and diem6 = 1 and to_char(ngay_diem6, 'YYYYMMDD') >= {0} and to_char(ngay_diem6, 'YYYYMMDD') <= {1} then 'Đ' ||',' WHEN diem6 is not null and diem6 = 0 and to_char(ngay_diem6, 'YYYYMMDD') >= {0} and to_char(ngay_diem6, 'YYYYMMDD') <= {1} then 'CĐ' || ',' else '' end
||case when diem7 is not null and diem7 = 1 and to_char(ngay_diem7, 'YYYYMMDD') >= {0} and to_char(ngay_diem7, 'YYYYMMDD') <= {1} then 'Đ' ||',' WHEN diem7 is not null and diem7 = 0 and to_char(ngay_diem7, 'YYYYMMDD') >= {0} and to_char(ngay_diem7, 'YYYYMMDD') <= {1} then 'CĐ' || ',' else '' end
||case when diem8 is not null and diem8 = 1 and to_char(ngay_diem8, 'YYYYMMDD') >= {0} and to_char(ngay_diem8, 'YYYYMMDD') <= {1} then 'Đ' ||',' WHEN diem8 is not null and diem8 = 0 and to_char(ngay_diem8, 'YYYYMMDD') >= {0} and to_char(ngay_diem8, 'YYYYMMDD') <= {1} then 'CĐ' || ',' else '' end
||case when diem9 is not null and diem9 = 1 and to_char(ngay_diem9, 'YYYYMMDD') >= {0} and to_char(ngay_diem9, 'YYYYMMDD') <= {1} then 'Đ' ||',' WHEN diem9 is not null and diem9 = 0 and to_char(ngay_diem9, 'YYYYMMDD') >= {0} and to_char(ngay_diem9, 'YYYYMMDD') <= {1} then 'CĐ' || ',' else '' end
||case when diem10 is not null and diem10 = 1 and to_char(ngay_diem10, 'YYYYMMDD') >= {0} and to_char(ngay_diem10, 'YYYYMMDD') <= {1} then 'Đ' ||',' WHEN diem10 is not null and diem10 = 0 and to_char(ngay_diem10, 'YYYYMMDD') >= {0} and to_char(ngay_diem10, 'YYYYMMDD') <= {1} then 'CĐ' || ',' else '' end
) end as diem15P
,case when m.kieu_mon = 0 then 
 (case when diem11 is not null and to_char(ngay_diem11, 'YYYYMMDD') >= {0} and to_char(ngay_diem11, 'YYYYMMDD') <= {1} then rtrim(to_char(diem11, 'FM90D99'), to_char(0, 'D')) || ','  else '' end
||case when diem12 is not null and to_char(ngay_diem12, 'YYYYMMDD') >= {0} and to_char(ngay_diem12, 'YYYYMMDD') <= {1} then rtrim(to_char(diem12, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem13 is not null and to_char(ngay_diem13, 'YYYYMMDD') >= {0} and to_char(ngay_diem13, 'YYYYMMDD') <= {1} then rtrim(to_char(diem13, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem14 is not null and to_char(ngay_diem14, 'YYYYMMDD') >= {0} and to_char(ngay_diem14, 'YYYYMMDD') <= {1} then rtrim(to_char(diem14, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem15 is not null and to_char(ngay_diem15, 'YYYYMMDD') >= {0} and to_char(ngay_diem15, 'YYYYMMDD') <= {1} then rtrim(to_char(diem15, 'FM90D99'), to_char(0, 'D')) ||','  else '' end)
else
(case when diem11 is not null and diem11 = 1 and to_char(ngay_diem11, 'YYYYMMDD') >= {0} and to_char(ngay_diem11, 'YYYYMMDD') <= {1} then 'Đ' ||',' WHEN diem11 is not null and diem11 = 0 and to_char(ngay_diem11, 'YYYYMMDD') >= {0} and to_char(ngay_diem11, 'YYYYMMDD') <= {1} then 'CĐ' || ',' else '' end
||case when diem12 is not null and diem12 = 1 and to_char(ngay_diem12, 'YYYYMMDD') >= {0} and to_char(ngay_diem12, 'YYYYMMDD') <= {1} then 'Đ' ||',' WHEN diem12 is not null and diem12 = 0 and to_char(ngay_diem12, 'YYYYMMDD') >= {0} and to_char(ngay_diem12, 'YYYYMMDD') <= {1} then 'CĐ' || ',' else '' end
||case when diem13 is not null and diem13 = 1 and to_char(ngay_diem13, 'YYYYMMDD') >= {0} and to_char(ngay_diem13, 'YYYYMMDD') <= {1} then 'Đ' ||',' WHEN diem13 is not null and diem13 = 0 and to_char(ngay_diem13, 'YYYYMMDD') >= {0} and to_char(ngay_diem13, 'YYYYMMDD') <= {1} then 'CĐ' || ',' else '' end
||case when diem14 is not null and diem14 = 1 and to_char(ngay_diem14, 'YYYYMMDD') >= {0} and to_char(ngay_diem14, 'YYYYMMDD') <= {1} then 'Đ' ||',' WHEN diem14 is not null and diem14 = 0 and to_char(ngay_diem14, 'YYYYMMDD') >= {0} and to_char(ngay_diem14, 'YYYYMMDD') <= {1} then 'CĐ' || ',' else '' end
||case when diem15 is not null and diem15 = 1 and to_char(ngay_diem15, 'YYYYMMDD') >= {0} and to_char(ngay_diem15, 'YYYYMMDD') <= {1} then 'Đ' ||',' WHEN diem15 is not null and diem15 = 0 and to_char(ngay_diem15, 'YYYYMMDD') >= {0} and to_char(ngay_diem15, 'YYYYMMDD') <= {1} then 'CĐ' || ',' else '' end
) end as diem1T_HS1 
,case when m.kieu_mon = 0 then 
 (case when diem16 is not null and to_char(ngay_diem16, 'YYYYMMDD') >= {0} and to_char(ngay_diem16, 'YYYYMMDD') <= {1} then rtrim(to_char(diem16, 'FM90D99'), to_char(0, 'D')) || ','  else '' end
||case when diem17 is not null and to_char(ngay_diem17, 'YYYYMMDD') >= {0} and to_char(ngay_diem17, 'YYYYMMDD') <= {1} then rtrim(to_char(diem17, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem18 is not null and to_char(ngay_diem18, 'YYYYMMDD') >= {0} and to_char(ngay_diem18, 'YYYYMMDD') <= {1} then rtrim(to_char(diem18, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem19 is not null and to_char(ngay_diem19, 'YYYYMMDD') >= {0} and to_char(ngay_diem19, 'YYYYMMDD') <= {1} then rtrim(to_char(diem19, 'FM90D99'), to_char(0, 'D')) ||','  else '' end
||case when diem20 is not null and to_char(ngay_diem20, 'YYYYMMDD') >= {0} and to_char(ngay_diem20, 'YYYYMMDD') <= {1} then rtrim(to_char(diem20, 'FM90D99'), to_char(0, 'D')) ||','  else '' end)
else
 (case when diem16 is not null and diem16 = 1 and to_char(ngay_diem16, 'YYYYMMDD') >= {0} and to_char(ngay_diem16, 'YYYYMMDD') <= {1} then 'Đ' ||',' WHEN diem16 is not null and diem16 = 0 and to_char(ngay_diem16, 'YYYYMMDD') >= {0} and to_char(ngay_diem16, 'YYYYMMDD') <= {1} then 'CĐ' || ',' else '' end
||case when diem17 is not null and diem17 = 1 and to_char(ngay_diem17, 'YYYYMMDD') >= {0} and to_char(ngay_diem17, 'YYYYMMDD') <= {1} then 'Đ' ||',' WHEN diem17 is not null and diem17 = 0 and to_char(ngay_diem17, 'YYYYMMDD') >= {0} and to_char(ngay_diem17, 'YYYYMMDD') <= {1} then 'CĐ' || ',' else '' end
||case when diem18 is not null and diem18 = 1 and to_char(ngay_diem18, 'YYYYMMDD') >= {0} and to_char(ngay_diem18, 'YYYYMMDD') <= {1} then 'Đ' ||',' WHEN diem18 is not null and diem18 = 0 and to_char(ngay_diem18, 'YYYYMMDD') >= {0} and to_char(ngay_diem18, 'YYYYMMDD') <= {1} then 'CĐ' || ',' else '' end
||case when diem19 is not null and diem19 = 1 and to_char(ngay_diem19, 'YYYYMMDD') >= {0} and to_char(ngay_diem19, 'YYYYMMDD') <= {1} then 'Đ' ||',' WHEN diem19 is not null and diem19 = 0 and to_char(ngay_diem19, 'YYYYMMDD') >= {0} and to_char(ngay_diem19, 'YYYYMMDD') <= {1} then 'CĐ' || ',' else '' end
||case when diem20 is not null and diem20 = 1 and to_char(ngay_diem20, 'YYYYMMDD') >= {0} and to_char(ngay_diem20, 'YYYYMMDD') <= {1} then 'Đ' ||',' WHEN diem20 is not null and diem20 = 0 and to_char(ngay_diem20, 'YYYYMMDD') >= {0} and to_char(ngay_diem20, 'YYYYMMDD') <= {1} then 'CĐ' || ',' else '' end
) end as diem1T_HS2 
,case when m.kieu_mon = 0 then 
    case when DIEM_HOC_KY is not null and to_char(NGAY_DIEM_HOC_KY, 'YYYYMMDD') >= {0} and to_char(NGAY_DIEM_HOC_KY, 'YYYYMMDD') <= {1} then rtrim(to_char(DIEM_HOC_KY, 'FM90D99'), to_char(0, 'D')) || ','  else '' end   
else
    case when DIEM_HOC_KY is not null and DIEM_HOC_KY = 1 and to_char(NGAY_DIEM_HOC_KY, 'YYYYMMDD') >= {0} and to_char(NGAY_DIEM_HOC_KY, 'YYYYMMDD') <= {1} then 'Đ' ||',' WHEN DIEM_HOC_KY is not null and DIEM_HOC_KY = 0 and to_char(NGAY_DIEM_HOC_KY, 'YYYYMMDD') >= {0} and to_char(NGAY_DIEM_HOC_KY, 'YYYYMMDD') <= {1} then 'CĐ' || ',' else '' end
end as diemHocKy
                    from diem_chi_tiet d 
                    left join hoc_sinh h on d.id_hoc_sinh = h.id
                    left join mon_hoc_truong m on d.id_mon_hoc_truong = m.id
                    where not ( d.IS_DELETE is not null and d.IS_DELETE =1 ) 
                    and not ( h.IS_DELETE is not null and h.IS_DELETE =1 ) 
                    and d.ID_NAM_HOC=:0 and d.ID_TRUONG=:1 and d.hoc_ky=:2
                        and (
                        (to_char(ngay_diem1, 'YYYYMMDD') >= {0} and to_char(ngay_diem1, 'YYYYMMDD') <= {1})
                        or (to_char(ngay_diem2, 'YYYYMMDD') >= {0} and to_char(ngay_diem2, 'YYYYMMDD') <= {1})
                        or (to_char(ngay_diem3, 'YYYYMMDD') >= {0} and to_char(ngay_diem3, 'YYYYMMDD') <= {1})
                        or (to_char(ngay_diem4, 'YYYYMMDD') >= {0} and to_char(ngay_diem4, 'YYYYMMDD') <= {1})
                        or (to_char(ngay_diem5, 'YYYYMMDD') >= {0} and to_char(ngay_diem5, 'YYYYMMDD') <= {1})
                        or (to_char(ngay_diem6, 'YYYYMMDD') >= {0} and to_char(ngay_diem6, 'YYYYMMDD') <= {1})
                        or (to_char(ngay_diem7, 'YYYYMMDD') >= {0} and to_char(ngay_diem7, 'YYYYMMDD') <= {1})
                        or (to_char(ngay_diem8, 'YYYYMMDD') >= {0} and to_char(ngay_diem8, 'YYYYMMDD') <= {1})
                        or (to_char(ngay_diem9, 'YYYYMMDD') >= {0} and to_char(ngay_diem9, 'YYYYMMDD') <= {1})
                        or (to_char(ngay_diem10, 'YYYYMMDD') >= {0} and to_char(ngay_diem10, 'YYYYMMDD') <= {1})
                        or (to_char(ngay_diem11, 'YYYYMMDD') >= {0} and to_char(ngay_diem11, 'YYYYMMDD') <= {1})
                        or (to_char(ngay_diem12, 'YYYYMMDD') >= {0} and to_char(ngay_diem12, 'YYYYMMDD') <= {1})
                        or (to_char(ngay_diem13, 'YYYYMMDD') >= {0} and to_char(ngay_diem13, 'YYYYMMDD') <= {1})
                        or (to_char(ngay_diem14, 'YYYYMMDD') >= {0} and to_char(ngay_diem14, 'YYYYMMDD') <= {1})
                        or (to_char(ngay_diem15, 'YYYYMMDD') >= {0} and to_char(ngay_diem15, 'YYYYMMDD') <= {1})
                        or (to_char(ngay_diem16, 'YYYYMMDD') >= {0} and to_char(ngay_diem16, 'YYYYMMDD') <= {1})
                        or (to_char(ngay_diem17, 'YYYYMMDD') >= {0} and to_char(ngay_diem17, 'YYYYMMDD') <= {1})
                        or (to_char(ngay_diem18, 'YYYYMMDD') >= {0} and to_char(ngay_diem18, 'YYYYMMDD') <= {1})
                        or (to_char(ngay_diem19, 'YYYYMMDD') >= {0} and to_char(ngay_diem19, 'YYYYMMDD') <= {1})
                        or (to_char(ngay_diem20, 'YYYYMMDD') >= {0} and to_char(ngay_diem20, 'YYYYMMDD') <= {1})
                        or (to_char(ngay_diem21, 'YYYYMMDD') >= {0} and to_char(ngay_diem21, 'YYYYMMDD') <= {1})
                        or (to_char(ngay_diem22, 'YYYYMMDD') >= {0} and to_char(ngay_diem22, 'YYYYMMDD') <= {1})
                        or (to_char(ngay_diem23, 'YYYYMMDD') >= {0} and to_char(ngay_diem23, 'YYYYMMDD') <= {1})
                        or (to_char(ngay_diem24, 'YYYYMMDD') >= {0} and to_char(ngay_diem24, 'YYYYMMDD') <= {1})
                        or (to_char(ngay_diem25, 'YYYYMMDD') >= {0} and to_char(ngay_diem25, 'YYYYMMDD') <= {1})
                        or (to_char(NGAY_DIEM_HOC_KY, 'YYYYMMDD') >= {0} and to_char(NGAY_DIEM_HOC_KY, 'YYYYMMDD') <= {1})
                        )", tu_ngay.ToString("yyyyMMdd"), den_ngay.ToString("yyyyMMdd"));
                    List<object> lstObjectParameter = new List<object>();
                    lstObjectParameter.Add(id_nam_hoc);
                    lstObjectParameter.Add(id_truong);
                    lstObjectParameter.Add(hoc_ky);
                    if (ma_khoi != null)
                    {
                        strQuery += " and h.id_khoi=:" + lstObjectParameter.Count;
                        lstObjectParameter.Add(ma_khoi.Value);
                    }
                    if (id_lop != null)
                    {
                        strQuery += " and d.id_lop=:" + lstObjectParameter.Count;
                        lstObjectParameter.Add(id_lop.Value);
                    }
                    if (id_hoc_sinh != null)
                    {
                        strQuery += " and d.ID_HOC_SINH=:" + lstObjectParameter.Count;
                        lstObjectParameter.Add(id_hoc_sinh.Value);
                    }
                    strQuery += " order by h.THU_TU,h.TEN,h.HO_DEM";
                    data = context.Database.SqlQuery<DiemChiTietEntity>(strQuery, lstObjectParameter.ToArray()).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<DiemChiTietEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        #endregion
    }
}
