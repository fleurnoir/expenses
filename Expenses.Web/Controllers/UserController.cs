using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Text;
using Expenses.Web.Common;
using Expenses.Common.Utils;
using Expenses.Web.Models;
using System.Net;

namespace Expenses.Web.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginData loginData)
        {
            if (loginData == null ||
                !Services.Get<IAuthentication> ().Login (loginData.Login, loginData.Password)) {
                return View (loginData == null ? null : new LoginData{ Login = loginData.Login });
            } else {
                FormsAuthentication.RedirectFromLoginPage (loginData.Login, false);
                return null;
            }
        }

        public void Logout()
        {
            Services.Get<IAuthentication> ().Logout ();
            Response.Redirect("~");
        }
    }
}
