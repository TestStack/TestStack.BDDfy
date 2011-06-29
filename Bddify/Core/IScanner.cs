namespace Bddify.Core
{
    public interface IScanner
    {
        Story Scan(object testObject);
    }
}