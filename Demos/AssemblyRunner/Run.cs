using System;
using Bddify.Core;

namespace AssemblyRunner
{
    public class Run
    {
        [STAThread]
        public static void Main()
        {
            typeof(Run).Assembly.Bddify(t => t.Name.StartsWith("When", StringComparison.Ordinal));
            Console.ReadLine();
        }
    }
}