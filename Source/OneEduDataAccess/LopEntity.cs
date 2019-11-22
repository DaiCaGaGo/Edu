using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class LopEntity : LOP
    {
        public long ID_LOP { get; set; }
        public string TEN_LOP { get; set; }
        public string TEN_KHOI { get; set; }
        public string LOAI_LOP_GDTX { get; set; }
        public string TEN_GVCN { get; set; }
        public string SDT_GVCN { get; set; }

    }
}
