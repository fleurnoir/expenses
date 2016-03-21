using System;
using Expenses.BL.Entities;
using System.Web.Mvc;
using System.Linq;
using Expenses.Web.Models;
using Expenses.Common.Utils;
using System.Collections.Generic;

namespace Expenses.Web.Controllers
{
    public class AccountController : ExpensesController<Account, AccountViewData>
    {
        protected override IEnumerable<AccountViewData> FillUpViewItems (IEnumerable<AccountViewData> items)
        {
            var currencies = Service.GetCurrencies ().ToDictionary(item=>item.Id, item=>item.ShortName);
            return items.Select (
                item => 
                { 
                    item.CurrencyName = currencies.SafeGet (item.CurrencyId);
                    return item;
                });
        }

        protected override AccountViewData FillUpViewItem (AccountViewData item)
        {
            item.CurrencyName = Service.GetCurrency (item.CurrencyId).Name;
            return item;
        }

        protected override void PopulateSelectLists (AccountViewData item)
        {
            var currencies = Service.GetCurrencies ();
            ViewBag.CurrencyId = new SelectList (currencies, nameof(Currency.Id), nameof(Currency.ShortName), item?.CurrencyId ?? currencies.FirstOrDefault()?.Id);
        }
    }
}

