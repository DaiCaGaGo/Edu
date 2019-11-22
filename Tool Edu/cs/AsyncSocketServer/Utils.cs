using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using NLog;

namespace AsyncSocketServer
{
    internal class Utilities
    {
        private readonly string _connectionstring;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        //string connectionstringLocal;
        public Utilities()
        {
            //connectionstring = "Data Source=103.1.210.1;Initial Catalog=SMS_Partners;User Id=sms_partner;Password=smspartners2@@11;";
            _connectionstring = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        }

        public bool Connect(SqlConnection con)
        {
            if (con.State == ConnectionState.Open)
                return true;
            try
            {
                con.Open(); return true;
            }
            catch { return false; }
        }

        public string MaHoaMatKhau(string originalPassword)
        {
            //Declarations
            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
            MD5 md5 = new MD5CryptoServiceProvider();
            var originalBytes = Encoding.Default.GetBytes(originalPassword);
            var encodedBytes = md5.ComputeHash(originalBytes);
            //Convert encoded bytes back to a 'readable' string
            return BitConverter.ToString(encodedBytes).Replace("-", "");
        }

        public string GetMessageFromCodeNum(int i)
        {
            switch (i)
            {
                case -1:
                    return "Error:IP cua doi tac da bi lock.";

                case -2:
                    return "Error:IP cua doi tac khong ton tai.";

                case -3:
                    return "Error:Tai khoan khong hop le.";

                case -4:
                    return "Error:Content overflow.";

                case -5:
                    return "Error:Loop tin nhan.";

                case -6:
                    return "Error:Brandname not found.";

                case -7:
                    return "Error:Telco not support.";

                case -8:
                    return "Alert:Khong gui tin 10' gan day.";

                case -98:
                    return "Error:Insert SMS fail.";

                case -99:
                    return "Error:Cannot connect to DB.";

                case -100:
                    return "Error:Loi xu ly command.";

                case -101:
                    return "Error:Mat ket noi voi doi tac.";

                default:
                    return "Ket smpp noi thanh cong.";
            }
        }

        public string SendMail(int code, object objemail, string parames)
        {
            string log = "";
            try
            {
                if (objemail != null && objemail.ToString() != "")
                {
                    var smtpClient = new SmtpClient("mail.1sms.vn");
                    string[] arr = objemail.ToString().Split(',');
                    foreach (string em in arr)
                    {
                        string rtcode = GetMessageFromCodeNum(code);
                        try
                        {
                            if (em != "")
                                smtpClient.SendAsync("admin@1sms.vn", em, "Canh bao ket noi ONESMS SMS Gateway, SMPP protocol:" + rtcode,
                                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " : " + rtcode + "\n" + parames, new object());
                        }
                        catch (Exception ex1)
                        {
                            log += "error email:" + em + "," + ex1.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log += ex.Message;
            }

            return log;
        }

        public string SendMail2SystemId(int code, string systemid, string parames)
        {
            string log = "";
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionstring))
                {
                    if (Connect(con))
                    {
                        using (SqlCommand com = new SqlCommand() { Connection = con })
                        {
                            com.CommandTimeout = 3000;
                            com.CommandText = "GetPartnerEmail";
                            com.CommandType = CommandType.StoredProcedure;
                            com.Parameters.AddWithValue("partnerusername", systemid);
                            object objemail = com.ExecuteScalar();
                            if (objemail != null && objemail.ToString() != "")
                            {
                                var smtpClient = new SmtpClient("mail.1sms.vn");
                                string[] arr = objemail.ToString().Split(',');
                                foreach (string em in arr)
                                {
                                    string rtcode = GetMessageFromCodeNum(code);
                                    try
                                    {
                                        if (em != "")
                                            smtpClient.SendAsync("admin@1sms.vn", em, "Canh bao ket noi ONESMS SMS Gateway, SMPP protocol:" + rtcode,
                                                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " : " + rtcode + "\n" + parames, new object());
                                    }
                                    catch (Exception ex1)
                                    {
                                        log += "error email:" + em + "," + ex1.Message;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log += ex.Message;
            }
            return log;
        }

        public string CheckLongtimeSendMsg()
        {
            string log = "";
            string[] arrAlertPartner = new string[] { "HDBankAPIOneSMS", "sapcom" };
            using (SqlConnection con = new SqlConnection(_connectionstring))
            {
                DataTable tblPartnerIDsNotSendAfter10Mi = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("select PartnerUser from Partners where PartnerID in(select Distinct(PartnerId) from Partner_SMS where TimeSend between DateAdd(MI,-10,Getdate()) and Getdate())", con);
                da.Fill(tblPartnerIDsNotSendAfter10Mi);
                foreach (string u in arrAlertPartner)
                {
                    if (tblPartnerIDsNotSendAfter10Mi.Select("PartnerUser='" + u + "'").Length <= 0)
                    {
                        log += SendMail2SystemId(-8, u, "Tai khoan:" + u);
                    }
                }
            }
            return log;
        }

        public int CheckPartnerBind(string user, string pass, string partnerIp)
        {
            string parames = user + partnerIp;
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionstring))
                {
                    if (Connect(con))
                    {
                        using (SqlCommand com = new SqlCommand() { Connection = con, CommandTimeout = 300 })
                        {
                            com.CommandText = "GetPartnerEmail";
                            com.CommandType = CommandType.StoredProcedure;
                            com.Parameters.AddWithValue("partnerusername", user);
                            object objemail = com.ExecuteScalar();

                            com.CommandText = ("SP_CheckIpExist");
                            com.CommandType = CommandType.StoredProcedure; com.Parameters.Clear();
                            com.Parameters.Add(new SqlParameter("@Partner_IP", partnerIp));
                            com.ExecuteNonQuery();

                            com.CommandText = ("SP_CheckIpLocked"); com.Parameters.Clear();
                            com.CommandType = CommandType.StoredProcedure;
                            com.Parameters.Add(new SqlParameter("@Partner_IP", partnerIp));
                            if (com.ExecuteScalar().ToString() == "true")
                            {
                                SendMail(-1, objemail, parames);
                                _logger.Log(LogLevel.Error, "user: " + user + " - pass: " + pass + " - partnerIP: " + partnerIp + " - ip locked");
                                return -1; //ip locked;
                            }

                            com.CommandText = ("SP_CheckPartnerIP"); com.Parameters.Clear();
                            com.CommandType = CommandType.StoredProcedure;
                            com.Parameters.Add(new SqlParameter("@IP", partnerIp));
                            com.Parameters.Add(new SqlParameter("@PartnerUser", user));
                            if (Convert.ToInt32(com.ExecuteScalar().ToString()) == 0)
                            {
                                SendMail(-2, objemail, parames);
                                _logger.Log(LogLevel.Error, "user: " + user + " - pass: " + pass + " - partnerIP: " + partnerIp + " - ip not found");
                                return -2; //ip not found;
                            }

                            com.Parameters.Clear(); com.CommandType = CommandType.StoredProcedure;
                            com.CommandText = ("SP_CheckLoginAPIreturnID");
                            com.Parameters.Add(new SqlParameter("@PartnerUser", user));
                            com.Parameters.Add(new SqlParameter("@PartnerPassAPI", MaHoaMatKhau(pass)));
                            com.Parameters.Add(new SqlParameter("@Partner_IP", partnerIp));
                            object obj = com.ExecuteScalar();
                            if (obj != null && obj.ToString() != "")
                            {
                                SendMail(1, objemail, parames);
                                return int.Parse(obj.ToString());
                            }
                            SendMail(-3, objemail, parames);
                            _logger.Log(LogLevel.Error, "user: " + user + " - pass: " + pass + " - partnerIP: " + partnerIp + " - user not found");
                            return -3; //user not found;
                        }
                    }
                    SendMail(-99, "tienlm@onesms.vn", parames);
                    _logger.Log(LogLevel.Error, "user: " + user + " - pass: " + pass + " - partnerIP: " + partnerIp + " - cannot connect to database");
                    return -99;//cannot connect to database.
                }
            }
            catch (Exception ex)
            {
                SendMail(-100, "tienlm@onesms.vn", parames + "|BindErrrorMessage" + ex.Message);
                _logger.Log(LogLevel.Error, "user: " + user + " - pass: " + pass + " - partnerIP: " + partnerIp + " - error: " + ex.Message);
                return -100;
            }
        }

        // ReSharper disable once FunctionComplexityOverflow
        public string PartnerSmsInsert(int partnerid, string partneruser, string sendername, string phone, string sms, string telco, bool isunicode, bool isflash)
        {
            string parames = "";
            try
            {
              

               parames = partnerid + "|" + partneruser + "|" + sendername + "|" + phone + "|" + sms + "|" + telco + "|" + isunicode + "|" + isflash;
                using (SqlConnection con = new SqlConnection(_connectionstring))
                {
                    _logger.Log(LogLevel.Error, phone + ":2 ");
                    if (Connect(con))
                    {
                        using (SqlCommand com = new SqlCommand() { Connection = con, CommandTimeout = 300, CommandType = CommandType.StoredProcedure })
                        {
                            
                            com.CommandText = "SPr_CheckLoopBrandPort_New";
                            com.Parameters.AddWithValue("phone", phone);
                            com.Parameters.AddWithValue("sms", sms);
                            com.Parameters.AddWithValue("partnerid", partnerid);
                            com.Parameters.AddWithValue("sendername", sendername);
                            com.Parameters.Add("@TrueSenderName", SqlDbType.NVarChar, 11); com.Parameters["@TrueSenderName"].Direction = ParameterDirection.Output;
                            com.Parameters.Add("@Ports", SqlDbType.NVarChar, 100); com.Parameters["@Ports"].Direction = ParameterDirection.Output;
                            com.Parameters.Add("@IsLoop", SqlDbType.Bit); com.Parameters["@IsLoop"].Direction = ParameterDirection.Output;
                            com.ExecuteNonQuery();
                            
                            string trueSenderName;
                            if (com.Parameters["@TrueSenderName"].Value.ToString() != "")
                                trueSenderName = com.Parameters["@TrueSenderName"].Value.ToString();
                            else
                            {
                                _logger.Log(LogLevel.Error, "partnerid: "
                                + partnerid
                                + " - partneruser: " + partneruser
                                + " - sendername: " + sendername
                                + " - phone: " + phone
                                + " - sms: " + sms
                                + " - sendername: " + sendername
                                + " - Telco: " + telco
                                + " - isunicode: " + isunicode
                                + " - isflash: " + isflash
                                + " - brandname not found");

                                return "-6";//brandname not found
                            }
                           
                            if (com.Parameters["@IsLoop"].Value.ToString().ToUpper() == "TRUE" || com.Parameters["@IsLoop"].Value.ToString() == "1")
                            {
                                SendMail2SystemId(-5, partneruser, parames);

                                _logger.Log(LogLevel.Error, "partnerid: "
                                + partnerid
                                + " - partneruser: " + partneruser
                                + " - sendername: " + sendername
                                + " - phone: " + phone
                                + " - sms: " + sms
                                + " - sendername: " + sendername
                                + " - Telco: " + telco
                                + " - isunicode: " + isunicode
                                + " - isflash: " + isflash
                                + " - sms loop");

                                return "-5";//sms loop
                            }
                            
                            string[] arrPorts = com.Parameters["@Ports"].Value.ToString().Split('|');
                            string portViettel = arrPorts[0];
                            string portEvn = arrPorts[5];
                            string portGpc = arrPorts[3];
                            string portVms = arrPorts[2];
                            string smsqc = sms;
                            bool isApproved = true;
                            
                            if (string.IsNullOrEmpty(telco))
                            {
                                _logger.Log(LogLevel.Error, "partnerid: "
                                + partnerid
                                + " - partneruser: " + partneruser
                                + " - sendername: " + sendername
                                + " - phone: " + phone
                                + " - sms: " + sms
                                + " - sendername: " + sendername
                                + " - Telco: " + telco
                                + " - isunicode: " + isunicode
                                + " - isflash: " + isflash
                                + " - not support telco");

                                return "-7";//not support telco
                            }
                            _logger.Log(LogLevel.Error, phone + ":8 ");
                            if ((telco.Equals("Viettel") && portViettel.Contains("AMS")) || (telco.Equals("EVN") && portEvn.Contains("AMS")))
                            {
                                if (!smsqc.Contains("(QC-VTL2) ") && !smsqc.Contains("De tu choi, soan TC gui 1313"))
                                    smsqc = "(QC-VTL2) " + sms + "De tu choi, soan TC gui 1313";
                                isApproved = false;
                            }
                            else if (telco == "GPC" && portGpc.Contains("AMS"))
                            {
                                smsqc = "(QC VMA2) " + sms + " Tu choi QC soan TC gui 1551";
                                isApproved = false;
                            }
                            else if (telco == "VMS" && portVms.Contains("AMS"))
                            {
                                smsqc = "(VIV2) " + sms + " Tu choi QC,soan NO gui 9241";
                                isApproved = false;
                            }
                           
                            if ((smsqc.Length > 459 && isunicode == false) || (smsqc.Length > 201 && isunicode) || sms.Length < 1)
                            {
                                _logger.Log(LogLevel.Error, "partnerid: "
                                + partnerid
                                + " - partneruser: " + partneruser
                                + " - sendername: " + sendername
                                + " - phone: " + phone
                                + " - sms: " + sms
                                + " - sendername: " + sendername
                                + " - Telco: " + telco
                                + " - isunicode: " + isunicode
                                + " - isflash: " + isflash
                                + " - content overflow");

                                return "-4";//content overflow
                            }
                           
                            com.Parameters.Clear();
                            com.CommandText = ("SP_Partner_sms_Insert");
                            com.Parameters.Add(new SqlParameter("@PartnerUser", partneruser));
                            com.Parameters.Add(new SqlParameter("@SenderName", trueSenderName));
                            com.Parameters.Add(new SqlParameter("@Sms", smsqc));
                            com.Parameters.Add(new SqlParameter("@isUnicode", isunicode));
                            com.Parameters.Add(new SqlParameter("@isFlash", isflash));
                            com.Parameters.Add(new SqlParameter("@countmes", CountMess(smsqc, isunicode)));
                            com.Parameters.Add(new SqlParameter("@phone", phone));
                            com.Parameters.Add(new SqlParameter("@isHot", CheckHot(sms)));
                            com.Parameters.Add(new SqlParameter("@isApproved", isApproved));
                            object id = com.ExecuteScalar();
                            
                            if (id != null && id.ToString() != "")
                            {
                                _logger.Log(LogLevel.Info, "partnerid: "
                                + partnerid
                                + " - partneruser: " + partneruser
                                + " - sendername: " + sendername
                                + " - phone: " + phone
                                + " - sms: " + sms
                                + " - sendername: " + sendername
                                + " - Telco: " + telco
                                + " - isunicode: " + isunicode
                                + " - isflash: " + isflash
                                + " - idSmsReturn: " + id
                                + " - Success!");
                                if (sendername.ToUpper()=="HDBANK")
                                {
                                    //try
                                    //{
                                    //    Console.WriteLine("Time: " + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff",
                                    //System.Globalization.CultureInfo.InvariantCulture) + " - sendername: " + sendername + " - phone: " + phone + " - sms: " + sms);
                                    //}
                                    //catch { }
                                }
                                return id.ToString();
                            }
                            
                            _logger.Log(LogLevel.Error, "partnerid: "
                                                        + partnerid
                                                        + " - partneruser: " + partneruser
                                                        + " - sendername: " + sendername
                                                        + " - phone: " + phone
                                                        + " - sms: " + sms
                                                        + " - sendername: " + sendername
                                                        + " - Telco: " + telco
                                                        + " - isunicode: " + isunicode
                                                        + " - isflash: " + isflash
                                                        + " - insert fail");
                            return "-98";//insert fail
                        }
                    }
                    _logger.Log(LogLevel.Error, phone + ":13 ");
                    LogApp("-99: " + parames);
                    
                    _logger.Log(LogLevel.Error, "partnerid: "
                                                + partnerid
                                                + " - partneruser: " + partneruser
                                                + " - sendername: " + sendername
                                                + " - phone: " + phone
                                                + " - sms: " + sms
                                                + " - sendername: " + sendername
                                                + " - Telco: " + telco
                                                + " - isunicode: " + isunicode
                                                + " - isflash: " + isflash
                                                + " - cannot connect to database");
                    return "-99";//cannot connect to database.
                }
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();

                LogApp("-100: " + parames + "|" + ex.Message);
                _logger.Log(LogLevel.Error, "partnerid: "
                    + partnerid
                    + " - partneruser: " + partneruser
                    + " - sendername: " + sendername
                    + " - phone: " + phone
                    + " - sms: " + sms
                    + " - sendername: " + sendername
                    + " - Telco: " + telco
                    + " - isunicode: " + isunicode
                    + " - isflash: " + isflash
                    + " - error: " + ex.Message
                    + " - Detail: " + ex.ToString()
                    +" - line "+ line);
                return "-100"; /*proccess fail*/
            }
        }

        private bool CheckHot(string sms)
        {
            if (sms.Contains("OTP") || sms.Contains("SO DU") || sms.Contains("Mat khau") || sms.Contains("Ma giao dich")
                || sms.Contains("Key") || sms.Contains("Code"))
                return true;
            return false;
        }

        public static int CountChar(string mes)
        {
            int meslen = 0;
            char[] charExten = { '\\', '\f', '^', '{', '}', '[', ']', '~', '|', '€' };
            foreach (char ch in mes)
            {
                int c = 1;
                for (int i = 0; i < charExten.Length; i++)
                {
                    if (charExten[i].Equals(ch))
                    {
                        c = 2; break;
                    }
                }
                meslen += c;
            }
            return meslen;
        }

        public static byte CountMess(string mes, bool isUnicode)
        {
            int meslen = CountChar(mes);
            byte countmes;
            if (isUnicode)
            {
                if (meslen <= 70) { countmes = 1; }
                else if (meslen > 70 && meslen <= 134) { countmes = 2; }
                else if (meslen > 134 && meslen <= 201) { countmes = 3; }
                else countmes = 4;
            }
            else
            {
                if (meslen <= 160) { countmes = 1; }
                else if (meslen > 160 && meslen <= 306) { countmes = 2; }
                else if (meslen > 306 && meslen <= 459) { countmes = 3; }
                else countmes = 4;
            }
            return countmes;
        }

        public string GetTelco(string phone)
        {
            if (phone.StartsWith("841") && phone.Length != 12)
            {
                return "";
            }
            if (phone.StartsWith("849") && phone.Length != 11)
            {
                return "";
            }
            string Telco = "";
            string a = "";
            string b = "";
            if (phone.Length == 11 || phone.Length == 12)
            {
                a = phone.Substring(0, 4);
                b = phone.Substring(0, 5);
            }
            if (phone.Length != 11 && phone.Length != 12)
                Telco = "";
            // VMS
            else if (a == "8490" || a == "8493" || a == "8489" || b == "84120" || b == "84121" || b == "84122" || b == "84126" || b == "84128")
                Telco = "VMS";
            // GPC
            else if (a == "8491" || a == "8494" || a == "8488" || b == "84123" || b == "84124" || b == "84125" || b == "84127" || b == "84129")
                Telco = "GPC";
            // Viettel
            else if (a == "8497" || a == "8498" || a == "8486" || b == "84161" || b == "84162" || b == "84163" || b == "84164" || b == "84165" || b == "84166" || b == "84167" || b == "84168" || b == "84169")
                Telco = "Viettel";
            // VNM
            else if (a == "8492" || b == "84188" || b == "84186")
                Telco = "VNM";
            // SFone
            else if (a == "8495" || b == "84155")
                Telco = "Sfone";
            // EVN
            else if (a == "8496")
                Telco = "Viettel";
            // Gtel
            else if (a == "8499" || b == "84199")
                Telco = "Gtel";

            else if (a == "8452")
                Telco = "VNM";
            else if (a == "8484" || a == "8481" || a == "8482" || a == "8483" || a == "8485")
                Telco = "GPC";
            else if (a == "8470" || a == "8479" || a == "8477" || a == "8476" || a == "8478")
                Telco = "VMS";
            else if (a == "8439" || a == "8438" || a == "8437" || a == "8436" || a == "8435"
                || a == "8434" || a == "8433" || a == "8432")
                Telco = "Viettel";


            else if (a == "8459" || b == "84592" || b == "84593" || b == "84598" || b == "84599")
                Telco = "Gtel";
            else if (a == "8456" || a == "8458")
                Telco = "VNM";
            else
                Telco = "";
            return Telco;



        }

        public string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^ a-zA-Z0-9_!@#$*&?%'~`€()-+={}|\\\\;:\",./<>?\\[\\]\n@£$¥èéùìòÇØøÅå_ÆæßÉ!#¤%&'()*+,-./:;<=>?¡ÄÖÑÜ§¿äöñüà^]+", "", RegexOptions.Compiled);
        }

        private void LogApp(string mes)
        {
            try
            {
                using (System.IO.StreamWriter fstr = new System.IO.StreamWriter(String.Format("SmscLog_Error{0}.txt", DateTime.Now.ToShortDateString().Replace("/", "-")), true))
                {
                    fstr.WriteLine("{0}: {1}", DateTime.Now, mes);
                }
            }
            catch
            {
                //
            }
        }
    }
}