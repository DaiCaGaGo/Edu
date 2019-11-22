using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class SuatAnBO
    {
        #region GET
        public List<SUAT_AN> getSuatAn(long id_truong, short? id_khoi, short id_nam_hoc, short? id_nhom_tuoi_mn, long? id_bua_an, long? id_thuc_don, DateTime ngay_an)
        {
            List<SUAT_AN> data = new List<SUAT_AN>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("SUAT_AN", "getSuaAn", id_truong, id_khoi, id_nam_hoc, id_nhom_tuoi_mn, id_bua_an, id_thuc_don, ngay_an);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var temp = (from p in context.SUAT_AN where p.ID_TRUONG == id_truong && p.ID_NAM_HOC == id_nam_hoc && p.IS_DELETE != true select p);
                    temp = temp.Where(x => DateTime.Compare(x.NGAY_AN.Value, ngay_an) == 0);
                    if (id_khoi != null)
                        temp = temp.Where(x => x.ID_KHOI == id_khoi);
                    if (id_nhom_tuoi_mn != null)
                        temp = temp.Where(x => x.ID_NHOM_TUOI_MN == id_nhom_tuoi_mn);
                    if (id_bua_an != null)
                        temp = temp.Where(x => x.ID_BUA_AN == id_bua_an);
                    if (id_thuc_don != null)
                        temp = temp.Where(x => x.ID_THUC_DON == id_thuc_don);
                    data = temp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<SUAT_AN>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public SUAT_AN getSuatAnByID(long id)
        {
            SUAT_AN data = new SUAT_AN();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("SUAT_AN", "getSuatAnByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.SUAT_AN where p.ID == id select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as SUAT_AN;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public SUAT_AN checkExists(long id_truong, short id_khoi, long id_bua_an, DateTime ngay_an)
        {
            SUAT_AN data = new SUAT_AN();
            var QICache = new DefaultCacheProvider();
            string time = ngay_an.ToString("yyyyMMdd");
            string strKeyCache = QICache.BuildCachedKey("SUAT_AN", "checkExists", id_truong, id_khoi, id_bua_an, time);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    //var tmp = (from p in context.SUAT_AN
                    //        where p.ID_TRUONG == id_truong && p.ID_KHOI == id_khoi && p.ID_BUA_AN == id_bua_an && p.IS_DELETE != true select p);
                    //tmp = tmp.Where(x => DateTime.Compare(x.NGAY_AN.Value, ngay_an) == 0);
                    //data = tmp.FirstOrDefault();

                    string strQuery = string.Format(@"select * from SUAT_AN where id_truong = {0} and id_khoi = {1} and ID_BUA_AN ={2} and to_char(ngay_an, 'YYYYMMDD') = {3} and (is_delete is null or is_delete = 0)", id_truong, id_khoi, id_bua_an, time);
                    data = context.Database.SqlQuery<SUAT_AN>(strQuery).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as SUAT_AN;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        #endregion
        #region SET
        public ResultEntity update(SUAT_AN detail_in, long? nguoi)
        {
            SUAT_AN detail = new SUAT_AN();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.SUAT_AN
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();

                    if (detail != null)
                    {
                        detail.ID_KHOI = detail_in.ID_KHOI;
                        detail.ID_NAM_HOC = detail_in.ID_NAM_HOC;
                        detail.ID_NHOM_TUOI_MN = detail_in.ID_NHOM_TUOI_MN;
                        detail.ID_BUA_AN = detail_in.ID_BUA_AN;
                        detail.ID_THUC_DON = detail_in.ID_THUC_DON;
                        detail.SO_HS_DANG_KY = detail_in.SO_HS_DANG_KY;
                        detail.TONG_NANG_LUONG_KCAL = detail_in.TONG_NANG_LUONG_KCAL;
                        detail.TONG_PROTID = detail_in.TONG_PROTID;
                        detail.TONG_GLUCID = detail_in.TONG_GLUCID;
                        detail.TONG_LIPID = detail_in.TONG_LIPID;
                        detail.NGAY_AN = detail_in.NGAY_AN;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
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
            QICache.RemoveByFirstName("SUAT_AN");
            return res;
        }
        public ResultEntity insert(SUAT_AN detail_in, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail_in.ID = context.Database.SqlQuery<long>("SELECT SUAT_AN_SEQ.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                    detail_in.NGAY_TAO = DateTime.Now;
                    detail_in.NGUOI_TAO = nguoi;
                    detail_in = context.SUAT_AN.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("SUAT_AN");
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
            SUAT_AN detail = new SUAT_AN();
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
                        sql = @"DELETE SUAT_AN_CHI_TIET WHERE ID_SUAT_AN=" + id.ToString();
                        context.Database.ExecuteSqlCommand(sql);
                        sql = @"DELETE SUAT_AN WHERE ID = " + id.ToString();
                        int resKQ = context.Database.ExecuteSqlCommand(sql);
                    }
                    else
                    {
                        sql = @"UPDATE SUAT_AN SET IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 WHERE ID=" + id.ToString();
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
            QICache.RemoveByFirstName("SUAT_AN_CHI_TIET");
            QICache.RemoveByFirstName("SUAT_AN");
            return res;
        }
        #endregion
    }
}
