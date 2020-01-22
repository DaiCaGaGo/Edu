using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CMS.TinTuc
{
    public partial class ThongBaoNhaTruongDetail : AuthenticatePage
    {
        long? id;
        ThongBaoNhaTruongBO thongBaoNhaTruongBO = new ThongBaoNhaTruongBO();
        LocalAPI localAPI = new LocalAPI();
        LogUserBO logUserBO = new LogUserBO();
        public string[] listImage;

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
                rcbLoaiTB.DataBind();
                rcbLoaiTB.SelectedValue = "1";
                if (id != null)
                {
                    THONG_BAO_NHA_TRUONG detailThongBaoNhaTruong = new THONG_BAO_NHA_TRUONG();
                    detailThongBaoNhaTruong = thongBaoNhaTruongBO.getThongBaoByID(id.Value);
                    if (detailThongBaoNhaTruong != null)
                    {
                        tbNoiDung.Text = detailThongBaoNhaTruong.NOI_DUNG != null ? detailThongBaoNhaTruong.NOI_DUNG : "";
                        rcbLoaiTB.SelectedValue = detailThongBaoNhaTruong.LOAI_THONG_BAO != null ? detailThongBaoNhaTruong.LOAI_THONG_BAO.ToString() : "";

                        if (!string.IsNullOrEmpty(detailThongBaoNhaTruong.ANH_NOI_DUNG))
                        {
                            string[] fileImage = detailThongBaoNhaTruong.ANH_NOI_DUNG.Split(';');
                            listImage = fileImage;
                            if (fileImage.Length > 0)
                            {
                                for (int i = 0; i < fileImage.Length; i++)
                                {
                                    if (!string.IsNullOrEmpty(fileImage[i]))
                                    {
                                        
                                    }
                                }
                            }
                        }

                        btEdit.Visible = is_access(SYS_Type_Access.SUA);
                        btAdd.Visible = false;
                    }
                    else
                    {
                        btEdit.Visible = false;
                        btAdd.Visible = is_access(SYS_Type_Access.THEM);
                    }
                }
                else
                {
                    btEdit.Visible = false;
                    btAdd.Visible = is_access(SYS_Type_Access.THEM);
                }
            }
        }
        protected void btAdd_Click(object sender, EventArgs e)
        {
            #region "check du lieu"
            if (!is_access(SYS_Type_Access.THEM))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }

            if (string.IsNullOrEmpty(tbNoiDung.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Nội dung thông báo không được để trống!');", true);
                return;
            }

            if (string.IsNullOrEmpty(rcbLoaiTB.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn phải chọn loại thông báo!');", true);
                return;
            }
            #endregion
            THONG_BAO_NHA_TRUONG detail = new THONG_BAO_NHA_TRUONG();
            detail.ID_TRUONG = Sys_This_Truong.ID;
            detail.CAP_HOC = Sys_This_Cap_Hoc;
            detail.ID_NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
            detail.NOI_DUNG = tbNoiDung.Text.Trim();
            detail.LOAI_THONG_BAO = Convert.ToInt16(rcbLoaiTB.SelectedValue);
            string tenFile = "";
            foreach (UploadedFile f in rauAnh.UploadedFiles)
            {
                Guid strGuid = Guid.NewGuid();
                //string fileName = strGuid.ToString() + f.GetName();
                string fileName = f.GetName();

                string folderPath = Server.MapPath("~/img/ThongBaoNhaTruong/" + Sys_This_Truong.MA + "_" + Sys_This_Truong.ID + "/");

                //Check whether Directory (Folder) exists.
                if (!Directory.Exists(folderPath))
                {
                    //If Directory (Folder) does not exists. Create it.
                    Directory.CreateDirectory(folderPath);
                }
                //Save the File to the Directory (Folder).
                f.SaveAs(folderPath + Path.GetFileName(f.FileName));

                tenFile += "/img/ThongBaoNhaTruong/" + Sys_This_Truong.MA + "_" + Sys_This_Truong.ID + "/" + fileName + ";";
            }
            if (!string.IsNullOrEmpty(tenFile)) tenFile = tenFile.TrimEnd(';');
            detail.ANH_NOI_DUNG = tenFile;
            ResultEntity res = thongBaoNhaTruongBO.insert(detail, Sys_User.ID);
            string strMsg = "";
            if (res.Res)
            {
                tbNoiDung.Text = "";
                strMsg = "notification('success', '" + res.Msg + "');";
                THONG_BAO_NHA_TRUONG resMa = (THONG_BAO_NHA_TRUONG)res.ResObject;
                logUserBO.insert(Sys_This_Truong.ID, "INSERT", "Thêm mới thông báo " + resMa.ID, Sys_User.ID, DateTime.Now);
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
        protected void btEdit_Click(object sender, EventArgs e)
        {
            #region "check du lieu"
            if (!is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }

            if (string.IsNullOrEmpty(tbNoiDung.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Nội dung thông báo không được để trống!');", true);
                return;
            }

            if (string.IsNullOrEmpty(rcbLoaiTB.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn phải chọn loại thông báo!');", true);
                return;
            }
            #endregion
            THONG_BAO_NHA_TRUONG detail = new THONG_BAO_NHA_TRUONG();
            detail = thongBaoNhaTruongBO.getThongBaoByID(id.Value);
            if (detail == null) detail = new THONG_BAO_NHA_TRUONG();
            if (detail.ID_TRUONG != Sys_This_Truong.ID)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện sửa dữ liệu này!');", true);
                return;
            }
            detail.ID_TRUONG = Sys_This_Truong.ID;
            detail.CAP_HOC = Sys_This_Cap_Hoc;
            detail.ID_NAM_HOC = Convert.ToInt16(Sys_Ma_Nam_hoc);
            detail.NOI_DUNG = tbNoiDung.Text.Trim();
            detail.LOAI_THONG_BAO = Convert.ToInt16(rcbLoaiTB.SelectedValue);
            string tenFile = "";
            foreach (UploadedFile f in rauAnh.UploadedFiles)
            {
                Guid strGuid = Guid.NewGuid();
                //string fileName = strGuid.ToString() + f.GetName();
                string fileName = f.GetName();

                string folderPath = Server.MapPath("~/img/ThongBaoNhaTruong/" + Sys_This_Truong.MA + "_" + Sys_This_Truong.ID + "/");

                //Check whether Directory (Folder) exists.
                if (!Directory.Exists(folderPath))
                {
                    //If Directory (Folder) does not exists. Create it.
                    Directory.CreateDirectory(folderPath);
                }
                //Save the File to the Directory (Folder).
                f.SaveAs(folderPath + Path.GetFileName(f.FileName));

                tenFile += "/img/ThongBaoNhaTruong/" + Sys_This_Truong.MA + "_" + Sys_This_Truong.ID + "/" + fileName + ";";
            }

            #region lấy ảnh còn lại đã upload
            string lstImgOld = String.Format("{0}", Request.Form["list_images_node[]"]);
            if (!string.IsNullOrEmpty(lstImgOld))
            {
                string[] imgOld = lstImgOld.Split(',');
                if (imgOld.Length > 0)
                {
                    for (int i = 0; i < imgOld.Length; i++)
                    {
                        string imgName = imgOld[i];
                        if (!string.IsNullOrEmpty(imgName))
                        {
                            if (!tenFile.Contains(imgName)) tenFile = tenFile += imgName + ";";
                        }
                    }
                }
            }
            #endregion

            if (string.IsNullOrEmpty(tenFile)) tenFile = tenFile.TrimEnd(';');
            detail.ANH_NOI_DUNG = tenFile;
            ResultEntity res = thongBaoNhaTruongBO.update(detail, Sys_User.ID);
            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
                logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật thông báo " + id, Sys_User.ID, DateTime.Now);
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
    }
}