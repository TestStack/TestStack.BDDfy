using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy.Reporters
{
    public class TextReporter : IProcessor
    {
        private readonly List<Exception> _exceptions = [];
        private readonly StringBuilder _text = new();
        private int _longestStepSentence;

        public void Process(Story story)
        {
            WriteStoryTitle(story);

            var allSteps = story.Scenarios.SelectMany(s => s.Steps)
                .Select(GetStepWithLines)
                .ToList();

            if (allSteps.Count != 0)
                _longestStepSentence = allSteps.SelectMany(s => s.Item2.Select(l => l.Length)).Max();

            foreach (var scenarioGroup in story.Scenarios.GroupBy(s => s.Id))
            {
                var firstScenario = scenarioGroup.First();

                if (scenarioGroup.Count() > 1)
                {
                    // all scenarios in an example based scenario share the same header and narrative
                    WriteScenario(firstScenario, false);

                    if(firstScenario.Example != null)
                    {
                        WriteLine();
                        WriteExamples(firstScenario, scenarioGroup);
                    }
                }
                else
                {
                    foreach (var scenario in story.Scenarios)
                    {
                        WriteScenario(scenario, true);
                    }
                }
                
                WriteTags(firstScenario.Tags);
            }

            ReportExceptions();
        }

        private void WriteScenario(Scenario scenario, bool writeResults)
        {
            WriteScenarioTitle(scenario);

            foreach (var step in scenario.Steps.Where(s => s.ShouldReport))
                WriteScenarioStep(scenario, GetStepWithLines(step), writeResults);
        }

        private static Tuple<Step, string[]> GetStepWithLines(Step s) => Tuple.Create(s, s.Title.Replace("\r\n", "\n").Split('\n').Select(l => PrefixWithSpaceIfRequired(l, s.ExecutionOrder)).ToArray());

        private void WriteTags(List<string> tags)
        {
            if (tags.Count == 0)
                return;

            WriteLine();
            WriteLine("Tags: {0}", string.Join(", ", tags));
        }

        private void WriteExamples(Scenario exampleScenario, IEnumerable<Scenario> scenarioGroup)
        {
            if (exampleScenario.Example is null) return;

            WriteLine("Examples: ");
            var scenarios = scenarioGroup.ToArray();
            var allPassed = scenarios.All(s => s.Result == Result.Passed);
            var exampleColumns = exampleScenario.Example.Headers.Length;
            var numberColumns = allPassed ? exampleColumns : exampleColumns + 2;
            var maxWidth = new int[numberColumns];
            var rows = new List<string[]>();

            void addRow(IEnumerable<string> cells, string result, string? error)
            {
                var row = new string[numberColumns];
                var index = 0;

                foreach (var cellText in cells)
                    row[index++] = cellText;

                if (!allPassed)
                {
                    row[numberColumns - 2] = result;
                    row[numberColumns - 1] = error!;
                }

                for (var i = 0; i < numberColumns; i++)
                {
                    var rowValue = row[i];
                    if (rowValue != null && rowValue.Length > maxWidth[i])
                        maxWidth[i] = rowValue.Length;
                }

                rows.Add(row);
            }

            addRow(exampleScenario.Example.Headers, "Result", "Errors");
            foreach (var scenario in scenarios)
            {
                var failingStep = scenario.Steps.FirstOrDefault(s => s.Result == Result.Failed);
                var error = failingStep == null
                    ? null
                    : string.Format("Step: {0} failed with exception: {1}", failingStep.Title, CreateExceptionMessage(failingStep));

                addRow(scenario.Example!.Values.Select(e => e.GetValueAsString()), scenario.Result.ToString(), error);
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

        private void WriteStoryTitle(Story story)
        {
            if (story?.Metadata?.Type is null) return; ;

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

        void WriteScenarioStep(Scenario scenario, Tuple<Step, string[]> stepAndLines, bool includeResults)
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
            if(step.Exception is not null) _exceptions.Add(step.Exception);

            var exceptionReference = string.Format("[Details at {0} below]", _exceptions.Count);
            if (!string.IsNullOrEmpty(step.Exception?.Message))
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

                if (exception.StackTrace is null) continue;
                WriteLine(exception.StackTrace);
            }

            WriteLine();
        }

        static string FlattenExceptionMessage(string message) => string.Join(" ", message
                .Replace("\t", " ") // replace tab with one space
                .Split(["\r\n", "\n"], StringSplitOptions.None)
                .Select(s => s.Trim()))
                .TrimEnd(','); // chop any , from the end

        void WriteScenarioTitle(Scenario scenario)
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

        public override string ToString() => _text.ToString();

        protected virtual void WriteLine(string? text = null) => _text.AppendLine(text);

        protected virtual void WriteLine(string text, params object[] args) => _text.AppendLine(string.Format(text, args));

        protected virtual void Write(string text, params object[] args) => _text.AppendFormat(text, args);
    }
}