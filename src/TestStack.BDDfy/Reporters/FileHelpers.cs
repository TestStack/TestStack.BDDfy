using System;
using System.IO;

namespace TestStack.BDDfy.Reporters
{
    public class FileHelpers
    {
        // http://stackoverflow.com/questions/52797/c-how-do-i-get-the-path-of-the-assembly-the-code-is-in#answer-283917
        internal static string AssemblyDirectory()
        {
            var basePath = AppContext.BaseDirectory;
            return Path.GetFullPath(basePath);
        }

    }
}
