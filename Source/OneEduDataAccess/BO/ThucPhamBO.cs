using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class ThucPhamBO
    {
        #region get
        public List<DM_THUC_PHAM> getThucPham(short? id_nhom, string ten, bool is_all = false, long id_all = 0, string text_all = "Chọn tất cả")
        {
            List<DM_THUC_PHAM> data = new List<DM_THUC_PHAM>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_THUC_PHAM", "getThucPham", id_nhom, ten, is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var temp = (from p in context.DM_THUC_PHAM where p.IS_DELETE != true select p);
                    if (id_nhom != null)
                        temp = temp.Where(x => x.ID_NHOM_THUC_PHAM == id_nhom);
                    if (!string.IsNullOrEmpty(ten))
                        temp = temp.Where(x => x.TEN.ToLower().Contains(ten.ToLower()));
                    temp = temp.OrderBy(x => x.THU_TU);
                    data = temp.ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<DM_THUC_PHAM>();
                        DM_THUC_PHAM item_all = new DM_THUC_PHAM();
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
                    data = QICache.Get(strKeyCache) as List<DM_THUC_PHAM>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public DM_THUC_PHAM getThucPhamByID(long id)
        {
            DM_THUC_PHAM data = new DM_THUC_PHAM();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_THUC_PHAM", "getThucPhamByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DM_THUC_PHAM where p.ID == id && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as DM_THUC_PHAM;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public DM_THUC_PHAM getThucPhamByNhomAndID(short id_nhom, long id)
        {
            DM_THUC_PHAM data = new DM_THUC_PHAM();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_THUC_PHAM", "getThucPhamByNhomAndID", id_nhom, id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DM_THUC_PHAM where p.ID_NHOM_THUC_PHAM == id_nhom && p.ID == id && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as DM_THUC_PHAM;
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
            string strKeyCache = QICache.BuildCachedKey("DM_THUC_PHAM", "getMaxThuTu");
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var sql = @"select MAX(THU_TU) as THU_TU from DM_THUC_PHAM where NOT (IS_DELETE is not null and IS_DELETE =1 )";
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
        public DM_THUC_PHAM checkExistThucPham(short id_nhom, string ten)
        {
            DM_THUC_PHAM data = new DM_THUC_PHAM();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_THUC_PHAM", "checkExistThucPham", id_nhom, ten);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DM_THUC_PHAM where p.ID_NHOM_THUC_PHAM == id_nhom && p.TEN.ToLower() == ten.ToLower() && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as DM_THUC_PHAM;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public short? getDonViTinhByThucPham(short? id_nhom_thuc_pham, long? id_thuc_pham)
        {
            short? data = null;
            DM_THUC_PHAM detail = new DM_THUC_PHAM();
            if (id_thuc_pham != null && id_nhom_thuc_pham != null) detail = getThucPhamByNhomAndID(id_nhom_thuc_pham.Value, id_thuc_pham.Value);
            if (detail != null) data = detail.DON_VI_TINH;
            return data;
        }
        public List<ThucPhamInThucDonEntity> getThucPhamInThucDon(long id_truong, short? ma_khoi, long id_thuc_don, short? id_nhom_thuc_pham, string ten_thuc_pham)
        {
            List<ThucPhamInThucDonEntity> data = new List<ThucPhamInThucDonEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_THUC_PHAM", "THUC_DON_CHI_TIET", "getThucPhamInThucDon", id_truong, ma_khoi, id_thuc_don, id_nhom_thuc_pham, ten_thuc_pham);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var strQuery = string.Format(@"select tp.id, tp.id_nhom_thuc_pham, tp.ten, tp.nang_luong_kcal, tp.protid, tp.glucid, tp.lipid
                        ,tblthucdon.nang_luong_kcal_new,tblthucdon.protid_new,tblthucdon.glucid_new,tblthucdon.lipid_new
                        ,tblThucDon.is_delete,tblThucDon.id as id_thuc_don_chi_tiet, tblthucdon.so_luong
                        ,case when (tblThucDon.id is not null and (tblThucDon.is_delete is null or tblThucDon.is_delete=0)) then 1 else 0 end is_chon
                        ,tp.don_vi_tinh
                        from DM_thuc_pham tp
                        left join (select tdct.id, tdct.id_thuc_don, tdct.id_thuc_pham, tdct.glucid as glucid_new, tdct.lipid as lipid_new
                        ,tdct.so_luong, tdct.nang_luong_kcal as nang_luong_kcal_new, tdct.protid as protid_new
                        ,tdct.id as id_thuc_don_chi_tiet, tdct.id_khoi,tdct.is_delete
                        from thuc_don_chi_tiet tdct
                        where tdct.id_truong=:0 and id_thuc_don=:1 {0}) tblThucDon
                        on tp.id=tblThucDon.id_thuc_pham
                        where not (tp.is_delete is not null and tp.is_delete=1)", (ma_khoi != null && ma_khoi > 0) ? " and tdct.id_khoi=" + ma_khoi.Value : "");
                    List<object> parameterList = new List<object>();
                    parameterList.Add(id_truong);
                    parameterList.Add(id_thuc_don);
                    if (id_nhom_thuc_pham != null)
                    {
                        strQuery += " and tp.id_nhom_thuc_pham=:" + parameterList.Count;
                        parameterList.Add(id_nhom_thuc_pham);
                    }
                    if (!string.IsNullOrEmpty(ten_thuc_pham))
                    {
                        strQuery += string.Format(" and lower(tp.ten) like lower(N'%{0}%')", ten_thuc_pham);
                    }
                    strQuery += " order by tp.id_nhom_thuc_pham,tp.thu_tu";
                    data = context.Database.SqlQuery<ThucPhamInThucDonEntity>(strQuery, parameterList.ToArray()).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<ThucPhamInThucDonEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<DM_THUC_PHAM> getThucPhamByNhom(short id_nhom)
        {
            List<DM_THUC_PHAM> data = new List<DM_THUC_PHAM>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DM_THUC_PHAM", "getThucPhamByNhom", id_nhom);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DM_THUC_PHAM where p.ID_NHOM_THUC_PHAM == id_nhom && p.IS_DELETE != true select p).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<DM_THUC_PHAM>;
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
        public ResultEntity update(DM_THUC_PHAM detail_in, long? nguoi)
        {
            DM_THUC_PHAM detail = new DM_THUC_PHAM();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.DM_THUC_PHAM
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();

                    if (detail != null)
                    {
                        detail.ID_NHOM_THUC_PHAM = detail_in.ID_NHOM_THUC_PHAM;
                        detail.TEN = detail_in.TEN;
                        detail.TEN_EN = detail_in.TEN_EN;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        detail.THU_TU = detail_in.THU_TU;
                        detail.DON_VI_TINH = detail_in.DON_VI_TINH;
                        detail.PHAN_TRAM_THAI_BO = detail_in.PHAN_TRAM_THAI_BO;
                        detail.NANG_LUONG_KCAL = detail_in.NANG_LUONG_KCAL;
                        detail.PROTID = detail_in.PROTID;
                        detail.GLUCID = detail_in.GLUCID;
                        detail.LIPID = detail_in.LIPID;
                        detail.PROTID_WEIGH = detail_in.PROTID_WEIGH;
                        detail.GLUCID_WEIGH = detail_in.GLUCID_WEIGH;
                        detail.LIPID_WEIGH = detail_in.LIPID_WEIGH;
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
            QICache.RemoveByFirstName("DM_THUC_PHAM");
            return res;
        }
        public ResultEntity insert(DM_THUC_PHAM detail_in, long? nguoi)
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
                    detail_in = context.DM_THUC_PHAM.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DM_THUC_PHAM");
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
            DM_THUC_PHAM detail = new DM_THUC_PHAM();
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
                        sql += @"DELETE DM_THUC_PHAM WHERE ID = " + id.ToString();
                        int resKQ = context.Database.ExecuteSqlCommand(sql);
                    }
                    else
                    {
                        sql += @"UPDATE DM_THUC_PHAM SET IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 WHERE ID=" + id.ToString();
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
            QICache.RemoveByFirstName("DM_THUC_PHAM");
            return res;
        }
        #endregion
    }
}
