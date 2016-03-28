using System;

namespace Expenses.BL.Entities
{
    public class Subcategory : Entity
    {
        public long CategoryId { get; set; }

        public string Name { get; set; }

        public override void CheckFields ()
        {
            if (String.IsNullOrEmpty (Name))
                throw new ArgumentException ($"The field {nameof(Name)} cannot be empty");
            if (CategoryId <= 0)
                throw new ArgumentException ("Category not set");
        }
    }
}

