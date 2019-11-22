using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS.Report
{
    public partial class HuongDanDangKyZalo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            XtraReport report = new ReportDangKyZalo();
            documentViewer.OpenReport(report);
        }
        protected void btIn_Click(object sender, EventArgs e)
        {
            
        }
    }
}