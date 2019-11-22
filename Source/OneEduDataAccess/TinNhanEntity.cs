using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class TinNhanEntity
    {
        public string TEN_NGUOI_NHAN { get; set; }
        public Nullable<short> GUI_BO_ME { get; set; }
        public long ID { get; set; }
        public long ID_TRUONG { get; set; }
        public Nullable<short> ID_KHOI { get; set; }
        public Nullable<long> ID_LOP { get; set; }
        public string TEN_LOP { get; set; }
        public Nullable<long> ID_NGUOI_NHAN { get; set; }
        public short LOAI_NGUOI_NHAN { get; set; }
        public string BRAND_NAME { get; set; }
        public string SDT_NHAN { get; set; }
        public Nullable<short> MA_GOI_TIN { get; set; }
        public string NOI_DUNG { get; set; }
        public string NOI_DUNG_KHONG_DAU { get; set; }
        public Nullable<long> SO_TIN { get; set; }
        public Nullable<short> TRANG_THAI { get; set; }
        public Nullable<System.DateTime> THOI_GIAN_GUI { get; set; }
        public Nullable<long> NGUOI_GUI { get; set; }
        public Nullable<System.DateTime> THOI_GIAN_NHAN { get; set; }
        public Nullable<short> ID_DOI_TAC { get; set; }
        public string LOAI_NHA_MANG { get; set; }
        public Nullable<short> KIEU_GUI { get; set; }
        public Nullable<short> IS_UNICODE { get; set; }
        public Nullable<short> IS_DA_NHAN { get; set; }
        public Nullable<short> SEND_NUMBER { get; set; }
        public Nullable<System.DateTime> NGAY_TAO { get; set; }
        public Nullable<long> NGUOI_TAO { get; set; }
        public Nullable<System.DateTime> NGAY_SUA { get; set; }
        public Nullable<long> NGUOI_SUA { get; set; }
        public Nullable<long> ID_NHAN_XET_HANG_NGAY { get; set; }
        public Nullable<long> ID_TONG_HOP_NXHN { get; set; }
        public short LOAI_TIN { get; set; }
        public Nullable<long> ID_THONG_BAO { get; set; }
        public short NAM_GUI { get; set; }
        public short THANG_GUI { get; set; }
        public short TUAN_GUI { get; set; }
        public string CP { get; set; }
        public string RES_CODE { get; set; }
    }
}
