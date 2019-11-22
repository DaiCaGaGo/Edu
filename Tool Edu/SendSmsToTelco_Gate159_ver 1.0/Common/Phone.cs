namespace Common
{
    public class Phone
    {
        public string GetTelco(string phone)
        {
            var telco = "";
            var a = "";
            var b = "";

            if (phone.Length == 11 || phone.Length == 12)
            {
                a = phone.Substring(0, 4);
                b = phone.Substring(0, 5);
            }
            if (phone.Length != 11 && phone.Length != 12)
                telco = "";
            // VMS
            else if (a == "8490" || a == "8493" || a == "8489" || b == "84120" || b == "84121" || b == "84122" || b == "84126" || b == "84128")
                telco = "VMS";
            // GPC
            else if (a == "8491" || a == "8494" || a == "8488" || b == "84123" || b == "84124" || b == "84125" || b == "84127" || b == "84129")
                telco = "GPC";
            // Viettel
            else if (a == "8497" || a == "8498" || a == "8486" || b == "84161" || b == "84162" || b == "84163" || b == "84164" || b == "84165" || b == "84166" || b == "84167" || b == "84168" || b == "84169")
                telco = "Viettel";
            // VNM
            else if (a == "8492" || b == "84188" || b == "84186")
                telco = "VNM";
            // SFone
            else if (a == "8495" || b == "84155")
                telco = "Sfone";
            // EVN
            else if (a == "8496")
                telco = "Viettel";
            else if (a == "8452" || a == "8458" || a == "8456")
                telco = "VNM"; 

            else if (a == "8499" || a == "8459" || b == "84199")
                telco = "Gtel";
            else if (a == "8452")
                telco = "VNM";
            else if (a == "8484" || a == "8481" || a == "8482" || a == "8483" || a == "8485")
                telco = "GPC";
            else if (a == "8470" || a == "8479" || a == "8477" || a == "8476" || a == "8478")
                telco = "VMS";
            else if (a == "8439" || a == "8438" || a == "8437" || a == "8436" || a == "8435"
                || a == "8434" || a == "8433" || a == "8432")
                telco = "Viettel";
            else
                telco = "";
            return telco;
        }
    }
}