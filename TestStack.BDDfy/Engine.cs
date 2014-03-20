using System;
using System.Linq;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Processors;

namespace TestStack.BDDfy
{
    public class Engine
    {
        private readonly string _storyCategory;
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

        public Engine(string storyCategory, IScanner scanner)
        {
            _storyCategory = storyCategory ?? "BDDfy";
            _scanner = scanner;
        }

        public Story Run()
        {
            _story = _scanner.Scan();
            _story.Category = _storyCategory;

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

        private Story _story;

        public Story Story
        {
            get { return _story; }
        }
    }
}