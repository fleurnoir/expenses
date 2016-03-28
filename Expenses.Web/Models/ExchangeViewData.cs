using System;
using Expenses.BL.Entities;
using Expenses.Common.Utils;

namespace Expenses.Web.Models
{
    public class ExchangeViewData : Exchange
    {
        public ExchangeViewData(){}
        public ExchangeViewData(Exchange exchange)
        {
            Cloner.Clone (exchange, this);
        }

        public string SourceAccountName { get; set; }

        public string DestAccountName { get; set; }

        public long SourceCurrencyId { get; set; }

        public string SourceCurrencyName { get; set; }

        public long DestCurrencyId { get; set; }

        public string DestCurrencyName { get; set; }
    }
}

