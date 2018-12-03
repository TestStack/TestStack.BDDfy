using System;

namespace TestStack.BDDfy
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class StepTitleAttribute : Attribute
    {        
        public StepTitleAttribute(string stepTitle, bool includeInputsInStepTitle = false, bool autoFormatStepText = false)
        {
            IncludeInputsInStepTitle = includeInputsInStepTitle;
            StepTitle = stepTitle;
            AutoFormatStepText = autoFormatStepText;
        }

        public string StepTitle { get; private set; }

        public bool? IncludeInputsInStepTitle { get; private set; }

        public bool AutoFormatStepText { get; }
    }
}