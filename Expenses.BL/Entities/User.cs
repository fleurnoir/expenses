using System;

namespace Expenses.BL.Entities
{
    public class User : IUnique
    {
        public long Id { get; set; }

        public string Login { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string Comment { get; set; }
    }
}

