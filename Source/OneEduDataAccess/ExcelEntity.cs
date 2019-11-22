using ClosedXML.Excel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    /// <summary>
    /// Summary description for ExcelEntity
    /// </summary>
    public class ExcelEntity
    {
        public ExcelEntity()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public string Name { get; set; }
        public string Type { get; set; }
        public XLColor Color { get; set; }
        public XLAlignmentHorizontalValues Align { get; set; }
        public HorizontalAlignment AlignNPOI { get; set; }
    }
    public class DataExcel
    {
        public DataExcel()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public string Name { get; set; }
        public int rowStart { get; set; }
        public int? indexSheet { get; set; }
        public string colStart { get; set; }
        public string colEnd { get; set; }
        public DataTable dt { get; set; }
        public List<ExcelEntity> lstColumn { get; set; }
        public bool have_stt { get; set; }
    }
}
