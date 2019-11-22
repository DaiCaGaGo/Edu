using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class HocSinhLopEntity
    {
        public long ID { get; set; }
        public long ID_TRUONG { get; set; }
        public long ID_LOP { get; set; }
        public string TEN_LOP { get; set; }
        public string HO_TEN { get; set; }
        public string TEN { get; set; }
        public string SDT_NHAN_TIN { get; set; }
        public string SDT_NHAN_TIN2 { get; set; }
        public Nullable<DateTime> NGAY_SINH { get; set; }
        public short? IS_GUI_BO_ME { get; set; }
        public short? THU_TU { get; set; }
        public short? HE_SO { get; set; }
        public short? SO_TIN_TRONG_NGAY { get; set; }
        public Nullable<short> IS_DK_KY1 { get; set; }
        public Nullable<short> IS_DK_KY2 { get; set; }
        public Nullable<short> IS_MIEN_GIAM_KY1 { get; set; }
        public Nullable<short> IS_MIEN_GIAM_KY2 { get; set; }
        public Nullable<short> IS_CON_GV { get; set; }
    }
}
