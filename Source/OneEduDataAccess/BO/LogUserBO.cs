using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class LogUserBO
    {
        public ResultEntity insert(long? id_truong, string thao_tac, string mo_ta, long? nguoi, DateTime ngay_tao)
        {
            LOG_USER detail = new LOG_USER();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail.ID_TRUONG = id_truong;
                    detail.THAO_TAC = thao_tac;
                    detail.MO_TA = mo_ta;
                    detail.NGAY_TAO = DateTime.Now;
                    detail.NGUOI_TAO = nguoi;
                    detail = context.LOG_USER.Add(detail);
                    context.SaveChanges();
                }
                res.ResObject = detail;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("LOG_USER");
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
