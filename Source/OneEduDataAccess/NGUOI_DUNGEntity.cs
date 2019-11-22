using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class NGUOI_DUNGEntity
    {
        public long ID { get; set; }
        public string TEN_DANG_NHAP { get; set; }
        public string FACE_BOOK { get; set; }
        public string MAT_KHAU { get; set; }
        public string TEN_HIEN_THI { get; set; }
        public string EMAIL { get; set; }
        public string SDT { get; set; }
        public string DIA_CHI { get; set; }
        public string ANH_DAI_DIEN { get; set; }
        public string GHI_CHU { get; set; }
        public Nullable<bool> TRANG_THAI { get; set; }
        public Nullable<System.DateTime> NGAY_TAO { get; set; }
        public Nullable<long> NGUOI_TAO { get; set; }
        public Nullable<System.DateTime> NGAY_SUA { get; set; }
        public Nullable<long> NGUOI_SUA { get; set; }
        public Nullable<bool> IS_ROOT { get; set; }
        public string MA_NGON_NGU { get; set; }
        public Nullable<bool> IS_DELETE { get; set; }
        public string QUYEN { get; set; }
        public Nullable<bool> IS_LOGIN_GMAIL { get; set; }
        public Nullable<bool> IS_LOGIN_FACEBOOK { get; set; }
        public Nullable<short> ID_DOI_TAC { get; set; }
    }
}
