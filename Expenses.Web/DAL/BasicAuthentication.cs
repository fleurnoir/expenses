using System;
using Expenses.Web.DAL;
using Expenses.BL.Service;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Security;
using System.Web.Security;

namespace Expenses.Web.DAL
{
    public class BasicAuthentication : IAuthentication, IMvcActionFilter, ICurrentUserProvider
    {
        IAuthenticationService m_service;

        public BasicAuthentication(IAuthenticationService service)
        {
            if (service == null)
                throw new ArgumentNullException ("service");
            m_service = service;
        }

        private const string UserIdKey = "UserId";

        public bool Login(string login, string password)
        {
            var dbUser = m_service.GetUser (login, password);
            if (dbUser != null) 
            {
                HttpContext.Current.Session [UserIdKey] = dbUser.Id;
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
            context.Session [UserIdKey] = null;
        }

        public bool IsLoggedIn => HttpContext.Current.Session[UserIdKey] != null;

        public void OnActionExecuting (ActionExecutingContext filterContext)
        {
            var context = HttpContext.Current;
            var userId = context.Session [UserIdKey] as long?;
            if (userId != null)
                return;

            context.Response.AddHeader("WWW-Authenticate", String.Format("Basic"));
            filterContext.Result = new HttpUnauthorizedResult();
        }

        public long GetUserId ()
        {
            var result = HttpContext.Current.Session[UserIdKey] as long?;
            if (result == null)
                throw new SecurityException ("User not authenticated");
            return (long)result;
        }
    }
}

