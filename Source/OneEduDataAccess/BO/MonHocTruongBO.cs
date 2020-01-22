using OneEduDataAccess.Model;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class MonHocTruongBO
    {
        #region Get
        public List<MON_HOC_TRUONG> getMonHocByTruongNamHocAndKhoi(long id_truong, short id_nam_hoc, short? ma_khoi)
        {
            List<MON_HOC_TRUONG> data = new List<MON_HOC_TRUONG>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MON_HOC_TRUONG", "getMonHocByTruongNamHocAndKhoi", id_truong, id_nam_hoc, ma_khoi);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var temp = (from p in context.MON_HOC_TRUONG
                                where p.IS_DELETE != true && p.ID_TRUONG == id_truong && p.ID_NAM_HOC == id_nam_hoc
                                select p);
                    if (ma_khoi != null)
                    {
                        switch (ma_khoi)
                        {
                            case 1:
                                temp = temp.Where(p => p.IS_1 == true);
                                break;
                            case 2:
                                temp = temp.Where(p => p.IS_2 == true);
                                break;
                            case 3:
                                temp = temp.Where(p => p.IS_3 == true);
                                break;
                            case 4:
                                temp = temp.Where(p => p.IS_4 == true);
                                break;
                            case 5:
                                temp = temp.Where(p => p.IS_5 == true);
                                break;
                            case 6:
                                temp = temp.Where(p => p.IS_6 == true);
                                break;
                            case 7:
                                temp = temp.Where(p => p.IS_7 == true);
                                break;
                            case 8:
                                temp = temp.Where(p => p.IS_8 == true);
                                break;
                            case 9:
                                temp = temp.Where(p => p.IS_9 == true);
                                break;
                            case 10:
                                temp = temp.Where(p => p.IS_10 == true);
                                break;
                            case 11:
                                temp = temp.Where(p => p.IS_11 == true);
                                break;
                            case 12:
                                temp = temp.Where(p => p.IS_12 == true);
                                break;
                        }
                    }
                    temp = temp.OrderBy(p => p.THU_TU).ThenBy(p => p.TEN);
                    data = temp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<MON_HOC_TRUONG>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<MON_HOC_TRUONG> getMonHocByTruongAndMaCapHoc(long id_truong, string ma_cap_hoc, short id_nam_hoc)
        {
            List<MON_HOC_TRUONG> data = new List<MON_HOC_TRUONG>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MON_HOC_TRUONG", "getMonHocByTruongAndMaCapHoc", id_truong, ma_cap_hoc, id_nam_hoc);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var temp = (from p in context.MON_HOC_TRUONG
                                where p.IS_DELETE != true && p.ID_TRUONG == id_truong && p.MA_CAP_HOC == ma_cap_hoc && p.ID_NAM_HOC == id_nam_hoc
                                select p);
                    temp = temp.OrderBy(p => p.THU_TU).ThenBy(p => p.TEN);
                    data = temp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<MON_HOC_TRUONG>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<MON_HOC_TRUONG> getListMonTruong(long id_truong, short id_nam_hoc, string ma_cap_hoc, string ten, short? kieu_mon, int mon_chuyen, short? loai_lop_GDTX)
        {
            List<MON_HOC_TRUONG> data = new List<MON_HOC_TRUONG>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MON_HOC_TRUONG", "getListMonTruong", id_truong, id_nam_hoc, ma_cap_hoc, ten, kieu_mon, mon_chuyen, loai_lop_GDTX);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var temp = (from p in context.MON_HOC_TRUONG
                                where p.IS_DELETE != true && p.ID_TRUONG == id_truong && p.ID_NAM_HOC == id_nam_hoc && p.MA_CAP_HOC == ma_cap_hoc
                                select p);
                    if (!String.IsNullOrEmpty(ten))
                        temp = temp.Where(x => x.TEN.ToLower().Contains(ten.ToLower()));
                    if (kieu_mon == 1)
                        temp = temp.Where(x => x.KIEU_MON == true);
                    if (kieu_mon == 0)
                        temp = temp.Where(x => x.KIEU_MON != true);
                    if (mon_chuyen == 1)
                        temp = temp.Where(x => x.IS_MON_CHUYEN == true);
                    if (loai_lop_GDTX != null && loai_lop_GDTX == 2)
                    {
                        temp = temp.Where(x => x.IS_6 == true || x.IS_7 == true || x.IS_8 == true || x.IS_9 == true);
                    }
                    else if (loai_lop_GDTX != null && loai_lop_GDTX == 3)
                    {
                        temp = temp.Where(x => x.IS_10 == true || x.IS_11 == true || x.IS_12 == true);
                    }
                    temp = temp.OrderBy(p => p.THU_TU).ThenBy(p => p.TEN);
                    data = temp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<MON_HOC_TRUONG>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public MON_HOC_TRUONG getMonTruongByID(long id)
        {
            MON_HOC_TRUONG data = new MON_HOC_TRUONG();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MON_HOC_TRUONG", "getMonTruongByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.MON_HOC_TRUONG where p.ID == id && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as MON_HOC_TRUONG;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public short? getMaxThuTuByTruongNamHoc(long id_truong, short id_nam_hoc, string ma_cap_hoc)
        {
            short? data = null;
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MON_HOC_TRUONG", "getMaxThuTuByTruongNamHoc", id_truong, id_nam_hoc, ma_cap_hoc);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var sql = @"select MAX(THU_TU) as THU_TU from MON_HOC_TRUONG 
                                    where ID_TRUONG = :0 and ID_NAM_HOC = :1 and MA_CAP_HOC = :2";
                    data = context.Database.SqlQuery<short?>(sql, id_truong, id_nam_hoc, ma_cap_hoc).FirstOrDefault();
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
        #region set
        #region update
        public ResultEntity update(MON_HOC_TRUONG detail_in, long? nguoi)
        {
            MON_HOC_TRUONG detail = new MON_HOC_TRUONG();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.MON_HOC_TRUONG
                              where p.ID == detail_in.ID && p.IS_DELETE != true
                              select p).FirstOrDefault();

                    if (detail != null)
                    {
                        detail.TEN = detail_in.TEN;
                        detail.IS_1 = detail_in.IS_1;
                        detail.IS_2 = detail_in.IS_2;
                        detail.IS_3 = detail_in.IS_3;
                        detail.IS_4 = detail_in.IS_4;
                        detail.IS_5 = detail_in.IS_5;
                        detail.IS_6 = detail_in.IS_6;
                        detail.IS_7 = detail_in.IS_7;
                        detail.IS_8 = detail_in.IS_8;
                        detail.IS_9 = detail_in.IS_9;
                        detail.IS_10 = detail_in.IS_10;
                        detail.IS_11 = detail_in.IS_11;
                        detail.IS_12 = detail_in.IS_12;
                        detail.KIEU_MON = detail_in.KIEU_MON;
                        detail.THU_TU = detail_in.THU_TU;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        detail.MA_CAP_HOC = detail_in.MA_CAP_HOC;
                        detail.HE_SO = detail_in.HE_SO;
                        detail.ID_MON_HOC = detail_in.ID_MON_HOC;
                        detail.IS_MON_CHUYEN = detail_in.IS_MON_CHUYEN;
                        detail.ID_TRUONG = detail_in.ID_TRUONG;
                        detail.ID_NAM_HOC = detail_in.ID_NAM_HOC;
                        context.SaveChanges();
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("MON_HOC_TRUONG");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        #endregion
        #region delete
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
                        var sql = @"update MON_HOC_TRUONG set IS_DELETE = 1, NGUOI_TAO=:0, NGAY_TAO=:1 where ID=:2";
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, id);
                    }
                    else
                    {
                        var sql = @"delete from MON_HOC_TRUONG where ID = :0";
                        context.Database.ExecuteSqlCommand(sql, id);
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("MON_HOC_TRUONG");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }

            return res;
        }
        #endregion
        #region insertEmpty
        public ResultEntity insertEmptyMonHoc(long id_truong, short id_nam_hoc, string ma_cap_hoc)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    string sql = string.Format(@"insert into mon_hoc_truong
                        (TEN,IS_1,IS_2,IS_3,IS_4,IS_5,IS_6,IS_7,IS_8,IS_9,IS_10,IS_11,IS_12
                        , kieu_mon, thu_tu, ma_cap_hoc, he_so, id_mon_hoc, id_truong, id_nam_hoc)
                        select TEN,IS_1,IS_2,IS_3,IS_4,IS_5,IS_6,IS_7
                        ,IS_8,IS_9,IS_10,IS_11,IS_12, kieu_mon, thu_tu
                        , ma_cap_hoc, he_so,id as id_mon_hoc
                        ,:0 as id_truong,:1 as id_nam_hoc 
                        from mon_hoc 
                        where not EXISTS (select * from mon_hoc_truong mht 
                                            where mon_hoc.id=mht.id_mon_hoc 
                                           and mht.id_truong = :2 and mht.id_nam_hoc = :3 )
                        and ma_cap_hoc = :4 and not (is_delete is not null and is_delete = 1)");
                    context.Database.ExecuteSqlCommand(sql, id_truong, id_nam_hoc, id_truong, id_nam_hoc, ma_cap_hoc);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("MON_HOC_TRUONG");
                QICache.RemoveByFirstName("MON_HOC");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        #endregion
        public ResultEntity insert(MON_HOC_TRUONG detail_in, long? nguoi)
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
                    context.MON_HOC_TRUONG.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("MON_HOC_TRUONG");
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
