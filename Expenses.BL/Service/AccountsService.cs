using System;
using Expenses.BL.Entities;
using Expenses.BL.Common;
using System.Data;

namespace Expenses.BL.Service
{
    public class AccountsService : EntityService<Account>
    {
        public AccountsService(IDataContextProvider provider) : base(provider){}

        public override Account Update (Account item)
        {
            if (item == null)
                throw new ArgumentNullException (nameof(item));
            using (var db = CreateContext ()) {
                var account = db.Accounts.Find (item.Id);
                // Updating only name and comment, other fields cannot be changed this way
                account.Name = item.Name;
                account.Comment = item.Comment;
                db.SaveChanges ();
                return account;
            }
        }
    }
}

