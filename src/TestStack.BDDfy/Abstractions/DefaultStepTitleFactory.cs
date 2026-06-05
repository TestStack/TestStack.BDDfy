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

            var callerSuppliedTemplate = stepTextTemplate is not null;
            var effectiveIncludeInputs = includeInputsInStepTitle ?? titleAttribute?.IncludeInputsInStepTitle;

            var template = stepTextTemplate ?? (titleAttribute is not null
                ? (NullIfEmpty(titleAttribute.StepTitle) ?? string.Empty)
                : NullIfEmpty(executableAttribute?.StepTitle));

            var hasTemplate = !string.IsNullOrWhiteSpace(template);
            template ??= methodInfo.Name;

            var formattedTitle = string.Format(Configurator.CultureInfo, template, flatInputArray);
            var stepTitle = (hasTemplate ? formattedTitle : Configurator.Humanizer.Humanize(formattedTitle)) 
                ?? throw new InvalidOperationException($"Failed to create a step title for method '{methodInfo.Name}'.");

            if (ShouldAddPrefix(hasTemplate, callerSuppliedTemplate, stepPrefix))
                stepTitle = PrependPrefix(stepTitle, stepPrefix);

            effectiveIncludeInputs ??= ShouldIncludeInputs(stepTitle, template, formattedTitle, titleAttribute, methodInfo);

            if (effectiveIncludeInputs ?? IncludeInputsInStepTitle)
                stepTitle = AppendInputSuffix(stepTitle, methodInfo, inputArguments, testContext);

            return stepTitle.Trim();
        }

        return new StepTitle(createTitle);
    }

    public StepTitle Create(string title, string stepPrefix, ITestContext testContext) =>
        new(PrependPrefix(title, stepPrefix));

    private bool ShouldAddPrefix(bool hasTemplate, bool callerSuppliedTemplate, string stepPrefix) =>
        !hasTemplate
        || IsPrimaryPrefix(stepPrefix)
        || (!callerSuppliedTemplate && AddGherkinPrefixToSecondarySteps);

    private static bool? ShouldIncludeInputs(
        string stepTitle,
        string stepTextTemplate,
        string formattedTitle,
        StepTitleAttribute? titleAttribute,
        MethodInfo methodInfo)
    {
        if (titleAttribute?.IncludeInputsInStepTitle is not null)
            return null;

        if (stepTextTemplate != formattedTitle)
            return false;

        if (stepTitle.Contains('<') && stepTitle.Contains('>'))
            return methodInfo.GetCustomAttributes<RunStepWithArgsAttribute>(true).Any();

        return null;
    }

    private static string AppendInputSuffix(
        string stepTitle,
        MethodInfo methodInfo,
        StepArgument[] inputArguments,
        ITestContext testContext)
    {
        var parameters = methodInfo.GetParameters();

        var inputRepresentations = inputArguments
            .Select((arg, i) => FormatInput(arg, parameters[i].Name, methodInfo.Name, testContext))
            .Where(x => IsNotAlreadyInTitle(stepTitle, x))
            .ToArray();

        if (inputRepresentations.Length == 0)
            return stepTitle;

        return stepTitle.Trim() + " " + string.Join(", ", inputRepresentations.Select(o => o.ToTextRepresentation()));
    }

    private static object FormatInput(StepArgument arg, string? parameterName, string methodName, ITestContext testContext)
    {
        if (testContext.Examples is null)
            return arg.Value?.FlattenArray() ?? Array.Empty<string>();

        var matchingHeader = FindMatchingExampleHeader(testContext.Examples, parameterName, arg.Name, methodName);
        return matchingHeader is not null
            ? $"<{matchingHeader}>"
            : arg.Value?.FlattenArray() ?? Array.Empty<string>();
    }

    private static string? FindMatchingExampleHeader(ExampleTable examples, string? parameterName, string? argName, string methodName)
    {
        var matchingHeaders = examples.Headers
            .Where(header => ExampleTable.HeaderMatches(header, parameterName) || ExampleTable.HeaderMatches(header, argName))
            .ToList();

        if (matchingHeaders.Count > 1)
            throw new AmbiguousMatchException(
                $"More than one headers for examples, match the parameter '{parameterName}' provided for '{methodName}'");

        return matchingHeaders.SingleOrDefault();
    }

    private static bool IsNotAlreadyInTitle(string stepTitle, object arg)
    {
        if (arg is not string token)
            return true;

        if (!token.StartsWith('<') || !token.EndsWith('>'))
            return true;

        return !stepTitle.Replace(" ", "").Contains(token, StringComparison.OrdinalIgnoreCase);
    }

    private static string PrependPrefix(string? title, string stepPrefix)
    {
        var stepTitle = (title ?? string.Empty).Trim();

        if (stepTitle.StartsWith(stepPrefix, ignoreCase: true, Configurator.CultureInfo))
            return stepTitle;

        if (stepTitle.Length == 0)
            return $"{stepPrefix} ";

        return $"{stepPrefix} {stepTitle[..1].ToLower(Configurator.CultureInfo)}{stepTitle[1..]}";
    }

    private static string? NullIfEmpty(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value;

    private static bool IsPrimaryPrefix(string prefix) =>
        prefix is "Given" or "When" or "Then";
}
