using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class CapTinTruongBO
    {
        #region get
        public List<CAP_TIN_TRUONG> getCapTinTruong(short id_doi_tac, long? id_truong)
        {
            List<CAP_TIN_TRUONG> data = new List<CAP_TIN_TRUONG>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CAP_TIN_TRUONG", "getCapTinTruong", id_doi_tac, id_truong);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.CAP_TIN_TRUONG where p.ID_DOI_TAC == id_doi_tac && p.IS_DELETE != true select p);
                    if (id_truong != null)
                        tmp = tmp.Where(x => x.ID_TRUONG == id_truong);
                    tmp = tmp.OrderByDescending(x => x.NGAY_CAP);
                    data = tmp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<CAP_TIN_TRUONG>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public CAP_TIN_TRUONG getCapTinTruongByID(long id)
        {
            CAP_TIN_TRUONG data = new CAP_TIN_TRUONG();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("CAP_TIN_TRUONG", "getCapTinTruongByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.CAP_TIN_TRUONG where p.ID == id select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as CAP_TIN_TRUONG;
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
        public ResultEntity update(CAP_TIN_TRUONG detail_in, long? nguoi)
        {
            CAP_TIN_TRUONG detail = new CAP_TIN_TRUONG();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.CAP_TIN_TRUONG where p.ID == detail_in.ID select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.ID_DOI_TAC = detail_in.ID_DOI_TAC;
                        detail.ID_TRUONG = detail_in.ID_TRUONG;
                        detail.SO_TIN_CAP = detail_in.SO_TIN_CAP;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CAP_TIN_TRUONG");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(CAP_TIN_TRUONG detail_in, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    #region "check quỹ tin đối tác"
                    long tong_cap_truong = 0, tong_truong_su_dung = 0;
                    TruongBO truongBO = new TruongBO();
                    TRUONG truong = new TRUONG();
                    truong = truongBO.getTruongById(detail_in.ID_TRUONG);
                    if (truong != null)
                    {
                        tong_cap_truong = truong.TONG_TIN_CAP != null ? truong.TONG_TIN_CAP.Value : 0;
                        tong_truong_su_dung = truong.TONG_TIN_DA_DUNG != null ? truong.TONG_TIN_DA_DUNG.Value : 0;
                    }
                    DOI_TAC doiTac = new DOI_TAC();
                    DoiTacBO doiTacBO = new DoiTacBO();
                    doiTac = doiTacBO.getDoiTacByID(detail_in.ID_DOI_TAC);
                    if (doiTac.TONG_TIN_CAP == null || (doiTac.TONG_TIN_CAP != null && doiTac.TONG_TIN_CAP.Value < Convert.ToInt64(detail_in.SO_TIN_CAP)))
                    {
                        res.Res = false;
                        res.Msg = "Số tin cấp không được lớn hơn số tin đại lý hiện có. Vui lòng kiểm tra lại!";
                    }
                    else if (tong_cap_truong + detail_in.SO_TIN_CAP < tong_truong_su_dung)
                    {
                        res.Res = false;
                        res.Msg = "Số tin thu hồi không được vượt quá số tin đã sử dụng. Vui lòng kiểm tra lại!";
                    }
                    else
                    {
                        detail_in.NGAY_CAP = DateTime.Now;
                        detail_in.NGUOI_CAP = nguoi;
                        detail_in = context.CAP_TIN_TRUONG.Add(detail_in);
                        context.SaveChanges();
                        #region "cập nhật quỹ tin trường học"
                        truong.TONG_TIN_CAP = (truong.TONG_TIN_CAP != null ? truong.TONG_TIN_CAP : 0) + detail_in.SO_TIN_CAP;
                        truongBO.updateQuyTinDoiTacCap(detail_in.ID_TRUONG, detail_in.SO_TIN_CAP, 0, nguoi);
                        #endregion
                    }
                    #endregion
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CAP_TIN_TRUONG");
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
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    if (!is_delete)
                    {
                        var sql = @"update CAP_TIN_TRUONG set IS_DELETE = 1,NGUOI_SUA=:0,NGAY_SUA=:1 where ID = :2";
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, id);
                    }
                    else
                    {
                        var sql = @"delete CAP_TIN_TRUONG where ID = :0";
                        context.Database.ExecuteSqlCommand(sql, id);
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CAP_TIN_TRUONG");
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
