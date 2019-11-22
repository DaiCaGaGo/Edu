using System;

namespace Entity
{
    public class SmsStatus
    {
        public long sms_id { get; set; }
        public int partner_id { get; set; }
        public string sender_name { get; set; }
        public string sms_content { get; set; }
        public bool is_unicode { get; set; }
        public bool is_flash { get; set; }
        public int count_mes { get; set; }
        public string port_name { get; set; }
        public string phone { get; set; }
        public string tel_code { get; set; }
        public DateTime time_send { get; set; }
        public DateTime time_reveive { get; set; }
        public string sent_code { get; set; }
        public long customer_sms_id { set; get; }
    }
}