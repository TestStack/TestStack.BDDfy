namespace TestStack.BDDfy.Configuration
{
    public static class Configurator
    {
        private static readonly Processors ProcessorsFactory = new Processors();
        public static Processors Processors 
        {
            get
            {
                return ProcessorsFactory;
            }
        }

        private static readonly BatchProcessors BatchProcessorFactory = new BatchProcessors();
        public static BatchProcessors BatchProcessors
        {
            get
            { return BatchProcessorFactory; }
        }

        private static readonly Scanners ScannersFactory = new Scanners();
        public static Scanners Scanners
        {
            get
            {
                return ScannersFactory;
            }
        }
    }
}