using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity.Infrastructure.Interception;
using Expenses.Web;
using Expenses.Web.Common;
using Expenses.BL.Service;
using Expenses.Common.Utils;

namespace Expenses.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var contextProvider = DataContextProvider.FromConnectionStringName("expenses");
            var authentication = new BasicAuthentication (new AuthenticationService (contextProvider));
            Services.Register<IMvcActionFilter> (authentication);
            Services.Register<IAuthentication> (authentication);
            Services.RegisterProvider<IExpensesService>(authentication);
        }
    }
}
