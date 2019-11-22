using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class DanhSachMonByLopEntity
    {
        public long ID_MON_TRUONG { get; set; }
        public string TEN { get; set; }
        public Nullable<short> HE_SO { get; set; }
        public Nullable<short> KIEU_MON { get; set; }
        public Nullable<short> HOC_KY { get; set; }
        public int? MON_CHUYEN { get; set; }
    }
}
