//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OneEduDataAccess.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class HOC_SINH
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HOC_SINH()
        {
            this.CHAM_AN = new HashSet<CHAM_AN>();
            this.CHUYEN_CAN = new HashSet<CHUYEN_CAN>();
            this.DANG_KY_AN = new HashSet<DANG_KY_AN>();
            this.DANH_GIA_DINH_KY_MON_TH = new HashSet<DANH_GIA_DINH_KY_MON_TH>();
            this.DANH_GIA_DINH_KY_TH = new HashSet<DANH_GIA_DINH_KY_TH>();
            this.DIEM_CHI_TIET = new HashSet<DIEM_CHI_TIET>();
            this.DIEM_TONG_KET = new HashSet<DIEM_TONG_KET>();
            this.HOC_PHI_PHIEU_THU_HOC_SINH = new HashSet<HOC_PHI_PHIEU_THU_HOC_SINH>();
            this.NHAN_XET_HANG_NGAY = new HashSet<NHAN_XET_HANG_NGAY>();
        }
    
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
        public Nullable<bool> IS_DK_KY1 { get; set; }
        public Nullable<bool> IS_DK_KY2 { get; set; }
        public Nullable<bool> IS_MIEN_GIAM_KY1 { get; set; }
        public Nullable<bool> IS_MIEN_GIAM_KY2 { get; set; }
        public Nullable<System.DateTime> NGAY_DK_KY1 { get; set; }
        public Nullable<System.DateTime> NGAY_DK_KY2 { get; set; }
        public Nullable<System.DateTime> NGAY_HUY_KY1 { get; set; }
        public Nullable<System.DateTime> NGAY_HUY_KY2 { get; set; }
        public Nullable<System.DateTime> NGAY_TAO { get; set; }
        public Nullable<long> NGUOI_TAO { get; set; }
        public Nullable<System.DateTime> NGAY_SUA { get; set; }
        public Nullable<long> NGUOI_SUA { get; set; }
        public Nullable<bool> IS_DELETE { get; set; }
        public Nullable<bool> IS_CON_GV { get; set; }
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
        public Nullable<bool> IS_GUI_BO_ME { get; set; }
        public string SDT_NHAN_TIN2 { get; set; }
        public string ZALO_CODE { get; set; }
        public string CHIEU_CAO { get; set; }
        public string CAN_NANG { get; set; }
        public string ANH_DAI_DIEN { get; set; }
        public Nullable<bool> IS_HOI_TRUONG_CHPH { get; set; }
        public Nullable<bool> IS_HOI_PHO_CHPH { get; set; }
        public Nullable<System.DateTime> NGAY_GUI_OTP { get; set; }
        public Nullable<short> OTP_COUNTER { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHAM_AN> CHAM_AN { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHUYEN_CAN> CHUYEN_CAN { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DANG_KY_AN> DANG_KY_AN { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DANH_GIA_DINH_KY_MON_TH> DANH_GIA_DINH_KY_MON_TH { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DANH_GIA_DINH_KY_TH> DANH_GIA_DINH_KY_TH { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DIEM_CHI_TIET> DIEM_CHI_TIET { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DIEM_TONG_KET> DIEM_TONG_KET { get; set; }
        public virtual DM_DAN_TOC DM_DAN_TOC { get; set; }
        public virtual DM_DOI_TUONG_CHINH_SACH DM_DOI_TUONG_CHINH_SACH { get; set; }
        public virtual DM_KHU_VUC DM_KHU_VUC { get; set; }
        public virtual DM_QUAN_HUYEN DM_QUAN_HUYEN { get; set; }
        public virtual DM_QUOC_TICH DM_QUOC_TICH { get; set; }
        public virtual DM_TINH_THANH DM_TINH_THANH { get; set; }
        public virtual DM_XA_PHUONG DM_XA_PHUONG { get; set; }
        public virtual GIOI_TINH GIOI_TINH { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HOC_PHI_PHIEU_THU_HOC_SINH> HOC_PHI_PHIEU_THU_HOC_SINH { get; set; }
        public virtual KHOI KHOI { get; set; }
        public virtual TRUONG TRUONG { get; set; }
        public virtual LOP LOP { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NHAN_XET_HANG_NGAY> NHAN_XET_HANG_NGAY { get; set; }
    }
}
