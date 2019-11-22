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
    public partial class DataMaNX : AuthenticatePage
    {
        MaNXDinhKyBO maNXDinhKyBO = new MaNXDinhKyBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            short ma = 1;
            try
            {
                ma=Convert.ToInt16(Request.QueryString.Get("ma"));
            }
            catch
            {

            }
            List<DataJsonEntity> lst = new List<DataJsonEntity>();
            lst = maNXDinhKyBO.getMaNhanXetDinhKyToDataJson(Sys_This_Truong.ID, ma);
            var json = new JavaScriptSerializer().Serialize(lst);
            Response.Write(json.ToString());
        }
    }
}