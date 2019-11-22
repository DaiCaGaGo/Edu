using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class DanhGiaDinhKyMonTHEntity
    {
        public string MA_HS { get; set; }
        public string TEN_HS { get; set; }
        public short MA_GIOI_TINH { get; set; }
        public string TEN_GIOI_TINH { get; set; }
        public Nullable<System.DateTime> NGAY_SINH { get; set; }
        public long ID { get; set; }
        public long ID_TRUONG { get; set; }
        public long ID_LOP { get; set; }
        public long ID_HOC_SINH { get; set; }
        public long ID_MON_TRUONG { get; set; }
        public short MA_KY_DG { get; set; }
        public string MA_NX { get; set; }
        public string NOI_DUNG_NX { get; set; }
        public Nullable<short> KTDK { get; set; }
        public Nullable<long> NGUOI_TAO { get; set; }
        public Nullable<System.DateTime> NGAY_TAO { get; set; }
        public Nullable<long> NGUOI_SUA { get; set; }
        public Nullable<System.DateTime> NGAY_SUA { get; set; }
        public Nullable<short> IS_DELETE { get; set; }
        public Nullable<short> MA_NAM_HOC { get; set; }
        public Nullable<short> MA_KHOI { get; set; }
        public string MUC { get; set; }
        public string TEN_MON_HOC { get; set; }
        public long ID_MON_HOC { get; set; }
    }
}
