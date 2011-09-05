namespace Bddify.Core
{
    public interface IHasScanner
    {
        IScanner GetScanner(string scenarioTitle);
        object TestObject { get; }
    }
}
