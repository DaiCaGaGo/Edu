using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class LichThiEntity
    {
        public long? ID { get; set; }
        public long? ID_TRUONG { get; set; }
        public short? ID_KHOI { get; set; }
        public short? ID_NAM_HOC { get; set; }
        public short? HOC_KY { get; set; }
        public long? ID_LOP { get; set; }
        public long? ID_MON_TRUONG { get; set; }
        public Nullable<System.DateTime> TIME_15P { get; set; }
        public Nullable<System.DateTime> TIME_1T { get; set; }
        public Nullable<System.DateTime> TIME_GK { get; set; }
        public Nullable<System.DateTime> TIME_HK { get; set; }
        public Nullable<short> THOI_GIAN_LAM_BAI { get; set; }
        public Nullable<System.DateTime> NGAY_TAO { get; set; }
        public long? NGUOI_TAO { get; set; }
        public Nullable<System.DateTime> NGAY_SUA { get; set; }
        public long? NGUOI_SUA { get; set; }
        public string TEN { get; set; }
    }
}
