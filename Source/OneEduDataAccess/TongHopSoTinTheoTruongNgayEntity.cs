using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class TongHopSoTinTheoTruongNgayEntity
    {
        public long ID_TRUONG { get; set; }
        public string TEN { get; set; }
        public Nullable<long> TONG_TIN { get; set; }
        public Nullable<long> TIN_THANH_CONG { get; set; }
        public Nullable<long> TIN_GUI_LOI { get; set; }
        public Nullable<long> TIN_DUNG_GUI { get; set; }
    }
}
