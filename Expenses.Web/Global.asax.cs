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

            var dbManager = new SqliteDatabaseManager ("Databases", "data source=users.sqlite;foreign keys=true");
            var contextProvider = new AuthenticationContextProvider(dbManager);

            var authenticationService = new AuthenticationService (contextProvider);
            var authentication = new BasicAuthentication (authenticationService);
            Services.Register<IAuthenticationService> (authenticationService);
            Services.Register<IMvcActionFilter> (authentication);
            Services.Register<IAuthentication> (authentication);
            Services.RegisterProvider<IExpensesService>(authentication);

            ModelBinders.Binders.Add(typeof(double), new DoubleModelBinder());
            ModelBinders.Binders.Add(typeof(double?), new DoubleModelBinder());


        }
    }
}
