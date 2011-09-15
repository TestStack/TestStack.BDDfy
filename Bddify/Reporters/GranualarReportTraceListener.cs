using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bddify.Core;

namespace Bddify.Reporters
{
    public class GranualarReportTraceListener : TraceListener
    {
        readonly List<Exception> _exceptions = new List<Exception>();
        private int _longestStepSentence;
        private static readonly TraceSource TraceSouce = new TraceSource("Bddify.Reporter.Granualar");

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            var story = data as Story;
            if(story != null)
                TraceStory(story);
        }

        public override void Write(string message)
        {
            Console.Write(message);
            TraceSouce.TraceInformation(message);
        }

        public override void WriteLine(string message)
        {
            Console.WriteLine(message);
            TraceSouce.TraceInformation(message);
        }

        private void TraceStory(Story story)
        {
            ReportStoryHeader(story);

            var allSteps = story.Scenarios.SelectMany(s => s.Steps);
            if (allSteps.Any())
                _longestStepSentence = allSteps.Max(s => s.ReadableMethodName.Length);

            foreach (var scenario in story.Scenarios)
            {
                Report(scenario);

                if (scenario.Steps.Any())
                {
                    foreach (var step in scenario.Steps.Where(s => s.ShouldReport))
                        ReportOnStep(scenario, step);
                }
            }

            ReportExceptions();
        }

        void ReportStoryHeader(Story story)
        {
            if (story.MetaData == null || story.MetaData.Type == null)
                return;

            WriteLine("Story : " + story.MetaData.Title);
            WriteLine("\t" + story.MetaData.AsA);
            WriteLine("\t" + story.MetaData.IWant);
            WriteLine("\t" + story.MetaData.SoThat);
            WriteLine(string.Empty);
        }

        void ReportOnStep(Scenario scenario, ExecutionStep step)
        {
            var message =
                string.Format
                    ("\t{0}  [{1}] ",
                    step.ReadableMethodName.PadRight(_longestStepSentence + 5),
                    NetToString.Convert(step.Result.ToString()));

            // if all the steps have passed, there is no reason to make noise
            if (scenario.Result == StepExecutionResult.Passed)
                message = "\t" + step.ReadableMethodName;

            if (step.Exception != null)
            {
                _exceptions.Add(step.Exception);

                var exceptionReference = string.Format("[Details at {0} below]", _exceptions.Count);
                if (!string.IsNullOrEmpty(step.Exception.Message))
                    message += string.Format("[{0}] {1}", FlattenExceptionMessage(step.Exception.Message), exceptionReference);
                else
                    message += string.Format("{0}", exceptionReference);
            }

            WriteLine(message);
        }

        void ReportExceptions()
        {
            if (_exceptions.Count == 0)
                return;

            WriteLine("Exceptions:");

            for (int index = 0; index < _exceptions.Count; index++)
            {
                var exception = _exceptions[index];
                WriteLine(string.Format("  {0}. ", index + 1));

                if (!string.IsNullOrEmpty(exception.Message))
                    WriteLine(FlattenExceptionMessage(exception.Message));

                WriteLine(exception.StackTrace);
            }
        }

        static string FlattenExceptionMessage(string message)
        {
            return message
                .Replace("\t", " ") // replace tab with one space
                .Replace(Environment.NewLine, ", ") // replace new line with one space
                .Trim() // trim starting and trailing spaces
                .Replace("  ", " ")
                .TrimEnd(','); // chop any , from the end
        }

        void Report(Scenario scenario)
        {
            WriteLine("Scenario: " + scenario.ScenarioText);
        }
    }
}