using System;

namespace TestStack.BDDfy
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class StepTitleAttribute : Attribute
    {
        public StepTitleAttribute(string stepTitle)
        {
            StepTitle = stepTitle;
        }
        
        public StepTitleAttribute(string stepTitle, bool includeInputsInStepTitle)
        {
            IncludeInputsInStepTitle = includeInputsInStepTitle;
            StepTitle = stepTitle;
        }

        public string StepTitle { get; private set; }

        public bool? IncludeInputsInStepTitle { get; private set; }
    }
}