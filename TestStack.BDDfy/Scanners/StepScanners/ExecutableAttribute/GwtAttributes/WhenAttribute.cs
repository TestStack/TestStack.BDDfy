﻿namespace TestStack.BDDfy.Scanners.StepScanners.ExecutableAttribute.GwtAttributes
{
    public class WhenAttribute : ExecutableAttribute
    {
        public WhenAttribute() : this(null) { }
        public WhenAttribute(string stepTitle) : base(Core.ExecutionOrder.Transition, stepTitle) { }
    }
}