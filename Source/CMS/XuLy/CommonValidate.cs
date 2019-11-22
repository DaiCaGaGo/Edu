using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace CMS.XuLy
{
    public class CommonValidate
    {
        private string Err01 = "Bắt buộc";
        private string Err02 = "Thành công";
        private string Err03 = "Không hợp lệ";
        private string Err04 = "Sai định dạng";
        public bool ValString(bool is_req, int? length, string value, out object val, out string Error)
        {
            val = value.Trim();
            Error = Err02;

            if (is_req && string.IsNullOrEmpty(value))
            {
                Error = Err01;
                return false;
            }
            if (!is_req && string.IsNullOrEmpty(value))
            {
                val = null;
                return true;
            }
            if (length != null && value.Length > length)
            {
                Error = "Không được vượt quá " + length + " ký tự";
                return false;
            }
            return true;
        }
        public bool ValInt(bool is_req, int? min, int? max, string value, out object val, out string Error)
        {
            val = value.Trim();
            Error = Err02;

            if (is_req && string.IsNullOrEmpty(value))
            {
                Error = Err01;
                return false;
            }
            if (!is_req && string.IsNullOrEmpty(value))
            {
                val = null;
                return true;
            }
            try
            {
                int countVal = value.Length;
                if ((min != null && countVal < min) || (max != null && countVal > max))
                {
                    Error = Err03;
                    return false;
                }
            }
            catch
            {

                Error = Err04;
                return false;
            }

            return true;
        }
        public bool ValBool(bool is_req, string value, out object val, out string Error)
        {
            val = value.Trim();
            Error = Err02;

            if (is_req && string.IsNullOrEmpty(value))
            {
                Error = Err01;
                return false;
            }
            if (!is_req && string.IsNullOrEmpty(value))
            {
                val = null;
                return true;
            }
            try
            {

                if (value.ToLower() == "true" || value == "1")
                    val = true;
                else if (value.ToLower() == "false" || value == "0")
                    val = false;
                else
                {
                    Error = Err04;
                    return false;
                }
            }
            catch
            {

                Error = Err04;
                return false;
            }

            return true;
        }
        public bool ValDecimal(bool is_req, Decimal? min, Decimal? max, string value, out object val, out string Error)
        {
            val = value.Trim();
            Error = Err02;

            if (is_req && string.IsNullOrEmpty(value))
            {
                Error = Err01;
                return false;
            }
            if (!is_req && string.IsNullOrEmpty(value))
            {
                val = null;
                return true;
            }
            try
            {
                if (value.Contains(","))
                {
                    Error = Err04;
                    return false;
                }
                val = Convert.ToDecimal(value);
                if ((min != null && (Decimal)val < min) || (max != null && (Decimal)val > max))
                {
                    Error = Err03;
                    return false;
                }
            }
            catch
            {
                Error = Err04;
                return false;
            }

            return true;
        }
        public bool ValDouble(bool is_req, double? min, double? max, string value, out object val, out string Error)
        {
            val = value.Trim();
            Error = Err02;

            if (is_req && string.IsNullOrEmpty(value))
            {
                Error = Err01;
                return false;
            }
            if (!is_req && string.IsNullOrEmpty(value))
            {
                val = null;
                return true;
            }
            try
            {
                if (value.Contains(","))
                {
                    Error = Err04;
                    return false;
                }
                val = Convert.ToDouble(value);
                if ((min != null && (double)val < min) || (max != null && (double)val > max))
                {
                    Error = Err03;
                    return false;
                }
            }
            catch
            {
                Error = Err04;
                return false;
            }

            return true;
        }
        public bool ValDate(bool is_req, DateTime? min, DateTime? max, string value, out object val, out string Error)
        {
            val = value.Trim();
            Error = Err02;

            if (is_req && string.IsNullOrEmpty(value))
            {
                Error = Err01;
                return false;
            }
            if (!is_req && string.IsNullOrEmpty(value))
            {
                val = null;
                return true;
            }
            try
            {
                DateTime dt = DateTime.ParseExact(value.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                val = dt;
                if ((min != null && dt.CompareTo(min) < 0) || (max != null && dt.CompareTo(max) < 0))
                {
                    Error = Err03;
                    return false;
                }
            }
            catch
            {
                Error = Err04;
                return false;
            }

            return true;
        }
        public bool ValDateTime(bool is_req, DateTime? min, DateTime? max, string value, out object val, out string Error)
        {
            val = value.Trim();
            Error = Err02;

            if (is_req && string.IsNullOrEmpty(value))
            {
                Error = Err01;
                return false;
            }
            if (!is_req && string.IsNullOrEmpty(value))
            {
                val = null;
                return true;
            }
            try
            {
                DateTime dt = DateTime.ParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                val = dt;
                if ((min != null && dt.CompareTo(min) < 0) || (max != null && dt.CompareTo(max) < 0))
                {
                    Error = Err03;
                    return false;
                }
            }
            catch
            {
                Error = Err04;
                return false;
            }

            return true;
        }
        public bool ValidateEmail(bool is_req, string value, out object val, out string Error)
        {
            val = value.Trim();
            Error = Err02;
            string regexPattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";

            if (is_req && string.IsNullOrEmpty(value))
            {
                Error = Err01;
                return false;
            }
            if (!is_req && string.IsNullOrEmpty(value))
            {
                val = null;
                return true;
            }

            Match matches = Regex.Match(value, regexPattern);
            if(!matches.Success)
            {
                Error = "Email không đúng định dạng";
                return false;
            }

            return true;
        }
        public bool ValidateSDT(bool is_req, string value, out string val, out string Error)
        {
            value = value.Trim();
            val = value;
            Error = Err02;
            if (is_req && string.IsNullOrEmpty(value))
            {
                Error = Err01;
                return false;
            }
            if (!is_req && string.IsNullOrEmpty(value))
            {
                val = null;
                return true;
            }
            if (!string.IsNullOrEmpty(value))
            {
                int count = value.Length;
                if (count < 9 && count > 12)
                {
                    Error = Err03;
                    return false;
                }
                else
                {
                    #region check 10 số đầu 0
                    if (count == 10 && value.IndexOf("0") == 0)
                    {
                        if (value.IndexOf("09") == 0 || value.IndexOf("08") == 0 || value.IndexOf("07") == 0 || value.IndexOf("05") == 0 || value.IndexOf("03") == 0)
                            val = "84" + value.Substring(1, count - 1);
                        else
                        {
                            val = null;
                            Error = Err03;
                            return false;
                        }
                    }
                    #endregion
                    #region check 10 số đầu 84
                    else if (count == 11 && value.IndexOf("84") == 0)
                    {
                        if (value.IndexOf("849") == 0 || value.IndexOf("848") == 0 || value.IndexOf("847") == 0 || value.IndexOf("845") == 0 || value.IndexOf("843") == 0)
                            val = value;
                        else
                        {
                            val = null;
                            Error = Err03;
                            return false;
                        }
                    }
                    #endregion
                    #region check 11 số đầu 0
                    else if (count == 11 && value.IndexOf("0") == 0)
                    {
                        #region check mạng viettel
                        if (value.IndexOf("0162") == 0 || value.IndexOf("0163") == 0 || value.IndexOf("0164") == 0 || 
                            value.IndexOf("0165") == 0 || value.IndexOf("0166") == 0 || value.IndexOf("0167") == 0 || 
                            value.IndexOf("0168") == 0 || value.IndexOf("0169") == 0)
                            val = "843" + value.Substring(3, count - 3);
                        #endregion
                        #region check mạng mobifone
                        else if (value.IndexOf("0120") == 0) val = "8470" + value.Substring(4, count - 4);
                        else if (value.IndexOf("0121") == 0) val = "8479" + value.Substring(4, count - 4);
                        else if (value.IndexOf("0122") == 0) val = "8477" + value.Substring(4, count - 4);
                        else if (value.IndexOf("0126") == 0) val = "8476" + value.Substring(4, count - 4);
                        else if (value.IndexOf("0128") == 0) val = "8478" + value.Substring(4, count - 4);
                        #endregion
                        #region check mạng vinaphone
                        else if (value.IndexOf("0123") == 0) val = "8483" + value.Substring(4, count - 4);
                        else if (value.IndexOf("0124") == 0) val = "8484" + value.Substring(4, count - 4);
                        else if (value.IndexOf("0125") == 0) val = "8485" + value.Substring(4, count - 4);
                        else if (value.IndexOf("0127") == 0) val = "8481" + value.Substring(4, count - 4);
                        else if (value.IndexOf("0129") == 0) val = "8482" + value.Substring(4, count - 4);
                        #endregion
                        #region check mạng vietnamMobile
                        else if (value.IndexOf("0186") == 0) val = "8456" + value.Substring(4, count - 4);
                        else if (value.IndexOf("0188") == 0) val = "8458" + value.Substring(4, count - 4);
                        #endregion
                        #region check mạng GMobile
                        else if (value.IndexOf("0199") == 0) val = "8459" + value.Substring(4, count - 4);
                        #endregion
                        else
                        {
                            val = null;
                            Error = Err03;
                            return false;
                        }
                    }
                    #endregion
                    #region check 11 số đầu 84
                    else if (count == 12 && value.IndexOf("84") == 0)
                    {
                        #region check mạng viettel
                        if (value.IndexOf("84162") == 0 || value.IndexOf("84163") == 0 || value.IndexOf("84164") == 0 ||
                            value.IndexOf("84165") == 0 || value.IndexOf("84166") == 0 || value.IndexOf("84167") == 0 ||
                            value.IndexOf("84168") == 0 || value.IndexOf("84169") == 0)
                            val = "843" + value.Substring(4, count - 4);
                        #endregion
                        #region check mạng mobifone
                        else if (value.IndexOf("84120") == 0) val = "8470" + value.Substring(5, count - 5);
                        else if (value.IndexOf("84121") == 0) val = "8479" + value.Substring(5, count - 5);
                        else if (value.IndexOf("84122") == 0) val = "8477" + value.Substring(5, count - 5);
                        else if (value.IndexOf("84126") == 0) val = "8476" + value.Substring(5, count - 5);
                        else if (value.IndexOf("84128") == 0) val = "8478" + value.Substring(5, count - 5);
                        #endregion
                        #region check mạng vinaphone
                        else if (value.IndexOf("84123") == 0) val = "8483" + value.Substring(5, count - 5);
                        else if (value.IndexOf("84124") == 0) val = "8484" + value.Substring(5, count - 5);
                        else if (value.IndexOf("84125") == 0) val = "8485" + value.Substring(5, count - 5);
                        else if (value.IndexOf("84127") == 0) val = "8481" + value.Substring(5, count - 5);
                        else if (value.IndexOf("84129") == 0) val = "8482" + value.Substring(5, count - 5);
                        #endregion
                        #region check mạng vietnamMobile
                        else if (value.IndexOf("84186") == 0) val = "8456" + value.Substring(5, count - 5);
                        else if (value.IndexOf("84188") == 0) val = "8458" + value.Substring(5, count - 5);
                        #endregion
                        #region check mạng GMobile
                        else if (value.IndexOf("84199") == 0) val = "8459" + value.Substring(5, count - 5);
                        #endregion
                        else
                        {
                            val = null;
                            Error = Err03;
                            return false;
                        }
                    }
                    #endregion
                    #region check 9 số mà thiếu số 0 ở đầu
                    else if (count == 9 && value.IndexOf("0") != 0)
                    {
                        val = "84" + value;
                    }
                    #endregion
                    else
                    {
                        val = null;
                        Error = Err03;
                        return false;
                    }
                }
            }
            return true;
        }
    }
}