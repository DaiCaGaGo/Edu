using OneEduDataAccess;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for NhomQuyenMenuBO
/// </summary>
public class NhomQuyenMenuBO
{
    #region Get

    public ResultEntity SetRoleToGroup(long idMENU, string maNHOMQUYEN, long? idNguoiDung)
    {
        ResultEntity res = new ResultEntity();
        res.Res = true;
        res.Msg = "Thành công";
        var QICache = new DefaultCacheProvider();
        try
        {
            using (oneduEntities context = new oneduEntities())
            {
                #region Query
                string strQuery1 = @"delete nguoi_dung_menu where ID_MENU= :0 and exists (select ID_MENU from NGUOI_DUNG where MA_NHOM_QUYEN= :1)";
                context.Database.ExecuteSqlCommand(strQuery1, idMENU, maNHOMQUYEN);
                string strQuery2 = @"insert into NGUOI_DUNG_MENU(ID_NGUOI_DUNG,ID_TRUONG,ID_MENU,TRANG_THAI,IS_XEM,IS_THEM,IS_SUA,IS_XOA,IS_DELETE,IS_SEND_SMS,IS_VIEW_INFOR,IS_EXPORT, NGAY_TAO, NGUOI_TAO) 
                    select nt.ID_NGUOI_DUNG,nt.ID_TRUONG,nqm.ID_MENU,nqm.TRANG_THAI,nqm.IS_XEM,nqm.IS_THEM,nqm.IS_SUA,nqm.IS_XOA,nqm.IS_DELETE,nqm.IS_SEND_SMS,nqm.IS_VIEW_INFOR,nqm.IS_EXPORT, nqm.NGAY_TAO, nqm.NGUOI_TAO     
                     FROM NHOM_QUYEN_MENU nqm join NGUOI_DUNG nd on nqm.MA_NHOM_QUYEN = nd.MA_NHOM_QUYEN join NGUOI_DUNG_TRUONG nt on nd.ID = nt.ID_NGUOI_DUNG
                    where nqm.ID_MENU = " + idMENU + "  and nqm.MA_NHOM_QUYEN = '" + maNHOMQUYEN + "'";
                #endregion
                context.Database.ExecuteSqlCommand(strQuery2);
                QICache.RemoveByFirstName("NGUOI_DUNG_MENU");
            }
        }
        catch (Exception ex)
        {
            res.Res = false;
            res.Msg = "Có lỗi xãy ra";
        }
        return res;
    }

    public List<NhomQuyenMenuEntity> getByNhomQuyenAndCapHoc(string ma_nhom_quyen, string ma_cap_hoc)
    {
        List<NhomQuyenMenuEntity> data = new List<NhomQuyenMenuEntity>();
        var QICache = new DefaultCacheProvider();
        string strKeyCache = QICache.BuildCachedKey("MENU", "NHOM_QUYEN_MENU", "getByNhomQuyenAndCapHoc", ma_nhom_quyen, ma_cap_hoc);
        if (!QICache.IsSet(strKeyCache))
        {
            using (oneduEntities context = new oneduEntities())
            {
                #region Query
                string strQuery = @"select m.ID,m.ID_CHA,m.TEN,nqm.ID as ID_NHOM_QUYEN_MENU,
                                    nqm.IS_XEM,
                                    nqm.IS_THEM,
                                    nqm.IS_SUA,
                                    nqm.IS_XOA,
                                    nqm.IS_SEND_SMS,
                                    nqm.IS_VIEW_INFOR,
                                    nqm.IS_EXPORT
                                    from MENU m
                                    left join nhom_quyen_menu nqm on m.ID=nqm.ID_MENU and nqm.ma_nhom_quyen=:0 
                                    and (nqm.TRANG_THAI is not null and nqm.TRANG_THAI=1)
                                    where m.ma_cap_hoc=:1
                                    order by m.THU_TU";
                #endregion
                data = context.Database.SqlQuery<NhomQuyenMenuEntity>(strQuery, ma_nhom_quyen, ma_cap_hoc).ToList();

                QICache.Set(strKeyCache, data, 300000);
            }
        }
        else
        {
            try
            {
                data = QICache.Get(strKeyCache) as List<NhomQuyenMenuEntity>;
            }
            catch
            {
                QICache.Invalidate(strKeyCache);
            }
        }
        return data;
    }
    public List<NHOM_QUYEN_MENU> GetMenuByNhomQuyen(string ma_nhom_quyen, string ma_cap_hoc, short? trang_thai)
    {
        var QICache = new DefaultCacheProvider();
        string strSession = QICache.BuildCachedKey("NHOM_QUYEN_MENU", "GetMenuByNhomQuyen", ma_nhom_quyen, ma_cap_hoc, trang_thai);

        var lsts = new List<NHOM_QUYEN_MENU>();
        if (!QICache.IsSet(strSession))
        {
            var context = new oneduEntities();
            var tmp = (from p in context.NHOM_QUYEN_MENU
                       where p.MA_NHOM_QUYEN == ma_nhom_quyen && p.MENU.MA_CAP_HOC == ma_cap_hoc
                       select p);
            if (trang_thai != null)
                tmp = tmp.Where(x => x.TRANG_THAI == trang_thai);
            lsts = tmp.ToList();
            QICache.Set(strSession, lsts, 300000);
        }
        else
        {
            try
            {
                lsts = QICache.Get(strSession) as List<NHOM_QUYEN_MENU>;
            }
            catch
            {
                QICache.Invalidate(strSession);
            }
        }
        return lsts;
    }
    public List<NhomQuyenMenuEntity> getNhomQuyenMenuByNguoiDung(string ma_nhom_quyen, long? idNguoiDung)
    {
        List<NhomQuyenMenuEntity> data = new List<NhomQuyenMenuEntity>();
        var QICache = new DefaultCacheProvider();
        string strKeyCache = QICache.BuildCachedKey("NHOM_QUYEN_MENU", "NGUOI_DUNG", "getNhomQuyenMenuByNguoiDung", ma_nhom_quyen, idNguoiDung);
        if (!QICache.IsSet(strKeyCache))
        {
            using (oneduEntities context = new oneduEntities())
            {
                #region Query
                string strQuery = @"select nqm.MA_NHOM_QUYEN AS ID_NHOM_QUYEN_MENU,
                                        nqm.ID_MENU,
                                        nqm.IS_XEM,
                                        nqm.IS_THEM,
                                        nqm.IS_SUA,
                                        nqm.IS_XOA,
                                        nqm.IS_SEND_SMS,
                                        nqm.IS_VIEW_INFOR
                                    from nhom_quyen_menu nqm 
                                    left join nguoi_dung nd on nqm.ma_nhom_quyen = nd.ma_nhom_quyen and nd.ma_nhom_quyen = :0
                                    where nd.id = :1";
                #endregion
                data = context.Database.SqlQuery<NhomQuyenMenuEntity>(strQuery, ma_nhom_quyen, idNguoiDung).ToList();

                QICache.Set(strKeyCache, data, 300000);
            }
        }
        else
        {
            try
            {
                data = QICache.Get(strKeyCache) as List<NhomQuyenMenuEntity>;
            }
            catch
            {
                QICache.Invalidate(strKeyCache);
            }
        }
        return data;
    }
    #endregion
    #region Set
    public ResultEntity insert(NHOM_QUYEN_MENU detail_in, long? nguoi)
    {
        ResultEntity res = new ResultEntity();
        res.Res = true;
        res.Msg = "Thành công";
        try
        {
            using (var context = new oneduEntities())
            {
                detail_in.ID = context.Database.SqlQuery<long>("SELECT NHOM_QUYEN_MENU_SEQ.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                detail_in.TRANG_THAI = (detail_in.IS_XEM == null || detail_in.IS_XEM.Value == false) ? Convert.ToInt16(0) : Convert.ToInt16(1);
                detail_in.NGAY_TAO = DateTime.Now;
                detail_in.NGUOI_TAO = nguoi;
                detail_in.NGAY_SUA = DateTime.Now;
                detail_in.NGUOI_SUA = nguoi;
                detail_in = context.NHOM_QUYEN_MENU.Add(detail_in);
                context.SaveChanges();
            }
            res.ResObject = detail_in;
            var QICache = new DefaultCacheProvider();
            QICache.RemoveByFirstName("NHOM_QUYEN_MENU");
        }
        catch (Exception ex)
        {
            res.Res = false;
            res.Msg = "Có lỗi xãy ra";
        }
        return res;
    }
    public ResultEntity update(NHOM_QUYEN_MENU detail_in, long? nguoi)
    {
        NHOM_QUYEN_MENU detail = new NHOM_QUYEN_MENU();
        ResultEntity res = new ResultEntity();
        res.Res = true;
        res.Msg = "Thành công";
        try
        {
            using (var context = new oneduEntities())
            {
                detail = (from p in context.NHOM_QUYEN_MENU
                          where p.ID == detail_in.ID && p.IS_DELETE != true
                          select p).FirstOrDefault();
                if (detail != null)
                {
                    detail.IS_SUA = detail_in.IS_SUA;
                    detail.IS_THEM = detail_in.IS_THEM;
                    detail.IS_XEM = detail_in.IS_XEM;
                    detail.IS_XOA = detail_in.IS_XOA;
                    detail.IS_SEND_SMS = detail_in.IS_SEND_SMS;
                    detail.IS_VIEW_INFOR = detail_in.IS_VIEW_INFOR;
                    detail.IS_EXPORT = detail_in.IS_EXPORT;
                    detail.TRANG_THAI = (detail.IS_XEM == null || detail.IS_XEM.Value == false) ? Convert.ToInt16(0) : Convert.ToInt16(1);
                    detail.NGAY_SUA = DateTime.Now;
                    detail.NGUOI_SUA = nguoi;
                    context.SaveChanges();
                    res.ResObject = detail;
                }
            }
            var QICache = new DefaultCacheProvider();
            QICache.RemoveByFirstName("NHOM_QUYEN_MENU");
        }
        catch (Exception ex)
        {
            res.Res = false;
            res.Msg = "Có lỗi xãy ra";
        }
        return res;
    }
    #endregion
}