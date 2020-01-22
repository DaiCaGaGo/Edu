using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class CaHocBO
    {
        #region Get
        public List<CA_HOC> getCaHoc()
        {
            List<CA_HOC> data = new List<CA_HOC>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CA_HOC", "getCaHoc");
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.CA_HOC where p.IS_DELETE != true select p).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<CA_HOC>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public CA_HOC getCaHocByID(long id)
        {
            CA_HOC data = new CA_HOC();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CA_HOC", "getCaHocByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.CA_HOC where p.ID == id select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as CA_HOC;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<CA_HOC> getCaHocByLopAndHocKy(long id_truong, long? id_lop, short id_hoc_ky)
        {
            List<CA_HOC> data = new List<CA_HOC>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CA_HOC", "getCaHocByLopAndHocKy", id_truong, id_lop, id_hoc_ky);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.CA_HOC
                               where p.IS_DELETE != true && p.ID_TRUONG == id_truong && p.ID_HOC_KY == id_hoc_ky && p.ID_LOP == id_lop
                               orderby p.TIET
                               select p);
                    data = tmp.ToList();

                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<CA_HOC>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<CA_HOC> getCaHocByLop(long id_lop)
        {
            List<CA_HOC> data = new List<CA_HOC>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CA_HOC", "getCaHocByLop", id_lop);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.CA_HOC where p.IS_DELETE != true && p.ID_LOP == id_lop select p).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<CA_HOC>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<CA_HOC> getCaHocByIDCauHinhCaHoc(long id_cau_hinh_ca_hoc)
        {
            List<CA_HOC> data = new List<CA_HOC>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CA_HOC", "getCaHocByIDCauHinhCaHoc", id_cau_hinh_ca_hoc);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.CA_HOC where p.IS_DELETE != true && p.ID_CAU_HINH_CA_HOC == id_cau_hinh_ca_hoc select p).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<CA_HOC>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<CaHocEntity> getCaHocByGiaoVienTruong(long id_truong, short id_nam_hoc, short id_hoc_ky, long? id_giao_vien)
        {
            List<CaHocEntity> data = new List<CaHocEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CA_HOC", "LOP_MON", "getCaHocByGiaoVienTruong", id_truong, id_nam_hoc, id_hoc_ky, id_giao_vien);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    try
                    {
                        string strQuery = @"select * from ca_hoc 
                            where is_delete=0 and ID_TRUONG=:0 
                            and id_hoc_ky=:1 and ma_nam_hoc=:2
                            and exists (select * from LOP_MON where ca_hoc.ID_LOP=lop_mon.ID_LOP 
                            and lop_mon.HOC_KY=ca_hoc.id_hoc_ky and lop_mon.ID_GIAO_VIEN=:3
                            and (ca_hoc.ID_MON_2=lop_mon.id_mon_truong or 
                            ca_hoc.ID_MON_3=lop_mon.id_mon_truong or
                            ca_hoc.ID_MON_4=lop_mon.id_mon_truong or
                            ca_hoc.ID_MON_5=lop_mon.id_mon_truong or
                            ca_hoc.ID_MON_6=lop_mon.id_mon_truong or
                            ca_hoc.ID_MON_7=lop_mon.id_mon_truong or
                            ca_hoc.ID_MON_8=lop_mon.id_mon_truong)) order by tiet";
                        data = context.Database.SqlQuery<CaHocEntity>(strQuery, id_truong, id_hoc_ky, id_nam_hoc, id_giao_vien).ToList();
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
                    data = QICache.Get(strKeyCache) as List<CaHocEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<ThoiKhoaBieuLopEntity> getThoiKhoaBieuLop(long id_truong, short id_nam_hoc, short id_hoc_ky, long id_lop)
        {
            List<ThoiKhoaBieuLopEntity> data = new List<ThoiKhoaBieuLopEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CA_HOC", "MON_HOC_TRUONG", "getThoiKhoaBieuLop", id_truong, id_nam_hoc, id_hoc_ky, id_lop);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    try
                    {
                        string strQuery = @"SELECT CH.TIET,
                        CH.ID_MON_2,M2.TEN AS TEN_MON_2,
                        CH.ID_MON_3,M3.TEN AS TEN_MON_3,
                        CH.ID_MON_4,M4.TEN AS TEN_MON_4,
                        CH.ID_MON_5,M5.TEN AS TEN_MON_5,
                        CH.ID_MON_6,M6.TEN AS TEN_MON_6,
                        CH.ID_MON_7,M7.TEN AS TEN_MON_7,
                        CH.ID_MON_8,M8.TEN AS TEN_MON_8
                        FROM CA_HOC CH 
                        LEFT JOIN MON_HOC_TRUONG M2 ON CH.ID_MON_2=M2.ID
                        LEFT JOIN MON_HOC_TRUONG M3 ON CH.ID_MON_3=M3.ID
                        LEFT JOIN MON_HOC_TRUONG M4 ON CH.ID_MON_4=M4.ID
                        LEFT JOIN MON_HOC_TRUONG M5 ON CH.ID_MON_5=M5.ID
                        LEFT JOIN MON_HOC_TRUONG M6 ON CH.ID_MON_6=M6.ID
                        LEFT JOIN MON_HOC_TRUONG M7 ON CH.ID_MON_7=M7.ID
                        LEFT JOIN MON_HOC_TRUONG M8 ON CH.ID_MON_8=M8.ID
                        WHERE CH.ID_TRUONG=:0 AND CH.MA_NAM_HOC=:1 AND CH.ID_LOP=:2 AND CH.ID_HOC_KY=:3 
                        AND NOT (CH.IS_DELETE IS NOT NULL AND CH.IS_DELETE=1) ORDER BY CH.TIET";
                        data = context.Database.SqlQuery<ThoiKhoaBieuLopEntity>(strQuery, id_truong, id_nam_hoc, id_lop, id_hoc_ky).ToList();
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
                    data = QICache.Get(strKeyCache) as List<ThoiKhoaBieuLopEntity>;
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
        public ResultEntity update(CA_HOC detail_in, long? nguoi)
        {
            CA_HOC detail = new CA_HOC();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.CA_HOC
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.ID_MON_2 = detail_in.ID_MON_2;
                        detail.ID_MON_3 = detail_in.ID_MON_3;
                        detail.ID_MON_4 = detail_in.ID_MON_4;
                        detail.ID_MON_5 = detail_in.ID_MON_5;
                        detail.ID_MON_6 = detail_in.ID_MON_6;
                        detail.ID_MON_7 = detail_in.ID_MON_7;
                        detail.ID_MON_8 = detail_in.ID_MON_8;
                        detail.THOI_GIAN = detail_in.THOI_GIAN;
                        detail.ID_CAU_HINH_CA_HOC = detail_in.ID_CAU_HINH_CA_HOC;
                        detail.NGUOI_SUA = nguoi;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.IS_DELETE = detail_in.IS_DELETE;
                        detail.ID_TRUONG = detail_in.ID_TRUONG;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CA_HOC");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity updateThoiGian(CA_HOC detail_in)
        {
            CA_HOC detail = new CA_HOC();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.CA_HOC
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.THOI_GIAN = detail_in.THOI_GIAN;
                        detail.NGAY_SUA = DateTime.Now;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CA_HOC");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity updateMonHoc(CA_HOC detail_in, long? nguoi)
        {
            CA_HOC detail = new CA_HOC();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.CA_HOC
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.ID_MON_2 = detail_in.ID_MON_2;
                        detail.ID_MON_3 = detail_in.ID_MON_3;
                        detail.ID_MON_4 = detail_in.ID_MON_4;
                        detail.ID_MON_5 = detail_in.ID_MON_5;
                        detail.ID_MON_6 = detail_in.ID_MON_6;
                        detail.ID_MON_7 = detail_in.ID_MON_7;
                        detail.ID_MON_8 = detail_in.ID_MON_8;
                        detail.NGUOI_SUA = nguoi;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.IS_DELETE = detail_in.IS_DELETE;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CA_HOC");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(CA_HOC detail_in, long? nguoi)
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
                    detail_in = context.CA_HOC.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CA_HOC");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insertFirstData(long id_truong, short id_hoc_ky, short id_nam_hoc, CAU_HINH_CA_HOC detai_cau_hinh)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    string sql = @"INSERT INTO CA_HOC (TIET, ID_LOP, ID_HOC_KY, 
                    id_cau_hinh_ca_hoc,THOI_GIAN, MA_NAM_HOC, ID_TRUONG)
                    select *
                    from ( ";
                    for (int i = 1; i <= 10; i++)
                    {
                        if (i == 1)
                            sql += string.Format(@" SELECT {0} as TIET,ID,{1} as HOC_KY, {2} as id_ch,{3} as tg,{4} as ma_nam_hoc, {5} AS ID_TRUONG
                                        FROM LOP where ID_TRUONG = {5} and ID_NAM_HOC = {4}", i, id_hoc_ky
                                         , detai_cau_hinh == null || detai_cau_hinh.ID == 0 ? "null" : detai_cau_hinh.ID.ToString()
                                         , i - 1
                                         , id_nam_hoc
                                         , id_truong
                                         );
                        else
                            sql += string.Format(@"
                                        union all SELECT {0} as TIET,ID,{1} as HOC_KY, {2} as id_ch,{3} as tg,{4} as ma_nam_hoc, {5} AS ID_TRUONG
                                        FROM LOP where ID_TRUONG = {5} and ID_NAM_HOC = {4}", i, id_hoc_ky
                                       , detai_cau_hinh == null || detai_cau_hinh.ID == 0 ? "null" : detai_cau_hinh.ID.ToString()
                                       , i - 1
                                       , id_nam_hoc
                                       , id_truong
                                       );
                    }
                    sql += @"
                    ) TBInsert 
                    where not exists(select * from CA_HOC where ca_hoc.id_truong = TBInsert.id_truong and CA_HOC.TIET= TBInsert.TIET 
                    and CA_HOC.ID_LOP= TBInsert.ID and CA_HOC.ID_HOC_KY=TBInsert.HOC_KY)";
                    context.Database.ExecuteSqlCommand(sql,
                    detai_cau_hinh == null || detai_cau_hinh.THOI_GIAN_TIET1 == null ? (object)DBNull.Value : detai_cau_hinh.THOI_GIAN_TIET1,
                    detai_cau_hinh == null || detai_cau_hinh.THOI_GIAN_TIET2 == null ? (object)DBNull.Value : detai_cau_hinh.THOI_GIAN_TIET2,
                    detai_cau_hinh == null || detai_cau_hinh.THOI_GIAN_TIET3 == null ? (object)DBNull.Value : detai_cau_hinh.THOI_GIAN_TIET3,
                    detai_cau_hinh == null || detai_cau_hinh.THOI_GIAN_TIET4 == null ? (object)DBNull.Value : detai_cau_hinh.THOI_GIAN_TIET4,
                    detai_cau_hinh == null || detai_cau_hinh.THOI_GIAN_TIET5 == null ? (object)DBNull.Value : detai_cau_hinh.THOI_GIAN_TIET5,
                    detai_cau_hinh == null || detai_cau_hinh.THOI_GIAN_TIET6 == null ? (object)DBNull.Value : detai_cau_hinh.THOI_GIAN_TIET6,
                    detai_cau_hinh == null || detai_cau_hinh.THOI_GIAN_TIET7 == null ? (object)DBNull.Value : detai_cau_hinh.THOI_GIAN_TIET7,
                    detai_cau_hinh == null || detai_cau_hinh.THOI_GIAN_TIET8 == null ? (object)DBNull.Value : detai_cau_hinh.THOI_GIAN_TIET8,
                    detai_cau_hinh == null || detai_cau_hinh.THOI_GIAN_TIET9 == null ? (object)DBNull.Value : detai_cau_hinh.THOI_GIAN_TIET9,
                    detai_cau_hinh == null || detai_cau_hinh.THOI_GIAN_TIET10 == null ? (object)DBNull.Value : detai_cau_hinh.THOI_GIAN_TIET10
                    );
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CA_HOC");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity doiCauHinhCaHoc(long id_truong, short id_hoc_ky, short id_nam_hoc, CAU_HINH_CA_HOC detai_cau_hinh)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    string sql = string.Format(@"update CA_HOC set ID_CAU_HINH_CA_HOC=:0,
                        THOI_GIAN= case when TIET=1 then :1
                                        when TIET=2 then :2
                                        when TIET=3 then :3
                                        when TIET=4 then :4
                                        when TIET=5 then :5
                                        when TIET=6 then :6
                                        when TIET=7 then :7
                                        when TIET=8 then :8
                                        when TIET=9 then :9
                                        when TIET=10 then :10 else '' end
                        where exists (select * from LOP l where l.ID_TRUONG=:11 and l.ID_NAM_HOC=:12 and l.ID=CA_HOC.ID_LOP) 
                        and ID_HOC_KY=:13");
                    context.Database.ExecuteSqlCommand(sql,
                    detai_cau_hinh == null || detai_cau_hinh.ID == 0 ? (object)DBNull.Value : detai_cau_hinh.ID,
                    detai_cau_hinh == null ? (object)DBNull.Value : detai_cau_hinh.THOI_GIAN_TIET1,
                    detai_cau_hinh == null ? (object)DBNull.Value : detai_cau_hinh.THOI_GIAN_TIET2,
                    detai_cau_hinh == null ? (object)DBNull.Value : detai_cau_hinh.THOI_GIAN_TIET3,
                    detai_cau_hinh == null ? (object)DBNull.Value : detai_cau_hinh.THOI_GIAN_TIET4,
                    detai_cau_hinh == null ? (object)DBNull.Value : detai_cau_hinh.THOI_GIAN_TIET5,
                    detai_cau_hinh == null ? (object)DBNull.Value : detai_cau_hinh.THOI_GIAN_TIET6,
                    detai_cau_hinh == null ? (object)DBNull.Value : detai_cau_hinh.THOI_GIAN_TIET7,
                    detai_cau_hinh == null ? (object)DBNull.Value : detai_cau_hinh.THOI_GIAN_TIET8,
                    detai_cau_hinh == null ? (object)DBNull.Value : detai_cau_hinh.THOI_GIAN_TIET9,
                    detai_cau_hinh == null ? (object)DBNull.Value : detai_cau_hinh.THOI_GIAN_TIET10,
                    id_truong, id_nam_hoc, id_hoc_ky
                    );
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CA_HOC");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity updateMonHocNull(long id_lop, short id_hoc_ky)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    string sql = string.Format(@"update CA_HOC set 
                    ID_MON_2 = null,ID_MON_3 = null,ID_MON_4 = null,
                    ID_MON_5 = null,ID_MON_6 = null,ID_MON_7 = null,ID_MON_8 = null
                    where ID_LOP=:0 and ID_HOC_KY=:1");
                    context.Database.ExecuteSqlCommand(sql, id_lop, id_hoc_ky
                    );
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CA_HOC");
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
                    var sql = @"update CA_HOC set  IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 where ID = :2";
                    context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, id);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CA_HOC");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity copyLichHocTheoKy(long id_truong, short id_nam_hoc, long id_lop)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    for (int i = 1; i < 11; i++)
                    {
                        string query = string.Format(@"UPDATE ca_hoc T1
                           SET T1.tiet     = {0},
                               T1.id_mon_2 =
                               (SELECT T2.id_mon_2
                                  FROM ca_hoc T2
                                 WHERE t2.id_truong = {1}
                                   and t2.ma_nam_hoc = {2}
                                   and t2.id_lop = {3}
                                   and t2.id_hoc_ky = 1
                                   and t2.tiet = {0})
                         WHERE T1.id IN (SELECT T2.id FROM ca_hoc T2 WHERE t2.id_truong = {1}
                                   and t2.ma_nam_hoc = {2}
                                   and t2.id_lop = {3}
                                   and t2.id_hoc_ky = 2 and t2.tiet = {0})", i, id_truong, id_nam_hoc, id_lop);
                        context.Database.ExecuteSqlCommand(query);
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CA_HOC");
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
