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
    
    public partial class QUOTAREMAINBYDAY
    {
        public long ACCOUNT_ID { get; set; }
        public string LOGIN_NAME { get; set; }
        public string SENDER_NAME { get; set; }
        public Nullable<long> QUOTA_QC_REMAIN { get; set; }
        public Nullable<long> QUOTA_CSKH_REMAIN { get; set; }
        public Nullable<long> QUOTA_CSKH_USE { get; set; }
        public Nullable<long> QUOTA_QC_USE { get; set; }
        public string SMS_TYPE { get; set; }
        public string TIME { get; set; }
        public string RESPONSED_CODE { get; set; }
        public string CREATE_DATE { get; set; }
    }
}
