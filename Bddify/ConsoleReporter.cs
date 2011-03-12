using System;

namespace Bddify
{
    public class ConsoleReporter : IProcessor
    {
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
            Bddifier.PrintOutput(step.ReadableMethodName + "  [Failed] ");
            Bddifier.PrintOutput("====================================");
            Bddifier.PrintOutput(step.Exception.Message);
            Bddifier.PrintOutput(step.Exception.StackTrace);
            Bddifier.PrintOutput("======== End of stack trace ========");
        }

        static void ReportSuccess(ExecutionStep step)
        {
            Bddifier.PrintOutput(step.ReadableMethodName);
        }

        static void ReportNotImplemented(ExecutionStep step)
        {
            var message = step.ReadableMethodName + "  [Not Implemented] ";
            if(!string.IsNullOrEmpty(step.Exception.Message))
                message += " : " + step.Exception.Message;

            Bddifier.PrintOutput(message);
        }

        static void Report(object objectUnderTest)
        {
            Bddifier.PrintOutput("Scenario: " + Bddifier.CreateSentenceFromTypeName(objectUnderTest.GetType().Name) + Environment.NewLine);
        }
    }
}