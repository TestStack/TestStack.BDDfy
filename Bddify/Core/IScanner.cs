namespace Bddify.Core
{
    public interface IScanner
    {
        Scenario Scan(object testObject);
    }
}