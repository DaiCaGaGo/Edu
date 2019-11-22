using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class CapTinDoiTacBO
    {
        #region get
        public List<CAP_TIN_DOI_TAC> getDoiTacDuocCapTin(short? id_doi_tac)
        {
            List<CAP_TIN_DOI_TAC> data = new List<CAP_TIN_DOI_TAC>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CAP_TIN_DOI_TAC", "getDoiTacDuocCapTin", id_doi_tac);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.CAP_TIN_DOI_TAC where p.IS_DELETE != true select p);
                    if (id_doi_tac != null)
                        tmp = tmp.Where(x => x.ID_DOI_TAC == id_doi_tac);
                    tmp = tmp.OrderByDescending(x => x.NGAY_CAP);
                    data = tmp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<CAP_TIN_DOI_TAC>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public CAP_TIN_DOI_TAC getCapTinDoiTacByID(long id)
        {
            CAP_TIN_DOI_TAC data = new CAP_TIN_DOI_TAC();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CAP_TIN_DOI_TAC", "getCapTinDoiTacByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.CAP_TIN_DOI_TAC where p.ID == id select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as CAP_TIN_DOI_TAC;
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
        public ResultEntity update(CAP_TIN_DOI_TAC detail_in, long? nguoi)
        {
            CAP_TIN_DOI_TAC detail = new CAP_TIN_DOI_TAC();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.CAP_TIN_DOI_TAC where p.ID == detail_in.ID select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.ID_DOI_TAC = detail_in.ID_DOI_TAC;
                        detail.SO_TIN_CAP = detail_in.SO_TIN_CAP;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CAP_TIN_DOI_TAC");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(CAP_TIN_DOI_TAC detail_in, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail_in.NGAY_CAP = DateTime.Now;
                    detail_in.NGUOI_CAP = nguoi;
                    detail_in = context.CAP_TIN_DOI_TAC.Add(detail_in);
                    context.SaveChanges();
                    res.ResObject = detail_in;
                    #region "thêm quota vào tổng tin cấp đối tác"
                    DoiTacBO doiTacBO = new DoiTacBO();
                    DOI_TAC doiTac = new DOI_TAC();
                    doiTac = doiTacBO.getDoiTacByID(detail_in.ID_DOI_TAC);
                    doiTac.TONG_TIN_CAP = (doiTac.TONG_TIN_CAP!=null? doiTac.TONG_TIN_CAP:0) + detail_in.SO_TIN_CAP;
                    doiTacBO.updateTongTinCap(doiTac, nguoi);
                    #endregion
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CAP_TIN_DOI_TAC");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity delete(short id, long? nguoi, bool is_delete=false)
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
                        var sql = @"update CAP_TIN_DOI_TAC set IS_DELETE = 1,NGUOI_SUA=:0,NGAY_SUA=:1 where ID = :2";
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, id);
                    }
                    else
                    {
                        var sql = @"delete CAP_TIN_DOI_TAC where ID = :0";
                        context.Database.ExecuteSqlCommand(sql, id);
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CAP_TIN_DOI_TAC");
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
