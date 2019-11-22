using OneEduDataAccess.Model;
using OneEduDataAccess.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class QuyTinBO
    {
        #region get
        public QUY_TIN getQuyTin(short nam_gui, short thang_gui, long id_truong, short loai_tin, long? nguoi, out bool is_insert_new_quy)
        {
            is_insert_new_quy = false;
            DataAccessAPI dataAccessAPI = new DataAccessAPI();
            QUY_TIN data = new QUY_TIN();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("QUY_TIN", "getQuyTin", nam_gui, thang_gui, id_truong, loai_tin, nguoi, is_insert_new_quy);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.QUY_TIN where p.NAM_GUI == nam_gui && p.THANG_GUI == thang_gui && p.ID_TRUONG == id_truong && p.LOAI_TIN == loai_tin select p).FirstOrDefault();
                    #region "insert quy tin"
                    if (data == null)
                    {
                        is_insert_new_quy = true;
                        if (nam_gui == DateTime.Now.Year && thang_gui == DateTime.Now.Month)
                        {
                            ResultEntity res = insert(nam_gui, thang_gui, id_truong, loai_tin, nguoi);
                        }
                    }
                    #endregion
                    #region "update quy tin thang"
                    else
                    {
                        if (nam_gui == DateTime.Now.Year && thang_gui == DateTime.Now.Month)
                        {
                            ResultEntity res = cap_nhat_quy_tin(nam_gui, thang_gui, id_truong, loai_tin, nguoi, null, 0);
                        }
                    }
                    #endregion
                    #region get lai data sau khi da insert, update
                    data = (from p in context.QUY_TIN where p.NAM_GUI == nam_gui && p.THANG_GUI == thang_gui && p.ID_TRUONG == id_truong && p.LOAI_TIN == loai_tin select p).FirstOrDefault();
                    #endregion
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as QUY_TIN;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public QUY_TIN getQuyTinByNamHoc(short nam_hoc, long id_truong, short loai_tin, long? nguoi)
        {
            DataAccessAPI dataAccessAPI = new DataAccessAPI();
            QUY_TIN data = new QUY_TIN();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("QUY_TIN", "getQuyTinByNamHoc", nam_hoc, id_truong, loai_tin, nguoi);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    List<QUY_TIN> lst1 = (from p in context.QUY_TIN
                                          where p.NAM_GUI == nam_hoc && p.THANG_GUI >= 8 && p.THANG_GUI <= 12 && p.ID_TRUONG == id_truong && p.LOAI_TIN == loai_tin
                                          orderby p.NAM_GUI, p.THANG_GUI
                                          select p).ToList();
                    List<QUY_TIN> lst2 = (from p in context.QUY_TIN
                                          where p.NAM_GUI == nam_hoc + 1 && p.THANG_GUI >= 1 && p.THANG_GUI <= 5 && p.ID_TRUONG == id_truong && p.LOAI_TIN == loai_tin
                                          orderby p.NAM_GUI, p.THANG_GUI
                                          select p).ToList();
                    #region Cộng quỹ đã cấp
                    if (lst1 == null) lst1 = new List<QUY_TIN>();
                    if (lst2 == null) lst2 = new List<QUY_TIN>();
                    lst1.AddRange(lst2);
                    lst1 = lst1.OrderBy(p => p.NAM_GUI).ThenBy(p => p.THANG_GUI).ToList();
                    data.TONG_CAP = lst1.AsEnumerable().Sum(o => o.TONG_CAP);
                    data.TONG_THEM = lst1.AsEnumerable().Sum(o => o.TONG_THEM);
                    data.TONG_DA_SU_DUNG = lst1.AsEnumerable().Sum(o => o.TONG_DA_SU_DUNG);
                    data.TONG_CON = data.TONG_CAP + data.TONG_THEM - data.TONG_DA_SU_DUNG;
                    #endregion
                    #region Cộng quỹ dự đoán
                    TruongBO truongBO = new TruongBO();
                    GoiTinBO goiTinBO = new GoiTinBO();
                    TRUONG truongDetail = new TRUONG();
                    GOI_TIN goiTinDetail = new GOI_TIN();
                    truongDetail = truongBO.getTruongById(id_truong);
                    if (truongDetail != null && truongDetail.MA_GOI_TIN != null)
                        goiTinDetail = goiTinBO.getGoiTinByMa(truongDetail.MA_GOI_TIN.Value);

                    QUY_TIN detail = new QUY_TIN();
                    detail.LOAI_TIN = loai_tin;
                    if (loai_tin == SYS_Loai_Tin.Tin_Lien_Lac)
                        detail.SO_TIN_HS = (goiTinDetail.SO_TIN_LIEN_LAC_HS == null ? 0 : goiTinDetail.SO_TIN_LIEN_LAC_HS.Value);
                    if (loai_tin == SYS_Loai_Tin.Tin_Thong_Bao)
                        detail.SO_TIN_HS = goiTinDetail.SO_TIN_THONG_BAO_HS == null ? 0 : goiTinDetail.SO_TIN_THONG_BAO_HS.Value;
                    if (truongDetail != null)
                        detail.SO_HS_DK = (truongDetail.SO_HS_DANG_KY == null ? 0 : truongDetail.SO_HS_DANG_KY.Value);
                    detail.TONG_CAP = detail.SO_HS_DK * detail.SO_TIN_HS;
                    detail.TONG_THEM = 0;


                    short nam_ht = Convert.ToInt16(DateTime.Now.Year);
                    short thang_ht = Convert.ToInt16(DateTime.Now.Month);
                    if (truongDetail != null && truongDetail.MA_GOI_TIN != null && goiTinDetail != null)
                    {
                        if (loai_tin == SYS_Loai_Tin.Tin_Lien_Lac)
                        {
                            if (nam_ht == nam_hoc)
                                for (int i = thang_ht; i <= 12; i++)
                                {
                                    data.TONG_CAP += (truongDetail.SO_HS_DANG_KY == null ? 0 : truongDetail.SO_HS_DANG_KY.Value)
                                    * (goiTinDetail.SO_TIN_LIEN_LAC_HS == null ? 0 : goiTinDetail.SO_TIN_LIEN_LAC_HS.Value)
                                    * 4;
                                }

                            for (int i = 1; i <= 5; i++)
                            {
                                if ((nam_ht == nam_hoc + 1 && i > thang_ht) || (nam_ht == nam_hoc))
                                {
                                    data.TONG_CAP += (truongDetail.SO_HS_DANG_KY == null ? 0 : truongDetail.SO_HS_DANG_KY.Value)
                                    * (goiTinDetail.SO_TIN_LIEN_LAC_HS == null ? 0 : goiTinDetail.SO_TIN_LIEN_LAC_HS.Value)
                                    * 4;
                                }
                            }
                        }
                        if (loai_tin == SYS_Loai_Tin.Tin_Thong_Bao)
                        {
                            if (nam_ht == nam_hoc)
                                for (int i = thang_ht; i <= 12; i++)
                                {
                                    data.TONG_CAP += (truongDetail.SO_HS_DANG_KY == null ? 0 : truongDetail.SO_HS_DANG_KY.Value)
                                    * (goiTinDetail.SO_TIN_THONG_BAO_HS == null ? 0 : goiTinDetail.SO_TIN_THONG_BAO_HS.Value);
                                }

                            for (int i = 1; i <= 5; i++)
                            {
                                if ((nam_ht == nam_hoc + 1 && i > thang_ht) || (nam_ht == nam_hoc))
                                {
                                    data.TONG_CAP += (truongDetail.SO_HS_DANG_KY == null ? 0 : truongDetail.SO_HS_DANG_KY.Value)
                                    * (goiTinDetail.SO_TIN_THONG_BAO_HS == null ? 0 : goiTinDetail.SO_TIN_THONG_BAO_HS.Value);
                                }
                            }
                        }
                    }
                    #endregion
                    data.TONG_CON = data.TONG_CAP + data.TONG_THEM - data.TONG_DA_SU_DUNG;
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as QUY_TIN;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<ThongkeQuyTinEntiy> thongKeQuyTin(short? ma_tinh, short? ma_huyen, long? id_truong, int nam_hoc, int? thang)
        {
            List<ThongkeQuyTinEntiy> data = new List<ThongkeQuyTinEntiy>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("QUY_TIN", "TRUONG", "thongKeQuyTin", ma_tinh, ma_huyen, id_truong, nam_hoc, thang);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    try
                    {
                        string strQuery = @"select q.ID_TRUONG,t.TEN 
                                            ,sum(case when q.LOAI_TIN=1 then TONG_CAP else 0 end) as TONG_CAP_LL
                                            ,sum(case when q.LOAI_TIN=2 then TONG_CAP else 0 end) as TONG_CAP_TB
                                            ,sum(case when q.LOAI_TIN=1 then TONG_THEM else 0 end) as TONG_THEM_LL
                                            ,sum(case when q.LOAI_TIN=2 then TONG_THEM else 0 end) as TONG_THEM_TB
                                            ,sum(case when q.LOAI_TIN=1 then TONG_DA_SU_DUNG else 0 end) as TONG_DA_GUI_LL
                                            ,sum(case when q.LOAI_TIN=2 then TONG_DA_SU_DUNG else 0 end) as TONG_DA_GUI_TB
                                            ,case when sum(case when q.LOAI_TIN=1 then TONG_CAP else 0 end)=0 then 0
                                                  else sum(case when q.LOAI_TIN=1 then TONG_DA_SU_DUNG else 0 end)*100.0/sum(case when q.LOAI_TIN=1 then TONG_CAP else 0 end) end as PHAN_TRAM_LL
                                            ,case when sum(case when q.LOAI_TIN=2 then TONG_CAP else 0 end)=0 then 0
                                                  else sum(case when q.LOAI_TIN=2 then TONG_DA_SU_DUNG else 0 end)*100.0/sum(case when q.LOAI_TIN=2 then TONG_CAP else 0 end) end as PHAN_TRAM_TB
                                            from QUY_TIN q
                                            join TRUONG t on q.ID_TRUONG=t.ID ";
                        if (ma_tinh != null)
                            strQuery += string.Format(@" and t.MA_TINH_THANH ={0} ", ma_tinh);
                        if (ma_huyen != null)
                            strQuery += string.Format(@" and t.MA_QUAN_HUYEN ={0} ", ma_huyen);
                        strQuery += " where (q.NAM_GUI * 100 +  q.THANG_GUI) >= :0 and (q.NAM_GUI * 100 +  q.THANG_GUI) <= :1 ";
                        if (id_truong != null && id_truong != 0)
                            strQuery += string.Format(@" and t.ID ={0} ", id_truong);
                        if (thang != null)
                            strQuery += string.Format(@" and q.THANG_GUI= {0} ", thang);
                        //strQuery += @" group by q.ID_TRUONG,t.TEN
                        //              order by t.Ten";
                        strQuery += @" group by q.ID_TRUONG,t.TEN";

                        string sql = "select * from (" + strQuery + ") a where (tong_da_gui_ll > 0 or tong_da_gui_tb > 0) order by Ten";

                        data = context.Database.SqlQuery<ThongkeQuyTinEntiy>(sql, nam_hoc * 100 + 8, (nam_hoc + 1) * 100 + 7).ToList();

                        QICache.Set(strKeyCache, data, 300000);
                    }
                    catch (Exception ex)
                    { }
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<ThongkeQuyTinEntiy>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }

        public QUY_TIN getQuyTinByID(long id)
        {
            QUY_TIN data = new QUY_TIN();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("QUY_TIN", "getQuyTinByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.QUY_TIN where p.ID == id select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as QUY_TIN;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        #endregion
        #region set
        #region update
        public ResultEntity cap_nhat_quy_tin(short nam_gui, short thang_gui, long id_truong, short loai_tin, long? nguoi, long? them, long tin_moi_gui)
        {
            QUY_TIN detail = new QUY_TIN();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                TruongBO truongBO = new TruongBO();
                GoiTinBO goiTinBO = new GoiTinBO();
                TRUONG truongDetail = new TRUONG();
                GOI_TIN goiTinDetail = new GOI_TIN();
                truongDetail = truongBO.getTruongById(id_truong);
                if (truongDetail != null && truongDetail.MA_GOI_TIN != null)
                    goiTinDetail = goiTinBO.getGoiTinByMa(truongDetail.MA_GOI_TIN.Value);
                if (truongDetail != null && truongDetail.MA_GOI_TIN != null && goiTinDetail != null)
                {
                    using (var context = new oneduEntities())
                    {
                        detail = (from p in context.QUY_TIN
                                  where p.NAM_GUI == nam_gui && p.THANG_GUI == thang_gui
                                  && p.ID_TRUONG == id_truong && p.LOAI_TIN == loai_tin
                                  select p).FirstOrDefault();
                        if (detail != null)
                        {
                            detail.SO_HS_DK = truongDetail.SO_HS_DANG_KY == null ? 0 : truongDetail.SO_HS_DANG_KY.Value;
                            long SO_TIN_HS = detail.SO_TIN_HS;
                            if (goiTinDetail != null)
                            {
                                #region set quy tin he
                                if (loai_tin == SYS_Loai_Tin.Tin_Lien_Lac) SO_TIN_HS = (goiTinDetail.SO_TIN_LIEN_LAC_HS == null ? 0 : goiTinDetail.SO_TIN_LIEN_LAC_HS.Value) * 4;
                                if (loai_tin == SYS_Loai_Tin.Tin_Thong_Bao)
                                {
                                    if (thang_gui != 6 && thang_gui != 7)
                                        SO_TIN_HS = goiTinDetail.SO_TIN_THONG_BAO_HS == null ? 0 : goiTinDetail.SO_TIN_THONG_BAO_HS.Value;
                                    else SO_TIN_HS = goiTinDetail.SO_TIN_HE_HS == null ? 0 : goiTinDetail.SO_TIN_HE_HS.Value;
                                }
                                //if (thang_gui != 6 && thang_gui != 7)
                                //{
                                //    if (loai_tin == SYS_Loai_Tin.Tin_Lien_Lac) SO_TIN_HS = (goiTinDetail.SO_TIN_LIEN_LAC_HS == null ? 0 : goiTinDetail.SO_TIN_LIEN_LAC_HS.Value) * 4;
                                //    if (loai_tin == SYS_Loai_Tin.Tin_Thong_Bao) SO_TIN_HS = goiTinDetail.SO_TIN_THONG_BAO_HS == null ? 0 : goiTinDetail.SO_TIN_THONG_BAO_HS.Value;
                                //}
                                //else
                                //{
                                //    if (loai_tin == SYS_Loai_Tin.Tin_Lien_Lac) SO_TIN_HS = 0;
                                //    if (loai_tin == SYS_Loai_Tin.Tin_Thong_Bao) SO_TIN_HS = goiTinDetail.SO_TIN_HE_HS == null ? 0 : goiTinDetail.SO_TIN_HE_HS.Value;
                                //}
                                #endregion
                            }
                            detail.SO_TIN_HS = SO_TIN_HS;
                            detail.TONG_CAP = detail.SO_HS_DK * SO_TIN_HS;
                            if (them != null)
                                detail.TONG_THEM = them == null ? 0 : them.Value;
                            detail.TONG_DA_SU_DUNG = detail.TONG_DA_SU_DUNG + tin_moi_gui;
                            detail.TONG_CON = detail.TONG_CAP + detail.TONG_THEM - detail.TONG_DA_SU_DUNG;
                            detail.NGAY_SUA = DateTime.Now;
                            detail.NGUOI_SUA = nguoi;
                            context.SaveChanges();
                            res.ResObject = detail;
                        }
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("QUY_TIN");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity update(QUY_TIN detail_in, long? nguoi)
        {
            QUY_TIN detail = new QUY_TIN();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.QUY_TIN where p.ID == detail_in.ID select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.TONG_CON = detail_in.TONG_CON;
                        detail.TONG_DA_SU_DUNG = detail_in.TONG_DA_SU_DUNG;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("QUY_TIN");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        #endregion
        #region insert
        public ResultEntity insert(short nam_gui, short thang_gui, long id_truong, short loai_tin, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                TruongBO truongBO = new TruongBO();
                GoiTinBO goiTinBO = new GoiTinBO();
                TRUONG truongDetail = new TRUONG();
                GOI_TIN goiTinDetail = new GOI_TIN();
                truongDetail = truongBO.getTruongById(id_truong);
                if (truongDetail != null && truongDetail.IS_ACTIVE_SMS == true && truongDetail.MA_GOI_TIN != null)
                    goiTinDetail = goiTinBO.getGoiTinByMa(truongDetail.MA_GOI_TIN.Value);
                if (truongDetail != null && truongDetail.IS_ACTIVE_SMS == true && truongDetail.MA_GOI_TIN != null && goiTinDetail != null)
                {
                    using (var context = new oneduEntities())
                    {
                        QUY_TIN detail = new QUY_TIN();
                        detail.ID = context.Database.SqlQuery<long>("SELECT QUY_TIN_SEQ.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                        detail.LOAI_TIN = loai_tin;
                        detail.NAM_GUI = nam_gui;
                        detail.THANG_GUI = thang_gui;
                        detail.ID_TRUONG = id_truong;
                        detail.MA_GOI_TIN = truongDetail.MA_GOI_TIN.Value;

                        if (loai_tin == SYS_Loai_Tin.Tin_Lien_Lac)
                            detail.SO_TIN_HS = (goiTinDetail.SO_TIN_LIEN_LAC_HS == null ? 0 : goiTinDetail.SO_TIN_LIEN_LAC_HS.Value) * 4;
                        else if (loai_tin == SYS_Loai_Tin.Tin_Thong_Bao)
                        {
                            if (thang_gui != 6 && thang_gui != 7)
                                detail.SO_TIN_HS = (goiTinDetail.SO_TIN_THONG_BAO_HS == null ? 0 : goiTinDetail.SO_TIN_THONG_BAO_HS.Value);
                            else detail.SO_TIN_HS = (goiTinDetail.SO_TIN_HE_HS == null ? 0 : goiTinDetail.SO_TIN_HE_HS.Value);
                        }

                        detail.SO_HS_DK = (truongDetail.SO_HS_DANG_KY == null ? 0 : truongDetail.SO_HS_DANG_KY.Value);
                        detail.TONG_CAP = detail.SO_HS_DK * detail.SO_TIN_HS;
                        detail.TONG_THEM = 0;
                        detail.TONG_DA_SU_DUNG = 0;
                        detail.TONG_CON = detail.TONG_CAP;
                        detail.NGAY_TAO = DateTime.Now;
                        detail.NGUOI_TAO = nguoi;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        detail = context.QUY_TIN.Add(detail);
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("QUY_TIN");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        #endregion
        #endregion
    }
}
