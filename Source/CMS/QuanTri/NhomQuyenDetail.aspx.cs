using CMS.XuLy;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS.QuanTri
{
    public partial class NhomQuyenDetail : AuthenticatePage
    {
        NhomQuyenBO nqBO = new NhomQuyenBO();
        string ma_nq;
        LocalAPI localAPI = new LocalAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("ma_nq") != null)
            {
                try
                {
                    ma_nq = Request.QueryString.Get("ma_nq");
                }
                catch { }
            }
            if (!IsPostBack)
            {
                if (ma_nq != null)
                {
                    NHOM_QUYEN detail = new NHOM_QUYEN();
                    detail = nqBO.getNhomQuyenByMa(ma_nq);
                    if (detail != null)
                    {
                        if (detail.MA != null) tbMa.Text = detail.MA;
                        if (detail.TEN != null)
                            tbTen.Text = detail.TEN.ToString();
                        if (detail.THU_TU != null) tbThuTu.Text = detail.THU_TU.ToString();

                        btEdit.Visible = true;
                        btAdd.Visible = false;
                    }
                    else
                    {
                        btEdit.Visible = false;
                        btAdd.Visible = true;
                    }
                }
                else
                {
                    btEdit.Visible = false;
                    btAdd.Visible = true;
                    getMaxThuTu();
                }
            }
        }

        protected void btAdd_Click(object sender, EventArgs e)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            NHOM_QUYEN detail = new NHOM_QUYEN();
            detail.MA = tbMa.Text;
            detail.TEN = tbTen.Text;
            detail.THU_TU = localAPI.ConvertStringToShort(tbThuTu.Text);
            NHOM_QUYEN checkMaNQ = nqBO.getNhomQuyenByMa(tbMa.Text.Trim());
            if (checkMaNQ == null)
            {
                res = nqBO.insert(detail, Sys_User.ID);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Mã nhóm quyền này đã tồn tại. Vui lòng kiểm tra lại!');", true);
                tbMa.Focus();
                return;
            }
            string strMsg = "";
            if (res.Res)
            {
                tbMa.Text = "";
                tbMa.Focus();
                tbTen.Text = "";
                getMaxThuTu();
                strMsg = "notification('success', '" + res.Msg + "');";
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
        protected void getMaxThuTu()
        {
            short? max_thu_tu = nqBO.getMaxThuTu();
            if (max_thu_tu != null)
                tbThuTu.Text = Convert.ToString(Convert.ToInt16(max_thu_tu) + 1);
            else tbThuTu.Text = "1";
        }
        protected void btEdit_Click(object sender, EventArgs e)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            NHOM_QUYEN detail = new NHOM_QUYEN();
            detail = nqBO.getNhomQuyenByMa(ma_nq);
            NHOM_QUYEN checkMaNQ = nqBO.getNhomQuyenByMa(tbMa.Text.Trim());
            if (checkMaNQ != null && checkMaNQ.MA != ma_nq)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Mã nhóm quyền này đã tồn tại. Vui lòng kiểm tra lại!');", true);
                tbMa.Focus();
                return;
            }
            else
            {
                if (detail != null)
                {
                    detail.MA = tbMa.Text.Trim();
                    detail.TEN = tbTen.Text.Trim();
                    detail.THU_TU = localAPI.ConvertStringToShort(tbThuTu.Text);
                    res = nqBO.update(ma_nq, detail, Sys_User.ID);
                }
            }
            string strMsg = "";
            if (res.Res)
            {
                strMsg = "notification('success', '" + res.Msg + "');";
            }
            else
            {
                strMsg = "notification('error', '" + res.Msg + "');";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
    }
}