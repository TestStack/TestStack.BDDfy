using System;

namespace Bddify.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RunScenarioWithArgsAttribute: Attribute
    {
        private readonly object[] _scenarioArguments;

        public RunScenarioWithArgsAttribute(params object[] scenarioArguments)
        {
            _scenarioArguments = scenarioArguments;
        }

        public object[] ScenarioArguments
        {
            get { return _scenarioArguments; }
        }
    }
}