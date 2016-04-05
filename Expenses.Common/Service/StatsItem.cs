using System;
using Expenses.BL.Entities;

namespace Expenses.Common.Service
{
    public class StatsItem
    {
        public long CurrencyId { get; set; }
        public OperationType Type { get; set; }
        public double Amount { get; set; }
    }
}

