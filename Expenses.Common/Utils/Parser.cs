using System;
using System.Globalization;

namespace Expenses.Common.Utils
{
    public static class Parser
    {
        public static DateTime? ToDateTime(this string s, string format)
        {
            if (s == null)
                return null;
            DateTime result;
            if(DateTime.TryParseExact (s, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                return result;
            return null;
        }

        public static long? ToInt64(this string s)
        {
            if (s == null)
                return null;
            long result;
            if(Int64.TryParse (s, out result))
                return result;
            return null;
        }

    }
}

