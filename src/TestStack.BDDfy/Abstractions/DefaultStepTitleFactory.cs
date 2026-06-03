using System.Reflection;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy.Abstractions;

internal class DefaultStepTitleFactory : IStepTitleFactory
{
    public bool IncludeInputsInStepTitle { get; set; } = true;
    public bool AddGherkinPrefixToSecondarySteps { get; set; } = true;

    public StepTitle Create(
        string? stepTextTemplate,
        bool? includeInputsInStepTitle,
        MethodInfo methodInfo,
        StepArgument[] inputArguments,
        ITestContext testContext,
        string stepPrefix)
    {
        string createTitle()
        {
            var flatInputArray = inputArguments.Select(o => o.Value!).FlattenArrays();
            var titleAttribute = methodInfo.GetCustomAttribute<StepTitleAttribute>(true);
            var executableAttribute = methodInfo.GetCustomAttribute<ExecutableAttribute>(true);

            var callerSuppliedTemplate = stepTextTemplate != null;
            includeInputsInStepTitle ??= titleAttribute?.IncludeInputsInStepTitle;
            stepTextTemplate ??= titleAttribute != null
                ? (NullIfEmpty(titleAttribute.StepTitle) ?? "")
                : NullIfEmpty(executableAttribute?.StepTitle);
            var stepTextTemplateWasNotSupplied = string.IsNullOrWhiteSpace(stepTextTemplate);

            stepTextTemplate ??= methodInfo.Name;

            var formattedStepTitle = string.Format(Configurator.CultureInfo, stepTextTemplate, flatInputArray);
            var stepTitle = stepTextTemplateWasNotSupplied ? Configurator.Humanizer.Humanize(formattedStepTitle) : formattedStepTitle;

            var shouldAddPrefix = stepTextTemplateWasNotSupplied
                || IsPrimaryPrefix(stepPrefix)
                || (!callerSuppliedTemplate && AddGherkinPrefixToSecondarySteps);

            if (shouldAddPrefix)
                stepTitle = AppendPrefix(stepTitle, stepPrefix);

            if (stepTextTemplate != formattedStepTitle && titleAttribute?.IncludeInputsInStepTitle is null)
                includeInputsInStepTitle??= false;

            if (stepTitle!.Contains('<') && stepTitle.Contains('>') && titleAttribute?.IncludeInputsInStepTitle is null)
                includeInputsInStepTitle??=false;

            if (includeInputsInStepTitle ?? IncludeInputsInStepTitle)
            {
                var parameters = methodInfo.GetParameters();
                var stringFlatInputs =
                    inputArguments
                        .Select((a, i) => new { ParameterName = parameters[i].Name, Value = a })
                        .Select(i =>
                        {
                            if (testContext.Examples != null)
                            {
                                var matchingHeaders = testContext.Examples.Headers
                                    .Where(header => ExampleTable.HeaderMatches(header, i.ParameterName) || ExampleTable.HeaderMatches(header, i.Value.Name))
                                    .ToList();

                                if (matchingHeaders.Count > 1)
                                    throw new AmbiguousMatchException($"More than one headers for examples, match the parameter '{i.ParameterName}' provided for '{methodInfo.Name}'");

                                var matchingHeader = matchingHeaders.SingleOrDefault();
                                if (matchingHeader != null)
                                    return string.Format("<{0}>", matchingHeader);
                            }
                            return i.Value.Value?.FlattenArray() ?? Array.Empty<string>();
                        })
                        .ToArray();

                stepTitle = stepTitle + " " + string.Join(", ", stringFlatInputs.Select(o => o.ToTextRepresentation()));
            }

            return stepTitle.Trim();
        }

        return new StepTitle(createTitle);
    }

    public StepTitle Create(string title, string stepPrefix, ITestContext testContext) => new(AppendPrefix(title, stepPrefix));

    private static string AppendPrefix(string? title, string stepPrefix)
    {
        var stepTitle = (title ?? string.Empty).Trim();

        if (!stepTitle.StartsWith(stepPrefix, ignoreCase: true, Configurator.CultureInfo))
        {
            if (stepTitle.Length == 0) return string.Format(Configurator.CultureInfo, "{0} ", stepPrefix);

            return string.Format(Configurator.CultureInfo, "{0} {1}{2}", stepPrefix, stepTitle[..1].ToLower(Configurator.CultureInfo), stepTitle[1..]);
        }

        return stepTitle;
    }

    private static string? NullIfEmpty(string? value) => string.IsNullOrWhiteSpace(value) ? null : value;

    private static bool IsPrimaryPrefix(string prefix) =>
        prefix is "Given" or "When" or "Then";
}