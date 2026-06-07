namespace TestStack.BDDfy.Tests.Reporters
{
    public class CustomExceptionFormatter : IExceptionFormatter
    {
        public string Format(string message) => message;

        public string Format(Exception exception) => exception.Message;
    }
}
