using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class DMBuaAnBO
    {
        #region Get
        public DM_BUA_AN getById(long id)
        {
            DM_BUA_AN data = new DM_BUA_AN();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_BUA_AN", "getById", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DM_BUA_AN
                            where p.ID == id && p.IS_DELETE != true
                            select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as DM_BUA_AN;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<DM_BUA_AN> getDM_BUA_ANByTruongKhoiAndNhomTuoi(long idTruong, short? id_khoi, short? id_nhom_tuoi_mn, string ten = "")
        {
            List<DM_BUA_AN> data = new List<DM_BUA_AN>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_BUA_AN", "getDM_BUA_ANByTruongKhoiAndNhomTuoi", idTruong, id_khoi, id_nhom_tuoi_mn, ten);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.DM_BUA_AN where p.IS_DELETE != true && p.ID_TRUONG == idTruong select p);
                    if (id_khoi != null)
                        tmp = tmp.Where(x => x.ID_KHOI == id_khoi);
                    if (id_nhom_tuoi_mn != null)
                        tmp = tmp.Where(x => x.ID_NHOM_TUOI_MN == id_nhom_tuoi_mn);
                    if (!string.IsNullOrEmpty(ten.Trim()))
                        tmp = tmp.Where(x => x.TEN.Contains(ten));
                    tmp = tmp.OrderBy(x => x.ID_KHOI).ThenBy(x => x.THU_TU).ThenBy(x => x.TEN);
                    data = tmp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<DM_BUA_AN>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<DM_BUA_AN> getBuaAnByTruongKhoi(long id_truong, short? id_khoi, bool is_all = false, long id_all = 0, string text_all = "Chọn tất cả")
        {
            List<DM_BUA_AN> data = new List<DM_BUA_AN>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_BUA_AN", "getBuaAnByTruongKhoi", id_truong, id_khoi, is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DM_BUA_AN where p.ID_TRUONG == id_truong && p.ID_KHOI == id_khoi && p.IS_DELETE != true select p).ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<DM_BUA_AN>();
                        DM_BUA_AN item_all = new DM_BUA_AN();
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
                    data = QICache.Get(strKeyCache) as List<DM_BUA_AN>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        #endregion
        #region Set
        #region update
        public ResultEntity update(DM_BUA_AN detail_in, long? nguoi)
        {
            DM_BUA_AN detail = new DM_BUA_AN();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.DM_BUA_AN
                              where p.ID == detail_in.ID && p.IS_DELETE != true
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.GHI_CHU = detail_in.GHI_CHU;
                        detail.GIA_TIEN = detail_in.GIA_TIEN;
                        detail.GLUCID_DEN = detail_in.GLUCID_DEN;
                        detail.TEN = detail_in.TEN;
                        detail.GLUCID_TU = detail_in.GLUCID_TU;
                        detail.ID = detail_in.ID;
                        detail.ID_KHOI = detail_in.ID_KHOI;
                        detail.ID_NHOM_TUOI_MN = detail_in.ID_NHOM_TUOI_MN;
                        detail.ID_TRUONG = detail_in.ID_TRUONG;
                        detail.IS_DELETE = detail_in.IS_DELETE;
                        detail.LIPID_DEN = detail_in.LIPID_DEN;
                        detail.LIPID_TU = detail_in.LIPID_TU;
                        detail.NANG_LUONG_KCAL_TU = detail_in.NANG_LUONG_KCAL_TU;
                        detail.NANG_LUONG_KCAL_DEN = detail_in.NANG_LUONG_KCAL_DEN;
                        detail.PROTID_DEN = detail_in.PROTID_DEN;
                        detail.PROTID_TU = detail_in.PROTID_TU;
                        detail.THU_TU = detail_in.THU_TU;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        detail.NANG_LUONG_TU_KCAL = detail_in.NANG_LUONG_TU_KCAL;
                        detail.NANG_LUONG_DEN_KCAL = detail_in.NANG_LUONG_DEN_KCAL;
                        detail.PROTID_TU_KCAL = detail_in.PROTID_TU_KCAL;
                        detail.PROTID_DEN_KCAL = detail_in.PROTID_DEN_KCAL;
                        detail.LIPID_TU_KCAL = detail_in.LIPID_TU_KCAL;
                        detail.LIPID_DEN_KCAL = detail_in.LIPID_DEN_KCAL;
                        detail.GLUCID_TU_KCAL = detail_in.GLUCID_TU_KCAL;
                        detail.GLUCID_DEN_KCAL = detail_in.GLUCID_DEN_KCAL;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DM_BUA_AN");
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
        public ResultEntity insert(DM_BUA_AN detail_in, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail_in.ID = context.Database.SqlQuery<long>("SELECT DM_BUA_AN_SEQ.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                    detail_in.NGAY_TAO = DateTime.Now;
                    detail_in.NGUOI_TAO = nguoi;
                    detail_in.NGAY_SUA = DateTime.Now;
                    detail_in.NGUOI_SUA = nguoi;
                    detail_in = context.DM_BUA_AN.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DM_BUA_AN");
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
        public ResultEntity delete(long id, long? nguoi, bool is_delete_all, bool is_delete = false)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            string sql = string.Empty;
            try
            {
                using (var context = new oneduEntities())
                {
                    if (is_delete_all)
                    {
                        sql = "delete from DM_BUA_AN where ID = :0";
                        context.Database.ExecuteSqlCommand(sql, id);
                    }
                    else if (!is_delete)
                    {
                        sql = @"update DM_BUA_AN set  IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 where ID = :2";
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, id);
                    }
                    else
                    {
                        sql = "delete from DM_BUA_AN where ID = :0";
                        context.Database.ExecuteSqlCommand(sql, id);
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DM_BUA_AN");
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
