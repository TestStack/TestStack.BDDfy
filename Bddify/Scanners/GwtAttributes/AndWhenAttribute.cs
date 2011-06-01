using System.CodeDom.Compiler;

namespace Bddify.Scanners.GwtAttributes
{
    public class AndWhenAttribute : ExecutableAttribute
    {
        public AndWhenAttribute() : base(Core.ExecutionOrder.ConsecutiveTransition) { }
    }
}