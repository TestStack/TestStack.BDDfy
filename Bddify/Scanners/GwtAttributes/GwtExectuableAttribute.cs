using Bddify.Core;

namespace Bddify.Scanners.GwtAttributes
{
    public class GwtExectuableAttribute : ExecutableAttribute
    {
        public GwtExectuableAttribute(ExecutionOrder order) : base(order)
        {
        }
    }
}