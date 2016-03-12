using System;

namespace Expenses.BL.Entities
{
    public class User : IUnique
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string PasswordHash { get; set; }
    }
}

