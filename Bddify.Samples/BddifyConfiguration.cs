using System.Collections.Generic;
using Bddify.Core;
using Bddify.Processors;
using Bddify.Reporters.ConsoleReporter;
using Bddify.Reporters.HtmlReporter;
using Bddify.Samples.TicTacToe;
using NUnit.Framework;

namespace Bddify.Samples
{
    [SetUpFixture]
    public class BddifyConfiguration
    {
        [SetUp]
        public void Config()
        {
            Configurator.Processors = () =>
                new List<IProcessor>
                                 {
                                     new TestRunner(),
                                     new ConsoleReporter(),
                                     new HtmlReportProcessor(),
                                     new CustomTextReporter(),
                                     new ExceptionProcessor()
                                 };
        }
    }
}