﻿using System;
using Expenses.BL.Entities;

namespace Expenses.BL.Service
{
    public interface IDataContextProvider
    {
        ExpensesContext CreateContext();
    }
}

