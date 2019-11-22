using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Oracle.DataAccess.Client;
//Oracle.DataAccess.Client;
namespace OneEduDataAccess.BO
{
    public class DiemTongKetBO
    {
        #region get
        public List<DIEM_TONG_KET> getDiemTongKet()
        {
            List<DIEM_TONG_KET> data = new List<DIEM_TONG_KET>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DIEM_TONG_KET", "getDiemTongKet");
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DIEM_TONG_KET where p.IS_DELETE != true select p).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<DIEM_TONG_KET>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<DIEM_TONG_KET> getDiemTongKetByLop(long id_lop)
        {
            List<DIEM_TONG_KET> data = new List<DIEM_TONG_KET>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DIEM_TONG_KET", "getDiemTongKetByLop", id_lop);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DIEM_TONG_KET where p.IS_DELETE != true && p.ID_LOP == id_lop select p).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<DIEM_TONG_KET>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public DIEM_TONG_KET getDiemTKByID(long id)
        {
            DIEM_TONG_KET data = new DIEM_TONG_KET();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DIEM_TONG_KET", "getDiemTKByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DIEM_TONG_KET where p.ID == id select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as DIEM_TONG_KET;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<DiemTongKetEntity> getDiemTongKetByTruongKhoiLop(long id_truong, short? ma_khoi, long? id_lop, short id_nam_hoc)
        {
            DataAccessAPI dataAccessAPI = new DataAccessAPI();
            List<DiemTongKetEntity> data = new List<DiemTongKetEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DIEM_TONG_KET", "HOC_SINH", "getDiemTongKetByTruongKhoiLop", id_nam_hoc, id_truong, ma_khoi, id_lop);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = @"select d.*,h.MA as MA_HS,h.HO_TEN as TEN_HS,h.NGAY_SINH 
                                        from DIEM_TONG_KET d
                                        join HOC_SINH h on h.ID_TRUONG=d.ID_TRUONG and d.ID_HOC_SINH =h.ID and d.ID_LOP=h.ID_LOP
                                        where not ( d.IS_DELETE is not null and d.IS_DELETE =1 ) 
                                            and not ( h.IS_DELETE is not null and h.IS_DELETE =1 ) 
                                            and d.ID_NAM_HOC=:0 and d.ID_TRUONG=:1
                                        ";
                    List<object> parameterList = new List<object>();
                    parameterList.Add(id_nam_hoc);
                    parameterList.Add(id_truong);
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
                    strQuery += " order by nvl(h.THU_TU, 0),NLSSORT(h.ten,'NLS_SORT=vietnamese'),NLSSORT(h.ho_dem,'NLS_SORT=vietnamese')";
                    data = context.Database.SqlQuery<DiemTongKetEntity>(strQuery, parameterList.ToArray()).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<DiemTongKetEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<DiemTongKetHocKyEntity> getDiemTongKetHocKyByTruongLop(long id_truong, long id_lop, short id_nam_hoc)
        {
            DataAccessAPI dataAccessAPI = new DataAccessAPI();
            List<DiemTongKetHocKyEntity> data = new List<DiemTongKetHocKyEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DIEM_TONG_KET", "HOC_SINH", "getDiemTongKetByTruongKhoiLop", id_nam_hoc, id_truong, id_lop);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = @"select id_hoc_sinh, hoc_ky
                    ,case when hoc_ky=1 and SUM(count1)> 0 then sum(tongDiemKy1)/sum(count1) else null end as diem_TB_Ky1
                    ,case when hoc_ky=2 and SUM(count2)> 0 then sum(tongDiemKy2)/sum(count2) else null end as diem_TB_Ky2
                    from (select d.id_hoc_sinh, d.hoc_ky,
                    case when d.hoc_ky=1 then sum(mt.he_so*d.DIEM_TRUNG_BINH_KY1) else 0 end as tongDiemKy1
                    ,case when d.hoc_ky=2 then sum(mt.he_so*d.DIEM_TRUNG_BINH_KY2) else 0 end as tongDiemKy2
                    ,case when d.hoc_ky=1 and d.DIEM_TRUNG_BINH_KY1 is not null and d.DIEM_TRUNG_BINH_KY1 > 0 then sum(mt.he_so) else 0 end as count1
                    ,case when d.hoc_ky=2 and d.DIEM_TRUNG_BINH_KY2 is not null and d.DIEM_TRUNG_BINH_KY2 > 0 then sum(mt.he_so) else 0 end as count2
                    from diem_chi_tiet d
                    left join mon_hoc_truong mt on d.ID_MON_HOC_TRUONG = mt.id and mt.ID_TRUONG=d.ID_TRUONG and d.ID_NAM_HOC=d.ID_NAM_HOC
                    where d.id_truong=:0 and d.id_lop=:1 and d.id_nam_hoc=:2 and not (kieu_mon is not null and kieu_mon=1)
                    group by d.id_hoc_sinh, d.hoc_ky,DIEM_TRUNG_BINH_KY2,DIEM_TRUNG_BINH_KY1) tbl
                    group by id_hoc_sinh, hoc_ky
                    order by id_hoc_sinh, hoc_ky
                                        ";
                    List<object> parameterList = new List<object>();
                    parameterList.Add(id_truong);
                    parameterList.Add(id_lop);
                    parameterList.Add(id_nam_hoc);
                    data = context.Database.SqlQuery<DiemTongKetHocKyEntity>(strQuery, parameterList.ToArray()).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<DiemTongKetHocKyEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<TongKetDiemEntity> viewTongKetDiem(long id_truong, long id_lop, short id_nam_hoc, short hoc_ky, List<DanhSachMonByLopEntity> lstMonLop)
        {
            List<TongKetDiemEntity> data = new List<TongKetDiemEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_SINH", "DIEM_CHI_TIET", "MON_HOC_TRUONG", "DIEM_TONG_KET", "viewTongKetDiem", id_truong, id_lop, id_nam_hoc, hoc_ky, lstMonLop);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    #region "load môn chỉ học kỳ 1 hoặc 2"
                    if (hoc_ky == 1 || hoc_ky == 2)
                    {
                        if (lstMonLop != null && lstMonLop.Count > 0)
                        {
                            string queryMon = @"select hs.ID,hs.MA,hs.HO_TEN,hs.TEN,hs.THU_TU";
                            for (int i = 0; i < lstMonLop.Count; i++)
                            {
                                queryMon += ",sum(case when d.ID_MON_HOC_TRUONG=" + lstMonLop[i].ID_MON_TRUONG + " then d.DIEM_TRUNG_BINH_KY1 else NULL end ) as MON_" + i + "_Ky1";
                                queryMon += ",sum(case when d.ID_MON_HOC_TRUONG=" + lstMonLop[i].ID_MON_TRUONG + " then d.DIEM_TRUNG_BINH_KY2 else NULL end ) as MON_" + i + "_Ky2";
                                queryMon += ",sum(case when d.ID_MON_HOC_TRUONG=" + lstMonLop[i].ID_MON_TRUONG + " then d.DIEM_TRUNG_BINH_CN else NULL end ) as MON_" + i + "_CN";
                                queryMon += ",sum(case when d.ID_MON_HOC_TRUONG=" + lstMonLop[i].ID_MON_TRUONG + " and d.hoc_ky=" + lstMonLop[i].HOC_KY + " then " + lstMonLop[i].KIEU_MON + " else NULL end ) as KIEU_MON_" + i;
                                queryMon += ",sum(case when d.ID_MON_HOC_TRUONG=" + lstMonLop[i].ID_MON_TRUONG + " and d.hoc_ky=" + lstMonLop[i].HOC_KY + " then " + lstMonLop[i].HE_SO + " else NULL end ) as HE_SO_" + i;
                                queryMon += ",sum(case when d.ID_MON_HOC_TRUONG=" + lstMonLop[i].ID_MON_TRUONG + " then " + lstMonLop[i].MON_CHUYEN + " else 0 end) as IS_MON_CHUYEN_" + i;
                            }
                            queryMon += @",dtk.TB_KY1,dtk.TB_KY2,dtk.TB_CN 
                                        from Hoc_Sinh hs 
                                        left join (select dct.*, m.kieu_mon, m.he_so from DIEM_CHI_TIET dct join mon_hoc_truong m on m.id=dct.id_mon_hoc_truong and m.ID_TRUONG=dct.ID_TRUONG and m.ID_NAM_HOC=dct.ID_NAM_HOC) d on hs.ID = d.ID_HOC_SINH and hs.ID_LOP = d.ID_LOP and d.HOC_KY=:0
                                        left join diem_tong_ket dtk on dtk.id_hoc_sinh = hs.id 
                                        and hs.id_lop = dtk.id_lop and hs.id_truong=dtk.id_truong and hs.ID_NAM_HOC=dtk.ID_NAM_HOC
                                        where hs.ID_TRUONG=:1 and hs.ID_NAM_HOC=:2 and hs.ID_LOP=:3
                                        group by hs.TEN,hs.HO_TEN,hs.ID,hs.MA,hs.HO_TEN,hs.THU_TU,dtk.TB_KY1,dtk.TB_KY2,dtk.TB_CN 
                                        order by nvl(hs.THU_TU, 0), NLSSORT(hs.ten,'NLS_SORT=vietnamese')";
                            List<object> parameterList = new List<object>();
                            parameterList.Add(hoc_ky);
                            parameterList.Add(id_truong);
                            parameterList.Add(id_nam_hoc);
                            parameterList.Add(id_lop);
                            data = context.Database.SqlQuery<TongKetDiemEntity>(queryMon, parameterList.ToArray()).ToList();
                            QICache.Set(strKeyCache, data, 300000);
                        }
                    }
                    #endregion
                    #region "hiển thị all môn kỳ 2 và những môn chỉ học ở kỳ 1"
                    else if (hoc_ky == 3)
                    {
                        if (lstMonLop != null && lstMonLop.Count > 0)
                        {
                            string queryMon = @"select hs.ID,hs.MA,hs.HO_TEN,hs.TEN,hs.THU_TU";
                            for (int i = 0; i < lstMonLop.Count; i++)
                            {
                                queryMon += ",sum(case when d.ID_MON_HOC_TRUONG=" + lstMonLop[i].ID_MON_TRUONG + " and d.hoc_ky=1 then d.DIEM_TRUNG_BINH_KY1 else NULL end ) as MON_" + i + "_Ky1";
                                queryMon += ",sum(case when d.ID_MON_HOC_TRUONG=" + lstMonLop[i].ID_MON_TRUONG + " then d.DIEM_TRUNG_BINH_KY2 else NULL end ) as MON_" + i + "_Ky2";
                                queryMon += ",sum(case when d.ID_MON_HOC_TRUONG=" + lstMonLop[i].ID_MON_TRUONG + " and d.hoc_ky=" +lstMonLop[i].HOC_KY+ " then d.DIEM_TRUNG_BINH_CN else NULL end ) as MON_" + i + "_CN";
                                queryMon += ",sum(case when d.ID_MON_HOC_TRUONG=" + lstMonLop[i].ID_MON_TRUONG + " and d.hoc_ky=" + lstMonLop[i].HOC_KY + " then " + lstMonLop[i].KIEU_MON + " else NULL end ) as KIEU_MON_" + i;
                                queryMon += ",sum(case when d.ID_MON_HOC_TRUONG=" + lstMonLop[i].ID_MON_TRUONG + " and d.hoc_ky=" + lstMonLop[i].HOC_KY + " then " + lstMonLop[i].HE_SO +" else NULL end ) as HE_SO_" + i;
                                queryMon += ",sum(case when d.ID_MON_HOC_TRUONG=" + lstMonLop[i].ID_MON_TRUONG + " and d.hoc_ky=" + lstMonLop[i].HOC_KY + " then " + lstMonLop[i].MON_CHUYEN + " else 0 end) as IS_MON_CHUYEN_" + i;
                            }
                            queryMon += @",dtk.TB_KY1,dtk.TB_KY2,dtk.TB_CN 
                                    from Hoc_Sinh hs 
                                        left join (select dct.*, m.kieu_mon, m.he_so from DIEM_CHI_TIET dct join MON_HOC_TRUONG m on m.id=dct.id_mon_hoc_truong and m.ID_TRUONG=dct.ID_TRUONG and m.ID_NAM_HOC=dct.ID_NAM_HOC) d on hs.ID = d.ID_HOC_SINH and hs.ID_LOP = d.ID_LOP
                                        left join diem_tong_ket dtk on dtk.id_hoc_sinh = hs.id   
                                        and hs.id_lop = dtk.id_lop and hs.id_truong=dtk.id_truong and hs.ID_NAM_HOC=dtk.ID_NAM_HOC
                                        where hs.ID_TRUONG=:0 and hs.ID_NAM_HOC=:1 and hs.ID_LOP=:2
                                        group by hs.ID,hs.MA,hs.HO_TEN,hs.TEN,hs.THU_TU,dtk.TB_KY1,dtk.TB_KY2,dtk.TB_CN 
                                        order by nvl(hs.THU_TU, 0), NLSSORT(hs.ten,'NLS_SORT=vietnamese')";
                            List<object> parameterList = new List<object>();
                            parameterList.Add(id_truong);
                            parameterList.Add(id_nam_hoc);
                            parameterList.Add(id_lop);
                            data = context.Database.SqlQuery<TongKetDiemEntity>(queryMon, parameterList.ToArray()).ToList();
                            QICache.Set(strKeyCache, data, 300000);
                        }
                    }
                    #endregion
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<TongKetDiemEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public DIEM_TONG_KET getDiemTrungBinhByHocSinh(long id_truong, short id_nam_hoc, long id_lop, long id_hoc_sinh)
        {
            DIEM_TONG_KET data = new DIEM_TONG_KET();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DIEM_TONG_KET", "getDiemTrungBinhByHocSinh", id_truong, id_nam_hoc, id_lop, id_hoc_sinh);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DIEM_TONG_KET where p.ID_TRUONG == id_truong && p.ID_NAM_HOC == id_nam_hoc && p.ID_LOP == id_lop && p.ID_HOC_SINH == id_hoc_sinh && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as DIEM_TONG_KET;
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
        public ResultEntity update(DIEM_TONG_KET detail_in, long? nguoi)
        {
            DIEM_TONG_KET detail = new DIEM_TONG_KET();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.DIEM_TONG_KET
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.ID_HOC_SINH = detail_in.ID_HOC_SINH;
                        detail.ID_LOP = detail_in.ID_LOP;
                        detail.ID_TRUONG = detail_in.ID_TRUONG;
                        detail.TB_KY1 = detail_in.TB_KY1;
                        detail.TB_KY2 = detail_in.TB_KY2;
                        detail.TB_CN = detail_in.TB_CN;
                        detail.TB_CN_TTL = detail_in.TB_CN_TTL;
                        detail.MA_HOC_LUC_KY1 = detail_in.MA_HOC_LUC_KY1;
                        detail.MA_HOC_LUC_KY2 = detail_in.MA_HOC_LUC_KY2;
                        detail.MA_HOC_LUC_CA_NAM = detail_in.MA_HOC_LUC_CA_NAM;
                        detail.MA_HOC_LUC_CA_NAM_TTL = detail_in.MA_HOC_LUC_CA_NAM_TTL;
                        detail.MA_HANH_KIEM_KY1 = detail_in.MA_HANH_KIEM_KY1;
                        detail.MA_HANH_KIEM_KY2 = detail_in.MA_HANH_KIEM_KY2;
                        detail.MA_HANH_KIEM_CA_NAM = detail_in.MA_HANH_KIEM_CA_NAM;
                        detail.MA_HANH_KIEM_CA_NAM_TTL = detail_in.MA_HANH_KIEM_CA_NAM_TTL;
                        detail.MA_DANH_HIEU_KY1 = detail_in.MA_DANH_HIEU_KY1;
                        detail.MA_DANH_HIEU_KY2 = detail_in.MA_DANH_HIEU_KY2;
                        detail.MA_DANH_HIEU_CN = detail_in.MA_DANH_HIEU_CN;
                        detail.IS_LEN_LOP = detail_in.IS_LEN_LOP;
                        detail.IS_TOT_NGHIEP = detail_in.IS_TOT_NGHIEP;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        detail.IS_DELETE = detail_in.IS_DELETE;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DIEM_TONG_KET");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
                //res.Msg = ex.ToString();
            }
            return res;
        }
        public ResultEntity updateHanhKiem(DIEM_TONG_KET detail_in, long? nguoi, short hoc_ky)
        {
            DIEM_TONG_KET detail = new DIEM_TONG_KET();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.DIEM_TONG_KET
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.MA_HANH_KIEM_KY1 = detail_in.MA_HANH_KIEM_KY1;
                        detail.MA_HANH_KIEM_KY2 = detail_in.MA_HANH_KIEM_KY2;
                        detail.MA_HANH_KIEM_CA_NAM = detail_in.MA_HANH_KIEM_CA_NAM;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        detail.IS_DELETE = detail_in.IS_DELETE;
                        if (hoc_ky == 1 && detail.MA_HOC_LUC_KY1 != null && detail.MA_HOC_LUC_KY1 > 0)
                        {
                            if (detail_in.MA_HANH_KIEM_KY1 != null && detail_in.MA_HANH_KIEM_KY1 == 1 && detail.MA_HOC_LUC_KY1 == 1)
                                detail.MA_DANH_HIEU_KY1 = 1;
                            else if (detail_in.MA_HANH_KIEM_KY1 != null && detail.MA_HANH_KIEM_KY1 > 0 && detail_in.MA_HANH_KIEM_KY1 < 3 && detail.MA_HOC_LUC_KY1 < 3)
                                detail.MA_DANH_HIEU_KY1 = 2;
                        }
                        if (hoc_ky == 2 && detail.MA_HOC_LUC_KY2 != null && detail.MA_HOC_LUC_KY2 > 0)
                        {
                            if (detail_in.MA_HANH_KIEM_KY2 != null && detail_in.MA_HANH_KIEM_KY2 == 1 && detail.MA_HOC_LUC_KY2 == 1)
                                detail.MA_DANH_HIEU_KY2 = 1;
                            else if (detail_in.MA_HANH_KIEM_KY2 != null && detail.MA_HANH_KIEM_KY2 > 0 && detail_in.MA_HANH_KIEM_KY2 < 3 && detail.MA_HOC_LUC_KY2 < 3)
                                detail.MA_DANH_HIEU_KY2 = 2;
                            //Danh hiệu HSG: hạnh kiểm tốt + học lực giỏi
                            //Danh hiệu HSTT: hạnh kiểm khá trở lên + học lực khá trở lên
                            if (detail_in.MA_HANH_KIEM_CA_NAM != null && detail_in.MA_HANH_KIEM_CA_NAM > 0)
                            {
                                if (detail.MA_HOC_LUC_CA_NAM == 1 && detail_in.MA_HANH_KIEM_CA_NAM == 1) detail.MA_DANH_HIEU_CN = 1;
                                else if ((detail.MA_HOC_LUC_CA_NAM == 1 || detail.MA_HOC_LUC_CA_NAM == 2) && (detail_in.MA_HANH_KIEM_CA_NAM == 1 || detail_in.MA_HANH_KIEM_CA_NAM == 2)) detail.MA_DANH_HIEU_CN = 2;
                            }
                        }
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DIEM_TONG_KET");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
                //res.Msg = ex.ToString();
            }
            return res;
        }
        public ResultEntity insert(DIEM_TONG_KET detail_in, long? nguoi)
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
                    detail_in = context.DIEM_TONG_KET.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DIEM_TONG_KET");
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
                    var sql = @"update DIEM_TONG_KET set IS_DELETE = 1,NGUOI_SUA=:0,NGAY_SUA=:1 where ID = :2";
                    context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, id);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DIEM_TONG_KET");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity UpdateChuyenLop(long id_hoc_sinh, long id_lop, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    string sql = string.Format(@"Update DIEM_TONG_KET set ID_LOP=:0 NGUOI_SUA=:1, NGAY_SUA=:2 where ID_HOC_SINH=:3");
                    context.Database.ExecuteSqlCommand(sql, id_lop, nguoi, DateTime.Now, id_hoc_sinh);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DIEM_TONG_KET");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insertEmpty(long id_truong, short ma_nam_hoc, long id_lop)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    string strQuery = string.Format(@"insert into DIEM_TONG_KET (ID_HOC_SINH,ID_LOP,ID_TRUONG,ID_NAM_HOC)
                        select HOC_SINH.ID as id_hoc_sinh,:0 as ID_LOP, :1 as id_truong, :2 as id_nam_hoc
                        from HOC_SINH
                        left join DIEM_TONG_KET d on HOC_SINH.ID_LOP=d.ID_LOP and HOC_SINH.ID=d.ID_HOC_SINH and d.ID_NAM_HOC=:2
                        and not ( d.IS_DELETE is not null and d.IS_DELETE =1 )
                        where HOC_SINH.ID_LOP=:0  and not ( HOC_SINH.IS_DELETE is not null and HOC_SINH.IS_DELETE =1 )
                        and not exists (select * from DIEM_TONG_KET dtk 
                                      where dtk.ID_HOC_SINH=HOC_SINH.ID and dtk.id_truong=:1 and dtk.ID_LOP=:0 and dtk.ID_NAM_HOC=:2)");
                    context.Database.ExecuteSqlCommand(strQuery, id_lop, id_truong, ma_nam_hoc);
                    //#region "cập nhật điểm tổng kết, học lực, danh hiệu"
                    //List<DiemTongKetHocKyEntity> lstTongKet = getDiemTongKetHocKyByTruongLop(id_truong, id_lop, ma_nam_hoc);
                    //if (lstTongKet != null && lstTongKet.Count > 0)
                    //{
                    //    for(int i = 0; i < lstTongKet.Count; i++)
                    //    {

                    //    }
                    //}
                    //#endregion
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DIEM_TONG_KET");
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
