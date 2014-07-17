using System;

namespace TestStack.BDDfy
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class StepTitleAttribute : Attribute
    {
        public StepTitleAttribute(string stepTitle)
        {
            this.StepTitle = stepTitle;
        }

        public string StepTitle { get; private set; }
    }
}