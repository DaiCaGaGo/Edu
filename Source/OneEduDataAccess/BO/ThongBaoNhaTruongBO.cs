using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class ThongBaoNhaTruongBO
    {
        public List<THONG_BAO_NHA_TRUONG> getThongBaoByTruong(long id_truong, string ma_cap_hoc, short id_nam_hoc, string noi_dung, short? loai_tb)
        {
            List<THONG_BAO_NHA_TRUONG> data = new List<THONG_BAO_NHA_TRUONG>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("THONG_BAO_NHA_TRUONG", "getThongBaoByTruong", id_truong, ma_cap_hoc, id_nam_hoc, noi_dung, loai_tb);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.THONG_BAO_NHA_TRUONG where p.ID_TRUONG == id_truong && p.CAP_HOC == ma_cap_hoc && p.ID_NAM_HOC == id_nam_hoc select p);
                    if (loai_tb != null)
                        tmp = tmp.Where(x => x.LOAI_THONG_BAO == loai_tb);
                    if (!string.IsNullOrEmpty(noi_dung))
                        tmp = tmp.Where(x => x.NOI_DUNG.ToLower().Contains(noi_dung.ToLower()));
                    tmp = tmp.OrderByDescending(x => x.NGAY_TAO);
                    data = tmp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<THONG_BAO_NHA_TRUONG>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public THONG_BAO_NHA_TRUONG getThongBaoByID(long id)
        {
            THONG_BAO_NHA_TRUONG data = new THONG_BAO_NHA_TRUONG();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("THONG_BAO_NHA_TRUONG", "getThongBaoByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.THONG_BAO_NHA_TRUONG where p.ID == id select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as THONG_BAO_NHA_TRUONG;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public ResultEntity update(THONG_BAO_NHA_TRUONG detail_in, long? nguoi)
        {
            THONG_BAO_NHA_TRUONG detail = new THONG_BAO_NHA_TRUONG();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.THONG_BAO_NHA_TRUONG where p.ID == detail_in.ID select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.ID_TRUONG = detail_in.ID_TRUONG;
                        detail.CAP_HOC = detail_in.CAP_HOC;
                        detail.ID_NAM_HOC = detail_in.ID_NAM_HOC;
                        detail.NOI_DUNG = detail_in.NOI_DUNG;
                        detail.ANH_NOI_DUNG = detail_in.ANH_NOI_DUNG;
                        detail.LOAI_THONG_BAO = detail_in.LOAI_THONG_BAO;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("THONG_BAO_NHA_TRUONG");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(THONG_BAO_NHA_TRUONG detail_in, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    long newID = context.Database.SqlQuery<long>("SELECT THONG_BAO_NHA_TRUONG_SEQ.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                    detail_in.ID = newID;
                    detail_in.NGAY_TAO = DateTime.Now;
                    detail_in.NGUOI_TAO = nguoi;
                    detail_in = context.THONG_BAO_NHA_TRUONG.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("THONG_BAO_NHA_TRUONG");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity delete(long id, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    var sql = @"delete from THONG_BAO_NHA_TRUONG where ID=:0";
                    context.Database.ExecuteSqlCommand(sql, id);
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("THONG_BAO_NHA_TRUONG");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
    }
}
