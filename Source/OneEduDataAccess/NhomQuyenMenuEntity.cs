using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class NhomQuyenMenuEntity
    {
        public long ID { get; set; }
        public long? ID_CHA { get; set; }
        public long? ID_NHOM_QUYEN_MENU { get; set; }
        public long? ID_MENU { get; set; }
        public string TEN { get; set; }
        public int? IS_XEM { get; set; }
        public int? IS_THEM { get; set; }
        public int? IS_SUA { get; set; }
        public int? IS_XOA { get; set; }
        public int? IS_SEND_SMS { get; set; }
        public int? IS_VIEW_INFOR { get; set; }
        public int? IS_EXPORT { get; set; }
    }
}
