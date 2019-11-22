using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class DMSachChiTietBO
    {
        #region get
        public List<DM_SACH_CHI_TIET> getNoiDungChiTietSach(string ma_cap_hoc, short? id_khoi, short? id_mon_hoc, long? id_sach, long? id_bai_hoc)
        {
            List<DM_SACH_CHI_TIET> data = new List<DM_SACH_CHI_TIET>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_SACH_CHI_TIET", "getNoiDungChiTietSach", ma_cap_hoc, id_khoi, id_mon_hoc, id_sach, id_bai_hoc);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var temp = (from p in context.DM_SACH_CHI_TIET where p.IS_DELETE != true select p);
                    if (id_khoi != null) temp = temp.Where(p => p.ID_KHOI == id_khoi);
                    else
                    {
                        if (ma_cap_hoc == SYS_Cap_Hoc.MN) temp = temp.Where(p => p.ID_KHOI == 13 || p.ID_KHOI == 14);
                        else if (ma_cap_hoc == SYS_Cap_Hoc.TH) temp = temp.Where(p => p.ID_KHOI == 1 || p.ID_KHOI == 2 || p.ID_KHOI == 3 || p.ID_KHOI == 4 || p.ID_KHOI == 5);
                        else if (ma_cap_hoc == SYS_Cap_Hoc.THCS) temp = temp.Where(p => (p.ID_KHOI == 6 || p.ID_KHOI == 7 || p.ID_KHOI == 8 || p.ID_KHOI == 9));
                        else if (ma_cap_hoc == SYS_Cap_Hoc.THPT) temp = temp.Where(p => p.ID_KHOI == 10 || p.ID_KHOI == 11 || p.ID_KHOI == 12);
                    }
                    if (id_mon_hoc != null) temp = temp.Where(p => p.ID_MON == id_mon_hoc);
                    if (id_sach != null) temp = temp.Where(p => p.ID_SACH == id_sach);
                    if (id_bai_hoc != null) temp = temp.Where(p => p.ID_TEN_BAI_HOC == id_bai_hoc);
                    temp = temp.OrderBy(p => p.ID_MON).ThenBy(p => p.ID_TEN_BAI_HOC).ThenBy(p => p.TRANG_SO).ThenBy(p => p.BAI_SO);
                    data = temp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<DM_SACH_CHI_TIET>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public DM_SACH_CHI_TIET getChiTietSachByID(long id)
        {
            DM_SACH_CHI_TIET data = new DM_SACH_CHI_TIET();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_SACH_CHI_TIET", "getChiTietSachByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DM_SACH_CHI_TIET where p.ID == id select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as DM_SACH_CHI_TIET;
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
        public ResultEntity update(DM_SACH_CHI_TIET detail_in, long? nguoi)
        {
            DM_SACH_CHI_TIET detail = new DM_SACH_CHI_TIET();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.DM_SACH_CHI_TIET
                              where p.ID == detail_in.ID && p.IS_DELETE != true
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.ID_KHOI = detail_in.ID_KHOI;
                        detail.ID_MON = detail_in.ID_MON;
                        detail.ID_SACH = detail_in.ID_SACH;
                        detail.ID_TEN_BAI_HOC = detail_in.ID_TEN_BAI_HOC;
                        detail.BAI_SO = detail_in.BAI_SO;
                        detail.TRANG_SO = detail_in.TRANG_SO;
                        detail.ICON = detail_in.ICON;
                        detail.NOI_DUNG = detail_in.NOI_DUNG;
                        detail.GHI_CHU = detail_in.GHI_CHU;
                        detail.NGAY_TAO = DateTime.Now;
                        detail.NGUOI_TAO = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DM_SACH_CHI_TIET");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(DM_SACH_CHI_TIET detail_in, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail_in.ID = context.Database.SqlQuery<long>("SELECT DM_SACH_CHI_TIET_SEQ.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                    detail_in.NGAY_TAO = DateTime.Now;
                    detail_in.NGUOI_TAO = nguoi;
                    detail_in = context.DM_SACH_CHI_TIET.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DM_SACH_CHI_TIET");
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
                    var sql = @"delete DM_SACH_CHI_TIET where ID = :0";
                    context.Database.ExecuteSqlCommand(sql, id);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DM_SACH_CHI_TIET");
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
