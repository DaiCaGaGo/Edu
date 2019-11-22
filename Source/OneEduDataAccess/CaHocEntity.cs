using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class CaHocEntity
    {
        public long ID { get; set; }
        public Nullable<short> TIET { get; set; }
        public Nullable<long> ID_LOP { get; set; }
        public Nullable<short> ID_HOC_KY { get; set; }
        public Nullable<long> ID_MON_2 { get; set; }
        public Nullable<long> ID_MON_3 { get; set; }
        public Nullable<long> ID_MON_4 { get; set; }
        public Nullable<long> ID_MON_5 { get; set; }
        public Nullable<long> ID_MON_6 { get; set; }
        public Nullable<long> ID_MON_7 { get; set; }
        public Nullable<long> ID_MON_8 { get; set; }
        public string THOI_GIAN { get; set; }
        public Nullable<long> ID_CAU_HINH_CA_HOC { get; set; }
        public Nullable<short> IS_DELETE { get; set; }
        public Nullable<System.DateTime> NGAY_TAO { get; set; }
        public Nullable<long> NGUOI_TAO { get; set; }
        public Nullable<System.DateTime> NGAY_SUA { get; set; }
        public Nullable<long> NGUOI_SUA { get; set; }
        public short MA_NAM_HOC { get; set; }
        public long ID_TRUONG { get; set; }
    }
}
