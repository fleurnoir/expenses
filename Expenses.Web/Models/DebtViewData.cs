using System;
using Expenses.BL.Entities;
using Expenses.Common.Utils;

namespace Expenses.Web.Models
{
    public class DebtViewData : Debt
    {
        public DebtViewData(){}

        public DebtViewData(Debt debt) {
            Cloner.Clone (debt, this);
        }

        public string AccountName { get; set; }

        public long CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public string TypeName { get; set; }

        public double RestAmount { get; set; } 
    }
}

