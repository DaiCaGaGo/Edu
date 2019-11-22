using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class NhomThucPhamBO
    {
        #region get
        public List<DM_NHOM_THUC_PHAM> getNhomThucPham(string ten, bool is_all = false, short id_all = 0, string text_all = "Chọn tất cả")
        {
            List<DM_NHOM_THUC_PHAM> data = new List<DM_NHOM_THUC_PHAM>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_NHOM_THUC_PHAM", "getNhomThucPham", ten, is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var temp = (from p in context.DM_NHOM_THUC_PHAM where p.IS_DELETE != true select p);
                    if (!string.IsNullOrEmpty(ten))
                        temp = temp.Where(x => x.TEN.ToLower().Contains(ten.ToLower()));
                    temp = temp.OrderBy(x => x.THU_TU);
                    data = temp.ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<DM_NHOM_THUC_PHAM>();
                        DM_NHOM_THUC_PHAM item_all = new DM_NHOM_THUC_PHAM();
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
                    data = QICache.Get(strKeyCache) as List<DM_NHOM_THUC_PHAM>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public DM_NHOM_THUC_PHAM getNhomThucPhamByID(short id)
        {
            DM_NHOM_THUC_PHAM data = new DM_NHOM_THUC_PHAM();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_NHOM_THUC_PHAM", "getNhomThucPhamByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DM_NHOM_THUC_PHAM where p.ID == id && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as DM_NHOM_THUC_PHAM;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public short? getMaxThuTu()
        {
            short? data = null;
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_NHOM_THUC_PHAM", "getMaxThuTu");
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var sql = @"select MAX(THU_TU) as THU_TU from DM_NHOM_THUC_PHAM where NOT (IS_DELETE is not null and IS_DELETE =1 )";
                    data = context.Database.SqlQuery<short?>(sql).FirstOrDefault();
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
        public DM_NHOM_THUC_PHAM checkExistNhomThucPham(string ten)
        {
            DM_NHOM_THUC_PHAM data = new DM_NHOM_THUC_PHAM();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_NHOM_THUC_PHAM", "checkExistNhomThucPham", ten);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DM_NHOM_THUC_PHAM where p.TEN.ToLower() == ten.ToLower() && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as DM_NHOM_THUC_PHAM;
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
        public ResultEntity update(DM_NHOM_THUC_PHAM detail_in, long? nguoi)
        {
            DM_NHOM_THUC_PHAM detail = new DM_NHOM_THUC_PHAM();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.DM_NHOM_THUC_PHAM
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();

                    if (detail != null)
                    {
                        detail.TEN = detail_in.TEN;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        detail.THU_TU = detail_in.THU_TU;
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
            QICache.RemoveByFirstName("DM_NHOM_THUC_PHAM");
            return res;
        }
        public ResultEntity insert(DM_NHOM_THUC_PHAM detail_in, long? nguoi)
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
                    detail_in = context.DM_NHOM_THUC_PHAM.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DM_NHOM_THUC_PHAM");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity delete(short id, long? nguoi, bool is_delete = false)
        {
            DM_NHOM_THUC_PHAM detail = new DM_NHOM_THUC_PHAM();
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
                        sql += @"DELETE DM_NHOM_THUC_PHAM WHERE ID = " + id.ToString();
                        int resKQ = context.Database.ExecuteSqlCommand(sql);
                    }
                    else
                    {
                        sql += @"UPDATE DM_NHOM_THUC_PHAM SET IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 WHERE ID=" + id.ToString();
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
            QICache.RemoveByFirstName("DM_NHOM_THUC_PHAM");
            return res;
        }
        #endregion
    }
}
