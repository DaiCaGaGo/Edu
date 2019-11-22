using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class NguoiDungBO
    {
        #region Get
        public List<NGUOI_DUNG> getNguoiDung(bool is_all = false, long id_all = 0, string text_all = "Chọn tất cả")
        {
            List<NGUOI_DUNG> data = new List<NGUOI_DUNG>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("NGUOI_DUNG", "getNguoiDung", is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.NGUOI_DUNG select p).ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<NGUOI_DUNG>();
                        NGUOI_DUNG item_all = new NGUOI_DUNG();
                        item_all.ID = id_all;
                        item_all.TEN_DANG_NHAP = text_all;
                        data.Insert(0, item_all);
                    }
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<NGUOI_DUNG>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<NGUOI_DUNGEntity> getAllNguoiDung(string userName, bool is_all = false, long id_all = 0, string text_all = "Chọn tất cả")
        {
            List<NGUOI_DUNGEntity> data = new List<NGUOI_DUNGEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("NGUOI_DUNG", "NHOM_QUYEN", "getAllNguoiDung", userName, is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from nd in context.NGUOI_DUNG
                               join nq in context.NHOM_QUYEN on nd.MA_NHOM_QUYEN equals nq.MA
                               where nd.IS_DELETE != true
                               select new NGUOI_DUNGEntity()
                               {
                                   ID = nd.ID,
                                   TEN_DANG_NHAP = nd.TEN_DANG_NHAP,
                                   TEN_HIEN_THI = nd.TEN_HIEN_THI,
                                   FACE_BOOK = nd.FACE_BOOK,
                                   EMAIL = nd.EMAIL,
                                   SDT = nd.SDT,
                                   DIA_CHI = nd.DIA_CHI,
                                   NGAY_SUA = nd.NGAY_SUA,
                                   QUYEN = nq.TEN
                               });
                    if (!string.IsNullOrEmpty(userName))
                        tmp = tmp.Where(x => x.TEN_DANG_NHAP.Contains(userName));
                    data = tmp.ToList();

                    if (is_all)
                    {
                        if (data == null) data = new List<NGUOI_DUNGEntity>();
                        NGUOI_DUNGEntity item_all = new NGUOI_DUNGEntity();
                        item_all.ID = id_all;
                        item_all.TEN_DANG_NHAP = text_all;
                        data.Insert(0, item_all);
                    }
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<NGUOI_DUNGEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<NGUOI_DUNGEntity> getNguoiDungByTruong(long id_truong, string userName, bool is_all = false, long id_all = 0, string text_all = "Chọn tất cả")
        {
            List<NGUOI_DUNGEntity> data = new List<NGUOI_DUNGEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("NGUOI_DUNG", "NHOM_QUYEN", "NGUOI_DUNG_TRUONG", "getNguoiDungByTruong", id_truong, userName, is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from nd in context.NGUOI_DUNG
                               join nq in context.NHOM_QUYEN on nd.MA_NHOM_QUYEN equals nq.MA
                               join ndt in context.NGUOI_DUNG_TRUONG on nd.ID equals ndt.ID_NGUOI_DUNG
                               where nd.IS_DELETE != true && ndt.ID_TRUONG == id_truong
                               && nd.IS_ROOT != true && nd.TRANG_THAI == true
                               select new NGUOI_DUNGEntity()
                               {
                                   ID = nd.ID,
                                   TEN_DANG_NHAP = nd.TEN_DANG_NHAP,
                                   TEN_HIEN_THI = nd.TEN_HIEN_THI,
                                   FACE_BOOK = nd.FACE_BOOK,
                                   EMAIL = nd.EMAIL,
                                   SDT = nd.SDT,
                                   DIA_CHI = nd.DIA_CHI,
                                   NGAY_SUA = nd.NGAY_SUA,
                                   QUYEN = nq.TEN
                               });
                    if (!string.IsNullOrEmpty(userName))
                        tmp = tmp.Where(x => x.TEN_DANG_NHAP.Contains(userName));
                    data = tmp.ToList();

                    if (is_all)
                    {
                        if (data == null) data = new List<NGUOI_DUNGEntity>();
                        NGUOI_DUNGEntity item_all = new NGUOI_DUNGEntity();
                        item_all.ID = id_all;
                        item_all.TEN_DANG_NHAP = text_all;
                        data.Insert(0, item_all);
                    }
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<NGUOI_DUNGEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public NGUOI_DUNG getLoginEmail(string email)
        {
            NGUOI_DUNG data = new NGUOI_DUNG();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("NGUOI_DUNG", "getLoginEmail", email);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.NGUOI_DUNG where p.EMAIL.Equals(email) && p.IS_LOGIN_GMAIL == true && p.IS_DELETE != true && p.TRANG_THAI == true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as NGUOI_DUNG;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public NGUOI_DUNG getLogin(string user_name, string pass)
        {
            NGUOI_DUNG data = new NGUOI_DUNG();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("NGUOI_DUNG", "getLogin", user_name, pass);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var cc11 = (from p in context.NGUOI_DUNG where p.TEN_DANG_NHAP.Equals(user_name) && p.MAT_KHAU.Equals(pass) && p.IS_DELETE != true && p.TRANG_THAI == true select p);
                    data = (from p in context.NGUOI_DUNG where p.TEN_DANG_NHAP.Equals(user_name) && p.MAT_KHAU.Equals(pass) && p.IS_DELETE != true && p.TRANG_THAI == true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as NGUOI_DUNG;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public NGUOI_DUNG getNguoiDungByID(long? id)
        {
            NGUOI_DUNG data = new NGUOI_DUNG();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("NGUOI_DUNG", "getNguoiDungByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.NGUOI_DUNG where p.ID == id && p.IS_DELETE != true && p.TRANG_THAI == true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as NGUOI_DUNG;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }

        public NGUOI_DUNG checkNguoiDungByPhone(string phone)
        {
            var QICache = new DefaultCacheProvider();
            string strSession = QICache.BuildCachedKey("NGUOI_DUNG", "checkNguoiDungByPhone", phone);
            NGUOI_DUNG detail = new NGUOI_DUNG();
            if (!QICache.IsSet(strSession))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    detail = (from p in context.NGUOI_DUNG
                              where p.SDT == phone
                              select p).FirstOrDefault();
                    QICache.Set(strSession, detail, 300000);
                }
            }
            else
            {
                try
                {
                    detail = QICache.Get(strSession) as NGUOI_DUNG;
                }
                catch
                {
                    QICache.Invalidate(strSession);
                }
            }
            return detail;
        }
        public NGUOI_DUNG checkNguoiDungByUser(string user)
        {
            var QICache = new DefaultCacheProvider();
            string strSession = QICache.BuildCachedKey("NGUOI_DUNG", "checkNguoiDungByUser", user);
            NGUOI_DUNG detail = new NGUOI_DUNG();
            if (!QICache.IsSet(strSession))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    detail = (from p in context.NGUOI_DUNG
                              where p.TEN_DANG_NHAP == user
                              select p).FirstOrDefault();
                    QICache.Set(strSession, detail, 300000);
                }
            }
            else
            {
                try
                {
                    detail = QICache.Get(strSession) as NGUOI_DUNG;
                }
                catch
                {
                    QICache.Invalidate(strSession);
                }
            }
            return detail;
        }
        public NGUOI_DUNG checkNguoiDungByEmail(string email)
        {
            var QICache = new DefaultCacheProvider();
            string strSession = QICache.BuildCachedKey("NGUOI_DUNG", "checkNguoiDungByEmail", email);
            NGUOI_DUNG detail = new NGUOI_DUNG();
            if (!QICache.IsSet(strSession))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    detail = (from p in context.NGUOI_DUNG
                              where p.EMAIL == email
                              select p).FirstOrDefault();
                    QICache.Set(strSession, detail, 300000);
                }
            }
            else
            {
                try
                {
                    detail = QICache.Get(strSession) as NGUOI_DUNG;
                }
                catch
                {
                    QICache.Invalidate(strSession);
                }
            }
            return detail;
        }
        public NGUOI_DUNG checkNguoiDungByUserPass(string user, string pass)
        {
            var QICache = new DefaultCacheProvider();
            string strSession = QICache.BuildCachedKey("NGUOI_DUNG", "checkNguoiDungByUserPass", user, pass);
            NGUOI_DUNG detail = new NGUOI_DUNG();
            if (!QICache.IsSet(strSession))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    detail = (from p in context.NGUOI_DUNG
                              where p.TEN_DANG_NHAP == user && p.MAT_KHAU == pass
                              select p).FirstOrDefault();
                    QICache.Set(strSession, detail, 300000);
                }
            }
            else
            {
                try
                {
                    detail = QICache.Get(strSession) as NGUOI_DUNG;
                }
                catch
                {
                    QICache.Invalidate(strSession);
                }
            }
            return detail;
        }
        #endregion
        #region Set
        public ResultEntity changeLanguage(long id, string ma_ngon_ngu)
        {
            NGUOI_DUNG detail = new NGUOI_DUNG();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.NGUOI_DUNG
                              where p.ID == id
                              select p).FirstOrDefault();

                    if (detail != null)
                    {
                        res.ResObject = detail;
                        detail.MA_NGON_NGU = ma_ngon_ngu;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("NGUOI_DUNG");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }

        public ResultEntity insert(NGUOI_DUNG detail_in, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail_in.ID = context.Database.SqlQuery<long>("SELECT NGUOI_DUNG_SEQ1.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                    detail_in.NGAY_TAO = DateTime.Now;
                    detail_in.NGUOI_TAO = nguoi;
                    detail_in = context.NGUOI_DUNG.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("NGUOI_DUNG");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "có lỗi xãy ra";
                //res.Msg = ex.ToString();
            }
            return res;
        }
        public ResultEntity update(NGUOI_DUNG detail_in, long? nguoi)
        {
            NGUOI_DUNG detail = new NGUOI_DUNG();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.NGUOI_DUNG
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.MAT_KHAU = detail_in.MAT_KHAU;
                        detail.TEN_HIEN_THI = detail_in.TEN_HIEN_THI;
                        detail.EMAIL = detail_in.EMAIL;
                        detail.SDT = detail_in.SDT;
                        detail.DIA_CHI = detail_in.DIA_CHI;
                        detail.ANH_DAI_DIEN = detail_in.ANH_DAI_DIEN;
                        detail.GHI_CHU = detail_in.GHI_CHU;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        detail.IS_DELETE = detail_in.IS_DELETE;
                        detail.MA_NGON_NGU = detail_in.MA_NGON_NGU;
                        detail.MA_NHOM_QUYEN = detail_in.MA_NHOM_QUYEN;
                        detail.FACE_BOOK = detail_in.FACE_BOOK;
                        detail.TRANG_THAI = detail_in.TRANG_THAI;
                        detail.IS_LOGIN_GMAIL = detail_in.IS_LOGIN_GMAIL;
                        detail.IS_LOGIN_FACEBOOK = detail_in.IS_LOGIN_FACEBOOK;
                        detail.ID_DOI_TAC = detail_in.ID_DOI_TAC;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("NGUOI_DUNG");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
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
                        var sql = @"update NGUOI_DUNG set IS_DELETE = 1, NGUOI_TAO=:0, NGAY_TAO=:1 where ID = :2";
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, ma);
                    }
                    else
                    {
                        var sql = @"delete from NGUOI_DUNG_MENU where ID_NGUOI_DUNG = :0";
                        context.Database.ExecuteSqlCommand(sql, ma);

                        sql = @"delete from NGUOI_DUNG_TRUONG where ID_NGUOI_DUNG = :0";
                        context.Database.ExecuteSqlCommand(sql, ma);

                        sql = @"delete from NGUOI_DUNG where ID = :0";
                        context.Database.ExecuteSqlCommand(sql, ma);
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("NGUOI_DUNG");
                QICache.RemoveByFirstName("NGUOI_DUNG_MENU");
                QICache.RemoveByFirstName("NGUOI_DUNG_TRUONG");
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
