namespace TestStack.BDDfy.Configuration
{
    public static class Configurator
    {
        public static bool AsyncVoidSupportEnabled { get; set; } = true;

        public static Processors Processors { get; } = new();

        public static BatchProcessors BatchProcessors { get; } = new();

        public static Scanners Scanners { get; } = new();

        public static IKeyGenerator IdGenerator { get; set; } = new SequentialKeyGenerator();

        public static IStepExecutor StepExecutor { get; set; } = new StepExecutor();

        public static IHumanizer Humanizer { get; set; } = new DefaultHumanizer();
    }
}