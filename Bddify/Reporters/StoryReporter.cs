using System.Diagnostics;
using Bddify.Core;

namespace Bddify.Reporters
{
    public class StoryReporter : IProcessor
    {
        private readonly string _reportFileName;
        static readonly TraceSource TraceSource = new TraceSource("Bddify.Reporter");

        static StoryReporter()
        {
            if (TraceSource.Listeners.Count == 0 ||
                (TraceSource.Listeners.Count == 1 && TraceSource.Listeners[0].GetType() == typeof(DefaultTraceListener)))
            {
                TraceSource.Switch = new SourceSwitch("default", "Information");
                TraceSource.Listeners.Add(new ConsoleReportTraceListener());
                TraceSource.Listeners.Add(new HtmlReportTraceListener());
            }
        }

        public StoryReporter(string reportFileName)
        {
            // ToDo: should somehow pass this on to the listeners
            _reportFileName = reportFileName;
        }

        public ProcessType ProcessType
        {
            get { return ProcessType.Report; }
        }

        public void Process(Story story)
        {
            TraceSource.TraceData(TraceEventType.Information, (int)TraceId.Story, story);
        }
    }
}