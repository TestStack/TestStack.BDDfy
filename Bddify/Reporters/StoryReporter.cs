using System.Diagnostics;
using Bddify.Core;

namespace Bddify.Reporters
{
    public class StoryReporter : IProcessor
    {
        private readonly string _reportFileName;
#if !(SILVERLIGHT)
        static readonly TraceSource TraceSource = new TraceSource("Bddify.Reporter");

        static StoryReporter()
        {
            if (TraceSource.Listeners.Count == 0 ||
                (TraceSource.Listeners.Count == 1 && TraceSource.Listeners[0].GetType() == typeof(DefaultTraceListener)))
            {
                TraceSource.Switch = new SourceSwitch("default", "Information");
                TraceSource.Listeners.Add(new GranualarReportTraceListener());

#if !(NET35)
                TraceSource.Listeners.Add(new HtmlReportTraceListener());
#endif
            }
        }
#endif

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
#if !SILVERLIGHT
            TraceSource.TraceData(TraceEventType.Information, (int)TraceId.Story, story);
#endif
        }
    }
}