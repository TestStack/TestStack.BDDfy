using System;
using System.IO;
using Bddify;

namespace SutBehaviors
{
    public static class AnotherBddifyExtension
    {
        /// <summary>
        /// This is a lame implementation of Bddify.
        /// This is just to show you that it is rather simple to implement a file reporter
        /// </summary>
        /// <param name="bddee">The class to be BDDified</param>
        /// <param name="filePath">The path to the file to be generated</param>
        public static void Bddify(this object bddee, string filePath)
        {
            using (var file = File.AppendText(filePath))
            {
                file.AutoFlush = true;

                Action<string> report =
                    s =>
                    {
                        BddifyReporter.DefaultPrintOutput(s);
                        file.WriteLine(s);
                    };

                BddifyReporter.PrintOutput = report;
                var bdder = new Bddifier(new BddifyReporter(), new Scanner(), bddee);
                bdder.Run();
            }
        }

    }
}