using System.Reflection;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy.Processors
{
    public class ScenarioExecutor(Scenario scenario)
    {
        private readonly Scenario _scenario = scenario;

        public void InitializeScenario()
        {
            if (_scenario.Example == null) 
                return;

            var type = _scenario.TestObject.GetType();
            var members = type
                .GetMembers(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
                .Where(m => m is FieldInfo || m is PropertyInfo)
                .Where(m => !m.Name.EndsWith("BackingField"));

            var fieldTargets = members.OfType<FieldInfo>()
                .Select(f => new StepArgument(f.Name, f.FieldType, () => f.GetValue(_scenario.TestObject), o => f.SetValue(_scenario.TestObject, o)));

            var propertyTargets = members.OfType<PropertyInfo>()
                .Select(p => new StepArgument(p.Name, p.PropertyType, () => p.GetValue(_scenario.TestObject, null), o => p.SetValue(_scenario.TestObject, o, null)));

            var memberTargets = fieldTargets.Union(propertyTargets).ToArray();
            var stepArgTargets = _scenario.Steps.SelectMany(s => s.Arguments).ToArray();

            foreach (var cell in _scenario.Example.Values)
            {
                var targets = ResolveTargets(cell, stepArgTargets, memberTargets);

                foreach (var target in targets)
                    target.SetValue(cell.GetValue(target.ArgumentType));
            }
        }

        private static StepArgument[] ResolveTargets(ExampleValue cell, StepArgument[] stepArgs, StepArgument[] members)
        {
            var matchingStepArgs = stepArgs.Where(a => cell.MatchesName(a.Name)).ToArray();
            var matchingMembers = members.Where(m => cell.MatchesName(m.Name)).ToArray();

            if (matchingStepArgs.Length == 0)
                return matchingMembers;

            // When step arguments match, also include compatible members (e.g. backing fields
            // read directly in the step body) but skip incompatible ones to avoid type errors
            // like parsing a string example value into an unrelated enum field.
            var compatibleMembers = matchingMembers.Where(m => cell.IsCompatibleWith(m.ArgumentType));
            return [.. matchingStepArgs.Union(compatibleMembers)];
        }

        public Result ExecuteStep(Step step)
        {
            try
            {
                if (Configurator.AsyncVoidSupportEnabled)
                    AsyncTestRunner.Run(() => Configurator.StepExecutor.Execute(step, _scenario.TestObject));
                else
                    Configurator.StepExecutor.Execute(step, _scenario.TestObject);
                step.Result = Result.Passed;
            }
            catch (Exception ex)
            {
                // ToDo: more thought should be put into this. Is it safe to get the exception?
                var exception = ExceptionResolver.Resolve(ex);

                if (exception is NotImplementedException)
                {
                    step.Result = Result.NotImplemented;
                    step.Exception = exception;
                }
                else if (IsInconclusive(exception))
                {
                    step.Result = Result.Inconclusive;
                    step.Exception = exception;
                }
                else
                {
                    step.Exception = exception;
                    step.Result = Result.Failed;
                }
            }

            return step.Result;
        }

        private static bool IsInconclusive(Exception exception)
        {
            return exception.GetType().Name.Contains("InconclusiveException");
        }
    }
}
