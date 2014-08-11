using System;
using System.Linq;
using System.Reflection;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy.Processors
{
    public class ScenarioExecutor
    {
        private readonly Scenario _scenario;

        public ScenarioExecutor(Scenario scenario)
        {
            _scenario = scenario;
        }

        public void InitializeScenario()
        {
            if (_scenario.Example == null) 
                return;

            var type = _scenario.TestObject.GetType();
            var memberInfos = type
                .GetMembers(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
                .Where(m => m is FieldInfo || m is PropertyInfo)
                .Where(m => !m.Name.EndsWith("BackingField"))
                .ToArray();

            var possibleTargets = memberInfos
                .OfType<FieldInfo>()
                .Select(f => new StepArgument(f, _scenario.TestObject))
                .Union(memberInfos.OfType<PropertyInfo>().Select(m => new StepArgument(m, _scenario.TestObject)))
                .Union(_scenario.Steps.SelectMany(s=>s.Arguments))
                .ToArray();

            foreach (var cell in _scenario.Example.Values)
            {
                var matchingMembers = possibleTargets
                    .Where(n => cell.MatchesName(n.Name))
                    .ToArray();

                if (!matchingMembers.Any())
                    continue;

                foreach (var matchingMember in matchingMembers)
                {
                    matchingMember.SetValue(cell.GetValue(matchingMember.ArgumentType));
                }
            }
        }

        public Result ExecuteStep(Step step)
        {
            try
            {
                AsyncTestRunner.Run(() => Configurator.StepExecutor.Execute(step, _scenario.TestObject));
                step.Result = Result.Passed;
            }
            catch (Exception ex)
            {
                // ToDo: more thought should be put into this. Is it safe to get the exception?
                var exception = ex;
                if (exception is TargetInvocationException)
                {
                    exception = ex.InnerException ?? ex;
                }

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
