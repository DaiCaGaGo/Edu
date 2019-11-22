using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class LopMonBO
    {
        #region get
        #region get LOP_MON theo lop, mon, hoc ky
        public LOP_MON getLopMonByLopMonHocKy(long idLop, long idMonTruong, short hocKy)
        {
            LOP_MON data = new LOP_MON();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("LOP_MON", "getLopMonByLopMonHocKy", idLop, idMonTruong, hocKy);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.LOP_MON
                            where p.ID_LOP == idLop && p.HOC_KY == hocKy && p.ID_MON_TRUONG == idMonTruong
                            select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as LOP_MON;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        #endregion
        #region get mon để xét cho lớp theo khoi lớp học kỳ
        public List<LopMonEntity> getMonXetChoLopByKhoiLopHocKy(string khoi, long id_truong, long? idLop, int nam_hoc, int hocKy, string ma_cap_hoc)
        {
            List<LopMonEntity> data = new List<LopMonEntity>();
            int idKhoi = 0;
            if (!string.IsNullOrEmpty(khoi))
                idKhoi = Convert.ToInt16(khoi);
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MON_HOC_TRUONG", "LOP_MON", "getMonXetChoLopByKhoiLopHocKy", khoi, id_truong, idLop, nam_hoc, hocKy, ma_cap_hoc);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    #region Query
                    string strQuery = @"select m.ID,m.ID_MON_HOC,m.TEN,KIEU_MON,
                                        case when lm.id is not null and lm.trang_thai = 1 then 1
                                        else 0 end IS_CHECK,
                                        lm.ID as ID_LOP_MON,
                                        lm.ID_GIAO_VIEN
                                        ,lm.SO_COT_DIEM_MIENG
                                        ,lm.SO_COT_DIEM_15P
                                        ,lm.SO_COT_DIEM_1T_HS1
                                        ,lm.SO_COT_DIEM_1T_HS2
                                        ,lm.ID_LOP
                                        , case when lm.IS_MON_CHUYEN is not null and lm.IS_MON_CHUYEN = 1 then 1
                                        else 0 end IS_MON_CHUYEN
                                        from MON_HOC_TRUONG m
                                        left join LOP_MON lm on m.ID = lm.id_mon_truong and id_lop =:0 and lm.hoc_ky = :1 and lm.trang_thai = 1
                                        where 1=1 and m.ID_TRUONG= :2 and m.ID_NAM_HOC= :3 AND m.MA_CAP_HOC = :4";
                    if (idKhoi == 1)
                        strQuery += " and IS_1 = 1";
                    if (idKhoi == 2)
                        strQuery += " and IS_2 = 1";
                    if (idKhoi == 3)
                        strQuery += " and IS_3 = 1";
                    if (idKhoi == 4)
                        strQuery += " and IS_4 = 1";
                    if (idKhoi == 5)
                        strQuery += " and IS_5 = 1";
                    if (idKhoi == 6)
                        strQuery += " and IS_6 = 1";
                    if (idKhoi == 7)
                        strQuery += " and IS_7 = 1";
                    if (idKhoi == 8)
                        strQuery += " and IS_8 = 1";
                    if (idKhoi == 9)
                        strQuery += " and IS_9 = 1";
                    if (idKhoi == 10)
                        strQuery += " and IS_10 = 1";
                    if (idKhoi == 11)
                        strQuery += " and IS_11 = 1";
                    if (idKhoi == 12)
                        strQuery += " and IS_12 = 1";
                    strQuery += " order by m.THU_TU";
                    #endregion
                    data = context.Database.SqlQuery<LopMonEntity>(strQuery, idLop, hocKy, id_truong, nam_hoc, ma_cap_hoc).ToList();

                    QICache.Set(strKeyCache, data, 300000);
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
        #endregion
        public LOP_MON getMonChuyenByLopHocKy(long idLop, int hocKy)
        {
            LOP_MON data = new LOP_MON();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("LOP_MON", "getMonChuyenByLopHocKy", idLop, hocKy);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = @"select * from lop_mon where id_lop=:0 and hoc_ky=:1 and is_mon_chuyen is not null and is_mon_chuyen=1 and not (IS_DELETE is not null and IS_DELETE = 1)";
                    data = context.Database.SqlQuery<LOP_MON>(strQuery, idLop, hocKy).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as LOP_MON;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<ItemComboEntity> getMonTruongByLopHocKyToCombo(long id_lop, short id_hocKy, bool is_all = false, long id_all = 0, string text_all = "Chọn tất cả")
        {
            List<ItemComboEntity> data = new List<ItemComboEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("LOP_MON", "MON_HOC_TRUONG", "getMonTruongByLopHocKyToCombo", id_lop, id_hocKy, is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    #region Query
                    string strQuery = @"select mh.ID as value, mh.ten as text from lop_mon lm 
                    join mon_hoc_truong mh ON lm.id_mon_truong = mh.id 
                    where id_lop = :0 and lm.hoc_ky = :1 and lm.trang_thai = 1
                    order by mh.THU_TU,mh.TEN
                    ";

                    #endregion
                    data = context.Database.SqlQuery<ItemComboEntity>(strQuery, id_lop, id_hocKy).ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<ItemComboEntity>();
                        ItemComboEntity item_all = new ItemComboEntity();
                        item_all.value = id_all;
                        item_all.text = text_all;
                        data.Insert(0, item_all);
                    }
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<ItemComboEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<MON_HOC_TRUONG> getMonTruongByLopHocKy(long id_lop, short id_hocKy)
        {
            List<MON_HOC_TRUONG> data = new List<MON_HOC_TRUONG>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MON_HOC_TRUONG", "LOP_MON", "getMonTruongByLopHocKy", id_lop, id_hocKy);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.MON_HOC_TRUONG
                            join lm in context.LOP_MON on p.ID equals lm.ID_MON_TRUONG
                            where lm.ID_LOP == id_lop && lm.HOC_KY == id_hocKy && lm.TRANG_THAI == true
                            orderby p.THU_TU, p.TEN
                            select p).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<MON_HOC_TRUONG>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<MonHocTruongCauHinhEntity> getMonTruongCauHinhByLopHocKy(long id_lop, short id_hocKy)
        {
            List<MonHocTruongCauHinhEntity> data = new List<MonHocTruongCauHinhEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MON_HOC_TRUONG", "LOP_MON", "getMonTruongCauHinhByLopHocKy", id_lop, id_hocKy);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.MON_HOC_TRUONG
                            join lm in context.LOP_MON on p.ID equals lm.ID_MON_TRUONG
                            where lm.ID_LOP == id_lop && lm.HOC_KY == id_hocKy && lm.TRANG_THAI == true
                            orderby p.THU_TU, p.TEN
                            select new MonHocTruongCauHinhEntity()
                            {
                                DIEM_CHI_TIET = p.DIEM_CHI_TIET,
                                HE_SO = p.HE_SO,
                                ID = p.ID,
                                ID_MON_HOC = p.ID_MON_HOC,
                                ID_NAM_HOC = p.ID_NAM_HOC,
                                ID_TRUONG = p.ID_TRUONG,
                                IS_1 = p.IS_1,
                                IS_10 = p.IS_10,
                                IS_11 = p.IS_11,
                                IS_12 = p.IS_12,
                                IS_2 = p.IS_2,
                                IS_3 = p.IS_3,
                                IS_4 = p.IS_4,
                                IS_5 = p.IS_5,
                                IS_6 = p.IS_6,
                                IS_7 = p.IS_7,
                                IS_8 = p.IS_8,
                                IS_9 = p.IS_9,
                                IS_DELETE = p.IS_DELETE,
                                IS_MON_CHUYEN = p.IS_MON_CHUYEN,
                                KIEU_MON = p.KIEU_MON,
                                MA_CAP_HOC = p.MA_CAP_HOC,
                                NGAY_SUA = p.NGAY_SUA,
                                NGAY_TAO = p.NGAY_TAO,
                                NGUOI_SUA = p.NGUOI_SUA,
                                NGUOI_TAO = p.NGUOI_TAO,
                                TEN = p.TEN,
                                THU_TU = p.THU_TU
                            }).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<MonHocTruongCauHinhEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<LopMonTruongEntity> getLopMonByGiaoVienNamHoc(long id_truong, short id_nam_hoc, short id_hoc_ky, long? id_giao_vien)
        {
            List<LopMonTruongEntity> data = new List<LopMonTruongEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("LOP_MON", "MON_HOC_TRUONG", "LOP", "getLopMonByGiaoVienNamHoc", id_truong, id_nam_hoc, id_hoc_ky, id_giao_vien);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    try
                    {
                        string strQuery = @"select lm.*,m.TEN as TEN_MON_TRUONG,l.TEN as TEN_LOP from lop_mon lm
                            join MON_HOC_TRUONG m on lm.id_mon_truong=m.ID
                            join LOP l on lm.ID_LOP =l.ID
                            where id_giao_vien=:0 and m.ID_TRUONG=:1 and m.id_nam_hoc=:2 and lm.HOC_KY=:3";
                        data = context.Database.SqlQuery<LopMonTruongEntity>(strQuery, id_giao_vien, id_truong, id_nam_hoc, id_hoc_ky).ToList();
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
                    data = QICache.Get(strKeyCache) as List<LopMonTruongEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<DanhSachMonByLopEntity> getMonByLopTruongHocKy(long id_truong, short id_nam_hoc, long id_lop, short hoc_ky)
        {
            List<DanhSachMonByLopEntity> data = new List<DanhSachMonByLopEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("LOP_MON", "MON_HOC_TRUONG", "getMonByLopTruongHocKy", id_truong, id_nam_hoc, id_lop, hoc_ky);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    try
                    {
                        if (hoc_ky == 1 || hoc_ky == 2)
                        {
                            string strQuery = @"SELECT M.ID_MON_TRUONG, MT.TEN, MT.HE_SO, MT.KIEU_MON, m.hoc_ky, case when m.is_mon_chuyen is not null and m.is_mon_chuyen=1 then 1 else 0 end as mon_chuyen
                        FROM LOP_MON M 
                        LEFT JOIN MON_HOC_TRUONG MT ON M.ID_MON_TRUONG = MT.ID AND MT.ID_TRUONG=:0 AND MT.ID_NAM_HOC=:1
                        WHERE M.ID_LOP=:2 AND M.HOC_KY=:3 AND M.TRANG_THAI IS NOT NULL AND M.TRANG_THAI=1 ORDER BY MT.THU_TU";
                            data = context.Database.SqlQuery<DanhSachMonByLopEntity>(strQuery, id_truong, id_nam_hoc, id_lop, hoc_ky).ToList();
                        }
                        else if (hoc_ky == 3)
                        {
                            string strQuery = @"select id_mon_truong, ten, he_so, kieu_mon, hoc_ky, mon_chuyen
                            from (SELECT M.ID_MON_TRUONG, MT.TEN, MT.HE_SO, MT.KIEU_MON, mt.thu_tu, m.hoc_ky, case when m.is_mon_chuyen is not null and m.is_mon_chuyen=1 then 1 else 0 end as mon_chuyen
                            FROM LOP_MON M 
                            LEFT JOIN MON_HOC_TRUONG MT ON M.ID_MON_TRUONG = MT.ID AND MT.ID_TRUONG=:0 AND MT.ID_NAM_HOC=:1
                            WHERE M.ID_LOP=:2 AND M.HOC_KY=2 AND M.TRANG_THAI IS NOT NULL AND M.TRANG_THAI=1
                            union all
                            select M.ID_MON_TRUONG, MT.TEN, MT.HE_SO, MT.KIEU_MON, mt.thu_tu, m.hoc_ky, case when m.is_mon_chuyen is not null and m.is_mon_chuyen=1 then 1 else 0 end as mon_chuyen
                            FROM LOP_MON M 
                            LEFT JOIN MON_HOC_TRUONG MT ON M.ID_MON_TRUONG = MT.ID AND MT.ID_TRUONG=:0 AND MT.ID_NAM_HOC=:1
                            WHERE M.ID_LOP=:2 AND M.HOC_KY=1 AND M.TRANG_THAI IS NOT NULL AND M.TRANG_THAI=1
                            and m.id_mon_truong not in (select lm.id_mon_truong from lop_mon lm where lm.id_lop=:2 
                            and lm.hoc_ky=2 and lm.TRANG_THAI IS NOT NULL AND lm.TRANG_THAI=1)) tbl
                            order by tbl.thu_tu";
                            data = context.Database.SqlQuery<DanhSachMonByLopEntity>(strQuery, id_truong, id_nam_hoc, id_lop).ToList();
                        }
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
                    data = QICache.Get(strKeyCache) as List<DanhSachMonByLopEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<LOP_MON> checkExistsGiaoVienLopMon(long id_giao_vien)
        {
            List<LOP_MON> data = new List<LOP_MON>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("LOP_MON", "checkExistsGiaoVienLopMon", id_giao_vien);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.LOP_MON where p.ID_GIAO_VIEN == id_giao_vien && p.IS_DELETE != true select p).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<LOP_MON>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<LOP_MON> getLopMonByLop(long idLop)
        {
            List<LOP_MON> data = new List<LOP_MON>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("LOP_MON", "getLopMonByLop", idLop);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.LOP_MON where p.ID_LOP == idLop && p.IS_DELETE != true select p).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<LOP_MON>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<LOP_MON> getLopMonByLopHocKy(long idLop, int hoc_ky)
        {
            List<LOP_MON> data = new List<LOP_MON>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("LOP_MON", "getLopMonByLopHocKy", idLop, hoc_ky);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.LOP_MON where p.ID_LOP == idLop && p.HOC_KY == hoc_ky && p.IS_DELETE != true select p).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<LOP_MON>;
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
        public ResultEntity XetMacDinhMonChoLopToanTruong(long id_truong, int ma_nam_hoc, string ma_cap_hoc, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    string sql = string.Format(@"
                            insert into LOP_MON (ID_LOP,ID_MON,HOC_KY,TRANG_THAI,NGAY_TAO,NGUOI_TAO,ID_MON_TRUONG,SO_COT_DIEM_MIENG,SO_COT_DIEM_15P,SO_COT_DIEM_1T_HS1,SO_COT_DIEM_1T_HS2,IS_MON_CHUYEN)
                            select ID_LOP,ID_MON,HOC_KY,TRANG_THAI,NGAY_TAO,NGUOI_TAO,ID_MON_TRUONG,SO_COT_DIEM_MIENG,SO_COT_DIEM_15P,SO_COT_DIEM_1T_HS1,SO_COT_DIEM_1T_HS2,IS_MON_CHUYEN
                            from
                            (
                            select l.ID as ID_LOP,mt.ID_MON_HOC as ID_MON,1 as HOC_KY, 1 as TRANG_THAI,:0 as NGAY_TAO,:1 as NGUOI_TAO,mt.ID as ID_MON_TRUONG
                            ,5 as SO_COT_DIEM_MIENG
                            ,5 as SO_COT_DIEM_15P
                            ,5 as SO_COT_DIEM_1T_HS1
                            ,5 as SO_COT_DIEM_1T_HS2
                            ,0 as IS_MON_CHUYEN
                            from MON_HOC_TRUONG mt
                            join LOP l on mt.ID_TRUONG=l.ID_TRUONG and mt.ID_NAM_HOC=l.ID_NAM_HOC 
                            and case when l.ID_KHOI=1 and mt.IS_1=1 then 1 when l.ID_KHOI=2 and mt.IS_2=1 then 1 when l.ID_KHOI=3 and mt.IS_3=1 then 1 when l.ID_KHOI=4 and mt.IS_4=1 then 1
                                     when l.ID_KHOI=5 and mt.IS_5=1 then 1 when l.ID_KHOI=6 and mt.IS_6=1 then 1 when l.ID_KHOI=7 and mt.IS_7=1 then 1 when l.ID_KHOI=8 and mt.IS_8=1 then 1
                                     when l.ID_KHOI=9 and mt.IS_9=1 then 1 when l.ID_KHOI=10 and mt.IS_10=1 then 1 when l.ID_KHOI=11 and mt.IS_11=1 then 1 when l.ID_KHOI=12 and mt.IS_12=1 then 1
                                     else 0 end =1
                            where mt.ID_TRUONG=:2 and mt.ID_NAM_HOC=:3 and mt.MA_CAP_HOC=:4 
                            and not exists (select * from LOP_MON where ID_LOP= l.ID and ID_MON_TRUONG=mt.ID)
                            union all
                            select l.ID as ID_LOP,mt.ID_MON_HOC as ID_MON,2 as HOC_KY, 1 as TRANG_THAI,:5 as NGAY_TAO,:6 as NGUOI_TAO,mt.ID as ID_MON_TRUONG
                            ,5 as SO_COT_DIEM_MIENG
                            ,5 as SO_COT_DIEM_15P
                            ,5 as SO_COT_DIEM_1T_HS1
                            ,5 as SO_COT_DIEM_1T_HS2
                            ,0 as IS_MON_CHUYEN
                            from MON_HOC_TRUONG mt
                            join LOP l on mt.ID_TRUONG=l.ID_TRUONG and mt.ID_NAM_HOC=l.ID_NAM_HOC 
                            and case when l.ID_KHOI=1 and mt.IS_1=1 then 1 when l.ID_KHOI=2 and mt.IS_2=1 then 1 when l.ID_KHOI=3 and mt.IS_3=1 then 1 when l.ID_KHOI=4 and mt.IS_4=1 then 1
                                     when l.ID_KHOI=5 and mt.IS_5=1 then 1 when l.ID_KHOI=6 and mt.IS_6=1 then 1 when l.ID_KHOI=7 and mt.IS_7=1 then 1 when l.ID_KHOI=8 and mt.IS_8=1 then 1
                                     when l.ID_KHOI=9 and mt.IS_9=1 then 1 when l.ID_KHOI=10 and mt.IS_10=1 then 1 when l.ID_KHOI=11 and mt.IS_11=1 then 1 when l.ID_KHOI=12 and mt.IS_12=1 then 1
                                     else 0 end =1
                            where mt.ID_TRUONG=:7 and mt.ID_NAM_HOC=:8 and mt.MA_CAP_HOC=:9 
                            and not exists (select * from LOP_MON where ID_LOP= l.ID and ID_MON_TRUONG=mt.ID)
                            )
                        ");
                    res.ResObject = context.Database.ExecuteSqlCommand(sql, DateTime.Now, nguoi, id_truong, ma_nam_hoc, ma_cap_hoc, DateTime.Now, nguoi, id_truong, ma_nam_hoc, ma_cap_hoc);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("LOP_MON");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity copyMonLopKy1SangKy2(long? id_lop, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    string sql = string.Format(@"INSERT into lop_mon (id_lop, id_mon, hoc_ky
                        , trang_thai, id_giao_vien, id_mon_truong, so_cot_diem_mieng
                        , SO_COT_DIEM_15P, SO_COT_DIEM_1T_HS1
                        , SO_COT_DIEM_1T_HS2, IS_MON_CHUYEN,NGAY_TAO, NGUOI_TAO)
                        (select id_lop, id_mon, 2 as hoc_ky, trang_thai
                        , id_giao_vien, id_mon_truong, so_cot_diem_mieng
                        , SO_COT_DIEM_15P, SO_COT_DIEM_1T_HS1, SO_COT_DIEM_1T_HS2, IS_MON_CHUYEN
                        ,:0 as ngay_tao, :1 as nguoi_tao
                        from lop_mon a
                        where a.id_lop = :2 and a.hoc_ky = 1 and not EXISTS (select * FROM lop_mon b 
                            where b.id_lop = :3 and b.hoc_ky = 2 
                            and a.id_mon_truong = b.id_mon_truong))
                        ");
                    res.ResObject = context.Database.ExecuteSqlCommand(sql, DateTime.Now, nguoi, id_lop, id_lop);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("LOP_MON");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity copyMonLopKy1SangKy2_capNhatTrangThai(long? id_lop)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    string sql = string.Format(@"update lop_mon set trang_thai = 0
                        where id in (
                        select id from lop_mon where id_lop = :0 and hoc_ky = 2 
                            and exists (select * from lop_mon where id_lop = :1 
                            and hoc_ky = 1 and trang_thai = 0)
                        and id_mon_truong in (select id_mon_truong 
                            from lop_mon where id_lop = :2 
                            and hoc_ky = 1 and trang_thai = 0))");
                    context.Database.ExecuteSqlCommand(sql, id_lop, id_lop, id_lop);
                    string sql1 = string.Format(@"update lop_mon set trang_thai = 1
                        where id in (
                        select id from lop_mon where id_lop = :0 and hoc_ky = 2 
                        and exists (select * from lop_mon where id_lop = :1 
                                and hoc_ky = 1 and trang_thai = 1)
                        and id_mon_truong in (select id_mon_truong 
                                from lop_mon where id_lop = :2 
                                and hoc_ky = 1 and trang_thai = 1))");
                    context.Database.ExecuteSqlCommand(sql1, id_lop, id_lop, id_lop);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("LOP_MON");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        #region update
        public ResultEntity update(LOP_MON detail_in, long? nguoi)
        {
            LOP_MON detail = new LOP_MON();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.LOP_MON
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.TRANG_THAI = detail_in.TRANG_THAI;
                        detail.NGUOI_SUA = nguoi;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.ID_GIAO_VIEN = detail_in.ID_GIAO_VIEN;
                        detail.SO_COT_DIEM_MIENG = detail_in.SO_COT_DIEM_MIENG;
                        detail.SO_COT_DIEM_15P = detail_in.SO_COT_DIEM_15P;
                        detail.SO_COT_DIEM_1T_HS1 = detail_in.SO_COT_DIEM_1T_HS1;
                        detail.SO_COT_DIEM_1T_HS2 = detail_in.SO_COT_DIEM_1T_HS2;
                        detail.IS_MON_CHUYEN = detail_in.IS_MON_CHUYEN;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("LOP_MON");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Sửa " + ex.ToString();// "Có lỗi xãy ra";
            }
            return res;
        }
        #endregion
        #region insert
        public ResultEntity insert(LOP_MON detail_in, long? nguoi)
        {
            LOP_MON detail = new LOP_MON();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail_in.NGUOI_TAO = nguoi;
                    detail_in.NGAY_TAO = DateTime.Now;
                    detail = context.LOP_MON.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("LOP_MON");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Thêm " + ex.ToString();// "Có lỗi xãy ra";
            }
            return res;
        }
        #endregion
        #region delete
        public ResultEntity delete(List<string> arrMa, long? nguoi)
        {
            LOP_MON detail = new LOP_MON();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            string sql = string.Empty;
            try
            {
                if (arrMa != null)
                {
                    using (var context = new oneduEntities())
                    {
                        for (int i = 0; i < arrMa.Count; i++)
                        {
                            sql += @"UPDATE LOP_MON SET IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 WHERE ID = " + arrMa[i].ToString() + " \n";
                        }
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now);
                    }
                    var QICache = new DefaultCacheProvider();
                    QICache.RemoveByFirstName("LOP_MON");
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
        #endregion
    }
}
