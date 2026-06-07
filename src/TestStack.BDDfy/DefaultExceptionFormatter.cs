namespace TestStack.BDDfy
{
    public class DefaultExceptionFormatter : IExceptionFormatter
    {
        const string SingleSpace = " ";
        public string Format(string message) =>
            string.Join(SingleSpace, message
                .Replace("\t", SingleSpace)
                .Split(["\r\n", "\n"], StringSplitOptions.None)
                .Select(s => s.Trim()))
                .TrimEnd(',');

        public string Format(Exception exception)
        {
            var message = string.IsNullOrEmpty(exception.Message)
                ? null
                : Format(exception.Message);

            if (exception.StackTrace is not null)
                return string.IsNullOrEmpty(message)
                    ? exception.StackTrace
                    : message + Environment.NewLine + exception.StackTrace;

            return message ?? string.Empty;
        }
    }
}
