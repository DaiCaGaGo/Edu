using CMS.XuLy;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS
{
    public class SYS_Profile
    {
        public TRUONG getThisTruong()
        {
            try
            {
                if (HttpContext.Current.Session["TRUONG"] == null)
                    return null;
                else return HttpContext.Current.Session["TRUONG"] as TRUONG;
            }
            catch
            {
                return null;
            }
        }
        public void setThisTruong(TRUONG detail)
        {
            HttpContext.Current.Session["TRUONG"] = detail;
        }

        public string getCapHoc()
        {
            try
            {
                if (HttpContext.Current.Session["CAP_HOC"] == null)
                    return null;
                else return HttpContext.Current.Session["CAP_HOC"] as string;
            }
            catch
            {
                return null;
            }
        }
        public void setLoaiLopGDTX(string detail)
        {
            HttpContext.Current.Session["LOAI_LOP_GDTX"] = detail;
        }
        public string getLoaiLopGDTX()
        {
            try
            {
                if (HttpContext.Current.Session["LOAI_LOP_GDTX"] == null)
                    return null;
                else return HttpContext.Current.Session["LOAI_LOP_GDTX"] as string;
            }
            catch
            {
                return null;
            }
        }
        public void setCapHoc(string detail)
        {
            HttpContext.Current.Session["CAP_HOC"] = detail;
        }

        public int getMaNamHoc()
        {
            try
            {
                LocalAPI localAPI = new LocalAPI();
                if (HttpContext.Current.Session["MA_NAM_HOC"] == null)
                {
                    int yearNow = localAPI.GetYearNow();
                    HttpContext.Current.Session["MA_NAM_HOC"] = yearNow;
                    return yearNow;
                }
                else return (int)HttpContext.Current.Session["MA_NAM_HOC"];
            }
            catch
            {
                return 0;
            }
        }
        public string getTenNamHoc()
        {
            int ma_year = getMaNamHoc();
            string ten_year = ma_year + "-" + (ma_year + 1);
            return ten_year;
        }
        public void setNamHoc(int nam_hoc)
        {
            HttpContext.Current.Session["MA_NAM_HOC"] = nam_hoc;
        }
        public int getHocKy()
        {
            try
            {
                LocalAPI localAPI = new LocalAPI();
                if (HttpContext.Current.Session["HOC_KY"] == null)
                {
                    int hocKy = localAPI.GetKyNow();
                    HttpContext.Current.Session["HOC_KY"] = hocKy;
                    return hocKy;
                }
                else return (int)HttpContext.Current.Session["HOC_KY"];
            }
            catch
            {
                return 1;
            }
        }
        public void setHocKy(int hoc_ky)
        {
            HttpContext.Current.Session["HOC_KY"] = hoc_ky;
        }

        public string getThongBao()
        {
            SYS_Cookies sYS_Cookies = new SYS_Cookies();
            string msg = sYS_Cookies.getCookie("ERROR_MSG");
            sYS_Cookies.setCookie("ERROR_MSG", "");
            return msg;
        }
        public void setThongBao(string msg)
        {
            SYS_Cookies sYS_Cookies = new SYS_Cookies();
            sYS_Cookies.setCookie("ERROR_MSG", msg);
        }
        public void Clear_Profile()
        {
            HttpContext.Current.Session["TRUONG"] = null;
            HttpContext.Current.Session["CAP_HOC"] = null;
            HttpContext.Current.Session["ERROR_MSG"] = null;
            HttpContext.Current.Session["MA_NAM_HOC"] = null;
            HttpContext.Current.Session["HOC_KY"] = null;
            SYS_Cookies sYS_Cookies = new SYS_Cookies();
            sYS_Cookies.clearCookie();
        }
    }
}