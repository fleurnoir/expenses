using System;

namespace Expenses.BL.Entities
{
    public class User : Entity
    {
        public string Login { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public override void CheckFields ()
        {
            if (String.IsNullOrEmpty (Login))
                throw new ArgumentException ($"The field {nameof(Login)} cannot be empty");
            if (String.IsNullOrEmpty (PasswordHash))
                throw new ArgumentException ($"The field {nameof(PasswordHash)} cannot be empty");
        }
    }
}

