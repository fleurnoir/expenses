using System;
using System.Web.Mvc;

namespace Expenses.Web.Common
{
    public interface IMvcActionFilter
    {
        void OnActionExecuting(ActionExecutingContext filterContext);
    }
}

