using OneEduDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CMS.Manage
{
    public partial class ManageCache : AuthenticatePage
    {
        private readonly DefaultCacheProvider dbCache = new DefaultCacheProvider();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Sys_User.IS_ROOT != true) Response.Redirect("~/Default.aspx");
            if (!IsPostBack)
            {
                var itemsList = new List<string>();
                itemsList = dbCache.GetAll();
                rlCache.DataSource = itemsList;
                rlCache.DataBind();
            }
        }

        protected void btXoa_Click(object sender, EventArgs e)
        {
            foreach (RadListBoxItem item in rlCache.CheckedItems)
            {
                if (item.Checked)
                {
                    dbCache.Invalidate(item.Text);
                }
            }
            var itemsList = new List<string>();
            itemsList = dbCache.GetAll();
            rlCache.DataSource = itemsList;
            rlCache.DataBind();
        }

        protected void btXoaAll_Click(object sender, EventArgs e)
        {
            dbCache.RemoveAll();
            var itemsList = new List<string>();
            itemsList = dbCache.GetAll();
            rlCache.DataSource = itemsList;
            rlCache.DataBind();
        }
    }
}