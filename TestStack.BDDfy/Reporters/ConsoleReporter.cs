using System;
using System.Collections.Generic;
using System.Linq;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy.Reporters
{
    public class ConsoleReporter : IProcessor
    {
        readonly List<Exception> _exceptions = new List<Exception>();
        private int _longestStepSentence;

        public void Process(Story story)
        {
            ReportStoryHeader(story);

            var allSteps = story.Scenarios.SelectMany(s => s.Steps).ToList();
            if (allSteps.Any())
                _longestStepSentence = allSteps.Max(s => PrefixWithSpaceIfRequired(s).Length);

            //TODO This should be a reporting service. 
            // Maybe we introduce a reporting model for the moment we can duplicate logic
            foreach (var scenarioGroup in story.Scenarios.GroupBy(s => s.Id))
            {
                if (scenarioGroup.Count() > 1)
                {
                    // all scenarios in an example based scenario share the same header and narrative
                    var exampleScenario = story.Scenarios.First();
                    Report(exampleScenario);

                    if (exampleScenario.Steps.Any())
                    {
                        foreach (var step in exampleScenario.Steps.Where(s => s.ShouldReport))
                            ReportOnStep(exampleScenario, step, false);
                    }

                    Console.WriteLine();
                    WriteExamples(exampleScenario, scenarioGroup);
                }
                else
                {
                    foreach (var scenario in story.Scenarios)
                    {
                        Report(scenario);

                        if (scenario.Steps.Any())
                        {
                            foreach (var step in scenario.Steps.Where(s => s.ShouldReport))
                                ReportOnStep(scenario, step, true);
                        }
                    }
                }
            }

            ReportExceptions();
        }

        private static void WriteExamples(Scenario exampleScenario, IGrouping<string, Scenario> scenarioGroup)
        {
            Console.WriteLine(@"Examples: ");
            var numberColumns = exampleScenario.ExampleHeaders.Length + 2;
            var maxWidth = new int[numberColumns];
            var rows = new List<string[]>();
            Action<string, IEnumerable<object>, string> addRow = (result, r, error) =>
            {
                var row = new string[numberColumns];
                row[0] = result;
                var index = 1;
                foreach (var o in r)
                {
                    row[index++] = o.ToString();
                }
                row[numberColumns - 1] = error;
                for (var i = 0; i < numberColumns; i++)
                {
                    var rowValue = row[i];
                    if (rowValue != null && rowValue.Length > maxWidth[i])
                        maxWidth[i] = rowValue.Length;
                }
                rows.Add(row);
            };
            addRow(string.Empty, exampleScenario.ExampleHeaders, "Errors");
            foreach (var scenario in scenarioGroup)
            {
                var failingStep = scenario.Steps.FirstOrDefault(s => s.Result == Result.Failed);
                string error;
                if (failingStep == null)
                    error = null;
                else
                {
                    error = string.Format("Step: {0} failed with exception: {1}", failingStep.Title, FlattenExceptionMessage(failingStep.Exception.Message));
                }
                addRow(scenario.Result.ToString(), scenario.Examples, error);
            }

            foreach (var row in rows)
            {
                WriteExampleRow(row, maxWidth);
            }
        }

        private static void WriteExampleRow(string[] row, int[] maxWidth)
        {
            Console.Write("\t|");
            for (int index   = 0; index < row.Length; index++)
            {
                var col = row[index];
                Console.Write("\t{0}\t|", (col??string.Empty).Trim().PadRight(maxWidth[index]));
            }
            Console.Write("\n");
        }

        private static void ReportStoryHeader(Story story)
        {
            if (story.Metadata == null || story.Metadata.Type == null)
                return;

            Console.WriteLine("Story: " + story.Metadata.Title);
            if (!string.IsNullOrEmpty(story.Metadata.Narrative1))
                Console.WriteLine("\t" + story.Metadata.Narrative1);
            if (!string.IsNullOrEmpty(story.Metadata.Narrative2))
                Console.WriteLine("\t" + story.Metadata.Narrative2);
            if (!string.IsNullOrEmpty(story.Metadata.Narrative3))
                Console.WriteLine("\t" + story.Metadata.Narrative3);
        }

        static string PrefixWithSpaceIfRequired(Step step)
        {
            var stepTitle = step.Title;
            var executionOrder = step.ExecutionOrder;

            if (executionOrder == ExecutionOrder.ConsecutiveAssertion ||
                executionOrder == ExecutionOrder.ConsecutiveSetupState ||
                executionOrder == ExecutionOrder.ConsecutiveTransition)
                stepTitle = "  " + stepTitle; // add two spaces in the front for indentation.

            return stepTitle.Replace(Environment.NewLine, Environment.NewLine + "\t\t");
        }

        void ReportOnStep(Scenario scenario, Step step, bool includeResults)
        {
            if (!includeResults)
            {
                Console.WriteLine("\t{0}", PrefixWithSpaceIfRequired(step).PadRight(_longestStepSentence));
                return;
            }

            var message =
                string.Format
                    ("\t{0}  [{1}] ",
                    PrefixWithSpaceIfRequired(step).PadRight(_longestStepSentence + 5),
                    Configurator.Scanners.Humanize(step.Result.ToString()));

            // if all the steps have passed, there is no reason to make noise
            if (scenario.Result == Result.Passed)
                message = "\t" + PrefixWithSpaceIfRequired(step);

            if (step.Exception != null)
            {
                _exceptions.Add(step.Exception);

                var exceptionReference = string.Format("[Details at {0} below]", _exceptions.Count);
                if (!string.IsNullOrEmpty(step.Exception.Message))
                    message += string.Format("[{0}] {1}", FlattenExceptionMessage(step.Exception.Message), exceptionReference);
                else
                    message += string.Format("{0}", exceptionReference);
            }

            if (step.Result == Result.Inconclusive || step.Result == Result.NotImplemented)
                Console.ForegroundColor = ConsoleColor.Yellow;
            else if (step.Result == Result.Failed)
                Console.ForegroundColor = ConsoleColor.Red;
            else if (step.Result == Result.NotExecuted)
                Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
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
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine("Scenario: " + scenario.Title);
        }

        public ProcessType ProcessType
        {
            get { return ProcessType.Report; }
        }
    }
}