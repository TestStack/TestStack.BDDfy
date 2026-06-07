namespace TestStack.BDDfy
{
    public interface IExceptionFormatter
    {
        string Format(string message);
        string Format(Exception exception);
    }
}
