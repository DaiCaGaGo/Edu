using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class MonHocBO
    {
        #region get
        #region get tat ca cac MON_HOC
        public List<MON_HOC> getMonHoc(bool is_all = false, short id_all = 0, string text_all = "Chọn tất cả")
        {
            List<MON_HOC> data = new List<MON_HOC>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MON_HOC", "getMonHoc", is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.MON_HOC where p.IS_DELETE != true select p).ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<MON_HOC>();
                        MON_HOC item_all = new MON_HOC();
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
                    data = QICache.Get(strKeyCache) as List<MON_HOC>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<MON_HOC> getMonHocByMaCap(string ma_cap_hoc)
        {
            List<MON_HOC> data = new List<MON_HOC>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MON_HOC", "getMonHocByMaCap", ma_cap_hoc);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.MON_HOC where p.MA_CAP_HOC == ma_cap_hoc && p.IS_DELETE != true select p).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<MON_HOC>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<Mon_MonTruongEntity> getNoneMonToMonTruong(long id_truong, short id_nam_hoc, string ma_cap_hoc)
        {
            List<Mon_MonTruongEntity> data = new List<Mon_MonTruongEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MON_HOC", "MON_HOC_TRUONG", "getNoneMonToMonTruong", id_truong, id_nam_hoc, ma_cap_hoc);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var sql = @"select * from (
                        select ten, MA_CAP_HOC , id as id_mon
                        ,:0 as id_nam_hoc, :1 as id_truong 
                        from mon_hoc where ma_cap_hoc = :2
                        ) tbInsert
                        where not exists (select * from mon_hoc_truong 
                        where mon_hoc_truong.ID_MON_HOC = tbInsert.id_mon 
                        and mon_hoc_truong.ma_cap_hoc = tbInsert.ma_cap_hoc
                        and mon_hoc_truong.id_truong = :1 and mon_hoc_truong.id_nam_hoc = :0)";
                    context.Database.SqlQuery<Mon_MonTruongEntity>(sql, id_nam_hoc, id_truong, ma_cap_hoc).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<Mon_MonTruongEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<MonHocEntity> getMonHocByKhoiTen(string cap_hoc, short idKhoi, string ten)
        {
            List<MonHocEntity> data = new List<MonHocEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MON_HOC", "getMonHocByKhoiTen", cap_hoc, idKhoi, ten);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var temp = (from p in context.MON_HOC
                                where p.IS_DELETE != true
                                select new MonHocEntity()
                                {
                                    ID = p.ID,
                                    TEN = p.TEN,
                                    IS_1 = p.IS_1,
                                    IS_2 = p.IS_2,
                                    IS_3 = p.IS_3,
                                    IS_4 = p.IS_4,
                                    IS_5 = p.IS_5,
                                    IS_6 = p.IS_6,
                                    IS_7 = p.IS_7,
                                    IS_8 = p.IS_8,
                                    IS_9 = p.IS_9,
                                    IS_10 = p.IS_10,
                                    IS_11 = p.IS_11,
                                    IS_12 = p.IS_12,
                                    KIEU_MON = p.KIEU_MON,
                                    KIEU_MON_STR = p.KIEU_MON == true ? "Điểm nhận xét" : "Điểm số",
                                    THU_TU = p.THU_TU,
                                    NGAY_SUA = p.NGAY_SUA,
                                    NGAY_TAO = p.NGAY_TAO,
                                    NGUOI_SUA = p.NGUOI_SUA,
                                    NGUOI_TAO = p.NGUOI_TAO,
                                    IS_DELETE = p.IS_DELETE,
                                    MA_CAP_HOC = p.MA_CAP_HOC,
                                    HE_SO = p.HE_SO
                                });
                    if (!string.IsNullOrEmpty(cap_hoc))
                        temp = temp.Where(p => p.MA_CAP_HOC == cap_hoc);
                    if (cap_hoc == "TH")
                    {
                        if (idKhoi == 1) temp = temp.Where(p => p.IS_1 == true);
                        else if (idKhoi == 2) temp = temp.Where(p => p.IS_2 == true);
                        else if (idKhoi == 3) temp = temp.Where(p => p.IS_3 == true);
                        else if (idKhoi == 4) temp = temp.Where(p => p.IS_4 == true);
                        else if (idKhoi == 5) temp = temp.Where(p => p.IS_5 == true);
                    }
                    else if (cap_hoc == "THCS")
                    {
                        if (idKhoi == 6) temp = temp.Where(p => p.IS_6 == true);
                        else if (idKhoi == 7) temp = temp.Where(p => p.IS_7 == true);
                        else if (idKhoi == 8) temp = temp.Where(p => p.IS_8 == true);
                        else if (idKhoi == 9) temp = temp.Where(p => p.IS_9 == true);
                    }
                    else if (cap_hoc == "THPT")
                    {
                        if (idKhoi == 10) temp = temp.Where(p => p.IS_10 == true);
                        else if (idKhoi == 11) temp = temp.Where(p => p.IS_11 == true);
                        else if (idKhoi == 12) temp = temp.Where(p => p.IS_12 == true);
                    }

                    if (!string.IsNullOrEmpty(ten))
                        temp = temp.Where(p => p.TEN.ToLower().Contains(ten.ToLower()));
                    temp = temp.OrderBy(p => p.THU_TU).ThenBy(p => p.TEN);
                    data = temp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<MonHocEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<MonHocEntity> getMonHocByKhoiCapHoc(string cap_hoc, short? idKhoi)
        {
            List<MonHocEntity> data = new List<MonHocEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MON_HOC", "getMonHocByKhoiCapHoc", cap_hoc, idKhoi);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    if (idKhoi != null)
                    {
                        var temp = (from p in context.MON_HOC
                                    where p.IS_DELETE != true
                                    select new MonHocEntity()
                                    {
                                        ID = p.ID,
                                        TEN = p.TEN,
                                        IS_1 = p.IS_1,
                                        IS_2 = p.IS_2,
                                        IS_3 = p.IS_3,
                                        IS_4 = p.IS_4,
                                        IS_5 = p.IS_5,
                                        IS_6 = p.IS_6,
                                        IS_7 = p.IS_7,
                                        IS_8 = p.IS_8,
                                        IS_9 = p.IS_9,
                                        IS_10 = p.IS_10,
                                        IS_11 = p.IS_11,
                                        IS_12 = p.IS_12,
                                        KIEU_MON = p.KIEU_MON,
                                        KIEU_MON_STR = p.KIEU_MON == true ? "Điểm nhận xét" : "Điểm số",
                                        THU_TU = p.THU_TU,
                                        NGAY_SUA = p.NGAY_SUA,
                                        NGAY_TAO = p.NGAY_TAO,
                                        NGUOI_SUA = p.NGUOI_SUA,
                                        NGUOI_TAO = p.NGUOI_TAO,
                                        IS_DELETE = p.IS_DELETE,
                                        MA_CAP_HOC = p.MA_CAP_HOC,
                                        HE_SO = p.HE_SO
                                    });
                        if (!string.IsNullOrEmpty(cap_hoc))
                            temp = temp.Where(p => p.MA_CAP_HOC == cap_hoc);
                        if (cap_hoc == "TH")
                        {
                            if (idKhoi == 1) temp = temp.Where(p => p.IS_1 == true);
                            else if (idKhoi == 2) temp = temp.Where(p => p.IS_2 == true);
                            else if (idKhoi == 3) temp = temp.Where(p => p.IS_3 == true);
                            else if (idKhoi == 4) temp = temp.Where(p => p.IS_4 == true);
                            else if (idKhoi == 5) temp = temp.Where(p => p.IS_5 == true);
                        }
                        else if (cap_hoc == "THCS")
                        {
                            if (idKhoi == 6) temp = temp.Where(p => p.IS_6 == true);
                            else if (idKhoi == 7) temp = temp.Where(p => p.IS_7 == true);
                            else if (idKhoi == 8) temp = temp.Where(p => p.IS_8 == true);
                            else if (idKhoi == 9) temp = temp.Where(p => p.IS_9 == true);
                        }
                        else if (cap_hoc == "THPT")
                        {
                            if (idKhoi == 10) temp = temp.Where(p => p.IS_10 == true);
                            else if (idKhoi == 11) temp = temp.Where(p => p.IS_11 == true);
                            else if (idKhoi == 12) temp = temp.Where(p => p.IS_12 == true);
                        }

                        temp = temp.OrderBy(p => p.THU_TU).ThenBy(p => p.TEN);
                        data = temp.ToList();
                    }
                    
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<MonHocEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        #endregion
        #region get MON_HOC theo id
        public MON_HOC getMonHocByID(short id)
        {
            MON_HOC data = new MON_HOC();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MON_HOC", "getMonHocByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.MON_HOC where p.ID == id && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as MON_HOC;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        #endregion
        #region "Check môn học đã tồn tại chưa"
        public List<MON_HOC> checkMonByTenKieu(string ten, bool kieu)
        {
            List<MON_HOC> data = new List<MON_HOC>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MON_HOC", "checkMonByTenKieu", ten, kieu);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.MON_HOC where p.IS_DELETE != true && p.TEN.ToLower() == ten.ToLower() && p.KIEU_MON == kieu select p).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<MON_HOC>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        #endregion
        #endregion

        #region set
        #region update
        public ResultEntity update(MON_HOC detail_in, long? nguoi)
        {
            MON_HOC detail = new MON_HOC();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.MON_HOC
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
                        context.SaveChanges();
                    }
                }
                var QICache = new DefaultCacheProvider();
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
        #region insert
        public ResultEntity insert(MON_HOC detail_in, long? nguoi)
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
                    context.MON_HOC.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
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
        #region delete
        public ResultEntity delete(short ma, long? nguoi, bool is_delete = false)
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
                        var sql = @"update MON_HOC set IS_DELETE = 1, NGUOI_TAO=:0, NGAY_TAO=:1 where ID = :2";
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, ma);
                    }
                    else
                    {
                        var sql = @"delete from MON_HOC where ID = :0";
                        context.Database.ExecuteSqlCommand(sql, ma);
                    }
                }
                var QICache = new DefaultCacheProvider();
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
        #endregion
    }
}
