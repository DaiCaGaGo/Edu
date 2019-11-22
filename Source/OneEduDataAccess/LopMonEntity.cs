using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class LopMonEntity 
    {
        public long ID { get; set; }
        public short? ID_MON_HOC { get; set; }
        public string TEN { get; set; }
        public int? KIEU_MON { get; set; }
        public int? IS_CHECK { get; set; }
        public long? ID_LOP_MON { get; set; }
        public long? ID_GIAO_VIEN { get; set; }
        public short? SO_COT_DIEM_MIENG { get; set; }
        public short? SO_COT_DIEM_15P { get; set; }
        public short? SO_COT_DIEM_1T_HS1 { get; set; }
        public short? SO_COT_DIEM_1T_HS2 { get; set; }
        public short? ID_LOP { get; set; }
        public int? IS_MON_CHUYEN { get; set; }

    }
}
