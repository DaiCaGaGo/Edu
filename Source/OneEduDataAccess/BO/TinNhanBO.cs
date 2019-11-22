using OneEduDataAccess.Model;
using OneEduDataAccess.Static;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class TinNhanBO
    {
        #region Get
        public List<KhoiHocSinhEntity> thongkeSoLuongTinNhanGuiTrongCacThang(long id_truong, int Ma_Nam_hoc)
        {
            List<KhoiHocSinhEntity> data = new List<KhoiHocSinhEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TIN_NHAN", "thongkeSoLuongTinNhanGuiTrongCacThang", id_truong, Ma_Nam_hoc);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    try
                    {
                        string strQuery = string.Format(@"
                        select sum(SO_TIN) as SOLUONG_HS, THANG_GUI as ID_NAM_HOC from Tin_nhan where Nam_gui = :0 and ID_TRUONG = :1 group by THANG_GUI order by thang_gui");
                        data = context.Database.SqlQuery<KhoiHocSinhEntity>(strQuery, Ma_Nam_hoc, id_truong).ToList();
                        // QICache.Set(strKeyCache, data, 300000);
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
        public long thongKeTongTinGuiQuaCP(string brand_name, string cp_name)
        {
            thongKeTongTinGuiQuaCPEntity data = new thongKeTongTinGuiQuaCPEntity();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TIN_NHAN", "thongKeTongTinGuiQuaCP", brand_name, cp_name);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = @"select 
                                        sum(SO_TIN) as so_tin
                                        from TIN_NHAN where cp='" + cp_name + "' and BRAND_NAME='" + brand_name + "'";
                    data = context.Database.SqlQuery<thongKeTongTinGuiQuaCPEntity>(strQuery).FirstOrDefault();
                    if (data == null || data.so_tin == null)
                    {
                        data = new thongKeTongTinGuiQuaCPEntity();
                        data.so_tin = 0;
                    }
                    //  QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as thongKeTongTinGuiQuaCPEntity;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data.so_tin.Value;
        }

        public long? getTongTinNhanByOtherCondition(long id_truong, short id_nam_hoc, DateTime tu_ngay, DateTime den_ngay, short? trang_thai, short? nha_mang, short? loai_tin_nhan, string noi_dung, string sdt, short? id_khoi, long? id_lop, int? so_tin_nhan)
        {
            long? data = null;
            var QICache = new DefaultCacheProvider();
            //string strKeyCache = QICache.BuildCachedKey("TIN_NHAN", "HOC_SINH", "GIAO_VIEN", "LOP", "getTongTinNhanByOtherCondition", id_truong, id_nam_hoc, tu_ngay, den_ngay, trang_thai, nha_mang, loai_tin_nhan, noi_dung, sdt, id_khoi, id_lop, so_tin_nhan);
            //if (!QICache.IsSet(strKeyCache))
            //{
            using (oneduEntities context = new oneduEntities())
            {
                string strQuery = string.Format(@"select 
                        case when LOAI_NGUOI_NHAN=1 then hs.ho_ten when LOAI_NGUOI_NHAN=2 then gv.HO_TEN else '' END as TEN_NGUOI_NHAN
                        , case when LOAI_NGUOI_NHAN=1 then hs.IS_GUI_BO_ME when LOAI_NGUOI_NHAN=2 then 0 else 0 END as GUI_BO_ME
                        ,l.id as id_lop
                        ,l.TEN as TEN_LOP
                        ,l.id_khoi as id_khoi
                        ,t.*
                        from tin_nhan t
                        left join hoc_sinh hs on t.id_truong = hs.id_truong and hs.id = t.ID_NGUOI_NHAN and t.LOAI_NGUOI_NHAN=1 and hs.id_nam_hoc={0}
                        left join giao_vien gv on gv.id = t.ID_NGUOI_NHAN and t.LOAI_NGUOI_NHAN=2 and gv.id_truong = t.id_truong
                        left join lop l on l.ID_NAM_HOC={0} and l.id_truong={1} and l.id=hs.id_lop and t.LOAI_NGUOI_NHAN=1
                        where THOI_GIAN_GUI>=:0 and THOI_GIAN_GUI<:1 AND t.id_truong={1}", id_nam_hoc, id_truong);
                List<object> parameterList = new List<object>();
                parameterList.Add(tu_ngay);
                parameterList.Add(den_ngay);
                if (trang_thai != null)
                {
                    strQuery += " and t.TRANG_THAI=:" + parameterList.Count;
                    parameterList.Add(trang_thai);
                }
                if (nha_mang != null)
                {
                    if (nha_mang == 1) strQuery += " and t.LOAI_NHA_MANG=N'Viettel'";
                    if (nha_mang == 2) strQuery += " and t.LOAI_NHA_MANG=N'MobiFone'";
                    if (nha_mang == 3) strQuery += " and t.LOAI_NHA_MANG=N'VinaPhone'";
                    if (nha_mang == 4) strQuery += " and t.LOAI_NHA_MANG=N'VietnamMobile'";
                    if (nha_mang == 5) strQuery += " and t.LOAI_NHA_MANG=N'Gmobile'";
                }
                if (loai_tin_nhan != null)
                {
                    strQuery += " and t.LOAI_TIN=:" + parameterList.Count;
                    parameterList.Add(loai_tin_nhan);
                }
                if (!string.IsNullOrEmpty(noi_dung))
                {
                    strQuery += " and lower(t.NOI_DUNG_KHONG_DAU) like N'%' || :" + parameterList.Count + " || N'%'";
                    parameterList.Add(noi_dung.ToLower());
                }
                if (!string.IsNullOrEmpty(sdt))
                {
                    strQuery += " and t.SDT_NHAN like N'%' || :" + parameterList.Count + " || N'%'";
                    parameterList.Add(sdt);
                }
                if (id_khoi != null)
                {
                    strQuery += " and l.id_khoi=:" + parameterList.Count;
                    parameterList.Add(id_khoi);
                }
                if (id_lop != null)
                {
                    strQuery += " and l.id=:" + parameterList.Count;
                    parameterList.Add(id_lop);
                }
                if (so_tin_nhan != null)
                {
                    strQuery += " and t.SO_TIN=:" + parameterList.Count;
                    parameterList.Add(so_tin_nhan);
                }
                strQuery += " order by t.THOI_GIAN_GUI desc";
                var sql = @"select sum(tmp.so_tin) as tong_tin from (" + strQuery + ") tmp";
                data = context.Database.SqlQuery<long?>(sql, parameterList.ToArray()).FirstOrDefault();
                //QICache.Set(strKeyCache, data, 300000);
            }
            //}
            //else
            //{
            //    try
            //    {
            //        data = QICache.Get(strKeyCache) as long?;
            //    }
            //    catch
            //    {
            //        QICache.Invalidate(strKeyCache);
            //    }
            //}
            return data;
        }
        public long? getTongTinNhanByOtherCondition1(long id_truong, short id_nam_hoc, string tu_ngay, string den_ngay, short? trang_thai, short? nha_mang, short? loai_tin_nhan, string noi_dung, string sdt, short? id_khoi, long? id_lop, int? so_tin_nhan, int? loai_nguoi_nhan)
        {
            long? data = null;
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TIN_NHAN", "HOC_SINH", "GIAO_VIEN", "LOP", "getTongTinNhanByOtherCondition1", id_truong, id_nam_hoc, tu_ngay, den_ngay, trang_thai, nha_mang, loai_tin_nhan, noi_dung, sdt, id_khoi, id_lop, so_tin_nhan);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = string.Format(@"select sum(t.so_tin) as tong_tin
                        from tin_nhan t
                        left join hoc_sinh hs on t.id_truong = hs.id_truong and hs.id_nam_hoc={0} and hs.id = t.ID_NGUOI_NHAN and t.LOAI_NGUOI_NHAN=1
                        left join lop l on l.ID_NAM_HOC={0} and l.id_truong={1} and l.id=hs.id_lop and t.LOAI_NGUOI_NHAN=1
                        where  to_char(THOI_GIAN_GUI, 'YYYYMMDD')>={2} and to_char(THOI_GIAN_GUI, 'YYYYMMDD')<{3} AND t.id_truong={1}", id_nam_hoc, id_truong, tu_ngay, den_ngay);
                    string tinNhanCu = string.Format(@"select sum(t.so_tin) as tong_tin
                        from tin_nhan_cu t
                        left join hoc_sinh hs on t.id_truong = hs.id_truong and hs.id_nam_hoc={0} and hs.id = t.ID_NGUOI_NHAN and t.LOAI_NGUOI_NHAN=1
                        left join lop l on l.ID_NAM_HOC={0} and l.id_truong={1} and l.id=hs.id_lop and t.LOAI_NGUOI_NHAN=1
                        where  to_char(THOI_GIAN_GUI, 'YYYYMMDD')>={2} and to_char(THOI_GIAN_GUI, 'YYYYMMDD')<{3} AND t.id_truong={1}", id_nam_hoc, id_truong, tu_ngay, den_ngay);

                    if (trang_thai != null)
                    {
                        strQuery += " and t.TRANG_THAI=" + trang_thai;
                        tinNhanCu += " and t.TRANG_THAI=" + trang_thai;
                    }
                    if (nha_mang != null)
                    {
                        if (nha_mang == 1)
                        {
                            strQuery += " and t.LOAI_NHA_MANG=N'Viettel'";
                            tinNhanCu += " and t.LOAI_NHA_MANG=N'Viettel'";
                        }
                        if (nha_mang == 2)
                        {
                            strQuery += " and t.LOAI_NHA_MANG=N'MobiFone'";
                            tinNhanCu += " and t.LOAI_NHA_MANG=N'MobiFone'";
                        }
                        if (nha_mang == 3)
                        {
                            strQuery += " and t.LOAI_NHA_MANG=N'VinaPhone'";
                            tinNhanCu += " and t.LOAI_NHA_MANG=N'VinaPhone'";
                        }
                        if (nha_mang == 4)
                        {
                            strQuery += " and t.LOAI_NHA_MANG=N'VietnamMobile'";
                            tinNhanCu += " and t.LOAI_NHA_MANG=N'VietnamMobile'";
                        }
                        if (nha_mang == 5)
                        {
                            strQuery += " and t.LOAI_NHA_MANG=N'Gmobile'";
                            tinNhanCu += " and t.LOAI_NHA_MANG=N'Gmobile'";
                        }
                    }
                    if (loai_tin_nhan != null)
                    {
                        strQuery += " and t.LOAI_TIN=" + loai_tin_nhan;
                        tinNhanCu += " and t.LOAI_TIN=" + loai_tin_nhan;
                    }
                    if (!string.IsNullOrEmpty(noi_dung))
                    {
                        strQuery += " and lower(t.NOI_DUNG_KHONG_DAU) like N'%" + noi_dung.ToLower() + "%'";
                        tinNhanCu += " and lower(t.NOI_DUNG_KHONG_DAU) like N'%" + noi_dung.ToLower() + "%'";
                    }
                    if (!string.IsNullOrEmpty(sdt))
                    {
                        strQuery += " and t.SDT_NHAN like N'%" + sdt + "%'";
                        tinNhanCu += " and t.SDT_NHAN like N'%" + sdt + "%'";
                    }
                    if (id_khoi != null)
                    {
                        strQuery += " and l.id_khoi=" + id_khoi;
                        tinNhanCu += " and l.id_khoi=" + id_khoi;
                    }
                    if (id_lop != null)
                    {
                        strQuery += " and l.id=" + id_lop;
                        tinNhanCu += " and l.id=" + id_lop;
                    }
                    if (so_tin_nhan != null)
                    {
                        strQuery += " and t.SO_TIN=" + so_tin_nhan;
                        tinNhanCu += " and t.SO_TIN=" + so_tin_nhan;
                    }
                    if (loai_nguoi_nhan != null)
                    {
                        strQuery += " and t.LOAI_NGUOI_NHAN=" + loai_nguoi_nhan;
                        tinNhanCu += " and t.LOAI_NGUOI_NHAN=" + loai_nguoi_nhan;
                    }

                    //strQuery += " order by t.THOI_GIAN_GUI desc";
                    var sql = @"select sum(tmp.tong_tin) as tong_tin from (" + strQuery + " union all " + tinNhanCu + ") tmp";
                    data = context.Database.SqlQuery<long?>(sql).FirstOrDefault();
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
        
        public List<TinNhanEntity> getTinNhanByKhoiLopNgay(long id_truong, string ma_cap_hoc, short id_nam_hoc, string tu_ngay, string den_ngay, short? trang_thai, short? nha_mang, short? loai_tin_nhan, string noi_dung, string sdt, short? id_khoi, long? id_lop, int? so_tin_nhan, int? loai_nguoi_nhan)
        {
            List<TinNhanEntity> data = new List<TinNhanEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TIN_NHAN", "HOC_SINH", "GIAO_VIEN", "LOP", "getTinNhanByKhoiLopNgay", id_truong, ma_cap_hoc, id_nam_hoc, tu_ngay, den_ngay, trang_thai, nha_mang, loai_tin_nhan, noi_dung, sdt, id_khoi, id_lop, so_tin_nhan, loai_nguoi_nhan);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = string.Format(@"select 
                        case when LOAI_NGUOI_NHAN=1 then hs.ho_ten when LOAI_NGUOI_NHAN=2 then gv.HO_TEN else '' END as TEN_NGUOI_NHAN
                        , case when LOAI_NGUOI_NHAN=1 then hs.IS_GUI_BO_ME when LOAI_NGUOI_NHAN=2 then 0 else 0 END as GUI_BO_ME
                        ,l.id as id_lop
                        ,l.TEN as TEN_LOP
                        ,l.id_khoi as id_khoi,
                        t.ID,
                        t.ID_TRUONG,
                        t.ID_NGUOI_NHAN,
                        t.LOAI_NGUOI_NHAN,
                        t.BRAND_NAME,
                        t.SDT_NHAN,
                        t.MA_GOI_TIN,
                        t.NOI_DUNG,
                        t.NOI_DUNG_KHONG_DAU,
                        t.SO_TIN,
                        t.TRANG_THAI,
                        t.THOI_GIAN_GUI,
                        t.NGUOI_GUI,
                        t.THOI_GIAN_NHAN,
                        t.ID_DOI_TAC,
                        t.LOAI_NHA_MANG,
                        t.KIEU_GUI,
                        t.IS_UNICODE,
                        t.IS_DA_NHAN,
                        t.SEND_NUMBER,
                        t.NGAY_TAO,
                        t.NGUOI_TAO,
                        t.NGAY_SUA,
                        t.NGUOI_SUA,
                        t.ID_NHAN_XET_HANG_NGAY,
                        t.ID_TONG_HOP_NXHN,
                        t.LOAI_TIN,
                        t.ID_THONG_BAO,
                        t.NAM_GUI,
                        t.THANG_GUI,
                        t.TUAN_GUI,
                        t.CP,
                        t.RES_CODE,
                        t.IS_SMS_ZALO_OTP
                        from vw_tin_nhan t
                        left join hoc_sinh hs on t.id_truong = hs.id_truong and hs.id_nam_hoc={0} and hs.id = t.ID_NGUOI_NHAN and t.LOAI_NGUOI_NHAN=1
                        left join giao_vien gv on gv.id_truong = t.id_truong and gv.id = t.ID_NGUOI_NHAN and t.LOAI_NGUOI_NHAN=2
                        left join lop l on l.ID_NAM_HOC={0} and l.id_truong={1} and l.id=hs.id_lop and t.LOAI_NGUOI_NHAN=1
                        where t.id_truong={1} and to_char(THOI_GIAN_GUI, 'YYYYMMDD')>={2} and to_char(THOI_GIAN_GUI, 'YYYYMMDD')<={3}", id_nam_hoc, id_truong, tu_ngay, den_ngay);

                    if (trang_thai != null)
                        strQuery += " and t.TRANG_THAI=" + trang_thai;
                    if (nha_mang != null)
                    {
                        if (nha_mang == 1)
                            strQuery += " and t.LOAI_NHA_MANG=N'Viettel'";
                        if (nha_mang == 2)
                            strQuery += " and t.LOAI_NHA_MANG=N'MobiFone'";
                        if (nha_mang == 3)
                            strQuery += " and t.LOAI_NHA_MANG=N'VinaPhone'";
                        if (nha_mang == 4)
                            strQuery += " and t.LOAI_NHA_MANG=N'VietnamMobile'";
                        if (nha_mang == 5)
                            strQuery += " and t.LOAI_NHA_MANG=N'Gmobile'";
                    }
                    if (loai_tin_nhan != null)
                        strQuery += " and t.LOAI_TIN=" + loai_tin_nhan;
                    if (!string.IsNullOrEmpty(noi_dung))
                        strQuery += " and lower(t.NOI_DUNG_KHONG_DAU) like N'%" + noi_dung.ToLower() + "%'";
                    if (!string.IsNullOrEmpty(sdt))
                        strQuery += " and t.SDT_NHAN like N'%" + sdt + "%'";
                    if (id_khoi != null)
                        strQuery += " and l.id_khoi=" + id_khoi;
                    if (id_lop != null)
                        strQuery += " and l.id=" + id_lop;
                    if (so_tin_nhan != null)
                        strQuery += " and t.SO_TIN=" + so_tin_nhan;
                    if (loai_nguoi_nhan != null)
                        strQuery += " and t.LOAI_NGUOI_NHAN=" + loai_nguoi_nhan;
                    strQuery += " order by t.THOI_GIAN_GUI desc";

                    data = context.Database.SqlQuery<TinNhanEntity>(strQuery).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<TinNhanEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }

        public List<TinNhanEntity> getTinNhanRoot(long? id_truong, string ma_cap_hoc, short id_nam_hoc, string tu_ngay, string den_ngay, short? trang_thai, short? nha_mang, short? loai_tin_nhan, string noi_dung, string sdt, short? id_khoi, long? id_lop, int? so_tin_nhan, int? loai_nguoi_nhan)
        {
            List<TinNhanEntity> data = new List<TinNhanEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TIN_NHAN", "HOC_SINH", "GIAO_VIEN", "LOP", "getTinNhanByKhoiLopNgay1", id_truong, ma_cap_hoc, id_nam_hoc, tu_ngay, den_ngay, trang_thai, nha_mang, loai_tin_nhan, noi_dung, sdt, id_khoi, id_lop, so_tin_nhan, loai_nguoi_nhan);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string sqlTruong = "";
                    if (id_truong != null) sqlTruong = " and l.id_truong=" + id_truong;
                    string strQuery = string.Format(@"select 
                        case when LOAI_NGUOI_NHAN=1 then hs.ho_ten when LOAI_NGUOI_NHAN=2 then gv.HO_TEN else '' END as TEN_NGUOI_NHAN
                        , case when LOAI_NGUOI_NHAN=1 then hs.IS_GUI_BO_ME when LOAI_NGUOI_NHAN=2 then 0 else 0 END as GUI_BO_ME
                        ,l.id as id_lop
                        ,l.TEN as TEN_LOP
                        ,l.id_khoi as id_khoi,
                        t.ID,
                        t.ID_TRUONG,
                        t.ID_NGUOI_NHAN,
                        t.LOAI_NGUOI_NHAN,
                        t.BRAND_NAME,
                        t.SDT_NHAN,
                        t.MA_GOI_TIN,
                        t.NOI_DUNG,
                        t.NOI_DUNG_KHONG_DAU,
                        t.SO_TIN,
                        t.TRANG_THAI,
                        t.THOI_GIAN_GUI,
                        t.NGUOI_GUI,
                        t.THOI_GIAN_NHAN,
                        t.ID_DOI_TAC,
                        t.LOAI_NHA_MANG,
                        t.KIEU_GUI,
                        t.IS_UNICODE,
                        t.IS_DA_NHAN,
                        t.SEND_NUMBER,
                        t.NGAY_TAO,
                        t.NGUOI_TAO,
                        t.NGAY_SUA,
                        t.NGUOI_SUA,
                        t.ID_NHAN_XET_HANG_NGAY,
                        t.ID_TONG_HOP_NXHN,
                        t.LOAI_TIN,
                        t.ID_THONG_BAO,
                        t.NAM_GUI,
                        t.THANG_GUI,
                        t.TUAN_GUI,
                        t.CP,
                        t.RES_CODE,
                        t.IS_SMS_ZALO_OTP
                        from tin_nhan t
                        left join hoc_sinh hs on t.id_truong = hs.id_truong and hs.id_nam_hoc={0} and hs.id = t.ID_NGUOI_NHAN and t.LOAI_NGUOI_NHAN=1
                        left join giao_vien gv on gv.id_truong = t.id_truong and gv.id = t.ID_NGUOI_NHAN and t.LOAI_NGUOI_NHAN=2
                        left join lop l on l.ID_NAM_HOC={0} {1} and l.id=hs.id_lop and t.LOAI_NGUOI_NHAN=1
                        where to_char(THOI_GIAN_GUI, 'YYYYMMDD')>={2} and to_char(THOI_GIAN_GUI, 'YYYYMMDD')<{3}", id_nam_hoc, sqlTruong, tu_ngay, den_ngay);

                    string tinNhanCu = string.Format(@"select 
                        case when LOAI_NGUOI_NHAN=1 then hs.ho_ten when LOAI_NGUOI_NHAN=2 then gv.HO_TEN else '' END as TEN_NGUOI_NHAN
                        , case when LOAI_NGUOI_NHAN=1 then hs.IS_GUI_BO_ME when LOAI_NGUOI_NHAN=2 then 0 else 0 END as GUI_BO_ME
                        ,l.id as id_lop
                        ,l.TEN as TEN_LOP
                        ,l.id_khoi as id_khoi,
                        t.SMS_ID as id,
                        t.ID_TRUONG,
                        t.ID_NGUOI_NHAN,
                        t.LOAI_NGUOI_NHAN,
                        t.BRAND_NAME,
                        t.SDT_NHAN,
                        t.MA_GOI_TIN,
                        t.NOI_DUNG,
                        t.NOI_DUNG_KHONG_DAU,
                        t.SO_TIN,
                        t.TRANG_THAI,
                        t.THOI_GIAN_GUI,
                        t.NGUOI_GUI,
                        t.THOI_GIAN_NHAN,
                        t.ID_DOI_TAC,
                        t.LOAI_NHA_MANG,
                        t.KIEU_GUI,
                        t.IS_UNICODE,
                        t.IS_DA_NHAN,
                        t.SEND_NUMBER,
                        t.NGAY_TAO,
                        t.NGUOI_TAO,
                        t.NGAY_SUA,
                        t.NGUOI_SUA,
                        t.ID_NHAN_XET_HANG_NGAY,
                        t.ID_TONG_HOP_NXHN,
                        t.LOAI_TIN,
                        t.ID_THONG_BAO,
                        t.NAM_GUI,
                        t.THANG_GUI,
                        t.TUAN_GUI,
                        t.CP,
                        t.RES_CODE,
                        t.IS_SMS_ZALO_OTP
                        from tin_nhan_cu t
                        left join hoc_sinh hs on t.id_truong = hs.id_truong and hs.id_nam_hoc={0} and hs.id = t.ID_NGUOI_NHAN and t.LOAI_NGUOI_NHAN=1
                        left join giao_vien gv on gv.id_truong = t.id_truong and gv.id = t.ID_NGUOI_NHAN and t.LOAI_NGUOI_NHAN=2
                        left join lop l on l.ID_NAM_HOC={0} {1} and l.id=hs.id_lop and t.LOAI_NGUOI_NHAN=1
                        where  to_char(THOI_GIAN_GUI, 'YYYYMMDD')>={2} and to_char(THOI_GIAN_GUI, 'YYYYMMDD')<{3}", id_nam_hoc, sqlTruong, tu_ngay, den_ngay);

                    if (id_truong != null)
                    {
                        strQuery += " AND t.id_truong=" + id_truong;
                        tinNhanCu += " and t.id_truong=" + id_truong;
                    }

                    if (trang_thai != null)
                    {
                        if (trang_thai == 0)
                        {
                            strQuery += " and t.TRANG_THAI is null";
                            tinNhanCu += " and t.TRANG_THAI is null";
                        }
                        else
                        {
                            strQuery += " and t.TRANG_THAI=" + trang_thai;
                            tinNhanCu += " and t.TRANG_THAI=" + trang_thai;
                        }
                    }
                    if (nha_mang != null)
                    {
                        if (nha_mang == 1)
                        {
                            strQuery += " and t.LOAI_NHA_MANG=N'Viettel'";
                            tinNhanCu += " and t.LOAI_NHA_MANG=N'Viettel'";
                        }
                        if (nha_mang == 2)
                        {
                            strQuery += " and t.LOAI_NHA_MANG=N'MobiFone'";
                            tinNhanCu += " and t.LOAI_NHA_MANG=N'MobiFone'";
                        }
                        if (nha_mang == 3)
                        {
                            strQuery += " and t.LOAI_NHA_MANG=N'VinaPhone'";
                            tinNhanCu += " and t.LOAI_NHA_MANG=N'VinaPhone'";
                        }
                        if (nha_mang == 4)
                        {
                            strQuery += " and t.LOAI_NHA_MANG=N'VietnamMobile'";
                            tinNhanCu += " and t.LOAI_NHA_MANG=N'VietnamMobile'";
                        }
                        if (nha_mang == 5)
                        {
                            strQuery += " and t.LOAI_NHA_MANG=N'Gmobile'";
                            tinNhanCu += " and t.LOAI_NHA_MANG=N'Gmobile'";
                        }
                    }
                    if (loai_tin_nhan != null)
                    {
                        strQuery += " and t.LOAI_TIN=" + loai_tin_nhan;
                        tinNhanCu += " and t.LOAI_TIN=" + loai_tin_nhan;
                    }
                    if (!string.IsNullOrEmpty(noi_dung))
                    {
                        strQuery += " and lower(t.NOI_DUNG_KHONG_DAU) like N'%" + noi_dung.ToLower() + "%'";
                        tinNhanCu += " and lower(t.NOI_DUNG_KHONG_DAU) like N'%" + noi_dung.ToLower() + "%'";
                    }
                    if (!string.IsNullOrEmpty(sdt))
                    {
                        strQuery += " and t.SDT_NHAN like N'%" + sdt + "%'";
                        tinNhanCu += " and t.SDT_NHAN like N'%" + sdt + "%'";
                    }
                    if (id_khoi != null)
                    {
                        strQuery += " and l.id_khoi=" + id_khoi;
                        tinNhanCu += " and l.id_khoi=" + id_khoi;
                    }
                    if (id_lop != null)
                    {
                        strQuery += " and l.id=" + id_lop;
                        tinNhanCu += " and l.id=" + id_lop;
                    }
                    if (so_tin_nhan != null)
                    {
                        strQuery += " and t.SO_TIN=" + so_tin_nhan;
                        tinNhanCu += " and t.SO_TIN=" + so_tin_nhan;
                    }
                    if (loai_nguoi_nhan != null)
                    {
                        strQuery += " and t.LOAI_NGUOI_NHAN=" + loai_nguoi_nhan;
                        tinNhanCu += " and t.LOAI_NGUOI_NHAN=" + loai_nguoi_nhan;
                    }

                    string query_exc = string.Format(@"select * from (" + strQuery + " union all " + tinNhanCu + ") tbl order by tbl.THOI_GIAN_GUI desc");

                    //strQuery += " order by t.THOI_GIAN_GUI desc";

                    data = context.Database.SqlQuery<TinNhanEntity>(query_exc).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<TinNhanEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public long? getTongTinNhanRoot(long? id_truong, short id_nam_hoc, string tu_ngay, string den_ngay, short? trang_thai, short? nha_mang, short? loai_tin_nhan, string noi_dung, string sdt, short? id_khoi, long? id_lop, int? so_tin_nhan, int? loai_nguoi_nhan)
        {
            long? data = null;
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TIN_NHAN", "HOC_SINH", "GIAO_VIEN", "LOP", "getTongTinNhanByOtherCondition1", id_truong, id_nam_hoc, tu_ngay, den_ngay, trang_thai, nha_mang, loai_tin_nhan, noi_dung, sdt, id_khoi, id_lop, so_tin_nhan);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string sqlTruong = "";
                    if (id_truong != null) sqlTruong = " and l.id_truong=" + id_truong;

                    string strQuery = string.Format(@"select sum(t.so_tin) as tong_tin
                        from tin_nhan t
                        left join hoc_sinh hs on t.id_truong = hs.id_truong and hs.id_nam_hoc={0} and hs.id = t.ID_NGUOI_NHAN and t.LOAI_NGUOI_NHAN=1
                        left join lop l on l.ID_NAM_HOC={0} {1} and l.id=hs.id_lop and t.LOAI_NGUOI_NHAN=1
                        where  to_char(THOI_GIAN_GUI, 'YYYYMMDD')>={2} and to_char(THOI_GIAN_GUI, 'YYYYMMDD')<{3} {4}", id_nam_hoc, sqlTruong, tu_ngay, den_ngay, id_truong != null ? (" and t.id_truong = " + id_truong) : "");
                    string tinNhanCu = string.Format(@"select sum(t.so_tin) as tong_tin
                        from tin_nhan_cu t
                        left join hoc_sinh hs on t.id_truong = hs.id_truong and hs.id_nam_hoc={0} and hs.id = t.ID_NGUOI_NHAN and t.LOAI_NGUOI_NHAN=1
                        left join lop l on l.ID_NAM_HOC={0} {1} and l.id=hs.id_lop and t.LOAI_NGUOI_NHAN=1
                        where  to_char(THOI_GIAN_GUI, 'YYYYMMDD')>={2} and to_char(THOI_GIAN_GUI, 'YYYYMMDD')<{3} {4}", id_nam_hoc, sqlTruong, tu_ngay, den_ngay, id_truong != null ? (" and t.id_truong = " + id_truong) : "");

                    if (trang_thai != null)
                    {
                        if (trang_thai == 0)
                        {
                            strQuery += " and t.TRANG_THAI is null";
                            tinNhanCu += " and t.TRANG_THAI is null";
                        }
                        else
                        {
                            strQuery += " and t.TRANG_THAI=" + trang_thai;
                            tinNhanCu += " and t.TRANG_THAI=" + trang_thai;
                        }
                    }
                    if (nha_mang != null)
                    {
                        if (nha_mang == 1)
                        {
                            strQuery += " and t.LOAI_NHA_MANG=N'Viettel'";
                            tinNhanCu += " and t.LOAI_NHA_MANG=N'Viettel'";
                        }
                        if (nha_mang == 2)
                        {
                            strQuery += " and t.LOAI_NHA_MANG=N'MobiFone'";
                            tinNhanCu += " and t.LOAI_NHA_MANG=N'MobiFone'";
                        }
                        if (nha_mang == 3)
                        {
                            strQuery += " and t.LOAI_NHA_MANG=N'VinaPhone'";
                            tinNhanCu += " and t.LOAI_NHA_MANG=N'VinaPhone'";
                        }
                        if (nha_mang == 4)
                        {
                            strQuery += " and t.LOAI_NHA_MANG=N'VietnamMobile'";
                            tinNhanCu += " and t.LOAI_NHA_MANG=N'VietnamMobile'";
                        }
                        if (nha_mang == 5)
                        {
                            strQuery += " and t.LOAI_NHA_MANG=N'Gmobile'";
                            tinNhanCu += " and t.LOAI_NHA_MANG=N'Gmobile'";
                        }
                    }
                    if (loai_tin_nhan != null)
                    {
                        strQuery += " and t.LOAI_TIN=" + loai_tin_nhan;
                        tinNhanCu += " and t.LOAI_TIN=" + loai_tin_nhan;
                    }
                    if (!string.IsNullOrEmpty(noi_dung))
                    {
                        strQuery += " and lower(t.NOI_DUNG_KHONG_DAU) like N'%" + noi_dung.ToLower() + "%'";
                        tinNhanCu += " and lower(t.NOI_DUNG_KHONG_DAU) like N'%" + noi_dung.ToLower() + "%'";
                    }
                    if (!string.IsNullOrEmpty(sdt))
                    {
                        strQuery += " and t.SDT_NHAN like N'%" + sdt + "%'";
                        tinNhanCu += " and t.SDT_NHAN like N'%" + sdt + "%'";
                    }
                    if (id_khoi != null)
                    {
                        strQuery += " and l.id_khoi=" + id_khoi;
                        tinNhanCu += " and l.id_khoi=" + id_khoi;
                    }
                    if (id_lop != null)
                    {
                        strQuery += " and l.id=" + id_lop;
                        tinNhanCu += " and l.id=" + id_lop;
                    }
                    if (so_tin_nhan != null)
                    {
                        strQuery += " and t.SO_TIN=" + so_tin_nhan;
                        tinNhanCu += " and t.SO_TIN=" + so_tin_nhan;
                    }
                    if (loai_nguoi_nhan != null)
                    {
                        strQuery += " and t.LOAI_NGUOI_NHAN=" + loai_nguoi_nhan;
                        tinNhanCu += " and t.LOAI_NGUOI_NHAN=" + loai_nguoi_nhan;
                    }

                    //strQuery += " order by t.THOI_GIAN_GUI desc";
                    var sql = @"select sum(tmp.tong_tin) as tong_tin from (" + strQuery + " union all " + tinNhanCu + ") tmp";
                    data = context.Database.SqlQuery<long?>(sql).FirstOrDefault();
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


        public List<TinNhanEntity> getTinNhanLoiByTruongNgayGui(long id_truong, string ma_cap_hoc, short id_nam_hoc, DateTime tu_ngay, DateTime den_ngay, short? trang_thai, short? nha_mang, short? loai_tin_nhan, string sdt)
        {
            List<TinNhanEntity> data = new List<TinNhanEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TIN_NHAN", "getTinNhanLoiByTruongNgayGui", id_truong, ma_cap_hoc, id_nam_hoc, tu_ngay, den_ngay, trang_thai, nha_mang, loai_tin_nhan, sdt);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = string.Format(@"SELECT * FROM TIN_NHAN
                        WHERE THOI_GIAN_GUI>=:0 AND THOI_GIAN_GUI<:1 AND ID_TRUONG={0}", id_truong);
                    List<object> parameterList = new List<object>();
                    parameterList.Add(tu_ngay);
                    parameterList.Add(den_ngay);
                    if (trang_thai != null)
                    {
                        strQuery += " and TRANG_THAI=:" + parameterList.Count;
                        parameterList.Add(trang_thai);
                    }
                    if (nha_mang != null)
                    {
                        if (nha_mang == 1) strQuery += " and LOAI_NHA_MANG=N'Viettel'";
                        if (nha_mang == 2) strQuery += " and LOAI_NHA_MANG=N'MobiFone'";
                        if (nha_mang == 3) strQuery += " and LOAI_NHA_MANG=N'VinaPhone'";
                        if (nha_mang == 4) strQuery += " and LOAI_NHA_MANG=N'VietnamMobile'";
                        if (nha_mang == 5) strQuery += " and LOAI_NHA_MANG=N'Gmobile'";
                    }
                    if (loai_tin_nhan != null)
                    {
                        strQuery += " and LOAI_TIN=:" + parameterList.Count;
                        parameterList.Add(loai_tin_nhan);
                    }
                    if (!string.IsNullOrEmpty(sdt))
                    {
                        strQuery += " and SDT_NHAN like N'%' || :" + parameterList.Count + " || N'%'";
                        parameterList.Add(sdt);
                    }
                    strQuery += " order by THOI_GIAN_GUI desc";
                    data = context.Database.SqlQuery<TinNhanEntity>(strQuery, parameterList.ToArray()).ToList();
                    // QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<TinNhanEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public TIN_NHAN checkExistsHocSinhByMaAndSDTAndNoiDung(long? id_nguoi_nhan, short loai_nguoi_nhan, string sdt, short loai_tb, DateTime? henGio, double subMinutes, string noi_dung_khong_dau)
        {
            TIN_NHAN data = new TIN_NHAN();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TIN_NHAN", "checkExistsHocSinhByMaAndSDTAndNoiDung", id_nguoi_nhan, loai_nguoi_nhan, sdt, loai_tb, henGio, subMinutes, noi_dung_khong_dau);
            if (!QICache.IsSet(strKeyCache))
            {
                DateTime den = (henGio != null && henGio.Value > DateTime.MinValue) ? henGio.Value : DateTime.Now;
                DateTime tu = den.AddMinutes(-subMinutes);
                using (var context = new oneduEntities())
                {
                    var tmp = (from p in context.TIN_NHAN
                               where p.LOAI_NGUOI_NHAN == loai_nguoi_nhan && p.SDT_NHAN == sdt && p.LOAI_TIN == loai_tb && p.THOI_GIAN_GUI >= tu && p.THOI_GIAN_GUI <= den
                               select p);
                    if (id_nguoi_nhan != null)
                        tmp = tmp.Where(x => x.ID_NGUOI_NHAN == id_nguoi_nhan);
                    data = tmp.ToList().FirstOrDefault(x => x.NOI_DUNG_KHONG_DAU.ToNormalizeLowerRelaceInBO() == noi_dung_khong_dau.ToNormalizeLowerRelaceInBO());
                    //   QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as TIN_NHAN;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<ThongKeTinNhanEntity> thongKeTheoKhoiLopNgay(long? id_truong, short? id_khoi, long? id_lop, short id_nam_hoc, DateTime tu_ngay, DateTime den_ngay)
        {
            List<ThongKeTinNhanEntity> data = new List<ThongKeTinNhanEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("LOP", "HOC_SINH", "TIN_NHAN", "thongKeTheoKhoiLopNgay", id_truong, id_khoi, id_lop, id_nam_hoc, tu_ngay, den_ngay);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = @"select l.id as id_lop, l.ten as TEN_LOP, l.id_khoi, l.ID_TRUONG
                        ,sum( case when tn.Loai_tin=1 then tn.SO_TIN else 0 end) as TONG_TIN_LL
                        ,sum( case when tn.TRANG_THAI=1 and tn.Loai_tin=1 then tn.SO_TIN else 0 end) as TIN_LL_TC
                        ,sum( case when tn.TRANG_THAI is null and tn.Loai_tin=1 then tn.SO_TIN else 0 end) as TIN_LL_CG
                        ,sum( case when tn.TRANG_THAI=2 and tn.Loai_tin=1 then tn.SO_TIN else 0 end) as TIN_LL_GL
                        ,sum( case when tn.TRANG_THAI=3 and tn.Loai_tin=1 then tn.SO_TIN else 0 end) as TIN_LL_DG
                        ,sum( case when tn.Loai_tin=2 then tn.SO_TIN else 0 end) as TONG_TIN_TB
                        ,sum( case when tn.TRANG_THAI=1 and tn.Loai_tin=2 then tn.SO_TIN else 0 end) as TIN_TB_TC
                        ,sum( case when tn.TRANG_THAI is null and tn.Loai_tin=2 then tn.SO_TIN else 0 end) as TIN_TB_CG
                        ,sum( case when tn.TRANG_THAI=2 and tn.Loai_tin=2 then tn.SO_TIN else 0 end) as TIN_TB_GL
                        ,sum( case when tn.TRANG_THAI=3 and tn.Loai_tin=2 then tn.SO_TIN else 0 end) as TIN_TB_DG
                        from  lop l
                        join hoc_sinh hs on hs.id_lop = l.id and hs.ID_TRUONG=l.ID_TRUONG
                        left join tin_nhan tn on tn.id_nguoi_nhan = hs.id and LOAI_NGUOI_NHAN=1 
                            and trunc(THOI_GIAN_GUI) >= :0 and trunc(THOI_GIAN_GUI) < :1
                        where l.ID_NAM_HOC=:2";
                    List<object> parameterList = new List<object>();
                    parameterList.Add(tu_ngay);
                    parameterList.Add(den_ngay);
                    parameterList.Add(id_nam_hoc);
                    if (id_truong != null)
                    {
                        strQuery += " and l.id_truong=:" + parameterList.Count;
                        parameterList.Add(id_truong);
                    }
                    if (id_khoi != null)
                    {
                        strQuery += " and l.id_khoi=:" + parameterList.Count;
                        parameterList.Add(id_khoi);
                    }
                    if (id_lop != null)
                    {
                        strQuery += " and l.id=:" + parameterList.Count;
                        parameterList.Add(id_lop);
                    }
                    strQuery += " group by l.id, l.ten, L.ID_KHOI, l.ID_TRUONG order by l.ten";
                    data = context.Database.SqlQuery<ThongKeTinNhanEntity>(strQuery, parameterList.ToArray()).ToList();
                    //  QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<ThongKeTinNhanEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }

        public List<ThongKeTinNhanEntity> thongKeTheoKhoiLopNgay1(long? id_truong, short? id_khoi, long? id_lop, short id_nam_hoc, string tu_ngay, string den_ngay)
        {
            List<ThongKeTinNhanEntity> data = new List<ThongKeTinNhanEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("LOP", "HOC_SINH", "TIN_NHAN", "thongKeTheoKhoiLopNgay1", id_truong, id_khoi, id_lop, id_nam_hoc, tu_ngay, den_ngay);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = string.Format(@"select l.id as id_lop, l.ten as TEN_LOP, l.id_khoi, l.ID_TRUONG
                        ,sum( case when tn.Loai_tin=1 then tn.SO_TIN else null end) as TONG_TIN_LL
                        ,sum( case when tn.TRANG_THAI=1 and tn.Loai_tin=1 then tn.SO_TIN else null end) as TIN_LL_TC
                        ,sum( case when tn.TRANG_THAI is null and tn.Loai_tin=1 then tn.SO_TIN else null end) as TIN_LL_CG
                        ,sum( case when tn.TRANG_THAI=2 and tn.Loai_tin=1 then tn.SO_TIN else null end) as TIN_LL_GL
                        ,sum( case when tn.TRANG_THAI=3 and tn.Loai_tin=1 then tn.SO_TIN else null end) as TIN_LL_DG
                        ,sum( case when tn.Loai_tin=2 then tn.SO_TIN else null end) as TONG_TIN_TB
                        ,sum( case when tn.TRANG_THAI=1 and tn.Loai_tin=2 then tn.SO_TIN else null end) as TIN_TB_TC
                        ,sum( case when tn.TRANG_THAI is null and tn.Loai_tin=2 then tn.SO_TIN else null end) as TIN_TB_CG
                        ,sum( case when tn.TRANG_THAI=2 and tn.Loai_tin=2 then tn.SO_TIN else null end) as TIN_TB_GL
                        ,sum( case when tn.TRANG_THAI=3 and tn.Loai_tin=2 then tn.SO_TIN else null end) as TIN_TB_DG
                        from  lop l
                        join hoc_sinh hs on hs.id_lop = l.id and hs.ID_TRUONG=l.ID_TRUONG
                        left join vw_tin_nhan tn on tn.id_nguoi_nhan = hs.id and LOAI_NGUOI_NHAN=1 
                            and to_char(THOI_GIAN_GUI, 'YYYYMMDD') >= {0} and to_char(THOI_GIAN_GUI, 'YYYYMMDD') < {1}
                        where l.ID_NAM_HOC={2}", tu_ngay, den_ngay, id_nam_hoc);
                    if (id_truong != null) strQuery += " and l.id_truong=" + id_truong;
                    if (id_khoi != null) strQuery += " and l.id_khoi=" + id_khoi;
                    if (id_lop != null) strQuery += " and l.id=" + id_lop;
                    strQuery += " group by l.id, l.ten, L.ID_KHOI, l.ID_TRUONG order by l.ten";

                    data = context.Database.SqlQuery<ThongKeTinNhanEntity>(strQuery).ToList();
                    //QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<ThongKeTinNhanEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }

        public List<ThongKeSoTin1LanGuiEntity> thongKeTongSoTin1LanGuiTheoLopNgay(long? id_truong, short? id_khoi, long? id_lop, short id_nam_hoc, DateTime tu_ngay, DateTime den_ngay)
        {
            List<ThongKeSoTin1LanGuiEntity> data = new List<ThongKeSoTin1LanGuiEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("LOP", "HOC_SINH", "TIN_NHAN", "thongKeTongSoTin1LanGuiTheoLopNgay", id_truong, id_khoi, id_lop, id_nam_hoc, tu_ngay, den_ngay);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = @"select l.id as id_lop, l.ten as TEN_LOP, l.id_khoi, l.ID_TRUONG
                        ,sum( case when tn.SO_TIN=1 then tn.SO_TIN else 0 end) as TONG_TIN_1
                        ,sum( case when tn.SO_TIN=2 then tn.SO_TIN else 0 end) as TONG_TIN_2
                        ,sum( case when tn.SO_TIN=3 then tn.SO_TIN else 0 end) as TONG_TIN_3
                        from  lop l
                        join hoc_sinh hs on hs.id_lop = l.id and hs.ID_TRUONG=l.ID_TRUONG
                        left join tin_nhan tn on tn.id_nguoi_nhan = hs.id and LOAI_NGUOI_NHAN=1 
                            and trunc(THOI_GIAN_GUI) >= :0 and trunc(THOI_GIAN_GUI) < :1
                        where l.ID_NAM_HOC=:2";
                    List<object> parameterList = new List<object>();
                    parameterList.Add(tu_ngay);
                    parameterList.Add(den_ngay);
                    parameterList.Add(id_nam_hoc);
                    if (id_truong != null)
                    {
                        strQuery += " and l.id_truong=:" + parameterList.Count;
                        parameterList.Add(id_truong);
                    }
                    if (id_khoi != null)
                    {
                        strQuery += " and l.id_khoi=:" + parameterList.Count;
                        parameterList.Add(id_khoi);
                    }
                    if (id_lop != null)
                    {
                        strQuery += " and l.id=:" + parameterList.Count;
                        parameterList.Add(id_lop);
                    }
                    strQuery += " group by l.id, l.ten, L.ID_KHOI, l.ID_TRUONG order by l.ten";
                    data = context.Database.SqlQuery<ThongKeSoTin1LanGuiEntity>(strQuery, parameterList.ToArray()).ToList();
                    //  QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<ThongKeSoTin1LanGuiEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<ThongKeTinNhanTheoCongTacVienEntity> thongKeTheoCTV(int nam, int thang)
        {
            List<ThongKeTinNhanTheoCongTacVienEntity> data = new List<ThongKeTinNhanTheoCongTacVienEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("NGUOI_DUNG", "TIN_NHAN", "thongKeTheoCTV", nam, thang);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = @"select t.NGUOI_GUI,n.TEN_DANG_NHAP
                    ,count(*) SO_TIN
                    ,count(distinct to_char(t.NGAY_TAO,'DD-MM-yyyy')) as SO_NGAY_GUI
                    ,case when count(distinct to_char(t.NGAY_TAO,'DD-MM-yyyy'))=0 then 0 else count(*)/count(distinct to_char(t.NGAY_TAO,'DD-MM-yyyy')) end as TRUNG_BINH
                    from TIN_NHAN t
                    join NGUOI_DUNG n on t.NGUOI_GUI=n.ID
                    where t.TRANG_THAI=1 and extract(month from t.NGAY_TAO) = :0 and extract(year from t.NGAY_TAO) = :1
                    group by t.NGUOI_GUI,n.TEN_DANG_NHAP";
                    data = context.Database.SqlQuery<ThongKeTinNhanTheoCongTacVienEntity>(strQuery, thang, nam).ToList();
                    //  QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<ThongKeTinNhanTheoCongTacVienEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<ThongKeTinNhanTheoCongTacVienEntity> thongKeTheoCTV1(int nam, int thang)
        {
            List<ThongKeTinNhanTheoCongTacVienEntity> data = new List<ThongKeTinNhanTheoCongTacVienEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("NGUOI_DUNG", "TIN_NHAN", "thongKeTheoCTV1", nam, thang);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = string.Format(@"select a.nguoi_gui, a.ten_dang_nhap
                        ,(SO_TIN + SO_TIN_cu) as SO_TIN 
                        ,(SO_NGAY_GUI + SO_NGAY_GUI_cu) as SO_NGAY_GUI
                        ,case when (SO_NGAY_GUI + SO_NGAY_GUI_cu) = 0 then 0 else (SO_TIN + SO_TIN_cu)/(SO_NGAY_GUI + SO_NGAY_GUI_cu) end as TRUNG_BINH
                        from (
                        select t.NGUOI_GUI,n.TEN_DANG_NHAP
                        ,count(*) SO_TIN
                        ,count(distinct to_char(t.NGAY_TAO,'DD-MM-yyyy')) as SO_NGAY_GUI
                        ,(select count(*) from tin_nhan_cu tc where t.nguoi_gui=tc.nguoi_gui and tc.TRANG_THAI=1 and extract(month from tc.NGAY_TAO) = {0} 
                        and extract(year from tc.NGAY_TAO) = {1}) as so_tin_cu
                        ,(select count(distinct to_char(tc.NGAY_TAO,'DD-MM-yyyy')) from tin_nhan_cu tc where t.nguoi_gui=tc.nguoi_gui and tc.TRANG_THAI=1 and extract(month from tc.NGAY_TAO) = {0} and extract(year from tc.NGAY_TAO) = {1}) as so_ngay_gui_cu
                        from TIN_NHAN t
                        join NGUOI_DUNG n on t.NGUOI_GUI=n.ID
                        where t.TRANG_THAI=1 and extract(month from t.NGAY_TAO) = {0} and extract(year from t.NGAY_TAO) = {1}
                        group by t.NGUOI_GUI,n.TEN_DANG_NHAP
                        ) a ", thang, nam);
                    data = context.Database.SqlQuery<ThongKeTinNhanTheoCongTacVienEntity>(strQuery).ToList();
                    //  QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<ThongKeTinNhanTheoCongTacVienEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }

        public List<TongKeTinNhanTheoDoiTacEntity> thongKeTheoDoiTac(long? id_truong, string telco, string doi_tac, string tu_ngay, string den_ngay)
        {
            DataAccessAPI dataAccessAPI = new DataAccessAPI();
            List<TongKeTinNhanTheoDoiTacEntity> data = new List<TongKeTinNhanTheoDoiTacEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TRUONG", "TIN_NHAN", "thongKeTheoDoiTac", id_truong, telco, doi_tac, tu_ngay, den_ngay);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = string.Format(@"select t.ID_TRUONG
                                        ,case when tr.TEN is null and t.LOAI_NHA_MANG is not null then 'Tổng '|| TO_CHAR(t.LOAI_NHA_MANG)
                                        else  TO_CHAR(tr.TEN) end as TEN
                                        ,t.CP
                                        ,case when t.LOAI_NHA_MANG is null and t.CP is not null then 'Tổng '||TO_CHAR(t.CP)
                                        else TO_CHAR(t.LOAI_NHA_MANG) end as LOAI_NHA_MANG
                                        ,sum(SO_TIN) as SO_TIN
                                        ,case when tr.Ten is null and t.Loai_nha_mang is null then 2
                                        when tr.TEN is null and t.Loai_nha_mang is not null then 1
                                        else 0 end as THU_TU
                                        from VW_TIN_NHAN t
                                        join TRUONG tr on t.ID_TRUONG=tr.ID
                                        where t.TRANG_THAI=1 and to_Char(THOI_GIAN_GUI,'YYYYMMDD') >={0} 
                                        and to_Char(THOI_GIAN_GUI,'YYYYMMDD') <= {1}", tu_ngay, den_ngay);
                    if (id_truong != null)
                        strQuery += " and t.id_truong=" + id_truong.ToString();
                    if (!string.IsNullOrEmpty(telco))
                        strQuery += " and t.LOAI_NHA_MANG='" + dataAccessAPI.sqlRemoveInjection(telco) + "'";
                    if (!string.IsNullOrEmpty(doi_tac))
                        strQuery += " and t.CP='" + dataAccessAPI.sqlRemoveInjection(doi_tac) + "'";

                    strQuery += @" group by GROUPING SETS((t.ID_TRUONG,tr.TEN,t.LOAI_NHA_MANG,t.CP),(t.LOAI_NHA_MANG,t.CP),(t.cp)) order by cp, thu_tu, loai_nha_mang";

                    data = context.Database.SqlQuery<TongKeTinNhanTheoDoiTacEntity>(strQuery).ToList();
                    //  QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<TongKeTinNhanTheoDoiTacEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }

        public List<ThongKeTinNhanTheoThuongHieuEntity> thongKeTheoThuongHieu1(string brand_name, string telco, string doi_tac, string tu_ngay, string den_ngay)
        {
            DataAccessAPI dataAccessAPI = new DataAccessAPI();
            List<ThongKeTinNhanTheoThuongHieuEntity> data = new List<ThongKeTinNhanTheoThuongHieuEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TRUONG", "TIN_NHAN", "thongKeTheoThuongHieu1", brand_name, telco, doi_tac, tu_ngay, den_ngay);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    
                    string strQuery = string.Format(@"select t.ID_TRUONG,
                                        t.brand_name,
                                        case when t.cp is null and t.brand_name is not null then 'Tổng ' || to_char(t.brand_name) else to_char(t.cp) end as cp,
                                        case when t.loai_nha_mang is null and t.CP is not null then 'Tổng ' || to_char(t.cp) else to_char(t.loai_nha_mang) end as loai_nha_mang,
                                        sum(so_Tin) as so_tin,
                                        case when t.loai_nha_mang is null and t.cp is null then 2
                                        when t.loai_nha_mang is null and t.cp is not null then 1
                                        else 0 end thu_tu
                                        from vw_tin_nhan t
                                        join TRUONG tr on t.ID_TRUONG=tr.ID
                                        where t.trang_thai=1 and to_Char(THOI_GIAN_GUI,'YYYYMMDD')>={0} and to_Char(THOI_GIAN_GUI,'YYYYMMDD')<={1}", tu_ngay, den_ngay);

                    if (!string.IsNullOrEmpty(brand_name)) strQuery += " and t.brand_name='" + brand_name + "'";

                    if (!string.IsNullOrEmpty(telco))
                        strQuery += " and t.LOAI_NHA_MANG='" + dataAccessAPI.sqlRemoveInjection(telco) + "'";

                    if (!string.IsNullOrEmpty(doi_tac))
                        strQuery += " and t.CP='" + dataAccessAPI.sqlRemoveInjection(doi_tac) + "'";

                    strQuery += @" group by grouping sets(
                        (t.ID_TRUONG,t.brand_name,CP, t.loai_nha_mang),
                        (t.brand_name,t.cp),
                        (t.brand_name))";

                    data = context.Database.SqlQuery<ThongKeTinNhanTheoThuongHieuEntity>(strQuery).ToList();
                    //  QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<ThongKeTinNhanTheoThuongHieuEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<ThongKeTinNhanTheoThuongHieuEntity> thongKeTheoThuongHieu(string brand_name, string telco, string doi_tac, string tu_ngay, string den_ngay)
        {
            DataAccessAPI dataAccessAPI = new DataAccessAPI();
            List<ThongKeTinNhanTheoThuongHieuEntity> data = new List<ThongKeTinNhanTheoThuongHieuEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TRUONG", "TIN_NHAN", "thongKeTheoThuongHieu", brand_name, telco, doi_tac, tu_ngay, den_ngay);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {

                    string strQuery = string.Format(@"select case when t.cp is null  AND T.LOAI_NHA_MANG IS NULL then 'Tổng ' || t.brand_name
                                        else t.brand_name end as brand_name,
                                        t.cp,
                                        t.loai_nha_mang,
                                        sum(so_Tin) as so_tin,
                                        case when t.cp is null and t.brand_name is not null then 1
                                        else 0 end thu_tu
                                        from vw_tin_nhan t
                                        join TRUONG tr on t.ID_TRUONG=tr.ID
                                        where t.trang_thai=1 and to_Char(THOI_GIAN_GUI,'YYYYMMDD')>={0} and to_Char(THOI_GIAN_GUI,'YYYYMMDD')<={1}", tu_ngay, den_ngay);

                    if (!string.IsNullOrEmpty(brand_name)) strQuery += " and t.brand_name='" + brand_name + "'";

                    if (!string.IsNullOrEmpty(telco))
                        strQuery += " and t.LOAI_NHA_MANG='" + dataAccessAPI.sqlRemoveInjection(telco) + "'";

                    if (!string.IsNullOrEmpty(doi_tac))
                        strQuery += " and t.CP='" + dataAccessAPI.sqlRemoveInjection(doi_tac) + "'";

                    strQuery += @" group by grouping sets((t.brand_name, CP, t.loai_nha_mang),(t.brand_name))
                                   order by t.BRAND_NAME, thu_tu, CP, t.loai_nha_mang";

                    data = context.Database.SqlQuery<ThongKeTinNhanTheoThuongHieuEntity>(strQuery).ToList();
                    //  QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<ThongKeTinNhanTheoThuongHieuEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<ThongKeTinNhanTheoThuongHieuEntity> doiSoatTinTheoNgay(long? id_truong, string tu_ngay, string den_ngay)
        {
            DataAccessAPI dataAccessAPI = new DataAccessAPI();
            List<ThongKeTinNhanTheoThuongHieuEntity> data = new List<ThongKeTinNhanTheoThuongHieuEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TRUONG", "TIN_NHAN", "TIN_NHAN_CU", "doiSoatTinTheoNgay", id_truong, tu_ngay, den_ngay);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = String.Format(@"select 
                         t.ID_TRUONG,
                         casE when t.CP is null AND T.LOAI_NHA_MANG IS NULL then 'Tổng ' || TR.TEN
                         ELSE TR.TEN END AS TEN_TRUONG ,
                         t.BRAND_NAME,
                         T.CP,
                         loai_nha_mang,
                         sum(so_Tin) as so_tin ,
                         casE when t.CP is null AND T.LOAI_NHA_MANG IS NULL then 1
                         ELSE 0 END AS thu_tu 
                          from vw_tin_nhan t
                          join TRUONG tr
                            on t.ID_TRUONG = tr.ID
                         where t.trang_thai = 1
                           and to_char(THOI_GIAN_GUI, 'YYYYMMDD') >= {0}
                           and to_char(THOI_GIAN_GUI, 'YYYYMMDD') <= {1}
                         ", tu_ngay, den_ngay);
                    if (id_truong != null) strQuery += " and t.id_truong = " + id_truong;

                    strQuery += @" group by grouping sets ((t.id_truong, tr.ten, t.BRAND_NAME, T.CP, loai_nha_mang), (T.ID_TRUONG, tr.ten))
                         order by t.id_truong, t.BRAND_NAME, thu_tu, CP, t.loai_nha_mang";
                    data = context.Database.SqlQuery<ThongKeTinNhanTheoThuongHieuEntity>(strQuery).ToList();
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<ThongKeTinNhanTheoThuongHieuEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public TIN_NHAN getTinNhanByID(long id)
        {
            TIN_NHAN data = new TIN_NHAN();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TIN_NHAN", "getTinNhanByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.TIN_NHAN where p.ID == id select p).FirstOrDefault();
                    //  QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as TIN_NHAN;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        #region Using tool gửi tin
        public List<TIN_NHAN> getTinNhanChoGui(string cp, int total_thread, int index_thread, int max_record = 500)
        {
            List<TIN_NHAN> data = new List<TIN_NHAN>();
            using (oneduEntities context = new oneduEntities())
            {
                data = (from p in context.TIN_NHAN
                        where !(p.TRANG_THAI != null && p.TRANG_THAI != 2) && p.CP == cp && p.THOI_GIAN_GUI != null && p.THOI_GIAN_GUI <= DateTime.Now && p.ID % total_thread == index_thread
                        select p).Take(max_record).ToList();
            }
            return data;
        }
        #endregion
        public List<TongHopSoTinTheoTruongNgayEntity> tongTinDaGuiTheoTruongAndNgay(long? id_truong, string tu_ngay, string den_ngay)
        {
            List<TongHopSoTinTheoTruongNgayEntity> data = new List<TongHopSoTinTheoTruongNgayEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TIN_NHAN", "TRUONG", "tongTinDaGuiTheoTruongAndNgay", id_truong, tu_ngay, den_ngay);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    try
                    {
                        string strQuery = string.Format(@"select tn.id_truong,t.ten,sum(SO_TIN) as tong_tin,
                        (select sum(so_tin) from TIN_NHAN t1 where t1.id_truong=tn.id_truong and to_char(t1.THOI_GIAN_GUI, 'YYYYMMDD') >= {0} and to_char(t1.THOI_GIAN_GUI, 'YYYYMMDD') <= {1} and trang_thai=1) as tin_thanh_cong,
                        (select sum(so_tin) from TIN_NHAN t1 where t1.id_truong=tn.id_truong and to_char(t1.THOI_GIAN_GUI, 'YYYYMMDD') >= {0} and to_char(t1.THOI_GIAN_GUI, 'YYYYMMDD') <= {1} and trang_thai=2) as tin_gui_loi,
                        (select sum(so_tin) from TIN_NHAN t1 where t1.id_truong=tn.id_truong and to_char(t1.THOI_GIAN_GUI, 'YYYYMMDD') >= {0}  and to_char(t1.THOI_GIAN_GUI, 'YYYYMMDD') <= {1} and trang_thai=3) as tin_dung_gui
                        from TIN_NHAN tn
                        join truong t on t.id=tn.id_truong where to_char(THOI_GIAN_GUI, 'YYYYMMDD') >= {0} and to_char(THOI_GIAN_GUI, 'YYYYMMDD') <= {1}"
                        , tu_ngay, den_ngay);
                        if (id_truong != null)
                            strQuery += string.Format(@" and tn.id_truong={0},t.id", id_truong);
                        strQuery += @" group by tn.id_truong,t.ten,t.trang_thai,t.id order by tong_tin desc, t.Ten";

                        data = context.Database.SqlQuery<TongHopSoTinTheoTruongNgayEntity>(strQuery).ToList();

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
                    data = QICache.Get(strKeyCache) as List<TongHopSoTinTheoTruongNgayEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public int? checkTinNhanXacNhanDangKyZalo(string sdt_nhan_tin, string ngay_gui)
        {
            int? data = null;
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TIN_NHAN", "checkTinNhanXacNhanDangKyZalo", sdt_nhan_tin, ngay_gui);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = string.Format(@"select count(ID) as count from tin_nhan where id_truong=1 and TO_Char(THOI_GIAN_GUI, 'DD/MM/YYYY') = '{0}' and (IS_SMS_ZALO_OTP is not null and IS_SMS_ZALO_OTP=1) and SDT_NHAN='{1}'", ngay_gui, sdt_nhan_tin);
                    data = context.Database.SqlQuery<int?>(strQuery).FirstOrDefault();
                    //  QICache.Set(strKeyCache, data, 300000);
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
        #endregion
        #region Set
        public ResultEntity insert(TIN_NHAN detail_in, long? nguoi)
        {
            QuyTinBO quyTinBO = new QuyTinBO();
            ResultEntity res = new ResultEntity();
            var QICache = new DefaultCacheProvider();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail_in.NGAY_TAO = DateTime.Now;
                    detail_in.NGUOI_TAO = nguoi;
                    detail_in = context.TIN_NHAN.Add(detail_in);
                    context.SaveChanges();
                    #region update trang thai gui tin nhan xet hang ngay
                    if (detail_in.ID_NHAN_XET_HANG_NGAY != null)
                    {
                        string str = @"update NHAN_XET_HANG_NGAY set IS_SEND = 1 where ID = :0";
                        context.Database.ExecuteSqlCommand(str, detail_in.ID_NHAN_XET_HANG_NGAY);
                        QICache.RemoveByFirstName("NHAN_XET_HANG_NGAY");
                    }
                    #endregion
                    #region update quỹ tin
                    TruongBO truongBO = new TruongBO();
                    TRUONG detailTruong = new TRUONG();
                    detailTruong = truongBO.getTruongById(detail_in.ID_TRUONG);
                    if (detailTruong.ID_DOI_TAC != null || detailTruong.ID_DOI_TAC > 0)
                    {
                        ResultEntity res1 = truongBO.updateQuyTinDoiTacCap(detail_in.ID_TRUONG, 0, detail_in.SO_TIN.Value, nguoi);
                    }
                    else
                    {
                        if (detail_in.TRANG_THAI == null)
                        {
                            short nam_hoc = detail_in.NAM_GUI;
                            short thang = detail_in.THANG_GUI;
                            short? tuan = detail_in.TUAN_GUI;
                            ResultEntity res1 = quyTinBO.cap_nhat_quy_tin(nam_hoc, thang,
                                detail_in.ID_TRUONG, detail_in.LOAI_TIN, nguoi, null, detail_in.SO_TIN == null ? 0 : detail_in.SO_TIN.Value);
                        }
                    }
                    #endregion
                }
                res.ResObject = detail_in;
                QICache.RemoveByFirstName("TIN_NHAN");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insertTinNhanTongHopHangNgay(List<TIN_NHAN> lst_tin_nhan, long id_truong, short nam_gui, short thang_gui, long? nguoi)
        {
            QuyTinBO quyTinBO = new QuyTinBO();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    long tin_moi_ll = 0;
                    long tin_moi_tb = 0;
                    string strUpdateNXHN = ""; string strUpdateTongHopNXHN = "";
                    foreach (var item in lst_tin_nhan)
                    {
                        item.NGAY_TAO = DateTime.Now;
                        item.NGUOI_TAO = nguoi;
                        if (item.LOAI_TIN == SYS_Loai_Tin.Tin_Lien_Lac)
                            tin_moi_ll += item.SO_TIN == null ? 0 : item.SO_TIN.Value;
                        if (item.LOAI_TIN == SYS_Loai_Tin.Tin_Thong_Bao)
                            tin_moi_tb += item.SO_TIN == null ? 0 : item.SO_TIN.Value;
                        item.ID = context.Database.SqlQuery<long>("SELECT TIN_NHAN_SEQ1.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                        #region "update trang thai gui nhan xet hang ngay"
                        if (item.ID_NHAN_XET_HANG_NGAY != null && item.ID_NHAN_XET_HANG_NGAY > 0)
                        {
                            strUpdateNXHN = string.Format(@"update NHAN_XET_HANG_NGAY set IS_SEND = 1 where ID = {0}", item.ID_NHAN_XET_HANG_NGAY);
                            context.Database.ExecuteSqlCommand(strUpdateNXHN);
                        }
                        #endregion
                        #region "update trang thai gui tong hop nhan xet hang ngay"
                        if (item.ID_TONG_HOP_NXHN != null && item.ID_TONG_HOP_NXHN > 0)
                        {
                            strUpdateTongHopNXHN = string.Format(@"update TONG_HOP_NHAN_XET_HANG_NGAY set IS_SEND = 1 where ID = {0}", item.ID_TONG_HOP_NXHN);
                            context.Database.ExecuteSqlCommand(strUpdateTongHopNXHN);
                        }
                        #endregion
                    }

                    #region "update quỹ tin trường thuộc đối tác"
                    TruongBO truongBO = new TruongBO();
                    TRUONG detailTruong = new TRUONG();
                    detailTruong = truongBO.getTruongById(id_truong);
                    if (detailTruong.ID_DOI_TAC != null || detailTruong.ID_DOI_TAC > 0)
                    {
                        res = truongBO.updateQuyTinDoiTacCap(id_truong, 0, tin_moi_ll + tin_moi_tb, nguoi);
                    }
                    else
                    {
                        #region update quỹ tin
                        if (tin_moi_ll > 0)
                        {
                            res = quyTinBO.cap_nhat_quy_tin(nam_gui, thang_gui,
                                id_truong, SYS_Loai_Tin.Tin_Lien_Lac, nguoi, null, tin_moi_ll);
                        }
                        if (tin_moi_tb > 0)
                        {
                            res = quyTinBO.cap_nhat_quy_tin(nam_gui, thang_gui,
                                id_truong, SYS_Loai_Tin.Tin_Thong_Bao, nguoi, null, tin_moi_tb);
                        }
                        #endregion
                    }
                    #endregion
                    #region Save tin nhắn
                    if (res.Res)
                    {
                        context.TIN_NHAN.AddRange(lst_tin_nhan);
                        context.SaveChanges();
                    }
                    #endregion
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("TIN_NHAN");
                QICache.RemoveByFirstName("NHAN_XET_HANG_NGAY");
                QICache.RemoveByFirstName("TONG_HOP_NHAN_XET_HANG_NGAY");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi khi lưu dữ liệu. Liên hệ với quản trị viên";
            }
            return res;
        }
        public ResultEntity insertTinNhanThongBao(List<TIN_NHAN> lst_tin_nhan, long id_truong, short nam_gui, short thang_gui, short tuan_gui, long? nguoi)
        {
            QuyTinBO quyTinBO = new QuyTinBO();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    long tin_moi_ll = 0;
                    long tin_moi_tb = 0;

                    foreach (var item in lst_tin_nhan)
                    {
                        item.NGAY_TAO = DateTime.Now;
                        item.NGUOI_TAO = nguoi;
                        if (item.LOAI_TIN == SYS_Loai_Tin.Tin_Lien_Lac)
                            tin_moi_ll += item.SO_TIN == null ? 0 : item.SO_TIN.Value;
                        if (item.LOAI_TIN == SYS_Loai_Tin.Tin_Thong_Bao)
                            tin_moi_tb += item.SO_TIN == null ? 0 : item.SO_TIN.Value;

                        item.ID = context.Database.SqlQuery<long>("SELECT TIN_NHAN_SEQ1.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                    }

                    #region "update quy tin truong doi tac"
                    TruongBO truongBO = new TruongBO();
                    TRUONG detailTruong = new TRUONG();
                    detailTruong = truongBO.getTruongById(id_truong);
                    if (detailTruong.ID_DOI_TAC != null || detailTruong.ID_DOI_TAC > 0)
                    {
                        res = truongBO.updateQuyTinDoiTacCap(id_truong, 0, tin_moi_ll + tin_moi_tb, nguoi);
                    }
                    else
                    {
                        #region update quỹ tin
                        if (tin_moi_ll > 0)
                        {
                            res = quyTinBO.cap_nhat_quy_tin(nam_gui, thang_gui,
                                id_truong, SYS_Loai_Tin.Tin_Lien_Lac, nguoi, null, tin_moi_ll);
                        }
                        if (tin_moi_tb > 0)
                        {
                            res = quyTinBO.cap_nhat_quy_tin(nam_gui, thang_gui,
                                id_truong, SYS_Loai_Tin.Tin_Thong_Bao, nguoi, null, tin_moi_tb);
                        }
                        #endregion
                    }
                    #endregion
                    #region save tin nhắn
                    if (res.Res)
                    {
                        context.TIN_NHAN.AddRange(lst_tin_nhan);
                        context.SaveChanges();
                    }
                    #endregion
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("TIN_NHAN");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi khi lưu dữ liệu. Liên hệ với quản trị viên";
            }
            return res;
        }
        public ResultEntity insertTinNXHN(List<TIN_NHAN> lst_tin_nhan, long id_truong, short nam_gui, short thang_gui, long? nguoi, int tong_tin, string strNXHN_ID)
        {
            QuyTinBO quyTinBO = new QuyTinBO();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            var QICache = new DefaultCacheProvider();
            try
            {
                using (var context = new oneduEntities())
                {
                    foreach (var item in lst_tin_nhan)
                    {
                        item.NGAY_TAO = DateTime.Now;
                        item.NGUOI_TAO = nguoi;
                        item.ID = context.Database.SqlQuery<long>("SELECT TIN_NHAN_SEQ1.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                    }
                    #region "update quy tin truong"
                    TruongBO truongBO = new TruongBO();
                    TRUONG detailTruong = new TRUONG();
                    detailTruong = truongBO.getTruongById(id_truong);
                    if (detailTruong.ID_DOI_TAC != null || detailTruong.ID_DOI_TAC > 0)
                    {
                        res = truongBO.updateQuyTinDoiTacCap(id_truong, 0, tong_tin, nguoi);
                    }
                    else
                    {
                        res = quyTinBO.cap_nhat_quy_tin(nam_gui, thang_gui,
                                id_truong, SYS_Loai_Tin.Tin_Lien_Lac, nguoi, null, tong_tin);
                    }
                    #endregion
                    #region save tin nhắn
                    if (res.Res)
                    {
                        context.TIN_NHAN.AddRange(lst_tin_nhan);
                        context.SaveChanges();

                        #region update trang thai gui NXHN
                        string updateNXHN = string.Format(@"update NHAN_XET_HANG_NGAY set IS_SEND = 1 where ID in {0}", strNXHN_ID);
                        context.Database.ExecuteSqlCommand(updateNXHN);
                        QICache.RemoveByFirstName("NHAN_XET_HANG_NGAY");
                        #endregion
                    }
                    #endregion
                }
                QICache.RemoveByFirstName("TIN_NHAN");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi khi lưu dữ liệu. Liên hệ với quản trị viên";
            }
            return res;
        }
        public ResultEntity updateGuiLaiTin(long id, long? id_truong, long? nguoi)
        {
            QuyTinBO quyTinBO = new QuyTinBO();
            TruongBO truongBO = new TruongBO();
            TIN_NHAN detail = new TIN_NHAN();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {

                    if (id_truong == null)
                        detail = (from p in context.TIN_NHAN where p.ID == id select p).FirstOrDefault();
                    else detail = (from p in context.TIN_NHAN where p.ID == id && p.ID_TRUONG == id_truong.Value select p).FirstOrDefault();


                    if (detail != null && detail.TRANG_THAI != 1)
                    {
                        string noi_dung = chuyenTiengVietKhongDau(detail.NOI_DUNG_KHONG_DAU);
                        TRUONG detailTruong = new TRUONG();
                        detailTruong = truongBO.getTruongById(detail.ID_TRUONG);

                        //string loai_nha_mang = detail.LOAI_NHA_MANG;
                        string phone = detail.SDT_NHAN;
                        string loai_nha_mang = getLoaiNhaMang(phone);
                        detail.LOAI_NHA_MANG = loai_nha_mang;
                        if (loai_nha_mang == "Viettel")
                        {
                            detail.BRAND_NAME = detailTruong.BRAND_NAME_VIETTEL;
                            detail.CP = detailTruong.CP_VIETTEL;
                        }
                        else if (loai_nha_mang == "GMobile")
                        {
                            detail.BRAND_NAME = detailTruong.BRAND_NAME_GTEL;
                            detail.CP = detailTruong.CP_GTEL;
                        }
                        else if (loai_nha_mang == "MobiFone")
                        {
                            detail.BRAND_NAME = detailTruong.BRAND_NAME_MOBI;
                            detail.CP = detailTruong.CP_MOBI;
                        }
                        else if (loai_nha_mang == "VinaPhone")
                        {
                            detail.BRAND_NAME = detailTruong.BRAND_NAME_VINA;
                            detail.CP = detailTruong.CP_VINA;
                        }
                        else if (loai_nha_mang == "VietnamMobile")
                        {
                            detail.BRAND_NAME = detailTruong.BRAND_NAME_VNM;
                            detail.CP = detailTruong.CP_VNM;
                        }
                        #region Save tin nhắn
                        if (res.Res)
                        {
                            detail.NOI_DUNG_KHONG_DAU = noi_dung;
                            detail.TRANG_THAI = null;
                            detail.NGAY_SUA = DateTime.Now;
                            detail.NGUOI_SUA = nguoi;
                            context.SaveChanges();
                            res.ResObject = detail;
                        }
                        #endregion
                        #region update quỹ tin
                        if (detailTruong.ID_DOI_TAC != null || detailTruong.ID_DOI_TAC > 0)
                        {
                            ResultEntity res1 = truongBO.updateQuyTinDoiTacCap(detail.ID_TRUONG, 0, detail.SO_TIN.Value, nguoi);
                        }
                        else
                        {
                            if (detail.TRANG_THAI == null)
                            {
                                short nam_hoc = detail.NAM_GUI;
                                short thang = detail.THANG_GUI;
                                short? tuan = detail.TUAN_GUI;
                                ResultEntity res1 = quyTinBO.cap_nhat_quy_tin(nam_hoc, thang,
                                    detail.ID_TRUONG, detail.LOAI_TIN, nguoi, null, detail.SO_TIN == null ? 0 : detail.SO_TIN.Value);
                            }
                        }
                        #endregion
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("TIN_NHAN");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity doiCPViettel(long id_truong, string cpFrom, string cpTo, string brandname)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    string sql1 = @"update TRUONG set CP_VIETTEL=:0 where ID=:1";
                    context.Database.ExecuteSqlCommand(sql1, cpTo, id_truong);
                    string sql2 = @"update TIN_NHAN set CP=:0 where ID_TRUONG=:1 and BRAND_NAME=:2 and CP=:3 and loai_nha_mang='Viettel' and (TRANG_THAI is null or TRANG_THAI=2)";
                    context.Database.ExecuteSqlCommand(sql2, cpTo, id_truong, brandname, cpFrom);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("TRUONG");
                QICache.RemoveByFirstName("TIN_NHAN");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity updateTrangThaiTinNhan(long id, long? id_truong, short trang_thai, string res_code, long? nguoi, bool is_update_by_tool = false)
        {
            QuyTinBO quyTinBO = new QuyTinBO();
            TIN_NHAN detail = new TIN_NHAN();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    if (id_truong == null)
                        detail = (from p in context.TIN_NHAN where p.ID == id select p).FirstOrDefault();
                    else detail = (from p in context.TIN_NHAN where p.ID == id && p.ID_TRUONG == id_truong select p).FirstOrDefault();
                    if (detail != null && detail.TRANG_THAI != 1)
                    {
                        #region "hoàn quỹ tin"
                        if ((detail.TRANG_THAI == 2 || detail.TRANG_THAI == null) && trang_thai == 3)
                        {
                            TruongBO truongBO = new TruongBO();
                            TRUONG detailTruong = new TRUONG();
                            detailTruong = truongBO.getTruongById(detail.ID_TRUONG);
                            if (detailTruong.ID_DOI_TAC != null || detailTruong.ID_DOI_TAC > 0)
                            {
                                res = truongBO.updateQuyTinDoiTacCap(detail.ID_TRUONG, 0, (-1) * detail.SO_TIN.Value, nguoi);
                            }
                            else
                            {
                                res = quyTinBO.cap_nhat_quy_tin(detail.NAM_GUI, detail.THANG_GUI,
                                 detail.ID_TRUONG, detail.LOAI_TIN, nguoi, null, (-1) * detail.SO_TIN.Value);
                            }
                        }
                        #endregion
                        #region Save tin nhắn
                        if (res.Res)
                        {
                            detail.TRANG_THAI = trang_thai;
                            if (is_update_by_tool)
                            {
                                detail.RES_CODE = res_code;
                                short send_number = 0;
                                try { send_number = detail.SEND_NUMBER.Value; } catch { }
                                detail.SEND_NUMBER = Convert.ToInt16(send_number + 1);
                                if (trang_thai == 1)
                                {
                                    detail.THOI_GIAN_NHAN = DateTime.Now;
                                    detail.IS_DA_NHAN = true;
                                }
                                else
                                {
                                    detail.THOI_GIAN_NHAN = null;
                                    detail.IS_DA_NHAN = false;
                                }
                            }
                            detail.NGAY_SUA = DateTime.Now;
                            detail.NGUOI_SUA = nguoi;
                            context.SaveChanges();
                            res.ResObject = detail;
                        }
                        #endregion
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("TIN_NHAN");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insertTinXacNhanZalo(TIN_NHAN detail_in, long? nguoi)
        {
            QuyTinBO quyTinBO = new QuyTinBO();
            ResultEntity res = new ResultEntity();
            var QICache = new DefaultCacheProvider();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail_in.NGAY_TAO = DateTime.Now;
                    detail_in.NGUOI_TAO = nguoi;
                    detail_in = context.TIN_NHAN.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                QICache.RemoveByFirstName("TIN_NHAN");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        #endregion

        #region tru tong quy tin va gui tin nhan
        public ResultEntity guiTinNhan(List<TIN_NHAN> lst_tin_nhan, long id_truong, short nam_gui, short thang_gui, long? nguoi, int tong_tin_gui, int loai_tin)
        {
            QuyTinBO quyTinBO = new QuyTinBO();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    #region "update quy tin truong"
                    TruongBO truongBO = new TruongBO();
                    TRUONG detailTruong = new TRUONG();
                    detailTruong = truongBO.getTruongById(id_truong);
                    if (detailTruong.ID_DOI_TAC != null || detailTruong.ID_DOI_TAC > 0)
                    {
                        long tong_cap = detailTruong.TONG_TIN_CAP == null ? 0 : detailTruong.TONG_TIN_CAP.Value;
                        long da_dung = detailTruong.TONG_TIN_DA_DUNG == null ? 0 : detailTruong.TONG_TIN_DA_DUNG.Value;
                        string sql = string.Format(@"update truong set TONG_TIN_CAP={0}, TONG_TIN_DA_DUNG={1}, NGAY_SUA={2}, NGUOI_SUA={3} where id={4}", (tong_cap + 0), (da_dung + tong_tin_gui), DateTime.Now, nguoi, id_truong);
                        context.Database.ExecuteSqlCommand(sql);
                    }
                    else
                    {
                        if (detailTruong.MA_GOI_TIN != null)
                        {
                            GoiTinBO goiTinBO = new GoiTinBO();
                            GOI_TIN goiTinDetail = goiTinBO.getGoiTinByMa(detailTruong.MA_GOI_TIN.Value);
                            if (goiTinDetail != null)
                            {
                                QUY_TIN detail = new QUY_TIN();
                                detail = (from p in context.QUY_TIN
                                          where p.NAM_GUI == nam_gui && p.THANG_GUI == thang_gui
                                          && p.ID_TRUONG == id_truong && p.LOAI_TIN == loai_tin
                                          select p).FirstOrDefault();
                                if (detail != null)
                                {
                                    detail.SO_HS_DK = detailTruong.SO_HS_DANG_KY == null ? 0 : detailTruong.SO_HS_DANG_KY.Value;
                                    long SO_TIN_HS = detail.SO_TIN_HS;

                                    if (loai_tin == SYS_Loai_Tin.Tin_Lien_Lac) SO_TIN_HS = (goiTinDetail.SO_TIN_LIEN_LAC_HS == null ? 0 : goiTinDetail.SO_TIN_LIEN_LAC_HS.Value) * 4;
                                    if (loai_tin == SYS_Loai_Tin.Tin_Thong_Bao)
                                    {
                                        #region set quy tin he
                                        if (thang_gui != 6 && thang_gui != 7)
                                            SO_TIN_HS = goiTinDetail.SO_TIN_THONG_BAO_HS == null ? 0 : goiTinDetail.SO_TIN_THONG_BAO_HS.Value;
                                        else SO_TIN_HS = goiTinDetail.SO_TIN_HE_HS == null ? 0 : goiTinDetail.SO_TIN_HE_HS.Value;
                                        #endregion
                                    }

                                    detail.SO_TIN_HS = SO_TIN_HS;
                                    detail.TONG_CAP = detail.SO_HS_DK * SO_TIN_HS;
                                    detail.TONG_DA_SU_DUNG = detail.TONG_DA_SU_DUNG + tong_tin_gui;
                                    detail.TONG_CON = detail.TONG_CAP + detail.TONG_THEM - detail.TONG_DA_SU_DUNG;
                                    detail.NGAY_SUA = DateTime.Now;
                                    detail.NGUOI_SUA = nguoi;
                                    context.SaveChanges();
                                }
                            }
                        }
                    }
                    #endregion

                    foreach (var item in lst_tin_nhan)
                    {
                        item.NGAY_TAO = DateTime.Now;
                        item.NGUOI_TAO = nguoi;
                        item.ID = context.Database.SqlQuery<long>("SELECT TIN_NHAN_SEQ1.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                    }
                    context.TIN_NHAN.AddRange(lst_tin_nhan);
                    context.SaveChanges();
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("TIN_NHAN");
                QICache.RemoveByFirstName("QUY_TIN");
                QICache.RemoveByFirstName("TRUONG");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi khi lưu dữ liệu. Liên hệ với quản trị viên";
            }
            return res;
        }
        #endregion

        #region check tin nhan da ton tai chua
        public TIN_NHAN checkExistsSms(long id_truong, short nam_gui, short thang_gui, long? id_nguoi_nhan, string sdt, DateTime? henGio, double subMinutes, string noi_dung_khong_dau)
        {
            TIN_NHAN data = new TIN_NHAN();
            DateTime den = (henGio != null && henGio.Value > DateTime.MinValue) ? henGio.Value : DateTime.Now;
            DateTime tu = den.AddMinutes(-subMinutes);
            using (var context = new oneduEntities())
            {
                var tmp = (from p in context.TIN_NHAN
                           where p.ID_TRUONG == id_truong && p.NAM_GUI == nam_gui && p.THANG_GUI == thang_gui
                           select p);
                if (id_nguoi_nhan != null)
                    tmp = tmp.Where(x => x.ID_NGUOI_NHAN == id_nguoi_nhan);
                tmp = tmp.Where(x => x.THOI_GIAN_GUI >= tu && x.THOI_GIAN_GUI <= den);
                tmp = tmp.Where(x => x.SDT_NHAN == sdt);
                data = tmp.ToList().FirstOrDefault(x => x.NOI_DUNG_KHONG_DAU.ToNormalizeLowerRelaceInBO() == noi_dung_khong_dau.ToNormalizeLowerRelaceInBO());
            }
            return data;
        }
        #endregion

        #region chuyển tiếng việt không dấu
        private static readonly string[] VietnameseSigns = new string[] { "aAeEoOuUiIdDyY", "áàạảãâấầậẩẫăắằặẳẵ", "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ", "éèẹẻẽêếềệểễ", "ÉÈẸẺẼÊẾỀỆỂỄ", "óòọỏõôốồộổỗơớờợởỡ", "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ", "úùụủũưứừựửữ", "ÚÙỤỦŨƯỨỪỰỬỮ", "íìịỉĩ", "ÍÌỊỈĨ", "đ₫", "Đ", "ýỳỵỷỹ", "ÝỲỴỶỸ" };
        public string chuyenTiengVietKhongDau(string str)
        {
            //Tiến hành thay thế , lọc bỏ dấu cho chuỗi
            if (str == null || str == "") return "";
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            str = RemoveSpecialCharacters(str);
            str = RemoveDiacritics(str);
            return str;
        }

        public string RemoveSpecialCharacters(string str)
        {
            str = str.Replace("“", "\"");
            str = str.Replace("”", "\"");
            return Regex.Replace(str, "[@#$%^&*`’]", "", RegexOptions.Compiled);
        }

        public string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.Normalize(NormalizationForm.FormD);
            var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC);
        }
        #endregion

        #region lay nha mang
        public string getLoaiNhaMang(string phone)
        {
            ChuyenMangGiuSoBO chuyenMangGiuSoBO = new ChuyenMangGiuSoBO();
            List<CHUYEN_MANG_GIU_SO> listChange = chuyenMangGiuSoBO.getChuyenMangGiuSo();
            if (!string.IsNullOrEmpty(phone))
            {
                long index_sdt = removeHeadPhone(phone);
                if (index_sdt > 0)
                {
                    CHUYEN_MANG_GIU_SO detail = listChange.FirstOrDefault(x => x.SDT == index_sdt);
                    if (detail != null) return detail.LOAI_NHA_MANG;
                }
                string a = "";
                string b = "";
                List<string> lst_viettel = new List<string> { "086", "096", "097", "098", "032", "033", "034", "035", "036", "037", "038", "039" };
                List<string> lst_mobi = new List<string> { "089", "090", "093", "070", "079", "077", "076", "078" };
                List<string> lst_vina = new List<string> { "088", "091", "094", "083", "084", "085", "081", "082" };
                List<string> lst_vnMobile = new List<string> { "092", "052", "056", "058" };
                List<string> lst_gMobile = new List<string> { "099", "059" };
                //List<string> lst_sPhone = new List<string> { "095" };

                if (phone.Length > 5)
                {
                    if (phone.IndexOf("84") == 0)
                    {
                        a = "0" + phone.Substring(2, 2);
                        b = "0" + phone.Substring(2, 3);
                    }
                    else
                    {
                        a = phone.Substring(0, 3);
                        b = phone.Substring(0, 4);
                    }
                }
                if (phone.IndexOf("84") == 0)
                {
                    if (phone.Length == 11) phone = "0" + phone.Substring(0, 9);
                    if (phone.Length == 12) phone = "0" + phone.Substring(0, 10);
                }

                if (phone.Length != 10) return "";

                if (lst_viettel.Contains(a) || lst_viettel.Contains(b))
                    return "Viettel";
                if (lst_mobi.Contains(a) || lst_mobi.Contains(b))
                    return "MobiFone";
                if (lst_vina.Contains(a) || lst_vina.Contains(b))
                    return "VinaPhone";
                if (lst_vnMobile.Contains(a) || lst_vnMobile.Contains(b))
                    return "VietnamMobile";
                if (lst_gMobile.Contains(a) || lst_gMobile.Contains(b))
                    return "GMobile";
                //if (lst_sPhone.Contains(a) || lst_sPhone.Contains(b))
                //    return "SPhone";
                return "";
            }
            else
            {
                return "";
            }
        }

        public long removeHeadPhone(string phone)
        {
            long index_sdt = 0;
            try
            {
                if (!string.IsNullOrEmpty(phone))
                {
                    if (phone.IndexOf("84") == 0)
                        index_sdt = Convert.ToInt64(phone.Substring(2, phone.Length - 2));
                    else if (phone.IndexOf("0") == 0)
                        index_sdt = Convert.ToInt64(phone.Substring(1, phone.Length - 1));
                    else index_sdt = Convert.ToInt64(phone);
                }
            }
            catch { }
            return index_sdt;
        }
        #endregion
    }
}
