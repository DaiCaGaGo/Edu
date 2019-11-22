using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class MonHocTruongCauHinhEntity : Model.MON_HOC_TRUONG
    {
        public bool IS_MIENG { get; set; }
        public bool IS_15P { get; set; }
        public bool IS_1THS1 { get; set; }
        public bool IS_1THS2 { get; set; }
    }
}
