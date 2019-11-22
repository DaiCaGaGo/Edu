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
    public partial class StudentInfor : System.Web.UI.Page
    {
        public List<DiemChiTietTheoHocSinhEntity> lstDiemChiTiet = new List<DiemChiTietTheoHocSinhEntity>();
        HocSinhBO hsBO = new HocSinhBO();
        LopBO lopBO = new LopBO();
        NhanXetHangNgayBO nxhnBO = new NhanXetHangNgayBO();
        long? id_hs;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("id_hs") != null)
            {
                try
                {
                    id_hs = Convert.ToInt64(Request.QueryString.Get("id_hs"));
                }
                catch (Exception ex) { }
            }
            if (!IsPostBack)
            {
                if (id_hs != null)
                {
                    int id_nam_hoc = Convert.ToInt16(DateTime.Now.Year);
                    int hoc_ky = 1;
                    int thang = DateTime.Now.Month;
                    if (thang >= 1 && thang < 8)
                    {
                        id_nam_hoc = id_nam_hoc - 1;
                        hoc_ky = 2;
                    }
                    HOC_SINH hocSinh = hsBO.getHocSinhByID(id_hs.Value);
                    if (hocSinh != null)
                    {
                        lbHoTen.Text = hocSinh.HO_TEN;
                        lbLop.Text = lopBO.getLopById(hocSinh.ID_LOP).TEN;
                        #region kqht
                        lstDiemChiTiet = hsBO.getDiemChiTietByHocSinh(hocSinh.ID_TRUONG, hocSinh.MA_CAP_HOC, Convert.ToInt16(id_nam_hoc), hocSinh.ID_LOP, 2, id_hs.Value);
                        #endregion
                    }
                }
            }
        }
    }
}