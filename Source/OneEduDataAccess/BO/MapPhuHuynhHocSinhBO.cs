using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class MapPhuHuynhHocSinhBO
    {
        HocSinhBO hocSinhBO = new HocSinhBO();
        public List<MAP_PH_HS> getDuLieuMap(string sdt_map, string ma_hs)
        {
            List<MAP_PH_HS> data = new List<MAP_PH_HS>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MAP_PH_HS", "getDuLieuMap", sdt_map, ma_hs);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.MAP_PH_HS where p.SDT_MAP == sdt_map && p.MA_HOC_SINH == ma_hs orderby p.NGAY_GUI_MA descending select p).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<MAP_PH_HS>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public MAP_PH_HS getMapDuLieuByID(long id)
        {
            MAP_PH_HS data = new MAP_PH_HS();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MAP_PH_HS", "getMapDuLieuByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.MAP_PH_HS where p.ID == id select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as MAP_PH_HS;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<MAP_PH_HS> getHocSinhByUserZaloID(string userid)
        {
            List<MAP_PH_HS> data = new List<MAP_PH_HS>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MAP_PH_HS", "getPhuHuynhByUserZaloID", userid);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.MAP_PH_HS where p.ZALO_USER_ID == userid && p.TRANG_THAI == true select p).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<MAP_PH_HS>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<ZaloTraCuuEntity> traCuuKetQuaHocTap(string fromuid)
        {
            List<ZaloTraCuuEntity> data = new List<ZaloTraCuuEntity>();
            var QICache = new DefaultCacheProvider();
            using (oneduEntities context = new oneduEntities())
            {
                List<MAP_PH_HS> lstHocSinh = getHocSinhByUserZaloID(fromuid);
                if (lstHocSinh.Count > 0)
                {
                    string strQuery = "";
                    string hoc_sinh = "";
                    for (int i = 0; i < lstHocSinh.Count; i++)
                    {
                        HOC_SINH detail = detail = hocSinhBO.getHocSinhByMa(lstHocSinh[i].MA_HOC_SINH);
                        if (detail != null) hoc_sinh += detail.ID + ",";
                    }
                    hoc_sinh = hoc_sinh.TrimEnd(',');
                    if (!string.IsNullOrEmpty(hoc_sinh))
                    {
                        strQuery = @"select ho_ten as title 
                            ,'oa.open.url' as type
                            ,'http://edu.onesms.vn/StudentInfor.aspx?id_hs=' || id as url
                            from hoc_sinh 
                            where id in (" + hoc_sinh + ")";
                    }
                    data = context.Database.SqlQuery<ZaloTraCuuEntity>(strQuery).ToList();
                }
            }
            return data;
        }
        public List<ZaloTraCuuEntity> traCuuDanhBaGiaoVien(string fromuid)
        {
            List<ZaloTraCuuEntity> data = new List<ZaloTraCuuEntity>();
            var QICache = new DefaultCacheProvider();
            using (oneduEntities context = new oneduEntities())
            {
                List<MAP_PH_HS> lstHocSinh = getHocSinhByUserZaloID(fromuid);
                if (lstHocSinh.Count > 0)
                {
                    string strQuery = "";
                    string hoc_sinh = "";
                    for (int i = 0; i < lstHocSinh.Count; i++)
                    {
                        HOC_SINH detail = detail = hocSinhBO.getHocSinhByMa(lstHocSinh[i].MA_HOC_SINH);
                        if (detail != null) hoc_sinh += detail.ID + ",";
                    }
                    hoc_sinh = hoc_sinh.TrimEnd(',');
                    if (!string.IsNullOrEmpty(hoc_sinh))
                    {
                        strQuery = @"select distinct hs.id_lop, 'Lớp ' || l.ten as title,
                            'https://edu.onesms.vn/TraCuuDanhBaGV.aspx?id_lop=' || hs.id_lop as url,'oa.open.url' as type
                            from hoc_sinh hs 
                            join lop l on hs.id_truong=l.id_truong and l.id=hs.id_lop
                            where hs.id_nam_hoc=2018 and hs.id in (" + hoc_sinh + ")";
                    }
                    data = context.Database.SqlQuery<ZaloTraCuuEntity>(strQuery).ToList();
                }
            }
            return data;
        }
        public List<ZaloTraCuuEntity> traCuuDanhBaChiHoiPhuHuynh(string fromuid)
        {
            List<ZaloTraCuuEntity> data = new List<ZaloTraCuuEntity>();
            var QICache = new DefaultCacheProvider();
            using (oneduEntities context = new oneduEntities())
            {
                List<MAP_PH_HS> lstHocSinh = getHocSinhByUserZaloID(fromuid);
                if (lstHocSinh.Count > 0)
                {
                    string strQuery = "";
                    string hoc_sinh = "";
                    for (int i = 0; i < lstHocSinh.Count; i++)
                    {
                        HOC_SINH detail = detail = hocSinhBO.getHocSinhByMa(lstHocSinh[i].MA_HOC_SINH);
                        if (detail != null) hoc_sinh += detail.ID + ",";
                    }
                    hoc_sinh = hoc_sinh.TrimEnd(',');
                    if (!string.IsNullOrEmpty(hoc_sinh))
                    {
                        strQuery = @"select distinct hs.id_lop, 'Lớp ' || l.ten as title,
                            'https://edu.onesms.vn/TraCuuDanhBaPhuHuynh.aspx?id_lop=' || hs.id_lop as url,
                            'oa.open.url' as type
                            from hoc_sinh hs 
                            join lop l on hs.id_truong=l.id_truong and l.id=hs.id_lop
                            where hs.id_nam_hoc=2018 and hs.id in (" + hoc_sinh + ")";
                    }
                    data = context.Database.SqlQuery<ZaloTraCuuEntity>(strQuery).ToList();
                }
            }
            return data;
        }
        public List<ZaloTraCuuEntity> traCuuThoiKhoaBieuLop(string fromuid)
        {
            List<ZaloTraCuuEntity> data = new List<ZaloTraCuuEntity>();
            var QICache = new DefaultCacheProvider();
            using (oneduEntities context = new oneduEntities())
            {
                List<MAP_PH_HS> lstHocSinh = getHocSinhByUserZaloID(fromuid);
                if (lstHocSinh.Count > 0)
                {
                    string strQuery = "";
                    string hoc_sinh = "";
                    for (int i = 0; i < lstHocSinh.Count; i++)
                    {
                        HOC_SINH detail = detail = hocSinhBO.getHocSinhByMa(lstHocSinh[i].MA_HOC_SINH);
                        if (detail != null) hoc_sinh += detail.ID + ",";
                    }
                    hoc_sinh = hoc_sinh.TrimEnd(',');
                    if (!string.IsNullOrEmpty(hoc_sinh))
                    {
                        strQuery = @"select distinct hs.id_lop, 'Lớp ' || l.ten as title,
                            'http://edu.onesms.vn/ThoiKhoaBieuLop.aspx?id_lop=' || hs.id_lop as url,
                            'oa.open.url' as type
                            from hoc_sinh hs 
                            join lop l on hs.id_truong=l.id_truong and l.id=hs.id_lop
                            where hs.id_nam_hoc=2018 and hs.id in (" + hoc_sinh + ")";
                    }
                    data = context.Database.SqlQuery<ZaloTraCuuEntity>(strQuery).ToList();
                }
            }
            return data;
        }
        public List<ZaloTraCuuEntity> traCuuTinTucTheoUser(string fromuid)
        {
            List<ZaloTraCuuEntity> data = new List<ZaloTraCuuEntity>();
            var QICache = new DefaultCacheProvider();
            using (oneduEntities context = new oneduEntities())
            {
                List<MAP_PH_HS> lstHocSinh = getHocSinhByUserZaloID(fromuid);
                if (lstHocSinh.Count > 0)
                {
                    string strQuery = "";
                    for (int i = 0; i < lstHocSinh.Count; i++)
                    {
                        HOC_SINH detail = detail = hocSinhBO.getHocSinhByMa(lstHocSinh[i].MA_HOC_SINH);
                        if (detail != null)
                        {
                            strQuery = @"select * from (select TIEU_DE as title, NOI_DUNG_TOM_TAT as subtitle, LINK as url, 'oa.open.url' as type, 'https://edu.onesms.vn' || substr(ANH_DAI_DIEN, 2, length(ANH_DAI_DIEN)) as image_url from tin_tuc where id_truong=" + detail.ID_TRUONG + " and MA_CAP_HOC='" + detail.MA_CAP_HOC + "' and ID_NAM_HOC=2018 order by ngay_su_kien desc) where rownum < 6";
                            break;
                        }
                    }
                    data = context.Database.SqlQuery<ZaloTraCuuEntity>(strQuery).ToList();
                }
            }
            return data;
        }
        public long? getCountIPMapTrongNgay(string zaloID, string ngay_truy_cap)
        {
            long? data = null;
            var QICache = new DefaultCacheProvider();
            using (oneduEntities context = new oneduEntities())
            {
                var sql = string.Format(@"select count(*) as count from MAP_PH_HS where ZALO_USER_ID='{0}' and to_char( trunc(NGAY_TRUY_CAP), 'dd/MM/yyyy')='{1}'", zaloID, ngay_truy_cap);
                data = context.Database.SqlQuery<long?>(sql).FirstOrDefault();
            }
            return data;
        }
        public List<ZaloTraCuuEntity> viewBaiTapVeNha(string fromuid, string ngay_btvn)
        {
            List<ZaloTraCuuEntity> data = new List<ZaloTraCuuEntity>();
            var QICache = new DefaultCacheProvider();
            using (oneduEntities context = new oneduEntities())
            {
                List<MAP_PH_HS> lstHocSinh = getHocSinhByUserZaloID(fromuid);
                if (lstHocSinh.Count > 0)
                {
                    string strQuery = "";
                    string hoc_sinh = "";
                    for (int i = 0; i < lstHocSinh.Count; i++)
                    {
                        HOC_SINH detail = detail = hocSinhBO.getHocSinhByMa(lstHocSinh[i].MA_HOC_SINH);
                        if (detail != null) hoc_sinh += detail.ID + ",";
                    }
                    hoc_sinh = hoc_sinh.TrimEnd(',');
                    if (!string.IsNullOrEmpty(hoc_sinh))
                    {
                        strQuery = string.Format(@"select distinct hs.id_lop, 'Lớp ' || l.ten as title,
                            'http://edu.onesms.vn/ViewBaiTapVeNha.aspx?id_lop=' || hs.id_lop || '&' || 'ngay_bai_tap=' || {0} as url,
                            'oa.open.url' as type
                            from hoc_sinh hs 
                            join lop l on hs.id_truong=l.id_truong and l.id=hs.id_lop
                            where hs.id_nam_hoc=2018 and hs.id in (" + hoc_sinh + ")", ngay_btvn);
                    }
                    data = context.Database.SqlQuery<ZaloTraCuuEntity>(strQuery).ToList();
                }
            }
            return data;
        }
        public ResultEntity insertFirst(int type, string sdt_map, string ma_hoc_sinh, string ma_bao_mat, string ipAddress, string userID_zalo)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    string time_action = DateTime.Now.ToString("dd/MM/yyyy");
                    long? count = getCountIPMapTrongNgay(userID_zalo, time_action);
                    if (count == null || count <= 3)
                    {
                        if (type == 1)// SĐT map khác với SĐT nhận tin
                        {
                            var sql = @"insert into MAP_PH_HS(SDT_MAP,MA_HOC_SINH,MA_BAO_MAT,TRANG_THAI,NGAY_GUI_MA,IP_ADDRESS,ZALO_USER_ID,NGAY_TRUY_CAP) values (:0,:1,:2,0,:3,:4,:5,:6)";
                            context.Database.ExecuteSqlCommand(sql, sdt_map, ma_hoc_sinh, ma_bao_mat, DateTime.Now, ipAddress, userID_zalo, DateTime.Now);
                        }
                        else if (type == 2)//SĐT map trùng với SĐT nhận tin
                        {
                            var sql = @"insert into MAP_PH_HS(SDT_MAP,MA_HOC_SINH,TRANG_THAI,IP_ADDRESS,ZALO_USER_ID,NGAY_TRUY_CAP) values (:0,:1,1,:2,:3,:4)";
                            context.Database.ExecuteSqlCommand(sql, sdt_map, ma_hoc_sinh, ipAddress, userID_zalo, DateTime.Now);
                        }
                    }
                    else
                    {
                        res.Res = false;
                        res.Msg = "Submit đã vượt quá 3 lần/ngày, bạn không thể thực hiện thao tác này";
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("MAP_PH_HS");
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
                    var sql = @"update MAP_PH_HS set TRANG_THAI=1 WHERE ID=:0";
                    context.Database.ExecuteSqlCommand(sql, id);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("MAP_PH_HS");
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
                    var sql = @"update MAP_PH_HS set MA_BAO_MAT=:0 WHERE ID=:1";
                    context.Database.ExecuteSqlCommand(sql, ma_bao_mat, id);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("MAP_PH_HS");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(MAP_PH_HS detail_in)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail_in.ID = context.Database.SqlQuery<long>("SELECT MAP_PH_HS_SEQ.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                    detail_in.NGAY_TRUY_CAP = DateTime.Now;
                    detail_in = context.MAP_PH_HS.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("MAP_PH_HS");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity update(MAP_PH_HS detail_in)
        {
            MAP_PH_HS detail = new MAP_PH_HS();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.MAP_PH_HS
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.SDT_MAP = detail_in.SDT_MAP;
                        detail.MA_HOC_SINH = detail_in.MA_HOC_SINH;
                        detail.ZALO_USER_ID = detail_in.ZALO_USER_ID;
                        detail.TRANG_THAI = detail_in.TRANG_THAI;
                        detail.NGAY_TAO = detail_in.NGAY_TAO;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("LOP");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
    }
}
