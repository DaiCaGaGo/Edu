using OneEduDataAccess.Model;
using OneEduDataAccess.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class TruongBO
    {
        #region get
        #region get tat ca cac truong
        public List<TRUONG> getTruong(string cap_hoc, string ten_truong, string ma_truong, bool? trang_thai, bool is_all = false, string id_all = "", string text_all = "Chọn tất cả")
        {
            List<TRUONG> data = new List<TRUONG>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TRUONG", "getTruong", cap_hoc, ten_truong, ma_truong, trang_thai, is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.TRUONGs where p.IS_DELETE != true select p);
                    if (cap_hoc == SYS_Cap_Hoc.MN)
                        tmp = tmp.Where(x => x.IS_MN == true);
                    if (cap_hoc == SYS_Cap_Hoc.TH)
                        tmp = tmp.Where(x => x.IS_TH == true);
                    if (cap_hoc == SYS_Cap_Hoc.THCS)
                        tmp = tmp.Where(x => x.IS_THCS == true);
                    if (cap_hoc == SYS_Cap_Hoc.THPT)
                        tmp = tmp.Where(x => x.IS_THPT == true);
                    if (cap_hoc == SYS_Cap_Hoc.GDTX)
                        tmp = tmp.Where(x => x.IS_GDTX == true);
                    if (!string.IsNullOrEmpty(ten_truong))
                        tmp = tmp.Where(x => x.TEN.ToLower().Contains(ten_truong.ToLower()));
                    if (!string.IsNullOrEmpty(ma_truong))
                        tmp = tmp.Where(x => x.MA.ToLower().Contains(ma_truong.ToLower()));
                    if (trang_thai != null)
                        tmp = tmp.Where(x => x.TRANG_THAI == trang_thai);
                    data = tmp.ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<TRUONG>();
                        TRUONG item_all = new TRUONG();
                        item_all.MA = id_all;
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
                    data = QICache.Get(strKeyCache) as List<TRUONG>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }

            return data;
        }
        #endregion
        #region get trường theo tỉnh, quận/huyện
        public List<TRUONG> getTruongByTinhHuyen(short? ma_tinh, short? ma_huyen, bool is_all = false, long id_all = 0, string text_all = "Chọn tất cả")
        {
            List<TRUONG> data = new List<TRUONG>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TRUONG", "getTruongByTinhHuyen", ma_tinh, ma_huyen, is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.TRUONGs where p.IS_DELETE != true select p);
                    if (ma_tinh != null)
                        tmp = tmp.Where(x => x.MA_TINH_THANH == ma_tinh);
                    if (ma_huyen != null)
                        tmp = tmp.Where(x => x.MA_QUAN_HUYEN == ma_huyen);
                    tmp = tmp.OrderBy(x => x.TEN);
                    data = tmp.ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<TRUONG>();
                        TRUONG item_all = new TRUONG();
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
                    data = QICache.Get(strKeyCache) as List<TRUONG>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        #endregion
        #region get truong theo id
        public TRUONG getTruongById(long id)
        {
            TRUONG data = new TRUONG();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TRUONG", "getTruongById", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.TRUONGs where p.ID == id && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as TRUONG;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        #endregion
        public TRUONG getTruongByMa(string ma)
        {
            TRUONG data = new TRUONG();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TRUONG", "getTruongByMa", ma);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.TRUONGs where p.MA == ma && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as TRUONG;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<TRUONG> getTruongAccessByNguoiDung(NGUOI_DUNGEntity user)
        {
            List<TRUONG> data = new List<TRUONG>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TRUONG", "getTruongAccessByNguoiDung", user.ID, user.IS_ROOT);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    if (user.IS_ROOT != null && user.IS_ROOT == true)
                        data = (from p in context.TRUONGs where p.IS_DELETE != true && p.TRANG_THAI == true select p).ToList();
                    else
                        data = (from p in context.TRUONGs
                                where p.IS_DELETE != true && p.TRANG_THAI == true && context.NGUOI_DUNG_TRUONG.Any(es => (es.TRANG_THAI == 1) && (es.ID_TRUONG == p.ID) && (es.ID_NGUOI_DUNG == user.ID))
                                select p).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<TRUONG>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }

            return data;
        }
        public List<TruongEntity> getTruongNotExistsNguoiDungTruong(long userID, short? id_doi_tac)
        {
            List<TruongEntity> data = new List<TruongEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TRUONG", "NGUOI_DUNG_TRUONG", "getTruongNotExistsNguoiDungTruong", userID, id_doi_tac);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = @"select ID, TEN 
                                        from truong 
                                        where not exists (select id_truong from nguoi_dung_truong where id_nguoi_dung = :0 and id_truong=truong.id and trang_thai = 1 and (is_delete = 0 or is_delete is null))";
                    if (id_doi_tac != null)
                        strQuery += string.Format(@" and ID_DOI_TAC={0}", id_doi_tac);
                    data = context.Database.SqlQuery<TruongEntity>(strQuery, userID).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<TruongEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }

            return data;
        }
        public List<TruongEntity> getTruongExistsNguoiDungTruong(long? userID)
        {
            List<TruongEntity> data = new List<TruongEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TRUONG", "NGUOI_DUNG_TRUONG", "getTruongExistsNguoiDungTruong", userID);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = @"select ID,TEN
                                        from truong 
                                        where exists (select id_truong from nguoi_dung_truong where id_nguoi_dung = :0 and id_truong=truong.id and trang_thai = 1 and (is_delete = 0 or is_delete is null))";

                    data = context.Database.SqlQuery<TruongEntity>(strQuery, userID).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<TruongEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }

            return data;
        }
        public List<TruongEntity> getTruongByDoiTac(short? id_doi_tac)
        {
            List<TruongEntity> data = new List<TruongEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("TRUONG", "getTruongByDoiTac", id_doi_tac);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = @"select * from TRUONG where not (is_delete is not null and is_delete=1)";
                    if (id_doi_tac != null)
                        strQuery += string.Format(@" and ID_DOI_TAC=" + id_doi_tac);
                    data = context.Database.SqlQuery<TruongEntity>(strQuery).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<TruongEntity>;
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
        public ResultEntity update(TRUONG detail_in, long? nguoi, bool is_update_quy_tin = false)
        {
            DataAccessAPI dataAccessAPI = new DataAccessAPI();
            QuyTinBO quyTinBO = new QuyTinBO();
            TRUONG detail = new TRUONG();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.TRUONGs
                              where p.ID == detail_in.ID && p.IS_DELETE != true
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.MA = detail_in.MA;
                        detail.TEN = detail_in.TEN;
                        detail.BRAND_NAME_VIETTEL = detail_in.BRAND_NAME_VIETTEL;
                        detail.BRAND_NAME_GTEL = detail_in.BRAND_NAME_GTEL;
                        detail.BRAND_NAME_MOBI = detail_in.BRAND_NAME_MOBI;
                        detail.BRAND_NAME_VINA = detail_in.BRAND_NAME_VINA;
                        detail.BRAND_NAME_VNM = detail_in.BRAND_NAME_VNM;
                        detail.CP_GTEL = detail_in.CP_GTEL;
                        detail.CP_MOBI = detail_in.CP_MOBI;
                        detail.CP_VIETTEL = detail_in.CP_VIETTEL;
                        detail.CP_VINA = detail_in.CP_VINA;
                        detail.CP_VNM = detail_in.CP_VNM;
                        detail.ID_DOI_TAC = detail_in.ID_DOI_TAC;
                        detail.DIA_CHI = detail_in.DIA_CHI;
                        detail.DIEN_THOAI = detail_in.DIEN_THOAI;
                        detail.EMAIL = detail_in.EMAIL;
                        detail.HIEU_TRUONG = detail_in.HIEU_TRUONG;
                        detail.DIEN_THOAI_HT = detail_in.DIEN_THOAI_HT;
                        detail.EMAIL_HT = detail_in.EMAIL_HT;
                        detail.DIEN_THOAI_NLH = detail_in.DIEN_THOAI_NLH;
                        detail.IS_TH = detail_in.IS_TH;
                        detail.IS_THCS = detail_in.IS_THCS;
                        detail.IS_THPT = detail_in.IS_THPT;
                        detail.IS_GDTX = detail_in.IS_GDTX;
                        detail.IS_MN = detail_in.IS_MN;
                        detail.TRANG_THAI = detail_in.TRANG_THAI;
                        detail.IS_ACTIVE_SMS = detail_in.IS_ACTIVE_SMS;
                        detail.IS_SAN_QUY_TIN_NAM = detail_in.IS_SAN_QUY_TIN_NAM;
                        short? MA_GOI_TIN_old = detail.MA_GOI_TIN;
                        detail.MA_GOI_TIN = detail_in.MA_GOI_TIN;
                        long? SO_HS_DANG_KY_old = detail.SO_HS_DANG_KY;
                        detail.SO_HS_DANG_KY = detail_in.SO_HS_DANG_KY;
                        detail.SO_HS_DUOC_MIEN = detail_in.SO_HS_DUOC_MIEN;
                        detail.MA_TINH_THANH = detail_in.MA_TINH_THANH;
                        detail.MA_QUAN_HUYEN = detail_in.MA_QUAN_HUYEN;
                        detail.MA_XA_PHUONG = detail_in.MA_XA_PHUONG;
                        detail.PHAN_TRAM_VUOT_HAN_MUC = detail_in.PHAN_TRAM_VUOT_HAN_MUC;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        detail.IS_DELETE = detail_in.IS_DELETE;
                        context.SaveChanges();
                        res.ResObject = detail;
                        #region Update quỹ tin
                        if (is_update_quy_tin && (MA_GOI_TIN_old != detail.MA_GOI_TIN || SO_HS_DANG_KY_old != detail.SO_HS_DANG_KY))
                        {
                            short nam_hoc = Convert.ToInt16(DateTime.Now.Year);
                            short thang = Convert.ToInt16(DateTime.Now.Month);
                            QUY_TIN qUY_TINLL = new QUY_TIN();
                            bool is_insert_new_quyll = false;
                            qUY_TINLL = quyTinBO.getQuyTin(nam_hoc, thang, detail.ID, SYS_Loai_Tin.Tin_Lien_Lac, nguoi, out is_insert_new_quyll);
                            QUY_TIN qUY_TINTB = new QUY_TIN();
                            bool is_insert_new_quytb = false;
                            qUY_TINTB = quyTinBO.getQuyTin(nam_hoc, thang, detail.ID, SYS_Loai_Tin.Tin_Thong_Bao, nguoi, out is_insert_new_quytb);
                        }
                        #endregion
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("TRUONG");
                QICache.RemoveByFirstName("QUY_TIN");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity nguoiDungUpdate(TRUONG detail_in, long? nguoi)
        {
            TRUONG detail = new TRUONG();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.TRUONGs
                              where p.ID == detail_in.ID && p.IS_DELETE != true
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.TEN = detail_in.TEN;
                        detail.DIA_CHI = detail_in.DIA_CHI;
                        detail.DIEN_THOAI = detail_in.DIEN_THOAI;
                        detail.EMAIL = detail_in.EMAIL;
                        detail.HIEU_TRUONG = detail_in.HIEU_TRUONG;
                        detail.DIEN_THOAI_HT = detail_in.DIEN_THOAI_HT;
                        detail.EMAIL_HT = detail_in.EMAIL_HT;
                        detail.DIEN_THOAI_NLH = detail_in.DIEN_THOAI_NLH;
                        detail.MA_TINH_THANH = detail_in.MA_TINH_THANH;
                        detail.MA_QUAN_HUYEN = detail_in.MA_QUAN_HUYEN;
                        detail.MA_XA_PHUONG = detail_in.MA_XA_PHUONG;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        detail.IS_DELETE = detail_in.IS_DELETE;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("TRUONG");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity updateQuyTinDoiTacCap(long id_truong, long them_tin, long so_tin_gui, long? nguoi)
        {
            TRUONG detail = new TRUONG();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.TRUONGs where p.ID == id_truong select p).FirstOrDefault();
                    if (detail != null)
                    {
                        long tong_cap = detail.TONG_TIN_CAP == null ? 0 : detail.TONG_TIN_CAP.Value;
                        long da_dung = detail.TONG_TIN_DA_DUNG == null ? 0 : detail.TONG_TIN_DA_DUNG.Value;
                        detail.TONG_TIN_CAP = tong_cap + them_tin;
                        detail.TONG_TIN_DA_DUNG = da_dung + so_tin_gui;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("TRUONG");
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
        public ResultEntity insert(TRUONG detail_in, long? nguoi)
        {
            DataAccessAPI dataAccessAPI = new DataAccessAPI();
            QuyTinBO quyTinBO = new QuyTinBO();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail_in.ID = context.Database.SqlQuery<long>("SELECT TRUONG_SEQ1.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                    detail_in.NGAY_TAO = DateTime.Now;
                    detail_in.NGUOI_TAO = nguoi;
                    detail_in = context.TRUONGs.Add(detail_in);
                    context.SaveChanges();
                    #region Update Quỹ tin
                    if (detail_in.IS_ACTIVE_SMS != null && detail_in.IS_ACTIVE_SMS == true && detail_in.MA_GOI_TIN != null)
                    {
                        short nam_hoc = Convert.ToInt16(DateTime.Now.Year);
                        short thang = Convert.ToInt16(DateTime.Now.Month);
                        ResultEntity res1 = quyTinBO.insert(nam_hoc, thang, detail_in.ID, SYS_Loai_Tin.Tin_Lien_Lac, nguoi);
                        ResultEntity res2 = quyTinBO.insert(nam_hoc, thang, detail_in.ID, SYS_Loai_Tin.Tin_Thong_Bao, nguoi);
                    }
                    #endregion
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("TRUONG");
                QICache.RemoveByFirstName("QUY_TIN");
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
        public ResultEntity delete(long ma, long? nguoi, bool is_delete = false)
        {
            TRUONG detail = new TRUONG();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    if (!is_delete)
                    {
                        var sql = @"update TRUONG set IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 where ID = :2";
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, ma);
                    }
                    else
                    {
                        var sql = @"delete from TRUONG where ID = :0";
                        context.Database.ExecuteSqlCommand(sql, ma);
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("TRUONG");
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
