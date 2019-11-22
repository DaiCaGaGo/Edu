using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class LopMonTruongEntity
    {
        public long ID_LOP { get; set; }
        public long ID_MON_TRUONG { get; set; }
        public short HOC_KY { get; set; }
        public string TEN_LOP { get; set; }
        public string TEN_MON_TRUONG { get; set; }
    }
}
