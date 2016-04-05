using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Expenses.BL.Entities;
using Expenses.Web.Models;
using Expenses.Common.Service;
using Expenses.Common.Utils;

namespace Expenses.Web.Controllers
{
    public class DebtController : ExpensesController<Debt, DebtViewData>
    {
        private static IDictionary<DebtType, string> m_typeNames = new Dictionary<DebtType, string>
        {
            {DebtType.Borrow, Strings.IOwe},
            {DebtType.Lend, Strings.OwedToMe}
        };

        protected override IEnumerable<DebtViewData> FillUpViewItems (IEnumerable<DebtViewData> items)
        {
            var storage = new CachedEntityStorage (Service);
            return items.Select (item => FillUpViewItemCore(storage, item));
        }

        private DebtViewData FillUpViewItemCore(IEntityStorage storage, DebtViewData item) {
            var account = storage.Get<Account> (item.AccountId);
            var currency = storage.Get<Currency> (account.CurrencyId);
            item.AccountName = account.Name;
            item.CurrencyId = account.CurrencyId;
            item.CurrencyName = currency.ShortName;
            item.TypeName = m_typeNames.SafeGet(item.Type);
            item.RestAmount = item.Amount - item.RepayedAmount;
            return item;
        }

        protected override DebtViewData FillUpViewItem (DebtViewData item)
        {
            return FillUpViewItemCore(new EntityStorage(Service), item);
        }

        protected override void PopulateSelectLists (DebtViewData item)
        {
            var accounts = Service.GetAccounts ();
            var accountId = item?.AccountId ?? accounts.FirstOrDefault ()?.Id;
            ViewBag.AccountId = new SelectList (accounts, nameof (Account.Id), nameof (Account.Name), accountId);
            ViewBag.Type = new SelectList (m_typeNames, nameof(KeyValuePair<int,int>.Key), nameof(KeyValuePair<int,int>.Value), item?.Type ?? DebtType.Borrow);
            if (accountId != null && accountId > 0) {
                ViewBag.CurrencyName = Service.GetCurrency (accounts.First(a=>a.Id == accountId).CurrencyId).ShortName;
            }
        }
    }
}

