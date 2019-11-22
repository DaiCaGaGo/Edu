using System;

namespace Entity
{
    public class PartnerSms
    {
        public long SmsId { get; set; }
        public int PartnerId { get; set; }
        public string SenderName { get; set; }
        public string Sms { get; set; }
        public bool IsUnicode { get; set; }
        public bool IsFlash { get; set; }
        public DateTime TimePost { get; set; }
        public bool IsHot { get; set; }
        public DateTime TimeSend { get; set; }
        public int CountMes { get; set; }
        public string Phone { get; set; }
        public bool IsRead { get; set; }
        public long CustomerSmsId { get; set; }
        public bool IsApproved { get; set; }
        public string Viettel { get; set; }
        public string Vnm { get; set; }
        public string Vms { get; set; }
        public string Gpc { get; set; }
        public string Sfone { get; set; }
        public string Evn { get; set; }
        public string Gtel { get; set; }
        public string BrdViettel { get; set; }
        public string BrdVnm { get; set; }
        public string BrdGpc { get; set; }
        public string BrdSfone { get; set; }
        public string BrdEvn { get; set; }
        public string BrdVms { get; set; }
        public string BrdGtel { get; set; }
        public string sentcode { get; set; }
    }
}