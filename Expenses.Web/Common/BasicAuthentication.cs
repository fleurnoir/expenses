using System;
using Expenses.Web.Common;
using Expenses.BL.Service;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Security;
using System.Web.Security;
using Expenses.Common.Utils;

namespace Expenses.Web.Common
{
    public class BasicAuthentication : IAuthentication, IMvcActionFilter
    {
        IAuthenticationService m_service;

        public BasicAuthentication(IAuthenticationService service)
        {
            if (service == null)
                throw new ArgumentNullException ("service");
            m_service = service;
        }

        private const string ExpensesServiceKey = "Service";

        public bool Login(string login, string password)
        {
            var service = m_service.Login (login, password);
            if (service != null) 
            {
                HttpContext.Current.Session [ExpensesServiceKey] = service;
                return true;
            }
            return false;
           

//            // Ensure there's a return URL
//            if (context.Request.QueryString ["ReturnUrl"] == null) {
//                context.Response.Redirect (FormsAuthentication.LoginUrl + "?ReturnUrl=" + context.Server.UrlEncode (FormsAuthentication.DefaultUrl));
//            }
//
//            var auth = context.Request.Headers["Authorization"];
//            if (!String.IsNullOrEmpty(auth))
//            {
//                var cred = ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(auth.Substring(6))).Split(':');
//                var user = new { Name = cred[0], Pass = cred[1] };
//                var dbUser = m_service.GetUser (user.Name, user.Pass);
//                if (dbUser != null) 
//                {
//                    context.Session [UserIdKey] = dbUser.Id;
//                    FormsAuthentication.RedirectFromLoginPage(user.Name, false);
//                    return;
//                }
//            }
//
//            // Force the browser to pop up the login prompt
//            context.Response.StatusCode = 401;
//            context.Response.AppendHeader("WWW-Authenticate", "Basic");
//
//            // This gets shown if they click "Cancel" to the login prompt
//            context.Response.Write("You must log in to access this URL.");
        }

        public void Logout ()
        {
            var context = HttpContext.Current;
            var service = context.Session [ExpensesServiceKey] as IExpensesService;
            if (service != null) {
                service.Dispose ();
                context.Session [ExpensesServiceKey] = null;
            }
        }

        public bool IsLoggedIn => HttpContext.Current.Session[ExpensesServiceKey] != null;

        public void OnActionExecuting (ActionExecutingContext filterContext)
        {
            var context = HttpContext.Current;
            var service = context.Session [ExpensesServiceKey] as IExpensesService;
            if (service != null)
                return;

            // auto login for test purposes
//            service = m_service.Login("andy", "asdf");
//            context.Session [ExpensesServiceKey] = service;
//            return;

            context.Response.AddHeader("WWW-Authenticate", String.Format("Basic"));
            filterContext.Result = new HttpUnauthorizedResult();
        }

        public IExpensesService GetService ()
        {
            var result = HttpContext.Current.Session[ExpensesServiceKey] as IExpensesService;
            if (result == null)
                throw new SecurityException ("User not authenticated");
            return result;
        }
    }
}

