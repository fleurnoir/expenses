using System;
using System.Security.Cryptography;
using System.Text;

namespace Expenses.Test
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            Console.WriteLine (Convert.ToBase64String (new SHA512CryptoServiceProvider ()
                .ComputeHash (Encoding.UTF8.GetBytes ("asdf"))));
        }
    }
}
