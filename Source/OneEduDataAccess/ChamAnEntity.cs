using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class ChamAnEntity
    {
        public string MA_HS { get; set; }
        public string TEN_HS { get; set; }
        public string TEN { get; set; }
        public string HO_DEM { get; set; }
        public Nullable<short> THU_TU { get; set; }
        public short MA_GIOI_TINH { get; set; }
        public string TEN_GIOI_TINH { get; set; }
        public Nullable<System.DateTime> NGAY_SINH { get; set; }
        public long ID { get; set; }
        public long ID_HOC_SINH { get; set; }
        public long ID_LOP { get; set; }
        public long ID_TRUONG { get; set; }
        public short ID_KHOI { get; set; }
        public short ID_NAM_HOC { get; set; }
        public Nullable<short> HOC_KY { get; set; }
        public long ID_BUA_AN { get; set; }
        public short THANG { get; set; }
        #region Đăng ký ăn
        public int IS_BUA_AN_0 { get; set; }
        public int IS_BUA_AN_1 { get; set; }
        public int IS_BUA_AN_2 { get; set; }
        public int IS_BUA_AN_3 { get; set; }
        public int IS_BUA_AN_4 { get; set; }
        public int IS_BUA_AN_5 { get; set; }
        public int IS_BUA_AN_6 { get; set; }
        public int IS_BUA_AN_7 { get; set; }
        public int IS_BUA_AN_8 { get; set; }
        public int IS_BUA_AN_9 { get; set; }
        #endregion
        #region Chấm ăn
        public int BUA_AN_0 { get; set; }
        public int BUA_AN_1 { get; set; }
        public int BUA_AN_2 { get; set; }
        public int BUA_AN_3 { get; set; }
        public int BUA_AN_4 { get; set; }
        public int BUA_AN_5 { get; set; }
        public int BUA_AN_6 { get; set; }
        public int BUA_AN_7 { get; set; }
        public int BUA_AN_8 { get; set; }
        public int BUA_AN_9 { get; set; }
        #endregion
    }
}
