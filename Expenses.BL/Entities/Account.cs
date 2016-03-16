using System;

namespace Expenses.BL.Entities
{
    public class Account : IUnique
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public long CurrencyId { get; set; }

        public double Amount { get; set; }

        public string Comment { get; set; }
    }
}

