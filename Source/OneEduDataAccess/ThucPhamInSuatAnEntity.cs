using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class ThucPhamInSuatAnEntity
    {
        public long ID { get; set; }
        public string TEN { get; set; }
        public string TEN_EN { get; set; }
        public short? DON_VI_TINH { get; set; }
        public Nullable<decimal> SO_LUONG { get; set; }
        public Nullable<decimal> NANG_LUONG_KCAL { get; set; }
        public Nullable<decimal> PROTID { get; set; }
        public Nullable<decimal> GLUCID { get; set; }
        public Nullable<decimal> LIPID { get; set; }
        public Nullable<decimal> NANG_LUONG_KCAL_NEW { get; set; }
        public Nullable<decimal> PROTID_NEW { get; set; }
        public Nullable<decimal> GLUCID_NEW { get; set; }
        public Nullable<decimal> LIPID_NEW { get; set; }
        public Nullable<long> ID_SUAT_AN_CHI_TIET { get; set; }
        public Nullable<long> ID_SUAT_AN { get; set; }
        public Nullable<short> ID_KHOI { get; set; }
        public Nullable<short> ID_NHOM_THUC_PHAM { get; set; }
        public Nullable<int> IS_CHON { get; set; }
    }
}
