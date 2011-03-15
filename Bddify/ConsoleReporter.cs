using System;
using System.Collections.Generic;
using System.Linq;

namespace Bddify
{
    public class ConsoleReporter : IProcessor
    {
        public static readonly Action<string> DefaultPrintOutput = Console.WriteLine;
        public static Action<string> PrintOutput = DefaultPrintOutput;
        readonly List<Exception> _exceptions = new List<Exception>();
        private int _longestStepSentence;

        public ProcessType ProcessType
        {
            get { return ProcessType.Report; }
        }

        public void Process(Bddee bddee)
        {
            var reporterRegistry
                = new Dictionary<StepExecutionResult, Action<ExecutionStep>>
                          {
                              {StepExecutionResult.Succeeded, s => ReportOnStep(s)},
                              {StepExecutionResult.Failed, s => ReportOnStep(s, true)},
                              {StepExecutionResult.Inconclusive, s => ReportOnStep(s)},
                              {StepExecutionResult.NotImplemented, s => ReportOnStep(s, true)},
                              {StepExecutionResult.NotExecuted, s => ReportOnStep(s)}
                          };

            _longestStepSentence = bddee.Steps.Max(s => s.ReadableMethodName.Length);

            Report(bddee);
            
            foreach (var step in bddee.Steps)
                reporterRegistry[step.Result](step);

            ReportExceptions();
        }

        void ReportOnStep(ExecutionStep step, bool reportOnException = false)
        {
            var message =
                string.Format
                    ("{0}  [{1}]",
                    step.ReadableMethodName.PadRight(_longestStepSentence + 5),
                    NetToString.CreateSentenceFromCamelName(step.Result.ToString()));

            if(reportOnException)
            {
                _exceptions.Add(step.Exception);
                message += string.Format(" :: Exception Stack Trace below on number [{0}]", _exceptions.Count);
            }

            PrintOutput(message);
        }

        void ReportExceptions()
        {
            if (_exceptions.Count == 0)
                return;

            PrintOutput(string.Empty);
            PrintOutput("Exceptions' Details: ");
            PrintOutput(string.Empty);

            for (int index = 0; index < _exceptions.Count; index++)
            {
                var exception = _exceptions[index];
                PrintOutput(string.Format("[{0}]:", index + 1));
                
                if (string.IsNullOrEmpty(exception.Message))
                    PrintOutput(exception.Message);
         
                PrintOutput(exception.StackTrace);
                PrintOutput("============================================================================================================");
            }
        }

        static void Report(Bddee bddee)
        {
            PrintOutput("Scenario: " +  bddee.ScenarioSentence + Environment.NewLine);
        }
    }
}