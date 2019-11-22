using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class DanhGiaDinhKyMonTHBO
    {
        #region get

        public List<DanhGiaDinhKyMonTHEntity> getDanhGiaDinhKyMonTHByTruongKhoiLopMonAndGiaiDoan(short id_nam_hoc, long id_truong, short? ma_khoi, long? id_lop, long? id_mon_truong, short ma_giai_doan)
        {
            DataAccessAPI dataAccessAPI = new DataAccessAPI();
            List<DanhGiaDinhKyMonTHEntity> data = new List<DanhGiaDinhKyMonTHEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DANH_GIA_DINH_KY_MON_TH", "HOC_SINH", "GIOI_TINH" + "getDanhGiaDinhKyMonTHByTruongKhoiLopMonAndGiaiDoan"
                , id_nam_hoc, id_truong, ma_khoi, id_lop, id_mon_truong, ma_giai_doan);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = @"select d.*,h.MA as MA_HS,h.HO_TEN as TEN_HS,h.MA_GIOI_TINH ,g.TEN as TEN_GIOI_TINH,h.NGAY_SINH 
                                        from DANH_GIA_DINH_KY_MON_TH d
                                        join HOC_SINH h on h.ID_TRUONG=d.ID_TRUONG and d.ID_HOC_SINH =h.ID and d.ID_LOP=h.ID_LOP
                                        join GIOI_TINH g on h.MA_GIOI_TINH=g.MA
                                        where not ( d.IS_DELETE is not null and d.IS_DELETE =1 ) and not ( h.IS_DELETE is not null and h.IS_DELETE =1 ) 
                                        and d.MA_NAM_HOC=:0 and d.ID_TRUONG=:1 and d.MA_KY_DG=:2
                                        ";
                    List<object> parameterList = new List<object>();
                    parameterList.Add(id_nam_hoc);
                    parameterList.Add(id_truong);
                    parameterList.Add(ma_giai_doan);

                    strQuery += " and h.ID_KHOI=:" + parameterList.Count;
                    parameterList.Add(ma_khoi == null ? (object)DBNull.Value : ma_khoi);

                    strQuery += " and d.ID_LOP=:" + parameterList.Count;
                    parameterList.Add(id_lop == null ? (object)DBNull.Value : id_lop);

                    strQuery += " and d.ID_MON_TRUONG=:" + parameterList.Count;
                    parameterList.Add(id_mon_truong == null ? (object)DBNull.Value : id_mon_truong);

                    strQuery += " order by h.THU_TU,NLSSORT(h.ten,'NLS_SORT=vietnamese'),NLSSORT(h.HO_DEM,'NLS_SORT=vietnamese')";
                    data = context.Database.SqlQuery<DanhGiaDinhKyMonTHEntity>(strQuery, parameterList.ToArray()).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<DanhGiaDinhKyMonTHEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public DANH_GIA_DINH_KY_MON_TH getDanhGiaDinhKyMonTHByID(long id)
        {
            DANH_GIA_DINH_KY_MON_TH data = new DANH_GIA_DINH_KY_MON_TH();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DANH_GIA_DINH_KY_MON_TH", "getDanhGiaDinhKyMonTHByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DANH_GIA_DINH_KY_MON_TH where p.ID == id select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as DANH_GIA_DINH_KY_MON_TH;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<DanhGiaDinhKyMonTHEntity> getDiemTongKetNamTheoHocSinh(short id_nam_hoc, long id_truong, long id_lop, long id_hoc_sinh)
        {
            List<DanhGiaDinhKyMonTHEntity> data = new List<DanhGiaDinhKyMonTHEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DANH_GIA_DINH_KY_MON_TH", "MON_HOC_TRUONG", "MON_HOC", "getDiemTongKetNamTheoHocSinh", id_nam_hoc, id_truong, id_lop, id_hoc_sinh);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = @"select d.id_hoc_sinh, d.ma_ky_dg, d.KTDK, d.noi_dung_nx,d.MUC
                        ,m.TEN as TEN_MON_HOC, d.ID_MON_TRUONG,mh.ID as ID_MON_HOC
                        from danh_gia_dinh_ky_mon_th d
                        left join MON_HOC_TRUONG m on d.ma_nam_hoc=m.id_nam_hoc and d.ID_TRUONG=m.ID_TRUONG 
                        and d.ID_MON_TRUONG=m.id
                        left join MON_HOC mh on m.ID_MON_HOC = mh.id
                        where d.MA_NAM_HOC=:0 and d.ID_TRUONG=:1 and d.ID_LOP=:2 and d.ID_HOC_SINH=:3 and d.ma_ky_dg=4
                        and not (d.IS_DELETE is not null and d.IS_DELETE=1)
                        and not (m.IS_DELETE is not null and m.IS_DELETE=1)
                        order by m.THU_TU";
                    data = context.Database.SqlQuery<DanhGiaDinhKyMonTHEntity>(strQuery, id_nam_hoc, id_truong, id_lop, id_hoc_sinh).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<DanhGiaDinhKyMonTHEntity>;
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
        public ResultEntity update(DANH_GIA_DINH_KY_MON_TH detail_in, NGUOI_DUNGEntity nguoi)
        {
            DANH_GIA_DINH_KY_MON_TH detail = new DANH_GIA_DINH_KY_MON_TH();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.DANH_GIA_DINH_KY_MON_TH
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.KTDK = detail_in.KTDK;
                        detail.MA_NX = detail_in.MA_NX;
                        detail.NOI_DUNG_NX = detail_in.NOI_DUNG_NX;
                        detail.MUC = detail_in.MUC;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi.ID;
                        detail.IS_DELETE = detail_in.IS_DELETE;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DANH_GIA_DINH_KY_MON_TH");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
                //res.Msg = ex.ToString();
            }
            return res;
        }
        public ResultEntity insert(DANH_GIA_DINH_KY_MON_TH detail_in, long? nguoi)
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
                    detail_in = context.DANH_GIA_DINH_KY_MON_TH.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DANH_GIA_DINH_KY_MON_TH");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insertEmpty(short id_nam_hoc, long id_truong, short ma_khoi, long id_lop, short giai_doan, long id_mon_hoc_truong, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    string strQuery = string.Format(@"insert into DANH_GIA_DINH_KY_MON_TH(ID_TRUONG,MA_NAM_HOC,ID_LOP,ID_HOC_SINH, MA_KY_DG,MA_KHOI,ID_MON_TRUONG, NGUOI_TAO, NGAY_TAO)
                                                    select ID_TRUONG,ID_NAM_HOC,ID_LOP,ID as ID_HOC_SINH,:0 as MA_KY_DG,:1 as MA_KHOI,:2 as ID_MON_TRUONG,:3 as NGUOI_TAO, (SELECT SYSDATE FROM DUAL) as NGAY_TAO
                                                    from HOC_SINH 
                                                    where HOC_SINH.ID_LOP=:4  and not ( HOC_SINH.IS_DELETE is not null and HOC_SINH.IS_DELETE =1 ) and ID_NAM_HOC=:5 and ID_TRUONG=:6
                                                    and not exists (select * from DANH_GIA_DINH_KY_MON_TH d 
                                                                    where d.ID_HOC_SINH=HOC_SINH.ID and d.MA_KY_DG=:0
                                                                    and d.id_mon_truong=:2
                                                                    and d.ID_LOP=HOC_SINH.ID_LOP 
                                                                    and d.ID_TRUONG=HOC_SINH.ID_TRUONG
                                                                    and d.MA_NAM_HOC=HOC_SINH.ID_NAM_HOC)");
                    context.Database.ExecuteSqlCommand(strQuery, giai_doan, ma_khoi, id_mon_hoc_truong, nguoi == null ? (object)DBNull.Value : nguoi, id_lop, id_nam_hoc, id_truong);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DANH_GIA_DINH_KY_MON_TH");
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
                        var sql = @"update DANH_GIA_DINH_KY_MON_TH set IS_DELETE = 1,NGUOI_SUA=:0,NGAY_SUA=:1 where ID = :2";
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, id);
                    }
                    else
                    {
                        var sql = @"delete DANH_GIA_DINH_KY_MON_TH where ID = :0";
                        context.Database.ExecuteSqlCommand(sql, id);
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DANH_GIA_DINH_KY_MON_TH");
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
                    string sql = string.Format(@"Update DANH_GIA_DINH_KY_MON_TH set ID_LOP=:0, MA_KHOI=:1, NGUOI_SUA=:2, NGAY_SUA=:3 where ID_HOC_SINH=:4");
                    context.Database.ExecuteSqlCommand(sql, id_lop, ma_khoi, nguoi, DateTime.Now, id_hoc_sinh);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DANH_GIA_DINH_KY_MON_TH");
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
