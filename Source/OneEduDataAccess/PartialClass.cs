using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess.Model
{
    public partial class MENU
    {
        [NotMapped]
        public bool IS_EXIST_CHILD { get; set; }
    }
    public partial class CA_HOC
    {
        [NotMapped]
        public bool? IS_UPDATE { get; set; }
    }
    public partial class DM_BUA_AN
    {
        [NotMapped]
        public string TEN_KHOI { get; set; }
        [NotMapped]
        public string TEN_NHOM_TUOI_MN { get; set; }
    }
}
