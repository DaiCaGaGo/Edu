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
    public partial class TraCuuDanhBaPhuHuynh : System.Web.UI.Page
    {
        LopBO lopBO = new LopBO();
        HocSinhBO hocSinhBO = new HocSinhBO();
        long? id_lop;
        public List<PhuHuynhHocSinhEntity> lstDanhBaPhuHuynh = new List<PhuHuynhHocSinhEntity>();
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
                    lblDanhBaPH.Text = "Danh bạ phụ huynh lớp " + lop.TEN;
                    lstDanhBaPhuHuynh = hocSinhBO.getChiHoiPhuHuynhByLop(id_lop.Value);
                }
            }
        }
    }
}