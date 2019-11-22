using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class ChamAnBO
    {
        #region get
        public List<ChamAnEntity> getChamAnByTruongLopBuaAnNgay(long id_truong, short id_khoi, short id_nam_hoc, long id_lop, int thang, int ngay, int hoc_ky, List<DM_BUA_AN> lstBuaAn)
        {
            List<ChamAnEntity> data = new List<ChamAnEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_SINH", "DANG_KY_AN", "CHAM_AN", "getChamAnByTruongLopBuaAnNgay", id_truong, id_khoi, id_nam_hoc, id_lop, thang, ngay, hoc_ky, lstBuaAn);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    if (lstBuaAn.Count > 0)
                    {
                        string strQuery = "select hs.MA as MA_HS,hs.ID as ID_HOC_SINH,hs.HO_TEN as TEN_HS,hs.NGAY_SINH,hs.TEN,hs.HO_DEM";
                        for (int i = 0; i < lstBuaAn.Count; i++)
                        {
                            strQuery += ",case when exists (select ID from DANG_KY_AN dk where dk.id_truong=hs.id_truong and dk.id_nam_hoc=hs.id_nam_hoc and hs.id_lop=dk.id_lop and dk.ID_HOC_SINH=hs.ID and dk.HOC_KY=" + hoc_ky + " and dk.ID_BUA_AN=" + lstBuaAn[i].ID + " and not (dk.is_Delete is not null and dk.is_delete=1)) then 1 else 0 end as IS_BUA_AN_" + i;
                            strQuery += ",case when exists (select ID from CHAM_AN ca where ca.id_truong=hs.id_truong and ca.id_nam_hoc=hs.id_nam_hoc and hs.id_lop=ca.id_lop and ca.ID_HOC_SINH = hs.ID and ca.THANG = " + thang + " and ca.hoc_ky=" + hoc_ky + " and ca.ID_BUA_AN = " + lstBuaAn[i].ID + " and not(ca.is_Delete is not null and ca.is_delete = 1) and ca.NGAY" + ngay + " = 1 ) then 1 else 0 end as BUA_AN_" + i;
                        }
                        strQuery += " from HOC_SINH hs where hs.id_truong=:0 and hs.id_khoi=:1 and hs.id_nam_hoc=:2 and hs.id_lop=:3";
                        strQuery += " order by hs.THU_TU,NLSSORT(hs.ten,'NLS_SORT=vietnamese'),NLSSORT(hs.ho_dem,'NLS_SORT=vietnamese')";
                        data = context.Database.SqlQuery<ChamAnEntity>(strQuery, id_truong, id_khoi, id_nam_hoc, id_lop).ToList();
                        QICache.Set(strKeyCache, data, 300000);
                    }
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<ChamAnEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public CHAM_AN getChamAnBy_Truong_Lop_BuaAn_HocSinh_Thang_HocKy(long id_truong, short id_khoi, short id_nam_hoc, long id_lop, long id_hoc_sinh, int thang, int hoc_ky, long id_bua_an)
        {
            CHAM_AN data = new CHAM_AN();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CHAM_AN", "getChamAnBy_Truong_Lop_BuaAn_HocSinh_Thang_HocKy", id_truong, id_khoi, id_nam_hoc, id_lop, id_hoc_sinh, thang, hoc_ky, id_bua_an);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.CHAM_AN where p.ID_TRUONG == id_truong && p.ID_KHOI == id_khoi && p.ID_NAM_HOC == id_nam_hoc && p.ID_LOP == id_lop && p.ID_HOC_SINH == id_hoc_sinh && p.THANG == thang && p.HOC_KY == hoc_ky && p.ID_BUA_AN == id_bua_an && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as CHAM_AN;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public long? getSoHocSinhByNgayAndBuaAn(long id_truong, short id_khoi, long id_bua_an, int id_nam_hoc, int thang, int ngay)
        {
            long? data = null;
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CHAM_AN", "getSoHocSinhByNgayAndBuaAn", id_truong, id_khoi, id_bua_an, id_nam_hoc, thang, ngay);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var sql = @"SELECT count(*) FROM CHAM_AN
                        WHERE ID_TRUONG=:0 AND ID_KHOI=:1 AND ID_NAM_HOC=:2 AND THANG=:3 AND ID_BUA_AN=:4
                        AND NOT (IS_DELETE IS NOT NULL AND IS_DELETE=1)";
                    sql += " AND (NGAY" + ngay + " IS NOT NULL AND NGAY" + ngay + "=1)";
                    data = context.Database.SqlQuery<long?>(sql, id_truong, id_khoi, id_nam_hoc, thang, id_bua_an).FirstOrDefault();
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
        #region set
        public ResultEntity update(CHAM_AN detail_in, long? nguoi)
        {
            CHAM_AN detail = new CHAM_AN();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.CHAM_AN
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.ID_HOC_SINH = detail_in.ID_HOC_SINH;
                        detail.ID_LOP = detail_in.ID_LOP;
                        detail.ID_TRUONG = detail_in.ID_TRUONG;
                        detail.ID_KHOI = detail_in.ID_KHOI;
                        detail.ID_NAM_HOC = detail_in.ID_NAM_HOC;
                        detail.HOC_KY = detail_in.HOC_KY;
                        detail.ID_BUA_AN = detail_in.ID_BUA_AN;
                        detail.THANG = detail_in.THANG;
                        detail.NGAY1 = detail_in.NGAY1;
                        detail.NGAY2 = detail_in.NGAY2;
                        detail.NGAY3 = detail_in.NGAY3;
                        detail.NGAY4 = detail_in.NGAY4;
                        detail.NGAY5 = detail_in.NGAY5;
                        detail.NGAY6 = detail_in.NGAY6;
                        detail.NGAY7 = detail_in.NGAY7;
                        detail.NGAY8 = detail_in.NGAY8;
                        detail.NGAY9 = detail_in.NGAY9;
                        detail.NGAY10 = detail_in.NGAY10;
                        detail.NGAY11 = detail_in.NGAY11;
                        detail.NGAY12 = detail_in.NGAY12;
                        detail.NGAY13 = detail_in.NGAY13;
                        detail.NGAY14 = detail_in.NGAY14;
                        detail.NGAY15 = detail_in.NGAY15;
                        detail.NGAY16 = detail_in.NGAY16;
                        detail.NGAY17 = detail_in.NGAY17;
                        detail.NGAY18 = detail_in.NGAY18;
                        detail.NGAY19 = detail_in.NGAY19;
                        detail.NGAY20 = detail_in.NGAY20;
                        detail.NGAY21 = detail_in.NGAY21;
                        detail.NGAY22 = detail_in.NGAY22;
                        detail.NGAY23 = detail_in.NGAY23;
                        detail.NGAY24 = detail_in.NGAY24;
                        detail.NGAY25 = detail_in.NGAY25;
                        detail.NGAY26 = detail_in.NGAY26;
                        detail.NGAY27 = detail_in.NGAY27;
                        detail.NGAY28 = detail_in.NGAY28;
                        detail.NGAY29 = detail_in.NGAY29;
                        detail.NGAY30 = detail_in.NGAY30;
                        detail.NGAY31 = detail_in.NGAY31;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CHAM_AN");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(CHAM_AN detail_in, long? nguoi)
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
                    detail_in.NGAY_SUA = DateTime.Now;
                    detail_in.NGUOI_SUA = nguoi;
                    detail_in = context.CHAM_AN.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CHAM_AN");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insertFirstData(long id_truong, short id_khoi, short ma_nam_hoc, long id_lop, short thang, short hoc_ky, List<DM_BUA_AN> lstBuaAn)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            DataAccessAPI dataAccessAPI = new DataAccessAPI();
            try
            {
                using (var context = new oneduEntities())
                {
                    if (lstBuaAn != null && lstBuaAn.Count > 0)
                    {
                        string strQuery = string.Format(@"insert into CHAM_AN 
                    (ID_HOC_SINH, ID_TRUONG, ID_KHOI, ID_LOP, ID_NAM_HOC, THANG, ID_BUA_AN, HOC_KY)
                    select distinct tbl.* from (");
                        for (int i = 0; i < lstBuaAn.Count; i++)
                        {
                            if (hoc_ky == 1)
                                strQuery += string.Format(@"select hs.id as id_hoc_sinh, hs.id_truong, hs.id_khoi, hs.id_lop, hs.ID_NAM_HOC, {0} as thang, {1} as id_bua_an, 1 as hoc_ky from hoc_sinh hs join dang_ky_an dk on dk.id_truong=hs.id_truong and dk.id_nam_hoc=hs.id_nam_hoc and dk.id_hoc_sinh=hs.id and not (dk.is_delete is not null and dk.is_delete=1) and dk.hoc_ky=1 where hs.id_truong={2} and hs.id_khoi={3} and hs.id_lop = {4} 
                                union all ", thang, lstBuaAn[i].ID, id_truong, id_khoi, id_lop);
                            else if (hoc_ky == 2)
                                strQuery += string.Format(@"select hs.id as id_hoc_sinh, hs.id_truong, hs.id_khoi, hs.id_lop, hs.ID_NAM_HOC, {0} as thang, {1} as id_bua_an, 2 as hoc_ky from hoc_sinh hs join dang_ky_an dk on dk.id_truong=hs.id_truong and dk.id_nam_hoc=hs.id_nam_hoc and dk.id_hoc_sinh=hs.id and not (dk.is_delete is not null and dk.is_delete=1) and dk.hoc_ky=2 where hs.id_truong={2} and hs.id_khoi={3} and hs.id_lop = {4} 
                                union all ", thang, lstBuaAn[i].ID, id_truong, id_khoi, id_lop);
                        }
                        strQuery = dataAccessAPI.removeLastString(strQuery, " union all ");
                        strQuery += string.Format(@")tbl
                            where not exists (select * from CHAM_AN d 
                            where d.id_truong=tbl.id_truong and d.id_khoi=tbl.id_khoi and d.id_lop=tbl.id_lop 
                            and d.ID_HOC_SINH=tbl.id_hoc_sinh and d.ID_TRUONG=:0 and d.ID_NAM_HOC=:1 and d.ID_LOP=:2 and d.thang=:3 and d.hoc_ky=:4)");
                        context.Database.ExecuteSqlCommand(strQuery, id_truong, ma_nam_hoc, id_lop, thang, hoc_ky);
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CHAM_AN");
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
            CHAM_AN detail = new CHAM_AN();
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
                        sql += @"UPDATE CHAM_AN SET IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 WHERE ID = " + id.ToString();
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now);
                    }
                    else
                    {
                        sql += @"DELETE CHAM_AN WHERE ID = " + id.ToString();
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
            QICache.RemoveByFirstName("CHAM_AN");
            return res;
        }
        #endregion
    }
}
