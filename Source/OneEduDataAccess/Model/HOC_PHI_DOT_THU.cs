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
    
    public partial class HOC_PHI_DOT_THU
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HOC_PHI_DOT_THU()
        {
            this.HOC_PHI_DOT_THU_LOP = new HashSet<HOC_PHI_DOT_THU_LOP>();
            this.HOC_PHI_PHIEU_THU_HOC_SINH = new HashSet<HOC_PHI_PHIEU_THU_HOC_SINH>();
        }
    
        public long ID { get; set; }
        public long ID_TRUONG { get; set; }
        public short ID_NAM_HOC { get; set; }
        public string TEN { get; set; }
        public string GHI_CHU { get; set; }
        public Nullable<bool> IS_TIEN_AN { get; set; }
        public Nullable<long> TONG_TIEN { get; set; }
        public Nullable<System.DateTime> THOI_GIAN_BAT_DAU { get; set; }
        public Nullable<System.DateTime> THOI_GIAN_KET_THUC { get; set; }
        public Nullable<short> ID_DOT_THU { get; set; }
        public Nullable<short> HOC_KY { get; set; }
        public Nullable<short> THANG { get; set; }
        public Nullable<bool> IS_DELETE { get; set; }
        public Nullable<System.DateTime> NGAY_TAO { get; set; }
        public Nullable<long> NGUOI_TAO { get; set; }
        public Nullable<System.DateTime> NGAY_SUA { get; set; }
        public Nullable<long> NGUOI_SUA { get; set; }
        public Nullable<long> SO_TIEN_AN { get; set; }
        public Nullable<long> THU_TU { get; set; }
    
        public virtual TRUONG TRUONG { get; set; }
        public virtual NAM_HOC NAM_HOC { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HOC_PHI_DOT_THU_LOP> HOC_PHI_DOT_THU_LOP { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HOC_PHI_PHIEU_THU_HOC_SINH> HOC_PHI_PHIEU_THU_HOC_SINH { get; set; }
    }
}
