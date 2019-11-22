using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class ViewBaiTapVeNhaEntity
    {
        public long ID_MON_HOC { get; set; }
        public long ID_SACH { get; set; }
        public string TEN_MON { get; set; }
        public string TEN_SACH { get; set; }
        public Nullable<short> BAI_SO { get; set; }
        public Nullable<short> TRANG_SO { get; set; }
        public string ICON { get; set; }
        public string NOI_DUNG { get; set; }
        public string BAI_TAP_CHI_TIET { get; set; }
    }
}
