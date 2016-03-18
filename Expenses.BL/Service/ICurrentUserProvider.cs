using System;
using Expenses.BL.Entities;
using Expenses.BL.Service;

namespace Expenses.BL.Service
{
	public interface ICurrentUserProvider
	{
        long GetUserId ();
	}

}

