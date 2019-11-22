using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class NhaCungCapThucPhamBO
    {
        #region get
        public List<DM_NHA_CUNG_CAP_THUC_PHAM> getAllNhaCungCap(bool is_all = false, long id_all = 0, string text_all = "Chọn tất cả")
        {
            List<DM_NHA_CUNG_CAP_THUC_PHAM> data = new List<DM_NHA_CUNG_CAP_THUC_PHAM>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_NHA_CUNG_CAP_THUC_PHAM", "getAllNhaCungCap", is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DM_NHA_CUNG_CAP_THUC_PHAM where p.IS_DELETE != true orderby p.TEN select p).ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<DM_NHA_CUNG_CAP_THUC_PHAM>();
                        DM_NHA_CUNG_CAP_THUC_PHAM item_all = new DM_NHA_CUNG_CAP_THUC_PHAM();
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
                    data = QICache.Get(strKeyCache) as List<DM_NHA_CUNG_CAP_THUC_PHAM>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<DM_NHA_CUNG_CAP_THUC_PHAM> getNhaCungCapByTruongTen(long id_truong, string ten)
        {
            List<DM_NHA_CUNG_CAP_THUC_PHAM> data = new List<DM_NHA_CUNG_CAP_THUC_PHAM>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_NHA_CUNG_CAP_THUC_PHAM", "getNhaCungCapByTruongTen", id_truong, ten);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var temp = (from p in context.DM_NHA_CUNG_CAP_THUC_PHAM where p.ID_TRUONG == id_truong && p.IS_DELETE != true select p);
                    if (!string.IsNullOrEmpty(ten))
                        temp = temp.Where(x => x.TEN.ToLower().Contains(ten.ToLower()));
                    temp = temp.OrderBy(x => x.TEN);
                    data = temp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<DM_NHA_CUNG_CAP_THUC_PHAM>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public DM_NHA_CUNG_CAP_THUC_PHAM getNhaCungCapByID(long id)
        {
            DM_NHA_CUNG_CAP_THUC_PHAM data = new DM_NHA_CUNG_CAP_THUC_PHAM();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_NHA_CUNG_CAP_THUC_PHAM", "getNhaCungCapByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DM_NHA_CUNG_CAP_THUC_PHAM where p.ID == id && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as DM_NHA_CUNG_CAP_THUC_PHAM;
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
            string strKeyCache = QICache.BuildCachedKey("DM_NHA_CUNG_CAP_THUC_PHAM", "getMaxThuTuByTruong", id_truong);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var sql = @"select MAX(THU_TU) as THU_TU from DM_NHA_CUNG_CAP_THUC_PHAM where ID_TRUONG=:0 and NOT (ID_DELETE is not null and ID_DELETE =1 )";
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
        public DM_NHA_CUNG_CAP_THUC_PHAM checkExist(long id_truong, string ten)
        {
            DM_NHA_CUNG_CAP_THUC_PHAM data = new DM_NHA_CUNG_CAP_THUC_PHAM();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_NHA_CUNG_CAP_THUC_PHAM", "checkExist", id_truong, ten);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DM_NHA_CUNG_CAP_THUC_PHAM where p.ID_TRUONG == id_truong && p.TEN.ToLower() == ten.ToLower() && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as DM_NHA_CUNG_CAP_THUC_PHAM;
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
        public ResultEntity update(DM_NHA_CUNG_CAP_THUC_PHAM detail_in, long? nguoi)
        {
            DM_NHA_CUNG_CAP_THUC_PHAM detail = new DM_NHA_CUNG_CAP_THUC_PHAM();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.DM_NHA_CUNG_CAP_THUC_PHAM
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();

                    if (detail != null)
                    {
                        detail.TEN = detail_in.TEN;
                        detail.ID_TRUONG = detail_in.ID_TRUONG;
                        detail.DIA_CHI = detail_in.DIA_CHI;
                        detail.SDT = detail_in.SDT;
                        detail.EMAIL = detail_in.EMAIL;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
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
            QICache.RemoveByFirstName("DM_NHA_CUNG_CAP_THUC_PHAM");
            return res;
        }
        public ResultEntity insert(DM_NHA_CUNG_CAP_THUC_PHAM detail_in, long? nguoi)
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
                    detail_in = context.DM_NHA_CUNG_CAP_THUC_PHAM.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DM_NHA_CUNG_CAP_THUC_PHAM");
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
            DM_NHA_CUNG_CAP_THUC_PHAM detail = new DM_NHA_CUNG_CAP_THUC_PHAM();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            string sql = string.Empty;
            try
            {
                using (var context = new oneduEntities())
                {

                    if (is_delete)
                    {
                        sql += @"DELETE DM_NHA_CUNG_CAP_THUC_PHAM WHERE ID = " + id.ToString();
                        int resKQ = context.Database.ExecuteSqlCommand(sql);
                    }
                    else
                    {
                        sql += @"UPDATE DM_NHA_CUNG_CAP_THUC_PHAM SET IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 WHERE ID=" + id.ToString();
                        int resKQ = context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }

            var QICache = new DefaultCacheProvider();
            QICache.RemoveByFirstName("DM_NHA_CUNG_CAP_THUC_PHAM");
            return res;
        }
        #endregion
    }
}
