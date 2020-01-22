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
    public partial class DanhSachGiaoVien : System.Web.UI.Page
    {
        TruongBO truongBO = new TruongBO();
        GiaoVienBO giaoVienBO = new GiaoVienBO();
        long? id_truong;
        public List<GIAO_VIEN> listGiaoVien = new List<GIAO_VIEN>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("id_truong") != null)
            {
                try
                {
                    id_truong = Convert.ToInt64(Request.QueryString.Get("id_truong"));
                }
                catch (Exception ex) { }
            }
            if (id_truong != null)
            {
                TRUONG truong = truongBO.getTruongById(id_truong.Value);
                if (truong != null)
                {
                    lblDanhBaGV.Text = "Danh bạ giáo viên trường " + truong.TEN;
                    listGiaoVien = giaoVienBO.getGiaoVienByTruong(id_truong.ToString());
                }
            }
        }
    }
}