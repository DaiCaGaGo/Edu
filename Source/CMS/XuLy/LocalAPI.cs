using ClosedXML.Excel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Telerik.Web.UI;

namespace CMS.XuLy
{
    public class LocalAPI
    {
        public string encryption(String password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encrypt;
            UTF8Encoding encode = new UTF8Encoding();
            //encrypt the given password string into Encrypted data  
            encrypt = md5.ComputeHash(encode.GetBytes(password));
            StringBuilder encryptdata = new StringBuilder();
            //Create a new string by using the encrypted data  
            for (int i = 0; i < encrypt.Length; i++)
            {
                encryptdata.Append(encrypt[i].ToString());
            }
            return encryptdata.ToString();
        }
        public string GetVerionCss()
        {
            #region Nhận dạng cấp đơn vị
            try
            {
                string verion = ConfigurationManager.AppSettings["VerionCss"];
                return verion;
            }
            catch { }
            return "1";
            #endregion
        }
        public string GetVerionJava()
        {
            #region Nhận dạng cấp đơn vị
            try
            {
                string verion = ConfigurationManager.AppSettings["VerionJava"];
                return verion;
            }
            catch { }
            return "1";
            #endregion
        }
        public short? ConvertStringToShort(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            try
            {
                short res = Convert.ToInt16(value);
                return res;
            }
            catch
            {
                return null;
            }
        }
        public int? ConvertStringToint(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            try
            {
                int res = Convert.ToInt32(value);
                return res;
            }
            catch
            {
                return null;
            }
        }
        public long? ConvertStringTolong(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            try
            {
                long res = Convert.ToInt64(value);
                return res;
            }
            catch
            {
                return null;
            }
        }
        public int GetYearNow()
        {
            #region Nhận dạng năm học, học kỳ

            int year = DateTime.Now.Year;
            int mon = DateTime.Now.Month;
            int dky2 = Convert.ToInt32(ConfigurationManager.AppSettings["ThangDauKy2"]),
                cky2 = Convert.ToInt32(ConfigurationManager.AppSettings["ThangCuoiKy2"]);
            if (mon <= cky2) year--;
            return year;

            #endregion
        }
        public int GetKyNow()
        {
            #region Nhận dạng năm học, học kỳ

            int ky = 1;
            int mon = DateTime.Now.Month;
            int dky2 = Convert.ToInt32(ConfigurationManager.AppSettings["ThangDauKy2"]),
                cky2 = Convert.ToInt32(ConfigurationManager.AppSettings["ThangCuoiKy2"]);
            if (mon <= cky2) ky = 2;
            else ky = 1;
            return ky;

            #endregion
        }
        public string GetSoKyTuMaHS()
        {
            #region Lấy số ký tự mã học sinh
            try
            {
                string soKyTuMaHS = ConfigurationManager.AppSettings["SoKyTuMaHS"];
                return soKyTuMaHS;
            }
            catch { }
            return "10";
            #endregion
        }
        public string SinhMaHS(string id_hs, short soKyTu)
        {
            string ma_hs = "";
            #region Lấy số ký tự mã học sinh
            try
            {

                soKyTu = Convert.ToInt16(GetSoKyTuMaHS());
                int count_soKyTu = soKyTu.ToString().Length;
                int count0 = 0;
                string str0 = "";
                if (count_soKyTu.ToString().Length < 10)
                {
                    count0 = 10 - count_soKyTu.ToString().Length;
                    for (int i = 0; i < count0; i++)
                    {
                        str0 += "0";
                    }
                }
                ma_hs = "HS" + str0 + id_hs;
                return ma_hs;
            }
            catch { }
            return ma_hs;
            #endregion
        }
        public int ExcelColumnNameToNumber(string columnName)
        {
            if (string.IsNullOrEmpty(columnName)) throw new ArgumentNullException("columnName");

            columnName = columnName.ToUpperInvariant();

            int sum = 0;

            for (int i = 0; i < columnName.Length; i++)
            {
                sum *= 26;
                sum += (columnName[i] - 'A' + 1);
            }

            return sum;
        }
        public void splitHoTen(string ho_ten, out string ho_dem, out string ten)
        {
            ho_dem = ""; ten = "";
            if (!string.IsNullOrEmpty(ho_ten))
            {
                if (ho_ten.Contains(" ")) ten = ho_ten.Substring(ho_ten.LastIndexOf(" "));
                else ten = ho_ten;
                int count_dem = ho_ten.Length - ten.Length;
                ho_dem = ho_ten.Substring(0, count_dem);
            }
        }
        public decimal? ConvertStringToDecimal(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            try
            {
                decimal res = Convert.ToDecimal(value);
                return res;
            }
            catch
            {
                return null;
            }
        }
        public int getSoTuanTrongThang(int thang, int nam)
        {
            DateTime ngayDauThang = new DateTime(nam, thang, 1);
            DateTime ngayCuoiThang = ngayDauThang.AddMonths(1).AddDays(-1);
            int thuHienTai = (int)ngayDauThang.DayOfWeek;
            int soChia = (ngayCuoiThang.Day + thuHienTai - 1) / 7;
            int soDu = (ngayCuoiThang.Day + thuHienTai - 1) % 7;
            int tuan_hien_tai = soChia;
            if (soDu > 0) tuan_hien_tai++;
            return tuan_hien_tai;
        }
        public string convertNgayTrongTuan(DateTime ngay, string first = "Thứ", string last = "Chủ nhật")
        {
            string thu = "";
            if (ngay.DayOfWeek.ToString() == "Monday") thu = first + " 2";
            if (ngay.DayOfWeek.ToString() == "Tuesday") thu = first + " 3";
            if (ngay.DayOfWeek.ToString() == "Wednesday") thu = first + " 4";
            if (ngay.DayOfWeek.ToString() == "Thursday") thu = first + " 5";
            if (ngay.DayOfWeek.ToString() == "Friday") thu = first + " 6";
            if (ngay.DayOfWeek.ToString() == "Saturday") thu = first + " 7";
            if (ngay.DayOfWeek.ToString() == "Sunday") thu = last;
            return thu;
        }
        public int getThisWeek()
        {
            //DateTime dt = new DateTime();
            //dt = DateTime.Now;
            //int thuHienTai = (int)dt.DayOfWeek;
            //int soChia = (dt.Day + thuHienTai - 1) / 7;
            //int soDu = (dt.Day + thuHienTai - 1) % 7;
            //int tuan_hien_tai = soChia;
            //if (soDu > 0) tuan_hien_tai++;
            //return tuan_hien_tai;
            DateTime dt = new DateTime();
            dt = DateTime.Now;
            int thuHienTai = (int)dt.DayOfWeek;
            int soChia = (dt.Day + (7 - (thuHienTai + 1))) / 7;
            int tuan_hien_tai = soChia + 1;
            return tuan_hien_tai;
        }
        private static readonly string[] VietnameseSigns = new string[] { "aAeEoOuUiIdDyY", "áàạảãâấầậẩẫăắằặẳẵ", "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ", "éèẹẻẽêếềệểễ", "ÉÈẸẺẼÊẾỀỆỂỄ", "óòọỏõôốồộổỗơớờợởỡ", "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ", "úùụủũưứừựửữ", "ÚÙỤỦŨƯỨỪỰỬỮ", "íìịỉĩ", "ÍÌỊỈĨ", "đ₫", "Đ", "ýỳỵỷỹ", "ÝỲỴỶỸ" };
        
        public string chuyenTiengVietKhongDau(string str)
        {
            if (str == null || str == "") return "";
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            str = RemoveSpecialCharacters(str);
            str = RemoveDiacritics(str);
            str = str.Replace("nbsp;", "");
            return str;
        }

        public string RemoveSpecialCharacters(string str)
        {
            str = str.Replace("–", "-")
                .Replace("‘", "'")
                .Replace("’", "'")
                .Replace("“", "\"")
                .Replace("”", "\"")
                .Replace("》", ">");
            return Regex.Replace(str, "[@#$%^&*`’]", "", RegexOptions.Compiled);
        }

        public string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.Normalize(NormalizationForm.FormD);
            var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC);
        }

        public string getTienTo(int? loai_chen_tin, string ho_ten, string ten, string tienTo, string noiDung)
        {
            if (loai_chen_tin == null) return noiDung;
            if (loai_chen_tin == 1) return ho_ten + " " + noiDung;
            if (loai_chen_tin == 2) return ten + " " + noiDung;
            if (loai_chen_tin == 3)
            {
                if (!string.IsNullOrEmpty(tienTo)) return tienTo + " " + noiDung;
                else return noiDung;
            }
            return noiDung;
        }
        public int demSoTin(string noi_dung)
        {
            if (noi_dung.Length < 161) return 1;
            else if (noi_dung.Length > 160 && noi_dung.Length < 307) return 2;
            else if (noi_dung.Length > 306 && noi_dung.Length < 460) return 3;
            else return 0;
        }
        public string Add84(string Phone)
        {
            try
            {
                if (string.IsNullOrEmpty(Phone))
                    return "";
                #region check 10 số đầu 0
                if (Phone.IndexOf("84") != 0 && Phone.Length == 10)
                    return "84" + Phone.Substring(1, Phone.Length - 1);
                #endregion
                #region check 11 số đầu 0
                else if (Phone.Length == 11 && Phone.IndexOf("0") == 0)
                {
                    #region check mạng viettel
                    if (Phone.IndexOf("0162") == 0 || Phone.IndexOf("0163") == 0 || Phone.IndexOf("0164") == 0 ||
                        Phone.IndexOf("0165") == 0 || Phone.IndexOf("0166") == 0 || Phone.IndexOf("0167") == 0 ||
                        Phone.IndexOf("0168") == 0 || Phone.IndexOf("0169") == 0)
                        return "843" + Phone.Substring(3, Phone.Length - 3);
                    #endregion
                    #region check mạng mobifone
                    else if (Phone.IndexOf("0120") == 0) return "8470" + Phone.Substring(4, Phone.Length - 4);
                    else if (Phone.IndexOf("0121") == 0) return "8479" + Phone.Substring(4, Phone.Length - 4);
                    else if (Phone.IndexOf("0122") == 0) return "8477" + Phone.Substring(4, Phone.Length - 4);
                    else if (Phone.IndexOf("0126") == 0) return "8476" + Phone.Substring(4, Phone.Length - 4);
                    else if (Phone.IndexOf("0128") == 0) return "8478" + Phone.Substring(4, Phone.Length - 4);
                    #endregion
                    #region check mạng vinaphone
                    else if (Phone.IndexOf("0123") == 0) return "8483" + Phone.Substring(4, Phone.Length - 4);
                    else if (Phone.IndexOf("0124") == 0) return "8484" + Phone.Substring(4, Phone.Length - 4);
                    else if (Phone.IndexOf("0125") == 0) return "8485" + Phone.Substring(4, Phone.Length - 4);
                    else if (Phone.IndexOf("0127") == 0) return "8481" + Phone.Substring(4, Phone.Length - 4);
                    else if (Phone.IndexOf("0129") == 0) return "8482" + Phone.Substring(4, Phone.Length - 4);
                    #endregion
                    #region check mạng vietnamMobile
                    else if (Phone.IndexOf("0186") == 0) return "8456" + Phone.Substring(4, Phone.Length - 4);
                    else if (Phone.IndexOf("0188") == 0) return "8458" + Phone.Substring(4, Phone.Length - 4);
                    #endregion
                    #region check mạng GMobile
                    else if (Phone.IndexOf("0199") == 0) return "8459" + Phone.Substring(4, Phone.Length - 4);
                    #endregion
                    else return Phone;
                }
                #endregion
                #region check 11 số đầu 84
                else if (Phone.Length == 12 && Phone.IndexOf("84") == 0)
                {
                    #region check mạng viettel
                    if (Phone.IndexOf("84162") == 0 || Phone.IndexOf("84163") == 0 || Phone.IndexOf("84164") == 0 ||
                        Phone.IndexOf("84165") == 0 || Phone.IndexOf("84166") == 0 || Phone.IndexOf("84167") == 0 ||
                        Phone.IndexOf("84168") == 0 || Phone.IndexOf("84169") == 0)
                        return "843" + Phone.Substring(4, Phone.Length - 4);
                    #endregion
                    #region check mạng mobifone
                    else if (Phone.IndexOf("84120") == 0) return "8470" + Phone.Substring(5, Phone.Length - 5);
                    else if (Phone.IndexOf("84121") == 0) return "8479" + Phone.Substring(5, Phone.Length - 5);
                    else if (Phone.IndexOf("84122") == 0) return "8477" + Phone.Substring(5, Phone.Length - 5);
                    else if (Phone.IndexOf("84126") == 0) return "8476" + Phone.Substring(5, Phone.Length - 5);
                    else if (Phone.IndexOf("84128") == 0) return "8478" + Phone.Substring(5, Phone.Length - 5);
                    #endregion
                    #region check mạng vinaphone
                    else if (Phone.IndexOf("84123") == 0) return "8483" + Phone.Substring(5, Phone.Length - 5);
                    else if (Phone.IndexOf("84124") == 0) return "8484" + Phone.Substring(5, Phone.Length - 5);
                    else if (Phone.IndexOf("84125") == 0) return "8485" + Phone.Substring(5, Phone.Length - 5);
                    else if (Phone.IndexOf("84127") == 0) return "8481" + Phone.Substring(5, Phone.Length - 5);
                    else if (Phone.IndexOf("84129") == 0) return "8482" + Phone.Substring(5, Phone.Length - 5);
                    #endregion
                    #region check mạng vietnamMobile
                    else if (Phone.IndexOf("84186") == 0) return "8456" + Phone.Substring(5, Phone.Length - 5);
                    else if (Phone.IndexOf("84188") == 0) return "8458" + Phone.Substring(5, Phone.Length - 5);
                    #endregion
                    #region check mạng GMobile
                    else if (Phone.IndexOf("84199") == 0) return "8459" + Phone.Substring(5, Phone.Length - 5);
                    #endregion
                    else return Phone;
                }
                #endregion
                else if (Phone.Length == 9)
                    return "84" + Phone;
                return Phone;
            }
            catch { return Phone; }
        }
        public string getLoaiNhaMang(string phone)
        {
            ChuyenMangGiuSoBO chuyenMangGiuSoBO = new ChuyenMangGiuSoBO();
            List<CHUYEN_MANG_GIU_SO> listChange = chuyenMangGiuSoBO.getChuyenMangGiuSo();
            if (!string.IsNullOrEmpty(phone))
            {
                long index_sdt = removeHeadPhone(phone);
                if (index_sdt > 0)
                {
                    CHUYEN_MANG_GIU_SO detail = listChange.FirstOrDefault(x => x.SDT == index_sdt);
                    if (detail != null) return detail.LOAI_NHA_MANG;
                }
                string a = "";
                List<string> lst_viettel = new List<string> { "086", "096", "097", "098", "032", "033", "034", "035", "036", "037", "038", "039" };
                List<string> lst_mobi = new List<string> { "089", "090", "093", "070", "079", "077", "076", "078" };
                List<string> lst_vina = new List<string> { "088", "091", "094", "083", "084", "085", "081", "082" };
                List<string> lst_vnMobile = new List<string> { "092", "052", "056", "058" };
                List<string> lst_gMobile = new List<string> { "099", "059" };
                //List<string> lst_sPhone = new List<string> { "095" };

                if (phone.Length == 11 && phone.IndexOf("84") == 0)
                    phone = "0" + phone.Substring(2, 9);
                else if (phone.Length == 9 && phone.IndexOf("0") != 0)
                    phone = "0" + phone;

                if (phone.Length != 10) return "";

                a = phone.Substring(0, 3);

                if (lst_viettel.Contains(a)) return "Viettel";
                if (lst_mobi.Contains(a)) return "MobiFone";
                if (lst_vina.Contains(a)) return "VinaPhone";
                if (lst_vnMobile.Contains(a)) return "VietnamMobile";
                if (lst_gMobile.Contains(a)) return "GMobile";
                
                return "";
            }
            else
            {
                return "";
            }
        }
        public string getLoaiNhaMangVietTat(string phone)
        {
            string loai_nha_mang = getLoaiNhaMang(phone);
            switch (loai_nha_mang)
            {
                case "Viettel":
                    return "VT";
                case "MobiFone":
                    return "MB";
                case "VinaPhone":
                    return "VN";
                case "VietnamMobile":
                    return "VNM";
                case "GMobile":
                    return "GM";
                default:
                    return "";
            }
        }
        public long removeHeadPhone(string phone)
        {
            long index_sdt = 0;
            try
            {
                if (!string.IsNullOrEmpty(phone))
                {
                    if (phone.IndexOf("84") == 0)
                        index_sdt = Convert.ToInt64(phone.Substring(2, phone.Length - 2));
                    else if (phone.IndexOf("0") == 0)
                        index_sdt = Convert.ToInt64(phone.Substring(1, phone.Length - 1));
                    else index_sdt = Convert.ToInt64(phone);
                }
            }
            catch { }
            return index_sdt;
        }
        public DataTable ConvertListToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
              TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                try
                {
                    if (!prop.PropertyType.ToString().Contains("ICollection"))
                        table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }
                catch { }
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    if (!prop.PropertyType.ToString().Contains("ICollection"))
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }
            return table;
        }
        public byte[] StreamToByteArray(string fileName)
        {
            try
            {
                var total_stream = new byte[0];
                using (Stream input = File.Open(fileName, FileMode.Open, FileAccess.Read))
                {
                    var stream_array = new byte[0];
                    var buffer = new byte[1024];
                    int read = 0;

                    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        stream_array = new byte[total_stream.Length + read];
                        total_stream.CopyTo(stream_array, 0);
                        Array.Copy(buffer, 0, stream_array, total_stream.Length, read);
                        total_stream = stream_array;
                    }
                }
                return total_stream;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }
        #region Export Thanhnv
        public bool checkColumnShowInRadGrid(RadGrid radGrid, string unique)
        {
            try
            {
                if (radGrid.MasterTableView.Columns.FindByUniqueName(unique) != null
                    && radGrid.MasterTableView.Columns.FindByUniqueName(unique).Visible == true
                    && radGrid.MasterTableView.Columns.FindByUniqueName(unique).Display == true)
                    return true;
                else return false;
            }
            catch { }
            return false;
        }
        public void ExportExcelByOledbMutilData(string serverPath, string path, string newName, string nameFileOutput, string nameSheet, List<ItemDanhMuc> listCell
          , List<DataExcel> lstData
            )
        {
            Guid strGuid = Guid.NewGuid();
            newName = strGuid.ToString() + newName;
            string pathNew = String.Format("{0}\\ExportTemps\\{1}", serverPath, newName);
            try
            {
                System.IO.File.Copy(path, pathNew, true);
                string strConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pathNew + @";Extended Properties=""Excel 12.0 Xml;HDR=No""";
                OleDbConnection objConn = new OleDbConnection(strConn);
                objConn.Open();
                OleDbCommand myCommand = new OleDbCommand();
                myCommand.Connection = objConn;
                try
                {
                    #region Set cell
                    if (listCell != null && listCell.Count > 0)
                    {
                        foreach (var item in listCell)
                        {
                            myCommand.CommandText =
                               "UPDATE [" + nameSheet + "$" + item.Value + "] SET F1=@value";
                            myCommand.Parameters.AddRange(new OleDbParameter[]
                            {
                                new OleDbParameter("@value", item.Text)
                           });
                            myCommand.ExecuteNonQuery();

                        }
                    }
                    #endregion
                    #region Set Data
                    foreach (DataExcel item in lstData)
                    {
                        int index = 1, indexStart = item.rowStart;
                        foreach (DataRow row in item.dt.Rows)
                        {
                            string strQuery = string.Format(@"UPDATE [{0}${1}{3}:{2}{3}] SET ", nameSheet, item.colStart, item.colEnd, indexStart);
                            List<OleDbParameter> lstParam = new List<OleDbParameter>();
                            if (item.have_stt)
                            {
                                #region Có STT
                                strQuery += " F1='" + index.ToString() + "'";
                                for (int i = 0; i < item.lstColumn.Count; i++)
                                {
                                    string valCell = row[item.lstColumn[i].Name].ToString();
                                    switch (item.lstColumn[i].Type)
                                    {
                                        case "String":
                                            break;
                                        case "Number":
                                            break;
                                        case "Decimal":
                                            break;
                                        case "Date":
                                            DateTime ngay1 = new DateTime();
                                            try
                                            {
                                                ngay1 = Convert.ToDateTime(row[item.lstColumn[i].Name].ToString());
                                                valCell = ngay1.ToString("dd/MM/yyyy");
                                            }
                                            catch { }
                                            break;
                                        case "DeteTime":
                                            DateTime ngay2 = new DateTime();
                                            try
                                            {
                                                ngay2 = Convert.ToDateTime(row[item.lstColumn[i].Name].ToString());
                                                valCell = ngay2.ToString("dd/MM/yyyy hh:mm:ss");
                                            }
                                            catch { }
                                            break;
                                    }
                                    strQuery += " ,F" + (i + 2).ToString() + "=@f" + (i + 2).ToString();
                                    lstParam.Add(new OleDbParameter("@f" + (i + 2).ToString(), valCell));
                                }
                                #endregion
                            }
                            else
                            {
                                #region Không có stt
                                for (int i = 0; i < item.lstColumn.Count; i++)
                                {
                                    string valCell = row[item.lstColumn[i].Name].ToString();
                                    switch (item.lstColumn[i].Type)
                                    {
                                        case "String":
                                            break;
                                        case "Number":
                                            break;
                                        case "Decimal":
                                            break;
                                        case "Date":
                                            DateTime ngay1 = new DateTime();
                                            try
                                            {
                                                ngay1 = Convert.ToDateTime(row[item.lstColumn[i].Name].ToString());
                                                valCell = ngay1.ToString("dd/MM/yyyy");
                                            }
                                            catch { }
                                            break;
                                        case "DeteTime":
                                            DateTime ngay2 = new DateTime();
                                            try
                                            {
                                                ngay2 = Convert.ToDateTime(row[item.lstColumn[i].Name].ToString());
                                                valCell = ngay2.ToString("dd/MM/yyyy hh:mm:ss");
                                            }
                                            catch { }
                                            break;
                                    }

                                    if (i == 0)
                                        strQuery += "  F" + (i + 1).ToString() + "=@f" + (i + 1).ToString();
                                    else
                                        strQuery += " ,F" + (i + 1).ToString() + "=@f" + (i + 1).ToString();
                                    lstParam.Add(new OleDbParameter("@f" + (i + 1).ToString(), valCell));
                                }
                                #endregion
                            }
                            myCommand.CommandText = strQuery;
                            myCommand.Parameters.Clear();
                            myCommand.Parameters.AddRange(lstParam.ToArray());
                            myCommand.ExecuteNonQuery();
                            indexStart++;
                            index++;
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Response.Write(ex.ToString());
                }
                finally
                {
                    objConn.Close();
                }

                HttpContext.Current.Response.Clear();
                byte[] file = StreamToByteArray(pathNew);
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.‌​sheet";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + nameFileOutput);
                HttpContext.Current.Response.BinaryWrite(file);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
            catch (Exception ex) { HttpContext.Current.Response.Write(ex.ToString()); }
            finally
            {
                File.Delete(pathNew);
            }
        }
        public void ExportExcel(string serverPath, string path, string newName, string nameFileOutput, int indexSheet, List<ItemDanhMuc> listCell
            , int rowStart, string colStart, string colEnd, DataTable dt, List<ExcelEntity> lstColumn, bool have_stt)
        {
            Guid strGuid = Guid.NewGuid();
            newName = strGuid.ToString() + newName;
            string pathNew = String.Format("{0}\\ExportTemps\\{1}", serverPath, newName);
            try
            {
                System.IO.File.Copy(path, pathNew, true);
                var workbook = new XLWorkbook(pathNew);
                var ws = workbook.Worksheet(indexSheet);
                #region Set cell
                if (listCell != null && listCell.Count > 0)
                {
                    foreach (var item in listCell)
                    {
                        var itemCell = ws.Range(item.Value);
                        itemCell.Value = item.Text;
                    }
                }
                #endregion
                #region Set Data
                string rangeData = string.Format("{0}{2}:{1}{3}", colStart, colEnd, rowStart, rowStart + dt.Rows.Count - 1);
                var rngNumbers = ws.Range(rangeData);
                int index = 1, indexStart = rowStart;
                foreach (DataRow row in dt.Rows)
                {
                    if (have_stt)
                    {
                        rngNumbers.Cell(index, 1).Value = index.ToString();
                        rngNumbers.Cell(index, 1).Style.Alignment.WrapText = true;
                        rngNumbers.Cell(index, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        rngNumbers.Cell(index, 1).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        rngNumbers.Cell(index, 1).Style.Border.TopBorderColor = XLColor.Black;
                        rngNumbers.Cell(index, 1).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        rngNumbers.Cell(index, 1).Style.Border.BottomBorderColor = XLColor.Black;
                        rngNumbers.Cell(index, 1).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        rngNumbers.Cell(index, 1).Style.Border.LeftBorderColor = XLColor.Black;
                        rngNumbers.Cell(index, 1).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        rngNumbers.Cell(index, 1).Style.Border.RightBorderColor = XLColor.Black;
                        for (int i = 0; i < lstColumn.Count; i++)
                        {
                            string valCell = row[lstColumn[i].Name] == null ? " " : row[lstColumn[i].Name].ToString().Replace("<b>", "").Replace("</b>", "");
                            switch (lstColumn[i].Type)
                            {
                                case "String":
                                    break;
                                case "%":
                                    try
                                    {
                                        decimal val = Convert.ToDecimal(row[lstColumn[i].Name].ToString()) * 100;
                                        valCell = val.ToString("#,##0.00") + "%";
                                    }
                                    catch { }
                                    break;
                                case "LenLop":
                                    string len_lop = "Chưa xét";
                                    try
                                    {
                                        string tmp = row[lstColumn[i].Name].ToString().ToLower();
                                        if (tmp == "1" || tmp == "true")
                                            len_lop = "Lên lớp";
                                        else if (tmp == "0" || tmp == "false")
                                            len_lop = "Lưu ban";
                                        else len_lop = "Chưa xét";
                                    }
                                    catch { }
                                    valCell = len_lop;
                                    break;
                                case "CoKhong":
                                    string co_khong = "";
                                    try
                                    {
                                        string tmp = row[lstColumn[i].Name].ToString().ToLower();
                                        if (tmp == "1" || tmp == "true")
                                            co_khong = "Có";
                                        else if (tmp == "0" || tmp == "false")
                                            co_khong = "";
                                        else co_khong = "";
                                    }
                                    catch { }
                                    valCell = co_khong;
                                    break;
                                case "Number":
                                    rngNumbers.Cell(index, i + 2).Style.NumberFormat.Format = "#,###";
                                    rngNumbers.Cell(index, i + 2).DataType = XLCellValues.Number;
                                    break;
                                case "Decimal":
                                    rngNumbers.Cell(index, i + 2).Style.NumberFormat.Format = "#,##0.00";
                                    rngNumbers.Cell(index, i + 2).DataType = XLCellValues.Number;
                                    break;
                                case "Date":
                                    DateTime ngay1 = new DateTime();
                                    try
                                    {
                                        ngay1 = Convert.ToDateTime(row[lstColumn[i].Name].ToString());
                                        valCell = ngay1.ToString("dd/MM/yyyy");
                                    }
                                    catch { }
                                    break;
                                case "DeteTime":
                                    DateTime ngay2 = new DateTime();
                                    try
                                    {
                                        ngay2 = Convert.ToDateTime(row[lstColumn[i].Name].ToString());
                                        valCell = ngay2.ToString("dd/MM/yyyy hh:mm:ss");
                                    }
                                    catch { }
                                    break;
                            }
                            rngNumbers.Cell(index, i + 2).SetValue<string>(valCell);
                            rngNumbers.Cell(index, i + 2).Style.Font.FontColor = lstColumn[i].Color;
                            rngNumbers.Cell(index, i + 2).Style.Alignment.Horizontal = lstColumn[i].Align;
                            rngNumbers.Cell(index, i + 2).Style.Alignment.WrapText = true;
                            rngNumbers.Cell(index, i + 2).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            rngNumbers.Cell(index, i + 2).Style.Border.TopBorderColor = XLColor.Black;
                            rngNumbers.Cell(index, i + 2).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            rngNumbers.Cell(index, i + 2).Style.Border.BottomBorderColor = XLColor.Black;
                            rngNumbers.Cell(index, i + 2).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            rngNumbers.Cell(index, i + 2).Style.Border.LeftBorderColor = XLColor.Black;
                            rngNumbers.Cell(index, i + 2).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            rngNumbers.Cell(index, i + 2).Style.Border.RightBorderColor = XLColor.Black;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < lstColumn.Count; i++)
                        {
                            string valCell = row[lstColumn[i].Name] == null ? " " : row[lstColumn[i].Name].ToString().Replace("<b>", "").Replace("</b>", "");
                            switch (lstColumn[i].Type)
                            {
                                case "String":
                                    break;
                                case "%":
                                    try
                                    {
                                        decimal val = Convert.ToDecimal(row[lstColumn[i].Name].ToString()) * 100;
                                        valCell = val.ToString("#,##0.00") + "%";
                                    }
                                    catch { }
                                    break;
                                case "LenLop":
                                    string len_lop = "Chưa xét";
                                    try
                                    {
                                        string tmp = row[lstColumn[i].Name].ToString().ToLower();
                                        if (tmp == "1" || tmp == "true")
                                            len_lop = "Lên lớp";
                                        else if (tmp == "0" || tmp == "false")
                                            len_lop = "Lưu ban";
                                        else len_lop = "Chưa xét";
                                    }
                                    catch { }
                                    valCell = len_lop;
                                    break;
                                case "CoKhong":
                                    string co_khong = "";
                                    try
                                    {
                                        string tmp = row[lstColumn[i].Name].ToString().ToLower();
                                        if (tmp == "1" || tmp == "true")
                                            co_khong = "Có";
                                        else if (tmp == "0" || tmp == "false")
                                            co_khong = "";
                                        else co_khong = "";
                                    }
                                    catch { }
                                    valCell = co_khong;
                                    break;
                                case "Number":
                                    rngNumbers.Cell(index, i + 1).Style.NumberFormat.Format = "#,###";
                                    rngNumbers.Cell(index, i + 1).DataType = XLCellValues.Number;
                                    break;
                                case "Decimal":
                                    rngNumbers.Cell(index, i + 1).Style.NumberFormat.Format = "#,##0.00";
                                    rngNumbers.Cell(index, i + 1).DataType = XLCellValues.Number;
                                    break;
                                case "Date":
                                    DateTime ngay1 = new DateTime();
                                    try
                                    {
                                        ngay1 = Convert.ToDateTime(row[lstColumn[i].Name].ToString());
                                        valCell = ngay1.ToString("dd/MM/yyyy");
                                    }
                                    catch { }
                                    break;
                                case "DeteTime":
                                    DateTime ngay2 = new DateTime();
                                    try
                                    {
                                        ngay2 = Convert.ToDateTime(row[lstColumn[i].Name].ToString());
                                        valCell = ngay2.ToString("dd/MM/yyyy hh:mm:ss");
                                    }
                                    catch { }
                                    break;
                            }
                            rngNumbers.Cell(index, i + 1).SetValue<string>(valCell);
                            rngNumbers.Cell(index, i + 1).Style.Font.FontColor = lstColumn[i].Color;
                            rngNumbers.Cell(index, i + 1).Style.Alignment.Horizontal = lstColumn[i].Align;
                            rngNumbers.Cell(index, i + 1).Style.Alignment.WrapText = true;
                            rngNumbers.Cell(index, i + 1).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            rngNumbers.Cell(index, i + 1).Style.Border.TopBorderColor = XLColor.Black;
                            rngNumbers.Cell(index, i + 1).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            rngNumbers.Cell(index, i + 1).Style.Border.BottomBorderColor = XLColor.Black;
                            rngNumbers.Cell(index, i + 1).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            rngNumbers.Cell(index, i + 1).Style.Border.LeftBorderColor = XLColor.Black;
                            rngNumbers.Cell(index, i + 1).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            rngNumbers.Cell(index, i + 1).Style.Border.RightBorderColor = XLColor.Black;
                        }
                    }
                    indexStart++;
                    index++;
                }
                #endregion
                workbook.SaveAs(pathNew);
                HttpContext.Current.Response.Clear();
                byte[] file = StreamToByteArray(pathNew);
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.‌​sheet";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + nameFileOutput);
                HttpContext.Current.Response.BinaryWrite(file);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                // HttpContext.Current.Response.Write(ex.ToString());
            }
            finally
            {
                File.Delete(pathNew);
            }
        }

        public void ExportExcelMutilData(string serverPath, string path, string newName, string nameFileOutput, int indexSheet, List<ItemDanhMuc> listCell
            , List<DataExcel> lstData)
        {
            Guid strGuid = Guid.NewGuid();
            newName = strGuid.ToString() + newName;
            string pathNew = String.Format("{0}\\ExportTemps\\{1}", serverPath, newName);
            try
            {
                System.IO.File.Copy(path, pathNew, true);
                var workbook = new XLWorkbook(pathNew);
                var ws = workbook.Worksheet(indexSheet);
                #region Set cell
                if (listCell != null && listCell.Count > 0)
                {
                    foreach (var item in listCell)
                    {
                        var itemCell = ws.Range(item.Value);
                        itemCell.Value = item.Text;
                    }
                }
                #endregion
                #region Set Data
                if (lstData != null && lstData.Count > 0)
                {
                    foreach (DataExcel item in lstData)
                    {
                        if (item.indexSheet != null)
                            ws = workbook.Worksheet(item.indexSheet.Value);
                        string rangeData = string.Format("{0}{2}:{1}{3}", item.colStart, item.colEnd, item.rowStart, item.rowStart + item.dt.Rows.Count - 1);
                        var rngNumbers = ws.Range(rangeData);
                        int index = 1, indexStart = item.rowStart;
                        foreach (DataRow row in item.dt.Rows)
                        {
                            if (item.have_stt)
                            {
                                rngNumbers.Cell(index, 1).Value = index.ToString();
                                rngNumbers.Cell(index, 1).Style.Alignment.WrapText = true;
                                rngNumbers.Cell(index, 1).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                rngNumbers.Cell(index, 1).Style.Border.TopBorderColor = XLColor.Black;
                                rngNumbers.Cell(index, 1).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                rngNumbers.Cell(index, 1).Style.Border.BottomBorderColor = XLColor.Black;
                                rngNumbers.Cell(index, 1).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                rngNumbers.Cell(index, 1).Style.Border.LeftBorderColor = XLColor.Black;
                                rngNumbers.Cell(index, 1).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                rngNumbers.Cell(index, 1).Style.Border.RightBorderColor = XLColor.Black;
                                for (int i = 0; i < item.lstColumn.Count; i++)
                                {
                                    string valCell = row[item.lstColumn[i].Name] == null ? " " : row[item.lstColumn[i].Name].ToString().Replace("<b>", "").Replace("</b>", "");
                                    switch (item.lstColumn[i].Type)
                                    {
                                        case "String":
                                            break;
                                        case "%":
                                            try
                                            {
                                                decimal val = Convert.ToDecimal(row[item.lstColumn[i].Name].ToString()) * 100;
                                                valCell = val.ToString("#,##0.00") + "%";
                                            }
                                            catch { }
                                            break;
                                        case "LenLop":
                                            string len_lop = "Chưa xét";
                                            try
                                            {
                                                string tmp = row[item.lstColumn[i].Name].ToString().ToLower();
                                                if (tmp == "1" || tmp == "true")
                                                    len_lop = "Lên lớp";
                                                else if (tmp == "0" || tmp == "false")
                                                    len_lop = "Lưu ban";
                                                else len_lop = "Chưa xét";
                                            }
                                            catch { }
                                            valCell = len_lop;
                                            break;
                                        case "CoKhong":
                                            string co_khong = "";
                                            try
                                            {
                                                string tmp = row[item.lstColumn[i].Name].ToString().ToLower();
                                                if (tmp == "1" || tmp == "true")
                                                    co_khong = "Có";
                                                else if (tmp == "0" || tmp == "false")
                                                    co_khong = "";
                                                else co_khong = "";
                                            }
                                            catch { }
                                            valCell = co_khong;
                                            break;
                                        case "Number":
                                            rngNumbers.Cell(index, i + 2).Style.NumberFormat.Format = "#,###";
                                            rngNumbers.Cell(index, i + 2).DataType = XLCellValues.Number;
                                            break;
                                        case "Decimal":
                                            rngNumbers.Cell(index, i + 2).Style.NumberFormat.Format = "#,##0.00";
                                            rngNumbers.Cell(index, i + 2).DataType = XLCellValues.Number;
                                            break;
                                        case "Date":
                                            DateTime ngay1 = new DateTime();
                                            try
                                            {
                                                ngay1 = Convert.ToDateTime(row[item.lstColumn[i].Name].ToString());
                                                valCell = ngay1.ToString("dd/MM/yyyy");
                                            }
                                            catch { }
                                            break;
                                        case "DeteTime":
                                            DateTime ngay2 = new DateTime();
                                            try
                                            {
                                                ngay2 = Convert.ToDateTime(row[item.lstColumn[i].Name].ToString());
                                                valCell = ngay2.ToString("dd/MM/yyyy hh:mm:ss");
                                            }
                                            catch { }
                                            break;
                                    }
                                    rngNumbers.Cell(index, i + 2).SetValue<string>(valCell);
                                    rngNumbers.Cell(index, i + 2).Style.Font.FontColor = item.lstColumn[i].Color;
                                    rngNumbers.Cell(index, i + 2).Style.Alignment.Horizontal = item.lstColumn[i].Align;
                                    rngNumbers.Cell(index, i + 2).Style.Alignment.WrapText = true;
                                    rngNumbers.Cell(index, i + 2).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                    rngNumbers.Cell(index, i + 2).Style.Border.TopBorderColor = XLColor.Black;
                                    rngNumbers.Cell(index, i + 2).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                    rngNumbers.Cell(index, i + 2).Style.Border.BottomBorderColor = XLColor.Black;
                                    rngNumbers.Cell(index, i + 2).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                    rngNumbers.Cell(index, i + 2).Style.Border.LeftBorderColor = XLColor.Black;
                                    rngNumbers.Cell(index, i + 2).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                    rngNumbers.Cell(index, i + 2).Style.Border.RightBorderColor = XLColor.Black;
                                }
                            }
                            else
                            {
                                for (int i = 0; i < item.lstColumn.Count; i++)
                                {
                                    string valCell = row[item.lstColumn[i].Name] == null ? " " : row[item.lstColumn[i].Name].ToString().Replace("<b>", "").Replace("</b>", "");
                                    switch (item.lstColumn[i].Type)
                                    {
                                        case "String":
                                            break;
                                        case "%":
                                            try
                                            {
                                                decimal val = Convert.ToDecimal(row[item.lstColumn[i].Name].ToString()) * 100;
                                                valCell = val.ToString("#,##0.00") + "%";
                                            }
                                            catch { }
                                            break;
                                        case "LenLop":
                                            string len_lop = "Chưa xét";
                                            try
                                            {
                                                string tmp = row[item.lstColumn[i].Name].ToString().ToLower();
                                                if (tmp == "1" || tmp == "true")
                                                    len_lop = "Lên lớp";
                                                else if (tmp == "0" || tmp == "false")
                                                    len_lop = "Lưu ban";
                                                else len_lop = "Chưa xét";
                                            }
                                            catch { }
                                            valCell = len_lop;
                                            break;
                                        case "CoKhong":
                                            string co_khong = "";
                                            try
                                            {
                                                string tmp = row[item.lstColumn[i].Name].ToString().ToLower();
                                                if (tmp == "1" || tmp == "true")
                                                    co_khong = "Có";
                                                else if (tmp == "0" || tmp == "false")
                                                    co_khong = "";
                                                else co_khong = "";
                                            }
                                            catch { }
                                            valCell = co_khong;
                                            break;
                                        case "Number":
                                            rngNumbers.Cell(index, i + 1).Style.NumberFormat.Format = "#,###";
                                            rngNumbers.Cell(index, i + 1).DataType = XLCellValues.Number;
                                            break;
                                        case "Decimal":
                                            rngNumbers.Cell(index, i + 1).Style.NumberFormat.Format = "#,##0.00";
                                            rngNumbers.Cell(index, i + 1).DataType = XLCellValues.Number;
                                            break;
                                        case "Date":
                                            DateTime ngay1 = new DateTime();
                                            try
                                            {
                                                ngay1 = Convert.ToDateTime(row[item.lstColumn[i].Name].ToString());
                                                valCell = ngay1.ToString("dd/MM/yyyy");
                                            }
                                            catch { }
                                            break;
                                        case "DeteTime":
                                            DateTime ngay2 = new DateTime();
                                            try
                                            {
                                                ngay2 = Convert.ToDateTime(row[item.lstColumn[i].Name].ToString());
                                                valCell = ngay2.ToString("dd/MM/yyyy hh:mm:ss");
                                            }
                                            catch { }
                                            break;
                                    }
                                    rngNumbers.Cell(index, i + 1).SetValue<string>(valCell);
                                    rngNumbers.Cell(index, i + 1).Style.Font.FontColor = item.lstColumn[i].Color;
                                    rngNumbers.Cell(index, i + 1).Style.Alignment.Horizontal = item.lstColumn[i].Align;
                                    rngNumbers.Cell(index, i + 1).Style.Alignment.WrapText = true;
                                    rngNumbers.Cell(index, i + 1).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                    rngNumbers.Cell(index, i + 1).Style.Border.TopBorderColor = XLColor.Black;
                                    rngNumbers.Cell(index, i + 1).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                    rngNumbers.Cell(index, i + 1).Style.Border.BottomBorderColor = XLColor.Black;
                                    rngNumbers.Cell(index, i + 1).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                    rngNumbers.Cell(index, i + 1).Style.Border.LeftBorderColor = XLColor.Black;
                                    rngNumbers.Cell(index, i + 1).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                    rngNumbers.Cell(index, i + 1).Style.Border.RightBorderColor = XLColor.Black;
                                }
                            }
                            indexStart++;
                            index++;
                        }
                    }
                }
                #endregion
                workbook.SaveAs(pathNew);
                HttpContext.Current.Response.Clear();
                byte[] file = StreamToByteArray(pathNew);
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.‌​sheet";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + nameFileOutput);
                HttpContext.Current.Response.BinaryWrite(file);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                //HttpContext.Current.Response.Write(ex.ToString());
            }
            finally
            {
                File.Delete(pathNew);
            }
        }
        public void ExportExcelDynamic(string serverPath, string path, string newName, string nameFileOutput, int indexSheet, List<ExcelHeaderEntity> listCell
                , int rowHeaderStart, int rowStart, string colStart, string colEnd, DataTable dt, List<ExcelHeaderEntity> lstHeader, List<ExcelEntity> lstColumn, bool have_stt)
        {
            Guid strGuid = Guid.NewGuid();
            newName = strGuid.ToString() + newName;
            string pathNew = String.Format("{0}\\ExportTemps\\{1}", serverPath, newName);
            try
            {
                System.IO.File.Copy(path, pathNew, true);
                var workbook = new XLWorkbook(pathNew);
                var ws = workbook.Worksheet(indexSheet);
                #region Set cell
                if (listCell != null && listCell.Count > 0)
                {
                    foreach (var item in listCell)
                    {
                        string colEndCell = GetExcelColumnName(ExcelColumnNameToNumber(item.colName) + item.colM - 1);
                        var itemCell = ws.Range(string.Format(@"{0}{1}:{2}{3}", item.colName, item.rowIndex, colEndCell, item.rowIndex + item.rowM - 1));
                        ws.Range(string.Format(@"{0}{1}:{2}{3}", item.colName, item.rowIndex, colEndCell, item.rowIndex + item.rowM - 1)).Merge();
                        itemCell.Value = item.name;
                        itemCell.Style.Font.Bold = true;
                        if (item.Align != null)
                            itemCell.Style.Alignment.Horizontal = item.Align.Value;
                        if (item.fontSize != null)
                            itemCell.Style.Font.FontSize = item.fontSize.Value;

                    }
                }
                #endregion
                #region SetHeader
                if (lstHeader != null && lstHeader.Count > 0)
                {
                    for (int i = 0; i < lstHeader.Count; i++)
                    {
                        var item = lstHeader[i];
                        string colName = colStart;
                        int rowIndex = rowHeaderStart;
                        #region Có cha
                        if (item.parentIndex != null)
                        {
                            ExcelHeaderEntity parentItem = lstHeader[item.parentIndex.Value];
                            if (parentItem.lstChild == null || parentItem.lstChild.Count == 0)
                            {
                                colName = parentItem.colName;
                                parentItem.lstChild = new List<ExcelHeaderEntity>();
                            }
                            else
                            {
                                string colnameParent = parentItem.lstChild[parentItem.lstChild.Count - 1].colName;
                                colName = GetExcelColumnName(ExcelColumnNameToNumber(parentItem.lstChild[parentItem.lstChild.Count - 1].colName) + parentItem.lstChild[parentItem.lstChild.Count - 1].colM);
                            }
                            rowIndex = parentItem.rowIndex + 1;
                            item.colName = colName;
                            item.rowIndex = rowIndex;
                            parentItem.lstChild.Add(item);
                            lstHeader[item.parentIndex.Value] = parentItem;
                        }
                        #endregion
                        #region Không cha
                        else
                        {
                            if (i > 0) colName = GetExcelColumnName(ExcelColumnNameToNumber(lstHeader[i - 1].colName) + 1);
                            rowIndex = rowHeaderStart;
                            item.colName = colName;
                            item.rowIndex = rowIndex;
                        }
                        #endregion
                        lstHeader[i] = item;
                        string colEndCell = GetExcelColumnName(ExcelColumnNameToNumber(item.colName) + item.colM - 1);
                        var itemCell = ws.Range(string.Format(@"{0}{1}:{2}{3}", item.colName, item.rowIndex, colEndCell, item.rowIndex + item.rowM - 1));
                        ws.Range(string.Format(@"{0}{1}:{2}{3}", item.colName, item.rowIndex, colEndCell, item.rowIndex + item.rowM - 1)).Merge();
                        itemCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        itemCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        itemCell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        itemCell.Style.Border.TopBorderColor = XLColor.Black;
                        itemCell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        itemCell.Style.Border.BottomBorderColor = XLColor.Black;
                        itemCell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        itemCell.Style.Border.LeftBorderColor = XLColor.Black;
                        itemCell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        itemCell.Style.Border.RightBorderColor = XLColor.Black;
                        itemCell.Style.Font.Bold = true;

                        var itemcol = ws.Column(item.colName);
                        if (item.width != null)
                        {
                            itemCell.Style.Alignment.WrapText = true;
                            itemcol.Width = item.width.Value;
                        }
                        var itemrow = ws.Row(item.rowIndex);
                        if (item.height != null)
                        {
                            itemrow.Height = item.height.Value;
                        }
                        else
                        {
                            itemrow.Height = 24;
                        }
                        itemCell.Value = item.name;
                    }
                }
                #endregion
                #region Set Data
                string rangeData = string.Format("{0}{2}:{1}{3}", colStart, colEnd, rowStart, rowStart + dt.Rows.Count - 1);
                var rngNumbers = ws.Range(rangeData);
                int index = 1, indexStart = rowStart;
                foreach (DataRow row in dt.Rows)
                {
                    if (have_stt)
                    {
                        rngNumbers.Cell(index, 1).Value = index.ToString();
                        rngNumbers.Cell(index, 1).Style.Alignment.WrapText = true;
                        rngNumbers.Cell(index, 1).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        rngNumbers.Cell(index, 1).Style.Border.TopBorderColor = XLColor.Black;
                        rngNumbers.Cell(index, 1).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        rngNumbers.Cell(index, 1).Style.Border.BottomBorderColor = XLColor.Black;
                        rngNumbers.Cell(index, 1).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        rngNumbers.Cell(index, 1).Style.Border.LeftBorderColor = XLColor.Black;
                        rngNumbers.Cell(index, 1).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        rngNumbers.Cell(index, 1).Style.Border.RightBorderColor = XLColor.Black;
                        for (int i = 0; i < lstColumn.Count; i++)
                        {
                            string valCell = row[lstColumn[i].Name] == null ? " " : row[lstColumn[i].Name].ToString().Replace("<b>", "").Replace("</b>", "");
                            switch (lstColumn[i].Type)
                            {
                                case "String":
                                    break;
                                case "%":
                                    try
                                    {
                                        decimal val = Convert.ToDecimal(row[lstColumn[i].Name].ToString()) * 100;
                                        valCell = val.ToString("#,##0.00") + "%";
                                    }
                                    catch { }
                                    break;
                                case "LenLop":
                                    string len_lop = "Chưa xét";
                                    try
                                    {
                                        string tmp = row[lstColumn[i].Name].ToString().ToLower();
                                        if (tmp == "1" || tmp == "true")
                                            len_lop = "Lên lớp";
                                        else if (tmp == "0" || tmp == "false")
                                            len_lop = "Lưu ban";
                                        else len_lop = "Chưa xét";
                                    }
                                    catch { }
                                    valCell = len_lop;
                                    break;
                                case "CoKhong":
                                    string co_khong = "";
                                    try
                                    {
                                        string tmp = row[lstColumn[i].Name].ToString().ToLower();
                                        if (tmp == "1" || tmp == "true")
                                            co_khong = "Có";
                                        else if (tmp == "0" || tmp == "false")
                                            co_khong = "";
                                        else co_khong = "";
                                    }
                                    catch { }
                                    valCell = co_khong;
                                    break;
                                case "check":
                                    string check = "";
                                    try
                                    {
                                        string tmp = row[lstColumn[i].Name].ToString().ToLower();
                                        if (tmp == "1" || tmp == "true")
                                            check = "x";
                                        else check = "";
                                    }
                                    catch { }
                                    valCell = check;
                                    break;
                                case "Number":
                                    rngNumbers.Cell(index, i + 2).Style.NumberFormat.Format = "#,###";
                                    rngNumbers.Cell(index, i + 2).DataType = XLCellValues.Number;
                                    break;
                                case "Decimal":
                                    rngNumbers.Cell(index, i + 2).Style.NumberFormat.Format = "#,##0.00";
                                    rngNumbers.Cell(index, i + 2).DataType = XLCellValues.Number;
                                    break;
                                case "Date":
                                    DateTime ngay1 = new DateTime();
                                    try
                                    {
                                        ngay1 = Convert.ToDateTime(row[lstColumn[i].Name].ToString());
                                        valCell = ngay1.ToString("dd/MM/yyyy");
                                    }
                                    catch { }
                                    break;
                                case "DeteTime":
                                    DateTime ngay2 = new DateTime();
                                    try
                                    {
                                        ngay2 = Convert.ToDateTime(row[lstColumn[i].Name].ToString());
                                        valCell = ngay2.ToString("dd/MM/yyyy hh:mm:ss");
                                    }
                                    catch { }
                                    break;
                            }
                            rngNumbers.Cell(index, i + 2).SetValue<string>(valCell);
                            rngNumbers.Cell(index, i + 2).Style.Font.FontColor = lstColumn[i].Color;
                            rngNumbers.Cell(index, i + 2).Style.Alignment.Horizontal = lstColumn[i].Align;
                            rngNumbers.Cell(index, i + 2).Style.Alignment.WrapText = true;
                            rngNumbers.Cell(index, i + 2).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            rngNumbers.Cell(index, i + 2).Style.Border.TopBorderColor = XLColor.Black;
                            rngNumbers.Cell(index, i + 2).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            rngNumbers.Cell(index, i + 2).Style.Border.BottomBorderColor = XLColor.Black;
                            rngNumbers.Cell(index, i + 2).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            rngNumbers.Cell(index, i + 2).Style.Border.LeftBorderColor = XLColor.Black;
                            rngNumbers.Cell(index, i + 2).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            rngNumbers.Cell(index, i + 2).Style.Border.RightBorderColor = XLColor.Black;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < lstColumn.Count; i++)
                        {
                            string valCell = row[lstColumn[i].Name] == null ? " " : row[lstColumn[i].Name].ToString().Replace("<b>", "").Replace("</b>", "");
                            switch (lstColumn[i].Type)
                            {
                                case "String":
                                    break;
                                case "%":
                                    try
                                    {
                                        decimal val = Convert.ToDecimal(row[lstColumn[i].Name].ToString()) * 100;
                                        valCell = val.ToString("#,##0.00") + "%";
                                    }
                                    catch { }
                                    break;
                                case "LenLop":
                                    string len_lop = "Chưa xét";
                                    try
                                    {
                                        string tmp = row[lstColumn[i].Name].ToString().ToLower();
                                        if (tmp == "1" || tmp == "true")
                                            len_lop = "Lên lớp";
                                        else if (tmp == "0" || tmp == "false")
                                            len_lop = "Lưu ban";
                                        else len_lop = "Chưa xét";
                                    }
                                    catch { }
                                    valCell = len_lop;
                                    break;
                                case "CoKhong":
                                    string co_khong = "";
                                    try
                                    {
                                        string tmp = row[lstColumn[i].Name].ToString().ToLower();
                                        if (tmp == "1" || tmp == "true")
                                            co_khong = "Có";
                                        else if (tmp == "0" || tmp == "false")
                                            co_khong = "";
                                        else co_khong = "";
                                    }
                                    catch { }
                                    valCell = co_khong;
                                    break;
                                case "check":
                                    string check = "";
                                    try
                                    {
                                        string tmp = row[lstColumn[i].Name].ToString().ToLower();
                                        if (tmp == "1" || tmp == "true")
                                            check = "x";
                                        else check = "";
                                    }
                                    catch { }
                                    valCell = check;
                                    break;
                                case "Number":
                                    rngNumbers.Cell(index, i + 1).Style.NumberFormat.Format = "#,###";
                                    rngNumbers.Cell(index, i + 1).DataType = XLCellValues.Number;
                                    break;
                                case "Decimal":
                                    rngNumbers.Cell(index, i + 1).Style.NumberFormat.Format = "#,##0.00";
                                    rngNumbers.Cell(index, i + 1).DataType = XLCellValues.Number;
                                    break;
                                case "Date":
                                    DateTime ngay1 = new DateTime();
                                    try
                                    {
                                        ngay1 = Convert.ToDateTime(row[lstColumn[i].Name].ToString());
                                        valCell = ngay1.ToString("dd/MM/yyyy");
                                    }
                                    catch { }
                                    break;
                                case "DeteTime":
                                    DateTime ngay2 = new DateTime();
                                    try
                                    {
                                        ngay2 = Convert.ToDateTime(row[lstColumn[i].Name].ToString());
                                        valCell = ngay2.ToString("dd/MM/yyyy hh:mm:ss");
                                    }
                                    catch { }
                                    break;
                            }
                            rngNumbers.Cell(index, i + 1).SetValue<string>(valCell);
                            rngNumbers.Cell(index, i + 1).Style.Font.FontColor = lstColumn[i].Color;
                            rngNumbers.Cell(index, i + 1).Style.Alignment.Horizontal = lstColumn[i].Align;
                            rngNumbers.Cell(index, i + 1).Style.Alignment.WrapText = true;
                            rngNumbers.Cell(index, i + 1).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            rngNumbers.Cell(index, i + 1).Style.Border.TopBorderColor = XLColor.Black;
                            rngNumbers.Cell(index, i + 1).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            rngNumbers.Cell(index, i + 1).Style.Border.BottomBorderColor = XLColor.Black;
                            rngNumbers.Cell(index, i + 1).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            rngNumbers.Cell(index, i + 1).Style.Border.LeftBorderColor = XLColor.Black;
                            rngNumbers.Cell(index, i + 1).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            rngNumbers.Cell(index, i + 1).Style.Border.RightBorderColor = XLColor.Black;
                        }
                    }
                    indexStart++;
                    index++;
                }
                #endregion
                workbook.SaveAs(pathNew);
                HttpContext.Current.Response.Clear();
                byte[] file = StreamToByteArray(pathNew);
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.‌​sheet";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + nameFileOutput);
                HttpContext.Current.Response.BinaryWrite(file);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                //HttpContext.Current.Response.Write(ex.ToString());
            }
            finally
            {
                File.Delete(pathNew);
            }
        }
        public void ExportExcelByNPOI(string serverPath, string tempOldPath, string newName, string nameFileOutput, string sheetName, List<ItemDanhMuc> listCell
           , int dtStartIndex, int excelStartColumnIndex, int excelEndColumnIndex, DataTable dt, List<ExcelEntity> lstColumn, bool have_stt
           , List<ItemDanhMuc> listBoldRows = null)
        {
            Guid strGuid = Guid.NewGuid();
            newName = strGuid.ToString() + newName;
            string tempNewPath = String.Format("{0}\\ExportTemps\\{1}", serverPath, newName);

            try
            {
                //copy from old path to new path
                System.IO.File.Copy(tempOldPath, tempNewPath, true);

                IWorkbook workbook = null;
                ISheet worksheet = null;
                using (FileStream stream = new FileStream(tempNewPath, FileMode.Open, FileAccess.Read))
                {
                    string Ext = System.IO.Path.GetExtension(tempNewPath); //<-get extention
                    switch (Ext.ToLower())
                    {
                        case ".xls":
                            workbook = new HSSFWorkbook(stream);
                            break;

                        case ".xlsx":
                            workbook = new XSSFWorkbook(stream);
                            break;
                    }
                }

                //npoi style format
                NPOIStyleFormat npoiStyleFormat = new NPOIStyleFormat(workbook);

                worksheet = workbook.GetSheet(sheetName); //<-get sheet by index
                if (worksheet == null)
                    worksheet = workbook.GetSheetAt(0);

                //Set header value
                int rowDataIndexStart = dtStartIndex;
                //foeach data row
                int sttValue = 1;

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        //create row item
                        IRow rowItem = worksheet.CreateRow(rowDataIndexStart);

                        int columnIndexStart = 0;
                        if (have_stt)
                        {
                            // add add cell stt
                            ICell cell = rowItem.CreateCell(columnIndexStart, CellType.String);
                            cell.SetCellValue(sttValue.ToString());
                            cell.CellStyle = npoiStyleFormat.AlignCenterCellStyle;

                            columnIndexStart++;
                            sttValue++;
                        }

                        //foreach list column
                        for (int i = 0; i < lstColumn.Count; i++)
                        {
                            ICell cell = null; //<-get cell           
                            string valCell = row[lstColumn[i].Name].ToString();
                            object cellValue = row[lstColumn[i].Name];

                            switch (lstColumn[i].Type)
                            {
                                case "String":
                                    cell = rowItem.CreateCell(columnIndexStart, CellType.String);
                                    cell.CellStyle.IsLocked = false;
                                    cell.CellStyle = npoiStyleFormat.GetCellStyle(lstColumn[i].AlignNPOI);
                                    if (cellValue != DBNull.Value)
                                    {
                                        cell.SetCellValue(Convert.ToString(cellValue));
                                    }
                                    else
                                        cell.SetCellValue(string.Empty);
                                    break;
                                case "Number":
                                    cell = rowItem.CreateCell(columnIndexStart, CellType.Numeric);
                                    cell.CellStyle.IsLocked = false;
                                    cell.CellStyle = npoiStyleFormat.GetCellStyle(lstColumn[i].AlignNPOI);
                                    if (cellValue != DBNull.Value)
                                    {
                                        cell.SetCellValue(Convert.ToInt32(cellValue));
                                    }
                                    else
                                        cell.SetCellValue(string.Empty);
                                    break;
                                case "Double":
                                case "Decimal":
                                    cell = rowItem.CreateCell(columnIndexStart, CellType.Numeric);
                                    cell.CellStyle.IsLocked = false;
                                    cell.CellStyle = npoiStyleFormat.GetCellStyle(lstColumn[i].AlignNPOI);
                                    if (cellValue != DBNull.Value)
                                    {
                                        cell.SetCellValue(Convert.ToDouble(cellValue));
                                    }
                                    else
                                        cell.SetCellValue(string.Empty);
                                    break;
                                case "Date":
                                    cell = rowItem.CreateCell(columnIndexStart, CellType.String);
                                    cell.CellStyle.IsLocked = false;
                                    cell.CellStyle = npoiStyleFormat.GetCellStyle(lstColumn[i].AlignNPOI);
                                    if (cellValue != DBNull.Value)
                                    {
                                        cell.SetCellValue(UtilsHelper.ParseDateStr(cellValue.ToString()));
                                    }
                                    else
                                        cell.SetCellValue(string.Empty);
                                    break;
                                case "DateTime":
                                    cell = rowItem.CreateCell(columnIndexStart, CellType.String);
                                    cell.CellStyle.IsLocked = false;
                                    cell.CellStyle = npoiStyleFormat.GetCellStyle(lstColumn[i].AlignNPOI);
                                    if (cellValue != DBNull.Value)
                                    {
                                        cell.SetCellValue(UtilsHelper.ParseDatetimeStr(cellValue.ToString()));
                                    }
                                    else
                                        cell.SetCellValue(string.Empty);
                                    break;
                                default:
                                    cell = rowItem.CreateCell(columnIndexStart, CellType.String);
                                    cell.CellStyle.IsLocked = false;
                                    cell.CellStyle = npoiStyleFormat.GetCellStyle(lstColumn[i].AlignNPOI);
                                    if (cellValue != DBNull.Value)
                                    {
                                        cell.SetCellValue(cellValue.ToString());
                                    }
                                    else
                                        cell.SetCellValue(string.Empty);
                                    break;

                            }
                            columnIndexStart++;
                        }
                        //empty cell
                        for (int i = excelStartColumnIndex + lstColumn.Count + 1; i <= excelEndColumnIndex; i++)
                        {
                            ICell cell = null; //<-get cell      
                            cell = rowItem.CreateCell(i, CellType.String);
                            cell.SetCellValue(string.Empty);
                            cell.CellStyle = npoiStyleFormat.GetCellStyle();
                            cell.CellStyle.IsLocked = false;
                        }

                        rowDataIndexStart++;
                    }
                }

                //list static cell 
                if (listCell != null)
                {
                    foreach (var staticCell in listCell)
                    {
                        //create row item
                        if (staticCell.RowIndex < 0 || staticCell.CellIndex < 0)
                        {
                            continue;
                        }

                        IRow rowItem = worksheet.GetRow(staticCell.RowIndex);
                        if (rowItem != null)
                        {
                            var cell = rowItem.GetCell(staticCell.CellIndex);
                            if (cell != null)
                            {
                                cell.CellStyle.IsLocked = false;
                                cell.SetCellValue(staticCell.Text);
                            }
                        }
                    }
                }

                //list bold rows
                if (listBoldRows != null)
                {
                    foreach (var rowBold in listBoldRows)
                    {
                        //create row item
                        if (rowBold.RowIndex < 0 || rowBold.CellIndex < 0)
                        {
                            continue;
                        }

                        IRow rowItem = worksheet.GetRow(rowBold.RowIndex);
                        if (rowItem != null)
                        {
                            for (var index = 0; index <= rowBold.CellIndex; index++)
                            {
                                var cell = rowItem.GetCell(index);
                                if (cell != null)
                                {
                                    var newCellStyle = npoiStyleFormat.NewCellStyle;
                                    newCellStyle.CloneStyleFrom(cell.CellStyle);
                                    newCellStyle.SetFont(npoiStyleFormat.BoldFont);

                                    cell.CellStyle = newCellStyle;
                                }
                            }
                        }
                    }
                }

                //write path
                using (FileStream stream = new FileStream(tempNewPath, FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(stream);
                }


                HttpContext.Current.Response.Clear();
                byte[] file = StreamToByteArray(tempNewPath);
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.‌​sheet";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + nameFileOutput);
                HttpContext.Current.Response.BinaryWrite(file);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                File.Delete(tempNewPath);
            }
        }
        private DataTable Excel_To_DataTable(string filePath, int pHojaIndex)
        {
            DataTable Tabla = null;
            try
            {
                if (System.IO.File.Exists(filePath))
                {

                    IWorkbook workbook = null;  //IWorkbook determina si es xls o xlsx              
                    ISheet worksheet = null;
                    string first_sheet_name = "";

                    using (FileStream FS = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
                    {
                        workbook = WorkbookFactory.Create(FS);          //Abre tanto XLS como XLSX
                        worksheet = workbook.GetSheetAt(pHojaIndex);    //Obtener Hoja por indice
                        first_sheet_name = worksheet.SheetName;         //Obtener el nombre de la Hoja

                        Tabla = new DataTable(first_sheet_name);
                        Tabla.Rows.Clear();
                        Tabla.Columns.Clear();

                        // Leer Fila por fila desde la primera
                        for (int rowIndex = 0; rowIndex <= worksheet.LastRowNum; rowIndex++)
                        {
                            DataRow NewReg = null;
                            IRow row = worksheet.GetRow(rowIndex);
                            IRow row2 = null;
                            IRow row3 = null;

                            if (rowIndex == 0)
                            {
                                row2 = worksheet.GetRow(rowIndex + 1); //Si es la Primera fila, obtengo tambien la segunda para saber el tipo de datos
                                row3 = worksheet.GetRow(rowIndex + 2); //Y la tercera tambien por las dudas
                            }

                            if (row != null) //null is when the row only contains empty cells 
                            {
                                if (rowIndex > 0) NewReg = Tabla.NewRow();

                                int colIndex = 0;
                                //Leer cada Columna de la fila
                                foreach (ICell cell in row.Cells)
                                {
                                    object valorCell = null;
                                    string cellType = "";
                                    string[] cellType2 = new string[2];

                                    if (rowIndex == 0) //Asumo que la primera fila contiene los titlos:
                                    {
                                        for (int i = 0; i < 2; i++)
                                        {
                                            ICell cell2 = null;
                                            if (i == 0) { cell2 = row2.GetCell(cell.ColumnIndex); }
                                            else { cell2 = row3.GetCell(cell.ColumnIndex); }

                                            if (cell2 != null)
                                            {
                                                switch (cell2.CellType)
                                                {
                                                    case CellType.Blank: break;
                                                    case CellType.Boolean: cellType2[i] = "System.Boolean"; break;
                                                    case CellType.String: cellType2[i] = "System.String"; break;
                                                    case CellType.Numeric:
                                                        if (HSSFDateUtil.IsCellDateFormatted(cell2)) { cellType2[i] = "System.DateTime"; }
                                                        else
                                                        {
                                                            cellType2[i] = "System.Double";  //valorCell = cell2.NumericCellValue;
                                                        }
                                                        break;

                                                    case CellType.Formula:
                                                        bool continuar = true;
                                                        switch (cell2.CachedFormulaResultType)
                                                        {
                                                            case CellType.Boolean: cellType2[i] = "System.Boolean"; break;
                                                            case CellType.String: cellType2[i] = "System.String"; break;
                                                            case CellType.Numeric:
                                                                if (HSSFDateUtil.IsCellDateFormatted(cell2)) { cellType2[i] = "System.DateTime"; }
                                                                else
                                                                {
                                                                    try
                                                                    {
                                                                        //DETERMINAR SI ES BOOLEANO
                                                                        if (cell2.CellFormula == "TRUE()") { cellType2[i] = "System.Boolean"; continuar = false; }
                                                                        if (continuar && cell2.CellFormula == "FALSE()") { cellType2[i] = "System.Boolean"; continuar = false; }
                                                                        if (continuar) { cellType2[i] = "System.Double"; continuar = false; }
                                                                    }
                                                                    catch { }
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    default:
                                                        cellType2[i] = "System.String"; break;
                                                }
                                            }
                                        }

                                        //Resolver las diferencias de Tipos
                                        if (cellType2[0] == cellType2[1]) { cellType = cellType2[0]; }
                                        else
                                        {
                                            if (cellType2[0] == null) cellType = cellType2[1];
                                            if (cellType2[1] == null) cellType = cellType2[0];
                                            if (cellType == "") cellType = "System.String";
                                        }

                                        //Obtener el nombre de la Columna
                                        string colName = "Column_{0}";
                                        try { colName = cell.StringCellValue; }
                                        catch { colName = string.Format(colName, colIndex); }

                                        //Verificar que NO se repita el Nombre de la Columna
                                        foreach (DataColumn col in Tabla.Columns)
                                        {
                                            if (col.ColumnName == colName) colName = string.Format("{0}_{1}", colName, colIndex);
                                        }

                                        //Agregar el campos de la tabla:
                                        DataColumn codigo = new DataColumn(colName, System.Type.GetType(cellType));
                                        Tabla.Columns.Add(codigo); colIndex++;
                                    }
                                    else
                                    {
                                        //Las demas filas son registros:
                                        switch (cell.CellType)
                                        {
                                            case CellType.Blank: valorCell = DBNull.Value; break;
                                            case CellType.Boolean: valorCell = cell.BooleanCellValue; break;
                                            case CellType.String: valorCell = cell.StringCellValue; break;
                                            case CellType.Numeric:
                                                if (HSSFDateUtil.IsCellDateFormatted(cell)) { valorCell = cell.DateCellValue; }
                                                else { valorCell = cell.NumericCellValue; }
                                                break;
                                            case CellType.Formula:
                                                switch (cell.CachedFormulaResultType)
                                                {
                                                    case CellType.Blank: valorCell = DBNull.Value; break;
                                                    case CellType.String: valorCell = cell.StringCellValue; break;
                                                    case CellType.Boolean: valorCell = cell.BooleanCellValue; break;
                                                    case CellType.Numeric:
                                                        if (HSSFDateUtil.IsCellDateFormatted(cell)) { valorCell = cell.DateCellValue; }
                                                        else { valorCell = cell.NumericCellValue; }
                                                        break;
                                                }
                                                break;
                                            default: valorCell = cell.StringCellValue; break;
                                        }
                                        //Agregar el nuevo Registro
                                        if (cell.ColumnIndex <= Tabla.Columns.Count - 1) NewReg[cell.ColumnIndex] = valorCell;
                                    }
                                }
                            }
                            if (rowIndex > 0) Tabla.Rows.Add(NewReg);
                        }
                        Tabla.AcceptChanges();
                    }
                }
                else
                {
                    throw new Exception("ERROR 404: El archivo especificado NO existe.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Tabla;
        }
        #endregion
        public DateTime? ConvertDDMMYYToDateTime(string value)
        {
            try
            {
                return DateTime.ParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch { }
            return null;
        }
        public string catChuoi(string noi_dung, int length)
        {
            string result = "";
            if (!string.IsNullOrEmpty(noi_dung))
            {
                if (noi_dung.Length > length) result = noi_dung.Substring(0, length) + "...";
                else result = noi_dung;
            }
            else return "";
            return result;
        }
        public object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
        public object SetPropValue(object src, string propName, object value)
        {
            src.GetType().GetProperty(propName).SetValue(src, value, null);
            return src;
        }
        public static String NumberToTextVN(decimal total)
        {
            try
            {
                string rs = "";

                total = Math.Round(total, 0);
                string[] ch = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
                string[] rch = { "lẻ", "mốt", "", "", "", "lăm" };
                string[] u = { "", "mươi", "trăm", "ngàn", "", "", "triệu", "", "", "tỷ", "", "", "ngàn", "", "", "triệu" };
                string nstr = total.ToString();

                int[] n = new int[nstr.Length];
                int len = n.Length;
                for (int i = 0; i < len; i++)
                {
                    n[len - 1 - i] = Convert.ToInt32(nstr.Substring(i, 1));
                }

                for (int i = len - 1; i >= 0; i--)
                {
                    if (i % 3 == 2)// số 0 ở hàng trăm
                    {
                        if (n[i] == 0 && n[i - 1] == 0 && n[i - 2] == 0) continue;//nếu cả 3 số là 0 thì bỏ qua không đọc
                    }
                    else if (i % 3 == 1) // số ở hàng chục
                    {
                        if (n[i] == 0)
                        {
                            if (n[i - 1] == 0) { continue; }// nếu hàng chục và hàng đơn vị đều là 0 thì bỏ qua.
                            else
                            {
                                rs += " " + rch[n[i]]; continue;// hàng chục là 0 thì bỏ qua, đọc số hàng đơn vị
                            }
                        }
                        if (n[i] == 1)//nếu số hàng chục là 1 thì đọc là mười
                        {
                            rs += " mười"; continue;
                        }
                    }
                    else if (i != len - 1)// số ở hàng đơn vị (không phải là số đầu tiên)
                    {
                        if (n[i] == 0)// số hàng đơn vị là 0 thì chỉ đọc đơn vị
                        {
                            if (i + 2 <= len - 1 && n[i + 2] == 0 && n[i + 1] == 0) continue;
                            rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);
                            continue;
                        }
                        if (n[i] == 1)// nếu là 1 thì tùy vào số hàng chục mà đọc: 0,1: một / còn lại: mốt
                        {
                            rs += " " + ((n[i + 1] == 1 || n[i + 1] == 0) ? ch[n[i]] : rch[n[i]]);
                            rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);
                            continue;
                        }
                        if (n[i] == 5) // cách đọc số 5
                        {
                            if (n[i + 1] != 0) //nếu số hàng chục khác 0 thì đọc số 5 là lăm
                            {
                                rs += " " + rch[n[i]];// đọc số 
                                rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);// đọc đơn vị
                                continue;
                            }
                        }
                    }

                    rs += (rs == "" ? " " : ", ") + ch[n[i]];// đọc số
                    rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);// đọc đơn vị
                }
                if (rs[rs.Length - 1] != ' ')
                    rs += " đồng";
                else
                    rs += "đồng";

                if (rs.Length > 2)
                {
                    string rs1 = rs.Substring(0, 2);
                    rs1 = rs1.ToUpper();
                    rs = rs.Substring(2);
                    rs = rs1 + rs;
                }
                return rs.Trim().Replace("lẻ,", "lẻ").Replace("mươi,", "mươi").Replace("trăm,", "trăm").Replace("mười,", "mười");
            }
            catch
            {
                return "";
            }

        }

        public void ghilog(string nameFile, string msg)
        {
            try
            {
                string path = "";
                DateTime dt = new DateTime();
                dt = DateTime.Now;
                string foldername = dt.ToString();
                path = urlLogEdu() + dt.Year.ToString() + dt.Month.ToString() + dt.Day.ToString() + dt.Hour.ToString() + nameFile + ".txt";
                using (StreamWriter sw = new StreamWriter(path, true))
                {
                    string str = DateTime.Now.ToString() + ": " + msg;
                    sw.WriteLine(str);
                }
            }
            catch
            {

            }
        }
        public string urlLogEdu()
        {
            string value = "";
            try
            {
                value = System.Configuration.ConfigurationManager.AppSettings["urlLogEdu"];
            }
            catch { }
            return value;
        }

        public void getBrandnameAndCp(TRUONG truongModel, string loai_nha_mang, out string brandname, out string cp)
        {
            if (truongModel != null)
            {
                if (loai_nha_mang == "Viettel")
                {
                    brandname = truongModel.BRAND_NAME_VIETTEL;
                    cp = truongModel.CP_VIETTEL;
                }
                else if (loai_nha_mang == "GMobile")
                {
                    brandname = truongModel.BRAND_NAME_GTEL;
                    cp = truongModel.CP_GTEL;
                }
                else if (loai_nha_mang == "MobiFone")
                {
                    brandname = truongModel.BRAND_NAME_MOBI;
                    cp = truongModel.CP_MOBI;
                }
                else if (loai_nha_mang == "VinaPhone")
                {
                    brandname = truongModel.BRAND_NAME_VINA;
                    cp = truongModel.CP_VINA;
                }
                else if (loai_nha_mang == "VietnamMobile")
                {
                    brandname = truongModel.BRAND_NAME_VNM;
                    cp = truongModel.CP_VNM;
                }
                else
                {
                    brandname = "";
                    cp = "";
                }
            }
            else
            {
                brandname = "";
                cp = "";
            }
        }
    }

    public class NPOIStyleFormat
    {
        IWorkbook workbook;
        private ICellStyle _alignLeftCellStyle;
        private ICellStyle _alignRightCellStyle;
        private ICellStyle _alignCenterCellStyle;
        private ICellStyle _boldCellStyle;
        public NPOIStyleFormat(IWorkbook _workbook)
        {
            this.workbook = _workbook;
        }
        public ICellStyle GetCellStyle(HorizontalAlignment alignment = HorizontalAlignment.Left)
        {

            switch (alignment)
            {
                case HorizontalAlignment.Left:
                    return AlignLeftCellStyle;
                case HorizontalAlignment.Right:
                    return AlignRightCellStyle;
                case HorizontalAlignment.Center:
                    return AlignCenterCellStyle;
            }

            //basic style
            return GetBasicStyle();
        }
        public ICellStyle AlignLeftCellStyle
        {
            get
            {
                if (_alignLeftCellStyle != null)
                    return _alignLeftCellStyle;
                else
                {
                    _alignLeftCellStyle = GetBasicStyle();
                    _alignLeftCellStyle.Alignment = HorizontalAlignment.Left;
                    _alignLeftCellStyle.SetFont(CellFont);
                }
                return _alignLeftCellStyle;
            }
        }
        public ICellStyle AlignRightCellStyle
        {
            get
            {
                if (_alignRightCellStyle != null)
                    return _alignRightCellStyle;
                else
                {
                    _alignRightCellStyle = GetBasicStyle();
                    _alignRightCellStyle.Alignment = HorizontalAlignment.Right;
                    _alignRightCellStyle.SetFont(CellFont);
                }
                return _alignRightCellStyle;
            }
        }
        public ICellStyle AlignCenterCellStyle
        {
            get
            {
                if (_alignCenterCellStyle != null)
                    return _alignCenterCellStyle;
                else
                {
                    _alignCenterCellStyle = GetBasicStyle();
                    _alignCenterCellStyle.Alignment = HorizontalAlignment.Center;
                    _alignCenterCellStyle.SetFont(CellFont);
                }
                return _alignCenterCellStyle;
            }
        }
        public IFont _boldFont { get; set; }
        public IFont BoldFont
        {
            get
            {
                if (_boldFont != null)
                    return _boldFont;
                else
                {
                    IFont font = workbook.CreateFont();
                    font.Boldweight = (short)FontBoldWeight.Bold;
                    font.FontName = "Times New Roman";
                    _boldFont = font;
                    return _boldFont;
                }
            }
        }
        public IFont _cellFont { get; set; }
        public IFont CellFont
        {
            get
            {
                if (_cellFont != null)
                    return _cellFont;
                else
                {
                    IFont font = workbook.CreateFont();
                    font.FontName = "Times New Roman";
                    _cellFont = font;
                    return _cellFont;
                }
            }
        }
        public ICellStyle NewCellStyle
        {
            get
            {
                ICellStyle _CellStyle = workbook.CreateCellStyle();
                _CellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                _CellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                _CellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                _CellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                _CellStyle.SetFont(CellFont);
                return _CellStyle;
            }
        }
        private ICellStyle GetBasicStyle()
        {
            ICellStyle _CellStyle = workbook.CreateCellStyle();
            _CellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            _CellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            _CellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            _CellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            _CellStyle.SetFont(CellFont);

            return _CellStyle;
        }
    }
}