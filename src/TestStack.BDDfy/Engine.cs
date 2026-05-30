using System;
using System.Linq;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Processors;

namespace TestStack.BDDfy
{
    public class Engine(IScanner scanner)
    {
        private readonly IScanner _scanner = scanner;
        private Story? _story;

        static Engine()
        {
            System.Runtime.Loader.AssemblyLoadContext.Default.Unloading += context => InvokeBatchProcessors();
        }

        static void InvokeBatchProcessors()
        {
            foreach (var batchProcessor in Configurator.BatchProcessors.GetProcessors())
            {
                batchProcessor.Process(StoryCache.Stories);
            }
        }

        public Story Run()
        {
            _story = _scanner.Scan();

            var processors = Configurator.Processors.GetProcessors(_story).ToList();

            try
            {
                //run processors in the right order regardless of the order they are provided to the Bddfier
                foreach (var processor in processors.Where(p => p.ProcessType < ProcessType.Disposal).OrderBy(p => (int)p.ProcessType))
                    processor.Process(_story);
            }
            finally
            {
                foreach (var finallyProcessor in processors.Where(p => p.ProcessType >= ProcessType.Disposal).OrderBy(p => (int)p.ProcessType))
                    finallyProcessor.Process(_story);
            }

            return _story;
        }

        public Story Story { get { return _story ?? throw new InvalidOperationException("Story has not been run yet"); } }
    }
}