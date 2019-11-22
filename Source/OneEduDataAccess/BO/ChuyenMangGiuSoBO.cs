using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class ChuyenMangGiuSoBO
    {
        public List<CHUYEN_MANG_GIU_SO> getChuyenMangGiuSo()
        {
            var QICache = new DefaultCacheProvider();
            string strSession = QICache.BuildCachedKey("CHUYEN_MANG_GIU_SO", "getChuyenMangGiuSo");
            List<CHUYEN_MANG_GIU_SO> detail = new List<CHUYEN_MANG_GIU_SO>();
            if (!QICache.IsSet(strSession))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    detail = (from p in context.CHUYEN_MANG_GIU_SO select p).ToList();
                    QICache.Set(strSession, detail, 24 * 60 * 60);
                }
            }
            else
            {
                try
                {
                    detail = QICache.Get(strSession) as List<CHUYEN_MANG_GIU_SO>;
                }
                catch
                {
                    QICache.Invalidate(strSession);
                }
            }
            return detail;
        }
        public CHUYEN_MANG_GIU_SO getNhaMangBySDT(long index_sdt)
        {
            var QICache = new DefaultCacheProvider();
            string strSession = QICache.BuildCachedKey("CHUYEN_MANG_GIU_SO", "getNhaMangBySDT", index_sdt);
            CHUYEN_MANG_GIU_SO detail = new CHUYEN_MANG_GIU_SO();
            if (!QICache.IsSet(strSession))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    detail = (from p in context.CHUYEN_MANG_GIU_SO where p.SDT == index_sdt select p).FirstOrDefault();
                  //  QICache.Set(strSession, detail, 300000);
                }
            }
            else
            {
                try
                {
                    detail = QICache.Get(strSession) as CHUYEN_MANG_GIU_SO;
                }
                catch
                {
                    QICache.Invalidate(strSession);
                }
            }
            return detail;
        }
        public ResultEntity update(CHUYEN_MANG_GIU_SO detail_in, long? nguoi)
        {
            CHUYEN_MANG_GIU_SO detail = new CHUYEN_MANG_GIU_SO();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.CHUYEN_MANG_GIU_SO
                              where p.SDT == detail_in.SDT
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        if (detail.ID_PROCESS == null || detail_in.ID_PROCESS > detail.ID_PROCESS)
                        {
                            detail.ID_PROCESS = detail_in.ID_PROCESS;
                            detail.LOAI_NHA_MANG = detail_in.LOAI_NHA_MANG;
                            context.SaveChanges();
                            res.ResObject = detail;
                        }
                    }
                    else
                    {
                        detail = new CHUYEN_MANG_GIU_SO();
                        detail.SDT = detail_in.SDT;
                        detail.ID_PROCESS = detail_in.ID_PROCESS;
                        detail.LOAI_NHA_MANG = detail_in.LOAI_NHA_MANG;
                        context.CHUYEN_MANG_GIU_SO.Add(detail);
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("CHUYEN_MANG_GIU_SO");
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
