using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class KhoaSoTheoMonEntity
    {
        public long ID_TRUONG { get; set; }
        public string TEN { get; set; }
        public long? ID_LOP { get; set; }
        public long? ID_MON { get; set; }
        public int? TRANG_THAI { get; set; }
        public long? ID { get; set; }
        public DateTime? NGAY_KHOA { get; set; }
        public short? NAM_HOC { get; set; }
        public long? NGUOI_KHOA { get; set; }
        public string MA_CAP_HOC { get; set; }
        public short? GIAI_DOAN { get; set; }
        public short? HOC_KY { get; set; }

    }
}
