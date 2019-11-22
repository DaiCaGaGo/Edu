using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CMS.QuanTri
{
    public partial class NguoiDungTruong : AuthenticatePage
    {
        long? id_NguoiDung_req = null;
        private TruongBO truongBO = new TruongBO();
        private NguoiDungBO nguoiDungBO = new NguoiDungBO();
        private NguoiDungTruongBO nguoiDungTruongBO = new NguoiDungTruongBO();
        private NguoiDungMenuBO nguoiDungMenuBO = new NguoiDungMenuBO();
        private NhomQuyenMenuBO nhomQuyenMenuBO = new NhomQuyenMenuBO();
        private LocalAPI localAPI = new LocalAPI();
        LogUserBO logUserBO = new LogUserBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("id_hoso") != null)
            {
                try
                {
                    id_NguoiDung_req = Convert.ToInt64(Request.QueryString.Get("id_hoso"));
                }
                catch { }
            }
            if (!IsPostBack)
            {
                objUser.SelectParameters.Add("userName", "");
                rcbUser.DataBind();
                NguoiDungBO nguoiDungBO = new NguoiDungBO();
                NGUOI_DUNG nguoiDung = new NGUOI_DUNG();
                nguoiDung = nguoiDungBO.getNguoiDungByID(id_NguoiDung_req.Value);
                short? id_doi_tac = nguoiDung.ID_DOI_TAC;
                objTruong.SelectParameters.Add("id_doi_tac", id_doi_tac.ToString());
                rcbTruong.DataBind();
                if (id_NguoiDung_req != null)
                {
                    if (rcbUser.Items.FindItemByValue(id_NguoiDung_req.ToString()) != null)
                    {
                        rcbUser.SelectedValue = id_NguoiDung_req.ToString();
                    }
                }
            }
        }
        protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
        {
            if (e.Argument == "RadWindowManager1_Close")
            {
                RadGrid1.Rebind();
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = truongBO.getTruongExistsNguoiDungTruong(localAPI.ConvertStringTolong(rcbUser.SelectedValue));
        }
        protected void btSave_Click(object sender, EventArgs e)
        {
            string strMsg = "";
            int countCheck = 0;
            foreach (RadComboBoxItem item in rcbTruong.Items)
            {
                if (item.Checked)
                {
                    countCheck++;
                    long id_truong = Convert.ToInt64(item.Value);
                    NGUOI_DUNG_TRUONG detailNguoiDungTruong = new NGUOI_DUNG_TRUONG();
                    detailNguoiDungTruong.ID_NGUOI_DUNG = Convert.ToInt64(rcbUser.SelectedValue);
                    detailNguoiDungTruong.ID_TRUONG = id_truong;
                    detailNguoiDungTruong.TRANG_THAI = 1;

                    NGUOI_DUNG nguoiDung = nguoiDungBO.getNguoiDungByID(id_NguoiDung_req);

                    ResultEntity res = nguoiDungTruongBO.insertOrUpdate(detailNguoiDungTruong, Sys_User.ID, nguoiDung.MA_NHOM_QUYEN);

                    if (res.Res)
                    {
                        strMsg = "notification('success', '" + res.Msg + "');";
                        logUserBO.insert(null, "UPDATE", "Cập nhật trường " + id_truong + " cho người dùng " + id_NguoiDung_req, Sys_User.ID, DateTime.Now);
                    }
                    else
                    {
                        strMsg = "notification('error', '" + res.Msg + "');";
                        break;
                    }
                }
            }
            if (countCheck == 0)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('warning', 'Chưa có trường nào được chọn!');", true);
                return;
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            rcbTruong.DataBind();
            RadGrid1.Rebind();
        }
        protected void btDeleteChon_Click(object sender, EventArgs e)
        {
            int success = 0, error = 0; string lst_id = string.Empty;

            foreach (GridDataItem row in RadGrid1.SelectedItems)
            {
                try
                {
                    short idTruong = Convert.ToInt16(row.GetDataKeyValue("ID").ToString());
                    string ten = row["TEN"].Text;
                    try
                    {
                        ResultEntity res = nguoiDungTruongBO.delete(Sys_User.ID, Convert.ToInt64(rcbUser.SelectedValue), idTruong);
                        lst_id += idTruong + ":" + ten + ", ";
                        if (res.Res)
                            success++;
                        else
                            error++;
                    }
                    catch
                    {
                        error++;
                    }
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Lỗi hệ thống. Bạn vui lòng kiểm tra lại');", true);
                }
            }
            string strMsg = "";
            if (error > 0)
            {
                strMsg = "notification('error', 'Có " + error + " bản ghi chưa được xóa. Liên hệ với quản trị viên');";
            }
            if (success > 0)
            {
                strMsg += " notification('success', 'Có " + success + " bản ghi được xóa.');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            rcbTruong.DataBind();
            RadGrid1.Rebind();
        }
        protected void rcbUser_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rcbTruong.DataBind();
            RadGrid1.Rebind();
        }


    }
}