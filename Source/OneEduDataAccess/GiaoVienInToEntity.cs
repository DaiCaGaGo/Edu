using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class GiaoVienInToEntity
    {
        public long ID { get; set; }
        public long ID_TO { get; set; }
        public long ID_TRUONG { get; set; }
        public long ID_GIAO_VIEN { get; set; }
        public string TEN_GIAO_VIEN { get; set; }
        public string SDT { get; set; }
    }
}
