using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy.Reporters
{
    public class TextReporter : IProcessor
    {
        private readonly List<Exception> _exceptions = new List<Exception>();
        private readonly StringBuilder _text = new StringBuilder();
        private int _longestStepSentence;

        public void Process(Story story)
        {
            ReportStoryHeader(story);

            var allSteps = story.Scenarios.SelectMany(s => s.Steps).ToList();
            if (allSteps.Any())
                _longestStepSentence = allSteps.Max(s => PrefixWithSpaceIfRequired(s).Length);

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

                    WriteLine();
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

        private void WriteExamples(Scenario exampleScenario, IEnumerable<Scenario> scenarioGroup)
        {
            WriteLine(@"Examples: ");
            var numberColumns = exampleScenario.Example.ColumnCount + 2;
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
            addRow(string.Empty, exampleScenario.Example.Headers, "Errors");
            foreach (var scenario in scenarioGroup)
            {
                var failingStep = scenario.Steps.FirstOrDefault(s => s.Result == Result.Failed);
                var error = failingStep == null ? null :
                    string.Format("Step: {0} failed with exception: {1}", failingStep.Title, CreateExceptionMessage(failingStep));
                addRow(scenario.Result.ToString(), scenario.Example.Values, error);
            }

            foreach (var row in rows)
            {
                WriteExampleRow(row, maxWidth);
            }


        }

        private void WriteExampleRow(string[] row, int[] maxWidth)
        {
            Write("|");
            for (int index   = 0; index < row.Length; index++)
            {
                var col = row[index];
                Write(" {0} |", (col??string.Empty).Trim().PadRight(maxWidth[index]));
            }
            Write("\n");
        }

        private void ReportStoryHeader(Story story)
        {
            if (story.Metadata == null || story.Metadata.Type == null)
                return;

            WriteLine("Story: " + story.Metadata.Title);
            if (!string.IsNullOrEmpty(story.Metadata.Narrative1))
                WriteLine("\t" + story.Metadata.Narrative1);
            if (!string.IsNullOrEmpty(story.Metadata.Narrative2))
                WriteLine("\t" + story.Metadata.Narrative2);
            if (!string.IsNullOrEmpty(story.Metadata.Narrative3))
                WriteLine("\t" + story.Metadata.Narrative3);
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
                WriteLine("\t{0}", PrefixWithSpaceIfRequired(step).PadRight(_longestStepSentence));
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
                message = CreateExceptionMessage(step);
            }

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
            // ToDo: if gets complex will change it with a regex
            return message
                .Replace("\t", " ") // replace tab with one space
                .Replace(Environment.NewLine, ", ") // replace new line with one space
                .Trim() // trim starting and trailing spaces
                .Replace("  ", " ")
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