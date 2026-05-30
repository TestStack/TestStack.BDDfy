using System.Runtime.CompilerServices;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Tests;

namespace TestStack.BDDfy.Samples
{
    public class Startup
    {
        [ModuleInitializer]
        public static void Initialize()
        {
            Configurator.Processors.Add(() => new XUnitOutputReporter());
            Configurator.Processors.ConsoleReport.Enable();
        }
    }
}