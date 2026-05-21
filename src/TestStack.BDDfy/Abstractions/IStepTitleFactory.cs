using System.Reflection;

namespace TestStack.BDDfy.Abstractions;

public interface IStepTitleFactory
{
    public StepTitle Create(
        string stepTextTemplate,
        bool includeInputsInStepTitle,
        MethodInfo methodInfo,
        StepArgument[] inputArguments,
        ITestContext testContext,
        string stepPrefix);

    StepTitle Create(string title, string stepPrefix, ITestContext testContext);
}