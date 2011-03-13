using System;

namespace Bddify
{
    public class ConsoleReporter : IProcessor
    {
        public static readonly Action<string> DefaultPrintOutput = Console.WriteLine;
        public static Action<string> PrintOutput = DefaultPrintOutput;

        public ProcessType ProcessType
        {
            get { return ProcessType.Report; }
        }

        public void Process(Bddee bddee)
        {
            Report(bddee.Object);
            foreach (var step in bddee.Steps)
                Report(step);
        }

        static void Report(ExecutionStep step)
        {
            switch (step.Result)
            {
                case StepExecutionResult.Succeeded:
                    ReportSuccess(step);
                    break;

                case StepExecutionResult.Failed:
                    ReportFailed(step);
                    break;

                case StepExecutionResult.Inconclusive:
                    ReportNotImplemented(step);
                    break;
            }
        }

        static void ReportFailed(ExecutionStep step)
        {
            PrintOutput(step.ReadableMethodName + "  [Failed] ");
            PrintOutput("====================================");
            PrintOutput(step.Exception.Message);
            PrintOutput(step.Exception.StackTrace);
            PrintOutput("======== End of stack trace ========");
        }

        static void ReportSuccess(ExecutionStep step)
        {
            PrintOutput(step.ReadableMethodName);
        }

        static void ReportNotImplemented(ExecutionStep step)
        {
            var message = step.ReadableMethodName + "  [Not Implemented] ";
            if(!string.IsNullOrEmpty(step.Exception.Message))
                message += " : " + step.Exception.Message;

            PrintOutput(message);
        }

        static void Report(object objectUnderTest)
        {
            PrintOutput("Scenario: " + NetToString.CreateSentenceFromTypeName(objectUnderTest.GetType().Name) + Environment.NewLine);
        }
    }
}