using OneEduDataAccess;
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
    public partial class ViewThongBao : System.Web.UI.Page
    {
        TruongBO truongBO = new TruongBO();
        ThongBaoNhaTruongBO thongBaoNhaTruongBO = new ThongBaoNhaTruongBO();
        long? id_truong; string ma_cap_hoc; short? loai_tb;
        public string[] listImage;

        public List<ThongBaoNhaTruongEntity> listThongBao = new List<ThongBaoNhaTruongEntity>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("id_truong") != null)
            {
                try
                {
                    id_truong = Convert.ToInt64(Request.QueryString.Get("id_truong"));
                }
                catch { }
            }
            if (Request.QueryString.Get("loai_tb") != null)
            {
                try
                {
                    ma_cap_hoc = Convert.ToString(Request.QueryString.Get("ma_cap_hoc"));
                }
                catch { }
            }
            if (Request.QueryString.Get("ma_cap_hoc") != null)
            {
                try
                {
                    loai_tb = Convert.ToInt16(Request.QueryString.Get("loai_tb"));
                }
                catch { }
            }
            if (!IsPostBack)
            {
                if (id_truong != null)
                {
                    int id_nam_hoc = Convert.ToInt16(DateTime.Now.Year);
                    int thang = DateTime.Now.Month;
                    if (thang >= 1 && thang < 8) id_nam_hoc = id_nam_hoc - 1;
                    List<THONG_BAO_NHA_TRUONG> listTB = thongBaoNhaTruongBO.getThongBaoByTruong(id_truong.Value, ma_cap_hoc, (Int16)id_nam_hoc, "", loai_tb);
                    foreach (THONG_BAO_NHA_TRUONG tb in listTB)
                    {
                        if (!string.IsNullOrEmpty(tb.ANH_NOI_DUNG))
                        {
                            string[] fileImage = tb.ANH_NOI_DUNG.Split(';');
                            listImage = fileImage;
                        }
                        ThongBaoNhaTruongEntity detail = new ThongBaoNhaTruongEntity();
                        detail.NOI_DUNG = tb.NOI_DUNG;
                        detail.LIST_IMAGE = listImage;
                        listThongBao.Add(detail);
                    }
                }
            }
        }
    }
}