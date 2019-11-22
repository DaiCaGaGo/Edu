using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS
{
    public partial class DangKyZaloMobileConfirm : System.Web.UI.Page
    {
        MapPhuHuynhHocSinhBO mapPhuHuynhHocSinhBO = new MapPhuHuynhHocSinhBO();
        long? id_map_ph_hs;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("id") != null)
            {
                try
                {
                    id_map_ph_hs = Convert.ToInt64(Request.QueryString.Get("id"));
                }
                catch (Exception ex) { }
            }
            lblThongBao.Text = "Mã xác thực đã được gửi đến sdt nhận sms hàng ngày. Vui lòng lấy mã và nhập vào đây để xác nhận";
            tbMaXacNhan.Focus();
        }
        protected void btXacNhan_Click(object sender, EventArgs e)
        {
            string ma_bao_mat = tbMaXacNhan.Text.Trim();
            if (string.IsNullOrEmpty(ma_bao_mat))
            {
                lblThongBao.Text = "Bạn chưa nhập mã xác nhận!";
                return;
            }
            string ma_confirm = "";
            if (id_map_ph_hs != null)
            {
                MAP_PH_HS detail = mapPhuHuynhHocSinhBO.getMapDuLieuByID(id_map_ph_hs.Value);
                if (detail != null)
                {
                    ma_confirm = detail.MA_BAO_MAT;
                    if (ma_confirm == ma_bao_mat)
                    {
                        ResultEntity res = mapPhuHuynhHocSinhBO.updateTrangThaiDuyet(detail.ID);
                        if (res.Res)
                        {
                            lblThongBao.Text = "SĐT đã đăng ký thành công!";
                            lblThongBao.ForeColor = Color.Green;
                            return;
                        }
                    }
                    else
                    {
                        lblThongBao.Text = "Mã xác nhận không đúng!";
                        return;
                    }
                }
            }
            else
            {
                lblThongBao.Text = "Có lỗi xảy ra. Liên hệ với quản trị viên để được giúp đỡ!";
                return;
            }
        }
    }
}