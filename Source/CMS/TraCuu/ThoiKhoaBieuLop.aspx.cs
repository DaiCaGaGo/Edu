using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS.TraCuu
{
    public partial class ThoiKhoaBieuLop : System.Web.UI.Page
    {
        LopBO lopBO = new LopBO();
        CaHocBO caHocBO = new CaHocBO();
        LocalAPI localAPI = new LocalAPI();
        long? id_lop;
        public List<ThoiKhoaBieuLopEntity> listTKB = new List<ThoiKhoaBieuLopEntity>();
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
                    lblLop.Text = "Thời khóa biểu lớp " + lop.TEN;
                    int id_nam_hoc = Convert.ToInt16(DateTime.Now.Year);
                    int hoc_ky = 1;
                    int thang = DateTime.Now.Month;
                    if (thang >= 1 && thang < 9)
                    {
                        id_nam_hoc = id_nam_hoc - 1;
                        hoc_ky = 2;
                    }
                    listTKB = caHocBO.getThoiKhoaBieuLop(lop.ID_TRUONG, Convert.ToInt16(id_nam_hoc), Convert.ToInt16(hoc_ky), id_lop.Value);
                }
            }
        }
    }
}