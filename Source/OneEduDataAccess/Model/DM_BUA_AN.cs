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
    
    public partial class DM_BUA_AN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DM_BUA_AN()
        {
            this.CHAM_AN = new HashSet<CHAM_AN>();
            this.DANG_KY_AN = new HashSet<DANG_KY_AN>();
            this.LICH_AN = new HashSet<LICH_AN>();
            this.SUAT_AN = new HashSet<SUAT_AN>();
            this.THUC_DON = new HashSet<THUC_DON>();
        }
    
        public long ID { get; set; }
        public string TEN { get; set; }
        public Nullable<long> THU_TU { get; set; }
        public Nullable<bool> IS_DELETE { get; set; }
        public Nullable<System.DateTime> NGAY_TAO { get; set; }
        public Nullable<long> NGUOI_TAO { get; set; }
        public Nullable<System.DateTime> NGAY_SUA { get; set; }
        public Nullable<long> NGUOI_SUA { get; set; }
        public long ID_TRUONG { get; set; }
        public string GHI_CHU { get; set; }
        public Nullable<decimal> PROTID_TU { get; set; }
        public Nullable<decimal> PROTID_DEN { get; set; }
        public Nullable<decimal> LIPID_TU { get; set; }
        public Nullable<decimal> LIPID_DEN { get; set; }
        public Nullable<decimal> GLUCID_TU { get; set; }
        public Nullable<decimal> GLUCID_DEN { get; set; }
        public Nullable<decimal> ID_NHOM_TUOI_MN { get; set; }
        public Nullable<decimal> GIA_TIEN { get; set; }
        public short ID_KHOI { get; set; }
        public Nullable<decimal> NANG_LUONG_KCAL_TU { get; set; }
        public Nullable<decimal> NANG_LUONG_KCAL_DEN { get; set; }
        public Nullable<decimal> NANG_LUONG_TU_KCAL { get; set; }
        public Nullable<decimal> NANG_LUONG_DEN_KCAL { get; set; }
        public Nullable<decimal> PROTID_TU_KCAL { get; set; }
        public Nullable<decimal> PROTID_DEN_KCAL { get; set; }
        public Nullable<decimal> LIPID_TU_KCAL { get; set; }
        public Nullable<decimal> LIPID_DEN_KCAL { get; set; }
        public Nullable<decimal> GLUCID_TU_KCAL { get; set; }
        public Nullable<decimal> GLUCID_DEN_KCAL { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHAM_AN> CHAM_AN { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DANG_KY_AN> DANG_KY_AN { get; set; }
        public virtual TRUONG TRUONG { get; set; }
        public virtual KHOI KHOI { get; set; }
        public virtual NHOM_TUOI_MN NHOM_TUOI_MN { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LICH_AN> LICH_AN { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SUAT_AN> SUAT_AN { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<THUC_DON> THUC_DON { get; set; }
    }
}
