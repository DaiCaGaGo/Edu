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
    
    public partial class DM_DON_VI_TINH
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DM_DON_VI_TINH()
        {
            this.KHO_THUC_PHAM = new HashSet<KHO_THUC_PHAM>();
            this.PHIEU_NHAP_KHO_DETAIL = new HashSet<PHIEU_NHAP_KHO_DETAIL>();
            this.PHIEU_XUAT_KHO_DETAIL = new HashSet<PHIEU_XUAT_KHO_DETAIL>();
        }
    
        public short ID { get; set; }
        public string TEN { get; set; }
        public Nullable<short> THU_TU { get; set; }
        public Nullable<bool> IS_DELETE { get; set; }
        public Nullable<System.DateTime> NGAY_TAO { get; set; }
        public Nullable<long> NGUOI_TAO { get; set; }
        public Nullable<System.DateTime> NGAY_SUA { get; set; }
        public Nullable<long> NGUOI_SUA { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KHO_THUC_PHAM> KHO_THUC_PHAM { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PHIEU_NHAP_KHO_DETAIL> PHIEU_NHAP_KHO_DETAIL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PHIEU_XUAT_KHO_DETAIL> PHIEU_XUAT_KHO_DETAIL { get; set; }
    }
}
