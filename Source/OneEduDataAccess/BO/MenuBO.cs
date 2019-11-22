using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class MenuBO
    {
        #region get
        #region get tat ca cac MENU
        public List<MENU> getMenu(string ma_cap_hoc, bool is_all = false, short id_all = 0, string text_all = "Chọn tất cả", bool is_sys = false)
        {
            List<MENU> data = new List<MENU>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MENU", "getMenu", ma_cap_hoc, is_all, id_all, text_all, is_sys);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.MENUs
                               where p.IS_DELETE != true && p.IS_SYS == is_sys
                               select p);
                    if (is_sys==false)
                        tmp = tmp.Where(x => x.MA_CAP_HOC == ma_cap_hoc);
                    data = tmp.OrderBy(x => x.THU_TU).ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<MENU>();
                        MENU item_all = new MENU();
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
                    data = QICache.Get(strKeyCache) as List<MENU>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        #endregion
        #region get MENU theo id
        public MENU getMENUById(long id)
        {
            MENU data = new MENU();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MENU", "getMENUById", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.MENUs where p.ID == id && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as MENU;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        #endregion
        public List<MENU> getMenuByNguoiDung(string ma_cap_hoc, long nguoi_dung, bool? is_xem, bool is_sys = false)
        {
            List<MENU> data = new List<MENU>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MENU", "NGUOI_DUNG_MENU", "getMenuByNguoiDung", ma_cap_hoc, nguoi_dung, is_xem, is_sys);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    if (is_xem != null)
                    {
                        var tmp = (from p in context.MENUs
                                   join nm in context.NGUOI_DUNG_MENU on p.ID equals nm.ID_MENU
                                   where p.IS_DELETE != true && nm.ID_NGUOI_DUNG == nguoi_dung && nm.IS_XEM == true && p.IS_SYS == is_sys
                                   orderby p.THU_TU
                                   select p);
                        if (is_sys == false)
                            tmp = tmp.Where(x => x.MA_CAP_HOC == ma_cap_hoc);
                        data = tmp.OrderBy(x => x.THU_TU).ToList();
                    }
                    else
                    {
                        var tmp = (from p in context.MENUs
                                   join nm in context.NGUOI_DUNG_MENU on p.ID equals nm.ID_MENU
                                   where p.IS_DELETE != true && nm.ID_NGUOI_DUNG == nguoi_dung && p.IS_SYS == is_sys
                                   orderby p.THU_TU
                                   select p);
                        if (is_sys == false)
                            tmp= tmp.Where(x => x.MA_CAP_HOC == ma_cap_hoc);
                        data = tmp.OrderBy(x => x.THU_TU).ToList();
                    }
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<MENU>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public MENU getMENUByUrl(string url, string ma_cap_hoc)
        {
            MENU data = new MENU();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("MENU", "getMENUByUrl", url, ma_cap_hoc);
            if (!QICache.IsSet(strKeyCache))
            {
                List<MENU> listMenu = new List<MENU>();
                listMenu = getMenu(ma_cap_hoc);
                data = listMenu.Where(c => c.URL != null && c.URL.ToUpper().Trim().Replace("~/", "") == url.ToUpper().Trim().Replace("~/", "")).FirstOrDefault();
                QICache.Set(strKeyCache, data, 300000);

            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as MENU;
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
        public ResultEntity update(MENU detail_in, long? nguoi)
        {
            MENU detail = new MENU();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.MENUs
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();

                    if (detail != null)
                    {
                        detail.TEN = detail_in.TEN;
                        detail.TEN_EG = detail_in.TEN_EG;
                        detail.ICON = detail_in.ICON;
                        detail.URL = detail_in.URL;
                        detail.ICON_CSS_CLASS = detail_in.ICON_CSS_CLASS;
                        detail.TRANG_THAI = detail_in.TRANG_THAI;
                        detail.IS_HIEN_THI = detail_in.IS_HIEN_THI;
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
            QICache.RemoveByFirstName("MENU");
            return res;
        }
        #endregion
        #region insert
        public ResultEntity insert(MENU detail_in, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail_in.NGUOI_TAO = nguoi;
                    if (detail_in.IS_SYS == null) detail_in.IS_SYS = false;
                    detail_in.NGAY_TAO = DateTime.Now;
                    detail_in = context.MENUs.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            var QICache = new DefaultCacheProvider();
            QICache.RemoveByFirstName("MENU");
            return res;
        }
        #endregion
        #region delete
        public ResultEntity delete(List<string> arrId, long? nguoi, bool? is_delete = false)
        {
            MENU detail = new MENU();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            string sql = string.Empty;
            try
            {
                if (arrId != null)
                {
                    using (var context = new oneduEntities())
                    {
                        for (int i = 0; i < arrId.Count; i++)
                        {
                            if (is_delete != null && is_delete.Value)
                                sql += @"DELETE FROM MENU WHERE ID = " + arrId[i].ToString() + " ";
                            else
                                sql += @"UPDATE MENU SET IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 WHERE ID = " + arrId[i].ToString() + " ";
                        }
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            var QICache = new DefaultCacheProvider();
            QICache.RemoveByFirstName("MENU");
            return res;
        }
        public ResultEntity delete(long id, long? nguoi, bool? is_delete = false)
        {
            MENU detail = new MENU();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            string sql = string.Empty;
            try
            {
                using (var context = new oneduEntities())
                {

                    if (is_delete != null && is_delete.Value)
                    {
                        sql += @"DELETE MENU WHERE ID = " + id.ToString();
                        int resKQ = context.Database.ExecuteSqlCommand(sql);
                    }
                    else
                    {
                        sql += @"UPDATE MENU SET IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 WHERE ID = " + id.ToString();
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
            QICache.RemoveByFirstName("MENU");
            return res;
        }
        #endregion
        #endregion
    }
}
