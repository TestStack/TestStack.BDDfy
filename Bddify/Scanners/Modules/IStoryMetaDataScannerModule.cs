using Bddify.Module;

namespace Bddify.Scanners.Modules
{
    public interface IStoryMetaDataScannerModule : IModule
    {
        IStoryMetaDataScanner GetMetaDataScanner(object testObject);
    }
}