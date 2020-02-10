using ClosedXML.Excel;
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

namespace CMS.LichSuTinNhan
{
    public partial class TinNhanDatLich : AuthenticatePage
    {
        TinNhanBO tinNhanBO = new TinNhanBO();
        TRUONG truong = new TRUONG();
        TruongBO truongBO = new TruongBO();
        public LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rcbTruong.DataSource = Sys_List_Truong;
                rcbTruong.DataBind();
                getTongTin();
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = tinNhanBO.getTinNhanHenGio(localAPI.ConvertStringTolong(rcbTruong.SelectedValue), tbNoiDung.Text.Trim());
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", "SetGridHeight();", true);
        }
        protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                //short? loai_tin = localAPI.ConvertStringToShort(item["LOAI_TIN"].Text);
                //item["LOAI_TIN"].Text = loai_tin == null ? "" : loai_tin == 1 ? "Liên lạc cá nhân" : loai_tin == 2 ? "Gửi tin thông báo" : "";
                short? trang_thai = localAPI.ConvertStringToShort(item["TRANG_THAI"].Text);
                item["TRANG_THAI"].Text = trang_thai == null ? "Chờ gửi" : trang_thai == 1 ? "Thành công" : trang_thai == 2 ? "Gửi lỗi" : "Dừng gửi";
                NguoiDungBO nguoiDungBO = new NguoiDungBO();
                long? id_nguoi_gui = localAPI.ConvertStringTolong(item["NGUOI_GUI"].Text);
                if (id_nguoi_gui != null)
                    item["NGUOI_GUI"].Text = nguoiDungBO.getNguoiDungByID(id_nguoi_gui).TEN_DANG_NHAP;
            }
        }
        protected void btTimKiem_Click(object sender, EventArgs e)
        {
            getTongTin();
            RadGrid1.Rebind();
        }
        protected void rcbTruong_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            getTongTin();
            RadGrid1.Rebind();
        }
        protected string getTongTin()
        {
            lblTongTin.Text = "";
            long? so_tin = 0;
            List<TinNhanEntity> listTinNhan = tinNhanBO.getTinNhanHenGio(localAPI.ConvertStringTolong(rcbTruong.SelectedValue), tbNoiDung.Text.Trim());
            so_tin = listTinNhan.Sum(x => x.SO_TIN);
            lblTongTin.Text = "Tổng số tin: " + so_tin;
            return lblTongTin.Text;
        }

        protected void btXoaTuyChon_Click(object sender, EventArgs e)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            List<TIN_NHAN> lstTinNhan = new List<TIN_NHAN>();
            foreach (GridDataItem item in RadGrid1.SelectedItems)
            {
                TIN_NHAN tinNhan = new TIN_NHAN();
                tinNhan.ID = Convert.ToInt64(item.GetDataKeyValue("ID").ToString());
                tinNhan.SO_TIN = Convert.ToInt16(item["SO_TIN"].Text);
                tinNhan.THOI_GIAN_GUI = Convert.ToDateTime(item["THOI_GIAN_GUI"].Text);
                tinNhan.LOAI_TIN = Convert.ToInt16(item["LOAI_TIN"].Text);
                tinNhan.ID_TRUONG = Convert.ToInt64(rcbTruong.SelectedValue);
                lstTinNhan.Add(tinNhan);
            }
            res = tinNhanBO.deleteTinHenGio(lstTinNhan, Sys_User.ID);
            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', 'Có " + lstTinNhan.Sum(x => x.SO_TIN) + " tin nhắn được xóa.');";
            }
            else strMsg = "notification('error', 'Có lỗi xảy ra.');";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
        }

        protected void btnXoa_Click(object sender, EventArgs e)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            List<TIN_NHAN> lstTinNhan = new List<TIN_NHAN>();
            foreach (GridDataItem item in RadGrid1.Items)
            {
                TIN_NHAN tinNhan = new TIN_NHAN();
                tinNhan.ID = Convert.ToInt64(item.GetDataKeyValue("ID").ToString());
                tinNhan.SO_TIN = Convert.ToInt16(item["SO_TIN"].Text);
                tinNhan.THOI_GIAN_GUI = Convert.ToDateTime(item["THOI_GIAN_GUI"].Text);
                tinNhan.LOAI_TIN = Convert.ToInt16(item["LOAI_TIN"].Text);
                tinNhan.ID_TRUONG = Convert.ToInt64(rcbTruong.SelectedValue);
                lstTinNhan.Add(tinNhan);
            }
            res = tinNhanBO.deleteTinHenGio(lstTinNhan, Sys_User.ID);
            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', 'Có " + lstTinNhan.Sum(x => x.SO_TIN) + " tin nhắn được xóa.');";
            }
            else strMsg = "notification('error', 'Có lỗi xảy ra.');";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
            RadGrid1.Rebind();
        }
    }
}