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

namespace CMS.Report
{
    public partial class BienLaiThuTienHS : AuthenticatePage
    {
        private HocSinhBO hsBO = new HocSinhBO();
        LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkChonTruong();
            btTimKiem.Visible = is_access(SYS_Type_Access.THEM);
            btTimKiem.Visible = is_access(SYS_Type_Access.XEM);
            if (!IsPostBack)
            {
                objKhoi.SelectParameters.Add("cap_hoc", Sys_This_Cap_Hoc);
                objKhoi.SelectParameters.Add("maLoaiLopGDTX", DbType.Int16, Sys_This_Lop_GDTX == null ? "" : Sys_This_Lop_GDTX.ToString());
                rcbKhoi.DataBind();
                objLop.SelectParameters.Add("ma_cap_hoc", Sys_This_Cap_Hoc);
                objLop.SelectParameters.Add("idTruong", Sys_This_Truong.ID.ToString());
                objLop.SelectParameters.Add("maNamHoc", Sys_Ma_Nam_hoc.ToString());
                rcbLop.DataBind();
            }
        }
        protected void btTimKiem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbSoTien.Text))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Số tiền không để trống');", true);
                return;
            }
            decimal soTien = 0;
            if (!string.IsNullOrEmpty(tbSoTien.Text))
                soTien = Convert.ToDecimal(tbSoTien.Text.Trim());
            #region get list fillter
            List<short> lst_ma_khoi = new List<short>();
            foreach (var item in rcbKhoi.CheckedItems)
            {
                lst_ma_khoi.Add(localAPI.ConvertStringToShort(item.Value).Value);
            }
            List<long> lst_id_lop = new List<long>();
            foreach (var item in rcbLop.CheckedItems)
            {
                lst_id_lop.Add(localAPI.ConvertStringTolong(item.Value).Value);
            }
            #endregion
            List<BienLaiThuTienHSEntity> lst = hsBO.getBienLaiThuTienHS(Sys_This_Truong.ID, Convert.ToInt16(Sys_Ma_Nam_hoc), Convert.ToInt16(Sys_Hoc_Ky), Sys_This_Cap_Hoc, tbSoTien.Text.Trim(), tbNoiDung.Text, LocalAPI.NumberToTextVN(soTien), lst_ma_khoi, lst_id_lop);
            string ReportID = "BLTT";
            Session["Datareport" + ReportID] = lst;
            Response.Redirect("~/Report/ReportView.aspx?ma=" + ReportID, true);

            
        }
        protected void rcbKhoi_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            string lst_ma_khoi = "";
            foreach (var item in rcbKhoi.CheckedItems)
            {
                if (string.IsNullOrEmpty(lst_ma_khoi))
                    lst_ma_khoi += item.Value;
                else lst_ma_khoi += "," + item.Value;
            }
            hdKhoi.Value = lst_ma_khoi;
            rcbLop.ClearSelection();
            rcbLop.Text = String.Empty;
            rcbLop.DataBind();
        }
    }
}