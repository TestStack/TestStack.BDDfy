namespace TestStack.BDDfy.Configuration
{
    public static class Configurator
    {
        static Configurator()
        {
            AsyncVoidSupportEnabled = true;
        }

        public static bool AsyncVoidSupportEnabled { get; set; }

        private static readonly Processors ProcessorsFactory = new Processors();
        public static Processors Processors
        {
            get { return ProcessorsFactory; }
        }

        private static readonly BatchProcessors BatchProcessorFactory = new BatchProcessors();
        public static BatchProcessors BatchProcessors
        {
            get { return BatchProcessorFactory; }
        }

        private static readonly Scanners ScannersFactory = new Scanners();
        public static Scanners Scanners
        {
            get { return ScannersFactory; }
        }

        private static IKeyGenerator _idGenerator = new SequentialKeyGenerator();
        public static IKeyGenerator IdGenerator
        {
            get { return _idGenerator; }
            set { _idGenerator = value; }
        }


        private static IStepExecutor _stepExecutor = new StepExecutor();
        public static IStepExecutor  StepExecutor
        {
            get { return _stepExecutor; }
            set { _stepExecutor = value; }
        }

    }
}