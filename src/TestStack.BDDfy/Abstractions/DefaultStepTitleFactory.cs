using System;
using System.Linq;
using System.Reflection;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy.Abstractions;

internal class DefaultStepTitleFactory : IStepTitleFactory
{
    public bool IncludeInputsInStepTitle { get; set; } = true;

    public StepTitle Create(
        string stepTextTemplate,
        bool? includeInputsInStepTitle,
        MethodInfo methodInfo,
        StepArgument[] inputArguments,
        ITestContext testContext,
        string stepPrefix)
    {
        string createTitle()
        {
            var flatInputArray = inputArguments.Select(o => o.Value).FlattenArrays();
            var name = methodInfo.Name;
            var titleAttribute = methodInfo.GetCustomAttribute<StepTitleAttribute>(true);
            var executableAttribute = methodInfo.GetCustomAttribute<ExecutableAttribute>(true);

            includeInputsInStepTitle ??= titleAttribute?.IncludeInputsInStepTitle ?? IncludeInputsInStepTitle;

            var titleTemplate = titleAttribute?.StepTitle ?? executableAttribute?.StepTitle;

            if (titleTemplate is not null)
            {
                name = string.Format(titleTemplate, flatInputArray);
            }

            var stepTitle = AppendPrefix(Configurator.Humanizer.Humanize(name), stepPrefix);

            if (!string.IsNullOrEmpty(stepTextTemplate)) stepTitle = string.Format(stepTextTemplate, flatInputArray);
            else if (includeInputsInStepTitle.Value)
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
                                    .Where(header => ExampleTable.HeaderMatches(header, i.ParameterName) ||
                                                    ExampleTable.HeaderMatches(header, i.Value.Name))
                                    .ToList();

                                if (matchingHeaders.Count > 1)
                                    throw new AmbiguousMatchException($"More than one headers for examples, match the parameter '{i.ParameterName}' provided for '{methodInfo.Name}'");

                                var matchingHeader = matchingHeaders.SingleOrDefault();
                                if (matchingHeader != null)
                                    return string.Format("<{0}>", matchingHeader);
                            }
                            return i.Value.Value.FlattenArray();
                        })
                        .ToArray();

                stepTitle = stepTitle + " " + string.Join(", ", stringFlatInputs);
            }

            return stepTitle.Trim();
        }

        return new StepTitle(createTitle);
    }

    public StepTitle Create(string title, string stepPrefix, ITestContext testContext) => new(AppendPrefix(title, stepPrefix));

    private static string AppendPrefix(string title, string stepPrefix)
    {
        if (!title.StartsWith(stepPrefix, StringComparison.CurrentCultureIgnoreCase))
        {
            if (title.Length == 0) return string.Format("{0} ", stepPrefix);
            return string.Format("{0} {1}{2}", stepPrefix, title[..1].ToLower(), title[1..]);
        }

        return title;
    }
}