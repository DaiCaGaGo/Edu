using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class TruongEntity
    {
        public long ID { get; set; }
        public string MA { get; set; }
        public string TEN { get; set; }
        public string BRAND_NAME { get; set; }
        public string DIA_CHI { get; set; }
        public string DIEN_THOAI { get; set; }
        public string EMAIL { get; set; }
        public string HIEU_TRUONG { get; set; }
        public string DIEN_THOAI_HT { get; set; }
        public string EMAIL_HT { get; set; }
        public string DIEN_THOAI_NLH { get; set; }
        public Nullable<int> IS_TH { get; set; }
        public Nullable<int> IS_THCS { get; set; }
        public Nullable<int> IS_THPT { get; set; }
        public Nullable<int> IS_GDTX { get; set; }
        public Nullable<int> TRANG_THAI { get; set; }
        public Nullable<short> MA_GOI_TIN { get; set; }
        public Nullable<long> SO_TIN_HS { get; set; }
        public Nullable<long> SO_HS_DANG_KY { get; set; }
        public Nullable<long> SO_HS_DUOC_MIEN { get; set; }
        public Nullable<long> TONG_TIN_CAP { get; set; }
        public Nullable<long> TONG_TIN_THEM { get; set; }
        public Nullable<long> TONG_TIN_DA_SU_DUNG { get; set; }
        public Nullable<long> TONG_TIN_CON { get; set; }
        public Nullable<System.DateTime> NGAY_TAO { get; set; }
        public Nullable<long> NGUOI_TAO { get; set; }
        public Nullable<System.DateTime> NGAY_SUA { get; set; }
        public Nullable<long> NGUOI_SUA { get; set; }
        public Nullable<int> IS_DELETE { get; set; }
        public Nullable<int> IS_MN { get; set; }
        public Nullable<int> IS_TRUONG_CHUYEN { get; set; }
    }
}
