using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS
{
    public partial class TraCuuDanhBaGV : System.Web.UI.Page
    {
        LopBO lopBO = new LopBO();
        GiaoVienBO giaoVienBO = new GiaoVienBO();
        long? id_lop;
        public List<GiaoVienEntity> lstGVBM = new List<GiaoVienEntity>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("id_lop") != null)
            {
                try
                {
                    id_lop = Convert.ToInt64(Request.QueryString.Get("id_lop"));
                }
                catch (Exception ex) { }
            }
            if (id_lop != null)
            {
                LOP lop = lopBO.getLopById(id_lop.Value);
                if (lop != null)
                {
                    lblDanhBaGV.Text = "Danh bạ giáo viên lớp " + lop.TEN;
                    int id_nam_hoc = Convert.ToInt16(DateTime.Now.Year);
                    int hoc_ky = 1;
                    int thang = DateTime.Now.Month;
                    if (thang >= 1 && thang < 8)
                    {
                        id_nam_hoc = id_nam_hoc - 1;
                        hoc_ky = 2;
                    }
                    if (lop.ID_GVCN != null)
                    {
                        GIAO_VIEN giaoVien = giaoVienBO.getGiaoVienByID(lop.ID_GVCN.Value);
                        if (giaoVien != null)
                        {
                            ltrGoiGVCN.Text = "<p><b>Giáo viên chủ nhiệm:</b> " + (giaoVien.HO_TEN) + (!string.IsNullOrEmpty(giaoVien.SDT) ? ": " + giaoVien.SDT : "") + "<a href='tel: " + giaoVien.SDT + " ' onclick='_gaq.push(['_trackEvent', 'Contact', 'Call Now Button', 'Phone']);' id='callnowbutton' style='display: block; height: 100px; width: 100px; background: url(https://edu.onesms.vn/img/calling.jpg) center 0px no-repeat; text-decoration: none; background-size: 100% 100%;display: inline-block;padding-left: 12px;margin-left: 9px;width: 33px;height: 34px;border-radius: 8px; '></a></p>";
                        }
                    }
                    lstGVBM = giaoVienBO.getGiaoVienBoMonByLop(id_lop.Value, Convert.ToInt16(hoc_ky));
                }
            }
        }
    }
}