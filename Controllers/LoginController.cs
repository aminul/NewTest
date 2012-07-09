using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SmartHR.Models;
using SmartHR.Core;

namespace SmartHR.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult LogOn()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                ViewBag.Message = "Please enter username and password for Sign In.";
                return View();
            }
        }

        public User ValidateUser(string username, string password)
        {
            using (AccountManagerEntities db = new AccountManagerEntities())
            {
                 User user=new User();
                try
                {
                    user = db.Users.Single(f => f.Username == username && f.Password == password);
                }
                catch
                {
                    user = null;

                }
                return user;
            }
        }


        [HttpPost]
        public ActionResult LogOn(LoginModel model, string returnUrl)
        {
            ViewBag.Message = "Please enter username and password for login.";
            if (ModelState.IsValid)
            {
                User user = ValidateUser(model.username, model.password);


                if (user != null)
                {
                   
                    var authTicket = new FormsAuthenticationTicket(1, model.username, DateTime.Now, DateTime.Now.AddMinutes(30), model.RememberMe,
                                                                "1");
                    string cookieContents = FormsAuthentication.Encrypt(authTicket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieContents)
                    {
                        Expires = authTicket.Expiration,
                        Path = FormsAuthentication.FormsCookiePath
                    };
                    Response.Cookies.Add(cookie);

                    if (!string.IsNullOrEmpty(returnUrl))
                        Response.Redirect(returnUrl);

                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    ViewBag.Message = "The user name or password provided is incorrect. Please try again";
               }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            ViewBag.Message = "Please enter username and password for Sign In.";
            return View("LogOn");
        }

    }
}
