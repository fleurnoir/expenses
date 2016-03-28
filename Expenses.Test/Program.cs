using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Linq;

namespace Expenses.Test
{
    class MainClass
    {
        public static void Main (string[] args)
        {
//            Console.WriteLine (Convert.ToBase64String (new SHA512CryptoServiceProvider ()
//                .ComputeHash (Encoding.UTF8.GetBytes ("asdf"))));
            using (var file = new StreamReader (File.Open ("expenses.HTM", FileMode.Open), Encoding.GetEncoding ("Windows-1251"))) {
                var html = file.ReadToEnd ();
                ExpensesImport.Import(ExpensesHtmlExport.Export (html), "expenses");
                Console.WriteLine ("Import successful");
//                foreach (var item in result) {
//                    Console.WriteLine ($"{item.Date}:{item.Account}:{item.Category}:{item.Subcategory}:{item.Quantity}:{item.Units}:{item.Som}:{item.Euro}:{item.Dollar}:{item.Rouble}:{item.Comment}");
//                }
            }
        }
    }
}
