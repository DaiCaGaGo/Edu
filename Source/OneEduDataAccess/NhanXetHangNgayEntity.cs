using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class NhanXetHangNgayEntity
    {
        public string MA_HS { get; set; }
        public string TEN_HS { get; set; }
        public short MA_GIOI_TINH { get; set; }
        public string TEN_GIOI_TINH { get; set; }
        public Nullable<System.DateTime> NGAY_SINH { get; set; }
        public Nullable<short> IS_GUI_BO_ME { get; set; }
        public Nullable<short> IS_DK_KY1 { get; set; }
        public Nullable<short> IS_DK_KY2 { get; set; }
        public Nullable<short> IS_MIEN_GIAM_KY1 { get; set; }
        public Nullable<short> IS_MIEN_GIAM_KY2 { get; set; }
        public Nullable<short> IS_CON_GV { get; set; }
        public string SDT { get; set; }
        public string SDT_KHAC { get; set; }
        public long ID_HS { get; set; }
        public Nullable<long> ID { get; set; }
        public Nullable<long> ID_HOC_SINH { get; set; }
        public Nullable<System.DateTime> NGAY_NX { get; set; }
        public string NOI_DUNG_NX { get; set; }
        public string TIEN_TO { get; set; }
        public string LIST_MA_NX { get; set; }
        public Nullable<long> ID_TRUONG { get; set; }
        public Nullable<short> MA_KHOI { get; set; }
        public Nullable<long> ID_LOP { get; set; }
        public Nullable<short> IS_TONG_HOP { get; set; }
        public Nullable<short> IS_SEND { get; set; }
        public Nullable<short> ID_NAM_HOC { get; set; }
    }
}
