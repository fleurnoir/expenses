using System;

namespace Expenses.BL.Entities
{
    public class Account : Entity
    {
        public string Name { get; set; }

        public long CurrencyId { get; set; }

        public double Amount { get; set; }
    }
}

