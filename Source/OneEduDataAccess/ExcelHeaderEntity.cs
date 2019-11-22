using ClosedXML.Excel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class ExcelHeaderEntity
    {
        public ExcelHeaderEntity()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public string name { get; set; }
        public int colM { get; set; }
        public int rowM { get; set; }
        public int? parentIndex { get; set; }
        public string colName { get; set; }
        public List<ExcelHeaderEntity> lstChild { get; set; }
        public int rowIndex { get; set; }
        public double? fontSize { get; set; }
        public double? width { get; set; }
        public double? height { get; set; }
        public XLAlignmentHorizontalValues? Align { get; set; }
        public HorizontalAlignment? AlignNPOI { get; set; }
    }
}
