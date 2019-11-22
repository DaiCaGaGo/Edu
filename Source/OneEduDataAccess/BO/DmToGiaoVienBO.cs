using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class DmToGiaoVienBO
    {
        #region get
        public List<TO_GIAO_VIEN> getToGiaoVien(long id_truong, string ten, bool is_all = false, long id_all = 0, string text_all = "Chọn tất cả")
        {
            List<TO_GIAO_VIEN> data = new List<TO_GIAO_VIEN>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TO_GIAO_VIEN", "getToGiaoVien", id_truong, ten, is_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    //data = (from p in context.TO_GIAO_VIEN where p.IS_DELETE != true orderby p.THU_TU select p).ToList();
                    var temp = (from p in context.TO_GIAO_VIEN where p.ID_TRUONG == id_truong && p.IS_DELETE != true select p);
                    if (!string.IsNullOrEmpty(ten))
                        temp = temp.Where(x => x.TEN.ToLower().Contains(ten.ToLower()));
                    temp = temp.OrderBy(x => x.THU_TU);
                    data = temp.ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<TO_GIAO_VIEN>();
                        TO_GIAO_VIEN item_all = new TO_GIAO_VIEN();
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
                    data = QICache.Get(strKeyCache) as List<TO_GIAO_VIEN>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public TO_GIAO_VIEN getToGiaoVienByID(long id)
        {
            TO_GIAO_VIEN data = new TO_GIAO_VIEN();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TO_GIAO_VIEN", "getToGiaoVienByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.TO_GIAO_VIEN where p.ID == id select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as TO_GIAO_VIEN;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public short? getMaxThuTuByTruong(long id_truong)
        {
            short? data = null;
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TO_GIAO_VIEN", "getMaxThuTuByTruong", id_truong);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var sql = @"select MAX(THU_TU) as THU_TU from TO_GIAO_VIEN where NOT (IS_DELETE is not null and IS_DELETE =1 ) and id_truong=:0";
                    data = context.Database.SqlQuery<short?>(sql, id_truong).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as short?;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        #endregion
        #region Tổ của giáo viên
        public ResultEntity insertOrUpdate(TO_GIAO_VIEN_GV detail_in, long? nguoi)
        {
            TO_GIAO_VIEN_GV detail = new TO_GIAO_VIEN_GV();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.TO_GIAO_VIEN_GV
                              where p.ID_TO == detail_in.ID_TO && p.ID_TRUONG == detail_in.ID_TRUONG && p.ID_GIAO_VIEN == detail_in.ID_GIAO_VIEN
                              select p).FirstOrDefault();

                    if (detail == null)
                    {
                        detail_in.ID = context.Database.SqlQuery<long>("SELECT TO_GIAO_VIEN_GV_SEQ.nextval FROM SYS.DUAL").FirstOrDefault();
                        detail_in.NGAY_TAO = DateTime.Now;
                        detail_in.NGUOI_TAO = nguoi;
                        detail_in.NGAY_SUA = DateTime.Now;
                        detail_in.NGUOI_SUA = nguoi;
                        detail_in = context.TO_GIAO_VIEN_GV.Add(detail_in);
                        context.SaveChanges();
                        res.ResObject = detail_in;
                    }
                    else
                    {
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("TO_GIAO_VIEN_GV");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public List<ToGiaoVienEntity> getGiaoVienNotExistsToGV(long ToGV, long id_truong)
        {
            List<ToGiaoVienEntity> data = new List<ToGiaoVienEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("GIAO_VIEN", "TO_GIAO_VIEN_GV", "getGiaoVienNotExistsToGV", ToGV, id_truong);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = @"select ID, TEN, HO_TEN 
                                        from GIAO_VIEN 
                                        where not exists (select id_giao_vien from TO_GIAO_VIEN_GV where id_to=:0 and id_giao_vien = GIAO_VIEN.ID and (is_delete = 0 or is_delete is null)) and MA_TRANG_THAI in (1,6) and id_truong=:1 and not (is_delete is not null and is_delete = 1)";
                    data = context.Database.SqlQuery<ToGiaoVienEntity>(strQuery, ToGV, id_truong).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<ToGiaoVienEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<ToGiaoVienEntity> getGiaoVienExistsToGiaoVien_GiaoVien(long? ID_ToGV)
        {
            List<ToGiaoVienEntity> data = new List<ToGiaoVienEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("GIAO_VIEN", "TO_GIAO_VIEN_GV", "getGiaoVienExistsToGiaoVien_GiaoVien", ID_ToGV);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = @"select ID, TEN, HO_TEN, SDT
                                        from giao_vien 
                                        where exists (select id_giao_vien from to_giao_vien_gv where id_to = :0 and id_giao_vien = GIAO_VIEN.ID and (is_delete = 0 or is_delete is null)) and MA_TRANG_THAI not in (4,5,7) and not (is_delete is not null and is_delete = 1)";
                    data = context.Database.SqlQuery<ToGiaoVienEntity>(strQuery, ID_ToGV).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<ToGiaoVienEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<DSGiaoVienTheoToEntity> getGiaoVienByToGV(long id_truong, List<short> lst_ma_to)
        {
            DataAccessAPI dataAccessAPI = new DataAccessAPI();
            List<DSGiaoVienTheoToEntity> data = new List<DSGiaoVienTheoToEntity>();
            string str_lst_ma_to = dataAccessAPI.ConvertListToString<short>(lst_ma_to, ",");
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("GIAO_VIEN", "TO_GIAO_VIEN_GV", "TIN_NHAN", "getGiaoVienByToGV", id_truong, str_lst_ma_to);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = "";
                    string strTemp = "";
                    if (lst_ma_to != null && lst_ma_to.Count > 0)
                    {
                        if (lst_ma_to.Count == 1)
                        {
                            strTemp = string.Format(@"select * from TO_GIAO_VIEN_GV tgv where g.ID=tgv.ID_GIAO_VIEN and tgv.ID_TO={0}", lst_ma_to[0]);
                        }
                        else if (lst_ma_to.Count > 1)
                        {
                            strTemp = string.Format(@"select tgv.* from TO_GIAO_VIEN_GV tgv where g.ID=tgv.ID_GIAO_VIEN and not (tgv.ID_TO !={0}", lst_ma_to[0]);
                            for (int i = 1; i < lst_ma_to.Count; i++)
                            {
                                strTemp += " and tgv.ID_TO != " + lst_ma_to[i];
                            }
                            strTemp += ")";
                        }
                        strQuery = @"select g.id, g.ten, g.ho_ten, g.sdt,case when sum(tn.so_tin)>0 then sum(tn.so_tin) else 0 end as SO_TIN_TRONG_NGAY
                            from GIAO_VIEN g
                            left join tin_nhan tn on g.id = tn.id_nguoi_nhan and g.id_truong = tn.id_truong 
                            and tn.loai_nguoi_nhan = 2 and tn.loai_tin = 2 and TRUNC(tn.ngay_tao)=TRUNC(:0)
                            where exists (" + strTemp;
                        strQuery += string.Format(@") and g.id_truong = {0} AND NOT(G.IS_delete is not null and g.is_delete = 1) group by g.id, g.ten, g.ho_ten, g.sdt
                            order by g.ten", id_truong);
                        data = context.Database.SqlQuery<DSGiaoVienTheoToEntity>(strQuery, DateTime.Now).ToList();
                    }
                    else
                    {
                        #region "hiển thị ds giáo viên trong trường"
                        strQuery = @"select gv.id, gv.ho_ten, gv.ten, gv.sdt
                            ,case when sum(tn.so_tin)>0 then sum(tn.so_tin) else 0 end as SO_TIN_TRONG_NGAY
                            from giao_vien gv
                            left join tin_nhan tn on gv.id=tn.id_nguoi_nhan and gv.id_truong=tn.id_truong 
                            and tn.loai_nguoi_nhan=2 and tn.loai_tin=2 and TRUNC(tn.ngay_tao)=TRUNC(:0)
                            where gv.id_truong=:1 and not (gv.MA_TRANG_THAI<>1 and gv.MA_TRANG_THAI<>6) and not (gv.is_delete is not null and gv.is_delete = 1)
                            group by gv.id, gv.ho_ten, gv.ten, gv.sdt order by gv.ten, gv.ho_ten";
                        data = context.Database.SqlQuery<DSGiaoVienTheoToEntity>(strQuery, DateTime.Now, id_truong).ToList();
                        #endregion
                    }
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<DSGiaoVienTheoToEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public GiaoVienInToEntity checkExistSdtGiaoVienInTo(long id_truong, long id_to_gv, string sdt)
        {
            GiaoVienInToEntity data = new GiaoVienInToEntity();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TO_GIAO_VIEN_GV", "GIAO_VIEN", "checkExistSdtGiaoVienInTo", id_truong, id_to_gv, sdt);
            try
            {
                if (!QICache.IsSet(strKeyCache))
                {
                    using (oneduEntities context = new oneduEntities())
                    {
                        var sql = string.Format(@"select tgv.*, gv.sdt,gv.ho_ten as ten_giao_vien
                    from to_giao_vien_gv tgv 
                    left join giao_vien gv on tgv.id_truong=gv.id_truong and tgv.id_giao_vien=gv.id
                    where tgv.id_truong=:0 and tgv.id_to=:1 and gv.sdt='{0}'", sdt);
                        data = context.Database.SqlQuery<GiaoVienInToEntity>(sql, id_truong, id_to_gv).FirstOrDefault();
                        QICache.Set(strKeyCache, data, 300000);
                    }
                }
                else
                {
                    try
                    {
                        data = QICache.Get(strKeyCache) as GiaoVienInToEntity;
                    }
                    catch
                    {
                        QICache.Invalidate(strKeyCache);
                    }
                }
            }
            catch (Exception ex) { }
            return data;
        }
        #endregion
        #region set
        public ResultEntity update(TO_GIAO_VIEN detail_in, long? USerID, long? ID)
        {
            TO_GIAO_VIEN detail = new TO_GIAO_VIEN();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    var sql = @"update TO_GIAO_VIEN set TEN=:0,THU_TU=:1,NGUOI_SUA=:2,NGAY_SUA=:3 where ID=:4";
                    context.Database.ExecuteSqlCommand(sql, detail_in.TEN, detail_in.THU_TU, USerID, DateTime.Now, ID);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("TO_GIAO_VIEN");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(TO_GIAO_VIEN detail_in, long? nguoi)
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
                    detail_in = context.TO_GIAO_VIEN.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("TO_GIAO_VIEN");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity delete(string ma, long? nguoi, bool is_delete = false)
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
                        var sql = @"update TO_GIAO_VIEN set IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 where ID = :2";
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, ma);
                    }
                    else
                    {
                        var sql = @"delete TO_GIAO_VIEN where ID = :0";
                        context.Database.ExecuteSqlCommand(sql, ma);
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("TO_GIAO_VIEN");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity deleteGiaoVien_in_ToGV(long? User, long idToGiaoVien, long idGiaoVien, bool is_delete = false)
        {
            TO_GIAO_VIEN_GV detail = new TO_GIAO_VIEN_GV();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.TO_GIAO_VIEN_GV
                              where p.IS_DELETE != true && p.ID_TO == idToGiaoVien && p.ID_GIAO_VIEN == idGiaoVien
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        if (!is_delete)
                        {
                            var sql = @"update TO_GIAO_VIEN_GV set IS_DELETE=1, NGAY_SUA=:0, NGUOI_SUA=:1 where ID=:2";
                            context.Database.ExecuteSqlCommand(sql, DateTime.Now, User, detail.ID);
                        }
                        else
                        {
                            var sql = @"delete from TO_GIAO_VIEN_GV where ID = :0";
                            context.Database.ExecuteSqlCommand(sql, detail.ID);
                        }
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("TO_GIAO_VIEN_GV");
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
