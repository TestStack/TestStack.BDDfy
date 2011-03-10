using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace Bddify
{
    public static class BddifyExtensions
    {
        public static void Bddify(this object bddee)
        {
            BddifyReporter.PrintOutput = BddifyReporter.DefaultPrintOutput;
            var bdder = new Bddifier(new BddifyReporter(), new Scanner(), new InconclusiveException(string.Empty),  bddee);
            //var stack = new StackTrace(true);
            //var frames = stack.GetFrames();
            //if (frames != null && frames.Any(f => f.GetMethod().Name == ""))
            //{
            //    Console.WriteLine(frames.First(f => f.GetMethod().Name == "").GetMethod().Name);
            //}
            bdder.Run();
        }
    }
}