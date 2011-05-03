using System.CodeDom.Compiler;

namespace Bddify.Scanners.GwtAttributes
{
    public class AndWhenAttribute : GwtExectuableAttribute
    {
        public AndWhenAttribute() : base(Core.ExecutionOrder.ConsecutiveTransition) { }
    }
}