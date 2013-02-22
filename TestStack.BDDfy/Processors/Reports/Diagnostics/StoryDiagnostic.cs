using System.Collections.Generic;

namespace TestStack.BDDfy.Processors.Reports.Diagnostics
{
    public class StoryDiagnostic
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public List<Scenario> Scenarios { get; set; }

        public class Scenario
        {
            public string Name { get; set; }
            public int Duration { get; set; }
            public List<Step> Steps { get; set; }
        }

        public class Step
        {
            public string Name { get; set; }
            public int Duration { get; set; }
        }
    }
}