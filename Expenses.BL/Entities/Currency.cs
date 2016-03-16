﻿using System;

namespace Expenses.BL.Entities
{
    public class Currency : IUnique
    {
        public int Id { get; set; }

        public string ShortName { get; set; }

        public string Name { get; set; }

        public string Comment { get; set; }
    }
}
