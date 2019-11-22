using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class CauHinhCaHocEntity
    {
        public long ID { get; set; }
        public DateTime? NGAY_BAT_DAU { get; set; }
        public DateTime? NGAY_KET_THUC { get; set; }
    }
}
