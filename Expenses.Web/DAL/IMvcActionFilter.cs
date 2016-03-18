using System;
using System.Web.Mvc;

namespace Expenses.Web.DAL
{
    public interface IMvcActionFilter
    {
        void OnActionExecuting(ActionExecutingContext filterContext);
    }
}

