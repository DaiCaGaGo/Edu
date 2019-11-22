using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class ChuyenCanBO
    {
        #region get
        public List<CHUYEN_CAN> getChuyenCan(long id_truong, short? ma_khoi, long? id_lop, short ma_nam_hoc, short thang)
        {
            List<CHUYEN_CAN> data = new List<CHUYEN_CAN>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CHUYEN_CAN", "getChuyenCan", id_truong, ma_khoi, id_lop, ma_nam_hoc, thang);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.CHUYEN_CAN where p.ID_TRUONG == id_truong && p.MA_KHOI == ma_khoi && p.ID_LOP == id_lop && p.ID_NAM_HOC == ma_nam_hoc && p.THANG == thang select p);
                    tmp = tmp.OrderBy(x => x.THANG);
                    data = tmp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<CHUYEN_CAN>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<ChuyenCanEntity> getDiemChuyenCanByTruongLopNamThang(long id_truong, short? ma_khoi, long? id_lop, short ma_nam_hoc, short thang)
        {
            List<ChuyenCanEntity> data = new List<ChuyenCanEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CHUYEN_CAN", "HOC_SINH", "GIOI_TINH", "getDiemChuyenCanByTruongLopNamThang", id_truong, ma_khoi, id_lop, ma_nam_hoc, thang);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = @"select cc.*,hs.MA as MA_HS,hs.HO_TEN as  TEN_HS,hs.MA_GIOI_TINH ,g.TEN as TEN_GIOI_TINH,hs.NGAY_SINH 
                        from CHUYEN_CAN cc
                        left join HOC_SINH hs on cc.id_hoc_sinh = hs.id
                        join GIOI_TINH g on hs.MA_GIOI_TINH=g.MA
                        where not ( cc.IS_DELETE is not null and cc.IS_DELETE =1 ) 
                        and not ( hs.IS_DELETE is not null and hs.IS_DELETE =1 ) 
                        AND cc.id_truong = :0
                        AND cc.id_lop = :1 and cc.ma_khoi = :2 and cc.id_nam_hoc = :3 and cc.thang = :4
                        order by nvl(hs.thu_tu, 0),NLSSORT(hs.ten, 'NLS_SORT=vietnamese'),NLSSORT(hs.ho_dem, 'NLS_SORT=vietnamese')";
                    
                    data = context.Database.SqlQuery<ChuyenCanEntity>(strQuery, id_truong, id_lop, ma_khoi, ma_nam_hoc, thang).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<ChuyenCanEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public CHUYEN_CAN getChuyenCanByID(long id)
        {
            CHUYEN_CAN data = new CHUYEN_CAN();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CHUYEN_CAN", "getChuyenCanByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.CHUYEN_CAN where p.ID == id select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as CHUYEN_CAN;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<CHUYEN_CAN> getChuyenCanByLop(long id_lop)
        {
            List<CHUYEN_CAN> data = new List<CHUYEN_CAN>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CHUYEN_CAN", "getChuyenCanByLop", id_lop);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.CHUYEN_CAN where p.ID_LOP == id_lop && p.IS_DELETE != true select p).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<CHUYEN_CAN>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public ChuyenCanEntity getTongSoBuoiNghiTrongNam(short id_nam_hoc, long id_truong, long id_lop, long id_hoc_sinh)
        {
            ChuyenCanEntity data = new ChuyenCanEntity();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CHUYEN_CAN", "getTongSoBuoiNghiTrongNam", id_nam_hoc, id_truong, id_lop, id_hoc_sinh);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = @"select sum(tong_phep) as nghi_P, sum(tong_khong_phep) as nghi_KP
                        from chuyen_can where id_nam_hoc=:0 and id_truong=:1 and id_lop=:2 and id_hoc_sinh=:3
                        and not (is_delete is not null and is_delete=1)";
                    data = context.Database.SqlQuery<ChuyenCanEntity>(strQuery, id_nam_hoc, id_truong, id_lop, id_hoc_sinh).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as ChuyenCanEntity;
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
        public ResultEntity update(CHUYEN_CAN detail_in, long? nguoi)
        {
            CHUYEN_CAN detail = new CHUYEN_CAN();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.CHUYEN_CAN
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.ID_HOC_SINH = detail_in.ID_HOC_SINH;
                        detail.ID_LOP = detail_in.ID_LOP;
                        detail.ID_TRUONG = detail_in.ID_TRUONG;
                        detail.MA_KHOI = detail_in.MA_KHOI;
                        detail.ID_NAM_HOC = detail_in.ID_NAM_HOC;
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
                        short countP = 0; short countK = 0;
                        if (detail_in.NGAY1 == "P") countP++; else if (detail_in.NGAY1 == "K") countK++;
                        if (detail_in.NGAY2 == "P") countP++; else if (detail_in.NGAY2 == "K") countK++;
                        if (detail_in.NGAY3 == "P") countP++; else if (detail_in.NGAY3 == "K") countK++;
                        if (detail_in.NGAY4 == "P") countP++; else if (detail_in.NGAY4 == "K") countK++;
                        if (detail_in.NGAY5 == "P") countP++; else if (detail_in.NGAY5 == "K") countK++;
                        if (detail_in.NGAY6 == "P") countP++; else if (detail_in.NGAY6 == "K") countK++;
                        if (detail_in.NGAY7 == "P") countP++; else if (detail_in.NGAY7 == "K") countK++;
                        if (detail_in.NGAY8 == "P") countP++; else if (detail_in.NGAY8 == "K") countK++;
                        if (detail_in.NGAY9 == "P") countP++; else if (detail_in.NGAY9 == "K") countK++;
                        if (detail_in.NGAY10 == "P") countP++; else if (detail_in.NGAY10 == "K") countK++;
                        if (detail_in.NGAY11 == "P") countP++; else if (detail_in.NGAY11 == "K") countK++;
                        if (detail_in.NGAY12 == "P") countP++; else if (detail_in.NGAY12 == "K") countK++;
                        if (detail_in.NGAY13 == "P") countP++; else if (detail_in.NGAY13 == "K") countK++;
                        if (detail_in.NGAY14 == "P") countP++; else if (detail_in.NGAY14 == "K") countK++;
                        if (detail_in.NGAY15 == "P") countP++; else if (detail_in.NGAY15 == "K") countK++;
                        if (detail_in.NGAY16 == "P") countP++; else if (detail_in.NGAY16 == "K") countK++;
                        if (detail_in.NGAY17 == "P") countP++; else if (detail_in.NGAY17 == "K") countK++;
                        if (detail_in.NGAY18 == "P") countP++; else if (detail_in.NGAY18 == "K") countK++;
                        if (detail_in.NGAY19 == "P") countP++; else if (detail_in.NGAY19 == "K") countK++;
                        if (detail_in.NGAY20 == "P") countP++; else if (detail_in.NGAY20 == "K") countK++;
                        if (detail_in.NGAY21 == "P") countP++; else if (detail_in.NGAY21 == "K") countK++;
                        if (detail_in.NGAY22 == "P") countP++; else if (detail_in.NGAY22 == "K") countK++;
                        if (detail_in.NGAY23 == "P") countP++; else if (detail_in.NGAY23 == "K") countK++;
                        if (detail_in.NGAY24 == "P") countP++; else if (detail_in.NGAY24 == "K") countK++;
                        if (detail_in.NGAY25 == "P") countP++; else if (detail_in.NGAY25 == "K") countK++;
                        if (detail_in.NGAY26 == "P") countP++; else if (detail_in.NGAY26 == "K") countK++;
                        if (detail_in.NGAY27 == "P") countP++; else if (detail_in.NGAY27 == "K") countK++;
                        if (detail_in.NGAY28 == "P") countP++; else if (detail_in.NGAY28 == "K") countK++;
                        if (detail_in.NGAY29 == "P") countP++; else if (detail_in.NGAY29 == "K") countK++;
                        if (detail_in.NGAY30 == "P") countP++; else if (detail_in.NGAY30 == "K") countK++;
                        if (detail_in.NGAY31 == "P") countP++; else if (detail_in.NGAY31 == "K") countK++;
                        detail.TONG_PHEP = countP;
                        detail.TONG_KHONG_PHEP = countK;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CHUYEN_CAN");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(CHUYEN_CAN detail_in, long? nguoi)
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
                    detail_in = context.CHUYEN_CAN.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CHUYEN_CAN");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insertFirstData(long? id_lop, short ma_nam_hoc)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    string sql = string.Format(@"insert into CHUYEN_CAN (ID_HOC_SINH, ID_TRUONG, MA_KHOI, ID_LOP, ID_NAM_HOC, THANG)
                        select * from 
                        (select id as id_hoc_sinh, id_truong, id_khoi, id_lop, ID_NAM_HOC
                            , 1 as thang from hoc_sinh where id_lop = :0
                        union all
                        select id as id_hoc_sinh, id_truong, id_khoi, id_lop, ID_NAM_HOC
                            , 2 as thang from hoc_sinh where id_lop = :1
                        union all
                        select id as id_hoc_sinh, id_truong, id_khoi, id_lop, ID_NAM_HOC
                            , 3 as thang from hoc_sinh where id_lop = :2
                        union all
                        select id as id_hoc_sinh, id_truong, id_khoi, id_lop, ID_NAM_HOC
                            , 4 as thang from hoc_sinh where id_lop = :3
                        union all
                        select id as id_hoc_sinh, id_truong, id_khoi, id_lop, ID_NAM_HOC
                            , 5 as thang from hoc_sinh where id_lop = :4
                        union all
                        select id as id_hoc_sinh, id_truong, id_khoi, id_lop, ID_NAM_HOC
                            , 6 as thang from hoc_sinh where id_lop = :5
                        union all
                        select id as id_hoc_sinh, id_truong, id_khoi, id_lop, ID_NAM_HOC
                            , 7 as thang from hoc_sinh where id_lop = :6
                        union all
                        select id as id_hoc_sinh, id_truong, id_khoi, id_lop, ID_NAM_HOC
                            , 8 as thang from hoc_sinh where id_lop = :7
                        union all
                        select id as id_hoc_sinh, id_truong, id_khoi, id_lop, ID_NAM_HOC
                            , 9 as thang from hoc_sinh where id_lop = :8
                        union all
                        select id as id_hoc_sinh, id_truong, id_khoi, id_lop, ID_NAM_HOC
                            , 10 as thang from hoc_sinh where id_lop = :9
                        union all
                        select id as id_hoc_sinh, id_truong, id_khoi, id_lop, ID_NAM_HOC
                            , 11 as thang from hoc_sinh where id_lop = :10
                        union all
                        select id as id_hoc_sinh, id_truong, id_khoi, id_lop, ID_NAM_HOC
                            , 12 as thang from hoc_sinh where id_lop = :11 )tbl
                        where not exists (select * from CHUYEN_CAN d 
                            where d.ID_HOC_SINH=tbl.id_hoc_sinh and d.ID_LOP=:12 and d.ID_NAM_HOC=:13)");
                    context.Database.ExecuteSqlCommand(sql, id_lop, id_lop, id_lop, id_lop, id_lop, id_lop, id_lop, id_lop, id_lop, id_lop, id_lop, id_lop, id_lop, ma_nam_hoc);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CHUYEN_CAN");
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
                    string sql = string.Format(@"Update CHUYEN_CAN set ID_LOP=:0, MA_KHOI=:1, NGUOI_SUA=:2, NGAY_SUA=:3 where ID_HOC_SINH=:4");
                    context.Database.ExecuteSqlCommand(sql, id_lop, ma_khoi, nguoi, DateTime.Now, id_hoc_sinh);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CHUYEN_CAN");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity delete(long id)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    var sql = @"delete from CHUYEN_CAN where ID = :0";
                    context.Database.ExecuteSqlCommand(sql, id);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CHUYEN_CAN");
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
