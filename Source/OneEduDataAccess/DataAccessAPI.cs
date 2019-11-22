using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class DataAccessAPI
    {
        public DataAccessAPI() { }
        public int getThisWeek()
        {
            DateTime dt = new DateTime();
            dt = DateTime.Now;
            int thuHienTai = (int)dt.DayOfWeek;
            int soChia = (dt.Day + thuHienTai - 1) / 7;
            int soDu = (dt.Day + thuHienTai - 1) % 7;
            int tuan_hien_tai = soChia;
            if (soDu > 0) tuan_hien_tai++;
            return tuan_hien_tai;
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

        public object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        public object SetPropValue(object src, string propName, object value)
        {
            src.GetType().GetProperty(propName).SetValue(src, value, null);
            return src;
        }

        public string ConvertDecimalToCD(decimal? value)
        {
            if (value != null && value == 0) return "CĐ";
            if (value != null && value == 1) return "Đ";
            return "";
        }

        public decimal? ConvertCDToDecimal(string value)
        {
            value = value.Trim();
            if (value.Length > 1) return 0;
            if (value.Length == 1) return 1;
            return null;
        }

        public string ConvertToCoKhong(object obj, string giatrisai = "Không", string giatridung = "Có", string giatrinull = "")
        {
            try
            {
                if (obj == null) return giatrinull;
                if (obj.ToString() == "1" || obj.ToString().ToLower() == "true")
                    return giatridung;
                else return giatrisai;
            }
            catch { }
            return giatrisai;
        }

        public string ConvertRTC(object obj, string giatrinull = "")
        {
            try
            {
                if (obj == null) return giatrinull;
                if (obj.ToString() == "3")
                    return "Rất tích cực";
                else if (obj.ToString() == "2")
                    return "Tích cực";
                else if (obj.ToString() == "1")
                    return "Chưa tích cực";
                else return giatrinull;
            }
            catch { }
            return giatrinull;
        }

        public int ConvertTrueFalseTo10(object obj)
        {
            try
            {
                if (obj == null) return 0;
                if (obj.ToString().ToLower() == "true" || obj.ToString() == "1")
                    return 1;
                else return 0;
            }
            catch { }
            return 0;
        }

        public bool Convert10ToTrueFalse(object obj)
        {
            try
            {
                if (obj == null) return false;
                if (obj.ToString() == "1" || obj.ToString().ToLower() == "true")
                    return true;
                else return false;
            }
            catch { }
            return false;
        }
        public string ConvertDiemToString(decimal? value, long id_mon_hoc_truong)
        {
            if (value == null) return "";
            MonHocTruongBO monHocTruongBO = new MonHocTruongBO();
            MON_HOC_TRUONG dmmon = new MON_HOC_TRUONG();
            dmmon = monHocTruongBO.getMonTruongByID(id_mon_hoc_truong);
            if (dmmon != null)
            {
                if (dmmon.KIEU_MON == null || (dmmon.KIEU_MON != null && dmmon.KIEU_MON == false))
                {
                    return DoFormat(value.Value);
                }
                else
                {
                    return ConvertDecimalToCD(value);
                }
            }
            return value.ToString();
        }

        public string DoFormat(decimal myNumber)
        {
            var s = string.Format("{0:0.##}", myNumber);

            if (s.EndsWith("00"))
            {
                return ((int)myNumber).ToString();
            }
            else
            {
                return s;
            }
        }
        public string ConvertListToString<T>(List<T> lstValue, string ky_tu)
        {
            string value = "";
            foreach (var item in lstValue)
            {
                if (string.IsNullOrEmpty(value))
                    value += item;
                else value += ky_tu + item;
            }
            return value;
        }
        public string getLoaiNhaMang(string phone)
        {
            if (!string.IsNullOrEmpty(phone))
            {
                string a = "";
                string b = "";
                List<string> lst_viettel = new List<string> { "086", "096", "097", "098", "032", "033", "034", "035", "036", "037", "038", "039" };
                List<string> lst_mobi = new List<string> { "089", "090", "093", "070", "079", "077", "076", "078" };
                List<string> lst_vina = new List<string> { "088", "091", "094", "083", "084", "085", "081", "082" };
                List<string> lst_vnMobile = new List<string> { "092", "052", "056", "058" };
                List<string> lst_gMobile = new List<string> { "099", "059" };
                //List<string> lst_sPhone = new List<string> { "095" };

                if (phone.Length > 5)
                {
                    if (phone.IndexOf("84") == 0)
                    {
                        a = "0" + phone.Substring(2, 2);
                        b = "0" + phone.Substring(2, 3);
                    }
                    else
                    {
                        a = phone.Substring(0, 3);
                        b = phone.Substring(0, 4);
                    }
                }
                if (phone.IndexOf("84") == 0)
                {
                    if (phone.Length == 11) phone = "0" + phone.Substring(0, 9);
                    if (phone.Length == 12) phone = "0" + phone.Substring(0, 10);
                }
                if (lst_viettel.Contains(a) || lst_viettel.Contains(b))
                    return "Viettel";
                if (lst_mobi.Contains(a) || lst_mobi.Contains(b))
                    return "MobiFone";
                if (lst_vina.Contains(a) || lst_vina.Contains(b))
                    return "VinaPhone";
                if (lst_vnMobile.Contains(a) || lst_vnMobile.Contains(b))
                    return "VietnamMobile";
                if (lst_gMobile.Contains(a) || lst_gMobile.Contains(b))
                    return "GMobile";
                //if (lst_sPhone.Contains(a) || lst_sPhone.Contains(b))
                //    return "SPhone";
                return "";
            }
            else
            {
                return "";
            }
        }
        public string sqlRemoveInjection(string strValue)
        {
            return strValue.Replace("'", "").Replace("--", "");
        }
        public string removeLastString(string text, string character)
        {
            if (text.Length < 1) return text;
            return text.Remove(text.ToString().LastIndexOf(character), character.Length);
        }
    }
}
