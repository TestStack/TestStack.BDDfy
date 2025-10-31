using System;
using TestStack.BDDfy.Annotations;

namespace TestStack.BDDfy
{
    /// <summary>
    /// This attribute is marked with <see cref="TestStack.BDDfy.Annotations.MeansImplicitUseAttribute"/>
    /// so that any method decorated with <c>[Executable]</c> (or derived GWT attributes)
    /// is treated as "used implicitly" by code-analysis tools (ReSharper/Rider/etc).
    ///
    /// Reason: step methods are discovered via reflection at runtime, which can trigger
    /// "unused" inspections in IDEs; marking the attribute with MeansImplicitUse prevents
    /// those false positives and keeps IDE/test explorers tidy without requiring per-method
    /// annotations such as <c>[UsedImplicitly]</c>.
    /// </summary>
    [MeansImplicitUse]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ExecutableAttribute(ExecutionOrder order, string stepTitle): Attribute
    {
        public ExecutionOrder ExecutionOrder { get; private set; } = order;
        public bool Asserts { get; set; }
        public string StepTitle { get; set; } = stepTitle;
        public int Order { get; set; }
        public bool ShouldReport { get; set; } = true;
    }
}