using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class DiemTongKetEntity
    {
        public string MA_HS { get; set; }
        public string TEN_HS { get; set; }
        public Nullable<System.DateTime> NGAY_SINH { get; set; }
        public long ID_HOC_SINH { get; set; }
        public long ID_LOP { get; set; }
        public long ID_TRUONG { get; set; }
        public Nullable<decimal> TB_KY1 { get; set; }
        public Nullable<decimal> TB_KY2 { get; set; }
        public Nullable<decimal> TB_CN { get; set; }
        public Nullable<decimal> TB_CN_TTL { get; set; }
        public Nullable<short> MA_HOC_LUC_KY1 { get; set; }
        public Nullable<short> MA_HOC_LUC_KY2 { get; set; }
        public Nullable<short> MA_HOC_LUC_CA_NAM { get; set; }
        public Nullable<short> MA_HOC_LUC_CA_NAM_TTL { get; set; }
        public Nullable<short> MA_HANH_KIEM_KY1 { get; set; }
        public Nullable<short> MA_HANH_KIEM_KY2 { get; set; }
        public Nullable<short> MA_HANH_KIEM_CA_NAM { get; set; }
        public Nullable<short> MA_HANH_KIEM_CA_NAM_TTL { get; set; }
        public Nullable<short> MA_DANH_HIEU_KY1 { get; set; }
        public Nullable<short> MA_DANH_HIEU_KY2 { get; set; }
        public Nullable<short> MA_DANH_HIEU_CN { get; set; }
        public Nullable<short> IS_LEN_LOP { get; set; }
        public Nullable<short> IS_TOT_NGHIEP { get; set; }
        public long ID { get; set; }
        public Nullable<System.DateTime> NGAY_TAO { get; set; }
        public Nullable<System.DateTime> NGAY_SUA { get; set; }
        public Nullable<long> NGUOI_TAO { get; set; }
        public Nullable<long> NGUOI_SUA { get; set; }
        public Nullable<short> IS_DELETE { get; set; }
        public short ID_NAM_HOC { get; set; }
    }
}
