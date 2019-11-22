using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class KhoThucPhamEntity
    {
        public long ID { get; set; }
        public long ID_TRUONG { get; set; }
        public short ID_NHOM_THUC_PHAM { get; set; }
        public long ID_THUC_PHAM { get; set; }
        public string TEN_THUC_PHAM { get; set; }
        public string TEN_NHOM_THUC_PHAM { get; set; }
        public Nullable<short> DON_VI_TINH { get; set; }
        public Nullable<decimal> SO_LUONG { get; set; }
        public Nullable<decimal> TONG_GIA { get; set; }
    }
}
