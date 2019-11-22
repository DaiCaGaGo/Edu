using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class HocPhiDotThuLopEntity
    {
        public long ID { get; set; }
        public long ID_DOT_THU { get; set; }
        public long ID_LOP { get; set; }
        public long ID_TRUONG { get; set; }
        public long ID_NAM_HOC { get; set; }
        public Nullable<short> IS_TIEN_AN { get; set; }
        public Nullable<long> SO_TIEN_AN { get; set; }
        public Nullable<long> TONG_TIEN { get; set; }
        public string GHI_CHU { get; set; }
        public Nullable<bool> IS_DELETE { get; set; }
        public Nullable<System.DateTime> NGAY_TAO { get; set; }
        public Nullable<long> NGUOI_TAO { get; set; }
        public Nullable<System.DateTime> NGAY_SUA { get; set; }
        public Nullable<long> NGUOI_SUA { get; set; }
        public short ID_KHOI { get; set; }
    }
}
