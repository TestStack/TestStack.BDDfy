namespace TestStack.BDDfy.Reporters.Diagnostics
{
    public class StoryDiagnostic
    {
        public string Name { get; set; } = null!;
        public int Duration { get; set; }
        public List<Scenario> Scenarios { get; set; } = [];

        public class Scenario
        {
            public string Name { get; set; } = null!;
            public int Duration { get; set; }
            public string Result { get; set; } = null!;
            public Example[] Examples { get; set; } = [];
            public List<Step> Steps { get; set; } = [];
        }

        public class Example
        {
            public int Duration { get; set; }
            public string? Error { get; set; }
            public string Result { get; set; } = null!;

            public string[] Headers { get; set; } = [];
            public string[] Values { get; set; } = [];
        }

        public class Step
        {
            public string Name { get; set; } = null!;
            public int Duration { get; set; }
        }
    }
}