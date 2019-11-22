using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneEduDataAccess
{
    public class SYS_Type_Access
    {
        public static readonly int XEM = 1;
        public static readonly int THEM = 2;
        public static readonly int SUA = 3;
        public static readonly int XOA = 4;
        public static readonly int SEND_SMS = 5;
        public static readonly int VIEW_INFOR = 6;
        public static readonly int EXPORT = 7;
        public static readonly List<int> lstTypeAccess = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };
    }
}