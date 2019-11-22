using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;

namespace CMS.CauHinhCaHoc
{
    public partial class CauHinhCaHocDetail : AuthenticatePage
    {
        short? ma_CauHinhCaHoc;
        CauHinhCaHocBO cauHinhCaHocBO = new CauHinhCaHocBO();
        LocalAPI localAPI = new LocalAPI();
        LogUserBO logUserBO = new LogUserBO();
        private string strMessage = "Thời gian tiết sau phải lớn hơn thời gian tiết trước!";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Get("id_hoso") != null)
            {
                try
                {
                    ma_CauHinhCaHoc = Convert.ToInt16(Request.QueryString.Get("id_hoso"));
                }
                catch { }
            }
            if (!IsPostBack)
            {
                if (ma_CauHinhCaHoc != null)
                {
                    CAU_HINH_CA_HOC detail = new CAU_HINH_CA_HOC();
                    detail = cauHinhCaHocBO.getCauHinhCaHocByID(Convert.ToInt64(ma_CauHinhCaHoc));
                    if (detail != null)
                    {
                        tbTenCauHinh.Text = detail.MUA;
                        rdNgayBatDau.SelectedDate = (detail.NGAY_BAT_DAU != null) ? detail.NGAY_BAT_DAU : null;
                        rdNgayKetThuc.SelectedDate = (detail.NGAY_KET_THUC != null) ? detail.NGAY_KET_THUC : null;
                        hdNgayBatDau.Value = (detail.NGAY_BAT_DAU != null) ? detail.NGAY_BAT_DAU.ToString() : null;
                        hdNgayKetThuc.Value = (detail.NGAY_KET_THUC != null) ? detail.NGAY_KET_THUC.ToString() : null;
                        if (detail.THOI_GIAN_TIET1 != null) rdTiet1.SelectedTime = TimeSpan.Parse(detail.THOI_GIAN_TIET1);
                        if (detail.THOI_GIAN_TIET2 != null) rdTiet2.SelectedTime = TimeSpan.Parse(detail.THOI_GIAN_TIET2);
                        if (detail.THOI_GIAN_TIET3 != null) rdTiet3.SelectedTime = TimeSpan.Parse(detail.THOI_GIAN_TIET3);
                        if (detail.THOI_GIAN_TIET4 != null) rdTiet4.SelectedTime = TimeSpan.Parse(detail.THOI_GIAN_TIET4);
                        if (detail.THOI_GIAN_TIET5 != null) rdTiet5.SelectedTime = TimeSpan.Parse(detail.THOI_GIAN_TIET5);
                        if (detail.THOI_GIAN_TIET6 != null) rdTiet6.SelectedTime = TimeSpan.Parse(detail.THOI_GIAN_TIET6);
                        if (detail.THOI_GIAN_TIET7 != null) rdTiet7.SelectedTime = TimeSpan.Parse(detail.THOI_GIAN_TIET7);
                        if (detail.THOI_GIAN_TIET8 != null) rdTiet8.SelectedTime = TimeSpan.Parse(detail.THOI_GIAN_TIET8);
                        if (detail.THOI_GIAN_TIET9 != null) rdTiet9.SelectedTime = TimeSpan.Parse(detail.THOI_GIAN_TIET9);
                        if (detail.THOI_GIAN_TIET10 != null) rdTiet10.SelectedTime = TimeSpan.Parse(detail.THOI_GIAN_TIET10);
                        if (detail.THOI_GIAN_TIET11 != null) rdTiet11.SelectedTime = TimeSpan.Parse(detail.THOI_GIAN_TIET11);
                        if (detail.THOI_GIAN_TIET12 != null) rdTiet12.SelectedTime = TimeSpan.Parse(detail.THOI_GIAN_TIET12);
                        if (detail.THOI_GIAN_TIET13 != null) rdTiet13.SelectedTime = TimeSpan.Parse(detail.THOI_GIAN_TIET13);
                        if (detail.THOI_GIAN_TIET14 != null) rdTiet14.SelectedTime = TimeSpan.Parse(detail.THOI_GIAN_TIET14);
                        if (detail.THOI_GIAN_TIET15 != null) rdTiet15.SelectedTime = TimeSpan.Parse(detail.THOI_GIAN_TIET15);

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
                    btAdd.Visible = is_access(SYS_Type_Access.SUA);
                }
            }
        }
        protected void btAdd_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.THEM))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            CAU_HINH_CA_HOC detail = new CAU_HINH_CA_HOC();
            ResultEntity resCheck = fillDataToCauHinhCaHoc(detail, ma_CauHinhCaHoc, false);

            string strMsg = "";
            if (resCheck.Res)
            {
                ResultEntity res = cauHinhCaHocBO.insert(detail, Sys_User.ID);
                if (res.Res)
                {
                    CAU_HINH_CA_HOC resCHCH = (CAU_HINH_CA_HOC)res.ResObject;
                    strMsg = "notification('success', '" + res.Msg + "');";
                    logUserBO.insert(Sys_This_Truong.ID, "INSERT", "Thêm mới ca học " + resCHCH.ID, Sys_User.ID, DateTime.Now);
                }
                else
                {
                    strMsg = "notification('error', '" + res.Msg + "');";
                }
            }
            else
            {
                strMsg = "notification('error', '" + resCheck.Msg + "');";
            }            
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
        protected void btEdit_Click(object sender, EventArgs e)
        {
            if (!is_access(SYS_Type_Access.SUA))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", "notification('error', 'Bạn không có quyền thực hiện thao tác này!');", true);
                return;
            }
            CAU_HINH_CA_HOC detail = new CAU_HINH_CA_HOC();
            ResultEntity resCheck = fillDataToCauHinhCaHoc(detail, ma_CauHinhCaHoc, true);
            string strMsg = "";
            if (resCheck.Res)
            {
                detail = (CAU_HINH_CA_HOC)resCheck.ResObject;
                ResultEntity res = cauHinhCaHocBO.update(detail, Sys_User.ID);
                if (res.Res)
                {
                    strMsg = "notification('success', '" + res.Msg + "');";
                    logUserBO.insert(Sys_This_Truong.ID, "UPDATE", "Cập nhật ca học " + ma_CauHinhCaHoc, Sys_User.ID, DateTime.Now);
                }
                else
                {
                    strMsg = "notification('error', '" + res.Msg + "');";
                }
            }
            else
            {
                strMsg = "notification('error', '" + resCheck.Msg + "');";
            }

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myalert", strMsg, true);
        }
        private ResultEntity fillDataToCauHinhCaHoc(CAU_HINH_CA_HOC detail, short? id, bool is_update)
        {
            ResultEntity res = new ResultEntity();
            TimeSpan maxTime = new TimeSpan(00, 00, 00);
            res.Res = true;
            if (id > 0)
            {
                detail.ID = Convert.ToInt64(ma_CauHinhCaHoc);
                detail = cauHinhCaHocBO.getCauHinhCaHocByID(detail.ID);
                if (detail == null) detail = new CAU_HINH_CA_HOC();
            }


            detail.MUA = tbTenCauHinh.Text;
            detail.ID_TRUONG = Sys_This_Truong.ID;
            detail.NGAY_BAT_DAU = Convert.ToDateTime(rdNgayBatDau.SelectedDate.Value);
            detail.NGAY_KET_THUC = Convert.ToDateTime(rdNgayKetThuc.SelectedDate.Value);

            // check ngay bat dau khong duoc nho hon ngay ket thuc
            if (detail.NGAY_BAT_DAU > detail.NGAY_KET_THUC)
            {
                res.Res = false;
                res.Msg = "Ngày bắt đầu phải nhỏ hơn ngày kết thúc!";
                return res;
            }

            List<CAU_HINH_CA_HOC> lstCauHinhCaHoc = cauHinhCaHocBO.getCauHinhCaHocByTruong(Sys_This_Truong.ID);
            if(!is_update || (is_update && lstCauHinhCaHoc.Count > 1 && (rdNgayBatDau.SelectedDate != Convert.ToDateTime(hdNgayBatDau.Value) || rdNgayKetThuc.SelectedDate != Convert.ToDateTime(hdNgayKetThuc.Value))))
            {
                // check ngay bat dau/ngay ket thuc record sau khong nam trong khoang ngay bat dau/ngay ket cua record truoc
                CauHinhCaHocEntity checkCauHinh = cauHinhCaHocBO.getMinMaxDateCauHinhCaHoc(Convert.ToInt64(Sys_This_Truong.ID));

                if ((detail.NGAY_BAT_DAU <= checkCauHinh.NGAY_KET_THUC && detail.NGAY_BAT_DAU >= checkCauHinh.NGAY_BAT_DAU)
                    || (detail.NGAY_KET_THUC >= checkCauHinh.NGAY_BAT_DAU && detail.NGAY_KET_THUC <= checkCauHinh.NGAY_KET_THUC)
                    || (detail.NGAY_BAT_DAU <= checkCauHinh.NGAY_BAT_DAU && detail.NGAY_KET_THUC >= checkCauHinh.NGAY_KET_THUC)
                    || (detail.NGAY_BAT_DAU <= checkCauHinh.NGAY_BAT_DAU && detail.NGAY_KET_THUC <= checkCauHinh.NGAY_KET_THUC && detail.NGAY_KET_THUC >= checkCauHinh.NGAY_BAT_DAU)
                    || (detail.NGAY_BAT_DAU >= checkCauHinh.NGAY_BAT_DAU && detail.NGAY_BAT_DAU <= checkCauHinh.NGAY_KET_THUC && detail.NGAY_KET_THUC >= checkCauHinh.NGAY_KET_THUC))
                {
                    res.Res = false;
                    res.Msg = "Ngày bắt đầu/ Ngày kết thúc không được nằm trong khoảng " + checkCauHinh.NGAY_BAT_DAU.ToString().Substring(0, 10) + " - " + checkCauHinh.NGAY_KET_THUC.ToString().Substring(0, 10);
                    return res;
                }
            }

            if (rdTiet1.SelectedTime != null && rdTiet1.SelectedTime > maxTime)
            {
                detail.THOI_GIAN_TIET1 = rdTiet1.SelectedTime.Value.ToString(@"hh\:mm");
                maxTime = rdTiet1.SelectedTime.Value;
            }
            if (rdTiet2.SelectedTime != null)
            {
                if (rdTiet2.SelectedTime <= maxTime)
                {
                    res.Res = false;
                    res.Msg = strMessage;
                    return res;
                }
                else
                {
                    detail.THOI_GIAN_TIET2 = rdTiet2.SelectedTime.Value.ToString(@"hh\:mm");
                    maxTime = rdTiet2.SelectedTime.Value;
                }
            }
            if (rdTiet3.SelectedTime != null)
            {
                if (rdTiet3.SelectedTime <= maxTime)
                {
                    res.Res = false;
                    res.Msg = strMessage;
                    return res;
                }
                else
                {
                    detail.THOI_GIAN_TIET3 = rdTiet3.SelectedTime.Value.ToString(@"hh\:mm");
                    maxTime = rdTiet3.SelectedTime.Value;
                }
            }
            if (rdTiet4.SelectedTime != null)
            {
                if (rdTiet4.SelectedTime <= maxTime)
                {
                    res.Res = false;
                    res.Msg = strMessage;
                    return res;
                }
                else
                {
                    detail.THOI_GIAN_TIET4 = rdTiet4.SelectedTime.Value.ToString(@"hh\:mm");
                    maxTime = rdTiet4.SelectedTime.Value;
                }
            }
            if (rdTiet5.SelectedTime != null)
            {
                if (rdTiet5.SelectedTime <= maxTime)
                {
                    res.Res = false;
                    res.Msg = strMessage;
                    return res;
                }
                else
                {
                    detail.THOI_GIAN_TIET5 = rdTiet5.SelectedTime.Value.ToString(@"hh\:mm");
                    maxTime = rdTiet5.SelectedTime.Value;
                }
            }
            if (rdTiet6.SelectedTime != null)
            {
                if (rdTiet6.SelectedTime <= maxTime)
                {
                    res.Res = false;
                    res.Msg = strMessage;
                    return res;
                }
                else
                {
                    detail.THOI_GIAN_TIET6 = rdTiet6.SelectedTime.Value.ToString(@"hh\:mm");
                    maxTime = rdTiet6.SelectedTime.Value;
                }
            }
            if (rdTiet7.SelectedTime != null)
            {
                if (rdTiet7.SelectedTime <= maxTime)
                {
                    res.Res = false;
                    res.Msg = strMessage;
                    return res;
                }
                else
                {
                    detail.THOI_GIAN_TIET7 = rdTiet7.SelectedTime.Value.ToString(@"hh\:mm");
                    maxTime = rdTiet7.SelectedTime.Value;
                }
            }
            if (rdTiet8.SelectedTime != null)
            {
                if (rdTiet8.SelectedTime <= maxTime)
                {
                    res.Res = false;
                    res.Msg = strMessage;
                    return res;
                }
                else
                {
                    detail.THOI_GIAN_TIET8 = rdTiet8.SelectedTime.Value.ToString(@"hh\:mm");
                    maxTime = rdTiet8.SelectedTime.Value;
                }
            }
            if (rdTiet9.SelectedTime != null)
            {
                if (rdTiet9.SelectedTime <= maxTime)
                {
                    res.Res = false;
                    res.Msg = strMessage;
                    return res;
                }
                else
                {
                    detail.THOI_GIAN_TIET9 = rdTiet9.SelectedTime.Value.ToString(@"hh\:mm");
                    maxTime = rdTiet9.SelectedTime.Value;
                }
            }
            if (rdTiet10.SelectedTime != null)
            {
                if (rdTiet10.SelectedTime <= maxTime)
                {
                    res.Res = false;
                    res.Msg = strMessage;
                    return res;
                }
                else
                {
                    detail.THOI_GIAN_TIET10 = rdTiet10.SelectedTime.Value.ToString(@"hh\:mm");
                    maxTime = rdTiet10.SelectedTime.Value;
                }
            }
            if (rdTiet11.SelectedTime != null)
            {
                if (rdTiet11.SelectedTime <= maxTime)
                {
                    res.Res = false;
                    res.Msg = strMessage;
                    return res;
                }
                else
                {
                    detail.THOI_GIAN_TIET11 = rdTiet11.SelectedTime.Value.ToString(@"hh\:mm");
                    maxTime = rdTiet11.SelectedTime.Value;
                }
            }
            if (rdTiet12.SelectedTime != null)
            {
                if (rdTiet12.SelectedTime <= maxTime)
                {
                    res.Res = false;
                    res.Msg = strMessage;
                    return res;
                }
                else
                {
                    detail.THOI_GIAN_TIET12 = rdTiet12.SelectedTime.Value.ToString(@"hh\:mm");
                    maxTime = rdTiet12.SelectedTime.Value;
                }
            }
            if (rdTiet13.SelectedTime != null)
            {
                if (rdTiet13.SelectedTime <= maxTime)
                {
                    res.Res = false;
                    res.Msg = strMessage;
                    return res;
                }
                else
                {
                    detail.THOI_GIAN_TIET13 = rdTiet13.SelectedTime.Value.ToString(@"hh\:mm");
                    maxTime = rdTiet13.SelectedTime.Value;
                }
            }
            if (rdTiet14.SelectedTime != null)
            {
                if (rdTiet14.SelectedTime <= maxTime)
                {
                    res.Res = false;
                    res.Msg = strMessage;
                    return res;
                }
                else
                {
                    detail.THOI_GIAN_TIET14 = rdTiet14.SelectedTime.Value.ToString(@"hh\:mm");
                    maxTime = rdTiet14.SelectedTime.Value;
                }
            }
            if (rdTiet15.SelectedTime != null)
            {
                if (rdTiet15.SelectedTime <= maxTime)
                {
                    res.Res = false;
                    res.Msg = strMessage;
                    return res;
                }
                else
                {
                    detail.THOI_GIAN_TIET15 = rdTiet15.SelectedTime.Value.ToString(@"hh\:mm");
                    maxTime = rdTiet15.SelectedTime.Value;
                }
            }
            res.ResObject = detail;
            return res;
        }
    }
}