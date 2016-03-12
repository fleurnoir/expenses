using System;

namespace Expenses.BL
{
    public class Account : IUnique
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CurrencyId { get; set; }

        public double Amount { get; set; }

        public string Comment { get; set; }
    }
}

