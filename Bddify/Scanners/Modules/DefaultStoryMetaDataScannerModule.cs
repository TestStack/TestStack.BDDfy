using Bddify.Module;

namespace Bddify.Scanners.Modules
{
    public class DefaultStoryMetaDataScannerModule : DefaultModule, IStoryMetaDataScannerModule
    {
        public IStoryMetaDataScanner GetMetaDataScanner(object testObject)
        {
            return new StoryAttributeMetaDataScanner();
        }
    }
}