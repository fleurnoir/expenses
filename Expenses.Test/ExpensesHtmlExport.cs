using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Globalization;

namespace Expenses.Test
{
    public class Expense
    {
        public DateTime Date { get; set; }
        public string Account { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }
        public double Quantity { get; set; }
        public string Units { get; set; }
        public double Som { get; set; }
        public double Euro { get; set; }
        public double Dollar { get; set; }
        public double Rouble { get; set; }
        public string Comment { get; set; }
    }

    public static class ExpensesHtmlExport
    {
        public static IList<Expense> Export(String html)
        {
            var regex = new Regex (
                "<tr>.*?"+
                    "<td.*?>(?<date>.*?)</td>.*?"+
                    "<td.*?>(?<account>.*?)</td>.*?"+
                    "<td.*?>(?<category>.*?)</td>.*?"+
                    "<td.*?>(?<subcategory>.*?)</td>.*?"+
                    "<td.*?>(?<quantity>.*?)</td>.*?"+
                    "<td.*?>(?<units>.*?)</td>.*?"+
                    "<td.*?>(?<som>.*?)</td>.*?"+
                    "<td.*?>.*?</td>.*?"+
                    "<td.*?>(?<euro>.*?)</td>.*?"+
                    "<td.*?>(?<dollar>.*?)</td>.*?"+
                    "<td.*?>.*?</td>.*?"+
                    "<td.*?>(?<rouble>.*?)</td>.*?"+
                    "<td.*?>.*?</td>.*?"+
                    "<td.*?>(?<comment>.*?)</td>.*?"+
                "</tr>", RegexOptions.Singleline);
            var matches = regex.Matches (html);
            return matches.OfType<Match>().Select(
                item=>new Expense
                {
                    Date = DateTime.ParseExact(item.Groups["date"].Value, "dd.MM.yyyy", CultureInfo.InvariantCulture),
                    Account = item.Groups["account"].Value,
                    Category = item.Groups["category"].Value,
                    Subcategory = item.Groups["subcategory"].Value,
                    Quantity = GetDouble(item, "quantity"),
                    Units = item.Groups["units"].Value,
                    Som = GetDouble(item, "som"),
                    Euro = GetDouble(item, "euro"),
                    Dollar = GetDouble(item, "dollar"),
                    Rouble = GetDouble(item, "rouble"),
                    Comment = item.Groups["comment"].Value.Replace("&nbsp",String.Empty).Replace("&quot;","\"")
                }).ToList();
        }

        private static readonly string Space = ((char)160).ToString ();

        private static double GetDouble(Match match, string group)
        {
            
            return Double.Parse (match.Groups [group].Value.Replace (Space, "").Replace (",", "."), CultureInfo.InvariantCulture);
        }
    }
}

