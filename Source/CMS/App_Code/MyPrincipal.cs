using OneEduDataAccess;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace CMS
{
    public class MyPrincipal : IPrincipal
    {
        public MyPrincipal(IIdentity identity)
        {
            Identity = identity;
        }

        public NGUOI_DUNGEntity User { get; set; }
        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            return true;
        }
    }
}