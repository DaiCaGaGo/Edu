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
    
    public partial class PHIEU_XUAT_KHO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PHIEU_XUAT_KHO()
        {
            this.PHIEU_XUAT_KHO_DETAIL = new HashSet<PHIEU_XUAT_KHO_DETAIL>();
        }
    
        public long ID { get; set; }
        public string MA_SO_PHIEU { get; set; }
        public System.DateTime NGAY_XUAT_KHO { get; set; }
        public Nullable<long> ID_NGUOI_XUAT_HANG { get; set; }
        public long ID_TRUONG { get; set; }
        public Nullable<bool> IS_DELETE { get; set; }
        public Nullable<System.DateTime> NGAY_TAO { get; set; }
        public Nullable<long> NGUOI_TAO { get; set; }
        public Nullable<System.DateTime> NGAY_SUA { get; set; }
        public Nullable<long> NGUOI_SUA { get; set; }
        public string GHI_CHU { get; set; }
        public Nullable<long> THU_TU { get; set; }
    
        public virtual GIAO_VIEN GIAO_VIEN { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PHIEU_XUAT_KHO_DETAIL> PHIEU_XUAT_KHO_DETAIL { get; set; }
        public virtual TRUONG TRUONG { get; set; }
    }
}
