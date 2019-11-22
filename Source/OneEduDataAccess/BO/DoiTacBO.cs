using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class DoiTacBO
    {
        #region get
        public List<DOI_TAC> getDoiTac(bool is_all = false, short id_all = 0, string text_all = "Chọn tất cả")
        {
            List<DOI_TAC> data = new List<DOI_TAC>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DOI_TAC", "getDoiTac", is_all, id_all, text_all);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DOI_TAC where p.IS_DELETE != true select p).ToList();
                    if (is_all)
                    {
                        if (data == null) data = new List<DOI_TAC>();
                        DOI_TAC item_all = new DOI_TAC();
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
                    data = QICache.Get(strKeyCache) as List<DOI_TAC>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public DOI_TAC getDoiTacByID(short id)
        {
            DOI_TAC data = new DOI_TAC();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("DOI_TAC", "getDoiTacByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.DOI_TAC where p.ID == id select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as DOI_TAC;
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
        public ResultEntity update(short id, string ten, string dia_chi, string sdt, long? nguoi, bool is_delete)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    var sql = @"Update DOI_TAC SET TEN=:0, DIA_CHI=:1, SDT=:2, NGAY_SUA = :3, NGUOI_SUA = :4, IS_DELETE = :5 WHERE ID = :6";
                    context.Database.ExecuteSqlCommand(sql, ten, dia_chi, sdt, DateTime.Now, nguoi, is_delete ? 1 : 0, id);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DOI_TAC");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity updateTongTinCap(DOI_TAC detail_in, long? nguoi)
        {
            DOI_TAC detail = new DOI_TAC();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.DOI_TAC where p.ID == detail_in.ID select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.TONG_TIN_CAP = detail_in.TONG_TIN_CAP;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DOI_TAC");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(string ten, string dia_chi, string sdt, long? nguoi, bool is_delete)
        {
            DOI_TAC detail = new DOI_TAC();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail.TEN = ten;
                    detail.DIA_CHI = dia_chi;
                    detail.SDT = sdt;
                    detail.NGAY_TAO = DateTime.Now;
                    detail.NGUOI_TAO = nguoi;
                    detail.IS_DELETE = is_delete;
                    detail = context.DOI_TAC.Add(detail);
                    context.SaveChanges();
                }
                res.ResObject = detail;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DOI_TAC");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity delete(int id,long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    var sql = @"update DOI_TAC set IS_DELETE = 1,NGUOI_SUA=:0,NGAY_SUA=:1 where ID = :2";
                    context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, id);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("DOI_TAC");
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
