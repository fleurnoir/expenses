using System;

namespace Expenses.BL.Entities
{
    public class OperationBase : Entity
    {
        public DateTime OperationTime { get; set; }

        public long UserId { get; set; }
    }
}

