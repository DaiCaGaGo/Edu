using OneEduDataAccess;
using OneEduDataAccess.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS
{
    public partial class DataMaNXHN : AuthenticatePage
    {
        MaNhanXetBO maNXBO = new MaNhanXetBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            List<DataJsonEntity> lst = new List<DataJsonEntity>();
            lst = maNXBO.getMaNhanXetToDataJson(Sys_This_Truong.ID);
            var json = new JavaScriptSerializer().Serialize(lst);
            Response.Write(json.ToString());
        }
    }
}