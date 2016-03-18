using System;

namespace Expenses.BL.Entities
{
    public class Entity : IUnique
    {
        public long Id { get; set; }

        public string Comment { get; set; }

        public virtual void CheckFields(){
        }
    }
}

