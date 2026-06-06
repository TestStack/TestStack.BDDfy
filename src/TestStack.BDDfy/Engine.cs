using System.Text;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Processors;
using TestStack.BDDfy.Reporters.Writers;

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
            var finalErrors = new StringBuilder();
            foreach (var batchProcessor in Configurator.BatchProcessors.GetProcessors())
            {
                try { batchProcessor.Process(StoryCache.Stories); }
                catch (Exception ex)
                {
                    finalErrors.AppendLine($"Error processing batch processor {batchProcessor.GetType().FullName}: {ex}");
                }
            }

            if (finalErrors.Length > 0)
            {
                FileWriter.Singleton.WriteContents(finalErrors.ToString(), "errors.log");
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