using System;
using System.Globalization;

namespace TestStack.BDDfy
{
    public static class DateTimeExtensions
    {
        public static string AsShortDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
        }
    }
}
