using System.IO;
using System.Text.RegularExpressions;
using System;

namespace Bddify
{
    public static class BddifyExtensions
    {
        public static void Bddify(this object bddee)
        {
            Bddifier.PrintOutput = Bddifier.DefaultPrintOutput;
            var bdder = new Bddifier();
            bdder.Run(bddee);
        }

        public static void BddifyIntoFile(this object bddee, string filePath)
        {
            using (var file = File.AppendText(filePath))
            {
                file.AutoFlush = true;

                Action<string> report =
                    s =>
                    {
                        Bddifier.DefaultPrintOutput(s);
                        file.WriteLine(s);
                    };

                Bddifier.PrintOutput = report;
                var bdder = new Bddifier();
                bdder.Run(bddee);
            }
        }

        public static string CreateSentenceCamelName(string name)
        {
            return Regex.Replace(name, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
        }
    }
}