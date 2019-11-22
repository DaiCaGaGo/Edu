using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class NguoiDungMenuBO
    {


        public ResultEntity SetRoleToManySchools(long idMENU, long id_truong, long id_NguoiDung_req, long? idNguoiDung)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            var QICache = new DefaultCacheProvider();
            try
            {
                using (oneduEntities context = new oneduEntities())
                {
                    #region Query
                    string strQuery1 = @"delete nguoi_dung_menu where ID_MENU= :0 and ID_TRUONG != :1 and 
                        exists (select * from NGUOI_DUNG_TRUONG where ID_NGUOI_DUNG = :2 
                        and NGUOI_DUNG_MENU.ID_NGUOI_DUNG=NGUOI_DUNG_TRUONG.ID_NGUOI_DUNG)";
                    context.Database.ExecuteSqlCommand(strQuery1, idMENU, id_truong, id_NguoiDung_req);
                    string strQuery2 = string.Format(@"insert into NGUOI_DUNG_MENU(ID_NGUOI_DUNG,ID_TRUONG,ID_MENU,TRANG_THAI,IS_XEM,IS_THEM,IS_SUA,IS_XOA,IS_DELETE,IS_SEND_SMS,IS_VIEW_INFOR,IS_EXPORT, NGAY_TAO, NGUOI_TAO) 
                                        select nm.ID_NGUOI_DUNG,nt.ID_TRUONG,nm.ID_MENU,nm.TRANG_THAI,nm.IS_XEM,nm.IS_THEM,IS_SUA,nm.IS_XOA,nm.IS_DELETE,nm.IS_SEND_SMS,nm.IS_VIEW_INFOR,nm.IS_EXPORT, :0 as NGAY_TAO,:1 as NGUOI_TAO
                                        FROM NGUOI_DUNG_MENU nm
                                        join NGUOI_DUNG_TRUONG nt on nm.ID_NGUOI_DUNG=nt.ID_NGUOI_DUNG and nt.ID_TRUONG !={0}
                                        where nm.ID_MENU = {1}  and nm.ID_NGUOI_DUNG = {2} and nm.ID_TRUONG={0}", id_truong, idMENU, id_NguoiDung_req);
                    #endregion
                    context.Database.ExecuteSqlCommand(strQuery2, DateTime.Now, idNguoiDung);
                    QICache.RemoveByFirstName("NGUOI_DUNG_MENU");
                    QICache.RemoveByFirstName("NGUOI_DUNG_TRUONG");
                }
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public List<NGUOI_DUNG_MENU> getByNguoiDungTruong(long id_nguoi, long id_truong)
        {
            List<NGUOI_DUNG_MENU> data = new List<NGUOI_DUNG_MENU>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("NGUOI_DUNG_MENU", "getByNguoiDungTruong", id_nguoi, id_truong);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.NGUOI_DUNG_MENU where p.IS_DELETE != true && p.ID_NGUOI_DUNG == id_nguoi && p.ID_TRUONG == id_truong && p.TRANG_THAI == 1 select p).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<NGUOI_DUNG_MENU>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public NGUOI_DUNG_MENU getByNguoiDungTruongMenu(long id_nguoi, long id_truong, long id_menu)
        {
            NGUOI_DUNG_MENU data = new NGUOI_DUNG_MENU();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("NGUOI_DUNG_MENU", "getByNguoiDungTruongMenu", id_nguoi, id_truong, id_menu);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.NGUOI_DUNG_MENU where p.IS_DELETE != true && p.ID_NGUOI_DUNG == id_nguoi && p.ID_TRUONG == id_truong && p.ID_MENU == id_menu && p.TRANG_THAI == 1 select p).FirstOrDefault();

                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as NGUOI_DUNG_MENU;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<NguoiDungMenuEntity> getQuyenByNguoiDungTruongCapHoc(long id_nguoi, long id_truong, string maCapHoc)
        {
            List<NguoiDungMenuEntity> data = new List<NguoiDungMenuEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MENU", "NGUOI_DUNG_MENU", "getQuyenByNguoiDungTruongCapHoc", id_nguoi, id_truong, maCapHoc);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = @"select m.ID,
                                        m.ID_CHA,
                                        m.TEN,
                                        ndm.ID AS ID_NGUOI_DUNG_MENU,
                                        ndm.ID_MENU,
                                        ndm.IS_XEM,
                                        ndm.IS_THEM,
                                        ndm.IS_SUA,
                                        ndm.IS_XOA,
                                        ndm.IS_SEND_SMS,
                                        ndm.IS_VIEW_INFOR,
                                        ndm.IS_EXPORT
                                        from MENU m
                                        left join nguoi_dung_menu ndm on m.ID=ndm.ID_MENU and ndm.id_truong = :0 
                                        and ndm.id_nguoi_dung = :1 
                                        --and (ndm.TRANG_THAI is not null and ndm.TRANG_THAI=1)
                                        where m.ma_cap_hoc = :2 
                                        order by m.THU_TU";

                    data = context.Database.SqlQuery<NguoiDungMenuEntity>(strQuery, id_truong, id_nguoi, maCapHoc).ToList();

                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<NguoiDungMenuEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<NguoiDungMenuEntity> getNguoiDungByTruong(long id_truong)
        {
            List<NguoiDungMenuEntity> data = new List<NguoiDungMenuEntity>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("NGUOI_DUNG_MENU", "NGUOI_DUNG", "getNguoiDungByTruong", id_truong);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = @"select m.ID_NGUOI_DUNG AS ID,
                                                nd.TEN_DANG_NHAP AS TEN
                                        from NGUOI_DUNG_MENU m
                                        inner join NGUOI_DUNG nd on m.ID_NGUOI_DUNG=nd.ID
                                        where m.IS_DELETE = 0 and m.ID_TRUONG = :0 and m.TRANG_THAI = 1
                                        group by m.ID_NGUOI_DUNG, nd.TEN_DANG_NHAP";

                    data = context.Database.SqlQuery<NguoiDungMenuEntity>(strQuery, id_truong).ToList();

                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<NguoiDungMenuEntity>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        #region Set
        public ResultEntity insert(NGUOI_DUNG_MENU detail_in, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail_in.ID = context.Database.SqlQuery<long>("SELECT NGUOI_DUNG_MENU_SEQ.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                    detail_in.TRANG_THAI = (detail_in.IS_XEM == null || detail_in.IS_XEM.Value == false) ? Convert.ToInt16(0) : Convert.ToInt16(1);
                    detail_in.NGAY_TAO = DateTime.Now;
                    detail_in.NGUOI_TAO = nguoi;
                    detail_in.NGAY_SUA = DateTime.Now;
                    detail_in.NGUOI_SUA = nguoi;
                    detail_in = context.NGUOI_DUNG_MENU.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("NGUOI_DUNG_MENU");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insertNhomQuyenMenuToNguoiDungMenu(NGUOI_DUNG_TRUONG detail_in, long? nguoi, string maNhomQuyen)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    string sql = @"update NGUOI_DUNG_MENU set TRANG_THAI = 1, IS_DELETE = 0
                                    where ID_NGUOI_DUNG = :0 and exists (select ID from NHOM_QUYEN_MENU where NHOM_QUYEN_MENU.MA_NHOM_QUYEN = :1 and NGUOI_DUNG_MENU.ID_MENU = NHOM_QUYEN_MENU.ID_MENU)";
                    context.Database.ExecuteSqlCommand(sql, detail_in.ID_NGUOI_DUNG, maNhomQuyen);
                    sql = string.Empty;
                    sql = @"insert into NGUOI_DUNG_MENU (ID_NGUOI_DUNG,ID_TRUONG,ID_MENU,TRANG_THAI,IS_THEM,
                            IS_XEM,IS_SUA,IS_XOA,IS_SEND_SMS,IS_VIEW_INFOR,IS_EXPORT,NGAY_TAO,NGUOI_TAO)
                            (select :0,:1,ID_MENU,TRANG_THAI,IS_THEM,IS_XEM,IS_SUA,IS_XOA,IS_SEND_SMS,
                            IS_VIEW_INFOR,IS_EXPORT,:2,:3 from NHOM_QUYEN_MENU nm
                            join menu m on nm.id_menu=m.id and m.MA_CAP_HOC=:4
                            where MA_NHOM_QUYEN = :5 
                            and not exists (select ID from NGUOI_DUNG_MENU where ID_NGUOI_DUNG = :6 AND ID_TRUONG = :7))";
                    res.ResObject = context.Database.ExecuteSqlCommand(sql, detail_in.ID_NGUOI_DUNG, detail_in.ID_TRUONG, DateTime.Now, nguoi, maNhomQuyen, detail_in.ID_NGUOI_DUNG, detail_in.ID_TRUONG);
                }

                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("NGUOI_DUNG_MENU");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity update(NGUOI_DUNG_MENU detail_in, long? nguoi)
        {
            NGUOI_DUNG_MENU detail = new NGUOI_DUNG_MENU();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.NGUOI_DUNG_MENU
                              where p.ID == detail_in.ID && p.IS_DELETE != true
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.IS_SUA = detail_in.IS_SUA;
                        detail.IS_THEM = detail_in.IS_THEM;
                        detail.IS_XEM = detail_in.IS_XEM;
                        detail.IS_XOA = detail_in.IS_XOA;
                        detail.IS_SEND_SMS = detail_in.IS_SEND_SMS;
                        detail.IS_VIEW_INFOR = detail_in.IS_VIEW_INFOR;
                        detail.IS_EXPORT = detail_in.IS_EXPORT;
                        detail.TRANG_THAI = (detail.IS_XEM == null || detail.IS_XEM.Value == false) ? Convert.ToInt16(0) : Convert.ToInt16(1);
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("NGUOI_DUNG_MENU");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity delete(long? nguoi, long idNguoiDung, long idTruong, bool is_delete = false)
        {
            NGUOI_DUNG_MENU detail = new NGUOI_DUNG_MENU();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.NGUOI_DUNG_MENU
                              where p.IS_DELETE != true && p.ID_NGUOI_DUNG == idNguoiDung && p.ID_TRUONG == idTruong && p.TRANG_THAI == 1
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        if (!is_delete)
                        {
                            var sql = @"update NGUOI_DUNG_MENU set TRANG_THAI = 0, NGUOI_SUA=:0, NGAY_SUA=:1 where ID_NGUOI_DUNG = :2 and ID_TRUONG = :3";
                            context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, idNguoiDung, idTruong);
                        }
                        else
                        {
                            var sql = @"delete from NGUOI_DUNG_MENU where ID = :0";
                            context.Database.ExecuteSqlCommand(sql, detail.ID);
                        }
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("NGUOI_DUNG_MENU");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }

            return res;
        }
        public ResultEntity updateNguoiDungMenuKhiUpdateNhomQuyen(long id_nguoi_dung, long id_truong, string ma_cap_hoc, string ma_nhom_quyen_old, string ma_nhom_quyen_new, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    string sql1 = @"update NGUOI_DUNG_MENU set TRANG_THAI=0, IS_DELETE=0, NGUOI_SUA=:0, NGAY_SUA=:1
                                    where ID_NGUOI_DUNG=:2 and ID_TRUONG=:3 and exists (select ID from NHOM_QUYEN_MENU where NHOM_QUYEN_MENU.MA_NHOM_QUYEN =:4 and NGUOI_DUNG_MENU.ID_MENU = NHOM_QUYEN_MENU.ID_MENU and NHOM_QUYEN_MENU.TRANG_THAI=1)";
                    context.Database.ExecuteSqlCommand(sql1, nguoi, DateTime.Now, id_nguoi_dung, id_truong, ma_nhom_quyen_old);
                    string sql = @"update NGUOI_DUNG_MENU set TRANG_THAI=1, IS_DELETE=0, NGUOI_SUA=:0, NGAY_SUA=:1
                                    where ID_NGUOI_DUNG=:2 and ID_TRUONG=:3 and exists (select ID from NHOM_QUYEN_MENU where NHOM_QUYEN_MENU.MA_NHOM_QUYEN=:4 and NGUOI_DUNG_MENU.ID_MENU = NHOM_QUYEN_MENU.ID_MENU and NHOM_QUYEN_MENU.TRANG_THAI=1)";
                    context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, id_nguoi_dung, id_truong, ma_nhom_quyen_new);
                    sql = string.Empty;
                    sql = @"insert into NGUOI_DUNG_MENU (ID_NGUOI_DUNG, ID_TRUONG, ID_MENU, TRANG_THAI, IS_THEM,
                            IS_XEM, IS_SUA, IS_XOA, IS_SEND_SMS, IS_VIEW_INFOR, IS_EXPORT, NGAY_TAO, NGUOI_TAO)
                            (select :0, :1, ID_MENU, nm.TRANG_THAI, IS_THEM, IS_XEM, IS_SUA, IS_XOA, IS_SEND_SMS,
                            IS_VIEW_INFOR, IS_EXPORT, :2, :3 from NHOM_QUYEN_MENU nm
                            join menu m on nm.id_menu=m.id and m.MA_CAP_HOC=:4
                            where MA_NHOM_QUYEN=:5 
                            and not exists (select ID from NGUOI_DUNG_MENU where ID_NGUOI_DUNG=:6 AND ID_TRUONG=:7))";
                    res.ResObject = context.Database.ExecuteSqlCommand(sql, id_nguoi_dung, id_truong, DateTime.Now, nguoi, ma_cap_hoc, ma_nhom_quyen_new, id_nguoi_dung, id_truong);
                }

                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("NGUOI_DUNG_MENU");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insertNguoiDungMenuTheoNhomQuyen(long id_nguoi_dung, long id_truong, string ma_cap_hoc, string ma_nhom_quyen, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    string sql = @"update NGUOI_DUNG_MENU set TRANG_THAI=1, IS_DELETE=0, NGUOI_SUA=:0, NGAY_SUA=:1
                                    where ID_NGUOI_DUNG=:2 and ID_TRUONG=:3 and exists (select ID from NHOM_QUYEN_MENU where NHOM_QUYEN_MENU.MA_NHOM_QUYEN=:4 and NGUOI_DUNG_MENU.ID_MENU = NHOM_QUYEN_MENU.ID_MENU)";
                    context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, id_nguoi_dung, id_truong, ma_nhom_quyen);
                    sql = string.Empty;
                    sql = @"insert into NGUOI_DUNG_MENU (ID_NGUOI_DUNG, ID_TRUONG, ID_MENU, TRANG_THAI, IS_THEM,
                            IS_XEM, IS_SUA, IS_XOA, IS_SEND_SMS, IS_VIEW_INFOR, IS_EXPORT, NGAY_TAO, NGUOI_TAO)
                            (select :0, :1, ID_MENU, nm.TRANG_THAI, IS_THEM, IS_XEM, IS_SUA, IS_XOA, IS_SEND_SMS,
                            IS_VIEW_INFOR, IS_EXPORT, :2, :3 from NHOM_QUYEN_MENU nm
                            join menu m on nm.id_menu=m.id and m.MA_CAP_HOC=:4
                            where MA_NHOM_QUYEN=:5 
                            and not exists (select ID from NGUOI_DUNG_MENU where ID_NGUOI_DUNG=:6 AND ID_TRUONG=:7))";
                    res.ResObject = context.Database.ExecuteSqlCommand(sql, id_nguoi_dung, id_truong, DateTime.Now, nguoi, ma_cap_hoc, ma_nhom_quyen, id_nguoi_dung, id_truong);
                }

                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("NGUOI_DUNG_MENU");
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
