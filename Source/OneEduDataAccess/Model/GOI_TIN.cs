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
    
    public partial class GOI_TIN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GOI_TIN()
        {
            this.QUY_TIN = new HashSet<QUY_TIN>();
            this.TIN_NHAN = new HashSet<TIN_NHAN>();
            this.TRUONGs = new HashSet<TRUONG>();
        }
    
        public short MA { get; set; }
        public string GHI_CHU { get; set; }
        public Nullable<long> SO_TIN_LIEN_LAC_HS { get; set; }
        public Nullable<short> TRANG_THAI { get; set; }
        public Nullable<System.DateTime> NGAY_TAO { get; set; }
        public Nullable<long> NGUOI_TAO { get; set; }
        public Nullable<System.DateTime> NGAY_SUA { get; set; }
        public Nullable<long> NGUOI_SUA { get; set; }
        public Nullable<int> THU_TU { get; set; }
        public Nullable<bool> IS_DELETE { get; set; }
        public Nullable<long> SO_TIN_THONG_BAO_HS { get; set; }
        public Nullable<long> SO_TIN_HE_HS { get; set; }
        public string TEN { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QUY_TIN> QUY_TIN { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TIN_NHAN> TIN_NHAN { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TRUONG> TRUONGs { get; set; }
    }
}
