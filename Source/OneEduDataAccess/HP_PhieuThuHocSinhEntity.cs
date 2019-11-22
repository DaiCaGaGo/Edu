using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class HP_PhieuThuHocSinhEntity
    {
        public long STT { get; set; }
        public string MA { get; set; }
        public string HO_TEN { get; set; }
        public short ID_KHOI { get; set; }
        public long ID_LOP { get; set; }
        public Nullable<DateTime> NGAY_SINH { get; set; }
        public string NOI_DUNG { get; set; }
        public string SO_TIEN { get; set; }
    }
}
