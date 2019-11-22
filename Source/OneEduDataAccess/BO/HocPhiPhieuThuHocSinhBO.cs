using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.BO
{
    public class HocPhiPhieuThuHocSinhBO
    {
        #region get
        public List<HOC_PHI_PHIEU_THU_HOC_SINH> getPhieuThuByLopAndDotThu(long id_truong, short id_nam_hoc, short? id_khoi, long? id_lop, long? id_dot_thu, string noi_dung_thu)
        {
            List<HOC_PHI_PHIEU_THU_HOC_SINH> data = new List<HOC_PHI_PHIEU_THU_HOC_SINH>();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_PHI_PHIEU_THU_HOC_SINH", "getPhieuThuByLopAndDotThu", id_truong, id_nam_hoc, id_khoi, id_lop, id_dot_thu, noi_dung_thu);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    var tmp = (from p in context.HOC_PHI_PHIEU_THU_HOC_SINH where p.ID_TRUONG == id_truong && p.ID_NAM_HOC == id_nam_hoc && p.IS_DELETE != true select p);
                    if (id_khoi != null)
                        tmp = tmp.Where(x => x.ID_KHOI == id_khoi);
                    if (id_lop != null)
                        tmp = tmp.Where(x => x.ID_LOP == id_lop);
                    if (id_dot_thu != null)
                        tmp = tmp.Where(x => x.ID_DOT_THU == id_dot_thu);
                    if (!string.IsNullOrEmpty(noi_dung_thu))
                        tmp = tmp.Where(x => x.GHI_CHU.ToLower().Contains(noi_dung_thu.ToLower()));
                    data = tmp.ToList();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as List<HOC_PHI_PHIEU_THU_HOC_SINH>;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public HOC_PHI_PHIEU_THU_HOC_SINH getPhieuThuHocSinhByID(long id)
        {
            HOC_PHI_PHIEU_THU_HOC_SINH data = new HOC_PHI_PHIEU_THU_HOC_SINH();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_PHI_PHIEU_THU_HOC_SINH", "getPhieuThuHocSinhByID", id);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    data = (from p in context.HOC_PHI_PHIEU_THU_HOC_SINH where p.ID == id && p.IS_DELETE != true select p).FirstOrDefault();
                    QICache.Set(strKeyCache, data, 300000);
                }
            }
            else
            {
                try
                {
                    data = QICache.Get(strKeyCache) as HOC_PHI_PHIEU_THU_HOC_SINH;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public HOC_PHI_PHIEU_THU_HOC_SINH checkExistsPhieuThu(long id_truong, short id_khoi, long id_lop, long id_dot_thu, long id_hoc_sinh)
        {
            HOC_PHI_PHIEU_THU_HOC_SINH data = new HOC_PHI_PHIEU_THU_HOC_SINH();
            var QICache = new DefaultCacheProvider();
            string strKeyCache = QICache.BuildCachedKey("HOC_PHI_PHIEU_THU_HOC_SINH", "checkExistsPhieuThu", id_truong, id_khoi, id_lop, id_dot_thu, id_hoc_sinh);
            if (!QICache.IsSet(strKeyCache))
            {
                using (oneduEntities context = new oneduEntities())
                {
                    try
                    {
                        data = (from p in context.HOC_PHI_PHIEU_THU_HOC_SINH where p.ID_TRUONG == id_truong && p.ID_KHOI == id_khoi && p.ID_LOP == id_lop && p.ID_DOT_THU == id_dot_thu && p.ID_HOC_SINH == id_hoc_sinh && p.IS_DELETE != true select p).FirstOrDefault();
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
                    data = QICache.Get(strKeyCache) as HOC_PHI_PHIEU_THU_HOC_SINH;
                }
                catch
                {
                    QICache.Invalidate(strKeyCache);
                }
            }
            return data;
        }
        public List<BienLaiThuTienHSEntity> getPhieuThuTienHocSinh(long id_truong, short id_nam_hoc, short hoc_ky, string ma_cap_hoc, string sotien, string noidung, string sotienbangchu, List<short> lst_ma_khoi, List<long> lst_id_lop)
        {
            DataAccessAPI dataAccessAPI = new DataAccessAPI();
            string str_lst_ma_khoi = "", str_lst_id_lop = "";
            str_lst_ma_khoi = dataAccessAPI.ConvertListToString<short>(lst_ma_khoi, ",");
            str_lst_id_lop = dataAccessAPI.ConvertListToString<long>(lst_id_lop, ",");
            List<BienLaiThuTienHSEntity> data = new List<BienLaiThuTienHSEntity>();
            var QICache = new DefaultCacheProvider();
            using (oneduEntities context = new oneduEntities())
            {
                string query = @"select Ho_Ten,Lop.Ten as Ten_Lop, " + sotien + " as So_Tien, '" + noidung + "' as Noi_Dung, '" + sotienbangchu + "' " +
                    "as Viet_Bang_Chu, ROW_NUMBER() OVER (partition by LOP.THU_TU,LOP.ID order by hoc_sinh.thu_tu,NLSSORT(hoc_sinh.ten,'NLS_SORT=vietnamese'),NLSSORT(hoc_sinh.ho_dem,'NLS_SORT=vietnamese')) as STT  " +
                    " from hoc_sinh join Lop on hoc_sinh.ID_lop=lop.ID  where not (hoc_sinh.is_delete is not null and hoc_sinh.is_delete=1) and hoc_sinh.id_truong=:0 and hoc_sinh.id_nam_hoc=:1 and ma_cap_hoc=:2";
                #region Khối
                if (lst_ma_khoi != null && lst_ma_khoi.Count == 1)
                    query += string.Format(@" and hoc_sinh.ID_KHOI ={0}", lst_ma_khoi[0]);
                else if (lst_ma_khoi != null && lst_ma_khoi.Count > 1)
                {
                    query += string.Format(" and not ( hoc_sinh.ID_KHOI !={0}", lst_ma_khoi[0]);
                    for (int i = 1; i < lst_ma_khoi.Count; i++)
                    {
                        query += string.Format(" and hoc_sinh.ID_KHOI !={0}", lst_ma_khoi[i]);
                    }
                    query += " )";
                }
                #endregion
                #region Lớp
                if (lst_id_lop != null && lst_id_lop.Count == 1)
                    query += string.Format(@" and ID_LOP ={0}", lst_id_lop[0]);
                else if (lst_id_lop != null && lst_id_lop.Count > 1)
                {
                    query += string.Format(" and not ( ID_LOP !={0}", lst_id_lop[0]);
                    for (int i = 1; i < lst_id_lop.Count; i++)
                    {
                        query += string.Format(" and ID_LOP !={0}", lst_id_lop[i]);
                    }
                    query += " )";
                }
                #endregion
                if (hoc_ky == 1) query += @" and TRANG_THAI_HOC in (1,2,3,8,9,10) and (IS_DK_KY1 is not null and IS_DK_KY1=1)";
                else if (hoc_ky == 2) query += @" and TRANG_THAI_HOC in (1,2,3,6,7,10) and (IS_DK_KY2 is not null and IS_DK_KY2=1)";
                query += string.Format(@" order by LOP.THU_TU,LOP.ID,hoc_sinh.thu_tu,NLSSORT(hoc_sinh.ten,'NLS_SORT=vietnamese'),NLSSORT(hoc_sinh.ho_dem,'NLS_SORT=vietnamese') ");
                data = context.Database.SqlQuery<BienLaiThuTienHSEntity>(query, id_truong, id_nam_hoc, ma_cap_hoc).ToList();
            }
            return data;
        }
        public List<BienLaiThuTienHSEntity> getPhieuThuHocSinhByLopAndDotThu(long id_truong, short id_nam_hoc, short hoc_ky, short id_khoi, long id_lop, long id_dot_thu)
        {
            List<BienLaiThuTienHSEntity> data = new List<BienLaiThuTienHSEntity>();
            var QICache = new DefaultCacheProvider();
            using (oneduEntities context = new oneduEntities())
            {
                string query = @"select ROW_NUMBER() OVER (partition by hs.id_lop order by hs.thu_tu,NLSSORT(hs.ten,'NLS_SORT=vietnamese'),NLSSORT(hs.ho_dem,'NLS_SORT=vietnamese')) as STT,
                hs.HO_TEN,
                case when pt.IS_TIEN_AN is not null and pt.is_tien_an = 1 then pt.GHI_CHU || ', tiền ăn ' || pt.SO_TIEN_AN else pt.GHI_CHU end as NOI_DUNG,
                pt.TONG_TIEN || ' VNĐ' as TONG_TIEN, numbertext1(TONG_TIEN) AS VIET_BANG_CHU,l.TEN as TEN_LOP, t.TEN as TEN_TRUONG
                from HOC_PHI_PHIEU_THU_HOC_SINH pt
                join hoc_sinh hs on pt.id_truong = hs.id_truong and pt.id_khoi = hs.id_khoi
                and pt.id_nam_hoc = hs.id_nam_hoc and pt.id_lop = hs.id_lop and pt.id_hoc_sinh = hs.id
                join lop l on pt.id_truong=l.id_truong and pt.id_nam_hoc=l.id_nam_hoc and pt.id_lop=l.id
                join truong t on pt.id_truong=t.id
                where pt.id_truong=:0 and pt.id_khoi=:1 and pt.id_nam_hoc=:2 and pt.id_lop=:3 and pt.id_dot_thu=:4";
                if (hoc_ky == 1) query += @" and hs.TRANG_THAI_HOC in (1,2,3,8,9,10)";
                else if (hoc_ky == 2) query += @" and hs.TRANG_THAI_HOC in (1,2,3,6,7,10)";
                query += string.Format(@" order by hs.id_lop,hs.thu_tu,NLSSORT(hs.ten,'NLS_SORT=vietnamese'),NLSSORT(hs.ho_dem,'NLS_SORT=vietnamese') ");
                data = context.Database.SqlQuery<BienLaiThuTienHSEntity>(query, id_truong, id_khoi, id_nam_hoc, id_lop, id_dot_thu).ToList();
            }
            return data;
        }
        public static String NumberToTextVN(decimal total)
        {
            try
            {
                string rs = "";

                total = Math.Round(total, 0);
                string[] ch = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
                string[] rch = { "lẻ", "mốt", "", "", "", "lăm" };
                string[] u = { "", "mươi", "trăm", "ngàn", "", "", "triệu", "", "", "tỷ", "", "", "ngàn", "", "", "triệu" };
                string nstr = total.ToString();

                int[] n = new int[nstr.Length];
                int len = n.Length;
                for (int i = 0; i < len; i++)
                {
                    n[len - 1 - i] = Convert.ToInt32(nstr.Substring(i, 1));
                }

                for (int i = len - 1; i >= 0; i--)
                {
                    if (i % 3 == 2)// số 0 ở hàng trăm
                    {
                        if (n[i] == 0 && n[i - 1] == 0 && n[i - 2] == 0) continue;//nếu cả 3 số là 0 thì bỏ qua không đọc
                    }
                    else if (i % 3 == 1) // số ở hàng chục
                    {
                        if (n[i] == 0)
                        {
                            if (n[i - 1] == 0) { continue; }// nếu hàng chục và hàng đơn vị đều là 0 thì bỏ qua.
                            else
                            {
                                rs += " " + rch[n[i]]; continue;// hàng chục là 0 thì bỏ qua, đọc số hàng đơn vị
                            }
                        }
                        if (n[i] == 1)//nếu số hàng chục là 1 thì đọc là mười
                        {
                            rs += " mười"; continue;
                        }
                    }
                    else if (i != len - 1)// số ở hàng đơn vị (không phải là số đầu tiên)
                    {
                        if (n[i] == 0)// số hàng đơn vị là 0 thì chỉ đọc đơn vị
                        {
                            if (i + 2 <= len - 1 && n[i + 2] == 0 && n[i + 1] == 0) continue;
                            rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);
                            continue;
                        }
                        if (n[i] == 1)// nếu là 1 thì tùy vào số hàng chục mà đọc: 0,1: một / còn lại: mốt
                        {
                            rs += " " + ((n[i + 1] == 1 || n[i + 1] == 0) ? ch[n[i]] : rch[n[i]]);
                            rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);
                            continue;
                        }
                        if (n[i] == 5) // cách đọc số 5
                        {
                            if (n[i + 1] != 0) //nếu số hàng chục khác 0 thì đọc số 5 là lăm
                            {
                                rs += " " + rch[n[i]];// đọc số 
                                rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);// đọc đơn vị
                                continue;
                            }
                        }
                    }

                    rs += (rs == "" ? " " : ", ") + ch[n[i]];// đọc số
                    rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);// đọc đơn vị
                }
                if (rs[rs.Length - 1] != ' ')
                    rs += " đồng";
                else
                    rs += "đồng";

                if (rs.Length > 2)
                {
                    string rs1 = rs.Substring(0, 2);
                    rs1 = rs1.ToUpper();
                    rs = rs.Substring(2);
                    rs = rs1 + rs;
                }
                return rs.Trim().Replace("lẻ,", "lẻ").Replace("mươi,", "mươi").Replace("trăm,", "trăm").Replace("mười,", "mười");
            }
            catch
            {
                return "";
            }

        }
        #endregion
        #region set
        public ResultEntity update(HOC_PHI_PHIEU_THU_HOC_SINH detail_in, long? nguoi)
        {
            HOC_PHI_PHIEU_THU_HOC_SINH detail = new HOC_PHI_PHIEU_THU_HOC_SINH();
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    detail = (from p in context.HOC_PHI_PHIEU_THU_HOC_SINH
                              where p.ID == detail_in.ID
                              select p).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.ID_DOT_THU = detail_in.ID_DOT_THU;
                        detail.ID_TRUONG = detail_in.ID_TRUONG;
                        detail.ID_NAM_HOC = detail_in.ID_NAM_HOC;
                        detail.ID_KHOI = detail_in.ID_KHOI;
                        detail.ID_LOP = detail_in.ID_LOP;
                        detail.ID_HOC_SINH = detail_in.ID_HOC_SINH;
                        detail.IS_TIEN_AN = detail_in.IS_TIEN_AN;
                        detail.SO_TIEN_AN = detail_in.SO_TIEN_AN;
                        detail.TONG_TIEN = detail_in.TONG_TIEN;
                        detail.GHI_CHU = detail_in.GHI_CHU;
                        detail.NGAY_SUA = DateTime.Now;
                        detail.NGUOI_SUA = nguoi;
                        context.SaveChanges();
                        res.ResObject = detail;
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("HOC_PHI_PHIEU_THU_HOC_SINH");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity insert(HOC_PHI_PHIEU_THU_HOC_SINH detail_in, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    long newID = context.Database.SqlQuery<long>("SELECT HOC_PHI_PHIEU_THU_HOC_SINH_SEQ.NEXTVAL FROM SYS.DUAL").FirstOrDefault();
                    detail_in.ID = newID;
                    detail_in.NGAY_TAO = DateTime.Now;
                    detail_in.NGUOI_TAO = nguoi;
                    detail_in = context.HOC_PHI_PHIEU_THU_HOC_SINH.Add(detail_in);
                    context.SaveChanges();
                }
                res.ResObject = detail_in;
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("HOC_PHI_PHIEU_THU_HOC_SINH");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity delete(long id, long? nguoi, bool is_delete = false)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            try
            {
                using (var context = new oneduEntities())
                {
                    if (!is_delete)
                    {
                        var sql = @"update HOC_PHI_PHIEU_THU_HOC_SINH set IS_DELETE = 1,NGUOI_TAO=:0,NGAY_TAO=:1 where ID = :2";
                        context.Database.ExecuteSqlCommand(sql, nguoi, DateTime.Now, id);
                    }
                    else
                    {
                        var sql = @"delete from HOC_PHI_PHIEU_THU_HOC_SINH where ID = :0";
                        context.Database.ExecuteSqlCommand(sql, id);
                    }
                }
                var QICache = new DefaultCacheProvider();
                QICache.RemoveByFirstName("HOC_PHI_PHIEU_THU_HOC_SINH");
            }
            catch (Exception ex)
            {
                res.Res = false;
                res.Msg = "Có lỗi xãy ra";
            }
            return res;
        }
        public ResultEntity taoPHieuByLopAndDotThu(long id_truong, short id_khoi, short id_nam_hoc, int hoc_ky, long id_lop, long id_dot_thu, long? nguoi)
        {
            ResultEntity res = new ResultEntity();
            res.Res = true;
            res.Msg = "Thành công";
            var QICache = new DefaultCacheProvider();
            try
            {
                using (oneduEntities context = new oneduEntities())
                {
                    string strQuery = @"insert into HOC_PHI_PHIEU_THU_HOC_SINH (ID_DOT_THU, ID_TRUONG, ID_NAM_HOC, ID_KHOI, ID_LOP
                        ,ID_HOC_SINH, GHI_CHU, IS_TIEN_AN, SO_TIEN_AN, TONG_TIEN, NGAY_TAO, NGUOI_TAO)
                        select dtl.id_dot_thu,hs.id_truong, hs.id_nam_hoc, hs.id_khoi, hs.id_lop,
                        hs.id as id_hoc_sinh,dtl.ghi_chu,dtl.is_tien_an,dtl.so_tien_an,dtl.TONG_TIEN,:0,:1
                        from hoc_sinh hs
                        left join HOC_PHI_DOT_THU_LOP dtl on hs.id_truong=hs.id_truong and 
                        hs.id_khoi=dtl.id_khoi and hs.id_lop=dtl.id_lop and dtl.id_dot_thu=:2
                        where hs.id_truong=:3 and hs.id_khoi=:4 and hs.id_nam_hoc=:5 and hs.id_lop=:6";
                    if (hoc_ky == 1) strQuery += " and hs.TRANG_THAI_HOC in (1,2,3,8,9,10)";
                    else if (hoc_ky == 2) strQuery += " and hs.TRANG_THAI_HOC in (1,2,3,6,7,10)";
                    context.Database.ExecuteSqlCommand(strQuery, DateTime.Now, nguoi, id_dot_thu, id_truong, id_khoi, id_nam_hoc, id_lop);
                    QICache.RemoveByFirstName("HOC_PHI_PHIEU_THU_HOC_SINH");
                }
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
}
