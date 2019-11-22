using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class SuatAnChiTietBO
    {
        #region get
        public List<SuatAnChiTietEntity> getSuatAnChiTiet(long id_truong, short id_khoi, long id_suat_an)
        {
            List<SuatAnChiTietEntity> data = new List<SuatAnChiTietEntity>();
            using (oneduEntities context = new oneduEntities())
            {
                var strQuery = string.Format(@"select sa.*,tp.ten
                        ,tp.nang_luong_kcal as nang_luong_kcal_old,tp.protid as protid_old,tp.glucid as glucid_old,tp.lipid as lipid_old
                        from suat_an_chi_tiet sa 
                        left join dm_thuc_pham tp on sa.id_thuc_pham = tp.id
                        where sa.id_truong=:0 and sa.id_khoi=:1 and sa.id_suat_an=:2 and not (sa.is_delete is not null and sa.is_delete=1) 
                        order by sa.id_nhom_thuc_pham, tp.ten");
                data = context.Database.SqlQuery<SuatAnChiTietEntity>(strQuery, id_truong, id_khoi, id_suat_an).ToList();
            }
            return data;
        }
        public SUAT_AN_CHI_TIET getSuatAnChiTietByID(long id)
        {
            SUAT_AN_CHI_TIET data = new SUAT_AN_CHI_TIET();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("SUAT_AN_CHI_TIET", "getSuatAnChiTietByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.SUAT_AN_CHI_TIET where p.ID == id && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as SUAT_AN_CHI_TIET;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public SUAT_AN_CHI_TIET getSuatAnChiTietByTruongAndSuatAn(long id_truong, short id_khoi, short id_nhom_thuc_pham, long id_thuc_pham, long id_suat_an)
        {
            SUAT_AN_CHI_TIET data = new SUAT_AN_CHI_TIET();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("SUAT_AN_CHI_TIET", "getSuatAnChiTietByTruongAndSuatAn", id_truong, id_khoi, id_nhom_thuc_pham, id_thuc_pham, id_suat_an);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.SUAT_AN_CHI_TIET where p.ID_TRUONG == id_truong && p.ID_KHOI == id_khoi && p.ID_SUAT_AN == id_suat_an && p.ID_NHOM_THUC_PHAM == id_nhom_thuc_pham && p.ID_THUC_PHAM == id_thuc_pham select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as SUAT_AN_CHI_TIET;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<ThucPhamInSuatAnEntity> getThucPhamInSuatAn(long id_truong, short? ma_khoi, long id_suat_an, short? id_nhom_thuc_pham, long? id_thuc_don, string ten_thuc_pham)
        {
            List<ThucPhamInSuatAnEntity> data = new List<ThucPhamInSuatAnEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_THUC_PHAM", "THUC_DON_CHI_TIET", "SUAT_AN_CHI_TIET", "getThucPhamInSuatAn", id_truong, ma_khoi, id_suat_an, id_nhom_thuc_pham, id_thuc_don, ten_thuc_pham);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var strQuery = "";

                    List<object> parameterList = new List<object>();
                    if (id_thuc_don != null)
                    {
                        strQuery = string.Format(@"select tp.id, tp.id_nhom_thuc_pham, tp.ten, tp.nang_luong_kcal, tp.protid, tp.glucid, tp.lipid  
                        ,tblthucDon.so_luong
                        ,tblThucDon.nang_luong_kcal as nang_luong_kcal_new
                        ,tblThucDon.protid as protid_new
                        ,tblThucDon.glucid as glucid_new
                        ,tblThucDon.lipid as lipid_new
                        ,case when (tblThucDon.id is not null and (tblThucDon.is_delete is null or tblThucDon.is_delete=0)) then 1 else 0 end is_chon
                        ,tp.don_vi_tinh
                        from DM_thuc_pham tp
                        left join (select td.*
                        from thuc_don_chi_tiet td
                        where td.id_truong=:0 and td.id_thuc_don=:1 {0}) tblThucDon
                        on tp.id=tblThucDon.id_thuc_pham
                        where not (tp.is_delete is not null and tp.is_delete=1)", (ma_khoi != null && ma_khoi > 0) ? " and td.id_khoi=" + ma_khoi.Value : "");
                        parameterList.Add(id_truong);
                        parameterList.Add(id_thuc_don);
                    }
                    else
                    {
                        strQuery = string.Format(@"select tp.id, tp.id_nhom_thuc_pham, tp.ten, tp.nang_luong_kcal, tp.protid, tp.glucid, tp.lipid
                        ,tblSuatAn.nang_luong_kcal_new,tblSuatAn.protid_new,tblSuatAn.glucid_new,tblSuatAn.lipid_new
                        ,tblSuatAn.is_delete,tblSuatAn.id as id_suat_an_chi_tiet, tblSuatAn.so_luong
                        ,case when (tblSuatAn.id is not null and (tblSuatAn.is_delete is null or tblSuatAn.is_delete=0)) then 1 else 0 end is_chon
                        ,tp.don_vi_tinh
                        from DM_thuc_pham tp
                        left join (select sact.id, sact.id_suat_an, sact.id_thuc_pham, sact.glucid as glucid_new, sact.lipid as lipid_new
                        ,sact.so_luong, sact.nang_luong_kcal as nang_luong_kcal_new, sact.protid as protid_new, sact.id_khoi,sact.is_delete
                        from suat_an_chi_tiet sact
                        where sact.id_truong=:0 and sact.id_suat_an=:1 {0}) tblSuatAn
                        on tp.id=tblSuatAn.id_thuc_pham
                        where not (tp.is_delete is not null and tp.is_delete=1)", (ma_khoi != null && ma_khoi > 0) ? " and sact.id_khoi=" + ma_khoi.Value : "");
                        parameterList.Add(id_truong);
                        parameterList.Add(id_suat_an);
                    }
                    if (id_nhom_thuc_pham != null)
                    {
                        strQuery += " and tp.id_nhom_thuc_pham=:" + parameterList.Count;
                        parameterList.Add(id_nhom_thuc_pham);
                    }
                    if (!string.IsNullOrEmpty(ten_thuc_pham))
                        strQuery += string.Format(@" and lower(tp.ten) like lower(N'%{0}%')", ten_thuc_pham);
                    strQuery += " order by tp.id_nhom_thuc_pham,tp.thu_tu";
                    data = context.Database.SqlQuery<ThucPhamInSuatAnEntity>(strQuery, parameterList.ToArray()).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<ThucPhamInSuatAnEntity>;
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
        public ResultEntity update(SUAT_AN_CHI_TIET detail_in, long? nguoi)
        {
            SUAT_AN_CHI_TIET detail = new SUAT_AN_CHI_TIET();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.SUAT_AN_CHI_TIET
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();

                    if (detail != null)
                    {
                        detail.ID_SUAT_AN = detail_in.ID_SUAT_AN;
                        detail.ID_NHOM_THUC_PHAM = detail_in.ID_NHOM_THUC_PHAM;
                        detail.ID_THUC_PHAM = detail_in.ID_THUC_PHAM;
                        detail.SO_LUONG = detail_in.SO_LUONG;
                        detail.DON_VI_TINH = detail_in.DON_VI_TINH;
                        detail.DON_GIA = detail_in.DON_GIA;
                        detail.TONG_GIA = detail_in.TONG_GIA;
                        detail.NANG_LUONG_KCAL = detail_in.NANG_LUONG_KCAL;
                        detail.PROTID = detail_in.PROTID;
                        detail.GLUCID = detail_in.GLUCID;
                        detail.LIPID = detail_in.LIPID;
                        detail.ID_TRUONG = detail_in.ID_TRUONG;
                        detail.ID_KHOI = detail_in.ID_KHOI;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            var QICache = new DefaultCacheProvider();
            QICache.RemoveByFirstName("SUAT_AN_CHI_TIET");
            return res;
        }
        public ResultEntity insert(SUAT_AN_CHI_TIET detail_in, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail_in.NGUOI_TAO = nguoi;
                    detail_in.NGAY_TAO = DateTime.Now;
                    detail_in = context.SUAT_AN_CHI_TIET.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            var QICache = new DefaultCacheProvider();
            QICache.RemoveByFirstName("SUAT_AN_CHI_TIET");
            return res;
        }
        public ResultEntity delete(long id, long? nguoi, bool is_delete = false)
        {
            SUAT_AN_CHI_TIET detail = new SUAT_AN_CHI_TIET();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            string sql = string.Empty;
            try
            {
                using (var context = new oneduEntities())
                {

                    if (!is_delete)
                    {
                        sql += @"UPDATE SUAT_AN_CHI_TIET SET IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 WHERE ID = " + id.ToString();
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now);
                    }
                    else
                    {
                        sql += @"DELETE SUAT_AN_CHI_TIET WHERE ID = " + id.ToString();
                        context.Database.ExecuteSqlCommand(sql);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }

            var QICache = new DefaultCacheProvider();
            QICache.RemoveByFirstName("SUAT_AN_CHI_TIET");
            return res;
        }
        public ResultEntity deleteThucPhamInSuatAn(long id_truong, long id_suat_an, long id_nhom_thuc_pham, long id_thuc_pham, long? nguoi, bool is_delete = false)
        {
            THUC_DON_CHI_TIET detail = new THUC_DON_CHI_TIET();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            string sql = string.Empty;
            try
            {
                using (var context = new oneduEntities())
                {

                    if (!is_delete)
                    {
                        sql = @"UPDATE SUAT_AN_CHI_TIET SET IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 WHERE ID_TRUONG=" + id_truong + " and ID_SUAT_AN=" + id_suat_an + " and ID_NHOM_THUC_PHAM=" + id_nhom_thuc_pham + " and ID_THUC_PHAM=" + id_thuc_pham;
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now);
                    }
                    else
                    {
                        sql = @"DELETE SUAT_AN_CHI_TIET WHERE ID_TRUONG=" + id_truong + " and ID_SUAT_AN=" + id_suat_an + " and ID_NHOM_THUC_PHAM=" + id_nhom_thuc_pham + " and ID_THUC_PHAM=" + id_thuc_pham;
                        context.Database.ExecuteSqlCommand(sql);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }

            var QICache = new DefaultCacheProvider();
            QICache.RemoveByFirstName("SUAT_AN_CHI_TIET");
            return res;
        }
        public ResultEntity insertOrUpdate(SUAT_AN_CHI_TIET detail_in, long? nguoi)
        {
            SUAT_AN_CHI_TIET detail = new SUAT_AN_CHI_TIET();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.SUAT_AN_CHI_TIET
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();

                    if (detail != null)
                    {
                        detail.SO_LUONG = detail_in.SO_LUONG;
                        detail.DON_VI_TINH = detail_in.DON_VI_TINH;
                        detail.DON_GIA = detail_in.DON_GIA;
                        detail.TONG_GIA = detail_in.TONG_GIA;
                        detail.NANG_LUONG_KCAL = detail_in.NANG_LUONG_KCAL;
                        detail.PROTID = detail_in.PROTID;
                        detail.GLUCID = detail_in.GLUCID;
                        detail.LIPID = detail_in.LIPID;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        detail.IS_DELETE = detail_in.IS_DELETE;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                    else
                    {
                        detail_in.ID = context.Database.SqlQuery<long>("SELECT SUAT_AN_CHI_TIET_SEQ.nextval FROM SYS.DUAL").FirstOrDefault();
                        detail_in.NGAY_TAO = DateTime.Now;
                        detail_in.NGUOI_TAO = nguoi;
                        detail_in.NGAY_SUA = DateTime.Now;
                        detail_in.NGUOI_SUA = nguoi;
                        detail_in = context.SUAT_AN_CHI_TIET.Add(detail_in);
                        context.SaveChanges();
                        res.ResObject = detail_in;
                    }
                }
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            var QICache = new DefaultCacheProvider();
            QICache.RemoveByFirstName("SUAT_AN_CHI_TIET");
            return res;
        }
        #endregion
    }
}
