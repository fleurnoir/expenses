using System;
using System.Linq;

namespace Expenses.BL.Entities
{
    public class Category : Entity
    {
        public string Name { get; set; }

        public OperationType Type { get; set; }

        public override void CheckFields ()
        {
            if (String.IsNullOrEmpty (Name))
                throw new ArgumentException ($"The field {nameof(Name)} cannot be empty");
            if (!Enum.GetValues(typeof(OperationType)).Cast<OperationType>().Any(type=>Type==type))
                throw new ArgumentException ("Unknown category type");
        }
    }
}

