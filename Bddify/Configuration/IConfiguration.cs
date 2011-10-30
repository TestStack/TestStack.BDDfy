using Bddify.Core;

namespace Bddify.Configuration
{
    public interface IConfiguration
    {
        bool Configures(Story story);
        int Priority { get; }
    }
}