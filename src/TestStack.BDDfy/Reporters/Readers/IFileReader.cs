namespace TestStack.BDDfy.Reporters.Readers
{
    public interface IFileReader
    {
        bool Exists(string path);
        string Read(string path);
    }
}