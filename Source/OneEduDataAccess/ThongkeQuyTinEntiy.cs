using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class ThongkeQuyTinEntiy
    {
        public long ID_TRUONG { get; set; }
        public string TEN { get; set; }
        public long TONG_CAP_LL { get; set; }
        public long TONG_CAP_TB { get; set; }
        public long TONG_THEM_LL { get; set; }
        public long TONG_THEM_TB { get; set; }
        public long TONG_DA_GUI_LL { get; set; }
        public long TONG_DA_GUI_TB { get; set; }
        public float PHAN_TRAM_LL { get; set; }
        public float PHAN_TRAM_TB { get; set; }
    }
}
