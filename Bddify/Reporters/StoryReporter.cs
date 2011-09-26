using System.Diagnostics;
using Bddify.Core;

namespace Bddify.Reporters
{
    public class StoryReporter : IProcessor
    {
        private readonly string _reportFileName;
#if !(SILVERLIGHT)
        readonly TraceSource _traceSource = new TraceSource("Bddify.Reporter");
#endif

        public StoryReporter(string reportFileName)
        {
#if !(SILVERLIGHT)
            if (_traceSource.Listeners.Count == 0 ||
                (_traceSource.Listeners.Count == 1 && _traceSource.Listeners[0].GetType() == typeof(DefaultTraceListener)))
            {
                _traceSource.Switch = new SourceSwitch("default", "Information");
                _traceSource.Listeners.Add(new ConsoleReportTraceListener());

#if !(NET35)
                _traceSource.Listeners.Add(new HtmlReportTraceListener(reportFileName));
#endif
            }
#endif
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
            _traceSource.TraceData(TraceEventType.Information, (int)TraceId.Story, story);
#endif
        }
    }
}