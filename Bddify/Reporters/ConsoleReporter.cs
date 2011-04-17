using System;
using System.Collections.Generic;
using System.Linq;
using Bddify.Core;

namespace Bddify.Reporters
{
    public class ConsoleReporter : IProcessor
    {
        readonly List<Exception> _exceptions = new List<Exception>();
        private int _longestStepSentence;
        private static Type _lastStoryType;

        public ProcessType ProcessType
        {
            get { return ProcessType.Report; }
        }

        public void Process(Story story)
        {
            var reporterRegistry
                = new Dictionary<StepExecutionResult, Action<ExecutionStep>>
                          {
                              {StepExecutionResult.Passed, s => ReportOnStep(s)},
                              {StepExecutionResult.Failed, s => ReportOnStep(s, true)},
                              {StepExecutionResult.Inconclusive, s => ReportOnStep(s)},
                              {StepExecutionResult.NotImplemented, s => ReportOnStep(s, true)},
                              {StepExecutionResult.NotExecuted, s => ReportOnStep(s)}
                          };

            _longestStepSentence = story.Scenarios.SelectMany(s => s.Steps).Max(s => s.ReadableMethodName.Length);

            ReportOnStory(story);

            foreach (var scenario in story.Scenarios)
            {
                Report(scenario);

                if (scenario.Steps.Any())
                {
                    foreach (var step in scenario.Steps.Where(s => s.ShouldReport))
                        reporterRegistry[step.Result](step);
                }

                Console.WriteLine();
            }

            ReportExceptions();
        }

        private static void ReportOnStory(Story story)
        {
            if(story.Type == null)
                return;

            if(story.Type == _lastStoryType)
                return; // we have already reported on this story

            _lastStoryType = story.Type;
            Console.WriteLine("Story: " + story.Narrative.Title);
            Console.WriteLine("\t" + story.Narrative.AsA);
            Console.WriteLine("\t" + story.Narrative.IWant);
            Console.WriteLine("\t" + story.Narrative.SoThat);
            Console.WriteLine();
        }

        void ReportOnStep(ExecutionStep step, bool reportOnException = false)
        {
            var message =
                string.Format
                    ("\t{0}  [{1}]",
                    step.ReadableMethodName.PadRight(_longestStepSentence + 5),
                    NetToString.FromCamelName(step.Result.ToString()));

            if(reportOnException)
            {
                _exceptions.Add(step.Exception);
                message += string.Format(" :: Exception Stack Trace below on number [{0}]", _exceptions.Count);
            }

            if (step.Result == StepExecutionResult.Inconclusive || step.Result == StepExecutionResult.NotImplemented)
                Console.ForegroundColor = ConsoleColor.Yellow;
            else if (step.Result == StepExecutionResult.Failed)
                Console.ForegroundColor = ConsoleColor.Red;
            else if (step.Result == StepExecutionResult.NotExecuted)
                Console.ForegroundColor = ConsoleColor.Gray;
            
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        void ReportExceptions()
        {
            if (_exceptions.Count == 0)
                return;

            Console.WriteLine("<< Exceptions' Details >>");

            for (int index = 0; index < _exceptions.Count; index++)
            {
                var exception = _exceptions[index];
                Console.WriteLine();
                Console.Write(string.Format("[{0}]: ", index + 1));
                
                if (!string.IsNullOrEmpty(exception.Message))
                    Console.WriteLine(FlattenExceptionMessage(exception.Message));
                else
                    Console.WriteLine();
                    
                Console.WriteLine(exception.StackTrace);
            }

            Console.WriteLine();
            Console.WriteLine("<< End of excetion details >>");
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
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Scenario: " +  scenario.ScenarioText);
        }
    }
}