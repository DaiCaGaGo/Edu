using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class HocSinhBO
    {
        #region get
        public List<HOC_SINH> getHocSinh(bool is_all = false, long id_all = 0, string text_all = "Chọn tất cả")
        {
            List<HOC_SINH> data = new List<HOC_SINH>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_SINH", "getHocSinh", is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.HOC_SINH
                            where p.IS_DELETE != true
                            orderby p.THU_TU, p.TEN, p.HO_DEM
                            select p).ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<HOC_SINH>();
                        HOC_SINH item_all = new HOC_SINH();
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
                    data = QICache.Get(strKeyCache) as List<HOC_SINH>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public HOC_SINH getHocSinhByID(long id)
        {
            HOC_SINH data = new HOC_SINH();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_SINH", "getHocSinhByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.HOC_SINH where p.ID == id select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as HOC_SINH;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public HOC_SINH getHocSinhByMa(string ma_hs)
        {
            HOC_SINH data = new HOC_SINH();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_SINH", "getHocSinhByMa", ma_hs);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.HOC_SINH where p.MA.Equals(ma_hs) && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as HOC_SINH;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }

        public HOC_SINH getHocSinhByMaAndNamHoc(string ma_hs, short id_nam_hoc)
        {
            HOC_SINH data = new HOC_SINH();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_SINH", "getHocSinhByMaAndNamHoc", ma_hs, id_nam_hoc);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.HOC_SINH where p.MA.Equals(ma_hs) && p.ID_NAM_HOC == id_nam_hoc && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as HOC_SINH;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<HOC_SINH> getHocSinhByLop(long id_lop)
        {
            List<HOC_SINH> data = new List<HOC_SINH>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_SINH", "getHocSinhByLop", id_lop);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.HOC_SINH
                            where p.ID_LOP == id_lop && p.IS_DELETE != true
                            orderby p.TEN, p.HO_DEM, p.THU_TU
                            select p).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<HOC_SINH>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<HOC_SINH> getHocSinhByTruongLopNamHoc(long id_truong, short? ma_khoi, long id_lop, short id_nam_hoc)
        {
            List<HOC_SINH> data = new List<HOC_SINH>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_SINH", "getHocSinhByTruongLopNamHoc", id_truong, ma_khoi, id_lop, id_nam_hoc);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var temp = (from p in context.HOC_SINH
                                where p.ID_TRUONG == id_truong
                                && p.ID_NAM_HOC == id_nam_hoc
                                && p.ID_LOP == id_lop
                                && p.IS_DELETE != true
                                select p);
                    if (ma_khoi != null)
                        temp = temp.Where(x => x.ID_KHOI == ma_khoi);
                    temp = temp.OrderBy(x => x.THU_TU).ThenBy(x => x.TEN).ThenBy(x => x.HO_DEM);
                    data = temp.ToList();
                    //data = (from p in context.HOC_SINH
                    //        where p.ID_TRUONG == id_truong
                    //        && p.ID_NAM_HOC == id_nam_hoc
                    //        && p.ID_KHOI == ma_khoi
                    //        && p.ID_LOP == id_lop
                    //        && p.IS_DELETE != true
                    //        orderby p.TEN, p.HO_DEM, p.THU_TU
                    //        select p).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<HOC_SINH>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<PhuHuynhHocSinhEntity> getChiHoiPhuHuynhByLop(long id_lop)
        {
            List<PhuHuynhHocSinhEntity> data = new List<PhuHuynhHocSinhEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_SINH", "getChiHoiPhuHuynhByLop", id_lop);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    try
                    {
                        string strQuery = string.Format(@"select ID as ID_HOC_SINH, HO_TEN as HO_TEN_HOC_SINH, 'Phụ huynh em ' || HO_TEN as TEN_PHU_HUYNH, SDT_NHAN_TIN from HOC_SINH where ID_LOP=:0");
                        data = context.Database.SqlQuery<PhuHuynhHocSinhEntity>(strQuery, id_lop).ToList();

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
                    data = QICache.Get(strKeyCache) as List<PhuHuynhHocSinhEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<HOC_SINH> getHocSinhByKhoiLop(long id_truong, short? ma_khoi, long? id_lop, short id_nam_hoc, string cap_hoc)
        {
            List<HOC_SINH> data = new List<HOC_SINH>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_SINH", "getHocSinhByKhoiLop", id_truong, ma_khoi, id_lop, id_nam_hoc, cap_hoc);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.HOC_SINH
                               where p.ID_TRUONG == id_truong && p.ID_NAM_HOC == id_nam_hoc && p.MA_CAP_HOC == cap_hoc && p.IS_DELETE != true
                               select p);
                    if (ma_khoi != null)
                        tmp = tmp.Where(x => x.ID_KHOI == ma_khoi);
                    if (id_lop != null)
                        tmp = tmp.Where(x => x.ID_LOP == id_lop);
                    tmp.OrderBy(x => x.TEN).ThenBy(x => x.HO_DEM).ThenBy(x => x.THU_TU);
                    data = tmp.ToList();

                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<HOC_SINH>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<HOC_SINH> getHocSinhByTruongKhoi(long id_truong, short ma_khoi, short id_nam_hoc)
        {
            List<HOC_SINH> data = new List<HOC_SINH>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_SINH", "getHocSinhByTruongKhoi", id_truong, ma_khoi, id_nam_hoc);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.HOC_SINH
                               where p.ID_TRUONG == id_truong && p.ID_KHOI == ma_khoi && p.ID_NAM_HOC == id_nam_hoc
                               select p);
                    tmp.OrderBy(x => x.TEN).ThenBy(x => x.HO_DEM).ThenBy(x => x.THU_TU);
                    data = tmp.ToList();

                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<HOC_SINH>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<HocSinhLopEntity> getHocSinhGuiThongBaoHangNgayByKhoiLop(long id_truong, short? ma_khoi, long? id_lop, short id_nam_hoc, string cap_hoc, short hoc_ky)
        {
            List<HocSinhLopEntity> data = new List<HocSinhLopEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_SINH", "LOP", "TIN_NHAN", "getHocSinhGuiThongBaoHangNgayByKhoiLop", id_truong, ma_khoi, id_lop, id_nam_hoc, cap_hoc, hoc_ky);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    try
                    {
                        string strQuery = string.Format(@"select hs.ID
                                                ,hs.HO_TEN
                                                ,hs.TEN
                                                ,hs.SDT_NHAN_TIN
                                                ,hs.SDT_NHAN_TIN2
                                                ,hs.NGAY_SINH
                                                ,hs.IS_GUI_BO_ME
                                                ,hs.ID_LOP
                                                ,hs.THU_TU
                                                ,hs.IS_DK_KY1, hs.IS_DK_KY2
                                                ,hs.IS_MIEN_GIAM_KY1, hs.IS_MIEN_GIAM_KY2, hs.IS_CON_GV
                                                ,l.TEN as TEN_LOP
                                                ,case when sum(hs.is_gui_bo_me)>0 then 2 else 1 end as HE_SO
                                                ,sum(tn.so_tin) as SO_TIN_TRONG_NGAY
                                            from HOC_SINH hs
                                            inner join LOP l on HS.ID_TRUONG = L.ID_TRUONG AND HS.id_nam_hoc = L.ID_NAM_HOC AND hs.ID_LOP = l.ID
                                            left join TIN_NHAN tn on hs.ID=tn.id_nguoi_nhan and tn.loai_nguoi_nhan=1 and hs.id_truong=tn.id_truong  and tn.loai_tin = 2
                                            and TRUNC(tn.ngay_tao)=TRUNC(:0)
                                            where hs.id_truong= :1 and hs.id_nam_hoc=:2");
                        if (ma_khoi != null && ma_khoi != 0)
                            strQuery += string.Format(@" and hs.ID_KHOI ={0}", ma_khoi);
                        if (id_lop != null && id_lop != 0) strQuery += string.Format(@" and hs.ID_LOP ={0}", id_lop);
                        if (hoc_ky == 1) strQuery += @" and hs.TRANG_THAI_HOC in (1,2,3,8,9,10)";
                        else if (hoc_ky == 2) strQuery += @" and hs.TRANG_THAI_HOC in (1,2,3,6,7,10)";
                        strQuery += @" and hs.MA_CAP_HOC = :3
                                                        group by hs.ID
                                                            ,hs.HO_TEN
                                                            ,hs.TEN
                                                            ,hs.SDT_NHAN_TIN
                                                            ,hs.SDT_NHAN_TIN2
                                                            ,hs.NGAY_SINH
                                                            ,hs.IS_GUI_BO_ME
                                                            ,hs.ID_LOP
                                                            ,hs.THU_TU
                                                            ,hs.IS_DK_KY1, hs.IS_DK_KY2
                                                            ,hs.IS_MIEN_GIAM_KY1, hs.IS_MIEN_GIAM_KY2, hs.IS_CON_GV
                                                            ,l.TEN
                                                        order by l.TEN, hs.TEN, hs.THU_TU";
                        data = context.Database.SqlQuery<HocSinhLopEntity>(strQuery, DateTime.Now, id_truong, id_nam_hoc, cap_hoc).ToList();

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
                    data = QICache.Get(strKeyCache) as List<HocSinhLopEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<HocSinhLopEntity> getHocSinhGuiThongBaoHangNgayByListKhoiLop(long id_truong, List<short> lst_ma_khoi, List<long> lst_id_lop, short id_nam_hoc, string cap_hoc, short hoc_ky)
        {
            DataAccessAPI dataAccessAPI = new DataAccessAPI();
            List<HocSinhLopEntity> data = new List<HocSinhLopEntity>();
            var QICache = new DefaultCacheProvider();
            string str_lst_ma_khoi = "", str_lst_id_lop = "";
            str_lst_ma_khoi = dataAccessAPI.ConvertListToString<short>(lst_ma_khoi, ",");
            str_lst_id_lop = dataAccessAPI.ConvertListToString<long>(lst_id_lop, ",");
            string strKeyCache = QICache.BuildCachedKey("HOC_SINH", "LOP", "TIN_NHAN", "getHocSinhGuiThongBaoHangNgayByListKhoiLop", id_truong, str_lst_ma_khoi, str_lst_id_lop, id_nam_hoc, cap_hoc, hoc_ky);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    try
                    {
                        string strQuery = string.Format(@"select hs.ID
                                                ,hs.HO_TEN,hs.HO_DEM
                                                ,hs.TEN
                                                ,hs.SDT_NHAN_TIN
                                                ,hs.SDT_NHAN_TIN2
                                                ,hs.NGAY_SINH
                                                ,hs.IS_GUI_BO_ME
                                                ,hs.ID_LOP
                                                ,hs.THU_TU
                                                ,hs.IS_DK_KY1, hs.IS_DK_KY2
                                                ,hs.IS_MIEN_GIAM_KY1, hs.IS_MIEN_GIAM_KY2, hs.IS_CON_GV
                                                ,l.TEN as TEN_LOP
                                                ,case when sum(hs.is_gui_bo_me)>0 then 2 else 1 end as HE_SO
                                                ,case when sum(tn.so_tin)>0 then sum(tn.so_tin) else 0 end as SO_TIN_TRONG_NGAY
                                            from HOC_SINH hs
                                            inner join LOP l on HS.ID_TRUONG = L.ID_TRUONG AND HS.id_nam_hoc = L.ID_NAM_HOC AND hs.ID_LOP = l.ID
                                            left join TIN_NHAN tn on hs.ID=tn.id_nguoi_nhan and tn.loai_nguoi_nhan=1 and hs.id_truong=tn.id_truong  and tn.loai_tin = 2
                                            and TRUNC(tn.ngay_tao)=TRUNC(:0)
                                            where hs.id_truong= :1 and hs.id_nam_hoc=:2 and not (hs.is_delete is not null and hs.is_delete=1)");
                        #region Khối
                        if (lst_ma_khoi != null && lst_ma_khoi.Count == 1)
                            strQuery += string.Format(@" and hs.ID_KHOI ={0}", lst_ma_khoi[0]);
                        else if (lst_ma_khoi != null && lst_ma_khoi.Count > 1)
                        {
                            strQuery += string.Format(" and not ( hs.ID_KHOI !={0}", lst_ma_khoi[0]);
                            for (int i = 1; i < lst_ma_khoi.Count; i++)
                            {
                                strQuery += string.Format(" and hs.ID_KHOI !={0}", lst_ma_khoi[i]);
                            }
                            strQuery += " )";
                        }
                        #endregion
                        #region Lớp
                        if (lst_id_lop != null && lst_id_lop.Count == 1)
                            strQuery += string.Format(@" and hs.ID_LOP ={0}", lst_id_lop[0]);
                        else if (lst_id_lop != null && lst_id_lop.Count > 1)
                        {
                            strQuery += string.Format(" and not ( hs.ID_LOP !={0}", lst_id_lop[0]);
                            for (int i = 1; i < lst_id_lop.Count; i++)
                            {
                                strQuery += string.Format(" and hs.ID_LOP !={0}", lst_id_lop[i]);
                            }
                            strQuery += " )";
                        }
                        #endregion
                        if (hoc_ky == 1) strQuery += @" and hs.TRANG_THAI_HOC in (1,2,3,8,9,10)";
                        else if (hoc_ky == 2) strQuery += @" and hs.TRANG_THAI_HOC in (1,2,3,6,7,10)";
                        strQuery += @" and hs.MA_CAP_HOC = :3
                                                        group by hs.ID
                                                            ,hs.HO_TEN,hs.HO_DEM
                                                            ,hs.TEN
                                                            ,hs.SDT_NHAN_TIN
                                                            ,hs.SDT_NHAN_TIN2
                                                            ,hs.NGAY_SINH
                                                            ,hs.IS_GUI_BO_ME
                                                            ,hs.ID_LOP
                                                            ,hs.THU_TU
                                                            ,hs.IS_DK_KY1, hs.IS_DK_KY2
                                                            ,hs.IS_MIEN_GIAM_KY1, hs.IS_MIEN_GIAM_KY2, hs.IS_CON_GV
                                                            ,l.TEN
                                                        order by l.TEN, hs.THU_TU,NLSSORT(hs.ten,'NLS_SORT=vietnamese'),NLSSORT(hs.ho_dem,'NLS_SORT=vietnamese')";
                        data = context.Database.SqlQuery<HocSinhLopEntity>(strQuery, DateTime.Now, id_truong, id_nam_hoc, cap_hoc).ToList();

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
                    data = QICache.Get(strKeyCache) as List<HocSinhLopEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }

        public long? getMaxThuTuByTruongKhoiLopNamHoc(long id_truong, short? ma_khoi, long? id_lop, short id_nam_hoc)
        {
            long? data = null;
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_SINH", "getMaxThuTuByTruongKhoiLopNamHoc", id_truong, ma_khoi, id_lop, id_nam_hoc);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var sql = @"select MAX(THU_TU) as THU_TU from HOC_SINH 
                        where ID_TRUONG = :0 and ID_KHOI=:1 and ID_NAM_HOC=:2 and ID_LOP=:3 AND NOT (IS_DELETE is not null and IS_DELETE=1 )";
                    data = context.Database.SqlQuery<long?>(sql, id_truong, ma_khoi, id_nam_hoc, id_lop).FirstOrDefault();
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
        public List<BienLaiThuTienHSEntity> getBienLaiThuTienHS(long id_truong, short id_nam_hoc, short hoc_ky, string ma_cap_hoc, string sotien, string noidung, string sotienbangchu, List<short> lst_ma_khoi, List<long> lst_id_lop)
        {
            DataAccessAPI dataAccessAPI = new DataAccessAPI();
            string str_lst_ma_khoi = "", str_lst_id_lop = "";
            str_lst_ma_khoi = dataAccessAPI.ConvertListToString<short>(lst_ma_khoi, ",");
            str_lst_id_lop = dataAccessAPI.ConvertListToString<long>(lst_id_lop, ",");
            List<BienLaiThuTienHSEntity> data = new List<BienLaiThuTienHSEntity>();
            var QICache = new DefaultCacheProvider();
            using (oneduEntities context = new oneduEntities())
            {
                string query = @"select Ho_Ten,Lop.Ten as Ten_Lop, " + sotien + " as So_Tien, '" + noidung + "' as Noi_Dung, '" + sotienbangchu + "' " +
                    "as Viet_Bang_Chu, ROW_NUMBER() OVER (partition by LOP.THU_TU,LOP.ID order by hoc_sinh.thu_tu,NLSSORT(hoc_sinh.ten,'NLS_SORT=vietnamese'),NLSSORT(hoc_sinh.ho_dem,'NLS_SORT=vietnamese')) as STT  " +
                    " from hoc_sinh join Lop on hoc_sinh.ID_lop=lop.ID  where not (hoc_sinh.is_delete is not null and hoc_sinh.is_delete=1) and hoc_sinh.id_truong=:0 and hoc_sinh.id_nam_hoc=:1 and ma_cap_hoc=:2";
                #region Khối
                if (lst_ma_khoi != null && lst_ma_khoi.Count == 1)
                    query += string.Format(@" and hoc_sinh.ID_KHOI ={0}", lst_ma_khoi[0]);
                else if (lst_ma_khoi != null && lst_ma_khoi.Count > 1)
                {
                    query += string.Format(" and not ( hoc_sinh.ID_KHOI !={0}", lst_ma_khoi[0]);
                    for (int i = 1; i < lst_ma_khoi.Count; i++)
                    {
                        query += string.Format(" and hoc_sinh.ID_KHOI !={0}", lst_ma_khoi[i]);
                    }
                    query += " )";
                }
                #endregion
                #region Lớp
                if (lst_id_lop != null && lst_id_lop.Count == 1)
                    query += string.Format(@" and ID_LOP ={0}", lst_id_lop[0]);
                else if (lst_id_lop != null && lst_id_lop.Count > 1)
                {
                    query += string.Format(" and not ( ID_LOP !={0}", lst_id_lop[0]);
                    for (int i = 1; i < lst_id_lop.Count; i++)
                    {
                        query += string.Format(" and ID_LOP !={0}", lst_id_lop[i]);
                    }
                    query += " )";
                }
                #endregion
                if (hoc_ky == 1) query += @" and TRANG_THAI_HOC in (1,2,3,8,9,10) and (IS_DK_KY1 is not null and IS_DK_KY1=1)";
                else if (hoc_ky == 2) query += @" and TRANG_THAI_HOC in (1,2,3,6,7,10) and (IS_DK_KY2 is not null and IS_DK_KY2=1)";
                query += string.Format(@" order by LOP.THU_TU,LOP.ID,hoc_sinh.thu_tu,NLSSORT(hoc_sinh.ten,'NLS_SORT=vietnamese'),NLSSORT(hoc_sinh.ho_dem,'NLS_SORT=vietnamese') ");
                data = context.Database.SqlQuery<BienLaiThuTienHSEntity>(query, id_truong, id_nam_hoc, ma_cap_hoc).ToList();
            }
            return data;
        }
        public List<BangTheoDoiHocSinhTHEntity> getBangTheoDoiHocSinh(long id_truong, short id_nam_hoc, short hoc_ky, short? maKhoi, long? idLop, string ma_cap_hoc, string ten_truong)
        {
            List<BangTheoDoiHocSinhTHEntity> data = new List<BangTheoDoiHocSinhTHEntity>();
            var QICache = new DefaultCacheProvider();
            using (oneduEntities context = new oneduEntities())
            {
                string query = @"select HOC_SINH.HO_TEN, HOC_SINH.SDT_NHAN_TIN,LOP.TEN AS TEN_LOP,'" + ten_truong + "' as TEN_TRUONG,GV.HO_TEN AS GVCN_TEN, GV.SDT AS GVCN_SDT, ROW_NUMBER() OVER (order by hoc_sinh.thu_tu,NLSSORT(hoc_sinh.ten,'NLS_SORT=vietnamese'),NLSSORT(hoc_sinh.ho_dem,'NLS_SORT=vietnamese')) as STT  " +
                    " from hoc_sinh join Lop on hoc_sinh.ID_lop=lop.ID LEFT JOIN GIAO_VIEN GV ON Lop.ID_GVCN=GV.ID where not (hoc_sinh.is_delete is not null and hoc_sinh.is_delete=1) and hoc_sinh.id_truong=:0 and hoc_sinh.id_nam_hoc=:1 and ma_cap_hoc=:2";
                if (maKhoi != null)
                    query += string.Format(@" and hoc_sinh.id_khoi={0}", maKhoi);
                if (idLop != null)
                    query += string.Format(@" and hoc_sinh.id_lop={0}", idLop);
                if (hoc_ky == 1) query += @" and TRANG_THAI_HOC in (1,2,3,8,9,10) and (IS_DK_KY1 is not null and IS_DK_KY1=1) ";
                else if (hoc_ky == 2) query += @" and TRANG_THAI_HOC in (1,2,3,6,7,10) and (IS_DK_KY2 is not null and IS_DK_KY2=1)";
                query += string.Format(@" order by hoc_sinh.thu_tu,NLSSORT(hoc_sinh.ten,'NLS_SORT=vietnamese'),NLSSORT(hoc_sinh.ho_dem,'NLS_SORT=vietnamese') ");
                data = context.Database.SqlQuery<BangTheoDoiHocSinhTHEntity>(query, id_truong, id_nam_hoc, ma_cap_hoc).ToList();
            }
            return data;
        }
        public List<PhieuDanhGiaDinhKy_TH> getPhieuLienLac(long id_truong, short id_nam_hoc, short hoc_ky, short? maKhoi, long? idLop, string ma_cap_hoc, string ten_truong)
        {
            List<PhieuDanhGiaDinhKy_TH> data = new List<PhieuDanhGiaDinhKy_TH>();
            var QICache = new DefaultCacheProvider();
            using (oneduEntities context = new oneduEntities())
            {
                //string query = @"select HOC_SINH.HO_TEN, HOC_SINH.SDT_NHAN_TIN,LOP.TEN AS TEN_LOP,'" + ten_truong + "' as TEN_TRUONG,GV.HO_TEN AS GVCN_TEN, GV.SDT AS GVCN_SDT, ROW_NUMBER() OVER (order by hoc_sinh.thu_tu,NLSSORT(hoc_sinh.ten,'NLS_SORT=vietnamese'),NLSSORT(hoc_sinh.ho_dem,'NLS_SORT=vietnamese')) as STT  " +
                //    " from hoc_sinh join Lop on hoc_sinh.ID_lop=lop.ID LEFT JOIN GIAO_VIEN GV ON Lop.ID_GVCN=GV.ID where not (hoc_sinh.is_delete is not null and hoc_sinh.is_delete=1) and hoc_sinh.id_truong=:0 and hoc_sinh.id_nam_hoc=:1 and ma_cap_hoc=:2";
                string query = @"select HOC_SINH.HO_TEN, HOC_SINH.SDT_NHAN_TIN,LOP.TEN AS TEN_LOP
                    ,to_char(hoc_sinh.ngay_sinh,'DD/MM/YYYY') as ngay_sinh
                    ,case when hoc_sinh.ma_gioi_tinh = 1 then 'Nam' 
                    when hoc_sinh.ma_gioi_tinh = 2 then 'Nữ' else '' end as gioi_tinh, 'Học kỳ ' || " + hoc_ky + "|| ' - Năm học ' || " + id_nam_hoc + " || '-' || " + (id_nam_hoc + 1) + " as hoc_ky, upper('" + ten_truong + "') as TEN_TRUONG,GV.HO_TEN AS GVCN_TEN, GV.SDT AS GVCN_SDT , ROW_NUMBER() OVER (order by hoc_sinh.thu_tu,NLSSORT(hoc_sinh.ten,'NLS_SORT=vietnamese'),NLSSORT(hoc_sinh.ho_dem,'NLS_SORT=vietnamese')) as STT from hoc_sinh join Lop on hoc_sinh.ID_lop=lop.ID LEFT JOIN GIAO_VIEN GV ON Lop.ID_GVCN=GV.ID where not (hoc_sinh.is_delete is not null and hoc_sinh.is_delete=1) and hoc_sinh.id_truong=:0 and hoc_sinh.id_nam_hoc=:1 and ma_cap_hoc=:2";
                if (maKhoi != null)
                    query += string.Format(@" and hoc_sinh.id_khoi={0}", maKhoi);
                if (idLop != null)
                    query += string.Format(@" and hoc_sinh.id_lop={0}", idLop);
                if (hoc_ky == 1) query += @" and TRANG_THAI_HOC in (1,2,3,8,9,10)";
                else if (hoc_ky == 2) query += @" and TRANG_THAI_HOC in (1,2,3,6,7,10)";
                query += string.Format(@" order by hoc_sinh.thu_tu,NLSSORT(hoc_sinh.ten,'NLS_SORT=vietnamese'),NLSSORT(hoc_sinh.ho_dem,'NLS_SORT=vietnamese') ");
                data = context.Database.SqlQuery<PhieuDanhGiaDinhKy_TH>(query, id_truong, id_nam_hoc, ma_cap_hoc).ToList();
            }
            return data;
        }
        public List<PhieuDanhGiaDinhKy_TH> getPhieuLienLac1(long id_truong, short id_nam_hoc, short hoc_ky, short? maKhoi, long? idLop, string ma_cap_hoc, string ten_truong)
        {
            List<PhieuDanhGiaDinhKy_TH> data = new List<PhieuDanhGiaDinhKy_TH>();
            var QICache = new DefaultCacheProvider();
            using (oneduEntities context = new oneduEntities())
            {
                string query = @"select HOC_SINH.HO_TEN, HOC_SINH.SDT_NHAN_TIN,LOP.TEN AS TEN_LOP
                    ,to_char(hoc_sinh.ngay_sinh,'DD/MM/YYYY') as ngay_sinh
                    ,case when hoc_sinh.ma_gioi_tinh = 1 then 'Nam' 
                    when hoc_sinh.ma_gioi_tinh = 2 then 'Nữ' else '' end as gioi_tinh, 'Học kỳ ' || " + hoc_ky + "|| ' - Năm học ' || " + id_nam_hoc + " || '-' || " + (id_nam_hoc + 1) + " as hoc_ky, upper('" + ten_truong + "') as TEN_TRUONG, ROW_NUMBER() OVER (order by hoc_sinh.thu_tu,NLSSORT(hoc_sinh.ten,'NLS_SORT=vietnamese'),NLSSORT(hoc_sinh.ho_dem,'NLS_SORT=vietnamese')) as STT, dg.NL_TPVTQ, dg.NL_HT, dg.NL_TGQVD, dg.NL_MANX, dg.NL_NX, dg.PC_CHCL, dg.PC_TTTN, dg.PC_TTKL, dg.PC_DKYT, dg.PC_MANX, dg.PC_NX, dg.NX_GVCN from hoc_sinh join Lop on hoc_sinh.ID_lop=lop.ID LEFT JOIN GIAO_VIEN GV ON Lop.ID_GVCN=GV.ID left join danh_gia_dinh_ky_th dg on hoc_sinh.id=dg.id_hoc_sinh and dg.MA_NAM_HOC=2019 and dg.id_lop=3721 and MA_KY_DG=2 where not (hoc_sinh.is_delete is not null and hoc_sinh.is_delete=1) and hoc_sinh.id_truong=:0 and hoc_sinh.id_nam_hoc=:1 and ma_cap_hoc=:2";
                if (maKhoi != null)
                    query += string.Format(@" and hoc_sinh.id_khoi={0}", maKhoi);
                if (idLop != null)
                    query += string.Format(@" and hoc_sinh.id_lop={0}", idLop);
                if (hoc_ky == 1) query += @" and TRANG_THAI_HOC in (1,2,3,8,9,10)";
                else if (hoc_ky == 2) query += @" and TRANG_THAI_HOC in (1,2,3,6,7,10)";
                query += string.Format(@" order by hoc_sinh.thu_tu,NLSSORT(hoc_sinh.ten,'NLS_SORT=vietnamese'),NLSSORT(hoc_sinh.ho_dem,'NLS_SORT=vietnamese') ");
                data = context.Database.SqlQuery<PhieuDanhGiaDinhKy_TH>(query, id_truong, id_nam_hoc, ma_cap_hoc).ToList();
            }
            return data;
        }
        public List<HocSinhEntity> getHocSinhByOther(long id_truong, short id_nam_hoc, short? maKhoi, long? idLop, string tenHS, short? sms_dangky, short? mienSMS, short? trangThai, short? ma_gioi_tinh, string ma_cap_hoc, string sdt)
        {
            List<HocSinhEntity> data = new List<HocSinhEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_SINH", "getHocSinhByOther", id_truong, id_nam_hoc, maKhoi, idLop, tenHS, sms_dangky, mienSMS, trangThai, ma_gioi_tinh, ma_cap_hoc, sdt);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string query = @"select * from hoc_sinh where not (is_delete is not null and is_delete=1) and id_truong=:0 and id_nam_hoc=:1 and ma_cap_hoc=:2";
                    if (maKhoi != null)
                        query += string.Format(@" and id_khoi={0}", maKhoi);
                    if (idLop != null)
                        query += string.Format(@" and id_lop={0}", idLop);
                    if (!string.IsNullOrEmpty(tenHS))
                        query += string.Format(@" and lower(ho_ten) like lower(N'%{0}%')", tenHS);
                    if (sms_dangky == 1)//đăng ký kỳ 1
                    {
                        query += string.Format(@" and (IS_DK_KY1 is not null and IS_DK_KY1=1)");
                        if (mienSMS == 1) query += string.Format(@" and (IS_MIEN_GIAM_KY1 is not null and IS_MIEN_GIAM_KY1=1)");
                        else if (mienSMS == 2)
                            query += string.Format(@" and not (IS_MIEN_GIAM_KY1 is not null and IS_MIEN_GIAM_KY1 =1)");
                    }
                    else if (sms_dangky == 2)//đăng ký kỳ 2
                    {
                        query += string.Format(@" and (IS_DK_KY2 is not null and IS_DK_KY2=1)");
                        if (mienSMS == 1) query += string.Format(@" and (IS_MIEN_GIAM_KY2 is not null and IS_MIEN_GIAM_KY2=1)");
                        else if (mienSMS == 2)
                            query += string.Format(@" and not (IS_MIEN_GIAM_KY2 is not null and IS_MIEN_GIAM_KY2 =1)");
                    }
                    else if (sms_dangky == 3)//ko sử dụng dịch vụ
                    {
                        query += string.Format(@" and not (IS_DK_KY1 is not null and IS_DK_KY1 = 1) and not (IS_DK_KY2 is not null and IS_DK_KY1 = 1)");
                    }
                    else
                    {
                        if (mienSMS == 1)
                            query += string.Format(@"and ((IS_MIEN_GIAM_KY2 is not null and IS_MIEN_GIAM_KY2=1) or (IS_MIEN_GIAM_KY1 is not null and IS_MIEN_GIAM_KY1=1))");
                        else if (mienSMS == 2)
                            query += string.Format(@"and (IS_MIEN_GIAM_KY2 is not null and IS_MIEN_GIAM_KY2=1 and IS_MIEN_GIAM_KY1 is not null and IS_MIEN_GIAM_KY1=1)");
                    }
                    if (trangThai != null)
                        query += string.Format(@" and trang_thai_hoc={0}", trangThai);
                    if (ma_gioi_tinh != null)
                        query += string.Format(@" and ma_gioi_tinh={0}", ma_gioi_tinh);
                    if (!string.IsNullOrEmpty(sdt))
                        query += string.Format(@" and SDT_NHAN_TIN like N'%{0}%'", sdt);
                    query += string.Format(@" order by nvl(thu_tu, 0),NLSSORT(ten,'NLS_SORT=vietnamese'),NLSSORT(ho_dem,'NLS_SORT=vietnamese')");
                    data = context.Database.SqlQuery<HocSinhEntity>(query, id_truong, id_nam_hoc, ma_cap_hoc).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<HocSinhEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<HocSinhEntity> getListHocSinhByListLop(long id_truong, short? ma_khoi, List<long> lst_id_lop, short id_nam_hoc, string cap_hoc, int hoc_ky)
        {
            List<HocSinhEntity> data = new List<HocSinhEntity>();
            using (oneduEntities context = new oneduEntities())
            {
                var tmp = string.Format(@"select hs.*,l.ten as ten_lop,t.ten as ten_truong from hoc_sinh hs left join lop l on hs.id_truong=l.id_truong and hs.id_lop=l.id left join truong t on hs.ID_TRUONG=t.id where hs.id_truong={0} and hs.MA_CAP_HOC='{1}' and hs.ID_NAM_HOC={2}", id_truong, cap_hoc, id_nam_hoc);
                if (ma_khoi != null)
                    tmp += " and hs.ID_KHOI=" + ma_khoi;
                if (lst_id_lop != null && lst_id_lop.Count == 1)
                    tmp += string.Format(@" and hs.ID_LOP ={0}", lst_id_lop[0]);
                else if (lst_id_lop != null && lst_id_lop.Count > 1)
                {
                    tmp += string.Format(" and not ( hs.ID_LOP !={0}", lst_id_lop[0]);
                    for (int i = 1; i < lst_id_lop.Count; i++)
                    {
                        tmp += string.Format(" and hs.ID_LOP !={0}", lst_id_lop[i]);
                    }
                    tmp += " )";
                }
                if (hoc_ky == 1) tmp += " and hs.TRANG_THAI_HOC in (1,2,3,8,9,10)";
                else if (hoc_ky == 2) tmp += " and hs.TRANG_THAI_HOC in (1,2,3,6,7,10)";
                tmp += " order by hs.id_khoi,hs.id_lop,hs.thu_tu, NLSSORT(hs.ten, 'NLS_SORT=vietnamese'), NLSSORT(hs.ho_dem, 'NLS_SORT=vietnamese')";
                data = context.Database.SqlQuery<HocSinhEntity>(tmp).ToList();
            }
            return data;
        }
        public HOC_SINH checkExistsHocSinh(short id_nam_hoc, long id_truong, short ma_khoi, long idLop, string ho_ten, List<string> sdt, DateTime? ngaySinh, string ma_hs = "")
        {
            var QICache = new DefaultCacheProvider();
            string strSession = QICache.BuildCachedKey("HOC_SINH", "checkExistsHocSinh", id_nam_hoc, id_truong, ma_khoi, idLop, ho_ten, sdt, ngaySinh, ma_hs);
            HOC_SINH detail = new HOC_SINH();
            if (!QICache.IsSet(strSession))
            {
                List<HOC_SINH> lst = getHocSinhByTruongKhoi(id_truong, ma_khoi, id_nam_hoc);
                lst = lst.Where(x => x.ID_LOP == idLop && x.HO_TEN.ToNormalizeLowerRelaceInBO() == ho_ten.ToNormalizeLowerRelaceInBO() && x.IS_DELETE != true).ToList();
                if (!string.IsNullOrEmpty(ma_hs))
                {
                    lst = lst.Where(x => x.MA == ma_hs).ToList();
                }
                else
                {
                    if (sdt.Count > 0)
                        lst = lst.Where(x => sdt.Contains(x.SDT_NHAN_TIN) || sdt.Contains(x.SDT_NHAN_TIN2) || sdt.Contains(x.SDT_BO) || sdt.Contains(x.SDT_ME) || sdt.Contains(x.SDT_NBH) || ((string.IsNullOrEmpty(x.SDT_NHAN_TIN) || x.SDT_NHAN_TIN == " ")
                        && (string.IsNullOrEmpty(x.SDT_NHAN_TIN2) || x.SDT_NHAN_TIN2 == " ")
                        && (string.IsNullOrEmpty(x.SDT_BO) || x.SDT_BO == " ")
                        && (string.IsNullOrEmpty(x.SDT_ME) || x.SDT_ME == " ")
                        && (string.IsNullOrEmpty(x.SDT_NBH) || x.SDT_NBH == " ")
                        )).ToList();

                    if (ngaySinh != null && ngaySinh > DateTime.MinValue)
                        lst = lst.Where(x => x.NGAY_SINH == ngaySinh || x.NGAY_SINH == null).ToList();
                }
                detail = lst.FirstOrDefault();
                QICache.Set(strSession, detail, 300000);
            }
            else
            {
                try
                {
                    detail = QICache.Get(strSession) as HOC_SINH;
                }
                catch
                {
                    QICache.Invalidate(strSession);
                }
            }
            return detail;
        }
        public long? getTongSoHocSinhByTruongCapHocNamHoc(long id_truong, string ma_cap_hoc, short nam_hoc, short hoc_ky)
        {
            long? data = null;
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_SINH", "getTongSoHocSinhByTruongCapHocNamHoc", id_truong, ma_cap_hoc, nam_hoc, hoc_ky);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var sql = @"select count(id) from hoc_sinh 
                        where not (IS_DELETE is not null and IS_DELETE = 1) 
                        and id_truong=:0 and MA_CAP_HOC=:1 and ID_NAM_HOC=:2";
                    if (hoc_ky == 1)
                    {
                        sql += @" and TRANG_THAI_HOC in (1,2,3,8,9,10)";
                    }
                    else if (hoc_ky == 2)
                    {
                        sql += @" and TRANG_THAI_HOC in (1,2,3,6,7,10)";
                    }
                    data = context.Database.SqlQuery<long?>(sql, id_truong, ma_cap_hoc, nam_hoc).FirstOrDefault();
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
        public long? getTongSoHocSinhDangKySMSByTruongCapHocNamHoc(long id_truong, string ma_cap_hoc, short nam_hoc, short hoc_ky)
        {
            long? data = null;
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_SINH", "getTongSoHocSinhDangKySMSByTruongCapHocNamHoc", id_truong, ma_cap_hoc, nam_hoc, hoc_ky);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var sql = @"select count(id) from hoc_sinh 
                        where not (IS_DELETE is not null and IS_DELETE = 1) 
                        and id_truong=:0 and MA_CAP_HOC=:1 and ID_NAM_HOC=:2";
                    if (hoc_ky == 1)
                        sql += @" and TRANG_THAI_HOC in (1,2,3,8,9,10) AND IS_DK_KY1=1";
                    else if (hoc_ky == 2)
                        sql += @" and TRANG_THAI_HOC in (1,2,3,6,7,10) AND IS_DK_KY2=1";
                    data = context.Database.SqlQuery<long?>(sql, id_truong, ma_cap_hoc, nam_hoc).FirstOrDefault();
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
        public List<HocSinhEntity> getHocSinhByTruongCapNamHoc(long id_truong, string cap_hoc, short id_nam_hoc, short hoc_ky)
        {
            List<HocSinhEntity> data = new List<HocSinhEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_SINH", "KHOI", "LOP", "TRANG_THAI_HS", "GIOI_TINH", "getHocSinhByTruongCapNamHoc", id_truong, cap_hoc, id_nam_hoc, hoc_ky);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    try
                    {
                        string strQuery = string.Format(@"select hs.*
,TO_CHAR(hs.ngay_sinh, 'DD/MM/YYYY')as str_ngay_sinh
,k.ten as ten_khoi, l.ten as ten_lop, tt.ten as trang_thai_hs, gt.ten as ten_gioi_tinh
,case when (hs.is_con_gv is not null and hs.is_con_gv = 1) then 'x' else '' end str_Con_GV
,case when (hs.is_gui_bo_me is not null and hs.is_gui_bo_me = 1) then 'x' else '' end str_Gui_Bo_me
,case when (hs.is_gui_bo_me is not null and hs.is_gui_bo_me = 1) then hs.sdt_nhan_tin || ';' || hs.sdt_nhan_tin2 else hs.sdt_nhan_tin end str_SDT_nhan_tin
,case when (hs.is_dk_ky1 is not null and hs.is_dk_ky1 = 1) then 'x' else '' end str_dk_ky1
,case when (hs.is_dk_ky2 is not null and hs.is_dk_ky2 = 1) then 'x' else '' end str_dk_ky2
,case when (hs.is_mien_giam_ky1 is not null and hs.is_mien_giam_ky1 = 1) then 'x' else '' end str_mien_ky1
,case when (hs.is_mien_giam_ky2 is not null and hs.is_mien_giam_ky2 = 1) then 'x' else '' end str_mien_ky2
,TO_CHAR(hs.ngay_tao, 'DD/MM/YYYY')as ngay_tao
from hoc_sinh hs 
join khoi k on k.ma = hs.id_khoi
join lop l on l.id_truong=hs.id_truong and l.id_khoi=hs.id_khoi and l.id_nam_hoc=hs.id_nam_hoc and l.id = hs.id_lop
left join trang_thai_hs tt on tt.ma = hs.trang_thai_hoc
left join gioi_tinh gt on gt.ma = hs.ma_gioi_tinh
where hs.id_truong=:0 and hs.ma_cap_hoc=:1 and hs.id_nam_hoc=:2");
                        if (hoc_ky == 1) strQuery += @" and hs.TRANG_THAI_HOC in (1,2,3,8,9,10)";
                        else if (hoc_ky == 2) strQuery += @" and hs.TRANG_THAI_HOC in (1,2,3,6,7,10)";
                        strQuery += @" order by k.ten, l.ten, hs.thu_tu, NLSSORT(hs.ten, 'NLS_SORT=vietnamese'), NLSSORT(hs.ho_dem, 'NLS_SORT=vietnamese')";
                        data = context.Database.SqlQuery<HocSinhEntity>(strQuery, id_truong, cap_hoc, id_nam_hoc).ToList();
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
                    data = QICache.Get(strKeyCache) as List<HocSinhEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<HocSinhEntity> getHocSinhByTruongKhoiLopTrangThai(long id_truong, string ma_cap_hoc, short id_khoi, short id_nam_hoc, long id_lop, short hoc_ky)
        {
            List<HocSinhEntity> data = new List<HocSinhEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_SINH", "getHocSinhByTruongKhoiLopTrangThai", id_truong, ma_cap_hoc, id_khoi, id_nam_hoc, id_lop, hoc_ky);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = string.Format(@"select * from HOC_SINH where id_truong=:0 and ma_cap_hoc=:1 and ID_KHOI=:2 and ID_NAM_HOC=:3 and ID_LOP=:4");
                    if (hoc_ky == 1)
                        tmp += " and TRANG_THAI_HOC in (1,2,3,8,9,10)";
                    else if (hoc_ky == 2) tmp += " and TRANG_THAI_HOC in (1,2,3,6,7,10)";
                    tmp += " and not (is_delete is not null and is_delete=1)";
                    data = context.Database.SqlQuery<HocSinhEntity>(tmp, id_truong, ma_cap_hoc, id_khoi, id_nam_hoc, id_lop).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<HocSinhEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<DiemChiTietTheoHocSinhEntity> getDiemChiTietByHocSinh(long id_truong, string ma_cap_hoc, short id_nam_hoc, long id_lop, short hoc_ky, long id_hoc_sinh)
        {
            List<DiemChiTietTheoHocSinhEntity> data = new List<DiemChiTietTheoHocSinhEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("LOP_MON", "MON_HOC_TRUONG", "DIEM_CHI_TIET", "getDiemChiTietByHocSinh", id_truong, ma_cap_hoc, id_nam_hoc, id_lop, hoc_ky, id_hoc_sinh);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = string.Format(@"select lm.ID_MON, lm.ID_MON_TRUONG, mt.TEN as ten_mon_hoc,dct.DIEM1
                    ,dct.DIEM2,dct.diem3,dct.DIEM4,dct.diem5,dct.diem6,dct.diem7,dct.diem8,dct.diem9,dct.diem10
                    ,dct.DIEM11,dct.DIEM12,dct.diem13,dct.DIEM14,dct.diem15,dct.diem16,dct.diem17,dct.diem18,dct.diem19,dct.diem20
                    ,dct.DIEM_TRUNG_BINH_KY1, dct.DIEM_TRUNG_BINH_KY2, dct.DIEM_TRUNG_BINH_CN
                    from LOP_MON lm 
                    left join MON_HOC_TRUONG mt on mt.ID_TRUONG=:0 and mt.MA_CAP_HOC=:1 and lm.ID_MON_TRUONG=mt.id
                    left join DIEM_CHI_TIET dct on dct.id_truong={0} and dct.id_lop=:2 and lm.ID_MON_TRUONG = dct.ID_MON_HOC_TRUONG 
                    and dct.ID_HOC_SINH=:3 and dct.HOC_KY=:4
                    where lm.id_lop={1} and lm.HOC_KY={2} and not (lm.IS_DELETE is not null and lm.IS_DELETE=1)
                    order by mt.THU_TU,mt.TEN", id_truong, id_lop, hoc_ky);
                    data = context.Database.SqlQuery<DiemChiTietTheoHocSinhEntity>(tmp, id_truong, ma_cap_hoc, id_lop, id_hoc_sinh, hoc_ky).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<DiemChiTietTheoHocSinhEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<HOC_SINH> getHocSinhBySDTNhanSMS(string sdt)
        {
            List<HOC_SINH> data = new List<HOC_SINH>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_SINH", "getHocSinhBySDTNhanSMS", sdt);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.HOC_SINH
                               where p.SDT_NHAN_TIN == sdt && p.IS_DELETE != true
                               select p);
                    tmp.OrderBy(x => x.TEN).ThenBy(x => x.HO_DEM).ThenBy(x => x.THU_TU);
                    data = tmp.ToList();

                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<HOC_SINH>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<HOC_SINH> getHocSinhMapZalo(string sdt, short id_nam_hoc)
        {
            List<HOC_SINH> data = new List<HOC_SINH>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_SINH", "getHocSinhMapZalo", sdt, id_nam_hoc);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.HOC_SINH
                               where p.SDT_NHAN_TIN == sdt && p.ID_NAM_HOC == id_nam_hoc && p.IS_DELETE != true
                               select p);
                    tmp.OrderBy(x => x.TEN).ThenBy(x => x.HO_DEM).ThenBy(x => x.THU_TU);
                    data = tmp.ToList();

                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<HOC_SINH>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<HocSinhEntity> getHocSinhByTruongAndPhone(long? id_truong, short id_nam_hoc, string sdt)
        {
            List<HocSinhEntity> data = new List<HocSinhEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_SINH", "getHocSinhByTruongAndPhone", id_truong, sdt);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string query = @"select hs.id, hs.id_truong, hs.ma_cap_hoc, hs.id_nam_hoc, hs.id_khoi, hs.id_lop, hs.ma, hs.HO_TEN, hs.NGAY_SINH, hs.SDT_NHAN_TIN, hs.SDT_NHAN_TIN2, case when hs.SDT_NHAN_TIN2 is not null then hs.SDT_NHAN_TIN || '; ' || hs.SDT_NHAN_TIN2 else hs.SDT_NHAN_TIN end as SDT_BM, t.ten as TEN_TRUONG, l.ten as TEN_LOP from hoc_sinh hs join truong t on t.id =  hs.id_truong join lop l on l.id = hs.id_lop where hs.id_nam_hoc = " + id_nam_hoc;
                    if (id_truong != null) query += " and hs.id_truong = " + id_truong;
                    if (!string.IsNullOrEmpty(sdt))
                        query += string.Format(@" and (hs.SDT_NHAN_TIN like N'%{0}%' or hs.SDT_NHAN_TIN2 like N'%{0}%')", sdt);
                    query += " order by hs.id_truong, hs.id_lop, hs.ten";
                    data = context.Database.SqlQuery<HocSinhEntity>(query, id_truong, id_nam_hoc).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<HocSinhEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<ThongKeHocSinhTheoTruongEntity> thongKeTongHocSinhTheoTruong(long? id_truong, string ma_cap_hoc, short id_nam_hoc, short hoc_ky)
        {
            List<ThongKeHocSinhTheoTruongEntity> data = new List<ThongKeHocSinhTheoTruongEntity>();
            using (oneduEntities context = new oneduEntities())
            {
                //var tmp = string.Format(@"select hs.id_truong,hs.id_khoi,hs.id_lop,
                //    case when hs.id_khoi is null and hs.id_lop is null then 'Tổng trường ' || to_char(t.ten) else to_char(t.ten) end as ten_truong,
                //    case when hs.id_lop is null and hs.id_khoi is not null then 'Tổng ' || to_char(k.ten) else to_char(k.ten) end as ten_khoi,
                //    l.ten as ten_lop,
                //    count(hs.id) as SO_HOC_SINH
                //    from hoc_sinh hs
                //    join TRUONG t on hs.ID_TRUONG=t.ID
                //    join lop l on hs.id_lop=l.id and l.ID_NAM_HOC={0}
                //    join khoi k on k.ma = hs.id_khoi
                //    where not (hs.IS_DELETE is not null and hs.IS_DELETE=1)", id_nam_hoc);
                var tmp = string.Format(@"select hs.id_truong,hs.id_khoi,hs.id_lop,t.ten  as ten_truong,
                   case when hs.id_khoi is null and hs.id_lop is null then 'Tổng trường ' || to_char(t.ten) else to_char(k.ten) end as ten_khoi,
                    case when hs.id_lop is null and hs.id_khoi is not null then 'Tổng ' || to_char(k.ten) else to_char(l.ten) end as ten_lop,
                    count(hs.id) as SO_HOC_SINH
                    from hoc_sinh hs
                    join TRUONG t on hs.ID_TRUONG=t.ID
                    join lop l on hs.id_lop=l.id and l.ID_NAM_HOC={0}
                    join khoi k on k.ma = hs.id_khoi and l.id_khoi = k.ma
                    where not (hs.IS_DELETE is not null and hs.IS_DELETE=1)", id_nam_hoc);
                if (id_truong != null)
                    tmp += " and hs.id_truong=" + id_truong;
                if (hoc_ky == 1) tmp += " and hs.TRANG_THAI_HOC in (1,2,3,6,7,10)";
                else if (hoc_ky == 2) tmp += " and hs.TRANG_THAI_HOC in (1,2,3,8,9,10)";
                if (!string.IsNullOrEmpty(ma_cap_hoc))
                {
                    if (ma_cap_hoc == SYS_Cap_Hoc.MN) tmp += " and k.IS_MN=1";
                    else if (ma_cap_hoc == SYS_Cap_Hoc.TH) tmp += " and k.IS_TH=1";
                    else if (ma_cap_hoc == SYS_Cap_Hoc.THCS) tmp += " and k.IS_THCS=1";
                    else if (ma_cap_hoc == SYS_Cap_Hoc.THPT) tmp += " and k.IS_THPT=1";
                }
                tmp += " group by grouping sets ((hs.id_truong,hs.id_khoi,hs.id_lop,t.ten,l.ten,k.ten), (hs.id_truong,t.ten,hs.id_khoi,k.ten), (hs.id_truong,t.ten)) order by hs.id_truong,hs.id_khoi,hs.id_lop";
                data = context.Database.SqlQuery<ThongKeHocSinhTheoTruongEntity>(tmp).ToList();
            }
            return data;
        }
        public List<ThongKeHocSinhTheoTruongEntity> thongKeTongSoHocSinh(long? id_truong, short id_nam_hoc, short hoc_ky)
        {
            List<ThongKeHocSinhTheoTruongEntity> data = new List<ThongKeHocSinhTheoTruongEntity>();
            using (oneduEntities context = new oneduEntities())
            {
                var sql = string.Format(@"select hs.id_truong, t.ten as TEN_TRUONG, count(hs.id) as SO_HOC_SINH from hoc_sinh hs
                    join TRUONG t on hs.ID_TRUONG = t.ID
                    where hs.id_nam_hoc = {0}", id_nam_hoc);
                if (id_truong != null) sql += " and hs.id_truong=" + id_truong;
                if (hoc_ky == 1) sql += " and hs.TRANG_THAI_HOC in (1,2,3,6,7,10)";
                else if (hoc_ky == 2) sql += " and hs.TRANG_THAI_HOC in (1,2,3,8,9,10)";
                sql += @"and not(t.is_delete is not null and t.is_delete = 1) 
                    and not(hs.is_delete is not null and hs.is_delete = 1)
                    group by hs.id_truong, t.ten
                    order by t.ten";
                data = context.Database.SqlQuery<ThongKeHocSinhTheoTruongEntity>(sql).ToList();

            }
            return data;
        }
        #endregion
        #region set
        public ResultEntity update(HOC_SINH detail_in, long? nguoi)
        {
            LopBO lopBO = new LopBO();
            LOP lOPDetail = new LOP();
            HOC_SINH detail = new HOC_SINH();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {

                var QICache = new DefaultCacheProvider();
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.HOC_SINH where p.ID == detail_in.ID select p).FirstOrDefault();
                    if (detail != null)
                    {
                        using (DbContextTransaction transaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                detail.MA = detail_in.MA;
                                #region Chuyển lớp
                                if (detail.ID_LOP != detail_in.ID_LOP)
                                {
                                    lOPDetail = lopBO.getLopById(detail_in.ID_LOP);
                                    if (lOPDetail != null)
                                    {
                                        detail.ID_LOP = detail_in.ID_LOP;
                                        detail.ID_KHOI = lOPDetail.ID_KHOI;

                                        string sql = string.Format(@"Update CHUYEN_CAN set ID_LOP=:0, MA_KHOI=:1, NGUOI_SUA=:2, NGAY_SUA=:3 where ID_HOC_SINH=:4");
                                        context.Database.ExecuteSqlCommand(sql, detail_in.ID_LOP, lOPDetail.ID_KHOI, nguoi, DateTime.Now, detail.ID);
                                        sql = string.Format(@"Update DANH_GIA_DINH_KY_MON_TH set ID_LOP=:0, MA_KHOI=:1, NGUOI_SUA=:2, NGAY_SUA=:3 where ID_HOC_SINH=:4");
                                        context.Database.ExecuteSqlCommand(sql, detail_in.ID_LOP, lOPDetail.ID_KHOI, nguoi, DateTime.Now, detail.ID);
                                        sql = string.Format(@"Update DANH_GIA_DINH_KY_TH set ID_LOP=:0, MA_KHOI=:1, NGUOI_SUA=:2, NGAY_SUA=:3 where ID_HOC_SINH=:4");
                                        context.Database.ExecuteSqlCommand(sql, detail_in.ID_LOP, lOPDetail.ID_KHOI, nguoi, DateTime.Now, detail.ID);
                                        sql = string.Format(@"Update DIEM_CHI_TIET set ID_LOP=:0, MA_KHOI=:1, NGUOI_SUA=:2, NGAY_SUA=:3 where ID_HOC_SINH=:4");
                                        context.Database.ExecuteSqlCommand(sql, detail_in.ID_LOP, lOPDetail.ID_KHOI, nguoi, DateTime.Now, detail.ID);
                                        sql = string.Format(@"Update DIEM_TONG_KET set ID_LOP=:0, NGUOI_SUA=:1, NGAY_SUA=:2 where ID_HOC_SINH=:3");
                                        context.Database.ExecuteSqlCommand(sql, detail_in.ID_LOP, nguoi, DateTime.Now, detail.ID);
                                        sql = string.Format(@"Update NHAN_XET_HANG_NGAY set ID_LOP=:0, MA_KHOI=:1, NGUOI_SUA=:2, NGAY_SUA=:3 where ID_HOC_SINH=:4");
                                        context.Database.ExecuteSqlCommand(sql, detail_in.ID_LOP, lOPDetail.ID_KHOI, nguoi, DateTime.Now, detail.ID);

                                        QICache.RemoveByFirstName("CHUYEN_CAN");
                                        QICache.RemoveByFirstName("DANH_GIA_DINH_KY_MON_TH");
                                        QICache.RemoveByFirstName("DANH_GIA_DINH_KY_TH");
                                        QICache.RemoveByFirstName("DIEM_CHI_TIET");
                                        QICache.RemoveByFirstName("DIEM_TONG_KET");
                                        QICache.RemoveByFirstName("NHAN_XET_HANG_NGAY");
                                    }
                                }
                                #endregion
                                detail.TEN = detail_in.TEN;
                                detail.HO_DEM = detail_in.HO_DEM;
                                detail.HO_TEN = detail_in.HO_TEN;
                                detail.THU_TU = detail_in.THU_TU;
                                detail.NGAY_SINH = detail_in.NGAY_SINH;
                                detail.MA_GIOI_TINH = detail_in.MA_GIOI_TINH;
                                detail.HO_TEN_BO = detail_in.HO_TEN_BO;
                                detail.HO_TEN_ME = detail_in.HO_TEN_ME;
                                detail.HO_TEN_NGUOI_BAO_HO = detail_in.HO_TEN_NGUOI_BAO_HO;
                                detail.NAM_SINH_BO = detail_in.NAM_SINH_BO;
                                detail.NAM_SINH_ME = detail_in.NAM_SINH_ME;
                                detail.NAM_SINH_NGUOI_BAO_HO = detail_in.NAM_SINH_NGUOI_BAO_HO;
                                detail.SDT_BO = detail_in.SDT_BO;
                                detail.SDT_ME = detail_in.SDT_ME;
                                detail.SDT_NBH = detail_in.SDT_NBH;
                                detail.SDT_NHAN_TIN = detail_in.SDT_NHAN_TIN;
                                detail.TRANG_THAI_HOC = detail_in.TRANG_THAI_HOC;
                                if (detail.IS_DK_KY1 != detail_in.IS_DK_KY1)
                                {
                                    if (detail_in.IS_DK_KY1 != null && detail_in.IS_DK_KY1 == true) detail.NGAY_DK_KY1 = DateTime.Now;
                                    else detail.NGAY_HUY_KY1 = DateTime.Now;
                                }
                                if (detail.IS_DK_KY2 != detail_in.IS_DK_KY2)
                                {
                                    if (detail_in.IS_DK_KY2 != null && detail_in.IS_DK_KY2 == true) detail.NGAY_DK_KY2 = DateTime.Now;
                                    else detail.NGAY_HUY_KY2 = DateTime.Now;
                                }
                                detail.IS_DK_KY1 = detail_in.IS_DK_KY1;
                                detail.IS_DK_KY2 = detail_in.IS_DK_KY2;
                                detail.IS_MIEN_GIAM_KY1 = detail_in.IS_MIEN_GIAM_KY1;
                                detail.IS_MIEN_GIAM_KY2 = detail_in.IS_MIEN_GIAM_KY2;
                                detail.IS_GUI_BO_ME = detail_in.IS_GUI_BO_ME;
                                detail.IS_CON_GV = detail_in.IS_CON_GV;
                                detail.SDT_NHAN_TIN2 = detail_in.SDT_NHAN_TIN2;
                                detail.NGUOI_SUA = nguoi;
                                detail.NGAY_SUA = DateTime.Now;
                                detail.IS_DELETE = detail_in.IS_DELETE;
                                detail.DIA_CHI = detail_in.DIA_CHI;
                                detail.MA_KHU_VUC = detail_in.MA_KHU_VUC;
                                detail.MA_TINH_THANH = detail_in.MA_TINH_THANH;
                                detail.MA_QUAN_HUYEN = detail_in.MA_QUAN_HUYEN;
                                detail.MA_XA_PHUONG = detail_in.MA_XA_PHUONG;
                                detail.MA_DAN_TOC = detail_in.MA_DAN_TOC;
                                detail.MA_DOI_TUONG_CS = detail_in.MA_DOI_TUONG_CS;
                                detail.MA_QUOC_TICH = detail_in.MA_QUOC_TICH;
                                detail.NOI_SINH = detail_in.NOI_SINH;
                                detail.SO_CMND = detail_in.SO_CMND;
                                detail.NGAY_CAP_CMND = detail_in.NGAY_CAP_CMND;
                                detail.NOI_CAP_CMND = detail_in.NOI_CAP_CMND;
                                detail.CHIEU_CAO = detail_in.CHIEU_CAO;
                                detail.CAN_NANG = detail_in.CAN_NANG;
                                detail.ANH_DAI_DIEN = detail_in.ANH_DAI_DIEN;

                                detail.IS_HOI_PHO_CHPH = detail_in.IS_HOI_PHO_CHPH;
                                detail.IS_HOI_TRUONG_CHPH = detail_in.IS_HOI_TRUONG_CHPH;
                                detail.NGAY_GUI_OTP = detail_in.NGAY_GUI_OTP;
                                detail.OTP_COUNTER = detail_in.OTP_COUNTER;

                                context.SaveChanges();
                                res.ResObject = detail;

                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                res.Res = false;
                                res.Msg = "Có lỗi xãy ra";
                                //res.Msg = ex.ToString();
                            }
                        }
                    }
                }
                QICache.RemoveByFirstName("HOC_SINH");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
                //res.Msg = ex.ToString();
            }
            return res;
        }
        public string sinhMaHS(string id_hs, int soKyTu)
        {
            string ma_hs = "";
            #region Lấy số ký tự mã học sinh
            try
            {
                int count0 = 0;
                int countMa = id_hs.Length;
                string str0 = "";
                if (countMa < soKyTu)
                {
                    count0 = soKyTu - countMa;
                    for (int i = 0; i < count0; i++)
                    {
                        str0 += "0";
                    }
                }
                ma_hs = "HS" + str0 + id_hs;
                return ma_hs;
            }
            catch { }
            return ma_hs;
            #endregion
        }
        public ResultEntity insert(HOC_SINH detail_in, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    long newID = context.Database.SqlQuery<long>("SELECT HOC_SINH_SEQ2.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                    detail_in.ID = newID;
                    detail_in.MA = sinhMaHS(newID.ToString(), 10);
                    detail_in.NGAY_TAO = DateTime.Now;
                    detail_in.NGUOI_TAO = nguoi;
                    detail_in = context.HOC_SINH.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("HOC_SINH");
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
            var QICache = new DefaultCacheProvider();
            try
            {
                using (var context = new oneduEntities())
                {
                    if (is_delete_all)
                    {
                        var sql = @"delete DIEM_CHI_TIET where ID_HOC_SINH = :0";
                        context.Database.ExecuteSqlCommand(sql, id);
                        sql = @"delete DIEM_TONG_KET where ID_HOC_SINH = :0";
                        context.Database.ExecuteSqlCommand(sql, id);
                        sql = @"delete TONG_HOP_NHAN_XET_HANG_NGAY where ID_HOC_SINH = :0";
                        context.Database.ExecuteSqlCommand(sql, id);
                        sql = @"delete NHAN_XET_HANG_NGAY where ID_HOC_SINH = :0";
                        context.Database.ExecuteSqlCommand(sql, id);
                        sql = @"delete CHUYEN_CAN where ID_HOC_SINH = :0";
                        context.Database.ExecuteSqlCommand(sql, id);

                        sql = @"delete DANH_GIA_DINH_KY_TH where ID_HOC_SINH = :0";
                        context.Database.ExecuteSqlCommand(sql, id);
                        sql = @"delete DANH_GIA_DINH_KY_MON_TH where ID_HOC_SINH = :0";
                        context.Database.ExecuteSqlCommand(sql, id);

                        sql = @"delete CHAM_AN where ID_HOC_SINH = :0";
                        context.Database.ExecuteSqlCommand(sql, id);
                        sql = @"delete DANG_KY_AN where ID_HOC_SINH = :0";
                        context.Database.ExecuteSqlCommand(sql, id);

                        sql = @"delete from HOC_SINH where ID = :0";
                        context.Database.ExecuteSqlCommand(sql, id);

                        QICache.RemoveByFirstName("DIEM_CHI_TIET");
                        QICache.RemoveByFirstName("DIEM_TONG_KET");
                        QICache.RemoveByFirstName("NHAN_XET_HANG_NGAY");
                        QICache.RemoveByFirstName("TONG_HOP_NHAN_XET_HANG_NGAY");
                        QICache.RemoveByFirstName("CHUYEN_CAN");

                        QICache.RemoveByFirstName("DANH_GIA_DINH_KY_TH");
                        QICache.RemoveByFirstName("DANH_GIA_DINH_KY_MON_TH");

                        QICache.RemoveByFirstName("CHAM_AN");
                        QICache.RemoveByFirstName("DANG_KY_AN");
                    }
                    else if (!is_delete)
                    {
                        var sql = @"update HOC_SINH set IS_DELETE = 1,NGUOI_SUA=:0,NGAY_SUA=:1 where ID = :2";
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, id);
                    }
                    else
                    {
                        var sql = @"delete from HOC_SINH where ID = :0";
                        context.Database.ExecuteSqlCommand(sql, id);
                    }

                }
                QICache.RemoveByFirstName("HOC_SINH");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity updateChuyenLop(long id_hoc_sinh, long id_lop, long nguoi)
        {
            LopBO lopBO = new LopBO();
            LOP lOPDetail = new LOP();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {

                var QICache = new DefaultCacheProvider();
                using (var context = new oneduEntities())
                {
                    var detail = (from p in context.HOC_SINH where p.ID == id_hoc_sinh select p).FirstOrDefault();
                    if (detail != null)
                    {
                        lOPDetail = lopBO.getLopById(id_lop);
                        if (detail.ID_LOP != id_lop && lOPDetail != null)
                        {
                            using (DbContextTransaction transaction = context.Database.BeginTransaction())
                            {
                                try
                                {
                                    detail.ID_LOP = id_lop;
                                    context.SaveChanges();
                                    res.ResObject = detail;
                                    string sql = string.Format(@"Update CHUYEN_CAN set ID_LOP=:0, MA_KHOI=:1, NGUOI_SUA=:2, NGAY_SUA=:3 where ID_HOC_SINH=:4");
                                    context.Database.ExecuteSqlCommand(sql, id_lop, lOPDetail.ID_KHOI, nguoi, DateTime.Now, id_hoc_sinh);
                                    sql = string.Format(@"Update DANH_GIA_DINH_KY_MON_TH set ID_LOP=:0, MA_KHOI=:1, NGUOI_SUA=:2, NGAY_SUA=:3 where ID_HOC_SINH=:4");
                                    context.Database.ExecuteSqlCommand(sql, id_lop, lOPDetail.ID_KHOI, nguoi, DateTime.Now, id_hoc_sinh);
                                    sql = string.Format(@"Update DANH_GIA_DINH_KY_TH set ID_LOP=:0, MA_KHOI=:1, NGUOI_SUA=:2, NGAY_SUA=:3 where ID_HOC_SINH=:4");
                                    context.Database.ExecuteSqlCommand(sql, id_lop, lOPDetail.ID_KHOI, nguoi, DateTime.Now, id_hoc_sinh);
                                    sql = string.Format(@"Update DIEM_CHI_TIET set ID_LOP=:0, MA_KHOI=:1, NGUOI_SUA=:2, NGAY_SUA=:3 where ID_HOC_SINH=:4");
                                    context.Database.ExecuteSqlCommand(sql, id_lop, lOPDetail.ID_KHOI, nguoi, DateTime.Now, id_hoc_sinh);
                                    sql = string.Format(@"Update DIEM_TONG_KET set ID_LOP=:0, NGUOI_SUA=:1, NGAY_SUA=:2 where ID_HOC_SINH=:3");
                                    context.Database.ExecuteSqlCommand(sql, id_lop, nguoi, DateTime.Now, id_hoc_sinh);
                                    sql = string.Format(@"Update NHAN_XET_HANG_NGAY set ID_LOP=:0, MA_KHOI=:1, NGUOI_SUA=:2, NGAY_SUA=:3 where ID_HOC_SINH=:4");
                                    context.Database.ExecuteSqlCommand(sql, id_lop, lOPDetail.ID_KHOI, nguoi, DateTime.Now, id_hoc_sinh);
                                    sql = string.Format(@"Update TONG_HOP_NHAN_XET_HANG_NGAY set ID_LOP=:0, NGUOI_SUA=:1, NGAY_SUA=:2 where ID_HOC_SINH=:3");
                                    context.Database.ExecuteSqlCommand(sql, id_lop, nguoi, DateTime.Now, id_hoc_sinh);
                                    transaction.Commit();
                                    QICache.RemoveByFirstName("CHUYEN_CAN");
                                    QICache.RemoveByFirstName("DANH_GIA_DINH_KY_MON_TH");
                                    QICache.RemoveByFirstName("DANH_GIA_DINH_KY_TH");
                                    QICache.RemoveByFirstName("DIEM_CHI_TIET");
                                    QICache.RemoveByFirstName("DIEM_TONG_KET");
                                    QICache.RemoveByFirstName("NHAN_XET_HANG_NGAY");
                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    res.Res = false;
                                    res.Msg = "Có lỗi xãy ra";
                                }
                            }
                        }
                    }
                }
                QICache.RemoveByFirstName("HOC_SINH");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity updateDangKySMS(HOC_SINH detail_in, long? nguoi)
        {
            HOC_SINH detail = new HOC_SINH();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                var QICache = new DefaultCacheProvider();
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.HOC_SINH where p.ID == detail_in.ID select p).FirstOrDefault();
                    if (detail != null)
                    {
                        if (detail.IS_DK_KY1 != detail_in.IS_DK_KY1)
                        {
                            if (detail_in.IS_DK_KY1 != null && detail_in.IS_DK_KY1 == true) detail.NGAY_DK_KY1 = DateTime.Now;
                            else detail.NGAY_HUY_KY1 = DateTime.Now;
                        }
                        if (detail.IS_DK_KY2 != detail_in.IS_DK_KY2)
                        {
                            if (detail_in.IS_DK_KY2 != null && detail_in.IS_DK_KY2 == true) detail.NGAY_DK_KY2 = DateTime.Now;
                            else detail.NGAY_HUY_KY2 = DateTime.Now;
                        }
                        detail.IS_DK_KY1 = detail_in.IS_DK_KY1;
                        detail.IS_DK_KY2 = detail_in.IS_DK_KY2;
                        detail.THU_TU = detail_in.THU_TU;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                QICache.RemoveByFirstName("HOC_SINH");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
                //res.Msg = ex.ToString();
            }
            return res;
        }
        public ResultEntity ChuyenHocSinhLenLopMoi(long id_truong, string ma_cap_hoc, int id_nam_cu, int id_nam_moi, long id_lop_cu, long id_lop_moi, short id_khoi_moi, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    string sql = string.Format(@"
                    insert into hoc_sinh (Ma, id_lop, ten, ngay_sinh, ma_gioi_tinh, sdt_nhan_tin, trang_thai_hoc,is_dk_ky1, is_dk_ky2,
                    id_truong, id_khoi, ma_cap_hoc, id_nam_hoc, ho_dem, ho_ten, thu_tu, ngay_tao, nguoi_tao)
                    select Ma, :0, ten, ngay_sinh, ma_gioi_tinh, sdt_nhan_tin, trang_thai_hoc,is_dk_ky1, is_dk_ky2,
                    id_truong, {0}, ma_cap_hoc, :1, ho_dem, ho_ten, thu_tu, sysdate, :2 from hoc_sinh a
                    where id_truong=:3 and ma_cap_hoc=:4 and id_nam_hoc= :5 and id_lop=:6 and trang_thai_hoc in (1,2,3,6,7,10) 
                    and not (is_delete is not null and is_delete=1)
                    and not exists (select * from hoc_sinh b where b.id_truong=:7 and b.id_nam_hoc=:8 
                    and b.id_lop = :9 and a.Ma = b.ma)", id_khoi_moi);
                    res.ResObject = context.Database.ExecuteSqlCommand(sql, id_lop_moi, id_nam_moi, nguoi, id_truong, ma_cap_hoc, id_nam_cu, id_lop_cu, id_truong, id_nam_moi, id_lop_moi);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("HOC_SINH");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity ChuyenLenLopTheoHocSinh(List<HOC_SINH> lstHocSinh, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    foreach (var item in lstHocSinh)
                    {
                        item.NGAY_TAO = DateTime.Now;
                        item.NGUOI_TAO = nguoi;
                        item.ID = context.Database.SqlQuery<long>("SELECT HOC_SINH_SEQ2.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                    }
                    context.HOC_SINH.AddRange(lstHocSinh);
                    context.SaveChanges();
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("HOC_SINH");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        #endregion

        #region get ds gui tin thong bao
        public List<HocSinhLopEntity> getHocSinhGuiTinThongBao(long id_truong, string cap_hoc, short id_nam_hoc, short id_khoi, long id_lop, short hoc_ky)
        {
            DataAccessAPI dataAccessAPI = new DataAccessAPI();
            List<HocSinhLopEntity> data = new List<HocSinhLopEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_SINH", "LOP", "TIN_NHAN", "getHocSinhGuiTinThongBao", id_truong, cap_hoc, id_nam_hoc, id_khoi, id_lop, hoc_ky);
            int nam_gui = DateTime.Now.Year;
            int thang_gui = DateTime.Now.Month;
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    try
                    {
                        string strQuery = string.Format(@"select hs.ID
                                                ,hs.HO_TEN,hs.HO_DEM
                                                ,hs.TEN
                                                ,hs.SDT_NHAN_TIN
                                                ,hs.SDT_NHAN_TIN2
                                                ,hs.NGAY_SINH
                                                ,hs.IS_GUI_BO_ME
                                                ,hs.ID_LOP
                                                ,hs.THU_TU
                                                ,hs.IS_DK_KY1, hs.IS_DK_KY2
                                                ,hs.IS_MIEN_GIAM_KY1, hs.IS_MIEN_GIAM_KY2, hs.IS_CON_GV
                                                ,l.TEN as TEN_LOP
                                               -- ,case when sum(hs.is_gui_bo_me)>0 then 2 else 1 end as HE_SO
                                                ,case when sum(tn.so_tin)>0 then sum(tn.so_tin) else 0 end as SO_TIN_TRONG_NGAY
                                            from HOC_SINH hs
                                            inner join LOP l on HS.ID_TRUONG = L.ID_TRUONG and hs.id_khoi = l.id_khoi AND HS.id_nam_hoc = L.ID_NAM_HOC AND hs.ID_LOP = l.ID
                                            left join TIN_NHAN tn on hs.id_truong = tn.id_truong and tn.nam_gui={0} and tn.thang_gui={1} and hs.ID=tn.id_nguoi_nhan and tn.loai_nguoi_nhan=1 and tn.loai_tin = 2
                                            and to_char(tn.ngay_tao, 'YYYYMMDD')=:0
                                            where hs.id_truong= :1 and hs.MA_CAP_HOC = :2 and hs.ID_KHOI=:3 and hs.id_nam_hoc=:4 and hs.ID_LOP=:5 and not (hs.is_delete is not null and hs.is_delete=1)", nam_gui, thang_gui);

                        if (hoc_ky == 1)
                        {
                            strQuery += @" and hs.TRANG_THAI_HOC in (1,2,3,8,9,10)";
                            strQuery += @" and ((hs.is_dk_ky1 is not null and hs.is_dk_ky1 = 1) or (hs.is_mien_giam_ky1 is not null and hs.is_mien_giam_ky1 = 1))";
                        }
                        else if (hoc_ky == 2)
                        {
                            strQuery += @" and hs.TRANG_THAI_HOC in (1,2,3,6,7,10)";
                            strQuery += @" and ((hs.is_dk_ky2 is not null and hs.is_dk_ky2 = 1) or (hs.is_mien_giam_ky2 is not null and hs.is_mien_giam_ky2 = 1))";
                        }
                        strQuery += @" group by hs.ID
                                            ,hs.HO_TEN,hs.HO_DEM
                                            ,hs.TEN
                                            ,hs.SDT_NHAN_TIN
                                            ,hs.SDT_NHAN_TIN2
                                            ,hs.NGAY_SINH
                                            ,hs.IS_GUI_BO_ME
                                            ,hs.ID_LOP
                                            ,hs.THU_TU
                                            ,hs.IS_DK_KY1, hs.IS_DK_KY2
                                            ,hs.IS_MIEN_GIAM_KY1, hs.IS_MIEN_GIAM_KY2, hs.IS_CON_GV
                                            ,l.TEN
                                            order by l.TEN, nvl(hs.THU_TU, 0), NLSSORT(hs.ten,'NLS_SORT=vietnamese'), NLSSORT(hs.ho_dem,'NLS_SORT=vietnamese')";
                        data = context.Database.SqlQuery<HocSinhLopEntity>(strQuery, DateTime.Now.ToString("yyyyMMdd"), id_truong, cap_hoc, id_khoi, id_nam_hoc, id_lop).ToList();

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
                    data = QICache.Get(strKeyCache) as List<HocSinhLopEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<HocSinhLopEntity> getHocSinhToanTruongGuiThongBao(long id_truong, string cap_hoc, short id_nam_hoc, short hoc_ky)
        {
            List<HocSinhLopEntity> data = new List<HocSinhLopEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_SINH", "LOP", "getHocSinhToanTruongGuiThongBao", id_truong, cap_hoc, id_nam_hoc, hoc_ky);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    try
                    {
                        string strQuery = string.Format(@"select hs.ID
                                                ,hs.HO_TEN,hs.HO_DEM
                                                ,hs.TEN
                                                ,hs.SDT_NHAN_TIN
                                                ,hs.SDT_NHAN_TIN2
                                                ,hs.NGAY_SINH
                                                ,hs.IS_GUI_BO_ME
                                                ,hs.ID_LOP,hs.id_khoi
                                                ,hs.THU_TU
                                                ,hs.IS_DK_KY1, hs.IS_DK_KY2
                                                ,hs.IS_MIEN_GIAM_KY1, hs.IS_MIEN_GIAM_KY2, hs.IS_CON_GV
                                               -- ,l.TEN as TEN_LOP
                                            from HOC_SINH hs
                                          --  inner join LOP l on HS.ID_TRUONG = L.ID_TRUONG and hs.id_khoi = l.id_khoi AND HS.id_nam_hoc = L.ID_NAM_HOC AND hs.ID_LOP = l.ID
                                            where hs.id_truong={0} and hs.MA_CAP_HOC='{1}' and hs.id_nam_hoc={2} and not (hs.is_delete is not null and hs.is_delete=1)", id_truong, cap_hoc, id_nam_hoc);

                        if (hoc_ky == 1) strQuery += @" and hs.TRANG_THAI_HOC in (1,2,3,8,9,10) and (hs.IS_DK_KY1 is not null and hs.IS_DK_KY1 = 1)";
                        else if (hoc_ky == 2) strQuery += @" and hs.TRANG_THAI_HOC in (1,2,3,6,7,10) and (hs.IS_DK_KY2 is not null and hs.IS_DK_KY2 = 1)";
                        //strQuery += @" order by l.TEN, hs.THU_TU,NLSSORT(hs.ten,'NLS_SORT=vietnamese'),NLSSORT(hs.ho_dem,'NLS_SORT=vietnamese')";
                        strQuery += @" order by hs.id_khoi, hs.id_lop, hs.THU_TU,NLSSORT(hs.ten,'NLS_SORT=vietnamese'),NLSSORT(hs.ho_dem,'NLS_SORT=vietnamese')";
                        data = context.Database.SqlQuery<HocSinhLopEntity>(strQuery).ToList();

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
                    data = QICache.Get(strKeyCache) as List<HocSinhLopEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<HocSinhLopEntity> getHocSinhGuiThongBaoTheoKhoiLop(long id_truong, List<short> lst_ma_khoi, List<long> lst_id_lop, short id_nam_hoc, string cap_hoc, short hoc_ky)
        {
            DataAccessAPI dataAccessAPI = new DataAccessAPI();
            List<HocSinhLopEntity> data = new List<HocSinhLopEntity>();
            var QICache = new DefaultCacheProvider();
            string str_lst_ma_khoi = "", str_lst_id_lop = "";
            str_lst_ma_khoi = dataAccessAPI.ConvertListToString<short>(lst_ma_khoi, ",");
            str_lst_id_lop = dataAccessAPI.ConvertListToString<long>(lst_id_lop, ",");
            string strKeyCache = QICache.BuildCachedKey("HOC_SINH", "LOP", "TIN_NHAN", "getHocSinhGuiThongBaoTheoKhoiLop", id_truong, str_lst_ma_khoi, str_lst_id_lop, id_nam_hoc, cap_hoc, hoc_ky);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    try
                    {
                        if (lst_ma_khoi != null && lst_ma_khoi.Count > 0 && lst_id_lop != null && lst_id_lop.Count > 0)
                        {
                            string strQuery = string.Format(@"select hs.ID
                                                ,hs.HO_TEN,hs.HO_DEM
                                                ,hs.TEN
                                                ,hs.SDT_NHAN_TIN
                                                ,hs.SDT_NHAN_TIN2
                                                ,hs.NGAY_SINH
                                                ,hs.IS_GUI_BO_ME
                                                ,hs.ID_LOP
                                                ,hs.THU_TU
                                                ,hs.IS_DK_KY1, hs.IS_DK_KY2
                                                ,hs.IS_MIEN_GIAM_KY1, hs.IS_MIEN_GIAM_KY2, hs.IS_CON_GV
                                                ,l.TEN as TEN_LOP
                                            from HOC_SINH hs
                                            inner join LOP l on HS.ID_TRUONG = L.ID_TRUONG AND HS.id_nam_hoc = L.ID_NAM_HOC AND hs.ID_LOP = l.ID
                                            where hs.id_truong={0} and hs.MA_CAP_HOC='{1}' and hs.id_nam_hoc={2} and not (hs.is_delete is not null and hs.is_delete=1)", id_truong, cap_hoc, id_nam_hoc);
                            #region Khối
                            if (lst_ma_khoi.Count == 1)
                                strQuery += string.Format(@" and hs.ID_KHOI ={0}", lst_ma_khoi[0]);
                            else if (lst_ma_khoi.Count > 1)
                            {
                                strQuery += string.Format(" and not ( hs.ID_KHOI !={0}", lst_ma_khoi[0]);
                                for (int i = 1; i < lst_ma_khoi.Count; i++)
                                {
                                    strQuery += string.Format(" and hs.ID_KHOI !={0}", lst_ma_khoi[i]);
                                }
                                strQuery += " )";
                            }
                            #endregion
                            #region Lớp
                            if (lst_id_lop.Count == 1)
                                strQuery += string.Format(@" and hs.ID_LOP ={0}", lst_id_lop[0]);
                            else if (lst_id_lop.Count > 1)
                            {
                                strQuery += string.Format(" and not ( hs.ID_LOP !={0}", lst_id_lop[0]);
                                for (int i = 1; i < lst_id_lop.Count; i++)
                                {
                                    strQuery += string.Format(" and hs.ID_LOP !={0}", lst_id_lop[i]);
                                }
                                strQuery += " )";
                            }
                            #endregion

                            if (hoc_ky == 1) strQuery += @" and hs.TRANG_THAI_HOC in (1,2,3,8,9,10) and (hs.IS_DK_KY1 is not null and hs.IS_DK_KY1 = 1)";
                            else if (hoc_ky == 2) strQuery += @" and hs.TRANG_THAI_HOC in (1,2,3,6,7,10) and (hs.IS_DK_KY1 is not null and hs.IS_DK_KY1 = 1)";
                            strQuery += @" order by l.TEN, hs.THU_TU,NLSSORT(hs.ten,'NLS_SORT=vietnamese'),NLSSORT(hs.ho_dem,'NLS_SORT=vietnamese')";
                            data = context.Database.SqlQuery<HocSinhLopEntity>(strQuery, DateTime.Now, id_truong, id_nam_hoc, cap_hoc).ToList();
                            QICache.Set(strKeyCache, data, 300000);
                        }
                    }
                    catch (Exception ex)
                    { }
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<HocSinhLopEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        #endregion

        public List<HocSinhEntity> getHocSinhByPhoneDangKyZalo(string phone, short id_nam_hoc, string maHocSinh)
        {
            List<HocSinhEntity> data = new List<HocSinhEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_SINH", "getHocSinhByPhoneDangKyZalo", phone, id_nam_hoc, maHocSinh);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string tmp = string.Format(@"select * from HOC_SINH where ID_NAM_HOC={0} and SDT_NHAN_TIN = '{1}'", id_nam_hoc, phone);
                    if (!string.IsNullOrEmpty(maHocSinh))
                        tmp += string.Format(@" and MA not in ({0})", maHocSinh);
                    tmp += " and (is_delete is null or is_delete=0)";
                    data = context.Database.SqlQuery<HocSinhEntity>(tmp).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<HocSinhEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }

        public void ghilog(string nameFile, string msg)
        {
            try
            {
                string path = "";
                DateTime dt = new DateTime();
                dt = DateTime.Now;
                string foldername = dt.ToString();
                path = "D:/LogEduZalo/" + dt.Year.ToString() + dt.Month.ToString() + dt.Day.ToString() + dt.Hour.ToString() + nameFile + ".txt";
                using (StreamWriter sw = new StreamWriter(path, true))
                {
                    string str = msg;
                    sw.WriteLine(str);
                }
            }
            catch
            {

            }
        }
    }
}
