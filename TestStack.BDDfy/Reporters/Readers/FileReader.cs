using System.IO;

namespace TestStack.BDDfy.Reporters.Readers
{
    public class FileReader : IFileReader
    {
        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        public string Read(string path)
        {
            return File.ReadAllText(path);
        }
    }
}