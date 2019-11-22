using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class PhieuThuHocPhiHocSinhEntity
    {
        public long ID { get; set; }
        public Nullable<long> ID_DOT_THU_LOP { get; set; }
        public long ID_TRUONG { get; set; }
        public short ID_KHOI { get; set; }
        public long ID_LOP { get; set; }
        public long ID_HOC_SINH { get; set; }
        public Nullable<decimal> TONG_TIEN { get; set; }
        public string GHI_CHU { get; set; }
        public Nullable<bool> IS_DELETE { get; set; }
        public Nullable<System.DateTime> NGAY_TAO { get; set; }
        public Nullable<long> NGUOI_TAO { get; set; }
        public Nullable<System.DateTime> NGAY_SUA { get; set; }
        public Nullable<long> NGUOI_SUA { get; set; }
        public Nullable<System.DateTime> THOI_GIAN_THU { get; set; }
        public Nullable<short> ID_DOT_THU { get; set; }
        public Nullable<short> HOC_KY { get; set; }
        public Nullable<short> THANG { get; set; }
        public string TEN_HOC_SINH { get; set; }
    }
}
