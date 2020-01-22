using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS.TinTuc
{
    public partial class ViewPost : System.Web.UI.Page
    {
        long? id;

        TinTucBO tinTucBO = new TinTucBO();
        TruongBO truongBO = new TruongBO();
        TIN_TUC tinTuc = new TIN_TUC();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("id") != null)
            {
                try
                {
                    id = Convert.ToInt16(Request.QueryString.Get("id"));
                }
                catch { }
            }
            if (!IsPostBack)
            {
                if (id != null)
                {
                    tinTuc = tinTucBO.getTinTucByID(id.Value);
                    title.Text = tinTuc.TIEU_DE;
                    title_h1.InnerText = tinTuc.TIEU_DE;
                    ngaytao.InnerText = tinTuc.NGAY_TAO.ToString();
                    content.Text = tinTuc.NOI_DUNG;

                    schoolName.InnerText = truongBO.getTruongById(tinTuc.ID_TRUONG).TEN;
                }
            }
        }
    }
}