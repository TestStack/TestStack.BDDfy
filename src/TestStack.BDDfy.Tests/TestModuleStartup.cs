using System.Runtime.CompilerServices;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy.Tests
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