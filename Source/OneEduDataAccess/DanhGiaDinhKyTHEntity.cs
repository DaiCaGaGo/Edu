using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class DanhGiaDinhKyTHEntity
    {
        public string MA_HS { get; set; }
        public string TEN_HS { get; set; }
        public short MA_GIOI_TINH { get; set; }
        public string TEN_GIOI_TINH { get; set; }
        public Nullable<System.DateTime> NGAY_SINH { get; set; }
        public long ID { get; set; }
        public long ID_TRUONG { get; set; }
        public short MA_NAM_HOC { get; set; }
        public long ID_LOP { get; set; }
        public long ID_HOC_SINH { get; set; }
        public short MA_KY_DG { get; set; }
        public short MA_KHOI { get; set; }
        public string NL_TPVTQ { get; set; }
        public string NL_HT { get; set; }
        public string NL_TGQVD { get; set; }
        public string NL_MANX { get; set; }
        public string NL_NX { get; set; }
        public string PC_CHCL { get; set; }
        public string PC_TTTN { get; set; }
        public string PC_TTKL { get; set; }
        public string PC_DKYT { get; set; }
        public string PC_MANX { get; set; }
        public string PC_NX { get; set; }
        public string NX_GVCN { get; set; }
        public Nullable<long> NGUOI_TAO { get; set; }
        public Nullable<System.DateTime> NGAY_TAO { get; set; }
        public Nullable<long> NGUOI_SUA { get; set; }
        public Nullable<System.DateTime> NGAY_SUA { get; set; }
        public Nullable<short> IS_DELETE { get; set; }
    }
}
