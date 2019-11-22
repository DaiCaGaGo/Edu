using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class DangKyAnBO
    {
        #region Get
        public List<HocSinhDangKyAnEntity> getHocSinhDangKyAnByTruongKhoiAndLop(long id_truong, short ma_khoi, long id_lop, short id_nam_hoc, string cap_hoc
            , short hoc_ky, long? id_bua_an, string ten_hs, short? is_dk)
        {
            List<HocSinhDangKyAnEntity> data = new List<HocSinhDangKyAnEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_SINH", "LOP", "KHOI", "DM_BUA_AN", "DANG_KY_AN", "getHocSinhDangKyAnByTruongKhoiAndLop"
                , id_truong, ma_khoi, id_lop, id_nam_hoc, cap_hoc, hoc_ky, id_bua_an, ten_hs, is_dk);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    try
                    {
                        string strQuery = string.Format(@"select d.ID, h.ID as ID_HOC_SINH,h.MA as MA_HOC_SINH,h.HO_TEN,h.NGAY_SINH,l.ID as ID_LOP,l.TEN as TEN_LOP,k.MA as ID_KHOI,k.TEN as TEN_KHOI,dm.ID as ID_BUA_AN,dm.TEN as TEN_BUA_AN 
                        --,case when d.ID is not null then 1 else 0 end as IS_DK
                        ,case when (d.ID is not null and (d.is_delete is null or d.is_delete=0)) then 1 else 0 end as IS_DK
                        from Hoc_sinh h
                        join lop l on h.ID_LOP=l.ID and l.ID_TRUONG=h.ID_TRUONG
                        join Khoi k on l.ID_KHOI=k.MA
                        join DM_BUA_AN dm on h.ID_TRUONG=dm.ID_TRUONG and h.ID_KHOI=dm.ID_KHOI and not (dm.IS_DELETE is not null and dm.IS_DELETE!=1)
                        left join DANG_KY_AN d on h.ID=d.ID_HOC_SINH and h.ID_TRUONG=d.ID_TRUONG and h.ID_LOP=d.ID_LOP and dm.ID=d.ID_BUA_AN
                        and d.HOC_KY=:0
                        where h.ID_TRUONG=:1 and h.ID_NAM_HOC=:2 and h.MA_CAP_HOC=:3 and (h.IS_DELETE is null or h.IS_DELETE = 0)
                        and l.ID_KHOI=:4 and h.ID_LOP=:5 {0} {1} {2} {3}"
                        , hoc_ky == 1 ? " and h.TRANG_THAI_HOC in (1,2,3,8,9,10)" : hoc_ky == 2 ? " and h.TRANG_THAI_HOC in (1,2,3,6,7,10)" : ""
                        , id_bua_an == null ? "" : " and dm.ID=" + id_bua_an.ToString()
                        , string.IsNullOrEmpty(ten_hs.Trim()) ? "" : " and UPPER(h.HO_TEN) like UPPER('%" + ten_hs + "%')"
                        , is_dk == null ? "" : (is_dk == 1 ?
                            " and (d.id is not null and (d.is_delete is null or d.is_delete=0))" :
                            " and (d.ID is null or (d.is_delete is not null and d.is_delete=1))"));
                        strQuery += " order by h.id,dm.id";
                        data = context.Database.SqlQuery<HocSinhDangKyAnEntity>(strQuery, hoc_ky, id_truong, id_nam_hoc, cap_hoc, ma_khoi, id_lop).ToList();
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
                    data = QICache.Get(strKeyCache) as List<HocSinhDangKyAnEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public long? getSoHocSinhDangKy(long id_truong, string ma_cap_hoc, short? ma_khoi, short id_nam_hoc, long? id_lop, short hoc_ky)
        {
            long? data = null;
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_SINH", "DM_BUA_AN", "DANG_KY_AN", "getSoHocSinhDangKy", id_truong, ma_khoi, id_lop, hoc_ky);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var sql = string.Format(@"select count (*) as tong_so from (select distinct tbl.id from (select h.id, d.id as id_dang_ky
                    from Hoc_sinh h
                    join DM_BUA_AN dm on h.ID_TRUONG=dm.ID_TRUONG and h.ID_KHOI=dm.ID_KHOI and not (dm.IS_DELETE is not null and dm.IS_DELETE!=1)
                    left join DANG_KY_AN d on h.ID=d.ID_HOC_SINH and h.ID_TRUONG=d.ID_TRUONG 
                    and h.ID_LOP=d.ID_LOP and dm.ID=d.ID_BUA_AN and d.HOC_KY=:0
                    and (d.id is not null and (d.is_delete is null or d.is_delete=0))
                    where h.ID_TRUONG=:1 and h.MA_CAP_HOC=:2 and h.ID_NAM_HOC=:3 and not (h.IS_DELETE is not null and h.IS_DELETE!=1)
                    and h.ID_KHOI=:4 and h.ID_LOP=:5 {0}) tbl where tbl.id_dang_ky is not null)",
                    hoc_ky == 1 ? " and h.TRANG_THAI_HOC in (1,2,3,8,9,10)" : hoc_ky == 2 ? " and h.TRANG_THAI_HOC in (1,2,3,6,7,10)" : "");
                    data = context.Database.SqlQuery<long?>(sql, hoc_ky, id_truong, ma_cap_hoc, id_nam_hoc, ma_khoi, id_lop).FirstOrDefault();
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
        public DANG_KY_AN getDangKyAnByHocSinh(long id_truong, short id_nam_hoc, long id_lop, long id_bua_an, long id_hoc_sinh, short hoc_ky)
        {
            DANG_KY_AN data = new DANG_KY_AN();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DANG_KY_AN", "getDangKyAnByHocSinh", id_truong, id_nam_hoc, id_lop, id_hoc_sinh, id_bua_an, hoc_ky);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DANG_KY_AN where p.ID_TRUONG == id_truong && p.ID_NAM_HOC == id_nam_hoc && p.ID_LOP == id_lop && p.ID_HOC_SINH == id_hoc_sinh && p.ID_BUA_AN == id_bua_an && p.HOC_KY == hoc_ky && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as DANG_KY_AN;
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
        public ResultEntity insertOrUpdate(DANG_KY_AN detail_in, bool insert, long? nguoi)
        {
            DANG_KY_AN detail = new DANG_KY_AN();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    if (insert)
                    {
                        detail_in.ID = context.Database.SqlQuery<long>("SELECT DANG_KY_AN_SEQ.nextval FROM SYS.DUAL").FirstOrDefault();
                        detail_in.NGAY_TAO = DateTime.Now;
                        detail_in.NGUOI_TAO = nguoi;
                        detail_in.NGAY_SUA = DateTime.Now;
                        detail_in.NGUOI_SUA = nguoi;
                        detail_in = context.DANG_KY_AN.Add(detail_in);
                        context.SaveChanges();
                        res.ResObject = detail_in;
                    }
                    else
                    {
                        detail = (from p in context.DANG_KY_AN
                                  where p.ID == detail_in.ID
                                  select p).FirstOrDefault();
                        detail.IS_DELETE = detail_in.IS_DELETE;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        detail.IS_DELETE = detail_in.IS_DELETE;
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
            QICache.RemoveByFirstName("DANG_KY_AN");
            return res;
        }
        #endregion
    }
}
