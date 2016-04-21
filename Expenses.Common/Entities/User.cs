using System;
using System.Text.RegularExpressions;

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
                throw new ArgumentException ("User name cannot be empty");

            if (Regex.Match(Login, "[a-zA-Z0-9_]+")?.Value != Login)
                throw new ArgumentException ("User name must contain only latin characters, numbers and underscore character");
            
            if (String.IsNullOrEmpty (PasswordHash))
                throw new ArgumentException ($"The field {nameof(PasswordHash)} cannot be empty");
        }
    }
}

