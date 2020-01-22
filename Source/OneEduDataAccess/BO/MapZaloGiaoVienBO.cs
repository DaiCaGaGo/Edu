using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class MapZaloGiaoVienBO
    {
        GiaoVienBO giaoVienBO = new GiaoVienBO();
        LopBO lopBO = new LopBO();
        public MAP_ZALO_GV getGiaoVienByPhone(string sdt_map, string sdt_sms)
        {
            MAP_ZALO_GV data = new MAP_ZALO_GV();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MAP_ZALO_GV", "getGiaoVienByPhone", sdt_map, sdt_sms);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.MAP_ZALO_GV where p.SDT_GV == sdt_map && p.SDT_NHAN_SMS == sdt_sms && p.TRANG_THAI == true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as MAP_ZALO_GV;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public MAP_ZALO_GV getDataByPhone(string phone)
        {
            MAP_ZALO_GV data = new MAP_ZALO_GV();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MAP_ZALO_GV", "getDataByPhone", phone);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.MAP_ZALO_GV where p.SDT_GV == phone && p.TRANG_THAI == true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as MAP_ZALO_GV;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public MAP_ZALO_GV getGiaoVienByZaloID(string zaloID)
        {
            MAP_ZALO_GV data = new MAP_ZALO_GV();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MAP_ZALO_GV", "getGiaoVienByZaloID", zaloID);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.MAP_ZALO_GV where p.ZALO_USER_ID == zaloID && p.TRANG_THAI == true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as MAP_ZALO_GV;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public MAP_ZALO_GV getMapDuLieuByID(long id)
        {
            MAP_ZALO_GV data = new MAP_ZALO_GV();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MAP_ZALO_GV", "getMapDuLieuByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.MAP_ZALO_GV where p.ID == id select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as MAP_ZALO_GV;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public ResultEntity insert(MAP_ZALO_GV detail_in)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail_in.ID = context.Database.SqlQuery<long>("SELECT MAP_ZALO_GV_SEQ.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                    detail_in.NGAY_TRUY_CAP = DateTime.Now;
                    detail_in = context.MAP_ZALO_GV.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("MAP_ZALO_GV");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity updateMaXacNhan(long id, string ma_bao_mat)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    var sql = @"update MAP_ZALO_GV set MA_XAC_NHAN=:0 WHERE ID=:1";
                    context.Database.ExecuteSqlCommand(sql, ma_bao_mat, id);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("MAP_ZALO_GV");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity updateTrangThaiDuyet(long id)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    var sql = @"update MAP_ZALO_GV set TRANG_THAI=1 WHERE ID=:0";
                    context.Database.ExecuteSqlCommand(sql, id);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("MAP_ZALO_GV");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public List<ZaloTraCuuEntity> traCuuThoiKhoaBieuLop(string fromuid, short id_nam_hoc)
        {
            List<ZaloTraCuuEntity> data = new List<ZaloTraCuuEntity>();
            var QICache = new DefaultCacheProvider();
            using (oneduEntities context = new oneduEntities())
            {
                MAP_ZALO_GV detail = getGiaoVienByZaloID(fromuid);
                if (detail != null)
                {
                    List<GIAO_VIEN> listGV = giaoVienBO.getGiaoVienByPhone(detail.SDT_NHAN_SMS);
                    if (listGV.Count > 0)
                    {
                        string strLop = "";
                        for (int i = 0; i < listGV.Count; i++)
                        {
                            LOP lop = lopBO.getLopByGVCN(id_nam_hoc, listGV[i].ID);
                            if (lop != null) strLop += lop.ID + ",";
                        }
                        strLop = strLop.TrimEnd(',');
                        if (!string.IsNullOrEmpty(strLop))
                        {
                            string strQuery = @"select distinct l.id, 'Lớp ' || l.ten as title,
                            'https://edu.1sms.vn/TraCuu/ThoiKhoaBieuLop.aspx?id_lop=' || l.id as url,
                            'oa.open.url' as type
                            from lop l where l.id in (" + strLop + ")";
                            data = context.Database.SqlQuery<ZaloTraCuuEntity>(strQuery).ToList();
                        }
                    }
                }
            }
            return data;
        }
        public List<ZaloTraCuuEntity> traLichThiLop(string fromuid, short id_nam_hoc)
        {
            List<ZaloTraCuuEntity> data = new List<ZaloTraCuuEntity>();
            var QICache = new DefaultCacheProvider();
            using (oneduEntities context = new oneduEntities())
            {
                MAP_ZALO_GV detail = getGiaoVienByZaloID(fromuid);
                if (detail != null)
                {
                    List<GIAO_VIEN> listGV = giaoVienBO.getGiaoVienByPhone(detail.SDT_NHAN_SMS);
                    if (listGV.Count > 0)
                    {
                        string strLop = "";
                        for (int i = 0; i < listGV.Count; i++)
                        {
                            LOP lop = lopBO.getLopByGVCN(id_nam_hoc, listGV[i].ID);
                            if (lop != null) strLop += lop.ID + ",";
                        }
                        strLop = strLop.TrimEnd(',');
                        if (!string.IsNullOrEmpty(strLop))
                        {
                            string strQuery = @"select distinct l.id, 'Lớp ' || l.ten as title,
                            'https://edu.1sms.vn/ThoiKhoaBieuLop.aspx?id_lop=' || l.id as url,
                            'oa.open.url' as type
                            from lop l where l.id in (" + strLop + ")";
                            data = context.Database.SqlQuery<ZaloTraCuuEntity>(strQuery).ToList();
                        }
                    }
                }
            }
            return data;
        }
        public List<ZaloTraCuuEntity> traCuuKetQuaHocTap(string fromuid, short id_nam_hoc)
        {
            List<ZaloTraCuuEntity> data = new List<ZaloTraCuuEntity>();
            var QICache = new DefaultCacheProvider();
            using (oneduEntities context = new oneduEntities())
            {
                MAP_ZALO_GV detail = getGiaoVienByZaloID(fromuid);
                if (detail != null)
                {
                    List<GIAO_VIEN> listGV = giaoVienBO.getGiaoVienByPhone(detail.SDT_NHAN_SMS);
                    if (listGV.Count > 0)
                    {
                        string strLop = "";
                        for (int i = 0; i < listGV.Count; i++)
                        {
                            LOP lop = lopBO.getLopByGVCN(id_nam_hoc, listGV[i].ID);
                            if (lop != null) strLop += lop.ID + ",";
                        }
                        strLop = strLop.TrimEnd(',');
                        if (!string.IsNullOrEmpty(strLop))
                        {
                            string strQuery = @" select 'Lớp ' || ten as title 
                            ,'oa.open.url' as type
                            ,'https://demo.1sms.vn/StudentInfor.aspx?id_hs=' || id as url
                            from lop
                            where id in (" + strLop + ")";
                            data = context.Database.SqlQuery<ZaloTraCuuEntity>(strQuery).ToList();
                        }
                    }
                }
            }
            return data;
        }
        public List<ZaloTraCuuEntity> viewBaiTapVeNha(string fromuid, short id_nam_hoc)
        {
            List<ZaloTraCuuEntity> data = new List<ZaloTraCuuEntity>();
            var QICache = new DefaultCacheProvider();
            using (oneduEntities context = new oneduEntities())
            {
                MAP_ZALO_GV detail = getGiaoVienByZaloID(fromuid);
                if (detail != null)
                {
                    List<GIAO_VIEN> listGV = giaoVienBO.getGiaoVienByPhone(detail.SDT_NHAN_SMS);
                    if (listGV.Count > 0)
                    {
                        string strLop = "";
                        for (int i = 0; i < listGV.Count; i++)
                        {
                            LOP lop = lopBO.getLopByGVCN(id_nam_hoc, listGV[i].ID);
                            if (lop != null) strLop += lop.ID + ",";
                        }
                        strLop = strLop.TrimEnd(',');
                        if (!string.IsNullOrEmpty(strLop))
                        {
                            string strQuery = string.Format(@"select distinct l.id, 'Lớp ' || l.ten as title,
                            'https://edu.1sms.vn/ViewBaiTapVeNha.aspx?id_lop=' || l.id as url,
                            'oa.open.url' as type
                            from lop l where l.id in (" + strLop + ")");
                            data = context.Database.SqlQuery<ZaloTraCuuEntity>(strQuery).ToList();
                        }
                    }
                }
            }
            return data;
        }
        public List<ZaloTraCuuEntity> traCuuDanhBaGiaoVienTruong(string fromuid, short id_nam_hoc)
        {
            List<ZaloTraCuuEntity> data = new List<ZaloTraCuuEntity>();
            var QICache = new DefaultCacheProvider();
            using (oneduEntities context = new oneduEntities())
            {
                MAP_ZALO_GV detail = getGiaoVienByZaloID(fromuid);
                if (detail != null)
                {
                    List<GIAO_VIEN> listGV = giaoVienBO.getGiaoVienByPhone(detail.SDT_NHAN_SMS);
                    if (listGV.Count > 0)
                    {
                        long id_truong = listGV[0].ID_TRUONG;
                        string strQuery = @"select distinct gv.id_truong, 'GV trường ' || t.ten as title,
                            'https://edu.1sms.vn/TraCuu/DanhSachGiaoVien.aspx?id_truong=' || id_truong as url,'oa.open.url' as type
                            from giao_vien gv join truong t on gv.id_truong = t.id
                            where id_truong=" + id_truong;
                        data = context.Database.SqlQuery<ZaloTraCuuEntity>(strQuery).ToList();
                    }
                }
            }
            return data;
        }
        public List<ZaloTraCuuEntity> traCuuDanhBaGiaoVienBoMon(string fromuid, short id_nam_hoc)
        {
            List<ZaloTraCuuEntity> data = new List<ZaloTraCuuEntity>();
            var QICache = new DefaultCacheProvider();
            using (oneduEntities context = new oneduEntities())
            {
                MAP_ZALO_GV detail = getGiaoVienByZaloID(fromuid);
                if (detail != null)
                {
                    List<GIAO_VIEN> listGV = giaoVienBO.getGiaoVienByPhone(detail.SDT_NHAN_SMS);
                    if (listGV.Count > 0)
                    {
                        string strLop = "";
                        for (int i = 0; i < listGV.Count; i++)
                        {
                            LOP lop = lopBO.getLopByGVCN(id_nam_hoc, listGV[i].ID);
                            if (lop != null) strLop += lop.ID + ",";
                        }
                        strLop = strLop.TrimEnd(',');
                        if (!string.IsNullOrEmpty(strLop))
                        {
                            string strQuery = @"select distinct l.id, 'Lớp ' || l.ten as title,
                            'https://edu.1sms.vn/TraCuuDanhBaGV.aspx?id_lop=' || l.id as url,'oa.open.url' as type
                            from lop l where l.id in (" + strLop + ")";
                            data = context.Database.SqlQuery<ZaloTraCuuEntity>(strQuery).ToList();
                        }
                    }
                }
            }
            return data;
        }
        public List<ZaloTraCuuEntity> traCuuDanhBaChiHoiPhuHuynh(string fromuid, short id_nam_hoc)
        {
            List<ZaloTraCuuEntity> data = new List<ZaloTraCuuEntity>();
            var QICache = new DefaultCacheProvider();
            using (oneduEntities context = new oneduEntities())
            {
                MAP_ZALO_GV detail = getGiaoVienByZaloID(fromuid);
                if (detail != null)
                {
                    List<GIAO_VIEN> listGV = giaoVienBO.getGiaoVienByPhone(detail.SDT_NHAN_SMS);
                    if (listGV.Count > 0)
                    {
                        string strLop = "";
                        for (int i = 0; i < listGV.Count; i++)
                        {
                            LOP lop = lopBO.getLopByGVCN(id_nam_hoc, listGV[i].ID);
                            if (lop != null) strLop += lop.ID + ",";
                        }
                        strLop = strLop.TrimEnd(',');
                        if (!string.IsNullOrEmpty(strLop))
                        {
                            string strQuery = @"select distinct l.id, 'Lớp ' || l.ten as title,
                            'https://edu.1sms.vn/TraCuuDanhBaPhuHuynh.aspx?id_lop=' || l.id as url,'oa.open.url' as type
                            from lop l where l.id in (" + strLop + ")";
                            data = context.Database.SqlQuery<ZaloTraCuuEntity>(strQuery).ToList();
                        }
                    }
                }
            }
            return data;
        }
        public List<ZaloTraCuuEntity> traCuuThoiKhoaBieuGiaoVien(string fromuid, short id_nam_hoc)
        {
            List<ZaloTraCuuEntity> data = new List<ZaloTraCuuEntity>();
            var QICache = new DefaultCacheProvider();
            using (oneduEntities context = new oneduEntities())
            {
                MAP_ZALO_GV detail = getGiaoVienByZaloID(fromuid);
                if (detail != null)
                {
                    List<GIAO_VIEN> listGV = giaoVienBO.getGiaoVienByPhone(detail.SDT_NHAN_SMS);
                    if (listGV.Count > 0)
                    {
                        long id_gv = listGV[0].ID;
                        string strQuery = @"select distinct id, 'GV ' || ten as title,
                            'https://edu.1sms.vn/TraCuu/LichDayGV.aspx?id_giao_vien=' || id as url,
                            'oa.open.url' as type
                            from giao_vien where id=" + id_gv;
                        data = context.Database.SqlQuery<ZaloTraCuuEntity>(strQuery).ToList();
                    }
                }
            }
            return data;
        }
    }
}