using System.Diagnostics;
using Bddify.Core;

namespace Bddify.Reporters
{
    public class StoryReporter : IProcessor
    {
        private readonly string _reportFileName;
        static readonly TraceSource TraceSource = new TraceSource("Reporter");

        static StoryReporter()
        {
            TraceSource.Switch = new SourceSwitch("default", "Information");
            TraceSource.Listeners.Add(new ConsoleReportTraceListener());
            TraceSource.Listeners.Add(new HtmlReportTraceListener());
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