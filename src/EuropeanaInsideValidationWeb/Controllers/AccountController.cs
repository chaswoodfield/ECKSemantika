using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EuropeanaInsideValidationWeb.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email = "", string password = "", bool remember=false)
        {
            if (email == "validator@semantika.si" && password == "123456")
            {
                FormsAuthentication.SetAuthCookie(email, remember);
                return RedirectToAction("Index", "Home", null);
            }
            else
            {

                ViewBag.Message = "Wrong password or username!";
            }

            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }
}
