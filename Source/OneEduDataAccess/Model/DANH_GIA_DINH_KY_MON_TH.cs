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
    
    public partial class DANH_GIA_DINH_KY_MON_TH
    {
        public long ID { get; set; }
        public long ID_TRUONG { get; set; }
        public long ID_LOP { get; set; }
        public long ID_HOC_SINH { get; set; }
        public long ID_MON_TRUONG { get; set; }
        public short MA_KY_DG { get; set; }
        public string MA_NX { get; set; }
        public string NOI_DUNG_NX { get; set; }
        public Nullable<short> KTDK { get; set; }
        public Nullable<long> NGUOI_TAO { get; set; }
        public Nullable<System.DateTime> NGAY_TAO { get; set; }
        public Nullable<long> NGUOI_SUA { get; set; }
        public Nullable<System.DateTime> NGAY_SUA { get; set; }
        public Nullable<bool> IS_DELETE { get; set; }
        public Nullable<short> MA_NAM_HOC { get; set; }
        public Nullable<short> MA_KHOI { get; set; }
        public string MUC { get; set; }
    
        public virtual LOP LOP { get; set; }
        public virtual HOC_SINH HOC_SINH { get; set; }
        public virtual MON_HOC_TRUONG MON_HOC_TRUONG { get; set; }
    }
}
