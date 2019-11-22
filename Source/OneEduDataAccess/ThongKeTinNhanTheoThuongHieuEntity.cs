using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class ThongKeTinNhanTheoThuongHieuEntity
    {
        public long? ID_TRUONG { get; set; }
        public string TEN_TRUONG { get; set; }
        public string BRAND_NAME { get; set; }
        public string CP { get; set; }
        public string LOAI_NHA_MANG { get; set; }
        public long SO_TIN { get; set; }
    }
}
