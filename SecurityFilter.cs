using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartHR.Controllers.Services;
using System.Web.Security;
using System.Security.Principal;

namespace SmartHR
{
    public class SecurityFilter : FilterAttribute, IAuthorizationFilter, IExceptionFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {

            HttpCookie authCookie = filterContext.HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                var identity = new GenericIdentity(authTicket.Name, "Forms");
                var principal = new GenericPrincipal(identity, new string[] { authTicket.UserData });
                filterContext.HttpContext.User = principal;
            }


            var Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var Action = filterContext.ActionDescriptor.ActionName;
            var User = filterContext.HttpContext.User;
            var IP = filterContext.HttpContext.Request.UserHostAddress;


            var isAccessAllowed = PageAccessManager.IsAccessAllowed(Controller, Action, User, IP);
            if (!isAccessAllowed)
            {
                FormsAuthentication.RedirectToLoginPage();
            }
        }

        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception != null && filterContext.Exception is System.Security.SecurityException)
            {
                var result = new ViewResult();
                result.ViewName = "SecurityError";
                filterContext.Result = result;
                filterContext.ExceptionHandled = true;
            }
        }
    }
}