using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class HocSinhEntity
    {
        public long ID { get; set; }
        public string MA { get; set; }
        public long ID_LOP { get; set; }
        public string TEN { get; set; }
        public Nullable<System.DateTime> NGAY_SINH { get; set; }
        public short MA_GIOI_TINH { get; set; }
        public string HO_TEN_BO { get; set; }
        public string HO_TEN_ME { get; set; }
        public string HO_TEN_NGUOI_BAO_HO { get; set; }
        public Nullable<short> NAM_SINH_BO { get; set; }
        public Nullable<short> NAM_SINH_ME { get; set; }
        public Nullable<short> NAM_SINH_NGUOI_BAO_HO { get; set; }
        public string SDT_BO { get; set; }
        public string SDT_ME { get; set; }
        public string SDT_NBH { get; set; }
        public string SDT_NHAN_TIN { get; set; }
        public Nullable<short> TRANG_THAI_HOC { get; set; }
        public Nullable<short> IS_DK_KY1 { get; set; }
        public Nullable<short> IS_DK_KY2 { get; set; }
        public Nullable<short> IS_MIEN_GIAM_KY1 { get; set; }
        public Nullable<short> IS_MIEN_GIAM_KY2 { get; set; }
        public Nullable<System.DateTime> NGAY_DK_KY1 { get; set; }
        public Nullable<System.DateTime> NGAY_DK_KY2 { get; set; }
        public Nullable<System.DateTime> NGAY_HUY_KY1 { get; set; }
        public Nullable<System.DateTime> NGAY_HUY_KY2 { get; set; }
        public Nullable<System.DateTime> NGAY_TAO { get; set; }
        public Nullable<long> NGUOI_TAO { get; set; }
        public Nullable<System.DateTime> NGAY_SUA { get; set; }
        public Nullable<long> NGUOI_SUA { get; set; }
        public Nullable<short> IS_DELETE { get; set; }
        public Nullable<short> IS_CON_GV { get; set; }
        public long ID_TRUONG { get; set; }
        public short ID_KHOI { get; set; }
        public string MA_CAP_HOC { get; set; }
        public string DIA_CHI { get; set; }
        public short ID_NAM_HOC { get; set; }
        public string HO_DEM { get; set; }
        public string HO_TEN { get; set; }
        public Nullable<short> THU_TU { get; set; }
        public Nullable<short> MA_QUOC_TICH { get; set; }
        public Nullable<short> MA_TINH_THANH { get; set; }
        public Nullable<short> MA_QUAN_HUYEN { get; set; }
        public Nullable<short> MA_KHU_VUC { get; set; }
        public Nullable<short> MA_DAN_TOC { get; set; }
        public Nullable<short> MA_DOI_TUONG_CS { get; set; }
        public string NOI_SINH { get; set; }
        public string SO_CMND { get; set; }
        public Nullable<System.DateTime> NGAY_CAP_CMND { get; set; }
        public string NOI_CAP_CMND { get; set; }
        public Nullable<long> MA_XA_PHUONG { get; set; }
        public Nullable<short> IS_GUI_BO_ME { get; set; }
        public string SDT_NHAN_TIN2 { get; set; }
        public string TEN_KHOI { get; set; }
        public string TEN_LOP { get; set; }
        public string TRANG_THAI_HS { get; set; }
        public string TEN_GIOI_TINH { get; set; }
        public string STR_CON_GV { get; set; }
        public string STR_GUI_BO_ME { get; set; }
        public string STR_SDT_NHAN_TIN { get; set; }
        public string STR_DK_KY1 { get; set; }
        public string STR_DK_KY2 { get; set; }
        public string STR_MIEN_KY1 { get; set; }
        public string STR_MIEN_KY2 { get; set; }
        public string STR_NGAY_SINH { get; set; }
        public string ZALO_CODE { get; set; }
        public string TEN_TRUONG { get; set; }
        public string SDT_BM { get; set; }
    }
}
