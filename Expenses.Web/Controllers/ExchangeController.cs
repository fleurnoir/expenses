using System;
using Expenses.BL.Entities;
using Expenses.Web.Models;
using System.Collections.Generic;
using Expenses.Common.Service;
using System.Linq;
using System.Web.Mvc;

namespace Expenses.Web.Controllers
{
    public class ExchangeController : ExpensesController<Exchange, ExchangeViewData>
    {
        protected override IEnumerable<ExchangeViewData> FillUpViewItems (IEnumerable<ExchangeViewData> items)
        {
            var storage = new CachedEntityStorage (Service);
            return items.Select (item => FillUpViewItem(item, storage));
        }

        private ExchangeViewData FillUpViewItem (ExchangeViewData item, IEntityStorage storage)
        {
            var sourceAccount = storage.Get<Account> (item.SourceAccountId);
            var destAccount = storage.Get<Account> (item.DestAccountId);
            var sourceCurrency = storage.Get<Currency> (sourceAccount.CurrencyId);
            var destCurrency = storage.Get<Currency> (destAccount.CurrencyId);

            item.SourceAccountName = sourceAccount.Name;
            item.DestAccountName = destAccount.Name;
            item.SourceCurrencyId = sourceCurrency.Id;
            item.SourceCurrencyName = sourceCurrency.ShortName;
            item.DestCurrencyId = destCurrency.Id;
            item.DestCurrencyName = destCurrency.ShortName;

            return item;
        }

        protected override ExchangeViewData FillUpViewItem (ExchangeViewData item)
        {
            return FillUpViewItem(item, new EntityStorage(Service));
        }

        protected override void PopulateSelectLists (ExchangeViewData entity)
        {
            var accounts = Service.GetAccounts ();
            var sourceAccountId = entity?.SourceAccountId ?? accounts.FirstOrDefault ()?.Id;
            var destAccountId = entity?.DestAccountId ?? accounts.FirstOrDefault ()?.Id;

            ViewBag.SourceAccountId = new SelectList(accounts, nameof(Account.Id), nameof(Account.Name), sourceAccountId);
            ViewBag.DestAccountId = new SelectList(accounts, nameof(Account.Id), nameof(Account.Name), destAccountId);

            if (sourceAccountId != null) {
                var currency = Service.GetAccount ((long)sourceAccountId).GetCurrency (Service);
                ViewBag.SourceCurrencyName = currency.ShortName;
                ViewBag.SourceCurrencyId = currency.Id;
            }
            if (destAccountId != null) {
                var currency = Service.GetAccount ((long)destAccountId).GetCurrency (Service);
                ViewBag.DestCurrencyName = currency.ShortName;
                ViewBag.DestCurrencyId = currency.Id;
            }
        }

        protected override void OnSaving (ExchangeViewData item)
        {
            var storage = new CachedEntityStorage (Service);
            if (storage.Get<Account> (item.SourceAccountId).CurrencyId == storage.Get<Account> (item.DestAccountId).CurrencyId) {
                item.DestAmount = item.SourceAmount;    
            }
        }

        [HttpPost]
        public JsonResult GetCurrency(long accountId)
        {
            var currency = Service.GetCurrency(Service.GetAccount (accountId).CurrencyId);
            return Json (new {id = currency.Id, name = currency.ShortName});
        }

    }
}

