using System.Reflection;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy
{
    public class ReflectiveScenarioScanner(string? scenarioTitle, params IStepScanner[] stepScanners): IScenarioScanner
    {
        public ReflectiveScenarioScanner(params IStepScanner[] stepScanners) : this(null, stepScanners) { }

        public virtual IEnumerable<Scenario> Scan(ITestContext testContext)
        {
            Type scenarioType;

            if (testContext.Examples == null)
            {
                var steps = ScanScenarioForSteps(testContext);
                scenarioType = testContext.TestObject.GetType();
                scenarioTitle ??= GetScenarioText(scenarioType);

                var orderedSteps = steps.OrderBy(o => o.ExecutionOrder).ThenBy(o => o.ExecutionSubOrder).ToList();
                PromoteConsecutiveSteps(orderedSteps);
                yield return new Scenario(testContext.TestObject, orderedSteps, scenarioTitle, testContext.Tags);
                yield break;
            }

            scenarioType = testContext.TestObject.GetType();
            scenarioTitle ??= GetScenarioText(scenarioType);

            var scenarioId = Configurator.IdGenerator.GetScenarioId();

            foreach (var example in testContext.Examples)
            {
                var steps = ScanScenarioForSteps(testContext, example);
                var orderedSteps = steps.OrderBy(o => o.ExecutionOrder).ThenBy(o => o.ExecutionSubOrder).ToList();
                PromoteConsecutiveSteps(orderedSteps);
                yield return new Scenario(scenarioId, testContext.TestObject, orderedSteps, scenarioTitle, example, testContext.Tags);
            }
        }

        static string? GetScenarioText(Type scenarioType) => Configurator.Humanizer.Humanize(scenarioType.Name);

        protected virtual IEnumerable<Step> ScanScenarioForSteps(ITestContext testContext)
        {
            var allSteps = new List<Step>();
            var scenarioType = testContext.TestObject.GetType();

            foreach (var methodInfo in GetMethodsOfInterest(scenarioType))
            {
                // chain of responsibility of step scanners
                foreach (var scanner in stepScanners)
                {
                    var steps = scanner.Scan(testContext, methodInfo);
                    if (steps.Any())
                    {
                        allSteps.AddRange(steps);
                        break;
                    }
                }
            }

            return allSteps;
        }

        protected virtual IEnumerable<Step> ScanScenarioForSteps(ITestContext testContext, Example example)
        {
            var allSteps = new List<Step>();
            var scenarioType = testContext.TestObject.GetType();

            foreach (var methodInfo in GetMethodsOfInterest(scenarioType))
            {
                // chain of responsibility of step scanners
                foreach (var scanner in stepScanners)
                {
                    var steps = scanner.Scan(testContext, methodInfo, example);
                    if (steps.Any())
                    {
                        allSteps.AddRange(steps);
                        break;
                    }
                }
            }

            return allSteps;
        }

        public virtual IEnumerable<MethodInfo> GetMethodsOfInterest(Type scenarioType)
        {
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            var properties = scenarioType.GetProperties(bindingFlags);
            var getMethods = properties.Select(p => p.GetGetMethod(true));
            var setMethods = properties.Select(p => p.GetSetMethod(true));
            var allPropertyMethods = getMethods.Union(setMethods);

            return [.. scenarioType
                .GetMethods(bindingFlags)
                .Where(m => m.GetCustomAttribute<IgnoreStepAttribute>(true) is null)
                .Except(allPropertyMethods).Where(mi=> mi is not null)
                .Select(x=>x!)];
        }

        private static void PromoteConsecutiveSteps(List<Step> orderedSteps)
        {
            ExecutionOrder? lastPrimaryOrder = null;

            foreach (var step in orderedSteps)
            {
                if (!step.AllowConsecutivePromotion)
                {
                    // Reset tracking — non-promotable steps don't participate
                    var po = GetPrimaryOrder(step.ExecutionOrder);
                    if (po != null) lastPrimaryOrder = po;
                    continue;
                }

                var primaryOrder = GetPrimaryOrder(step.ExecutionOrder);
                if (primaryOrder == null) continue;

                if (primaryOrder == lastPrimaryOrder)
                {
                    if (!IsAlreadyConsecutive(step.ExecutionOrder))
                    {
                        step.ExecutionOrder = GetConsecutiveOrder(step.ExecutionOrder);
                        ReplacePrefixWithAnd(step);
                    }
                    else
                    {
                        StripRedundantKeyword(step);
                    }
                }
                else
                {
                    lastPrimaryOrder = primaryOrder;
                }
            }
        }

        private static ExecutionOrder? GetPrimaryOrder(ExecutionOrder order) => order switch
        {
            ExecutionOrder.SetupState or ExecutionOrder.ConsecutiveSetupState => ExecutionOrder.SetupState,
            ExecutionOrder.Transition or ExecutionOrder.ConsecutiveTransition => ExecutionOrder.Transition,
            ExecutionOrder.Assertion or ExecutionOrder.ConsecutiveAssertion => ExecutionOrder.Assertion,
            _ => null
        };

        private static bool IsAlreadyConsecutive(ExecutionOrder order) =>
            order is ExecutionOrder.ConsecutiveSetupState or ExecutionOrder.ConsecutiveTransition or ExecutionOrder.ConsecutiveAssertion;

        private static ExecutionOrder GetConsecutiveOrder(ExecutionOrder order) => order switch
        {
            ExecutionOrder.SetupState => ExecutionOrder.ConsecutiveSetupState,
            ExecutionOrder.Transition => ExecutionOrder.ConsecutiveTransition,
            ExecutionOrder.Assertion => ExecutionOrder.ConsecutiveAssertion,
            _ => order
        };

        private static void ReplacePrefixWithAnd(Step step)
        {
            var title = step.Title;
            if (string.IsNullOrEmpty(title)) return;

            var addPrefix = Configurator.StepTitleFactory.AddGherkinPrefixToSecondarySteps;

            string[] prefixes = ["Given ", "When ", "Then "];
            foreach (var prefix in prefixes)
            {
                if (title.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    var remainder = title[prefix.Length..];
                    step.OverrideTitle(addPrefix ? "And " + remainder : remainder);
                    return;
                }
            }

            if (!addPrefix)
                return;

            // No known prefix found — prepend "And" with lowercased first char
            step.OverrideTitle("And " + title[..1].ToLowerInvariant() + title[1..]);
        }

        /// <summary>
        /// Strips redundant Gherkin keywords from already-consecutive step titles.
        /// E.g. "And given the pin is correct" → "And the pin is correct"
        /// </summary>
        private static void StripRedundantKeyword(Step step)
        {
            var title = step.Title;
            if (string.IsNullOrEmpty(title)) return;

            string[] redundantPrefixes = ["And given ", "And when ", "And then ", "But given ", "But when ", "But then "];
            foreach (var prefix in redundantPrefixes)
            {
                if (title.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    var conjunction = title[..prefix.IndexOf(' ')]; // "And" or "But"
                    var remainder = title[prefix.Length..];
                    step.OverrideTitle(conjunction + " " + remainder);
                    return;
                }
            }
        }
    }
}