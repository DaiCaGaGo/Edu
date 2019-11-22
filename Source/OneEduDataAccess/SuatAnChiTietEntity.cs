using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class SuatAnChiTietEntity
    {
        public long ID_SUAT_AN { get; set; }
        //public long ID_THUC_DON { get; set; }
        public short ID_NHOM_THUC_PHAM { get; set; }
        public long ID_THUC_PHAM { get; set; }
        public string TEN { get; set; }
        public short? DON_VI_TINH { get; set; }
        public Nullable<decimal> SO_LUONG { get; set; }
        public Nullable<decimal> NANG_LUONG_KCAL { get; set; }
        public Nullable<decimal> PROTID { get; set; }
        public Nullable<decimal> GLUCID { get; set; }
        public Nullable<decimal> LIPID { get; set; }
        public Nullable<decimal> NANG_LUONG_KCAL_OLD { get; set; }
        public Nullable<decimal> PROTID_OLD { get; set; }
        public Nullable<decimal> GLUCID_OLD { get; set; }
        public Nullable<decimal> LIPID_OLD { get; set; }
        public Nullable<short> ID_KHOI { get; set; }
        public long ID_TRUONG { get; set; }
        public long ID_BUA_AN { get; set; }
        public int IS_NANG_LUONG_CHUAN { get; set; }
        public int IS_NANG_LUONG_SUAT_AN { get; set; }
    }
}
