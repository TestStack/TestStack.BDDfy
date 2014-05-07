using System;
using System.Linq;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Processors;

namespace TestStack.BDDfy
{
    public class Engine
    {
        private readonly string _reportFilename;
        private readonly IScanner _scanner;

        static Engine()
        {
            AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
        }

        static void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            foreach (var batchProcessor in Configurator.BatchProcessors.GetProcessors())
            {
                batchProcessor.Process(StoryCache.Stories);
            }
        }

        public Engine(string reportFilename, IScanner scanner)
        {
            _reportFilename = reportFilename ?? "BDDfy";
            _scanner = scanner;
        }

        public Story Run()
        {
            Story = _scanner.Scan();
            Story.ReportFilename = _reportFilename;

            var processors = Configurator.Processors.GetProcessors(Story).ToList();

            try
            {
                //run processors in the right order regardless of the order they are provided to the Bddfier
                foreach (var processor in processors.Where(p => p.ProcessType < ProcessType.Disposal).OrderBy(p => (int)p.ProcessType))
                    processor.Process(Story);
            }
            finally
            {
                foreach (var finallyProcessor in processors.Where(p => p.ProcessType >= ProcessType.Disposal).OrderBy(p => (int)p.ProcessType))
                    finallyProcessor.Process(Story);
            }

            return Story;
        }

        public Story Story { get; private set; }
    }
}