using System.Linq;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Processors;

namespace TestStack.BDDfy
{
    public class Engine(IScanner scanner)
    {
        private readonly IScanner _scanner = scanner;

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
            Story = _scanner.Scan();

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