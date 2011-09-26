using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bddify.Core;

namespace Bddify.Reporters
{
    public class ConsoleReportTraceListener : TraceListener
    {
        readonly List<Exception> _exceptions = new List<Exception>();
        private int _longestStepSentence;

        void Process(Story story)
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

        private static void ReportStoryHeader(Story story)
        {
            if (story.MetaData == null || story.MetaData.Type == null)
                return;

            Console.WriteLine("Story: " + story.MetaData.Title);
            Console.WriteLine("\t" + story.MetaData.AsA);
            Console.WriteLine("\t" + story.MetaData.IWant);
            Console.WriteLine("\t" + story.MetaData.SoThat);
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

#if !SILVERLIGHT
            if (step.Result == StepExecutionResult.Inconclusive || step.Result == StepExecutionResult.NotImplemented)
                Console.ForegroundColor = ConsoleColor.Yellow;
            else if (step.Result == StepExecutionResult.Failed)
                Console.ForegroundColor = ConsoleColor.Red;
            else if (step.Result == StepExecutionResult.NotExecuted)
                Console.ForegroundColor = ConsoleColor.Gray;
#endif

            Console.WriteLine(message);
#if !SILVERLIGHT
            Console.ForegroundColor = ConsoleColor.White;
#endif
        }

        void ReportExceptions()
        {
            Console.WriteLine();
            if (_exceptions.Count == 0)
                return;

            Console.Write("Exceptions:");

            for (int index = 0; index < _exceptions.Count; index++)
            {
                var exception = _exceptions[index];
                Console.WriteLine();
                Console.Write(string.Format("  {0}. ", index + 1));

                if (!string.IsNullOrEmpty(exception.Message))
                    Console.WriteLine(FlattenExceptionMessage(exception.Message));
                else
                    Console.WriteLine();

                Console.WriteLine(exception.StackTrace);
            }

            Console.WriteLine();
        }

        static string FlattenExceptionMessage(string message)
        {
            // ToDo: if gets complex will change it with a regex
            return message
                .Replace("\t", " ") // replace tab with one space
                .Replace(Environment.NewLine, ", ") // replace new line with one space
                .Trim() // trim starting and trailing spaces
                .Replace("  ", " ")
                .TrimEnd(','); // chop any , from the end
        }

        static void Report(Scenario scenario)
        {
#if !SILVERLIGHT
            Console.ForegroundColor = ConsoleColor.White;
#endif
            Console.WriteLine();
            Console.WriteLine("Scenario: " + scenario.ScenarioText);
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            var story = data as Story;
            if(story != null)
                Process(story);
        }

        public override void Write(string message)
        {
        }

        public override void WriteLine(string message)
        {
        }
    }
}