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
    
    public partial class NHU_CAU_NANG_LUONG_TRUONG
    {
        public long ID { get; set; }
        public long ID_TRUONG { get; set; }
        public Nullable<decimal> ID_NHOM_TUOI_MN { get; set; }
        public Nullable<decimal> PROTID_TU { get; set; }
        public Nullable<decimal> PROTID_DEN { get; set; }
        public Nullable<decimal> LIPID_TU { get; set; }
        public Nullable<decimal> LIPID_DEN { get; set; }
        public Nullable<decimal> GLUCID_TU { get; set; }
        public Nullable<decimal> GLUCID_DEN { get; set; }
        public string GHI_CHU { get; set; }
        public Nullable<long> THU_TU { get; set; }
        public Nullable<bool> IS_DELETE { get; set; }
        public Nullable<System.DateTime> NGAY_TAO { get; set; }
        public Nullable<long> NGUOI_TAO { get; set; }
        public Nullable<System.DateTime> NGAY_SUA { get; set; }
        public Nullable<long> NGUOI_SUA { get; set; }
        public short ID_KHOI { get; set; }
    
        public virtual KHOI KHOI { get; set; }
        public virtual NHOM_TUOI_MN NHOM_TUOI_MN { get; set; }
        public virtual TRUONG TRUONG { get; set; }
    }
}
