using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class DiemTongKetHocKyEntity
    {
        public long ID_HOC_SINH { get; set; }
        public long HOC_KY { get; set; }
        public Nullable<decimal> DIEM_TB_KY1 { get; set; }
        public Nullable<decimal> DIEM_TB_KY2 { get; set; }
    }
}
