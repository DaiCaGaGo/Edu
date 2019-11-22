using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CMS.Control
{
    public partial class MainMenu : System.Web.UI.UserControl
    {
        public LoginHelper LoginHelper = new LoginHelper();
        public NGUOI_DUNGEntity nguoi_dung;
        public SYS_Profile sYS_Profile = new SYS_Profile();
        protected void Page_Load(object sender, EventArgs e)
        {
            nguoi_dung = LoginHelper.GetUserLogged;
            radMenu.DataSource = GetAllMenuActiveOfUser();
            radMenu.DataBind();
        }
        protected void RadMenu1_ItemDataBound(object sender, RadMenuEventArgs e)
        {
            MENU row = (MENU)e.Item.DataItem;
            if (row.IS_BAT_DAU_GROUP != null && row.IS_BAT_DAU_GROUP == true)
            {
                e.Item.CssClass = "group-item-menu";
            }
            //FOR ROOT ITEM
            if (nguoi_dung.IS_ROOT != true)
            {
                if (row.ID_CHA == null && !row.IS_EXIST_CHILD)
                {
                    //set disabled item
                    var radMenuItem = radMenu.FindItemByValue(row.ID.ToString());
                    if (radMenuItem != null)
                    {
                        radMenuItem.Enabled = false;
                    }
                }
            }
        }
        private List<MENU> GetAllMenuActiveOfUser()
        {
            var mn = new MenuBO();
            List<MENU> listAllMenu = mn.getMenu(sYS_Profile.getCapHoc(),false,0,"",false).Where(x=>x.IS_HIEN_THI==true).ToList();

            List<MENU> listAllPhanQuyen = new List<MENU>();
            //remove child menu if not having right
            if (nguoi_dung != null)
            {
                if (nguoi_dung.IS_ROOT != null && nguoi_dung.IS_ROOT == true)
                {
                    listAllPhanQuyen = listAllMenu;
                }
                else
                {
                    var listMenuOfNhomQuyen = mn.getMenuByNguoiDung(sYS_Profile.getCapHoc(), nguoi_dung.ID,true,false).Where(x => x.IS_HIEN_THI == true).ToList();
                    foreach (var menuItem in listAllMenu)
                    {
                        if (menuItem.ID_CHA == null)
                        {
                            bool isExistChild = listMenuOfNhomQuyen.Any(c => c.ID_CHA == menuItem.ID && c.IS_HIEN_THI == true);
                            menuItem.IS_EXIST_CHILD = isExistChild;
                            listAllPhanQuyen.Add(menuItem);
                        }
                        else
                        {
                            var isExistInNhomQuyen = listMenuOfNhomQuyen.Any(c => c.ID == menuItem.ID && c.IS_HIEN_THI == true);
                            if (isExistInNhomQuyen)
                                listAllPhanQuyen.Add(menuItem);
                        }
                    }
                }
            }
            return listAllPhanQuyen;
        }
    }
}