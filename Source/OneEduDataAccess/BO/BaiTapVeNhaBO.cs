using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class BaiTapVeNhaBO
    {
        #region get
        public List<BAI_TAP_VE_NHA> getBaiTapVeNhaByFilter(long id_truong, string ma_cap_hoc, short id_khoi, short id_nam_hoc, long? id_lop, string tu_ngay, string den_ngay)
        {
            List<BAI_TAP_VE_NHA> data = new List<BAI_TAP_VE_NHA>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("BAI_TAP_VE_NHA", "getBaiTapVeNhaByFilter", id_truong, ma_cap_hoc, id_khoi, id_nam_hoc, id_lop, tu_ngay, den_ngay);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = string.Format(@"select * from BAI_TAP_VE_NHA where id_truong=:0 and ma_cap_hoc=:1 and id_khoi=:2 and id_nam_hoc=:3 and not (is_delete is not null and is_delete=1)");
                    if (id_lop != null)
                        strQuery += " and id_lop=" + id_lop;
                    if (!string.IsNullOrEmpty(tu_ngay) && !string.IsNullOrEmpty(den_ngay))
                        strQuery += string.Format(@" and to_char(NGAY_BTVN, 'YYYYMMDD')>='{0}' and to_char(NGAY_BTVN, 'YYYYMMDD')<='{1}'", tu_ngay, den_ngay);
                    data = context.Database.SqlQuery<BAI_TAP_VE_NHA>(strQuery, id_truong, ma_cap_hoc, id_khoi, id_nam_hoc).ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<BAI_TAP_VE_NHA>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public BAI_TAP_VE_NHA getBaiTapVeNhaByID(long id)
        {
            BAI_TAP_VE_NHA data = new BAI_TAP_VE_NHA();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("BAI_TAP_VE_NHA", "getBaiTapVeNhaByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.BAI_TAP_VE_NHA where p.ID == id select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as BAI_TAP_VE_NHA;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public BAI_TAP_VE_NHA getBaiTapVeNhaByNgay(long id_truong, short id_nam_hoc, long id_lop, string ngay_bai_tap)
        {
            BAI_TAP_VE_NHA data = new BAI_TAP_VE_NHA();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("BAI_TAP_VE_NHA", "getBaiTapVeNhaByNgay", id_truong, id_nam_hoc, id_lop, ngay_bai_tap);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = string.Format(@"select * from BAI_TAP_VE_NHA where id_truong={0} and id_nam_hoc={1}
                        and id_lop ={2} and to_char(ngay_btvn, 'YYYYMMDD') = {3} and not (is_delete is not null and is_delete=1)", id_truong, id_nam_hoc, id_lop, ngay_bai_tap);
                    data = context.Database.SqlQuery<BAI_TAP_VE_NHA>(strQuery).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as BAI_TAP_VE_NHA;
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
        public ResultEntity update(BAI_TAP_VE_NHA detail_in, long? nguoi)
        {
            BAI_TAP_VE_NHA detail = new BAI_TAP_VE_NHA();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.BAI_TAP_VE_NHA
                              where p.ID == detail_in.ID && p.IS_DELETE != true
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.ID_TRUONG = detail_in.ID_TRUONG;
                        detail.MA_CAP_HOC = detail_in.MA_CAP_HOC;
                        detail.ID_KHOI = detail_in.ID_KHOI;
                        detail.ID_NAM_HOC = detail_in.ID_NAM_HOC;
                        detail.ID_LOP = detail_in.ID_LOP;
                        detail.NGAY_BTVN = detail_in.NGAY_BTVN;
                        detail.NOI_DUNG = detail_in.NOI_DUNG;
                        detail.NGAY_TAO = DateTime.Now;
                        detail.NGUOI_TAO = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("BAI_TAP_VE_NHA");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(BAI_TAP_VE_NHA detail_in, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail_in.ID = context.Database.SqlQuery<long>("SELECT BAI_TAP_VE_NHA_SEQ.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                    detail_in.NGAY_TAO = DateTime.Now;
                    detail_in.NGUOI_TAO = nguoi;
                    detail_in = context.BAI_TAP_VE_NHA.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("BAI_TAP_VE_NHA");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity delete(long id)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    var sql = @"delete BAI_TAP_VE_NHA_CHI_TIET where ID_BTVN = :0";
                    context.Database.ExecuteSqlCommand(sql, id);
                    sql = @"delete BAI_TAP_VE_NHA where ID = :0";
                    context.Database.ExecuteSqlCommand(sql, id);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("BAI_TAP_VE_NHA_CHI_TIET");
                QICache.RemoveByFirstName("BAI_TAP_VE_NHA");
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
