using System;
using Expenses.BL.Entities;
using Expenses.Common.Utils;

namespace Expenses.Web.Models
{
    public class AccountViewData : Account
    {
        public AccountViewData(){}

        public AccountViewData(Account account)
        {
            Cloner.Clone (account, this);
        }

        public string CurrencyName { get; set; }
    }
}

