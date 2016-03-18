﻿using System;
using System.Web.Mvc;
using Expenses.Common.Utils;

namespace Expenses.Web.DAL
{
    public class CustomActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting (ActionExecutingContext filterContext)
        {
            base.OnActionExecuting (filterContext);
            Services.Get<IMvcActionFilter> ().OnActionExecuting(filterContext);
        }
    }
}

