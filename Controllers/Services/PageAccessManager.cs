using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;

namespace SmartHR.Controllers.Services
{
    public class PageAccessManager
    {
        public static bool IsAccessAllowed(string Controller, string Action, IPrincipal User, string IP)
        {
            if (Controller == "Login")
                return true;

            if (Controller == "Dashboard" && User.IsInRole("1"))
                return true;

            return false;
        }

    }
}