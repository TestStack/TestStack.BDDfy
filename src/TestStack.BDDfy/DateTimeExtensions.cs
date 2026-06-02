using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy
{
    public static class DateTimeExtensions
    {
        public static string AsShortDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToString(Configurator.CultureInfo.DateTimeFormat.ShortDatePattern, Configurator.CultureInfo);
        }
    }
}
