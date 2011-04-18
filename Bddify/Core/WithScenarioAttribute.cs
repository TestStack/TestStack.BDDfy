using System;

namespace Bddify.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class WithScenarioAttribute : Attribute
    {
        public Type ScenarioType { get; private set; }

        public WithScenarioAttribute(Type scenarioType)
        {
            ScenarioType = scenarioType;
        }
    }
}