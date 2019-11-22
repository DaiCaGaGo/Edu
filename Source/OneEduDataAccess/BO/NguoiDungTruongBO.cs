using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class NguoiDungTruongBO
    {
        TruongBO truongBO = new TruongBO();
        NguoiDungMenuBO nguoiDungMenuBO = new NguoiDungMenuBO();
        LogUserBO logUserBO = new LogUserBO();
        #region Get
        public NGUOI_DUNG_TRUONG getNguoiDungTruongByID(long? id)
        {
            NGUOI_DUNG_TRUONG data = new NGUOI_DUNG_TRUONG();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("NGUOI_DUNG_TRUONG", "getNguoiDungTruongByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.NGUOI_DUNG_TRUONG where p.ID == id && p.IS_DELETE != true && p.TRANG_THAI == 1 select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as NGUOI_DUNG_TRUONG;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public NGUOI_DUNG_TRUONG getNguoiDungTruongByIDNguoiDungAndTruong(long id_truong, long id_nguoi_dung)
        {
            NGUOI_DUNG_TRUONG data = new NGUOI_DUNG_TRUONG();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("NGUOI_DUNG_TRUONG", "getNguoiDungTruongByIDNguoiDungAndTruong", id_truong, id_nguoi_dung);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.NGUOI_DUNG_TRUONG where p.ID_NGUOI_DUNG == id_nguoi_dung && p.ID_TRUONG == id_truong && p.IS_DELETE != true && p.TRANG_THAI == 1 select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as NGUOI_DUNG_TRUONG;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<NGUOI_DUNG_TRUONG> getListTruongByNguoiDung(long id_nguoi_dung)
        {
            List<NGUOI_DUNG_TRUONG> data = new List<NGUOI_DUNG_TRUONG>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("NGUOI_DUNG_TRUONG", "getListTruongByNguoiDung", id_nguoi_dung);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.NGUOI_DUNG_TRUONG where p.ID_NGUOI_DUNG == id_nguoi_dung && p.IS_DELETE != true && p.TRANG_THAI == 1 select p).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<NGUOI_DUNG_TRUONG>;
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
        public ResultEntity insertOrUpdate(NGUOI_DUNG_TRUONG detail_in, long? nguoi, string maNhomQuyen)
        {
            NGUOI_DUNG_TRUONG detail = new NGUOI_DUNG_TRUONG();
            NguoiDungMenuBO nguoiDungMenuBO = new NguoiDungMenuBO();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.NGUOI_DUNG_TRUONG
                              where p.IS_DELETE != true && p.ID_NGUOI_DUNG == detail_in.ID_NGUOI_DUNG && p.ID_TRUONG == detail_in.ID_TRUONG && p.TRANG_THAI != 1
                              select p).FirstOrDefault();
                    if (detail == null)
                    {
                        detail_in.ID = context.Database.SqlQuery<long>("SELECT NGUOI_DUNG_TRUONG_SEQ.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                        detail_in.NGAY_TAO = DateTime.Now;
                        detail_in.NGUOI_TAO = nguoi;
                        detail_in.NGAY_SUA = DateTime.Now;
                        detail_in.NGUOI_SUA = nguoi;
                        detail_in = context.NGUOI_DUNG_TRUONG.Add(detail_in);

                        context.SaveChanges();
                        res.ResObject = detail_in;
                    }
                    else
                    {
                        detail.TRANG_THAI = detail_in.TRANG_THAI;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;

                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                    #region insert người dùng menu 
                    long id_truong = detail_in.ID_TRUONG;
                    TRUONG truong = truongBO.getTruongById(id_truong);
                    if (truong != null)
                    {
                        if (truong.IS_MN == true)
                            res = nguoiDungMenuBO.insertNguoiDungMenuTheoNhomQuyen(detail_in.ID_NGUOI_DUNG, detail_in.ID_TRUONG, "MN", maNhomQuyen, nguoi);
                        if (truong.IS_TH == true)
                            res = nguoiDungMenuBO.insertNguoiDungMenuTheoNhomQuyen(detail_in.ID_NGUOI_DUNG, detail_in.ID_TRUONG, "TH", maNhomQuyen, nguoi);
                        if (truong.IS_THCS == true)
                            res = nguoiDungMenuBO.insertNguoiDungMenuTheoNhomQuyen(detail_in.ID_NGUOI_DUNG, detail_in.ID_TRUONG, "THCS", maNhomQuyen, nguoi);
                        if (truong.IS_THPT == true)
                            res = nguoiDungMenuBO.insertNguoiDungMenuTheoNhomQuyen(detail_in.ID_NGUOI_DUNG, detail_in.ID_TRUONG, "THPT", maNhomQuyen, nguoi);
                        logUserBO.insert(null, "UPDATE", "Cập nhật menu người dùng: nhóm quyền " + maNhomQuyen + " với trường " + id_truong, nguoi, DateTime.Now);
                    }
                    #endregion
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("NGUOI_DUNG_TRUONG");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(NGUOI_DUNG_TRUONG detail_in, long? nguoi, string ma_nhom_quyen)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail_in.ID = context.Database.SqlQuery<long>("SELECT NGUOI_DUNG_TRUONG_SEQ.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                    detail_in.NGAY_TAO = DateTime.Now;
                    detail_in.NGUOI_TAO = nguoi;
                    detail_in = context.NGUOI_DUNG_TRUONG.Add(detail_in);
                    context.SaveChanges();
                    res.ResObject = detail_in;
                    #region "insert menu menu người dùng trường dựa vào mã nhóm quyền"
                    long id_truong = detail_in.ID_TRUONG;
                    TRUONG truong = truongBO.getTruongById(id_truong);
                    if (truong != null)
                    {
                        if (truong.IS_MN == true)
                            res = nguoiDungMenuBO.insertNguoiDungMenuTheoNhomQuyen(detail_in.ID_NGUOI_DUNG, detail_in.ID_NGUOI_DUNG, "MN", ma_nhom_quyen, nguoi);
                        if (truong.IS_TH == true)
                            res = nguoiDungMenuBO.insertNguoiDungMenuTheoNhomQuyen(detail_in.ID_NGUOI_DUNG, detail_in.ID_TRUONG, "TH", ma_nhom_quyen, nguoi);
                        if (truong.IS_THCS == true)
                            res = nguoiDungMenuBO.insertNguoiDungMenuTheoNhomQuyen(detail_in.ID_NGUOI_DUNG, detail_in.ID_TRUONG, "THCS", ma_nhom_quyen, nguoi);
                        if (truong.IS_THPT == true)
                            res = nguoiDungMenuBO.insertNguoiDungMenuTheoNhomQuyen(detail_in.ID_NGUOI_DUNG, detail_in.ID_TRUONG, "THPT", ma_nhom_quyen, nguoi);
                        logUserBO.insert(null, "UPDATE", "Cập nhật menu người dùng: nhóm quyền " + ma_nhom_quyen + " với trường " + id_truong, nguoi, DateTime.Now);
                    }
                    #endregion
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("NGUOI_DUNG_TRUONG");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "có lỗi xãy ra";
                //res.Msg = ex.ToString();
            }
            return res;
        }
        #region delete nguoi dung truong
        public ResultEntity delete(long? nguoi, long idNguoiDung, long idTruong, bool is_delete = false)
        {
            NGUOI_DUNG_TRUONG detail = new NGUOI_DUNG_TRUONG();
            NguoiDungMenuBO nguoiDungMenuBO = new NguoiDungMenuBO();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.NGUOI_DUNG_TRUONG
                              where p.IS_DELETE != true && p.ID_NGUOI_DUNG == idNguoiDung && p.ID_TRUONG == idTruong && p.TRANG_THAI == 1
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        if (!is_delete)
                        {
                            var sql = @"update NGUOI_DUNG_TRUONG set TRANG_THAI = 0, NGUOI_SUA=:0, NGAY_SUA=:1 where ID = :2";
                            context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, detail.ID);
                        }
                        else
                        {
                            var sql = @"delete from NGUOI_DUNG_TRUONG where ID = :0";
                            context.Database.ExecuteSqlCommand(sql, detail.ID);
                        }
                        // delete nhom quyen theo nguoi dung truong
                        res = nguoiDungMenuBO.delete(nguoi, idNguoiDung, idTruong);
                    }
                }
                var QICache = new DefaultCacheProvider();
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
        #endregion
    }
}
