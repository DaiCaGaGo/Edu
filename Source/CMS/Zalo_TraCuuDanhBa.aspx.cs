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
    public partial class Zalo_TraCuuDanhBa : System.Web.UI.Page
    {
        LopBO lopBO = new LopBO();
        GiaoVienBO giaoVienBO = new GiaoVienBO();
        LopMonBO lopMonBO = new LopMonBO();
        MonHocTruongBO monTruongBO = new MonHocTruongBO();
        HocSinhBO hocSinhBO = new HocSinhBO();
        long? id_lop;
        int? chph;
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
            if (Request.QueryString.Get("chph") != null)
            {
                try
                {
                    chph = Convert.ToInt16(Request.QueryString.Get("chph"));
                }
                catch (Exception ex) { }
            }
            if (id_lop != null)
            {
                int id_nam_hoc = Convert.ToInt16(DateTime.Now.Year);
                int hoc_ky = 1;
                int thang = DateTime.Now.Month;
                if (thang >= 1 && thang < 8)
                {
                    id_nam_hoc = id_nam_hoc - 1;
                    hoc_ky = 2;
                }
                LOP lop = lopBO.getLopById(id_lop.Value);
                if (lop != null)
                {
                    if (chph != null && chph == 1)
                    {
                        #region Phụ huynh
                        divGV.Visible = false;
                        divCHPH.Visible = true;
                        lblTitle.Text = "Danh sách Chi hội Phụ huynh lớp " + lop.TEN;
                        RadGrid1.DataSource = hocSinhBO.getChiHoiPhuHuynhByLop(id_lop.Value);
                        RadGrid1.DataBind();
                        #endregion
                    }
                    else
                    {
                        divGV.Visible = true;
                        divCHPH.Visible = false;
                        lblTitle.Text = "Danh sách Giáo viên lớp " + lop.TEN;
                        #region GVCN
                        if (lop.ID_GVCN != null)
                        {
                            GIAO_VIEN giaoVien = giaoVienBO.getGiaoVienByID(lop.ID_GVCN.Value);
                            if (giaoVien != null)
                            {
                                //lblGVCN.Text = "GVCN: " + (giaoVien.HO_TEN) + (!string.IsNullOrEmpty(giaoVien.SDT) ? ": " + giaoVien.SDT : "");
                                ltrGoiGVCN.Text = "Giáo viên chủ nhiệm: " + (giaoVien.HO_TEN) + (!string.IsNullOrEmpty(giaoVien.SDT) ? ": " + giaoVien.SDT : "") + "<a href='tel: " + giaoVien.SDT + " ' onclick='_gaq.push(['_trackEvent', 'Contact', 'Call Now Button', 'Phone']);' id='callnowbutton' style='display: block; height: 100px; width: 100px; background: url(https://edu.onesms.vn/img/calling.jpg) center 0px no-repeat; text-decoration: none; background-size: 100% 100%;'></a>";
                            }
                        }
                        #endregion
                        #region GV bo mon
                        List<GiaoVienEntity> lst = giaoVienBO.getGiaoVienBoMonByLop(id_lop.Value, Convert.ToInt16(hoc_ky));
                        if (lst.Count > 0)
                        {
                            RadGrid2.Visible = true;
                        }
                        else RadGrid2.Visible = false;
                        RadGrid2.DataSource = lst;
                        RadGrid2.DataBind();
                        #endregion
                    }
                }
            }
        }
    }
}