using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class ThongKeHocSinhTheoTruongEntity
    {
        public long? ID_TRUONG { get; set; }
        public short? ID_KHOI { get; set; }
        public long? ID_LOP { get; set; }
        public string TEN_TRUONG { get; set; }
        public string TEN_KHOI { get; set; }
        public string TEN_LOP { get; set; }
        public long SO_HOC_SINH { get; set; }
    }
}
