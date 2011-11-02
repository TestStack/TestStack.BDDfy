using Bddify.Core;
using Bddify.Module;

namespace Bddify.Reporters
{
    public interface IReportModule : IModule
    {
        void Report(Story story);
    }
}