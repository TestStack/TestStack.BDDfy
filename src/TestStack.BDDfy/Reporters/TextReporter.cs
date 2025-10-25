using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy.Reporters
{
    public class TextReporter : IProcessor
    {
        private readonly List<Exception> _exceptions = new();
        private readonly StringBuilder _text = new();
        private int _longestStepSentence;

        public void Process(Story story)
        {
            ReportStoryHeader(story);

            var allSteps = story.Scenarios.SelectMany(s => s.Steps)
                .Select(GetStepWithLines)
                .ToList();
            if (allSteps.Any())
                _longestStepSentence = allSteps.SelectMany(s => s.Item2.Select(l => l.Length)).Max();

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
                            ReportOnStep(exampleScenario, GetStepWithLines(step), false);
                    }

                    WriteLine();
                    WriteExamples(exampleScenario, scenarioGroup);
                    ReportTags(exampleScenario.Tags);
                }
                else
                {
                    foreach (var scenario in story.Scenarios)
                    {
                        Report(scenario);

                        if (scenario.Steps.Any())
                        {
                            foreach (var step in scenario.Steps.Where(s => s.ShouldReport))
                                ReportOnStep(scenario, GetStepWithLines(step), true);
                        }
                    }

                    var exampleScenario = story.Scenarios.First();
                    ReportTags(exampleScenario.Tags);
                }
            }

            ReportExceptions();
        }

        private static Tuple<Step, string[]> GetStepWithLines(Step s)
        {
            return Tuple.Create(s, s.Title.Replace("\r\n", "\n").Split('\n').Select(l => PrefixWithSpaceIfRequired(l, s.ExecutionOrder)).ToArray());
        }

        private void ReportTags(List<string> tags)
        {
            if (!tags.Any())
                return;

            WriteLine();
            WriteLine("Tags: {0}", string.Join(", ", tags));
        }

        private void WriteExamples(Scenario exampleScenario, IEnumerable<Scenario> scenarioGroup)
        {
            WriteLine("Examples: ");
            var scenarios = scenarioGroup.ToArray();
            var allPassed = scenarios.All(s => s.Result == Result.Passed);
            var exampleColumns = exampleScenario.Example.Headers.Length;
            var numberColumns = allPassed ? exampleColumns : exampleColumns + 2;
            var maxWidth = new int[numberColumns];
            var rows = new List<string[]>();

            Action<IEnumerable<string>, string, string> addRow = (cells, result, error) =>
            {
                var row = new string[numberColumns];
                var index = 0;

                foreach (var cellText in cells)
                    row[index++] = cellText;

                if (!allPassed)
                {
                    row[numberColumns - 2] = result;
                    row[numberColumns - 1] = error;
                }

                for (var i = 0; i < numberColumns; i++)
                {
                    var rowValue = row[i];
                    if (rowValue != null && rowValue.Length > maxWidth[i])
                        maxWidth[i] = rowValue.Length;
                }

                rows.Add(row);
            };

            addRow(exampleScenario.Example.Headers, "Result", "Errors");
            foreach (var scenario in scenarios)
            {
                var failingStep = scenario.Steps.FirstOrDefault(s => s.Result == Result.Failed);
                var error = failingStep == null
                    ? null
                    : string.Format("Step: {0} failed with exception: {1}", failingStep.Title, CreateExceptionMessage(failingStep));

                addRow(scenario.Example.Values.Select(e => e.GetValueAsString()), scenario.Result.ToString(), error);
            }

            foreach (var row in rows)
                WriteExampleRow(row, maxWidth);
        }

        private void WriteExampleRow(string[] row, int[] maxWidth)
        {
            for (int index = 0; index < row.Length; index++)
            {
                var col = row[index];
                Write("| {0} ", (col ?? string.Empty).Trim().PadRight(maxWidth[index]));
            }
            WriteLine("|");
        }

        private void ReportStoryHeader(Story story)
        {
            if (story.Metadata == null || story.Metadata.Type == null)
                return;

            WriteLine(story.Metadata.TitlePrefix + story.Metadata.Title);
            if (!string.IsNullOrEmpty(story.Metadata.Narrative1))
                WriteLine("\t" + story.Metadata.Narrative1);
            if (!string.IsNullOrEmpty(story.Metadata.Narrative2))
                WriteLine("\t" + story.Metadata.Narrative2);
            if (!string.IsNullOrEmpty(story.Metadata.Narrative3))
                WriteLine("\t" + story.Metadata.Narrative3);
        }

        static string PrefixWithSpaceIfRequired(string stepTitle, ExecutionOrder executionOrder)
        {
            if (executionOrder == ExecutionOrder.ConsecutiveAssertion ||
                executionOrder == ExecutionOrder.ConsecutiveSetupState ||
                executionOrder == ExecutionOrder.ConsecutiveTransition)
                stepTitle = "  " + stepTitle; // add two spaces in the front for indentation.

            return stepTitle;
        }

        void ReportOnStep(Scenario scenario, Tuple<Step, string[]> stepAndLines, bool includeResults)
        {
            if (!includeResults)
            {
                foreach (var line in stepAndLines.Item2)
                {
                    WriteLine("\t{0}", line);
                }
                return;
            }

            var step = stepAndLines.Item1;
            var humanizedResult = Configurator.Humanizer.Humanize(step.Result.ToString());

            string message;
            if (scenario.Result == Result.Passed)
                message = string.Format("\t{0}", stepAndLines.Item2[0]);
            else
            {
                var paddedFirstLine = stepAndLines.Item2[0].PadRight(_longestStepSentence + 5);
                message = string.Format("\t{0}  [{1}] ", paddedFirstLine, humanizedResult);
            }

            if (stepAndLines.Item2.Length > 1)
            {
                message = string.Format("{0}\r\n{1}", message, string.Join("\r\n", stepAndLines.Item2.Skip(1)));
            }

            if (step.Exception != null)
                message += CreateExceptionMessage(step);

            if (step.Result == Result.Inconclusive || step.Result == Result.NotImplemented)
                ForegroundColor = ConsoleColor.Yellow;
            else if (step.Result == Result.Failed)
                ForegroundColor = ConsoleColor.Red;
            else if (step.Result == Result.NotExecuted)
                ForegroundColor = ConsoleColor.Gray;

            WriteLine(message);
            ForegroundColor = ConsoleColor.White;
        }

        private string CreateExceptionMessage(Step step)
        {
            _exceptions.Add(step.Exception);

            var exceptionReference = string.Format("[Details at {0} below]", _exceptions.Count);
            if (!string.IsNullOrEmpty(step.Exception.Message))
                return string.Format("[{0}] {1}", FlattenExceptionMessage(step.Exception.Message), exceptionReference);

            return string.Format("{0}", exceptionReference);
        }

        void ReportExceptions()
        {
            WriteLine();
            if (_exceptions.Count == 0)
                return;

            Write("Exceptions:");

            for (int index = 0; index < _exceptions.Count; index++)
            {
                var exception = _exceptions[index];
                WriteLine();
                Write(string.Format("  {0}. ", index + 1));

                if (!string.IsNullOrEmpty(exception.Message))
                    WriteLine(FlattenExceptionMessage(exception.Message));
                else
                    WriteLine();

                WriteLine(exception.StackTrace);
            }

            WriteLine();
        }

        static string FlattenExceptionMessage(string message)
        {
            return string.Join(" ", message
                .Replace("\t", " ") // replace tab with one space
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)
                .Select(s => s.Trim()))
                .TrimEnd(','); // chop any , from the end
        }

        void Report(Scenario scenario)
        {
            ForegroundColor = ConsoleColor.White;
            WriteLine();
            WriteLine("Scenario: " + scenario.Title);
        }

        public virtual ConsoleColor ForegroundColor { get; set; }

        public ProcessType ProcessType
        {
            get { return ProcessType.Report; }
        }

        public override string ToString()
        {
            return _text.ToString();
        }

        protected virtual void WriteLine(string text = null)
        {
            _text.AppendLine(text);
        }

        protected virtual void WriteLine(string text, params object[] args)
        {
            _text.AppendLine(string.Format(text, args));
        }

        protected virtual void Write(string text, params object[] args)
        {
            _text.AppendFormat(text, args);
        }
    }
}