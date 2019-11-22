using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class HocSinhDangKyAnEntity
    {
        public long? ID { get; set; }
        public long ID_HOC_SINH { get; set; }
        public string MA_HOC_SINH { get; set; }
        public string HO_TEN { get; set; }
        public DateTime? NGAY_SINH { get; set; }
        public long ID_LOP { get; set; }
        public string TEN_LOP { get; set; }
        public short ID_KHOI { get; set; }
        public string TEN_KHOI { get; set; }
        public long? ID_BUA_AN { get; set; }
        public string TEN_BUA_AN { get; set; }
        public long IS_DK { get; set; }

    }
}
