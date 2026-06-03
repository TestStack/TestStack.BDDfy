namespace TestStack.BDDfy
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    
    public class StepTitleAttribute(string stepTitle): Attribute
    {
        public StepTitleAttribute(string stepTitle, bool includeInputsInStepTitle): this(stepTitle)
        {
            IncludeInputsInStepTitle = includeInputsInStepTitle;
        }

        public string StepTitle { get; private set; } = stepTitle;

        public bool? IncludeInputsInStepTitle { get; private set; }
    }
}