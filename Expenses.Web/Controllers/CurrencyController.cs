using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using Expenses.BL.Entities;
using Expenses.BL.Service;
using Expenses.Web.Common;
using Expenses.Common.Utils;

namespace Expenses.Web.Controllers
{
    public class CurrencyController : ExpensesController<Currency>
    {
    }
}
