using Bddify.Core;

namespace Bddify.Module
{
    public class DefaultModule : IModule
    {
        public virtual bool RunsOn(Story story)
        {
            return true;
        }

        public virtual int Priority
        {
            get { return 100; }
        }
    }
}